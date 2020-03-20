<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPerfilUsuario.ascx.cs" Inherits="SAT.UserControls.wucPerfilUsuario" %>
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
            
}
}

</script>
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Perfiles de Usuario</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" TabIndex="1" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPerfilesUsuario" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="2" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvPerfilesUsuario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPerfilesUsuario" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="3"
OnPageIndexChanging="gvPerfilesUsuario_PageIndexChanging" OnSorting="gvPerfilesUsuario_Sorting"
PageSize="5" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdPerfilUsuario" HeaderText="PerfilUsuario" SortExpression="IdPerfilUsuario" Visible="false" />
<asp:BoundField DataField="Perfil" HeaderText="Perfil" SortExpression="Perfil" />
<asp:BoundField DataField="Activo" HeaderText="¿Activo?" SortExpression="Activo" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionar" runat="server" Text="Seleccionar" OnClick="lnkSeleccionar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>


