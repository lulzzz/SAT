<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucImpresionPorte.ascx.cs" Inherits="SAT.UserControls.wucImpresionPorte" %>
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>

<!-- Declarando Script de Utilización de Funciones del Control de Usuario -->
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    //Declarando Función de Fin de Petición
    function EndRequestHandler(sender, args) {
        //Validando que no existan errores
        if (args.get_error() == undefined) {
            //Invocando Función de Configuración
            ConfiguraJQueryWucImpresionCP();
        }
    }

    //Declarando función de Configuración
    function ConfiguraJQueryWucImpresionCP() {
        $(document).ready(function () {



        });
    }

    //Invocando Función de Configuración
    ConfiguraJQueryWucImpresionCP();
</script>

<div class="columna2x">
    <div class="header_seccion">
        <asp:UpdatePanel ID="uplblEncabezado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <img src="../Image/Exclamacion.png" />
                <h2><asp:Label ID="lblEncabezado" runat="server" Text="Seleccione los recursos a mostrar en la Carta Porte"></asp:Label></h2>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="uppnlOperador" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlOperador" runat="server">
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="ddlOperador">Operador</label>
                    </div>
                    <div class="control2x">
                        <asp:DropDownList ID="ddlOperador" runat="server" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppnlUnidadMotriz" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlUnidadMotriz" runat="server">
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="ddlUnidadMotriz">Unidad</label>
                    </div>
                    <div class="control2x">
                        <asp:DropDownList ID="ddlUnidadMotriz" runat="server" CssClass="dropdown2x" TabIndex="2"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppnlUnidadArrastre1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlUnidadArrastre1" runat="server">
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="ddlUnidadArrastre1">Caja 1</label>
                    </div>
                    <div class="control2x">
                        <asp:DropDownList ID="ddlUnidadArrastre1" runat="server" CssClass="dropdown2x" TabIndex="3"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="uppnlUnidadArrastre2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlUnidadArrastre2" runat="server">
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="ddlUnidadArrastre2">Caja 2</label>
                    </div>
                    <div class="control2x">
                        <asp:DropDownList ID="ddlUnidadArrastre2" runat="server" CssClass="dropdown2x" TabIndex="4"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="renglon2x" style="float:left; padding-bottom:10px; height:30px;">
        <div class="validador" style="float:right; height:30px">
            <asp:UpdatePanel ID="upbtnImprimir" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton ID="imbImprimir" runat="server" ImageUrl="~/Image/imprimir.png" Width="32" Height="32"
                        OnClick="imbImprimir_Click" TabIndex="6" ToolTip="Imprimir Carta Porte" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="imbImprimir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>