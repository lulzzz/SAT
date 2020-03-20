<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCiudad.ascx.cs" Inherits="SAT.UserControls.wucCiudad" %>
<!--hoja de estilos que dan formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!--Script que valida los controles de la oagina-->
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Script que valida la insercion de datos en los controles-->
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryCiudad
ConfiguraJQueryCiudad();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryCiudad() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable ValidaCiudad
var validaCiudad = function (evt) {
//Creación de las variables y asignacion de los controles de la pagina Ciudad
var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1;
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaCiudad);
});
}
ConfiguraJQueryCiudad();
</script>
<!--Fin Script-->
<!--Fin del script-->
<div class="contenedor_media_seccion">
<div class="header_seccion">
<img src="../Image/ciudad.jpg" />
<h2>Ciudad</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" MaxLength="100" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblPais">País:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlPais" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlPais" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstado">Estado:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlEstado" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlEstado" CssClass="dropdown2x" TabIndex="3" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlPais" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>   
<div class="renglon2x">
<div class="controlr">
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="OnClick_lkbBitacora" TabIndex="4"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar"   OnClick="btnGuardar_Click"  CssClass="boton" TabIndex="5" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="boton_cancelar" TabIndex="6" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click"  CssClass="boton" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
