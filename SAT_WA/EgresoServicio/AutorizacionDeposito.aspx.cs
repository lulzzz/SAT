using SAT_CL;
using SAT_CL.Autorizacion;
using SAT_CL.CXP;
using SAT_CL.EgresoServicio;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.EgresoServicio
{
    public partial class AutorizacionDeposito : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento generado al cargar la página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es una recarga de página
            if (!this.Page.IsPostBack)
                //Inicializando página
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

            //Invocando Método de Guardado
            result = guardaXML();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Obtiene Facturas Ligadas
                using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedor.ObtieneFacturasPorServicio(Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdServicio"])))
                {
                    //Validando Registros
                    if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id", lblOrdenadoGrid.Text, true, 1);

                        //Añadiendo Tabla a Sesión
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table1");
                    }
                    else
                    {
                        //Inicializando GridView
                        Controles.InicializaGridview(gvFacturasLigadas);

                        //Eliminando Tabla de Sesión
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    }
                }

                //Obteniendo Autorización
                int idAutorizacion = Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdServicio"]);

                //Cargando Autorizaciones
                cargaDetalleAutorizaciones();

                //Marcando Fila
                Controles.MarcaFila(gvAutorizaciones, idAutorizacion.ToString(), "Id", "Id-IdDeposito-IdServicio-IdFactura", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenarAutorizaciones.Text, Convert.ToInt32(ddlTamanoAutorizaciones.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentana_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Validando Comando
            switch (lnk.CommandName)
            {
                case "FacturadoConcepto":
                    {
                        //Abriendo Ventana Modal
                        ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                        break;
                    }
                case "FacturasLigadas":
                    {
                        //Abriendo Ventana Modal
                        ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Facturas Ligadas", "contenedorVentanaFacturasLigadas", "ventanaFacturasLigadas");
                        break;
                    }
                case "ReferenciasViaje":
                    {
                        //Abriendo Ventana Modal
                        ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Referencias Viaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                        break;
                    }
            }

            //Inicializando Indices
            Controles.InicializaIndices(gvAutorizaciones);

            //Actualizando Reporte
            upgvAutorizaciones.Update();
        }

        #region Eventos GridView "Autorizacion"

        /// <summary>
        /// Evento producido al dar click sobre algún botón de autorización pendiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAutorizacion_Click(object sender, EventArgs e)
        {
            //Si existen registraos en el Gridview
            if (gvAutorizaciones.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvAutorizaciones, sender, "lnk", false);

                //Determinando el botón pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Referencias":
                        inicializaReferencias(Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdDeposito"]), 51, "Referencias Depósito");
                        break;
                    case "Bitacora":
                        inicializaBitacoraRegistro(gvAutorizaciones.SelectedDataKey["IdDeposito"].ToString(), "51", "Bitacorá Depósito");
                        break;
                }
            }
        }
        /// <summary>
        /// Evento producido al cambiar la selección del elemento todos en el GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosAutorizacion_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros activos
            if (gvAutorizaciones.DataKeys.Count > 0)
            {
                //Referenciando al control que produjo el evento
                CheckBox todos = (CheckBox)sender;

                //Referenciando a la etiqueta contador del pie
                using (Label c = (Label)gvAutorizaciones.FooterRow.FindControl("lblSeleccionAutorizacion"))
                    //Realizando la selección de todos los elementos del GridView
                    c.Text = Controles.SeleccionaFilasTodas(gvAutorizaciones, "chkAutorizacion", todos.Checked).ToString();
            }
        }
        /// <summary>
        /// Evento producido al cambiar la selección de algún elemento autorización pendiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAutorizacion_CheckedChanged(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvAutorizaciones.DataKeys.Count > 0)
            {
                //Obteniendo el control que ha sido afectado
                CheckBox d = (CheckBox)sender;

                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvAutorizaciones, sender, "chk", false);
                //Suma cada elemento marcado
                Controles.SumaSeleccionadosFooter(d.Checked, gvAutorizaciones, "lblSeleccionAutorizacion");

                //Si retiró selección
                if (!d.Checked)
                {
                    //Referenciando control de encabezado
                    CheckBox t = (CheckBox)gvAutorizaciones.HeaderRow.FindControl("chkTodosAutorizacion");

                    t.Checked = d.Checked;
                }
            }
        }

        /// <summary>
        /// Evento producido al pulsar algún botón de exportación de contenido de GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {
            //Determinado el botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "Autorizaciones":
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToCharArray());
                    break;
            }
        }

        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewAutorizaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            Controles.CambiaTamañoPaginaGridView(gvAutorizaciones, mit, Convert.ToInt32(ddlTamanoAutorizaciones.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento producido al cambiar el índice de página del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAutorizaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            Controles.CambiaIndicePaginaGridView(gvAutorizaciones, mit, e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GridView de Detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAutorizaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Obteniendo origen de datos
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            //Cambiando indice de página
            lblOrdenarAutorizaciones.Text = Controles.CambiaSortExpressionGridView(gvAutorizaciones, mit, e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView "Autorizaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAutorizaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Origen de la Fila
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                //Instanciando Control de Total de Facturas de Servicio
                using (LinkButton lbkTotalFacS = (LinkButton)e.Row.FindControl("lkbTotalCobro"))
                //Instanciando Control de Total de Facturas de Servicio
                using (LinkButton lkbTotalFacL = (LinkButton)e.Row.FindControl("lkbTotalFL"))
                {
                    //Validando que existan los Controles
                    if (lbkTotalFacS != null && lkbTotalFacL != null)
                    {
                        //Obteniendo Indicador
                        int idServicio = Convert.ToInt32(rowView["IdServicio"].ToString() == "" ? "0" : rowView["IdServicio"].ToString());

                        //Validando que exista el Servicio
                        if (idServicio > 0)
                        {
                            //Habilitando Controles
                            lbkTotalFacS.Enabled =
                            lkbTotalFacL.Enabled = true;
                        }
                        else
                        {
                            //Deshabilitando Controles
                            lbkTotalFacS.Enabled =
                            lkbTotalFacL.Enabled = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click sobre los Links de Totales del Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbTotalCobro_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvAutorizaciones, sender, "lnk", false);

            //Validando Comando
            switch (lnk.CommandName)
            {
                case "TotalCobro":
                    {
                        //Instanciando Factura
                        using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdFactura"])))
                        {
                            //Validando que exista la factura
                            if (fac.habilitar)
                            {
                                //Inicializando Control
                                ucFacturadoConcepto.InicializaControl(fac.id_factura);

                                //Alternando Ventana
                                ScriptServer.AlternarVentana(lnk, "FacturadoConcepto", "Edicion Conceptos", "contenedorVentanaEdicionConceptos", "ventanaEdicionConceptos");
                            }
                        }

                        break;
                    }
                case "TotalFacturasLigadas":
                    {
                        //Obtiene Facturas Ligadas
                        using (DataTable dtFacturasLigadas = SAT_CL.CXP.FacturadoProveedorRelacion.ObtieneFacturasEntidad(51, Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdDeposito"]), true))
                        {
                            //Validando Registros
                            if (Validacion.ValidaOrigenDatos(dtFacturasLigadas))
                            {
                                //Cargando GridView
                                Controles.CargaGridView(gvFacturasLigadas, dtFacturasLigadas, "Id-IdFPR", lblOrdenadoGrid.Text, true, 1);

                                //Añadiendo Tabla a Sesión
                                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturasLigadas, "Table1");
                            }
                            else
                            {
                                //Inicializando GridView
                                Controles.InicializaGridview(gvFacturasLigadas);

                                //Eliminando Tabla de Sesión
                                Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                            }
                        }

                        //Abriendo Ventana Modal
                        ScriptServer.AlternarVentana(lnk, lnk.GetType(), "Facturas Ligadas", "contenedorVentanaFacturasLigadas", "ventanaFacturasLigadas");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "No. Viaje"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReferenciasViaje_Click(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvAutorizaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAutorizaciones, sender, "lnk", false);

                //Inicializando Referencias de Viaje
                ucReferenciasViaje.InicializaControl(Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdServicio"]));

                //Abriendo Ventana Modal
                ScriptServer.AlternarVentana(this, "Referencias Viaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
            }
        }

        #endregion

        #region Eventos GridView "Factura Proveedor"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando el Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoFL.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            //Asignando Expresión de Ordenamiento
            lblOrdenadoGrid.Text = Controles.CambiaSortExpressionGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasLigadas_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            //Cambiando Página
            Controles.CambiaIndicePaginaGridView(gvFacturasLigadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Exportar el Contenido del GridView "Factura Proveedor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }

        #endregion

        /// <summary>
        /// Evento producido al pulsar el botón Autorizar o Rechazar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAutorizarRechazar_Click(object sender, EventArgs e)
        {
            //Definiendo objerto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Obteniendo los elementos seleccionados
            GridViewRow[] filas = Controles.ObtenerFilasSeleccionadas(gvAutorizaciones, "chkAutorizacion");

            //Definiendo mensaje final
            string mensaje = "";

            //Si existen filas seleccionadas
            if (filas.Length > 0)
            {
                //Definiendo variable de actualización general de resultados
                bool actualizar = false;

                //Para cada uno de los elementos seleccionados
                foreach (GridViewRow r in filas)
                {
                    //Seleccionando la fila actual
                    gvAutorizaciones.SelectedIndex = r.RowIndex;

                    //Instanciando autorización
                    using (AutorizacionRealizada a = new AutorizacionRealizada(Convert.ToInt32(gvAutorizaciones.SelectedDataKey.Value)))
                    {
                        //Si la autorización se encontró
                        if (a.id_autorizacion_realizada > 0)
                        {
                            //Instanciando depósito
                            using (Deposito d = new Deposito(Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdDeposito"])))
                            {
                                //Si el depósito existe
                                if (d.id_deposito > 0)
                                {
                                    //Validamos estatus 
                                    if (d.Estatus == SAT_CL.EgresoServicio.Deposito.EstatusDeposito.EnAutorizacion)
                                    {

                                        //Referenciando al usuario en sesión
                                        using (Usuario u = (Usuario)Session["usuario"])
                                        {
                                            //Instanciando detalle de concepto aplicable
                                            using (ConceptoRestriccion dc = new ConceptoRestriccion(ConceptoRestriccion.ObtieneConceptoRestriccionCoincidenteClasificacion(d.objDetalleLiquidacion.id_servicio, d.id_concepto)))
                                            {
                                                //Determinando si es requerido un cambio en el estatus de depósito
                                                AutorizacionDetalle da = Autorizacion.CargaAutorizacionesAplicablesRegistro(Autorizacion.TipoAutorizacion.MontoDepositoServicio, 57, dc.id_concepto_restriccion, d.objDetalleLiquidacion.monto.ToString());

                                                //Inicialzaindo transacciones
                                                //Creamos la transacción 
                                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                {
                                                    //Realizando la acción solicitada para la selección
                                                    switch (((Button)sender).CommandName)
                                                    {
                                                        case "Autorizar":
                                                            //Actualizando confirmación de autorización
                                                            resultado = a.ConfirmaAutorizacion(true, u.id_usuario);
                                                            //Si no existe error
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Validando existencia de negaciones
                                                                resultado = da.ValidaAutorizacionNegada(51, d.id_deposito);
                                                                //Verificando si es pobible cambiar el estatus del depósito (En Autorización -> Por Depositar)
                                                                if (resultado.OperacionExitosa)
                                                                    //Actualizando estatus de depósito
                                                                    resultado = d.ActualizaEstatusAPorDepositar(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), u.id_usuario);
                                                                //Si ya hubo negaciones del depósito
                                                                else
                                                                    //Solicitando recarga de GV de pendientes
                                                                    actualizar = true;
                                                            }
                                                            break;
                                                        case "Rechazar":
                                                            //Rechazando autorización
                                                            resultado = a.ConfirmaAutorizacion(false, u.id_usuario);
                                                            //Si no existe error
                                                            if (resultado.OperacionExitosa)
                                                            {
                                                                //Realizando la deshabilitación de todas las autorizaciones solicitadas
                                                                resultado = da.DeshabilitaAutorizacionesSinConfirmar(51, d.id_deposito, u.id_usuario);

                                                                //Si no hay errores
                                                                if (resultado.OperacionExitosa)
                                                                    //Realizando la actualización del estatus del depósito
                                                                    resultado = d.ActualizaEstatusARegistrado(u.id_usuario);
                                                                //Si no hay errores
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    //Deshabilitamos Autorizaciones
                                                                    resultado = SAT_CL.Autorizacion.AutorizacionRealizada.DeshabilitaAutorizacionRealizada(51, d.id_deposito, u.id_usuario);
                                                                }
                                                            }
                                                            using (AnticipoProgramado anticipoProgramado = new AnticipoProgramado(d.id_deposito, d.id_compania_emisor))
                                                            {
                                                                if (anticipoProgramado.habilitar)
                                                                {
                                                                    resultado = anticipoProgramado.DeshabilitaAnticipoProgramado(u.id_usuario);
                                                                    if (resultado.OperacionExitosa)
                                                                    {
                                                                        using (Deposito objDeposito = new Deposito(d.id_deposito))
                                                                        {
                                                                            resultado = objDeposito.DeshabilitaDeposito(u.id_usuario);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                    }

                                                    //Si se actualizó
                                                    if (resultado.OperacionExitosa)
                                                        //Indicando actualización requerida al terminar las iteraciones
                                                        actualizar = true;

                                                    //Finalizando transacciones
                                                    if (resultado.OperacionExitosa)
                                                    {
                                                        scope.Complete();
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("Ya no se encuentra en autorización el depósito, actualice la ventana");

                                    //Añadiendo resultado al mensaje final
                                    mensaje += string.Format("Folio {0}: {1}<br/>", d.no_deposito, resultado.Mensaje);
                                }
                            }
                        }
                    }
                }
                //En base al resultado general
                if (actualizar)
                    //Actualizando pendientes
                    cargaDetalleAutorizaciones();
            }

            //Inicialziando indices del GridView
            Controles.InicializaIndices(gvAutorizaciones);

            //Mostrando resultado
            lblError.Text = mensaje;
        }

        /// <summary>
        /// Evento producido al pulsar el botón Actualizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            //Limpiamos Etiqueta
            lblError.Text = "";
            //Actualizamos Autorizaciones.
            cargaDetalleAutorizaciones();
        }

        #endregion

        #region Eventos Facturado

        /// <summary>
        /// Evento Producido el Guardar el Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Concepto
            result = ucFacturadoConcepto.GuardarFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga de Autorizaciones
                cargaDetalleAutorizaciones();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido el Eliminar el Concepto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucFacturadoConcepto.EliminaFacturaConcepto();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Carga de Autorizaciones
                cargaDetalleAutorizaciones();

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos Referencias Viaje

        /// <summary>
        /// Evento Producido al Guardar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                cargaDetalleAutorizaciones();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// /// Evento Producido al Eliminar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                cargaDetalleAutorizaciones();

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de manera general el contenido de la página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();
            //Cargando detalle de autorización
            cargaDetalleAutorizaciones();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando catálogo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoAutorizaciones, "", 26);
            //Cargando catálogo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFL, "", 26);
        }
        /// <summary>
        /// Realiza la carga de las autorizaciones pendientes
        /// </summary>
        private void cargaDetalleAutorizaciones()
        {
            //Definiendo origen de datos
            using (DataTable mit = AutorizacionRealizadaCargaPendientes.CargaAutorizacionesPendientes(Autorizacion.TipoAutorizacion.MontoDepositoServicio, ((Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando los detalles pendientes en el GridView
                    Controles.CargaGridView(gvAutorizaciones, mit, "Id-IdDeposito-IdServicio-IdFactura", lblOrdenarAutorizaciones.Text, true, 1);

                    //Almacenando en sesión
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAutorizaciones);

                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }

            //Inicializamos Indices
            Controles.InicializaIndices(gvAutorizaciones);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/Prueba.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/RecepcionDocumento.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }

        #region Métodos "Facturas Ligadas"

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
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable para Factura
            int idFactura = 0;

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
                                //Instanciando Receptor-Compania
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
                                                                            Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdServicio"]), doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                            doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                            doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                            Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI, "I", 1,
                                                                            (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
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
                                                    //Insertando Relación de Factura Proveedor con el Deposito
                                                    result = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 51, Convert.ToInt32(gvAutorizaciones.SelectedDataKey["IdDeposito"].ToString()), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

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
                monto_tc = Convert.ToDecimal(document.DocumentElement.Attributes["TipoCambio"].Value);
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
                if (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) > 0)

                    //Asignando Valores
                    tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
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
                //Asignando Valores
                tasa_imp_tras = Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value);
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }

        #endregion

        #endregion
    }
}