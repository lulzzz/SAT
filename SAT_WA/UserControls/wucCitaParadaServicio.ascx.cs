using SAT_CL.Despacho;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucCitaParadaServicio : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Origen de datos con las paradas del servicio solicitado
        /// </summary>
        private DataTable _mitParadasServicio;
        /// <summary>
        /// Id de Servicio a consultar
        /// </summary>
        private int _id_servicio;

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Actualización de fecha de cita de la parada seleccionada en el control
        /// </summary>
        public event EventHandler ClickActualizarCita;

        /// <summary>
        /// Evento que Manipula el Manejador "Actualizar Cita de Parada"
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnClickActualizarCita(EventArgs e)
        {   
            //Validando que exista el Evento
            if (ClickActualizarCita != null)
                //Iniciando Evento
                ClickActualizarCita(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Carga de control de usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no hay un postback que afecte al control
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
        {   //Invocando Método de Asignación
            asignaAtributos();
        }
        /// <summary>
        /// Clic en botón de GridView de Paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCitaParada_Click(object sender, EventArgs e)
        {
            //Determinando que botón fue pulsado
            switch (((LinkButton)sender).CommandName)
            { 
                case "Editar":
                    //Seleccionando fila y cambiando a modo edición
                    Controles.SeleccionaFila(gvParadasServicio, sender, "lnk", true);
                    //Recargando contenido de gridview
                    Controles.CargaGridView(gvParadasServicio, this._mitParadasServicio, "Id", "", true, 2);
                    //Limpiando errores
                    lblErrorActualizacionCitaParada.Text = "";
                    break;
                case "Aceptar":                    
                    //Validando que exista un Evento
                    if (ClickActualizarCita != null)
                        //Generando evento de actualización de parada
                        OnClickActualizarCita(e);
                    break;
                case"Cancelar":
                    //Inicializando indices de selección
                    Controles.InicializaIndices(gvParadasServicio);
                    //Recargando contenido de gridview
                    Controles.CargaGridView(gvParadasServicio, this._mitParadasServicio, "Id", "", true, 2);
                    //Limpiando errores
                    lblErrorActualizacionCitaParada.Text = "";
                    break;
            }
        }
        /// <summary>
        /// Cambio de página del gridview de paradas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadasServicio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Realizando cambio de página
            Controles.CambiaIndicePaginaGridView(gvParadasServicio, this._mitParadasServicio, e.NewPageIndex, true, 2);
        }
        /// <summary>
        /// Enlace a datos de cada fila del gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvParadasServicio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si existen registros
            if (gvParadasServicio.DataKeys.Count > 0)
            {
                //Para columnas de datos
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;

                    //Determinando el estatus de la fila, sólo para filas normales y alternas
                    if (e.Row.RowState == DataControlRowState.Normal || 
                        e.Row.RowState == DataControlRowState.Alternate)
                    {
                        //Recuperando controles de interés
                        using (LinkButton lkbCita = (LinkButton)e.Row.FindControl("lkbCitaParada"))
                        {
                            using (Label lblCita = (Label)e.Row.FindControl("lblCitaParada"))
                            {
                                //Si la parada ya se encuentra actualizada en su fecha de llegada no es editable
                                DateTime fechaLlegada;
                                DateTime.TryParse(row["FechaLlegada"].ToString(), out fechaLlegada);
                                if (Fecha.EsFechaMinima(fechaLlegada))
                                {
                                    //Si es una parada operativa
                                    if (row.Field<string>("TipoParada") == "Operativa")
                                    {
                                        //Mostrando link de edición y ocultando etiqueta
                                        lkbCita.Visible = true;
                                        lblCita.Visible = false;
                                    }
                                    else
                                    {
                                        //Ocultando link de edición y mostrando etiqueta
                                        lkbCita.Visible = false;
                                        lblCita.Visible = true;
                                    }
                                }
                                else
                                {
                                    //Ocultando link de edición y mostrando etiqueta
                                    lkbCita.Visible = false;
                                    lblCita.Visible = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Exportación de GV de Paradas a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarVencimientoHistorial_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(this._mitParadasServicio, "");
        }
        /// <summary>
        /// Cambio de tamaño de página de GV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoParadasServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de página
            Controles.CambiaTamañoPaginaGridView(gvParadasServicio, this._mitParadasServicio, Convert.ToInt32(ddlTamanoParadasServicio.SelectedValue), true, 2);
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Realizando carga de paradas del servicio solicitado
        /// </summary>
        private void cargaParadasServicio()
        {
            //Inicializando indices
            Controles.InicializaIndices(gvParadasServicio);

            //Cargando conjunto de paradas asociadas al servicio indicado
            using (DataTable mit = Parada.CargaParadasParaVisualizacion(this._id_servicio))
            { 
                //Cargando gridview
                Controles.CargaGridView(gvParadasServicio, mit, "Id", "", true, 2);
                //Guardando origen de datos
                this._mitParadasServicio = mit;
            }
        }
        /// <summary>
        /// Método Privado encargado de Asignar los Atributos de la Forma
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_mitParadasServicio"] = this._mitParadasServicio;
            ViewState["_id_servicio"] = this._id_servicio;
        }
        /// <summary>
        /// Método Privado encargado de Recuperar el Valor de los Atributos de la Forma
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (ViewState != null)
            {
                //Asignando Servicios Maestros
                this._mitParadasServicio = (DataTable)ViewState["_mitParadasServicio"];
                this._id_servicio = Convert.ToInt32(ViewState["_id_servicio"]);
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Inicializa el contenido del control, mostrando las paradas correspondientes
        /// </summary>
        /// <param name="id_servicio">Id de Servicio</param>
        public void InicializaControl(int id_servicio)
        { 
            //Asignando Id de Servicio a mostrar
            this._id_servicio = id_servicio;
            //cargando catálogo de tamañó de página de GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadasServicio, "", 39);
            //Cargando paradas pertenecientes al servicio
            cargaParadasServicio();
            //Limpiando errores
            lblErrorActualizacionCitaParada.Text = "";
        }
        /// <summary>
        /// Realiza la actualziación de la parada solicitada
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion ActualizaCitaParada()
        { 
            //Declarando objeto de retultado sin errores
            RetornoOperacion resultado = new RetornoOperacion(this._id_servicio);

            //Inicializando transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando parada seleccionada
                using (Parada p = new Parada(Convert.ToInt32(gvParadasServicio.SelectedDataKey["Id"])),
                    pAnterior = new Parada(Parada.BuscaParadaAnteriorOperativa(this._id_servicio, p.secuencia_parada_servicio)),
                    pSiguiente = new Parada(Parada.BuscaParadaPosteriorOperativa(this._id_servicio, p.secuencia_parada_servicio)))
                {
                    //Obteniendo fecha por establecer
                    DateTime fecha = Convert.ToDateTime(((TextBox)gvParadasServicio.SelectedRow.FindControl("txtCitaParada")).Text);

                    //Si es la primer parada
                    if (p.secuencia_parada_servicio == 1)
                    {
                        //Si la siguiente parada posee fecha de cita menor o igual a la solicitada
                        if (pSiguiente.cita_parada.CompareTo(fecha) <= 0)
                            resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser menor a '{0:dd/MM/yyyy HH:mm}'", pSiguiente.cita_parada));
                        //De lo contrario se realiza la actualización de la cita en el encabezado de servicio
                        else
                        {
                            using (SAT_CL.Documentacion.Servicio srv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                                resultado = srv.ActualizaCitaCarga(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            if (!resultado.OperacionExitosa)
                                resultado = new RetornoOperacion(string.Format("Error al actualizar cita de carga en encabezado de servicio: {0}", resultado.Mensaje));
                        }
                    }
                    //Si es la última
                    else if (p.secuencia_parada_servicio == new Parada(Parada.ObtieneUltimaParada(this._id_servicio)).secuencia_parada_servicio)
                    {
                        //Si la parada anterior posee fecha de cita mayor o igual a la solicitada
                        if (pAnterior.cita_parada.CompareTo(fecha) >= 0)
                            resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser mayor a '{0:dd/MM/yyyy HH:mm}'", pAnterior.cita_parada));
                        //De lo contrario se actualiza la fecha de descarga en el encabezado de servicio
                        else
                        {
                            using (SAT_CL.Documentacion.Servicio srv = new SAT_CL.Documentacion.Servicio(this._id_servicio))
                                resultado = srv.ActualizaCitaDescarga(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            if (!resultado.OperacionExitosa)
                                resultado = new RetornoOperacion(string.Format("Error al actualizar cita de descarga en encabezado de servicio: {0}", resultado.Mensaje));
                        }
                    }
                    //Si es una intermedia
                    else
                    {
                        //Si la cita de parada apor actualizar no se encuentra entre la cita de la parada anterior y siguiente
                        if (pAnterior.cita_parada.CompareTo(fecha) >= 0 || pSiguiente.cita_parada.CompareTo(fecha) <= 0)
                            resultado = new RetornoOperacion(string.Format("La fecha por actualizar debe ser mayor a '{0:dd/MM/yyyy HH:mm}' y menor que '{1:dd/MM/yyyy HH:mm}'.", pAnterior.cita_parada, pSiguiente.cita_parada));
                    }

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Actualizando parada
                        resultado = p.ActualizaCitaLlegada(fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    //Si no hay errores
                    if (resultado.OperacionExitosa)
                        //Confirmando transacción
                        scope.Complete();
                }
            }

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
                //Recargando Listado de paradas
                cargaParadasServicio();

            //Mostrando resultado
            lblErrorActualizacionCitaParada.Text = resultado.Mensaje;

            //Devolviendo resultado
            return resultado;
        }

        #endregion
                
    }
}