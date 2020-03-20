<%@ Page Title="Tipo Cobro Recurrente" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoCobroRecurrente.aspx.cs" Inherits="SAT.Liquidacion.TipoCobroRecurrente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la pagina-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript"  src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript"  src="../Scripts/jquery.validationEngine.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!--Script que valida la inserción de los datos de los controles-->
<script type="text/javascript">
//Obtiene la instancia actual de la página y amade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la función ConfiguraJqueryTipoCobroRecurrente
ConfiguraJQueryTipoCobroRecurrente();
}
}
//Declara la función qe valida los controles de la página
function ConfiguraJQueryTipoCobroRecurrente()
{
$(document).ready(function () {
//Creación y asignación de la función a la variable ValidaTipoCobroRecurrente
var validaTipoCobroRecurrente = function () {
//Creacion de las variables y asignación de los controles del formulario TipoCobroRecurrente
    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtClaveContabilidad.ClientID%>").validationEngine('validate');
//Devuelve un valor a la función
    return isValid1 && isValid2;
};
//Permite que los eventos de guardar activen la función de validación de controles
$("#<%=btnGuardar.ClientID%>").click(validaTipoCobroRecurrente);
$("#<%=lkbGuardar.ClientID%>").click(validaTipoCobroRecurrente);
});
}
//Invocando a la función
ConfiguraJQueryTipoCobroRecurrente();
</script>
<!--Fin Script-->
<div id="encabezado_forma">
<img src="../Image/tipocobro.png" />
<h1>Tipo Cobro Recurrente</h1>
</div>
<!--Inicio Menu contextual-->
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
<!--Fin menú Contextual-->
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Depositos.png" />
<h2>Descripcion Cobros Recurrentes</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class ="etiqueta">
<label for="ddlTipoAplicacion">Tipo Aplicación: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTipoAplicacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoAplicacion" CssClass="dropdown2x" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoAplicacion_SelectedIndexChanged"></asp:DropDownList>
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
<label for="txtDescripcion">Descripción: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required,CONCEPTONOMINA12]" MaxLength="150" TabIndex="2"></asp:TextBox>
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
<label for="txtClaveContabilidad">Clave Contabililidad: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtClaveContabilidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtClaveContabilidad" CssClass="textbox2x validate[required,CLAVENOMINA12]" MaxLength="15" TabIndex="3"></asp:TextBox>
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
<label for="ddlConceptoSat">Cátalogo Sat: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlConceptoSat" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlConceptoSat" CssClass="dropdown2x" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoAplicacion" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta"></div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upchkGravado" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkGravado" Text="Gravado" TabIndex="5" />                           
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
<div class="etiqueta"></div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upchkPositivo" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkPositivo" Text="Positivo" TabIndex="6"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoAplicacion" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta"></div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upchkSinTermino" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkSinTermino" Text="¿Es un Cobro Recurrente?" TabIndex="7"/>
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
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" CssClass="boton_cancelar" TabIndex="8" Text="Cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" TabIndex="9" OnClick="btnGuardar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>            
<div class ="renglon2x">
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uplblError" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
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
</div><!--FinColumna-->
</div><!--Fin SeccionControles-->
</asp:Content>
