<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCambioOperador.ascx.cs" Inherits="SAT.UserControls.wucCambioOperador" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlCambioOperador();
        }
    }
    //Función encargada de reconstruir los Scripts de Validación
    function ConfiguraJQueryControlCambioOperador() {
        $(document).ready(function () {
            //Función de validación de cambio de operador
            var validacionCambioOperador = function (evt) {
                var isValidP1 = !$("#<%=txtNvoOperador.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
                return isValidP1 && isValidP2;
};
            //Boton Confirmación Cambio Operador
            $("#<%=btnAceptarCambioOperador.ClientID%>").click(validacionCambioOperador);
            //Cargando Catalogo AutoCompleta Operadores Disponibles
            $("#<%=txtNvoOperador.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=25&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                appendTo: "<%=this.Contenedor%>"
            });
            //Autocomplete
            $("#<%=txtUnidad.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=49&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
                appendTo: "<%=this.Contenedor%>"
            });
            //Fecha de Inicio de asignación de operador
            $("#<%=txtFechaInicioAsignacion.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
});
}
//Invocando Funcion de Configuracion
ConfiguraJQueryControlCambioOperador();
</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <h3>Cambio de Operador Principal</h3>
    </div>
    <div class="columna">
        <div class="renglon2x">
            <label class="mensaje_modal">Seleccione un Operador de la lista que se despliega.</label>
        </div>
                <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtUnidad">Unidad</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUnidad" OnTextChanged="txtUnidad_TextChanged" AutoPostBack="true" runat="server" Enabled="false" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtNvoOperador">Nvo. Operador</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtNvoOperador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtNvoOperador" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtFechaInicioAsignacion">Fecha Inicio</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFechaInicioAsignacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFechaInicioAsignacion" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="lblOperadorActual">Operador Actual</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uplblOperadorActual" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOperadorActual" runat="server" CssClass="label_negrita"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="txtUnidad" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnAceptarCambioOperador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptarCambioOperador" runat="server" CssClass="boton" Text="Aceptar" OnClick="OnClick_btnRegistrar" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
