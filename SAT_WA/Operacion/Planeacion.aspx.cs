using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Xml.Linq;
using System.Globalization;

namespace SAT.Operacion
{
    public partial class Planeacion : System.Web.UI.Page
    {
        #region Atributos

        /// <summary>
        /// 
        /// </summary>
        private static string _nombre_pagina = "Planeacion";
        /// <summary>
        /// Contenedor de la Forma
        /// </summary>
        private string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recarga de página
            if (!Page.IsPostBack)
                //Inicializando contenido de forma
                inicializaForma();

            //Validando sesion única
            //validaSesionUnica();
        }
        /// <summary>
        /// Evento Producido al Dar Click en los Botones de Configuración
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfiguraSemana_Click(object sender, ImageClickEventArgs e)
        {
            //Obteniendo Control
            ImageButton img = (ImageButton)sender;

            //Obteniendo Semana Deseada
            int semana = Convert.ToInt32(lblSemana.Text);
            int semana_final = 0;

            //Validando Comando
            switch (img.CommandArgument)
            {
                case "Mas":
                    {
                        //Añadiendo una Semana
                        semana_final = semana + 1;
                        break;
                    }
                case "Menos":
                    {
                        //Restando una Semana
                        semana_final = semana - 1;
                        break;
                    }
            }

            //Obteniendo Semana Máxima
            //Declarando Variables Auxiliares
            System.Globalization.GregorianCalendar greCal = new GregorianCalendar();

            //Obteniendo Dia de la Semana
            int semana_maxima = greCal.GetWeekOfYear(new DateTime(Fecha.ObtieneFechaEstandarMexicoCentro().Year, 12, 31, 0, 0, 0), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            //Validando que no exceda el Limite Superior
            if (semana_final > semana_maxima)

                //Asignando Minimo
                semana_final = 1;
            //Validando que no exceda el Limite Inferior
            else if (semana_final < 1)

                //Asignando Maximo
                semana_final = semana_maxima;

            //Asignando Valor Requerido
            lblSemana.Text = semana_final.ToString();

            //Invocando Método de Carga-Descarga
            cargaParadasCargaDescargaPendientes();
        }
        /// <summary>
        /// Cambio se selección para uso de filtro de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se ha marcado el filtro
            if (chkRangoFechas.Checked)
            {
                //Habilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = true;

                //Colocando fechas predeterminadas (una semana)
                txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm");
                txtFechaInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            }
            //Si no se ha marcado
            else
            {
                //Deshabilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = false;

                //Limpiando fechas
                txtFechaInicio.Text = txtFechaFin.Text = "";
            }
        }
        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Determinando la pestaña pulsada
            switch (((Button)sender).CommandName)
            {
                case "Servicios":
                    //Cambiando estilos de pestañas
                    btnPestanaViajes.CssClass = "boton_pestana_activo";
                    btnPestanaUnidades.CssClass = "boton_pestana";
                    //Asignando vista activa de la forma
                    mtvPlaneacion.SetActiveView(vwServicios);
                    break;
                case "Unidades":
                    //Cambiando estilos de pestañas
                    btnPestanaViajes.CssClass = "boton_pestana";
                    btnPestanaUnidades.CssClass = "boton_pestana_activo";
                    //Asignando vista activa de la forma
                    mtvPlaneacion.SetActiveView(vwUnidades);
                    break;
            }
        }
        /// <summary>
        /// Clic en filtro de tipo de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbUnidad_CheckedChanged(object sender, EventArgs e)
        {
            //Limpiando contenido de gv de unidades
            Controles.InicializaGridview(gvUnidades);
            //Eliminando origen de datos
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }
        /// <summary>
        /// Click en botón de búsqueda de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarUnidades_Click(object sender, EventArgs e)
        {
            //Cargamos Unidades
            cargaUnidades();

        }
        /// <summary>
        /// Cambio de tamaño del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoUnidades.SelectedValue), true, 4);
        }
        /// <summary>
        /// Click en exportación de contenido de GV de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarUnidades_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "*".ToArray());
        }
        /// <summary>
        /// Click en algún Link de GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionUnidad_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Historial":
                        //Construyendo URL de ventana de historial de unidad
                        string url = Cadena.RutaRelativaAAbsoluta("~/Operacion/Planeacion.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + gvUnidades.SelectedDataKey["*IdUnidad"].ToString() + "&idRegistroB=1");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=600";
                        //Abriendo Nueva Ventana
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Historial de Unidades", configuracion, Page);
                        break;
                    case "UltimoMonitoreo":
                        //Cargando contenido de bitácora de monitoreo
                        wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                        //Apertura de ventana de historial de monitoreo
                        alternaVentanaModal("historialBitacora", lkb);
                        break;
                    case "Publicar":
                        //Abriendo ventana para la Publicación de la Unidad
                        alternaVentanaModal("publicacionUnidad", lkb);
                        //Inicializamos Control de Usuario
                        //wucPublicacionUnidad.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), 0);
                        break;
                    case " Respuesta(s) Por Ver":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuesta", lkb);
                        //cargamos respuesta
                        //cargaResultadoRespuesta(gvUnidades);
                        break;
                    case " Respuesta(s)":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuesta", lkb);
                        //cargamos respuesta
                        //cargaResultadoRespuesta(gvUnidades);
                        break;
                    case "Tomar Servicio":
                        //Asignamos Valor
                        lblValor.Text = "No";
                        //Copiamos Servicio;
                        //copiarServicioPUFinal(lkb, false, null);
                        break;
                    case "Ya Publicada":
                        //Abriendo ventana para la Publicación de la Unidad
                        alternaVentanaModal("publicacionUnidad", lkb);
                        //Inicializamos Control de Usuario
                        //wucPublicacionUnidad.InicializaControl(0, consumoObtieneIdPublicacionUnidad(gvUnidades));
                        break;
                }
            }
        }
        /// <summary>
        /// Cambio de página activa del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// Enlace a datos de cada fila del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    /**** APLICANDO COLOR DE FONDO DE FILA ACORDE A ESTATUS DE UNIDAD Y VENCIMIENTOS ****/
                    //Determinando estatus de unidad
                    switch ((Unidad.Estatus)row.Field<byte>("*IdEstatusUnidad"))
                    {
                        case Unidad.Estatus.ParadaDisponible:
                            //Si hay un vencimiento
                            if (row.Field<int>("*IdVencimiento") > 0)
                                //Cambiando color de fondo de la fila a rojo
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                            break;
                    }
                    //***RECUPERANDO CONTROLES PARA LA FUNCIONALIDAD DE PUBLICACIÓN DE UNIDADES***//
                    using (LinkButton lkbPublicacion = (LinkButton)e.Row.FindControl("lkbEstatusPublicacion"),
                                      lkbRespuestas = (LinkButton)e.Row.FindControl("lkbEstatusRespuestas"),
                                      lkbAceptarRespuesta = (LinkButton)e.Row.FindControl("lkbAceptarRespuesta"),
                                      lkbCopia = (LinkButton)e.Row.FindControl("lkbCopia"))
                    {
                        /*/Validando que el DataSet contenga las tablas
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4")))
                        {
                            //Declaramos variable
                            DataRow re = null;

                            //Obtenemos Publicación de la Unidad
                            re = (from DataRow r in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4").Rows
                                  where Convert.ToInt32(r["idUnidad"]) == row.Field<int>("*IdUnidad")
                                  select r).FirstOrDefault(); ;
                            //Validamos que exista elementos
                            if (re != null)
                            {
                                //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                                //Asignamos Existencia de Publicación
                                lkbPublicacion.CommandName = lkbPublicacion.Text = "Ya Publicada";
                                //Si no hay respuestas
                                if (re.Field<int>("respuestasTotales") != 0)
                                {
                                    //Validamos Respuestas por Ver
                                    if (re.Field<int>("respuestasPorVer") != 0)
                                    {
                                        //Asignamos Descripción
                                        lkbRespuestas.CommandName = " Respuesta(s) Por Ver";
                                        lkbRespuestas.Text = re.Field<int>("respuestasPorVer").ToString() + " Respuesta(s) Por Ver";
                                    }
                                    else
                                    {
                                        //Asignamos Descripción
                                        lkbRespuestas.CommandName = " Respuesta(s)";
                                        lkbRespuestas.Text = re.Field<int>("respuestasTotales").ToString() + " Respuesta(s)";
                                    }
                                }
                                else
                                {
                                    //Asignamos Descripción
                                    lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                                }

                                //Aceptación de Publicación
                                if (re.Field<int>("respuestasAceptadas") > 0 && re.Field<int>("idServicio") == 0)
                                {
                                    //Asignamos Descripción
                                    lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = "En Espera Datos del Viaje";
                                }
                                else
                                {
                                    //Asignamos Descripción
                                    lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                                }

                                //Documentacion de Publicación
                                if (re.Field<int>("idServicio") != 0)
                                {
                                    //Asignamos Descripción
                                    lkbCopia.CommandName = lkbCopia.Text = "Tomar Servicio";
                                }
                                else
                                {
                                    //Asignamos Descripción
                                    lkbCopia.CommandName = lkbCopia.Text = " ";
                                }
                            }
                            else
                            {
                                //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                                //Asignamos Existencia de Publicación
                                lkbPublicacion.CommandName = lkbPublicacion.Text = "Publicar";
                                lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                                lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                                lkbCopia.CommandName = lkbCopia.Text = " ";
                            }
                        }
                        else
                        {

                            //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                            //Asignamos Existencia de Publicación
                            lkbPublicacion.CommandName = lkbPublicacion.Text = "Publicar";
                            lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                            lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                            lkbCopia.CommandName = lkbCopia.Text = " ";
                        }//*/
                    }
                }
            }
        }

        /// <summary>
        /// Cambio de criterio de ordenamiento en GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenarUnidades.Text = Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 4);
        }
        /// <summary>
        /// Click en botón de busqueda de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarServicios_Click(object sender, EventArgs e)
        {
            //Cargando servicios
            cargaServicios();

            //Cargando resumen de pendientes por semana
            cargaParadasCargaDescargaPendientes();
        }
        /// <summary>
        /// Cambio de tamaño de página en GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 4);
        }
        /// <summary>
        /// Exportación de contenido de GV de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServicios_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "id_servicio-movimiento-id_parada_actual-IdParadaInicial");
        }
        /// <summary>
        /// Click en algún Link de GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionServicio_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Documentacion":
                        //Inicializando control de documentación
                        wucServicioDocumentacion.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), true, UserControls.wucServicioDocumentacion.VistaDocumentacion.Paradas);
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("documentacionServicio", lkb);
                        break;
                    case "Asignaciones":
                        //Determinando que ventana debe mostrarse
                        //Si el recurso no está asignado
                        if (lkb.Text == "Sin Asignar")
                        {
                            //Inicializando control de asignación de recurso
                            wucPreAsignacionRecurso.InicializaPreAsignacion(Convert.ToInt32(gvServicios.SelectedDataKey["movimiento"]));
                            //Mostrando ventana modal de asignaciones
                            alternaVentanaModal("asignacionRecursos", lkb);
                        }
                        //Si ya está asignado
                        else
                        {
                            //Inicializando ventana de recursos asignados al servicio
                            cargaRecursosAsignados();
                            //Mostrando ventana modal de asignaciones del servicio
                            alternaVentanaModal("unidadesAsignadas", lkb);
                        }
                        break;
                    case "Eventos":
                        //Instanciando servicio
                        using (Servicio srv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])))
                            //Inicializando control de eventos de parada
                            wucParadaEvento.InicializaControl(Convert.ToInt32(srv.estatus == Servicio.Estatus.Documentado ? gvServicios.SelectedDataKey["IdParadaInicial"] : gvServicios.SelectedDataKey["id_parada_actual"]));
                        //Indicando que argumento adicional para el cierre de la ventana eventos será ejecutado
                        lkbCerrarEventosParada.CommandArgument = "Eventos";
                        //Mostrando ventana de eventos
                        alternaVentanaModal("eventosParada", lkb);
                        break;
                    case "Referencia":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]));
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("referenciasServicio", lkb);
                        break;
                    case "Cancelar":
                        //Mostrando ventana de confirmación
                        alternaVentanaModal("confirmacionCancelacion", lkb);
                        //Limpiamos Control de Cancelación
                        txtMotivoCancelacion.Text = "";
                        break;
                    case "Publicar":
                        //Instanciamos Servicio
                        using (SAT_CL.Documentacion.Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])))
                        {
                            //Validamos Estatus del Servicio
                            if (objServicio.estatus == Servicio.Estatus.Documentado)
                            {
                                //Abriendo ventana para la Publicación de la Servicio
                                alternaVentanaModal("PublicacionServicio", lkb);
                                //Inicializamos Control de Usuario
                                //wucPublicacionServicio.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), 0);
                            }
                            else
                            {
                                //Mostramos Notificación Error
                                //Mostrando Mensaje de Operación
                                TSDK.ASP.ScriptServer.MuestraNotificacion(gvServicios, "El servicio debe estar Documentado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            }
                        }
                        break;
                    case " Respuesta(s) Por Ver":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuestaPS", lkb);
                        //cargamos respuesta
                        //cargaResultadoRespuestaPS(gvServicios);
                        break;
                    case " Respuesta(s)":
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuestaPS", lkb);
                        //cargamos respuesta
                        //cargaResultadoRespuestaPS(gvServicios);
                        break;
                    case "Ya Publicada":
                        //Abriendo ventana para la Publicación de la Unidad
                        alternaVentanaModal("PublicacionServicio", lkb);
                        //Inicializamos Control de Usuario
                        //wucPublicacionServicio.InicializaControl(0, consumoObtieneIdPublicacionServicio(gvServicios));
                        break;
                    case "Despachar":
                        {
                            //Crea variable que almacena el numero de servicio 
                            int noServicio = Convert.ToInt32(gvServicios.SelectedDataKey["NoServicio"]);
                            //Crea variable que almacena la ruta de apertura de Documentacion por servicio
                            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~Operacion/Planeacion.aspx", "~/Despacho.aspx?noServicio=" + noServicio);
                            Response.Redirect(urlDestino);


                            break;
                        }
                    case "OperacionAlcance":
                        //Instanciando servicio
                        using (Servicio serv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])))
                        {
                            if (serv.habilitar)
                            {
                                wucClasificacion.InicializaControl(1, serv.id_servicio, serv.id_compania_emisor);
                                alternaVentanaModal("Clasificacion", this.Page);
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Cambio de página activa edl GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// Enlace a datos de cada fila del GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Recuperando controles de funcionalidad
                    using (LinkButton lkbCancelar = (LinkButton)e.Row.FindControl("lkbCancelarServicio"))
                    {
                        /**** APLICANDO HABILITACIÓN DE CONTROL DE CANCELACIÓN ****/
                        //Instanciando servicio
                        using (Servicio srv = new Servicio(Convert.ToInt32(row.Field<int>("id_servicio"))))
                        {
                            //Si el estatus es documentado
                            if (srv.estatus == Servicio.Estatus.Documentado)
                                //Se habilita opción de cancelación
                                lkbCancelar.Visible = true;
                            else
                                lkbCancelar.Visible = false;
                        }
                    }

                    /**** APLICANDO SEMAFOREO EN BASE A ESTATUS DE CITAS DE CARGA Y DESCARGA ****/
                    using (Image imgEstatusDescarga = (Image)e.Row.FindControl("imgSemaforoDescarga"),
                        imgEstatusCarga = (Image)e.Row.FindControl("imgSemaforoCarga"))
                    {
                        //Aplicando criterio de visibilidad
                        imgEstatusCarga.Visible =
                        imgEstatusDescarga.Visible = true;

                        //Dependiendo el estatus de carga
                        switch (row["SemaforoCitaCarga"].ToString())
                        {
                            case "Verde":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_verde.png";
                                imgEstatusCarga.ToolTip = "Cita en Tiempo";
                                break;
                            case "Amarillo":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_naranja.png";
                                imgEstatusCarga.ToolTip = "Retrazo en Cita";
                                break;
                            case "Rojo":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_rojo.png";
                                imgEstatusCarga.ToolTip = "Cita por Vencer";
                                break;
                            case "OK":
                            case "":
                                imgEstatusCarga.ImageUrl = "~/Image/Entrada.png";
                                imgEstatusCarga.ToolTip = "Llegada en Tiempo";
                                break;
                            case "TACHE":
                                imgEstatusCarga.ImageUrl = "~/Image/Salida.png";
                                imgEstatusCarga.ToolTip = "Llegada Tarde";
                                break;
                            default:
                                imgEstatusCarga.ImageUrl = "";
                                imgEstatusCarga.ToolTip = "";
                                imgEstatusCarga.Visible = false;
                                break;
                        }

                        //Dependiendo el estatus de descarga
                        switch (row["SemaforoCitaDescarga"].ToString())
                        {
                            case "Verde":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_verde.png";
                                imgEstatusDescarga.ToolTip = "Cita en Tiempo";
                                break;
                            case "Amarillo":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_naranja.png";
                                imgEstatusDescarga.ToolTip = "Retrazo en Cita";
                                break;
                            case "Rojo":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_rojo.png";
                                imgEstatusDescarga.ToolTip = "Cita por Vencer";
                                break;
                            case "OK":
                            case "":
                                imgEstatusDescarga.ImageUrl = "~/Image/Entrada.png";
                                imgEstatusDescarga.ToolTip = "Llegada en Tiempo";
                                break;
                            case "TACHE":
                                imgEstatusDescarga.ImageUrl = "~/Image/Salida.png";
                                imgEstatusDescarga.ToolTip = "Llegada Tarde";
                                break;
                            default:
                                imgEstatusDescarga.ImageUrl = "";
                                imgEstatusDescarga.ToolTip = "";
                                imgEstatusDescarga.Visible = false;
                                break;
                        }
                    }
                    //***RECUPERANDO CONTROLES PARA LA FUNCIONALIDAD DE PUBLICACIÓN DE SERVICIOS***//
                    /*/
                    using (LinkButton lkbPublicacion = (LinkButton)e.Row.FindControl("lkbEstatusPublicacion"),
                                      lkbRespuestas = (LinkButton)e.Row.FindControl("lkbEstatusRespuestas"),
                                      lkbAceptarRespuesta = (LinkButton)e.Row.FindControl("lkbAceptarRespuesta"))
                    {
                        //Validando que el DataSet contenga las tablas
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7")))
                        {
                            //Declaramos variable
                            DataRow re = null;

                            //Obtenemos Publicación de la Unidad
                            re = (from DataRow r in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7").Rows
                                  where Convert.ToInt32(r["idServicioOrigen"]) == row.Field<int>("id_servicio")
                                  select r).FirstOrDefault(); ;
                            //Validamos que exista elementos
                            if (re != null)
                            {
                                //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                                //Asignamos Existencia de Publicación
                                if (re.Field<string>("EstatusServicio") == "Vendido" || re.Field<string>("EstatusServicio") == "Publicado")
                                {
                                    lkbPublicacion.CommandName = lkbPublicacion.Text = "Ya Publicada";
                                }


                                //Si no hay respuestas
                                if (re.Field<int>("respuestasTotales") != 0)
                                {
                                    //Validamos Respuestas por Ver
                                    if (re.Field<int>("respuestasPorVer") != 0)
                                    {
                                        //Asignamos Descripción
                                        lkbRespuestas.CommandName = " Respuesta(s) Por Ver";
                                        lkbRespuestas.Text = re.Field<int>("respuestasPorVer").ToString() + " Respuesta(s) Por Ver";
                                    }
                                    else
                                    {
                                        //Asignamos Descripción
                                        lkbRespuestas.CommandName = " Respuesta(s)";
                                        lkbRespuestas.Text = re.Field<int>("respuestasTotales").ToString() + " Respuesta(s)";
                                    }
                                }
                                else
                                {
                                    //Asignamos Descripción
                                    lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                                }

                                //Aceptación de Publicación
                                if (re.Field<int>("respuestasAceptadas") > 0 && re.Field<int>("idServicio") == 0)
                                {
                                    //Asignamos Descripción
                                    lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = "Por Confirmar";
                                }
                                else
                                {
                                    //Asignamos Descripción
                                    lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                                }

                            }
                            else
                            {
                                //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                                //Asignamos Existencia de Publicación
                                lkbPublicacion.CommandName = lkbPublicacion.Text = "Publicar";
                                lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                                lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                            }
                        }
                        else
                        {

                            //*APLICAMOS ESTATUS DE PUBLICACIONES DE ACUERDO A LA FILA DEVUELTA
                            //Asignamos Existencia de Publicación
                            lkbPublicacion.CommandName = lkbPublicacion.Text = "Publicar";
                            lkbRespuestas.CommandName = lkbRespuestas.Text = "No hay Respuesta(s)";
                            lkbAceptarRespuesta.CommandName = lkbAceptarRespuesta.Text = " ";
                        }
                    }//*/
                    //Habilita el link de Documentar
                    LinkButton lkbTransportista = (LinkButton)e.Row.FindControl("lkbTransportista"),
                               lkbDespachar = (LinkButton)e.Row.FindControl("lkbDespachar");
                    if (lkbTransportista.Text == "Sin Asignar")
                    {
                        lkbDespachar.Visible = false;
                    }
                    else
                        lkbDespachar.Visible = true;

                }
            }
        }
        /// <summary>
        /// Cambio de Criterio de orden de GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoServicios.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 4);
        }
        /// <summary>
        /// Click en botón nuevo servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNuevoServicio_Click(object sender, EventArgs e)
        {
            //Mostrando ventana modal de selección de nuevo servicio (copia/documentación)
            alternaVentanaModal("seleccionServicioMovimiento", lkbNuevoServicio);
        }
        /// <summary>
        /// Maneja el evento disparado por el botón guardar del control de usuario Clasificación
        /// </summary>
        protected void wucClasificacion_ClickGuardar(object sender, EventArgs e)
        {
            //Guardando contenido de control
            wucClasificacion.GuardaCambiosClasificacionServicio();
            alternaVentanaModal("Clasificacion", this.Page);
            //Guardando selección actual de Id de Servicio
            string id_servicio = gvServicios.SelectedDataKey["id_servicio"].ToString();
            //Cargando servicios
            cargaServicios();
            //Aplicando selección Previa
            if (id_servicio != "")
                Controles.MarcaFila(gvServicios, id_servicio, "id_servicio", "id_servicio-movimiento-id_parada_actual-IdParadaInicial-NoServicio", (DataSet)Session["DS"], "Table", lblOrdenadoServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 3);

        }
        /// <summary>
        /// Maneja el evento disparado por el botón cancelar del control de usuario Clasificación
        /// </summary>
        protected void wucClasificacion_ClickCancelar(object sender, EventArgs e)
        {
            wucClasificacion.CancelaCambiosClasificacion();
            alternaVentanaModal("Clasificacion", this.Page);
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializando contenido de Controles
        /// </summary>
        private void inicializaForma()
        {
            //Asignando Nombre de Sesión Única
            //asignaSesionUnica();
            //Inicializando catálogos requeridos
            cargaCatalogos();
            //Inicializando Semana Actual
            obtieneSemanaActual();
            //Inicializando pestaña de viajes
            inicializaPestanaServicios();
            //Inicializando pestaña de unidades
            inicializaPestanaUnidades();
            //Carga Catalogos para Publicación
            cargaCatalogosPublicacion();
        }
        /// <summary>
        /// Método encargado de Asignar el nombre de la Sesion
        /// </summary>
        private void asignaSesionUnica()
        {
            if (!string.IsNullOrEmpty(_nombre_pagina))
                Session["FormaWeb"] = _nombre_pagina;
            else
                Session["FormaWeb"] = "";
        }
        /// <summary>
        /// Método encargado de Validar que el sistema se encuentre en una sola ventana
        /// </summary>
        private void validaSesionUnica()
        {
            //Validando Página
            if (Session["FormaWeb"] != null)
            {
                if (!string.IsNullOrEmpty(Session["FormaWeb"].ToString()))
                {
                    if (!(_nombre_pagina.Equals(Session["FormaWeb"].ToString())))
                    {
                        string script = @"alert('" + _nombre_pagina + @"'); window.top.close();";
                        //Registrando Script
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ValidaSesionUnica", script, true);
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Obtener la Semana Actual
        /// </summary>
        private void obtieneSemanaActual()
        {
            //Declarando Variables Auxiliares
            System.Globalization.GregorianCalendar greCal = new GregorianCalendar();

            //Obteniendo Dia de la Semana
            lblSemana.Text = greCal.GetWeekOfYear(Fecha.ObtieneFechaEstandarMexicoCentro(), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString();
        }
        /// <summary>
        /// Configura el contenido inicial de la pestaña de servicios
        /// </summary>
        private void inicializaPestanaServicios()
        {
            //Limpiando controles
            txtCliente.Text =
            txtNoServicio.Text = "";
            //Inicializando contenido de GV con carga de unidades
            cargaServicios();
            //Carga de resumen
            cargaParadasCargaDescargaPendientes();
        }
        /// <summary>
        /// Configura el contenido inicial de la pestaña de unidades
        /// </summary>
        private void inicializaPestanaUnidades()
        {
            //Limpiando controles
            txtNoUnidad.Text =
            txtUbicacion.Text = "";
            //Inicializando contenido de GV con carga de unidades
            cargaUnidades();
        }
        /// <summary>
        /// Carga de catálogos requeridos
        /// </summary>
        private void cargaCatalogos()
        {
            /*** PESTAÑA DE VIAJES ACTIVOS  ***/
            int idCompania = ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServicios, "", 26);
            //Catálogo de tamaño de GV de Resumen por Cliente
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoResumenCliente, "", 26);

            /*** PESTAÑA DE UNIDADES  ***/
            //Cargando catálogo de Estatus de unidades
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(lbxEstatus, "Todos", 53);
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoUnidades, "", 26);
            //Cargando catálogo de Flotas por Usuario
            DataTable dtCatalogo = CapaNegocio.m_capaNegocio.CargaCatalogo(193, "", ((UsuarioSesion)Session["usuario_sesion"]).id_usuario, "", idCompania, "");
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCatalogo))
            {
                Controles.CargaListBox(lbxFlota, dtCatalogo, "id", "descripcion");
            }
            else
                Controles.InicializaListBox(lbxFlota, "NINGUNO");
            lbxFlota.SelectedIndex = 0;

            //Catalogos de Clasificación
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxTerminal, 9, "TODOS", idCompania, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxOperacionServicio, 3, "TODOS", idCompania, "", 6, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxAlcance, 3, "TODOS", idCompania, "", 5, "");
        }
        /// <summary>
        /// Realiza la búsqueda de unidades y su asignación a servicio o movimiento actual
        /// </summary>
        private void cargaUnidades()
        {
            //Cargamos Publicaciones Activas de Nuestras Unidades
            //cargaPublicacionesActivasUnidades();

            //Declarando parámetro para identificar el tipo de unidades a mostrar como principales
            string tipo_unidad = "";

            //Determinando el tipo de unidades a mostrar
            //Si se solicita unidades motrices
            if (rdbUnidadMotriz.Checked)
                tipo_unidad = "Motriz";
            //Si se solicita unidades de arrastre
            if (rdbUnidadArrastre.Checked)
                tipo_unidad = "Arrastre";

            //Obteniendo estatus de pago
            string id_estatus = Controles.RegresaSelectedValuesListBox(lbxEstatus, "{0},", true, false);

            string id_flota = Controles.RegresaSelectedValuesListBox(lbxFlota, "{0},", true, false);

            //Cargando Unidades
            using (DataTable mit = Reportes.CargaDespachoSimplificadoUnidades(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoUnidad.Text,
                                        id_estatus.Length > 1 ? id_estatus.Substring(0, id_estatus.Length - 1) : "", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1)), 
                                        tipo_unidad, chkUnidadesPropias.Checked, chkUnidadesNoPropias.Checked, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteUnidad.Text, "ID:", 1)), id_flota.Length > 1 ? id_flota.Substring(0, id_flota.Length - 1) : ""))
            {

                //Llenando gridview
                Controles.CargaGridView(gvUnidades, mit, "*IdUnidad-*IdServicio-Movimiento-*IdParada-*IdEventoActual", lblOrdenarUnidades.Text, true, 4);
                //Guardando en sesión el origen de datos
                if (mit != null)
                {

                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
                }
                else
                {
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }

            //Quitando selecciones de fila existentes
            Controles.InicializaIndices(gvUnidades);
        }
        /// <summary>
        /// Carga de catálogos requeridos
        /// </summary>
        private void cargaCatalogosPublicacion()
        {
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddTamanoResultadoRespuestaPS, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRespuestasPS, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRespuestas, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoResultadoRespuesta, "", 26);
        }
        /// <summary>
        /// Realiza la carga de las unidades manteniendo una selección de registro previa
        /// </summary>
        private void cargaUnidadesManteniendoSeleccion()
        {
            //Obteniendo el registro seleccionado actualmente
            string id_registro_seleccion = gvUnidades.SelectedIndex > -1 ? gvUnidades.SelectedDataKey["*IdUnidad"].ToString() : "";
            //Cargando Gridview
            cargaUnidades();
            //Restableciendo selección en caso de ser necesario
            if (id_registro_seleccion != "")
                Controles.MarcaFila(gvUnidades, id_registro_seleccion, "*IdUnidad", "*IdUnidad-*IdServicio-Movimiento-*IdParada-*IdEventoActual", (DataSet)Session["DS"], "Table1", lblOrdenarUnidades.Text, Convert.ToInt32(ddlTamanoUnidades.SelectedValue), true, 4);
        }
        /// <summary>
        /// Realiza la búsqueda y muestra los servicios activos
        /// </summary>
        private void cargaServicios()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_cita_carga = DateTime.MinValue, fec_fin_cita_carga = DateTime.MinValue,
                     fec_ini_cita_descarga = DateTime.MinValue, fec_fin_cita_descarga = DateTime.MinValue;

            //Validando si se Requieren las Fechas
            if (chkRangoFechas.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rdbCitaCarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_cita_carga);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_cita_carga);
                }
                else if (rdbCitaDescarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFechaInicio.Text, out fec_ini_cita_descarga);
                    DateTime.TryParse(txtFechaFin.Text, out fec_fin_cita_descarga);
                }
            }

            //Obteniendo Filtros de Clasificación
            string operacion = Controles.RegresaSelectedValuesListBox(lbxOperacionServicio, "{0},", true, false);
            string alcance = Controles.RegresaSelectedValuesListBox(lbxAlcance, "{0},", true, false);
            string terminal = Controles.RegresaSelectedValuesListBox(lbxTerminal, "{0},", true, false);

            //Cargando Unidades
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaPlaneacionServicios(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), txtNoServicio.Text, chkDocumentados.Checked, chkIniciados.Checked,
                operacion.Length > 1 ? operacion.Substring(0, operacion.Length - 1) : "", terminal.Length > 1 ? terminal.Substring(0, terminal.Length - 1) : "",
                alcance.Length > 1 ? alcance.Substring(0, alcance.Length - 1) : "", fec_ini_cita_carga, fec_fin_cita_carga, fec_ini_cita_descarga, fec_fin_cita_descarga,
                Cadena.RegresaCadenaSeparada(txtNoUnidadA.Text, "ID:", 1).ToString(), Cadena.RegresaCadenaSeparada(txtNoOperador.Text, "ID:", 1).ToString()  ))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Obtenemos Arreglos de Id
                    int[] id = (from DataRow r in mit.Rows
                                select r.Field<int>("id_servicio")).ToArray();

                    //Cargamos Publicaciones Activas de Nuestros Servicios
                    //cargaPublicacionesActivasServicios(id);
                }

                Controles.CargaGridView(gvServicios, mit, "id_servicio-movimiento-id_parada_actual-IdParadaInicial-NoServicio", lblOrdenadoServicios.Text, true, 4);
                //Guardando en sesión el origen de datos
                if (mit != null)
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                else
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Quitando selecciones de fila existentes
            Controles.InicializaIndices(gvServicios);
        }


        /// <summary>
        /// Realiza la búsqueda y carga el resumen por semana de las paradas con pendientes de carga / descarga
        /// </summary>
        private void cargaParadasCargaDescargaPendientes()
        {
            //Declarando Variables Auxiliares
            System.Globalization.GregorianCalendar calendario_actual = new GregorianCalendar();
            DateTime fecha_calculada = DateTime.MinValue;
            DateTime fecha_inicio = DateTime.MinValue, fecha_fin = DateTime.MinValue;
            int semana_diferencial = 0;
            double dia_diferencial = 0;

            //Obteniendo diferencial de la Semana Deseada y la Semana Actual
            semana_diferencial = Convert.ToInt32(lblSemana.Text) - calendario_actual.GetWeekOfYear(Fecha.ObtieneFechaEstandarMexicoCentro(), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            //Añadiendo o Restando Dias dependiendo del Diferencial
            fecha_calculada = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(semana_diferencial * 7);

            //Si es Domingo
            if (fecha_calculada.DayOfWeek == DayOfWeek.Sunday)

                //Asignando Ultimo Dia de la Semana
                dia_diferencial = 7;
            else
                //Asignando Dia Actual
                dia_diferencial = Convert.ToDouble(fecha_calculada.DayOfWeek);

            //Obteniendo Fecha de Inicio
            fecha_inicio = fecha_calculada.Date.AddDays(1 - dia_diferencial);
            //Obteniendo Fecha de Fin
            fecha_fin = fecha_calculada.Date.AddDays(7 - dia_diferencial);

            //Obteniendo eventos de carga descarga de interés (sobre selección de vista en radiobuttons)
            using (DataSet ds = SAT_CL.Despacho.Reporte.ObtieneParadasCargaDescargaSemanaActual(fecha_inicio, fecha_fin, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, rdbResumenPendientes.Checked))
            {
                //Llenando gridview
                Controles.CargaGridView(gvResumenSemanal, ds, "Table", "Evento", "");
                Controles.CargaGridView(gvResumenCliente, ds, "Table1", "Cliente-Ubicacion-Evento", "");

                //Validando origen de datos, si hay datos
                if (Validacion.ValidaOrigenDatos(ds, "Table", "Table1"))
                {
                    //Añadiendo tablas a sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table2");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table3");
                }
                //Si no hay datos
                else
                {
                    //Borrado existentes de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }
            }

            //Cargando totales
            calculaTotalesPieResumen();
            calculaTotalesPieResumenCliente();
        }
        /// <summary>
        /// Calcula los totales del pie de GV de resumen
        /// </summary>
        private void calculaTotalesPieResumen()
        {
            using (DataTable dt = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"))
            {
                //Si el origen de datos fue recuperado
                if (dt != null)
                {
                    gvResumenSemanal.FooterRow.Cells[1].Text = dt.Compute("SUM(Lunes)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[2].Text = dt.Compute("SUM(Martes)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[3].Text = dt.Compute("SUM(Miercoles)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[4].Text = dt.Compute("SUM(Jueves)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[5].Text = dt.Compute("SUM(Viernes)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[6].Text = dt.Compute("SUM(Sabado)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[7].Text = dt.Compute("SUM(Domingo)", "").ToString();
                    gvResumenSemanal.FooterRow.Cells[8].Text = dt.Compute("SUM(Total)", "").ToString();
                }
            }
        }
        /// <summary>
        /// Calcula los totales del pie de GV de resumen
        /// </summary>
        private void calculaTotalesPieResumenCliente()
        {
            using (DataTable dt = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"))
            {
                //Si el origen de datos fue recuperado
                if (dt != null)
                {
                    gvResumenCliente.FooterRow.Cells[3].Text = dt.Compute("SUM(Lunes)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[4].Text = dt.Compute("SUM(Martes)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[5].Text = dt.Compute("SUM(Miercoles)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[6].Text = dt.Compute("SUM(Jueves)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[7].Text = dt.Compute("SUM(Viernes)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[8].Text = dt.Compute("SUM(Sabado)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[9].Text = dt.Compute("SUM(Domingo)", "").ToString();
                    gvResumenCliente.FooterRow.Cells[10].Text = dt.Compute("SUM(Total)", "").ToString();
                }
            }
        }
        /// <summary>
        /// Realiza la búsqueda de viajes activos y recarga el GV de los mismos manteniendo la selección del registro actual
        /// </summary>
        private void cargaServiciosManteniendoSeleccion()
        {
            //Obteniendo el registro seleccionado actualmente
            string id_registro_seleccion = gvServicios.SelectedIndex > -1 ? gvServicios.SelectedDataKey["id_servicio"].ToString() : "";
            //Cargando Gridview
            cargaServicios();
            //Restableciendo selección en caso de ser necesario
            if (id_registro_seleccion != "")
                Controles.MarcaFila(gvServicios, id_registro_seleccion, "id_servicio", "id_servicio-movimiento-id_parada_actual-IdParadaInicial-NoServicio", (DataSet)Session["DS"], "Table", lblOrdenadoServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 4);

            //Carga de resumen
            cargaParadasCargaDescargaPendientes();
        }


        #endregion

        #region Ventanas Modales

        /// <summary>
        /// Click en botones de cierre de ventanas modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {
                case "HistorialBitacora":
                    //Cerrando ventana modal 
                    alternaVentanaModal("historialBitacora", lkbCerrar);
                    break;
                case "Bitacora":
                    //Cerrando ventana modal 
                    alternaVentanaModal("bitacoraMonitoreo", lkbCerrar);
                    //Mostrando Historial
                    alternaVentanaModal("historialBitacora", lkbCerrar);
                    break;
                case "HistorialVencimientos":
                    //Cerrando ventana de historial
                    alternaVentanaModal("historialVencimientos", lkbCerrar);
                    //Determinando si es necesario mostrar alguna ventana diciional
                    switch (lkbCerrar.CommandArgument)
                    {
                        case "NotificacionVencimientos":
                            //Abriendo ventana de notificación de vencimientos
                            alternaVentanaModal("indicadorVencimientos", lkbCerrar);
                            break;
                        default:
                            cargaServiciosManteniendoSeleccion();
                            break;
                    }
                    break;
                case "SeleccionServMov":
                    //Cerrando ventana modal 
                    alternaVentanaModal("seleccionServicioMovimiento", lkbCerrar);
                    break;
                case "AsignacionRecursos":
                    //Cerrando ventana modal 
                    alternaVentanaModal("asignacionRecursos", lkbCerrar);
                    break;
                case "CopiaServicio":
                    //Cerrando ventana modal 
                    alternaVentanaModal("copiaServicioMaestro", lkbCerrar);
                    break;
                case "EventosParada":
                    //Cerrando ventana de actualziación de eventos
                    alternaVentanaModal("eventosParada", lkbCerrar);
                    //En base al argumento opcional del control, se determina si es requerida la apertura de la ventana de salida
                    if (lkbCerrar.CommandArgument == "Salida")
                        //Abriendo ventana de salida
                        alternaVentanaModal("inicioFinMovimientoServicio", lkbCerrar);
                    //Si es la ventana de documentación
                    else if (lkbCerrar.CommandArgument == "CitasEventos")
                        //Abriendo control de documentación
                        alternaVentanaModal("documentacionServicio", lkbCerrar);
                    break;
                case "ReferenciasServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("referenciasServicio", lkbCerrar);
                    break;
                case "Documentacion":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("documentacionServicio", lkbCerrar);
                    break;
                case "UnidadesAsignadas":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("unidadesAsignadas", lkbCerrar);
                    break;
                case "ResumenCliente":
                    //Mostrando ventana modal del resumen por cliente
                    alternaVentanaModal("resumenPorCliente", lkbCerrar);
                    break;
                case "CerrarPublicacion":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("PublicacionServicio", lkbCerrar);
                    break;
                case "CerrarPublicacionUnidad":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("publicacionUnidad", lkbCerrar);
                    break;
                case "CerrarResultadoRespuesta":
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuesta", lkbCerrar);
                    //Carga Unidades
                    cargaUnidades();
                    break;
                case "SeleccionRespuestas":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("opcionSeleccionRespuesta", lkbCerrar);
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuesta", lkbCerrar);
                    //Cargamos Resultados
                    //cargaResultadoRespuesta(lkbCerrar);

                    break;
                case "AceptarRespuesta":
                    // Abriendo ventana 
                    alternaVentanaModal("aceptarRespuesta", lkbCerrar);
                    break;
                case "CerrarElemento":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("elemento", lkbCerrar);
                    break;
                case "RechazarRespuesta":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("rechazarRespuesta", lkbCerrar);
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuesta", lkbCerrar);
                    //Cargamos Resultados
                    //cargaResultadoRespuesta(lkbCerrar);
                    break;
                case "CerrarResultadoRespuestaPS":
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuestaPS", lkbCerrar);
                    //Cargamos Servicios
                    cargaServicios();
                    break;
                case "SeleccionRespuestasPS":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("opcionSeleccionRespuestaPS", lkbCerrar);
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("resultadoRespuestaPS", lkbCerrar);
                    //Cargamos Resultados
                    //cargaResultadoRespuestaPS(lkbCerrar);
                    break;
                case "CerrarTercero":
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("tercero", lkbCerrar);
                    break;
                case "Clasificacion":
                    alternaVentanaModal("Clasificacion", lkbCerrar);
                    break;
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
                case "historialBitacora":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialBitacoraModal", "historialBitacora");
                    break;
                case "bitacoraMonitoreo":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "bitacoraMonitoreoModal", "bitacoraMonitoreo");
                    break;
                case "historialVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialVencimientosModal", "historialVencimientos");
                    break;
                case "seleccionServicioMovimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionServicioMovimientoModal", "seleccionServicioMovimiento");
                    break;
                case "asignacionRecursos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "asignacionRecursosModal", "asignacionRecursos");
                    break;
                case "copiaServicioMaestro":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "copiaServicioMaestroModal", "copiaServicioMaestro");
                    break;
                case "eventosParada":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "eventosParadaModal", "eventosParada");
                    break;
                case "referenciasServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "referenciasServicioModal", "referenciasServicio");
                    break;
                case "documentacionServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "documentacionServicioModal", "documentacionServicio");
                    break;
                case "confirmacionCancelacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionCancelacionModal", "confirmacionCancelacion");
                    break;
                case "unidadesAsignadas":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "unidadesAsignadasModal", "unidadesAsignadas");
                    break;
                case "confirmacionQuitarRecursos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionQuitarRecursosModal", "confirmacionQuitarRecursos");
                    break;
                case "resumenPorCliente":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "resumenPorClienteModal", "resumenPorCliente");
                    break;
                case "PublicacionServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalPublicar", "confirmacionPublicar");
                    break;
                case "publicacionUnidad":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalPublicarUnidad", "confirmacionPublicarUnidad");
                    break;
                case "resultadoRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorResultadoRespuesta", "ventanaResultadoRespuesta");
                    break;
                case "opcionSeleccionRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionRespuestas", "seleccionrespuestas");
                    break;
                case "aceptarRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoAceptarRespuesta", "confirmacionAceptarRespuesta");
                    break;
                case "elemento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoElemento", "confirmacionElemento");
                    break;
                case "rechazarRespuesta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionRechazarRespuesta", "confirmacionRechazarRespuesta");
                    break;
                case "resultadoRespuestaPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorResultadoRespuestaPS", "ventanaResultadoRespuestaPS");
                    break;
                case "opcionSeleccionRespuestaPS":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionRespuestasPS", "contenedorSeleccionRespuestasPS");
                    break;
                case "tercero":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoTercero", "confirmacionTercero");
                    break;
                case "Clasificacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorClasificacion", "ventanaClasificacion");
                    break;

            }
        }

        #region Bitácora Monitoreo

        /// <summary>
        /// Nuevo asiento enbitácora de monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoHistorial_btnNuevoBitacora(object sender, EventArgs e)
        {
            //Cerrando ventana de historial de bitácora
            alternaVentanaModal("historialBitacora", this);
            //Inicializando control para nueva bitácora
            wucBitacoraMonitoreo.InicializaControl(0, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]),//*-*
                    Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]), Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEventoActual"]),
                    Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]), 19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
            //Mostrando ventana de control bitácora
            alternaVentanaModal("bitacoraMonitoreo", this);
        }
        /// <summary>
        /// Consulta de bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoHistorial_lkbConsultar(object sender, EventArgs e)
        {
            //Cerrando ventana de historial de bitácora
            alternaVentanaModal("historialBitacora", this);
            //Inicializando control para nueva bitácora
            wucBitacoraMonitoreo.InicializaControl(wucBitacoraMonitoreoHistorial.id_bitacora_monitoreo, 0, 0, 0, 0, 0, 0);
            //Mostrando ventana de control bitácora
            alternaVentanaModal("bitacoraMonitoreo", this);
        }
        /// <summary>
        /// Guardar cambio en bitácora de monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreo_ClickRegistrar(object sender, EventArgs e)
        {
            //Guardando bitácora
            RetornoOperacion resultado = wucBitacoraMonitoreo.RegistraBitacoraMonitoreo();

            //Si hay actualización
            if (resultado.OperacionExitosa)
            {
                //Recargando contenido de bitácora de monitoreo
                wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                //Cargando Servicios
                cargaServiciosManteniendoSeleccion();
                //Ocultando ventana actual
                alternaVentanaModal("bitacoraMonitoreo", this);
                //Mostrando ventana de historial
                alternaVentanaModal("historialBitacora", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Eliminar bitácora de monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreo_ClickEliminar(object sender, EventArgs e)
        {
            //Guardando bitácora
            RetornoOperacion resultado = wucBitacoraMonitoreo.DeshabilitaBitacoraMonitoreo();

            //Si hay actualización
            if (resultado.OperacionExitosa)
            {
                //Recargando contenido de bitácora de monitoreo
                wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                //Cargando Servicios
                cargaServiciosManteniendoSeleccion();
                //Ocultando ventana actual
                alternaVentanaModal("bitacoraMonitoreo", this);
                //Mostrando ventana de historial
                alternaVentanaModal("historialBitacora", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Nuevo Servicio

        /// <summary>
        /// Clic en botón Nuevo servicio o reposicionamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoServicioMovimiento_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            Button boton = (Button)sender;

            //Ocultando ventana de selección
            alternaVentanaModal("seleccionServicioMovimiento", this);

            switch (boton.CommandName)
            {
                case "NuevoServicio":
                    //Inicializando contenido de control
                    wucServicioDocumentacion.InicializaControl();
                    //Abriendo ventana de copia de servicio maestro
                    alternaVentanaModal("documentacionServicio", this);
                    break;
                case "CopiaServicio":
                    //Inicializando contenido de control
                    wucServicioCopia.InicializaServicioCopia();
                    //Abriendo ventana de copia de servicio maestro
                    alternaVentanaModal("copiaServicioMaestro", this);
                    break;
            }
        }
        /// <summary>
        /// Clic en botón copiar servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioCopia_ClickGuardarServicioCopia(object sender, EventArgs e)
        {
            //Declarando variable de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciando servicio seleccionado a copia
            using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(wucServicioCopia.id_servicio_maestro))
                //Realizando copia de servicio seleccionado
                resultado = wucServicioCopia.CopiaServicio();

            //Si no hay errores de copia
            if (resultado.OperacionExitosa)
            {
                //Ocultando ventana modal de copia
                alternaVentanaModal("copiaServicioMaestro", this);

                //Actualizando lista de servicios
                cargaServicios();

                //Seleccionando servicio nuevo
                Controles.MarcaFila(gvServicios, resultado.IdRegistro.ToString(), "id_servicio", "id_servicio-movimiento-id_parada_actual-IdParadaInicial", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenadoServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 4);

                //SI hay un registro seleccionado
                if (gvServicios.SelectedIndex != -1)
                {
                    //Inicializando control de documentación
                    wucServicioDocumentacion.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), true, UserControls.wucServicioDocumentacion.VistaDocumentacion.Paradas);
                    //Mostrando ventana modal correspondiente
                    alternaVentanaModal("documentacionServicio", this);
                }

                //Carga de resumen
                cargaParadasCargaDescargaPendientes();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Clic en botón cancelar copia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioCopia_ClickCancelarServicioCopia(object sender, EventArgs e)
        {
            //Ocultando ventana modal de copia de servicio
            alternaVentanaModal("copiaServicioMaestro", this);
        }
        /// <summary>
        /// Agrega una parada al servicio que se está documentado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_ImbAgregarParada_Click(object sender, EventArgs e)
        {
            //Guardando parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaParadaServicio();

            //Si se documentó correctamente
            if (resultado.OperacionExitosa)
            {
                cargaServicios();
                //Carga de resumen
                cargaParadasCargaDescargaPendientes();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Elimina una parada de servicio y sus dependiencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbEliminarParada_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.EliminaParadaServicio();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Agrega un evento y/o producto a la parada activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_ImbAgregarProducto_Click(object sender, EventArgs e)
        {
            //Guardando producto de parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaProductoEvento();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Elimina un producto y sus dependencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbEliminarProducto_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.EliminaProductoEvento();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza el encabezado del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_BtnAceptarEncabezado_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaEncabezadoServicio();

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();
                //Cerrando ventana modal
                alternaVentanaModal("documentacionServicio", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón de citas de eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbCitasEventos_Click(object sender, EventArgs e)
        {
            //Si hay una parada seleccionada
            if (wucServicioDocumentacion.id_parada > 0)
            {
                //Cerrando ventana modal de documentación
                alternaVentanaModal("documentacionServicio", this);
                //Asignando comando de cierre de ventana de eventos
                lkbCerrarEventosParada.CommandArgument = "CitasEventos";
                //Inicializando control
                wucParadaEvento.InicializaControl(wucServicioDocumentacion.id_parada);
                //Mostrando ventana modal de eventos
                alternaVentanaModal("eventosParada", this);
            }
        }

        #endregion

        #region Asignación Recursos

        /// <summary>
        /// Clic en botón Agregar Recuros a Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickAgregarRecurso(object sender, EventArgs e)
        {
            //Asignabndo recurso
            asignaRecursoMovimiento();
        }
        /// <summary>
        /// Clic en botón liberar recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickLiberarRecurso(object sender, EventArgs e)
        {
            //Realizando la liberación de los recursos
            RetornoOperacion resultado = wucPreAsignacionRecurso.LiberaRecurso();

            //Si se ha liberado el recurso correctamente
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de servicios y sus asignaciones de movimiento
                cargaServiciosManteniendoSeleccion();
                //Actualziando lista de unidades
                cargaUnidades();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el proceso de adición de recurso al movimiento indicado
        /// </summary>
        private void asignaRecursoMovimiento()
        {
            //Realizando asignación de recursos
            RetornoOperacion resultado = wucPreAsignacionRecurso.AsignaRecursoViaje();

            //Si se agrega correctamente
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de servicios y sus asignaciones de movimiento
                cargaServiciosManteniendoSeleccion();
                //Actualziando lista de unidades
                cargaUnidades();
            }

            //Mostrando resultados
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón quitar de la lista de recursos asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbQuitarRecursoAsignado_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que existan Recursos Asignados
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosAsignados, sender, "lnk", false);

                //Instanciamos Asignación
                using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso
                                                                                  (Convert.ToInt32(gvRecursosAsignados.SelectedDataKey.Value)))
                {
                    //Validamos Deshabilitación de Recursos
                    resultado = objMovimientoAsignacionRecurso.ValidaDeshabilitacionRecursos();
                }
                //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {

                    //Asignamos Mensaje a la ventana Modal
                    lblMensaje.Text = resultado.Mensaje;
                    //Ocultando ventana de recursos asignados
                    alternaVentanaModal("unidadesAsignadas", (LinkButton)sender);
                    //Mostrando ventana de confirmación para quitar recurso asociado
                    alternaVentanaModal("confirmacionQuitarRecursos", (LinkButton)sender);
                }
                else
                    //Deshabilitamos Recurso Asignado
                    deshabilitaRecurso();
            }
        }
        /// <summary>
        /// Deshabilita Recurso
        /// </summary>
        private void deshabilitaRecurso()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la asignación
            using (MovimientoAsignacionRecurso r = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedValue)))
                //Realizando la deshabilitación
                resultado = r.DeshabilitaMovimientosAsignacionesRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Cargando lista de recursos asignados
                cargaRecursosAsignados();
                //Actualizamos la consulta de servicios pendientes
                cargaServiciosManteniendoSeleccion();
                //Actualizamos el grid
                upgvServicios.Update();
            }

            //Mostrando resultados
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en aceptar eliminar recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminacionRecurso_Click(object sender, EventArgs e)
        {
            //Deshabilitamos Recurso Asignado
            deshabilitaRecurso();
            //Ocultando ventana modal de confirmación
            alternaVentanaModal("confirmacionQuitarRecursos", (Button)sender);
        }
        /// <summary>
        /// Click en cancelar eliminación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminacionRecurso_Click(object sender, EventArgs e)
        {
            //Ocultando ventana de confirmación
            alternaVentanaModal("confirmacionQuitarRecursos", (Button)sender);
            //Mostrando ventana de recursos
            alternaVentanaModal("unidadesAsignadas", (Button)sender);
        }
        /// <summary>
        /// Realiza la carga de los Recursos Asignados 
        /// </summary>
        private void cargaRecursosAsignados()
        {
            //Obteniendo los recursos asignados
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(Convert.ToInt32(gvServicios.SelectedDataKey["movimiento"])))
            {
                //Inicializamos Indices
                Controles.InicializaIndices(gvRecursosAsignados);
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosAsignados, dt, "Id-IdRecurso", "", false, 0);

                //Validando datattable  no sea null
                if (dt != null)
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table3");
                else
                    //Eliminamos Tabla del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
            }
        }
        #endregion

        #region Referencias de Servicio

        /// <summary>
        /// Click Guardar Referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Realizando guardado de referencia
            RetornoOperacion resultado = wucReferenciaViaje.GuardaReferenciaViaje();
            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando Gridview
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click Eliminar Referencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Realizando guardado de referencia
            RetornoOperacion resultado = wucReferenciaViaje.EliminaReferenciaViaje();
            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando Gridview
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos de Parada

        /// <summary>
        /// Click en botón Actualizar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnActualizarClick(object sender, EventArgs e)
        {
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.GuardaEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {
                //Determinando el origen de apertura de ventana de eventos
                switch (lkbCerrarEventosParada.CommandArgument)
                {
                    case "Eventos":
                        //Se cierra ventana de eventos
                        alternaVentanaModal("eventosParada", this);
                        break;
                    case "CitasEventos":
                    case "Salida":
                        //Sin acciones, se debe cerrar manualmente la ventana
                        break;
                }

                //Actualizando lista de servicios
                cargaServiciosManteniendoSeleccion();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón Cancelar edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnCancelarClick(object sender, EventArgs e)
        {
            //Realizando cancelación de edición de evento
            wucParadaEvento.CancelarActualizacion();

            //Determinando el origen de apertura de ventana de eventos
            switch (lkbCerrarEventosParada.CommandArgument)
            {
                case "Eventos":
                    //Se cierra ventana de eventos
                    alternaVentanaModal("eventosParada", this);
                    break;
                case "CitasEventos":
                case "Salida":
                    //Sin acciones, se debe cerrar manualmente la ventana
                    break;
            }
        }
        /// <summary>
        /// Click en botón Nuevo Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnNuevoClick(object sender, EventArgs e)
        {
            //Realizando inserción de nuevo evento
            RetornoOperacion resultado = wucParadaEvento.NuevoEvento();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón eliminar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnlkbEliminarClick(object sender, EventArgs e)
        {
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.EliminaEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de servicios
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Cancelaciones

        /// <summary>
        /// Click en botones de cancelación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelacion_Click(object sender, EventArgs e)
        {
            //Determinando el comando a realizar
            switch (((Button)sender).CommandName)
            {
                case "Aceptar":
                    //Realizando cancelación
                    cancelaServicio();


                    break;
                case "Cancelar":
                    //Cerrando ventana de confirmación
                    alternaVentanaModal("confirmacionCancelacion", btnCancelarCancelacion);
                    break;
            }
        }
        /// <summary>
        /// Realiza la cancelación del servicio actual
        /// </summary>
        private void cancelaServicio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando servicio actual
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])))
            {
                //Si el registro se cargó correctamente
                if (servicio.id_servicio > 0)
                    //Actualizando registro
                    resultado = servicio.CancelaServicio(((Usuario)Session["usuario"]).id_usuario, txtMotivoCancelacion.Text);
            }

            //Validando si la operación de guardado fue correcta
            if (resultado.OperacionExitosa)
            {
                //Cargando servicios
                cargaServicios();
                //Actualizando lista de resumen
                cargaParadasCargaDescargaPendientes();
                //Cerrando ventana de confirmación
                alternaVentanaModal("confirmacionCancelacion", btnAceptarCancelacion);
            }

            //Mostrando mensaje de actualización 
            ScriptServer.MuestraNotificacion(btnAceptarCancelacion, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Resumen por Cliente

        /// <summary>
        /// Cambio de filtro de resumen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbResumenTotal_CheckedChanged(object sender, EventArgs e)
        {
            //Si se selecciona
            if (rdbResumenTotal.Checked)
                cargaParadasCargaDescargaPendientes();
        }
        /// <summary>
        /// Cambio de filtro de resumen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbResumenPendientes_CheckedChanged(object sender, EventArgs e)
        {
            //Si se selecciona
            if (rdbResumenPendientes.Checked)
                cargaParadasCargaDescargaPendientes();
        }
        /// <summary>
        /// Click en botón Vista de Resumen por Cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerResumenCliente_Click(object sender, EventArgs e)
        {
            //Mostrando ventana modal del resumen por cliente
            alternaVentanaModal("resumenPorCliente", lkbVerResumenCliente);
        }
        /// <summary>
        /// Cambio de criterio de orden del gv de resumen por cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumenCliente_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoPorResumenCliente.Text = Controles.CambiaSortExpressionGridView(gvResumenCliente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
            //Cargando totales
            calculaTotalesPieResumenCliente();
        }
        /// <summary>
        /// Cambio de página activa del gv de resumen por cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumenCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvResumenCliente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
            //Cargando totales
            calculaTotalesPieResumenCliente();
        }
        /// <summary>
        /// Tamaño de página de gv de resumen por cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoResumenCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvResumenCliente, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoResumenCliente.SelectedValue));
            //Cargando totales
            calculaTotalesPieResumenCliente();
        }
        /// <summary>
        /// Exportación de contenido de gv de resumen por cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarResumenCliente_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "");
        }

        #endregion

        #endregion

        #region Métodos Publicacion Unidad

        /// <summary>
        /// Carga Publicaciones Activas de las Unidades
        /// </summary>
        //private void cargaPublicacionesActivasUnidades()
        //{
        //    //Obtenemos Depósito
        //    using (DataTable mit = consumoPublicacionesActivasUnidades())
        //    {

        //        //Validando que el DataSet contenga las tablas
        //        if (Validacion.ValidaOrigenDatos(mit))
        //        {
        //            //Añadiendo Tablas al DataSet de Session
        //            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table4");

        //        }
        //        else
        //        {
        //            //Eliminando Tablas del DataSet de Session
        //            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
        //        }
        //    }
        //}


        /// <summary>
        /// Carga las Publicaciones Activas de las Unidades
        /// </summary>
        /// <returns></returns>
        //private DataTable consumoPublicacionesActivasUnidades()
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    DataTable mit = null;
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        //    {
        //        //Instanciamos Usuario
        //        using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
        //        {
        //            //Instanciamos Compañia
        //            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //            {

        //                string resultado_web_service = despacho.PublicacionesCompania(objCompania.id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                                     TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //                //Obtenemos Documento generado
        //                XDocument xDoc = XDocument.Parse(resultado_web_service);

        //                //Validamos que exista Respuesta
        //                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //                {
        //                    //Traduciendo resultado
        //                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //                }
        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Obtenemos DataSet
        //                    ds.ReadXml(xDoc.Document.Element("Publicaciones").Element("NewDataSet").CreateReader());
        //                    //Asignamos tabla
        //                    mit = ds.Tables["Table"];
        //                }
        //                else
        //                {
        //                    //Establecmos Mensaje Resultado
        //                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //                }
        //            }
        //        }
        //        //Cerramos Conexiones
        //        despacho.Close();
        //    }
        //    return mit;
        //}



        #endregion

        #region Métodos Publicación de Unidades
        /// <summary>
        /// Evento generado al publicar una Unidad
        /// </summary>
        protected void wucPublicacionUnidad_ClickRegistrar(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Publicamos Unidad
            //resultado = wucPublicacionUnidad.PublicaUnidad();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos ventana para la Publicación de la Unidad
                alternaVentanaModal("publicacionUnidad", this);

                //Cargamos gv de Unidades
                cargaUnidades();
            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion

        #region Métodos Respuestas de Unidades

        /// <summary>
        /// Método encargado de cargar las Respuestas
        /// </summary>
        /// <param name="control"></param>
        //private void cargaResultadoRespuesta(System.Web.UI.Control control)
        //{
        //    //Obtenemos los Resultados
        //    using (DataTable mit = consumoResultadoRespuestas(control))
        //    {

        //        //Validando que el DataSet contenga las tablas
        //        if (Validacion.ValidaOrigenDatos(mit))
        //        {
        //            //Cargando los GridView
        //            Controles.CargaGridView(gvResultadoRespuesta, mit, "Id", lblOrdenadoResultadoRespuesta.Text, false, 3);
        //            //Añadiendo Tablas al DataSet de Session
        //            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table5");

        //        }
        //        else
        //        {   //Inicializando GridViews
        //            Controles.InicializaGridview(gvResultadoRespuesta);
        //            //Eliminando Tablas del DataSet de Session
        //            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
        //        }
        //    }
        //}

        /// <summary>
        /// Carga los Resultados de la Respuestas
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        //private DataTable consumoResultadoRespuestas(System.Web.UI.Control control)
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


        //    string resultado_web_service = despacho.ObtieneRespuestasPublicacion(consumoObtieneIdPublicacionUnidad(control), true, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //    //Obtenemos Documento generado
        //    XDocument xDoc = XDocument.Parse(resultado_web_service);

        //    //Validamos que exista Respuesta
        //    if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //    {
        //        //Traduciendo resultado
        //        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //    }
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Obtenemos DataSet
        //        ds.ReadXml(xDoc.Document.Element("RespuestasPublicacion").Element("NewDataSet").CreateReader());
        //    }

        //    else
        //    {
        //        //Establecmos Mensaje Resultado
        //        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //    }
        //    //Si Existe Error
        //    if (!resultado.OperacionExitosa)
        //    {
        //        //Mostrando Mensaje de Operación
        //        TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //    }
        //    return ds.Tables["Table"];
        //}

        /// <summary>
        /// Carga las Respuestas
        /// </summary>
        /// <param name="control"></param>
        //private void cargaRespuestas(System.Web.UI.Control control)
        //{
        //    //Obtenemos Detalle de la Respuesta
        //    using (DataSet ds = consumoRespuestas(control))
        //    {

        //        //Validando que el DataSet contenga las tablas
        //        if (Validacion.ValidaOrigenDatos(ds.Tables["Table"]))
        //        {
        //            //Cargando los GridView
        //            Controles.CargaGridView(gvRespuestas, ds.Tables["Table"], "Id-TarifaOfertada", lblOrdenadoRespuestas.Text, false, 3);
        //            //Añadiendo Tablas al DataSet de Session
        //            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table6");

        //        }
        //        else
        //        {   //Inicializando GridViews
        //            Controles.InicializaGridview(gvRespuestas);
        //            //Eliminando Tablas del DataSet de Session
        //            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");
        //        }
        //    }
        //}

        /// <summary>
        /// Carga las Respuestas
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataSet consumoRespuestas(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.VisualizarRespuesta(Convert.ToInt32(gvResultadoRespuesta.SelectedValue), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);

            //Obtenemos Documento generado
            XDocument xDoc = XDocument.Parse(resultado_web_service);

            //Validamos que exista Respuesta
            if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
            {
                //Traduciendo resultado
                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Obtenemos DataSet
                ds.ReadXml(xDoc.Document.Element("VisualizacionRespuesta").Element("NewDataSet").CreateReader());
            }

            else
            {
                //Establecmos Mensaje Resultado
                resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
            }
            //Si Existe Error
            if (!resultado.OperacionExitosa)
            {
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return ds;
        }

        /// <summary>
        /// Carga las Respuestas
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private int consumoObtieneIdPublicacionUnidad(System.Web.UI.Control control)
        {
            //Establecemos variable resultado
            DataSet ds = new DataSet();
            int Id_Publicacion = 0;
            RetornoOperacion resultado = new RetornoOperacion();

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


            string resultado_web_service = despacho.ObtienePublicacionUnidad(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvUnidades.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

            //Obtenemos Documento generado
            XDocument xDoc = XDocument.Parse(resultado_web_service);

            //Validamos que exista Respuesta
            if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
            {
                //Traduciendo resultado
                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Obtenemos DataSet
                ds.ReadXml(xDoc.Document.Element("Publicacion").Element("NewDataSet").CreateReader());

                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(ds))
                {
                    //Establecemos Id Retorno
                    Id_Publicacion = (from DataRow r in ds.Tables["Table"].Rows
                                      select r.Field<int>("idPublicacion")).FirstOrDefault();
                }
            }

            else
            {
                //Establecmos Mensaje Resultado
                resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
            }
            //Si Existe Error
            if (!resultado.OperacionExitosa)
            {
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return Id_Publicacion;
        }
        /// <summary>
        ///  Método encargado de cargar las Ciudades
        /// </summary>
        /// <param name="id_respuesta"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private DataTable cargaCiudades(int id_respuesta, System.Web.UI.Control control)
        {
            //Declaramos Objeto resultado
            DataTable mit = new DataTable();
            //Obtenemos las Ciudades
            using (DataSet ds = consumoRespuestas(control))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds.Tables["Table1"]))
                {
                    //Obtenemos La Referencias del Concepto Origen
                    DataRow[] re = (from DataRow r in ds.Tables["Table1"].Rows
                                    where Convert.ToInt32(r["IdRespuesta"]) == id_respuesta
                                    select r).ToArray();
                    //Validamos que exista elementos
                    if (re.Length > 0)
                    {
                        //Obtenemos Datatable Referencias solo del Concepto Origen
                        mit = TSDK.Datos.OrigenDatos.ConvierteArregloDataRowADataTable(re);
                    }


                }
            }
            //Devolvemos Resultado
            return mit;
        }

        /// <summary>
        /// Acepta la Respuesta  de la Publicación de la Unidad
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion aceptarRespuesta()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();

            //Instanciamos Compañia
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {

                string resultado_web_service = despacho.AceptaRespuestaPublicacion(Convert.ToInt32(gvRespuestas.SelectedValue), Convert.ToDecimal(txtTarifaAceptadaPU.Text), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                   ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


                //Obtenemos Documento generado
                XDocument xDoc = XDocument.Parse(resultado_web_service);

                //Validamos que exista Respuesta
                if (xDoc != null)
                {
                    //Traduciendo resultado
                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Personalizamos Mensaje
                        resultado = new RetornoOperacion("La Respuesta ha sido Aceptada", true);
                    }

                }
                else
                {
                    //Establecmos Mensaje Resultado
                    resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                }
                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            return resultado;
        }

        #endregion

        #region Eventos Respuestas Unidades

        /// <summary>
        /// Evento generado al cambiar el tamalo de la Págin de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuesta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.NewPageIndex, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuesta_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoResultadoRespuesta.Text = Controles.CambiaSortExpressionGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.SortExpression, false, 3);

        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlResultadoRespuesta_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvResultadoRespuesta, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), Convert.ToInt32(ddlTamanoResultadoRespuesta.SelectedValue), false, 3);

        }

        /// <summary>
        /// Evento generado al Ver los Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionResultado_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;
            //Si hay registros
            if (gvResultadoRespuesta.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvResultadoRespuesta, sender, "lnk", false);
                //De acuerdo al comando del botón
                switch (lkbCerrar.CommandName)
                {
                    case "VerDetalles":

                        //Mostrando ventana modal 
                        alternaVentanaModal("opcionSeleccionRespuesta", gvResultadoRespuesta);
                        //ocultamos ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuesta", gvResultadoRespuesta);
                        //Cargamos Respuestas
                        //cargaRespuestas(gvResultadoRespuesta);
                        break;
                    case "RechazarRespuestas":
                        //Mostrando ventana modal 
                        alternaVentanaModal("rechazarRespuesta", gvResultadoRespuesta);
                        //Ocultamos ventana modal correspondiente
                        alternaVentanaModal("resultadoRespuesta", gvResultadoRespuesta);
                        break;
                }
            }
        }

        /// <summary>
        /// Evento Generado al enlazar el Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvRespuestas.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Eventos
                    using (GridView gvCiudadRespuesta = (GridView)e.Row.FindControl("gvCiudadRespuesta"))
                    {
                        //Carga Eventos para cada una de las Paradas
                        using (DataTable mit = cargaCiudades(Convert.ToInt32(gvRespuestas.DataKeys[e.Row.RowIndex].Value), gvRespuestas))
                        {
                            //Validamos Origen de Datos
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                            {
                                //Cargamos Grid View Eventos
                                TSDK.ASP.Controles.CargaGridView(gvCiudadRespuesta, mit, "IdCiudad", "");

                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al Cambiar el Tamaño del Drop Down List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRespuestas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), Convert.ToInt32(ddlTamanoRespuestas.SelectedValue), false, 3);

        }
        /// <summary>
        /// Cambio de página activa del GV de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.NewPageIndex, false, 4);
        }

        /// <summary>
        /// Cambio de criterio de ordenamiento en GV de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestas_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoRespuestas.Text = Controles.CambiaSortExpressionGridView(gvRespuestas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.SortExpression, false, 4);
        }

        /// <summary>
        /// Click en algún Link de GV de Respúestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRespuestaPU_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvRespuestas.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvRespuestas, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Aceptar":
                        //limpiamos  Control de Tarifa
                        txtTarifaAceptadaPU.Text = "";//string.Format(gvRespuestas.SelectedDataKey["TarifaOfertada"].ToString(), "0:C2");
                        // Abriendo ventana 
                        alternaVentanaModal("aceptarRespuesta", lkb);
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuesta", lkb);
                        break;
                    case "Confirmar":
                        // Abriendo ventana para de Ciudades
                        alternaVentanaModal("informacionViajes", lkb);
                        //Carga Viajes
                        cargaServicios();
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuesta", lkb);
                        break;
                }
            }
        }

        /// <summary>
        /// Evento generado al Aceptar la Respuesta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarRespuesta_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Pestaña Activa de Unidad
            if (mtvPlaneacion.ActiveViewIndex == 1)
            {
                //Aceptamos Respuesta
                resultado = aceptarRespuesta();
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {
                    //Cerramos ventana Modal
                    alternaVentanaModal("aceptarRespuesta", btnAceptarRespuesta);
                    //Mostrando ventana modal 
                    alternaVentanaModal("opcionSeleccionRespuesta", btnAceptarRespuesta);
                    //Cargamos Respuestas
                    //cargaRespuestas(btnAceptarRespuesta);
                }
            }
            else
            {

                lblValorTercero.Text = "No";
                //Aceptamos Respuesta de la Publicación de Servicio
                //resultado = aceptarRespuestaPS();
                //Validamos resultado
                if (resultado.OperacionExitosa)
                {

                    //Obtenemos primer movimiento del Servicio
                    using (SAT_CL.Despacho.Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), 1))
                    {
                        //Insertamos Asignación Recurso
                        resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                objMovimiento.id_movimiento, MovimientoAsignacionRecurso.Tipo.Tercero, 0, resultado.IdRegistro,
                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }


                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Cerramos ventana Modal
                        alternaVentanaModal("aceptarRespuesta", btnAceptarRespuesta);
                        //Cargamos Servicios
                        cargaServicios();
                    }
                }
                //Validamos que no exista Tercero
                else if (resultado.IdRegistro == -6)
                {
                    //Inicializamos Controles 
                    lblDescripcionTercero.Text = Cadena.RegresaCadenaSeparada(resultado.Mensaje, "ID:", 0);
                    //Inicializamos Autocomplete
                    //inicializaAutocompleteTransportista(btnAceptarRespuesta);
                    lblTercero.Text = "Transportista";
                    //Mostramos Modal en Caso de Ser Necesario
                    if (lblValorTercero.Text == "No")
                    {
                        //Abrimos Ventana Modal
                        alternaVentanaModal("tercero", btnAceptarRespuesta);
                        lblValorTercero.Text = "Si";
                    }

                }
            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        /// <summary>
        /// Evento generado al Agregar un Elemento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarElemento_Click(object sender, EventArgs e)
        {
            //Agregamos Elemento
            //agregaElemento(btnAgregarElemento);
        }

        /// <summary>
        /// Evento generado al agregar al transportista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarTercero_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Insertamos Relación de Transportista
            //resultado = insertaRelacionTercero(btnAgregarTercero);

            //Aceptamos Respuesta de la Publicación de Servicio
            //resultado = aceptarRespuestaPS();
            //Validamos resultado
            if (resultado.OperacionExitosa)
            {
                //Obtenemos primer movimiento del Servicio
                using (SAT_CL.Despacho.Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), 1))
                {
                    //Insertamos Asignación Recurso
                    resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                            objMovimiento.id_movimiento, MovimientoAsignacionRecurso.Tipo.Tercero, 0, resultado.IdRegistro,
                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cerramos Modal
                    if (lblValorTercero.Text == "Si")
                    {
                        //Cerramos Modal
                        alternaVentanaModal("tercero", btnAgregarTercero);
                        //Cerramos ventana Modal
                        alternaVentanaModal("aceptarRespuesta", btnAgregarTercero);
                    }
                    //Carga Servicios 
                    cargaServicios();
                }
            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarTercero, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        /// <summary>
        /// Evento Generado al rechazar la Respuesta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRechazarRespuesta_Click(object sender, EventArgs e)
        {
            //Declaramos Objetp Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Rechazamos Respuesta
            //resultado = consumoRechazarRespuesta();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Mostrando ventana modal correspondiente
                alternaVentanaModal("resultadoRespuesta", btnRechazarRespuesta);
                //Cerramos ventana modal correspondiente
                alternaVentanaModal("rechazarRespuesta", btnRechazarRespuesta);
                //Cargamos Resultados
                //cargaResultadoRespuesta(btnRechazarRespuesta);
            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnRechazarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        #endregion

        //#region Métodos Tomar Servicio Publicación de Unidad
        ///// <summary>
        /////  Método Encargado de Copiar un Servicio de la Publicación de Unidad
        ///// </summary>
        ///// <param name="control"></param>
        ///// <param name="validacion"></param>
        //private void copiarServicioPUFinal(System.Web.UI.Control control, bool validacion, DataSet ds)
        //{
        //    //Declaramos Objeto Retorno
        //    RetornoOperacion resultado = new RetornoOperacion(0);
        //    int id_servicio = 0;
        //    if (validacion == false)
        //    {
        //        //Mostrando ventana modal correspondiente
        //        resultado = muestraElemento(control, out ds);
        //    }
        //    //Validamos resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Seleccionando el servicio
        //        DataRow s = (from DataRow r in ds.Tables["Servicio"].Rows
        //                     select r).FirstOrDefault();
        //        //Declaramos Id de Movimiento
        //        int id_movimiento = 0;
        //        //Copiamos Servicio
        //        resultado = SAT_CL.Documentacion.Servicio.CopiarServicioPublicacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
        //                   Convert.ToInt32(s["idCliente"]), s["noViaje"].ToString(), s["confirmacion"].ToString(),
        //                   s["observacion"].ToString(), Convert.ToInt32(s["idProducto"]), 0, Convert.ToDecimal(s["peso"]), Convert.ToDecimal(s["tarifaAceptada"]),
        //                  Convert.ToInt32(s["idConcepto"]), ds.Tables["Parada"], 1, out id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

        //        //Validamos resultado
        //        if (resultado.OperacionExitosa)
        //        {
        //            //Instanciamos Unidad
        //            using (SAT_CL.Global.Unidad objUnidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedValue)))
        //            {
        //                //Inseratmos Asignación
        //                resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
        //                        id_movimiento, MovimientoAsignacionRecurso.Tipo.Unidad, objUnidad.id_tipo_unidad, Convert.ToInt32(gvUnidades.SelectedValue),
        //                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Asignamos Id de Servicio
        //                    id_servicio = resultado.IdRegistro;
        //                    //Actualizamos Id de Servicio
        //                    resultado = actualizaIdServicioDestino(id_servicio);
        //                }
        //            }
        //        }

        //        //Si no hay errores
        //        if (resultado.OperacionExitosa)
        //        {
        //            //Crenado resultado con Id de Servicio nuevo
        //            resultado = new RetornoOperacion(id_servicio);
        //            //Cargamos Unidades
        //            cargaUnidades();
        //            //Cerramos Modal
        //            if (lblValor.Text == "Si")
        //            {
        //                //Cerramos Ventana Modal
        //                alternaVentanaModal("elemento", control);
        //            }
        //        }
        //    }

        //    //Mostrando Mensaje de Operación
        //    TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //}

        ///// <summary>
        ///// Mostramos Venta de Alta de Elementos Correspondiestes(Producto, Cliente, Concepto, Paradas)
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion muestraElemento(System.Web.UI.Control control, out DataSet ds)
        //{
        //    //Limpiamos Valores
        //    inicializaValoresElemento();
        //    //Declaramos Objeto Resultado
        //    RetornoOperacion resultado = new RetornoOperacion();
        //    ds = new DataSet();
        //    //Validamos Tipo de Información
        //    if (mtvPlaneacion.ActiveViewIndex == 1)
        //    {
        //        //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
        //        ds = obtieneServicioPU(control, out resultado);
        //    }
        //    else
        //    {
        //        //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
        //        ds = obtieneServicioPS(control, out resultado);
        //    }
        //    //Validamos Resultado en Caso de Error
        //    if (!resultado.OperacionExitosa)
        //    {
        //        //Inicializamos Elemento 
        //        lblDescipcion.Text = Cadena.RegresaCadenaSeparada(resultado.Mensaje, "ID:", 0);
        //        //Inicializamos Elemento 
        //        lblIdElemento.Text = Cadena.RegresaCadenaSeparada(resultado.Mensaje, "ID:", 1);
        //        //De acuerdo al Error Obtenido
        //        switch (resultado.IdRegistro)
        //        {
        //            //No existe el Cliente
        //            case -6:
        //                //Inicializamos Autocomplete
        //                inicializaAutocompleteCliente(control);
        //                lblElemento.Text = "Cliente";
        //                //Mostramos Modal en Caso de Ser Necesario
        //                if (lblValor.Text == "No")
        //                {
        //                    //Abrimos Ventana Modal
        //                    alternaVentanaModal("elemento", control);
        //                    lblValor.Text = "Si";
        //                }
        //                break;
        //            //No existe la Ubicación
        //            case -7:
        //                //Inicializamos Autocomplete
        //                inicializaAutocompleteUbicacion(control);
        //                lblElemento.Text = "Ubicacion";
        //                //Mostramos Modal en Caso de Ser Necesario
        //                if (lblValor.Text == "No")
        //                {
        //                    //Abrimos Ventana Modal
        //                    alternaVentanaModal("elemento", control);
        //                    //Cambiamos Valor
        //                    lblValor.Text = "Si";
        //                }
        //                break;
        //            case -8:
        //                //Inicializamos Aucomplete
        //                inicializaAutocompleteConcepto(control);
        //                lblElemento.Text = "Concepto";
        //                //Mostramos Modal en Caso de Ser Necesario
        //                if (lblValor.Text == "No")
        //                {
        //                    //Abrimos Ventana Modal
        //                    alternaVentanaModal("elemento", control);
        //                    lblValor.Text = "Si";
        //                }
        //                break;
        //            //No existe el Producto
        //            case -9:
        //                //Inicializamos Aucomplete
        //                inicializaAutocompleteProducto(control);
        //                lblElemento.Text = "Producto";
        //                //Mostramos Modal en Caso de Ser Necesario
        //                if (lblValor.Text == "No")
        //                {
        //                    //Abrimos Ventana Modal
        //                    alternaVentanaModal("elemento", control);
        //                    lblValor.Text = "Si";
        //                }
        //                break;
        //        }
        //    }
        //    //Deviolvemos Resultado
        //    return resultado;
        //}
        ///// <summary>
        ///// Copia un Servcio de una Publicacion de Unidad
        ///// </summary>
        ///// <returns></returns>
        //private DataSet obtieneServicioPU(System.Web.UI.Control control, out RetornoOperacion resultado)
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        //    {
        //        //Instanciamos Usuario
        //        using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
        //        {
        //            //Instanciamos Compañia
        //            using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //            {

        //                string resultado_web_service = despacho.ObtieneDatosServicioUnidad(consumoObtieneIdPublicacionUnidad(control), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //                //Obtenemos Documento generado
        //                XDocument xDoc = XDocument.Parse(resultado_web_service);

        //                //Validamos que exista Respuesta
        //                if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //                {
        //                    //Traduciendo resultado
        //                    resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //                }
        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Obtenemos DataSet
        //                    ds.ReadXml(xDoc.Document.Element("Confirmacion").CreateReader());
        //                }
        //            }
        //        }
        //        despacho.Close();
        //    }
        //    return ds;
        //}

        ///// <summary>
        /////  Inicializamos Autocomplete de Cliente
        ///// </summary>
        ///// <param name="control"></param>
        //private void inicializaAutocompleteCliente(System.Web.UI.Control control)
        //{
        //    //Generamos Sript
        //    string script =
        //    @"<script type='text/javascript'>
        //      $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
        //      </script>";

        //    //Registrando el script sólo para los paneles que producirán actualización del mismo
        //    System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteCliente", script, false);
        //}
        ///// <summary>
        /////  Inicializamos Autocomplete de Transportista
        ///// </summary>
        ///// <param name="control"></param>
        //private void inicializaAutocompleteTransportista(System.Web.UI.Control control)
        //{
        //    //Generamos Sript
        //    string script =
        //    @"<script type='text/javascript'>
        //      $('#" + this.txtTercero.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=18&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
        //      </script>";

        //    //Registrando el script sólo para los paneles que producirán actualización del mismo
        //    System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteCliente", script, false);
        //}
        ///// <summary>
        /////  Inicializamos Autocomplete de Ublicacion
        ///// </summary>
        ///// <param name="control"></param>
        //private void inicializaAutocompleteUbicacion(System.Web.UI.Control control)
        //{
        //    //Generamos Sript
        //    string script =
        //    @"<script type='text/javascript'>
        //      $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
        //      </script>";

        //    //Registrando el script sólo para los paneles que producirán actualización del mismo
        //    System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteUbicacion", script, false);
        //}
        ///// <summary>
        /////  Inicializamos Autocomplete de Concepto
        ///// </summary>
        ///// <param name="control"></param>
        //private void inicializaAutocompleteConcepto(System.Web.UI.Control control)
        //{
        //    //Generamos Sript
        //    string script =
        //    @"<script type='text/javascript'>
        //      $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=52&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
        //      </script>";

        //    //Registrando el script sólo para los paneles que producirán actualización del mismo
        //    System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteConcepto", script, false);
        //}
        ///// <summary>
        ///// Inicializamos Autocomplete de Producto
        ///// </summary>
        ///// <param name="control"></param>
        //private void inicializaAutocompleteProducto(System.Web.UI.Control control)
        //{
        //    //Generamos Sript
        //    string script =
        //    @"<script type='text/javascript'>
        //      $('#" + this.txtElemento.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=1&param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
        //      </script>";

        //    //Registrando el script sólo para los paneles que producirán actualización del mismo
        //    System.Web.UI.ScriptManager.RegisterStartupScript(control, control.GetType(), "AutocompleteProducto", script, false);
        //}
        ///// <summary>
        ///// Copia un Servcio de una Publicacion de Servicio
        ///// </summary>
        ///// <returns></returns>
        //private DataSet obtieneServicioPS(System.Web.UI.Control control, out RetornoOperacion resultado)
        //{
        //    ////Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    resultado = new RetornoOperacion();

        //    ////Consumimos Web Service
        //    ////using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        //    ////{
        //    ////    Instanciamos Usuario
        //    ////    using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
        //    ////    {
        //    ////        Instanciamos Compañia
        //    ////        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //    ////        {

        //    ////            string resultado_web_service = despacho.ObtienePublicacionServicio(Convert.ToInt32(gvServicios.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //    ////                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //    ////            Obtenemos Documento generado
        //    ////            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //    ////            Validamos que exista Respuesta
        //    ////            if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //    ////            {
        //    ////                Traduciendo resultado
        //    ////                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //    ////            }
        //    ////            Validamos Resultado
        //    ////            if (resultado.OperacionExitosa)
        //    ////            {
        //    ////                Obtenemos DataSet
        //    ////                ds.ReadXml(xDoc.Document.Element("Confirmacion").CreateReader());
        //    ////            }
        //    ////        }
        //    ////    }
        //    ////    despacho.Close();
        //    ////}
        //    return ds;
        //}

        ///// <summary>
        ///// Inicializamos Valores 
        ///// </summary>
        //private void inicializaValoresElemento()
        //{
        //    //Limpiamos Valores
        //    lblElemento.Text = "";
        //    lblDescipcion.Text = "";
        //    lblIdElemento.Text = "";
        //    txtElemento.Text = "";
        //}

        ///// <summary>
        ///// Actualiza el Id de Servicio Destino
        ///// </summary>
        ///// <param name="id_servicio"></param>
        ///// <returns></returns>
        //private RetornoOperacion actualizaIdServicioDestino(int id_servicio)
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = despacho.ActualizaServicioUnidad(Convert.ToInt32(consumoObtieneIdPublicacionUnidad(gvUnidades)), id_servicio, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //            }
        //        }
        //        //Cerramos Web Service
        //        despacho.Close();
        //    }
        //    return resultado;
        //}

        ///// <summary>
        ///// Rechazamos la Respuesta
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion consumoRechazarRespuesta()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        //    {

        //        string resultado_web_service = despacho.RechazaRespuestaPublicacion(Convert.ToInt32(gvResultadoRespuesta.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                           ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //        //Obtenemos Documento generado
        //        XDocument xDoc = XDocument.Parse(resultado_web_service);

        //        //Validamos que exista Respuesta
        //        if (xDoc != null)
        //        {
        //            //Traduciendo resultado
        //            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //        }
        //        else
        //        {
        //            //Establecmos Mensaje Resultado
        //            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //        }
        //        //Cerramos Web Service
        //        despacho.Close();
        //    }
        //    return resultado;
        //}

        ///// <summary>
        ///// Método encargado  de Agregar Elemnto
        ///// </summary>
        //private void agregaElemento(System.Web.UI.Control control)
        //{
        //    //Declaramos Objeto Resultado
        //    RetornoOperacion resultado = new RetornoOperacion();
        //    DataSet ds = new DataSet();
        //    //De acuerdo al Elemto Por Registrar
        //    switch (lblElemento.Text)
        //    {
        //        //No existe el Cliente
        //        case "Cliente":
        //            //Validamos Tipo de Relación a Insertar
        //            //Unidad
        //            if (mtvPlaneacion.ActiveViewIndex == 1)
        //            {
        //                //Insertamos Relación de
        //                resultado = insertaRelacionCliente();
        //            }
        //            else
        //            {
        //                //Insertamos Relación de
        //                // resultado = insertaRelacionClientePS();
        //            }
        //            break;
        //        //No existe la Ubicación
        //        case "Ubicacion":
        //            //Validamos Tipo de Relación a Insertar
        //            //Unidad
        //            if (mtvPlaneacion.ActiveViewIndex == 1)
        //            {
        //                //Insertamos Relación
        //                resultado = insertaRelacionUbicacion();
        //            }
        //            else
        //            {
        //                //Insertamos Relación
        //                // resultado = insertaRelacionUbicacionPS();
        //            }
        //            break;
        //        case "Concepto":
        //            //Validamos Tipo de Relación a Insertar
        //            //Unidad
        //            if (mtvPlaneacion.ActiveViewIndex == 1)
        //            {
        //                //Insertamos Relación
        //                resultado = insertaRelacionConcepto();
        //            }
        //            else
        //            {
        //                //Insertamos Relación
        //                // resultado = insertaRelacionConceptoPS();
        //            }
        //            break;
        //        //No existe el Producto
        //        case "Producto":
        //            //Validamos Tipo de Relación a Insertar
        //            //Unidad
        //            if (mtvPlaneacion.ActiveViewIndex == 1)
        //            {
        //                //Insertamos Relación
        //                resultado = insertaRelacionProducto();
        //            }
        //            else
        //            {
        //                //Insertamos Relación
        //                // resultado = insertaRelacionProductoPS();
        //            }
        //            break;

        //    }
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Validamos Tipo de Información
        //        if (mtvPlaneacion.ActiveViewIndex == 1)
        //        {
        //            //Validamos Obtención de Información del Viaje de la Publicacipón de Unidad
        //            ds = obtieneServicioPU(control, out resultado);
        //        }
        //        else
        //        {
        //            //Validamos Obtención de Información del Viaje de la Publicacipón de Servicio
        //            ds = obtieneServicioPS(control, out resultado);
        //        }

        //        //Validamos Resultado
        //        if (resultado.OperacionExitosa)
        //        {
        //            //Unidad
        //            if (mtvPlaneacion.ActiveViewIndex == 1)
        //            {
        //                //Mostramos Nuevo Elemento
        //                copiarServicioPUFinal(control, true, ds);
        //            }
        //            else
        //            {
        //                // copiarServicioPSFinal(control, true);
        //            }
        //        }
        //        else
        //        {
        //            DataSet ds1 = new DataSet();
        //            //Mostramos Informacion
        //            muestraElemento(control, out ds1);
        //        }
        //    }
        //    //Mostrando Mensaje de Operación
        //    TSDK.ASP.ScriptServer.MuestraNotificacion(btnAgregarElemento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //}

        ///// <summary>
        /////Insertamos Relación de Concepto
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion insertaRelacionConcepto()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = global.InsertaRelacionConceptoFletePublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Personalizamos Mensaje
        //                    resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
        //                }

        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
        //            }
        //        }
        //        global.Close();
        //        return resultado;
        //    }
        //}

        ///// <summary>
        /////Insertamos Relación de Ubicacion
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion insertaRelacionUbicacion()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = global.InsertaRelacionUbicacionPublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Personalizamos Mensaje
        //                    resultado = new RetornoOperacion("La Ubicación ha sido Registrada", true);
        //                }

        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
        //            }
        //        }
        //        global.Close();
        //        return resultado;
        //    }
        //}

        ///// <summary>
        /////Insertamos Relación de Producto
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion insertaRelacionProducto()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = global.InsertaRelacionProductoPublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Personalizamos Mensaje
        //                    resultado = new RetornoOperacion("El Producto ha sido Registrado", true);
        //                }

        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
        //            }
        //        }
        //        global.Close();
        //        return resultado;
        //    }
        //}
        ///// <summary>
        /////Insertamos Relación de Cliente
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion insertaRelacionCliente()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = global.InsertaRelacionClientePublicacion(Convert.ToInt32(lblIdElemento.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtElemento.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Personalizamos Mensaje
        //                    resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
        //                }

        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
        //            }
        //        }
        //        global.Close();
        //        return resultado;
        //    }
        //}

        ///// <summary>
        /////Insertamos Relación de Tercero
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion insertaRelacionTercero(System.Web.UI.Control control)
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
        //    {
        //        //Instanciamos Compañia
        //        using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //        {

        //            string resultado_web_service = global.InsertaRelacionTransportistaServicio(Convert.ToInt32(gvRespuestasPS.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTercero.Text, "ID:", 1)), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                               TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                               ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //            //Obtenemos Documento generado
        //            XDocument xDoc = XDocument.Parse(resultado_web_service);

        //            //Validamos que exista Respuesta
        //            if (xDoc != null)
        //            {
        //                //Traduciendo resultado
        //                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));


        //                //Validamos Resultado
        //                if (resultado.OperacionExitosa)
        //                {
        //                    //Personalizamos Mensaje
        //                    resultado = new RetornoOperacion("Tu respuesta ha sido Registrada", true);
        //                }

        //            }
        //            else
        //            {
        //                //Establecmos Mensaje Resultado
        //                resultado = new RetornoOperacion("No es posible obtener la respuesta WS");
        //            }
        //        }
        //        global.Close();
        //        return resultado;
        //    }
        //}
        //#endregion

        //#region Métodos Publicación de Servicio

        ///// <summary>
        ///// Evento generado al publicar un Servicio
        ///// </summary>
        //protected void wucPublicacionServicio_ClickRegistrar(object sender, EventArgs e)
        //{
        //    //Declaramos Objeto Resultado
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Publicamos Servicio
        //    //resultado = wucPublicacionServicio.PublicaServicio();
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Cerramos ventana para la Publicación del Servicio
        //        alternaVentanaModal("PublicacionServicio", this);

        //        //Carga Publicaciones de Servicio
        //        cargaServicios();
        //    }
        //}

        ///// <summary>
        ///// Carga Publicaciones Activas de los Servicios
        ///// </summary>
        ///// <param name="id_servicios"></param>
        //private void cargaPublicacionesActivasServicios(int[] id_servicios)
        //{
        //    //Obtenemos Depósito
        //    //using (DataTable mit = consumoPublicacionesActivasServicios(id_servicios))
        //    //{
        //    //    //Validando que el DataSet contenga las tablas
        //    //    if (Validacion.ValidaOrigenDatos(mit))
        //    //    {
        //    //        //Añadiendo Tablas al DataSet de Session
        //    //        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table7");
        //    //    }
        //    //    else
        //    //    {
        //    //        //Eliminando Tablas del DataSet de Session
        //    //        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
        //    //    }
        //    //}
        //}

        ///// <summary>
        ///// Carga las Publicaciones Activas de los Servicios
        ///// </summary>
        ///// <returns></returns>
        ////private DataTable consumoPublicacionesActivasServicios(int[] id_servicios)
        ////{
        ////    //Establecemos variable resultado
        ////    DataSet ds = new DataSet();
        ////    DataTable mit = null;
        ////    RetornoOperacion resultado = new RetornoOperacion();
        ////    DespachoCentral.ArrayOfInt array = new DespachoCentral.ArrayOfInt();
        ////    array.AddRange(id_servicios);
        ////    //Consumimos Web Service
        ////    using (DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient())
        ////    {
        ////        //Instanciamos Usuario
        ////        using (SAT_CL.Seguridad.Usuario objUsuario = new SAT_CL.Seguridad.Usuario(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
        ////        {
        ////            //Consumimos  Web Service
        ////            string resultado_web_service = despacho.ServiciosCompania(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, array, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        ////                                 TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        ////            //Obtenemos Documento generado
        ////            XDocument xDoc = XDocument.Parse(resultado_web_service);

        ////            //Validamos que exista Respuesta
        ////            if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        ////            {
        ////                //Traduciendo resultado
        ////                resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        ////            }
        ////            //Validamos Resultado
        ////            if (resultado.OperacionExitosa)
        ////            {
        ////                //Obtenemos DataSet
        ////                ds.ReadXml(xDoc.Document.Element("Servicios").Element("NewDataSet").CreateReader());
        ////                //Asignamos tabla
        ////                mit = ds.Tables["Table"];
        ////            }
        ////            else
        ////            {
        ////                //Establecmos Mensaje Resultado
        ////                resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        ////            }
        ////        }
        ////        //Cerramos Conexiones
        ////        despacho.Close();
        ////    }
        ////    return mit;
        ////}

        ///// <summary>
        ///// Obtiene el Id de Publicación de Servicio
        ///// </summary>
        ///// <param name="control"></param>
        ///// <returns></returns>
        //private int consumoObtieneIdPublicacionServicio(System.Web.UI.Control control)
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    int Id_Publicacion = 0;
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


        //    string resultado_web_service = despacho.ObtienePublicacionServicio(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvServicios.SelectedValue), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //    //Obtenemos Documento generado
        //    XDocument xDoc = XDocument.Parse(resultado_web_service);

        //    //Validamos que exista Respuesta
        //    if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //    {
        //        //Traduciendo resultado
        //        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //    }
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Obtenemos DataSet
        //        ds.ReadXml(xDoc.Document.Element("Servicio").Element("NewDataSet").CreateReader());

        //        //Validamos Origen de Datos
        //        if (Validacion.ValidaOrigenDatos(ds.Tables["Table"]))
        //        {
        //            //Establecemos Id Retorno
        //            Id_Publicacion = (from DataRow r in ds.Tables["Table"].Rows
        //                              select r.Field<int>("idServicio")).FirstOrDefault();
        //        }
        //    }

        //    else
        //    {
        //        //Establecmos Mensaje Resultado
        //        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //    }
        //    //Si Existe Error
        //    if (!resultado.OperacionExitosa)
        //    {
        //        //Mostrando Mensaje de Operación
        //        TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //    }
        //    return Id_Publicacion;
        //}

        //#region Evetos Publicación Servicio
        //#endregion
        //#endregion

        #region Eventos Publicación de Servicios


        #endregion

        #region Eventos Respuestas Servicio
        /// <summary>
        /// Evento generado al cambiar de Página de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuestaPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento genetrado al cambiar el Sort de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResultadoRespuestaPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoResultadoRespuestaPS.Text = Controles.CambiaSortExpressionGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el tamaño de Resultados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddTamanolResultadoRespuestaPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvResultadoRespuestaPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), Convert.ToInt32(ddTamanoResultadoRespuestaPS), false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Tamaño de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestasPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.NewPageIndex, false, 4);

        }

        /// <summary>
        /// Evento generado al cambiar el Sort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRespuestasPS_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoRespuestasPS.Text = Controles.CambiaSortExpressionGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.SortExpression, false, 4);

        }

        /// <summary>
        /// Eventp generado al cambiar el tamaño de Respuestas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRespuestasPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRespuestasPS, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), Convert.ToInt32(ddlTamanoRespuestasPS.SelectedValue), false, 4);

        }

        /// <summary>
        /// Evento generado al  Ver los Detalles de la Respuesta de Publicación de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerDetalleResultadoPS_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvResultadoRespuestaPS.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvResultadoRespuestaPS, sender, "lnk", false);
                //Mostrando ventana modal 
                alternaVentanaModal("opcionSeleccionRespuestaPS", gvResultadoRespuestaPS);
                //ocultamos ventana modal correspondiente
                alternaVentanaModal("resultadoRespuestaPS", gvResultadoRespuestaPS);
                //Cargamos Respuestas de una Publicación de Servicio
                //cargaRespuestasPS(gvResultadoRespuestaPS);
            }
        }

        /// <summary>
        /// Click en algún Link de GV de Respúestas de la Públicación de Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRespuestaPS_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvRespuestasPS.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvRespuestasPS, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Aceptar":
                        //limpiamos  Control de Tarifa
                        txtTarifaAceptadaPU.Text = "";//;
                        // Abriendo ventana 
                        alternaVentanaModal("aceptarRespuesta", lkb);
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuestaPS", lkb);
                        break;
                    case "Confirmar":
                        //Cerramos Modal
                        alternaVentanaModal("opcionSeleccionRespuestaPS", lkb);
                        //Cerramos Ventana modal
                        alternaVentanaModal("confirmarRespuestaPU", lkb);
                        break;
                }
            }
        }

        #endregion

        //#region Método Respuesta de Servicio
        ///// <summary>
        ///// Método encargado de cargar las Respuestas
        ///// </summary>
        ///// <param name="control"></param>
        //private void cargaResultadoRespuestaPS(System.Web.UI.Control control)
        //{
        //    //Obtenemos Depósito
        //    using (DataTable mit = consumoResultadoRespuestasPS(control))
        //    {

        //        //Validando que el DataSet contenga las tablas
        //        if (Validacion.ValidaOrigenDatos(mit))
        //        {
        //            //Cargando los GridView
        //            Controles.CargaGridView(gvResultadoRespuestaPS, mit, "Id", lblOrdenadoResultadoRespuestaPS.Text, false, 3);
        //            //Añadiendo Tablas al DataSet de Session
        //            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table8");

        //        }
        //        else
        //        {   //Inicializando GridViews
        //            Controles.InicializaGridview(gvResultadoRespuestaPS);
        //            //Eliminando Tablas del DataSet de Session
        //            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table8");
        //        }
        //    }
        //}

        ///// <summary>
        ///// Carga los Resultados de la Respuestas PS
        ///// </summary>
        ///// <param name="control"></param>
        ///// <returns></returns>
        //private DataTable consumoResultadoRespuestasPS(System.Web.UI.Control control)
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


        //    string resultado_web_service = despacho.ObtieneRespuestasServicio(consumoObtieneIdPublicacionServicio(control), true, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                     TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""));

        //    //Obtenemos Documento generado
        //    XDocument xDoc = XDocument.Parse(resultado_web_service);

        //    //Validamos que exista Respuesta
        //    if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //    {
        //        //Traduciendo resultado
        //        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //    }
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Obtenemos DataSet
        //        ds.ReadXml(xDoc.Document.Element("RespuestasServicio").Element("NewDataSet").CreateReader());
        //    }

        //    else
        //    {
        //        //Establecmos Mensaje Resultado
        //        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //    }
        //    //Si Existe Error
        //    if (!resultado.OperacionExitosa)
        //    {
        //        //Mostrando Mensaje de Operación
        //        TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //    }
        //    return ds.Tables["Table"];
        //}

        ///// <summary>
        ///// Método encargado de cargar las Respuestas de Una Publicación de Servcicio
        ///// </summary>
        //private void cargaRespuestasPS(System.Web.UI.Control control)
        //{
        //    //Obtenemos Depósito
        //    using (DataSet ds = consumoRespuestasPS(control))
        //    {

        //        //Validando que el DataSet contenga las tablas
        //        if (Validacion.ValidaOrigenDatos(ds.Tables["Table"]))
        //        {
        //            //Cargando los GridView
        //            Controles.CargaGridView(gvRespuestasPS, ds.Tables["Table"], "Id-TarifaOfertada", lblOrdenadoRespuestasPS.Text, false, 3);
        //            //Añadiendo Tablas al DataSet de Session
        //            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table9");

        //        }
        //        else
        //        {   //Inicializando GridViews
        //            Controles.InicializaGridview(gvRespuestasPS);
        //            //Eliminando Tablas del DataSet de Session
        //            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table9");
        //        }
        //    }
        //}

        ///// <summary>
        ///// Carga las Respuestas de la Publicación de Servicio
        ///// </summary>
        ///// <returns></returns>
        //private DataSet consumoRespuestasPS(System.Web.UI.Control control)
        //{
        //    //Establecemos variable resultado
        //    DataSet ds = new DataSet();
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Consumimos Web Service
        //    DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


        //    string resultado_web_service = despacho.VisualizarRespuesta(Convert.ToInt32(gvResultadoRespuestaPS.SelectedValue), ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                       TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
        //                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);

        //    //Obtenemos Documento generado
        //    XDocument xDoc = XDocument.Parse(resultado_web_service);

        //    //Validamos que exista Respuesta
        //    if (xDoc.Descendants("idRegistro").FirstOrDefault().Value != null)
        //    {
        //        //Traduciendo resultado
        //        resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //    }
        //    //Validamos Resultado
        //    if (resultado.OperacionExitosa)
        //    {
        //        //Obtenemos DataSet
        //        ds.ReadXml(xDoc.Document.Element("VisualizacionRespuesta").Element("NewDataSet").CreateReader());
        //    }

        //    else
        //    {
        //        //Establecmos Mensaje Resultado
        //        resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //    }
        //    //Si Existe Error
        //    if (!resultado.OperacionExitosa)
        //    {
        //        //Mostrando Mensaje de Operación
        //        TSDK.ASP.ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //    }
        //    return ds;
        //}

        ///// <summary>
        ///// Aceptar la Respuesta de la Publicación del Servicio
        ///// </summary>
        ///// <returns></returns>
        //private RetornoOperacion aceptarRespuestaPS()
        //{
        //    //Establecemos variable resultado
        //    RetornoOperacion resultado = null;

        //    //Consumimos Web Service
        //    DespachoCentral.DespachoClient despacho = new DespachoCentral.DespachoClient();


        //    //Instanciamos Compañia
        //    using (SAT_CL.Global.CompaniaEmisorReceptor objCompania = new CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
        //    {

        //        string resultado_web_service = despacho.AceptaRespuestaServicio(Convert.ToInt32(gvRespuestasPS.SelectedValue), Convert.ToDecimal(txtTarifaAceptadaPU.Text), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
        //                           TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, objCompania.id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
        //                           ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);


        //        //Obtenemos Documento generado
        //        XDocument xDoc = XDocument.Parse(resultado_web_service);

        //        //Validamos que exista Respuesta
        //        if (xDoc != null)
        //        {
        //            //Traduciendo resultado
        //            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

        //        }
        //        else
        //        {
        //            //Establecmos Mensaje Resultado
        //            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
        //        }
        //        //Mostrando Mensaje de Operación
        //        TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarRespuesta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        //    }
        //    return resultado;
        //}
        //#endregion
    }
}