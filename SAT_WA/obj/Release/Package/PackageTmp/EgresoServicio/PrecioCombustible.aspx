<%@ Page Title="Precio Combustible" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrecioCombustible.aspx.cs" Inherits="SAT.EgresoServicio.PrecioCombustible" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--invoca a las hojas de estilo de la pagina principal-->
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />
<!--invoca a los estilos de las validaciones de las cajas de texto-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Invoca a los script que validan los campos del formulario-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<!--Invoa a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!--Codigo que valida los controles del formulario-->
<script type="text/javascript">
//Gestio las actualizaciones cliente servidor para la validación de los controles
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args){
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
ConfiguraJQueryCostoCombustible();
}
}
//Declara la funcion Configuracion que valida los campos
function ConfiguraJQueryCostoCombustible()
{
$(document).ready(function (){
//Crea variables para la validción de los controles
var validaCostoCombustible = function () {
//Creación de las variables que permiten validar cada campo.
var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtPrecioCombustible.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');                   
//Devuelve un valor a la función
return isValid1 && isValid2 && isValid3 && isValid4; 
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaCostoCombustible);
$("#<%=lkbGuardar.ClientID%>").click(validaCostoCombustible);

});
// *** Fecha de inicio y fin (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$(document).ready(function () {
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}
//Invoca a la función ConfiguraJQueryPrecioCombustible()
ConfiguraJQueryCostoCombustible();
</script>
<div id="encabezado_forma">
<img src="../Image/SignoPesos.png"/>
<h1>Precio Combustible</h1>
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
<li>
    <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
    <asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
    <asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
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
<div class ="header_seccion">
<img src="../Image/EstacionCombustible.png" />
<h2> Precio de Combustible</h2>
</div>  
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTabla">Entidad:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTabla" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTabla" TabIndex="1" CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRegistro">Registro:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtRegistro" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRegistro" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicio"> Fecha Inicio:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtFechaInicio">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class ="etiqueta">
<label for="txtFechaFin">Fecha Fin:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtFechaFin" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox validate[required, custom[dateTime24],custom[past]]" TabIndex="4"></asp:TextBox>
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
<div class="etiqueta">
<label for="txtPrecioCombustible">Precio Combustible:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtPrecioCombustible" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPrecioCombustible" CssClass="textbox_50px validate[required], custom[positiveNumber]" MaxLength="18" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoCombustible">Tipo Combustible:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTipoCombustible"  UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoCombustible" CssClass="dropdown" TabIndex="5" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class ="etiqueta">
<label for="txtReferencia">Referencia: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtReferencia" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox  runat="server" ID="txtReferencia" CssClass="textbox2x validate[required]" MaxLength="50" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" CssClass="boton_cancelar" ID="btnCancelar" Text="Cancelar" OnClick="btnCancela_Click" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" CssClass="boton" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="8" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px">
<asp:UpdatePanel runat="server" ID="uplblError" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</asp:Content>
