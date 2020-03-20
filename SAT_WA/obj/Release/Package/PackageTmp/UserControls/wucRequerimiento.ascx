<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRequerimiento.ascx.cs" Inherits="SAT.UserControls.wucRequerimiento"  %>
<div class="contenido_pestañas_documentacion">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcionReq">Descripción</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtDescripcionReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcionReq" runat="server" CssClass="textbox2x"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
<asp:AsyncPostBackTrigger ControlID="ddlTabla" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTabla">Cuando</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTabla" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTabla" runat="server" CssClass="dropdown2x"
OnSelectedIndexChanged="ddlTabla_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlRegistro">Es igual a</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlRegistro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlRegistro" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
<asp:AsyncPostBackTrigger ControlID="ddlTabla" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
        
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTablaReq">Entonces</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTablaReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTablaReq" runat="server" CssClass="dropdown2x" CausesValidation="true"
OnSelectedIndexChanged="ddlTablaReq_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlFiltro">Tendra</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlFiltro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlFiltro" runat="server" CssClass="dropdown2x"
OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
<asp:AsyncPostBackTrigger ControlID="ddlTablaReq" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCondicion">Cumpliendo con</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCondicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCondicion" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlValor">Al valor</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlValor" runat="server" CssClass="dropdown2x" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
<asp:AsyncPostBackTrigger ControlID="ddlTablaReq" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="ddlFiltro" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" CssClass="boton" OnClick="btnGuardar_Click" Text ="Guardar" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" OnClick="btnCancelar_Click" Text ="Cancelar" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador"></div>
</div>    
</div>
<div class="columna600px">        
<div class="renglon3x">
<div class="control2x">
<asp:UpdatePanel ID="upddlTamanoReqDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoReqDisp">Mostrar:</label>
<asp:DropDownList ID="ddlTamanoReqDisp" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoReqDisp_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarReqDisp">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarReqDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarReqDisp" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDisponibles" runat="server" OnClick="lnkExportarDisponibles_Click" Text="Exportar Excel"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvReqDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvReqDisponibles" runat="server" AllowPaging="true" AllowSorting="true"
AutoGenerateColumns="false" OnSorting="gvReqDisponibles_Sorting" ShowFooter="true"
OnPageIndexChanging="gvReqDisponibles_PageIndexChanging" PageSize="5" CssClass="gridview">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Cuando" HeaderText="Cuando" SortExpression="Cuando" />
<asp:BoundField DataField="EsIgual" HeaderText="Es Igual" SortExpression="EsIgual" />
<asp:BoundField DataField="Entonces" HeaderText="Entonces" SortExpression="Entonces" />
<asp:BoundField DataField="Tendra" HeaderText="Tendra" SortExpression="Tendra" />
<asp:BoundField DataField="CumpliendoCon" HeaderText="Cumpliendo con" SortExpression="CumpliendoCon" />
<asp:BoundField DataField="AlValor" HeaderText="Al Valor" SortExpression="AlValor" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoReqDisp" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvReqDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>