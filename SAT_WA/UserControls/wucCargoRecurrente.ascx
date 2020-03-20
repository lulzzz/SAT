<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCargoRecurrente.ascx.cs" Inherits="SAT.UserControls.wucCargoRecurrente" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlCargoRecurrente();
        }
    }
    //Función encargada de reconstruir los Scripts de Validación
    function ConfiguraJQueryControlCargoRecurrente() {
        $(document).ready(function () {
            //Declarando Función de Validación
            var validaCargoRecurrente = function (evt) {
                var isValid1 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtValorUnitario.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado de Validación
                return isValid1 && isValid2
            }
            //Añadiendo Función a Evento Click
            $("#<%=btnGuardar.ClientID%>").click(validaCargoRecurrente);
        });
    }
    //Invocando Funcion de Configuracion
    ConfiguraJQueryControlCargoRecurrente();
</script>
<div class="contenido_pestañas_documentacion">
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x"></div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="ddlTipo">Tipo</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="txtCantidad">Cantidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCantidad" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="ddlUnidad">Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="txtValorUnitario">Valor Unitario</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtValorUnitario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorUnitario" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnEliminar" runat="server" CssClass="boton" Text="Eliminar" 
                                OnClick="btnEliminar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" 
                                OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" 
                                OnClick="btnGuardar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamano">Mostrar:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamano" runat="server" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"
                                TabIndex="5" CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label>Ordenado:</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblCriterio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblCriterio" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvResultado" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlr">
                    <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_200px_altura">
                <asp:UpdatePanel ID="upgvResultado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvResultado" runat="server" AllowPaging="true" AllowSorting="true"
                            OnPageIndexChanging="gvResultado_PageIndexChanging" OnSorting="gvResultado_Sorting"
                            PageSize="5" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True"
                            AutoGenerateColumns="false" EmptyDataText="No hay registros" Width="100%">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                                <asp:BoundField DataField="ValorU" HeaderText="Valor Unitario" SortExpression="ValorU" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkEditar_Click" Text="Editar"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>        
    </div>
    
</div>