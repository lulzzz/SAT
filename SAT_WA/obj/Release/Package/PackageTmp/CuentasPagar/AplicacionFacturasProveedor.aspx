<%@ Page Title="Aplicación Facturas Proveedor" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="AplicacionFacturasProveedor.aspx.cs" Inherits="SAT.CuentasPagar.AplicacionFacturasProveedor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraAplicacionFacturaProveedorPago();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraAplicacionFacturaProveedorPago() {
            $(document).ready(function () {

                //Cargando Catalogo de Autocompletado
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

                //Declarando Función de Validación de Busqueda
                var validaBusquedaFacturas = function () {
                    //Añadiendo Validador
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2;
                }

                //Añadiendo Función de Validación al Evento Click del Boton
                $("#<%=btnBuscar.ClientID%>").click(validaBusquedaFacturas);
                //Añadiendo Función de Validación al Evento Click del Boton
                $("#<%=btnAplicarFacturas.ClientID%>").click(validaBusquedaFacturas);
            });
        }

        //Invocando Función de Configuración
        ConfiguraAplicacionFacturaProveedorPago();
    </script>
    <div id="encabezado_forma">
        <h1>Aplicación de Facturas de Proveedor</h1>
    </div>
    <div class="contenedor_controles">
        <div class="columna2x">
            <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCliente">Proveedor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCliente" runat="server" TabIndex="1" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFolio">Folio</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolio" runat="server" TabIndex="2" CssClass="textbox_50px validate[custom[onlyNumberSp]]" MaxLength="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUUID">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUID" runat="server" TabIndex="3" CssClass="textbox2x" MaxLength="36"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAplicarFacturas" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAplicarFacturas" runat="server" Text="Aplicar" CssClass="boton_cancelar"
                                    OnClick="btnAplicarFacturas_Click" TabIndex="5" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="4" CssClass="boton"
                                    OnClick="btnBuscar_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvFacturas" />
                        <asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
                        <asp:AsyncPostBackTrigger ControlID="btnContinuarAplicacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="header_seccion">
            <h2>Facturas Por Aplicar</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoFacturas">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoFacturas" runat="server" CssClass="dropdown" TabIndex="5"
                            OnSelectedIndexChanged="ddlTamanoFacturas_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFacturas">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenadoFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoFacturas" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvFacturas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportarFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarFacturas" runat="server" Text="Exportar" TabIndex="6" OnClick="lnkExportarFacturas_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarFacturas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvFacturas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvFacturas" runat="server" AllowPaging="true" AllowSorting="true"
                        OnPageIndexChanging="gvFacturas_PageIndexChanging" OnSorting="gvFacturas_Sorting"
                        PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkTodosFactura" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkTodosFactura_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVariosFactura" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkTodosFactura_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" />
                            <asp:BoundField DataField="TipoServicio" HeaderText="Tipo de Servicio" SortExpression="TipoServicio" />
                            <asp:BoundField DataField="Anticipo" HeaderText="Anticipo" SortExpression="Anticipo" />
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                            <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Trasladado" HeaderText="Imp. Trasladado" SortExpression="Trasladado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Retenido" HeaderText="Imp. Retenido" SortExpression="Retenido" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPendiente" HeaderText="Monto Pendiente" SortExpression="MontoPendiente" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPorAplicar" HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Monto Preferente" SortExpression="MontoPreferente">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMXA" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
                                        Text='<%# Eval("MontoPreferente", "{0:0.00}") %>' Enabled="false" TabIndex="9" Style="text-align: right">
                                    </asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MP2" HeaderText="MP2" SortExpression="MP2" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCambiar" runat="server" Text="Cambiar" OnClick="lnkCambiar_Click" CommandName="Cambiar" Enabled="false"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturas" />
                    <asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
                    <asp:AsyncPostBackTrigger ControlID="btnContinuarAplicacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Confirmación de Facturas de Anticipo -->
    <div id="contenedorVentanaConfirmacionAnticipo" class="modal">
        <div id="ventanaConfirmacionAnticipo" class="contenedor_ventana_confirmacion_arriba" style="width:650px">
            <div class="header_seccion">
                <img src="../Image/ExclamacionRoja.png" />
                <h2>Existen Anticipos de Servicio</h2>
            </div>
            <div class="columna3x">
                <div class="grid_seccion_completa_100px_altura">
                    <asp:UpdatePanel ID="uplblFacturas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblFacturas" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
                            <asp:AsyncPostBackTrigger ControlID="btnContinuarAplicacion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamanoFI">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoFI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoFI" runat="server" CssClass="dropdown" TabIndex="3" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoFI_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblOrdenadoFI">Ordenado</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblOrdenadoFI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblOrdenadoFI" runat="server"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="4" OnClick="lnkExportar_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_100px_altura">
                    <asp:UpdatePanel ID="uplblConfirmacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAnticiposProveedor" runat="server" AllowPaging="true" AllowSorting="true"
                                OnPageIndexChanging="gvAnticiposProveedor_PageIndexChanging" OnSorting="gvAnticiposProveedor_Sorting"
                                PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
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
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="NoEgreso" HeaderText="No. Egreso" SortExpression="NoEgreso" />
                                    <asp:BoundField DataField="NoAnticipo" HeaderText="No. Anticipo" SortExpression="NoAnticipo" />
                                    <asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidacion" SortExpression="NoLiquidacion" />
                                    <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                    <asp:BoundField DataField="MontoDisponible" HeaderText="Monto Disponible" SortExpression="MontoDisponible" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
                            <asp:AsyncPostBackTrigger ControlID="btnContinuarAplicacion" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoFI" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnContinuarAplicacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnContinuarAplicacion" runat="server" CssClass="boton" Text="Continuar"
                                    OnClick="btnContinuarAplicacion_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarAplicacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarAplicacion" runat="server" CssClass="boton_cancelar" Text="Cancelar"
                                    OnClick="btnCancelarAplicacion_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
