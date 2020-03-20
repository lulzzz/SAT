using SAT_CL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.ControlEvidencia
{
    public partial class VisorHojaInstruccion : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al Generarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Aplicando seguridad de la forma
            SAT_CL.Seguridad.Forma.AplicaSeguridadForma(this, "content1", ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

            //Validando si se produjo un PostBack
            if (!Page.IsPostBack)
            {   //Invocando método de Recarga de la Pagina
                inicializaPagina();
                //Establecemos control por default
                this.Form.DefaultButton = btnBuscar.UniqueID;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el boton "Atras"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAtras_OnClick(object sender, ImageClickEventArgs e)
        {   //Invocando Método de Direccionamiento a Pagina Anterior
       PilaNavegacionPaginas.DireccionaPaginaAnterior();
        }
        /// <summary>
        /// Evento Producido al Presionar el boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {   //Obteniendo Reporte de Hojas de Instrucción
            DataTable reporte = SAT_CL.ControlEvidencia.Reportes.CargaReporteHojaInstruccion(
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCompania.Text, ":", 1)),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtCliente.Text, "ID:", 1)),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtRemitente.Text, "ID:", 1)),
                Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtDestinatario.Text, "ID:", 1)),
                txtDescripcion.Text); 
            //Validando que la Tabla contenga 
            if (Validacion.ValidaOrigenDatos(reporte))
            {   //Cargando GridView de Reportes
                Controles.CargaGridView(gvResumen, reporte, "Id", "", true, 1);
                //Añadiendo Tabla a DataSet de Session
                Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], reporte, "Table");
                //Inicializando Indices del GridView
                Controles.InicializaIndices(gvResumen);
            }
            else 
            {   //Inicializando GridView        
                Controles.InicializaGridview(gvResumen);
                //Eliminando Tabla del DataSet de Session
                OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
            }
        }

        #region Eventos GridView "Resumen"

        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Tamaño de Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoResumen_OnSelectedIndexChanged(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resumen" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvResumen, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoResumen.SelectedValue), true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el boton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_OnClick(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Exporta el Contenido del GridView "gvResultado" (Recupera el DataTable del DataSet)
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de página del Gridview "Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Cambia el Indice de la Pagina del GridView
                Controles.CambiaIndicePaginaGridView(gvResumen, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el orden del Gridview "Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumen_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Muestra el Nombre de la columna por la cual se ordena el GridView
                lblOrdenarResumen.Text = Controles.CambiaSortExpressionGridView(gvResumen, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Ver"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVer_OnClick(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Seleccionando fila del GridView
                Controles.SeleccionaFila(gvResumen, sender, "lnk", false);
                //Asignando Variables de Session de "Registro" y "Estatus"
                Session["id_registro"] = gvResumen.SelectedDataKey.Value;
                Session["estatus"] = Pagina.Estatus.Lectura;
                //Insertando Pagina a Pila de Llamadas
               PilaNavegacionPaginas.InsertaPaginaPila("~/ControlEvidencia/VisorHojaInstruccion.aspx", 0, Pagina.Estatus.Nuevo);
                //Direccionando a Hoja de Instrucción
                Response.Redirect("~/ControlEvidencia/HojaInstruccion.aspx");
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el LinkButton "Bitacora"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkBitacora_OnClick(object sender, EventArgs e)
        {   //Validando que el GridView contenga Registros
            if (gvResumen.DataKeys.Count > 0)
            {   //Seleccionando la fila del control que produjo el evento
                Controles.SeleccionaFila(gvResumen, sender, "lnk", false);
                //Invocando Método que inicializa la Bitacora
                inicializaBitacora(Convert.ToInt32(gvResumen.SelectedDataKey.Value), 40, "Hoja Instrucción");
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Pagina
        /// </summary>
        private void inicializaPagina()
        {   //Cargando Catalogos
            cargaCatalogos();
            //Inicializando el GridView
            Controles.InicializaGridview(gvResumen);
            //Enfocando el Control Principal
            txtCliente.Focus();
            this.Form.DefaultButton = btnBuscar.ClientID;
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Instanciamos Compañia  para visualización en el control
            using (SAT_CL.Global.CompaniaEmisorReceptor objCompaniaEmisorReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                txtCompania.Text = objCompaniaEmisorReceptor.nombre + " ID:" + objCompaniaEmisorReceptor.id_compania_emisor_receptor.ToString();
            }
            //Tamaño de los Resumenes
           CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoResumen, "", 26);
        }

        /// <summary>
        /// Configura y muestra ventana de bitácora de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla (Titulo de bitácora)</param>
        private void inicializaBitacora(int id_registro, int id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/ControlEvidencia/HojaInstruccion.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitácora", configuracion, Page);
        }
        #endregion
    }
}