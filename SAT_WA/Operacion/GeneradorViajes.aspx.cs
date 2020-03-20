using SAT_CL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class GeneradorViajes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando PostBack
            if (!Page.IsPostBack)
                inicializaForma();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCliente_TextChanged(object sender, EventArgs e)
        {
            inicializaViajesMaestros();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAccion_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).CommandName)
            {
                case "Buscar":
                    {
                        cargaImportacionesAnteriores(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1, "0")));
                        break;
                    }
                case "Generar":
                    {
                        if (!Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                            creaViajesMasivos();
                        else
                            gestionaVentanaModal("Generar");
                        break;
                    }
                case "ConfirmarGeneracion":
                    {
                        creaViajesMasivos();
                        gestionaVentanaModal("Generar");
                        break;
                    }
                case "CerrarGeneracion":
                    {
                        gestionaVentanaModal("Generar");
                        break;
                    }
                case "Limpiar":
                    {
                        if (!Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                        {
                            Session["DS"] = null;
                            inicializaForma();
                            ScriptServer.MuestraNotificacion(this.Page, "La Página se Inicializo Correctamente", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                        else
                            gestionaVentanaModal("Limpiar");
                        break;
                    }
                case "ConfirmarLimpieza":
                    {
                        Session["DS"] = null;
                        inicializaForma();
                        gestionaVentanaModal("Limpiar");
                        ScriptServer.MuestraNotificacion(this.Page, "La Página se Inicializo Correctamente", ScriptServer.NaturalezaNotificacion.Informacion, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "CerrarLimpieza":
                    {
                        gestionaVentanaModal("Limpiar");
                        break;
                    }
                case "ConfirmarCancelacion":
                    {
                        //Validando Selección
                        if (gvViajesImportados.SelectedIndex != -1)
                        {
                            //Eliminando Registro
                            RetornoOperacion retorno = new RetornoOperacion();
                            using (SAT_CL.Documentacion.ServicioImportacionDetalle det = new SAT_CL.Documentacion.ServicioImportacionDetalle(Convert.ToInt32(gvViajesImportados.SelectedDataKey["Id"])))
                            {
                                if (det.habilitar)
                                    retorno = det.DeshabilitaServicioImportacionDetalle(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                else
                                    retorno = new RetornoOperacion("No se puede recuperar el Detalle");

                                if (retorno.OperacionExitosa)
                                {
                                    //Quitando Fila
                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvViajesImportados.SelectedDataKey["Id"].ToString()))
                                        dr.Delete();

                                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                                    cargaImportacionesAnteriores(det.id_servicio_importacion, 0, 0, 0);
                                    gestionaVentanaModal("CancelarViaje");
                                }
                                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
                case "CerrarCancelacion":
                    {
                        gestionaVentanaModal("CancelarViaje");
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnImportacion_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).CommandName)
            {
                case "ConfirmarImportacion":
                    {
                        if (gvImportacionesAnteriores.SelectedIndex != -1)
                        {
                            Session["DS"] = null;
                            //Método de Importación
                            importaViajesMasivos(Convert.ToInt32(gvImportacionesAnteriores.SelectedDataKey["Id"]));
                            gestionaVentanaModal("ImportacionViajes");
                        }
                        else
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Debe seleccionar una Importación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "ContinuarImportacion":
                    {
                        if (gvImportacionesAnteriores.SelectedIndex != -1)
                        {
                            Session["DS"] = null;
                            using (DataTable dtServiciosImportacion = SAT_CL.Documentacion.ServicioImportacionDetalle.ObtieneImportaciones(Convert.ToInt32(gvImportacionesAnteriores.SelectedDataKey["Id"])))
                            {
                                if (Validacion.ValidaOrigenDatos(dtServiciosImportacion))
                                {
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(1, "Los Viajes fueron cargados Exitosamente!", true), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.CargaGridView(gvViajesImportados, dtServiciosImportacion, "Id-IdServicio-Secuencia", "", true, 4);
                                    //Añadiendo Datos a Sesión
                                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosImportacion, "Table1");
                                }
                                else
                                {
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.InicializaGridview(gvViajesImportados);
                                    //Eliminando de Sesión
                                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                                }
                            }

                            gestionaVentanaModal("ImportacionViajes");
                        }
                        else
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Debe seleccionar una Importación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "CerrarImportacion":
                    {
                        Controles.InicializaIndices(gvViajesImportados);
                        gestionaVentanaModal("ImportacionViajes");
                        break;
                    }
                case "ConsultarImportacion":
                    {
                        if (gvImportacionesAnteriores.SelectedIndex != -1)
                        {
                            Session["DS"] = null;
                            using (DataTable dtServiciosImportacion = SAT_CL.Documentacion.ServicioImportacionDetalle.ObtieneImportaciones(Convert.ToInt32(gvImportacionesAnteriores.SelectedDataKey["Id"])))
                            {
                                if (Validacion.ValidaOrigenDatos(dtServiciosImportacion))
                                {
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(1, "Los Viajes fueron cargados Exitosamente!", true), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.CargaGridView(gvViajesImportados, dtServiciosImportacion, "Id-IdServicio-Secuencia", "", true, 4);
                                    //Añadiendo Datos a Sesión
                                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtServiciosImportacion, "Table1");
                                }
                                else
                                {
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.InicializaGridview(gvViajesImportados);
                                    //Eliminando de Sesión
                                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                                }
                            }
                            gestionaVentanaModal("ImportacionViajes");
                        }
                        else
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Debe seleccionar una Importación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCompletarViajes_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).CommandName)
            {
                case "CompletarViajes":
                    {
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                        {
                            int pendientes = ((from DataRow r in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Rows
                                               where Convert.ToInt32(r["IdServicio"]) == 0
                                               select r).ToList().Count);
                            int total = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Rows.Count;
                            lblConfirmacionViajes.Text = string.Format("Tiene '{0}' viajes pendientes de '{1}'", pendientes, total);
                            gestionaVentanaModal("CompletarViajes");
                        }
                        else
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No hay datos por importar"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "ConfirmarCompletarViajes":
                    {
                        if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                        { 
                            //Obteniendo Filas
                            RetornoOperacion retorno = new RetornoOperacion();
                            int idServicio = 0, idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario; 
                            string no_servicio = "", no_viaje = "";
                            List<RetornoOperacion> operaciones = new List<RetornoOperacion>();
                            foreach (DataRow dr in OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").Select("IdServicio = 0"))
                            {
                                //Inicializando Bloque Transaccional
                                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                    //Detalles
                                    using (SAT_CL.Documentacion.ServicioImportacionDetalle det = new SAT_CL.Documentacion.ServicioImportacionDetalle(Convert.ToInt32(dr["Id"])))
                                    using (SAT_CL.Documentacion.ServicioImportacion serv_imp = new SAT_CL.Documentacion.ServicioImportacion(det.id_servicio_importacion))
                                    {
                                        DateTime citaCarga = DateTime.MinValue, citaDescarga = DateTime.MinValue;
                                        DateTime.TryParse(dr["CitaCarga"].ToString(), out citaCarga);
                                        DateTime.TryParse(dr["CitaDescarga"].ToString(), out citaDescarga);
                                        retorno = confirmaViajeImportador(serv_imp.id_servicio_maestro, dr["NoViaje"].ToString(), citaCarga, citaDescarga,
                                                                    dr["Operador"].ToString(), dr["Unidad"].ToString(), serv_imp.id_transportista,
                                                                    dr["AutoTermino"].ToString().Equals("SI") ? true : false, idUsuario, out no_servicio, out no_viaje);
                                        if (retorno.OperacionExitosa)
                                        {
                                            idServicio = retorno.IdRegistro;
                                            retorno = det.ActualizaServicioProduccion(idServicio, citaCarga, citaDescarga, dr["Operador"].ToString(), dr["Unidad"].ToString(), no_viaje, idUsuario);
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Quitando Fila
                                                foreach (DataRow drs in ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + dr["Id"].ToString()))
                                                {
                                                    drs["Id"] = idServicio;
                                                    drs["NoServicio"] = no_servicio;
                                                    drs["NoViaje"] = no_viaje;
                                                    if (dr["AutoTermino"].ToString().Equals("SI"))
                                                        drs["Estatus"] = "Terminado";
                                                    else
                                                        drs["Estatus"] = "Iniciado";
                                                    drs["Operador"] = dr["Operador"].ToString();
                                                    drs["AutoTermino"] = dr["AutoTermino"].ToString();
                                                    drs["Unidad"] = dr["Unidad"].ToString();
                                                    dr["CitaCarga"] = citaCarga.ToString("dd/MM/yyyy HH:mm");
                                                    dr["CitaDescarga"] = citaDescarga.ToString("dd/MM/yyyy HH:mm");
                                                }

                                                ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();
                                                retorno = new RetornoOperacion(retorno.IdRegistro, string.Format("El Viaje '{0}' ha sido registrado", no_servicio), true);
                                                scope.Complete();
                                            }
                                        }
                                    }
                                    //Añadiendo a Lista Final
                                    operaciones.Add(retorno);
                                }
                            }

                            Controles.InicializaIndices(gvViajesImportados);
                            Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                            cargaImportacionesAnteriores(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1, "0")));
                            gestionaVentanaModal("CompletarViajes");
                            foreach (RetornoOperacion ret in operaciones)
                                ScriptServer.MuestraNotificacion(this.Page, ret.IdRegistro.ToString(), ret.Mensaje, ret.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }

                        break;
                    }
                case "CerrarCompletarViajes":
                    {
                        gestionaVentanaModal("CompletarViajes");
                        break;
                    }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlViajeMaestro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configurando Viaje Maestro
            configuraViajeMaestro(Convert.ToInt32(ddlViajeMaestro.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtTransportista_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            switch (((LinkButton)sender).CommandName)
            {
                case "ImportacionAnterior":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id-Sec");
                        break;
                    }
                case "ViajesImportados":
                    {
                        //Exportando Contenido
                        Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio");
                        break;
                    }
            }
        }

        #region Eventos GridView "Importación Anterior"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoIA_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controles.CambiaTamañoPaginaGridView(gvImportacionesAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoIA.SelectedValue), true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvImportacionesAnteriores_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoIA.Text = Controles.CambiaSortExpressionGridView(gvImportacionesAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvImportacionesAnteriores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Controles.CambiaIndicePaginaGridView(gvImportacionesAnteriores, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 4);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvImportacionesAnteriores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Validando Fila Obtenida
                if (row != null)
                {
                    //Obteniendo Controles
                    using (Image imgEstatusImportacion = (Image)e.Row.FindControl("imgEstatus"))
                    using (ImageButton imbSeleccionar = (ImageButton)e.Row.FindControl("imbSeleccionar"))
                    {
                        if (row["Estatus"] != null)
                        {
                            if (imgEstatusImportacion != null && imbSeleccionar != null)
                            {
                                switch (row["Estatus"].ToString())
                                {
                                    case "Registrado":
                                        {
                                            //Estatus
                                            imgEstatusImportacion.ImageUrl = "~/Image/Circle-Programado.png";
                                            imgEstatusImportacion.ToolTip = "Finalice la Gestión de sus Viajes";
                                            //Seleccionar
                                            imbSeleccionar.ImageUrl = "~/Image/Select.png";
                                            imbSeleccionar.CommandName = "CompletarImportacion";
                                            imbSeleccionar.ToolTip = "Seleccione la Importación que desee copiar";
                                            break;
                                        }
                                    case "En Progreso":
                                        {
                                            //Estatus
                                            imgEstatusImportacion.ImageUrl = "~/Image/Circle-IniciadoPendientes.png";
                                            imgEstatusImportacion.ToolTip = "Aún tiene viajes por Completar";
                                            //Seleccionar
                                            imbSeleccionar.ImageUrl = "~/Image/Select.png";
                                            imbSeleccionar.CommandName = "CompletarImportacion";
                                            imbSeleccionar.ToolTip = "Complete la Importación antes de copiar";
                                            break;
                                        }
                                    case "Terminado":
                                        {
                                            //Estatus
                                            imgEstatusImportacion.ImageUrl = "~/Image/Circle-Terminado.png";
                                            imgEstatusImportacion.ToolTip = "Ha completado sus viajes";
                                            //Seleccionar
                                            imbSeleccionar.ImageUrl = "~/Image/Select.png";
                                            imbSeleccionar.CommandName = "CopiarImportacion";
                                            imbSeleccionar.ToolTip = "Seleccione la Importación que desee copiar";
                                            break;
                                        }
                                    default:
                                        {
                                            //Estatus
                                            imgEstatusImportacion.ImageUrl = "~/Image/Circle-TermiandoPendientes.png";
                                            imgEstatusImportacion.ToolTip = "Busque sus Viajes en la sección de busqueda";
                                            //Seleccionar
                                            imbSeleccionar.ImageUrl = "~/Image/Select.png";
                                            imbSeleccionar.CommandName = "";
                                            imbSeleccionar.ToolTip = "Seleccione la Importación que desee copiar";
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSeleccionar_Click(object sender, ImageClickEventArgs e)
        {
            if (gvImportacionesAnteriores.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvImportacionesAnteriores, sender, "imb", false);
                //Validando Comando del Boton
                switch (((ImageButton)sender).CommandName)
                {
                    case "CopiarImportacion":
                        {
                            if (!Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                            {
                                //Configurando Controles
                                imgImportacion.ImageUrl = "~/Image/Exclamacion.png";
                                lblMensajeImportacion.Text = "Ingrese sus Citas de Carga/Descarga para copiar su importación";
                                pnlContinuarImp.Visible = btnContinuarImportacion.Visible = false;
                                btnConfirmarImportacion.Visible = btnConsultarImportacion.Visible = true;
                            }
                            else
                            {
                                //Configurando Controles
                                imgImportacion.ImageUrl = "~/Image/ExclamacionRoja.png";
                                lblMensajeImportacion.Text = "Tiene datos un importación cargada, aún asi ¿Desea hacer una importación nueva?";
                                pnlContinuarImp.Visible = btnContinuarImportacion.Visible = false;
                                btnConfirmarImportacion.Visible = btnConsultarImportacion.Visible = true;
                            }
                            break;
                        }
                    case "CompletarImportacion":
                        {
                            if (!Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1")))
                            {
                                //Configurando Controles
                                imgImportacion.ImageUrl = "~/Image/Exclamacion.png";
                                lblMensajeImportacion.Text = "Esta importación esta pendiente, presione 'Continuar' para completarla";
                                pnlContinuarImp.Visible = btnContinuarImportacion.Visible = 
                                btnConfirmarImportacion.Visible = true;
                                btnConsultarImportacion.Visible = false;
                            }
                            else
                            {
                                //Configurando Controles
                                imgImportacion.ImageUrl = "~/Image/ExclamacionRoja.png";
                                lblMensajeImportacion.Text = "Tiene datos en la pantalla, aún asi ¿Desea 'Continuar' o 'Confirmar' la importación?";
                                pnlContinuarImp.Visible = btnContinuarImportacion.Visible =
                                btnConfirmarImportacion.Visible = true;
                                btnConsultarImportacion.Visible = false;
                            }
                            break;
                        }
                }
                gestionaVentanaModal("ImportacionViajes");
            }
        }

        #endregion

        #region Eventos GridView "Gestión de Viajes"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajesImportados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Para columnas de datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Validando Fila Obtenida
                if (row != null)
                {
                    //Obteniendo Controles
                    using (ImageButton imbEstatusImportacion = (ImageButton)e.Row.FindControl("imbEstatusImportacion"),
                                imbEditarServicio = (ImageButton)e.Row.FindControl("imbEditarServicio"),
                                imbEliminarFila = (ImageButton)e.Row.FindControl("imbEliminarFila"))
                    using (ImageButton imbEstatusImportacionE = (ImageButton)e.Row.FindControl("imbEstatusImportacionE"),
                                imbConfirmarServicio = (ImageButton)e.Row.FindControl("imbConfirmarServicio"),
                                imbCancelarFila = (ImageButton)e.Row.FindControl("imbCancelarFila"))
                    {
                        if (row["Estatus"] != null)
                        {
                            //Controles Antes de Edición
                            if (imbEstatusImportacion != null && imbEditarServicio != null && imbEliminarFila != null)
                            {
                                switch (row["Estatus"].ToString())
                                {
                                    case "Sin Registrar":
                                        {
                                            /** CONFIGURANDO CONTROLES **/
                                            //Estatus
                                            imbEstatusImportacion.ImageUrl = "~/Image/TripPending.png";
                                            imbEstatusImportacion.ToolTip = "Pendiente por Registrar";
                                            imbEstatusImportacion.Enabled = false;
                                            imbEstatusImportacion.Visible = true;
                                            //Edición
                                            imbEditarServicio.ImageUrl = "~/Image/ManageEdit.png";
                                            imbEditarServicio.ToolTip = "Actualice los Datos de su Servicio";
                                            imbEditarServicio.CommandName = "Editar";
                                            imbEditarServicio.Enabled =
                                            imbEditarServicio.Visible = true;
                                            //Eliminación
                                            imbEliminarFila.ImageUrl = "~/Image/ManageMinus.png";
                                            imbEliminarFila.ToolTip = "Elimine el Servicio de la Lista";
                                            imbEliminarFila.CommandName = "Eliminar";
                                            imbEliminarFila.Enabled =
                                            imbEliminarFila.Visible = true;
                                            break;
                                        }
                                    case "Iniciado":
                                        {
                                            /** CONFIGURANDO CONTROLES **/
                                            //Estatus
                                            imbEstatusImportacion.ImageUrl = "~/Image/Circle-Iniciado.png";
                                            imbEstatusImportacion.ToolTip = "Su Viaje ha sido iniciado";
                                            imbEstatusImportacion.Enabled = false;
                                            imbEstatusImportacion.Visible = true;
                                            //Edición
                                            imbEditarServicio.ImageUrl = "~/Image/ManageLock.png";
                                            imbEditarServicio.ToolTip = "Sus datos han sido actualizados";
                                            imbEditarServicio.CommandName = "Bloquear";
                                            imbEditarServicio.Enabled = false;
                                            imbEditarServicio.Visible = true;
                                            //Eliminación
                                            imbEliminarFila.ImageUrl = "~/Image/ManageCancel.png";
                                            imbEliminarFila.ToolTip = "Sus datos han sido actualizados";
                                            imbEliminarFila.CommandName = "Bloquear";
                                            imbEliminarFila.Enabled = 
                                            imbEliminarFila.Visible = false;
                                            break;
                                        }
                                    case "Terminado":
                                        {
                                            /** CONFIGURANDO CONTROLES **/
                                            //Estatus
                                            imbEstatusImportacion.ImageUrl = "~/Image/Circle-Terminado.png";
                                            imbEstatusImportacion.ToolTip = "Su Viaje ha sido terminado";
                                            imbEstatusImportacion.Enabled = false;
                                            imbEstatusImportacion.Visible = true;
                                            //Edición
                                            imbEditarServicio.ImageUrl = "~/Image/ManageLock.png";
                                            imbEditarServicio.ToolTip = "Sus datos han sido actualizados";
                                            imbEditarServicio.CommandName = "Bloquear";
                                            imbEditarServicio.Enabled = false;
                                            imbEditarServicio.Visible = true;
                                            //Eliminación
                                            imbEliminarFila.ImageUrl = "~/Image/ManageCancel.png";
                                            imbEliminarFila.ToolTip = "Sus datos han sido actualizados";
                                            imbEliminarFila.CommandName = "Bloquear";
                                            imbEliminarFila.Enabled =
                                            imbEliminarFila.Visible = false;
                                            break;
                                        }
                                    default:
                                        {
                                            imbEstatusImportacion.Visible =
                                            imbEditarServicio.Visible =
                                            imbEliminarFila.Visible = false;
                                            break;
                                        }
                                }
                            }
                            //Controles En la Edición
                            else if (imbEstatusImportacionE != null && imbConfirmarServicio != null && imbCancelarFila != null)
                            {
                                switch (row["Estatus"].ToString())
                                {
                                    case "Sin Registrar":
                                        {
                                            /** CONFIGURANDO CONTROLES **/
                                            //Estatus
                                            imbEstatusImportacionE.ImageUrl = "~/Image/TripWorking.png";
                                            imbEstatusImportacionE.ToolTip = "Editando el Servicio";
                                            imbEstatusImportacionE.Enabled = false;
                                            imbEstatusImportacionE.Visible = true;
                                            //Confirmación
                                            imbConfirmarServicio.ImageUrl = "~/Image/ManageSave.png";
                                            imbConfirmarServicio.ToolTip = "Actualice los Datos de su Servicio";
                                            imbConfirmarServicio.CommandName = "Confirmar";
                                            imbConfirmarServicio.Enabled =
                                            imbConfirmarServicio.Visible = true;
                                            //Regresar
                                            imbCancelarFila.ImageUrl = "~/Image/ManageBack.png";
                                            imbCancelarFila.ToolTip = "Regrese a Editar otro Servicio";
                                            imbCancelarFila.CommandName = "Cancelar";
                                            imbCancelarFila.Enabled =
                                            imbCancelarFila.Visible = true;
                                            break;
                                        }
                                    default:
                                        {
                                            imbEstatusImportacionE.Visible =
                                            imbConfirmarServicio.Visible =
                                            imbCancelarFila.Visible = false;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBitacoraOperativa_Click(object sender, ImageClickEventArgs e)
        {
            if (gvViajesImportados.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvViajesImportados, sender, "imb", false);

                //Validando Comando del Boton
                switch (((ImageButton)sender).CommandName)
                {
                    case "Operativa":
                        {
                            if (Convert.ToInt32(gvViajesImportados.SelectedDataKey["IdServicio"]) > 0)
                            {
                                //Construyendo URL 
                                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/GeneradorViajes.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=1&idR=" + gvViajesImportados.SelectedDataKey["IdServicio"].ToString() + "&tB=Servicio");
                                //Definiendo Configuracion de la Ventana
                                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                                //Abriendo Nueva Ventana
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
                            }
                            else
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Servicio no esta registrado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            break;
                        }
                    case "Importacion":
                        {
                            if (Convert.ToInt32(gvViajesImportados.SelectedDataKey["IdServicio"]) > 0)
                            {
                                //Construyendo URL 
                                string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Operacion/GeneradorViajes.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=241&idR=" + gvViajesImportados.SelectedDataKey["Id"].ToString() + "&tB=ServicioImportacion");
                                //Definiendo Configuracion de la Ventana
                                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
                                //Abriendo Nueva Ventana
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
                            }
                            else
                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Servicio no esta registrado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
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
        protected void imbActualizarServicio_Click(object sender, ImageClickEventArgs e)
        {
            if (gvViajesImportados.DataKeys.Count > 0)
            {
                Controles.SeleccionaFila(gvViajesImportados, sender, "imb", true);
                int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;
                using (SAT_CL.Documentacion.ServicioImportacionDetalle det = new SAT_CL.Documentacion.ServicioImportacionDetalle(Convert.ToInt32(gvViajesImportados.SelectedDataKey["Id"])))
                {
                    if (det.habilitar)
                    {
                        //Validando Comando del Boton
                        switch (((ImageButton)sender).CommandName)
                        {
                            /** ACCIONES SERVICIO **/
                            //Sin Registrar
                            case "Editar":
                                {
                                    if (det.id_servicio == 0)
                                        Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                                    else
                                        Controles.InicializaIndices(gvViajesImportados);
                                    break;
                                }
                            case "Eliminar":
                                {
                                    if (det.id_servicio == 0)
                                        gestionaVentanaModal("CancelarViaje");
                                    else
                                    {
                                        Controles.InicializaIndices(gvViajesImportados);
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Servicio ya fue confirmado, no se puede editar desde aquí"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                    break;
                                }
                            case "Confirmar":
                                {
                                    //Instanciando Encabezado
                                    using (SAT_CL.Documentacion.ServicioImportacion serv_imp = new SAT_CL.Documentacion.ServicioImportacion(det.id_servicio_importacion))
                                    
                                    {
                                        if (serv_imp.habilitar)
                                        {
                                            if (det.id_servicio == 0)
                                            {
                                                //Obteniendo Valores de Controles
                                                using (TextBox txtNV = (TextBox)gvViajesImportados.SelectedRow.FindControl("txtNoViajeE"))
                                                using (TextBox txtCC = (TextBox)gvViajesImportados.SelectedRow.FindControl("txtCitaCargaE"))
                                                using (TextBox txtCD = (TextBox)gvViajesImportados.SelectedRow.FindControl("txtCitaDescargaE"))
                                                using (TextBox txtOp = (TextBox)gvViajesImportados.SelectedRow.FindControl("txtOperadorE"))
                                                using (TextBox txtUn = (TextBox)gvViajesImportados.SelectedRow.FindControl("txtUnidadE"))
                                                using (CheckBox chkTermino = (CheckBox)gvViajesImportados.SelectedRow.FindControl("chkTerminoAutoE"))
                                                {
                                                    if (txtNV != null && txtCC != null && txtCD != null && txtOp != null && txtUn != null && chkTermino != null)
                                                    {
                                                        DateTime citaCarga = DateTime.MinValue, citaDescarga = DateTime.MinValue;
                                                        DateTime.TryParse(txtCC.Text, out citaCarga);
                                                        DateTime.TryParse(txtCD.Text, out citaDescarga);

                                                        //Confirmando Servicio
                                                        RetornoOperacion retorno = new RetornoOperacion();
                                                        int idServicio = 0; string no_servicio = "", no_viaje = "";
                                                        //Inicializando Bloque Transaccional
                                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                        {
                                                            retorno = confirmaViajeImportador(serv_imp.id_servicio_maestro, txtNV.Text, citaCarga, citaDescarga,
                                                            txtOp.Text, txtUn.Text, serv_imp.id_transportista, chkTermino.Checked, idUsuario, out no_servicio, out no_viaje);
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                idServicio = retorno.IdRegistro;
                                                                retorno = det.ActualizaServicioProduccion(idServicio, citaCarga, citaDescarga, txtOp.Text, txtUn.Text, no_viaje, idUsuario);
                                                                if (retorno.OperacionExitosa)
                                                                {
                                                                    //Quitando Fila
                                                                    foreach (DataRow dr in ((DataSet)Session["DS"]).Tables["Table1"].Select("Id = " + gvViajesImportados.SelectedDataKey["Id"].ToString()))
                                                                    {
                                                                        dr["IdServicio"] = idServicio;
                                                                        dr["NoServicio"] = no_servicio;
                                                                        dr["NoViaje"] = no_viaje;
                                                                        if(chkTermino.Checked)
                                                                        dr["Estatus"] = "Terminado";
                                                                        else
                                                                        dr["Estatus"] = "Iniciado";
                                                                        dr["Operador"] = txtOp.Text;
                                                                        dr["AutoTermino"] = chkTermino.Checked ? "SI" : "NO";
                                                                        dr["Unidad"] = txtUn.Text;
                                                                        dr["CitaCarga"] = citaCarga.ToString("dd/MM/yyyy HH:mm");
                                                                        dr["CitaDescarga"] = citaDescarga.ToString("dd/MM/yyyy HH:mm");
                                                                    }

                                                                    ((DataSet)Session["DS"]).Tables["Table1"].AcceptChanges();
                                                                    scope.Complete();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Controles.InicializaIndices(gvViajesImportados);
                                                                Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                                                                cargaImportacionesAnteriores(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1, "0")));
                                                            }
                                                        }

                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            Controles.InicializaIndices(gvViajesImportados);
                                                            Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                                                            cargaImportacionesAnteriores(serv_imp.id_servicio_importacion, 0, 0, 0);
                                                        }
                                                        ScriptServer.MuestraNotificacion(this.Page, retorno.IdRegistro.ToString(), retorno.Mensaje, retorno.OperacionExitosa ? ScriptServer.NaturalezaNotificacion.Exito : ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Controles.InicializaIndices(gvViajesImportados);
                                                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Servicio ya fue confirmado, no se puede editar desde aquí"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                cargaImportacionesAnteriores(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1, "0")));
                                            }
                                        }
                                        else
                                        {
                                            Controles.InicializaIndices(gvViajesImportados);
                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede acceder al Encabezado de la Importación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            cargaImportacionesAnteriores(0, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)), Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1, "0")));
                                        }
                                    }

                                    break;
                                }
                            //Registrado
                            case "Bloquear":
                                {
                                    Controles.InicializaIndices(gvViajesImportados);
                                    ScriptServer.MuestraNotificacion(this.Page, "El Servicio ya fue confirmado, no se puede editar desde aquí", ScriptServer.NaturalezaNotificacion.Alerta, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    break;
                                }
                            case "Cancelar":
                                {
                                    //Inicializando Sin Edición
                                    Controles.InicializaIndices(gvViajesImportados);
                                    Controles.CargaGridView(gvViajesImportados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id-IdServicio-Secuencia", "", true, 4);
                                    break;
                                }
                        }
                    }
                    else
                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede acceder al Detalle"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }

        #endregion

        #endregion 

        #region Métodos

        /// <summary>
        /// 
        /// </summary>
        private void inicializaForma()
        {
            //Inicializando Fechas
            DateTime fecha_inicial = Fecha.ObtieneFechaEstandarMexicoCentro();
            if (fecha_inicial.Hour <= 12)
            {
                txtCitaCarga.Text = txtCitaCImp.Text = string.Format("{0:dd/MM/yyyy 08:00}", fecha_inicial);
                txtCitaDescarga.Text = txtCitaDImp.Text = string.Format("{0:dd/MM/yyyy 12:00}", fecha_inicial);
            }
            else
            {
                txtCitaCarga.Text = txtCitaCImp.Text = string.Format("{0:dd/MM/yyyy 12:00}", fecha_inicial);
                txtCitaDescarga.Text = txtCitaDImp.Text = string.Format("{0:dd/MM/yyyy 16:00}", fecha_inicial);
            }

            //Inicializando Controles
            Session["DS"] = null;
            txtCliente.Text = "";
            cargaCatalogos();
            Controles.InicializaGridview(gvImportacionesAnteriores);
            Controles.InicializaGridview(gvViajesImportados);
        }
        /// <summary>
        /// 
        /// </summary>
        private void cargaCatalogos()
        {
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoIA, "", 3182);
            inicializaViajesMaestros();
        }
        /// <summary>
        /// Método encargado de Cargar los Controles de Viajes Maestros
        /// </summary>
        private void inicializaViajesMaestros()
        {
            //Validando Datos del Cliente
            int idCliente = Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1));
            if (idCliente > 0)
            {
                //Cargando Maestros
                using (DataTable dtMaestros = CapaNegocio.m_capaNegocio.CargaCatalogo(191, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", idCliente, ""))
                {
                    if (Validacion.ValidaOrigenDatos(dtMaestros))
                    {
                        Controles.CargaDropDownList(ddlViajeMaestro, dtMaestros, "id", "descripcion");
                    }
                    else
                        Controles.InicializaDropDownList(ddlViajeMaestro, "El Cliente no posee Viajes Maestros");
                }
            }
            else
                Controles.InicializaDropDownList(ddlViajeMaestro, "El Cliente no posee Viajes Maestros");

            //Configurando Viaje Maestro
            configuraViajeMaestro(Convert.ToInt32(ddlViajeMaestro.SelectedValue));
        }
        /// <summary>
        /// Método encargado de Gestionar los Datos del Viaje Maestro
        /// </summary>
        /// <param name="id_viaje_maestro"></param>
        private void configuraViajeMaestro(int id_viaje_maestro)
        {
            using (SAT_CL.Documentacion.Servicio serv_m = new SAT_CL.Documentacion.Servicio(id_viaje_maestro))
            {
                if (serv_m.habilitar)
                {
                    using (SAT_CL.Global.Ubicacion carga = new SAT_CL.Global.Ubicacion(serv_m.id_ubicacion_carga))
                    using (SAT_CL.Global.Ubicacion descarga = new SAT_CL.Global.Ubicacion(serv_m.id_ubicacion_descarga))
                    {
                        if (carga.habilitar && descarga.habilitar)
                        {
                            txtOrigen.Text = string.Format("{0} ID:{1}", carga.descripcion, carga.id_ubicacion);
                            txtDestino.Text = string.Format("{0} ID:{1}", descarga.descripcion, descarga.id_ubicacion);
                        }
                        else { txtOrigen.Text = txtDestino.Text = ""; }
                    }
                }
                else { txtOrigen.Text = txtDestino.Text = ""; }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_serv_imp"></param>
        /// <param name="id_compania"></param>
        /// <param name="id_cliente"></param>
        /// <param name="id_transportista"></param>
        private void cargaImportacionesAnteriores(int id_serv_imp, int id_compania, int id_cliente, int id_transportista)
        {
            //Obteniendo Importaciones
            using (DataTable dtImportaciones = SAT_CL.Documentacion.ServicioImportacion.ObtieneImportaciones(id_compania, id_cliente, id_transportista, id_serv_imp))
            {
                if (Validacion.ValidaOrigenDatos(dtImportaciones))
                {
                    if (id_serv_imp > 0)
                        Controles.MarcaFila(gvImportacionesAnteriores, id_serv_imp.ToString(), "Id", "Id-Estatus", dtImportaciones, "", Convert.ToInt32(ddlTamanoIA.SelectedValue), true, 2);
                    else
                    {
                        Controles.CargaGridView(gvImportacionesAnteriores, dtImportaciones, "Id-Estatus", "", true, 2);
                        Controles.InicializaIndices(gvImportacionesAnteriores);
                    }
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportaciones, "Table");
                }
                else
                {
                    Controles.InicializaGridview(gvImportacionesAnteriores);
                    Controles.InicializaIndices(gvImportacionesAnteriores);
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_importacion"></param>
        private void importaViajesMasivos(int id_servicio_importacion)
        {
            int idServicioImportacion = 0;
            using (SAT_CL.Documentacion.ServicioImportacion si = new SAT_CL.Documentacion.ServicioImportacion(id_servicio_importacion))
            {
                if (si.habilitar)
                {
                    DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
                    DateTime citaCarga, citaDescarga;
                    DateTime.TryParse(txtCitaCImp.Text, out citaCarga);
                    DateTime.TryParse(txtCitaDImp.Text, out citaDescarga);
                    int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;

                    using (SAT_CL.Documentacion.Servicio serv_m = new SAT_CL.Documentacion.Servicio(si.id_servicio_maestro))
                    {
                        if (serv_m.habilitar)
                        {
                            using (DataTable dtDetalles = SAT_CL.Documentacion.ServicioImportacionDetalle.ObtieneImportaciones(si.id_servicio_importacion))
                            {
                                if (Validacion.ValidaOrigenDatos(dtDetalles))
                                {
                                    //Descripción del Maestro
                                    string maestro = SAT_CL.Global.Referencia.CargaReferencia(serv_m.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(serv_m.id_compania_emisor, 1, "Identificador", 0, "Servicio Maestro"));
                                    //Obteniendo Datos para Gestión
                                    string variable = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Importador | Cantidad Máx", serv_m.id_compania_emisor);
                                    string prefijo = Cadena.RegresaCadenaSeparada(variable, "|", 0, "");
                                    int cantidad = dtDetalles.Rows.Count;
                                    int cantidad_permitida = Convert.ToInt32(Cadena.RegresaCadenaSeparada(variable, "|", 1, "0"));
                                    if (cantidad <= cantidad_permitida)
                                    {
                                        RetornoOperacion retorno = new RetornoOperacion();
                                        
                                        //Inicializando Bloque Transaccional
                                        using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Insertando Encabezado
                                            retorno = SAT_CL.Documentacion.ServicioImportacion.InsertaServicioImportacion(serv_m.id_servicio,
                                                                        0, serv_m.id_compania_emisor, fecha_actual, citaCarga, citaDescarga,
                                                                        si.id_transportista, idUsuario);
                                            if (retorno.OperacionExitosa)
                                            {
                                                idServicioImportacion = retorno.IdRegistro;
                                                //Declarando Variables para Insertar en la Tabla temporal
                                                using (DataTable dtImportacion = new DataTable())
                                                {
                                                    //Creando Columnas Llave
                                                    dtImportacion.Columns.Add("Id", typeof(int));
                                                    dtImportacion.Columns.Add("IdServicio", typeof(int));

                                                    //Añadiendo columna para enumerar resultados
                                                    DataColumn cID = new DataColumn("Secuencia", typeof(int));
                                                    cID.AutoIncrement = true;
                                                    cID.AutoIncrementSeed = 1;
                                                    cID.AutoIncrementStep = 1;
                                                    dtImportacion.Columns.Add(cID);

                                                    //Creando Columnas Necesarias
                                                    dtImportacion.Columns.Add("NoServicio", typeof(string));
                                                    dtImportacion.Columns.Add("NoViaje", typeof(string));
                                                    dtImportacion.Columns.Add("Estatus", typeof(string));
                                                    dtImportacion.Columns.Add("Maestro", typeof(string));
                                                    dtImportacion.Columns.Add("CitaCarga", typeof(DateTime));
                                                    dtImportacion.Columns.Add("CitaDescarga", typeof(DateTime));
                                                    dtImportacion.Columns.Add("Operador", typeof(string));
                                                    dtImportacion.Columns.Add("Unidad", typeof(string));
                                                    dtImportacion.Columns.Add("AutoTermino", typeof(string));

                                                    //Inicializando Ciclo
                                                    int cont = 1;
                                                    foreach (DataRow dr in dtDetalles.Rows)
                                                    {
                                                        //Obteniendo Datos para gestión del No. Viaje
                                                        string periodo = "";
                                                        periodo = fecha_actual.Day <= 15 ? "1-15" : (DateTime.DaysInMonth(fecha_actual.Year, fecha_actual.Month) == 31 ? "16-31" : "16-30");
                                                        //Armando Datos
                                                        string no_viaje = string.Format("{0}/{1}/SC/{2}-{3:yyyyMMdd_HHmmss}-{4:000}", prefijo, periodo, maestro, Fecha.ObtieneFechaEstandarMexicoCentro(), cont);
                                                        no_viaje = dr["Operador"].ToString().Trim().Equals("") ? no_viaje.ToUpper().Replace("/CC/", "/SC/") : no_viaje.ToUpper().Replace("/SC/", "/CC/");

                                                        //Insertando Detalle en BD
                                                        retorno = SAT_CL.Documentacion.ServicioImportacionDetalle.InsertaServicioImportacionDetalle(idServicioImportacion, 0, no_viaje.ToUpper(), cont, citaCarga, citaDescarga, dr["Operador"].ToString().ToUpper(), dr["Unidad"].ToString().ToUpper(), idUsuario);
                                                        if (retorno.OperacionExitosa)
                                                        {
                                                            //Insertando Registros de Gestión
                                                            dtImportacion.Rows.Add(retorno.IdRegistro, 0, null, "", no_viaje, "Sin Registrar", maestro,
                                                                    citaCarga.ToString("dd/MM/yyyy HH:mm"), citaDescarga.ToString("dd/MM/yyyy HH:mm"), 
                                                                    dr["Operador"].ToString(), dr["Unidad"].ToString(), "SI");
                                                        }
                                                        else
                                                        {
                                                            dtImportacion.Rows.Clear();
                                                            break;
                                                        }
                                                        cont++;
                                                    }

                                                    if (retorno.OperacionExitosa && Validacion.ValidaOrigenDatos(dtImportacion))
                                                    {
                                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(1, "Los Viajes fueron cargados Exitosamente!", true), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                        Controles.CargaGridView(gvViajesImportados, dtImportacion, "Id-IdServicio-Secuencia", "", true, 4);
                                                        Controles.InicializaIndices(gvViajesImportados);
                                                        //Añadiendo Datos a Sesión
                                                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportacion, "Table1");
                                                        cargaImportacionesAnteriores(idServicioImportacion, si.id_compania, serv_m.id_cliente_receptor, si.id_transportista);
                                                        //Completando Transacción
                                                        scope.Complete();
                                                    }
                                                    else
                                                    {
                                                        Controles.InicializaGridview(gvViajesImportados);
                                                        Controles.InicializaIndices(gvViajesImportados);
                                                        //Eliminando de Sesión
                                                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Esta importación no tiene Viajes"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    Controles.InicializaGridview(gvViajesImportados);
                                    Controles.InicializaIndices(gvViajesImportados);
                                }
                            }
                            
                            
                        }
                        else
                        {
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recupear el Servicio Maestro"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            Controles.InicializaGridview(gvViajesImportados);
                            Controles.InicializaIndices(gvViajesImportados);
                        }
                    }
                }
                else
                {
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recupear la Importación"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    Controles.InicializaGridview(gvViajesImportados);
                    Controles.InicializaIndices(gvViajesImportados);
                }
            }
        }
        /// <summary>
        /// Método encargado de Generar la Plantilla de Viajes
        /// </summary>
        private void creaViajesMasivos()
        {
            //Obteniendo Valores
            int cantidad = 0;
            int.TryParse(txtCantidad.Text, out cantidad);
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            DateTime citaCarga, citaDescarga;
            DateTime.TryParse(txtCitaCarga.Text, out citaCarga);
            DateTime.TryParse(txtCitaDescarga.Text, out citaDescarga);
            int idUsuario = ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario;

            /** Validando Datos de Ingreso **/
            if (cantidad > 0)
            {
                if (citaCarga != DateTime.MinValue && citaCarga != DateTime.MinValue)
                {
                    /** VALIDANDO CONDICIONANTES: Cliente, Servicio, Transportista, Cantidad**/
                    using (SAT_CL.Global.CompaniaEmisorReceptor transportista = new SAT_CL.Global.CompaniaEmisorReceptor(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtTransportista.Text, "ID:", 1))))
                    {
                        if (transportista.habilitar)
                        {
                            using (SAT_CL.Documentacion.Servicio serv_m = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(ddlViajeMaestro.SelectedValue)))
                            {
                                if (serv_m.habilitar)
                                {
                                    if (serv_m.id_cliente_receptor == Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)))
                                    {
                                        //Descripción del Maestro
                                        string maestro = SAT_CL.Global.Referencia.CargaReferencia(serv_m.id_servicio, 1, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(serv_m.id_compania_emisor, 1, "Identificador", 0, "Servicio Maestro"));

                                        //Obteniendo Datos para Gestión
                                        string variable = CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Prefijo Importador | Cantidad Máx", serv_m.id_compania_emisor);
                                        string prefijo = Cadena.RegresaCadenaSeparada(variable, "|", 0, "");
                                        int cantidad_permitida = Convert.ToInt32(Cadena.RegresaCadenaSeparada(variable, "|", 1, "0"));
                                        if (cantidad <= cantidad_permitida)
                                        {
                                            RetornoOperacion retorno = new RetornoOperacion();
                                            int idServicioImportacion = 0;
                                            //Inicializando Bloque Transaccional
                                            using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                            {
                                                //Insertando Encabezado
                                                retorno = SAT_CL.Documentacion.ServicioImportacion.InsertaServicioImportacion(serv_m.id_servicio,
                                                                            0, serv_m.id_compania_emisor, fecha_actual, citaCarga, citaDescarga,
                                                                            transportista.id_compania_emisor_receptor, idUsuario);
                                                if (retorno.OperacionExitosa)
                                                {
                                                    idServicioImportacion = retorno.IdRegistro;
                                                    //Declarando Variables para Insertar en la Tabla temporal  
                                                    using (DataTable dtImportacion = new DataTable())
                                                    {
                                                        //Creando Columnas Llave
                                                        dtImportacion.Columns.Add("Id", typeof(int));
                                                        dtImportacion.Columns.Add("IdServicio", typeof(int));

                                                        //Añadiendo columna para enumerar resultados
                                                        DataColumn cID = new DataColumn("Secuencia", typeof(int));
                                                        cID.AutoIncrement = true;
                                                        cID.AutoIncrementSeed = 1;
                                                        cID.AutoIncrementStep = 1;
                                                        dtImportacion.Columns.Add(cID);

                                                        //Creando Columnas Necesarias
                                                        dtImportacion.Columns.Add("NoServicio", typeof(string));
                                                        dtImportacion.Columns.Add("NoViaje", typeof(string));
                                                        dtImportacion.Columns.Add("Estatus", typeof(string));
                                                        dtImportacion.Columns.Add("Maestro", typeof(string));
                                                        dtImportacion.Columns.Add("CitaCarga", typeof(DateTime));
                                                        dtImportacion.Columns.Add("CitaDescarga", typeof(DateTime));
                                                        dtImportacion.Columns.Add("Operador", typeof(string));
                                                        dtImportacion.Columns.Add("Unidad", typeof(string));
                                                        dtImportacion.Columns.Add("AutoTermino", typeof(string));

                                                        //Inicializando Ciclo
                                                        int cont = 1;
                                                        while (cont <= cantidad)
                                                        {
                                                            //Obteniendo Datos para gestión del No. Viaje
                                                            string periodo = "";
                                                            periodo = fecha_actual.Day <= 15 ? "1-15" : (DateTime.DaysInMonth(fecha_actual.Year, fecha_actual.Month) == 31 ? "16-31" : "16-30");
                                                            //Armando Datos
                                                            string no_viaje = string.Format("{0}/{1}/SC/{2}-{3:yyyyMMdd_HHmmss}-{4:000}", prefijo, periodo, maestro, Fecha.ObtieneFechaEstandarMexicoCentro(), cont);

                                                            //Insertando Detalle en BD
                                                            retorno = SAT_CL.Documentacion.ServicioImportacionDetalle.InsertaServicioImportacionDetalle(idServicioImportacion, 0, no_viaje, cont, citaCarga, citaDescarga, "", "", idUsuario);
                                                            if (retorno.OperacionExitosa)
                                                            {
                                                                //Insertando Registros de Gestión
                                                                dtImportacion.Rows.Add(retorno.IdRegistro, 0, null, "", no_viaje, "Sin Registrar", maestro,
                                                                        citaCarga.ToString("dd/MM/yyyy HH:mm"), citaDescarga.ToString("dd/MM/yyyy HH:mm"), "", "", "SI");
                                                                cont++;
                                                            }
                                                            else
                                                            {
                                                                dtImportacion.Rows.Clear();
                                                                break;
                                                            }
                                                        }

                                                        if (retorno.OperacionExitosa && Validacion.ValidaOrigenDatos(dtImportacion))
                                                        {
                                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(1, "Los Viajes fueron cargados Exitosamente!", true), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                            Controles.CargaGridView(gvViajesImportados, dtImportacion, "Id-IdServicio-Secuencia", "", true, 4);
                                                            Controles.InicializaIndices(gvViajesImportados);
                                                            //Añadiendo Datos a Sesión
                                                            Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtImportacion, "Table1");
                                                            //Completando Transacción
                                                            scope.Complete();
                                                        }
                                                        else
                                                        {
                                                            Controles.InicializaGridview(gvViajesImportados);
                                                            Controles.InicializaIndices(gvViajesImportados);
                                                            //Eliminando de Sesión
                                                            Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion(string.Format("Solo puede importar hasta '{0}' por vez", cantidad_permitida)), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                            Controles.InicializaGridview(gvViajesImportados);
                                            Controles.InicializaIndices(gvViajesImportados);
                                        }
                                    }
                                    else
                                    {
                                        ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Cliente no coincide con el servicio"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        Controles.InicializaGridview(gvViajesImportados);
                                        Controles.InicializaIndices(gvViajesImportados);
                                    }
                                }
                                else
                                {
                                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("El Viaje Maestro no es valido"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    Controles.InicializaGridview(gvViajesImportados);
                                    Controles.InicializaIndices(gvViajesImportados);
                                }
                            }
                        }
                        else
                        {
                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Ingrese una Transportista"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                            Controles.InicializaGridview(gvViajesImportados);
                            Controles.InicializaIndices(gvViajesImportados);
                        }
                    }
                }
                else
                {
                    ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Ingrese Citas validas"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                    Controles.InicializaGridview(gvViajesImportados);
                    Controles.InicializaIndices(gvViajesImportados);
                }
            }
            else
            {
                ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("Ingrese una cantidad valida"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                Controles.InicializaGridview(gvViajesImportados);
                Controles.InicializaIndices(gvViajesImportados);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_servicio_m"></param>
        /// <param name="no_viaje"></param>
        /// <param name="cita_carga"></param>
        /// <param name="cita_descarga"></param>
        /// <param name="operador"></param>
        /// <param name="unidad"></param>
        /// <param name="id_usuario"></param>
        /// <returns></returns>
        private RetornoOperacion confirmaViajeImportador(int id_servicio_m, string no_viaje, DateTime cita_carga, DateTime cita_descarga,
                                                    string operador, string unidad, int id_transportista, bool termino_auto, int id_usuario,
                                                    out string no_servicio, out string no_viaje_ref)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            DateTime fec_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            no_servicio = "";
            using (SAT_CL.Documentacion.Servicio serv_m = new SAT_CL.Documentacion.Servicio(id_servicio_m))
            {
                //Inicializando Bloque Transacional
                using (TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    no_viaje_ref = operador.Trim().Equals("") ? no_viaje.ToUpper().Replace("/CC/", "/SC/") : no_viaje.ToUpper().Replace("/SC/", "/CC/");
                    //Actualizando Registro
                    int idServicio = 0;
                    retorno = serv_m.CopiarServicio(cita_carga, cita_descarga, no_viaje_ref.ToUpper(), "", id_usuario);
                    if (retorno.OperacionExitosa)
                    {
                        idServicio = retorno.IdRegistro;
                        using (SAT_CL.Despacho.Movimiento mov1 = new SAT_CL.Despacho.Movimiento(idServicio, 1))
                        {
                            if (mov1.habilitar)
                            {
                                retorno = SAT_CL.Despacho.MovimientoAsignacionRecurso.InsertaMovimientoAsignacionRecursoParaDespacho(mov1.id_compania_emisor,
                                                                    mov1.id_movimiento, mov1.id_parada_origen, SAT_CL.Despacho.EstanciaUnidad.Tipo.Operativa,
                                                                    SAT_CL.Despacho.EstanciaUnidad.TipoActualizacionInicio.Manual,
                                                                    SAT_CL.Despacho.MovimientoAsignacionRecurso.Tipo.Tercero, 0, id_transportista, id_usuario);
                                if (retorno.OperacionExitosa)
                                    retorno = new RetornoOperacion(idServicio);
                            }
                            else
                                retorno = new RetornoOperacion("No se puede recuperar el 1er Movimiento");
                        }

                        if (retorno.OperacionExitosa)
                        {
                            //Instanciando Servicio
                            using (SAT_CL.Documentacion.Servicio serv_new = new SAT_CL.Documentacion.Servicio(idServicio))
                            {
                                if (serv_new.habilitar)
                                {
                                    no_servicio = serv_new.no_servicio;

                                    //Obteniendo Tipos de Referencia (Operador|Unidad)
                                    int idTipoOp = SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(serv_new.id_compania_emisor, 1, "Operador", 0, "Referencia de Viaje"),
                                        idTipoUn = SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(serv_new.id_compania_emisor, 1, "Placas Tractor", 0, "Referencia de Viaje");

                                    if (idTipoOp > 0)
                                    {
                                        retorno = SAT_CL.Global.Referencia.InsertaReferencia(idServicio, 1, idTipoOp, operador, fec_actual, id_usuario);
                                        if (retorno.OperacionExitosa)
                                            retorno = new RetornoOperacion(idServicio);
                                    }

                                    if (retorno.OperacionExitosa)
                                    {
                                        if (idTipoUn > 0)
                                        {
                                            retorno = SAT_CL.Global.Referencia.InsertaReferencia(idServicio, 1, idTipoUn, unidad, fec_actual, id_usuario);
                                            if (retorno.OperacionExitosa)
                                                retorno = new RetornoOperacion(idServicio);
                                        }
                                    }
                                    
                                    if (retorno.OperacionExitosa)
                                        //Terminando Servicio
                                        retorno = SAT_CL.Documentacion.Servicio.TerminoAutomaticoServicioTercero(idServicio, id_usuario, termino_auto);
                                }
                                else
                                    retorno = new RetornoOperacion("No se puede recuperar el Servicio");
                            }
                        }

                        //Validación Final
                        if (retorno.OperacionExitosa)
                        {
                            retorno = new RetornoOperacion(idServicio);
                            scope.Complete();
                        }
                    }
                }
                return retorno;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comando"></param>
        private void gestionaVentanaModal(string comando)
        {
            switch (comando)
            {
                case "Limpiar":
                    ScriptServer.AlternarVentana(this.Page, this.Page.GetType(), comando, "confirmacionVentanaConfirmacionLimpieza", "ventanaConfirmacionLimpieza");
                    break;
                case "CancelarViaje":
                    ScriptServer.AlternarVentana(this.Page, this.Page.GetType(), comando, "confirmacionVentanaConfirmacionCancelacion", "ventanaConfirmacionCancelacion");
                    break;
                case "Generar":
                    ScriptServer.AlternarVentana(this.Page, this.Page.GetType(), comando, "confirmacionVentanaConfirmacionGeneracion", "ventanaConfirmacionGeneracion");
                    break;
                case "ImportacionViajes":
                    ScriptServer.AlternarVentana(this.Page, this.Page.GetType(), comando, "confirmacionVentanaGestionImportacion", "ventanaGestionImportacion");
                    break;
                case "CompletarViajes":
                    ScriptServer.AlternarVentana(this.Page, this.Page.GetType(), comando, "confirmacionVentanaCompletarViajes", "ventanaCompletarViajes");
                    break;
            }
        }

        #endregion
    }
}