using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Administrativo
{
    public partial class ReporteEgresoIngreso : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)

                //Inicializando Página
                inicializaPagina();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Busqueda
            buscaAplicacionFichas();
        }
        /// <summary>
        /// Evento Producido al Marcar o Desmarcar el Control "Incluir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkIncluir_CheckedChanged(object sender, EventArgs e)
        {
            //Invocando Método de Configuración
            configuraControles();
        }

        #region Eventos GridView "Fichas Aplicadas"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
            //Invoca al método que calcula el monto de las fichas de ingreso.
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
        protected void gvFichasIngreso_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            //Invoca al método que calcula el monto de las fichas de ingreso.
            sumaTotales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasIngreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
            //Invoca al método que calcula el monto de las fichas de ingreso.
            sumaTotales();
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Carga Catalogos
            cargaCatalogo();

            //Inicializando GridView
            Controles.InicializaGridview(gvFichasIngreso);

            //Habilitando Inclusión
            chkIncluir.Checked = true;

            //Instanciando Compania Emiora
            using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validando que exista la Compania
                if (cer.habilitar)

                    //Añadiendo Valor
                    txtCompaniaEmi.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
                else
                    //Limpiando el Control
                    txtCompaniaEmi.Text = "";
            }

            //Asignando Fechas Iniciales
            txtFecFin.Text = Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            txtFecIni.Text = Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");

            //Invocando Método de Configuración
            configuraControles();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogo()
        {
            //Tamaño GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);

            //Cargando Cuentas de la Compania
            using (DataTable dtCuentas = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(42, "", 25, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, ""))
            {
                //Validando que existan Controles
                if (Validacion.ValidaOrigenDatos(dtCuentas))

                    //Cargando Control
                    Controles.CargaDropDownList(ddlCuenta, dtCuentas, "id", "descripcion");
                else
                    //Inicializando Drop
                    Controles.InicializaDropDownList(ddlCuenta, "-- Seleccione una Cuenta");
            }
        }
        /// <summary>
        /// Método encargado de Obtener el Reporte de Fichas
        /// </summary>
        private void buscaAplicacionFichas()
        {
            //Declarando Variables Auxiliares
            DateTime fec_ini = DateTime.MinValue;
            DateTime fec_fin = DateTime.MinValue;

            //Incluyendo Fechas
            if (chkIncluir.Checked)
            {
                //Obteniendo Fechas
                DateTime.TryParse(txtFecIni.Text, out fec_ini); 
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }

            //Validando que exista una Cuenta
            if (!ddlCuenta.SelectedValue.Equals("0"))
            {
                //Obteniendo Reporte de Fichas Aplicadas
                using (DataTable dtFichasAplicadas = SAT_CL.Bancos.Reporte.ObtieneReporteEgresoIngreso(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompaniaEmi.Text, "ID:", 1)),
                                                            Convert.ToInt32(ddlCuenta.SelectedValue), fec_ini, fec_fin))
                {
                    //Validando que Existan Registros
                    if (Validacion.ValidaOrigenDatos(dtFichasAplicadas))
                    {
                        //Cargando GridView
                        Controles.CargaGridView(gvFichasIngreso, dtFichasAplicadas, "IdEgresoIngreso", "", true, 1);

                        //Añadiendo a Session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasAplicadas, "Table");
                    }
                    else
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFichasIngreso);

                        //Eliminando de Session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    }
                }

                //Invocando Método de Suma de Totales
                sumaTotales();
            }
        }
        /// <summary>
        /// Método encargado de Calcular el total del monto de la busqueda de fichas de ingreso.
        /// </summary>
        private void sumaTotales()
        {
            //Valida que existan datos en el grid view. Invoca el método de validación origen de datos y recupera los valores encontrados del gridview Fichas Ingreso
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Realiza la suma de la columna Monto y en el pie del grid view muestra el resultado
                gvFichasIngreso.FooterRow.Cells[8].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoEgreso)", "")));
                //Realiza la suma de la columna Monto Pesos y en el pie del grid view muestra el resultado
                gvFichasIngreso.FooterRow.Cells[9].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoIngreso)", "")));
            }
            //Si no existen datos en el grid view Ficha Ingreso
            else
            {
                //Asigna el valor 0 al pie del gridview de la columna Monto y Monto Pesos
                gvFichasIngreso.FooterRow.Cells[8].Text =
                gvFichasIngreso.FooterRow.Cells[9].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método encargado de Configurar los Controles
        /// </summary>
        private void configuraControles()
        {
            //Habilitando Controles
            txtFecIni.Enabled =
            txtFecFin.Enabled = chkIncluir.Checked;

            //Validando que no se Incluyan
            if (!chkIncluir.Checked)
            {
                //Asignando Fechas por Defecto
                txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddDays(-7).ToString("dd/MM/yyyy HH:mm");
                txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
            }
        }

        #endregion
    }
}