using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class ReporteDevoluciones : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack en la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización de Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscarDevolucionesDetalles();
        }
        /// <summary>
        /// Evento Producido al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Servicio":
                    mtvDevoluciones.ActiveViewIndex = 0;
                    btnServicio.CssClass = "boton_pestana_activo";
                    btnDetalle.CssClass = "boton_pestana";
                    break;
                case "Detalle":
                    mtvDevoluciones.ActiveViewIndex = 1;
                    btnServicio.CssClass = "boton_pestana";
                    btnDetalle.CssClass = "boton_pestana_activo";
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando del Control
            switch (lnk.CommandName)
            {
                case "Referencias":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "referencia");

                    //Validando Argumento
                    switch (lnk.CommandArgument)
                    {
                        case "Devolucion":
                            //Cerrando ventana modal 
                            alternaVentanaModal(lnk, "devolucion");
                            break;
                    }

                    break;
                case "Devolucion":
                    //Cerrando ventana modal 
                    alternaVentanaModal(lnk, "devolucion");
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido de los GridView's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando Comando
            switch(lnk.CommandName)
            {
                case "Devoluciones":
                    //Exportando Tabla
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                    break;
                case "Detalles":
                    //Exportando Tabla
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                    break;
            }
        }
        /// <summary>
        /// Evento que permite imprimir el formato de caja de devolucion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbImprimir_Click(object sender, EventArgs e)
        {
            //Valida que existan registrosen el gridView.
            if (gvDevoluciones.DataKeys.Count != 0)
            {
                //Selecciona la fila del gridview
                Controles.SeleccionaFila(gvDevoluciones, sender, "lnk", false);
                //Obteniendo Ruta            
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/ReporteDevoluciones.aspx", "~/RDLC/Reporte.aspx");
                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "CajaDevolucion", Convert.ToInt32(gvDevoluciones.SelectedDataKey["Id"])), "CajaDevolucion", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);

            }
        }
        #region Eventos GridView "Devoluciones"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Devoluciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoDev.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Devoluciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoDev.Text = Controles.CambiaSortExpressionGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Devoluciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDevoluciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvDevoluciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Editar la Devolución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditaDevolucion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvDevoluciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDevoluciones, sender, "lnk", false);

                //Instanciando Devolución
                using(SAT_CL.Despacho.DevolucionFaltante df = new SAT_CL.Despacho.DevolucionFaltante(Convert.ToInt32(gvDevoluciones.SelectedDataKey["Id"])))
                {
                    //Validando que exista
                    if (df.habilitar)
                    {
                        //Inicializando Control
                        wucDevolucionFaltante.InicializaDevolucion(df.id_devolucion_faltante);

                        //Mostrando Devolución
                        alternaVentanaModal(this, "devolucion");
                    }
                    else
                        //Mostrando Error
                        ScriptServer.MuestraNotificacion(this.Page, "No se puede Acceder a la Devolución", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Eventos GridView "Detalles"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoDet.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoDet.Text = Controles.CambiaSortExpressionGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvDetalles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Editar la Devolución
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditaDetalle_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvDetalles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvDetalles, sender, "lnk", false);

                //Instanciando Devolución
                using (SAT_CL.Despacho.DevolucionFaltante df = new SAT_CL.Despacho.DevolucionFaltante(Convert.ToInt32(gvDetalles.SelectedDataKey["IdDevolucion"])))
                {
                    //Validando que exista
                    if (df.habilitar)
                    {
                        //Inicializando Control
                        wucDevolucionFaltante.InicializaDevolucion(df.id_devolucion_faltante);

                        //Mostrando Devolución
                        alternaVentanaModal(this, "devolucion");
                    }
                    else
                        //Mostrando Error
                        ScriptServer.MuestraNotificacion(this.Page, "No se puede Acceder a la Devolución", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

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

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Buscando Devoluciones y Detalles
                buscarDevolucionesDetalles();

                //Inicializando Indices
                Controles.InicializaIndices(gvDevoluciones);
                Controles.InicializaIndices(gvDetalles);
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

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Buscando Devoluciones y Detalles
                buscarDevolucionesDetalles();

                //Inicializando Indices
                Controles.InicializaIndices(gvDevoluciones);
                Controles.InicializaIndices(gvDetalles);
            }

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

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Buscando Devoluciones y Detalles
                buscarDevolucionesDetalles();

                //Inicializando Indices
                Controles.InicializaIndices(gvDevoluciones);
                Controles.InicializaIndices(gvDetalles);
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
            ucReferenciaViaje.InicializaControl(wucDevolucionFaltante.objDevolucionFaltante.id_devolucion_faltante, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 156);

            //Alternando Ventanas Modales
            alternaVentanaModal(this, "referencia");
            alternaVentanaModal(this, "devolucion");

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
            ucReferenciaViaje.InicializaControl(wucDevolucionFaltante.idDetalle, wucDevolucionFaltante.objDevolucionFaltante.id_compania_emisora, 0, 157);

            //Alternando Ventanas Modales
            alternaVentanaModal(this, "referencia");
            alternaVentanaModal(this, "devolucion");

            //Actualizando Comando del Boton Cerrar
            lkbCerrarReferencias.CommandArgument = "Devolucion";
        }


        #endregion

        #region Eventos Referencias Devolución

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Referencia
            result = ucReferenciaViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)
            {
                //Validando Tabla
                switch (ucReferenciaViaje.Tabla)
                {
                    case 156:
                        {
                            //Inicializando Devolución
                            wucDevolucionFaltante.InicializaDevolucion(ucReferenciaViaje.Registro);
                            break;
                        }
                    case 157:
                        {
                            //Instanciando Detalle
                            using (SAT_CL.Despacho.DevolucionFaltanteDetalle det = new SAT_CL.Despacho.DevolucionFaltanteDetalle(ucReferenciaViaje.Registro))
                            {
                                //Validando que Exista el Registro
                                if (det.habilitar)

                                    //Inicializando Devolución
                                    wucDevolucionFaltante.InicializaDevolucion(det.id_devolucion_faltante);
                            }
                            break;
                        }
                }

                //Buscando Devoluciones y Detalles
                buscarDevolucionesDetalles();
            }

            //Mostrando Resultado de la Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Referencia
            result = ucReferenciaViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese exitosa
            if (result.OperacionExitosa)
            {
                //Validando Tabla
                switch (ucReferenciaViaje.Tabla)
                {
                    case 156:
                        {
                            //Inicializando Devolución
                            wucDevolucionFaltante.InicializaDevolucion(ucReferenciaViaje.Registro);
                            break;
                        }
                    case 157:
                        {
                            //Instanciando Detalle
                            using (SAT_CL.Despacho.DevolucionFaltanteDetalle det = new SAT_CL.Despacho.DevolucionFaltanteDetalle(ucReferenciaViaje.Registro))
                            {
                                //Validando que Exista el Registro
                                if (det.habilitar)

                                    //Inicializando Devolución
                                    wucDevolucionFaltante.InicializaDevolucion(det.id_devolucion_faltante);
                            }
                            break;
                        }
                }

                //Buscando Devoluciones y Detalles
                buscarDevolucionesDetalles();
            }

            //Mostrando Resultado de la Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Invocando ´Carga de Catalogos
            cargaCatalogos();

            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridViews
            Controles.InicializaGridview(gvDevoluciones);
            Controles.InicializaGridview(gvDetalles);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDev, "", 26);

            //Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDet, "", 26);

            //Cargando Catalogos de Devolución
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "-- Seleccione un Tipo", 3143);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "-- Seleccione un Estatus", 3144);
        }
        /// <summary>
        /// Método encargado de Buscar las Devoluciones y los Detalles
        /// </summary>
        private void buscarDevolucionesDetalles()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_cap = DateTime.MinValue;
            DateTime fec_fin_cap = DateTime.MinValue;
            DateTime fec_ini_dev = DateTime.MinValue;
            DateTime fec_fin_dev = DateTime.MinValue;

            //Validando si se Requieren las Fechas
            if (chkIncluir.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbCaptura.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_cap);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_cap);
                }
                else if (rbDevolucion.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_dev);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_dev);
                }
            }

            //Obteniendo Reporte
            using (DataSet ds = SAT_CL.Despacho.Reporte.ReporteDevolucionesDetalle(Convert.ToInt32(txtNoDevolucion.Text == "" ? "0" : txtNoDevolucion.Text), Convert.ToByte(ddlTipo.SelectedValue),
                                Convert.ToByte(ddlEstatus.SelectedValue), fec_ini_cap, fec_fin_cap, fec_ini_dev, fec_fin_dev, txtObservacion.Text, txtReferencia.Text,
                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoViaje.Text,
                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0"))))
            {
                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table"))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDevoluciones, ds.Tables["Table"], "Id", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDevoluciones);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Validando que Existan Registros
                if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvDetalles, ds.Tables["Table1"], "Id-IdDevolucion", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvDetalles);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }
        /// <summary>
        /// Administra la visualización de ventanas modales en la página (muestra/oculta)
        /// </summary>
        /// <param name="control">Control que afecta a la ventana</param>
        /// <param name="nombre_script_ventana">Nombre del script de la ventana</param>
        private void alternaVentanaModal(Control control, string nombre_script_ventana)
        {
            //Determinando que ventana será afectada (mostrada/ocultada)
            switch (nombre_script_ventana)
            {
                case "referencia":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "devolucion":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalDevolucionFaltante", "devolucionFaltante");
                    break;
            }
        }
        #endregion
    }
}