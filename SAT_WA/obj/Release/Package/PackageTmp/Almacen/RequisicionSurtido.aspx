<%@ Page Title="Requisición Surtido" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequisicionSurtido.aspx.cs" Inherits="SAT.Almacen.RequisicionSurtido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraRequisicionSurtido();
            }
        }

        //Función de Configuración
        function ConfiguraRequisicionSurtido() {
            $(document).ready(function () {

                //Validando Control
                $("#<%=btnSurtir.ClientID%>").click(function () {

                    //Validando Controles
                    var isValid1 = !$("#<%=txtCantidadReq.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultado Obtenido
                    return isValid1;
                });

            });
        }

        //Invocando Función de Configuración
        ConfiguraRequisicionSurtido();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/EntradasSalidas.png" />
        <h1>Requisiciones Surtido</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lblNoRequisicion">No. Requisición</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblNoRequisicion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblNoRequisicion" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompania">Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
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
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtAlmacen">Almacen</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSolicitante">Solicitante</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtSolicitante" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSolicitante" runat="server" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta_155px">
                    <label for="txtFechaSolicitud">Fecha Solicitud</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaSolicitud" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaSolicitud" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="txtFechaEntReq">Fecha Entrega Req.</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaEntReq" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaEntReq" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="txtFechaEntrega">Fecha Entrega</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaEntrega" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Totales.png" />
            <h2>Productos Requeridos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
                            TabIndex="1" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenado">Ordenado</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplkbExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportar" runat="server" Text="Exportar" OnClick="lkbExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvProductosRequeridos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvProductosRequeridos" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                        OnPageIndexChanging="gvProductosRequeridos_PageIndexChanging" OnSorting="gvProductosRequeridos_Sorting"
                        CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
                            <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                            <asp:BoundField DataField="CodProducto" HeaderText="Cod. Producto" SortExpression="CodProducto" />
                            <asp:BoundField DataField="CantidadInicial" HeaderText="Cantidad Solicitada" SortExpression="CantidadInicial" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad Restante" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="CantidadAbastecida" HeaderText="Cantidad Abastecida" SortExpression="CantidadAbastecida" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad Medida" SortExpression="UnidadMedida" />
                            <asp:TemplateField HeaderText="Relacionados" SortExpression="Relacionados">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbRelacionados" runat="server" Text="Relacionados" OnClick="lkbRelacionados_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Disponibles" HeaderText="Disponibles" SortExpression="Disponibles" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Surtir">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAutomatico" runat="server" Text="Automatico" CommandName="Automatico" OnClick="lkbSurtir_Click"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbManual" runat="server" Text="Manual" CommandName="Manual" OnClick="lkbSurtir_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="gvDesempaquetarProducto" />
                    <asp:AsyncPostBackTrigger ControlID="btnSurtir" />
                    <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana para Desempaquetar Productos -->
    <div id="contenedorVentanaProductoDesempaquetado" class="modal">
        <div id="ventanaProductoDesempaquetado" class="contenedor_ventana_confirmacion_arriba" style="width:640px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialBitacora" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarProductoDesempaquetado" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ProductoDesempaquetado" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Desempaquetar Producto</h2>
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamanoProdDes">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoProdDes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoProdDes" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoProdDes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblOrdenadoProdDes">Ordenado</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblOrdenadoProdDes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblOrdenadoProdDes" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvDesempaquetarProducto" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50pxr">
                        <asp:UpdatePanel ID="uplnkExportarProdDes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarProdDes" runat="server" Text="Exportar" OnClick="lnkExportarProdDes_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarProdDes" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvDesempaquetarProducto" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvDesempaquetarProducto" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                                OnPageIndexChanging="gvDesempaquetarProducto_PageIndexChanging" OnSorting="gvDesempaquetarProducto_Sorting"
                                PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                    <asp:BoundField DataField="CodProducto" HeaderText="Cod. Producto" SortExpression="CodProducto" />
                                    <asp:BoundField DataField="FecCaducidad" HeaderText="Fecha Caducidad" SortExpression="FecCaducidad" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="Cantidad Deseada" SortExpression="CantidadDeseada">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCantidadDeseada" runat="server" CssClass="textbox_100px" Text='<%# Eval("CantidadDeseada") %>' 
                                                Enabled="false" style="text-align:right"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbCambiarCantidadDesemp" runat="server" Text="Cambiar" CommandName="Cambiar" OnClick="lkbCambiarCantidadDesemp_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbDesempaquetar" runat="server" Text="Desempaquetar" OnClick="lkbDesempaquetar_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoProdDes" />
                            <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana para Desempaquetar Productos -->
    <div id="contenedorVentanaSurtirProductoManual" class="modal">
        <div id="ventanaSurtirProductoManual" class="contenedor_ventana_confirmacion_arriba" style="width:640px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarSurtirProductoManual" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarSurtirProductoManual" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="SurtirProductoManual" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Abastecer Producto</h2>
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="lblCantidadI">C. Requerida</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCantidadI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCantidadI" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoProdDes" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductoInventario" />
                                <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblCantidadS">C. Surtida</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCantidadS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCantidadS" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoProdDes" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductoInventario" />
                                <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblCantidadR">C. Faltante</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCantidadR" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCantidadR" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoProdDes" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                                <asp:AsyncPostBackTrigger ControlID="gvProductoInventario" />
                                <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon3x">
                    <div class="etiqueta_50px">
                        <label for="ddlTamanoProdInv">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoProdInv" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoProdInv" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoProdInv_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblOrdenadoProdInv">Ordenado</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblOrdenadoProdInv" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblOrdenadoProdInv" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvDesempaquetarProducto" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uplnkExportarProdInv" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarProdInv" runat="server" Text="Exportar" OnClick="lnkExportarProdInv_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarProdInv" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvProductoInventario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvProductoInventario" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                                OnPageIndexChanging="gvProductoInventario_PageIndexChanging" OnSorting="gvProductoInventario_Sorting"
                                PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
                                    <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                                    <asp:BoundField DataField="Lote" HeaderText="Lote" SortExpression="Lote" />
                                    <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                                    <asp:BoundField DataField="FechaCaducidad" HeaderText="Fecha de Caducidad" SortExpression="FechaCaducidad" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                                    <asp:TemplateField HeaderText="CantidadDeseada" SortExpression="CantidadDeseada">
                                        <ItemStyle HorizontalAlign="Right" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCantidadDeseada" runat="server" Text='<%# Eval("CantidadDeseada", "{0:0.00}") %>' CssClass="textbox_50px" style="text-align:right" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbCambiar" runat="server" CommandName="Cambiar" Text="Cambiar" OnClick="lkbCambiar_Click" Enabled="true"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoProdDes" />
                            <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                            <asp:AsyncPostBackTrigger ControlID="btnSurtidoManual" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon3x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnSurtidoManual" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSurtidoManual" runat="server" CssClass="boton" Text="Abastecer" OnClick="btnSurtidoManual_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana para Surtir Productos Automatico -->
    <div id="contenedorVentanaCantidadProducto" class="modal">
        <div id="ventanaCantidadProducto" class="contenedor_ventana_confirmacion" style="width:320px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCantidadProducto" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCantidadProducto" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="CantidadProducto" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Ingrese la Cantidad que desea Abastecer</h2>
            </div>
            <div class="columna">
                <div class="renglon">
                    <div class="etiqueta">
                        <label>Cantidad</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCantidadReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCantidadReq" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" style="text-align:right"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvProductosRequeridos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnSurtir" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSurtir" runat="server" Text="Surtir" CssClass="boton" OnClick="btnSurtir_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
