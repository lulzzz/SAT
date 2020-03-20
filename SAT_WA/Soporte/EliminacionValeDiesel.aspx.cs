using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;
using TSDK.Base;
using System.Collections.Generic;
using System.Transactions;

namespace SAT.Soporte
{
    public partial class EliminacionValeDiesel : System.Web.UI.Page
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
            //Buscando vales coincidentes con filtros
            buscaValeDiesel();
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
            TSDK.ASP.Controles.InicializaGridview(gvEliminacionValeDiesel);

            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

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
        /// Realiza la búsqueda de vales de diesel
        /// </summary>
        private void buscaValeDiesel()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_sol = DateTime.MinValue;
            DateTime fec_fin_sol = DateTime.MinValue;
            DateTime fec_ini_car = DateTime.MinValue;
            DateTime fec_fin_car = DateTime.MinValue;
            int id_cliente = 0;
            int id_ubicacion = 0;
            //int id_servicio = ;
            byte id_estatus = 0;
            int id_unidad = 0;
            int id_operador = 0;
            int id_proveedor = 0;
            string complemento = "";

            //Validando si se Requieren las Fechas
            if (chkIncluir.Checked)
            {
                //Validando el Tipo de Fecha Requerida
                if (rbCarga.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_car);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_car);
                }
                else if (rbSolicitud.Checked)
                {
                    //Obteniendo Fechas
                    DateTime.TryParse(txtFecIni.Text, out fec_ini_sol);
                    DateTime.TryParse(txtFecFin.Text, out fec_fin_sol);
                }
            }

            //Inicializando indices de selección
            Controles.InicializaIndices(gvEliminacionValeDiesel);

            //Obteniendo Reporte de Saldos Globales
            using (DataTable dtEliminacionValeDiesel = SAT_CL.EgresoServicio.Reportes.ReporteValesDiesel(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                        id_cliente, txtNoVale.Text, txtNoServicio.Text, fec_ini_sol, fec_fin_sol, fec_ini_car, fec_fin_car, id_ubicacion, id_unidad, id_operador,
                        id_proveedor, id_estatus, complemento, Cadena.RegresaCadenaSeparada(txtUnidadDiesel.Text, "ID:", 0, "0").Trim(), txtNoViaje.Text))

            {
                //Cargando GridView
                Controles.CargaGridView(gvEliminacionValeDiesel, dtEliminacionValeDiesel, "Id-NoVale-NoServicio", "", true, 2);

                //Validando que existan Registros
                if (dtEliminacionValeDiesel != null)
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtEliminacionValeDiesel, "Table");
                else
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }

            //Sumando Totales
            sumaTotales();
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
                gvEliminacionValeDiesel.FooterRow.Cells[20].Text = string.Format("{0}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Litros)", "")));
                gvEliminacionValeDiesel.FooterRow.Cells[21].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoTotal)", "")));
            }
            else
            {
                //Mostrando Totales en Cero                
                gvEliminacionValeDiesel.FooterRow.Cells[20].Text = string.Format("{0}", 0);
                gvEliminacionValeDiesel.FooterRow.Cells[21].Text = string.Format("{0:C2}", 0);
            }
        }

        /// <summary>
        /// Evento Producido al Eliminar Contenido del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public RetornoOperacion EliminaVale()
        {
            RetornoOperacion resultado = new RetornoOperacion(1);
            //Creamos lista de registros
            List<KeyValuePair<string, byte[]>> registros = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene Registros
            if (gvEliminacionValeDiesel.DataKeys.Count > 0)
            { //Obtiene filas seleccionadas
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvEliminacionValeDiesel, "chkVarios");               
                //Verificando que existan filas Seleccionadas
                if (selected_rows.Length != 0)
                {
                    //Almacenando Rutas en arreglo
                    foreach (GridViewRow row in selected_rows)
                    {//Instanciar Vale del valor obtenido de la fila seleccionada
                        using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvEliminacionValeDiesel.DataKeys[row.RowIndex].Value)))
                        {
                            if (ad.id_asignacion_diesel != 0)
                            {
                                using (SAT_CL.EgresoServicio.DetalleLiquidacion dl = new SAT_CL.EgresoServicio.DetalleLiquidacion(ad.id_asignacion_diesel, 69))
                                    ad.EliminaValeDiesel(ad.id_asignacion_diesel, ad.id_unidad_diesel, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, dl.id_detalle_liquidacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                        }                    
                    }
                }
            }
            return resultado;
        }
        #endregion

        #region Eventos GridView "Vales de Diesel"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvEliminacionValeDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 2);

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEliminacionValeDiesel_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvEliminacionValeDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 2);

            //Sumando Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEliminacionValeDiesel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvEliminacionValeDiesel, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 2);

            //Sumando Totales
            sumaTotales();
        }

        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvEliminacionValeDiesel.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvEliminacionValeDiesel.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvEliminacionValeDiesel, "chkVarios", chk.Checked);
                        break;
                }
            }
        }
        #endregion

        #region VentanaModal
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            string vales = "";
            //Seleccionando fila
            GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvEliminacionValeDiesel, "chkVarios");
            //Verificando que existan filas seleccionadas
            if (selected_rows.Length != 0)
            {
                foreach (GridViewRow row in selected_rows)
                {
                    gvEliminacionValeDiesel.SelectedIndex = row.RowIndex;
                    //Instanciar Vale del valor obtenido de la fila seleccionada
                    using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvEliminacionValeDiesel.DataKeys[row.RowIndex].Value)))
                    {
                        if (ad.id_asignacion_diesel != 0)
                        {
                            vales = ad.no_vale + " " + vales;

                        }
                    }
                        
                }
             ucSoporte.InicializaControlUsuario(vales,1, Convert.ToString(gvEliminacionValeDiesel.SelectedDataKey["NoServicio"]));
             //Mostrando ventana modal correspondiente
             ScriptServer.AlternarVentana(lnkEliminar, "Soporte", "soporteTecnicoModal", "soporteTecnico");
            }
            else//Mostrando Mensaje
                
            ScriptServer.MuestraNotificacion(this.Page, "Debe Seleccionar al menos 1 Vale", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        protected void wucSoporteTecnico_ClickAceptarSoporte(object sender, EventArgs e)
        {
            //Realizando el guardado
            RetornoOperacion resultado = new RetornoOperacion();  
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                resultado = EliminaVale();
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
            buscaValeDiesel();
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

