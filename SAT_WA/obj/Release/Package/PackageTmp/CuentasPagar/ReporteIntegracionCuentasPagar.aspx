<%@ Page Title="Reporte de Integración de CxP" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteIntegracionCuentasPagar.aspx.cs" Inherits="SAT.CuentasPagar.ReporteIntefracionCuentasPagar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Liquidacion.css" type="text/css" rel="stylesheet" />
<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
<script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
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
function ConfiguraJQuerySaldosDetalle()
{
//Inicializando Función
$(document).ready(function () {

//Cargando Catalogo de Autocompletado
$("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

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

//Listado de Estatus de Cobro
$("#<%=ddlEstatusFactura.ClientID%>").multiselect({
selectedList: 2,
selectall: 1
});

//Configurando Validación de Controles
$("#<%=btnBuscar.ClientID%>").click(function () {

//Añadiendo Controles a la Validación
var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');

//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3;
});

//Añadiendo Encabezado Fijo
$("#<%=gvFacturasProveedor.ClientID%>").gridviewScroll({
width: document.getElementById("contenedor_gv_facturas").offsetWidth - 15,
height: 550
});

});
}

//Invocando Función de Configuración
ConfiguraJQuerySaldosDetalle();
</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Reporte de Integración CxP</h1>
</div>
<div class="seccion_controles">

<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Facturas</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px" MaxLength="10"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtFolio">Folio</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px" MaxLength="20"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFolio">UUID</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUUID" runat="server" CssClass="textbox_100px" MaxLength="20"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta"><label for="ddlEstatusFactura">Estatus</label></div>
<div class="control">
<asp:ListBox runat="server" ID="ddlEstatusFactura" SelectionMode="multiple" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha Fac. Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha Fac. Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class ="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No. de Servicio</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox_100px"></asp:TextBox>
</div>
<div class="etiqueta">
<label for="txtNoViaje">No. Viaje Cliente</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox_100px"></asp:TextBox>
</div>
</div>
<div class ="renglon2x">
<div class="etiqueta">
<label for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox_100px"></asp:TextBox>
</div>
</div>
<div class ="renglon2x">
<div class="etiqueta">
<label for="txtNoAnticipo">No. de Anticipo</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtNoAnticipo" runat="server" CssClass="textbox_100px"></asp:TextBox>
</div>
<div class="etiqueta">
<label for="txtNoLiquidacion">No. Liquidación</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtNoLiquidacion" runat="server" CssClass="textbox_100px"></asp:TextBox>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Detalle de Facturas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="9" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="10" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedor_gv_facturas">
<asp:UpdatePanel ID="upgvFacturasProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasProveedor" runat="server" AllowPaging="True" TabIndex="12"
OnPageIndexChanging="gvFacturasProveedor_PageIndexChanging" PageSize="250" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gvFacturasProveedor_RowDataBound">
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" Visible="true" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" >
</asp:BoundField>
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusFactura" HeaderText="Estatus" SortExpression="EstatusFactura" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CPorte" HeaderText="Carta Porte" SortExpression="CPorte" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="NoAnticipo" HeaderText="No. Anticipo" SortExpression="NoAnticipo" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoAnticipo" HeaderText="Monto Anticipo" SortExpression="MontoAnticipo" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstAnticipo" HeaderText="Estatus Anticipo" SortExpression="EstAnticipo" />
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" ItemStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoLiquidacion" HeaderText="Monto Liquidación" SortExpression="MontoLiquidacion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstLiquidacion" HeaderText="Estatus Liquidación" SortExpression="EstLiquidacion" />
<asp:BoundField DataField="MontoOtros" DataFormatString="{0:c}" HeaderText="Monto Otros" SortExpression="MontoOtros">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoFactura" HeaderText="Monto Factura" SortExpression="MontoFactura" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoAplicado" DataFormatString="{0:c}" HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoProgramado" DataFormatString="{0:c}" HeaderText="Monto Programado" SortExpression="MontoProgramado">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Saldo" SortExpression="Saldo" ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:LinkButton ID="lkbAplicacionesFactura" runat="server" OnClick="lnkAplicaciones_Click" Text='<%#Eval("Saldo", "{0:c}") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
<!-- Ventana de Fichas Aplicadas -->
<div id="contenidoVentanaFichasFacturas" class="modal">
<div id="ventanaFichasFacturas" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFF" runat="server" CommandName="FichasFacturas" OnClick="lnkCerrarFF_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<asp:UpdatePanel ID="uplblVentanaFacturasFichas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2><asp:Label ID="lblVentanaFacturasFichas" runat="server" Text="Aplicaciones y Relaciones de la Factura"></asp:Label></h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon3x">
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
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting" CssClass="gridview" AllowSorting="true" 
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
<asp:BoundField DataField="Egreso" HeaderText="No. Egreso" SortExpression="Egreso" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="IdEntidad" HeaderText="IdEntidad" SortExpression="IdEntidad" Visible="false" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Content>