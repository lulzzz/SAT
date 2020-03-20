<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="DepositosRechazados.aspx.cs" Inherits="SAT.EgresoServicio.DepositosRechazados" Title="Depósitos Rechazados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Operacion.css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script>
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryAutorizacionDeposito();
}
}

//Creando función para configuración de jquery en formulario
function ConfiguraJQueryAutorizacionDeposito() {
$(document).ready(function () {

var validacionBusqueda = function () {
var isValidP1 = !$("#<%=txtFechaSolicitudI.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtFechaSolicitudF.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2;
};

$("#<%=txtUnidad.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
});

$("#<%=txtOperador.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
});

$("#<%=txtProveedor.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=10&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
});

$("#<%=txtFechaSolicitudI.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaSolicitudF.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

$('#<%=gvDepositosRechazados.ClientID %>').gridviewScroll({
width: 1260,
height: 450
});

});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryAutorizacionDeposito();

</script>
    
<div id="encabezado_forma">
<img src="../Image/SignoPesosRechazo.png" />
<h1>Depósitos Rechazados</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros de Búsqueda</h2>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoDeposito">Tipo Depósito</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlTipoDeposito" runat="server" CssClass="dropdown"></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaSolicitudI">Desde </label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtFechaSolicitudI" runat="server" MaxLength="16" CssClass="textbox_100px validate[custom[dateTime24]]"></asp:TextBox>
</div>
<div class="etiqueta_50px">
<label for="txtFechaSolicitudF">al</label>
</div>
<div class="control_100px">
<asp:TextBox ID="txtFechaSolicitudF" runat="server" MaxLength="16" CssClass="textbox_100px validate[custom[dateTime24]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOperador">Operador</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnidad">Unidad</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x"></asp:TextBox>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnBuscarDepositosRechazados" runat="server" OnClick="btnBuscarDepositosRechazados_Click" CssClass="boton" Text="Buscar" />
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Depósitos Rechazados</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDepositos">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoDepositos" runat="server" OnSelectedIndexChanged="ddlTamanoDepositos_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</div>
<div class="etiqueta">
<label for="lblCriterioGridDepositos">Ordenado por:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCriterioGridDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridDepositos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDepositosRechazados" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="ulkbExcelExportarDepositosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelExportarDepositosPendientes" CommandName="DepositosPendientes"
runat="server" OnClick="lkbExcelExportarDepositosRechazados_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelExportarDepositosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvDepositosRechazados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDepositosRechazados" runat="server" AutoGenerateColumns="False"
AllowPaging="True" CssClass="gridview" AllowSorting="True" OnPageIndexChanging="gvDepositosRechazados_PageIndexChanging"
OnSorting="gvDepositosRechazados_Sorting" PageSize="25" ShowFooter="True" Width="100%">
<Columns>
<asp:BoundField DataField="NoAnticipo" HeaderText="No. Anticipo" SortExpression="NoAnticipo">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Factura" HeaderText="Factura" SortExpression="Factura">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="No. Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SolicitadoPor" HeaderText="Solicitado Por" SortExpression="SolicitadoPor">
</asp:BoundField>
<asp:BoundField DataField="FechaRechazo" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Rechazo" SortExpression="FechaRechazo">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="RechazadoPor" HeaderText="Rechazado Por" SortExpression="RechazadoPor" />
<asp:BoundField DataField="MotivoRechazo" HeaderText="Motivo Rechazo" SortExpression="MotivoRechazo" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDepositos" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarDepositosRechazados" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
