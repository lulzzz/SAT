<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucEncabezadoServicio.ascx.cs" Inherits="SAT.UserControls.wucEncabezadoServicio" %>
 <!-- Estilos -->
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryEncabezadoServicio();
}
}

//Función de Configuración
function ConfiguraJQueryEncabezadoServicio() {
$(document).ready(function () {

    //Función de validación Edición de  Cliente
    var validacionEdicionCliente = function (evt) {
        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
        var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');

        return isValid1
    };
    //Botón Aceptar Hacer Servicio
    $("#<%= btnGuardarRef.ClientID %>").click(validacionEdicionCliente);
// *** Catálogos Autocomplete *** //;
    $("#<%=  txtCliente.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    });
});
}
//Invocando Función de Configuración
ConfiguraJQueryEncabezadoServicio();
</script>
<div class="seccion_controles_modal">
    <div class="header_seccion">
        <img src="../Image/ResumenReporte.png" />
        <h2 runat="server" id="lblNoServicio"></h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server"  CssClass="textbox2x validate[required, custom[IdCatalogo]]"  ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarRef" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarRef" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtReferencia">Referencia</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarRef" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarRef" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtCartaPorte">Carta Porte</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtCartaPorte" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarRef" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarRef" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtObservacion">Observación</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarRef" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarRef" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon_boton">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardarRef" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardarRef" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarRef_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarRef" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelarRef" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelarRef" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarRef_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarRef" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>