using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.Base;
using SAT_CL;
using TSDK.ASP;
using SAT_CL.Global;
using System.Data;
using TSDK.Datos;
namespace SAT.General
{
    public partial class Operador : System.Web.UI.Page
    {
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

            //Si no es una recarga de págin
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Click en botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            guardaOperador();
        }
        /// <summary>
        /// Clieck en botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus actual es edición
            if (((TSDK.ASP.Pagina.Estatus)Session["estatus"]) == TSDK.ASP.Pagina.Estatus.Edicion)
                //Pasando a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;

            //Inicializando forma
            inicializaForma();
        }
        /// <summary>
        /// Click en Link de Apertura de control Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVentana_Click(object sender, EventArgs e)
        {
            //Habilitando el Control
            ucDireccion.Enable = true;
            //Validando que exista un registro previo
            if (Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) != 0)
                //Inicializando Control con el Registro Previo
                ucDireccion.InicializaControl(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)));
            else//Inicializando Control por Defecto
                ucDireccion.InicializaControl();
            gestionaVentanasModales(this, "Direccion");
           
        }
        /// <summary>
        /// Click en elementos del menú de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch (((LinkButton)sender).CommandName)
            {
                case "Nuevo":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = TSDK.ASP.Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando Mensaje de Error
                        lblError.Text = "";
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Abrir":
                    {
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(76, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Invocando Método de Guardado
                        guardaOperador();
                        break;
                    }
                case "Editar":
                    {
                        //Asignando estatus nuevo
                        Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaForma();
                        break;
                    }
                case "Baja":
                        //Inicializando fecha de baja
                        txtCFechaBaja.Text = "";
                        //Mostrando ventana de confirmación
                        TSDK.ASP.ScriptServer.AlternarVentana(upMenuPrincipal, upMenuPrincipal.GetType(), "BajaOperador", "modalBajaOperador", "confirmacionBajaOperador");
                        //Estableciendo foco
                        txtCFechaBaja.Focus();
                    break;
                case "Reactivar":
                    {
                        reactivaOperador();
                        break;
                    }
                case "Vencimientos":
                    {
                        //Inicializando contenido de vencimientos
                        wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Operador, Convert.ToInt32(Session["id_registro"]), true);
                        //Mostrando ventana de cambio de operador
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbVencimientos, lkbVencimientos.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
                        break;
                    }
                case "Historial":
                    {
                        //Construyendo URL de ventana de historial de unidad
                        string url = Cadena.RutaRelativaAAbsoluta("~/General/Unidad.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + Session["id_registro"].ToString() + "&idRegistroB=2");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=900,height=500";
                        //Abriendo Nueva Ventana
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "76", "Operador");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "76", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    inicializaArchivosRegistro(Session["id_registro"].ToString(), "76", "0");
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
                case "Renuncia":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Renuncia", Convert.ToInt32(Session["id_registro"])), "Renuncia", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "ContratoIndeterminado":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ContratoIndeterminado", Convert.ToInt32(Session["id_registro"])), "ContratoIndeterminado", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "ContratoTiempoDefinido":
                    {
                        //Limpia los controles de la ventana Modal
                        txtPeriodoInicial.Text = "";
                        txtPeriodoFinal.Text = "";
                        //Abre una ventana modal
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbContratoTiempoDefinido, "AbrirVentana", "contenedorModalContratoDefinido", "ModalContratoDefinido"); 
                        break;
                    }
                case "RefrescaEcosistema":
                    //Si ya existe un registro activo
                    if (Convert.ToInt32(Session["id_registro"]) > 0)
                        consumoActualizaOperador();

                    break;
                case "CuentasBanco":
                    //Validamos Exista Compañia
                    if (Session["id_registro"].ToString() != "0")
                    {
                        //Mostramos Modal
                        alternaVentanaModal("cuentaBancos", lkbCuentasBanco);

                        //Inicializamos Control
                        wucCuentaBancos.InicializaControl(76, Convert.ToInt32(Session["id_registro"]));


                    }
                    break;
                case "ProveedorWS":
                    {
                        //Validando Estatus
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Edicion:
                            case Pagina.Estatus.Lectura:
                                {
                                    //Cerrando ventana de edición de vencimiento
                                    TSDK.ASP.ScriptServer.AlternarVentana((LinkButton)sender, "ProveedorWS", "contenedorVentanaProveedorWS", "ventanaProveedorWS");
                                    //Inicializamos Control de Historial de Lecturas
                                    wucProveedorGPSDiccionario.InicializaControl(76, Convert.ToInt32(Session["id_registro"]));
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Mostramos Modal
            alternaVentanaModal("cuentaBancos", (LinkButton)sender);
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana del Proveedor de WS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarProveedorWS_Click(object sender, EventArgs e)
        {
            //Cerrando ventana de edición de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarProveedorWS, "ProveedorWS", "contenedorVentanaProveedorWS", "ventanaProveedorWS");
        }

        /// <summary>
        /// Evento Producido al Guardar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickGuardarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Obteniendo resultado de la Operación
            result = ucDireccion.GuardaDireccion();
            //Instanciando Direccion
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(result.IdRegistro))
            {   //Validando que exista
                if (dir.id_direccion != 0)
                {   //Mostrando Descripcion
                    txtDireccion.Text = txtDireccion.ToolTip = string.Format("{0}   ID:{1}", dir.ObtieneDireccionCompleta(), dir.id_direccion);
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = txtDireccion.ToolTip = "";
            }
            if(result.OperacionExitosa)
            gestionaVentanasModales(this, "CerrarVentanaDireccion");

        }
        /// <summary>
        /// Evento Producido al Eliminar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickEliminarDireccion(object sender, EventArgs e)
        {   //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Eliminando Dirección
            result = ucDireccion.EliminaDireccion();
            //Validando si la Operación fue Exitosa
            if (result.OperacionExitosa)
            {   //Validando si el registro eliminado es igual al Seleccionado
                if (Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)) == result.IdRegistro)
                    //Limpiando Control
                    txtDireccion.Text = txtDireccion.ToolTip = "";
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar la Dirección
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDireccion_ClickSeleccionarDireccion(object sender, EventArgs e)
        {   //Instanciando Dirección
            using (SAT_CL.Global.Direccion dir = new SAT_CL.Global.Direccion(ucDireccion.SeleccionaDireccion()))
            {   //Validando que exista una Dirección
                if (dir.id_direccion != 0)
                {
                    //Mostrando Descripción
                    txtDireccion.Text = txtDireccion.ToolTip = string.Format("{0}   ID:{1}", dir.ObtieneDireccionCompleta(), dir.id_direccion);
                    //Deshabilitando el Control
                    ucDireccion.Enable = false;
                }
                else//Limpiando Control
                    txtDireccion.Text = txtDireccion.ToolTip = "";
            }
        }
        /// <summary>
        /// Evento click en botón de cancelación de baja de operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarBajaOperador_Click(object sender, EventArgs e)
        {
            gestionaVentanasModales(this, "BajaOperador");
            
        }
        /// <summary>
        /// Evento click en botón de confirmación de baja de operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarBajaOperador_Click(object sender, EventArgs e)
        {
            //Realizando baja de operador
            bajaOperador();
        }
        /// <summary>
        /// Evento click del botón cerrar ventana modal de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVencimientos_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            switch (((LinkButton)sender).CommandName)
            {
                case "Historial":
                    //Cerrar ventana de vencimientos
                    TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarHistorialVencimientos, lkbCerrarHistorialVencimientos.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
                    break;
                case "Vencimiento":
                    //Cerrar ventana de vencimientos
                    TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarVencimientoSeleccionado, lkbCerrarVencimientoSeleccionado.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
                    TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
                    break;
            }
        }
        /// <summary>
        /// Evento click del botón Consultar Vencimiento (Control Usuario Historial)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnlkbConsultar_Click(object sender, EventArgs e)
        {
            //Inicializar vencimiento en lectura
            wucVencimiento.InicializaControl(wucVencimientosHistorial.id_vencimiento, false);
            //Cerrar ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            //Abrir ventana de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
        }
        /// <summary>
        /// Evento click sobre link terminar vencimiento Control (Usuario Historial)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnlkbTerminar_Click(object sender, EventArgs e)
        {
            //Inicializar vencimiento con habilitación de término de vencimiento
            wucVencimiento.InicializaControl(wucVencimientosHistorial.id_vencimiento, true);
            //Cerrar ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            //Abrir ventana de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
        }
        /// <summary>
        /// Evento click sobre botón nuevo vencimiento (Control Usuario Historial)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnbtnNuevoVencimiento_Click(object sender, EventArgs e)
        {
            //Abriendo ventana de vencimiento para nuevo registro
            wucVencimiento.InicializaControl(76, Convert.ToInt32(Session["id_registro"]));
            //Cerrar ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            //Abrir ventana de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
        }
        /// <summary>
        /// Evento click en guardar vencimiento (Control Usuario Vencimiento)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickGuardarVencimiento_Click(object sender, EventArgs e)
        {
            //Realizando guardado de vencimiento
            RetornoOperacion resultado = wucVencimiento.GuardaVencimiento();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de vencimientos
                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Operador, Convert.ToInt32(Session["id_registro"]), true);
                //Cerrando ventana de edición de vencimiento
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
                //Abriendo ventana de vencimientos
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            }
        }
        /// <summary>
        /// Evento click en botón Terminar vencimiento (Control Usuario Vencimiento)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickTerminarVencimiento_Click(object sender, EventArgs e)
        {
            //Realizando guardado de vencimiento
            RetornoOperacion resultado = wucVencimiento.TerminaVencimiento();
            //Sino hay error
            if (resultado.OperacionExitosa)
            {
                //Actualizando lista de vencimientos
                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Operador, Convert.ToInt32(Session["id_registro"]), true);
                //Cerrando ventana de edición de vencimiento
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
                //Abriendo ventana de vencimientos
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            }
        }

        /// <summary>
        /// Evento que permite imprimir el contrato de tiempo definido de un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            //Valida que exista fechas
            if (txtPeriodoInicial.Text != "")
            {
                //Variables de tipo datetime que almacenan los valores capturados en las cajas de texto
                DateTime fechaInicio = Convert.ToDateTime(txtPeriodoInicial.Text);
                DateTime fechaFin = Convert.ToDateTime(txtPeriodoFinal.Text);
                //Obteniendo Ruta
                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/RDLC/Reporte.aspx");
                //Instanciando nueva ventana de navegador para apertura de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}&fechaInicio={3}&fechaFin={4}", urlReporte, "ContratoTiempoDefinido", Convert.ToInt32(Session["id_registro"]), Convert.ToDateTime(fechaInicio), Convert.ToDateTime(fechaFin)), "ContratoTiempoDefinido", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                //Cierra la ventan modal
                gestionaVentanasModales(btnImprimir, "CerrarVentanaContrato");                
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
                case "cuentaBancos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaAltaCuentas", "ventanaAltaCuentas");
                    break;
            }
        }

        #region Eventos Calificacion
        /// <summary>
        /// Evento que cierra ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentana_Click(object sender, EventArgs e)
        {
            //Obtiene el control linkButom para definir que accion se realizara
            LinkButton lnk = (LinkButton)sender;
            //Valida el linkbutton y determina la acción a ejecutar
            switch (lnk.CommandName)
            {
                //Cierra la ventana modal de Calificación
                case "CierraCalificacion":                    
                        //Invoca al método que carga los indicadores de calificación y cierra la ventan modal del Control de usuario CAlificación
                        gestionaVentanasModales(lnk, "CerrarVentanaCalificacion");
                        break;
                    
                //Cierra la ventana modal de Historial Calificación
                case "CierraHistorialCalificacion":
                    //Cierra la ventana modal de Historial
                    gestionaVentanasModales(lnk, "CerrarVentanaHistorial");
                    break;
                case "CierraDireccion":                    
                        gestionaVentanasModales(lnk, "CerrarVentanaDireccion");                        
                        break;          
                case "CierraContratoDefinido":
                        gestionaVentanasModales(lnk, "CerrarVentanaContrato");
                        break;

            }
            inicializaForma();

        }
        /// <summary>
        /// Evento que alamcena la calificación general calificada
        /// </summary>
        protected void wucCalificacion_ClickGuardarCalificacionGeneral(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asigna al objeto retorno el resultado del método del control de usuario que guardar calificación general calificada
            retorno = wucCalificacion.GuardarCalificacionGeneral();
            //Valida la operación de almacenamiento de la operación
            if (retorno.OperacionExitosa)
            {
                inicializaForma();
                gestionaVentanasModales(this, "CierraVentanaCalificacion");                
            }
        }
        /// <summary>
        /// Evento que inicializan los valores del control de usuario Calificacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imgbCalificacion_Click(object sender, ImageClickEventArgs e)
        {
            //Inicializa el control de usuario CAlificacion                
            wucCalificacion.InicializaControl(76,(int) Session["id_registro"], 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, 0, true);            
            //Invoca al método que abre la ventana modal
            gestionaVentanasModales(this, "Calificacion");
        }
        /// <summary>
        /// Evento que inicializa el control de usuario Historial Calificación 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbComentarios_Click(object sender, EventArgs e)
        {
            //Inicializa el control de usuario CAlificacion                
            wucHistorialCalificacion.InicializaControl(76,(int) Session["id_registro"]);            
            //Abre la ventana modal
            gestionaVentanasModales(this, "HistorialCalificacion");
        }
        #endregion
        
        
        
        
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catalogos
            cargaCatalogos();
            //Habilitando controles
            habilitaControles();
            //Habilitando menú
            habilitaMenu();
            //Cargando contenido de controles
            cargaContenidoControles();
            //Inicializando Control de direcciones
            ucDireccion.InicializaControl();
        }
        /// <summary>
        /// Instancía un objeto SAT_CL.Global.Operador y asigna el valor de sus atributos sobre controles de asp
        /// </summary>
        private void cargaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Borrando el contenido 
                    imgOperador.ImageUrl = "~/Image/default-user.png";
                    lblId.Text = "Por Asignar";
                    //Instanciando Compañía de la sesión de usuario
                    using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                    txtNombre.Text =
                    txtFechaNacimiento.Text =
                    txtRFC.Text =
                    txtCURP.Text =
                    txtNSS.Text =
                    txtRControl.Text = "";
                    ddlTipoLicencia.SelectedValue = "0";
                    txtNoLicencia.Text = "0";
                    txtDireccion.Text = txtDireccion.ToolTip =
                    txtTelefono.Text =
                    txtTelefonoCasa.Text =
                    txtFechaIngreso.Text =
                    txtFechaBaja.Text = "";
                    ddlEstatus.SelectedValue = "1";
                    ddlTipoOperador.SelectedValue = "1";
                    txtUbicacionActual.Text =
                    txtFechaActualizacion.Text = "";
                    lblCodAuth.Text = "----";
                    //Controles de la Ventana Modal
                    txtPeriodoInicial.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                    txtPeriodoFinal.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(1).ToString("dd/MM/yyyy");
                    imgbCalificacion.Visible = false;
                    lkbComentarios.Visible = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Controles de la Ventana Modal
                    txtPeriodoInicial.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");
                    txtPeriodoFinal.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(1).ToString("dd/MM/yyyy");
                    //Instanciando registro operador
                    using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                    {                        
                        //Si el registro existe
                        if (o.habilitar)
                        {
                            cargaCalificacion();
                            //Borrando el contenido 
                            lblId.Text = o.id_operador.ToString();
                            //Instanciando Compañía de la sesión de usuario
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(o.id_compania_emisor))
                                txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                            ddlTipoOperador.SelectedValue = o.id_tipo.ToString();
                            txtNombre.Text = o.nombre;
                            txtFechaNacimiento.Text = o.fecha_nacimiento.ToString("dd/MM/yyyy");
                            txtRFC.Text = o.rfc;
                            txtCURP.Text = o.curp;
                            txtNSS.Text = o.nss;
                            txtRControl.Text = o.r_control;
                            ddlTipoLicencia.SelectedValue = o.id_tipo_licencia.ToString();
                            txtNoLicencia.Text = o.numero_licencia;
                            //Instanciando dirección
                            using (SAT_CL.Global.Direccion d = new SAT_CL.Global.Direccion(o.id_direccion))
                                txtDireccion.Text = txtDireccion.ToolTip = string.Format("{0}   ID:{1}", d.ObtieneDireccionCompleta(), o.id_direccion);
                            txtTelefono.Text = o.telefono;
                            txtTelefonoCasa.Text = o.telefono_casa;
                            txtFechaIngreso.Text = o.fecha_ingreso.ToString("dd/MM/yyyy");
                            txtFechaBaja.Text = o.fecha_baja.CompareTo(DateTime.MinValue) != 0 ? o.fecha_baja.ToString("dd/MM/yyyy") : "";
                            ddlEstatus.SelectedValue = o.id_estatus.ToString();
                            //Determinando la ubicación del operador en base a estatus, id de parada y movimiento
                            string ubicacionActual = "";
                            switch (o.estatus)
                            {
                                case SAT_CL.Global.Operador.Estatus.Disponible:
                                case SAT_CL.Global.Operador.Estatus.Ocupado:
                                    //Instanciando Parada actual
                                    using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(o.id_parada))
                                        ubicacionActual = p.descripcion;
                                    break;
                                case SAT_CL.Global.Operador.Estatus.Transito:
                                    //Instanciando movimiento
                                    using (SAT_CL.Despacho.Movimiento m = new SAT_CL.Despacho.Movimiento(o.id_movimiento))
                                        ubicacionActual = m.descripcion;
                                    break;
                                default:
                                    ubicacionActual = "No Disponible";
                                    break;
                            }
                            txtUbicacionActual.Text = ubicacionActual;
                            txtFechaActualizacion.Text = o.fecha_actualizacion.CompareTo(DateTime.MinValue) != 0 ? o.fecha_actualizacion.ToString("dd/MM/yyyy HH:mm") : "";
                            lblCodAuth.Text = o.cod_authenticacion;
                            //Busca la foto del operador                
                            using (DataTable dtFotoOperador = SAT_CL.Global.ArchivoRegistro.CargaArchivoRegistro(76, o.id_operador, 21, 0))
                            {
                                //Valida los datos del dataset
                                if (Validacion.ValidaOrigenDatos(dtFotoOperador))
                                {
                                    //Recorre el dataset y la ruta de la foto del operador lo asigna al control de image Foto Operador
                                    foreach (DataRow r in dtFotoOperador.Rows)
                                        //Asigna la ubicación  de la foto del operador al control de imagen
                                        imgOperador.ImageUrl = String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=256&ancho=256&url={0}", Convert.ToString(r["URL"]));
                                }
                                else
                                {
                                    imgOperador.ImageUrl = "~/Image/default-user.png";
                                }
                            }
                        }
                    }
                    break;
            }

            //Limpiando errores
            lblError.Text = "";
        }
        /// <summary>
        /// Habilita o deshabilita loc controles de la forma en base a su estatus
        /// </summary>
        private void habilitaControles()
        { 
            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    txtNombre.Enabled =
                    ddlTipoOperador.Enabled =
                    txtFechaNacimiento.Enabled =
                    txtRFC.Enabled =
                    txtCURP.Enabled =
                    txtNSS.Enabled =
                    txtRControl.Enabled =
                    ddlTipoLicencia.Enabled =
                    txtNoLicencia.Enabled =
                    lnkVentana.Enabled =
                    txtTelefono.Enabled =
                    txtTelefonoCasa.Enabled =
                    txtFechaIngreso.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    //VentanaModal
                    txtPeriodoInicial.Enabled =
                    txtPeriodoFinal.Enabled =
                    btnImprimir.Enabled = true;;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    txtNombre.Enabled =
                    ddlTipoOperador.Enabled =
                    txtFechaNacimiento.Enabled =
                    txtRFC.Enabled =
                    txtCURP.Enabled =
                    txtNSS.Enabled =
                    txtRControl.Enabled =
                    ddlTipoLicencia.Enabled =
                    txtNoLicencia.Enabled =
                    lnkVentana.Enabled =
                    txtTelefono.Enabled =
                    txtTelefonoCasa.Enabled =
                    txtFechaIngreso.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled =
                    //VentanaModal
                    txtPeriodoInicial.Enabled =
                    txtPeriodoFinal.Enabled =
                    btnImprimir.Enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar el Menu
        /// </summary>
        private void habilitaMenu()
        {   //Validando Estatus de Session
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = 
                        lkbReactivar.Enabled =
                        lkbBajaOperador.Enabled = 
                        lkbVencimientos.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;
                        //Vencimientos
                        lkbImprimirRenuncia.Enabled =
                        lkbContratoTiempoDefinido.Enabled =
                        lkbContratoIndeterminado.Enabled = false;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbImprimirRenuncia.Enabled=
                        lkbSalir.Enabled = true;
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbReactivar.Enabled =
                        lkbBajaOperador.Enabled =
                        lkbVencimientos.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;
                        //Vencimientos
                        lkbImprimirRenuncia.Enabled =
                        lkbContratoTiempoDefinido.Enabled =
                        lkbContratoIndeterminado.Enabled = true;
                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbImprimirRenuncia.Enabled=
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbReactivar.Enabled =
                        lkbBajaOperador.Enabled =
                        lkbVencimientos.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;
                        //Vencimientos
                        lkbImprimirRenuncia.Enabled =
                        lkbContratoTiempoDefinido.Enabled =
                        lkbContratoIndeterminado.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Carga el conjunto de catalogos de la forma sobre los controles DropDownList respectivos
        /// </summary>
        private void cargaCatalogos()
        { 
            //Tipos de Licencia
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoLicencia, "No Aplica", 1105);
            //Estatus de Operador
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 57);
            //Tipo de Operador
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperador, "", 3146);
        }
        /// <summary>
        /// Inserta o Actualiza los valores del registro
        /// </summary>
        private void guardaOperador()
        { 
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    resultado = SAT_CL.Global.Operador.InsertaOperador(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), (SAT_CL.Global.Operador.Tipo)Convert.ToInt32(ddlTipoOperador.SelectedValue), 
                                        txtNombre.Text.ToUpper(), Convert.ToDateTime(txtFechaNacimiento.Text), txtRFC.Text.ToUpper(), txtCURP.Text.ToUpper(), txtNSS.Text.ToUpper(), txtRControl.Text.ToUpper(),
                                        txtTelefono.Text.ToUpper(), txtTelefonoCasa.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)),
                                        Convert.ToDateTime(txtFechaIngreso.Text), Convert.ToByte(ddlTipoLicencia.SelectedValue),
                                        txtNoLicencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando operador actual
                    using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                    { 
                        //Si el operador existe
                        if (o.habilitar)
                        {
                            resultado = o.EditaOperador(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), (SAT_CL.Global.Operador.Tipo)Convert.ToInt32(ddlTipoOperador.SelectedValue), 
                                        txtNombre.Text.ToUpper(), Convert.ToDateTime(txtFechaNacimiento.Text), txtRFC.Text.ToUpper(), txtCURP.Text.ToUpper(), txtNSS.Text.ToUpper(), txtRControl.Text.ToUpper(),
                                        txtTelefono.Text.ToUpper(), txtTelefonoCasa.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDireccion.Text, "ID:", 1)),
                                        Convert.ToDateTime(txtFechaIngreso.Text), Convert.ToByte(ddlTipoLicencia.SelectedValue),
                                        txtNoLicencia.Text.ToUpper(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        }
                        else
                            resultado = new RetornoOperacion("El operador no fue encontrado.");
                    }
                    break;
            }   

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            { 
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaForma();
            }
         
            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza la baja del registro operador
        /// </summary>
        private void bajaOperador()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando operador actual
                using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                {
                    //Si el operador existe
                    if (o.habilitar)
                    {
                        //Actualizando estatus a baja
                        resultado = o.ActualizaEstatusABaja(Convert.ToDateTime(txtCFechaBaja.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        //Si no hay errores
                        if (resultado.OperacionExitosa && o.ActualizaAtributosInstancia())
                            //Actualizando información de ubicación actual
                            resultado = o.ActualizaParadaYMovimiento(0, 0, Convert.ToDateTime(txtCFechaBaja.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    else
                        resultado = new RetornoOperacion("El operador no fue encontrado.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaForma();
            }

            //Ocultando ventana de confirmación de baja
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptarBajaOperador, upbtnAceptarBajaOperador.GetType(), "BajaOperador", "modalBajaOperador", "confirmacionBajaOperador");

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Reactiva el registro operador
        /// </summary>
        private void reactivaOperador()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando operador actual
            using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
            {
                //Si el operador existe
                if (o.habilitar)
                    resultado = o.ActualizaEstatusADisponible(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    resultado = new RetornoOperacion("El operador no fue encontrado.");
            }

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaForma();
            }

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = string.Format("{0}?P1={1}", Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/Accesorios/AbrirRegistro.aspx"), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=650,height=600";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
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
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Configura la ventana de carga y descarga de archivos relacionados al registro indicado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_archivo_tipo_configuracion">Id Configuración de tipo de archivo a consultar</param>
        private void inicializaArchivosRegistro(string id_registro, string id_tabla, string id_archivo_tipo_configuracion)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/General/Operador.aspx", "~/Accesorios/ArchivoRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idTV=" + id_archivo_tipo_configuracion);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Archivos", 800, 480, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Actualiza los Datos del Operador de Ecosistema
        /// </summary>
        /// <returns></returns>
        private void consumoActualizaOperador()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Operador objOperador = new SAT_CL.Global.Operador(Convert.ToInt32(Session["id_registro"])))
                {                 
                        //Obtenemos Resultado                     
                        string resultado_web_service = global.EditaOperadorCentral(objOperador.id_operador, objOperador.nombre, Convert.ToByte((TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddTicks(-objOperador.fecha_nacimiento.Ticks).Year - 1)),
                                                   objOperador.r_control, Vencimiento.ValidaLicenciaVigente(objOperador.id_operador), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
                                   TSDK.Base.Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 25, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "Consumo Web Service", "Contraseña"), ""),
                                                 ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).nombre);



                        //Obtenemos Documento generado
                        XDocument xDoc = XDocument.Parse(resultado_web_service);

                        //Validamos que exista Respuesta
                        if (xDoc != null)
                        {
                            //Traduciendo resultado
                            resultado = new RetornoOperacion(Convert.ToInt32(xDoc.Descendants("idRegistro").FirstOrDefault().Value), xDoc.Descendants("mensaje").FirstOrDefault().Value.ToString(), Convert.ToBoolean(xDoc.Descendants("operacionExitosa").FirstOrDefault().Value));

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Personalizamos Mensaje
                                resultado = new RetornoOperacion("El Operador ha sido actualizado", true);
                            }

                        }
                        else
                        {
                            //Establecmos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                        }
                    }
                //Cerramos Web Service
                global.Close();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(lkbRefrescaEcosistema, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        private void gestionaVentanasModales(Control sender, string nombre_ventana)
        {
            switch (nombre_ventana)
            {
                case "Calificacion":
                    ScriptServer.AlternarVentana(sender, "Calificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");
                    break;
                case "HistorialCalificacion":
                    ScriptServer.AlternarVentana(sender, "HistorialCalificacion", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");
                    break;
                case "Direccion":
                    ScriptServer.AlternarVentana(sender, "Direccion", "contenedorDireccionModal", "direccionModal");
                    break;
                case "CierraVentanaCalificacion":
                    ScriptServer.AlternarVentana(sender, "CierraVentanaCalificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");
                    break;
                case "CerrarVentanaCalificacion":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaCalificacion", "contenedorVentanaCalificacion", "ventanaCalificacion");
                    break;
                case "CerrarVentanaHistorial":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaHistorial", "contenedorVentanaHistorialCalificacion", "ventanaHistorialCalificacion");
                    break;
                case "CerrarVentanaDireccion":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaDireccion", "contenedorDireccionModal", "direccionModal");
                    break;
                case "CerrarVentanaContrato":
                    ScriptServer.AlternarVentana(sender, "CerrarVentanaContrato", "contenedorModalContratoDefinido", "ModalContratoDefinido");
                    break;
                case "BajaOperador":
                    ScriptServer.AlternarVentana(sender, "BajaOperador", "modalBajaOperador", "confirmacionBajaOperador");
                    break;
            }
        }
        /// <summary>
        /// Método que obtiene las calificaciones de un operador
        /// </summary>       
        private void cargaCalificacion()
        {
            //Creación de la variable Calificación 
            byte Calificacion = 0;
            int CantidadComentarios = 0;
            //Instancia a la clase operador
            using(SAT_CL.Global.Operador op = new SAT_CL.Global.Operador((int)Session["id_registro"]))
            {
                //Valida que exista el registro de operador
                if (op.id_operador > 0)
                {
                    //Si el registro es de tipo operador
                    if (op.id_tipo == Convert.ToByte(SAT_CL.Global.Operador.Tipo.Operador))
                    {
                        //Si el estatus del operador es diferente de baja
                        if (op.id_estatus != Convert.ToByte(SAT_CL.Global.Operador.Estatus.Baja))
                        {
                            //Muestra  y abilita los botones que invocan a los controles de usuario Calificación e Historial
                            imgbCalificacion.Visible = 
                            lkbComentarios.Visible = 
                            lkbComentarios.Enabled =
                            imgbCalificacion.Enabled = true;
                        }
                        //Si el operador esta en estatus de baja
                        else
                        {
                            //Solo deshabilita el uso del boton que invoca al control de usuario calificación 
                            imgbCalificacion.Visible =
                            lkbComentarios.Visible = 
                            lkbComentarios.Enabled = true;
                            imgbCalificacion.Enabled = false;
                        }                    
                    }
                    //Si es un empleado o mécanico
                    else
                    {
                        //No muestra los controles
                        imgbCalificacion.Visible = false;
                        lkbComentarios.Visible = false;
                    }
                }
            
                //Obtiene el promedio de calificación del operador
                Calificacion = SAT_CL.Calificacion.Calificacion.ObtieneEntidad(76, op.id_operador);
                CantidadComentarios = SAT_CL.Calificacion.Calificacion.ObtieneNumeroComentarios(76, op.id_operador);
                //Acorde al promedio colocara el promedio
                switch (Calificacion)
                {
                    case 1:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella1.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 2:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella2.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 3:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella3.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 4:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella4.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    case 5:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella5.png";
                        lkbComentarios.Text = Calificacion + " / 5" + " ( " + CantidadComentarios + " opiniones  )";
                        break;
                    default:
                        imgbCalificacion.ImageUrl = "~/Image/Estrella.png";
                        lkbComentarios.Text = "0 / 5" + " ( 0 Opiniones  )";
                        break;
                }
            }
        }


        #endregion




        
    }
}