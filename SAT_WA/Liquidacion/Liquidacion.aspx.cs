using SAT_CL.CXP;
using SAT_CL.Despacho;
using SAT_CL.EgresoServicio;
using SAT_CL.FacturacionElectronica;
using SAT_CL.Global;
using SAT_CL.Liquidacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Transactions;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Liquidacion
{
    public partial class Liquidacion : System.Web.UI.Page
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
            //Validando que se produjo un PostBack
            if(!(Page.IsPostBack))
               
                //Invocando Método de Inicialización de Página
                inicializaPagina();

            //Invocando Método de Carga
            cargaAutocompletaEntidad();
        }
        /// <summary>
        /// Método Público Web encargado de Obtener los Archivos del Lado del Cliente
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void ArchivoSesion(object context, string file_name)
        {   
            //Obteniendo Archivo Codificado en UTF8
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
        /// Evento generado al cambiar la Vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVista_OnClick(object sender, EventArgs e)
        {
            //Determinando el tipo de acción a realizar
            switch (((Button)sender).CommandName)
            {
                case "Busqueda":
                    mtvEncabezado.ActiveViewIndex = 0;
                    btnBusqueda.CssClass = "boton_pestana_activo";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnResumen.CssClass = "boton_pestana";
                    //Invoca al método que permite limpiar los controles de la pestaña busqueda
                    limpiaBusquedaLiquidacion();
                    break;
                case "Liquidacion":
                    mtvEncabezado.ActiveViewIndex = 1;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana_activo";
                    btnResumen.CssClass = "boton_pestana";
                    break;
                case "Resumen":
                    mtvEncabezado.ActiveViewIndex = 2;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana";
                    btnResumen.CssClass = "boton_pestana_activo";

                    //Cargando Resumen
                    cargaResumenTotalLiquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text));

                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Exportar Resumen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarResumenLiq_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table11"), "Id", "IdTipoRegistro");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Imprimir Liquidación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimirLiquidacion_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Ruta
                    string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Liquidacion/Liquidacion.aspx", "~/RDLC/Reporte.aspx");

                    //Instanciando nueva ventana de navegador para apertura de registro
                    TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "Liquidacion", liq.id_liquidacion), "Liquidación", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                }
                else
                    //Mostrando Excepción
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No Existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResumenLiquidacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando el Tipo de Registro
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Tipo de Registro
                string tipo_registro = ((DataRowView)e.Row.DataItem).Row.ItemArray[1].ToString();

                //Validando el Tipo de Registro
                switch (tipo_registro)
                {
                    //Servicio
                    case "1":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_servicio";
                            break;
                        }
                    //Movimiento
                    case "2":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_movimiento";
                            break;
                        }
                    //Detalle Liquidación
                    case "3":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            break;
                        }
                    //Detalle Liquidación con Valor Negativo
                    case "4":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;

                            break;
                        }
                    //Totales Por Servicio
                    case "5":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[6].CssClass = 
                            e.Row.Cells[7].CssClass = 
                            e.Row.Cells[8].CssClass = 
                            e.Row.Cells[9].CssClass =
                            e.Row.Cells[10].CssClass = "liquidacion_totales_liquidacion";

                            //Validando que existan Depositos
                            if (e.Row.Cells[6].Text.Equals("Total Deposito"))
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                            else
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;

                            break;
                        }
                    //Cobros Recurrentes
                    case "6":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_cobro_recurrente";
                            break;
                        }
                    //Totales de la Liquidación
                    case "7":
                        {
                            //Asignando Estilo
                            e.Row.CssClass = "liquidacion_detalle";
                            e.Row.Cells[6].CssClass = 
                            e.Row.Cells[7].CssClass = 
                            e.Row.Cells[8].CssClass = 
                            e.Row.Cells[9].CssClass =
                            e.Row.Cells[10].CssClass = "liquidacion_totales_liquidacion";

                            //Validando que existan Depositos/Deducciones/Descuentos
                            if (e.Row.Cells[6].Text.Equals("Total Deposito") || e.Row.Cells[6].Text.Equals("Total Deducci&#243;n") || e.Row.Cells[6].Text.Equals("Total Descuento"))
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                            else
                                //Asignando Color de Texto
                                e.Row.Cells[10].ForeColor = System.Drawing.Color.Black;
                            break;
                        }
                }
            }
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
                                var tipoEntidad = " + ddlTipoEnt.SelectedValue + @";
                            
                                //Evento Change
                                $('#ctl00_content1_ddlTipoEnt').change(function () {
                                    
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
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=28&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 2:
                                            {   
                                                //Cargando Catalogo de Operadores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=27&param=" + id_compania + @"'});
                                                break;
                                            }
                                        case 3:
                                            {   
                                                //Cargando Catalogo de Proveedores
                                                $('#" + txtEntidad.ClientID + @"').autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=45&param=" + id_compania + @"'});
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
        /// Evento Producido al presionar el Boton "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {   
            //Instanciando Liquidaciones
            using (DataTable dtLiquidaciones = SAT_CL.Liquidacion.Liquidacion.ObtieneLiquidacionesEntidad(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)Convert.ToInt32(ddlTipoEnt.SelectedValue),
                                                Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1))))
            {  
                //Validando si Existen registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtLiquidaciones))
                {   
                    //Cargando Liquidaciones
                    TSDK.ASP.Controles.CargaGridView(gvLiquidacion, dtLiquidaciones, "Id", "");
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtLiquidaciones, "Table");
                }
                else
                {   
                    //Cargando Liquidaciones
                    TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
                    //Añadiendo Tabla al DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table");
                }
                
                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
                //Limpiando Mensaje
                lblErrorBusqueda.Text = "";
            }
        }
        /// <summary>
        /// Evento Producido al presionar el Boton "Crear"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrear_Click(object sender, EventArgs e)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando Variables Auxiliares
            int id_unidad = ddlTipoEnt.SelectedValue == "1" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            int id_operador = ddlTipoEnt.SelectedValue == "2" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            int id_proveedor = ddlTipoEnt.SelectedValue == "3" ? Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtEntidad.Text, "ID:", 1)) : 0;
            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
            DateTime fecha_ultimo_viaje = DateTime.MinValue;
            
            //Validando que exista el Operador
            if (id_operador != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_operador, Convert.ToInt32(ddlTipoEnt.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);
            //Validando que exista el Proveedor
            else if (id_proveedor != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_proveedor, Convert.ToInt32(ddlTipoEnt.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);
            //Validando que exista el Unidad
            else if (id_unidad != 0)
                //Obteniendo Dias Pagados
                dias_pagados = ObtieneDiasPagados(id_unidad, Convert.ToInt32(ddlTipoEnt.SelectedValue), TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), out fecha_ultimo_viaje);
            
            //Validando que exista una Fecha Valida
            if (fecha_ultimo_viaje != DateTime.MinValue)
            {   
                //Insertando Encabezado de Liquidación            
                result = SAT_CL.Liquidacion.Liquidacion.InsertaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 0,
                            (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)Convert.ToByte(ddlTipoEnt.SelectedValue), id_operador, id_unidad, id_proveedor,
                            TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro(), 0, 0, 0, 0, 0, 0, 0, dias_pagados.Days, false, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                
                //validando que la Operación fuese exitosa
                if (result.OperacionExitosa)
                {   
                    //Habilitando Controles
                    habilitaControlesEncabezado(Pagina.Estatus.Lectura);
                    //Inicializando Valores
                    inicializaValoresLiquidacion(result.IdRegistro);
                    //Quitando Selección de Liquidacion
                    TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);

                    //Instanciando Liquidación
                    using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(result.IdRegistro))

                        //Actualizando Carga de Totales de la Liquidación
                        cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                }
            }
            else
                //Instanciando Excepción
                result = new RetornoOperacion("No existen Viajes para esta Entidad");
            
            //Mostrando Mensaje de Operación
            lblErrorBusqueda.Text = result.Mensaje;
        }
        /// <summary>
        /// Evento Producido al presionar el Boton "Guardar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Validando que exista la Liquidación
            if (Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text) != 0)
            {   
                //Declarando Variable Auxiliar
                DateTime fecha_liquidacion;
                //Obteniendo Fecha
                DateTime.TryParse(txtFecha.Text, out fecha_liquidacion);
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {   
                    //Validando que exista la Liquidación
                    if (liq.id_liquidacion != 0)
                    {   
                        //Validando Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Declarando Objetos Auxiliares 
                            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
                            DateTime fecha_ultimo_viaje = DateTime.MinValue;
                            
                            //Validando que exista el Operador
                            if (liq.id_operador != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_operador, Convert.ToInt32(ddlTipoEnt.SelectedValue), fecha_liquidacion, out fecha_ultimo_viaje);
                            //Validando que exista el Proveedor
                            else if (liq.id_proveedor != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_proveedor, Convert.ToInt32(ddlTipoEnt.SelectedValue), fecha_liquidacion, out fecha_ultimo_viaje);
                            //Validando que exista el Unidad
                            else if (liq.id_unidad != 0)
                                //Obteniendo Dias Pagados
                                dias_pagados = ObtieneDiasPagados(liq.id_unidad, Convert.ToInt32(ddlTipoEnt.SelectedValue), fecha_liquidacion, out fecha_ultimo_viaje);
                            
                            //Validando que la Fecha Ingresada sea mayor que la Fecha de la Liquidación
                            if (fecha_liquidacion.CompareTo(liq.fecha_liquidacion) > 0 || fecha_liquidacion.CompareTo(liq.fecha_liquidacion) == 0)
                            
                                //Editando Liquidación
                                result = liq.EditaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, liq.no_liquidacion, liq.estatus,
                                            liq.tipo_asignacion, liq.id_operador, liq.id_unidad, liq.id_proveedor, fecha_liquidacion, liq.total_salario,
                                            liq.total_deducciones, liq.total_sueldo, liq.total_anticipos, liq.total_comprobaciones, liq.total_descuentos, liq.total_alcance,
                                            dias_pagados.Days, liq.bit_transferencia, liq.id_transferencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            
                            //Si la Fecha Ingresada es menor a la Fecha de la Liquidación y Existen Pagos
                            else if (fecha_liquidacion.CompareTo(liq.fecha_liquidacion) < 0)
                            {
                                //Validando que existan Pagos
                                if (liq.ValidaPagosLiquidacion())

                                    //Instanciando Excepción
                                    result = new RetornoOperacion("La Fecha no puede ser menor a la Fecha actual de la Liquidación ya que Existen Pagos");
                                else
                                    //Editando Liquidación
                                    result = liq.EditaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, liq.no_liquidacion, liq.estatus,
                                                liq.tipo_asignacion, liq.id_operador, liq.id_unidad, liq.id_proveedor, fecha_liquidacion, liq.total_salario,
                                                liq.total_deducciones, liq.total_sueldo, liq.total_anticipos, liq.total_comprobaciones, liq.total_descuentos, liq.total_alcance,
                                                dias_pagados.Days, liq.bit_transferencia, liq.id_transferencia, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }

                            //Validando que la Operacion fue Correcta
                            if (result.OperacionExitosa)
                            {
                                //Habilitación de Controles
                                habilitaControlesEncabezado(Pagina.Estatus.Lectura);

                                //Actualizando Datos del Encabezado
                                inicializaValoresLiquidacion(Convert.ToInt32(lblId.Text));

                                //Actualizando Carga de Totales de la Liquidación
                                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                            }
                        }
                        else
                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
            else
                //Instanciando Error
                result = new RetornoOperacion("No existe la Liquidación");
            //Mostrando mensaje Obtenido
            lblErrorLiquidacion.Text = result.Mensaje;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditar_Click(object sender, EventArgs e)
        {   
            //Validando que Exista una Liquidación
            if(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text) != 0)
                
                //Habilitación de Controles
                habilitaControlesEncabezado(Pagina.Estatus.Edicion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {   
            //Habilitación de Controles
            habilitaControlesEncabezado(Pagina.Estatus.Lectura);
            //Inicializando Valores
            inicializaValoresLiquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text));
            //Limpiando Control
            lblErrorLiquidacion.Text = "";
        }
        /// <summary>
        /// Eventos Producido al Presionar el Boton "Guardar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarPago_Click(object sender, EventArgs e)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            
            //Declarando Objeto de Contención de Mensajes
            string[] resultados = new string[1];
            
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {   
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Validando Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Obteniendo Entidad
                        int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                        //Obteniendo Control
                        Button btn = (Button)sender;
                        
                        //Validando Comando
                        switch(btn.CommandName)
                        {
                            case "EditaPago":
                                {
                                    //Si el Pago fue Seleccionado
                                    if (gvPagos.SelectedIndex != -1)
                                    {
                                        //Instanciando Pago
                                        using (SAT_CL.Liquidacion.Pago pago = new Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                                        {
                                            //Validando que Exista el Pago
                                            if (pago.id_pago > 0)
                                            {
                                                //Editando Pago
                                                result = pago.EditaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), pago.id_tarifa, txtDescripcion.Text, txtReferencia.Text,
                                                                                liq.id_unidad, liq.id_operador, liq.id_proveedor, pago.objDetallePago.id_servicio,
                                                                                pago.objDetallePago.id_movimiento, liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                                Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación fuese Exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Validando que exista un Viaje Seleccionado
                                                    if (gvViajes.SelectedIndex != -1)

                                                        //Cargando Movimientos y Pagos
                                                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                                                    else
                                                    {
                                                        //Invocando Método de Carga de Pagos generales
                                                        cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                                        //Inicializando Indices
                                                        TSDK.ASP.Controles.InicializaIndices(gvViajes);
                                                        TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                                                        upgvViajes.Update();
                                                        upgvMovimientos.Update();
                                                    }

                                                    //Inicializando Indices
                                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);
                                                }
                                            }
                                            else
                                                //Instanciando Excepción
                                                result = new RetornoOperacion("No Existe el Pago");
                                        }
                                    }
                                    break;
                                }
                            case "CreaPago":
                                {
                                    //Validando que exista una seleccion
                                    if (gvMovimientos.SelectedIndex != -1)
                                    {
                                        //Validando que el Movimiento este Validado
                                        if (validaMovimientoTerminado(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                                        {
                                            //Validando que existe un Pago
                                            if (Convert.ToInt32(gvMovimientos.SelectedDataKey["NoPago"]) != 0)
                                            {
                                                //Insertando Pago
                                                result = new RetornoOperacion("Ya Existe un Pago");
                                            }
                                            else
                                            {
                                                //Inicializando Bloque Transaccional
                                                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                                {
                                                    //Insertando Pago
                                                    result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                        liq.id_unidad, liq.id_operador, liq.id_proveedor, Convert.ToInt32(gvViajes.SelectedDataKey["Id"]),
                                                                        Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, Convert.ToDecimal(txtCantidad.Text), 0,
                                                                        Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                    //Validando que la Operación haya sido Exitosa
                                                    if (result.OperacionExitosa)
                                                    {
                                                        //Insertando Pago del Movimiento
                                                        result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(result.IdRegistro, Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]),
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que la Operación haya sido Exitosa
                                                        if (result.OperacionExitosa)
                                                        {
                                                            //Actualizando Depositos
                                                            result = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                            //Validando que la Operación haya sido Exitosa
                                                            if (result.OperacionExitosa)

                                                                //Completando Transacción
                                                                trans.Complete();
                                                        }
                                                    }
                                                }
                                            }
                                            //Validando que la Operación haya sido exitosa
                                            if (result.OperacionExitosa)
                                            
                                                //Cargando Movimientos y Pagos por Viaje
                                                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                                        }
                                        else
                                            //Instanciando Excepción
                                            result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");
                                    }
                                    else
                                    {
                                        //Inicializando Bloque Transaccional
                                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                                        {
                                            //Obteniendo Filas Seleccionadas
                                            GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");

                                            //Validando que existan
                                            if (gvFilas.Length > 0)
                                            {
                                                //Creando Longitud Dinamica
                                                resultados = new string[gvFilas.Length];
                                                //Declarando Contador
                                                int contador = 0;
                                                //Insertando Pago
                                                result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                                    liq.id_unidad, liq.id_operador, liq.id_proveedor, Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), 0, liq.id_liquidacion,
                                                                    Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación fuese exitosa
                                                if (result.OperacionExitosa)
                                                {
                                                    //Obteniendo Id de Pago
                                                    int idPago = result.IdRegistro;

                                                    //Iniciando Recorrido de Movimientos
                                                    foreach (GridViewRow movimiento in gvFilas)
                                                    {
                                                        //Declarando Objeto para Actualizar Depositos y Vales de Diesel
                                                        RetornoOperacion res1 = new RetornoOperacion();

                                                        //Seleccionando Indice Actual
                                                        gvMovimientos.SelectedIndex = movimiento.RowIndex;

                                                        //Validando que el Movimiento este Validado
                                                        if (validaMovimientoTerminado(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                                                        {
                                                            //Insertando Pago del Movimiento
                                                            result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(idPago, Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]),
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operación sea Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualizando Depositos
                                                                res1 = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, true);

                                                                //Validando que se hayan hecho las Actualizaciones
                                                                if (!res1.OperacionExitosa)
                                                                {
                                                                    //Asignando Valores
                                                                    resultados = new string[1];
                                                                    resultados[0] = "Error: Mov. " + gvMovimientos.SelectedDataKey["Id"].ToString() + " " + res1.Mensaje;

                                                                    //Terminando Ciclo
                                                                    break;
                                                                }

                                                                //Guardando Mensaje de Operación
                                                                resultados[contador] = "Mov. " + gvMovimientos.SelectedDataKey["Id"].ToString() + " " + result.Mensaje;
                                                            }

                                                            //Incrementando Contador
                                                            contador++;
                                                        }
                                                        else
                                                        {
                                                            //Instanciando Excepción
                                                            result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");

                                                            //Terminando Ciclo
                                                            break;
                                                        }
                                                    }

                                                    //Validando que la Operación fuese exitosa
                                                    if (result.OperacionExitosa)

                                                        //Completando Transaccion
                                                        trans.Complete();

                                                    //Mostrando Mensaje(s)
                                                    lblErrorPago.Text = String.Join("\n", resultados);
                                                }
                                            }
                                        }

                                        //Cargando Movimientos y Pagos
                                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                                    }

                                    //Mostrando Mensaje
                                    lblErrorPago.Text = result.Mensaje;

                                    break;
                                }
                            case "CreaOtrosPagos":
                                {
                                    //Insertando Pago
                                    result = SAT_CL.Liquidacion.Pago.InsertaPago(Convert.ToInt32(ddlTipoPago.SelectedValue), 0, txtDescripcion.Text, txtReferencia.Text,
                                                        liq.id_unidad, liq.id_operador, liq.id_proveedor, 0, 0, liq.id_liquidacion,
                                                        Convert.ToDecimal(txtCantidad.Text), 0, Convert.ToDecimal(txtValorU.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando Operación Exitosa
                                    if (result.OperacionExitosa)
                                    {
                                        //Invocando Método de Carga
                                        cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                                        //Inicializando Indices
                                        TSDK.ASP.Controles.InicializaIndices(gvViajes);
                                        TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                                        upgvViajes.Update();
                                        upgvMovimientos.Update();
                                    }
                                    break;
                                }
                        }

                        //Actualizando Carga de Totales de la Liquidación
                        cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);

                        //Ocultando ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnGuardarPago, upbtnGuardarPago.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");

                        //Mostrando Resultado de la Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarPago_Click(object sender, EventArgs e)
        {   
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);
            TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
            
            //Limpiando Controles
            limpiaControlesPagos();
            habilitaControlesPagos(false);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarPago, upbtnCancelarPago.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Crear Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearPago_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validando que Exista
                if (liq.id_liquidacion > 0)
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvFilas = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                        
                        //Validando que Existan Filas
                        if (gvFilas.Length > 0)
                        {
                            //Actualizando Comando
                            btnGuardarPago.CommandName = "CreaPago";
                            upbtnGuardarPago.Update();

                            //Deshabilitando Control
                            txtDescripcion.Enabled = false;
                            uptxtDescripcion.Update();

                            //Alternando Ventana Modal
                            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCrearPago, upbtnCrearPago.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
                        }
                        else
                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPago, "No hay Filas Seleccionadas", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPago, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearPago, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Crear Otros Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearOtrosPagos_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validando que Exista
                if (liq.id_liquidacion > 0)
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Actualizando Comando
                        btnGuardarPago.CommandName = "CreaOtrosPagos";
                        upbtnGuardarPago.Update();

                        //Configurando Controles
                        habilitaControlesPagos(true);
                        limpiaControlesPagos();

                        //Deshabilitando Control
                        txtDescripcion.Enabled = true;
                        uptxtDescripcion.Update();

                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }

                        //Alternando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnCrearOtrosPagos, upbtnCrearOtrosPagos.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearOtrosPagos, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
                else
                    //Mostrando Mensaje de Operación
                    TSDK.ASP.ScriptServer.MuestraNotificacion(upbtnCrearOtrosPagos, "No existe la Liquidación", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Contenido del Control "Cantidad"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCantidad_TextChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal cantidad = 0.00M, valoru = 0.00M;;

            //Obteniendo Valores
            if (Decimal.TryParse(txtCantidad.Text, out cantidad) && Decimal.TryParse(txtValorU.Text, out valoru))
            {
                //Asignando Valor Calculado
                txtCantidad.Text = string.Format("{0:0.00}", cantidad);
                txtTotal.Text = string.Format("{0:0.00}", cantidad * valoru);
            }
            else
            {
                //Asignando Valor por Defecto
                txtTotal.Text =
                txtCantidad.Text = "0.00";
            }
            
            //Asignando Enfoque al Control
            txtValorU.Focus();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Contenido del Control "Valor Unitario"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtValorU_TextChanged(object sender, EventArgs e)
        {
            //Declarando Variables Auxiliares
            decimal cantidad = 0.00M, valoru = 0.00M; ;

            //Obteniendo Valores
            if (Decimal.TryParse(txtCantidad.Text, out cantidad) && Decimal.TryParse(txtValorU.Text, out valoru))
            {
                //Asignando Valor Calculado
                txtValorU.Text = string.Format("{0:0.00}", valoru);
                txtTotal.Text = string.Format("{0:0.00}", cantidad * valoru);
            }
            else
            {
                //Asignando Valor por Defecto
                txtTotal.Text =
                txtValorU.Text = "0.00";
            }

            //Asignando Enfoque al Control
            txtReferencia.Focus();
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Cerrar Imagen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrar_Click(object sender, EventArgs e)
        {
            //Deseleccionando Todas las Filas
            TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
            
            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrar, uplnkCerrar.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Cerrar Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaCRP_Click(object sender, EventArgs e)
        {
            //Alternando Ventana Modal 
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarVentanaCRP, uplnkCerrarVentanaCRP.GetType(), "VerCobrosPendientes", "contenedorVentanaCobrosPendientes", "ventanaCobrosPendientes");

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvCobroRecurrenteLiquidacion);
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Cerrar Historial"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarVentanaCRH_Click(object sender, EventArgs e)
        {
            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarVentanaCRH, uplnkCerrarVentanaCRH.GetType(), "VerCobrosHistorial", "contenedorVentanaHistorialCobrosRecurrentes", "ventanaHistorialCobrosRecurrentes");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Crear Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearComp_Click(object sender, EventArgs e)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validando que Exista
                if (liq.id_liquidacion > 0)
                {
                    //Validando el Estatus de la Liquidación
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                    {
                        //Validando que este seleccionado un Viaje
                        if (gvViajes.SelectedIndex != -1)
                        {
                            //Validando que este seleccionado un Viaje
                            if (gvMovimientos.SelectedIndex != -1)
                            {
                                //Inicializando Controles
                                inicializaControlComprobaciones(0, 0);

                                //Declarando Script de Ventana Modal
                                string script = @"<script type='text/javascript'>
                                        //Mostrando ventana modal 
                                        $('#contenedorVentanaComprobaciones').animate({ width: 'toggle' });
                                        $('#ventanaComprobaciones').animate({ width: 'toggle' });
                                    </script>";

                                //Registrando Script
                                ScriptManager.RegisterStartupScript(upbtnCrearComp, upbtnCrearComp.GetType(), "VentanaComprobacion", script, false);
                            }
                        }
                    }
                    else
                        //Mostrando Mensaje de Operación
                        TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarComprobacion_Click(object sender, EventArgs e)
        {
            //Inicializando Controles
            inicializaControlComprobaciones(0, 0);

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                //Cargando resultados Actualizados
                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
            }
            
            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarComprobacion, upbtnCancelarComprobacion.GetType(), "VentanaComprobacion", "contenedorVentanaComprobaciones", "ventanaComprobaciones");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Guardar Comprobación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardarComprobacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaComprobacion();

            //Instanciando Liquidación
            using(SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text)))
                
                //Actualizando Carga de Totales de la Liquidación
                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Agregar Factura"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAgregarFactura_Click(object sender, EventArgs e)
        {
            //Guardando Factura
            guardaFacturaXML();

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))

                //Actualizando Carga de Totales de la Liquidación
                cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cerrar Liquidación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarLiquidacion_Click(object sender, EventArgs e)
        {
            //Invocando Método de Cerrar
            cierraLiquidacion();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Aceptar Operación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarOperacion_Click(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Inicializando Transacción
                    using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                    {
                        //Instanciando Pago
                        using (SAT_CL.Liquidacion.Pago pay = new SAT_CL.Liquidacion.Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                        {
                            //Validando que existe 
                            if (pay.id_pago != 0)
                            {
                                //Deshabilitando Pago
                                result = pay.DeshabilitaPago(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            }
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("No existe el Pago.");

                            //Validando que la Operación fuese exitosa
                            if (result.OperacionExitosa)
                            {
                                //Deshabilitando Detalle de Liquidación
                                result = pay.objDetallePago.DeshabilitaDetalleLiquidacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que el Detalle se haya deshabilitado
                                if (result.OperacionExitosa)
                                {
                                    //Validando si existe un solo Movimiento
                                    if (pay.objDetallePago.id_movimiento > 0)
                                    {
                                        //Instanciando Pago del Movimiento
                                        using (SAT_CL.Liquidacion.PagoMovimiento pm = new SAT_CL.Liquidacion.PagoMovimiento(pay.id_pago, pay.objDetallePago.id_movimiento))
                                        {
                                            //Validando que exista el Pago del Movimiento
                                            if (pm.id_pago_movimiento > 0)
                                            {
                                                //Deshabilitando Pago del Movimiento
                                                result = pm.DeshabilitaPagoMovimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación hay sido exitosa
                                                if (result.OperacionExitosa)
                                                
                                                    //Actualziando la Referencia para quitar la Liquidación 
                                                    result = actualizaLiquidacionDepositosVales(pay.objDetallePago.id_movimiento, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Obteniendo Movimientos
                                        using (DataTable dtPagoMovimientos = SAT_CL.Liquidacion.PagoMovimiento.ObtienePagosMovimiento(pay.id_pago))
                                        {
                                            //Validando que existan Pagos a Movimientos
                                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagoMovimientos))
                                            {
                                                //Iniciando Recorrido de Movimientos
                                                foreach (DataRow dr in dtPagoMovimientos.Rows)
                                                {
                                                    //Instanciando Pago del Movimiento
                                                    using (SAT_CL.Liquidacion.PagoMovimiento pm = new SAT_CL.Liquidacion.PagoMovimiento(Convert.ToInt32(dr["Id"])))
                                                    {
                                                        //Validando que exista el Pago del Movimiento
                                                        if (pm.id_pago_movimiento > 0)
                                                        {
                                                            //Deshabilitando Pago del Movimiento
                                                            result = pm.DeshabilitaPagoMovimiento(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                            //Validando que la Operacion Actual haya sido Exitosa
                                                            if (result.OperacionExitosa)
                                                            {
                                                                //Actualziando la Referencia para quitar la Liquidación 
                                                                result = actualizaLiquidacionDepositosVales(pm.id_movimiento, liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, false);

                                                                //Si la Operación Falló
                                                                if (!result.OperacionExitosa)
                                                                    break;
                                                            }
                                                            else
                                                                //Rompiendo Ciclo
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Validando que la Operacion Actual haya sido Exitosa
                                    if (result.OperacionExitosa)
                                        
                                        //Completando Transacción
                                        trans.Complete();
                                }
                            }
                        }
                    }
                    //Validando que las Operaciones hayan sido exitosas
                    if (result.OperacionExitosa)
                    {
                        //Validando que exista un Viaje Seleccionado
                        if (gvViajes.SelectedIndex != -1)

                            //Cargando Movimientos y Pagos
                            cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                        else
                        {
                            //Invocando Método de Carga de Pagos generales
                            cargaPagosGeneralLiquidacion(liq.id_liquidacion);

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvViajes);
                            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                            upgvViajes.Update();
                            upgvMovimientos.Update();
                        }
                        
                        //Inicializando Indices
                        TSDK.ASP.Controles.InicializaIndices(gvPagos);

                        //Alternando Ventana Modal
                        TSDK.ASP.ScriptServer.AlternarVentana(upbtnAceptarOperacion, upbtnAceptarOperacion.GetType(), "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                    }
                }
            }

            //Mostrando Mensaje de Operación
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);

        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar Operación"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarOperacion_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnCancelarOperacion, upbtnCancelarOperacion.GetType(), "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Ver Pendientes"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVerCobrosPendientes_Click(object sender, EventArgs e)
        {
            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(upbtnVerCobrosPendientes, upbtnVerCobrosPendientes.GetType(), "VerCobrosPendientes", "contenedorVentanaCobrosPendientes", "ventanaCobrosPendientes");
        }
        
        #region Eventos GridView "Liquidaciones"

        /// <summary>
        /// Evento Producido al Cambiar la Expresión de Ordenamiento del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Expresion de Ordenamiento
            lblOrdenado.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.SortExpression);
            
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), e.NewPageIndex);
            
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvLiquidacion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportar_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), "Id");
        }
        
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Liquidaciones"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamano_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table"), Convert.ToInt32(ddlTamanoViajes.SelectedValue));
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {   
            //Validando que existen Registros
            if (gvLiquidacion.DataKeys.Count > 0)
            {   
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvLiquidacion, sender, "lnk", false);
                //habilitando Controles
                habilitaControlesEncabezado(Pagina.Estatus.Lectura);
                //Cargando datos de la Liquidación
                inicializaValoresLiquidacion(Convert.ToInt32(gvLiquidacion.SelectedDataKey["Id"]));
            }
        }

        #endregion

        #region Eventos GridView "Viajes"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_Sorting(object sender, GridViewSortEventArgs e)
        {   
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoViajes.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvViajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.SortExpression);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvViajes);
            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvPagos);
            //Actualiza Controles de Pagos
            limpiaControlesPagos();
            habilitaControlesPagos(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvViajes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvViajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), e.NewPageIndex);
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvViajes);
            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvPagos);
            //Actualiza Controles de Pagos
            limpiaControlesPagos();
            habilitaControlesPagos(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoViajes_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvViajes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), Convert.ToInt32(ddlTamanoViajes.SelectedValue));
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvViajes);
            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvPagos);
            //Actualiza Controles de Pagos
            limpiaControlesPagos();
            habilitaControlesPagos(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarViajes_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table1"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkMovimientos_Click(object sender, EventArgs e)
        {   
            //Validando que existen Registros
            if (gvViajes.DataKeys.Count > 0)
            {   
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvViajes, sender, "lnk", false);
                
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {   
                    //Validando que exista la Liquidación
                    if (liq.id_liquidacion > 0)
                    {
                        //Obteniendo Entidad
                        int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;
                    
                        //Cargando Movimientos
                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad, liq.id_tipo_asignacion, liq.id_estatus);
                    }
                }
            }
        }
        /// <summary>
        /// Maneja el evento click en el botón PAGAR del Gridview de Servicios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbPagarServicio_Click(object sender, EventArgs e)
        {
            //Validando que existan registros en el gridview
            if (gvViajes.DataKeys.Count > 0)
            {
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Seleccionando fila
                            TSDK.ASP.Controles.SeleccionaFila(gvViajes, sender, "lnk", false);
                            
                            //Realizando búsqueda de tarifa y en caso de ser tarifa única se aplica el pago
                            buscaTarifaPagoServicio();
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvViajes);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Referencias de Viaje"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkReferenciasServicio_Click(object sender, EventArgs e)
        {
            //Validando que existan registros en el gridview
            if (gvViajes.DataKeys.Count > 0)
            {
                //Seleccionando fila
                TSDK.ASP.Controles.SeleccionaFila(gvViajes, sender, "lnk", false);

                //Instanciando Factura
                using (SAT_CL.Documentacion.Servicio serv = new SAT_CL.Documentacion.Servicio(Convert.ToInt32(gvViajes.SelectedDataKey["Id"])))
                {
                    //Validando que Exista la Factura
                    if (serv.id_servicio > 0)
                    {
                        //Inicializando Control
                        ucReferenciasViaje.InicializaControl(serv.id_servicio);

                        //Mostrando Ventana
                        TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
                    }
                }
            }
        }

        #region Eventos Referencias Viaje

        /// <summary>
        /// Evento Producido al Guardar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickGuardarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.GuardaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                inicializaValoresLiquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text));

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// /// Evento Producido al Eliminar las Referencias del Viaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucReferenciasViaje_ClickEliminarReferenciaViaje(object sender, EventArgs e)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Eliminando Concepto
            result = ucReferenciasViaje.EliminaReferenciaViaje();

            //Validando que la Operación fuese Exitosa
            if (result.OperacionExitosa)

                //Inicializando Valores
                inicializaValoresLiquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text));

            //Mostrando Resultado
            TSDK.ASP.ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Evento encargado de Cerrar la Ventana Modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarReferencias_Click(object sender, EventArgs e)
        {
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvViajes);

            //Mostrando Ventana
            TSDK.ASP.ScriptServer.AlternarVentana(this, "ReferenciasViaje", "contenedorVentanaReferenciasViaje", "ventanaReferenciasViaje");
        }

        #endregion

        #endregion

        #region Eventos GridView "Movimientos"

        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoMov_SelectedIndexChanged(object sender, EventArgs e)
        {   
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoMov.SelectedValue));
            //Deshabilitando Controles de Captura de Pagos
            habilitaControlesPagos(false);
            limpiaControlesPagos();
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Eventos Producido al dar Click en el Boton "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarMov_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), "Id");
        }
        /// <summary>
        /// Eventos Producido al Cambiar el Ordenamiento de "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_Sorting(object sender, GridViewSortEventArgs e)
        {   //Cambiando Expresion de Ordenamiento
            lblOrdenadoMov.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);
            //Deshabilitando Controles de Captura de Pagos
            habilitaControlesPagos(false);
            limpiaControlesPagos();
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página de "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {   
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvMovimientos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);
            //Deshabilitando Controles de Captura de Pagos
            habilitaControlesPagos(false);
            limpiaControlesPagos();
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Evento Producido al Seleccionar un Marcador del GridView "Movimientos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {   
            //Obteniendo Control Disparador
            CheckBox chk = (CheckBox)sender;
            
            //Obteniendo Tipos de Pago
            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                //Validando que existan los Conceptos de Pago
                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                    //Cargando DropDownList
                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                else
                    //Inicializando DropDownList
                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
            }
            
            //Validando el Control
            switch(chk.ID)
            {
                case "chkTodos":
                    {   
                        //Limpiando Controles de Pagos
                        limpiaControlesPagos();
                        //Validando que la fila sea seleccionada
                        if (!chk.Checked)
                        {   
                            //Seleccionando Todas las Filas
                            TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", chk.Checked);
                            //Visualizando Control
                            btnCrearPago.Visible = btnCrearPago.Enabled = false;
                        }
                        else
                        {
                            //Invocando Método de Configuración
                            configuraVentanaPagos("VariosMovimientos");
                        }
                        //Habilitando Controles según el Valor
                        habilitaControlesPagos(chk.Checked);

                        break;
                    }
                case "chkVarios":
                    {   
                        //Obteniendo Filas Seleccionadas
                        GridViewRow[] gvRows = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios");
                        //Habilitando Controles según la condición
                        habilitaControlesPagos(gvRows.Length > 0);
                        //Si no existen filas seleccionadas
                        if (gvRows.Length > 0)
                        {   
                            //Obteniendo Item de forma Dinamica*/
                            ListItem li = ddlTipoPago.Items.FindByText("Movimiento");
                            //Limpiando Items
                            ddlTipoPago.Items.Clear();
                            //Añadiendo Item
                            ddlTipoPago.Items.Add(li);
                            //Asignando Valor
                            txtDescripcion.Text = "Pago por Movimiento";
                            //Mostrando Total de Movimientos
                            txtCantidad.Text = gvRows.Length.ToString();

                            //Visualizando Control
                            btnCrearPago.Visible = btnCrearPago.Enabled = true;
                        }
                        else
                        {
                            //Limpiando Controles de Pagos
                            limpiaControlesPagos();

                            //Visualizando Control
                            btnCrearPago.Visible = btnCrearPago.Enabled = false;
                        }
                        break;
                    }
            }
            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvPagos);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
        }
        /// <summary>
        /// Evento Producido al Dar Click en el Link "Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarPago_Click(object sender, EventArgs e)
        {   
            //Validando que existan Movimientos
            if (gvMovimientos.DataKeys.Count > 0)
            {   
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Seleccionando la Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

                            //Obteniendo Tipos de Pago
                            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                            {
                                //Validando que existan los Conceptos de Pago
                                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                    //Cargando DropDownList
                                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                else
                                    //Inicializando DropDownList
                                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                            }
                            
                            //Obteniendo Control
                            LinkButton pago = (LinkButton)sender;
                            
                            //Instanciando Movimiento
                            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                            {
                                //Validando el Comando
                                switch (pago.CommandName)
                                {
                                    case "Editar":
                                        {

                                            break;
                                        }
                                    case "Pagar":
                                        {
                                            //Realizando búsqueda de tarifa y en caso de ser tarifa única se aplica el pago
                                            buscaTarifaPagoMovimiento();
                                            break;
                                        }
                                }
                            }
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Presionar el Link "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarMovimiento_Click(object sender, EventArgs e)
        {
            //Validando que existan Movimientos
            if (gvMovimientos.DataKeys.Count > 0)
            {   
                //Seleccionando la Fila
                TSDK.ASP.Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {
                    //Invocando Método de Habilitación de Controles
                    habilitaControlesLiquidacion(liq.estatus);

                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Obteniendo Pagos
                    using (DataTable dtPagos = SAT_CL.Liquidacion.Pago.ObtienePagosMovimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, liq.id_tipo_asignacion, id_entidad, liq.id_estatus, true))
                    {
                        //Validando que existen Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagos))
                        {
                            //Cargando Pagos
                            TSDK.ASP.Controles.CargaGridView(gvPagos, dtPagos, "IdPago", "", true, 1);
                            //Añadiendo Tablas a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPagos, "Table3");
                            //Mostrando Totales
                            gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvPagos);
                            //Eliminando Tabla de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                            //Mostrando Totales
                            gvPagos.FooterRow.Cells[5].Text = "0.00";
                        }
                    }
                    
                    //Instanciando Deposito
                    using (DataTable dtAnticipos = SAT_CL.EgresoServicio.Deposito.CargaDepositosMovimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), id_entidad, liq.id_tipo_asignacion))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtAnticipos))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvAnticipos, dtAnticipos, "Id", "", true, 1);
                            //Añadiendo tabla a Session
                            TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtAnticipos, "Table4");
                            //Mostrando Totales
                            gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                            //Eliminando Tabla de Session
                            TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                            //Mostrando Totales
                            gvAnticipos.FooterRow.Cells[6].Text = "0.00";
                        }
                    }

                    //Obteniendo Comprobaciones
                    using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesMovimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text)))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvComprobaciones, dtComprobaciones, "Id", "", true, 1);
                            //Añadiendo tabla a Session
                            TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtComprobaciones, "Table5");
                            //Mostrando Totales
                            gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                            //Eliminando Tabla de Session
                            TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                            //Mostrando Totales
                            gvComprobaciones.FooterRow.Cells[4].Text = "0.00";
                        }
                    }

                    //Obteniendo Vales de Diesel
                    using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvDiesel, dtDiesel, "Id", "", true, 2);
                            //Añadiendo tabla a Session
                            TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtDiesel, "Table7");
                            //Mostrando Totales
                            gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvDiesel);

                            //Eliminando Tabla de Session
                            TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");

                            //Mostrando Sumatoria de Totales
                            gvDiesel.FooterRow.Cells[5].Text = "0.00";
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvMovimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {   
            //Validando que la Fila sea de Tipo de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Recuperando la fila de datos correspondiente
                DataRow dr = ((DataRowView)e.Row.DataItem).Row.Table.Rows[e.Row.DataItemIndex];
                //Si hay registrosa ligados
                if (dr["NoPago"] != DBNull.Value)
                {
                    //Obteniendo Indicador de Pago
                    int idPago = Convert.ToInt32(dr["NoPago"]);
                    
                    //Obteniendo Controles de interés sobre la fila del gridview
                    LinkButton pago = (LinkButton)e.Row.FindControl("lnkEditarPago");
                    CheckBox chk = (CheckBox)e.Row.FindControl("chkVarios");
                    LinkButton evidencia = (LinkButton)e.Row.FindControl("lnkEvidencias");

                    //Validando que exista el Control "lnkEditarPago"
                    if (pago != null)
                    {
                        //Cambiando el Texto del Control    
                        //Si ya tiene pago y no es de tarifa
                        if (idPago != 0)
                            pago.CommandName = pago.Text = "";
                        //Si no hay pago
                        else
                            pago.CommandName = pago.Text = "Pagar";
                    }

                    //Validando que exista el Control "chkVarios"
                    if (chk != null)
                        //Cambiando la Habilitación del Control
                        chk.Enabled = idPago == 0 ? true : false;

                    /*/Validando que exista el Control "lnkEvidencias"
                    if (evidencia != null)
                        //Cambiando el Texto del Control
                        evidencia.Visible = ((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[7].ToString() != "0" ? true : false;*/
                }
            }
        }
        /// <summary>
        /// Evento disparado al Presionar el LinkButton "Evidencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEvidencias_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvMovimientos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvMovimientos, sender, "lnk", false);

                //Validando si existen Evidencias
                if (Convert.ToInt32(gvMovimientos.SelectedDataKey["NoSegmento"]) > 0)
                {
                    //Instanciando el Movimiento
                    using (Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                    {
                        //Validando que exista el Movimiento
                        if (mov.id_servicio > 0)
                        {
                            //Invocando Método de Carga
                            cargaImagenesDetalle(mov.id_servicio);
                            
                            //Alternando Ventana Modal
                            TSDK.ASP.ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "VentanaEvidencias", "contenidoVentanaEvidencias", "ventanaEvidencias");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Evento disparado al Presionar el Boton "Cerrar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarImagen_Click(object sender, EventArgs e)
        {
            //Seleccionando Fila
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

            //Alternando Ventana Modal
            TSDK.ASP.ScriptServer.AlternarVentana(uplnkCerrarImagen, uplnkCerrarImagen.GetType(), "VentanaEvidencias", "contenidoVentanaEvidencias", "ventanaEvidencias");
        }

        #endregion

        #region Eventos GridView "Pagos"

        /// <summary>
        /// Eventos Producido al Cambiar el Ordenamiento de "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoPago.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.SortExpression);
            
            //Validando que Existan Registros
            if (gvPagos.DataKeys.Count > 0)

                //Mostrando Totales
                gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página de "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), e.NewPageIndex);

            //Validando que Existan Registros
            if (gvPagos.DataKeys.Count > 0)

                //Mostrando Totales
                gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Tamaño del GridView "Pagos"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvPagos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), Convert.ToInt32(ddlTamanoPago.SelectedValue));

            //Validando que Existan Registros
            if (gvPagos.DataKeys.Count > 0)

                //Mostrando Totales
                gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Eliminar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarPago_Click(object sender, EventArgs e)
        {   
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table3"), "Id-IdPago");
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Editar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditarPagoGnrl_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvPagos, sender, "lnk", false);

                //Instanciando Pago
                using (SAT_CL.Liquidacion.Pago pag = new SAT_CL.Liquidacion.Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                {
                    //Validando que existe el Pago
                    if (pag.id_pago != 0)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pag.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if (liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                                {
                                    //Habilitando Controles
                                    habilitaControlesPagos(true);

                                    //Obteniendo Tipos de Pago
                                    using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                                    {
                                        //Validando que existan los Conceptos de Pago
                                        if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                            //Cargando DropDownList
                                            Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                                        else
                                            //Inicializando DropDownList
                                            Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                                    }

                                    //Obteniendo Item de forma Dinamica
                                    ListItem li = ddlTipoPago.Items.FindByValue(pag.id_tipo_pago.ToString());
                                    //Limpiando Items
                                    ddlTipoPago.Items.Clear();
                                    //Añadiendo Item
                                    ddlTipoPago.Items.Add(li);
                                    
                                    //Asignando Valores 
                                    ddlTipoPago.SelectedValue = pag.id_tipo_pago.ToString();

                                    //Asignando Valores
                                    txtCantidad.Text = pag.objDetallePago.cantidad.ToString();
                                    txtValorU.Text = pag.objDetallePago.valor_unitario.ToString();
                                    txtTotal.Text = pag.objDetallePago.monto.ToString();
                                    txtDescripcion.Text = pag.descripcion;
                                    txtReferencia.Text = pag.referencia;

                                    //Asignando Comando
                                    btnGuardarPago.CommandName = "EditaPago";
                                    upbtnGuardarPago.Update();

                                    //Mostrando ventana de captura
                                    TSDK.ASP.ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "ventanaTarifasPago", "contenedorVentanaPagos", "ventanaPagos");
                                }
                                else
                                {
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);

                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                    else
                        //Asignando Valores 
                        limpiaControlesPagos();

                    //Quitando la selección de las Filas
                    TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
                }
            }
        }
        /// <summary>
        /// Evento Producido al dar Click en el Boton "Eliminar Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminar_Click(object sender, EventArgs e)
        {
            //Validando si existen los Pagos
            if (gvPagos.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvPagos, sender, "lnk", false);

                //Instanciando Pago
                using (Pago pago = new Pago(Convert.ToInt32(gvPagos.SelectedDataKey["IdPago"])))
                {
                    //Validando que Exista
                    if(pago.id_pago > 0)
                    {
                        //Instanciando Liquidación
                        using(SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(pago.objDetallePago.id_liquidacion))
                        {
                            //Validando que Exista
                            if(liq.id_liquidacion > 0)
                            {
                                //Validando el Estatus de la Liquidación
                                if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)

                                    //Alternando Ventana Modal
                                    TSDK.ASP.ScriptServer.AlternarVentana(upgvPagos, upgvPagos.GetType(), "ConfirmacionOperacion", "contenedorVentanaConfirmacion", "ventanaConfirmacion");
                                else
                                {   
                                    //Inicializando Indices
                                    TSDK.ASP.Controles.InicializaIndices(gvPagos);
                                    
                                    //Mostrando Mensaje de Operación
                                    TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos Gridview "Anticipos"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), Convert.ToInt32(ddlTamanoAnticipos.SelectedValue));

            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
                //Mostrando Totales
                gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarAnticipos_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), "IdDetalle");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoAnticipo.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.SortExpression);
            
            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
                //Mostrando Totales
                gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAnticipos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvAnticipos, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table4"), e.NewPageIndex);
            
            //Validando que Existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
                //Mostrando Totales
                gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// Evento Producido al Presionar el LinkButton "Seleccionar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkSeleccionarDeposito_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvAnticipos.DataKeys.Count > 0)
            {
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {

                            //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvAnticipos, sender, "lnk", false);

                            //Inicializando Controles
                            inicializaControlComprobaciones(0, Convert.ToInt32(gvAnticipos.SelectedDataKey["Id"]));

                            //Declarando Script de Ventana Modal
                            string script = @"<script type='text/javascript'>
                                //Mostrando ventana modal 
                                $('#contenedorVentanaComprobaciones').animate({ width: 'toggle' });
                                $('#ventanaComprobaciones').animate({ width: 'toggle' });
                            </script>";

                            //Registrando Script
                            ScriptManager.RegisterStartupScript(upgvAnticipos, upgvAnticipos.GetType(), "VentanaComprobacion", script, false);
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvAnticipos);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }
            }
        }

        #endregion

        #region Eventos GridView "Comprobaciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkCerrarComprobacion_Click(object sender, EventArgs e)
        {
            //Deseleccionando Todas las Filas
            TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
            TSDK.ASP.Controles.InicializaIndices(gvAnticipos);

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                //Cargando resultados Actualizados
                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
            }

            //Declarando Script de Ventana Modal
            string script = @"<script type='text/javascript'>
                                //Mostrando ventana modal 
                                $('#contenedorVentanaComprobaciones').animate({ width: 'toggle' });
                                $('#ventanaComprobaciones').animate({ width: 'toggle' });
                            </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(uplnkCerrarComprobacion, uplnkCerrarComprobacion.GetType(), "VentanaComprobacion", script, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), Convert.ToInt32(ddlTamanoComp.SelectedValue));
            
            //Validando que existan Comprobaciones
            if (gvComprobaciones.DataKeys.Count > 0)
                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.SortExpression);
            //Validando que existan Comprobaciones
            if (gvComprobaciones.DataKeys.Count > 0)
                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvComprobaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvComprobaciones, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), e.NewPageIndex);

            //Validando que existan Comprobaciones
            if (gvComprobaciones.DataKeys.Count > 0)
                //Mostrando Totales
                gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarComp_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table5"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEditaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Validando que este seleccionado un Viaje
                if (gvViajes.SelectedIndex != -1)
                {
                    //Instanciando Liquidación
                    using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                    {
                        //Validando que Exista
                        if (liq.id_liquidacion > 0)
                        {
                            //Validando el Estatus de la Liquidación
                            if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                            {

                                //Seleccionando Fila
                                TSDK.ASP.Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                                //Inicializando Controles
                                inicializaControlComprobaciones(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"]), 0);

                                //Declarando Script de Ventana Modal
                                string script = @"<script type='text/javascript'>
                                //Mostrando ventana modal 
                                $('#contenedorVentanaComprobaciones').animate({ width: 'toggle' });
                                $('#ventanaComprobaciones').animate({ width: 'toggle' });
                            </script>";

                                //Registrando Script
                                ScriptManager.RegisterStartupScript(upgvComprobaciones, upgvComprobaciones.GetType(), "VentanaComprobacion", script, false);
                            }
                            else
                            {
                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                                //Mostrando Mensaje de Operación
                                TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminaComp_Click(object sender, EventArgs e)
        {
            //Validando que existan 
            if (gvComprobaciones.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Instanciando Liquidación
                using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                {
                    //Validando que Exista
                    if (liq.id_liquidacion > 0)
                    {
                        //Validando el Estatus de la Liquidación
                        if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado)
                        {
                            //Seleccionando Fila
                            TSDK.ASP.Controles.SeleccionaFila(gvComprobaciones, sender, "lnk", false);

                            //Instanciando Comprobación
                            using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(gvComprobaciones.SelectedDataKey["Id"])))
                            {
                                //Validando que exista la COmprobación
                                if (cmp.id_comprobacion > 0)
                                {
                                    //Validando que el Movimiento este Terminado
                                    if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))

                                        //Deshabilitando el Registro
                                        result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    else
                                        //Instanciando Excepción
                                        result = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");
                                }
                            }

                            //Validando que la Operación haya sido exitosa
                            if (result.OperacionExitosa)
                            {
                                //Obteniendo Entidad
                                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                                //Cargando resultados Actualizados
                                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                                //Inicializando Indices
                                TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
                            }
                        }
                        else
                        {
                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                            //Mostrando Mensaje de Operación
                            TSDK.ASP.ScriptServer.MuestraNotificacion(this, "La Liquidación esta Cerrada, Imposible su Edición", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        }
                    }
                }

                //Mostrando Mensaje
                lblErrorComprobacion.Text = result.Mensaje;
            }
        }

        #endregion

        #region Eventos GridView "Facturas de Comprobaciones"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFacComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), Convert.ToInt32(ddlTamanoFacComp.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFacComp_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), "Id");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoFacComp.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFacturasComprobacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvFacturasComprobacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table6"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarFactura_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if (gvFacturasComprobacion.DataKeys.Count > 0)
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();

                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvFacturasComprobacion, sender, "lnk", false);

                //Inicializando Transacción
                using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                {
                    //Instanciando Factura
                    using (FacturadoProveedorRelacion cfp = new FacturadoProveedorRelacion(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFacturaRelacion"])))
                    {
                        //Validando que exista la Factura
                        if (cfp.habilitar)
                        {
                            //Deshabilitando Factura
                            result = cfp.DeshabilitarFacturaPoveedorRelacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando la Operación de la Transacción
                            if (result.OperacionExitosa)
                            {
                                //Instanciando Factura
                                using (FacturadoProveedor fp = new FacturadoProveedor(Convert.ToInt32(gvFacturasComprobacion.SelectedDataKey["IdFactura"])))
                                {
                                    //Deshabilitando Factura
                                    result = fp.DeshabilitaFacturadoProveedor(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                    //Validando la Operación de la Transacción
                                    if (result.OperacionExitosa)
                                    {
                                        //Instanciando Comprobación
                                        using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text)))
                                        {
                                            //Validando que exista la Comprobación
                                            if (cmp.id_comprobacion > 0)
                                            {
                                                //Actualizando el Total de las Comprobaciones
                                                result = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando la Operación de la Transacción
                                                if (result.OperacionExitosa)

                                                    //Completando Transacción
                                                    trans.Complete();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //Validando la Operación de la Transacción
                if (result.OperacionExitosa)

                    //Inicializando Controles
                    inicializaControlComprobaciones(Convert.ToInt32(lblIdComprobacion.Text), 0);

                //Mostrando mensaje de Operacion
                lblErrorComprobacion.Text = result.Mensaje;

            }
        }

        #endregion

        #region Eventos GridView "Diesel"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoDiesel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), Convert.ToInt32(ddlTamanoDiesel.SelectedValue));
            
            //Validando que existan Vales de Diesel
            if (gvDiesel.DataKeys.Count > 0)

                //Mostrando Totales
                gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarDiesel_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.NewPageIndex);

            //Validando que existan Vales de Diesel
            if (gvDiesel.DataKeys.Count > 0)

                //Mostrando Totales
                gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDiesel_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoDiesel.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvDiesel, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table7"), e.SortExpression);

            //Validando que existan Vales de Diesel
            if (gvDiesel.DataKeys.Count > 0)

                //Mostrando Totales
                gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
        }

        #endregion

        #region Eventos GridView "Cobros Recurrentes"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCR_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), Convert.ToInt32(ddlTamanoCR.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarCR_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentes_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCR.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobrosRecurrentes, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table8"), e.NewPageIndex);
        }

        #endregion

        #region Eventos GridView "Cobro Recurrente Liquidación"
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarCRL_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCRL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), Convert.ToInt32(ddlTamanoCRL.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobroRecurrenteLiquidacion_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCRL.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobroRecurrenteLiquidacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobroRecurrenteLiquidacion, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table9"), e.NewPageIndex);
        }

        #endregion

        #region Eventos GridView "Cobro Recurrente Historial"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarCRH_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoCRH_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), Convert.ToInt32(ddlTamanoCRH.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentesHistorial_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoCRH.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.SortExpression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCobrosRecurrentesHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvCobrosRecurrentesHistorial, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table10"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkVerHistorial_Click(object sender, EventArgs e)
        {
            //Validando que existan Registros
            if(gvCobroRecurrenteLiquidacion.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                TSDK.ASP.Controles.SeleccionaFila(gvCobroRecurrenteLiquidacion, sender, "lnk", false);

                //Instanciando Cobro Recurrente
                using (SAT_CL.Liquidacion.CobroRecurrente cr = new SAT_CL.Liquidacion.CobroRecurrente(Convert.ToInt32(gvCobroRecurrenteLiquidacion.SelectedDataKey["Id"])))
                {
                    //Validando que exista el Cobro Recurrente
                    if (cr.id_cobro_recurrente > 0)
                    {
                        //Instanciando Cobros Recurrentes
                        using (DataTable dtCobrosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneHistorialCobrosRecurrentes(cr.id_cobro_recurrente, cr.id_tipo_entidad_aplicacion, cr.id_unidad, cr.id_operador, cr.id_proveedor_compania, cr.id_compania_emisor))
                        {
                            //Validando que existen los Cobros Recurrentes
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosRecurrentes))
                            {
                                //Cargando Cobro Recurrente
                                TSDK.ASP.Controles.CargaGridView(gvCobrosRecurrentesHistorial, dtCobrosRecurrentes, "Id", "");

                                //Añadiendo Tabla a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCobrosRecurrentes, "Table10");
                            }
                            else
                            {
                                //Inicializando Cobro Recurrente
                                TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentesHistorial);

                                //Añadiendo Tabla a Session
                                Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table10");
                            }
                        }
                    }
                    else
                    {
                        //Inicializando Cobro Recurrente
                        TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentesHistorial);

                        //Añadiendo Tabla a Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table10");
                    }

                    //Alternando Ventana Modal
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvCobroRecurrenteLiquidacion, upgvCobroRecurrenteLiquidacion.GetType(), "VerCobrosHistorial", "contenedorVentanaHistorialCobrosRecurrentes", "ventanaHistorialCobrosRecurrentes");
                }
            }
        }

        #endregion

        #region Eventos GridView "Evidencias"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoEvidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando Tamaño del GridView
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), Convert.ToInt32(ddlTamanoEvidencia.SelectedValue));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarEvidencia_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), "Id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.NewPageIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresion de Ordenamiento
            lblOrdenadoEvidencias.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvEvidencias, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table12"), e.SortExpression);
        }
        /// <summary>
        /// Evento Producido al Enlazar los Datos al Control GridView "Evidencias"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEvidencias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Validando que sea de Tipo Registro de Datos
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Obteniendo Control
                ImageButton ib = (ImageButton)e.Row.FindControl("ibEstatus");

                //Validando que exista el Control
                if (ib != null)
                {
                    //Validando el Estatus
                    switch (((System.Data.DataRowView)(e.Row.DataItem)).Row.ItemArray[2].ToString())
                    {
                        //Recibido
                        case "1":
                        //Recibido c/Reenvio
                        case "2":
                            {
                                //Asignando Valores
                                ib.ImageUrl = "~/Image/EstatusRecibido.png";
                                ib.Visible = true;
                                break;
                            }
                        default:
                            {
                                //Asignando Valores
                                ib.ImageUrl = "";
                                ib.Visible = false;
                                break;
                            }
                    }
                }
            }
        }

        #endregion

        #region Eventos "Tarifas de Pago"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarVentanaTarifasPago_Click(object sender, EventArgs e)
        {
            //Mostrando ventana modal con resultados
            TSDK.ASP.ScriptServer.AlternarVentana(uplkbCerrarVentanaTarifasPago, uplkbCerrarVentanaTarifasPago.GetType(), "ventanaTarifasPago", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
        }
        /// <summary>
        /// Maneja el evento Click sobre el boton APLICAR del Gridview de Tarifas de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbSeleccionarTarifaPago_Click(object sender, EventArgs e)
        {
            //Seleccionando fila actual
            TSDK.ASP.Controles.SeleccionaFila(gvTarifasPago, sender, "lnk", false);
            //Aplicando tarifa seleccionada sobre el registro correspondiente
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text)))
                {
                    //Determinando que entidad se afecta con esta liquidación
                    int id_entidad = 0;
                    switch (liquidacion.tipo_asignacion)
                    {
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad:
                            id_entidad = liquidacion.id_unidad;
                            break;
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador:
                            id_entidad = liquidacion.id_operador;
                            break;
                        case SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Proveedor:
                            id_entidad = liquidacion.id_proveedor;
                            break;
                    }

                    //Si la búsqueda de tarifa se relizó por servicio
                    if (gvTarifasPago.ToolTip == "Tarifas de Pago Servicio")
                    {
                        //Aplicando tarifa al servicio
                        resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value), Convert.ToInt32(gvViajes.SelectedDataKey.Value),
                                                                liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si se actualizó correctamente
                        if (resultado.OperacionExitosa)
                        {
                            //Obteniendo Pago
                            int idPago = resultado.IdRegistro;
                            
                            //Obteniendo Tarifas Secundarias
                            using (DataTable dtTarifasSecundarias = SAT_CL.TarifasPago.TarifaCompuesta.ObtieneTarifasSecundarias(Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value)))
                            {
                                //Validando que Existen Registros
                                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTarifasSecundarias))
                                {
                                    //Recorriendo Ciclo de Tarifas Secundarias
                                    foreach (DataRow dr in dtTarifasSecundarias.Rows)
                                    {
                                        //Aplicando Tarifa Secundaria
                                        resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(dr["IdTarifaSecundaria"]), Convert.ToInt32(gvViajes.SelectedDataKey.Value),
                                                                liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que la Operación fuese Correcta
                                        if (!resultado.OperacionExitosa)

                                            //Terminando Ciclo
                                            break;
                                    }
                                }
                                else
                                    //Instanciando Resultado Positivo
                                    resultado = new RetornoOperacion(idPago);

                                //Validando que la Operación fuese Exitosa
                                if (resultado.OperacionExitosa)
                                {
                                    //Realizando actualización de vales y anticipos
                                    resultado = actualizaLiquidacionDepositosValesServicio(Convert.ToInt32(gvViajes.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad, liquidacion.id_tipo_asignacion);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                    {
                                        //Cargando los Movimientos y los Pagos
                                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad, liquidacion.id_tipo_asignacion, liquidacion.id_estatus);
                                        //Confirmando cambios realizados
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                    }
                    //Si la búsqueda fue para un movimiento
                    else
                    {
                        //Aplicando tarifa
                        resultado = Pago.AplicaTarifaPagoMovimiento(liquidacion.id_liquidacion, Convert.ToInt32(gvTarifasPago.SelectedDataKey.Value), Convert.ToInt32(gvMovimientos.SelectedDataKey.Value),
                                                                  liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                        //Si no hay problemas con el pago
                        if (resultado.OperacionExitosa)
                        {                            
                            //Realizando actualización de vales y anticipos
                            resultado = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad, liquidacion.id_tipo_asignacion, true);

                            //Si no hay errores
                            if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                            {                                
                                //Cargando los Movimientos y los Pagos
                                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad, liquidacion.id_tipo_asignacion, liquidacion.id_estatus);
                                //Confirmando cambios realizados
                                scope.Complete();
                            }
                            else
                                resultado = new RetornoOperacion(string.Format("Error al actualizar anticipos y vales del movimiento ID '{0}': {1}", gvMovimientos.SelectedDataKey.Value, resultado.Mensaje));
                        }
                    }

                    //Cerrando ventana modal con resultados
                    TSDK.ASP.ScriptServer.AlternarVentana(upgvTarifasPago, upgvTarifasPago.GetType(), "ventanaTarifasPago", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
                }
            }

            //Borrando selección de elemento
            TSDK.ASP.Controles.InicializaIndices(gvTarifasPago);

            //Mostrando resultado
            lblErrorLiquidacion.Text = resultado.Mensaje;
        }



        /// <summary>
        /// Maneja el cambio de selección en el catálogo de Tarifas Aplicables mostrados por página 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoGridViewTarifasAplicables_SelectedIndexChanged(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.CambiaTamañoPaginaGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table13"), Convert.ToInt32(ddlTamanoGridViewTarifasAplicables.SelectedValue));
        }
        /// <summary>
        /// Maneja el evento click en el botón de exportación de Tarifas Aplicables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbExportarTarifasAplicales_Click(object sender, EventArgs e)
        {
            TSDK.ASP.Controles.ExportaContenidoGridView(TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table13"), "");
        }
        /// <summary>
        /// Maneja el cambio de página de Gridview de tarifas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifasPago_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TSDK.ASP.Controles.CambiaIndicePaginaGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table13"), e.NewPageIndex);
        }
        /// <summary>
        /// Maneja el ordenamiento de las Tarifas Aplicables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTarifasPago_Sorting(object sender, GridViewSortEventArgs e)
        {
            lblOrdenadoTarifasAplicables.Text = TSDK.ASP.Controles.CambiaSortExpressionGridView(gvTarifasPago, TSDK.Datos.OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table13"), e.SortExpression);
        }

        #endregion

        #region Eventos Recibo de Nómina
        /// <summary>
        /// Evento generado al dar click en Recibo de Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbReciboNomina_Click(object sender, EventArgs e)
        {
              //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validamos que exista Liquidación
                if (objLiquidacion.id_liquidacion > 0)
                {
                    //Creación del objeto botonMenú
                    LinkButton botonMenu = (LinkButton)sender;
                    //Evalúa cada opcion del menú Principal.
                    switch (botonMenu.CommandName)
                    {
                        case "Timbrar":
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(lkbTimbrar, lkbTimbrar.GetType(), "AbrirVentana", "contenidoConfirmacionTimbrarLiquidacion", "confirmacionTimbrarLiquidacion");

                            break;
                        case "Cancelar":
                            //Abrir Ventana Modal
                            ScriptServer.AlternarVentana(lkbCancelarTimbrado, lkbCancelarTimbrado.GetType(), "AbrirrVentana", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");


                            break;
                    }
                }
            }           
        }

        /// <summary>
        /// Evento generado al Cancelar el Recibo de Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarCancelarTimbrado_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validamos que exista Liquidación
                if(objLiquidacion.id_liquidacion > 0)
                {
                    //Obtenemos Comprobante Vigente
                    using (SAT_CL.FacturacionElectronica.Comprobante objComprobante = new SAT_CL.FacturacionElectronica.Comprobante(SAT_CL.FacturacionElectronica.Comprobante.ObtieneReciboNominaVigente(objLiquidacion.id_liquidacion)))
                    {
                        //Validamos Comprobante
                        if (objComprobante.id_comprobante > 0)
                        {
                            //Timbramos Liquidación
                            resultado = objComprobante.CancelaComprobante(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            if (resultado.OperacionExitosa)
                            {
                                //Cerrar Ventana Modal
                                ScriptServer.AlternarVentana(btnAceptarCancelarTimbrado, btnAceptarCancelarTimbrado.GetType(), "CerrarVentana", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");

                                //Limpiamos Etiqueta
                                lblErrorCancelarTimbrado.Text = "";
                            }
                            else
                            {
                                //Mostramos Error
                                lblErrorCancelarTimbrado.Text = resultado.Mensaje;
                            }
                        }
                        else
                        {
                            //Establecemos Mesaje
                            lblErrorCancelarTimbrado.Text = "No existe Recibo de Nómina Vigente.";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Evento generado al Timbrar una Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAceptarTimbrarLiquidacion_Click(object sender, EventArgs e)
        {
            //Declaramos objeto Resultado
            RetornoOperacion resultado = new RetornoOperacion();
            //Instanciamos Liquidacion
            using (SAT_CL.Liquidacion.Liquidacion objLiquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Validamos que exista Liquidación
                if (objLiquidacion.id_liquidacion > 0)
                {
                    //Timbramos Liquidación
                  //  resultado = objLiquidacion.ImportaTimbraLiquidacionComprobante_V3_2(   ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,"N", HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2.xslt"),
                                                                  //  HostingEnvironment.MapPath("~/XSLT/FacturacionElectronica/cadenaoriginal_3_2_desconectado.xslt"), Catalogo.RegresaDescripcionCatalogo(1111, Convert.ToInt32(ddlPeriocidadPago.SelectedValue)), Convert.ToInt32(ddlMetodoPago.SelectedValue),
                                                                  //  Convert.ToInt32(ddlSucursal.SelectedValue), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    //Mostramos Error
                    lblErrorTimbrarLiquidacion.Text = resultado.Mensaje;
                }
            }
        }

        /// <summary>
        /// Evento generado al cerrar la ventana de Timbrado de la Liquidación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarTimbrarLiquidacion_Click(object sender, EventArgs e)
        {

            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarTimbrarLiquidacion, lkbCerrarTimbrarLiquidacion.GetType(), "CerrarVentana", "contenidoConfirmacionTimbrarLiquidacion", "confirmacionTimbrarLiquidacion");

            //Limpiamos Etiqueta
            lblErrorTimbrarLiquidacion.Text = "";
            ddlPeriocidadPago.SelectedValue = "1";
        }

        /// <summary>
        /// Evento generado al Cancelar el Recibo de Nómina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbCerrarCancelarTimbrado_Click(object sender, EventArgs e)
        {
            //Cerrar Ventana Modal
            ScriptServer.AlternarVentana(lkbCerrarCancelarTimbrado, lkbCerrarCancelarTimbrado.GetType(), "CerrarVentana", "contenidoConfirmacionCancelarTimbrado", "confirmacionCancelarTimbrado");

            //Limpiamos Etiqueta
            lblErrorCancelarTimbrado.Text = "";

        }
        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar la Página
        /// </summary>
        private void inicializaPagina()
        {   
            //Invocando Método de Carga 
            cargaCatalogos();
            
            //Limpiando Session
            Session["DS"] = null;
            Session["id_registro"] = 0;
            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
            TSDK.ASP.Controles.InicializaGridview(gvViajes);
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvPagos);
            TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
            TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
            TSDK.ASP.Controles.InicializaGridview(gvFacturasComprobacion);
            TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentes);
            TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
            
            //Invocando Métodos de Carga de Encabezado
            habilitaControlesEncabezado(Pagina.Estatus.Lectura);
            limpiaDatosEncabezadoLiquidacion();
            
            //Deshabilitando Controles de Pago
            habilitaControlesPagos(false);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos
        /// </summary>
        private void cargaCatalogos()
        {   
            //Cargando Catalogos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEnt, "", 62);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");

            //Obteniendo Tipos de Pago
            using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
            {
                //Validando que existan los Conceptos de Pago
                if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                    //Cargando DropDownList
                    Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                else
                    //Inicializando DropDownList
                    Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
            }

            //Cargando Catalogos de Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoViajes, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoMov, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoPago, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamano, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoAnticipos, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFacComp, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoDiesel, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCR, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCRL, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoCRH, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoEvidencia, "", 26);
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoGridViewTarifasAplicables, "", 18);

            //Recibo de  Nómina
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlPeriocidadPago, "", 1111);
            //Sucursales
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlSucursal, 50, "Sin Asignar", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Métodos de Pago
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlMetodoPago, "", 80);

        }
        /// <summary>
        /// Método Privado encargado de habilitar los Controles del Encabezado del Liquidación
        /// </summary>
        /// <param name="estatus">Estatus de Controles</param>
        private void habilitaControlesEncabezado(Pagina.Estatus estatus)
        {   
            //Validando Estatus
            switch(estatus)
            {   
                //Lectura
                case Pagina.Estatus.Lectura:
                    {   
                        //Asignando Habilitación
                        txtFecha.Enabled = 
                        txtDescripcion.Enabled = 
                        txtReferencia.Enabled = 
                        btnGuardar.Enabled = 
                        btnCancelar.Enabled = false;
                        btnEditar.Enabled = true;
                        break;
                    }
                //Edición
                case Pagina.Estatus.Edicion:
                    {   
                        //Asignando Habilitación
                        txtFecha.Enabled = 
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = true;
                        txtDescripcion.Enabled =
                        txtReferencia.Enabled =
                        btnEditar.Enabled = false;
                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de inicializar los Valores de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion"></param>
        private void inicializaValoresLiquidacion(int id_liquidacion)
        {   
            //Instanciando Encabezado de Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
            {   
                //Validando que existe la Liquidacion
                if (liq.id_liquidacion != 0)
                {   
                    //Cambiando Indice del MultiView
                    mtvEncabezado.ActiveViewIndex = 1;
                    btnBusqueda.CssClass = "boton_pestana";
                    btnLiquidacion.CssClass = "boton_pestana_activo";
                    //Asignando Valores
                    lblId.Text = liq.id_liquidacion.ToString();
                    lblNoLiq.Text = liq.no_liquidacion.ToString();
                    txtFecha.Text = liq.fecha_liquidacion.ToString("dd/MM/yyyy HH:mm");
                    lblEstatus.Text = liq.estatus.ToString();

                    //Validando que se encuentre Liquidado
                    if (liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado)
                    {
                        //Asignando Valores
                        lblPercepciones.Text = liq.total_salario.ToString();
                        lblSueldo.Text = liq.total_sueldo.ToString();
                        lblAnticipos.Text = liq.total_anticipos.ToString();
                        lblDeducciones.Text = liq.total_deducciones.ToString();
                        lblComprobaciones.Text = liq.total_comprobaciones.ToString();
                        lblDescuentos.Text = liq.total_descuentos.ToString();
                        lblAlcance.Text = liq.total_alcance.ToString();
                    }
                    else
                        //Actualizando Carga de Totales de la Liquidación
                        cargaValoresTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                                        
                    //Habilitando Controles
                    btnCerrarLiquidacion.Enabled = liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado ? true : false;

                    //Validando que el Tipo de Asignación del Recurso sea Unidad
                    if (liq.id_tipo_asignacion == 1)
                    {   
                        //Instanciando Unidad
                        using (SAT_CL.Global.Unidad uni = new SAT_CL.Global.Unidad(liq.id_unidad))
                            //Asignando Descripción
                            lblEntidad.Text = uni.numero_unidad;
                        lblTipoEntidad.Text = "Unidad";
                    }
                    //Validando que el Tipo de Asignación del Recurso sea Operador
                    else if (liq.id_tipo_asignacion == 2)
                    {   
                        //TO DO: Clase de Operador
                        using (SAT_CL.Global.Operador op = new SAT_CL.Global.Operador(liq.id_operador))
                            //Asignando Descripción
                            lblEntidad.Text = op.nombre;
                        lblTipoEntidad.Text = "Operador";
                    }
                    //Validando que el Tipo de Asignación del Recurso sea Proveedor
                    else if (liq.id_tipo_asignacion == 3)
                    {   
                        //Instanciando Unidad
                        using (SAT_CL.Global.CompaniaEmisorReceptor pro = new SAT_CL.Global.CompaniaEmisorReceptor(liq.id_proveedor))
                            //Asignando Descripción
                            lblEntidad.Text = pro.nombre;
                        lblTipoEntidad.Text = "Proveedor";
                    }
                    //Declarando Variable Auxiliar
                    int id_entidad = 0;
                    //Asignando Entidad Encontrada
                    if (liq.id_operador != 0)
                        id_entidad = liq.id_operador;
                    else if (liq.id_unidad != 0)
                        id_entidad = liq.id_unidad;
                    else if (liq.id_proveedor != 0)
                        id_entidad = liq.id_proveedor;
                    
                    //Instanciando Reporte
                    using (DataTable dtViajes = SAT_CL.Despacho.Reporte.ObtieneViajesEntidad(liq.id_tipo_asignacion, id_entidad, liq.fecha_liquidacion, liq.id_estatus))
                    {   
                        //Validando que existan los Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtViajes))
                        {   
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvViajes, dtViajes, "Id", "", true, 1);
                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtViajes, "Table1");
                        }
                        else
                        {   
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvViajes);
                            //Añadiendo Tabla a Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
                        }
                    }

                    //Invocando Método de Carga de Pagos Generales
                    cargaPagosGeneralLiquidacion(liq.id_liquidacion);
                    
                    //Invocando Método de Carga
                    cargaCobrosRecurrentes(liq.id_liquidacion);

                    //Invocando Método de Carga
                    cargaCobrosRecurrentesTotales(liq.id_liquidacion, liq.id_tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora);
                }
                else
                    //Invocando Método de Limpieza de Controles
                    limpiaDatosEncabezadoLiquidacion();
                
                //Inicializando Indices de los Viajes
                TSDK.ASP.Controles.InicializaIndices(gvViajes);
                //Inicializando Movimientos y Pagos
                TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
                TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            }            
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los Controles del Encabezado de Liquidación
        /// </summary>
        private void limpiaDatosEncabezadoLiquidacion()
        {   
            //Limpiando Controles
            lblTipoEntidad.Text = "Por Asignar";
            lblEntidad.Text = "Ninguna";
            lblPercepciones.Text =
            lblAnticipos.Text =
            lblDeducciones.Text =
            lblComprobaciones.Text =
            lblAlcance.Text = "0.00";
            
            //Inicializando GridView de Viajes
            TSDK.ASP.Controles.InicializaGridview(gvViajes);
            //Añadiendo Tabla a Session
            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table1");
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Valores Totales de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion">Liquidación</param>
        /// <param name="tipo_asignacion">Tipo de Asignación de la Liquidación</param>
        /// <param name="id_unidad">Unidad de la Liquidación</param>
        /// <param name="id_operador">Operador de la Liquidación</param>
        /// <param name="id_proveedor">Proveedor de la Liquidación</param>
        /// <param name="id_compania_emisora">Compania Emisora de la Liquidación</param>
        private void cargaValoresTotalesLiquidacion(int id_liquidacion, SAT_CL.Liquidacion.Liquidacion.TipoAsignacion tipo_asignacion, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisora)
        {
            //Obteniendo Totales
            using(DataTable dtTotalesLiq = SAT_CL.Liquidacion.Liquidacion.ObtieneMontosTotalesLiquidacion(id_liquidacion, tipo_asignacion, id_unidad, id_operador, id_proveedor, id_compania_emisora))
            {
                //Validando que existan los Valores
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotalesLiq))
                {
                    //Recorriendo Tabla
                    foreach (DataRow dr in dtTotalesLiq.Rows)
                    {
                        //Mostrando Valores Obtenidos
                        lblPercepciones.Text = dr["TPercepcion"].ToString();
                        lblSueldo.Text = dr["TSueldo"].ToString();
                        lblAnticipos.Text = dr["TAnticipos"].ToString();
                        lblDeducciones.Text = dr["TDeducciones"].ToString();
                        lblComprobaciones.Text = dr["TComprobaciones"].ToString();
                        lblAlcance.Text = dr["TAlcance"].ToString();
                        lblDescuentos.Text = dr["TDescuentos"].ToString();
                    }
                }
                else
                {
                    //Mostrando Valores por Defecto
                    lblPercepciones.Text = 
                    lblSueldo.Text = 
                    lblAnticipos.Text = 
                    lblDeducciones.Text = 
                    lblComprobaciones.Text =
                    lblAlcance.Text = "0.00";
                }
            }

            //Actualizando paneles requeridos
            uplblPercepciones.Update();
            uplblSueldo.Update();
            uplblAnticipos.Update();
            uplblDeducciones.Update();
            uplblComprobaciones.Update();
            uplblDescuentos.Update();
            uplblAlcance.Update();
        }
        /// <summary>
        /// Método encargado de Cargar los Pagos de la Liquidación que no Pertenecen a un Servicio y/o Movimiento
        /// </summary>
        /// <param name="id_liquidacion">Liquidacion Actual</param>
        private void cargaPagosGeneralLiquidacion(int id_liquidacion)
        {
            //Obteniendo Pagos
            using(DataTable dtPagosGenerales = SAT_CL.Liquidacion.Pago.ObtienePagosLiquidacion(id_liquidacion))
            {
                /*** Pagos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtPagosGenerales))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvPagos, dtPagosGenerales, "IdPago", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtPagosGenerales.Copy(), "Table3");
                    //Mostrando Totales
                    gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
                    upgvPagos.Update();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPagos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                    //Mostrando Sumatoria de Totales
                    gvPagos.FooterRow.Cells[5].Text = "0.00";
                }
            }

            //Inicializando Indices
            TSDK.ASP.Controles.InicializaIndices(gvViajes);
            TSDK.ASP.Controles.InicializaIndices(gvMovimientos);

            //Inicializando Grids
            TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
            TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
            TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);
            TSDK.ASP.Controles.InicializaGridview(gvDiesel);
            
            //Actualizando Controles
            upgvViajes.Update();
            upgvMovimientos.Update();
            upgvAnticipos.Update();
            upgvCobroRecurrenteLiquidacion.Update();
            upgvDiesel.Update();
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Movimientos dado un Viaje
        /// </summary>
        /// <param name="id_viaje">Viaje Seleccionado</param>
        /// <param name="id_liquidacion">Liquidacion Actual</param>
        private void cargaMovimientosPagosViajes(int id_viaje, int id_liquidacion, int id_entidad, byte id_tipo_entidad, byte id_estatus_liquidacion)
        {   
            //Obteniendo Movimientos por Viaje
            using (DataSet ds = SAT_CL.Despacho.Reporte.ObtieneMovimientosYPagosPorViaje(id_viaje, id_liquidacion, id_entidad, id_tipo_entidad, id_estatus_liquidacion, true))
            {   
                /*** Movimientos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table"))
                {   
                    //Cargando Movimientos
                    TSDK.ASP.Controles.CargaGridView(gvMovimientos, ds.Tables["Table"], "Id-NoPago-NoSegmento", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table"].Copy(), "Table2");
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvMovimientos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");
                }
                /*** Pagos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                {   
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvPagos, ds.Tables["Table1"], "IdPago", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table1"].Copy(), "Table3");
                    //Mostrando Totales
                    gvPagos.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table3"].Compute("SUM(Total)", "")).ToString();
                    upgvPagos.Update();
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvPagos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table3");
                    //Mostrando Sumatoria de Totales
                    gvPagos.FooterRow.Cells[5].Text = "0.00";
                }
                /*** Anticipos ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table2"))
                {   
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvAnticipos, ds.Tables["Table2"], "Id", "", true, 1);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table2"].Copy(), "Table4");
                    //Mostrando Totales
                    gvAnticipos.FooterRow.Cells[6].Text = (((DataSet)Session["DS"]).Tables["Table4"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvAnticipos);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table4");
                    //Mostrando Sumatoria de Totales
                    gvAnticipos.FooterRow.Cells[6].Text = "0.00";
                }
                /*** Comprobaciones ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table3"))
                {   
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvComprobaciones, ds.Tables["Table3"], "Id", "", true, 2);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table3"].Copy(), "Table5");
                    //Mostrando Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = (((DataSet)Session["DS"]).Tables["Table5"].Compute("SUM(Monto)", "")).ToString();
                }
                else
                {   
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvComprobaciones);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table5");
                    //Mostrando Sumatoria de Totales
                    gvComprobaciones.FooterRow.Cells[4].Text = "0.00";
                }
                /*** Diesel ***/
                //Validando que existen Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table4"))
                {
                    //Cargando Pagos
                    TSDK.ASP.Controles.CargaGridView(gvDiesel, ds.Tables["Table4"], "Id", "", true, 2);
                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], ds.Tables["Table4"].Copy(), "Table7");
                    //Mostrando Totales
                    gvDiesel.FooterRow.Cells[5].Text = (((DataSet)Session["DS"]).Tables["Table7"].Compute("SUM(Total)", "")).ToString();
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvDiesel);
                    //Eliminando Tabla de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table7");
                    //Mostrando Sumatoria de Totales
                    gvDiesel.FooterRow.Cells[5].Text = "0.00";
                }
                
                //Deshabilita los Controles de Captura y Edicion de Pagos
                limpiaControlesPagos();
                habilitaControlesPagos(false);
                
                //Inicializando Controles
                TSDK.ASP.Controles.InicializaIndices(gvMovimientos);
                TSDK.ASP.Controles.InicializaIndices(gvPagos);
                TSDK.ASP.Controles.InicializaIndices(gvAnticipos);
                
                //Quitando Seleccion de las Filas
                TSDK.ASP.Controles.SeleccionaFilasTodas(gvMovimientos, "chkVarios", false);
            }

            //Instanciando liquidación
            using (SAT_CL.Liquidacion.Liquidacion l = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
                //Actualziando información de liquidación
                cargaValoresTotalesLiquidacion(id_liquidacion, l.tipo_asignacion, l.id_unidad, l.id_operador, l.id_proveedor, l.id_compania_emisora);
        }
        /// <summary>
        /// Método privado encargado de Habilitar los Controles según el Estatus de la Liquidación
        /// </summary>
        /// <param name="estatus">Estatus de la Liquidación</param>
        private void habilitaControlesLiquidacion(SAT_CL.Liquidacion.Liquidacion.Estatus estatus)
        {
            //Validando el Estatus de la Liquidación
            switch(estatus)
            {
                case SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado:

                case SAT_CL.Liquidacion.Liquidacion.Estatus.Depositado:

                case SAT_CL.Liquidacion.Liquidacion.Estatus.Transferido:
                    {
                        //Deshabilitando Controles
                        gvPagos.Enabled =
                        gvAnticipos.Enabled =
                        gvComprobaciones.Enabled = false;

                        break;
                    }
                case SAT_CL.Liquidacion.Liquidacion.Estatus.Registrado:
                    {
                        //Habilitando Controles
                        gvPagos.Enabled =
                        gvAnticipos.Enabled =
                        gvComprobaciones.Enabled = true;

                        break;
                    }
                
            }
        }
        /// <summary>
        /// Método Privado encargado de Validar el Estatus del Movimiento
        /// </summary>
        /// <param name="id_movimiento">Movimiento Por Validar</param>
        /// <returns></returns>
        private bool validaMovimientoTerminado(int id_movimiento)
        {
            //Declarando Objeto de Retorno
            bool result = false;

            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Obteniendo Entidad
                int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;
                MovimientoAsignacionRecurso.Tipo tipo_asignacion = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? MovimientoAsignacionRecurso.Tipo.Operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? MovimientoAsignacionRecurso.Tipo.Unidad : MovimientoAsignacionRecurso.Tipo.Tercero;

                //Instanciando Movimiento
                using (SAT_CL.Despacho.Movimiento mov = new Movimiento(id_movimiento))
                {
                    //Validando que exista el Movimiento
                    if (mov.id_movimiento > 0)
                    {
                        //Instanciando Asignación del Recurso
                        using (SAT_CL.Despacho.MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(mov.id_movimiento, MovimientoAsignacionRecurso.Estatus.Terminado, tipo_asignacion, id_entidad))
                        {
                            //Validando que exista la Asignación
                            if (mar.id_movimiento_asignacion_recurso > 0)

                                //Asignando Positiva la Validación
                                result = true;
                            else
                                //Asignando Negativa la Validación
                                result = false;
                        }
                    }
                }
            }

            //Devovliendo Resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Cerrar la Liquidación
        /// </summary>
        private void cierraLiquidacion()
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
            {
                //Declarando Objeto de Retorno
                RetornoOperacion result = new RetornoOperacion();
                
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;
                    
                    //Validando que no existan Depositos Pendientes
                    if (!SAT_CL.EgresoServicio.DetalleLiquidacion.ValidaDepositosPendientesLiquidacion(liq.id_liquidacion, liq.id_estatus, liq.id_tipo_asignacion, id_entidad))
                    {
                        //Obteniendo Montos Totales
                        using (DataTable dtTotales = SAT_CL.Liquidacion.Liquidacion.ObtieneMontosTotalesLiquidacion(liq.id_liquidacion, liq.tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora))
                        {
                            //Declarando Variable Auxiliares
                            decimal t_salario = 0.00M, t_deducciones = 0.00M, t_sueldo = 0.00M, t_anticipos = 0.00M,
                                    t_comprobaciones = 0.00M, t_descuentos = 0.00M, t_alcanze = 0.00M;

                            //Validando que existan Valores
                            if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTotales))
                            {
                                //Recorriendo Ciclo
                                foreach (DataRow dr in dtTotales.Rows)
                                {
                                    //Asignando Valores
                                    t_salario = Convert.ToDecimal(dr["TPercepcion"]);
                                    t_deducciones = Convert.ToDecimal(dr["TDeducciones"]);
                                    t_sueldo = Convert.ToDecimal(dr["TSueldo"]);
                                    t_anticipos = Convert.ToDecimal(dr["TAnticipos"]);
                                    t_comprobaciones = Convert.ToDecimal(dr["TComprobaciones"]);
                                    t_descuentos = Convert.ToDecimal(dr["TDescuentos"]);
                                    t_alcanze = Convert.ToDecimal(dr["TAlcance"]);
                                }
                            }

                            //Cerrando la Liquidación
                            result = liq.CierraLiquidacion(t_salario, t_deducciones, t_sueldo, t_anticipos, t_comprobaciones, t_descuentos, t_alcanze, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Validando que la Liquidación se haya Cerrado
                            if (result.OperacionExitosa)

                                //Inicializando Valores de la Liquidación
                                inicializaValoresLiquidacion(liq.id_liquidacion);
                        }
                    }
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("Existen depositos pendientes, Imposible cerrar la Liquidación");
                }
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe la Liquidación");

                //Mostrando Mensaje de la Operación
                lblErrorLiquidacion.Text = result.Mensaje;
            }
        }
        /// <summary>
        /// Método Privado encargado de Actualizar los registros ligados a los Movimientos y a los Detalles de Liquidación
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <returns></returns>
        private RetornoOperacion actualizaMovimientosDetalle(int id_liquidacion)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
            {
                //Declarando Objeto de Retorno
                RetornoOperacion resultBD = new RetornoOperacion();

                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;
                    MovimientoAsignacionRecurso.Tipo tipo_asignacion = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? MovimientoAsignacionRecurso.Tipo.Operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? MovimientoAsignacionRecurso.Tipo.Unidad : MovimientoAsignacionRecurso.Tipo.Tercero;

                    //Obteniendo Movimientos
                    using (DataSet ds = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneMovimientosYDetallesPorLiquidacion(id_liquidacion, DetalleLiquidacion.Estatus.Registrado, id_entidad, (byte)tipo_asignacion))
                    {
                        //Validando que existan Movimientos
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table") && TSDK.Datos.Validacion.ValidaOrigenDatos(ds, "Table1"))
                        {
                            //Recorriendo Movimientos
                            foreach (DataRow dr in ds.Tables["Table"].Rows)
                            {
                                //Validando que exista un Movimiento
                                if (Convert.ToInt32(dr["IdMovimiento"]) > 0)
                                {
                                    //Obteniendo Asignacion de Recursos
                                    using (MovimientoAsignacionRecurso mar = new MovimientoAsignacionRecurso(Convert.ToInt32(dr["IdMovimiento"]), MovimientoAsignacionRecurso.Estatus.Terminado, tipo_asignacion, id_entidad))
                                    {
                                        //Validando que existe la Asignación del Recurso
                                        if (mar.id_movimiento_asignacion_recurso > 0)
                                        {
                                            //Actualizando Estatus a "Liquidado"
                                            resultBD = mar.ActualizaEstatusMovimientoAsignacionRecurso(MovimientoAsignacionRecurso.Estatus.Liquidado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la operación haya sido Exitosa
                                            if (resultBD.OperacionExitosa)
                                            {
                                                //Actualizando Diesel
                                                resultBD = actualizaDiesel(Convert.ToInt32(dr["IdMovimiento"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.fecha_liquidacion);

                                                //Validando que la operación haya sido Exitosa
                                                if (!resultBD.OperacionExitosa)
                                                    //Finalizando Ciclo
                                                    break;
                                            }
                                            else
                                                //Finalizando Ciclo
                                                break;
                                        }
                                        else
                                            //Finalizando Ciclo
                                            break;
                                    }
                                }
                            }

                            //Recorriendo Detalles de Liquidación
                            foreach (DataRow dr in ds.Tables["Table1"].Rows)
                            {
                                //Instanciando Detalle de Liquidación
                                using (DetalleLiquidacion dl = new DetalleLiquidacion(Convert.ToInt32(dr["Id"])))
                                {
                                    //Validando que exista el Detalle de Liquidación
                                    if (dl.id_detalle_liquidacion > 0)
                                    {
                                        //Liquida Detalle
                                        resultBD = dl.LiquidaDetalle(liq.id_liquidacion, liq.fecha_liquidacion, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando que se haya Realizado la Operación exitosamente
                                        if (!resultBD.OperacionExitosa)
                                            //Terminando Ciclo
                                            break;
                                    }
                                    else
                                        //Terminando Ciclo
                                        break;
                                }
                            }
                        }
                        else
                            //Instanciando Excepcion
                            resultBD = new RetornoOperacion("No existen Movimientos");
                    }
                }
                //Devolviendo Resultado Obtenido
                return resultBD;
            }
        }
        /// <summary>
        /// Método Privado encargado de Actualizar las Asignaciones de Diesel
        /// </summary>
        /// <param name="id_movimiento"></param>
        /// <param name="id_liquidacion"></param>
        /// <param name="id_entidad"></param>
        /// <param name="id_tipo_entidad"></param>
        private RetornoOperacion actualizaDiesel(int id_movimiento, int id_liquidacion, int id_entidad, byte id_tipo_entidad, DateTime fec_liq)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Asignación de Diesel
            using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(id_movimiento, id_liquidacion, id_entidad, id_tipo_entidad))
            {
                //Validando que existan Registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                {
                    //Recorriendo Ciclo
                    foreach (DataRow dr in dtDiesel.Rows)
                    {
                        //Instanciando Vale de Diesel
                        using (SAT_CL.EgresoServicio.AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(dr["Id"])))
                        {
                            //Validando que exista el Vale
                            if (ad.id_asignacion_diesel > 0)
                            {
                                //Liquidando Vale
                                result = ad.LiquidaValeDiesel(id_liquidacion, fec_liq, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando la Operación
                                if (!result.OperacionExitosa)
                                    //Terminando Ciclo
                                    break;
                            }
                            else
                            {
                                //Instanciando Excepción
                                result = new RetornoOperacion("Vale inaccesible, Imposible su Liquidación");
                                //Terminando Ciclo
                                break;
                            }
                        }
                    }
                }
                else
                    //Instanciando Excepcion
                    result = new RetornoOperacion(1, "No existen Vales de Diesel", true);
            }

            //Devolviendo Objeto de Retorno
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Cargar la Tira de Imagenes
        /// </summary>
        /// <param name="id_servicio">Servicio</param>
        private void cargaImagenesDetalle(int id_servicio)
        {
            //Realizando la carga de URL de imagenes a mostrar
            using (DataTable mit = SAT_CL.ControlEvidencia.ControlEvidenciaDocumento.ObtieneControlEvidenciaDocumentosImagenes(id_servicio, Convert.ToInt32(gvMovimientos.SelectedDataKey["NoSegmento"])))
            {
                //Validando que existan imagenes a mostrar
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(mit))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvEvidencias, mit, "Id", "");

                    //Añadiendo Tabla 
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], mit, "Table12");
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvEvidencias);

                    //Eliminando Tabla 
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table12");
                }   
                    
            }
        }

        #region Métodos "Pagos"

        /// <summary>
        /// Método Privado encargado de Habilitar los Controles de Captura del Pago
        /// </summary>
        /// <param name="enabled"></param>
        private void habilitaControlesPagos(bool enabled)
        {   
            //Asignando habilitación o Deshabilitación de Controles
            ddlTipoPago.Enabled =
            txtCantidad.Enabled =
            txtValorU.Enabled =
            //txtTotal.Enabled = 
            //txtDescripcion.Enabled =
            txtReferencia.Enabled =
            btnGuardarPago.Enabled =
            btnCancelarPago.Enabled = enabled;
        }
        /// <summary>
        /// Método Privado encargado de Limpiar los datos de los Controles de Pagos
        /// </summary>
        private void limpiaControlesPagos()
        {   
            //Asignando habilitación o Deshabilitación de Controles
            txtCantidad.Text =
            txtValorU.Text =
            txtTotal.Text = "0";
            txtDescripcion.Text =
            txtReferencia.Text = 
            lblErrorPago.Text = "";
        }
        /// <summary>
        /// Obtiene los Dias Pagados
        /// </summary>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="tipo_entidad">Tipo de Entidad</param>
        /// <param name="fecha_liquidacion">Fecha de la Liquidación</param>
        /// <param name="fecha_ultimo_viaje">Fecha del Ultimo Viaje</param>
        /// <returns></returns>
        private TimeSpan ObtieneDiasPagados(int id_entidad, int tipo_entidad, DateTime fecha_liquidacion, out DateTime fecha_ultimo_viaje)
        {   
            //Declarando Objeto de Retorno
            TimeSpan dias_pagados = new TimeSpan(0, 0, 0);
            fecha_ultimo_viaje = DateTime.MinValue;
            
            //Obteniendo la Ultima Fecha
            DateTime fecha_ultima_liq = SAT_CL.Liquidacion.Liquidacion.ObtieneUltimaFechaLiquidacion(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                (SAT_CL.Liquidacion.Liquidacion.TipoAsignacion)tipo_entidad, id_entidad);
            
            //Validando que exista una Fecha de Ultima Liquidación
            if (fecha_ultima_liq != DateTime.MinValue)
                //Obtiene Dias pagados
                dias_pagados = fecha_liquidacion - fecha_ultima_liq;
            else
            {   
                //Obteniendo Primer Viaje
                DateTime fecha_primer_viaje = SAT_CL.Despacho.Reporte.ObtieneFechaPrimerViajeAsignado(id_entidad, tipo_entidad);
                //Validando que exista una Fecha del Primer Viaje
                if (fecha_primer_viaje != DateTime.MinValue)
                    //Obtiene Dias pagados
                    dias_pagados = fecha_liquidacion - fecha_primer_viaje;
            }
            
            //Obtiene Fecha del Ultimo Viaje Asignado
            fecha_ultimo_viaje = SAT_CL.Despacho.Reporte.ObtieneFechaUltimoViajeAsignado(id_entidad, tipo_entidad);
            //Devolviendo Dias Pagados
            return dias_pagados;
        }
        /// <summary>
        /// Método Privado encargado de Crear los Pagos por Movimiento
        /// </summary>
        /// <returns></returns>
        private RetornoOperacion CreaPagoPorMovimientos(int id_movimiento, int id_pago)
        {   
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            //Instanciando el Movimiento
            using (SAT_CL.Despacho.Movimiento mov = new SAT_CL.Despacho.Movimiento(id_movimiento))
            {   
                //Declarando Objeto de Retorno
                if (mov.id_movimiento > 0)
                    //Insertando Pago Movimiento
                    result = SAT_CL.Liquidacion.PagoMovimiento.InsertaPagoMovimiento(id_pago, mov.id_movimiento, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                else
                    //Instanciando Excepción
                    result = new RetornoOperacion("No Existe el Movimiento");
            }
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Actualizar la Liquidación de los Depositos
        /// </summary>
        /// <param name="id_movimiento">Movimiento de Referencia de los Depositos y Vales</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación Actual</param>
        /// <param name="estatus">Estatus por Actualizar</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_app">Tipo de la Entidad de la Liquidación</param>
        /// <param name="actualiza_liquidacion">Valor que indica si los Depositos y/o Vales se van a Referenciar o Regresar</param>
        /// <returns></returns>
        private RetornoOperacion actualizaLiquidacionDepositosVales(int id_movimiento, int id_liquidacion, int id_entidad, byte id_tipo_app, bool actualiza_liquidacion)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(id_movimiento);

            //Obteniendo Depositos
            using (DataTable dtDepositos = Deposito.CargaDepositosMovimiento(id_movimiento, id_entidad, id_tipo_app))
            {
                //Validando que existan los registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDepositos))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtDepositos.Rows)
                    {
                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(dr["Id"])))
                        {
                            //Actualizando Liquidación del Deposito
                            result = dep.ActualizaLiquidacionDeposito(actualiza_liquidacion ? id_liquidacion : 0, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Si la Operación fue incorrecta
                            if (!result.OperacionExitosa)
                            {
                                result = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", dep.no_deposito, result.Mensaje));
                                //Se termina el ciclo
                                break;
                            }
                        }
                    }
                }
            }

            //Si no hay errores
            if (result.OperacionExitosa)
            {
                //Obteniendo Asignación de Diesel
                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselMovimiento(id_movimiento, id_liquidacion, id_entidad, id_tipo_app))
                {
                    //Validando que existan los registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtDiesel.Rows)
                        {
                            //Instanciando Deposito
                            using (AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = ad.ActualizaLiquidacionVale(actualiza_liquidacion ? id_liquidacion : 0, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                {
                                    result = new RetornoOperacion(string.Format("Error al actualizar vale de diesel '{0}': {1}", ad.no_vale, result.Mensaje));
                                    //Se termina el ciclo
                                    break;
                                }
                            }
                        }
                    }
                    else
                        //Instanciando Depositos
                        result = new RetornoOperacion(1);
                }
            }

            //Validando que la Liquidacion se vaya a actualizar
            if (!actualiza_liquidacion && result.OperacionExitosa)
            {
                //Obteniendo Comprobaciones
                using (DataTable dtComprobaciones = SAT_CL.Liquidacion.Comprobacion.ObtieneComprobacionesMovimiento(id_movimiento, id_liquidacion))
                {
                    //Validando que existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtComprobaciones))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtComprobaciones.Rows)
                        {
                            //Instanciando Deposito
                            using (Comprobacion cmp = new Comprobacion(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = cmp.DeshabilitaComprobacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                    break;
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }

        /// <summary>
        /// Método Privado encargado de Actualizar la Liquidación de los Depositos
        /// </summary>
        /// <param name="id_servicio">Servicio de Referencia de los Depositos y Vales</param>
        /// <param name="id_liquidacion">Referencia a la Liquidación Actual</param>
        /// <param name="estatus">Estatus por Actualizar</param>
        /// <param name="id_entidad">Entidad de la Liquidación</param>
        /// <param name="id_tipo_app">Tipo de la Entidad de la Liquidación</param>
        /// <returns></returns>
        private RetornoOperacion actualizaLiquidacionDepositosValesServicio(int id_servicio, int id_liquidacion, int id_entidad, byte id_tipo_app)
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion(id_servicio);

            //Obteniendo Depositos
            using (DataTable dtDepositos = Deposito.CargaDepositosServicio(id_servicio, id_entidad, id_tipo_app))
            {
                //Validando que existan los registros
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDepositos))
                {
                    //Recorriendo Registros
                    foreach (DataRow dr in dtDepositos.Rows)
                    {
                        //Instanciando Deposito
                        using (Deposito dep = new Deposito(Convert.ToInt32(dr["Id"])))
                        {
                            //Actualizando Liquidación del Deposito
                            result = dep.ActualizaLiquidacionDeposito(id_liquidacion, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                            //Si la Operación fue incorrecta
                            if (!result.OperacionExitosa)
                            {
                                result = new RetornoOperacion(string.Format("Error al actualizar depósito '{0}': {1}", dep.no_deposito, result.Mensaje));
                                //Se termina el ciclo
                                break;
                            }
                        }
                    }
                }
            }

            //Si no hay errores
            if (result.OperacionExitosa)
            {
                //Obteniendo Asignación de Diesel
                using (DataTable dtDiesel = SAT_CL.EgresoServicio.AsignacionDiesel.ObtieneAsignacionesDieselServicio(id_servicio, id_liquidacion, id_entidad, id_tipo_app))
                {
                    //Validando que existan los registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtDiesel))
                    {
                        //Recorriendo Registros
                        foreach (DataRow dr in dtDiesel.Rows)
                        {
                            //Instanciando Deposito
                            using (AsignacionDiesel ad = new AsignacionDiesel(Convert.ToInt32(dr["Id"])))
                            {
                                //Actualizando Liquidación del Deposito
                                result = ad.ActualizaLiquidacionVale(id_liquidacion, DetalleLiquidacion.Estatus.Registrado, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si la Operación fue incorrecta
                                if (!result.OperacionExitosa)
                                {
                                    result = new RetornoOperacion(string.Format("Error al actualizar vale de diesel '{0}': {1}", ad.no_vale, result.Mensaje));
                                    //Se termina el ciclo
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //Devolviendo resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método que permite limpiar los controles del panel de busqueda.
        /// </summary>
        private void limpiaBusquedaLiquidacion()
        {
            //Limpia la caja de texto Entidad y el lblErrorBusqueda.
            txtEntidad.Text = "";
            lblErrorBusqueda.Text = "";
            //Carga el catalogo del dropdownlist tipo entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTipoEnt, "", 62);
            //Inicializando Controles
            TSDK.ASP.Controles.InicializaGridview(gvLiquidacion);
        }
        #endregion

        #region Métodos "Comprobaciones"

        /// <summary>
        /// Método Privado encargado de Inicializar el Control de Comprobaciones
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobacion</param>
        /// <param name="id_deposito">Referencia del Deposito</param>
        private void inicializaControlComprobaciones(int id_comprobacion, int id_deposito)
        {
            //Instanciando Comprobación
            using (Comprobacion cmp = new Comprobacion(id_comprobacion))
            
                //Instanciando Deposito
            using(Deposito dep = new Deposito(id_deposito))
            {
                //Validar que exista la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = cmp.id_comprobacion.ToString();
                    ddlConcepto.SelectedValue = cmp.id_concepto_comprobacion.ToString();
                    txtObservacion.Text = cmp.observacion_comprobacion;
                    txtValorUnitario.Text = cmp.objDetalleComprobacion.monto.ToString();
                }
                else
                {
                    //Asignando Valores
                    lblIdComprobacion.Text = "Por Asignar";
                    txtObservacion.Text = "";

                    //Validando que exista el Depsoito
                    if (dep.id_deposito > 0)
                    {
                        //Concatenando Valor al Control TextBox
                        txtValorUnitario.Text = dep.objDetalleLiquidacion.monto.ToString();
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 39, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", dep.id_concepto, "");
                    }
                    else
                    {   //Limpiando Control
                        txtValorUnitario.Text = "";
                        //Cargando Catalogo
                        SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 38, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
                    }
                    //Habilitando Control
                    txtValorUnitario.Enabled = true;
                }

                //Cargando Facturas
                cargaFacturasComprobacion(id_comprobacion);

                //Limpiando Mensaje
                lblErrorComprobacion.Text = "";
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar las Facturas de la Comprobación
        /// </summary>
        /// <param name="id_comprobacion">Referencia de la Comprobación</param>
        private void cargaFacturasComprobacion(int id_comprobacion)
        {
            //Obteniendo Facturas
            using (DataTable dtFacturas = FacturadoProveedorRelacion.ObtieneFacturasComprobacion(id_comprobacion))
            {
                //Validando que existan registros
                if(TSDK.Datos.Validacion.ValidaOrigenDatos(dtFacturas))
                {
                    //Cargando GridView
                    TSDK.ASP.Controles.CargaGridView(gvFacturasComprobacion, dtFacturas, "IdFactura-IdFacturaRelacion", "", true, 2);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFacturas, "Table6");

                    //Deshabilitando Control
                    txtValorUnitario.Enabled = false;
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvFacturasComprobacion);

                    //Añadiendo Tablas a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table6");
                }

                //Inicializando Indices
                TSDK.ASP.Controles.InicializaIndices(gvFacturasComprobacion);
            }
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de los Conceptos de la Factura
        /// </summary>
        /// <param name="cfdi"></param>
        /// <param name="concepto"></param>
        /// <param name="tasa_imp_tras"></param>
        /// <param name="tasa_imp_ret"></param>
        /// <param name="imp_ret"></param>
        /// <param name="imp_tras"></param>
        private void obtieneCantidadesConcepto(XmlDocument cfdi, XmlNode concepto, out decimal tasa_imp_tras, out decimal tasa_imp_ret)
        {   
            //Validación de Retenciones
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
            {   
                //Asignando Valores
                tasa_imp_ret = (Convert.ToDecimal(cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"]["cfdi:Retencion"].Attributes["importe"].Value) / Convert.ToDecimal(concepto.Attributes["importe"].Value)) * 100;
            }
            else
                //Asignando Valores
                tasa_imp_ret = 0;
            //Validación de Traslados
            if (cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
            {
                //Obteniendo Valor de la Tasa
                string tasaImpTrasladado = cfdi.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"]["cfdi:Traslado"].Attributes["tasa"].Value;

                //Remplazando Puntos Decimales
                tasaImpTrasladado = tasaImpTrasladado.Replace(".", "|");

                //Validando que exista un valor despues del Punto decimal
                if (Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1)) > 0.00M)

                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 1));
                else
                    //Asignando Valores
                    tasa_imp_tras = Convert.ToDecimal(Cadena.RegresaCadenaSeparada(tasaImpTrasladado, "|", 0));
            }
            else
                //Asignando Valores
                tasa_imp_tras = 0;
        }
        /// <summary>
        /// Método Privado encargado de Obtener las Cantidades de la Factura
        /// </summary>
        /// <param name="document">Factura XML</param>
        /// <param name="total_p">Total en Pesos</param>
        /// <param name="subtotal_p">Subtotal en Pesos</param>
        /// <param name="descuento_p">Descuento en Pesos</param>
        /// <param name="traslado_p">Importe Trasladado en Pesos</param>
        /// <param name="retenido_p">Importe Retenido en Pesos</param>
        /// <param name="monto_tc">Monto del Tipo de Cambio</param>
        private void obtieneCantidades(XmlDocument document, out decimal total_p, out decimal subtotal_p, out decimal descuento_p, out decimal traslado_p, out decimal retenido_p, out decimal monto_tc)
        {   
            //Validando si existe el Tipo de Cambio
            if (document.DocumentElement.Attributes["TipoCambio"] == null)
            {   
                //Asignando Atributo Obligatorios
                monto_tc = 1;
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value);
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value);
                traslado_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0;
                retenido_p = document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0;
                
                //Asignando Atributos Opcionales
                descuento_p = document.DocumentElement.Attributes["descuento"] == null ? 0 :
                    Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value);
            }
            else
            {   
                //Asignando Atributo Obligatorios
                monto_tc = Convert.ToDecimal(document.DocumentElement.Attributes["TipoCambio"].Value);
                total_p = Convert.ToDecimal(document.DocumentElement.Attributes["total"].Value) * monto_tc;
                subtotal_p = Convert.ToDecimal(document.DocumentElement.Attributes["subTotal"].Value) * monto_tc;
                traslado_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value) : 0) * monto_tc;
                retenido_p = (document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] != null ?
                    Convert.ToDecimal(document.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value) : 0) * monto_tc;
                
                //Asignando Atributos Opcionales
                descuento_p = (document.DocumentElement.Attributes["descuento"] == null ? 0 : Convert.ToDecimal(document.DocumentElement.Attributes["descuento"].Value)) * monto_tc;
            }
        }
        /// <summary>
        /// Método privado encargado de Validar la Factura en formato XML
        /// </summary>
        /// <param name="mensaje">Mensaje de Operación</param>
        /// <returns></returns>
        private bool validaFacturaXML(out string mensaje)
        {   
            //Declarando Objeto de Retorno
            bool result = false;
            
            //Limpiando Mensaje
            mensaje = "";
            
            //Validando que exista un Archivo en Sessión
            if (Session["XML"] != null)
            {   
                //Declarando Documento XML
                XmlDocument doc = (XmlDocument)Session["XML"];
                
                //Validando que exista el Documento
                if (doc != null)
                {
                    try
                    {   //Instanciando Compania
                        using (SAT_CL.Global.CompaniaEmisorReceptor emi = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                        {
                            //Declarando Variable Auxiliar
                            int idEmisor = 0;
                            
                            //Validando que exista la Compania
                            if (emi.id_compania_emisor_receptor > 0)

                                //Asignando Emisor
                                idEmisor = emi.id_compania_emisor_receptor;
                            
                            //Si no existe
                            else
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion resultado = new RetornoOperacion();
                                
                                //Insertando Compania
                                resultado = CompaniaEmisorReceptor.InsertaCompaniaEmisorRecepto("", doc.DocumentElement["cfdi:Emisor"].Attributes["rfc"].Value.ToUpper(), doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(),
                                                                            doc.DocumentElement["cfdi:Emisor"].Attributes["nombre"].Value.ToUpper(), 0, false, false, true, 0, "", "", "", 0, 0, 0, 0,
                                                                            "FACTURAS DE PROVEEDOR", "", 0, 0, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                //Asignando Registro
                                idEmisor = resultado.IdRegistro;
                            }
                            
                            //Validando que el RFC sea igual
                            if (idEmisor > 0)
                            {
                                //Instanciando Compania
                                using (SAT_CL.Global.CompaniaEmisorReceptor cer = new SAT_CL.Global.CompaniaEmisorReceptor(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                {   
                                    //Validando que el RFC sea igual
                                    if (cer.rfc.ToUpper() == doc.DocumentElement["cfdi:Receptor"].Attributes["rfc"].Value.ToUpper())
                                    {   
                                        //Instanciando XSD de validación
                                        using (EsquemasFacturacion ef = new EsquemasFacturacion(doc["cfdi:Comprobante"].Attributes["version"].Value))
                                        {   
                                            //Validando que exista el XSD
                                            if (ef.id_esquema_facturacion != 0)
                                            {   
                                                //Declarando variables Auxiliares
                                                bool addenda;
                                                
                                                //Obteniendo XSD
                                                string[] esquemas = EsquemasFacturacion.CargaEsquemasPadreYComplementosCFDI(ef.version, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, out addenda);

                                                //Validando que Exista una Addenda
                                                if (doc.DocumentElement["cfdi:Addenda"] != null)

                                                    //Removiendo Addendas
                                                    doc.DocumentElement["cfdi:Addenda"].RemoveAll();

                                                //Obteniendo Validación
                                                result = TSDK.Base.Xml.ValidaXMLSchema(doc.InnerXml, esquemas, out mensaje);
                                            }
                                        }
                                    }
                                    else
                                    {   
                                        //Mostrando el Mensaje
                                        mensaje = "El RFC de la factura no coincide con el Receptor";
                                        
                                        //Asignando Negativa el Objeto de retorno
                                        result = false;
                                    }
                                }
                            }
                            else
                            {   
                                //Mostrando el Mensaje
                                mensaje = "El RFC de la factura no coincide con el Emisor";
                                
                                //Asignando Negativa el Objeto de retorno
                                result = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //Mostrando Mensaje
                        mensaje = e.Message;
                    }
                }
                else//Mensaje de Error
                    mensaje = "No se ha podido cargar el Archivo";
            }
            else//Mensaje de Error
                mensaje = "No se ha podido localizar el Archivo";
            
            //Devolviendo Resultado Obtenido
            return result;
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Factura en XML
        /// </summary>
        private void guardaFacturaXML()
        {
            //Declarando Objeto de Operación
            RetornoOperacion resultado = new RetornoOperacion();
            
            //Declarando Objeto de Mensaje
            string mensaje = "";
            
            //Declarando Variable para Factura
            int idFactura = 0;

            //Instanciando Comprobacion
            using (SAT_CL.Liquidacion.Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text == "Por Asignar" ? "0" : lblIdComprobacion.Text)))
            {
                //Validando la Comprobación
                if (cmp.id_comprobacion > 0)
                {
                    //Validando que el Movimiento este Terminado
                    if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))
                    {
                        //Inicializando transacción
                        using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                        {
                            //Declarando Documento XML
                            XmlDocument doc = (XmlDocument)Session["XML"];

                            //Validando que exista una Factura
                            if (doc != null)
                            {
                                //Recuperando tabla temporal
                                if (validaFacturaXML(out mensaje))
                                {
                                    //Declarando variables de Montos
                                    decimal total_p, subtotal_p, descuento_p, traslado_p, retenido_p, monto_tc;

                                    //Obteniendo Valores
                                    obtieneCantidades(doc, out total_p, out subtotal_p, out descuento_p, out traslado_p, out retenido_p, out monto_tc);

                                    //Instanciando Emisor-Compania
                                    using (SAT_CL.Global.CompaniaEmisorReceptor emisor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Emisor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                    {
                                        //Validando que Exista el Emisor
                                        if (emisor.id_compania_emisor_receptor != 0)
                                        {
                                            //Instanciando Emisor-Compania
                                            using (SAT_CL.Global.CompaniaEmisorReceptor receptor = SAT_CL.Global.CompaniaEmisorReceptor.ObtieneInstanciaCompaniaRFC(doc["cfdi:Comprobante"]["cfdi:Receptor"].Attributes["rfc"].Value, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor))
                                            {
                                                //Validando que coincida el RFC
                                                if (receptor.id_compania_emisor_receptor == ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor)
                                                {
                                                    //Declarando Variables Auxiliares
                                                    decimal totalImpTrasladados = 0.00M, totalImpRetenidos = 0.00M;

                                                    /** Retenciones **/
                                                    //Validando que no exista el Nodo
                                                    if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"] == null)
                                                    {
                                                        //Validando que existan Retenciones
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"] != null)
                                                        {
                                                            //Validando que existan Nodos
                                                            if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"].ChildNodes.Count > 0)
                                                            {
                                                                //Recorriendo Retenciones
                                                                foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Retenciones"])
                                                                {
                                                                    //Sumando Impuestos Retenidos
                                                                    totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Asignando Total de Impuestos
                                                        totalImpRetenidos = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosRetenidos"].Value);


                                                    /** Traslados **/
                                                    //Validando que no exista el Nodo
                                                    if (doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"] == null)
                                                    {
                                                        //Validando que existan Traslados
                                                        if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"] != null)
                                                        {
                                                            //Validando que existan Nodos
                                                            if (doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"].ChildNodes.Count > 0)
                                                            {
                                                                //Recorriendo Traslados
                                                                foreach (XmlNode node in doc.DocumentElement["cfdi:Impuestos"]["cfdi:Traslados"])
                                                                {
                                                                    //Sumando Impuestos Trasladados
                                                                    totalImpRetenidos += Convert.ToDecimal(node.Attributes["importe"].Value);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                        //Asignando Total de Impuestos
                                                        totalImpTrasladados = Convert.ToDecimal(doc.DocumentElement["cfdi:Impuestos"].Attributes["totalImpuestosTrasladados"].Value);
                                                    
                                                    //Insertando factura
                                                    resultado = FacturadoProveedor.InsertaFacturadoProveedor(emisor.id_compania_emisor_receptor, receptor.id_compania_emisor_receptor,
                                                                                        Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), doc["cfdi:Comprobante"].Attributes["serie"] == null ? "" : doc["cfdi:Comprobante"].Attributes["serie"].Value,
                                                                                        doc["cfdi:Comprobante"].Attributes["folio"] == null ? "" : doc["cfdi:Comprobante"].Attributes["folio"].Value,
                                                                                        doc.DocumentElement["cfdi:Complemento"]["tfd:TimbreFiscalDigital"].Attributes["UUID"].Value,
                                                                                        Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), (byte)FacturadoProveedor.TipoComprobante.CFDI,
                                                                                        "I", 1, (byte)FacturadoProveedor.EstatusFactura.EnRevision, (byte)FacturadoProveedor.EstatusRecepion.Recibida,
                                                                                        0, 0, 0, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value), Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["subTotal"].Value),
                                                                                        doc["cfdi:Comprobante"].Attributes["descuentos"] == null ? 0 : Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["descuentos"].Value),
                                                                                        totalImpTrasladados, totalImpRetenidos,
                                                                                        doc["cfdi:Comprobante"].Attributes["Moneda"] == null ? "" : doc["cfdi:Comprobante"].Attributes["Moneda"].Value,
                                                                                        monto_tc, Convert.ToDateTime(doc["cfdi:Comprobante"].Attributes["fecha"].Value), total_p, subtotal_p, descuento_p, traslado_p,
                                                                                        retenido_p, false, DateTime.MinValue, Convert.ToDecimal(doc["cfdi:Comprobante"].Attributes["total"].Value),
                                                                                        doc["cfdi:Comprobante"].Attributes["condicionesDePago"] == null ? "" : doc["cfdi:Comprobante"].Attributes["condicionesDePago"].Value,
                                                                                        emisor.dias_credito, 1, (byte)FacturadoProveedor.EstatusValidacion.ValidacionSintactica, "",
                                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                                }
                                                else
                                                    //Instanciando Excepcion
                                                    resultado = new RetornoOperacion("La Compania Receptora no esta registrada");
                                            }
                                        }
                                        else
                                            //Instanciando Excepcion
                                            resultado = new RetornoOperacion("El Compania Proveedora no esta registrado");
                                    }

                                    //Validando que se inserto la Factura
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Obteniendo Factura
                                        idFactura = resultado.IdRegistro;

                                        //Obteniendo Nodos de Concepto
                                        XmlNodeList xmlNL = doc.GetElementsByTagName("cfdi:Concepto");

                                        //Declarando Variables Auxiliares
                                        decimal tasa_imp_ret, tasa_imp_tras;
                                        bool res = true;
                                        int contador = 0;

                                        //Recorriendo cada 
                                        while (res)
                                        {
                                            //Obteniendo Concepto
                                            XmlNode node = xmlNL[contador];

                                            //Obteniendo Cantidades del Concepto
                                            obtieneCantidadesConcepto(doc, node, out tasa_imp_tras, out tasa_imp_ret);

                                            //Insertando Cocepto de Factura
                                            resultado = FacturadoProveedorConcepto.InsertaFacturaProveedorConcepto(idFactura, Convert.ToDecimal(node.Attributes["cantidad"].Value),
                                                                    node.Attributes["unidad"] == null ? "" : node.Attributes["unidad"].Value, node.Attributes["noIdentificacion"] == null ? "" : node.Attributes["noIdentificacion"].Value,
                                                                    node.Attributes["descripcion"].Value, 0, Convert.ToDecimal(node.Attributes["valorUnitario"] == null ? "1" : node.Attributes["valorUnitario"].Value),
                                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value),
                                                                    Convert.ToDecimal(node.Attributes["importe"] == null ? "1" : node.Attributes["importe"].Value) * monto_tc,
                                                                    tasa_imp_ret, tasa_imp_tras, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Incrementando Contador
                                            contador++;

                                            //Obteniendo resultado del Ciclo
                                            res = contador >= xmlNL.Count ? false : resultado.OperacionExitosa;
                                        }
                                        //Validando resultado
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Declarando Variables Auxiliares
                                            string ruta;

                                            //Compuesta de un path genérico para esta ventana, el Id de Tabla e Id de Registro, más el nombre asignado al archivo (incluyendo extensión)
                                            ruta = string.Format(@"{0}{1}\{2}", SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Directorio Raíz Gestor Archivos SAT"), 72.ToString("0000"), idFactura.ToString("0000000") + "-" + Session["XMLFileName"].ToString());

                                            //Insertamos Registro
                                            resultado = SAT_CL.Global.ArchivoRegistro.InsertaArchivoRegistro(72, idFactura, 8, "FACTURA: " + Session["XMLFileName"].ToString(), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario,
                                                Encoding.UTF8.GetBytes(doc.OuterXml), ruta);
                                        }
                                    }

                                    //Validando la Operación de la Transacción
                                    if (resultado.OperacionExitosa)
                                    {
                                        //Insertando Comprobacion de Factura
                                        resultado = FacturadoProveedorRelacion.InsertarFacturadoProveedorRelacion(idFactura, 104, Convert.ToInt32(lblIdComprobacion.Text), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                        //Validando la Operación de la Transacción
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Actualizando el Total de las Comprobaciones
                                            resultado = cmp.ActualizaTotalComprobacion(FacturadoProveedorRelacion.ObtieneTotalFacturasComprobacion(cmp.id_comprobacion), ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando la Operación de la Transacción
                                            if (resultado.OperacionExitosa)

                                                //Completando Transacción
                                                trans.Complete();
                                        }
                                    }
                                }
                                else
                                {
                                    //Instanciando Excepcion
                                    resultado = new RetornoOperacion(mensaje);

                                    //Limpiando Session
                                    Session["XML"] = null;
                                }
                            }
                            else
                            {
                                //Instanciando Excepcion
                                resultado = new RetornoOperacion("No existe la Factura");

                                //Limpiando Session
                                Session["XML"] = null;
                            }
                        }
                    }
                    else
                        //Instanciando Excepción
                        resultado = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");

                    //Validando que la Operación haya Sido Exitosa
                    if (resultado.OperacionExitosa)
                    {
                        //Instanciando Liquidación
                        using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text == "Por Asignar" ? "0" : lblId.Text)))
                        {
                            //Obteniendo Entidad
                            int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                            //Cargando resultados Actualizados
                            cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                            //Inicializando Indices
                            TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);
                        }

                        //Inicializando Controles
                        inicializaControlComprobaciones(Convert.ToInt32(lblIdComprobacion.Text), 0);
                    }
                }
                else
                {
                    //Instanciando Excepcion
                    resultado = new RetornoOperacion("No existe la Comprobación");

                    //Limpiando Session
                    Session["XML"] = null;
                }
            }

            //Mostrando Mensaje de la Operación
            lblErrorComprobacion.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Método Privado encargado de Guardar la Comprobación
        /// </summary>
        private void guardaComprobacion()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();

            //Obteniendo Deposito
            int idDeposito = Convert.ToInt32(gvAnticipos.SelectedIndex == -1 ? 0 : gvAnticipos.SelectedDataKey["Id"]);

            //Instanciando Liquidación
            using(SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text)))
            
            //Instanciando Deposito
            using (SAT_CL.EgresoServicio.Deposito dep = new SAT_CL.EgresoServicio.Deposito(idDeposito))
            {
                //Validando que no exista la Comprobación
                if (lblIdComprobacion.Text == "Por Asignar")
                {
                    //Validando que el Movimiento se encuentre Terminado
                    if (validaMovimientoTerminado(dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_movimiento : Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))

                        //Insertando Comprobación
                        result = Comprobacion.InsertaComprobacion(idDeposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text, false, 0,
                                                                    DetalleLiquidacion.Estatus.Registrado, liq.id_unidad, liq.id_operador, liq.id_proveedor,
                                                                    Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), dep.id_deposito > 0 ? dep.objDetalleLiquidacion.id_movimiento : Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"]),
                                                                    liq.fecha_liquidacion, liq.id_liquidacion, 1, Convert.ToDecimal(txtValorUnitario.Text),
                                                                    ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                    else
                        //Instanciando Excepción
                        result = new RetornoOperacion("El Movimiento " + gvMovimientos.SelectedDataKey["Id"].ToString() + " no esta Terminado");
                }
                else
                {
                    //Instanciando Comprobación
                    using(Comprobacion cmp = new Comprobacion(Convert.ToInt32(lblIdComprobacion.Text)))
                    {
                        //Validando que exista el Registro
                        if(cmp.id_comprobacion > 0)
                        {
                            //Validando que el Movimiento se encuentre Terminado
                            if (validaMovimientoTerminado(cmp.objDetalleComprobacion.id_movimiento))

                                //Editando Comprobación
                                result = cmp.EditaComprobacion(cmp.id_deposito, Convert.ToInt32(ddlConcepto.SelectedValue), 0, txtObservacion.Text,
                                                        cmp.bit_transferencia, cmp.id_transferencia, (DetalleLiquidacion.Estatus)cmp.objDetalleComprobacion.id_estatus_liquidacion,
                                                        cmp.objDetalleComprobacion.id_unidad, cmp.objDetalleComprobacion.id_operador, cmp.objDetalleComprobacion.id_proveedor_compania,
                                                        cmp.objDetalleComprobacion.id_servicio, cmp.objDetalleComprobacion.id_movimiento, cmp.objDetalleComprobacion.fecha_liquidacion,
                                                        cmp.objDetalleComprobacion.id_liquidacion, cmp.objDetalleComprobacion.cantidad, Convert.ToDecimal(txtValorUnitario.Text == "" ? "0" : txtValorUnitario.Text),
                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                            else
                                //Instanciando Excepción
                                result = new RetornoOperacion("El Movimiento " + cmp.objDetalleComprobacion.id_movimiento.ToString() + " no esta Terminado");
                        }
                    }
                }

                //Validando que la Operación haya sido exitosa
                if (result.OperacionExitosa)
                {
                    //Obteniendo Entidad
                    int id_entidad = liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Operador ? liq.id_operador : liq.tipo_asignacion == SAT_CL.Liquidacion.Liquidacion.TipoAsignacion.Unidad ? liq.id_unidad : liq.id_proveedor;

                    //Cargando resultados Actualizados
                    cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), liq.id_liquidacion, id_entidad, liq.id_tipo_asignacion, liq.id_estatus);

                    //Inicializando Indices
                    TSDK.ASP.Controles.InicializaIndices(gvComprobaciones);

                    //Inicializando Controles
                    inicializaControlComprobaciones(result.IdRegistro, idDeposito);
                }

                //Mostrando Mensaje de la Operacion
                lblErrorComprobacion.Text = result.Mensaje;
            }
        }

        #endregion

        #region Métodos "Cargos Recurrentes"

        /// <summary>
        /// Método Privado encargado de Caragr los Cobros Recurrentes dada una Liquidación
        /// </summary>
        /// <param name="id_liquidacion"></param>
        private void cargaCobrosRecurrentes(int id_liquidacion)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
            {
                //Obteniendo Fecha de Calculo
                DateTime fecha_liq = liq.estatus == SAT_CL.Liquidacion.Liquidacion.Estatus.Liquidado ? TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro() : liq.fecha_liquidacion;
                
                //Obteniendos Cargos Recurrentes
                using (DataTable dtCargosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesEntidad(liq.id_tipo_asignacion, liq.id_unidad, liq.id_operador, liq.id_proveedor, liq.id_compania_emisora, fecha_liq))
                {
                    //Validando que Existan Registros
                    if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCargosRecurrentes))
                    {
                        //Cargando GridView
                        TSDK.ASP.Controles.CargaGridView(gvCobrosRecurrentes, dtCargosRecurrentes, "Id", "", true, 1);

                        //Añadiendo Tabla a Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCargosRecurrentes, "Table8");
                    }
                    else
                    {
                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvCobrosRecurrentes);

                        //Eliminando Tabla de Session
                        Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table8");
                    }
                }
            }
        }
        /// <summary>
        /// Método Privado encargado de Cargar Todos los Cobros Recurrentes que ha Tenido la Entidad
        /// </summary>
        /// <param name="id_liquidacion">Liquidación Actual</param>
        /// <param name="id_tipo_entidad">Tipo de Entidad de Aplicación</param>
        /// <param name="id_unidad">Unidad</param>
        /// <param name="id_operador">Operador</param>
        /// <param name="id_proveedor">Proveedor</param>
        /// <param name="id_compania_emisor">Compania Emisora</param>
        private void cargaCobrosRecurrentesTotales(int id_liquidacion, byte id_tipo_entidad, int id_unidad, int id_operador, int id_proveedor, int id_compania_emisor)
        {
            //Instanciando Cobros Recurrentes
            using (DataTable dtCobrosRecurrentes = SAT_CL.Liquidacion.CobroRecurrente.ObtieneCobrosRecurrentesTotales(id_liquidacion, id_tipo_entidad, id_unidad, id_operador, id_proveedor, id_compania_emisor))
            {
                //Validando que existen los Cobros Recurrentes
                if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtCobrosRecurrentes))
                {
                    //Cargando Cobro Recurrente
                    TSDK.ASP.Controles.CargaGridView(gvCobroRecurrenteLiquidacion, dtCobrosRecurrentes, "Id", "");

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtCobrosRecurrentes, "Table9");
                }
                else
                {
                    //Inicializando Cobro Recurrente
                    TSDK.ASP.Controles.InicializaGridview(gvCobroRecurrenteLiquidacion);

                    //Añadiendo Tabla a Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table9");
                }
            }
        }


        #endregion

        #region Métodos Resumen Liquidación

        /// <summary>
        /// Método Privado encargado de Cargar el Resumen Total de la Liquidación
        /// </summary>
        /// <param name="id_liquidacion"></param>
        private void cargaResumenTotalLiquidacion(int id_liquidacion)
        {
            //Instanciando Liquidación
            using (SAT_CL.Liquidacion.Liquidacion liq = new SAT_CL.Liquidacion.Liquidacion(id_liquidacion))
            {
                //Validando que exista la Liquidación
                if (liq.id_liquidacion > 0)
                {
                    //Obteniendo Reporte Total
                    using (DataTable dtResumenLiquidación = SAT_CL.EgresoServicio.DetalleLiquidacion.ObtieneResumenTotalLiquidacion(id_liquidacion))
                    {
                        //Validando que existan Registros
                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtResumenLiquidación))
                        {
                            //Cargando GridView
                            TSDK.ASP.Controles.CargaGridView(gvResumenLiquidacion, dtResumenLiquidación, "Id", "");

                            //Ajustando Tamaño del GridView
                            gvResumenLiquidacion.PageSize = dtResumenLiquidación.Rows.Count + 1;

                            //Añadiendo Tabla a DataSet de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtResumenLiquidación, "Table11");
                        }
                        else
                        {
                            //Inicializando GridView
                            TSDK.ASP.Controles.InicializaGridview(gvResumenLiquidacion);

                            //Eliminando Tabla de DataSet de Session
                            Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table11");
                        }
                    }
                }
                else
                {
                    //Inicializando GridView
                    TSDK.ASP.Controles.InicializaGridview(gvResumenLiquidacion);

                    //Eliminando Tabla de DataSet de Session
                    Session["DS"] = TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table11");
                }
            }
        }

        #endregion

        #region Pagos con Tarifa

        /// <summary>
        /// Realiza la búsqueda de la tarifa de pago del servicio
        /// </summary>
        private void buscaTarifaPagoServicio()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando transaccion
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación actual
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text)))
                {
                    //Declarando variables auxiliares
                    int id_entidad_pago = 0;
                    SAT_CL.TarifasPago.Tarifa.PerfilPago perfil_pago;
                    //Determinando el tipo de entidad a pagar
                    if (liquidacion.id_operador > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Operador;
                        id_entidad_pago = liquidacion.id_operador;
                    }
                    else if (liquidacion.id_unidad > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Unidad;
                        id_entidad_pago = liquidacion.id_unidad;
                    }
                    else
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Transportista;
                        id_entidad_pago = liquidacion.id_proveedor;
                    }

                    //Realizando búsqueda de tarifa de pago aplicable
                    using (DataTable tarifas = SAT_CL.TarifasPago.Tarifa.ObtieneTarifasPagoServicio(Convert.ToInt32(gvViajes.SelectedDataKey.Value), perfil_pago, id_entidad_pago, true))
                    {
                        //Si existen registros
                        if (tarifas != null)
                        {
                            //Guardando tarifas en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], tarifas, "Table13");

                            //Si existe más de una tarifa que aplique a este servicio
                            if (tarifas.Rows.Count > 1)
                            {
                                //Cargando conjunto de tarifas en control Gridview
                                TSDK.ASP.Controles.CargaGridView(gvTarifasPago, tarifas, "IdTarifa", lblOrdenadoTarifasAplicables.Text);
                                //Asignando Tooltip para diferenciar la entidad a la que se aplicará la tarifa (Tarifas de Pago Servicio / Tarifas de Pago Movimiento)
                                gvTarifasPago.ToolTip = "Tarifas de Pago Servicio";
                                //Mostrando ventana modal con resultados
                                TSDK.ASP.ScriptServer.AlternarVentana(upgvViajes, upgvViajes.GetType(), "ventanaTarifasPago", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
                            }
                            //Si sólo hay una coincidencia
                            else if (tarifas.Rows.Count == 1)
                            {
                                //Aplicando tarifa a servicio
                                resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, tarifas.Rows[0].Field<int>("IdTarifa"), Convert.ToInt32(gvViajes.SelectedDataKey.Value),
                                                                        liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor,
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si se actualizó correctamente
                                if (resultado.OperacionExitosa)
                                {
                                    //Obteniendo Pago
                                    int idPago = resultado.IdRegistro;

                                    //Obteniendo Tarifas Secundarias
                                    using (DataTable dtTarifasSecundarias = SAT_CL.TarifasPago.TarifaCompuesta.ObtieneTarifasSecundarias(tarifas.Rows[0].Field<int>("IdTarifa")))
                                    {
                                        //Validando que Existen Registros
                                        if (TSDK.Datos.Validacion.ValidaOrigenDatos(dtTarifasSecundarias))
                                        {
                                            //Recorriendo Ciclo de Tarifas Secundarias
                                            foreach (DataRow dr in dtTarifasSecundarias.Rows)
                                            {
                                                //Aplicando Tarifa Secundaria
                                                resultado = Pago.AplicaTarifaPagoServicio(liquidacion.id_liquidacion, Convert.ToInt32(dr["IdTarifaSecundaria"]), Convert.ToInt32(gvViajes.SelectedDataKey.Value),
                                                                        liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                //Validando que la Operación fuese Correcta
                                                if (!resultado.OperacionExitosa)

                                                    //Terminando Ciclo
                                                    break;
                                            }
                                        }
                                        else
                                            //Instanciando Resultado Positivo
                                            resultado = new RetornoOperacion(idPago);

                                        //Validando que la Operación fuese Exitosa
                                        if (resultado.OperacionExitosa)
                                        {
                                            //Realizando actualización de vales y anticipos
                                            resultado = actualizaLiquidacionDepositosValesServicio(Convert.ToInt32(gvViajes.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion);

                                            //Si no hay errores
                                            if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                            {
                                                //Cargando los Movimientos y los Pagos
                                                cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.id_estatus);
                                                //Confirmando cambios realizados
                                                scope.Complete();
                                            }
                                        }
                                    }
                                }
                                
                                
                                //Si se aplicó la tarifa correctamente
                                if (resultado.OperacionExitosa)
                                {
                                    

                                    //Actualizando anticipos y vales de diesel
                                    resultado = actualizaLiquidacionDepositosValesServicio(Convert.ToInt32(gvViajes.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                    {                                        
                                        //Cargando los Movimientos y los Pagos
                                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.id_estatus);
                                        //Completando transacción
                                        scope.Complete();
                                    }
                                }
                            }
                        }
                        //Si no hay tarifas
                        else
                        {
                            //Borrando origen de datos previo
                            TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table13");

                            //Indicando que no existen tarifas aplicables
                            resultado = new RetornoOperacion("No existen tarifas de pago coincidentes para este servicio.");
                        }
                    }
                }
            }

            //Mostrando resultado
            lblErrorLiquidacion.Text = resultado.Mensaje;
        }

        /// <summary>
        /// Realiza la búsqueda de la tarifa de pago del movimiento seleccionado
        /// </summary>
        private void buscaTarifaPagoMovimiento()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion();

            //Inicializando bloque transaccional
            using (TransactionScope scope = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
            {
                //Instanciando liquidación actual
                using (SAT_CL.Liquidacion.Liquidacion liquidacion = new SAT_CL.Liquidacion.Liquidacion(Convert.ToInt32(lblId.Text)))
                {
                    //Declarando variables auxiliares
                    int id_entidad_pago = 0;
                    SAT_CL.TarifasPago.Tarifa.PerfilPago perfil_pago;
                    //Determinando el tipo de entidad a pagar
                    if (liquidacion.id_operador > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Operador;
                        id_entidad_pago = liquidacion.id_operador;
                    }
                    else if (liquidacion.id_unidad > 0)
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Unidad;
                        id_entidad_pago = liquidacion.id_unidad;
                    }
                    else
                    {
                        perfil_pago = SAT_CL.TarifasPago.Tarifa.PerfilPago.Transportista;
                        id_entidad_pago = liquidacion.id_proveedor;
                    }

                    //Realizando búsqueda de tarifa de pago aplicable
                    using (DataTable tarifas = SAT_CL.TarifasPago.Tarifa.ObtieneTarifasPagoMovimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey.Value), perfil_pago, id_entidad_pago, false))
                    {
                        //Si existen registros
                        if (tarifas != null)
                        {
                            //Guardando tarifas en sesión
                            Session["DS"] = TSDK.Datos.OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], tarifas, "Table13");

                            //Si existe más de una tarifa que aplique a este servicio
                            if (tarifas.Rows.Count > 1)
                            {
                                //Cargando conjunto de tarifas en control Gridview
                                TSDK.ASP.Controles.CargaGridView(gvTarifasPago, tarifas, "IdTarifa", lblOrdenadoTarifasAplicables.Text);
                                //Asignando Tooltip para diferenciar la entidad a la que se aplicará la tarifa (Tarifas de Pago Servicio / Tarifas de Pago Movimiento)
                                gvTarifasPago.ToolTip = "Tarifas de Pago Movimiento";
                                //Mostrando ventana modal con resultados
                                TSDK.ASP.ScriptServer.AlternarVentana(upgvMovimientos, upgvMovimientos.GetType(), "ventanaTarifasPago", "contenedorVentanaTarifasPago", "ventanaTarifasPago");
                            }
                            //Si sólo hay una coincidencia
                            else if (tarifas.Rows.Count == 1)
                            {
                                //Aplicando tarifa a servicio
                                resultado = Pago.AplicaTarifaPagoMovimiento(liquidacion.id_liquidacion, tarifas.Rows[0].Field<int>("IdTarifa"), Convert.ToInt32(gvMovimientos.SelectedDataKey.Value),
                                                                        liquidacion.id_unidad, liquidacion.id_operador, liquidacion.id_proveedor,
                                                                        ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Si no hay problemas con el pago
                                if (resultado.OperacionExitosa)
                                {
                                    //Realizando actualización de vales y anticipos
                                    resultado = actualizaLiquidacionDepositosVales(Convert.ToInt32(gvMovimientos.SelectedDataKey.Value), liquidacion.id_liquidacion, id_entidad_pago, liquidacion.id_tipo_asignacion, true);

                                    //Si no hay errores
                                    if (resultado.OperacionExitosa && liquidacion.ActualizaLiquidacion())
                                    {                                        
                                        //Cargando los Movimientos y los Pagos
                                        cargaMovimientosPagosViajes(Convert.ToInt32(gvViajes.SelectedDataKey["Id"]), Convert.ToInt32(lblId.Text), id_entidad_pago, liquidacion.id_tipo_asignacion, liquidacion.id_estatus);
                                        //Confirmando cambios realizados
                                        scope.Complete();
                                    }
                                    else
                                        resultado = new RetornoOperacion(string.Format("Error al actualizar anticipos y vales del movimiento ID '{0}': {1}", gvMovimientos.SelectedDataKey.Value, resultado.Mensaje));
                                }
                            }
                        }
                    }
                }
            }

            //Si no hay tarifas coincidentes o bien no se encontró una aplicación (detalle) en la tarifa coincidente
            if (!resultado.OperacionExitosa)
            {
                //Invocando Método de Configuración
                configuraVentanaPagos("Movimiento");
            }

            //Mostrando resultado
            lblErrorLiquidacion.Text = resultado.Mensaje;
        }
        /// <summary>
        /// Método encargado de Configurar la Ventana de Pagos
        /// </summary>
        /// <param name="comand">Comando de Configuración</param>
        private void configuraVentanaPagos(string comand)
        {
            //Validando Comando
            switch(comand)
            {
                case "Movimiento":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }
                        
                        //Se borra origen de datos anterior
                        TSDK.Datos.OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table13");

                        //Instanciando el movimiento de interés
                        using (Movimiento mov = new Movimiento(Convert.ToInt32(gvMovimientos.SelectedDataKey["Id"])))
                        {
                            //Obteniendo Item de forma Dinamica
                            ListItem li = ddlTipoPago.Items.FindByText("Km " + mov.TipoMovimiento.ToString());
                            //Limpiando Items
                            ddlTipoPago.Items.Clear();
                            //Añadiendo Item
                            ddlTipoPago.Items.Add(li);
                            //Habilitando Controles para Insercción de Pago
                            limpiaControlesPagos();
                            habilitaControlesPagos(true);
                            //Asignando Cantidad
                            txtCantidad.Text = mov.kms.ToString();

                            //Asignando Comando
                            btnGuardarPago.CommandName = "CreaPago";
                            upbtnGuardarPago.Update();

                            //Deshabilitando Control
                            txtDescripcion.Enabled = false;
                            uptxtDescripcion.Update();

                            //Instanciando Paradas
                            using (SAT_CL.Despacho.Parada parada_o = new Parada(mov.id_parada_origen), parada_d = new Parada(mov.id_parada_destino))
                            {
                                //Validando que Existan
                                if (parada_o.id_parada > 0 && parada_d.id_parada > 0)

                                    //Asignando Valor
                                    txtDescripcion.Text = "Pago desde " + parada_o.descripcion + " a " + parada_d.descripcion;
                                else
                                    //Limpiando Descripción
                                    txtDescripcion.Text = "";
                            }
                        }

                        //Mostrando ventana de captura
                        TSDK.ASP.ScriptServer.AlternarVentana(this, this.GetType(), "VentanaPagos", "contenedorVentanaPagos", "ventanaPagos");
                        break;
                    }
                case "VariosMovimientos":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }
                        
                        //Obteniendo Item de forma Dinamica
                        ListItem li = ddlTipoPago.Items.FindByText("Movimiento");
                        //Limpiando Items
                        ddlTipoPago.Items.Clear();
                        //Añadiendo Item
                        ddlTipoPago.Items.Add(li);
                        //Asignando Valor
                        txtDescripcion.Text = "Pago a Varios Movimientos";
                        //Obteniendo Control
                        CheckBox chkVarios;
                        
                        //Recorriendo cada fila
                        foreach (GridViewRow gvr in gvMovimientos.Rows)
                        {   
                            //Obteniendo Control
                            chkVarios = (CheckBox)gvr.FindControl("chkVarios");
                            
                            //Validando que existe la Fila
                            if (chkVarios != null)
                                //Seleccionando Fila
                                chkVarios.Checked = chkVarios.Enabled;
                        }

                        //Mostrando Total de Movimientos
                        txtCantidad.Text = TSDK.ASP.Controles.ObtenerFilasSeleccionadas(gvMovimientos, "chkVarios").Length.ToString();

                        //Asignando Comando
                        btnGuardarPago.CommandName = "CreaPago";
                        upbtnGuardarPago.Update();

                        //Visualizando Control
                        btnCrearPago.Visible = btnCrearPago.Enabled = true;
                        break;
                    }
                case "SinMovimiento":
                    {
                        //Obteniendo Tipos de Pago
                        using (DataTable dtConceptosPago = SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(33, "", 0, "", 0, ""))
                        {
                            //Validando que existan los Conceptos de Pago
                            if (Validacion.ValidaOrigenDatos(dtConceptosPago))

                                //Cargando DropDownList
                                Controles.CargaDropDownList(ddlTipoPago, dtConceptosPago, "id", "descripcion");
                            else
                                //Inicializando DropDownList
                                Controles.InicializaDropDownList(ddlTipoPago, "-- No hay Conceptos de Pago");
                        }

                        //Habilitando Controles para Insercción de Pago
                        limpiaControlesPagos();
                        habilitaControlesPagos(true);

                        //Asignando Comando Indicado
                        btnGuardarPago.CommandName = "CreaOtrosPagos";

                        break;
                    }
            }
        }

        #endregion

        #endregion
    }
}