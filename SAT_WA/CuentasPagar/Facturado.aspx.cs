using SAT_CL;
using SAT_CL.CXP;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.CuentasPagar
{
    public partial class Facturado : System.Web.UI.Page
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

            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)
                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento producido al dar click sobre algún elemento de menú
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
                        //Actualizando Session
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        Session["id_registro"] = 0;
                        Session["DS"] =
                        Session["XML"] = null;
                        
                        //Inicializamos la pagina
                        inicializaPagina();

                        //Limpiando Mensaje de Error
                        lblError.Text =
                        lblErrorFactura.Text =
                        lblErrorConcepto.Text = "";
                        break;
                    }
                //Permite abrir registros de la Recepcion de factura
                case "Abrir":
                    {   
                        //Inicializando Apertura
                        inicializaAperturaRegistro(72, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                //Guarda el registro en la BD
                case "Guardar":
                    {   
                        //Invocando Método de Guardado
                        guardaFacturaProveedor();
                        break;
                    }
                //Envia al usuario a la pagina principal de la aplicación
                case "Salir":
                    {   
                        //Regresando a Página Anterior
                        PilaNavegacionPaginas.DireccionaPaginaAnterior();
                        break;
                    }
                //Permite al usuario editar el registro actual
                case "Editar":
                    {
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Inicializamos la pagina
                        inicializaPagina();
                        break;
                    }
                //Deshabilita un registro de la Recepcion de factura
                case "Eliminar":
                    {   
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();
                        
                        //Instanciando registro actual
                        using (FacturadoProveedor DesFP = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Si la Recepcion existe
                            if (DesFP.id_recepcion == 0)
                                //Deshabilitamos el registro
                                resultado = DesFP.DeshabilitaFacturadoProveedor(((Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepción
                                resultado = new RetornoOperacion("La Factura ya ha sido recibida, Imposible su Eliminación");
                            
                            //Si se deshabilitó correctamente
                            if (resultado.OperacionExitosa)
                            {   
                                //Estableciendo estatus a nuevo registro
                                Session["estatus"] = Pagina.Estatus.Nuevo;
                                //Inicializando Id de registro activo 
                                Session["id_registro"] = 0;
                                //Inicialziando la forma
                                inicializaPagina();
                            }
                            
                            //Mostrando resultado
                            lblError.Text = resultado.Mensaje;
                        }
                        break;
                    }
                case "Actualizar":
                    {
                        //Mostrando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbActualizar, "VentanaActualizar", "contenido_concepto_modal", "concepto_modal");
                        break;
                    }
                case "Aceptar":
                    {
                        //Declarando Objeto de Retorno
                        RetornoOperacion resultado = new RetornoOperacion();
                        
                        //Instanciando registro actual
                        using (FacturadoProveedor FP = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Si la Recepcion existe
                            if (FP.habilitar)
                            {
                                //Validando los Estatus donde se puede Aceptar
                                if ((FacturadoProveedor.EstatusFactura)FP.id_estatus_factura == FacturadoProveedor.EstatusFactura.EnRevision)

                                    //Deshabilitamos el registro
                                    resultado = FP.AceptaFacturaProveedor(((Usuario)Session["usuario"]).id_usuario);
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

                        //Si se Actualizó correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Estableciendo estatus a nuevo registro
                            Session["estatus"] = Pagina.Estatus.Lectura;
                            //Inicializando Id de registro activo 
                            Session["id_registro"] = resultado.IdRegistro;
                            //Inicialziando la forma
                            inicializaPagina();
                        }

                        //Mostrando resultado
                        ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Refacturacion":
                    {
                        //Estableciendo estatus a nuevo registro
                        Session["estatus"] = Pagina.Estatus.Copia;

                        //Inicialziando la forma
                        inicializaPagina();

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(boton, "Ahora puede agregar la Factura para su Refacturación.", ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.CentroDerecha);
                        break;
                    }
                case "Bitacora":
                    {   
                        //Inicializando Ventana de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "72", "Factura Proveedor");
                        break;
                    }
                case "Referencias":
                    {   
                        //Inicializando Ventana de Referencia
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "72", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    {
                        //Si hay un registro en sesión
                        if (Session["id_registro"].ToString() != "0")
                            inicializaArchivosRegistro(Session["id_registro"].ToString(), "72", "0");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Disparado al Presionar el Boton "Aceptar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Guardado
            guardaFacturaProveedor();
        }
        /// <summary>
        /// Evento Disparado al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    
                    //Asignando Estatus a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;
                    break;
                case Pagina.Estatus.Copia:
                    {   
                        //Validando que exista Sesión
                        if (Convert.ToInt32(Session["id_registro"]) > 0)
                            //Asignando Estatus a Lectura
                            Session["estatus"] = Pagina.Estatus.Lectura;
                        else
                            //Asignando Estatus a Lectura
                            Session["estatus"] = Pagina.Estatus.Nuevo;

                        //Mostrando Mensaje de Operación
                        ScriptServer.MuestraNotificacion(this, "Se Cancelo la Refacturación", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.CentroDerecha);
                        break;
                    }
            }
           
            //Invocando Método de Inicialización
            inicializaPagina();
        }

        #region Eventos Importación Factura

        /// <summary>
        /// Evento producido al Presionar el Boton "Importar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportar_Click(object sender, EventArgs e)
        {   
            //Validando que existe el Archivo
            if(Session["XML"] != null)
            {
                //Obteniendo Documento
                XDocument doc = (XDocument)Session["XML"];
                XNamespace ns = doc.Root.GetNamespaceOfPrefix("cfdi");

                try
                {
                    //Declarando variables Auxiliares
                    string rfc = "", nombre = "";

                    //Validando Version
                    switch (doc.Root.Attribute("Version") != null ? doc.Root.Attribute("Version").Value : doc.Root.Attribute("version").Value)
                    {
                        case "3.2":
                            {
                                //Asignando valores
                                rfc = doc.Root.Element(ns + "Emisor").Attribute("rfc").Value.ToUpper();
                                nombre = doc.Root.Element(ns + "Emisor").Attribute("nombre").Value.ToUpper();
                                break;
                            }
                        case "3.3":
                            {
                                //Asignando valores
                                rfc = doc.Root.Element(ns + "Emisor").Attribute("Rfc").Value.ToUpper();
                                nombre = doc.Root.Element(ns + "Emisor").Attribute("Nombre").Value.ToUpper();
                                break;
                            }
                    }
                    
                    //Instanciando Compania Emisora (Proveedor)
                    using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(rfc, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                    {
                        //Validando que Exista el Proveedor
                        if (emi.id_compania_emisor_receptor > 0)
                        {
                            validaEstatusPublicacionSAT(btnImportar);
                        }
                        else
                        {
                            //Cargando Tipos de Servicio
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "");

                            //Mostrando Proveedor por Ingresar
                            lblProveedorFactura.Text = nombre;

                            //Mostrando ventana Modal
                            TSDK.ASP.ScriptServer.AlternarVentana(upbtnImportar, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Obteniendo Mensaje
                    string excepcion = ex.Message;

                    //Mostrando Notificación
                    ScriptServer.MuestraNotificacion(btnImportar, excepcion, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

            }
        }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOperacion_Click(object sender, EventArgs e)
        {
            //Ocultando ventana Modal de inserción de proveedor
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptarOperacion, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");

            //Validando estatus de publicación
            validaEstatusPublicacionSAT(btnAceptarOperacion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarOperacion_Click(object sender, EventArgs e)
        {
            //Limpiando XML de Sesión
            Session["XML"] = null;
            
            //Ocultando ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, "Ventana Confirmación", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        
        #endregion      

        #region Eventos Factura Conceptos

        /// <summary>
        /// Evento Disparado al Presionar el Boton "Aceptar Concepto"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFacturaConcepto_Click(object sender, EventArgs e)
        {   
            //Guardando Concepto
            guardaConceptosFactura();
        }
        /// <summary>
        /// Evento Disparado al Presionar el Boton "Cancelar Concepto"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarFacturaConcepto_Click(object sender, EventArgs e)
        {   
            //Limpiando Controles
            limpiaControlesConcepto();
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
        }

        #region Eventos GridView Conceptos

        /// <summary>
        /// Evento Producido al Cambiar el tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvConceptosFacturaConcepto, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosFacturaConcepto_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {   
            //Cambiando Ordenamiento del GridView
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvConceptosFacturaConcepto, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptosFacturaConcepto_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {   
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvConceptosFacturaConcepto, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarExcel_Click(object sender, EventArgs e)
        {   
            //Exportando Excel
            Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "IdUnidad", "IdConceptoCobro");
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarFacturaConcepto_Click(object sender, EventArgs e)
        {   
            //Validando que existan Registros
            if (gvConceptosFacturaConcepto.DataKeys.Count > 0)
            {   
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);
                
                //Instanciando Concepto
                using (FacturadoProveedorConcepto fpc = new FacturadoProveedorConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                {   
                    //Validando que exista el Concepto
                    if (fpc.id_facturado_proveedor_concepto != 0)
                    {   
                        //Asignando valores
                        txtCantidadFacturaConcepto.Text = fpc.cantidad.ToString();
                        txtUnidad.Text = fpc.unidad;
                        txtIdentificadorFacturaConcepto.Text = fpc.identificador;
                        txtConceptoCobro.Text = fpc.concepto_cobro;
                        txtValorUniFacturaConcepto.Text = fpc.valor_unitario.ToString();
                        txtImporteFacturaConcepto.Text = string.Format("{0:#,###,###,###.00}", fpc.importe);
                        txtTasaImpRetFacturaConcepto.Text = string.Format("{0:#,###,###,###.00}", fpc.tasa_impuesto_retenido);
                        txtTasaImpTraFacturaConcepto.Text = string.Format("{0:#,###,###,###.00}", fpc.tasa_impuesto_trasladado);
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al presionar el Link Button "Eliminar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFacturaConcepto_Click(object sender, EventArgs e)
        {   
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvConceptosFacturaConcepto, sender, "lnk", false);
            
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {   
                //Instanciando Concepto
                using (FacturadoProveedorConcepto fpc = new FacturadoProveedorConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                {   
                    //Validando que exista
                    if(fpc.id_facturado_proveedor_concepto != 0)
                        
                        //Deshabilitando Concepto
                        result = fpc.DeshabilitaFacturaProveedorConcepto(((Usuario)Session["usuario"]).id_usuario);
                }
                
                //Validando que la Operacion fuera Exitosa
                if (result.OperacionExitosa)
                {   
                    //Obteniendo Valores Totales
                    using (DataTable dtTotales = FacturadoProveedorConcepto.ObtieneValoresTotalesFacturaProveedor(Convert.ToInt32(Session["id_registro"])))
                    {   
                        //Validando Origen de Datos
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                        {   
                            //Instanciando Factura
                            using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                            {   
                                //Validando que exista la Factura
                                if (fp.id_factura != 0)
                                {   
                                    //Recorriendo Registros
                                    foreach (DataRow dr in dtTotales.Rows)
                                    {   
                                        //Actualizando Totales
                                        result = fp.ActualizaTotalesFacturadoProveedor(Convert.ToDecimal(dr["TotalFactura"]), Convert.ToDecimal(dr["SubTotalFactura"]), Convert.ToDecimal(dr["TrasladadoFactura"]),
                                                            Convert.ToDecimal(dr["RetenidoFactura"]), Convert.ToDecimal(dr["TotalFacPesos"]), Convert.ToDecimal(dr["SubTotalFacPesos"]),
                                                            Convert.ToDecimal(dr["TrasladadoFacPesos"]), Convert.ToDecimal(dr["RetenidoFacPesos"]), Convert.ToDecimal(dr["TotalFactura"]),
                                                            ((Usuario)Session["usuario"]).id_usuario);
                                    }
                                    
                                    //Validando que la Operacion haya sido exitosa
                                    if (result.OperacionExitosa)
                                        
                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }
                    }
                }
                
                //Validando que la Operacion fuera Exitosa
                if (result.OperacionExitosa)
                {   
                    //Cambiando Estatus de Página
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    
                    //Inicializando Página
                    inicializaPagina();
                    
                    //Cargando Catalogos
                    cargaConceptosFactura();
                    
                    //Limpiando Controles
                    limpiaControlesConcepto();
                    
                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
                }
                
                //Mostrando Mensaje
                lblErrorConcepto.Text = result.Mensaje;
            }
        }

        #endregion

        #endregion

        #region Eventos Modal Factura Concepto

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarFactura_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Actualización
            actualizaEstatusFactura("Acepta");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarFactura_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Actualización
            actualizaEstatusFactura("Cancela");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRechazarFactura_Click(object sender, EventArgs e)
        {   
            //Invocando Método de Actualización
            actualizaEstatusFactura("Rechaza");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarConcepto_Click(object sender, EventArgs e)
        {   
            //Obteniendo Filas Selecionadas
            GridViewRow[] filas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvConceptos, "chkVarios");
            
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando que existen Registros Seleccionados
            if (filas.Length > 0)
            {   
                //Inicializando Transaccion
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {   
                    //Declarando variable Auxiliares
                    int contador = 0;
                    
                    //Iniciando Ciclo
                    while (contador < filas.Length)
                    {   
                        //Obteniendo Fila
                        GridViewRow fila = filas[contador];
                        
                        //Obteniendo indice de la Fila Actual
                        gvConceptos.SelectedIndex = fila.RowIndex;
                        
                        //Instanciando Concepto
                        using (FacturadoProveedorConcepto fpc = new FacturadoProveedorConcepto(Convert.ToInt32(gvConceptos.SelectedDataKey["Id"])))
                        {   
                            //Validando que exista el Concepto
                            if (fpc.id_facturado_proveedor_concepto != 0)
                                
                                //Actualizando Clasificación
                                result = fpc.ActualizaClasificacionConcepto(Convert.ToInt32(ddlClasifCont.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepcion
                                result = new RetornoOperacion("No existe el Concepto.");
                            
                            //Incrementando Contador
                            contador = result.OperacionExitosa ? contador + 1 : filas.Length;
                        }
                    }
                    
                    //Validando que la Operación haya sido exitosa
                    if (result.OperacionExitosa)
                        
                        //Completando Transacción
                        trans.Complete();
                }
                
                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                    
                    //Cargando Reporte
                    cargaConceptosClasificacion();
            }
            else
                //Instanciando Excepcion
                result = new RetornoOperacion("No existen filas seleccionadas");
            
            //Mostrando Mensaje Obtenido
            lblErrorConcepto.Text = result.Mensaje;
        }

        #region Eventos GridView Modal Conceptos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Ordenamiento del GridView
            lblOrdenadoGrid.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvConceptos, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression, true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvConceptos, ((DataSet)Session["DS"]).Tables["Table1"], e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGrid_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando el Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvConceptos, ((DataSet)Session["DS"]).Tables["Table1"], Convert.ToInt32(ddlTamanoGrid.SelectedValue), true, 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Exportando Excel
            Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table1"], "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Evalua el ID del CheckBox en el que se produce el cambio
            switch (((CheckBox)sender).ID)
            {   //Caso para el CheckBox "Todos"
                case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                    CheckBox chk = (CheckBox)gvConceptos.HeaderRow.FindControl("chkTodos");
                    //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvConceptos, "chkVarios", chk.Checked);
                    break;
            }//fin switch
        }

        #endregion

        #endregion

        #region Eventos Validación SAT

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
                    guardaXML();

                    //Ocultando ventana modal
                    ScriptServer.AlternarVentana(btnAceptarValidacion, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                    break;
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
            //habilitando Menu
            habilitaMenu();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   
            //Cargando Control
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoFactura, "", 59);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGrid, "", 18);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 58);
            
            //Factura Ventana Modal
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServ, 28, "-- Seleccione el Tipo --", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSegNeg, 26, "-- Seleccione el Segmento --", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            
            //Concepto Ventana Modal
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlClasifCont, 27, "-- Seleccione la Clasificación --", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            
            //Carga catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoServicio, 28, "");
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {   
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Encabezado de la Factura
                        txtCompania.Enabled = false;
                        txtProveedor.Enabled =
                        ddlTipoFactura.Enabled =
                        txtSerie.Enabled =
                        txtFolio.Enabled =
                        txtUUID.Enabled =
                        txtFechaFactura.Enabled =
                        txtMoneda.Enabled =
                            
                        //Datos de la Factura
                        txtMontoTC.Enabled =
                        txtFechaTC.Enabled = true;
                        txtTotal.Enabled =
                        txtSubTotal.Enabled = false;
                        txtDescuento.Enabled = true;
                        txtTrasladado.Enabled =
                        txtRetenido.Enabled = false;
                        txtCondPago.Enabled = true;
                        txtSaldo.Enabled = false;
                        txtDiasCredito.Enabled = false;
                        
                        //Habilitando Controles
                        btnAceptar.Enabled = 
                        btnCancelar.Enabled = true;
                        
                        //Deshabilitando Controles
                        habilitaControlesConcepto(false);

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   
                        //Encabezado de la Factura
                        txtCompania.Enabled = 
                        txtProveedor.Enabled =
                        ddlTipoFactura.Enabled =
                        txtSerie.Enabled =
                        txtFolio.Enabled =
                        txtUUID.Enabled =
                        txtFechaFactura.Enabled =
                        txtMoneda.Enabled =
                        
                        //Datos de la Factura
                        txtMontoTC.Enabled =
                        txtFechaTC.Enabled =
                        txtTotal.Enabled =
                        txtSubTotal.Enabled =
                        txtDescuento.Enabled =
                        txtTrasladado.Enabled =
                        txtRetenido.Enabled =
                        txtCondPago.Enabled =
                        txtSaldo.Enabled =
                        txtDiasCredito.Enabled = false;
                        
                        //Habilitando Controles
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = false;
                        
                        //Deshabilitando y Limpiando Controles
                        habilitaControlesConcepto(false);
                        limpiaControlesConcepto();

                        break;
                    }
                case Pagina.Estatus.Copia:
                    {
                        //Encabezado de la Factura
                        txtCompania.Enabled =
                        txtProveedor.Enabled =
                        ddlTipoFactura.Enabled =
                        txtSerie.Enabled =
                        txtFolio.Enabled =
                        txtUUID.Enabled =
                        txtFechaFactura.Enabled =
                        txtMoneda.Enabled =

                        //Datos de la Factura
                        txtMontoTC.Enabled =
                        txtFechaTC.Enabled =
                        txtTotal.Enabled =
                        txtSubTotal.Enabled =
                        txtDescuento.Enabled =
                        txtTrasladado.Enabled =
                        txtRetenido.Enabled =
                        txtCondPago.Enabled =
                        txtSaldo.Enabled =
                        txtDiasCredito.Enabled = false;

                        //Habilitando Controles
                        btnAceptar.Enabled = false;
                        btnCancelar.Enabled = true;

                        //Deshabilitando y Limpiando Controles
                        habilitaControlesConcepto(false);
                        limpiaControlesConcepto();

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        using(FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                        {   
                            if(fp.id_factura != 0)
                            {   
                                //Validando que el Comprobante sea CFDI
                                if(fp.id_tipo_factura == (byte)FacturadoProveedor.TipoComprobante.CFDI)
                                {   
                                    //Encabezado de la Factura
                                    txtCompania.Enabled =
                                    txtProveedor.Enabled =
                                    ddlTipoFactura.Enabled =
                                    txtSerie.Enabled =
                                    txtFolio.Enabled =
                                    txtUUID.Enabled =
                                    txtFechaFactura.Enabled =
                                    txtMoneda.Enabled =
                                    
                                    //Datos de la Factura
                                    txtMontoTC.Enabled =
                                    txtFechaTC.Enabled =
                                    txtTotal.Enabled =
                                    txtSubTotal.Enabled =
                                    txtDescuento.Enabled =
                                    txtTrasladado.Enabled =
                                    txtRetenido.Enabled =
                                    txtCondPago.Enabled =
                                    txtSaldo.Enabled = 
                                    txtDiasCredito.Enabled = false;
                                    
                                    //Habilitando Controles
                                    btnAceptar.Enabled =
                                    btnCancelar.Enabled = false;
                                    
                                    //Deshabilitando y Limpiando Controles
                                    habilitaControlesConcepto(true);
                                    limpiaControlesConcepto();
                                }
                                else
                                {   
                                    //Encabezado de la Factura
                                    txtCompania.Enabled = false;
                                    txtProveedor.Enabled = true;
                                    ddlTipoFactura.Enabled = false;
                                    txtSerie.Enabled =
                                    txtFolio.Enabled =
                                    txtUUID.Enabled =
                                    txtFechaFactura.Enabled =
                                    txtMoneda.Enabled = true;
                                    
                                    //Datos de la Factura
                                    txtMontoTC.Enabled =
                                    txtFechaTC.Enabled =
                                    txtTotal.Enabled =
                                    txtSubTotal.Enabled = false;
                                    txtDescuento.Enabled = true;
                                    txtTrasladado.Enabled =
                                    txtRetenido.Enabled =
                                    txtSaldo.Enabled = false;
                                    txtCondPago.Enabled = true;
                                    txtDiasCredito.Enabled = false;
                                    
                                    //Habilitando Controles
                                    btnAceptar.Enabled =
                                    btnCancelar.Enabled = true;
                                    
                                    //Habilitando y Limpiando Controles
                                    habilitaControlesConcepto(true);
                                    limpiaControlesConcepto();
                                }
                                
                                //Habilitando Controles
                                btnAceptar.Enabled =
                                btnCancelar.Enabled = true;
                            }

                        }

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores
        /// </summary>
        private void inicializaValores()
        {   
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Encabezado de la Factura
                        lblId.Text = "Por Asignar";
                        
                        //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {   
                            //Asignando Compania
                            txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                            txtDiasCredito.Text = cer.dias_credito.ToString();
                        }
                        txtProveedor.Text =
                        txtSerie.Text =
                        txtFolio.Text =
                        txtUUID.Text = 
                        txtMoneda.Text = "";
                        txtFechaFactura.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        
                        //Datos de la Factura
                        txtMontoTC.Text = "1";
                        txtFechaTC.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        txtTotal.Text =
                        txtSubTotal.Text =
                        txtDescuento.Text =
                        txtTrasladado.Text =
                        txtRetenido.Text =
                        txtSaldo.Text = "0.00";
                        txtCondPago.Text = "";
                        
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvConceptosFacturaConcepto);
                        TSDK.ASP.Controles.InicializaGridview(gvConceptos);
                        limpiaControlesConcepto();

                        break;
                    }
                case Pagina.Estatus.Copia:

                case Pagina.Estatus.Lectura:
                    
                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Factura
                        using(SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista la Factura
                            if (fp.id_factura != 0)
                            {   
                                /** Encabezado de la Factura **/
                                lblId.Text = fp.id_factura.ToString();
                                
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(fp.id_compania_receptor))
                                {   
                                    //Asignando Compania
                                    txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                                    txtDiasCredito.Text = cer.dias_credito.ToString();
                                }

                                //Instanciando Proveedor
                                using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(fp.id_compania_proveedor))
                                    
                                    //Asignando Proveedor
                                    txtProveedor.Text = pro.nombre + " ID:" + pro.id_compania_emisor_receptor.ToString();

                                //Asignando Valores
                                ddlTipoServ.SelectedValue = fp.id_tipo_servicio.ToString();
                                ddlSegNeg.SelectedValue = fp.id_segmento_negocio.ToString();
                                ddlTipoFactura.SelectedValue = fp.id_tipo_factura.ToString();
                                ddlEstatus.SelectedValue = fp.id_estatus_factura.ToString();
                                txtSerie.Text = fp.serie;
                                txtFolio.Text = fp.folio.ToString();
                                txtUUID.Text = fp.uuid;
                                txtMoneda.Text = fp.moneda;
                                txtFechaFactura.Text = fp.fecha_factura.ToString("dd/MM/yyyy HH:mm");
                                
                                /** Datos de la Factura **/
                                txtMontoTC.Text = string.Format("{0:#,###,###,###.00}", fp.monto_tipo_cambio);
                                txtFechaTC.Text = fp.fecha_tipo_cambio.ToString("dd/MM/yyyy HH:mm");
                                txtTotal.Text = string.Format("{0:#,###,###,###.00}", fp.total_factura);
                                txtSubTotal.Text = string.Format("{0:#,###,###,###.00}", fp.subtotal_factura);
                                txtDescuento.Text = string.Format("{0:#,###,###,###.00}", fp.descuento_factura);
                                txtTrasladado.Text = string.Format("{0:#,###,###,###.00}", fp.trasladado_factura);
                                txtRetenido.Text = string.Format("{0:#,###,###,###.00}", fp.retenido_factura);
                                txtSaldo.Text = string.Format("{0:#,###,###,###.00}", fp.saldo);
                                txtCondPago.Text = fp.condicion_pago;
                                
                                //Invocando Método de Carga
                                cargaConceptosFactura();
                                cargaConceptosClasificacion();
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = 
                        lkbGuardar.Enabled = 
                        lkbSalir.Enabled = true;
                        lkbEditar.Enabled = 
                        lkbEliminar.Enabled = 
                        lkbActualizar.Enabled = 
                        lkbRefacturacion.Enabled =
                        lkbBitacora.Enabled = 
                        lkbReferencias.Enabled = 
                        lkbActualizar.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbSalir.Enabled = 
                        lkbEditar.Enabled = 
                        lkbEliminar.Enabled = 
                        lkbActualizar.Enabled = 
                        lkbBitacora.Enabled = 
                        lkbReferencias.Enabled = 
                        lkbActualizar.Enabled = true;

                        //Validando Estatus
                        switch ((FacturadoProveedor.EstatusFactura)Convert.ToInt32(ddlEstatus.SelectedValue))
                        {
                            case FacturadoProveedor.EstatusFactura.Refacturacion:
                            case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                            case FacturadoProveedor.EstatusFactura.Liquidada:
                            case FacturadoProveedor.EstatusFactura.Cancelada:
                            case FacturadoProveedor.EstatusFactura.Rechazada:
                                {
                                    //Deshabilitando Control del Menú
                                    lkbRefacturacion.Enabled = false;
                                    break;
                                }
                            case FacturadoProveedor.EstatusFactura.EnRevision:
                            case FacturadoProveedor.EstatusFactura.Aceptada:
                                {
                                    //Habilitando Control del Menú
                                    lkbRefacturacion.Enabled = true;
                                    break;
                                }
                        }

                        break;
                    }
                case Pagina.Estatus.Copia:

                case Pagina.Estatus.Edicion:
                    {
                        lkbNuevo.Enabled = 
                        lkbAbrir.Enabled = 
                        lkbGuardar.Enabled = 
                        lkbSalir.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbActualizar.Enabled = false;
                        lkbBitacora.Enabled = 
                        lkbReferencias.Enabled = 
                        lkbActualizar.Enabled = true;

                        //Validando Estatus
                        switch ((FacturadoProveedor.EstatusFactura)Convert.ToInt32(ddlEstatus.SelectedValue))
                        {
                            case FacturadoProveedor.EstatusFactura.Refacturacion:
                            case FacturadoProveedor.EstatusFactura.AplicadaParcial:
                            case FacturadoProveedor.EstatusFactura.Liquidada:
                            case FacturadoProveedor.EstatusFactura.Cancelada:
                            case FacturadoProveedor.EstatusFactura.Rechazada:
                                {
                                    //Deshabilitando Control del Menú
                                    lkbRefacturacion.Enabled = false;
                                    break;
                                }
                            case FacturadoProveedor.EstatusFactura.EnRevision:
                            case FacturadoProveedor.EstatusFactura.Aceptada:
                                {
                                    //Habilitando Control del Menú
                                    lkbRefacturacion.Enabled = true;
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Guardar las Facturas
        /// </summary>
        private void guardaFacturaProveedor()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variables Auxiliares
            DateTime fecha_factura, fecha_tc;
            decimal monto_tc = txtMontoTC.Text == "" || txtMontoTC.Text == "0"? 1 : Convert.ToDecimal(txtMontoTC.Text);
            
            //Obteniendo Fechas
            DateTime.TryParse(txtFechaFactura.Text, out fecha_factura);
            DateTime.TryParse(txtFechaTC.Text, out fecha_tc);
            
            //Validando Estatus de Página
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Instanciando Proveedor
                        using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1))))

                            //Insertando Factura
                            result = FacturadoProveedor.InsertaFacturadoProveedor(pro.id_compania_emisor_receptor,
                                                                        Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                        txtSerie.Text, txtFolio.Text, txtUUID.Text, fecha_factura, Convert.ToByte(ddlTipoFactura.SelectedValue), 
                                                                        "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision,
                                                                        Convert.ToDecimal(txtDescuento.Text == "" ? "0" : txtDescuento.Text),
                                                                        (byte)FacturadoProveedor.EstatusRecepion.SinProceso, txtMoneda.Text,
                                                                        Convert.ToDecimal(txtMontoTC.Text == "" ? "1" : txtMontoTC.Text), fecha_tc,
                                                                        Convert.ToDecimal(txtDescuento.Text == "" ? "0" : txtDescuento.Text) * monto_tc, txtCondPago.Text, pro.dias_credito,
                                                                        ((Usuario)Session["usuario"]).id_usuario);
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Factura del Proveedor
                        using(FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista la factura
                            if(fp.id_factura != 0)
                            {   
                                //Instanciando Proveedor
                                using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1))))
                                    
                                    //Editando la Factura
                                    result = fp.EditaFacturadoProveedor(pro.id_compania_emisor_receptor,
                                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)),
                                                                txtSerie.Text, txtFolio.Text,
                                                                txtUUID.Text, fecha_factura, Convert.ToByte(ddlTipoFactura.SelectedValue), fp.tipo_comprobante, fp.id_naturaleza_cfdi, Convert.ToByte(ddlEstatus.SelectedValue),
                                                                Convert.ToDecimal(txtDescuento.Text == "" ? "0" : txtDescuento.Text),
                                                                fp.id_estatus_recepcion != (byte)FacturadoProveedor.EstatusRecepion.SinProceso ? fp.id_estatus_recepcion : (byte)FacturadoProveedor.EstatusRecepion.SinProceso, 
                                                                Convert.ToByte(ddlSegNeg.SelectedValue), Convert.ToByte(ddlTipoServ.SelectedValue), txtMoneda.Text, Convert.ToDecimal(txtMontoTC.Text == "" ? "1" : txtMontoTC.Text), fecha_tc,
                                                                Convert.ToDecimal(txtDescuento.Text == "" ? "0" : txtDescuento.Text) * monto_tc, txtCondPago.Text, pro.dias_credito,
                                                                ((Usuario)Session["usuario"]).id_usuario);
                            }
                        }
                        break;
                    }
            }
            //Validando que la Operación haya sido Exitosa
            if(result.OperacionExitosa)
            {   
                //Asignando Sessiones
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;
                
                //Invocando Método de Inicialización de Página
                inicializaPagina();
            }
            
            //Mostrando Mensaje
            lblError.Text = result.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   
            //Asignando Session
            Session["id_tabla"] = idTabla;
            
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/Facturado.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString());
            
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Recepción de Facturas", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/Facturado.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Recepción", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/Facturado.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/Facturado.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }

        #region Métodos Importación Factura

        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
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
        /// <param name="archivoBase64"></param>
        /// <param name="nombreArchivo"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        [WebMethod]
        public static string LecturaArchivo(string archivoBase64, string nombreArchivo, string mimeType)
        {
            //Definiendo objeto de retorno
            string resultado = "";

            //Si hay elementos
            if (!string.IsNullOrEmpty(archivoBase64))
            {
                //Validando tipo de archivo (mime type), debe ser .xml
                if (mimeType == "text/xml")
                {
                    try
                    {
                        //Convietiendo archivo a bytes
                        byte[] responseData = Convert.FromBase64String(TSDK.Base.Cadena.RegresaCadenaSeparada(archivoBase64, "base64,", 1));

                        //Declarando Documento XML
                        XmlDocument doc = new XmlDocument();
                        XDocument responseXml = new XDocument();

                        //Obteniendo XML en cadena
                        using (MemoryStream ms = new MemoryStream(responseData))
                            //Cargando Documento XML
                            doc.Load(ms);

                        //Convirtiendo XML
                        responseXml = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(doc);
                        //Almacenando en variables de sesión
                        HttpContext.Current.Session["XML"] = responseXml;
                        HttpContext.Current.Session["XMLFileName"] = nombreArchivo;
                        //Instanciando Resultado Positivo
                        resultado = string.Format("Archivo '{0}' cargado correctamente!!!", nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        //Limpiando en variables de sesión
                        HttpContext.Current.Session["XML"] = null;
                        HttpContext.Current.Session["XMLFileName"] = "";
                        //Instanciando Excepción
                        resultado = string.Format("Error al Cargar el Archivo: '{0}'", ex.Message);
                    }
                }
                else
                    //Si el tipo de archivo no es válido
                    resultado = "El archivo seleccionado no tiene un formato válido. Formatos permitidos '.xls' / '.xlsx'.";
            }
            else
                //Instanciando Excepción
                resultado = "No se encontró contenido en el archivo.";

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la validación del estatus de publicación del CFDI en servidores del SAT
        /// </summary>
        private void validaEstatusPublicacionSAT(System.Web.UI.Control control)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando auxiliares
            string rfc_emisor, rfc_receptor, UUID;
            decimal monto; DateTime fecha_expedicion;

            //Obteniendo Documento XML
            XDocument xDocument = (XDocument)Session["XML"];
            XNamespace ns = xDocument.Root.GetNamespaceOfPrefix("cfdi");
            XNamespace tfd = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Namespace TimbreFiscalDigital");

            //Convirtiendo Documento
            XmlDocument xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                //Cargando XML Document
                xmlDocument.Load(xmlReader);
            }

            //Validando versión
            switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
            {
                case "3.2":
                case "3.3":
                    {
                        //Realizando validación de estatus en SAT
                        result = Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

                        //Colocando resultado sobre controles
                        imgValidacionSAT.Src = result.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                        headerValidacionSAT.InnerText = result.Mensaje;
                        lblRFCEmisor.Text = rfc_emisor;
                        lblRFCReceptor.Text = rfc_receptor;
                        lblUUID.Text = UUID;
                        lblTotalFactura.Text = monto.ToString("C");
                        lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                        break;
                    }
            }

            //Mostrando resultado de consulta en SAT (ventana modal)
            ScriptServer.AlternarVentana(control, "DatosValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
        }
        /// <summary>
        /// Método encargado de Importar el Archvio XML
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion guardaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxiliar
            int idFacturaProveedor = 0;

            //Validando Archivo XML
            if (Session["XML"] != null)
            {
                //Obteniendo Archivo XML
                XDocument documento = (XDocument)Session["XML"];

                //Validando estatus de la Forma
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Copia:
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Validando versión
                                switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                                {
                                    case "3.2":
                                        {
                                            //Insertando CFDI 3.2
                                            result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                     Convert.ToInt32(ddlTipoServicio.SelectedValue), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                    case "3.3":
                                        {
                                            //Insertando CFDI 3.3
                                            result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                     Convert.ToInt32(ddlTipoServicio.SelectedValue), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            break;
                                        }
                                }

                                //Validando Operación Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Guardando Factura Nueva
                                    idFacturaProveedor = result.IdRegistro;

                                    //Refacturando
                                    result = SAT_CL.CXP.FacturadoProveedor.RefacturacionCXP(Convert.ToInt32(Session["id_registro"]), idFacturaProveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Resultado Positivo
                                        result = new RetornoOperacion(idFacturaProveedor);

                                        //Completando Transacción
                                        scope.Complete();
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {
                            //Validando versión
                            switch (documento.Root.Attribute("version") != null ? documento.Root.Attribute("version").Value : documento.Root.Attribute("Version").Value)
                            {
                                case "3.2":
                                    {
                                        //Insertando CFDI 3.2
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion32(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 Convert.ToInt32(ddlTipoServicio.SelectedValue), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    }
                                case "3.3":
                                    {
                                        //Insertando CFDI 3.3
                                        result = SAT_CL.CXP.FacturadoProveedor.ImportaComprobanteVersion33(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                 Convert.ToInt32(ddlTipoServicio.SelectedValue), Session["XMLFileName"].ToString(), documento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                        break;
                                    }
                            }

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            
                                //Guardando Factura Nueva
                                idFacturaProveedor = result.IdRegistro;
                            break;
                        }
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("Cargue su archivo XML para importar");

            //Validando que exista
            if (result.OperacionExitosa)
            {
                //Reasignando Id de registro
                result = new RetornoOperacion(idFacturaProveedor);
                //Establecemos el id del registro
                Session["id_registro"] = result.IdRegistro;
                //Establecemos el estatus de la forma
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Eliminando Contenido en Sessión del XML
                Session["XML"] = 
                Session["XMLFileName"] = null;
                //Inicializamos la forma
                inicializaPagina();
            }

            //Actualizamos la etiqueta de errores
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Devolviendo resultado Obtenido
            return result;
        }

        #endregion

        #region Métodos Facturado Concepto

        /// <summary>
        /// Método encargado de Habilitar los Controles del Concepto
        /// </summary>
        /// <param name="enable"></param>
        private void habilitaControlesConcepto(bool enable)
        {   
            //Habilitando Controles
            txtCantidadFacturaConcepto.Enabled =
            txtUnidad.Enabled =
            txtIdentificadorFacturaConcepto.Enabled =
            txtConceptoCobro.Enabled =
            txtValorUniFacturaConcepto.Enabled =
            txtTasaImpRetFacturaConcepto.Enabled =
            txtTasaImpTraFacturaConcepto.Enabled =
            btnAceptarFacturaConcepto.Enabled =
            btnCancelarFacturaConcepto.Enabled =
            gvConceptosFacturaConcepto.Enabled =
            ddlTamano.Enabled =
            lnkExportarExcel.Enabled = enable;
            
            //Ventana Modal
            bool modal_enable = (Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Nuevo ? false : true;
            ddlTamanoGrid.Enabled =
            lnkExportar.Enabled =
            gvConceptos.Enabled =
            ddlTipoServ.Enabled =
            ddlSegNeg.Enabled =
            btnAceptarFactura.Enabled =
            btnCancelarFactura.Enabled =
            btnRechazarFactura.Enabled =
            ddlClasifCont.Enabled =
            btnAceptarConcepto.Visible = modal_enable;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Conceptos de la Factura
        /// </summary>
        private void cargaConceptosFactura()
        {   
            //Obteniendo Conceptos
            using (DataTable dtConceptos = FacturadoProveedorConcepto.ObtieneConceptosFactura(Convert.ToInt32(Session["id_registro"])))
            {   
                //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptos))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvConceptosFacturaConcepto, dtConceptos, "Id", "", true, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtConceptos, "Table");
                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvConceptosFacturaConcepto);
                    //Eliminando Tabla de ViewState
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    
                }
            }

            //Mostrando Totales
            escribeTotales();
        }
        /// <summary>
        /// Método Privado encargado de Escribir los Valores Totales de los Conceptos
        /// </summary>
        private void escribeTotales()
        {   
            //Validando Origen de datos
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {   
                //Mostrando Totales
                gvConceptosFacturaConcepto.FooterRow.Cells[6].Text = string.Format("{0:#,###,###,###.00}", ((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Importe)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[7].Text = string.Format("{0:#,###,###,###.00}", ((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ImportePesos)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[9].Text = string.Format("{0:#,###,###,###.00}", ((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ImporteRetenido)", ""));
                gvConceptosFacturaConcepto.FooterRow.Cells[11].Text = string.Format("{0:#,###,###,###.00}", ((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(ImporteTrasladado)", ""));
            }
            else
            {   
                //Mostrando Totales en 0
                gvConceptosFacturaConcepto.FooterRow.Cells[6].Text = string.Format("{0:#,###,###,###.00}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[7].Text = string.Format("{0:#,###,###,###.00}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[9].Text = string.Format("{0:#,###,###,###.00}", 0);
                gvConceptosFacturaConcepto.FooterRow.Cells[11].Text = string.Format("{0:#,###,###,###.00}", 0);
            }
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles
        /// </summary>
        private void limpiaControlesConcepto()
        {   
            //Limpiando Controles
            txtCantidadFacturaConcepto.Text =
            txtValorUniFacturaConcepto.Text =
            txtImporteFacturaConcepto.Text =
            txtTasaImpRetFacturaConcepto.Text =
            txtTasaImpTraFacturaConcepto.Text = "0.00";
            txtUnidad.Text =
            txtIdentificadorFacturaConcepto.Text =
            txtConceptoCobro.Text = "";
        }
        /// <summary>
        /// Método Privado encargado de Guardar los Conceptos
        /// </summary>
        private void guardaConceptosFactura()
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Inicializando Transacción
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {   
                //Obteniendo el Tipo de Cambio
                decimal monto_tc = txtMontoTC.Text == "" || txtMontoTC.Text == "0" ? 1 : Convert.ToDecimal(txtMontoTC.Text);
                
                //Validando que este seleccionado algún registro
                if (gvConceptosFacturaConcepto.SelectedIndex != -1)
                {   
                    //Instanciando Concepto
                    using (FacturadoProveedorConcepto fpc = new FacturadoProveedorConcepto(Convert.ToInt32(gvConceptosFacturaConcepto.SelectedDataKey["Id"])))
                    {   
                        //Validando si existe el Concepto 
                        if (fpc.id_facturado_proveedor_concepto != 0)
                        {   
                            //Editando Concepto
                            result = fpc.EditaFacturaProveedorConcepto(Convert.ToInt32(Session["id_registro"]), Convert.ToDecimal(txtCantidadFacturaConcepto.Text),
                                            txtUnidad.Text, txtIdentificadorFacturaConcepto.Text, txtConceptoCobro.Text, 0, Convert.ToDecimal(txtValorUniFacturaConcepto.Text),
                                            Convert.ToDecimal(txtImporteFacturaConcepto.Text), Convert.ToDecimal(txtImporteFacturaConcepto.Text) * monto_tc,
                                            Convert.ToDecimal(txtTasaImpRetFacturaConcepto.Text), Convert.ToDecimal(txtTasaImpTraFacturaConcepto.Text),
                                            ((Usuario)Session["usuario"]).id_usuario);

                        }
                    }
                }
                else
                {   
                    //Insertando Concepto
                    result = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(Convert.ToInt32(Session["id_registro"]), Convert.ToDecimal(txtCantidadFacturaConcepto.Text),
                                            txtUnidad.Text, txtIdentificadorFacturaConcepto.Text, txtConceptoCobro.Text, 0, Convert.ToDecimal(txtValorUniFacturaConcepto.Text),
                                            Convert.ToDecimal(txtImporteFacturaConcepto.Text), Convert.ToDecimal(txtImporteFacturaConcepto.Text) * monto_tc,
                                            Convert.ToDecimal(txtTasaImpRetFacturaConcepto.Text), Convert.ToDecimal(txtTasaImpTraFacturaConcepto.Text),
                                            ((Usuario)Session["usuario"]).id_usuario);
                }
                
                //Validando que la Operacion fuera Exitosa
                if(result.OperacionExitosa)
                {   
                    //Obteniendo Valores Totales
                    using(DataTable dtTotales = FacturadoProveedorConcepto.ObtieneValoresTotalesFacturaProveedor(Convert.ToInt32(Session["id_registro"])))
                    {   
                        //Validando Origen de Datos
                        if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                        {   
                            //Instanciando Factura
                            using(FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
                            {   
                                //Validando que exista la Factura
                                if(fp.id_factura != 0)
                                {   
                                    //Recorriendo Registros
                                    foreach(DataRow dr in dtTotales.Rows)
                                    {   
                                        //Actualizando Totales
                                        result = fp.ActualizaTotalesFacturadoProveedor(Convert.ToDecimal(dr["TotalFactura"]), Convert.ToDecimal(dr["SubTotalFactura"]), Convert.ToDecimal(dr["TrasladadoFactura"]),
                                                            Convert.ToDecimal(dr["RetenidoFactura"]), Convert.ToDecimal(dr["TotalFacPesos"]), Convert.ToDecimal(dr["SubTotalFacPesos"]),
                                                            Convert.ToDecimal(dr["TrasladadoFacPesos"]), Convert.ToDecimal(dr["RetenidoFacPesos"]), Convert.ToDecimal(dr["TotalFactura"]), 
                                                            ((Usuario)Session["usuario"]).id_usuario);
                                    }
                                    
                                    //Validando que la Operacion haya sido exitosa
                                    if (result.OperacionExitosa)
                                        
                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }
                    }
                }
            }
            
            //Validando que la Operacion fuera Exitosa
            if (result.OperacionExitosa)
            {   
                //Cambiando Estatus de Página
                Session["estatus"] = Pagina.Estatus.Edicion;
                
                //Inicializando Página
                inicializaPagina();
                
                //Cargando Catalogos
                cargaConceptosFactura();
                
                //Cargando Conceptos con su Clasificacion
                cargaConceptosClasificacion();
                
                //Limpiando Controles
                limpiaControlesConcepto();
                
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvConceptosFacturaConcepto);
            }
            
            //Mostrando Mensaje
            lblErrorConcepto.Text = result.Mensaje;
        }


        #endregion

        #region Métodos Factura Modal

        /// <summary>
        /// Método Privado encargado de Actualizar el Estatus de la Factura
        /// </summary>
        /// <param name="accion"></param>
        private void actualizaEstatusFactura(string accion)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando Estatus
            FacturadoProveedor.EstatusFactura estatus = FacturadoProveedor.EstatusFactura.EnRevision;
            
            //Validando la Acción
            switch (accion)
            {
                case "Acepta":
                    //Asignando Estatus
                    estatus = FacturadoProveedor.EstatusFactura.Aceptada;
                    break;
                case "Cancela":
                    //Asignando Estatus
                    estatus = FacturadoProveedor.EstatusFactura.Cancelada;
                    break;
                case "Rechaza":
                    //Asignando Estatus
                    estatus = FacturadoProveedor.EstatusFactura.Rechazada;
                    break;
            }
            
            //Instanciando Factura de Proveedor
            using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(Session["id_registro"])))
            {   
                //Validando que exista la Factura
                if (fp.habilitar)
                {
                    //Si se Acepta la Factura
                    if ((FacturadoProveedor.EstatusFactura)fp.id_estatus_factura == FacturadoProveedor.EstatusFactura.Aceptada)

                        //Aceptando factura
                        result = fp.AceptaFacturaProveedor(Convert.ToInt32(ddlSegNeg.SelectedValue), Convert.ToInt32(ddlTipoServ.SelectedValue),
                                                            ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Actualizando Estatus
                        result = fp.ActualizaEstatusFacturadoProveedor(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion("No existe la Factura");
                
                //Validando si la Operación fue exitosa
                if (result.OperacionExitosa)
                {   
                    //Asignando Sessiones
                    Session["id_registro"] = result.IdRegistro;
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    
                    //Invocando Método de Inicialización
                    inicializaPagina();
                }
                
                //Mostrando Mensaje
                lblErrorFactura.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Conceptos de la Factura con su Clasificación
        /// </summary>
        private void cargaConceptosClasificacion()
        {   
            //Obteniendo Conceptos Ligados
            using (DataTable dtConceptosClas = FacturadoProveedorConcepto.ObtieneConceptosClasificacionFactura(Convert.ToInt32(Session["id_registro"])))
            {   
                //Validando que existan los Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtConceptosClas))
                {   
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvConceptos, dtConceptosClas, "Id", "", true, 1);
                    
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtConceptosClas, "Table1");
                    
                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvConceptos);
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvConceptos);
                    
                    //Eliminando Tabla de ViewState
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        #endregion

        #endregion
    }
}
