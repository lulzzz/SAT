<%@ Page Title="Saldos Globales" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteSaldosGlobales.aspx.cs" Inherits="SAT.CuentasPagar.ReporteSaldosGlobales" %>
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
                ConfiguraJQuerySaldosGlobales();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQuerySaldosGlobales()
        {
            //Inicializando Función
            $(document).ready(function () {

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

                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2;
                });

                //Cargando Catalogo AutoCompleta
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

            });
        }

        //Invocando Función de Configuración
        ConfiguraJQuerySaldosGlobales();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Reporte de Saldos Globales</h1>
    </div>
    <div class="contenedor_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="1" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="2" />
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
                    <label for="txtCliente">Proveedor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
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
    <div class="contenedor_controles">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="6"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSaldosGlobales" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="7" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvFichasIngreso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvSaldosGlobales" runat="server" AllowPaging="true" AllowSorting="true"
                        OnPageIndexChanging="gvSaldosGlobales_PageIndexChanging" OnSorting="gvSaldosGlobales_Sorting"
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
                            <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
                            <asp:BoundField DataField="TotalFacturado" HeaderText="Total Facturado" SortExpression="TotalFacturado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalPagosAplicados" HeaderText="Total Pagos Aplicados" SortExpression="TotalPagosAplicados" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
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
</asp:Content>