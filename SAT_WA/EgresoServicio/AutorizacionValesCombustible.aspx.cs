using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.EgresoServicio;

namespace SAT.EgresoServicio
{
    /// <summary>
    /// CdeBehind de la forma AutorizacionValesCombustible
    /// </summary>
    public partial class AutorizacionValesCombustible : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento activado al presionar el botón "Actualizar Vales" o "Rechazar Vales"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizarEstatusVales_Click(object sender, EventArgs e)
        {
            //Crear un botón virtual que nos permita obtener el Commando del botón presionado
            Button boton = (Button)sender;
            //Llamar al método específico, mandando el comando del botón
            actualizarEstatusVales(boton.CommandName);
        }
        /// <summary>
        /// Evento activado al seleccionar el checkbox del encabezado de la primer columna. Selecciona todos los registros mostrados para su Rechazo o Autorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosValesCombustible_CheckedChanged(object sender, EventArgs e)
        {
            //Validar si el GridView contiene registros
            if (gvAutorizacionValesCombustible.DataKeys.Count > 0)
            {
                //Evalua el checkbox que llama al evento
                switch (((CheckBox)sender).ID)
                {
                    //Si es el checkbox del encabezado
                    case "chkTodosValesCombustible":
                        {
                            //Crear un checkbox donde se asigna el control del encabezado
                            CheckBox check = (CheckBox)gvAutorizacionValesCombustible.HeaderRow.FindControl("chkTodosValesCombustible");
                            //Asignar el valor de "chkTodosNominaEmpleado" a todos los controles 
                            Controles.SeleccionaFilasTodas(gvAutorizacionValesCombustible, "chkVariosValesCombustible", check.Checked);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento activado al seleccionar un checkbox de la primer columna, diferente al encabezado. Prepara el registro para su Rechazo o Autorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkVariosValesCombustible_CheckedChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Evento activado al elegir un elemento del dropdownlist TamañoGrid. Recarga el gridview con un nuevo numero de elementos mostrados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Validar que existan registros en el gridview
            if (gvAutorizacionValesCombustible.DataKeys.Count > 0)
            {
                //Se cambia el tamaño del gridview en base al elemento seleccionad
                Controles.CambiaTamañoPaginaGridView(gvAutorizacionValesCombustible, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoGrid.SelectedValue), true, 3);
            }
            sumaTotales();
        }
        /// <summary>
        /// Evento activado al cambiar el orden de alguna columna del gridview. Recarga el gridview con el nuevo orden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAutorizacionValesCombustible_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //Valdiar que existan registros en el gridview
            if (gvAutorizacionValesCombustible.DataKeys.Count > 0)
            {
                //Asignanro el orden en el label ordenado
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvAutorizacionValesCombustible, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
            }
            //Recalcular totales
            sumaTotales();
        }
        /// <summary>
        /// Evento activado al cambiar de página del gridview. Recarga el gridview con el siguiente conjunto de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAutorizacionValesCombustible_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Validar que existan registros en el gridview
            if (gvAutorizacionValesCombustible.DataKeys.Count > 0)
            {
                //Cambiar el incide de la página
                Controles.CambiaIndicePaginaGridView(gvAutorizacionValesCombustible, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
            }
            //Recalcula totales
            sumaTotales();
        }
        /// <summary>
        /// Evento activado al presionar el linkbutton "Exportar" en algun gridview. Descarga el contenido actual del gridview en un archivo XSLX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Crear un linkbutton virtual
            using (LinkButton lnk = (LinkButton)sender)
            {
                //Validar el gridview que se desea descargar mediante el CommandName del linkbutton
                switch (lnk.CommandName)
                {
                    case "ValesCombustible":
                        {
                            //Exportar el contenido
                            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                            //Terminar
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento activado al iniciar la página. Permite asignar seguridad a la forma e inicializar controles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si se carga por un método diferente a postback
            if (!Page.IsPostBack)
                //Invoca al método principal
                inicializaPagina();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Cambia el estatus de los vales seleccionados a "Actualizado" o "Rechazado", según el comando recibido
        /// </summary>
        private void actualizarEstatusVales(string comando)
        {
            //Crear objeto RetornoOperacion para obtener mensaje de error
            RetornoOperacion resultado = new RetornoOperacion();
            //Verificar que el gridview contenga registros
            if (gvAutorizacionValesCombustible.DataKeys.Count > 0)
            {
                //Obtener filas seleccionadas
                GridViewRow[] registrosSeleccionados = Controles.ObtenerFilasSeleccionadas(gvAutorizacionValesCombustible, "chkVariosValesCombustible");
                //Verificar que existen registros seleccionadas
                if (registrosSeleccionados.Length > 0)
                {
                    //Por cada registro seleccionado...
                    foreach (GridViewRow gvRow in registrosSeleccionados)
                    {
                        //...Crear un objeto
                        using (SAT_CL.EgresoServicio.AsignacionDiesel objAD = new SAT_CL.EgresoServicio.AsignacionDiesel(Convert.ToInt32(gvAutorizacionValesCombustible.DataKeys[gvRow.RowIndex].Value)))
                        {
                            /*/Mediante el comando determinar el nuevo estatus
                            if (comando == "ActualizarVales")//Si es Actualizar, el estatus es 2
                                //Llamando al método que ejecuta la actualizacion
                                resultado = objAD.ActualizaEstatusVale(2, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else if (comando == "RechazarVales")//Si es Rechazar, el estatus es 5
                                //Llamando al método que ejecuta la actualizacion
                                resultado = objAD.ActualizaEstatusVale(5, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);//*/
                        }
                        //Si se actualizó el registro/objeto, mostrar notificacion
                        if (resultado.OperacionExitosa)
                            ScriptServer.MuestraNotificacion(this, resultado.Mensaje, ScriptServer.NaturalezaNotificacion.Exito, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
                //Si no seleccionó ningun registro
                else
                    //Mostrar aviso
                    ScriptServer.MuestraNotificacion(this, "Seleccione los vales que desea modificar.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
            //Por último, recarga el gridview
            cargaValesCombustible();
        }   
        /// <summary>
        /// Carga el contenido de todos los drop down list
        /// </summary>
        private void cargaCatalogos()
        {
            //DDL Mostrar
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGrid, "", 26);
        }
        /// <summary>
        /// Obtiene una tabla con los detalles de los vales para combustible
        /// </summary>
        private void cargaValesCombustible()
        {
            //Obtener reporte
            using (DataTable dtValesCombustible = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneValesCombustible())
            {
                //Validar registros obtenidos
                if (Validacion.ValidaOrigenDatos(dtValesCombustible))
                {
                    //Cargar datos al gridview
                    Controles.CargaGridView(gvAutorizacionValesCombustible, dtValesCombustible, "Id", lblOrdenado.Text, true, 1);
                    //Añadir resultado a session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtValesCombustible, "Table");
                }
                else
                {
                    //Inicializa GridView
                    Controles.InicializaGridview(gvAutorizacionValesCombustible);
                    //Eliminando resultado de session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            sumaTotales();
        }
        /// <summary>
        /// Método princpal que carga los métodos secundarios
        /// </summary>
        private void inicializaPagina()
        {
            //Cargar catálogos en DDL.
            cargaCatalogos();
            //Inicializar el gridview
            Controles.InicializaGridview(gvAutorizacionValesCombustible);
            //Cargar el contenido del gridview
            cargaValesCombustible();
        }
        /// <summary>
        /// Recalcula los piés de las columnas que deban llevar totales
        /// </summary>
        private void sumaTotales()
        {
            //Suma la columna Total
            gvAutorizacionValesCombustible.FooterRow.Cells[9].Text = 
                string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
        }
        #endregion
    }
}