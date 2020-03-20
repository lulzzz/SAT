using SAT_CL;
using SAT_CL.CXP;
using SAT_CL.EgresoServicio;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.EgresoServicio
{
    public partial class DepositosPendientes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento generado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recraga de página
            if (!this.IsPostBack)
                //Inicializando la forma
                inicializaPagina();
        }
        /// <summary>
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Anticipo":
                    mtvDepositos.ActiveViewIndex = 0;
                    btnAnticipo.CssClass = "boton_pestana_activo";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnFacturasProveedor.CssClass = "boton_pestana";
                    btnEgresosVarios.CssClass = "boton_pestana";
                    //Inicializa los Valores de Depósito
                    inicializaValoresDepositos();
                    break;
                case "Liquidacion":
                    mtvDepositos.ActiveViewIndex = 1;
                    btnAnticipo.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana_activo";
                    btnFacturasProveedor.CssClass = "boton_pestana";
                    btnEgresosVarios.CssClass = "boton_pestana";
                    //Inicializa los Valores de Liquidación
                    inicializaValoresLiquidacion();
                    break;
                case "Facturacion":
                    mtvDepositos.ActiveViewIndex = 2;
                    btnAnticipo.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnFacturasProveedor.CssClass = "boton_pestana_activo";
                    btnEgresosVarios.CssClass = "boton_pestana";
                    //Inicializa los Valores de Facturación
                    inicializaValoresFacturas();
                    break;
                case "EgresosVarios":
                    mtvDepositos.ActiveViewIndex = 3;
                    btnAnticipo.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnFacturasProveedor.CssClass = "boton_pestana";
                    btnEgresosVarios.CssClass = "boton_pestana_activo";
                    break;
            }
        }
        /// <summary>
        /// Evento producido al pulsar algún botón de exportación de registros a Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {
            //Definiendo origen de datos en blanco
            DataTable mit = null;

            //Determinando el comando del botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "DepositosPendientes":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
                    break;
                case "UltimosDepositos":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1");
                    break;
                case "LiquidacionesPendientes":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2");
                    break;
                case "FacturasPendientes":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3");
                    break;
                case "UltimasFacturas":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4");
                    break;
                case "FacturasLigadas":
                    //Recuperando Tabla correspondiente a controles
                    mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5");
                    break;
            }

            //Exportando contenido del origen de datos
            Controles.ExportaContenidoGridView(mit, "*".ToCharArray());
        }
        /// <summary>
        /// Evento generado al cambiar de Banco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBanco_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga catalogo Cuentas 
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuenta, 23, "-Seleccione la Cuenta-", Convert.ToInt32(ddlBanco.SelectedValue), "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }

        /// <summary>
        /// Validamos Exista Depósitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAltaCuentaDepositos_Click(object sender, EventArgs e)
        {
            //Validamos Exista Depósitos
            if (gvDepositosPendientes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvDepositosPendientes, sender, "lnk", false);

                //Mostramos Modal
                alternaVentanaModal("cuentaBancos", (LinkButton)sender);

                //Inicializamos Control
                wucCuentaBancos.InicializaControl(Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdTabla"]), Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdRegistro"]));
            }
        }

        /// <summary>
        /// Alta de Cuenta de Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAltaCuentaFacturas_Click(object sender, EventArgs e)
        {
            //Validamos Exista Depósitos
            if (gvFacturasPendientes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvFacturasPendientes, sender, "lnk", false);

                //Mostramos Modal
                alternaVentanaModal("cuentaBancos", (LinkButton)sender);

                //Inicializamos Control
                wucCuentaBancos.InicializaControl(Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["IdTabla"]), Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["IdRegistro"]));
            }
        }

        /// <summary>
        /// Alta de Cuenta de Liquidacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAltaCuentaLiquidacion_Click(object sender, EventArgs e)
        {
            //Validamos Exista Depósitos
            if (gvLiquidacionesPendientes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvLiquidacionesPendientes, sender, "lnk", false);

                //Mostramos Modal
                alternaVentanaModal("cuentaBancos", (LinkButton)sender);

                //Inicializamos Control
                wucCuentaBancos.InicializaControl(Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdTabla"]), Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdRegistro"]));
            }
        }

        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //De Acuerdo a la Pestaña Activa
            //Depósitos
            if (mtvDepositos.ActiveViewIndex == 0)
            {
                //Cargamos Depósitos
                cargaDepositosPendientes();
            }
            //Liquidaciones
            else if (mtvDepositos.ActiveViewIndex == 1)
            {
                //Cargamos Liquidaciones
                cargaLiquidacionesPendientes();
            }
            //Liquidaciones
            else if (mtvDepositos.ActiveViewIndex == 2)
            {
                //Cargamos Facturas
                cargaFacturasPendientes();
            }


            //Mostramos Modal
            alternaVentanaModal("cuentaBancos", (LinkButton)sender);
        }
        /// <summary>
        /// Cerrando Ventana de Anticipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarDepositoFac_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Mostramos Modal
            alternaVentanaModal(lkb.CommandName, (LinkButton)sender);

            //Inicializando Indices
            Controles.InicializaIndices(gvDepositosPendientes);
            Controles.InicializaIndices(gvLiquidacionesPendientes);

            //Actualizando Controles
            upgvDepositosPendientes.Update();
            upgvLiquidacionesPendientes.Update();
        }

        #region Eventos Depositos

        /// <summary>
        /// Evento Generado al Dar clic en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscaDepositosRealizados();
        }

        #region Eventos GridView Depositos

        /// <summary>
        /// Evento producido al presionar el checkbox "DepositosTodos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDepositosTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvDepositosPendientes.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;

                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvDepositosPendientes.FooterRow.FindControl("lblContadorDepositos"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvDepositosPendientes, "chkSeleccionDeposito", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento producido al presionar el cada checkbox de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionDeposito_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvDepositosPendientes.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvDepositosPendientes, "lblContadorDepositos");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvDepositosPendientes.HeaderRow.FindControl("chkDepositosTodos");
                    //deshabilitando seleccion
                    t.Checked = d.Checked;
                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño del GridView Depósitos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvDepositosPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoGrid.SelectedValue));
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Depósitos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvDepositosPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Depósitos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositosPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridDepositosPendientes.Text = Controles.CambiaSortExpressionGridView(gvDepositosPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView Depósitos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositosPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Instanciando Control
                using (CheckBox chk = (CheckBox)e.Row.FindControl("chkSeleccionDeposito"))
                using (Label lbl = (Label)e.Row.FindControl("lblSeleccionDeposito"))
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lkbDepositarAnticipo"))
                {
                    //Validando que existan los Controles
                    if (chk != null && lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["Concepto"].ToString())
                        {
                            case "Anticipo Proveedor":
                            case "Finiquito Proveedor":
                                {
                                    //Configurando Controles
                                    chk.Visible = false;
                                    lbl.Visible = false;
                                    lkb.Visible = true;
                                    break;
                                }
                            default:
                                {
                                    //Configurando Controles
                                    chk.Visible = true;
                                    lbl.Visible = true;
                                    lkb.Visible = false;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento producido al dar click sobre algún botón de registro Depósitos Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDepositos_Click(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;

            //Si existen registros
            if (gvDepositosPendientes.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvDepositosPendientes, sender, "lnk", false);

                //En base al comando definido para el botón
                switch (b.CommandName)
                {
                    case "Referencias":
                        {
                            inicializaReferencias(Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["Id"]), 51, "Referencias Depósito");
                            break;
                        }
                    case "Bitacora":
                        {
                            inicializaBitacoraRegistro(gvDepositosPendientes.SelectedDataKey["Id"].ToString(), "51", "Bitacorá Depósito");
                            break;
                        }
                    case "AnticipoFactura":
                        {
                            //Validando que exista la Cuenta
                            if (Convert.ToInt32(ddlCuenta.SelectedValue) > 0)
                            {
                                //Gestionando Comando para Anticipos
                                btnDepositarDepFac.CommandName = "Anticipo";
                                btnRechazarDepFac.CommandName = "RechazoAnticipo";

                                //Asignando Fecha
                                txtFechaDepFac.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                                txtReferenciaDepFac.Text = "";

                                //Mostrando Ventana de Deposito
                                alternaVentanaModal("AnticipoFactura", b);
                            }
                            else
                            {   
                                //Mostrando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Seleccione la Cuenta"), ScriptServer.PosicionNotificacion.AbajoDerecha);

                                //Inicializando Indices
                                Controles.InicializaIndices(gvDepositosPendientes);
                            }
                            break;
                        }
                }
            }
        }

        #endregion

        /// <summary>
        /// Evento producido al presionar el boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnBuscarOperador_Click(object sender, EventArgs e)
        {
            //Ejecución del metodo que realiza la busqueda
            buscaDepositosRealizados();
        }

        #region GridView Ultimos Depositos

        /// <summary>
        /// Evento producido al cambiar el tamaño del GridView Últimos Depósitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGridViewUltimosDepositos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvUltimosDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoGridViewUltimosDepositos.SelectedValue));
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Últimos Depósitos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUltimosDepositos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvUltimosDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Últimos Depósitos
        /// </summary>    
        protected void gvUltimosDepositos_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewUltimosDepositos.Text = Controles.CambiaSortExpressionGridView(gvUltimosDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }

        #endregion

        /// <summary>
        /// Evento producido al pulsar algún botón de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Boton_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue el que produjo el evento
            switch (((Button)sender).CommandName)
            {
                case "Actualizar":
                    //Actualizando los depósitos pendientes
                    cargaDepositosPendientes();
                    break;
                case "Depositar":
                    //Validamos Selección de Cuenta de origen para la transferencia
                    if (ddlCuenta.SelectedValue != "0")
                    {
                        //Validamos Fecha del Deposito
                        if (Convert.ToDateTime(txtFechaDeposito.Text) <= Fecha.ObtieneFechaEstandarMexicoCentro())
                            //Deposita un pendiente
                            actualizaDepositos(btnDepositar, txtReferencia.Text, Convert.ToDateTime(txtFechaDeposito.Text), Convert.ToInt32(ddlFormaPagoDeposito.SelectedValue), SAT_CL.EgresoServicio.Deposito.EstatusDeposito.PorLiquidar);
                        else
                            ScriptServer.MuestraNotificacion(btnDepositar, "La Fecha del Depósito debe ser menor o igual a la Fecha Actual", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        ScriptServer.MuestraNotificacion(btnDepositar, "Seleccione la Cuenta Bancaria", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    break;
                case "Rechazar":
                    //Realizando asignaciones solicitadas
                    actualizaDepositos(btnRechazar, txtReferencia.Text, DateTime.MinValue, 0, SAT_CL.EgresoServicio.Deposito.EstatusDeposito.Registrado);
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Pulsar algun Boton de Depositos de Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BotonDepFac_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue el que produjo el evento
            switch (((Button)sender).CommandName)
            {
                case "Anticipo":
                    {
                        //Validamos Selección de Cuenta de origen para la transferencia
                        if (ddlCuenta.SelectedValue != "0")
                        {
                            //Validamos Fecha del Deposito
                            if (Convert.ToDateTime(txtFechaDepFac.Text) <= Fecha.ObtieneFechaEstandarMexicoCentro())
                                //Deposita un pendiente
                                actualizaEgresoFactura(btnDepositarDepFac, "Deposito");
                            else
                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(btnDepositarDepFac, "La Fecha del Depósito debe ser menor o igual a la Fecha Actual", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        else
                            ScriptServer.MuestraNotificacion(btnDepositarDepFac, "Seleccione la Cuenta Bancaria", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "RechazoAnticipo":
                    {
                        //Realizando asignaciones solicitadas
                        actualizaDepositos(btnRechazarDepFac, txtReferenciaDepFac.Text, DateTime.MinValue, 0, SAT_CL.EgresoServicio.Deposito.EstatusDeposito.Registrado);
                        //Ocultando Ventana
                        alternaVentanaModal("AnticipoFactura", btnRechazarDepFac);
                        break;
                    }
                case "Liquidacion":
                    {
                        //Validamos Selección de Cuenta de origen para la transferencia
                        if (ddlCuenta.SelectedValue != "0")
                        {
                            //Validamos Fecha del Deposito
                            if (Convert.ToDateTime(txtFechaDepFac.Text) <= Fecha.ObtieneFechaEstandarMexicoCentro())
                                //Deposita un pendiente
                                actualizaEgresoFactura(btnDepositarDepFac, "Liquidacion");
                            else
                                ScriptServer.MuestraNotificacion(btnDepositarDepFac, "La Fecha del Depósito debe ser menor o igual a la Fecha Actual", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        else
                            ScriptServer.MuestraNotificacion(btnDepositarDepFac, "Seleccione la Cuenta Bancaria", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "RechazoLiquidacion":
                    {
                        //Realizando asignaciones solicitadas
                        actualizaLiquidaciones(btnRechazarDepFac, txtReferenciaDepFac.Text, DateTime.MinValue, 0, SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado);
                        //Ocultando Ventana
                        alternaVentanaModal("AnticipoFactura", btnRechazarDepFac);
                        break;
                    }
            }
        }

        #endregion

        #region Eventos Facturas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFacturasPorPagar_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidaciones
            using (DataTable dtFacturasPagadas = SAT_CL.CXP.Reportes.ObtieneFacturasPagadas(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedorFacturas.Text, "ID:", 1))))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturasPagadas))
                {
                    //Cargamos el grid view
                    Controles.CargaGridView(gvUltimasFacturas, dtFacturasPagadas, "", lblOrdenadoLiquidacion.Text, true, 0);

                    //Guardando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasPagadas, "Table4");
                }
                else
                {
                    //Inicializamos el grid view
                    Controles.InicializaGridview(gvUltimasFacturas);

                    //Eliminando origen de datos en sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                }
            }
        }

        #region Eventos GridView "Facturas Por Pagar"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoFacturas.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturasPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Mostrando Ordenamiento del GridView
            lblOrdenadoFacturas.Text = Controles.CambiaSortExpressionGridView(gvFacturasPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFacturasTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvFacturasPendientes.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;

                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvFacturasPendientes.FooterRow.FindControl("lblContadorFacturas"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvFacturasPendientes, "chkSeleccionFactura", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionFactura_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvFacturasPendientes.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvFacturasPendientes, "lblContadorFacturas");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvFacturasPendientes.HeaderRow.FindControl("chkFacturasTodos");

                    //Validando que exista el control
                    if (t != null)
                        //deshabilitando seleccion
                        t.Checked = d.Checked;
                }
            }
        }

        #endregion

        #region Eventos GridView Utimas Facturas

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUltimasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvUltimasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUltimasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Mostrando Ordenamiento del GridView
            lblOrdenadoUltimasFacturas.Text = Controles.CambiaSortExpressionGridView(gvUltimasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvUltimasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoUltimasFacturas.SelectedValue));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BotonFacturacion_Click(object sender, EventArgs e)
        {
            //Instanciando Boton
            Button btn = (Button)sender;

            //Validando Comando
            switch (btn.CommandName)
            {
                case "Depositar":
                    {
                        //Deposita un pendiente
                        actualizaFacturas(btnDepositarFactura, SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada);
                        break;
                    }
                case "Rechazar":
                    {
                        //Rechazando Facturas
                        rechazaFacturas(btnRechazarFactura);
                        break;
                    }
                case "Actualizar":
                    {
                        //Cargando Facturas
                        cargaFacturasPendientes();
                        break;
                    }
            }
        }

        #endregion

        #region Eventos Liquidaciones

        #region Eventos GridView "Liquidaciones Pendientes"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoLiquidacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvLiquidacionesPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoLiquidacion.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacionesPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvLiquidacionesPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacionesPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Mostrando Ordenamiento del GridView
            lblOrdenadoLiquidacion.Text = Controles.CambiaSortExpressionGridView(gvLiquidacionesPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// Evento producido al presionar el checkbox "DepositosTodos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkLiquidacionesTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvLiquidacionesPendientes.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;

                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvLiquidacionesPendientes.FooterRow.FindControl("lblContadorLiquidaciones"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvLiquidacionesPendientes, "chkSeleccionLiquidacion", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento producido al presionar el cada checkbox de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionLiquidacion_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvLiquidacionesPendientes.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvLiquidacionesPendientes, "lblContadorLiquidaciones");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvLiquidacionesPendientes.HeaderRow.FindControl("chkLiquidacionesTodos");
                    //deshabilitando seleccion
                    t.Checked = d.Checked;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView de Liquidaciones Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacionesPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Instanciando Control
                using (CheckBox chk = (CheckBox)e.Row.FindControl("chkSeleccionLiquidacion"))
                using (Label lbl = (Label)e.Row.FindControl("lblLiquidacion"))
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lkbCobroRecurrente"))
                using (LinkButton lnk = (LinkButton)e.Row.FindControl("lkbDepositarLiquidacion"))
                {
                    //Validando que existan los Controles
                    if (chk != null && lkb != null && lbl != null && lnk != null)
                    {
                        //Validando Pago en Contra
                        switch (row["PagoEnContra"].ToString())
                        {
                            case "No":
                                {
                                    //Ocultando Link de Cobro
                                    lkb.Visible = false;
                                    break;
                                }
                            case "Si":
                                {
                                    //Mostrando Control
                                    lkb.Visible = true;
                                    break;
                                }
                        }

                        //Validando Concepto de Egreso
                        switch (row["IdEgresoConcepto"].ToString())
                        {
                            case "18":
                                {
                                    //Configurando Controles
                                    chk.Visible = false;
                                    lbl.Visible = false;
                                    lnk.Visible = true;
                                    break;
                                }
                            default:
                                {
                                    //Configurando Controles
                                    chk.Visible = true;
                                    lbl.Visible = true;
                                    lnk.Visible = false;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCobroRecurrente_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvLiquidacionesPendientes.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvLiquidacionesPendientes, sender, "lnk", false);

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Liquidación
                    if (liq.habilitar)
                    {
                        //Validando que exista la Liquidación
                        if (liq.total_alcance <= 0)
                        {
                            //Obteniendo Entidad
                            int id_entidad = liq.id_unidad > 0 ? liq.id_unidad : liq.id_operador > 0 ? liq.id_operador : liq.id_proveedor;
                            byte id_tipo_entidad = (byte)(liq.id_unidad > 0 ? 1 : liq.id_operador > 0 ? 2 : 3);

                            //Inicializando Cobro Recurrente
                            wucCobroRecurrente.InicializaCobroRecurrente(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                        id_tipo_entidad, id_entidad, liq.total_alcance * -1, 82, liq.id_liquidacion);

                            //Mostrando Ventana
                            ScriptServer.AlternarVentana((LinkButton)sender, "Cobro Recurrente", "contenedorVentanaCobroRecurrente", "ventanaCobroRecurrente");
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion((LinkButton)sender, new RetornoOperacion("El Monto de la Liquidación no esta en Contra"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion((LinkButton)sender, new RetornoOperacion("No existe la Liquidación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDepositarLiquidacion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvLiquidacionesPendientes.DataKeys.Count > 0)
            {
                //Validando que exista la Cuenta
                if (Convert.ToInt32(ddlCuenta.SelectedValue) > 0)
                {
                    //Seleccionando Fila
                    Controles.SeleccionaFila(gvLiquidacionesPendientes, sender, "lnk", false);

                    //Gestionando Comando para Anticipos
                    btnDepositarDepFac.CommandName = "Liquidacion";
                    btnRechazarDepFac.CommandName = "RechazoLiquidacion";

                    //Asignando Fecha
                    txtFechaDepFac.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                    txtReferenciaDepFac.Text = "";

                    //Mostrando Ventana de Anticipo
                    alternaVentanaModal("AnticipoFactura", this.Page);
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Seleccione la Cuenta"), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Guardar el Cobro Recurrente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucCobroRecurrente_ClickGuardarCobroRecurrente(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Cobro Recurrente
            result = wucCobroRecurrente.GuardaCobroRecurrente();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Cargando Liquidaciones Pendientes
                cargaLiquidacionesPendientes();

                //Mostrando Ventana
                ScriptServer.AlternarVentana(this.Page, "Cobro Recurrente", "contenedorVentanaCobroRecurrente", "ventanaCobroRecurrente");
            }

            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar el Cobro Recurrente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucCobroRecurrente_ClickCancelarCobroRecurrente(object sender, EventArgs e)
        {
            //Mostrando Ventana
            ScriptServer.AlternarVentana(this.Page, "Cobro Recurrente", "contenedorVentanaCobroRecurrente", "ventanaCobroRecurrente");

            //Inicializando Indices
            Controles.InicializaIndices(gvLiquidacionesPendientes);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarLiquidaciones_Click(object sender, EventArgs e)
        {
            //Invoca Método de Busqueda
            buscaLiquidacionesRealizadas();
        }

        #region Eventos GridView "Liquidaciones Realizadas"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoLiquidaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvUltimasLiquidaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoLiquidaciones.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUltimasLiquidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvUltimasLiquidaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUltimasLiquidaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Mostrando Ordenamiento del GridView
            lblOrdenadoUltimasLiq.Text = Controles.CambiaSortExpressionGridView(gvUltimasLiquidaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BotonLiquidacion_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue el que produjo el evento
            switch (((Button)sender).CommandName)
            {
                case "Actualizar":
                    {
                        //Actualizando los depósitos pendientes
                        cargaLiquidacionesPendientes();
                        break;
                    }
                case "Depositar":
                    {
                        //Validamos Selección de Cuenta de origen para la transferencia
                        if (ddlCuenta.SelectedValue != "0")
                        {
                            //Obteniendo Fecha
                            DateTime fec_liq = DateTime.MinValue;
                            DateTime.TryParse(txtFechaLiquidacion.Text, out fec_liq);
                            
                            //Validamos Fecha del Deposito
                            if (fec_liq <= Fecha.ObtieneFechaEstandarMexicoCentro())
                                
                                //Deposita un pendiente
                                actualizaLiquidaciones(btnDepositarLiquidacion, txtReferenciaLiquidacion.Text, fec_liq, Convert.ToInt32(ddlFormaPagoLiquidacion.SelectedValue), SAT_CL.Liquidacion.Liquidacion.Estatus.Depositado);
                            else
                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(btnDepositarLiquidacion, "La Fecha del Depósito debe ser menor o igual a la Fecha Actual", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        else
                            //Mostrando resultado
                            ScriptServer.MuestraNotificacion(btnDepositarLiquidacion, "Seleccione la Cuenta Bancaria", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Rechazar":
                    {
                        //Realizando asignaciones solicitadas
                        actualizaLiquidaciones(btnRechazarLiquidacion, txtReferenciaLiquidacion.Text, DateTime.MinValue, 0, SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado);
                        break;
                    }
            }
        }

        #endregion

        #region Eventos Egresos Varios

        /// <summary>
        /// Evento disparado al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        //inicializaPagina();
                        inicializaSeccionEgresosVarios();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(101, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaFichaIngreso(lkbGuardar);
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        //inicializaPagina();
                        inicializaSeccionEgresosVarios();
                        break;
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (ei.id_egreso_ingreso != 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Deshabilitando Producto
                                result = ei.DeshabilitarEgresoIngreso(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    //inicializaPagina();
                                    inicializaSeccionEgresosVarios();
                                }

                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(lkbEliminar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
                case "Depositar":
                    {
                        //Instanciando Egreso
                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (ei.id_egreso_ingreso != 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Aplicando Egreso
                                result = ei.DepositaEgresosVarios(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = result.IdRegistro;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Lectura;
                                    //Inicializando Forma
                                    //inicializaPagina();
                                    inicializaSeccionEgresosVarios();
                                }
                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(lkbDepositar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "101", "Ficha Ingreso");
                        break;
                    }
                case "Referencias":
                    {
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "101", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    {
                        //Si hay un registro en sesión
                        if (Session["id_registro"].ToString() != "0")

                            //Invocando Método de Inicialización de Referencias
                            inicializaArchivosRegistro(Session["id_registro"].ToString(), "101", "20");
                        break;
                    }
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Método de Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configurando Controles
            configuraControlesMetodoPago();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Moneda"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            btnGuardar.Focus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlConcepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraConceptoProveedor(Convert.ToInt32(ddlConcepto.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Validando que el Método de Pago sea Efectivo
            if (ddlMetodoPago.SelectedValue == "9")

                //Invocando Método de Guardado
                guardaFichaIngreso(btnGuardar);

            //Validando que Exista una Cuenta de Origen
            else if (ddlCuentaOrigen.SelectedValue != "0")

                //Invocando Método de Guardado
                guardaFichaIngreso(btnGuardar);
            else
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnGuardar, "Debe Seleccionar un Cuenta de Origen", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:

                    //Asignando a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
                default:

                    //Asignando a Nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    break;
            }

            //Invocando Inicialización de Página
            inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMonto_TextChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            ddlMoneda.Focus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFechaEI_TextChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            txtMonto.Focus();
        }

        #endregion

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
            decimal monto_aplicado = 0.00M, monto_inicial = 0.00M;
            SAT_CL.Bancos.EgresoIngreso.Estatus estatus;
            SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatusFactura;
            int id_egreso = obtieneEgresoEntidad();
            SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT forma_pago = (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToInt32(ddlFormaPagoDepFac.SelectedValue);

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Egreso
                using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(id_egreso))
                {
                    //Validando que exista el Egreso
                    if (egreso.habilitar)
                    {
                        //Validando Comando
                        switch (btnDepositarDepFac.CommandName)
                        {
                            case "Anticipo":
                                {
                                    //Depositando Anticipo
                                    result = egreso.ActualizarEstatusEgresoDepositado(Convert.ToDateTime(txtFechaDepFac.Text),
                                                                                    Convert.ToInt32(ddlCuenta.SelectedValue),
                                                                                    Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdCuenta"]),
                                                                                    txtReferenciaDepFac.Text.ToUpper(), forma_pago,
                                                                                    ((Usuario)Session["usuario"]).id_usuario);
                                    break;
                                }
                            case "Liquidacion":
                                {
                                    //Depositando Liquidación
                                    result = egreso.ActualizarEstatusEgresoLiquidacionAceptada(Convert.ToDateTime(txtFechaDepFac.Text), Convert.ToInt32(ddlCuenta.SelectedValue), 
                                                                    Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdCuenta"]), txtReferenciaDepFac.Text.ToUpper(),
                                                                    forma_pago,((Usuario)Session["usuario"]).id_usuario);
                                    break;
                                }
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Actualizando Atributos
                            egreso.ActualizaEgresoIngreso();

                            //Obteniendo Monto Inicial
                            monto_inicial = egreso.ObtieneSaldoEgreso();

                            //Obteniendo Facturas Seleccionadas
                            GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturasLigadas, "chkVariosFactura");

                            //Validando que Existan Registros
                            if (gvrs.Length > 0)
                            {
                                //Creando Arreglo Dinamicamente
                                result_msn = new string[gvrs.Length];

                                //Obteniendo Estatus
                                estatus = egreso.estatus;

                                //Recorriendo Filas
                                foreach (GridViewRow gvr in gvrs)
                                {
                                    //Seleccionando Fila
                                    gvFacturasLigadas.SelectedIndex = gvr.RowIndex;

                                    //Validando que el Monto no sea 0
                                    if (Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["MontoPorAplicar"]) != 0.00M)
                                    {
                                        //Insertando Ficha de Ingreso Aplicada
                                        result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"]), egreso.id_egreso_ingreso,
                                                            Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["MontoPorAplicar"]), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), false, 0, false, 0,
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Exitosa
                                        if (result.OperacionExitosa)
                                        {
                                            //Instanciando Factura
                                            using (SAT_CL.CXP.FacturadoProveedor fac = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"])))
                                            {
                                                //Validando que exista la Factura
                                                if (fac.id_factura > 0)
                                                {
                                                    //Validando el Estatus de la Factura
                                                    if ((SAT_CL.CXP.FacturadoProveedor.EstatusFactura)fac.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)
                                                    
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("La Factura no esta Aceptada");
                                                    else
                                                    {
                                                        //Validando Totales de Aplicación
                                                        if (((fac.total_factura - Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["AplicacionesConfirmadas"])) - Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["MontoPorAplicar"])) < 0)

                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("La Aplicación excede el Monto de la Factura");
                                                        else
                                                        {
                                                            //Calculando Estatus de la Factura
                                                            estatusFactura = (fac.total_factura - Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["AplicacionesConfirmadas"])) - Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["MontoPorAplicar"]) > 0 ? SAT_CL.CXP.FacturadoProveedor.EstatusFactura.AplicadaParcial : SAT_CL.CXP.FacturadoProveedor.EstatusFactura.Liquidada;

                                                            //Actualizando Estatus de la Factura
                                                            result = fac.ActualizaEstatusFacturadoProveedor(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        }
                                                    }
                                                }
                                            }

                                            //Validando que la Operación fuese Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Guardando Mensaje de la Operación
                                                result_msn[contador] = "* La Factura " + gvFacturasLigadas.SelectedDataKey["Id"].ToString() + ": Ha sido aplicada Exitosamente";

                                                //Incrementando Contador
                                                contador++;
                                            }
                                            else
                                            {
                                                //Creando Arreglo
                                                result_msn = new string[1];
                                                result_msn[1] = "Error en la Factura " + gvFacturasLigadas.SelectedDataKey["Id"].ToString() + ": " + result.Mensaje;

                                                //Terminando Ciclo
                                                break;
                                            }
                                        }
                                        else
                                            //Terminando Ciclo
                                            break;
                                    }
                                }

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Recorriendo Filas
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table5"].Rows)

                                        //Sumando Monto por Aplicar
                                        monto_aplicado = monto_aplicado + Convert.ToDecimal(dr["MontoPorAplicar"]);

                                    //Validando que el Anticipo tiene saldo
                                    if (monto_aplicado == monto_inicial)

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
                                //Mostrando Excepción
                                result = new RetornoOperacion("No hay Facturas Seleccionadas");
                        }

                    }
                    else
                        //Mostrando Excepción
                        result = new RetornoOperacion("No existe el Anticipo del Proveedor");

                    //Validando que las Operaciones fuesen Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Invocando Método de Busqueda de Egresos
                        cargaDepositosPendientes();
                        cargaLiquidacionesPendientes();

                        //Mostrando Mensaje de Operación
                        result = new RetornoOperacion(result.IdRegistro, string.Format("* El Egreso No. {0}: Ha sido actualizado", egreso.secuencia_compania), true);

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(btnAplicarFacEg, "ExitoEgreso", result.Mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(btnAplicarFacEg, "ExitoFacturas", string.Join("<br/>", result_msn), ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Ocultando Ventana Modal
                        alternaVentanaModal("FacturasLigadas", btnAplicarFacEg);

                        //Completando Transacción
                        trans.Complete();
                    }
                    else
                    {
                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(btnAplicarFacEg, new RetornoOperacion(result.IdRegistro, string.Format("* El Egreso No. {0}: No ha sido actualizado, {1}", egreso.secuencia_compania, result.Mensaje), true), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }

        #region Eventos Facturas Ligadas

        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Obteniendo Control
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lnkAceptarFacturaLigada"))
                {

                    CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosFactura");

                    //Validando que exista el Link
                    if (lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["Estatus"].ToString())
                        {
                            case "En Revisión (Aceptar)":
                                {
                                    //Habilitando Control
                                    lkb.Enabled = true;
                                    chk.Visible = false;
                                    break;
                                }
                            default:
                                {
                                    //Deshabilitando Control
                                    lkb.Enabled = false;
                                    chk.Visible = true;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento que actualiza el estatus de  una factura  a Aceptada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAceptarFacturaLigada_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Selecciona  una factura ligada a servicio 
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando Relación
                using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFPR"])))
                {
                    //Validando que exista la Relación
                    if (fpr.habilitar)
                    {
                        //Instanciando registro actual
                        using (FacturadoProveedor FP = new FacturadoProveedor(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"])))
                        {
                            //Si la Recepcion existe
                            if (FP.habilitar)
                            {
                                //Validando los Estatus donde se puede Aceptar
                                if ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                                    //Actualiza el estatus del registro
                                    resultado = FP.AceptaFacturaProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                {
                                    //Validando Estatus de la Factura
                                    switch ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura)
                                    {
                                        case FacturadoProveedor.EstatusFactura.Aceptada:
                                            {
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La Factura ya ha sido Aceptada.");
                                                break;
                                            }
                                        case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                        case FacturadoProveedor.EstatusFactura.Liquidada:
                                            {
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La Factura tiene Pagos Aplicados.");
                                                break;
                                            }
                                        case FacturadoProveedor.EstatusFactura.Refacturacion:
                                            {
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La Factura ha sido Refacturada.");
                                                break;
                                            }
                                        case FacturadoProveedor.EstatusFactura.Cancelada:
                                            {
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La Factura ha sido Cancelada.");
                                                break;
                                            }
                                        case FacturadoProveedor.EstatusFactura.Rechazada:
                                            {
                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("La Factura ha sido Rechazada.");
                                                break;
                                            }
                                    }
                                }
                            }
                        }

                        //Valida la acción de actualización
                        if (resultado.OperacionExitosa)

                            //Invoca al método cargaFacturasLigadas()
                            cargaFacturasLigadas(fpr.id_tabla, fpr.id_registro);
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No existe la relación");
                }

                //Envia un mensaje con el resultado de la operación. 
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), Convert.ToInt32(ddlTamanoFL.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Marcar un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFactura_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;

                //Obteniendo Egreso
                using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(obtieneEgresoEntidad()))
                {
                    //Validando que exista el Egreso
                    if (egreso.habilitar)
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
                                        foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table5"].Rows)

                                            //Editando el Registro
                                            dr["MontoPorAplicar"] = "0.00";

                                        //Aceptando Cambios
                                        ((DataSet)Session["DS"]).Tables["Table5"].AcceptChanges();

                                        //Cargando GridView
                                        Controles.CargaGridView(gvFacturasLigadas, ((DataSet)Session["DS"]).Tables["Table5"], "Id-IdFPR-SaldoActual-MontoPorAplicar-AplicacionesConfirmadas", "", true, 2);

                                        //Recorriendo Filas
                                        foreach (GridViewRow gvr in gvFacturasLigadas.Rows)
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
                                        foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table5"].Rows)

                                            //Editando el Registro
                                            dr["MontoPorAplicar"] = "0.00";

                                        //Aceptando Cambios
                                        ((DataSet)Session["DS"]).Tables["Table5"].AcceptChanges();

                                        //Cargando GridView
                                        Controles.CargaGridView(gvFacturasLigadas, ((DataSet)Session["DS"]).Tables["Table5"], "Id-IdFPR-SaldoActual-MontoPorAplicar-AplicacionesConfirmadas", "", true, 2);

                                        //Desmarcando todas las Filas
                                        Controles.SeleccionaFilasTodas(gvFacturasLigadas, "chkVariosFactura", chk.Checked);
                                    }

                                    //Obteniendo Control de Encabezado
                                    CheckBox chkEncabezado = (CheckBox)gvFacturasLigadas.HeaderRow.FindControl("chkTodosFactura");

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

                        //Actualizando Totales
                        actualizaTotales();
                    }
                    else
                    {
                        //Deshabilitando Control
                        chk.Checked = false;

                        //Mostrando Error
                        ScriptServer.MuestraNotificacion(chk, new RetornoOperacion("* Debe Seleccionar una Ficha de Ingreso"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
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
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Obteniendo Control
                LinkButton lnk = (LinkButton)sender;
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Obteniendo Control
                TextBox txt = (TextBox)gvFacturasLigadas.SelectedRow.FindControl("txtMXA");

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
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table5"].Select("Id = " + gvFacturasLigadas.SelectedDataKey["Id"].ToString()))
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
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(string.Format("La Cantidad excede el Monto de {0:0.00}", dr["MPA2"])), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }

                                    //Deshabilitando el Control
                                    txt.Enabled = false;

                                    //Configurando Control
                                    lnk.Text = lnk.CommandName = "Cambiar";
                                }

                                //Actualizando Cambios
                                ((DataSet)Session["DS"]).Tables["Table5"].AcceptChanges();

                                //Obtenemos Filas Seleccionadas
                                GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturasLigadas, "chkVariosFactura");

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
                                Controles.CargaGridView(gvFacturasLigadas, ((DataSet)Session["DS"]).Tables["Table5"], "Id-IdFPR-SaldoActual-MontoPorAplicar-AplicacionesConfirmadas", "", true, 2);

                                //Validando que Existan Indices
                                if (indices_chk.Length > 0)
                                {
                                    //Creando Ciclo
                                    foreach (int indice in indices_chk)
                                    {
                                        //Seleccionando Indice
                                        gvFacturasLigadas.SelectedIndex = indice;

                                        //Obteniendo Control
                                        CheckBox chkFila = (CheckBox)gvFacturasLigadas.SelectedRow.FindControl("chkVariosFactura");

                                        //Validando que exista el Control
                                        if (chkFila != null)
                                        {
                                            //Marcando Control
                                            chkFila.Checked = true;

                                            //Obteniendo Control
                                            LinkButton lkb = (LinkButton)gvFacturasLigadas.SelectedRow.FindControl("lnkCambiar");

                                            //Validando que exista el Control
                                            if (lkb != null)

                                                //Habilitando Control
                                                lkb.Enabled = true;
                                        }
                                    }
                                }

                                //Inicializando INdices
                                Controles.InicializaIndices(gvFacturasLigadas);
                            }
                            break;
                        }
                }

                //Actualizando Totales
                actualizaTotales();
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo encargado de inicializar la pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Cargamos los valores predefinidos para cada campo
            cargaCatalogos();
            //Cargando contenido de controles GridView
            cargaDepositosPendientes();
            //Cargando Liquidaciones
            cargaLiquidacionesPendientes();
            //Cargando Facturas
            cargaFacturasPendientes();
            //Inicializando Sección de Egresos Varios
            inicializaSeccionEgresosVarios();

            //Validando si Existe Sesión
            if (Convert.ToInt32(Session["id_registro"]) != 0 && (Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Nuevo)
            {
                //Configurando Egresos Varios
                mtvDepositos.ActiveViewIndex = 3;
                btnAnticipo.CssClass = "boton_pestana";
                btnLiquidacion.CssClass = "boton_pestana";
                btnFacturasProveedor.CssClass = "boton_pestana";
                btnEgresosVarios.CssClass = "boton_pestana_activo";
            }
            else
            {
                //Configurando Depositos
                mtvDepositos.ActiveViewIndex = 0;
                btnAnticipo.CssClass = "boton_pestana_activo";
                btnLiquidacion.CssClass = "boton_pestana";
                btnFacturasProveedor.CssClass = "boton_pestana";
                btnEgresosVarios.CssClass = "boton_pestana";
                //Inicializa los Valores de Depósito
                inicializaValoresDepositos();
            }
        }
        /// <summary>
        /// Carga los catalogos de la forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño de GridView Depósitos Pendientes
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGrid, "", 26);
            //Tamaño de GridView Últimos Depósitos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGridViewUltimosDepositos, "", 26);
            //Tamaño de GridView Liquidaciones Pendientes
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoLiquidacion, "", 26);
            //Tamaño de GridView Últimas Liquidaciones
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoLiquidaciones, "", 26);
            //Tamaño de GridView Facturas Pendientes
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturas, "", 26);
            //Tamaño de GridView Últimas Facturas
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoUltimasFacturas, "", 26);
            //Tamaño de GridView Facturas Ligadas al Anticipo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 26);
            //Cargando Tipos de Forma de Pago (REGULADO POR EL SAT V3.3 DE CFDI)
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPagoDeposito, 185, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPagoLiquidacion, 185, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPagoFactura, 185, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPagoDepFac, 185, "");
            //Asignando Valor Por Defecto
            ddlFormaPagoDeposito.SelectedValue = ddlFormaPagoLiquidacion.SelectedValue = ddlFormaPagoFactura.SelectedValue = ddlFormaPagoDepFac.SelectedValue = "8";
            //Carga catalogo Bancos
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBanco, 76, "-Seleccione el Banco-", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBanco, 22, "-Seleccione el Banco-", 0, "", 0, "");
            //Carga catalogo Cuentas 
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuenta, 23, "-Seleccione la Cuenta-", Convert.ToInt32(ddlBanco.SelectedValue), "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }
        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "cuentaBancos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaAltaCuentas", "ventanaAltaCuentas");
                    break;
                case "FacturasLigadas":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaAplicacionFactura", "ventanaAplicacionFactura");
                    break;
                case "AnticipoFactura":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaDepositoFactura", "ventanaDepositoFactura");
                    break;
            }
        }

        #region Métodos Depositos

        /// <summary>
        /// Inicializa Valores Depósitos
        /// </summary>
        private void inicializaValoresDepositos()
        {
            //Limpiando resultados y referencia de última operación
            txtFechaDeposito.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
            txtReferencia.Text = "";
        }
        /// <summary>
        /// Realiza la carga del contenido de controles GridView
        /// </summary>
        private void cargaDepositosPendientes()
        {
            //Inicializa Valores Depósitos
            inicializaValoresDepositos();
            //Inicializamos el grid Ultimos Depósitos
            Controles.InicializaGridview(gvUltimosDepositos);
            Controles.InicializaIndices(gvDepositosPendientes);

            //Obteniendo Depositos pendientes
            using (DataTable mit = SAT_CL.EgresoServicio.Deposito.CargaRegistrosPorDepositar(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Cargamos el grid view
                Controles.CargaGridView(gvDepositosPendientes, mit, "Id-IdEgreso-NoDeposito-IdCuenta-IdTabla-IdRegistro", lblCriterioGridDepositosPendientes.Text, true, 0);

                //Validamos Carga Depositos Pendientes
                if (mit != null)
                    //Guardando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                else
                    //Elimina Tabla Datasets
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }
        }
        /// <summary>
        /// Realiza el cambio de estatus de un depósito
        /// </summary>
        /// <param name="control">Control que se dispara</param>
        /// <param name="estatus">Nuevo Estatus depósito</param>
        private void actualizaDepositos(System.Web.UI.Control control, string motivo_referencia, DateTime fec_dep, int id_forma_pago, SAT_CL.EgresoServicio.Deposito.EstatusDeposito estatus)
        {
            //Defininedo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            List<int> deps_idx = new List<int>();
            SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT forma_pago = (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)id_forma_pago;

            //Verificando que existan depósitos seleccionados
            GridViewRow[] depositos = Controles.ObtenerFilasSeleccionadas(gvDepositosPendientes, "chkSeleccionDeposito");

            //Si existen depositos
            if (depositos.Length > 0)
            {
                //Para cada uno de los controles marcados
                foreach (GridViewRow r in depositos)

                    //Añadiendo indice
                    deps_idx.Add(r.RowIndex);

            }
            else if (gvDepositosPendientes.SelectedIndex != -1)

                //Añadiendo indice
                deps_idx.Add(gvDepositosPendientes.SelectedIndex);

            //Validando Lista
            if (deps_idx.Count > 0)
            {
                //Recorriendo Egresos
                foreach (int idx in deps_idx)
                {
                    //Seleccionando la fila
                    gvDepositosPendientes.SelectedIndex = idx;

                    //Instanciando egreso por depósito
                    using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdEgreso"])))
                    {
                        //Si el registro existe
                        if (egreso.habilitar)
                        {
                            //Identificando la acción solicitada por el usuario
                            switch (estatus)
                            {
                                //Depositado
                                case Deposito.EstatusDeposito.PorLiquidar:
                                    {
                                        //Obteniendo Facturas Ligadas a la Entidad
                                        using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(51, egreso.id_registro, true))
                                        {
                                            //Validando si hay facturas
                                            if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))

                                                //Instanciando Excepción
                                                resultado = new RetornoOperacion("Existen Facturas Ligadas al Anticipo");
                                            else
                                                //Actualizando a Estatus Depositado
                                                resultado = egreso.ActualizarEstatusEgresoDepositado(fec_dep, Convert.ToInt32(ddlCuenta.SelectedValue),
                                                                        Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdCuenta"]),
                                                                        motivo_referencia.ToUpper(), forma_pago, ((Usuario)Session["usuario"]).id_usuario);
                                        }
                                        break;
                                    }
                                //Rechazado
                                case Deposito.EstatusDeposito.Registrado:
                                    {
                                        //Cancelando Deposito
                                        resultado = egreso.ActualizarEstatusEgresoDepositoCancelado(motivo_referencia.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    }
                            }
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(control, new RetornoOperacion("Información de depósito no encontrada, actualice y vuelva a intentarlo."), ScriptServer.PosicionNotificacion.AbajoDerecha);

                    }

                    //Mostrando resultado por viaje
                    ScriptServer.MuestraNotificacion(control, string.Format("ResultadoMultiple_{0}", Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["NoDeposito"])), string.Format("Folio {0}: {1}", Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["NoDeposito"]), resultado.Mensaje), resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

                //Reiniciar el GridView Depositos Pendientes
                cargaDepositosPendientes();
            }
            else
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(control, "Error, no se han seleccionado depósitos", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el cambio de estatus de un depósito
        /// </summary>
        /// <param name="control">Control que se dispara</param>
        /// <param name="estatus">Nuevo Estatus depósito</param>
        private void actualizaEgresoFactura(System.Web.UI.Control control, string entidad_egreso)
        {
            //Defininedo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT forma_pago = (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToInt32(ddlFormaPagoDepFac.SelectedValue);

            //Validando si Existe la Cuenta de Origen
            if (ddlCuenta.SelectedValue != "0")
            {
                //Validando Entidad de Egreso
                switch (entidad_egreso)
                {
                    case "Deposito":
                        {
                            //Si existe un Deposito Seleccionado
                            if (gvDepositosPendientes.SelectedIndex != -1)
                            {
                                //Validando que exista la Cuenta
                                if (Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdCuenta"]) > 0)
                                {
                                    //Instanciando egreso por depósito
                                    using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdEgreso"])))
                                    {
                                        //Si el registro existe
                                        if (egreso.habilitar)
                                        {
                                            //Obteniendo Facturas Ligadas a la Entidad
                                            using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(51, egreso.id_registro, true))
                                            {
                                                //Validando si hay facturas
                                                if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                                                {
                                                    //Ocultando Ventana
                                                    alternaVentanaModal("AnticipoFactura", this);

                                                    //Cargando Facturas 
                                                    cargaFacturasLigadas(51, egreso.id_registro);

                                                    //Mostrando Ventana
                                                    alternaVentanaModal("FacturasLigadas", this);

                                                    //Mostrando Totales
                                                    actualizaTotales();
                                                }
                                                else
                                                {
                                                    //Mostrando Ventana
                                                    alternaVentanaModal("AnticipoFactura", this);

                                                    //Actualizando a Estatus Depositado
                                                    resultado = egreso.ActualizarEstatusEgresoDepositado(Convert.ToDateTime(txtFechaDepFac.Text),
                                                                            Convert.ToInt32(ddlCuenta.SelectedValue),
                                                                            Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdCuenta"]),
                                                                            txtReferenciaDepFac.Text.ToUpper(), forma_pago,
                                                                            ((Usuario)Session["usuario"]).id_usuario);

                                                    //Validando Operación Exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Mostrando Mensaje de Operación
                                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("* El Egreso No. {0}: Ha sido actualizado", egreso.secuencia_compania), true);

                                                        //Reiniciar el GridView Depositos Pendientes
                                                        cargaDepositosPendientes();
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("* El Egreso No. {0}: No ha sido actualizado, {1}", egreso.secuencia_compania, resultado.Mensaje), true);

                                                    //Mostrando Mensaje
                                                    ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Información de depósito no encontrada, actualice y vuelva a intentarlo."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe la Cuenta"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe un Deposito Seleccionado."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                    case "Liquidacion":
                        {
                            //Si existe un Deposito Seleccionado
                            if (gvLiquidacionesPendientes.SelectedIndex != -1)
                            {
                                //Validando que exista la Cuenta
                                if (Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdCuenta"]) > 0)
                                {
                                    //Instanciando egreso por depósito
                                    using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdEgreso"])))
                                    {
                                        //Si el registro existe
                                        if (egreso.habilitar)
                                        {
                                            //Obteniendo Facturas Ligadas a la Entidad
                                            using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(82, egreso.id_registro, true))
                                            {
                                                //Validando si hay facturas
                                                if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                                                {
                                                    //Ocultando Ventana
                                                    alternaVentanaModal("AnticipoFactura", this);

                                                    //Cargando Facturas 
                                                    cargaFacturasLigadas(82, egreso.id_registro);

                                                    //Mostrando Ventana
                                                    alternaVentanaModal("FacturasLigadas", this);

                                                    //Mostrando Totales
                                                    actualizaTotales();
                                                }
                                                else
                                                {
                                                    //Mostrando Ventana
                                                    alternaVentanaModal("AnticipoFactura", this.Page);

                                                    //Depositando Liquidación
                                                    resultado = egreso.ActualizarEstatusEgresoLiquidacionAceptada(Convert.ToDateTime(txtFechaDepFac.Text), Convert.ToInt32(ddlCuenta.SelectedValue), 
                                                                            Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdCuenta"]), txtReferenciaDepFac.Text.ToUpper(),
                                                                            forma_pago,((Usuario)Session["usuario"]).id_usuario);

                                                    //Validando Operación Exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Mostrando Mensaje de Operación
                                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("* El Egreso No. {0}: Ha sido actualizado", egreso.secuencia_compania), true);

                                                        //Reiniciar el GridView Depositos Pendientes
                                                        cargaDepositosPendientes();
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("* El Egreso No. {0}: No ha sido actualizado, {1}", egreso.secuencia_compania, resultado.Mensaje), true);

                                                    //Mostrando Mensaje
                                                    ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                }
                                            }
                                        }
                                        else
                                            //Instanciando Excepción
                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Información de depósito no encontrada, actualice y vuelva a intentarlo."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe la Cuenta"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe una Liquidación Seleccionada."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                }
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Seleccione la Cuenta"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Actualizar los Totales
        /// </summary>
        private void actualizaTotales()
        {
            //Declarando Variables Auxiliares
            decimal saldo_egreso = 0.00M;
            int id_egreso = obtieneEgresoEntidad();

            //Instanciando Egreso
            using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(id_egreso))
            {
                //Validando que exista el Egreso
                if (egreso.habilitar)

                    //Obteniendo Saldo de Egreso
                    saldo_egreso = egreso.ObtieneSaldoEgreso();
            }
            
            //Asignando Saldo Disponible de el Anticipo
            lblSaldoFI.Text = string.Format("{0:C2}", saldo_egreso);

            //Validando que Existan Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5")))
            {
                //Mostrando el Saldo por Aplicar
                lblPorAplicar.Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(MontoPorAplicar)", "")));
                lblSaldoFinal.Text = string.Format("{0:C2}", saldo_egreso - Convert.ToDecimal((((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(MontoPorAplicar)", ""))));
            }
            else
            {
                //Mostrando el Saldo en Ceros
                lblPorAplicar.Text =
                lblSaldoFinal.Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Realiza la busqueda de los últimos depósitos en base a la clave del operador
        /// </summary>
        private void buscaDepositosRealizados()
        {
            //Realizamos consulta 
            using (DataTable mit = SAT_CL.EgresoServicio.Reportes.VisorUltimosDepositos(Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1), "0")), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1), "0")),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, ':', 1)), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Almacenando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
                    //Cargamos Grid View
                    Controles.CargaGridView(gvUltimosDepositos, mit, "Id", lblCriterioGridViewUltimosDepositos.Text, true, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvUltimosDepositos);
                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        private void inicializaReferencias(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/Prueba.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }
        /// <summary>
        /// Método que inicializa el control bitácora del registro
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="titulo">TItulo a mostrar</param>
        private void inicializaBitacoraRegistro(string idRegistro, string idTabla, string titulo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        #endregion

        #region Métodos Liquidación
        /// <summary>
        /// Inicializa Valores Liquidaciones
        /// </summary>
        private void inicializaValoresLiquidacion()
        {
            //Limpiamos Controles
            txtFechaLiquidacion.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
            txtReferenciaLiquidacion.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Liquidaciones por Depositar
        /// </summary>
        private void cargaLiquidacionesPendientes()
        {
            //Inicializamos Valores
            inicializaValoresLiquidacion();
            //Inicializamos el grid Ultimos Depósitos
            Controles.InicializaGridview(gvUltimasLiquidaciones);
            Controles.InicializaIndices(gvLiquidacionesPendientes);

            //Instanciando Liquidaciones
            using (DataTable dtLiquidaciones = SAT_CL.Liquidacion.Liquidacion.ObtieneLiquidacionesPorDepositar(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtLiquidaciones))
                {
                    //Cargamos el grid view
                    Controles.CargaGridView(gvLiquidacionesPendientes, dtLiquidaciones, "Id-IdEgreso-IdCuenta-NoLiquidacion-IdTabla-IdRegistro", lblOrdenadoLiquidacion.Text, true, 0);

                    //Guardando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtLiquidaciones, "Table2");
                }
                else
                {
                    //Inicializamos el grid view
                    Controles.InicializaGridview(gvLiquidacionesPendientes);

                    //Eliminando origen de datos en sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Realiza el cambio de estatus de un Depósito
        /// </summary>
        /// <param name="control">Control que dispara</param>
        /// <param name="estatus">Nuevo Estatus depósito</param>
        private void actualizaLiquidaciones(System.Web.UI.Control control, string motivo_referencia, DateTime fecha_dep, int id_forma_pago, SAT_CL.Liquidacion.Liquidacion.Estatus estatus)
        {
            //Defininedo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            List<int> liqs_idx = new List<int>();
            SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT forma_pago = (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)id_forma_pago;

            //Verificando que existan depósitos seleccionados
            GridViewRow[] liquidaciones = Controles.ObtenerFilasSeleccionadas(gvLiquidacionesPendientes, "chkSeleccionLiquidacion");

            //Si existen depositos
            if (liquidaciones.Length > 0)
            {
                //Para cada uno de los controles marcados
                foreach (GridViewRow r in liquidaciones)

                    //Añadiendo indice
                    liqs_idx.Add(r.RowIndex);
            }
            else if (gvLiquidacionesPendientes.SelectedIndex != -1)

                //Añadiendo indice
                liqs_idx.Add(gvLiquidacionesPendientes.SelectedIndex);

            //Si existen 
            if (liqs_idx.Count > 0)
            {
                //Para cada uno de los controles marcados
                foreach (int idx in liqs_idx)
                {
                    //Seleccionando la fila
                    gvLiquidacionesPendientes.SelectedIndex = idx;

                    //Instanciando egreso por depósito
                    using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdEgreso"])))
                    {
                        //Si el registro existe
                        if (egreso.id_egreso_ingreso > 0)
                        {
                            //Instanciando Liquidación
                            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(egreso.id_registro))
                            {
                                //Validando que exista el Registro
                                if (liq.id_liquidacion > 0)
                                {
                                    //Identificando la acción solicitada por el usuario
                                    switch (estatus)
                                    {
                                        //Depositado
                                        case SAT_CL.Liquidacion.Liquidacion.Estatus.Depositado:
                                            {
                                                //Obteniendo Facturas Ligadas a la Entidad
                                                using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(82, egreso.id_registro, true))
                                                {
                                                    //Validando si hay facturas
                                                    if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))

                                                        //Instanciando Excepción
                                                        resultado = new RetornoOperacion("Existen Facturas Ligadas a la Liquidación");
                                                    else
                                                    {   
                                                        //Validando que exista la Cuenta
                                                        if (Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdCuenta"]) > 0)

                                                            //Depositando Liquidación
                                                            resultado = egreso.ActualizarEstatusEgresoLiquidacionAceptada(fecha_dep, Convert.ToInt32(ddlCuenta.SelectedValue), 
                                                                                    Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdCuenta"]), motivo_referencia.ToUpper(),
                                                                                    forma_pago,((Usuario)Session["usuario"]).id_usuario);
                                                        else
                                                            //Instanciando Excepción
                                                            resultado = new RetornoOperacion("No existe la Cuenta");
                                                    }
                                                }
                                                break;
                                            }
                                        //Rechazado
                                        case SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado:
                                            resultado = egreso.ActualizarEstatusEgresoLiquidacionCancelada(motivo_referencia.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                            break;
                                    }
                                }
                            }
                        }
                        else
                            //Instanciando Excepcion
                            resultado = new RetornoOperacion("Información de Liquidación no encontrada, actualice y vuelva a intentarlo.");
                    }
                    //Mostrando resultado por Factura
                    ScriptServer.MuestraNotificacion(control, string.Format("ResultadoMultiple_{0}", Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["NoLiquidacion"])), string.Format("Liquidación {0}: {1}", Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["NoLiquidacion"]), resultado.Mensaje), resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                }

                //Reiniciar el GridView Liquidaciones Pendientes
                cargaLiquidacionesPendientes();
            }
            else
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(control, "Error, no se han seleccionado liquidaciones", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Realiza la busqueda de los últimos depósitos en base a la clave del operador
        /// </summary>
        private void buscaLiquidacionesRealizadas()
        {
            //Realizamos consulta 
            using (DataTable mit = SAT_CL.Liquidacion.Liquidacion.ObtieneLiquidacionesDepositadas(Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperadorLiquidacion.Text, ':', 1), "0")), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidadLiquidacion.Text, ':', 1), "0")),
                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedorLiquidacion.Text, ':', 1)), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Almacenando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table3");
                    //Cargamos Grid View
                    Controles.CargaGridView(gvUltimasLiquidaciones, mit, "NoLiquidacion", lblOrdenadoUltimasLiq.Text, true, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvUltimasLiquidaciones);
                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }

        #endregion

        #region Método Facturas

        /// <summary>
        /// Inicializa Valores Facturas
        /// </summary>
        private void inicializaValoresFacturas()
        {
            //Inicializamos Controles
            txtReferenciaFactura.Text = "";
            txtFechaFactura.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Liquidaciones por Depositar
        /// </summary>
        private void cargaFacturasPendientes()
        {
            //Inicializa Valores
            inicializaValoresFacturas();
            //Inicializamos el grid Ultimos Depósitos
            Controles.InicializaGridview(gvUltimasFacturas);
            Controles.InicializaIndices(gvFacturasPendientes);

            //Instanciando Liquidaciones
            using (DataTable dtFacturasPendientes = SAT_CL.CXP.Reportes.ObtieneFacturasPorPagar((((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturasPendientes))
                {
                    //Cargamos el grid view
                    Controles.CargaGridView(gvFacturasPendientes, dtFacturasPendientes, "Id-NoFactura-IdCuenta-IdTabla-IdRegistro", lblOrdenadoLiquidacion.Text, true, 0);

                    //Guardando origen de datos en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasPendientes, "Table3");
                }
                else
                {
                    //Inicializamos el grid view
                    Controles.InicializaGridview(gvFacturasPendientes);

                    //Eliminando origen de datos en sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }
        /// <summary>
        /// Realiza el cambio de estatus de una Factura
        /// </summary>
        /// <param name="estatus">Nuevo Estatus depósito</param>
        ///<param name="botonActualizar"> Boton Actualizar</param>
        private void actualizaFacturas(System.Web.UI.Control botonActualizar, SAT_CL.CXP.FacturadoProveedor.EstatusFactura estatus)
        {

            //Defininedo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Verificando que existan depósitos seleccionados
            GridViewRow[] facturas = Controles.ObtenerFilasSeleccionadas(gvFacturasPendientes, "chkSeleccionFactura");

            //
            if (ddlCuenta.SelectedValue != "0")
            {
                //Validamos Fecha de Depositos
                if (Convert.ToDateTime(txtFechaFactura.Text) <= Fecha.ObtieneFechaEstandarMexicoCentro())
                {
                    //Si existen 
                    if (facturas.Length > 0)
                    {
                        //Para cada uno de los controles marcados
                        foreach (GridViewRow r in facturas)
                        {
                            //Seleccionando la fila
                            gvFacturasPendientes.SelectedIndex = r.RowIndex;

                            //Inicializando Bloque
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Instanciando Aplicación
                                using (SAT_CL.CXC.FichaIngresoAplicacion fia = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["Id"])))
                                {
                                    //Validando que Exista la Aplicación
                                    if (fia.id_ficha_ingreso_aplicacion > 0)
                                    {
                                        //Instanciando Factura de Proveedor
                                        using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(fia.id_registro))
                                        {
                                            //Validando que existe la Factura
                                            if (fp.id_factura > 0)
                                            {
                                                //Validando que existe una Cuenta
                                                if (Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["IdCuenta"]) > 0)
                                                {
                                                    //Instanciando Proveedor
                                                    using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(fp.id_compania_proveedor))
                                                    {
                                                        //Validando que exista el Proveedor
                                                        if (pro.id_compania_emisor_receptor > 0)
                                                        {
                                                            //Insertando Pago de la Factura
                                                            resultado = SAT_CL.Bancos.EgresoIngreso.InsertaPagoFactura(Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 25, pro.id_compania_emisor_receptor, pro.nombre,
                                                                                Convert.ToInt32(ddlCuenta.SelectedValue), Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["IdCuenta"]), "", fia.monto_aplicado, 1, fia.monto_aplicado, Convert.ToDateTime(txtFechaFactura.Text), txtReferenciaLiquidacion.Text.ToUpper(),
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                        }
                                                        else
                                                        {
                                                            //Instanciando Excepción
                                                            resultado = new RetornoOperacion("No Existe el Proveedor");
                                                        }
                                                    }
                                                    //Validando que la Operación fuese Exitosa
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Obteniendo Pago de la Factura
                                                        int idPagoFactura = resultado.IdRegistro;

                                                        //Actualizando Pago de la Aplicación
                                                        resultado = fia.ActualizaPagoFacturaAplicacion(idPagoFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que la Operación fuese Exitosa
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando la Factura
                                                            resultado = fp.ActualizaEstatusFacturadoProveedor(fp.ObtieneEstatusFacturaAplicada(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que se haya Actualizado Exitosamente
                                                            if (resultado.OperacionExitosa)

                                                                //Completando Transacción
                                                                trans.Complete();
                                                        }
                                                        //Mostrando resultado por Factura
                                                        ScriptServer.MuestraNotificacion(botonActualizar, string.Format("ResultadoMultiple_{0}", fia.id_registro), string.Format("Factura {0}: {1}", Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["NoFactura"]), resultado.Mensaje), resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                                    }
                                                }
                                                else
                                                    //Mostrando resultado por Factura
                                                    ScriptServer.MuestraNotificacion(botonActualizar, string.Format("ResultadoMultiple_{0}", fia.id_registro), string.Format("Factura {0}: No existe una Cuenta para este Proveedor", Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["NoFactura"])), resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                        }
                                    }
                                }

                            }

                        }

                        //Reiniciar el GridView Liquidaciones Pendientes
                        cargaFacturasPendientes();
                    }
                    else
                        //Mostrando resultado
                        ScriptServer.MuestraNotificacion(botonActualizar, "Error, no se han seleccionado facturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando resultado
                    ScriptServer.MuestraNotificacion(botonActualizar, "La Fecha del Depósito debe ser menor o igual a la Fecha Actual.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

            }
            else
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(botonActualizar, "Debe Selecionar una Cuenta", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);




        }

        /// <summary>
        /// Método encargado de Rechazar las Facturas
        /// </summary>
        /// <param name="control">Control que se dispara</param>
        private void rechazaFacturas(System.Web.UI.Control control)
        {
            //Defininedo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Verificando que existan depósitos seleccionados
            GridViewRow[] facturas = Controles.ObtenerFilasSeleccionadas(gvFacturasPendientes, "chkSeleccionFactura");

            //Si existen Facturas
            if (facturas.Length > 0)
            {
                //Para cada uno de los controles marcados
                foreach (GridViewRow r in facturas)
                {
                    //Seleccionando la fila
                    gvFacturasPendientes.SelectedIndex = r.RowIndex;

                    //Instanciando Aplicación
                    using (SAT_CL.CXC.FichaIngresoAplicacion fia = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(gvFacturasPendientes.SelectedDataKey["Id"])))
                    {
                        //Validando que Exista la Aplicación
                        if (fia.id_ficha_ingreso_aplicacion > 0)
                            resultado = fia.RechazaPagoFacturaProveedor(txtReferenciaFactura.Text, ((UsuarioSesion)Session["usuario_sesion"]).id_usuario);
                    }
                }

                //Reiniciar el GridView Liquidaciones Pendientes
                cargaFacturasPendientes();
            }
            else
                resultado = new RetornoOperacion("No se han seleccionado facturas");

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos Egresos Varios

        /// <summary>
        /// Método Privado encargado de Inicializar la Sección de Egresos Varios
        /// </summary>
        private void inicializaSeccionEgresosVarios()
        {
            //Obteniendo Estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];
            //Cargando Catalogos
            cargaCatalogosEgresosVarios();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogosEgresosVarios()
        {
            //Cargando Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 79);
            //Cargando Conceptos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 64, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Tipos de Moneda
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "", 11);
            //Cargando Tipos de Método de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 80);
            //Inicializando DropDownList
            TSDK.ASP.Controles.InicializaDropDownList(ddlCuentaOrigen, "-- Seleccione una Cuenta");
            //Carga Catálogo de Departamentos.
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDepartamento, 89, "TODOS");
        }
        /// <summary>
        /// Método Privado encargado de habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbEliminar.Enabled =
                        lkbDepositar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbEliminar.Enabled =
                        lkbDepositar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbEliminar.Enabled = false;
                        lkbDepositar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:

                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Valores
                        ddlConcepto.Enabled =
                        txtNombreDep.Enabled =
                        txtProveedorEgreso.Enabled =
                        txtNumCheque.Enabled =
                        txtNoTransferenciaBancaria.Enabled =
                        ddlMetodoPago.Enabled =
                        ddlCuentaOrigen.Enabled =
                        txtCuentaDestino.Enabled =
                        txtMonto.Enabled =
                        ddlMoneda.Enabled =
                        txtFechaEI.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        ddlDepartamento.Enabled =
                        txtObservacion.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Valores
                        ddlConcepto.Enabled =
                        txtNombreDep.Enabled =
                        txtProveedorEgreso.Enabled =
                        txtNumCheque.Enabled =
                        txtNoTransferenciaBancaria.Enabled =
                        ddlMetodoPago.Enabled =
                        ddlCuentaOrigen.Enabled =
                        txtCuentaDestino.Enabled =
                        txtMonto.Enabled =
                        ddlMoneda.Enabled =
                        txtFechaEI.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        ddlDepartamento.Enabled =
                        txtCuentaDestino.Enabled =
                        txtProveedorEgreso.Enabled =
                        txtObservacion.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializando Valores
                        lblNoEgreso.Text = "Por Asignar";
                        txtNombreDep.Text =
                        txtNumCheque.Text = "";
                        txtNoTransferenciaBancaria.Text = "";
                        txtMonto.Text =
                        txtMontoPesos.Text = "0.00";
                        txtFechaEI.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                        txtObservacion.Text = "";
                        //Invocando Método de Configuración
                        configuraControlesMetodoPago();

                        //Invocando Método de Configuración
                        configuraConceptoProveedor(Convert.ToInt32(ddlConcepto.SelectedValue));

                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Producto
                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (ei.id_egreso_ingreso != 0)
                            {
                                //Inicializando Valores
                                lblNoEgreso.Text = ei.secuencia_compania.ToString();
                                ddlEstatus.SelectedValue = ei.id_estatus.ToString();
                                txtMonto.Text = string.Format("{0:0.00}", ei.monto);
                                txtMontoPesos.Text = string.Format("{0:0.00}", ei.monto_pesos);
                                txtFechaEI.Text = ei.fecha_egreso_ingreso.ToString("dd/MM/yyyy");
                                ddlMetodoPago.SelectedValue = ei.id_forma_pago.ToString();
                                ddlMoneda.SelectedValue = ei.id_moneda.ToString();
                                ddlConcepto.SelectedValue = ei.id_egreso_ingreso_concepto.ToString();
                                ddlDepartamento.SelectedValue = SAT_CL.Global.Referencia.CargaReferencia("0", 101, ei.id_egreso_ingreso, "Datos Contables", "Departamento");
                                //Invocando Método de Configuración
                                configuraConceptoProveedor(Convert.ToInt32(ddlConcepto.SelectedValue));

                                //Asignando Valores
                                txtNombreDep.Text = ei.nombre_depositante;

                                //Instanciando Proveedor
                                using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(ei.id_registro))
                                {
                                    //Validando que exista el Registro
                                    if (pro.habilitar)

                                        //Asignando Proveedor
                                        txtProveedorEgreso.Text = pro.nombre + " ID:" + pro.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Control
                                        txtProveedorEgreso.Text = "";
                                }

                                //Instanciamos Transferencia Bancaria
                                using (SAT_CL.Global.Referencia objReferencia = new SAT_CL.Global.Referencia(ei.id_transferencia_bancaria))
                                {
                                    //Inicializamos Valores                                     
                                    txtNoTransferenciaBancaria.Text = objReferencia.valor;
                                }

                                //Obteniendo Referencias
                                using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(ei.id_egreso_ingreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Observación Egreso-Ingreso", 0, "Datos Contables")))
                                {
                                    //Valdiando que Existan
                                    if (Validacion.ValidaOrigenDatos(dtRef))
                                    {
                                        //Recorriendo Ciclo
                                        foreach (DataRow dr in dtRef.Rows)
                                        {
                                            //Validando que Exista el Registro
                                            if (Convert.ToInt32(dr["Id"]) > 0)
                                            {
                                                //Instanciando Observación
                                                using (SAT_CL.Global.Referencia observacion = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                                {
                                                    //Validando que exista la Referencia
                                                    if (observacion.habilitar)

                                                        //Asignando Valor
                                                        txtObservacion.Text = observacion.valor;
                                                    else
                                                        //Limpiando Valor
                                                        txtObservacion.Text = "";
                                                }
                                            }
                                        }
                                    }
                                }
                                //Validando el Método de Pago
                                switch (ddlMetodoPago.SelectedValue)
                                {

                                    //Cheque Nominativo
                                    case "10":
                                        {
                                            //Asignando Valores
                                            txtNumCheque.Text = ei.num_cheque;
                                            txtCuentaDestino.Text = "";
                                            break;
                                        }
                                    //Transferencia
                                    case "8":
                                    //Tarjeta de Credito
                                    case "11":
                                    //Tarjeta de Credito
                                    case "12":
                                    //Tarjeta de Credito
                                    case "13":
                                    //Tarjeta de Credito
                                    case "16":
                                    //Tarjeta de Debito
                                    case "15":
                                        {
                                            //Asignando Valores
                                            txtNumCheque.Text = "";
                                            txtCuentaDestino.Text = ei.num_cheque;
                                            break;
                                        }
                                    default:
                                        {
                                            //Asignando Valores
                                            txtNumCheque.Text =
                                            txtCuentaDestino.Text = "";
                                            break;
                                        }
                                }

                                //Invocando Método de Configuración
                                configuraControlesMetodoPago();

                                //Cargando Cuentas de Destino -- Donde se va a Recibir el Dinero
                                ddlCuentaOrigen.SelectedValue = ei.id_cuenta_origen.ToString();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Guardar las Fichas de Ingreso
        /// </summary>
        private void guardaFichaIngreso(System.Web.UI.Control control)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            RetornoOperacion retorno = new RetornoOperacion();
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando Estatus de Pagina
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:
                        {
                            //Insertando Egreso
                            result = SAT_CL.Bancos.EgresoIngreso.InsertaEgresosVarios(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                        (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? 25 : 0, (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedorEgreso.Text, "ID:", 1)) : 0,
                                        (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? Cadena.RegresaCadenaSeparada(txtProveedorEgreso.Text, "ID:", 0) : txtNombreDep.Text,
                                        Convert.ToInt32(ddlConcepto.SelectedValue), (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToByte(ddlMetodoPago.SelectedValue),
                                        Convert.ToInt32(ddlCuentaOrigen.SelectedValue), 0, txtNumCheque.Text == "" ? txtCuentaDestino.Text : txtNumCheque.Text, Convert.ToDecimal(txtMonto.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                        Convert.ToDecimal(txtMontoPesos.Text), fecha, txtNoTransferenciaBancaria.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Egreso
                                int idEgreso = result.IdRegistro;
                                //Si el control de observación  no esta vacio
                                if (txtObservacion.Text != "")
                                {
                                    //Insertando Referencia
                                    result = SAT_CL.Global.Referencia.InsertaReferencia(idEgreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Observación Egreso-Ingreso", 0, "Datos Contables"),
                                                txtObservacion.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                //Operación Exitosa?
                                if (result.OperacionExitosa)

                                    retorno = SAT_CL.Global.Referencia.InsertaReferencia(idEgreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Departamento", 0, "Datos Contables"), ddlDepartamento.SelectedValue,
                                                                                    TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //La operación fue Exitosa
                                if (retorno.OperacionExitosa)
                                    //Instanciando Id de Egreso
                                    result = new RetornoOperacion(idEgreso);
                            }
                            break;
                        }
                    case Pagina.Estatus.Edicion:
                        {
                            //Instanciando Egreso
                            using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando que exista un Id Valido
                                if (ei.id_egreso_ingreso != 0)
                                {
                                    //Validando que se encuentre Capturada para poderse Editar
                                    if (ei.estatus == SAT_CL.Bancos.EgresoIngreso.Estatus.Capturada)

                                        //Editando Egreso
                                        result = ei.EditaEgresosVarios(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? 25 : 0, (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedorEgreso.Text, "ID:", 1)) : 0,
                                                    (ddlConcepto.SelectedValue.Equals("17") || ddlConcepto.SelectedValue.Equals("50")) ? Cadena.RegresaCadenaSeparada(txtProveedorEgreso.Text, "ID:", 0) : txtNombreDep.Text,
                                                    Convert.ToInt32(ddlConcepto.SelectedValue), (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToByte(ddlMetodoPago.SelectedValue),
                                                    Convert.ToInt32(ddlCuentaOrigen.SelectedValue), 0, txtNumCheque.Text == "" ? txtCuentaDestino.Text : txtNumCheque.Text, Convert.ToDecimal(txtMonto.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                                    Convert.ToDecimal(txtMontoPesos.Text), fecha, txtNoTransferenciaBancaria.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion(string.Format("Estatus '{0}' del Egreso, Imposible su Edición", ei.estatus));
                                    //Obteniendo Egreso
                                    int idEgreso = result.IdRegistro;

                                    //Validando Operación
                                    if (result.OperacionExitosa)
                                    {

                                        //Obteniendo Referencias
                                        using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(ei.id_egreso_ingreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Observación Egreso-Ingreso", 0, "Datos Contables")))
                                        {
                                            //Valdiando que Existan
                                            if (Validacion.ValidaOrigenDatos(dtRef))
                                            {
                                                //Recorriendo Ciclo
                                                foreach (DataRow dr in dtRef.Rows)
                                                {
                                                    //Validando que Exista el Registro
                                                    if (Convert.ToInt32(dr["Id"]) > 0)
                                                    {
                                                        //Instanciando Observación
                                                        using (SAT_CL.Global.Referencia observacion = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                                        {
                                                            //Validando que exista la Referencia
                                                            if (observacion.habilitar)
                                                            {
                                                                //Si la el control no tiene texto
                                                                if (txtObservacion.Text == "")
                                                                    //Elimina la referencia de observación
                                                                    result = SAT_CL.Global.Referencia.EliminaReferencia(observacion.id_referencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                //En caso contrario
                                                                else
                                                                    //Editando Referencia
                                                                    result = SAT_CL.Global.Referencia.EditaReferencia(observacion.id_referencia, txtObservacion.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                //La Operación es Exitosa?
                                                                if (result.OperacionExitosa)
                                                                    //Instanciando Id de Egreso
                                                                    result = new RetornoOperacion(idEgreso);
                                                            }
                                                        }
                                                    }

                                                    //Termiando Ciclo
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Insertando Referencia
                                                result = SAT_CL.Global.Referencia.InsertaReferencia(idEgreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Observación Egreso-Ingreso", 0, "Datos Contables"),
                                                            txtObservacion.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Instanciando Id de Egreso
                                                result = new RetornoOperacion(idEgreso);
                                            }
                                        }
                                    }
                                    //Inserta departamento
                                    //Validando Operación
                                    if (result.OperacionExitosa)
                                    {
                                        //Obteniendo referencias de Egreso
                                        using (DataTable dtDepto = SAT_CL.Global.Referencia.CargaReferencias(ei.id_egreso_ingreso, 101, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 101, "Departamento", 0, "Datos Contables")))
                                        {
                                            if (Validacion.ValidaOrigenDatos(dtDepto))
                                            {
                                                //Recorriendo Ciclo
                                                foreach (DataRow dr in dtDepto.Rows)
                                                {
                                                    //Validando que Exista el Registro
                                                    if (Convert.ToInt32(dr["Id"]) > 0)
                                                    {
                                                        //Instanciando Observación
                                                        using (SAT_CL.Global.Referencia depto = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                                        {
                                                            //Validando que exista la Referencia
                                                            if (depto.habilitar)
                                                                //Editando Referencia
                                                                result = SAT_CL.Global.Referencia.EditaReferencia(depto.id_referencia, ddlDepartamento.SelectedValue, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            //Instanciando Id de Egreso
                                                            result = new RetornoOperacion(idEgreso);
                                                        }
                                                    }
                                                    //Termiando Ciclo
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                //Validando que la Operación haya sido Exitosa
                if (result.OperacionExitosa)

                    //Completando Transacción
                    trans.Complete();
            }
            //Validando que la Operación haya sido Exitosa
            if (result.OperacionExitosa)
            {
                //Asignando Valores de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;

                //Inovcando Método de Inicialización
                //inicializaPagina();
                inicializaSeccionEgresosVarios();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(control, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {
            //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString() + "&P3=1");
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Obtener el Monto en Pesos
        /// </summary>
        /// <param name="monto">Monto a Convertir</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_moneda">Moneda a Utilizar</param>
        /// <param name="fecha">Fecha de Periodo del Tipo de Cambio</param>
        /// <returns></returns>
        private decimal obtieneMontoConvertido(decimal monto, int id_compania, byte id_moneda, DateTime fecha)
        {
            //Inicializando Monto de Retorno
            decimal monto_pesos = 0.00M;

            //Instanciando Tipo de Cambio
            using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(id_compania, id_moneda, fecha, (byte)SAT_CL.Bancos.TipoCambio.OperacionUso.Factura))
            {
                //Validando que exista el Tipo de Cambio
                if (tc.id_tipo_cambio > 0)

                    //Calculando Valor
                    monto_pesos = monto * tc.valor_tipo_cambio;
                else
                    //Asignando Valor
                    monto_pesos = monto;
            }

            //Devolviendo Cantidad Obtenida
            return monto_pesos;
        }
        /// <summary>
        /// Método encargado de Configurar los controles dado un Método de Pago
        /// </summary>
        private void configuraControlesMetodoPago()
        {

            //Validando el Método de Pago
            switch (ddlMetodoPago.SelectedValue)
            {
                //Cheque Nominativo
                case "10":
                    {
                        //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaOrigen, 42, "-- Seleccione una Cuenta", 25, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");

                        //Configurando Controles
                        txtNumCheque.Enabled = true;
                        txtCuentaDestino.Enabled = false;
                        //Asignando Valores
                        txtCuentaDestino.Text = "";
                        break;
                    }
                //Efectivo
                case "9":
                //Efectivo
                case "14":
                //No Identificado
                case "17":
                    {
                        //Inicializando Control
                        TSDK.ASP.Controles.InicializaDropDownList(ddlCuentaOrigen, "-- Seleccione una Cuenta");

                        //Deshabilitando Controles
                        txtNumCheque.Enabled =
                        txtCuentaDestino.Enabled = false;
                        //Asignando Valores
                        txtNumCheque.Text =
                        txtCuentaDestino.Text = "";
                        break;
                    }
                //Transferencia
                case "8":
                //Tarjeta de Credito
                case "11":
                //Monedero Electrònico
                case "12":
                //Dinero Electrònico
                case "13":
                //Tarjeta Servicio
                case "16":
                //Tarjeta de Debito
                case "15":
                    {
                        //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaOrigen, 42, "", 25, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");

                        //Configurando Controles
                        txtNumCheque.Enabled = false;
                        txtCuentaDestino.Enabled = true;
                        //Asignando Valores
                        txtNumCheque.Text = "";
                        break;
                    }
            }
            //Valida los estatus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                //Si el estatus de la página es Lectura
                case Pagina.Estatus.Lectura:
                    {
                        //Deshabilita los controles
                        txtNumCheque.Enabled =
                        txtCuentaDestino.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Configuración de los Controles
        /// </summary>
        /// <param name="id_concepto"></param>
        private void configuraConceptoProveedor(int id_concepto)
        {
            //Validando el Concepto
            switch (id_concepto)
            {
                //Anticipos de proveedor (17) y Notas de Crédito de Proveedor (50)
                case 17:
                case 50:
                    {
                        //Configurando Controles
                        txtProveedorEgreso.Visible = true;
                        txtNombreDep.Visible = false;
                        break;
                    }
                default:
                    {
                        //Configurando Controles
                        txtProveedorEgreso.Visible = false;
                        txtNombreDep.Visible = true;
                        break;
                    }
            }

            //Limpiando Controles
            txtProveedorEgreso.Text =
            txtNombreDep.Text = "";
        }

        #endregion

        #region Métodos Facturas Ligadas

        /// <summary>
        /// Método encargado de Cargar las Facturas Ligadas
        /// </summary>
        /// <param name="id_tipo_entidad">Tipo de Entidad</param>
        /// <param name="id_registro">Registro</param>
        private void cargaFacturasLigadas(int id_tipo_entidad, int id_registro)
        {
            //Obtiene Facturas Ligadas
            using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(id_tipo_entidad, id_registro, true))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id-IdFPR-SaldoActual-MontoPorAplicar-AplicacionesConfirmadas", lblOrdenadoGrid.Text, true, 2);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table5");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasLigadas);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvFacturasLigadas);
        }
        /// <summary>
        /// Método Privado encargado de la Configuración de los Registros de las Facturas
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="indices_chk">Indice de Controles</param>
        private void configuraRegistroFactura(object sender, out int[] indices_chk)
        {
            //Inicializando Variables
            indices_chk = new int[1];
            int contador = 0;
            bool indicador = false;
            int id_egreso = obtieneEgresoEntidad();

            //Declarando Variables Auxiliares
            decimal monto_sumatoria = 0.00M;
            decimal monto_disponible = 0.00M;
            decimal saldo_actual_egreso = 0.00M;

            //Obteniendo Control
            CheckBox chk = (CheckBox)sender;

            //Validando que el Control haya sido Marcado
            if (chk.Checked)
            {
                //Validando que exista el Anticipo
                using (SAT_CL.Bancos.EgresoIngreso egreso = new SAT_CL.Bancos.EgresoIngreso(id_egreso))
                {
                    //Validando que exista el Anticipo
                    if (egreso.id_egreso_ingreso > 0)
                    {
                        //Desmarcando Control para excluirlo del Calculo
                        chk.Checked = false;

                        //Obteniendo las Otras Filas Seleccionadas
                        GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturasLigadas, "chkVariosFactura");

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
                                gvFacturasLigadas.SelectedIndex =
                                indices_chk[contador] = gv.RowIndex;

                                //Añadiendo el Monto Pendiente a la Sumatoria
                                monto_sumatoria = monto_sumatoria + Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["MontoPorAplicar"]);

                                //Incrementando Contador
                                contador++;
                            }
                        }

                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvFacturasLigadas, sender, "chk", false);

                        //Guardando Indice Actual
                        indices_chk[contador] = gvFacturasLigadas.SelectedIndex;

                        //Obteniendo Saldo Actual Ficha
                        saldo_actual_egreso = egreso.ObtieneSaldoEgreso();

                        //Calculando Monto Disponible
                        monto_disponible = (saldo_actual_egreso - monto_sumatoria) < 0 ? 0 : (saldo_actual_egreso - monto_sumatoria);

                        //Obteniendo Resultado del Indicador
                        indicador = monto_disponible > 0 ? true : false;

                        //Obteniendo Fila por Editar
                        DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table5"].Select("Id = " + gvFacturasLigadas.SelectedDataKey["Id"].ToString() + " ");

                        //Recorriendo Registro Encontrado
                        foreach (DataRow dr in drEdit)
                        {
                            //Validando que el Monto Disponible sea Mayor o Igual al Monto Pendiente
                            if (monto_disponible >= Convert.ToDecimal(gvFacturasLigadas.SelectedDataKey["SaldoActual"]))
                            {
                                //Actualizando Registros
                                dr["MontoPorAplicar"] = string.Format("{0:0.00}", gvFacturasLigadas.SelectedDataKey["SaldoActual"]);
                                dr["MPA2"] = string.Format("{0:0.00}", gvFacturasLigadas.SelectedDataKey["SaldoActual"]);
                            }
                            else
                            {
                                //Actualizando Registros
                                dr["MontoPorAplicar"] = string.Format("{0:0.00}", monto_disponible);
                                dr["MPA2"] = string.Format("{0:0.00}", monto_disponible);
                            }
                        }

                        //Aceptando Cambios
                        ((DataSet)Session["DS"]).Tables["Table5"].AcceptChanges();
                    }
                }
            }
            else
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "chk", false);

                //Obteniendo Fila por Editar
                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table5"].Select("Id = " + gvFacturasLigadas.SelectedDataKey["Id"].ToString());

                //Recorriendo Registro Encontrado
                foreach (DataRow dr in drEdit)
                {
                    //Actualizando Registro
                    dr["MontoPorAplicar"] = "0.00";
                    dr["MPA2"] = "0.00";
                }

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table5"].AcceptChanges();

                //Obtenemos Filas Seleccionadas
                GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturasLigadas, "chkVariosFactura");

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
            Controles.CargaGridView(gvFacturasLigadas, ((DataSet)Session["DS"]).Tables["Table5"], "Id-IdFPR-SaldoActual-MontoPorAplicar-AplicacionesConfirmadas", "", true, 2);

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
                        gvFacturasLigadas.SelectedIndex = indice;

                        //Obteniendo Control
                        CheckBox chkFila = (CheckBox)gvFacturasLigadas.SelectedRow.FindControl("chkVariosFactura");

                        //Validando que exista el Control
                        if (chkFila != null)
                        {
                            //Marcando Control
                            chkFila.Checked = true;

                            //Obteniendo Control
                            LinkButton lnk = (LinkButton)gvFacturasLigadas.SelectedRow.FindControl("lnkCambiar");

                            //Validando que exista el Control
                            if (lnk != null)

                                //Habilitando Control
                                lnk.Enabled = indicador ? chk.Checked : false;
                        }
                    }
                }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvFacturasLigadas);
        }
        /// <summary>
        /// Método encargado de Obtener el Egreso de la Entidad
        /// </summary>
        /// <returns></returns>
        private int obtieneEgresoEntidad()
        {
            //Declarando Objeto de Retorno
            int id_egreso = 0;

            //Validando Comando
            switch (btnDepositarDepFac.CommandName)
            {
                case "Anticipo":
                    {
                        //Validando que haya Seleccion
                        if (gvDepositosPendientes.SelectedIndex != -1)
                            //Obteniendo Id de Egreso
                            id_egreso = Convert.ToInt32(gvDepositosPendientes.SelectedDataKey["IdEgreso"]);
                        break;
                    }
                case "Liquidacion":
                    {
                        //Validando que haya Seleccion
                        if (gvLiquidacionesPendientes.SelectedIndex != -1)
                        
                            //Obteniendo Id de Egreso
                            id_egreso = Convert.ToInt32(gvLiquidacionesPendientes.SelectedDataKey["IdEgreso"]);
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return id_egreso;
        }

        #endregion

        #endregion
    }
}
