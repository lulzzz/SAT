<%@ Page Title="OrdenesCompraPendientes" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrdenesCompraPendientes.aspx.cs" Inherits="SAT.Almacen.OrdenesCompraPendientes" %>

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
                ConfiguraOrdenesCompraPendientes();
            }
        }
        //Función de Configuración
        function ConfiguraOrdenesCompraPendientes() {
            $(document).ready(function () {
                //Carga el autocomplet de proveedor
                $("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
                //Cargando Controles de Fecha
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Añadiendo Validación al Evento Click del Boton
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    var isValid1;
                    var isValid2;
                    var isValid3 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');

                    //Validando el Control
                    if ($("#<%=chkEntrega.ClientID%>").is(':checked') == true || $("#<%=chkSolicitud.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                        isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid1 = true;
                        isValid2 = true;
                    }

                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3;
                });


            });
        }

        //Invocando Función de Configuración
        ConfiguraOrdenesCompraPendientes();

        //Declarando Función de Validación de Fechas
        function CompareDates() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
            var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

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
        <img src="../Image/compras.png" />
        <h1>Ordenes de Compra Pendientes</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtProveedor">No. Orden C. </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoOrden" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoorden" runat="server" CssClass="textbox" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtProveedor">Proveedor </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkSolicitud" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkSolicitud" runat="server" TabIndex="2" Text="Solicitud" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkEntrega" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkEntrega" runat="server" TabIndex="3" Text="Entrega" />
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
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4" MaxLength="16"></asp:TextBox>
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
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="6" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="7" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <h2>Resultados Obtenidos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="14" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
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
                        <asp:AsyncPostBackTrigger ControlID="gvOrdenesCompraPendientes" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="15" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvOrdenesCompraPendientes" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvOrdenesCompraPendientes" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                        OnPageIndexChanging="gvOrdenesCompraPendientes_PageIndexChanging" OnSorting="gvOrdenesCompraPendientes_Sorting"
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
                            <asp:TemplateField HeaderText="NoCompra" SortExpression="NoCompra">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCompra" runat="server" Text='<%# Eval("NoCompra") %>' OnClick="lnkCompra_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega" SortExpression="FechaEntrega" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Productos" HeaderText="Productos" SortExpression="Productos" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="TotalOrden" HeaderText="Total" SortExpression="TotalOrden" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" FooterStyle-HorizontalAlign="Right"/>
                            <asp:TemplateField HeaderText="Contiene Factura" SortExpression="Factura"  ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                                <ItemTemplate>                                    
                                    <asp:LinkButton ID="lkbFactura" runat="server" Text='<%#string.Format("{0:C2}" , Eval("Factura")) %>' OnClick="lkbFactura_Click"  DataFormatString="{0:C2}">                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Pago" HeaderText="Pago Aplicado" SortExpression="Pago" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"  DataFormatString="{0:C2}"/>
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
