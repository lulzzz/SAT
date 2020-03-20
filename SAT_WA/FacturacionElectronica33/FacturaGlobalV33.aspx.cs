using SAT_CL.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using FE33 = SAT_CL.FacturacionElectronica33;

namespace SAT.FacturacionElectronica33
{
    public partial class FacturaGlobalV33 : System.Web.UI.Page
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
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
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
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(13, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaFacturaGlobal();
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        //Carga Paquetes Terminadaos
                        cargaPaquetesTerminados();
                        break;
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (fg.id_factura_global > 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Deshabilitando Producto
                                result = fg.DeshabilitarFacturaGlobal(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaPagina();
                                }

                                //Mostrando Mensaje de Operación
                                lblError.Text = result.Mensaje;
                            }
                        }
                        break;
                    }
                case "Bitacora":
                    {
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "13", "Factura Global");
                        break;
                    }
                case "Referencias":
                    {
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "13", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion
            if (Convert.ToInt32(Session["id_registro"]) > 0)
            {
                //Determinando la pestaña pulsada
                switch (((Button)sender).CommandName)
                {
                    case "FacturasDisponibles":
                        //Cambiando estilos de pestañas
                        btnPestanaFacturasDisponibles.CssClass = "boton_pestana_activo";
                        btnPestanaFacturasLigadas.CssClass = "boton_pestana";
                        //Asignando vista activa de la forma
                        mtvFacturas.SetActiveView(vwFacturasDisponibles);
                        break;
                    case "FacturasLigadas":
                        //Cambiando estilos de pestañas
                        btnPestanaFacturasDisponibles.CssClass = "boton_pestana";
                        btnPestanaFacturasLigadas.CssClass = "boton_pestana_activo";
                        //Asignando vista activa de la forma
                        mtvFacturas.SetActiveView(vwFacturasLigadas);
                        break;
                }
            }
            //Cargamos Facturas
            cargaFacturas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaFacturaGlobal();
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
        
        #region Eventos GridView "Facturas Disponibles"
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacDisp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFacDisp.SelectedValue));
            //Invocando Método de Suma
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacDisp_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacDisp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            //Invocando Método de Suma
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Método encargado de Cargar los Paquetes Terminados dado una Compania y un Cliente
        /// </summary>
        private void cargaPaquetesTerminados()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Factura Global
                        using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista el Paquete
                            if (fg.id_factura_global > 0)
                            {
                                //Creando Script de Autocompletado
                                string scriptAutocompleta = @"<script type='text/javascript'>
                                                                //Añadiendo Función de Autocompletado al Control (Cliente)
                                                                $('#" + txtPaqPrevio.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=35&param=" + fg.id_compania.ToString() + @"&param2=" + fg.id_compania_cliente.ToString() + @"'});
                                                              </script>";

                                //Ejecutando Script
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaConfiguracionPaquetePT", scriptAutocompleta, false);
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasDisponibles, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaTotalesFacturasDisponibles();
        }
        /// <summary>
        /// Evento Producido al Enlazar un Origen de Datos con el GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Controles
                CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosFactura");
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkAgregarFactura");

                //Validando que exista el Control
                if (chk != null && lnk != null)
                {
                    //Obteniendo Origen de la Fila
                    DataRowView rowView = (DataRowView)e.Row.DataItem;

                    //Obteniendo Indicador
                    int indicador = Convert.ToInt32(rowView["Indicador"].ToString() == "" ? "1" : rowView["Indicador"].ToString());

                    //Validando Indicador
                    switch (indicador)
                    {
                        case 0:
                            {
                                //Habilitando Controles
                                lnk.Enabled =
                                chk.Enabled = true;
                                break;
                            }
                        default:
                            {
                                //Deshabilitando Controles
                                lnk.Enabled =
                                chk.Enabled = false;
                                break;
                            }
                    }
                }

                //Obteniendo Controles
                using (LinkButton lkbRef1 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia1"),
                                    lkbRef2 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia2"),
                                      lkbRef3 = (LinkButton)e.Row.FindControl("lnkAgregarReferencia3"))
                {
                    //Validando que exista el Control
                    if (lkbRef1 != null && lkbRef2 != null && lkbRef3 != null)
                    {
                        //Obteniendo Origen de la Fila
                        DataRowView rowView = (DataRowView)e.Row.DataItem;

                        //Mostrando Control
                        lkbRef1.Visible =
                        lkbRef2.Visible =
                        lkbRef3.Visible = true;

                        //Validando que Exista el Servicio
                        switch (rowView["NoServicio"].ToString())
                        {
                            //Si no es de Servicio
                            case "No Aplica":
                                {
                                    //Deshabilitando Control
                                    lkbRef1.Enabled =
                                    lkbRef2.Enabled =
                                    lkbRef3.Enabled = false;

                                    break;
                                }
                            default:
                                {
                                    //Habilitando Control
                                    lkbRef1.Enabled =
                                    lkbRef2.Enabled =
                                    lkbRef3.Enabled = true;

                                    //Validando que exista almenos un Valor
                                    if (rowView["NoViaje"].ToString().Equals(""))

                                        //Asignando Valor
                                        lkbRef1.Text = "Sin Referencia";
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar el ChekBox "Todos" del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFactura_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Obteniendo Control Disparador
                CheckBox chk = (CheckBox)sender;
                CheckBox chkVarios;

                //Validando el Control
                switch (chk.ID)
                {
                    case "chkTodosFactura":
                        {
                            //Validando que la fila sea seleccionada
                            if (!chk.Checked)
                            {
                                //Seleccionando Todas las Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosFactura", chk.Checked);

                                //Visualizando Control
                                btnGuardarFactura.Visible = btnGuardarConceptos.Enabled = false;
                            }
                            else
                            {
                                //Recorriendo cada fila
                                foreach (GridViewRow gvr in gvFacturasDisponibles.Rows)
                                {
                                    //Obteniendo Control
                                    chkVarios = (CheckBox)gvr.FindControl("chkVariosFactura");

                                    //Validando que existe la Fila
                                    if (chkVarios != null)

                                        //Validando que el Control este Habilitado para su Seleccion
                                        chkVarios.Checked = chkVarios.Enabled ? chk.Enabled : false;
                                }

                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                                //Visualizando Control de Guardado
                                btnGuardarFactura.Visible = btnGuardarConceptos.Enabled = gvFilas.Length > 0 ? true : false;
                            }

                            break;
                        }
                    case "chkVariosFactura":
                        {
                            //Obteniendo Control de ENcabezado
                            CheckBox chkHeader = (CheckBox)gvFacturasDisponibles.HeaderRow.FindControl("chkTodosFactura");

                            //Validando que el control se haya desmarcado
                            if (!chk.Checked)

                                //Desmarcando Encabezado
                                chkHeader.Checked = false;

                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                            //Validando que existan filas seleccionadas
                            if (gvr.Length > 0)

                                //Visualizando Control de Guardado
                                btnGuardarFactura.Visible = true;
                            else
                                //Ocultando Control de Guardado
                                btnGuardarFactura.Visible = false;

                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Instanciando la Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Cargando los Conceptos Disponibles de la Factura
                    using (DataTable dtConceptos = SAT_CL.Facturacion.FacturadoConcepto.CargaConceptosDisponibleFactura(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]), fg.id_compania_cliente))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                        {
                            //Cargando GridView de Conceptos
                            TSDK.ASP.Controles.CargaGridView(gvFacturaConceptos, dtConceptos, "IdFacturaConcepto", "", true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["SD"], dtConceptos, "Table2");
                        }
                        else
                        {
                            //Inicializando GridView de Conceptos
                            TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["SD"], "Table2");
                        }
                    }

                    //Inicializando GridView de Conceptos
                    TSDK.ASP.Controles.InicializaIndices(gvFacturaConceptos);

                    //Abriendo Ventana Modal de Conceptos
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasDisponibles, upgvFacturasDisponibles.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");
                }
            }
        }
        /// <summary>
        /// Evento Producido al Agregar un Registro del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Agregamos Factura Global
                result = agregarFacturaGlobal();

                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Agregamos Factura Global
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion agregarFacturaGlobal()
        {
            //Declarando Objeto Resultado
            RetornoOperacion result = new RetornoOperacion();
            //Validando que existe la Factura 
            if (Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]) > 0)
            {
                //Instanciando Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (fg.id_factura_global > 0)
                    {
                        //Validando el Estatus
                        if (fg.estatus == SAT_CL.Facturacion.FacturaGlobal.Estatus.Registrada)
                        {
                            //Validando el Estatus de Facturación Electronica
                            if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobal(fg.id_factura_global) == 0)

                                //Insertando Registro de Relación
                                result = SAT_CL.Facturacion.FacturadoFacturacion.InsertarFacturadoFacturacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                                0, Convert.ToInt32(Session["id_registro"]), 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica, Imposible su Edición");

                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion(string.Format("El Estatus '{0}' de la Factura Global no permite su Edición", fg.estatus));

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
                        }
                    }
                }
            }
            else
            {
                //Instanciando Excepción
                result = new RetornoOperacion("No existe la Factura");

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
            }

            //Validando que las Operaciones hayan sido exitosas
            if (result.OperacionExitosa)
            {
                //Personalizando Mensaje de la Operación
                result = new RetornoOperacion(string.Format("La Factura {0}: se ha agregado exitosamente", gvFacturasDisponibles.SelectedDataKey["IdFactura"]), true);

                //Invocando Método de Carga
                cargaFacturas();
            }

            return result;
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Editar Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Validando que exista una Factura Seleccionada
                if (Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]) > 0)
                {
                    //Inicializando Control
                    ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]));

                    //Abriendo Ventana Modal
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasDisponibles, upgvFacturasDisponibles.GetType(), "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                }
            }
        }
        /// <summary>
        /// Evento Producido el Guardar el Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Concepto
            result = ucFacturadoConcepto.GuardarFacturaConcepto();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido el Eliminar el Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucFacturadoConcepto.EliminaFacturaConcepto();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaEC_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarVentanaEC, uplnkCerrarVentanaEC.GetType(), "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");

            //Validando Pestaña Activa
            if (mtvFacturas.ActiveViewIndex == 0)
            {
                //Abriendo Ventana Modal
                TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarVentanaEC, uplnkCerrarVentanaEC.GetType(), "Agregar Factura Global", "confirmacionAgregarFacturaGlobal", "agregarFacturaGlobal");
            }
            else
            {
                //Cargamos Facturas
                cargaFacturas();
            }
            //Inicializamos  Valores de Factura
            obtieneTotalesFacturaGlobal();
        }
        /// <summary>
        /// Evento generado al Agregar el Servicio a una Factura Global
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSi_Click(object sender, EventArgs e)
        {

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Agregamos Factura Global
            result = agregarFacturaGlobal();

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnSi, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Cerrar Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnSi, upbtnSi.GetType(), "Agregar Factura Global", "confirmacionAgregarFacturaGlobal", "agregarFacturaGlobal");

        }
        /// <summary>
        /// Evento generado al No Acepatar el Servicio a una Factura Global
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNo_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

            //Invocando Método de Carga
            cargaFacturas();

            //Cerrar Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnNo, upbtnNo.GetType(), "Agregar Factura Global", "confirmacionAgregarFacturaGlobal", "agregarFacturaGlobal");

        }
        /// <summary>
        /// Evento Producido al Agregar las Referencias de una Factura del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAgregarReferencia_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"])))
                {
                    //Validando que Exista la Factura
                    if (fac.id_factura > 0)
                    {
                        //Inicializando Control
                        ucReferenciasViaje.InicializaControl(fac.id_servicio);

                        //Mostrando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Guardar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// /// Evento Producido al Eliminar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga
                cargaFacturas();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarReferencias_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
        }

        #endregion

        #region Eventos GridView "Facturas Ligadas"
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacLigadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacLigadas.SelectedValue), true, 2);

            //Invocando Método de Facturas Ligadas
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Registro del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que Exista
                    if (fg.id_factura_global > 0)
                    {
                        //Validando el Estatus
                        if (fg.estatus == SAT_CL.Facturacion.FacturaGlobal.Estatus.Registrada)
                        {
                            //Validando el Estatus de Facturación Electronica
                            if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobal(fg.id_factura_global) == 0)
                            {
                                //Instanciando Registro
                                using (SAT_CL.Facturacion.FacturadoFacturacion ff = new SAT_CL.Facturacion.FacturadoFacturacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFF"])))
                                {
                                    //Validando que exista el Registro
                                    if (ff.id_facturado_facturacion > 0)
                                    {
                                        //Validando que la Factura no este Facturada
                                        if (ff.id_factura_electronica == 0)

                                            //Deshabilitando Registro
                                            result = ff.DeshabilitaFacturadoFacturacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("La Factura ha sido Timbrada, Imposible su Edición");
                                    }
                                }

                                //Validando que la Operación fuese exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Cargando Todas las Facturas
                                    cargaFacturas();

                                    //Cargando Totales
                                    obtieneTotalesFacturaGlobal();

                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                                }
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica, Imposible su Edición");

                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion(string.Format("El Estatus '{0}' de la Factura Global no permite su Edición", fg.estatus));

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                        }
                    }
                }

                //Mostrando Notificación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Link "Ver Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerConceptos_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando la Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Cargando los Conceptos Disponibles de la Factura
                    using (DataTable dtConceptos = SAT_CL.Facturacion.FacturadoConcepto.CargaConceptosLigadosFactura(Convert.ToInt32(Session["id_registro"]),
                                                                Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"]), fg.id_compania_cliente))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                        {
                            //Cargando GridView de Conceptos
                            TSDK.ASP.Controles.CargaGridView(gvFacturaConceptos, dtConceptos, "IdFacturaConcepto", "", true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["SD"], dtConceptos, "Table2");
                        }
                        else
                        {
                            //Inicializando GridView de Conceptos
                            TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["SD"], "Table2");
                        }
                    }

                    //Obteniendo Control de Encabezado
                    CheckBox chkHeader = (CheckBox)gvFacturaConceptos.HeaderRow.FindControl("chkTodosConceptos");

                    //Validando que se Obtuvo el Control
                    if (chkHeader != null)
                        //Deshabilitando Control
                        chkHeader.Enabled = false;

                    //Ocultando Control
                    btnGuardarConceptos.Visible = false;

                    //Inicializando GridView de Conceptos
                    TSDK.ASP.Controles.InicializaIndices(gvFacturaConceptos);

                    //Abriendo Ventana Modal de Conceptos
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasLigadas, upgvFacturasLigadas.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");
                }
            }
        }
        /// <summary>
        /// Evento Producido al Editar los Conceptos de las Facturas Ligadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarConceptosLigados_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Instanciando Factura Global
                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validando que exista la Factura Global
                    if (fg.habilitar)
                    {
                        //Validando el Estatus de Facturación Electronica
                        if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobal(fg.id_factura_global) == 0)
                        {
                            //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                            //Validando que exista una Factura Seleccionada
                            if (Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"]) > 0)
                            {
                                //Inicializando Control
                                ucFacturadoConcepto.InicializaControl(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFactura"]));

                                //Abriendo Ventana Modal
                                TSDK.ASP.ScriptServer.AlternarVentana(upgvFacturasLigadas, upgvFacturasLigadas.GetType(), "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                            }
                        }
                        else
                            //Mostrando Excepción
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Factura se encuentra Registrada en Facturación Electronica, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No Existe la Factura Global", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);

            //Invocando Método de Facturas Ligadas
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacLigadas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);

            //Invocando Método de Facturas Ligadas
            sumaTotalesFacturasLigadas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Facturas Ligadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacLigadas_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }
        #endregion

        #region Eventos GridView "Factura Conceptos"
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacConceptos.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturaConceptos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturaConceptos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Enlazar un Origen de Datos con el GridView "Facturas Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturaConceptos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosConceptos");

                //Validando que exista el Control
                if (chk != null)
                {
                    //Obteniendo Origen de la Fila
                    DataRowView rowView = (DataRowView)e.Row.DataItem;

                    //Obteniendo Indicador
                    int indicador = Convert.ToInt32(rowView["Indicador"].ToString() == "" ? "1" : rowView["Indicador"].ToString());

                    //Validando Indicador
                    switch (indicador)
                    {
                        case 0:
                            {
                                //Habilitando Control
                                chk.Enabled = true;
                                break;
                            }
                        case 1:
                            {
                                //Deshabilitando Control
                                chk.Enabled = false;
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar el Control CheckBox "Todos o Varios" del GridView "Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosConceptos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturaConceptos.DataKeys.Count > 0)
            {
                //Obteniendo Control Disparador
                CheckBox chk = (CheckBox)sender;
                CheckBox chkVarios;

                //Validando el Control
                switch (chk.ID)
                {
                    case "chkTodosConceptos":
                        {
                            //Validando que la fila sea seleccionada
                            if (!chk.Checked)
                            {
                                //Seleccionando Todas las Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturaConceptos, "chkVariosConceptos", chk.Checked);

                                //Visualizando Control
                                btnGuardarConceptos.Visible = false;
                            }
                            else
                            {
                                //Recorriendo cada fila
                                foreach (GridViewRow gvr in gvFacturaConceptos.Rows)
                                {
                                    //Obteniendo Control
                                    chkVarios = (CheckBox)gvr.FindControl("chkVariosConceptos");

                                    //Validando que existe la Fila
                                    if (chkVarios != null)

                                        //Validando que el Control este Habilitado para su Seleccion
                                        chkVarios.Checked = chkVarios.Enabled ? chk.Enabled : false;
                                }

                                //Visualizando Control de Guardado
                                btnGuardarConceptos.Visible = chk.Checked;
                            }

                            break;
                        }
                    case "chkVariosConceptos":
                        {
                            //Obteniendo Control de ENcabezado
                            CheckBox chkHeader = (CheckBox)gvFacturaConceptos.HeaderRow.FindControl("chkTodosConceptos");

                            //Validando que el control se haya desmarcado
                            if (!chk.Checked)

                                //Desmarcando Encabezado
                                chkHeader.Checked = false;

                            //Obteniendo Filas Seleccionadas
                            GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturaConceptos, "chkVariosConceptos");

                            //Validando que existan filas seleccionadas
                            if (gvr.Length > 0)

                                //Visualizando Control de Guardado
                                btnGuardarConceptos.Visible = true;
                            else
                                //Ocultando Control de Guardado
                                btnGuardarConceptos.Visible = false;

                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFacConceptos.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Factura Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacConceptos_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }
        #endregion

        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar - Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFacturas_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            cargaFacturas();
        }
        /// <summary>
        /// Click en botón Importar archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbImportarArchivo_Click(object sender, EventArgs e)
        {
            //Si hay un registro activo en sesión
            if (Convert.ToInt32(Session["id_registro"]) > 0)
            {
                //Construyendo URL 
                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/CuentasCobrar/ImportadorTarifaCobro.aspx");

                //Invoca el método de la clase ScriptServer que abrira una nueva ventana acorde a una ubicacion, un titulo de la ventana, una configuracion de dimensiones.
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idFacturaGlobal={1}", url, Session["id_registro"]), "Importación de Tarifa de Cobro", 1080, 620, false, false, false, true, true, true, Page);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Abriendo Ventana Modal de Conceptos
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");

            //Inicializando GridViews
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarFactura_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno y Variables Auxiliares
            RetornoOperacion result = new RetornoOperacion();
            string[] mensaje_operacion = new string[1];
            int contador = 0;

            //Instanciando Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista
                if (fg.id_factura_global > 0)
                {
                    //Validando el Estatus
                    if (fg.estatus == SAT_CL.Facturacion.FacturaGlobal.Estatus.Registrada)
                    {
                        //Validando el Estatus de Facturación Electronica
                        if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobal(fg.id_factura_global) == 0)
                        {
                            //Validando el Estatus de Facturación Electronica 3.3
                            if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobalV3_3(fg.id_factura_global) == 0)
                            {
                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFactura");

                                //Inicializando Bloque Transaccional
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Validand que existan Conceptos
                                    if (gvr.Length > 0)
                                    {
                                        //Creando Arreglo Dinamico
                                        mensaje_operacion = new string[gvr.Length];

                                        //Iniciando Ciclo
                                        foreach (GridViewRow gv in gvr)
                                        {
                                            //Seleccionando Indice
                                            gvFacturasDisponibles.SelectedIndex = gv.RowIndex;


                                            //Insertando Registro de Relación
                                            result = SAT_CL.Facturacion.FacturadoFacturacion.InsertarFacturadoFacturacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                                            0, Convert.ToInt32(Session["id_registro"]), 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la Operación haya Sido Incorrecta
                                            if (!result.OperacionExitosa)
                                            {
                                                //Obteniendo Mensaje de Error
                                                mensaje_operacion = new string[1];
                                                mensaje_operacion[0] = result.Mensaje;

                                                //Terminando Ciclo
                                                break;
                                            }
                                            else
                                            {
                                                //Guardando Mensaje de la Operación
                                                mensaje_operacion[contador] = "La Factura " + gvFacturasDisponibles.SelectedDataKey["IdFactura"].ToString() + ": Se ha Agregado Exitosamente";

                                                //Incrementando Contador
                                                contador++;
                                            }
                                        }

                                        //Validando que las Operaciones hayan sido exitosas
                                        if (result.OperacionExitosa)
                                        {
                                            //Guardando el Registro
                                            int idReg = result.IdRegistro;

                                            //Personalizando Mensaje
                                            result = new RetornoOperacion(idReg, "*La Factura " + gvFacturasDisponibles.SelectedDataKey["IdFactura"].ToString() + ": Se ha Agregado Exitosamente", true);

                                            //Cargando Facturas
                                            cargaFacturas();

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No hay Conceptos Seleccionados");
                                }
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica(3.3), Imposible su Edición");

                                //Desmarcando Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosConceptos", false);
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica(3.2), Imposible su Edición");

                            //Desmarcando Filas
                            TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosConceptos", false);
                        }
                    }
                    else
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("El Estatus '{0}' de la Factura Global no permite su Edición", fg.estatus));

                        //Desmarcando Filas
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosConceptos", false);
                    }
                }
            }

            //Mostrando Notificaciones
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Mostrando Mensaje de Operación
            lblError.Text = string.Join("<br />", mensaje_operacion);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Conceptos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarConceptos_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista
                if (fg.id_factura_global > 0)
                {
                    //Validando el Estatus
                    if (fg.estatus == SAT_CL.Facturacion.FacturaGlobal.Estatus.Registrada)
                    {
                        //Validando el Estatus de Facturación Electronica
                        if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobal(fg.id_factura_global) == 0)
                        {
                            //Validando el Estatus de Facturación Electronica v3.3
                            if (SAT_CL.Facturacion.FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobalV3_3(fg.id_factura_global) == 0)
                            {
                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvFacturaConceptos, "chkVariosConceptos");

                                //Inicializando Bloque Transaccional
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Validand que existan Conceptos
                                    if (gvr.Length > 0)
                                    {
                                        //Iniciando Ciclo
                                        foreach (GridViewRow gv in gvr)
                                        {
                                            //Seleccionando Indice
                                            gvFacturaConceptos.SelectedIndex = gv.RowIndex;

                                            //Insertando Registro de Relación
                                            result = SAT_CL.Facturacion.FacturadoFacturacion.InsertarFacturadoFacturacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["IdFactura"]),
                                                                                    Convert.ToInt32(gvFacturaConceptos.SelectedDataKey["IdFacturaConcepto"]), Convert.ToInt32(Session["id_registro"]), 0, 0,
                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la Operación haya Sido Incorrecta
                                            if (!result.OperacionExitosa)

                                                //Terminando Ciclo
                                                break;
                                        }

                                        //Validando que las Operaciones hayan sido exitosas
                                        if (result.OperacionExitosa)
                                        {
                                            //Cargando Facturas
                                            cargaFacturas();

                                            //Abriendo Ventana Modal de Conceptos
                                            TSDK.ASP.ScriptServer.AlternarVentana(upbtnGuardarConceptos, upbtnGuardarConceptos.GetType(), "ConceptosDisponibles", "contenedorVentanaConceptos", "ventanaConceptos");

                                            //Inicializando Indices
                                            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

                                            //Completando Transacción
                                            trans.Complete();
                                        }
                                    }
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("No hay Conceptos Seleccionados");
                                }
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica(3.3), Imposible su Edición");

                                //Desmarcando Filas
                                TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturaConceptos, "chkVariosConceptos", false);
                            }
                        }
                        else
                        {
                            //Instanciando Excepción
                            result = new RetornoOperacion("La Factura se encuentra Registrada en Facturación Electronica(3.2), Imposible su Edición");

                            //Desmarcando Filas
                            TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturaConceptos, "chkVariosConceptos", false);
                        }
                    }
                    else
                    {
                        //Instanciando Excepción
                        result = new RetornoOperacion(string.Format("El Estatus '{0}' de la Factura Global no permite su Edición", fg.estatus));

                        //Desmarcando Filas
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvFacturaConceptos, "chkVariosConceptos", false);
                    }
                }
            }

            //Mostrando Notificación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar - Paquete"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarPQ_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Objeto de Retorno
            string msn_operacion = "";

            //Instanciando Factura Global
            using (SAT_CL.Facturacion.PaqueteProceso pq = new SAT_CL.Facturacion.PaqueteProceso(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtPaqPrevio.Text, "ID:", 1, "0"))))
            {
                //Validando que exista un Paquete
                if (pq.id_paquete_proceso > 0)
                {
                    //Obteniendo Detalles
                    using (DataTable dtDetallesPaquete = SAT_CL.Facturacion.PaqueteProcesoDetalle.ObtieneFacturacionPaqueteProceso(pq.id_paquete_proceso))
                    {
                        //Validando que Existan Detalles
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDetallesPaquete))
                        {
                            //Recorriendo Detalles
                            foreach (DataRow dr in dtDetallesPaquete.Rows)
                            {
                                //Instanciando Detalle
                                using (SAT_CL.Facturacion.PaqueteProcesoDetalle ppd = new SAT_CL.Facturacion.PaqueteProcesoDetalle(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que Exista el Detalle
                                    if (ppd.id_paquete_proceso_detalle > 0)
                                    {
                                        //Validando que no exista una Factura Electronica
                                        if (FacturadoFacturacion.ObtieneFacturacionElectronicaActiva(ppd.id_facturado) == 0)

                                            //Insertando Paquete
                                            result = SAT_CL.Facturacion.FacturadoFacturacion.InsertarFacturadoFacturacion(ppd.id_facturado, ppd.id_facturado_detalle,
                                                  Convert.ToInt32(Session["id_registro"]), 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion(ppd.id_facturado, "La Factura se encuentra Registrada Electronicamente", false);

                                        //Si no se agrego
                                        if (!result.OperacionExitosa)

                                            //Obteniendo Mensaje de Operación
                                            msn_operacion += string.Format("Factura No. {0}: {1}.<br />", ppd.id_facturado, result.Mensaje);
                                    }
                                }
                            }
                            //Personalizando Resultado
                            result = new RetornoOperacion("Operación Exitosa.", true);

                            //Cargando Facturas
                            cargaFacturas();

                            //Carga Totales de la Factura Global
                            obtieneTotalesFacturaGlobal();
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede Acceder al Paquete, es posible que haya sido Eliminado");
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se puede Acceder al Paquete, es posible que haya sido Eliminado");
            }

            //Mostrando Mensaje de Error
            lblError.Text = msn_operacion;
            //Mostrando Mensaje de Error
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarPQ, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos Factura Electrónica

        /// <summary>
        /// Evento Generado al Cambiar la FE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoFacturacionElectronica_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando Factura un Solo Concepto
            if (ddlTipoFacturacionElectronica.SelectedValue == "3")
            {
                //Habilitamos Control
                txtNoIdentificacion.Enabled = true;
                txtTotalFactura.Enabled = true;
            }
            else
            {
                //Deshabilitamos Control
                txtNoIdentificacion.Enabled = false;
                txtTotalFactura.Enabled = false;
            }
        }
        /// <summary>
        /// Evento geenrado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbFEReferencias_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            
                //Inicialiozamos Ventana de Referencia
                inicializaReferencias(objFacturaGlobal.id_factura_global.ToString(), "13", "Factura Global");
        }
        /// <summary>
        /// Evento generado al dar clik en link de Registrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkRegistrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Abre Ventana Modal
            ScriptServer.AlternarVentana(lkbRegistrarFacturaElectronica, lkbRegistrarFacturaElectronica.GetType(), "AbrirVentana", "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
            //Instanciamos Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal((Convert.ToInt32(Session["id_registro"]))))
            
                //Cargando Cuentas Pago
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", objFacturaGlobal.id_compania_cliente, "", 0, "");
            
            //Inicializamos Valores
            inicializaValoresRegistroFacturacionElectronica();
        }
        /// <summary>
        /// Evento generado al cerrar la ventana de Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarRegistarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Cerramo Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarRegistarFacturacionElectronica, uplkbCerrarRegistarFacturacionElectronica.GetType(), "CerrarVentanaModal", "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
        }
        /// <summary>
        /// Evento generdo al cerrar la venta de Timbrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarTimbrarFacturacionElectronica, uplkbCerrarTimbrarFacturacionElectronica.GetType(), "CerrarVentana", "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
        }
        /// <summary>
        ///  Evento generado al dar clik en link en Timbrar Facturación Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Abre Ventana Modal
            ScriptServer.AlternarVentana(lkbTimbrarFacturaElectronica, lkbTimbrarFacturaElectronica.GetType(), "AbrirVentana", "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
            //Inicializa Valores 
            inicializaValoresTimbrarFacturacionElectronica();
        }
        /// <summary>
        /// Evebto generado al Registrar la Factura Electrónica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFacturaElectronica_Click(object sender, EventArgs e)
        {
            //Si existe la Tabla de Referencias
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            
                //Eliminamos Tabla
                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
            
            //Validamos que el tipo sea diferente a un solo concepto
            if (ddlTipoFacturacionElectronica.SelectedValue != "3")
            
                //Craga GV Referencias
                cargaReferencias();
            
            //Si existen Referencias
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            {
                //Mostramos Venata Modal
                alternaVentanaModal("ReferenciasRegistro", btnRegistrarFacturaElectronica);
                //Cerramos Ventana Modal de Registro
                alternaVentanaModal("RegistrarFacturacionElectronica", btnRegistrarFacturaElectronica);
            }
            else
            {
                //Declaramos objeto Retorno
                RetornoOperacion resultado = registraFacturacionElectronica();

                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnRegistrarFacturaElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                //Validamos que el tipo sea diferente a un solo concepto
                if (ddlTipoFacturacionElectronica.SelectedValue == "3")
                {
                    //Craga Totales de la FG 
                    obtieneTotalesFacturaGlobal();

                    //Carga Facturas Ligadas
                    cargaFacturas();
                }
            }
        }
        /// <summary>
        /// Evento generado al registrar la FE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFE_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Retorno
            RetornoOperacion resultado = registraFacturacionElectronica();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnRegistrarFE, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Registra la Factura Electrónica
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion registraFacturacionElectronica()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista la Factura
                if (objFacturaGlobal.id_factura_global > 0)
                {
                    //Registramos Factura
                    resultado = objFacturaGlobal.ImportaFacturaGlobal_V3_3(Convert.ToDecimal(txtTotalFactura.Text), Convert.ToByte(ddlFormaPago.SelectedValue), Convert.ToByte(ddlMetodoPago.SelectedValue), Convert.ToInt32(ddlUsoCFDI.SelectedValue), Convert.ToInt32(ddlSucursal.SelectedValue),
                                  ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToByte(ddlTipoFacturacionElectronica.SelectedValue), obtieneReferencias().TrimEnd(','), txtNoIdentificacion.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Si existen Referencias
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
                        {
                            //Cerramo Ventana Modal
                            alternaVentanaModal("ReferenciasRegistro", btnRegistrarFE);
                        }
                        else
                        {
                            //Cerramos Ventana Modal de Registro
                            alternaVentanaModal("RegistrarFacturacionElectronica", btnRegistrarFacturaElectronica);
                        }
                    }
                }
                else
                {
                    //Establecemos Resultado
                    resultado = new RetornoOperacion("No se pueden reecuperar datos complementarios de la Factura Global.");
                }
            }
            //Devolvemos Resultado
            return resultado;
        }
        /// <summary>
        /// Obtiene Referencias de Viaje
        /// </summary>
        /// <returns></returns>
        private string obtieneReferencias()
        {
            //Verificando que existan depósitos seleccionados
            GridViewRow[] Tipo = Controles.ObtenerFilasSeleccionadas(gvReferencias, "chkSeleccionTipo");
            //Declarando Arreglo para almacenar las Referencias
            string Referencias = "0";
            //Si existen 
            if (Tipo.Length > 0)
            {
                //Para cada uno de los controles marcados
                foreach (GridViewRow r in Tipo)
                {
                    //Seleccionando la fila
                    gvReferencias.SelectedIndex = r.RowIndex;
                    //Instanciando egreso por depósito
                    Referencias += gvReferencias.SelectedDataKey["Id"] + ",";
                }
            }
            //Retornamos Valor
            return Referencias != "0" ? Referencias.TrimStart('0') : Referencias;
        }
        /// <summary>
        /// Evento generado al dar click en Aceptar Comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarComentario_Click(object sender, EventArgs e)
        {
            //Actualizamos Comentario   
            RetornoOperacion resultado = actualizaComentario();
            //Cerramos Ventana Modal
            alternaVentanaModal("Comentario", btnAceptarComentario);
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarComentario, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al dar click en Comentario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentario_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana
            alternaVentanaModal("Comentario", lkbComentario);
        }
        /// <summary>
        /// Actualiza el Comentario de la Factura
        /// </summary>
        private RetornoOperacion actualizaComentario()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Comprobante
            using (FE33.Comprobante objComprobante = new FE33.Comprobante(FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobalV3_3(Convert.ToInt32(Session["id_registro"]))))
            {
                //Validamos Factura Electrónica
                if (objComprobante.id_comprobante33 > 0)
                {
                    //Obteniendo Referencias
                    using (DataTable dtRef = SAT_CL.Global.Referencia.CargaReferencias(objComprobante.id_comprobante33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Comentario", 0, "Facturacion Electrónica")))
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
                                    using (SAT_CL.Global.Referencia comentario = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                    {
                                        //Validando que exista la Referencia
                                        if (comentario.habilitar)
                                        {
                                            //Editamos Referencia
                                            resultado = SAT_CL.Global.Referencia.EditaReferencia(comentario.id_referencia, txtComentario.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Terminamos el Ciclo
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Insertando Referencia
                            resultado = SAT_CL.Global.Referencia.InsertaReferencia(objComprobante.id_comprobante33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Comentario", 0, "Facturacion Electrónica"),
                                        txtComentario.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                        }

                    }
                }
                else
                {
                    //Establecemos Error
                    resultado = new RetornoOperacion("No existe Registró Facturación Electrónica");
                }
            }
            //Devolvemos Valor Return
            return resultado;
        }
        /// <summary>
        /// Evento producido al presionar el checkbox "TipoTodos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTipoTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvReferencias.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;
                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvReferencias.FooterRow.FindControl("lblContadorTipo"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvReferencias, "chkSeleccionTipo", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento producido al presionar el cada checkbox de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionTipo_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvReferencias.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvReferencias, "lblContadorTipo");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvReferencias.HeaderRow.FindControl("chkTipoTodos");
                    //deshabilitando seleccion
                    t.Checked = d.Checked;
                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoReferencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoReferencias.SelectedValue));
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView Tipo Referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReferencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoReferencias.Text = Controles.CambiaSortExpressionGridView(gvReferencias, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// Evento generado al dar click en Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAddendaFacturaElectronica_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbAddendaFacturaElectronica, lkbAddendaFacturaElectronica.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");

            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista la Factura
                if (objFacturaGlobal.id_factura_global > 0)
                {
                    //Validamos que exista Relación
                    using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                    {
                        //CargaCatalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlAddenda, 49, "Ninguno", comp.id_compania_emisor, "", comp.id_compania_receptor, "");
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al timbrar la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Registramos Factura
                resultado = objFacturaGlobal.TimbraFacturaGlobal_V3_3(txtSerie.Text, chkOmitirAddenda.Checked, HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"),
                                                                        HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Validamos Resltado
                if (resultado.OperacionExitosa)
                {
                    //Instanciando Factura Global
                    using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Validando que exista un Id Valido
                        if (fg.id_factura_global > 0)
                        {
                            //Inicializando Valor estatus                              
                            ddlEstatus.SelectedValue = fg.id_estatus.ToString();
                        }
                    }
                }

            }
            //Mostramos Mensaje
            lblTimbrarFacturacionElectronica.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Evento generado al Cerrar Addenda Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarwucAddendaComprobante_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarwucAddendaComprobante, uplkbCerrarwucAddendaComprobante.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");

        }

        /// <summary>
        /// Evento generado al cambiar el método de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*/Si el Método de pago es efectivo
            if (ddlMetodoPago.SelectedValue == "9" || ddlMetodoPago.SelectedValue == "17")
            {
                //Asignamos cuenta como NO IDENTIFICADO
                ddlCuenta.SelectedValue = "0";
                //Deshabilitamos Control
                ddlCuenta.Enabled = false;
            }
            else
            {
                //En caso de existir cuenta
                if (ddlCuenta.Items.Count > 1)
                {
                    //Asignamos cuenta por default
                    ddlCuenta.SelectedValue = ddlCuenta.Items[1].Value;
                    //Habilitamos Control
                    ddlCuenta.Enabled = true;
                }
            }//*/
        }
        /// <summary>
        /// Evento generado al Cerrar Addenda Comprobante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAddendaComprobante_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarAddenda, lkbCerrarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
        }
        /// <summary>
        /// Evento generado al dar click en Aceptar Addenda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAddenda_Click(object sender, EventArgs e)
        {
            //Validamos que exista Addenda Seleccionada
            if (ddlAddenda.SelectedValue != "0")
            {
                //Instanciando Comprobante
                using (SAT_CL.Facturacion.FacturaGlobal Fac = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                {
                    //Validamos Id Comprobante
                    if (Fac.id_factura_electronica33 > 0)
                    {
                        //Inicializamos Control
                        wucAddendaComprobante.InicializaControl("3.3", Fac.id_factura_electronica33, Convert.ToInt32(ddlAddenda.SelectedValue));
                        //Cerrar Ventana Modal
                        ScriptServer.AlternarVentana(btnAceptarAddenda, btnAceptarAddenda.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
                        //Abrir Ventana Modal
                        ScriptServer.AlternarVentana(btnAceptarAddenda, upbtnAceptarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
                    }
                    else if (Fac.id_factura_electronica > 0)
                    {
                        //Inicializamos Control
                        wucAddendaComprobante.InicializaControl("3.2", Fac.id_factura_electronica, Convert.ToInt32(ddlAddenda.SelectedValue));
                        //Cerrar Ventana Modal
                        ScriptServer.AlternarVentana(btnAceptarAddenda, btnAceptarAddenda.GetType(), "CerrarVentana", "contenidoConfirmacionAddenda", "confirmacionAddenda");
                        //Abrir Ventana Modal
                        ScriptServer.AlternarVentana(btnAceptarAddenda, upbtnAceptarAddenda.GetType(), "AbrirVentana", "contenidoConfirmacionWucComprobanteAddenda", "confirmacionWucComprobanteAddenda");
                    }
                    else
                    {
                        lblErrorAddenda.Text = "No se encontró registró Factura Electrónica";
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al dar click en "Ver CFDI"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerComprobante_Click(object sender, EventArgs e)
        {
            //Instanciamos Comprobante
            using (FE33.Comprobante objComprobante = new FE33.Comprobante(FacturadoFacturacion.ObtieneFacturaElectronicaFacturaGlobalV3_3(Convert.ToInt32(Session["id_registro"]))))
            {
                //Validamos que exista el Comprobante
                if (objComprobante.id_comprobante33 > 0)
                {
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/FacturacionElectronica33/ComprobanteV33.aspx");

                    //Estatus de Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;

                    //Instanciando nueva ventana de navegador para apertura de registro
                    Response.Redirect(string.Format("{0}?idRegistro={1}", urlReporte, objComprobante.id_comprobante33));
                }
                else
                    //Mostrando Notificación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No existe la Factura Electrónica", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();

            //Poner el cursor en el primer control
            ddlEstatus.Focus();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo de Estatus de Factura
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 83);
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacConceptos, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacDisp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacLigadas, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFormaPago, 185, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 3195);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoFacturacionElectronica, "", 3142);
            //Sucursales
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Catalogos de Tamaño  Referencias de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoReferencias, "", 26);
            //Inicializamos Valor
            ddlTamanoReferencias.SelectedValue = "100";
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
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;
                        //FE
                        lkbRegistrarFacturaElectronica.Enabled =
                        lkbTimbrarFacturaElectronica.Enabled =
                        lkbVerComprobante.Enabled =
                        lkbAddendaFacturaElectronica.Enabled =
                        lkbEliminarCFDI.Enabled =
                        lkbCancelarCFDI.Enabled =
                        lkbPDF.Enabled =
                        lkbXML.Enabled =
                        lkbFEReferencias.Enabled =
                        lkbEmail.Enabled = false;

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
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;
                        //FE
                        lkbRegistrarFacturaElectronica.Enabled =
                        lkbTimbrarFacturaElectronica.Enabled =
                        lkbVerComprobante.Enabled =
                        lkbAddendaFacturaElectronica.Enabled =
                        lkbEliminarCFDI.Enabled =
                        lkbCancelarCFDI.Enabled =
                        lkbPDF.Enabled =
                        lkbXML.Enabled =
                        lkbFEReferencias.Enabled =
                        lkbEmail.Enabled = false;

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
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;
                        //FE
                        lkbRegistrarFacturaElectronica.Enabled =
                        lkbTimbrarFacturaElectronica.Enabled =
                        lkbAddendaFacturaElectronica.Enabled =
                        lkbVerComprobante.Enabled =
                        lkbEliminarCFDI.Enabled =
                        lkbCancelarCFDI.Enabled =
                        lkbPDF.Enabled =
                        lkbXML.Enabled =
                        lkbFEReferencias.Enabled =
                        lkbEmail.Enabled = true;

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
                    {
                        //Asignando Valores
                        txtDescripcion.Enabled =
                        txtCliente.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        txtReferencia.Enabled =
                        txtPaqPrevio.Enabled =
                        btnBuscarFacturas.Enabled = false;
                        break;
                    }

                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Valores
                        txtDescripcion.Enabled =
                        txtCliente.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        txtReferencia.Enabled =
                        txtPaqPrevio.Enabled =
                        btnBuscarFacturas.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Valores
                        txtDescripcion.Enabled =
                        txtCliente.Enabled =
                        btnGuardar.Enabled =
                        txtPaqPrevio.Enabled = false;
                        btnCancelar.Enabled = false;
                        txtReferencia.Enabled =
                        btnBuscarFacturas.Enabled = true;
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
                        lblNoFactura.Text = "Por Asignar";
                        txtCliente.Text =
                        txtDescripcion.Text =
                        txtReferencia.Text = "";

                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);
                        TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);
                        TSDK.ASP.Controles.InicializaGridview(gvFacturaConceptos);

                        //Asignando Valores
                        lblSubtotal.Text = string.Format("{0:0.00}", 0);
                        lblTrasladado.Text = string.Format("{0:0.00}", 0);
                        lblRetenido.Text = string.Format("{0:0.00}", 0);
                        lblTotal.Text = string.Format("{0:0.00}", 0);
                        txtTotalFactura.Text = string.Format("{0:0.00}", 0);

                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Producto
                        using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (fg.id_factura_global > 0)
                            {
                                //Inicializando Valores
                                lblNoFactura.Text = fg.no_factura_global;
                                txtDescripcion.Text = fg.descripcion;
                                ddlEstatus.SelectedValue = fg.id_estatus.ToString();
                                txtReferencia.Text = "";

                                //Instanciando Cliente
                                using (SAT_CL.Global.CompaniaEmisorReceptor cli = new SAT_CL.Global.CompaniaEmisorReceptor(fg.id_compania_cliente))
                                {
                                    //Validando que Exista el Registro
                                    if (cli.id_compania_emisor_receptor > 0)

                                        //Asignando Cliente
                                        txtCliente.Text = cli.nombre + " ID:" + cli.id_compania_emisor_receptor.ToString();
                                    else
                                        //Limpiando Control
                                        txtCliente.Text = "";
                                }

                                //Cargando Facturas
                                cargaFacturas();
                            }
                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Factura Global
        /// </summary>
        private void guardaFacturaGlobal()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Insertando Registro
                        result = SAT_CL.Facturacion.FacturaGlobal.InsertarFacturaGlobal(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "",
                                                                            SAT_CL.Facturacion.FacturaGlobal.Estatus.Registrada, 0, 0, txtDescripcion.Text,
                                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Factura Global
                        using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista una Factura Global
                            if (fg.id_factura_global > 0)

                                //Insertando Registro
                                result = fg.EditarFacturaGlobal(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                fg.estatus, fg.id_factura_electronica, fg.id_factura_electronica33, txtDescripcion.Text,
                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }

                        break;
                    }
            }

            //Validando que la Operación haya sido exitosa
            if (result.OperacionExitosa)
            {
                //Asignando valor a Session Registro
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Edicion;

                //Inicializando la Página
                inicializaPagina();

                //Carga Paqutes Terminados
                cargaPaquetesTerminados();
            }

            //Mostrando Mensaje de la Operación
            lblError.Text = result.Mensaje;
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);

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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());

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
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);

            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Cargar las Facturas 
        /// </summary>
        public void cargaFacturas()
        {
            //Instanciando Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que exista el Registro
                if (fg.id_factura_global > 0)
                {
                    //Cargando Facturas
                    using (DataSet ds = SAT_CL.Facturacion.FacturadoConcepto.CargaFacturasTodas(fg.id_factura_global, fg.id_compania_cliente,
                                        ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtReferencia.Text))
                    {
                        /** Facturas Disponibles **/
                        //Validando que existan Facturas Disponibles
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvFacturasDisponibles, ds.Tables["Table"], "IdFactura", lblOrdenadoFacDisp.Text, true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                        }

                        /** Facturas Ligadas **/
                        //Validando que existan Facturas Ligadas
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvFacturasLigadas, ds.Tables["Table1"], "IdFactura-IdFF", lblOrdenadoFacLigadas.Text, true, 2);

                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);

                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        }
                    }

                    //Cargando Totales
                    obtieneTotalesFacturaGlobal();
                }
                else
                {
                    //Inicializando GridViews
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasLigadas);
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasDisponibles);

                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
                TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);

                //Invocando Métodos de Suma
                sumaTotalesFacturasDisponibles();
                sumaTotalesFacturasLigadas();
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener los Totales de la Factura Global
        /// </summary>
        private void obtieneTotalesFacturaGlobal()
        {
            //Obteniendo Totales
            using (DataTable dtTotales = SAT_CL.Facturacion.FacturadoFacturacion.ObtieneTotalesFacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtTotales.Rows)
                    {
                        //Asignando Valores
                        lblSubtotal.Text = string.Format("{0:C2}", dr["SubTotal"]);
                        lblTrasladado.Text = string.Format("{0:C2}", dr["TrasladadoFac"]);
                        lblRetenido.Text = string.Format("{0:C2}", dr["RetenidoFac"]);
                        lblTotal.Text = string.Format("{0:C2}", dr["Total"]);
                        txtTotalFactura.Text = string.Format("{0:0.00}", dr["Total"]);
                    }
                }
                else
                {
                    //Asignando Valores
                    lblSubtotal.Text = string.Format("{0:C2}", 0);
                    lblTrasladado.Text = string.Format("{0:C2}", 0);
                    lblRetenido.Text = string.Format("{0:C2}", 0);
                    lblTotal.Text = string.Format("{0:C2}", 0);
                    txtTotalFactura.Text = string.Format("{0:0.00}", 0);
                }
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales (Facturas Disponibles)
        /// </summary>
        private void sumaTotalesFacturasDisponibles()
        {
            //Validando que exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvFacturasDisponibles.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalFlete)", "")));
                gvFacturasDisponibles.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalOtros)", "")));
                gvFacturasDisponibles.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IVA)", "")));
                gvFacturasDisponibles.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retencion)", "")));
                gvFacturasDisponibles.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Subtotal)", "")));
                gvFacturasDisponibles.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales
                gvFacturasDisponibles.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
                gvFacturasDisponibles.FooterRow.Cells[10].Text = string.Format("{0:C2}", 0);
                gvFacturasDisponibles.FooterRow.Cells[11].Text = string.Format("{0:C2}", 0);
                gvFacturasDisponibles.FooterRow.Cells[12].Text = string.Format("{0:C2}", 0);
                gvFacturasDisponibles.FooterRow.Cells[13].Text = string.Format("{0:C2}", 0);
                gvFacturasDisponibles.FooterRow.Cells[14].Text = string.Format("{0:C2}", 0);
            }
            //Asignamos Estilo
            gvFacturasDisponibles.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasDisponibles.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasDisponibles.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasDisponibles.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasDisponibles.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasDisponibles.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
        }
        /// <summary>
        /// Método encargado de Sumar los Totales (Facturas Ligadas)
        /// </summary>
        private void sumaTotalesFacturasLigadas()
        {
            //Validando que exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
            {
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(TotalFlete)", "")));
                gvFacturasLigadas.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(TotalOtros)", "")));
                gvFacturasLigadas.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(IVA)", "")));
                gvFacturasLigadas.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Retencion)", "")));
                gvFacturasLigadas.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Subtotal)", "")));
                gvFacturasLigadas.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales
                gvFacturasLigadas.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
                gvFacturasLigadas.FooterRow.Cells[10].Text = string.Format("{0:C2}", 0);
                gvFacturasLigadas.FooterRow.Cells[11].Text = string.Format("{0:C2}", 0);
                gvFacturasLigadas.FooterRow.Cells[12].Text = string.Format("{0:C2}", 0);
                gvFacturasLigadas.FooterRow.Cells[13].Text = string.Format("{0:C2}", 0);
                gvFacturasLigadas.FooterRow.Cells[14].Text = string.Format("{0:C2}", 0);
            }
            //Establecemos Estilos del Grid View
            gvFacturasLigadas.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
            gvFacturasLigadas.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
        }

        #endregion

        #region Métodos Facturación Electrónica

        /// <summary>
        /// Inicializa Valores para el registro a Facturación Electrónica
        /// </summary>
        private void inicializaValoresRegistroFacturacionElectronica()
        {
            ddlTipoFacturacionElectronica.SelectedValue = "1";
            txtNoIdentificacion.Text = "";

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

            //Instanciamos Factura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal((Convert.ToInt32(Session["id_registro"]))))
            {
                //Cargando Cuentas Pago
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUsoCFDI, 108, "Ninguno", objFacturaGlobal.id_compania_cliente, "", 0, "");
            }
            lblErrorFacturacionElectronica.Text = "";
            txtTotalFactura.Enabled = false;
        }

        /// <summary>
        /// Inicializa Valores para el Timbrado de la Facturación Electrónica
        /// </summary>
        private void inicializaValoresTimbrarFacturacionElectronica()
        {
            txtSerie.Text = "";
            chkOmitirAddenda.Enabled = true;
            lblTimbrarFacturacionElectronica.Text = "";
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        private void inicializaReferencias(string id_registro, string id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }

        #endregion

        #region FE

        /// <summary>
        /// Envia Email
        /// </summary>
        private void Email()
        {
            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Declaramos variable errores
                List<string> errores = new List<string>();
                //Declaramos resultado
                RetornoOperacion resultado = new RetornoOperacion();
                //Instanciamos Comprobamte
                using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                {
                    //Enviamos link
                    resultado = objCompobante.EnviaEmail(18, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).email, txtAsunto.Text, txtMensaje.Text, out errores, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Recorremos errores
                        foreach (string error in errores)
                        {
                            //Muestra mensaje de Error
                            lblErrorEmail.Text += error + " <br>";
                        }
                    }
                    lblErrorEmail.Text += resultado.Mensaje;
                }
            }
        }

        /// <summary>
        /// Realiza la descarga del XML del comprobante
        /// </summary>
        private void descargarXML()
        {
            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Factura Electrónica
                if (objFacturaGlobal.id_factura_electronica33 > 0)
                {
                    //Instanciando registro en sesión
                    using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                    {
                        //Si existe y está generado
                        if (c.bit_generado)
                        {
                            //Obteniendo bytes del archivo XML
                            byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);

                            //Realizando descarga de archivo
                            if (cfdi_xml.Length > 0)
                            {
                                //Instanciando al emisor
                                using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                                    TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}{1}_{2}.xml", !em.nombre_corto.Equals("") ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Método encargado de Buscar las Referencias de Viajes
        /// </summary>
        private void cargaReferencias()
        {
            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtReferencia = SAT_CL.Facturacion.Reporte.ObtienesDatosReferenciasFacturaGlobal(Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtReferencia))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvReferencias, dtReferencia, "Id", "", false, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtReferencia, "Table2");
                }
                else
                {
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
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
                case "Comentario":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoComentario", "confirmacionComentario");
                    break;
                case "ReferenciasRegistro":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "RegistrarFacturacionElectronica":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
                    break;
            }
        }
        /// <summary>
        /// Cancelar CFDI
        /// </summary>
        private void CancelaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando Factura Electronica
                if (objFacturaGlobal.habilitar && objFacturaGlobal.id_factura_electronica > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaGlobal.id_factura_electronica))
                    {
                        //Cancelamos Comprobante
                        resultado = objCompobante.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Insertando Referencia
                            resultado = SAT_CL.Global.Referencia.InsertaReferencia(objCompobante.id_comprobante, 119, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 119, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                        txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);

                            //Validamos Resltado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando Factura Global
                                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Validando que exista un Id Valido
                                    if (fg.id_factura_global > 0)
                                    {
                                        //Inicializando Valor estatus                              
                                        ddlEstatus.SelectedValue = fg.id_estatus.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (objFacturaGlobal.habilitar && objFacturaGlobal.id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                    {
                        //Cancelamos Comprobante
                        resultado = objCompobante.CancelacionPendiente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Insertando Referencia
                            resultado = SAT_CL.Global.Referencia.InsertaReferencia(objCompobante.id_comprobante33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                        txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);

                            //Validamos Resltado
                            if (resultado.OperacionExitosa)
                            {
                                //Instanciando Factura Global
                                using (SAT_CL.Facturacion.FacturaGlobal fg = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Validando que exista un Id Valido
                                    if (fg.id_factura_global > 0)
                                    {
                                        //Inicializando Valor estatus
                                        ddlEstatus.SelectedValue = fg.id_estatus.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("No existe la Factura Electrónica");
            }
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnAceptarCancelacionCFDI, upbtnAceptarCancelacionCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");

            lblError.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Eliminar CFDI
        /// </summary>
        private void EliminaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando Facturación CFDI
                if (objFacturaGlobal.id_factura_electronica > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica.Comprobante objCompobante = new SAT_CL.FacturacionElectronica.Comprobante(objFacturaGlobal.id_factura_electronica))
                    {
                        //Validando CFDI 3.2
                        if (objCompobante.habilitar)

                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se puede recuperar el CFDI v3.2");
                    }
                }
                else if (objFacturaGlobal.id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobamte
                    using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                    {
                        //Validando CFDI 3.2
                        if (objCompobante.habilitar)

                            //Enviamos link
                            resultado = objCompobante.DeshabilitaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            resultado = new RetornoOperacion("No se puede recuperar el CFDI v3.3");
                    }
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("No existe la Factura Electrónica");
            }

            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(this, "CerrarVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento generado al Abrir la venta E-mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEmail_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbEmail, lkbEmail.GetType(), "AbrirVentanaModal", "contenidoConfirmacionEmail", "confirmacionEmail");
        }
        /// Evento generado al cerrar la ventana Moda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarEmail_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarEmail, uplkbCerrarEmail.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEmail", "confirmacionEmail");
        }
        /// <summary>
        /// Evento generado al dar click en Aceptar E-mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEmail_Click(object sender, EventArgs e)
        {
            //Envia Email
            Email();
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

            //De acuerdo al comando del botón
            switch (lnk.CommandName)
            {
                case "comentario":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Comentario", lnk);
                    break;
                case "referenciasRegistro":
                    //Cerrando ventana modal 
                    alternaVentanaModal("ReferenciasRegistro", lnk);
                    break;
            }
        }
        /// <summary>
        /// Evento generado al Eliminar un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarCFDI_Click(object sender, EventArgs e)
        {
            EliminaCFDI();
        }

        /// <summary>
        /// Evento generado al Cancelra CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminarCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarEliminarCFDI, upbtnCancelarEliminarCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");


        }

        /// <summary>
        /// Evento generado al Aceptar la Cancelación un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            CancelaCFDI();
        }

        /// <summary>
        /// Evento generado al Cancelar  la  Cancelación de un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarCancelacionCFDI, upbtnCancelarCancelacionCFDI.GetType(), "CerrarVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");

        }

        /// <summary>
        /// Evento generado al Eliminar un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminarCFDI_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbEliminarCFDI, lkbEliminarCFDI.GetType(), "AbrirVentanaModal", "contenidoConfirmacionEliminarCFDI", "confirmacionEliminarCFDI");

        }

        /// <summary>
        /// Evento generado al cancelar CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelarCFDI_Click(object sender, EventArgs e)
        {
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(lkbCancelarCFDI, lkbCancelarCFDI.GetType(), "AbrirVentanaModal", "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");


        }

        /// <summary>
        /// Evento generado al exportar a PDF un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPDF_Click(object sender, EventArgs e)
        {
            //Obteniendo Ruta  Convert.ToInt32(Session["id_registro"])
            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/FacturaGlobalV33.aspx", "~/RDLC/Reporte.aspx");
            //Instanciamos Fcatura Global
            using (SAT_CL.Facturacion.FacturaGlobal objFacturaGlobal = new SAT_CL.Facturacion.FacturaGlobal(Convert.ToInt32(Session["id_registro"])))
            {
                //Validamos que exista Factura Electrónica
                if (objFacturaGlobal.id_factura_electronica33 > 0)
                {
                    //Instanciamos Comprobante
                    using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(objFacturaGlobal.id_factura_electronica33))
                    {
                        //Validamos que el comprobante se encuentre Timbrado
                        if (objComprobante.id_comprobante33 > 0)
                        {
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteV33", objFacturaGlobal.id_factura_electronica33), "Comprobante v3.3", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al descargar el XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbXML_Click(object sender, EventArgs e)
        {
            //Descarga XML
            descargarXML();
        }
        #endregion       
    }
}