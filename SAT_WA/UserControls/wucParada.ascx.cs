using System;
using System.Web.UI.WebControls;
using System.Data;
using SAT_CL.Despacho;
using SAT_CL.Documentacion;
using TSDK.Base;
using Microsoft.SqlServer.Types;
using TSDK.ASP;
using SAT_CL.Seguridad;
using System.Linq;
using TSDK.Datos;
using System.Transactions;

namespace SAT.UserControls
{
    public partial class wucParada : System.Web.UI.UserControl
    {
        #region Atributos

        /// <summary>
        /// Id Servicio
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id Compania
        /// </summary>
        private int _id_compania;
        public int id_compania { get { return this._id_compania; } }
        /// <summary>
        /// Dataset Paradas
        /// </summary>
        private DataTable _table_Paradas;
        /// <summary>
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickEditar;
        /// <summary>
        /// Declaración del evento ClickAgregarAbajo
        /// </summary>
        public event EventHandler ClickAgregarAbajo;
        /// <summary>
        /// Declaración de Evento ClickAgregarArriba
        /// </summary>
        public event EventHandler ClickAgregarArriba;
        /// <summary>
        /// Declaración de Evento ClickGuardar
        /// </summary>
        public event EventHandler ClickEliminar;
        /// <summary>
        /// Declaración de Evento ClickInsertar Evento
        /// </summary>
        public event EventHandler ClickInsertarEvento;
        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set
            {   //Asignando Orden
                ddlTipoEvento.TabIndex =
                chkTipoParada.TabIndex =
                txtUbicacion.TabIndex =
                txtCita.TabIndex =
                btnAgregarParadaArriba.TabIndex =
                btnGuardar.TabIndex =
                btnAgregarAbajo.TabIndex =
                btnCancelar.TabIndex =
                ddlTamanoParadas.TabIndex =
                lkbExportarParadas.TabIndex =
                gvParadas.TabIndex = value;
            }
            get { return ddlTipoEvento.TabIndex; }
        }

        #endregion

        #region Eventos


        /// <summary>
        /// Evento Generado al Cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
            {
                //Carga catalogos
                cargaCatalogos();
            }
            else
            {
                //Si es PostaBack
                //Recupera Atributos
                recuperaAtributos();
            }
        }

        /// <summary>
        /// Evento producido previo a la carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Almacenando valores viewstate
            asignaAtributos();
        }

        /// <summary>
        /// Evento genereado al Seleccionar una parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbSeleccionarParada(object sender, EventArgs e)
        {
            //Validando si existen Registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvParadas, sender, "lnk", false);

                //Instanciando Parada
                using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey.Value)))
                {
                    //Validando Existencia
                    if (objParada.habilitar)
                    {
                        //Asignando Valores
                        txtCita.Text = objParada.cita_parada == DateTime.MinValue ? "" : objParada.cita_parada.ToString("dd/MM/yyyy HH:mm");
                        chkTipoParada.Checked = objParada.Tipo == Parada.TipoParada.Operativa ? false : true;

                        //Instanciamos Ubicacion
                        using (SAT_CL.Global.Ubicacion objUbicacion = new SAT_CL.Global.Ubicacion(objParada.id_ubicacion))
                        {
                            //Inicializamos Controles
                            txtUbicacion.Text = objUbicacion.descripcion + " ID:" + objUbicacion.id_ubicacion;

                            //Validando Tipo
                            switch (objParada.Tipo)
                            {
                                case Parada.TipoParada.Servicio:
                                    //Cargando Catalogos
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 10, "Ninguno");
                                    break;
                                case Parada.TipoParada.Operativa:
                                    //Cargando Catalogos
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 10, "");
                                    break;
                            }

                            ddlTipoEvento.SelectedValue = ParadaEvento.ObtienerPrimerTipoEvento(objParada.id_parada).ToString();
                        }
                    }
                    
                    
                }
                //Hbailita Controles
                habilitaControles();
                configuraTipoParada();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTipoParada_CheckedChanged(object sender, EventArgs e)
        {
            //Configurando Tipo de Parada
            configuraTipoParada();
        }

        /// <summary>
        /// Evento Generado al dar click en el boton Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Inicializa Valores
            inicializaValores();
            //Inicializamos Indices Grid View
            TSDK.ASP.Controles.InicializaIndices(gvParadas);
            //Habilita Controles
            habilitaControles();
        }

        /// <summary>
        /// Evento generado al Editar un evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbEditarEvento(object sender, EventArgs e)
        {
            //Instanciamos Gid View
            using (GridView gvEventos = (GridView)(((LinkButton)sender).Parent.Parent.Parent.Parent))
            {
                //Validamos Registros
                if (gvEventos.DataKeys.Count > 0)
                {
                    //Seleccionando Fila
                    TSDK.ASP.Controles.SeleccionaFila(gvEventos, sender, "lnk", false);

                    //Editamos Evento
                    editaEvento(Convert.ToInt32(gvEventos.SelectedDataKey.Value), gvEventos);

                }
            }
        }

        /// <summary>
        /// Evento generado al seleccionar un evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbEliminarEvento(object sender, EventArgs e)
        {
            //Instanciamos Gid View
            using (GridView gvEventos = (GridView)(((LinkButton)sender).Parent.Parent.Parent.Parent))
            {
                //Validamos Registros
                if (gvEventos.DataKeys.Count > 0)
                {
                    //Seleccionando Fila
                    TSDK.ASP.Controles.SeleccionaFila(gvEventos, sender, "lnk", false);

                    //Deshabilitamos Evento
                    deshabilitaParadaEvento(Convert.ToInt32(gvEventos.SelectedDataKey.Value), gvEventos.DataKeys.Count, gvEventos);

                }

            }
        }

        /// <summary>
        /// Evento generado al dar click en Bitácora del Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacoraEvento(object sender, EventArgs e)
        {
            //Instanciamos Gid View
            using (GridView gvEventos = (GridView)((LinkButton)sender).Parent.Parent.Parent.Parent.Parent.Parent)
            {
                //Validamos Registros
                if (gvEventos.DataKeys.Count > 0)
                {
                    //Seleccionando Fila
                    TSDK.ASP.Controles.SeleccionaFila(gvEventos, sender, "lnk", false);
                    //Mostramos Bitácora
                    inicializaBitacora(gvEventos.SelectedValue.ToString(), "7", "Bitácora Evento");
                }
            }
        }

        /// Evento generado al dar click en Bitácora de la Parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbBitacoraParada(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
            //Validamos que existan Registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Mostramos Bitácora
                inicializaBitacora(gvParadas.SelectedValue.ToString(), "5", "Bitácora Parada");
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectedIndexChanged_ddlTamanoParadas(object sender, EventArgs e)
        {
            //Asignando Ordenamiento
            this._table_Paradas.DefaultView.Sort = lblOrdenarParadas.Text;

            //Cambiando Tamaño de Registros
            Controles.CambiaTamañoPaginaGridView(gvParadas, this._table_Paradas, Convert.ToInt32(ddlTamanoParadas.SelectedValue));

            //Validando que existan Llaves
            if (gvParadas.DataKeys.Count > 0)
            {
                //Habilitamos Controles
                habilitaControles();

                //Inicializamos Valores
                inicializaValores();
            }
        }

        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sorting_gvParadas(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvParadas.DataKeys.Count > 0)
            {
                //Asignando Ordenamiento
                this._table_Paradas.DefaultView.Sort = lblOrdenarParadas.Text;

                //Cambiando Ordenamiento
                lblOrdenarParadas.Text = Controles.CambiaSortExpressionGridView(gvParadas, this._table_Paradas, e.SortExpression);

                //Habilitamos Controles
                habilitaControles();

                //Inicializamos Valores
                inicializaValores();
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar la Paginación del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PageIndexChanging_gvParadas(object sender, GridViewPageEventArgs e)
        {
            //Asignando Ordenamiento
            this._table_Paradas.DefaultView.Sort = lblOrdenarParadas.Text;

            //Cambiando el Tamaño de la Página
            Controles.CambiaIndicePaginaGridView(gvParadas, this._table_Paradas, e.NewPageIndex);

            //Validando que existan Llaves
            if (gvParadas.DataKeys.Count > 0)
            {
                //Habilitamos Controles
                habilitaControles();

                //Inicializamos Valores
                inicializaValores();
            }
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Excel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Excel
            Controles.ExportaContenidoGridView(this._table_Paradas);
        }

        /// <summary>
        /// Evento Generado al enlazar el Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvParadas.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Eventos
                    using (GridView gvEventos = (GridView)e.Row.FindControl("gvEventos"))
                    {
                        //Buscamos Grid View de Eventos
                        using (LinkButton lkbEliminar = (LinkButton)e.Row.FindControl("lkbEliminarParada"),
                                          lkbSeleccionar = (LinkButton)e.Row.FindControl("lkbSeleccionarParada"),
                                          lkbInsertarEvento = (LinkButton)e.Row.FindControl("lkbInsertarEvento"))
                        {
                            //Carga Eventos para cada una de las Paradas
                            using (DataTable mit = SAT_CL.Despacho.ParadaEvento.CargaEventosParaVisualizacion(Convert.ToInt32(gvParadas.DataKeys[e.Row.RowIndex].Value)))
                            {
                                //Validamos Origen de Datos
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                                {
                                    //Cargamos Grid View Eventos
                                    TSDK.ASP.Controles.CargaGridView(gvEventos, mit, "IdEvento", "");

                                    //De acuaerdo al estatus de la pagina habilitamos controles

                                    switch ((Pagina.Estatus)Session["estatus"])
                                    {
                                        case Pagina.Estatus.Nuevo:
                                            lkbEliminar.Enabled =
                                            lkbSeleccionar.Enabled =
                                            lkbInsertarEvento.Enabled = false;
                                            break;
                                        case Pagina.Estatus.Lectura:
                                            lkbEliminar.Enabled =
                                            lkbInsertarEvento.Enabled =
                                            lkbSeleccionar.Enabled = false;
                                            break;
                                        case Pagina.Estatus.Edicion:
                                            lkbEliminar.Enabled =
                                            lkbInsertarEvento.Enabled =
                                            lkbSeleccionar.Enabled = true;
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento Disparado al Editar la parada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnEditar(object sender, EventArgs e)
        {
            if (ClickEditar != null)

                OnClickEditar(new EventArgs());
        }

        /// <summary>
        /// Evento disparado al presionar el boton Agregar Abajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnAgregarAbajo(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickAgregarAbajo != null)
                OnClickAgregarAbajo(new EventArgs());
        }

        /// <summary>
        /// Evento disparado al presionar el boton Agregar Abajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_btnAgregarArriba(object sender, EventArgs e)
        {
            //Validando que exista un Evento
            if (ClickAgregarArriba != null)
                OnClickAgregarArriba(new EventArgs());
        }

        /// <summary>
        /// Evento disparado al presionar el boton Eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbEliminar(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
            //Validando que exista un Evento
            if (ClickEliminar != null)
                OnClickEliminar(new EventArgs());
        }

        /// <summary>
        /// Evento disparado al presionar el boton Insertar Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClick_lkbInsertarEvento(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvParadas, sender, "lnk", false);
            //Validando que exista un Evento
            if (ClickInsertarEvento != null)
                OnClickInsertarEvento(new EventArgs());
        }
        /// <summary>
        /// Evento producido al dar click en boton referencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbReferenciasParada_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //Validamos Registros
            if (gvParadas.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvParadas, sender, "imb", false);
                inicializaReferenciaRegistro(gvParadas.SelectedValue.ToString(), "5", ((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            }
        }
        #endregion

        #region Manejador de Eventos


        /// <summary>
        /// Manipula Evento ClickGuardar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickEditar(EventArgs e)
        {
            if (ClickEditar != null)
                ClickEditar(this, e);

        }

        /// <summary>
        /// Manipula Evento ClickAgregarAbajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickAgregarAbajo(EventArgs e)
        {
            if (ClickAgregarAbajo != null)
                ClickAgregarAbajo(this, e);

        }

        /// <summary>
        /// Manipula Evento ClickAgregarArriba
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickAgregarArriba(EventArgs e)
        {
            if (ClickAgregarArriba != null)
                ClickAgregarArriba(this, e);

        }

        /// <summary>
        /// Manipula Evento ClicEliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickEliminar(EventArgs e)
        {
            if (ClickEliminar != null)
                ClickEliminar(this, e);

        }

        /// <summary>
        /// Manipula Evento ClicInsertarEvento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnClickInsertarEvento(EventArgs e)
        {
            if (ClickInsertarEvento != null)
                ClickInsertarEvento(this, e);
        }


        /// <summary>
        /// Evento Generado al dar click en el link de Kilometraje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkKilometraje_Click(object sender, EventArgs e)
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
                lblError.Text = retorno.Mensaje;
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
        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {

            //Cargando Catalogos 
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 10, "");
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoParadas, "", 18);

        }

        /// <summary>
        /// Método encargado de cargar la Paradas
        /// </summary>
        private void cargaParadas()
        {
            //Inicializamos Indices del grid View Paradas
            TSDK.ASP.Controles.InicializaIndices(gvParadas);

            //Validando que Exista un Servicio
            if (this._id_servicio != 0)
            {
                //Obtenemos Paradas
                using (DataTable mit = SAT_CL.Despacho.Parada.CargaParadasParaVisualizacion(this._id_servicio))
                {
                    //Valida Origen de Datos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    {
                        //Cargamos Grid View
                        TSDK.ASP.Controles.CargaGridView(gvParadas, mit, "Id-Secuencia-IdMovimiento", "", false, 0);

                        //Añadiendo Tabla 
                        this._table_Paradas = mit;
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvParadas);

                        //Eliminando Tabla 
                        this._table_Paradas = null;
                    }
                }
            }
            else
            {
                //Inicializando GridView
                TSDK.ASP.Controles.InicializaGridview(gvParadas);

                //Eliminando Tabla 
                this._table_Paradas = null;
            }
            //Actualizamos los datos de las paradas
            cargaDatosParadas();
        }

        /// <summary>
        /// Metodo encargado de cargar los indicadores de las paradas 
        /// </summary>
        private void cargaDatosParadas()
        {
            //Validando que Exista un Servicio
            if (this._id_servicio != 0)
            {
                //Obtenemos Paradas
                using (DataTable mit = SAT_CL.Documentacion.Servicio.CargaDatosParadas(this._id_servicio))
                {
                    //Valida Origen de Datos
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                    {
                        //Recuperamos la informacion de la consulta
                        foreach (DataRow r in mit.Rows)
                        {
                            lblTotalParadas.Text = r["Paradas"].ToString();
                            lblTotalKilometros.Text = r["Kilometraje"].ToString();
                            lblTiempo.Text = r["Transito"].ToString();
                            lblCargas.Text = r["Cargas"].ToString();
                            lblOTCargas.Text = r["CargasOT"].ToString();
                            lblTiempoCarga.Text = r["CargasTiempo"].ToString();
                            lblDescargas.Text = r["Descargas"].ToString();
                            lblOTDescargas.Text = r["DescargasOT"].ToString();
                            lblTiempoDescarga.Text = r["DescargasTiempo"].ToString();
                        }
                    }
                    else
                    {
                        lblTotalParadas.Text = "";
                        lblTotalKilometros.Text = "";
                        lblTiempo.Text = "";
                        lblCargas.Text = "";
                        lblOTCargas.Text = "";
                        lblTiempoCarga.Text = "";
                        lblDescargas.Text = "";
                        lblOTDescargas.Text = "";
                        lblTiempoDescarga.Text = "";
                    }
                }
            }
            else
            {
                lblTotalParadas.Text = "";
                lblTotalKilometros.Text = "";
                lblTiempo.Text = "";
                lblCargas.Text = "";
                lblOTCargas.Text = "";
                lblTiempoCarga.Text = "";
                lblDescargas.Text = "";
                lblOTDescargas.Text = "";
                lblTiempoDescarga.Text = "";
            }
        }

        /// <summary>
        /// Método encargado de cargar Eventos
        /// </summary>
        /// <param name="id_Parada"></param>
        /// <param name="gvEventos"></param>
        private void cargaEventos(int id_Parada, GridView gvEventos)
        {
            //Inicializamos Indices del grid View Paradas
            TSDK.ASP.Controles.InicializaIndices(gvParadas);

            //Oobtenemos Eventos corespondientes a las paradas
            using (DataTable mit = SAT_CL.Despacho.ParadaEvento.CargaEventosParaVisualizacion(id_Parada))
            {
                //Cargamos Grid View
                TSDK.ASP.Controles.CargaGridView(gvEventos, mit, "IdEvento", "", false, 0);

            }
        }
        /// <summary>
        /// Recupera Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdServicio"]) != 0)
                this._id_servicio = Convert.ToInt32(ViewState["IdServicio"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["IdCompania"]) != 0)
                this._id_compania = Convert.ToInt32(ViewState["IdCompania"]);
            if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataTable)ViewState["TableParadas"]))
                this._table_Paradas = (DataTable)ViewState["TableParadas"];
        }

        /// <summary>
        /// Asigna Atributos
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["IdServicio"] = this._id_servicio;
            ViewState["IdCompania"] = this._id_compania;
            ViewState["TableParadas"] = this._table_Paradas;
        }

        /// <summary>
        /// Inicializa Valores
        /// </summary>
        private void inicializaValores()
        {
            chkTipoParada.Checked = false;
            txtUbicacion.Text = "";
            txtCita.Text = "";
            lblError.Text = "";
        }

        /// <summary>
        /// Inicializa Valores Forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga Catalogos
            cargaCatalogos();
            //Carga Pardas
            cargaParadas();

        }
        /// <summary>
        /// Habilita Controles
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    chkTipoParada.Enabled =
                    ddlTipoEvento.Enabled =
                    txtUbicacion.Enabled =
                    txtCita.Enabled =
                    btnGuardar.Enabled =
                    btnAgregarAbajo.Enabled =
                    btnAgregarParadaArriba.Enabled = false;
                    break;
                case Pagina.Estatus.Lectura:
                    chkTipoParada.Enabled =
                    ddlTipoEvento.Enabled =
                    txtUbicacion.Enabled =
                    txtCita.Enabled =
                    btnGuardar.Enabled =
                    btnAgregarAbajo.Enabled =
                    btnAgregarParadaArriba.Enabled = false;
                    break;
                case Pagina.Estatus.Edicion:
                    //Si esta seleccionada una Parada
                    if (gvParadas.SelectedIndex != -1)
                    {
                        chkTipoParada.Enabled =
                        ddlTipoEvento.Enabled =
                        txtUbicacion.Enabled =
                        txtCita.Enabled =
                        btnGuardar.Enabled =
                        btnAgregarAbajo.Enabled =
                        btnAgregarParadaArriba.Enabled = true;
                    }
                    else
                    {
                        chkTipoParada.Enabled =
                        ddlTipoEvento.Enabled =
                        txtUbicacion.Enabled =
                        txtCita.Enabled =
                        btnGuardar.Enabled =
                        btnAgregarAbajo.Enabled =
                        btnAgregarParadaArriba.Enabled = false;
                    }
                    break;
            }

        }

        /// <summary>
        /// Inicializamos Controles
        /// </summary>
        /// <param name="idServicio"></param>
        /// <param name="idCompania"></param>
        public void InicializaControl(int idServicio, int idCompania)
        {
            //Asignando Atributos
            this._id_servicio = idServicio;
            this._id_compania = idCompania;
            //Inicializa Valores
            inicializaValores();
            //Cargamos Paradas
            cargaParadas();
            //habilitamos Controles
            configuraTipoParada();
            habilitaControles();
        }

        /// <summary>
        /// Guarda las Parada Arriba de la parada Seleccionada
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaParadaArriba()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            SAT_CL.Despacho.Parada.TipoParada tipo = chkTipoParada.Checked ? SAT_CL.Despacho.Parada.TipoParada.Servicio : Parada.TipoParada.Operativa;

            //Validamos que exista elemento selecionado
            if (gvParadas.SelectedIndex != -1)
            {
                //Obteniendo Fecha
                DateTime cita;
                DateTime.TryParse(txtCita.Text, out cita);
                
                //Insertamos Parada
                resultado = Parada.NuevaParadaServicio(this._id_servicio, Convert.ToInt32(gvParadas.SelectedDataKey["Secuencia"]) == 1 ? 1 : Convert.ToInt32(gvParadas.SelectedDataKey["Secuencia"]), tipo,
                            Convert.ToInt32(ddlTipoEvento.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1)),
                            SqlGeography.Null, cita, 0, this._id_compania, this._table_Paradas.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Control
                    InicializaControl(this._id_servicio, this._id_compania);

                }
                //Mostramos Mensaje Error
                lblError.Text = resultado.Mensaje;
            }
            return resultado;
        }

        /// <summary>
        /// Guarda las Parada Arriba de la parada Seleccionada
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion GuardaParadaAbajo()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            SAT_CL.Despacho.Parada.TipoParada tipo = chkTipoParada.Checked ? SAT_CL.Despacho.Parada.TipoParada.Servicio : Parada.TipoParada.Operativa;

            //Validamos que exista elemento selecionado
            if (gvParadas.SelectedIndex != -1)
            {
                //Obteniendo Fecha
                DateTime cita;
                DateTime.TryParse(txtCita.Text, out cita);
                
                //Insertamos Parada
                resultado = Parada.NuevaParadaServicio(this._id_servicio, Convert.ToInt32(gvParadas.SelectedDataKey["Secuencia"]) + 1, tipo,
                          Convert.ToInt32(ddlTipoEvento.SelectedValue), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1)),
                            SqlGeography.Null, cita, 0, this._id_compania, this._table_Paradas.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Control
                    InicializaControl(this._id_servicio, this._id_compania);

                }
                //Mostramos Mensaje Error
                lblError.Text = resultado.Mensaje;
            }
            return resultado;
        }

        /// <summary>
        /// Edita las Paradas
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion EditaParada()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            SAT_CL.Despacho.Parada.TipoParada tipo = chkTipoParada.Checked ? SAT_CL.Despacho.Parada.TipoParada.Servicio : Parada.TipoParada.Operativa;

            //Validamos que exista elemento selecionado
            if (gvParadas.SelectedIndex != -1)
            {
                //Instanciamos Parada
                using (Parada objParada = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["Id"])))
                using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1))))
                {
                    if (objParada.habilitar && ubi.habilitar)
                    {
                        //Obteniendo Fecha
                        DateTime cita;
                        DateTime.TryParse(txtCita.Text, out cita);

                        //Validando Estatus
                        if (objParada.Estatus == Parada.EstatusParada.Registrado)
                        {
                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Editamos Parada
                                resultado = objParada.EditaParada(tipo, ubi.id_ubicacion, ubi.geoubicacion, ubi.descripcion, cita, ((Usuario)Session["usuario"]).id_usuario);

                                //Validando Operación
                                if (resultado.OperacionExitosa)
                                {
                                    if (objParada.id_servicio > 0)
                                    {
                                        //Instanciamos Servicio 
                                        using (SAT_CL.Documentacion.Servicio objServicio = new SAT_CL.Documentacion.Servicio(objParada.id_servicio))
                                        {
                                            //Validando Secuencia
                                            if (objParada.secuencia_parada_servicio == 1 && objServicio.habilitar)
                                            
                                                //6.3 Actualizamos la ubicación de Carga del Servicio
                                                resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, ubi.id_ubicacion, cita,
                                                                                      objServicio.id_ubicacion_descarga, objServicio.cita_descarga, objServicio.porte,
                                                                                      objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                       ((Usuario)Session["usuario"]).id_usuario);
                                            
                                            else if (objServicio.habilitar)
                                            {
                                                //Obteniendo Secuencia Maxima
                                                decimal sec_max = 0.00M;
                                                sec_max = (from DataRow dr in this._table_Paradas.Rows
                                                           select Convert.ToDecimal(dr["Secuencia"])).Max();

                                                if (sec_max > 0.00M)
                                                {
                                                    if (sec_max == objParada.secuencia_parada_servicio)
                                                    
                                                        //6.3 Actualizamos la ubicación de Carga del Servicio
                                                        resultado = objServicio.EditarServicio(objServicio.id_cliente_receptor, objServicio.id_ubicacion_carga,
                                                                                             objServicio.cita_carga, ubi.id_ubicacion, cita, objServicio.porte,
                                                                                             objServicio.referencia_cliente, objServicio.observacion_servicio,
                                                                                             ((Usuario)Session["usuario"]).id_usuario);
                                                    
                                                }
                                                else
                                                    resultado = new RetornoOperacion("La parada no es de Servicio");
                                            }
                                            else
                                                resultado = new RetornoOperacion("La parada no es de Servicio");
                                        }
                                    }
                                }

                                //Validando Operación
                                if (resultado.OperacionExitosa)
                                {
                                    resultado = new RetornoOperacion(objParada.id_parada);
                                    scope.Complete();
                                }
                            }
                        }
                        else
                            resultado = new RetornoOperacion("La Parada no esta registrada");
                    }
                    else
                        resultado = new RetornoOperacion("No se puede recuperar la Ubicación de la Parada");

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializamos Control
                        InicializaControl(this._id_servicio, this._id_compania);
                    }
                    //Mostramos Mensaje Error
                    lblError.Text = resultado.Mensaje;
                }//*/

                /*/Instanciando Parada
                using (Parada st = new Parada(Convert.ToInt32(gvParadas.SelectedDataKey["Id"])))
                using (SAT_CL.Global.Ubicacion ubi = new SAT_CL.Global.Ubicacion(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, "ID:", 1))))
                {
                    //Validando que exista
                    if (st.habilitar && ubi.habilitar)
                    {
                        //Obteniendo Fecha
                        DateTime cita;
                        DateTime.TryParse(txtCita.Text, out cita);
                        
                        //Insertando Paradas
                        resultado = Parada.NuevaParadaDocumentacion(st.id_servicio, st.secuencia_parada_servicio, st.Tipo, ubi.id_ubicacion, ubi.geoubicacion, cita,
                                            0, ubi.id_compania_emisor, this._table_Paradas.Rows.Count, ((Usuario)Session["usuario"]).id_usuario);

                        //Validamos Resultado
                        if (resultado.OperacionExitosa)
                        
                            //Inicializamos Control
                            InicializaControl(this._id_servicio, this._id_compania);
                        
                        //Mostramos Mensaje Error
                        lblError.Text = resultado.Mensaje;
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se pueden recuperar los Datos de la Parada");
                }//*/
            }

            //Devolviendo Resultado
            return resultado;
        }

        /// <summary>
        ///Deshabilita Paradas
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion DeshabilitaParada()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que exista elemento selecionado
            if (gvParadas.SelectedIndex != -1)
            {
                //Instanciamos Parada
                using (Parada objParada = new SAT_CL.Despacho.Parada(Convert.ToInt32((gvParadas.SelectedDataKey["Id"]))))
                {
                    //Editamos Parada
                    resultado = objParada.DeshabilitaParadaServicio(((Usuario)Session["usuario"]).id_usuario, this._table_Paradas.Rows.Count);

                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializamos Control
                        InicializaControl(this._id_servicio, this._id_compania);
                    }
                    //Mostramos Mensaje Error
                    lblError.Text = resultado.Mensaje;
                }
            }
            return resultado;

        }

        /// <summary>
        /// Método encvargado de Insertar Evento
        /// </summary>
        public RetornoOperacion InsertaEvento()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validamos Existencia de Paradas
            if (gvParadas.DataKeys.Count > 0)
            {
                //Instanciamos Gid View
                using (GridView gvEventos = (GridView)gvParadas.SelectedRow.FindControl("gvEventos"))
                {
                    //Insertamos Evento
                    resultado = insertaEvento(Convert.ToInt32(gvParadas.SelectedDataKey["Id"]), gvEventos.DataKeys.Count, gvEventos);
                }
            }
            //Devolvemos Resultado
            return resultado;
        }

        /// <summary>
        /// Inserta Evento
        /// </summary>
        /// <param name="id_parada">Id Parada</param>
        /// <param name="totalEventos">Total eventos</param>
        /// <param name="gvEventos">Gv Eventos</param>
        private RetornoOperacion insertaEvento(int id_parada, int totalEventos, GridView gvEventos)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciando Parada
            using (Parada stop = new Parada(id_parada))
            {
                //Validando Tipo de Parada
                if (stop.habilitar && stop.Tipo == Parada.TipoParada.Operativa)
                {
                    //Editamos Evento
                    resultado = ParadaEvento.InsertaParadaEventoEnServicio(this._id_servicio, id_parada, gvEventos.DataKeys.Count, ((Usuario)Session["usuario"]).id_usuario);
                }
                else
                    //Instanciando Excepción
                    resultado = new RetornoOperacion("La Parada es de Servicio, Imposible agregar eventos");
            }

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Eventos
                cargaEventos(id_parada, gvEventos);

                //Habilitamos Controles
                habilitaControles();
                configuraTipoParada();

                //Inicializamos Controles
                inicializaValores();
            }
            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;

            //Devolvemos Resultado
            return resultado;

        }

        /// <summary>
        /// Edita Evento
        /// </summary>
        /// <param name="id_evento"></param>
        /// <param name="gvEventos"></param>
        private void editaEvento(int id_evento, GridView gvEventos)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Evento
            using (ParadaEvento objParadaEvento = new ParadaEvento(id_evento))
            {
                //Editamos Evento
                resultado = objParadaEvento.EditaParadaEventoEnServicio(((Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Eventos
                    cargaEventos(objParadaEvento.id_parada, gvEventos);

                    //Habilitamos Controles
                    habilitaControles();

                    //Inicializamos Controles
                    inicializaValores();
                }
            }
            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;

        }

        /// <summary>
        /// Deshabilita Evento
        /// </summary>
        /// <param name="Id_evento">Id Evento</param>
        /// <param name="TotalEventos">Total Eventos</param>
        /// <param name="gvEventos">gvEnetos</param>
        private void deshabilitaParadaEvento(int Id_evento, int TotalEventos, GridView gvEventos)
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Evento
            using (ParadaEvento objParadaEvento = new ParadaEvento(Id_evento))
            {
                //Editamos Parada
                resultado = objParadaEvento.DeshabilitaParadaEventoEnServicio(((Usuario)Session["usuario"]).id_usuario, TotalEventos);

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Evento
                    cargaEventos(objParadaEvento.id_parada, gvEventos);

                    //Habilitamos Controles
                    habilitaControles();

                    //Inicializamos Controles
                    inicializaValores();
                }
            }
            //Mostramos Mensaje Error
            lblError.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Método encargado de Configurar Tipo de Parada
        /// </summary>
        private void configuraTipoParada()
        {
            //Validando Tipo de Parada
            if (chkTipoParada.Checked)
            {
                //Cargando Catalogos
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 10, "Ninguno");
                //Configurando Controles
                ddlTipoEvento.SelectedValue = "0";
                txtCita.Text = "";
                txtCita.Enabled =
                ddlTipoEvento.Enabled = false;
            }
            else
            {
                //Cargando Catalogos
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 10, "");
                //Configurando Controles
                txtCita.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                txtCita.Enabled =
                ddlTipoEvento.Enabled = true;
            }
        }

        /// <summary>
        /// Configura la ventana de visualización de bitácora del registro solicitado
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucParada.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Configura la ventana de referencias del registro solicitado
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/UserControls/wucParada.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencias", 800, 500, false, false, false, true, true, Page);
        }
        #endregion        
    }
}