using SAT_CL;
using SAT_CL.Global;
using SAT_CL.Seguridad;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
namespace SAT.UserControls
{
    public partial class wucRuta : System.Web.UI.UserControl
    {

        #region Atributos
        private int _id_servicio;
        /// <summary>
        /// Indica la Tabla de Rutas
        /// </summary>
        private DataTable _mitRuta;
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

            }
        }

        /// <summary>
        /// Evento generado a Calcular la ruta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmarCalculo_Click(object sender, System.EventArgs e)
        {
            //Calculamo s Ruta
            CalculaRuta();
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
                        chkEfectivoCasetas.Checked = false;
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
                TSDK.ASP.Controles.CargaGridView(gvRuta, this._mitRuta, "IdSegmento-IdRuta-IdMovimiento", lblOrdenadoDiesel.Text, true, 0);
        }

        #endregion

        #region Métodos Casetas 
        /// <summary>
        /// Carga las Casetas
        /// </summary>
        private void cargaCasetas()
        {
            //Cargando Casetas
            this._mitCasetas = SAT_CL.Ruta.RutaCaseta.CargaMontoCasetaRuta(Convert.ToInt32(gvRuta.SelectedValue), Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"])));
            //Si no hay registros
            if (this._mitCasetas == null)
                TSDK.ASP.Controles.InicializaGridview(gvCasetas);
            else
                //Mostrandolos en gridview
                TSDK.ASP.Controles.CargaGridView(gvCasetas, this._mitCasetas, "Descripcion", lblOrdenadoCasetas.Text, true, 0);
            //Suma Totales
            sumaTotalesCasetas();
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
                gvCasetas.FooterRow.Cells[4].HorizontalAlign =
                gvCasetas.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                //Calculamos Totales
                gvCasetas.FooterRow.Cells[4].Text = string.Format("{0:C2}", 0);
                gvCasetas.FooterRow.Cells[5].Text = string.Format("{0:C2}", 0);
                gvCasetas.FooterRow.Cells[4].HorizontalAlign =
                gvCasetas.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
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
            this._mitConceptos = SAT_CL.Ruta.RutaDeposito.CargaDepositosRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), this._id_servicio);
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
            //Instanciamos Unidad
            using (SAT_CL.Global.Unidad objUnidad = new Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
            {
                //Cargando Casetas
                this._mitDiesel = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)));
                //Si no hay registros
                if (this._mitDiesel == null)
                    TSDK.ASP.Controles.InicializaGridview(gvDiesel);
                else
                    //Mostrandolos en gridview
                    TSDK.ASP.Controles.CargaGridView(gvDiesel, this._mitDiesel, "Litros", lblOrdenadoDiesel.Text, true, 1);
                //Asignando Leyenda de tipo de unidad y Tarjeta IAVE sobre GridView de Casetas
                lblDimensionesUnidadCasetas.Text = string.Format("[ Cuotas para: {0} {1} Ejes | {2}  |  Tarjeta IAVE: {3} ]", UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"])), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue)), SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag I+D") == "" ? SAT_CL.Global.Referencia.CargaReferencia("0", 19, objUnidad.id_unidad, "Control Cruces", "Tag IAVE") == "" ? "No" : "Si" : "Si");
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
            ViewState["mitRuta"] = this._mitRuta;
            ViewState["mitCasetas"] = this._mitCasetas;
            ViewState["mitConceptos"] = this._mitConceptos;
            ViewState["mitDiesel"] = this._mitDiesel;
        }
        
        /// <summary>
        /// Método encargado de Recuperar los Atributos
        /// </summary>
        private void recuperaAtributos()
        {
                 this._id_servicio = Convert.ToInt32(ViewState["_id_servicio"]);
              //Asignamos Valores
                if (ViewState["mitRuta"] != null)
                    this._mitRuta = (DataTable)ViewState["mitRuta"];
                if (ViewState["mitCasetas"] != null)
                    this._mitCasetas = (DataTable)ViewState["mitCasetas"];
                if (ViewState["mitConceptos"] != null)
                    this._mitConceptos = (DataTable)ViewState["mitConceptos"];
                if (ViewState["mitDiesel"] != null)
                    this._mitDiesel = (DataTable)ViewState["mitDiesel"];
            
        }
        
        /// <summary>
        /// Inicializamos Control
        /// </summary>
        public void InicializaControl(int id_servicio)
        {
          //Asignamos Id de Servicio
            this._id_servicio = id_servicio;
            //Carga Catalogos
            cargaCatalogos();
            //Cargamos Rutas
            cargaRutas();
        }


        /// <summary>
        /// Calculamos Ruta
        /// </summary>
        /// <returns></returns>
        public RetornoOperacion CalculaRuta()
        {
            //Declaramos Mensaje
            RetornoOperacion resultado = new RetornoOperacion();

            //Validamos que exista elemento selecionado
            if (gvRuta.SelectedIndex != -1)
            {
                //Instanciamos Unidad
                using (SAT_CL.Global.Unidad objUnidad = new Unidad(SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneUnidadMotriz(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
                {
                    //Cargando Vales
                    using (DataTable mitVales = SAT_CL.Ruta.RutaTipoUnidad.CargaValesRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), UnidadTipo.RegresaDescripcionUnidadTipo(objUnidad.id_tipo_unidad), SAT_CL.Despacho.SegmentoCarga.ObtieneConfiguracionSegmento(Convert.ToInt32(gvRuta.SelectedValue))))
                    {
                        //Cargando Casetas
                        using (DataTable mitCasetas = SAT_CL.Ruta.RutaCaseta.CargaMontoCasetaRuta(Convert.ToInt32(gvRuta.SelectedValue), Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), SAT_CL.Despacho.MovimientoAsignacionRecurso.ObtieneTotalEjesUnidadNoCanceladas(Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]))))
                        {
                            //Cargando Conceptos
                            using (DataTable mitConceptos = SAT_CL.Ruta.RutaDeposito.CargaDepositosRutaSegmento(Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), this._id_servicio))
                            {

                                //Calulamos Ruta del Segmento
                                resultado = SAT_CL.Ruta.Ruta.CalculaRuta(mitCasetas, mitVales, mitConceptos, Convert.ToInt32(gvRuta.SelectedDataKey["IdRuta"]), chkEfectivoCasetas.Checked,
                                         Convert.ToInt32(gvRuta.SelectedDataKey["IdSegmento"]), Convert.ToInt32(gvRuta.SelectedDataKey["IdMovimiento"]), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,((UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);
                            }
                        }
                    }
                    //Validamos Resultado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializamos Control
                        InicializaControl(this._id_servicio);
                        //Cargamos Rutas
                        cargaRutas();
                        //Mostramos Ventana modal
                        alternaVentanaModal("calculo", btnConfirmarCalculo);
                        //Inicializamos Indices
                        Controles.InicializaIndices(gvRuta);

                    }
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(btnConfirmarCalculo, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
            return resultado;
        }

        #endregion 

      

  


    }
}