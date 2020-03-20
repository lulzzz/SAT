using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Almacen
{
    public partial class OrdenesCompraPendientes : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando que se haya Producido un PostBack
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
            //Buscando Ordenes de Compra
            buscarOrdenesCompraPendientes();
        }
        /// <summary>
        /// Evento que direcciona a la forma OrdenCompra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCompra_Click(object sender, EventArgs e)
        {
            //Selecciona la fila del gridView
            Controles.SeleccionaFila(gvOrdenesCompraPendientes, sender, "lnk", false);
            //Asigna a la variable de session la orden de compra
            Session["id_registro"] = Convert.ToInt32(gvOrdenesCompraPendientes.SelectedDataKey["Id"]);
            Session["estatus"] = Pagina.Estatus.Lectura;
            //Cargala la ruta del formulario Orden de compra
            string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenesCompraPendientes.aspx", "~/Almacen/OrdenCompra.aspx");
            //Redirecciona a la nueva forma
            Response.Redirect(url);
        }
        /// <summary>
        /// Evento que direcciona a la forma de factura proveedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbFactura_Click(object sender, EventArgs e)
        {
            //Selecciona la fila del gridview
            Controles.SeleccionaFila(gvOrdenesCompraPendientes, sender, "lnk", false);
            //Asigna a la variable de session de la orden de compra
            int id_factura = Convert.ToInt32(gvOrdenesCompraPendientes.SelectedDataKey["IdFacturaProveedor"]);
            //Valida que exista factura de proveedor
            if (id_factura > 0)
            {
                Session["id_registro"] = id_factura;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Carga la direccion del formaulario Factura proveedor
                string url = Cadena.RutaRelativaAAbsoluta("~/Almacen/OrdenesCompraPendientes.aspx", "~/CuentasPagar/Facturado.aspx");
                Response.Redirect(url);
            }

        }
        #region Eventos GridView "Ordenes de Compra Pendientes"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvOrdenesCompraPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
            //Invoca al método que calcula los totales de las columnas del gridview
            calculaTotalesOrdenesCompra();
        }
        /// <summary>
        /// Evento Producido al Exportar al Contenido del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrdenesCompraPendientes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvOrdenesCompraPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            //Invoca al método que calcula los totales de las columnas del gridview
            calculaTotalesOrdenesCompra();
            
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Ordenes de Compra Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvOrdenesCompraPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvOrdenesCompraPendientes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            //Invoca al método que calcula los totales de las columnas del gridview
            calculaTotalesOrdenesCompra();
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Inicializando Fechas
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");

            //Inicializando GridView
            Controles.InicializaGridview(gvOrdenesCompraPendientes);

            //Cargando Tamaño
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
        }
        /// <summary>
        /// Método encargado de Buscar las Ordenes de Compra Pendientes
        /// </summary>
        private void buscarOrdenesCompraPendientes()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent;
            fec_ini_sol = fec_fin_sol = fec_ini_ent = fec_fin_ent = DateTime.MinValue;

            //Validando que se encuentre 
            if (chkSolicitud.Checked)
            {
                //Asignando Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini_sol);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_sol);
            }
            if (chkEntrega.Checked)
            {
                //Asignando Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini_ent);
                DateTime.TryParse(txtFecFin.Text, out fec_fin_ent);
            }

            //Obteniendo Ordenes de Compra
            using (DataTable dtOrdenCompra = SAT_CL.Almacen.Reportes.ObtieneOrdenesCompraPendientes(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                fec_ini_sol, fec_fin_sol, fec_ini_ent, fec_fin_ent, Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),txtNoorden.Text))
            {
                //Validando que existen Registros
                if (Validacion.ValidaOrigenDatos(dtOrdenCompra))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvOrdenesCompraPendientes, dtOrdenCompra, "Id-IdFacturaProveedor", lblOrdenado.Text, true, 2);

                    //Añadiendo Tabla a Sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtOrdenCompra, "Table");
                    //Realiza el calculo de las columnas

                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvOrdenesCompraPendientes);

                    //Eliminando Tabla de Sesión
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            //Inicializa los indices del gridview
            Controles.InicializaIndices(gvOrdenesCompraPendientes);
            //Invoca al método que realiza el calculo de los totales de las columnas del gridview
            calculaTotalesOrdenesCompra();
        }
        /// <summary>
        /// Método encargado de realizar la suma de los 
        /// </summary>
        private void calculaTotalesOrdenesCompra()
        {
            //Instancia a la variable de sesion que almacena los datos de las ordenes de compra pendientes
            using (DataTable dtOrdenesCompra = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"))
            {
                //Validación de la tabla dtOrdenesCompra
                if (Validacion.ValidaOrigenDatos(dtOrdenesCompra))
                {
                    //Realiza la suma de las columnas
                    gvOrdenesCompraPendientes.FooterRow.Cells[6].Text = string.Format("{0}", (dtOrdenesCompra.Compute("SUM(Productos)", "")));
                    gvOrdenesCompraPendientes.FooterRow.Cells[7].Text = string.Format("{0:C2}", (dtOrdenesCompra.Compute("SUM(TotalOrden)", "")));
                    gvOrdenesCompraPendientes.FooterRow.Cells[8].Text = string.Format("{0:C2}", (dtOrdenesCompra.Compute("SUM(Factura)", "")));
                    gvOrdenesCompraPendientes.FooterRow.Cells[9].Text = string.Format("{0:C2}", (dtOrdenesCompra.Compute("SUM(Pago)", "")));
                }
                //En caso contrario deja en valor 0
                else
                {
                    gvOrdenesCompraPendientes.FooterRow.Cells[6].Text = string.Format("{0}", 0);
                    gvOrdenesCompraPendientes.FooterRow.Cells[7].Text = string.Format("{0:C2}", 0);
                    gvOrdenesCompraPendientes.FooterRow.Cells[8].Text = string.Format("{0:C2}", 0);
                    gvOrdenesCompraPendientes.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
                }
            }
        }
        #endregion




    }
}