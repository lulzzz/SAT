using SAT_CL.Despacho;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;

namespace SAT.UserControls
{
    public partial class wucParadaEvento : System.Web.UI.UserControl
    {
        #region Atributos

        protected string GUID;

        private int _id_parada;
        private int _id_evento;
        private DataTable _mit_eventos;
        /// <summary>
        /// Id de Parada de Interés
        /// </summary>
        public int id_parada { get { return this._id_parada; } }
        /// <summary>
        /// Id de Evento de Interés
        /// </summary>
        public int id_evento { get { return this._id_evento; } }
        /// <summary>
        /// COnjunto de Eventos existentes en la parada
        /// </summary>
        public DataTable mit_eventos { get { return this._mit_eventos; } }

        #endregion

        #region Manejadores de Eventos

        /// <summary>
        /// Manejador Actualización de Evento
        /// </summary>
        public event EventHandler BtnActualizar_Click;
        /// <summary>
        /// Manejador Cancelación de Edición
        /// </summary>
        public event EventHandler BtnCancelar_Click;
        /// <summary>
        /// Manejador Nuevo Evento
        /// </summary>
        public event EventHandler BtnNuevo_Click;
        /// <summary>
        /// Manejador de Eliminar Evento
        /// </summary>
        public event EventHandler LkbEliminar_Click;

        /// <summary>
        /// Evento Click en Botón Actualizar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnBtnActualizar_Click(EventArgs e)
        { 
            //Si hay asignación de manejador desde interfaz
            if (this.BtnActualizar_Click != null)
                this.BtnActualizar_Click(this, e);
        }
        /// <summary>
        /// Evento Click en Botón Cancelar
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnBtnCancelar_Click(EventArgs e)
        {
            //Si hay asignación de manejador desde interfaz
            if (this.BtnCancelar_Click != null)
                this.BtnCancelar_Click(this, e);
        }
        /// <summary>
        /// Evento Click en Botón Nuevo
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnBtnNuevo_Click(EventArgs e)
        {
            //Si hay asignación de manejador desde interfaz
            if (this.BtnNuevo_Click != null)
                this.BtnNuevo_Click(this, e);
        }
        /// <summary>
        /// Evento Click del link de eliminación de evento
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnLkbEliminar_Click(EventArgs e)
        {
            //Si hay asignación de manejador desde interfaz
            if (this.LkbEliminar_Click != null)
                this.LkbEliminar_Click(this, e);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Carga del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.GUID = Guid.NewGuid().ToString("N");
            //Validando que se Produjo un PostBack
            if (Page.IsPostBack)
                //Recuperando Atributos
                recuperaAtributos();
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Recuperando Atributos
            asignaAtributos();
        }
        /// <summary>
        /// Click en botón de exportación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarEventos_Click(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(this._mit_eventos, "");
        }
        /// <summary>
        /// Cambio de Tamaño de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEventos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño de gv
            Controles.CambiaTamañoPaginaGridView(gvEventos, this._mit_eventos, Convert.ToInt32(ddlTamanoEventos.SelectedValue), true, 1);
        }
        /// <summary>
        /// Cambio en indice de página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvEventos, this._mit_eventos, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Click en botón de GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionEvento_Click(object sender, EventArgs e)
        {
            //Si existen registros
            if (gvEventos.DataKeys.Count > 0)
            {
                //Seleccionando evento
                Controles.SeleccionaFila(gvEventos, sender, "lnk", false);
                this._id_evento = Convert.ToInt32(gvEventos.SelectedDataKey["IdEvento"]);

                //Obteniendo elemento que produce el evento
                LinkButton lkb = (LinkButton)sender;

                //Determinando comando a ejecutar
                switch (lkb.CommandName)
                {
                    case "Editar":
                        //Realizando configuración para edición
                        configuraVistaControl();                        
                        break;
                    case "Eliminar":
                        //Generando evento
                        if (this.LkbEliminar_Click != null)
                            OnLkbEliminar_Click(e);
                        break;
                    case "Bitacora":                        
                        //Construyendo URL 
                        string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta(Request.AppRelativeCurrentExecutionFilePath, "~/Accesorios/BitacoraRegistro.aspx?idT=7&idR=" + this._id_evento.ToString() + "&tB=Evento de Parada");
                        //Definiendo Configuracion de la Ventana
                        string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                        
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, this.Page);
                        break;
                }
            }
        }
        /// <summary>
        /// Click en botón de control eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEventoParada_Click(object sender, EventArgs e)
        {
            //Determinando comando a ejecutar
            switch (((Button)sender).CommandName)
            { 
                case "Nuevo":
                    //Si se ha asignado un manejador de eventos para esta acción
                    if (this.BtnNuevo_Click != null)
                        OnBtnNuevo_Click(e);
                    break;
                case "Actualizar":
                    //Si se ha asignado un manejador de eventos para esta acción
                    if (this.BtnActualizar_Click != null)
                        OnBtnActualizar_Click(e);
                    break;
                case "Cancelar":
                    //Si se ha asignado un manejador de eventos para esta acción
                    if (this.BtnCancelar_Click != null)
                        OnBtnCancelar_Click(e);
                    break;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_id_parada"] = this._id_parada;
            ViewState["_id_evento"] = this._id_evento;
            ViewState["_mit_eventos"] = this._mit_eventos;
        }
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Validando que Existan atributos antes de reasignar
            if (ViewState["_id_parada"] != null && ViewState["_id_evento"] != null)
            {
                this._id_parada = Convert.ToInt32(ViewState["_id_parada"]);
                this._id_evento = (Convert.ToInt32(ViewState["_id_evento"]));
                if (ViewState["_mit_eventos"] != null)
                    this._mit_eventos = (DataTable)ViewState["_mit_eventos"];
            }
        }
        /// <summary>
        /// Inicializa el contenido del control a partir de una parada
        /// </summary>
        /// <param name="id_parada">Id de Parada a consultar</param>
        public void InicializaControl(int id_parada)
        {
            //Inicializando control
            inicializaControl(id_parada, 0);
        }
        /// <summary>
        /// Inicializa el contenido del control a partir de una parada y evento
        /// </summary>
        /// <param name="id_parada">Id de Parada a consultar</param>
        /// <param name="id_evento">Id de Evento</param>
        public void InicializaControl(int id_parada, int id_evento)
        {
            //Inicializando control
            inicializaControl(id_parada, id_evento);
        }
        /// <summary>
        /// Inicializa el contenido del control
        /// </summary>
        /// <param name="id_parada"></param>
        /// <param name="id_evento"></param>
        private void inicializaControl(int id_parada, int id_evento)
        { 
            //Guardando sobre atributos de objeto
            this._id_parada = id_parada;
            this._id_evento = id_evento;
            
            //Instanciando parada actual
            using (Parada p = new Parada(this._id_parada))
            {
                //Configurando encabezado
                h2EncabezadoControlParada.InnerText = string.Format("Eventos de Parada '{0}'", Cadena.TruncaCadena(p.descripcion, 45, "..."));
                //Actualizando panel
                uph2EncabezadoControlParada.Update();
            }

            //Tamaño Grid Eventos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEventos, "", 26);

            //Cargando eventos de parada
            cargaEventosParada();
            //Configurando vista solicitada (resumen de eventos / edición de evento)
            configuraVistaControl();
        }
        /// <summary>
        /// Carga los eventos de la parada solicitada
        /// </summary>
        private void cargaEventosParada()
        {
            //Cargando los eventos asociados a la parada
            using (DataTable mit = ParadaEvento.CargaEventosParaActualizacionFechas(this._id_parada))
            {
                //inicialziando selecciones
                Controles.InicializaIndices(gvEventos);
                //Cargando GridView
                Controles.CargaGridView(gvEventos, mit, "IdEvento", "", true, 1);
                //Almacenando origen de datos
                this._mit_eventos = mit;

                //Si hay registros
                if (this._mit_eventos != null && this._id_evento > 0)
                    //Se busca y selecciona el evento solicitado
                    Controles.MarcaFila(gvEventos, this._id_evento.ToString(), "IdEvento", "IdEvento", this._mit_eventos, "", Convert.ToInt32(ddlTamanoEventos.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Determina si se mostrará la vista de resumen de eventos o edición de eventos
        /// </summary>
        private void configuraVistaControl()
        { 
            //Si hay un evento seleccionado para visualizar
            if (this._id_evento > 0)
            {
                //Cargando catálogo de Retraso en eventos
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMotivoRetraso, "Ninguno", 64);
                //Instanciando parada actual
                using (Parada parada = new Parada(this._id_parada))
                    //Cargando catálogo de eventos disponibles
                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEventos, 40, "", parada.id_tipo_parada, "", 0, "");

                //Instanciando evento solicitado
                using (SAT_CL.Despacho.ParadaEvento evento = new ParadaEvento(this._id_evento))
                {
                    //Asignando datos de evento sobre controles
                    ddlTipoEventos.SelectedValue = evento.id_tipo_evento.ToString();
                    txtCitaEvento.Text = Fecha.ConvierteDateTimeString(evento.cita_evento, "dd/MM/yyyy HH:mm");
                    txtFechaInicioEvento.Text = Fecha.ConvierteDateTimeString(evento.inicio_evento, "dd/MM/yyyy HH:mm");
                    txtFechaFinEvento.Text = Fecha.ConvierteDateTimeString(evento.fin_evento, "dd/MM/yyyy HH:mm");
                    ddlMotivoRetraso.SelectedValue = evento.id_motivo_retraso_evento.ToString();
                }

                //Cambiando vista a edición
                mtvEventosParada.SetActiveView(vwEdicionEvento);
            }
            //Si no hay evento
            else
                //Cambiando vista a resumen
                mtvEventosParada.SetActiveView(vwResumenEventos);

            //Actualziando UpdatePanel Principal (switch de vista)
            upmtvEventosParada.Update();
        }
        /// <summary>
        /// Realiza el guardado del evento en edición
        /// </summary>
        public RetornoOperacion GuardaEvento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos Fechas
            DateTime citaEvento = DateTime.MinValue;
            DateTime inicioEvento = DateTime.MinValue;
            DateTime finEvento = DateTime.MinValue;
            //Realizando conversión de fechas 
            DateTime.TryParse(txtCitaEvento.Text, out citaEvento);
            DateTime.TryParse(txtFechaInicioEvento.Text, out inicioEvento);
            DateTime.TryParse(txtFechaFinEvento.Text, out finEvento);

            //Inicialziando transacción
            using (System.Transactions.TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Evento
                using (ParadaEvento objEvento = new ParadaEvento(this._id_evento))
                {
                    //Realizando actualización de cita (si es requerido)
                    resultado = objEvento.ActualizaCitaEvento(citaEvento, ((Usuario)Session["usuario"]).id_usuario);

                    //Si no hay errores y se actualizan los atributos de la insancia evento despúes de la actualización de cita
                    if (resultado.OperacionExitosa && objEvento.ActualizaParadaEvento())
                        //Editamos Evento
                        resultado = objEvento.EditaParadaEventoEnDespacho(Convert.ToByte(ddlTipoEventos.SelectedValue), inicioEvento, ParadaEvento.TipoActualizacionInicio.Manual,
                                                                     finEvento, ParadaEvento.TipoActualizacionFin.Manual,
                                                                     Convert.ToByte(ddlMotivoRetraso.SelectedValue), ((Usuario)Session["usuario"]).id_usuario);
                    else

                        resultado = new RetornoOperacion("Error al actualizar fecha de cita de evento.");
                }

                //Si no hay errores
                if (resultado.OperacionExitosa)
                    //Confirmando cambios realizados
                    scope.Complete();
            }

            //Validamos Resultado final
            if (resultado.OperacionExitosa)
            {
                //Asignando Id de Evento guardado
                this._id_evento = resultado.IdRegistro;
                //Cargamos Eventos
                cargaEventosParada();

                //Cambiando a vista de resumen
                mtvEventosParada.SetActiveView(vwResumenEventos);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Cancela el alta/edición de un evento, retornando a la vista de resumen
        /// </summary>
        public void CancelarActualizacion()
        {
            //Quitando petición de selección de evento
            this._id_evento = 0;

            //Cargando listado de eventos
            cargaEventosParada();

            //Cambiando a vista de resumen
            mtvEventosParada.SetActiveView(vwResumenEventos);
            
        }
        /// <summary>
        /// Realiza la deshabilitación del registro evento activo en el control
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EliminaEvento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que exista un evento para deshabilitar
            if (this._id_evento > 0)
            {
                //Instanciamos Evento
                using (ParadaEvento objEvento = new ParadaEvento(this._id_evento))
                {
                    //Instanciando servicio del evento
                    using (SAT_CL.Documentacion.Servicio srv = new SAT_CL.Documentacion.Servicio(objEvento.id_servicio))
                    {
                        //Si el servicio está documentado
                        if (srv.estatus == SAT_CL.Documentacion.Servicio.Estatus.Documentado)
                            //Editamos Parada bajo reglas de documentación
                            resultado = objEvento.DeshabilitaParadaEventoEnServicio(((Usuario)Session["usuario"]).id_usuario, this._mit_eventos != null ? this._mit_eventos.Rows.Count : 0);
                        else
                            //Editamos Parada bajo reglas de despacho
                            resultado = objEvento.DeshabilitaParadaEventoEnDespacho(((Usuario)Session["usuario"]).id_usuario, this._mit_eventos != null ? this._mit_eventos.Rows.Count : 0);
                    }
                }
            }
            //Si no hay parada
            else
                resultado = new RetornoOperacion("No hay un evento que eliminar.");

            //Validamos Resultado final
            if (resultado.OperacionExitosa)
            {
                //Borrando Id de Evento
                this._id_evento = 0;
                //Cargamos Eventos
                cargaEventosParada();

                //Cambiando vista a resumen
                mtvEventosParada.SetActiveView(vwResumenEventos);
            }

            //Devolviendo resultado
            return resultado;
        }
        /// <summary>
        /// Realiza la inserción de un nuevo evento con el tipo predeterminado para una secuencia de paradas
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion NuevoEvento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validando que exista una parada para asociar evento
            if (this._id_parada > 0)
            {
                //Instanciando parada
                using (Parada parada = new Parada(this._id_parada))
                    //Editamos Parada
                    resultado = ParadaEvento.InsertaParadaEventoEnDespacho(parada.id_servicio, parada.id_parada, this._mit_eventos != null ? this._mit_eventos.Rows.Count : 0, ((Usuario)Session["usuario"]).id_usuario);
            }
            //Si no hay parada
            else
                resultado = new RetornoOperacion("No hay una parada a la cual asignar un nuevo evento.");

            //Validamos Resultado final
            if (resultado.OperacionExitosa)
            {
                //Borrando Id de Evento
                this._id_evento = resultado.IdRegistro;
                //Cargamos Eventos
                cargaEventosParada();

                //Cambiando vista a resumen
                mtvEventosParada.SetActiveView(vwResumenEventos);
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion
        
    }
}