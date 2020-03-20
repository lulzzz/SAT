using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Datos;

namespace SAT.Administrativo
{
    public partial class ReporteFichaIngreso : System.Web.UI.Page
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

            //Cargando Autocompletado
            cargaCatalogoAutocompleta();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNombreDep_TextChanged(object sender, EventArgs e)
        {
            //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCtaOrigen, 42, "-- Seleccione una Cuenta", Convert.ToInt32(ddlTipoDep.SelectedValue), "",
                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 1)), "");
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tipo de Entidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Configurando Entidad
            configuraEntidad();
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
            if(chkIncluir.Checked)
            {
                //Obteniendo Fechas de los Controles
                DateTime.TryParse(txtFecIni.Text, out fec_ini);
                DateTime.TryParse(txtFecFin.Text, out fec_fin);
            }
            
            //Obteniendo Movimientos
            using (DataTable dtFichasIngreso = SAT_CL.Bancos.Reporte.ObtieneReporteFichasIngreso(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaEmi.Text, "ID:", 1, "0")),
                    Convert.ToInt32(txtNoFicha.Text == "" ? "0" : txtNoFicha.Text), Convert.ToByte(ddlEstatus.SelectedValue),
                    Convert.ToByte(ddlTipoDep.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 1, "0")),
                    Convert.ToByte(ddlMetodoPago.SelectedValue), Convert.ToInt32(ddlConcepto.SelectedValue), Convert.ToInt32(ddlCtaOrigen.SelectedValue),
                    Convert.ToInt32(ddlCtaDestino.SelectedValue), Convert.ToByte(ddlMoneda.SelectedValue), fec_ini, fec_fin))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFichasIngreso))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvFichasIngreso, dtFichasIngreso, "IdFichaIngreso", "", true, 1);

                    //Añadiendo a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasIngreso, "Table");
                }
                else
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvFichasIngreso);

                    //Eliminando de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            totales();
        }

        #region Eventos GridView "Movimientos"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
            //Invoca al método que calcula el monto de las fichas de ingreso.
            totales();
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
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            //Invoca al método que calcula el monto de las fichas de ingreso.
            totales();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasIngreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasIngreso, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            //Invoca al método que calcula el monto de las fichas de ingreso.
            totales();
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

            //Invocando Método de Configuración
            configuraEntidad();

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvFichasIngreso);

            //Inicializando Fechas
            txtFecIni.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().AddMonths(-1).ToString("dd/MM/yyyy HH:mm");
            txtFecFin.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy HH:mm");
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de Autocompletado
        /// </summary>
        private void cargaCatalogoAutocompleta()
        {
            //Obteniendo Compania
            string idCompania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script de Carga
            string script = @"<script>
                                
                                //Serializando Control
                                $('#" + this.ddlTipoDep.ClientID + @"').serialize();

                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + this.ddlTipoDep.SelectedValue + @";

                                //Validando Tipo de Entidad
                                switch (tipoEntidad) {
                                    case 25:
                                        {   
                                            //Cargando Catalogo AutoCompleta
                                            $('#" + txtNombreDep.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=3&param=" + idCompania + @"'});
                                            break;
                                        }
                                }
                                
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaEntidad", script, false);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //Tamaño GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            //Estatus de la Ficha
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "-- Seleccione un Estatus", 79);
            //Cargando Tipo de Entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoDep, 41, "-- Seleccione un Tipo");
            //Cargando Conceptos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 43, "-- Seleccione un Concepto", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Tipos de Moneda
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMoneda, "-- Seleccione una Moneda", 11);
            //Cargando Tipos de Método de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "-- Seleccione una Método", 80);
            //Cargando Cuentas de Destino -- Donde se va a Recibir el Dinero
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCtaDestino, 42, "-- Seleccione una Cuenta", 25, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
            //Inicializando DropDownList
            TSDK.ASP.Controles.InicializaDropDownList(ddlCtaOrigen, "-- Seleccione una Cuenta");
        }
        /// <summary>
        /// Método encargado de Configurar la Entidad
        /// </summary>
        private void configuraEntidad()
        {
            //Validando el Tipo de Entidad
            switch (ddlTipoDep.SelectedValue)
            {
                //Todos
                case "0":
                    {
                        //Deshabilitando Control
                        txtNombreDep.Enabled = false;
                        break;
                    }
                default:
                    {
                        //Habilitando Control
                        txtNombreDep.Enabled = true;
                        break;
                    }
            }
        }
        
        /// <summary>
        /// Método que calcula el total del monto de la busqueda de fichas de ingreso.
        /// </summary>
        private void totales()
        {
            //Valida que existan datos en el grid view. Invoca el método de validación origen de datos y recupera los valores encontrados del gridview Fichas Ingreso
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Realiza la suma de la columna Monto y en el pie del grid view muestra el resultado
                gvFichasIngreso.FooterRow.Cells[12].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Monto)", "")));
                //Realiza la suma de la columna Monto Pesos y en el pie del grid view muestra el resultado
                gvFichasIngreso.FooterRow.Cells[13].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoPesos)", "")));
            }
            //Si no existen datos en el grid view Ficha Ingreso
            else
            {
                //Asigna el valor 0 al pie del gridview de la columna Monto y Monto Pesos
                gvFichasIngreso.FooterRow.Cells[12].Text =
                gvFichasIngreso.FooterRow.Cells[13].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion
    }
}