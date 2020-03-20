using SAT_CL;
using SAT_CL.Despacho;
using SAT_CL.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucPreAsignacionRecurso : System.Web.UI.UserControl
    {
        #region Atributos

        private int _idMovimiento;
        private DataSet _ds;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack en la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se produzca un PostBack
            if (!(Page.IsPostBack))
                
                //Asignando Atributos
                asignaAtributos();
            else
                //Recuperando Atributos
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

        #region Manejadores de Evento

        /// <summary>
        /// Manejador de Evento "Asignar Recurso"
        /// </summary>
        public event EventHandler ClickAsignarRecurso;

        /// <summary>
        /// Evento que Manipula el Manejador "Asignar Recurso"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickAsignarRecurso(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickAsignarRecurso != null)
                //Iniciando Evento
                ClickAsignarRecurso(this, e);
        }

        /// <summary>
        /// Manejador de Evento "Liberar Recurso"
        /// </summary>
        public event EventHandler ClickLiberarRecurso;

        /// <summary>
        /// Evento que Manipula el Manejador "Liberar Recurso"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickLiberarRecurso(EventArgs e)
        {
            //Validando que exista el Evento
            if (ClickLiberarRecurso != null)
                //Iniciando Evento
                ClickLiberarRecurso(this, e);
        }


        #endregion

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
                    cargaUnidades();
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
            lblError.Text = "";
            //lblErrorKm.Text = "";

            //Inicializamos los indicadores
            //inicializaIndicadores();
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
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Inicializando GridView
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
        /// Evento disparado al dar click en el linkbutton de la ventana modal de servicios asignados a unidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarServiciosAsignados_Click(object sender, EventArgs e)
        {
            //Cerrando modal de servicios asignados a la unidad
            alternaVentanaModal(uplnkCerrarServiciosAsignados, "ServiciosAsignadosUnidad");
        }
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

        #region Eventos GridView "Recursos Disponibles"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRecursosDisponibles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Validando que Existan Recursos Disponibles
            if(gvRecursosDisponibles.DataKeys.Count > 0)
            
                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;
            
            //Cambia Tamaño del GridView 
            Controles.CambiaTamañoPaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), Convert.ToInt32(ddlTamanoRecursosDisponibles.SelectedValue), true, 5);
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
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"));
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validando que Existan Recursos Disponibles
            if (gvRecursosDisponibles.DataKeys.Count > 0)

                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;
            
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.NewPageIndex, false, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Recursos Disponibles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursosDisponibles_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que Existan Recursos Disponibles
            if (gvRecursosDisponibles.DataKeys.Count > 0)

                //Asignando Ordenamiento
                this._ds.Tables["Table"].DefaultView.Sort = lblOrdenarRecursosDisponibles.Text;

            //Ordenando el GridView
            lblOrdenarRecursosDisponibles.Text = Controles.CambiaSortExpressionGridView(gvRecursosDisponibles, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table"), e.SortExpression, false, 1);
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

                    //Obteniendo Origen de la Fila
                    DataRowView rowView = (DataRowView)e.Row.DataItem;

                    //Obteniendo Servicio
                    int no_servicio = Convert.ToInt32(rowView["NoServicio"].ToString() == "" ? "0" : rowView["NoServicio"].ToString());
                    string estatus = rowView["Estatus"].ToString();

                    //Instanciando Link
                    using (LinkButton lkb = (LinkButton)e.Row.FindControl("lkbLiberar"))
                    {
                        //Validando que exista no exista el Servicio
                        if (no_servicio != 0 && (estatus.Equals("Ocupado") || estatus.Equals("Parada Ocupado")))

                            //Mostrando Control
                            lkb.Visible = true;
                        else
                            //Ocultando Control
                            lkb.Visible = false;
                    }
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
                    //Validando que exista un Evento
                    if (ClickLiberarRecurso != null)
                        
                        //Iniciando Manejador
                        OnClickLiberarRecurso(e);
                }
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
            if (this._idMovimiento > 0)
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
                    alternaVentanaModal(upgvRecursosDisponibles, "ConfirmacionAsignacionRecurso");
                }
                else
                {
                    //Validando que exista un Evento
                    if (ClickAsignarRecurso != null)

                        //Iniciando Manejador
                        OnClickAsignarRecurso(e);
                }
            }
            else
                //Mostramos Mensaje
                lblError.Text = "Seleccione el Movimiento";
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

        #endregion

        #region Eventos Liberación 

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

            //Cerramos Ventana modal
            alternaVentanaModal(upgvRecursosDisponibles, "LiberacionRecurso");

            //Validando que exista un Evento
            if (ClickLiberarRecurso != null)

                //Iniciando Manejador
                OnClickLiberarRecurso(e);
        }

        #endregion

        #region Eventos Agregar Recurso

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
                //Validando que exista un Evento
                if (ClickAsignarRecurso != null)

                    //Iniciando Manejador
                    OnClickAsignarRecurso(e);
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

        #endregion

        #region Eventos Vencimientos

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

                            //Ocultando ventana de notificación de vencimientos
                            alternaVentanaModal(btnAceptarIndicadorVencimientos, "IndicadorVencimientos");

                            //Si no hay problema
                            if (resultado.OperacionExitosa)
                            {
                                //Actualizando lista de unidades
                                upgvRecursosDisponibles.Update();

                                //Validando que exista un Evento
                                if (ClickAsignarRecurso != null)

                                    //Iniciando Manejador
                                    OnClickAsignarRecurso(e);
                            }
                            else
                                //Establecemos Mensaje de error
                                lblMensajeAsignacion.Text = resultado.Mensaje;
                            break;
                    }

                    //Actualizando mensaje de resultado
                    uplblMensajeAsignacion.Update();

                    
                    break;
            }
        }

        #endregion

        #region Eventos Servicios Asignados

        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Servicios Asignados"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosAsignados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvServiciosAsignados, OrigenDatos.RecuperaDataTableDataSet(this._ds, "Table2"), e.NewPageIndex, false, 1);
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

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método encargado de Inicializar el Control
        /// </summary>
        private void inicializaControl()
        {
            //Limpiando Controles
            lblError.Text = 
            txtNombreOperador.Text =
            txtNoUnidad.Text =
            txtPropietarioUnidad.Text = "";
            
            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializamos Grid View
            Controles.InicializaGridview(gvRecursosDisponibles);
        }
        /// <summary>
        /// Método encargado de Cargar los Controles de Catalogo
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamano Recursos Disponibles
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRecursosDisponibles, "", 56);
            //Tipo Unidad
            //CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoUnidad, 24, "");
            //Estatus Unidad
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusUnidad, "Todos", 53);
            //Estatus Operador
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusOperador, "Todos", 57);
        }
        /// <summary>
        /// Método encargado de Asignar los Atributos del Control
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdMovimiento"] = this._idMovimiento;
            ViewState["DS"] = this._ds;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos del Control
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Exista el Servicio
            if (Convert.ToInt32(ViewState["IdMovimiento"]) > 0)

                //Asignando Valor
                this._idMovimiento = Convert.ToInt32(ViewState["IdMovimiento"]);

            //Validando que existan Registros
            if (ViewState["DS"] != null)

                //Asignando Valores
                this._ds = (DataSet)ViewState["DS"];
        }
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
                case "ConfirmacionAsignacionRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionAsignacionRecurso", "confirmacionAsignacionRecurso");
                    break;
                case "ConfirmacionQuitarRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionQuitarRecursos", "confirmacionQuitarRecursos");
                    break;
                case "IndicadorVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalIndicadorVencimientos", "contenidoModalIndicadorVencimientos");
                    break;
                case "ListaVencimientos":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "modalHistorialVencimientos", "vencimientosRecurso");
                    break;
                case "LiberacionRecurso":
                    ScriptServer.AlternarVentana(control, nombre_script_ventana, "contenidoConfirmacionLiberacionRecurso", "confirmacionLiberacionRecurso");
                    break;
            }
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
        /// Realiza la carga de las Unidades 
        /// </summary>
        private void cargaUnidades()
        {
            //Obteniendo las Unidades Disponibles
            using (DataTable dt = Unidad.CargaUnidadesParaAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoUnidad.Text, 0,
                                  Convert.ToByte(ddlEstatusUnidad.SelectedValue), Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtPropietarioUnidad.Text, ':', 1), "0")), false,
                                  Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUbicacionUnidad.Text, ':', 1), "0"))))
            {
                //Cargando GridView de Recursos Disponibles
                Controles.CargaGridView(gvRecursosDisponibles, dt, "Id-id_ubicacion-Asignaciones-PorLiquidar-PendientesLiq", lblOrdenarRecursosDisponibles.Text, false, 5);

                //Validando que la Tabla no sea null
                if (dt != null)

                    //Añadiendo Tabla
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table");
                else
                    //Eliminamos Tabla
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
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

                    //Añadiendo Tabla
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table");
                else
                    //Eliminamos Tabla
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
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

                    //Añadiendo Tabla
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table");
                else
                    //Eliminamos Tabla
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table");
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
                Controles.InicializaIndices(gvAnticiposPendientes);
                //Cargando GridView de Viajes
                Controles.CargaGridView(gvAnticiposPendientes, dt, "", "", false, 0);
                //Validando que el datatable no sea null
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = TSDK.Datos.OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table1");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table1");
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
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table2");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table2");
                }
            }
        }
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
                if (dt != null)
                {
                    //Añadiendo Tabla a DataSet de Session
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, dt, "Table2");
                }
                else
                {
                    //Borrando de sesión los viajes cargados anteriormente
                    this._ds = OrigenDatos.EliminaTablaDataSet(this._ds, "Table2");
                }
            }
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
                    //Operador
                    tipo = MovimientoAsignacionRecurso.Tipo.Operador;
                    break;
                case 2:
                    //Tercero
                    tipo = MovimientoAsignacionRecurso.Tipo.Tercero;
                    break;
            }
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
                    this._ds = OrigenDatos.AñadeTablaDataSet(this._ds, mitVencimientosAsociados, "Table3");

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
                    OrigenDatos.EliminaTablaDataSet(this._ds, "Table3");

                //Cargando GridView de Vencimientos
                TSDK.ASP.Controles.CargaGridView(gvVencimientos, mitVencimientosAsociados, "Id", "", true, 1);
                upgvVencimientos.Update();
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Método encargado de Inicializar la Pre-asignación
        /// </summary>
        /// <param name="id_movimiento">Servicio</param>
        public void InicializaPreAsignacion(int id_movimiento)
        {
            //Asignando Servicio
            this._idMovimiento = id_movimiento;

            //Invocando Método de Inicialización
            inicializaControl();
        }
        /// <summary>
        /// Método encargado de Asignar y/ó Pre-Asignar los Recursos a un Servicio
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion AsignaRecursoViaje()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Validando que exista el Servicio
            if (this._idMovimiento > 0)
            {
                //Declarando Variable Auxiliares
                MovimientoAsignacionRecurso.Tipo tipo; int tipo_unidad;

                //Obteniendo Tipo de Recurso
                obtieneTipoRecurso(out tipo, out tipo_unidad);

                //Realizando la asignación del Recuros 
                result = MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecurso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                           this._idMovimiento, tipo, tipo_unidad, Convert.ToInt32(gvRecursosDisponibles.SelectedValue),
                           ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);                
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No hay movimiento al cual asignar el recurso.");

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)
            {
                //Validando que exista un Asignación Activa
                if (result.IdRegistro > 0)
                    //Buscando Recursos Disponibles
                    buscaRecursosDisponibles();
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("El recurso solicitado ya fue asignado al movimiento.");
            }

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;

            //Inicializando Indices
            Controles.InicializaIndices(gvRecursosDisponibles);

            //Actualizando Controles
            uplblError.Update();
            upgvRecursosDisponibles.Update();

            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método encargado de Liberar el Recurso a la Unidad/Operador al que este Ligado
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion LiberaRecurso()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declarando Variable Auxiliares
            MovimientoAsignacionRecurso.Tipo tipo; int tipo_unidad;
            
            //Obteniendo Tipo de Recurso
            obtieneTipoRecurso(out tipo, out tipo_unidad);

            //De acuerdo al Tipo de Recurso
            switch (tipo)
            {
                //Unidad
                case MovimientoAsignacionRecurso.Tipo.Unidad:
                    //Instanciamos Unidad
                    using (Unidad objUnidad = new Unidad(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Unidad
                        result = objUnidad.LiberarUnidad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;
                //Operador
                case MovimientoAsignacionRecurso.Tipo.Operador:
                    //Instanciamos Operador
                    using (Operador objOperador = new Operador(Convert.ToInt32(gvRecursosDisponibles.SelectedValue)))
                    {
                        //Liberamos Operador
                        result = objOperador.LiberarOperador(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    }
                    break;
            }

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Buscando Recursos Disponibles
                buscaRecursosDisponibles();

            //Mostrando Mensaje de Operación
            lblError.Text = result.Mensaje;

            //Devolviendo Resultado Obtenido
            return result;
        }



        #endregion
    }
}