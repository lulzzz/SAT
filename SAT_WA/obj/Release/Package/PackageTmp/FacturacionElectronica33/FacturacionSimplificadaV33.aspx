<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FacturacionSimplificadaV33.aspx.cs" Inherits="SAT.FacturacionElectronica33.FacturacionSimplificadaV33" %>

<%@ Register Src="~/UserControls/wucProducto.ascx" TagName="wucProducto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucFacturadoConceptoV33.ascx" TagName="wucFacturadoConceptoV33" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucAddendaComprobante.ascx" TagName="wucAddendaComprobante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="uc1" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucEncabezadoServicio.ascx" TagPrefix="uc1" TagName="wucEncabezadoServicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQuerySaldosDetalle();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraJQuerySaldosDetalle() {
            //Inicializando Función
            $(document).ready(function () {
                //Cargando Catalogo de Autocompletado
                $("#<%=txtClienteFS.ClientID%>").autocomplete({
                        source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
                });
                //Cargando Control DateTimePicker "Fecha Inicio"
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Cargando Control DateTimePicker "Fecha Fin"
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Función de validación de campos
                var validacionCancelacion = function (evt) {
                    var isValidP1 = !$("#<%=txtMotivo.ClientID%>").validationEngine('validate');
                    return isValidP1;
                };
                //Boton Cancelar
                $("#<%= btnAceptarCancelacionCFDI.ClientID%>").click(validacionCancelacion);
                //Configurando Validación de Controles
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Añadiendo Controles a la Validación
                    var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtClienteFS.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtFolioFS.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4;
                });
                //Autocompleta del Número Económico
                $("#<%=txtEconomico.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
            });
        }
        //Invocando Función de Configuración
        ConfiguraJQuerySaldosDetalle();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Facturación x Servicio</h1>
    </div>
    <div class="contenedor_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="UPtxtClienteFS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtClienteFS" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="2" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Fechas" TabIndex="3" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="4" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtServicio">No. Servicio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="UPtxtServicioFS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtServicioFS" runat="server" CssClass="textbox " TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPorte">C. Porte</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtPorte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPorte" runat="server" CssClass="textbox " TabIndex="6" MaxLength="30"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtReferencia">Referencia</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="7" MaxLength="500"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEconomico">No. Económico</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtEconomico" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <asp:CheckBox ID="chkSinRegistrar" runat="server" Checked="true" Text="Sin Registrar" CssClass="label" ToolTip="Muestra los Servicios Sin Proceso alguno de FE" TabIndex="8" />
                </div>
                <div class="etiqueta">
                    <asp:CheckBox ID="chkRegistradas" runat="server" Checked="true" Text="Registrados" CssClass="label" ToolTip="Muestra los Servicios Registrados para FE (Sin Timbrar)" TabIndex="9" />
                </div>
                <div class="etiqueta">
                    <asp:CheckBox ID="chkTimbradas" runat="server" Checked="true" Text="Timbrados" CssClass="label" ToolTip="Muestra los Servicios Timbrados en FE" TabIndex="10" />
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFolioFS">Folio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFolioFS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolioFS" runat="server" CssClass="textbox_50px validate[custom[onlyNumberSp]]" TabIndex="11" MaxLength="12"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="12" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="13"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="14" CommandName="ServicioxFacturar" OnClick="lkbExportarServiciosxFacturar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvServicioxFacturar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvServicioxFacturar" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="15"
                        OnPageIndexChanging="gvServicioxFacturar_PageIndexChanging" OnSorting="gvServicioxFacturar_Sorting"
                        PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="NoFactura" HeaderText="No. Factura" SortExpression="NoFactura" Visible="false" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Porte" />
                            <asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbServicio" runat="server" CommandName="servicio" OnClick="lkbOtros_Click" Text='<%#Eval("Servicio") %>' ToolTip="Muestra el Producto"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NoViaje" SortExpression="NoViaje">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbReferenciaServ" runat="server" CommandName="referenciasServicio" OnClick="lkbOtros_Click" Text='<%#Eval("NoViaje") %>' ToolTip="Ver y Añadir Referencias de Servicio"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Porte" SortExpression="Porte">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbPorte" runat="server" CommandName="porte" OnClick="lkbOtros_Click" Text='<%#Eval("Porte") %>' ToolTip="Cambiamos Referencias del Viaje"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                            <asp:BoundField DataField="FechaTermino" HeaderText="Fecha Termino Servicio" SortExpression="FechaTermino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
                            <asp:BoundField DataField="FacturaE" HeaderText="Factura Electronica" SortExpression="FacturaE" />
                            <asp:BoundField DataField="EstatusFE" HeaderText="Estatus Fac. Electronica" SortExpression="EstatusFE" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha Timbrado" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:TemplateField HeaderText="Sub Total" SortExpression="SubTotal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerConceptos" runat="server" OnClick="lkbVerConceptos_Click" Text='<%# string.Format("{0:C2}", Eval("SubTotal")) %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Trasladado" HeaderText="Imp. Trasladado" SortExpression="Trasladado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Retenido" HeaderText="Imp. Retenido" SortExpression="Retenido" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Registró FE" SortExpression="RegistroFE">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbRegistroFE" runat="server" CommandName='<%# Eval("RegistroFE") %>' OnClick="lkbRegistroFE_Click" Text='<%# Eval("RegistroFE") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Timbrar FE" SortExpression="TimbrarFE">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbTimbrarFE" runat="server" CommandName='<%# Eval("TimbrarFE") %>' OnClick="lkbTimbrarFE_Click" Text='<%# Eval("TimbrarFE") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Otros FE" SortExpression="OtrosFE">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAddenda" runat="server" OnClick="lkbAddenda_Click" Text="Addenda"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbComentario" runat="server" Text="Comentario" OnClick="lkbComentario_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Accesorios" SortExpression="Accesorios">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:UpdatePanel runat="server" ID="uplkbPDF" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbPDF" runat="server" CommandName="PDF" Text="PDF" OnClick="lkbAccesorios_Click"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbPDF" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel runat="server" ID="uplkbXML" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbXML" runat="server" CommandName="XML" Text="XML" OnClick="lkbAccesorios_Click"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbXML" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel runat="server" ID="uplkbVerComprobante" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbVerComprobante" runat="server" CommandName="VerComprobante" Text="Comprobante" OnClick="lkbAccesorios_Click"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TimbradoPor" HeaderText="Timbrado Por" SortExpression="TimbradoPor" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarCFDI" />
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrarFE" />
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                    <asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
                    <asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacionCFDI" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
                    <asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Ventana de Edición de Conceptos (Detalles de Factura) -->
    <div id="contenedorVentanaEdicionConceptos" class="modal">
        <div id="ventanaEdicionConceptos" class="contenedor_modal_seccion_completa_arriba" style="width: 1200px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarVentanaEC" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarVentanaEC" runat="server" Text="Cerrar" CommandName="edicionConceptos" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/FacturacionCargos.png" />
                <h2>Detalles de la Factura</h2>
            </div>
            <asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <tectos:wucFacturadoConceptoV33 ID="ucFacturadoConcepto" runat="server" OnClickGuardarFacturaConcepto="wucFacturadoConceptoV33_ClickGuardarFacturaConcepto" OnClickEliminarFacturaConcepto="wucFacturadoConceptoV33_ClickEliminarFacturaConcepto" TabIndex="9" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Ventana Confirmación Registra Factura Electronica -->
    <div id="contenidoConfirmacionRegistarFacturacionElectronica" class="modal">
        <div id="confirmacionRegistarFacturacionElectronica" class="contenedor_ventana_confirmacion">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarRegistarFacturacionElectronica" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarRegistarFacturacionElectronica" CommandName="registrarFacturacionElectronica" runat="server" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <h3>Registrar Factura</h3>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlTipoComrobante">Tipo</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlTipoComrobante" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoComrobante" Enabled="false" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlFormaPago">Forma de Pago</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlFormaPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFormaPago" runat="server"
                                    CssClass="dropdown2x">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlSucursal">Sucursal</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSucursal" runat="server"
                                    CssClass="dropdown2x">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlMetodoPago">Método de Pago</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlMetodoPago" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged"
                                    CssClass="dropdown2x">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlUsoCFDI">Uso CFDI</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlUsoCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlUsoCFDI" runat="server"
                                    CssClass="dropdown2x">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnRegistrarFacturaElectronica" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnRegistrarFacturaElectronica" runat="server" OnClick="btnRegistrarFacturaElectronica_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Referencias -->
    <div id="contenedorVentanaReferencias" class="modal">
        <div id="ventanaReferencias" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarDev" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarReferencias" runat="server" CommandName="referenciasRegistro" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="12">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/EnvioRecepcion.png" />
                <h2>Referencias Viaje</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTamanoReferencias">Mostrar</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamanoReferencias" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoReferencias" runat="server" TabIndex="13" OnSelectedIndexChanged="ddlTamanoReferencias_SelectedIndexChanged"
                                CssClass="dropdown" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoReferencias">Ordenado</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenadoReferencias" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoReferencias" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvReferencias" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvReferencias" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvReferencias" runat="server" AllowPaging="true" AllowSorting="true"
                            CssClass="gridview" ShowFooter="true" TabIndex="25" OnSorting="gvReferencias_Sorting"
                            OnPageIndexChanging="gvReferencias_PageIndexChanging" AutoGenerateColumns="false">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:TemplateField SortExpression="Tipo">
                                    <FooterTemplate>
                                        <asp:Label ID="lblContadorTipo" runat="server" Text="0"></asp:Label>
                                        <br />
                                        Seleccionados
                                    </FooterTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkTipoTodos" runat="server" AutoPostBack="True" Checked="true" CssClass="LabelResalta"
                                            OnCheckedChanged="chkTipoTodos_CheckedChanged" Text="Todos" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSeleccionTipo" runat="server" Checked="true" AutoPostBack="True" OnCheckedChanged="chkSeleccionTipo_CheckedChanged" />
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoReferencias" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnRegistrarFE" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnRegistrarFE" runat="server" OnClick="btnRegistrarFE_Click" CssClass="boton" Text="Registrar" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Eliminación CFDI -->
    <div id="contenidoConfirmacionEliminarCFDI" class="modal">
        <div id="confirmacionEliminarCFDI" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Eliminar Factura Electrónica</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea eliminar la Factura Electrónica ?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarEliminarCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarEliminarCFDI" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarEliminarCFDI_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarEliminarCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarEliminarCFDI" runat="server" OnClick="btnAceptarEliminarCFDI_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Timbrar Factura -->
    <div id="contenidoConfirmacionTimbrarFacturacionElectronica" class="modal">
        <div id="confirmaciontimbrarFacturacionElectronica" class="contenedor_ventana_confirmacion">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarFacturacionElectronica" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarTimbrarFacturacionElectronica" runat="server" CommandName="timbrarFacturacionElectronica" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <h3>Timbrar Factura</h3>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtSerie">Serie</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtSerieFS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSerieFS" Text="" runat="server" CssClass="textbox validate[custom[onlyLetterSp]]" MaxLength="10">
                                </asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="control2x">
                        <asp:UpdatePanel ID="upchkOmitirAddenda" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkOmitirAddenda" runat="server" Text="Facturar sin 'Addenda'." />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="control" style="width: auto">
                        <asp:UpdatePanel ID="lbllblTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblTimbrarFacturacionElectronica" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarTimbrarFacturacionElectronica" runat="server" OnClick="btnAceptarTimbrarFacturacionElectronica_Click" CssClass="boton" Text="Timbrar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Cancelación CFDI -->
    <div id="contenidoConfirmacionCancelacionCFDI" class="modal">
        <div id="confirmacionCancelacionCFDI" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Cancelar Factura Electrónica</h3>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="upmtvCancelacionCFDI" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:MultiView ID="mtvCancelacionCFDI" runat="server">
                            <asp:View ID="vwFacturaSinAplicaciones" runat="server">
                                <div class="renglon2x">
                                    <label class="mensaje_modal">
                                        ¿Realmente desea Cancelar la Factura Electrónica?
                                    </label>
                                </div>
                            </asp:View>
                            <asp:View ID="vwFacturaConAplicaciones" runat="server">
                                <div class="renglon2x">
                                    <label class="mensaje_modal">
                                        Las siguientes Aplicaciones seran Eliminadas. ¿Desea Continuar?
                                    </label>
                                </div>
                                <div class="grid_seccion_completa_150px_altura">
                                    <asp:UpdatePanel ID="upgvAplicaciones" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvAplicaciones" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false"
                                                CssClass="gridview" ShowFooter="true" Width="100%">
                                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                                <FooterStyle CssClass="gridviewfooter" />
                                                <HeaderStyle CssClass="gridviewheader" />
                                                <RowStyle CssClass="gridviewrow" />
                                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                                <Columns>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                                    <asp:BoundField DataField="FacturaFicha" HeaderText="Factura/Ficha" SortExpression="FacturaFicha" />
                                                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                                                    <asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                    <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="renglon2x"></div>
                                <div class="renglon2x"></div>
                                <div class="renglon2x"></div>
                                <div class="renglon2x"></div>
                                <div class="renglon2x"></div>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtMotivo">Motivo:</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtMotivo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine" Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x"></div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarCancelacionCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarCancelacionCFDI" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarCancelacionCFDI_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarCancelacionCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarCancelacionCFDI" runat="server" OnClick="btnAceptarCancelacionCFDI_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Addenda -->
    <div id="contenidoConfirmacionAddenda" class="modal">
        <div id="confirmacionAddenda" class="contenedor_ventana_confirmacion">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarAddenda" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarAddenda" runat="server" CommandName="confirmacionAddenda" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <h3>Addenda</h3>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlAddenda">Tipo</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlAddenda" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlAddenda" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarAddenda" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarAddenda" runat="server" OnClick="btnAceptarAddenda_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Comprobante Addenda -->
    <div id="contenidoConfirmacionWucComprobanteAddenda" class="modal">
        <div id="confirmacionWucComprobanteAddenda" class="contenedor_modal_seccion_completa_arriba" style="top: 15px; width: 950px">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarwucAddendaComprobante" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarwucAddendaComprobante" runat="server" CommandName="addenda" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna">
                <asp:UpdatePanel ID="upwucAddendaComprobante" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:wucAddendaComprobante ID="wucAddendaComprobante" OnClickEliminar="wucAddendaComprobante_ClickEliminar" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarAddenda" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL DE REFERENCIAS DE SERVICIO -->
    <div id="referenciasServicioModal" class="modal">
        <div id="referenciasServicio" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lnkCerrar_Click" CommandName="referenciasServicio">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Clasificacion.png" />
                <h2>Referencias Servicio</h2>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true"
                            OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
                            OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- Ventana Comentario -->
    <div id="contenidoComentario" class="modal">
        <div id="confirmacionComentario" class="contenedor_ventana_confirmacion">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrar" runat="server" CommandName="comentario" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtComentario">Comentario</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtComentario" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtComentario" runat="server" Text=" " CssClass="textbox2x" MaxLength="500" TabIndex="1"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarComentario" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarComentario" runat="server" OnClick="btnAceptarComentario_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Comentario Producto-->
    <div id="contenidoProducto" class="modal">
        <div id="Producto" class="contenedor_modal_seccion_completa_arriba">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarProducto" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarProducto" runat="server" CommandName="producto" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucProducto" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucProducto ID="wucProducto" runat="server"
                        TabIndex="23" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- VENTANA MODAL DE ACTUALIZACIÓN DE ENCABEZADO DE SERVICIO -->
    <div id="encabezadoServicioModal" class="modal">
        <div id="encabezadoServicio" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarEncabezadoServicio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarEncabezadoServicio" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="EncabezadoServicio">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucReferenciaServicio" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucEncabezadoServicio ID="wucEncabezadoServicio" runat="server"
                        OnClickGuardarReferencia="wucEncabezadoServicio_ClickGuardarReferencia" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicioxFacturar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
