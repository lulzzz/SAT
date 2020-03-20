using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Operacion
{
    public partial class ReporteMovimientos : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
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

            //Cargando Autocompletado
            cargaAutocompletaEntidad();
        }
        /// <summary>
        /// Evento Producido al Marcar la Opción "Movimiento Vacio"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkMovVacio_CheckedChanged(object sender, EventArgs e)
        {
            //Validando que este Habilitando el Control
            if (chkMovVacio.Checked)
            {
                //Deshabilitando Control
                txtNoServicio.Enabled = false;
                txtNoServicio.Text = "";
            }
            else
                //Habilitando Control
                txtNoServicio.Enabled = true;
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tipo de Entidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipoEntidad_SelectedIndexChanged(object sender, EventArgs e)
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
            //Obteniendo Fechas
            DateTime fecha_ini, fecha_fin;
            DateTime.TryParse(txtFechaIni.Text, out fecha_ini);
            DateTime.TryParse(txtFechaFin.Text, out fecha_fin);

            //Obteniendo Movimientos
            using (DataTable dtMovimientos = SAT_CL.Despacho.Reporte.ReporteMovimientosGeneral(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompaniaEmi.Text, "ID:", 1, "0")),
                    chkMovVacio.Checked ? "0" : txtNoServicio.Text, Convert.ToByte(ddlEstatus.SelectedValue),
                    Convert.ToByte(ddlTipo.SelectedValue), Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtOrigen.Text, "ID:", 1, "0")),
                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtDestino.Text, "ID:", 1, "0")), Convert.ToByte(ddlTipoEntidad.SelectedValue),
                    Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1, "0")), fecha_ini, fecha_fin))
            {
                //Validando que Existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtMovimientos))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvMovimientos, dtMovimientos, "NoMovimiento", "", true, 1);

                    //Añadiendo a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtMovimientos, "Table");
                }
                else
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvMovimientos);

                    //Eliminando de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
                //Mostrar totales a pie de columna
                sumaTotalesgvMovimientos();
            }
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
            Controles.CambiaTamañoPaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamano.SelectedValue));
            //Mostrando totales
            sumaTotalesgvMovimientos();
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
        protected void gvMovimientos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Vales de Diesel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvMovimientos, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
            //Mostrando totales
            sumaTotalesgvMovimientos();
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

            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);

            //Cargando controles de Fechas
            DateTime fecha_actual = Fecha.ObtieneFechaEstandarMexicoCentro();
            txtFechaIni.Text = fecha_actual.AddDays(-7).ToString("dd/MM/yyyy HH:mm");
            txtFechaFin.Text = fecha_actual.ToString("dd/MM/yyyy HH:mm");

            //Invocando Carga de Catalogos
            cargaCatalogos();

            //Invocando Método de Configuración
            configuraEntidad();
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Catalogo Autocompleta
        /// </summary>
        private void cargaAutocompletaEntidad()
        {
            //Obteniendo Compania
            string id_compania = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString();

            //Declarando Script
            string script = @"<script type='text/javascript'>
                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + ddlTipoEntidad.SelectedValue + @";
                            
                                //Evento Change
                                $('#" + ddlTipoEntidad.ClientID + @"').change(function () {
                                    
                                    //Limpiando Control
                                    $('#" + txtEntidad.ClientID + @"').val('');

                                    //Invocando Funcion
                                    CargaAutocompleta();
                                });
                                
                                //Declarando Función de Autocompleta
                                function CargaAutocompleta(){
                                    //Validando Tipo de Entidad
                                    switch (tipoEntidad) {
                                        case 1:
                                            {   
                                                //Cargando Catalogo de Unidades
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 2:
                                            {   
                                                //Cargando Catalogo de Operadores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 3:
                                            {   
                                                //Cargando Catalogo de Proveedores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=16&param=" + id_compania + @"'});
                                                break;
                                            }
                                    }
                                }
                                
                                //Invocando Funcion
                                CargaAutocompleta();
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
            //Estatus y Tipo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "-- Seleccione un Opción", 61);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "-- Seleccione un Opción", 63);
            //Tipo Asignación
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEntidad, "-- Seleccione un Opción", 46);
        }
        /// <summary>
        /// Método encargado de Configurar la Entidad
        /// </summary>
        private void configuraEntidad()
        {
            //Validando el Tipo de Entidad
            switch (ddlTipoEntidad.SelectedValue)
            {
                //Todos
                case "0":
                    {
                        //Deshabilitando Control
                        txtEntidad.Enabled = false;
                        break;
                    }
                default:
                    {
                        //Habilitando Control
                        txtEntidad.Enabled = true;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de mostrar los totales en el pie de cada columna.
        /// </summary>
        private void sumaTotalesgvMovimientos()
        {
            //Validar origen de datos
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando totales
                gvMovimientos.FooterRow.Cells[12].Text = string.Format("{0}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Kms)", "")));
                gvMovimientos.FooterRow.Cells[16].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TPagos)", "")));
                gvMovimientos.FooterRow.Cells[17].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TAnticipos)", "")));
                gvMovimientos.FooterRow.Cells[18].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TComprobaciones)", "")));
                gvMovimientos.FooterRow.Cells[19].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TDiesel)", "")));
            }
            else
            {
                gvMovimientos.FooterRow.Cells[12].Text =
                gvMovimientos.FooterRow.Cells[16].Text =
                gvMovimientos.FooterRow.Cells[17].Text =
                gvMovimientos.FooterRow.Cells[18].Text =
                gvMovimientos.FooterRow.Cells[19].Text = string.Format("{0:C2}", 0);
            }
        }
        #endregion
    }
}