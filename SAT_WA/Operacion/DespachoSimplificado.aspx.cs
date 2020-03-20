using Microsoft.SqlServer.Types;
using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class DespachoSimplificado : System.Web.UI.Page
    {
        #region Atributos

        /// <summary>
        /// 
        /// </summary>
        private static string _nombre_pagina = "DespachoSimplificado";

        #endregion

        #region Eventos

        /// <summary>
        /// Carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recarga de página
            if (!Page.IsPostBack)
                //Inicializando contenido de forma
                inicializaForma();

            //Invocando Script de Configuración
            //validaSesionUnica();
        }
        /// <summary>
        /// Maneja el click sobre algún elemento del menú de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lkb = (LinkButton)sender;
            
            //Determianndo que elemento fue pulsado
            switch (lkb.CommandName)
            {
                case "MovimientosVacio":
                    //Mostrando control de movimientos en vacío
                    wucTerminoMovimientoVacio.InicializaControl();
                    alternaVentanaModal("movimientosVacio", lkbMenuMovimientosVacio);
                    break;
                case "Vencimientos":
                    //Inicializando control de historial de vencimientos
                    wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, 0);
                    //Configurando botón de cierre para no mostrar ninguna ventana posterior
                    lkbCerrarHistorialVencimientos.CommandArgument = "EstatusVencimiento";
                    //Mostrando ventana correspondiente
                    alternaVentanaModal("historialVencimientos", lkb);

                    /*/Inicializando control de historial
                    wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, 0);
                    //Inicializando captura de nuevo vencimiento
                    wucVencimiento.InicializaControl(19, 0);
                    //Mostrando ventana modal
                    alternaVentanaModal("actualizacionVencimiento", lkbMenuVencimientos);//*/
                    break;
                case "CambioOperador":
                    {
                        //Inicializamos Control
                        wucCambioOperador.InicializaControl();
                        //Mostrando ventana de cambio de operador
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbCambioOperador, lkbCambioOperador.GetType(), "CambioOperador", "modalCambioOperador", "confirmacionCambioOperador");
                        break;
                    }
                case "ServiciosPendientes":
                    //Definiendo Configuracion de la Ventana
                    string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES";
                    //Abriendo Nueva Ventana
                    TSDK.ASP.ScriptServer.AbreNuevaVentana("ServiciosPendientes.aspx", "Abrir", configuracion, Page);
                    break;
                case "HistorialViajes":
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/DespachoSimplificado.aspx", "~/Accesorios/HistorialServicio.aspx");
                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?&idRegistro={2}", urlReporte, "Porte", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES", Page);
                    break;
                case "ImpresionDocumentos":
                    //Obteniendo Ruta
                    string impresion_url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/Accesorios/ImpresionDocumentos.aspx");
                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?&idRegistro={2}&idUsuario={3}", impresion_url, "Impresiones", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario), "Impresion Documentos", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES", Page);
                    break;
            }
        }
        /// <summary>
        /// Clic en filtro de tipo de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbUnidad_CheckedChanged(object sender, EventArgs e)
        {
            //Limpiando contenido de gv de unidades
            Controles.InicializaGridview(gvUnidades);
            //Eliminando origen de datos
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            //Limpiando errores
            lblErrorServicio.Text = "";
        }
        /// <summary>
        /// Click en botón de búsqueda de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarServicios_Click(object sender, EventArgs e)
        {
            cargaUnidadesDespacho();
        }
        /// <summary>
        /// Cambio de tamaño del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoUnidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoUnidades.SelectedValue), true, 4);
        }
        /// <summary>
        /// Click en exportación de contenido de GV de unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarUnidades_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "*".ToArray());
        }
        /// <summary>
        /// Click en algún Link de GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionUnidad_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "NoUnidad":
                        //Construyendo URL de ventana de historial de unidad
                        string url = Cadena.RutaRelativaAAbsoluta("~/Operacion/DespachoSimplificado.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + gvUnidades.SelectedDataKey["*IdUnidad"].ToString() + "&idRegistroB=1");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width='+screen.width+',height='+screen.height+',fullscreen=YES";
                        //Abriendo Nueva Ventana
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Historial de Unidades", configuracion, Page);
                        break;
                    case "EstatusVencimiento":
                        {
                            //Validando si Existe el Vencimiento
                            if (Convert.ToInt32(gvUnidades.SelectedDataKey["*IdVencimiento"]) > 0)
                            
                                //Inicializando Control
                                wucVencimientoSimp.InicializaVencimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdVencimiento"]), 0, 0);
                            else
                                //Inicializando Control
                                wucVencimientoSimp.InicializaVencimiento(0, 19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));

                            //Mostrando ventana correspondiente
                            alternaVentanaModal("actualizacionVencimientoSimp", lkb);
                            break;
                        }
                    case "UltimoMonitoreo":
                        //Cargando contenido de bitácora de monitoreo
                        wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                        //Apertura de ventana de historial de monitoreo
                        alternaVentanaModal("historialBitacora", lkb);
                        break;
                    case "Nuevo":
                        //Mostrando ventana de selección de acción
                        alternaVentanaModal("seleccionServicioMovimiento", lkb);
                        break;
                    case "Documentacion":
                        //Inicializando control de documentación
                        wucServicioDocumentacion.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]), true, UserControls.wucServicioDocumentacion.VistaDocumentacion.Paradas);
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("documentacionServicio", lkb);
                        break;

                    case "Kms":
                        actualizaKilometrajeMovimiento();                        
                        break;
                    case "Remolque":
                        //Determinando el Id de parada a utilizar para inicializar control 
                        int id_parada = Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]);
                        //(Si la unidad sólo está asignada al movimiento, pero no se encuentra en la parada de inicio del mismo. Esto sólo ocurre cuando el Servicio está Documentado.)
                        //Instanciando servicio
                        using (Servicio srv = new Servicio(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])))
                        { 
                            //Si el servicio se encuentra documentado
                            if (srv.estatus == Servicio.Estatus.Documentado)
                            {
                                //Instanciando movimiento actual
                                using (Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                                    //Recuperando parada inicial del servicio
                                    id_parada = mov.id_parada_origen;
                            }
                        }

                        //Inicializando control de asignación de recurso
                        wucAsignacionRecurso.InicializaAsignacionRecurso(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]), id_parada, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        //Mostrando ventana modal de asignaciones
                        alternaVentanaModal("asignacionRecursos", lkb);
                        break;
                    case "Eventos":
                        //Inicializando control de eventos de parada
                        wucParadaEvento.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]), Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEventoActual"]));
                        //Indicando que cargumento adicional para el cierre de la ventana eventos será ejecutado
                        lkbCerrarEventosParada.CommandArgument = "EventoActual";
                        //Mostrando ventana de eventos
                        alternaVentanaModal("eventosParada", lkb);
                        break;
                    case "Referencia":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]));
                        //Asignando comando
                        lkbCerrarReferencias.CommandArgument = "Unidades";
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("referenciasServicio", lkb);
                        break;
                    case "Porte":
                        //Inicializando control de eventos de parada
                        wucParadaEvento.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]));
                        //Indicando que cargumento adicional para el cierre de la ventana eventos será ejecutado
                        lkbCerrarEventosParada.CommandArgument = "Eventos";
                        //Mostrando ventana de eventos
                        alternaVentanaModal("eventosParada", lkb);
                        break;
                    case "Salida":
                        //Configurando ventana de actualización de fecha para salida de parada
                        configuraVentanaActualizacionFecha(lkb.CommandName);
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", lkb);
                        break;
                    case "Llegada":
                        //Si hay servicio asignado
                        if (Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]) > 0)
                        {
                            //Configurando ventana de actualización de fecha para llegada a parada
                            configuraVentanaActualizacionFecha(lkb.CommandName);
                            //Abriendo ventana de confirmación de llegada a primer parada de servicio
                            alternaVentanaModal("inicioFinMovimientoServicio", lkb);
                        }
                        //Si hay movimiento vacío (reposicionamiento)
                        else
                        {
                            //Configurando ventana para término de mov vacío
                            configuraVentanaActualizacionFecha(lkb.CommandName);
                            //Abriendo modal de confirmación
                            alternaVentanaModal("inicioFinMovimientoServicio", lkb);
                        }
                        break;
                    case "Iniciar":
                        //Configurando ventana de actualización de fecha para inicio de servicio
                        configuraVentanaActualizacionFecha(lkb.CommandName);
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", lkb);
                        break;
                    case "Terminar":
                        //Configurando ventana de actualización de fecha para fin de servicio
                        configuraVentanaActualizacionFecha(lkb.CommandName);
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", lkb);
                        break;
                    case "ImprimirPorte":
                        //Invocando Método de Validación en la Carta Porte
                        if (SAT_CL.Documentacion.Servicio.ValidaImpresionEspecificaCartaPorte(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])).OperacionExitosa)
                        {
                            //Mostramos Modal
                            alternaVentanaModal("impresionPorte", lkb);
                            //Inicializando control
                            wucImpresionPorte.InicializaImpresionCartaPorte(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]), UserControls.wucImpresionPorte.TipoImpresion.CartaPorte);
                        }
                        else
                        {
                            //Obteniendo Ruta
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/DespachoSimplificado.aspx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Porte", Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                        break;
                    case "ImprimirPorteViajera":
                        //Invocando Método de Validación en la Carta Porte Vijera
                        if (gvUnidades.SelectedIndex != -1)
                        {
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "PorteViajera", Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);

                        }
                        break;
                    case "BitacoraViaje":
                        //Invocando Método de Validación en la Carta Porte
                        if (SAT_CL.Documentacion.Servicio.ValidaImpresionEspecificaCartaPorte(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])).OperacionExitosa)
                        {
                            //Mostramos Modal
                            alternaVentanaModal("impresionPorte", lkb);
                            //Inicializando control
                            wucImpresionPorte.InicializaImpresionCartaPorte(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]), UserControls.wucImpresionPorte.TipoImpresion.BitacoraViaje);
                        }
                        else
                        {
                            //Obteniendo Ruta
                            string urlBitacora = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/DespachoSimplificado.aspx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlBitacora, "BitacoraViaje", Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])), "Bitacora de Viaje", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                        break;
                    case "Validar":
                        {
                            //Cargando los Proveedores Disponibles por Unidad
                            using (DataTable dtServiciosGPS = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(104, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), ""))
                            {
                                //Validando que existan Registros
                                if (Validacion.ValidaOrigenDatos(dtServiciosGPS))
                                {
                                    //Validando que solo Exista un Servicio GPS
                                    if (dtServiciosGPS.Rows.Count == 1)
                                    {
                                        //Recorriendo Registros
                                        foreach (DataRow dr in dtServiciosGPS.Rows)
                                        {
                                            //Guargando Incidencia de GPS
                                            RetornoOperacion result = new RetornoOperacion();

                                            //Validando Unidad
                                            result = validaUnidad(Convert.ToInt32(dr["id"]));

                                            //Validando Resultado
                                            if (result.OperacionExitosa)

                                                //Cargando Unidades Despacho
                                                cargaUnidadesManteniendoSeleccion();

                                            //Mostrando Notificación
                                            ScriptServer.MuestraNotificacion(lkb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                            //Terminando Ciclo
                                            break;
                                        }
                                    }
                                    else if (dtServiciosGPS.Rows.Count > 1)
                                    {
                                        //Cargando Control
                                        Controles.CargaDropDownList(ddlServicioGPS, dtServiciosGPS, "id", "descripcion");

                                        //Mostrando Ventana Modal
                                        alternaVentanaModal("ProveedorGPS", this.Page);
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion(lkb, new RetornoOperacion("No Existen Servicios GPS para esta Unidad"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            break;
                        }
                    case "Evaluacion":
                        {
                            //Instanciando Resultado de Evaluación
                            using (SAT_CL.Monitoreo.EvaluacionBitacora evaluacion = new SAT_CL.Monitoreo.EvaluacionBitacora(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEvaluacion"])))
                            {
                                //Validando Si existen Registros
                                if (evaluacion.habilitar)
                                {
                                    //Mostrando Ventana
                                    alternaVentanaModal("VentanaETA", (LinkButton)sender);

                                    //Instanciando Parada Destino
                                    using (SAT_CL.Despacho.Parada destino = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParadaDestino"])))
                                    using (SAT_CL.Despacho.Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                                    {
                                        //Asignando Destino
                                        lblDestino.Text = destino.habilitar ? destino.descripcion : "--";
                                        
                                        //Asignando Valores
                                        lblDistancia.Text = string.Format("{0}/{1} Km", Decimal.Round(Convert.ToDecimal(evaluacion.distancia) / 1000, 5), mov.habilitar ? mov.kms.ToString() : "N/A");
                                        lblTiempo.Text = Cadena.ConvierteMinutosACadena(Convert.ToInt32(evaluacion.tiempo / 60));
                                        lblHoraLlegada.Text = evaluacion.hora_llegada == DateTime.MinValue ? "" : evaluacion.hora_llegada.ToString("dd/MM/yyyy HH:mm");
                                        lblCita.Text = evaluacion.cita == DateTime.MinValue ? "" : evaluacion.cita.ToString("dd/MM/yyyy HH:mm");

                                        //Validando Hora de Llegada
                                        if (evaluacion.hora_llegada.CompareTo(evaluacion.cita == DateTime.MinValue ? evaluacion.hora_llegada : evaluacion.cita) > 0)

                                            //Asignando Estilo
                                            lblHoraLlegada.CssClass = "label_error";
                                        else
                                            //Asignando Estilo
                                            lblHoraLlegada.CssClass = "label_negrita";
                                    }
                                }
                                else
                                    //Instanciando Excepción
                                    ScriptServer.MuestraNotificacion((LinkButton)sender, new RetornoOperacion("No existe la Evaluación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                            break;
                        }
                }
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
                                  where dr["Asignacion"].ToString().Equals("Tercero")
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbEvaluacion_Click(object sender, ImageClickEventArgs e)
        {
            //Si existen registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvUnidades, sender, "imb", false);
                //Convirtiendo objeto a ImageButton
                ImageButton imb = (ImageButton)sender;

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Invocando Método de Resultado
                result = muestraResultadoETA(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]),
                                             Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]), 
                                             Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEvaluacion"]), 
                                             Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParadaDestino"]));

                //Mostrando Ventana Modal
                alternaVentanaModal("ResultadoETA", this.Page);

                //Instanciando Excepción
                //ScriptServer.MuestraNotificacion(imb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Cambio de página activa del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// Enlace a datos de cada fila del GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Creando Menu Contextual
                    Controles.CreaMenuContextual(e, "menuDespachoSimp", "menuDespachoSimpOpciones", "MostrarMenuDespachoSimplificado", true, true);
                    
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Encontrando controles de interés
                    using (LinkButton lkbNuevo = (LinkButton)e.Row.FindControl("lkbNuevoServMov"),
                        lkbEstatusVencimiento = (LinkButton)e.Row.FindControl("lkbEstatusVencimiento"),
                        lkbKms = (LinkButton)e.Row.FindControl("lkbKms"),
                        lkbActParada = (LinkButton)e.Row.FindControl("lkbSalidaLLegada"),
                        lkbActServicio = (LinkButton)e.Row.FindControl("lkbIniciarTerminar"),
                        lkbImprimir = (LinkButton)e.Row.FindControl("lkbImprimirPorte"),
                        lkbImprimirViajera = (LinkButton)e.Row.FindControl("lkbImprimirPorteViajera"),
                        lkbBitacoraViaje = (LinkButton)e.Row.FindControl("lkbBitocoraViaje"),
                        lkbValidarEvaluacion = (LinkButton)e.Row.FindControl("lkbValidar"),
                        lkbHoraEstimada = (LinkButton)e.Row.FindControl("lkbHoraEstimada"))
                    {
                        using (Label lblServMov = (Label)e.Row.FindControl("lblNoServMov"))
                        {
                            using (ImageButton imgEvaluacion = (ImageButton)e.Row.FindControl("imbEvaluacion"))
                            using (Image imgHoraLlegada = (Image)e.Row.FindControl("imbHoraLlegada"))
                            {
                                /**** APLICANDO CONFIGURACIÓN DE INFORMACIÓN DE NO. DE MOVIMIENTO O SERVICIO ****/
                                //Si no hay asignación de movimiento o servicio
                                if (row.Field<int>("*IdServicio") == 0 && row.Field<int>("Movimiento") == 0)
                                {
                                    //Ocultando etiqueta
                                    lblServMov.Visible = false;
                                    //Configurando comando de botón Para Nuevas Asignaciones
                                    lkbNuevo.CommandName = "Nuevo";
                                    lkbNuevo.Text = "Nuevo";
                                    lkbNuevo.ToolTip = "Asignar Nuevo Servicio o Reposicionar";

                                    //Si no existen vencimientos
                                    if (row.Field<int>("*IdVencimiento") == 0)
                                        //Mostrando acceso a nuevo servicio o movimiento
                                        lkbNuevo.Visible = true;
                                    else
                                        //Si hay vencimiento, no se permiten nuevas asignaciones
                                        lkbNuevo.Visible = false;
                                }
                                //Si hay servicio y movimiento
                                else if (row.Field<int>("*IdServicio") > 0 && row.Field<int>("Movimiento") > 0)
                                {
                                    //Ocultando etiqueta
                                    lblServMov.Visible = false;
                                    //Configurando comando de botón Para Nuevas Asignaciones
                                    lkbNuevo.CommandName = "Documentacion";
                                    lkbNuevo.Text = row.Field<string>("NoServicioMov");
                                    lkbNuevo.ToolTip = "Actualizar Información principal del Servicio";

                                    //Si no existen vencimientos
                                    if (row.Field<int>("*IdVencimiento") == 0)
                                        //Mostrando acceso a nuevo servicio o movimiento
                                        lkbNuevo.Visible = true;
                                    else
                                        //Si hay vencimiento, no se permiten nuevas asignaciones
                                        lkbNuevo.Visible = false;
                                }
                                //Si sólo hay movimiento o sólo hay servicio
                                else if ((row.Field<int>("*IdServicio") == 0 && row.Field<int>("Movimiento") > 0) ||
                                    (row.Field<int>("*IdServicio") > 0 && row.Field<int>("Movimiento") == 0))
                                {
                                    //Mostrando etiqueta
                                    lblServMov.Visible = true;
                                    //Ocultando acceso a nuevo servicio o movimiento
                                    lkbNuevo.Visible = false;
                                }

                                /**** APLICANDO CONFIGURACIÓN DE ESTATUS MODIFICADO POR VENCIMIENTOS  ****/
                                //Si no existen vencimientos
                                if (row.Field<int>("*IdVencimiento") > 0)
                                {
                                    //Asignando Color si Existe Vencimiento
                                    e.Row.Cells[3].BackColor =
                                    e.Row.Cells[2].BackColor =
                                    e.Row.Cells[1].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                                }

                                /**** APLICANDO CONFIGURACIÓN DE ESTATUS MODIFICADO POR VENCIMIENTOS 
                                //Si hay un vencimiento
                                if (row.Field<int>("*IdVencimiento") > 0)
                                {
                                    //Mostrando estatus en Link
                                    lkbEstatusVencimiento.Visible = true;
                                    //lblEstatusVencimiento.Visible = false;
                                }
                                //Si no hay vencimientos
                                else
                                {
                                    //Mostrando estatus en Link
                                    lkbEstatusVencimiento.Visible = false;
                                    //lblEstatusVencimiento.Visible = true;
                                }****/


                                /**** APLICANDO CONFIGURACIÓN DE CALCULO DE KILOMETRAJE ****/
                                //Si no hay movimiento asignados
                                if (row.Field<int>("Movimiento") == 0)
                                    lkbKms.Visible = false;


                                /**** APLICANDO CONFIGURACIÓN DE IMPRESIÓN DE CARTA PORTE ****/
                                //Si no hay movimiento asignados
                                if (row.Field<int>("*IdServicio") == 0)
                                {
                                    lkbBitacoraViaje.Visible =
                                    lkbImprimir.Visible = 
                                    lkbImprimirViajera.Visible = false;
                                }
                                else
                                {
                                    lkbBitacoraViaje.Visible =
                                    lkbImprimir.Visible =
                                    lkbImprimirViajera.Visible = true;
                                }

                                /**** APLICANDO CONFIGURACIÓN DE ACTUALIZACIÓN DE PARADAS ****/
                                //Si la unidad está en estatus ocupado y tiene movimiento asignado
                                if ((Unidad.Estatus)row.Field<byte>("*IdEstatusUnidad") == Unidad.Estatus.ParadaOcupado
                                    && row.Field<int>("Movimiento") > 0 && (SAT_CL.Documentacion.Servicio.Estatus)row.Field<byte>("*IdEstatusServicio") != SAT_CL.Documentacion.Servicio.Estatus.Documentado)
                                    //Aplicando texto y comando requerido
                                    lkbActParada.Text = lkbActParada.CommandName = "Salida";
                                //Si la unidad está en tránsito
                                else if ((Unidad.Estatus)row.Field<byte>("*IdEstatusUnidad") == Unidad.Estatus.Transito)
                                    //Aplicando texto y comando requerido
                                    lkbActParada.Text = lkbActParada.CommandName = "Llegada";
                                //Cualquier otro caso
                                else
                                    //Sin Texto (No visible)
                                    lkbActParada.Text = lkbActParada.CommandName = "";
                                //Sin Texto (No visible)
                                lkbActServicio.Text = lkbActServicio.CommandName = "";


                                /**** APLICANDO CONFIGURACIÓN DE ACTUALIZACIÓN DE SERVICIO ****/
                                //Si el estatus del servicio es Documentado
                                if ((SAT_CL.Documentacion.Servicio.Estatus)row.Field<byte>("*IdEstatusServicio") == SAT_CL.Documentacion.Servicio.Estatus.Documentado)
                                    //Aplicando texto y comando requerido
                                    lkbActServicio.Text = lkbActServicio.CommandName = "Iniciar";
                                //Si su estatus es Iniciado
                                else if ((SAT_CL.Documentacion.Servicio.Estatus)row.Field<byte>("*IdEstatusServicio") == SAT_CL.Documentacion.Servicio.Estatus.Iniciado)
                                {
                                    //Obteneindo la última parada del servicio y comparandola con la parada actual
                                    if (Parada.ObtieneUltimaParada(row.Field<int>("*IdServicio")) == row.Field<int>("*IdParada"))
                                        //Aplicando texto y comando requerido
                                        lkbActServicio.Text = lkbActServicio.CommandName = "Terminar";
                                }

                                /** APLICANDO CONFIGURACIÓN DE EVALUACIÓN DE LA UNIDAD **/
                                //Validando que exista la Evaluación
                                if (row.Field<int>("*IdEvaluacion") > 0)
                                {
                                    //Validando que existan los Controles
                                    if (lkbValidarEvaluacion != null && imgEvaluacion != null && lkbHoraEstimada != null)
                                    {
                                        //Mostrando Control
                                        lkbValidarEvaluacion.Visible =
                                        lkbHoraEstimada.Enabled = true;
                                        
                                        //Validando Resultado de la Evaluacion
                                        switch ((SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora)row.Field<byte>("*IdRespuestaEvaluacion"))
                                        {
                                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok:
                                                    //Configurando Control
                                                    imgEvaluacion.Visible = 
                                                    imgEvaluacion.Enabled = true;
                                                    imgEvaluacion.ImageUrl = "~/Image/semaforo_verde.png";
                                                    imgEvaluacion.ToolTip = "Unidad sin Problemas";
                                                    break;
                                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadAlejandose:
                                                    //Configurando Control
                                                    imgEvaluacion.Visible = 
                                                    imgEvaluacion.Enabled = true;
                                                    imgEvaluacion.ImageUrl = "~/Image/semaforo_rojo.png";
                                                    imgEvaluacion.ToolTip = "La Unidad se esta alejando del Destino";
                                                    break;//*/
                                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente:
                                                    //Configurando Control
                                                    imgEvaluacion.Visible =
                                                    imgEvaluacion.Enabled = true;
                                                    imgEvaluacion.ImageUrl = "~/Image/semaforo_rojo.png";
                                                    imgEvaluacion.ToolTip = "La Unidad no se encuentra en la Ubicación";
                                                    break;
                                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida:
                                                    //Configurando Control
                                                    imgEvaluacion.Visible =
                                                    imgEvaluacion.Enabled = true;
                                                    imgEvaluacion.ImageUrl = "~/Image/semaforo_rojo.png";
                                                    imgEvaluacion.ToolTip = "La Unidad esta Detenida";
                                                    break;
                                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido:
                                                    //Configurando Control
                                                    imgEvaluacion.Visible =
                                                    imgEvaluacion.Enabled = true;
                                                    imgEvaluacion.ImageUrl = "~/Image/semaforo_rojo.png";
                                                    imgEvaluacion.ToolTip = "La Unidad excedio el tiempo permitido de Posicionamiento";
                                                    break;
                                        }

                                        //Instanciando Evaluacion
                                        using (SAT_CL.Monitoreo.EvaluacionBitacora evaluacion = new SAT_CL.Monitoreo.EvaluacionBitacora(row.Field<int>("*IdEvaluacion")))
                                        {
                                            //Validando Evaluación
                                            if (evaluacion.habilitar)
                                            {
                                                //Resultado Evaluacion Fecha
                                                int result = evaluacion.hora_llegada.CompareTo(evaluacion.cita == DateTime.MinValue ? evaluacion.hora_llegada : evaluacion.cita);

                                                //Validando Resultado
                                                switch (result)
                                                {
                                                    case 1:
                                                        {
                                                            //Configurando Semaforo
                                                            imgHoraLlegada.Visible = true;
                                                            imgHoraLlegada.ImageUrl = "~/Image/semaforo_rojo.png";
                                                            break;
                                                        }
                                                    case 0:
                                                        {
                                                            //Configurando Semaforo
                                                            imgHoraLlegada.Visible = false;
                                                            break;
                                                        }
                                                    case -1:
                                                        {
                                                            //Configurando Semaforo
                                                            imgHoraLlegada.Visible = true;
                                                            imgHoraLlegada.ImageUrl = "~/Image/semaforo_verde.png";
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            //Ocultando Indicador
                                                            imgHoraLlegada.Visible = false;
                                                            break;
                                                        }
                                                }
                                                
                                                //Asignando Estilo
                                                //e.Row.Cells[12].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                                            }
                                            else
                                                //Ocultando Indicador
                                                imgHoraLlegada.Visible = false;
                                        }
                                    }
                                }
                                else
                                {
                                    //Ocultando Indicador
                                    imgEvaluacion.Visible = false;

                                    //Mostrando Control
                                    lkbValidarEvaluacion.Visible = true;
                                    lkbHoraEstimada.Enabled = false;
                                }

                                /**** APLICANDO COLOR DE FONDO DE FILA ACORDE A ESTATUS DE UNIDAD Y VENCIMIENTOS ****/
                                //Determinando estatus de unidad
                                switch ((Unidad.Estatus)row.Field<byte>("*IdEstatusUnidad"))
                                {
                                    case Unidad.Estatus.ParadaDisponible:
                                        //Si hay un vencimiento
                                        if (row.Field<int>("*IdVencimiento") > 0)
                                            //Cambiando color de fondo de la fila a rojo
                                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                                        break;
                                }

                                //OCULTA O MUESTRA LA IMAGEN CUANDO LAS UNIDADES SON DE ARRASTRE O NO
                                using (ImageButton imb = (ImageButton)e.Row.FindControl("imbOperador"))
                                {
                                    //VALIDA SI ESTA SELECCIONADA LA OPCIÓN DE ARRASTRE
                                    if (rdbUnidadArrastre.Checked)
                                        //NO MUESTRA LA IMAGEN
                                        imb.Visible = false;
                                    //EN CASO CONTRARIO
                                    else
                                    {
                                        ////Recuperando el operador asignado a cada unidad                                 
                                        int id_operador = Convert.ToInt32(row.Field<int>("*IdOperador"));
                                        //Si no tiene operador asignado
                                        if (id_operador != 0)
                                            //MUESTRA LA IMAGEN DE OPERADOR ASIGNADO
                                            imb.ImageUrl = "~/Image/OperadorSimp.png";
                                        //En caso contrario
                                        else
                                            //MUESTRA LA IMAGENQUE OERMITE ASIGNAR UN OPERADOR
                                            imb.ImageUrl = "~/Image/AsignaOperador.png";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Validación GPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidarGPS_Click(object sender, EventArgs e)
        {
            //Validando que existan Unidades
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Declarando Objeto de Retorno
                List<RetornoOperacion> result = new List<RetornoOperacion>();
                /** Definición de "unidades" **/
                // Item1 - IdProveedorUnidadWS
                // Item2 - IdProveedorWS
                // Item3 - IdServicio
                // Item4 - IdParada
                // Item5 - IdMovimiento
                // Item6 - idParadaDestino
                List<Tuple<int, int, int, int, int, int>> unidades = new List<Tuple<int, int, int, int, int, int>>();
                List<int> proveedores_ws = new List<int>();

                //Obteniendo Unidades
                unidades = (from DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Rows
                            where Convert.ToInt32(dr["IdProveedorWS"]) != 0
                            select new Tuple<int, int, int, int, int, int>
                            (Convert.ToInt32(dr["IdProveedorUnidadWS"]),
                             Convert.ToInt32(dr["IdProveedorWS"]),
                             Convert.ToInt32(dr["*IdServicio"]),
                             Convert.ToInt32(dr["*IdParada"]),
                             Convert.ToInt32(dr["Movimiento"]),
                             Convert.ToInt32(dr["*IdParadaDestino"])
                             )).ToList();
                //Obteniendo Proveedores WS
                proveedores_ws = (from DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").Rows
                                  where Convert.ToInt32(dr["IdProveedorWS"]) != 0
                                  select Convert.ToInt32(dr["IdProveedorWS"])).Distinct().ToList();

                if (unidades.Count > 0)
                {
                    if (proveedores_ws.Count > 0)
                    {
                        //Evaluando Unidades
                        result = SAT_CL.Monitoreo.ProveedorWSUnidad.EvaluaUnidadesGPS(proveedores_ws, unidades, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        foreach(RetornoOperacion retorno in result)
                            //Mostrando resultado de la transacción
                            ScriptServer.MuestraNotificacion(this.Page, retorno.IdRegistro.ToString(), retorno.Mensaje, retorno.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Instanciando Excepción
                        ScriptServer.MuestraNotificacion(btnValidarGPS, new RetornoOperacion("No se pudieron recuperar los Proveedores GPS"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Instanciando Excepción
                    ScriptServer.MuestraNotificacion(btnValidarGPS, new RetornoOperacion("No se recuperaron las Unidades Correctamente"), ScriptServer.PosicionNotificacion.AbajoDerecha);

                //Cargando Gridview
                cargaUnidadesDespacho();
            }
            else
                //Instanciando Excepción
                ScriptServer.MuestraNotificacion(btnValidarGPS, new RetornoOperacion("No Existen Unidades por Validar"), ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Cambio de criterio de ordenamiento en GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenarUnidades.Text = Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 4);
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
                case "HistorialBitacora":
                    //Cerrando ventana modal 
                    alternaVentanaModal("historialBitacora", lkbCerrar);
                    break;
                case "Bitacora":
                    //Cerrando ventana modal 
                    alternaVentanaModal("bitacoraMonitoreo", lkbCerrar);
                    //Mostrando Historial
                    alternaVentanaModal("historialBitacora", lkbCerrar);
                    //Quitando Imagen del Script
                    wucBitacoraMonitoreo.RemueveImagenElevatedZoom();
                    break;
                case "SeleccionServMov":
                    //Cerrando ventana modal 
                    alternaVentanaModal("seleccionServicioMovimiento", lkbCerrar);
                    break;
                case "AsignacionRecursos":
                    //Cerrando ventana modal 
                    alternaVentanaModal("asignacionRecursos", lkbCerrar);
                    break;
                case "CopiaServicio":
                    //Cerrando ventana modal 
                    alternaVentanaModal("copiaServicioMaestro", lkbCerrar);
                    break;
                case "ReubicacionUnidad":
                    //Cerrando ventana modal 
                    alternaVentanaModal("reubicacionUnidad", lkbCerrar);
                    break;
                case "MovimientosVacio":
                    //Cerrando modal de movimiento vacío
                    alternaVentanaModal("movimientosVacio", lkbCerrar);
                    //Actualziando contenido de gridview
                    cargaUnidadesManteniendoSeleccion();
                    break;
                case "IndicadorVencimientos":
                    //Cerrando ventana modal de notificación de vencimientos
                    alternaVentanaModal("indicadorVencimientos", lkbCerrar);
                    break;
                case "HistorialVencimientos":
                    //Cerrando ventana de historial
                    alternaVentanaModal("historialVencimientos", lkbCerrar);
                    //Determinando si es necesario mostrar alguna ventana diciional
                    switch (lkbCerrar.CommandArgument)
                    {
                        case "NotificacionVencimientos":
                            //Abriendo ventana de notificación de vencimientos
                            alternaVentanaModal("indicadorVencimientos", lkbCerrar);
                            break;
                        default:
                            cargaUnidadesManteniendoSeleccion();
                            break;
                    }

                    break;
                case "ActualizacionVencimiento":
                    //Cerrando ventana modal de vencimiento
                    alternaVentanaModal("actualizacionVencimiento", lkbCerrar);
                    //Si hay un registro visualizado en historial
                    if (wucVencimientosHistorial.id_recurso > 0)
                        //Abriendo ventana de historial de vencimientos
                        alternaVentanaModal("historialVencimientos", lkbCerrar);
                    break;
                case "ActualizacionVencimientoSimp":
                    //Cerrando ventana modal de vencimiento
                    alternaVentanaModal("actualizacionVencimientoSimp", lkbCerrar);
                    break;
                case "ConfirmacionEventosPendientes":
                    //Cerrando ventana modal de confirmación
                    alternaVentanaModal("confirmacionEventosPendientes", lkbCerrar);
                    //Abriendo ventana de salida
                    alternaVentanaModal("inicioFinMovimientoServicio", lkbCerrar);
                    break;
                case "EventosParada":
                    //Cerrando ventana de actualización de eventos
                    alternaVentanaModal("eventosParada", lkbCerrar);
                    //En base al argumento opcional del control, se determina si es requerida la apertura de la ventana de salida o documentación
                    if (lkbCerrar.CommandArgument == "Salida")
                        //Abriendo ventana de salida
                        alternaVentanaModal("inicioFinMovimientoServicio", lkbCerrar);
                    else if (lkbCerrar.CommandArgument == "Documentacion")
                        //Abriendo ventana de documentación
                        alternaVentanaModal("documentacionServicio", lkbCerrar);
                    break;
                case "ReferenciasServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("referenciasServicio", lkbCerrar);
                    //En base al argumento opcional del control, se determina si es requerida la apertura de la ventana de salida
                    if (lkbCerrar.CommandArgument == "Pendientes")
                        //Abriendo ventana de pendientes
                        alternaVentanaModal("servicioDocumentadoPendiente", lkbCerrar);
                    break;
                case "ActualizacionParadas":
                    //Cerrando modal de actualización de paradas
                    alternaVentanaModal("actualizacionParadas", lkbCerrar);
                    break;
                case "KilometrajeMovimiento":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("kilometrajeMovimiento", lkbCerrar);
                    break;
                case "Documentacion":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("documentacionServicio", lkbCerrar);
                    break;
                case "ServicioPendiente":
                    //Cerrando modal de asignación de unidad a servicio documentado pendiente
                    alternaVentanaModal("servicioDocumentadoPendiente", lkbCerrar);
                    break;
                case "CambioOperador":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("CambioOperador", lkbCerrar);
                    break;
                case "CerrarPublicacion":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("PublicacionUnidad", lkbCerrar);
                    break;
                case "CerrarRequisicion":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("RequisicionesServicio", lkbCerrar);
                    break;
                case "VentanaETA":
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal("VentanaETA", lkbCerrar);
                    break;
                default:
                    //  Abriendo ventana para la Publicación de la Unidad
                    alternaVentanaModal(lkbCerrar.CommandName, lkbCerrar);
                    break;
            }
        }

        #endregion

        #region Bitácora Monitoreo

        /// <summary>
        /// Nuevo asiento enbitácora de monitoreo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucBitacoraMonitoreoHistorial_btnNuevoBitacora(object sender, EventArgs e)
        {
            //Cerrando ventana de historial de bitácora
            alternaVentanaModal("historialBitacora", this);
            //Inicializando control para nueva bitácora
            wucBitacoraMonitoreo.InicializaControl(0, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]),
                    Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]), Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEventoActual"]), 
                    Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]), 19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
            //Mostrando ventana de control bitácora
            alternaVentanaModal("bitacoraMonitoreo", this);
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
                //Recargando contenido de bitácora de monitoreo
                wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                //Cargando Unidades
                cargaUnidadesManteniendoSeleccion();
                //Ocultando ventana actual
                alternaVentanaModal("bitacoraMonitoreo", this);
                //Mostrando ventana de historial
                alternaVentanaModal("historialBitacora", this);
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
                //Recargando contenido de bitácora de monitoreo
                wucBitacoraMonitoreoHistorial.InicializaControl(19, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                //Cargando Unidades
                cargaUnidadesManteniendoSeleccion();
                //Ocultando ventana actual
                alternaVentanaModal("bitacoraMonitoreo", this);
                //Mostrando ventana de historial
                alternaVentanaModal("historialBitacora", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Nuevo Servicio o Movimiento

        /// <summary>
        /// Clic en botón Nuevo servicio o reposicionamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevoServicioMovimiento_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            Button boton = (Button)sender;

            //Ocultando ventana de selección
            alternaVentanaModal("seleccionServicioMovimiento", this);

            switch (boton.CommandName)
            {
                case "NuevoServicio":
                    using (Parada p = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])))
                        //Inicializando contenido de control
                        wucServicioDocumentacion.InicializaControl(p.id_ubicacion);
                    //Abriendo ventana de copia de servicio maestro
                    alternaVentanaModal("documentacionServicio", this);
                    break;
                case "CopiaServicio":
                    using (Parada p = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])))
                        //Inicializando contenido de control
                        wucServicioCopia.InicializaServicioCopia(p.id_ubicacion);
                    //Abriendo ventana de copia de servicio maestro
                    alternaVentanaModal("copiaServicioMaestro", this);
                    break;
                case "ServicioPendiente":
                    //Cargando listado de servicios pendientes
                    cargaServiciosPendientes();
                    //Abriendo ventana de asignación de servicios pendientes
                    alternaVentanaModal("servicioDocumentadoPendiente", this);
                    break;
                case "Movimiento":
                    //Inicializando contenido de control
                    wucReubicacion.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                    Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual,
                                                    EstanciaUnidad.TipoActualizacionInicio.Manual, EstanciaUnidad.TipoActualizacionFin.Manual);
                    //Abriendo ventana de reubicacion
                    alternaVentanaModal("reubicacionUnidad", this);
                    break;
            }
        }
        /// <summary>
        /// Clic en botón copiar servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioCopia_ClickGuardarServicioCopia(object sender, EventArgs e)
        {
            //Declarando variable de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicialziando transacción
            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando unidad implicada
                using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
                {
                    //Si la unidad se encuentra en estatus apropiado (parada disponible)
                    if (unidad.EstatusUnidad == Unidad.Estatus.ParadaDisponible)
                    {
                        //Instanciando servicio seleccionado a copia
                        using (SAT_CL.Documentacion.Servicio maestro = new SAT_CL.Documentacion.Servicio(wucServicioCopia.id_servicio_maestro))
                        {
                            //Instanciando estancia actual de la unidad
                            using (EstanciaUnidad estancia = new EstanciaUnidad(unidad.id_estancia))
                            {
                                //Instanciando parada asociada a la estancia de la unidad
                                using (Parada parada = new Parada(estancia.id_parada))
                                {
                                    //Validando que el servicio seleccionado tenga inicio en la ubicación actual de la unidad
                                    if (maestro.id_ubicacion_carga == parada.id_ubicacion)
                                    {
                                        //Realizando copia de servicio seleccionado
                                        resultado = wucServicioCopia.CopiaServicio();

                                        //Si no hubo errores de copiado, se debe asignar el primer recurso del servicio
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Recuperando primer movimiento del servicio copiado
                                            using (Movimiento mov = new Movimiento(resultado.IdRegistro, 1))
                                            {
                                                //Si el movimiento fue recuperado
                                                if (mov.id_movimiento > 0)
                                                    //Realizando la asignación del Recuros 
                                                    resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursoParaDespacho(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                               mov.id_movimiento, mov.id_parada_origen, EstanciaUnidad.Tipo.Operativa, EstanciaUnidad.TipoActualizacionInicio.Manual, MovimientoAsignacionRecurso.Tipo.Unidad,
                                                               unidad.id_tipo_unidad, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                else
                                                    resultado = new RetornoOperacion("No pudo ser recuperado el primer movimiento del servicio copiado.");
                                            }
                                        }
                                        else
                                            resultado = new RetornoOperacion(string.Format("Error al realizar copia de servicio: {0}", resultado.Mensaje));

                                        //Si no hay errores se confirma transacción
                                        if (resultado.OperacionExitosa)
                                            scope.Complete();
                                    }
                                    //Si la unidad no se encuentra en el lugar apropiado para el servicio
                                    else
                                        resultado = new RetornoOperacion(string.Format("La unidad no se encuentra en el punto de inicio del servicio solicitado, reubique primero esta unidad."));
                                }
                            }
                        }
                    }
                    //Si el estatus de la unidad no es el adecuado
                    else
                        resultado = new RetornoOperacion("La unidad no se encuentra en estatus Disponible.");
                }
            }
            
            //Si no hay errores de copia
            if (resultado.OperacionExitosa)
            {
                //Ocultando ventana modal
                alternaVentanaModal("copiaServicioMaestro", this);
                //Actualizando carga de unidades
                cargaUnidadesManteniendoSeleccion();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Clic en botón cancelar copia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioCopia_ClickCancelarServicioCopia(object sender, EventArgs e)
        {
            //Ocultando ventana modal de copia de servicio
            alternaVentanaModal("copiaServicioMaestro", this);
        }
        /// <summary>
        /// Click en Registrar Movimiento vacío
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReubicacion_ClickRegistrar(object sender, EventArgs e)
        {
            //Realizando validaciones de vencimientos
            RetornoOperacion resultado = validaVencimientosActivos("Vacio");

            //Si no hay vencimientos
            if (resultado.OperacionExitosa)
                //Creando movimiento vacío
                movimientoVacio();
            //Si hay vencimientos que no permiten continuar
            else
            {
                //Ocultando ventana modal de mov vacío
                alternaVentanaModal("reubicacionUnidad", this);
                //Mostrando notificación de vencimientos
                alternaVentanaModal("indicadorVencimientos", this);
            }
        }
        /// <summary>
        /// Click en Cancelar Movimiento Vacío
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReubicacion_ClickCancelar(object sender, EventArgs e)
        {
            //Ocultando ventana de reubicacion
            alternaVentanaModal("reubicacionUnidad", this);
        }


        #endregion

        #region Asignación Recursos

        /// <summary>
        /// Clic en botón Agregar Recuros a Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickAgregarRecurso(object sender, EventArgs e)
        {
            //Validando recurso por asignar
            RetornoOperacion resultado = validaVencimientosActivos("Unidad");

            //Si no hay vencimientos
            if (resultado.OperacionExitosa)
                asignaRecursoMovimiento();
            //Si hay vencimientos que no permiten continuar
            else
            {
                //Ocultando ventana modal de mov vacío
                alternaVentanaModal("asignacionRecursos", this);
                //Mostrando notificación de vencimientos
                alternaVentanaModal("indicadorVencimientos", this);
            }
        }
        /// <summary>
        /// Clic en botón quitar recurso asignado a movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickQuitarRecurso(object sender, EventArgs e)
        {
            //Realizando asignación de recursos
            RetornoOperacion resultado = wucAsignacionRecurso.DeshabilitaRecurso();

            //Si se agrega correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de unidades y sus asignaciones de movimiento
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Clic botón reubicar unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickReubicarRecurso(object sender, EventArgs e)
        {
            //Cerrando ventana de asignación de recurso
            alternaVentanaModal("asignacionRecursos", this);
            //inicializando control de mov vacío
            wucReubicacion.InicializaControl(wucAsignacionRecurso.idRecurso, ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                            Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionInicio.Manual, 
                            EstanciaUnidad.TipoActualizacionFin.Manual);
            //Abriendo ventana de movimiento vacío
            alternaVentanaModal("reubicacionUnidad", this);            
        }
        /// <summary>
        /// Clic en botón liberar recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickLiberarRecurso(object sender, EventArgs e)
        { 
            //Realizando la liberación de los recursos
            RetornoOperacion resultado = wucAsignacionRecurso.LiberarRecurso();

            //Si se ha liberado el recurso correctamente
            if(resultado.OperacionExitosa)
                //Actualizando lista de unidades y sus asignaciones de movimiento
                cargaUnidadesManteniendoSeleccion();
        }
        /// <summary>
        /// Realiza el proceso de adición de recurso al movimiento indicado
        /// </summary>
        private void asignaRecursoMovimiento()
        {
            //Realizando asignación de recursos
            RetornoOperacion resultado = wucAsignacionRecurso.AgregaAsignacionRecurso();

            //Si se agrega correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de unidades y sus asignaciones de movimiento
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultados
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el proceso de adición de recurso al movimiento indicado
        /// </summary>
        private void movimientoVacio()
        {
            //Creando movimiento vacío
            RetornoOperacion resultado = wucReubicacion.RegistraMovimientoVacioSinOrden();
            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Actualizando Gridview
                cargaUnidadesManteniendoSeleccion();
                //Ocultando ventana de reposicionamiento
                alternaVentanaModal("reubicacionUnidad", this);
            }

            //Mostrando error
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Inicio - Fin Servicio / Fin Mov. Vacío

        /// <summary>
        /// Click en algún botón de la ventana de dialogo de Salida y Llegada de Parada o Inicio y Fin de Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIngresoSalida_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //Determinando el comando a ejecutar
            switch (btn.CommandName)
            {
                case "Cancelar":
                    //Sin Acciones secundarias
                    break;
                case "Iniciar":
                    //Iniciando servicio
                    iniciaServicio();
                    break;
                case "Llegada":
                    //Instanciando movimiento actualmente seleccionado
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                    {
                        //Si existe el movimiento
                        if (mov.id_movimiento > 0)
                        {
                            //Si es movimiento de servicio
                            if (mov.id_servicio > 0)
                                //Actualizando llegada a parada correspondiente
                                actualizaLlegadaParada();
                            //Si es mov vacío
                            else
                                //Terminando mov vacío
                                terminaMovVacio();
                        }
                    }
                    break;
                case "Salida":
                    //Realizando el proceso de salida correspondiente
                    realizaProcesoSalida();
                    break;
                case "Terminar":
                    //Terminando servicio
                    realizaProcesoTerminoServicio();
                    break;
            }

            //Cerrando ventana modal correspondiente
            alternaVentanaModal("inicioFinMovimientoServicio", btn);
        }
        /// <summary>
        /// Configura el contenido de la ventana de actualziación de fechas (llegada, salida, inicio y fin)
        /// </summary>
        /// <param name="configuracion"></param>
        private void configuraVentanaActualizacionFecha(string configuracion)
        {
            //Determinando el tipo de configuracion a establecer
            switch (configuracion)
            {
                case "Iniciar":
                    //Instanciando servicio
                    using (Servicio srv = new Servicio(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])))
                    {
                        lblValorFechaSalida.Visible = false;
                        lblFechaSalida.Text = "";
                        lblEncabezadoActualizacionServMov.InnerText = "Inicio de Servicio";
                        lblTipoFechaCita.Text = "Cita Llegada";
                        lblFechaCita.Text = srv.cita_carga.ToString("dd/MM/yyyy HH:mm");
                        lblFechaActualizacion.Text = "Fecha Inicio Serv.";
                        ddlRazonLlegadaTarde.Enabled = true;
                        muestraRecursosMovimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]));
                    }
                    break;
                case "Terminar":
                    //Instanciando parada final
                    using (Parada parada = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])))
                    {
                        lblValorFechaSalida.Visible = false;
                        lblFechaSalida.Text = "";
                        lblEncabezadoActualizacionServMov.InnerText = "Terminar Servicio";
                        lblTipoFechaCita.Text = "Fecha Llegada";
                        lblFechaCita.Text = parada.fecha_llegada.ToString("dd/MM/yyyy HH:mm");
                        lblFechaActualizacion.Text = "Fecha Fin Serv.";
                        ddlRazonLlegadaTarde.Enabled = false;
                        muestraRecursosMovimiento(0);
                    }
                    break;
                case "Llegada":
                    //Instanciando movimiento actual
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                    {
                        //Si existe el movimiento
                        if(mov.id_movimiento > 0)
                        {
                            //Instanciando parada de destino
                            using (Parada destino = new Parada(mov.id_parada_destino), origen = new Parada(mov.id_parada_origen))
                            {
                                //Determinando si el movimiento es vacío o de servicio, si es de servicio...
                                if (mov.id_servicio > 0)
                                {
                                    lblEncabezadoActualizacionServMov.InnerText = "Llegada a Parada";
                                    lblTipoFechaCita.Text = "Cita Llegada";
                                    lblFechaCita.Text = destino.cita_parada.ToString("dd/MM/yyyy HH:mm");
                                    lblFechaActualizacion.Text = "Fecha Llegada";
                                    ddlRazonLlegadaTarde.Enabled = true;
                                }
                                //Si es vacío
                                else
                                {
                                    lblEncabezadoActualizacionServMov.InnerText = "Terminar Movimiento Vacío";                                   
                                    lblFechaActualizacion.Text = "Fecha Llegada";
                                    ddlRazonLlegadaTarde.Enabled = false;
                                  
                                }
                                 //Determinando si el movimiento es vacío o de servicio, si es de servicio...
                                if (mov.id_servicio > 0)
                                {
                                    lblValorFechaSalida.Visible = true;
                                    lblFechaSalida.Text = origen.fecha_salida.ToString("dd/MM/yyyy HH:mm");
                                }
                                else
                                {
                                    //Fechas de Salida
                                    lblTipoFechaCita.Text = "Fecha Salida";
                                    lblFechaCita.Text = origen.fecha_salida.ToString("dd/MM/yyyy HH:mm");
                                    lblValorFechaSalida.Visible = false;
                                    lblFechaSalida.Text = "";
                                }
                            }

                        }
                        muestraRecursosMovimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]));
                    }
                    break;
                case "Salida":
                    //Instanciando parada actual
                    using (Parada parada = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])))
                    {
                        lblValorFechaSalida.Visible = false;
                        lblFechaSalida.Text = "";
                        lblEncabezadoActualizacionServMov.InnerText = "Salida de Parada";
                        lblTipoFechaCita.Text = "Fecha Llegada";
                        lblFechaCita.Text = parada.fecha_llegada.ToString("dd/MM/yyyy HH:mm");
                        lblFechaActualizacion.Text = "Fecha Salida";
                        ddlRazonLlegadaTarde.Enabled = true;
                        muestraRecursosMovimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]));
                    }
                    break;
            }

            txtFechaActualizacion.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            //Limpiando razón de llegada tarde
            ddlRazonLlegadaTarde.SelectedValue = "0";
            //Indicando comando de botón aceptar
            btnAceptarIngresoSalida.CommandName = configuracion;
        }
        /// <summary>
        /// Inicia el servicio programado para la unidad
        /// </summary>
        private void iniciaServicio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la parada inicial del servicio
            using (Parada paradaServicio = new Parada(1, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])))
            {
                //Realizando la actualización de la parada
                resultado = paradaServicio.ActualizarFechaLlegada(Convert.ToDateTime(txtFechaActualizacion.Text), Parada.TipoActualizacionLlegada.Manual, Convert.ToByte(ddlRazonLlegadaTarde.SelectedValue),
                             EstanciaUnidad.TipoActualizacionInicio.Manual, ((Usuario)Session["usuario"]).id_usuario);
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
                //Actualizando Gridview de Unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarIngresoSalida, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza la fecha de llegada (y causa de posible retraso) a una parada
        /// </summary>
        private void actualizaLlegadaParada()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando movimiento
            using (Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
            {
                //Instanciando la parada actual
                using (Parada objParada = new Parada(mov.id_parada_destino))
                {
                    //Realizando la actualización de la parada
                    resultado = objParada.ActualizarFechaLlegada(Convert.ToDateTime(txtFechaActualizacion.Text), Parada.TipoActualizacionLlegada.Manual, Convert.ToByte(ddlRazonLlegadaTarde.SelectedValue),
                                 EstanciaUnidad.TipoActualizacionInicio.Manual, ((Usuario)Session["usuario"]).id_usuario);
                }
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
                //Actualizando Gridview de Unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarIngresoSalida, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza validaciones previas a la salida de los recursos de una parada
        /// </summary>
        private void realizaProcesoSalida()
        {
            //Validando eventos pendientes de actualizar
            RetornoOperacion resultado = ParadaEvento.ValidaEventosSinIniciar(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]), Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]));

            //Si no hay pendientes
            if (resultado.OperacionExitosa)
            {
                //Validando vencimientos del movimiento
                resultado = validaVencimientosActivos("Movimiento");
                //Si no hay vencimientos obligatorios
                if (resultado.OperacionExitosa)
                {
                    //Realizando Actualización de fecha de salida
                    actualizaFechaSalida();
                    //Cerrando ventana de confirmación de fecha de salida de parada
                    alternaVentanaModal("inicioFinMovimientoServicio", btnAceptarIngresoSalida);
                }
                //De lo contrario
                else
                {
                    //Ocultando ventana modal de salida
                    alternaVentanaModal("inicioFinMovimientoServicio", btnAceptarIngresoSalida);
                    //Mostrando notificación de vencimientos
                    alternaVentanaModal("indicadorVencimientos", btnAceptarIngresoSalida);
                }
            }
            //Si hay pendientes
            else
            {
                //Asignando valores de Configuración para la Salida
                btnNoActualizacionEventos.CommandName = "NoActualizarEnSalida";
                //Ocultando ventana modal de salida
                alternaVentanaModal("inicioFinMovimientoServicio", btnAceptarIngresoSalida);
                //Mostrando ventana modal de selección de actualización de eventos
                alternaVentanaModal("confirmacionEventosPendientes", btnAceptarIngresoSalida);
            }
        }
        /// <summary>
        ///  Método encargado de actualizar la Fecha de Salida de la Parada
        /// </summary>
        /// <returns></returns>
        private void actualizaFechaSalida()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando unidad 
            using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
            {
                //Obteniendo estancia actual de la unidad
                using (EstanciaUnidad estancia = new EstanciaUnidad(unidad.id_estancia))
                {
                    //Comparando parada actual de la unidad con parada por actualizar
                    if (estancia.id_parada == Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]))
                    {
                        //Instanciando la parada actual
                        using (Parada parada = new Parada(estancia.id_parada))
                        {
                            //Realizando la actualización de la parada
                            resultado = parada.ActualizarFechaSalida(Convert.ToDateTime(txtFechaActualizacion.Text), Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionFin.Manual, 
                                                                ParadaEvento.TipoActualizacionFin.Manual, Convert.ToByte(ddlRazonLlegadaTarde.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
                        }
                    }
                    else
                        resultado = new RetornoOperacion("La parada actual de la unidad no coincide con la parada por actualizar. Actualice la lista de Unidades.");
                }
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
                //Actualizando Gridview de Unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostramos Mensaje Error
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el proceso de gestión del termino del Viaje
        /// </summary>
        private void realizaProcesoTerminoServicio()
        {
            //Validando eventos pendientes de actualizar
            RetornoOperacion resultado = ParadaEvento.ValidaEventosSinIniciar(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]), Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]));

            //Si no hay pendientes
            if (resultado.OperacionExitosa)
            
                //Terminando Servicio
                terminaServicio();
            //Si hay pendientes
            else
            {
                //Asignando valores de Configuración para el termino del Viaje
                btnNoActualizacionEventos.CommandName = "NoActualizarEnTerminoViaje";
                //Ocultando ventana modal de salida
                alternaVentanaModal("inicioFinMovimientoServicio", btnAceptarIngresoSalida);
                //Mostrando ventana modal de selección de actualización de eventos
                alternaVentanaModal("confirmacionEventosPendientes", btnAceptarIngresoSalida);
            }
        }
        /// <summary>
        ///  Termina el servicio actual de la unidad
        /// </summary>
        /// <returns></returns>
        private void terminaServicio()
        {
            //Validando eventos pendientes de actualizar
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando unidad 
            using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
            {
                //Obteniendo estancia actual de la unidad
                using (EstanciaUnidad estancia = new EstanciaUnidad(unidad.id_estancia))
                {
                    //Comparando parada actual de la unidad con parada por actualizar
                    if (estancia.id_parada == Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]))
                    {
                        //Instanciando servicio por terminar
                        using (Servicio servicio = new Servicio(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"])))
                        {
                            //Validando Servicio
                            if (servicio.habilitar)
                                //Terminando el servicio actual
                                resultado = servicio.TerminaServicio(Convert.ToDateTime(txtFechaActualizacion.Text), Parada.TipoActualizacionSalida.Manual, ParadaEvento.TipoActualizacionFin.Manual, ((Usuario)Session["usuario"]).id_usuario);
                            else
                                resultado = new RetornoOperacion("No se puede recuperar el servicio.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("La parada final del servicio no coincide con la parada actual de la unidad. Actualice la lista de unidades.");
                }
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
                //Actualizando Gridview de Unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza la fecha de llegada al destino del movimiento, terminando al mismo y liberando recursos utilizados
        /// </summary>
        private void terminaMovVacio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //inicializando transacción
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando movimiento vacío
                using (SAT_CL.Despacho.Movimiento mov = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                {
                    //Si el movimiento se recuperó
                    if (mov.id_movimiento > 0)
                    {
                        //Instanciando parada de origen
                        using (SAT_CL.Despacho.Parada origen = new Parada(mov.id_parada_origen))
                        {
                            //Validamos fecha de salida de origen vs fecha llegada a destino
                            if (Convert.ToDateTime(txtFechaActualizacion.Text).CompareTo(origen.fecha_salida) > 0)
                                //Terminamos Movimiento
                                resultado = mov.TerminaMovimientoVacio(Convert.ToDateTime(txtFechaActualizacion.Text),
                                                EstanciaUnidad.TipoActualizacionInicio.Manual, ((Usuario)Session["usuario"]).id_usuario);
                            else
                                resultado = new RetornoOperacion(string.Format("La Llegada al Destino '{0:dd/MM/yyyy HH:mm}' debe ser Mayor a la Salida del Origen '{1:dd/MM/yyyy HH:mm}'", Convert.ToDateTime(txtFechaActualizacion.Text), origen.fecha_salida));
                        }
                    }
                    //Si no hay movimiento
                    else
                        resultado = new RetornoOperacion(string.Format("Movimiento '{0}' no encontrado.", gvUnidades.SelectedDataKey["Movimiento"]));
                }

                //Finalizando transacción
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Si no existe Error
            if (resultado.OperacionExitosa)
                //Actualizando Gridview de Unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarIngresoSalida, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza el kilometraje del movimiento y su servicio en caso de tenerlo
        /// </summary>
        private void actualizaKilometrajeMovimiento()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos nuestro movimiento 
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
            {
                //Validamos que el movimiento tenga un id de servicio ligado 
                if (objMovimiento.id_servicio != 0)
                {
                    //En caso de que el movimiento tenga un servicio ligado, instanciamos nuestro servicio
                    using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                    {
                        //Realizamos la actualizacion del kilometraje
                        resultado = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                }
                else
                {
                    //En caso de que el movimiento no tenga id de servicio ligado
                    //Invocamos el metodo de actualizacion de kilometraje del movimiento
                    resultado = Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }

                switch (resultado.IdRegistro)
                {
                    //Caso en el que no hubo cambios en el kilometraje
                    case -50:
                            //No hay tareas por realizar
                            break;
                    //Caso en el que no se encontro el kilometraje
                    case -100:
                            using (Parada origen = new Parada(objMovimiento.id_parada_origen), destino = new Parada(objMovimiento.id_parada_destino))
                            {
                                //Inicializando Control
                                wucKilometraje.InicializaControlKilometraje(0, objMovimiento.id_compania_emisor, origen.id_ubicacion, destino.id_ubicacion);
                                //Mostrando ventana de captura de kilometraje
                                alternaVentanaModal("kilometrajeMovimiento", gvUnidades);
                            }
                            break;
                    //Caso en el que se actualizo 
                    default:
                            //Actualizamos el grid de unidades
                            cargaUnidadesManteniendoSeleccion();
                            break;
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Vencimientos de Unidades

        /// <summary>
        /// Clic en botón Ver Historial de Vencimientos (ventana de notificación de vencimientos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerHistorialVencimientos_Click(object sender, EventArgs e)
        {
            //Ocultando ventana de notificación
            alternaVentanaModal("indicadorVencimientos", lkbVerHistorialVencimientos);
            //Inicializando control de vencimientos
            wucVencimientosHistorial.InicializaControl((TipoVencimiento.TipoAplicacion)Convert.ToByte(lkbVerHistorialVencimientos.CommandArgument), Convert.ToInt32(lkbVerHistorialVencimientos.CommandName));
            //Configurando botón de cierre para volver ventana de notificación tras el cierre de la ventana de historial
            lkbCerrarHistorialVencimientos.CommandArgument = "NotificacionVencimientos";
            //Mostrando ventana de historial de vencimientos
            alternaVentanaModal("historialVencimientos", lkbVerHistorialVencimientos);
        }
        /// <summary>
        /// Clic en Botón Aceptar (ventana de notificación de vencimientos)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarIndicadorVencimientos_Click(object sender, EventArgs e)
        {
            //Determinando el comando a ejecutar
            switch (((Button)sender).CommandName)
            {
                case "Obligatorio":                    
                    //Mostrando resultado
                    ScriptServer.MuestraNotificacion(btnAceptarIndicadorVencimientos, new RetornoOperacion(lblMensajeHistorialVencimientos.Text), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    break;
                case "Opcional":
                    //Determinando el nivel al que se está aplicando la consulta de vencimientos activos
                    switch (btnAceptarIndicadorVencimientos.CommandArgument)
                    {
                        case "Movimiento":
                            //Realizando la actualización de la parada
                            actualizaFechaSalida();
                            break;
                        case "Unidad":
                            //Realizando asignación de unidad
                            asignaRecursoMovimiento();
                            break;
                        case "Vacio":
                            //Realizando movimiento en vacío
                            movimientoVacio();
                            break;
                    }
                    break;
            }

            //Ocultando ventana de notificación de vencimientos
            alternaVentanaModal("indicadorVencimientos", btnAceptarIndicadorVencimientos);
        }
        /// <summary>
        /// Clic en botón Guardar Vencimiento (Ventana de actualización de vencimientos)
        /// </summary>
        protected void wucVencimiento_ClickGuardarVencimiento(object sender, EventArgs e)
        {
            //Realizando guardado de vencimiento
            RetornoOperacion resultado = wucVencimiento.GuardaVencimiento();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Si la solicitud de visualización de vencimientos fue por una actualización de salida
                if (lkbVerHistorialVencimientos.CommandArgument != "")
                    //Actualizando lista de vencimientos con datos de confirmación de vencimientos
                    wucVencimientosHistorial.InicializaControl((TipoVencimiento.TipoAplicacion)Convert.ToByte(lkbVerHistorialVencimientos.CommandArgument), Convert.ToInt32(lkbVerHistorialVencimientos.CommandName));
                //De lo contrario
                else
                {
                    //Instanciando vencimiento almacenado
                    using(Vencimiento v = new Vencimiento(resultado.IdRegistro))
                    //Se actualizará sobre la unidad involucrada en la inserción de vencimiento
                    wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, v.id_registro);
                }

                //Cerrando ventana de edición de vencimiento
                alternaVentanaModal("actualizacionVencimiento", this);
                //Abriendo ventana de historial de vencimientos
                alternaVentanaModal("historialVencimientos", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Clic en Botón Terminar Vencimiento (Ventana de actualización de vencimientos)
        /// </summary>
        protected void wucVencimiento_ClickTerminarVencimiento(object sender, EventArgs e)
        {
            //Realizando guardado de vencimiento
            RetornoOperacion resultado = wucVencimiento.TerminaVencimiento();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Si la solicitud de visualización de vencimientos fue por una actualización de salida
                if (lkbVerHistorialVencimientos.CommandArgument != "")
                    //Actualizando lista de vencimientos con datos de confirmación de vencimientos
                    wucVencimientosHistorial.InicializaControl((TipoVencimiento.TipoAplicacion)Convert.ToByte(lkbVerHistorialVencimientos.CommandArgument), Convert.ToInt32(lkbVerHistorialVencimientos.CommandName));
                //De lo contrario
                else
                {
                    //Instanciando vencimiento almacenado
                    using (Vencimiento v = new Vencimiento(resultado.IdRegistro))
                        //Se actualizará sobre la unidad involucrada en la inserción de vencimiento
                        wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Unidad, v.id_registro);
                }

                //Cerrando ventana de edición de vencimiento
                alternaVentanaModal("actualizacionVencimiento", this);
                //Abriendo ventana de historial de vencimientos
                alternaVentanaModal("historialVencimientos", this);
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Clic en botón consultar vencimiento (ventana historial de vencimientos)
        /// </summary>
        protected void wucVencimientosHistorial_lkbConsultar(object sender, EventArgs e)
        {
            //Inicializando control de vencimiento en modo consulta
            wucVencimiento.InicializaControl(wucVencimientosHistorial.id_vencimiento, false);
            //Cerrando ventana de historial de vencimientos
            alternaVentanaModal("historialVencimientos", this);
            //Mostrando ventana de actualización de vencimiento
            alternaVentanaModal("actualizacionVencimiento", this);
        }
        /// <summary>
        /// Clic en botón terminar, para apertura de vencimiento con privilegios de término (ventana historial de vencimientos)
        /// </summary>
        protected void wucVencimientosHistorial_lkbTerminar(object sender, EventArgs e)
        {
            //Inicializando control de vencimiento en modo consulta
            wucVencimiento.InicializaControl(wucVencimientosHistorial.id_vencimiento, true);
            //Cerrando ventana de historial de vencimientos
            alternaVentanaModal("historialVencimientos", this);
            //Mostrando ventana de actualización de vencimiento
            alternaVentanaModal("actualizacionVencimiento", this);
        }
        /// <summary>
        /// Clic en botón nuevo, para captura de vencimiento con privilegios de captura (ventana historial de vencimientos)
        /// </summary>
        protected void wucVencimientosHistorial_btnNuevoVencimiento(object sender, EventArgs e)
        {
            //Inicializando control de actualización de vencimiento para nuevo registro
            wucVencimiento.InicializaControl(19, wucVencimientosHistorial.id_recurso);
            //Cerrar ventana de historial de vencimientos
            alternaVentanaModal("historialVencimientos", this);
            //Abrir ventana de actualización de vencimiento
            alternaVentanaModal("actualizacionVencimiento", this);
        }
        /// <summary>
        /// Determina si la unidad solicitada a reposicionar tiene vencimientos activos y notifica al usuario de ello
        /// </summary>
        /// <param name="origen">Origen de conuslta de vencimientos: Movimiento, Vacio o Unidad</param>
        /// <returns></returns>
        private RetornoOperacion validaVencimientosActivos(string origen)
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Declarando tabla para almacenar vencimientos
            DataTable mitVencimientosGeneral = new DataTable();

            //Determinando el origen de carga de los vencimientos
            switch (origen)
            {
                case "Movimiento":
                    mitVencimientosGeneral = Vencimiento.CargaVencimientosActivosMovimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]),
                                                                                                Fecha.ObtieneFechaEstandarMexicoCentro());
                    break;
                case "Unidad":
                    mitVencimientosGeneral = Vencimiento.CargaVencimientosActivosRecurso((TipoVencimiento.TipoAplicacion)wucAsignacionRecurso.idTipoAsignacion, wucAsignacionRecurso.idRecurso,
                                                                                                Fecha.ObtieneFechaEstandarMexicoCentro());
                    break;
                case "Vacio":
                    //Validando unidad motriz
                    using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_unidad, wucReubicacion.fecha_inicio))
                    {
                        //Si hay vencimientos
                        if (mit != null)
                            //Añadiendo vencimientos a tabla principal
                            mitVencimientosGeneral.Merge(mit, true, MissingSchemaAction.Add);
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
                                mitVencimientosGeneral.Merge(mit, true, MissingSchemaAction.Add);
                        }
                    }

                    //Validando arrastre 1
                    using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_remolque1, wucReubicacion.fecha_inicio))
                    {
                        //Si hay vencimientos
                        if (mit != null)
                            //Añadiendo vencimientos a tabla principal
                            mitVencimientosGeneral.Merge(mit, true, MissingSchemaAction.Add);
                    }

                    //Validando arrastre 2
                    using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_remolque2, wucReubicacion.fecha_inicio))
                    {
                        //Si hay vencimientos
                        if (mit != null)
                            //Añadiendo vencimientos a tabla principal
                            mitVencimientosGeneral.Merge(mit, true, MissingSchemaAction.Add);
                    }

                    //Validando dolly
                    using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, wucReubicacion.id_dolly, wucReubicacion.fecha_inicio))
                    {
                        //Si hay vencimientos
                        if (mit != null)
                            //Añadiendo vencimientos a tabla principal
                            mitVencimientosGeneral.Merge(mit, true, MissingSchemaAction.Add);
                    }
                    break;
            }

            //Si hay recursos en el concentrado
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(mitVencimientosGeneral))
            {
                //Determinando si hay vencimientos obligatorios
                int obligatorios = (from DataRow r in mitVencimientosGeneral.Rows
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

                //Obteniendo primer recurso y tipo de recurso con vencimientos, dando prioridad a aquellos que son obligatorios
                Pair recurso = (from DataRow r in mitVencimientosGeneral.Rows
                                orderby r.Field<byte>("IdPrioridad") ascending
                                select new Pair(r.Field<int>("IdTipoRecurso"), r.Field<int>("IdRecurso"))).FirstOrDefault();

                //Guardandolo en el comando y argumento de vencimientos a mostrar
                lkbVerHistorialVencimientos.CommandArgument = recurso.First.ToString();
                lkbVerHistorialVencimientos.CommandName = recurso.Second.ToString();

                //Indicando nivel de validación de vencimientos
                btnAceptarIndicadorVencimientos.CommandArgument = origen;

                //Actualizando paneles de actualización necesarios (actualización con multiples origenes en controles de usuario y página)
                upimgAlertaVencimiento.Update();
                uplblMensajeHistorialVencimientos.Update();
                upbtnAceptarIndicadorVencimientos.Update();
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Vencimientos (Simplificado) Unidades

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucVencimientoSimp_ClickGuardarVencimiento(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Vencimiento
            result = wucVencimientoSimp.GuardaVencimiento();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Cargando Unidades
                cargaUnidadesManteniendoSeleccion();

                //Mostrando ventana correspondiente
                alternaVentanaModal("actualizacionVencimientoSimp", this.Page);
            }

            //Mostrando Resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucVencimientoSimp_ClickTerminarVencimiento(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Terminando Vencimiento
            result = wucVencimientoSimp.TerminaVencimiento();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Cargando Unidades
                cargaUnidadesManteniendoSeleccion();

                //Mostrando ventana correspondiente
                alternaVentanaModal("actualizacionVencimientoSimp", this.Page);
            }

            //Mostrando Resultado
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos de Parada

        /// <summary>
        /// Comando de botón de eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEvento_Click(object sender, EventArgs e)
        {
            //Determinando el comando a realziar
            switch (((Button)sender).CommandName)
            {
                /**** VENTANA DE CONFIRMACIÓN DE EVENTOS PENDIENTES  ****/
                case "Actualizar":
                    //Cerrando ventana modal de confirmación
                    alternaVentanaModal("confirmacionEventosPendientes", btnSiActualizacionEventos);
                    //Inicializando control de eventos de parada
                    wucParadaEvento.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]));
                    //Indicando que cargumento adicional para el cierre de la ventana eventos será ejecutado
                    lkbCerrarEventosParada.CommandArgument = "Salida";
                    //Mostrando ventana de eventos
                    alternaVentanaModal("eventosParada", btnSiActualizacionEventos);
                    break;
                case "NoActualizarEnSalida":
                    //Cerrando ventana modal de confirmación
                    alternaVentanaModal("confirmacionEventosPendientes", btnNoActualizacionEventos);
                    //Continuar con acción de salida de parada
                    actualizaFechaSalida();
                    break;
                case "NoActualizarEnTerminoViaje":
                    //Cerrando ventana modal de confirmación
                    alternaVentanaModal("confirmacionEventosPendientes", btnNoActualizacionEventos);
                    //Continuar con acción de salida de parada
                    terminaServicio();
                    break;
            }
        }
        /// <summary>
        /// Click en botón Actualizar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnActualizarClick(object sender, EventArgs e)
        { 
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.GuardaEvento();
            
            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {
                //Determinando el origen de apertura de ventana de eventos
                switch (lkbCerrarEventosParada.CommandArgument)
                { 
                    case "EventoActual":
                            //Se cierra ventana de eventos
                            alternaVentanaModal("eventosParada", this);
                        break;
                    case "Eventos":
                    case "Salida":
                        //Sin acciones, se debe cerrar manualmente la ventana
                        break;
                }                

                //Actualizando lista de unidades
                cargaUnidadesManteniendoSeleccion();                
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón Cancelar edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnCancelarClick(object sender, EventArgs e)
        {
            //Realizando cancelación de edición de evento
            wucParadaEvento.CancelarActualizacion();
                       
            //Determinando el origen de apertura de ventana de eventos
            switch (lkbCerrarEventosParada.CommandArgument)
            {
                case "EventoActual":
                        //Se cierra ventana de eventos
                        alternaVentanaModal("eventosParada", this);
                    break;
                case "Eventos":
                case "Salida":
                    //Sin acciones, se debe cerrar manualmente la ventana
                    break;
            }
        }
        /// <summary>
        /// Click en botón Nuevo Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnNuevoClick(object sender, EventArgs e)
        { 
            //Realizando inserción de nuevo evento
            RetornoOperacion resultado = wucParadaEvento.NuevoEvento();            
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón eliminar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnlkbEliminarClick(object sender, EventArgs e)
        {
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.EliminaEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
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
            {
                //Determinando quien abrio la ventana
                if (lkbCerrarReferencias.CommandArgument == "Pendientes")
                    cargaServiciosPendientesManteniendoSeleccion();
                else
                    //Actualizando Gridview
                    cargaUnidadesManteniendoSeleccion();
            }

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
            {
                //Determinando quien abrio la ventana
                if (lkbCerrarReferencias.CommandArgument == "Pendientes")
                    cargaServiciosPendientesManteniendoSeleccion();
                else
                    //Actualizando Gridview
                    cargaUnidadesManteniendoSeleccion();
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion
                
        #region Kilometraje

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
                //Instanciamos nuestro movimiento 
                using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"])))
                {
                    //Si el movimiento pertenece a un servicio
                    if (objMovimiento.id_servicio > 0)
                    {
                        //Instanciamos nuestro servicio
                        using (Servicio objServicio = new Servicio(objMovimiento.id_servicio))
                            //Realizamos la actualizacion del kilometraje
                            resultado = objServicio.CalculaKilometrajeServicio(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    //En caso contrario
                    else
                        //Actualizando kilometraje de de movimiento
                        resultado = Movimiento.ActualizaKilometrajeMovimiento(objMovimiento.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                }

                //Si no hay error
                if (resultado.OperacionExitosa)
                {
                    //Cerrando ventana de captura de kilometraje
                    alternaVentanaModal("kilometrajeMovimiento", this);
                    //Actualizando grid de unidades
                    cargaUnidadesManteniendoSeleccion();
                }
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Evaluacion Ubicacion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            //Guargando Incidencia de GPS
            RetornoOperacion result = new RetornoOperacion();

            //Validando Unidad
            result = validaUnidad(Convert.ToInt32(ddlServicioGPS.SelectedValue));

            //Validando Resultado
            if (result.OperacionExitosa)

                //Cargando Unidades Despacho
                cargaUnidadesManteniendoSeleccion();

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this.Page, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalidaETA_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            Button btn = (Button)sender;

            //Mostrando Ventana Modal
            alternaVentanaModal("ResultadoETA", btn);

            //Validando Comando
            switch (btn.CommandName)
            {
                case "Salida":
                    {
                        //Configurando ventana de actualización de fecha para salida de parada
                        configuraVentanaActualizacionFecha("Salida");
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", btnSalidaETA);
                        break;
                    }
                case "Iniciar":
                    {
                        //Configurando ventana de actualización de fecha para inicio de servicio
                        configuraVentanaActualizacionFecha("Iniciar");
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", btnSalidaETA);
                        break;
                    }
                case "Nuevo":
                    {
                        //Ocultando ventana de selección
                        alternaVentanaModal("seleccionServicioMovimiento", btnSalidaETA);
                        break;
                    }
                case "Llegada":
                    {
                        //Si hay servicio asignado
                        if (Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]) > 0)
                        {
                            //Configurando ventana de actualización de fecha para llegada a parada
                            configuraVentanaActualizacionFecha("Llegada");
                            //Abriendo ventana de confirmación de llegada a primer parada de servicio
                            alternaVentanaModal("inicioFinMovimientoServicio", btnSalidaETA);
                        }
                        //Si hay movimiento vacío (reposicionamiento)
                        else
                        {
                            //Configurando ventana para término de mov vacío
                            configuraVentanaActualizacionFecha("Llegada");
                            //Abriendo modal de confirmación
                            alternaVentanaModal("inicioFinMovimientoServicio", btnSalidaETA);
                        }
                        break;
                    }
                case "Terminar":
                    {
                        //Configurando ventana de actualización de fecha para fin de servicio
                        configuraVentanaActualizacionFecha("Terminar");
                        //Abriendo ventana de confirmación de llegada a primer parada de servicio
                        alternaVentanaModal("inicioFinMovimientoServicio", btnSalidaETA);
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Ver Historial"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbResultadoETA_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Control
            switch(lkb.CommandName)
            {
                case "Historial":
                    {
                        //Cambiando de Vista a Historial
                        mtvEvaluacion.ActiveViewIndex = 1;

                        //Configurando Controles
                        chkIncluirETA.Checked = true;
                        txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
                        txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

                        //Invocando Método de Carga
                        cargaHistorialEvaluacion();
                        break;
                    }
                case "Resultado":
                    {
                        //Mostrando Resultado
                        muestraResultadoETA(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), 
                                                     Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]),
                                                     Convert.ToInt32(gvUnidades.SelectedDataKey["*IdEvaluacion"]), 
                                                     Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParadaDestino"]));
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoETA_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            Controles.CambiaTamañoPaginaGridView(gvEvaluaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoETA.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarETA_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvaluaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvEvaluaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvaluaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            Controles.CambiaSortExpressionGridView(gvEvaluaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbConsultar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvEvaluaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvEvaluaciones, sender, "lnk", false);

                //
                ScriptServer.MuestraNotificacion(this.Page, muestraEvaluacionConsulta(Convert.ToInt32(gvEvaluaciones.SelectedDataKey["Id"])), ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarETA_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            cargaHistorialEvaluacion();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializando contenido de Controles
        /// </summary>
        private void inicializaForma()
        {
            //asignaSesionUnica();
            //Limpiando controles
            txtNoUnidad.Text =
            txtUbicacion.Text = "";
            cargaCatalogos();
            //Inicializando contenido de GV con carga de unidades
            cargaUnidadesDespacho();
        }
        /// <summary>
        /// Método encargado de Asignar el nombre de la Sesion
        /// </summary>
        private void asignaSesionUnica()
        {
            if (!string.IsNullOrEmpty(_nombre_pagina))
                Session["FormaWeb"] = _nombre_pagina;
            else
                Session["FormaWeb"] = "";
        }
        /// <summary>
        /// Método encargado de Validar que el sistema se encuentre en una sola ventana
        /// </summary>
        private void validaSesionUnica()
        {
            //Validando Página
            if (Session["FormaWeb"] != null)
            {
                if (!string.IsNullOrEmpty(Session["FormaWeb"].ToString()))
                {
                    if (!(_nombre_pagina.Equals(Session["FormaWeb"].ToString())))
                    {
                        string mensaje = "";
                        XDocument validacionXML = XDocument.Load(Server.MapPath("~/XML/ValidacionFormas.xml"));
                        if (validacionXML != null)
                        {
                            
                            
                            

                        }

                        if (!string.IsNullOrEmpty(mensaje))
                        {
                            string script = @"alert('" + _nombre_pagina + @"'); window.top.close();";
                            //Registrando Script
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ValidaSesionUnica", script, true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Método encargado de Cargar el Script de Configuración
        /// </summary>
        private void cargaScriptEncabezadoUnidades()
        {
            //Creando Script de Configuración
            string script = @"<script type='text/javascript'>
                                //Añadiendo Encabezado Fijo
                                $('#"+ gvUnidades.ClientID +@"').gridviewScroll({
                                    width: document.getElementById('contenedorUnidades').offsetWidth - 15,
                                    height: 400
                                });
                            </script>";

            //Ejecutando Script
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ConfiguraEncabezado", script, false);
        }
        /// <summary>
        /// Carga de catálogos requeridos3182
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando catálogo de Estatus de unidades
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(lbxEstatus, "Todos", 53);            
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoUnidades, "", 3182);
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoETA, "", 3182);
            //Razón Llegada Tarde a parada
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRazonLlegadaTarde, "Ninguna", 21);
            //Cargando catálogo de Flotas por Usuario
            DataTable dtCatalogo =  CapaNegocio.m_capaNegocio.CargaCatalogo(193, "", ((UsuarioSesion)Session["usuario_sesion"]).id_usuario, "", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            //Validando que existen Registros
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCatalogo))
            {
                Controles.CargaListBox(lbxFlota, dtCatalogo, "id", "descripcion");                
            }
            else
                Controles.InicializaListBox(lbxFlota, "NINGUNO");
            lbxFlota.SelectedIndex = 0;
        }
        /// <summary>
        /// Realiza la búsqueda de unidades y su asignación a servicio o movimiento actual
        /// </summary>
        private void cargaUnidadesDespacho()
        {
            //Declarando parámetro para identificar el tipo de unidades a mostrar como principales
            string tipo_unidad = "";

            //Determinando el tipo de unidades a mostrar
            //Si se solicita unidades motrices
            if (rdbUnidadMotriz.Checked)
                tipo_unidad = "Motriz";
            //Si se solicita unidades de arrastre
            if (rdbUnidadArrastre.Checked)
                tipo_unidad = "Arrastre";

            //Obteniendo estatus de pago
            string id_estatus = Controles.RegresaSelectedValuesListBox(lbxEstatus, "{0},", true, false);
            //Obteniendo estatus de pago
            string id_flota = Controles.RegresaSelectedValuesListBox(lbxFlota, "{0},", true, false);

            //Cargando Unidades
            using (DataTable mit = Reportes.CargaDespachoSimplificadoUnidades(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoUnidad.Text,
                                       id_estatus.Length > 1 ? id_estatus.Substring(0, id_estatus.Length - 1) : "", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1)), 
                                       tipo_unidad,chkUnidadesPropias.Checked,chkUnidadesNoPropias.Checked, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), id_flota.Length > 1 ? id_flota.Substring(0, id_flota.Length - 1) : ""))
            {
                //Llenando gridview
                Controles.CargaGridView(gvUnidades, mit, "*IdUnidad-IdProveedorWS-IdProveedorUnidadWS-*IdServicio-Movimiento-*IdParada-*IdEventoActual-*IdOperador-*IdVencimiento-*IdEvaluacion-*IdParadaDestino", lblOrdenarUnidades.Text, true, 4);
                //Guardando en sesión el origen de datos
                if (mit != null)
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                else
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Quitando selecciones de fila existentes
            Controles.InicializaIndices(gvUnidades);
        }
        /// <summary>
        /// Realiza la carga de las unidades manteniendo una selección de registro previa
        /// </summary>
        private void cargaUnidadesManteniendoSeleccion()
        {
            //Obteniendo el registro seleccionado actualmente
            string id_registro_seleccion = gvUnidades.SelectedIndex > -1 ? gvUnidades.SelectedDataKey["*IdUnidad"].ToString() : "";
            //Cargando Gridview
            cargaUnidadesDespacho();
            //Restableciendo selección en caso de ser necesario
            if (id_registro_seleccion != "")
                Controles.MarcaFila(gvUnidades, id_registro_seleccion, "*IdUnidad", "*IdUnidad-IdProveedorWS-IdProveedorUnidadWS-*IdServicio-Movimiento-*IdParada-*IdEventoActual-*IdOperador-*IdVencimiento-*IdEvaluacion-*IdParadaDestino", (DataSet)Session["DS"], "Table", lblOrdenarUnidades.Text, Convert.ToInt32(ddlTamanoUnidades.SelectedValue), true, 4);
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
                case "historialBitacora":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialBitacoraModal", "historialBitacora");
                    break;
                case "bitacoraMonitoreo":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "bitacoraMonitoreoModal", "bitacoraMonitoreo");
                    break;
                case "seleccionServicioMovimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionServicioMovimientoModal", "seleccionServicioMovimiento");
                    break;
                case "asignacionRecursos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "asignacionRecursosModal", "asignacionRecursos");
                    break;
                case "copiaServicioMaestro":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "copiaServicioMaestroModal", "copiaServicioMaestro");
                    break;
                case "reubicacionUnidad":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "reubicacionUnidadModal", "reubicacionUnidad");
                    break;
                case "movimientosVacio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "movimientosVacioModal", "movimientosVacio");
                    break;
                case "inicioFinMovimientoServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "inicioFinMovimientoServicioModal", "inicioFinMovimientoServicio");
                    break;
                case "indicadorVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "indicadorVencimientosModal", "indicadorVencimientos");
                    break;
                case "historialVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialVencimientosModal", "historialVencimientos");
                    break;
                case "actualizacionVencimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "actualizacionVencimientoModal", "actualizacionVencimiento");
                    break;
                case "actualizacionVencimientoSimp":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "actualizacionVencimientoSimplificadoModal", "actualizacionVencimientoSimplificado");
                    break;
                case "confirmacionEventosPendientes":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionEventosPendientesModal", "confirmacionEventosPendientes");
                    break;
                case "eventosParada":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "eventosParadaModal", "eventosParada");
                    break;
                case "referenciasServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "referenciasServicioModal", "referenciasServicio");
                    break;
                case "kilometrajeMovimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "kilometrajeMovimientoModal", "kilometrajeMovimiento");
                    break;
                case "documentacionServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "documentacionServicioModal", "documentacionServicio");
                    break;
                case "servicioDocumentadoPendiente":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "asignacionServicioDocumentadoModal", "asignacionServicioDocumentado");
                    break;
                case "CambioOperador":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalCambioOperador", "confirmacionCambioOperador");
                    break;
                case "PublicacionUnidad":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "modalPublicar", "confirmacionPublicar");
                    break;
                case "RequisicionesServicio":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaRequisiciones", "ventanaRequisiciones");
                    break;
                case "AltaRequisicion":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaAltaRequisicion", "ventanaAltaRequisicion");
                    break;
                case "ProveedorGPS":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaProveedorGPS", "ventanaProveedorGPS");
                    break;
                case "VentanaETA":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaETA", "ventanaETA");
                    break;
                case "ResultadoETA":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaResultadoValidacion", "ventanaResultadoValidacion");
                    break;
                case "impresionPorte":
                    ScriptServer.AlternarVentana(control, control.GetType(), nombre_ventana, "contenedorVentanaImpresionPorte", "ventanaImpresionPorte");
                    break;
            }
        }

        #endregion     

        #region Cambio de Operador
        /// <summary>
        /// Método encargado de Registrar el Cambio de Operador
        /// </summary>
        protected void wucCambioOperador_ClickRegistrar(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Realizamos el Cambio de Operador
           resultado = wucCambioOperador.CambioOperador();

            //Validamos Resultado
            if(resultado.OperacionExitosa)
                //Cerramos Ventana Modal
                alternaVentanaModal("CambioOperador", this);
            //CArga las unidades para verificar si contienen operador asignado
            cargaUnidadesDespacho();

            //Mostrando mensaje de resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Documentación

        /// <summary>
        /// Agrega una parada al servicio que se está documentado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_ImbAgregarParada_Click(object sender, EventArgs e)
        {
            //Guardando parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaParadaServicio();

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Si se realizó una nueva documentación
                if (wucServicioDocumentacion.nueva_documentacion)
                {
                    //Instanciando unidad implicada
                    using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
                    {
                        //Recuperando primer movimiento del servicio copiado
                        using (Movimiento mov = new Movimiento(wucServicioDocumentacion.id_servicio, 1))
                        {
                            //Si el movimiento fue recuperado
                            if (mov.id_movimiento > 0)
                                //Realizando la asignación del Recuros 
                                resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursoParaDespacho(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                           mov.id_movimiento, mov.id_parada_origen, EstanciaUnidad.Tipo.Operativa, EstanciaUnidad.TipoActualizacionInicio.Manual, MovimientoAsignacionRecurso.Tipo.Unidad,
                                           unidad.id_tipo_unidad, unidad.id_unidad, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                resultado = new RetornoOperacion("No pudo ser recuperado el primer movimiento del servicio documentado.");
                        }
                    }                    
                }
            }

            //Si se insertó correctamente la asignación y/o parada 
            if (resultado.OperacionExitosa)
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Elimina una parada de servicio y sus dependiencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbEliminarParada_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.EliminaParadaServicio();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Agrega un evento y/o producto a la parada activa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_ImbAgregarProducto_Click(object sender, EventArgs e)
        {
            //Guardando producto de parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaProductoEvento();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Elimina un producto y sus dependencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbEliminarProducto_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.EliminaProductoEvento();

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Se actualiza contenido de gridview de unidades
                cargaUnidadesManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Actualiza el encabezado del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_BtnAceptarEncabezado_Click(object sender, EventArgs e)
        {
            //Eliminando parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaEncabezadoServicio();

            //Si no hay errores
            if (resultado.OperacionExitosa)
            {
                //Se actualiza contenido de gridview de unidades
                cargaUnidadesManteniendoSeleccion();
                //Cerrando ventana modal
                alternaVentanaModal("documentacionServicio", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        /// <summary>
        /// Click en fechas de cita de parada de servicio (documentación)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_LkbCitasEventos_Click(object sender, EventArgs e)
        {
            //Cerrando ventana de documentación
            alternaVentanaModal("documentacionServicio", this);

            //Inicializando control de eventos de parada
            wucParadaEvento.InicializaControl(wucServicioDocumentacion.id_parada);
            //Indicando que cargumento adicional para el cierre de la ventana eventos será ejecutado
            lkbCerrarEventosParada.CommandArgument = "Documentacion";
            //Mostrando ventana de eventos
            alternaVentanaModal("eventosParada", this);
        }

        #endregion

        #region Servicios Documentados Pendientes

        /// <summary>
        /// Realiza la carga de los servicios documentados pendientes de asignación
        /// </summary>
        private void cargaServiciosPendientes()
        { 
            //Inicialziando indice de selección
            Controles.InicializaIndices(gvServiciosPendientes);
            //Cargando servicios
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaPlaneacionServicios(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, "", true, true,
                                    DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, "","",""))
            { 
                //Declarando tabla de filtrado de servicios
                DataTable mitFiltrada = null;

                //Si hay registros por filtrar
                if (mit != null)
                {
                    //Adoptando esquema de origen de datos inicial
                    mitFiltrada = mit.Clone();

                    //Instanciando parada actual de unidad
                    using (Parada parada_unidad = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])))
                    {
                        //Filtrando por estatus de servicio (Documentados)
                        DataRow[] filtrados = (from DataRow s in mit.Rows
                                               where new Servicio(s.Field<int>("id_servicio")).estatus == Servicio.Estatus.Documentado 
                                               select s).ToArray();

                        //Si hay elementos que filtrar
                        if(filtrados.Length > 0)
                        //Filtrando por coincidencia de origen de movimiento (Ubicación)
                        filtrados = (from DataRow s in filtrados
                                               where new Parada(s.Field<int>("IdParadaInicial")).id_ubicacion == parada_unidad.id_ubicacion
                                               select s).ToArray();

                        //Si hay elementos resultantes
                        if (filtrados.Length > 0)
                            //Pasar registros filtrados a tabla
                            mitFiltrada = OrigenDatos.ConvierteArregloDataRowADataTable(filtrados);

                        /* DONE: Se cambia método de filtrado, para mejor performance
                        //Para cada una de las filas recuperadas
                        foreach (DataRow s in mit.Rows)
                        {
                            //Recuperando Id de Parada actual del servicio
                            int id_parada_inicial = 0;

                            //Instanciando servicio
                            using (Servicio srv = new Servicio(s.Field<int>("id_servicio")))
                            {
                                //En base al estatus del servicio
                                switch (srv.estatus)
                                {
                                    case Servicio.Estatus.Documentado:
                                        id_parada_inicial = s.Field<int>("IdParadaInicial");
                                        break;
                                    case Servicio.Estatus.Iniciado:
                                        id_parada_inicial = s.Field<int>("id_parada_actual");
                                        break;
                                }
                            }

                            //Instanciando parada activa del servicio por asignar
                            using (Parada parada_servicio = new Parada(id_parada_inicial))
                                //Si coincide la ubicación de la unidad y la parada actual del servicio
                                if (parada_unidad.id_ubicacion == parada_servicio.id_ubicacion)
                                    //Importando fila a nueva tabla
                                    mitFiltrada.ImportRow(s);
                        }*/
                    }
                }

                //Llenando control Gridview
                Controles.CargaGridView(gvServiciosPendientes, mitFiltrada, "id_servicio-movimiento-id_parada_actual-IdParadaInicial", lblOrdenadoPorServiciosPendientes.Text, true, 2);
                //Si hay registros
                if (mitFiltrada != null)
                    //Guardando origen de datos
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitFiltrada, "Table1");
                else
                    //Borrando origen de datos
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
            }
        }
        /// <summary>
        /// Realiza la carga de los servicios documentados pendientes de asignación sin reiniciar indice de selección de registro
        /// </summary>
        private void cargaServiciosPendientesManteniendoSeleccion()
        {
            //Manteniendo Id de selección
            string id_servicio = gvServiciosPendientes.SelectedDataKey["id_servicio"].ToString();
            //Cargando Elementos
            cargaServiciosPendientes();
            //Buscando selección previa
            Controles.MarcaFila(gvServiciosPendientes, id_servicio, "id_servicio", "id_servicio-movimiento-id_parada_actual-IdParadaInicial", (DataSet)Session["DS"], "Table1", "", gvServiciosPendientes.PageSize, true, 2);
        }
        /// <summary>
        /// Cambio de página en lista de servicios pendietes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvServiciosPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Enlace a datos de filas de GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvServiciosPendientes.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Encontrando controles de interés
                    using (LinkButton lkbAsignar = (LinkButton)e.Row.FindControl("lkbAsignarUnidad"))
                    {
                        /**** APLICANDO CONFIGURACIÓN DE ASIGNACIÓN ****/
                        //Recuperando Id de Parada actual del servicio
                        int id_parada_actual_servicio = row.Field<int>("id_parada_actual");
                        //Recuperando Id de Movimiento
                        int id_movimiento = row.Field<int>("movimiento");

                        //Instanciando movimiento
                        using (Movimiento mov = new Movimiento(id_movimiento))
                        { 
                            //Si el movimiento existe
                            if (mov.id_movimiento > 0 && mov.habilitar)
                            { 
                                //En base al estatus del mismo
                                switch (mov.EstatusMovimiento)
                                { 
                                    case Movimiento.Estatus.Registrado:
                                        //Si la parada actual es 0
                                        if (id_parada_actual_servicio == 0)
                                            id_parada_actual_servicio = mov.id_parada_origen;
                                        break;
                                    case Movimiento.Estatus.Terminado:
                                        id_parada_actual_servicio = 0;
                                        break;
                                }
                            }
                        }
                        //Instanciando parada actual de unidad y parada activa del servicio por asignar
                        using(Parada parada_unidad = new Parada(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"])),
                            parada_servicio = new Parada(id_parada_actual_servicio))
                        //Si no coincide la ubicación de la unidad y la parada actual del servicio
                        if (parada_unidad.id_ubicacion != parada_servicio.id_ubicacion)
                            //Ocultando link de asignación
                            lkbAsignar.Visible = false;
                        //Si las ubicaciones son las mismas
                        else
                            //Ocultando link de asignación
                            lkbAsignar.Visible = true;
                    }

                    /**** APLICANDO SEMAFOREO EN BASE A ESTATUS DE CITAS DE CARGA Y DESCARGA ****/
                    using (Image imgEstatusDescarga = (Image)e.Row.FindControl("imgSemaforoDescarga"),
                        imgEstatusCarga = (Image)e.Row.FindControl("imgSemaforoCarga"))
                    {
                        //Aplicando criterio de visibilidad
                        imgEstatusCarga.Visible =
                        imgEstatusDescarga.Visible = true;

                        //Dependiendo el estatus de carga
                        switch (row["SemaforoCitaCarga"].ToString())
                        {
                            case "Verde":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_verde.png";
                                imgEstatusCarga.ToolTip = "Cita en Tiempo";
                                break;
                            case "Amarillo":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_naranja.png";
                                imgEstatusCarga.ToolTip = "Retrazo en Cita";
                                break;
                            case "Rojo":
                                imgEstatusCarga.ImageUrl = "~/Image/semaforo_rojo.png";
                                imgEstatusCarga.ToolTip = "Cita por Vencer";
                                break;
                            case "OK":
                            case "":
                                imgEstatusCarga.ImageUrl = "~/Image/Entrada.png";
                                imgEstatusCarga.ToolTip = "Llegada en Tiempo";
                                break;
                            case "TACHE":
                                imgEstatusCarga.ImageUrl = "~/Image/Salida.png";
                                imgEstatusCarga.ToolTip = "Llegada Tarde";
                                break;
                            default:
                                imgEstatusCarga.ImageUrl = "";
                                imgEstatusCarga.ToolTip = "";
                                imgEstatusCarga.Visible = false;
                                break;
                        }

                        //Dependiendo el estatus de descarga
                        switch (row["SemaforoCitaDescarga"].ToString())
                        {
                            case "Verde":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_verde.png";
                                imgEstatusDescarga.ToolTip = "Cita en Tiempo";
                                break;
                            case "Amarillo":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_naranja.png";
                                imgEstatusDescarga.ToolTip = "Retrazo en Cita";
                                break;
                            case "Rojo":
                                imgEstatusDescarga.ImageUrl = "~/Image/semaforo_rojo.png";
                                imgEstatusDescarga.ToolTip = "Cita por Vencer";
                                break;
                            case "OK":
                            case "":
                                imgEstatusDescarga.ImageUrl = "~/Image/Entrada.png";
                                imgEstatusDescarga.ToolTip = "Llegada en Tiempo";
                                break;
                            case "TACHE":
                                imgEstatusDescarga.ImageUrl = "~/Image/Salida.png";
                                imgEstatusDescarga.ToolTip = "Llegada Tarde";
                                break;
                            default:
                                imgEstatusDescarga.ImageUrl = "";
                                imgEstatusDescarga.ToolTip = "";
                                imgEstatusDescarga.Visible = false;
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Click en algún botón del gridview de servicios pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionServicioPendiente_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServiciosPendientes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServiciosPendientes, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {                   
                    case "Referencia":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServiciosPendientes.SelectedDataKey["id_servicio"]));
                        //Realizando cierre de servicios pendientes
                        alternaVentanaModal("servicioDocumentadoPendiente", lkb);
                        //Asignando argumento auxiliar de cerrado de ventana
                        lkbCerrarReferencias.CommandArgument = "Pendientes";
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("referenciasServicio", lkb);
                        break;
                    case "Asignaciones":                        
                        //Obteniendo recurso seleccionado
                        using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
                        {
                            //Instanciando servicio
                            using (Servicio srv = new Servicio(Convert.ToInt32(gvServiciosPendientes.SelectedDataKey["id_servicio"])))
                            {
                                RetornoOperacion resultado= new RetornoOperacion();

                                //Validando movimiento mayor a 0
                                if (gvServiciosPendientes.SelectedDataKey["movimiento"].ToString() != "0")
                                {
                                    //Asignando recurso
                                    resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursoParaDespacho(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                    Convert.ToInt32(gvServiciosPendientes.SelectedDataKey["movimiento"]), srv.estatus == Servicio.Estatus.Documentado ? Convert.ToInt32(gvServiciosPendientes.SelectedDataKey["IdParadaInicial"]) : Convert.ToInt32(gvServiciosPendientes.SelectedDataKey["id_parada_actual"]),
                                                                    EstanciaUnidad.Tipo.Operativa, EstanciaUnidad.TipoActualizacionInicio.Manual, MovimientoAsignacionRecurso.Tipo.Unidad, unidad.id_tipo_unidad, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]), ((Usuario)Session["usuario"]).id_usuario);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Cerrando ventana modal
                                        alternaVentanaModal("servicioDocumentadoPendiente", lkb);
                                        //Cargando lista de unidades
                                        cargaUnidadesManteniendoSeleccion();
                                    }
                                }
                                else
                                    resultado = new RetornoOperacion("No hay un movimiento al cual asignar el recurso.");

                                //Mostrando resultado
                                ScriptServer.MuestraNotificacion(lkb, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botón exportar servicios pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServiciosPendientes_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "".ToArray());
        }
        /// <summary>
        /// Maneja el ordenamiento del GV de Servicios Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoPorServiciosPendientes.Text = Controles.CambiaSortExpressionGridView(gvServiciosPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 2);
        }

        #endregion
        
        /// <summary>
        /// Evento que mostrara la ficha tecnica del operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbOperador_Click(object sender, ImageClickEventArgs e)
        {
            //Valida que existan registros en el gridview
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Selecciona el renglon del gridview a consultar
                Controles.SeleccionaFila(gvUnidades, sender, "imb", false);
                //Obtiene el operador
                int idOperador =Convert.ToInt32(gvUnidades.SelectedDataKey["*IdOperador"]);
                //Valida que exista el operador
                if (idOperador != 0)
                {
                    //Asigna valores a la variable url con la ruta de ubicacion de la ventana Abrir registros con los datos obtenidos de la tabla bancos
                    string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/DespachoSimplificado.aspx", "~/Accesorios/DatosOperador.aspx?idOp=" + idOperador);
                    //Define las dimensiones de la ventana Abrir registros de banco
                    string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=905,height=625";
                    //Invoca al método de la variable ScripServer que abrira una nueva ventana con los registros pertenecientes a la tabla Bancos
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir Registro Banco", configuracion, Page);
                }
                //Si la unidad no tiene operador asignado
                else
                {
                    //Inicializamos Control Cambio operador con la unidad 
                    wucCambioOperador.InicializaControl(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]));
                    //Mostrando ventana de cambio de operador
                    alternaVentanaModal("CambioOperador", this);
                }
            }
        }

        #region Proveedor GPS

        /// <summary>
        /// Método encargado de Validar la Unidad
        /// </summary>
        /// <param name="id_antena_gps">Proveedor de GPS</param>
        /// <returns></returns>
        private RetornoOperacion validaUnidad(int id_antena_gps)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que este Seleccionada una Unidad
            if (gvUnidades.SelectedIndex != -1)
            {
                //Instanciando Unidad
                using (Unidad unidad = new Unidad(Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"])))
                {
                    //Instanciando Unidad
                    if (unidad.habilitar)
                    {
                        //Declarando Objetos de Salida
                        SqlGeography ubicacion_gps_unidad = SqlGeography.Null;
                        int id_evaluacion = 0;

                        //Evaluando Unidad
                        result = unidad.EvaluaUnidadGPS(Convert.ToInt32(id_antena_gps),
                                                        Convert.ToInt32(gvUnidades.SelectedDataKey["*IdServicio"]),
                                                        Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]),
                                                        Convert.ToInt32(gvUnidades.SelectedDataKey["Movimiento"]),
                                                        Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParadaDestino"]),
                                                        out ubicacion_gps_unidad, out id_evaluacion,
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Invocando Método de Resultado
                            result = muestraResultadoETA(unidad.id_unidad, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParada"]), 
                                                         id_evaluacion, Convert.ToInt32(gvUnidades.SelectedDataKey["*IdParadaDestino"]));

                            //Mostrando Ventana Modal
                            alternaVentanaModal("ResultadoETA", this.Page);
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Unidad");
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existe una Unidad Seleccionada");

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encaragdo de Mostrar el Resultado de la ETA
        /// </summary>
        /// <param name="id_unidad"></param>
        /// <param name="id_parada"></param>
        /// <param name="id_evaluacion"></param>
        /// <param name="id_parada_destino"></param>
        private RetornoOperacion muestraResultadoETA(int id_unidad, int id_parada, int id_evaluacion, int id_parada_destino)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Objeto de Ubicación
            System.Drawing.Bitmap imagen = null;

            /** Validando Resultado de la Evaluación **/
            //Instanciando Unidad
            using (Unidad unidad = new Unidad(id_unidad))
            {
                //Instanciando Unidad
                if (unidad.habilitar)
                {
                    //Instanciando ETA
                    using (SAT_CL.Monitoreo.EvaluacionBitacora eta = new SAT_CL.Monitoreo.EvaluacionBitacora(id_evaluacion))
                    using (SAT_CL.Monitoreo.BitacoraMonitoreo bit = new SAT_CL.Monitoreo.BitacoraMonitoreo(eta.id_bitacora))
                    {
                        //Validando que exista la Evaluación
                        if (eta.habilitar && bit.habilitar)
                        {
                            //Creando Marcador
                            SAT_CL.Maps.StaticMap.MarcadorMapa marcador = new SAT_CL.Maps.StaticMap.MarcadorMapa(System.Drawing.ColorTranslator.FromHtml("#33FF00"), bit.geoubicacion, 'U');

                            //Asignando Valores
                            lblVelocidad.Text = bit.velocidad.ToString() + " Km/h";
                            lblDireccion.Text = bit.nombre_ubicacion.Replace("null", "");
                            
                            //Validando Resultado
                            switch (eta.resultado_bitacora)
                            {
                                case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok:
                                case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadTiempoExcedido:
                                    {
                                        //Personalizando Excepción
                                        string res_eta1 = string.Format("{0}", eta.tiempo_excedido > 0 ? "La Unidad tiene Tiempo Excedido de Posicionamiento" : "Unidad Sin Problemas");
                                        imgResultadoETA.Src = string.Format("{0}", eta.tiempo_excedido > 0 ? "../Image/ExclamacionRoja.png" : "../Image/Exclamacion.png");
                                        lblResultadoETA1.Text = res_eta1;
                                        lblResultadoETA2.Text = string.Format("{1} Fecha Ultimo Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                        lblResultadoETA3.Text = "";

                                        //Validando Estatus de la Unidad
                                        switch (unidad.EstatusUnidad)
                                        {
                                            case Unidad.Estatus.ParadaDisponible:
                                                {
                                                    //Si existe la Parada de Destino
                                                    if (id_parada_destino > 0)
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea Iniciar el Servicio?";
                                                        btnSalidaETA.CommandName = "Iniciar";
                                                        //Configurando Controles
                                                        btnSalidaETA.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea actualizar la Unidad?";
                                                        btnSalidaETA.CommandName = "Nuevo";
                                                        //Configurando Controles
                                                        btnSalidaETA.Visible = true;
                                                    }

                                                    //Recuperando Imagen
                                                    result = recuperaImagenParada(marcador, id_parada, out imagen);
                                                    break;
                                                }
                                            case Unidad.Estatus.ParadaOcupado:
                                                {
                                                    //Si existe la Parada de Destino
                                                    if (id_parada_destino > 0)
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea dar Salida a la Unidad?";
                                                        btnSalidaETA.CommandName = "Salida";
                                                        btnSalidaETA.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea terminar el Servicio?";
                                                        btnSalidaETA.CommandName = "Terminar";
                                                        btnSalidaETA.Visible = true;
                                                    }

                                                    //Recuperando Imagen
                                                    result = recuperaImagenParada(marcador, id_parada, out imagen);
                                                    break;
                                                }
                                            case Unidad.Estatus.Transito:
                                                {
                                                    //Instanciando Movimiento y Parada Destino
                                                    using (Movimiento mov = new Movimiento(bit.id_movimiento))
                                                    using (Parada stop = new Parada(mov.id_parada_destino))
                                                    using (Ubicacion ubicacion = new Ubicacion(stop.id_ubicacion))
                                                    {
                                                        //Validando que existan Registros
                                                        if (mov.habilitar && stop.habilitar && ubicacion.habilitar)
                                                        {
                                                            //Validando que exista la Ubicación
                                                            if (ubicacion.geoubicacion != SqlGeography.Null)
                                                            {
                                                                //Obtiene Distancia Permitida
                                                                int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                //Obteniendo Distancia por Ubicación
                                                                using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                                                SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                {
                                                                    //Validando que exista la Referencia
                                                                    if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                    {
                                                                        //Recorriendo Registro
                                                                        foreach (DataRow dr in dtDistancia.Rows)

                                                                            //Obteniendo Distancia Permitida
                                                                            distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                                    }
                                                                }

                                                                //Validando Tipo de Geometria
                                                                switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                                {
                                                                    case "Point":
                                                                        {
                                                                            //Validando que el Punto no exceda mas de 10 metros
                                                                            if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, bit.geoubicacion, distancia_permitida))

                                                                                //Instanciando Resultado Positivo
                                                                                result = new RetornoOperacion(0);
                                                                            else
                                                                                //Instanciando Excepción
                                                                                result = new RetornoOperacion("La Unidad no se encuentra en la Ubicación");
                                                                            break;
                                                                        }
                                                                    case "LineString":
                                                                    case "CompoundCurve":
                                                                    case "Polygon":
                                                                    case "CurvePolygon":
                                                                        {
                                                                            //Obtiene Punto mas Cercano
                                                                            SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                                            //Validando que exista el Punto mas Cercano
                                                                            if (punto_cercano != SqlGeography.Null)
                                                                            {
                                                                                //Validando que haya Intersección en las columnas
                                                                                if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, bit.geoubicacion))

                                                                                    //Instanciando Resultado Positivo
                                                                                    result = new RetornoOperacion(0);
                                                                                else
                                                                                    //Instanciando Excepción
                                                                                    result = new RetornoOperacion("La Unidad no se encuentra en la Ubicación");
                                                                            }
                                                                            else
                                                                                //Instanciando Excepción
                                                                                result = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                            break;
                                                                        }
                                                                }

                                                                //Validando Ubicación en Destino
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Asignando Mensaje Personalizado
                                                                    lblResultadoETA3.Text = "La Unidad ha Llegado al Destino. ¿Desea dar Llegada a la Unidad?";
                                                                    btnSalidaETA.CommandName = "Llegada";
                                                                    btnSalidaETA.Visible = true;
                                                                    
                                                                    //Creando Poligono
                                                                    SAT_CL.Maps.StaticMap.PoligonoMapa poligono = new SAT_CL.Maps.StaticMap.PoligonoMapa(System.Drawing.ColorTranslator.FromHtml("#0F16A4"),
                                                                                                                      System.Drawing.ColorTranslator.FromHtml("#2530F4"), 3, ubicacion.geoubicacion, '2');

                                                                    //Creando Ubicación en el Mapa
                                                                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, poligono, out imagen);
                                                                }
                                                                else
                                                                {
                                                                    //Asignando Mensaje Personalizado
                                                                    lblResultadoETA3.Text = "";
                                                                    btnSalidaETA.CommandName = "";
                                                                    btnSalidaETA.Visible = false;

                                                                    //Creando Ubicación del Mapa
                                                                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, out imagen);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente:
                                    {
                                        //Personalizando Excepción
                                        imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                        lblResultadoETA1.Text = "La Unidad no esta en la Ubicación" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                        lblResultadoETA2.Text = string.Format("{1} Fecha Ultimo Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                        lblResultadoETA3.Text = "";

                                        //Validando Estatus de la Unidad
                                        switch (unidad.EstatusUnidad)
                                        {
                                            case Unidad.Estatus.ParadaDisponible:
                                                {
                                                    //Si existe la Parada de Destino
                                                    if (id_parada_destino > 0)
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea Iniciar el Servicio?";
                                                        btnSalidaETA.CommandName = "Iniciar";
                                                        //Configurando Controles
                                                        btnSalidaETA.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea actualizar la Unidad?";
                                                        btnSalidaETA.CommandName = "Nuevo";
                                                        //Configurando Controles
                                                        btnSalidaETA.Visible = true;
                                                    }

                                                    //Recuperando Imagen
                                                    result = recuperaImagenParada(marcador, id_parada, out imagen);
                                                    break;
                                                }
                                            case Unidad.Estatus.ParadaOcupado:
                                                {
                                                    //Si existe la Parada de Destino
                                                    if (id_parada_destino > 0)
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea dar Salida a la Unidad?";
                                                        btnSalidaETA.CommandName = "Salida";
                                                        btnSalidaETA.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        //Asignando Mensaje Personalizado
                                                        lblResultadoETA3.Text = "¿Desea terminar el Servicio?";
                                                        btnSalidaETA.CommandName = "Terminar";
                                                        btnSalidaETA.Visible = true;
                                                    }

                                                    //Recuperando Imagen
                                                    result = recuperaImagenParada(marcador, id_parada, out imagen);
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadAlejandose:
                                    {
                                        //Personalizando Excepción
                                        imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                        lblResultadoETA1.Text = "La Unidad esta alejandose" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                        lblResultadoETA2.Text = string.Format("{1} Fecha Ultimo Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                        lblResultadoETA3.Text = "";

                                        //Validando Estatus de la Unidad
                                        switch (unidad.EstatusUnidad)
                                        {
                                            case Unidad.Estatus.Transito:
                                                {
                                                    //Instanciando Movimiento y Parada Destino
                                                    using (Movimiento mov = new Movimiento(bit.id_movimiento))
                                                    using (Parada stop = new Parada(mov.id_parada_destino))
                                                    using (Ubicacion ubicacion = new Ubicacion(stop.id_ubicacion))
                                                    {
                                                        //Validando que existan Registros
                                                        if (mov.habilitar && stop.habilitar && ubicacion.habilitar)
                                                        {
                                                            //Validando que exista la Ubicación
                                                            if (ubicacion.geoubicacion != SqlGeography.Null)
                                                            {
                                                                //Obtiene Distancia Permitida
                                                                int distancia_permitida = Convert.ToInt32(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Limite Distancia Ubicación"));

                                                                //Obteniendo Distancia por Ubicación
                                                                using (DataTable dtDistancia = SAT_CL.Global.Referencia.CargaReferencias(ubicacion.id_ubicacion, 15,
                                                                                                SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 15, "Limite Distancia Ubicacion", 0, "Ubicacion")))
                                                                {
                                                                    //Validando que exista la Referencia
                                                                    if (Validacion.ValidaOrigenDatos(dtDistancia))
                                                                    {
                                                                        //Recorriendo Registro
                                                                        foreach (DataRow dr in dtDistancia.Rows)

                                                                            //Obteniendo Distancia Permitida
                                                                            distancia_permitida = Convert.ToInt32(dr["Valor"]);
                                                                    }
                                                                }

                                                                //Validando Tipo de Geometria
                                                                switch (ubicacion.geoubicacion.STGeometryType().Value)
                                                                {
                                                                    case "Point":
                                                                        {
                                                                            //Validando que el Punto no exceda mas de 10 metros
                                                                            if (DatosEspaciales.ValidaDistanciaPermitida(ubicacion.geoubicacion, bit.geoubicacion, distancia_permitida))

                                                                                //Instanciando Resultado Positivo
                                                                                result = new RetornoOperacion(0);
                                                                            else
                                                                                //Instanciando Excepción
                                                                                result = new RetornoOperacion("La Unidad no se encuentra en la Ubicación");
                                                                            break;
                                                                        }
                                                                    case "LineString":
                                                                    case "CompoundCurve":
                                                                    case "Polygon":
                                                                    case "CurvePolygon":
                                                                        {
                                                                            //Obtiene Punto mas Cercano
                                                                            SqlGeography punto_cercano = ubicacion.geoubicacion.STBuffer(distancia_permitida);

                                                                            //Validando que exista el Punto mas Cercano
                                                                            if (punto_cercano != SqlGeography.Null)
                                                                            {
                                                                                //Validando que haya Intersección en las columnas
                                                                                if (DatosEspaciales.ValidaInterseccionSqlGeography(punto_cercano, bit.geoubicacion))

                                                                                    //Instanciando Resultado Positivo
                                                                                    result = new RetornoOperacion(0);
                                                                                else
                                                                                    //Instanciando Excepción
                                                                                    result = new RetornoOperacion("La Unidad no se encuentra en la Ubicación");
                                                                            }
                                                                            else
                                                                                //Instanciando Excepción
                                                                                result = new RetornoOperacion("No se logro obtener el Punto mas Cercano", false);
                                                                            break;
                                                                        }
                                                                }

                                                                //Validando Ubicación en Destino
                                                                if (result.OperacionExitosa)
                                                                {
                                                                    //Asignando Mensaje Personalizado
                                                                    lblResultadoETA3.Text = "La Unidad ha Llegado al Destino. ¿Desea dar Llegada a la Unidad?";
                                                                    btnSalidaETA.CommandName = "Llegada";
                                                                    btnSalidaETA.Visible = true;

                                                                    //Creando Poligono
                                                                    SAT_CL.Maps.StaticMap.PoligonoMapa poligono = new SAT_CL.Maps.StaticMap.PoligonoMapa(System.Drawing.ColorTranslator.FromHtml("#0F16A4"),
                                                                                                                      System.Drawing.ColorTranslator.FromHtml("#2530F4"), 3, ubicacion.geoubicacion, '2');

                                                                    //Creando Ubicación en el Mapa
                                                                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, poligono, out imagen);
                                                                }
                                                                else
                                                                {
                                                                    //Asignando Mensaje Personalizado
                                                                    lblResultadoETA3.Text = "";
                                                                    btnSalidaETA.CommandName = "";
                                                                    btnSalidaETA.Visible = false;

                                                                    //Creando Ubicación del Mapa
                                                                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, out imagen);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    //Personalizando Excepción
                                                    lblResultadoETA3.Text = "";
                                                    btnSalidaETA.Visible = false;
                                                    //Creando Ubicación del Mapa
                                                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, out imagen);
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida:
                                    {
                                        //Personalizando Excepción
                                        imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                        lblResultadoETA1.Text = "La Unidad esta Detenida" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                        lblResultadoETA2.Text = string.Format("{1} Fecha Ultimo Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                        lblResultadoETA3.Text = "";
                                        btnSalidaETA.Visible = false;

                                        //Creando Ubicación del Mapa
                                        result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, out imagen);
                                        break;
                                    }
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Unidad");

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Mostrando Imagen
                    MemoryStream ms = new MemoryStream();
                    imagen.Save(ms, ImageFormat.Png);
                    var base64Data = Convert.ToBase64String(ms.ToArray());
                    imgMapaResultado.Src = "data:image/jpeg;base64," + base64Data;
                    result = new RetornoOperacion(1);
                }
                else
                    //Imagen por Defecto
                    imgMapaResultado.Src = "~/Image/noDisponible.jpg";

                //Activando Pestaña de Resultados
                mtvEvaluacion.ActiveViewIndex = 0;
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Rcuperar la Imagen de una Parada de Destino
        /// </summary>
        /// <param name="marcador">Marcador de Posición GPS</param>
        /// <param name="id_parada">Parada Actual</param>
        /// <param name="imagen">Imagen Obtenido</param>
        /// <returns></returns>
        private RetornoOperacion recuperaImagenParada(SAT_CL.Maps.StaticMap.MarcadorMapa marcador, int id_parada, out System.Drawing.Bitmap imagen)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            imagen = null;

            //Instanciando Destino
            using (Parada destino = new Parada(id_parada))
            using (Ubicacion dest = new Ubicacion(destino.id_ubicacion))
            {
                //Validando que existan
                if (dest.habilitar && destino.habilitar)
                {
                    //Creando Poligono
                    SAT_CL.Maps.StaticMap.PoligonoMapa poligono = new SAT_CL.Maps.StaticMap.PoligonoMapa(System.Drawing.ColorTranslator.FromHtml("#0F16A4"),
                                                                      System.Drawing.ColorTranslator.FromHtml("#2530F4"), 3, dest.geoubicacion, 'S');

                    //Creando Ubicación en el Mapa
                    result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, poligono, out imagen);
                    
                    /*/Validando la Ubicación
                    switch (dest.geoubicacion.STGeometryType().Value)
                    {
                        case "Point":
                            {
                                //Creando Ubicación en el Mapa
                                result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, poligono, out imagen);
                                break;
                            }
                        case "LineString":
                        case "CompoundCurve":
                        case "Polygon":
                        case "CurvePolygon":
                            {
                                //Creando Marcador
                                SAT_CL.Maps.StaticMap.MarcadorMapa centro_destino = new SAT_CL.Maps.StaticMap.MarcadorMapa(System.Drawing.ColorTranslator.FromHtml("#0F16A4"), dest.geoubicacion.EnvelopeCenter(), '2');

                                //Creando Ubicación en el Mapa
                                result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, poligono, centro_destino, out imagen);
                                break;
                            }
                    }//*/
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No se pudo encontrar el Destino");
            }

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Cargar el Historial de la Evaluación
        /// </summary>
        private void cargaHistorialEvaluacion()
        {
            //Declarando Variables Auxiliares
            DateTime fec_bit_ini, fec_bit_fin, fec_eva_ini, fec_eva_fin;

            //Inicializando Variables
            fec_bit_ini = fec_bit_fin = fec_eva_ini = fec_eva_fin = DateTime.MinValue;

            //Validando si se Incluyen Fechas
            if (chkIncluirETA.Checked)
            {
                //Validando el Tipo de Fecha
                if (rbBitacora.Checked)
                {
                    //Obteniendo Fechas de los Controles
                    DateTime.TryParse(txtFecIni.Text, out fec_bit_ini);
                    DateTime.TryParse(txtFecFin.Text, out fec_bit_fin);
                }
                else if (rbEvaluacion.Checked)
                {
                    //Obteniendo Fechas de los Controles
                    DateTime.TryParse(txtFecIni.Text, out fec_eva_ini);
                    DateTime.TryParse(txtFecFin.Text, out fec_eva_fin);
                }
            }

            //Obteniendo Reporte de Historial
            using (DataTable dtHistorialETA = SAT_CL.Monitoreo.Reporte.ObtieneHistorialEvaluaciones(19,
                                                Convert.ToInt32(gvUnidades.SelectedDataKey["*IdUnidad"]),
                                                fec_bit_ini, fec_bit_fin, fec_eva_ini, fec_eva_fin))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtHistorialETA))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvEvaluaciones, dtHistorialETA, "Id", lblOrdenadoETA.Text, true, 1);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtHistorialETA, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvEvaluaciones);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvEvaluaciones);
            }
        }
        /// <summary>
        /// Método encargado de Mostrar el Resultado de la Evaluación
        /// </summary>
        /// <param name="id_evaluacion">Evaluación</param>
        /// <returns></returns>
        public RetornoOperacion muestraEvaluacionConsulta(int id_evaluacion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Declarando Objeto de Ubicación
            System.Drawing.Bitmap imagen = null;

            //Instanciando ETA
            using (SAT_CL.Monitoreo.EvaluacionBitacora eta = new SAT_CL.Monitoreo.EvaluacionBitacora(id_evaluacion))
            using (SAT_CL.Monitoreo.BitacoraMonitoreo bit = new SAT_CL.Monitoreo.BitacoraMonitoreo(eta.id_bitacora))
            {
                //Validando que exista la Evaluación
                if (eta.habilitar && bit.habilitar)
                {
                    //Validando que exista el Punto en la Bitacora
                    if (bit.geoubicacion != SqlGeography.Null)
                    {
                        //Creando Marcador
                        SAT_CL.Maps.StaticMap.MarcadorMapa marcador = new SAT_CL.Maps.StaticMap.MarcadorMapa(System.Drawing.ColorTranslator.FromHtml("#33FF00"), bit.geoubicacion, 'U');

                        //Asignando Valores
                        lblVelocidad.Text = bit.velocidad.ToString() + " Km/h";
                        lblDireccion.Text = bit.nombre_ubicacion.Replace("null", "");

                        //Ocultando Control de Actualización                        
                        btnSalidaETA.Visible = false;

                        //Validando Resultado
                        switch (eta.resultado_bitacora)
                        {
                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.Ok:
                                {
                                    //Personalizando Excepción
                                    imgResultadoETA.Src = "../Image/Exclamacion.png";
                                    lblResultadoETA1.Text = "Unidad sin Problemas" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                    lblResultadoETA2.Text = string.Format("{1} Fecha Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                    lblResultadoETA3.Text = "";
                                    break;
                                }
                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UbicacionNoCoincidente:
                                {
                                    //Personalizando Excepción
                                    imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                    lblResultadoETA1.Text = "La Unidad no esta en la Ubicación" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                    lblResultadoETA2.Text = string.Format("{1} Fecha Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                    lblResultadoETA3.Text = "";
                                    break;
                                }
                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadDetenida:
                                {
                                    //Personalizando Excepción
                                    imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                    lblResultadoETA1.Text = "La Unidad esta Detenida" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                    lblResultadoETA2.Text = string.Format("{1} Fecha Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                    lblResultadoETA3.Text = "";
                                    break;
                                }
                            case SAT_CL.Monitoreo.EvaluacionBitacora.ResultadoBitacora.UnidadAlejandose:
                                {
                                    //Personalizando Excepción
                                    imgResultadoETA.Src = "../Image/ExclamacionRoja.png";
                                    lblResultadoETA1.Text = "La Unidad esta Alejandose" + (eta.tiempo_excedido > 0 ? " y tiene Tiempo Excedido" : "");
                                    lblResultadoETA2.Text = string.Format("{1} Fecha Posicionamiento: {0:dd/MM/yyyy HH:mm}.", bit.fecha_bitacora, eta.tiempo_excedido > 0 ? string.Format("Tiempo Excedido: {0}.", Cadena.ConvierteMinutosACadena(eta.tiempo_excedido)) : "");
                                    lblResultadoETA3.Text = "";
                                    break;
                                }
                        }

                        //Creando Ubicación del Mapa
                        result = SAT_CL.Maps.StaticMap.objStaticMap.ObtieneMapaUbicacion(SAT_CL.Maps.StaticMap.TipoMapa.Camino, 607, 303, marcador, out imagen);
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Ubicación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No existe la Evaluación");
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Mostrando Imagen
                MemoryStream ms = new MemoryStream();
                imagen.Save(ms, ImageFormat.Png);
                var base64Data = Convert.ToBase64String(ms.ToArray());
                imgMapaResultado.Src = "data:image/jpeg;base64," + base64Data;
                result = new RetornoOperacion(1);
            }
            else
                //Imagen por Defecto
                imgMapaResultado.Src = "~/Image/noDisponible.jpg";

            //Mostrando Vista de Resultado
            mtvEvaluacion.ActiveViewIndex = 0;

            //Devolviendo Resultado Obtenido
            return result;
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