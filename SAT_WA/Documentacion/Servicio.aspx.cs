using System;
using System.Transactions;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using SAT_CL.Despacho;
using TSDK.Datos;
using SAT_CL.Seguridad;
using System.Data;
using SAT_CL.Global;

namespace SAT.Documentacion
{
    public partial class Servicio : System.Web.UI.Page
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
    
            //Si no es una recarga de página
            if (!this.IsPostBack)
            {
                //Inicializando contenido de la forma web
                inicializaForma();
            }
            
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
                case "Nuevo":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    //Limpiando Id de sesión
                    Session["id_registro"] = 0;
                    //Limpiando contenido de forma
                    inicializaForma();
                    //Foco a primer control de captura
                    txtUbicacionCarga.Focus();
                    break;
                case "Abrir":
                    inicializaAperturaRegistro(1, false);
                    break;
                case "Guardar":
                    guardaServicio();
                    break;
                case "Imprimir":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", "~/RDLC/Reporte.aspx");

                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Porte", Convert.ToInt32(Session["id_registro"])), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                    }
                    break;
                case "Editar":
                    //Asignando estatus nuevo
                    Session["estatus"] = Pagina.Estatus.Edicion;
                    //Limpiando contenido de forma
                    inicializaForma();
                    //Foco a primer control de captura
                    txtUbicacionCarga.Focus();
                    break;
                case "Eliminar":
                    deshabilitaServicio();
                    break;
                case "Cancelar":
                    //Cerrando ventana de confirmación
                    alternaVentanaModal("confirmacionCancelacion", lkbCancelar);
                    break;
                case "AbrirMaestro":
                    inicializaAperturaRegistro(1, true);
                    break;
                case "HacerMaestro":
                    inicializaVentanaHacerMaestro();
                    break;
                case "CopiarMaestro":
                    //Inicializando Control de Copia de Servicio
                    ucServicioCopia.InicializaServicioCopia();

                    //Mostrando Ventana
                    TSDK.ASP.ScriptServer.AlternarVentana(lkbCopiarMaestro, lkbCopiarMaestro.GetType(), "VentanaServicioCopia", "contenedorVentanaCopiaServicio", "ventanaCopiaServicio");
                    break;
                case "Bitacora":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaBitacora(Session["id_registro"].ToString(), "1", "Servicio");
                    break;
                case "Referencias":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "1", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                    break;
                case "Archivos":
                    //Si hay un registro en sesión
                    if (Session["id_registro"].ToString() != "0")
                        inicializaArchivosRegistro(Session["id_registro"].ToString(), "1", "0");
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
                case "Temperaturas":

                    //Inicializando Control de Copia de Servicio
                    wucTemperaturaUnidad.InicializaTemperaturasServicio(Convert.ToInt32(Session["id_registro"]));

                    //Mostrando Ventana
                    TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VentanaTemperatura", "contenedorVentanaTemperatura", "ventanaTemperatura");
                    break;
                case "NoFacturable":

                    //Si hay registros
                    if (Convert.ToInt32(Session["id_registro"]) > 0)
                    {
                        //Abre ventana modal
                        ScriptServer.AlternarVentana(lkbNoFacturable, lkbNoFacturable.GetType(), "AbrirVentana", "confirmacionNoFacturable", "NoFacturable");
                        //Limpiamos Control
                        txtMotivoNoFacturable.Text = "";

                    }
                    break;
            }
        }

        /// <summary>
        /// Evento que cambia el estado de un servicio a No facturable .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNoFacturable_Click(object sender, EventArgs e)
        { //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Factura
            using (SAT_CL.Facturacion.Facturado fac = SAT_CL.Facturacion.Facturado.ObtieneFacturaServicio(Convert.ToInt32(Session["id_registro"])))
            {
                //Validando que Exista la Factura
                if (fac.id_factura > 0)

                    //Actualizando Estatus
                    result = fac.ActualizaEstatusNoFacturable(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, txtMotivoNoFacturable.Text);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Factura");

                //Validando que la Operación fuese Exitosa
                if (result.OperacionExitosa)
                {
                    //Pasando Estatus a Lectura
                    Session["estatus"] = Pagina.Estatus.Lectura;

                    //Invocando Método de Inicialización
                    inicializaForma();

                    //Cierra la ventana modal
                    ScriptServer.AlternarVentana(btnNoFacturable, "CerrarVentana", "confirmacionNoFacturable", "NoFacturable");
                }

                //Mostrando Mensaje de Operación
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Evento que cierra la ventana modal que confirma si un servicio es facturable o no al dar clic en el link Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarNoFacturable_Click(object sender, EventArgs e)
        {
            //Cierra la ventana modal
            ScriptServer.AlternarVentana(lnkCerrarNoFacturable, "CerrarVentana", "confirmacionNoFacturable", "NoFacturable");
        }
        /// <summary>
        /// Evento producido al cambiar el lugar de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUbicacionCarga_TextChanged(object sender, EventArgs e)
        {
            muestraInformacionSitioCarga();
        }

        /// <summary>
        /// Click en botones de cancelación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelacion_Click(object sender, EventArgs e)
        {
            //Determinando el comando a realizar
            switch (((Button)sender).CommandName)
            {
                case "Aceptar":
                    //Realizando cancelación
                    cancelaServicio();


                    break;
                case "Cancelar":
                    //Cerrando ventana de confirmación
                    alternaVentanaModal("confirmacionCancelacion", btnCancelarCancelacion);
                    break;
            }
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
                case "confirmacionCancelacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionCancelacionModal", "confirmacionCancelacion");
                    break;
            }
        }
        /// <summary>
        /// Evento producido al cambiar el lugar de descarga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtUbicacionDescarga_TextChanged(object sender, EventArgs e)
        {
            muestraInformacionSitioDescarga();
        }
        /// <summary>
        /// Evento producido al cambiar el cliente seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            muestraInformacionCliente();
        }
        /// <summary>
        /// Evento producido al pulsar él botón Guardar o Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarCancelar_Click(object sender, EventArgs e)
        {
            //Determinando que botón ha sido el pulsado
            switch (((Button)sender).CommandName)
            {
                case "Guardar":
                    guardaServicio();
                    break;
                case "Cancelar":
                    //Si el estatus actual es edición
                    if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion)
                        //Cambiando estatus a lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;

                    //Inicializando contenido de página
                    inicializaForma();
                    break;
                case "Buscar":
                    //Realizando la búsqueda de servicios maestros coincidentes
                    //buscaServiciosMaestros();
                    break;
                case "GuardarMaestro":
                    hacerServicioMaestro();
                    break;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma web
        /// </summary>
        private void inicializaForma()
        {
            //Cargando valores de registro
            cargaControlesForma();
            //Habilitando controles
            habilitaControlesEstatus();
            //Habilitando los menús de la forma
            habilitaMenus();
            //Inicializando controles de usuario
            inicalizaControlesUsuario();
            //Inicializando búsqueda de servicios maestros
            //inicializaVentanaServiciosMaestros();
        }
        /// <summary>
        /// Carga la configuración de todos los controles de usuario auxiliares para la personalización de Servicios
        /// </summary>
        private void inicalizaControlesUsuario()
        {
            //Obteniendo el Id de Servicio
            int id_servicio = Convert.ToInt32(Session["id_registro"]);
            //Obteniendo Id de Compañía
            int id_compania = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1));

            //Paradas
            wucParada.InicializaControl(id_servicio, id_compania);
            //Productos
            wucProducto.InicializaControl(id_servicio, id_compania);
            //Clasificación
            wucClasificacion.InicializaControl(1, id_servicio, id_compania);
            //Facturacion
            wucFacturado.InicializaControl(id_servicio);
            //Detalles de facturación (cargos)
            wucFacturadoConcepto.InicializaControl(wucFacturado.idFactura);
            //Referencias de Viaje
            ucReferenciaViaje.InicializaControl(id_servicio, id_compania, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), 1);
            //Inicializando Control de Usuario
            ucEvidenciaSegmento.InicializaControlUsuario(id_servicio);
        }
        /*// <summary>
        /// Carga el contenido inicial de los controles de búsqueda de servicio maestro
        /// </summary>
        private void inicializaVentanaServiciosMaestros()
        {
            //Limpiando filtros
            txtClienteMaestro.Text =
            txtUbicacionOrigenMaestro.Text =
            txtUbicacionDestinoMaestro.Text = "";
            //Limpiando resultados de búsqueda
            TSDK.ASP.Controles.InicializaGridview(gvServiciosMaestros);
            //Asignando foco
            txtClienteMaestro.Focus();
        }*/
        /// <summary>
        /// Carga la ventana modal para ingresar descripción de servicio maestro
        /// </summary>
        private void inicializaVentanaHacerMaestro()
        {
            txtDescripcionMaestro.Text = "";
            txtDescripcionMaestro.Focus();
        }
        /// <summary>
        /// Carga los valores del registro sobre los controles que los mostrarán en base a un estatus de forma
        /// </summary>
        private void cargaControlesForma()
        {
            //Determinando el estatus de carga de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //Encabezado
                    //Instanciando compañía guardada en sesión
                    using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                    txtNoServicio.Text = "";
                    lblCopiaDe.Text = "";
                    txtEstatus.Text = "Sin Registrar";

                    //Info Carga
                    txtUbicacionCarga.Text = 
                    txtDireccionCarga.Text =
                    txtCoordenadasCarga.Text =
                    lkbMapaCarga.NavigateUrl = "";
                    txtCitaCarga.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

                    //Info Descarga
                    txtUbicacionDescarga.Text =
                    txtDireccionDescarga.Text =
                    txtCoordenadasDescarga.Text =
                    lkbMapaDescarga.NavigateUrl = "";
                    txtCitaDescarga.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddHours(2).ToString("dd/MM/yyyy HH:mm");

                    //Datos Cliente
                    txtCliente.Text =
                    txtDireccionCliente.Text =
                    txtRFC.Text = "";
                    lblLimite.Text = 
                    lblTotal.Text = 
                    lblSaldo.Text = string.Format("{0:C2}", 0);

                    //Referencias Cliente
                    txtCartaPorte.Text =
                    txtReferenciaCliente.Text =
                    txtObservacion.Text = "";

                    break;
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    //Instanciando registro servicio
                    using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (servicio.id_servicio > 0)
                        {
                            //Instanciando compañía guardada en sesión
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(servicio.id_compania_emisor))
                                txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                            txtNoServicio.Text = servicio.no_servicio;
                            
                            //Si es copia de un servicio maestro
                            if (servicio.id_servicio_base > 0)
                            { 
                                //Instanciando registro base
                                using (SAT_CL.Documentacion.Servicio sm = new SAT_CL.Documentacion.Servicio(servicio.id_servicio_base))
                                    lblCopiaDe.Text = string.Format("Copia de: {0}", sm.no_servicio);
                            }
                            //De lo contrario
                            else
                                lblCopiaDe.Text = "";

                            txtEstatus.Text = servicio.estatus.ToString();

                            //Info Carga
                            //Instanciando ubicación de carga
                            using (SAT_CL.Global.Ubicacion uc = new SAT_CL.Global.Ubicacion(servicio.id_ubicacion_carga))
                            {
                                txtUbicacionCarga.Text = string.Format("{0}   ID:{1}", uc.descripcion, uc.id_ubicacion);
                                txtDireccionCarga.Text = uc.ObtieneDireccionCompleta();
                                txtCoordenadasCarga.Text = string.Format("{0}, {1}", uc.latitud, uc.longitud);
                                lkbMapaCarga.NavigateUrl = "~/Maps/UbicacionMapa.aspx?id_ubicacion=" + Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1);
                            }
                            txtCitaCarga.Text = servicio.cita_carga.ToString("dd/MM/yyyy HH:mm");

                            //Info Descarga
                            //Instanciando ubicación de descarga
                            using (SAT_CL.Global.Ubicacion ud = new SAT_CL.Global.Ubicacion(servicio.id_ubicacion_descarga))
                            {
                                txtUbicacionDescarga.Text = string.Format("{0}   ID:{1}", ud.descripcion, ud.id_ubicacion);
                                txtDireccionDescarga.Text = ud.ObtieneDireccionCompleta();
                                txtCoordenadasDescarga.Text = string.Format("{0}, {1}", ud.latitud, ud.longitud);
                                lkbMapaDescarga.NavigateUrl = "~/Maps/UbicacionMapa.aspx?id_ubicacion=" + Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1);
                            }
                            txtCitaDescarga.Text = servicio.cita_descarga.ToString("dd/MM/yyyy HH:mm");

                            //Datos Cliente
                            //Instanciando Cliente
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, servicio.id_cliente_receptor, 1))
                            {
                                //Mostrando su nombre
                                txtCliente.Text = string.Format("{0} [{1}] ID:{2}", c.nombre, c.nombre_corto, c.id_compania_emisor_receptor);
                                //Cargando la dirección y RFC
                                using (SAT_CL.Global.Direccion d = new SAT_CL.Global.Direccion(c.id_direccion))
                                    //Si la dirección existe
                                    if (d.id_direccion > 0)
                                        txtDireccionCliente.Text = d.ObtieneDireccionCompleta();
                                
                                //Asignando Valores
                                txtRFC.Text = c.rfc;
                                lblLimite.Text = string.Format("{0:C2}", c.limite_credito);
                                lblTotal.Text = string.Format("{0:C2}", c.saldo_actual);
                                lblSaldo.Text = string.Format("{0:C2}", c.limite_credito - c.saldo_actual);
                            }

                            //Referencias Cliente
                            txtCartaPorte.Text = servicio.porte;
                            txtReferenciaCliente.Text = servicio.referencia_cliente;
                            txtObservacion.Text = servicio.observacion_servicio;
                        }
                    }
                    break;
                case Pagina.Estatus.Copia:
                    //TODO: Implementar copia de servicio
                    break;
            }

            //Limpiando mensajes existentes
            lblErrorServicio.Text = "";
        }
        /// <summary>
        /// Habilita o deshabilita los controles de la forma en base a su estatus actual
        /// </summary>
        private void habilitaControlesEstatus()
        {
            //Determinando el estatus de carga de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    //txtCompania.Enabled = 

                    //Info Carga
                    txtUbicacionCarga.Enabled =
                    txtCitaCarga.Enabled =

                    //Info Descarga
                    txtUbicacionDescarga.Enabled =
                    txtCitaDescarga.Enabled =

                    //Datos Cliente
                    txtCliente.Enabled =

                    //Referencias Cliente
                    txtCartaPorte.Enabled =
                    txtReferenciaCliente.Enabled =
                    txtObservacion.Enabled =

                    //Botones Guardar Cancelar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;

                    //Controles de usuario
                    wucProducto.Enabled =
                    wucClasificacion.Enabled =

                    wucFacturadoConcepto.Enabled = false;

                    break;
                case Pagina.Estatus.Lectura:
                    //txtCompania.Enabled =

                    //Info Carga
                    txtUbicacionCarga.Enabled =
                    txtCoordenadasCarga.Enabled =
                    txtCitaCarga.Enabled =

                    //Info Descarga
                    txtUbicacionDescarga.Enabled =
                    txtCitaDescarga.Enabled =

                    //Datos Cliente
                    txtCliente.Enabled =

                    //Referencias Cliente
                    txtCartaPorte.Enabled =
                    txtReferenciaCliente.Enabled =
                    txtObservacion.Enabled =

                    //Botones Guardar Cancelar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;

                    //Controles de usuario
                    wucProducto.Enabled =
                    wucClasificacion.Enabled =
                    wucFacturadoConcepto.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    //txtCompania.Enabled = false;

                    //Info Carga
                    txtUbicacionCarga.Enabled =
                    txtCitaCarga.Enabled =

                    //Info Descarga
                    txtUbicacionDescarga.Enabled =
                    txtCitaDescarga.Enabled = false;

                    //Datos Cliente
                    txtCliente.Enabled =

                    //Referencias Cliente
                    txtCartaPorte.Enabled =
                    txtReferenciaCliente.Enabled =
                    txtObservacion.Enabled =

                    //Botones Guardar Cancelar
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;

                    //Controles de usuario
                    wucProducto.Enabled =
                    wucClasificacion.Enabled =
                    wucFacturadoConcepto.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Habilita o deshabilita opciones del menú de la forma en base a su estatus actual
        /// </summary>
        private void habilitaMenus()
        {
            //Determinando el estatus de carga de la forma
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    lkbNuevo.Enabled =
                    lkbGuardar.Enabled = true;
                    lkbEditar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled =
                    lkbImprimir.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled = 
                    lkbReferenciaTemperaturas.Enabled = false;
                    break;
                case Pagina.Estatus.Lectura:
                    lkbNuevo.Enabled =
                    lkbImprimir.Enabled =
                    lkbEditar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled =
                    lkbReferenciaTemperaturas.Enabled = true;
                    lkbGuardar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    lkbNuevo.Enabled =
                    lkbImprimir.Enabled =
                    lkbGuardar.Enabled =
                    lkbCancelar.Enabled =
                    lkbEliminar.Enabled =
                    lkbBitacora.Enabled =
                    lkbReferencias.Enabled =
                    lkbArchivos.Enabled =
                    lkbReferenciaTemperaturas.Enabled = true;
                    lkbEditar.Enabled = false;  
                    break;
            }
        }
        /// <summary>
        /// Realiza el guardado en BD de la indormación del servicio
        /// </summary>
        private void guardaServicio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();
            int id_servicio = 0;

            //Validando rango de fechas
            resultado = validaFechasCargaDescarga();
            if (resultado.OperacionExitosa)
            {
                //Determinando el tipo de guardado a realizar
                switch ((Pagina.Estatus)Session["estatus"])
                {
                    case Pagina.Estatus.Nuevo:

                        //Creando ambiente transaccional
                        using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Insertando nuevo servicio
                            resultado = SAT_CL.Documentacion.Servicio.InsertarServicio("", 1, 0, false, 0, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaCarga.Text),
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaDescarga.Text), txtCartaPorte.Text.ToUpper(),
                                                                                txtReferenciaCliente.Text.ToUpper(), Fecha.ObtieneFechaEstandarMexicoCentro(), txtObservacion.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);

                            //Si no hay dificultades con la inserción
                            if (resultado.OperacionExitosa)
                            {
                                id_servicio = resultado.IdRegistro;
                                //Instanciamos Servicio
                                using(SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(resultado.IdRegistro))
                                {

                                   //Actualizamos Refrencias de Servicio
                                    resultado = objServicio.ActualizacionReferenciaViaje(txtReferenciaCliente.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                
                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Inicializando información de paradas, segmentos y movimientos de viaje
                                    resultado = Parada.InsertaServicio(id_servicio, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaCarga.Text),
                                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaDescarga.Text),
                                                                                    0, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), ((Usuario)Session["usuario"]).id_usuario, false);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertando Clasificacion predeterminada
                                        resultado = SAT_CL.Global.Clasificacion.InsertaClasificacion(1, id_servicio, 0, 0, 0, 0, 0, 0, 0, 0, 0, ((Usuario)Session["usuario"]).id_usuario);

                                        //Si no hay errores
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Realizando Insercción de Factura
                                            resultado = SAT_CL.Facturacion.Facturado.InsertaFactura(id_servicio, ((Usuario)Session["usuario"]).id_usuario);
                                        }
                                    }
                                }
                                }
                            }

                            //Si no hay errores encontrados
                            if (resultado.OperacionExitosa)
                                //Confirmando modificaciones
                                transaccion.Complete();
                        }
                        break;
                    case Pagina.Estatus.Edicion:
                    //Creando ambiente transaccional
                        using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Instanciando servicio actual
                            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Si el registro se cargó correctamente
                                if (servicio.id_servicio > 0)
                                {
                                    //Actualizando registro
                                    resultado = servicio.EditarServicio(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaCarga.Text),
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1)), Convert.ToDateTime(txtCitaDescarga.Text), txtCartaPorte.Text.ToUpper(),
                                                                                txtReferenciaCliente.Text.ToUpper(), txtObservacion.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                    id_servicio = resultado.IdRegistro;
                                    //Validando Operación Exitosa
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Actualizamos Referencia de Viaje
                                        resultado = servicio.ActualizacionReferenciaViaje(txtReferenciaCliente.Text.ToUpper(), ((Usuario)Session["usuario"]).id_usuario);
                                    }
                                }

                                //Si no hay errores encontrados
                                if (resultado.OperacionExitosa)
                                {
                                    //Instanciando Resultado Positivo
                                    resultado = new RetornoOperacion(id_servicio);

                                    //Confirmando modificaciones
                                    transaccion.Complete();
                                }
                            }
                        }
                        break;
                }

                //Validando si la operación de guardado fue correcta
                if (resultado.OperacionExitosa)
                {
                    //Asignando estatus de lectura en caso de ser edición el actual
                    if ((Pagina.Estatus)Session["estatus"] == Pagina.Estatus.Edicion)
                        Session["estatus"] = Pagina.Estatus.Lectura;
                    //De lo contrario, se asigna estatus de edición
                    else
                        Session["estatus"] = Pagina.Estatus.Edicion;

                    Session["id_registro"] = id_servicio;
                    //Cargando contenido de página
                    inicializaForma();
                    //Carga Control de Referencia

                }
            }

            //Mostrando mensaje de actualización 
            lblErrorServicio.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza la cancelación del servicio actual
        /// </summary>
        private void cancelaServicio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando servicio actual
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro se cargó correctamente
                if (servicio.id_servicio > 0)
                    //Actualizando registro
                    resultado = servicio.CancelaServicio(((Usuario)Session["usuario"]).id_usuario,txtMotivoCancelacion.Text);
            }

            //Validando si la operación de guardado fue correcta
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de lectura registro
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Cargando contenido de página
                inicializaForma();

                //Cerrando ventana de confirmación
                alternaVentanaModal("confirmacionCancelacion", btnAceptarCancelacion);
            }

            //Mostrando mensaje de actualización 
            ScriptServer.MuestraNotificacion(btnAceptarCancelacion, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza la deshabilitación del servicio y sus dependencias
        /// </summary>
        private void deshabilitaServicio()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando servicio actual
            using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el registro se cargó correctamente
                if (servicio.id_servicio > 0)
                    //Actualizando registro
                    resultado = servicio.DeshabilitarServicio(((Usuario)Session["usuario"]).id_usuario);
            }

            //Validando si la operación de guardado fue correcta
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de nuevo registro
                Session["estatus"] = Pagina.Estatus.Nuevo;
                Session["id_registro"] = 0;
                //Cargando contenido de página
                inicializaForma();
            }

            //Mostrando mensaje de actualización 
            lblErrorServicio.Text = resultado.Mensaje;
        }
           /// <summary>
        /// Realiza la deshabilitación del servicio y sus dependencias
        /// </summary>
        private void hacerServicioMaestro()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando bloque transaccional
            using (TransactionScope transaccion = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando servicio actual
                using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
                {
                    //Si el registro se cargó correctamente
                    if (servicio.id_servicio > 0)
                        //Actualizando registro
                        resultado = servicio.HacerMaestro(((Usuario)Session["usuario"]).id_usuario);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        resultado = SAT_CL.Global.Referencia.InsertaReferencia(servicio.id_servicio, 1, ReferenciaTipo.ObtieneIdReferenciaTipo(0, 1, "Identificador", 0, "Servicio Maestro"), txtDescripcionMaestro.Text.ToUpper(), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((Usuario)Session["usuario"]).id_usuario, true);
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Finalizando transacción
                    transaccion.Complete();
            }

            //Validando si la operación de guardado fue correcta
            if (resultado.OperacionExitosa)
            {
                //Asignando estatus de nuevo registro
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Cargando contenido de página
                inicializaForma();
            }

            //Mostrando mensaje de actualización 
            lblErrorServicio.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Muestra los datos vinculados al sitio de carga
        /// </summary>
        private void muestraInformacionSitioCarga()
        {
            //Limpiando controles asociados
            txtDireccionCarga.Text =
            txtCoordenadasCarga.Text =
            lkbMapaCarga.NavigateUrl = "";

            //Si hay texto nuevo
            if (txtUbicacionCarga.Text.Trim() != "")
            {
                //Recuperando Id de Ubicación 
                int id_ubicacion = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1));
                //Si el formato de cadena permite la recuperación del ID de Ubicación
                if (id_ubicacion > 0)
                {
                    //Instanciando registro desde BD
                    using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(id_ubicacion))
                    {
                        //Cargando la dirección y geoubicación
                        txtDireccionCarga.Text = u.ObtieneDireccionCompleta();
                        txtCoordenadasCarga.Text = string.Format("{0}, {1}", u.latitud, u.longitud);
                        lkbMapaCarga.NavigateUrl = "~/Maps/UbicacionMapa.aspx?id_ubicacion=" + Cadena.RegresaCadenaSeparada(txtUbicacionCarga.Text, "ID:", 1);
                    }
                }
            }

            //Manteniendo foco en control de dirección
            txtUbicacionCarga.Focus();
        }
        /// <summary>
        /// Muestra los datos vinculados al sitio de descarga
        /// </summary>
        private void muestraInformacionSitioDescarga()
        {
            //Limpiando controles asociados
            txtDireccionDescarga.Text =
            txtCoordenadasDescarga.Text =
            lkbMapaDescarga.NavigateUrl = "";

            //Si hay texto nuevo
            if (txtUbicacionDescarga.Text.Trim() != "")
            {
                //Recuperando Id de Ubicación 
                int id_ubicacion = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1));
                //Si el formato de cadena permite la recuperación del ID de Ubicación
                if (id_ubicacion > 0)
                {
                    //Instanciando registro desde BD
                    using (SAT_CL.Global.Ubicacion u = new SAT_CL.Global.Ubicacion(id_ubicacion))
                    {
                        //Cargando la dirección y geoubicación
                        txtDireccionDescarga.Text = u.ObtieneDireccionCompleta();
                        txtCoordenadasDescarga.Text = string.Format("{0}, {1}", u.latitud, u.longitud);
                        lkbMapaDescarga.NavigateUrl = "~/Maps/UbicacionMapa.aspx?id_ubicacion=" + Cadena.RegresaCadenaSeparada(txtUbicacionDescarga.Text, "ID:", 1);
                    }
                }
            }

            //Manteniendo foco en control de dirección
            txtUbicacionDescarga.Focus();
        }
        /// <summary>
        /// Muestra los datos vinculados al Cliente seleccionado
        /// </summary>
        private void muestraInformacionCliente()
        {
            //Limpiando controles asociados
            txtDireccionCliente.Text =
            txtRFC.Text = "";

            //Si hay texto nuevo
            if (txtCliente.Text.Trim() != "")
            {
                //Recuperando Id de Cliente 
                int id_cliente = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1));
                //Si el formato de cadena permite la recuperación del ID de Cliente
                if (id_cliente > 0)
                {
                    //Instanciando registro desde BD
                    using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, id_cliente, 1))
                    {
                        //Cargando la dirección y RFC
                        using (SAT_CL.Global.Direccion d = new SAT_CL.Global.Direccion(c.id_direccion))
                            //Si la dirección existe
                            if (d.id_direccion > 0)
                                txtDireccionCliente.Text = d.ObtieneDireccionCompleta();
                        
                        //Asignando Valores
                        txtRFC.Text = c.rfc;
                        lblLimite.Text = string.Format("{0:C2}", c.limite_credito);
                        lblTotal.Text = string.Format("{0:C2}", c.saldo_actual);
                        lblSaldo.Text = string.Format("{0:C2}", c.limite_credito - c.saldo_actual);

                    }
                }
            }

            //Manteniendo foco en control actual
            txtCliente.Focus();
        }
        /// <summary>
        /// Valida que la fecha de carga sea menor a la descarga
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaFechasCargaDescarga()
        {
            //Inicializando resultado de validación, sin errores
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Si la fecha de carga es mayor que la fecha de descarga
            if (Convert.ToDateTime(txtCitaCarga.Text).CompareTo(Convert.ToDateTime(txtCitaDescarga.Text)) > 0)
                resultado = new RetornoOperacion("La 'Cita de Carga' debe ser 'menor' que la 'Cita de Descarga'.");

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la actualización de contenido dentro de los paneles que poseen contenido del registro servicio
        /// </summary>
        private void actualizaPanelesServicio()
        {
            //Compañía
            uptxtCompania.Update();
            //Número de Servicio
            uptxtNoServicio.Update();
            //Estatus de Servicio
            uptxtEstatus.Update();

            //Ubicación de lugar de carga
            uptxtUbicacionCarga.Update();
            uptxtDireccionCarga.Update();
            uptxtCoordenadasCarga.Update();
            uplnkMapaCarga.Update();
            uptxtCitaCarga.Update();

            //Ubicación de lugar de descarga
            uptxtUbicacionDescarga.Update();
            uptxtDireccionDescarga.Update();
            uptxtCoordenadasDescarga.Update();
            uplnkMapaDescarga.Update();
            uptxtCitaDescarga.Update();

            //Datos del Cliente
            uptxtCliente.Update();
            uptxtDireccionCliente.Update();
            uptxtRFC.Update();
            uptxtCartaPorte.Update();
            uptxtReferenciaCliente.Update();
            uptxtObservacion.Update();
        }
        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro solicitado
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de apertura de registros
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="maestros">True para mostrar sólo servicios maestros, False para mostrar todos excepto paestros</param>
        private void inicializaAperturaRegistro(int idTabla, bool maestros)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", string.Format("~/Accesorios/AbrirRegistro.aspx?P1={0}&P3={1}", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToByte(maestros)));
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Abrir", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de referencias del registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencias", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Documentacion/Servicio.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }
        /*// <summary>
        /// Busca los servicios maestros solicitados
        /// </summary>
        private void buscaServiciosMaestros()
        {
            //Obteniendo registros de interés
            using (System.Data.DataTable mit = SAT_CL.Documentacion.Servicio.CargaServiciosMaestros(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtClienteMaestro.Text, "ID:", 1)), 
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionOrigenMaestro.Text, "ID:", 1)), 
                                                                                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionDestinoMaestro.Text, "ID:", 1))))
            { 
                //Llenando GridView
                Controles.CargaGridView(gvServiciosMaestros, mit, "Id", "", true, 0);
                //Almacenando Origen de datos
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((System.Data.DataSet)Session["DS"], mit);
            }
        }*/

        #endregion

        #region Controles de Usuario

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
        /// Maneja el evento click sobre el botón agregar arriba del control Parada
        /// </summary>
        protected void wucParada_ClickAgregarArriba(object sender, EventArgs e)
        {
            //Actualizando parada inicial
            RetornoOperacion resultado = wucParada.GuardaParadaArriba();
            //Si no hay problemas de actualización
            if (resultado.OperacionExitosa)
            {
                //Actualizando controles de servicio
                cargaControlesForma();
                //Actualizando los paneles de servicio
                actualizaPanelesServicio();
                //Inicializamos Control de Producto
                wucProducto.InicializaControl(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                //Inicializando Control de Usuario
                ucEvidenciaSegmento.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]));
            }
        }
        /// <summary>
        /// Maneja el evento click sobre el botón agregar abajo del control Parada
        /// </summary>
        protected void wucParada_ClickAgregarAbajo(object sender, EventArgs e)
        {
            //Actualizando parada final
            RetornoOperacion resultado = wucParada.GuardaParadaAbajo();
            //Si no hay problemas de actualización
            if (resultado.OperacionExitosa)
            {
                //Actualizando controles de servicio
                cargaControlesForma();
                //Actualizando los paneles de servicio
                actualizaPanelesServicio();
                //Inicializamos Control de Producto
                wucProducto.InicializaControl(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                //Inicializando Control de Usuario
                ucEvidenciaSegmento.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]));
            }
        }
        /// <summary>
        /// Maneja el evento click sobre el botón editar del control Parada
        /// </summary>
        protected void wucParada_ClickEditar(object sender, EventArgs e)
        {
            //Actualizando parada en edición
            RetornoOperacion resultado = wucParada.EditaParada();
            //Si no hay problemas de actualziación
            if (resultado.OperacionExitosa)
            {
                //Actualizando controles de servicio
                cargaControlesForma();
                //Actualizando los paneles de servicio
                actualizaPanelesServicio();
                //Inicializamos Control de Producto
                wucProducto.InicializaControl(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                //Inicializando Control de Usuario
                ucEvidenciaSegmento.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]));
            }
        }
        /// <summary>
        /// Maneja el evento click sobre el botón editar del control Parada
        /// </summary>
        protected void wucParada_ClickEliminar(object sender, EventArgs e)
        {
            //Eliminando parada seleccionada
            RetornoOperacion resultado = wucParada.DeshabilitaParada();
            //Si no hay problemas de actualziación
            if (resultado.OperacionExitosa)
            {
                //Actualizando controles de servicio
                cargaControlesForma();
                //Actualizando los paneles de servicio
                actualizaPanelesServicio();
                //Inicializamos Control de Producto
                wucProducto.InicializaControl(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                //Inicializando Control de Usuario
                ucEvidenciaSegmento.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]));
            }
        }

        /// <summary>
        /// Maneja el evento click sobre el botón agregar abajo del control Parada
        /// </summary>
        protected void wucParada_ClickInsertarEvento(object sender, EventArgs e)
        {
            //Actualizando parada final
            RetornoOperacion resultado = wucParada.InsertaEvento();
            //Si no hay problemas de actualización
            if (resultado.OperacionExitosa)
            {
                //Inicializamos Control de Producto
                wucProducto.InicializaControl(Convert.ToInt32(Session["id_registro"]), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)));
                //Inicializando Control de Usuario
                ucEvidenciaSegmento.InicializaControlUsuario(Convert.ToInt32(Session["id_registro"]));
            }
        }

        /// <summary>
        /// Maneja el evento click sobre el botón guardar del control facturado
        /// </summary>
        protected void wucFacturado_ClickGuardarFactura(object sender, EventArgs e)
        {
            //Realizando guardado de regstro
            wucFacturado.GuardaFactura();
        }
        /// <summary>
        /// Maneja el evento click sobre el botón guardar del control facturado
        /// </summary>
        protected void wucFacturado_ClickAplicarTarifa(object sender, EventArgs e)
        {
            //Realizando guardado de regstro
            RetornoOperacion resultado = wucFacturado.AplicaTarifaFactura();
            if (resultado.OperacionExitosa)
            {
                //Actualziando encabezado de factura
                wucFacturado.InicializaControl(Convert.ToInt32(Session["id_registro"]));
                upwucFacturado.Update();
                wucFacturadoConcepto.InicializaControl(wucFacturado.idFactura);
                upwucFacturadoConcepto.Update();
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

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualziando encabezado de factura
                wucFacturado.InicializaControl(Convert.ToInt32(Session["id_registro"]));

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
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

            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Actualziando encabezado de factura
                wucFacturado.InicializaControl(Convert.ToInt32(Session["id_registro"]));

            //Mostrando Notificación
            ScriptServer.MuestraNotificacion(this.Page, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        
        /// <summary>
        /// Evento Producido al Guardar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {   
            //Declaramos Objeto resutado
            RetornoOperacion resultado = new RetornoOperacion();
            //Guardando Referencia
          resultado=  ucReferenciaViaje.GuardaReferenciaViaje();
            //Validamos Resultado
            if(resultado.OperacionExitosa)
            {
                //Instanciando servicio actual
                using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
                {
                    //Actualizamos Enzabezado de Viaje
                    txtReferenciaCliente.Text = servicio.referencia_cliente;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Eliminar una Referencia del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciaViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {  
            //Declaramos Objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Eliminando Referencia
           resultado = ucReferenciaViaje.EliminaReferenciaViaje();
            //Validamos Resulado
            if(resultado.OperacionExitosa)
            {
                 //Instanciando servicio actual
                using (SAT_CL.Documentacion.Servicio servicio = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(Session["id_registro"])))
                {
                    //Actualizamos Enzabezado de Viaje
                    txtReferenciaCliente.Text = servicio.referencia_cliente;
                }
                
            }

        }
        /// <summary>
        /// Evento Producido al Guardar la Copia del Servicio Maestro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucServicioCopia_ClickGuardarServicioCopia(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Copiando Servicio Maestro
            result = ucServicioCopia.CopiaServicio();

            //Validando que la Operación fuese Exitosa
            if(result.OperacionExitosa)
            {
                //Añadiendo Registro a Session
                Session["id_registro"] = result.IdRegistro;

                //Estatus en Lectura
                Session["estatus"] = Pagina.Estatus.Lectura;

                //Inicializando Forma
                inicializaForma();

                //Ocasionando Postback a la Pagina
                Response.Redirect(Page.AppRelativeVirtualPath);
            }
        }
        /// <summary>
        /// Evento Producido al Cancelar la Copia del Servicio Maestro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucServicioCopia_ClickCancelarServicioCopia(object sender, EventArgs e)
        {
            //Cancelando Copia
            TSDK.ASP.ScriptServer.AlternarVentana(this, "VentanaServicioCopia", "contenedorVentanaCopiaServicio", "ventanaCopiaServicio");            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucTemperaturaUnidad_ClickGuardarTemperaturas(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            wucTemperaturaUnidad.GuardaTemperaturas();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTemperaturas_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VentanaTemperatura", "contenedorVentanaTemperatura", "ventanaTemperatura");
        }       
    }
}