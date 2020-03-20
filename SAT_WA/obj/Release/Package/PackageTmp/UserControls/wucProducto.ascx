<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProducto.ascx.cs" Inherits="SAT.UserControls.wucProducto" %>
<script type='text/javascript'>
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
    if (args.get_error() == undefined) {
    ConfiguraJQueryControlProducto();
    }
    }
    //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQueryControlProducto() {
        $(document).ready(function () {
                                        
            //Función de validación de campos
            var validacionProducto = function () {
                var isValidP1;
                                        
                //Validando la Visibilidad del Control
                if($("#<%=txtProductoCarga.ClientID%>").is(':visible') == true){
                    //Validando Producto de Carga
                    isValidP1 = !$("#<%=txtProductoCarga.ClientID%>").validationEngine('validate');
                }
                else {
                    //Validando Producto de Descarga
                    isValidP1 = !$("#<%=txtProductoDescarga.ClientID%>").validationEngine('validate');
                }

                //Validando el Resto de los Controles
                var isValidP2 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
                var isValidP3 = !$("#<%=txtPeso.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenidos
                return isValidP1 && isValidP2 && isValidP3;
            };

            //Función de validación de campos
            var validacionCalculoCantidadProducto = function () 
            {
            var isValidP1;
                                        
            //Validando Producto de Descarga
            isValidP1 = !$("#<%=txtProductoDescarga.ClientID%>").validationEngine('validate');
                                        
            return isValidP1;
            };
                                        
            //Validación de campos requeridos
            $("#<%=lnkProducto.ClientID%>").click(validacionCalculoCantidadProducto); 

            //Validación de campos requeridos
            $("#<%=btnGuardarProducto.ClientID%>").click(validacionProducto);

            //Sugerencias de producto
            $("#<%=txtProductoCarga.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>&param2=1&param3=<%=this.idServicio%>',
                appendTo: "<%=this.Contenedor%>"
                });
            //Sugerencias de producto
            $("#<%=txtProductoDescarga.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>&param2=2&param3=<%=this.idServicio%>',
                appendTo: "<%=this.Contenedor%>"
                });
                                    
        });
    }
    //Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryControlProducto();
</script>
<!-- Contenido del Control -->
<div class="contenedor_seccion_95per">
    <div class="header_seccion">
        <img src="../Image/ArmadoPaquete.png" />
        <h2>Productos del Servicio</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlParadas">Lugar</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlParadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlParadas" runat="server" CssClass="dropdown2x" OnSelectedIndexChanged="ddlParadas_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlTipo">Actividad</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplnkProducto" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="lnkProducto" OnClick="lnkProducto_Click" Text="Producto"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTipo" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtProducto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtProductoCarga" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="true"></asp:TextBox>
                        <asp:TextBox ID="txtProductoDescarga" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTipo" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtCantidad">Cantidad</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCantidad" runat="server" TabIndex="4" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkProducto" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlUnidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlUnidad" runat="server" TabIndex="5" CssClass="dropdown"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkProducto" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtPeso">Peso</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtPeso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtPeso" runat="server" TabIndex="6" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkProducto" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlUnidadPeso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlUnidadPeso" runat="server" TabIndex="7" CssClass="dropdown"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="ddlParadas" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkProducto" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta"></div>
            <div class="control">
                <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblId" runat="server" Text="Por Asignar" Visible="false"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="label_error" Text="Error"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                    <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardarProducto" runat="server" Text="Agregar" TabIndex="8"
                            OnClick="btnGuardar_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                            OnClick="btnCancelar_Click" CssClass="boton_cancelar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        
    </div>
    <div class="contenedor_55per_derecha">
        <div class="renglon100per">
            <div class="etiqueta">
                <label for="ddlTamanoReqDisp">Registros</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoReqDisp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label for="ddlTamanoReqDisp"></label>
                        <asp:DropDownList ID="ddlTamanoReqDisp" runat="server" CssClass="dropdown" TabIndex="9" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamanoReqDisp_SelectedIndexChanged" Enabled="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label>Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenarReqDisp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenarReqDisp" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicioProductos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click" Text="Exportar Excel" TabIndex="10"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvServicioProductos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvServicioProductos" runat="server" TabIndex="11" PageSize="10" EmptyDataText="No hay registros"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvServicioProductos_Sorting" Width="100%"
                        OnPageIndexChanging="gvServicioProductos_PageIndexChanging" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="IdParada" HeaderText="IdParada" SortExpression="IdParada" Visible="false" />
                            <asp:BoundField DataField="Parada" HeaderText="Parada" SortExpression="Parada" />
                            <asp:BoundField DataField="IdTipo" HeaderText="IdTipo" SortExpression="IdTipo" Visible="false" />
                            <asp:BoundField DataField="IdProducto" HeaderText="IdProducto" SortExpression="IdProducto" Visible="false" />
                            <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UniCant" HeaderText="Unidad Cantidad" SortExpression="UniCant" />
                            <asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UniPeso" HeaderText="Unidad Peso" SortExpression="UniPeso" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkEditar_Click" Text="Editar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" OnClick="lnkEliminar_Click" Text="Eliminar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="OnClick_lkbBitacora"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbBitacora" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarProducto" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoReqDisp" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>
</div>
