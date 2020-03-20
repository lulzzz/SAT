<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDevolucionFaltante.ascx.cs" Inherits="SAT.UserControls.wucDevolucionFaltante" %>
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)      
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryDevolucionFaltante();
        }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryDevolucionFaltante() {
        $(document).ready(function () {

            $("#<%=btnGuardar.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtFechaDevolucion.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado
                return isValid1;
            });

            //Validando Controles del Detalle
            $("#<%=btnGuardarDetalle.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtDescripcionProd.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtCodProducto.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado
                return isValid1 && isValid2 && isValid3;
            });

            //Cargando Control de Fecha
            $("#<%=txtFechaDevolucion.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });

            //Cargando Catalogos de Producto
            $("#<%=txtCodProducto.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=41',
                appendTo: "<%=this.Contenedor%>",
                select: function (event, ui) {

                    //Asignando Selección al Valor del Control
                    $("#<%=txtCodProducto.ClientID%>").val(ui.item.value);

                    //Causando Actualización del Control
                    __doPostBack('<%= txtCodProducto.UniqueID %>', '');
                }
            });
        });
    }

    //Invocando Función de Configuración
    ConfiguraJQueryDevolucionFaltante();
</script>
<div class="columna2x">
    <div class="header_seccion">
        <h2>Devoluciones</h2>
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="lblNoDevolucion">No. Devolución</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel ID="uplblNoDevolucion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="etiqueta">
                        <b><asp:Label ID="lblNoDevolucion" runat="server"></asp:Label></b>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
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
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="ddlEstatus">Estatus</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="txtFechaDevolucion">Fecha</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel ID="uptxtFechaDevolucion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtFechaDevolucion" runat="server" CssClass="textbox2x" MaxLength="16"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div><br />
    </div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="txtObservacion">Observación</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" MaxLength="150"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x">
        <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAgregarRef" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnAgregarRef" runat="server" Text="Agregar Ref." CssClass="boton_cancelar" OnClick="btnAgregarRef_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnGuardar" runat="server" CssClass="boton" OnClick="btnGuardar_Click" Text="Guardar" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" OnClick="btnCancelar_Click" Text="Cancelar" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div class="columna2x">
    <div class="header_seccion">
        <h2>Referencias</h2>
    </div>
    <div class="renglon2x">
        <div class="etiqueta_50px">
            <label for="ddlTamano">Mostrar</label>
        </div>
        <div class="control_100px">
            <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta_50px">
            <label>Ordenado</label>
        </div>
        <div class="etiqueta_155px">
            <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <b>
                        <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvReferencias" EventName="Sorting" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta_50pxr">
            <asp:UpdatePanel ID="uplkbExcel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lkbExcel" runat="server" Text="Exportar" CommandName="Referencias" OnClick="lkbExcel_Click"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lkbExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="grid_seccion_completa_100px_altura">
        <asp:UpdatePanel ID="upgvReferencias" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvReferencias" runat="server" OnSorting="gvReferencias_Sorting" CssClass="gridview"
                    OnPageIndexChanging="gvReferencias_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                    ShowFooter="true" Width="100%" PageSize="5" AutoGenerateColumns="false">
                    <AlternatingRowStyle CssClass="gridviewrowalternate" />
                    <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                    <FooterStyle CssClass="gridviewfooter" />
                    <HeaderStyle CssClass="gridviewheader" />
                    <RowStyle CssClass="gridviewrow" />
                    <SelectedRowStyle CssClass="gridviewrowselected" />
                    <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                    <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    <Columns>
                        <asp:BoundField DataField="NombreTipo" HeaderText="Tipo" SortExpression="NombreTipo" />
                        <asp:BoundField DataField="ValorReferencia" HeaderText="Referencia" SortExpression="ValorReferencia" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="header_seccion">
    <h2>Detalles</h2>
</div>
<div class="renglon_pestaña_documentacion" style="width: 1050px">
    <div class="control_100px">
        <label>Estatus</label>
    </div>
    <div class="control_60px">
        <label>Cantidad</label>
    </div>
    <div class="control_100px">
        <label>Unidad</label>
    </div>
    <div class="control_100px">
        <label>Código Producto</label>
    </div>
    <div class="control2x">
        <label>Descripción Producto</label>
    </div>
    <div class="control">
        <label>Razon Detalle</label>
    </div>
</div>
<div style="float:left;">
<div class="renglon_pestaña_documentacion" style="width: 1050px">
    <div class="control_100px">
        <asp:UpdatePanel ID="upddlEstatusDet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlEstatusDet" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control_60px">
        <asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="txtCantidad" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control_100px">
        <asp:UpdatePanel ID="upddlUnidad" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control_100px">
        <asp:UpdatePanel ID="uptxtCodProducto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="txtCodProducto" runat="server" CssClass="textbox_100px validate[required]" MaxLength="100"
                    OnTextChanged="txtCodProducto_TextChanged" AutoPostBack="true"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control2x">
        <asp:UpdatePanel ID="uptxtDescripcionProd" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="txtDescripcionProd" runat="server" CssClass="textbox2x validate[required]" MaxLength="250"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtCodProducto" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control">
        <asp:UpdatePanel ID="upddlRazonDet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlRazonDet" runat="server" CssClass="dropdown"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="controlBoton">
        <asp:UpdatePanel ID="upbtnGuardarDetalle" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnGuardarDetalle" runat="server" CssClass="boton" OnClick="btnGuardarDetalle_Click" Text="Guardar" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="controlBoton">
        <asp:UpdatePanel ID="upbtnCancelarDetalle" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnCancelarDetalle" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarDetalle_Click" Text="Cancelar" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<br />
<div class="renglon3x">
    <div class="etiqueta">
        <label for="ddlTamanoDet">Mostrar</label>
    </div>
    <div class="control">
        <asp:UpdatePanel ID="upddlTamanoDet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlTamanoDet" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoDet_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="etiqueta_50px">
        <label for="lblOrdenadoDet">Ordenado</label>
    </div>
    <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblOrdenadoDet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <b>
                    <asp:Label ID="lblOrdenadoDet" runat="server"></asp:Label></b>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvDetalles" EventName="Sorting" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="etiqueta_50pxr">
        <asp:UpdatePanel ID="uplkbExportarDet" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="lkbExportarDet" runat="server" Text="Exportar" CommandName="Detalles" OnClick="lkbExcel_Click"></asp:LinkButton>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lkbExportarDet" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplbkImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbImprimir_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
    <asp:UpdatePanel ID="upgvDetalles" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvDetalles" runat="server" OnSorting="gvDetalles_Sorting" CssClass="gridview"
                OnPageIndexChanging="gvDetalles_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                ShowFooter="true" Width="100%" PageSize="5" AutoGenerateColumns="false" 
                OnRowDataBound="gvDetalles_RowDataBound">
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
                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                    <asp:BoundField DataField="CodigoProducto" HeaderText="Codigo Producto" SortExpression="CodigoProducto" />
                    <asp:BoundField DataField="DescripcionProducto" HeaderText="Descripción Producto" SortExpression="DescripcionProducto" />
                    <asp:BoundField DataField="RazonDetalle" HeaderText="Razon Detalle" SortExpression="RazonDetalle" />
                    <asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbReferenciaDetalle" runat="server" Text='<%# Eval("Referencia") %>' OnClick="lkbReferenciaDetalle_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditarDetalle" runat="server" Text="Editar" OnClick="lkbEditarDetalle_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEliminarDetalle" runat="server" Text="Eliminar" OnClick="lkbEliminarDetalle_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="btnGuardarDetalle" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarDetalle" />
            <asp:AsyncPostBackTrigger ControlID="ddlTamanoDet" />
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>