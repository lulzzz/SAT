using System;
using System.Data;
using SAT_CL.Despacho;
using SAT_CL.Bancos;
using TSDK.ASP;
using System.Transactions;
using TSDK.Datos;
using TSDK.Base;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAT.Soporte
{
    public partial class CambioOperador : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se haya Producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Inicialización de Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Buscando movimientos coincidentes con filtros
            buscaMovimiento();
        }
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
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvDepositos);
            TSDK.ASP.Controles.InicializaGridview(gvRecursos);

            //Inicializando Fechas
            //txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            //txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            ////Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);

        }
        /// <summary>
        /// Realiza la búsqueda de movimientos
        /// </summary>
        private void buscaMovimiento()
        {//Validnado que exista un dato para filtrar
            if (txtNoServicio.Text != "" || txtNoViaje.Text != "")
            {//Instanciando clase Servicio
                using (SAT_CL.Documentacion.Servicio s = new SAT_CL.Documentacion.Servicio(txtNoServicio.Text, txtNoViaje.Text, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                {//Validando que el servicio este terminado
                    if (s.estatus == SAT_CL.Documentacion.Servicio.Estatus.Terminado)
                    {
                        //Obteniendo Movimientos
                        using (DataTable dtMovimientos = SAT_CL.Documentacion.Reportes.BuscarMovimiento(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                Convert.ToInt32(Cadena.RegresaElementoNoVacio(txtNoServicio.Text, "0")), txtNoViaje.Text, 0.00m, 0, 0))
                        {
                            //Validando que Existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtMovimientos))
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvMovimientos, dtMovimientos, "NoMovimiento-NoServicio", "", true, 1);

                                //Añadiendo a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtMovimientos, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvMovimientos);

                                //Eliminando de Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }//Obteniendo Recursos
                        using (DataTable dtRecursos = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignacionesServicio(s.id_servicio))
                        {
                            //Validando que Existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtRecursos))
                            {
                                TSDK.ASP.Controles.CargaGridView(gvRecursos, dtRecursos, "Id", "", true, 1);
                                //Añadiendo a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtRecursos, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvRecursos);
                                //Eliminando de Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }//Obteniendo Depositos
                        using (DataTable dtDepositos = SAT_CL.EgresoServicio.Reportes.CargaAnticiposRecursoServicio(0, 0, 0, s.id_servicio))
                        {
                            //Validando que Existan Registros
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDepositos))
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.CargaGridView(gvDepositos, dtDepositos, "Id", "", true, 1);
                                //Añadiendo a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDepositos, "Table");
                            }
                            else
                            {
                                //Cargando GridView
                                TSDK.ASP.Controles.InicializaGridview(gvDepositos);
                                //Eliminando de Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                            }
                        }//Visualiza los datos
                        CargarDatos();
                    }
                    else//Muestra mensaje
                    ScriptServer.MuestraNotificacion(this.Page, "Su viaje no esta terminado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
   
                }                
            }
            else//Muestra mensaje
                ScriptServer.MuestraNotificacion(this.Page, "Debe ingresar algun dato.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Realiza el cambio de operador
        /// </summary>
        public RetornoOperacion CambiaOperador()
        {
            RetornoOperacion retorno = new RetornoOperacion();
            //Creamos lita de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene registros
            if (gvMovimientos.DataKeys.Count > 0)
            {              
                //Obtiene filas seleccionadas 
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                //Verificando que existan filas seleccionadas
                if (selected_rows.Length != 0)
                {
                    foreach (GridViewRow row in selected_rows)
                    {
                        gvMovimientos.SelectedIndex = row.RowIndex;
                        using (DataTable dtAsignaciones = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignaciones(Convert.ToInt32(gvMovimientos.SelectedDataKey["NoMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado))
                        {
                            if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                            {
                                List<DataRow> operadores = (from DataRow op in dtAsignaciones.AsEnumerable()
                                                            where Convert.ToInt32(op["IdTipoAsignacion"]) == 2
                                                            select op).ToList();
                                if (operadores.Count > 0)
                                {
                                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                    {
                                        foreach (DataRow dr in operadores)
                                        {
                                            using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = new SAT_CL.Despacho.MovimientoAsignacionRecurso(Convert.ToInt32(dr["Id"])))
                                            {
                                                if (mar.habilitar)
                                                {
                                                    retorno = mar.ActualizaOperadorMovimientoAsignacionRecurso(Convert.ToInt32((Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 1, "1"))), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                    if (retorno.OperacionExitosa)
                                                    {
                                                        using (DataTable dtAnticipos = SAT_CL.EgresoServicio.DetalleLiquidacion.CargaAnticipos(Convert.ToInt32(gvMovimientos.SelectedDataKey["NoMovimiento"])))
                                                        {
                                                            if (dtAnticipos != null)
                                                            {
                                                                if (Validacion.ValidaOrigenDatos(dtAnticipos))
                                                                {
                                                                    List<DataRow> depositos = (from DataRow dep in dtAnticipos.AsEnumerable()
                                                                                               where Convert.ToInt32(dep["IdTabla"]) == 51
                                                                                               select dep).ToList();
                                                                    if (depositos.Count > 0)
                                                                    {
                                                                        foreach (DataRow de in depositos)
                                                                        {
                                                                            using (SAT_CL.EgresoServicio.Deposito d = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(de["IdRegistro"])))
                                                                            {

                                                                                if (d.habilitar && d.objDetalleLiquidacion.habilitar)
                                                                                {
                                                                                    retorno = d.objDetalleLiquidacion.ActualizaOperadorDetalleLiquidacion(Convert.ToInt32((Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 1, "1"))), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    if (retorno.OperacionExitosa)
                                                                                    {
                                                                                        if (!d.bit_efectivo)
                                                                                        {
                                                                                            using (CuentaBancos objCuentaBancos = CuentaBancos.ObtieneCuentaBanco(76, Convert.ToInt32((Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 1, "1"))), CuentaBancos.TipoCuenta.Default))
                                                                                            {
                                                                                                retorno = d.EditaDepositoCuentaDestino(objCuentaBancos.id_cuenta_bancos, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                if (retorno.OperacionExitosa)
                                                                                                {
                                                                                                    using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(51, Convert.ToInt32(de["IdRegistro"])))
                                                                                                    {
                                                                                                        if (ei.habilitar)
                                                                                                        {
                                                                                                            retorno = ei.EditaCuentaDestino(objCuentaBancos.id_cuenta_bancos, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    retorno = new RetornoOperacion("No hay depositos para este movimiento");
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    List<DataRow> vales = (from DataRow val in dtAnticipos.AsEnumerable()
                                                                                           where Convert.ToInt32(val["IdTabla"]) == 69
                                                                                           select val).ToList();
                                                                    if (vales.Count > 0)
                                                                    {
                                                                        foreach (DataRow va in vales)
                                                                        {
                                                                            using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(Convert.ToInt32(va["IdRegistro"]), 69))
                                                                            {
                                                                                if (dl.habilitar)
                                                                                {
                                                                                    retorno = dl.ActualizaOperadorDetalleLiquidacion(Convert.ToInt32((Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 1, "1"))), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                    if (retorno.OperacionExitosa)
                                                                                    {
                                                                                        using (CuentaBancos objCuentaBancos = CuentaBancos.ObtieneCuentaBanco(76, Convert.ToInt32((Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 1, "1"))), CuentaBancos.TipoCuenta.Default))
                                                                                        {
                                                                                            //    retorno = d.EditaDepositoCuentaDestino(objCuentaBancos.id_cuenta_bancos, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                            //if (retorno.OperacionExitosa)
                                                                                            //{
                                                                                            using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(51, Convert.ToInt32(va["IdRegistro"])))
                                                                                            {
                                                                                                if (ei.habilitar)
                                                                                                {
                                                                                                    retorno = ei.EditaCuentaDestino(objCuentaBancos.id_cuenta_bancos, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        // }
                                                                                        //}                                                                            
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    retorno = new RetornoOperacion("No hay vales para este movimiento");
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    retorno = new RetornoOperacion("No hay anticipos para este movimiento");
                                                                    break;
                                                                }
                                                            }
                                                            //(retorno.OperacionExitosa)
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    retorno = new RetornoOperacion("No se puede recuperar la asignacion");
                                                    break;
                                                }
                                            }
                                        }
                                        if (retorno.OperacionExitosa)
                                        {
                                            trans.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    retorno = new RetornoOperacion("No hay operadores para este movimiento");
                                    break;
                                }
                            }
                        }
                    }
                }
                            
            }
            return retorno;
        }

        /// <summary>
        /// Realiza la carga de datos
        /// </summary>
        protected void CargarDatos()
        {//Instanciando clase de servicio
            using (SAT_CL.Documentacion.Servicio s = new SAT_CL.Documentacion.Servicio(txtNoServicio.Text,txtNoViaje.Text, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {//Validando que el servicio este terminado
                if (s.estatus == SAT_CL.Documentacion.Servicio.Estatus.Terminado)
                {
                    lblEstatus.Text = Convert.ToString(s.estatus);//Mostrando el estatus 
                }
                lblNoServicio.Text = s.no_servicio;//Mostrando numero de servicio
                lblFecDoc.Text = Convert.ToString(s.fecha_documentacion);//Mostrando fecha de documentacion
                //Instanciando clase compania emisor receptor
                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(s.id_cliente_receptor))
                {
                    lblCliente.Text = cer.nombre;//Mostrando nombre del cliente
                }//Intanciando clase de servicio despacho
                using (SAT_CL.Despacho.ServicioDespacho sd = new SAT_CL.Despacho.ServicioDespacho(ServicioDespacho.TipoCarga.IdServicio, s.id_servicio))
                {
                    lblIniViaje.Text = Convert.ToString(sd.fecha_inicio);//Mostrando fecha de inicio
                    lblFinViaje.Text = Convert.ToString(sd.fecha_fin);//Mostrando fecha de fin
                }
            }
        }
        #endregion

        #region Eventos GridView "Movimientos"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

        }

        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvMovimientos.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvMovimientos.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", chk.Checked);
                        break;
                }
                //CargarDatos();   
            }
        }
        #endregion

        #region Eventos GridView "Deposito y Vale"

        ///// <summary>
        ///// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Cambiando tamaño del GridView
        //    Controles.CambiaTamañoPaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        //}
        ///// <summary>
        ///// Evento Producido al Exportar al Contenido del GridView "Vales de Diesel"
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lnkExportar_Click(object sender, EventArgs e)
        //{
        //    //Exportando Contenido del GridView
        //    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        //}
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Deposito y Vale"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Deposito y Vale"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvDepositos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

        }

        ///// <summary>
        ///// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        //{   //Validando Si el GridView contiene Registros
        //    if (gvMovimientos.DataKeys.Count > 0)
        //    {   //Evalua el ID del CheckBox en el que se produce el cambio
        //        switch (((CheckBox)sender).ID)
        //        {   //Caso para el CheckBox "Todos"
        //            case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
        //                CheckBox chk = (CheckBox)gvMovimientos.HeaderRow.FindControl("chkTodos");
        //                //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
        //                TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", chk.Checked);
        //                break;
        //        }
        //        CargarDatos();
        //    }
        //}
        #endregion

        #region Eventos GridView "Recursos"

        ///// <summary>
        ///// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Cambiando tamaño del GridView
        //    Controles.CambiaTamañoPaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);
        //}
        ///// <summary>
        ///// Evento Producido al Exportar al Contenido del GridView "Vales de Diesel"
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void lnkExportar_Click(object sender, EventArgs e)
        //{
        //    //Exportando Contenido del GridView
        //    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        //}
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvRecursos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Recursos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecursos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvRecursos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

        }

        ///// <summary>
        ///// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        //{   //Validando Si el GridView contiene Registros
        //    if (gvMovimientos.DataKeys.Count > 0)
        //    {   //Evalua el ID del CheckBox en el que se produce el cambio
        //        switch (((CheckBox)sender).ID)
        //        {   //Caso para el CheckBox "Todos"
        //            case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
        //                CheckBox chk = (CheckBox)gvMovimientos.HeaderRow.FindControl("chkTodos");
        //                //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
        //                TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", chk.Checked);
        //                break;
        //        }
        //        CargarDatos();
        //    }
        //}
        #endregion

        #region VentanaModal "Cambio Operador"
        /// <summary>
        /// Evento Producido al dar click en boton Cambiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCambiar_Click(object sender, EventArgs e)
        {
            string OpeAnt = "";
            //Creamos lita de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene registros
            if (gvMovimientos.DataKeys.Count > 0)
            {//Obtiene filas seleccionadas
                using (SAT_CL.Documentacion.Servicio s = new SAT_CL.Documentacion.Servicio(txtNoServicio.Text, txtNoViaje.Text, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                //TODO: Obtener Detalles de liquidación del servicio y/ó movimientos
                using (DataTable dtDetalles = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneDetallesLiquidacionXServicioMovimiento(s.id_servicio))
                {
                    /*** Si existen registros de la tabla 79/114 (Pago/Comprobación) ***/

                    //Mando la escepción
                    //Personaliizar excepción: "El Servicio se encuentra pagado en la Liq. '{0}', elimine el pago para poderla cambiar"
                    if (Validacion.ValidaOrigenDatos(dtDetalles))
                    {
                        List<DataRow> detalles = (from DataRow dt in dtDetalles.AsEnumerable()
                                                  where Convert.ToInt32(dt["IdTabla"]) == 79 || Convert.ToInt32(dt["IdTabla"]) == 104
                                                  select dt).ToList();
                        int det = (from DataRow r in dtDetalles.Rows
                                   where r.Field<int>("IdTabla") == 79 || r.Field<int>("IdTabla") == 104
                                   select r.Field<int>("NoLiquidacion")).FirstOrDefault();
                        if (detalles.Count == 0)
                        {
                            GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                            //Verificando que existan filas seleccionadas
                            if (selected_rows.Length != 0)
                            {
                                foreach (GridViewRow row in selected_rows)
                                {//Instanciar Recurso del valor obtenido de la fila seleccionada
                                    gvMovimientos.SelectedIndex = row.RowIndex;
                                    using (DataTable dtAsignaciones = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignaciones(Convert.ToInt32(gvMovimientos.SelectedDataKey["NoMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado))
                                    {
                                        if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                                        {
                                            List<DataRow> operadores = (from DataRow op in dtAsignaciones.AsEnumerable()
                                                                        where Convert.ToInt32(op["IdTipoAsignacion"]) == 2 //&& Convert.ToInt32(op["IdEstatus"]) == 3
                                                                        select op).ToList();
                                            if (operadores.Count > 0)
                                            {
                                                foreach (DataRow dr in operadores)
                                                {
                                                    using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = new SAT_CL.Despacho.MovimientoAsignacionRecurso(Convert.ToInt32(dr["Id"])))
                                                    {//Validando que existen registros
                                                        if (mar.habilitar)
                                                        {//Instanciando Clase operador
                                                            using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(mar.id_recurso_asignado))
                                                            {
                                                                OpeAnt = o.nombre;//Mostrando nombre del operador anterior
                                                                inicializaCambioOperador(OpeAnt);//Inicializa ventana modal
                                                                                                 //Mostrando ventana modal correspondiente
                                                                ScriptServer.AlternarVentana(lnkCambiar, "CambioOpe", "cambioOperadorModal", "cambioOperador");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else//Mostrando Mensaje
                                                ScriptServer.MuestraNotificacion(this.Page, "El operador esta liquidado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                        }
                                    }
                                }
                            }
                        }
                        else
                            ScriptServer.MuestraNotificacion(this.Page, string.Format("El Servicio se encuentra pagado en la Liq. '{0}', elimine el pago para poderla cambiar", det), ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                    {
                        GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                        //Verificando que existan filas seleccionadas
                        if (selected_rows.Length != 0)
                        {
                            foreach (GridViewRow row in selected_rows)
                            {//Instanciar Recurso del valor obtenido de la fila seleccionada
                                gvMovimientos.SelectedIndex = row.RowIndex;
                                using (DataTable dtAsignaciones = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignaciones(Convert.ToInt32(gvMovimientos.SelectedDataKey["NoMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado))
                                {
                                    if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                                    {
                                        List<DataRow> operadores = (from DataRow op in dtAsignaciones.AsEnumerable()
                                                                    where Convert.ToInt32(op["IdTipoAsignacion"]) == 2 //&& Convert.ToInt32(op["IdEstatus"]) == 3
                                                                    select op).ToList();
                                        if (operadores.Count > 0)
                                        {
                                            foreach (DataRow dr in operadores)
                                            {
                                                using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = new SAT_CL.Despacho.MovimientoAsignacionRecurso(Convert.ToInt32(dr["Id"])))
                                                {//Validando que existen registros
                                                    if (mar.habilitar)
                                                    {//Instanciando Clase operador
                                                        using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(mar.id_recurso_asignado))
                                                        {
                                                            OpeAnt = o.nombre;//Mostrando nombre del operador anterior
                                                            inicializaCambioOperador(OpeAnt);//Inicializa ventana modal
                                                                                             //Mostrando ventana modal correspondiente
                                                            ScriptServer.AlternarVentana(lnkCambiar, "CambioOpe", "cambioOperadorModal", "cambioOperador");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else//Mostrando Mensaje
                                            ScriptServer.MuestraNotificacion(this.Page, "El operador esta liquidado", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else//Mostrando Mensaje
                ScriptServer.MuestraNotificacion(this.Page, "Debe Seleccionar un Movimiento", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento Producido al dar click en boton Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarCambioOperador_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(lkbCerrarCambioOperador, "CambioOpe", "cambioOperadorModal", "cambioOperador");
        }
        /// <summary>
        /// Evento Producido al abrir ventana modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inicializaCambioOperador(string OpeAnt)
        {
            lblAntOpe.Text = OpeAnt;
            txtNuevoOpe.Text = "";
        }
        /// <summary>
        /// Evento Producido al dar click en boton Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string OpeAnt = "";
            //Seleccionando fila
            GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
            //Verificando que existan filas seleccionadas
            if (selected_rows.Length != 0)
            {
                foreach (GridViewRow row in selected_rows)
                    gvMovimientos.SelectedIndex = row.RowIndex;
                using (DataTable dtAsignaciones = SAT_CL.Despacho.MovimientoAsignacionRecurso.CargaAsignaciones(Convert.ToInt32(gvMovimientos.SelectedDataKey["NoMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado))
                {
                    if (Validacion.ValidaOrigenDatos(dtAsignaciones))
                    {
                        List<DataRow> operadores = (from DataRow op in dtAsignaciones.AsEnumerable()
                                                    where Convert.ToInt32(op["IdTipoAsignacion"]) == 2 //&& Convert.ToInt32(op["IdEstatus"]) == 3
                                                    select op).ToList();
                        if (operadores.Count > 0)
                        {
                            foreach (DataRow dr in operadores)
                            {
                                using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = new SAT_CL.Despacho.MovimientoAsignacionRecurso(Convert.ToInt32(dr["Id"])))
                                    if (mar.habilitar)
                                    {
                                        using (SAT_CL.Global.Operador o = new SAT_CL.Global.Operador(mar.id_recurso_asignado))
                                        {
                                            OpeAnt = o.nombre + "Nuevo Operador:" + Cadena.RegresaCadenaSeparada(txtNuevoOpe.Text, "ID:", 0, "0");
                                        }
                                    }
                            }
                        }
                    }
                }
            }

                ucSoporte.InicializaControlUsuario(OpeAnt,3, Convert.ToString(gvMovimientos.SelectedDataKey["NoServicio"]));
            //inicializaCambioOperador(OpeAnt);
            //Mostrando ventana modal correspondiente
            ScriptServer.AlternarVentana(btnAcepta, "Soporte", "soporteTecnicoModal", "soporteTecnico");
            ScriptServer.AlternarVentana(btnAcepta, "CambioOpe", "cambioOperadorModal", "cambioOperador");
                

            
        }

        #endregion

        #region VentanaModal "Soporte"
        /// <summary>
        /// Evento Producido al dar click en boton Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucSoporteTecnico_ClickAceptarSoporte(object sender, EventArgs e)
        {
            //Realizando el guardado
            RetornoOperacion result = new RetornoOperacion();
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Realizando el guardado
                result = CambiaOperador();

                //Si no hay errores
                if (result.OperacionExitosa)
                {
                    ucSoporte.GuardaSoporte();
                }
                if(result.OperacionExitosa)
                {
                    trans.Complete();
                }
            }
            //Cerrando ventana modal
            ScriptServer.AlternarVentana(this, "Soporte", "soporteTecnicoModal", "soporteTecnico");

            ScriptServer.MuestraNotificacion(this.Page, result.Mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);
            buscaMovimiento();
            Controles.InicializaIndices(gvMovimientos);

        }
        /// <summary>
        /// Evento Producido al dar click en boton Cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarSoporte_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(lkbCerrarEliminacionVale, "Soporte", "soporteTecnicoModal", "soporteTecnico");
        }
        /// <summary>
        /// Evento Producido al dar click en boton Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucSoporteTecnico_ClickCancelarSoporte(object sender, EventArgs e)
        {
            //Cerrando ventana modal de edición
            ScriptServer.AlternarVentana(this, "Soporte", "soporteTecnicoModal", "soporteTecnico");
        }


        #endregion
    }
}