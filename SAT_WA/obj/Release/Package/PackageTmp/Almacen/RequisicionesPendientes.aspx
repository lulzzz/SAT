<%@ Page Title="Requisiciones Pendientes" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="RequisicionesPendientes.aspx.cs" Inherits="SAT.Almacen.RequisicionesPendientes" %>

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
                ConfiguraRequisicionesPendientes();
            }
        }
        //Función de Configuración
        function ConfiguraRequisicionesPendientes() {
            $(document).ready(function () {
                $("#<%=txtAlmacen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
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
                    var isValid3 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtNoRequisicio.ClientID%>").validationEngine('validate');
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
                    return isValid1 && isValid2 && isValid3 && isValid4;
                });


            });
        }

        //Invocando Función de Configuración
        ConfiguraRequisicionesPendientes();

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
        <img src="../Image/FacturacionCargos.png" />
        <h1>Requisiciones Pendientes</h1>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="columna2x">
            <div class=" renglon2x">
                <div class="etiqueta">
                    <label for="txtNoRequisicio">No.Requisición</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel runat="server" ID="uptxtNorequisicion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtNoRequisicio" CssClass="textbox validate[custom[integer]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class=" renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatus">Estatus</label>
                </div>
                <div class="control">
                    <asp:DropDownList runat="server" ID="ddlEstatus" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                </div>
            </div>
            <div class=" renglon2x">
                <div class="etiqueta">
                    <label for="txtAlmacen">Almacén</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtAlmacen" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtAlmacen" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkSolicitud" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkSolicitud" runat="server" TabIndex="4" Text="Solicitud" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkEntrega" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkEntrega" runat="server" TabIndex="5" Text="Entrega" />
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
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="6" MaxLength="16"></asp:TextBox>
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
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="7" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="8" CssClass="boton" OnClick="btnBuscar_Click" />
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
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="9" AutoPostBack="true"
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
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionesPendiente" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="10" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvRequisicionesPendiente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvRequisicionesPendiente" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                        OnPageIndexChanging="gvRequisicionesPendiente_PageIndexChanging" OnSorting="gvRequisicionesPendiente_Sorting"
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
                            <asp:BoundField DataField="NoRequisicion" HeaderText="No. Requisición" SortExpression="NoRequisicion" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Almacen" HeaderText="Almacen" SortExpression="Almacen" />
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                            <asp:BoundField DataField="OrdenCompra" HeaderText="Orden de Compra" SortExpression="OrdenCompra" />
                            <asp:BoundField DataField="CantSolicitado" HeaderText="Cant. Solicitado" SortExpression="CantSolicitado" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega" SortExpression="FechaEntrega" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" SortExpression="Solicitante" />
                            <asp:TemplateField HeaderText="Disponibles" SortExpression="Disponibles">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbDisponibles" runat="server" Text='<%# Eval("Disponibles") %>' OnClick="lkbProductos_Click" CommandName="Disponibles"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Faltantes" HeaderText="Faltantes" SortExpression="Faltantes" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbCancelar" runat="server" Text="Cancelar" OnClick="lkbCancelar_Click"></asp:LinkButton>
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
</asp:Content>
