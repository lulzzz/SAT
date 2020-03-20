<%@ Page Title="Comprobante Recepción de Pagos" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ComprobanteRecepcionPagosV10.aspx.cs" Inherits="SAT.FacturacionElectronica33.ComprobanteRecepcionPagosV10" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos Solicitud de Empleo -->
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQuery();
}
}

//Declarando Función de configuración de jQuery
function ConfiguraJQuery() {
$(document).ready(function () {
$("#<%=txtCliente.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});
var validacionSustitucion = function () {
var isValid1 = !$("#<%=txtMotivoCancelacionCFDI.ClientID%>").validationEngine('validate');
return isValid1;
};
$("#<%=btnAceptarSustituirCFDI.ClientID%>").click(validacionSustitucion);
});
}

//Configurando en carga inicial de formulario
ConfiguraJQuery();
</script>
<div id="encabezado_forma">
<h1>CFDI de Recepción de Pagos</h1>
</div>
<div class="seccion_controles">
<div class="columna4x"></div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/ResumenReporte.png" />
<h2>Resumen Pendientes x Cliente</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoGVClientes">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoGVClientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGVClientes" runat="server" CssClass="dropdown_100px" TabIndex="1"
OnSelectedIndexChanged="ddlTamanoGVClientes_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoPorGVClientes">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoPorGVClientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPorGVClientes" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvClientes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<asp:UpdatePanel ID="uplnkExportarGVClientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarGVClientes" runat="server" Text="Exportar" TabIndex="2" CommandName="Clientes" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarGVClientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvClientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvClientes" runat="server" AllowPaging="false" AllowSorting="True" TabIndex="3"
OnPageIndexChanging="gvClientes_PageIndexChanging" OnSorting="gvClientes_Sorting" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
<ItemTemplate>
<asp:LinkButton ID="lnkClienteResumen" runat="server" OnClick="lnkClienteResumen_Click" Text='<%# Eval("Cliente") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="PagosConSaldo" HeaderText="Pagos Con Saldo (Aún No Disponibles)" SortExpression="PagosConSaldo" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="PagosDisponibles" HeaderText="Pagos Disponibles" SortExpression="PagosDisponibles" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="CFDIConPendientes" HeaderText="CFDI Con Pendientes" SortExpression="CFDIConPendientes" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVClientes" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSustituirCFDI" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="columna2x" style="margin-left:25px;">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda Rápida de Cliente</h2>
</div>
<asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
<div class="renglon2x">
<div class="etiqueta_100px">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" TabIndex="4" runat="server" CssClass="textbox2x validate[custom[IDCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" CommandName="Cliente" TabIndex="5" Text="Buscar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
</div>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnInformacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnInformacionCFDI" Text="Información de CFDI" OnClick="btnVista_OnClick" CommandName="InformacionCFDI" runat="server" CssClass="boton_pestana_activo" TabIndex="6" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnCFDIPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSustituirCFDI" />
<asp:AsyncPostBackTrigger ControlID="gvClientes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnCFDIPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCFDIPendientes" Text="Búsqueda de CFDI's" OnClick="btnVista_OnClick" runat="server" CommandName="CFDIPendientes" CssClass="boton_pestana" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnInformacionCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSustituirCFDI" />
<asp:AsyncPostBackTrigger ControlID="gvClientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvCFDIPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvSolicitud" runat="server" ActiveViewIndex="0">
<asp:View ID="vwActualizacionCFDI" runat="server">
<div class="header_seccion">
<asp:UpdatePanel ID="uph2InformacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2 id="h2InformacionCFDI" runat="server">* Titulo Dinámico *</h2>
</ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="gray">
<a href="#" class="fa fa-file-archive-o"></a>
<ul>
<li><asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li><asp:LinkButton ID="lkbTimbrarFacturaElectronica" runat="server" Text="Timbrar" OnClick="lkbElementoMenu_Click" CommandName="Timbrar"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbEliminarCFDI" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar"></asp:LinkButton></li>
</ul>
</li>
<li class="yellow"><a href="#" class="fa fa-download"></a>
<ul>
<li><asp:LinkButton ID="lkbPDF" runat="server" Text="PDF" OnClick="lkbElementoMenu_Click" CommandName="PDF"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbXML" runat="server" Text="XML" OnClick="lkbElementoMenu_Click" CommandName="XML"></asp:LinkButton></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
<asp:PostBackTrigger ControlID="lkbXML" />
<asp:PostBackTrigger ControlID="lkbPDF" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<label for="lblSerieFolio">Serie y Folio</label>
</div>
<div class="etiqueta_200px">
<asp:UpdatePanel ID="uplblSerieFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblSerieFolio" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="lblUUID">UUID</label>
</div>
<div class="etiqueta_200px">
<asp:UpdatePanel ID="uplblUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblUUID" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x" style="margin-left:25px;">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblEstatus">Estatus</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblEstatus" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_200px">
<asp:UpdatePanel ID="uplblSustituyeA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSustituyeA" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblFechaExpedicion" class="label">F. Expedición</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
                
</div>
<div class="etiqueta">
<label for="lblFechaCancelacion" class="label">F. Cancelación</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblFechaCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaCancelacion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="header_seccion"></div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Pin_Verde.png" />
<h2>Pagos Disponibles</h2>
<div class="controlr">
<asp:UpdatePanel ID="upbtnAgregarPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarPagos" runat="server" CssClass="boton2x" Text="Agregar a CFDI" CommandName="Agregar" OnClick="btnPagos_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnQuitarPagos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVPagosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>    
</div>  
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoGVPagosDisponibles">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoGVPagosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGVPagosDisponibles" runat="server" CssClass="dropdown_100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoGVPagosDisponibles_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoPorGVPagosDisponibles">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoPorGVPagosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPorGVPagosDisponibles" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagosDisponibles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportargvPagosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportargvPagosDisponibles" runat="server" Text="Exportar" CommandName="PagosDisponibles"
OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportargvPagosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvPagosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPagosDisponibles" runat="server" AllowPaging="True" AllowSorting="True"
OnPageIndexChanging="gvPagosDisponibles_PageIndexChanging" OnSorting="gvPagosDisponibles_Sorting"
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
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkPagosTodosDisp" runat="server" AutoPostBack="true"
OnCheckedChanged="chkPagosTodos_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkPagosVariosDisp" runat="server" AutoPostBack="true"
OnCheckedChanged="chkPagosTodos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FI" HeaderText="F.I." SortExpression="FI" >
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="Fecha" HeaderText="Fecha Ingreso" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="FormaPago" HeaderText="Forma Pago" SortExpression="FormaPago" >
<ItemStyle Width="120px" />
</asp:BoundField>
<asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda">
<ItemStyle Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:TemplateField HeaderText="Aplicado" SortExpression="Aplicado">
<ItemTemplate>
<asp:Label ID="lblAplicadoPagoDisponible" runat="server" Text='<%# Eval("Aplicado", "{0:c}") %>'></asp:Label>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:TemplateField>
<asp:BoundField DataField="Saldo" HeaderText="Saldo" SortExpression="Saldo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarPagos" />
<asp:AsyncPostBackTrigger ControlID="btnQuitarPagos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVPagosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Pin_Azul.png" />
<h2>Pagos del CFDI Actual</h2>
<div class="controlr">
<asp:UpdatePanel ID="upbtnQuitarPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnQuitarPagos" runat="server" CssClass="boton_cancelar2x" CommandName="Quitar" Text="Quitar de CFDI" OnClick="btnPagos_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagosEnCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPagos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVPagosCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div> 
</div> 
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoGVPagosCFDI">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoGVPagosCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGVPagosCFDI" runat="server" CssClass="dropdown_100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoGVPagosCFDI_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoPorGVPagosCFDI">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoPorGVPagosCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPorGVPagosCFDI" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagosEnCFDI" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportargvPagosCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportargvPagosCFDI" runat="server" Text="Exportar" CommandName="PagosCFDI"
OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportargvPagosCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvPagosEnCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPagosEnCFDI" runat="server" AllowPaging="True" AllowSorting="True"
OnPageIndexChanging="gvPagosEnCFDI_PageIndexChanging" OnSorting="gvPagosEnCFDI_Sorting"
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
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkPagosTodosCFDI" runat="server" AutoPostBack="true"
OnCheckedChanged="chkPagosTodos_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkPagosVariosCFDI" runat="server" AutoPostBack="true"
OnCheckedChanged="chkPagosTodos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FI" HeaderText="F.I." SortExpression="FI" >
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="Fecha" HeaderText="Fecha Ingreso" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="FormaPago" HeaderText="Forma Pago" SortExpression="FormaPago" >
<ItemStyle Width="120px" />
</asp:BoundField>
<asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda">
<ItemStyle Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:TemplateField HeaderText="Aplicado" SortExpression="Aplicado">
<ItemTemplate>
<asp:Label ID="lblAplicadoPagoCFDI" runat="server" Text='<%# Eval("Aplicado", "{0:c}") %>'></asp:Label>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:TemplateField>
<asp:BoundField DataField="Saldo" HeaderText="Saldo" SortExpression="Saldo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
<ItemStyle HorizontalAlign="Right" Width="80px" />
<FooterStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarPagos" />
<asp:AsyncPostBackTrigger ControlID="btnQuitarPagos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVPagosCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
</asp:View>
<asp:View ID="vwCFDIPagos" runat="server">
<div class="header_seccion">
<img src="../Image/AnalisisDoc.png" />
<h2>Búsqueda y Consulta de CFDI's de Recepción de Pagos</h2>
</div>
<div class="columna4x">
            
<div class="renglon3x">
<div class="etiqueta">
<label class="Label" for="txtFolio">Folio:</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_50px" MaxLength="40"></asp:TextBox>
</div>
<div class="control_100px">
<asp:CheckBox ID="chkCFDIRegistrados" runat="server" Text="Registrados" CssClass="label_negrita" Checked="true" />                    
</div>
<div class="control_100px">
<asp:CheckBox ID="chkCFDITimbrados" runat="server" Text="Timbrados" CssClass="label_negrita" Checked="false" />    
</div>
<div class="control_100px">
<asp:CheckBox ID="chkCFDIPorSustituir" runat="server" Text="Por Sustituir" CssClass="label_negrita" Checked="true" />         
</div>
<div class="control_100px">
<asp:CheckBox ID="chkCFDISustituidos" runat="server" Text="Sustituidos" CssClass="label_negrita" Checked="false" />    
</div>
</div>
            
            
            
<div class="renglon2x">
<div class="etiqueta">
<label class="Label">Fecha Inicial Exp:</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label">Fecha Final Exp:</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</div>
</div>
            
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscarCFDI" runat="server" OnClick="btnBuscar_Click" CommandName="CFDI" Text="Buscar" CssClass="boton" />
</div>
                
</div>
</div>
<div class="header_seccion">
<h2>CFDI's Encontrados</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoGVCFDIPagos">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoGVCFDIPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGVCFDIPagos" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoGVCFDIPagos_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoGVCFDIPagos">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoGVCFDIPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoGVCFDIPagos" runat="server" Text="" CssClass="Label"> </asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCFDIPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportarGVCFDIPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="div_orden_gridviewS">
<asp:LinkButton ID="lkbExportarGVCFDIPagos" runat="server" OnClick="lnkExportar_Click" CommandName="CFDIPendientes">Exportar</asp:LinkButton>
</div>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarGVCFDIPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvCFDIPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCFDIPagos" runat="server" TabIndex="16"
AllowPaging="True" AllowSorting="True"
OnPageIndexChanging="gvCFDIPagos_OnPageIndexChanging"
OnSorting="gvCFDIPagos_OnSorting"
ShowFooter="True" PageSize="25" AutoGenerateColumns="False" OnRowDataBound="gvCFDIPagos_RowDataBound" Width="100%">
<Columns>
<asp:TemplateField>
<HeaderTemplate>
<asp:CheckBox ID="chkTodosCFDI" runat="server"
OnCheckedChanged="chkTodosCFDI_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVariosCFDI" runat="server"
OnCheckedChanged="chkTodosCFDI_CheckedChanged" AutoPostBack="true" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie">
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio">
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" >
<ItemStyle Width="150px" />
</asp:BoundField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" >
<ItemStyle Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="ConteoPagos" HeaderText="Conteo Pagos" SortExpression="ConteoPagos">
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="FechaCaptura" HeaderText="Fecha Captura" SortExpression="FechaCaptura" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha Expedición" SortExpression="FechaExpedicion" DataFormatString="{0:yyy-MM-dd HH:mm}">
<ItemStyle HorizontalAlign="Right" Width="50px" />
</asp:BoundField>
<asp:BoundField DataField="ExpedidoPor" HeaderText="Expedido Por" SortExpression="ExpedidoPor" >
<ItemStyle Width="100px" />
</asp:BoundField>
<asp:BoundField DataField="SustituyeA" HeaderText="Sustituye A" SortExpression="SustituyeA">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaCancelacion" HeaderText="Fecha de Cancelación" SortExpression="FechaCancelacion" DataFormatString="{0:yyy-MM-dd HH:mm}">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" Width="80px" />
</asp:BoundField>
<asp:BoundField DataField="CanceladoPor" HeaderText="Cancelado Por" SortExpression="CanceladoPor" >
<ItemStyle Width="100px" />
</asp:BoundField>
<asp:BoundField DataField="Motivo" HeaderText="Motivo" SortExpression="Motivo" >
<ItemStyle Width="150px" />
</asp:BoundField>
<asp:TemplateField HeaderText="Consultar" SortExpression="Consultar">
<ItemTemplate>
<asp:LinkButton ID="lnkConsultarCFDI" runat="server" Text='<%# Eval("Consultar") %>' OnClick="lkbDetalles_Click" CommandName='<%# Eval("Consultar") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sustituir" SortExpression="Sustituir">
<ItemTemplate>
<asp:LinkButton ID="lnkSustituirCFDI" runat="server" Text='<%# Eval("Sustituir") %>' OnClick="lkbDetalles_Click" CommandName="Sustituir"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbXMLCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbXMLCFDI" runat="server" OnClick="lkbDetalles_Click"
CommandName="XML">XML</asp:LinkButton>
<asp:LinkButton ID="lkbPDFCFDI" runat="server" OnClick="lkbDetalles_Click"
CommandName="PDF">PDF</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbXMLCFDI" />
<asp:PostBackTrigger ControlID="lkbPDFCFDI" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>                   
                                     
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarCFDI" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVCFDIPagos" />
</Triggers>
</asp:UpdatePanel>
</div>

<div class="header_seccion">
<h2>Exportación de CFDI's</h2>
</div>
        
<div class="renglon2x">
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uprdbPDF">
<ContentTemplate>
<asp:CheckBox ID="chkPDF" Text="PDF" runat="server" CssClass="label_negrita" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uprdbXML">
<ContentTemplate>
<asp:CheckBox ID="chkXML" Text="XML" runat="server" CssClass="label_negrita" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="upbtnExportarPDF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnExportarCFDI" runat="server" CssClass="boton" Text="Exportar" OnClick="btnExportar_OnClick" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnExportarCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
        
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvClientes" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGVClientes" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarElimnarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnCFDIPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnInformacionCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSustituirCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>

<div id="contenidoConfirmacionRegistarFacturacionElectronica" class="modal" >
<div id="confirmacionRegistarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarRegistarFacturacionElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarRegistarFacturacionElectronica" runat="server" Text="Cerrar" CommandName="RegistrarCFDI"  OnClick="lkbCerrarVentanaModal_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Registrar CFDI de Recepción de Pagos</h3>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlSucursal">Sucursal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRegistrarFacturaElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRegistrarFacturaElectronica" runat="server" CssClass="boton" OnClick="btnRegistrarFacturaElectronica_Click" Text="Registrar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<div id="contenidoConfirmacionTimbrarFacturacionElectronica" class="modal">
<div id="confirmaciontimbrarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarFacturacionElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTimbrarFacturacionElectronica" runat="server" Text="Cerrar" CommandName="TimbrarCFDI" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Timbrar CFDI de Recepción de Pagos</h3>
<asp:Panel ID="pnlTimbrarCFDI" runat="server" DefaultButton="btnAceptarTimbrarFacturacionElectronica">
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" Text="" runat="server" CssClass="textbox validate[custom[onlyLetterSp]]" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbrarFacturacionElectronica" runat="server"  OnClick="btnAceptarTimbrarFacturacionElectronica_Click"  CssClass ="boton"  Text="Timbrar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Panel>
</div>
</div>

<div id="contenidoConfirmacionEliminarFacturacionElectronica" class="modal">
<div id="confirmacionEliminarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplnkCerrarConfirmacionEliminar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarConfirmacionEliminar" runat="server" Text="Cerrar" CommandName="EliminarCFDI" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3 style="margin-left:25px;">Eliminar el CFDI de Recepción de Pagos</h3>
<div class="columna400px">
<div class="renglon2x">
<div class="etiqueta_400px">
<label for="btnAceptarElimnarCFDI">Esta acción dejará disponibles los Pagos para otro CFDI. <br /> ¿Desea Continuar?</label>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarElimnarCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarElimnarCFDI" runat="server"  OnClick="btnAceptarElimnarCFDI_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<div id="contenidoConfirmacionSustituirFacturacionElectronica" class="modal">
<div id="confirmacionSustituirFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplnkCerrarConfirmacionSustituir" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarConfirmacionSustituir" runat="server" Text="Cerrar" CommandName="SustituirCFDI" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3 style="margin-left:25px;">Sustituir el CFDI de Recepción de Pagos</h3>
<div class="columna400px">
<div class="renglon2x_35h">
<label for="btnAceptarElimnarCFDI">Colocará como 'Pendiente de Cancelación' el CFDI actual, dejando disponibles los Pagos para un nuevo CFDI que será creado en blanco. Si Desea Continuar, coloque un Motivo de Cancelación.</label>
</div>
<div class="renglon2x"></div>
<asp:Panel ID="pnlSustitucionCFDI" runat="server" DefaultButton="btnAceptarSustituirCFDI">
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoCancelacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivoCancelacionCFDI" runat="server" CssClass="textbox2x validate[required]" MaxLength="200"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCFDIPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarSustituirCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarSustituirCFDI" runat="server"  OnClick="btnAceptarSustituirCFDI_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
</div>
</div>
</div>
</asp:Content>
