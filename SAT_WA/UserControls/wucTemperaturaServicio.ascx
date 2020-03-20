<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucTemperaturaServicio.ascx.cs" Inherits="SAT.UserControls.TemperaturaServicio" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryTemperatura();
}
}

//Declarand Función de Configuración
function ConfiguraJQueryTemperatura()
{   //Inicializando Función
$(document).ready(function () {

    //Declarando Función de Validación
    var validaTemperaturas = function () {

        //Validando Controles
        var isValid1 = !$("#<%=txtMaxima1.ClientID%>").validationEngine('validate');
        var isValid2 = !$("#<%=txtMedia1.ClientID%>").validationEngine('validate');
        var isValid3 = !$("#<%=txtMinima1.ClientID%>").validationEngine('validate');
        var isValid4;
        var isValid5;
        var isValid6;

        //Validando el Control
        if ($("#<%=chkFull.ClientID%>").is(':checked') == true) {
            //Validando Controles
            isValid4 = !$("#<%=txtTMaxima2.ClientID%>").validationEngine('validate');
            isValid5 = !$("#<%=txtTMaxima2.ClientID%>").validationEngine('validate');
            isValid6 = !$("#<%=txtTMaxima2.ClientID%>").validationEngine('validate');
        }
        else {
            //Validando Controles
            isValid4 = true;
            isValid5 = true;
            isValid6 = true;
        }

        //Devolviendo Resultado de la Validación
        return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6;
    }

    //Añadiendo validación al Boton
    $("#<%=btnGuardar.ClientID%>").click(validaTemperaturas);

});
}

//Invocando Función de Configuración
ConfiguraJQueryTemperatura();


</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/Termometro.png" />
        <h2>Temperatura Servicio</h2>
    </div>
    <div class="columna2x_sin_float">
        <div class="renglon2x_35h">
            <div class="etiqueta">
            
            </div>
            <div class="control_100px">
                <img src="../Image/TemperaturaMinima.png" />                
            </div>
            <div class="control_100px">
                <img src="../Image/TemperaturaMedia.png" />
            </div>
            <div class="control_100px">
                <img src="../Image/TemperaturaMaxima.png" />
            </div>
        </div>    
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtTMinima1">Temperatura</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtMinima1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtMinima1" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtMedia1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtMedia1" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtMaxima1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtMaxima1" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <asp:CheckBox ID="chkFull" runat="server" Text="Full" AutoPostBack="true" OnCheckedChanged="chkFull_CheckedChanged"/>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTMinima2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTMinima2" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkFull" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTMedia2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTMedia2" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkFull" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTMaxima2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTMaxima2" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkFull" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>        
        <div class="renglon2x">
            <div class="etiqueta">
            
            </div>
            <div class="controlBoton">
                <asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click"/>
            </div>
        </div>
    </div>     
</div>