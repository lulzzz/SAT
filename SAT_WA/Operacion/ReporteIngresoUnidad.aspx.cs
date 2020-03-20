using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class ReporteIngresoUnidad : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se Haya Producido un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Validando que Existan las Fechas de Ingreso
            DateTime fec_ini = DateTime.MinValue;
            DateTime fec_fin = DateTime.MinValue;

            //Validando la Inclusión de Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas de los Controles
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }

            //Obteniendo Unidades
            using (DataTable dtUnidades = SAT_CL.Despacho.Reporte.ReporteGeneralUnidades(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaEmi.Text, "ID:", 1, "0")), fec_ini, fec_fin))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtUnidades))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvUnidades, dtUnidades, "IdUnidad", "", true, 1);

                    //Añadiendo a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtUnidades, "Table");
                }
                else
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvUnidades);

                    //Eliminando de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }

                //Cargando Totales
                cargaTotalesUnidades();
            }
        }

        #region Eventos GridView "Unidades"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));

            //Cargando Totales
            cargaTotalesUnidades();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);

            //Cargando Totales
            cargaTotalesUnidades();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Unidades"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUnidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvUnidades, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);

            //Cargando Totales
            cargaTotalesUnidades();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "ver Historial"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerHistorial_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvUnidades.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvUnidades, sender, "lnk", false);

                //Construyendo URL de ventana de historial de unidad
                string url = Cadena.RutaRelativaAAbsoluta("~/Operacion/ReporteIngresoUnidad.aspx", "~/Accesorios/HistorialMovimiento.aspx?idRegistro=" + gvUnidades.SelectedDataKey["IdUnidad"].ToString() + "&idRegistroB=1");

                //Definiendo Configuracion de la Ventana
                string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=900,height=500";

                //Abriendo Nueva Ventana
                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Historial Movimiento", configuracion, Page);
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Instanciando Companis Emisor Receptor
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que Existe la Compania
                if (cer.id_compania_emisor_receptor > 0)

                    //Asignando Valor
                    txtCompaniaEmi.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando Control
                    txtCompaniaEmi.Text = "";
            }

            //Invocando Carga de Catalogos
            cargaCatalogos();

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvUnidades);

            //Cargando Unidades
            cargaTotalesUnidades();

            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
        }
        /// <summary>
        /// Método encargado de Mostrar los Totales de las Unidades
        /// </summary>
        private void cargaTotalesUnidades()
        {
            //Validando que Existe el Origen de Datos
            if(TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales Calculados
                gvUnidades.FooterRow.Cells[3].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(KmCargado)", "")));
                gvUnidades.FooterRow.Cells[4].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(KmVacio)", "")));
                gvUnidades.FooterRow.Cells[5].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("AVG(Vacio)", "")));
                gvUnidades.FooterRow.Cells[6].Text = string.Format("{0:0.00}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalKms)", "")));
                gvUnidades.FooterRow.Cells[7].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalIngreso)", "")));
            }
            else
            {
                //Mostrando Totales en 0
                gvUnidades.FooterRow.Cells[3].Text = string.Format("{0:0.00}", 0);
                gvUnidades.FooterRow.Cells[4].Text = string.Format("{0:0.00}", 0);
                gvUnidades.FooterRow.Cells[5].Text = string.Format("{0:0.00}", 0);
                gvUnidades.FooterRow.Cells[6].Text = string.Format("{0:0.00}", 0);
                gvUnidades.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion
    }
}