<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLecturaHistorial.ascx.cs" Inherits="SAT.UserControls.wucLecturaHistorial" %>
<!--hoja de estilos que dan formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryLecturaHistorial
ConfiguraJQueryLecturaHistorial();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryLecturaHistorial() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable validaLecturaHistorial
var validaLecturaHistorial = function (evt) {
//Creación de las variables y asignacion de los controles de la pagina LecturaHistorial
var isValid1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2;
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnBuscar.ClientID%>").click(validaLecturaHistorial);
});
$(document).ready(function () {
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
$(document).ready(function () {
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}
ConfiguraJQueryLecturaHistorial();
</script>
<div class="contenedor_recursos_asignados">
<div class="header_seccion">
<img src="../Image/Calendar2.png" />
<h2>Lecturas Historial</h2>
</div>
<div class="columna3x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicio">Fecha Inicio:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaFin">Fecha Fin:</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaFin" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoLectura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoLectura" OnClick="btnNuevoLecturaHistorial_Click" runat="server" CssClass="boton"  Text="Nuevo"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnBusca_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x"></div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoLecturaHistorial">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoLecturaHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoLecturaHistorial" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoLecturaHistorial_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoLecturaHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoLecturaHistorial" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLecturaHistorial" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarLecturaHistorial" runat="server" TabIndex="5" 
OnClick="lkbExportarLecturaHistorial_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarLecturaHistorial" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvLecturaHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLecturaHistorial" runat="server"  AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvLecturaHistorial_PageIndexChanging" OnSorting="gvLecturaHistorial_Sorting" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvLecturaHistorial_RowDataBound" >
<Columns>
<asp:BoundField DataField="FechaLectura" HeaderText="Fecha Lectura" SortExpression="FechaLectura" DataFormatString="{0:dd/MM/yyyy HH:mm}"  />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="LitrosLectura" HeaderText="Litros Lectura" SortExpression="LitrosLectura" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="KmsLectura" HeaderText="Kms. Lec." SortExpression="KmsLectura" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="HrsLectura" HeaderText="Hrs. Lec." SortExpression="HrsLectura" ItemStyle-HorizontalAlign="Right" />   
<asp:BoundField DataField="Rendimiento" HeaderText="Rendimiento" SortExpression="Rendimiento" ItemStyle-HorizontalAlign="Right" />                                  
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbConsultarLectura"  OnClick="lkbLecturaHistorialClick" runat="server" Text="Consultar" Enabled="true"  CommandName="Consultar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoLecturaHistorial" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>