using SAT_CL.CXP;
using SAT_CL.EgresoServicio;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.EgresoServicio
{
    public partial class DieselFactura : System.Web.UI.Page
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
            if (!(Page.IsPostBack))
                
                //Inicialiando Página
                inicializaPagina();
        }


        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFactura_Click(object sender, EventArgs e)
        {
            //Inicializamos Indices
            gvVales.EditIndex = -1;
            //Invocando Método de Busqueda
            buscaFacturas();
            //Busca Vales
            buscaVales();
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturas);
            //Inicializando Reporte
            TSDK.ASP.Controles.InicializaGridview(gvValesAsignados);
           
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar Vale"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarVale_Click(object sender, EventArgs e)
        {   
            //Validando que existe una Estación de Combustible
            if (ddlEstacionCombustible.SelectedValue != "0")
            {
                //Inicializamos Indices
                gvVales.EditIndex = -1;
                //Invocando Método de Busqueda
                buscaVales();
                //Inicialiamos Indices
                Controles.InicializaIndices(gvVales);
            }
            else
                //Mostrando Error
                ScriptServer.MuestraNotificacion(btnBuscarVale, "Debe seleccionar una Estación de Combustible", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
               
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Asignar Vales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarVales_Click(object sender, EventArgs e)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que exista una Factura Seleccionada
            if (gvFacturas.SelectedIndex != -1)
            {   
                //Obteniendo Filas Seleccionadas
                GridViewRow[] filas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvVales, "chkVarios");
                
                //Validando que existan Vales Seleccionados
                if(filas.Length > 0)
                {   
                    //Inicializando Bloque Transacional
                    using(TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {   
                        //Instanciando la Factura
                        using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                        {   
                            //Validando que exista la Factura
                            if (fp.id_factura != 0)
                            {   
                                //Declarando Contador
                                int contador = 0;
                                //Iniciando Ciclo
                                while (contador < filas.Length)
                                {   
                                    //Obteniendo Indice
                                    gvVales.SelectedIndex = filas[contador].RowIndex;
                                    
                                    //Instanciando 
                                    using (AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(gvVales.SelectedDataKey["Id"])))
                                    {   
                                        //Validando que exista el Vale
                                        if (ad.id_asignacion_diesel != 0)
                                        {   
                                            //Asignando el Vale a la Factura
                                            result = ad.AsignaFacturaValeDiesel(fp.id_factura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //Validando que la Operación haya sido exitosa
                                            contador = result.OperacionExitosa ? contador + 1 : filas.Length;
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("No existe el Vale");
                                    }
                                }
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe la Factura");
                        }
                        
                        //Validando que la Operación haya sido exitosa
                        if (result.OperacionExitosa)
                            
                            //Completando Transacción
                            trans.Complete();
                    }
                    
                    //Validando el Resultado de la Operación
                    if(result.OperacionExitosa)
                    {
                        //Obteniendo Factura de Proveedor
                        int idFacturaProveedor = Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]);
                        
                        //Vargando Reportes
                        buscaVales();
                        buscaFacturas();
                        
                        //Marcando Fila
                        Controles.MarcaFila(gvFacturas, idFacturaProveedor.ToString(), "Id", "Id-Monto-Asignado", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenadoFactura.Text, Convert.ToInt16(ddlTamanoFac.SelectedValue), true, 1);

                        //Recargando Vales Asignados
                        buscaValesAsignados();
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe seleccionar al menos un Vale");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe Seleccionar una Factura");
            
            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(btnAsignarVales, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #region GridView "Factura"

        /// <summary>
        /// Evento Producido al Presionar el Boton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionar_Click(object sender, EventArgs e)
        {   
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);
            //cargando Reporte
            buscaValesAsignados();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Ordenamiento del GridView
            lblOrdenadoFactura.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Controles
                LinkButton lnk = (LinkButton)e.Row.FindControl("lkbSeleccionar");

                //Validando que Exista el Control
                if(lnk != null)
                {
                    //Obteniendo Origen de la Fila
                    DataRowView rowView = (DataRowView)e.Row.DataItem;

                    //Asignando Habilitación del Control
                    lnk.Enabled = rowView["Estatus"].ToString() == "OK" ? false : true;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFac_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvVales, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamanoFac.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFactura_Click(object sender, EventArgs e)
        {   
            //Exportando Excel
            Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cargando Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionCombustible, 30, "", Convert.ToInt32(ddlProveedor.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }

        #region Eventos Vales Asignados

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Vales Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValesAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvValesAsignados, ((DataSet)Session["DS"]).Tables["Table2"], e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Vales Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvValesAsignados_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Ordenamiento del GridView
            lblOrdenadoVale.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvValesAsignados, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoValesAsig_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvValesAsignados, ((DataSet)Session["DS"]).Tables["Table2"], Convert.ToInt32(ddlTamanoValesAsig.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Vales Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarValesAsig_Click(object sender, EventArgs e)
        {   
            //Exportando Excel
            Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table2"], "Id");
        }

        #endregion

        #endregion        

        #region Eventos GridView "Vales"

        /// <summary>
        /// Evento Producido al Seleccionar un Control CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
            CheckBox chkHeader = (CheckBox)gvVales.HeaderRow.FindControl("chkTodos");

            //Evalua el ID del CheckBox en el que se produce el cambio
            switch (((CheckBox)sender).ID)
            {   
                //Caso para el CheckBox "Todos"
                case "chkTodos":
                    {   
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvVales, "chkVarios", chkHeader.Checked);
                        break;
                    }
                case "chkVarios":
                    {
                        //Obteniendo Control
                        CheckBox chk = (CheckBox)sender;

                        //Declarando Variable Auxiliar
                        GridViewRow[] gvr = null;
                        
                        //Validando que exista el Control
                        if (chk != null)
                        {
                            //Validando que Exista una Factura Seleccionada
                            if (gvFacturas.SelectedIndex != -1)
                            {
                                //Obteniendo Monto Disponible
                                decimal monto_disponible_factura = Convert.ToDecimal(gvFacturas.SelectedDataKey["Monto"]) - Convert.ToDecimal(gvFacturas.SelectedDataKey["Asignado"]);
                                decimal monto_acumulado = 0.00M;
                                decimal monto_disponible = 0.00M;

                                //Obteniendo Filas Seleccionadas
                                gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvVales, "chkVarios");

                                //Validando que haya sido marcado el Control
                                if(chk.Checked)
                                {
                                    //Validando que Existan Filas
                                    if(gvr.Length > 0)
                                    {
                                        //Recorriendo Cada Fila
                                        foreach (GridViewRow gr in gvr)
                                        {
                                            //Selecionando Fila
                                            gvVales.SelectedIndex = gr.RowIndex;

                                            //Sumando Total
                                            monto_acumulado = monto_acumulado + Convert.ToDecimal(gvVales.SelectedDataKey["Monto"]);
                                        }
                                    }

                                    //Calculando Monto Disponible
                                    monto_disponible = monto_disponible_factura - monto_acumulado;

                                    //Validando que el monto de los vales anteriores no excedan el monto disponible
                                    if(monto_disponible <= 0)
                                    
                                    {
                                        //Desmarcando el Control
                                        chk.Checked = false;

                                        //Mostrando Error
                                        ScriptServer.MuestraNotificacion(gvVales, "El monto del Vale excede el Monto de la Factura.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
               

                                     
                                    }
                                }

                                //Validando que se encuentren todas las filas seleccionadas
                                if (gvr.Length == gvVales.Rows.Count)

                                    //Marcando Encabezado
                                    chkHeader.Checked = true;
                                else
                                    //Desmarcando Encabezado
                                    chkHeader.Checked = false;

                                //Seleccionando Fila
                                TSDK.ASP.Controles.SeleccionaFila(gvVales, sender, "chk", false);

                                //Obteniendo Control "LinkButton"
                                LinkButton lnk = (LinkButton)gvVales.SelectedRow.FindControl("lnkEditar");

                                //Validando que exista el Control
                                if (chk != null && lnk != null)
                                {
                                    //Validando que el Control este Marcado
                                    if (chk.Checked)

                                        //Deshabilitando Control
                                        lnk.Enabled = false;
                                    else
                                        //Deshabilitando Control
                                        lnk.Enabled = true;
                                }

                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvVales);
                            }
                            else
                            {
                                //Desmarcando Casilla
                                chk.Checked = false;

                                //Mostrando Error
                                ScriptServer.MuestraNotificacion(gvVales, "Debe Seleccionar una Factura", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVales_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Ordenamiento del GridView
            lblOrdenadoVale.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvVales, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvVales, ((DataSet)Session["DS"]).Tables["Table1"], e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoVale_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvVales, ((DataSet)Session["DS"]).Tables["Table1"], Convert.ToInt32(ddlTamanoVale.SelectedValue), true, 3);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarVale_Click(object sender, EventArgs e)
        {   
            //Exportando Excel
            Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table1"], "Id");
        }

        #region Edición Vale

        /// <summary>
        /// Evento generado al Editar un Vales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarE_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvVales.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvVales, sender, "lnk", true);
                //Carga Vales
                buscaVales();
                //Inicializamos Valores
                using (SAT_CL.EgresoServicio.AsignacionDiesel objVale = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvVales.SelectedValue)))
                {
                    //Instanciamos Detalle Liquidacion
                    using(SAT_CL.EgresoServicio.DetalleLiquidacion objDetalleLiquidacion = new DetalleLiquidacion( objVale.id_asignacion_diesel, 69))
                    //Instanciamos Litrsob
                    using (TextBox txtLitrosE = (TextBox)gvVales.SelectedRow.FindControl("txtLitrosE"))
                    {
                        using (TextBox txtCCombustible = (TextBox)gvVales.SelectedRow.FindControl("txtCCombustibleE"))
                        {
                            using (TextBox txtFechaCargaE = (TextBox)gvVales.SelectedRow.FindControl("txtFechaE"))
                            {
                                //Inicializamos Valores
                                txtLitrosE.Text = objDetalleLiquidacion.cantidad.ToString();
                                txtCCombustible.Text = objDetalleLiquidacion.valor_unitario.ToString();
                                txtFechaCargaE.Text = objVale.fecha_carga.ToString("dd/MM/yyyy HH:mm");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al Guardar un Vale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGuardarE_Click(object sender, EventArgs e)
        {
            //Evento general al Guardar el Vale
            editaVale();
        }
        
        /// <summary>
        /// Evento generado al Cancelar un Vale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCancelarE_Click(object sender, EventArgs e)
        {
            //Inicializamos Indice
            Controles.InicializaIndices(gvVales);
            //Cargamos Vales
            buscaVales();
        }
        
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCancelar_Click(object sender, EventArgs e)
        {   
            //Validando que contenga campos Llave
            if (gvVales.DataKeys.Count > 0)
            {   
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvVales);
            }
        }

        /// <summary>
        /// Evento generado al  Quitar un Vale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkQuitar_Click(object sender, EventArgs e)
        { 
            //Declaramos objetpo Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si hay registros
            if (gvValesAsignados.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvValesAsignados, sender, "lnk", false);
                //Inicializamos Valores
                using (SAT_CL.EgresoServicio.AsignacionDiesel objVale = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvValesAsignados.SelectedValue)))
                {                     
                    //Factura
                    resultado = objVale.QuitaFacturaValeDiesel(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if(resultado.OperacionExitosa)
                    {
                        //Inicializamos Inidices
                        Controles.InicializaIndices(gvValesAsignados);
                        //Cargamos Vales Asignados
                        buscaValesAsignados();
                        //Cargamos Fcaturas
                        buscaFacturas();
                        //Busca Vales
                        buscaVales();

                    }
                    //Mostrando Resultado
                    ScriptServer.MuestraNotificacion(gvValesAsignados, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {   
            //Cargando Catalogos
            cargaCatalogos();
            //Inicializando GridViews
            TSDK.ASP.Controles.InicializaGridview(gvFacturas);
            TSDK.ASP.Controles.InicializaGridview(gvVales);
            TSDK.ASP.Controles.InicializaGridview(gvValesAsignados);
            txtFechaCarga.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {   
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFac, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoVale, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoValesAsig, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 29, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacionCombustible, 30, "-- Seleccione una Estación", Convert.ToInt32(ddlProveedor.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {   
            //Obteniendo Reporte
            using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneReporteFacturasDiesel(ddlProveedor.SelectedValue,
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString(), txtSerie.Text,
                                        Convert.ToInt32(txtFolio.Text == ""? "0":txtFolio.Text),""))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvFacturas, dtFacturas, "Id-Monto-Asignado", lblOrdenadoFactura.Text, true, 1);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvFacturas);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar los Vales de Diesel
        /// </summary>
        private void buscaVales()
        {   
            //Declaramos Variable para la Fecha de Carga
            DateTime fecha_carga_inicio = DateTime.MinValue;
            //Declaramos Variable para la Fecha de Carga
            DateTime fecha_carga_fin = DateTime.MinValue;
            //Si esta Marcada la Fecha de Carga
            if(chkIncluir.Checked)
            {
                fecha_carga_inicio = Convert.ToDateTime(txtFechaCarga.Text);
                fecha_carga_fin = Convert.ToDateTime(txtFechaCarga.Text).AddHours(23).AddMinutes(59);
            }
            //Declaramos Variable Para Estación de Combustible
            int idEstacionCombustible = 0;
            //Si no existe Estación de Combustible
            if(ddlEstacionCombustible.SelectedValue =="")
            {
                ddlEstacionCombustible.SelectedValue = "-1";
            }
            else
            {
                idEstacionCombustible = Convert.ToInt32(ddlEstacionCombustible.SelectedValue);
            }
            //Obteniendo Reporte
            using (DataTable dtVales = SAT_CL.EgresoServicio.Reportes.ObtieneValesDiesel(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    txtNoVale.Text, idEstacionCombustible, fecha_carga_inicio, fecha_carga_fin))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtVales))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvVales, dtVales, "Id-Monto",lblOrdenadoVale.Text, true, 3);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtVales, "Table1");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvVales);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }               
            }
            
        }

        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaValesAsignadosTotales()
        { 
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            {
                //Mostrando Totales
                gvValesAsignados.FooterRow.Cells[6].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(Monto)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvValesAsignados.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar los Vales de Diesel Asignados a una Factura
        /// </summary>
        private void buscaValesAsignados()
        {   
            //Obteniendo Facturas Seleccionadas
            using (DataTable dtValesAsignados = AsignacionDiesel.ObtieneValesPorFactura(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
            {   
                //Validando que existan los Vales Asignados
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtValesAsignados))
                {   
                    //Cargando Vales Seleccionados
                    TSDK.ASP.Controles.CargaGridView(gvValesAsignados, dtValesAsignados, "Id", lblOrdenadoValesAsig.Text, true, 2);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtValesAsignados, "Table2");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvValesAsignados);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Método encargado de Sumar Totales
            sumaValesAsignadosTotales();
        }

        /// <summary>
        /// Metodo encargado de Editar el Vale
        /// </summary>
        private void editaVale()
        {
            //Declaracion de objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            using (TextBox txtLitrosE = (TextBox)gvVales.SelectedRow.FindControl("txtlitrosE"))
            {
                using (TextBox txtCCombustible = (TextBox)gvVales.SelectedRow.FindControl("txtCCombustibleE"))
                {
                    using (TextBox txtFechaCargaE = (TextBox)gvVales.SelectedRow.FindControl("txtFechaE"))
                    {
                        //Editamos Vale
                        using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvVales.SelectedDataKey.Value)))
                        {
                            //Validando que exista la Asignación
                            if (ad.id_asignacion_diesel != 0)
                            {
                                //Obteniendo Fecha
                                string s = "";
                                DateTime fec_carga;
                                DateTime.TryParse(txtFechaCargaE.Text, out fec_carga);

                                //Instanciando Costo de Combustible
                                using (CostoCombustible cc = new CostoCombustible(ad.id_costo_diesel))
                                {
                                    //Validando que exista una Fecha de Carga
                                    if (fec_carga != DateTime.MinValue)
                                    {
                                        //Si existe el Costo del Combustible
                                        if (cc.habilitar)
                                        {
                                            //Comparando Fechas con respecto al costo de diesel
                                            if (cc.fecha_inicio.CompareTo(fec_carga) < 0 && cc.fecha_fin.CompareTo(fec_carga) > 0)
                                            {
                                                //Editando Asignación
                                                resultado = ad.EditaAsignacionDiesel(ad.nombre_operador_proveedor, ad.id_compania_emisor, ad.id_ubicacion_estacion, ad.fecha_solicitud,
                                                                    fec_carga, ad.id_costo_diesel, ad.id_tipo_combustible, ad.id_factura, ad.bit_transferencia_contable, ad.referencia, ad.id_lectura,
                                                                    ad.id_deposito, ad.tipo_vale, Convert.ToDecimal(txtLitrosE.Text == "" ? "0" : txtLitrosE.Text), cc.costo_combustible,
                                                                    ad.objDetalleLiquidacion.id_unidad, ad.objDetalleLiquidacion.id_operador, ad.objDetalleLiquidacion.id_proveedor_compania,
                                                                    ad.objDetalleLiquidacion.id_servicio, ad.objDetalleLiquidacion.id_movimiento, ad.id_unidad_diesel,
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                            else
                                                //Instanciando Excepcion
                                                resultado = new RetornoOperacion("La Fecha no esta en el periodo del Costo del Combustible");
                                        }
                                        else
                                            //Editando Asignación
                                            resultado = ad.EditaAsignacionDiesel(ad.nombre_operador_proveedor, ad.id_compania_emisor, ad.id_ubicacion_estacion, ad.fecha_solicitud,
                                                                fec_carga, ad.id_costo_diesel, ad.id_tipo_combustible, ad.id_factura, ad.bit_transferencia_contable, ad.referencia, ad.id_lectura,
                                                                ad.id_deposito, ad.tipo_vale, Convert.ToDecimal(txtLitrosE.Text == "" ? "0" : txtLitrosE.Text),
                                                                Convert.ToDecimal(txtCCombustible.Text == "" ? "0" : txtCCombustible.Text), ad.objDetalleLiquidacion.id_unidad, ad.objDetalleLiquidacion.id_operador,
                                                                ad.objDetalleLiquidacion.id_proveedor_compania, ad.objDetalleLiquidacion.id_servicio,
                                                                ad.objDetalleLiquidacion.id_movimiento, ad.id_unidad_diesel, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    }
                                    else
                                        //Editando Asignación
                                        resultado = ad.EditaAsignacionDiesel(ad.nombre_operador_proveedor, ad.id_compania_emisor, ad.id_ubicacion_estacion, ad.fecha_solicitud,
                                                                fec_carga, ad.id_costo_diesel, ad.id_tipo_combustible, ad.id_factura, ad.bit_transferencia_contable, ad.referencia, ad.id_lectura,
                                                                ad.id_deposito, ad.tipo_vale, Convert.ToDecimal(txtLitrosE.Text == "" ? "0" : txtLitrosE.Text),
                                                                ad.objDetalleLiquidacion.valor_unitario, ad.objDetalleLiquidacion.id_unidad, ad.objDetalleLiquidacion.id_operador,
                                                                ad.objDetalleLiquidacion.id_proveedor_compania, ad.objDetalleLiquidacion.id_servicio,
                                                                ad.objDetalleLiquidacion.id_movimiento, ad.id_unidad_diesel, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }

                                //Validando que la Operación sea Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvVales);
                                    gvVales.EditIndex = -1;
                                    //Realizando Busqueda
                                    buscaVales();
                                }
                                else
                                    //Recargando el Contenido del GridView
                                    TSDK.ASP.Controles.CargaGridView(gvVales, ((DataSet)Session["DS"]).Tables["Table1"], "Id", "", true, 1);
                            }

                            else
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe el Vale");

                            //Mostrando Resultado
                            ScriptServer.MuestraNotificacion(gvVales, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }
        }

        #endregion

        
    }
}