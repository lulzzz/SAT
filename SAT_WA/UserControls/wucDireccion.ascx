<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDireccion.ascx.cs" Inherits="SAT.UserControls.wucDireccion" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlDireccion();
        }
    }
    //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQueryControlDireccion() {
        $(document).ready(function () {
            //Función de validación de campos
            var validacionDireccion = function (evt) {
                var isValidP1 = !$("#<%=txtCalle.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtColonia.ClientID%>").validationEngine('validate');
    var isValidP3 = !$("#<%=txtMunicipio.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtCP.ClientID%>").validationEngine('validate');

    return isValidP1 && isValidP2 && isValidP3 && isValidP4;
};
    //Boton Guardar
    $("#<%=btnAceptar.ClientID%>").click(validacionDireccion);
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryControlDireccion();
</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/Direccion.png" />
        <h2>Datos dirección</h2>
    </div>
    <div class="columna2x">       
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtCalle">Calle</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtCalle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCalle" runat="server" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtNoExt">No. Exterior</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtNoExt" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtNoExt" runat="server" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtNoInt">No. Interior</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtNoInt" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtNoInt" runat="server" CssClass="textbox2x"
                            MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtColonia">Colonia</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtColonia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtColonia" runat="server"
                            CssClass="textbox2x validate[required]" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtLocalidad">Ciudad</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtLocalidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtLocalidad" runat="server"
                            CssClass="textbox2x validate[required]" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtMunicipio">Municipio</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtMunicipio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtMunicipio" runat="server"
                            CssClass="textbox2x validate[required]" MaxLength="50"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">                
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblID" runat="server" CssClass="LabelResalta" Visible="false">Por Asignar</asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>           
        </div>

    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="ddlPais">Pais</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlPais" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlPais" runat="server" CssClass="dropdown2x"
                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlIDSTA">Estado</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlIDSTA" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlIDSTA" runat="server"
                            CssClass="dropdown2x" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPais" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtCP">Código Postal</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtCP" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCP" runat="server"
                            CssClass="textbox2x validate[required]" MaxLength="5"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="txtReferencia">Referencia</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtReferencia" runat="server"
                            CssClass="textbox2x" MaxLength="200"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label class="Label" for="ddlTipoUbicacion">Tipo</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlTipoUbicacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTipoUbicacion" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x"></div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptar" runat="server" CssClass="boton"
                            OnClick="btnAceptar_OnClick" Text="Aceptar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="boton"
                            OnClick="btnBuscar_Click" Text="Buscar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar"
                            OnClick="btnCancelar_OnClick" Text="Cancelar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />


                        <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div class="contenedor_seccion_completa" style="width:1147px">
    <div class="header_seccion">
        <img src="../Image/TablaResultado.png" />
        <h2>Direcciones encontradas</h2>
    </div>
    <div class="renglon3x">
        <div class="etiqueta">
            <label class="Label" for="ddlTamaño">Mostrar:</label>
        </div>
        <div class="control">
            <asp:UpdatePanel ID="upddlTamaño" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlTamaño" runat="server" AutoPostBack="True" CssClass="dropdown"
                        OnSelectedIndexChanged="ddlTamaño_SelectedIndexChanged">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta">
            <label class="Label" for="lblOrden">Ordenados por: </label>
        </div>
        <div class="etiqueta">
            <asp:UpdatePanel runat="server" ID="upOrdenGrid" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblOrden" runat="server"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvUbicaciones" EventName="Sorting" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta">
            <asp:UpdatePanel ID="uplkbExportar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lkbExportar" runat="server" CausesValidation="False" OnClick="lkbExportar_Click">Exportar</asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lkbExportar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="grid_seccion_completa_altura_variable">
        <asp:UpdatePanel ID="upgvUbicaciones" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvUbicaciones" runat="server" AllowPaging="true" AllowSorting="true"
                    OnPageIndexChanging="gvUbicaciones_PageIndexChanging" OnSorting="gvUbicaciones_Sorting"
                    ShowFooter="true" PageSize="5" Width="100%" AutoGenerateColumns="false">
                    <AlternatingRowStyle CssClass="gridviewrowalternate" />
                    <FooterStyle CssClass="gridviewfooter" />
                    <HeaderStyle CssClass="gridviewheader" />
                    <RowStyle CssClass="gridviewrow" />
                    <SelectedRowStyle CssClass="gridviewrowselected" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                        <asp:BoundField DataField="TipoUbicacion" HeaderText="Tipo Ubicación" SortExpression="TipoUbicacion" />
                        <asp:BoundField DataField="Pais" HeaderText="País" SortExpression="Pais" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
                        <asp:BoundField DataField="Municipio" HeaderText="Municipio" SortExpression="Municipio" />
                        <asp:BoundField DataField="Localidad" HeaderText="Localidad" SortExpression="Localidad" />
                        <asp:BoundField DataField="Colonia" HeaderText="Colonia" SortExpression="Colonia" />
                        <asp:BoundField DataField="Calle" HeaderText="Calle" SortExpression="Calle" />
                        <asp:BoundField DataField="Numero" HeaderText="Número" SortExpression="Numero" />
                        <asp:BoundField DataField="CP" HeaderText="CP" SortExpression="CP" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbEditar" runat="server" OnClick="lkbEditar_Click"
                                    Text="Editar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbSeleccionar" runat="server" OnClick="lkbSeleccionar_Click"
                                    Text="Seleccionar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbEliminar" runat="server" OnClick="lkbEliminar_Click"
                                    Text="Eliminar"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamaño" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>


