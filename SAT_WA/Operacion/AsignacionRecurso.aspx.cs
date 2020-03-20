using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using SAT_CL.Global;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class AsignacionRecurso : System.Web.UI.Page
    {
        #region Servicios

        #region Eventos 
        /// <summary>
        /// Evento generado al cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se produjo un PostBack
            if (!Page.IsPostBack)
            {
                //Inicializa Pagina
                inicializaPagina();
                //Botón Default
                this.Form.DefaultButton = btnBuscarServicios.UniqueID;
                //Establece el foco al control

            }            
        }

        /// <summary>
        /// Evento generado al dar Click en Buscar Servicios Sin Iniciar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarServicios_Click(object sender, EventArgs e)
        {
            //Carga Viajes Sin Iniciar
            cargaViajesSinIniciar();
            //Limpiamos Etiqueta Error
            lblError.Text = 
            lblErrorKm.Text = "";
            //Actualizamos los indicadores
            inicializaIndicadores();
        }

        /// <summary>
        /// Evento producido al seleccionar un Servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbServicios_Click(object sender, EventArgs e)
        {

            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvSevicios, sender, "lnk", false);
            //Carga Recursos Asignados al Movimiento
            cargaRecursosAsignados();
        }

        /// <summary>
        /// Evento generado al dar Click en Reubicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbUbicacion_OnClick(object sender, EventArgs e)
        {
            //Validamos datos del Grid View y Vista Unidad
            if (gvRecursosDisponibles.DataKeys.Count > 0 &&mtvRecursos.ActiveViewIndex ==0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Inicializamos Control Movimiento en vacio
                if (wucReubicacion.InicializaControl(Convert.ToInt32(gvRecursosDisponibles.SelectedValue), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                      Parada.TipoActualizacionLlegada.Manual, Parada.TipoActualizacionSalida.Manual, EstanciaUnidad.TipoActualizacionInicio.Manual, EstanciaUnidad.TipoActualizacionFin.Manual).OperacionExitosa)
                {
                    //Mostramos Ventana Modal de reubicación
                    alternaVentanaModal(upgvRecursosDisponibles, "ReubicacionUnidades");
                }
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
                    //Cerrando ventana modal de reubicación
                    alternaVentanaModal(this, "ReubicacionUnidades");

                    //Cargamos Recursos Disponibles
                    cargaRecursosDiponibles();

                    //Actualizando los indicadores
                    inicializaIndicadores();
                }
            }
            //Si existen vencimientos
            else
            {
                //Cerrando ventana de movimiento vacío (reubicación)
                alternaVentanaModal(this, "ReubicacionUnidades");
                //Mostrando ventana de notificación de vencimientos
                alternaVentanaModal(this, "IndicadorVencimientos");
            }
        }

             /// <summary>
        /// Eventó Generado al Cancelar la Reubicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReubicacion_OnClickCancelar(object sender, EventArgs e)
        {
            //Cerrando ventana de reubicación
            alternaVentanaModal(this, "ReubicacionUnidades");
        }

        /// <summary>
        /// Evento Producido  al enlazar los datos del Grid View Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Servicios
                if (gvSevicios.DataKeys.Count > 0)
                {
                    //Obteniendo referencia a control de interés
                    using (LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkRecursos"))
                    {
                        //Recuperando origen de datos
                        DataRow row = ((DataRowView)e.Row.DataItem).Row;
                        //Si hay que ocultar el botón
                        if (row["Recursos"].ToString() == "0")
                            lnkButton.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoSevicios_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvSevicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoSevicios.SelectedValue), true, 1);
            //Inicializamos Grid View de Recursos Asignados
            inicializaGridViewRecursosAsignados();
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
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvSevicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
            //Inicializamos Grid View de Recursos Asignados
            inicializaGridViewRecursosAsignados();
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Servicios"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarSevicios.Text = Controles.CambiaSortExpressionGridView(gvSevicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            //Inicializamos Grid View de Recursos Asignados
            inicializaGridViewRecursosAsignados();
        }
        /// <summary>
        /// Evento disparado al dar click en la ubicacion origen del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkOrigen_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvSevicios, sender, "lnk", false);
            //Instanciamos la ubicacion origen
            using (Ubicacion objUbicacion = new Ubicacion(Convert.ToInt32(gvSevicios.SelectedDataKey.Values["id_origen"])))
            {
                //Asignamos los datos de la ubicacion a las etiquetas respectivas
                //Nombre de la ubicacion
                lblNombre.Text = objUbicacion.descripcion;
                //Direccion de la ubicacion
                lblDireccionUbicacion.Text = objUbicacion.ObtieneDireccionCompleta();
                //Latitud longitud
                lblGeoubicacion.Text = objUbicacion.latitud.ToString() + " ," + objUbicacion.longitud.ToString();
                //Telefonos
                lblTelefono.Text = objUbicacion.telefono;

            }
            //Mostramos la modal con los datos de la ubicación origen
            alternaVentanaModal(upgvSevicios, "UbicacionOrigenDestino");
        }
        /// <summary>
        /// Evento disparado al dar click en la ubicacion destino del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDestino_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvSevicios, sender, "lnk", false);
            //Instanciamos la ubicacion origen
            using (Ubicacion objUbicacion = new Ubicacion(Convert.ToInt32(gvSevicios.SelectedDataKey.Values["id_destino"])))
            {
                //Asignamos los datos de la ubicacion a las etiquetas respectivas
                //Nombre de la ubicacion
                lblNombre.Text = objUbicacion.descripcion;
                //Direccion de la ubicacion
                lblDireccionUbicacion.Text = objUbicacion.ObtieneDireccionCompleta();
                //Latitud longitud
                lblGeoubicacion.Text = objUbicacion.latitud.ToString() + " ," + objUbicacion.longitud.ToString();
                //Telefonos
                lblTelefono.Text = objUbicacion.telefono;
            }
            //Mostramos la modal con los datos de la ubicación destino      
            alternaVentanaModal(upgvSevicios, "UbicacionOrigenDestino");
        }
        /// <summary>
        /// Evento disparado al dar click en el Kilometraje del servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkKilometraje_Click(object sender, EventArgs e)
        {
            //Validando que haya registros
            if(gvSevicios.DataKeys.Count > 0)
            {
                //Declaramos Mensaje
                RetornoOperacion resultado = new RetornoOperacion();
                
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvSevicios, sender, "lnk", false);

                //Instanciamos Servicio
                using (Servicio objServicio = new Servicio(Convert.ToInt32(gvSevicios.SelectedDataKey["Id"])))
                {
                    //Validando que Existe el Registro
                    if (objServicio.id_servicio > 0)

                        //Terminamos Servicio
                        resultado = objServicio.CalculaKilometrajeServicio(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se puede acceder al Registro");
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                
                    //Cargamos Servicios
                    cargaViajesSinIniciar();
                
                //Mostramos Resultado
                lblErrorKm.Text = resultado.Mensaje;
            }
        }
        /// <summary>
        /// Evento disparado al dar click al link button cerrar cuya funcion es la de cerrar la ventana modal que muestra los datos de la ubicacion deseada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Cerrando ventana de ubicación origen o destino
            alternaVentanaModal(uplnkCerrar, "UbicacionOrigenDestino");
        }
        /// <summary>
        /// evento disparado al dar click en el link (icono) de recursos asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkRecursos_Click(object sender, EventArgs e)
        {
            if (gvSevicios.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvSevicios, sender, "lnk", false);
                //Carga Recursos Asignados al Movimiento
                cargaRecursosAsignados();
                //Mostramos la modal con los datos deseados  
                alternaVentanaModal(upgvSevicios, "RecursosAsignados");
            }
        }
        /// <summary>
        /// Evento disparado al dar click en el boton cerrar de la ventana modal recursos asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarUnidadesAsignadas_Click(object sender, EventArgs e)
        {
            //Ocultando ventana modal de asignaciones
            alternaVentanaModal(uplnkCerrarUnidadesAsignadas, "RecursosAsignados");
        }
        //Evento disparado al dar click en el linkbutton de la ventana modal de servicios asignados a unidad
        protected void lnkCerrarServiciosAsignados_Click(object sender, EventArgs e)
        {
            //Cerrando modal de servicios asignados a la unidad
            alternaVentanaModal(uplnkCerrarServiciosAsignados, "ServiciosAsignadosUnidad");
        }

        #endregion

        #region Métodos 
        /// <summary>
        /// Inicializa Pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializamos Valores
            inicializaGridView();
            //Cargamos Catalogo
            cargaCatalogos();
            //Inicializamos los indicadores 
            inicializaIndicadores();
        }

        /// <summary>
        /// Inicializa Valores Grid View
        /// </summary>
        private void inicializaGridView()
        {
            //Inicializamos Grid View
            Controles.InicializaGridview(gvRecursosDisponibles);
            Controles.InicializaGridview(gvSevicios);
            Controles.InicializaGridview(gvRecursosAsignados);
        }

        /// <summary>
        ///Carga Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //ddlTamanoSevicios
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoSevicios, "", 56);
            //Unidades
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRecursosDisponibles, "", 18);
            //ddlTamanoRecursosDisponibles
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlRecursosAsignados, "", 18);
            //Estatus Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusUnidad, "Todos", 53);
            //Tipo Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "");
            //Estatus Operador
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusOperador, "Todos", 57);
        }

        /// <summary>
        /// Método encargado de Inicializar todos los grid view
        /// </summary>
        private void inicializaGridViewRecursos()
        {
            //Inicializamos Grid View Recursos Asignados
            Controles.InicializaGridview(gvRecursosAsignados);
            //Inicializamos Grid View Recursos Disponibles
            Controles.InicializaGridview(gvRecursosDisponibles);
            //Borrando de sesión los Recursos Disponibles cargados anteriormente
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }

        /// <summary>
        /// Metodo encargado de inicializar los indicadores de la forma
        /// </summary>
        private void inicializaIndicadores()
        {
            //Recuperamos los indicadores de servicio
            using (DataTable t = Servicio.CargaIndicadoresServicios(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                if (Validacion.ValidaOrigenDatos(t))
                {
                    foreach (DataRow r in t.Rows)
                    {
                        lblDocumentados.Text = r["PendientesDespacho"].ToString();
                        lblCitaTarde.Text = r["CitasPerdidas"].ToString();
                    }
                }
            }
            //Recuperamos los indicadores de UNIDADES
            using (DataTable t = SAT_CL.Global.Unidad.CargaIndicadoresUnidades(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                if (Validacion.ValidaOrigenDatos(t))
                {
                    foreach (DataRow r in t.Rows)
                    {
                        lblTractores.Text = r["TractorDisponible"].ToString();
                        lblCajas.Text = r["RemolqueDisponible"].ToString();
                        lblDisponibilidad.Text = r["TiempoDisponibilidad"].ToString();                        
                        
                    }
                }
            }
        }

        /// <summary>
        /// Realzia la carga de los viajes sin Iniciar 
        /// </summary>
        private void cargaViajesSinIniciar()
        {
            //Actualizamos Grid View
            inicializaGridViewRecursos();
            //Obteniendo detalles de viaje
            using (DataTable dt = Reportes.CargaServiciosSinIniciar(Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCliente.Text, "0"), ':', 1)),
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCiudadOrigen.Text, "0"), ':', 1)),
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(Cadena.VerificaCadenaVacia(txtCiudadDestino.Text, "0"), ':', 1)),
                                                                    txtFecha.Text, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Inicializando gridView
                Controles.InicializaIndices(gvSevicios);
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvSevicios, dt, "Id-id_origen-id_destino-Recursos", lblOrdenarSevicios.Text, true, 1);

                //Validando que la Tabla Contenga Registros
                if (Validacion.ValidaOrigenDatos(dt))
                {
                   //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table");
                }
                else
                {
                    //Eliminamos Tabla DataSet en Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Carga Anticipos pendientes (Vales de Diesel y Depósitos)
        /// </summary>
        /// <param name="id_operador">Id Operador</param>
        /// <param name="id_unidad">Id Unidad</param>
        /// <param name="id_proveedor_compania">Id Proveedor Compania</param>
        private void cargaAnticiposPendientes(int id_operador, int id_unidad, int id_proveedor_compania)
        {
            //Obteniendo detalles de viaje
            using (DataTable dt = SAT_CL.EgresoServicio.Reportes.CargaAnticiposPendientes(id_operador, id_unidad, id_proveedor_compania))
            {
                //Inicializando indices gridView
                Controles.InicializaIndices(gvServiciosAsignados);
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvAnticiposPendientes, dt, "", "", false, 0);
                //Validando que el datatable no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table5");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                }
            }
        }

        #endregion
        #endregion

        #region Recursos Disponibles

        #region Eventos

        /*// <summary>
        /// Evento generado al cambiar chkPropio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPropio_OnCheckedChanged(object sender, EventArgs e)
        {
            //Validamos si ésta marcado Propio
            if (chkPropio.Checked)
            {
                //Limpiamos control Propietario
                txtPropietarioUnidad.Text = "";
                //Deshabilitamos control
                txtPropietarioUnidad.Enabled = false;
            }
            else
            {
                //Habilitamos control
                txtPropietarioUnidad.Enabled = true;
            }
        }//*/

        /// <summary>
        /// Eventó Generado al Cerrar Anticipo Pendientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAnticiposP_Click(object sender, EventArgs e)
        {
            //Cerrando ventana modal de anticipos
            alternaVentanaModal(lkbCerrarAnticiposP, "AnticiposPendientes");
        }
        /// <summary>
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            Controles.InicializaGridview(gvRecursosDisponibles);
            //Limpiamos Etiqueta Ordenear
            lblOrdenarRecursosDisponibles.Text = "";
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Unidad":
                    mtvRecursos.ActiveViewIndex = 0;
                    btnUnidad.CssClass = "boton_pestana_activo";
                    btnOperador.CssClass = "boton_pestana";
                    btnTercero.CssClass = "boton_pestana";
                    break;
                case "Operador":
                    mtvRecursos.ActiveViewIndex = 1;
                    btnUnidad.CssClass = "boton_pestana";
                    btnOperador.CssClass = "boton_pestana_activo";
                    btnTercero.CssClass = "boton_pestana";
                    break;
                case "Tercero":
                    mtvRecursos.ActiveViewIndex = 2;
                    btnTercero.CssClass = "boton_pestana_activo";
                    btnOperador.CssClass = "boton_pestana";
                    btnUnidad.CssClass = "boton_pestana";
                    break;
            }
        }

        /// <summary>
        /// Evento generado al dar Click en Buscar Recursos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {            
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Unidades":
                    //Carga Unidades
                    cargaUnidades(); ;
                    break;
                case "Operadores":
                    //Cargamos Operadores
                    cargaOperadores();
                    break;
                case "Tercero":
                    //Carga Terceros
                    cargaTerceros();
                    break;
            }
            //Limpiamos Etiqueta Error
            lblError.Text = 
            lblErrorKm.Text = "";

            //Inicializamos los indicadores
            inicializaIndicadores();
        }

        /// <summary>
        /// Metodo encargado de buscar los recursos disponibles de acuerdo al tipo 
        /// </summary>
        /// <param name="tipo"></param>
        private void buscaRecursosDisponibles()
        {
            //Determinando el tipo de acción a realizar
            switch (mtvRecursos.ActiveViewIndex)
            {
                case 0:
                    //Carga Unidades
                    cargaUnidades(); ;
                    break;
                case 1:
                    //Cargamos Operadores
                    cargaOperadores();
                    break;
                case 2:
                    //Carga Terceros
                    cargaTerceros();
                    break;
            }           
        }

        /// <summary>
        /// Evento generado al cambier el tipo de Unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Inicializamos Grid View
            Controles.InicializaGridview(gvRecursosDisponibles);
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRecursosDisponibles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView 
            Controles.CambiaTamañoPaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"),
                                        Convert.ToInt32(ddlTamanoRecursosDisponibles.SelectedValue), true, 5);
        }

        /// <summary>
        /// E
        /// vento Producido al pulsa
        /// r el botón "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarRecursosDisponibles_OnClick(object sender, EventArgs e)
        {
            //Exportando Recursos Disponibles
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"));
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, false, 1);
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarRecursosDisponibles.Text = Controles.CambiaSortExpressionGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, false, 1);
        }

        /// <summary>
        /// Evento Producido  al enlazar los datos del Grid View "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si el tipo de la fila es datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Si existen recursos mostrados
                if (gvRecursosDisponibles.DataKeys.Count > 0)
                {
                    //link Hiperlink ubicación
                    insertaCeldaUltimaUbicacion(e.Row);
                    //Metodo encargado de mostrar u ocultar los iconos de indicador
                    visualizaIndicador(e.Row);
                }
            }           
        }
        /// <summary>
        /// Evento producido al pulsar el link Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbServiciosRecursoDisponible_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros en gridview
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos Tipo de Asignación
                MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;

                //De acuerdo a la vista Activa
                switch (mtvRecursos.GetActiveView().ID)
                {
                    case "vwUnidad":
                        tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                        break;
                    case "vwOperador":
                        tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                        break;
                    case "vwTercero":
                        tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                        break;
                }
                //Cargamos Servicios de acuerdo a la vista Activa
                cargaServiciosAsignadosAlRecurso(tipo);
                //Colocamos la descripcion correcta del titulo
                lblServicios.Text = "Servicios Asignados";
                uplblServicios.Update();
                //Mostrando servicios asignados al recurso
                alternaVentanaModal(upgvServiciosAsignados, "ServiciosAsignadosUnidad");
            }
        }

        /// <summary>
        /// Evento disparado al dar click en el link Liquidar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkLiquidar_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros en gridview
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos Tipo de Asignación
                MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;

                //De acuerdo a la vista Activa
                switch (mtvRecursos.GetActiveView().ID)
                {
                    case "vwUnidad":
                        tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                        break;
                    case "vwOperador":
                        tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                        break;
                    case "vwTercero":
                        tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                        break;
                }
                //Cargamos Servicios de acuerdo a la vista Activa
                cargaServiciosTerminadosAsignadosAlRecurso(tipo);

                //Colocamos la descripcion correcta del titulo
                lblServicios.Text = "Servicios Terminados por Liquidar";
                uplblServicios.Update();

                //Mostrando servicios asignados al recurso
                alternaVentanaModal(upgvServiciosAsignados, "ServiciosAsignadosUnidad");
            }
        }

        /// <summary>
        /// Evento producido al Agregar un Recurso Disponible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAgregar_Click(object sender, EventArgs e)
        {
            //Declaramos variable Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Seleccion el Servicio
            if (gvSevicios.SelectedIndex != -1 && gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables para almacenar el tipo de recurso
                MovimientoAsignacionRecurso.Tipo tipo;
                int tipo_unidad;

                //Obtenemos Tipo de recurso  para su asignación
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Validamos Asignación ligadas al Recurso 
                resultado = MovimientoAsignacionRecurso.ValidaAsignacionesLigadasAlRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                    //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {

                    //Asignamos Mensaje
                    lblMensajeAsignacion.Text = resultado.Mensaje;

                    //Mostrando ventana de confirmación de recurso asociado
                    alternaVentanaModal(upgvRecursosAsignados, "ConfirmacionAsignacionRecurso");
                }
                else
                {



                    //Guardamos Movimiento Asignación Recurso
                    asignaRecurso(tipo, tipo_unidad);
                    //Actualizamos la consulta de viajes sin iniciar
                    cargaViajesSinIniciar();
                    //Actualizamos el grid
                    upgvSevicios.Update();
                    //Actualizamos la consulta de unidades disponibles
                    buscaRecursosDisponibles();                  



                }
            }
            else
            {
                //Mostramos Mensaje
                lblError.Text = "Seleccione el Servicio";
            }

        }
       

        /// <summary>
        /// Evento producido al pulsar el link Pendientes Liq
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPendientesLiq_Click(object sender, EventArgs e)
        {
            //Validando existencia de registros en gridview
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables
                int id_operador = 0;
                int id_unidad = 0;
                int id_tercero = 0;

                //De acuerdo a la vista Activa
                switch (mtvRecursos.GetActiveView().ID)
                {
                    case "vwUnidad":
                        id_unidad = (Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                        break;
                    case "vwOperador":
                        id_operador = (Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                        break;
                    case "vwTercero":
                        id_tercero = (Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                        break;
                }
                //Cargamos Anticipos Pendientes
                cargaAnticiposPendientes(id_operador, id_unidad, id_tercero);

                //Monstrando ventana de anticipos pendientes
                alternaVentanaModal(upgvRecursosDisponibles, "AnticiposPendientes");
            }
        }
        /// <summary>
        /// Evento Produciado Al Aceptar Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAsignacion_OnClick(object sender, EventArgs e)
        {
            //Declaramos Variable
            MovimientoAsignacionRecurso.Tipo tipo = MovimientoAsignacionRecurso.Tipo.Operador;
            int tipo_unidad = 0;

            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //Validando el tipo de vencimiento
            RetornoOperacion resultado = validaVencimientosActivos();

            //Cerrando ventana de confirmación de recurso asociado
            alternaVentanaModal(upbtnAceptarAsignacion, "ConfirmacionAsignacionRecurso");

            //Si no hay vencimientos
            if (resultado.OperacionExitosa)
            {
                //Asignación Recurso Disponible
                asignaRecurso(tipo, tipo_unidad);
                //Recargamos la consulta de servicios disponible
                cargaViajesSinIniciar();
                //Actualizamos el grid
                upgvSevicios.Update();                
            }
            //Si hay vencimientos activos
            else
                //Mostrar ventana informativa de vencimientos
                alternaVentanaModal(upbtnAceptarAsignacion, "IndicadorVencimientos");
        }

   
        /// Evento producido al Cancelar la Asignación de Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarAsignacion_OnClick(object sender, EventArgs e)
        {   
            //Cerrando ventana de confirmación de resurso asociado
            alternaVentanaModal(upbtnCancelarAsignacion, "ConfirmacionAsignacionRecurso");
        }

        /// <summary>
        /// Método encarga de obtener el tipo de recurso (Tractor, Remolque, Operador)
        /// </summary>
        /// <param name="tipo"> Tipo recurso (Tractor, Remolque, Operador)</param>
        /// <param name="tipo_unidad">Tipo Tractor (Remolque, Dolly,Tractor)</param>
        private void obtieneTipoRecurso(out MovimientoAsignacionRecurso.Tipo tipo, out int tipo_unidad)
        {
            //TODO: Sólo obtiene el tipo de Unidad (Remolque, Tractor) para Asignaciones de Unidades.
            tipo = MovimientoAsignacionRecurso.Tipo.Operador;
            tipo_unidad = 0;
            //Obtenemos Vista Activa para asignación de Tipo de Recurso
            switch (mtvRecursos.ActiveViewIndex)
            {
                case 0:
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Unidad;
                        //Instanciamos Unidad
                        using (SAT_CL.Global.Unidad objUnidad = new SAT_CL.Global.Unidad(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                        {
                            //Establecemos valor del Tipo de Unidad
                            tipo_unidad = objUnidad.id_tipo_unidad;
                        }
                        break;
                    }
                case 1:
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                        break;
                    }
                case 2:
                    {
                        tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                        break;
                    }
            }
        }
        #endregion 

        #region Métodos

        /// <summary>
        /// Carga Servicios asignados al Recurso
        /// </summary>
        /// <param name="tipo">Tipo Asignación (Unidad, Operador, Tercero)</param>
        private void cargaServiciosAsignadosAlRecurso(MovimientoAsignacionRecurso.Tipo tipo)
        {
            //Obteniendo detalles de viaje
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaServiciosAsignadosAlRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
            {
                //Inicializando indices gridView
                Controles.InicializaIndices(gvServiciosAsignados);
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvServiciosAsignados, dt, "Servicio", "", false, 5);
                //Validando que el datatable no sea null
                if(dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table4");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                }
            }
        }
        /// <summary>
        /// Carga Servicios terminados asignados al Recurso
        /// </summary>
        /// <param name="tipo">Tipo Asignación (Unidad, Operador, Tercero)</param>
        private void cargaServiciosTerminadosAsignadosAlRecurso(MovimientoAsignacionRecurso.Tipo tipo)
        {
            //Obteniendo detalles de viaje
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaServiciosTerminadosAsignadosAlRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
            {
                //Inicializando indices gridView
                Controles.InicializaIndices(gvServiciosAsignados);
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvServiciosAsignados, dt, "Servicio", "", false, 5);
                //Validando que el datatable no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table4");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                }
            }
        }

        /// <summary>
        /// Carga Recursos Disponibles
        /// </summary>
        private void cargaRecursosDiponibles()
        {
            //De acuerdo a la vista Activa, cargamos Recursos Disponibles
            switch (mtvRecursos.GetActiveView().ID)
            {
                case "vwUnidad":
                    cargaUnidades();
                    break;
                case "vwOperador":
                    cargaOperadores();
                    break;
                case "vwTercero":
                    cargaTerceros();
                    break;
            }
        }

        /// <summary>
        /// Método encargado de asignar los recursos
        /// </summary>
        /// <param name="tipo">Tipo Asignación (Unidad, Operador, Tercero)</param>
        /// <param name="tipo_unidad">Tipo Unidad (Tractor, Remolque, Dolly)</param>
        private void asignaRecurso(MovimientoAsignacionRecurso.Tipo tipo, int tipo_unidad)
        {
            //Definiendo objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Realizando la asignación del Recuros 
            resultado = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                       Convert.ToInt32(gvSevicios.SelectedValue), tipo, tipo_unidad, Convert.ToInt32(gvRecursosDisponibles.SelectedValue),
                       ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {
                //Cargando Viajes
                cargaViajesSinIniciar();
                
                //Cargamos Recursos Asignados
                cargaRecursosAsignados();

                //Cargamos Recursos Disponibles
                cargaRecursosDiponibles();

                //Actualizando indicadores
                inicializaIndicadores();
            }

            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;
            Controles.InicializaIndices(gvRecursosDisponibles);
            Controles.InicializaIndices(gvRecursosAsignados);

            //Actualizando Controles
            uplblError.Update();
            upgvRecursosDisponibles.Update();
            upgvRecursosAsignados.Update();
            uplblDocumentados.Update();
            uplblCitaTarde.Update();
            upplblTractores.Update();
            uplblCajas.Update();
            upplblDisponibilidad.Update();
        }

        /// <summary>
        /// Método Privado encargado de Insertar el link Asignaciones en las filas del Grid View Rcursos Disponibles
        /// </summary>
        /// <param name="fila"></param>
        private void visualizaIndicador(GridViewRow fila)
        {
            //Seleccionamos la fila actual
            gvRecursosDisponibles.SelectedIndex = fila.RowIndex;

            //Validamos si se debe de mostrar o no el icono de asignaciones pendientes
            if (gvRecursosDisponibles.SelectedDataKey["Asignaciones"].ToString() == "0")
            {
                //Buscamos HiperLinks
                using (LinkButton lnkAsignaciones = (LinkButton)fila.FindControl("lnkAsignaciones"))
                {
                    //No mostramos el linkButton
                    lnkAsignaciones.Visible = false;
                }
            }
            //Validamos si se debe de mostrar o no el icono de asignaciones pendientes
            if (gvRecursosDisponibles.SelectedDataKey["PorLiquidar"].ToString() == "0")
            {
                //Buscamos HiperLinks
                using (LinkButton lnkLiquidar = (LinkButton)fila.FindControl("lnkLiquidar"))
                {
                    //No mostramos el linkButton
                    lnkLiquidar.Visible = false;
                }
            }
            //Validamos si se debe de mostrar o no el icono de asignaciones pendientes
            if (gvRecursosDisponibles.SelectedDataKey["PendientesLiq"].ToString() == "0")
            {
                //Buscamos HiperLinks
                using (LinkButton lnkDeposito = (LinkButton)fila.FindControl("lnkDeposito"))
                {
                    //No mostramos el linkButton
                    lnkDeposito.Visible = false;
                }
            }
            //Validamos si se debe de mostrar o no el icono de asignaciones pendientes
            if (mtvRecursos.ActiveViewIndex == 2)
            {
                //Buscamos HiperLinks
                using (LinkButton lnkLiberar = (LinkButton)fila.FindControl("lkbLiberar"))
                {
                    //No mostramos el linkButton
                    lnkLiberar.Visible = false;
                }
            }
        }

        
        #endregion
        #endregion

        #region Recursos Asignados
        #region Eventos

        /// <summary>
        /// Inicializamos Grid View Recursos Asignados
        /// </summary>
        private void inicializaGridViewRecursosAsignados()
        {
            //Inicializando gridView
            Controles.InicializaGridview(gvRecursosAsignados);
            //Borrando de sesión los viajes cargados anteriormente
            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
        }

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Recursos Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRecursosAsignados_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvRecursosAsignados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"),
                                        Convert.ToInt32(ddlRecursosAsignados.SelectedValue), true, 5);
        }

        /// <summary>
        /// Evento Producido al pulsar el link "Recursos Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarRecursosAsignados_OnClick(object sender, EventArgs e)
        {
            //Exportando Recursos Asignados
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Recursos Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvRecursosAsignados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, false, 1);
        }

        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Recursos Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosAsignados_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarRecursosAsignados.Text = Controles.CambiaSortExpressionGridView(gvRecursosAsignados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, false, 1);
        }

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Servicios Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvServiciosAsignados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex, false, 1);
        }

        /// <summary>
        /// Evento Producido  al enlazar los datos del Grid View Servicios Asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosAsignados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvSevicios.DataKeys.Count > 0)
                {
                    //Buscamos HiperLinks
                    using (HyperLink lkbMapa = (HyperLink)e.Row.FindControl("lkbMapa"))
                    {
                        using (Label lblOrigen = (Label)e.Row.FindControl("lblOrigen"), lblDestino = (Label)e.Row.FindControl("lblDestino"))
                        {
                            lkbMapa.NavigateUrl = "~/Maps/UbicacionMapaCargaDescarga.aspx?id_ubicacion_carga=" + Cadena.RegresaCadenaSeparada(lblOrigen.ToolTip.ToString(), "ID:", 1) + "&id_ubicacion_descarga=" + Cadena.RegresaCadenaSeparada(lblDestino.ToolTip.ToString(), "ID:", 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento producido al Quitar un Recurso Asignado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbQuitar_Click(object sender, EventArgs e)
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
                    alternaVentanaModal(upgvRecursosAsignados, "RecursosAsignados");
                    //Mostrando ventana de confirmación para quitar recurso asociado
                    alternaVentanaModal(upgvRecursosAsignados, "ConfirmacionQuitarRecurso");
                }
                else
                {
                    //Deshabilitamos Recurso Asignado
                    deshabilitaRecurso();
                    //Actualizamos la consulta de servicios pendientes
                    cargaViajesSinIniciar();
                    cargaRecursosAsignados();
                    //Actualizamos el grid
                    upgvSevicios.Update();
                    upgvRecursosAsignados.Update();
                    //Ocultando modal de recursos asignados   
                    alternaVentanaModal(upgvRecursosAsignados, "RecursosAsignados");
                }
            }
        }

        /// <summary>
        /// Evento producido al Eliminar un Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminacionRecurso_OnClick(object sender, EventArgs e)
        {
            //Deshabilitamos Recurso Asignado
            deshabilitaRecurso();
            //Actualizamos la consulta de servicios pendientes
            cargaViajesSinIniciar();
            //Actualizamos el grid
            upgvSevicios.Update();
            //Ocultando Ventana Modal de confirmación de eliminación de recursos asociados
            alternaVentanaModal(upbtnAceptarEliminacionRecurso, "ConfirmacionQuitarRecurso");
        }

        #endregion
        #region Métodos

        /// <summary>
        /// Realiza la carga de los Recursos Asignados 
        /// </summary>
        private void cargaRecursosAsignados()
        {
            //Obteniendo los recursos asignados
            using (DataTable dt = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaMovimientosAsignacionParaVisualizacion(Convert.ToInt32(gvSevicios.SelectedValue)))
            {
                //Inicializamos Indices
                Controles.InicializaIndices(gvRecursosAsignados);
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosAsignados, dt, "Id-IdRecurso", lblOrdenarRecursosAsignados.Text, false, 0);
       
                //Validando datattable  no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table2");
                }
                else
                {
                    //Eliminamos Tabla del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
            }
        }
        
        /// <summary>
        /// Realiza la carga de las Unidades 
        /// </summary>
        private void cargaUnidades()
        {
            //Obteniendo las Unidades Disponibles
            using (DataTable dt = Unidad.CargaUnidadesParaAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoUnidad.Text, Convert.ToByte(ddlTipoUnidad.SelectedValue),
                                  Convert.ToByte(ddlEstatusUnidad.SelectedValue), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtPropietarioUnidad.Text, ':', 1), "0")), false,
                                  Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUbicacionUnidad.Text, ':', 1), "0"))))
            {
                //Cargando GridView de Recursos Disponibles
                Controles.CargaGridView(gvRecursosDisponibles, dt, "Id-id_ubicacion-Asignaciones-PorLiquidar-PendientesLiq", lblOrdenarRecursosDisponibles.Text, false, 5);

                //Validando que la Tabla  no sea null
                if (dt != null)
                {
                   //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");
                }
                else
                {
                    //Eliminamos Tabla del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Realiza la carga de los Operadores 
        /// </summary>
        private void cargaOperadores()
        {
            using (DataTable dt = SAT_CL.Global.Operador.CargaOperadoresParaAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtNombreOperador.Text, ':', 1), "0")),
                                Convert.ToByte(ddlEstatusOperador.SelectedValue)))
            {
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosDisponibles, dt, "Id-id_ubicacion-Asignaciones-PorLiquidar-PendientesLiq", lblOrdenarRecursosDisponibles.Text, false, 5);
                //Validando que la Tabla no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");
                    
                }
                else
                {
                    //Eliminamos Tabla  del DataSet Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Realiza la carga de los Terceros 
        /// </summary>
        private void cargaTerceros()
        {
            //Obteniendo los Terceros
            using (DataTable dt = SAT_CL.Global.CompaniaEmisorReceptor.CargaTercerosParaAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                        txtNombreTercero.Text))
            {
                //Cargando GridView 
                Controles.CargaGridView(gvRecursosDisponibles, dt, "Id-id_ubicacion-Asignaciones-PorLiquidar-PendientesLiq", lblOrdenarRecursosDisponibles.Text, false, 5);
                
                //Validando que la Tabla no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dt, "Table1");
                }
                else
                {
                    //Eliminamos Tabla DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }
        }

        /// <summary>
        /// Método Privado encargado de Insertar el link Ultima Ubicacion
        /// </summary>
        /// <param name="fila"></param>
        private void insertaCeldaUltimaUbicacion(GridViewRow fila)
        {
            //Buscamos HiperLinks
            using (HyperLink hlnkUltimaUbicacion = (HyperLink)fila.FindControl("hlnkUltimaUbicacion"))
            {
                //Marcamos como seleccionada la fila actual
                gvRecursosDisponibles.SelectedIndex = fila.RowIndex;
                hlnkUltimaUbicacion.NavigateUrl = "~/Maps/UbicacionMapa.aspx?id_ubicacion=" + gvRecursosDisponibles.SelectedDataKey["id_ubicacion"];
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
            {
                //Realizando la deshabilitación
                resultado = r.DeshabilitaMovimientosAsignacionesRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_usuario);
            }

            //Si no existe ningún error
            if (resultado.OperacionExitosa)
            {               
                //Cargamos Recursos Asignados
                cargaRecursosAsignados();

                //Cargamos Recursos Disponibles
                cargaRecursosDiponibles();

                //Actualizando indicadores
                inicializaIndicadores();
            }            
            
            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;
        }
        #endregion
               
        #endregion

        #region Vencimientos
        
        /// <summary>
        /// Determina si el recurso tiene vencimientos activos y notifica al usuario de ello
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion validaVencimientosActivos()
        {
            //Declarando objeto de retorno sin errores
            RetornoOperacion resultado = new RetornoOperacion(0);

            //Determinando el tipo de aplicación de vencimientos a buscar
            TipoVencimiento.TipoAplicacion tipo_aplicacion = TipoVencimiento.TipoAplicacion.Unidad;
            switch (mtvRecursos.ActiveViewIndex)
            {
                case 0:
                    tipo_aplicacion = TipoVencimiento.TipoAplicacion.Unidad;
                    break;
                case 1:
                    tipo_aplicacion = TipoVencimiento.TipoAplicacion.Operador;
                    break;
                case 2:
                    tipo_aplicacion = TipoVencimiento.TipoAplicacion.Transportista;
                    break;
            }

            //Declarando tabla concentradora de vencimientos
            DataTable mitVencimientosAsociados = null;
            //Si es unidad u operador, se obtienen vencimientos de recurso asociado
            switch (tipo_aplicacion)
            {
                case TipoVencimiento.TipoAplicacion.Unidad:
                    //Obteniendo al operador asociado
                    int id_operador = AsignacionOperadorUnidad.ObtieneOperadorAsignadoAUnidad(Convert.ToInt32(gvRecursosDisponibles.SelectedDataKey.Value));
                    //Si existe un operador
                    if (id_operador > 0)
                        //Obteniendo vencimientos
                        using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Operador, id_operador, Fecha.ObtieneFechaEstandarMexicoCentro()))
                        {
                            //Si existen vencimientos
                            if (mit != null)
                            {
                                mitVencimientosAsociados = new DataTable();
                                //Copiando a tabla concentradora
                                mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
                            }
                        }
                    break;
                case TipoVencimiento.TipoAplicacion.Operador:
                    //Obteniendo la unidad asociada
                    int id_unidad = AsignacionOperadorUnidad.ObtieneUnidadAsignadoAOperador(Convert.ToInt32(gvRecursosDisponibles.SelectedDataKey.Value));
                    //Si existe una unidad
                    if (id_unidad > 0)
                        //Obteniendo vencimientos
                        using (DataTable mit = Vencimiento.CargaVencimientosActivosRecurso(TipoVencimiento.TipoAplicacion.Unidad, id_unidad, Fecha.ObtieneFechaEstandarMexicoCentro()))
                        {
                            //Si existen vencimientos
                            if (mit != null)
                            {
                                mitVencimientosAsociados = new DataTable();
                                //Copiando a tabla concentradora
                                mitVencimientosAsociados.Merge(mit, true, MissingSchemaAction.Add);
                            }
                        }
                    break;
            }

            //Validando existencia de vencimientos del recurso
            using (DataTable mitVencimientos = Vencimiento.CargaVencimientosActivosRecurso(tipo_aplicacion,
                                                Convert.ToInt32(gvRecursosDisponibles.SelectedDataKey.Value), Fecha.ObtieneFechaEstandarMexicoCentro()))
            {
                //Si hay vencimientos del recurso principal que mostrar
                if (mitVencimientos != null)
                {
                    //SI no hubo vencimientos de recurso asociado
                    if (mitVencimientosAsociados == null)
                        mitVencimientosAsociados = new DataTable();

                    //Añadiendo contenido a tabla concentradora
                    mitVencimientosAsociados.Merge(mitVencimientos, true, MissingSchemaAction.Add);
                }

                //Si hay recursos en el concentrado
                if (mitVencimientosAsociados != null)
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
                TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientosAsociados, "Id", "", true, 1);
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
            TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientosAsociados, "Id", "", true, 1);
            upgvVencimientos.Update();

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Evento click del link Ver Historial del Vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerHistorialVencimientos_Click(object sender, EventArgs e)
        {
            //Abriendo ventana de vencimientos
            alternaVentanaModal(lkbVerHistorialVencimientos, "ListaVencimientos");
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
                    //Determinando el comando a ejecutar
                    switch (((Button)sender).CommandName)
                    {
                        case "Obligatorio":
                            //Asignando resultado
                            lblMensajeAsignacion.Text = lblMensajeHistorialVencimientos.Text;
                            break;
                        case "Opcional":
                            //Declaramos variables para almacenar el tipo de recurso
                            MovimientoAsignacionRecurso.Tipo tipo;
                            int tipo_unidad;
                            //Obtenemos Tipo de recurso  para su asignación
                            obtieneTipoRecurso(out tipo, out tipo_unidad);
                            //Validamos Asignación Estancias del Recurso
                            RetornoOperacion resultado = MovimientoAsignacionRecurso.ValidaEstanciaAsignacionRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                            //Si no hay problema
                            if (resultado.OperacionExitosa)
                            {
                                //Asignando recurso
                                asignaRecurso(tipo, tipo_unidad);
                                //Actualizando lista de unidades
                                upgvRecursosAsignados.Update();
                                upgvRecursosDisponibles.Update();
                            }
                            else
                                //Establecemos Mensaje de error
                                lblMensajeAsignacion.Text = resultado.Mensaje;
                            break;
                    }
                    //Actualizando mensaje de resultado
                    uplblMensajeAsignacion.Update();

                    //Ocultando ventana de notificación de vencimientos
                    alternaVentanaModal(btnAceptarIndicadorVencimientos, "IndicadorVencimientos");
                    break;

                case "MovVacio":

                    //Determinando el comando a ejecutar
                    switch (((Button)sender).CommandName)
                    {
                        case "Obligatorio":
                            //Asignando resultado
                            lblMensajeAsignacion.Text = lblMensajeHistorialVencimientos.Text;

                            break;
                        case "Opcional":
                            //Registramos un Movimiento en Vacio
                            RetornoOperacion resultado = wucReubicacion.RegistraMovimientoVacioSinOrden();

                            //Si no existe Error
                            if (resultado.OperacionExitosa)
                            {
                                //Cargamos Recursos Disponibles
                                cargaRecursosDiponibles();
                                //Actualziando gridview
                                upgvRecursosDisponibles.Update();
                            }
                            //Si no se pudo guardar el movimiento, se actualiza panel para visualizar errores 
                            else
                            {
                                //Actualizando mensajes de error
                                upwucReubicacion.Update();
                                //Mostrando ventana de movimiento
                                alternaVentanaModal(this, "ReubicacionUnidades");
                            }
                            break;
                    }
                    //Ocultando ventana de notificación de vencimientos
                    alternaVentanaModal(btnAceptarIndicadorVencimientos, "IndicadorVencimientos");                   
                    break;
            }
        }
        /// <summary>
        /// Evento click del link cerrar historial de vencimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarHistorialVencimientos_Click(object sender, EventArgs e)
        {
            //Cerrar ventana de vencimientos
            alternaVentanaModal(lkbCerrarHistorialVencimientos, "ListaVencimientos");
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

        #endregion       
        /// <summary>
        /// Administra la visualización de ventanas modales en la página (muestra/oculta)
        /// </summary>
        /// <param name="control">Control que afecta a la ventana</param>
        /// <param name="nombre_script_ventana">Nombre del script de la ventana</param>
        private void alternaVentanaModal(Control control, string nombre_script_ventana)
        {
            //Determinando que ventana será afectada (mostrada/ocultada)
            switch (nombre_script_ventana)
            { 
                case "RecursosAsignados":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoUnidadesAsignadas", "unidadesAsignadas");
                    break;
                case "ServiciosAsignadosUnidad":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoServiciosAsignados", "modalServiciosAsignados");
                    break;
                case "AnticiposPendientes":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoAnticiposPendientes", "modalAnticiposPendientes");
                    break;
                case "ReubicacionUnidades":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionUbicacion", "confirmacionUbicacion");
                    break;
                case "UbicacionOrigenDestino":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoDatosUbicacion", "datosUbicacion");
                    break;
                case "ConfirmacionAsignacionRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionAsignacionRecurso", "confirmacionAsignacionRecurso");
                    break;
                case "ConfirmacionQuitarRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionQuitarRecursos", "confirmacionQuitarRecursos");
                    break;
                case "IndicadorVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                    break;
                case"ListaVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalHistorialVencimientos", "vencimientosRecurso");
                    break;
                case "LiberacionRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");
                    break;
            }
        }

        #region Liberacion Recurso

        #region Métodos
        /// <summary>
        /// Método encargado de la Liberación de las Unidades y Operadores
        /// </summary>
        /// <param name="tipo">Tipo de recurso (Unidad/Operador)</param>
        /// <returns></returns>
        private RetornoOperacion LiberarRecurso(MovimientoAsignacionRecurso.Tipo tipo)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //De acuerdo al Tipo de Recurso
            switch (tipo)
            {
                //Unidad
                case MovimientoAsignacionRecurso.Tipo.Unidad:
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Unidad
                        resultado = objUnidad.LiberarUnidad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;
                //Operador
                case MovimientoAsignacionRecurso.Tipo.Operador:
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Operador
                        resultado = objOperador.LiberarOperador(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;

            }
            //Mostramos Mensaje
            lblError.Text = resultado.Mensaje;

            //Devolvemos Objeto retorno
            return resultado;
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Efvento generado al liberar un Recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbLiberar_Click(object sender, EventArgs e)
        {
            //Declaramos variable Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Seleccion el Servicio
            if (gvRecursosDisponibles.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRecursosDisponibles, sender, "lnk", false);

                //Declaramos variables para almacenar el tipo de recurso
                MovimientoAsignacionRecurso.Tipo tipo;
                int tipo_unidad;

                //Obtenemos Tipo de recurso  para su asignación
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Validamos Asignación ligadas al Recurso  para su liberación
                resultado = MovimientoAsignacionRecurso.ValidaLiberacionRecurso(tipo, Convert.ToInt32(gvRecursosDisponibles.SelectedValue));

                //Si existe asignación ligada
                if (!resultado.OperacionExitosa)
                {
                    
                    //Asignamos Mensaje
                    lblMensajeLiberacionRecurso.Text = resultado.Mensaje;

                    //Mostrando ventana de confirmación de recurso asociado
                    alternaVentanaModal(upgvRecursosDisponibles, "LiberacionRecurso");
                }
                else
                {
                    //Liberamos Recurso
                    resultado = LiberarRecurso(tipo);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Actualizamos la consulta de unidades disponibles
                        buscaRecursosDisponibles();
                        //Cerramos Ventana modal
                        alternaVentanaModal(upgvRecursosDisponibles, "LiberacionRecurso");
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al cancelar la libración de las Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarLiberacionRecurso_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana modal
            alternaVentanaModal(upbtnCancelarLiberacionRecurso, "LiberacionRecurso");
        }

        /// <summary>
        /// Evento generado al liberar los recurso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarLiberacionRecurso_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Declaramos variables para almacenar el tipo de recurso
            MovimientoAsignacionRecurso.Tipo tipo;
            int tipo_unidad;

            //Obtenemos Tipo de recurso  para su asignación
            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //Liberamos Recurso
            resultado = LiberarRecurso(tipo);

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Actualizamos la consulta de unidades disponibles
                buscaRecursosDisponibles();
            }
            //Cerramos Ventana modal
            alternaVentanaModal(upgvRecursosDisponibles, "LiberacionRecurso");
        }
        #endregion
        #endregion
    }
}