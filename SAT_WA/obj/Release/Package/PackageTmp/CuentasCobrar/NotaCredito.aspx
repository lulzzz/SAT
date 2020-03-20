<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="NotaCredito.aspx.cs" Inherits="SAT.CuentasCobrar.NotaCredito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryNotaCredito();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryNotaCredito() {
            $(document).ready(function () {

                //Obteniendo Función de Validación
                var validaBusqueda = function () {
                    //Validando Controles
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    var isValid4 = true;
                    var isValid5 = true;

                    //Validando si las fechas son Requeridas
                    if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
                        isValid4 = !$("#<%=txtFechaIni.ClientID%>").validationEngine('validate');
                        isValid5 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                    }

                    //Devolviendo Resultado de Validación
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                }

                //Valida Criterios de Busqueda
                $("#<%=btnBuscar.ClientID%>").click(validaBusqueda);
                $("#<%=btnCfdiDisponibles.ClientID%>").click(validaBusqueda);
                $("#<%=btnNotasCredito.ClientID%>").click(validaBusqueda);
                //Añadiendo funcion de Calendario
                $("#<%=txtFechaIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaTC.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y',
                    closeOnDateSelect: true,
                    onSelectDate: function (selected, evtn) {
                        //Asignando Fecha
                        $("#<%=txtFechaTC%>").val(selected.format('dd/MM/yyyy'));
                        //Causando Actualización del Control
                        __doPostBack('<%= txtFechaTC.UniqueID %>', '');
                    }
                });

                //Cargando Clientes
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

                //Validando Nota de Credito
                $("#<%=btnAceptar.ClientID%>").click(function () {
                    //Validando Controles de Creación de Nota de Credito
                    var isValid1 = !$("#<%=txtTipoCambioFE.ClientID%>").validationEngine('validate');
                    var isValid1 = !$("#<%=txtFechaTC.ClientID%>").validationEngine('validate');
                    var isValid1 = !$("#<%=txtMonto.ClientID%>").validationEngine('validate');
                });
            });
        }

        //Invocando Función de Configuración
        ConfiguraJQueryNotaCredito();
    </script>
    <div id="encabezado_forma">
        <h1>Notas de Credito</h1>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Busqueda de Comprobantes</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
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
                            <asp:TextBox ID="txtSerie" runat="server" MaxLength="25" CssClass="textbox_100px validate[max[9999]]" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="txtFolio">Folio</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" MaxLength="40" CssClass="textbox_100px validate[integer]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaIni">Desde</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:CheckBox ID="chkIncluir" runat="server" TabIndex="5" Text="¿Incluir?" />
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaFin">Hasta</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton"
                                TabIndex="7" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div class="contenedor_botones_pestaña">
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnCfdiDisponibles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnCfdiDisponibles" runat="server" TabIndex="8" OnClick="btnVisorPestana_Click"
                        CssClass="boton_pestana_activo" Text="Cfdi Disponibles" CommandName="CfdiPendientes" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnNotasCredito" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnNotasCredito" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnNotasCredito" runat="server" TabIndex="9" OnClick="btnVisorPestana_Click"
                        CssClass="boton_pestana" Text="Notas de Credito" CommandName="NotasCredito" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCfdiDisponibles" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <asp:UpdatePanel ID="upmtvComprobantesNC" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mtvComprobantesNC" runat="server">
                    <asp:View ID="vwCfdiDisponibles" runat="server">
                        <div class="header_seccion">
                            <img src="../Image/FacturacionCargos.png" />
                            <h2>Comprobantes Disponibles</h2>
                        </div>
                        <div class="renglon4x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamano">Mostrar:</label>
                            </div>
                            <div class="control_100px">
                                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" TabIndex="10"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta_80px">
                                <label for="lblOrdenado">Ordenado:</label>
                            </div>
                            <div class="etiqueta_155px">
                                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenado" runat="server" CssClass="label_negrita"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnNotasCredito" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCfdiDisponibles" />
                                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta_50pxr">
                                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="11"
                                            OnClick="lnkExportar_Click" CommandName="CfdiDisponibles"></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="controlBoton">
                                <asp:UpdatePanel ID="upbtnCrearNC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnCrearNC" runat="server" Text="Crear NC." CssClass="boton"
                                            OnClick="btnCrearNC_Click" TabIndex="12" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="controlBoton">
                                <asp:UpdatePanel ID="upbtnNC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="btnNC" runat="server" Text="NC." CssClass="boton_cancelar"
                                            OnClick="btnNC_Click" TabIndex="12" Visible="false" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="grid_seccion_completa_400px_altura">
                            <asp:UpdatePanel ID="upgvComprobantesSinPagar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvComprobantesSinPagar" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                                        AllowPaging="true" CssClass="gridview" OnPageIndexChanging="gvComprobantesSinPagar_PageIndexChanging"
                                        OnSorting="gvComprobantesSinPagar_Sorting" ShowFooter="true" TabIndex="13" PageSize="25" Width="100%">
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
                                                    <asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkVarios_CheckedChanged" AutoPostBack="true" />
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkVarios_CheckedChanged" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" />
                                            <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Descuentos" HeaderText="Descuentos" SortExpression="Descuentos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Total" HeaderText="Monto Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="SaldoAplicado" HeaderText="Saldo Aplicado" SortExpression="SaldoAplicado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="Saldo Pendiente" SortExpression="SaldoPendiente">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSaldoPendiente" runat="server" Text='<%# Eval("SaldoPendiente", "{0:C2}") %>' CssClass="label_error"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saldo Por Aplicar" SortExpression="SaldoPorAplicar">
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMXA" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" Text='<%# Eval("SaldoPorAplicar","{0:0.00}") %>'
                                                        Enabled="false" Style="text-align: right"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lkbCambiarMonto" runat="server" Text="Cambiar" OnClick="lkbCambiarMonto_Click" CommandName="Cambiar" Enabled="false"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnNotasCredito" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCfdiDisponibles" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                    <asp:View ID="vwNotasCredito" runat="server">
                        <div class="header_seccion">
                            <img src="../Image/archivo_xml2.png" />
                            <h2>Notas de Credito Timbradas</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamanoNC">Mostrar:</label>
                            </div>
                            <div class="control_100px">
                                <asp:UpdatePanel ID="upddlTamanoNC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTamanoNC" runat="server" CssClass="dropdown_100px" TabIndex="14"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoNC_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta_80px">
                                <label for="lblOrdenadoNC">Ordenado:</label>
                            </div>
                            <div class="etiqueta_155px">
                                <asp:UpdatePanel ID="uplblOrdenadoNC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenadoNC" runat="server" CssClass="label_negrita"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvNotasCredito" />
                                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta_50pxr">
                                <asp:UpdatePanel ID="uplnkExportarNC" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkExportarNC" runat="server" Text="Exportar" TabIndex="15"
                                            OnClick="lnkExportar_Click" CommandName="NotasCredito"></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="grid_seccion_completa_400px_altura">
                            <asp:UpdatePanel ID="upgvNotasCredito" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvNotasCredito" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                                        AllowPaging="true" CssClass="gridview" OnPageIndexChanging="gvNotasCredito_PageIndexChanging"
                                        OnSorting="gvNotasCredito_Sorting" ShowFooter="true" TabIndex="10" PageSize="25" Width="70%">
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
                                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="NoFicha" HeaderText="No. Ficha" SortExpression="NoFicha" ItemStyle-Width="60px" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                            <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
                                            <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="Descuentos" HeaderText="Descuentos" SortExpression="Descuentos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="Total" HeaderText="Monto Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                            <asp:TemplateField HeaderText="CFDI's Aplicados" SortExpression="CfdiAplicados" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lkbAplicacionesNC" runat="server" Text='<%# Eval("CfdiAplicados", "{0:C2}") %>' OnClick="lkbAplicacionesNC_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descargas">
                                                <HeaderStyle HorizontalAlign="Center" Width="64px" />
                                                <ItemStyle HorizontalAlign="Center" Width="64px" />
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="upimbXML" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div style="width:32px; height:auto; float:left">
                                                                <asp:ImageButton ID="imbXML" runat="server" ToolTip="Descargue su XML" CommandName="XML"
                                                                    ImageUrl="~/Image/cfdi_xml.png" Width="32" Height="32" OnClick="imbDescargas_Click" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="imbXML" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdatePanel ID="upimbPDF" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div style="width:32px; height:auto; float:right">
                                                                <asp:ImageButton ID="imbPDF" runat="server" ToolTip="Descargue su PDF" CommandName="PDF"
                                                                    ImageUrl="~/Image/cfdi_pdf.png" Width="32" Height="32" OnClick="imbDescargas_Click" />
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="imbPDF" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoNC" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnNotasCredito" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCfdiDisponibles" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnNotasCredito" />
                <asp:AsyncPostBackTrigger ControlID="btnCfdiDisponibles" />
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <!-- Ventana encargada de Mostrar los CFDI's amparados por una NC -->
    <div id="contenedorVentanaCfdiRelacionados" class="modal">
        <div id="ventanaCfdiRelacionados" class="contenedor_ventana_confirmacion">
            <div class="columna3x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplnkCerrarCfdiRelacionados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkCerrarCfdiRelacionados" runat="server" Text="Cerrar" CommandName="CfdiRelacionados"
                                OnClick="lnkCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/cfdiXML.png" />
                    <h2>CFDI's Relacionados</h2>
                </div>
                <div class="renglon3x">
                    <div class="etiqueta_50px">
                        <label for="ddlTamanoCfdiRel">Mostrar:</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="upddlTamanoCfdiRel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoCfdiRel" runat="server" CssClass="dropdown_100px" TabIndex="17"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoCfdiRel_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80px">
                        <label for="lblOrdenadoCfdiRel">Ordenado:</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblOrdenadoCfdiRel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblOrdenadoCfdiRel" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCfdiRelacionados" />
                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50pxr">
                        <asp:UpdatePanel ID="uplnkExportarCfdiRelacionados" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarCfdiRelacionados" runat="server" Text="Exportar" TabIndex="18"
                                    OnClick="lnkExportar_Click" CommandName="CfdiRelacionados"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_200px_altura">
                    <asp:UpdatePanel ID="upgvCfdiRelacionados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvCfdiRelacionados" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                                AllowPaging="true" CssClass="gridview" OnPageIndexChanging="gvCfdiRelacionados_PageIndexChanging"
                                OnSorting="gvCfdiRelacionados_Sorting" ShowFooter="true" TabIndex="19" PageSize="25">
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
                                    <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:TemplateField HeaderText="UUID" SortExpression="UUID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUUID" runat="server" Text='<%# Eval("UUID") %>' CssClass="label_negrita"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Version" SortExpression="Version">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("Version") %>' CssClass="label_correcto"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
                                    <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Descuentos" HeaderText="Descuentos" SortExpression="Descuentos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Total" HeaderText="Monto Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="MontoAmparado" HeaderText="Monto Amparado" SortExpression="MontoAmparado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvNotasCredito" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoCfdiRel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana de Registro de Factura Electronica v3.3 -->
    <div id="contenedorVentanaTimbraNotaCredito" class="modal">
        <div id="ventanaTimbraNotaCredito" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarNotaCredito" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarNotaCredito" runat="server" Text="Cerrar" CommandName="NotaCredito"
                            OnClick="lnkCerrarVentanaModal_Click">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Crea tu Nota de Credito</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtSerieFE">Serie</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtSerieFE" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSerieFE" runat="server" CssClass="textbox" MaxLength="40" TabIndex="11"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80px">
                        <asp:UpdatePanel ID="upchkAplicar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkAplicar" runat="server" Text="¿Aplicar?" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlConcepto">Concepto</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlFormaPago">Forma de Pago</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlFormaPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="dropdown2x" TabIndex="12"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlMetodoPago">Método de Pago</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown2x" TabIndex="13"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlUsoCFDI">Uso del CFDI</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlUsoCFDI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlUsoCFDI" runat="server" CssClass="dropdown2x" TabIndex="14"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtTipoCambioFE">Tipo de Cambio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtTipoCambioFE" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTipoCambioFE" runat="server" TabIndex="15"
                                    CssClass="textbox validate[required, custom[positiveNumber6]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaTC">Fecha</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaTC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaTC" runat="server" CssClass="textbox validate[custom[date]]" TabIndex="16"
                                    OnTextChanged="txtFechaTC_TextChanged" MaxLength="10" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlFormaPago">Moneda</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlMonedaNC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlMonedaNC" runat="server" CssClass="dropdown2x" TabIndex="17" Enabled="false"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtMonto">Monto Total</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtMonto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtMonto" runat="server" TabIndex="18" Enabled="false"
                                    CssClass="textbox validate[required, custom[positiveNumber6]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div class="renglon_boton">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar"
                                    OnClick="btnAceptar_Click" TabIndex="18" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar"
                                    OnClick="btnCancelar_Click" TabIndex="19" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCrearNC" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobantesSinPagar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
