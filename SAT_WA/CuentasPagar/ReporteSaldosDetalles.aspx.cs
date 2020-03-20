using SAT_CL.CXP;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class ReporteSaldosDetalles : System.Web.UI.Page 
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

            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaSaldosDetalle();
        }
        /// <summary>
        /// Evento Producido al Cerrar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Cerrando ventana modal 
            gestionaVentanas(lnk, lnk.CommandName);

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvSaldosDetalle);

            //Actualizando Reporte
            upgvSaldosDetalle.Update();
        }
        /// <summary>
        /// Evento Producido al Exportar los Archivos XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportarXML_Click(object sender, EventArgs e)
        {
            //Exportando Archivo(s) XML
            exportaXML();
        }

        #region Eventos GridView "Saldos Detalle"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);

            //Sumando Totales
            sumaTotalesDetalles();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosDetalle_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);

            //Sumando Totales
            sumaTotalesDetalles();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Saldos Detalle"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSaldosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvSaldosDetalle, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

            //Sumando Totales
            sumaTotalesDetalles();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el "Monto Aplicado"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAplicaciones_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Mostrando Ventana
                ScriptServer.AlternarVentana(this, this.GetType(), "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");

                //Invocando Método de Carga
                buscaFichasAplicadas(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"]));

                //Inicializando Indices
                Controles.InicializaIndices(gvServiciosEntidad);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando Si el GridView contiene Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {   
                //Declarando Controles
                CheckBox chkVarios = null, chkTodos = null;
                
                //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   
                    //"Todos"
                    case "chkTodos":
                        {
                            //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                            chkTodos = (CheckBox)gvSaldosDetalle.HeaderRow.FindControl("chkTodos");
                            
                            //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                            Controles.SeleccionaFilasTodas(gvSaldosDetalle, "chkVarios", chkTodos.Checked);
                            break;
                        }
                    //"Varios"
                    case "chkVarios":
                        {
                            //Obteniendo Control
                            chkVarios = (CheckBox)sender;

                            //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                            chkTodos = (CheckBox)gvSaldosDetalle.HeaderRow.FindControl("chkTodos");

                            //Obteniendo Filas Selecionadas
                            GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvSaldosDetalle, "chkVarios");

                            //Validando que exista el Control
                            if (chkTodos != null)
                            {
                                //Validando que existan Filas
                                if (gvr.Length == 0)

                                    //Desmarcando Control del Encabezado
                                    chkTodos.Checked = false;
                                
                                //Validando si todas las Filas han sido Seleccionadas
                                else if (gvr.Length == gvSaldosDetalle.Rows.Count)
                                
                                    //Marcando Control del Encabezado
                                    chkTodos.Checked = true;
                                else
                                    //Desmarcando Control del Encabezado
                                    chkTodos.Checked = false;
                            }

                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en "Descargar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDescargaXML_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Declarando Documento XML
                XDocument doc = new XDocument();

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSaldosDetalle, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.CXP.FacturadoProveedor factura = new FacturadoProveedor(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))

                //Instanciando Proveedor
                using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(factura.id_compania_proveedor))
                {
                    //Validando que exista la Factura
                    if (factura.habilitar && proveedor.habilitar)
                    {
                        //Declarando Objeto de Retorno
                        byte[] xml = null;

                        //Obteniendo Archivos
                        using (DataTable dtArchivos = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(72, factura.id_factura, 8, 0))
                        {
                            //Validando que Existan Registros
                            if (Validacion.ValidaOrigenDatos(dtArchivos))
                            {
                                //Recorriendo Archivos
                                foreach (DataRow dr in dtArchivos.Rows)
                                {
                                    try
                                    {
                                        //Obteniendo Bytes del Archivo
                                        xml = System.IO.File.ReadAllBytes(dr["URL"].ToString());

                                        //Resultado Positivo
                                        result = new RetornoOperacion(0, "", true);
                                    }
                                    catch (Exception ex)
                                    {
                                        //Obteniendo Bytes del Archivo
                                        xml = null;

                                        //Instanciando resultado Positivo
                                        result = new RetornoOperacion(-1, ex.Message, false);
                                    }

                                    //Validando que exista el Archivo
                                    if (xml != null)
                                    {
                                        //Declarando Variables Auxiliares
                                        Encoding encoding;
                                        string str = "";

                                        //Intentando encontrar codificación
                                        try
                                        {
                                            //Leyendo Stream
                                            using (var stream = new MemoryStream(xml))
                                            {
                                                //Leyendo XML
                                                using (var xmlreader = new XmlTextReader(stream))
                                                {
                                                    //Obteniendo Codificación
                                                    xmlreader.MoveToContent();
                                                    encoding = xmlreader.Encoding;
                                                }
                                            }

                                            //Obteniendo Cadena del XML
                                            str = encoding.GetString(xml);
                                        }
                                        catch (Exception ex)
                                        {
                                            //Obteniendo Cadena del XML
                                            str = Encoding.Default.GetString(xml);
                                        }

                                        //Intentando Obtener Bytes
                                        try
                                        {
                                            //Obteniendo XML en Codificación UTF8
                                            xml = Encoding.UTF8.GetBytes(str);

                                            //Instanciando resultado Positivo
                                            result = new RetornoOperacion(0, "", true);
                                        }
                                        catch (Exception ex)
                                        {
                                            //Obteniendo Bytes del Archivo
                                            xml = null;

                                            //Instanciando resultado Positivo
                                            result = new RetornoOperacion(-1, ex.Message, false);
                                        }
                                    }

                                    //Terminando Ciclo
                                    break;
                                }
                            }
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Descargando Archivo PDF
                            TSDK.Base.Archivo.DescargaArchivo(xml, string.Format("{0}_{1}{2}.xml", proveedor.rfc, factura.serie, factura.folio), TSDK.Base.Archivo.ContentType.text_xml);
                        }
                        else
                            //Mostrando Excepción
                            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                    {   
                        //Instanciando resultado Positivo
                        result = new RetornoOperacion("No Existe la Factura", false);

                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvSaldosDetalle);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbValidacion_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion retorno = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvSaldosDetalle, sender, "imb", false);

                //Instanciando Factura
                using (SAT_CL.CXP.FacturadoProveedor factura = new FacturadoProveedor(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))
                //Instanciando Proveedor
                using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(factura.id_compania_proveedor))
                {
                    //Validando que exista la Factura
                    if (factura.habilitar && proveedor.habilitar)
                    {
                        //Obteniendo Archivos
                        using (DataTable dtArchivos = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(72, factura.id_factura, 8, 0))
                        {
                            //Validando que Existan Registros
                            if (Validacion.ValidaOrigenDatos(dtArchivos))
                            {
                                //Recorriendo Archivos
                                foreach (DataRow dr in dtArchivos.Rows)
                                {
                                    //Declarando auxiliares
                                    string rfc_emisor, rfc_receptor, UUID;
                                    decimal monto; DateTime fecha_expedicion;

                                    //Obteniendo Archivo
                                    byte[] comp = System.IO.File.ReadAllBytes(dr["URL"].ToString());

                                    //Declarando Documento XML
                                    XmlDocument xmlDocument = new XmlDocument();
                                    XDocument xDocument = new XDocument();

                                    //Obteniendo XML en cadena
                                    using (MemoryStream ms = new MemoryStream(comp))
                                        //Cargando Documento XML
                                        xmlDocument.Load(ms);

                                    //Convirtiendo XML
                                    xDocument = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(xmlDocument);

                                    //Validando versión
                                    switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
                                    {
                                        case "3.2":
                                        case "3.3":
                                            {
                                                //Realizando validación de estatus en SAT
                                                retorno = SAT_CL.FacturacionElectronica.Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

                                                //Colocando resultado sobre controles
                                                imgValidacionSAT.Src = retorno.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                                                headerValidacionSAT.InnerText = retorno.Mensaje;
                                                lblRFCEmisor.Text = rfc_emisor;
                                                lblRFCReceptor.Text = rfc_receptor;
                                                lblUUIDM.Text = UUID;
                                                lblTotalFactura.Text = monto.ToString("C");
                                                lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                                                break;
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "ValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidacionSAT_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "ValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
            //Inicializando Indices
            Controles.InicializaIndices(gvSaldosDetalle);
        }
        #endregion

        #region Eventos GridView "Fichas Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFF_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFF.SelectedValue));

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkServiciosEntidad_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFichasFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasFacturas, sender, "lnk", false);

                //Obteniendo Servicios
                using (DataTable dtServiciosEntidad = SAT_CL.Bancos.EgresoIngreso.ObtieneServiciosEntidad(
                                                        Convert.ToInt32(gvFichasFacturas.SelectedDataKey["IdEntidad"]),
                                                        Convert.ToInt32(gvFichasFacturas.SelectedDataKey["IdRegistro"])))
                {
                    //Validando que existan Registros
                    if (Validacion.ValidaOrigenDatos(dtServiciosEntidad))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvServiciosEntidad, dtServiciosEntidad, "NoServicio", lblOrdenadoSE.Text);

                        //Añadiendo Tabla a Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosEntidad, "Table2");
                    }
                    else
                    {
                        //Inicilaizando GridView
                        Controles.InicializaGridview(gvServiciosEntidad);

                        //Eliminando Tabla de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    }

                    //Abriend Ventana
                    gestionaVentanas(this.Page, "ServiciosEntidad");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Instanciando Control
                using (Label lbl = (Label)e.Row.FindControl("lblServiciosEntidad"))
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lnkServiciosEntidad"))
                {
                    //Validando que existan los Controles
                    if (lbl != null && lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["IdEntidad"].ToString())
                        {
                            case "51":
                                {
                                    //Configurando Controles
                                    lbl.Visible = true;
                                    lkb.Visible = false;
                                    break;
                                }
                            case "82":
                                {
                                    //Configurando Controles
                                    lbl.Visible = false;
                                    lkb.Visible = true;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos GridView "Servicios Entidad"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosEntidad_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosEntidad_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoSE.Text = Controles.CambiaSortExpressionGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoSE.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarSE_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Mostrando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridView
            Controles.InicializaGridview(gvSaldosDetalle);

            //Invocando Método de Carga
            cargaCatalogos();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos de Tamaño de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSE, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus,"TODOS",58);
        }
        /// <summary>
        /// Método encargado de Buscar los Saldos Detalle
        /// </summary>
        private void buscaSaldosDetalle()
        {
            //Obteniendo Fechas
            DateTime fec_ini = DateTime.MinValue, fec_fin = DateTime.MinValue;

            //Validando que se Incluyan las Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }

            //Obteniendo Reporte de Saldos Detalle
            using (DataTable dtSaldosDetalle = SAT_CL.CXP.Reportes.ObtieneReporteSaldoDetalle(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), fec_ini, fec_fin,
                                                        Convert.ToByte(ddlEstatus.SelectedValue), txtSerie.Text, txtFolio.Text, txtUUID.Text))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtSaldosDetalle))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvSaldosDetalle, dtSaldosDetalle, "NoFactura", lblOrdenado.Text, true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtSaldosDetalle, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvSaldosDetalle);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Sumando Totales
                sumaTotalesDetalles();
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales al Pie del GridView
        /// </summary>
        private void sumaTotalesDetalles()
        {
            //Validando que Existan Registros
            if(Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvSaldosDetalle.FooterRow.Cells[10].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SubTotal)", "")));
                gvSaldosDetalle.FooterRow.Cells[11].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Trasladado)", "")));
                gvSaldosDetalle.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Retenido)", "")));
                gvSaldosDetalle.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
                gvSaldosDetalle.FooterRow.Cells[14].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
                gvSaldosDetalle.FooterRow.Cells[15].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoActual)", "")));
            }
            else
            {
                //Mostrando Totales
                gvSaldosDetalle.FooterRow.Cells[10].Text =
                gvSaldosDetalle.FooterRow.Cells[11].Text =
                gvSaldosDetalle.FooterRow.Cells[12].Text =
                gvSaldosDetalle.FooterRow.Cells[13].Text =
                gvSaldosDetalle.FooterRow.Cells[14].Text =
                gvSaldosDetalle.FooterRow.Cells[15].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Gestionar las Ventanas Modales
        /// </summary>
        /// <param name="sender">Control que Ejecuta la Acción</param>
        /// <param name="nombre_ventana">Nombre de la Ventana</param>
        private void gestionaVentanas(Control sender, string nombre_ventana)
        {
            //Validando Nombre
            switch (nombre_ventana)
            {
                case "FichasFacturas":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "VentanaFichasFacturas", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
                    break;
                case "ServiciosEntidad":
                    //Mostrando Ventana Modal
                    ScriptServer.AlternarVentana(sender, "VentanaServiciosEntidad", "contenidoVentanaServiciosEntidad", "ventanaServiciosEntidad");
                    //Inicializando Indices
                    Controles.InicializaIndices(gvFichasFacturas);
                    upgvFichasFacturas.Update();
                    break;
            }
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_factura"></param>
        private void buscaFichasAplicadas(int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXP.FacturadoProveedor.ObtieneAplicacionesRelacionFacturasProveedor(id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id-IdEntidad-IdRegistro", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }

            //Invocando Método de Suma
            sumaFichasFacturas();
        }
        /// <summary>
        /// Método 
        /// </summary>
        private void sumaFichasFacturas()
        {
            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = "0.00";
        }
        /// <summary>
        /// Método encargado de Exportar los Archivos XML de las Facturas de Proveedor
        /// </summary>
        private void exportaXML()
        {
            //Verificando que el GridView contiene Registros
            if (gvSaldosDetalle.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Creamos lista de archivos
                List<KeyValuePair<string, byte[]>> archivos = new List<KeyValuePair<string, byte[]>>();

                //Creamos lista errores
                List<string> errores = new List<string>();

                //Obteniendo Filas Seleccionadas
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvSaldosDetalle, "chkVarios");
                
                //Verificando que existan filas Seleccionadas
                if (selected_rows.Length != 0)
                {
                    //Almacenando Rutas el Arreglo
                    foreach (GridViewRow row in selected_rows)
                    {
                        //Asignando Indice
                        gvSaldosDetalle.SelectedIndex = row.RowIndex;

                        //Instanciando Factura
                        using (SAT_CL.CXP.FacturadoProveedor factura = new FacturadoProveedor(Convert.ToInt32(gvSaldosDetalle.SelectedDataKey["NoFactura"])))

                        //Instanciando Proveedor
                        using (SAT_CL.Global.CompaniaEmisorReceptor proveedor = new SAT_CL.Global.CompaniaEmisorReceptor(factura.id_compania_proveedor))
                        {
                            //Validando que exista la Factura
                            if (factura.habilitar && proveedor.habilitar)
                            {
                                //Declarando Objeto de Retorno
                                byte[] xml = null;

                                //Obteniendo Archivos
                                using (DataTable dtArchivos = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(72, factura.id_factura, 8, 0))
                                {
                                    //Validando que Existan Registros
                                    if (Validacion.ValidaOrigenDatos(dtArchivos))
                                    {
                                        //Recorriendo Archivos
                                        foreach (DataRow dr in dtArchivos.Rows)
                                        {
                                            try
                                            {
                                                //Obteniendo Bytes del Archivo
                                                xml = System.IO.File.ReadAllBytes(dr["URL"].ToString());

                                                //Resultado Positivo
                                                result = new RetornoOperacion(0, "", true);
                                            }
                                            catch (Exception ex)
                                            {
                                                //Obteniendo Bytes del Archivo
                                                xml = null;

                                                //Instanciando resultado Positivo
                                                result = new RetornoOperacion(-1, ex.Message, false);

                                                //Añadiendo Error
                                                errores.Add(ex.Message);
                                            }

                                            //Validando que exista el Archivo
                                            if (xml != null)
                                            {
                                                //Declarando Variables Auxiliares
                                                Encoding encoding;
                                                string str = "";

                                                //Intentando encontrar codificación
                                                try
                                                {
                                                    //Leyendo Stream
                                                    using (var stream = new MemoryStream(xml))
                                                    {
                                                        //Leyendo XML
                                                        using (var xmlreader = new XmlTextReader(stream))
                                                        {
                                                            //Obteniendo Codificación
                                                            xmlreader.MoveToContent();
                                                            encoding = xmlreader.Encoding;
                                                        }
                                                    }

                                                    //Obteniendo Cadena del XML
                                                    str = encoding.GetString(xml);
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Obteniendo Cadena del XML
                                                    str = Encoding.Default.GetString(xml);
                                                }

                                                //Intentando Obtener Bytes
                                                try
                                                {
                                                    //Obteniendo XML en Codificación UTF8
                                                    xml = Encoding.UTF8.GetBytes(str);

                                                    //Instanciando resultado Positivo
                                                    result = new RetornoOperacion(0, "", true);
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Obteniendo Bytes del Archivo
                                                    xml = null;

                                                    //Instanciando resultado Positivo
                                                    result = new RetornoOperacion(-1, ex.Message, false);
                                                }
                                            }
                                            else
                                                //Añadiendo Error
                                                errores.Add(string.Format("No se pudo recuperar la Factura '{0}{1}'", factura.serie, factura.folio));

                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                }

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Construyendo Nombre del Archivo
                                    string file_name_cxp = (factura.serie + factura.folio).Equals("") ? factura.uuid : factura.serie + factura.folio;
                                    //Añadimos XML
                                    archivos.Add(new KeyValuePair<string, byte[]>(string.Format("{0}_{1}.xml", proveedor.rfc, file_name_cxp), xml));
                                }
                            }
                            else
                                //Asignando Error
                                errores.Add("No Existe la Factura.");
                        }
                    }

                    //Validando operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Genera el zip con las rutas
                        byte[] file_zip = Archivo.ConvirteArchivoZIP(archivos, out errores);

                        //Si almenos un archivo fue correcto descarga
                        if (file_zip != null)
                        {
                            //Desmarcando Filas
                            Controles.SeleccionaFilasTodas(gvSaldosDetalle, "chkVarios", false);

                            //Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                            CheckBox chkTodos = (CheckBox)gvSaldosDetalle.HeaderRow.FindControl("chkTodos");

                            //Validando que exista el Control
                            if (chkTodos != null)

                                //Desmarcarndo Control
                                chkTodos.Checked = false;

                            //Descarga el zip generado
                            Archivo.DescargaArchivo(file_zip, string.Format("Facturas_{0:yyyyMMddHHmmss}.zip", Fecha.ObtieneFechaEstandarMexicoCentro()), Archivo.ContentType.binary_octetStream);
                        }
                    }

                    //Mostrando Mensaje de Error
                    string error_msn = "";

                    //Recorremos errores
                    foreach (string error in errores)
                    
                        //Muestra mensaje de Error
                        error_msn += error + " \n";

                    //Validando que Existan Errores
                    if (!error_msn.Equals(""))

                        //Mostrando Errores
                        ScriptServer.MuestraNotificacion(this.Page, error_msn, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, "No hay Facturas Seleccionadas", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);

                //Inicializando Indices
                Controles.InicializaIndices(gvSaldosDetalle);
            }
            else
                //Mostrando Excepción
                ScriptServer.MuestraNotificacion(this.Page, "No existen Registros", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion 
    }
}