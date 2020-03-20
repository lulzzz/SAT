using SAT_CL.Mantenimiento;
using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Mantenimiento
{
    public partial class ActividadesTaller : System.Web.UI.Page
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
                //inicializa Pagina
                inicializaPagina();
            }

        }
        /// <summary>
        /// Método encargado de Buscar las Actividades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargaActividadesPorAsignar();
            //Inicializa Grid View Asignaciones
            Controles.InicializaGridview(gvAsignaciones);
        }
        /// <summary>
        /// Evento generado al Cambiar el Area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cargando Catalogo Sub Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubArea, "TODOS", 1122, Convert.ToInt32(ddlArea.SelectedValue));
        }
        /// <summary>
        /// Evento generado al Cerrar las Asignaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAsignacion_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarAsigancion, uplkbCerrarAsigancion.GetType(), "CerrarVentanaModal", "contenidoAsignacion",
                "confirmacionAsignacion");

            //Cargamos Asignaciones
            cargaAsignaciones();

            //Inicializamos Indices
            Controles.InicializaIndices(gvAsignaciones);

        }
        /// <summary>
        /// Evento generado al Cambiar el Check de Terminado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTerminada_CheckedChanged(object sender, EventArgs e)
        {
            //Habilitamos Tex Box de Fechas
            txtFechaFinOrdenTrabajo.Enabled = txtFechaInicioOrdenTrabajo.Enabled = chkTerminada.Checked;

        }

        /// <summary>
        /// Evento generado al Cerrar la Fecha de Inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarFechaInicio_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaInicio, uplkbCerrarFechaInicio.GetType(), "MostrarVentanaModal", "contenidoAsignacion",
                "confirmacionAsignacion");

            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaInicio, uplkbCerrarFechaInicio.GetType(), "AbrirVentanaModal", "contenidoFechaInicio",
                "confirmacionFechaInicio");
        }
        /// <summary>
        /// Evento generado al Cerrar la Fecha de Fin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarFechaTerminar_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaTerminar, uplkbCerrarFechaTerminar.GetType(), "Asignacion", "contenidoAsignacion",
                "confirmacionAsignacion");
            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaTerminar, uplkbCerrarFechaTerminar.GetType(), "TerminarAsignacion", "contenidoFechaTerminarAsignacion",
                "confirmacionFechaTerminarAsignacion");

        }
        /// <summary>
        /// Evento generado al dar click en el link cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarFechaTerminarActividad_Click(object sender, EventArgs e)
        {
            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarFechaTerminarActividad, uplkbCerrarFechaTerminarActividad.GetType(), "MostrarVentanaModal", "contenidoFechaTerminarActividad",
                "confirmacionFechaTerminarActividad");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lkb = (LinkButton)sender;

            //Validando Comando
            switch (lkb.CommandName)
            {
                case "Requisicion":
                    {
                        //Alternando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(lkb, "VentanaRequisicion", "contenidoVentanaRequisicion", "ventanaRequisicion");

                        //Cargando Requisiciones
                        cargaRequisiciones();

                        //Inicializamos Indices 
                        Controles.InicializaIndices(gvRequisicion);
                        break;
                    }
                case "Almacen":
                    {
                        //Alternando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(lkb, "VentanaAlmacen", "contenedorVentanaIngresoAlmacen", "ventanaIngresoAlmacen");
                        break;
                    }
                     case "inicioAsignacionDirecto":
                    {
                        //Alternando Ventana
                        alternaVentanaModal("InicioAsignacionDirecto", lkb);
                        break;
                    }
                      case "terminoAsignacionDirecto":
                    {
                        //Alternando Ventana
                        alternaVentanaModal("TerminoAsignacionDirecto", lkb);
                        break;
                    }

            }
        }

        #region Eventos "Actividades"

        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminarActividad_Click(object sender, EventArgs e)
        {
            //Cerar Ventana Modal
            ScriptServer.AlternarVentana(upbtnCancelarEliminarActividad, upbtnCancelarEliminarActividad.GetType(), "MostrarVentanaModal", "contenidoConfirmacionEliminarActividad",
                "confirmacionEliminarActividad");
        }
        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelarActividad_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarCancelarActividad, upbtnCancelarCancelarActividad.GetType(), "MostrarVentanaModal", "contenidoConfirmacionCancelarActividad",
                "confirmacionCancelarActividad");
        }
        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarTerminarActividad_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnTerminarActividad, upbtnTerminarActividad.GetType(), "TerminarActividad", "contenidoFechaTerminarActividad",
                "confirmacionFechaTerminarActividad");
        }
        /// <summary>
        /// Evento generado al Terminar la actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTerminarActividad_Click(object sender, EventArgs e)
        {
            //Terminar Actividad
            terminarActividad();
        }
        /// <summary>
        /// Evento geenrado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelarActividad_Click(object sender, EventArgs e)
        {
            //Evento generado al cancelar una Actividad
            cancelaActividad();
        }

        /// <summary>
        /// Evento geenrado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTerminarActividad_Click(object sender, EventArgs e)
        {
            //Terminamos Actividad
            terminarActividad();
        }
        /// <summary>
        /// Evento geenrado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarActividad_Click(object sender, EventArgs e)
        {
            //Eliminacion de Actvidad
            eliminarActividad();
        }

        #region Eventos GridView "Actividades"

        /// <summary>
        /// Evento generado al Abrir la Actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAbrirActividad_Click(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos Datos GV
            if (gvActividades.DataKeys.Count > 0)
            {
               
                //Instanciamos Orden Trabajo Actividad
                using (OrdenTrabajoActividad objOrdenTrabajoActividad = new OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
                {
                    //Abrir Actividad
                    resultado = objOrdenTrabajoActividad.AbrirActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Cargamos Actividades npor Asignar
                        cargaActividadesPorAsignar();
                        //Cerramos Ventana Modal
                        alternaVentanaModal("AbrirActividad", btnAceptarAbrirActividad);
                    }
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(btnAceptarAbrirActividad, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                }
            }
        }

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvActividades, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvActividades, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewActividades.SelectedValue));
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Documentos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelActividades_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden delGV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActividades_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewActividades.Text = Controles.CambiaSortExpressionGridView(gvActividades, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }

        /// <summary>
        /// Evento generado al dar click en Eliminar req
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbEliminarReq_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvRequisicion, sender, "lnk", false);
            //Mostramos Ventana
            alternaVentanaModal("EliminarReq", gvRequisicion);
        }
        /// <summary>
        /// Evento generado al Eliminar la Requisicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarReq_Click(object sender, EventArgs e)
        {
            //Validamos que existan Registros
            if(gvRequisicion.DataKeys.Count > 0)
            {
                //Declaramos Objeto Retorno
                RetornoOperacion resultado = new RetornoOperacion();

                //Deshabilitamos Liga de la Requisicion y la Orden
                resultado = deshabilitaRequisicion();

                //Validamos Resultado
                if (resultado.OperacionExitosa)
                    //Cerramos Ventana
                    alternaVentanaModal("EliminarReq", gvRequisicion);

                //Mostrando Mensaje de Operación
                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Evento generado al pulsar el link de Requisiciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRequisiciones_Click(object sender, EventArgs e)
        {
            //Validando que Existan los Registros
            if (gvActividades.DataKeys.Count > 0)
            {
                //Obteniendo Link
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvActividades, sender, "lnk", false);

                //Validando Comando
                switch (lkb.CommandName)
                {
                    case "Requisiciones":
                        {
                            //Cargando Requisiciones
                            cargaRequisiciones();

                            //Inicializando Indices
                            Controles.InicializaIndices(gvRequisicion);
                            break;
                        }
                    case "Agregar":
                        {
                            //Mostrando Ventana
                            ScriptServer.AlternarVentana(lkb, "IngresoAlmacen", "contenedorVentanaIngresoAlmacen", "ventanaIngresoAlmacen");

                            //Limpiando Controles
                            txtAlmacen.Text = "";
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Evento generado al Crear una Nueva Requisicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbNuevaRequisicion_Click(object sender, EventArgs e)
        {
            //Limpiamos Control
            txtAlmacenReq.Text = "";
            //Mostramos Ventana Modal
            alternaVentanaModal("AlmacenReq", lnkNuevaRequisicion);
        }
        /// <summary>
        /// Evento generado al pulsar el link de Actividadse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbActividades_Click(object sender, EventArgs e)
        {           
            //Validamos Datos GV
            if (gvActividades.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvActividades, sender, "lnk", false);
                //Validando estatus de Página
                switch (((LinkButton)sender).CommandName)
                {
                     case "Asignacion":
                        {
                            //Mostrar Ventana Modal
                            alternaVentanaModal("Asignacion", gvActividades);

                            //Inicializamos Control
                            wucAsignacionActividad.InicializaAsignacionActividad(0, Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), Convert.ToInt32(gvActividades.SelectedDataKey["IdActividad"]),1);
                            break;
                        }
                    case "Eliminar":
                        {
                            //Mostrar Ventana Modal
                            ScriptServer.AlternarVentana(gvActividades, upgvActividades.GetType(), "MostrarVentanaModal", "contenidoConfirmacionEliminarActividad",
                                "confirmacionEliminarActividad");
                            break;
                        }
                    case "Terminar":
                        {
                            //Limpiamos Control
                            txtFechaTerminarActividad.Text = "";
                            //Mostrar Ventana Modal
                            ScriptServer.AlternarVentana(gvActividades, upgvActividades.GetType(), "MostrarVentanaModal", "contenidoFechaTerminarActividad",
                                "confirmacionFechaTerminarActividad");
                            break;
                        }
                    case "Cancelar":
                        {
                            //Mostrar Ventana Modal
                            ScriptServer.AlternarVentana(gvActividades, upgvActividades.GetType(), "MostrarVentanaModal", "contenidoConfirmacionCancelarActividad",
                                "confirmacionCancelarActividad");
                            break;
                        }
                    case "AgregarActividad":
                        {
                            //Mostrar Ventana Modal
                            alternaVentanaModal("Actividades", gvActividades);

                            //Inicializando Control
                            ucActividadOrdenTrabajo.InicializaControl(0,Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), 0);

                            break;
                        }
                    case "AbrirActividad":
                        {

                            //Mostrar Ventana Modal
                            alternaVentanaModal("AbrirActividad", gvActividades);

                            break;
                        }
                       
                }
                //Carga Asignaciones y Requisiciones
                cargaAsignaciones();
                cargaRequisiciones();
            }
        }
        /// <summary>
        /// Evento Producido al pulsar el link de Orden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkOrden_Click(object sender, EventArgs e)
        {
            //Validamos Datos GV
            if (gvActividades.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvActividades, sender, "lnk", false);

                //Carga Asignaciones
                cargaAsignaciones();

                //Cargando Requisiciones
                cargaRequisiciones();

                //Activiamos View Index
                mtvRequisicion.ActiveViewIndex = 0;
            }
        }

        #endregion

        #endregion

        #region Eventos "Requisiciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarReq_Click(object sender, EventArgs e)
        {
            
            //Declarando Variables Auxiliares
            int idRequisicion = 0;
            int idAlmacen = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1));

            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando Actividad y Requisición
                using (SAT_CL.Mantenimiento.OrdenTrabajoActividad orden_actividad = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
                using (SAT_CL.Mantenimiento.Actividad actividad = new SAT_CL.Mantenimiento.Actividad(orden_actividad.id_actividad))
                using (SAT_CL.Almacen.Requisicion requisicion = new SAT_CL.Almacen.Requisicion(actividad.id_requisicion))
                {

                    //Validamos Estatrus de la Actividad no se encentre Cancelada.
                    if (orden_actividad.EstatusActividad != OrdenTrabajoActividad.EstatusOrdenActividad.Cancelada)
                    {
                        //Validando que exista la Actividad
                        if (actividad.habilitar && requisicion.habilitar)
                        {
                            //Insertando Requsición
                            result = SAT_CL.Almacen.Requisicion.InsertaRequisicion(0, requisicion.id_compania_emisora, 0, (byte)SAT_CL.Almacen.Requisicion.Estatus.Registrada,
                                        requisicion.referencia, (byte)SAT_CL.Almacen.Requisicion.Tipo.Trabajo, idAlmacen, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                        Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), requisicion.fecha_entrega,
                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando Operación Exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Requisición
                                idRequisicion = result.IdRegistro;

                                //Insertando Requisición de Actividad de la Orden de Trabajo
                                result = OrdenTrabajoActividadRequisicion.InsertaOrdenActividadRequisicion(Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]),
                                            idRequisicion, Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Obteniendo Detalles
                                    using (DataTable dtDetalles = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(requisicion.id_requisicion))
                                    {
                                        //Validando que existan Detalles
                                        if (Validacion.ValidaOrigenDatos(dtDetalles))
                                        {
                                            //Recorriendo Detalles
                                            foreach (DataRow dr in dtDetalles.Rows)
                                            {
                                                //Instanciando Detalle
                                                using (SAT_CL.Almacen.RequisicionDetalle detalle = new SAT_CL.Almacen.RequisicionDetalle(Convert.ToInt32(dr["NoDetalle"])))
                                                {
                                                    //Validando Detalle
                                                    if (detalle.Habilitar)
                                                    {
                                                        //Insertando Detalle
                                                        result = SAT_CL.Almacen.RequisicionDetalle.InsertaDetalleRequisicion(idRequisicion, detalle.Cantidad, detalle.IdUnidadMedida,
                                                                    detalle.IdProducto, detalle.CodigoProducto, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando si la Operación no fue exitosa
                                                        if (!result.OperacionExitosa)

                                                            //Terminando Ciclo
                                                            break;
                                                    }
                                                    else
                                                        //Instanciando Excepción
                                                        result = new RetornoOperacion("No se puede Acceder al Detalle");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)

                            //Inicializando Requisición
                            ucRequisicion.InicializaRequisicion(idRequisicion, Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), 0, false, true);
                        else
                            //Inicializando Requisición
                            ucRequisicion.InicializaRequisicion(0, Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), 0, false, true);


                        //Alternando Ventana de Requisiciones
                        ScriptServer.AlternarVentana(upbtnGuardarReq, "VentanaRequisicion", "contenidoVentanaRequisicion", "ventanaRequisicion");

                        //Alternando Ventana de Ingreso Almacen
                        ScriptServer.AlternarVentana(upbtnGuardarReq, "VentanaAlmacen", "contenedorVentanaIngresoAlmacen", "ventanaIngresoAlmacen");

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Instanciando Requisición
                            result = new RetornoOperacion(idRequisicion);

                            //Completando Transacción
                            trans.Complete();
                        }                       
                    }
                    else
                    {
                        //Establecemos Mensaje
                        result = new RetornoOperacion("El estatus de la Actividad no permite su edición.");
                    }
                }
            }
            
            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarReq_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarReq, "VentanaAlmacen", "contenedorVentanaIngresoAlmacen", "ventanaIngresoAlmacen");
        }

        /// <summary>
        /// Evento generado al Guardar la Requisición de un Almacen sin Copia de una Maestra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarAlmacenReq_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Declaramos Variables
            int idRequisicion = 0;
            int idAlmacen = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacenReq.Text, "ID:", 1));

            //Instanciamos Orden Trabajo Actividad
            using (OrdenTrabajoActividad objOrdenTrabajoActividad = new OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                //Validamos Estatrus de la Actividad no se encentre Cancelada.
                if (objOrdenTrabajoActividad.EstatusActividad != OrdenTrabajoActividad.EstatusOrdenActividad.Cancelada)
                {
                    //Inicializando Bloque Transaccional
                    using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Insertando Requsición
                        result = SAT_CL.Almacen.Requisicion.InsertaRequisicion(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0, (byte)SAT_CL.Almacen.Requisicion.Estatus.Registrada,
                                                "", (byte)SAT_CL.Almacen.Requisicion.Tipo.Trabajo, idAlmacen, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                                Fecha.ObtieneFechaEstandarMexicoCentro(), Fecha.ObtieneFechaEstandarMexicoCentro(), DateTime.MinValue,
                                                ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Validando Operación Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Obteniendo Requisición
                            idRequisicion = result.IdRegistro;

                            //Insertando Requisición de Actividad de la Orden de Trabajo
                            result = OrdenTrabajoActividadRequisicion.InsertaOrdenActividadRequisicion(Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]),
                                        idRequisicion, Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);


                        }
                        //Validamos Resultado
                        if (result.OperacionExitosa)
                        {

                            //Inicializando Requisición
                            ucRequisicion.InicializaRequisicion(idRequisicion, Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), 0, false, true);

                            //Alternando Ventana de Requisiciones
                            alternaVentanaModal("Requisicion", upbtnGuardarAlmacenReq);

                            //Cerramos Ventana de Almacen
                            alternaVentanaModal("AlmacenReq", upbtnGuardarAlmacenReq);
                        }
                        //Validmos Resultado
                        if (result.OperacionExitosa)
                        {
                            //Finalizamos Transacción
                            trans.Complete();
                        }
                    }
                }
                else
                {
                    //Establecemos Mensaje
                    result = new RetornoOperacion("El estatus de la Actividad no permite su edición.");
                }
            }
            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        #region Eventos GridView "Requisiciones"

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicion_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRequisicion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvRequisicion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamañoGridViewRequisicion.SelectedValue));
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Documentos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelRequisicion_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden delGV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicion_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewRequisicion.Text = Controles.CambiaSortExpressionGridView(gvRequisicion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewRequisicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            Controles.CambiaTamañoPaginaGridView(gvRequisicion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamañoGridViewRequisicion.SelectedValue));
        }


        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionDetalle_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvRequisicionDetalle, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);

            //Suma Totales
            sumaTotalesRequisicionDetalle();
        }
 
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Documentos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelRequisicionDetalle_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"));
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden delGV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRequisicionDetalle_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewRequisicionDetalle.Text = Controles.CambiaSortExpressionGridView(gvRequisicionDetalle, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);

            //Suma Totales
            sumaTotalesRequisicionDetalle();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewRequisicionDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño
            Controles.CambiaTamañoPaginaGridView(gvRequisicionDetalle, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamañoGridViewRequisicionDetalle.SelectedValue));

            //Suma Totales
            sumaTotalesRequisicionDetalle();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRequisicionOrden_Click(object sender, EventArgs e)
        {
            //Validando que Existan los Registros
            if (gvRequisicion.DataKeys.Count > 0)
            {
                //Obteniendo Link
                LinkButton lkb = (LinkButton)sender;

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvRequisicion, sender, "lnk", false);

                //Inicializando Requisición
                ucRequisicion.InicializaRequisicion(Convert.ToInt32(gvRequisicion.SelectedDataKey["Id"]), Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), 0, false, true);

                //Alternando Ventana
                TSDK.ASP.ScriptServer.AlternarVentana(this, "VentanaRequisicion", "contenidoVentanaRequisicion", "ventanaRequisicion");
            }
        }

        #endregion

        #endregion

        #region Eventos "Asignaciones"

        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelarAsignacion_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnCancelarCancelarAsignacion, upbtnCancelarCancelarAsignacion.GetType(), "CerrarVentanaModal", "contenidoConfirmacionCancelarAsignacion",
                "confirmacionCancelarAsignacion");
        }
        /// <summary>
        /// Evento generado al dar click en Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelarAsignacion_Click(object sender, EventArgs e)
        {
            //Evento generado al Cancelar la asignación
            cancelaAsignacion();
        }
        /// <summary>
        /// Evento generado al Cancelar la Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelarAsignacion_Click(object sender, EventArgs e)
        {
            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(gvAsignaciones, upgvAsignaciones.GetType(), "CancelarAsignacion", "contenidoConfirmacionCancelarAsignacion",
                "confirmacionCancelarAsignacion");
        }

        #region Eventos GridView "Asignaciones"

        /// <summary>
        /// Evento producido al cambiar el indice de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvAsignaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
        }
        /// <summary>
        /// Evento producido al cambiar el tamaño de página del GV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvAsignaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamañoGridViewAsignaciones.SelectedValue));
        }
        /// <summary>
        /// Eventó producido al pulsar el botón de exportación de contenido de GV de Documentos a excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcelAsignaciones_Onclick(object sender, EventArgs e)
        {
            Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "");
        }
        /// <summary>
        /// Evento producido al cambiar el criterio de orden delGV de Documentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAsignaciones_Onsorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewAsignaciones.Text = Controles.CambiaSortExpressionGridView(gvAsignaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
        }
        /// <summary>
        /// Evento geenrado al dar click en Responsable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbResponsable_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAsignaciones.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

                //Mostrar Ventana Modal
                ScriptServer.AlternarVentana(gvAsignaciones, upgvAsignaciones.GetType(), "MostrarVentanaModal", "contenidoAsignacion",
                    "confirmacionAsignacion");
                
                //Inicializamos Control
                wucAsignacionActividad.InicializaAsignacionActividad(Convert.ToInt32(gvAsignaciones.SelectedValue), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), Convert.ToInt32(gvActividades.SelectedDataKey["IdActividad"]), Convert.ToByte(SAT_CL.Global.Catalogo.RegresaDescripcionValor(3158, gvAsignaciones.SelectedDataKey["Puesto"].ToString())));
            }
        }

        #endregion

        #endregion

        #region Eventos Control "Asignación Actividad"

        /// <summary>
        /// Evento generado al Guardar La Asignación
        /// </summary>
        protected void wucAsignacionActividad_ClickGuardarAsignacion(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Establecemos Mensaje Resultado
            resultado = wucAsignacionActividad.GuardaAsignacionActividad();
            
                //Mostramos Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            
        }
        /// <summary>
        /// Evento generado al Iniciar la Actividad
        /// </summary>
        protected void wucAsignacionActividad_ClickIniciarAsignacion(object sender, EventArgs e)
        {
            //Limpiamos Etiqueta 
            txtFechaInicio.Text = "";

            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "MostrarVentanaModal", "contenidoAsignacion",
                "confirmacionAsignacion");

            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "AbrirVentanaModal", "contenidoFechaInicio",
                "confirmacionFechaInicio");

        }

       

        /// <summary>
        /// Evento generado al dar click en elimina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionActividad_ClickEliminarAsignacion(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado 
            RetornoOperacion resultado = new RetornoOperacion();

            //Deshabilitamos Asignacion
            resultado = wucAsignacionActividad.EliminaAsignacionActividad();

                //Mostramos Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
               

        }
        /// <summary>
        /// Evento generado al Terminar la Actividad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucAsignacionActividad_ClickTerminarAsignacion(object sender, EventArgs e)
        {
            //Limpiamos Controles
            txtFechaTerminar.Text = "";
            //Cerramos Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "Asignacion", "contenidoAsignacion",
                "confirmacionAsignacion");
            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(this, this.GetType(), "TerminarAsignacion", "contenidoFechaTerminarAsignacion",
                "confirmacionFechaTerminarAsignacion");

        }
        /// <summary>
        /// Evento generado al Registrar la Actividad
        /// </summary>
        protected void wucActividadOrdenTrabajo_Registrar(object sender, EventArgs e)
        {
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Registramos Actividad
            resultado = ucActividadOrdenTrabajo.RegistraOtrdenTrabajoActividad();

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {  
                //Carga Actividades Por Asignar
                cargaActividadesPorAsignar();

                //Inicializando Control
                ucActividadOrdenTrabajo.InicializaControl(0, Convert.ToInt32(gvActividades.SelectedDataKey["IdOrden"]), 0);
            }
            //Mostramos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento generado al dar click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarInicio_Click(object sender, EventArgs e)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Iniciamos Asignacion
            resultado = wucAsignacionActividad.IniciarAsignacion(Convert.ToDateTime(txtFechaInicio.Text));

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                ScriptServer.AlternarVentana(this, this.GetType(), "MostrarVentanaModal", "contenidoAsignacion",
                    "confirmacionAsignacion");

                //Mostrar Ventana Modal
                ScriptServer.AlternarVentana(btnAceptarInicio, upbtnAceptarInicio.GetType(), "AbrirVentanaModal", "contenidoFechaInicio",
                    "confirmacionFechaInicio");
                //Carga Actividades
                cargaActividadesPorAsignasManteniendoSeleccion();
                //Cargamos Asignaciones
                cargaAsignaciones();

            }
            
                //Mostramos Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);                
            
        }

        /// <summary>
        /// Método generado al Aceptar el Inicio de la Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarInicioDirecto_Click(object sender, EventArgs e)
        { 
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Iniciamos Asignacion
            resultado = wucAsignacionActividad.IniciarAsignacion(Convert.ToDateTime(txtFechaInicioDirecto.Text));

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                alternaVentanaModal("InicioAsignacionDirecto", btnAceptarInicioDirecto);

                //Carga Actividades
                cargaActividadesPorAsignasManteniendoSeleccion();
                //Cargamos Asignaciones
                cargaAsignaciones();
                //Inicializamos Indices
                Controles.InicializaIndices(gvAsignaciones);

            }

            //Mostramos Mensaje
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);   
        }

        /// <summary>
        /// Evento generado al Terminar la Asignacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTerminar_Click(object sender, EventArgs e)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Terminamos Asignacion
            resultado = wucAsignacionActividad.TerminarAsignacion(Convert.ToDateTime(txtFechaTerminar.Text));

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                ScriptServer.AlternarVentana(btnAceptarTerminar, upbtnAceptarTerminar.GetType(), "MostrarVentanaModal", "contenidoAsignacion",
                    "confirmacionAsignacion");

                //Mostrar Ventana Modal
                ScriptServer.AlternarVentana(btnAceptarTerminar, upbtnAceptarTerminar.GetType(), "TerminarAsignacion", "contenidoFechaTerminarAsignacion",
                    "confirmacionFechaTerminarAsignacion");
                //Carga Actividades
                cargaActividadesPorAsignar();
                //Cargamos Asignaciones
                cargaAsignaciones();
            }

                //Mostramos Mensaje
                TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                
        }

        /// <summary>
        /// Evento generado al dar clickal Terminar la Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTerminarDirecto_Click(object sender, EventArgs e)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Terminamos Asignacion
            resultado = wucAsignacionActividad.TerminarAsignacion(Convert.ToDateTime(txtFechaTerminarDirecto.Text));

            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cerramos Ventana Modal
                alternaVentanaModal("TerminoAsignacionDirecto", btnAceptarTerminarDirecto);

                //Carga Actividades
                cargaActividadesPorAsignar();
                //Cargamos Asignaciones
                cargaAsignaciones();
                //Inicializamos Indices
                Controles.InicializaIndices(gvAsignaciones);
            }

            //Mostramos Mensaje
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Evento generado al dar click en las operaciones de Asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbOperaciones_Click(object sender, EventArgs e)
        {
            //Validamos Datos GV de Asignaciones
            if (gvAsignaciones.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvAsignaciones, sender, "lnk", false);

                //Inicializamos Control
                wucAsignacionActividad.InicializaAsignacionActividad(Convert.ToInt32(gvAsignaciones.SelectedValue), Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), Convert.ToInt32(gvActividades.SelectedDataKey["IdActividad"]),1);                      
                 
                //Validando estatus de Página
                switch (((LinkButton)sender).CommandName)
                {
                      case "Iniciar":
                        { 
                            //Limpiamos Etiqueta 
                            txtFechaInicioDirecto.Text = "";
                            //Iniciamos Asignación
                            alternaVentanaModal("InicioAsignacionDirecto", gvAsignaciones);

                            break;
                        }
                    case "Terminar":
                        { 
                            //Limpiamos Etiqueta 
                            txtFechaTerminarDirecto.Text = "";
                            //Iniciamos Asignación
                            alternaVentanaModal("TerminoAsignacionDirecto", gvAsignaciones);
                            break;
                        }
                }
            }
        }

        #endregion

        #region Eventos Control "Requisiciones"
        /// <summary>
        /// Evento generado al Cancelar una Requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelar_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvRequisicion, sender, "lnk", false);
            //Mostramos Ventana modal
            alternaVentanaModal("CanRequisicion", this.gvRequisicion);
        }
        /// <summary>
        /// Evento generado al Cancelar la Requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCanRequisicion_Click(object sender, EventArgs e)
        {
            //Validamos Datos GV de Asignaciones
            if (gvRequisicion.DataKeys.Count > 0)
            {
                //Declaramos objeto Resultado
                RetornoOperacion resultado = new RetornoOperacion();
               
                //Instanciamos Requisicion
                using(SAT_CL.Almacen.Requisicion objRequisicion = new SAT_CL.Almacen.Requisicion(Convert.ToInt32(gvRequisicion.SelectedValue)))
                {
                    //Cancelamos Requisición
                    resultado = objRequisicion.CancelaRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    if (resultado.OperacionExitosa)
                    {
                        //Cerramos Ventana
                        alternaVentanaModal("CanRequisicion", this.gvRequisicion);
                        //Invocando Carga de Requisiciones
                        cargaRequisiciones();
                    }
                    //Mostrando Mensaje de Operación
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        /// <summary>
        /// Evenyo generado al dar click en el Detalle de la Requisición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDetalle_Click(object sender, EventArgs e)
        {
             //Validamos Datos GV de Asignaciones
            if (gvRequisicion.DataKeys.Count > 0)
            {
                //Seleccionando la fila actual
                Controles.SeleccionaFila(gvRequisicion, sender, "lnk", false);
                //Mostramos Grid View Detalle
                mtvRequisicion.ActiveViewIndex = 1;
                //Cargamos las Requisiciones Detalle
                cargaRequisicionesDetalles();
            }
        }


        /// <summary>
        /// Evento Generado al dar click en Requisición Encabezado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbRequicionEncabezado_Click(object sender, EventArgs e)
        {
            //Inicializamos Indices
            Controles.InicializaGridview(gvRequisicion);

            //Cargamos Requisiciones
            cargaRequisiciones();

            //Mostramos Vista
            mtvRequisicion.ActiveViewIndex = 0;
        }

        /// <summary>
        /// Evento generado al Guardar la Requisicion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucRequisicion_ClickGuardarRequisicion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Guardando Requisición
            result = ucRequisicion.GuardaRequisicion();

            //ValidandoOoperación Exitosa
            if (result.OperacionExitosa)
            {
                //Invocando Carga de Requisiciones
                cargaRequisiciones();

                //Inicializamos Indices de la Requisición
                Controles.InicializaIndices(gvRequisicion);
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucRequisicion_ClickSolicitarRequisicion(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Solicitando Requisición
            result = ucRequisicion.SolicitaRequisicion();

            //ValidandoOoperación Exitosa
            if (result.OperacionExitosa)
            {
                //Invocando Carga de Requisiciones
                cargaRequisiciones();

                //Inicializamos Indices de la Requisición
                Controles.InicializaIndices(gvRequisicion);
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Mètodo encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Carga Catalogos
            cargaCatalogos();
            //Inicializamos GridView
            inicializaGridView();
            //Habilita Controles
            habilitaControles();
            //Inicializa Controles
            inicializaValores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlArea, "TODOS", 1121);
            //Cargando Catalogo Sub Area
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlSubArea, "TODOS", 1122, Convert.ToInt32(ddlArea.SelectedValue));
            //Cargando Catalogo Tamaño DropDownList Actividades
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewActividades, "", 26, 0);
            //Cargando Catalogo Tamaño DropDownList Asiganciones
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewAsignaciones, "", 39, 0);
            //Cargando Catalogo Tamaño DropDownList Requisiciones
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewRequisicion, "", 39, 0);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewRequisicionDetalle, "", 39, 0);
        }
        /// <summary>
        /// Inicializamos Grid View
        /// </summary>
        private void inicializaGridView()
        {
            //Inicializando Controles
            Controles.InicializaGridview(gvActividades);
            Controles.InicializaGridview(gvAsignaciones);
            Controles.InicializaGridview(gvRequisicion);
        }

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {
            //Mostrando Fechas
            txtFechaInicioOrdenTrabajo.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFechaFinOrdenTrabajo.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Método encargado de Inicializar los Valores de la Página
        /// </summary>
        private void habilitaControles()
        {
            //Habilitamos Controles
            chkTerminada.Checked =
            txtFechaInicioOrdenTrabajo.Enabled =
            txtFechaFinOrdenTrabajo.Enabled = false;
            chkActiva.Checked =
            chkPausada.Checked = true;
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
                case "Asignacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoAsignacion", "confirmacionAsignacion");
                    break;
                case "Actividades":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaActividades", "ventanaActividades");
                    break;
                case "InicioAsignacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoFechaInicio", "confirmacionFechaInicio");
                    break;
                case "TerminoAsignacion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoFechaTerminarAsignacion", "confirmacionFechaTerminarAsignacion");
                    break;
                case "InicioAsignacionDirecto":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoFechaInicioDirecto", "confirmacionFechaInicioDirecto");
                    break;
                case "TerminoAsignacionDirecto":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoFechaTerminarAsignacionDirecto", "confirmacionFechaTerminarAsignacionDirecto");
                    break;
                case "CanRequisicion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionCanRequisicion", "confirmacionCanRequisicion");
                    break;
                case "AlmacenReq":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaIngresoAlmacenReq", "ventanaIngresoAlmacenReq");
                    break;
                     case "Requisicion":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoVentanaRequisicion", "ventanaRequisicion");
                    break;
                     case "EliminarReq":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionEliminarRequisicion", "confirmacionEliminarRequisicion");
                    break;
                     case "AbrirActividad":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionAbrirActividad", "confirmacionAbrirActividad");
                    break;

            }
        }
        /// <summary>
        /// Evento Producido al Cerrar las Ventanas Modales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Obteniendo Control
            LinkButton lnk = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lnk.CommandName)
            {
                case "activiades":
                    //Cerrando ventana modal 
                    alternaVentanaModal("Actividades", lnk);
                    //Inicializando Indices
                    Controles.InicializaIndices(gvActividades);
                    break;
                case "inicioAsignacionDirecto":
                    //Cerrando ventana modal 
                    alternaVentanaModal("InicioAsignacionDirecto", lnk);
                    break;
                case "terminoAsignacionDirecto":
                    //Cerrando ventana modal 
                    alternaVentanaModal("TerminoAsignacionDirecto", lnk);
                    //Inicializando Indices
                    break;
                case "canRequisicion":
                    //Cerrando ventana modal 
                    alternaVentanaModal("CanRequisicion", lnk);
                    //Inicializando Indices
                    break;
                case "almacenReq":
                    //Cerrando ventana modal 
                    alternaVentanaModal("AlmacenReq", lnk);
                    //Inicializando Indices
                    break;
                case "eliminarReq":
                    //Cerrando ventana modal 
                    alternaVentanaModal("EliminarReq", lnk);
                    //Inicializando Indices
                    break;
                case "abrirActividad":
                    //Cerrando ventana modal 
                    alternaVentanaModal("AbrirActividad", lnk);
                    //Inicializando Indices
                    break;
            }
        }

        #region Métodos "Actividades"

        /// <summary>
        /// Realiza la carga de las unidades manteniendo una selección de registro previa
        /// </summary>
        private void cargaActividadesPorAsignasManteniendoSeleccion()
        {
            //Obteniendo el registro seleccionado actualmente
            string id_registro_seleccion = gvActividades.SelectedIndex > -1 ? gvActividades.SelectedDataKey["Id"].ToString() : "";
            //Cargando Gridview
            cargaActividadesPorAsignar();
            //Restableciendo selección en caso de ser necesario
            if (id_registro_seleccion != "")
                Controles.MarcaFila(gvActividades, id_registro_seleccion, "Id", "Id-IdOrden-IdActividad", (DataSet)Session["DS"], "Table", lblCriterioGridViewActividades.Text, Convert.ToInt32(ddlTamañoGridViewActividades.SelectedValue), false, 4);
        }

        /// <summary>
        /// Método encargado de cargar las Actividades por Asignar
        /// </summary>
        private void cargaActividadesPorAsignar()
        {
            //Declaramos Fecha de Inicio de la Orden de Trabajo
            DateTime fecha_inicio = Convert.ToDateTime(txtFechaInicioOrdenTrabajo.Text);
            DateTime fecha_fin = Convert.ToDateTime(txtFechaFinOrdenTrabajo.Text);
            //Declaramos Variable parada cada uno de los Estatus
            byte activa = (byte)SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Activa;
            byte pausada = (byte)SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Pausada;
            byte terminada = (byte)SAT_CL.Mantenimiento.OrdenTrabajo.EstatusOrdenTrabajo.Terminada;
            //Validamos Si se Desean Buscar Ordenes de Trabajo Activa
            if(!chkActiva.Checked)
            {
              //Omitimos Estatus
                activa = 0;
            }
            //Validamos Si se Desean Buscar Ordenes de Trabajo Pausadas
            if (!chkPausada.Checked)
            {
                //Omitimos Estatus
                pausada = 0;
            }
            //Validamos Si se Desean Buscar Ordenes de Trabajo Terminadas
            if (!chkTerminada.Checked)
            {
                //Omitimos Estatus
                terminada = 0;
                //Asignamos Fechas Minimas
                fecha_inicio = DateTime.MinValue;
                fecha_fin = DateTime.MinValue;
            }

            //Declaramos Variable para almacenar los estatus de la Orden de Trabajo
            string id_estatus =  activa.ToString() + ","+ pausada.ToString() +","+ terminada.ToString();
            //Obtenemos Detalles
            using (DataTable mit = SAT_CL.Mantenimiento.Reportes.CargaActividadesPorAsignar(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoOrden.Text,
                                                                                            id_estatus, 0, 0, txtUnidad.Text,
                                                                                            Convert.ToByte(ddlArea.SelectedValue), Convert.ToByte(ddlSubArea.SelectedValue), fecha_inicio,
                                                                                            fecha_fin))
            {
                //Valida Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Añadiendo Tabla
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");

                    //Cargamos Grid View
                    TSDK.ASP.Controles.CargaGridView(gvActividades, mit, "Id-IdOrden-IdActividad", lblCriterioGridViewActividades.Text, false, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvActividades);
                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        /// <summary>
        /// Cancela Asignacion
        /// </summary>
        private void cancelaActividad()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Asignacion
            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad objActividadAsignacion = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                // Cancelamos Asignacion
                res = objActividadAsignacion.CancelaOrdenTrabajoActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (res.OperacionExitosa)
                {
                    //Cargamos Actibidades
                    cargaActividadesPorAsignar();
                }
            }
            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(btnAceptarCancelarActividad, upbtnAceptarCancelarActividad.GetType(), "MostrarVentanaModal", "contenidoConfirmacionCancelarActividad",
                        "confirmacionCancelarActividad");
            //Establecemos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, res, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
            
        }
        /// <summary>
        /// Terminar ACtividad
        /// </summary>
        private void terminarActividad()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Asignacion
            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad objActividad = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                // Cancelamos Asignacion
                res = objActividad.TerminaActividad(Convert.ToDateTime(txtFechaTerminarActividad.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (res.OperacionExitosa)
                {
                    //Cargamos Actibidades
                    cargaActividadesPorAsignar();
                    //Cerrar Ventana Modal
                    ScriptServer.AlternarVentana(btnTerminarActividad, btnTerminarActividad.GetType(), "TerminarActividad", "contenidoFechaTerminarActividad",
                        "confirmacionFechaTerminarActividad");
                }
            }
            //Establecemos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, res, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Cancela Asignacion
        /// </summary>
        private void eliminarActividad()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Asignacion
            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad objActividadAsignacion = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                // Cancelamos Asignacion
                res = objActividadAsignacion.DeshabilitaOrdenTrabajoActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (res.OperacionExitosa)
                {
                    //Cargamos Actibidades
                    cargaActividadesPorAsignar();
                    //Inicializamos Grid View de Asignaciones
                    Controles.InicializaGridview(gvAsignaciones);

                }
            }
            //Mostrar Ventana Modal
            ScriptServer.AlternarVentana(btnAceptarEliminarActividad, upbtnAceptarEliminarActividad.GetType(), "MostrarVentanaModal", "contenidoConfirmacionEliminarActividad",
                        "confirmacionEliminarActividad");
            //Establecemos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, res, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        /// <summary>
        /// Cancela Asignacion
        /// </summary>
        private RetornoOperacion abrirActividad()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Asignacion
            using (SAT_CL.Mantenimiento.OrdenTrabajoActividad objActividad = new SAT_CL.Mantenimiento.OrdenTrabajoActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                // Cancelamos Asignacion
                res = objActividad.AbrirActividad(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                //Validamos Resultado
                if (res.OperacionExitosa)
                {
                    //Cargamos Actividades
                    cargaActividadesPorAsignar();

                }
            }
            //Obtenemos Resultado
            return res;
        }

        #endregion

        #region Métodos "Asignación"

        /// <summary>
        /// Método encargado de cargar las Actividades por Asignar
        /// </summary>
        private void cargaAsignaciones()
        {

            //Obtenemos Detalles
            using (DataTable mit = SAT_CL.Mantenimiento.ActividadAsignacion.CargaAsignacionesRequeridas(Convert.ToInt32(gvActividades.SelectedDataKey["Id"]), Convert.ToInt32(gvActividades.SelectedDataKey["IdActividad"])))
            {
                //Valida Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Añadiendo Tabla
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table1");

                    //Cargamos Grid View
                    TSDK.ASP.Controles.CargaGridView(gvAsignaciones, mit, "Id-Puesto", lblCriterioGridViewAsignaciones.Text, false, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAsignaciones);

                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
                upgvAsignaciones.Update();
            }
        }
        /// <summary>
        /// Cancela Asignacion
        /// </summary>
        private void cancelaAsignacion()
        {
            //Declaramos Objeto Resultado
            RetornoOperacion res = new RetornoOperacion();

            //Instanciamos Asignacion
            using (SAT_CL.Mantenimiento.ActividadAsignacion objActividadAsignacion = new SAT_CL.Mantenimiento.ActividadAsignacion(Convert.ToInt32(gvAsignaciones.SelectedValue)))
            {
                // Cancelamos Asignacion
                res = objActividadAsignacion.CancelaActividadAsignacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Validamos Resultado
                if (res.OperacionExitosa)
                {
                    //Cargamos Asignaciones
                    cargaAsignaciones();
                    //Carga Actividades
                    cargaActividadesPorAsignar();
                }
            }
            //Establecemos Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, res, TSDK.ASP.ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(btnAceptarCancelarAsignacion, upbtnAceptarCancelarAsignacion.GetType(), "CancelarAsignacion", "contenidoConfirmacionCancelarAsignacion",
                "confirmacionCancelarAsignacion");
        }

        #endregion

        #region Métodos "Requisición"

        /// <summary>
        /// Deshabilitamos Requisición
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion deshabilitaRequisicion()
        {
            //Definiendo objeto de resultado
            TSDK.Base.RetornoOperacion resultado = new TSDK.Base.RetornoOperacion();

            //Validando que exista un registro seleccionado
            if (gvRequisicion.SelectedIndex != -1)
            {

                //Instanciando registro a editar
                using (OrdenTrabajoActividadRequisicion objOrdenTrabajoReq = new OrdenTrabajoActividadRequisicion(OrdenTrabajoActividadRequisicion.ObtieneOrdenTrabajActividadRequisicion(Convert.ToInt32(gvRequisicion.SelectedValue))))
                {
                    //Si existe el documento
                    if (objOrdenTrabajoReq.id_orden_actividad_requisicion > 0)
                        //Realizando guardado de documento
                        resultado = objOrdenTrabajoReq.DeshabilitaOrdenActividadRequisicion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                    else
                        resultado = new TSDK.Base.RetornoOperacion("El registro no fue recuperado, posiblemente ya no existe.");
                }

                //Si no existe error
                if (resultado.OperacionExitosa)
                {
                    //Actualziando contenido de GridView
                    cargaRequisiciones();
                    //Inicializmos Indices
                Controles.InicializaIndices(gvRequisicion);
                }
            }
            
                //Devolvemos Resultado
                return  resultado;
        }

        /// <summary>
        /// Método encargado de cargar las Requisiciones por Asignar
        /// </summary>
        private void cargaRequisiciones()
        {
            //Obtenemos Detalles
            using (DataTable mit = SAT_CL.Almacen.Reportes.ObtieneRequisicionesActividad(Convert.ToInt32(gvActividades.SelectedDataKey["Id"])))
            {
                //Valida Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Añadiendo Tabla
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table2");

                    //Cargamos Grid View
                    TSDK.ASP.Controles.CargaGridView(gvRequisicion, mit, "Id", lblCriterioGridViewRequisicion.Text, false, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvRequisicion);
                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }

            }
        }


        /// <summary>
        /// Método encargado de cargar las Requisiciones por Asignar
        /// </summary>
        private void cargaRequisicionesDetalles()
        {
            //Obtenemos Detalles
            using (DataTable mit = SAT_CL.Almacen.RequisicionDetalle.ObtieneDetallesRequisicion(Convert.ToInt32(gvRequisicion.SelectedDataKey["Id"])))
            {
                //Valida Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Añadiendo Tabla
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table3");

                    //Cargamos Grid View
                    TSDK.ASP.Controles.CargaGridView(gvRequisicionDetalle, mit, "NoDetalle", lblCriterioGridViewRequisicionDetalle.Text, false, 0);
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvRequisicionDetalle);
                    //Elimina Tabla Dataset
                    OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                }

            }
            //Suma Totales del Grid View
            sumaTotalesRequisicionDetalle();
        }

        /// <summary>
        /// Método encargado de Sumar los Totales de la Requisición Detalles
        /// </summary>
        private void sumaTotalesRequisicionDetalle()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3")))
            {
                //Mostrando Totales
                gvRequisicionDetalle.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvRequisicionDetalle.FooterRow.Cells[8].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion



    



 
      

      

     

       

        #endregion
    }
}