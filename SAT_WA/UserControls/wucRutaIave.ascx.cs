using SAT_CL;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.UserControls
{
    public partial class wucRutaIave : System.Web.UI.UserControl
    {

        #region Atributos
        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _id_servicio;
        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _asignacion_recurso;
        /// <summary>
        /// Id Unidad
        /// </summary>
        private int _id_unidad;
        /// <summary>
        /// Id Operador
        /// </summary>
        private int _id_operador;
        /// <summary>
        /// Id proveedor compañia
        /// </summary>
        private int _id_proveedor_compania;
        /// <summary>
        /// Indica la Tabla de Rutas
        /// </summary>
        private DataTable _mitRuta;
        /// <summary>
        /// Indica la Tabla de Rutas
        /// </summary>
        private DataTable _mitRutaCambio;
        /// <summary>
        /// Indica la Tabla de Casetas
        /// </summary>
        private DataTable _mitCasetas;
        /// <summary>
        /// Indica la Tabla de Conceptos
        /// </summary>
        private DataTable _mitConceptos;
        /// <summary>
        /// Indica la Tabla de Diesel
        /// </summary>
        private DataTable _mitDiesel;
        #endregion

        #region Eventos

        /// <summary>
        /// Evento producido al  cargar el control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Produjo un PostBack
            if (Page.IsPostBack)
            {
                //Recuperando Atributos
                recuperaAtributos();
            }

            //Implementado seguridad de este recurso
            SAT_CL.Seguridad.Forma.AplicaSeguridadControlusuarioWeb(this, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
        }
        /// <summary>
        /// Evento Producido antes de Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Recuperando Atributos
            asignaAtributos();
        }

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

                case "detalles":
                    //Cerramos Ventana Modal
                    alternaVentanaModal("detalles", lkbCerrar);
                    break;
                case "calculo":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("calculo", lkbCerrar);
                    break;
                case "CambioRuta":
                    //ocultamos ventana modal correspondiente
                    alternaVentanaModal("cambioRuta", lkbCerrar);
                    break;

            }
        }
        /// <summary>
        /// Evento generado a Calcular la ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarCalculoRuta_Click(object sender, System.EventArgs e)
        {
            //Calculamo s Ruta
            CalculaRutaGeneral();
        }
        /// <summary>
        /// Click en pestaña del menú principal de la forma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion

        #region Eventos Ruta


        /// <summary>
        /// Evento corting de gridview de Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRuta_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvRuta.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitRuta.DefaultView.Sort = lblOrdenadoRuta.Text;
                //Cambiando Ordenamiento
                lblOrdenadoRuta.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvRuta, this._mitRuta, e.SortExpression, true, 1);
            }
        }

        /// <summary>
        /// Evento cambio de página en gridview de Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRuta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvRuta, this._mitRuta, e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoRuta_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvRuta, this._mitRuta, Convert.ToInt32(ddlTamanoRuta.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento click en botón de exportación de Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarRuta_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitRuta, "");
        }

        /// <summary>
        /// Ecvento generado al generar una Accion en Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRuta_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvRuta.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvRuta, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Detalles":
                        // Abriendo ventana 
                        alternaVentanaModal("detalles", lkb);
                        //Cargamos Casetas
                        cargaCasetas();
                        //Cargamos Diesel
                        cargaDiesel();
                        //Cargamos Conceptos
                        cargaConceptos();
                        break;
                    case "Calcular":
                        //Mostramos Ventana modal
                        alternaVentanaModal("calculo", lkb);
                        //Cargamos Diesel
                        //chkEfectivoCasetas.Checked = false;
                        break;
                    case "Cambio":
                        //Cargamos casetas
                        cargaRutasCambio();
                        break;
                }
            }
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
                case "detalles":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "detalles", "contenedorDetalles");
                    break;
                case "calculo":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmarCalculo", "confirmacionConfirmarCalculo");
                    break;
                case "cambioRuta":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoCambioRuta", "confirmacionCambioRuta");
                    break;                 
            }
        }

        /// <summary>
        /// Carga de catálogos 
        /// </summary>
        private void cargaCatalogos()
        {
            //Catálogo de tamaño de GV
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCasetas, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDiesel, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoRuta, "", 26);
            //Catálogo de tamaño de GV 
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoConceptos, "", 26);

        }
        #endregion

        #region Métodos Ruta
        /// <summary>
        /// Carga las Rutas Coincidentes de un Servicio
        /// </summary>
        private void cargaRutas()
        {
            //Cargando Lecturas
            this._mitRuta = SAT_CL.Ruta.Ruta.CargaRutasServicio(this._id_servicio);
            //Si no hay registros
            if (this._mitRuta == null)
                TSDK.ASP.Controles.InicializaGridview(gvRuta);
            else
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvRuta, this._mitRuta, "id-IdSegmento-IdRuta-IdMovimiento", lblOrdenadoDiesel.Text, true, 0);
        }
        #endregion

        #region Métodos Casetas 
        /// <summary>
        /// Carga las Casetas
        /// </summary>
        private void cargaCasetas()
        {
            //Valida que existan los registros del la tabla dtOrdenAlmacenDetalle
            if (Validacion.ValidaOrigenDatos(this._mitRuta))
            {
                List<DataRow> ObtieneMovimientoInicial = (from DataRow mi in this._mitRuta.AsEnumerable()
                                                          where mi.Field<Decimal>("Secuencia") == 1
                                                          select mi).ToList();
                if (ObtieneMovimientoInicial.Count > 0)
                {
                    //Recorre la tabla dtOrdenAlmacenDetalle de la orden de Almacen
                    foreach (DataRow mi in ObtieneMovimientoInicial)
                    {
                        //Cargando Casetas
                        this._mitCasetas = SAT_CL.Ruta.RutaCaseta.CargaMontoCasetaRutaIave(Convert.ToInt32(mi["IdSegmento"]), Convert.ToString(gvRuta.SelectedDataKey["IdRuta"]),
                            SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(mi["IdMovimiento"])));
                        //Si no hay registros
                        if (this._mitCasetas == null)
                            TSDK.ASP.Controles.InicializaGridview(gvCasetas);
                        else
                            //Mostrandolos en gridview
                            TSDK.ASP.Controles.CargaGridView(gvCasetas, this._mitCasetas, "Descripcion", lblOrdenadoCasetas.Text, true, 0);
                        //Suma Totales
                        sumaTotalesCasetas();
                    }
                }
            }       
        }

        /// <summary>
        /// Método encargado de Sumar los Totales de las Casetas
        /// </summary>
        private void sumaTotalesCasetas()
        {
            //Validando que existe la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._mitCasetas))
            {
                //Calculamos Totales
                gvCasetas.FooterRow.Cells[4].Text = string.Format("{0:C2}", this._mitCasetas.Compute("SUM(MontoIAVE)", ""));
                gvCasetas.FooterRow.Cells[5].Text = string.Format("{0:C2}", this._mitCasetas.Compute("SUM(Monto)", ""));
                gvCasetas.FooterRow.Cells[6].Text = string.Format("{0:C2}", this._mitCasetas.Compute("SUM(Deposito)", ""));
                gvCasetas.FooterRow.Cells[4].HorizontalAlign =
                gvCasetas.FooterRow.Cells[5].HorizontalAlign =
                gvCasetas.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                //Calculamos Totales
                gvCasetas.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
                gvCasetas.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
                gvCasetas.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
                gvCasetas.FooterRow.Cells[4].HorizontalAlign =
                gvCasetas.FooterRow.Cells[5].HorizontalAlign =
                gvCasetas.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        #endregion

        #region Eventos Casetas
        /// <summary>
        /// Evento corting de gridview de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCasetas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvCasetas.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitCasetas.DefaultView.Sort = lblOrdenadoCasetas.Text;
                //Cambiando Ordenamiento
                lblOrdenadoCasetas.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCasetas, this._mitCasetas, e.SortExpression, true, 0);
            }
        }

        /// <summary>
        /// Evento cambio de página en gridview de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCasetas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCasetas, this._mitCasetas, e.NewPageIndex, true, 0);
        }

        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCasetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCasetas, this._mitCasetas, Convert.ToInt32(ddlTamanoCasetas.SelectedValue), true, 0);
        }

        /// <summary>
        /// Evento click en botón de exportación de Casetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarCasetas_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitCasetas, "");
        }

        #endregion

        #region Metodos Conceptos
        /// <summary>
        /// Carga las Casetas
        /// </summary>
        private void cargaConceptos()
        {
            //Cargando Casetas
            this._mitConceptos = SAT_CL.Ruta.RutaDeposito.CargaDepositosRutaSegmento(Convert.ToString(gvRuta.SelectedDataKey["IdRuta"]), this._id_servicio);
            //Si no hay registros
            if (this._mitConceptos == null)
                TSDK.ASP.Controles.InicializaGridview(gvConceptos);
            else
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvConceptos, this._mitConceptos, "Id",  lblOrdenadoConceptos.Text, true, 0);
            //Suma Montos
            sumaTotalesConceptos();
        }

        /// <summary>
        /// Método encargado de Sumar los Totales de los Conceptos
        /// </summary>
        private void sumaTotalesConceptos()
        {
            //Validando que existe la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._mitConceptos))
            {
                //Calculamos Totales
                gvConceptos.FooterRow.Cells[1].Text = string.Format("{0:C2}", this._mitConceptos.Compute("SUM(Monto)", ""));
                gvConceptos.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                //Calculamos Totales
                gvConceptos.FooterRow.Cells[1].Text = string.Format("{0:C2}", 0);
                gvConceptos.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        #endregion
        #region Eventos Conceptos
        /// <summary>
        /// Evento corting de gridview de Conceptos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvConceptos.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitConceptos.DefaultView.Sort = lblOrdenadoConceptos.Text;
                //Cambiando Ordenamiento
                lblOrdenadoConceptos.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvConceptos, this._mitConceptos, e.SortExpression, true, 1);
            }
        }

        /// <summary>
        /// Evento cambio de página en gridview de Conceptos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvConceptos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvConceptos, this._mitConceptos, e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoConceptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvConceptos, this._mitConceptos, Convert.ToInt32(ddlTamanoConceptos.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento click en botón de exportación de Conceptos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarConceptos_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitConceptos, "");
        }
        #endregion

        #region Metodos Diesel

        /// <summary>
        /// Carga el Diesel
        /// </summary>
        private void cargaDiesel()
        {
            ////Instanciamos Unidad
            //using (SAT_CL.Global.Unidad objUnidad = new Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
            //{
            //    //Cargando Casetas
            //    this._mitDiesel = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)));
            //    //Si no hay registros
            //    if (this._mitDiesel == null)
            //        TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            //    else
            //        //Mostrandolos en gridview
            //        TSDK.ASP.Controles.CargaGridView(gvDiesel, this._mitDiesel, "Litros", lblOrdenadoDiesel.Text, true, 1);
            //    //Asignando Leyenda de tipo de unidad y Tarjeta IAVE sobre GridView de Casetas
            //    lblDimensionesUnidadCasetas.Text = string.Format("[ Cuotas para: {0} {1} Ejes | {2}  |  Tarjeta IAVE: {3} ]", UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"])), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)), SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag I+D") == "" ? SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag IAVE") == "" ? "No" : "Si" : "Si");
            //}

            //Valida que existan los registros del la tabla dtOrdenAlmacenDetalle
            if (Validacion.ValidaOrigenDatos(this._mitRuta))
            {
                List<DataRow> ObtieneMovimientoInicial = (from DataRow mi in this._mitRuta.AsEnumerable()
                                                          where mi.Field<Decimal>("Secuencia") == 1
                                                          select mi).ToList();
                if (ObtieneMovimientoInicial.Count > 0)
                {
                    //Recorre la tabla dtOrdenAlmacenDetalle de la orden de Almacen
                    foreach (DataRow mi in ObtieneMovimientoInicial)
                    {
                        using (SAT_CL.Global.Unidad objUnidad = new Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
                        {
                            //Cargando Casetas
                            this._mitDiesel = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(mi["IdSegmento"]),
                                UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad),objUnidad.id_unidad.ToString());
                            //this._mitDiesel = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]),
                            //    UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)));
                            ////Si no hay registros
                            //Si no hay registros
                            if (this._mitDiesel == null)
                                TSDK.ASP.Controles.InicializaGridview(gvDiesel);
                            else
                                //Mostrandolos en gridview
                                TSDK.ASP.Controles.CargaGridView(gvDiesel, this._mitDiesel, "Id", lblOrdenadoDiesel.Text, true, 1);
                            //Asignando Leyenda de tipo de unidad y Tarjeta IAVE sobre GridView de Casetas
                            lblDimensionesUnidadCasetas.Text = string.Format("[ Cuotas para: {0} {1} Ejes | {2}  |  Tarjeta IAVE: {3} ]", UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"])), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)), SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag I+D") == "" ? SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag IAVE") == "" ? "No" : "Si" : "Si");

                        }
                    }
                }
            }
            sumaTotalesDiesel();
        }
        /// <summary>
        /// Método encargado de Sumar los Totales de los Conceptos
        /// </summary>
        private void sumaTotalesDiesel()
        {
            //Validando que existe la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._mitDiesel))
            {
                //Calculamos Totales
                gvDiesel.FooterRow.Cells[5].Text = string.Format("{0:C2}", this._mitDiesel.Compute("SUM(Litros)", ""));
                gvDiesel.FooterRow.Cells[6].Text = string.Format("{0:C2}", this._mitDiesel.Compute("SUM(CostoCombustible)", ""));
                gvDiesel.FooterRow.Cells[7].Text = string.Format("{0:C2}", this._mitDiesel.Compute("SUM(Total)", ""));
                gvDiesel.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                gvDiesel.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                gvDiesel.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                //Calculamos Totales
                gvDiesel.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
                gvDiesel.FooterRow.Cells[6].Text = string.Format("{0:C2}", 0);
                gvDiesel.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                gvDiesel.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                gvDiesel.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                gvDiesel.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        #endregion

        #region Eventos Diesel
        /// <summary>
        /// Evento corting de gridview de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Validando que existan Llaves
            if (gvDiesel.DataKeys.Count > 0)
            {   //Asignando Criterio de Ordenacion
                this._mitDiesel.DefaultView.Sort = lblOrdenadoDiesel.Text;
                //Cambiando Ordenamiento
                lblOrdenadoDiesel.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvDiesel, this._mitDiesel, e.SortExpression, true, 1);
            }
        }

        /// <summary>
        /// Evento cambio de página en gridview de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvDiesel, this._mitDiesel, e.NewPageIndex, true, 1);
        }

        /// <summary>
        /// Evento cambio de tamaño de página de gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDiesel_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvDiesel, this._mitDiesel, Convert.ToInt32(ddlTamanoDiesel.SelectedValue), true, 1);
        }

        /// <summary>
        /// Evento click en botón de exportación de Diesel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarDiesel_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(this._mitDiesel, "");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Asignar los Atributos 
        /// </summary>
        private void asignaAtributos()
        {
            //Asignando Atributos
            ViewState["_id_servicio"] = this._id_servicio;
            ViewState["_asignacion_recurso"] = this._asignacion_recurso;
            ViewState["idUnidad"] = this._id_unidad;
            ViewState["idOperador"] = this._id_operador;
            ViewState["idProveedorCompania"] = this._id_proveedor_compania;
            ViewState["mitRuta"] = this._mitRuta;
            ViewState["mitRutaCambio"] = this._mitRutaCambio;
            ViewState["mitCasetas"] = this._mitCasetas;
            ViewState["mitConceptos"] = this._mitConceptos;
            ViewState["mitDiesel"] = this._mitDiesel;
        }
        
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
            //Recuperando Atributos
            this._id_servicio = Convert.ToInt32(ViewState["_id_servicio"]);
            //Recuperando Atributos
            this._asignacion_recurso = Convert.ToInt32(ViewState["_asignacion_recurso"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idUnidad"]) != 0)
                this._id_unidad = Convert.ToInt32(ViewState["idUnidad"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idOperador"]) != 0)
                this._id_operador = Convert.ToInt32(ViewState["idOperador"]);
            //Recuperando Atributos
            if (Convert.ToInt32(ViewState["idProveedorCompania"]) != 0)
                this._id_proveedor_compania = Convert.ToInt32(ViewState["idProveedorCompania"]);
            //Asignamos Valores
            if (ViewState["mitRuta"] != null)
                    this._mitRuta = (DataTable)ViewState["mitRuta"];
            //Asignamos Valores
            if (ViewState["mitRutaCambio"] != null)
                this._mitRutaCambio = (DataTable)ViewState["mitRutaCambio"];
            //Recuperando Atributos
            if (ViewState["mitCasetas"] != null)
                    this._mitCasetas = (DataTable)ViewState["mitCasetas"];
            //Recuperando Atributos
            if (ViewState["mitConceptos"] != null)
                    this._mitConceptos = (DataTable)ViewState["mitConceptos"];
            //Recuperando Atributos
            if (ViewState["mitDiesel"] != null)
                    this._mitDiesel = (DataTable)ViewState["mitDiesel"];
            
        }
        
        /// <summary>
        /// Inicializamos Control
        /// </summary>
        public void InicializaControl(int id_servicio, int asignacion_recurso, int id_unidad, int id_operador, int id_proveedor_compania)
        {
          //Asignamos Id de Servicio
            this._id_servicio = id_servicio;
            this._asignacion_recurso = asignacion_recurso;
            this._id_unidad = id_unidad;
            this._id_operador = id_operador;
            this._id_proveedor_compania = id_proveedor_compania;
            //Carga Catalogos
            cargaCatalogos();
            //Cargamos Rutas
            cargaRutas();
        }

        /// <summary>
        /// Calculamos Ruta
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion CalculaRutaGeneral()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();
            int idmovimiento = 0;
            int idsegmento = 0;
            string idruta = "0,";
            List<int> Rutas = new List<int>();
            List<int> Segmentos = new List<int>();
            //Validamos que exista elemento selecionado
            if (gvRuta.SelectedIndex != -1)
            {
                //Valida que existan los registros del la tabla dtOrdenAlmacenDetalle
                if (Validacion.ValidaOrigenDatos(this._mitRuta))
                {
                    List<DataRow> ObtieneMovimientoInicial = (from DataRow mi in this._mitRuta.AsEnumerable()
                                                              //where mi.Field<string>("Calcular") == "Calcular"
                                                              select mi).ToList();
                    if (ObtieneMovimientoInicial.Count > 0)
                    {
                        //Recorre la tabla dtOrdenAlmacenDetalle de la orden de Almacen
                        foreach (DataRow mi in ObtieneMovimientoInicial)
                        {
                            if (Convert.ToInt32(mi["Secuencia"]) == 1)
                            {
                                idmovimiento = Convert.ToInt32(mi["IdMovimiento"]);
                                idsegmento = Convert.ToInt32(mi["IdSegmento"]);
                            }
                            using (SAT_CL.Ruta.Ruta objRutas = new SAT_CL.Ruta.Ruta(Convert.ToInt32(mi["IdRuta"])))
                            using (SAT_CL.Despacho.SegmentoCarga objSegmento = new SAT_CL.Despacho.SegmentoCarga(Convert.ToInt32(mi["IdSegmento"])))
                            {
                                if (objSegmento.habilitar && objRutas.habilitar)
                                {
                                    if (objSegmento.id_ruta == 0)
                                    {
                                        if (objSegmento.id_estatus_segmento == objRutas.tipo_aplicacion && objSegmento.id_estatus_segmento == 1)
                                        {
                                            idruta = Convert.ToString(mi["IdRuta"]) + "," + idruta;
                                            Rutas.Add(Convert.ToInt32(mi["IdRuta"]));
                                            Segmentos.Add(objSegmento.id_segmento_carga);
                                        }
                                        if (objSegmento.id_estatus_segmento != 1)
                                        {
                                            idruta = Convert.ToString(mi["IdRuta"]) + "," + idruta;
                                            Rutas.Add(Convert.ToInt32(mi["IdRuta"]));
                                            Segmentos.Add(objSegmento.id_segmento_carga);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                idruta = idruta + ']';

                idruta = TSDK.Base.Cadena.RegresaCadenaSeparada(idruta, ",]", 0);


                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
                {
                    //Cargando Vales
                    //using (DataTable mitVales = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]),
                    //    Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad),
                    //    SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue))))
                    using (DataTable mitVales = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), objUnidad.id_unidad.ToString()))
                    {
                        //Cargando Casetas
                        using (DataTable mitCasetas = SAT_CL.Ruta.RutaCaseta.CargaMontoCasetaRutaIave(idsegmento, idruta, SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(idmovimiento)))
                        {
                            //Cargando Conceptos
                            using (DataTable mitConceptos = SAT_CL.Ruta.RutaDeposito.CargaDepositosRutaSegmento(idruta, this._id_servicio))
                            {
                                //Calulamos Ruta del Segmento
                                resultado = SAT_CL.Ruta.Ruta.CalculaRuta(mitCasetas, mitVales, mitConceptos, Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]),
                                idsegmento, idmovimiento, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, this._asignacion_recurso, this._id_operador, this._id_proveedor_compania, Rutas, Segmentos, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            }
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializamos Control
                        InicializaControl(this._id_servicio, this._asignacion_recurso, this._id_unidad, this._id_operador, this._id_proveedor_compania);
                        //Cargamos Rutas
                        cargaRutas();
                        //Mostramos Ventana modal
                        alternaVentanaModal("calculo", btnConfirmarCalculoRuta);
                        //Inicializamos Indices
                        Controles.InicializaIndices(gvRuta);
                    }
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnConfirmarCalculoRuta, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            return resultado;
        }
        #endregion

        /// <summary>
        /// Evento cambio de página en gridview de Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCambioRuta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCambioRuta, this._mitRuta, e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Carga las Rutas Coincidentes de un Servicio
        /// </summary>
        private void cargaRutasCambio()
        {
            //Cargando Lecturas
            using (this._mitRutaCambio = SAT_CL.Ruta.Ruta.CargaRutasSecuencias(this._id_servicio,Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]),Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"])))
            {
                //Valida Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(this._mitRutaCambio))
                {
                    //Cargamos Grid View
                    TSDK.ASP.Controles.CargaGridView(gvCambioRuta, _mitRutaCambio, "id-IdRuta", "");
                    //Asigna a la variable de sesion los datos del dataset invocando al método AñadeTablaDataSet
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], _mitRutaCambio, "Table");
                    alternaVentanaModal("cambioRuta", gvRuta);
                }
                else
                {
                    //Inicializa el gridView 
                    Controles.InicializaGridview(gvCambioRuta);
                    //Elimina los datos del dataset si se realizo una consulta anterior
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");

                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(gvRuta, "No tiene rutas coincidentes para realizar cambio", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }


        }
        /// <summary>
        /// Ecvento generado al generar una Accion en Ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbAccionRutaCambio_Click(object sender, EventArgs e)
        {
            //Si hay registros
            if (gvCambioRuta.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvCambioRuta, sender, "lnk", false);
                //Convirtiendo objeto a linkbutton
                LinkButton lkb = (LinkButton)sender;

                //Determinando el comando por ejecutar
                switch (lkb.CommandName)
                {
                    case "Seleccionar":
                        //Cargamos Casetas
                        ActualizacionRutas();
                        alternaVentanaModal("cambioRuta", lkb);
                        break;
                }
            }
        }

        /// <summary>
        /// Carga las Rutas Coincidentes de un Servicio
        /// </summary>
        private void ActualizacionRutas()
        {
            //Valida Origen de Datos
            //Valida que existan los registros del la tabla dtOrdenAlmacenDetalle
            if (Validacion.ValidaOrigenDatos(this._mitRutaCambio))
            {
                List<DataRow> RutaNueva = (from DataRow mi in this._mitRutaCambio.AsEnumerable()
                                                          where mi.Field<int>("id") == Convert.ToInt32(gvCambioRuta.SelectedDataKey["id"])
                                                          select mi).ToList();
                if (RutaNueva.Count > 0)
                {
                    //Recorre la tabla dtOrdenAlmacenDetalle de la orden de Almacen
                    foreach (DataRow mi in RutaNueva)
                    {
                        foreach (DataRow dr in this._mitRuta.Select("id = " + gvRuta.SelectedDataKey["id"].ToString()))
                        {
                            dr["IdRuta"] = Convert.ToInt32(mi["IdRuta"]);
                            dr["Ruta"] = mi["Ruta"].ToString();
                            dr["Aplicacion"] = mi["Aplicacion"].ToString();
                            dr["Calcular"] = mi["Calcular"].ToString();
                        }
                        this._mitRuta.AcceptChanges();
                    }
                }
                //Cargamos Grid View
                TSDK.ASP.Controles.CargaGridView(gvRuta, this._mitRuta, "id-IdSegmento-IdRuta-IdMovimiento", "");
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRuta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //validando Fila de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando información de la fila actual
                if (e.Row.DataItem != null)
                {
                    //Obteniendo Fila de Datos
                    DataRow fila = ((DataRowView)e.Row.DataItem).Row;
                    //Obteniendo Control Contenedor
                    using (LinkButton lkbDetalles = (LinkButton)e.Row.FindControl("lkbDetalles"))
                    using (LinkButton lkbCalcular = (LinkButton)e.Row.FindControl("lkbCalcular"))
                    using (LinkButton lkbCambio = (LinkButton)e.Row.FindControl("lkbCambio"))
                    {
                        //Validando Fila
                        if (fila != null)
                        {
                            //Verificar que la columna donde se encuentran los controles dinámicos no esté vacía
                            if (fila["Ruta"].ToString() == "SIN RUTA")
                            {
                                //Obteniendo Control Contenedor
                                lkbDetalles.Visible = 
                                lkbCalcular.Visible =
                                lkbCambio.Visible = false;
                            }
                            //Verificar que la columna donde se encuentran los controles dinámicos no esté vacía
                            else if (fila["Calcular"].ToString() == "Calculada")
                            {
                                //Obteniendo Control Contenedor
                                lkbDetalles.Visible = true;
                                lkbCalcular.Visible =
                                lkbCambio.Visible = false;
                            }
                            else
                            {
                                //Obteniendo Control Contenedor
                                //Obteniendo Control Contenedor
                                lkbDetalles.Visible =
                                lkbCalcular.Visible =
                                lkbCambio.Visible = true;
                            }
                        }
                    }
                }
            }
        }
    }
}