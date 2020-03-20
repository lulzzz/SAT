using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using TSDK.ASP;
using TSDK.Base;
using TSDK.Datos;

namespace SAT.Administrativo
{
    public partial class FichaIngreso : System.Web.UI.Page
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

            //Validando que se haya producido un PostBack
            if (!Page.IsPostBack)

                //Invocando Método de Inicialización de Página
                inicializaPagina();

            //Invocando Carga de Autocompletado
            cargaCatalogoAutocompleta();
        }
        /// <summary>
        /// Evento disparado al Presionar el Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbElementoMenu_Click(object sender, EventArgs e)
        {   
            //Obteniendo Link
            LinkButton lkb = (LinkButton)sender;
            
            //Validando estatus de Página
            switch (lkb.CommandName)
            {
                case "Nuevo":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        //Limpiando Id de sesión
                        Session["id_registro"] = 0;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Abrir":
                    {   
                        //Inicializando Apertura de Registros
                        inicializaAperturaRegistro(101, ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor);

                        break;
                    }
                case "Guardar":
                    {
                        //Validando que Exista una Cuenta de Origen
                        if (ddlCuentaOrigen.SelectedValue != "0")

                            //Invocando Método de Guardado
                            guardaFichaIngreso();
                        else
                            //Mostrando Mensaje de Error
                            ScriptServer.MuestraNotificacion(lkb, "Debe Seleccionar un Cuenta de Origen", ScriptServer.NaturalezaNotificacion.Error, ScriptServer.PosicionNotificacion.AbajoDerecha);
                        break;
                    }
                case "Editar":
                    {   
                        //Asignando estatus nuevo
                        Session["estatus"] = Pagina.Estatus.Edicion;
                        //Limpiando contenido de forma
                        inicializaPagina();
                        break;
                    }
                case "Eliminar":
                    {
                        //Instanciando Producto
                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                        {
                            //Validando que exista un Id Valido
                            if (ei.id_egreso_ingreso != 0)
                            {
                                //Declarando Objeto de Retorno
                                RetornoOperacion result = new RetornoOperacion();

                                //Deshabilitando Producto
                                result = ei.DeshabilitarFichaIngreso(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                
                                //Validando que la Operación sea exitosa
                                if (result.OperacionExitosa)
                                {   
                                    //Limpiando registro de Session
                                    Session["id_registro"] = 0;
                                    //Cambiando a Estatus "Nuevo"
                                    Session["estatus"] = Pagina.Estatus.Nuevo;
                                    //Inicializando Forma
                                    inicializaPagina();
                                }
                                
                                //Mostrando Mensaje de Operación
                                ScriptServer.MuestraNotificacion(lkb, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                            }
                        }
                        break;
                    }
                case "Imprimir":
                    {
                        //Obteniendo Ruta
                        string urlReporte = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/RDLC/Reporte.aspx");
                        //Instanciando nueva ventana de navegador para apertura de registro
                        TSDK.ASP.ScriptServer.AbreNuevaVentana(string.Format("{0}?idTipoReporte={1}&idRegistro={2}", urlReporte, "FichaIngreso", Convert.ToInt32(Session["id_registro"])), "FichaIngreso", "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500", Page);
                        break;
                    }
                case "Bitacora":
                    {   
                        //Invocando Método de Inicializacion de Bitacora
                        inicializaBitacora(Session["id_registro"].ToString(), "101", "Ficha Ingreso");
                        break;
                    }
                case "Referencias":
                    {   
                        //Invocando Método de Inicialización de Referencias
                        inicializaReferenciaRegistro(Session["id_registro"].ToString(), "101", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString());
                        break;
                    }
                case "Archivos":
                    //TODO: Implementar uso de archivos ligados a registro
                    break;
                case "Acerca":
                    //TODO: Implementar uso de acerca de
                    break;
                case "Ayuda":
                    //TODO: Implementar uso de ayuda
                    break;
            }
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Método de Pago"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Actualizando lista de cuentas
            cargarCatalogosCuentaFormaPago();

            //Asignando Enfoque al Control para introducir el número de referencia (operación bancaria)
            txtNumCheque.Focus();
        }

        private void cargarCatalogosCuentaFormaPago()
        {
            //Instanciando forma de pago seleccionada
            using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(Convert.ToInt32(ddlMetodoPago.SelectedValue)))
            {
                //SI existe la forma de pago
                if (fp.habilitar)
                {
                    //TODO: Aplicar habilitación y borrado de contenido ACORDE A CATÁLOGO FORMA PAGO DEL SAT
                    switch (fp.clave)
                    {
                        //_02_Cheque
                        case "02":
                        //_03_TransaferenciaElectronica
                        case "03":
                        //_04_TarjetaCredito
                        case "04":
                        //_05_MonederoElectronico
                        case "05":
                        //_28_TarjetaDebito
                        case "28":
                        //_29_TarjetaServicios
                        case "29":
                            //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaOrigen, 42, "-- Seleccione una Cuenta", Convert.ToInt32(ddlTipoEntidad.SelectedValue), "",
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 1)), "");
                            //Cargando Cuentas de Destino -- Donde se va a Recibir el Dinero
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaDestino, 42, "", 25, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "");
                            break;
                        //_06_DineroElectronico
                        case "06":
                            //Cargando Cuentas de Origen -- Donde se va a Obtener el Dinero
                            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlCuentaOrigen, 42, "-- Seleccione una Cuenta", Convert.ToInt32(ddlTipoEntidad.SelectedValue), "",
                                            Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 1)), "");
                            //Inicializando cuentas de destino
                            Controles.InicializaDropDownList(ddlCuentaDestino, "-- No Requerida");
                            break;
                        default:
                            Controles.InicializaDropDownList(ddlCuentaOrigen, "-- Seleccione una Cuenta");
                            Controles.InicializaDropDownList(ddlCuentaDestino, "-- Seleccione una Cuenta");
                            break;
                    }
                }
                else
                    ScriptServer.MuestraNotificacion(ddlMetodoPago, "La forma de Pago seleccionada no pudo ser encontrada.", ScriptServer.NaturalezaNotificacion.Advertencia, ScriptServer.PosicionNotificacion.AbajoDerecha);
            }
        }

        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Invocando Método de Guardado
            guardaFichaIngreso();
        }
        /// <summary>
        /// Evento Producido al Presionar el Boton "Cancelar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Validando estatus de Página
            switch((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Edicion:
                    
                        //Asignando a Lectura
                        Session["estatus"] = Pagina.Estatus.Lectura;
                        break;
                default:
                    
                        //Asignando a Nuevo
                        Session["estatus"] = Pagina.Estatus.Nuevo;
                        break;
            }

            //Invocando Inicialización de Página
            inicializaPagina();
        }
        /// <summary>
        /// Evento Producido al Cambiar el Indice del DropDownList "Moneda"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Asignando Valor en caso de Vacio
            txtMonto.Text = txtMonto.Text == "" ? "0.00" : txtMonto.Text;

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            btnGuardar.Focus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtMonto_TextChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Asignando Valor en caso de Vacio
            txtMonto.Text = txtMonto.Text == "" ? "0.00" : txtMonto.Text;

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            ddlMoneda.Focus();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFechaEI_TextChanged(object sender, EventArgs e)
        {
            //Obteniendo Fecha
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Calculando Montos
            txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                Convert.ToByte(ddlMoneda.SelectedValue), fecha));

            //Asignando Enfoque al Control
            txtMonto.Focus();
        }//*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtNombreDep_TextChanged(object sender, EventArgs e)
        {
            cargarCatalogosCuentaFormaPago();
            //Asignando Enfoque al Control
            ddlMetodoPago.Focus();
        }

        #region Eventos GridView "Fichas Facturas"

        /// <summary>
        /// Evento Producido al Cambiar el Indice de Página del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Cambiando Indice de Página
            Controles.CambiaIndicePaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.NewPageIndex);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// Evento Producido al Cambiar la Expresión del Ordenamiento del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvFichasFacturas_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Cambiando Expresión del Ordenamiento
            lblOrdenadoFF.Text = Controles.CambiaSortExpressionGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), e.SortExpression);

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTamanoFF_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cambiando tamaño del GridView
            Controles.CambiaTamañoPaginaGridView(gvFichasFacturas, OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"), Convert.ToInt32(ddlTamanoFF.SelectedValue));

            //Validando que Existan Registros
            if (Validacion.ValidaOrigenDatos(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2")))

                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
            else
                //Mostrando Totales
                gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
        }
        /// <summary>
        /// Evento Producido al Eliminar la Aplicación del GridView "Fichas Facturas"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkEliminarAplicacion_Click(object sender, EventArgs e)
        {
            //Validando que Existan Registros
            if (gvFichasFacturas.DataKeys.Count > 0)
            {
                //Seleccionando Fila
                Controles.SeleccionaFila(gvFichasFacturas, sender, "lnk", false);
                SAT_CL.Facturacion.Facturado.EstatusFactura estatusFactura;

                //Obteniendo Control
                using (LinkButton lkb = (LinkButton)sender)
                {
                    //Declarando Objeto de Retorno
                    RetornoOperacion result = new RetornoOperacion();

                    //Declarando Variable Auxiliar
                    SAT_CL.Bancos.EgresoIngreso.Estatus estatus;

                    //Instanciando Aplicación de la Ficha de Ingreso
                    using (SAT_CL.CXC.FichaIngresoAplicacion fia = new SAT_CL.CXC.FichaIngresoAplicacion(Convert.ToInt32(gvFichasFacturas.SelectedDataKey["Id"])))
                    {
                        //Validando que exista el Registro
                        if (fia.id_ficha_ingreso_aplicacion > 0)
                        {
                            //Inicializando Bloque Transaccional
                            using (TransactionScope trans = TSDK.Datos.Transaccion.InicializaBloqueTransaccional(System.Transactions.IsolationLevel.ReadCommitted))
                            {
                                //Deshabilitando Aplicación de la Ficha de Ingreso
                                result = fia.DeshabilitarFichaIngresoAplicacion(((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                //Validando que la Operación fuese Exitosa
                                if (result.OperacionExitosa)
                                {
                                    //Instanciando la Ficha de Ingreso
                                    using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(fia.id_egreso_ingreso))
                                    {
                                        //Validando que exista la Ficha de Ingreso
                                        if (ei.id_egreso_ingreso > 0)
                                        {
                                            //Asignando Estatus
                                            estatus = ei.estatus;

                                            //Validando que existan Registros
                                            if (OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2").Rows.Count - 1 > 0)

                                                //Asignando Estatus a Aplciada Parcial
                                                estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.AplicadaParcial;

                                            else
                                                //Asignando Estatus A Capturada
                                                estatus = SAT_CL.Bancos.EgresoIngreso.Estatus.Capturada;

                                            //Actualizando Ficha de Ingreso
                                            result = ei.ActualizaFichaIngresoEstatus(estatus, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                            //Validando que la Operación fuese Exitosa
                                            if (result.OperacionExitosa)
                                            {
                                                //Instanciando Factura
                                                using (SAT_CL.Facturacion.Facturado fac = new SAT_CL.Facturacion.Facturado(Convert.ToInt32(fia.id_registro)))
                                                {
                                                    //Validando que exista la Factura
                                                    if (fac.id_factura > 0)
                                                    {
                                                        //Calculando Estatus de la Factura
                                                        estatusFactura = SAT_CL.Facturacion.Facturado.ObtieneMontoPendienteAplicacion(fia.id_registro) == fac.total_factura ? SAT_CL.Facturacion.Facturado.EstatusFactura.Registrada : SAT_CL.Facturacion.Facturado.EstatusFactura.AplicadaParcial;

                                                        //Actualizando Estatus de la Factura
                                                        result = fac.ActualizaEstatusFactura(estatusFactura, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                                        //Validando que la Operación fuese Exitosa
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
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No se puede Acceder a la Ficha de Ingreso");

                        //Validando que la Operación fuese Exitosa
                        if (result.OperacionExitosa)
                        {
                            //Asignando Session
                            Session["estatus"] = Pagina.Estatus.Edicion;
                            Session["id_registro"] = fia.id_egreso_ingreso;

                            //Inicializando Página
                            inicializaPagina();

                            //Asignando Resultados
                            result = new RetornoOperacion(string.Format("La Factura {0}: Ha sido Desaplicada por el monto de {1:C2}", fia.id_registro, fia.monto_aplicado), result.OperacionExitosa);

                            //Invocando Métodos de Busqueda
                            buscaFichasAplicadas(fia.id_egreso_ingreso, 0);

                            //Inicializando Indices
                            Controles.InicializaIndices(gvFichasFacturas);
                        }

                        //Mostrando Mensaje de Error
                        ScriptServer.MuestraNotificacion(gvFichasFacturas, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkExportarFF_Click(object sender, EventArgs e)
        {
            //Exportando Contenido del GridView
            Controles.ExportaContenidoGridView(OrigenDatos.RecuperaDataTableDataSet((DataSet)Session["DS"], "Table2"));
        }

        #endregion

        #endregion

        #region Métodos

        /// <summary>
        /// Método Privado encargado de Inicializar los Controles de la Página
        /// </summary>
        private void inicializaPagina()
        {
            //Obteniendo Estatus de Página
            Session["estatus"] = Session["estatus"] == null ? Pagina.Estatus.Nuevo : Session["estatus"];
            //Cargando Catalogos
            cargaCatalogos();
            //Habilitando Menu
            habilitaMenu();
            //Habilitando Controles
            habilitaControles();
            //Inicializando Valores
            inicializaValores();
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
                                $('#" + this.ddlTipoEntidad.ClientID + @"').serialize();

                                //Obteniendo Tipo de Entidad
                                var tipoEntidad = " + this.ddlTipoEntidad.SelectedValue + @";

                                //Validando Tipo de Entidad
                                switch (tipoEntidad) {
                                    case 25:
                                        {   
                                            //Cargando Catalogo AutoCompleta
                                            $('#" + txtNombreDep.ClientID + @"').autocomplete({
                                                source:'../WebHandlers/AutoCompleta.ashx?id=15&param="+idCompania+@"',
                                                select:  function (event, ui) {
            
                                                    //Asignando Selección al Valor del Control
                                                    $('#" + txtNombreDep.ClientID + @"').val(ui.item.value);

                                                    //Causando Actualización del Control
                                                    __doPostBack('" + txtNombreDep.UniqueID + @"', '');
                                                }
                                            });
                                            
                                            break;
                                        }
                                }
                                
                              </script>";

            //Registrando Script
            ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAutocompletaEntidad", script, false);
        }
        /// <summary>
        /// Método Privado encargado de Cargar los Catalogos de la Forma
        /// </summary>
        private void cargaCatalogos()
        {
            //Cargando Tipo de Entidad
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlTipoEntidad, 41, "-- Ninguno");
            //Cargando Conceptos
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlConcepto, 43, "", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, "", 0, "");
            //Cargando Tipos de Moneda (REGULADO POR EL SAT V3.3 DE CFDI)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMoneda, 110, "");
            //Cargando Tipos de Forma de Pago (REGULADO POR EL SAT V3.3 DE CFDI)
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogo(ddlMetodoPago, 109, "");
            //Cargando Estatus
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlEstatus, "", 79);
            //Cuenta de Destino del Pago
            Controles.InicializaDropDownList(ddlCuentaDestino, "-- Seleccione una Cuenta");
            //Inicializando DropDownList
            TSDK.ASP.Controles.InicializaDropDownList(ddlCuentaOrigen, "-- Seleccione una Cuenta");
            //Cargando Tamaño del GridView
            SAT_CL.CapaNegocio.m_capaNegocio.CargaCatalogoGeneral(ddlTamanoFF, "", 3182);
        }
        /// <summary>
        /// Método Privado encargado de habilitar el Menú
        /// </summary>
        private void habilitaMenu()
        {   
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbImprimir.Enabled=
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = false;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {   
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbImprimir.Enabled =
                        lkbSalir.Enabled = true;
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled = false;
                        //Edicion
                        lkbEditar.Enabled = true;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled =
                        lkbArchivos.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Edicion:
                    {   
                        //Archivo
                        lkbNuevo.Enabled =
                        lkbAbrir.Enabled =
                        lkbGuardar.Enabled =
                        btnGuardar.Enabled =
                        lkbImprimir.Enabled =
                        lkbSalir.Enabled = true;
                        //Edicion
                        lkbEditar.Enabled = false;
                        //Herramientas
                        lkbBitacora.Enabled =
                        lkbReferencias.Enabled = true;
                        lkbArchivos.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Habilitar los Controles
        /// </summary>
        private void habilitaControles()
        {
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:

                case Pagina.Estatus.Edicion:
                    {
                        //Asignando Valores
                        ddlTipoEntidad.Enabled =
                        ddlConcepto.Enabled =
                        txtNombreDep.Enabled =
                        txtNumCheque.Enabled =
                        ddlMetodoPago.Enabled =
                        ddlCuentaOrigen.Enabled =
                        ddlCuentaDestino.Enabled =
                        txtMonto.Enabled =
                        ddlMoneda.Enabled =
                        txtFechaEI.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled = 
                        gvFichasFacturas.Enabled = true;

                        break;
                    }
                case Pagina.Estatus.Lectura:
                    {
                        //Asignando Valores
                        ddlTipoEntidad.Enabled =
                        ddlConcepto.Enabled =
                        txtNombreDep.Enabled =
                        txtNumCheque.Enabled =
                        ddlMetodoPago.Enabled =
                        ddlCuentaOrigen.Enabled =
                        ddlCuentaDestino.Enabled =
                        txtMonto.Enabled =
                        ddlMoneda.Enabled =
                        txtFechaEI.Enabled =
                        btnGuardar.Enabled =
                        btnCancelar.Enabled =
                        gvFichasFacturas.Enabled = false;

                        break;
                    }
            }
        }
        /// <summary>
        /// Método Privado encargado de Inicializar los Valores de la Página
        /// </summary>
        private void inicializaValores()
        {   
            //Validando Estatus de Pagina
            switch ((Pagina.Estatus)Session["estatus"])
            {
                case Pagina.Estatus.Nuevo:
                    {   
                        //Inicializando Valores
                        lblNoFicha.Text = "Por Asignar";
                        txtNombreDep.Text = 
                        txtNumCheque.Text = "";
                        txtMonto.Text = 
                        txtMontoPesos.Text = "0.00";
                        txtFechaEI.Text = TSDK.Base.Fecha.ObtieneFechaEstandarMexicoCentro().ToString("dd/MM/yyyy");

                        //Inicializando GridView
                        TSDK.ASP.Controles.InicializaGridview(gvFichasFacturas);

                        cargarCatalogosCuentaFormaPago();
                        break;
                    }
                case Pagina.Estatus.Lectura:

                case Pagina.Estatus.Edicion:
                    {   
                        //Instanciando Producto
                        using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                        {   
                            //Validando que exista un Id Valido
                            if (ei.id_egreso_ingreso != 0)
                            {   
                                //Inicializando Valores
                                lblNoFicha.Text = ei.secuencia_compania.ToString();
                                txtNumCheque.Text = ei.num_cheque;
                                txtMonto.Text = string.Format("{0:0.00}", ei.monto);
                                txtMontoPesos.Text = string.Format("{0:0.00}", ei.monto_pesos);
                                txtFechaEI.Text = ei.fecha_egreso_ingreso.ToString("dd/MM/yyyy");
                                ddlEstatus.SelectedValue = ei.id_estatus.ToString();
                                ddlTipoEntidad.SelectedValue = ei.id_tabla.ToString();
                                ddlMetodoPago.SelectedValue = ei.id_forma_pago.ToString();
                                ddlMoneda.SelectedValue = ei.id_moneda.ToString();
                                ddlConcepto.SelectedValue = ei.id_egreso_ingreso_concepto.ToString();

                                //Validando que Existe la Entidad
                                if(ei.id_tabla > 0)
                                    //Asignando Valor
                                    txtNombreDep.Text = ei.nombre_depositante + " ID:" + ei.id_registro.ToString();
                                else if (ei.id_tabla == 0)
                                    //Asignando Valor
                                    txtNombreDep.Text = ei.nombre_depositante;


                                //Cargando Cuentas de Destino -- Donde se va a Recibir el Dinero
                                cargarCatalogosCuentaFormaPago();

                                ddlCuentaOrigen.SelectedValue = ei.id_cuenta_origen.ToString();
                                ddlCuentaDestino.SelectedValue = ei.id_cuenta_destino.ToString();

                                //Buscando Fichas Aplicadas
                                buscaFichasAplicadas(ei.id_egreso_ingreso, 0);
                            }
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Método encargado de Guardar las Fichas de Ingreso
        /// </summary>
        private void guardaFichaIngreso()
        {
            //Declarando Objeto de Retorno
            RetornoOperacion result = new RetornoOperacion();
            DateTime fecha = DateTime.MinValue;
            DateTime.TryParse(txtFechaEI.Text, out fecha);

            //Declarando Variables Auxiliares
            int idRegistro = 0;
            bool existe_cuenta = true;
            
            //Validando el Tipo de Entidad
            switch (ddlTipoEntidad.SelectedValue)
            {
                //Cliente Proveedor
                case "25":
                    //Asignando Valores
                    idRegistro = Convert.ToInt32(TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 1));

                    //Validando formato de cuentas seleccionadas acorde forma de pago
                    result = validaCuentasSeleccionadasFormaPago();
                    existe_cuenta = result.OperacionExitosa;
                    break;
            }
            
            //Validando si Existe la Cuenta
            if (existe_cuenta)
            {
                //Calculando Montos
                txtMontoPesos.Text = string.Format("{0:0.00}", obtieneMontoConvertido(Convert.ToDecimal(txtMonto.Text), ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor,
                    Convert.ToByte(ddlMoneda.SelectedValue), fecha));
                
                //Obtiene el Tipo de Cambio
                using (SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, 
                                                        Convert.ToByte(ddlMoneda.SelectedValue), fecha, 0))
                {
                    //Validando que exista el tipo de Cmbio
                    if (tc.habilitar)

                        //Instanciando Resultado Positivo
                        result = new RetornoOperacion(1);
                    else
                    {
                        //Validando Moneda
                        if (Convert.ToByte(ddlMoneda.SelectedValue) == 1)

                            //Instanciando Resultado Positivo
                            result = new RetornoOperacion(1);
                        else
                            //Instanciando Excepción
                            result = new RetornoOperacion("No existe el Tipo de Cambio");
                    }
                }

                //Validando Operación Exitosa
                if (result.OperacionExitosa)
                {
                    //Validando Estatus de Pagina
                    switch ((Pagina.Estatus)Session["estatus"])
                    {
                        case Pagina.Estatus.Nuevo:
                            {
                                //Insertando 
                                result = SAT_CL.Bancos.EgresoIngreso.InsertaFichaIngreso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(ddlTipoEntidad.SelectedValue),
                                            idRegistro, TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 0),
                                            Convert.ToInt32(ddlConcepto.SelectedValue), (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToByte(ddlMetodoPago.SelectedValue),
                                            Convert.ToInt32(ddlCuentaOrigen.SelectedValue), Convert.ToInt32(ddlCuentaDestino.SelectedValue),
                                            txtNumCheque.Text, Convert.ToDecimal(txtMonto.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                            Convert.ToDecimal(txtMontoPesos.Text), fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);

                                break;
                            }
                        case Pagina.Estatus.Edicion:
                            {
                                //Instanciando Producto
                                using (SAT_CL.Bancos.EgresoIngreso ei = new SAT_CL.Bancos.EgresoIngreso(Convert.ToInt32(Session["id_registro"])))
                                {
                                    //Validando que exista un Id Valido
                                    if (ei.id_egreso_ingreso != 0)
                                    {
                                        result = ei.EditaFichaIngreso(((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor, Convert.ToInt32(ddlTipoEntidad.SelectedValue),
                                            idRegistro, TSDK.Base.Cadena.RegresaCadenaSeparada(txtNombreDep.Text, "ID:", 0),
                                            Convert.ToInt32(ddlConcepto.SelectedValue), (SAT_CL.Bancos.EgresoIngreso.FormaPagoSAT)Convert.ToByte(ddlMetodoPago.SelectedValue),
                                            Convert.ToInt32(ddlCuentaOrigen.SelectedValue), Convert.ToInt32(ddlCuentaDestino.SelectedValue),
                                            txtNumCheque.Text, Convert.ToDecimal(txtMonto.Text), Convert.ToByte(ddlMoneda.SelectedValue),
                                            Convert.ToDecimal(txtMontoPesos.Text), fecha, ((SAT_CL.Seguridad.Usuario)Session["usuario"]).id_usuario);
                                    }
                                }

                                break;
                            }
                    }
                }
            }

            //Validando que la Operación haya sido Exitosa
            if(result.OperacionExitosa)
            {
                //Asignando Valores de Session
                Session["id_registro"] = result.IdRegistro;
                Session["estatus"] = Pagina.Estatus.Lectura;

                //Inovcando Método de Inicialización
                inicializaPagina();
            }

            //Mostrando Mensaje de Operación
            ScriptServer.MuestraNotificacion(this, result, ScriptServer.PosicionNotificacion.AbajoDerecha);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Forma de Bitacora
        /// </summary>
        /// <param name="idRegistro">Id de Registro</param>
        /// <param name="idTabla">Id de Tabla</param>
        /// <param name="Titulo">Titulo</param>
        private void inicializaBitacora(string idRegistro, string idTabla, string Titulo)
        {   //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/BitacoraRegistro.aspx?idT=" + idTabla + "&idR=" + idRegistro + "&tB=" + Titulo);
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método Privado encargado de Inicializar la Apertura de un Registro
        /// </summary>
        /// <param name="idTabla">Id de Tabla</param>
        private void inicializaAperturaRegistro(int idTabla, int idCompania)
        {   //Asignando Session
            Session["id_tabla"] = idTabla;
            //Construyendo URL 
            string url = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/AbrirRegistro.aspx?P1=" + idCompania.ToString() + "&P3=2");
            //Definiendo Configuracion de la Ventana
            string configuracion = "location=NO,toolbar=NO,scrollbars=YES,menubar=NO,status=YES,width=800,height=500";
            //Abriendo Nueva Ventana
            TSDK.ASP.ScriptServer.AbreNuevaVentana(url, "", configuracion, Page);
        }
        /// <summary>
        /// Método encargado de Referenciar el Registro
        /// </summary>
        /// <param name="id_registro">Id de Registro</param>
        /// <param name="id_tabla">Id de Tabla</param>
        /// <param name="id_compania">Id de Compania</param>
        private void inicializaReferenciaRegistro(string id_registro, string id_tabla, string id_compania)
        {   //Declarando variable para armado de URL
            string urlDestino = TSDK.Base.Cadena.RutaRelativaAAbsoluta("~/Administrativo/FichaIngreso.aspx", "~/Accesorios/ReferenciaRegistro.aspx?idT=" + id_tabla + "&idR=" + id_registro + "&idC=" + id_compania);
            //Instanciando nueva ventana de navegador para apertura de referencias de registro
            TSDK.ASP.ScriptServer.AbreNuevaVentana(urlDestino, "Referencia", 800, 500, false, false, false, true, true, Page);
        }
        /// <summary>
        /// Método Privado encargado de Obtener el Monto en Pesos
        /// </summary>
        /// <param name="monto">Monto a Convertir</param>
        /// <param name="id_compania">Compania Emisora</param>
        /// <param name="id_moneda">Moneda a Utilizar</param>
        /// <param name="fecha">Fecha de Periodo del Tipo de Cambio</param>
        /// <returns></returns>
        private decimal obtieneMontoConvertido(decimal monto, int id_compania, byte id_moneda, DateTime fecha)
        {
            //Inicializando Monto de Retorno
            decimal monto_pesos = 0.00M;

            //Instanciando Tipo de Cambio
            using(SAT_CL.Bancos.TipoCambio tc = new SAT_CL.Bancos.TipoCambio(id_compania, id_moneda, fecha, 0))
            {
                //Validando que exista el Tipo de Cambio
                if (tc.id_tipo_cambio > 0)
                
                    //Calculando Valor
                    monto_pesos = monto * tc.valor_tipo_cambio;
                else
                    //Asignando Valor
                    monto_pesos = monto;
            }

            //Devolviendo Cantidad Obtenida
            return monto_pesos;
        }
        /// <summary>
        /// Método Privado encargado de Buscar las Fichas Aplicadas
        /// </summary>
        /// <param name="id_ficha_ingreso"></param>
        /// <param name="id_factura"></param>
        private void buscaFichasAplicadas(int id_ficha_ingreso, int id_factura)
        {
            //Obteniendo Reporte
            using (DataTable dtFichasFacturas = SAT_CL.CXC.FichaIngresoAplicacion.ObtieneFichasFacturas(id_ficha_ingreso, id_factura))
            {
                //Validando que Existen Registros
                if (Validacion.ValidaOrigenDatos(dtFichasFacturas))
                {
                    //Cargando GridView
                    Controles.CargaGridView(gvFichasFacturas, dtFichasFacturas, "Id", "", true, 1);

                    //Añadiendo Tabla a Session
                    Session["DS"] = OrigenDatos.AñadeTablaDataSet((DataSet)Session["DS"], dtFichasFacturas, "Table2");

                    //Mostrando Totales
                    gvFichasFacturas.FooterRow.Cells[3].Text = string.Format("{0:C2}", (((DataSet)Session["DS"]).Tables["Table2"].Compute("SUM(MontoAplicado)", "")));
                }
                else
                {
                    //Inicializando GridView
                    Controles.InicializaGridview(gvFichasFacturas);

                    //Eliminando Tabla de Session
                    Session["DS"] = OrigenDatos.EliminaTablaDataSet((DataSet)Session["DS"], "Table2");

                    //Mostrando Totales
                    gvFichasFacturas.FooterRow.Cells[3].Text = "0.00";
                }
            }
        }
        /// <summary>
        /// Realiza el proceso de validación de formatos (expresiones regulares de cuentas ordenante/beneficiaria)
        /// </summary>
        private RetornoOperacion validaCuentasSeleccionadasFormaPago()
        {
            //Declarando objeto de resultado
            RetornoOperacion resultado = new RetornoOperacion(1);

            //Validando fecha de incio de implementación de CFDI de Recepción de Pagos
            DateTime inicio_cfdi_pagos = Fecha.ConvierteStringDateTime(SAT_CL.CapaNegocio.m_capaNegocio.RegresaVariableCatalogoBD("Fecha Inicio CFDI Comprobante Pago 1.0", ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_Sesion"]).id_compania_emisor_receptor));

            //Si ya se ha configurado una fecha de inicio de emisión de CFDI de Recepción de Pagos
            if (!Fecha.EsFechaMinima(inicio_cfdi_pagos))
            {
                //Si la fecha del pago a registrar es mayor o igual a la fecha de implementación
                if (Convert.ToDateTime(txtFechaEI.Text).CompareTo(inicio_cfdi_pagos) > 0)
                {
                    //Instanciando forma de pago seleccionada
                    using (SAT_CL.FacturacionElectronica33.FormaPago fp = new SAT_CL.FacturacionElectronica33.FormaPago(Convert.ToInt32(ddlMetodoPago.SelectedValue)))
                    {
                        //SI existe la forma de pago
                        if (fp.habilitar)
                        {
                            //Creando validador de expresión regular
                            System.Text.RegularExpressions.Regex reCtaOrd = new System.Text.RegularExpressions.Regex("^" + fp.patron_cta_ordenante + "$");
                            System.Text.RegularExpressions.Regex reCtaBen = new System.Text.RegularExpressions.Regex("^" + fp.patron_cta_beneficiario + "$");

                            switch (fp.clave)
                            {
                                //_02_Cheque
                                case "02":
                                //_03_TransaferenciaElectronica
                                case "03":
                                //_04_TarjetaCredito
                                case "04":
                                //_05_MonederoElectronico
                                case "05":
                                //_28_TarjetaDebito
                                case "28":
                                //_29_TarjetaServicios
                                case "29":
                                    //Validando cuenta de origen (ACORDE AL FORMATO DEL SAT)
                                    if (ddlCuentaOrigen.SelectedValue != "0" && fp.patron_cta_ordenante != "")
                                    {
                                        //Recuperando elemento de texto
                                        if (!reCtaOrd.IsMatch(new SAT_CL.Bancos.CuentaBancos(Convert.ToInt32(ddlCuentaOrigen.SelectedValue)).num_cuenta))
                                            resultado = new RetornoOperacion("La Cuenta Ordenante no tiene el formato requerido por el SAT.");
                                    }

                                    //Validando cuenta Beneficiario (SIEMPRE SERÁ REQUERIDA ACORDE AL FORMATO DEL SAT)
                                    if (resultado.OperacionExitosa && fp.patron_cta_beneficiario != "")
                                    {
                                        if (ddlCuentaDestino.SelectedValue == "0")
                                            resultado = new RetornoOperacion("La Cuenta del beneficiario es Requerida.");
                                        //Recuperando elemento de texto
                                        else if (!reCtaBen.IsMatch(new SAT_CL.Bancos.CuentaBancos(Convert.ToInt32(ddlCuentaDestino.SelectedValue)).num_cuenta))
                                            resultado = new RetornoOperacion("La Cuenta del Beneficiario no tiene el formato requerido por el SAT.");
                                    }
                                    break;
                                //_06_DineroElectronico
                                case "06":
                                    //Validando cuenta de origen (ACORDE AL FORMATO DEL SAT)
                                    if (ddlCuentaOrigen.SelectedValue != "0" && fp.patron_cta_ordenante != "")
                                    {
                                        //Recuperando elemento de texto
                                        if (!reCtaOrd.IsMatch(new SAT_CL.Bancos.CuentaBancos(Convert.ToInt32(ddlCuentaOrigen.SelectedValue)).num_cuenta))
                                            resultado = new RetornoOperacion("La Cuenta Ordenante no tiene el formato requerido por el SAT.");
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                            resultado = new RetornoOperacion("La forma de Pago seleccionada no pudo ser encontrada.");
                    }
                }
            }

            //Devolviendo resultado
            return resultado;
        }

        #endregion

        protected void LinkButton1_Click(object sender, EventArgs e)
        {/*
            //Realizando exportación a CFDI
            System.Collections.Generic.List<int> lista = new System.Collections.Generic.List<int>();
            lista.Add(Convert.ToInt32(Session["id_registro"]));
            SAT_CL.Bancos.EgresoIngreso.ImportarFIComprobanteV3_3(lista, 0, 22, 1);*/
        }
    }
}