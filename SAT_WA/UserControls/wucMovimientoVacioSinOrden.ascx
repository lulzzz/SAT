<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMovimientoVacioSinOrden.ascx.cs" Inherits="SAT.UserControls.MovimientoVacioSinOrden" %>
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
    var isValid3 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
    return isValid1 && isValid2 && isValid3
};
    //Botón Buscar Servicios
    $("#<%=  btnAceptarMovimientoVacioSinOrden.ClientID %>").click(validacionMovimientoVacio);
    $("#<%=txtFechaSalidaUnidades.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    // *** Catálogos Autocomplete *** //
    $("#<%=txtNombreTercero.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=18&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
        appendTo: "<%=this.Contenedor%>"
    });
    //Sugerencias de Ubicación
    $("#<%= txtUbicacion.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
        appendTo: "<%=this.Contenedor%>"
    });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryMovimientoVacio();
</script>
<div class="seccion_controles">
<div class="columna550px">
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblLugarOrigen">Origen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblLugarOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblLugarOrigen" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblFechaLlegada" class="Label">Fecha Llegada</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblFechaLlegada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaLlegada" runat="server" class="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="ddlRemolque1" />
<asp:AsyncPostBackTrigger ControlID="ddlRemolque2" />
<asp:AsyncPostBackTrigger ControlID="ddlDolly" />
<asp:AsyncPostBackTrigger ControlID="ddlUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacion">Destino</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    <div class="renglon2x">
<div class="etiqueta" >
<label for="txtFechaSalidaUnidades" class="Label">
Fecha Salida</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaSalidaUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaSalidaUnidades" Enabled="true" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="chkFechaActualMov" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkFechaActualMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkFechaActualMov" runat="server" AutoPostBack="true" Text="Fecha Actual" OnCheckedChanged="chkFechaActualMov_CheckedChanged" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreTercero">Operador</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkTercero" runat="server" AutoPostBack="true" Text="Tercero" OnCheckedChanged="chkTercero_OnCheckedChanged"  />
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
<asp:DropDownList ID="ddlUnidad" AutoPostBack="true" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlUnidad_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlRemolque1">Remolque 1</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlRemolque1"  runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlRemolque1" AutoPostBack="true" OnSelectedIndexChanged="ddlRemolque1_SelectedIndexChanged" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
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
<asp:DropDownList ID="ddlRemolque2" runat="server" OnSelectedIndexChanged="ddlRemolque2_SelectedIndexChanged" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
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
<asp:DropDownList ID="ddlDolly" runat="server" OnSelectedIndexChanged="ddlDolly_SelectedIndexChanged"
     CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtRemolque13ero">Rem. 3ero 1</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtRemolque13ero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRemolque13ero" runat="server" CssClass="textbox" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtRemolque23ero">Rem. 3ero 2</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtRemolque23ero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRemolque23ero" runat="server" CssClass="textbox" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarMovimientoVacioSinOrden" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMovimientoVacioSinOrden" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarMovimientoVacioSinOrden" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarMovimientoVacioSinOrden" runat="server"  OnClick="OnClick_btnCancelar"  CssClass="boton_cancelar" Text="Cancelar"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarMovimientoVacioSinOrden" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarMovimientoVacioSinOrden" runat="server" CssClass="boton" Text="Aceptar"  OnClick="OnClick_btnRegistrar"/>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>