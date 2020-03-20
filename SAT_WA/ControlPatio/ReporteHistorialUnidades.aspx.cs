using SAT_CL.ControlPatio;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SAT.ControlPatio
{
    public partial class ReporteHistorialUnidades : System.Web.UI.Page
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
            //Validando si se Produjo un PostBack
            if(!(Page.IsPostBack))
                
                //Inicializando Página
                inicializarPagina();

            //Construyendo Script
            string script = @"<script type='text/javascript'>
                                $(document).ready(function(){
                                    $('#ctl00_content1_txtTransportista').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=21&param=" + ddlPatio.SelectedValue + @"' })
                                });
                            </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaTransportista", script, false);

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(uphplImagenZoom, uphplImagenZoom.GetType(), "jqzoom", @"$(document).ready(function() {$('.MYCLASS').jqzoom({ 
            zoomType: 'standard',  alwaysOn : false,  zoomWidth: 435,  zoomHeight:300,  position:'left',  
            xOffset: 125,  yOffset:0,  showEffect : 'fadein',  hideEffect: 'fadeout'  });});", true);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Patios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaHistorialUnidades();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaHistorialUnidades();
        }
        /// <summary>
        /// Evento Producido al Seleccionar el Control "Incluir Fechas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Validación
            configuraValidacionFechas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Link "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbThumbnailDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }
        /// <summary>
        /// Evento disparado al dar click en el link button para cerrar la ventana modal de imagenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);

            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            hplImagenZoom.NavigateUrl = "~/Image/noDisponible.jpg";

            //Imagen seleccionada
            imgImagenZoom.ImageUrl = "~/Image/noDisponible.jpg";

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();

            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                                $('#contenidoVentanaEvidencias').animate({ width: 'toggle' });
                                                $('#ventanaEvidencias').animate({ width: 'toggle' });
                                              </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), "Imagenes", script, false);

        }
        /// <summary>
        /// Evento Disparado al Presionar el Boton "Imagenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEvidencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {
                //Seleccionando la Fila
                TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "lnk", false);

                //Validando si existen Evidencias
                if (Convert.ToInt32(gvEntidades.SelectedDataKey["NoEvidencias"]) > 0)
                {
                    //Cargando Imagenes al DataList
                    cargaImagenesDetalle();
                    //Actualizamos el updatepanel
                    updtlImagenImagenes.Update();
                    uphplImagenZoom.Update();

                    //Declarando Script de Ventana Modal
                    string script = @"<script type='text/javascript'>
                                                $('#contenidoVentanaEvidencias').animate({ width: 'toggle' });
                                                $('#ventanaEvidencias').animate({ width: 'toggle' });
                                              </script>";
                    //Registrando Script
                    ScriptManager.RegisterStartupScript(upgvEntidades, upgvEntidades.GetType(), "Imagenes", script, false);
                }
            }
        }

        #region Eventos GridView "Entidades"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
                
                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id-IdEvt");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obteniendo Control
            LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkEvidencias");

            //Validando que exista el Control
            if (lnkButton != null)
            {
                //Validación de Datos
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[10].ToString() != "0")
                    //Mostrando Control
                    lnkButton.Visible = true;
                else
                    //Ocultando Control
                    lnkButton.Visible = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "lnk", false);

                //Obteniendo Eventos Terminados
                using (DataTable dtEventoTerminados = EventoDetalleAcceso.ObtieneEventosDetalleTerminados(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                {
                    //Validando que existan los Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventoTerminados))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvBitacora, dtEventoTerminados, "Id", "", true, 1);

                        //Añadiendo Tabla a DataSet
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtEventoTerminados, "Table1");
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvBitacora);

                        //Eliminando Tabla a DataSet
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    }
                }

                //Creando Script de ventana Modal
                string script = @"<script>
                                    $('#contenidoBitacoraUnidades').animate({ width: 'toggle' });
                                    $('#bitacoraUnidades').animate({ width: 'toggle' });
                                  </script>";

                //Registrando Script
                ScriptManager.RegisterStartupScript(upgvEntidades, upgvEntidades.GetType(), "MuestraBitacora", script, false);
            }
        }

        #endregion

        #region Eventos Bitacora

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], Convert.ToInt32(ddlTamanoBit.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacora_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoBit.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarBit_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvBitacora.DataKeys.Count > 0)

                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table1"], "Id");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);

            //Creando Script de ventana Modal
            string script = @"<script>
                                    $('#contenidoBitacoraUnidades').animate({ width: 'toggle' });
                                    $('#bitacoraUnidades').animate({ width: 'toggle' });
                                  </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(upbtnCerrar, upbtnCerrar.GetType(), "MuestraBitacora", script, false);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializarPagina()
        {
            //Inicializando GridViews
            TSDK.ASP.Controles.InicializaGridview(gvEntidades);
            TSDK.ASP.Controles.InicializaGridview(gvTiempoUnidades);

            //Cargando Catalogos
            cargaCatalogos();

            //Inicializando Controles de Fecha
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            
            //Cargamos la consulta inicial 
            buscaHistorialUnidades();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
            {   
                //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Todos", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", 0, "");
                
                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();
            }

            //Cargando Catalogo de Tipos de Entidades
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEntidad, "Todos", 67);

            //Cargando Catalogo de Estatus de Acceso
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusAcceso, "Todos", 72);           

            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            
            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoBit, "", 18);
        }
        /// <summary>
        /// Método Privado encargado de Buscar el Historial de las Unidades
        /// </summary>
        private void buscaHistorialUnidades()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;
            
            //Validando si se Incluiran Fechas
            if(chkFechas.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }
            
            //Obteniendo Reporte de Historial de Unidades
            using (DataSet dsHistorial = Reporte.ObtieneHistorialUnidades(Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1)),
                                            Convert.ToByte(ddlTipoEntidad.SelectedValue), txtDescripcion.Text, txtIdentificacion.Text, Convert.ToInt32(ddlEstatusAcceso.SelectedValue),
                                            0, fec_ini, fec_fin))
            {
                //Valida que existan las Tablas en el DataSet
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsHistorial, true))
                {
                    
                    //Cargando GridViews
                    //Consulta General
                    TSDK.ASP.Controles.CargaGridView(gvEntidades, dsHistorial, 0, "Id-NoEvidencias", "", true, 1);
                    //Grafica tiempos por unidad
                    TSDK.ASP.Controles.CargaGridView(gvTiempoUnidades, dsHistorial, 1, "EstatusTiempo");
                    //Grafica Unidades por tipo
                    TSDK.ASP.Controles.CargaGridView(gvUnidadesTipo, dsHistorial, 3, "");
                    //Grafica Transportistas
                    TSDK.ASP.Controles.CargaGridView(gvTransportista, dsHistorial, 2, "");

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsHistorial, 0, "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsHistorial, 1, "Table1");
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsHistorial, 2, "Table2");
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsHistorial, 3, "Table3");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6"),
                                                                                  System.Drawing.ColorTranslator.FromHtml("#FFAEC9"), System.Drawing.ColorTranslator.FromHtml("#B97A57"), System.Drawing.ColorTranslator.FromHtml("#C3C3C3"),
                                                                                  System.Drawing.ColorTranslator.FromHtml("#B5E61D"), System.Drawing.ColorTranslator.FromHtml("#99D9EA"), System.Drawing.ColorTranslator.FromHtml("#FF1CC7"),
                                                                                  System.Drawing.ColorTranslator.FromHtml("#FFF200"), System.Drawing.ColorTranslator.FromHtml("#E80068"), System.Drawing.ColorTranslator.FromHtml("#7F7F7F")};

                    //Cargando Grafica de Unidades por Tiempo
                    TSDK.ASP.Controles.CargaGrafica(chtTiempoUnidades, "Tiempo Transcurrido", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "EstatusTiempo", "Cantidad", false, false, colores, " ");

                    //Cargando Grafica de Unidades por tipo
                    TSDK.ASP.Controles.CargaGrafica(chtUnidadesTipo, "Tipo Unidad", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3").DefaultView,
                                         "Descripcion", "Valor", false, false, colores, " ");

                    //Cargando Grafica de Unidades por transportista
                    TSDK.ASP.Controles.CargaGrafica(chtTransportista, "Transportista", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").DefaultView,
                                         "Descripcion", "Valor", false, false, colores, " ");

                    //Mostrando Suma de Cantidades
                    gvTiempoUnidades.FooterRow.Cells[1].Text = string.Format("{0}",((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(Cantidad)",""));

                    //Recorremos la tabla de indicadores, recuperamos y almacenamos 
                    foreach (DataRow r in dsHistorial.Tables[4].Rows)
                    {
                        lblUnidades.Text = r["UnidadesEncontradas"].ToString();
                        lblTiempoProm.Text = r["TiempoPromedio"].ToString();
                        lblTransportista.Text = r["TransportistasEncontrados"].ToString();
                    }
                }
                else
                {
                    //Inicializando GridViews
                    TSDK.ASP.Controles.InicializaGridview(gvEntidades);
                    TSDK.ASP.Controles.InicializaGridview(gvUnidadesTipo);
                    TSDK.ASP.Controles.InicializaGridview(gvTiempoUnidades);
                    TSDK.ASP.Controles.InicializaGridview(gvTransportista);

                    //Eliminando las Tablas de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar la Tira de Imagenes
        /// </summary>
        private void cargaImagenesDetalle()
        {
            //Realizando la carga de URL de imagenes a mostrar
            using (DataTable mit = SAT_CL.ControlPatio.DetalleAccesoPatio.ObtieneImagenesUnidades(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
            {
                //Validando que existan imagenes a mostrar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))

                    //Cargando DataList
                    TSDK.ASP.Controles.CargaDataList(dtlImagenImagenes, mit, "URL", "", "");
                else
                    //Inicializando DataList
                    TSDK.ASP.Controles.CargaDataList(dtlImagenImagenes, null, "URL", "", "");
            }
        }
        /// <summary>
        /// Método Privado encargado de Configurar la Validación de Fechas
        /// </summary>
        private void configuraValidacionFechas()
        {
            //Validando que este Seleccionado
            if (chkFechas.Checked)
            {
                //Generamos script para validación de Fechas
                string script =
                @"<script type='text/javascript'>
                
                    var validacionBusqueda = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#ctl00_content1_txtTransportista').validationEngine('validate');
                        var isValid2 = !$('#ctl00_content1_txtFecIni').validationEngine('validate');
                        var isValid3 = !$('#ctl00_content1_txtFecFin').validationEngine('validate');
                        return isValid1 && isValid2 && isValid3;
                    }; 
                    //Botón Buscar
                    $('#ctl00_content1_btnBuscar').click(validacionBusqueda);
                  </script>";


                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(upchkFechas, upchkFechas.GetType(), "ValidacionFechas", script, false);
            }
        }

        #endregion
    }
}