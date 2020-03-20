<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ControlAnticipos.aspx.cs" Inherits="SAT.Liquidacion.ControlAnticipos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
    <link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryControlAnticipos();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraJQueryControlAnticipos() {
            $(document).ready(function () {

                /** CATALOGOS DE AUTOCOMPLETADO **/
                //Proveedor
                $("#<%=txtProveedor.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtProveedor.ClientID%>").val(ui.item.value);
                        //Causando Actualización del Control
                        __doPostBack('<%= txtProveedor.UniqueID %>', '');
                    }
                });
                //Cargando Controles de Fecha
                $("#<%=txtCitaCarga.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtCitaDescarga.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Validando Busqueda
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    var isValid1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                    var isValid2;
                    var isValid3;
                    if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid2 = !$("#<%=txtCitaCarga.ClientID%>").validationEngine('validate');
                        isValid3 = !$("#<%=txtCitaDescarga.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid2 = true;
                        isValid3 = true;
                    }
                    return isValid1 && isValid2 && isValid3;
                });

                //Validando Proveedor al Cargar la Tarifa
                $('.vaAdeudoTotal').click(function () {
                    var isValid1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                    return isValid1;
                });

                //Validando Registro ANTICIPO
                $("#<%=btnRegistrarDep.ClientID%>").click(function () {
                    var isValid1 = !$("#<%=txtReferenciaDep.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtMontoAnt.ClientID%>").validationEngine('validate');
                    return isValid1 && isValid2;
                });
                //Validando Registro FINIQUITO
                $("#<%=btnRegistrarFiniquito.ClientID%>").click(function () {
                    var isValid1 = !$("#<%=txtReferenciaFin.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtMontoFiniquito.ClientID%>").validationEngine('validate');
                    return isValid1 && isValid2;
                });

            });
        }
        ConfiguraJQueryControlAnticipos();

        function CompareDates() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtCitaCarga.ClientID%>").val();
            var txtDate2 = $("#<%=txtCitaDescarga.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
                //Mostrando Mensaje de Operación
                return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
        }
    </script>
    <div id="encabezado_forma">
        <img width="32" height="32" src="../Image/Depositos.png" />
        <h1>Control Anticipos</h1>
    </div>
    <br />
    <br />
    <br />
    <div class="contenedor_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Proveedor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"
                                TabIndex="1" OnTextChanged="txtProveedor_TextChanged"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float: left;">
                <div class="etiqueta_155px">
                    <label for="txtCitaCarga">Cita de Carga</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCitaCarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCitaCarga" runat="server" MaxLength="20" TabIndex="2"
                                CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Fechas" TabIndex="3" Checked="true" Enabled="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float: left;">
                <div class="etiqueta_155px">
                    <label for="txtCitaDescarga">Cita de Descarga</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCitaDescarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCitaDescarga" runat="server" MaxLength="20" TabIndex="4"
                                CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="5" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div class="contenedor_controles">
        <div class="header_seccion">
            <img src="../Image/EnTransito.png" />
            <h2>Viajes por Anticipar</h2>
        </div>
        <div class="renglon4x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="6"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true">
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
                        <asp:Label ID="lblOrdenado" runat="server" CssClass="label_negrita"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarCA" runat="server" Text="Exportar" TabIndex="7" CommandName="ControlAnticipos" 
                            OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarCA" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvViajesAnticipos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvViajesAnticipos" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="8"
                        OnPageIndexChanging="gvViajesAnticipos_PageIndexChanging" OnSorting="gvViajesAnticipos_Sorting"
                        OnRowDataBound="gvViajesAnticipos_RowDataBound" PageSize="250" CssClass="gridview" 
                        ShowFooter="True" Width="90%" AutoGenerateColumns="False">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="IdServicioM" HeaderText="IdServicioM" SortExpression="IdServicioM" Visible="false" />
                            <asp:BoundField DataField="IdTarifaProveedor" HeaderText="IdTarifaProveedor" SortExpression="IdTarifaProveedor" Visible="false" />
                            <asp:BoundField DataField="ServImps" HeaderText="ServImps" SortExpression="ServImps" Visible="false" />
                            <asp:BoundField DataField="ServImpsAnt" HeaderText="ServImpsAnt" SortExpression="ServImpsAnt" Visible="false" />
                            <asp:BoundField DataField="FechaOperacion" HeaderText="FechaOperacion" SortExpression="FechaOperacion" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-Width="120" ItemStyle-Width="120" />
                            <asp:BoundField DataField="Maestro" HeaderText="Maestro" SortExpression="Maestro" ItemStyle-CssClass="label_negrita" />
                            <asp:BoundField DataField="TotalViajes" HeaderText="Viajes Totales" SortExpression="TotalViajes" HeaderStyle-Width="60" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="CC" HeaderText="Con Chofer (CC)" SortExpression="CC" ItemStyle-CssClass="label_correcto" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="SC" HeaderText="Sin Chofer (SC)" SortExpression="SC" ItemStyle-CssClass="label_error" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="Depositos" HeaderText="Depositos" SortExpression="Depositos" Visible="false" />
                            <asp:TemplateField HeaderText="Adeudo Total" SortExpression="AdeudoTotal">
                                <HeaderStyle Width="80" />
                                <ItemStyle HorizontalAlign="Right" Width="80" />
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <label>Sin IVA:</label>
                                    <asp:Label ID="lblAdeudoSinIVA" runat="server" CssClass="label_negrita" 
                                        Text='<%# string.Format("{0:C2}", TSDK.Base.Cadena.RegresaCadenaSeparada(Eval("AdeudoTotal").ToString(),"|", 0, "0")) %>'></asp:Label>
                                    <br />
                                    <label>Total:</label>
                                    <asp:LinkButton ID="lkbAdeudoTotal" runat="server" OnClick="lkbAccionesAnticipos_Click" CommandName="TarifasProveedor"
                                        Text='<%# string.Format("{0:C2}", TSDK.Base.Cadena.RegresaCadenaSeparada(Eval("AdeudoTotal").ToString(),"|", 1, "0")) %>' ToolTip="Mantenga sus Tarifas al día" CssClass="vaAdeudoTotal" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DepsPrevios" HeaderText="Deps. Previos" SortExpression="DepsPrevios" DataFormatString="{0:C2}" ItemStyle-CssClass="label_error" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="AdeudoTotalCC" HeaderText="Adeudo Total (CC)" SortExpression="AdeudoTotalCC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#3AB53A" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="PermitidoCC" HeaderText="Permitido (CC)" SortExpression="PermitidoCC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#3AB53A" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="DepsCC" HeaderText="Depos. (CC)" SortExpression="DepsCC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-CssClass="label_error" HeaderStyle-BackColor="#3AB53A" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:TemplateField HeaderText="Anticipo (CC) Auto." SortExpression="AnticipoAutoCC">
                                <HeaderStyle Width="60" BackColor="#3AB53A" ForeColor="White" />
                                <ItemStyle HorizontalAlign="Right" Width="60" />
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAnticipoAutorizadoCC" runat="server" CssClass="label_negrita"
                                         Text='<%# Eval("AnticipoAutoCC", "{0:C2}") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lkbAnticipoAutorizadoCC" runat="server" Text='<%# Eval("AnticipoAutoCC", "{0:C2}") %>'
                                         OnClick="lkbAccionesAnticipos_Click" CommandName="AnticipoAutorizadoCC" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AdeudoTotalSC" HeaderText="Adeudo Total (SC)" SortExpression="AdeudoTotalSC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#E24848" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="PermitidoSC" HeaderText="Permitido (SC)" SortExpression="PermitidoSC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#E24848" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:BoundField DataField="DepsSC" HeaderText="Depos. (SC)" SortExpression="DepsSC" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-CssClass="label_error" HeaderStyle-BackColor="#E24848" HeaderStyle-ForeColor="White" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:TemplateField HeaderText="Anticipo (SC) Auto." SortExpression="AnticipoAutoSC">
                                <HeaderStyle Width="60" BackColor="#E24848" ForeColor="White" />
                                <ItemStyle HorizontalAlign="Right" Width="60" />
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAnticipoAutorizadoSC" runat="server" CssClass="label_negrita"
                                         Text='<%# Eval("AnticipoAutoSC", "{0:C2}") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lkbAnticipoAutorizadoSC" runat="server" Text='<%# Eval("AnticipoAutoSC", "{0:C2}") %>'
                                         OnClick="lkbAccionesAnticipos_Click" CommandName="AnticipoAutorizadoSC" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LiqsCerradas" HeaderText="Liq. Cerradas" SortExpression="LiqsCerradas" DataFormatString="{0:C2}" ItemStyle-CssClass="label_error" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="60" ItemStyle-Width="60" />
                            <asp:TemplateField HeaderText="Finiquito Auto." SortExpression="FiniquitoAuto">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Right" Width="60" />
                                <FooterStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblFiniquitoAutorizado" runat="server" CssClass="label_negrita"
                                         Text='<%# Eval("FiniquitoAuto", "{0:C2}") %>' Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lkbFiniquitoAutorizado" runat="server" Text='<%# Eval("FiniquitoAuto", "{0:C2}") %>'
                                         OnClick="lkbAccionesAnticipos_Click" CommandName="FiniquitoAutorizado" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Depositos CxP">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbDepositos" runat="server" ImageUrl="~/Image/FacturacionCargos.png" Width="28" Height="28"
                                        OnClick="imbDepositos_Click" ToolTip="Vea sus Anticipos para ligar Facturas" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="txtProveedor" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarTarifa" />
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrarDep" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarDep" />
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrarFiniquito" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarFiniquito" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Alta -->
    <div id="contenedorVentanaTarifaProveedor" class="modal">
        <div id="ventanaTarifaProveedor" class="contenedor_ventana_confirmacion" style="min-width:485px; padding-bottom:15px;">
            <div class="columna2x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplnkCerrar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkCerrarVentanaModal" runat="server" CommandName="TarifaProveedor" OnClick="lnkCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/SignoPesosRechazo.png" />
                    <h2>Tarifa de Proveedor</h2>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblProveedor">Proveedor</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblProveedor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblProveedor" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblServicioMaestro">Servicio Maestro</label>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblServicioMaestro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblServicioMaestro" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_155px">
                        <label for="txtCostoSC">Costo (Sin Chofer)</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCostoSC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCostoSC" runat="server" CssClass="textbox" TabIndex="9" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_155px">
                        <label for="txtCostoCC">Costo (Con Chofer)</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCostoCC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCostoCC" runat="server" CssClass="textbox" TabIndex="10" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardarTarifa" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardarTarifa" runat="server" CssClass="boton" Text="Guardar"
                                    OnClick="btnGuardarTarifa_Click" TabIndex="11" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana Alta de Anticipos -->
    <div id="contenedorVentanaRegistroAnticipo" class="modal">
        <div id="ventanaRegistroAnticipo" class="contenedor_ventana_confirmacion" style="min-width:485px; padding-bottom:10px;">
            <div class="columna2x">
                <div class="header_seccion">
                    <img src="../Image/Depositos.png" />
                    <h2>Anticipa tu Viaje</h2>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblConceptoAnt">Concepto</label>
                    </div>
                    <div class="etiqueta_200px">
                        <asp:UpdatePanel ID="uplblConceptoAnt" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblConceptoAnt" runat="server" Text="ANTICIPO PROVEEDOR" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblFechaViajes">Viajes del día</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblFechaViajes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblFechaViajes" runat="server" Text="dd/MM/yyyy" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblTotalViajes">Total de Viajes</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblTotalViajes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblTotalViajes" runat="server" Text="--" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblMontoPer">Monto Permitido</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblMontoPer" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMontoPer" runat="server" Text="--" CssClass="label_correcto"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblDepPrevios">Deps. Previos</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblDepPrevios" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblDepPrevios" runat="server" Text="--" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtReferenciaDep">Referencia</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtReferenciaDep" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReferenciaDep" runat="server" CssClass="textbox2x validate[required]"
                                    TabIndex="12" placeHolder="Tiene hasta 50 caracteres" MaxLength="50"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label>Anticipo</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtMontoAnt" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMontoAnt" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" TabIndex="12"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnRegistrarDep" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnRegistrarDep" runat="server" CssClass="boton" CommandArgument=""
                                    TabIndex="13" Text="Registrar" CommandName="Registrar" OnClick="btnAccionesAnticipos_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarDep" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarDep" runat="server" CssClass="boton_cancelar"
                                    TabIndex="14" Text="Cerrar" CommandName="Cancelar" OnClick="btnAccionesAnticipos_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- Ventana Alta de Finiquitos -->
    <div id="contenedorVentanaRegistroFiniquito" class="modal">
        <div id="ventanaRegistroFiniquito" class="contenedor_ventana_confirmacion" style="min-width:485px; padding-bottom:10px;">
            <div class="columna2x">
                <div class="header_seccion">
                    <img src="../Image/Depositos.png" />
                    <h2>Finiquita tu Viaje</h2>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblConceptoFiniquito">Concepto</label>
                    </div>
                    <div class="etiqueta_200px">
                        <asp:UpdatePanel ID="uplblConceptoFiniquito" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblConceptoFiniquito" runat="server" Text="FINIQUITO A PROVEEDOR" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblFechaVFin">Viajes del día</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblFechaVFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblFechaVFin" runat="server" Text="dd/MM/yyyy" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblTotalVFin">Total de Viajes</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblTotalVFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblTotalVFin" runat="server" Text="--" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="lblMontoTFin">Monto Total +IVA</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblMontoTFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMontoTFin" runat="server" Text="--" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblMontoPerFin">Monto Permitido</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblMontoPerFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMontoPerFin" runat="server" Text="--" CssClass="label_correcto"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtReferenciaFin">Referencia</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtReferenciaFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReferenciaFin" runat="server" CssClass="textbox2x"
                                    TabIndex="16" placeHolder="Tiene hasta 50 caracteres" MaxLength="50"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtMontoFiniquito">Finiquito</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtMontoFiniquito" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMontoFiniquito" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" TabIndex="17"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnRegistrarFiniquito" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnRegistrarFiniquito" runat="server" CssClass="boton"
                                    TabIndex="13" Text="Registrar" CommandName="RegistrarFiniquito" OnClick="btnAccionesAnticipos_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarFiniquito" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarFiniquito" runat="server" CssClass="boton_cancelar"
                                    TabIndex="14" Text="Cerrar" CommandName="CancelarFiniquito" OnClick="btnAccionesAnticipos_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- Ventana de Gestión de CxP -->
    <div id="contenedorVentanaCxP" class="modal">
        <div id="ventanaCxP" class="contenedor_ventana_confirmacion" style="min-width:485px; padding-bottom:10px;">
            <div class="columna3x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplnkCerrarCxP" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkCerrarCxP" runat="server" CommandName="DepositosCxP" OnClick="lnkCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/XML.png" />
                    <h2>Relacione sus Anticipos y Finiquitos</h2>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvImportacionesAnteriores" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAnticiposCxP" runat="server" TabIndex="13" 
                                CssClass="gridview" AllowPaging="false" AllowSorting="false" 
                                AutoGenerateColumns="false" ShowFooter="true" Width="100%" PageSize="250">
                                <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:BoundField DataField="IdDeposito" HeaderText="IdDeposito" SortExpression="IdDeposito" Visible="false" />
                                    <asp:BoundField DataField="NoAnticipo" HeaderText="NoAnticipo" SortExpression="NoAnticipo" />
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                                    <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                                    <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                                    <asp:BoundField DataField="FacturasCxP" HeaderText="Facturas CxP" SortExpression="FacturasCxP" />
                                    <asp:TemplateField HeaderText="--">
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbFacturaCxP" runat="server" ImageUrl="~/Image/archivo_xml2.png" Width="28" Height="28"
                                                OnClick="imbFacturaCxP_Click" ToolTip="Seleccione la Factura que desee agregar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvViajesAnticipos" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
