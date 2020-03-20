<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoCambio.aspx.cs" Inherits="SAT.Administrativo.TipoCambio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion de Configuración
ConfiguraJQueryTipoCambio();
}
}
//Declara la función que configura los Controles
function ConfiguraJQueryTipoCambio() {
$(document).ready(function () {

//Función de Validación
var validaTipoCambio = function () {
//Validando Controles
var isValid1 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtValor.ClientID%>").validationEngine('validate');

//Devolviendo Resultado Obtenido
return isValid1 && isValid2;
}

//Añadiendo Validación a Eventos Click
$("#<%=btnGuardar.ClientID%>").click(validaTipoCambio);
$("#<%=lkbGuardar.ClientID%>").click(validaTipoCambio);

//Cargando Control de Fechas
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
closeOnDateSelect: true,
onSelectDate: function (selected, evnt) {
//Asignando Valor Seleccionado
$("#<%=txtFecha.ClientID%>").val(selected.format('dd/MM/yyyy'));
}
});

});
}
//Invocando Función de Configuración
ConfiguraJQueryTipoCambio();
</script>
<div id="encabezado_forma">
<img src="../Image/SignoPesos.png" />
<h1>Tipo de Cambio</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbAbrir" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="seccion_controles">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlMoneda">Moneda</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlMoneda" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlMoneda" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtFecha">Fecha</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecha" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="2" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtValor">Valor</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValor" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber6]]" TabIndex="3" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipoOperacion">Tipo Operación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoOperacion" runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="6" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="5" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</asp:Content>
