using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using fe33 = SAT_CL.FacturacionElectronica33;
using seguridad = SAT_CL.Seguridad;
using TSDK.Base;
using TSDK.Datos;
using TSDK.ASP;
using SAT_CL.Bancos;
using SAT_CL;

namespace SAT.FacturacionElectronica33
{
    public partial class ComprobanteRecepcionPagosV10 : System.Web.UI.Page
    {
        #region Generales

        #region Eventos
        
        /// <summary>
        /// Evento de carga de formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recarga de página
            if (!IsPostBack)
                //inicialziando contenido de formulario
                inicializarForma();
        }
        /// <summary>
        /// Click en opción del GV de clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkClienteResumen_Click(object sender, EventArgs e)
        {
            //Seleccionando fila solicitada
            Controles.SeleccionaFila(gvClientes, sender, "lnk", false);
            //Realizando carga inicial de contenido de pastañas
            inicializarPestanaActiva(0);
        }
        /// <summary>
        /// Click en botón buscar cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Determinando que botón se pulsó
            switch (((Button)sender).CommandName)
            {
                case "Cliente":
                    //Recuperando Id de CLiente
                    string id_cliente = Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1);
                    //Realizando carga de resumen por cliente y localizando cliente siolicitado para su selección
                    cargarResumenCliente();
                    Controles.MarcaFila(gvClientes, id_cliente);
                    //Cargando contenido de pestaña predeterminada
                    inicializarPestanaActiva(0);
                    break;
                case "CFDI":
                    //Actualizando lista de CFDI
                    configurarPestanaBusquedaCFDI();
                    break;
            }
            
        }
        /// <summary>
        /// Cambio en tamaño de página de GV de Clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGVClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aplicando criterio de visualización
            Controles.CambiaTamañoPaginaGridView(gvClientes, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table"), Convert.ToInt32(ddlTamanoGVClientes.SelectedValue), true, 0);

            //Realizando carga inicial de contenido de pastañas
            inicializarPestanaActiva(0);
        }
        /// <summary>
        /// Cambio de página de GV de Clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando criterio de visualización
            Controles.CambiaIndicePaginaGridView(gvClientes, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table"), e.NewPageIndex, true, 0);

            //Realizando carga inicial de contenido de pastañas
            inicializarPestanaActiva(0);
        }
        /// <summary>
        /// Cambio de Orden del GV de Clientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvClientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Aplicando criterio de visualización
            lblOrdenadoPorGVClientes.Text = Controles.CambiaSortExpressionGridView(gvClientes, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table"), e.SortExpression, true, 0);

            //Realizando carga inicial de contenido de pastañas
            inicializarPestanaActiva(0);
        }
        /// <summary>
        /// Click en botón de exportación a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            switch (((LinkButton)sender).CommandName)
            {
                case "PagosDisponibles":
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "*".ToArray());
                    break;
                case "PagosCFDI":
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "*".ToArray());
                    break;
                case "Clientes":
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToArray());
                    break;
                case "CFDIPendientes":
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Consultar", "Sustituir");
                    break;
            }
        }
        /// <summary>
        /// Click en botón de Cambio de Vista activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            switch (((Button)sender).CommandName)
            {
                case "InformacionCFDI":
                    inicializarPestanaActiva(0);
                    break;
                case "CFDIPendientes":
                    inicializarPestanaActiva(1);
                    break;
            }
        }        

        #endregion

        #region Métodos

        /// <summary>
        /// Carga los catálogos con elementos estáticos de uso común en el formulario
        /// </summary>
        private void cargarCatalogosEstaticos()
        {
            //Tamaño de GV Clientes
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGVClientes, "", 56);
            //Tamaño de GV Pagos Disponibles
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGVPagosDisponibles, "", 26);
            //Tamaño de GV Pagos CFDI
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGVPagosCFDI, "", 26);
            //Tamaño de GV de CFDI
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGVCFDIPagos, "", 26);
            //Sucursales del EMisor del CFDI
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }
        /// <summary>
        /// Realiza la configuración incial del formulario web
        /// </summary>
        private void inicializarForma()
        {
            //Definiendo estatus de formulario
            Session["estatus"] = Pagina.Estatus.Nuevo;
            Session["id_registro"] = 0;

            //Cargando catálogos estáticos
            cargarCatalogosEstaticos();
            //Cargando GV de Resumen de Cliente
            cargarResumenCliente();
            //Asignando Indice predeterminado de Vista activa (0 - Información de CFDI - Estatus Nuevo)
            inicializarPestanaActiva(0);            
        }
        /// <summary>
        /// Carga el GV de Resumen por cliente y limpia el filtro de búsqueda
        /// </summary>
        private void cargarResumenCliente()
        {
            //Recuperando resumen desde BD
            using (DataTable mit = fe33.Reporte.ObtenerResumenPendientesCFDIPagosPorCliente(((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Cargando GV
                Controles.CargaGridView(gvClientes, mit, "IdCliente", "", true, 0);

                //Si hay datos
                if (mit != null)
                    //Preservando en sesión la información
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet(((DataSet)Session["DS"]), mit, "Table");
                else
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet(((DataSet)Session["DS"]), "Table");

                //Limpiando filtro
                txtCliente.Text = "";
            }
        }
        /// <summary>
        /// Configura el contenido de la vista solicitada (considerando vistas secundarias)
        /// </summary>
        /// <param name="indice">Indice base 0 a definir como actuvo en el elemento Multiview</param>
        private void inicializarPestanaActiva(int indice)
        {
            //Asignando indice
            mtvSolicitud.ActiveViewIndex = indice;

            //Determinando indice activo solicitado
            switch (indice)
            {
                //Información de CFDI
                case 0:
                    //Aplicando Estilos de botónes (pestañas)
                    btnInformacionCFDI.CssClass = "boton_pestana_activo";
                    btnCFDIPendientes.CssClass = "boton_pestana";

                    //Mostrando contenido de Pestaña
                    configurarPestanaInformacionCFDI();
                    
                    break;
                //CFDI con Existentes
                case 1:
                    //Aplicando Estilos de botónes (pestañas)
                    btnInformacionCFDI.CssClass = "boton_pestana";
                    btnCFDIPendientes.CssClass = "boton_pestana_activo";

                    //Mostrando contenido de pestaña
                    configurarPestanaBusquedaCFDI();
                    break;
            }            
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
                case "RegistrarCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionRegistarFacturacionElectronica", "confirmacionRegistarFacturacionElectronica");
                    break;
                case "TimbrarCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionTimbrarFacturacionElectronica", "confirmaciontimbrarFacturacionElectronica");
                    break;
                case "EliminarCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionEliminarFacturacionElectronica", "confirmacionEliminarFacturacionElectronica");
                    break;
                case "SustituirCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionSustituirFacturacionElectronica", "confirmacionSustituirFacturacionElectronica");
                    break;
            }
        }

        #endregion

        #endregion

        #region Pestaña Información CFDI

        #region Eventos

        /// <summary>
        /// Click en alguna opción del menún de información de CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {

            //Determinando la acción a realizar
            LinkButton lnk = (LinkButton)sender;
            switch (lnk.CommandName)
            {
                case "Nuevo":
                    //Nuevo registro
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    Session["id_registro"] = "0";

                    //Actualizando contenido de pestaña
                    inicializarPestanaActiva(0);

                    break;
                case "Timbrar":
                    //Validando que exista un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                    {                        
                        //Mostrando ventana
                        alternaVentanaModal("TimbrarCFDI", lkbTimbrarFacturaElectronica);
                        //Inicializando contenido de ventana
                        txtSerie.Text = "";
                        //Estableciendo foco
                        txtSerie.Focus();
                    }
                    break;
                case "Eliminar":
                    //Validando que exista un registro en sesión
                    if (Session["id_registro"].ToString() != "0")                    
                        //Mostrando ventana de confirmación
                        alternaVentanaModal("EliminarCFDI", lkbEliminarCFDI);
                    break;
                case "PDF":
                case "XML":
                    //Instanciando registro comprobante
                    using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el cfd existe
                        if (cfdi.habilitar)
                        {
                            //Si es PDF
                            if (lnk.CommandName == "PDF")
                            {
                                //Obteniendo Ruta
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteRecepcionPagosV10.aspx", "~/RDLC/Reporte.aspx");

                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobantePago", cfdi.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            }
                            //Si es XML
                            else
                            {
                                //Si existe y está generado
                                if (cfdi.bit_generado)
                                {
                                    //Obteniendo bytes del archivo XML
                                    byte[] cfdi_xml = System.IO.File.ReadAllBytes(cfdi.ruta_xml);

                                    //Realizando descarga de archivo
                                    if (cfdi_xml.Length > 0)
                                    {
                                        //Instanciando al emisor
                                        using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                                            TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, cfdi.serie, cfdi.folio), TSDK.Base.Archivo.ContentType.text_xml);
                                    }
                                }
                            }
                        }
                        else
                            //Mostrando error
                            ScriptServer.MuestraNotificacion(lnk, "El CFDI no se ha recuperado, es posible que ya no exista en BD.", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    break;
                case "Bitacora":
                    inicializaBitacora(Session["id_registro"].ToString(), "209", "Bitácora de CFDI Recepción Pagos");
                    break;
                case "Referencias":
                    inicializaReferenciaRegistro(Session["id_registro"].ToString(), "209", ((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                    break;
            }            
        }
        /// <summary>
        /// Click en botón de acción (Agregar/Quitar) pagos de CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPagos_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //Determinando que botón fue pulsado
            switch (btn.CommandName)
            {
                case "Agregar":
                    //Validando que existan registros por agregar
                    if (gvPagosDisponibles.DataKeys.Count > 0)
                    {
                        //Si no un registro activo en sesión (nuevo CFDI)
                        if (((Pagina.Estatus)Session["estatus"]) == Pagina.Estatus.Nuevo)
                            //Realizando apertura de ventana modal de registro de CFDI
                            alternaVentanaModal("RegistrarCFDI", btn);
                        //Si se está editando un CFDI
                        else if (((Pagina.Estatus)Session["estatus"]) == Pagina.Estatus.Edicion)
                            //Agregando pagoas a CFDI
                            agregarPagosCFDI();
                        //Si es sólo consulta
                        else
                            //Mostrando error por estatus invalido para operación
                            ScriptServer.MuestraNotificacion(btn, "El estatus actual es de sólo Consulta.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando error por inexistencia de elementos
                        ScriptServer.MuestraNotificacion(btn, "No hay Pagos por Agregar.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    break;
                case "Quitar":
                    //Validando que existan registros por agregar
                    if (gvPagosEnCFDI.DataKeys.Count > 0)
                    {
                        //Si se está editando un CFDI
                        if (((Pagina.Estatus)Session["estatus"]) == Pagina.Estatus.Edicion)
                            //Quitando de CFDI
                            quitarPagosCFDI();
                        //Si es sólo consulta
                        else
                            //Mostrando error por estatus invalido para operación
                            ScriptServer.MuestraNotificacion(btn, "El estatus actual es de sólo Consulta.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando error por inexistencia de elementos
                        ScriptServer.MuestraNotificacion(btn, "No hay Pagos por Quitar.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    break;
            }
        }
        /// <summary>
        /// Cambio de tamaño de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGVPagosDisponibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aplicando visualización de GV de Pagos Disponibles
            Controles.CambiaTamañoPaginaGridView(gvPagosDisponibles, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table1"), Convert.ToInt32(ddlTamanoGVPagosDisponibles.SelectedValue), true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosDisponibles);

            //Ocultando acciones sobre pagos
            btnAgregarPagos.Visible = false;
        }
        /// <summary>
        /// Cambio de página en GV de Pagos Disponibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando visualización de GV de Pagos Disponibles
            Controles.CambiaIndicePaginaGridView(gvPagosDisponibles, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table1"), e.NewPageIndex, true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosDisponibles);

            //Ocultando acciones sobre pagos
            btnAgregarPagos.Visible = false;
        }
        /// <summary>
        /// Orden GV Pagos Disponibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Aplicando visualización de GV de Pagos Disponibles
            lblOrdenadoPorGVPagosDisponibles.Text = Controles.CambiaSortExpressionGridView(gvPagosDisponibles, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table1"), e.SortExpression, true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosDisponibles);

            //Ocultando acciones sobre pagos
            btnAgregarPagos.Visible = false;
        }
        /// <summary>
        /// Cambio de tamaño de página GV Pagos en CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGVPagosCFDI_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aplicando visualización de GV de Pagos en CFDI
            Controles.CambiaTamañoPaginaGridView(gvPagosEnCFDI, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table2"), Convert.ToInt32(ddlTamanoGVPagosCFDI.SelectedValue), true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosEnCFDI);

            //Ocultando acciones sobre pagos
            btnQuitarPagos.Visible = false;
        }
        /// <summary>
        /// Cambio de indice de página GV Pagos en CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosEnCFDI_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando visualización de GV de Pagos en CFDI
            Controles.CambiaIndicePaginaGridView(gvPagosEnCFDI, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table2"), e.NewPageIndex, true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosEnCFDI);

            //Ocultando acciones sobre pagos
            btnQuitarPagos.Visible = false;
        }
        /// <summary>
        /// Cambio de orden GV Pagos en CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagosEnCFDI_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Aplicando visualización de GV de Pagos Disponibles
            lblOrdenadoPorGVPagosCFDI.Text = Controles.CambiaSortExpressionGridView(gvPagosEnCFDI, OrigenDatos.RecuperaDataTableDataSet(((DataSet)Session["DS"]), "Table2"), e.SortExpression, true, 2);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosEnCFDI);

            //Ocultando acciones sobre pagos
            btnQuitarPagos.Visible = false;
        }
        /// <summary>
        /// Realiza las acciones de des/selección de elementos de pago desde y hacia un CFDI de PAGO (Auxiliar en operaciones Agregar y Quitar Pagos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPagosTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Determinando que control está siendo usado 
            CheckBox chk = (CheckBox)sender;

            //Clasificando entre las operaciones posibles
            switch (chk.ID)
            {
                //Cabecera de GV Pagos Disponibles
                case "chkPagosTodosDisp":
                    //Validando que existan registros en el GV
                    if (gvPagosDisponibles.DataKeys.Count > 0)
                    {
                        //Seleccionando Todas las Filas
                        Controles.SeleccionaFilasTodas(gvPagosDisponibles, "chkPagosVariosDisp", chk.Checked);

                        //Aplicando criterio de habilitación
                        btnAgregarPagos.Visible = chk.Checked;
                    }
                    else
                        chk.Checked = false;
                    break;
                //Detalle (Pago) de GV Pagos Disponibles
                case "chkPagosVariosDisp":
                    //Validando que existan registros en el GV
                    if (gvPagosDisponibles.DataKeys.Count > 0)
                    {
                        //Obteniendo Control de ENcabezado
                        CheckBox chkHeader = (CheckBox)gvPagosDisponibles.HeaderRow.FindControl("chkPagosTodosDisp");

                        //Validando que el control se haya desmarcado
                        if (!chk.Checked)
                            //Desmarcando Encabezado
                            chkHeader.Checked = false;

                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvPagosDisponibles, "chkPagosVariosDisp");

                        //Validando que existan filas seleccionadas
                        if (gvr.Length > 0)
                            //Visualizando Control de Guardado
                            btnAgregarPagos.Visible = true;
                        else
                            //Ocultando Control de Guardado
                            btnAgregarPagos.Visible = false;

                        break;
                    }
                    else
                        chk.Checked = false;
                    break;
                //Cabecera de GV Pagos en CFDI
                case "chkPagosTodosCFDI":
                    //Validando que existan registros en el GV
                    if (gvPagosEnCFDI.DataKeys.Count > 0)
                    {
                        //Seleccionando Todas las Filas
                        Controles.SeleccionaFilasTodas(gvPagosEnCFDI, "chkPagosVariosCFDI", chk.Checked);

                        //Aplicando criterio de habilitación
                        btnQuitarPagos.Visible = chk.Checked;
                    }
                    else
                        chk.Checked = false;
                    break;
                //Detalle (Pago) de GV Pagos en CFDI
                case "chkPagosVariosCFDI":
                    //Validando que existan registros en el GV
                    if (gvPagosEnCFDI.DataKeys.Count > 0)
                    {
                        //Obteniendo Control de ENcabezado
                        CheckBox chkHeader = (CheckBox)gvPagosEnCFDI.HeaderRow.FindControl("chkPagosTodosCFDI");

                        //Validando que el control se haya desmarcado
                        if (!chk.Checked)
                            //Desmarcando Encabezado
                            chkHeader.Checked = false;

                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvr = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvPagosEnCFDI, "chkPagosVariosCFDI");

                        //Validando que existan filas seleccionadas
                        if (gvr.Length > 0)
                            //Visualizando Control de Guardado
                            btnQuitarPagos.Visible = true;
                        else
                            //Ocultando Control de Guardado
                            btnQuitarPagos.Visible = false;

                        break;
                    }
                    else
                        chk.Checked = false;
                    break;
            }
        }
        /// <summary>
        /// Click en botón registrar CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegistrarFacturaElectronica_Click(object sender, EventArgs e)
        {
            //Realizando la Inserción del nuevo CFDI
            crearCFDIRecepcionPagos();
        }
        /// <summary>
        /// Click en botón cerrar ventan modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando que botón realiza el cierre
            LinkButton btn = (LinkButton)sender;
            //Cerrando ventana modal
            alternaVentanaModal(btn.CommandName, btn);
        }
        /// <summary>
        /// Click en botón Aceptar Timbrado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrarFacturacionElectronica_Click(object sender, EventArgs e)
        {
            //Realizando timbrado
            timbrarComprobantePago();
        }
        /// <summary>
        /// Click en botón eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarElimnarCFDI_Click(object sender, EventArgs e)
        {
            //Realizando eliminación
            eliminarCFDI();
        }
        /// <summary>
        /// Click en botón aceptar Sustitución de CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarSustituirCFDI_Click(object sender, EventArgs e)
        {
            //Realizando sustitución
            sustituirComprobantePago();
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Carga el contenido de la pestaña "Información CFDI" acorde al estatus de visualización
        /// </summary>
        private void configurarPestanaInformacionCFDI()
        {
            //Habilitando elementos del menú
            habilitarMenuInformacionCFDI();

            //Determianndo el estatus de formulario
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //Asignando Titulo de Pestaña
                    h2InformacionCFDI.InnerText = "Nuevo CFDI";
                    //Limpiando contenido de controles de información de CFDI
                    lblSerieFolio.Text = lblUUID.Text = lblSustituyeA.Text = lblEstatus.Text = lblFechaExpedicion.Text = lblFechaCancelacion.Text = "";

                    //Cargando lista de FI (Pagos) disponibles
                    cargarPagosDisponibles();

                    //Cargando Lista de FI del CFDI(vacía)
                    cargarPagosEnCFDI();
                    break;
                case Pagina.Estatus.Edicion:
                    //Asignando Titulo de Pestaña
                    h2InformacionCFDI.InnerText = "Edición de CFDI";
                    //Limpiando contenido de controles de información de CFDI
                    lblSerieFolio.Text = "--";
                    lblUUID.Text = "--";

                    //Recuperando Referencia de CFDI Relacionado
                    using (fe33.Comprobante cfdiSustituido = fe33.ComprobanteRelacion.ObtenerCFDISustituido(Convert.ToInt32(Session["id_registro"])))
                        lblSustituyeA.Text = string.Format("{2} {0}{1}", cfdiSustituido.serie, cfdiSustituido.folio, cfdiSustituido.id_comprobante33 > 0 ? "Sustituye A: " : "");

                    lblEstatus.Text = "Registrado";
                    lblFechaExpedicion.Text = "--";
                    lblFechaCancelacion.Text = "--";

                    //Cargando lista de FI (Pagos) disponibles
                    cargarPagosDisponibles();

                    //Cargando Lista de FI del CFDI
                    cargarPagosEnCFDI();

                    break;
                case Pagina.Estatus.Lectura:
                    //Asignando Titulo de Pestaña
                    h2InformacionCFDI.InnerText = "Consulta de CFDI";

                    //Instanciando comprobante activo en sesión y sustituido (en caso de aplicar)
                    using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(Session["id_registro"])),
                        cfdiSustituido = fe33.ComprobanteRelacion.ObtenerCFDISustituido(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Instanciando timbre de CFDI
                        using (fe33.TimbreFiscalDigital tCfdi = fe33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
                        {

                            //Asignando contenido de etiquetas
                            lblSerieFolio.Text = string.Format("{0}{1}", cfdi.serie, cfdi.folio);
                            lblUUID.Text = tCfdi.UUID;
                            lblSustituyeA.Text = string.Format("{2} {0}{1}", cfdiSustituido.serie, cfdiSustituido.folio, cfdiSustituido.id_comprobante33 > 0 ? "Sustituye A: " : "");

                            lblEstatus.Text = ((fe33.Comprobante.EstatusVigencia)cfdi.id_estatus_vigencia).ToString();
                            lblFechaExpedicion.Text = cfdi.fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                            lblFechaCancelacion.Text = Fecha.ConvierteDateTimeString(cfdi.fecha_cancelacion, "dd/MM/yyyy HH:mm");

                            //Cargando lista de FI (Pagos) disponibles (vacío)
                            Controles.InicializaGridview(gvPagosDisponibles);

                            //Cargando Lista de FI del CFDI 
                            cargarPagosEnCFDI();
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Realiza la búsqueda de Pagos para su integración en un CFDI
        /// </summary>
        private void cargarPagosDisponibles()
        {
            //Validando Selección de un cliente en la lista
            if (gvClientes.SelectedIndex != -1)
            {
                //Recuperando resumen desde BD
                using (DataTable mit = EgresoIngreso.ObtieneFIDisponiblesParaCFDIRecepcionPagos(((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvClientes.SelectedDataKey.Value)))
                {
                    //Cargando GV de Pagos Disponibles
                    Controles.CargaGridView(gvPagosDisponibles, mit, "Id-IdEgresoIngreso", "", true, 2);

                    //Si hay datos
                    if (mit != null)
                        //Preservando en sesión la información
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet(((DataSet)Session["DS"]), mit, "Table1");
                    else
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet(((DataSet)Session["DS"]), "Table1");

                    //Limpiando selecciones
                    Controles.InicializaIndices(gvPagosDisponibles);
                }
            }
            else
                //Inicializando gridview
                Controles.InicializaGridview(gvPagosDisponibles);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosDisponibles);

            btnAgregarPagos.Visible = false;
        }
        /// <summary>
        /// Realiza la búsqueda de los Pagos asignados al CFDI de Recepción de Pagos Actual (Registro Sesión)
        /// </summary>
        private void cargarPagosEnCFDI()
        {
            //Si hay un registro en sesión
            if (Session["id_registro"].ToString() != "0")
            {
                //Recuperando resumen desde BD
                using (DataTable mit = fe33.EgresoIngresoComprobante.ObtienePagosCFDIRecepcionPagos(Convert.ToInt32(Session["id_registro"])))
                {
                    //Cargando GV de Pagos Disponibles
                    Controles.CargaGridView(gvPagosEnCFDI, mit, "Id-IdEgresoIngresoComp", "", true, 2);

                    //Si hay datos
                    if (mit != null)
                        //Preservando en sesión la información
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet(((DataSet)Session["DS"]), mit, "Table2");
                    else
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet(((DataSet)Session["DS"]), "Table2");

                    //Limpiando selecciones
                    Controles.InicializaIndices(gvPagosDisponibles);
                }
            }
            else
                //Inicializando gridview
                Controles.InicializaGridview(gvPagosEnCFDI);

            //Actualizando totales al pie
            cargarTotalesPieGV(gvPagosEnCFDI);
            
            //Ocultando acciones sobre pagos
            btnQuitarPagos.Visible = false;
        }
        /// <summary>
        /// Actualiza el total al pie del GV solicitado
        /// </summary>
        /// <param name="gv"></param>
        private void cargarTotalesPieGV(GridView gv)
        {
            //Determinando que control se requiere actualizar
            switch (gv.ID)
            {
                case "gvPagosDisponibles":
                    if (gvPagosDisponibles.DataKeys.Count > 0)
                    {
                        //Aplicando sumatoria al pie
                        using (DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"))
                        {
                            gvPagosDisponibles.FooterRow.Cells[5].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Monto)", "") : 0);
                            gvPagosDisponibles.FooterRow.Cells[6].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Aplicado)", "") : 0);
                            gvPagosDisponibles.FooterRow.Cells[7].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Saldo)", "") : 0);
                        }
                    }
                    break;
                case "gvPagosEnCFDI":
                    if (gvPagosEnCFDI.DataKeys.Count > 0)
                    {
                        //Aplicando sumatoria al pie
                        using (DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"))
                        {
                            gvPagosEnCFDI.FooterRow.Cells[5].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Monto)", "") : 0);
                            gvPagosEnCFDI.FooterRow.Cells[6].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Aplicado)", "") : 0);
                            gvPagosEnCFDI.FooterRow.Cells[7].Text = string.Format("{0:c}", mit != null ? mit.Compute("SUM(Saldo)", "") : 0);
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// Des/Habilita las opciones del menú de la pestaña Información CFDI
        /// </summary>
        private void habilitarMenuInformacionCFDI()
        {
            //Determinando el estatus actual solicitado
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    lkbNuevo.Enabled = false;
                    lkbTimbrarFacturaElectronica.Enabled = false;
                    lkbEliminarCFDI.Enabled = false;

                    lkbPDF.Enabled = false;
                    lkbXML.Enabled = false;

                    lkbBitacora.Enabled = false;
                    lkbReferencias.Enabled = false;

                    break;
                case Pagina.Estatus.Edicion:
                    lkbNuevo.Enabled = true;
                    lkbTimbrarFacturaElectronica.Enabled = true;
                    lkbEliminarCFDI.Enabled = true;

                    lkbPDF.Enabled = true;
                    lkbXML.Enabled = false;

                    lkbBitacora.Enabled = true;
                    lkbReferencias.Enabled = true;

                    break;
                case Pagina.Estatus.Lectura:
                    lkbNuevo.Enabled = true;
                    lkbTimbrarFacturaElectronica.Enabled = false;
                    lkbEliminarCFDI.Enabled = false;

                    lkbPDF.Enabled = true;
                    lkbXML.Enabled = true;

                    lkbBitacora.Enabled = true;
                    lkbReferencias.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Realiza la transacción de inserción de CFDI
        /// </summary>
        private void crearCFDIRecepcionPagos()
        {
            //Recuperando Id de Uso de CFDI "Por Definir"
            int id_uso_cfdi = Convert.ToInt32(SAT_CL.Global.Catalogo.RegresaValorCadenaValor(3194, "P01"));
            //Creando lista de FI (Pagos) seleccionados
            GridViewRow[] filas = Controles.ObtenerFilasSeleccionadas(gvPagosDisponibles, "chkPagosVariosDisp");
            List<KeyValuePair<int, int>> listaFI = (from GridViewRow gvr in filas
                                 select new KeyValuePair<int, int>(Convert.ToInt32(gvPagosDisponibles.DataKeys[gvr.RowIndex].Value), Convert.ToInt32(gvPagosDisponibles.DataKeys[gvr.RowIndex]["IdEgresoIngreso"]))).DefaultIfEmpty(new KeyValuePair<int, int>()).ToList();
            //Insertando CFDI
            RetornoOperacion resultado = EgresoIngreso.ImportarFIComprobanteV3_3(listaFI, Convert.ToInt32(ddlSucursal.SelectedValue), id_uso_cfdi, ((seguridad.UsuarioSesion)Session["usuario_Sesion"]).id_usuario);

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Cerrando ventana modal
                alternaVentanaModal("RegistrarCFDI", btnRegistrarFacturaElectronica);
                recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Edicion);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnRegistrarFacturaElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza operaciones de recarga de contenido de Información de CFDI posteriores a una actualización de su contenido
        /// </summary>
        /// <param name="resultado"></param>
        private void recargarCFDIEnEdicionConsulta(RetornoOperacion resultado, Pagina.Estatus estatus)
        {
            //Asignando nuevo estatus
            Session["estatus"] = estatus;
            //Asignando Registro de sesión
            Session["id_registro"] = estatus == Pagina.Estatus.Nuevo ? 0 : resultado.IdRegistro;
            //Preservando Id de cliente
            string id_cliente = gvClientes.SelectedDataKey.Value.ToString();
            //Actualizando resumen de cliente
            cargarResumenCliente();
            //Volviendo a seleccionar el cliente
            Controles.MarcaFila(gvClientes, id_cliente);

            //Asignando indice
            mtvSolicitud.ActiveViewIndex = 0;
            //Recargando Contenido de Vista Información CFDI
            configurarPestanaInformacionCFDI();
        }
        /// <summary>
        /// Añade el conjunto de pagos marcados al CFDI activo en sesión
        /// </summary>
        private void agregarPagosCFDI()
        {
            //Creando lista de elementos por agregar
            GridViewRow[] filas = Controles.ObtenerFilasSeleccionadas(gvPagosDisponibles, "chkPagosVariosDisp");
            List<KeyValuePair<int, int>> listaFI = (from GridViewRow gvr in filas
                                 select new KeyValuePair<int, int>(Convert.ToInt32(gvPagosDisponibles.DataKeys[gvr.RowIndex].Value), Convert.ToInt32(gvPagosDisponibles.DataKeys[gvr.RowIndex]["IdEgresoIngreso"]))).DefaultIfEmpty(new KeyValuePair<int, int>()).ToList();

            //Añadiendo elementos
            RetornoOperacion resultado = fe33.EgresoIngresoComprobante.AgregarIngresosACFDIPagos(listaFI, ((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario, Convert.ToInt32(Session["id_registro"]));

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Recragando contenido de formulario
                recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Edicion);

                //Actualizando panel de resumen
                upgvClientes.Update();                
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAgregarPagos, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Quita los pagos seleccionados edl CFDI activo en sesión
        /// </summary>
        private void quitarPagosCFDI()
        {
            //Creando lista de elementos por agregar
            GridViewRow[] filas = Controles.ObtenerFilasSeleccionadas(gvPagosEnCFDI, "chkPagosVariosCFDI");
            List<int> listaFI = (from GridViewRow gvr in filas
                                 select Convert.ToInt32(gvPagosEnCFDI.DataKeys[gvr.RowIndex]["IdEgresoIngresoComp"])).DefaultIfEmpty(0).ToList();

            //Añadiendo elementos
            RetornoOperacion resultado = fe33.EgresoIngresoComprobante.EliminarIngresosDeCFDIPagos(listaFI, ((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario, Convert.ToInt32(Session["id_registro"]));

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Recragando contenido de formulario
                recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Edicion);

                //Actualizando panel de resumen
                upgvClientes.Update();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAgregarPagos, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza la deshabilitación del registro activo en sesión
        /// </summary>
        private void eliminarCFDI()
        {
            //Realizando proceso de eliminación de CFDI
            //Instanciando CFDI
            using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                //Realizando deshabilitación
                RetornoOperacion resultado = cfdi.DeshabilitaCFDIComplementoRecepcionPagosV10(((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Cerrando ventana modal
                    alternaVentanaModal("EliminarCFDI", btnAceptarElimnarCFDI);

                    //Actualizando resumen y pestaña de información de CFDI
                    recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Nuevo);
                }

                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnAceptarElimnarCFDI, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            }
        }
        /// <summary>
        /// Realiza el timbrado del CFDI activo en la pestaña Información de CFDI
        /// </summary>
        private void timbrarComprobantePago()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Factura
            using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(Session["id_registro"])))
            {
                //Registramos Factura
                resultado = cfdi.TimbraComprobanteRecepcionPagoV1_0(txtSerie.Text, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3.xslt"), Server.MapPath("~/XSLT/FacturacionElectronica3_3/cadenaoriginal_3_3_desconectado.xslt"));

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Se cierra ventana modal de timbrado
                    alternaVentanaModal("TimbrarCFDI", btnAceptarTimbrarFacturacionElectronica);
                    //Actualizando estatus a consulta
                    recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Lectura);
                }

                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnAceptarTimbrarFacturacionElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Realiza la sustitución de un CFDI timbrado por uno en blanco
        /// </summary>
        private void sustituirComprobantePago()
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Factura
            using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(gvCFDIPagos.SelectedDataKey.Value)))
            {
                //Registramos Factura
                resultado = cfdi.SustituyeCFDIComplementoPagosV10(txtMotivoCancelacionCFDI.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Se cierra ventana modal de timbrado
                    alternaVentanaModal("SustituirCFDI", btnAceptarSustituirCFDI);
                    //Actualizando estatus a edición
                    recargarCFDIEnEdicionConsulta(resultado, Pagina.Estatus.Edicion);
                    //Actualizando contenido de UpdatePanel
                    upmtvCFDIPagos.Update();
                    upbtnInformacionCFDI.Update();
                    upbtnCFDIPendientes.Update();
                }

                //Mostrando resultado
                ScriptServer.MuestraNotificacion(btnAceptarTimbrarFacturacionElectronica, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteRecepcionPagosV10.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
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
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteRecepcionPagosV10.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        #endregion

        #endregion

        #region Pestaña CFDI Existentes

        #region Eventos

        /// <summary>
        /// Cambio de indice de página GV CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCFDIPagos_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando criterio de visuzalización
            Controles.CambiaIndicePaginaGridView(gvCFDIPagos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Enlace a datos de cada fila GV CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCFDIPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si hay datos que validar
            if (gvCFDIPagos.DataKeys.Count > 0)
            {
                //Determianndo el tipo de fila de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando control check
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkVariosCFDI");
                    
                    //Recuperando fila a enlazar
                    DataRow dr = ((DataRowView)e.Row.DataItem).Row;

                    //Si el CFDI no se encuentra Timbrado
                    if (!dr.Field<bool>("*BitGenerado"))
                        //Aplicando criterio de selección
                        chk.Enabled = chk.Checked = false;

                    //Determinando color que se aplicará al fondo de la fila en base a su estatus
                    switch (dr.Field<string>("Estatus"))
                    {
                        case "Registrado":
                        case "Vigente":
                            //Sin color personalizado
                            break;
                        case "Por Sustituir":
                            //Naranja
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8E08E");
                            break;
                        case "Sustituido":
                            //Rojo
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D75454");
                            e.Row.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Cambio de orden en GV CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCFDIPagos_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //Aplicando criterio de visuzalización
            lblOrdenadoGVCFDIPagos.Text = Controles.CambiaSortExpressionGridView(gvCFDIPagos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Cambio de tamaño de página de GV CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGVCFDIPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Aplicando criterio de visuzalización
            Controles.CambiaTamañoPaginaGridView(gvCFDIPagos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoGVCFDIPagos.SelectedValue), true, 3);
        }
        /// <summary>
        /// Click en botón de GV de CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDetalles_Click(object sender, EventArgs e)
        {
            //Validando que existan registros
            if (gvCFDIPagos.DataKeys.Count > 0)
            {
                //Seleccionando registro
                Controles.SeleccionaFila(gvCFDIPagos, sender, "lnk", false);

                //Recuperando referencia de control pulsado
                LinkButton lnk = (LinkButton)sender;

                //Determinando el comendo del botón
                switch (lnk.CommandName)
                {
                    case "Consultar":
                    case "Editar":
                        Session["estatus"] = lnk.CommandName == "Consultar" ? Pagina.Estatus.Lectura : Pagina.Estatus.Edicion; 
                        //Estableciendo estatus y registros de sesión
                        Session["id_registro"] = gvCFDIPagos.SelectedDataKey.Value;

                        //Inicializando contenido de pestaña
                        inicializarPestanaActiva(0);
                        //Actualizando contenido de UpdatePanel
                        upmtvCFDIPagos.Update();
                        upbtnInformacionCFDI.Update();
                        upbtnCFDIPendientes.Update();
                        break;
                    case "Sustituir":
                        //Limpiando motivo
                        txtMotivoCancelacionCFDI.Text = "";
                        //Mostrar ventana modal de confirmación
                        alternaVentanaModal("SustituirCFDI", lnk);
                        //Estableciendo foco incial
                        txtMotivoCancelacionCFDI.Focus();
                        break;
                    case "XML":
                    case "PDF":
                        //Instanciando registro comprobante
                        using (fe33.Comprobante cfdi = new fe33.Comprobante(Convert.ToInt32(gvCFDIPagos.SelectedDataKey.Value)))
                        {
                            //Si el cfd existe
                            if (cfdi.habilitar)
                            {
                                //Si es PDF
                                if(lnk.CommandName == "PDF")
                                {
                                    //Obteniendo Ruta
                                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ComprobanteRecepcionPagosV10.ascx", "~/RDLC/Reporte.aspx");

                                    //TODO: Instanciando nueva ventana de navegador para apertura de registro
                                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobantePago", cfdi.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                }
                                //Si es XML
                                else
                                {
                                    //Si existe y está generado
                                    if (cfdi.bit_generado)
                                    {
                                        //Obteniendo bytes del archivo XML
                                        byte[] cfdi_xml = System.IO.File.ReadAllBytes(cfdi.ruta_xml);

                                        //Realizando descarga de archivo
                                        if (cfdi_xml.Length > 0)
                                        {
                                            //Instanciando al emisor
                                            using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                                                TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, cfdi.serie, cfdi.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                                        }
                                    }
                                }
                            }
                            else
                                //Mostrando error
                                ScriptServer.MuestraNotificacion(lnk, "El CFDI no se ha recuperado, es posible que ya no exista en BD.", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                            break;
                }
            }
        }
        /// <summary>
        /// Click en check de GV  CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosCFDI_CheckedChanged(object sender, EventArgs e)
        {
            //Determinando que control está siendo usado 
            CheckBox chk = (CheckBox)sender;

            //Clasificando entre las operaciones posibles
            switch (chk.ID)
            {
                //Cabecera de GV CFDI
                case "chkTodosCFDI":
                    //Validando que existan registros en el GV
                    if (gvCFDIPagos.DataKeys.Count > 0)
                        //Seleccionando Todas las Filas
                        Controles.SeleccionaFilasTodas(gvCFDIPagos, "chkVariosCFDI", chk.Checked, false, false);
                    else
                        chk.Checked = false;
                    break;
                //Detalle (Pago) de GV Pagos Disponibles
                case "chkVariosCFDI":
                    //Validando que existan registros en el GV
                    if (gvCFDIPagos.DataKeys.Count > 0)
                    {
                        //Obteniendo Control de ENcabezado
                        CheckBox chkHeader = (CheckBox)gvCFDIPagos.HeaderRow.FindControl("chkTodosCFDI");

                        //Validando que el control se haya desmarcado
                        if (!chk.Checked)
                            //Desmarcando Encabezado
                            chkHeader.Checked = false;
                        
                        break;
                    }
                    else
                        chk.Checked = false;
                    break;                
            }
        }
        /// <summary>
        /// Click en botón Exportar CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportar_OnClick(object sender, EventArgs e)
        {
            exportarCFDI();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga el contenido de la pestaña "Búsqueda de CFDI" acorde al estatus de visualización
        /// </summary>
        private void configurarPestanaBusquedaCFDI()
        {
            //Validando que exista un cliente seleccionado
            if (gvClientes.SelectedIndex != -1)
            {
                //Realizando búsqueda de CFDI acorde a filtros definidos
                using (DataTable mitCFDI = fe33.Reporte.ObtenerCFDIRecepcionPagos(((seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvClientes.SelectedDataKey.Value), Fecha.ConvierteStringDateTime(txtFechaInicio.Text), Fecha.ConvierteStringDateTime(txtFechaFin.Text), txtFolio.Text, chkCFDIRegistrados.Checked, chkCFDITimbrados.Checked, chkCFDISustituidos.Checked, chkCFDIPorSustituir.Checked))
                {
                    //Si hay elementos
                    if (Validacion.ValidaOrigenDatos(mitCFDI))
                    {
                        //Creando Tamaño Dinamico
                        gvCFDIPagos.PageSize = mitCFDI == null ? 0 : mitCFDI.Rows.Count;
                        //Actualizando Origen de datos
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitCFDI, "Table3");
                    }
                    else
                        //Eliminando de Origen de datos
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");

                    //Cargando grid
                    Controles.CargaGridView(gvCFDIPagos, mitCFDI, "Id-*BitGenerado", "", true, 3);

                    //Inicializando indices de selección
                    Controles.InicializaIndices(gvCFDIPagos);
                }
            }
            else
            {
                Controles.InicializaGridview(gvCFDIPagos);
            }
        }

        /// <summary>
        /// Exportar PDF y XML
        /// </summary>
        private void exportarCFDI()
        {
            //Creamos lista de archivos
            List<KeyValuePair<string, byte[]>> archivos = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene Registros
            if (gvCFDIPagos.DataKeys.Count > 0)
            {   //Obteniendo Filas Seleccionadas
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvCFDIPagos, "chkVariosCFDI");
                //Verificando que existan filas Seleccionadas
                if (selected_rows.Length != 0)
                {
                    //Almacenando Rutas el Arreglo
                    foreach (GridViewRow row in selected_rows)
                    {   //Instanciando Comprobante de el Valor obtenido de la Fila Seleccionada
                        using (fe33.Comprobante comp = new fe33.Comprobante (Convert.ToInt32(gvCFDIPagos.DataKeys[row.RowIndex].Value)))
                        {
                            //Validamos Seleccion de Radio Buton de PDF
                            if (chkPDF.Checked == true)
                            {
                                //Añadimos PDF
                                archivos.Add(new KeyValuePair<string, byte[]>(comp.serie + comp.folio.ToString() + ".pdf", comp.GeneraPDFComprobantePagoV33()));
                            }
                            //Validando Selección de XML
                            if (chkXML.Checked == true)
                            {
                                //Verificando que exista el Archivo
                                if (File.Exists(comp.ruta_xml))
                                {
                                    //Guardando Archivo en arreglo de Bytes
                                    byte[] xml_file = System.IO.File.ReadAllBytes(comp.ruta_xml);
                                    //Añadimos XML
                                    archivos.Add(new KeyValuePair<string, byte[]>(comp.serie + comp.folio.ToString() + ".xml", xml_file));
                                }
                            }
                        }
                    }
                    //Genera el zip con las rutas
                    byte[] file_zip = Archivo.ConvirteArchivoZIP(archivos, out errores);
                    //Si almenos un archivo fue correcto descarga 
                    if (file_zip != null)
                    {
                        //Descarga el zip generado
                        Archivo.DescargaArchivo(file_zip, string.Format("CFDI_Pagos_{0:ddMMyyyy_HHmmss}.zip", Fecha.ObtieneFechaEstandarMexicoCentro()),
                                                   Archivo.ContentType.binary_octetStream);
                    }
                    else
                    {
                        string error_general = "";
                        //Recorremos errores
                        foreach (string error in errores)
                        {
                            //Muestra mensaje de Error
                            error_general += error + " <br>";
                        }
                        //Mostrando error
                        ScriptServer.MuestraNotificacion(btnExportarCFDI, error_general, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
                else//Mostrando Mensaje
                    //Mostrando error
                    ScriptServer.MuestraNotificacion(btnExportarCFDI, "No ha seleccionado ningún CFDI.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }


        #endregion

        #endregion

    }
}