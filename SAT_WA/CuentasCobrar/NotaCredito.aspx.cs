using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.ASP;
using TSDK.Datos;
using System.Data;
using System.Web.Hosting;

namespace SAT.CuentasCobrar
{
    public partial class NotaCredito : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al Efectuarse una Petición al Servidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVisorPestana_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            Button btn = (Button)sender;
            //Actualizando Busqueda
            actualizaBusqueda(btn.CommandName);
            //Inicializando Indices
            Controles.InicializaIndices(gvComprobantesSinPagar);
            Controles.InicializaIndices(gvNotasCredito);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            lblOrdenado.Text = lblOrdenadoNC.Text = lblOrdenadoCfdiRel.Text = "";

            //Validando Comando Activo
            switch (mtvComprobantesNC.ActiveViewIndex)
            {
                case 0:
                    //Actualizando Busqueda
                    actualizaBusqueda("CfdiPendientes");
                    Controles.InicializaIndices(gvComprobantesSinPagar);
                    break;
                case 1:
                    //Actualizando Busqueda
                    actualizaBusqueda("NotasCredito");
                    Controles.InicializaIndices(gvNotasCredito);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCambiarMonto_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvComprobantesSinPagar.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvComprobantesSinPagar, sender, "lnk", false);

                //Obteniendo Control
                using (TextBox txtMXA = (TextBox)gvComprobantesSinPagar.SelectedRow.FindControl("txtMXA"))
                using (LinkButton lnk = (LinkButton)sender)
                {
                    if (txtMXA != null && lnk != null)
                    {
                        //Validando el Comando del Control
                        switch (lnk.CommandName)
                        {
                            case "Guardar":
                                {
                                    RetornoOperacion retorno = new RetornoOperacion();
                                    int idComprobante = Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"]);
                                    //Recorriendo Filas
                                    foreach (DataRow dr in (((DataSet)Session["DS"]).Tables["Table"]).Select("Id = " + idComprobante.ToString()))
                                    {
                                        //Obteniendo Montos por Validar
                                        decimal monto_por_aplicar = 0.00M, monto_pendiente = 0.00M;
                                        decimal.TryParse(txtMXA.Text, out monto_por_aplicar);
                                        decimal.TryParse(dr["SaldoPendiente"].ToString(), out monto_pendiente);

                                        /** VALIDANDO MONTO DE APLICACION **/
                                        if (monto_por_aplicar > 0)
                                        {
                                            //Si el monto es mayor al permitido
                                            if (monto_por_aplicar > monto_pendiente)
                                                retorno = new RetornoOperacion(string.Format("El monto no debe de exceder la cantidad de '{0}'", monto_pendiente));
                                            
                                            //Si el monto es menor o igual
                                            else if (monto_por_aplicar <= monto_pendiente)
                                                retorno = new RetornoOperacion(idComprobante);
                                        }
                                        else
                                            retorno = new RetornoOperacion("Debe ingresar un monto valido por aplicar");

                                        //Deshabilitando el Control
                                        txtMXA.Enabled = false;
                                        //Configurando Control
                                        lnk.Text = lnk.CommandName = "Cambiar";

                                        //Validando operación
                                        if (retorno.OperacionExitosa)
                                        {
                                            //Aceptando Cambios
                                            dr["SaldoPorAplicar"] = string.Format("{0:0.00}", monto_por_aplicar);
                                            ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();
                                        }

                                        //Obtenemos Filas Seleccionadas
                                        GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvComprobantesSinPagar, "chkVarios");

                                        //Cargando GridView
                                        Controles.CargaGridView(gvComprobantesSinPagar, ((DataSet)Session["DS"]).Tables["Table"], "Id-SaldoPendiente-SaldoPorAplicar", "", true, 2);
                                        Controles.InicializaIndices(gvComprobantesSinPagar);

                                        //Marcando Filas
                                        marcaFilasSelecionadas(gvrs);

                                        //Mostrando Mensaje
                                        ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }

                                    break;
                                }
                            case "Cambiar":
                                {
                                    //Habilitando el Control
                                    txtMXA.Enabled = true;
                                    //Configurando Control
                                    lnk.Text = lnk.CommandName = "Guardar";
                                    Controles.InicializaIndices(gvComprobantesSinPagar);
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Exportar los Datos de los Comprobantes Sin Pagar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Validando Comando
            switch (((LinkButton)sender).CommandName)
            {
                case "CfdiDisponibles":
                    {
                        //Exportando Datos
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                        break;
                    }
                case "NotasCredito":
                    {
                        //Exportando Datos
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                        break;
                    }
                case "CfdiRelacionados":
                    {
                        //Exportando Datos
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Crear Nota de Credito"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearNC_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Filas Marcadas
            GridViewRow[] gvFilas = Controles.ObtenerFilasSeleccionadas(gvComprobantesSinPagar, "chkVarios");

            //Validando que existan Facturas Seleccionadas
            if (gvFilas.Length > 0)
            {
                //Declarando Variables Auxiliares
                List<Tuple<int, decimal>> cmps = new List<Tuple<int, decimal>>();
                int idMoneda = 0;

                //Recorriendo Comprobantes
                foreach (GridViewRow gvr in gvFilas)
                {
                    //Seleccionando Fila
                    gvComprobantesSinPagar.SelectedIndex = gvr.RowIndex;
                    //Asignando Comprobante
                    cmps.Add(new Tuple<int, decimal>(Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"]), Convert.ToDecimal(gvComprobantesSinPagar.SelectedDataKey["SaldoPorAplicar"])));
                }

                //Validando Moneda
                List<int> cfdis = (from Tuple<int, decimal> cfdi in cmps select cfdi.Item1).ToList();
                result = SAT_CL.FacturacionElectronica33.Comprobante.ValidaMonedaComprobantes(string.Join(",", cfdis.ToArray()), out idMoneda);

                //Validando Moneda de los Comprobantes
                if (result.OperacionExitosa)
                {
                    //Obteniendo Saldo Pendiente
                    decimal saldo_p = 0.00M;
                    //Partiendo Comprobantes
                    foreach (Tuple<int, decimal> cfdi in cmps)
                    {
                        //Instanciando Comprobante
                        using (SAT_CL.FacturacionElectronica33.Comprobante cmp = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(cfdi.Item1)))
                        {
                            //Validando Existencia del Comprobante
                            if (cmp.habilitar)
                            {
                                //Obteniendo Saldo Pendiente
                                decimal saldo_unitario_cmp = SAT_CL.FacturacionElectronica33.Reporte.ObtieneSaldoPendienteComprobante(cmp.id_comprobante33), saldo_por_aplicar = 0.00M;

                                //Si el saldo de mi Cfdi es mayor o igual a mi monto pendiente
                                if (cfdi.Item2 >= saldo_unitario_cmp)
                                    saldo_por_aplicar = saldo_unitario_cmp;
                                //Si el saldo de mi Cfdi es menor a mi monto pendiente
                                else if (cfdi.Item2 < saldo_unitario_cmp)
                                    saldo_por_aplicar = cfdi.Item2;

                                //Validando Saldo
                                if (saldo_por_aplicar > 0)
                                    //Añadiendo Saldo Unitario del Comprobante
                                    saldo_p += saldo_por_aplicar;
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion(string.Format("El Comprobante '{0}{1}' ya no tiene Saldo Pendiente", cmp.serie, cmp.folio));
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se puede recuperar el Comprobante");
                        }

                        //Si hay Errores
                        if (!result.OperacionExitosa)

                            //Terminando Ciclo
                            break;
                    }

                    //Validando Resultado
                    if (result.OperacionExitosa)
                    {
                        //Inicializando Registro de NC
                        inicializaRegistroNotaCredito(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), idMoneda, saldo_p);
                        //Asignando Comando
                        btnAceptar.CommandName = "VariosComprobantes";
                        //Mostrando la Ventana Modal
                        gestionaVentanaModal("NotaCredito", (Button)sender);
                    }

                    //Inicializa Indices
                    Controles.InicializaIndices(gvComprobantesSinPagar);
                }
                else
                    //Mostrando Excepción
                    result = new RetornoOperacion("No todas las Monedas de Comprobantes Coinciden");

            }
            else
                //Mostrando Excepción
                result = new RetornoOperacion("Debe seleccionar al menos 1 factura");

            //Mostrando Resultado Final
            ScriptServer.MuestraNotificacion(btnCrearNC, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Fecha del Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFechaTC_TextChanged(object sender, EventArgs e)
        {
            //Declarando variables Auxiliares
            int idCompania = ((SAT_CL.Global.CompaniaEmisorReceptor)Session["usuario_sesion"]).id_compania_emisor_receptor;

            //Asignando Fecha de Tipo de Cambio
            DateTime fecha_tc = DateTime.MinValue;
            DateTime.TryParse(txtFechaTC.Text, out fecha_tc);
            
            //Validando Existencia del Tipo de Cambio
            using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(idCompania, Convert.ToByte(ddlMonedaNC.SelectedValue), fecha_tc, 0))
            {
                //Si existe el Tipo de Cambio
                if (tc.habilitar)

                    //Asignando Valor TC
                    txtTipoCambioFE.Text = tc.valor_tipo_cambio.ToString();
                else
                    //Asignando Valor TC en Blanco
                    txtTipoCambioFE.Text = "";

                //Deshabilitando Campos
                txtFechaTC.Enabled =
                txtTipoCambioFE.Enabled = true;
                //Limpiando Controles
                txtFechaTC.Text = fecha_tc.ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar una Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Invocando Ventana Modal
            gestionaVentanaModal(lnk.CommandName, lnk);
        }

        #region Eventos GridView "Comprobantes Sin Pagar"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantesSinPagar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                /*/Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Validando Fila Obtenida
                if (row != null)
                {
                    //Obteniendo Controles
                    using (LinkButton lkbCambiarMonto = (LinkButton)e.Row.FindControl("lkbCambiarMonto"))
                    using (CheckBox chkVarios = (CheckBox)e.Row.FindControl("chkVarios"))
                    {
                        if (lkbCambiarMonto != null && chkVarios != null)
                        {
                            //Validando Monto por Aplicar
                            decimal monto_por_aplicar = 0.00M;
                            decimal.TryParse(row["SaldoPorAplicar"].ToString(), out monto_por_aplicar);
                            if (monto_por_aplicar > 0)
                            {
                                //Asignando Configuración de Controles para Edición
                                lkbCambiarMonto.Enabled = true;
                                chkVarios.Checked = true;
                            }
                            else
                            {
                                //Asignando Configuración de Controles para Edición
                                lkbCambiarMonto.Enabled = false;
                                chkVarios.Checked = false;
                            }
                        }
                    }
                }//*/
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "ComprobantesSinPagar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvComprobantesSinPagar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "ComprobantesSinPagar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantesSinPagar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de Página
            Controles.CambiaIndicePaginaGridView(gvComprobantesSinPagar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "ComprobantesSinPagar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantesSinPagar_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            Controles.CambiaSortExpressionGridView(gvComprobantesSinPagar, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Marcar los Checks del Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkVarios_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvComprobantesSinPagar.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;

                //Validando Identificador
                switch (chk.ID)
                {
                    case "chkTodos":
                        {
                            //Valiando Marcado de Check
                            if (chk.Checked)
                            {
                                //Recorriendo Filas
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)
                                {
                                    //Editando el Registro
                                    decimal monto_pend = 0.00M;
                                    decimal.TryParse(dr["SaldoPendiente"].ToString(), out monto_pend);
                                    dr["SaldoPorAplicar"] = string.Format("{0:0.00}", monto_pend);
                                }
                            }
                            else
                            {
                                //Recorriendo Filas
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)
                                    //Editando el Registro
                                    dr["SaldoPorAplicar"] = string.Format("{0:0.00}", 0.00M);
                            }

                            //Aceptando Cambios
                            ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                            //Cargando GridView
                            Controles.CargaGridView(gvComprobantesSinPagar, ((DataSet)Session["DS"]).Tables["Table"], "Id-SaldoPendiente-SaldoPorAplicar", "", true, 2);

                            //Marcando Todas las Filas
                            Controles.SeleccionaFilasTodas(gvComprobantesSinPagar, "chkVarios", chk.Checked);

                            //Marcando Filas
                            GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvComprobantesSinPagar, "chkVarios");
                            marcaFilasSelecionadas(gvr);
                            break;
                        }
                    case "chkVarios":
                        {
                            //Seleccionando Fila
                            Controles.SeleccionaFila(gvComprobantesSinPagar, sender, "chk", false);

                            //Valiando Marcado de Check
                            if (chk.Checked)
                            {
                                //Obteniendo Fila por Editar
                                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvComprobantesSinPagar.SelectedDataKey["Id"].ToString() + " ");
                                foreach (DataRow dr in drEdit)
                                {
                                    //Editando el Registro
                                    decimal monto_pend = 0.00M;
                                    decimal.TryParse(dr["SaldoPendiente"].ToString(), out monto_pend);
                                    dr["SaldoPorAplicar"] = string.Format("{0:0.00}", monto_pend);
                                }
                            }
                            else
                            {
                                //Obteniendo Fila por Editar
                                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvComprobantesSinPagar.SelectedDataKey["Id"].ToString() + " ");
                                foreach (DataRow dr in drEdit)
                                {
                                    //Editando el Registro
                                    decimal monto_pend = 0.00M;
                                    dr["SaldoPorAplicar"] = string.Format("{0:0.00}", monto_pend);
                                }
                            }

                            //Aceptando Cambios
                            ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                            //Obteniendo Filas Marcadas
                            GridViewRow[] gvFilas = Controles.ObtenerFilasSeleccionadas(gvComprobantesSinPagar, "chkVarios");

                            //Cargando GridView
                            Controles.CargaGridView(gvComprobantesSinPagar, ((DataSet)Session["DS"]).Tables["Table"], "Id-SaldoPendiente-SaldoPorAplicar", "", true, 2);
                            Controles.InicializaIndices(gvComprobantesSinPagar);

                            //Marcando Filas
                            marcaFilasSelecionadas(gvFilas);
                            break;
                        }
                }
            }
        }
        /*// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCrearNC_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvComprobantesSinPagar.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvComprobantesSinPagar, sender, "lnk", false);

                //Instanciando Comprobante
                using (SAT_CL.FacturacionElectronica33.Comprobante cmp = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"])))
                {
                    //Validando Existencia del Comprobante
                    if (cmp.habilitar)
                    {
                        //Obteniendo Saldo Pendiente
                        decimal saldo_p = SAT_CL.FacturacionElectronica33.Reporte.ObtieneSaldoPendienteComprobante(cmp.id_comprobante33);

                        //Validando Saldo
                        if (saldo_p > 0)
                        {
                            //Inicializando Registro de NC
                            inicializaRegistroNotaCredito(cmp.id_compania_receptor, cmp.id_moneda, saldo_p);
                            //Asignando Comando
                            btnAceptar.CommandName = "Comprobante";
                            //Mostrando la Ventana Modal
                            gestionaVentanaModal("NotaCredito", (LinkButton)sender);
                        }
                        else
                            //Instanciando Excepción
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Comprobante ya no tiene Saldo Pendiente"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Comprobante"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }//*/
        /// <summary>
        /// Evento Producido al Aceptar crear la Nota de Credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //Declarando Objetos de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            List<Tuple<int, decimal>> cfdi = new List<Tuple<int, decimal>>();
            DateTime fec_tc;
            DateTime.TryParse(txtFechaTC.Text, out fec_tc);
            int idCliente = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1));
            int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
            int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
            
            //Obteniendo Control
            Button btn = (Button)sender;

            //Validando Comando
            switch (btn.CommandName)
            {
                /*//
                case "Comprobante":
                    {
                        //Validando Comprobantes
                        if (Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"]) > 0)
                        {
                            //Añadiendo a Lista
                            cfdi.Add(Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"]));

                            //Invocando Método de Nota de Credito
                            result = SAT_CL.FacturacionElectronica33.Comprobante.CreaNotaCreditoCxC(idCompania, idCliente, Convert.ToInt32(ddlFormaPago.SelectedValue),
                                        Convert.ToInt32(ddlMetodoPago.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue), fec_tc, Convert.ToDecimal(txtTipoCambioFE.Text),
                                        cfdi, txtSerieFE.Text, chkAplicar.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                        HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"),
                                        idUsuario);
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede recuperar el Comprobante");
                        break;
                    }//*/
                case "VariosComprobantes":
                    {
                        //Obteniendo Filas Marcadas
                        GridViewRow[] gvFilas = Controles.ObtenerFilasSeleccionadas(gvComprobantesSinPagar, "chkVarios");

                        //Validando que existan Facturas Seleccionadas
                        if (gvFilas.Length > 0)
                        {
                            //Recorriendo Comprobantes
                            foreach (GridViewRow gvr in gvFilas)
                            {
                                //Seleccionando Fila
                                gvComprobantesSinPagar.SelectedIndex = gvr.RowIndex;
                                //Asignando Datos de Interes
                                cfdi.Add(new Tuple<int, decimal>(Convert.ToInt32(gvComprobantesSinPagar.SelectedDataKey["Id"]), Convert.ToDecimal(gvComprobantesSinPagar.SelectedDataKey["SaldoPorAplicar"])));
                            }

                            //Validando si hay comprobantes
                            if (cfdi.Count > 0)
                                //Invocando Método de Nota de Credito
                                result = SAT_CL.FacturacionElectronica33.Comprobante.CreaNotaCreditoCxC(idCompania, idCliente, Convert.ToInt32(ddlFormaPago.SelectedValue),
                                            Convert.ToInt32(ddlMetodoPago.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue), fec_tc, Convert.ToDecimal(txtTipoCambioFE.Text),
                                            cfdi, txtSerieFE.Text, chkAplicar.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                            HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"),
                                            idUsuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No se pueden recuperar los Comprobantes");
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No hay Comprobantes seleccionados");
                        

                        break;
                    }
            }
            
            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Cargando Comprobantes
                buscaComprobantesConSaldo();
                //Inicializando indices
                Controles.InicializaIndices(gvComprobantesSinPagar);
                //Mostrando la Ventana Modal
                gestionaVentanaModal("NotaCredito", btnAceptar);
            }
            
            //Mostrando Resultado
            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar la creación de la Nota de Credito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Mostrando la Ventana Modal
            gestionaVentanaModal("NotaCredito", btnCancelar);
        }

        #endregion

        #region Eventos GridView "Notas de Credito"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoNC_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvNotasCredito, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoNC.SelectedValue), true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNotasCredito_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de Página
            Controles.CambiaIndicePaginaGridView(gvNotasCredito, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNotasCredito_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoNC.Text = Controles.CambiaSortExpressionGridView(gvNotasCredito, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbDescargas_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvNotasCredito.DataKeys.Count > 0)
            {
                //Selecionando Fila
                Controles.SeleccionaFila(gvNotasCredito, sender, "imb", false);

                //Instanciando CFDI
                using (SAT_CL.FacturacionElectronica33.Comprobante cfdi = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvNotasCredito.SelectedDataKey["Id"])))
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                {
                    //Validando Existencia
                    if (cfdi.habilitar && emisor.habilitar)
                    {
                        //Validando Control
                        switch (((ImageButton)sender).CommandName)
                        {
                            case "PDF":
                                {
                                    //Obteniendo PDF
                                    TSDK.Base.Archivo.DescargaArchivo(cfdi.GeneraPDFComprobanteV33(), string.Format("{0}_{1}{2}.pdf", emisor.nombre_corto.Equals("") ? emisor.rfc : emisor.nombre_corto, cfdi.serie, cfdi.folio), Archivo.ContentType.binary_octetStream);
                                    break;
                                }
                            case "XML":
                                {
                                    //Obteniendo PDF
                                    TSDK.Base.Archivo.DescargaArchivo(System.IO.File.ReadAllBytes(cfdi.ruta_xml), string.Format("{0}_{1}{2}.xml", emisor.nombre_corto.Equals("") ? emisor.rfc : emisor.nombre_corto, cfdi.serie, cfdi.folio), Archivo.ContentType.binary_octetStream);
                                    break;
                                }
                        }
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el comprobante"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAplicacionesNC_Click(object sender, EventArgs e)
        {
            //Validando Datos
            if (gvNotasCredito.DataKeys.Count > 0)
            {
                //Selecionando Fila
                Controles.SeleccionaFila(gvNotasCredito, sender, "lnk", false);

                //Cargando CFDI's Relacionados
                cargaCfdiRelacionados();

                //Mostrando Ventana
                gestionaVentanaModal("CfdiRelacionados", this.Page);
            }
        }

        #endregion

        #region Eventos GridView "Cfdi's Relacionados"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCfdiRelacionados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando indice de Página
            Controles.CambiaIndicePaginaGridView(gvCfdiRelacionados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCfdiRelacionados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoCfdiRel.Text = Controles.CambiaSortExpressionGridView(gvCfdiRelacionados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCfdiRel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCfdiRelacionados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoCfdiRel.SelectedValue), true, 2);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializando Valores
            inicializaControles();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCfdiRel, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoNC, "", 26);
            //Cargando Tipos de Moneda (REGULADO POR EL SAT V3.3 DE CFDI)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMonedaNC, 110, "");
            //Cargando Tipos de Forma de Pago (REGULADO POR EL SAT V3.3 DE CFDI)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "");
            //Cargando Tipos de Forma de Pago (REGULADO POR EL SAT V3.3 DE CFDI)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 3195);
            //Cargando Tipo de Nota de Credito
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 17, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            ddlConcepto.SelectedValue = SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Concepto Cobro Nota de Credito v3.3", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
        }
        /// <summary>
        /// Método encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            //Inicializando GridView
            Controles.InicializaGridview(gvComprobantesSinPagar);
            Controles.InicializaGridview(gvNotasCredito);

            //Inicializando controles de Pestaña
            btnCfdiDisponibles.CssClass = "boton_pestana_activo";
            btnNotasCredito.CssClass = "boton_pestana";
            mtvComprobantesNC.ActiveViewIndex = 0;

            //Inicializando Controles de Fechas
            chkIncluir.Checked = true;
            txtFechaIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método encargado de Obtener los Comprobantes con Saldo Pendiente
        /// </summary>
        private void buscaComprobantesConSaldo()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;

            //Validando Si se requieren Fechas
            if (chkIncluir.Checked)
            {   
                //Obteniendo Fechas
                DateTime.TryParse(txtFechaIni.Text, out fec_ini);
                DateTime.TryParse(txtFechaFin.Text, out fec_fin);
            }

            //Obteniendo Datos de Interes
            using (DataTable dtCFDIConSaldo = SAT_CL.FacturacionElectronica33.Reporte.ObtieneComprobantesConSaldo(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtSerie.Text, 
                                                                                                            Convert.ToInt32(txtFolio.Text.Equals("") ? "0" : txtFolio.Text), fec_ini, fec_fin))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtCFDIConSaldo))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvComprobantesSinPagar, dtCFDIConSaldo, "Id-SaldoPendiente-SaldoPorAplicar", lblOrdenado.Text, true, 2);
                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCFDIConSaldo, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvComprobantesSinPagar);
                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método encargado de Obtener las Notas de Credito
        /// </summary>
        private void buscaNCTimbradas()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;

            //Validando Si se requieren Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFechaIni.Text, out fec_ini);
                DateTime.TryParse(txtFechaFin.Text, out fec_fin);
            }

            //Obteniendo Datos de Interes
            using (DataTable dtNotasCredito = SAT_CL.Facturacion.Reporte.ObtieneCfdiNotasCredito(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtSerie.Text,
                                                                                                Convert.ToInt32(txtFolio.Text.Equals("") ? "0" : txtFolio.Text), fec_ini, fec_fin))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtNotasCredito))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvNotasCredito, dtNotasCredito, "Id-IdFacturado", lblOrdenadoNC.Text, true, 2);

                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtNotasCredito, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvNotasCredito);

                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método encargado de Mostrar los CFDI's Amparados 
        /// </summary>
        private void cargaCfdiRelacionados()
        {
            //Obteniendo Datos de Interes
            using (DataTable dtCfdiRelacionados = SAT_CL.Facturacion.Reporte.ObtieneCfdiAmparadosNC(Convert.ToInt32(gvNotasCredito.SelectedDataKey["Id"])))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dtCfdiRelacionados))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvCfdiRelacionados, dtCfdiRelacionados, "Id", lblOrdenadoCfdiRel.Text, true, 2);

                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCfdiRelacionados, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvCfdiRelacionados);

                    //Asignando a Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Método encargado de Marcar las Filas despues de una Carga de Grid
        /// </summary>
        /// <param name="gvrs"></param>
        private void marcaFilasSelecionadas(GridViewRow[] gvrs)
        {
            //Validando que Existan Filas
            if (gvrs.Length > 0)
            {
                //Obteniendo indices
                List<int> idxs = (from GridViewRow gvr in gvrs
                                  select gvr.RowIndex).ToList();
                foreach (int idx in idxs)
                {
                    gvComprobantesSinPagar.SelectedIndex = idx;
                    //Obteniendo Control
                    CheckBox chkFila = (CheckBox)gvComprobantesSinPagar.SelectedRow.FindControl("chkVarios");

                    //Validando que exista el Control
                    if (chkFila != null)
                    {
                        //Marcando Control
                        chkFila.Checked = true;

                        //Obteniendo Control
                        LinkButton lnk = (LinkButton)gvComprobantesSinPagar.SelectedRow.FindControl("lkbCambiarMonto");

                        //Validando que exista el Control
                        if (lnk != null)

                            //Habilitando Control
                            lnk.Enabled = true;
                    }
                }
                Controles.InicializaIndices(gvComprobantesSinPagar);
            }
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="nombre_ventana">Nombre de la Ventana a Mostrar/Ocultar</param>
        /// <param name="sender">Control que dispara el Evento</param>
        private void gestionaVentanaModal(string nombre_ventana, Control sender)
        {
            //Validando Ventana
            switch (nombre_ventana)
            {
                case "NotaCredito":
                    //Alternando Ventana
                    ScriptServer.AlternarVentana(sender, nombre_ventana, "contenedorVentanaTimbraNotaCredito", "ventanaTimbraNotaCredito");
                    break;
                case "CfdiRelacionados":
                    //Alternando Ventana
                    ScriptServer.AlternarVentana(sender, nombre_ventana, "contenedorVentanaCfdiRelacionados", "ventanaCfdiRelacionados");
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Inicializar los Controles de Registro de la Nota de Credito
        /// </summary>
        /// <param name="id_cliente">Cliente</param>
        /// <param name="id_moneda">Moneda de la Operación</param>
        /// <param name="monto_nc">Monto</param>
        private void inicializaRegistroNotaCredito(int id_cliente, int id_moneda, decimal monto_nc)
        {
            //Declarando variables Auxiliares
            int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
            
            //Asignando Fecha de Tipo de Cambio
            DateTime fecha_tc = Fecha.ObtieneFechaEstandarMexicoCentro();

            //Validando Moneda
            if (id_moneda != 1)
            {
                //Validando Existencia del Tipo de Cambio
                using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(idCompania, (byte)id_moneda, fecha_tc, 0))
                {
                    //Si existe el Tipo de Cambio
                    if (tc.habilitar)

                        //Asignando Valor TC
                        txtTipoCambioFE.Text = tc.valor_tipo_cambio.ToString();
                    else
                        //Asignando Valor TC en Blanco
                        txtTipoCambioFE.Text = "";

                    //Deshabilitando Campos
                    txtFechaTC.Enabled =
                    txtTipoCambioFE.Enabled = true;
                    //Limpiando Controles
                    txtFechaTC.Text = fecha_tc.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                //Deshabilitando Campos
                txtFechaTC.Enabled =
                txtTipoCambioFE.Enabled = false;
                //Limpiando Controles
                txtFechaTC.Text = "";
                txtTipoCambioFE.Text = "1";
            }
            
            //Cargando Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", id_cliente, "", 0, "");
            //Obteniendo Items
            ListItem liFormaPago = ddlFormaPago.Items.FindByText("Por definir [99]");
            ListItem liMetodoPago = ddlMetodoPago.Items.FindByText("Pago en parcialidades o diferido");
            ListItem liUsoCFDI = ddlUsoCFDI.Items.FindByText("[P01] Por definir");

            //Validando Item FP
            if (liFormaPago != null)
                //Asignando Forma de Pago
                ddlFormaPago.SelectedValue = liFormaPago.Value;
            //Validando Item MP
            if (liMetodoPago != null)
                //Asignando Método de Pago
                ddlMetodoPago.SelectedValue = liMetodoPago.Value;
            //Validando Item Uso CFDI
            if (liUsoCFDI != null)
                //Asignando Uso del CFDI
                ddlUsoCFDI.SelectedValue = liUsoCFDI.Value;

            //Asignando Moneda
            ddlMonedaNC.SelectedValue = id_moneda.ToString();

            //Asignando Monto
            txtMonto.Text = monto_nc.ToString();

            //Habilitando Opción de Aplicación
            chkAplicar.Checked = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comando"></param>
        private void actualizaBusqueda(string comando)
        {
            //Validando Comando
            switch (comando)
            {
                case "CfdiPendientes":
                    {
                        //Inicializando controles de Pestaña
                        btnCfdiDisponibles.CssClass = "boton_pestana_activo";
                        btnNotasCredito.CssClass = "boton_pestana";
                        mtvComprobantesNC.ActiveViewIndex = 0;

                        //Buscando Comprobantes
                        buscaComprobantesConSaldo();
                        break;
                    }
                case "NotasCredito":
                    {
                        //Inicializando controles de Pestaña
                        btnCfdiDisponibles.CssClass = "boton_pestana";
                        btnNotasCredito.CssClass = "boton_pestana_activo";
                        mtvComprobantesNC.ActiveViewIndex = 1;

                        //Buscando Comprobantes
                        buscaNCTimbradas();
                        break;
                    }

            }
        }

        #endregion

        protected void btnNC_Click(object sender, EventArgs e)
        {
            int id_fac_global = 0;
            int.TryParse(txtFolio.Text, out id_fac_global);
            RetornoOperacion retorno = new RetornoOperacion();

            if (id_fac_global > 0)
            {
                retorno = SAT_CL.Facturacion.Facturado.CopiaFacturado(SAT_CL.FacturacionElectronica33.Comprobante.OrigenDatos.FacturaGlobal, id_fac_global, new List<Tuple<int, int, int>>(), 1);
            }
            else
                retorno = new RetornoOperacion("No hay Factura");

            ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
    }
}