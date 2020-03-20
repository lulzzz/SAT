using SAT_CL.ControlPatio;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SAT.ControlPatio
{
    public partial class ReporteHistorialAndenes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
          
            //Validando si e Produjo un PostBack
            if (!(Page.IsPostBack))

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaAndenes();

            //Cargando Indicadores
            cargaIndicadores();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaAndenes();
        }

        #region Eventos GridView Andenes

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAndenes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvAndenes, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex);
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAndenes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvAndenes, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Enlazar el Origen de Datos con el Control GridView "Andenes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAndenes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Recuperando controles de interés
            Image img = (Image)e.Row.FindControl("imgEstatus");

            //Validando que exista el Control
            if (img != null)
            {
                //Valida que exista el Indicador
                if (((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() != "")
                {
                    //Obteniendo Valor
                    int indicador = Convert.ToInt32(((DataRowView)e.Row.DataItem).Row.ItemArray[5]);

                    //Validando los Estatus de Operación
                    switch (((DataRowView)e.Row.DataItem).Row.ItemArray[2].ToString())
                    {
                        case "Cargando":
                            {
                                /*Configurando Imagen*/
                                img.Visible = true;

                                //Validando el Indicador
                                if (indicador == 1)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoOKCarga.png";
                                else if (indicador == 2)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoEXCarga.png";
                                break;
                            }
                        case "Descargando":
                            {
                                /*Configurando Imagen*/
                                img.Visible = true;

                                //Validando el Indicador
                                if (indicador == 1)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoOKDescarga.png";
                                else if (indicador == 2)
                                    //Agregando Ruta
                                    img.ImageUrl = "~/Image/EntidadTiempoEXDescarga.png";
                                break;
                            }
                        case "Disponible":
                            {
                                //Ocultando Control
                                img.Visible = false;
                                img.ImageUrl = "";
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño de la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvAndenes, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAndenes.DataKeys.Count > 0)

                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"], "Id");
        }
        /// <summary>
        /// Evento Producido al 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAndenes.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvAndenes, sender, "lnk", false);

                //Obteniendo Eventos Terminados
                using (DataTable dtEventoTerminados = EventoDetalleAcceso.ObtieneEventosEntidadTerminadosHistorial(Convert.ToInt32(gvAndenes.SelectedDataKey["Id"])))
                {
                    //Validando que existan los Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtEventoTerminados))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvBitacora, dtEventoTerminados, "Id", "", true, 1);

                        //Añadiendo Tabla a DataSet
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtEventoTerminados, "Table1");
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvBitacora);

                        //Eliminando Tabla a DataSet
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                    }
                }

                //Creando Script de ventana Modal
                string script = @"<script>
                                    $('#contenidoBitacoraUnidades').animate({ width: 'toggle' });
                                    $('#bitacoraUnidades').animate({ width: 'toggle' });
                                  </script>";

                //Registrando Script
                ScriptManager.RegisterStartupScript(upgvAndenes, upgvAndenes.GetType(), "MuestraBitacora", script, false);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Actualizar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Carga de Indicadores
            cargaIndicadores();
        }

        #endregion

        #region Eventos Bitacora

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], Convert.ToInt32(ddlTamanoBit.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacora_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoBit.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], e.SortExpression);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvBitacora, ((DataSet)Session["DS"]).Tables["Table1"], e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarBit_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvBitacora.DataKeys.Count > 0)

                //Exporta Contenido del GridView
                TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table1"], "Id");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvAndenes);

            //Creando Script de ventana Modal
            string script = @"<script>
                                    $('#contenidoBitacoraUnidades').animate({ width: 'toggle' });
                                    $('#bitacoraUnidades').animate({ width: 'toggle' });
                                  </script>";
            //Registrando Script
            ScriptManager.RegisterStartupScript(upbtnCerrar, upbtnCerrar.GetType(), "MuestraBitacora", script, false);
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Invocando Método de Carga
            cargaCatalogos();

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvAndenes);
            TSDK.ASP.Controles.InicializaGridview(gvEstatusEntidades);

            //Invocando Método de Carga de Indicadores
            cargaIndicadores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar lo Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {
            //Obteniendo Instancia de Clase
            using (UsuarioPatio up = UsuarioPatio.ObtieneInstanciaDefault(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario))
            {
                //Cargando los Patios por Compania
                SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlPatio, 34, "Ninguno", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, "", 0, "");

                //Asignando Patio por Defecto
                ddlPatio.SelectedValue = up.id_patio.ToString();
            }

            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);

            //Cargando Catalogo de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoBit, "", 18);

            //Inicializando Fechas de Busqueda
            txtFechaIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método Privado encargado de Buscar los Andenes
        /// </summary>
        private void buscaAndenes()
        {
            //Obteniendo Fecha del Filtro
            DateTime fecha_ini = DateTime.MinValue, fecha_fin = DateTime.MinValue;
            
            //Obteniendo Fechas
            DateTime.TryParse(txtFechaIni.Text, out fecha_ini);
            DateTime.TryParse(txtFechaFin.Text, out fecha_fin);
            
            //Obteniendo Reporte
            using (DataSet dsAndenes = Reporte.CargaEstatusEntidadesGenerales(EntidadPatio.TipoEntidad.Anden, Convert.ToInt32(ddlPatio.SelectedValue), fecha_ini, fecha_fin))
            {
                //Validando que existan Andenes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dsAndenes, true))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvAndenes, dsAndenes, 0, "Id", "", true, 1);
                    TSDK.ASP.Controles.CargaGridView(gvEstatusEntidades, dsAndenes, 1, "Estatus", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsAndenes, 0, "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsAndenes, 1, "Table1");

                    //Personalizando Arreglo de Colores
                    System.Drawing.Color[] colores = new System.Drawing.Color[] { System.Drawing.ColorTranslator.FromHtml("#3265CC"), System.Drawing.ColorTranslator.FromHtml("#DC3811"), System.Drawing.ColorTranslator.FromHtml("#FE9900"), 
                                                                                  System.Drawing.ColorTranslator.FromHtml("#109518"), System.Drawing.ColorTranslator.FromHtml("#990099"), System.Drawing.ColorTranslator.FromHtml("#0098C6") };
                    //Carga grafica
                    TSDK.ASP.Controles.CargaGrafica(chtEntidades, "Estatus", "No. Unidades", SeriesChartType.Pie, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1").DefaultView,
                                         "Estatus", "NoUnidades", false, false, colores, " ");

                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAndenes);
                    TSDK.ASP.Controles.InicializaGridview(gvEstatusEntidades);

                    //Quitando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                }
            }

            //
            cargaIndicadores();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Indicadores
        /// </summary>
        private void cargaIndicadores()
        {
            //Obteniendo Fecha del Filtro
            DateTime fecha_ini = DateTime.MinValue, fecha_fin = DateTime.MinValue;

            //Obteniendo Fechas
            DateTime.TryParse(txtFechaIni.Text, out fecha_ini);
            DateTime.TryParse(txtFechaFin.Text, out fecha_fin);
            
            //Obteniendo Reporte
            using (DataTable dtIndicadores = EntidadPatio.RetornaIndicadoresEntidad(Convert.ToInt32(ddlPatio.SelectedValue), fecha_ini, fecha_fin))
            {
                //Validando que existan Andenes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtIndicadores))
                {
                    //Recorriendo cada Fila
                    foreach (DataRow dr in dtIndicadores.Rows)
                    {
                        //Asignando Valores
                        lblCajonesDisp.Text = dr["AndenesDisponibles"].ToString();
                        lblCajonesOcup.Text = dr["AndenesOcupados"].ToString();
                        lblUtilizacion.Text = dr["UtilizacionAndenes"].ToString() + "%";
                        lblTiempoPromedio.Text = dr["TiempoPromedioAndenes"].ToString();
                    }
                }
                else
                {
                    //Inicializando Valores
                    lblCajonesDisp.Text =
                    lblCajonesOcup.Text = "0.00";
                    lblUtilizacion.Text = "0.00%";
                    lblTiempoPromedio.Text = "0.00";
                }
            }
        }

        #endregion
    }
}