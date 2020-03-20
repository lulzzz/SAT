using Microsoft.SqlServer.Types;
using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Facturacion;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class Despacho : System.Web.UI.Page
    {
        #region Servicios

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
           // SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)
            {
                //Inicializa Pagina
                inicializaPagina();
                //VAlida los datos del querystring
                if (Request.QueryString["noServicio"] != null)
                {
                    //Asigna a control NoServicio el valor del queryString
                    txtNoServicio.Text = Request.QueryString["noServicio"];
                    //Invoca al método carga los servicios a buscar
                    cargaServiciosParaDespacho();
                    //Validando que existan Registros en el GridView
                    if (gvServicios.DataKeys.Count > 0)
                    {
                        //Selecciona la fila del gridview
                        Controles.MarcaFila(gvServicios, Request.QueryString["noServicio"], "Servicio");
                        //Invoca al método que carga las paradas de un servicio
                        cargaParadas();
                    }
                }

                //Establece el foco al control
                txtNoServicio.Focus();

            }
            else
            {
                /*/Carga Grid View para Insertar los link creado Asignaciones, Agregar,
                //Validando que exista DS en Session
                if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table3"))
                {
                    //Cargamos Recursos Disponibles
                    cargaRecursosDiponibles();
                }

                //Validando que exista DS en Session
                if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Table4"))
                {
                    //Cargando GridView Recursos Asignados
                    Controles.CargaGridView(gvRecursosAsignados, ((DataSet)Session["DS"]).Tables["Table4"], "Id-IdRecurso", "", false, 0);
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvRecursosAsignados);
                    //Eliminamos tabla Recursos Asignados
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                }*/
            }
        }

        /// <summary>
        /// Cerrar alguna ventana modal
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

                case "calcularRuta":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("calcularRuta", lkbCerrar);
                    break;
                case "EncabezadoServicio":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("encabezadoServicio", lkbCerrar);
                    break;
                case "Devolucion":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("devolucion", lkbCerrar);
                    break;

            }
            //Inicializamos Grid Paradas
            Controles.InicializaGridview(gvParadas);
        }
        /// <summary>
        /// Evento producido al dar clic sobre algún elemento de menú
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Determinando el botón pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "InsertarParada":
                    //Validamos existan Registros
                    if (gvParadas.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvParadas.SelectedIndex != -1)
                        {
                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(uplkbInsertaParada, uplkbInsertaParada.GetType(), "AbreVentanaModal", "contenidoInsertaParada", "confirmacionInsertaParada");

                            //Inicializamos Valores 
                            inicializaValoresInsertaParada();

                            //Cargando Catalogos 
                            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 40, "", Convert.ToInt32(ddlTipoParada.SelectedValue),
                                                                   "", 0, "");

                            //Generamos script para validación de Fechas
                            string script =
                            @"<script type='text/javascript'>
                
                            var validacionParada = function (evt) {
                                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                                var isValid1 = !$('#ctl00_content1_txtUbicacionP').validationEngine('validate');
                                var isValid2 = !$('#ctl00_content1_txtCita').validationEngine('validate');
                                return isValid1 && isValid2
                            }; 
                            //Botón Buscar
                            $('#ctl00_content1_btnAceptarInsertaParada').click(validacionParada);
                            </script>";


                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            System.Web.UI.ScriptManager.RegisterStartupScript(uplkbInsertaParada, uplkbInsertaParada.GetType(), "validacionParada", script, false);

                        }
                    }
                    break;
                case "ReversaLlegada":
                    //Validamos existan Registros
                    if (gvParadas.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvParadas.SelectedIndex != -1)
                        {
                            //Reversa Llegada
                            reversaFechaLlegada();
                        }
                    }
                    break;
                case "ReversaSalida":
                    //Validamos existan Registros
                    if (gvParadas.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvParadas.SelectedIndex != -1)
                        {
                            //Reversa Salida
                            reversaFechaSalida();
                        }
                    }
                    break;
                case "DeshabilitaParada":
                    //Validamos Existencia de Paradas
                    if (gvParadas.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvParadas.SelectedIndex != -1)
                        {
                            //Validamos Documentos Recibidos ligando el segmento de la Parada
                            validaDocumentosDeshabilitaParada();
                        }
                    }
                    break;
                case "Clasificacion":
                    //Validamos Existencia de Servicios
                    if (gvServicios.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvServicios.SelectedIndex != -1)
                        {
                            //Mostramos Vista
                            mtvDocumentacion.ActiveViewIndex = 0;
                            lkbCerrarDocumentacion.CommandName = "";

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(uplkbClasificacion, uplkbClasificacion.GetType(), "AbreVentanaModal", "modalDocumentacion", "contenidoDocumentacion");

                            //Clasificación
                            wucClasificacion.InicializaControl(1, Convert.ToInt32(gvServicios.SelectedValue), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        }
                    }
                    break;
                case "ReferenciasViaje":
                    //Validamos Existencia de Servicios
                    if (gvServicios.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvServicios.SelectedIndex != -1)
                        {
                            //Mostramos Vista
                            mtvDocumentacion.ActiveViewIndex = 1;
                            lkbCerrarDocumentacion.CommandName = "";

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(uplkbReferenciaViaje, uplkbReferenciaViaje.GetType(), "AbreVentanaModal", "modalDocumentacion", "contenidoDocumentacion");

                            //Referencias de Viaje
                            wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedValue));

                        }
                    }
                    break;
                case "Producto":
                    //Validamos Existencia de Paradas
                    if (gvServicios.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvServicios.SelectedIndex != -1)
                        {
                            //Mostramos Vista
                            mtvDocumentacion.ActiveViewIndex = 2;
                            lkbCerrarDocumentacion.CommandName = "";

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(uplkbProducto, uplkbProducto.GetType(), "AbreVentanaModal", "modalDocumentacion", "contenidoDocumentacion");

                            //Productos
                            wucProducto.InicializaControl(Convert.ToInt32(gvServicios.SelectedValue), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        }
                    }
                    break;
                case "MTCDespacho":
                    //Declarando Objeto de Retorno
                    RetornoOperacion resultado = new RetornoOperacion();
                    //Validamos la configuracion de MTC
                    //Validamos existan Registros
                    if (gvParadas.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvServicios.SelectedIndex != -1)
                        {
                            //Instanciando los parametros 
                            using (DataTable dtViaje = SAT_CL.Documentacion.Reportes.CreaciondeViaje(Convert.ToInt32(gvServicios.SelectedDataKey.Value)))
                            {
                                XDocument datosObtenidos = new XDocument();
                                //Objeto de Parametros
                                XDocument parametros = new XDocument();
                                //Existen Parametros
                                if (Validacion.ValidaOrigenDatos(dtViaje))
                                {
                                    List<DataRow> EnvioViaje = (from DataRow p in dtViaje.AsEnumerable()
                                                                select p).ToList();
                                    if (EnvioViaje.Count > 0)
                                    {
                                        foreach (DataRow dr in EnvioViaje)
                                        {
                                            //Instanciando Proveedor de WB Para Consumo
                                            using (SAT_CL.Monitoreo.ProveedorWS ws = new SAT_CL.Monitoreo.ProveedorWS(Convert.ToInt32(dr["IdProvedor"])))
                                            {
                                                //Validando si existe el Registro
                                                if (ws.habilitar)
                                                {
                                                    //Validando que sea  la configuracion establecida para la compañia 
                                                    switch (ws.identificador)
                                                    {
                                                        case "MTC - Despacho":
                                                            {
                                                                //Enviamos peteción MTC "MetodoGeneraViaje"
                                                                resultado = ServicioWeb.ObtenerRespuestaWeb(ServicioWeb.CrearSolicitudWeb(ws.endpoin),
                                                                //SAT_CL.Monitoreo.ProveedorWSUnidad.CreateSoapEnvelopeBuscarDespacho("438", Convert.ToString(dr["Usuario"]), Convert.ToString(dr["Password"])));
                                                                SAT_CL.Monitoreo.ProveedorWSUnidad.CreateSoapEnvelopeBuscarDespacho(Convert.ToString(dr["Nombre"]), Convert.ToString(dr["Usuario"]), Convert.ToString(dr["Password"])));
                                                                //Validamos Solicitud exitosa
                                                                if (resultado.OperacionExitosa)
                                                                {
                                                                    ////Obtenemos Documento generado
                                                                    XDocument xDoc = XDocument.Parse(resultado.Mensaje);
                                                                    XElement Folio = xDoc.Descendants("folioDespacho").FirstOrDefault();
                                                                    XElement Fecha = xDoc.Descendants("fechaInicio").FirstOrDefault();
                                                                    XElement Tea = xDoc.Descendants("TEA").FirstOrDefault();
                                                                    XElement TeaFinal = xDoc.Descendants("TEA_FINAL").FirstOrDefault();
                                                                    //Validamos que exista Respuesta
                                                                    if (Folio != null)
                                                                    {
                                                                        //abrimos ventana modal
                                                                        alternaVentanaModal("despachoMTC", this.Page);
                                                                        lblfolio.Text = Convert.ToString(Folio);
                                                                        lblfecha.Text = Convert.ToString(Fecha);
                                                                        lblTEA.Text = Convert.ToString(Tea);
                                                                        lblTEAFinal.Text = Convert.ToString(TeaFinal);
                                                                    }
                                                                    else
                                                                        resultado = new RetornoOperacion("No existen despacho en MTC");                                                               
                                                                }
                                                                else
                                                                    //Establecmos Mensaje Resultado
                                                                    resultado = new RetornoOperacion("No es posible obtener la respuesta MTC");
                                                                break;
                                                            }
                                                        default:
                                                            //Instanciando Excepción
                                                            resultado = new RetornoOperacion("No existe la Configuración de WS para este Proveedor");
                                                            break;
                                                    }
                                                }
                                                else
                                                    resultado = new RetornoOperacion(true);
                                            }
                                        }
                                    }
                                    else
                                        resultado = new RetornoOperacion("El viaje es menor a cero");
                                }
                                else
                                    resultado = new RetornoOperacion("No existen parametros");
                            }
                        }
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de la Operación
                        ScriptServer.MuestraNotificacion(this, "Seleccione un servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                    break;
                case "Cargos":
                    //Validamos Existencia de Paradas
                    if (gvServicios.DataKeys.Count > 0)
                    {
                        //Validamos Selección
                        if (gvServicios.SelectedIndex != -1)
                        {
                            //Mostramos Vista
                            mtvDocumentacion.ActiveViewIndex = 3;
                            lkbCerrarDocumentacion.CommandName = "";

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(uplkbCargos, uplkbCargos.GetType(), "AbreVentanaModal", "modalDocumentacion", "contenidoDocumentacion");

                            //Obtenemos Fcatura
                            Facturado objFactura = Facturado.ObtieneFacturaServicio(Convert.ToInt32(gvServicios.SelectedValue));
                            //Cargos 
                            wucFacturadoConcepto.InicializaControl(objFactura.id_factura);
                        }
                    }
                    break;
                case "TerminoMovimientoVacio":
                    //Inicializando contenido de control
                    WucTerminoMovimientoVacio.InicializaControl();
                    //Registrando el script sólo para los paneles que producirán actualización del mismo
                    ScriptServer.AlternarVentana(uplkbTerminoMovimientoVacio, uplkbTerminoMovimientoVacio.GetType(), "AbreVentanaModal", "contenidoConfirmacionTerminoMovimientoVacio", "confirmacionTerminoMovimientoVacio");
                    break;
                case "CalcularRuta":
                    //Inicializando contenido de control para el Calculo de Ruta
                    wucCalcularRuta.InicializaControl(Convert.ToInt32(gvServicios.SelectedValue));
                    //Mostramos  Modal
                    alternaVentanaModal("calcularRuta", lkbCalcular);
                    break;
                case "ImprimirDocumentos":
                    //Obteniendo Ruta
                    string impresion_url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Accesorios/ImpresionDocumentos.aspx");
                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?&idRegistro={2}&idUsuario={3}", impresion_url, "Impresiones", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario), "Impresion Documentos", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES", Page);
                    break;
            }
        }

        /// <summary>
        /// Evento generado al Cerrar la Ventana de Documentación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarDocumentacion_Click(object sender, EventArgs e)
        {
            if (lkbCerrarDocumentacion.CommandName == "")
            {
                //Cerramos Ventana Modal
                ScriptServer.AlternarVentana(uplkbCerrarDocumentacion, uplkbCerrarDocumentacion.GetType(), "CerrarVentanaModal", "modalDocumentacion", "contenidoDocumentacion");

                //Inicializamos Grid Paradas
                Controles.InicializaGridview(gvParadas);
            }
            else
            {
                //Alternando Ventanas Modales
                alternaVentanaModal("referencias", this.Page);
                alternaVentanaModal("devolucion", this.Page);
            }
        }

        /// <summary>
        /// Evento generado al cerrar el termino del Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTerminoMovimiento_Click(object sender, EventArgs e)
        {

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(uplkbCerrarTerminoMovimiento, uplkbCerrarTerminoMovimiento.GetType(), "CierreVentanaModal", "contenidoConfirmacionTerminoMovimientoVacio", "confirmacionTerminoMovimientoVacio");

        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);
            //Inicializamos Valores gvParada
            InicializaValoresgvParada();

        }

        /// <summary>
        /// Mètodo encargado de Inicializar los Valores de gvParada
        /// </summary>
        private void InicializaValoresgvParada()
        {
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvParadas);
            //Eliminamos Tabla de la Session
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSevicios_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                                        Convert.ToInt32(ddlTamanoSevicios.SelectedValue), true, 2);
            //Inicializamos Valores gvParada
            InicializaValoresgvParada();
        }

        /// <summary>
        /// Evento generado al cambiar el tipo de Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoParada_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargando Catalogos  de Evento
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 40, "", Convert.ToInt32(ddlTipoParada.SelectedValue),
                                                   "", 0, "");
            //Validamos Tipo de Parada
            if ((Parada.TipoParada)Convert.ToInt32(ddlTipoParada.SelectedValue) == Parada.TipoParada.Operativa)
            {
                txtCita.Enabled = true;
                //Generamos script para validación de Fechas
                string script =
                @"<script type='text/javascript'>
                
                    var validacionParada = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#ctl00_content1_txtUbicacionP').validationEngine('validate');
                        var isValid2 = !$('#ctl00_content1_txtCita').validationEngine('validate');
                        return isValid1 && isValid2
                    }; 
                //Botón Buscar
                $('#ctl00_content1_btnAceptarInsertaParada').click(validacionParada);
                                    </script>";


                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterStartupScript(upddlTipoParada, upddlTipoParada.GetType(), "validacionParada", script, false);

            }
            else
            {
                //Deshabilitamos Controles
                txtCita.Enabled = false;
                txtCita.Text = "";

                //Generamos script para validación de Fechas
                string script =
                @"<script type='text/javascript'>
                
                    var validacionParada = function (evt) {
                        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                        var isValid1 = !$('#ctl00_content1_txtUbicacionP').validationEngine('validate');
                        return isValid1 
                    }; 
                //Botón Buscar
                $('#ctl00_content1_btnAceptarInsertaParada').click(validacionParada);
                                    </script>";


                //Registrando el script sólo para los paneles que producirán actualización del mismo
                System.Web.UI.ScriptManager.RegisterStartupScript(upddlTipoParada, upddlTipoParada.GetType(), "validacionParada", script, false);

            }
        }

        /// <summary>
        /// Método encargado de Enlazar el Grid View Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    {
                        //Creamos instancias del tipo LinkButton correspondientes 
                        using (LinkButton lkbTerminarServicio = (LinkButton)fila.FindControl("lkbTerminarServicio"),
                          lkbValidacionTerminar = (LinkButton)fila.FindControl("lkbValidacionTerminar"))
                        {
                            //Si no existe la Fecha de Llegada
                            if (lkbValidacionTerminar.Text == "True")
                            {
                                //Habilitamos links Terminar Servicio
                                lkbTerminarServicio.Enabled = true;
                            }
                            else
                            {
                                //Deshabilitamos links Terminar Servicio
                                lkbTerminarServicio.Enabled = false;
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServicios_OnClick(object sender, EventArgs e)
        {
            //Exportando de Servicios
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarSevicios.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
            //Inicializa Valores Parada
            InicializaValoresgvParada();
        }


        /// <summary>
        /// Evento Generado al dar click  Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbServicio_OnClick(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;
            //Validamos que existan Servicios
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //En base al comando definido para el botón
                switch (b.CommandName)
                {
                    case "Terminar":
                        {
                            //Mostramos Ventana Modal
                            ScriptServer.AlternarVentana(upgvSevicios, upgvSevicios.GetType(), "AbreVentanaModal", "contenidoFechaTerminoServicio", "confirmacionFechaTerminoServicio");

                            //Inicializamos fecha Termino
                            txtFechaTermino.Text = Parada.ObtieneFechaSalidaUltimaParada(Convert.ToInt32(gvServicios.SelectedValue)).ToString("dd/MM/yyyy HH:mm");

                            break;
                        }
                    case "Calcular":
                        {
                            //Calcula Kilometraje
                            calculaKilometraje();
                            break;
                        }
                    case "Seleccionar":
                        {
                            //Carga Paradas
                            cargaParadas();
                            break;
                        }
                    case "ReabrirServicio":
                        {
                            //Reabrir Servicio
                            reabrirServicio();
                            break;
                        }
                    case "CartaPorte":
                        {
                            //Invocando Método de Validación en la Carta Porte
                            if (SAT_CL.Documentacion.Servicio.ValidaImpresionEspecificaCartaPorte(Convert.ToInt32(gvServicios.SelectedValue)).OperacionExitosa)
                            {
                                //Mostramos Modal
                                alternaVentanaModal("impresionPorte", b);
                                //Inicializando control
                                wucImpresionPorte.InicializaImpresionCartaPorte(Convert.ToInt32(gvServicios.SelectedValue), UserControls.wucImpresionPorte.TipoImpresion.CartaPorte);
                            }
                            else
                            {
                                //Construyendo URL
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Porte", Convert.ToInt32(gvServicios.SelectedValue)), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            }
                            break;
                        }
                    case "BitacoraViaje":
                        {
                            //Invocando Método de Validación en la Carta Porte
                            if (SAT_CL.Documentacion.Servicio.ValidaImpresionEspecificaCartaPorte(Convert.ToInt32(gvServicios.SelectedValue)).OperacionExitosa)
                            {
                                //Mostramos Modal
                                alternaVentanaModal("impresionPorte", b);
                                //Inicializando control
                                wucImpresionPorte.InicializaImpresionCartaPorte(Convert.ToInt32(gvServicios.SelectedValue), UserControls.wucImpresionPorte.TipoImpresion.BitacoraViaje);
                            }
                            else
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "BitacoraViaje", Convert.ToInt32(gvServicios.SelectedValue)), "Bitacora de Viaje", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            }
                            break;
                        }
                    case "EncabezadoServicio":
                        //Validamos Existencia de Servicios
                        if (gvServicios.DataKeys.Count > 0)
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                //Mostramos Modal
                                alternaVentanaModal("encabezadoServicio", b);
                                //Inicializando control de referencias de servicio
                                wucEncabezadoServicio.InicializaEncabezadoServicio(Convert.ToInt32(gvServicios.SelectedValue));
                            }
                        }
                        break;
                    case "ReferenciasViaje":
                        //Validamos Existencia de Servicios
                        if (gvServicios.DataKeys.Count > 0)
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                //Mostramos Vista
                                mtvDocumentacion.ActiveViewIndex = 1;
                                lkbCerrarDocumentacion.CommandName = "";

                                //Registrando el script sólo para los paneles que producirán actualización del mismo
                                alternaVentanaModal("referencias", b);

                                //Referencias de Viaje
                                wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedValue));

                            }
                        }
                        break;
                    //Si la eleccion del menu es la opcion Imprimir 
                    case "CartaPorteViajera":
                        //Validamos Existencia de Servicios
                        if (gvServicios.DataKeys.Count > 0)
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "PorteViajera", Convert.ToInt32(gvServicios.SelectedValue)), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            }
                        }
                        break;
                    //Si la eleccion del menu es la opcion Imprimir 
                    case "HojaInstrucciones":
                        //Validamos Existencia de Servicios
                        if (gvServicios.DataKeys.Count > 0)
                        {
                            //Validamos Selección
                            if (gvServicios.SelectedIndex != -1)
                            {
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                                //Instanciando nueva ventana de navegador para apertura de registro
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "HojaDeInstruccion", Convert.ToInt32(gvServicios.SelectedValue)), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Evento Generado al dar  click en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Cargamos Servicios 
            cargaServiciosParaDespacho();
            //Inicicializamos Indices
            Controles.InicializaIndices(gvServicios);
            //Inicializamos Grid View Paradas
            Controles.InicializaGridview(gvParadas);
            //Limpiamos Etiqueta Servicio
            lblErrorServicio.Text = "";
            lblErrorParada.Text = "";
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa Pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializamos Valores
            inicializaValoresFechaLlegada();
            inicializaValoresFechaSalida();
            //Habilitamos valores Llegada
            habilitaControlesFechaLlegada();
            habilitaControlesFechaSalida();
            //Cargamos Catalogo
            cargaCatalogos();

            //Inicializamos Grid Views
            Controles.InicializaGridview(gvParadas);
            Controles.InicializaGridview(gvServicios);
            Controles.InicializaGridview(gvEventos);

            //Inicializamos los indicadores
            inicializaIndicadores();
        }

        /// <summary>
        ///Carga Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Servicios
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSevicios, "", 56);
            //Paradas
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadas, "", 56);
            //Eventos
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEventos, "", 18);
            //Razón Llegada/Salida Tarde
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRazonLlegadaTarde, "Ninguna", 21);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRazonSalidaTarde, "Ninguna", 21);
            //Motivo Retraso
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMotivoRetraso, "Ninguno", 64);
            //Tipo Parada
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoParada, "", 19);
            //Cargando Catalogos 
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 40, "", Convert.ToInt32(ddlTipoParada.SelectedValue),
                                                   "", 0, "");
        }
        /// <summary>
        /// Metodo encargado de cargar los distintos indicadores relacionados con la forma
        /// </summary>
        private void inicializaIndicadores()
        {
            ///Cargamos los indicadores ligados al servicio
            using (DataTable t = Servicio.CargaIndicadoresServicios(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validamos que la consulta se haya generado
                if (Validacion.ValidaOrigenDatos(t))
                {
                    //Obtenemos los indicadores de interes 
                    foreach (DataRow r in t.Rows)
                    {
                        lblIniciar.Text = r["PorIniciar"].ToString();
                        lblTransito.Text = r["EnTransito"].ToString();
                        lblTerminar.Text = r["PorTerminar"].ToString();
                    }
                }
            }
            //Cargamos los indicadores ligados a unidades
            using (DataTable t = Unidad.CargaIndicadoresUnidades(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validamos que la consulta se haya generado
                if (Validacion.ValidaOrigenDatos(t))
                {
                    //Obtenemos los indicadores de interes 
                    foreach (DataRow r in t.Rows)
                    {
                        lblCajas.Text = r["EnEstancia"].ToString();
                        lblEstancia.Text = r["TiempoEstancia"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Realiza la carga de servicios para despacho
        /// </summary>
        private void cargaServiciosParaDespacho()
        {
            //Obteniendo detalles de viaje
            using (DataTable dt = SAT_CL.Documentacion.Reportes.CargaServiciosParaDespacho(Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCliente.Text, "0"), ':', 1)),
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCiudadOrigen.Text, "0"), ':', 1)),
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCiudadDestino.Text, "0"), ':', 1)),
                                                                     ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoServicio.Text,
                                                                     chkTerminado.Checked, txtNoReferencia.Text, txtCartaPorte.Text))
            {
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvServicios, dt, "Id-IdCliente-Servicio", lblOrdenarSevicios.Text, true, 2);

                //Validando que la Tabla Contenga Registros
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Inicializa los indicadores
            inicializaIndicadores();
        }
        /// <summary>
        /// Realiza el proceso de gestión del termino del Viaje
        /// </summary>
        private void realizaProcesoTerminoServicio()
        {
            //Validando eventos pendientes de actualizar
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos última parada del servicio en cuestión
            using (Parada objParada = new Parada(Parada.ObtieneUltimaParada(Convert.ToInt32(gvServicios.SelectedValue))))
            {
                //Validando Ultima Parada
                if (objParada.habilitar)
                {
                    //Validando Estatus
                    if (objParada.Estatus == Parada.EstatusParada.Iniciado)
                    {
                        //Validando eventos pendientes de actualizar
                        resultado = ParadaEvento.ValidaEventosSinIniciar(Convert.ToInt32(gvServicios.SelectedValue), objParada.id_parada);

                        //Si no hay pendientes
                        if (resultado.OperacionExitosa)
                        {
                            //Terminando Servicio
                            terminarServicio();
                            //Mostramos Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarTerminoServicio, btnAceptarTerminoServicio.GetType(), "CierreVentanaModal", "contenidoFechaTerminoServicio", "confirmacionFechaTerminoServicio");
                        }
                        //Si hay pendientes
                        else
                        {
                            //Mostramos Mensaje
                            lblMensajeEventos.Text = resultado.Mensaje;
                            //Asignando comando
                            btnSiActualizacionEventos.CommandName =
                            btnNoActualizacionEventos.CommandName = "Servicio";
                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(btnAceptarTerminoServicio, btnAceptarTerminoServicio.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");
                            //Mostramos Ventana Modal
                            ScriptServer.AlternarVentana(btnAceptarTerminoServicio, btnAceptarTerminoServicio.GetType(), "CierreVentanaModal", "contenidoFechaTerminoServicio", "confirmacionFechaTerminoServicio");
                        }
                    }
                }
                else
                {   
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("Tienes paradas pendientes por Terminar, aún no puedes terminar el Servicio.");
                    //Mostrando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Terminamos Servicio
        /// </summary>
        private void terminarServicio()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedValue)))
            {
                //Terminamos Servicio
                resultado = objServicio.TerminaServicio(Convert.ToDateTime(txtFechaTermino.Text), Parada.TipoActualizacionSalida.Manual, ParadaEvento.TipoActualizacionFin.Manual, ((Usuario)Session["usuario"]).id_usuario);
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Servicios
                cargaServiciosParaDespacho();

                //Inicializamos Indices
                Controles.InicializaIndices(gvServicios);
                Controles.InicializaGridview(gvParadas);
            }
            //Mostramos Resultado
            lblErrorTerminoServicio.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Reabrir Servicio
        /// </summary>
        private void reabrirServicio()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedValue)))
            {
                //Terminamos Servicio
                resultado = objServicio.ReversaTerminaServicio(((Usuario)Session["usuario"]).id_usuario);
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Servicios
                cargaServiciosParaDespacho();

                //Inicializamos Indices
                Controles.InicializaIndices(gvServicios);
                Controles.InicializaGridview(gvParadas);

            }
            //Mostramos Resultado
            lblErrorServicio.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Calculamos Kilometraje
        /// </summary>
        private void calculaKilometraje()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Servicio
            using (Servicio objServicio = new Servicio(Convert.ToInt32(gvServicios.SelectedValue)))
            {
                //Terminamos Servicio
                resultado = objServicio.CalculaKilometrajeServicio(((Usuario)Session["usuario"]).id_usuario);
            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Servicios
                cargaServiciosParaDespacho();

                //Cargamos Parads
                cargaParadas();
            }
            //Mostramos Resultado
            lblErrorServicio.Text = resultado.Mensaje;
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
                case "calcularRuta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoCalcularRuta", "confirmacionCalcularRuta");
                    break;
                case "encabezadoServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "encabezadoServicioModal", "encabezadoServicio");
                    break;
                case "devolucion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalDevolucionFaltante", "devolucionFaltante");
                    break;
                case "referencias":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalDocumentacion", "contenidoDocumentacion");
                    break;
                case "RecursosBitacora":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaRecursosMovimiento", "ventanaRecursosMovimiento");
                    break;
                case "historialBitacora":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialBitacoraModal", "historialBitacora");
                    break;
                case "bitacoraMonitoreo":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "bitacoraMonitoreoModal", "bitacoraMonitoreo");
                    break;
                case "impresionPorte":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaImpresionPorte", "ventanaImpresionPorte");
                    break;
                case "despachoMTC":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "despachoModalMTC", "despachoMTC");
                    break;
            }
        }


        #endregion

        #endregion

        #region Paradas

        #region Eventos

        /// <summary>
        /// Evento generado al dar click en la Bitácora de la Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacoraParada_Click(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
                //Mostramos Bitácora
                inicializaBitacora(gvParadas.SelectedDataKey["IdOrigen"].ToString(), "5", "Parada");
            }
        }

        /// <summary>
        /// Evento Generado al dar click al Cancelar la Insercción de Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarInsertaParada_Click(object sender, EventArgs e)
        {
            //Inicializamos Valores
            inicializaValoresInsertaParada();

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(btnCancelarInsertaParada, btnCancelarInsertaParada.GetType(), "CierreVentanaModal", "contenidoInsertaParada", "confirmacionInsertaParada");

        }
        /// <summary>
        /// Evento Generado al dar click al Aceptar la Insercción de Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarInsertaParada_Click(object sender, EventArgs e)
        {
            //Validamos que no existan documentos recibidos
            validaDocumentosInsertaParada();
        }

        /// <summary>
        /// Seleccionamos Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionParada_OnClick(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
        }
        /// <summary>
        /// Evento producido al seleccionar una Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbParadas_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
        }
        /// <summary>
        /// Método encargado de Enlazar el Grid View Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    {
                        //Recuperando fila de datos
                        DataRow datos = ((DataRowView)fila.DataItem).Row;

                        //Creamos instancias del tipo LinkButton correspondientes a las Fechas de la Parada
                        using (LinkButton lkbFechaLlegada = (LinkButton)fila.FindControl("lkbFechaLlegada"),
                          lkbFechaSalida = (LinkButton)fila.FindControl("lkbFechaSalida"),
                          lkbDevolucion = (LinkButton)fila.FindControl("lnkDevolucionMov")
                          )
                        {
                            //Si  existe la Fecha de Llegada
                            if (lkbFechaLlegada.Text != "Sin Asignar")
                            {
                                //Deshabilitamos Control
                                lkbFechaLlegada.Enabled = false;

                                //Si no existe la Fecha de Salida
                                if (lkbFechaSalida.Text == "Sin Asignar")
                                {
                                    //Asignamos color a la fila
                                    fila.BackColor = System.Drawing.ColorTranslator.FromHtml("#A6FC67");
                                }

                            }
                            else
                            {
                                //Si no existe la Fecha de Llegada
                                //Validamos que no sea la primer parada del servicio
                                if (e.Row.RowIndex != 0)
                                {
                                    //Creamos instancias del tipo LinkButton correspondientes a las Fechas de la Parada anterior
                                    using (LinkButton lkbFechaLlegadaAnterior = (LinkButton)gvParadas.Rows[e.Row.RowIndex - 1].FindControl("lkbFechaLlegada"),
                                         lkbFechaSalidaAnterior = (LinkButton)gvParadas.Rows[e.Row.RowIndex - 1].FindControl("lkbFechaSalida"))
                                    {
                                        //Si existen fechas de la parada anterior
                                        if (lkbFechaLlegadaAnterior.Text != "Sin Asignar" && lkbFechaSalidaAnterior.Text != "Sin Asignar")
                                        {
                                            //Asignamos color a la fila
                                            fila.BackColor = System.Drawing.ColorTranslator.FromHtml("#A6FC67");
                                        }
                                    }
                                }
                                else
                                {
                                    //Asignamos color a la fila
                                    fila.BackColor = System.Drawing.ColorTranslator.FromHtml("#A6FC67");
                                }
                            }

                            //Si no existe la Fecha de Salida
                            if (lkbFechaSalida.Text != "Sin Asignar")
                            {
                                //Deshabilitamos
                                lkbFechaSalida.Enabled = false;
                            }

                            if (!datos["Secuencia"].Equals(DBNull.Value))
                            {
                                //Determinando secuencia de parada visualizada
                                if (Convert.ToByte(datos["Secuencia"]) > 1)
                                    //Se muestra opción de devolución
                                    lkbDevolucion.Visible = true;
                                else
                                    //Se oculta opción de devolución
                                    lkbDevolucion.Visible = false;
                            }
                        }
                        
                    }
                    break;
            }
        }
        /// <summary>
        /// Evento encargado de 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBitacoraMonitoreo_Click(object sender, ImageClickEventArgs e)
        {
            //Validando que existan registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvParadas, sender, "imb", false);

                //Cargando Recursos Asignados
                cargaRecursosMovimiento();

                //Inicializando Indices
                Controles.InicializaIndices(gvRecursosAsignados);

                //Muestra Ventana
                alternaVentanaModal("RecursosBitacora", this.Page);
            }
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Paradas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvParadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Evento producido al pulsar el link Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEventos_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

            //Instanciamos Parad
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Validamos exista Parada
                if (objParada.habilitar)
                {
                    //Cargamos Eventos
                    cargaEventos(objParada.id_parada);

                    //Cargando Catalogos 
                    CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventos, 40, "", objParada.id_tipo_parada, "", 0, "");

                    //Registrando el script sólo para los paneles que producirán actualización del mismo
                    ScriptServer.AlternarVentana(upgvParadas, upgvParadas.GetType(), "CierreVentanaModal", "contenidoActualizacionEventos", "actualizacionEventos");
                }
            }
        }

        /// <summary>
        /// Evento producido al pulsar el link Asignaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAsignacionRecursos_OnClick(object sender, EventArgs e)
        {
            //Validamos que existan Movimientos
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

                //Cargamos Recursos Asignados
                ucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]),
                                                                ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Mostramos Ventana Modal
                ScriptServer.AlternarVentana(upgvParadas, upgvParadas.GetType(), "AbreVentanaModal", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoParadas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvParadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"),
                                        Convert.ToInt32(ddlTamanoParadas.SelectedValue), true, 3);
        }


        /// <summary>
        /// Evento Producido al pulsar el botón "Exportar Paradas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarParadas_OnClick(object sender, EventArgs e)
        {
            //Exportando de Servicios
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }

        /// <summary>
        /// Evento Generado al dar click en Aceptar Fecha Salida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarIngresoSalida_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si existen eventos Sin Iniciar
            resultado = ParadaEvento.ValidaEventosSinIniciar(Convert.ToInt32(gvServicios.SelectedValue), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]));

            //Validamos Resultado
            if (!resultado.OperacionExitosa)
            {
                //Mostramos Mensaje
                lblMensajeEventos.Text = resultado.Mensaje;

                //Asignando comando
                btnSiActualizacionEventos.CommandName =
                btnNoActualizacionEventos.CommandName = "Parada";

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upbtnAceptarIngresoSalida, upbtnAceptarIngresoSalida.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upbtnAceptarIngresoSalida, upbtnAceptarIngresoSalida.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");
            }
            else
            {
                //Instanciando la parada actual
                using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
                {
                    //Obteniendo movimiento con origen en la parada de interés
                    using (Movimiento mov = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(Convert.ToInt32(gvServicios.SelectedDataKey.Value), objParada.id_parada)))
                        //Validando existencia de vencimientos para alguno de los recursos asignados
                        resultado = validaVencimientosActivosMovimiento(mov.id_movimiento);

                    //Si no hay vencimientos
                    if (resultado.OperacionExitosa)
                    {
                        //Actualizando salida
                        resultado = actualizaFechaSalida();

                        //Si no hay errores de actualización
                        if (resultado.OperacionExitosa)
                            //Cerrando ventana de confirmación de hora de salida
                            ScriptServer.AlternarVentana(upbtnAceptarIngresoSalida, upbtnAceptarIngresoSalida.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
                    }
                    //Si existen vencimientos
                    else
                    {
                        //Cerrando ventana de confirmación de hora de salida
                        ScriptServer.AlternarVentana(upbtnAceptarIngresoSalida, upbtnAceptarIngresoSalida.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
                        //Mostrando ventana de notificación de vencimientos
                        ScriptServer.AlternarVentana(btnAceptarIngresoSalida, btnAceptarIngresoSalida.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                    }
                }
            }
        }




        /// <summary>
        /// Evento producido al pulsar el link Fecha Llegada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbFechaLlegada_OnClick(object sender, EventArgs e)
        {
            //Validamos que existan Paradas
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

                //Validamos la parada es la Inicial
                if (Convert.ToDecimal(gvParadas.SelectedDataKey["Secuencia"]) == 1)
                {
                    //Actualizamos Etiquetas de la ventana Modal
                    lblTituloActualizacionLlegada.Text = " Inicial del Servicio.";
                    lblMensajeLlegada.Text = "Al continuación iniciará el Servicio.";
                    lblFechaSalida.Text = "";
                    lblValorFechaSalida.Visible = false;
                }
                else
                {
                    //Mostramos Fecha de Salida de la Parada Anterior
                    lblValorFechaSalida.Visible = true;
                    //Obtenemos Parada Anterior
                    using (Parada objParadaAnterior = new Parada(Convert.ToDecimal(gvParadas.SelectedDataKey["Secuencia"]) - 1, Convert.ToInt32(gvServicios.SelectedDataKey.Value)))

                    {
                        //Asignamos Fecha de Salida
                        lblFechaSalida.Text = objParadaAnterior.fecha_salida == DateTime.MinValue ? "Sin Fecha" : objParadaAnterior.fecha_salida.ToString("dd/MM/yyyy HH:mm");
                    }

                }
                //Asignamos Cita  de la parada a control
                lblCitaLlegada.Text = gvParadas.SelectedDataKey["Cita"].ToString();
                //Inicializamos Control
                txtFechaLlegada.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                //Mostrando Recursos
                muestraRecursosMovimiento(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]));
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upgvParadas, upgvParadas.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionLlegada", "confirmacionActualizacionLlegada");
            }
        }
        /// <summary>
        /// Método encargado de Mostrar los Recursos del Movimiento
        /// </summary>
        private void muestraRecursosMovimiento(int idMovimiento)
        {
            //Obteniendo Recursos
            using (DataTable dt = MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(idMovimiento))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Obteniendo Operadores
                    DataRow op = (from DataRow dr in dt.Rows
                                  where dr["Asignacion"].ToString().Equals("Operador")
                                  select dr).FirstOrDefault();
                    //Validando Op
                    if (op != null)
                    {
                        //Asignando Valores
                        lblOp.Text = op["Descripcion"].ToString();
                        divOperador.Visible = true;
                    }
                    else
                        //Ocultando Controles
                        divOperador.Visible = false;

                        //Obteniendo Operadores
                        DataRow pr = (from DataRow dr in dt.Rows
                                      where dr["Asignacion"].ToString().Equals("Operador")
                                      select dr).FirstOrDefault();
                    //Validando Pr
                    if (pr != null)
                    {
                        //Asignando Valores
                        lblPr.Text = pr["Descripcion"].ToString();
                        divTercero.Visible = true;
                    }
                    else
                        //Ocultando Controles
                        divTercero.Visible = false;

                    //Obteniendo Unidades
                    List<DataRow> uns = (from DataRow dr in dt.Rows
                                         where !dr["Asignacion"].ToString().Equals("Operador") &&
                                               !dr["Asignacion"].ToString().Equals("Tercero")
                                         select dr).ToList();
                    //Validando Un
                    if (uns.Count > 0)
                    {
                        //Creando Contador
                        int cont = 1;
                        divUnidades.Visible = true;
                        lblU1.Text = lblU2.Text = lblU3.Text = "----";

                        //Recorriendo Unidades
                        foreach (DataRow uni in uns)
                        {
                            switch (cont)
                            {
                                case 1:
                                    //Asignando Valores
                                    lblU1.Text = uni["Descripcion"].ToString();
                                    break;
                                case 2:
                                    //Asignando Valores
                                    lblU2.Text = uni["Descripcion"].ToString();
                                    break;
                                case 3:
                                    //Asignando Valores
                                    lblU2.Text = uni["Descripcion"].ToString();
                                    break;
                            }

                            //Incrementando Contador
                            cont++;
                        }
                    }
                    else
                        //Ocultando Controles
                        divUnidades.Visible = false;
                }
                else
                    //Ocultando Controles
                    divOperador.Visible = divUnidades.Visible = divTercero.Visible = false;
                
            }
        }

        /// <summary>
        /// Evento producido al pulsar el link Fecha Salida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbFechaSalida_OnClick(object sender, EventArgs e)
        {
            //Validamos que existan Movimientos
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

                //Asignamos Fecha de llegada de la parada
                lblFechaLlegada.Text = gvParadas.SelectedDataKey["FechaLlegada"].ToString();
                //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Salida
                inicializaValoresFechaSalida();
                //Inicializamos Control
                txtFechaSalida.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                habilitaControlesFechaSalida();

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upgvParadas, upgvParadas.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
            }
        }
        /// <summary>
        /// Evento Generado al dar click en Cancelar  Fecha Llegada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarIngresoLlegada_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCancelarIngresoLlegada, upbtnCancelarIngresoLlegada.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionLlegada", "confirmacionActualizacionLlegada");

            //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Llegada
            inicializaValoresFechaLlegada();
            habilitaControlesFechaLlegada();
        }

        /// <summary>
        /// Evento Generado al dar click en Cancelar  Fecha Salida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarIngresoSalida_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCancelarIngresoSalida, upbtnCancelarIngresoSalida.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");

            //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Salida
            inicializaValoresFechaSalida();
            habilitaControlesFechaSalida();
        }
        /// <summary>
        /// Evento Generado al dar click en el link de Kilometraje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbKilometrajeMov_Click(object sender, EventArgs e)
        {
            //Validamos que existan Movimientos
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion retorno = new RetornoOperacion();

                //Instanciamos nuestro movimiento 
                using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"])))
                {
                    //Validamos que el movimiento tenga un id de servicio ligado 
                    if (objMovimiento.id_servicio != 0)
                    {
                        //En caso de que el movimiento tenga un servicio ligado, instanciamos nuestro servicio
                        using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                        {
                            //Realizamos la actualizacion del kilometraje
                            retorno = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                    else
                    {
                        //En caso de que el movimiento no tenga id de servicio ligado
                        //Invocamos el metodo de actualizacion de kilometraje del movimiento
                        retorno = Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }

                    switch (retorno.IdRegistro)
                    {
                        //Caso en el que no hubo cambios en el kilometraje
                        case -50:
                            {
                                //No hay tareas por realizar
                                break;
                            }
                        //Caso en el que no se encontro el kilometraje
                        case -100:
                            {
                                using (Parada origen = new Parada(objMovimiento.id_parada_origen), destino = new Parada(objMovimiento.id_parada_destino))
                                {

                                    //Inicializando Control
                                    ucKilometraje.InicializaControlKilometraje(0, objMovimiento.id_compania_emisor, origen.id_ubicacion, destino.id_ubicacion);
                                    //Alternando Ventana
                                    TSDK.ASP.ScriptServer.AlternarVentana(upgvParadas, upgvParadas.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");

                                }
                                break;
                            }
                        //Caso en el que se actualizo 
                        default:
                            {
                                //Actualizamos el grid de paradas
                                cargaParadas();
                                break;
                            }
                    }
                }
                //Mostrando Mensaje de Operación
                lblErrorServicio.Text = retorno.Mensaje;
            }
        }

        /// <summary>
        /// Evento Producido al Guardar el Kilometraje
        /// </summary>
        protected void ucKilometraje_ClickGuardar(object sender, EventArgs e)
        {
            //Declarando Objeto de retorno
            RetornoOperacion result = new RetornoOperacion();

            //Invocando Método de Guardado
            result = ucKilometraje.GuardaKilometraje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //REALIZAMOS LA ACTUALIZACION DEL KILOMETRAJE DEL MOVIMIENTO SELECCIONADO
                //Validamos que existan Movimientos
                if (gvParadas.DataKeys.Count > 0)
                {
                    //Declarando Objeto de Retorno
                    RetornoOperacion retorno = new RetornoOperacion();

                    //Instanciamos nuestro movimiento 
                    using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"])))
                    {
                        //Instanciamos nuestro servicio
                        using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                        {
                            //Realizamos la actualizacion del kilometraje
                            retorno = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                }

                //ACTUALIZAMOS EL GRID DE PARADAS
                //Cargando Paradas
                cargaParadas();
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrar_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvParadas);

            //Mostrando ventana modal con resultados
            TSDK.ASP.ScriptServer.AlternarVentana(uplkbCerrar, uplkbCerrar.GetType(), "VentanaKilometraje", "ventanaKilometraje", "contenedorVentanaKilometraje");
        }
        /// <summary>
        /// Evento click del link Ver Historial del Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerHistorialVencimientos_Click(object sender, EventArgs e)
        {
            //Abriendo ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(lkbVerHistorialVencimientos, lkbVerHistorialVencimientos.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
        }
        /// <summary>
        /// Evento click del link cerrar historial de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarHistorialVencimientos_Click(object sender, EventArgs e)
        {
            //Cerrar ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarHistorialVencimientos, lkbCerrarHistorialVencimientos.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
        }
        /// <summary>
        /// Evento cambio de página en gridview de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //cambiar pagina activa
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvVencimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento de enlace a datos de cada fila del gridview de paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVencimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si es un fila de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos
                DataRow row = ((DataRowView)e.Row.DataItem).Row;
                //Validando que existan los datios requeridos en el origen
                if (row.Table.Columns.Contains("IdPrioridad"))
                {
                    //Determinando prioridad del vencimiento
                    if ((TipoVencimiento.Prioridad)Convert.ToByte(row["IdPrioridad"]) == TipoVencimiento.Prioridad.Obligatorio)
                    {
                        //Cambiando color de forndo de la fila
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbReferenciasParada_Click(object sender, ImageClickEventArgs e)
        {
            //Validamos Registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "imb", false);
                inicializaReferenciaRegistro(gvParadas.SelectedDataKey["IdOrigen"].ToString(), "5", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            }            
        }
            #endregion

            #region Métodos

            /// <summary>
            /// Inicialzamos valores Fecha de Llegada
            /// </summary>
            private void inicializaValoresFechaLlegada()
        {
            //Inicializalizamos valores
            //Actualizamos Etiquetas de la ventana Modal
            lblTituloActualizacionLlegada.Text = " Llegada de la Parada.";
            lblMensajeLlegada.Text = "Al continuación iniciará la parada y terminará el movimiento.";
            txtFechaLlegada.Text = "";
            ddlRazonLlegadaTarde.SelectedValue = "0";
            lblErrorLlegada.Text = "";
        }

        /// <summary>
        /// Inicialzamos valores Fecha de Llegada
        /// </summary>
        private void inicializaValoresInsertaParada()
        {
            //Inicializalizamos valores
            ddlTipoParada.SelectedValue = "1";
            txtUbicacionP.Text = "";
            txtCita.Text = "";
            lblErrorInsertaParada.Text = "";
        }

        /// <summary>
        /// Inicialzamos valores Termino Servicio
        /// </summary>
        private void inicializaValoresTerminoServicio()
        {
            //Inicializalizamos valores
            txtFechaTermino.Text = "";
            lblErrorTerminoServicio.Text = "";
        }
        /// <summary>
        /// Habilitamos Controles Fecha de Llegada
        /// </summary>
        private void habilitaControlesFechaLlegada()
        {
            //Habilitamos controles para Ingresó de la Fecha de Llegada
            txtFechaLlegada.Enabled = true;
        }

        /// <summary>
        /// Inicialzamos valores Fecha de Salida
        /// </summary>
        private void inicializaValoresFechaSalida()
        {
            //Inicializalizamos valores
            txtFechaSalida.Text = "";
            lblErrorSalida.Text = "";
        }

        /// <summary>
        /// Habilitamos Controles Fecha de Salida
        /// </summary>
        private void habilitaControlesFechaSalida()
        {
            //Habilitamos controles para Ingresó de la Fecha de Llegada
            txtFechaSalida.Enabled = true;
        }

        /// <summary>
        /// Reversa la Fecha de Salida
        /// </summary>
        private void reversaFechaLlegada()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la parada actual
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Realizando la actualización de la parada
                resultado = objParada.ReversaParadaFechaLlegada(((Usuario)Session["usuario"]).id_usuario);
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Paradas
                cargaParadas();

            }
            //Mostramos Mensaje Error
            lblErrorParada.Text = resultado.Mensaje;
        }

        /// <summary>
        ///Deshabilita Paradas
        /// </summary>
        /// <returns></returns>
        private void deshabilitaParada()
        {
            //Establecemos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que exista elemento selecionado
            if (gvParadas.SelectedIndex != -1)
            {
                //Instanciamos Parada
                using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
                {
                    //Instanciando servicio seleccionado
                    using (Servicio srv = new Servicio(Convert.ToInt32(gvServicios.SelectedDataKey["Id"])))
                    {
                        //Si el servicio se encuentra documentado
                        if (srv.estatus == Servicio.Estatus.Documentado)
                            //Deshabilitando con reglas de documentación
                            resultado = objParada.DeshabilitaParadaServicio(((Usuario)Session["usuario"]).id_usuario, (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")).Rows.Count);
                        else
                            //Editamos Parada
                            resultado = objParada.DeshabilitaParadaDespacho(((Usuario)Session["usuario"]).id_usuario, (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")).Rows.Count);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        {
                            //Cargamos Paradas
                            cargaParadas();
                        }

                        //Mostramos Mensaje Error
                        lblErrorParada.Text = resultado.Mensaje;
                    }
                }
            }
        }

        /// <summary>
        /// Inserta Parad
        /// </summary>
        private RetornoOperacion insertaParada()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int k = gvParadas.DataKeys.Count;
            //Declaramos Cita
            DateTime CitaParada = DateTime.MinValue;
            //Si existe Cita
            if (txtCita.Text != "")
            {
                CitaParada = Convert.ToDateTime(txtCita.Text);
            }
            //Realizando la actualización de la parada
            resultado = Parada.NuevaParadaDespacho(Convert.ToInt32(gvServicios.SelectedValue), Convert.ToDecimal(gvParadas.SelectedDataKey["Secuencia"]) + 1,
                                                      (Parada.TipoParada)Convert.ToByte(ddlTipoParada.SelectedValue), Convert.ToInt32(ddlTipoEvento.SelectedValue),
                                                     Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionP.Text, "ID:", 1)),
                                                      SqlGeography.Null, CitaParada, 0, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                   (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")).Rows.Count, ((Usuario)Session["usuario"]).id_usuario);

            //Si no existe Error
            if (resultado.OperacionExitosa)
            {
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upbtnAceptarInsertaParada, upbtnAceptarInsertaParada.GetType(), "AbreVentanaModal", "contenidoInsertaParada", "confirmacionInsertaParada");

                //Cargamos Paradas
                cargaParadas();

                //Inicializamos Indices
                Controles.InicializaIndices(gvParadas);

            }
            //Mostramos Mensaje Error
            lblErrorInsertaParada.Text = resultado.Mensaje;
            //Devolcemos Resultado
            return resultado;
        }

        /// <summary>
        /// Reversa Fecha Salida
        /// </summary>
        private void reversaFechaSalida()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la parada actual
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Realizando la actualización de la parada
                resultado = objParada.ReversaParadaFechaSalida(((Usuario)Session["usuario"]).id_usuario);
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Paradas
                cargaParadas();

            }

            //Mostramos Mensaje Error
            lblErrorParada.Text = resultado.Mensaje;

        }

        /// <summary>
        ///  Método encargado de actualizar la Fecha de Salida de la Parada
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion actualizaFechaSalida()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la parada actual
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Realizando la actualización de la parada
                resultado = objParada.ActualizarFechaSalida(Convert.ToDateTime(txtFechaSalida.Text), Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionFin.Manual, 
                                            ParadaEvento.TipoActualizacionFin.Manual, Convert.ToByte(ddlRazonSalidaTarde.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Paradas
                cargaParadas();

                //Cargamos Servicios
                cargaServiciosParaDespacho();

                //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Salida
                inicializaValoresFechaSalida();
                habilitaControlesFechaSalida();

            }
            else
            {
                //Mostramos Mensaje Error
                lblErrorSalida.Text = resultado.Mensaje;
            }

            //Devolvemo Valor
            return resultado;
            /*
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Instanciando la parada actual
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Si la parada existe
                if (objParada.habilitar)
                {
                    //Obteniendo movimiento con origen en la parada de interés
                    using (Movimiento mov = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(Convert.ToInt32(gvServicios.SelectedDataKey.Value), objParada.id_parada)))
                        //Validando existencia de vencimientos para alguno de los recursos asignados
                        resultado = validaVencimientosActivosMovimiento(mov.id_movimiento);

                    //Si no hay vencimientos
                    if (resultado.OperacionExitosa)
                    {
                        //Realizando la actualización de la parada
                        resultado = objParada.ActualizarFechaSalida(Convert.ToDateTime(txtFechaSalida.Text), Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionFin.Manual, ParadaEvento.TipoActualizacionFin.Manual,
                                   ((Usuario)Session["usuario"]).id_usuario);

                        //Si no existe Error en la actualización de la salida
                        if (resultado.OperacionExitosa)
                        {
                            //Cargamos Paradas
                            cargaParadas();

                            //Cargamos Servicios
                            cargaServiciosParaDespacho();

                            //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Salida
                            inicializaValoresFechaSalida();
                            habilitaControlesFechaSalida();
                        }
                        //Si hay problemas, se indica el error
                        else
                            //Mostramos Mensaje Error
                            lblErrorSalida.Text = resultado.Mensaje;
                    }
                    //Si hay vencimientos
                    else                    
                    {
                        //Mostrando ventana de notificación de vencimientos
                        ScriptServer.AlternarVentana(this, this.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                    }
                }
            }            

            //Devolvemo Valor
            return resultado;*/
        }

        /// <summary>
        /// Método encargado de actualizar la Fecha de Llegada de la Parada
        /// </summary>
        private void actualizaFechaLlegada()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la parada actual
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Realizando la actualización de la parada
                resultado = objParada.ActualizarFechaLlegada(Convert.ToDateTime(txtFechaLlegada.Text), Parada.TipoActualizacionLlegada.Manual, Convert.ToByte(ddlRazonLlegadaTarde.SelectedValue),
                             EstanciaUnidad.TipoActualizacionInicio.Manual, ((Usuario)Session["usuario"]).id_usuario);
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
            {
                //Cargamos Paradas
                cargaParadas();

                //Cargamos Servicios
                cargaServiciosParaDespacho();

                //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Llegada
                inicializaValoresFechaLlegada();
                habilitaControlesFechaLlegada();

                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upbtnAceptarIngresoLlegada, upbtnAceptarIngresoLlegada.GetType(), "CierraVentanaModal", "contenidoConfirmacionActualizacionLlegada", "confirmacionActualizacionLlegada");

            }
            else
            {
                //Establecemos Mensaje Error
                lblErrorLlegada.Text = resultado.Mensaje;
            }
        }

        /// <summary>
        /// Carga Paradas
        /// </summary>
        private void cargaParadas()
        {
            //Obtenemos Paradas ligados al Id de Servicio
            using (DataTable mit = Parada.CargaParadasParaVisualizacionDespacho(Convert.ToInt32(gvServicios.SelectedDataKey.Value)))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvParadas, mit, "IdMovimiento-IdOrigen-IdUbicacion-Secuencia-Cita-FechaLlegada-IdDevolucion", "", true, 3);

                //Inicializando selección de paradas
                //Controles.InicializaIndices(gvParadas);

                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");
                }
                else
                {
                    //Eliminamos Tabla de la Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
            //Inicializamos los indicadores
            inicializaIndicadores();
        }

        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=700";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de referencias del registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencias", 800, 500, false, false, false, true, true, Page);
        }
        #endregion

        #endregion

        #region Bitacora Monitoreo

        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "RecursosBitacora":
                    {
                        //Cerrando ventana
                        alternaVentanaModal(lkb.CommandName, this);
                        break;
                    }
                case "HistorialBitacora":
                    {
                        //Cerrando ventana
                        alternaVentanaModal("historialBitacora", this);
                        //Mostrando ventana
                        alternaVentanaModal("RecursosBitacora", this);
                        break;
                    }
                case "Bitacora":
                    {
                        //Cerrando ventana
                        alternaVentanaModal("bitacoraMonitoreo", this);
                        //Mostrando ventana
                        alternaVentanaModal("RecursosBitacora", this);
                        break;
                    }
                case "DespachoMTC":
                    {
                        //Cerrando ventana
                        alternaVentanaModal("despachoMTC", this);
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosAsignados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando Tipo de Fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Obteniendo Control
                using (Image img = (Image)e.Row.FindControl("imgAsignacion"))
                {
                    //Validando que exista el Control
                    if (img != null)
                    {
                        //Validando Existencia
                        if (row["Asignacion"] != null)
                        {
                            //Validando Asignación
                            switch (row["Asignacion"].ToString())
                            {
                                case "Operador":
                                    //Asignando Imagen
                                    img.ImageUrl = "~/Image/operador2.png";
                                    break;
                                case "Tercero":
                                    //Asignando Imagen
                                    img.ImageUrl = "~/Image/transportista2.png";
                                    break;
                                default:
                                    //Asignando Imagen
                                    img.ImageUrl = "~/Image/unidad.png";
                                    break;
                            }
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
        protected void imbSeleccionar_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRecursosAsignados, sender, "imb", false);

                //Instanciando Movimiento Asignación Recurso
                using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"])))
                {
                    //Validando Registro
                    if (mar.habilitar)
                    {
                        //Obteniendo Entidad
                        int idTabla = mar.id_tipo_asignacion == 1 ? 19 : (mar.id_tipo_asignacion == 2 ? 76 : 25);

                        //Obteniendo Control
                        ImageButton imb = (ImageButton)sender;

                        //Validando Comando
                        switch (imb.CommandName)
                        {
                            case "Bitacora":
                                {
                                    //Inicializando control para nueva bitácora
                                    wucBitacoraMonitoreo.InicializaControl(0, Convert.ToInt32(gvServicios.SelectedDataKey["Id"]),
                                            Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), 0,
                                            Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), idTabla, mar.id_recurso_asignado);
                                    //Mostrando Ventana
                                    alternaVentanaModal("bitacoraMonitoreo", this.Page);
                                    //Ocultando Ventana
                                    alternaVentanaModal("RecursosBitacora", this.Page);
                                    break;
                                }
                            case "HistorialBitacora":
                                {
                                    //Recargando contenido de bitácora de monitoreo
                                    wucBitacoraMonitoreoHistorial.InicializaControl(idTabla, mar.id_recurso_asignado);
                                    //Mostrando Ventana
                                    alternaVentanaModal("historialBitacora", this.Page);
                                    //Ocultando Ventana
                                    alternaVentanaModal("RecursosBitacora", this.Page);
                                    break;
                                }
                        }
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Recurso"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        /// <summary>
        /// Nuevo asiento enbitácora de monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoHistorial_btnNuevoBitacora(object sender, EventArgs e)
        {
            //Instanciando Movimiento Asignación Recurso
            using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"])))
            {
                //Validando Registro
                if (mar.habilitar)
                {
                    //Obteniendo Entidad
                    int idTabla = mar.id_tipo_asignacion == 1 ? 19 : (mar.id_tipo_asignacion == 2 ? 76 : 25);

                    //Cerrando ventana de historial de bitácora
                    alternaVentanaModal("historialBitacora", this);

                    //Inicializando control para nueva bitácora
                    wucBitacoraMonitoreo.InicializaControl(0, Convert.ToInt32(gvServicios.SelectedDataKey["Id"]),
                            Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), 0,
                            Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), idTabla, mar.id_recurso_asignado);
                    
                    //Mostrando ventana de control bitácora
                    alternaVentanaModal("bitacoraMonitoreo", this);
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar el Recurso"), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
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
                //Instanciando Movimiento Asignación Recurso
                using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"])))
                {
                    //Validando Registro
                    if (mar.habilitar)
                    {
                        //Obteniendo Entidad
                        int idTabla = mar.id_tipo_asignacion == 1 ? 19 : (mar.id_tipo_asignacion == 2 ? 76 : 25);

                        //Recargando contenido de bitácora de monitoreo
                        wucBitacoraMonitoreoHistorial.InicializaControl(idTabla, mar.id_recurso_asignado);
                        //Ocultando ventana actual
                        alternaVentanaModal("bitacoraMonitoreo", this);
                        //Mostrando ventana de historial
                        alternaVentanaModal("historialBitacora", this);
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se puede recuperar el Recurso");
                }
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
                //Instanciando Movimiento Asignación Recurso
                using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedDataKey["Id"])))
                {
                    //Validando Registro
                    if (mar.habilitar)
                    {
                        //Obteniendo Entidad
                        int idTabla = mar.id_tipo_asignacion == 1 ? 19 : (mar.id_tipo_asignacion == 2 ? 76 : 25);

                        //Recargando contenido de bitácora de monitoreo
                        wucBitacoraMonitoreoHistorial.InicializaControl(idTabla, mar.id_recurso_asignado);
                        //Ocultando ventana actual
                        alternaVentanaModal("bitacoraMonitoreo", this);
                        //Mostrando ventana de historial
                        alternaVentanaModal("historialBitacora", this);
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se puede recuperar el Recurso");
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        private void cargaRecursosMovimiento()
        {
            //Obteniendo los Terceros
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"])))
            {
                //Validando Datos
                if (Validacion.ValidaOrigenDatos(dt))
                {
                    //Cargando Valores
                    Controles.CargaGridView(gvRecursosAsignados, dt, "Id", "", true, 1);
                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table4");
                }
                else
                {
                    //Inicializando Valores
                    Controles.InicializaGridview(gvRecursosAsignados);
                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                }
            }
        }

        #endregion

        #endregion

        #region Recursos

        #region Eventos

        /// <summary>
        /// Eventó Generado al Terminar un Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTerminoServicio_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaTerminoServicio, lkbCerrarFechaTerminoServicio.GetType(), "CierreVentanaModal", "contenidoFechaTerminoServicio", "confirmacionFechaTerminoServicio");
        }

        /// <summary>
        /// Evento Producido al Terminar el Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTerminoServicio_Click(object sender, EventArgs e)
        {
            //Terminamos Servicio
            realizaProcesoTerminoServicio();
            //terminarServicio();
        }

        #endregion

        #endregion

        #region Administración de Eventos

        #region Eventos

        /// <summary>
        /// Evento producido al pulsar el link  Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEvento_Click(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;

            //Si existen registros
            if (gvEventos.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvEventos, sender, "lnk", false);

                //En base al comando definido para el botón
                switch (b.CommandName)
                {
                    case "Seleccionar":
                        {
                            //Instanciamos Evento
                            using (ParadaEvento objParadaEvento = new ParadaEvento(Convert.ToInt32(gvEventos.SelectedDataKey.Value)))
                            {
                                //Inicializamos controles del evento
                                ddlTipoEventos.SelectedValue = objParadaEvento.id_tipo_evento.ToString();
                                txtFechaInicioEvento.Text = objParadaEvento.inicio_evento == DateTime.MinValue ? "" : objParadaEvento.inicio_evento.ToString("dd/MM/yyyy HH:mm");
                                txtFechaFinEvento.Text = objParadaEvento.fin_evento == DateTime.MinValue ? "" : objParadaEvento.fin_evento.ToString("dd/MM/yyyy HH:mm");
                                ddlMotivoRetraso.SelectedValue = objParadaEvento.id_motivo_retraso_evento.ToString();

                            }
                            //Limpiamos Etiqueta Error
                            lblErrorEventos.Text = "";
                            break;
                        }
                    case "Eliminar":
                        {
                            //Deshabilitamos Evento
                            deshabilitaParadaEvento(Convert.ToInt32(gvEventos.SelectedValue), (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")).Rows.Count);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Evento Generado al dar click en Aceptar Fecha Llegada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarIngresoLlegada_Click(object sender, EventArgs e)
        {
            //Actualizamos Fecha Llegada
            actualizaFechaLlegada();

        }

        /// <summary>
        /// Evento Generado al Modificar el Eveto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModificarEvento_Click(object sender, EventArgs e)
        {
            //Validamos Seleccoón de Evento
            if (gvEventos.SelectedIndex != -1)
            {
                //Modificamos Evento
                editamosEventoFechas();
            }
            else
            {
                //Mostramos Mensaje
                lblErrorEventos.Text = "Seleccione el Evento";
            }
        }

        /// <summary>
        /// Evento Generado al Insertar el Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInsertarEvento_Click(object sender, EventArgs e)
        {
            //Insertamos Eventos
            insertaEvento(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), gvEventos.DataKeys.Count);

        }
        /// <summary>
        /// Evento Generado al dar click cuando no se desea Actualizar los Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoActualizacionEventos_Click(object sender, EventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando Comando
            Button btnNo = (Button)sender;

            //Validando Comando
            switch (btnNo.CommandName)
            {
                default:
                case "Parada":
                    {
                        //Instanciando la parada actual
                        using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
                        {
                            //Obteniendo movimiento con origen en la parada de interés
                            using (Movimiento mov = new Movimiento(Movimiento.BuscamosMovimientoParadaOrigen(Convert.ToInt32(gvServicios.SelectedDataKey.Value), objParada.id_parada)))
                                //Validando existencia de vencimientos para alguno de los recursos asignados
                                resultado = validaVencimientosActivosMovimiento(mov.id_movimiento);

                            //Si no hay vencimientos
                            if (resultado.OperacionExitosa)
                            {
                                //Actualizando salida
                                resultado = actualizaFechaSalida();

                                //Cerrando ventana de confirmación de término de eventos
                                ScriptServer.AlternarVentana(upbtnNoActualizacionEventos, upbtnNoActualizacionEventos.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");
                                //Si hay errores de actualización
                                if (!resultado.OperacionExitosa)
                                    //Abriendo ventana de confirmación de salida
                                    ScriptServer.AlternarVentana(upbtnNoActualizacionEventos, upbtnNoActualizacionEventos.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
                            }
                            //Si existen vencimiento
                            else
                            {
                                //Cerrando ventana de confirmación de hora de salida
                                ScriptServer.AlternarVentana(upbtnNoActualizacionEventos, upbtnNoActualizacionEventos.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");
                                //Mostrando ventana de notificación de vencimientos
                                ScriptServer.AlternarVentana(upbtnNoActualizacionEventos, btnAceptarIngresoSalida.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                            }
                        }
                        break;
                    }
                case "Servicio":
                    {
                        //Terminando Servicio
                        terminarServicio();
                        //Cerrando ventana de confirmación de término de eventos
                        ScriptServer.AlternarVentana(upbtnNoActualizacionEventos, upbtnNoActualizacionEventos.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");
                        break;
                    }
            }
        }

        /// <summary>
        /// Evento Generado al dar click al botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarActualizacionEventos_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCancelarActualizacionEventos, upbtnCancelarActualizacionEventos.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");

            //Inicializamos Valores de Actualización de Fecha de Salida
            inicializaValoresFechaSalida();
            habilitaControlesFechaSalida();
        }

        /// <summary>
        /// Evento Generado al dar click al No Actualizar los Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSiActualizacionEventos_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnSiActualizacionEventos, upbtnSiActualizacionEventos.GetType(), "CierreVentanaModal", "contenidoConfirmacionActualizacionEventos", "confirmacionConfirmacionActualizacionEventos");

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnSiActualizacionEventos, upbtnSiActualizacionEventos.GetType(), "AbreVentanaModal", "contenidoActualizacionEventos", "actualizacionEventos");

            //Validando Comando
            switch (((Button)sender).CommandName)
            {
                default:
                case "Parada":
                    {
                        //Instanciamos Parad
                        using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
                        {
                            //Validamos exista Parada
                            if (objParada.habilitar)
                            {
                                //Cargamos Eventos
                                cargaEventos(objParada.id_parada);
                                //Cargando Catalogos 
                                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventos, 40, "", objParada.id_tipo_parada, "", 0, "");
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la parada."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
                case "Servicio":
                    {
                        //Instanciamos Parad
                        using (Parada objParada = new Parada(Parada.ObtieneUltimaParada(Convert.ToInt32(gvServicios.SelectedValue))))
                        {
                            //Validamos exista Parada
                            if (objParada.habilitar)
                            {
                                //Cargamos Eventos
                                cargaEventos(objParada.id_parada);
                                //Cargando Catalogos 
                                CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventos, 40, "", objParada.id_tipo_parada, "", 0, "");
                            }
                            else
                                //Instanciando Excepción
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la última parada."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        break;
                    }
            }
        }

        /// Evento Generado al dar click Cancelar Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEvento_Click(object sender, EventArgs e)
        {
            //Inicializamos Controles
            inicializaValoresEventos();
        }

        /// <summary>
        /// Evento Generado al dar click Cerrar Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarEventos_Click(object sender, EventArgs e)
        {
            //Inicializamos Valores
            inicializaValoresEventos();

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCerrarEvento, upbtnCerrarEvento.GetType(), "CerrarVentanaModal", "contenidoActualizacionEventos", "actualizacionEventos");

            //Obteniendo control
            Button btnCerrar = (Button)sender;

            //Validando Comando
            switch (btnCerrar.CommandName)
            {
                default:
                case "Parada":
                    {
                        //Validamos si la ventana se mostro a partir de la actualización de Fecha de Salida
                        if (txtFechaSalida.Text != "")

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(upbtnCerrarEvento, upbtnCerrarEvento.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
                        break;
                    }
                case "Servicio":
                    {
                        //Validamos si la ventana se mostro a partir de la actualización de Fecha de Termino
                        if (txtFechaTermino.Text != "")

                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(upbtnCerrarEvento, upbtnCerrarEvento.GetType(), "CierreVentanaModal", "contenidoFechaTerminoServicio", "confirmacionFechaTerminoServicio");
                        break;
                    }
            }

            //Cargamos paradas
            cargaParadas();
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Eventos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, false, 1);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Eventos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEventos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"),
                                        Convert.ToInt32(ddlTamanoEventos.SelectedValue), false, 1);
        }

        /// Evento Producido al pulsar el botón "Exportar Eventos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarEventos_OnClick(object sender, EventArgs e)
        {
            //Exportando eventos
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }
        /// <summary>
        /// Evento disparado al dar click al link button cerrar cuya funcion es la de cerrar la ventana modal que muestra los datos de la ubicacion deseada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Actualziando paradas
            cargaParadas();
            //Generamos Scriptd
            ScriptServer.AlternarVentana(uplnkCerrar, uplnkCerrar.GetType(), "CierreVentanaModal", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa Valores Eventos
        /// </summary>
        private void inicializaValoresEventos()
        {
            //Inicializamos Valores
            txtFechaInicioEvento.Text = "";
            txtFechaFinEvento.Text = "";
            ddlMotivoRetraso.SelectedValue = "0";
            lblErrorEventos.Text = "";

            //Inicializamos Indices Grid View Eventos
            TSDK.ASP.Controles.InicializaIndices(gvEventos);
        }
        /// <summary>
        /// Método encargado de cargar los Eventos de la parada
        /// </summary>
        /// <param name="id_parada"></param>
        private void cargaEventos(int id_parada)
        {
            //Obtenemos Paradas
            using (DataTable mit = ParadaEvento.CargaEventosParaActualizacionFechas(id_parada))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvEventos, mit, "IdEvento", "", false, 0);

                //Valida Origen de Datos
                if (mit != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");
                }
                else
                {
                    //Eliminamos Tabla a DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
            //Inicializamos Indices
            Controles.InicializaIndices(gvEventos);
        }


        /// <summary>
        /// Deshabilita Evento
        /// </summary>
        /// <param name="Id_evento">Id Eventos</param>
        /// <param name="TotalEventos">Total de Eventos actuales en el Grid View</param>
        private void deshabilitaParadaEvento(int Id_evento, int TotalEventos)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Evento
            using (ParadaEvento objParadaEvento = new ParadaEvento(Id_evento))
            {
                //Editamos Parada
                resultado = objParadaEvento.DeshabilitaParadaEventoEnDespacho(((Usuario)Session["usuario"]).id_usuario, TotalEventos);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Evento
                    cargaEventos(objParadaEvento.id_parada);
                }
            }
            //Mostramos Mensaje Error
            lblErrorEventos.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Insertamos Eventos
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="totalEventos">Total Eventos</param>
        private void insertaEvento(int id_parada, int totalEventos)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //declaramos variable para almacenar total de eventos
            int total_eventos = 0;

            //Validamos Origen de Datos
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))
            {

                //Obtenemos total de eventos actuales
                total_eventos = (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")).Rows.Count;
            }

            //Editamos Evento
            resultado = ParadaEvento.InsertaParadaEventoEnDespacho(Convert.ToInt32(gvServicios.SelectedDataKey.Value), id_parada,
                       total_eventos, ((Usuario)Session["usuario"]).id_usuario);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Eventos
                cargaEventos(id_parada);

                //Inicializamos Controles
                inicializaValoresEventos();
            }
            //Mostramos Resultado
            lblErrorEventos.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Editamos las Fechas de los eventos
        /// </summary>
        //private void editamosEventoFechas()
        //{
        //    //Declaramos Mensaje
        //    RetornoOperacion resultado = new RetornoOperacion();

        //    //Decharamos Fechas
        //    DateTime inicioEvento = DateTime.MinValue;
        //    DateTime finEvento = DateTime.MinValue;

        //    //Validamos asignación de Fechas
        //    if (txtFechaInicioEvento.Text != "")
        //    {
        //        //Asignamos valor
        //        inicioEvento = Convert.ToDateTime(txtFechaInicioEvento.Text);
        //    }
        //    if (txtFechaFinEvento.Text != "")
        //    {
        //        //Asignamos valor
        //        finEvento = Convert.ToDateTime(txtFechaFinEvento.Text);
        //    }
        //    //Instanciamos Evento
        //    //Creamos la transacción 
        //    using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
        //    {
        //        using (ParadaEvento objEvento = new ParadaEvento(Convert.ToInt32(gvEventos.SelectedDataKey.Value)))
        //        {
        //            //Editamos Evento
        //            resultado = objEvento.EditaParadaEventoEnDespacho(Convert.ToByte(ddlTipoEventos.SelectedValue), inicioEvento, ParadaEvento.TipoActualizacionInicio.Manual,
        //                                                         finEvento, ParadaEvento.TipoActualizacionFin.Manual,
        //                                                         Convert.ToByte(ddlMotivoRetraso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);

        //            //Validamos Resultado
        //            if (resultado.OperacionExitosa)
        //            {

        //                //Cargamos Eventos
        //                cargaEventos(objEvento.id_parada);

        //                //Inicializamos Controles
        //                inicializaValoresEventos();
        //            }
        //            ////Validamos que se realiza la operaciones anteriores para poder mandar los datos WEBSERVICE
        //            if (resultado.OperacionExitosa)
        //            {
        //                //metodo para verificar si cuenta con el proveedor 
        //                resultado = SAT_CL.Monitoreo.ProveedorWSUnidad.GenerarDespachoMTC(Convert.ToInt32(gvServicios.SelectedDataKey["Id"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), 2, Convert.ToInt32(objEvento.secuencia_evento_parada));
        //                //resultado = new RetornoOperacion(true);
        //            }
        //            //Mostramos Mensaje
        //            lblErrorEventos.Text = resultado.Mensaje;
        //        }
        //        //Validamos Resultado
        //        if (resultado.OperacionExitosa)
        //            //Completamos transación
        //            scope.Complete();
        //    }
        //}
        /// <summary>
        /// Editamos las Fechas de los eventos
        /// </summary>
        private void editamosEventoFechas()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Decharamos Fechas
            DateTime inicioEvento = DateTime.MinValue;
            DateTime finEvento = DateTime.MinValue;

            //Validamos asignación de Fechas
            if (txtFechaInicioEvento.Text != "")
            {
                //Asignamos valor
                inicioEvento = Convert.ToDateTime(txtFechaInicioEvento.Text);
            }
            if (txtFechaFinEvento.Text != "")
            {
                //Asignamos valor
                finEvento = Convert.ToDateTime(txtFechaFinEvento.Text);
            }
            //Instanciamos Evento
            using (ParadaEvento objEvento = new ParadaEvento(Convert.ToInt32(gvEventos.SelectedDataKey.Value)))
            {
                //Editamos Evento
                resultado = objEvento.EditaParadaEventoEnDespacho(Convert.ToByte(ddlTipoEventos.SelectedValue), inicioEvento, ParadaEvento.TipoActualizacionInicio.Manual,
                                                             finEvento, ParadaEvento.TipoActualizacionFin.Manual,
                                                             Convert.ToByte(ddlMotivoRetraso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Eventos
                    cargaEventos(objEvento.id_parada);

                    //Inicializamos Controles
                    inicializaValoresEventos();
                }
                //Mostramos Mensaje
                lblErrorEventos.Text = resultado.Mensaje;
            }
        }

        #endregion

        #endregion

        #region Controles de Usuario Documentación

        /// <summary>
        /// Maneja el evento producido durante la pulsación del botón guardar de Control de Usuario wucProducto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucProducto_ClickGuardarProducto(object sender, EventArgs e)
        {
            //Guardando producto
            wucProducto.GuardaProductoServicio();
        }
        /// <summary>
        /// Maneja el evento producido durante la pulsación de algún botón eliminar de Control de Usuario wucProducto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucProducto_ClickEliminarProducto(object sender, EventArgs e)
        {
            //Guardando producto
            wucProducto.EliminaProductoServicio();
        }
        /// <summary>
        /// Maneja el evento disparado por el botón guardar del control de usuario Clasificación
        /// </summary>
        protected void wucClasificacion_ClickGuardar(object sender, EventArgs e)
        {
            //Guardando contenido de control
            wucClasificacion.GuardaCambiosClasificacionServicio();
        }
        /// <summary>
        /// Maneja el evento disparado por el botón cancelar del control de usuario Clasificación
        /// </summary>
        protected void wucClasificacion_ClickCancelar(object sender, EventArgs e)
        {
            wucClasificacion.CancelaCambiosClasificacion();
        }

        /// <summary>
        /// Evento Producido al Guardar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Guardando Referencia
            resultado = wucReferenciaViaje.GuardaReferenciaViaje();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                cargaServiciosParaDespacho();

                //Inicializamos Indices de la Parada
                Controles.InicializaGridview(gvParadas);
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Eliminando Referencia
            resultado = wucReferenciaViaje.EliminaReferenciaViaje();
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                cargaServiciosParaDespacho();

                //Inicializamos Indices de la Parada
                Controles.InicializaGridview(gvParadas);
            }
        }

        /// <summary>
        /// Maneja el evento click sobre el botón guardar del control facturado concepto
        /// </summary>
        protected void wucFacturadoConcepto_ClickGuardarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualización
            resultado = wucFacturadoConcepto.GuardarFacturaConcepto();

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        /// <summary>
        /// Maneja el evento click sobre el botón eliminar del control facturado concepto
        /// </summary>
        protected void wucFacturadoConcepto_ClickEliminarFacturaConcepto(object sender, EventArgs e)
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando actualización
            resultado = wucFacturadoConcepto.EliminaFacturaConcepto();

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionRecurso_ClickAgregarRecurso(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando Vencimientos
            result = validaVencimientosActivosRecurso();

            //Validando que la Operación fuese Correcta
            if (result.OperacionExitosa)
            {
                //Invocando Método de Agregación de Recurso
                result = ucAsignacionRecurso.AgregaAsignacionRecurso();

                //Validando que la Operación fuese Exitosa
                if (result.OperacionExitosa)
                {
                    //Cargamos Recursos Disponibles -IdOrigen
                    ucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                    //Cargando Paradas
                    cargaParadas();
                }
            }
            //Si hay vencimientos
            else
            {
                //Mostrando ventana modal de notificación de vencimientos
                ScriptServer.AlternarVentana(this, this.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");

                //Mostrando ventana modal de notificación de vencimientos
                ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentanaModal", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionRecurso_ClickQuitarRecurso(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Invocando Método de Deshabilitación de Recurso
            result = ucAsignacionRecurso.DeshabilitaRecurso();

            //Validando que la Operación fuese Correcta
            if (result.OperacionExitosa)
            {
                //Cargamos Recursos Disponibles -IdOrigen
                ucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Cargando Paradas
                cargaParadas();
            }
        }
        /// <summary>
        /// Evento generado al Liberar el Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionRecurso_ClickLiberarRecurso(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Invocando Método de Liberación de Recurso
            result = ucAsignacionRecurso.LiberarRecurso();

            //Validando qresultado
            if (result.OperacionExitosa)
            {
                //Inicializa Control de Usuario
                ucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                //Cargando Paradas
                cargaParadas();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionRecurso_ClickReubicarRecurso(object sender, EventArgs e)
        {
            //Inicializamos Control Movimiento en vacio
            if (wucReubicacion.InicializaControl(ucAsignacionRecurso.idRecurso, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                  Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionInicio.Manual, EstanciaUnidad.TipoActualizacionFin.Manual).OperacionExitosa)
            {
                //Mostramos Ventana Modal Reubicación
                ScriptServer.AlternarVentana(this, this.GetType(), "AbreVentana", "contenidoConfirmacionUbicacion", "confirmacionUbicacion");

                //Ocultamoss Ventana Modal Asignación Recursos
                ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentana", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
            }
        }
        /// <summary>
        /// Eventó Generado al Registrar un  Movimiento en Vacio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReubicacion_OnClickRegistrar(object sender, EventArgs e)
        {
            //Validando vencimientos de recursos del movimiento vacío
            RetornoOperacion resultado = validaVencimientosActivosMovVacio();

            //Si no hay vencimientos
            if (resultado.OperacionExitosa)
            {
                //Registramos un Movimiento en Vacio
                resultado = wucReubicacion.RegistraMovimientoVacioSinOrden();

                //Si no existe Error
                if (resultado.OperacionExitosa)
                {
                    //Cerrando Ventana Modal de reubicación
                    ScriptServer.AlternarVentana(this, this.GetType(), "CierraVentana", "contenidoConfirmacionUbicacion", "confirmacionUbicacion");

                    //Mostrando Ventana Modal Asignación Recursos
                    ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentana", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");

                    //Cargamos Recursos Disponibles -IdOrigen
                    ucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                }
            }
            //Si existen vencimientos
            else
            {
                //Cerrando ventana de movimiento vacío (reubicación)
                ScriptServer.AlternarVentana(this, this.GetType(), "CierraVentana", "contenidoConfirmacionUbicacion", "confirmacionUbicacion");
                //Mostrando ventana de notificación de vencimientos
                ScriptServer.AlternarVentana(this, this.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
            }
        }

        /// <summary>
        /// Eventó Generado al Cancelar la Reubicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReubicacion_OnClickCancelar(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "CierraVentana", "contenidoConfirmacionUbicacion", "confirmacionUbicacion");

            //Ocultamoss Ventana Modal Asignación Recursos
            ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentana", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
        }

        #endregion

        #region Vencimientos

        /// <summary>
        /// Determina si el recurso tiene vencimientos activos y notifica al usuario de ello
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaVencimientosActivosRecurso()
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Determinando el tipo de aplicación de vencimientos a buscar
            TipoVencimiento.TipoAplicacion tipo_aplicacion = (TipoVencimiento.TipoAplicacion)ucAsignacionRecurso.idTipoAsignacion;

            //Declarando tabla concentradora de vencimientos
            DataTable mitVencimientosAsociados = new DataTable();
            //Si es unidad u operador, se obtienen vencimientos de recurso asociado
            switch (tipo_aplicacion)
            {
                case TipoVencimiento.TipoAplicacion.Unidad:
                    //Obteniendo al operador asociado
                    int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(ucAsignacionRecurso.idRecurso);
                    //Si existe un operador
                    if (id_operador > 0)
                        //Obteniendo vencimientos
                        using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Operador, id_operador, Fecha.ObtieneFechaEstandarMexicoCentro()))
                        {
                            //Si existen vencimientos
                            if (mit != null)
                                //Copiando a tabla concentradora
                                mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
                        }
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    //Obteniendo la unidad asociada
                    int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(ucAsignacionRecurso.idRecurso);
                    //Si existe una unidad
                    if (id_unidad > 0)
                        //Obteniendo vencimientos
                        using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, id_unidad, Fecha.ObtieneFechaEstandarMexicoCentro()))
                        {
                            //Si existen vencimientos
                            if (mit != null)
                                //Copiando a tabla concentradora
                                mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
                        }
                    break;
            }

            //Validando existencia de vencimientos del recurso
            using (DataTable mitVencimientos = Vencimiento.CargaVencimientosActivosRecurso(tipo_aplicacion, ucAsignacionRecurso.idRecurso, Fecha.ObtieneFechaEstandarMexicoCentro()))
            {
                //Si hay vencimientos del recurso principal que mostrar
                if (mitVencimientos != null)
                    //Añadiendo contenido a tabla concentradora
                    mitVencimientosAsociados.Merge(mitVencimientos, true, MissingSchemaAction.Add);

                //Si hay recursos en el concentrado
                if (mitVencimientosAsociados.Rows.Count > 0)
                {
                    //Guardando origen de datos
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitVencimientosAsociados, "Table6");

                    //Determinando si hay vencimientos obligatorios
                    int obligatorios = (from DataRow r in mitVencimientosAsociados.Rows
                                        where r.Field<byte>("IdPrioridad") == Convert.ToByte(TipoVencimiento.Prioridad.Obligatorio)
                                        select r).Count();
                    if (obligatorios > 0)
                    {
                        //Indicando error
                        resultado = new RetornoOperacion("Existen vencimientos 'Obligatorios', debe terminarlos e intentar nuevamente.");
                        //Actualizando icono de alerta
                        imgAlertaVencimiento.ImageUrl = "~/Image/ExclamacionRoja.png";
                        //Actualizando mensaje 
                        lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                        //Actualizando comando 
                        btnAceptarIndicadorVencimientos.CommandName = "Obligatorio";
                    }
                    else
                    {
                        //Indicando error
                        resultado = new RetornoOperacion("Existen vencimientos 'Opcionales', de clic 'Aceptar' para Continuar.");
                        //Actualizando icono de alerta
                        imgAlertaVencimiento.ImageUrl = "~/Image/Exclamacion.png";
                        //Actualizando mensaje 
                        lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                        //Actualizando comando 
                        btnAceptarIndicadorVencimientos.CommandName = "Opcional";
                    }

                    //Indicando nivel de validación de vencimientos
                    btnAceptarIndicadorVencimientos.CommandArgument = "Recurso";

                    //Actualizando paneles de actualización necesarios
                    upimgAlertaVencimiento.Update();
                    uplblMensajeHistorialVencimientos.Update();
                    upbtnAceptarIndicadorVencimientos.Update();
                }
                else
                    //Eliminando de origen de datos
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");

                //Cargando GridView de Vencimientos
                TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientosAsociados.Rows.Count == 0 ? null : mitVencimientosAsociados, "Id", "", true, 1);
                upgvVencimientos.Update();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Determina si el conjunto de recursos asignados a un movimiento vacio tiene vencimientos que no permitan dicho movimiento (control de usuario)
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaVencimientosActivosMovVacio()
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declarando tabla concentradora de vencimientos
            DataTable mitVencimientosAsociados = new DataTable();

            //Validando unidad motriz
            using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_unidad, wucReubicacion.fecha_inicio))
            {
                //Si hay vencimientos
                if (mit != null)
                    //Añadiendo vencimientos a tabla principal
                    mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
            }
            //Obteniendo operador asociado
            int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(wucReubicacion.id_unidad);
            //Si hay operador asignado
            if (id_operador > 0)
            {
                //Validando operador
                using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Operador, id_operador, wucReubicacion.fecha_inicio))
                {
                    //Si hay vencimientos
                    if (mit != null)
                        //Añadiendo vencimientos a tabla principal
                        mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
                }
            }

            //Validando arrastre 1
            using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_remolque1, wucReubicacion.fecha_inicio))
            {
                //Si hay vencimientos
                if (mit != null)
                    //Añadiendo vencimientos a tabla principal
                    mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
            }

            //Validando arrastre 2
            using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_remolque2, wucReubicacion.fecha_inicio))
            {
                //Si hay vencimientos
                if (mit != null)
                    //Añadiendo vencimientos a tabla principal
                    mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
            }

            //Validando dolly
            using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_dolly, wucReubicacion.fecha_inicio))
            {
                //Si hay vencimientos
                if (mit != null)
                    //Añadiendo vencimientos a tabla principal
                    mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
            }

            //Si hay recursos en el concentrado
            if (mitVencimientosAsociados.Rows.Count > 0)
            {
                //Guardando origen de datos
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitVencimientosAsociados, "Table6");

                //Determinando si hay vencimientos obligatorios
                int obligatorios = (from DataRow r in mitVencimientosAsociados.Rows
                                    where r.Field<byte>("IdPrioridad") == Convert.ToByte(TipoVencimiento.Prioridad.Obligatorio)
                                    select r).Count();
                if (obligatorios > 0)
                {
                    //Indicando error
                    resultado = new RetornoOperacion("Existen vencimientos 'Obligatorios', debe terminarlos e intentar nuevamente.");
                    //Actualizando icono de alerta
                    imgAlertaVencimiento.ImageUrl = "~/Image/ExclamacionRoja.png";
                    //Actualizando mensaje 
                    lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                    //Actualizando comando 
                    btnAceptarIndicadorVencimientos.CommandName = "Obligatorio";
                }
                else
                {
                    //Indicando error
                    resultado = new RetornoOperacion("Existen vencimientos 'Opcionales', de clic 'Aceptar' para Continuar.");
                    //Actualizando icono de alerta
                    imgAlertaVencimiento.ImageUrl = "~/Image/Exclamacion.png";
                    //Actualizando mensaje 
                    lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                    //Actualizando comando 
                    btnAceptarIndicadorVencimientos.CommandName = "Opcional";
                }

                //Indicando nivel de validación de vencimientos
                btnAceptarIndicadorVencimientos.CommandArgument = "MovVacio";

                //Actualizando paneles de actualización necesarios
                upimgAlertaVencimiento.Update();
                uplblMensajeHistorialVencimientos.Update();
                upbtnAceptarIndicadorVencimientos.Update();
            }
            else
                //Eliminando de origen de datos
                OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");

            //Cargando GridView de Vencimientos
            TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientosAsociados.Rows.Count == 0 ? null : mitVencimientosAsociados, "Id", "", true, 1);
            upgvVencimientos.Update();

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Determina si algún recurso asignado al movimiento tiene vencimientos activos y notifica al usuario de ello
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento a validar</param>
        /// <returns></returns>
        private RetornoOperacion validaVencimientosActivosMovimiento(int id_movimiento)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(id_movimiento);

            //Validando existencia de vencimientos del recurso
            using (DataTable mitVencimientos = Vencimiento.CargaVencimientosActivosMovimiento(id_movimiento, Fecha.ObtieneFechaEstandarMexicoCentro()))
            {
                //Si hay recursos en el concentrado
                if (mitVencimientos != null)
                {
                    //Guardando origen de datos
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitVencimientos, "Table6");

                    //Determinando si hay vencimientos obligatorios
                    int obligatorios = (from DataRow r in mitVencimientos.Rows
                                        where r.Field<byte>("IdPrioridad") == Convert.ToByte(TipoVencimiento.Prioridad.Obligatorio)
                                        select r).Count();
                    if (obligatorios > 0)
                    {
                        //Indicando error
                        resultado = new RetornoOperacion("Existen vencimientos 'Obligatorios', debe terminarlos e intentar nuevamente.");
                        //Actualizando icono de alerta
                        imgAlertaVencimiento.ImageUrl = "~/Image/ExclamacionRoja.png";
                        //Actualizando mensaje 
                        lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                        //Actualizando comando 
                        btnAceptarIndicadorVencimientos.CommandName = "Obligatorio";
                    }
                    else
                    {
                        //Indicando error
                        resultado = new RetornoOperacion("Existen vencimientos 'Opcionales', dar clic en 'Aceptar' para Continuar.");
                        //Actualizando icono de alerta
                        imgAlertaVencimiento.ImageUrl = "~/Image/Exclamacion.png";
                        //Actualizando mensaje 
                        lblMensajeHistorialVencimientos.Text = resultado.Mensaje;
                        //Actualizando comando 
                        btnAceptarIndicadorVencimientos.CommandName = "Opcional";
                    }

                    //Indicando nivel de validación de vencimientos
                    btnAceptarIndicadorVencimientos.CommandArgument = "Movimiento";

                    //Actualizando paneles de actualización necesarios
                    upimgAlertaVencimiento.Update();
                    uplblMensajeHistorialVencimientos.Update();
                    upbtnAceptarIndicadorVencimientos.Update();
                }
                else
                    //Eliminando de origen de datos
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");

                //Cargando GridView de Vencimientos
                TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientos, "Id", "", true, 1);
                upgvVencimientos.Update();
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Evento Click del botón Aceptar Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarIndicadorVencimientos_Click(object sender, EventArgs e)
        {
            //Determinando el nivel al que se está aplicando la consulta de vencimientos activos
            switch (btnAceptarIndicadorVencimientos.CommandArgument)
            {
                case "Recurso":
                    {
                        //Determinando el comando a ejecutar
                        switch (((Button)sender).CommandName)
                        {
                            case "Obligatorio":
                                //Asignando resultado
                                //lblMensajeHistorialVencimientos.Text;

                                break;
                            case "Opcional":
                                //Agregamos el Recurso
                                RetornoOperacion resultado = ucAsignacionRecurso.AgregaAsignacionRecurso();

                                break;
                        }

                        //Ocultando ventana de notificación de vencimientos
                        ScriptServer.AlternarVentana(btnAceptarIndicadorVencimientos, btnAceptarIndicadorVencimientos.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                        //Mostramos Ventana Modal de recursos asignados
                        ScriptServer.AlternarVentana(btnAceptarIndicadorVencimientos, btnAceptarIndicadorVencimientos.GetType(), "AbreVentanaModal", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");

                        break;
                    }
                case "MovVacio":
                    {
                        //Determinando el comando a ejecutar
                        switch (((Button)sender).CommandName)
                        {
                            case "Obligatorio":
                                //Asignando resultado
                                //lblMensajeAsignacion.Text = lblMensajeHistorialVencimientos.Text;

                                break;
                            case "Opcional":
                                //Registramos un Movimiento en Vacio
                                RetornoOperacion resultado = wucReubicacion.RegistraMovimientoVacioSinOrden();

                                //Si no existe Error
                                if (resultado.OperacionExitosa)
                                {
                                    //Mostrando Ventana Modal Asignación Recursos
                                    ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentana", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
                                }
                                //Si no se pudo guardar el movimiento, se actualiza panel para visualizar errores 
                                else
                                {
                                    //Actualizando mensajes de error
                                    upwucReubicacion.Update();
                                    //Mostrando ventana de movimiento
                                    ScriptServer.AlternarVentana(this, this.GetType(), "CierraVentana", "contenidoConfirmacionUbicacion", "confirmacionUbicacion");
                                }
                                break;
                        }
                        //Ocultando ventana de notificación de vencimientos
                        ScriptServer.AlternarVentana(btnAceptarIndicadorVencimientos, btnAceptarIndicadorVencimientos.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                        //Ocultamoss Ventana Modal Asignación Recursos
                        ScriptServer.AlternarVentana(this, this.GetType(), "CierreVentana", "contenidoRecursosAsignadosDespacho", "actualizacionRecursosAsignadosDespacho");
                        break;
                    }
                case "Movimiento":
                    //Determinando el comando a ejecutar
                    switch (((Button)sender).CommandName)
                    {
                        case "Obligatorio":
                            //Asignando resultado
                            lblErrorSalida.Text = lblMensajeHistorialVencimientos.Text;

                            break;
                        case "Opcional":
                            //Instanciando parada
                            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
                            {
                                //Realizando la actualización de la parada
                                RetornoOperacion resultado = objParada.ActualizarFechaSalida(Convert.ToDateTime(txtFechaSalida.Text), Parada.TipoActualizacionSalida.Manual,
                                                                                        EstanciaUnidad.TipoActualizacionFin.Manual, ParadaEvento.TipoActualizacionFin.Manual,
                                                                                        Convert.ToByte(ddlRazonSalidaTarde.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);

                                //Si no existe Error en la actualización de la salida
                                if (resultado.OperacionExitosa)
                                {
                                    //Cargamos Paradas
                                    cargaParadas();

                                    //Cargamos Servicios
                                    cargaServiciosParaDespacho();

                                    //Inicializalizamos  y Habilitamos controles para Ingresó de la Fecha de Salida
                                    inicializaValoresFechaSalida();
                                    habilitaControlesFechaSalida();
                                }
                                //Si hay problemas, se indica el error
                                else
                                {
                                    //Mostramos Mensaje Error
                                    lblErrorSalida.Text = resultado.Mensaje;
                                    //Mostrando ventana de confirmación de salida
                                    ScriptServer.AlternarVentana(btnAceptarIndicadorVencimientos, upbtnNoActualizacionEventos.GetType(), "AbreVentanaModal", "contenidoConfirmacionActualizacionSalida", "confirmacionActualizacionSalida");
                                }
                            }
                            break;
                    }

                    //Ocultando ventana de notificación de vencimientos
                    ScriptServer.AlternarVentana(btnAceptarIndicadorVencimientos, btnAceptarIndicadorVencimientos.GetType(), "NotificacionVencimientos", "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");

                    break;
            }
        }

        #endregion

        #region  Documentos

        /// <summary>
        /// Evento generado al Insertar la Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarInsertaParadaDocumento_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnAceptarInsertaParadaDocumento, upbtnAceptarInsertaParadaDocumento.GetType(), "CierraVentanaModalEventos", "modalIndicadorInsertaParadaDocumento", "contenidoModalIndicadorInsertaParadaDocumentos");

            //Insertamos la Parada
            res = insertaParada();

            //Validamos Resultado
            if (!res.OperacionExitosa)
            {
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(upbtnAceptarInsertaParadaDocumento, upbtnAceptarInsertaParadaDocumento.GetType(), "AbreVentanaModalInseraccionParada", "contenidoInsertaParada", "confirmacionInsertaParada");

            }
        }

        /// <summary>
        /// Evento Generado aldar click en el boton cancelara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarInsertaParadaDocumento_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para la Apertura de Insercción de Paradas
            ScriptServer.AlternarVentana(upbtnCancelarInsertaParadaDocumento, upbtnCancelarInsertaParadaDocumento.GetType(), "CierraVentanaModal", "contenidoInsertaParada", "confirmacionInsertaParada");

            //Registrando el script sólo para Cierre de Ventana de Advertencia
            ScriptServer.AlternarVentana(upbtnCancelarInsertaParadaDocumento, upbtnCancelarInsertaParadaDocumento.GetType(), "AbreVentanaModal", "modalIndicadorInsertaParadaDocumento", "contenidoModalIndicadorInsertaParadaDocumentos");

        }
        /// <summary>
        /// Evento generado al dar click el el botón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarDeshabilitaParadaDocumento_Click(object sender, EventArgs e)
        {
            //Cerramos Script de Advertencia
            ScriptServer.AlternarVentana(upbtnAceptarDeshabilitaParadaDocumento, upbtnAceptarDeshabilitaParadaDocumento.GetType(), "AbreVentanaModal",
                "modalIndicadorDeshabilitaParadaDocumento", "contenidoModalIndicadorDeshabilitaParadaDocumentos");

            //Deshabilitamos Parada
            deshabilitaParada();


        }
        /// <summary>
        /// Evento generado al dar click en el botón
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarDeshabilitaParadaDocumento_Click(object sender, EventArgs e)
        {
            //Cerramos Script de Advertencia
            ScriptServer.AlternarVentana(upbtnCancelarDeshabilitaParadaDocumento, upbtnCancelarDeshabilitaParadaDocumento.GetType(), "CierraVentanaModal",
                "modalIndicadorDeshabilitaParadaDocumento", "contenidoModalIndicadorDeshabilitaParadaDocumentos");

        }
        /// <summary>
        /// Validamos existencia de Documentos Recibidos ligando el segmento de la insercción de la parada
        /// </summary>
        /// <param name="id_movimiento"></param>
        private void validaDocumentosInsertaParada()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Parada
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Validamos Documentos recibidos ligado al segmento de la parada
                res = objParada.ValidacionDocumentosRecibidosInsertaParada();
            }

            //Validamos Resultado
            if (!res.OperacionExitosa)
            {
                //Mostramos Mensaje Error
                lblMensajeInsertaParadaDocumento.Text = res.Mensaje;
                //Registrando el script sólo para el Cierre de Moda Insercción de Paradas
                ScriptServer.AlternarVentana(upbtnAceptarInsertaParada, upbtnAceptarInsertaParada.GetType(), "CierraVentanaModal", "contenidoInsertaParada", "confirmacionInsertaParada");

                //Registrando el script sólo para Apertura de Ventana de Advertencia para la Cancelación de Documentos
                ScriptServer.AlternarVentana(upbtnAceptarInsertaParada, upbtnAceptarInsertaParada.GetType(), "AbreVentanaModal", "modalIndicadorInsertaParadaDocumento", "contenidoModalIndicadorInsertaParadaDocumentos");

            }
            else
            {
                //Insertamos Parada
                insertaParada();
            }
        }

        /// <summary>
        /// Validamos existencia de Documentos Recibidos ligando el segmento de la parada deshabilitada
        /// </summary>
        /// <param name="id_movimiento"></param>
        private void validaDocumentosDeshabilitaParada()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Parada
            using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["IdOrigen"])))
            {
                //Validamos Documentos ligado al segmento e la parada
                res = objParada.ValidacionDocumentosRecibidosDeshabilitaParada();

                //Validamos Resultado
                if (!res.OperacionExitosa)
                {
                    //Mostramos Mensaje Error
                    lblMensajeDeshabilitaParadaDocumento.Text = res.Mensaje;

                    //Mostramos Script de Advertencia de Documentos Recibidos para su cancelación
                    ScriptServer.AlternarVentana(uplkbDeshabilitaParada, uplkbDeshabilitaParada.GetType(), "AbreVentanaModal",
                        "modalIndicadorDeshabilitaParadaDocumento", "contenidoModalIndicadorDeshabilitaParadaDocumentos");

                }
                else
                {
                    //Deshabilitamos Parada
                    deshabilitaParada();
                }
            }
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
            cargaServiciosParaDespacho();

            Controles.InicializaGridview(gvParadas);

        }


        #endregion

        #region Devoluciones

        /// <summary>
        /// Click en opción devolución del GV de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDevolucionMov_Click(object sender, EventArgs e)
        {
            //Si existen registros mostrados
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "lnk", false);


                //Validando si Existe la Devolución
                if (gvParadas.SelectedDataKey["IdDevolucion"].ToString() != "0")
                {
                    //Inicializando Control de Devoluciones
                    wucDevolucionFaltante.InicializaDevolucion(Convert.ToInt32(gvParadas.SelectedDataKey["IdDevolucion"]));

                    //Abriendo ventana modal 
                    alternaVentanaModal("devolucion", (LinkButton)sender);
                    
                    //Mostramos Vista
                    mtvDocumentacion.ActiveViewIndex = 1;
                }
                else
                {
                    //Validando si Existe el Servicio
                    if (gvServicios.SelectedDataKey["Id"].ToString() != "0")
                    {
                        //Instanciando Movimiento
                        using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvParadas.SelectedDataKey["IdMovimiento"])))
                        {
                            //Validando que exista el Movimiento
                            if (mov.habilitar)
                            {
                                //Inicializando Control
                                wucDevolucionFaltante.InicializaDevolucion(mov.id_compania_emisor, mov.id_servicio, mov.id_movimiento, mov.id_parada_destino);

                                //Abriendo ventana modal 
                                alternaVentanaModal("devolucion", (LinkButton)sender);

                                //Mostramos Vista
                                mtvDocumentacion.ActiveViewIndex = 1;
                            }
                            else
                                //Mostrando Excepción
                                ScriptServer.MuestraNotificacion(this, "No se encontró el movimiento en cuestión.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                    else
                        //Mostrando Excepción
                        ScriptServer.MuestraNotificacion(this, "No hay Servicio Seleccionado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }

            }
        }

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
            alternaVentanaModal("referencias", this.Page);
            alternaVentanaModal("devolucion", this.Page);

            //Actualizando Comando del Boton Cerrar
            lkbCerrarDocumentacion.CommandName = "Devolucion";
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
            alternaVentanaModal("referencias", this.Page);
            alternaVentanaModal("devolucion", this.Page);

            //Actualizando Comando del Boton Cerrar
            lkbCerrarDocumentacion.CommandName = "Devolucion";
        }

        #endregion

        #region Impresión Carta Porte

        /// <summary>
        /// Método encargado de Imprimir el la Carta Porte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucImpresionPorte_ClickImprimirCartaPorte(object sender, EventArgs e)
        {
            //Invocando Método de Impresión
            wucImpresionPorte.ImprimeCartaPorte();
        }

        #endregion
    }
}