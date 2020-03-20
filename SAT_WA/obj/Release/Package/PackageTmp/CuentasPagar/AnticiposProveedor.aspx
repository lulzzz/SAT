<%@ Page Title="Anticipos Proveedor" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="AnticiposProveedor.aspx.cs" Inherits="SAT.CuentasPagar.AnticiposProveedor" %>
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
ConfiguraAnticipoProveedor();
}
}
//Declarando Función de Configuración
function ConfiguraAnticipoProveedor() {
$(document).ready(function () {

//Cargando Catalogo de Autocompletado
$("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'});

//Declarando Función de Validación de Busqueda
var validaBusquedaFichas = function () {
//Añadiendo Validador
var isValid1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
//Devolviendo Resultado Obtenido
return isValid1;
}

//Declarando Función de Validación de Busqueda
var validaBusquedaFacturas = function () {
//Añadiendo Validador
var isValid1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
//Devolviendo Resultado Obtenido
return isValid1 && isValid2;
}

//Añadiendo Función de Validación al Evento Click del Boton
$("#<%=btnBuscar.ClientID%>").click(validaBusquedaFichas);

//Añadiendo Función de Validación al Evento Click del Boton
$("#<%=btnBuscarFacturas.ClientID%>").click(validaBusquedaFacturas);                
});
}

//Invocando Función de Configuración
ConfiguraAnticipoProveedor();
</script>
<div id="encabezado_forma">
<h1>Aplicación de Pagos</h1>
</div>
<div class="contenedor_controles">
<div class="columna2x">
<asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProveedor" runat="server" TabIndex="1" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="2" CssClass="boton"
OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorFicha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFicha" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:Panel>
            
</div>
</div>
<div class="contenedor_controles">
<div class="header_seccion">
<h2>Anticipos a Proveedor</h2>
</div>
<div class="renglon3x">
            
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFI">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFI" runat="server" CssClass="dropdown" TabIndex="3" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFI_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFI" runat="server"></asp:Label></b>
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
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvAnticiposProveedor" runat="server" UpdateMode="Conditional">
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
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Facturas" HeaderText="Facturas" SortExpression="Facturas" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkVerFacturasAplicadas" runat="server" Text='<%# Eval("MontoAplicado", "{0:C2}") %>'
OnClick="lnkVerFacturasAplicadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MontoDisponible" HeaderText="Monto Disponible" SortExpression="MontoDisponible" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionar" runat="server" Text="Seleccionar" OnClick="lnkSeleccionar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFI" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_controles">
<div class="header_seccion">
<h2>Facturas Por Aplicar</h2>
</div>
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbTodos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbTodos" runat="server" Text="Todos" Checked="false" GroupName="General"
OnCheckedChanged="rbFacturas_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbFacturas" runat="server" Text="Facturas de Anticipo" Checked="true" GroupName="General"
OnCheckedChanged="rbFacturas_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbTodos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="txtUUID">UUID</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUUID" runat="server" CssClass="textbox" TabIndex="6" MaxLength="36"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtSerie">Serie</label>
</div>
<div class="control_80px">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_50px" TabIndex="7" MaxLength="10"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtFolio">Folio</label>
</div>
<div class="control_80px">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_50px validate[custom[onlyNumberSp]]" TabIndex="8" MaxLength="12"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
            
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="lblSaldoFI">Saldo:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblSaldoFI" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblPorAplicar">Por Aplicar:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblPorAplicar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblPorAplicar" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblSaldoFinal">Saldo Final:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFinal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblSaldoFinal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="control2x">
<asp:UpdatePanel ID="uplblErrorFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFactura" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFacturas" runat="server" Text="Buscar" TabIndex="7" CssClass="boton"
OnClick="btnBuscarFacturas_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAplicarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAplicarFacturas" runat="server" Text="Aplicar" CssClass="boton_cancelar"
OnClick="btnAplicarFacturas_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFacturas">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacturas" runat="server" CssClass="dropdown" TabIndex="5"
OnSelectedIndexChanged="ddlTamanoFacturas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFacturas">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFacturas" runat="server"></asp:Label></b>
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
<asp:PostBackTrigger ControlID="lnkExportar" />
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
<asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Trasladado" HeaderText="Imp. Trasladado" SortExpression="Trasladado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Retenido" HeaderText="Imp. Retenido" SortExpression="Retenido" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkVerFichasAplicadas" runat="server" Text='<%# Eval("MontoAplicado", "{0:C2}") %>'
OnClick="lnkVerFichasAplicadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MontoPendiente" HeaderText="Monto Pendiente" SortExpression="MontoPendiente" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SaldoFactura" HeaderText="Saldo de la Factura" SortExpression="SaldoFactura" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:TextBox ID="txtMXA" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" Text='<%# Eval("MontoPorAplicar","{0:0.00}") %>'
Enabled="false" TabIndex="9" style="text-align:right">
                                        
</asp:TextBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MPA2" HeaderText="MPA2" SortExpression="MPA2" Visible="false" />
<asp:BoundField DataField="AplicacionesConfirmadas" HeaderText="AplicacionesConfirmadas" SortExpression="AplicacionesConfirmadas" Visible="false" />
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
<asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFL" />
<asp:AsyncPostBackTrigger ControlID="rbTodos" />
<asp:AsyncPostBackTrigger ControlID="rbFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- Ventana encargada de mostrar las Facturas Ligadas -->
<div id="contenidoVentanaFacturasLigadas" class="modal">
<div id="ventanaFacturasLigadas" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFL" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFL" runat="server" CommandName="FacturasLigadas" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Facturas Ligadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFL">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFL" runat="server" CssClass="dropdown" TabIndex="5"
OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFL">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFL" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFL" runat="server" Text="Exportar" TabIndex="6" OnClick="lnkExportarFL_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFL" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AutoGenerateColumns="false" Width="100%"
OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging" OnSorting="gvFacturasLigadas_Sorting"
CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true">
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
<asp:BoundField DataField="FacturaFicha" HeaderText="Serie-Folio" SortExpression="FacturaFicha" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Aplicaciones y Relaciones -->
<div id="contenidoVentanaFichasFacturas" class="modal">
<div id="ventanaFichasFacturas" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFF" runat="server" CommandName="FichasFacturas" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Aplicaciones y Relaciones de la Factura</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFF">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFF" runat="server" CssClass="dropdown" TabIndex="10"
OnSelectedIndexChanged="ddlTamanoFF_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFF">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFF" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="11" CommandName="FichasFacturas" OnClick="lnkExportarFF_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting"
OnRowDataBound="gvFichasFacturas_RowDataBound" CssClass="gridview" AllowSorting="true" 
AllowPaging="true" ShowFooter="true">
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
<asp:BoundField DataField="Egreso" HeaderText="No. Egreso" SortExpression="Egreso" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Viajes" SortExpression="Viajes">
<ItemTemplate>
<asp:LinkButton ID="lnkServiciosEntidad" runat="server" OnClick="lnkServiciosEntidad_Click" Text='<%# Eval("Viajes") %>'></asp:LinkButton>
<asp:Label ID="lblServiciosEntidad" runat="server" Text='<%# Eval("Viajes") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Fichas Aplicadas -->
<div id="contenidoVentanaServiciosEntidad" class="modal">
<div id="ventanaServiciosEntidad" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarSE" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarSE" runat="server" CommandName="ServiciosEntidad" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Servicios de la Liquidación</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoSE">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoSE" runat="server" CssClass="dropdown" TabIndex="10"
OnSelectedIndexChanged="ddlTamanoSE_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoSE">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoSE" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosEntidad" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarSE" runat="server" Text="Exportar" TabIndex="11" OnClick="lnkExportarSE_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarSE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvServiciosEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosEntidad" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvServiciosEntidad_PageIndexChanging" OnSorting="gvServiciosEntidad_Sorting"
CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoSE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Content>
