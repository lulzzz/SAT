using SAT_CL.Despacho;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucTerminoMovimientoVacio : System.Web.UI.UserControl
    {
        #region Atributos

        private int _id_movimiento;
        /// <summary>
        /// Almacena el Nombre del Contenedor
        /// </summary>
        public string Contenedor;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento generado al cargar la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando  PostBack
            if (!this.IsPostBack)
                InicializaControl();
        }
        /// <summary>
        /// Propiedad que establece el Orden de la Tabulación de los Controles
        /// </summary>
        public short TabIndex
        {
            set  
            {   //Asignando Tab
                ddlEstatus.TabIndex =
                ddlTipoAsignacion.TabIndex =
                txtValor.TabIndex =
                txtCiudadOrigenT.TabIndex = 
                txtCiudadDestino.TabIndex =
                chkRangoFechas.TabIndex =
                txtFechaInicio.TabIndex =
                txtFechaFin.TabIndex = 
                chkRangoFechasSalida.TabIndex =
                txtFechaInicioSalida.TabIndex = 
                txtFechaFinSalida.TabIndex =
                ddlTamanoMovimientos.TabIndex =
                lkbExportarMovimientos.TabIndex =
                gvMovimientos.TabIndex = value;
            }
            get { return ddlEstatus.TabIndex; }
        }
        /// <summary>
        /// Evento generando al dar clic en el Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbMovimientos_Click(object sender, EventArgs e)
        {
            //Recuperando el botón pulsado
            LinkButton b = (LinkButton)sender;

            //Declaramos Objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Seleccionando la fila actual
            Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

            //Validamos que existan Movimientos
            if (gvMovimientos.DataKeys.Count > 0)
            {
              
                //En base al comando definido para el botón
                switch (b.CommandName)
                {
                    case "Terminar":

                        //Instanciando movimiento vacío
                        using (SAT_CL.Despacho.Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["IdMovimiento"])))

                        //Instanciando Origen y Destino
                        using(Parada po = new Parada(mov.id_parada_origen), pd = new Parada(mov.id_parada_destino))
                        {
                            //Asignando Valores
                            lblOrigen.Text = po.descripcion;
                            lblDestino.Text = pd.descripcion;
                            lblFechaInicio.Text = po.fecha_salida.ToString("dd/MM/yyyy HH:mm");
                            chkFechaActual.Checked = true;
                            //Cargando Fecha Actual
                            cargaFechaActual();
                            //Registrando el script sólo para los paneles que producirán actualización del mismo
                            ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "AbreVentanaModal", "contenidoFechaTerminarMovimiento", "confirmacionFechaTerminarMovimiento");
                        }
                        break;

                    case "Eliminar":
                        //Registrando el script sólo para los paneles que producirán actualización del mismo
                        ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "AbreVentanaModal", "contenidoConfirmacionEliminarMovimiento", "confirmacionEliminarMovimiento");
                        break;
                    case "Reversa":
                        //Reversa Movimiento
                        reversaMovimiento();
                        break;
                }
            }
        }
        /// <summary>
        /// Evento generado al marcar o desmarcar el Control "Fecha Actual"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFechaActual_CheckedChanged(object sender, EventArgs e)
        {
            //Cargando Fecha Actual
            cargaFechaActual();
        }
        /// <summary>
        /// Evento generado al cerrar el Termino de Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarFechaTerminarMovimiento_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
            
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(uplkbCerrarFechaTerminarMovimiento, uplkbCerrarFechaTerminarMovimiento.GetType(), "AbreVentanaModal", "contenidoFechaTerminarMovimiento", "confirmacionFechaTerminarMovimiento");
        }
        /// <summary>
        /// Evento generado al aceptar la terminación del movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTerminarMovimiento_Click(object sender, EventArgs e)
        {
            //Evento generado al Terminar un Movimiento
            terminarMovimiento();
        }
        /// <summary>
        /// Evento generado al aceptar la eliminación del movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarEliminarMovimiento_Click(object sender, EventArgs e)
        {
            eliminarMovimiento();
        }
        /// <summary>
        /// Evento generado al Cancelar la Eliminación del Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarEliminarMovimiento_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCancelarEliminarMovimiento, upbtnCancelarEliminarMovimiento.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEliminarMovimiento", "confirmacionEliminarMovimiento");
        }
        /// <summary>
        /// Evento generado al Cancelar el Termino del Movimietno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarTerminarMovimiento_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(upbtnCancelarTerminarMovimiento, upbtnCancelarTerminarMovimiento.GetType(), "CerrarVentanaModal", "contenidoFechaTerminarMovimiento", "confirmacionFechaTerminarMovimiento");
        }

        /// <summary>
        /// Evento generado al cerrar la eliminación del evento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarEliminarMovimiento_Click(object sender, EventArgs e)
        {
            //Registrando el script sólo para los paneles que producirán actualización del mismo
            ScriptServer.AlternarVentana(uplkbCerrarEliminarMovimiento, uplkbCerrarEliminarMovimiento.GetType(), "CerrarVentanaModal", "contenidoConfirmacionEliminarMovimiento", "confirmacionEliminarMovimiento");
        }
        /// <summary>
        /// Evento generado al cambiar el Tamaño del Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoMovimientos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambia Tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"),
                                        Convert.ToInt32(ddlTamanoMovimientos.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Ordenando el GridView
            lblOrdenarMovimientos.Text = Controles.CambiaSortExpressionGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Prodicido al Cambiar el Indice de página del Gridview "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando el indice del GridView
            Controles.CambiaIndicePaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, false, 3);
        }
        /// <summary>
        /// Evento Generado al enlazar el Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Por cada Fila de Tipo Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Validamos existencia de Paradas
                if (gvMovimientos.DataKeys.Count > 0)
                {
                    //Buscamos Grid View de Eventos
                    using (LinkButton lkbTerminar = (LinkButton)e.Row.FindControl("lkbTerminar"),
                                      lkbReversa = (LinkButton)e.Row.FindControl("lkbReversaMov"))
                    {
                        using (Label lblEstatus = (Label)e.Row.FindControl("lblEstatus"))
                        {
                            //Validamos que exista la Fecha de Salida
                            if (lblEstatus.Text== "Iniciado")
                            {
                                lkbTerminar.Visible = true;
                                lkbReversa.Visible = false;
                            }
                            else
                            {
                                lkbTerminar.Visible = false;
                                lkbReversa.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento generado al exportar los Movimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarMovimientos_Click(object sender, EventArgs e)
        {
            //Exportando eventos
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"));
        }

        /// <summary>
        /// Evento producido al cambiar el tipo de asignación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoAsignacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Limpiamos controles
            txtValor.Text = "";
            string id_tipo = "";
            //Validamos Tipo de Asignación para mostrar el control correspondiente
            switch (ddlTipoAsignacion.SelectedValue)
            {
                //Unidad
                case "1":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Unidad";
                    id_tipo = "12";
                    break;
                //Operador
                case "2":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Operador";
                    id_tipo = "11";
                    break;
                //Tercero
                case "3":
                    //Asiganamos variable correspondientes
                    lblValor.Text = "Tercero";
                    id_tipo = "17";
                    break;
            }
            //Generamos Sript
            string script =
            @"<script type='text/javascript'>
              $('#" + this.txtValor.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=" + id_tipo + @" &param=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() + @"', appendTo: '" + this.Contenedor + @"'});
              </script>";

            //Registrando el script sólo para los paneles que producirán actualización del mismo
            System.Web.UI.ScriptManager.RegisterStartupScript(upddlTipoAsignacion,upddlTipoAsignacion.GetType(), "AutocompleteRecursos", script, false);
        }
        /// <summary>
        /// Evento generado al buscar los movimientos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarMovimientos_Click(object sender, EventArgs e)
        {
            //Carga Movimientos en Vacio
            cargaMovimientosVacio();
        }
        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de Llegada del Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechas_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkRangoFechas.Checked))
            {
                //Inicializamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:00";
                txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFechaInicio.Enabled = txtFechaFin.Enabled = chkRangoFechas.Checked;
        }
        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de Salida del Movimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkRangoFechasSalida_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkRangoFechasSalida.Checked))
            {
                //Inicializamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFechaInicioSalida.Text = primerdia.ToString("dd/MM/yyyy") + " 00:00";
                txtFechaFinSalida.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFechaInicioSalida.Enabled = txtFechaFinSalida.Enabled = chkRangoFechasSalida.Checked;
        }

        #endregion

        #region Métodos

        /// <summary>
        ///Carga Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño Movimientos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoMovimientos, "", 56);
            //Tipo Asignacion
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoAsignacion, "--TODOS--", 46);
            //Estatus del Movimiento
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "--TODOS--", 61);
        }
        /// <summary>
        ///Inicializamos Valores
        /// </summary>
        private void inicializaValores()
        {
            ddlEstatus.SelectedValue = "0";
            ddlTipoAsignacion.SelectedValue = "0";
            txtValor.Text = "";
            txtFechaInicio.Enabled = txtFechaInicioSalida.Enabled = chkRangoFechas.Checked = false;
           txtFechaFin.Enabled = txtFechaFinSalida.Enabled = chkRangoFechasSalida.Checked = false;
            DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
            DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
            txtFechaInicio.Text = primerdia.ToString("dd/MM/yyyy") + " 00:00";
            txtFechaFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
            txtFechaInicioSalida.Text = primerdia.ToString("dd/MM/yyyy") + " 00:00";
            txtFechaFinSalida.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";
            txtCiudadOrigenT.Text = "";
            txtCiudadDestino.Text = "";
        }
        /// <summary>
        /// Método encargado de cargar los Movimientos en Vacio
        /// </summary>
        private void cargaMovimientosVacio()
        {
            //Declaramos variables de Fechas 
            DateTime fechaInicioLlegada = DateTime.MinValue, fechaFinLlegada = DateTime.MinValue, fechaInicioSalida = DateTime.MinValue, fechaFinSalida = DateTime.MinValue;

            //De acuerdo al chek box de fechas de Registró
            if (chkRangoFechas.Checked)
            {
                //Declaramos variables de Fechas de Registró
                fechaInicioLlegada = Convert.ToDateTime(txtFechaInicio.Text);
                fechaFinLlegada = Convert.ToDateTime(txtFechaFin.Text);
            }

            //De acuerdo al chek box de fechas de Depósito
            if (chkRangoFechasSalida.Checked)
            {
                //Declaramos variables de Fechas de Depósito
                fechaInicioSalida = Convert.ToDateTime(txtFechaInicioSalida.Text);
                fechaFinSalida = Convert.ToDateTime(txtFechaFinSalida.Text);
            }

            //Obtenemos Movimientos en Vacio
            using (DataTable mit = SAT_CL.Despacho.Reporte.CargaMovimientoVacio(Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtCiudadOrigenT.Text, ':', 1), "0")),
                                 Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtCiudadDestino.Text, ':', 1), "0")), Convert.ToByte(ddlTipoAsignacion.SelectedValue),
                                  Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtValor.Text, ':', 1), "0")), Convert.ToByte(ddlEstatus.SelectedValue),
                                  fechaInicioLlegada, fechaFinLlegada, fechaInicioSalida, fechaFinSalida, this._id_movimiento, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Cargamos Grid View
                Controles.CargaGridView(gvMovimientos, mit, "IdMovimiento-FechaSalida", "", true, 1);

                //Valida Origen de Datos
                if (mit != null)
                
                    //Añadiendo Tabla a DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                else
                    //Eliminamos Tabla a DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Inicializamos Indices
            Controles.InicializaIndices(gvMovimientos);
        }

        /// <summary>
        /// Terminamos Movimiento
        /// </summary>
        private void terminarMovimiento()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //inicializando transacción
            using (System.Transactions.TransactionScope scope = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando movimiento vacío
                using (SAT_CL.Despacho.Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["IdMovimiento"])))
                {
                    //Si el movimiento se recuperó
                    if (mov.id_movimiento > 0)
                    {
                        //Instanciando parada de origen
                        using (SAT_CL.Despacho.Parada origen = new Parada(mov.id_parada_origen))
                        {
                            //Validamos fecha de salida de origen vs fecha llegada a destino
                            if (Convert.ToDateTime(txtFechaTerminar.Text).CompareTo(origen.fecha_salida) > 0)
                                //Terminamos Movimiento
                                resultado = mov.TerminaMovimientoVacio(chkFechaActual.Checked ? TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro() : Convert.ToDateTime(txtFechaTerminar.Text), EstanciaUnidad.TipoActualizacionInicio.Manual, ((Usuario)Session["usuario"]).id_usuario);
                            else
                                resultado = new RetornoOperacion(string.Format("La Llegada al Destino '{0:dd/MM/yyyy HH:mm}' debe ser Mayor a la Salida del Origen '{1:dd/MM/yyyy HH:mm}'", Convert.ToDateTime(txtFechaTerminar.Text), origen.fecha_salida));
                        }
                    }
                    //Si no hay movimiento
                    else
                        resultado = new RetornoOperacion(string.Format("Movimiento '{0}' no encontrado.", gvMovimientos.SelectedDataKey["IdMovimiento"]));
                }

                //Finalizando transacción
                if (resultado.OperacionExitosa)
                    scope.Complete();
            }


            //Validamos Resultado final
            if (resultado.OperacionExitosa)
            {
                //Cargamos Movimientos
                cargaMovimientosVacio();
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(btnAceptarTerminarMovimiento, btnAceptarTerminarMovimiento.GetType(), "CierreVentanaModal", "contenidoFechaTerminarMovimiento", "confirmacionFechaTerminarMovimiento");
            }

            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarTerminarMovimiento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Elimina Movimiento
        /// </summary>
        private void eliminarMovimiento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Movimiento
            using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedValue)))
            {
                //Validamos Estatus del Movimiento
                if ((Movimiento.Estatus)objMovimiento.id_estatus_movimiento == Movimiento.Estatus.Terminado)
                {
                    //Deshabilitamos Movimiento Vacio Terminado
                    resultado = objMovimiento.DeshabilitaMovimientoVacioTerminado(((Usuario)Session["usuario"]).id_usuario);
                }
                else
                    resultado = objMovimiento.DeshabilitaMovimientoVacioIniciado(((Usuario)Session["usuario"]).id_usuario);

            }
            //Validamos Resultado
            if (resultado.OperacionExitosa)
            {
                //Cargamos Movimientos
                cargaMovimientosVacio();
                //Registrando el script sólo para los paneles que producirán actualización del mismo
                ScriptServer.AlternarVentana(btnAceptarEliminarMovimiento, btnAceptarEliminarMovimiento.GetType(), "CierreVentanaModal", "contenidoConfirmacionEliminarMovimiento", "confirmacionEliminarMovimiento");

            }
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarEliminarMovimiento, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        /// <summary>
        /// Reversa Movimiento
        /// </summary>
        private void reversaMovimiento()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

                //Instanciamos Movimiento
                using (Movimiento objMovimiento = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedValue)))
                {
                    //Terminamos Movimiento
                    resultado = objMovimiento.ReversaTerminaMovimientoVacio(((Usuario)Session["usuario"]).id_usuario);
                }
                //Validamos Resultado
                if (resultado.OperacionExitosa)
                {
                    //Cargamos Movimientos
                    cargaMovimientosVacio();                  
                }
                //Mostrando resultado
                ScriptServer.MuestraNotificacion(gvMovimientos, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método encargado de Cargar la fecha Actual
        /// </summary>
        private void cargaFechaActual()
        {
            //Marcando Fecha Actual
            if (chkFechaActual.Checked)
            
                //Mostrando Fecha Actual
                txtFechaTerminar.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            else
                //Mostrando Fecha Actual
                txtFechaTerminar.Text = "";
        }
        /// <summary>
        /// Evento de inicialización de contenido de control
        /// </summary>
        public void InicializaControl()
        {
            //Carga catalogos
            cargaCatalogos();
            //Inicializamos Grid View
            Controles.InicializaGridview(gvMovimientos);
            inicializaValores();            
        }
        /// <summary>
        /// Evento de inicialización de contenido de control
        /// </summary>
        /// <param name="id_movimiento">Id de Movimiento en vacío que deberá ser buscado y seleccionado</param>
        public void InicializaControl(int id_movimiento)
        {
            //Inicializando control de usuario 
            InicializaControl();

            //Si hay que seleccionar un movimiento en particular
            if (id_movimiento > 0)
            {
                this._id_movimiento = id_movimiento;
                //Buscando movimiento en particular por su ID
                cargaMovimientosVacio();
            }
        }

        #endregion
    }
}