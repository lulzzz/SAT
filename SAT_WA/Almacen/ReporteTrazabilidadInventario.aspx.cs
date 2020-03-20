using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;
using TSDK.Datos;
using TSDK.ASP;
using SAT_CL;

namespace SAT.Almacen
{
    public partial class ReporteTrazabilidadInventario : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Producirse una Actualización en la Página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Reporte
            buscaReporteTrazabilidad();
        }

        #region Eventos GridView "Inventario"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "IdTipoRegistro");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInventario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando origen de datos de la fila
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                //Validando Tipo de Registro
                switch (row["IdTipoRegistro"].ToString())
                {
                    case "1":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_servicio";
                            break;
                        }
                    case "2":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            break;
                        }
                    case "3":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                            break;
                        }
                    case "4":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[6].CssClass =
                            e.Row.Cells[7].CssClass = "liquidacion_totales_liquidacion";
                            break;
                        }
                }
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar al Contenido de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando GridView
            Controles.InicializaGridview(gvInventario);
        }
        /// <summary>
        /// Método encargado de Buscar el Reporte
        /// </summary>
        private void buscaReporteTrazabilidad()
        {
            //Obteniendo Reporte
            using (DataTable dtInventario = SAT_CL.Almacen.Reportes.ObtieneReporteTrazabilidad(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtAlmacen.Text, "ID:", 1)), txtLote.Text,
                                            Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtProducto.Text, "ID:", 1))))
            {
                //Validando si existen Registros
                if (Validacion.ValidaOrigenDatos(dtInventario))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvInventario, dtInventario, "IdTipoRegistro", "");

                    //Asignando tamaño Dinamico
                    gvInventario.PageSize = dtInventario.Rows.Count + 1;

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtInventario, "Table");
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvInventario);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion
    }
}