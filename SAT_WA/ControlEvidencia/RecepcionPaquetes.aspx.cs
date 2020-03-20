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
    public partial class RecepcionPaquetes : System.Web.UI.Page
    {
        #region Eventos Encabezado

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

            //Validando que se haya producido un PostBack
            if (!Page.IsPostBack)
            {   //Invocando Método de Carga de Pagina
                inicializaPagina();         
            }
        }

        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Atras"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAtras_Click(object sender, ImageClickEventArgs e)
        {   //Direccionando Pagina Anterior
        PilaNavegacionPaginas.DireccionaPaginaAnterior();
        }

        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Recibir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecibir_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {
                //Recibe Documentos
                recibeDocumento();

                //Generamos script para ocultar de Ventana Modal
                string script =

                @"<script type='text/javascript'>
                $('#contenidoConfirmacionRecibir').animate({ width: 'toggle' });
                $('#confirmacionRecibir').animate({ width: 'toggle' });     
                </script>";

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarRecibir, upbtnAceptarRecibir.GetType(), "CierreConfirmacion", script, false);


            }

        }


        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            //Carga Paquetes 
            cargaPaquetesEnRecepcion();

            //Limpiamos Etiqueta Error
            lblError.Text = "";

            //Inicializamos Grid View Documentos
            Controles.InicializaGridview(gvDocumentos);

            //Inicializamos Indices del Grid Paquetes
            Controles.InicializaIndices(gvPaquetes);

            //Inicializamos Id de Sessión
            Session["id_registro"] = 0;

        }

        #endregion

        #region Eventos GridView "Paquetes"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Paquetes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPaquetes_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvPaquetes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoPaquetes.SelectedValue), true, 2);
                //Inicializamos Grid View
                Controles.InicializaGridview(gvDocumentos);
                //Inicializo Indices
                Controles.InicializaIndices(gvDocumentos);
                lblError.Text = "";
            }

        }

        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Exportar Paquetes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarPaquete_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {   //Exporta el Contenido del GridView "gvResultado" (Recupera el DataTable del DataSet)
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "TiempoF");
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice de página del Gridview "Table"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPaquetes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {   //Cambia el Indice de la Pagina del GridView
                Controles.CambiaIndicePaginaGridView(gvPaquetes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
                //Inicializamos Grid View
                Controles.InicializaGridview(gvDocumentos);
                //Inicializo Indices
                Controles.InicializaIndices(gvDocumentos);
                lblError.Text = "";
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Table"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPaquetes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {   //Muestra el Nombre de la columna por la cual se ordena el GridView
                lblOrdenarPaquetes.Text = Controles.CambiaSortExpressionGridView(gvPaquetes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
                //Inicializamos Grid View
                Controles.InicializaGridview(gvDocumentos);
                //Inicializo Indices
               Controles.InicializaIndices(gvDocumentos);
                lblError.Text = "";
            }
        }

        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Ver"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVer_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {
                //Selecionando la fila actual
                Controles.SeleccionaFila(gvPaquetes, sender, "lnk", false);
                //Limpiamos Error Mensaje
                lblError.Text = "";
                //Cargamos los documnetos ligados al paquete
                cargaDocumentosDelPaquete();
                //Carga Imagenes Documnetos
                cargaImagenDocumentos();

            }
        }



        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Ver"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacoraP_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {
                //Selecionando la fila actual
                Controles.SeleccionaFila(gvPaquetes, sender, "lnk", false);

                inicializaBitacoraRegistro(gvPaquetes.SelectedDataKey.Value.ToString(), "42", "Paquete");
            }
        }

        #endregion

        #region Eventos GridView "DocumentosPaquete"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDocumentos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDocumentos.SelectedValue), true, 2);
            }
        }

        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Exportar Documentos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDocumentos_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {   //Exporta el Contenido del GridView "gvDocumentos" (Recupera el DataTable del DataSet)
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice de página del Gridview "Table1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0) 
            {   //Invocando Método de cambio de indice de Pagina
                Controles.CambiaIndicePaginaGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Table1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDocumentos_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {   //Muestra el nombre del ordenamiento del GridView
                lblOrdenarDocumentos.Text = Controles.CambiaSortExpressionGridView(gvDocumentos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);
            }
        }

        /// <summary>
        /// Evento Producido al cambiar CheckedChanged propiedad de CheckBox del GridView "Table1"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosDocumentos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;
                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvDocumentos.FooterRow.FindControl("lblContadorDetalles"))
                {
                    //Asigna el Valor de "ChkTodosDocumentos" a todos los Controles CheckBox 
                    c.Text = Controles.SeleccionaFilasTodas(gvDocumentos, "chkVariosDocumentos", todos.Checked).ToString();
                }

            }
            //Limpiamos Etiqueta Error
            lblError.Text = "";
        }

        /// <summary>
        /// Evento generado al seleccionar cada uno de los Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDetallesDocumentos_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvDocumentos.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Realizando des/selección del elemento
                Controles.SumaSeleccionadosFooter(d.Checked, gvDocumentos, "lblContadorDetalles");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvDocumentos.HeaderRow.FindControl("chkTodosDocumentos");
                    //deshabilitando seleccion
                    t.Checked = d.Checked;
                }
            }
            //Limpiamos Etiqueta Error
            lblError.Text = "";
        }

        /// <summary>
        /// Evento generado al dar click en el Boton Limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //Inicializa Pagina
            inicializaPagina();
        }

        /// <summary>
        /// Evento generado al dar click en el boton Aclarat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAclarar_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvPaquetes.DataKeys.Count > 0)
            {
                //Aclara Documneto
                aclaraDocumento();

                //Generamos script para ocultar de Ventana Modal
                string script =

                @"<script type='text/javascript'>
                $('#contenidoConfirmacionAclarar').animate({ width: 'toggle' });
                $('#confirmacionAclarar').animate({ width: 'toggle' });     
                </script>";

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterStartupScript(upbtnAceptarAclarar, upbtnAceptarAclarar.GetType(), "CierreConfirmacion", script, false);

            }
        }

        /// <summary>
        /// Método que inicializa el control bitácora del registro
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="titulo">TItulo a mostrar</param>
        private void inicializaBitacoraRegistro(string idRegistro, string idTabla, string titulo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionPaquetes.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }


        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Bitácora"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {
                //Selecionando la fila actual
                Controles.SeleccionaFila(gvDocumentos, sender, "lnk", false);

                inicializaBitacoraRegistro(gvDocumentos.SelectedDataKey.Value.ToString(), "41", "Documento");
            }
        }

        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferencias_OnClick(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvDocumentos.DataKeys.Count > 0)
            {
                //Selecionando la fila actual
                Controles.SeleccionaFila(gvDocumentos, sender, "lnk", false);

                //Inicializamos Ventana Referencias
                inicializaReferencias(Convert.ToInt32(gvDocumentos.SelectedDataKey["IdControlEvidenciaDocumento"]), 41, "Documento");
            }
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionPaquetes.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1) + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }

        /// <summary>
        /// Evento generado al dar click en el link Id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbId(object sender, EventArgs e)
        {
            //Validamos existencia de Grid View
            if (gvPaquetes.DataKeys.Count > 0)
            {
                //Seleccionando fila del GridView
                Controles.SeleccionaFila(gvPaquetes, sender, "lnk", false);
                //Insertando Pagina a Pila de Llamadas
                PilaNavegacionPaginas.InsertaPaginaPila("~/ControlEvidencia/RecepcionPaquetes.aspx", 0, Pagina.Estatus.Nuevo);
                //Asignando datos de sesión
                Session["id_registro"] = gvPaquetes.SelectedDataKey.Value;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Direccionando a Hoja de Instrucción
                Response.Redirect("~/ControlEvidencia/ArmadoEnvioPaquetes.aspx");
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
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de recargar la Pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Carga Catalogo
            cargaCatalogos();
            //Inicializa Grid View
            inicializaGridView();
            //Inicializa Valores
            inicializaValores();
            //Carga Imagenes Documentos
            cargaImagenDocumentos();

        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga Catalogo Documento Grid View
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDocumentos, "", 26);
            //Carga Catalogo Paquetes Grid View
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPaquetes, "", 26);
        }

        /// <summary>
        /// Método Privado encargado de inicializar Grid View de la Forma
        /// </summary>
        private void inicializaGridView()
        {
            //Inicializando GridViews
            Controles.InicializaGridview(gvDocumentos);
            Controles.InicializaGridview(gvPaquetes);
            Controles.InicializaIndices(gvDocumentos);
            Controles.InicializaIndices(gvPaquetes);
        }

        /// <summary>
        /// Método Privado encargado de inicializar Grid View de la Forma
        /// </summary>
        private void inicializaValores()
        {

            //Instanciamos Compañia  para visualización en el control
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();

                //Carga Catalogos
                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOrigen, 9, "TODOS", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");
                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlDestino, 9, "TODOS", Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), "", 0, "");

                //Inicializamos  valores 
                ddlDestino.SelectedValue = "0";
            }
            //Validamos que exista la Clave para asignación de la Terminal de Evidencia
            if (((SAT_CL.Seguridad.Usuario)Session["usuario"]).Configuracion.ContainsKey("Terminal Control Evidencias"))
            {
                //Asignando valores de catálogos según perfil de recepción
                using (SAT_CL.Global.Ubicacion i = new SAT_CL.Global.Ubicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).Configuracion["Terminal Control Evidencias"],((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {
                    //Terminal del usuario
                    ddlDestino.SelectedValue = i.id_ubicacion.ToString();
                }
            }
            else
            {
                //Terminal del usuario Sin Asignar
                ddlDestino.SelectedValue = "0";
            }
            ddlOrigen.SelectedValue = "0";
            lblError.Text = "";
            //Inicializamos Id de Sessión
            Session["id_registro"] = 0;
        }



        /// <summary>
        /// Realiza la carga de los paquetes en Recepción
        /// </summary>
        private void cargaPaquetesEnRecepcion()
        {
            //Realizando la carga de los paquetes 
            using (DataTable mit = PaqueteEnvio.CargaPaquetesEnRecepcion(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)), Convert.ToInt32(ddlOrigen.SelectedValue),
                               Convert.ToInt32(ddlDestino.SelectedValue)))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Llenando GridView
                    Controles.CargaGridView(gvPaquetes, mit, "Id", lblOrdenarPaquetes.Text, true, 2);

                    //Guardando origen de datos Encabezado
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Eliminamos Tabla en sessión
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    //Inicializamos Grid View
                   Controles.InicializaGridview(gvPaquetes);
                }
            }
        }

        /// <summary>
        /// Carga Documentos del Paquete 
        /// </summary>
        private void cargaDocumentosDelPaquete()
        {
            //Validamos Selección de documentos
            if (gvPaquetes.SelectedIndex != -1)
            {

                //Validamos existecia de Select Value
                gvPaquetes.SelectedIndex = gvPaquetes.SelectedValue != null ? gvPaquetes.SelectedIndex : gvPaquetes.SelectedIndex - 1;
                //Realizando la carga de los documentos del paquete
                using (DataTable mit = PaqueteEnvioDocumento.CargaDocumentoDelPaquete(Convert.ToInt32(Convert.ToInt32(gvPaquetes.SelectedDataKey.Value))))
                {

                    //Validamos Origen de Datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Llenando GridView
                        Controles.CargaGridView(gvDocumentos, mit, "Id-IdControlEvidenciaDocumento", lblOrdenarDocumentos.Text, true, 2);

                        //Guardando origen de datos Encabezado
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
                    }
                    else
                    {
                        //Eliminamos Tabla en sessión
                        OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        //Inicializamos Grid View
                        Controles.InicializaGridview(gvDocumentos);
                    }

                }
            }
            else
            { 
                //Eliminamos Tabla en sessión
                OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                //Inicializamos Grid View
                Controles.InicializaGridview(gvDocumentos);
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
            if (gvPaquetes.SelectedIndex == -1)

                //Cargando lista vacía
                Controles.CargaDataList(dtlImagenDocumentos, null, "URL", "", "");
            else
                //Realizando la carga de URL de imagenes a mostrar
                using (DataTable mit = PaqueteEnvio.CargaPaquetesDocumentosImagenes(Convert.ToInt32(gvPaquetes.SelectedDataKey.Value)))
                {
                    //Cargando DataList
                    Controles.CargaDataList(dtlImagenDocumentos, mit, "URL", "", "");
                }
        }

        /// <summary>
        /// Metotodo encargado de Reciir los Docuentos
        /// </summary>
        private void recibeDocumento()
        {
            //obteniendo filas seleccionadas
            GridViewRow[] Documentos = Controles.ObtenerFilasSeleccionadas(gvDocumentos, "chkVariosDocumentos");

            //si hay vales seleccionados
            if (Documentos.Length > 0)
            {
                //Declaramos Objeto Resultado
               RetornoOperacion resultado = new RetornoOperacion();

                //Declaramos Variable para almacenar el Id del documento
                string idDocumento = "";

                //recorriendo cada uno de los detalles
                foreach (GridViewRow Documento in Documentos)
                {
                    //Iniciamos Transacción
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Selecionamos Fila
                        gvDocumentos.SelectedIndex = Documento.RowIndex;
                        //Instanciamos Documento
                        using (PaqueteEnvioDocumento objPaqueteDocumento = new PaqueteEnvioDocumento(Convert.ToInt32(gvDocumentos.SelectedDataKey.Value)))
                        {
                            //Validamos que es Estatus sea En Transito
                            if (objPaqueteDocumento.estatus == PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.Transito)
                            {
                                resultado = objPaqueteDocumento.ActualizaEstatusPaqueteEnvioDocumento(PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.Recibido, ((Usuario)Session["usuario"]).id_usuario);

                                //Asignamos Id del Documento
                                idDocumento = resultado.IdRegistro.ToString();

                                //Si se Actualizo correctamente el estatus del paquete
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Control Evidencia Docuemento
                                    using (ControlEvidenciaDocumento objControlEvidencia = new ControlEvidenciaDocumento(objPaqueteDocumento.id_control_evidencia_documento))
                                    {
                                        //Instanciando registro documento
                                        using (ServicioControlEvidencia se = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicioControlEvidencia, objControlEvidencia.id_servicio_control_evidencia))
                                        {
                                            //Actualizamos Estatu de Control Evidencia
                                            resultado = objControlEvidencia.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.Recibido, ((Usuario)Session["usuario"]).id_usuario);

                                            //Validamos Actualización de Estatus
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Actualizando estatus del Servicio Control de evidencia
                                                resultado = se.ActualizaEstatusGeneralServicioControlEvidencia(((Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                    }


                                }
                                //Mostramos Resultado Error 
                                lblError.Text = lblError.Text + idDocumento + " " + resultado.Mensaje + "<br>";


                            }
                        }
                        //Finalizando transacción
                        if (resultado.OperacionExitosa)
                        {
                            scope.Complete();
                        }
                    }
                }
                //Cargamos Paquete
                cargaPaquetesEnRecepcion();

                //Cargamos Documentos
                cargaDocumentosDelPaquete();

                //Inicializa Indices GRid View
                Controles.InicializaIndices(gvDocumentos);


            }
        }


        /// <summary>
        /// Metotodo encargado de Recibir los Documentos
        /// </summary>
        private void aclaraDocumento()
        {
            //obteniendo filas seleccionadas
            GridViewRow[] Documentos = Controles.ObtenerFilasSeleccionadas(gvDocumentos, "chkVariosDocumentos");

            //si hay vales seleccionados
            if (Documentos.Length > 0)
            {
                //Declaramos Objeto Resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Declaramos Variable para almacenar el Id del documento
                string idDocumento = " ";

                //recorriendo cada uno de los detalles
                foreach (GridViewRow Documento in Documentos)
                {
                    //Iniciamos Transacción
                    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {

                        //Selecionamos Fila
                        gvDocumentos.SelectedIndex = Documento.RowIndex;
                        //Instanciamos Documento
                        using (PaqueteEnvioDocumento objPaqueteDocumento = new PaqueteEnvioDocumento(Convert.ToInt32(gvDocumentos.SelectedDataKey.Value)))
                        {
                            //Validamos que es Estatus sea En Transito
                            if (objPaqueteDocumento.estatus == PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.Transito)
                            {
                                resultado = objPaqueteDocumento.ActualizaEstatusPaqueteEnvioDocumento(PaqueteEnvioDocumento.EstatusPaqueteEnvioDocumento.En_Aclaracion, ((Usuario)Session["usuario"]).id_usuario);

                                //Asignamos Id del Documento
                                idDocumento = resultado.IdRegistro.ToString();

                                //Si se Actualizo correctamente el estatus del paquete
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciamos Control Evidencia Docuemento
                                    using (ControlEvidenciaDocumento objControlEvidencia = new ControlEvidenciaDocumento(objPaqueteDocumento.id_control_evidencia_documento))
                                    {
                                        //Instanciando registro documento
                                        using (ServicioControlEvidencia se = new ServicioControlEvidencia(ServicioControlEvidencia.TipoConsulta.IdServicioControlEvidencia, objControlEvidencia.id_servicio_control_evidencia))
                                        {
                                            resultado = objControlEvidencia.ActualizaEstatusControlEvidenciaDocumento(ControlEvidenciaDocumento.EstatusDocumento.En_Aclaracion, ((Usuario)Session["usuario"]).id_usuario);
                                           
                                            //Validamos Actualización de Estatus
                                            if (resultado.OperacionExitosa)
                                            {
                                                //Actualizando estatus del Servicio Control de evidencia
                                                resultado = se.ActualizaEstatusGeneralServicioControlEvidencia(((Usuario)Session["usuario"]).id_usuario);
                                            }
                                        }
                                    }


                                }

                                //Mostramos Resultado Error 
                                lblError.Text = lblError.Text + idDocumento + " " + resultado.Mensaje + "<br>";

                            }
                        }
                        //Finalizando transacción
                        if (resultado.OperacionExitosa)
                        {
                            scope.Complete();
                        }
                    }

                }
                //Cargamos Paquete
                cargaPaquetesEnRecepcion();

                //Cargamos Documentos
                cargaDocumentosDelPaquete();
                //Inicializa Indices GRid View
                Controles.InicializaIndices(gvDocumentos);
            }
        }

        #endregion
    }
}