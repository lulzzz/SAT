using System;
using System.Collections.Generic; //Permite hacer las listas
using System.Data;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;
using SAT_CL.Global;

namespace SAT.Nomina
{
    /// <summary>
    /// CodeBehind de la forma Reporte de Nominas de Empleado
    /// </summary>
    public partial class ReporteNominaEmpleado : System.Web.UI.Page
    {
        #region Eventos
        /// <summary>
        /// Evento disparado al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validar que se produjo un postback
            if (!Page.IsPostBack)
            {
                //Invocar al método de inicializacion global
                inicializaPagina();
            }
        }
        /// <summary>
        /// Evento disparado al presionar el botón Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Invocar método de busqueda
            cargaNominaEmpleado();
        }
        /// <summary>
        /// Evento disparado al dar click en el botón btnExportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportar_Click(object sender, EventArgs e)
        {
            exportarNominasEmpleado();
        }
        /// <summary>
        /// Evento disparado al cambiar el estado del checkbox Fecha Nomina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFiltroNomina_CheckedChanged(object sender, EventArgs e)
        {
            habilitaFechas();
        }
        /// <summary>
        /// Evento disparado al cambiar el estado del checkbox FechaPago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkFiltroPago_CheckedChanged(object sender, EventArgs e)
        {
            habilitaFechas();
        }
        /// <summary>
        /// Evento disparado al cambiar el estado del checkbox TodosNominaEmpleado o NominaEmpleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodosNominaEmpleado_CheckedChanged(object sender, EventArgs e)
        {
            //Validar si el GridView contiene registros
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {
                //Evalua el checkbox que llama al evento
                switch (((CheckBox)sender).ID)
                {
                    //Si es el checkbox del encabezado
                    case "chkTodosNominaEmpleado":
                        {
                            //Crear un checkbox donde se asigna el control del encabezado
                            CheckBox check = (CheckBox)gvNominaEmpleados.HeaderRow.FindControl("chkTodosNominaEmpleado");
                            //Asignar el valor de "chkTodosNominaEmpleado" a todos los controles 
                            Controles.SeleccionaFilasTodas(gvNominaEmpleados, "chkVariosNominaEmpleado", check.Checked);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento disparado al elegir un elemento del dropdownlist Mostrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Verificar que existan registros en el GV
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {   //Se cambia el Tamaño de Pagina en base al GridView "Resultado" y al DataSet que contiene la Tabla del origen de Datos
                Controles.CambiaTamañoPaginaGridView(gvNominaEmpleados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoGrid.SelectedValue), true, 3);
            }
            //Suma Totales
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al cambiar el orden del gridview Nomina Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEmpleado_OnSorting(object sender, GridViewSortEventArgs e)
        {
            //Verificar que existan registros en el GridView
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {
                //Asignando al laber el Orden
                lblOrdenado.Text = Controles.CambiaSortExpressionGridView(gvNominaEmpleados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression, true, 3);
            }
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al cambiar de página en el gridview Nomina Empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvNominaEmpleado_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Verificar que existan registros en el Gridiew
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {
                //Cambiar índice de página
                Controles.CambiaIndicePaginaGridView(gvNominaEmpleados, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex, true, 3);
            }
            sumaTotales();
        }
        /// <summary>
        /// Evento disparado al presionar el enlace Exportar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            //Referenciando al botón pulsado
            using (LinkButton boton = (LinkButton)sender)
            {
                //Evaluando Boton Presionado
                switch (boton.CommandName)
                {
                    case "NominaEmpleado":
                        {
                            //Exporta el contenido de la tabla cargada en el gridview
                            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "");
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click al Link de Imprimir Nomina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkDescargarNomina_Click(object sender, EventArgs e)
        {
            //Revisar que el GridView contenga registros
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {
                //Seleccionar la fila
                Controles.SeleccionaFila(gvNominaEmpleados, sender, "lnk", false);
                
                //Instanciar a la nomina de empleado
                using (SAT_CL.Nomina.NomEmpleado NomEmp = new SAT_CL.Nomina.NomEmpleado(Convert.ToInt32(gvNominaEmpleados.SelectedDataKey["Id"])))
                {
                    //Obtener el control que activa el evento
                    LinkButton enlace = (LinkButton)sender;
                    //Botón presionado
                    switch (enlace.CommandName)
                    {
                        case "PDF":
                            {
                                //Obtener ruta
                                string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/FacturacionElectronica/ReporteComprobante.ascx", "~/RDLC/Reporte.aspx");
                                //Instanciar el tipo de comprobante
                                //Si la nomina de empleado usa comprobante en la version anterior
                                if (NomEmp.id_comprobante != 0)
                                {
                                    using (SAT_CL.FacturacionElectronica.Comprobante Comprobante = new SAT_CL.FacturacionElectronica.Comprobante(NomEmp.id_comprobante))
                                    {
                                        if (Comprobante.generado)
                                        {
                                            //Instanciar nueva ventana del navegador
                                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Comprobante", Comprobante.id_comprobante), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                        }
                                    }
                                }
                                //Si la nomina de empleado usa comprobante en la version 3.3
                                else if (NomEmp.id_comprobante33 != 0)
                                {
                                    using (SAT_CL.FacturacionElectronica33.Comprobante Comprobante33 = new SAT_CL.FacturacionElectronica33.Comprobante(NomEmp.id_comprobante33))
                                    {
                                        if (Comprobante33.bit_generado)
                                        {
                                            //Instanciar nueva ventana del navegador
                                            TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "ComprobanteV33", Comprobante33.id_comprobante33), "Comprobante", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptServer.MuestraNotificacion(this, "La nómina de empleado no tiene un comprobante.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                break;
                            }
                        case "XML":
                            {
                                //Si la nomina de empleado usa comprobante versión 3.3
                                if(NomEmp.id_comprobante33 > 0)
                                {
                                    descargaXML_comprobante33(NomEmp.id_comprobante33);
                                }
                                //Si la nómina de empleado usa comprobante en la antigua version
                                else if (NomEmp.id_comprobante > 0)
                                {
                                    descargaXML_comprobante(NomEmp.id_comprobante);
                                }
                                else
                                {
                                    ScriptServer.MuestraNotificacion(this, "La nómina de empleado no tiene un comprobante.", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                                
                                break;
                            }
                    }
                }
            }
        }        
        #endregion

        #region Métodos
        /// <summary>
        /// Método encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {
            //DDL Tamaño de GV Nomina
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGrid, "", 26);
            //DDL Estatus Nomina
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "--TODOS--", 3148);
        }
        /// <summary>
        /// Método encargado de llenar la nómina
        /// </summary>
        private void cargaNominaEmpleado()
        {
            //Declarar variables de fecha
            DateTime fec_pago_i, fec_pago_f, fec_nomi_i, fec_nomi_f;
            fec_pago_i = fec_pago_f = fec_nomi_i = fec_nomi_f = DateTime.MinValue;
            //Falidar fechas solicitadas
            if (chkFiltroPago.Checked)
            {
                //Obtener fechas
                DateTime.TryParse(txtFechaInicio.Text, out fec_pago_i);
                DateTime.TryParse(txtFechaFin.Text, out fec_pago_f);
            }
            if (chkFiltroNomina.Checked)
            {
                //Obtener fechas
                DateTime.TryParse(txtFechaInicio.Text, out fec_nomi_i);
                DateTime.TryParse(txtFechaFin.Text, out fec_nomi_f);
            }
            //Obtener reporte
            using (DataTable dtReporteNominaEmpleados = SAT_CL.Nomina.Reporte.ObtieneReporteNominaEmpleados(Convert.ToInt32(Cadena.RegresaCadenaSeparada(txtEmpleado.Text, "ID:", 1, "0")),Convert.ToInt32(txtNoNomina.Text == "" ? "0" : txtNoNomina.Text),fec_pago_i, fec_pago_f, fec_nomi_i, fec_nomi_f,Convert.ToByte(ddlEstatus.SelectedValue),((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
            {
                if (Validacion.ValidaOrigenDatos(dtReporteNominaEmpleados))
                {
                    //Cargar GV
                    Controles.CargaGridView(gvNominaEmpleados, dtReporteNominaEmpleados, "Id", lblOrdenado.Text, true, 1);
                    //Añadiendo resultado a sesion
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtReporteNominaEmpleados, "Table");
                }
                else
                {
                    //Inicializa GridView
                    Controles.InicializaGridview(gvNominaEmpleados);
                    //Eliminando resultado de session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
            }
            sumaTotales();
        }
        /// <summary>
        /// Método para descargar el XML si la nomina de empleado tiene Comprobante en la antigua versión
        /// </summary>
        /// <param name="idComprobante"></param>
        private void descargaXML_comprobante(int idComprobante)
        {
            //Instanciar registro en sesión
            using (SAT_CL.FacturacionElectronica.Comprobante Comprobante = new SAT_CL.FacturacionElectronica.Comprobante(idComprobante))
            {
                //Si existe y está generado
                if (Comprobante.generado && Comprobante.habilitar)
                {
                    //Obtener bytes del archivo XML
                    byte[] comprobante_nomina_XML = System.IO.File.ReadAllBytes(Comprobante.ruta_xml);
                    //Realizar descarga de archivo
                    if (comprobante_nomina_XML.Length > 0)
                    {
                        //Instanciar al emisor
                        using (CompaniaEmisorReceptor Emisor = new CompaniaEmisorReceptor(Comprobante.id_compania_emisor))
                        {
                            Archivo.DescargaArchivo(comprobante_nomina_XML, string.Format("{0}_{1}{2}.xml", Emisor.nombre_corto != "" ? Emisor.nombre_corto : Emisor.rfc, Comprobante.serie, Comprobante.folio), Archivo.ContentType.binary_octetStream);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Método para descargar el XML si la nomina de empleado tiene Comprobante versión 3.3
        /// </summary>
        /// <param name="idComprobante33"></param>
        private void descargaXML_comprobante33(int idComprobante33)
        {
            //Instanciar registro en sesión
            using (SAT_CL.FacturacionElectronica33.Comprobante Comprobante33 = new SAT_CL.FacturacionElectronica33.Comprobante(idComprobante33))
            {
                //Si existe y está generado
                if (Comprobante33.bit_generado && Comprobante33.habilitar)
                {
                    //Obtener bytes del archivo XML
                    byte[] comprobante_nomina_XML = System.IO.File.ReadAllBytes(Comprobante33.ruta_xml);
                    //Realizar descarga de archivo
                    if (comprobante_nomina_XML.Length > 0)
                    {
                        //Instanciar al emisor
                        using (CompaniaEmisorReceptor Emisor = new CompaniaEmisorReceptor(Comprobante33.id_compania_emisor))
                        {
                            Archivo.DescargaArchivo(comprobante_nomina_XML, string.Format("{0}_{1}{2}.xml", Emisor.nombre_corto != "" ? Emisor.nombre_corto : Emisor.rfc, Comprobante33.serie, Comprobante33.folio), Archivo.ContentType.binary_octetStream);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Exportar XML y PDF de la nomina de empleados
        /// </summary>
        private void exportarNominasEmpleado()
        {
            //Declarar lista de archivos
            List<KeyValuePair<string, byte[]>> archivos = new List<KeyValuePair<string, byte[]>>();
            //Declarar lista de errores
            List<string> errores = new List<string>();
            //Verificar que el gridview contenga registros
            if (gvNominaEmpleados.DataKeys.Count > 0)
            {
                //Obtener filas seleccionadas
                GridViewRow[] filasSeleccionadas = Controles.ObtenerFilasSeleccionadas(gvNominaEmpleados, "chkVariosNominaEmpleado");
                //Verificar que existan filas seleccionadas
                if (filasSeleccionadas.Length > 0)
                {
                    //Almacenar rutas en un arreglo
                    foreach (GridViewRow row in filasSeleccionadas)
                    {//Instanciar Nomina de Empleado del valor obtenido de la fila seleccionada
                        using (SAT_CL.Nomina.NomEmpleado NomEmp = new SAT_CL.Nomina.NomEmpleado(Convert.ToInt32(gvNominaEmpleados.DataKeys[row.RowIndex].Value)))
                        {
                            //Instanciar al comprobante que corresponde a la nómina
                            //Si la nomina de empleado usa comprobante en la version anterior
                            if (NomEmp.id_comprobante != 0)
                            {
                                using (SAT_CL.FacturacionElectronica.Comprobante Comprobante = new SAT_CL.FacturacionElectronica.Comprobante(NomEmp.id_comprobante))
                                {
                                    //Validar seleccion de PDF
                                    if (chkPDF.Checked == true)
                                    {
                                        //Añadir PDF
                                        using (CompaniaEmisorReceptor Emisor = new CompaniaEmisorReceptor(Comprobante.id_compania_emisor))
                                        {
                                            archivos.Add(new KeyValuePair<string, byte[]>(Emisor.nombre_corto != "" ? Comprobante.serie : Emisor.rfc + Comprobante.serie + Comprobante.folio.ToString() + ".pdf", NomEmp.GeneraPDFComprobanteNomina33()));
                                        }
                                    }
                                    //Validar seleccion de XML
                                    if (chkXML.Checked == true)
                                    {
                                        //Guardar archivo en arreglo de bytes
                                        byte[] archivoXML = System.IO.File.ReadAllBytes(Comprobante.ruta_xml);
                                        //Añadir XML a la lista
                                        using (CompaniaEmisorReceptor Emisor = new CompaniaEmisorReceptor(Comprobante.id_compania_emisor))
                                        {
                                            archivos.Add(new KeyValuePair<string, byte[]>(Emisor.nombre_corto != "" ? Emisor.nombre_corto : Emisor.rfc + Comprobante.serie + Comprobante.folio.ToString() + ".xml", archivoXML));
                                        }
                                    }
                                }
                            }
                            //Si la nomina de empleado usa comprobante en la version 3.3
                            else if (NomEmp.id_comprobante33 != 0)
                            {
                                using (SAT_CL.FacturacionElectronica33.Comprobante Comprobante33 = new SAT_CL.FacturacionElectronica33.Comprobante(NomEmp.id_comprobante33))
                                {
                                    //Validar seleccion de PDF
                                    if (chkPDF.Checked == true)
                                    {
                                        //Añadir PDF
                                        archivos.Add(new KeyValuePair<string, byte[]>(Comprobante33.serie + Comprobante33.folio.ToString() + ".pdf", NomEmp.GeneraPDFComprobanteNomina33()));
                                    }
                                    //Validar seleccion de XML
                                    if (chkXML.Checked == true)
                                    {
                                        //Guardar archivo en arreglo de bytes
                                        byte[] archivoXML = System.IO.File.ReadAllBytes(Comprobante33.ruta_xml);
                                        //Añadir XML a la lista
                                        archivos.Add(new KeyValuePair<string, byte[]>(Comprobante33.serie + Comprobante33.folio.ToString() + ".xml", archivoXML));
                                    }
                                }
                            }
                        }
                    }
                    //Generar archivo comprimido con las rutas
                    byte[] zip_file = Archivo.ConvirteArchivoZIP(archivos, out errores);
                    //Si al menos un archivo fue correcto; descarga.
                    if (zip_file != null)
                    {
                        Archivo.DescargaArchivo(zip_file, "NominasEmpleado.zip", Archivo.ContentType.binary_octetStream);
                    }
                    else
                    {
                        //Recorrer errores
                        foreach (string error in errores)
                        {
                            //Muestra mensaje de error
                            //lblError.Text += error + "<br>";
                        }
                    }
                }
                else //Mostrar mensaje
                {
                    //lblError.Text ="Debe seleccionar al menos un comprobante.";
                }
            }
        }
        /// <summary>
        /// Método encargado de habilitar los Textbox de fechas si al menos un checkbox está activo
        /// </summary>
        private void habilitaFechas()
        {
            if (chkFiltroNomina.Checked || chkFiltroPago.Checked)
            {
                //Habilitar
                txtFechaInicio.Enabled = true;
                txtFechaFin.Enabled = true;
            }
            else
            {
                txtFechaInicio.Enabled = false;
                txtFechaFin.Enabled = false;
                txtFechaInicio.Text = "";
                txtFechaFin.Text = "";
            }
        }
        /// <summary>
        /// Método encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Carga los catalogos
            cargaCatalogos();
            //Inicializa el gridview
            Controles.InicializaGridview(gvNominaEmpleados);
        }
        /// <summary>
        /// Método encargado de imprimir los totales de las columnas en el pie de cada una
        /// </summary>
        private void sumaTotales()
        {
            //TotalPagado
            //gvNominaEmpleados.FooterRow.Cells[20].Text = string.Format("{0:C2}", Convert.ToDecimal(((DataSet)Session["DS"]).Tables["Table"].Compute("SUM(TotalPagado)", "")));
            
            
            //TotalOtrosPagos
            //TotalDeducciones
            //OtrasDeducciones
            //Incapacidad
            //Infonavit
            //ISPT
            //IMSS
            //TotalPercepciones
            //OtrasPersepciones
            //Sueldo
            //Aguinaldo
        }
        #endregion
    }
}