using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAT_CL.Seguridad;
using TSDK.ASP;
using TSDK.Datos;
using SAT_CL;
using TSDK.Base;
using SAT_CL.Despacho;
using SAT_CL.Global;

namespace SAT.Despacho
{
    public partial class ServiciosPendientes : System.Web.UI.Page
    {

        #region Atributos

        /// <summary>
        /// Origen de datos con Servicios Pendientes
        /// </summary>
        private DataTable _mit;
        /// <summary>
        /// Origen de datos con Recursos Asignados al servicio
        /// </summary>
        private DataTable _mit2;

        #endregion

        #region Eventos

        /// <summary>
        /// Carga de la forma web
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (!Page.IsPostBack)
            {
                //Invocando  Método de Asignación
                asignaAtributos();
                //Inicializando Página
                inicializaForma();
            }
            else
                //Invocando  Método de Recuperación
                recuperaAtributos();
        }
        /// <summary>
        /// Evento desencadenado antes de Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {   
            //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Evento desencadenado al dar click en el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga
            cargaServicios();
        }
        /// <summary>
        /// Cambio de tamaño de página en GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Valida que el grid view contenga datos
            if (gvServicios.DataKeys.Count != 0)
            {
                //Si contiene datos realiza un ordenamiento de datos acorde al valor almacenado en lblOrdenandoServicio
                this._mit.DefaultView.Sort = lblOrdenadoServicios.Text;
                //Carga el grid view acorde al tamaño seleccionado del dropdownlist Tamaño servicio
                Controles.CambiaTamañoPaginaGridView(gvServicios, this._mit, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 4);
            }
            
        }
        /// <summary>
        /// Exportación de contenido de GV de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarServicios_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(this._mit, "*".ToArray());
        }
        /// <summary>
        /// Cambio de página activa edl GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Valida que existan valores en el grid view
            if (gvServicios.DataKeys.Count != 0)
            {
                //Si contiene datos realiza un ordenamiento acorde al valor almacenado en lblOrdenandoServicio
                this._mit.DefaultView.Sort = lblOrdenadoServicios.Text;
                //Carga el grid view con los valores ordenados
                Controles.CambiaIndicePaginaGridView(gvServicios, this._mit, e.NewPageIndex, true, 4);
            }
            
        }
        /// <summary>
        /// Cambio de Criterio de orden de GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Valida que existan datos en el gridview
            if (gvServicios.DataKeys.Count != 0)
            {
                //Si contiene datos realiza un ordenamiento acorde al valor almacenado en lblOrdenandoServicio
                this._mit.DefaultView.Sort = lblOrdenadoServicios.Text;
                //Asigna como valor al lblOrdenandoServicio el resultado de invoca al método encargado de realizar el ordenamiento de datos
                lblOrdenadoServicios.Text = Controles.CambiaSortExpressionGridView(gvServicios, this._mit, e.SortExpression, true, 4);
            }
        }
        /// <summary>
        /// Click en algún Link de GV de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionServicio_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                        
                    case "Documentacion":
                        //Inicializando control de documentación
                        wucServicioDocumentacion.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]), true, UserControls.wucServicioDocumentacion.VistaDocumentacion.Encabezado);
                        //Mostrando ventana modal correspondiente
                        alternaVentanaModal("documentacionServicio", lkb);
                        break;
                    case "Asignaciones":
                        //Determinando que ventana debe mostrarse
                        //Si el recurso no está asignado
                        if (lkb.Text == "Sin Asignar")
                        {
                            //Inicializando control de asignación de recurso
                            wucPreAsignacionRecurso.InicializaPreAsignacion(Convert.ToInt32(gvServicios.SelectedDataKey["movimiento"]));
                            //Mostrando ventana modal de asignaciones
                            alternaVentanaModal("asignacionRecursos", lkb);
                        }
                        //Si ya está asignado
                        else
                        {
                            //Inicializando ventana de recursos asignados al servicio
                            cargaRecursosAsignados();
                            //Mostrando ventana modal de asignaciones del servicio
                            alternaVentanaModal("unidadesAsignadas", lkb);
                        }
                        break;
                    case "Referencia":
                        //Inicializando control de referencias de servicio
                        wucReferenciaViaje.InicializaControl(Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]));
                        //Realizando apertura de referencias de servicio
                        alternaVentanaModal("referenciasServicio", lkb);
                        break;
                    case "CartaPorte":
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/ServiciosPendientes.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Porte", Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"])), "Carta Porte", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    case "Vencimiento":
                        {
                            //Validando que exista un Indicador
                            if (Convert.ToInt32(gvServicios.SelectedDataKey["Indicador"]) > 0)
                            {
                                //Inicializando Control de Historial de Movimientos
                                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Servicio, Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]));
                                //Realizando apertura de referencias de servicio
                                alternaVentanaModal("historialVencimientos", lkb);
                            }
                            else if (Convert.ToInt32(gvServicios.SelectedDataKey["Indicador"]) == 0)
                            {
                                //Inicializando Historial
                                wucVencimientosHistorial.InicializaControl(SAT_CL.Global.TipoVencimiento.TipoAplicacion.Servicio, Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]));
                                //Inicializando Control de Alta de Vencimientos
                                wucVencimiento.InicializaControl(1, Convert.ToInt32(gvServicios.SelectedDataKey["id_servicio"]));
                                //Realizando apertura de referencias de servicio
                                alternaVentanaModal("actualizacionVencimiento", lkb);
                            }

                            break;
                        }
                }
            }
        }

        #region Eventos Historial Vencimientos

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
                //Instanciando vencimiento almacenado
                using (Vencimiento v = new Vencimiento(resultado.IdRegistro))
                    //Se actualizará sobre la unidad involucrada en la inserción de vencimiento
                    wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Servicio, v.id_registro);

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
                //Instanciando vencimiento almacenado
                using (Vencimiento v = new Vencimiento(resultado.IdRegistro))
                    //Se actualizará sobre la unidad involucrada en la inserción de vencimiento
                    wucVencimientosHistorial.InicializaControl(TipoVencimiento.TipoAplicacion.Servicio, v.id_registro);

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
            wucVencimiento.InicializaControl(1, wucVencimientosHistorial.id_recurso);

            //Cerrar ventana de historial de vencimientos
            alternaVentanaModal("historialVencimientos", this);
            //Abrir ventana de actualización de vencimiento
            alternaVentanaModal("actualizacionVencimiento", this);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        private void inicializaForma()
        { 
            //Cargando catálogo de tamaño de página
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServicios, "", 26);
            
            //Inicializando Controles
            chkDocumentado.Checked =
            chkIniciado.Checked = 
            chkCitaCarga.Checked = true;
            chkCitaDescarga.Checked = false;

            //Inicializando Fechas
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Cargando el listado de servicios pendientes
            cargaServicios();
        }
        /// <summary>
        /// Realiza la búsqueda y muestra los servicios activos
        /// </summary>
        private void cargaServicios()
        {
            //Declarando Variables de Fecha
            DateTime cita_carga_ini, cita_carga_fin, cita_descarga_ini, cita_descarga_fin;
            cita_carga_ini = cita_carga_fin = cita_descarga_ini = cita_descarga_fin = DateTime.MinValue;

            //Validando Fechas Solicitadas
            if (chkCitaCarga.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIni.Text, out cita_carga_ini);
                DateTime.TryParse(txtFecFin.Text, out cita_carga_fin);
            }
            if (chkCitaDescarga.Checked)
            {
                //Obteniendo Fecha
                DateTime.TryParse(txtFecIni.Text, out cita_descarga_ini);
                DateTime.TryParse(txtFecFin.Text, out cita_descarga_fin);
            }

            //Cargando Unidades
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaPlaneacionServicios(((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1, "0")), "",
                                            chkDocumentado.Checked, chkIniciado.Checked, cita_carga_ini, cita_carga_fin, cita_descarga_ini,
                                            cita_descarga_fin, txtReferencia.Text, Cadena.RegresaCadenaSeparada(txtNoUnidadA.Text, "ID:", 1),Cadena.RegresaCadenaSeparada(txtNoOperador.Text, "ID:", 1)))
            {
                //Llenando gridview
                Controles.CargaGridView(gvServicios, mit, "id_servicio-movimiento-Indicador", lblOrdenadoServicios.Text, true, 4);
                //Guardando en sesión el origen de datos
                if (mit != null)
                    this._mit = mit;
                else
                    this._mit = null;
            }
        }
        /// <summary>
        /// Realiza la búsqueda de viajes activos y recarga el GV de los mismos manteniendo la selección del registro actual
        /// </summary>
        private void cargaServiciosManteniendoSeleccion()
        {
            //Obteniendo el registro seleccionado actualmente
            string id_registro_seleccion = gvServicios.SelectedIndex > -1 ? gvServicios.SelectedDataKey["id_servicio"].ToString() : "";
            //Cargando Gridview
            cargaServicios();
            //Restableciendo selección en caso de ser necesario
            if (id_registro_seleccion != "")
                Controles.MarcaFila(gvServicios, id_registro_seleccion, "id_servicio", "id_servicio-movimiento-id_parada_actual-IdParadaInicial", this._mit, lblOrdenadoServicios.Text, Convert.ToInt32(ddlTamanoServicios.SelectedValue), true, 4);
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos
        /// </summary>
        private void asignaAtributos()
        {   
            ViewState["mit"] = this._mit;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            if (ViewState["mit"] != null)
                this._mit = (DataTable)ViewState["mit"];
        }

        #endregion

        #region Ventanas Modales

        /// <summary>
        /// Click en botones de cierre de ventanas modales
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
                case "AsignacionRecursos":
                    //Cerrando ventana modal 
                    alternaVentanaModal("asignacionRecursos", lkbCerrar);
                    break;
                case "ReferenciasServicio":
                    //Cerrando modal de referencias de servicio
                    alternaVentanaModal("referenciasServicio", lkbCerrar);
                    break;
                case "Documentacion":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("documentacionServicio", lkbCerrar);
                    break;
                case "UnidadesAsignadas":
                    //Cerrando modal de actualización de alta de kilometraje
                    alternaVentanaModal("unidadesAsignadas", lkbCerrar);
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
                            cargaServicios();
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
                case "asignacionRecursos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "asignacionRecursosModal", "asignacionRecursos");
                    break;
                case "referenciasServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "referenciasServicioModal", "referenciasServicio");
                    break;
                case "documentacionServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "documentacionServicioModal", "documentacionServicio");
                    break;
                case "unidadesAsignadas":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "unidadesAsignadasModal", "unidadesAsignadas");
                    break;
                case "confirmacionQuitarRecursos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "confirmacionQuitarRecursosModal", "confirmacionQuitarRecursos");
                    break;
                case "historialVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "historialVencimientosModal", "historialVencimientos");
                    break;
                case "actualizacionVencimiento":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "actualizacionVencimientoModal", "actualizacionVencimiento");
                    break;
            }
        }

        #region Asignación Recursos

        /// <summary>
        /// Clic en botón Agregar Recuros a Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickAgregarRecurso(object sender, EventArgs e)
        {
            //Asignabndo recurso
            asignaRecursoMovimiento();
        }
        /// <summary>
        /// Clic en botón liberar recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionRecurso_ClickLiberarRecurso(object sender, EventArgs e)
        {
            //Realizando la liberación de los recursos
            RetornoOperacion resultado = wucPreAsignacionRecurso.LiberaRecurso();

            //Si se ha liberado el recurso correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de servicios y sus asignaciones de movimiento
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el proceso de adición de recurso al movimiento indicado
        /// </summary>
        private void asignaRecursoMovimiento()
        {
            //Realizando asignación de recursos
            RetornoOperacion resultado = wucPreAsignacionRecurso.AsignaRecursoViaje();

            //Si se agrega correctamente
            if (resultado.OperacionExitosa)
                //Actualizando lista de servicios y sus asignaciones de movimiento
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultados
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón quitar de la lista de recursos asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbQuitarRecursoAsignado_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que existan Recursos Asignados
            if (gvRecursosAsignados.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosAsignados, sender, "lnk", false);

                //Instanciamos Asignación
                using (MovimientoAsignacionRecurso objMovimientoAsignacionRecurso = new MovimientoAsignacionRecurso
                                                                                  (Convert.ToInt32(gvRecursosAsignados.SelectedDataKey.Value)))
                {
                    //Validamos Deshabilitación de Recursos
                    resultado = objMovimientoAsignacionRecurso.ValidaDeshabilitacionRecursos();
                }
                //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {

                    //Asignamos Mensaje a la ventana Modal
                    lblMensaje.Text = resultado.Mensaje;
                    //Ocultando ventana de recursos asignados
                    alternaVentanaModal("unidadesAsignadas", (LinkButton)sender);
                    //Mostrando ventana de confirmación para quitar recurso asociado
                    alternaVentanaModal("confirmacionQuitarRecursos", (LinkButton)sender);
                }
                else
                    //Deshabilitamos Recurso Asignado
                    deshabilitaRecurso();
            }
        }
        /// <summary>
        /// Deshabilita Recurso
        /// </summary>
        private void deshabilitaRecurso()
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando la asignación
            using (MovimientoAsignacionRecurso r = new MovimientoAsignacionRecurso(Convert.ToInt32(gvRecursosAsignados.SelectedValue)))
                //Realizando la deshabilitación
                resultado = r.DeshabilitaMovimientosAsignacionesRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Cargando lista de recursos asignados
                cargaRecursosAsignados();
                //Actualizamos la consulta de servicios pendientes
                cargaServiciosManteniendoSeleccion();
                //Actualizamos el grid
                upgvServicios.Update();
                //Si no hay registros asignados al servicio
                if (gvRecursosAsignados.DataKeys.Count == 0)
                    //Ocultando ventana de recursos asignados
                    alternaVentanaModal("unidadesAsignadas", this);
            }

            //Mostrando resultados
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en aceptar eliminar recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminacionRecurso_Click(object sender, EventArgs e)
        {
            //Ocultando ventana modal de confirmación
            alternaVentanaModal("confirmacionQuitarRecursos", (Button)sender);
            //Mostrando ventana de recursos asignados
            alternaVentanaModal("unidadesAsignadas", this);
            //Deshabilitamos Recurso Asignado
            deshabilitaRecurso();            
        }
        /// <summary>
        /// Click en cancelar eliminación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminacionRecurso_Click(object sender, EventArgs e)
        {
            //Ocultando ventana de confirmación
            alternaVentanaModal("confirmacionQuitarRecursos", (Button)sender);
            //Mostrando ventana de recursos
            alternaVentanaModal("unidadesAsignadas", (Button)sender);
        }
        /// <summary>
        /// Realiza la carga de los Recursos Asignados 
        /// </summary>
        private void cargaRecursosAsignados()
        {
            //Obteniendo los recursos asignados
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(Convert.ToInt32(gvServicios.SelectedDataKey["movimiento"])))
            {
                //Inicializamos Indices
                Controles.InicializaIndices(gvRecursosAsignados);
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosAsignados, dt, "Id-IdRecurso", "", false, 0);

                //Validando datattable  no sea null
                if (dt != null)
                    //Guardando tabla en origen de datos correspondiente
                    this._mit2 = dt;
                else
                    //Eliminamos Tabla 
                    this._mit2 = null;
            }
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
                //Actualizando Gridview
                cargaServiciosManteniendoSeleccion();

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
                //Actualizando Gridview
                cargaServiciosManteniendoSeleccion();

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Documentación del Servicio

        /// <summary>
        /// Agrega una parada al servicio que se está documentado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucServicioDocumentacion_ImbAgregarParada_Click(object sender, EventArgs e)
        {
            //Guardando parada
            RetornoOperacion resultado = wucServicioDocumentacion.GuardaParadaServicio();

            //Si se documentó correctamente
            if (resultado.OperacionExitosa)
                cargaServiciosManteniendoSeleccion();

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
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

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
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

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
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();

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
                //Se actualiza contenido de gridview de servicios
                cargaServiciosManteniendoSeleccion();
                //Cerrando ventana modal
                alternaVentanaModal("documentacionServicio", this);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion
        /// <summary>
        /// Enlace a datos de cada fila del GV de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    /**** APLICANDO COLOR DE FONDO DE FILA ACORDE A ESTATUS DE UNIDAD Y VENCIMIENTOS ****/
                    //Determinando estatus de unidad
                    switch (Convert.ToInt32(row["Indicador"]))
                    {
                        case 1:
                            //Cambiando color de fondo de la fila a rojo
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                            break;
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

        #endregion
    }
}