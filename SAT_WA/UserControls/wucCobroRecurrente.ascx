<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCobroRecurrente.ascx.cs" Inherits="SAT.UserControls.wucCobroRecurrente" %>
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraWucCobroRecurrente();
        }
    }
    //Función de Configuración
    function ConfiguraWucCobroRecurrente() {
        $(document).ready(function () {

            //Añadiendo Validación a Evento Click
            $("#<%=btnAceptar.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtEntidad.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtTotalDeuda.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtDescuento.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtFecInicio.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4;
            });

            //Añadiendo Extensor de Fecha
            $("#<%=txtFecInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y',
                timepicker: false
            });

        });
    }

    //Función de Validación
    function MaxValue() {

        //Obteniendo Valores
        var totalDeuda = $("#<%=txtTotalDeuda.ClientID%>").val();
        var descuento = $("#<%=txtDescuento.ClientID%>").val();

        //Validando que exista
        if (parseFloat(descuento) > parseFloat(totalDeuda))

            //Devolviendo Resultado
            return "El Descuento '"+ descuento +"' no puede ser mayor al Total de la Deuda '"+totalDeuda+"'";
    }

    //Invocando Función de Configuración
    ConfiguraWucCobroRecurrente();
</script>
<div class="columna2x">
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="ddlTipoEntApl">Tipo Entidad</label>
        </div>
        <div class="control">
            <asp:UpdatePanel ID="upddlTipoEntApl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlTipoEntApl" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="txtEntidad">Entidad</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel ID="uptxtEntidad" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtEntidad" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon">
        <div class="etiqueta">
            <label for="txtTotalDeuda">Total Deuda</label>
        </div>
        <div class="control_100px">
            <asp:UpdatePanel ID="uptxtTotalDeuda" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtTotalDeuda" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" Enabled="false"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon">
        <div class="etiqueta">
            <label for="txtDescuento">Descuento</label>
        </div>
        <div class="control_100px">
            <asp:UpdatePanel ID="uptxtDescuento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtDescuento" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber], funcCall[MaxValue[]]]"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta">
            <label> Por Dia</label>
        </div>
    </div>
    <div class="renglon">
        <div class="etiqueta">
            <label for="txtFecInicio">Inicio</label>
        </div>
        <div class="control">
            <asp:UpdatePanel ID="uptxtFecInicio" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtFecInicio" runat="server" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
