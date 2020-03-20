<%@ Page Title="Requisición Servicio" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequisicionServicio.aspx.cs" Inherits="SAT.Almacen.RequisicionServicio" %>
<%@ Register Src="~/UserControls/wucRequisicion.ascx" TagName="wucRequisicion" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraRequisicionServicio();
}
}

//Declarando Función de Configuración
function ConfiguraRequisicionServicio() {
$(document).ready(function () {
//Cargando Catalogos Autocompleta
$("#<%=txtCliente.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});
$("#<%=txtAlmacen.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});

//Cargando Controles de Fecha
$("#<%=txtFecIniReq.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecFinReq.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecIniServ.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecFinServ.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Validando controles de Busqueda de Servicio
var validaBusquedaServicio = function () {
//Declarando Objetos de Retorno
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2;
var isValid3;

//Validando el Control
if ($("#<%=chkIncluirServ.ClientID%>").is(':checked') == true) {
//Validando Controles
isValid2 = !$("#<%=txtFecIniServ.ClientID%>").validationEngine('validate');
isValid3 = !$("#<%=txtFecFinServ.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo
isValid2 = true;
isValid3 = true;
}

//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3;
}
//Añadiendo Validador a Evento Click
$("#<%=btnBuscarServicio.ClientID%>").click(validaBusquedaServicio);

//Validando controles de Busqueda de Servicio
var validaBusquedaRequisicion = function () {
//Declarando Objetos de Retorno
var isValid1 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
var isValid2;
var isValid3;

//Validando el Control
if ($("#<%=chkIncluirReq.ClientID%>").is(':checked') == true) {
//Validando Controles
isValid2 = !$("#<%=txtFecIniReq.ClientID%>").validationEngine('validate');
isValid3 = !$("#<%=txtFecFinReq.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo
isValid2 = true;
isValid3 = true;
}

//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3;
}
//Añadiendo Validador a Evento Click
$("#<%=btnBuscarRequisicion.ClientID%>").click(validaBusquedaRequisicion);
});
$(document).keyup(function (e) {
if (e.keyCode == 27) { // escape key maps to keycode `27`
//Ocultando Menu
OcultarMenuRequisicion();
OcultarMenuServicio();
}
});
$(document).click(function (e) {

//Ocultando Menu
OcultarMenuRequisicion();
OcultarMenuServicio();
});
}

//Declarando Función de Validación de Fechas
function CompareDatesReq() {
//Obteniendo Valores
var txtDate1 = $("#<%=txtFecIniReq.ClientID%>").val();
var txtDate2 = $("#<%=txtFecFinReq.ClientID%>").val();

//Fecha en Formato MM/DD/YYYY
var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

//Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
if (date1 > date2)
//Mostrando Mensaje de Operación
return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}

//Declarando Función de Validación de Fechas
function CompareDatesServ() {
//Obteniendo Valores
var txtDate1 = $("#<%=txtFecIniServ.ClientID%>").val();
var txtDate2 = $("#<%=txtFecFinServ.ClientID%>").val();

//Fecha en Formato MM/DD/YYYY
var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

//Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
if (date1 > date2)
//Mostrando Mensaje de Operación
return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}

//Invocando Función de Configuración
ConfiguraRequisicionServicio();

//Función encargada de Mostrar el Ménu
function MostrarMenuRequisicion(control, e) {
//Ocultando en caso de estar Abierto
OcultarMenuRequisicion();

//Obteniendo Coordenadas de las Forma
var posx = e.pageX + 'px';
var posy = e.pageY + 'px';

//Si el Evento es de Tipo Click
if (e.type == 'click')

//Detener Propagación del Evento
e.stopPropagation();

//Asignando Posiciones al Documento
document.getElementById(control).style.position = 'absolute';
document.getElementById(control).style.left = posx;
document.getElementById(control).style.top = posy;

//Ejecutando 
$(document).ready(function (evt) {

//Mostrando DIV
$('#' + control).slideDown(100);
});
}
//Función encargada de Ocultar el Ménu
function OcultarMenuRequisicion() {
$(document).ready(function () {

//Ocultando DIV
$('.menuRequisiciones').slideUp(100);
});
}

//Función encargada de Mostrar el Ménu
function MostrarMenuServicio(control, e) {
//Ocultando en caso de estar Abierto
OcultarMenuServicio();

//Obteniendo Coordenadas de las Forma
var posx = e.pageX + 'px';
var posy = e.pageY + 'px';

//Si el Evento es de Tipo Click
if (e.type == 'click')

//Detener Propagación del Evento
e.stopPropagation();

//Asignando Posiciones al Documento
document.getElementById(control).style.position = 'absolute';
document.getElementById(control).style.left = posx;
document.getElementById(control).style.top = posy;

//Ejecutando 
$(document).ready(function (evt) {

//Mostrando DIV
$('#' + control).slideDown(100);
});
}
//Función encargada de Ocultar el Ménu
function OcultarMenuServicio() {
$(document).ready(function () {

//Ocultando DIV
$('.menuServicios').slideUp(100);
});
}
</script>
<div id="encabezado_forma">
<h1>Requisición Servicio</h1>
</div>
<div class="contenedor_controles">
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda por Requisición</h2>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="txtNoRequisicion">No. Requisición</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoRequisicion" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="ddlEstatusRequisicion">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusRequisicion" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtAlmacen">Almacen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbSolicitud" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbSolicitud" runat="server" Text="Solicitud" GroupName="General1" Checked="true" TabIndex="4" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbEntrega" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbEntrega" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbEntrega" runat="server" Text="Entrega" GroupName="General1" TabIndex="5" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbSolicitud" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtFecIniReq">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIniReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIniReq" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="6" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluirReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluirReq" runat="server" Text="¿Incluir?" TabIndex="7" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtFecFinReq">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFinReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFinReq" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDatesReq[]]" TabIndex="8" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarRequisicion" runat="server" CssClass="boton" Text="Buscar"
OnClick="btnBuscarRequisicion_Click" TabIndex="9" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevaRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevaRequisicion" runat="server" CssClass="boton_cancelar" Text="Nueva"
OnClick="btnNuevaRequisicion_Click" TabIndex="10" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Requerimiento.png" />
<h2>Requisiciones</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoRequisicion">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRequisicion" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoRequisicion_SelectedIndexChanged" TabIndex="11"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoRequisicion">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoRequisicion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRequisiciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRequisicion" runat="server" Text="Exportar" TabIndex="12" 
OnClick="lkbExportar_Click" CommandName="Requisicion"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRequisicion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura" oncontextmenu="return false">
<asp:UpdatePanel ID="upgvRequisiciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRequisiciones" runat="server" PageSize="25" Width="100%" TabIndex="13"
AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvRequisiciones_Sorting"
OnRowDataBound="gvRequisiciones_RowDataBound" OnPageIndexChanging="gvRequisiciones_PageIndexChanging" 
CssClass="gridview" ShowFooter="True">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="NoRequisicion" HeaderText="No. Requisición" SortExpression="NoRequisicion" />
<asp:BoundField DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
<ItemTemplate>
<asp:LinkButton ID="lkbNoServicio" runat="server" OnClick="lkbNoServicio_Click" Text='<%# Eval("NoServicio") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="OrdenCompra" HeaderText="Orden de Compra" SortExpression="OrderCompra" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Almacen" HeaderText="Almacen" SortExpression="Almacen" />
<asp:BoundField DataField="Solicitante" HeaderText="Solicitante" SortExpression="Solicitante" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha de Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaEntrega" HeaderText="Fecha de Entrega" SortExpression="FechaEntrega" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField>
<ItemTemplate>
<div id="menuReqServicio" runat="server">
<img src="../Image/menu_context2.png" />
</div>
<div id="menuReqServicioOpciones" runat="server" class="MenuContext menuRequisiciones" style="display:none;">
<div class="ContextItem">
<asp:LinkButton ID="lkbEditarReq" runat="server" OnClick="lkbEditarReq_Click" Text="Editar"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lkbSolicitar" runat="server" OnClick="lkbSolicitar_Click" Text="Solicitar"></asp:LinkButton>
</div>
</div>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarRequisicion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRequisicion" />
<asp:AsyncPostBackTrigger ControlID="wucRequisicion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEliminacion" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaRequisicion" />
<asp:AsyncPostBackTrigger ControlID="gvRequisicionesServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda por Servicio</h2>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="txtNoServicio">No. Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="14"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="ddlEstatusServicio">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusServicio" runat="server" CssClass="dropdown" TabIndex="15"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbCarga" runat="server" Text="Carga" GroupName="General" Checked="true" TabIndex="17" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbDescarga" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbDescarga" runat="server" Text="Descarga" GroupName="General" TabIndex="18" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtFecIniServ">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIniServ" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIniServ" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="19" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluirServ" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluirServ" runat="server" Text="¿Incluir?" TabIndex="20" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtFecFinServ">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFinServ" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFinServ" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDatesServ[]]" TabIndex="21" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarServicio" runat="server" CssClass="boton" Text="Buscar"
OnClick="btnBuscarServicio_Click" TabIndex="22" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Documentacion.png" />
<h2>Servicios</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoServicio">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoServicio" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoServicio_SelectedIndexChanged" TabIndex="23"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoServicio">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoServicio" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServicio" runat="server" Text="Exportar" TabIndex="24" 
OnClick="lkbExportar_Click" CommandName="Servicios"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura" oncontextmenu="return false">
<asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServicios" runat="server" PageSize="25" Width="100%"
AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvServicios_Sorting"
OnPageIndexChanging="gvServicios_PageIndexChanging" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True"
OnRowDataBound="gvServicios_RowDataBound">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="OrdenCompra" HeaderText="Orden de Compra" SortExpression="OrdenCompra" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:TemplateField HeaderText="Requisición" SortExpression="Requisicion">
<ItemTemplate>
<asp:LinkButton ID="lkbRequisicion" runat="server" OnClick="lkbRequisicion_Click" Text='<%# Eval("Requisicion") %>' Enabled="true"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" ItemStyle-HorizontalAlign="Left"/>
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" ItemStyle-HorizontalAlign="Left"/>
<asp:BoundField DataField="CitaCarga" HeaderText="Cita de Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="CitaDescarga" HeaderText="Cita de Descarga" SortExpression="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField>
<ItemTemplate>
<div id="menuServicio" runat="server">
<img src="../Image/menu_context2.png" />
</div>
<div id="menuServicioOpciones" runat="server" class="MenuContext menuServicios" style="display:none;">
<div class="ContextItem">
<asp:LinkButton ID="lkbNuevaReq" runat="server" Text="Nueva Req." OnClick="lkbNuevaReq_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lkbAgregar" runat="server" OnClick="lkbAgregar_Click" Text="Agregar"></asp:LinkButton>
</div>
</div>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicio" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServicio" />
<asp:AsyncPostBackTrigger ControlID="wucRequisicion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarEliminacion" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaRequisicion" />
<asp:AsyncPostBackTrigger ControlID="gvRequisiciones" />
<asp:AsyncPostBackTrigger ControlID="gvRequisicionesServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- Ventana de Alta de Requisiciones -->
<div id="contenedorVentanaAltaRequisicion" class="modal">
<div id="ventanaAltaRequisicion" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAltaReq" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAltaRequisicion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="AltaRequisicion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucRequisicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucRequisicion ID="wucRequisicion" runat="server" OnClickGuardarRequisicion="wucRequisicion_ClickGuardarRequisicion"
OnClickSolicitarRequisicion="wucRequisicion_ClickSolicitarRequisicion" OnClickReferenciarRequisicion="wucRequisicion_ClickReferenciarRequisicion"
Contenedor="#ventanaAltaRequisicion"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="gvRequisiciones" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaRequisicion" />
<asp:AsyncPostBackTrigger ControlID="gvRequisicionesServicio" />
</Triggers>
</asp:UpdatePanel>            
</div>
</div>

<!-- Ventana Alta de Referencias -->
<div id="contenedorVentanaReferencias" class="modal">
<div id="ventanaReferencias" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Referencias" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Referencias de Requisición</h2>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upwucReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="wucReferencias" runat="server" OnClickGuardarReferenciaViaje="wucReferencias_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="wucReferencias_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucRequisicion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Confirmación de Eliminación del Servicio -->
<div id="contenedorVentanaConfirmacionEliminaServicioReq" class="modal">
<div id="ventanaConfirmacionEliminaServicioReq" class="contenedor_ventana_confirmacion_arriba">
<div class="header_seccion">
<img src="../Image/ExclamacionRoja.png" />
<h2>
<asp:UpdatePanel ID="uplblMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensaje" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRequisiciones" />
</Triggers>
</asp:UpdatePanel>
</h2>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminacion" runat="server" CssClass="boton" Text="Aceptar"
OnClick="btnAceptarEliminacion_Click" CommandName="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminacion" runat="server" CssClass="boton_cancelar" Text="Cancelar"
OnClick="btnAceptarEliminacion_Click" CommandName="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- VENTANA MODAL REQUISICIONES POR SERVICIO -->
<div id="contenedorVentanaRequisiciones" class="modal">
<div id="ventanaRequisiciones" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarRequisiones" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarRequisiones" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="RequisicionesServicio" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna4x">
<div class="header_seccion">
<h2>Requisiciones del Servicio</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label>Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoServReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoServReq" runat="server" AutoPostBack="true" CssClass="dropdown_100px"
OnSelectedIndexChanged="ddlTamanoServReq_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoServReq">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoServReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoServReq" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRequisicionesServicio" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarServReq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServReq" runat="server" Text="Exportar" CommandName="ServicioRequisiciones" OnClick="lkbExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServReq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvRequisicionesServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRequisicionesServicio" runat="server" AllowPaging="true" AllowSorting="true"
PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False" 
OnSorting="gvRequisicionesServicio_Sorting" OnPageIndexChanging="gvRequisicionesServicio_PageIndexChanging">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoRequisicion" HeaderText="No. Requisición" SortExpression="NoRequisicion" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Almacen" HeaderText="Almacen" SortExpression="Almacen" />
<asp:BoundField DataField="CantSolicitado" HeaderText="Cant. Solicitado" SortExpression="CantSolicitado" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega" SortExpression="FechaEntrega" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Solicitante" HeaderText="Solicitante" SortExpression="Solicitante" />
<asp:BoundField DataField="Disponibles" HeaderText="Disponibles" SortExpression="Disponibles" />
<asp:BoundField DataField="Faltantes" HeaderText="Faltantes" SortExpression="Faltantes" ItemStyle-HorizontalAlign="Right" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbConsultarReq" runat="server" Text="Consultar" OnClick="lkbConsultarReq_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEliminarRelacion" runat="server" Text="Eliminar" OnClick="lkbEliminarRelacion_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServReq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

</asp:Content>
