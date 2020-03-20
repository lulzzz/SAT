using SAT_CL;
using SAT_CL.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using System.Linq;
using TSDK.Datos;
using System.Xml.Linq;
using System.Xml;

namespace SAT.FacturacionElectronica33
{
    public partial class ReporteComprobanteV33 : System.Web.UI.Page
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
            {
                //Recarga Pagina
                inicializaPagina();
            }
        }
        /// <summary>
        /// Evento disparado al presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            //Carga de Grid View
            cargaComprobantes33();
        }
        /// <summary>
        /// Evento disparado al presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            //Inicializamos Pagina
            inicializaPagina();
            //Limpiamos Controles:
            txtReceptor.Text = "";
            txtSerie.Text = "";
            txtFolio.Text = "";
            ddlEstatus.SelectedValue = "0";
            ddlTipo.SelectedValue = "0";
            chkGenerado.Checked = false;
            rdbExpedicion.Checked = true;
            rdbCaptura.Checked = false;
            rdbCancelacion.Checked = false;
        }
        /// <summary>
        /// Evento disparado al presionar el LinkButton "Bitacora" o "Referencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbDetalles_Click(object sender, EventArgs e)
        {   //Evaluando que el GridView tenga registros
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Referenciando al botón pulsado
                using (LinkButton boton = (LinkButton)sender)
                {   //Seleccionando la fila actual
                    Controles.SeleccionaFila(gvComprobantes, sender, "lnk", false);
                    //Evaluando Boton Presionado
                    switch (boton.CommandName)
                    {
                        case "Bitacora":
                            {   //Visualizando bitácora de registro
                                inicializaBitacoraRegistro(gvComprobantes.SelectedValue.ToString(), "209", "Bitacora");
                                break;
                            }
                        case "Referencias":
                            {   //Visualizando referencia de registro
                                inicializaReferencias(gvComprobantes.SelectedValue.ToString(), "209", "Combrobante");
                                break;
                            }
                        case "Email":
                            {
                                //Inicializando contenido de controles de envío de correo
                                //Instanciando comprobante de interés
                                using (SAT_CL.FacturacionElectronica33.Comprobante comp = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantes.SelectedDataKey["Id"])))
                                {
                                    //Si hay comprobante timbrado
                                    if (comp.bit_generado)
                                    {
                                        //Instanciando compañía de interés
                                        using (SAT_CL.Global.CompaniaEmisorReceptor c = new SAT_CL.Global.CompaniaEmisorReceptor(comp.id_compania_emisor))
                                        {
                                            string destinatarios = "";
                                            //Cargando contactos (Destinatarios)
                                            using (DataTable mitContactos = SAT_CL.Global.Referencia.CargaReferencias(comp.id_compania_receptor, 25, 2058))
                                            {
                                                //Si hay elementos
                                                if (mitContactos != null)
                                                {
                                                    foreach (DataRow r in mitContactos.Rows)
                                                    {
                                                        //Si ya existe contenido en el control
                                                        if (destinatarios != "")
                                                            destinatarios = destinatarios + ";\r\n" + r.Field<string>("Valor");
                                                        //De lo contrario
                                                        else
                                                            destinatarios = r.Field<string>("Valor");
                                                    }
                                                }
                                            }
                                            //Inicializando control de envío de comprobante
                                            wucEmailCFDI.InicializaControl(((SAT_CL.Seguridad.Usuario)Session["usuario"]).email, string.Format("CFDI {0} [{1}]", comp.serie + comp.folio.ToString(), c.rfc), destinatarios,
                                                "Los archivos se encuentran adjuntos en este mensaje. Si usted no ha solicitado el envío de este comprobante, por favor contacte a su ejecutivo de cuenta.", comp.id_comprobante33);
                                        }
                                    }
                                    else
                                        ScriptServer.MuestraNotificacion(this, "El comprobante no se ha timbrado.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                //Abrir Ventana Modal
                                ScriptServer.AlternarVentana(this, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");
                                break;
                            }
                        case "XML":
                            {
                                //Instanciamos Comprobante
                                using (SAT_CL.FacturacionElectronica33.Comprobante c = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantes.SelectedValue)))
                                {
                                    //Si existe y está generado
                                    if (c.bit_generado)
                                    {
                                        //Obteniendo bytes del archivo XML
                                        byte[] cfdi_xml = System.IO.File.ReadAllBytes(c.ruta_xml);

                                        //Realizando descarga de archivo
                                        if (cfdi_xml.Length > 0)
                                        {
                                            //Instanciando al emisor
                                            using (SAT_CL.Global.CompaniaEmisorReceptor em = new SAT_CL.Global.CompaniaEmisorReceptor(c.id_compania_emisor))
                                                TSDK.Base.Archivo.DescargaArchivo(cfdi_xml, string.Format("{0}_{1}{2}.xml", em.nombre_corto != "" ? em.nombre_corto : em.rfc, c.serie, c.folio), TSDK.Base.Archivo.ContentType.binary_octetStream);
                                        }
                                    }
                                }
                                break;
                            }
                        case "PDF":
                            {
                                //Obteniendo Ruta
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/ReporteComprobante.ascx", "~/RDLC/Reporte.aspx");
                                //Instanciamos Comprobante
                                using (SAT_CL.FacturacionElectronica33.Comprobante objComprobante = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantes.SelectedValue)))
                                {
                                    //Validamos que el comprobante se encuentre Timbrado
                                    if (objComprobante.bit_generado)
                                    {
                                        //Validando Tipo de Comprobante
                                        switch (objComprobante.id_tipo_comprobante)
                                        {
                                            case 1: /** 1.- Ingreso **/
                                            case 2: /** 2.- Egreso **/
                                            case 3: /** 3.- Traslado **/
                                                {
                                                    //Instanciando nueva ventana de navegador para apertura de registro
                                                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteV33", objComprobante.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                                    break;
                                                }
                                            case 4: /** 4.- Nómina **/
                                                {
                                                    //Obteniendo Nómina de Empleado
                                                    using (SAT_CL.Nomina.NomEmpleado ne = new SAT_CL.Nomina.NomEmpleado(SAT_CL.Nomina.NomEmpleado.ObtieneIdNomEmpleadoV3_3(objComprobante.id_comprobante33)))
                                                    {
                                                        //Validando Nómina de Empleado
                                                        if (ne.habilitar)

                                                            //Instanciando nueva ventana de navegador para apertura de registro
                                                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteNominaN12v33", ne.id_nomina_empleado), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                                        else
                                                            //Mostrando Excepción
                                                            ScriptServer.MuestraNotificacion(this.Page, new RetornoOperacion("No se puede recuperar la Nómina del Empleado"), ScriptServer.PosicionNotificacion.AbajoDerecha);
                                                    }
                                                    break;
                                                }
                                            case 5: /** 5.- Pago **/
                                                {
                                                    //Instanciando nueva ventana de navegador para apertura de registro
                                                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobantePago", objComprobante.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                                    break;
                                                }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }
        /// <summary>
        /// Click en botón enviar email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEmailCFDI_BtnEnviarEmail_Click(object sender, EventArgs e)
        {
            //Envíando correo
            RetornoOperacion resultado = wucEmailCFDI.EnviaEmail(true, true);
            //Si no hay errores
            if (resultado.OperacionExitosa)
                //Cerrando ventana modal
                ScriptServer.AlternarVentana(this, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(this, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Click en botón cerrar ventana modal de envío de comprobante por email
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void wucEmailCFDI_LkbCerrarEmail_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(this, "EnvioEmail", "contenidoConfirmacionEmail", "confirmacionEmail");
        }
        /// <summary>
        /// Evento disparado al cambiar CheckedChanged propiedad de CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   //Validando Si el GridView contiene Registros
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Evalua el ID del CheckBox en el que se produce el cambio
                switch (((CheckBox)sender).ID)
                {   //Caso para el CheckBox "Todos"
                    case "chkTodos"://Se crea un CheckBox donde se le asigna el Control CheckBox con el ID "chkTodos"
                        CheckBox chk = (CheckBox)gvComprobantes.HeaderRow.FindControl("chkTodos");
                        //Asigna el Valor de "ChkTodos" a todos los Controles CheckBox 
                        Controles.SeleccionaFilasTodas(gvComprobantes, "chkVarios", chk.Checked);
                        break;
                }
            }
        }
        /// <summary>
        /// Evento disparado al Dar Click en el LinkButton "lkbSolicitud"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportar_OnClick(object sender, EventArgs e)
        {
            //Exportamos Archivos
            Exportar();
        }
        /// <summary>
        /// Evento disparado cambiar el criterio de Ordenamiento de GridView "Comprobante"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantes_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Asignando al Label el Criterio de Ordenamiento
                lblCriteriogvComprobantes.Text = Controles.CambiaSortExpressionGridView(gvComprobantes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
                //Añadimos total
                gvComprobantes.FooterRow.Cells[20].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")).ToString());
            }
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al cambiar el Indice de pagina del GridView "Reportes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantes_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Verificando que existan registro en el GridView
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Cambiando el Indice de Pagina
                Controles.CambiaIndicePaginaGridView(gvComprobantes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
                //Añadimos total
                gvComprobantes.FooterRow.Cells[20].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")).ToString());
            }
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al cambiar el Tamaño del GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamañogvComprobantes_SelectedIndexChanged(object sender, EventArgs e)
        {   //Verificando que existan registro en el GridView
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvComprobantes, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamañogvComprobantes.SelectedValue), true, 3);
                //Añadimos total
                gvComprobantes.FooterRow.Cells[20].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")).ToString());
            }
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al hacer click al LinkButton Exportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarExcel_OnClick(object sender, EventArgs e)
        {
            //Referenciando al botón pulsado
            using (LinkButton boton = (LinkButton)sender)
            {
                //Evaluando Boton Presionado
                switch (boton.CommandName)
                {
                    case "Facturas":
                        {
                            //Exporta el contenido de la tabla cargada en el gridview
                            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                            break;
                        }
                }
            }
        }

        #endregion

        #region Métodos
        
        /// <summary>
        /// Método encaragdo de Inicializar la Pagina
        /// </summary>
        private void inicializaPagina()
        {
            //Carga Catalogos de la Forma
            cargaCatalogos();
            //Carga el GridView
            Controles.InicializaGridview(gvComprobantes);
        }
        /// <summary>
        /// Método encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Carga Catalogo de Estatus
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "Todos", 3200);
            
            //Carga Catalogo de tipos, segun la compania actual
            CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipo, 186, "--TODOS [*]--", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "",0,"");
            
            //Carga el catagolo del DropDownList que tiene el Tamaño del GridView
            CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamañogvComprobantes, "", 26);
        }
        /// <summary>
        /// Método encargado de Cargar los Valores (ComprobanteV33) en el GridView
        /// </summary>
        private void cargaComprobantes33()
        {
            //Declaramos variables
            DateTime inicioFecha, finFecha, inicioFechaExpedicion, finFechaExpedicion, inicioFechaCaptura, finFechaCaptura, inicioFechaCancelacion, finFechaCancelacion;
            //Inicializamos Variables
            inicioFechaExpedicion =
            finFechaExpedicion =
            inicioFechaCaptura =
            finFechaCaptura =
            inicioFechaCancelacion =
            finFechaCancelacion = DateTime.MinValue;
            DateTime.TryParse(txtFechaInicio.Text, out inicioFecha);
            DateTime.TryParse(txtFechaFin.Text, out finFecha);            
            
            //Si la fecha de inicio es mayor que la fecha fin
            if ((inicioFecha.CompareTo(finFecha)) < 0 || (inicioFecha == DateTime.MinValue && finFecha == DateTime.MinValue))
            {
                //Validamos selección de fechas
                if (rdbExpedicion.Checked)
                {
                    //Obteniendo fechas
                    inicioFechaExpedicion = inicioFecha;
                    finFechaExpedicion = finFecha;
                }
                //Validamos selección de fechas
                if (rdbCaptura.Checked)
                {
                    //Convirtiendo fechas a cadena
                    inicioFechaCaptura = inicioFecha;
                    finFechaCaptura = finFecha;
                }
                //Validamos selección de fechas
                if (rdbCancelacion.Checked)
                {
                    //Convirtiendo fechas a cadena
                    inicioFechaCancelacion = inicioFecha;
                    finFechaCancelacion = finFecha;
                }
                // Separar cada parámetro y asignar el valor, para enviar al método CargaComprobantes33 de Reporte.cs
                int IdCompEm = ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor;
                int IdCompRe = Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtReceptor.Text, ':', 1), "0"));
                byte Tipo = Convert.ToByte(ddlTipo.SelectedValue);
                byte Estatus = Convert.ToByte(ddlEstatus.SelectedValue);
                DateTime IFExp = inicioFechaExpedicion;
                DateTime FFExp = finFechaExpedicion;
                bool Generado = Convert.ToBoolean(chkGenerado.Checked);
                string Serie = Cadena.VerificaCadenaVacia(txtSerie.Text, "0");
                int Folio =Convert.ToInt32(Cadena.VerificaCadenaVacia(txtFolio.Text, "0"));
                DateTime IFCap =inicioFechaCaptura;
                DateTime FFCap =finFechaCaptura;
                DateTime IFCan =inicioFechaCancelacion;
                DateTime FFCan =finFechaCancelacion;
                int Usuario =Convert.ToInt32(Cadena.VerificaCadenaVacia(Cadena.RegresaCadenaSeparada(txtUsuarioTimbra.Text, ':', 1), "0"));

                //Obtenemos resultado del reporte generado
                using (DataTable mit = SAT_CL.FacturacionElectronica33.Reporte.CargaComprobantes33(IdCompEm,IdCompRe,Tipo,Estatus,IFExp,FFExp,Generado,Serie,Folio,IFCap,FFCap,IFCan,FFCan,Usuario))
                {
                    //Validando origen de datos
                    if (Validacion.ValidaOrigenDatos(mit))
                    {
                        //Cargando GridView con Datos
                        Controles.CargaGridView(gvComprobantes, mit, "Id-IdFactura", lblCriteriogvComprobantes.Text, true, 3);
                        //Guardando tabla en session
                        Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table");
                    }
                    else
                    {
                        //Eliminando tablas del dataset de session
                        Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                        //Cargando GridView Vacío
                        Controles.InicializaGridview(gvComprobantes);
                    }
                }
            }
            else
            {
                lblError.Text = "La FECHA INICIO debe ser menor que la FECHA FIN.";
            }
        }
        /// <summary>
        /// Método encargado de Sumar los Totales del Reporte
        /// </summary>
        private void sumaTotales()
        {
            //Validando que Exista la Tabla
            if (TSDK.Datos.Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table")))
            {
                //Mostrando Totales
                gvComprobantes.FooterRow.Cells[14].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Flete)", "")));
                gvComprobantes.FooterRow.Cells[15].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Renta)", "")));
                gvComprobantes.FooterRow.Cells[16].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Estadias)", "")));
                gvComprobantes.FooterRow.Cells[17].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Casetas)", "")));
                gvComprobantes.FooterRow.Cells[18].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Otros)", "")));
                gvComprobantes.FooterRow.Cells[19].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Subtotal)", "")));
                gvComprobantes.FooterRow.Cells[20].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Descuento)", "")));
                gvComprobantes.FooterRow.Cells[21].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IVATrasladado)", "")));
                gvComprobantes.FooterRow.Cells[22].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(IVARetenido)", "")));
                gvComprobantes.FooterRow.Cells[23].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Impuestos)", "")));
                gvComprobantes.FooterRow.Cells[24].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(Total)", "")));
                gvComprobantes.FooterRow.Cells[26].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(MontoAplicado)", "")));
                gvComprobantes.FooterRow.Cells[27].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(SaldoActual)", "")));
            }
            else
            {
                //Mostrando Totales en Cero
                gvComprobantes.FooterRow.Cells[14].Text =
                gvComprobantes.FooterRow.Cells[15].Text =
                gvComprobantes.FooterRow.Cells[16].Text =
                gvComprobantes.FooterRow.Cells[17].Text =
                gvComprobantes.FooterRow.Cells[18].Text =
                gvComprobantes.FooterRow.Cells[19].Text =
                gvComprobantes.FooterRow.Cells[20].Text =
                gvComprobantes.FooterRow.Cells[21].Text =
                gvComprobantes.FooterRow.Cells[22].Text =
                gvComprobantes.FooterRow.Cells[23].Text =
                gvComprobantes.FooterRow.Cells[24].Text =
                gvComprobantes.FooterRow.Cells[26].Text =
                gvComprobantes.FooterRow.Cells[27].Text = string.Format("{0:C2}", 0);
            }
        }
        /// <summary>
        /// Método que inicializa el control bitácora del registro
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="titulo">TItulo a mostrar</param>
        private void inicializaBitacoraRegistro(string idRegistro, string idTabla, string titulo)
        {
            //Declarando variables para armado de URL
            string urlDestino = Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ReporteComprobanteV33.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + titulo);
            //Instanciando nueva ventana de navegador para apertura de bitacora de registro
            ScriptServer.AbreNuevaVentana(urlDestino, "Bitacora", 700, 420, false, false, false, true, true, Page);
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
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica33/ReporteComprobanteV33.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor + "&tB=" + nombre_tabla);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "Referencias", configuracion, Page);
        }
        /// <summary>
        /// Exportar PDF y XML
        /// </summary>
        private void Exportar()
        {
            //Creamos lista de archivos
            List<KeyValuePair<string, byte[]>> archivos = new List<KeyValuePair<string, byte[]>>();
            //Creamos lista errores
            List<string> errores = new List<string>();
            //Verificando que el GridView contiene Registros
            if (gvComprobantes.DataKeys.Count > 0)
            {   //Obteniendo Filas Seleccionadas
                GridViewRow[] selected_rows = Controles.ObtenerFilasSeleccionadas(gvComprobantes, "chkVarios");
                //Verificando que existan filas Seleccionadas
                if (selected_rows.Length != 0)
                {
                    //Almacenando Rutas el Arreglo
                    foreach (GridViewRow row in selected_rows)
                    {   //Instanciando Comprobante de el Valor obtenido de la Fila Seleccionada
                        using (SAT_CL.FacturacionElectronica33.Comprobante comp =
                            new SAT_CL.FacturacionElectronica33.Comprobante
                                (Convert.ToInt32(gvComprobantes.DataKeys[row.RowIndex].Value)))
                        {
                            //Validamos Seleccion de Radio Buton de PDF
                            if (chkPDF.Checked == true)
                            {
                                //Declarando Arreglo de Bytes
                                byte[] pdfs = null;
                                
                                //Validando Tipo de Comprobante
                                switch (comp.id_tipo_comprobante)
                                {
                                    case 1: /** 1.- Ingreso **/
                                    case 2: /** 2.- Egreso **/
                                    case 3: /** 3.- Traslado **/
                                    default:
                                        //Obteniendo PDF en bytes
                                        pdfs = comp.GeneraPDFComprobanteV33();
                                        break;
                                    case 4: /** 4.- Nómina **/
                                        {
                                            //Obteniendo Nómina de Empleado
                                            using (SAT_CL.Nomina.NomEmpleado ne = new SAT_CL.Nomina.NomEmpleado(SAT_CL.Nomina.NomEmpleado.ObtieneIdNomEmpleadoV3_3(comp.id_comprobante33)))
                                            {
                                                //Validando Nómina de Empleado
                                                if (ne.habilitar)

                                                    //Obteniendo PDF en bytes
                                                    pdfs = ne.GeneraPDFComprobanteNomina33();
                                                else
                                                    //Vaciando Bytes
                                                    pdfs = null;
                                            }
                                            break;
                                        }
                                    case 5: /** 5.- Pago **/
                                        //Obteniendo PDF en bytes
                                        pdfs = comp.GeneraPDFComprobantePagoV33();
                                        break;
                                }
                                
                                //Añadimos PDF
                                archivos.Add(new KeyValuePair<string, byte[]>(comp.serie + comp.folio.ToString() + ".pdf", pdfs));
                            }
                            //Validando Selección de XML
                            if (chkXML.Checked == true)
                            {
                                //Verificando que exista el Archivo
                                if (File.Exists(comp.ruta_xml))
                                {
                                    //Guardando Archivo en arreglo de Bytes
                                    byte[] xml_file = System.IO.File.ReadAllBytes(comp.ruta_xml);
                                    //Añadimos XML
                                    archivos.Add(new KeyValuePair<string, byte[]>(comp.serie + comp.folio.ToString() + ".xml", xml_file));
                                }
                            }
                        }
                    }
                    //Genera el zip con las rutas
                    byte[] file_zip = Archivo.ConvirteArchivoZIP(archivos, out errores);
                    //Si almenos un archivo fue correcto descarga 
                    if (file_zip != null)
                    {
                        //Instanciando Excepción
                        using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Validando Compania
                            if (cer.habilitar)

                                //Descarga el zip generado
                                Archivo.DescargaArchivo(file_zip, string.Format("{0}_FACTURAS_{1:yyyyMMdd_HHmmss}.zip", string.IsNullOrEmpty(cer.nombre_corto.Trim()) ? cer.nombre_corto.ToUpper() : cer.rfc, Fecha.ObtieneFechaEstandarMexicoCentro()),
                                                           Archivo.ContentType.binary_octetStream);
                        }
                    }
                    else
                    {
                        //Recorremos errores
                        foreach (string error in errores)
                        {
                            //Muestra mensaje de Error
                            lblError.Text += error + " <br>";
                        }
                    }
                }
                else//Mostrando Mensaje
                    lblError.Text = "Debe Seleccionar al menos un Comprobante";
            }
        }
        /// <summary>
        /// Muestra u oculta la ventana modal solicitada
        /// </summary>
        /// <param name="nombre_ventana">Nombre de ventana por ocultar</param>
        /// <param name="control">Control que solicita la acción</param>
        private void alternaVentanaModal(string nombre_ventana, System.Web.UI.Control control)
        {
            //Determinando la ventana por alternar
            switch (nombre_ventana)
            {
                case "CancelarCFDI":
                    ScriptServer.AlternarVentana(control, nombre_ventana, "contenidoConfirmacionCancelacionCFDI", "confirmacionCancelacionCFDI");
                    break;
            }
        }
        /// <summary>
        /// Método encargado de Buscar las Aplicaciones
        /// </summary>
        private void cargaAplicaciones()
        {
            //Instanciando Aplicaciones
            using (DataTable dtAplicaciones = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(0, Convert.ToInt32(gvComprobantes.SelectedDataKey["IdFactura"])))
            {
                //Validando que existan Aplicaciones
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAplicaciones))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvAplicaciones, dtAplicaciones, "Id", "", true, 2);
                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAplicaciones, "Table2");
                    //Cambiando a Vista de Aplicaciones
                    mtvCancelacionCFDI.ActiveViewIndex = 1;
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAplicaciones);
                    //Inicializando GridView
                    Controles.InicializaGridview(gvAplicaciones);
                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                    //Cambiando a Vista de Mensaje
                    mtvCancelacionCFDI.ActiveViewIndex = 0;
                }
            }
        }
        /// <summary>
        /// Cancelar CFDI
        /// </summary>
        private RetornoOperacion cancelaCFDI()
        {
            //Declaramos resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Inicializando Bloque Transaccional
            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciamos Comprobamte
                using (SAT_CL.FacturacionElectronica33.Comprobante objCompobante = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantes.SelectedDataKey["Id"])))
                {
                    //Validando Existencia
                    if (objCompobante.habilitar)
                    {
                        //Validado Tipo de Comprobante
                        switch (objCompobante.id_tipo_comprobante)
                        {
                            case 1:
                            case 2:
                            case 3:
                            default:
                                {
                                    //Asignando Retorno Positivo
                                    resultado = new RetornoOperacion(objCompobante.id_comprobante33);
                                    break;
                                }
                            case 4:
                                {
                                    //Asignando Retorno Positivo
                                    resultado = new RetornoOperacion("No puede Cancelar el Comprobante de Nómina desde esta Pantalla");
                                    break;
                                }
                            case 5:
                                {
                                    //Asignando Retorno Positivo
                                    resultado = new RetornoOperacion("No puede Cancelar el Comprobante de Pago desde esta Pantalla");
                                    break;
                                }
                        }

                        //Validando Retorno
                        if (resultado.OperacionExitosa)
                        {
                            //Enviamos link
                            resultado = objCompobante.CancelacionPendiente(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validamos Resultado
                            if (resultado.OperacionExitosa)
                            {
                                //Insertando Referencia
                                resultado = SAT_CL.Global.Referencia.InsertaReferencia(objCompobante.id_comprobante33, 209, SAT_CL.Global.ReferenciaTipo.ObtieneIdReferenciaTipo(0, 209, "Motivo Cancelación", 0, "Facturacion Electrónica"),
                                            txtMotivo.Text, TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario, true);
                                //Validando Operación Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Deshabilitando Aplicaciones al Cancelar el Comprobante
                                    //TODO: Validar si esta función es necesaria aqui
                                    //resultado = SAT_CL.CXC.FichaIngresoAplicacion.DeshabilitaAplicacionesFacturado(Convert.ToInt32(gvComprobantes.SelectedDataKey["IdFactura"]), objCompobante.id_comprobante33, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Cerramo Ventana Modal
                                        alternaVentanaModal("CancelarCFDI", btnAceptarCancelacionCFDI);
                                        //Carga Comprobante
                                        cargaComprobantes33();
                                        //Completando Transacción
                                        trans.Complete();
                                    }
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("No se puede recuperar el Comprobante");
                }
            }

            //Devolvemos Valor
            return resultado;
        }

        #endregion

        /// <summary>
        /// Evento generado al dar click en Cancelar CFDI de la Factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCancelarCFDI_Click(object sender, EventArgs e)
        {
            //Valida si existen Comprobantes
            if (gvComprobantes.DataKeys.Count > 0)
            {
                //Seleccionando la fila correspondiente
                Controles.SeleccionaFila(gvComprobantes, sender, "lnk", false);
                //Cargamos Aplicaciones
                cargaAplicaciones();
                //Mostramos ventana modal 
                alternaVentanaModal("CancelarCFDI", gvComprobantes);
            }
        }
        /// <summary>
        /// Evento generado al Cancelar  la  Cancelación de un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            alternaVentanaModal("CancelarCFDI", btnCancelarCancelacionCFDI);
        }
        /// <summary>
        /// Evento generado al Aceptar la Cancelación un CFDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelacionCFDI_Click(object sender, EventArgs e)
        {
            //Cancelamos Factura
            RetornoOperacion resultado = cancelaCFDI();
            //Mostrando resultado
            ScriptServer.MuestraNotificacion(btnAceptarCancelacionCFDI, resultado, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Enlace de datos de cada fila del GV de Comprobantes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Si hay registros que enlazar
            if (gvComprobantes.DataKeys.Count > 0)
            {
                //Determianndo si la fila es del tipo de interés (fila de datos)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Recuperando origen de datos de la fila
                    DataRow row = ((DataRowView)e.Row.DataItem).Row;
                    //Si la fila corresponde a un CFDI cancelado
                    if (row.Field<string>("Estatus") == "Cancelado")
                        //Se modifica el color de fondo de la fila
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFB5C2");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbAccionCFDI_Click(object sender, ImageClickEventArgs e)
        {
            //Validando Datos
            if (gvComprobantes.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion retorno = new RetornoOperacion();

                //Seleccionando Fila
                Controles.SeleccionaFila(gvComprobantes, sender, "imb", false);
                
                //Instanciando CFDI
                using (SAT_CL.FacturacionElectronica33.Comprobante cfdi = new SAT_CL.FacturacionElectronica33.Comprobante(Convert.ToInt32(gvComprobantes.SelectedDataKey["Id"])))
                using (SAT_CL.FacturacionElectronica33.TimbreFiscalDigital tfd = SAT_CL.FacturacionElectronica33.TimbreFiscalDigital.RecuperaTimbreFiscalComprobante(cfdi.id_comprobante33))
                using (SAT_CL.Global.CompaniaEmisorReceptor emisor = new SAT_CL.Global.CompaniaEmisorReceptor(cfdi.id_compania_emisor))
                using (SAT_CL.Global.CompaniaEmisorReceptor receptor = new SAT_CL.Global.CompaniaEmisorReceptor(cfdi.id_compania_receptor))
                {
                    //Validando Existencia
                    if (cfdi.habilitar && tfd.habilitar)
                    {
                        //Validando Comando
                        switch (((ImageButton)sender).CommandName)
                        {
                            case "Cancelacion":
                                {
                                    //Declarando Variables Auxiliares
                                    SAT_CL.FacturacionElectronica33.CancelacionCDFI.RespuestaConsultaCFDI respuestaConsulta = SAT_CL.FacturacionElectronica33.CancelacionCDFI.RespuestaConsultaCFDI.SinRespuesta;
                                    XDocument acuse = null;

                                    //Cancelando Comprobante
                                    retorno = SAT_CL.FacturacionElectronica33.CancelacionCDFI.objCancelacion.ConsultaComprobanteCxC(cfdi.id_comprobante33, out respuestaConsulta, out acuse);

                                    //Colocando resultado sobre controles
                                    imgAcuseSAT.Src = retorno.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                                    headerAcuseSAT.InnerText = retorno.Mensaje;
                                    lblRFCEmisorAC.Text = emisor.rfc;
                                    lblRFCReceptorAC.Text = receptor.rfc;
                                    lblUUIDAC.Text = tfd.UUID;
                                    lblTotalFacturaAC.Text = cfdi.total_captura.ToString("C");
                                    lblFechaExpedicionAC.Text = cfdi.fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                                    //Asignando Valores Cancelación
                                    lblEstatusSAT.CssClass = acuse.Root.Element("Estado").Value.Equals("Vigente") ? "label_correcto" : "label_error";
                                    lblEstatusSAT.Text = acuse.Root.Element("Estado").Value;
                                    lblEsCancelableAC.CssClass = acuse.Root.Element("EsCancelable").Value.Contains("Cancelable") ? "label_negrita" : "label_error";
                                    lblEsCancelableAC.Text = acuse.Root.Element("EsCancelable").Value;

                                    //Mostrando Ventana Modal
                                    ScriptServer.AlternarVentana(this.Page, "CancelacionSAT", "contenedorVentanaConsultaAcuseCancelacion", "ventanaConsultaAcuseCancelacion");
                                    break;
                                }
                            case "Validacion":
                                {
                                    //Declarando auxiliares
                                    string rfc_emisor, rfc_receptor, UUID;
                                    decimal monto; DateTime fecha_expedicion;

                                    //Obteniendo Archivo
                                    byte[] comp = System.IO.File.ReadAllBytes(cfdi.ruta_xml);

                                    //Declarando Documento XML
                                    XmlDocument xmlDocument = new XmlDocument();
                                    XDocument xDocument = new XDocument();

                                    //Obteniendo XML en cadena
                                    using (MemoryStream ms = new MemoryStream(comp))
                                        //Cargando Documento XML
                                        xmlDocument.Load(ms);

                                    //Convirtiendo XML
                                    xDocument = TSDK.Base.Xml.ConvierteXmlDocumentAXDocument(xmlDocument);

                                    //Validando versión
                                    switch (xDocument.Root.Attribute("version") != null ? xDocument.Root.Attribute("version").Value : xDocument.Root.Attribute("Version").Value)
                                    {
                                        case "3.2":
                                        case "3.3":
                                            {
                                                //Realizando validación de estatus en SAT
                                                retorno = SAT_CL.FacturacionElectronica.Comprobante.ValidaEstatusPublicacionSAT(xmlDocument, out rfc_emisor, out rfc_receptor, out monto, out UUID, out fecha_expedicion);

                                                //Colocando resultado sobre controles
                                                imgValidacionSAT.Src = retorno.OperacionExitosa ? "../Image/Exclamacion.png" : "../Image/ExclamacionRoja.png";
                                                headerValidacionSAT.InnerText = retorno.Mensaje;
                                                lblRFCEmisor.Text = rfc_emisor;
                                                lblRFCReceptor.Text = rfc_receptor;
                                                lblUUID.Text = UUID;
                                                lblTotalFactura.Text = monto.ToString("C");
                                                lblFechaExpedicion.Text = fecha_expedicion.ToString("dd/MM/yyyy HH:mm");
                                                break;
                                            }
                                    }

                                    //Mostrando Ventana Modal
                                    ScriptServer.AlternarVentana(this.Page, "ValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
                                    break;
                                }
                        }
                    }
                    else
                        //Instanciando Excepción
                        retorno = new RetornoOperacion("No se puede recuperar el CFDI");
                }

                //Mostrando Resultado
                ScriptServer.MuestraNotificacion(this.Page, retorno, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarValidacionSAT_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "CancelacionSAT", "contenedorVentanaConsultaAcuseCancelacion", "ventanaConsultaAcuseCancelacion");
            //Inicializando Indices
            Controles.InicializaIndices(gvComprobantes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnValidacionSAT_Click(object sender, EventArgs e)
        {
            //Mostrando Ventana Modal
            ScriptServer.AlternarVentana(this.Page, "ValidacionSAT", "contenidoResultadoConsultaSATModal", "contenidoResultadoConsultaSAT");
            //Inicializando Indices
            Controles.InicializaIndices(gvComprobantes);
        }
    }
}