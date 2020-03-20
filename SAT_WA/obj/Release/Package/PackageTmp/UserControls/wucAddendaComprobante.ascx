<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="wucAddendaComprobante.ascx.cs" Inherits="SAT.UserControls.wucAddendaComprobante" %>
 <div style="width:1000px">
<div class="header_seccion">
<img src="../Image/Direccion.png" />
<h2>Addenda Comprobante</h2>
</div>
<div class="columna" style="width:450px; float:left">
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" TabIndex="2"  CssClass="boton"
 Text="Guardar" OnClick="btnGuardar_OnClick" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminar" runat="server" TabIndex="2" CssClass="boton"
 Text="Eliminar"  OnClick="btnEliminar_OnClick"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  style="height:200px; width:450px">
<asp:UpdatePanel ID="uptrvFormas" runat="server" UpdateMode="Conditional" RenderMode="Block">
<ContentTemplate>
<asp:TreeView ID="trvFormas" runat="server" ShowCheckBoxes="None"
OnSelectedNodeChanged="trvFormas_OnSelectedNodeChanged"
TabIndex="1" ontreenodedatabound="trvFormas_TreeNodeDataBound">
</asp:TreeView>
<asp:XmlDataSource ID="xmlds" runat="server"></asp:XmlDataSource>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="gvAtributos" />
<asp:AsyncPostBackTrigger ControlID="btnCopiar" />
<asp:AsyncPostBackTrigger ControlID="btnQuitar" />
</Triggers>
</asp:UpdatePanel>
    </div>
<div class="renglon">
<div class="control">
<asp:UpdatePanel ID="upbtnCopiar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ImageButton ID="btnCopiar" Visible="false"  runat="server" ImageUrl="~/Imagenes/boton_añadir_nodo.png"
ToolTip="Copiar Nodo" CausesValidation="false" OnClick="click_EdicionNodo" CommandName="Copiar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upbtnQuitar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ImageButton ID="btnQuitar" Visible="false" runat="server"  ImageUrl="~/Imagenes/boton_quitar_nodo.png"
ToolTip="Quitar Nodo" CausesValidation="false" OnClick="click_EdicionNodo" CommandName="Quitar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<asp:UpdatePanel ID="uplblAvisoNodo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblAvisoNodo" runat="server"  CssClass="label" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnCopiar" />
<asp:AsyncPostBackTrigger ControlID="btnQuitar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x" style="float:left">
<div class="renglon">
<asp:UpdatePanel ID="uplblNodo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h4><asp:Label ID="lblNodo" runat="server"  Text="Nodo: Por Asignar"></asp:Label></h4>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" EventName="SelectedNodeChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon">
<h4>Elementos</h4>
</div>
<div class="renglon">
<div class="control">
<asp:UpdatePanel ID="upElemento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="Elemento" runat="server" Enabled="false" CssClass="textbox">
</asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnSave" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" EventName="SelectedNodeChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<asp:UpdatePanel ID="upbtnSave" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" Text="Editar Valor"
 CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnSave" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" EventName="SelectedNodeChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon">
<h4>Atributos</h4>
</div>
<div style="width: 98%; height:250px; float: left; border:1px solid #DDD; overflow:scroll;">
<asp:UpdatePanel ID="upgvAtributos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAtributos" runat="server" AllowPaging="True" AllowSorting="false"
AutoGenerateColumns="False" ShowFooter="True"  Enabled="false" CssClass="gridview"  >
<RowStyle CssClass="FilaGridViewCSS" />
<FooterStyle CssClass="PieGridViewCSS" />
<PagerStyle CssClass="PaginadorGridViewCSS" />
<SelectedRowStyle CssClass="renglon2xeleccionadaGridViewCSS" />
<HeaderStyle CssClass="EncabezadoGridViewCSS" />
<EditRowStyle CssClass="FilaEdicionGridViewCSS" />
<AlternatingRowStyle CssClass="FilaAlternaGridViewCSS" />
<Columns>
<asp:TemplateField HeaderText="Atributo" SortExpression="Atributo">
 <ItemTemplate>
 <asp:Label ID="lblAtributo" runat="server" Text='<%# Eval("Atributo") %>'></asp:Label>
 </ItemTemplate>
 </asp:TemplateField>
 <asp:TemplateField HeaderText="Valor" SortExpression="Valor">
 <ItemTemplate>
 <asp:TextBox ID="txtValor" runat="server" CssClass="InputL" Text='<%# Eval("Valor") %>'>
 </asp:TextBox>
 </ItemTemplate>
 </asp:TemplateField>
 <asp:TemplateField HeaderText="">
 <ItemTemplate>
 <asp:LinkButton ID="lkbSave" runat="server" Text="Guardar" OnClick="lkbSave_OnClick"
  ValidationGroup="EditaAtributosXML" />
 </ItemTemplate>
 </asp:TemplateField>
 </Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlAddenda" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="trvFormas" EventName="SelectedNodeChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
    