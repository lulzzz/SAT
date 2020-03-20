using SAT_CL;
using SAT_CL.Global;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.General
{
    public partial class Unidad : System.Web.UI.Page
    {

        #region Eventos

        /// <summary>
        /// Evento de de carga de la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no es una recraga de página
            if (!this.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// Evento click del botón guardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Si ya existe un registro activo
            if (Convert.ToInt32(Session["id_registro"]) > 0)
                guardaUnidad();
            //Si no hay registro
            else
            {
                txtUbicacionInicial.Text = "";
                txtFechaEstanciaInicial.Text = txtFechaAdquisicion.Text + " 00:00";
                //Mostrando ventana de ubicación inicial
                TSDK.ASP.ScriptServer.AlternarVentana(btnGuardar, btnGuardar.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");
                //Asignando foco
                txtUbicacionInicial.Focus();
            }
        }
        /// <summary>
        /// Evento click dek botón cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Si el estatus actual de la página es edición
            if ((TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Edicion)
                //Actualizando estatus a lectura
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;

            //Inicializando contenido de forma
            inicializaForma();
        }
        /// <summary>
        /// Evento click sobre algún elemento del menú de la página
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
                        inicializaAperturaRegistro(19, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                        break;
                    }
                case "Guardar":
                    {
                        //Si ya existe un registro activo
                        if (Convert.ToInt32(Session["id_registro"]) > 0)
                            guardaUnidad();
                        //Si no hay registro
                        else
                        {
                            txtUbicacionInicial.Text = "";
                            txtFechaEstanciaInicial.Text = txtFechaAdquisicion.Text + " 00:00";
                            //Mostrando ventana de ubicación inicial
                            TSDK.ASP.ScriptServer.AlternarVentana(lkbGuardar, lkbGuardar.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");
                            //Asignando foco
                            txtUbicacionInicial.Focus();
                        }
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
                    txtCFechaBaja.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    //Mostrando ventana de confirmación
                    TSDK.ASP.ScriptServer.AlternarVentana(lkbBajaUnidad, lkbBajaUnidad.GetType(), "BajaUnidad", "modalBajaUnidad", "confirmacionBajaUnidad");
                    //Estableciendo foco
                    txtCFechaBaja.Focus();
                    break;
                case "Reactivar":
                    {
                        //Limpiando contenido de ventana modal
                        txtUbicacionInicial.Text = "";
                        txtFechaEstanciaInicial.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                        //Mostrando ventana de ubicación inicial
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbReactivar, lkbReactivar.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");
                        //Asignando foco
                        txtUbicacionInicial.Focus();
                        break;
                    }
                case "CambioOperador":
                    {
                        //Inicializamos Control
                        wucCambioOperador.InicializaControl(Convert.ToInt32(Session["id_registro"]));
                        //Mostrando ventana de cambio de operador
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbCambioOperador, lkbCambioOperador.GetType(), "CambioOperador", "modalCambioOperador", "confirmacionCambioOperador");
                        break;
                    }
                case "Vencimientos":
                    {
                        //Inicializando contenido de vencimientos
                        wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Unidad, Convert.ToInt32(Session["id_registro"]), true);
                        //Mostrando ventana de cambio de operador
                        TSDK.ASP.ScriptServer.AlternarVentana(lkbVencimientos, lkbVencimientos.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
                        break;
                    }
                case "Lecturas":
                    {
                        //Validando Estatus
                        switch((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Edicion:
                            
                            case Pagina.Estatus.Lectura:
                                {
                                    //Inicializando contenido de Lecturas
                                    wucLecturaHistorial.InicializaControl(Convert.ToInt32(Session["id_registro"]), true);
                                    //Mostrando ventana de cambio de operador
                                    TSDK.ASP.ScriptServer.AlternarVentana(lkbLecturaHistorial, lkbLecturaHistorial.GetType(), "HistorialLectura", "modalLecturaHistorial", "lecturaHistorial");
                                    break;
                                }
                        }
                        
                        break;
                    }
                case "Historial":
                    {
                        //Construyendo URL de ventana de historial de unidad
                        string url = Cadena.RutaRelativaAAbsoluta("~/General/Unidad.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + Session["id_registro"].ToString() + "&idRegistroB=1");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=1150,height=600";
                        //Abriendo Nueva Ventana
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
                        break;
                    }
                case "Bitacora":
                    {   //Inicializando Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "19", "Operador");
                        break;
                    }
                case "Referencias":
                    {   //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "19", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
                case "RefrescaEcosistema":
                       //Si ya existe un registro activo
                    if (Convert.ToInt32(Session["id_registro"]) > 0)
                        consumoActualizaUnidad();

                    break;
                case "ProveedorGPS":
                    {
                        //Validando Estatus
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Edicion:
                            case Pagina.Estatus.Lectura:
                                {
                                    //Cerrando ventana de edición de vencimiento
                                    TSDK.ASP.ScriptServer.AlternarVentana((LinkButton)sender, "ProveedorGPS", "contenedorVentanaProveedorGPS", "ventanaProveedorGPS");

                                    //Inicializando Proveedor GPS
                                    inicializaProveedorGPS();
                                    break;
                                }
                        }


                        
                        break;
                    }
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
                                    wucProveedorGPSDiccionario.InicializaControl(19,Convert.ToInt32(Session["id_registro"]));
                                    break;
                                }
                        }



                        break;
                    }
                case "KmsAsignado":
                    {
                        //Validando Estatus
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Edicion:
                            case Pagina.Estatus.Lectura:
                                {
                                    //Cerrando ventana de edición de vencimiento
                                    TSDK.ASP.ScriptServer.AlternarVentana((LinkButton)sender, "KmsAsignado", "contenedorVentanaKmsAsignado", "ventanaKmsAsignado");
                                    //Limpiando Control
                                    txtKmsAsignadoNvo.Text = "";
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        #region Eventos Kms Asignado

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaKmsAsignado_Click(object sender, EventArgs e)
        {
            //Cerrar ventana de vencimientos
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarVentanaKmsAsignado, lkbCerrarVentanaKmsAsignado.GetType(), "KmsAsignado", "contenedorVentanaKmsAsignado", "ventanaKmsAsignado");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarKms_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion retorno = new RetornoOperacion("Debe de Existir una Unidad para Actualizar");
            
            //Validando Sesión
            switch (((Pagina.Estatus)Session["estatus"]))
            {
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando Existencia
                            if (unidad.habilitar)
                            {
                                //Obteniendo Kms Nuevo
                                decimal kms_nvo = 0.00M;
                                decimal.TryParse(txtKmsAsignadoNvo.Text, out kms_nvo);

                                //Validando Kms
                                if (kms_nvo > 0)
                                {
                                    //Validando Kilometraje Identico
                                    if (!(kms_nvo == unidad.kilometraje_asignado))
                                    {
                                        //Validando Kms con el Anterior
                                        decimal kms_diff = kms_nvo - unidad.kilometraje_asignado;
                                        decimal kms_min = 0.00M, kms_max = 0.00M;

                                        //Obteniendo Limites por Compania
                                        decimal.TryParse(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Kms Tolerancia (Min)", unidad.id_compania_emisor), out kms_min);
                                        decimal.TryParse(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Kms Tolerancia (Max)", unidad.id_compania_emisor), out kms_max);

                                        //Validando Diferencia
                                        if (kms_diff > 0)
                                        {
                                            //Validando KMS Máximo
                                            if (kms_diff <= kms_max)

                                                //Agregando Resultado positivo
                                                retorno = new RetornoOperacion(unidad.id_unidad);
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion(string.Format("El aumento máximo de kilometraje es de '{0:0.00}' puede actualizar hasta '{1:0.00}'", kms_max, unidad.kilometraje_asignado + kms_max));
                                        }
                                        else
                                        {
                                            //Validando KMS Minimo
                                            if (Math.Abs(kms_diff) <= kms_min)

                                                //Agregando Resultado positivowidt
                                                retorno = new RetornoOperacion(unidad.id_unidad);
                                            else
                                                //Instanciando Excepción
                                                retorno = new RetornoOperacion(string.Format("El decremento máximo de kilometraje es de '{0:0.00}' puede actualizar hasta '{1:0.00}'", kms_min, unidad.kilometraje_asignado - kms_min));
                                        }

                                        //Validando Operación
                                        if (retorno.OperacionExitosa)
                                        
                                            //Actualizando Kms
                                            retorno = unidad.ActualizaOdometroUnidad(kms_nvo, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                    else
                                        //Instanciando Excepción
                                        retorno = new RetornoOperacion("Ingrese un Kilometraje Distinto");
                                }
                                else
                                    //Instanciando Excepción
                                    retorno = new RetornoOperacion("Ingrese un Kilometraje Valido");
                            }
                            else
                                //Instanciando Excepción
                                retorno = new RetornoOperacion("No se puede recuperar la Unidad");
                        }

                        break;
                    }
            }

            //Validando Operación
            if (retorno.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = retorno.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Edicion;
                inicializaForma();

                //Cerrar ventana de vencimientos
                TSDK.ASP.ScriptServer.AlternarVentana(btnActualizarKms, btnActualizarKms.GetType(), "KmsAsignado", "contenedorVentanaKmsAsignado", "ventanaKmsAsignado");
            }

            //Mostrando Resultado
            ScriptServer.MuestraNotificacion(btnActualizarKms, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        /// <summary>
        /// Cambio de tipo de unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaCatalogosTipoUnidad();
            habilitaControlesTipoUnidad();
        }        
        /// <summary>
        /// Evento cambio de marca para opción otro propietario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPropietario_CheckedChanged(object sender, EventArgs e)
        {
            //(Des)Habilitando caja de texto propietario y estableciendo foco si se habilita
            if (chkPropietario.Checked)
            {
                txtPropietario.Enabled = true;
                txtPropietario.Text = "";
                txtPropietario.Focus();
            }
            else
            {
                txtPropietario.Enabled = false;
                txtPropietario.Text = "Unidad Propia   ID:0";
                chkPropietario.Focus();
            }
        }
        /// <summary>
        /// Evento click en botón de cancelación de baja de unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarBajaUnidad_Click(object sender, EventArgs e)
        {
            TSDK.ASP.ScriptServer.AlternarVentana(btnCancelarBajaUnidad, btnCancelarBajaUnidad.GetType(), "BajaUnidad", "modalBajaUnidad", "confirmacionBajaUnidad");
        }
        /// <summary>
        /// Evento click en botón de confirmación de baja de unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarBajaUnidad_Click(object sender, EventArgs e)
        {
            //Realizando baja de unidad
            bajaUnidad();
        }
        /// <summary>
        /// Evento click en botón aceptar ubicación inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarUbicacionInicial_Click(object sender, EventArgs e)
        {
            //Dependiendo del estatus de la página
            switch((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                //Ubicación para nueva unidad
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Confirmando guardado
                    guardaUnidad();
                    break;
                //Ubicación para reactivación de unidad
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    reactivaUnidad();
                    break;
            }            
        }
        /// <summary>
        /// Evento click en el botón cancelar ubicación inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarUbicacionInicial_Click(object sender, EventArgs e)
        {
            //Ocultando ventana
            TSDK.ASP.ScriptServer.AlternarVentana(btnCancelarUbicacionInicial, btnCancelarUbicacionInicial.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");
        }
        /// <summary>
        /// Evento click del botón aceptar cambio de operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCambioOperador_Click(object sender, EventArgs e)
        {
            //Guardando nuevo operador
            cambioOperadorUnidad();
        }
        /// <summary>
        /// Evento click del botón cerrar ventana modal de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVencimientos_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            switch(((LinkButton)sender).CommandName)
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
            wucVencimiento.InicializaControl(19, Convert.ToInt32(Session["id_registro"]));
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
                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Unidad, Convert.ToInt32(Session["id_registro"]), true);
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
                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Unidad, Convert.ToInt32(Session["id_registro"]), true);
                //Cerrando ventana de edición de vencimiento
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Vencimiento", "modalVencimiento", "vencimientoSeleccionado");
                //Abriendo ventana de vencimientos
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VencimientosHistorial", "modalHistorialVencimientos", "vencimientosRecurso");
            }
        }

        #region Eventos Proveedor GPS

        /// <summary>
        /// Evento Producido al Cerrar la Ventana del Proveedor de GPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarProveedorGPS_Click(object sender, EventArgs e)
        {
            //Cerrando ventana de edición de vencimiento
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarProveedorGPS, "ProveedorGPS", "contenedorVentanaProveedorGPS", "ventanaProveedorGPS");
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
        /// Evento Producido al Cambiar el Texto del Proveedor de GPS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtProveedor_TextChanged(object sender, EventArgs e)
        {
            //Validando Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista la Unidad
                            if (unidad.habilitar)
                            
                                //Cargando Servicios GPS
                                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlServicioGPS, 100, "-- Seleccione un Servicio GPS", unidad.id_compania_emisor,
                                                                               "", Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)), "");
                            else
                                //Inicializando Control
                                Controles.InicializaDropDownList(ddlServicioGPS, "-- Seleccione un Servicio GPS");
                        }
                        break;
                    }
                default:
                    {
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlServicioGPS, "-- Seleccione un Servicio GPS");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Guardar el Servicio GPS de la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarGPS_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Validando Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Validando que no exista el Servicio GPS
                        if (!ddlServicioGPS.SelectedValue.Equals("0"))
                        {
                            //Instanciando Unidad
                            using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                            {
                                //Validando Unidad
                                if (unidad.habilitar)
                                {
                                    //Validando que haya un Registro Seleccionado
                                    if (gvAsignaciones.SelectedIndex != -1)
                                    {
                                        //Instanciando Servicio GPS Unidad
                                        using (SAT_CL.Monitoreo.ProveedorWSUnidad pro_uni = new SAT_CL.Monitoreo.ProveedorWSUnidad(Convert.ToInt32(gvAsignaciones.SelectedDataKey["Id"])))
                                        {
                                            //Validando que Exista el Registro
                                            if (pro_uni.habilitar)

                                                //Editando Proveedor Unidad
                                                result = pro_uni.EditarProveedorWSUnidad(Convert.ToInt32(ddlServicioGPS.SelectedValue), unidad.id_unidad,
                                                                 txtIdentificador.Text, pro_uni.bit_antena_defecto, Convert.ToInt32(txtTEncendido.Text), Convert.ToInt32(txtTApagado.Text),
                                                                 ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("No existe la Relación de la Unidad");
                                        }
                                    }
                                    else
                                        //Insertando Proveedor Unidad
                                        result = SAT_CL.Monitoreo.ProveedorWSUnidad.InsertarProveedorWSUnidad(Convert.ToInt32(ddlServicioGPS.SelectedValue), unidad.id_unidad,
                                                                                    txtIdentificador.Text, chkDefault.Checked, Convert.ToInt32(txtTEncendido.Text), Convert.ToInt32(txtTApagado.Text),
                                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No existe la Unidad");
                            }
                        }
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existe el Servicio GPS");
                        break;
                    }
            }

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Inicilización
                inicializaProveedorGPS();

            //Mostrando Resultado de la Operación
            ScriptServer.MuestraNotificacion(btnGuardarGPS, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al Cancelar el Servicio GPS de la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarGPS_Click(object sender, EventArgs e)
        {
            //Limpiando Controles
            txtProveedor.Text =
            txtIdentificador.Text = 
            txtTEncendido.Text =
            txtTApagado.Text = "";
            chkDefault.Checked =
            chkDefault.Enabled = false;

            //Inicializando DropDownList
            Controles.InicializaDropDownList(ddlServicioGPS, "-- Seleccione un Servicio GPS");

            //Inicializando Indices
            Controles.InicializaIndices(gvAsignaciones);
        }

        #region Eventos GridView "gvAsignaciones"

        /// <summary>
        /// Evento Producido al Enlazar las Filas al GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Fila
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Encontrando controles de interés
                using (LinkButton lkbMarcar = (LinkButton)e.Row.FindControl("lnkMarcaDefault"))
                {
                    //Validando que exista el Control
                    if (lkbMarcar != null)
                    {

                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvAsignaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoGPS.Text = Controles.CambiaSortExpressionGridView(gvAsignaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 0);
        }
        /// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvAsignaciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoGPS.SelectedValue), true, 0);
        }
        /// <summary>
        /// Evento Producido al Exportar el Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarGPS_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }
        /// <summary>
        /// Evento Producido al Eliminar la Relación de Servicio GPS con la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarGPS_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAsignaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Proveedor Unidad
                using (SAT_CL.Monitoreo.ProveedorWSUnidad pu = new SAT_CL.Monitoreo.ProveedorWSUnidad(Convert.ToInt32(gvAsignaciones.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Relación
                    if (pu.habilitar)
                    {
                        //Validando que no sea la Antena Predeterminada
                        if (!pu.bit_antena_defecto)

                            //Deshabilita la Relación
                            result = pu.DeshabilitarProveedorWSUnidad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No puede Eliminar la Antena Predeterminada");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Relación");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)

                    //Invocando Método de Inicilización
                    inicializaProveedorGPS();
                else
                    //Inicializando Controles
                    Controles.InicializaIndices(gvAsignaciones);

                //Mostrando Resultado de la Operación
                ScriptServer.MuestraNotificacion(gvAsignaciones, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Editar la Relación de Servicio GPS con la Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarGPS_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAsignaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

                //Instanciando Relación
                using (SAT_CL.Monitoreo.ProveedorWSUnidad pro_uni = new SAT_CL.Monitoreo.ProveedorWSUnidad(Convert.ToInt32(gvAsignaciones.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Registro
                    if (pro_uni.habilitar)
                    {
                        //Instanciando Proveedor WS
                        using (SAT_CL.Monitoreo.ProveedorWS pro = new SAT_CL.Monitoreo.ProveedorWS(pro_uni.id_proveedor_ws))
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(pro.id_proveedor))
                        {
                            //Asignando Proveedor
                            txtProveedor.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();

                            //Cargando Servicios GPS
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlServicioGPS, 100, "-- Seleccione un Servicio GPS", pro.id_compania, "", pro.id_proveedor, "");

                            //Asignando Valores
                            ddlServicioGPS.SelectedValue = pro_uni.id_proveedor_ws.ToString();
                            txtIdentificador.Text = pro_uni.identificador_unidad;
                            txtTEncendido.Text = pro_uni.tiempo_encendido.ToString();
                            txtTApagado.Text = pro_uni.tiempo_apagado.ToString();
                            chkDefault.Checked = pro_uni.bit_antena_defecto;
                            chkDefault.Enabled = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Marcar la Relación de Servicio GPS con la Unidad como Predeterminada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkMarcaDefault_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAsignaciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Proveedor Unidad
                using (SAT_CL.Monitoreo.ProveedorWSUnidad pu = new SAT_CL.Monitoreo.ProveedorWSUnidad(Convert.ToInt32(gvAsignaciones.SelectedDataKey["Id"])))
                {
                    //Validando que exista la Relación
                    if (pu.habilitar)
                    {
                        //Validando que no sea la Antena Predeterminada
                        if (!pu.bit_antena_defecto)

                            //Deshabilita la Relación
                            result = pu.MarcaAntenaPorDefecto(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("Ya es la Antena Predeterminada");
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Relación");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)

                    //Invocando Método de Inicilización
                    inicializaProveedorGPS();
                else
                    //Inicializando Controles
                    Controles.InicializaIndices(gvAsignaciones);

                //Mostrando Resultado de la Operación
                ScriptServer.MuestraNotificacion(gvAsignaciones, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la forma (cargando catalogos, contenido y asignando habilitación de controles)
        /// </summary>
        private void inicializaForma()
        { 
            //Cargando catálogos
            cargaCatalogosGenerales();
            //Cargando contenido de controles
            inicializaContenidoControles();
            //Habilitando controles
            habilitaControles();
            //Habilitando elementos del menú
            habilitaMenu();
        }
        /// <summary>
        /// Inicializa el contenido de los controles (en blanco o predeterminado) con los datos de un registro
        /// </summary>
        private void inicializaContenidoControles()
        {
            //Determinando el estatus de la página
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Borrando el contenido 
                    lblId.Text = "Por Asignar";
                    //Instanciando Compañía de la sesión de usuario
                    using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                    txtNumUnidad.Text = "";
                    //ddlTipoUnidad.SelectedValue = "0";
                    //Cargando catálogos dependientes
                    cargaCatalogosTipoUnidad();
                    txtNoEjes.Text = "";
                    txtPropietario.Text = "Unidad Propia   ID:0";
                    chkPropietario.Checked = false;
                    txtFechaAdquisicion.Text =
                    txtFechaBaja.Text = "";
                    //ddlEstadoPlacas.SelectedValue = "0";
                    txtPlacas.Text = "";
                    txtOperador.Text = "Sin Asignar   ID:0";
                    ddlEstatus.SelectedValue = "1";
                    txtUbicacionActual.Text =
                    txtFechaActualizacion.Text = "";

                    ddlMarca.SelectedValue = "0";
                    txtModelo.Text =
                    txtSerie.Text =
                    txtAno.Text = "";
                    ddlMarcaMotor.SelectedValue = "0";
                    txtModeloMotor.Text =
                    txtSerieMotor.Text = "";
                    txtPeso.Text = "0";
                    ddlUnidadPeso.SelectedValue = "0";
                    txtCapacidadCombustible.Text = "0";
                    txtAntenaGPS.Text = "";
                    txtCombustibleAsignado.Text = 
                    txtKmAsigando.Text = "0";
                    break;
                case TSDK.ASP.Pagina.Estatus.Lectura:
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando registro unidad
                    using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si el registro existe
                        if (u.habilitar)
                        {
                            //Borrando el contenido 
                            lblId.Text = u.id_unidad.ToString();
                            //Instanciando Compañía de la sesión de usuario
                            using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(u.id_compania_emisor))
                                txtCompania.Text = string.Format("{0}   ID:{1}", c.nombre, c.id_compania_emisor_receptor);
                            txtNumUnidad.Text = u.numero_unidad;
                            ddlTipoUnidad.SelectedValue = u.id_tipo_unidad.ToString();
                            //Cargando catálogos dependientes
                            cargaCatalogosTipoUnidad();
                            ddlSubTipoUnidad.SelectedValue = u.id_sub_tipo_unidad.ToString();
                            ddlDimensiones.SelectedValue = u.id_dimension.ToString();
                            txtNoEjes.Text = u.ejes.ToString();
                            chkPropietario.Checked = u.bit_no_propia;
                            //Instanciando Proveedor
                            using (SAT_CL.Global.CompaniaEmisorReceptor p = new SAT_CL.Global.CompaniaEmisorReceptor(u.id_compania_proveedor))
                                txtPropietario.Text = u.bit_no_propia ? string.Format("{0}   ID:{1}", p.nombre, p.id_compania_emisor_receptor) : "Unidad Propia   ID:0";

                            txtFechaAdquisicion.Text = u.fecha_adquisicion.ToString("dd/MM/yyyy");
                            txtFechaBaja.Text = u.EstatusUnidad == SAT_CL.Global.Unidad.Estatus.Baja ? u.fecha_baja.ToString("dd/MM/yyyy") : "";
                            ddlEstadoPlacas.SelectedValue = u.id_estado_placas.ToString();
                            txtPlacas.Text = u.placas;
                            //Instanciando al operador
                            using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(u.id_operador))
                                txtOperador.Text = u.id_operador > 0 ? string.Format("{0}   ID:{1}", o.nombre, o.id_operador) : "Sin Asignar   ID:0";
                            ddlEstatus.SelectedValue = u.id_estatus_unidad.ToString();
                            //Determinando la ubicación del operador en base a estatus, id de parada y movimiento
                            string ubicacionActual = "";
                            switch (u.EstatusUnidad)
                            {
                                case SAT_CL.Global.Unidad.Estatus.ParadaDisponible:
                                case SAT_CL.Global.Unidad.Estatus.ParadaOcupado:
                                    //Instanciando Estancia actual
                                    using (SAT_CL.Despacho.EstanciaUnidad est = new SAT_CL.Despacho.EstanciaUnidad(u.id_estancia))
                                    //Instanciando Parada
                                    using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(est.id_parada))
                                        ubicacionActual = p.descripcion;
                                    break;
                                case SAT_CL.Global.Unidad.Estatus.Transito:
                                    //Instanciando movimiento
                                    using (SAT_CL.Despacho.Movimiento m = new SAT_CL.Despacho.Movimiento(u.id_movimiento))
                                        ubicacionActual = m.descripcion;
                                    break;
                                default:
                                    ubicacionActual = "No Disponible";
                                    break;
                            }
                            txtUbicacionActual.Text = ubicacionActual;
                            txtFechaActualizacion.Text = u.fecha_actualizacion.CompareTo(DateTime.MinValue) != 0 ? u.fecha_actualizacion.ToString("dd/MM/yyyy HH:mm") : "";

                            ddlMarca.SelectedValue = u.id_marca.ToString();
                            txtModelo.Text = u.modelo;
                            txtSerie.Text = u.serie;
                            txtAno.Text = u.ano > 0 ? u.ano.ToString() : "";
                            ddlMarcaMotor.SelectedValue = u.id_marca_motor.ToString();
                            txtModeloMotor.Text = u.modelo_motor;
                            txtSerieMotor.Text = u.serie_motor;
                            txtPeso.Text = u.peso_tara.ToString();
                            ddlUnidadPeso.SelectedValue = u.id_unidad_medida_peso.ToString();
                            txtCapacidadCombustible.Text = u.capacidad_combustible.ToString();
                            txtAntenaGPS.Text = u.antena_gps_principal;
                            txtCombustibleAsignado.Text = u.combustible_asignado.ToString();
                            txtKmAsigando.Text = u.kilometraje_asignado.ToString(); 
                        }
                    }
                    break;
            }

            //Limpiando errores
            lblError.Text = "";
        }
        /// <summary>
        /// Realiza la carga de los controles de selección que lo requieran
        /// </summary>
        private void cargaCatalogosGenerales()
        {
            //Tipos de Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "", 0, "", 0, "");            
            //Estado en que se expiden las placas
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstadoPlacas, 51, "");
            //Estatus operador
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 53);            
            //Unidad de Peso
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlUnidadPeso, 52, "No Asignado", 2, "", 0, "");
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGPS, "", 26);
        }
        /// <summary>
        /// Carga los catálogos dependientes del tipo de unidad seleccionada
        /// </summary>
        private void cargaCatalogosTipoUnidad()
        {
            //Cargando catálogos dependientes
            //Subtipo de Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubTipoUnidad, "No Asignado", 1109, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
            //Dimensiones tipo unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlDimensiones, "No Asignado", 1108, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
            //Marca de la unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMarca, "No Asignado", 55, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
            //Marca del motor
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMarcaMotor, "No Asignado", 1110, Convert.ToInt32(ddlTipoUnidad.SelectedValue));
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
                    txtNumUnidad.Enabled =
                    ddlTipoUnidad.Enabled =
                    ddlSubTipoUnidad.Enabled = 
                    ddlDimensiones.Enabled =
                    txtNoEjes.Enabled = 
                    txtPropietario.Enabled = 
                    chkPropietario.Enabled = 
                    txtFechaAdquisicion.Enabled =
                    ddlEstadoPlacas.Enabled =
                    txtPlacas.Enabled =
                    ddlMarca.Enabled =
                    txtModelo.Enabled =
                    txtSerie.Enabled = 
                    txtAno.Enabled = 
                    ddlMarcaMotor.Enabled = 
                    txtModeloMotor.Enabled =
                    txtSerieMotor.Enabled =
                    txtPeso.Enabled = 
                    ddlUnidadPeso.Enabled =
                    txtKmAsigando.Enabled =
                    txtCapacidadCombustible.Enabled =
                    txtAntenaGPS.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = false;
                    break;
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    txtNumUnidad.Enabled =
                    ddlTipoUnidad.Enabled =
                    ddlSubTipoUnidad.Enabled =
                    ddlDimensiones.Enabled =
                    txtNoEjes.Enabled =
                    chkPropietario.Enabled =                    
                    txtFechaAdquisicion.Enabled =
                    ddlEstadoPlacas.Enabled =
                    txtPlacas.Enabled =
                    ddlMarca.Enabled =
                    txtModelo.Enabled =
                    txtSerie.Enabled =
                    txtAno.Enabled =
                    ddlMarcaMotor.Enabled =
                    txtModeloMotor.Enabled =
                    txtSerieMotor.Enabled =
                    txtPeso.Enabled =
                    ddlUnidadPeso.Enabled =                    
                    txtCapacidadCombustible.Enabled =
                    txtAntenaGPS.Enabled =
                    btnGuardar.Enabled =
                    btnCancelar.Enabled = true;

                    txtKmAsigando.Enabled = (TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Nuevo ? true : false;

                    txtPropietario.Enabled = chkPropietario.Checked;

                    //Habilitando controles por tipo de unidad
                    habilitaControlesTipoUnidad();
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Obteniendo asignaciones en cualquier estatus de la unidad
                    using (System.Data.DataTable mit = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUltimasAsignacionesRecurso(SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Unidad, Convert.ToInt32(Session["id_registro"]), 1))
                    {          
                        //Instanciando unidad
                        using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                            //Si ya hay asignaciones del recurso o se tiene operador asignado no se permite cambio de tipo de unidad
                            ddlTipoUnidad.Enabled = (mit != null || unidad.id_operador > 0) ? false : true;

                        txtNumUnidad.Enabled =
                        ddlSubTipoUnidad.Enabled =
                        ddlDimensiones.Enabled =
                        txtNoEjes.Enabled =
                        chkPropietario.Enabled =
                        txtFechaAdquisicion.Enabled =
                        ddlEstadoPlacas.Enabled =
                        txtPlacas.Enabled =
                        ddlMarca.Enabled =
                        txtModelo.Enabled =
                        txtSerie.Enabled =
                        txtAno.Enabled =
                        ddlMarcaMotor.Enabled =
                        txtModeloMotor.Enabled =
                        txtSerieMotor.Enabled =
                        txtPeso.Enabled =
                        ddlUnidadPeso.Enabled =
                        txtCapacidadCombustible.Enabled =
                        txtAntenaGPS.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        //Se permite edición del elemento cuando no existen asignaciones del recurso hacia algún movimiento
                        txtKmAsigando.Enabled = mit != null ? false : true;

                        txtPropietario.Enabled = chkPropietario.Checked;

                        //Habilitando controles por tipo de unidad
                        habilitaControlesTipoUnidad();
                    }
                    break;
            }
        }
        /// <summary>
        /// Realiza la habilitación de los controles correspondientes según el tipo de unidad (motriz/arrastre)
        /// </summary>
        private void habilitaControlesTipoUnidad()
        { 
            //Instanciando tipo de unidad
            using (SAT_CL.Global.UnidadTipo tipo = new SAT_CL.Global.UnidadTipo(Convert.ToInt32(ddlTipoUnidad.SelectedValue)))
            { 
                //Si es unidad motríz
                if (tipo.bit_motriz)
                {
                    //Habilitando controles requeridos
                    ddlMarcaMotor.Enabled =
                    txtModeloMotor.Enabled =
                    txtSerieMotor.Enabled =
                    txtCapacidadCombustible.Enabled = true;

                    //Deshabilitando no requeridos
                    ddlDimensiones.Enabled = false;
                }
                //Si es arrastre
                else
                {
                    //Deshabilitando controles requeridos
                    ddlMarcaMotor.Enabled =
                    txtModeloMotor.Enabled =
                    txtSerieMotor.Enabled =
                    txtCapacidadCombustible.Enabled = false;

                    //Habilitando no requeridos
                    ddlDimensiones.Enabled = true;
                }
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
                        lkbBajaUnidad.Enabled =
                        lkbCambioOperador.Enabled =
                        lkbVencimientos.Enabled = false;
                        //Herramientas
                        lkbHistorial.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbKmsAsignado.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Lectura:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbKmsAsignado.Enabled =
                        lkbGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled =
                        lkbReactivar.Enabled =
                        lkbBajaUnidad.Enabled =
                        lkbCambioOperador.Enabled = 
                        lkbVencimientos.Enabled =
                        //Herramientas
                        lkbHistorial.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    {   //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        lkbSalir.Enabled = 
                        //Edicion
                        lkbEditar.Enabled =
                        lkbReactivar.Enabled =
                        lkbBajaUnidad.Enabled =
                        lkbCambioOperador.Enabled = 
                        lkbVencimientos.Enabled =
                        //Herramientas
                        lkbHistorial.Enabled = 
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbKmsAsignado.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
            }
        }
        /// <summary>
        /// Realiza el guardado (alta o edición) del registro actual 
        /// </summary>
        private void guardaUnidad()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //En base al estatus
            switch ((TSDK.ASP.Pagina.Estatus)Session["estatus"])
            {
                case TSDK.ASP.Pagina.Estatus.Nuevo:
                    //Validando fecha de estancia, debe ser mayor o igual que la fecha de adquisición
                    if (Convert.ToDateTime(txtFechaAdquisicion.Text).CompareTo(Convert.ToDateTime(txtFechaEstanciaInicial.Text)) < 1)
                    {
                        //Inicializando bloque transaccional
                        using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Insertando nueva unidad       
                            resultado = SAT_CL.Global.Unidad.InsertaUnidad(txtNumUnidad.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), Convert.ToInt32(ddlTipoUnidad.SelectedValue), Convert.ToByte(ddlSubTipoUnidad.SelectedValue),
                                                        Convert.ToByte(txtNoEjes.Text), Convert.ToInt32(ddlDimensiones.SelectedValue), chkPropietario.Checked, Convert.ToDateTime(txtFechaAdquisicion.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtPropietario.Text, "ID:", 1)),
                                                        Convert.ToByte(ddlMarca.SelectedValue), txtModelo.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaElementoNoVacio(txtAno.Text, "0")), txtSerie.Text.ToUpper(), Convert.ToByte(ddlMarcaMotor.SelectedValue), txtModeloMotor.Text.ToUpper(),
                                                        txtSerieMotor.Text.ToUpper(), Convert.ToByte(ddlEstadoPlacas.SelectedValue), txtPlacas.Text.ToUpper(), Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPeso.Text, "0")), Convert.ToByte(ddlUnidadPeso.SelectedValue),
                                                        Convert.ToDecimal(txtKmAsigando.Text), Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCapacidadCombustible.Text, "0")), txtAntenaGPS.Text, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            //Guardando Id de Unidad
                            int id_unidad = resultado.IdRegistro;

                            //Si no hay error en la inserción de unidad
                            if (resultado.OperacionExitosa)
                            {
                                //Obteniendo la parada predeterminada para la ubicación seleccionada
                                using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(SAT_CL.Despacho.Parada.ObtieneParadaComodinUbicacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionInicial.Text, "ID:", 1)), true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario)))
                                    if (p.habilitar)
                                        //Insertando nueva estancia para la misma
                                        resultado = SAT_CL.Despacho.EstanciaUnidad.InsertaEstanciaUnidad(p.id_parada, id_unidad, SAT_CL.Despacho.EstanciaUnidad.Tipo.Operativa, Convert.ToDateTime(txtFechaEstanciaInicial.Text), SAT_CL.Despacho.EstanciaUnidad.TipoActualizacionInicio.Manual, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        resultado = new RetornoOperacion("No fue posible crear la parada principal de la ubicación seleccionada.");
                            }

                            //Si no hay errores
                            if (resultado.OperacionExitosa)
                            {
                                int id_estancia = resultado.IdRegistro;
                                //Instanciandio unidad
                                using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(id_unidad))
                                    //Si la unidad se localizó
                                    if (u.habilitar)
                                        //Actualizando datos de ultima estancia o movimiento
                                        resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, Convert.ToDateTime(txtFechaEstanciaInicial.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si no hay errores
                                if (resultado.OperacionExitosa)
                                {
                                    //Señalando registro original del resultado
                                    resultado = new RetornoOperacion(id_unidad);
                                    //Confirmando operaciones realizadas
                                    scope.Complete();
                                }
                            }                            
                        }
                    }
                    //La fecha de inicio de estancia debe ser mayor o igual a la fecha de adquisición
                    else
                        resultado = new RetornoOperacion("La fecha de inicio de estancia debe ser mayor o igual a la fecha de adquisición de la unidad.");

                    //Ocultando ventana modal
                    TSDK.ASP.ScriptServer.AlternarVentana(btnAceptarUbicacionInicial, btnAceptarUbicacionInicial.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");
                    break;
                case TSDK.ASP.Pagina.Estatus.Edicion:
                    //Instanciando unidad actual
                    using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                    {
                        //Si la unidad existe
                        if (u.habilitar)
                        {
                            resultado = u.EditaUnidad(txtNumUnidad.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), Convert.ToInt32(ddlTipoUnidad.SelectedValue), Convert.ToByte(ddlSubTipoUnidad.SelectedValue),
                                                Convert.ToByte(txtNoEjes.Text), Convert.ToInt32(ddlDimensiones.SelectedValue), chkPropietario.Checked, Convert.ToDateTime(txtFechaAdquisicion.Text), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtPropietario.Text, "ID:", 1)),
                                                Convert.ToByte(ddlMarca.SelectedValue), txtModelo.Text.ToUpper(), Convert.ToInt32(Cadena.RegresaElementoNoVacio(txtAno.Text, "0")), txtSerie.Text.ToUpper(), Convert.ToByte(ddlMarcaMotor.SelectedValue), txtModeloMotor.Text.ToUpper(),
                                                txtSerieMotor.Text.ToUpper(), Convert.ToByte(ddlEstadoPlacas.SelectedValue), txtPlacas.Text.ToUpper(), Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtPeso.Text, "0")), Convert.ToByte(ddlUnidadPeso.SelectedValue),
                                                Convert.ToDecimal(Cadena.RegresaElementoNoVacio(txtCapacidadCombustible.Text)), txtAntenaGPS.Text, u.id_configuracion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
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
        /// Realiza la actualización a estatus baja de la unidad actual
        /// </summary>
        private void bajaUnidad()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializano transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando unidad actual
                using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                {
                    //Si la unidad existe
                    if (u.habilitar)
                    {
                        //Actualizando estatus de unidad a baja
                        resultado = u.ActualizaEstatusABaja(Convert.ToDateTime(txtCFechaBaja.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //Obteniendo estancia actual
                            using (SAT_CL.Despacho.EstanciaUnidad est = new SAT_CL.Despacho.EstanciaUnidad(SAT_CL.Despacho.EstanciaUnidad.ObtieneEstanciaUnidadIniciada(u.id_unidad)))
                            { 
                                //Si la estancia existe
                                if (est.habilitar)
                                {
                                    //Se termina la estancia
                                    resultado = est.TerminaEstanciaUnidad(Convert.ToDateTime(txtCFechaBaja.Text), SAT_CL.Despacho.EstanciaUnidad.TipoActualizacionFin.Manual, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                        //Actualizando datos de ultima estancia o movimiento
                                        resultado = u.ActualizaEstanciaYMovimiento(0, 0, Convert.ToDateTime(txtCFechaBaja.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        resultado = new RetornoOperacion("No fue posible terminar la estancia de unidad.");
                                }
                                //De lo contrario
                                else
                                    resultado = new RetornoOperacion("No fue posible obtener la estancia actual de la unidad para su término.");
                            }
                        }
                    }
                    else
                        resultado = new RetornoOperacion("La Unidad no fue encontrada.");

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(u.id_unidad);
                        //Confirmando cambios
                        scope.Complete();
                    }
                }                
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
            TSDK.ASP.ScriptServer.AlternarVentana(btnAceptarBajaUnidad, btnAceptarBajaUnidad.GetType(), "BajaUnidad", "modalBajaUnidad", "confirmacionBajaUnidad");

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Reactiva el registro unidad
        /// </summary>
        private void reactivaUnidad()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando unidad actual
                using (SAT_CL.Global.Unidad u = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                {
                    //Si la unidad existe
                    if (u.habilitar)
                    {
                        //Actualizando estatus de unidad
                        resultado = u.ActualizaEstatusADisponible(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                        //Si no hay errores
                        if (resultado.OperacionExitosa)
                        {
                            //obteniendo parada comodín de la ubicación solicitada
                            using (SAT_CL.Despacho.Parada p = new SAT_CL.Despacho.Parada(SAT_CL.Despacho.Parada.ObtieneParadaComodinUbicacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacionInicial.Text, "ID:", 1)), true, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario)))
                                if (p.habilitar)
                                {
                                    //Insertando estancia de reposicionamiento
                                    resultado = SAT_CL.Despacho.EstanciaUnidad.InsertaEstanciaUnidad(p.id_parada, u.id_unidad, SAT_CL.Despacho.EstanciaUnidad.Tipo.Operativa, Convert.ToDateTime(txtFechaEstanciaInicial.Text), SAT_CL.Despacho.EstanciaUnidad.TipoActualizacionInicio.Manual, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && u.ActualizaAtributosInstancia())
                                    {
                                        int id_estancia = resultado.IdRegistro;
                                        //Actualizando datos de ultima estancia o movimiento
                                        resultado = u.ActualizaEstanciaYMovimiento(id_estancia, 0, Convert.ToDateTime(txtFechaEstanciaInicial.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                    else
                                        resultado = new RetornoOperacion("No fue posible crear la nueva estancia de unidad.");
                                }
                                else
                                    resultado = new RetornoOperacion("No fue posible crear la parada principal de la ubicación seleccionada.");
                        }
                    }
                    else
                        resultado = new RetornoOperacion("La unidad no fue encontrada.");

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                    {
                        resultado = new RetornoOperacion(u.id_unidad);
                        //Finalizando transacción
                        scope.Complete();
                    }
                }
            }

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaForma();
            }

            //Ocultando ventana de estancia inicial
            TSDK.ASP.ScriptServer.AlternarVentana(btnAceptarUbicacionInicial, btnAceptarUbicacionInicial.GetType(), "UbicacionInicial", "modalEstanciaInicial", "confirmacionEstanciaInicial");

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Realiza el cambio del operador principal asignado a la unidad
        /// </summary>
        private void cambioOperadorUnidad()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Método encargado de Cambiar el Operador.
            resultado = wucCambioOperador.CambioOperador();

            //Si no hay errores de guardado
            if (resultado.OperacionExitosa)
            {
                //Guardando datos de registro para carga de estatus de lectura
                Session["id_registro"] = resultado.IdRegistro;
                Session["estatus"] = TSDK.ASP.Pagina.Estatus.Lectura;
                inicializaForma();
                //Cerrando ventana de cambio de operador
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "CambioOperador", "modalCambioOperador", "confirmacionCambioOperador");

            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
 
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = string.Format("{0}?P1={1}", Cadena.RutaRelativaAAbsoluta("~/General/Unidad.aspx", "~/Accesorios/AbrirRegistro.aspx"), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
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
            string url = Cadena.RutaRelativaAAbsoluta("~/General/Unidad.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
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
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/General/Unidad.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 650, false, false, false, true, true, Page);
        }     

        #endregion

        #region Métodos Cambio de Operador
        /// <summary>
        /// Método encargado de Registrar el Cambio de Operador
        /// </summary>
        protected void wucCambioOperador_ClickRegistrar(object sender, EventArgs e)
        {
            //Realizamos el Cambio de Operador
            cambioOperadorUnidad();
        }

        /// <summary>
        /// Método encargado de Cerrar la Ventana de Cambio de Operador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarCambioOperador_Click(object sender, EventArgs e)
        {
            //Mostrando ventana de cambio de operador
            TSDK.ASP.ScriptServer.AlternarVentana(uplkbCerrarCambioOperador, uplkbCerrarCambioOperador.GetType(), "CambioOperador", "modalCambioOperador", "confirmacionCambioOperador");
        }

        /// <summary>
        /// Actualiza los Datos de la Unidad de Ecosistema
        /// </summary>
        /// <returns></returns>
        private void consumoActualizaUnidad()
        {
            //Establecemos variable resultado
            RetornoOperacion resultado = null;

            //Consumimos Web Service
            using (GlobalCentral.GlobalClient global = new GlobalCentral.GlobalClient())
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                {
                    //Declaramos Variable paara validar existencia de Antena
                    bool antena = false;
                    //Validamos que Tenga Antena Principa
                    if (objUnidad.antena_gps_principal != "")
                    {
                        antena = true;
                    }
                    //Instanciamos Tipo de Unidad
                    using (SAT_CL.Global.UnidadTipo objUnidadTipo = new SAT_CL.Global.UnidadTipo(objUnidad.id_tipo_unidad))
                    {
                        //Obtenemos Resultado                     
                        string resultado_web_service = global.EditaUnidadCentral(objUnidad.id_unidad, objUnidad.numero_unidad, objUnidadTipo.descripcion_unidad,
                                                                                   Catalogo.RegresaDescripcionCatalogo(1109, objUnidad.id_sub_tipo_unidad), objUnidad.modelo,
                                                                                   objUnidad.ano, objUnidad.placas, antena, Catalogo.RegresaDescripcionCatalogo(1108, objUnidad.id_dimension),
                                                                                    CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Direccion del Servidor", 0), CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Nombre de BD", 0),
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
                                resultado = new RetornoOperacion("La Unidad ha sido actualizada", true);
                            }

                        }
                        else
                        {
                            //Establecmos Mensaje Resultado
                            resultado = new RetornoOperacion("No es posible obtener la respuesta de WS");
                        }
                    }
                }
                //Cerramos Web Service
                global.Close();
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(lkbRefrescaEcosistema, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion

        #region Historial de Lecturas

        /// <summary>
        /// Método encargadpo de Cerrar el Historial de Lecturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarLecturaHistorial_Click(object sender, EventArgs e)
        {
            //Método encargado de Cerrar la Ventana de Historial de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarLecturaHistorial, lkbCerrarLecturaHistorial.GetType(), "HistorialLectura", "modalLecturaHistorial", "lecturaHistorial");
        }

        /// <summary>
        /// Método encargado de Crear una Nueva Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLecturaHistorial_btnNuevaLectura(object sender, EventArgs e)
        {
            //Inicializamos Control para Registro de Lecturas
            wucLectura.InicializaControl(0, Convert.ToInt32(Session["id_registro"]));
            //Método encargado de Abrir la ventana de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Lectura", "modalLectura", "Lectura");
            //Método encargado de Cerrar la Ventana de Historial de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");
     
        }

        #endregion

        #region Lecturas

        /// <summary>
        /// Método encargado de Cerrar el Control de Usuario de las Lecturas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarLectura_Click(object sender, EventArgs e)
        {
            //Método encargado de Cerrar la ventana de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(lkbCerrarLectura, lkbCerrarLectura.GetType(), "Lectura", "modalLectura", "Lectura");

            //Inicializamos Control de Historial de Lecturas
            wucLecturaHistorial.InicializaControl(Convert.ToInt32(Session["id_registro"]), true);

            //Método encargado de Abrir la Ventana de Historial de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");

        }


        #endregion

        #region Métodos Proveedor GPS

        /// <summary>
        /// 
        /// </summary>
        private void inicializaProveedorGPS()
        {
            //Validando Sesión
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad unidad = new SAT_CL.Global.Unidad(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando Unidad
                            if (unidad.habilitar)
                            {
                                //Inicializando Controles
                                txtProveedor.Text =
                                txtIdentificador.Text = 
                                txtTEncendido.Text =
                                txtTApagado.Text = "";
                                Controles.InicializaDropDownList(ddlServicioGPS, "-- Seleccione un Servicio GPS");

                                //Obteniendo Servicios GPS por Unidad
                                using (DataTable dtServiciosGPS = SAT_CL.Monitoreo.ProveedorWSUnidad.ObtieneProveedorUnidad(unidad.id_unidad))
                                {
                                    //Validando Registros
                                    if (Validacion.ValidaOrigenDatos(dtServiciosGPS))
                                    {
                                        //Cargando GridView
                                        Controles.CargaGridView(gvAsignaciones, dtServiciosGPS, "Id", lblOrdenadoGPS.Text, true, 0);

                                        //Añadiendo a Sesión
                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosGPS, "Table");
                                    }
                                    else
                                    {
                                        //Inicializando GridView
                                        Controles.InicializaGridview(gvAsignaciones);

                                        //Eliminando de Sesión
                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                                    }
                                }

                                //Inicializando Controles
                                Controles.InicializaIndices(gvAsignaciones);
                            }
                        }
                        break;
                    }
            }
        }

        #endregion

        /// <summary>
        /// Método encaragdo de Guardar la lectura
        /// </summary>
        protected void wucLectura_ClickGuardarLectura(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Guardamos Lectura
            resultado = wucLectura.GuardarLectura();

            //Validamos Resultado
            if(resultado.OperacionExitosa)
            {
                
                //Método encargado de Cerrar la ventana de Lectura
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Lectura", "modalLectura", "Lectura");

                //Inicializamos Control de Historial de Lectura
                wucLecturaHistorial.InicializaControl(Convert.ToInt32(Session["id_registro"]), true);

                //Método encargado de Abrir la Ventana de Historial de Lectura
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");


            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de Eliminar una Lectura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucLectura_ClickEliminarLectura(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Guardamos Lectura
            resultado = wucLectura.DeshabilitarLectura();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Método encargado de Cerrar la ventana de Lectura
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Lectura", "modalLectura", "Lectura");

                //Inicializamos Control de Historial de Lectura
                wucLecturaHistorial.InicializaControl(Convert.ToInt32(Session["id_registro"]), true);

                //Método encargado de Abrir la Ventana de Historial de Lectura
                TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");

            }
            //Mostramos Error
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Método encargado de Consultar una Lectura
        /// </summary>
        protected void wucLecturaHistorial_lkbConsultar(object sender, EventArgs e)
        {
            //Inicializamos Control de Usuario Lectura
            wucLectura.InicializaControl(wucLecturaHistorial.id_lectura, Convert.ToInt32(Session["id_registro"]));
            //Método encargado de Abrir la ventana de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "Lectura", "modalLectura", "Lectura");
            //Método encargado de Cerrar la Ventana de Historial de Lectura
            TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "HistorialLectura", "modalLecturaHistorial", "LecturaHistorial");
        }
    }
}