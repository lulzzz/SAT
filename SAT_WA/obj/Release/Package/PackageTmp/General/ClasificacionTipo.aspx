<%@ Page Title="Tipo Clasificación" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClasificacionTipo.aspx.cs" Inherits="SAT.General.ClasificacionTipo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Hoja de estilo para el formato del formulario-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!--hoja de estilo para el formato de validación de controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Script para la validación de datos-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!--Script que valida el contenido de los controles-->
<script type ="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryClasificacionTipo
ConfiguraJQueryClasificacionTipo();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryClasificacionTipo() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable validaClasificacionTipo
var validaClasificacionTipo = function () {
//Creación de las variables y asignacion de los controles de la pagina ClasificacionTipo
var isValid1 = !$("#<%=txtCodigo.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2;
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaClasificacionTipo);
$("#<%=lkbGuardar.ClientID%>").click(validaClasificacionTipo);
});
}
ConfiguraJQueryClasificacionTipo();
</script>
<!--Fin del script-->
<div id="encabezado_forma">
<img src="../Image/Clasificacion.png" />
<h1>Tipo Clasificación</h1>
</div>
<!--Menú Principal-->
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
<!--Fin Menú Principal-->
<div class="seccion_controles">
<div class="header_seccion">
<h2>Descripción</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompania">Compañia: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtCompania" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCompania" CssClass="textbox2x" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlClasificacion">Clasificación:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlClasificacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlClasificacion" CssClass="dropdown2x" TabIndex="2"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCodigo">Código:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtCodigo" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCodigo" CssClass="textbox validate[required ,custom[onlyLetterSp]]" TabIndex="3" MaxLength="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" TabIndex="4" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="6"/>
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
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="5" />
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

