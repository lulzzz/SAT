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
    public partial class ReporteEventos : System.Web.UI.Page
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
        /// Cerrar alguna ventana modal
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
                case "EventosParada":
                    //Cerrando ventana de actualziación de eventos
                    alternaVentanaModal("eventosParada", lkbCerrar);
                    //En base al argumento opcional del control, se determina si es requerida la apertura de la ventana de salida
                    if (lkbCerrar.CommandArgument == "Salida")
                        //Abriendo ventana de salida
                        alternaVentanaModal("inicioFinMovimientoServicio", lkbCerrar);
                    break;
            }
        }

        #region Eventos GridView "Eventos"

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventos_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvEventos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 3);
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Eventos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelEventos_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdAccesorio", "IdTipoEvento");

        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden del GV de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEventos_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewEventos.Text = Controles.CambiaSortExpressionGridView(gvEventos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento generado al dar click en la Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacora_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvEventos.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvEventos, sender, "lnk", false);
                //Inicializamos Bitacora
                inicializaBitacora(Convert.ToInt32(gvEventos.SelectedValue), 7, "Eventos");

                Controles.CargaGrafica(ChtEventos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Descripcion", "*Estancia", true);
            }
        }
        /// <summary>
        /// Evento generado al dar click en el Link de Eventos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEventos_Click(object sender, EventArgs e)
        {
            //Validando que el GridView contenga Registros
            if (gvEventos.DataKeys.Count > 0)
            {
                //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvEventos, sender, "lnk", false);

                //Inicializando Control
                wucParadaEvento.InicializaControl(Convert.ToInt32(gvEventos.SelectedDataKey["IdParada"]));

                //Se cierra ventana de eventos
                alternaVentanaModal("eventosParada", this);
            }
        }

        #endregion

        #region Eventos Parada Evento

        /// <summary>
        /// Click en botón Actualizar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnActualizarClick(object sender, EventArgs e)
        {
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.GuardaEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {
                //Obteniendo Evento Seleccionado
                int idEvento = Convert.ToInt32(gvEventos.SelectedDataKey["Id"]);
                
                //Se cierra ventana de eventos
                alternaVentanaModal("eventosParada", this);

                //Actualizando lista de eventos
                cargaEventos();

                //Macando Fila
                Controles.MarcaFila(gvEventos, idEvento.ToString(), "Id", "Id-IdParada", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblCriterioGridViewEventos.Text, Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 3);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón Cancelar edición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnCancelarClick(object sender, EventArgs e)
        {
            //Realizando cancelación de edición de evento
            wucParadaEvento.CancelarActualizacion();

            //Se cierra ventana de eventos
            alternaVentanaModal("eventosParada", this);
        }
        /// <summary>
        /// Click en botón Nuevo Evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnBtnNuevoClick(object sender, EventArgs e)
        {
            //Realizando inserción de nuevo evento
            RetornoOperacion resultado = wucParadaEvento.NuevoEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {
                //Obteniendo Evento Seleccionado
                int idEvento = Convert.ToInt32(gvEventos.SelectedDataKey["Id"]);

                //Actualizando lista de eventos
                cargaEventos();

                //Macando Fila
                Controles.MarcaFila(gvEventos, idEvento.ToString(), "Id", "Id-IdParada", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblCriterioGridViewEventos.Text, Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 3);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón eliminar evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucParadaEvento_OnlkbEliminarClick(object sender, EventArgs e)
        {
            //Realizando actualización de evento
            RetornoOperacion resultado = wucParadaEvento.EliminaEvento();

            //Si se actualizó correctamente
            if (resultado.OperacionExitosa)
            {   
                //Obteniendo Evento Seleccionado
                int idEvento = Convert.ToInt32(gvEventos.SelectedDataKey["Id"]);
                
                //Actualizando lista de eventos
                cargaEventos();

                //Macando Fila
                Controles.MarcaFila(gvEventos, idEvento.ToString(), "Id", "Id-IdParada", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblCriterioGridViewEventos.Text, Convert.ToInt32(ddlTamañoGridViewEventos.SelectedValue), true, 3);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

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
            TSDK.ASP.Controles.InicializaGridview(gvEventos);
        }
        /// <summary>
        /// 
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogos Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "TODOS", 8);
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
            txtNoServicio.Text = "";
            txtUbicacion.Text = "";
            txtFecha.Text = "";
            ddlTipoEvento.SelectedValue = "0";
            ddlEstatus.SelectedValue = "0";
        }

        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacora(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/ReporteEventos.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de cargar las Eventoses
        /// </summary>
        private void cargaEventos()
        {
            //Declaramos variables de Fechas 
            DateTime fecha = DateTime.MinValue;
            //De acuerdo al chek box de fechas de Liquidación
            if (txtFecha.Text != "")
            {
                //Declaramos variables de Fechas de Registró
                fecha = Convert.ToDateTime(txtFecha.Text);
            }

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.Despacho.Reporte.ReporteEventos(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                   Cadena.VerificaCadenaVacia(txtNoServicio.Text, ""),
                                                                   Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUbicacion.Text, ':', 1, "0")), 
                                                                   Convert.ToByte(ddlTipoEvento.SelectedValue), Convert.ToByte(ddlEstatus.SelectedValue),
                                                                   fecha, txtReferencia.Text))
            {

                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvEventos, ds.Tables["Table"], "Id-IdParada", "", true, 3);
                    Controles.CargaGridView(gvResumen, ds.Tables["Table1"], "Descripcion", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"], "Table1");
                    //Carga grafica
                    Controles.CargaGrafica(ChtEventos, "Serie1", "Area1", SeriesChartType.Pie, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                          "Descripcion", "*Estancia", true);
                    //gvResumen.FooterRow.Cells[1].Text = (((DataSet)Session["DS"]).Tables["Table1"].Compute("SUM(*Estancia)", "")).ToString();

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvEventos);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
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
                case "eventosParada":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "eventosParadaModal", "eventosParada");
                    break;
            }
        }

        #endregion
    }
}