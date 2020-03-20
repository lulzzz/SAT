<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CancelacionTimbreFiscalV33.aspx.cs" Inherits="SAT.FacturacionElectronica33.CancelacionTimbreFiscalV33" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Hoja de estilos que da formato a la página-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJqueryReporteCancelacionV33();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJqueryReporteCancelacionV33() {
$(document).ready(function () {

//Validación 
var validacionReporteComprobante= function () {

var isValid1 = !$("#<%=txtReceptor.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
return isValid1 && isValid2;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteComprobante);

//Añadiendo Función de Autocompletado al Control
$("#<%=txtReceptor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJqueryReporteCancelacionV33();
</script>
<div id="encabezado_forma">
<h1>Cancelación Timbre Fiscal</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar Comprobantes </h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoLiquidacion">
Receptor
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipo">Tipo </label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" AutoPostBack="true" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFolio" runat="server" TabIndex="3">Folio</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />

</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />

</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" CssClass="boton" Text="Buscar" TabIndex="9" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Comprobante</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewComprobante">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewComprobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewComprobante" OnSelectedIndexChanged="ddlTamañogvComprobante_SelectedIndexChanged" runat="server" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewComprobante">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewComprobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewComprobante" TabIndex="12" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobante" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelComprobante" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelComprobante" OnClick="lkbExportarGv_OnClick" runat="server" Text="Exportar" TabIndex="13"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelComprobante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvComprobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvComprobante" CssClass="gridview" OnPageIndexChanging="gvComprobante_OnPageIndexChanging"  OnSorting="gvComprobante_OnSorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="14" Width="100%">
<Columns>
<asp:TemplateField>
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="15" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="16" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="ReceptorRFC" HeaderText="RFC" SortExpression="ReceptorRFC" />   
<asp:BoundField DataField="Receptor" HeaderText="Cliente" SortExpression="Receptor" />                         
<asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha de Expedición" SortExpression="FechaExpedicion" DataFormatString="{0:yyy-MM-dd hh:mm tt}" />
<asp:BoundField DataField="FechaCancelacion" HeaderText="Fecha de Cancelación" SortExpression="FechaCancelacion" DataFormatString="{0:yyy-MM-dd hh:mm tt}" />
<asp:BoundField DataField="CanceladoPor" HeaderText="Cancelado por" SortExpression="CanceladoPor"> <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MotivoCancelacion" HeaderText="Motivo" SortExpression="MotivoCancelacion"> <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:c}" ><ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Descuento" HeaderText="Descuento" SortExpression="Descuento" DataFormatString="{0:c}" ><ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVATrasladado" HeaderText="IVA Trasladado" SortExpression="IVATrasladado" DataFormatString="{0:c}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVARetenido" HeaderText="IVA Retenido" SortExpression="IVARetenido" DataFormatString="{0:c}" ><ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}" ><ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbDetalles_Click" CommandName="Bitacora"></asp:LinkButton>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbDetalles_Click" CommandName="Referencias"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewComprobante" />
<asp:AsyncPostBackTrigger ControlID="btnReportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnReportar" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnReportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnReportar"  OnClick="btnReportar_OnClick" runat="server" CssClass="boton" Text="Reportar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
