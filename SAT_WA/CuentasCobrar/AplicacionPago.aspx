<%@ Page Title="Aplicación de Pagos" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="AplicacionPago.aspx.cs" Inherits="SAT.CuentasCobrar.AplicacionPago" %>

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
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraAplicacionPago();
}
}
//Declarando Función de Configuración
function ConfiguraAplicacionPago() {
$(document).ready(function () {

//Cargando Catalogo de Autocompletado
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

//Declarando Función de Validación de Busqueda
var validaBusquedaFichas = function () {
//Añadiendo Validador
    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtNoFicha.ClientID%>").validationEngine('validate');
//Devolviendo Resultado Obtenido
return isValid1 && isValid2;
}

//Declarando Función de Validación de Busqueda
var validaBusquedaFacturas = function () {
//Añadiendo Validador
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
//Devolviendo Resultado Obtenido
return isValid1 && isValid2;
}

//Añadiendo Función de Validación al Evento Click del Boton
$("#<%=btnBuscar.ClientID%>").click(validaBusquedaFichas);

//Añadiendo Función de Validación al Evento Click del Boton
$("#<%=btnBuscarFacturas.ClientID%>").click(validaBusquedaFacturas);
//Scroll de GV de Fichas    
$("#<%=gvFichasIngreso.ClientID%>").gridviewScroll({
width: 1200,
height: 400
});
//Scroll de GV de Facturas    
$("#<%=gvFacturas.ClientID%>").gridviewScroll({
width: 1200,
height: 400
});
//Scroll de GV de Aplicaciones    
$("#<%=gvFichasFacturas.ClientID%>").gridviewScroll({
width: 1000,
height: 400
});
});
}

//Invocando Función de Configuración
ConfiguraAplicacionPago();
</script>
<div id="encabezado_forma">
<h1>Aplicación de Pagos</h1>
</div>
<div class="contenedor_controles">
<asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="txtCliente">Cliente</label>
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
<asp:UpdatePanel ID="uplblErrorFicha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFicha" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
</div>
<div class="contenedor_controles">
<div class="header_seccion">
<h2>Fichas de Ingreso</h2>
</div>
<div class="renglon3x" style="float:left;">
<div class="etiqueta_50px">
<label for="txtNoFicha">No. Ficha</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtNoFicha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoFicha" runat="server" CssClass="textbox_100px validate[custom[positiveNumber]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtNumOperacion">Num. Operación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNumOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNumOperacion" runat="server" CssClass="textbox" TabIndex="3"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="3" CssClass="boton"
OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div><br />
<div class="renglon3x" style="float:left;">
<div class="etiqueta_80px">
<label for="lblCapturadas">Capturadas:</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblCapturadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblCapturadas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<label for="lblAplicadasPar">A. Parciales:</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblAplicadasPar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblAplicadasPar" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<label for="lblTotFichas">Total Fichas:</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblTotFichas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblTotFichas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x" style="float:left;">
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
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
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
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvFichasIngreso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasIngreso" runat="server" AllowPaging="True" AllowSorting="True"
OnPageIndexChanging="gvFichasIngreso_PageIndexChanging" OnSorting="gvFichasIngreso_Sorting"
PageSize="250" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gvFichasIngreso_RowDataBound">
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
<asp:BoundField DataField="NoFicha" HeaderText="No. Ficha" SortExpression="NoFicha" />
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:LinkButton ID="lnkEstatusFI" runat="server" Text='<%#Eval("Estatus") %>' CommandName="CambiarEstatusFI" OnClick="lnkEstatusFI_Click">LinkButton</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="NumOperacion" HeaderText="Num. Operación" SortExpression="NumOperacion" />
<asp:BoundField DataField="FechaCaptura" HeaderText="Fecha Captura" SortExpression="FechaCaptura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
    <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkVerFacturasAplicadas" runat="server" Text='<%# Eval("MontoAplicado", "{0:C2}") %>'
OnClick="lnkVerFacturasAplicadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MontoDisponible" HeaderText="Monto Disponible" SortExpression="MontoDisponible" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
    <ItemStyle HorizontalAlign="Right" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarCambiarEstatusFI" />
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
<div class="etiqueta_50px">
<label for="txtFacturaGlobal">Factura G.</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtFacturaGlobal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFacturaGlobal" runat="server" CssClass="textbox_100px" TabIndex="5" MaxLength="50"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
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
<label for="txtReferencia">Referencia</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" TabIndex="7"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon3x">
<div class="etiqueta">
<label for="lblSaldoFI">Saldo Anterior FI:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblSaldoFI" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblPorAplicar">Por Aplicar:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblPorAplicar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblPorAplicar" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblSaldoFinal">Saldo Final:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFinal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblSaldoFinal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
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
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFacturas" runat="server" Text="Buscar" TabIndex="10" CssClass="boton"
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
<div class="columna">
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtFolio">Folio</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_50px validate[custom[onlyNumberSp]]" TabIndex="8" MaxLength="12"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="upchkSoloTimbradas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkSoloTimbradas" runat="server" Checked ="true" CssClass="label_negrita" Text="Sólo Facturas Timbradas" TabIndex="9" />
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
<asp:DropDownList ID="ddlTamanoFacturas" runat="server" CssClass="dropdown" TabIndex="11"
OnSelectedIndexChanged="ddlTamanoFacturas_SelectedIndexChanged" AutoPostBack="true">
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
<asp:LinkButton ID="lnkExportarFacturas" runat="server" Text="Exportar" TabIndex="12" OnClick="lnkExportarFacturas_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturas" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvFacturas_PageIndexChanging" OnSorting="gvFacturas_Sorting" TabIndex="13"
PageSize="250" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
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
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Folio" HeaderText="Serie y Folio" SortExpression="Folio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:BoundField DataField="NoViaje" HeaderText="NoViaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" />
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
<asp:TemplateField HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkVerFichasAplicadas" runat="server" Text='<%# Eval("MontoAplicado", "{0:C2}") %>'
OnClick="lnkVerFichasAplicadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MontoPendiente" HeaderText="Monto Pendiente" SortExpression="MontoPendiente" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:TextBox ID="txtMXA" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" Text='<%# Eval("MontoPorAplicar","{0:0.00}") %>'
Enabled="false" TabIndex="9" Style="text-align: right">
                                        
</asp:TextBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MPA2" HeaderText="MPA2" SortExpression="MPA2" Visible="false" />
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
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCambiarEstatusFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenidoVentanaFichasFacturas" class="modal">
<div id="ventanaFichasFacturas" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFF" runat="server" OnClick="lnkCerrarFF_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<asp:UpdatePanel ID="uplblVentanaFacturasFichas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2>
<asp:Label ID="lblVentanaFacturasFichas" runat="server"></asp:Label></h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFF">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFF" runat="server" CssClass="dropdown" TabIndex="5"
OnSelectedIndexChanged="ddlTamanoFF_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFF">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoFF" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="6" OnClick="lnkExportarFF_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" style="width: auto;">
<asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%"
OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting"
CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true" PageSize="250">
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
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="AplicadoPor" HeaderText="Aplicado Por" SortExpression="AplicadoPor" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarAplicacion" runat="server" Text="Eliminar" OnClick="lnkEliminarAplicacion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="uplblErrorFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFF" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
<asp:AsyncPostBackTrigger ControlID="gvFacturas" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<div id="contenidoConfirmacionCambioEstatusFI" class="modal">
<div id="confirmacionCambioEstatusFI"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplnkCerrarConfirmacionCambiarEstatus" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarConfirmacionCambiarEstatus" runat="server" Text="Cerrar" CommandName="CambiarEstatusFI" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h2 style="margin-left:25px;">Descartar Saldo en Pago de Cliente</h2>
<div class="columna400px" style="margin-top:0px;">
<div class="renglon2x">
<div class="etiqueta_400px">
<label for="btnAceptarElimnarCFDI">Esta acción cambiará el estatus del Pago a 'Aplicado', su saldo ya no será contemplado para futuras aplicaciones. <br /> ¿Desea Continuar?</label>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCambiarEstatusFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCambiarEstatusFI" runat="server"  OnClick="btnAceptarCambiarEstatusFI_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

</asp:Content>
