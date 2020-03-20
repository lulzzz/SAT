using SAT_CL;
using SAT_CL.ControlEvidencia;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.ControlEvidencia
{
    public partial class VisorDocumentos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(uphplImagenZoom, uphplImagenZoom.GetType(), "jqzoom", @"$(document).ready(function() {$('.MYCLASS').jqzoom({ 
            zoomType: 'standard',  alwaysOn : false,  zoomWidth: 435,  zoomHeight:300,  position:'left',  
            xOffset: 125,  yOffset:0,  showEffect : 'fadein',  hideEffect: 'fadeout'  });});", true);       
            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)
            {   //Invocando Método de Inicializacion
                inicializaPagina();

            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Atras"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAtras_Click(object sender, ImageClickEventArgs e)
        {   //Direcciona a Pagina Anterior
            PilaNavegacionPaginas.DireccionaPaginaAnterior();
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            //Obteniendo Tablas de los Reportes
            using (DataSet dtables = Reportes.CargaReporteDocumentos(
                 txtNViaje.Text, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)),
                 Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                 Convert.ToByte(ddlEstatus.SelectedValue), txtReferencia.Text,
                 Fecha.ConvierteStringDateTime(txtFechaInicial.Text), Fecha.ConvierteStringDateTime(txtFechaFinal.Text), txtCartaPorte.Text))
            {
                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(dtables, "Table") && Validacion.ValidaOrigenDatos(dtables, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvResumen, dtables.Tables["Table"], "Documentos", "", true, 1);
                    Controles.CargaGridView(gvDetalles, dtables.Tables["Table1"], "IdServicio-IdServicioControlEvidencia", "", true, 2);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtables.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtables.Tables["Table1"], "Table1");
                    gvResumen.FooterRow.Cells[2].Text = (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalServicios)", "")).ToString();

                    //Carga grafica
                    Controles.CargaGrafica(ChtDocumentos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").DefaultView,
                                          "Documentos", "TotalServicios", true);

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvDetalles);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }//Quitando Selección de los Detalles
                gvDetalles.SelectedIndex = -1;
            }

            //Inicializamos Controles Para Carga de Imagenes
            //Cambiamos Vista
            mtvDocumentosDigitalizados.ActiveViewIndex = 1;
            //Carga Imagenes
            cargaImagenDocumentos();
            //Cambiando estilos de pestañas
            btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
            btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
        }

        /// <summary>
        /// Click en check de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFechasInicio_CheckedChanged(object sender, EventArgs e)
        {
            //Aplicando habilitació nde controles de fecha y valores predeterminados
            if (chkFechasInicio.Checked)
            {
                txtFechaInicial.Enabled = txtFechaFinal.Enabled = true;
                //Asignando fechas de filtrado
                //Fecha actual
                DateTime fecha = Fecha.ObtieneFechaEstandarMexicoCentro();
                //Dia de la semana actual
                DayOfWeek hoy = fecha.DayOfWeek;
                //Fecha actual menos la cantidad de días del día de la semana
                txtFechaInicial.Text = fecha.Date.AddDays(-1 * ((double)hoy - 1)).ToString("dd/MM/yyyy HH:mm");
                //Fecha actual
                txtFechaFinal.Text = fecha.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                txtFechaInicial.Enabled = txtFechaFinal.Enabled = false;
                txtFechaInicial.Text = txtFechaFinal.Text = "";
            }
        }

        #region Eventos GridView "Resumen"

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumen_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Muestra el Nombre de la columna por la cual se ordena el GridView
                lblOrdenarResumen.Text = Controles.CambiaSortExpressionGridView(gvResumen, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            }
        }

        #endregion

        #region Eventos GridView "Detalles"

        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Exportar Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarDetalles_OnClick(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDetalles.DataKeys.Count > 0)
            {   //Exporta el Contenido del GridView "gvResultado" (Recupera el DataTable del DataSet)
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño de Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDetalles_OnSelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDetalles.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Detalles" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDetalles.SelectedValue), true, 2);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de página del Gridview "Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDetalles.DataKeys.Count > 0)
            {   //Cambia el Indice de la Pagina del GridView
                Controles.CambiaIndicePaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDetalles.DataKeys.Count > 0)
            {   //Muestra el Nombre de la columna por la cual se ordena el GridView
                lblOrdenarDetalles.Text = Controles.CambiaSortExpressionGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Bitacora"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_OnClick(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDetalles.DataKeys.Count > 0)
            {   //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);
                //Validando que existe el Id
                if (Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicio"]) != 0)
                    //Invocando Método que inicializa la Bitacora
                    inicializaBitacora(Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicio"]), 1, "Servicio");
                //Carga grafica
                Controles.CargaGrafica(ChtDocumentos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").DefaultView,
                                      "Documentos", "TotalServicios", true);

            }
        }
        /// <summary>
        /// Evento que abre la ventana de bitacora sobre las evidencias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacoraEvidencias_Click(object sender, EventArgs e)
        {
            if (gvDetalles.DataKeys.Count > 0)
            {   //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);
                //Validando que existe el Id
                if (Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicioControlEvidencia"]) != 0)
                    //Invocando Método que inicializa la Bitacora
                    inicializaBitacoraEvidencias(Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicioControlEvidencia"]), 43, "ServicioControlEvidencia");

            }            
        }
        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Pagina
        /// </summary>
        private void inicializaPagina()
        {   //Invocando Método de Carga de Catalogos
            cargaCatalogos();
            //Inicializa
            Controles.InicializaGridview(gvResumen);
            Controles.InicializaGridview(gvDetalles);
            Controles.InicializaGridview(gvDocumentosDigitalizados);

            //Asignando fechas de filtrado
            //Fecha actual
            DateTime fecha = Fecha.ObtieneFechaEstandarMexicoCentro();
            //Dia de la semana actual
            DayOfWeek hoy = fecha.DayOfWeek;
            //Fecha actual menos la cantidad de días del día de la semana
            txtFechaInicial.Text = fecha.Date.AddDays(-1 * ((double)hoy - 1)).ToString("dd/MM/yyyy HH:mm");
            //Fecha actual
            txtFechaFinal.Text = fecha.ToString("dd/MM/yyyy HH:mm");

            //Enfocando el Primer Control
            txtCliente.Focus();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Instanciamos Compañia  para visualización en el control
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
            }
            //Invocando Métodos de Carga de los Catalogos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDetalles, "", 26);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "Todos", 42);
            //Tamaño GV Documentos Digitalizados
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentosDigitalizados, "", 18);
        }

        #endregion

        #region Eventos Digitalización

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Documentos Digitalizadas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoDocumentosDigitalizados_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDocumentosDigitalizados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"),
                                        Convert.ToInt32(ddlTamanoDocumentosDigitalizados.SelectedValue), true, 6);
        }

        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Documentos Digitalizados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDocumentosDigitalizados_OnClick(object sender, EventArgs e)
        {
            //Exportando Documentos
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"));
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosDigitalizados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el Indice de pagina del GridView
            Controles.CambiaIndicePaginaGridView(gvDocumentosDigitalizados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex, true, 6);
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Documentos Digitalizados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentosDigitalizados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
         lblDocumentosDigitalizados.Text = Controles.CambiaSortExpressionGridView(gvDocumentosDigitalizados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression, true, 6);
        }

  

        /// <summary>
        /// Evento producido al seleccionar alguna opcion de documento Digitalizado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDocumentoDigitalizado_Click(object sender, EventArgs e)
        {
            //Validamos Datos
            if (gvDocumentosDigitalizados.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvDocumentosDigitalizados, sender, "lnk", false);

                    //Determinando el comando que produjo el evento
                    switch (((LinkButton)sender).CommandName)
                    {
                        case "Imagen":
                            inicializaImagenes(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey.Value), 36, "Control Evidencia - Documento", 4);
                            break;
                        case "Bitacora":
                            inicializaBitacora(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey.Value), 36, "Control Evidencia - Documento");
                            break;
                        case "Referencias":
                            inicializaReferencias(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey.Value), 36, "Control Evidencia - Documento");
                            break;

                    }
                
            }

        }

           /// <summary>
        /// Evento genrado al dar click en Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDocumentos_Click(object sender, EventArgs e)
        { 
            //Si hay registros
            if (gvDetalles.DataKeys.Count > 0)
            {
                //Inicializamos Controles para Vista de Documentos
                
                //Cambiamos Vista
                mtvDocumentosDigitalizados.ActiveViewIndex = 0;
                //Cambiando estilos de pestañas
                btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana";

                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando a ejecutar es Recibido 
                if (lkb.CommandName == "Recibido")
                {

                    //Cargamos Documentos Digitalizados
                    cargaDocumentosViajeoDigitalizado();
                }
                else
                    //Inicializamos Grid View
                    Controles.InicializaGridview(gvDocumentosDigitalizados);
                 
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
            if (gvDetalles.DataKeys.Count > 0)
            {
                //Determinando la pestaña pulsada
                switch (((Button)sender).CommandName)
                {
                    case "RecibirDocumentosDigitalizados":
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana";
                        //Asignando vista activa de la forma
                        mtvDocumentosDigitalizados.SetActiveView(vwRecibirDocumentosDigitalizados);
                        break;
                    case "DocumentosDigitalizados":
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        //Asignando vista activa de la forma
                        mtvDocumentosDigitalizados.SetActiveView(vwDocumentosDigitalizados);
                        //Craga Documentos
                        cargaImagenDocumentos();
                        break;
                }
            }
        }

        /// <summary>
        /// Evento producido 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbThumbnailDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            //URL de imagen a mostrar en panel de zoom
            //hplImagenZoom.ImageUrl = string.Format("../../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            hplImagenZoom.NavigateUrl = string.Format("~/Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("~/Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }

        /// <summary>
        /// Evento producido al cambiar el listado de imagenes a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbVerReales_CheckedChanged(object sender, EventArgs e)
        {
            cargaImagenDocumentos();
        }
        /// <summary>
        /// Evento producido al cambiar el listado de imagenes a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbVerEjemplos_CheckedChanged(object sender, EventArgs e)
        {
            cargaImagenDocumentos();
        }
        #endregion

        #region Métodos Documentos Digitalizados

        /// <summary>
        /// Realzia la carga de los viajes que aún tienen documentos por recibir
        /// </summary>
        private void cargaDocumentosViajeoDigitalizado()
        {
            //Inicializando indice de selección
            Controles.InicializaIndices(gvDocumentosDigitalizados);

            //Obteniendo detalles de viaje
            using (DataTable dt = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicio"]),
                                          0, 0))
            {
                //Validando que la Tabla Contenga Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView de Viajes
                    Controles.CargaGridView(gvDocumentosDigitalizados, dt, "Id-IdHID-Documento-IdLugarCobro-IdSegmento-IdSegmentoControlEvidencia-Estatus", lblDocumentosDigitalizados.Text, true, 6);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table3");
                }
                else
                {
                    //Inicializando gridView
                    Controles.InicializaGridview(gvDocumentosDigitalizados);
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacoraEvidencias(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1) + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        /// <param name="id_configuracion_tipo_archivo">Id de Configuración de tipo de archivo s seleccionar</param>
        private void inicializaImagenes(int id_registro, int id_tabla, string nombre_tabla, int id_configuracion_tipo_archivo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1) + "&idTV=" + id_configuracion_tipo_archivo + "&tB=" + nombre_tabla + "&actualizaPadre=" + false);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            ScriptServer.AbreNuevaVentana(url, "Imagenes", configuracion, Page);
        }

        /// <summary>
        /// Realiza la carga de la galería de imagenes
        /// </summary>
        private void cargaImagenDocumentos()
        {
            //Vista previa por default
            imgImagenZoom.ImageUrl = "~/Image/noDisponible.jpg";
            hplImagenZoom.NavigateUrl = "~/Image/noDisponible.jpg";

            //Si no hay viaje seleccionado
            if (gvDetalles.SelectedIndex == -1)
                //Cargando lista vacía
                Controles.CargaDataList(dtlImagenDocumentos, null, "URL", "", "");
            else
            {
                //Origen de datos vacío
                DataTable mit = null;

                //Si la carga es en base a documentos reales de la orden
                if (rdbVerReales.Checked)
                    //Realizando la carga de URL de imagenes a mostrar
                    mit = ControlEvidenciaDocumento.ObtieneControlEvidenciaDocumentosImagenes(Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicio"]));
                //Si la carga es con ejemplos
                else
                    //Realizando la carga de URL de imagenes a mostrar
                    mit = HojaInstruccionDocumento.ObtieneHojasDeInstruccionesDocumentosImagenes(Convert.ToInt32(gvDetalles.SelectedDataKey["IdServicioControlEvidencia"]));

                //Cargando DataList
                Controles.CargaDataList(dtlImagenDocumentos, mit, "URL", "", "");
            }
        }

        #endregion



    

        
    }
}