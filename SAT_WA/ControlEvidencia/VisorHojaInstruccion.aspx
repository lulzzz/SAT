<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="VisorHojaInstruccion.aspx.cs" Inherits="SAT.ControlEvidencia.VisorHojaInstruccion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos Autocomplete, Zoom y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryVisorHI();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryVisorHI() {

//Validación campos Visor Hoja de Instrucción
$(document).ready(function () {
//Función de validación de encabezado de servicio
var validacionVisorHojaInstruccion = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtRemitente.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtDestinatario.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3
};
//Botón Buscar
$("#<%= btnBuscar.ClientID %>").click(validacionVisorHojaInstruccion);
});
// *** Catálogos Autocomplete *** //
$(document).ready(function () {
    $("#<%=txtRemitente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
    $("#<%=txtDestinatario.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
    $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=7' });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryVisorHI();
</script>

<div id="encabezado_forma">
<img src="../Image/Indicadores.png" />
<h1>Visor de hojas de instruccion.</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar hoja de instruccion por</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtCliente">Cliente</label>
</div>
<div class="control">
<asp:TextBox ID="txtCliente" runat="server" TabIndex="2" CssClass="textbox2x validate[custom[IdCatalogo]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtRemitente">Remitente</label>
</div>
<div class="control">
<asp:TextBox ID="txtRemitente" runat="server" TabIndex="3" CssClass="textbox2x validate[custom[IdCatalogo]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtDestinatario">Destinatario</label>
</div>
<div class="control">
<asp:TextBox ID="txtDestinatario" runat="server" TabIndex="4" CssClass="textbox2x validate[custom[IdCatalogo]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="">Descripción</label>
</div>
<div class="control">
<asp:TextBox ID="txtDescripcion" runat="server" TabIndex="5" CssClass="textbox2x" MaxLength="200">
</asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">                   
</div>
<div class="control2x">
<asp:TextBox ID="txtCompania" CssClass="textbox2x" runat="server" TabIndex="1" Enabled="false" Visible="false"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" TabIndex="6"
OnClick="btnBuscar_OnClick" ValidationGroup="General" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Hojas Encontradas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoResumen">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoResumen" runat="server" CssClass="dropdown" TabIndex="7" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoResumen_OnSelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarResumen">Ordenar:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>                        
<asp:Label ID="lblOrdenarResumen" runat="server" CssClass="Label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvResumen" EventName="Sorting" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar Excel" TabIndex="8"
OnClick="lnkExportar_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumen" runat="server" AllowPaging="True"
AllowSorting="True" AutoGenerateColumns="False" CssClass="gridview"
OnPageIndexChanging="gvResumen_PageIndexChanging"
OnSorting="gvResumen_Sorting" PageSize="25" ShowFooter="True" TabIndex="9" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Remitente" HeaderText="Remitente" SortExpression="Remitente" />
<asp:BoundField DataField="Destinatario" HeaderText="Destinatario" SortExpression="Destinatario" />
<asp:BoundField DataField="TotalDocumentos" HeaderText="Total de Documentos" SortExpression="TotalDocumentos" />
<asp:BoundField DataField="TotalAccesorios" HeaderText="Total de Accesorios" SortExpression="TotalAccesorios" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
<asp:BoundField DataField="TerminalC" HeaderText="Terminal de Cobro" SortExpression="TerminalC" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkVer" runat="server" Text="Ver" OnClick="lnkVer_OnClick"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:UpdatePanel ID="uplnkBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkBitacora" runat="server" Text="Bitacora"
OnClick="lnkBitacora_OnClick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkBitacora" />
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoResumen" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>   
</asp:Content>
