using SAT_CL.Despacho;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Documentacion
{
    public partial class AnticiposProgramados : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Carga de la páginalkbAcciones_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.lkbAcciones_Click);

            //Si no es recarga de página
            if (!IsPostBack)
            {
                inicializaForma();
                if (Request.QueryString["idReg1"] != null)
                {
                    //Invoca al método carga los servicios a buscar
                    cargaControlAnticipoProgramado();
                }
            }

        }
        /// <summary>
        /// Cambio se selección para uso de filtro de fechas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se ha marcado el filtro
            if (chkRangoFechas.Checked)
            {
                //Habilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = true;

                //Colocando fechas predeterminadas (una semana)
                txtFechaFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(1).AddMinutes(-1).ToString("dd/MM/yyyy HH:mm");
                txtFechaInicio.Text = Fecha.ObtieneFechaEstandarMexicoCentro().Date.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            }
            //Si no se ha marcado
            else
            {
                //Deshabilitando controles de filtrado
                rdbCitaCarga.Enabled =
                rdbCitaDescarga.Enabled =
                rdbInicioServicio.Enabled =
                rdbFinServicio.Enabled =
                txtFechaInicio.Enabled =
                txtFechaFin.Enabled = false;

                //Limpiando fechas
                txtFechaInicio.Text = txtFechaFin.Text = "";
            }
        }
        /// <summary>
        /// Click en botón buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscaServicios();
        }
        /// <summary>
        /// Evento que muestra los anticipos programados y normales del servicio seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbVerAnticipos_Click(object sender, EventArgs e)
        {
            //Si hay registros servicio mostrados
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvServiciosContextual, sender, "lnk", false);
                //Titulo de Control
                h2EncabezadoAnticiposProgramados.InnerText = string.Format("Anticipos del servicio '{0}'", gvServiciosContextual.SelectedDataKey["NoServicio"]);
                //ALternando Ventana
                ScriptServer.AlternarVentana(this.Page, "VerAnticipos", "contenedorVerAnticipos", "ventanaVerAnticipos");
                VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
            }

        }
        /// <summary>
        /// Evento que dispara la ventana modal de programación de anticipos, vales de diésel y cálculo de rutas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbProgramacion_Click(object sender, EventArgs e)
        {
            //Si hay registros servicio mostrados
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvServiciosContextual, sender, "lnk", false);

                //ALternando Ventana
                ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
            }

        }
        /// <summary>
        /// Evento que muestra la ventana modal de programación de anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProgramarAnticipo_Click(object sender, EventArgs e)
        {
            //Si hay registros servicio mostrados
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvServiciosContextual, sender, "btn", false);

                //ALternando Ventana
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");

                //Declaramos objeto Resultado
                RetornoOperacion resultado = new RetornoOperacion();

                //Declaramos variables
                int id_unidad = 0; int id_operador = 0; int id_tercero = 0;
                //Indicando que no es requerido mostrar solo concepto de anticipo a proveedor
                ucDepositos.SoloAnticipoProveedor = false;

                //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                       Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Instanciando Movimiento
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))
                    {
                        //Instanciando Servicio
                        using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                        {
                            ucDepositos.HabilitaConcepto = true;
                            ucDepositos.MuestraSolicitar = false;
                            //Inicializamos Control Depósito en Edición
                            ucDepositos.InicializaControl(0, id_unidad, id_operador, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]), ser.id_servicio, mov.id_movimiento,
                                      Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]), ser.id_cliente_receptor);

                        }
                    }
                }

                //Sólo si hay error
                if (!resultado.OperacionExitosa)
                    //Notificando que no es posible realizar esta acción para otro proveedor
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Evento que dispara la ventana modal para registrar un nuevo vale de diésel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValeDiesel_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Seleccionando fila actual
            Controles.SeleccionaFila(gvServiciosContextual, sender, "btn", false);

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
            resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                                Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");

                //Instanciando Movimiento
                using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))

                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                {
                    //Inicializamos Control Movimiento en vacio
                    ucAsignacionDiesel.InicializaControlUsuario(0, id_unidad, id_operador, id_tercero, ser.id_servicio, mov.id_movimiento,
                                                                Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]));
                }
            }

            //Sólo si hay error
            if (!resultado.OperacionExitosa)
                //Notificando que no es posible realizar esta acción para otro proveedor
                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento que dispara la ventana modal para hacer cálculos a la ruta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalcularRuta_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Si hay registros servicio mostrados
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvServiciosContextual, sender, "btn", false);

                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "CalcularRuta", "contenedorCalcularRuta", "ventanaCalcularRuta");
                //Declaramos variables
                int id_unidad = 0; int id_operador = 0; int id_tercero = 0;
                //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                       Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
                //Instanciamos Movimiento
                using (SAT_CL.Despacho.Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))
                {
                    //Inicializando contenido de control para el Calculo de Ruta
                    wucCalcularRuta.InicializaControl(objMovimiento.id_servicio, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]), id_unidad, id_operador, id_tercero);
                }
            }
        }
        /// <summary>
        /// Evento que cierra la ventana modal de los anticipos programados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            ScriptServer.AlternarVentana(this.Page, "VerAnticipos", "contenedorVerAnticipos", "ventanaVerAnticipos");
            //Recargando Grid
            //buscaServicios();
            Controles.InicializaIndices(gvServiciosContextual);
            VerAnticipos(0);
        }
        /// <summary>
        /// Evento que cierra la ventana modal de Programación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModalProgramacion_Click(object sender, EventArgs e)
        {
            //Alternando Ventana
            ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
            //Recargando Grid
            ///buscaServicios();
            Controles.InicializaIndices(gvServiciosContextual);
        }
        /// <summary>
        /// Evento que cierra la ventana modal para programar anticipos del servicio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModalProgramarAnticipo_Click(object sender, EventArgs e)
        {
            //Validando si se está cerrando desde la vista edición o nuevo
            if (gvAnticipos.DataKeys.Count == 0)
            {
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                //VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                Controles.InicializaIndices(gvAnticipos);
            }

        }
        /// <summary>
        /// Evento que cierra la ventana modal de vales de diésel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModalAsignacionDieselProgramado_Click(object sender, EventArgs e)
        {
            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                //VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                Controles.InicializaIndices(gvAnticipos);
            }
        }
        /// <summary>
        /// Evento que cierra la ventana modal de cálculo de rutas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModalCalcularRuta_Click(object sender, EventArgs e)
        {
            //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
            //Recargando Grid
            //buscaServicios();
            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "CalcularRuta", "contenedorCalcularRuta", "ventanaCalcularRuta");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "CalcularRuta", "contenedorCalcularRuta", "ventanaCalcularRuta");
                //VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                Controles.InicializaIndices(gvAnticipos);
            }
               
        }
        /// <summary>
        /// Evento que registra o edita un depósito programado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickRegistrar(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                     Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
            //Insertamos Depósitos
            resultado = ucDepositos.RegistraDeposito();

            //Validamos Inserción de Depósito
            if (resultado.OperacionExitosa)
            {
                //Instanciando Deposito
                using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(resultado.IdRegistro))


                //Instanciando Concepto de Deposito
                using (SAT_CL.EgresoServicio.ConceptoDeposito cd = new SAT_CL.EgresoServicio.ConceptoDeposito(dep.id_concepto))
                {
                    //Validando que existan los Registros
                    if (dep.habilitar && cd.habilitar)
                    {
                        //Validando que 
                        if (!cd.descripcion.Equals("Anticipo Proveedor"))
                        {
                            //Validamos Estatus del Depósito diferente de Registrado
                            if (dep.id_estatus != 1)
                            {
                                //Mostramos Reporte Anticipos
                                //mtvAnticipos.ActiveViewIndex = 2;
                                //cargamos Anticipos
                                //cargaAnticiposEntidad(id_operador, id_unidad, id_tercero);
                            }
                        }
                    }

                }
            }

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
            }
        }

        /// <summary>
        /// Eventó Generado al Cancelar un Depósito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickCancelar(object sender, EventArgs e)
        {
            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                //VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                Controles.InicializaIndices(gvAnticipos);
            }
        }
        /// <summary>
        /// Evento que deshabilita un depósito programado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDepositos_ClickEliminar(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Eliminamos Depósito
            resultado = ucDepositos.DeshabilitaDeposito(Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"]));

            //Validamos Deshabilitado de Depósito
            if (resultado.OperacionExitosa)
            {
                //Recargando GridView de Anticipos
                //buscaServicios();
                VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedValue));
            }
            //Controles.InicializaIndices(gvAnticipos);
            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            //Alternando Ventana
            ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
        }

        /// <summary>
        /// Cambio de tamaño de página de gridview de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvServiciosContextual, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewServicios.SelectedValue), true, 3);
        }

        /// <summary>
        /// Cambio de tamaño de página de gridview de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañoGridViewAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewAnticipos.SelectedValue), true, 3);
        }

        /// <summary>
        /// Cambio de página en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosContextual_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvServiciosContextual, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }
        /// <summary>
        /// Cambio de criterio de orden en gv de servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosContextual_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewServicios.Text = Controles.CambiaSortExpressionGridView(gvServiciosContextual, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }

        /// <summary>
        /// Cambio de página en gv de anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
        }

        /// <summary>
        /// Cambio de criterio de orden en gv de anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblCriterioGridViewAnticipos.Text = Controles.CambiaSortExpressionGridView(gvAnticipos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
        }
        /// <summary>
        /// Evento que llena el GridView de Anticipos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //validando Fila de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando información de la fila actual
                if (e.Row.DataItem != null)
                {
                    //Obteniendo Fila de Datos
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;

                    //Validando Fila
                    if (fila != null)
                    {
                        //Verificar que la columna donde se encuentran los controles dinámicos no esté vacía

                        if (fila["Programado"] != null)
                        {
                            using (LinkButton lkbEditarAnticipoProgramado = (LinkButton)e.Row.FindControl("lkbEditarAnticipoProgramado"),
                                lkbEliminarAnticipoProgramado = (LinkButton)e.Row.FindControl("lkbEliminarAnticipoProgramado"),
                                lkbEditarAsignacionDieselProgramado = (LinkButton)e.Row.FindControl("lkbEditarAsignacionDieselProgramado"))
                            {
                                lkbEditarAsignacionDieselProgramado.Visible = false;

                                if (lkbEditarAnticipoProgramado != null && lkbEliminarAnticipoProgramado != null)
                                {
                                    //Validando que la fila corresponda a un Anticipo:
                                    // ("1"): Programado
                                    // ("2"): Rechazado
                                    // ("3"): IAVE
                                    // Then: Normal
                                    //Si es un Anticipo Programado
                                    if (Convert.ToString(fila["Programado"]).Equals("1"))
                                    {
                                        //Coloreando fila de verde por ser Anticipo Programado
                                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#9FF781");
                                        //Validamos si el estatus del registro está Por Liquidar o Liquidado para inhabilitar su edición y eliminación
                                        if (Convert.ToString(fila["Estatus"]).Equals("Por Liquidar") || Convert.ToString(fila["Estatus"]).Equals("Liquidado"))
                                            lkbEditarAnticipoProgramado.Visible = lkbEliminarAnticipoProgramado.Visible = false;
                                        else
                                            lkbEditarAnticipoProgramado.Visible = lkbEliminarAnticipoProgramado.Visible = true;
                                    }
                                    //Si es un Anticipo Programado Rechazado
                                    else if (Convert.ToString(fila["Estatus"]).Equals("Rechazado"))
                                    {
                                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#F6CECE");
                                        lkbEditarAnticipoProgramado.Visible = lkbEliminarAnticipoProgramado.Visible = false;
                                    }
                                    else if (Convert.ToString(fila["Programado"]).Equals("3"))
                                    {
                                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#A9A9F5");
                                        lkbEditarAnticipoProgramado.Visible = lkbEliminarAnticipoProgramado.Visible = false;
                                    }
                                    else
                                    {
                                        if (Convert.ToString(fila["Concepto"]).Equals("Vale de Diesel"))
                                            lkbEditarAsignacionDieselProgramado.Visible = true;
                                        lkbEditarAnticipoProgramado.Visible = lkbEliminarAnticipoProgramado.Visible = false;
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento producido al pulsar el link Anticipos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAnticiposProgramados_OnClick(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;
            //Seleccionando fila actual
            //Controles.SeleccionaFila(gvServiciosContextual, sender, "lnk", false);
            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))

            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
            {
                //Si existen registros
                if (gvAnticipos.DataKeys.Count > 0)
                {
                    //Seleccionando la fila actual
                    Controles.SeleccionaFila(gvAnticipos, sender, "lnk", false);

                    //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                    resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                           Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));

                    //Si la Asignaciòn es correcta
                    if (resultado.OperacionExitosa)
                    {
                        //En base al comando definido para el botón
                        switch (b.CommandName)
                        {
                            case "Editar":
                                {

                                    //ALternando Ventana
                                    ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");
                                    ucDepositos.HabilitaConcepto = false;
                                    ucDepositos.MuestraSolicitar = true;
                                    //Inicializamos Control Depósito en Ediciòn
                                    ucDepositos.InicializaControl(Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"]), id_unidad, id_operador, id_tercero,
                                                Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdServicio"]), Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]), serv.id_cliente_receptor);

                                    //else
                                    //inicializaAnticiposProgramados(serv.id_servicio, Convert.ToInt32(gvAnticipos.SelectedValue));

                                    break;
                                }
                            case "Eliminar":
                                {
                                    resultado = ucDepositos.DeshabilitaDeposito(Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"]));

                                    if (resultado.OperacionExitosa)
                                    {
                                        VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedValue));
                                        //buscaServicios();
                                    }

                                    //Mostrando Mensaje
                                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

                                    break;
                                }
                            case "EditarValeDiesel":
                                {
                                    //Alternando Ventana: Mostramos Vista Vale
                                    ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");

                                    //Inicializamos Control Movimiento en vacio
                                    ucAsignacionDiesel.InicializaControlUsuario(Convert.ToInt32(gvAnticipos.SelectedValue), id_unidad, id_operador, id_tercero,
                                                  serv.id_servicio, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"]), Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]));
                                    break;
                                }
                        }
                    }
                    else
                        //Mostrando Mensaje
                        ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa el contenido de la página
        /// </summary>
        private void inicializaForma()
        {
            //Cargando catálogos requeridos
            cargaCatalogos();
            //Inicializando GridView de servicios
            Controles.InicializaGridview(gvServiciosContextual);
            Controles.InicializaGridview(gvAnticipos);
            Controles.InicializaGridview(gvServiciosContextual);

        }
        /// <summary>
        /// Realiza la carga de los catálogos utilizados en la forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño de Gridview (5-25)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewServicios, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewAnticipos, "", 18);
        }

        /// <summary>
        /// Realiza la búsqueda de servicios coincidentes a los filtros señalados
        /// </summary>
        private void buscaServicios()
        {
            //Declarando variables para rangos de fecha
            DateTime inicial_cita_carga = DateTime.MinValue;
            DateTime final_cita_carga = DateTime.MinValue;
            DateTime inicial_cita_descarga = DateTime.MinValue;
            DateTime final_cita_descarga = DateTime.MinValue;
            DateTime inicial_inicio_servicio = DateTime.MinValue;
            DateTime final_inicio_servicio = DateTime.MinValue;
            DateTime inicial_fin_servicio = DateTime.MinValue;
            DateTime final_fin_servicio = DateTime.MinValue;

            //Determinando que criterio será utilizado
            if (chkRangoFechas.Checked)
            {
                //Cita carga
                if (rdbCitaCarga.Checked)
                {
                    inicial_cita_carga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_carga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Cita Descarga
                else if (rdbCitaDescarga.Checked)
                {
                    inicial_cita_descarga = Convert.ToDateTime(txtFechaInicio.Text);
                    final_cita_descarga = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Inicio de Servicio
                else if (rdbInicioServicio.Checked)
                {
                    inicial_inicio_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_inicio_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
                //Fin de Servicio
                else
                {
                    inicial_fin_servicio = Convert.ToDateTime(txtFechaInicio.Text);
                    final_fin_servicio = Convert.ToDateTime(txtFechaFin.Text);
                }
            }

            //Realizando la carga de los servicios coincidentes
            using (DataTable mit = SAT_CL.Documentacion.Reportes.CargaReporteServiciosAnticiposProgramados(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, txtNoServicio.Text, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEconomico.Text, "ID:", 1)),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtOrigen.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDestino.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                inicial_cita_carga, final_cita_carga, inicial_cita_descarga, final_cita_descarga,
                inicial_inicio_servicio, final_inicio_servicio, inicial_fin_servicio, final_fin_servicio))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvServiciosContextual, mit, "Id-IdMovimiento-IdMovimientoAsignacion-IdRecurso-NoServicio", lblCriterioGridViewServicios.Text, true, 3);
                //Si no hay registros
                if (mit == null)
                    //Elimiando de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //Si existen registros, se sobrescribe
                else
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
            }
        }
        /// <summary>
        /// Método que carga el GridView de Anticipos.
        /// </summary>
        /// <param name="IdServicio"></param>
        private void VerAnticipos(int IdServicio)
        {
            //Realizando la carga de los servicios coincidentes
            using (DataTable mit = SAT_CL.EgresoServicio.AnticipoProgramado.CargaAnticiposProgramados(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, IdServicio))
            {
                //Cargando Gridview
                Controles.CargaGridView(gvAnticipos, mit, "Id-Estatus-Monto", lblCriterioGridViewAnticipos.Text, true, 3);

                //Si no hay registros
                if (mit == null)
                    //Elimiando de sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                //Si existen registros, se sobrescribe
                else
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                //Validando que existe la Tabla
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Calculamos Totales
                    gvAnticipos.FooterRow.Cells[6].Text = string.Format("{0:C2}", mit.Compute("SUM(Monto)", ""));
                }
                else
                {
                    //Calculamos Totales
                    gvAnticipos.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
                }
            }
        }
        /// <summary>
        /// Metodo Inicializa los anticipos programados
        /// </summary>
        private void cargaControlAnticipoProgramado()
        {
            string No_Servicio = "";
            //Asigna a control NoServicio el valor del queryString
            No_Servicio = Convert.ToString(Request.QueryString["idReg1"]);
            //Instanciando Servicio
            txtNoServicio.Text = No_Servicio.ToString();
            buscaServicios();
            //Validando que existan Registros en el GridView
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Selecciona la fila del gridview
                Controles.MarcaFila(gvServiciosContextual, No_Servicio, "NoServicio");
                //Si hay registros servicio mostrados
                if (gvServiciosContextual.DataKeys.Count > 0)
                {
                    //ALternando Ventana
                    ScriptServer.AlternarVentana(this.Page, "ProgramarAnticipo", "contenedorProgramarAnticipo", "ventanaProgramarAnticipo");

                    //Declaramos objeto Resultado
                    RetornoOperacion resultado = new RetornoOperacion();

                    //Declaramos variables
                    int id_unidad = 0; int id_operador = 0; int id_tercero = 0;
                    //Indicando que no es requerido mostrar solo concepto de anticipo a proveedor
                    ucDepositos.SoloAnticipoProveedor = false;

                    //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                    resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                           Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Movimiento
                        using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))
                        {
                            //Instanciando Servicio
                            using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                            {
                                ucDepositos.HabilitaConcepto = true;
                                ucDepositos.MuestraSolicitar = false;
                                //Inicializamos Control Depòsito en Edición
                                ucDepositos.InicializaControl(0, id_unidad, id_operador, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]), ser.id_servicio, mov.id_movimiento,
                                          Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]), ser.id_cliente_receptor);

                            }
                        }
                    }

                    //Sólo si hay error
                    if (!resultado.OperacionExitosa)
                        //Notificando que no es posible realizar esta acción para otro proveedor
                        ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }

        }

        #endregion

        #region Eventos UserControl "Diesel"

        /// <summary>
        /// Eventó Generado al Cancelar un Vale de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCancelarAsignacion(object sender, EventArgs e)
        {
            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                //VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                Controles.InicializaIndices(gvAnticipos);
            }
        }
        /// <summary>
        /// Evento generado al Guardar un Vale de Diesel
        /// </summary>
        protected void ucAsignacionDiesel_ClickGuardarAsignacion(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Declaramos variable para almacenar Recurso
            int id_unidad = 0;
            int id_operador = 0;
            int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto y obtenemos asignaciòn
            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                     Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
            //Insertamos Diesel
            resultado = ucAsignacionDiesel.GuardaDiesel();

            //Mostrando Mensaje
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

            //Validamos el tipo de operación, si es inserción o edición para cargar los datos correspondientes
            if (gvAnticipos.DataKeys.Count == 0)
            {
                //Alternando Ventana
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                //ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                //buscaServicios();
                //Controles.InicializaIndices(gvServiciosContextual);
                //Controles.InicializaIndices(gvAnticipos);
            }
            else
            {
                ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                VerAnticipos(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
            }
        }

        /// <summary>
        /// Evento Generado al Calculado Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCalculado1(object sender, EventArgs e)
        {
            InicializaInformacionDieselKms(ucAsignacionDiesel);
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(ucAsignacionDiesel, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");
        }

        /// <summary>
        /// Evento Generado al Calculado Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCostoDiesel(object sender, EventArgs e)
        {
            //InicializaInformacionDieselKms(ucAsignacionDiesel);
            wucCostoCombustible.InicializaControl(ucAsignacionDiesel.idEstacionCombustible, ucAsignacionDiesel.idTipoCombustible);
            alternaVentanaModal("CostoDiesel", this.Page);
            //Mostramos Ventana Modal
            //TSDK.ASP.ScriptServer.AlternarVentana(ucAsignacionDiesel, "CostoDiesel", "contenedorVentanaConfirmacionInformacionCostoDiesel", "ventanaConfirmacionInformacionCostoDiesel");
        }

        /// <summary>
        /// Evento generado al dar click en Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaInformacionCalculado_Click(object sender, EventArgs e)
        {
            //Mostramos Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(lnkCerrarVentanaInformacionCalculado, "Calculado", "contenedorVentanaConfirmacionInformacionCalculado", "ventanaConfirmacionInformacionCalculado");

        }
        /// <summary>
        /// Inicializamos Información de Diesel y Kms
        /// <
        /// </summary>
        private void InicializaInformacionDieselKms(System.Web.UI.Control control)
        {
            //Declaramos Resultado
            RetornoOperacion resultado = new RetornoOperacion(0);
            //Declarando Variables Auxiliares cap_unidad = 0;
            decimal cap_unidad = 0;
            int id_unidad = 0;
            DateTime fecha_carga = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro();
            //Instanciamos Diesel
            using (SAT_CL.EgresoServicio.AsignacionDiesel objAsignacionDiesel = new SAT_CL.EgresoServicio.AsignacionDiesel(ucAsignacionDiesel.idAsignacionDiesel))
            {
                //Validamos Vale
                if (objAsignacionDiesel.id_asignacion_diesel > 0)
                {
                    //Establecemos Fecha Carga
                    fecha_carga = objAsignacionDiesel.fecha_carga;
                }
            }

            //Instanciando Unidad
            using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(ucAsignacionDiesel.idUnidadDiesel))
            {
                //Validando que Exista la Unidad
                if (uni.id_unidad > 0)

                    //Obteniendo Capacidad de Combustible
                    cap_unidad = uni.capacidad_combustible;
                id_unidad = uni.id_unidad;


                //Obtenemos rendimiento
                decimal rendimiento = Convert.ToDecimal(Cadena.VerificaCadenaVacia(SAT_CL.Global.Referencia.CargaReferencia("0", 19, id_unidad, "Rendimiento Unidad", "Rendimiento"), "0"));
                //Si el Rendimiento No existe
                if (rendimiento == 0)
                {
                    //Si el Kilometraje es Diferente de O
                    if (uni.kilometraje_asignado != 0)
                    {
                        //Siel Combustible Asignado es Diferente de 0
                        if (uni.combustible_asignado != 0)
                        {
                            //Calculamos rendimiento
                            rendimiento = uni.kilometraje_asignado / uni.combustible_asignado;
                        }
                    }
                }

                //Inicializamos Valores
                lblCapacidadTanque.Text = cap_unidad.ToString() + "lts";
                lblRendimiento.Text = Cadena.TruncaCadena(rendimiento.ToString(), 5, "") + "kms/lts";
                lblFechaUltimaCarga.Text = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel).ToString("dd/MM/yy HH:mm");
                lblKmsUltimaCarga.Text = SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga).ToString() + "kms";
                //Validamos que sea diferente de 0 para la Division
                if (rendimiento > 0)
                {
                    lblCalculado.Text = Cadena.TruncaCadena((SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento)).ToString(), 5, "") + "lts";
                    lblSobrante.Text = Cadena.TruncaCadena((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))).ToString(), 5, "") + "lts";
                    lblAlcanceKms.Text = Cadena.TruncaCadena(((Convert.ToDecimal(cap_unidad) - (SAT_CL.Global.Unidad.ObtieneKmsTotalesUltimacarga(id_unidad, SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneUltimaFechaCargaDiesel(id_unidad, ucAsignacionDiesel.idAsignacionDiesel), fecha_carga) / Convert.ToDecimal(rendimiento))) * rendimiento).ToString(), 5, "") + "kms.";
                }
                else
                {
                    //Mostramos Resultado
                    resultado = new RetornoOperacion("El rendimiento debe ser Mayor a 0");
                }
            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(control, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }

        #endregion
        #region "VisualizacionKilometrajes"
        /// <summary>
        /// Evento Generado Visualizacion de Kilometros por Segmentos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucAsignacionDiesel_ClickCalculadoSegmentos(object sender, EventArgs e)
        {
            InicializaKilometrosSegmentos(ucAsignacionDiesel);
            //Mostramos Ventana Modal
            alternaVentanaModal("Segmentos", this.Page);
        }
        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModal1_Click(object sender, EventArgs e)
        {
            //Determinando botón pulsado
            LinkButton lkbCerrar = (LinkButton)sender;

            //De acuerdo al comando del botón
            switch (lkbCerrar.CommandName)
            {

                case "Segmentos":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("Segmentos", lkbCerrar);
                    ucAsignacionDiesel.InicializaControlUsuario();
                    break;
                case "Kilometros":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("Kilometros", lkbCerrar);
                    break;
                case "CostoDiesel":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("CostoDiesel", lkbCerrar);
                    break;
                case "AsignacionDieselProgramado":
                    alternaVentanaModal("AsignacionDieselProgramado", lkbCerrar);
                    break;
            }
        }
        /// <summary>
        /// Método abre ventanas modales de manera dinamica
        /// </summary>
        /// <param name="nombre_ventana"></param>
        /// <param name="control"></param>
        /// <param name="control"></param>
        private void alternaVentanaModal(string nombre_ventana, Control control)
        {
            //Determina que modal abrira
            switch (nombre_ventana)
            {
                case "Segmentos":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "Segmentos", "contenedorSegmentos");
                    break;
                case "Kilometros":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorVentanaKilometraje", "ventanaKilometraje");
                    break;
                case "CostoDiesel":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorCostoCombustible", "ventanaCostoCombustible");
                    break;
                case "AsignacionDieselProgramado":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                    break;
            }
        }
        /// <summary>
        /// Inicializamos Información de Diesel y Kms
        /// <
        /// </summary>
        private void InicializaKilometrosSegmentos(System.Web.UI.Control control)
        {
            //Validamos si el Depósito es para Operador
            //Instanciando Movimiento
            using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))
            //Instanciando Servicio
            using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
            {
                //Inicializamos Control Movimiento en vacio
                int id_recurso = MovimientoAsignacionRecurso.ObtienePrimerRecursoAsignado(mov.id_movimiento, MovimientoAsignacionRecurso.Tipo.Unidad);
                int id_ubicacion = ucAsignacionDiesel.id_ubicacion_actual;
                using (SAT_CL.Documentacion.Servicio doc = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                {
                    if (doc.habilitar)
                    {
                        cargaKilometrosDisel(doc.id_compania_emisor, doc.id_servicio, id_recurso, id_ubicacion, ucAsignacionDiesel.id_servicio_grid);
                    }
                }
            }
        }
        /// <summary>
        /// Click en algún Link de GV de Unidades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAcciones_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvServiciosContextual.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvServiciosContextual, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "CartaPorte":
                        //Invocando Método de Validación en la Carta Porte Vijera
                        if (gvServiciosContextual.SelectedIndex != -1)
                        {
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "PorteViajera", Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"])), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);

                        }
                        break;
                    case "HojaInstruccion":
                        //Invocando Método de Validación en la Carta Porte Vijera
                        if (gvServiciosContextual.SelectedIndex != -1)
                        {
                            string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/Despacho.aspx", "~/RDLC/Reporte.aspx");
                            //Instanciando nueva ventana de navegador para apertura de registro
                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "HojaDeInstruccion", Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"])), "PorteViajera", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        }
                        break;
                    case "Programacion":
                        //Invocando Método de Validación en la Carta Porte Vijera
                        if (gvServiciosContextual.SelectedIndex != -1)
                        {
                            //ALternando Ventana
                            ScriptServer.AlternarVentana(this.Page, "Programacion", "contenedorProgramacion", "ventanaProgramacion");
                        }
                        break;
                    case "GastosGenerales":
                        if (gvServiciosContextual.SelectedIndex != -1)
                        {
                            //Obtenemos registro completo del servicio
                            SAT_CL.Documentacion.Servicio Viaje = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["Id"]));
                            //Realizando la carga de los servicios coincidentes
                            using (DataTable mit = SAT_CL.EgresoServicio.AnticipoProgramado.CargaAnticiposProgramados(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Viaje.id_servicio))
                            {
                                //Si no hay registros
                                if (mit == null)
                                    //Mostrando Excepción
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El servicio no tiene gastos."), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                else
                                {
                                    //Generamos archivo PDF a descargar
                                    byte[] bytes = SAT_CL.Documentacion.Servicio.GeneraPDFGastosGenerales(Viaje.id_servicio);
                                    //Descargamos el archivo
                                    TSDK.Base.Archivo.DescargaArchivo(bytes, string.Format("Gastos_generales_servicio_{0}.pdf", Viaje.no_servicio), Archivo.ContentType.binary_octetStream);

                                }
                            }
                        }
                        break;
                }
            }
        }
        #endregion

        #region GridView "Factura"
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActualizacionkms_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice del GridView
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvActualizacionkms, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos del GridView "Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvActualizacionkms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        /// <summary>
        /// Método encargado de Obtener 
        /// </summary>
        /// <param name="id_compania"></param>
        /// <param name="id_servicio"></param>
        /// <param name="id_unidad"></param>
        /// <param name="id_ubicacion"></param>
        private void cargaKilometrosDisel(int id_compania, int id_servicio, int id_unidad, int id_ubicacion, int id_segmeto_servicio)
        {
            //Invoca al dataset para inicializar los valores del gridview si existe en relación a una orden de compra
            using (DataSet ds = SAT_CL.Global.Reporte.CargaKilometrajes(id_compania, id_servicio, id_unidad, id_ubicacion, id_segmeto_servicio))
            {
                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Añadiendo Tablas al DataSet de Session 
                    Controles.CargaGridView(gvActualizacionkms, ds.Tables["Table1"], "Id-IdKilometraje-IdServicio-IdUbicacionOrigen-IdUbicacionDestino", "", true, 1);
                }
                else
                {
                    //Inicializando GridViews
                    Controles.InicializaGridview(gvActualizacionkms);
                }
            }
        }
        /// <summary>
        /// Evento generado al dar click en Calcular
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbKmsActualizar_Click(object sender, EventArgs e)
        {
            //Validando que existen Registros
            if (gvActualizacionkms.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvActualizacionkms, sender, "lnk", false);

                //Determinando el comando del botón pulsado
                switch (((LinkButton)sender).CommandName)
                {
                    case "Editar":
                        //Determinando si existe o no un registro kilometraje
                        if (gvActualizacionkms.SelectedDataKey["IdKilometraje"].ToString() == "0")
                            //Inicializando Control
                            ucKilometraje.InicializaControlKilometraje(0, (((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario), Convert.ToInt32(gvActualizacionkms.SelectedDataKey["IdUbicacionOrigen"]), Convert.ToInt32(gvActualizacionkms.SelectedDataKey["IdUbicacionDestino"]));
                        //Si ya existe un kilometraje
                        else
                            ucKilometraje.InicializaControlKilometraje(Convert.ToInt32(gvActualizacionkms.SelectedDataKey["IdKilometraje"]), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Mostramos Ventana Modal
                        alternaVentanaModal("Kilometros", this.Page);
                        break;
                    case "VerPendientes":
                        //Cargando movimientos
                        //cargaMovimientosPendiente();
                        //Mostrar venta modal de movimientos pendientes
                        TSDK.ASP.ScriptServer.AlternarVentana(gvActualizacionkms, "MovimientosPendientes", "ventanaMovimientosPendientesModal", "ventanaMovimientosPendientes");
                        break;
                }
            }
        }
        /// <summary>
        /// Evento Producido al Guardar el Kilometraje
        /// </summary>
        protected void ucKilometraje_ClickGuardar(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            RetornoOperacion resultado = ucKilometraje.GuardaKilometraje();

            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Inicaliza kilometros
                InicializaKilometrosSegmentos(ucAsignacionDiesel);
                //Mostrando Ventana Modal
                alternaVentanaModal("Kilometros", this.Page);
                //Cargando movimientos
            }

            //Mostrando resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }


        /// <summary>
        /// Editar el campo de grid "Soporte Tecnico Detalles"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvServiciosContextual_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Cargando Menu Contextual
            Controles.CreaMenuContextual(e, "menuContext", "menuOptions", "MostrarMenu", true, true);
        }

        /// <summary>
        /// Evento Producido al Guardar el Costo Combustible
        /// </summary>
        protected void wucCostoCombustible_ClickGuardar(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            RetornoOperacion resultado = wucCostoCombustible.guardarPrecioCombustible();

            //Validando Operación Exitosa
            if (resultado.OperacionExitosa)
            {
                //Seleccionando fila actual
                Controles.SeleccionaFila(gvServiciosContextual, sender, "btn", false);

                //Declaramos variables
                int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

                //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
                resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                                    Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Alternando Ventana
                    alternaVentanaModal("CostoDiesel", this.Page);
                    //Instanciando Movimiento
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))

                    //Instanciando Servicio
                    using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                    {
                        //Inicializamos Control Movimiento en vacio
                        ucAsignacionDiesel.InicializaControlUsuario(0, id_unidad, id_operador, id_tercero, ser.id_servicio, mov.id_movimiento,
                                                                    Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]));
                    }
                }

                //Sólo si hay error
                if (!resultado.OperacionExitosa)
                    //Notificando que no es posible realizar esta acción para otro proveedor
                    ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Cerrar alguna ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaModalCostoCombustible_Click(object sender, EventArgs e)
        {

            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Seleccionando fila actual
            Controles.SeleccionaFila(gvServiciosContextual, sender, "btn", false);

            //Declaramos variables
            int id_unidad = 0; int id_operador = 0; int id_tercero = 0;

            //Validamos si el Tipo de Asignación actual es  el correcto, obtenemos recurso por asignar
            resultado = SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTipoAsignacion(out id_unidad, out id_operador, out id_tercero, Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]),
                                                Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdRecurso"]));
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Alternando Ventana
                //ScriptServer.AlternarVentana(this.Page, "AsignacionDieselProgramado", "contenedorAsignacionDieselProgramado", "ventanaAsignacionDieselProgramado");
                alternaVentanaModal("CostoDiesel", this.Page);
                //Instanciando Movimiento
                using (Movimiento mov = new Movimiento(Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimiento"])))

                //Instanciando Servicio
                using (SAT_CL.Documentacion.Servicio ser = new SAT_CL.Documentacion.Servicio(mov.id_servicio))
                {
                    //Inicializamos Control Movimiento en vacio
                    ucAsignacionDiesel.InicializaControlUsuario(0, id_unidad, id_operador, id_tercero, ser.id_servicio, mov.id_movimiento,
                                                                Convert.ToInt32(gvServiciosContextual.SelectedDataKey["IdMovimientoAsignacion"]));
                }
            }

            //Sólo si hay error
            if (!resultado.OperacionExitosa)
                //Notificando que no es posible realizar esta acción para otro proveedor
                ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        #endregion
    }
}