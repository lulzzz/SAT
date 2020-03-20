using SAT_CL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.ControlEvidencia
{
    public partial class HojaInstruccion : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento producido al cargar la página
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

            //Si no es un arecarga de página
            if (!this.IsPostBack)
            {
                this.Form.DefaultButton = btnAceptar.UniqueID;
                //Inicializando contenido de forma
                inicializaForma();
            }
        }

        /// <summary>
        /// Evento producido al pulsar el botón atrás
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAtras_Click(object sender, ImageClickEventArgs e)
        {
         TSDK.ASP.PilaNavegacionPaginas.DireccionaPaginaAnterior();
        }

        /// <summary>
        /// Evento producido al     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;
            //De acuerdo al nombre de comando asignado 
            switch (boton.CommandName)
            {
                //Establecemos la pagina en estatus Nuevo
                case "Nuevo":
                    {
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializando Id de Registro activo
                        Session["id_registro"] = 0;
                        //Inicializamos la pagina
                        inicializaForma();

                        break;
                    }
                //Permite abrir registros 
                case "Abrir":
                    {
                        //Inicializando ventana de apertura
                        inicializaAperturaRegistro(40, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                //Guarda el registro en la BD
                case "Guardar":
                    {
                        //Guardamos el registro
                        guardaHI();
                        break;
                    }
                case "Editar":
                    {
                        //Establecemos el estatus de la pagina a edicion
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //cargando pagina
                        inicializaForma();
                        break;
                    }
                case "Copiar":
                    {
                        //Establecemos el estatus de la pagina a edicion
                        Session["estatus"] = Pagina.Estatus.Copia;
                        //cargando pagina
                        inicializaForma();

                        break;
                    }
                case "Bitacora":
                    {
                        //Mostrando ventana
                        inicializaBitacora(Convert.ToInt32(Session["id_registro"]), 40, "Hoja de Instrucciones");
                        break;
                    }
                case "Referencias":
                    {
                        //Preparando consulta de referencias
                        inicializaReferencias(Convert.ToInt32(Session["id_registro"]), 40, "Hoja Instrucciones");
                        break;
                    }
                case "MapaCarga":
                    {
                        inicializaImagenes(Convert.ToInt32(Session["id_registro"]), 40, "Hoja Instruccion", 5);
                        break;
                    }
                case "MapaDescarga":
                    {
                        inicializaImagenes(Convert.ToInt32(Session["id_registro"]), 40, "Hoja Instruccion", 6);
                        break;
                    }
                case "Imprimir":
                    {
                        /* TODO Para nueva Implementación de Impresión
                        //Instanciamos Hoja de Instrucción
                        using (SAT_CL.ControlEvidencia.HojaInstruccion objHojaInstruccion =new SAT_CL.ControlEvidencia.HojaInstruccion((Convert.ToInt32(Session["id_registro"]))))
                        {
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisor = new SAT_CL.Global.CompaniaEmisorReceptor(objHojaInstruccion.id_compania_emisor))
                            {
                                using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(objHojaInstruccion.id_cliente_receptor))
                                {
                                    using (SAT_CL.Global.Ubicacion objRemitente = new SAT_CL.Global.Ubicacion(objHojaInstruccion.id_remitente))
                                    {
                                        using (SAT_CL.Global.Ubicacion objDestinatario = new SAT_CL.Global.Ubicacion(objHojaInstruccion.id_destinatario))
                                        {
                                            string Cliente = objCompaniaReceptor.id_compania_emisor_receptor == 0 ? "TODOS" : objCompaniaReceptor.nombre;

                                            string Remitente = objRemitente.id_ubicacion == 0 ? "TODOS" : objRemitente.descripcion;

                                            string Destinatario = objDestinatario.id_ubicacion == 0 ? "TODOS" : objDestinatario.descripcion;


                                            //Declarando variable para armado de URL
                                            string urlDestino = "../../ImpresionReportes/Impresion.aspx?idReporte=18&Compania=" + objCompaniaEmisor.nombre + "&Cliente=" + Cliente +
                                                "&Carga=" + Remitente + "&Descarga=" + Destinatario +
                                                 "&IdHojaInstruccion=" + objHojaInstruccion.id_hoja_instruccion.ToString();
                                           
                                            //Instanciando nueva ventana de navegador para apertura de registro
                                            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Accesorios", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                        }
                                    }
                                }
                            }
                        }*/

                        break;
                    }
            }
        }
        /// <summary>
        /// Evento producido al dar click en el botón Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            guardaHI();
        }
        /// <summary>
        /// Evento producido al dar click en el botón Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus actual es edición o copia de registro
            if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion ||
                (Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Copia)
                Session["estatus"] = Pagina.Estatus.Lectura;

            //Inicializando contenido de forma
            inicializaForma();
        }

        /// <summary>
        /// Evento producido al click en el botón guardar un nuevo documento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbGuardarDocN_Click(object sender, EventArgs e)
        {
            guardaDocumento();
        }
        /// <summary>
        /// Evento producido al pulsar botón de guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbGuardarDocE_Click(object sender, EventArgs e)
        {
            //Determinando el comando por ejecutar
            switch (((LinkButton)sender).CommandName)
            {
                case "Guardar":
                    guardaDocumento();
                    break;
                case "Cancelar":
                    gvDocumentoHoja.EditIndex = -1; 
                    //Actualizar contenido de GridView
                    using (DataTable mit = TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"))
                        Controles.CargaGridView(gvDocumentoHoja, mit, "Id-IdTipoDocumento-IdTipoEvento-IdRecepcionEntrega-IdCopiaOriginal", lblCriterioGridViewDocumentoHoja.Text, false, 0);
                    //Cargando catalogos al pie
                    cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
                    break;
            }
        }
        /// <summary>
        /// Evento producido al pulsar algún botón de registro documento en modo de lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEdicionDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando por ejecutar
            switch (((LinkButton)sender).CommandName)
            {
                case "Editar":
                     //Validamos Datos
                    if (gvDocumentoHoja.DataKeys.Count > 0)
                    {
                        //Asignar modo de edición a la fila actual
                        Controles.SeleccionaFila(gvDocumentoHoja, sender, "lnk", true);

                        //Actualizar contenido de GridView
                        using (DataTable mit = TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"))
                            Controles.CargaGridView(gvDocumentoHoja, mit, "Id-IdTipoDocumento-IdTipoEvento-IdRecepcionEntrega-IdCopiaOriginal", lblCriterioGridViewDocumentoHoja.Text, false, 0);

                        //Cargando catalogos al pie
                        cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
                        //Cargando catalogos de edición
                        cargaCatalogoDocumento(gvDocumentoHoja.SelectedRow);
                        //Asignando valores actuales de registro
                        using (DropDownList ddlDocumento = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlDocumentoE"),
                                ddlEvento = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlEventoE"),
                                ddlAccion = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlAccionE"),
                                ddlFormato = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlFormatoE"))
                        {
                            ddlDocumento.SelectedValue = gvDocumentoHoja.SelectedDataKey["IdTipoDocumento"].ToString();
                            ddlEvento.SelectedValue = gvDocumentoHoja.SelectedDataKey["IdTipoEvento"].ToString();
                            ddlAccion.SelectedValue = gvDocumentoHoja.SelectedDataKey["IdRecepcionEntrega"].ToString();
                            ddlFormato.SelectedValue = gvDocumentoHoja.SelectedDataKey["IdCopiaOriginal"].ToString();
                            ddlDocumento.Focus();
                        }
                    }

                    break;
                case "Eliminar":
                     //Validamos Datos
                    if (gvDocumentoHoja.DataKeys.Count > 0)
                    {
                        //Asignar modo de edición a la fila actual
                        Controles.SeleccionaFila(gvDocumentoHoja, sender, "lnk", false);
                        //Generamos script para Apertura de Ventana Modal
                        string script =

                        @"<script type='text/javascript'>
                        $('#contenidoConfirmacionEliminarHIDocumento').animate({ width: 'toggle' });
                        $('#confirmacionEliminarHIDocumento').animate({ width: 'toggle' });     
                        </script>";

                        //Registrando el script sólo para los paneles que producirán actualización del mismo
                        System.Web.UI.ScriptManager.RegisterStartupScript(upgvDocumentoHoja, upgvDocumentoHoja.GetType(), "AbreConfirmacion", script, false);

                    }
                    break;
                case "Bitacora":
                     //Validamos Datos
                    if (gvDocumentoHoja.DataKeys.Count > 0)
                    {
                        //Seleccionando fila
                        Controles.SeleccionaFila(gvDocumentoHoja, sender, "lnk", false);
                        inicializaBitacora(Convert.ToInt32(gvDocumentoHoja.SelectedDataKey.Value), 38, "Documentos HI");
                    }
                    break;
                case "Referencias":
                     //Validamos Datos
                    if (gvDocumentoHoja.DataKeys.Count > 0)
                    {

                        Controles.SeleccionaFila(gvDocumentoHoja, sender, "lnk", false);
                        inicializaReferencias(Convert.ToInt32(gvDocumentoHoja.SelectedDataKey.Value), 38, "Documentos HI");
                    }
                    break;
                case "Imagen":
                    //Validamos Datos
                    if (gvDocumentoHoja.DataKeys.Count > 0)
                    {
                        Controles.SeleccionaFila(gvDocumentoHoja, sender, "lnk", false);
                        inicializaImagenes(Convert.ToInt32(gvDocumentoHoja.SelectedDataKey.Value), 38, "Documento HI", 1);
                    }
                    break;
            }
        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentoHoja_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvDocumentoHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentoHoja_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvDocumentoHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewDocumentoHoja.SelectedValue));
            cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Documentos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelDocumentoHoja_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdAccesorio", "IdTipoEvento");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden delGV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentoHoja_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewDocumentoHoja.Text = Controles.CambiaSortExpressionGridView(gvDocumentoHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
        }
        /// <summary>
        /// Evento producido al  cambiar el tamaño de página del gridview de accesorios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewAccesorioHoja_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvAccesorioHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamañoGridViewAccesorioHoja.SelectedValue));
            cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
        }
        /// <summary>
        /// Evento producido al pulsar el botón de exportación a excel en el gridview de accesorios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelAccesorioHoja_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "IdTipoDocumento", "IdTipoEvento", "IdRecepcionEntrega", "IdCopiaOriginal", "*Evidencia", "*Sello");
        }
        /// <summary>
        /// Evento producido al cambiar el indice activo de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAccesorioHoja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvAccesorioHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
            cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
        }
        /// <summary>
        /// Evento producido al ordenar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAccesorioHoja_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioAccesorioHoja.Text = Controles.CambiaSortExpressionGridView(gvAccesorioHoja, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
            cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
        }
        /// <summary>
        /// Evento producido al pulsar algún botón de accesorio en estatus de lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEdicionAcc_Click(object sender, EventArgs e)
        {
            //Determinando el comando por ejecutar
            switch (((LinkButton)sender).CommandName)
            {
                case "Editar":
                    //Asignar modo de edición a la fila actual
                    Controles.SeleccionaFila(gvAccesorioHoja, sender, "lnk", true);

                    //Actualizar contenido de GridView
                    using (DataTable mit = TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"))
                        Controles.CargaGridView(gvAccesorioHoja, mit, "Id-IdAccesorio-IdTipoEvento", lblCriterioAccesorioHoja.Text, false, 0);

                    //Cargando catalogos al pie
                    cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
                    //Cargando catalogos de edición
                    cargaCatalogoAccesorio(gvAccesorioHoja.SelectedRow);
                    //Asignando valores actuales de registro
                    using (DropDownList ddlAccesorio = (DropDownList)gvAccesorioHoja.SelectedRow.FindControl("ddlAccesorioE"),
                            ddlEvento = (DropDownList)gvAccesorioHoja.SelectedRow.FindControl("ddlEventoE"))
                    {
                        ddlAccesorio.SelectedValue = gvAccesorioHoja.SelectedDataKey["IdAccesorio"].ToString();
                        ddlEvento.SelectedValue = gvAccesorioHoja.SelectedDataKey["IdTipoEvento"].ToString();
                        ddlAccesorio.Focus();
                    }

                    break;
                case "Eliminar":

                    if (gvAccesorioHoja.DataKeys.Count > 0)
                    {
                        //Asignar modo de edición a la fila actual
                        Controles.SeleccionaFila(gvAccesorioHoja, sender, "lnk", false);
                        //Generamos script para Apertura de Ventana Modal
                        string script =

                        @"<script type='text/javascript'>
                        $('#contenidoConfirmacionEliminarHIAccesorio').animate({ width: 'toggle' });
                        $('#confirmacionEliminarHIAccesorio').animate({ width: 'toggle' });     
                        </script>";

                        //Registrando el script sólo para los paneles que producirán actualización del mismo
                        System.Web.UI.ScriptManager.RegisterStartupScript(upgvAccesorioHoja, upgvAccesorioHoja.GetType(), "AbreConfirmacion", script, false);

                    }
                    break;
                case "Bitacora":
                    //Seleccionando fila
                    Controles.SeleccionaFila(gvAccesorioHoja, sender, "lnk", false);
                    inicializaBitacora(Convert.ToInt32(gvAccesorioHoja.SelectedDataKey.Value), 37, "Accesorio HI");
                    break;
                case "Referencias":
                    Controles.SeleccionaFila(gvAccesorioHoja, sender, "lnk", false);
                    inicializaReferencias(Convert.ToInt32(gvAccesorioHoja.SelectedDataKey.Value), 37, "Accosorio HI");
                    break;
            }
        }
        /// <summary>
        /// Evento producido al pulsar algín botón de registro accesorio en modo de edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbGuardarAccE_Click(object sender, EventArgs e)
        {
            //Determinando el comando por ejecutar
            switch (((LinkButton)sender).CommandName)
            {
                case "Guardar":
                    guardaAccesorio();
                    break;
                case "Cancelar":
                    gvAccesorioHoja.EditIndex = -1;
                    //Actualizar contenido de GridView
                    using (DataTable mit = TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"))
                        Controles.CargaGridView(gvAccesorioHoja, mit, "Id-IdAccesorio-IdTipoEvento", lblCriterioAccesorioHoja.Text, false, 0);
                    //Cargando catalogos al pie
                    cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
                    break;
            }
        }
        /// <summary>
        /// Evento producido al pulsar él botón de guardado de nuevo accesorio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbGuardarAccN_Click(object sender, EventArgs e)
        {
            guardaAccesorio();
        }
        /// <summary>
        /// Evento producido al dar click sobre una imagen de la tira de imagenes de documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbThumbnailDoc_Click(object sender, EventArgs e)
        {
            //Determinando el comando solicitado para su carga en el control de imagen con zoom
            //URL de imagen a mostrar en panel de zoom
            //hplImagenZoom.ImageUrl = string.Format("../../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }

        /// <summary>
        /// Evento generado al Actualizar las Imagenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarImagenes_Click(object sender, EventArgs e)
        {
         //Cargamos Imagenes
          cargaImagenDocumentos();
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de forma general el estatus de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Inicializando valores
            inicializaValores();
            //Cargando Catálogos 
            cargaCatalogos();
            //Inicializando menú de forma
            inicializaMenu();
            //Habilitando controles
            habilitaControles();
            //Cargando los documentos de la HI
            cargaDocumentos();
            //Cargando imagenes
            cargaImagenDocumentos();
            //Inicializamos Etiqueta Error
            inicializaEtiquetaError();
            //Cargando los accesorios  
            cargaAccesorios();
            //Colocando posicón inicial dentro de pantalla
            txtDescipcion.Focus();
        }
        /// <summary>
        /// Realiza la carga de los catálogos necesarios en la forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Terminales (Instalaciones tipo terminal)
            //Terminales
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTerminalCobro, 9,"- Seleccione un elemento -", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");
            //Tamaño Grid Documentos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewDocumentoHoja, "", 18);
            //Tamaño Grid Accesorios
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewAccesorioHoja, "", 39);
        }
        /// <summary>
        /// Realiza la carga de los catálogos de accesorios en base a la linea del Gv que sea proporcionada
        /// </summary>
        /// <param name="gvr"></param>
        private void cargaCatalogoAccesorio(GridViewRow gvr)
        {
            //Determinando el tipo de fila
            switch (gvr.RowType)
            {
                //Fila de datos
                case DataControlRowType.DataRow:
                    //Validando estatus de edición
                    //Cargando Lista de Documentos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlAccesorioE", "", 33);
                    //Eventos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlEventoE", "", 32);
                    break;
                //Fila de Pie
                case DataControlRowType.Footer:
                    //Cargando Lista de Documentos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlAccesorioN", "", 33);
                    //Eventos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlEventoN", "", 32);
                    break;
            }
        }
        /// <summary>
        /// Realiza la carga de los catálogos de documento en base a la linea del Gv que sea proporcionada
        /// </summary>
        /// <param name="gvr"></param>
        private void cargaCatalogoDocumento(GridViewRow gvr)
        {
            //Determinando el tipo de fila
            switch (gvr.RowType)
            {
                //Fila de datos
                case DataControlRowType.DataRow:
                    //Validando estatus de edición
                    //Cargando Lista de Documentos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlDocumentoE", "", 28);
                    //Eventos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlEventoE", "", 32);
                    //Acciones
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlAccionE", "", 35);
                    //Formatos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlFormatoE", "", 30);
                    break;
                //Fila de Pie
                case DataControlRowType.Footer:
                    //Cargando Lista de Documentos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlDocumentoN", "", 28);
                    //Eventos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlEventoN", "", 32);
                    //Acciones
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlAccionN", "", 35);
                    //Formatos
                    CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(gvr, "ddlFormatoN", "", 30);
                    break;
            }
        }
        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            //Verificamos estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializamos Valores
                        //Instanciamos Compañia  para visualización en el control
                        using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
                        }
                        lblEstatusCopia.Text = "";
                        lblID.Text =
                        txtDescipcion.Text =
                        txtCliente.Text =
                        txtRemitente.Text =
                        txtConsignatario.Text = "";
                        ddlTerminalCobro.SelectedValue = "0";

                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Obtenemos datos de la Caseta
                        using (SAT_CL.ControlEvidencia.HojaInstruccion hi = new SAT_CL.ControlEvidencia.HojaInstruccion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Inicializamos Valores
                            lblEstatusCopia.Text = "";
                            lblID.Text = hi.id_hoja_instruccion.ToString();
                            txtDescipcion.Text = hi.descripcion_hoja_instruccion;
                            //Instanciamos Compañia  para visualización en el control
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(hi.id_compania_emisor))
                            {
                                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
                            }
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor (hi.id_cliente_receptor))
                                txtCliente.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                            using (SAT_CL.Global.Ubicacion rem = new SAT_CL.Global.Ubicacion(hi.id_remitente))
                                txtRemitente.Text = string.Format("{0}   ID:{1}", rem.descripcion, rem.id_ubicacion);
                            using (SAT_CL.Global.Ubicacion des = new SAT_CL.Global.Ubicacion(hi.id_destinatario))
                                txtConsignatario.Text = string.Format("{0}   ID:{1}", des.descripcion, des.id_ubicacion);
                            //Terminales (Instalaciones tipo terminal)
                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTerminalCobro, 9, "", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");
                            ddlTerminalCobro.SelectedValue = hi.id_terminal_cobro.ToString();
                        }
                        break;
                    }
                case Pagina.Estatus.Copia:
                    {
                        //Obtenemos datos de la Caseta
                        using (SAT_CL.ControlEvidencia.HojaInstruccion hi = new SAT_CL.ControlEvidencia.HojaInstruccion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Inicializamos Valores
                            lblEstatusCopia.Text = "[ Modo Copia ]";
                            lblID.Text = hi.id_hoja_instruccion.ToString();
                            txtDescipcion.Text = hi.descripcion_hoja_instruccion;
                            //Instanciamos Compañia  para visualización en el control
                            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(hi.id_compania_emisor))
                            {
                                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
                            }
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(hi.id_cliente_receptor))
                                txtCliente.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                            using (SAT_CL.Global.Ubicacion rem = new SAT_CL.Global.Ubicacion(hi.id_remitente))
                                txtRemitente.Text = string.Format("{0}   ID:{1}", rem.descripcion, rem.id_ubicacion);
                            using (SAT_CL.Global.Ubicacion des = new SAT_CL.Global.Ubicacion(hi.id_destinatario))
                                txtConsignatario.Text = string.Format("{0}   ID:{1}", des.descripcion, des.id_ubicacion);
                            //Terminales (Instalaciones tipo terminal)
                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTerminalCobro, 9, "", Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), "", 0, "");
                            ddlTerminalCobro.SelectedValue = hi.id_terminal_cobro.ToString();
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Inicializa Etiquetas Error
        /// </summary>
        private void inicializaEtiquetaError()
        {
            //Limpiando errores
            lblError.Text = lblError2.Text = lblError3.Text = "";
        }

        /// <summary>
        /// Metodo encargado de Inicializar el Menu
        /// </summary>
        private void inicializaMenu()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = false;
                        lkbCopia.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        lkbMapaCarga.Enabled = false;
                        lkbMapaDescarga.Enabled = false;
                        lkbImprimir.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEditar.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbCopia.Enabled = false;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbMapaCarga.Enabled = false;
                        lkbMapaDescarga.Enabled = false;
                        lkbImprimir.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbCopia.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        lkbMapaCarga.Enabled = true;
                        lkbMapaDescarga.Enabled = true;
                        lkbImprimir.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Copia:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = false;
                        lkbCopia.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        lkbMapaCarga.Enabled = false;
                        lkbMapaDescarga.Enabled = false;
                        lkbImprimir.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Realiza la habilitación de los controles de la forma en base al estatus actual de la misma
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Copia:
                    txtDescipcion.Enabled = true;
                    txtCompania.Enabled = false;
                    txtCliente.Enabled =
                    txtRemitente.Enabled =
                    txtConsignatario.Enabled =
                    ddlTerminalCobro.Enabled =
                    btnAceptar.Enabled =
                    btnCancelar.Enabled = true;
                    dtlImagenDocumentos.Enabled = (Pagina.Estatus)Session["estatus"] != Pagina.Estatus.Nuevo ? true : false;
                    break;
                case Pagina.Estatus.Lectura:
                    txtDescipcion.Enabled =
                    txtCompania.Enabled =
                    txtCliente.Enabled =
                    txtRemitente.Enabled =
                    txtConsignatario.Enabled =
                    ddlTerminalCobro.Enabled =
                    btnAceptar.Enabled =
                    btnCancelar.Enabled = false;

                    dtlImagenDocumentos.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Determina el estatus que se asignará a un control (habilitación) en base al estatus general de la forma
        /// </summary>
        /// <returns></returns>
        protected bool asignaEstatusControlGridView()
        {
            //Devolviendo valor de habilitación
            return (Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion ? true : false;
        }
        /// <summary>
        /// Guarda los datos del encabezado y actualiza el estatus de la forma
        /// </summary>
        private void guardaHI()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            Pagina.Estatus estatusTemp = (Pagina.Estatus)Session["estatus"];

            //Validando selección de lugar de cobro
            if (ddlTerminalCobro.SelectedValue != "0")
            {
                //Determinando el estatus de la forma
                switch (estatusTemp)
                {
                    case Pagina.Estatus.Nuevo:
                        //Insertando nuevo registr
                        resultado = SAT_CL.ControlEvidencia.HojaInstruccion.InsertarHojaInstruccion(txtDescipcion.Text.ToUpper(), Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRemitente.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtConsignatario.Text, "ID:", 1)),
                                                                    Convert.ToInt32(ddlTerminalCobro.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Cambiando temporalmente el estatus por adoptar (Edición para continuar con la carga de lso documentos)
                        estatusTemp = Pagina.Estatus.Edicion;
                        break;
                    case Pagina.Estatus.Edicion:
                        //Instanciando registro
                        using (SAT_CL.ControlEvidencia.HojaInstruccion hi = new SAT_CL.ControlEvidencia.HojaInstruccion(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Si el registro existe
                            if (hi.id_hoja_instruccion > 0)
                            {
                                //Realizando actualización
                                resultado = hi.EditarHojaInstruccion(txtDescipcion.Text.ToUpper(), Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRemitente.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtConsignatario.Text, "ID:", 1)),
                                                                    Convert.ToInt32(ddlTerminalCobro.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Cambiando temporalmente el estatus por adoptar (Lectura para sólo visualizar los cambios hechos)
                                estatusTemp = Pagina.Estatus.Lectura;
                            }
                            else
                                resultado = new RetornoOperacion("Error al recuperar la HI.");
                        }
                        break;
                    case Pagina.Estatus.Copia:

                        //Realizando copia de registro solicitado
                        resultado = SAT_CL.ControlEvidencia.HojaInstruccion.CopiaHojaInstruccion(Convert.ToInt32(Session["id_registro"]), txtDescipcion.Text.ToUpper(), Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1), "0")), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtRemitente.Text, "ID:", 1)), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtConsignatario.Text, "ID:", 1)),
                                                                    Convert.ToInt32(ddlTerminalCobro.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Cambiando temporalmente el estatus por adoptar (Edición para continuar con la edición de elementos copiados)
                        estatusTemp = Pagina.Estatus.Edicion;
                        break;
                }
            }
            else
                resultado = new RetornoOperacion("Es requerido seleccionar el lugar de cobro.");

            //Si no hay errores 
            if (resultado.OperacionExitosa)
            {
                //Confirmando estatus por asignar
                Session["estatus"] = estatusTemp;
                //Asignando Id de registro
                Session["id_registro"] = resultado.IdRegistro;
                //Cargando contenido de página
                inicializaForma();
            }

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Deshabilita la HI y sus detalles, inicializando el estatus de la páagina a Nuevo
        /// </summary>
        private void deshabilitaHojaInstruccion()
        {
            //Instanciando registro
            using (SAT_CL.ControlEvidencia.HojaInstruccion hi = new SAT_CL.ControlEvidencia.HojaInstruccion(Convert.ToInt32(Session["id_registro"])))
            {
                //Definiendo objeto de resultado
                TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

                //Si el registro existe
                if (hi.id_hoja_instruccion > 0)
                {
                    //Realizando actualización
                    resultado = hi.DeshabilitaHojaInstruccionCascada(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                    resultado = new TSDK.Base.RetornoOperacion("Error al recuperar la HI.");

                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Inicializando estatus de nuevo registro
                    Session["id_registro"] = 0;
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    inicializaForma();
                }

                //Mostrando resultado de actualización
                lblError.Text = resultado.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "AceptarEliminarHI"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarHI_Click(object sender, EventArgs e)
        {
            deshabilitaHojaInstruccion();

            //Generamos script para ocultar de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionEliminarHI').animate({ width: 'toggle' });
            $('#confirmacionEliminarHI').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarEliminarHI, upbtnAceptarEliminarHI.GetType(), "CierreConfirmacion", script, false);


        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "AceptarEliminarDocumentoHI"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarDocumentoHI_Click(object sender, EventArgs e)
        {
            
            //Deshabilitando documento
            deshabilitaDocumento();
            
            //Generamos script para Ocultar de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionEliminarHIDocumento').animate({ width: 'toggle' });
            $('#confirmacionEliminarHIDocumento').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarEliminarHIDocumento, upbtnAceptarEliminarHIDocumento.GetType(), "CierreConfirmacion", script, false);


        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "AceptarEliminarHIAccesorio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarHIAccesorio_Click(object sender, EventArgs e)
        {
            //Deshabilitando documento
            deshabilitaAccesorio();
           
            //Generamos script para Ocultar de Ventana Modal
            string script =

            @"<script type='text/javascript'>
            $('#contenidoConfirmacionEliminarHIAccesorio').animate({ width: 'toggle' });
            $('#confirmacionEliminarHIAccesorio').animate({ width: 'toggle' });     
            </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarEliminarHIAccesorio, upbtnAceptarEliminarHIAccesorio.GetType(), "CierreConfirmacion", script, false);


        }
        /// <summary>
        /// Guarda el documento que se esté editando
        /// </summary>
        private void guardaDocumento()
        {
            //Definiendo objeto de resultado
            TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

            //Determinando el tipo de guardado a realizar
            //Nuevo registro
            if (gvDocumentoHoja.EditIndex == -1)
            {
                //Recuperando controles de catálogo
                using (DropDownList ddlDocumento = (DropDownList)gvDocumentoHoja.FooterRow.FindControl("ddlDocumentoN"),
                        ddlEvento = (DropDownList)gvDocumentoHoja.FooterRow.FindControl("ddlEventoN"),
                        ddlAccion = (DropDownList)gvDocumentoHoja.FooterRow.FindControl("ddlAccionN"),
                        ddlFormato = (DropDownList)gvDocumentoHoja.FooterRow.FindControl("ddlFormatoN"))
                {
                    //Recuperando controles de marcado
                    using (CheckBox chkEvidencia = (CheckBox)gvDocumentoHoja.FooterRow.FindControl("chkEvidenciaN"),
                        chkSello = (CheckBox)gvDocumentoHoja.FooterRow.FindControl("chkSelloN"))
                    {
                        //Campo de texto (referencia)
                        using (TextBox txtObservacion = (TextBox)gvDocumentoHoja.FooterRow.FindControl("txtObservacionN"))
                        {
                            //Realizando guardado de documento
                            resultado = SAT_CL.ControlEvidencia.HojaInstruccionDocumento.InsertarHojaInstruccionDocumento(Convert.ToInt32(Session["id_registro"]),
                                                    Convert.ToInt32(ddlDocumento.SelectedValue), chkEvidencia.Checked, Convert.ToInt32(ddlEvento.SelectedValue), Convert.ToInt32(ddlAccion.SelectedValue),
                                                    Convert.ToInt32(ddlFormato.SelectedValue), chkSello.Checked, txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }
            }
            //Edición
            else
            {
                //Recuperando controles de catálogo
                using (DropDownList ddlDocumento = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlDocumentoE"),
                        ddlEvento = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlEventoE"),
                        ddlAccion = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlAccionE"),
                        ddlFormato = (DropDownList)gvDocumentoHoja.SelectedRow.FindControl("ddlFormatoE"))
                {
                    //Recuperando controles de marcado
                    using (CheckBox chkEvidencia = (CheckBox)gvDocumentoHoja.SelectedRow.FindControl("chkEvidenciaE"),
                        chkSello = (CheckBox)gvDocumentoHoja.SelectedRow.FindControl("chkSelloE"))
                    {
                        //Campo de texto (referencia)
                        using (TextBox txtObservacion = (TextBox)gvDocumentoHoja.SelectedRow.FindControl("txtObservacionE"))
                        {
                            //Instanciando registro a editar
                            using (SAT_CL.ControlEvidencia.HojaInstruccionDocumento doc = new SAT_CL.ControlEvidencia.HojaInstruccionDocumento(Convert.ToInt32(gvDocumentoHoja.SelectedDataKey.Value)))
                            {
                                //Si existe el documento
                                if (doc.id_hoja_instruccion_documento > 0)
                                {
                                    //Realizando guardado de documento
                                    resultado = doc.EditarHojaInstruccionDocumento(Convert.ToInt32(Session["id_registro"]),
                                                            Convert.ToInt32(ddlDocumento.SelectedValue), chkEvidencia.Checked, Convert.ToInt32(ddlEvento.SelectedValue), Convert.ToInt32(ddlAccion.SelectedValue),
                                                            Convert.ToInt32(ddlFormato.SelectedValue), chkSello.Checked, txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    resultado = new TSDK.Base.RetornoOperacion("El registro no fue recuperado, posiblemente ya no existe.");
                            }
                        }
                    }
                }
            }

            //Si no existe error
            if (resultado.OperacionExitosa)
                //Actualziando contenido de GridView
                cargaDocumentos();

            //Mostrando resultado de inserción
            lblError2.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza la deshabilitación del documento solicitado
        /// </summary>
        private void deshabilitaDocumento()
        {
            //Validando que exista un registro seleccionado
            if (gvDocumentoHoja.SelectedIndex != -1)
            {
                //Definiendo objeto de resultado
                TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

                //Instanciando registro a editar
                using (SAT_CL.ControlEvidencia.HojaInstruccionDocumento doc = new SAT_CL.ControlEvidencia.HojaInstruccionDocumento(Convert.ToInt32(gvDocumentoHoja.SelectedDataKey.Value)))
                {
                    //Si existe el documento
                    if (doc.id_hoja_instruccion_documento > 0)
                        //Realizando guardado de documento
                        resultado = doc.DeshabilitaHojaInstruccionDocumento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        resultado = new TSDK.Base.RetornoOperacion("El registro no fue recuperado, posiblemente ya no existe.");
                }

                //Si no existe error
                if (resultado.OperacionExitosa)
                    //Actualziando contenido de GridView
                    cargaDocumentos();

                //Mostrando resultado de inserción
                lblError2.Text = resultado.Mensaje;
            }
        }
        /// <summary>
        /// Realiza la carga de los documentos asociados a la HI actual
        /// </summary>
        private void cargaDocumentos()
        {
            //Cargando Documentos
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    Controles.InicializaGridview(gvDocumentoHoja);
                    break;
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Copia:
                    using (DataTable doc = SAT_CL.ControlEvidencia.HojaInstruccionDocumento.ObtieneHojaInstruccionDocumentos(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si existen registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(doc))
                        {
                            //Inicializando indices
                            Controles.InicializaIndices(gvDocumentoHoja);
                            //Guardando tabla en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], doc, "Table");
                            //Llenando GridView
                            Controles.CargaGridView(gvDocumentoHoja, doc, "Id-IdTipoDocumento-IdTipoEvento-IdRecepcionEntrega-IdCopiaOriginal", lblCriterioGridViewDocumentoHoja.Text, false, 0);
                        }
                        else
                        {
                            //Borrando tabla existente en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            //Inicializando GridView
                            Controles.InicializaGridview(gvDocumentoHoja);
                        }
                    }
                    break;
            }

            //Catálogos pie Documentos
            cargaCatalogoDocumento(gvDocumentoHoja.FooterRow);
        }
        /// <summary>
        /// Realiza la carga de la galería de imagenes
        /// </summary>
        private void cargaImagenDocumentos()
        {
            //Vista previa por default
            imgImagenZoom.ImageUrl = "~/Image/noDisponible.jpg";
            hplImagenZoom.NavigateUrl = "~/Image/noDisponible.jpg";

            //Cargando Documentos
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    Controles.CargaDataList(dtlImagenDocumentos, null, "URL", "", "");
                    break;
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Copia:
                    //Realizando la carga de URL de imagenes a mostrar
                    using (DataTable mit = SAT_CL.ControlEvidencia.HojaInstruccionDocumento.ObtieneHojaInstruccionDocumentosImagenes(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Cargando DataList
                        Controles.CargaDataList(dtlImagenDocumentos, mit, "URL", "", "");
                    }
                    break;
            }
        }
        /// <summary>
        /// Guarda el accesorio que se esté editando
        /// </summary>
        private void guardaAccesorio()
        {
            //Definiendo objeto de resultado
            TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

            //Determinando el tipo de guardado a realizar
            //Nuevo registro
            if (gvAccesorioHoja.EditIndex == -1)
            {
                //Recuperando controles de catálogo
                using (DropDownList ddlAccesorio = (DropDownList)gvAccesorioHoja.FooterRow.FindControl("ddlAccesorioN"),
                        ddlEvento = (DropDownList)gvAccesorioHoja.FooterRow.FindControl("ddlEventoN"))
                {
                    //Campo de texto (referencia)
                    using (TextBox txtObservacion = (TextBox)gvAccesorioHoja.FooterRow.FindControl("txtObservacionN"))
                    {
                        //Realizando guardado de documento
                        resultado = SAT_CL.ControlEvidencia.HojaInstruccionAccesorio.InsertarHojaInstruccionAccesorio(Convert.ToInt32(Session["id_registro"]),
                                                Convert.ToInt32(ddlAccesorio.SelectedValue), Convert.ToInt32(ddlEvento.SelectedValue),
                                                txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
            }
            //Edición
            else
            {
                //Recuperando controles de catálogo
                using (DropDownList ddlAccesorio = (DropDownList)gvAccesorioHoja.SelectedRow.FindControl("ddlAccesorioE"),
                        ddlEvento = (DropDownList)gvAccesorioHoja.SelectedRow.FindControl("ddlEventoE"))
                {
                    //Campo de texto (referencia)
                    using (TextBox txtObservacion = (TextBox)gvAccesorioHoja.SelectedRow.FindControl("txtObservacionE"))
                    {
                        //Instanciando registro a editar
                        using (SAT_CL.ControlEvidencia.HojaInstruccionAccesorio acc = new SAT_CL.ControlEvidencia.HojaInstruccionAccesorio(Convert.ToInt32(gvAccesorioHoja.SelectedDataKey.Value)))
                        {
                            //Si existe el documento
                            if (acc.id_hoja_instruccion_accesorio > 0)
                            {
                                //Realizando guardado de documento
                                resultado = acc.EditarHojaInstruccionAccesorio(Convert.ToInt32(Session["id_registro"]),
                                                        Convert.ToInt32(ddlAccesorio.SelectedValue), Convert.ToInt32(ddlEvento.SelectedValue),
                                                        txtObservacion.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                            else
                                resultado = new TSDK.Base.RetornoOperacion("El registro no fue recuperado, posiblemente ya no existe.");
                        }
                    }
                }
            }

            //Si no existe error
            if (resultado.OperacionExitosa)
                //Actualziando contenido de GridView
                cargaAccesorios();

            //Mostrando resultado de inserción
            lblError3.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza la deshabilitación del accesorio solicitado
        /// </summary>
        private void deshabilitaAccesorio()
        {
            //Validando que exista un registro seleccionado
            if (gvAccesorioHoja.SelectedIndex != -1)
            {
                //Definiendo objeto de resultado
                TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

                //Instanciando registro a editar
                using (SAT_CL.ControlEvidencia.HojaInstruccionAccesorio acc = new SAT_CL.ControlEvidencia.HojaInstruccionAccesorio(Convert.ToInt32(gvAccesorioHoja.SelectedDataKey.Value)))
                {
                    //Si existe el documento
                    if (acc.id_hoja_instruccion_accesorio > 0)
                        //Realizando guardado de documento
                        resultado = acc.DeshabilitaHojaInstruccionAccesorio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        resultado = new TSDK.Base.RetornoOperacion("El registro no fue recuperado, posiblemente ya no existe.");
                }

                //Si no existe error
                if (resultado.OperacionExitosa)
                    //Actualziando contenido de GridView
                    cargaAccesorios();

                //Mostrando resultado de inserción
                lblError3.Text = resultado.Mensaje;
            }
        }
        /// <summary>
        /// Realiza la carga de los accesorios ligados a la HI actual
        /// </summary>
        private void cargaAccesorios()
        {
            //Cargando Documentos
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    Controles.InicializaGridview(gvAccesorioHoja);
                    break; 
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                case Pagina.Estatus.Copia:
                    using (DataTable acc = SAT_CL.ControlEvidencia.HojaInstruccionAccesorio.ObtieneHojaInstruccionAccesorios(Convert.ToInt32(Session["id_registro"])))
                    { 
                        //Si existen registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(acc))
                        {
                            //Inicializando indices
                            Controles.InicializaIndices(gvAccesorioHoja);
                            //Guardando tabla en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], acc, "Table1");
                            //Llenando GridView
                            Controles.CargaGridView(gvAccesorioHoja, acc, "Id-IdAccesorio-IdTipoEvento", lblCriterioAccesorioHoja.Text, false, 0);
                        }
                        else
                        { 
                            //Borrando tabla existente en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                            //Inicializando GridView
                            Controles.InicializaGridview(gvAccesorioHoja);
                        }
                    }
                    break;
            }

            //Catálogos pie Documentos
            cargaCatalogoAccesorio(gvAccesorioHoja.FooterRow);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/AbrirRegistro.aspx?P1="+ idCompania.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1) + "&idTV=" + id_configuracion_tipo_archivo + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=600,height=550";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Imagenes", configuracion, Page);
        }

        #endregion          

    }
}