<%@ Page Title="Saldos Detalle" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteSaldosDetalle.aspx.cs" Inherits="SAT.CuentasCobrar.ReporteSaldosDetalle" %>

<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagName="wucFacturadoConcepto" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
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
                //Añadiendo Encabezado Fijo
                $("#<%=gvSaldosDetalle.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorSaldosDetalle").offsetWidth - 15,
                    height: 400,
                    freezesize: 2
                });
                $("#<%=gvFichasFacturas.ClientID%>").gridviewScroll({
                    width: 1000,
                    height: 400
                });
                //Listado de Estatus de Cobro
                $("#<%=lbxEstatusCobro.ClientID%>").multiselect({
                    selectedList: 2,
                    selectall: 1
                });
                //Cargando Catalogo de Autocompletado
                $("#<%=txtCliente.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
                });
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
                    var isValid4 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2 && isValid3 && isValid4;
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
        <asp:Panel runat="server" ID="pnlReporteSaldosDetalle" DefaultButton="btnBuscar">
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCliente">Cliente</label>
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
                        <label for="txtFecIni">Fecha Inicio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="2" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Fechas" TabIndex="2" />
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
                                <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="3" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtReferencia">Referencia</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtNoServicio">No Servicio</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox_50px" MaxLength="30" TabIndex="4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFolio">Folio</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_50px validate[custom[onlyNumberSp]]" TabIndex="5" MaxLength="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlEstatusCobro">Estatus Cobro</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uplbxEstatusCobro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ListBox runat="server" ID="lbxEstatusCobro" SelectionMode="multiple" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label>Tipo Servicio</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <label class="label_negrita">Filtros de Proceso de Revisión</label>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <asp:CheckBox ID="chkSinProceso" runat="server" Text="Sin Revisión" />
                    </div>
                    <div class="etiqueta">
                        <asp:CheckBox ID="chkProcesoActual" runat="server" Text="En Revisión" />
                    </div>
                    <div class="etiqueta_155px">
                        <asp:CheckBox ID="chkProcesoTerminado" runat="server" Text="Revisión Terminada" />
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label class="label_negrita">Filtros de Facturación</label>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <asp:CheckBox ID="chkIndicadorServicio" runat="server" Text="Sólo Factura de Servicio" />
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label class="label_negrita">Facturación Electronica</label>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uprbTodos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:RadioButton ID="rbTodos" runat="server" Checked="true" Text="Todos" GroupName="General" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rbSi" />
                                <asp:AsyncPostBackTrigger ControlID="rbNo" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uprbSi" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:RadioButton ID="rbSi" runat="server" Checked="false" Text="Si" GroupName="General" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rbTodos" />
                                <asp:AsyncPostBackTrigger ControlID="rbNo" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uprbNo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:RadioButton ID="rbNo" runat="server" Checked="false" Text="No" GroupName="General" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rbTodos" />
                                <asp:AsyncPostBackTrigger ControlID="rbSi" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="6" CssClass="boton" OnClick="btnBuscar_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="contenedor_controles">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="7"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="control">
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
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="8" CommandName="SaldoDetalle" OnClick="lkbExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="contenedorSaldosDetalle" class="grid_seccion_completa_altura_variable">
            <asp:UpdatePanel ID="upgvSaldosDetalle" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvSaldosDetalle" runat="server" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvSaldosDetalle_RowDataBound" OnPageIndexChanging="gvSaldosDetalle_PageIndexChanging"
                        OnSorting="gvSaldosDetalle_Sorting" PageSize="250" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="NoFactura" HeaderText="No. Factura" SortExpression="NoFactura" Visible="false" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" ItemStyle-Width="150px" />
                            <asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
                                <ItemStyle Width="50px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkServicio" runat="server" Text='<%# Eval("Servicio") %>' OnClick="lnkServicio_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                            <asp:TemplateField HeaderText="Devoluciones" SortExpression="Devoluciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerDevoluciones" runat="server" Text='<%# Eval("Devoluciones") %>' OnClick="lkbVerDevoluciones_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FacOtros" HeaderText="Fac. Otros" SortExpression="FacOtros" />
                            <asp:BoundField DataField="Referencia1" HeaderText="Referencia 1" SortExpression="Referencia1" />
                            <asp:BoundField DataField="Referencia2" HeaderText="Referencia 2" SortExpression="Referencia2" />
                            <asp:BoundField DataField="Referencia3" HeaderText="Referencia 3" SortExpression="Referencia3" />
                            <asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" />
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="FecIniServicio" HeaderText="Fec. Inicio Servicio" SortExpression="FecIniServicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FecFinServicio" HeaderText="Fec. Fin Servicio" SortExpression="FecFinServicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
                            <asp:BoundField DataField="Paquete" HeaderText="Paquete" SortExpression="Paquete" />
                            <asp:BoundField DataField="FechaTermino" HeaderText="Fecha Termino P." SortExpression="FechaTermino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="EstatusPro" HeaderText="Estatus Proceso" SortExpression="EstatusPro" />
                            <asp:BoundField DataField="FacGlobal" HeaderText="Fac. Global" SortExpression="FacGlobal" />
                            <asp:BoundField DataField="EstatusFG" HeaderText="Estatus Fac. Global" SortExpression="EstatusFG" />
                            <asp:BoundField DataField="VersionCFDI" HeaderText="Versión CFDI" SortExpression="VersionCFDI" ItemStyle-Font-Bold="true" />
                            <asp:BoundField DataField="FacturaE" HeaderText="Factura Electronica" SortExpression="FacturaE" />
                            <asp:BoundField DataField="EstatusFE" HeaderText="Estatus Fac. Electronica" SortExpression="EstatusFE" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega" SortExpression="FechaEntrega" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaPago" HeaderText="Fecha Pago" SortExpression="FechaPago" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Recibe" HeaderText="Recibe" SortExpression="Recibe" />
                            <asp:TemplateField HeaderText="Sub Total" SortExpression="SubTotal">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerConceptos" runat="server" OnClick="lkbVerConceptos_Click" Text='<%# string.Format("{0:C2}", Eval("SubTotal")) %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
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
                                    <asp:LinkButton ID="lkbFichasAplicadas" runat="server" Text='<%# string.Format("{0:C2}", Eval("MontoAplicado")) %>' OnClick="lkbFichasAplicadas_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Ventana de Edición de Conceptos (Detalles de Factura) -->
    <div id="contenedorVentanaEdicionConceptos" class="modal">
        <div id="ventanaEdicionConceptos" class="contenedor_modal_seccion_completa_arriba" style="width: 1200px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarVentanaEC" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarVentanaEC" runat="server" Text="Cerrar" CommandName="EdicionConceptos" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/FacturacionCargos.png" />
                <h2>Detalles de la Factura</h2>
            </div>
            <asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <tectos:wucFacturadoConcepto ID="ucFacturadoConcepto" runat="server" Enabled="false" TabIndex="9" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Fichas Aplicadas -->
    <div id="contenidoVentanaFichasFacturas" class="modal">
        <div id="ventanaFichasFacturas" class="contenedor_modal_seccion_completa_arriba">
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
                            <asp:Label ID="lblVentanaFacturasFichas" runat="server" Text="Fichas Aplicadas"></asp:Label></h2>
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
                            <asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="11" CommandName="FichasFacturas" OnClick="lkbExportar_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarFF" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_altura_variable">
                <asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%"
                            OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting"
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
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="FacturaFicha" HeaderText="No. Ficha" SortExpression="FacturaFicha" />
                                <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" SortExpression="FechaIngreso" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                                <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AplicadoPor" HeaderText="Aplicado Por" SortExpression="AplicadoPor" />
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

    <!-- Ventana Resumen Devoluciones -->
    <div id="contenedorVentanaResumenDevoluciones" class="modal">
        <div id="ventanaResumenDevoluciones" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarDev" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarDev" runat="server" CommandName="Devoluciones" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="12">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/EnvioRecepcion.png" />
                <h2>Devoluciones por Viaje</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoDev">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoDev" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoDev" runat="server" TabIndex="13" OnSelectedIndexChanged="ddlTamanoDev_SelectedIndexChanged"
                                CssClass="dropdown_100px" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoDev">Ordenado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uplblOrdenadoDev" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoDev" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvDevoluciones" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplnkExportarDevolucion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarDevolucion" runat="server" Text="Exportar" CommandName="Devolucion" OnClick="lkbExportar_Click"
                                TabIndex="14"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarDevolucion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvDevoluciones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvDevoluciones" runat="server" AllowPaging="true" AllowSorting="true"
                            CssClass="gridview" ShowFooter="true" TabIndex="15" OnSorting="gvDevoluciones_Sorting" Width="100%"
                            OnPageIndexChanging="gvDevoluciones_PageIndexChanging" AutoGenerateColumns="false">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="NoDevolucion" HeaderText="No. Devolución" SortExpression="NoDevolucion" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                <asp:BoundField DataField="FechaDevolucion" HeaderText="Fecha Devolucion" SortExpression="FechaDevolucion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" />
                                <asp:BoundField DataField="UbicacionDevolucion" HeaderText="Ubicacion Devolución" SortExpression="UbicacionDevolucion" />
                                <asp:BoundField DataField="CodProd" HeaderText="Código Producto" SortExpression="CodProd" />
                                <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                                <asp:BoundField DataField="RazonDetalle" HeaderText="Razon Detalle" SortExpression="RazonDetalle" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoDev" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Ventana Resumen Devoluciones -->
    <div id="contenedorVentanaInformacionViaje" class="modal">
        <div id="ventanaInformacionViaje" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarServicio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarServicio" runat="server" CommandName="Servicio" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Servicio.png" />
                <h2>Información del Viaje</h2>
            </div>
            <div class="renglon3x">
                <div class="etiqueta">
                    <label for="ddlTamanoServicio">Mostrar</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamanoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoServicio" runat="server" TabIndex="17" OnSelectedIndexChanged="ddlTamanoServicio_SelectedIndexChanged"
                                CssClass="dropdown" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoServicio">Ordenado</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblOrdenadoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoServicio" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvInfoViaje" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50pxr">
                    <asp:UpdatePanel ID="uplkbExportarServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbExportarServicio" runat="server" Text="Exportar" CommandName="Devolucion" OnClick="lkbExportar_Click"
                                TabIndex="18"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbExportarServicio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvInfoViaje" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvInfoViaje" runat="server" AllowPaging="true" AllowSorting="true"
                            CssClass="gridview" ShowFooter="true" TabIndex="18" OnSorting="gvInfoViaje_Sorting" Width="100%"
                            OnPageIndexChanging="gvInfoViaje_PageIndexChanging" AutoGenerateColumns="false">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="IDServicio" HeaderText="IdServicio" SortExpression="IDServicio" Visible="false" />
                                <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                                <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" />
                                <asp:BoundField DataField="EstadiasDepositadas" HeaderText="Estadias Depositadas" SortExpression="EstadiasDepositadas" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="EstadiasCobradas" HeaderText="Estadias Cobradas" SortExpression="EstadiasCobradas" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="PensionCobrada" HeaderText="Pension Cobrada" SortExpression="PensionCobrada" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="CasetasDepositadas" HeaderText="Casetas Depositadas" SortExpression="CasetasDepositadas" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="CasetasCobradas" HeaderText="Casetas Cobradas" SortExpression="CasetasCobradas" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ManiobrasDepositadas" HeaderText="Maniobras Depositadas" SortExpression="ManiobrasDepositadas" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="ManiobrasCobradas" HeaderText="Maniobras Cobradas" SortExpression="ManiobrasCobradas" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosDetalle" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoDev" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
