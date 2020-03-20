<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucEmailCFDIV3_3.ascx.cs" Inherits="SAT.UserControls.wucEmailCFDIV3_3" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryEmail33<%=this.ID%>();
}
}
//Creando función para configuración de jquery en formulario
    function ConfiguraJQueryEmail33<%=this.ID%>() {

//Validación campos Hoja de Instrucción
$(document).ready(function () {

//Función de validación de campos
var validacionEmail = function (evt) {
var isValidP1 = !$("#<%=txtDestinatariosEmail.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtAsunto.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2;
};
//Boton Aceptar Email
$("#<%=btnAceptarEmail.ClientID%>").click(validacionEmail);


});
}
//Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryEmail33<%=this.ID%>();
</script>
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEmail" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEmail" runat="server" Text="Cerrar" OnClick="lkbCerrarEmail_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EnvioRecepcion.png" />
<h2>Envío de CFDI por E-mail</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtAsunto">Asunto:</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtAsunto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAsunto" runat="server" CssClass="textbox2x validate[required]" ></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="height:65px;">
<div class="etiqueta">
<label class="Label" for="txtDestinatariosEmail">Destinatarios:</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtDestinatariosEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDestinatariosEmail" runat="server" CssClass="textbox3x validate[required]" TextMode="MultiLine" ></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="height:80px;">
<div class="etiqueta">
<label class="Label" for="txtMensaje">Mensaje:</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMensaje" runat="server" CssClass="textbox3x" TextMode="MultiLine"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEmail" Text="Enviar" runat="server" OnClick="btnAceptarEmail_Click" CssClass="boton"></asp:Button>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
</div>