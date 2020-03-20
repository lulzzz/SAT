using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Base;

namespace SAT.CuentasPagar
{
    public partial class EntregaFactura : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se Produjo un PostBack
            if (!Page.IsPostBack)
                //Invocando Método de Inicialización
                inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   //Cargando reporte
            cargaReporteFacturas();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Recibir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecibir_Click(object sender, EventArgs e)
        {   //Obteniendo Filas Seleccionadas
            GridViewRow[] gvFilasSeleccionadas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvReporteFacturas, "chkVarios");
            //Validando que haya Filas Seleccionadas
            if (gvFilasSeleccionadas.Length > 0)
            {   //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                //Inicializando Bloque Transaccional
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {   //Declarando Variables Auxiliares
                    bool res = true;
                    int contador = 0;
                    //Iniciando Ciclo
                    while(res)
                    {   //Obteniendo Indice
                        gvReporteFacturas.SelectedIndex = gvFilasSeleccionadas[contador].RowIndex;
                        //Instanciando Factura
                        using (SAT_CL.CXP.FacturadoProveedor fp = new SAT_CL.CXP.FacturadoProveedor(Convert.ToInt32(gvReporteFacturas.SelectedDataKey["IdFactura"])))
                        {   //Validando que exista la Factura
                            if (fp.id_factura != 0)
                            {   //Deshabilitando Factura
                                result = fp.EntregaFacturaProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Incrementando Conatdor
                                contador++;
                                //Obteniendo resultado
                                res = contador >= gvFilasSeleccionadas.Length ? false : result.OperacionExitosa;
                            }
                            else//Terminando Ciclo
                                res = false;
                        }
                    }
                    //validando que la Operación haya sido Exitosa
                    if (result.OperacionExitosa)
                        //Completando Transacción
                        trans.Complete();
                }
                //Validando la Operación
                if (result.OperacionExitosa)
                    //Cargando reporte
                    cargaReporteFacturas();
                //Mostrando Mensaje de la Operación
                lblError.Text = result.Mensaje;
            }
            else//Mostrando Mensaje de Error
                lblError.Text = "No existen filas Seleccionadas";
        }

        #region Eventos GridView "Reporte"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReporteFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresión de Ordenamiento
            lblCriterio.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvReporteFacturas, ((DataSet)Session["DS"]).Tables["Table"], e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvReporteFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvReporteFacturas, ((DataSet)Session["DS"]).Tables["Table"], e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Cambiando Tamaño de Página
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvReporteFacturas, ((DataSet)Session["DS"]).Tables["Table"], Convert.ToInt32(ddlTamano.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExcel_Click(object sender, EventArgs e)
        {   //Exportando Contenido
            TSDK.ASP.Controles.ExportaContenidoGridView(((DataSet)Session["DS"]).Tables["Table"]);
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Control ChecBox de Seleción del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Evalua el ID del CheckBox en el que se produce el cambio
            switch (((CheckBox)sender).ID)
            {   //Caso para el CheckBox "Todos"
                case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                    CheckBox chk = (CheckBox)gvReporteFacturas.HeaderRow.FindControl("chkTodos");
                    //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvReporteFacturas, "chkVarios", chk.Checked);
                    break;
            }//fin switch
        }
        /// <summary>
        /// Evento Producido al Presionar el Link de Bitacora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbBitacoraFactura_Click(object sender, EventArgs e)
        {   //Seleccionando Fila
            TSDK.ASP.Controles.SeleccionaFila(gvReporteFacturas, sender, "lnk", false);
            //Obteniendo Registro
            string idRegistro = gvReporteFacturas.SelectedDataKey["IdFactura"].ToString();
            //Inicializando indices
            TSDK.ASP.Controles.InicializaIndices(gvReporteFacturas);
            //Inicializando Bitacora
            inicializaBitacora(idRegistro, "72", "Factura Proveedor");
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {   //Cargando Catalogo
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18);
            //Instanciando Compania
            using(SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {   //Validando que exista la Compania
                if (cer.id_compania_emisor_receptor != 0)
                    //Mostrando Valor de la Compania
                    txtCompania.Text = cer.nombre + " ID:" + cer.id_compania_emisor_receptor.ToString();
            }
            //Inicializando GridView
            TSDK.ASP.Controles.InicializaGridview(gvReporteFacturas);
        }
        /// <summary>
        /// Método privado encargado de Cargar el Reporte de las Facturas
        /// </summary>
        private void cargaReporteFacturas()
        {   //Declarando Objeto de Fecha
            DateTime fecha_recepcion = DateTime.MinValue;
            //Validando que exista una Fecha
            if (txtFechaRecepcion.Text != "")
                //Obteniendo fecha
                fecha_recepcion = Convert.ToDateTime(txtFechaRecepcion.Text);
            //Obteniendo Reporte
            using (DataTable dtFacturas = SAT_CL.CXP.FacturadoProveedor.ObtieneReporteFacturasRecepcion(Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtProveedor.Text, "ID:", 1)),
                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)), txtSerie.Text, txtFolio.Text,
                                            txtUUID.Text, fecha_recepcion))
            {   //Validando que existan registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                {   //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvReporteFacturas, dtFacturas, "IdFactura", "");
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table");
                }
                else
                {   //Cargando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvReporteFacturas);
                    //Añadiendo Tabla al DataSet de Session
                    TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/CuentasPagar/EntregaFactura.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Recepción", configuracion, Page);
        }

        #endregion
    }
}