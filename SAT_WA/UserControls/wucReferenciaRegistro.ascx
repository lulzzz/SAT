<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReferenciaRegistro.ascx.cs" Inherits="SAT.UserControls.wucReferenciaRegistro" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryControlReferencia();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryControlReferencia() {
$(document).ready(function () {

//Función de validación de campos
var validacionReferencia = function () {
    var isValidP1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
return isValidP1;
};
    //Función de validación de campos del GridView
    var validacionValorReferencia = function () {
        var isValidP1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
//Validación de campos requeridos
    $("#<%=btnAgregar.ClientID%>").click(validacionReferencia);

    $("#gvReferencias tr input[id*='txtValorReferencia']").each(function () {
        $(this).keydown(function (event) {
            // Just to find all non-decimal characters and replace them with blank.
            $(this).val($(this).val().replace(/[^\d].+/, ""));

            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                event.preventDefault();

                isValidated = false;
            }
            else { isValidated = true; }
        });
    });
    
});

}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryControlReferencia();
</script>
<div class="contenido_control_referencia">
    <div class="encabezado_control">
        <h2>Referencias Ligadas</h2>
    </div>
    <div class="contenido_resultado_referencia">
        <div class="arbol_referencia">
            <asp:UpdatePanel ID="uplArbol" runat="server" UpdateMode="Always">
                <ContentTemplate>                    
                    <asp:TreeView ID="trvReferencias" runat="server" ShowCheckBoxes="None" 
                        OnSelectedNodeChanged="trvReferencias_SelectedNodeChanged" CssClass="arbol"
                        ShowLines="True" ImageSet="Arrows">
                        <ParentNodeStyle CssClass="nodoArbolHoja1" />
                        <HoverNodeStyle CssClass="nodoArbolTocado" />
                        <SelectedNodeStyle CssClass="nodoArbolSeleccionado" />
                        <RootNodeStyle CssClass="nodoArbolRaiz" />
                        <NodeStyle CssClass="nodoArbol" />
                    </asp:TreeView>                                        
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="grid_referencia">
            <asp:UpdatePanel ID="uplgvReferencias" runat="server">
                <ContentTemplate>                
                <asp:GridView ID="gvReferencias" runat="server" AutoGenerateColumns="False" 
                Width="100%" Height="100%" PageSize="5" CssClass="gridview">
                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                <FooterStyle CssClass="gridviewfooter" />
                <HeaderStyle CssClass="gridviewheader" />
                <RowStyle CssClass="gridviewrow" />
                <SelectedRowStyle CssClass="gridviewrowselected" />
                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                <Columns>
                    <asp:BoundField DataField="nombre_grupo" HeaderText="Grupo Referencia" />
                    <asp:BoundField DataField="nombre_tipo" HeaderText="Tipo Referencia" />
                    <asp:TemplateField HeaderText="Referencia" SortExpression="valor_referencia">
                    <ItemTemplate>
                    <asp:Label ID="lblValorReferencia" runat="server" Text='<%# Bind("valor_referencia") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                    <asp:TextBox ID="txtValorReferencia" runat="server" MaxLength="500" CssClass="textbox validate[required]" Text='<%# Bind("valor_referencia") %>'></asp:TextBox>
                    </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="" />
                    <asp:TemplateField HeaderText="Editar">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server" onclick="lnkEditar_Click">Editar</asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkAceptar" runat="server" onclick="lnkAceptar_Click">Aceptar</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkCancelar" runat="server" onclick="lnkCancelar_Click">Cancelar</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan = "2">
                                    <asp:LinkButton ID="lnkEliminar" runat="server" onclick="lnkEliminar_Click">Eliminar</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_filtro_referencia">
        <div class="columna2x">
            <div class="renglon2x"></div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtReferencia">Referencia</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]" MaxLength="500"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton"></div>
                <div class="controlBoton">
                    <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" CssClass="boton"
                        Text="Agregar" CausesValidation="False" />
                </div>
            </div>
        </div>
    </div>
    <div class="imagen_control">
        <img src="../Image/Referencias.png" />
    </div>
</div>
