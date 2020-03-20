<%@ Page Title="Saldos Detalle" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteSaldosDetalles.aspx.cs" Inherits="SAT.CuentasPagar.ReporteSaldosDetalles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
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
                ConfiguraJQuerySaldosDetalle();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQuerySaldosDetalle() {
            //Inicializando Función
            $(document).ready(function () {

                //Cargando Catalogo de Autocompletado
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

    //Cargando Control DateTimePicker "Fecha Inicio"
    $("#<%=txtFecIni.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    //Cargando Control DateTimePicker "Fecha Fin"
    $("#<%=txtFecFin.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });

    //Configurando Validación de Controles
    $("#<%=btnBuscar.ClientID%>").click(function () {

        //Añadiendo Controles a la Validación
        var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
    var isValid3 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');

    //Devolviendo Resultado Obtenido
    return isValid1 && isValid2 && isValid3;
});
});
}

//Invocando Función de Configuración
ConfiguraJQuerySaldosDetalle();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Reporte de Saldos a Detalle</h1>
    </div>
    <div class="contenedor_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Proveedor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatus">Estatus de Pago</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="3" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="4" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="5" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSerie">Serie</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px" TabIndex="6" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="txtFolio">Folio</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px" TabIndex="7" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUUID">UUID</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x validate[max[36]]" TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="9" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="10" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="11" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnExportarXML" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnExportarXML" runat="server" CssClass="boton_cancelar" Text="Exportar XML" OnClick="btnExportarXML_Click" TabIndex="12" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportarXML" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvSaldosDetalle" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvSaldosDetalle" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="13"
                        OnPageIndexChanging="gvSaldosDetalle_PageIndexChanging" OnSorting="gvSaldosDetalle_Sorting"
                        PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkTodos" runat="server"
                                        OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVarios" runat="server"
                                        OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NoFactura" HeaderText="No. Factura" SortExpression="NoFactura" Visible="true" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                            <asp:TemplateField HeaderText="UUID" SortExpression="UUID">
                                <ItemTemplate>
                                    <asp:Label ID="lblUUID" runat="server" Text='<%# Eval("UUID") %>' CssClass="label_negrita"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha Recepcion" SortExpression="FechaRecepcion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaEstPago" HeaderText="Fecha Estimada Pago" SortExpression="FechaEstPago" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FechaUltimoPago" HeaderText="Fecha Ultimo Pago" SortExpression="FechaUltimoPago" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Trasladado" HeaderText="Imp. Trasladado" SortExpression="Trasladado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Retenido" HeaderText="Imp. Retenido" SortExpression="Retenido" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAplicaciones" runat="server" Text='<%# string.Format("{0:C2}", Eval("MontoAplicado")) %>' OnClick="lnkAplicaciones_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Descargas">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplnkDescargaXML" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkDescargaXML" runat="server" OnClick="lnkDescargaXML_Click">
                                                <img src="../Image/cfdiXML.png" />
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lnkDescargaXML" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Validación SAT">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbValidacion" runat="server" OnClick="imbValidacion_Click" ImageUrl="~/Image/cfdi_consulta.png" Width="25" Height="25" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Fichas Aplicadas -->
    <div id="contenidoVentanaFichasFacturas" class="modal">
        <div id="ventanaFichasFacturas" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarFF" runat="server" CommandName="FichasFacturas" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <asp:UpdatePanel ID="uplblVentanaFacturasFichas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2>
                            <asp:Label ID="lblVentanaFacturasFichas" runat="server" Text="Aplicaciones y Relaciones de la Factura"></asp:Label></h2>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="ddlTamanoFF">Mostrar</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamanoFF" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoFF" runat="server" CssClass="dropdown" TabIndex="10"
                                OnSelectedIndexChanged="ddlTamanoFF_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoFF">Ordenado</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenadoFF" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoFF" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplnkExportarFF" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="11" CommandName="FichasFacturas" OnClick="lnkExportarFF_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarFF" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_100px_altura">
                <asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
                            OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting"
                            OnRowDataBound="gvFichasFacturas_RowDataBound" CssClass="gridview" AllowSorting="true"
                            AllowPaging="true" ShowFooter="true">
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
                                <asp:BoundField DataField="Egreso" HeaderText="No. Egreso" SortExpression="Egreso" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                <asp:BoundField DataField="IdEntidad" HeaderText="IdEntidad" SortExpression="IdEntidad" Visible="false" />
                                <asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
                                <asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Viajes" SortExpression="Viajes">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkServiciosEntidad" runat="server" OnClick="lnkServiciosEntidad_Click" Text='<%# Eval("Viajes") %>'></asp:LinkButton>
                                        <asp:Label ID="lblServiciosEntidad" runat="server" Text='<%# Eval("Viajes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Ventana de Fichas Aplicadas -->
    <div id="contenidoVentanaServiciosEntidad" class="modal">
        <div id="ventanaServiciosEntidad" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarSE" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarSE" runat="server" CommandName="ServiciosEntidad" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Servicios de la Liquidación</h2>
            </div>
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="ddlTamanoSE">Mostrar</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamanoSE" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoSE" runat="server" CssClass="dropdown" TabIndex="10"
                                OnSelectedIndexChanged="ddlTamanoSE_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoSE">Ordenado</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenadoSE" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoSE" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServiciosEntidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplnkExportarSE" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarSE" runat="server" Text="Exportar" TabIndex="11" OnClick="lnkExportarSE_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarSE" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_100px_altura">
                <asp:UpdatePanel ID="upgvServiciosEntidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvServiciosEntidad" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
                            OnPageIndexChanging="gvServiciosEntidad_PageIndexChanging" OnSorting="gvServiciosEntidad_Sorting"
                            CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                                <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
                                <asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoSE" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- Ventana Confirmación Resultado Consulta SAT -->
    <div id="contenidoResultadoConsultaSATModal" class="modal">
        <div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba">
            <div class="columna2x">
                <div class="header_seccion">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
                            <h3 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h3>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="">Emisor</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="">Receptor</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblRFCReceptor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblRFCReceptor" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="">UUID</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblUUIDM" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblUUIDM" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="">Fecha Expedición</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="">Total Factura</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblTotalFactura" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblTotalFactura" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon_boton">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnValidacionSAT" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnValidacionSAT" runat="server" Text="Continuar" CssClass="boton" 
                                    OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
