using SAT_CL;
using System;
using System.Data;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using System.Xml;
using System.Text;
using System.IO;
namespace SAT.FacturacionElectronica
{
    public partial class AddendaEmisor : System.Web.UI.Page
    {
        #region Eventos Addenda
        /// <summary>
        /// Evento generado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Si no hay recarga de la misma página
            if (!this.IsPostBack)
                //Inicializando estatus general de la forma
                inicializaPagina();
        }


        /// <summary>
        /// Evento generado al dar clik en el bóton Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptar_Click(object sender, EventArgs e)
        {

            //Guardamos Addenda
            guardaAddenda();

        }

        /// <summary>
        /// Evento Generado al  Cancelar una Caseta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            //Inicialzando indices del GridView
            Controles.InicializaIndices(gvAddendaEmisor);

            //Cancela Registro
            cancelaRegistro();

            //Inicicializamos Pagina
            inicializaPagina();

        }

        /// <summary>
        /// MEvento generado al dar click en el Menú Caseta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;
            //De acuerdo al nombre de comando asignado 
            switch (boton.CommandName)
            {
                //Establecemos la pagina en estatus Nuevo
                case "Nuevo":
                    {
                        //Establecemos el estatus de la pagina a nuevo 
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Inicializando Id de Registro activo
                        Session["id_registro"] = 0;
                        Session["XML"] = null;
                        Session["XSD"] = null;
                        //Inicializamos la pagina
                        inicializaPagina();
                        break;
                    }
                //Permite abrir registros 
                case "Abrir":
                    {
                        //Inicializando ventana de apertura
                        inicializaAperturaRegistro();
                        break;
                    }
                //Guarda el registro en la BD
                case "Guardar":
                    {
                        //Guardamos el registro
                        //guardaAddenda();
                        break;
                    }

                case "Editar":
                    {
                        //Establecemos el estatus de la pagina a edicion
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //cargando pagina
                        inicializaPagina();
                    }
                    break;

                //Envia al usuario a la pagina principal de la aplicación
                case "Salir":
                    {
                        break;
                    }
                case "Bitacora":
                    {
                        //Mostrando ventana
                        inicializaBitacora(Session["id_registro"].ToString(), "113", "Addenda");
                        break;
                    }
                case "Referencia":
                    {
                        //Preparando consulta de accesorios
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "113",((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Eliminar":
                    {
                        //Deshabilita Addenda
                        deshabilitaAddenda();
                        Session["XML"] = null;
                        break;
                    }
            }
        }

        #endregion

        #region Eventos Addenda Emisor


        /// <summary>
        /// Evento generado al Aceptar la Addenda Emisor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarAddendaEmisor_Click(object sender, EventArgs e)
        {
            //Validamos si Existe Addenda Emisor
            if (gvAddendaEmisor.SelectedIndex != -1)
            {
                //Editamos Addenda Emiso
                editaAddendaEmisor();
            }
            else
            {
                //Guarda Addenda Emisor
                guardaAddendaEmisor();
            }
        }
        /// <summary>
        /// Metodo encargados de inicializar los Controles del GridView Costos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAddendaEmisor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Obtenemos la fila actual
            GridViewRow fila = e.Row;

            //Verificamos si la fila es para los elementos de datos o es la fila del footer
            switch (fila.RowType)
            {
                //Fila tipo Datos, para obtener objetos LinkButton correspondientes
                case DataControlRowType.DataRow:

                    //Creamos instancias 
                    using (TextBox txtReceptorE = (TextBox)fila.FindControl("txtReceptor"))
                    {
                        using (LinkButton lkbDeshabilitarE = (LinkButton)fila.FindControl("lkbDeshabilitarE"),
                                          lkbBitacoraI = (LinkButton)fila.FindControl("lkbBitacoraI"),
                                          lkbEditarE = (LinkButton)fila.FindControl("lkbEditarE")
                                          )
                        {
                            {
                                switch ((Pagina.Estatus)Session["estatus"])
                                {
                                    //Habilitamos campos de acuerdo al estatus de la pagina
                                    case Pagina.Estatus.Nuevo:
                                        {
                                            lkbDeshabilitarE.Enabled =
                                            lkbBitacoraI.Enabled =
                                                // txtXMLPredeterminado.Enabled =
                                            lkbEditarE.Enabled = false;
                                        }
                                        break;
                                    case Pagina.Estatus.Lectura:
                                        {
                                            lkbDeshabilitarE.Enabled = false;
                                            lkbBitacoraI.Enabled = true;
                                            lkbEditarE.Enabled = false;
                                            break;
                                        }
                                    case Pagina.Estatus.Edicion:
                                        {
                                            //Validamos Diferente que no este en Edicion
                                            if (gvAddendaEmisor.SelectedIndex == -1)
                                            {
                                                lkbDeshabilitarE.Enabled =
                                                lkbEditarE.Enabled =
                                                lkbBitacoraI.Enabled = true;
                                            }

                                            break;
                                        }
                                }
                            }

                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Evento generado al dar Click en el Link de Exportar a Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcel_Onclick(object sender, EventArgs e)
        {
            //Validamos Registros
            if (gvAddendaEmisor.DataKeys.Count > 0)
            {
                ///Recúperando Datatable 
                DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
                ///Exporta Grid View
                Controles.ExportaContenidoGridView(mit, "");
            }
        }

        /// <summary>
        /// Evento generado al cerrar la adenda Emisor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarAddendaEmisor_Click(object sender, EventArgs e)
        {
            //Inicializamos Inices
            Controles.InicializaIndices(gvAddendaEmisor);
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarAddendaEmisor, lkbCerrarAddendaEmisor.GetType(), "CerrarVentana", "contenidoConfirmacionAddendaEmisor", "confirmacionAddendaEmisor");
            //Inicializamos Valores
            inicializaValoresAddendaEmisor();
        }

        /// <summary>
        /// Evento generado añ Añadir Addenda Emisor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNuevaAddendaE_Click(object sender, EventArgs e)
        {
            //Selecionando la fila actual
            Controles.InicializaIndices(gvAddendaEmisor);
            //Abrir Ventana Modal
            ScriptServer.AlternarVentana(btnNuevaAddendaE, btnNuevaAddendaE.GetType(), "AbrirVentana", "contenidoConfirmacionAddendaEmisor", "confirmacionAddendaEmisor");
            //Inicializamos Valores
            inicializaValoresAddendaEmisor();
        }

        /// <summary>
        /// Evento Generado al dar clic en los Links de Addenda Emisor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void clickMenuFormaAddendaEmisor_Click(object sender, EventArgs e)
        {
            //Referenciamos al objeto que disparo el evento 
            LinkButton boton = (LinkButton)sender;
            if (gvAddendaEmisor.DataKeys.Count > 0)
            {

                //De acuerdo al nombre de comando asignado 
                switch (boton.CommandName)
                {
                    //Guarda el registro en la BD
                    case "EditarE":
                        {
                            //Selecionando la fila actual
                            Controles.SeleccionaFila(gvAddendaEmisor, sender, "lnk", false);
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(gvAddendaEmisor, gvAddendaEmisor.GetType(), "AbrirVentana", "contenidoConfirmacionAddendaEmisor", "confirmacionAddendaEmisor");
                            //Inicializamos Valores
                            inicializaValoresAddendaEmisor();
                            break;
                        }
                    //Guarda el registro en la BD
                    case "BitacoraI":
                        {
                            //Selecionando la fila actual
                            Controles.SeleccionaFila(gvAddendaEmisor, sender, "lnk", false);

                            //Mostrando ventana
                            inicializaBitacora(gvAddendaEmisor.SelectedValue.ToString(), "112", "Addenda Emisor");
                            break;
                        }
                    //Deshabilita  el registro en la BD
                    case "DeshabilitarE":
                        {
                            //Selecionando la fila actual
                            Controles.SeleccionaFila(gvAddendaEmisor, sender, "lnk", false);
                            //Deshabilitamos Costo
                            deshabilitaAddendaEmisor();
                            break;
                        }
                }
            }
        }


        /// <summary>
        /// Evento genereado al cambiar  Tamaño de Grid View
        /// </summary>
        ///<param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAddendaEmisor_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //Recuperando Datatable
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            ///Cambia Tamaño Grid View 
            Controles.CambiaTamañoPaginaGridView(gvAddendaEmisor, mit, Convert.ToInt32(ddlTamañoGridViewgvAddendaEmisor.SelectedValue), true, 0);
        }

        /// <summary>
        /// Evento Generado al cambiar indice de la Pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAddendaEmisor_OnpageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /// Recuperando Datatable
            DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
            ///Aplicando paginacion al Grid View
            Controles.CambiaIndicePaginaGridView(gvAddendaEmisor, mit, e.NewPageIndex, true, 1);
        }

        #endregion

        #region Metodos Addenda
        /// <summary>
        /// Metodo General de la forma
        /// </summary>
        private void inicializaPagina()
        {

            //Carga catalogos
            cargaCatalogos();
            //Inicializa Valores
            inicializaValores();
            //habilita controles();
            habilitaControles();
            //Inicializa Menu
            inicializaMenu();


        }

        /// <summary>
        /// Carga catalogod
        /// </summary>
        private void cargaCatalogos()
        {

            //Carga catalogo Elemento
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlElemento, "", 1106, 0);
            //Carga catalogo Tamaño Grid View
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañoGridViewgvAddendaEmisor, "", 18, 0);

        }

        /// <summary>
        /// Inicializa Valores de la Caseta
        /// </summary>
        private void inicializaValores()
        {
            //Verificamos estatus de la Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Inicializamos Valores
                        lblID.Text = "";
                        ddlElemento.SelectedValue = "1";
                        txtDescripcion.Text = "";
                        Controles.InicializaGridview(gvXsd);
                        //Inicializa Grid View
                        Controles.InicializaGridview(gvAddendaEmisor);
                        lblErrorAddenda.Text = "";
                        break;
                    }
                case Pagina.Estatus.Lectura:
                case Pagina.Estatus.Edicion:
                    {
                        //Obtenemos datos de la Caseta
                        using (SAT_CL.FacturacionElectronica.Addenda objAddenda = new SAT_CL.FacturacionElectronica.Addenda(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Asignamos Valores de la Caseta
                            lblID.Text = objAddenda.id_addenda.ToString();
                            ddlElemento.SelectedValue = objAddenda.id_elemento_comprobante.ToString();
                            txtDescripcion.Text = objAddenda.descripcion.ToString();
                            //Declaramos tabla para visualizar XSD
                            DataTable mit = new DataTable();
                            mit.Columns.Add("XSD", typeof(string));
                            DataRow r = mit.NewRow();
                            r["XSD"] = objAddenda.xsd_validation.InnerXml;;
                            mit.Rows.Add(r);
                            Controles.CargaGridView(gvXsd,mit, "", "", false,0);
                        }
                        //Carga Addenda Emisor
                        CargaAddendaEmisor();
                        break;
                    }
            }
        }

        /// <summary>
        /// Metodo encargado de habilitar y deshabilitar los controles de la forma 
        /// </summary>
        private void habilitaControles()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        ddlElemento.Enabled =
                        txtDescripcion.Enabled =
                            //afuArchivo.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = true;
                        btnNuevaAddendaE.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        ddlElemento.Enabled =
                        txtDescripcion.Enabled =
                            //afuArchivo.Enabled =
                        btnAceptar.Enabled =
                        btnCancelar.Enabled = false;
                        btnNuevaAddendaE.Enabled =false;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        ddlElemento.Enabled =
                        txtDescripcion.Enabled =
                            //afuArchivo.Enabled =
                        btnAceptar.Enabled =
                        btnNuevaAddendaE.Enabled =
                        btnCancelar.Enabled = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {   //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());
            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();
            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))
                //Cargando Documento XML
                doc.Load(ms);
            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XSD"] = doc;
            System.Web.HttpContext.Current.Session["XSDFileName"] = file_name;


        }
        /// <summary>
        /// Metodo encargado de Inicializar el Menu
        /// </summary>
        private void inicializaMenu()
        {
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = false;
                        lkbBitacora.Enabled = false;
                        lkbReferencias.Enabled = false;
                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = false;
                        lkbEditar.Enabled = true;
                        lkbEliminar.Enabled = false;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {
                        //Habilitamos Controles
                        lkbNuevo.Enabled = true;
                        lkbAbrir.Enabled = true;
                        lkbGuardar.Enabled = true;
                        lkbEditar.Enabled = false;
                        lkbEliminar.Enabled = true;
                        lkbBitacora.Enabled = true;
                        lkbReferencias.Enabled = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// Cancela e Inicializa los valores de un registro acorde al estatus de la página
        /// </summary>
        private void cancelaRegistro()
        {
            //Determinando el estatus que le corresponde
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    Session["estatus"] = (int)Pagina.Estatus.Lectura;
                    break;
                case Pagina.Estatus.Nuevo:
                    Session["estatus"] = (int)Pagina.Estatus.Nuevo;
                    break;
            }
            Session["XSD"] = null;
            Session["XML"] = null;
        }

        /// <summary>
        ///  Metodo encargado de Guardar la Addenda
        /// </summary>
        private void guardaAddenda()
        {
            //Validando que existe el Archivo
            if (Session["XSD"] != null)
            {

                //Obteniendo Documento
                XmlDocument doc = (XmlDocument)Session["XSD"];

                try
                {
                    //Declaracion de objeto resultado
                    RetornoOperacion resultado = new RetornoOperacion();
                    //De acuerdo al estatus de la pagina
                    switch ((Pagina.Estatus)Session["estatus"])
                    {
                        //Insertando Caseta
                        case Pagina.Estatus.Nuevo:
                            {
                                //Validamos Extención de Archivo
                                if (Path.GetExtension(Session["XSDFileName"].ToString()) == ".xsd")
                                {

                                    resultado = SAT_CL.FacturacionElectronica.Addenda.InsertaAddenda(Convert.ToInt32(ddlElemento.SelectedValue),
                                                txtDescripcion.Text, doc, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                                else
                                    //Mostramos etiqueta error
                                    resultado = new RetornoOperacion("El fomato de extensión es incorrecta");
                            }
                            break;
                        //Editando la Caseta en Estatus Edición
                        case Pagina.Estatus.Edicion:
                            {
                                //Obtenemos datos de la Caseta para su edición.
                                using (SAT_CL.FacturacionElectronica.Addenda objAddenda = new SAT_CL.FacturacionElectronica.Addenda(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Editamos Registro
                                    resultado = objAddenda.EditarAddenda(Convert.ToInt32(ddlElemento.SelectedValue),
                                                                        txtDescripcion.Text, objAddenda.xsd_validation, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                }
                            }
                            break;
                    }

                    //Validamos que la operacion se haya realizado
                    if (resultado.OperacionExitosa)
                    {
                        //Cambiamos  Estatus de la Pagina
                        switch ((Pagina.Estatus)Session["estatus"])
                        {
                            case Pagina.Estatus.Nuevo:
                                {
                                    //Establecemos el id del registro
                                    Session["id_registro"] = resultado.IdRegistro;
                                    //Establecemos el estatus de la forma
                                    Session["estatus"] = Pagina.Estatus.Edicion;
                                    //Establecemos Mensaje
                                    lblErrorAddenda.Text = resultado.Mensaje;
                                    //Inicializamos la forma
                                    inicializaPagina();
                                }
                                break;
                            case Pagina.Estatus.Edicion:
                                {
                                    //Establecemos el id del registro
                                    Session["id_registro"] = resultado.IdRegistro;
                                    //Establecemos el estatus de la forma
                                    Session["estatus"] = Pagina.Estatus.Lectura;
                                    //Establecemos Mensaje
                                    lblErrorAddenda.Text = resultado.Mensaje;
                                    //Inicializamos la forma
                                    inicializaPagina();
                                }
                                break;
                        }
                    }
                    else
                    {
                        //Establecemos Mensaje
                        lblErrorAddenda.Text = resultado.Mensaje;
                    }
                }
                catch { }
            }
        }


        /// <summary>
        /// Evento producido al  canbiar el Sorting del Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAddendaEmisor_Onsorting(object sender, GridViewSortEventArgs e)
        {
            ///Validando que existan Registros en el Grid View
            if (gvAddendaEmisor.DataKeys.Count > 0)
            {
                //Recuperando Datatable
                DataTable mit = OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table");
                //Aplicando nuevo criterio de orden
                lblCriterioGridViewAddendaEmisor.Text = Controles.CambiaSortExpressionGridView(gvAddendaEmisor, mit, e.SortExpression, false, 0);
            }
        }


        /// <summary>
        /// Método que inicializa el cuadro de dialogo para apertura de registros
        /// </summary>
        private void inicializaAperturaRegistro()
        {
            //Definiendo el Id de tabla por abrir
            Session["id_tabla"] = 113;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/AddendaEmisor.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Recepción de Facturas", configuracion, Page);

        }


        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/AddendaEmisor.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Bitacora de Cobros", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {
            //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/AddendaEmisor.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }

        /// <summary>
        /// Deshabilita Addenda
        /// </summary>
        private void deshabilitaAddenda()
        {
            //Declaracion de ojeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciando registro actual 
            using (SAT_CL.FacturacionElectronica.Addenda objAddenda = new SAT_CL.FacturacionElectronica.Addenda(Convert.ToInt32(Session["id_registro"])))
            {
                //Deshabilitamos Registro
                resultado = objAddenda.DeshabilitaAddenda(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Si se Deshabilito el registro correctamente
                if (resultado.OperacionExitosa)
                {
                    //Estableciendo estatus a nuevo registro
                    Session["estatus"] = Pagina.Estatus.Nuevo;
                    //Estableciendo el ID de Registro
                    Session["id_registro"] = 0;
                    //Inicialziando la forma
                    inicializaPagina();
                    //Establecemos Etiqueta Error
                    lblErrorAddenda.Text = resultado.Mensaje;
                }
                else
                {
                    //Mostrando Error
                    lblErrorAddenda.Text = resultado.Mensaje;
                }

            }
        }



        #endregion


        #region Metodos Addenda Emisor

        /// <summary>
        /// Deshabilita Addenda
        /// </summary>
        private void deshabilitaAddendaEmisor()
        {
            //Declaracion de ojeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciando registro actual 
            using (SAT_CL.FacturacionElectronica.AddendaEmisor objAddendaEmisor = new SAT_CL.FacturacionElectronica.AddendaEmisor(Convert.ToInt32(gvAddendaEmisor.SelectedValue)))
            {
                //Deshabilitamos Registro
                resultado = objAddendaEmisor.DeshabilitaAddendaEmisor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                //Si se Deshabilito el registro correctamente
                if (resultado.OperacionExitosa)
                {
                    //Inicializamos Indices
                    Controles.InicializaIndices(gvAddendaEmisor);
                    //Carga Addenda Emisor
                    CargaAddendaEmisor();
                }
                else
                {
                    //Mostrando Error
                    lblErrorAdedendaEmisor.Text = resultado.Mensaje;
                }

            }
        }

        /// <summary>
        /// Inicializa Valores Addenda Emisor
        /// </summary>
        private void inicializaValoresAddendaEmisor()
        {
            //Validamos Selección
            if (gvAddendaEmisor.SelectedIndex != -1)
            {
                //Instanciamos Addenda Emisor
                using (SAT_CL.FacturacionElectronica.AddendaEmisor objAddendaEmisor = new SAT_CL.FacturacionElectronica.AddendaEmisor(Convert.ToInt32(gvAddendaEmisor.SelectedValue)))
                {
                    //Instanciamos Receptor
                    using (SAT_CL.Global.CompaniaEmisorReceptor objReceptor = new SAT_CL.Global.CompaniaEmisorReceptor(objAddendaEmisor.id_receptor))
                    {
                        txtReceptor.Text = objReceptor.nombre == null ? "TODOS" : objReceptor.nombre + " ID:" + objReceptor.id_compania_emisor_receptor.ToString();
                    }
                }
            }
            else
            {
                //Limpiamos control
                txtReceptor.Text = "";
                lblErrorAdedendaEmisor.Text = "";
            }
            Session["XML"] = null;
        }

        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesionAddendaEmisor(object context, string file_name)
        {   //Obteniendo Archivo Codificado en UTF8
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] archivoXML = utf8.GetBytes(context.ToString());
            //Declarando Documento XML
            XmlDocument doc = new XmlDocument();
            //Obteniendo XML en cadena
            using (MemoryStream ms = new MemoryStream(archivoXML))
                //Cargando Documento XML
                doc.Load(ms);
            //Guardando en sesión el objeto creado
            System.Web.HttpContext.Current.Session["XML"] = doc;
            System.Web.HttpContext.Current.Session["XMLFileName"] = file_name;


        }

        /// <summary>
        /// Metodo encargado de Guardar la Addenda Emisor
        /// </summary>
        private void guardaAddendaEmisor()
        {
            //Declaramos objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Validando que existe el Archivo
            if (Session["XML"] != null)
            {

                //Obteniendo Documento
                XmlDocument doc = (XmlDocument)Session["XML"];

                try
                {
                    //Validamos Extención de Archivo
                    if (Path.GetExtension(Session["XMLFileName"].ToString()) == ".xml")
                    {
                        //Insertamos el Costo de la Caseta
                        resultado = SAT_CL.FacturacionElectronica.AddendaEmisor.IngresarAddendaEmisor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                                                                         Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtReceptor.Text, ':', 1)),
                                                                       Convert.ToInt32(Session["id_registro"]), doc, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                    }
                    else
                    {
                        //Mostramos etiqueta error
                        resultado = new RetornoOperacion("El fomato de extensión es incorrecta");
                    }
                    //Validamos que la operacion se haya realizado
                    if (resultado.OperacionExitosa)
                    {
                        //Inicializa Indices Grid View
                        Controles.InicializaIndices(gvAddendaEmisor);
                        //Cerrar Ventana Modal
                        ScriptServer.AlternarVentana(btnAceptarAddendaEmisor, btnAceptarAddendaEmisor.GetType(), "CerrarVentana", "contenidoConfirmacionAddendaEmisor", "confirmacionAddendaEmisor");
                        //Inicializamos Valores
                        inicializaValoresAddendaEmisor();
                        //Carga Addenda Emisor
                        CargaAddendaEmisor();
                    }
                    else
                    {
                        lblErrorAdedendaEmisor.Text = resultado.Mensaje;
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Metodo encargado de Guardar la Addenda Emisor
        /// </summary>
        private void editaAddendaEmisor()
        {

            //Declaracion de objeto resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Instanciamos Addenda Emisor
            using (SAT_CL.FacturacionElectronica.AddendaEmisor objAddendaEmisor = new SAT_CL.FacturacionElectronica.AddendaEmisor(Convert.ToInt32(gvAddendaEmisor.SelectedDataKey.Value)))
            {
                //Insertamos el Costo de la Caseta
                resultado = objAddendaEmisor.EditaAddendaEmisor(objAddendaEmisor.id_emisor,
                                                                    Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtReceptor.Text, ':', 1)),
                                                                Convert.ToInt32(Session["id_registro"]), objAddendaEmisor.xml_predeterminado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
            }

            //Validamos que la operacion se haya realizado
            if (resultado.OperacionExitosa)
            {
                //Inicializa Indices Grid View
                Controles.InicializaIndices(gvAddendaEmisor);
                //Cerrar Ventana Modal
                ScriptServer.AlternarVentana(btnAceptarAddendaEmisor, btnAceptarAddendaEmisor.GetType(), "CerrarVentana", "contenidoConfirmacionAddendaEmisor", "confirmacionAddendaEmisor");
                //Inicializamos Valores
                inicializaValoresAddendaEmisor();
                //Carga Addenda Emisor
                CargaAddendaEmisor();

            }
            else
            {
                lblErrorAdedendaEmisor.Text = resultado.Mensaje;
            }
        }


        /// <summary>
        /// Metodo encargado de Cargar la Addenda Emisor
        /// </summary>
        protected void CargaAddendaEmisor()
        {
            //cargando los detalles del registro requisición
            using (DataTable mit = SAT_CL.FacturacionElectronica.AddendaEmisor.CargaAddendaEmisor((Convert.ToInt32(Session["id_registro"])), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                //Validamos Origen de Datos
                if (Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando el GridView correspondiente
                    Controles.CargaGridView(gvAddendaEmisor, mit, "Id", lblCriterioGridViewAddendaEmisor.Text, false, 0);

                    //Asignando origen de datos a sesión
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                }
                else
                {
                    //Cargando el GridView correspondiente
                    Controles.CargaGridView(gvAddendaEmisor, mit, "Id", lblCriterioGridViewAddendaEmisor.Text, false, 0);

                    //Elimina Table
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
        }

        #endregion


    }
}