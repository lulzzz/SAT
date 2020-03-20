using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using SAT_CL.Llantas;
using TSDK.Base;

namespace SAT.LLantas
{
    public partial class AdministracionLlantas : System.Web.UI.Page
    {
        /// <summary>
        /// Evento generado al Cargar la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no es una recraga de página
            if (!this.IsPostBack)
                inicializaForma();
        }

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Forma
        /// </summary>
        private void inicializaForma()
        {


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
                case "montarLlanta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionContenedorMontarLLanta", "seleccionMontarLLanta");
                    break;
                case "desmontarLlanta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "seleccionContenedorDesmontarLLanta", "seleccionDesmontarLLanta");
                    break;
                case "LlantasDesmontadas":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorLLantasDesmontadas", "LlantasDesmontadas");
                    break;
            }
        }

        /// <summary>
        /// Método encargado al seleccionar la Opción Montar Llanta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaMontar_Click(object sender, EventArgs e)
        {
            //Determinando el comando a realziar
            switch (((Button)sender).CommandName)
            {
                case "Desmontadas":
                    //Abrimos Ventana Modal
                    alternaVentanaModal("LlantasDesmontadas", btnDesmontada);
                    //Cerrar Ventana Modal
                    alternaVentanaModal("montarLlanta", btnDesmontada);
                    break;

            }
        }

        /// <summary>
        /// Método encargado al seleccionar la Opción Montar Llanta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaDesmontar_Click(object sender, EventArgs e)
        {
            //Abrimos  Modal de Acuerdo a la Opción
        }
        #endregion

        #region Eventos

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

                case "MontarLlanta":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("montarLlanta", lkbCerrar);
                    break;
                case "DesmontarLlanta":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("desmontarLlanta", lkbCerrar);
                    break;
                case "LlantasDesmontadas":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("LlantasDesmontadas", lkbCerrar);
                    break;

            }
        }

        #endregion

        #region Métodos Posición


        /// <summary>
        /// Método encargado de cargar la Posición
        /// </summary>
        private void cargaPosicion()
        {
            //Obtenemos Depósito
            using (DataTable mitPosicion = LlantaPosicion.CargaPosicionesUnidad(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtUnidad.Text, ':', 1))))
            {

                // Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(mitPosicion))
                {
                    // Cargando los GridView
                    Controles.CargaGridView(gvPosicion, mitPosicion, "Id", "", false, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mitPosicion, "Table");

                }
                else
                {   //Inicializando GridViews
                    Controles.InicializaGridview(gvPosicion);


                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Método encargado de Cargar las Posiciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargaPosicion();
        }

        #endregion

        #region Eventos Posición

        /// <summary>
        /// Evento corting de gridview de Posicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPosicion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvPosicion.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table").DefaultView.Sort = lblOrdenadoPosicion.Text;
                //Cambiando Ordenamiento
                lblOrdenadoPosicion.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPosicion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de Posicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPosicion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPosicion, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de Posicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarPosicion_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
        }

        /// <summary>
        /// Click en algún Link de GV de Posicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionPosición_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvPosicion.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvPosicion, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Montar":
                        //Mostramos Opciones para Montar la Llanta
                        alternaVentanaModal("montarLlanta", gvPosicion);
                        break;
                    case "Desmontar":
                        //Mostramos Opciones para Desmontar la Llanta
                        alternaVentanaModal("desmontarLlanta", gvPosicion);
                        break;
                }
            }
        }

        /// <summary>
        /// Inicializa los Controles del GV Posicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPosicion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:
                    //Creamos instancias del tipo LinkButton correspondientes a Deshabilitar y Guardar
                    using (LinkButton lnkEditar = (LinkButton)fila.FindControl("lnkEditar"),
                      lnkBitacora = (LinkButton)fila.FindControl("lnkBitacora"),
                      lnkDeshabilitar = (LinkButton)fila.FindControl("lnkDeshabilitar"))
                    {
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            //Habilitamos campos de acuerdo al estatus de la pagina
                            case Pagina.Estatus.Nuevo:
                            case Pagina.Estatus.Copia:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                }
                                break;
                            case Pagina.Estatus.Lectura:
                                {
                                    // Deshabilitamos LinkButton´s
                                    lnkEditar.Enabled =
                                    lnkBitacora.Enabled =
                                    lnkDeshabilitar.Enabled = false;
                                    break;
                                }
                            case Pagina.Estatus.Edicion:
                                {
                                    //Si la fila no esta en modo de edicion
                                    if (gvPosicion.EditIndex == -1)
                                    {
                                        lnkEditar.Enabled =
                                        lnkDeshabilitar.Enabled =
                                        lnkBitacora.Enabled = true;
                                    }
                                    break;
                                }
                        }
                    }
                    break;
                //Fila Tipo Footer para Obtener los datos
                case DataControlRowType.Footer:
                    {
                        //Creamos Instancias de Tipo TextBox y DropDownList
                        using (TextBox txtlitros = (TextBox)fila.FindControl("txtlitros"))
                        {
                            using (DropDownList ddlTipoOperacion = (DropDownList)fila.FindControl("ddlTipoOperacion"),
                                                ddlEstacion = (DropDownList)fila.FindControl("ddlEstacion"))
                            {
                                using (LinkButton lnkInsertar = (LinkButton)fila.FindControl("lnkInsertar"))
                                {
                                    //Validando Estatus de la Pagina
                                    switch ((Pagina.Estatus)Session["estatus"])
                                    {
                                        case Pagina.Estatus.Nuevo:
                                            //case Pagina.Estatus.Copiar:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                                ddlEstacion.Enabled =
                                                txtlitros.Enabled =
                                                ddlTipoOperacion.Enabled = false;
                                            }
                                            break;
                                        case Pagina.Estatus.Lectura:
                                            {
                                                //Deshabilitamos controles
                                                lnkInsertar.Enabled =
                                              ddlEstacion.Enabled =
                                              txtlitros.Enabled =
                                              ddlTipoOperacion.Enabled = false;

                                            }
                                            break;
                                        case Pagina.Estatus.Edicion:
                                            {
                                                //Habilitamos controles 
                                                lnkInsertar.Enabled =
                                                ddlTipoOperacion.Enabled =
                                                txtlitros.Enabled = true;
                                                ddlEstacion.Enabled = true;
                                                //Si la fila esta en modo de Edicion
                                                if (gvPosicion.EditIndex != -1)
                                                {
                                                    //Deshabilitamos controles 
                                                    lnkInsertar.Enabled =
                                                    ddlEstacion.Enabled =
                                                    txtlitros.Enabled =
                                                ddlTipoOperacion.Enabled = false;
                                                }
                                            }
                                            break;
                                    }
                                    //cargando catalogo de estaciones
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlEstacion, 20, "Ninguna", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

                                    //cargando catalogo Tipo Operacion
                                    SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoOperacion, "", 3167);

                                }
                            }
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Eventos Llantas Desmontadas
        /// <summary>
        /// Evento corting de gridview de LlantasDesmontadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLlantasDesmontadas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvLlantasDesmontadas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView.Sort = lblOrdenadoLlantasDesmontadas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoLlantasDesmontadas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvLlantasDesmontadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento cambio de página en gridview de LlantasDesmontadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLlantasDesmontadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvLlantasDesmontadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento click en botón de exportación de LlantasDesmontadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarLlantasDesmontadas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
        }

        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoLlantasDesmontadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvLlantasDesmontadas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoLLantasDesmontadas), true, 1);
        }
        #endregion

        #region Métodos Llantas Desmontadas
        #endregion 
    }
}