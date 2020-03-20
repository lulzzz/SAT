using System;
using System.Data;
using SAT_CL.EgresoServicio;
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
    public partial class DepositoTesoreria : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Si no es una recarga de página
            if(!this.IsPostBack)
            {
                //Inicializando la forma
                inicializaForma();
                //Asignamos Focus
                txtNoDeposito.Focus();
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }
        }
        /// <summary>
        /// Evento generado al dar clic en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Cargando Depositos
            buscaDepositos();
        }

        #region Eventos Depositos

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDepositos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Registros           
            Controles.CambiaTamañoPaginaGridView(gvDepositoTesoreria, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoDepositos.SelectedValue), true, 1);

            //Suma Totales
            sumaTotales();
        }

        /// <summary>
        /// Evento Producido al Cambiar el Orden de los Datos del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositoTesoreria_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvDepositoTesoreria.DataKeys.Count > 0)
            {

                //Cambiando Ordenamiento
                lblOrdenarDepositos.Text = Controles.CambiaSortExpressionGridView(gvDepositoTesoreria, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

                //Suma Totales
                sumaTotales();
            }
        }

        /// <summary>
        /// Evento Generado al cambiar indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDepositoTesoreria_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Aplicando paginacion al Grid View
            Controles.CambiaIndicePaginaGridView(gvDepositoTesoreria, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);

            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar un Reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Obteniendo Link
            LinkButton lnk = (LinkButton)sender;

            //Validando el Comando
            switch (lnk.CommandName)
            {
                case "Depositos":
                    //Exporta Grid View
                    Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                    break;
            }
        }

        /// <summary>
        /// Evento generado al dar Click en el Link Bitácora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClik_Bitacora(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvDepositoTesoreria.DataKeys.Count > 0)
            {
                //Selecionamo Fila
                Controles.SeleccionaFila(gvDepositoTesoreria, sender, "lnk", false);

                //Inicializa Bitácora
                InicializaBitacora(gvDepositoTesoreria.SelectedDataKey.Value.ToString(), "51", "Depósitos");
            }
        }

        /// <summary>
        /// Evento producido al marcar o desmarcar el filtrado por fechas de Registró
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkIncluir_CheckedChanged(object sender, EventArgs e)
        {
            //Si se marca la búsqueda por Fechas
            if (!(chkIncluir.Checked))
            {
                //Inicializamos Controles de Fechas
                DateTime primerdia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, 1);
                DateTime ultimoDia = new DateTime(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month, DateTime.DaysInMonth(TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Year, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().Month));
                txtFecIni.Text = primerdia.ToString("dd/MM/yyyy") + " 00:01";
                txtFecFin.Text = ultimoDia.ToString("dd/MM/yyyy") + " 23:59";

            }
            //Habilitación de cajas de texto para fecha
            txtFecIni.Enabled = txtFecFin.Enabled = chkIncluir.Checked;
        }

        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvDepositoTesoreria.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvDepositoTesoreria.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvDepositoTesoreria, "chkVarios", chk.Checked);
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa de manera general los componentes de la forma
        /// </summary>
        private void inicializaForma()
        {
            //Carga catalogos
            cargaCatalogos();
            //Inicializa controles
            inicializaControles();
            TSDK.ASP.Controles.InicializaGridview(gvDepositoTesoreria);
        }
        /// <summary>
        /// Método privado de cargar los catalogos de la forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Catalogo TamañomGrid View
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDepositos, "", 26);
            //Cargando Catalogo de Conceptos de Deposito
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConceptoDeposito, 71, "TODOS", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
        }
        /// <summary>
        /// Método privado de inicializar los controles
        /// </summary>
        private void inicializaControles()
        {
            txtNoDeposito.Text = "";
            ddlConceptoDeposito.SelectedValue = "0";
            txtNoServicio.Text = "";
            txtNoViaje.Text = "";
            txtCartaPorte.Text = "";
            txtUnidad.Text = "";
            txtOperador.Text = "";
            txtTercero.Text = "";
            txtIdentificador.Text = "";
            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Metodo privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro"></param>
        /// <param name="idTabla"></param>
        /// <param name="Titulo"></param>
        private void InicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Soporte/DepositoTesoreria.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=600,height=550";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora de Soporte", configuracion, Page);
        }
        /// <summary>
        /// Metodo de cargar los depositos
        /// </summary>
        private void buscaDepositos()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_doc = DateTime.MinValue;
            DateTime fec_fin_doc = DateTime.MinValue;
            DateTime fec_ini_dep = DateTime.MinValue;
            DateTime fec_fin_dep = DateTime.MinValue;
            //Validando si se Requieren las Fechas
            if (chkIncluir.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbDeposito.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_dep);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_dep);
                }
                else if (rbDocumentacion.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_doc);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_doc);
                }
            }

            //Inicializando indices de selección
            Controles.InicializaIndices(gvDepositoTesoreria);

            //Obtenemos Depósito
            using (DataSet ds = SAT_CL.EgresoServicio.Reportes.ReporteDepositos(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToInt32(Cadena.VerificaCadenaVacia(txtNoDeposito.Text, "0")), Convert.ToByte(4),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtUnidad.Text, ":", 1, "0")),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtOperador.Text, ":", 1, "0")),
                                                                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtTercero.Text, ":", 1, "0")),
                                                                    Cadena.VerificaCadenaVacia(txtIdentificador.Text, "0"), 2, Cadena.VerificaCadenaVacia(txtNoServicio.Text, "0"),
                                                                    fec_ini_doc, fec_fin_doc, fec_ini_dep, fec_fin_dep, 0, 0, Convert.ToInt32(ddlConceptoDeposito.SelectedValue), txtCartaPorte.Text, txtNoViaje.Text))
            {
                //Validando que el DataSet contenga las tablas
                if (Validacion.ValidaOrigenDatos(ds, "Table") && Validacion.ValidaOrigenDatos(ds, "Table1"))
                {
                    //Cargando los GridView
                    Controles.CargaGridView(gvDepositoTesoreria, ds.Tables["Table"], "Id-Servicio-Folio-NoLiquidacion", "", true, 1);
                    //Añadiendo Tablas al DataSet de Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"], "Table");
                }
                else
                {   //Inicializando GridViews
                    //Controles.InicializaGridview(gvResumen);
                    Controles.InicializaGridview(gvDepositoTesoreria);
                    
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
                //Suma Totales al pie
                sumaTotales();
                //Limpiamos Etiqueta
                lblError.Text = "";
            }
        }

        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvDepositoTesoreria.FooterRow.Cells[3].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Monto)", "")));
                gvDepositoTesoreria.FooterRow.Cells[4].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoComprobacion)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvDepositoTesoreria.FooterRow.Cells[3].Text = string.Format("{0:C2}", 0);
                gvDepositoTesoreria.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Metodo encargado de regresar los depositos a tesoreria
        /// </summary>
        public RetornoOperacion RegresaDeposito()
        {
            RetornoOperacion retorno = new RetornoOperacion();
            //Creamos lista de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista de errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene Registros
            if(gvDepositoTesoreria.DataKeys.Count > 0)
            {//Obtiene filas seleccionadas
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvDepositoTesoreria, "chkVarios");
                //Verificando que existan filas seleccionadas
                if(selected_rows.Length != 0)
                {//Almacenando Rutas en arreglo
                    foreach(GridViewRow row in selected_rows)
                    {//Instanciar Deposito a regresar a tesoreria
                        gvDepositoTesoreria.SelectedIndex = row.RowIndex;
                        using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(gvDepositoTesoreria.SelectedDataKey["Id"])))
                        {
                            if(dep.id_deposito != 0)
                            {
                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                {
                                            retorno = dep.RegresaDepositoTesoreria(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    if (retorno.OperacionExitosa)
                                    {
                                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(51, dep.id_deposito))
                                        {
                                            retorno = ei.RegresaDeposito(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                            //SAT_CL.Bancos.EgresoIngreso
                                            if (retorno.OperacionExitosa)
                                            {
                                                //Obteniendo Referencias
                                                using (DataTable dtReferencias = SAT_CL.Global.Referencia.CargaReferencias(ei.id_egreso_ingreso, 101))
                                                {
                                                    //Validando que existan Referencias
                                                    if (Validacion.ValidaOrigenDatos(dtReferencias))
                                                    {
                                                        //Recorriendo Referencias
                                                        foreach (DataRow dr in dtReferencias.Rows)
                                                        {
                                                            //Instanciando Referencia de Vencimiento
                                                            using (SAT_CL.Global.Referencia ven = new SAT_CL.Global.Referencia(Convert.ToInt32(dr["Id"])))
                                                            {
                                                                //Validando que exista
                                                                if (ven.habilitar)

                                                                    //Eliminando Referencia
                                                                    retorno = SAT_CL.Global.Referencia.EliminaReferencia(ven.id_referencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (retorno.OperacionExitosa)
                                    {
                                        trans.Complete();
                                        ScriptServer.MuestraNotificacion(this.Page, retorno.Mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            //Devolviendo Resultado Obtenido
            return retorno;
        }
        #endregion

        #region VentanaModal
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            RetornoOperacion retorno = new RetornoOperacion();
            string depositos = "";
            //Seleccionando fila
            GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvDepositoTesoreria, "chkVarios");
            //Verificando que existan filas seleccionadas
            if (selected_rows.Length != 0)
            {
                foreach (GridViewRow row in selected_rows)
                {
                    gvDepositoTesoreria.SelectedIndex = row.RowIndex;
                    //Instanciar Vale del valor obtenido de la fila seleccionada
                    using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(Convert.ToInt32(gvDepositoTesoreria.DataKeys[row.RowIndex].Value)))
                    {
                        if (dep.id_deposito != 0)
                        {
                            using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(dep.id_deposito, 51))
                            {
                                if (dl.id_liquidacion == 0)
                                {
                                    depositos = dep.no_deposito + " " + depositos;
                                    ucSoporte.InicializaControlUsuario(depositos, 2, Convert.ToString(gvDepositoTesoreria.SelectedDataKey["Servicio"]));
                                    //Mostrando ventana modal correspondiente
                                    ScriptServer.AlternarVentana(lnkEliminar, "Soporte", "soporteTecnicoModal", "soporteTecnico");
                                }
                                else
                                {
                                    retorno = new RetornoOperacion("El deposito " + Convert.ToInt32(gvDepositoTesoreria.SelectedDataKey["Folio"]) + " esta en la liquidacion " + Convert.ToInt32(gvDepositoTesoreria.SelectedDataKey["NoLiquidacion"]) + ", no se puede rechazar.");
                                    ScriptServer.MuestraNotificacion(this.Page, retorno.Mensaje,ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }

                }

            }
            else//Mostrando Mensaje
            {
                retorno = new RetornoOperacion("Debe Seleccionar al menos 1 Deposito");
                ScriptServer.MuestraNotificacion(this.Page, retorno.Mensaje, ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }              
        }
        protected void wucSoporteTecnico_ClickAceptarSoporte(object sender, EventArgs e)
        {
            //Realizando el guardado
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                resultado = RegresaDeposito();
                //Si no hay errores
                if (resultado.OperacionExitosa)
                {
                    //Actualizando lista de vales de diesel
                    resultado = ucSoporte.GuardaSoporte();
                }
                if(resultado.OperacionExitosa)
                {
                    trans.Complete();
                }
            }
            //Cerrando ventana modal
            ScriptServer.AlternarVentana(this, "Soporte", "soporteTecnicoModal", "soporteTecnico");
            buscaDepositos();
            ScriptServer.MuestraNotificacion(this.Page, resultado.Mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }

        protected void lkbCerrarVentanaModal_Click(object sender, EventArgs e)
        {
            ScriptServer.AlternarVentana(lkbCerrarEliminacionVale, "Soporte", "soporteTecnicoModal", "soporteTecnico");
        }
        protected void wucSoporteTecnico_ClickCancelarSoporte(object sender, EventArgs e)
        {
            //Cerrando ventana modal de edición
            ScriptServer.AlternarVentana(this, "Soporte", "soporteTecnicoModal", "soporteTecnico");
        }
        #endregion
    }
}