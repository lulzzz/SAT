using SAT_CL.ControlPatio;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SAT.ControlPatio.Reportes
{
    public partial class UnidadesDentro : System.Web.UI.Page
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
          
            //Validando si e Produjo un PostBack
            if (!(Page.IsPostBack))
                //Inicializando Página
                inicializaPagina();

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(uphplImagenZoom, uphplImagenZoom.GetType(), "jqzoom", @"$(document).ready(function() {$('.MYCLASS').jqzoom({ 
            zoomType: 'standard',  alwaysOn : false,  zoomWidth: 435,  zoomHeight:300,  position:'left',  
            xOffset: 125,  yOffset:0,  showEffect : 'fadein',  hideEffect: 'fadeout'  });});", true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   //Invocando Método de Busqueda
            cargaUnidadesDentro();
        }
        
        /// <summary>
        /// Evento disparado al dar click en el link actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBuscar_Click(object sender, EventArgs e)
        {
            //Actualizamos la consulta de unidades dentro
            cargaUnidadesDentro();
        }

        /// <summary>
        /// Evento disparado al dar click en cualquiera de los indicadores disponibles en el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkGrafica_Click(object sender, EventArgs e)
        {
            LinkButton objLink = (LinkButton)sender;
            switch (objLink.CommandName)
            {
                case ("UnidadesPatio"):
                    {
                        //Carga la grafica de pay con unidades por estatus
                        cargaEstatusUnidades();
                        //Carga la linea de tiempo con unidades en patio por hora
                        cargaUnidadesHora();                       
                        break;
                    }
                case ("TiempoPatio"):
                    {
                        //Carga la grafica de pay con unidades por tiempo de estancia
                        cargaTiempoUnidades();
                        //Carga la linea de tiempo con unidades en patio por hora
                        cargaUnidadesHora();
                        break;
                    }
                case ("EntradaSalida"):
                    {
                        //Carga la grafica de pay con las entradas y salidas realizadas
                        cargaEntradaSalidas();
                        //Carga la grafica de tiempo con las operaciones de entrada y salida por hora
                        cargaESHora();
                        break;
                    }
            }
            //Recargamos el grid principal
            cargaUnidadesDentro();
            //Recargamos los indicadores
            inicializaIndicadoresUnidad();
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
        /// <summary>
        /// Evento producido al dar click sobre una imagen de la tira de imagenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbThumbnailDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            //URL de imagen a mostrar en panel de zoom            
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }

        /// <summary>
        /// Metodo Encargado de Mostrar el mapa del patio actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkMapa_Click(object sender, EventArgs e)
        {
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                                $('#mapa_patio').animate({ width: 'toggle' });
                                                $('#visualizacion_mapa_patio').animate({ width: 'toggle' });
                                              </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(uplnkMapa, uplnkMapa.GetType(), "Mapa", script, false);
            //Limpiamos indicadores de entidad seleccionada
            lblUnidad.Text = "";
            lblTiempoOperacion.Text = "";
            lblEntidad.Text = "";
            lblNoOperaciones.Text = "";
            lblTiempoPromedioAnden.Text = "";
            lblUtilizacionAnden.Text = "";

        }
        /// <summary>
        /// Evento disparado al dar click en el mapa de patio actualizando su contenido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgLayout_Click(object sender, ImageClickEventArgs e)
        {

            //Invocando Carga de Layout
            cargaLayOutPatio();

            //Mostramos los datos de la entidad seleccionada
            inicializaIndicadoresEntidad(e.X, e.Y);
            //Inicializamos los indicadores            
            inicializaIndicadoresUnidad();
        }
        /// <summary>
        /// Carga el conjunto de layouts ligados a las zonas del patio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlZonaPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   //Invocando Carga de Layout
            cargaLayOutPatio();
        }
        /// <summary>
        /// Evento disparado al dar click al link button cerrar cuya funcion es la de cerrar la ventana modal que muestra el mapa del patio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                            $('#mapa_patio').animate({ width: 'toggle' });
                                            $('#visualizacion_mapa_patio').animate({ width: 'toggle' });
                                      </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(uplnkCerrar, uplnkCerrar.GetType(), "Mapa", script, false);

        }
        /// <summary>
        /// Método Privado encargado de Cargar el Layout de la Zona de Patio
        /// </summary>
        private void cargaLayOutPatio()
        {   //Obteniendo LayOut con Entidades
            Session["Dibujo"] = SAT_CL.ControlPatio.ZonaPatio.PintaLayOutZona(Convert.ToInt32(ddlZonaPatio.SelectedValue),
                                    Server.MapPath("~/Image/EntidadTiempoOKCarga.png"), Server.MapPath("~/Image/EntidadTiempoOKDescarga.png"),
                                    Server.MapPath("~/Image/EntidadTiempoOK.png"), Server.MapPath("~/Image/EntidadTiempoEXCarga.png"),
                                    Server.MapPath("~/Image/EntidadTiempoEXDescarga.png"), Server.MapPath("~/Image/EntidadTiempoEX.png"),
                                    20, 20);
            //Asignando URL a la Imagen
            imgLayout.ImageUrl = "~/WebHandlers/VisorImagen.ashx?q=" + Environment.TickCount.ToString();

        }
        
        /// <summary>
        /// Inicializamo los indicadores relacionados con una entidad dadas sus coordenadas
        /// </summary>
        public void inicializaIndicadoresEntidad(int x, int y)
        {
            //Inicializamos los indicadores de unidad en la pagina
            using (DataTable t = SAT_CL.ControlPatio.EntidadPatio.ObtieneIndicadoresPorEntidad(x, y))
            {
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                    //Si el origen es valido recorremos la tabla 
                    foreach (DataRow r in t.Rows)
                    {
                        lblEntidad.Text = r["Nombre"].ToString();
                        lblUnidad.Text = "Unidad: " + r["Unidad"].ToString();
                        lblTiempo.Text = r["Estatus"].ToString() + ", " + r["TiempoOperacion"].ToString();
                        lblNoOperaciones.Text = "Operaciones día: " + r["NoOperaciones"].ToString();
                        lblTiempoPromedioAnden.Text = "Tiempo Prom.: " + r["TiempoPromedio"].ToString();
                        lblUtilizacionAnden.Text = "% Utilización: " + r["Utilizacion"].ToString() + "%";
                    }
            }
        }

        #region Eventos GridView "Entidades"

        /// <summary>
        /// Metodo encargado de controlar el numero de registros a mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue));
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEntidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {   //Recuperando controles de interés
            Image img = (Image)e.Row.FindControl("imgEstatus");
            LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkEvidencias");
            //Validando que exista el Control
            if (img != null)
            {   //Validación de Datos
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() != "")
                {   //Instanciando Evento
                    using (EventoDetalleAcceso eda = new EventoDetalleAcceso(Convert.ToInt32(((DataRowView)e.Row.DataItem).Row.ItemArray[5])))
                    //Instanciando Tipo de Evento
                    using (TipoEvento te = new TipoEvento(eda.id_tipo_evento))
                    //Validando el Estatus del Tiempo
                    switch (((DataRowView)e.Row.DataItem).Row.ItemArray[6].ToString())
                    {
                        case "0"://Ninguno
                            {   //Quitando Imagen
                                img.ImageUrl = "";
                                img.Visible = false;
                                break;
                            }
                        case "1"://En Tiempo
                            {   //Carga
                                if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Carga)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoOKCarga.png";
                                //Descarga
                                else if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Descarga)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoOKDescarga.png";
                                //Estaciona
                                else if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Estaciona)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoOK.png";
                                //Visualizando Control
                                img.Visible = true;
                                break;
                            }
                        case "2"://Fuera de Tiempo
                            {   //Carga
                                if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Carga)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoEXCarga.png";
                                //Descarga
                                else if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Descarga)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoEXDescarga.png";
                                //Estaciona
                                else if (te.naturaleza_evento == TipoEvento.NaturalezaEvento.Estaciona)
                                    //Asignando Imagen Correspondiente
                                    img.ImageUrl = "~/Image/EntidadTiempoEX.png";
                                //Visualizando Control
                                img.Visible = true;
                                break;
                            }
                    }
                    //Instanciando Detalle de Acceso
                    using(DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(((DataRowView)e.Row.DataItem).Row.ItemArray[0])))
                    //Instanciando Acceso de Patio
                    using(AccesoPatio ap = new AccesoPatio(dap.id_acceso_entrada))    
                    //Instanciando Ubicación de Patio
                    using(UbicacionPatio up = new UbicacionPatio(Convert.ToInt32(ddlPatio.SelectedValue)))
                    {   //Validando que la Unidad no sobrepase el Tiempo en Patio
                        if (up.tiempo_limite > Convert.ToInt32((TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro() - ap.fecha_acceso).TotalMinutes))
                            //Asignando Color
                            e.Row.Cells[4].ForeColor = System.Drawing.Color.DarkGreen;
                        else//Asignando Color
                            e.Row.Cells[4].ForeColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
            //Validando que exista el Control
            if (lnkButton != null)
            {
                //Validación de Datos
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[9].ToString() != "0")
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
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id-IdEvt");
        }
        /// <summary>
        /// Evento Producido al Presionar el Link de Bitacora
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
                using(DataTable dtEventoTerminados = EventoDetalleAcceso.ObtieneEventosDetalleTerminados(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                {
                    //Validando que existan los Registros
                    if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventoTerminados))
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

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvEntidades);
            //Cargando Catalogos
            cargaCatalogos();           
            //Obtenemos el estatus actual del patio           
            cargaUnidadesDentro();
            //Carga Grafica de unidades por estatus
            cargaEstatusUnidades();
            //Carga la linea de tiempo con unidades por hora
            cargaUnidadesHora();
            //Cargamos los indicadores relacionados con unidad
            inicializaIndicadoresUnidad();
            //Cargamos el layout
            cargaLayOutPatio();

        }

        /// <summary>
        /// Inicializamo los indicadores relacionados con unidades dentro de patio
        /// </summary>
        public void inicializaIndicadoresUnidad()
        {
            //Inicializamos los indicadores de unidad en la pagina
            using (DataTable t = SAT_CL.ControlPatio.DetalleAccesoPatio.retornaIndicadoresUnidades(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                    //Si el origen es valido recorremos la tabla 
                    foreach (DataRow r in t.Rows)
                    {
                        lblUnidades.Text = r["Unidades"].ToString();
                        lblEntradaSalida.Text = r["EntradasSalidas"].ToString();                        
                        lblTiempo.Text = r["Tiempo"].ToString();
                        lblInfoMapa1.Text = r["Unidades"].ToString();

                        lblInfoMapa3.Text = r["Descargando"].ToString();
                        lblInfoMapa2.Text = r["Cargando"].ToString();
                        lblInfoMapa4.Text = r["CargadasxDescargar"].ToString();
                        lblInfoMapa5.Text = r["CargadasxSalir"].ToString();
                        lblInfoMapa6.Text = r["VaciasxCargar"].ToString();
                        lblInfoMapa7.Text = r["VaciasxSalir"].ToString();
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {   
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {   
                //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                
                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();
            }
            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoBit, "", 18);
            //Cargando Zonas de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlZonaPatio, 37, "", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
        }
        /// <summary>
        /// Método Privado encargado de cargar el grid view principal con las unidades dentro del patio
        /// </summary>
        private void cargaUnidadesDentro()
        {   
            //Obteniendo Reporte de Unidades con su Evento Actual
            using (DataTable dtUnidadesEvt = Reporte.ObtieneUnidadesEventoActual("", "", Convert.ToInt32(ddlPatio.SelectedValue)))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtUnidadesEvt))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEntidades, dtUnidadesEvt, "Id-IdEvt-NoEvidencias", "", true, 1);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesEvt, "Table");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEntidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }               
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Grid y Grafica con las Unidades por su Estatus
        /// </summary>
        private void cargaEstatusUnidades()
        {   
            //Obteniendo Estatus de Unidades
            using(DataTable dtEstatusUnidades = DetalleAccesoPatio.ObtieneUnidadesEstatus(Convert.ToInt32(ddlPatio.SelectedValue)))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEstatusUnidades))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEstatusUnidades, dtEstatusUnidades, "", "", true, 1);
                    
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtEstatusUnidades, "Table1");
                    
                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtUnidades, "Estatus", "No. Unidades", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Estatus", "NoUnidades", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Unidades por Estatus";
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusUnidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Metodo encargado de cargar la grafica con desglose de las unidades de acuerdo a su tiempo de estancia
        /// </summary>
        private void cargaTiempoUnidades()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dtTiempoUnidades = DetalleAccesoPatio.ObtieneUnidadesRangoEstancia(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTiempoUnidades))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEstatusUnidades, dtTiempoUnidades, "", "", true, 1);

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtTiempoUnidades, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtUnidades, "Tiempo", "No. Unidades", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Descripcion", "Valor", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Unidades por Tiempo de Estancia";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusUnidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Metodo encargado de cargar la cantidad de entradas y salidas y mostrarlas en la grafica correspondiente
        /// </summary>
        private void cargaEntradaSalidas()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dtES = DetalleAccesoPatio.ObtieneEntradasSalidas(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtES))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEstatusUnidades,dtES,"","");

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtES, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtUnidades, "Tipo", "No. Unidades", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Descripcion", "Valor", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Operaciones Entrada-Salida";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusUnidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Grid y las Unidades en su Periodo de Tiempo
        /// </summary>
        private void cargaUnidadesHora()
        {   
            //Obteniendo Estatus de Unidades
            using (DataSet dsPeriodoUnidades = DetalleAccesoPatio.ObtienePeriodoUnidades(Convert.ToInt32(ddlPatio.SelectedValue)))
            {   
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsPeriodoUnidades, true))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvLineaTiempo, dsPeriodoUnidades.Tables[1], "", "", true, 1);

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsPeriodoUnidades.Tables[0], "Table2");
                    
                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };

                    //Invocando Método de Carga
                    TSDK.ASP.Controles.CargaGrafica(chtLineaTiempo, "Periodo", "No. Unidades", SeriesChartType.Line, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").DefaultView,
                                         "Periodo", "NoUnidades", false, false, colores," ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombreLinea.Text = "Unidades en Patio por Hora";  
                }
                else
                {   
                    //Inicializando GridView
                    
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusUnidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        /// <summary>
        /// Carga la grafica de linea de tiempo con las entradas y salidas por hora
        /// </summary>
        private void cargaESHora()
        {
            //Obteniendo Estatus de Unidades
            using (DataSet dsESHora = DetalleAccesoPatio.ObtieneESHora(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsESHora, true))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvLineaTiempo, dsESHora.Tables[1], "", "", true, 1);

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsESHora.Tables[0], "Table2");
                    
                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };

                    //Invocando Método de Carga
                    TSDK.ASP.Controles.CargaGrafica(chtLineaTiempo, "Periodo", "Operacion", SeriesChartType.Line, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").DefaultView,
                                         "Rango", "Cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombreLinea.Text = "Operaciones Entrada-Salida por Hora";                    
                    
                }
                else
                {   //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusUnidades);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
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


        
    }
}