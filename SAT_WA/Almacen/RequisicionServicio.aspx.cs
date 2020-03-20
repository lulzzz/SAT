using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Almacen
{
    public partial class RequisicionServicio : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Producirse un Evento en la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)
            {
                //Invocando Método de inicialización
                inicializaPagina();
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Buscar Servicio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarServicio_Click(object sender, EventArgs e)
        {
            //Buscando Servicios
            buscaServicios();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Buscar Requisición"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarRequisicion_Click(object sender, EventArgs e)
        {
            //Buscando Requisiciones
            buscaRequisiciones();
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch(lkb.CommandName)
            {
                case "Servicios":
                    {
                        //Exportando Servicios
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
                        break;
                    }
                case "Requisicion":
                    {
                        //Exportando Requisiciones
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
                        break;
                    }
                case "ServicioRequisiciones":
                    {
                        //Exportando Requisiciones del Servicio
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
                        break;
                    }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Boton "Nueva Requisición"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaRequisicion_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            Controles.InicializaIndices(gvServicios);
            Controles.InicializaIndices(gvRequisiciones);
            
            //Inicializando Control
            wucRequisicion.InicializaRequisicion(0, 0, 0, 0);
            
            //Mostrando ventana de Nueva Requisición
            alternaVentanaModal("AltaRequisicion", btnNuevaRequisicion);
        }
        /// <summary>
        /// Evento Producido al Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Referencias":
                    {
                        //Cerrando Referencias
                        alternaVentanaModal(lkb.CommandName, lkb);

                        //Mostrando Alta de Requisiciones
                        alternaVentanaModal("AltaRequisicion", lkb);
                        break;
                    }
                default:
                    {
                        //Invocando Método de Alternado
                        alternaVentanaModal(lkb.CommandName, lkb);
                        break;
                    }
            }            
        }
        /// <summary>
        /// Evento Producido al Click a los Botones "Aceptar" y "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminacion_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            Button btn = (Button)sender;

            //Validando Comando
            switch (btn.CommandName)
            {
                case "Aceptar":
                    {
                        //Declarando Objeto de Retono
                        RetornoOperacion result = new RetornoOperacion();

                        //Instanciando Requisición
                        using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"])))
                        {
                            //Validando que existe la Requisición
                            if (requisicion.habilitar)

                                //Solicitando Requisición
                                result = requisicion.EliminaServicioRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe la Requisición");
                        }

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Requisición
                            int idRequisicion = result.IdRegistro;

                            //Invocando Métodos de Busqueda
                            buscaRequisiciones();
                            buscaServicios();

                            //Marcando Fila 
                            Controles.MarcaFila(gvRequisiciones, idRequisicion.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), lblOrdenadoRequisicion.Text, Convert.ToInt32(ddlTamanoRequisicion.SelectedValue), true, 2);
                        }

                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion(btn, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Ocultando ventana
                        alternaVentanaModal("ConfirmacionEliminacion", btn);
                        break;
                    }
                case "Cancelar":
                    {
                        //Ocultando ventana
                        alternaVentanaModal("ConfirmacionEliminacion", btn);

                        //Inicializando Indices
                        Controles.InicializaIndices(gvRequisiciones);
                        break;
                    }
            }
        }

        #region Eventos GridView "Requisiciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRequisicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaTamañoPaginaGridView(gvRequisiciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoRequisicion.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisiciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoRequisicion.Text = Controles.CambiaSortExpressionGridView(gvRequisiciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Paginación
            Controles.CambiaIndicePaginaGridView(gvRequisiciones, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisiciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Añadiendo Menu Contextual
                Controles.CreaMenuContextual(e, "menuReqServicio", "menuReqServicioOpciones", "MostrarMenuRequisicion", true, true);

                //Obteniendo Control
                using (LinkButton lkb = (LinkButton)e.Row.FindControl("lkbSolicitar"))
                using (CheckBox chk = (CheckBox)e.Row.FindControl("chkVarios"))
                {
                    //Validando que exista el Control
                    if (lkb != null)
                    {
                        //Validando Estatus
                        if (!row["IdEstatus"].ToString().Equals(""))
                        {
                            //Validando Estatus
                            switch ((SAT_CL.Almacen.Requisicion.Estatus)Convert.ToInt32(row["IdEstatus"]))
                            {
                                case SAT_CL.Almacen.Requisicion.Estatus.Solicitada:
                                    {
                                        //Ocultando Control
                                        lkb.Visible = false;
                                        break;
                                    }
                                case SAT_CL.Almacen.Requisicion.Estatus.Registrada:
                                    {
                                        //Mostrando Control
                                        lkb.Visible = true;
                                        break;
                                    }
                                default:
                                    {
                                        //Ocultando Control
                                        lkb.Visible = false;
                                        break;
                                    }
                            }
                        }
                    }

                    //Validando Control
                    if (chk != null)
                    {
                        //Validando Servicio
                        if (!row["NoServicio"].ToString().Equals(""))
                        {
                            //Validando valor
                            switch (row["NoServicio"].ToString())
                            {
                                case "----":
                                    {
                                        //Mostrando Control
                                        chk.Visible = true;
                                        break;
                                    }
                                default:
                                    {
                                        //Ocultando Control
                                        chk.Visible = false;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Check del GridView "Requisiciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisiciones.DataKeys.Count > 0)
            {
                //Obteniendo Control Disparador
                CheckBox chk = (CheckBox)sender;

                //Validando el Control
                switch (chk.ID)
                {
                    case "chkTodos":
                        {
                            //Seleccionando Fila
                            Controles.SeleccionaFilasTodas(gvRequisiciones, "chkVarios", chk.Checked);
                            break;
                        }
                    case "chkVarios":
                        {
                            //Inicializando Indices
                            Controles.InicializaIndices(gvServicios);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEditarReq_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisiciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisiciones, sender, "lnk", false);

                //Inicializando Control
                wucRequisicion.InicializaRequisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"]), 0, 0, 0);

                //Mostrando Ventana
                alternaVentanaModal("AltaRequisicion", gvRequisiciones);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSolicitar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisiciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisiciones, sender, "lnk", false);

                //Declarando Objeto de Retono
                RetornoOperacion result = new RetornoOperacion();

                //Instanciando Requisición
                using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"])))
                {
                    //Validando que existe la Requisición
                    if (requisicion.habilitar)
                    
                        //Solicitando Requisición
                        result = requisicion.SolicitaRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Requisición");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Requisición
                    int idRequisicion = result.IdRegistro;

                    //Invocando Método de Guardado
                    buscaRequisiciones();

                    //Marcando Fila
                    Controles.MarcaFila(gvRequisiciones, idRequisicion.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), lblOrdenadoRequisicion.Text, Convert.ToInt32(ddlTamanoRequisicion.SelectedValue), true, 2);
                }

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion(gvRequisiciones, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNoServicio_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisiciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisiciones, sender, "lnk", false);

                //Instanciando Requisición
                using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"])))
                {
                    //Validando que existe la Requisición
                    if (requisicion.habilitar && requisicion.id_servicio > 0)
                    {
                        //Instanciando Servicio
                        using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(requisicion.id_servicio))
                        {
                            //Validando que exista el Servicio
                            if (serv.habilitar)
                            {
                                //Mostrando Mensaje de Operación
                                lblMensaje.Text = string.Format("La Requisición '{0}' pertenece al Servicio '{1}' <br />¿Desea eliminar la relación?", requisicion.no_requisicion, serv.no_servicio);

                                //Solicitando Requisición
                                alternaVentanaModal("ConfirmacionEliminacion", (LinkButton)sender);
                            }
                            else
                                //Mostrando Notificación
                                ScriptServer.MuestraNotificacion((LinkButton)sender, "No se puede acceder al Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                    else
                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion((LinkButton)sender, "La Requisición no tiene Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvRequisiciones.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisiciones, sender, "lnk", false);

                //Instanciando Requisición
                using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"])))
                {
                    //Validando que existe la Requisición
                    if (requisicion.habilitar)
                    {
                        //Validando que no exista el Servicio
                        if (requisicion.id_servicio == 0)
                        
                            //Mostrando Notificación
                            ScriptServer.MuestraNotificacion((LinkButton)sender, "Seleccione el Servicio que Desea Requerir", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        else
                        {
                            //Mostrando Notificación
                            ScriptServer.MuestraNotificacion((LinkButton)sender, "La Requisición ya tiene un Servicio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                            //Inicializando Indices
                            Controles.InicializaIndices(gvRequisiciones);
                        }
                    }
                    else
                    {
                        //Mostrando Notificación
                        ScriptServer.MuestraNotificacion((LinkButton)sender, "No Existe la Requisición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

                        //Inicializando Indices
                        Controles.InicializaIndices(gvRequisiciones);
                    }
                }
            }
        }

        #endregion

        #region Eventos GridView "Servicios"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            Controles.CambiaTamañoPaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoServicio.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Paginación
            Controles.CambiaIndicePaginaGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenadoServicio.Text = Controles.CambiaSortExpressionGridView(gvServicios, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRequisicion_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //Cargando Requisiciones Ligadas
                cargaRequisicionesServicio();

                //Mostrando Ventana Modal
                alternaVentanaModal("RequisicionesServicio", (LinkButton)sender);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAgregar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Obteniendo Filas Seleccionadas
                GridViewRow[] gvr = Controles.ObtenerFilasSeleccionadas(gvRequisiciones, "chkVarios");

                //Validando que existan Filas Selecionadas
                if (gvr.Length > 0)
                {
                    //Inicializando Transacción
                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Seleccionando Fila
                        Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                        //Recorriendo Ciclo
                        foreach (GridViewRow gv in gvr)
                        {
                            //Marcando Indice
                            gvRequisiciones.SelectedIndex = gv.RowIndex;

                            //Instanciando Requisición
                            using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisiciones.SelectedDataKey["Id"])))
                            {
                                //Validando si existe la Requisición
                                if (requisicion.habilitar)

                                    //Actualizando Servicio
                                    result = requisicion.AgregaServicioRequisicion(Convert.ToInt32(gvServicios.SelectedDataKey["Id"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                    //Instanciando Excepción
                                    result = new RetornoOperacion("No Existe la Requisición");
                            }

                            //Validando que la Operación no haya sido Exitosa
                            if (!result.OperacionExitosa)

                                //Terminando Ciclo
                                break;
                        }

                        //Validando resultado Final
                        if (result.OperacionExitosa)

                            //Completando Transacción
                            trans.Complete();
                    }
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("Debe Seleccionar al menos una Requisición");

                //Validando Operaciónes Exitosas
                if (result.OperacionExitosa)
                {
                    //Obteniendo Selecciones
                    int idServicio = Convert.ToInt32(gvServicios.SelectedDataKey["Id"]);

                    //Invocando Métodos de Busqueda
                    buscaRequisiciones();
                    buscaServicios();

                    //Marcando Fila
                    Controles.MarcaFila(gvServicios, idServicio.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenadoServicio.Text, Convert.ToInt32(ddlTamanoServicio.SelectedValue), true, 2);
                }

                //Mostrando Resultado de Operación
                ScriptServer.MuestraNotificacion(gvServicios, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Creando Menu Contextual
                Controles.CreaMenuContextual(e, "menuServicio", "menuServicioOpciones", "MostrarMenuServicio", true, true);

                /*/Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Obteniendo Control
                using (LinkButton lkbNueva = (LinkButton)e.Row.FindControl("lkbNuevaReq"),
                        lkbReq = (LinkButton)e.Row.FindControl("lkbRequisicion"))
                {
                    //Validando que exista el Control
                    if (lkbNueva != null && lkbReq != null)
                    {
                        //Validando que no este Vacia
                        if (!row["IdRequisicion"].ToString().Equals(""))
                        {
                            //Validando que exista una Requisición
                            if(Convert.ToInt32(row["IdRequisicion"]) > 0)
                            {
                                //Ocultando Links
                                lkbNueva.Visible =
                                lkbReq.Enabled = false;
                            }
                            else
                            {
                                //Mostrando Links
                                lkbNueva.Visible =
                                lkbReq.Enabled = true;
                            }
                        }
                    }
                }//*/
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNuevaReq_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvServicios.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvServicios, sender, "lnk", false);

                //Inicializando Requisición
                wucRequisicion.InicializaRequisicion(0, 0, 0, Convert.ToInt32(gvServicios.SelectedDataKey["Id"]));

                //Mostrando Ventana de Requisición
                alternaVentanaModal("AltaRequisicion", (LinkButton)sender);
            }
        }

        #endregion

        #region Eventos Alta Requisición

        /// <summary>
        /// Evento encargado de Guardar la Requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucRequisicion_ClickGuardarRequisicion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Requisición
            result = wucRequisicion.GuardaRequisicion();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)
            {
                //Invocando Métodos de Busqueda
                buscaRequisiciones();
                buscaServicios();
            }

            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(wucRequisicion, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Solicitar la Requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucRequisicion_ClickSolicitarRequisicion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Requisición
            result = wucRequisicion.SolicitaRequisicion();

            //Validando Operación Exitosa
            if (result.OperacionExitosa)

                //Invocando Método de Busqueda
                buscaRequisiciones();

            //Mostrando Resultado de Operación
            ScriptServer.MuestraNotificacion(wucRequisicion, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Abrir el Control de Referencias de las Requisiciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucRequisicion_ClickReferenciarRequisicion(object sender, EventArgs e)
        {
            //Validando que existe la Requisición
            if (wucRequisicion.idRequisicion > 0)
            {
                //Inicializando Control de Requisición
                wucReferencias.InicializaControl(wucRequisicion.idRequisicion, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, 138);

                //Ocultando Ventana
                alternaVentanaModal("AltaRequisicion", this);
                //Mostrando Ventana
                alternaVentanaModal("Referencias", this);
            }
            else
                //Mostrando Excepción
                ScriptServer.MuestraNotificacion(this, "No existe la Requisición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #region Eventos Referencias

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferencias_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Guardando Referencias
            wucReferencias.GuardaReferenciaViaje();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucReferencias_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Eliminando Referencias
            wucReferencias.EliminaReferenciaViaje();
        }

        #endregion

        #region Eventos GridView Requisiciones

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoServReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de la Pagina
            Controles.CambiaTamañoPaginaGridView(gvRequisicionesServicio, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoServReq.SelectedValue), true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbConsultarReq_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvRequisicionesServicio.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisicionesServicio, sender, "lnk", false);

                //Inicializando Control
                wucRequisicion.InicializaRequisicion(Convert.ToInt32(gvRequisicionesServicio.SelectedDataKey["Id"]), 0, 0, 0);

                //Mostrando Ventana
                alternaVentanaModal("AltaRequisicion", (LinkButton)sender);

                //Cerrando Ventana
                alternaVentanaModal("RequisicionesServicio", (LinkButton)sender);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminarRelacion_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvRequisicionesServicio.DataKeys.Count > 0)
            {
                //Instanciando Excepción
                RetornoOperacion result = new RetornoOperacion();
                
                //Seleccionando Fila
                Controles.SeleccionaFila(gvRequisicionesServicio, sender, "lnk", false);

                //Instanciando Requisición
                using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisicionesServicio.SelectedDataKey["Id"])))
                {
                    //Validando que existe la Requisición
                    if (requisicion.habilitar)

                        //Solicitando Requisición
                        result = requisicion.EliminaServicioRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("No existe la Requisición");
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Requisición
                    int idRequisicion = result.IdRegistro;
                    int idServicio = Convert.ToInt32(gvServicios.SelectedDataKey["Id"]);

                    //Invocando Métodos de Busqueda
                    cargaRequisicionesServicio();
                    buscaRequisiciones();
                    buscaServicios();
                    

                    //Marcando Fila 
                    Controles.MarcaFila(gvServicios, idServicio.ToString(), "Id", "Id", OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), lblOrdenadoServicio.Text, Convert.ToInt32(ddlTamanoServicio.SelectedValue), true, 2);
                }

                //Mostrando Notificación
                ScriptServer.MuestraNotificacion((LinkButton)sender, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionesServicio_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Mostrando Expresión
            lblOrdenadoRequisicion.Text = Controles.CambiaSortExpressionGridView(gvRequisicionesServicio, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionesServicio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvRequisicionesServicio, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex, true, 2);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Cargando Catalogos
            cargaCatalogos();

            //Inicializando GridView
            Controles.InicializaGridview(gvServicios);
            Controles.InicializaGridview(gvRequisiciones);
            Controles.InicializaGridview(gvRequisicionesServicio);

            //Inicializando Fechas
            txtFecIniReq.Text =
            txtFecIniServ.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFinReq.Text =
            txtFecFinServ.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando Controles
            chkIncluirReq.Checked =
            chkIncluirServ.Checked = true;
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando estatus de servicio más la opción todos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusServicio, "Todos", 6);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatusRequisicion, "Todos", 2126);
            
            //Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServicio, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRequisicion, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoServReq, "", 26);
        }
        /// <summary>
        /// Método encargado de Buscar los Servicios
        /// </summary>
        private void buscaServicios()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_car = DateTime.MinValue;
            DateTime fec_fin_car = DateTime.MinValue;
            DateTime fec_ini_des = DateTime.MinValue;
            DateTime fec_fin_des = DateTime.MinValue;

            //Validando si se Requieren las Fechas
            if (chkIncluirServ.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbCarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIniServ.Text, out fec_ini_car);
                    DateTime.TryParse(txtFecFinServ.Text, out fec_fin_car);
                }
                else if (rbDescarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIniServ.Text, out fec_ini_des);
                    DateTime.TryParse(txtFecFinServ.Text, out fec_fin_des);
                }
            }
            
            //Obteniendo Servicios
            using (DataTable dtServicios = SAT_CL.Documentacion.Reportes.ObtieneServiciosRequisicion(
                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            txtNoServicio.Text, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), 
                                            Convert.ToByte(ddlEstatusServicio.SelectedValue), fec_ini_car, fec_fin_car, fec_ini_des, fec_fin_des))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(dtServicios))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvServicios, dtServicios, "Id", lblOrdenadoServicio.Text, true, 2);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServicios, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvServicios);

                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvServicios);
            }
        }
        /// <summary>
        /// Método encargado de Buscar las Requisiciones
        /// </summary>
        private void buscaRequisiciones()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_sol = DateTime.MinValue;
            DateTime fec_fin_sol = DateTime.MinValue;
            DateTime fec_ini_ent = DateTime.MinValue;
            DateTime fec_fin_ent = DateTime.MinValue;

            //Validando si se Requieren las Fechas
            if (chkIncluirReq.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbSolicitud.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIniReq.Text, out fec_ini_sol);
                    DateTime.TryParse(txtFecFinReq.Text, out fec_fin_sol);
                }
                else if (rbEntrega.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIniReq.Text, out fec_ini_ent);
                    DateTime.TryParse(txtFecFinReq.Text, out fec_fin_ent);
                }
            }
            
            //Obteniendo Servicios
            using (DataTable dtRequisiciones = SAT_CL.Almacen.Reportes.ObtieneRequisiciones(
                                            ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            txtNoRequisicion.Text,
                                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1)),                                            
                                            Convert.ToByte(ddlEstatusRequisicion.SelectedValue), fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent))
            {
                //Validando Registros
                if (Validacion.ValidaOrigenDatos(dtRequisiciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvRequisiciones, dtRequisiciones, "Id", lblOrdenadoServicio.Text, true, 2);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtRequisiciones, "Table1");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvRequisiciones);

                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvRequisiciones);
            }
        }
        /// <summary>
        /// Método encargado de Alternar la Ventana Modal
        /// </summary>
        /// <param name="nombre_ventana"></param>
        /// <param name="control"></param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "AltaRequisicion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaAltaRequisicion", "ventanaAltaRequisicion");
                    break;
                case "Referencias":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaReferencias", "ventanaReferencias");
                    break;
                case "ConfirmacionEliminacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaConfirmacionEliminaServicioReq", "ventanaConfirmacionEliminaServicioReq");
                    break;
                case "RequisicionesServicio":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaRequisiciones", "ventanaRequisiciones");
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Cargar las Requisiciones de los Servicios
        /// </summary>
        private void cargaRequisicionesServicio()
        {
            //Obteniendo Requisiciones
            using (DataTable dtRequisiciones = SAT_CL.Almacen.Requisicion.ObtieneRequisicionesServicio(Convert.ToInt32(gvServicios.SelectedDataKey["Id"])))
            {
                //Validando que existan Registros
                if (Validacion.ValidaOrigenDatos(dtRequisiciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvRequisicionesServicio, dtRequisiciones, "Id", lblOrdenadoRequisicion.Text, true, 2);

                    //Añadiendo a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtRequisiciones, "Table2");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvRequisicionesServicio);

                    //Eliminando de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }

                //Inicializando Indices
                Controles.InicializaIndices(gvRequisicionesServicio);
            }
        }

        #endregion
    }
}