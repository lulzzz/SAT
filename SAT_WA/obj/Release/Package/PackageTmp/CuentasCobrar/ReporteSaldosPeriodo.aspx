<%@ Page Title="Saldos Por Periodo" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteSaldosPeriodo.aspx.cs" Inherits="SAT.CuentasCobrar.ReporteSaldosPeriodo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
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
ConfiguraJQuerySaldosPeriodo();
}
}

//Declarando Función de Configuración
function ConfiguraJQuerySaldosPeriodo()
{
//Inicializando Función
$(document).ready(function () {

//Cargando Catalogo de Autocompletado
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'});

//Configurando Validación de Controles
$("#<%=btnBuscar.ClientID%>").click(function () {

//Añadiendo Controles a la Validación
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');

//Devolviendo Resultado Obtenido
return isValid1;
});
});
}

//Invocando Función de Configuración
ConfiguraJQuerySaldosPeriodo();
</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Reporte de Saldos Por Periodo</h1>
</div>
<div class="contenedor_controles">
<asp:Panel ID="pnlReporteSaldosPedido" runat="server" DefaultButton="btnBuscar">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="2" CssClass="boton" OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Panel>
</div>
<div class="contenedor_controles">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="3"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSaldosPeriodo" />
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
<asp:UpdatePanel ID="upgvSaldosPeriodo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSaldosPeriodo" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvSaldosPeriodo_PageIndexChanging" OnSorting="gvSaldosPeriodo_Sorting"
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
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Saldo15Dias" HeaderText="15 Dias" SortExpression="Saldo15Dias" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Saldo30Dias" HeaderText="30 Dias" SortExpression="Saldo30Dias" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Saldo45Dias" HeaderText="45 Dias" SortExpression="Saldo45Dias" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SaldoMayor45" HeaderText="Mayor de 45 Dias" SortExpression="SaldoMayor45" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SaldoTotal" HeaderText="Saldo Total" SortExpression="SaldoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
