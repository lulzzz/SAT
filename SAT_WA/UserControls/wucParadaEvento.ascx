<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucParadaEvento.ascx.cs" Inherits="SAT.UserControls.wucParadaEvento" %>
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />    
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler<%=this.GUID%>);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler<%=this.GUID%>(sender, args) {
if (args.get_error() == undefined) {
    ConfiguraJQuerywucParadaEvento<%=this.GUID%>();
}
}
//Creando función para configuración de jquery en control de usuario
    function ConfiguraJQuerywucParadaEvento<%=this.GUID%>() {
$(document).ready(function () {

//Validación de controles de búsqueda
var validacionFechasEvento = function () {
var isValidP1 = !$("#<%=txtCitaEvento.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtFechaInicioEvento.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtFechaFinEvento.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos
$("#<%=this.btnActualizarEventoParada.ClientID%>").click(validacionFechasEvento);

$("#<%=txtCitaEvento.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaInicioEvento.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFinEvento.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}

//Invocación Inicial de método de configuración JQuery
    ConfiguraJQuerywucParadaEvento<%=this.GUID%>();
</script>
<div class="header_seccion">
<img src="../Image/Descarga.png" />
<asp:UpdatePanel ID="uph2EncabezadoControlParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2 id="h2EncabezadoControlParada" runat="server">Eventos de Parada ''</h2>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upmtvEventosParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvEventosParada" runat="server">
<asp:View ID="vwResumenEventos" runat="server">
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoEventos">
Mostrar:
</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoEventos"></label>
<asp:DropDownList ID="ddlTamanoEventos" runat="server" OnSelectedIndexChanged="ddlTamanoEventos_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown_100px">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:Button ID="btnNuevoEventoParada" runat="server" OnClick="btnEventoParada_Click" CommandName="Nuevo" Text="Nuevo Evento" CssClass="boton" />
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbExportarEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarEventos" runat="server" Text="Exportar Excel" OnClick="lkbExportarEventos_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEventos" runat="server" AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvEventos_PageIndexChanging" AllowPaging="True" ShowFooter="True"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Secuencia" HeaderText="No." SortExpression="Secuencia" DataFormatString="{0:0}">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="CitaEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Evento" SortExpression="CitaEvento">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="InicioEvento" HeaderText="Inicio Evento" SortExpression="InicioEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FinEvento" HeaderText="Fin Evento" SortExpression="FinEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MotivoRetraso" HeaderText="Motivo Retraso" SortExpression="MotivoRetraso" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEditarEvento" runat="server" Text="Editar" OnClick="lkbAccionEvento_Click" CommandName="Editar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEliminarEvento" runat="server" Text="Eliminar" OnClick="lkbAccionEvento_Click" CommandName="Eliminar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacoraEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacoraEvento" runat="server" Text="Bitácora" OnClick="lkbAccionEvento_Click" CommandName="Bitacora"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<%--<asp:PostBackTrigger ControlID="lkbBitacoraEvento" />--%>
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwEdicionEvento" runat="server">
<div class="contenedor_media_seccion">
<div class="columna" style="width:auto;">
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipoEventos">Tipo Evento</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEventos" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtCitaEvento">Cita</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCitaEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCitaEvento" runat="server" CssClass="textbox validate[custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtFechaInicioEvento">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicioEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicioEvento" runat="server" CssClass="textbox validate[custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtFechaFinEvento">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFinEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFinEvento" runat="server" CssClass="textbox validate[custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlMotivoRetraso">Motivo Retraso</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlMotivoRetraso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMotivoRetraso" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEventoParada" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarEventoParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
<div class="renglon">
<div class="controlBoton">
<asp:Button ID="btnCancelarEventoParada" runat="server" Text="Cancelar"
CssClass="boton_cancelar" OnClick="btnEventoParada_Click" CommandName="Cancelar" />
</div>
<div class="controlBoton">
<asp:Button ID="btnActualizarEventoParada" runat="server" OnClick="btnEventoParada_Click" CommandName="Actualizar" Text="Actualizar"
CssClass="boton" />
</div>
</div>
</div>  
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
</asp:UpdatePanel>