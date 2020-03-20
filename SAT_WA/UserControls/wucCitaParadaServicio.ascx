<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCitaParadaServicio.ascx.cs" Inherits="SAT.UserControls.wucCitaParadaServicio" %>
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUCCitaParada();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryUCCitaParada() {
$(document).ready(function () {

//Validación de controles de búsqueda
var validacionCitaParada = function () {
var isValidP1 = !$('.scriptCitaParada').validationEngine('validate');

return isValidP1;
};
//Validación de campos requeridos
$('.scriptGuardarCitaParada').click(validacionCitaParada);

/*Selectrores de fecha: Actualización de Llegadas y Salidas, Inicio y Fin de Eventos*/
$('.scriptCitaParada').datetimepicker({
    lang: 'es',
    format: 'd/m/Y H:i'
});

});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryUCCitaParada();
</script>
<div class="header_seccion">
<img src="../Image/Ruta.png" />
<h2>Paradas del Servicio</h2>
</div>
<div class="columna3x">
<div class="etiqueta_50px">
<label for="ddlTamanoParadasServicio">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoParadasServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoParadasServicio" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoParadasServicio_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarVencimientoHistorial" runat="server" TabIndex="5"
OnClick="lkbExportarVencimientoHistorial_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarVencimientoHistorial" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvParadasServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadasServicio" runat="server" AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvParadasServicio_PageIndexChanging" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvParadasServicio_RowDataBound" PageSize="5">
<Columns>
<asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" ReadOnly="True" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TipoParada" HeaderText="Tipo Parada" SortExpression="TipoParada" ReadOnly="True" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" ReadOnly="True" />
<asp:TemplateField HeaderText="Cita" SortExpression="Cita">
<ItemTemplate>
<asp:Label ID="lblCitaParada" runat="server" Text='<%#Eval("Cita", "{0:dd/MM/yyyy HH:mm}") %>' />
<asp:LinkButton ID="lkbCitaParada" runat="server" Text='<%#Eval("Cita", "{0:dd/MM/yyyy HH:mm}") %>' ToolTip="Editar Fecha" OnClick="lkbCitaParada_Click"  CommandName="Editar"/>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtCitaParada" runat="server" Text='<%#Eval("Cita", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="textbox scriptCitaParada validate[required, custom[dateTime24]]"></asp:TextBox>
</EditItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Fecha Llegada" SortExpression="FechaLlegada">
<ItemTemplate>
<asp:Label ID="lblFechaLlegada" runat="server" Text='<%#Eval("FechaLlegada", "{0:dd/MM/yyyy HH:mm}") %>' />
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lkbGuardarCitaParada" runat="server" CommandName="Aceptar" OnClick="lkbCitaParada_Click" CssClass="scriptGuardarCitaParada">Aceptar</asp:LinkButton>
<asp:LinkButton ID="lkbCancelarCitaParada" runat="server" CommandName="Cancelar" OnClick="lkbCitaParada_Click">Cancelar</asp:LinkButton>
</EditItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="ActualizacionLlegada" HeaderText="Actualización" SortExpression="ActualizacionLlegada" ReadOnly="True" />
<asp:BoundField DataField="RazonLlegadaTarde" HeaderText="Razón Tarde" SortExpression="RazonLlegadaTarde" ReadOnly="True" />
<asp:BoundField DataField="FechaSalida" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Salida" SortExpression="FechaSalida" ReadOnly="True">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ActualizacionSalida" HeaderText="Actualización" SortExpression="ActualizacionSalida" ReadOnly="True" />
<asp:BoundField DataField="Kilometraje" HeaderText="Kilometraje" SortExpression="Kilometraje" ReadOnly="True" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" ReadOnly="True">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoParadasServicio" />
</Triggers>
</asp:UpdatePanel>
</div><br />
<div class="columna3x">    
<asp:UpdatePanel ID="uplblErrorActualizacionCitaParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorActualizacionCitaParada" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvParadasServicio" />
</Triggers>
</asp:UpdatePanel>    
</div><br />