using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.Datos;
using TSDK.ASP;
using SAT_CL.Global;

namespace SAT.Accesorios
{
    public partial class AbrirRegistro : System.Web.UI.Page
    {
        #region Eventos

        /// <summary>
        /// Evento Producido al efectuarse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Validando si se produjo un PostBack
            if (!(Page.IsPostBack))
                //Inicializando Página
                inicializaPagina();
            else
            {   //Validando que exista DS en Session
                if (TSDK.Datos.Validacion.ValidaOrigenDatos((DataSet)Session["DS"], "Abrir"))
                    //Cargando GridView
                    Controles.CargaGridView(gvAbrir, ((DataSet)Session["DS"]).Tables["Abrir"], "Id", "", true, 1);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "ddlFiltro"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {   //Invocando Método que Muestra los Controles
            muestraControles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   //Cargando Reporte
            cargaGridView();
        }

        #region Eventos GridView "Abrir"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "gvAbrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando si existen Registros
            if(gvAbrir.DataKeys.Count > 0)
                //Cambiando Tamaño del GridView
                Controles.CambiaTamañoPaginaGridView(gvAbrir, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Abrir"), Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Ordenamiento del GridView "gvAbrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAbrir_Sorting(object sender, GridViewSortEventArgs e)
        {   //Validando si existen Registros
            if (gvAbrir.DataKeys.Count > 0)
                //Cambiando Expresión del Ordenamiento
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvAbrir, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Abrir"), e.SortExpression, true, 1);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Paginación del GridView "gvAbrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAbrir_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando si existen Registros
            if (gvAbrir.DataKeys.Count > 0)
                //Cambiando Indice de Página
                Controles.CambiaIndicePaginaGridView(gvAbrir, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Abrir"), e.NewPageIndex, true, 1);
        }
        /// <summary>
        /// Evento Producido al Añadir una fila al GridView "gvAbrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAbrir_RowDataBound(object sender, GridViewRowEventArgs e)
        {   //Insertando Fila
            insertaCeldaAbrir(e.Row);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvAbrir.DataKeys.Count > 0)
                //Exportando Contenido del GridView
                Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Abrir"));
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Abrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkbAbrir_Click(object sender, EventArgs e)
        {   //Validando si existen Registros
            if (gvAbrir.DataKeys.Count > 0)
            {   //Seleccionando Fila
                Controles.SeleccionaFila(gvAbrir, sender, "lnk", false);
                //Estableciendo registro
                Session["id_registro"] = gvAbrir.SelectedDataKey.Value;
                //Estableciendo estatus a solo lectura
                Session["estatus"] = 2;
                Session["id_registro_b"] = 0;
                Session["id_registro_c"] = 0;
                Session["Abrir"] = null;
                //Actualizando Ventana Padre
                ScriptServer.ActualizaVentanaPadre(Page);
            }
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {   //Validando que existe la Tabla
            if (Session["id_tabla"] != null)
            {   //Inicializando Catalogos
                cargaCatalogos();
                //Validando que exista un registro a localizar
                if (Session["id_registro"].ToString() != "0")
                {   //Cargando el grid
                    cargaGridView();
                    //Si existen registros mostrados
                    if (gvAbrir.DataKeys.Count > 0)
                        //Selecionando el registro actual en Sesión
                        TSDK.ASP.Controles.MarcaFila(gvAbrir, Session["id_registro"].ToString(), "Id", "Id", (DataSet)Session["DS"], "Abrir", lblOrdenado.Text, Convert.ToInt32(ddlTamano.SelectedValue), true, 1);
                }
                else//Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAbrir);
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Página
        /// </summary>
        private void cargaCatalogos()
        {   //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlFiltro, 5, "TODOS", Convert.ToInt32(Session["id_tabla"]), "", 0, "");//Carga las opciones de filtrado
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 18); //Carga opciones de selección: 5,10,15... registros.
            //Mostrando Controles
            muestraControles();
        }
        /// <summary>
        /// Método Privado encargado de Cargar el Reporte
        /// </summary>
        private void cargaGridView()
        {   //Obteniendo valores QueryString
            string Param1 = Request.QueryString.Get("P1");
            string Param2 = Request.QueryString.Get("P2");
            string Param3 = Request.QueryString.Get("P3");
            string Param4 = Request.QueryString.Get("P4");
            string Param5 = Request.QueryString.Get("P5");
            string Param6 = Request.QueryString.Get("P6");
            //Obteniendo Busqueda
            using(DataSet dsAbrir = SAT_CL.Global.VisorRegistros.CargaRegistrosTabla(Convert.ToInt32(Session["id_tabla"]), Param1, ddlFiltro.SelectedValue, Param3, Param4, Param5, Param6, txtBusqueda.Visible == true? txtBusqueda.Text : ddlBusqueda.SelectedValue))
            {   //Validando que existan Registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dsAbrir, "Table"))
                {   //Cargando GridView
                    Controles.CargaGridView(gvAbrir, dsAbrir.Tables["Table"], "Id", "", true, 1);
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dsAbrir.Tables["Table"], "Abrir");
                }
                else
                {   //Inicializando GridView
                    Controles.InicializaGridview(gvAbrir);
                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Abrir");
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Insertar el botón Abrir en las filas
        /// </summary>
        /// <param name="fila"></param>
        private void insertaCeldaAbrir(GridViewRow fila)
        {   //Declarando variables a utilizar
            TableCell celda = new TableCell();
            ///////Determinando el tipo de celda///////
            //Si es celda de datos
            if (fila.RowType == DataControlRowType.DataRow)
            {   //Instanciando nuevo LinkButton
                LinkButton lkbAbrir = new LinkButton();
                //Asignando Id de control
                lkbAbrir.ID = "lkbAbrir";
                //Estableciendo texto
                lkbAbrir.Text = "Abrir";
                //Estableciendo estilo
                lkbAbrir.CssClass = "LinkButton";
                //Estableciendo evento click
                lkbAbrir.Click += lkbAbrir_Click;
                //Quitando validacion
                lkbAbrir.CausesValidation = false;

                //Instanciando nuevo update panel
                UpdatePanel up = new UpdatePanel();
                up.UpdateMode = UpdatePanelUpdateMode.Conditional;
                up.ContentTemplateContainer.Controls.Add(lkbAbrir);
                //Definiendo trigger sincrono para el link contenido en el panel
                PostBackTrigger trigger = new PostBackTrigger();
                trigger.ControlID = lkbAbrir.ID;
                up.Triggers.Add(trigger);
                //Añadiendo controles (UpdatePanel y LinkButton) a la celda
                celda.Controls.Add(up);
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda encabezado
            else if (fila.RowType == DataControlRowType.Header)
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "EncabezadoGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
            //Si es celda pie
            else
            {
                try
                {   //Estableciendo estilo de celda
                    celda.CssClass = "PieGridViewCSS";
                }
                catch (NullReferenceException)
                {   //Si el estilo no fue definido no se establece
                }
                //Añadiendo celda al grid
                fila.Cells.Add(celda);
            }
        }
        /// <summary>
        /// Método Privado encargado de Mostrar los Controles dependiendo del Tipo de Campo
        /// </summary>
        private void muestraControles()
        {   //Instanciando Columna Filtro
            using (TablaColumnaFiltro tcf = new TablaColumnaFiltro(Convert.ToInt32(ddlFiltro.SelectedValue)))
            {   //Validando que exista el Registro
                if (tcf.id_columna_filtro != 0)
                {   //Validando que sea de Tipo catalogo
                    if (tcf.id_tipo_catalogo != 0 || tcf.id_tabla_catalogo != 0)
                    {   //Habilitando Controles
                        ddlBusqueda.Visible = true;
                        txtBusqueda.Visible = false;
                        txtBusqueda.Enabled = true;
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlBusqueda, 7, "", Convert.ToInt32(ddlFiltro.SelectedValue), "", 0, "");
                    }
                    else
                    {   //Habilitando Controles
                        ddlBusqueda.Visible = false;
                        txtBusqueda.Visible = 
                        txtBusqueda.Enabled = true;
                        //Inicializando Control
                        Controles.InicializaDropDownList(ddlBusqueda, "Ninguna");
                    }

                }
                else
                {
                    //Habilitando Controles
                    ddlBusqueda.Visible = false;
                    txtBusqueda.Visible = true;
                    txtBusqueda.Enabled = false;
                    //Inicializando Control
                    Controles.InicializaDropDownList(ddlBusqueda, "Ninguna");
                }
            }
        }

        #endregion
    }
}