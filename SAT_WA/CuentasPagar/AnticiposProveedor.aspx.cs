using SAT_CL.CXC;
using SAT_CL.CXP;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class AnticiposProveedor : System.Web.UI.Page
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
        /// Evento Producido al Cambiar las Opciones de Filtrado de Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbFacturas_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFacturas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar Fichas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            Controles.InicializaIndices(gvAnticiposProveedor);
            
            //Invocando Método de Busqueda
            buscaAnticipoProveedor();

            //Actualizando Totales
            actualizaTotales();
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
            SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatusFactura;

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
                    if (gvAnticiposProveedor.SelectedIndex != -1)
                    {
                        //Instanciando Ficha de Ingreso
                        using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
                        {
                            //Validando que existe el Registro
                            if (egreso.id_egreso_ingreso > 0)
                            {
                                //Obteniendo Estatus
                                estatus = egreso.estatus;

                                //Recorriendo Filas
                                foreach (GridViewRow gvr in gvrs)
                                {
                                    //Seleccionando Fila
                                    gvFacturas.SelectedIndex = gvr.RowIndex;

                                    //Validando que el Monto no sea 0
                                    if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) != 0.00M)
                                    {
                                        //Insertando Ficha de Ingreso Aplicada
                                        result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]),
                                                            Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0,
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (SAT_CL.CXP.FacturadoProveedor fac = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                            {
                                                //Validando que exista la Factura
                                                if (fac.id_factura > 0)
                                                {
                                                    //Calculando Estatus de la Factura
                                                    estatusFactura = (fac.total_factura - Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoAplicado"])) - Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) > 0 ? SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial : SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;

                                                    //Actualizando Estatus de la Factura
                                                    result = fac.ActualizaEstatusFacturadoProveedor(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación fuese Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Instanciando Relación
                                                        using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(fac.id_factura, egreso.id_tabla, egreso.id_registro))
                                                        {
                                                            //Si existe la Factura
                                                            if (fpr.habilitar)

                                                                //Instanciando Factura
                                                                result = new RetornoOperacion(fac.id_factura);
                                                            else
                                                            {
                                                                //Insertando Relación
                                                                result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(fac.id_factura, egreso.id_tabla, egreso.id_registro,
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando Resultado
                                                                if (result.OperacionExitosa)

                                                                    //Instanciando Factura
                                                                    result = new RetornoOperacion(fac.id_factura);
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //Validando que la Operación fuese Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Guardando Mensaje de la Operación
                                                result_msn[contador] = "* La Factura " + gvFacturas.SelectedDataKey["Id"].ToString() + ": Ha sido aplicada Exitosamente";

                                                //Incrementando Contador
                                                contador++;
                                            }
                                            else
                                            {
                                                //Creando Arreglo
                                                result_msn = new string[1];
                                                result_msn[1] = "Error en la Factura " + gvFacturas.SelectedDataKey["Id"].ToString() + ": " + result.Mensaje;

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

                                    //Validando que el Anticipo tiene saldo
                                    if (monto_aplicado == Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]))

                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Aplicada;
                                    else
                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;

                                    //Validando que la Operación fuese Exitosa
                                    if (result.OperacionExitosa)

                                        //Actualizando Estatus de el Anticipo de Ingreso
                                        result = egreso.ActualizaFichaIngresoEstatus(SAT_CL.Bancos.EgresoIngreso.TipoOperacion.Egreso, estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Anticipo del Proveedor");
                                //Mostrando Mensaje de Operación
                                lblErrorFactura.Text = result.Mensaje;
                            }
                        }
                    }
                    else
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion("No hay un Anticipo Seleccionado");
                        //Mostrando Mensaje de Operación
                        lblErrorFactura.Text = result.Mensaje;
                    }

                    //Validando que las Operaciones fuesen Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Invocando Método de Busqueda de Anticipo Proveedor
                        buscaAnticipoProveedor();

                        //Invocando Método de Busqueda de Facturas
                        buscaFacturas();

                        //Mostrando Mensaje de Operación
                        lblErrorFicha.Text = string.Format("* El Anticipo No. {0}: Ha sido actualizado", result.IdRegistro);
                        lblErrorFactura.Text = string.Join("<br/>", result_msn);

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(this, lblErrorFicha.Text, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Mostrando Saldos
                        actualizaTotales();

                        //Completando Transacción
                        trans.Complete();
                    }
                    else
                    {
                        //Instanciando Excepción
                        lblErrorFicha.Text = result.Mensaje;

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
            else
                //Mostrando Mensaje de Operación
                lblErrorFactura.Text = "No hay Facturas Seleccionadas";
        }

        #region Eventos GridView "Anticipo Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 7);

            //Inicializando Indices
            Controles.InicializaIndices(gvAnticiposProveedor);

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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Invocando Método de Suma
                sumaTotalesFacturas();

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Invocando Método de Suma
            sumaTotalAnticiposProveedor();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            Controles.CambiaSortExpressionGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 7);

            //Inicializando Indices
            Controles.InicializaIndices(gvAnticiposProveedor);

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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Invocando Método de Suma
            sumaTotalAnticiposProveedor();

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 7);

            //Inicializando Indices
            Controles.InicializaIndices(gvAnticiposProveedor);

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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }

            //Invocando Método de Suma
            sumaTotalAnticiposProveedor();

            //Invocando Método de Suma
            sumaTotalesFacturas();

            //Mostrando Saldos
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros 
            if (gvAnticiposProveedor.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAnticiposProveedor, sender, "lnk", false);

                //Mostrando el Saldos
                actualizaTotales();

                //Buscando Facturas
                buscaFacturas();

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
                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
        /// Evento Producido al Seleccionar el Monto Aplicado del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerFacturasAplicadas_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvAnticiposProveedor.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAnticiposProveedor, sender, "lnk", false);

                //Invocando Método de Busqueda
                buscaFichasAplicadas(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]), 0);

                //Mostrando Ventana
                gestionaVentanas(gvAnticiposProveedor, "FacturasLigadas");
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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacturas.SelectedValue));

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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Cambiando Expresión del Ordenamiento
                Controles.CambiaSortExpressionGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);

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
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);

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
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;

                //Validando que exista una Ficha Seleccionada
                if (gvAnticiposProveedor.SelectedIndex != -1)
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
                                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Rows)

                                        //Editando el Registro
                                        dr["MontoPorAplicar"] = "0.00";

                                    //Aceptando Cambios
                                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();

                                    //Cargando GridView
                                    Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
                switch (((LinkButton)sender).CommandName)
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
                                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
                if (gvAnticiposProveedor.SelectedIndex != -1)
                {
                    //Instanciando Ficha de Ingreso
                    using (SAT_CL.Bancos.EgresoIngreso ficha_ingreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
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
                            if (Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]) >= Convert.ToDecimal(gvFacturas.SelectedDataKey["SaldoFactura"]))
                            {
                                //Validando que 
                                if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) > 0.00M)

                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);
                                else
                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["SaldoFactura"]);
                            }
                            else
                            {
                                //Validando que el Monto por Aplicar sea Menor que el Monto Disponible
                                if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) <= Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]))
                                {
                                    //Validando que 
                                    if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]) > 0.00M)

                                        //Actualizando Registros
                                        monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPorAplicar"]);
                                    else
                                        //Actualizando Registros
                                        monto_x_aplicar = Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]);
                                }
                                else
                                    //Actualizando Registros
                                    monto_x_aplicar = Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]);
                            }

                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Insertando Ficha de Ingreso Aplicada
                                result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(9, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"]),
                                                    monto_x_aplicar, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0,
                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Validando que el Anticipo tiene saldo
                                    if (monto_x_aplicar == Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]))

                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Aplicada;
                                    else
                                        //Asignando Estatus
                                        estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;

                                    //Actualizando Estatus del Anticipo
                                    result = ficha_ingreso.ActualizaFichaIngresoEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando que la Operación fuese Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Mensaje de Operación
                                        result = new RetornoOperacion(string.Format("El Anticipo {0}: ha sido Actualizada", gvAnticiposProveedor.SelectedDataKey["Id"]));

                                        //Mostrando Mensaje de Operación
                                        lblErrorFicha.Text = result.Mensaje;

                                        //Instanciando Factura
                                        using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                                        {
                                            //Validando que exista la Factura
                                            if (fac.id_factura > 0)
                                            {
                                                //Calculando Estatus de la Factura
                                                estatusFactura = Convert.ToDecimal(gvFacturas.SelectedDataKey["SaldoFactura"]) - monto_x_aplicar > 0 ? SAT_CL.Facturacion.Facturado.EstatusFactura.AplicadaParcial : SAT_CL.Facturacion.Facturado.EstatusFactura.Liquidada;

                                                //Actualizando Estatus de la Factura
                                                result = fac.ActualizaEstatusFactura(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación Fuese Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Instanciando Mensaje de Operación
                                                    result = new RetornoOperacion(string.Format("La Factura {0}: ha sido Aplicada Exitosamente", fac.id_factura));

                                                    //Invocando Método de Busqueda de Anticipo Proveedor
                                                    buscaAnticipoProveedor();

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

                //Invocando Método de Busqueda
                buscaAplicacionesRelacion(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]));

                //Mostrando Ventana
                gestionaVentanas(gvFacturas, "FichasFacturas");
            }
        }

        #endregion

        #region Eventos GridView "Facturas Ligadas"

        /// <summary>
        /// Evento Producido al Cerrar la(s) Ventana(s) Modal(es)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Cerrando ventana modal 
            gestionaVentanas(lnk, lnk.CommandName);

            //Validando Comando
            switch (lnk.CommandName)
            {
                case "FacturasLigadas":
                    {
                        //Inicializando Indices
                        Controles.InicializaIndices(gvAnticiposProveedor);
                        break;
                    }
                case "FichasFacturas":
                    {
                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturas);
                        break;
                    }
            }
            
            //Limpiando Controles
            lblErrorFactura.Text =
            lblErrorFicha.Text = "";

            //Actualizando Totales
            actualizaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 2);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = "0.00";
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFL.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 2);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = "0.00";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFL.SelectedValue), true, 2);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[4].Text = "0.00";
        }
        /*// <summary>
        /// Evento Producido al Eliminar la Aplicación del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarAplicacion_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);
                SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatusFactura;

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Declarando Variable Auxiliar
                SAT_CL.Bancos.EgresoIngreso.Estatus estatus;
                int id_ficha_ingreso = 0;
                int id_factura = 0;

                //Instanciando Aplicación de el Anticipo
                using (SAT_CL.CXC.FichaIngresoAplicacion fia = new FichaIngresoAplicacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if (fia.id_ficha_ingreso_aplicacion > 0)
                    {
                        //Inicializando Bloque Transaccional
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Deshabilitando Aplicación de el Anticipo
                            result = fia.DeshabilitarFichaIngresoAplicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Instanciando el Anticipo de Ingreso
                                using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                {
                                    //Validando que exista el Anticipo
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
                                            estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Depositado;

                                        //Actualizando Ficha de Ingreso
                                        result = ei.ActualizaFichaIngresoEstatus(SAT_CL.Bancos.EgresoIngreso.TipoOperacion.Egreso, estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (SAT_CL.CXP.FacturadoProveedor fac = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(fia.id_registro)))
                                            {
                                                //Validando que exista la Factura
                                                if (fac.id_factura > 0)
                                                {
                                                    //Calculando Estatus de la Factura
                                                    estatusFactura = SAT_CL.CXP.FacturadoProveedor.ObtieneMontoPendienteAplicacion(fia.id_registro) == fac.total_factura ? SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Aceptada : SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial;

                                                    //Actualizando Estatus de la Factura
                                                    result = fac.ActualizaEstatusFacturadoProveedor(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

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
                        result = new RetornoOperacion("No se puede Acceder al Anticipo");

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
                                        result = new RetornoOperacion(string.Format("El Anticipo {0}: Ha sido Desaplicado por el monto de {1:C2}", ei.secuencia_compania, fia.monto_aplicado));
                                    }

                                    break;
                                }
                        }

                        //Invocando Métodos de Busqueda
                        buscaFichasAplicadas(id_ficha_ingreso, id_factura);
                        buscaAnticipoProveedor();
                        buscaFacturas();

                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturasLigadas);
                    }

                    //Mostrando Mensaje de Error
                    lblErrorFF.Text = result.Mensaje;
                }
            }
        }//*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFL_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }
        

        #endregion

        #region Eventos GridView "Fichas Facturas"

        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Instanciando Control
                using (Label lbl = (Label)e.Row.FindControl("lblServiciosEntidad"))
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lnkServiciosEntidad"))
                {
                    //Validando que existan los Controles
                    if (lbl != null && lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["IdEntidad"].ToString())
                        {
                            case "51":
                                {
                                    //Configurando Controles
                                    lbl.Visible = true;
                                    lkb.Visible = false;
                                    break;
                                }
                            case "82":
                                {
                                    //Configurando Controles
                                    lbl.Visible = false;
                                    lkb.Visible = true;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, true, 4);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 4);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFF_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoFF.SelectedValue), true, 4);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkServiciosEntidad_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFichasFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasFacturas, sender, "lnk", false);

                //Obteniendo Servicios
                using (DataTable dtServiciosEntidad = SAT_CL.Bancos.EgresoIngreso.ObtieneServiciosEntidad(
                                                        Convert.ToInt32(gvFichasFacturas.SelectedDataKey["IdEntidad"]),
                                                        Convert.ToInt32(gvFichasFacturas.SelectedDataKey["IdRegistro"])))
                {
                    //Validando que existan Registros
                    if (Validacion.ValidaOrigenDatos(dtServiciosEntidad))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvServiciosEntidad, dtServiciosEntidad, "NoServicio", lblOrdenadoSE.Text);

                        //Añadiendo Tabla a Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosEntidad, "Table3");
                    }
                    else
                    {
                        //Inicilaizando GridView
                        Controles.InicializaGridview(gvServiciosEntidad);

                        //Eliminando Tabla de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                    }

                    //Abriend Ventana
                    gestionaVentanas(this.Page, "ServiciosEntidad");
                }
            }
        }

        #endregion

        #region Eventos GridView "Servicios Entidad"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosEntidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosEntidad_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoSE.Text = Controles.CambiaSortExpressionGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoSE.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarSE_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"));
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
            Controles.InicializaGridview(gvAnticiposProveedor);
            Controles.InicializaGridview(gvFacturas);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFI, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturas, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSE, "", 26);
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
                case "FacturasLigadas":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "FacturasLigadas", "contenidoVentanaFacturasLigadas", "ventanaFacturasLigadas");
                    break;
                case "FichasFacturas":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "FichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
                    break;
                case "ServiciosEntidad":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "ServiciosEntidad", "contenidoVentanaServiciosEntidad", "ventanaServiciosEntidad");
                    //Inicializando Indices
                    Controles.InicializaIndices(gvFichasFacturas);
                    upgvFichasFacturas.Update();
                    break;
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Anticipo Proveedor
        /// </summary>
        private void buscaAnticipoProveedor()
        {
            //Obteniendo Valor
            using (DataTable dtAnticiosProveedor = SAT_CL.Bancos.EgresoIngreso.ObtieneAnticiposYNotasCreditoProveedor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                                                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1))))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtAnticiosProveedor))
                {
                    //Validando que exista una Ficha Seleccionada
                    if (gvAnticiposProveedor.SelectedIndex != -1)

                        //Marcando Fila
                        TSDK.ASP.Controles.MarcaFila(gvAnticiposProveedor, gvAnticiposProveedor.SelectedDataKey["Id"].ToString(), "Id", "Id-MontoDisponible-IdServicio", dtAnticiosProveedor, lblOrdenadoFI.Text, Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 7);
                    else
                        //Cargando GridView
                        Controles.CargaGridView(gvAnticiposProveedor, dtAnticiosProveedor, "Id-MontoDisponible-IdEntidad-IdRegistro-IdServicio", lblOrdenadoFI.Text, true, 7);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAnticiosProveedor, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAnticiposProveedor);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }

            //Invocando Método de Suma
            sumaTotalAnticiposProveedor();
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {
            //Obteniendo Servicio
            int idEntidad = 0, idRegistro = 0;

            //Validando que esten solo las facturas de Servicio
            if (rbFacturas.Checked)
            {
                //Validando que exista la Selección
                if (gvAnticiposProveedor.SelectedIndex != -1)
                {
                    //Asignando Valores
                    idEntidad = Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["IdEntidad"]);
                    idRegistro = Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["IdRegistro"]);
                }
            }

            //Obteniendo Valor
            using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneFacturasAnticipoProveedor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)), txtSerie.Text, txtUUID.Text, Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolio.Text, "0")),
                        idEntidad, idRegistro))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturas, dtFacturas, "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
        /// Método encargado de Sumar los Totales de los Anticipos
        /// </summary>
        private void sumaTotalAnticiposProveedor()
        {
            //Validando que exista el Origend e Datos
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Sumando Totales
                gvAnticiposProveedor.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
                gvAnticiposProveedor.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
                gvAnticiposProveedor.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoDisponible)", "")));
            }
            else
            {
                //Sumando Totales
                gvAnticiposProveedor.FooterRow.Cells[9].Text =
                gvAnticiposProveedor.FooterRow.Cells[10].Text =
                gvAnticiposProveedor.FooterRow.Cells[11].Text = string.Format("{0:C2}", 0);
            }
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
                gvFacturas.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(SubTotal)", "")));
                gvFacturas.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Trasladado)", "")));
                gvFacturas.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Retenido)", "")));
                gvFacturas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoTotal)", "")));
                gvFacturas.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
                gvFacturas.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPendiente)", "")));
                gvFacturas.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(SaldoFactura)", "")));
                gvFacturas.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", "")));
            }
            else
            {
                //Sumando Totales
                gvFacturas.FooterRow.Cells[9].Text =
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
                //Validando que exista el Anticipo
                using (SAT_CL.Bancos.EgresoIngreso ficha = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvAnticiposProveedor.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Anticipo
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
                        monto_disponible = (Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]) - monto_sumatoria) < 0 ? 0 : (Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]) - monto_sumatoria);

                        //Obteniendo Resultado del Indicador
                        indicador = monto_disponible > 0 ? true : false;

                        //Obteniendo Fila por Editar
                        DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                        //Recorriendo Registro Encontrado
                        foreach (DataRow dr in drEdit)
                        {
                            //Validando que el Monto Disponible sea Mayor o Igual al Monto Pendiente
                            if (monto_disponible >= Convert.ToDecimal(gvFacturas.SelectedDataKey["SaldoFactura"]))
                            {
                                //Actualizando Registros
                                dr["MontoPorAplicar"] = string.Format("{0:0.00}", gvFacturas.SelectedDataKey["SaldoFactura"]);
                                dr["MPA2"] = string.Format("{0:0.00}", gvFacturas.SelectedDataKey["SaldoFactura"]);
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
            Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table1"], "Id-SaldoFactura-MontoPorAplicar-MontoAplicado", "", true, 2);

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
            using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneAnticiposFacturasProveedor(id_ficha_ingreso, id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasLigadas, dtFichasFacturas, "Id", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table2");

                    //Mostrando Totales
                    gvFacturasLigadas.FooterRow.Cells[4].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasLigadas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                    //Mostrando Totales
                    gvFacturasLigadas.FooterRow.Cells[4].Text = "0.00";
                }
            }
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales
        /// </summary>
        private void actualizaTotales()
        {
            //Validando que una Ficha de Ingreso este Seleccionada
            if (gvAnticiposProveedor.SelectedIndex != -1)
            {
                //Asignando Saldo Disponible de el Anticipo
                lblSaldoFI.Text = string.Format("{0:C2}", gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]);

                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                {
                    //Mostrando el Saldo por Aplicar
                    lblPorAplicar.Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", "")));
                    lblSaldoFinal.Text = string.Format("{0:C2}", Convert.ToDecimal(gvAnticiposProveedor.SelectedDataKey["MontoDisponible"]) - Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoPorAplicar)", ""))));
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

        #region Métodos "Relacion y Aplicación de Facturas"

        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_factura"></param>
        private void buscaAplicacionesRelacion(int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXP.FacturadoProveedor.ObtieneAplicacionesRelacionFacturasProveedor(id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id-IdEntidad-IdRegistro", "", true, 4);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
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
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = "0.00";
        }

        #endregion        

        #endregion
    }
}