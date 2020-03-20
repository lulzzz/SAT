using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
namespace SAT.Almacen
{
    public partial class ExistenciasAlmacen : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            //Validando que se Haya Producido un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// Evento generado al buscar las Eventoses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga las Eventos
            cargaExistencias();
        }
        /// <summary>
        /// Evento que cambia el tamaño de registros del GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvExistencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 3);
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento que exporta a un archivo de excel los registros del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento que permite al cambio de paginación del GridView del GridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvExistencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvExistencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento que regula el ordenamiento de registros en base a la columna seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvExistencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvExistencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);

            //Suma Totales
            sumaTotales();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Mostrando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvExistencias);

            //Invocando Carga de Catalogos
            cargaCatalogos();
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
        /// Método encargado de Cargar los Valores en el GridView
        /// </summary>
        private void cargaExistencias()
        {
            DateTime fechaInicio = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            if (chkIncluir.Checked)
            {
                //Asignamos variable de Fechas
                fechaInicio = Convert.ToDateTime(txtFecIni.Text);
                fechaFin = Convert.ToDateTime(txtFecFin.Text);
            }

            //Obtenemo Resultado del reporte Generado
            using (DataTable mit = SAT_CL.Almacen.Reportes.ObtieneExistenciasAlmacen(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(
                                                                                    txtAlmacen.Text, "ID:", 1), "0")), txtProducto.Text, txtLote.Text, txtSerie.Text, fechaInicio, fechaFin))
            {
                //Validando Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando GridView con Datos
                    Controles.CargaGridView(gvExistencias, mit, "Id", lblOrdenado.Text, true, 3);
                    //Guardando Tabla en Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Eliminando Tablas del DataSet de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    //Cargando GridView Vacio
                    Controles.InicializaGridview(gvExistencias);
                }
            }

            //Suma Totales
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
                gvExistencias.FooterRow.Cells[7].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(PrecioEntrada)", "")));
                gvExistencias.FooterRow.Cells[8].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(PrecioSalidaActual)", "")));
                gvExistencias.FooterRow.Cells[9].Text = string.Format("{0:0.00}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CantEntrada)", "")));
                gvExistencias.FooterRow.Cells[10].Text = string.Format("{0:0.00}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CantSalida)", "")));
                gvExistencias.FooterRow.Cells[11].Text = string.Format("{0:0.00}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(CantExistencia)", "")));
                gvExistencias.FooterRow.Cells[12].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvExistencias.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                gvExistencias.FooterRow.Cells[8].Text = string.Format("{0:C2}", 0);
                gvExistencias.FooterRow.Cells[9].Text = string.Format("{0:0.00}", 0);
                gvExistencias.FooterRow.Cells[10].Text = string.Format("{0:0.00}", 0);
                gvExistencias.FooterRow.Cells[11].Text = string.Format("{0:0.00}", 0);
                gvExistencias.FooterRow.Cells[12].Text = string.Format("{0:C2}", 0);
            }
        }

        #endregion
    }
}