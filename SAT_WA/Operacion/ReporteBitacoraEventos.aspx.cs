using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;


namespace SAT.Operacion
{
    public partial class ReporteBitacoraEventos : System.Web.UI.Page
    {
        
        #region Eventos
        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando  PostBack
            if (!Page.IsPostBack)
            {
                //Inicializando  la forma
                inicializaForma();
                //Asignamos Focus
                //.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }

        }
        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraEventos_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Permite cambiar de indice de pagina acorde al tamaño del gridview
            Controles.CambiaIndicePaginaGridView(gvBitacoraEventos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 5);
            //Invoca al método cargaMontosManiobra
            cargaMontosManiobra();
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraEventos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia el rango de valores visibles en el gridview (25-50-75-100 registros por vista)
            Controles.CambiaTamañoPaginaGridView(gvBitacoraEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 5);
            //Invoca al método cargaMontosManiobra
            cargaMontosManiobra();
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelEventos_Onclick(object sender, EventArgs e)
        {
            //Invoca al metodoq ue permite exportar el gridview a formato de excel.
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdServicio", "IdMovimiento");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacoraEventos_Onsorting(object sender, GridViewSortEventArgs e)
        {
            //Permite el ordenamiento de las columnas de manera ascendente o descendente
            lblCriterioGridViewEventos.Text = Controles.CambiaSortExpressionGridView(gvBitacoraEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 5);
            //Inonvoca al método cargaMontosManiobras
            cargaMontosManiobra();
        }
        
        /// <summary>
        /// Evento generado al buscar las Eventoses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
           //Carga las Eventos
            cargaEventos();
        }

        /// <summary>
        /// Evento que permite habilitar o deshabilitar los controles de fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCita_CheckedChanged(object sender, EventArgs e)
        {
            //Valida si el check box esta activado
            if (chkCita.Checked == true)
            {
                //Habilita los controles de fecha.
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = true;
                //Asigna valores de fecha a los controles.
                txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
                txtFechaInicio.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            }
            //En caso de que no se cumpla 
            else
            {
                //Limpia las cajas de texto de fecha
                txtFechaInicio.Text = "";
                txtFechaFin.Text = "";
                //Deshabilita las cajas de texto.
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = false;
            }
        }
        /// <summary>
        /// Evento que permite cerrar una ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarEventoModal_Click(object sender, EventArgs e)
        {
            //Invoca al método que eprmite abrir una ventana modal.
            ScriptServer.AlternarVentana(lkbCerrarEventoModal, "CerrarVentana", "contenedorEventoModal", "EventoModal");
        }
        /// <summary>
        /// Evento que permite abrir una ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerEvento_Click(object sender, EventArgs e)
        {
            //Valida si existen datos en el gridview
            if (gvBitacoraEventos.DataKeys.Count > 0)
            {
                //Selecciona la fila del gridview a la cual se le dio clic en el link
                Controles.SeleccionaFila(gvBitacoraEventos, sender, "lnk", false);
                //Invoca al método de parada 
                using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvBitacoraEventos.SelectedDataKey["IdMovimiento"])))
                {
                    //Invoca e Inicializa los valores del control de usuario ParadaEvento
                     wucParadaEvento.InicializaControl(mov.id_parada_destino);
                    //Abre la ventana modal 
                    ScriptServer.AlternarVentana(upgvBitacoraEventos, gvBitacoraEventos.GetType(), "AbrirVentana", "contenedorEventoModal", "EventoModal");
                }
            }            
        }
        /// <summary>
        /// Evento que permite actualizar los datos de un evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_BtnActualizar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignación de valores al objeto retorno
            retorno = wucParadaEvento.GuardaEvento();

            //Valida resultado de la operación
            if (retorno.OperacionExitosa)
                //Cargando eventos en caso de éxito
                cargaEventosManteniendoSeleccion();

            //Notificación de que se ha realizado correctamente o no la acción de actualizar
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento que permite crear nuevos eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_BtnNuevo_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignación de valores al objeto retorno
            retorno = wucParadaEvento.NuevoEvento(); 

            //Valida resultado de la operación
            if (retorno.OperacionExitosa)
                //Cargando eventos en caso de éxito
                cargaEventosManteniendoSeleccion();

            //Notificaciones de que se realizo correctamente o no la creación de un evento          
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);            
        }
        /// <summary>
        /// Evento que permite regresar a la vista principal del control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_BtnCancelar_Click(object sender, EventArgs e)
        {
           //Instancia a la clase movimiento
            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvBitacoraEventos.SelectedDataKey["IdMovimiento"])))
                //Inicializa el control de usuaio ParadaEvento
                wucParadaEvento.InicializaControl(mov.id_parada_destino);
        }
        /// <summary>
        /// Evento que permite eliminar un evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_LkbEliminar_Click(object sender, EventArgs e)
        {
            //Creación del objeto retorno
            RetornoOperacion retorno = new RetornoOperacion();
            //Asignación de valores al objeto retorno
            retorno = wucParadaEvento.EliminaEvento();

            //Valida resultado de la operación
            if (retorno.OperacionExitosa)
                //Cargando eventos en caso de éxito
                cargaEventosManteniendoSeleccion();

            //Notificación de que se realizo correctamente o no la eliminación de un evento
            ScriptServer.MuestraNotificacion(this, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }        

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga controles
            cargaCatalogos();
            //Inicializa Controles
            inicializaControles();
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvBitacoraEventos);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewEventos, "", 26);
            //Cargando Catalogos Tipo Evento
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEvento, 56, "TODOS");

        }

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles
        /// </summary>
        private void inicializaControles()
        {
            txtCliente.Text = "";
            txtNoServicio.Text = "";
            txtReferencia.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtFechaInicio.Enabled =
            txtFechaFin.Enabled = false;
            ddlTipoEvento.SelectedValue = "0";
            chkEstatus.Checked = true;
            chkEstatusTerminado.Checked = true;
            chkCita.Checked = false;
        }

        /// <summary>
        /// Método encargado de cargar los valores a un gridview.
        /// </summary>
        private void cargaEventos()
        {
            //Inicializando Indices
            Controles.InicializaIndices(gvBitacoraEventos);

            //Declara e inicio variables de Fechas 
            DateTime fechaInicio= DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            //Valida si el control fecha_inicio no tiene valor de fecha
            if (txtFechaInicio.Text != "")
            {
                //convierte los valores a tipo datetime
                DateTime.TryParse(txtFechaInicio.Text, out fechaInicio);
                DateTime.TryParse(txtFechaFin.Text, out fechaFin);
            }

            //Obtiene resuktado de la consulta a base de datos y lo almasena en el dataset
            using (DataTable  mit = SAT_CL.Despacho.Reporte.ReporteBitacoraEvento(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,                                                                   
                                                                                 Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text,':',1,"0")), txtNoServicio.Text,txtReferencia.Text,
                                                                                 Convert.ToByte(chkEstatus.Checked),Convert.ToByte(chkEstatusTerminado.Checked),
                                                                                fechaInicio,fechaFin,Convert.ToByte(ddlTipoEvento.SelectedValue)
                                                                                ))
            {
                //Cargando los GridView
                Controles.CargaGridView(gvBitacoraEventos, mit, "IdServicio-IdMovimiento", "", true, 5);

                //Validando que el DataSet contenga las tablas
                if (mit != null) 
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                //En caso contrario
                else
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }
            //Invoca al método cargaMontoManiobra
            cargaMontosManiobra();
        }
        /// <summary>
        /// Carga la lista de eventos que coincida con los criterios de búsqueda y mantiene la selección actual del GV
        /// </summary>
        private void cargaEventosManteniendoSeleccion()
        { 
            //Recuperando selección del GV
            string id_movimiento = gvBitacoraEventos.SelectedIndex != -1 ? gvBitacoraEventos.SelectedDataKey["IdMovimiento"].ToString() : "";

            //Cargando eventos
            cargaEventos();

            //Si habia un elemento seleccionado previamente
            if (id_movimiento != "")
                //Marcando de nueva cuenta el registro
                TSDK.ASP.Controles.MarcaFila(gvBitacoraEventos, id_movimiento, "IdMovimiento", "IdServicio-IdMovimiento", 
                                    TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblCriterioGridViewEventos.Text, 
                                    Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 5);
        }
        /// <summary>
        /// Método que realiza la suma de los montos de maniobra 
        /// </summary>
        private void cargaMontosManiobra()
        {
            //Recupera los valores del datatable y valida si existen registros o no.
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Agrega al pie de pagina del gridview en la columna 39 la suma de los montos de maniobra
                gvBitacoraEventos.FooterRow.Cells[39].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalManiobras)", "")));
            }
            //En caso de que no existan valores recuperados del datatable
            else
            {
                //Carga con valor 0 el pie del gridview
                gvBitacoraEventos.FooterRow.Cells[39].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion












        
    }
}