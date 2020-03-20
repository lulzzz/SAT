<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMovimientoVacio.ascx.cs" Inherits="SAT.UserControls.wucMovimientoVacio" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryMovimientoVacio();
        }
    }
    //Creando función para configuración de jquery en formulario
    function ConfiguraJQueryMovimientoVacio() {
        $(document).ready(function () {
            //Función de validación CambioUnidad
            var validacionMovimientoVacio = function (evt) {
                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                var isValid1 = !$("#<%=txtNombreTercero.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtFechaSalidaUnidades.ClientID%>").validationEngine('validate');
    return isValid1 && isValid2
};
    //Botón Buscar Servicios
    $("#<%=  btnAceptarMovimientoVacio.ClientID %>").click(validacionMovimientoVacio);
    $("#<%=txtFechaSalidaUnidades.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    // *** Catálogos Autocomplete *** //
    $("#<%=txtNombreTercero.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=18&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
        appendTo: "<%=this.Contenedor%>"
    });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryMovimientoVacio();
</script>
<div class="contenido_pestañas_documentacion">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta" >
<label for="txtFechaSalidaUnidades" class="Label">
Fecha Salida</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaSalidaUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaSalidaUnidades" Enabled="true" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"  TabIndex="11"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlOperador">Operador</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtOperador" runat="server" CssClass="dropdown"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="ddlUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreTercero">Tercero</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombreTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreTercero" runat="server" CssClass="textbox validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkTercero" runat="server" AutoPostBack="true" Text="Tercero" TabIndex="8"  OnCheckedChanged="chkTercero_OnCheckedChanged"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlUnidad">Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddLUnidad" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlUnidad" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlUnidad_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlRemolque1">Remolque 1</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlRemolque1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlRemolque1" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlRemolque2">Remolque 2</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlRemolque2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlRemolque2" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlDolly">Dolly</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlDolly" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlDolly" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_155px">
<label for="txtRemolque13ero">1 Rem. 3ero</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtRemolque13ero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRemolque13ero" runat="server" CssClass="textbox" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_155px">
<label for="txtRemolque23ero">2 Rem. 3ero</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtRemolque23ero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRemolque23ero" runat="server" CssClass="textbox" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblbError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarMovimientoVacio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarMovimientoVacio" runat="server"  OnClick="OnClick_btnCancelar"  CssClass="boton_cancelar" Text="Cancelar"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarMovimientoVacio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarMovimientoVacio" runat="server" CssClass="boton" Text="Aceptar"  OnClick="OnClick_btnRegistrar"/>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>