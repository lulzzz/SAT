using SAT_CL;
using SAT_CL.ControlEvidencia;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.ControlEvidencia
{
    public partial class ArmadoEnvioPaquetes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando que se haya producido un PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando la Pagina
                inicializaPagina();


            }
        }

        #region Eventos Tab Actualización (Nuevo/Edición)

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Compañia"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCompania_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualizando catálogos de Origen/Destino
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigen, 62, "- Seleccione el Origen -", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestino, 62, "- Seleccione el Destino -", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");

            //En base al estatus de la forma
            //Estatus de nuevo registro
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                buscaPaqueteActivo();
            else
                //Actualizando lista de Documentos pendientes
                cargaDocumentosPenientesEnvio();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Origen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOrigen_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //En base al estatus de la forma
            //Estatus de nuevo registro
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                buscaPaqueteActivo();
            else
                //Actualizando lista de Documentos pendientes
                cargaDocumentosPenientesEnvio();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Destino"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDestino_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //En base al estatus de la forma
            //Estatus de nuevo registro
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                buscaPaqueteActivo();
            else
                //Actualizando lista de Documentos pendientes
                cargaDocumentosPenientesEnvio();
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "AceptarEnviar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEnviar_Click(object sender, EventArgs e)
        {
            if (mtvPaquetes.ActiveViewIndex == 0)
                enviaPaquete();

            //Generamos script para Ocultar de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionEnviar').animate({ width: 'toggle' });
            $('#confirmacionEnviar').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarEnviar, upbtnAceptarEnviar.GetType(), "CierreConfirmacion", script, false);


        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "AceptarEliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminar_Click(object sender, EventArgs e)
        {
            //Validando que el modo actual corresponda al comando seleccionado
            if (mtvPaquetes.ActiveViewIndex == 0)
                deshabilitaPaquete();

            //Generamos script para Ocultar de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionEliminar').animate({ width: 'toggle' });
            $('#confirmacionEliminar').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarEliminar, upbtnAceptarEliminar.GetType(), "CierreConfirmacion", script, false);


        }

        #endregion

        #region Eventos GridView "Documentos"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "DocumentosA"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDocumentosA_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvDocumentosA, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoDocumentosA.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDocumentosA_OnClick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { 
            Controles.CambiaIndicePaginaGridView(gvDocumentosA, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosA_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando el Ordenamiento de Pagina
            lblOrdenarDocumentosA.Text = Controles.CambiaSortExpressionGridView(gvDocumentosA, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox del GridView "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosDocumentos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvDocumentosA.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Evalua el ID del CheckBox en el que se produce el cambio
                if (d.ID == "chkTodosDocumentos")
                {
                    //Obtenemos su referencia 
                    using (Label l = (Label)gvDocumentosA.FooterRow.FindControl("lblSeleccionadosA"))
                    {
                        //Actualizamos el texto de la etiqueta
                        l.Text = Controles.SeleccionaFilasTodas(gvDocumentosA, "chkVariosDocumentos", d.Checked).ToString();
                    }
                }
                else
                {
                    //Sumando/restando elemento afectado
                    Controles.SumaSeleccionadosFooter(d.Checked, gvDocumentosA, "lblSeleccionadosA");

                    //Si retiró selección
                    if (!d.Checked)
                    {
                        //Referenciando control de encabezado
                        CheckBox t = (CheckBox)gvDocumentosA.HeaderRow.FindControl("chkTodosDocumentos");
                        //Aplicando marcado de elemento
                        t.Checked = d.Checked;
                    }
                }
            }
        }
        /// <summary>
        /// Evento producido al pulsar algún botón del GV de documento sin envíar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferenciaDoc_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros en gv
            if (gvDocumentosA.DataKeys.Count > 0)
            {
                //Seleccionando dila actual
                Controles.SeleccionaFila(gvDocumentosA, sender, "lnk", false);
                //Mostrando referencias 
                inicializaReferencias(Convert.ToInt32(gvDocumentosA.SelectedDataKey.Value), 41, "Documentos");
            }
        }

        #endregion

        #region Eventos GridView "DocumentosB"

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "DocumentosB"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDocumentosB_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvDocumentosB, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDocumentosB.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDocumentoB_OnClick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de página del Gridview "DocumentosB"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosB_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el Indice de Pagina
            Controles.CambiaIndicePaginaGridView(gvDocumentosB, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "DocumentosB"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosB_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando el Ordenamiento de Pagina
            lblOrdenarDocumentosB.Text = Controles.CambiaSortExpressionGridView(gvDocumentosB, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox del GridView "Paquetes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosPaquetes_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvDocumentosB.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Evalua el ID del CheckBox en el que se produce el cambio
                if (d.ID == "chkTodosPaquetes")
                {
                    //Obtenemos su referencia 
                    using (Label l = (Label)gvDocumentosB.FooterRow.FindControl("lblSeleccionadosB"))
                    {
                        //Actualizamos el texto de la etiqueta
                        l.Text = Controles.SeleccionaFilasTodas(gvDocumentosB, "chkVariosPaquetes", d.Checked).ToString();
                    }
                }
                else
                {
                    //Sumando/restando elemento afectado
                    Controles.SumaSeleccionadosFooter(d.Checked, gvDocumentosB, "lblSeleccionadosB");

                    //Si retiró selección
                    if (!d.Checked)
                    {
                        //Referenciando control de encabezado
                        CheckBox t = (CheckBox)gvDocumentosB.HeaderRow.FindControl("chkTodosPaquetes");
                        //Aplicando marcado de elemento
                        t.Checked = d.Checked;
                    }
                }
            }
        }

        #endregion

        #region Eventos Búsqueda

 
        /// <summary>
        /// Evento producido al pulsar el botón buscar paquetes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {   
            cargaPaquetes(true);
        }

        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFechasBusqueda_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if ((chkFechasBusqueda.Checked))
            {
                //Generamos script para validación de Fechas
                string  script =
                @"<script type='text/javascript'>
                
                    var validacionBusqueda = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#ctl00_content1_txtFechaInicioB').validationEngine('validate');
                        var isValid2 = !$('#ctl00_content1_txtFechaFinB').validationEngine('validate');
                        return isValid1 && isValid2
                    }; 
                //Botón Buscar
                $('#ctl00_content1_btnBuscar').click(validacionBusqueda);
                                    </script>";


                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(upchkFechasBusqueda, upchkFechasBusqueda.GetType(), "Validacion", script, false);

            }
            else
            {
                //Limpiamos Etiquetas de fechas
                txtFechaInicioB.Text = "";
                txtFechaFinB.Text = "";
            }
            //Habilitación de radios
            rdbFechaLlegada.Enabled = rdbFechaSalida.Enabled =
             //Habilitación de cajas de texto para fecha
            txtFechaInicioB.Enabled = txtFechaFinB.Enabled = chkFechasBusqueda.Checked;
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
                case "ArmadoEdicion":
                    mtvPaquetes.ActiveViewIndex = 0;
                    btnArmadoEdicion.CssClass = "boton_pestana_activo";
                    btnBusqueda.CssClass = "boton_pestana";
                    break;
                case "Busqueda":
                    mtvPaquetes.ActiveViewIndex = 1;
                    btnArmadoEdicion.CssClass = "boton_pestana";
                    btnBusqueda.CssClass = "boton_pestana_activo";
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPaquete_Click(object sender, EventArgs e)
        {
            //Validando que existan registros en el GV
            if (gvBusqueda.DataKeys.Count > 0)
            {
                //Seleccionado el registro actual
                Controles.SeleccionaFila(gvBusqueda, sender, "lnk", false);
                //Determinando el tipo de acción a realizar
                switch (((LinkButton)sender).CommandName)
                {
                    case "Abrir":
                        //Asignando datos de sesión
                        Session["id_registro"] = gvBusqueda.SelectedDataKey.Value;
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        //Cargando contenido de página
                        inicializaPagina();
                        //Indicando indice activo en TabControl
                      mtvPaquetes.ActiveViewIndex = 0;
                      //Cabiamos Estilos activación de los Botones
                      btnArmadoEdicion.CssClass = "boton_pestana_activo";
                      btnBusqueda.CssClass = "boton_pestana";
                        break;
                    case "Referencias":
                        break;
                    case "Bitacora":
                        break;
                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página activo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBusqueda_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvBusqueda, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBusqueda_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewBusqueda.Text = Controles.CambiaSortExpressionGridView(gvBusqueda, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de exportación a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarBusqueda_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "");
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvBusqueda, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamañoGridViewBusqueda.SelectedValue), true, 1);
        }

        #endregion

        /// <summary>
        /// Evento Producido al Dar Click en algun Boton del Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Obteniendo objeto del Evento
            LinkButton link = (LinkButton)sender;
            //Evaluando Control
            switch (link.CommandName)
            {
                case "Buscar":
                    //Cambia indice de tab
                    mtvPaquetes.ActiveViewIndex = 1;
                    break;
                case "Nuevo":
                    //Cambia indice de tab
                    mtvPaquetes.ActiveViewIndex = 0;
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    Session["id_registro"] = 0;
                    inicializaPagina();
                    break;
                case "Guardar":
                    //Validando que el modo actual corresponda al comando seleccionado
                    if (mtvPaquetes.ActiveViewIndex == 0)
                        guardaPaquete();
                    break;
                case "Imprimir":
                    {
                        //Declaramos objeto Resultado
                        RetornoOperacion resultado = new RetornoOperacion();

                        //Validando que el paquete del reporte a imprimir contenga evidencias
                        DataTable dtContenidoPaquete = SAT_CL.ControlEvidencia.Reportes.ObtieneServiciosPaqueteEnvio(Convert.ToInt32(Session["id_registro"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        
                        //Validamos que hayan retornado valores válidos
                        if (Validacion.ValidaOrigenDatos(dtContenidoPaquete))
                        {
                            //Obteniendo Ruta
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/ArmadoEnvioPaquetes.aspx", "~/RDLC/Reporte.aspx");

                            //Instanciando nueva ventana de navegador para apertura de reporte
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "EnvioPaquete", Convert.ToInt32(Session["id_registro"])), "Envío de paquete", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                        else
                        {
                            resultado = new RetornoOperacion("El paquete no contiene evidencias, no es posible imprimir un paquete vacío.");
                            //Mostrando Mensaje
                            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                    break;
                case "Salir":
                   PilaNavegacionPaginas.DireccionaPaginaAnterior();
                    break;
                case "Editar":
                    //Validando que el modo actual corresponda al comando seleccionado
                    if (mtvPaquetes.ActiveViewIndex == 0)
                    {
                        //Validando edición
                        RetornoOperacion resultado = validaEdicionPaquete();
                        //Si no hay ningún problema
                        if (resultado.OperacionExitosa)
                        {
                            Session["estatus"] = Pagina.Estatus.Edicion;
                            inicializaPagina();
                        }
                        else
                            lblError.Text = resultado.Mensaje;
                    }
                    break;
                case "Bitacora":
                    //Validando que el modo actual corresponda al comando seleccionado
                    if (mtvPaquetes.ActiveViewIndex == 0)
                        inicializaBitacora(Convert.ToInt32(Session["id_registro"]), 42, "Paquete de Documentos");
                    break;
                case "Referencias":
                    //Validando que el modo actual corresponda al comando seleccionado
                    if (mtvPaquetes.ActiveViewIndex == 0)
                        inicializaReferencias(Convert.ToInt32(Session["id_registro"]), 42, "Paquete de Evidencias");
                    break;
            }
        }
        /// <summary>
        /// Evento disparado al Dar Click en el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            //Determinando el comando por realizar
            switch (((Button)sender).CommandName)
            {
                case "Guardar":
                    guardaPaquete();
                    break;
                case "Cancelar":
                    //Si el estatus es edición
                    if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion)
                        Session["estatus"] = Pagina.Estatus.Lectura;

                    //Cargando contenido
                    inicializaPagina();
                    break;
            }
        }
        /// <summary>
        /// Evento producido al pulsar algún botón de actualización de contenido de paquete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizacion_OnClick(object sender, EventArgs e)
        {
            //Validando existencia de registro activo en sesión y que su estatus permita la edición de detalles
            using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el estatus es Registrado
                if (p.id_estatus == PaqueteEnvio.EstatusPaqueteEnvio.Registrado && p.id_paquete_envio > 0)
                {
                    //Determinando que acción será realizada
                    switch (((Button)sender).CommandName)
                    {
                        case "Agregar":
                            //Añadiendo la selección al paqute
                            anadeSeleccionPaquete();
                            break;
                        case "AgregarTodos":
                            anadeTodosPaquete();
                            break;
                        case "Quitar":
                            quitaSeleccionPaquete();
                            break;
                        case "QuitarTodos":
                            quitaTodosPaquete();
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Evento disparado al Dar Click en el Boton "Atras"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAtras_Click(object sender, ImageClickEventArgs e)
        {   //Direccionando a Pagina Anterior
           PilaNavegacionPaginas.DireccionaPaginaAnterior();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de inicializan el contenido de la forma
        /// </summary>
        private void inicializaPagina()
        {   //Cargando Catalogos
            cargaCatalogos();
            //Habilitando los Controles de la Forma
            habilitaControles();
            //Inicializando menú
            inicializaMenu();
            //Inicializando valores de registro
            inicializaValores();
            //Cargando Detalles de Paquete
            cargaDocumentosPaquete();
            //Cargando Documentos pendientes
            cargaDocumentosPenientesEnvio();
            //Inicializando búsqueda de paquetes
            cargaPaquetes(false); 
        }
        /// <summary>
        /// Realiza la búsqueda de un páquete registrado (activo) para la compañía, origen y destino especificados
        /// </summary>
        private void buscaPaqueteActivo()
        {
            //Instanciando paquete registrado con coincidencias de compañía, origen y destino
            using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(ddlOrigen.SelectedValue), Convert.ToInt32(ddlDestino.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1))))
            {
                //Validnado que el registro exista
                if (p.id_paquete_envio > 0)
                {
                    //Asignando estatus de edición de registro encontrado
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    Session["id_registro"] = p.id_paquete_envio;
                    //Inicializando página
                    inicializaPagina();
                }
                else
                    //cargando solo lista de pendientes
                    cargaDocumentosPenientesEnvio();
            }
        }
        /// <summary>
        /// Carga los valores de registro y los asigna sobre los controles correspondientes
        /// </summary>
        private void inicializaValores()
        {
            //Determinando tipo de carga a realizar en base al estatus de sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    lblId.Text = "ID";
                    chkFechasBusqueda.Checked = false;

                    //Actualizando catálogos
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigen, 9, "- Seleccione el Origen -", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestino, 9, "- Seleccione el Destino -", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");

                    //Asignando Lugar de origen
                    ddlOrigen.SelectedValue = "0";
                    

                    ddlDestino.SelectedValue = "0";
                    ddlEstatus.SelectedValue = "1";

                    ddlMedioEnvio.SelectedValue = "0";
                    txtReferencia.Text = "";

                    lblFecSal.Text = lblFecLle.Text = "";

                    //Limpiando errores
                    lblError.Text = lblError2.Text = "";
                    break;

                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    //Instanciando registro actual en sesión
                    using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(Session["id_registro"])))
                    {
                        //SI existe el registro
                        if (p.habilitar)
                        {
                            lblId.Text = p.id_paquete_envio.ToString();
                            //Instanciamos Compañia  para visualización en el control
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(p.id_compania_emisor))
                            {
                                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
                            }


                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigen, 9, "- Seleccione el Origen -", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");
                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestino, 9, "- Seleccione el Destino -", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");


                            ddlOrigen.SelectedValue = p.id_terminal_origen.ToString();
                            ddlDestino.SelectedValue = p.id_terminal_destino.ToString();
                            ddlEstatus.SelectedValue = ((byte)p.id_estatus).ToString();

                            ddlMedioEnvio.SelectedValue = p.id_medio_envio.ToString();
                            txtReferencia.Text = p.referencia_envio;

                            lblFecSal.Text = Fecha.ConvierteDateTimeString(p.fecha_salida, "dd/MM/yyyy HH:mm");
                            lblFecLle.Text = Fecha.ConvierteDateTimeString(p.fecha_llegada, "dd/MM/yyyy HH:mm");

                            //Limpiando errores
                            lblError.Text = lblError2.Text = "";
                        }
                        else
                        {
                            //Limpiando errores
                            lblError.Text = "El registro ha sido eliminado o no pudo ser encontrado.";
                            lblError2.Text = "";
                        }
                    }

                    break;
            }


        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Instanciamos Compañia  para visualización en el control
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
                txtCompaniaB.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
            }
            /*Catálogos de Armado y Edición*/
            //Cargando Catalogos del Encabezado 
            //Actualizando catálogos
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigen, 9, "- Seleccione el Origen -", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestino, 9, "- Seleccione el Destino -", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");


            //Cargardo Catalogos del Medio de Envio
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMedioEnvio, "- Seleccione Medio de Envío -", 37);
            //Cargando Catalogo de Estatus
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 38);

            //Catalogos de Tamaño de los GridView
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentosA, "", 26);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentosB, "", 26);

            /*Catálogos Búsqueda*/
            //Cargando Catalogos de Busqueda
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigenB, 9, "Todos", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestinoB, 9, "Todos", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");


            //Cargardo Catalogos del Medio de Envio
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMedioEnvioB, "Todos", 37);
            //Cargando Catalogo de Estatus
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusB, "Todos", 38);

            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewBusqueda, "", 26);
        }
        /// <summary>
        /// Inicializando menú de opciones
        /// </summary>
        private void inicializaMenu()
        {
            //Determinando el etstaus de la página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //Habilitamos Controles
                    lkbBuscar.Enabled =
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled = true;
                    lkbImprimir.Enabled = false;
                    lkbEditar.Enabled =
                    lkbEliminar.Enabled =
                    lkbEnviar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled = false;
                    break;
                case Pagina.Estatus.Lectura:
                    //Habilitamos Controles
                    lkbBuscar.Enabled =
                    lkbNuevo.Enabled = true;
                    lkbGuardar.Enabled = false;
                    lkbImprimir.Enabled = true;
                    lkbEditar.Enabled = true;
                    lkbEliminar.Enabled =
                    lkbEnviar.Enabled = false;
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled = true;
                    break;
                case Pagina.Estatus.Edicion:
                    //Habilitamos Controles
                    lkbBuscar.Enabled =
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled = true;
                    lkbImprimir.Enabled = false;
                    lkbEditar.Enabled = false;
                    lkbEliminar.Enabled =
                    lkbEnviar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Método encargado de habilitar los controles de la forma
        /// </summary>
        private void habilitaControles()
        {   //Evaluando el Estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:

                    //Encabezado de Paquete
                    ddlOrigen.Enabled =
                    ddlDestino.Enabled = true;
                    ddlEstatus.Enabled = false;
                    ddlMedioEnvio.Enabled =
                    txtReferencia.Enabled = true;

                    //Guardar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;

                    //Botones de agregar y quitar
                    btnAgregarDoc.Enabled =
                    btnAgregarTodos.Enabled =
                        //Documentos B
                    btnQuitarDoc.Enabled =
                    btnQuitarTodos.Enabled = false;

                    break;

                case Pagina.Estatus.Lectura:
                    //Origen / Destino
                    ddlOrigen.Enabled =
                    ddlDestino.Enabled =
                    ddlEstatus.Enabled =

                    //Guardar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled =

                    //Encabezado de Paquete
                    ddlMedioEnvio.Enabled =
                    txtReferencia.Enabled =

                    //Botones de agregar y quitar
                    btnAgregarDoc.Enabled =
                    btnAgregarTodos.Enabled =
                        //Documentos B
                    btnQuitarDoc.Enabled =
                    btnQuitarTodos.Enabled = false;
                    break;

                case Pagina.Estatus.Edicion:
                    //Origen / Destino
                    ddlOrigen.Enabled =
                    ddlDestino.Enabled =
                    ddlEstatus.Enabled = false;

                    //Guardar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled =

                    //Encabezado de Paquete
                    ddlMedioEnvio.Enabled =
                    txtReferencia.Enabled =

                    //Botones de agregar y quitar
                    btnAgregarDoc.Enabled =
                    btnAgregarTodos.Enabled =
                        //Documentos B
                    btnQuitarDoc.Enabled =
                    btnQuitarTodos.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Carga los documentos que se encuentran disponibles para envío
        /// </summary>
        private void cargaDocumentosPenientesEnvio()
        {
            //Realizando la carga de documentos pendientes
            using (DataTable mit = ControlEvidenciaDocumento.CargaDocumentosEnvioPendiente(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), Convert.ToInt32(ddlOrigen.SelectedValue), Convert.ToInt32(ddlDestino.SelectedValue)))
            { 
                //Si hay datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Inicializando selección de indices
                    Controles.InicializaIndices(gvDocumentosA);
                    //Llenando GridView
                    Controles.CargaGridView(gvDocumentosA, mit, "Id-Viaje-Documento", lblOrdenarDocumentosA.Text, true, 2);
                    //Guardando en sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Cargando GridView vacío
                    Controles.InicializaGridview(gvDocumentosA);
                    //Borrando sesión 
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método encargado de obtener los documentos ligados al paquete actual
        /// </summary>
        private void cargaDocumentosPaquete()
        {
            //Determinando el estatus de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //Inicializando el GridView
                    Controles.InicializaGridview(gvDocumentosB);
                    //Borrando de sesión el origen anterior (en caso de existir)
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    break;
                default:
                    //Obteniendo Reporte de los Documentos por Paquete
                    using (DataTable mit = PaqueteEnvioDocumento.CargaDocumentoDelPaquete(Convert.ToInt32(Session["id_registro"])))
                    { 
                        //Validando que la Tabla contenga Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                        {
                            //Inicializando selección de indices
                            Controles.InicializaIndices(gvDocumentosB);
                            //Cargando Reporte en el GridView
                            Controles.CargaGridView(gvDocumentosB, mit, "Id-Viaje-Documento", lblOrdenarDocumentosB.Text, true, 2);
                            //Añadiendo Tabla a DataSet de Session
                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
                        }
                        else
                        {   //Inicializando el GridView
                            Controles.InicializaGridview(gvDocumentosB);
                            //Borrando de sesión el origen anterior (en caso de existir)
                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Realizando la carga de paquetes
        /// </summary>
        /// <param name="sobreescribir">True para indicar que el origen de datos ya existente debe ser reemplazado</param>
        private void cargaPaquetes(bool sobreescribir)
        {
            //Determinando el estatus actual de la página
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo)
                //Inicializando indices de selección de GV
                Controles.InicializaIndices(gvBusqueda);

            //Si se desea sobreescribir una busqueda de paquetes
            if (sobreescribir)
            {
                /* TODO  Obtenemos Instalación predeterminada por el Usuario
                 * //referenciando al usuario activo
                 using (Usuario u = (Usuario)Session["usuario"])
                 {
                     //Obteniendo el instalación a la que pertenece el uisuario para el control de evidencia
                     using (clInstalacion i = new clInstalacion(Convert.ToInt32(u.Configuracion["Id Instalación Control Evidencias"])))
                     {
                         //Asignando Id de Compañía
                         ddlCompaniaB.SelectedValue = i.id_compania_emisor.ToString();

                         //Actualizando catálogos
                         BibliotecaClasesSICDB.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigenB, 62, "- Todos -", Convert.ToInt32(ddlCompaniaB.SelectedValue), "", 0, "");
                         BibliotecaClasesSICDB.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestinoB, 62, "- Todos -", Convert.ToInt32(ddlCompaniaB.SelectedValue), "", 0, "");
                         //Asignando Lugar de origen
                         ddlOrigenB.SelectedValue = i.id_instalacion.ToString();
                     }
                 }*/

                //Definiendo variables de filtrado de fecha (validando filtrado realizado y por filtro de llegada o salida)
                DateTime inicio_salida = chkFechasBusqueda.Checked ? (rdbFechaSalida.Checked ? Convert.ToDateTime(txtFechaInicioB.Text) : DateTime.MinValue) : DateTime.MinValue;
                DateTime fin_salida = chkFechasBusqueda.Checked ? (rdbFechaSalida.Checked ? Convert.ToDateTime(txtFechaFinB.Text).AddHours(23.9999) : DateTime.MinValue) : DateTime.MinValue;
                DateTime inicio_llegada = chkFechasBusqueda.Checked ? (rdbFechaLlegada.Checked ? Convert.ToDateTime(txtFechaInicioB.Text) : DateTime.MinValue) : DateTime.MinValue;
                DateTime fin_llegada = chkFechasBusqueda.Checked ? (rdbFechaLlegada.Checked ? Convert.ToDateTime(txtFechaFinB.Text) : DateTime.MinValue) : DateTime.MinValue;

                //Realizando la búsqueda de paquetes
                using (DataTable mit = SAT_CL.ControlEvidencia.Reportes.CargaReportePaquetesEnvio( Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), Convert.ToInt32(ddlOrigenB.SelectedValue), Convert.ToInt32(ddlDestinoB.SelectedValue),
                                        Convert.ToInt32(ddlEstatusB.SelectedValue), Convert.ToInt32(ddlMedioEnvioB.SelectedValue), txtReferenciaB.Text, inicio_salida, fin_salida, inicio_llegada, fin_llegada))
                {
                    //Validando origen de datos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    {
                        //Realizando llenado de GV
                        Controles.CargaGridView(gvBusqueda, mit, "Id", lblCriterioGridViewBusqueda.Text, true, 1);
                        //Almacenando en sesión
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");
                    }
                    else
                    {
                        //Inicializando gridview
                        Controles.InicializaGridview(gvBusqueda);
                        //Eliminando tabla anterior
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    }
                }

                //Inicializando indices de selección
                Controles.InicializaIndices(gvBusqueda);
            }
            //De lo contrario
            else
            {
                //Recuperando tabla de sesión
                DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2");

                //Si no existe ningún origen de datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    //Recargando contenido de GV
                    Controles.CargaGridView(gvBusqueda, mit, "Id", lblCriterioGridViewBusqueda.Text, true, 1);
                else
                    //Cargando GV vacío
                    Controles.InicializaGridview(gvBusqueda);
            }
        }
        /// <summary>
        /// Determina si es posible realizar alguna edición sobre el paquete y su contenido
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaEdicionPaquete()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(Convert.ToInt32(Session["id_registro"]));

            //Instanciando registro
            using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el paquete existe
                if (p.id_paquete_envio > 0)
                {
                    //Si el paquete está en estatus distinto de registrado
                    if (p.id_estatus != PaqueteEnvio.EstatusPaqueteEnvio.Registrado)
                        resultado = new RetornoOperacion("El paquete sólo puede ser editado en estatus 'Registrado'.");
                    else if (p.id_terminal_origen != Convert.ToInt32(ddlOrigen.SelectedValue))
                    {
                        resultado = new RetornoOperacion("El paquete sólo puede ser editado por un usuario del mismo Lugar de Origen del envío.");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza el guardado del paquete
        /// </summary>
        private void guardaPaquete()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(); 
           //Validamos que el Origen sea Diferente que el Destino
            if (ddlOrigen.SelectedValue != ddlDestino.SelectedValue)
            {
                //Validamos Selección de Origen
                if (ddlOrigen.SelectedValue != "0")
                {
                    //Validamos Selección Destino
                    if (ddlDestino.SelectedValue != "0")
                    {
                        //Validamos Selección Medio de Envió
                        if (ddlMedioEnvio.SelectedValue != "0")
                        {
                            //Determinando tipo de guardado en base a la estatus
                            switch ((Pagina.Estatus)Session["estatus"])
                            {
                                case Pagina.Estatus.Nuevo:
                                    //Insertando nuevo paquete
                                    resultado = PaqueteEnvio.InsertaPaqueteEnvio(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), Convert.ToInt32(ddlOrigen.SelectedValue),
                                                                    Convert.ToInt32(ddlDestino.SelectedValue), Convert.ToInt32(ddlMedioEnvio.SelectedValue), txtReferencia.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                    break;
                                case Pagina.Estatus.Edicion:
                                    //Instanciando al registro activo en sesión
                                    using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(Session["id_registro"])))
                                    {
                                        //Si el paquete existe
                                        if (p.id_paquete_envio > 0)
                                            //Realizando edición
                                            resultado = p.EditaPaqueteEnvio(Convert.ToInt32(ddlMedioEnvio.SelectedValue), txtReferencia.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                        else
                                            resultado = new RetornoOperacion("Paquete no encontrado.");
                                    }
                                    break;
                            }

                            //Si no existe ningún error
                            if (resultado.OperacionExitosa)
                            {
                                //Asignando Id de de registro activo en sesión
                                Session["id_registro"] = resultado.IdRegistro;
                                Session["estatus"] = (Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo ? Pagina.Estatus.Edicion : Pagina.Estatus.Lectura;
                                //Cargando contenido de ventana
                                inicializaPagina();
                            }

                            //Mostrando resultado
                            lblError.Text = resultado.Mensaje;
                        }
                        else
                        {
                            lblError.Text = "Seleccione el Medio de Envió";
                        }
                    }
                    else
                    {
                        lblError.Text = "Seleccione el Destino";
                    }
                }

                else
                {
                    lblError.Text = "Seleccione el Origen";
                }
            }
            else
            {
                lblError.Text = "El Origen debe ser diferente al Destino";
            }
        }
        /// <summary>
        /// Realiza la deshabilitación del paquete y sus dependencias
        /// </summary>
        private void deshabilitaPaquete()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando al registro activo en sesión
            using (PaqueteEnvio p = new PaqueteEnvio(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el paquete existe
                if (p.id_paquete_envio > 0)
                    //Realizando edición
                    resultado = p.DeshabilitaPaqueteEnvioCascada(((Usuario)Session["usuario"]).id_usuario);
                else
                    resultado = new RetornoOperacion("Paquete no encontrado.");
            }

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Asignando Id de de registro activo en sesión
                Session["id_registro"] = 0;
                Session["estatus"] = Pagina.Estatus.Nuevo;
                //Cargando contenido de ventana
                inicializaPagina();
            }

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza el envío del paquete a su terminal de destino
        /// </summary>
        private void enviaPaquete()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Recuperando los detalles del paquete
            using (DataTable mit = PaqueteEnvioDocumento.CargaDocumentoDelPaquete(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando el origen de datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Inicializando transacción
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {

                        //Para cada uno de los detalles
                        foreach (DataRow r in mit.Rows)
                        {
                            //Instanciando el detalle de paquete
                            using (PaqueteEnvioDocumento dp = new PaqueteEnvioDocumento(Convert.ToInt32(r["Id"])))
                            {
                                //Si el detalle de paquete existe
                                if (dp.id_paquete_envio_documento > 0)
                                {
                                    //Realizando la actualización de su estatus a "transito"
                                    resultado = dp.ActualizaEstatusPaqueteEnvioDocumento(PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.Transito, ((Usuario)Session["usuario"]).id_usuario);

                                    //Si no hay error
                                    if (resultado.OperacionExitosa)
                                    {
                                        //instanciando documento
                                        using (ControlEvidenciaDocumento d = new ControlEvidenciaDocumento(dp.id_control_evidencia_documento))
                                        {
                                            //Instanciando Servicio Evidencia
                                            using (ServicioControlEvidencia se = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicioControlEvidencia, d.id_servicio_control_evidencia))
                                            {

                                                //SI el documento existe
                                                if (d.id_control_evidencia_documento > 0)
                                                {
                                                    //Actualizando el documento
                                                    resultado = d.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.Transito, ((Usuario)Session["usuario"]).id_usuario);

                                                    //Validamos Actualización de Control Evidencia
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizando estatus del Servicio Control de evidencia
                                                        resultado = se.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                }
                                                else
                                                {
                                                    resultado = new RetornoOperacion(string.Format("Documento Id:{0} no encontrado.", dp.id_control_evidencia_documento));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion(string.Format("Detalle de paquete Id:{0} no encontrado.", r["Id"]));
                            }

                            //Si existe algún error
                            if (!resultado.OperacionExitosa)
                                break;
                        }
                        //Finalizando transacción
                        if (resultado.OperacionExitosa)
                        {
                            scope.Complete();
                        }
                    }
                }
                else
                    resultado = new RetornoOperacion("No existen documentos en este paquete. El paquete no puede ser enviado");
            }

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus en sesión
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Cargando contenido de ventana
                inicializaPagina();
            }

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Recupera los documentos seleccionados para añadir al paquete actual y genera los detalle de envío
        /// </summary>
        private void anadeSeleccionPaquete()
        {
            //Validando existencia de registros
            if (gvDocumentosA.DataKeys.Count > 0)
            {
                //Recuperandoselección de GridView
                GridViewRow[] seleccionados = Controles.ObtenerFilasSeleccionadas(gvDocumentosA, "chkVariosDocumentos");

                //Si no existen resultados
                if (seleccionados.Length > 0)
                {
                    //Inicio de resultado
                    lblError2.Text = "Resultado Agregar Selección:<br/>";

                    //Definiendo objeto de resultado
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Para cada uno de los registros seleccionados
                    foreach (GridViewRow r in seleccionados)
                    {
                        //Seleccionando la fila actual
                        gvDocumentosA.SelectedIndex = r.RowIndex;
                        //Instanciando registro documento
                        using (ControlEvidenciaDocumento d = new ControlEvidenciaDocumento(Convert.ToInt32(gvDocumentosA.SelectedDataKey.Value)))
                        {
                            //Instanciando registro documento
                            using (ServicioControlEvidencia se = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicioControlEvidencia, d.id_servicio_control_evidencia))
                            {
                                //Si el documento existe
                                if (d.id_control_evidencia_documento > 0)
                                {
                                    //Determinando acciones a realizar para el documento en base al estatus actual
                                    switch (d.id_estatus_documento)
                                    {
                                        case ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio:
                                            //insertando detalle de paquete
                                            resultado = PaqueteEnvioDocumento.InsertaPaqueteEnvioDocumento(d.id_control_evidencia_documento, Convert.ToInt32(Session["id_registro"]), ((Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        case ControlEvidenciaDocumento.EstatusDocumento.En_Aclaracion:
                                            //Inicializando Transacción
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Recuperando el detalle de paquete anterior
                                                using (PaqueteEnvioDocumento dp = PaqueteEnvioDocumento.ObtieneUltimoDetallePaqueteDocumento(d.id_control_evidencia_documento))
                                                {
                                                    //Si el detalle de paquete existe
                                                    if (dp.id_paquete_envio_documento > 0)
                                                    {
                                                        //Actualizando estatus a No Recibido
                                                        resultado = dp.ActualizaEstatusPaqueteEnvioDocumento(PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.No_Recibidos, ((Usuario)Session["usuario"]).id_usuario);
                                                        //Si no hay errores
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando estatus de documento
                                                            resultado = d.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio, ((Usuario)Session["usuario"]).id_usuario);

                                                            //Validamos Actualización de Control Evidencia
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Actualizando estatus del Servicio Control de evidencia
                                                                resultado = se.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Si no existe error
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Insertando nuevo detalle de paquete
                                                                    resultado = PaqueteEnvioDocumento.InsertaPaqueteEnvioDocumento(d.id_control_evidencia_documento, Convert.ToInt32(Session["id_registro"]), ((Usuario)Session["usuario"]).id_usuario);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        resultado = new RetornoOperacion("El detalle de paquete 'En Aclaración' no pudo ser recuperado.");
                                                }

                                                //Finalizando transacción
                                                //Finalizando transacción
                                                if (resultado.OperacionExitosa)
                                                {
                                                    scope.Complete();
                                                }
                                            }
                                            break;
                                        default:
                                            resultado = new RetornoOperacion("Operación no válida.");
                                            break;
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion(string.Format("El Documento '{0}' no existe.", gvDocumentosA.SelectedDataKey.Value));

                                //Sobreescribiendo resultado general de documento
                                resultado = new RetornoOperacion(string.Format("{0}-{1}: {2}", gvDocumentosA.SelectedDataKey["Viaje"], gvDocumentosA.SelectedDataKey["Documento"], resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                        //Añadiendo resultado a etiqueta de resultados                    
                        lblError2.Text += "• " + resultado.Mensaje + "<br/>";
                    }

                    //Actualizando documnetos y detalle de paquete
                    cargaDocumentosPenientesEnvio();
                    cargaDocumentosPaquete();
                }
            }
        }
        /// <summary>
        /// Añade todos los elementos del origen de datos del GV de Pendientes
        /// </summary>
        private void anadeTodosPaquete()
        {
            //Validando existencia de registros
            if (gvDocumentosA.DataKeys.Count > 0)
            {
                //Recuperando origen de datos
                DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");

                //Inicio de resultado
                lblError2.Text = "Resultado Agregar Todos:<br/>";

                //Definiendo objeto de resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Para cada uno de los registros seleccionados
                foreach (DataRow r in mit.Rows)
                {
                    //Instanciando registro documento
                    using (ControlEvidenciaDocumento d = new ControlEvidenciaDocumento(Convert.ToInt32(r["Id"])))
                    {
                        //Instanciando registro documento
                        using (ServicioControlEvidencia se = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicioControlEvidencia, d.id_servicio_control_evidencia))
                        {

                            //Si el documento existe
                            if (d.id_control_evidencia_documento > 0)
                            {
                                //Determinando acciones a realizar para el documento en base al estatus actual
                                switch (d.id_estatus_documento)
                                {
                                    case ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio:
                                        //insertando detalle de paquete
                                        resultado = PaqueteEnvioDocumento.InsertaPaqueteEnvioDocumento(d.id_control_evidencia_documento, Convert.ToInt32(Session["id_registro"]), ((Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    case ControlEvidenciaDocumento.EstatusDocumento.En_Aclaracion:
                                        //Inicializando Transacción

                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Recuperando el detalle de paquete anterior
                                            using (PaqueteEnvioDocumento dp = PaqueteEnvioDocumento.ObtieneUltimoDetallePaqueteDocumento(d.id_control_evidencia_documento))
                                            {
                                                //Si el detalle de paquete existe
                                                if (dp.id_paquete_envio_documento > 0)
                                                {
                                                    //Actualizando estatus a No Recibido
                                                    resultado = dp.ActualizaEstatusPaqueteEnvioDocumento(PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.No_Recibidos, ((Usuario)Session["usuario"]).id_usuario);
                                                    //Si no hay errores
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        //Actualizando estatus de documento
                                                        resultado = d.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio, ((Usuario)Session["usuario"]).id_usuario);

                                                        //Validamos Actualización de Control Evidencia
                                                        if (resultado.OperacionExitosa)
                                                        {
                                                            //Actualizando estatus del Servicio Control de evidencia
                                                            resultado = se.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            //Si no existe error
                                                            if (resultado.OperacionExitosa)
                                                                //Insertando nuevo detalle de paquete
                                                                resultado = PaqueteEnvioDocumento.InsertaPaqueteEnvioDocumento(d.id_control_evidencia_documento, Convert.ToInt32(Session["id_registro"]), ((Usuario)Session["usuario"]).id_usuario);
                                                        }
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion("El detalle de paquete 'En Aclaración' no pudo ser recuperado.");
                                            }


                                            //Finalizando transacción
                                            if (resultado.OperacionExitosa)
                                            {
                                                scope.Complete();
                                            }
                                        }
                                        break;
                                    default:
                                        resultado = new RetornoOperacion("Operación no válida.");
                                        break;
                                }
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("El Documento '{0}' no existe.", gvDocumentosA.SelectedDataKey.Value));

                            //Sobreescribiendo resultado general de documento
                            resultado = new RetornoOperacion(string.Format("{0}-{1}: {2}", r["Viaje"], r["Documento"], resultado.Mensaje), resultado.OperacionExitosa);
                        }
                    }

                    //Añadiendo resultado a etiqueta de resultados                    
                    lblError2.Text += "• " + resultado.Mensaje + "<br/>";
                }

                //Actualizando documnetos y detalle de paquete
                cargaDocumentosPenientesEnvio();
                cargaDocumentosPaquete();
            }
        }
        /// <summary>
        /// Remueve los elementos seleccionados del paquete actual
        /// </summary>
        private void quitaSeleccionPaquete()
        {
            //Validando existencia de registros
            if (gvDocumentosB.DataKeys.Count > 0)
            {
                //Recuperandoselección de GridView
                GridViewRow[] seleccionados = Controles.ObtenerFilasSeleccionadas(gvDocumentosB, "chkVariosPaquetes");

                //Si no existen resultados
                if (seleccionados.Length > 0)
                {
                    //Inicio de resultado
                    lblError2.Text = "Resultado Quitar Selección:<br/>";

                    //Definiendo objeto de resultado
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Para cada uno de los registros seleccionados
                    foreach (GridViewRow r in seleccionados)
                    {
                        //Seleccionando la fila actual
                        gvDocumentosB.SelectedIndex = r.RowIndex;

                        //Instanciando detalle de paquete
                        using (PaqueteEnvioDocumento dp = new PaqueteEnvioDocumento(Convert.ToInt32(gvDocumentosB.SelectedDataKey.Value)))
                        {
                            //Si el detalle de paquete existe
                            if (dp.id_paquete_envio_documento > 0)
                            {
                                //Realizando la deshabilitación del detallle
                                resultado = dp.DeshabilitaPaqueteEnvioDocumento(((Usuario)Session["usuario"]).id_usuario);
                                //Sobreescribiendo resultado general de documento
                                resultado = new RetornoOperacion(string.Format("{0}-{1}: {2}", gvDocumentosB.SelectedDataKey["Viaje"], gvDocumentosB.SelectedDataKey["Documento"], resultado.Mensaje), resultado.OperacionExitosa);
                            }
                        }
                        //Añadiendo resultado a etiqueta de resultados                    
                        lblError2.Text += "• " + resultado.Mensaje + "<br/>";
                    }

                    //Actualizando documnetos y detalle de paquete
                    cargaDocumentosPenientesEnvio();
                    cargaDocumentosPaquete();
                }
            }
        }
        /// <summary>
        /// Remueve todos los elementos del paquete actual
        /// </summary>
        private void quitaTodosPaquete()
        {
            //Validando existencia de registros
            if (gvDocumentosB.DataKeys.Count > 0)
            {
                //Recuperandoselección de GridView
                DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1");

                //Inicio de resultado
                lblError2.Text = "Resultado Quitar Todos:<br/>";

                //Definiendo objeto de resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Para cada uno de los registros seleccionados
                foreach (DataRow r in mit.Rows)
                {
                    //Instanciando detalle de paquete
                    using (PaqueteEnvioDocumento dp = new PaqueteEnvioDocumento(Convert.ToInt32(r["Id"])))
                    {
                        //Si el detalle de paquete existe
                        if (dp.id_paquete_envio_documento > 0)
                        {
                            //Realizando la deshabilitación del detallle
                            resultado = dp.DeshabilitaPaqueteEnvioDocumento(((Usuario)Session["usuario"]).id_usuario);
                            //Sobreescribiendo resultado general de documento
                            resultado = new RetornoOperacion(string.Format("{0}-{1}: {2}", r["Viaje"], r["Documento"], resultado.Mensaje), resultado.OperacionExitosa);
                        }
                    }
                    //Añadiendo resultado a etiqueta de resultados                    
                    lblError2.Text += "• " + resultado.Mensaje + "<br/>";
                }

                //Actualizando documnetos y detalle de paquete
                cargaDocumentosPenientesEnvio();
                cargaDocumentosPaquete();
            }
        }

        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacora(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1) + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }

        #endregion
    }
}