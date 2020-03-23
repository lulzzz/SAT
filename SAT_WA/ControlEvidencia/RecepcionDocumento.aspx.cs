using SAT_CL;
using SAT_CL.ControlEvidencia;
using SAT_CL.Despacho;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Documentacion;
using System.Linq;
using System.Transactions;

namespace SAT.ControlEvidencia
{
    public partial class RecepcionDocumento : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al cargar la forma
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
            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)
            {
                //Asignando botón predeterminado
                this.Form.DefaultButton = btnBuscar.UniqueID;
                //Invoca Método de Carga de Pagina
                inicializaPagina();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMenuOperacion_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lkb = (LinkButton)sender;

            //Determianndo que elemento fue pulsado
            switch (lkb.CommandName)
            {
                case "ImpresionDocumentos":
                    {
                        //Obteniendo Ruta
                        string impresion_url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Accesorios/ImpresionDocumentos.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?&idRegistro={2}&idUsuario={3}", impresion_url, "Impresiones", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario), "Impresion Documentos", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES", Page);
                        break;
                    }
            }
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
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
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
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = false;

                //Limpiando fechas
                txtFechaInicio.Text = txtFechaFin.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            //Realizando la búsqueda de los viajes correspondientes
            cargaViajesPendientesRecepcion();

            //Inicializamos Controles
            //Cambiamos Vista
            mtvDocumentosDigitalizados.ActiveViewIndex = 1;
            //Carga Imagenes
            cargaImagenDocumentos();
            //Cambiando estilos de pestañas
            btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
            btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
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
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            recibeDocumentosViaje();
            //Generamos script para Apertura de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionRecibeDocumentos').animate({ width: 'toggle' });
            $('#confirmacionRecibeDocumentos').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptar, upbtnAceptar.GetType(), "AbreConfirmacion", script, false);

            //Inicializamos Controles
            //Cambiamos Vista
            mtvDocumentosDigitalizados.ActiveViewIndex = 1;
            //Carga Imagenes
            cargaImagenDocumentos();
            //Cambiando estilos de pestañas
            btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
            btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
        }

        /// <summary>
        /// Evento generado al recibir los Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecibir_Click(object sender, EventArgs e)
        {
           string script=
            @"<script type='text/javascript'>
            $('#contenidoConfirmacionRecibeDocumentos').animate({ width: 'toggle' });
            $('#confirmacionRecibeDocumentos').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnRecibir, upbtnRecibir.GetType(), "AbreConfirmacion", script, false);

            //Editamos la etiqueta de Confirmación
           using(SAT_CL.Documentacion.Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
           {
               //Validamos Estatus Terminado
               if(objServicio.estatus == Servicio.Estatus.Terminado)
               {
                   //Mostrando Mensaje
                   lblMensajeConfirmacionRecepcion.Text = "Al recibir los documentos el proceso no se podrá revertir.¿Desea continuar?";
               }
               else
               {
                   //Mostrando Mensaje
                   lblMensajeConfirmacionRecepcion.Text = "El servicio aún no ha sido Terminado.¿Desea continuar?";
               }
           }
        }

        #region Eventos GridView "Servicios"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoServicios_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                                        Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 3);
            //Inicializando el GridView
            Controles.InicializaGridview(gvDocumentos);
        }

        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServicios_OnClick(object sender, EventArgs e)
        {
            //Exportando el Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
            //Inicializando el GridView
            Controles.InicializaGridview(gvDocumentos);
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarServicios.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
            //Inicializando el GridView
            Controles.InicializaGridview(gvDocumentos);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de número de viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbIniciarRecepcion_Click(object sender, EventArgs e)
        {
            //Declaranmos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

            //Validamos que exista un Servicio Control Evidencia
            using (ServicioControlEvidencia objServicioControl = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Validmos que exista el Servicio Control Evidencia
                if (objServicioControl.id_servicio_control_evidencia <= 0)
                {
                    //Insertamos Servicio Control Evidencia
                    resultado = ServicioControlEvidencia.InsertaServicioControlEvidencia(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Si se actualiza correctamente
                    if (resultado.OperacionExitosa)
                        //Recargando grid para mostrar fecha de inicio de control de evidencia
                        cargaViajesPendientesRecepcionManteniendoSeleccion();

                    //Personalisando resultado con número de servicio
                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Servicio {0}: {1}", gvServicios.SelectedDataKey["IdServicio"], resultado.Mensaje), resultado.OperacionExitosa);

                    //Mostramos Mensaje de resultado
                    ScriptServer.MuestraNotificacion(gvServicios, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

            }

            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Mostrando Ventana Modal
            alternaVentanaModal("documentosEncontrados", lnk);

            //Cargando los documentos del viaje seleccionado
            cargaDocumentosViaje();

            //Cargando imagenes
            cargaImagenDocumentos();
            //Cambiamos Vista
            mtvDocumentosDigitalizados.ActiveViewIndex = 1;
            //Cambiando estilos de pestañas
            btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
            btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";

            //Habilitando recepción de documentos de viaje 
            habilitaRecepcionDocumentos();
        }
        /// <summary>
        /// Evento producido al pulsar el botón Resumen Segmentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSegmentos_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

            //Determinando el botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "Segmentos":
                    //Cargamos Resumen
                    cargaResumenSegmentos();
                    //Generamos script para Apertura de Ventana Modal
                    string script =

                        @"<script type='text/javascript'>
                     $('#contenidoResumenSegmentos').animate({ width: 'toggle' });
                $('#modalResumenSegmentos').animate({ width: 'toggle' });     
                   </script>";

                    //Registrando el script sólo para los paneles que producirán actualización del mismo
                    System.Web.UI.ScriptManager.RegisterStartupScript(upgvSegmentos, upgvSegmentos.GetType(), "AbreModalResumenSegmentos", script, false);

                    break;
                case "HacerServicio":
                    txtClienteHacerServicio.Text = "";
                    //Registrando el script sólo para los paneles que producirán actualización del mismo
                    ScriptServer.AlternarVentana(upgvSegmentos, upgvSegmentos.GetType(), "AbreVentanaModal", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");
                    break;

            }
            //Inicializamos Valores de Evidencia
            inicializaValoresControlEvidenciaDocumentoDigitalizado();

        }
        /// <summary>
        /// Evento click sobre alguno de los botónes de acciones con ventanas modales del servicio
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

                //Determinando el comando a ejecutar
                switch (lkb.CommandName)
                {
                    case "CartaPorte":
                        //Inicializando control de encabezado de servicio
                        wucEncabezadoServicio.InicializaEncabezadoServicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]));
                        //Mostrando ventana modal
                        alternaVentanaModal("encabezadoServicio", lkb);
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "ReferenciaCliente":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]));
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("referenciasServicio", lkb);
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "KmsServicio":
                        //Actualizando kilometraje de servicio
                        actualizaKilometrajeServicio();
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "KmsMovimiento":
                        //Actualizando kilometraje de movimiento
                        actualizaKilometrajeMovimiento();
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "Total":
                        //Instanciando servicio
                        using (Servicio serv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                            //Configurando rótulo
                            h2CargosServicio.InnerText = string.Format("Cargos del Servicio '{0}'", serv.no_servicio);

                            //Recuperando factura de servicio
                            using (SAT_CL.Facturacion.Facturado fact = SAT_CL.Facturacion.Facturado.ObtieneFacturaServicio(serv.id_servicio))
                                //Inicializando control de cargos de servicio
                                wucFacturadoConcepto.InicializaControl(fact.id_factura);

                            //Mostrando ventana modal
                            alternaVentanaModal("cargosServicio", lkb);
                        }
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "Tarifa":
                        //Aplicando tarifa
                        aplicaTarifaServicio();
                        //Cambiamos Vista
                        mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        break;
                    case "Estatus":
                        //Declaranmos Objeto Resultado
                        RetornoOperacion resultado = new RetornoOperacion(0);

                        //Validamos que exista un Servicio Control Evidencia
                        using (ServicioControlEvidencia objServicioControl = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                            //Validmos que exista el Servicio Control Evidencia
                            if (objServicioControl.id_servicio_control_evidencia <= 0)
                            {
                                //Insertamos Servicio Control Evidencia
                                resultado = ServicioControlEvidencia.InsertaServicioControlEvidencia(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]), Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si se actualiza correctamente
                                if (resultado.OperacionExitosa)
                                    //Recargando grid para mostrar fecha de inicio de control de evidencia
                                    cargaViajesPendientesRecepcionManteniendoSeleccion();

                                //Personalisando resultado con número de servicio
                                resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Servicio {0}: {1}", gvServicios.SelectedDataKey["IdServicio"], resultado.Mensaje), resultado.OperacionExitosa);

                                //Mostramos Mensaje de resultado
                                ScriptServer.MuestraNotificacion(gvServicios, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }

                        }
                        //Cargamos Documentos Digitalizados
                        cargaDocumentosViajeoDigitalizado();
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana";
                        //Asignando vista activa de la forma
                        mtvDocumentosDigitalizados.SetActiveView(vwRecibirDocumentosDigitalizados);
                        //Habilita Menu
                        habilitaRecepcionDocumentosDigitalizados();
                        break;
                    case "Paradas":
                        //Instanciando servicio
                        using (Servicio serv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                            if (serv.habilitar)
                            {
                                CargaParadas(serv.id_servicio);
                                alternaVentanaModal("Paradas", this.Page);
                            }

                        }
                        break;
                    case "OperacionAlcance":
                        //Instanciando servicio
                        using (Servicio serv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
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
        /// Evento Producido al Dar Click en el Link "Ver Devolución"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDevolucion_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Validando que el Servicio tenga Devoluciones
                if (Convert.ToInt32(gvServicios.SelectedDataKey["IndDevolucion"]) > 0)
                {
                    //Invocando Método de Carga
                    cargaResumenDevoluciones(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]));

                    //Alternando Ventana Modal
                    alternaVentanaModal("devolucionesServicio", lkb);
                }
                else
                {
                    //Obtiene Ultima Parada del Servicio
                    int idParada = SAT_CL.Despacho.Parada.ObtieneUltimaParada(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]));
                    int idMovimiento = 0;

                    //Validando que Exista la Parada
                    if (idParada > 0)
                    {
                        //Obteniendo Movimiento
                        idMovimiento = SAT_CL.Despacho.Movimiento.BuscamosUltimoMovimiento(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]), idParada);

                        //Validando que Exista el Movimiento
                        if (idMovimiento > 0)
                        {
                            //Inicializando Devolución
                            wucDevolucionFaltante.InicializaDevolucion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]), idMovimiento, idParada);

                            //Alternando Ventana Modal
                            alternaVentanaModal("altaDevolucion", lkb);
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, "No se encontro el ultimo movimiento", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "No se encontro la ultima parada", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                //Carga Documentos
                cargaImagenDocumentos();
                //Cambiamos Vista
                mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                //Cambiando estilos de pestañas
                btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
            }
        }

        #endregion

        #region Eventos Devoluciones

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoDev.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvDevoluciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoDev.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNoDevolucion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDevoluciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDevoluciones, sender, "lnk", false);

                //Obteniendo Control
                LinkButton lkb = (LinkButton)sender;

                //Instanciando Devolución
                using (SAT_CL.Despacho.DevolucionFaltante df = new DevolucionFaltante(Convert.ToInt32(gvDevoluciones.SelectedDataKey["IdDevolucion"])))
                {
                    //Validando que Exista la Devolución
                    if (df.habilitar)
                    {
                        //Inicializando Control
                        wucDevolucionFaltante.InicializaDevolucion(df.id_devolucion_faltante);

                        //Alternando Ventanas
                        alternaVentanaModal("altaDevolucion", lkb);
                        alternaVentanaModal("devolucionesServicio", lkb);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "Devoluciones":
                    //Exportando Contenido del GridView
                    TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
                    break;
            }
        }

        #endregion

        #region Eventos GridView "Documentos"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoDocumentos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"),
                                        Convert.ToInt32(ddlTamanoDocumentos.SelectedValue), true, 6);
        }

        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDocumentos_OnClick(object sender, EventArgs e)
        {
            //Exportando Documentos
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el Indice de pagina del GridView
            Controles.CambiaIndicePaginaGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 6);
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Segmentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSegmentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el Indice de pagina del GridView
            Controles.CambiaIndicePaginaGridView(gvSegmentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "ResumenSegmentos"), e.NewPageIndex,false,0);
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblDocumentos.Text = Controles.CambiaSortExpressionGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 6);
        }

        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvDocumentos.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Evalua el ID del CheckBox en el que se produce el cambio
                if (d.ID == "chkTodos")
                {
                    //Obtenemos su referencia 
                    using (Label l = (Label)gvDocumentos.FooterRow.FindControl("lblSeleccionadosDoc"))
                    {
                        //Actualizamos el texto de la etiqueta
                        l.Text = Controles.SeleccionaFilasTodas(gvDocumentos, "chkVarios", d.Checked).ToString();
                    }
                }
                else
                {
                    //Sumando/restando elemento afectado
                    Controles.SumaSeleccionadosFooter(d.Checked, gvDocumentos, "lblSeleccionadosDoc");

                    //Si retiró selección
                    if (!d.Checked)
                    {
                        //Referenciando control de encabezado
                        CheckBox t = (CheckBox)gvDocumentos.HeaderRow.FindControl("chkTodos");
                        //Aplicando marcado de elemento
                        t.Checked = d.Checked;
                    }
                }
            }
        }

        #endregion

        #endregion 

        #region Métodos


        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {          
            //Instanciamos Compañia  para visualización en el control
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" +  objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
            }

            //Invocando Método de carga
            cargaCatalogos();

            //Validamos que exista la Clave
            if (((SAT_CL.Seguridad.Usuario)Session["usuario"]).Configuracion.ContainsKey("Terminal Control Evidencias"))
            {
                //Asignando valores de catálogos según perfil de recepción
                using (SAT_CL.Global.Ubicacion i = new SAT_CL.Global.Ubicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).Configuracion["Terminal Control Evidencias"], ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    //Terminal del usuario
                    ddlTerminal.SelectedValue = i.id_ubicacion.ToString();
                }
            }
            else
            {
                //Terminal del usuario Sin Asignar
                ddlTerminal.SelectedValue = "0";
            }

            //Inicializando los GridView
            Controles.InicializaGridview(gvDocumentos);
            Controles.InicializaGridview(gvServicios);
            //Inicializando los GridView
            Controles.InicializaGridview(gvDocumentosDigitalizados);

            //Indicando listado de imagenes muestra a visualizar
            rdbVerReales.Checked = true; 
            rdbVerEjemplos.Checked = false;

            //Control de Inicio
            ddlTerminal.Focus();
            //Che
            chkSoloServicios.Checked = true;
        }

        /// <summary>
        /// Método encargado de los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            int idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
            //Tamaño GV Viajes
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServicios, "", 26);
            //Tamaño GV Documentos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentos, "", 18);
            //Tamaño GV Devoluciones
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDev, "", 26);
            //Tamaño GV Documentos Digitalizados
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentosDigitalizados, "", 18);
            /** CLASIFICACIÓN DEL VIAJE **/
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxTerminal, 9, "TODOS", idCompania, "", 0, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxOperacionServicio, 3, "TODOS", idCompania, "", 6, "");
            CapaNegocio.m_capaNegocio.CargaCatalogo(lbxAlcance, 3, "TODOS", idCompania, "", 5, "");
            //Terminales
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTerminal, 9, "Sin Asignar", idCompania, "", 0, "");
            //Grid Paradas
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewAnticipos, "", 18);
        }

        /// <summary>    
        /// Realiza la carga de los viajes que aún tienen documentos por recibir
        /// </summary>
        private void cargaViajesPendientesRecepcion()
        {
            //Declarando variables para rangos de fecha
            DateTime inicial_cita_carga, final_cita_carga, inicial_cita_descarga, final_cita_descarga,
                     inicial_inicio_servicio, final_inicio_servicio, inicial_fin_servicio, final_fin_servicio;
            //Inicializando Valores
            inicial_cita_carga = final_cita_carga = inicial_cita_descarga = final_cita_descarga = DateTime.MinValue;
            inicial_inicio_servicio = final_inicio_servicio = inicial_fin_servicio = final_fin_servicio = DateTime.MinValue;

            //Determinando que criterio será utilizado
            if (chkRangoFechas.Checked)
            {
                //Cita carga
                if (rdbCitaCarga.Checked)
                {
                    inicial_cita_carga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_carga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Cita Descarga
                else if (rdbCitaDescarga.Checked)
                {
                    inicial_cita_descarga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_descarga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Inicio de Servicio
                else if (rdbInicioServicio.Checked)
                {
                    inicial_inicio_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_inicio_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Fin de Servicio
                else
                {
                    inicial_fin_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_fin_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
            }

            //Obteniendo Filtros de Clasificación
            string operacion = Controles.RegresaSelectedValuesListBox(lbxOperacionServicio, "{0},", true, false);
            string alcance = Controles.RegresaSelectedValuesListBox(lbxAlcance, "{0},", true, false);
            string terminal = Controles.RegresaSelectedValuesListBox(lbxTerminal, "{0},", true, false);

            //Inicializando indices de selección
            Controles.InicializaIndices(gvServicios);

            //Obteniendo viajes pendientes
            using (DataTable dt = SAT_CL.ControlEvidencia.Reportes.ObtienePendientesRecepcionEvidencia(txtNoServicio.Text, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                  Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1), "0")), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1), "0")), 
                                  txtReferencia.Text, chkSoloServicios.Checked, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), txtPorte.Text, inicial_cita_carga, final_cita_carga, inicial_cita_descarga, final_cita_descarga,
                                  inicial_inicio_servicio, final_inicio_servicio, inicial_fin_servicio, final_fin_servicio, operacion.Length > 1 ? operacion.Substring(0, operacion.Length - 1) : "", 
                                  terminal.Length > 1 ? terminal.Substring(0, terminal.Length - 1) : "", alcance.Length > 1 ? alcance.Substring(0, alcance.Length - 1) : ""))
            {
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvServicios, dt, "IdServicio-IdServicioControlEvidencia-Viaje-IdMovimiento-IndDevolucion", lblOrdenarServicios.Text, true, 3);

                //Validando que la Tabla Contenga Registros
                if (dt != null)
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table");
                //De lo contrario
                else
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Borrando Documentos de sesión
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            //Limpiando GridView de Documentos
            Controles.InicializaGridview(gvDocumentos);
        }
        /// <summary>    
        /// Realiza la carga de los viajes que aún tienen documentos por recibir, manteniendo la selección actual de registro
        /// </summary>
        private void cargaViajesPendientesRecepcionManteniendoSeleccion()
        {
            //Guardando selección actual de Id de Servicio
            string id_servicio = gvServicios.SelectedDataKey["IdServicio"].ToString();
            //Cargando Grid de pendientes
            cargaViajesPendientesRecepcion();

            //Aplicando selección Previa
            if (id_servicio != "")
                Controles.MarcaFila(gvServicios, id_servicio, "IdServicio", "IdServicio-IdServicioControlEvidencia-Viaje-IdMovimiento", (DataSet)Session["DS"], "Table", lblOrdenarServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 3);
        }

        /// <summary>
        /// Realzia la carga de los viajes que aún tienen documentos por recibir
        /// </summary>
        private void cargaDocumentosViaje()
        {
            //Inicializando indice de selección
            Controles.InicializaIndices(gvDocumentos);

            //Obteniendo detalles de viaje
            using (DataTable dt = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]),
                                          Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1), "0")), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1), "0"))))
            {
                //Validando que la Tabla Contenga Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView de Viajes
                    Controles.CargaGridView(gvDocumentos, dt, "Id-IdHID-Documento-IdLugarCobro-IdSegmento-IdSegmentoControlEvidencia-Estatus", lblDocumentos.Text, true, 6);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");
                }
                else
                {
                    //Inicializando gridView
                    Controles.InicializaGridview(gvDocumentos);
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Realzia la carga de los viajes que aún tienen documentos por recibir
        /// </summary>
        private void cargaDocumentosViajeoDigitalizado()
        {
            //Inicializando indice de selección
            Controles.InicializaIndices(gvDocumentosDigitalizados);

            //Obteniendo detalles de viaje
            using (DataTable dt = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]),
                                          Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1), "0")), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1), "0"))))
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
        /// Realiza la carga de la galería de imagenes
        /// </summary>
        private void cargaImagenDocumentos()
        { 
            //Vista previa por default
            imgImagenZoom.ImageUrl = "~/Image/noDisponible.jpg";
            hplImagenZoom.NavigateUrl = "~/Image/noDisponible.jpg";

            //Si no hay viaje seleccionado
            if (gvServicios.SelectedIndex == -1)
                //Cargando lista vacía
                Controles.CargaDataList(dtlImagenDocumentos, null, "URL", "", "");
            else
            {
                //Origen de datos vacío
                DataTable mit = null;

                //Si la carga es en base a documentos reales de la orden
                if (rdbVerReales.Checked)
                    //Realizando la carga de URL de imagenes a mostrar
                    mit = ControlEvidenciaDocumento.ObtieneControlEvidenciaDocumentosImagenes(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]));
                //Si la carga es con ejemplos
                else
                    //Realizando la carga de URL de imagenes a mostrar
                    mit = HojaInstruccionDocumento.ObtieneHojasDeInstruccionesDocumentosImagenes(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicioControlEvidencia"]));

                //Cargando DataList
                Controles.CargaDataList(dtlImagenDocumentos, mit, "URL", "", "");
            }
        }

        /// <summary>
        /// Determina si es posible realizar la recepcón de documentos del viaje
        /// </summary>
        private void habilitaRecepcionDocumentos()
        {
            //Instanciando COntrol de Evidencia
            using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Si el estatus del registro es "No Recibido" o "En Aclaración"
                if (ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.En_Aclaracion ||
                    ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.No_Recibidos ||
                     ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.Imagen_Digitalizada)
                {
                    //Habilitando botón de recepción
                    btnRecibir.Enabled = true;
                }
                else
                    btnRecibir.Enabled = false;
            }
        }

        /// <summary>
        /// Inicializamos Vlores de Imagen Digitalizada
        /// </summary>
        private void inicializaImagenDigitalizada()
        {

        }
        /// <summary>
        /// Realiza la recepción de documentos del viaje seleccionado
        /// </summary>
        private void recibeDocumentosViaje()
        {
            //Validamos que exista Terminal de Cobro
            if (ddlTerminal.SelectedValue != "0")
            {
                //Validando que existan Registros en el GridView
                if (gvDocumentos.DataKeys.Count > 0)
                {
                    //Obteniendo Filas Seleccionadas
                    GridViewRow[] gvFilas = Controles.ObtenerFilasSeleccionadas(gvDocumentos, "chkVarios");

                    //Validando que Existan Filas Seleccionadas
                    if (gvFilas.Length > 0)
                    {
                        //Manteniendo el Id de viaje al que pertenecen los documentos
                        string id_viaje = gvServicios.SelectedDataKey["IdServicio"].ToString();

                        //Declarando variable de resultado
                        RetornoOperacion result = new RetornoOperacion((Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])));
                        // Instacniamos Control Evidencia
                        using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                            //Recorriendo los Documentos
                            foreach (GridViewRow doc in gvFilas)
                            {
                                //Seleccionando Indice de la Fila actual
                                gvDocumentos.SelectedIndex = doc.RowIndex;

                                 
                                //Validando si el documento no se ha recibido anteriormente (La fila cuenta con Id diferente de 0)
                                if (gvDocumentos.SelectedDataKey["Id"].ToString() == "0" || gvDocumentos.SelectedDataKey["Estatus"].ToString() == "Imagen Digitalizada")
                                {
                                    //Definiendo estatus de documentos por recibir
                                    //(Lugar de cobro igual a terminal de usuario, "Recibido", de lo contrario "Reenvío")
                                    ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ddlTerminal.SelectedValue == gvDocumentos.SelectedDataKey["IdLugarCobro"].ToString() ? ControlEvidenciaDocumento.EstatusDocumento.Recibido : ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio;

                                    //Instanciando Hoja de Instruccion Documento
                                    using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(Convert.ToInt32(gvDocumentos.SelectedDataKey["IdHID"])))
                                    {
                                        //Declarando Variables de Obtención de Formatos
                                        //(Si es original, se niega valor de copia)
                                        bool bit_copia = hid.id_copia_original == 1 ? false : true;
                                        //(Si es copia, se nieva valor de original)
                                        bool bit_original = hid.id_copia_original == 2 ? false : true;
                                        //Si no Existe Evidencia Control Documento
                                        if (gvDocumentos.SelectedDataKey["Id"].ToString() == "0")
                                        {

                                            //Realizando actualización
                                            result = ControlEvidenciaDocumento.InsertaControlEvidenciaDocumento(ce.id_servicio_control_evidencia, ce.id_servicio, Convert.ToInt32(gvDocumentos.SelectedDataKey["IdSegmentoControlEvidencia"]), Convert.ToInt32(gvDocumentos.SelectedDataKey["IdSegmento"]), (byte)hid.id_tipo_documento, estatus_documento,
                                                                                    hid.id_hoja_instruccion_documento, Convert.ToInt32(ddlTerminal.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                                    Convert.ToInt32(gvDocumentos.SelectedDataKey["IdLugarCobro"]), bit_original, bit_copia,
                                                                                    hid.bit_sello, 0, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        }
                                        else
                                        {
                                            //Instnaciamos Control Evidencia Documento
                                            using (ControlEvidenciaDocumento objDocumento = new ControlEvidenciaDocumento(Convert.ToInt32(gvDocumentos.SelectedDataKey["Id"])))
                                            {
                                                //Editamos Estatus de Documento a Recibido
                                                result = objDocumento.EditaControlEvidenciaDocumentoRecibido(DateTime.Now, Convert.ToInt32(ddlTerminal.SelectedValue), Convert.ToInt32(gvDocumentos.SelectedDataKey["IdLugarCobro"]),
                                                    estatus_documento,((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                        //Instanciamos Segmento para obtenener información de las paradas
                                        using (SegmentoCarga objSegmento = new SegmentoCarga(Convert.ToInt32(gvDocumentos.SelectedDataKey["IdSegmento"])))
                                        {
                                            //Instanciamos Parada de Inicio y Parada Fin
                                            using (Parada objParadaInicio = new Parada(objSegmento.id_parada_inicio), objParadaFin = new Parada(objSegmento.id_parada_fin))
                                            {
                                                //Añadiendo resultado a mensaje final
                                                result = new RetornoOperacion(result.IdRegistro, string.Format("{0}: {1}", gvDocumentos.SelectedDataKey["Documento"] + " [" + objParadaInicio.descripcion + "-" + objParadaFin.descripcion
                                                + "] ", result.Mensaje), result.OperacionExitosa);
                                                
                                                //Mostrando mensaje
                                                ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Definiendo estatus de documentos por recibir
                                    //(Lugar de cobro igual a terminal de usuario, "Recibido", de lo contrario "Reenvío")
                                    ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ddlTerminal.SelectedValue == gvDocumentos.SelectedDataKey["IdLugarCobro"].ToString() ? ControlEvidenciaDocumento.EstatusDocumento.Recibido : ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio;

                                    //Instanciando Evidencia
                                    using (ControlEvidenciaDocumento evidencia = new ControlEvidenciaDocumento(Convert.ToInt32(gvDocumentos.SelectedDataKey["Id"])))
                                    {
                                        //Validando que exista la Evidencia
                                        if (evidencia.habilitar)
                                        {
                                            //Validando que el Estatus este en Aclaración
                                            if ((ControlEvidenciaDocumento.EstatusDocumento)evidencia.id_estatus_documento == ControlEvidenciaDocumento.EstatusDocumento.Imagen_Digitalizada)
                                            {
                                                //Validando que no este en un Paquete
                                                if (!PaqueteEnvioDocumento.ValidaDocumentoPaqueteDetalle(evidencia.id_control_evidencia_documento))

                                                    //Actualizando Estatus
                                                    result = evidencia.ActualizaEstatusControlEvidenciaDocumento(Convert.ToInt32(ddlTerminal.SelectedValue), estatus_documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El Documento se encuentra en un Paquete");
                                            }
                                            else
                                            {
                                                //Validando Estatus para Excepción
                                                switch((ControlEvidenciaDocumento.EstatusDocumento)evidencia.id_estatus_documento)
                                                {
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Recibido:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento se encuentra Recibido");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.No_Recibido:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento no esta Recibido");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Cancelado:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento no esta Cancelado");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento esta Recibido con un Reenvio pendiente");
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Actualizando estatus de Control de evidencia
                            result = ce.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Mostrando resultado principal
                            result = new RetornoOperacion(result.IdRegistro, string.Format("* Servicio {0}: Act. Estatus - {1}", gvServicios.SelectedDataKey["Viaje"], result.Mensaje), result.OperacionExitosa);
                            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            //Cargando GV de viajes
                            cargaViajesPendientesRecepcion();

                            //Seleccionando registro anteriormente marcado
                            Controles.MarcaFila(gvServicios, id_viaje, "IdServicio");

                            //Si se logró seleccionar el registro
                            if (gvServicios.SelectedIndex != -1)
                                //Cargando documentos
                                cargaDocumentosViaje();
                        }
                    }
                }
            }
            else
                ScriptServer.MuestraNotificacion(btnAceptar, "No existe asignación de la Terminal de Cobro", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Realiza la recepción de documentos del viaje seleccionado
        /// </summary>
        private RetornoOperacion recibeDocumentoViajeImagenDigitalizacion(object sender)
        {
            //Declarmos Valor Retorno
            RetornoOperacion evidenciaDocumento = new RetornoOperacion(0);
            //Validamos que exista Terminal de Cobro
            if (ddlTerminal.SelectedValue != "0")
            {
                //Validando que existan Registros en el GridView
                if (gvDocumentosDigitalizados.DataKeys.Count > 0)
                {
                    //Seleccionando la fila correspondiente
                    Controles.SeleccionaFila(gvDocumentosDigitalizados, sender, "lnk", false);

                    //Validando que Existan Filas Seleccionadas
                    if (gvDocumentosDigitalizados.SelectedIndex != -1)
                    {
                        //Manteniendo el Id de viaje al que pertenecen los documentos
                        string id_viaje = gvServicios.SelectedDataKey["IdServicio"].ToString();

                        //Declarando variable de resultado
                        RetornoOperacion result = new RetornoOperacion((Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])));
                        // Instacniamos Control Evidencia
                        using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                                //Validando si el documento no se ha recibido anteriormente (La fila cuenta con Id diferente de 0)
                            if (gvDocumentosDigitalizados.SelectedDataKey["Id"].ToString() == "0")
                            {
                                //Creamos la transacción 
                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Definiendo estatus de documentos por recibir
                                    //Imagen Digitalizada
                                    ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ControlEvidenciaDocumento.EstatusDocumento.Imagen_Digitalizada;

                                    //Instanciando Hoja de Instruccion Documento
                                    using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdHID"])))
                                    {
                                        //Declarando Variables de Obtención de Formatos
                                        //(Si es original, se niega valor de copia)
                                        bool bit_copia = hid.id_copia_original == 1 ? false : true;
                                        //(Si es copia, se nieva valor de original)
                                        bool bit_original = hid.id_copia_original == 2 ? false : true;

                                        //Realizando actualización
                                        result = ControlEvidenciaDocumento.InsertaControlEvidenciaDocumento(ce.id_servicio_control_evidencia, ce.id_servicio, Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmentoControlEvidencia"]),
                                                Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmento"]), (byte)hid.id_tipo_documento, estatus_documento,
                                                                                hid.id_hoja_instruccion_documento, 0, DateTime.MinValue,
                                                                                0, bit_original, bit_copia,
                                                                                hid.bit_sello, 0, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Asignamos Valor de Eviedncia Documento
                                        evidenciaDocumento = result;
                                        //Instanciamos Segmento para obtenener información de las paradas
                                        using (SegmentoCarga objSegmento = new SegmentoCarga(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmento"])))
                                        {
                                            //Instanciamos Parada de Inicio y Parada Fin
                                            using (Parada objParadaInicio = new Parada(objSegmento.id_parada_inicio), objParadaFin = new Parada(objSegmento.id_parada_fin))
                                            {
                                                //Añadiendo resultado a mensaje final
                                                result = new RetornoOperacion(result.IdRegistro, string.Format("{0}: {1}", gvDocumentosDigitalizados.SelectedDataKey["Documento"] + " [" + objParadaInicio.descripcion + "-" + objParadaFin.descripcion
                                                + "] ", result.Mensaje), result.OperacionExitosa);

                                                //Mostrando mensaje
                                                ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                        }
                                    }


                                    //Actualizando estatus de Control de evidencia
                                    result = ce.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validamos Transaccion
                                    if(result.OperacionExitosa)
                                    {
                                        //Finalizamos Transacions
                                        scope.Complete();
                                    }
                                }
                            }
                            else
                            {
                                //Mostramos Id de Documento
                                evidenciaDocumento = new RetornoOperacion(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["Id"]));
                            }
                                    //Mostrando resultado principal
                            result = new RetornoOperacion(result.IdRegistro, string.Format("* Servicio {0}: Act. Estatus - {1}", gvServicios.SelectedDataKey["Viaje"], result.Mensaje), result.OperacionExitosa);
                            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            //Cargando GV de viajes
                            cargaViajesPendientesRecepcion();

                            //Seleccionando registro anteriormente marcado
                            Controles.MarcaFila(gvServicios, id_viaje, "IdServicio");

                            //Si se logró seleccionar el registro
                            if (gvServicios.SelectedIndex != -1)
                                //Cargando documentos
                                cargaDocumentosViajeoDigitalizado();
                        }
                    }
                }
            }
            else
                ScriptServer.MuestraNotificacion(btnAceptar, "No existe asignación de la Terminal de Cobro", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Devolvemos Valor
            return evidenciaDocumento;
        }

        /// <summary>
        /// Realiza la carga del Resumen de los Segmentos
        /// </summary>
        private void cargaResumenSegmentos()
        {
            //Obteniendo detalles de viaje
            using (DataTable dt = SegmentoControlEvidencia.CargaResumenSegmentos(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Validando que la Tabla Contenga Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando GridView de Viajes
                    Controles.CargaGridView(gvSegmentos, dt, "Id", lblDocumentos.Text, false,0);
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "ResumenSegmentos");
                }
                else
                {
                    //Inicializando gridView
                    Controles.InicializaGridview(gvSegmentos);
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "ResumenSegmentos");
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
            //Validando Registro
            if (id_registro > 0)
            {
                //Construyendo URL 
                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                //Abriendo Nueva Ventana
                ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe un registro"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        private void inicializaReferencias(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Validando Registro
            if (id_registro > 0)
            {
                //Construyendo URL 
                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                //Abriendo Nueva Ventana
                ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe un registro"), ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            //Validando Registro
            if (id_registro > 0)
            {
                //Construyendo URL 
                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&idTV=" + id_configuracion_tipo_archivo + "&tB=" + nombre_tabla + "&actualizaPadre=" + false);
                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                //Abriendo Nueva Ventana
                ScriptServer.AbreNuevaVentana(url, "Imagenes", configuracion, Page);
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No existe un registro"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos y Eventos GV Servicios (Acciones modal)
        
        

        /// <summary>
        /// Click en botón cerrar de ventanas modales de acciones de servicio
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
                case "ReferenciasServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("referenciasServicio", lkbCerrar);

                    //Validando Argumento
                    switch (lkbCerrar.CommandArgument)
                    {
                        case "Devolucion":
                            alternaVentanaModal("altaDevolucion", lkbCerrar);
                            break;
                    }
                    break;
                case "EncabezadoServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("encabezadoServicio", lkbCerrar);
                    break;
                case "KilometrajeMovimiento":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("kilometrajeMovimiento", lkbCerrar);
                    break;
                case "CargosServicio":
                    alternaVentanaModal("cargosServicio", lkbCerrar);
                    break;
                case "DocumentosEncontrados":
                    alternaVentanaModal("documentosEncontrados", lkbCerrar);
                    break;
                case "Devolucion":
                    alternaVentanaModal("altaDevolucion", lkbCerrar);
                    break;
                case "DevolucionesServicio":
                    alternaVentanaModal("devolucionesServicio", lkbCerrar);
                    break;
                case "Paradas":
                    alternaVentanaModal("Paradas", lkbCerrar);
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
        private void alternaVentanaModal(string nombre_ventana, Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {                                
                case "referenciasServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "referenciasServicioModal", "referenciasServicio");
                    break;
                case "encabezadoServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "encabezadoServicioModal", "encabezadoServicio");
                    break;
                case "kilometrajeMovimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "kilometrajeMovimientoModal", "kilometrajeMovimiento");
                    break;
                case "cargosServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "cargosServicioModal", "cargosServicio");
                    break;
                case "documentosEncontrados":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaDocumentosEncontrados", "ventanaDocumentosEncontrados");
                    break;
                case "altaDevolucion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalDevolucionFaltante", "devolucionFaltante");
                    break;
                case "devolucionesServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaResumenDevoluciones", "ventanaResumenDevoluciones");
                    break;
                case "Paradas":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVerAnticipos", "ventanaVerAnticipos");
                    break;
                case "Clasificacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorClasificacion", "ventanaClasificacion");
                    break;
            }
        }

        /// <summary>
        /// Enlace a datos de cada fila de gridview servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Para filas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    /**** APLICANDO CONFIGURACIÓN DE RECEPCIÓN MÚLTIPLE ****/
                    //Encontrando controles de interés
                    CheckBox chkFila = (CheckBox)e.Row.FindControl("chkSeleccionarServicio");
                    //Asignando valor predeterminado de visibilidad de control
                    chkFila.Visible = false;

                    //Si es de servicio
                    if (row.Field<int>("IdServicio") > 0)
                    { 
                        //Instanciando servicio
                        using (Servicio srv = new Servicio((row.Field<int>("IdServicio"))))
                        {
                            //Si el servicio está terminado
                            if(srv.estatus == Servicio.Estatus.Terminado)
                                chkFila.Visible = true;
                        }
                    }


                    using (LinkButton lkbCalcular = (LinkButton)e.Row.FindControl("lkbTarifaServ"),
                        lkbKms = (LinkButton)e.Row.FindControl("lkbKmsServ"), lkbNoFac = (LinkButton)e.Row.FindControl("lkbNoFacturable"))
                    {
                        /**** APLICANDO CONFIGURACIÓN DE CÁLCULO DE TARIFA Y SERVICIO NO FACTURABLE ****/
                        //Si es un servicio
                        if (row.Field<int>("IdServicio") > 0)                        
                            //Mostrando link de tarifa
                            lkbCalcular.Visible = 
                            lkbNoFac.Visible = true;
                        //no es servicio
                        else
                            //Ocultando link
                            lkbCalcular.Visible = 
                            lkbNoFac.Visible = false;

                        /**** APLICANDO CONFIGURACIÓN DE CÁLCULO DE KILOMETRAJE ****/
                        //Si es un servicio
                        if (row.Field<int>("IdServicio") > 0)
                            lkbKms.CommandName = "KmsServicio";
                        //no es servicio
                        else
                            lkbKms.CommandName = "KmsMovimiento";
                    }

                     //Encontrando controles de interés de Servicio
                    using (LinkButton lkbServicio = (LinkButton)e.Row.FindControl("lkbSegmentos"))
                    {
                        //Si no existe el Servicio
                        if(lkbServicio.Text ==" ")
                        {
                            //Asignamos Valor Default
                            lkbServicio.Text = "Hacer Servicio";
                            lkbServicio.CommandName = "HacerServicio";
                        }
                    }

                    //Encontrando controles de interés de Servicio
                    using (LinkButton lkbDevolucion = (LinkButton)e.Row.FindControl("lkbDevolucion"))
                    {
                        //Si es un servicio
                        if (row.Field<int>("IdServicio") > 0)

                            //Mostrando Control
                            lkbDevolucion.Visible = true;
                        else
                            //Ocultando Control
                            lkbDevolucion.Visible = false;
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "No Facturable"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNoFacturable_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServicios.DataKeys.Count > 0)
            {                
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);
                //Abre ventana modal
                ScriptServer.AlternarVentana(upgvServicios, gvServicios.GetType(), "AbrirVentana", "confirmacionNoFacturable", "NoFacturable");
                //Limpiamos Control
                txtMotivoNoFacturable.Text="";
                //Carga Documentos
                cargaImagenDocumentos();
                //Cambiamos Vista
                mtvDocumentosDigitalizados.ActiveViewIndex = 1;
                //Cambiando estilos de pestañas
                btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";

            }
        }
        /// <summary>
        /// Evento que cierra la ventana modal que confirma si un servicio es facturable o no al dar clic en el link Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarNoFacturable_Click(object sender, EventArgs e)
        {
            //Cierra la ventana modal
            ScriptServer.AlternarVentana(lnkCerrarNoFacturable, "CerrarVentana", "confirmacionNoFacturable", "NoFacturable");
        }
        /// <summary>
        /// Evento que cambia el estado de un servicio a No facturable .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoFacturable_Click(object sender, EventArgs e)
        {
            //valida que existan registros en el GridView
            if (gvServicios.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Servicio
                using (SAT_CL.Facturacion.Facturado fac = SAT_CL.Facturacion.Facturado.ObtieneFacturaServicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                {
                    //Validando que exista la Factura
                    if (fac.id_factura > 0)

                        //Actualizando Estatus a "No Facturable"
                        result = fac.ActualizaEstatusNoFacturable(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, txtMotivoNoFacturable.Text);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No Existe la Factura");

                    //Validando que la Operación fuese Exitosa
                    if (result.OperacionExitosa)

                        //Cargando Viajes
                        cargaViajesPendientesRecepcion();

                    //Mostrando Mensaje
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            //Cierra la ventana modal
            ScriptServer.AlternarVentana(btnNoFacturable, "CerrarVentana", "confirmacionNoFacturable", "NoFacturable");
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
                cargaViajesPendientesRecepcionManteniendoSeleccion();

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
                cargaViajesPendientesRecepcionManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Kilometraje (No está en uso total)

        /// <summary>
        /// Clic Guardar en Control de Kilometraje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucKilometraje_ClickGuardar(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            RetornoOperacion resultado = wucKilometraje.GuardaKilometraje();

            //Validando que la Operación fuese Exitosa
            if (resultado.OperacionExitosa)
            {
                //Validando de nueva cuenta el calculo apropiado de kilometraje en todos los movimientos del servicio
                resultado = actualizaKilometrajeMovimientoAMovimiento();
                //Si no hay error
                if (resultado.OperacionExitosa)
                    //Cerrando ventana de captura de kilometraje
                    alternaVentanaModal("kilometrajeMovimiento", this);
            }
        }
        /// <summary>
        /// Actualiza el kilometraje del movimiento y su servicio en caso de tenerlo, haciendo actualización por cada mov del servicio
        /// </summary>
        private RetornoOperacion actualizaKilometrajeMovimientoAMovimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Cargando movimientos del servicio
            using (DataTable mitMovimientos = Movimiento.CargaMovimientos(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Si hay elementos devueltos
                if (mitMovimientos != null)
                {
                    //Instanciando servicio en cuestión
                    using (Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                    {
                        //Para cada movimiento encontrado
                        foreach (DataRow m in mitMovimientos.Rows)
                        {
                            //Instanciando Movimiento actual en actualización
                            using (Movimiento mov = new Movimiento(m.Field<int>("Id")))
                            {
                                //Realizamos la actualizacion del kilometraje del movimiento y del servicio
                                resultado = objServicio.CalculaKilometrajeServicio(mov.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Determinando acción a realizar
                                switch (resultado.IdRegistro)
                                {
                                    //Caso en el que no hubo cambios en el kilometraje
                                    case -50:
                                    //Actualización correcta
                                    default:
                                        resultado = new RetornoOperacion(resultado.IdRegistro == -50 ? mov.id_movimiento: resultado.IdRegistro, string.Format("Mov. {0}: {1}", mov.id_movimiento, resultado.Mensaje), resultado.OperacionExitosa);
                                        break;
                                    //Caso en el que no se encontro el kilometraje
                                    case -100:
                                        using (Parada origen = new Parada(mov.id_parada_origen), destino = new Parada(mov.id_parada_destino))
                                        {
                                            //Guardando información de movimiento por actualizar
                                            lkbCerrarKilometrajeMovimiento.CommandArgument = mov.id_movimiento.ToString();
                                            //Inicializando Control
                                            wucKilometraje.InicializaControlKilometraje(0, mov.id_compania_emisor, origen.id_ubicacion, destino.id_ubicacion);
                                            //Mostrando ventana de captura de kilometraje
                                            alternaVentanaModal("kilometrajeMovimiento", gvServicios);
                                            //Señalando resultado en mensaje
                                            resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Mov. {0}: {1}", mov.id_movimiento, resultado.Mensaje), resultado.OperacionExitosa);
                                        }
                                        break;
                                }

                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                //Si hay error, se interrumpe ciclo de actualización
                                if (!resultado.OperacionExitosa)
                                    break;
                            }
                        }
                    }
                }
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando listado de servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Actualiza el kilometraje del movimiento en caso de existir
        /// </summary>
        private void actualizaKilometrajeMovimiento()
        {
            //Realizamos la actualizacion del kilometraje del movimiento y del servicio
            RetornoOperacion resultado = Movimiento.ActualizaKilometrajeMovimiento(Convert.ToInt32(gvServicios.SelectedDataKey["IdMovimiento"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Determinando acción a realizar
            switch (resultado.IdRegistro)
            {
                //Caso en el que no hubo cambios en el kilometraje
                case -50:
                //Actualización correcta
                default:
                    resultado = new RetornoOperacion(resultado.IdRegistro == -50 ? Convert.ToInt32(gvServicios.SelectedDataKey["IdMovimiento"]) : resultado.IdRegistro, string.Format("Mov. {0}: {1}", gvServicios.SelectedDataKey["IdMovimiento"], resultado.Mensaje), resultado.OperacionExitosa);
                    break;
                //Caso en el que no se encontro el kilometraje
                case -100:
                    //Señalando resultado en mensaje
                    resultado = new RetornoOperacion(resultado.IdRegistro, string.Format("Mov. {0}: {1}", gvServicios.SelectedDataKey["IdMovimiento"], resultado.Mensaje), resultado.OperacionExitosa);
                    break;
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando listado de servicios
                cargaViajesPendientesRecepcion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza el kilometraje del servicio en caso de existir
        /// </summary>
        private void actualizaKilometrajeServicio()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();
  
            //Instanciando servicio en cuestión
            using (Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                resultado = objServicio.CalculaKilometrajeServicio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando listado de servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Referencias Encabezado Servicio

        /// <summary>
        /// Evento Producido al Guardar las Referencias del Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEncabezadoServicio_ClickGuardarReferencia(object sender, EventArgs e)
        {           
                //Guardando Referencias
                wucEncabezadoServicio.GuardaEncabezadoServicio();
            
                //Cerrando Ventana Modal
                alternaVentanaModal("encabezadoServicio", this);

                //Cargando servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();
            
        }

        #endregion

        #region Cargos de Servicio

        /// <summary>
        /// Guardar cargo
        /// </summary>
        protected void wucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Realizando guardado
            RetornoOperacion resultado = wucFacturadoConcepto.GuardarFacturaConcepto();

            //Si se guardó correctamente
            if (resultado.OperacionExitosa)
            { 
                //Cerrando ventana modal
                //alternaVentanaModal("cargosServicio", this);
                //Actualizando lista de servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Eliminar cargo
        /// </summary>
        protected void wucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Realizando guardado
            RetornoOperacion resultado = wucFacturadoConcepto.EliminaFacturaConcepto();

            //Si se guardó correctamente
            if (resultado.OperacionExitosa)
            {
                //Cerrando ventana modal
                //alternaVentanaModal("cargosServicio", this);
                
                //Actualizando lista de servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Tarifa

        /// <summary>
        /// Realiza la búsqueda y aplicación de la tarifa correspondiente sobre el servicio seleccionado
        /// </summary>
        /// <returns></returns>
        private void aplicaTarifaServicio()
        {
            //Declarando objeto de retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando factura del servicio seleccionado
            using (SAT_CL.Facturacion.Facturado objFacturado = SAT_CL.Facturacion.Facturado.ObtieneFacturaServicio(Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Validando que exista un Id de factura
                if (objFacturado.id_factura != 0)
                    //Obteniendo Resultado de la Edición
                    resultado = objFacturado.ActualizaTarifa(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    resultado = new RetornoOperacion("El servicio no tiene un encabezado de factura o no pudo ser recuperado.");
            }

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualizando listado de servicios
                cargaViajesPendientesRecepcionManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Hacer Servicio

        #region Eventos Hacer Servicio

        /// <summary>
        /// Evento generado al pulsar el botón Aceptar Hacer Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarHacerServicio_Click(object sender, EventArgs e)
        {
            //Evento generado al Hacer un Servicio un Movimiento en Vacio
            hacerServicio();
        }

        /// <summary>
        /// Evento generado al Cerrar el Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarHacerServicio_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(uplkbCerrarHacerServicio, uplkbCerrarHacerServicio.GetType(), "CerrarVentanaModalHacerServicio", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");

        }
        #endregion

        #region Métodos Hacer Servicio

        /// <summary>
        /// Método encargado de Hacer un Servicio un Movimiento en Vacio
        /// </summary>
        private void hacerServicio()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvServicios.SelectedDataKey["IdMovimiento"])))
            {
                //Hacemos Servicio el Movimiento
                resultado = objMovimiento.HacerMovimientoVacioaServicio(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteHacerServicio.Text, ":", 1)), 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Viajes Pendientes Manteniendo la Selecciòn
                cargaViajesPendientesRecepcion();
                //Inicializamos Control Documentos
                Controles.InicializaGridview(gvDocumentos);
                Controles.InicializaIndices(gvServicios);
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(btnAceptarHacerServicio, btnAceptarHacerServicio.GetType(), "AceptarVentanaModalHacerServicio", "contenidoConfirmacionHacerServicio", "confirmacionHacerServicio");

            }
            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(btnAceptarHacerServicio, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
          
        }

        #endregion
        
        #endregion

        #region Devoluciones

        #region Eventos Devolución

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDevolucion();

            //Validando que el Resultado sea Exitoso
            if (result.OperacionExitosa)
            {
                //Obteniendo Servicio
                int idServicio = Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]);

                //Actualizando Carga Viajes
                cargaViajesPendientesRecepcion();

                //Marcando Fla
                Controles.MarcaFila(gvServicios, idServicio.ToString(), "IdServicio", "IdServicio-IdServicioControlEvidencia-Viaje-IdMovimiento-IndDevolucion",
                            OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenarServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue),
                            true, 3);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickGuardarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.GuardaDetalleDevolucion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickEliminarDevolucionDetalle(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Devolución
            result = wucDevolucionFaltante.EliminaDetalleDevolucion();

            //Validando que el Resultado sea Exitoso
            if (result.OperacionExitosa)
            {
                //Obteniendo Servicio
                int idServicio = Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]);

                //Actualizando Carga Viajes
                cargaViajesPendientesRecepcion();

                //Marcando Fla
                Controles.MarcaFila(gvServicios, idServicio.ToString(), "IdServicio", "IdServicio-IdServicioControlEvidencia-Viaje-IdMovimiento-IndDevolucion",
                            OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenarServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue),
                            true, 3);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDevolucion(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            wucReferenciaViaje.InicializaControl(wucDevolucionFaltante.objDevolucionFaltante.id_devolucion_faltante, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 156);

            //Alternando Ventanas Modales
            ScriptServer.AlternarVentana(this, "Devoluciones", "modalDevolucionFaltante", "devolucionFaltante");
            ScriptServer.AlternarVentana(this, "Referencias", "referenciasServicioModal", "referenciasServicio");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Devolucion";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucDevolucionFaltante_ClickAgregarReferenciasDetalle(object sender, EventArgs e)
        {
            //Inicializando Control de Referencias
            wucReferenciaViaje.InicializaControl(wucDevolucionFaltante.idDetalle, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 157);

            //Alternando Ventanas Modales
            ScriptServer.AlternarVentana(this, "Devoluciones", "modalDevolucionFaltante", "devolucionFaltante");
            ScriptServer.AlternarVentana(this, "Referencias", "referenciasServicioModal", "referenciasServicio");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Devolucion";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click1(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando del Control
            switch (lnk.CommandName)
            {
                case "Referencias":
                    //Cerrando ventana modal 
                    ScriptServer.AlternarVentana(this, "Referencias", "referenciasServicioModal", "referenciasServicio");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Devolucion":
                            //Cerrando ventana modal 
                            ScriptServer.AlternarVentana(this, "Devoluciones", "modalDevolucionFaltante", "devolucionFaltante");
                            break;
                    }

                    break;
            }
        }

        #endregion

        #endregion

        #region Métodos Devolucion

        /// <summary>
        /// Método encargado de Cargar el Resumen de las Devoluciones
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        private void cargaResumenDevoluciones(int id_servicio)
        {
            //Obteniendo Devoluciones
            using (DataTable dtDevoluciones = SAT_CL.Despacho.DevolucionFaltante.ObtieneDevolucionesServicio(id_servicio))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDevoluciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDevoluciones, dtDevoluciones, "Id-IdDevolucion", lblOrdenadoDev.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDevoluciones, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDevoluciones);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvDevoluciones);
        }

        #endregion

        #region Recepción Múltiple

        /// <summary>
        /// Click en seleccionar todos los elementos de la página del gv activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionarTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Recuperando contador del pie
            Label lblContadorSeleccion = (Label)gvServicios.FooterRow.FindControl("lblContadorSeleccionados");
            //Seleccionando todos los elementos de la página
            lblContadorSeleccion.Text = Controles.SeleccionaFilasTodas(gvServicios, "chkSeleccionarServicio", ((CheckBox)sender).Checked, false, false).ToString();
        }
        /// <summary>
        /// Click en Seleccionar el elemento del gv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSeleccionarServicio_CheckedChanged(object sender, EventArgs e)
        {
            //Obteniendo la referencia del control actualizado
            CheckBox chkFila = (CheckBox)sender;
            Controles.SumaEtiquetaFooter(gvServicios, "lblContadorSeleccionados", chkFila.Checked ? 1 : -1);
            //Actualizando chek de encabezado
            CheckBox chkEncabezado = (CheckBox)gvServicios.HeaderRow.FindControl("chkSeleccionarTodos");
            if (!chkFila.Checked)
                chkEncabezado.Checked = false;
        }
        /// <summary>
        /// Click en botón recibir seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecibirSeleccion_Click(object sender, EventArgs e)
        {
            //Personalizando mensaje de confirmación
            lblConfirmacionRecepcionMultiple.Text = string.Format("Se dará por recibida la documentación de '{0}' servicios. ¿Desea Continuar?", ((Label)gvServicios.FooterRow.FindControl("lblContadorSeleccionados")).Text);
            //Mostrando ventana de confirmación
            ScriptServer.AlternarVentana(btnRecibirSeleccion, "RecepcionMultiple", "confirmacionRecepcionMultipleModal", "confirmacionRecepcionMultiple");
        }
        /// <summary>
        /// Click en botón de ventana de confirmación de recepción múltiple
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarRecepcionMultiple_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            Button btnConfirmacion = (Button)sender;


            //En base al comando del botón
            switch (btnConfirmacion.CommandName)
            { 
                case "Confirmar":
                    //Realizando recepción múltiple
                    recibirMultiplesServicios();
                    break;
            }

            //Cerrando ventana de forma predeterminada independientemente de la selección del comando
            ScriptServer.AlternarVentana(btnConfirmacion, "RecepcionMultiple", "confirmacionRecepcionMultipleModal", "confirmacionRecepcionMultiple");
        }
        /// <summary>
        /// Realiza el proceso de recepción de documentos de múltiples servicios
        /// </summary>
        private void recibirMultiplesServicios()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion("No hay elementos seleccionados para su recepción.");

            if (!ddlTerminal.SelectedValue.Equals("0") && !ddlTerminal.SelectedValue.Equals(""))
            {
                //Obteniendo conjunto de servicios seleccionados
                GridViewRow[] filas = Controles.ObtenerFilasSeleccionadas(gvServicios, "chkSeleccionarServicio");

                //Si hay elementos seleccionados
                if (filas.Count(f => f != null) > 0)
                {
                    //Declarando auxiliar para determinar si se ha realizado alguna actualización que reflejar al final
                    bool actualizacion_pendiente = false;

                    //Para cada uno de los elementos
                    foreach (GridViewRow r in filas)
                    {
                        //Seleccionando fila
                        gvServicios.SelectedIndex = r.RowIndex;
                        //Obteniendo elemento llave (Id de Servicio)
                        int id_servicio = Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"]);
                        //Inicializando itereción de servicio sin error
                        resultado = new RetornoOperacion(id_servicio);

                        //Creando transacción
                        using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Cargando los segmentos del servicio
                            using (DataTable mitDocumentos = ControlEvidenciaDocumento.CargaDocumentosControlEvidencia(id_servicio,
                                                Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtOperador.Text, ':', 1), "0")),
                                                Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1), "0"))))
                            {
                                //Si hay elementos que inspeccionar
                                if (mitDocumentos != null)
                                {
                                    // Instacniamos Control Evidencia
                                    using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, id_servicio))
                                    {
                                        //Parada cada documento
                                        foreach (DataRow d in mitDocumentos.Rows)
                                        {
                                            //Definiendo estatus de documentos por recibir
                                            //(Lugar de cobro igual a terminal de usuario, "Recibido", de lo contrario "Reenvío")
                                            ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ddlTerminal.SelectedValue == d.Field<int>("IdLugarCobro").ToString() ? ControlEvidenciaDocumento.EstatusDocumento.Recibido : ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio;

                                            //Validando si el documento no se ha recibido anteriormente (La fila cuenta con Id diferente de 0) o Imagen Digitalizada
                                            if (d.Field<int>("Id") == 0 || d.Field<string>("Estatus") == "Imagen Digitalizada")
                                            {
                                                //Si no Existe el Documento
                                                if (d.Field<int>("Id") == 0)
                                                {
                                                    //Instanciando Hoja de Instruccion Documento
                                                    using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(Convert.ToInt32(d.Field<int>("IdHID"))))
                                                    {
                                                        //Declarando Variables de Obtención de Formatos
                                                        //(Si es original, se niega valor de copia)
                                                        bool bit_copia = hid.id_copia_original == 1 ? false : true;
                                                        //(Si es copia, se nieva valor de original)
                                                        bool bit_original = hid.id_copia_original == 2 ? false : true;

                                                        //Realizando actualización
                                                        resultado = ControlEvidenciaDocumento.InsertaControlEvidenciaDocumento(ce.id_servicio_control_evidencia, ce.id_servicio, d.Field<int>("IdSegmentoControlEvidencia"), d.Field<int>("IdSegmento"), (byte)hid.id_tipo_documento, estatus_documento,
                                                                                                hid.id_hoja_instruccion_documento, Convert.ToInt32(ddlTerminal.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                                                d.Field<int>("IdLugarCobro"), bit_original, bit_copia,
                                                                                                hid.bit_sello, 0, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Si hay errores
                                                        if (!resultado.OperacionExitosa)
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    //Instnaciamos Control Evidencia Documento
                                                    using (ControlEvidenciaDocumento objDocumento = new ControlEvidenciaDocumento(Convert.ToInt32(d.Field<int>("Id"))))
                                                    {
                                                        //Editamos Estatus de Documento a Recibido
                                                        resultado = objDocumento.EditaControlEvidenciaDocumentoRecibido(DateTime.Now, Convert.ToInt32(ddlTerminal.SelectedValue), d.Field<int>("IdLugarCobro"),
                                                            estatus_documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    }
                                                }
                                            }
                                        }

                                        //Si no hay errores
                                        if (resultado.OperacionExitosa)
                                            //Actualizando estatus de Control de evidencia
                                            resultado = ce.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    }
                                }
                            }

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                //Creando resultado personalizado
                                using (Servicio srv = new Servicio(id_servicio))
                                    resultado = new RetornoOperacion(id_servicio, string.Format("Recepción de Servicio '{0}': {1}", srv.no_servicio, resultado.Mensaje), resultado.OperacionExitosa);

                                //Terminando bloque transaccional
                                scope.Complete();

                                //Indicando que se debe actualizar el gv de servicios al finalizar esta operación
                                if (!actualizacion_pendiente)
                                    actualizacion_pendiente = true;
                            }

                            //Mostrando resultado por viaje
                            ScriptServer.MuestraNotificacion(btnConfirmarRecepcionMultiple, string.Format("ResultadoRecepcionMultiple_{0}", id_servicio), resultado.Mensaje, resultado.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }

                    //Si hay actualizaciones pendientes
                    if (actualizacion_pendiente)
                        //Realizando la búsqueda de los viajes con pendientes
                        cargaViajesPendientesRecepcion();
                }
            }
            else
                resultado = new RetornoOperacion("No tiene una Terminal de Cobro asignada, contacte al departamento de sistemas para asignarle una");
        }

        #endregion

        #region Eventos Digitalizacion
        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPestana_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion
            if (gvServicios.DataKeys.Count > 0)
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
                        //Hbailita Rcepcion Doc
                        habilitaRecepcionDocumentosDigitalizados();
                        break;
                    case "DocumentosDigitalizados":
                        //Cambiando estilos de pestañas
                        btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
                        btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";
                        //Asignando vista activa de la forma
                        mtvDocumentosDigitalizados.SetActiveView(vwDocumentosDigitalizados);
                        //Carga Imagenes
                        cargaImagenDocumentos();
                        break;
                }
            }
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
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosDocumentosDigitalizados_CheckedChanged(object sender, EventArgs e)
        { 
            //Si existen registros
            if (gvDocumentosDigitalizados.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Evalua el ID del CheckBox en el que se produce el cambio
                if (d.ID == "chkTodos")
                {
                    //Obtenemos su referencia 
                    using (Label l = (Label)gvDocumentosDigitalizados.FooterRow.FindControl("lblSeleccionadosDoc"))
                    {
                        //Actualizamos el texto de la etiqueta
                        l.Text = Controles.SeleccionaFilasTodas(gvDocumentosDigitalizados, "chkVarios", d.Checked).ToString();
                    }
                }
                else
                {
                    //Sumando/restando elemento afectado
                    Controles.SumaSeleccionadosFooter(d.Checked, gvDocumentosDigitalizados, "lblSeleccionadosDoc");

                    //Si retiró selección
                    if (!d.Checked)
                    {
                        //Referenciando control de encabezado
                        CheckBox t = (CheckBox)gvDocumentosDigitalizados.HeaderRow.FindControl("chkTodos");
                        //Aplicando marcado de elemento
                        t.Checked = d.Checked;
                    }
                }
            }
        }

        /// <summary>
        /// Evento producido al seleccionar alguna opcion de documento Digitalizado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDocumentoDigitalizado_Click(object sender, EventArgs e)
        { 
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Insertamos Evidencia Documento en Estatus Digitalizacion
            resultado = recibeDocumentoViajeImagenDigitalizacion(sender);
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Determinando el comando que produjo el evento
                switch (((LinkButton)sender).CommandName)
                {
                    case "Imagen":
                        inicializaImagenes(Convert.ToInt32(resultado.IdRegistro), 36, "Control Evidencia - Documento", 4);
                        break;
                    case "Bitacora":
                        inicializaBitacora(Convert.ToInt32(resultado.IdRegistro), 36, "Control Evidencia - Documento");
                        break;
                    case "Referencias":
                        inicializaReferencias(Convert.ToInt32(resultado.IdRegistro), 36, "Control Evidencia - Documento");
                        break;

                }
            }

        }


        /// <summary>
        /// Evento generado al Recibir los Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecibirDD_Click(object sender, EventArgs e)
        {
            //Validamos Seleccion
            if (gvServicios.DataKeys.Count > 0)
            {
                //Recibimos Documentos Digitalizados
                recibeDocumentosViajeDigitalizado();
            }
            else
                //Mostrando mensaje
                ScriptServer.MuestraNotificacion(btnRecibirDD, "Seleccione el Servicio",ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                  
        }
        #endregion

        #region Métodos Documentos Digitalizados
        /// <summary>
        /// Determina si es posible realizar la recepcón de documentos del viaje
        /// </summary>
        private void habilitaRecepcionDocumentosDigitalizados()
        {
            //Instanciando COntrol de Evidencia
            using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
            {
                //Si el estatus del registro es "No Recibido" o "En Aclaración"
                if (ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.En_Aclaracion ||
                    ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.No_Recibidos
                    ||  ce.id_estatus_documentos == ServicioControlEvidencia.EstatusServicioControlEvidencias.Imagen_Digitalizada)
                {
                    //Habilitando botón de recepción
                    btnRecibirDD.Enabled = true;
                }
                else
                    btnRecibirDD.Enabled = false;
            }
        }

        /// <summary>
        /// Inicializamos Grid View Digitalizado
        /// </summary>
        private void inicializaValoresControlEvidenciaDocumentoDigitalizado()
        {
            //Inicializamos Grid View
            mtvDocumentosDigitalizados.ActiveViewIndex = 1;
            //Inicialiamos Evidencia Documento Digitalizado
            cargaImagenDocumentos();
            //Cambiando estilos de pestañas
            btnPestanaRecibirDocumentosDigitalizados.CssClass = "boton_pestana";
            btnPestanaDocumentosDigitalizados.CssClass = "boton_pestana_activo";

        }

        /// <summary>
        /// Realiza la recepción de documentos del viaje seleccionado
        /// </summary>
        private void recibeDocumentosViajeDigitalizado()
        {
            //Validamos que exista Terminal de Cobro
            if (ddlTerminal.SelectedValue != "0")
            {
                //Validando que existan Registros en el GridView
                if (gvDocumentosDigitalizados.DataKeys.Count > 0)
                {
                    //Obteniendo Filas Seleccionadas
                    GridViewRow[] gvFilas = Controles.ObtenerFilasSeleccionadas(gvDocumentosDigitalizados, "chkVarios");

                    //Validando que Existan Filas Seleccionadas
                    if (gvFilas.Length > 0)
                    {
                        //Manteniendo el Id de viaje al que pertenecen los documentos
                        string id_viaje = gvServicios.SelectedDataKey["IdServicio"].ToString();

                        //Declarando variable de resultado
                        RetornoOperacion result = new RetornoOperacion((Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])));
                        // Instacniamos Control Evidencia
                        using (ServicioControlEvidencia ce = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicio, Convert.ToInt32(gvServicios.SelectedDataKey["IdServicio"])))
                        {
                            //Recorriendo los Documentos
                            foreach (GridViewRow doc in gvFilas)
                            {
                                //Seleccionando Indice de la Fila actual
                                gvDocumentosDigitalizados.SelectedIndex = doc.RowIndex;


                                //Validando si el documento no se ha recibido anteriormente (La fila cuenta con Id diferente de 0)
                                if (gvDocumentosDigitalizados.SelectedDataKey["Id"].ToString() == "0" || gvDocumentosDigitalizados.SelectedDataKey["Estatus"].ToString() == "Imagen Digitalizada")
                                {
                                    //Definiendo estatus de documentos por recibir
                                    //(Lugar de cobro igual a terminal de usuario, "Recibido", de lo contrario "Reenvío")
                                    ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ddlTerminal.SelectedValue == gvDocumentosDigitalizados.SelectedDataKey["IdLugarCobro"].ToString() ? ControlEvidenciaDocumento.EstatusDocumento.Recibido : ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio;

                                    //Instanciando Hoja de Instruccion Documento
                                    using (HojaInstruccionDocumento hid = new HojaInstruccionDocumento(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdHID"])))
                                    {
                                        //Declarando Variables de Obtención de Formatos
                                        //(Si es original, se niega valor de copia)
                                        bool bit_copia = hid.id_copia_original == 1 ? false : true;
                                        //(Si es copia, se nieva valor de original)
                                        bool bit_original = hid.id_copia_original == 2 ? false : true;

                                        //Si no Existe Evidencia Control Documento
                                        if (gvDocumentosDigitalizados.SelectedDataKey["Id"].ToString() == "0")
                                        {
                                            //Realizando actualización
                                            result = ControlEvidenciaDocumento.InsertaControlEvidenciaDocumento(ce.id_servicio_control_evidencia, ce.id_servicio, Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmentoControlEvidencia"]), Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmento"]), (byte)hid.id_tipo_documento, estatus_documento,
                                                                                    hid.id_hoja_instruccion_documento, Convert.ToInt32(ddlTerminal.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(),
                                                                                    Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdLugarCobro"]), bit_original, bit_copia,
                                                                                    hid.bit_sello, 0, "", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        }
                                        else
                                        {
                                            //Instnaciamos Control Evidencia Documento
                                            using(ControlEvidenciaDocumento objDocumento = new ControlEvidenciaDocumento(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["Id"])))
                                            {
                                                                               
                                                //Editamos Estatus de Documento a Recibido
                                                result = objDocumento.EditaControlEvidenciaDocumentoRecibido(DateTime.Now, Convert.ToInt32(ddlTerminal.SelectedValue), Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdLugarCobro"]),
                                                    estatus_documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                        //Instanciamos Segmento para obtenener información de las paradas
                                        using (SegmentoCarga objSegmento = new SegmentoCarga(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["IdSegmento"])))
                                        {
                                            //Instanciamos Parada de Inicio y Parada Fin
                                            using (Parada objParadaInicio = new Parada(objSegmento.id_parada_inicio), objParadaFin = new Parada(objSegmento.id_parada_fin))
                                            {
                                                //Añadiendo resultado a mensaje final
                                                result = new RetornoOperacion(result.IdRegistro, string.Format("{0}: {1}", gvDocumentosDigitalizados.SelectedDataKey["Documento"] + " [" + objParadaInicio.descripcion + "-" + objParadaFin.descripcion
                                                + "] ", result.Mensaje), result.OperacionExitosa);

                                                //Mostrando mensaje
                                                ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Definiendo estatus de documentos por recibir
                                    //(Lugar de cobro igual a terminal de usuario, "Recibido", de lo contrario "Reenvío")
                                    ControlEvidenciaDocumento.EstatusDocumento estatus_documento = ddlTerminal.SelectedValue == gvDocumentosDigitalizados.SelectedDataKey["IdLugarCobro"].ToString() ? ControlEvidenciaDocumento.EstatusDocumento.Recibido : ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio;

                                    //Instanciando Evidencia
                                    using (ControlEvidenciaDocumento evidencia = new ControlEvidenciaDocumento(Convert.ToInt32(gvDocumentosDigitalizados.SelectedDataKey["Id"])))
                                    {
                                        //Validando que exista la Evidencia
                                        if (evidencia.habilitar)
                                        {
                                            //Validando que el Estatus este en Aclaración
                                            if ((ControlEvidenciaDocumento.EstatusDocumento)evidencia.id_estatus_documento == ControlEvidenciaDocumento.EstatusDocumento.En_Aclaracion)
                                            {
                                                //Validando que no este en un Paquete
                                                if (!PaqueteEnvioDocumento.ValidaDocumentoPaqueteDetalle(evidencia.id_control_evidencia_documento))

                                                    //Actualizando Estatus
                                                    result = evidencia.ActualizaEstatusControlEvidenciaDocumento(Convert.ToInt32(ddlTerminal.SelectedValue), estatus_documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                else
                                                    //Instanciando Excepción
                                                    result = new RetornoOperacion("El Documento se encuentra en un Paquete");
                                            }
                                            else
                                            {
                                                //Validando Estatus para Excepción
                                                switch ((ControlEvidenciaDocumento.EstatusDocumento)evidencia.id_estatus_documento)
                                                {
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Recibido:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento se encuentra Recibido");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.No_Recibido:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento no esta Recibido");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Cancelado:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento no esta Cancelado");
                                                            break;
                                                        }
                                                    case ControlEvidenciaDocumento.EstatusDocumento.Recibido_Reenvio:
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Documento esta Recibido con un Reenvio pendiente");
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //Actualizando estatus de Control de evidencia
                            result = ce.ActualizaEstatusGeneralServicioControlEvidencia(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Mostrando resultado principal
                            result = new RetornoOperacion(result.IdRegistro, string.Format("* Servicio {0}: Act. Estatus - {1}", gvServicios.SelectedDataKey["Viaje"], result.Mensaje), result.OperacionExitosa);
                            ScriptServer.MuestraNotificacion(btnAceptar, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            //Cargando GV de viajes
                            cargaViajesPendientesRecepcion();

                            //Seleccionando registro anteriormente marcado
                            Controles.MarcaFila(gvServicios, id_viaje, "IdServicio");

                            //Si se logró seleccionar el registro
                            if (gvServicios.SelectedIndex != -1)
                            {
                                //Cargando documentos
                                cargaDocumentosViajeoDigitalizado();
                            }
                            else
                            {
                                //Limpiamos Grid View
                                Controles.InicializaGridview(gvDocumentosDigitalizados);
                            }
                        }
                    }
                }
            }
            else
                ScriptServer.MuestraNotificacion(btnAceptar, "No existe asignación de la Terminal de Cobro", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos Grid Paradas
        /// <summary>
        /// Cambio de tamaño de página de gridview de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewAnticipos.SelectedValue), true, 3);
        }

        /// <summary>
        /// Cambio de página en gv de anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }

        /// <summary>
        /// Cambio de criterio de orden en gv de anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewAnticipos.Text = Controles.CambiaSortExpressionGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento que llena el GridView de Anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //validando Fila de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando información de la fila actual
                if (e.Row.DataItem != null)
                {
                    //Obteniendo Fila de Datos
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;

                    //Validando Fila
                    if (fila != null)
                    {
                        //Verificar que la columna donde se encuentran los controles dinámicos no esté vacía

                        if (fila["Id"] != null)
                        {

                            //Validando que la fila corresponda paradas:
                            // ("1"): Operacion
                            // ("2"): Sergicio
                            //Si es un Anticipo Programado
                            if (Convert.ToString(fila["Id"]).Equals("1"))
                            {
                                //Coloreando fila de verde por ser Anticipo Programado
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#9FF781");
                            }
                            //Si es un Anticipo Programado Rechazado
                            else if (Convert.ToString(fila["Id"]).Equals("2"))
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6CECE");
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Método que carga el GridView de Anticipos.
        /// </summary>
        /// <param name="IdServicio"></param>
        private void CargaParadas(int id_servicio)
        {
            //Realizando la carga de los servicios coincidentes         
            using (DataTable dt = SAT_CL.ControlEvidencia.Reportes.ObtieneEvidenciasParadas(id_servicio))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvAnticipos, dt, "Id", lblCriterioGridViewAnticipos.Text, true, 3);
                //Si no hay registros
                if (dt == null)
                    //Elimiando de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //Si existen registros, se sobrescribe
                else
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table");
            }
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
            string id_servicio = gvServicios.SelectedDataKey["IdServicio"].ToString();
            //Cargando Grid de pendientes
            //Realizando la búsqueda de los viajes correspondientes
            cargaViajesPendientesRecepcion();
            //Aplicando selección Previa
            if (id_servicio != "")
                Controles.MarcaFila(gvServicios, id_servicio, "IdServicio", "IdServicio-IdServicioControlEvidencia-Viaje-IdMovimiento", (DataSet)Session["DS"], "Table", lblOrdenarServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 3);

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
    }
}