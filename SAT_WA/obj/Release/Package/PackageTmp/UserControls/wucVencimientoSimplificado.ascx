<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucVencimientoSimplificado.ascx.cs" Inherits="SAT.UserControls.wucVencimientoSimplificado" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryVencimientoSimp();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryVencimientoSimp() {
    $(document).ready(function () {

        //Cargando Controles de Fechas
        $("#<%=txtFecIni.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
        $("#<%=txtFecFin.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });

        //Validando al Momento de Guardar
        $("#<%=btnGuardar.ClientID%>").click(function () {

            //Validando Controles
            var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
            var isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');            

            //Devolviendo Resultado
            return isValid1 && isValid2;
        });

        //Validando al Momento de Terminar
        $("#<%=btnTerminar.ClientID%>").click(function () {

            //Validando Controles
            var isValid1 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');

            //Devolviendo Resultado
            return isValid1;
        });

    });
}

//Invocando Función
ConfiguraJQueryVencimientoSimp();

//Declarando Función de Validación de Fechas
function CompareDates() {
//Obteniendo Valores
var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

//Fecha en Formato MM/DD/YYYY
var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

//Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
if (date1 > date2)
    //Mostrando Mensaje de Operación
    return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}
</script>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Vencimiento</h2>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblEntidad" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblRegistro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRegistro" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="ddlTipoVencimiento">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoVencimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoVencimiento" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
</div>
<div class="etiqueta_320px">
<label class="label_negrita">Los Tipos de Vencimiento que contienen (*) no permitiran que la unidad se pueda utilizar</label>
</div>
</div>
<div class="renglon2x" style="float:left"></div>
<div class="renglon2x" style="float:left">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="txtFecIni">Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon" style="float:left">
<div class="etiqueta">
<label for="txtFecFin">Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton" style="float:left">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnTerminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnTerminar" runat="server" CssClass="boton" Text="Terminar" OnClick="btnTerminar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
</div>
</div>