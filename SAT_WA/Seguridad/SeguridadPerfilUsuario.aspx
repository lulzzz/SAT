<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="SeguridadPerfilUsuario.aspx.cs" Inherits="SAT.Seguridad.SeguridadPerfilUsuario"  Title="Administración de Seguridad" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Hojas de estilo-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div id="encabezado_forma">
<h1>Seguridad y Configuración de Acceso</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<h2>Administración de la Seguridad</h2>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<label for="ddlNivelDeSeguridad">Aplicar a</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlNivelDeSeguridad" runat="server">
<ContentTemplate>
<asp:DropDownList ID="ddlNivelDeSeguridad" CssClass="dropdown" OnSelectedIndexChanged="ddlNivelDeSeguridad_SelectedIndexChanged" runat="server" AutoPostBack="true"
TabIndex="1" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblPerfilUsuario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblPerfilUsuario" runat="server">Perfil</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlNivelDeSeguridad" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPerfilUsuario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPerfilUsuario" OnSelectedIndexChanged="ddlPerfilUsuario_SelectedIndexChanged" CssClass="dropdown" runat="server" AutoPostBack="true"
TabIndex="2" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlNivelDeSeguridad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton"></div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnConfigurar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnConfigurar" runat="server" CssClass="boton" Text="Configurar" TabIndex="3" OnClick="btnConfigurar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_media_seccion">
<div class="header_seccion">
<h2>Configuración de Acceso</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamañoGridViewAcciones">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewAcciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewAcciones" runat="server" OnSelectedIndexChanged="gvAcciones_OnSelectedIndexChanged" TabIndex="4" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewAcciones">Ordenado Por:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblCriterioGridViewAcciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewAcciones" TabIndex="5" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAcciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelAcciones" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelAcciones" runat="server" Text="Exportar" TabIndex="6" OnClick="lkbExportarExcelAcciones_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelAcciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="grid_media_seccion">
<asp:UpdatePanel ID="upgvAcciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAcciones"  CssClass="grid_media_seccion" OnRowDataBound="gvAcciones_OnRowDataBound" OnPageIndexChanging="gvAcciones_OnpageIndexChanging" OnSorting="gvAcciones_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" TabIndex="14"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Accion" HeaderText="Acción" SortExpression="Accion" />
<asp:TemplateField ItemStyle-Width="10%" HeaderText="Oculto" >
<ItemTemplate>
<asp:CheckBox ID="chkNegado" TabIndex="7" runat="server" Enabled="False"/>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-Width="10%">
<ItemTemplate>
<asp:LinkButton ID="lkbCambiarPermiso" runat="server" OnClick="lkbCambiarPermiso_Click">Cambiar</asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewAcciones" />
<asp:AsyncPostBackTrigger ControlID="btnConfigurar" />
<asp:AsyncPostBackTrigger ControlID="ddlNivelDeSeguridad" />
<asp:AsyncPostBackTrigger ControlID="ddlPerfilUsuario" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>

