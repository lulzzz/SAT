﻿using SAT_CL.ControlPatio;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SAT.ControlPatio
{
    public partial class ReporteCajones : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al 
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
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga de Indicadores
            inicializaIndicadoresEntidad();
            //Mostramos la consulta actual de cajones
            cargaEstatusCajonesActual();
            //Cargamos nuestra grafica linea de tiempo con la ocupacion de cajones por hora
            cargaEventosEntidadHora();
            //Cargamos la grafica de pay 
            cargaResumenTiempoOperacionEstacionado();
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Carga de Indicadores
            inicializaIndicadoresEntidad();
            //Mostramos la consulta actual de cajones
            cargaEstatusCajonesActual();
            //Cargamos nuestra grafica linea de tiempo con la ocupacion de cajones por hora
            cargaEventosEntidadHora();
            //Cargamos la grafica de pay 
            cargaResumenTiempoOperacionEstacionado();
        }
        /// <summary>
        /// Evento disparado al dar click en el link button para cerrar la ventana modal de imagenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.InicializaIndices(gvCajones);

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
        protected void ibEvidencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvCajones.DataKeys.Count > 0)
            {
                //Seleccionando la Fila
                TSDK.ASP.Controles.SeleccionaFila(gvCajones, sender, "lnk", false);

                //Validando si existen Evidencias
                if (Convert.ToInt32(gvCajones.SelectedDataKey["NoEvidencias"]) > 0)
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
                    ScriptManager.RegisterStartupScript(upgvCajones, upgvCajones.GetType(), "Imagenes", script, false);
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
        /// Evento disparado al daar click en los vinculos correspondientes a los indicadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkIndicadores_Click(object sender, EventArgs e)
        {
            //Obtenemos la referencia al control que genero el evento
            LinkButton objLink = (LinkButton)sender;

            //De acuerdo a su command name generamos la accion correspondiente
            switch (objLink.CommandName)
            {
                case "Disponibles":
                    {
                        //Cargamos la grafica de pay 
                        cargaResumenEstatusCajones();
                        break;
                    }
                case "Ocupados":
                    {

                        //Cargamos la grafica de pay
                        cargaResumenTiempoOperacionEstacionado();
                        break;
                    }

                case "TiempoPromedio":
                    {
                        //Cargamos la grafica de pay
                        cargaResumenEventosTiempo();

                        break;
                    }
                case "Utilizacion":
                    {
                        //Cargamos la grafica de pay con los tiempos de la entidades del día de hoy
                        cargaResumenTiempoEntidades();

                        break;
                    }
            }
            //Cargamos la linea de tiempo con unidades en cajon por hora
            cargaEventosEntidadHora();
            //Cargamos el grid con el detalle de andenes
            cargaEstatusCajonesActual();
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
            lblTiempo.Text = "";
            lblEntidad.Text = "";
            lblNoOperaciones.Text = "";
            lblTiempoPromedio.Text = "";
            lblUtilizacion.Text = "";

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
            inicializaIndicadoresEntidad();            
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

        #region Eventos GridView Andenes

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCajones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCajones, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCajones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCajones, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Enlazar el Origen de Datos con el Control GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCajones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Recuperando controles de interés
            Image img = (Image)e.Row.FindControl("imgEstatus");
            LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkEvidencias");

            //Validando que exista el Control
            if (img != null)
            {
                //Valida que exista el Indicador
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() != "")
                {
                    //Obteniendo Valor
                    int indicador = Convert.ToInt32(((DataRowView)e.Row.DataItem).Row.ItemArray[5]);

                    //Validando los Estatus de Operación
                    switch (((DataRowView)e.Row.DataItem).Row.ItemArray[2].ToString())
                    {
                        case "Ocupado":
                            {
                                /*Configurando Imagen*/
                                img.Visible = true;

                                //Validando el Indicador
                                if (indicador == 1)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoOK.png";
                                else if (indicador == 2)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoEX.png";
                                break;
                            }
                        case "Disponible":
                            {
                                //Ocultando Control
                                img.Visible = false;
                                img.ImageUrl = "";
                                break;
                            }
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
        /// Evento Producido al Cambiar el Tamaño de la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCajones, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvCajones.DataKeys.Count > 0)

                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id");
        }
        /// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvCajones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvCajones, sender, "lnk", false);

                //Obteniendo Eventos Terminados
                using (DataTable dtEventoTerminados = EventoDetalleAcceso.ObtieneEventosEntidadTerminadosDia(Convert.ToInt32(gvCajones.SelectedDataKey["Id"])))
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
                                    $('#contenidoBitacoraCajones').animate({ width: 'toggle' });
                                    $('#bitacoraCajones').animate({ width: 'toggle' });
                                  </script>";

                //Registrando Script
                ScriptManager.RegisterStartupScript(upgvCajones, upgvCajones.GetType(), "MuestraBitacora", script, false);
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
            TSDK.ASP.Controles.InicializaIndices(gvCajones);

            //Creando Script de ventana Modal
            string script = @"<script>
                                    $('#contenidoBitacoraCajones').animate({ width: 'toggle' });
                                    $('#bitacoraCajones').animate({ width: 'toggle' });
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
        private void inicializaPagina()
        {
            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializando GridViews
            TSDK.ASP.Controles.InicializaGridview(gvCajones);
            TSDK.ASP.Controles.InicializaGridview(gvPay);

            //Invocando Método de Carga de Indicadores
            inicializaIndicadoresEntidad();
            //Mostramos la consulta actual de cajones
            cargaEstatusCajonesActual();
            //Cargamos la grafica de pay 
            cargaResumenTiempoOperacionEstacionado();
            //Cargamos nuestra grafica linea de tiempo con la ocupacion de cajones por hora
            cargaEventosEntidadHora();
            //Cargamos el mapa
            cargaLayOutPatio();
        }
        /// <summary>
        /// Método Privado encargado de Cargar lo Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
            {
                //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", 0, "");

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
        /// Método Privado encargado de Cargar los Indicadores
        /// </summary>
        private void inicializaIndicadoresEntidad()
        {
            //Obteniendo Reporte
            using (DataTable dtIndicadores = EntidadPatio.RetornaIndicadoresEntidad(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan inidicadores
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtIndicadores))
                {
                    //Recorriendo cada Fila
                    foreach (DataRow dr in dtIndicadores.Rows)
                    {
                        //Asignando Valores
                        lblCajonesDisp.Text = dr["CajonesDisponibles"].ToString();
                        lblCajonesOcup.Text = dr["CajonesOcupados"].ToString();
                        lblUtilizacion.Text = dr["UtilizacionCajones"].ToString();
                        lblTiempoPromedio.Text = dr["TiempoPromedioCajones"].ToString();
                    }
                }               
            }
        }

        /// <summary>
        /// Método Privado encargado de Cargar la Tira de Imagenes
        /// </summary>
        private void cargaImagenesDetalle()
        {
            //Realizando la carga de URL de imagenes a mostrar
            using (DataTable mit = SAT_CL.ControlPatio.DetalleAccesoPatio.ObtieneImagenesUnidades(Convert.ToInt32(gvCajones.SelectedDataKey["id_detalle_acceso"])))
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
        /// Método Privado encargado de cargar el estatus actual de los andenes 
        /// </summary>
        private void cargaEstatusCajonesActual()
        {
            //Obteniendo Reporte
            using (DataTable dtAndenes = EntidadPatio.CargaEstatusEntidadesActuales(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Andenes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAndenes, true))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvCajones, dtAndenes, "Id-NoEvidencias-id_detalle_acceso", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAndenes, "Table");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvCajones);
                    //Quitando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        
        /// <summary>
        /// Metodo encargado de cargar la linea de tiempo con cantidad de eventos por dia
        /// </summary>
        private void cargaEventosEntidadHora()
        {
            //Obteniendo Estatus de Unidades
            using (DataSet ds = EntidadPatio.CargaOcupacionEntidadHora(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, true))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvLineaTiempo, ds.Tables[1], "", "", true, 1);

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables[0], "Table2");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };

                    //Invocando Método de Carga
                    TSDK.ASP.Controles.CargaGrafica(chtLineaTiempo, "Periodo", "No Eventos", SeriesChartType.Line, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").DefaultView,
                                         "rango", "cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombreLinea.Text = "Eventos Cajon por Hora";
                }
                else
                {
                    //Inicializando GridView

                    TSDK.ASP.Controles.InicializaGridview(gvLineaTiempo);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        
        /// <summary>
        /// Metodo encargado de cargar el resumen de entidades por estatus
        /// </summary>
        private void cargaResumenEstatusCajones()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dt = EntidadPatio.CargaResumenEstatusEntidades(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvPay, dt, "", "");

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtEntidades, "Descripcion", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "rango", "cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Estatus Cajones";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPay);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Metodo encargado de cargar la distribucion de eventos de acuerdo al tiempo de ejecucion
        /// </summary>
        private void cargaResumenEventosTiempo()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dt = EntidadPatio.CargaResumenEventosTiempo(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvPay, dt, "", "");

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtEntidades, "Descripcion", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "rango", "cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Tiempo Eventos Día";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPay);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Metodo encargado de realizar el resumen de tiempo ocupado y disponible
        /// </summary>
        private void cargaResumenTiempoEntidades()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dt = EntidadPatio.CargaResumenTiempoEntidad(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvPay, dt, "", "");

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtEntidades, "Descripcion", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "rango", "cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Tiempo Ocupado/Disponible Andenes";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPay);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Metodo encargado de cargar la dispersion de eventos de carga por tiempo de operacion
        /// </summary>
        /// <param name="tipo_evento"></param>
        private void cargaResumenTiempoOperacionEstacionado()
        {
            //Obteniendo Estatus de Unidades
            using (DataTable dt = EntidadPatio.CargaResumenEventosTiempo(EntidadPatio.TipoEntidad.Cajon, Convert.ToInt32(ddlPatio.SelectedValue), EntidadPatio.EstatusCarga.Estacionando))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvPay, dt, "", "");

                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtEntidades, "Descripcion", "Cantidad", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "rango", "cantidad", false, false, colores, " ");
                    //Actualizamos la etiqueta de la grafica
                    lblNombrePay.Text = "Eventos Carga Tiempos";
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPay);
                    //Eliminando Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        #endregion

        
    }
}