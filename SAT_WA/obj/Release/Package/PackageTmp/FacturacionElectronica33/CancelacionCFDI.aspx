<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CancelacionCFDI.aspx.cs" Inherits="SAT.FacturacionElectronica33.CancelacionCFDI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <!-- Estilos de la Forma -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                //Invocando Función de Configuración
                ConfiguraJQueryCfdiCancelados();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryCfdiCancelados() {
            $(document).ready(function () {

                //Validación 
                var validacionReporteComprobante = function () {

                    var isValid1 = !$("#<%=txtReceptor.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
                    return isValid1 && isValid2;
                };
                //Validación de campos requeridos
                $("#<%=this.btnConsultaCFDI.ClientID%>").click(validacionReporteComprobante);

                //Añadiendo Función de Autocompletado al Control
                $("#<%=txtReceptor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
            });
        }

        //Invocando Función de Configuración
        ConfiguraJQueryCfdiCancelados();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Reporte de Comprobantes Cancelados</h1>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Buscaque de comprobantes</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="lblEmisor">Emisor</label>
                </div>
                <div class="etiqueta_400px">
                    <asp:UpdatePanel ID="uplblEmisor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblEmisor" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="txtReceptor">Receptor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="txtSerie">Serie</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px validate[custom[onlyLetterSp]]" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="txtFolio">Folio</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px validate[custom[onlyNumberSp]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnConsultaCFDI" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnConsultaCFDI" runat="server" CssClass="boton" CommandName="ConsultaCFDI"
                                Text="Consulta CFDI" OnClick="btnCFDI_Click" TabIndex="4" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="contenedor_botones_pestaña">
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnFacturasCxC" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnFacturasCxC" runat="server" Text="Facturas CxC" CssClass="boton_pestana_activo" CommandName="CxC" OnClick="btnFacturas_Click" TabIndex="5" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnFacturasCxP" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnFacturasCxP" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnFacturasCxP" runat="server" Text="Facturas CxP" CssClass="boton_pestana" CommandName="CxP" OnClick="btnFacturas_Click" TabIndex="6" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnFacturasCxC" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_tabs">
        <asp:UpdatePanel ID="upmtvCfdiCancelacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mtvCfdiCancelacion" runat="server">
                    <asp:View ID="vwCxC" runat="server">
                        <div class="header_seccion">
                            <img width="32" height="32" src="../Image/cancelar-cfdi.png" />
                            <h2>Cancele y Actualice sus Comprobantes</h2>
                        </div>
                        <div class="renglon4x">
                            <div class="etiqueta">
                                <label for="ddlTamanoCFDI">Mostrar</label>
                            </div>
                            <div class="control">
                                <asp:UpdatePanel ID="upddlTamanoCFDI" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTamanoCFDI" runat="server" AutoPostBack="true" CssClass="dropdown"
                                            OnSelectedIndexChanged="ddlTamanoCFDI_SelectedIndexChanged" TabIndex="7">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <label for="lblOrdenadoCFDI">Ordenado Por:</label>
                            </div>
                            <div class="etiqueta_155px">
                                <asp:UpdatePanel ID="uplblOrdenadoCFDI" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenadoCFDI" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvCfdiPendientes" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel runat="server" ID="uplkbExportarCFDI" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lkbExportarCFDI" runat="server" OnClick="lkbExportar_Click"
                                            CommandName="CfdiPendientes" Text="Exportar" TabIndex="8"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lkbExportarCFDI" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="controlBoton">
                                <asp:UpdatePanel ID="upbtnActualizarCFDI" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnActualizarCFDI" runat="server" Text="Actualizar CFDI's" TabIndex="9"
                                            OnClick="btnActualizarCFDI_Click" CssClass="boton" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="grid_seccion_completa">
                            <asp:UpdatePanel ID="upgvCfdiPendientes" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvCfdiPendientes" runat="server" CssClass="gridview" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                        OnPageIndexChanging="gvCfdiPendientes_PageIndexChanging"
                                        OnSorting="gvCfdiPendientes_Sorting"
                                        OnRowDataBound="gvCfdiPendientes_RowDataBound"
                                        ShowFooter="True" TabIndex="9" Width="100%">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkTodos" runat="server"
                                                        OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkVarios" runat="server"
                                                        OnCheckedChanged="chkTodos_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie Folio" SortExpression="SerieFolio" />
                                            <asp:BoundField DataField="OrigenCFDI" HeaderText="Origen CFDI" SortExpression="OrigenCFDI" />
                                            <asp:TemplateField SortExpression="UUID" HeaderText="UUID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUUID" runat="server" Text='<%# Eval("UUID") %>' CssClass="label_negrita"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                            <asp:BoundField DataField="Receptor" HeaderText="Cliente" SortExpression="Receptor" />
                                            <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha de Expedición" SortExpression="FechaExpedicion" DataFormatString="{0:yyy-MM-dd hh:mm tt}" />
                                            <asp:BoundField DataField="TimbradoPor" HeaderText="TimbradoPor" SortExpression="TimbradoPor" />
                                            <asp:BoundField DataField="MotivoCancelacion" HeaderText="Motivo" SortExpression="MotivoCancelacion">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" />
                                            <asp:BoundField DataField="TipoCambio" HeaderText="Tipo de Cambio" SortExpression="TipoCambio" DataFormatString="{0:c}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" DataFormatString="{0:c}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:c}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descuentos" HeaderText="Descuentos" SortExpression="Descuentos" DataFormatString="{0:c}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Accion Acuse" SortExpression="AccionAcuse">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbAccionCancelacion" runat="server" ToolTip='<%# Eval("AccionAcuse") %>' CommandName='<%# Eval("AccionAcuse") %>'
                                                        OnClick="imbAccionCancelacion_Click" ImageUrl="~/Image/cfdi_consulta.png" Width="25" Height="25" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnConsultaCFDI" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoCFDI" />
                                    <asp:AsyncPostBackTrigger ControlID="btnActualizarCFDI" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                    <asp:View ID="vwCxP" runat="server">
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnConsultaCFDI" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- Ventana de Consulta ante el SAT -->
    <div id="contenedorVentanaConsultaCFDI" class="modal">
        <div id="ventanaConsultaCFDI" class="contenedor_ventana_confirmacion">
            <div class="columna2x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplkbCerrarVentanaModal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarConsultaCFDI" runat="server" CommandName="ConsultaCFDI" OnClick="lkbCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/Pin_Azul.png" />
                    <asp:UpdatePanel ID="uplblEncabezado" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h2>
                                <asp:Label ID="lblEncabezado" runat="server" Text="Consulta del Comprobante 'X'"></asp:Label></h2>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
