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

namespace SAT.Ruta
{
    public partial class RutaCruces : System.Web.UI.Page
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
            gvIaves.EditIndex = -1;
            //Invocando Método de Busqueda
            buscaFacturas();
            //Busca IAVE
            buscarCrucesIaves();        
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturas);
            //Inicializando Reporte
            TSDK.ASP.Controles.InicializaGridview(gvIavesAsignados);

        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar IAVE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarCaseta_Click(object sender, EventArgs e)
        {
                //Inicializamos Indices
                gvIaves.EditIndex = -1;
                //Invocando Método de Busqueda
                buscarCrucesIaves();
                //Inicialiamos Indices
                Controles.InicializaIndices(gvIaves);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Asignar IAVE"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAsignarIave_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Total Casetas
            Decimal TotalCasetas = 0;
            Decimal TotalFacturaAsignado = 0;
            //Validando que exista una Factura Seleccionada
            if (gvFacturas.SelectedIndex != -1)
            {
                //Obteniendo Filas Seleccionadas
                GridViewRow[] filas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvIaves, "chkVarios");

                //Validando que existan IAVE Seleccionados
                if (filas.Length > 0)
                {
                    //Inicializando Bloque Transacional
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Instanciando la Factura
                        using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"])))
                        {
                            //Validando que exista la Factura
                            if (fp.id_factura != 0)
                            {
                                //Declarando Contador
                                int contador = 0;
                                //Iniciando Ciclo Total  Casetas Seleccionados
                                while (contador < filas.Length)
                                {
                                    //Obteniendo Indice
                                    gvIaves.SelectedIndex = filas[contador].RowIndex;                                    
                                    //Instanciando 
                                    if(Convert.ToInt32(gvIaves.SelectedDataKey["Id"]) > 0)
                                    {
                                        //Validando que exista el IAVE
                                        TotalCasetas = TotalCasetas + Convert.ToDecimal(gvIaves.SelectedDataKey["Monto"]);
                                        contador = contador + 1;
                                    }
                                }
                                //Asignacion 
                                TotalFacturaAsignado = Convert.ToDecimal(gvFacturas.SelectedDataKey["Asignado"]) > 0 ? (fp.total_factura - Convert.ToDecimal(gvFacturas.SelectedDataKey["Asignado"])) : fp.total_factura;
                                if (TotalCasetas <= TotalFacturaAsignado)
                                {
                                    //Declarando Contador
                                    int contadori = 0;
                                    //Iniciando Ciclo
                                    while (contadori < filas.Length)
                                    {
                                        //Obteniendo Indice
                                        gvIaves.SelectedIndex = filas[contadori].RowIndex;
                                        TotalCasetas = TotalCasetas + Convert.ToDecimal(gvIaves.SelectedDataKey["Id"]);
                                        //Instanciando 
                                        using (SAT_CL.Ruta.CrucesAutorizadosIave Iave = new SAT_CL.Ruta.CrucesAutorizadosIave(Convert.ToInt32(gvIaves.SelectedDataKey["Id"])))
                                        {
                                            //Validando que exista el IAVE
                                            if (Iave.habilitar)
                                            {
                                                //Asignando el Iave a la Factura
                                                result = Iave.AsignaFacturaIave(fp.id_factura, 2, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Validando que la Operación haya sido exitosa
                                                contadori = result.OperacionExitosa ? contadori + 1 : filas.Length;
                                            }
                                        }
                                    }

                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("El Total de la factura es menor al costos de las casetas");
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
                    if (result.OperacionExitosa)
                    {
                        //Obteniendo Factura de Proveedor
                        int idFacturaProveedor = Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]);

                        //Vargando Reportes
                        buscarCrucesIaves();
                        buscaFacturas();

                        //Marcando Fila
                        Controles.MarcaFila(gvFacturas, idFacturaProveedor.ToString(), "Id", "Id-Monto-Asignado", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenadoFactura.Text, Convert.ToInt16(ddlTamanoFac.SelectedValue), true, 1);

                        //Recargando IAVES Asignados
                        buscaIavesAsignados(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]));
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe seleccionar al menos una caseta Iave");
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Debe Seleccionar una Factura");

            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(btnAsignarIave, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            buscaIavesAsignados(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]));
            lblmontoactual.Text = '$'+ Convert.ToString(gvFacturas.SelectedDataKey["Monto"]);
            buscarCrucesIaves();
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
                if (lnk != null)
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
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvIaves, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamanoFac.SelectedValue), true, 1);
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
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlIave, 30, "", Convert.ToInt32(ddlProveedor.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }

        #region Eventos IAVE Asignados

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "IAVE Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIavesAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvIavesAsignados, ((DataSet)Session["DS"]).Tables["Table2"], e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "IAVE Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIavesAsignados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Ordenamiento del GridView
            lblOrdenadoIave.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvIavesAsignados, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "IAVE Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoIavesAsig_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvIavesAsignados, ((DataSet)Session["DS"]).Tables["Table2"], Convert.ToInt32(ddlTamanoIavesAsig.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "IAVE Asignados"
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

        #region Eventos GridView "IAVE"

        /// <summary>
        /// Evento Producido al Seleccionar un Control CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
            CheckBox chkHeader = (CheckBox)gvIaves.HeaderRow.FindControl("chkTodos");

            //Evalua el ID del CheckBox en el que se produce el cambio
            switch (((CheckBox)sender).ID)
            {
                //Caso para el CheckBox "Todos"
                case "chkTodos":
                    {
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvIaves, "chkVarios", chkHeader.Checked);
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
                                gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvIaves, "chkVarios");

                                //Validando que haya sido marcado el Control
                                if (chk.Checked)
                                {
                                    //Validando que Existan Filas
                                    if (gvr.Length > 0)
                                    {
                                        //Recorriendo Cada Fila
                                        foreach (GridViewRow gr in gvr)
                                        {
                                            //Selecionando Fila
                                            gvIaves.SelectedIndex = gr.RowIndex;

                                            //Sumando Total
                                            monto_acumulado = monto_acumulado + Convert.ToDecimal(gvIaves.SelectedDataKey["Monto"]);
                                        }
                                    }

                                    //Calculando Monto Disponible
                                    monto_disponible = monto_disponible_factura - monto_acumulado;

                                    //Validando que el monto de los IAVE anteriores no excedan el monto disponible
                                    if (monto_disponible <= 0)

                                    {
                                        //Desmarcando el Control
                                        chk.Checked = false;

                                        //Mostrando Error
                                        ScriptServer.MuestraNotificacion(gvIaves, "El monto de la caseta excede el Monto de la Factura.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);



                                    }
                                    lblmontoactual.Text = '$' + monto_disponible.ToString();
                                    lblmontocasetas.Text = '$' + monto_acumulado.ToString();
                                }

                                //Validando que se encuentren todas las filas seleccionadas
                                if (gvr.Length == gvIaves.Rows.Count)

                                    //Marcando Encabezado
                                    chkHeader.Checked = true;
                                else
                                    //Desmarcando Encabezado
                                    chkHeader.Checked = false;

                                //Seleccionando Fila
                                TSDK.ASP.Controles.SeleccionaFila(gvIaves, sender, "chk", false);

                                //Obteniendo Control "LinkButton"
                                LinkButton lnk = (LinkButton)gvIaves.SelectedRow.FindControl("lnkEditar");

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
                                TSDK.ASP.Controles.InicializaIndices(gvIaves);
                            }
                            else
                            {
                                //Desmarcando Casilla
                                chk.Checked = false;

                                //Mostrando Error
                                ScriptServer.MuestraNotificacion(gvIaves, "Debe Seleccionar una Factura", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
        protected void gvIaves_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Ordenamiento del GridView
            lblOrdenadoIave.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvIaves, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvIaves_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvIaves, ((DataSet)Session["DS"]).Tables["Table1"], e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView IAVE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoIave_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvIaves, ((DataSet)Session["DS"]).Tables["Table1"], Convert.ToInt32(ddlTamanoIave.SelectedValue), true, 3);
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

        #region Edición IAVE
        /// <summary>
        /// Evento generado al  Quitar un Iave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkQuitar_Click(object sender, EventArgs e)
        {
            //Declaramos objetpo Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si hay registros
            if (gvIavesAsignados.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvIavesAsignados, sender, "lnk", false);
                //Inicializamos Valores
                using (SAT_CL.Ruta.CrucesAutorizadosIave objIave = new SAT_CL.Ruta.CrucesAutorizadosIave(Convert.ToInt32(gvIavesAsignados.SelectedValue)))
                {
                    //Factura
                    resultado = objIave.AsignaFacturaIave(0,1,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializamos Inidices
                        Controles.InicializaIndices(gvIavesAsignados);
                        //Cargamos Iave Asignados
                        buscaIavesAsignados(Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]));
                        //Cargamos Fcaturas
                        buscaFacturas();
                        //Busca Iaves
                        buscarCrucesIaves();

                    }
                    //Mostrando Resultado
                    ScriptServer.MuestraNotificacion(gvIavesAsignados, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            TSDK.ASP.Controles.InicializaGridview(gvIaves);
            TSDK.ASP.Controles.InicializaGridview(gvIavesAsignados);
            //Mostrando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            lblmontocasetas.Text = "";
            lblmontocasetas.Text = "";
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFac, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoIave, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoIavesAsig, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlProveedor, 197, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlIave, 30, "-- Seleccione una Estación", Convert.ToInt32(ddlProveedor.SelectedValue), "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {
            //Obteniendo Reporte
            using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneReporteFacturasIAVE(ddlProveedor.SelectedValue,
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString(), txtSerie.Text,
                                        Convert.ToInt32(txtFolio.Text == "" ? "0" : txtFolio.Text), ""))
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
        /// Método Privado encargado de Buscar los Iave
        /// </summary>
        private void buscarCrucesIaves()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;

            //Validando que se Incluyan las Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }

            //Obteniendo Reporte
            using (DataTable dtIave = SAT_CL.EgresoServicio.Reportes.ObtieneCrucesIave(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    txtNoCaseta.Text, Cadena.VerificaCadenaVacia(txtTag.Text,"0"), fec_ini, fec_fin))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtIave))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvIaves, dtIave, "Id-Monto", lblOrdenadoIave.Text, true, 3);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtIave, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvIaves);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
            //Método encargado de Sumar Totales
            sumaIaveTotales();
        }

        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaCasetasAsignadosTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            {
                //Mostrando Totales
                gvIavesAsignados.FooterRow.Cells[9].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvIavesAsignados.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
            }
        }

        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaIaveTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Mostrando Totales
                gvIavesAsignados.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Monto)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvIavesAsignados.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar las casetas iave Asignados a una Factura
        /// </summary>
        private void buscaIavesAsignados(int id_ruta)
        {
            //Obteniendo Facturas Seleccionadas
            using (DataTable dtIaveAsignados = SAT_CL.Ruta.CrucesAutorizadosIave.ObtieneIAVEPorFactura(id_ruta))
            {
                //Validando que existan las iave Asignados
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtIaveAsignados))
                {
                    //Cargando Iave Seleccionados
                    TSDK.ASP.Controles.CargaGridView(gvIavesAsignados, dtIaveAsignados, "Id-MontoAplicado", lblOrdenadoIavesAsig.Text, true, 2);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtIaveAsignados, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvIavesAsignados);
                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Método encargado de Sumar Totales
            sumaCasetasAsignadosTotales();
        }

        #endregion
    }
}