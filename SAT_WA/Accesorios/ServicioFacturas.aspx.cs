using SAT_CL;
using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Accesorios
{
    public partial class ServicioFacturas : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesionDepositoFactura(object context, string file_name)
        {
            //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());

            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();

            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))

                //Cargando Documento XML
                doc.Load(ms);

            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID;
            decimal monto; DateTime fecha_expedicion;

            //Realizando validación de estatus en SAT
            result = Comprobante.ValidaEstatusPublicacionSAT((XmlDocument)Session["XML"], out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

            //Colocando resultado sobre controles
            imgValidacionSAT.Src = result.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
            headerValidacionSAT.InnerText = result.Mensaje;
            lblRFCEmisor.Text = rfc_emisor;
            lblRFCReceptor.Text = rfc_receptor;
            lblUUID.Text = UUID;
            lblTotalFactura.Text = monto.ToString("C");
            lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");

            //Mostrando resultado de consulta en SAT (ventana modal)
            ScriptServer.AlternarVentana(btnAgregarFactura, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
        }
        /// <summary>
        /// Realiza el procedimiento base de guardado de factura de proveedor
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardarFacturaProveedor()
        {
            //Invocando Método de Guardado
            RetornoOperacion result = guardaXML();
            
            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Cargando Facturas Ligadas
                cargaFacturasLigadas();
                
                /*/Obteniendo valores QueryString
                string idServicio = Request.QueryString.Get("idRegistro");

                //Obtiene Facturas Ligadas
                using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedor.ObtieneFacturasPorServicio(Convert.ToInt32(idServicio)))
                {
                    //Validando Registros
                    if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id", lblOrdenadoGrid.Text, true, 2);

                        //Añadiendo Tabla a Sesión
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table");
                    }
                    else
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvFacturasLigadas);

                        //Eliminando Tabla de Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    }
                }*/
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Obteniendo Control
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lnkAceptarFacturaLigada"))
                {
                    //Validando que exista el Link
                    if (lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["Estatus"].ToString())
                        {
                            case "En Revisión (Aceptar)":
                                {
                                    //Habilitando Control
                                    lkb.Enabled = true;
                                    break;
                                }
                            default:
                                {
                                    //Deshabilitando Control
                                    lkb.Enabled = false;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento que actualiza el estatus de  una factura  a Aceptada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAceptarFacturaLigada_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Selecciona  una factura ligada a servicio 
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando registro actual
                using (FacturadoProveedor FP = new FacturadoProveedor(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"])))
                {
                    //Si la Recepcion existe
                    if (FP.habilitar)
                    {
                        //Validando los Estatus donde se puede Aceptar
                        if ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                            //Actualiza el estatus del registro
                            resultado = FP.AceptaFacturaProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                        {
                            //Validando Estatus de la Factura
                            switch ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura)
                            {
                                case FacturadoProveedor.EstatusFactura.Aceptada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ya ha sido Aceptada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                case FacturadoProveedor.EstatusFactura.Liquidada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura tiene Pagos Aplicados.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Refacturacion:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Refacturada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Cancelada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Cancelada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Rechazada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Rechazada.");
                                        break;
                                    }
                            }
                        }
                    }
                }

                //Valida la acción de actualización
                if (resultado.OperacionExitosa)

                    //Invoca al método cargaFacturasLigadas()
                    cargaFacturasLigadas();

                //Envia un mensaje con el resultado de la operación. 
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar la Relación de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFacturaLiq_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Decalrando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Instanciando Relación
                using (FacturadoProveedorRelacion fpr = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["IdFPR"])))
                {
                    //Validando que exista la Relación
                    if (fpr.habilitar)
                    {
                        //Validando Entidad
                        switch (fpr.id_tabla)
                        {
                            case 51:
                                {
                                    //Instanciando Deposito
                                    using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(fpr.id_registro))
                                    {
                                        //Validando el Estatus del Deposito
                                        if (dep.habilitar && dep.Estatus != SAT_CL.EgresoServicio.Deposito.EstatusDeposito.PorLiquidar)
                                        
                                            //Resultado Positivo
                                            result = new RetornoOperacion(0, "", true);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Anticipo se encuentra Depositado, Imposible Eliminar la Relación con la Factura");
                                    }
                                    break;
                                }
                            case 82:
                                {
                                    //Instanciando Deposito
                                    using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(fpr.id_registro))
                                    {
                                        //Validando el Estatus del Deposito
                                        if (liq.habilitar && liq.estatus != SAT_CL.Liquidacion.Liquidacion.Estatus.Depositado)
                                            
                                            //Resultado Positivo
                                            result = new RetornoOperacion(0, "", true);
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("La Liquidación se encuentra Depositada, Imposible Eliminar su Relación con la Factura");
                                    }
                                    break;
                                }
                        }
                        
                        //Si se puede eliminar
                        if (result.OperacionExitosa)
                        
                            //Deshabilitando Factura
                            result = fpr.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        
                    }
                    else
                        //Instanciando Excepcion
                        result = new RetornoOperacion("No Existe la Relación de Facturas");

                    //Validando que se Deshabilitara
                    if (result.OperacionExitosa)
                    {
                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturasLigadas);

                        //Recargando Facturas Ligadas
                        cargaFacturasLigadas();
                    }
                }

                //Envia un mensaje con el resultado de la operación. 
                ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento click en ventana modal de confirmación de validación de factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidacionSAT_Click(object sender, EventArgs e)
        {
            //Determinando respuesta del usuario
            switch (((Button)sender).CommandName)
            {
                case "Descartar":
                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnCanelarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
                case "Continuar":
                    //Realizando proceso de guardado de factura de proveedor
                    guardarFacturaProveedor();

                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnAceptarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
            }
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
            TSDK.ASP.Controles.InicializaIndices(gvFacturasDisponibles);
            TSDK.ASP.Controles.InicializaIndices(gvFacturasLigadas);
        }

        #region Eventos GridView "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoFL.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link 'Relación'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRelacionAplicacionFacLig_Click(object sender, EventArgs e)
        {
            //Validando que existan Facturas Ligadas
            if (gvFacturasLigadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasLigadas, sender, "lnk", false);

                //Invocando Método de Busqueda
                buscaAplicacionesRelacion(Convert.ToInt32(gvFacturasLigadas.SelectedDataKey["Id"]));

                //Cerrando ventana modal 
                ScriptServer.AlternarVentana(gvFacturasLigadas, "RelacionesAplicaciones", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
            }
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "FacturasLigadas":
                    //Exportando Contenido
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                    break;
                case "FacturasLiq":
                    //Exportando Contenido
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                    break;
            }
        }

        #endregion

        #region Eventos "Facturas Existentes"

        /// <summary>
        /// Evento Producido al Buscar Facturas Disponibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarFac_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscarFacturasEntidad();
        }
        /// <summary>
        /// Evento Producido al Ligar las Facturas a la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLigarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Obteniendo Deposito
                string idTipoEnt = Request.QueryString.Get("idRegistro");
                string idRegistro = Request.QueryString.Get("idRegistroB");
                
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Filas Seleccionadas
                GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFac");

                //Si existen Filas Seleccionadas
                if (gvr.Length > 0)
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Recorriendo Filas
                        foreach (GridViewRow gv in gvr)
                        {
                            //Seleccionando Fila
                            gvFacturasDisponibles.SelectedIndex = gv.RowIndex;

                            //Instanciando Factura de Proveedor
                            using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"])))
                            {
                                //Insertando Relación Factura Proveedor
                                result = SAT_CL.CXP.FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"]),
                                                                        Convert.ToInt32(idTipoEnt), Convert.ToInt32(idRegistro), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando el Resultado
                                if (result.IdRegistro > 0 || result.IdRegistro == -1)
                                {
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(btnLigarFactura, string.Format("ResultadoNotificacion_{0}", Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"])),
                                                    string.Format(result.OperacionExitosa ? "La Factura '{0}{1}' ha sido añadida {2}" : "La Factura '{0}{1}' ya se encuentra ligada {2}",
                                                    fp.serie, fp.folio, idTipoEnt.Equals("51") ? "al Deposito." : "a la Liquidación"),
                                                    result.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                    //Instanciando 
                                    result = result.IdRegistro == -1 ? new RetornoOperacion("", true) : result;
                                }
                                else
                                    //Terminando Ciclo
                                    break;
                            }
                        }

                        //Validando Operaciones Exitosas
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }

                    //Validando Relaciones
                    if (result.OperacionExitosa)
                    {
                        //Inicializando Indices
                        Controles.InicializaIndices(gvFacturasDisponibles);

                        //Invocando Métodos de Busqueda
                        buscarFacturasEntidad();
                        cargaFacturasLigadas();
                    }
                }
                else
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(btnLigarFactura, "Debe Seleccionar Facturas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Vista de las Facturas de Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerFacturasLiq_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Nueva":
                    {
                        //Mostrando Vista
                        mtvFacturasLiquidacion.ActiveViewIndex = 1;

                        //Limpiando Sessión
                        Session["XML"] =
                        Session["XMLFileName"] = null;
                        break;
                    }
                case "Ver":
                    {
                        //Mostrando Vista
                        mtvFacturasLiquidacion.ActiveViewIndex = 0;

                        //Invocando Métodos de Busqueda
                        buscarFacturasEntidad();
                        cargaFacturasLigadas();
                        break;
                    }
            }
        }

        #endregion

        #region Eventos GridView "Facturas Disponibles"

        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Obteniendo Control
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lnkAceptarFacturaDisponible"))
                {
                    //Validando que exista el Link
                    if (lkb != null)
                    {
                        //Validando Tipo de Registro
                        switch (row["Estatus"].ToString())
                        {
                            case "En Revisión (Aceptar)":
                                {
                                    //Habilitando Control
                                    lkb.Enabled = true;
                                    break;
                                }
                            default:
                                {
                                    //Deshabilitando Control
                                    lkb.Enabled = false;
                                    break;
                                }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Marcar/Desmarcar una Factura Disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosFac_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Obteniendo Control
                CheckBox chk = (CheckBox)sender;

                //Validando que Exista el Control
                if (chk != null)
                {
                    //Validando ID del Control
                    switch (chk.ID)
                    {
                        case "chkTodosFac":
                            {
                                //Gestionando Filas
                                Controles.SeleccionaFilasTodas(gvFacturasDisponibles, "chkVariosFac", chk.Checked);
                                break;
                            }
                        case "chkVariosFac":
                            {
                                //Obteniendo Filas Seleccionadas
                                GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvFacturasDisponibles, "chkVariosFac");

                                //Si existen Filas
                                if (gvr != null)
                                {
                                    //Obteniendo Encabezado
                                    CheckBox header = (CheckBox)gvFacturasDisponibles.HeaderRow.FindControl("chkTodosFac");

                                    //Validando que exista el Control
                                    if (header != null)
                                    {
                                        //Validando que 
                                        if (gvr.Length == gvFacturasDisponibles.Rows.Count)

                                            //Marcando Control de Encabezado
                                            header.Checked = true;
                                        else
                                            //Desmarcando Control de Encabezado
                                            header.Checked = false;
                                    }

                                }
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacLiq_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del gridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFacLiq.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoFacLiq.Text = Controles.CambiaSortExpressionGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Facturas Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFacturasDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkAceptarFacturaDisponible_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Selecciona  una factura ligada a servicio 
                Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Instanciando registro actual
                using (FacturadoProveedor FP = new FacturadoProveedor(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"])))
                {
                    //Si la Recepcion existe
                    if (FP.habilitar)
                    {
                        //Validando los Estatus donde se puede Aceptar
                        if ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                            //Actualiza el estatus del registro
                            resultado = FP.AceptaFacturaProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                        {
                            //Validando Estatus de la Factura
                            switch ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura)
                            {
                                case FacturadoProveedor.EstatusFactura.Aceptada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ya ha sido Aceptada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                                case FacturadoProveedor.EstatusFactura.Liquidada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura tiene Pagos Aplicados.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Refacturacion:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Refacturada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Cancelada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Cancelada.");
                                        break;
                                    }
                                case FacturadoProveedor.EstatusFactura.Rechazada:
                                    {
                                        //Instanciando Excepción
                                        resultado = new RetornoOperacion("La Factura ha sido Rechazada.");
                                        break;
                                    }
                            }
                        }
                    }
                }

                //Valida la acción de actualización
                if (resultado.OperacionExitosa)

                    //Invoca al método de facturas disponibles
                    buscarFacturasEntidad();

                //Envia un mensaje con el resultado de la operación. 
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link 'Relación'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRelacionAplicacionFacDisp_Click(object sender, EventArgs e)
        {
            //Validando que existan Facturas Ligadas
            if (gvFacturasDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFacturasDisponibles, sender, "lnk", false);

                //Invocando Método de Busqueda
                buscaAplicacionesRelacion(Convert.ToInt32(gvFacturasDisponibles.SelectedDataKey["Id"]));

                //Cerrando ventana modal 
                ScriptServer.AlternarVentana(gvFacturasDisponibles, "RelacionesAplicaciones", "contenidoVentanaFichasFacturas", "ventanaFichasFacturas");
            }
        }

        #endregion

        #region Eventos GridView "Fichas Facturas"

        /// <summary>
        /// Evento Producido al Enlazar Datos al GridView
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
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 1);

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
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 1);

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
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFF.SelectedValue), true, 1);

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
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
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
            Controles.CambiaIndicePaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosEntidad_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoSE.Text = Controles.CambiaSortExpressionGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServiciosEntidad, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoSE.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarSE_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"));
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogo();
            
            //Inicializando GridView
            Controles.InicializaGridview(gvFacturasLigadas);
            Controles.InicializaGridview(gvFacturasDisponibles);

            //Invocando Método de Carga
            cargaFacturasLigadas();
            buscarFacturasEntidad();
        }
        /// <summary>
        /// Método encargado de Cargar el Catalogo
        /// </summary>
        private void cargaCatalogo()
        {
            //Tamaño de Gridview
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 18);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 18);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSE, "", 18);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacLiq, "", 26);
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

        #region Métodos "Relacion y Aplicación de Facturas"

        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_factura"></param>
        private void buscaAplicacionesRelacion(int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXP.FacturadoProveedor.ObtieneAplicacionesRelacionFacturasProveedor(id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id-IdEntidad-IdRegistro", "", true, 2);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
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
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[6].Text = "0.00";
        }

        #endregion        

        #region Métodos "Facturas Ligadas"

        /// <summary>
        /// Método encargado de Cargar las Facturas Ligadas
        /// </summary>
        private void cargaFacturasLigadas()
        {
            //Obteniendo valores QueryString
            string idTipoEntidad = Request.QueryString.Get("idRegistro");
            string idRegistro = Request.QueryString.Get("idRegistroB");
            string idServicio = Request.QueryString.Get("idRegistroC");

            //Obtiene Facturas Ligadas
            using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(Convert.ToInt32(idTipoEntidad), Convert.ToInt32(idRegistro), false))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id-IdFPR", lblOrdenadoGrid.Text, true, 2);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasLigadas);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private RetornoOperacion validaFacturaXML()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Limpiando Mensaje
            string mensaje = "";

            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];

                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idProveedorEmisor = 0;

                            //Asignando Emisor
                            idProveedorEmisor = emi.id_compania_emisor_receptor;

                            //Instanciando Proveedor
                            result = new RetornoOperacion(idProveedorEmisor);

                            //Validando que el RFC sea igual
                            if (idProveedorEmisor > 0)
                            {
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que el RFC sea igual
                                    if (cer.rfc.ToUpper() == doc.DocumentElement["cfdi:Receptor"].Attributes["rfc"].Value.ToUpper())
                                    {
                                        //Instanciando XSD de validación
                                        using (EsquemasFacturacion ef = new EsquemasFacturacion(doc["cfdi:Comprobante"].Attributes["version"].Value))
                                        {
                                            //Validando que exista el XSD
                                            if (ef.id_esquema_facturacion != 0)
                                            {
                                                //Declarando variables Auxiliares
                                                bool addenda;

                                                //Obteniendo XSD
                                                string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(ef.version, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out addenda);

                                                //Validando que Existen Addendas
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
                                                    doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                //Obteniendo Validación
                                                bool value = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);

                                                //Instanciando Resultado
                                                result = new RetornoOperacion(mensaje, value);
                                            }
                                        }
                                    }
                                    else
                                        //Asignando Negativa el Objeto de retorno
                                        result = new RetornoOperacion("El RFC de la factura no coincide con el Receptor");
                                }
                            }
                            else
                                //Asignando Negativa el Objeto de retorno
                                result = new RetornoOperacion("El RFC de la factura no coincide con el Emisor");
                        }
                    }
                    catch (Exception e)
                    {
                        //Asignando Negativa el Objeto de retorno
                        result = new RetornoOperacion(e.Message);
                    }
                }
                else
                    //Mensaje de Error
                    result = new RetornoOperacion("No se ha podido cargar el Archivo");
            }
            else
                //Mensaje de Error
                result = new RetornoOperacion("No se ha podido localizar el Archivo");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Guardar la Factura apartir de un XML
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable para Factura
            int idFactura = 0;

            //Obteniendo valores QueryString
            string idTipoEntidad = Request.QueryString.Get("idRegistro");
            string idEntidad = Request.QueryString.Get("idRegistroB");

            //Inicializando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando Archivo XML
                if (Session["XML"] != null)
                {
                    //Obteniendo Archivo XML
                    XDocument documento = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument((XmlDocument)Session["XML"]);
                    XNamespace ns = documento.Root.GetNamespaceOfPrefix("cfdi");

                    //Validando versión
                    switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                    {
                        case "3.2":
                            {
                                //Instanciando Emisor-Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(documento.Root.Element(ns + "Emisor").Attribute("rfc").Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que el Proveedor 
                                    if (emisor.id_compania_emisor_receptor == obtieneProveedorEntidad(idTipoEntidad, idEntidad))

                                        //Insertando CFDI 3.2
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 0, Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Proveedor no coincide con el de la Factura");
                                }
                                break;
                            }
                        case "3.3":
                            {
                                //Instanciando Emisor-Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(documento.Root.Element(ns + "Emisor").Attribute("Rfc").Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {
                                    //Validando que el Proveedor 
                                    if (emisor.id_compania_emisor_receptor == obtieneProveedorEntidad(idTipoEntidad, idEntidad))

                                        //Insertando CFDI 3.3
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 0, Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Proveedor no coincide con el de la Factura");
                                    break;
                                }
                            }
                    }

                    //Validando Operación Exitosa
                    if (result.OperacionExitosa)
                    {
                        //Guardando Factura Nueva
                        idFactura = result.IdRegistro;

                        //Insertando Relación de Factura Proveedor con la Entidad
                        result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, Convert.ToInt32(idTipoEntidad), Convert.ToInt32(idEntidad), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
            }

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoCentro);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Público encargado de Guardar la Factura apartir de un XML
        /// </summary>
        private RetornoOperacion guardaFacturaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable para Factura
            int idFactura = 0;

            //Obteniendo valores QueryString
            string idTipoEntidad = Request.QueryString.Get("idRegistro");
            string idEntidad = Request.QueryString.Get("idRegistroB");

            //Inicializando transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Validando XML
                result = validaFacturaXML();

                //Validando Resultado
                if (result.OperacionExitosa)
                {
                    try
                    {
                        //Declarando Documento XML
                        XmlDocument doc = (XmlDocument)Session["XML"];

                        //Declarando variables de Montos
                        decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                        //Obteniendo Valores
                        obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                        //Instanciando Emisor-Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Validando que coincida el RFC del Emisor
                            if (emisor.id_compania_emisor_receptor > 0)
                            {
                                //Validando que el Proveedor 
                                if (emisor.id_compania_emisor_receptor == obtieneProveedorEntidad(idTipoEntidad, idEntidad))
                                {
                                    //Instanciando Emisor-Compania
                                    using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                    {
                                        //Validando que coincida el RFC del Receptor
                                        if (receptor.rfc.Equals(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value))
                                        {
                                            //Declarando Variables Auxiliares
                                            decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                            /** Retenciones **/
                                            //Validando que no exista el Nodo
                                            if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                            {
                                                //Validando que existan Retenciones
                                                if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                {
                                                    //Validando que existan Nodos
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                    {
                                                        //Recorriendo Retenciones
                                                        foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                        {
                                                            //Sumando Impuestos Retenidos
                                                            totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                //Asignando Total de Impuestos
                                                totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                            /** Traslados **/
                                            //Validando que no exista el Nodo
                                            if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                            {
                                                //Validando que existan Traslados
                                                if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                {
                                                    //Validando que existan Nodos
                                                    if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                    {
                                                        //Recorriendo Traslados
                                                        foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                        {
                                                            //Sumando Impuestos Trasladados
                                                            totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                                //Asignando Total de Impuestos
                                                totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);

                                            //Insertando factura
                                            result = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                Convert.ToInt32(idTipoEntidad.Equals("51") ? Request.QueryString.Get("idRegistroC") : "0"),
                                                                                doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                                                "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                totalImpTrasladados, totalImpRetenidos,
                                                                                doc["cfdi:Comprobante"].Attributes["Moneda"] == null ? "" : doc["cfdi:Comprobante"].Attributes["Moneda"].Value,
                                                                                monto_tc, Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), total_p, subtotal_p, descuento_p, traslado_p,
                                                                                retenido_p, false, DateTime.MinValue, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value),
                                                                                doc["cfdi:Comprobante"].Attributes["condicionesDePago"] == null ? "" : doc["cfdi:Comprobante"].Attributes["condicionesDePago"].Value,
                                                                                emisor.dias_credito, 1, (byte)FacturadoProveedor.EstatusValidacion.ValidacionSintactica, "",
                                                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando Operación
                                            if (result.OperacionExitosa)
                                            {
                                                //Obteniendo Factura
                                                idFactura = result.IdRegistro;

                                                //Obteniendo Nodos de Concepto
                                                XmlNodeList xmlNL = doc.GetElementsByTagName("cfdi:Concepto");

                                                //Declarando Variables Auxiliares
                                                decimal tasa_imp_ret, tasa_imp_tras;
                                                bool res = true;
                                                int contador = 0;

                                                //Recorriendo cada
                                                while (res)
                                                {
                                                    //Obteniendo Concepto
                                                    XmlNode node = xmlNL[contador];
                                                    //Obteniendo Cantidades del Concepto
                                                    obtieneCantidadesConcepto(doc, node, out tasa_imp_tras, out tasa_imp_ret);
                                                    //Insertando Cocepto de Factura
                                                    result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                                            node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                                            node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                                            Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                                            Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                                            tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    //Incrementando Contador
                                                    contador++;
                                                    //Obteniendo resultado del Ciclo
                                                    res = contador >= xmlNL.Count ? false : result.OperacionExitosa;
                                                }

                                                //Validando resultado
                                                if (result.OperacionExitosa)
                                                {
                                                    //Declarando Variables Auxiliares
                                                    string ruta;
                                                    //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                                    ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());
                                                    //Insertamos Registro
                                                    result = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                                        Encoding.UTF8.GetBytes(doc.OuterXml), ruta);

                                                    //Validando Operación Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Insertando Relación de Factura Proveedor con la Entidad
                                                        result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, Convert.ToInt32(idTipoEntidad), Convert.ToInt32(idEntidad), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando Operación Exitosa
                                                        if (result.OperacionExitosa)

                                                            //Completando Transacción
                                                            trans.Complete();
                                                    }
                                                }
                                            }

                                        }
                                        else
                                            //Instanciando Excepcion
                                            result = new RetornoOperacion("La Compania Receptora no esta registrada");
                                    }
                                }
                                else
                                    //Instanciando Excepcion
                                    result = new RetornoOperacion(string.Format("El Proveedor '{0}[{1}]' no coincide el ingresado ", emisor.nombre, emisor.rfc));
                            }
                            else
                                //Instanciando Excepcion
                                result = new RetornoOperacion("El Compania Proveedora no esta registrado");
                        }
                    }
                    catch (Exception e)
                    {
                        //Mostrando Excepción
                        result = new RetornoOperacion(e.Message);
                    }
                }
            }

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoCentro);

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Obtener el Proveedor de la Entidad
        /// </summary>
        /// <returns></returns>
        private int obtieneProveedorEntidad(string tipoEntidad, string idRegistro)
        {
            //Declarando Objeto de Retorno
            int idProveedor = 0;

            //Validando Tipo
            switch (tipoEntidad)
            {
                case "51":
                    {
                        //Instanciando Deposito
                        using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(idRegistro)))
                        {
                            //Validando que exista el Deposito
                            if (dep.habilitar)

                                //Asignando Proveedor
                                idProveedor = dep.objDetalleLiquidacion.id_proveedor_compania;
                        }

                        break;
                    }
                case "82":
                    {
                        //Obteniendo Liquidación
                        string idLiquidacion = Request.QueryString.Get("idRegistroB");

                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(idRegistro)))
                        {
                            //Validando que exista la Liquidación
                            if (liq.habilitar)

                                //Asignando Proveedor
                                idProveedor = liq.id_proveedor;
                        }
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return idProveedor;
        }
        /// <summary>
        /// Método encargado de Obtener el Proveedor de la Entidad
        /// </summary>
        /// <returns></returns>
        private int obtieneCompaniaEntidad(string tipoEntidad, string idRegistro)
        {
            //Declarando Objeto de Retorno
            int idCompania = 0;

            //Validando Tipo
            switch (tipoEntidad)
            {
                case "51":
                    {
                        //Instanciando Deposito
                        using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(idRegistro)))
                        {
                            //Validando que exista el Deposito
                            if (dep.habilitar)

                                //Asignando Proveedor
                                idCompania = dep.id_compania_emisor;
                        }

                        break;
                    }
                case "82":
                    {
                        //Obteniendo Liquidación
                        string idLiquidacion = Request.QueryString.Get("idRegistroB");

                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(idRegistro)))
                        {
                            //Validando que exista la Liquidación
                            if (liq.habilitar)

                                //Asignando Proveedor
                                idCompania = liq.id_compania_emisora;
                        }
                        break;
                    }
            }

            //Devolviendo Resultado Obtenido
            return idCompania;
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de la Factura
        /// </summary>
        /// <param name="document">Factura XML</param>
        /// <param name="total_p">Total en Pesos</param>
        /// <param name="subtotal_p">Subtotal en Pesos</param>
        /// <param name="descuento_p">Descuento en Pesos</param>
        /// <param name="traslado_p">Importe Trasladado en Pesos</param>
        /// <param name="retenido_p">Importe Retenido en Pesos</param>
        /// <param name="monto_tc">Monto del Tipo de Cambio</param>
        private void obtieneCantidades(XmlDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
        {
            //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {
                //Asignando Atributo Obligatorios
                monto_tc = 1;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value);
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value);
                traslado_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0;
                retenido_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0;

                //Asignando Atributos Opcionales
                descuento_p = document.DocumentElement.Attributes["descuento"] == null ? 0 :
                    Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value);
            }
            else
            {
                //Asignando Atributo Obligatorios
                decimal.TryParse(document.DocumentElement.Attributes["TipoCambio"].Value, out monto_tc);
                monto_tc = monto_tc == 0 ? 1 : monto_tc;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value) * monto_tc;
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value) * monto_tc;
                traslado_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0) * monto_tc;
                retenido_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0) * monto_tc;

                //Asignando Atributos Opcionales
                descuento_p = (document.DocumentElement.Attributes["descuento"] == null ? 0 : Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value)) * monto_tc;
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private void obtieneCantidadesConcepto(XmlDocument cfdi, XmlNode concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {
            //Validación de Retenciones
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
            {
                //Validando que el Importe no sea "0"
                if (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) > 0 && Convert.ToDecimal(concepto.Attributes["importe"].Value) > 0)
                {
                    //Asignando Valores
                    tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
                }
                else
                    //Asignando Valores
                    tasa_imp_ret = 0;
            }
            else
                //Asignando Valores
                tasa_imp_ret = 0;

            //Validación de Traslados
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
            {
                //Obteniendo Valor de la Tasa
                string tasaImpTrasladado = cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value;

                //Remplazando Puntos Decimales
                tasaImpTrasladado = tasaImpTrasladado.Replace(".", "|");

                //Validando que exista un valor despues del Punto decimal
                if (Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1)) > 0.00M)

                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1));
                else
                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 0));
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }

        #endregion

        #region Métodos "Facturas Existentes"

        /// <summary>
        /// Método encargado de Buscar las Facturas Disponibles
        /// </summary>
        private void buscarFacturasEntidad()
        {
            //Obteniendo Deposito
            string idTipoEntidad = Request.QueryString.Get("idRegistro");
            string idRegistro = Request.QueryString.Get("idRegistroB");

            //Obteniendo Facturas Disponibles
            using (DataTable dtFacturasDisponibles = SAT_CL.CXP.Reportes.ObtieneFacturasDisponiblesEntidad(
                                                            obtieneCompaniaEntidad(idTipoEntidad, idRegistro),
                                                            obtieneProveedorEntidad(idTipoEntidad, idRegistro), 
                                                            txtSerie.Text, Convert.ToInt32(txtFolio.Text == "" ? "0" : txtFolio.Text),
                                                            txtUUID.Text))
            {
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(dtFacturasDisponibles))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFacturasDisponibles, dtFacturasDisponibles, "Id", lblOrdenadoFacLiq.Text, true, 3);

                    //Añadiendo Tabla a Sessión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasDisponibles, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFacturasDisponibles);

                    //Eliminando Tabla de Sessión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        #endregion

        #endregion
    }
}