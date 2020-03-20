using SAT_CL.CXP;
using SAT_CL.EgresoServicio;
using SAT_CL.Global;
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
    public partial class AplicacionFacturasProveedor : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaFacturas();

            //Limpiando Control
            lblError.Text = "";
        }
        /// <summary>
        /// Evento Producido al Aplicar las Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAplicarFacturas_Click(object sender, EventArgs e)
        {
            //Obteniendo Facturas Seleccionadas
            GridViewRow[] gvrs = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

            //Declarando Variables Auxiliares
            bool tieneAnticipos = false;
            string mensajeFactura = "";
            string[] depositos = new string[1];
            int contador = 0;

            //Validando que Existan Registros
            if (gvrs.Length > 0)
            {
                //Creando Arreglo Dinamico
                depositos = new string[gvrs.Length];

                //Recorriendo Facturas
                foreach (GridViewRow gvr in gvrs)
                {
                    //Asignando Indice
                    gvFacturas.SelectedIndex = gvr.RowIndex;

                    //Validando que tenga deposito
                    if (gvFacturas.SelectedDataKey["Anticipo"].ToString().Equals("Si"))
                    {
                        //Asignando Positiva la Variable
                        tieneAnticipos = true;

                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(gvFacturas.SelectedDataKey["IdDeposito"])))
                        {
                            //Validando que exista la factura
                            if (dep.habilitar)
                            {
                                //Concatenando Mensaje
                                mensajeFactura += string.Format("*La Factura '{0}', esta ligada al Anticipo No. '{1}'<br />", gvFacturas.SelectedDataKey["Id"], dep.no_deposito);

                                //Asignando Deposito
                                depositos[contador] = dep.id_deposito.ToString();

                                //Incrementando Contador
                                contador++;
                            }
                        }
                    }

                    //Asignando Mensaje de Operación
                    lblFacturas.Text = mensajeFactura;
                }

                //Validando que tenga Anticipos
                if (tieneAnticipos)
                {
                    //Obteniendo Anticipos de Proveedor
                    using (DataTable dtAnticiosProveedor = SAT_CL.Bancos.EgresoIngreso.ObtieneAnticiposProveedor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                                                                    string.Join(",", depositos)))
                    {
                        //Validando que existan Registros
                        if (Validacion.ValidaOrigenDatos(dtAnticiosProveedor))
                        {
                            //Cargando GridView
                            Controles.CargaGridView(gvAnticiposProveedor, dtAnticiosProveedor, "Id-MontoDisponible-IdServicio", lblOrdenadoFI.Text, true, 7);

                            //Añadiendo Tabla a Session
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAnticiosProveedor, "Table1");

                            //Mostrando Ventana de Confirmación
                            ScriptServer.AlternarVentana(this.Page, "ConfirmacionAnticipo", "contenedorVentanaConfirmacionAnticipo", "ventanaConfirmacionAnticipo");
                        }
                        else
                        {
                            //Inicializando GridView
                            Controles.InicializaGridview(gvAnticiposProveedor);

                            //Eliminando Tabla de Session
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                            //Invocando Método de Guardado
                            guardaAplicaciones();
                        }
                    }
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAnticiposProveedor);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");

                    //Invocando Método de Guardado
                    guardaAplicaciones();
                }
            }
            else
                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(this, "No hay Facturas Seleccionadas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Continuar la Aplicación de Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnContinuarAplicacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaAplicaciones();
        }
        /// <summary>
        /// Evento Producido al Cancelar la Aplicación de Facturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarAplicacion_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana de Confirmación
            ScriptServer.AlternarVentana(this.Page, "ConfirmacionAnticipo", "contenedorVentanaConfirmacionAnticipo", "ventanaConfirmacionAnticipo");
        }

        #region Eventos GridView "Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)

                    //Editando el Registro
                    dr["MontoPreferente"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

                //Cambiando tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFacturas.SelectedValue));

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Invocando Método de Suma
            sumaTotalesAplicacion();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacturas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Recorriendo Filas
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)

                    //Editando el Registro
                    dr["MontoPreferente"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

                //Cambiando Expresión del Ordenamiento
                Controles.CambiaSortExpressionGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Invocando Método de Suma
            sumaTotalesAplicacion();
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
                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)

                    //Editando el Registro
                    dr["MontoPreferente"] = "0.00";

                //Aceptando Cambios
                ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                //Cargando GridView
                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

                //Desmarcando todas las Filas
                Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", false);
            }
            else
            {
                //Inicializando GridView
                Controles.InicializaGridview(gvFacturas);

                //Eliminando Tabla de Session
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Invocando Método de Suma
            sumaTotalesAplicacion();
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

                //Declarando variable que Guardara las Filas Seleccionadas
                int[] indices_chk = new int[1];
                int contador = 0;

                //Validando Id del Control
                switch (chk.ID)
                {
                    case "chkTodosFactura":
                        {
                            //Selecciona Todas las Filas
                            Controles.SeleccionaFilasTodas(gvFacturas, "chkVariosFactura", chk.Checked);

                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");
                            
                            //Validando que el Control haya sido marcado
                            if (chk.Checked)
                            {
                                //Validando que Existan Filas
                                if (gvr.Length > 0)
                                {
                                    //Creando Arreglo Dinamico
                                    indices_chk = new int[gvr.Length];
                                    
                                    //Recorriendo Ciclo
                                    foreach (GridViewRow gv in gvr)
                                    {
                                        //Seleccionando Fila
                                        gvFacturas.SelectedIndex = gv.RowIndex;

                                        //Obteniendo Fila por Editar
                                        DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                                        //Recorriendo Registro Encontrado
                                        foreach (DataRow dr in drEdit)
                                        {
                                            //Actualizando Registros
                                            dr["MontoPreferente"] =
                                            dr["MP2"] = string.Format("{0:0.00}", Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]) - Convert.ToDecimal(dr["MontoPorAplicar"]));
                                        }

                                        //Incrementando Contador
                                        contador++;
                                    }
                                }
                            }
                            else
                            {
                                //Inicializando Variable
                                indices_chk = null;
                                
                                //Recorriendo Registro Encontrado
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Rows)
                                {
                                    //Actualizando Registros
                                    dr["MontoPreferente"] = 
                                    dr["MP2"] = string.Format("{0:0.00}", 0);
                                }
                            }

                            //Aceptando Cambios
                            ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                            //Cargando GridView
                            Controles.CargaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 1);

                            //Validando que Existen 
                            if (gvr.Length > 0)
                            {
                                //Recorriendo Ciclo
                                foreach (GridViewRow gv in gvr)
                                {
                                    //Seleccionando Indice
                                    gvFacturas.SelectedIndex = gv.RowIndex;

                                    //Obteniendo Control
                                    CheckBox chkIndice = (CheckBox)gvFacturas.SelectedRow.FindControl("chkVariosFactura");

                                    //Validando que Exista el Control
                                    if (chkIndice != null)
                                    {
                                        //Marcando el Control
                                        chkIndice.Checked = true;

                                        //Obteniendo Control
                                        LinkButton lnkIndice = (LinkButton)gvFacturas.SelectedRow.FindControl("lnkCambiar");

                                        //Validando que Exista el Control de Cambio
                                        if (lnkIndice != null)

                                            //Habilitando Control
                                            lnkIndice.Enabled = true;
                                    }
                                }
                            }

                            //Obteniendo Encabezado
                            CheckBox chkEncabezado = (CheckBox)gvFacturas.HeaderRow.FindControl("chkTodosFactura");

                            //Validando que Exista el Control
                            if (chkEncabezado != null)

                                //Marcando el Control según su Valor
                                chkEncabezado.Checked = chk.Checked;

                            //Inicializando Indices
                            Controles.InicializaIndices(gvFacturas);

                            break;
                        }
                    case "chkVariosFactura":
                        {
                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturas, "chkVariosFactura");

                            //Validando que existan Filas Seleccionadas
                            if(gvr.Length > 0)
                            {
                                //Creando Arreglo de forma Dinamica
                                indices_chk = new int[gvr.Length];

                                //Iniciando Ciclo
                                foreach(GridViewRow gv in gvr)
                                {
                                    //Guardando Indices
                                    indices_chk[contador] = gv.RowIndex;

                                    //Incremenando Contador
                                    contador++;
                                }
                            }
                            else
                                //Inicializando el Arreglo
                                indices_chk = null;
                            
                            //Seleccionando Fila
                            Controles.SeleccionaFila(gvFacturas, sender, "chk", false);

                            //Validando que se haya marcado la Opción
                            if (chk.Checked)
                            {                                
                                //Obteniendo Fila por Editar
                                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                                //Recorriendo Registro Encontrado
                                foreach (DataRow dr in drEdit)
                                {
                                    //Validando que los montos por Aplicar no sobrepasen los Montos Pendientes
                                    if ((Convert.ToDecimal(dr["MontoPendiente"]) - Convert.ToDecimal(dr["MontoPorAplicar"])) >= 0)
                                    {
                                        //Actualizando Registros
                                        dr["MontoPreferente"] = string.Format("{0:0.00}", (Convert.ToDecimal(dr["MontoPendiente"]) - Convert.ToDecimal(dr["MontoPorAplicar"])));
                                        dr["MP2"] = string.Format("{0:0.00}", (Convert.ToDecimal(dr["MontoPendiente"]) - Convert.ToDecimal(dr["MontoPorAplicar"])));
                                    }
                                    else
                                    {
                                        //Actualizando Registros
                                        dr["MontoPreferente"] = string.Format("{0:0.00}", 0);
                                        dr["MP2"] = string.Format("{0:0.00}", 0);
                                    }
                                }
                            }
                            else
                            {
                                //Obteniendo Fila por Editar
                                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                                //Recorriendo Registro Encontrado
                                foreach (DataRow dr in drEdit)
                                {
                                    //Actualizando Registros
                                    dr["MontoPreferente"] = string.Format("{0:0.00}", 0);
                                    dr["MP2"] = string.Format("{0:0.00}", 0);
                                }
                            }

                            //Aceptando Cambios
                            ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                            //Cargando GridView
                            Controles.CargaGridView(gvFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 1);

                            //Validando que Existen Indices Seleccionados
                            if (indices_chk != null)
                            {
                                //Recorriendo Ciclo
                                foreach (int ind in indices_chk)
                                {
                                    //Seleccionando Indice
                                    gvFacturas.SelectedIndex = ind;

                                    //Obteniendo Control
                                    CheckBox chkIndice = (CheckBox)gvFacturas.SelectedRow.FindControl("chkVariosFactura");

                                    //Validando que Exista el Control
                                    if (chkIndice != null)
                                    {
                                        //Marcando el Control
                                        chkIndice.Checked = true;

                                        //Obteniendo Control
                                        LinkButton lnkIndice = (LinkButton)gvFacturas.SelectedRow.FindControl("lnkCambiar");

                                        //Validando que Exista el Control de Cambio
                                        if (lnkIndice != null)

                                            //Habilitando Control
                                            lnkIndice.Enabled = true;
                                    }
                                }
                            }

                            //Inicializando Indices
                            Controles.InicializaIndices(gvFacturas);

                            break;
                        }
                }

                //Invocando Método de Suma
                sumaTotalesAplicacion();
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

                                //Recorriendo Registros
                                foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString()))
                                {
                                    //Validando que el Valor Ingresado no supera al Permitido
                                    value = Convert.ToDecimal(dr["MP2"]) >= Convert.ToDecimal(txt.Text == "" ? "0" : txt.Text) ? true : false;

                                    //Realizando Validación
                                    if (value)

                                        //Actualizando Registro
                                        dr["MontoPreferente"] = string.Format("{0:0.00}", txt.Text == "" ? "0" : txt.Text);
                                    else
                                    {
                                        //Actualizando Registro
                                        dr["MontoPreferente"] = string.Format("{0:0.00}", dr["MP2"]);

                                        //Instanciando Excepción
                                        lblError.Text = string.Format("La Cantidad excede el Monto de {0:0.00}", dr["MP2"]);
                                    }

                                    //Obteniendo Control
                                    LinkButton lnk = (LinkButton)sender;

                                    //Deshabilitando el Control
                                    txt.Enabled = false;

                                    //Configurando Control
                                    lnk.Text = lnk.CommandName = "Cambiar";
                                }

                                //Actualizando Cambios
                                ((DataSet)Session["DS"]).Tables["Table"].AcceptChanges();

                                //Cargando GridView
                                Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

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

                //Invocando Método de Suma
                sumaTotalesAplicacion();
            }
        }
        /*// <summary>
        /// Evento Producido al Marcar un Registro del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAplicar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturas, sender, "lnk", false);

                //Declarando Variables Auxiliares
                decimal monto_x_aplicar = 0.00M;

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Fila por Editar
                DataRow[] drEdit = ((DataSet)Session["DS"]).Tables["Table"].Select("Id = " + gvFacturas.SelectedDataKey["Id"].ToString() + " ");

                //Recorriendo Ciclo
                foreach (DataRow dr in drEdit)
                {
                    //Validando que exista el Monto Preferente
                    if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPreferente"]) > 0.00M)

                        //Actualizando Registros
                        monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPreferente"]);
                    else
                        //Actualizando Registros
                        monto_x_aplicar = Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPendiente"]) - Convert.ToDecimal(dr["MontoPorAplicar"]);
                }
                //Validando que Exista un Monto por Aplicar
                if (monto_x_aplicar > 0)

                    //Insertando Ficha de Ingreso Aplicada
                    result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), 0,
                                        monto_x_aplicar, DateTime.MinValue, false, 0, false, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion(string.Format("La Factura {0}: No se puede aplicar facturas con monto de: $0.00", gvFacturas.SelectedDataKey["Id"]));

                //Validando que la Operación fuese Exitosa
                if (result.OperacionExitosa)
                {
                    //Instanciando Mensaje de Operación
                    result = new RetornoOperacion(string.Format("La Factura {0}: ha sido Aplicada Exitosamente por el monto de: {1:C2}", gvFacturas.SelectedDataKey["Id"], monto_x_aplicar));

                    //Invocando Método de Busqueda de Facturas
                    buscaFacturas();
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvFacturas);

                //Mostrando Mensaje de Operación
                lblError.Text = result.Mensaje;
            }
        }//*/

        #endregion

        #region Eventos GridView "Anticipo Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFI.SelectedValue), true, 7);
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            Controles.CambiaSortExpressionGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 7);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Anticipo Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticiposProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvAnticiposProveedor, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 7);
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

            //Inicializando GridViews
            Controles.InicializaGridview(gvFacturas);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacturas, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFI, "", 26);
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Facturas
        /// </summary>
        private void buscaFacturas()
        {
            //Obteniendo Valor
            using (DataTable dtFacturas = SAT_CL.CXP.Reportes.ObtieneFacturasPorAplicar(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtUUID.Text, Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolio.Text, "0"))))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturas, dtFacturas, "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Invocando Método de Suma
                sumaTotalesAplicacion();
            }
        }
        /// <summary>
        /// Método Privado encargado de la Configuración de los Registros de las Facturas
        /// </summary>
        /// <param name="sender">Control</param>
        /// <param name="indices_chk">Indice de Controles</param>
        private void configuraRegistroFactura(object sender)
        {
            //Inicializando Variables
            int [] indices_chk = new int[1];
            //int contador = 0;
            bool indicador = false;

            //Declarando Variables Auxiliares
            //decimal monto_sumatoria = 0.00M;
            //decimal monto_disponible = 0.00M;

            //Obteniendo Control
            CheckBox chk = (CheckBox)sender;

            //Cargando GrdiView
            Controles.CargaGridView(gvFacturas, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdDeposito-Anticipo-MontoPendiente-MontoPreferente-MontoPorAplicar", "", true, 2);

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
        /// Método encargado de Sumar los Totales al Pie del GridView
        /// </summary>
        private void sumaTotalesAplicacion()
        {
            //Validando que existan Registros
            if (Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table"))
            {
                //Sumando Totales
                gvFacturas.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvFacturas.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Trasladado)", "")));
                gvFacturas.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retenido)", "")));
                gvFacturas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
                gvFacturas.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
                gvFacturas.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoPendiente)", "")));
                gvFacturas.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoPorAplicar)", "")));
                gvFacturas.FooterRow.Cells[16].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoPreferente)", "")));
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
        /// Método encargado de Guardar las Aplicaciones a Facturas de Proveedor
        /// </summary>
        private void guardaAplicaciones()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            string[] result_msn = new string[1];
            int contador = 0;

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
                    //Recorriendo Filas
                    foreach (GridViewRow gvr in gvrs)
                    {
                        //Seleccionando Fila
                        gvFacturas.SelectedIndex = gvr.RowIndex;

                        //Validando que el Monto no sea 0
                        if (Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPreferente"]) != 0.00M)
                        {
                            //Insertando Ficha de Ingreso Aplicada
                            result = SAT_CL.CXC.FichaIngresoAplicacion.InsertarFichaIngresoAplicacion(72, Convert.ToInt32(gvFacturas.SelectedDataKey["Id"]), 0,
                                                Convert.ToDecimal(gvFacturas.SelectedDataKey["MontoPreferente"]), DateTime.MinValue, false, 0, false, 0,
                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Operación fuese Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Guardando Mensaje de la Operación
                                result_msn[contador] = string.Format("* La Factura {0}: Ha sido aplicada Exitosamente por el monto de: {1:C2}", gvFacturas.SelectedDataKey["Id"], gvFacturas.SelectedDataKey["MontoPreferente"]);

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

                    //Validando que las Operaciones fuesen Exitosas
                    if (result.OperacionExitosa)
                    {
                        //Invocando Método de Busqueda de Facturas
                        buscaFacturas();

                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturas);

                        //Completando Transacción
                        trans.Complete();
                    }

                    //Mostrando Mensaje de Operación
                    lblError.Text = string.Join("<br/>", result_msn);
                }
            }
            else
            {
                //Mostrando Mensaje de Operación
                lblError.Text = "No hay Facturas Seleccionadas";
                ScriptServer.MuestraNotificacion(this, lblError.Text, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion
    }
}