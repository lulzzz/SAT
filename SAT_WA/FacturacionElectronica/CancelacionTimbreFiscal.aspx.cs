using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using SAT_CL;

namespace SAT.FacturacionElectronica
{
    public partial class CancelacionTimbreFiscal : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento disparado al Producirse un PostBack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {   //Si no es un postback se inicializa la forma
            if (!Page.IsPostBack)
            {   //Recarga Pagina
                inicializaPagina();
            }
        }
        /// <summary>
        /// Evento disparado al presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {   //Cargando GridView
            cargaGridView();
        }
        /// <summary>
        /// Evento disparado al presionar el Boton "Reportar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReportar_OnClick(object sender, EventArgs e)
        {
            cancelaTimbres();
        }

        #region Eventos GridView
        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvComprobante.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        TSDK.ASP.Controles.SeleccionaFilasTodas(gvComprobante, "chkVarios", chk.Checked);
                        break;
                }
            }
        }
        /// <summary>
        /// Evento disparado al presionar el LinkButton "Bitacora" o "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDetalles_Click(object sender, EventArgs e)
        {   //Evaluando que el GridView tenga registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Referenciando al botón pulsado
                using (LinkButton boton = (LinkButton)sender)
                {   //Seleccionando la fila actual
                    TSDK.ASP.Controles.SeleccionaFila(gvComprobante, sender, "lnk", false);
                    //Evaluando Boton Presionado
                    switch (boton.CommandName)
                    {
                        case "Bitacora":
                            {   //Visualizando bitácora de registro
                                inicializaBitacoraRegistro(gvComprobante.SelectedDataKey.Value.ToString(), "119", "Bitacora");
                                break;
                            }
                        case "Referencias":
                            {   //Visualizando referencia de registro
                             inicializaReferencias(gvComprobante.SelectedDataKey.Value.ToString(), "119", "Referencias");
                                break;
                            }
                        case "Comprobante":
                            {   //Selecionando la fila actual
                                TSDK.ASP.Controles.SeleccionaFila(gvComprobante, sender, "lnk", false);
                                //Guarda Id del registro seleccionado
                                Session["id_registro"] = gvComprobante.SelectedDataKey.Value;
                                //Cambiando Estatus a Lectura
                                Session["estatus"] = Pagina.Estatus.Lectura;
                                
                                break;
                            }
                        case "Timbre":
                            {   //Se construye el URL de la pagina que se Solicitará
                                String url = "/SIC/ModuloFacturacionElectronica/Operacion/TimbreFiscal.aspx?";
                                //Se agregan el Id de Comprobante de la Fila Seleccionada
                                url += "id_comprobante=" + gvComprobante.SelectedDataKey.Value;
                                //Abre la Pagina con los datos Solicitados
                                TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Timbre Fiscal", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=520,height=450", Page);
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Evento disparado cambiar el criterio de Ordenamiento de GridView "Reportes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobante_OnSorting(object sender, GridViewSortEventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Asignando al Label el Criterio de Ordenamiento
                lblCriterioGridViewComprobante.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvComprobante, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 0);
                //Cargando GridView con DatosTSDK.ASP.Controles.
                TSDK.ASP.Controles.CargaGridView(gvComprobante, (DataSet)Session["DS"], "Table", "Id-Folio", "", true, 4);
            }
        }
        /// <summary>
        /// Evento disparado al cambiar el Indice de pagina del GridView "Reportes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobante_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Cambiando el Indice de Pagina
                TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvComprobante, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 0);
                //Cargando GridVie con Datos
                TSDK.ASP.Controles.CargaGridView(gvComprobante, (DataSet)Session["DS"], "Table", "Id-Folio", "", true, 4);
            }
        }
        /// <summary>
        /// Evento disparado al cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañogvComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvComprobante, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañoGridViewComprobante.SelectedValue), true, 1);
                //Cargando GridView con Datos
                TSDK.ASP.Controles.CargaGridView(gvComprobante, (DataSet)Session["DS"], "Table", "Id-Folio", "", true, 4); 

            }
        }
        /// <summary>
        /// Evento disparado al hacer click al LinkButton Exportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarGv_OnClick(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Exporta el contenido de la tabla cargada en el gridview
                TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                //Cargando GridView con Datos
                TSDK.ASP.Controles.CargaGridView(gvComprobante, (DataSet)Session["DS"], "Table", "Id-Folio", "", true, 4);
            }
        }

        /// <summary>
        /// Evento generado al dar click en Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Carga Reporte
            cargaGridView();
        }
        #endregion

        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Inicializar la Pagina
        /// </summary>
        private void inicializaPagina()
        {   //Carga los Valores de los DropDownList
            cargaCatalogos();
            //Carga los Valores del GridView
            cargaGridView();
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga el catagolo Tipo
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipo, "TODOS", 1107);
            //Asignando Tipo "Ingreso"
            ddlTipo.SelectedValue = "0";
            //Carga el catagolo del DropDownList que tiene el Tamaño del GridView
           CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewComprobante, "", 56);
        }
        /// <summary>
        /// Método encargado de cargar los Valores del GridView
        /// </summary>
        private void cargaGridView()
        {   //Obteniendo DataSet con Resultados
            using (DataTable mit = SAT_CL.FacturacionElectronica.Reporte.CargaCancelacionTimbreFiscal(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,Convert.ToByte(ddlTipo.SelectedValue),
                                  Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(TSDK.Base.Cadena.RegresaCadenaSeparada(txtReceptor.Text, ':',1), "0")),
                                   Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(txtFolio.Text, "0"))))
            {
                //Validando Origen de Datos
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {   //Añadiendo Tabla a Variable de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit);
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvComprobante, mit, "Id-Folio", "", true, 4);
                }
                else
                {
                    //Eliminando origen de datos
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvComprobante);
                }
            }
            //Limpiando Mensaje
            lblError.Text = "";
        }
        /// <summary>
        /// Realiza la solicitud de cancelación de los timbres solicitados
        /// </summary>
        private void cancelaTimbres()
        {
            //inicializando resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Se evalua si existen DataKeys(Indices de Origen de Datos de los Registros en el GridView)
            if (gvComprobante.DataKeys.Count > 0)
            {   //Se obtienen las filas que estan seleccionadas por el CheckBox "chkVarios"
                GridViewRow[] gvfilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvComprobante, "chkVarios");
                //Se evalua si existen filas seleccionadas en el GridView
                if (gvfilas.Length > 0)
                {
                    //Validamos Màximo permitido de Comprobantes
                    if (Convert.ToInt32(gvfilas.Length) <= Convert.ToInt32(CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Maximo Cancelacion CFDI")) )
                     {
                        //Definiendo objeto para almacenar los resultados del proceso
                    string mensajes = "";

                    //Para cada fila seleccionada
                    foreach (GridViewRow r in gvfilas)
                    {
                        //Definiendo objeto de resultado por operación
                        RetornoOperacion res = new RetornoOperacion();

                        //Seleccionando la fila actual
                        gvComprobante.SelectedIndex = r.RowIndex;
                        //Instanciando comprobante actual
                        using (SAT_CL.FacturacionElectronica.Comprobante c = new SAT_CL.FacturacionElectronica.Comprobante(Convert.ToInt32(gvComprobante.SelectedDataKey["Id"])))
                        {
                            //Si existe el comprobante
                            if (c.id_comprobante > 0)
                            {
                                //Realizando cancelación
                              res = new RetornoOperacion(string.Format("{0}: {1}", gvComprobante.SelectedDataKey["Folio"], c.CancelaTimbreFiscalDigital(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario).Mensaje));
                            }
                            else
                                res = new RetornoOperacion(string.Format("{0}: El comprobante no fue localizado.", gvComprobante.SelectedDataKey["Folio"]));
                        }

                        //Añadiendo resultado al general
                        mensajes += res.Mensaje + "<br/>";
                    }

                    //Armando resultado general
                    resultado = new RetornoOperacion(mensajes, true);
                     }
                    else
                        //Mostrando Mensaje
                        resultado = new RetornoOperacion(string.Format("Sólo es posible la cancelación de {0} Comprobantes.", CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Maximo Cancelacion CFDI")));

                }
                else//Mostrando Mensaje
                    resultado = new RetornoOperacion("Debe Seleccionar al menos 1 Comprobante");
            }
            else
                resultado = new RetornoOperacion("No existen comprobantes coincidentes por cancelar.");

            //Actualizando contenido de forma
            cargaGridView();

            //Mostrando resultado
            lblError.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Método encargado de Inicializa los Registros de las Bitacoras
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="titulo">Titulo de la Ventana</param>
        private void inicializaBitacoraRegistro(string id_registro, string id_tabla, string titulo)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobante.DataKeys.Count > 0)
            {   //Declarando variables para armado de URL
                string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/CancelacionTimbreFiscal.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&tB=" + titulo);
                //Instanciando nueva ventana de navegador para apertura de bitacora de registro
                TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Bitacora", 700, 420, false, false, false, true, true, Page);
            }
        }
        /// <summary>
        /// Configura y muestra ventana de referencias de registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="nombre_tabla">Nombre de la Tabla</param>
        private void inicializaReferencias(string id_registro, string id_tabla, string nombre_tabla)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/CancelacionTimbreFiscal.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }

        #endregion

      
    }
}