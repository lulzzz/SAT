using SAT_CL.ControlPatio;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.ControlPatio
{
    public partial class OperacionPatio : System.Web.UI.Page
    {
        private string estado;
        #region Eventos

        /// <summary>
        /// Evento Producido al generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
                //Invocando Método de Inicialización
                inicializaPagina();

            //Recuperando update panel de imagen con script jqzoom y registrando script de efectos
            ScriptManager.RegisterClientScriptBlock(uphplImagenZoom, uphplImagenZoom.GetType(), "jqzoom", @"$(document).ready(function() {$('.MYCLASS').jqzoom({ 
            zoomType: 'standard',  alwaysOn : false,  zoomWidth: 435,  zoomHeight:300,  position:'left',  
            xOffset: 125,  yOffset:0,  showEffect : 'fadein',  hideEffect: 'fadeout'  });});", true);

            //Invocando Método de Carga
            cargaCatalogoAutoCompleta();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del Control "Patios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Validando que exista un Patio Valido
            if (ddlPatio.SelectedValue != "0")
                //Buscando Unidades
                buscarUnidades();
            else//Inicializando Reporte
                TSDK.ASP.Controles.InicializaGridview(gvEntidades);
            //Cargando Zonas de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlZonaPatio, 37, "", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
            //Cargando LayOut
            cargaLayOutPatio();
            //Inicializamos indicadores de unidad
            inicializaIndicadoresUnidad();
            //Inicializamos los indicadores de entidad
            inicializaIndicadoresEntidad();
            //Invocando Método de Carga
            cargaCatalogoAutoCompleta();

        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   //Invocando Método de Busqueda
            buscarUnidades();
        }   

        
        #region Eventos GridView "Entidades"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando Tamaño del GridView
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
        {
            //Recuperando controles de interés
            ImageButton imbEvento = (ImageButton)e.Row.FindControl("imbEvento"),
                       imbContinuar = (ImageButton)e.Row.FindControl("imbContinuar");               
            Image img = (Image)e.Row.FindControl("imgEstatus");    
            Label lblOperacion = (Label)e.Row.FindControl("lblOperacion");
            LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkEvidencias");
            if (lblOperacion != null)
                estado = lblOperacion.Text;
            //Validando que exista el Control
            if (imbEvento != null)
            {
                //Asignando Comando
                imbEvento.CommandName = imbEvento.ToolTip = ((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() == "0" ? "Iniciar Evento" : "Terminar Evento Actual";
                imbEvento.ImageUrl = ((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() == "0" ? "~/Image/Iniciar.png" : "~/Image/Terminar.png";
            }
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
                    using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(((DataRowView)e.Row.DataItem).Row.ItemArray[0])))
                    //Instanciando Acceso de Patio
                    using (AccesoPatio ap = new AccesoPatio(dap.id_acceso_entrada))
                    //Instanciando Ubicación de Patio
                    using (UbicacionPatio up = new UbicacionPatio(Convert.ToInt32(ddlPatio.SelectedValue)))
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
            //Obteniendo Control para la Operaciones
           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                using (DropDownList ddlOp = (DropDownList)e.Row.FindControl("ddlOperacion"))
                using (Label lblConfirmacionIni = (Label)e.Row.FindControl("lblConfirmacionIni"),
                       lblConfirmacionFin = (Label)e.Row.FindControl("lblConfirmacionFin"),
                       lblInicioAnden = (Label)e.Row.FindControl("lblInicioAnden"),
                       lblFinAnden = (Label)e.Row.FindControl("lblFinAnden"),
                       lblAndenCajon = (Label)e.Row.FindControl("lblAndenCajon"),
                       lblFechaConfirmacion = (Label)e.Row.FindControl("lblFechaConfirmacion"),
                       lblFechaAsignacion = (Label)e.Row.FindControl("lblFechaAsignacion"),
                       lblOperador = (Label)e.Row.FindControl("lblOperador"),
                       lblFechaIniAnden = (Label)e.Row.FindControl("lblFechaIniAnden"),
                       lblFechaFinAnden = (Label)e.Row.FindControl("lblFechaFinAnden"))
                using (Image imgConfirmacionIni = (Image)e.Row.FindControl("imgConfirmacionIni"),
                       imgConfirmacionFin = (Image)e.Row.FindControl("imgConfirmacionFin"),
                       imgInicioAnden = (Image)e.Row.FindControl("imgInicioAnden"),
                       imgFinAnden = (Image)e.Row.FindControl("imgFinAnden"),
                       imgOperador = (Image)e.Row.FindControl("imgOperador"))
                {
                    //if (lnkEvento.Text == "Terminar Evento")
                    //    lnkContinuar.Visible = true;
                    //if (imbContinuar != null)
                        //imbContinuar.ToolTip = ((DataRowView)e.Row.DataItem).Row.ItemArray[7].ToString().Contains("Cajón") ? "Andén" : "Cajón";                    
                        //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOp, 192, "", Convert.ToInt32(ddlPatio.SelectedValue), lblOperacion.Text, 0, "");
                    if (((DataRowView)e.Row.DataItem).Row.ItemArray[7].ToString().Contains("Cajón") || ((DataRowView)e.Row.DataItem).Row.ItemArray[7].ToString().Contains("Andén"))
                    {
                        if (((DataRowView)e.Row.DataItem).Row.ItemArray[7].ToString().Contains("Cajón"))
                        {
                            imgInicioAnden.Visible = imgFinAnden.Visible = false;
                            if (lblConfirmacionIni.Text == "NO")
                            {
                                imbContinuar.Visible = imbEvento.Visible = false;
                            }
                            else if (lblConfirmacionIni.Text == "OK")
                            {
                                imbContinuar.Visible = imbEvento.Visible = true;
                            }
                        }
                        else if (((DataRowView)e.Row.DataItem).Row.ItemArray[7].ToString().Contains("Andén"))
                        {
                            imgInicioAnden.Visible = imgFinAnden.Visible = true;
                            if (lblFinAnden.Text == "NO")
                            {
                                imbContinuar.Visible = imbEvento.Visible = false;
                            }
                            else if (lblFinAnden.Text == "OK")
                            {
                                imbContinuar.Visible = imbEvento.Visible = true;
                            }
                        }
                        //imbContinuar.Visible = true;
                        if (lblOperador.Text != "")
                        {
                            imgOperador.Visible = true;
                            imgOperador.ToolTip = "Operador: " + lblOperador.Text + " asignado por Coordinador " + lblFechaConfirmacion.Text;
                        }
                        if (lblConfirmacionIni.Text == "NO")
                        {
                            imgConfirmacionIni.Visible = true;
                            imgConfirmacionIni.ImageUrl = "~/Image/TripPending.png";
                            imgConfirmacionIni.ToolTip = lblFechaConfirmacion.Text;                            
                        }
                        else if (lblConfirmacionIni.Text == "OK")
                        {
                            imgConfirmacionIni.Visible = true;
                            imgConfirmacionIni.ImageUrl = "~/Image/Entrada.png";
                            imgConfirmacionIni.ToolTip = lblFechaConfirmacion.Text;                            
                        }
                        if (lblConfirmacionFin.Text == "NO")
                        {
                            imgConfirmacionFin.Visible = true;
                            imgConfirmacionFin.ImageUrl = "~/Image/TripPending.png";
                        }
                        else if (lblConfirmacionFin.Text == "OK")
                        {
                            imgConfirmacionFin.Visible = true;
                            imgConfirmacionFin.ImageUrl = "~/Image/Entrada.png";
                        }
                        if (lblInicioAnden.Text == "NO")
                        {
                            //imgInicioAnden.Visible = true;
                            imgInicioAnden.ImageUrl = "~/Image/TripPending.png";
                            imgInicioAnden.ToolTip = lblFechaIniAnden.Text;
                        }
                        else if (lblInicioAnden.Text == "OK")
                        {
                            //imgInicioAnden.Visible = true;
                            imgInicioAnden.ImageUrl = "~/Image/Entrada.png";
                            imgInicioAnden.ToolTip = lblFechaIniAnden.Text;
                        }
                        if (lblFinAnden.Text == "NO")
                        {
                            //imgFinAnden.Visible = true;
                            imgFinAnden.ImageUrl = "~/Image/TripPending.png";
                            imgFinAnden.ToolTip = lblFechaFinAnden.Text;
                        }
                        else if (lblFinAnden.Text == "OK")
                        {
                            //imgFinAnden.Visible = true;
                            imgFinAnden.ImageUrl = "~/Image/Entrada.png";
                            imgFinAnden.ToolTip = lblFechaFinAnden.Text;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando que existan Registros
            if(gvEntidades.DataKeys.Count > 0)
                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id-IdEvt");
        }
        /// <summary>
        /// Evento Producido al Presionar un link del GridView "Entidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEvento_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {   //Asignando Fecha Actual
                txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                //Validando el Comando del Boton
                switch (((ImageButton)sender).CommandName)
                {
                    case "Iniciar Evento":
                        {   //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "imb", true);
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdEvt", "", true, 1);
                            //Obteniendo Control para Andenes
                            using (TextBox txtAnd = (TextBox)gvEntidades.SelectedRow.FindControl("txtAnden"))
                            //using (Label lblOperacion = (Label)gvEntidades.SelectedRow.FindControl("lblOperacion"))
                            //Obteniendo Control para Cajones
                            using (TextBox txtCaj = (TextBox)gvEntidades.SelectedRow.FindControl("txtCajon"))
                            //Obteniendo Control para la Operaciones
                            using (DropDownList ddlOp = (DropDownList)gvEntidades.SelectedRow.FindControl("ddlOperacion"))
                            {   //Carga Catalogo de Operaciones
                                using (SAT_CL.ControlPatio.DetalleAccesoPatio dap = new SAT_CL.ControlPatio.DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                {
                                    if(dap.habilitar)
                                    {
                                        //using (SAT_CL.ControlPatio.AccesoPatio ap = new SAT_CL.ControlPatio.AccesoPatio(dap.id_acceso_entrada))
                                        //{
                                        //    if(ap.habilitar)
                                        //    {
                                        //        using (SAT_CL.ControlPatio.EventoDetalleAcceso eda = new SAT_CL.ControlPatio.EventoDetalleAcceso(Convert.ToString(dap.id_detalle_acceso_patio)))
                                        //        {
                                        //            if (eda.habilitar)
                                        //            {
                                                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOp, 192, "", Convert.ToInt32(ddlPatio.SelectedValue), "", dap.id_detalle_acceso_patio, "");
                                        //            }
                                        //            else
                                        //                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlOp, 192, "", Convert.ToInt32(ddlPatio.SelectedValue), Convert.ToString(ap.id_tipo_acceso), 0, "");
                                        //        }
                                        //    }
                                        //}
                                    }                                    
                                }
                                //Invocando Método de Configuración
                                configuraControlesEntidades(ddlOp.SelectedValue);
                                //Limpiando Controles
                                txtAnd.Text =
                                txtCaj.Text = "";
                                //Estableciendo Enfoque del Control
                                ddlOp.Focus();
                            }

                            //Invocando Método de Carga
                            cargaCatalogoAutoCompleta();
                            break;
                        }
                    case "Terminar Evento Actual":
                        {   //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvEntidades, sender, "imb", false);
                            //Asignando Fecha
                            lblFecha.Text = txtFecha.Text;
                            lblError.Text = "";
                            //Declarando Script de Ventana Modal
                            string script = @"<script type='text/javascript'>
                                                $('#contenidoVentanaConfirmacion').animate({ width: 'toggle' });
                                                $('#ventanaConfirmacion').animate({ width: 'toggle' });
                                              </script>";
                            //Registrando Script
                            ScriptManager.RegisterStartupScript(upgvEntidades, upgvEntidades.GetType(), "ConfirmacionTermino", script, false);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Invocando Método de Configuración
            configuraControlesEntidades(((DropDownList)sender).SelectedValue);

            //Invocando Método de Carga
            cargaCatalogoAutoCompleta();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAccionGridView_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvEntidades.DataKeys.Count > 0)
            {   
                //Validando que existe una Selección
                if (gvEntidades.SelectedIndex != -1)
                {   
                    //Declarando Objeto de Retorno
                    RetornoOperacion result = new RetornoOperacion();
                    //Validando el Comando del Control
                    switch (((LinkButton)sender).CommandName)
                    {
                        case "Guardar":
                            {   
                                //Obteniendo Control para Andenes
                                using (TextBox txtAnd = (TextBox)gvEntidades.SelectedRow.FindControl("txtAnden"))
                                //Obteniendo Control para Cajones
                                using (TextBox txtCaj = (TextBox)gvEntidades.SelectedRow.FindControl("txtCajon"))
                                //Obteniendo Control para la Operaciones
                                using (DropDownList ddlOp = (DropDownList)gvEntidades.SelectedRow.FindControl("ddlOperacion"))
                                //Instanciando Tipo de Evento
                                using (TipoEvento te = new TipoEvento(Convert.ToInt32(ddlOp.SelectedValue)))
                                {   
                                    //Obteniendo Fecha
                                    DateTime fec_ini;
                                    DateTime.TryParse(txtFecha.Text, out fec_ini);
                                    //Validando que exista el tipo de Entidad
                                    if (te.id_tipo_entidad > 0)
                                    {   
                                        //Declarando Variable Auxiliar
                                        int id_entidad = 0;
                                        //Validando el Tipo de Entidad
                                        switch ((EntidadPatio.TipoEntidad)te.id_tipo_entidad)
                                        {
                                            case EntidadPatio.TipoEntidad.Anden:
                                                {   
                                                    //Obteniendo la Entidad
                                                    id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAnd.Text, "ID:", 1));
                                                    break;
                                                }
                                            case EntidadPatio.TipoEntidad.Cajon:
                                                {   
                                                    //Obteniendo la Entidad
                                                    id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCaj.Text, "ID:", 1));
                                                    break;
                                                }
                                        }
                                        
                                        //Inicializando Transacción
                                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {   
                                            //Insertando Evento del Detalle
                                            result = EventoDetalleAcceso.InsertaEventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"]), te.id_tipo_evento,
                                                        id_entidad, 0, fec_ini, DateTime.MinValue, "", (byte)EventoDetalleAcceso.TipoActualizacion.Web, 0, false, false,
                                                        Convert.ToInt32(null), Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToBoolean(null), Convert.ToBoolean(null),
                                                        Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToInt32(null),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            
                                            //Validando que se haya insertado Correctamente
                                            if (result.OperacionExitosa)
                                            {   
                                                //Instanciando la Entidad de Patio
                                                using (EntidadPatio ep = new EntidadPatio(id_entidad))
                                                {   //Obteniendo Evento
                                                    int idEvento = result.IdRegistro;
                                                    //Validando que existe la Entidad de Patio
                                                    if (ep.id_entidad_patio > 0)
                                                    {   //Validando la Naturaleza del Evento
                                                        switch(te.naturaleza_evento)
                                                        {
                                                            case TipoEvento.NaturalezaEvento.Carga:
                                                                {   //Actualizando Evento Actual
                                                                    result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Cargando, fec_ini, idEvento, 
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                            case TipoEvento.NaturalezaEvento.Descarga:
                                                                {   //Actualizando Evento Actual
                                                                    result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Descargando, fec_ini, idEvento,
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                            case TipoEvento.NaturalezaEvento.Estaciona:
                                                                {   //Actualizando Evento Actual
                                                                    result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Estacionando, fec_ini, idEvento,
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                        }
                                                        //Validando que se haya Actualizado el Evento
                                                        if (result.OperacionExitosa)
                                                        {   //Instanciando Detalle de Acceso
                                                            using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                                            {   //Validando que exista el Detalle
                                                                if (dap.id_detalle_acceso_patio > 0)
                                                                {   //Validando la Naturaleza del Evento
                                                                    switch (te.naturaleza_evento)
                                                                    {
                                                                        case TipoEvento.NaturalezaEvento.Carga:
                                                                            {   //Actualizando Detalle de Anden
                                                                                result = dap.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.Carga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                break;
                                                                            }
                                                                        case TipoEvento.NaturalezaEvento.Descarga:
                                                                            {   //Actualizando Detalle de Anden
                                                                                result = dap.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.Descarga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                break;
                                                                            }
                                                                        case TipoEvento.NaturalezaEvento.Estaciona:
                                                                            {   //Actualizando Detalle de Anden
                                                                                result = dap.ActualizaCajonCargaDetalle(ep.id_entidad_patio, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.Estacionado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                break;
                                                                            }
                                                                    }
                                                                    //Validando que la Operación fuese Exitosa
                                                                    if (result.OperacionExitosa)
                                                                        //Completando Transacción
                                                                        trans.Complete();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //Validando que la Operación haya sido Exitosa
                                        if (result.OperacionExitosa)
                                        {   //Asignando Fecha Actual
                                            txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                                            //Buscando Unidades
                                            buscarUnidades();
                                        }
                                    }
                                }
                                //Inicializamos los indicadores de unidad
                                inicializaIndicadoresUnidad();
                                //Inicializamos los indicadores de entidad
                                inicializaIndicadoresEntidad();
                                //Actualizamos los paneles correspondientes
                                upplblUnidades.Update();
                                upplblDescargando.Update();
                                uplblCargando.Update();
                                uplblEstacionada.Update();
                                uplblAnden.Update();
                                uplblCajon.Update();
                                break;
                            }
                        case "Cancelar":
                            {   //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvEntidades);
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvEntidades, ((DataSet)Session["DS"]).Tables["Table"], "Id-IdEvt", "", true, 1);
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {   //Validando que exista una Selección
            if (gvEntidades.SelectedIndex != -1)
            {   //Obteniendo Fecha de Termino
                DateTime fecha;
                DateTime.TryParse(txtFecha.Text, out fecha);
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Inicializando Transacción
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {   //Instanciando Evento
                    using (EventoDetalleAcceso evt = new EventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["IdEvt"])))
                    {   //Validando que exista el Evento
                        if (evt.id_evento_detalle_acceso > 0)
                        {   //Terminando Evento del Detalle
                            result = evt.TerminaEventoDetalle(fecha, EventoDetalleAcceso.TipoActualizacion.Web, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Validando que se haya insertado Correctamente
                            if (result.OperacionExitosa)
                            {   //Instanciando la Entidad de Patio
                                using (EntidadPatio ep = new EntidadPatio(evt.id_entidad_patio))
                                {   //Validando que existe la Entidad de Patio
                                    if (ep.id_entidad_patio > 0)
                                        //Actualizando Evento Actual
                                        result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Vacio, EntidadPatio.EstatusCarga.Ninguno, fecha, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    //Validando que se haya Actualizado el Evento
                                    if (result.OperacionExitosa)
                                    {   //Instanciando Tipo de Evento
                                        using (TipoEvento te = new TipoEvento(evt.id_tipo_evento))
                                        //Instanciando Detalle de Acceso
                                        using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                        {   //Validando que exista el Detalle
                                            if (dap.id_detalle_acceso_patio > 0)
                                            {   //Validando la Naturaleza del Evento
                                                switch (te.naturaleza_evento)
                                                {
                                                    case TipoEvento.NaturalezaEvento.Carga:
                                                        {   //Actualizando Detalle de Anden
                                                            result = dap.ActualizaAndenCargaDetalle(0, true, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            break;
                                                        }
                                                    case TipoEvento.NaturalezaEvento.Descarga:
                                                        {   //Actualizando Detalle de Anden
                                                            result = dap.ActualizaAndenCargaDetalle(0, false, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            break;
                                                        }
                                                    case TipoEvento.NaturalezaEvento.Estaciona:
                                                        {   //Actualizando Detalle de Anden
                                                            result = dap.ActualizaCajonCargaDetalle(0, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                            break;
                                                        }
                                                }
                                                //Validando que la Operación fuese Exitosa
                                                if (result.OperacionExitosa)
                                                    //Completando Transacción
                                                    trans.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Validando que la Operación haya sido Exitosa
                if (result.OperacionExitosa)
                {   //Buscando Unidades
                    buscarUnidades();
                    //
                    inicializaIndicadoresUnidad();
                    //Asignando Fecha Actual
                    txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    //Declarando Script de Ventana Modal
                    string script = @"<script type='text/javascript'>
                                            $('#contenidoVentanaConfirmacion').animate({ width: 'toggle' });
                                            $('#ventanaConfirmacion').animate({ width: 'toggle' });
                                      </script>";
                    //Registrando Script
                    ScriptManager.RegisterStartupScript(upbtnAceptar, upbtnAceptar.GetType(), "ConfirmacionTermino", script, false);
                }
                //Mostrando Mensaje de Operación
                lblError.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            TSDK.ASP.Controles.InicializaIndices(gvEntidades);
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                                $('#contenidoVentanaConfirmacion').animate({ width: 'toggle' });
                                                $('#ventanaConfirmacion').animate({ width: 'toggle' });
                                              </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(upbtnCancelar, upbtnCancelar.GetType(), "ConfirmacionTermino", script, false);
        }

        

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
            inicializaIndicadoresUnidad();
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
        /// Evento producido al Dar Click en el Boton "GuardarAutorizacion"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarEvento_Click(object sender, EventArgs e)
        {
            //TerminaEventoIniciaEvento();
            if (Convert.ToInt32(ddlTipo.SelectedValue) > 0)
            {//Validando que exista una Selección
                if (gvEntidades.SelectedIndex != -1)
                {   //Obteniendo Fecha de Termino
                    DateTime fecha;
                    DateTime.TryParse(txtFecha.Text, out fecha);
                    //Declarando Objeto de Retorno
                    RetornoOperacion result = new RetornoOperacion();
                    //Instanciando Tipo de Evento
                    using (TipoEvento te = new TipoEvento(Convert.ToInt32(ddlTipo.SelectedValue)))
                    {
                        //Obteniendo Fecha
                        DateTime fec_ini;
                        DateTime.TryParse(txtFecha.Text, out fec_ini);
                        //Validando que exista el tipo de Entidad
                        if (te.id_tipo_entidad > 0)
                        {
                            //Declarando Variable Auxiliar
                            int id_entidad = 0;
                            //Validando el Tipo de Entidad
                            switch ((EntidadPatio.TipoEntidad)te.id_tipo_entidad)
                            {
                                case EntidadPatio.TipoEntidad.Anden:
                                    {
                                        //Obteniendo la Entidad
                                        id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAndenCajon.Text, "ID:", 1));
                                        break;
                                    }
                                case EntidadPatio.TipoEntidad.Cajon:
                                    {
                                        //Obteniendo la Entidad
                                        id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAndenCajon.Text, "ID:", 1));
                                        break;
                                    }
                            }
                            //Inicializando Transacción
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {   //Instanciando Evento
                                using (EventoDetalleAcceso evt = new EventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["IdEvt"])))
                                {   //Validando que exista el Evento
                                    if (evt.id_evento_detalle_acceso > 0)
                                    {   //Terminando Evento del Detalle
                                        result = evt.TerminaEventoDetalle(fecha, EventoDetalleAcceso.TipoActualizacion.Web, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        //Validando que se haya insertado Correctamente
                                        if (result.OperacionExitosa)
                                        {   //Instanciando la Entidad de Patio
                                            using (EntidadPatio ep = new EntidadPatio(evt.id_entidad_patio))
                                            {   //Validando que existe la Entidad de Patio
                                                if (ep.id_entidad_patio > 0)
                                                    //Actualizando Evento Actual
                                                    result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Vacio, EntidadPatio.EstatusCarga.Ninguno, fecha, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                //Validando que se haya Actualizado el Evento
                                                if (result.OperacionExitosa)
                                                {   //Instanciando Tipo de Evento
                                                    using (TipoEvento tet = new TipoEvento(evt.id_tipo_evento))
                                                    //Instanciando Detalle de Acceso
                                                    using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                                    {   //Validando que exista el Detalle
                                                        if (dap.id_detalle_acceso_patio > 0)
                                                        {   //Validando la Naturaleza del Evento
                                                            switch (tet.naturaleza_evento)
                                                            {
                                                                case TipoEvento.NaturalezaEvento.Carga:
                                                                    {   //Actualizando Detalle de Anden
                                                                        result = dap.ActualizaAndenCargaDetalle(0, true, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                        break;
                                                                    }
                                                                case TipoEvento.NaturalezaEvento.Descarga:
                                                                    {   //Actualizando Detalle de Anden
                                                                        result = dap.ActualizaAndenCargaDetalle(0, false, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                        break;
                                                                    }
                                                                case TipoEvento.NaturalezaEvento.Estaciona:
                                                                    {   //Actualizando Detalle de Anden
                                                                        result = dap.ActualizaCajonCargaDetalle(0, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                        break;
                                                                    }
                                                            }
                                                            //Validando que la Operación fuese Exitosa
                                                            if (result.OperacionExitosa)

                                                            {
                                                                //Insertando Evento del Detalle
                                                                result = EventoDetalleAcceso.InsertaEventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"]), te.id_tipo_evento,
                                                                            id_entidad, 0, fec_ini, DateTime.MinValue, "", (byte)EventoDetalleAcceso.TipoActualizacion.Web, 0, false, false,
                                                                            Convert.ToInt32(null), Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToBoolean(null), Convert.ToBoolean(null),
                                                                            Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToInt32(null),
                                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                                //Validando que se haya insertado Correctamente
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Instanciando la Entidad de Patio
                                                                    using (EntidadPatio epe = new EntidadPatio(id_entidad))
                                                                    {   //Obteniendo Evento
                                                                        int idEvento = result.IdRegistro;
                                                                        //Validando que existe la Entidad de Patio
                                                                        if (epe.id_entidad_patio > 0)
                                                                        {   //Validando la Naturaleza del Evento
                                                                            switch (te.naturaleza_evento)
                                                                            {
                                                                                case TipoEvento.NaturalezaEvento.Carga:
                                                                                    {   //Actualizando Evento Actual
                                                                                        result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Cargando, fec_ini, idEvento,
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                        break;
                                                                                    }
                                                                                case TipoEvento.NaturalezaEvento.Descarga:
                                                                                    {   //Actualizando Evento Actual
                                                                                        result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Descargando, fec_ini, idEvento,
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                        break;
                                                                                    }
                                                                                case TipoEvento.NaturalezaEvento.Estaciona:
                                                                                    {   //Actualizando Evento Actual
                                                                                        result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Estacionando, fec_ini, idEvento,
                                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                        break;
                                                                                    }
                                                                            }
                                                                            //Validando que se haya Actualizado el Evento
                                                                            if (result.OperacionExitosa)
                                                                            {   //Instanciando Detalle de Acceso
                                                                                using (DetalleAccesoPatio dape = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                                                                {   //Validando que exista el Detalle
                                                                                    if (dape.id_detalle_acceso_patio > 0)
                                                                                    {   //Validando la Naturaleza del Evento
                                                                                        switch (te.naturaleza_evento)
                                                                                        {
                                                                                            case TipoEvento.NaturalezaEvento.Carga:
                                                                                                {   //Actualizando Detalle de Anden
                                                                                                    result = dape.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Carga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                    break;
                                                                                                }
                                                                                            case TipoEvento.NaturalezaEvento.Descarga:
                                                                                                {   //Actualizando Detalle de Anden
                                                                                                    result = dape.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Descarga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                    break;
                                                                                                }
                                                                                            case TipoEvento.NaturalezaEvento.Estaciona:
                                                                                                {   //Actualizando Detalle de Anden
                                                                                                    result = dape.ActualizaCajonCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Estacionado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                    break;
                                                                                                }
                                                                                        }
                                                                                        //Validando que la Operación fuese Exitosa
                                                                                        if (result.OperacionExitosa)
                                                                                            //Completando Transacción
                                                                                            trans.Complete();
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Validando que la Operación haya sido Exitosa
                    if (result.OperacionExitosa)
                    {   //Buscando Unidades
                        buscarUnidades();
                        txtAndenCajon.Text = "";
                        //
                        inicializaIndicadoresUnidad();
                        //Asignando Fecha Actual
                        txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Declarando Script de Ventana Modal
                        string script = @"<script type='text/javascript'>
                                            $('#contenedorVentanaEvento').animate({ width: 'toggle' });
                                            $('#ventanaEvento').animate({ width: 'toggle' });
                                      </script>";
                        //Registrando Script
                        ScriptManager.RegisterStartupScript(upbtnGuardarEvento, upbtnGuardarEvento.GetType(), "ConfirmacionTermino", script, false);
                    }
                    //Mostrando Mensaje de Operación
                    lblError.Text = result.Mensaje;
                }
            }
        }
        /// <summary>
        /// Evento producido al Dar Click en el Boton "GuardarAutorizacion"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEvento_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(this.Page, "ventanaEvento", "contenedorVentanaEvento", "ventanaEvento");
        }
        /// <summary>
        /// Evento que carga los valores del dropdownlist unidad medida acorde a la opción seleccionada dropdownlist Tipo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Carga los valores al DropDownList ddlUnidadMedida
            string tipo = Convert.ToInt32(ddlTipo.SelectedValue) == 3 ? "1" : "2";
            string scriptAutocomplete = "";
            //Tipo para Autocomplete 
            switch (tipo)
            {
                case "1":
                    {
                        //Declarando Objeto de Script
                        scriptAutocomplete = @"<script type='text/javascript'>
                                                /** Productos **/
                                                $('#" + txtAndenCajon.ClientID + @"').autocomplete({
                                                    source: '../WebHandlers/AutoCompleta.ashx?id=23&param=" + ddlPatio.SelectedValue + @"',
                                                    appendTo: '#ventanaEvento',
                                                });
                                                </script>";


                        break;
                    }
                case "2":
                    {
                        //Declarando Objeto de Script
                        scriptAutocomplete = @"<script type='text/javascript'>
                                                /** Productos **/
                                                $('#" + txtAndenCajon.ClientID + @"').autocomplete({
                                                    source: '../WebHandlers/AutoCompleta.ashx?id=22&param=" + ddlPatio.SelectedValue + @"',
                                                    appendTo: '#ventanaEvento',
                                                });
                                                </script>";

                        break;
                    }
            }
            //Validando evento para campo 
            if (gvEntidades.DataKeys.Count > 0)
            {   //Asignando Fecha Actual
                using (SAT_CL.ControlPatio.DetalleAccesoPatio dap = new SAT_CL.ControlPatio.DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                {
                    if (dap.habilitar)
                    {
                        using (SAT_CL.ControlPatio.EventoDetalleAcceso eda = new SAT_CL.ControlPatio.EventoDetalleAcceso(Convert.ToString(dap.id_detalle_acceso_patio)))
                        {
                            if (eda.habilitar)
                            {
                                using (SAT_CL.ControlPatio.EntidadPatio pat = new SAT_CL.ControlPatio.EntidadPatio(eda.id_entidad_patio))
                                {
                                    if (pat.habilitar)
                                    {
                                        //Validacion 
                                        if (Convert.ToInt32(ddlTipo.SelectedValue) == 3 || Convert.ToInt32(ddlTipo.SelectedValue) == 0)
                                        {
                                            chkAndenActual.Enabled = false;
                                            chkAndenActual.Checked = false;
                                            txtAndenCajon.Enabled = true;
                                            txtAndenCajon.Text = "";
                                            chkAndenActual.Visible = false;
                                        }
                                        //Validacion 
                                        else if(Convert.ToInt32(ddlTipo.SelectedValue) !=3 && pat.id_tipo_entidad == 2)
                                        {
                                            chkAndenActual.Enabled = true;
                                            chkAndenActual.Checked = false;
                                            txtAndenCajon.Enabled = true;
                                            txtAndenCajon.Text = "";
                                            chkAndenActual.Visible = false;
                                        }
                                        else
                                        {
                                            chkAndenActual.Enabled = true;
                                            chkAndenActual.Checked = false;
                                            txtAndenCajon.Enabled = true;
                                            txtAndenCajon.Text = "";
                                            chkAndenActual.Visible = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "AutocompleteProductos", scriptAutocomplete, false);
        }

        /// <summary>
        /// Eventos de los botones deltro del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkContinuar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion(1);
            if (gvEntidades.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvEntidades, sender, "imb", false);
                ImageButton imb = (ImageButton)sender;
                switch (imb.CommandName)
                {
                    case "Continuar":
                        {   //Inicizaliza modal
                            ScriptServer.AlternarVentana(this.Page, "ventanaEvento", "contenedorVentanaEvento", "ventanaEvento");
                            using (SAT_CL.ControlPatio.DetalleAccesoPatio dap = new SAT_CL.ControlPatio.DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                            {
                                if (dap.habilitar)
                                {
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 192, "Seleccione un Evento", Convert.ToInt32(ddlPatio.SelectedValue), "", dap.id_detalle_acceso_patio, "");
                                }
                            }
                            //SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 36, "Seleccionar", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
                            break;
                        }
                }
            }          
        }
        #endregion

        #region Eventos Layout Patio

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlZonaPatio_SelectedIndexChanged(object sender, EventArgs e)
        {   //Invocando Carga de Layout
            cargaLayOutPatio();
        }
        

        #endregion

        #region Eventos Imagenes

        /// <summary>
        /// Evento disparado al Presionar el Boton "Cerrar"
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
            hplImagenZoom.NavigateUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&url={0}", ((LinkButton)sender).CommandName);
            
            //Imagen seleccionada
            imgImagenZoom.ImageUrl = string.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&ancho=200&alto=150&url={0}", ((LinkButton)sender).CommandName);

            //Actualiz<ando panel de imagen en zoom
            uphplImagenZoom.Update();
        }

        #endregion

        #endregion 

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   //Asignando Fecha Actual
            txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvEntidades);
            //Invocando Método de Carga
            cargaCatalogos();
            //Inicializamos los indicadores de unidad
            inicializaIndicadoresUnidad();
            //Inicializamos los indicadores de entidad
            inicializaIndicadoresEntidad();
            //Asignando Enfoque al Control
            txtNoUnidad.Focus();
            //Cargando el reporte de unidades en patio
            buscarUnidades();
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
                        lblInfoMapa1.Text = lblUnidades.Text = r["Unidades"].ToString();
                        lblInfoMapa3.Text = lblDescargando.Text = r["Descargando"].ToString();
                        lblInfoMapa2.Text = lblCargando.Text = r["Cargando"].ToString();
                        lblEstacionada.Text = r["Estacionadas"].ToString();
                        lblInfoMapa4.Text = r["CargadasxDescargar"].ToString();
                        lblInfoMapa5.Text = r["CargadasxSalir"].ToString();
                        lblInfoMapa6.Text = r["VaciasxCargar"].ToString();
                        lblInfoMapa7.Text = r["VaciasxSalir"].ToString();
                    }
            }
        }

        /// <summary>
        /// Inicializamo los indicadores relacionados con unidades dentro de patio
        /// </summary>
        public void inicializaIndicadoresEntidad()
        {
            //Inicializamos los indicadores de unidad en la pagina
            using (DataTable t = SAT_CL.ControlPatio.EntidadPatio.RetornaIndicadoresEntidad(Convert.ToInt32(ddlPatio.SelectedValue)))
            {
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(t))
                    //Si el origen es valido recorremos la tabla 
                    foreach (DataRow r in t.Rows)
                    {
                        lblCajon.Text = r["CajonesDisponibles"].ToString();
                        lblTotalCajon.Text = r["TotalCajones"].ToString();
                        lblAnden.Text = r["AndenesDisponibles"].ToString();
                        lblTotalAnden.Text = r["TotalAndenes"].ToString();
                        
                    }
            }
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
                        lblTiempoPromedio.Text = "Tiempo Prom.: " + r["TiempoPromedio"].ToString();
                        lblUtilizacion.Text = "% Utilización: " + r["Utilizacion"].ToString() + "%";
                    }
            }
        }

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 3182);
            ddlTamano.SelectedIndex = ddlTamano.Items.IndexOf(ddlTamano.Items.FindByText("250 Registros"));
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {   //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();
            }
            //Cargando Zonas de Patio
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlZonaPatio, 37, "", Convert.ToInt32(ddlPatio.SelectedValue), "", 0, "");
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Unidades
        /// </summary>
        private void buscarUnidades()
        {   
            //Obteniendo Reporte de Unidades con su Evento Actual
            using (DataTable dtUnidadesEvt = Reporte.ObtieneUnidadesEventoActual(txtNoUnidad.Text, txtPlacas.Text, Convert.ToInt32(ddlPatio.SelectedValue)))
            {   
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvEntidades);
                
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtUnidadesEvt))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEntidades, dtUnidadesEvt, "Id-IdEvt-NoEvidencias-Estado", lblOrdenado.Text, true, 1);
                    
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidadesEvt, "Table");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEntidades);
                    
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Cargando Layout
            cargaLayOutPatio();

            //Inicializamos indicadores de unidad
            inicializaIndicadoresUnidad();
        }
        /// <summary>
        /// Método Privado encargado de Configurar los Controles del GridView "Entidades"
        /// </summary>
        /// <param name="value">Valor del Tipo de Evento</param>
        private void configuraControlesEntidades(string value)
        {   //Validando que exista una Selección
            if (gvEntidades.SelectedIndex != -1)
            {   //Obteniendo Control para Andenes
                using (TextBox txtAnd = (TextBox)gvEntidades.SelectedRow.FindControl("txtAnden"))
                //Obteniendo Control para Cajones
                using (TextBox txtCaj = (TextBox)gvEntidades.SelectedRow.FindControl("txtCajon"))
                //Instanciando Tipo de Evento
                using (TipoEvento te = new TipoEvento(Convert.ToInt32(value)))
                {   //Validando que exista el Registro
                    if(te.id_tipo_entidad > 0)
                    {   //Validando Tipo de Entidad
                        switch(te.id_tipo_entidad)
                        {
                            case 1:
                                {   //Visualizando Control de Andenes
                                    txtAnd.Visible = true;
                                    txtCaj.Visible = false;
                                    txtCaj.Text = "";
                                    break;
                                }
                            case 2:
                                {   //Visualizando Control de Cajones
                                    txtAnd.Visible = false;
                                    txtCaj.Visible = true;
                                    txtAnd.Text = "";
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de 
        /// </summary>
        private void cargaCatalogoAutoCompleta()
        {
            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                                //Carga Catalogo Autocompleta 'Andenes'
                                                $('*[id$=gvEntidades] input[id$=txtAnden]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=22&param=" + ddlPatio.SelectedValue + @"'});
                                                //Carga Catalogo Autocompleta 'Cajones'
                                                $('*[id$=gvEntidades] input[id$=txtCajon]').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=23&param=" + ddlPatio.SelectedValue + @"'});
                                              </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAndenCajon", script, false);

        }
        /// <summary>
        /// Método encargado de insertar evento
        /// </summary>
        private void TerminaEventoIniciaEvento()
        {//Validando que exista una Selección
            if (gvEntidades.SelectedIndex != -1)
            {   //Obteniendo Fecha de Termino
                DateTime fecha;
                DateTime.TryParse(txtFecha.Text, out fecha);
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Instanciando Tipo de Evento
                using (TipoEvento te = new TipoEvento(Convert.ToInt32(ddlTipo.SelectedValue)))
                {
                    //Obteniendo Fecha
                    DateTime fec_ini;
                    DateTime.TryParse(txtFecha.Text, out fec_ini);
                    //Validando que exista el tipo de Entidad
                    if (te.id_tipo_entidad > 0)
                    {
                        //Declarando Variable Auxiliar
                        int id_entidad = 0;
                        //Validando el Tipo de Entidad
                        switch ((EntidadPatio.TipoEntidad)te.id_tipo_entidad)
                        {
                            case EntidadPatio.TipoEntidad.Anden:
                                {
                                    //Obteniendo la Entidad
                                    id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAndenCajon.Text, "ID:", 1));
                                    break;
                                }
                            case EntidadPatio.TipoEntidad.Cajon:
                                {
                                    //Obteniendo la Entidad
                                    id_entidad = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtAndenCajon.Text, "ID:", 1));
                                    break;
                                }
                        }
                        //Inicializando Transacción
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {   //Instanciando Evento
                            using (EventoDetalleAcceso evt = new EventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["IdEvt"])))
                            {   //Validando que exista el Evento
                                if (evt.id_evento_detalle_acceso > 0)
                                {   //Terminando Evento del Detalle
                                    result = evt.TerminaEventoDetalle(fecha, EventoDetalleAcceso.TipoActualizacion.Web, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    //Validando que se haya insertado Correctamente
                                    if (result.OperacionExitosa)
                                    {   //Instanciando la Entidad de Patio
                                        using (EntidadPatio ep = new EntidadPatio(evt.id_entidad_patio))
                                        {   //Validando que existe la Entidad de Patio
                                            if (ep.id_entidad_patio > 0)
                                                //Actualizando Evento Actual
                                                result = ep.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Vacio, EntidadPatio.EstatusCarga.Ninguno, fecha, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //Validando que se haya Actualizado el Evento
                                            if (result.OperacionExitosa)
                                            {   //Instanciando Tipo de Evento
                                                using (TipoEvento tet = new TipoEvento(evt.id_tipo_evento))
                                                //Instanciando Detalle de Acceso
                                                using (DetalleAccesoPatio dap = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                                {   //Validando que exista el Detalle
                                                    if (dap.id_detalle_acceso_patio > 0)
                                                    {   //Validando la Naturaleza del Evento
                                                        switch (tet.naturaleza_evento)
                                                        {
                                                            case TipoEvento.NaturalezaEvento.Carga:
                                                                {   //Actualizando Detalle de Anden
                                                                    result = dap.ActualizaAndenCargaDetalle(0, true, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                            case TipoEvento.NaturalezaEvento.Descarga:
                                                                {   //Actualizando Detalle de Anden
                                                                    result = dap.ActualizaAndenCargaDetalle(0, false, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                            case TipoEvento.NaturalezaEvento.Estaciona:
                                                                {   //Actualizando Detalle de Anden
                                                                    result = dap.ActualizaCajonCargaDetalle(0, dap.bit_cargado, DetalleAccesoPatio.EstatusPatio.SinAsignacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                    break;
                                                                }
                                                        }
                                                        //Validando que la Operación fuese Exitosa
                                                        if (result.OperacionExitosa)

                                                        {
                                                            //Insertando Evento del Detalle
                                                            result = EventoDetalleAcceso.InsertaEventoDetalleAcceso(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"]), te.id_tipo_evento,
                                                                        id_entidad, 0, fec_ini, DateTime.MinValue, "", (byte)EventoDetalleAcceso.TipoActualizacion.Web, 0, false, false,
                                                                        Convert.ToInt32(null), Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToBoolean(null), Convert.ToBoolean(null),
                                                                        Convert.ToDateTime(null), Convert.ToDateTime(null), Convert.ToInt32(null),
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que se haya insertado Correctamente
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Instanciando la Entidad de Patio
                                                                using (EntidadPatio epe = new EntidadPatio(id_entidad))
                                                                {   //Obteniendo Evento
                                                                    int idEvento = result.IdRegistro;
                                                                    //Validando que existe la Entidad de Patio
                                                                    if (epe.id_entidad_patio > 0)
                                                                    {   //Validando la Naturaleza del Evento
                                                                        switch (te.naturaleza_evento)
                                                                        {
                                                                            case TipoEvento.NaturalezaEvento.Carga:
                                                                                {   //Actualizando Evento Actual
                                                                                    result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Cargando, fec_ini, idEvento,
                                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    break;
                                                                                }
                                                                            case TipoEvento.NaturalezaEvento.Descarga:
                                                                                {   //Actualizando Evento Actual
                                                                                    result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Descargando, fec_ini, idEvento,
                                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    break;
                                                                                }
                                                                            case TipoEvento.NaturalezaEvento.Estaciona:
                                                                                {   //Actualizando Evento Actual
                                                                                    result = epe.ActualizaEventoActualEntidadPatio(EntidadPatio.Estatus.Ocupado, EntidadPatio.EstatusCarga.Estacionando, fec_ini, idEvento,
                                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    break;
                                                                                }
                                                                        }
                                                                        //Validando que se haya Actualizado el Evento
                                                                        if (result.OperacionExitosa)
                                                                        {   //Instanciando Detalle de Acceso
                                                                            using (DetalleAccesoPatio dape = new DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                                                                            {   //Validando que exista el Detalle
                                                                                if (dape.id_detalle_acceso_patio > 0)
                                                                                {   //Validando la Naturaleza del Evento
                                                                                    switch (te.naturaleza_evento)
                                                                                    {
                                                                                        case TipoEvento.NaturalezaEvento.Carga:
                                                                                            {   //Actualizando Detalle de Anden
                                                                                                result = dape.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Carga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                break;
                                                                                            }
                                                                                        case TipoEvento.NaturalezaEvento.Descarga:
                                                                                            {   //Actualizando Detalle de Anden
                                                                                                result = dape.ActualizaAndenCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Descarga, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                break;
                                                                                            }
                                                                                        case TipoEvento.NaturalezaEvento.Estaciona:
                                                                                            {   //Actualizando Detalle de Anden
                                                                                                result = dape.ActualizaCajonCargaDetalle(ep.id_entidad_patio, dape.bit_cargado, DetalleAccesoPatio.EstatusPatio.Estacionado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                break;
                                                                                            }
                                                                                    }
                                                                                    //Validando que la Operación fuese Exitosa
                                                                                    if (result.OperacionExitosa)
                                                                                        //Completando Transacción
                                                                                        trans.Complete();
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //Validando que la Operación haya sido Exitosa
                if (result.OperacionExitosa)
                {   //Buscando Unidades
                    buscarUnidades();
                    //
                    inicializaIndicadoresUnidad();
                    //Asignando Fecha Actual
                    txtFecha.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                    //Declarando Script de Ventana Modal
                    string script = @"<script type='text/javascript'>
                                            $('#contenedorVentanaEvento').animate({ width: 'toggle' });
                                            $('#ventanaEvento').animate({ width: 'toggle' });
                                      </script>";
                    //Registrando Script
                    ScriptManager.RegisterStartupScript(upbtnGuardarEvento, upbtnGuardarEvento.GetType(), "ConfirmacionTermino", script, false);
                }
                //Mostrando Mensaje de Operación
                lblError.Text = result.Mensaje;
            }            
        }
        #region Métodos Layout

        /// <summary>
        /// Método Privado encargado de Cargar el Layout de la Zona de Patio
        /// </summary>
        private void cargaLayOutPatio()
        {   
            //Obteniendo LayOut con Entidades
            Session["Dibujo"] = SAT_CL.ControlPatio.ZonaPatio.PintaLayOutZona(Convert.ToInt32(ddlZonaPatio.SelectedValue),
                                    Server.MapPath("~/Image/EntidadTiempoOKCarga.png"), Server.MapPath("~/Image/EntidadTiempoOKDescarga.png"),
                                    Server.MapPath("~/Image/EntidadTiempoOK.png"), Server.MapPath("~/Image/EntidadTiempoEXCarga.png"),
                                    Server.MapPath("~/Image/EntidadTiempoEXDescarga.png"), Server.MapPath("~/Image/EntidadTiempoEX.png"),
                                    15, 15);   
            //Asignando URL a la Imagen
            imgLayout.ImageUrl = "~/WebHandlers/VisorImagen.ashx?q=" + Environment.TickCount.ToString();
        }

        /// <summary>
        /// evento que cambia el almacen por default asignado a un usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAndenActual_CheckedChanged(object sender, EventArgs e)
        {

            if (chkAndenActual.Checked == true)
            {
                //Validando que existan Registros
                if (gvEntidades.DataKeys.Count > 0)
                {   //Asignando Fecha Actual
                    using (SAT_CL.ControlPatio.DetalleAccesoPatio dap = new SAT_CL.ControlPatio.DetalleAccesoPatio(Convert.ToInt32(gvEntidades.SelectedDataKey["Id"])))
                    {
                        if (dap.habilitar)
                        {
                            using (SAT_CL.ControlPatio.EventoDetalleAcceso eda = new SAT_CL.ControlPatio.EventoDetalleAcceso(Convert.ToString(dap.id_detalle_acceso_patio)))
                            {
                                if (eda.habilitar)
                                {
                                    using (SAT_CL.ControlPatio.EntidadPatio pat = new SAT_CL.ControlPatio.EntidadPatio(eda.id_entidad_patio))
                                    {
                                        if(pat.habilitar)
                                         txtAndenCajon.Text = string.Format("{0} ID:{1}", pat.descripcion, pat.id_entidad_patio);
                                        txtAndenCajon.Enabled = false;
                                    }
                                }           
                            }
                        }
                    }
                }
            }
            else
            {
                txtAndenCajon.Enabled = true;
            }
        }
        #endregion

        #region Métodos Imagenes

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
                    Controles.CargaDataList(dtlImagenImagenes, mit, "URL", "", "");
                else
                    //Inicializando DataList
                    Controles.CargaDataList(dtlImagenImagenes, null, "URL", "", "");
            }
        }

        #endregion

        #endregion
    }
}