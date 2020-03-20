<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteComplementoPagos.aspx.cs" Inherits="SAT.FacturacionElectronica33.ReporteComplementoPagos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Liquidacion.css" rel="stylesheet" type="text/css" />
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
    <script>
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryComprobantePagos();
            }
        } 
        //Creando función para configuración de jquery en formulario
        function ConfiguraJQueryComprobantePagos() {
            $(document).ready(function () {
                //Añadiendo funcion de Calendario
                $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Declarando Función de Validación
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Validando Controles
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado Obtenido
                    return isValid1;
                });
                //Añadiendo Función de Autocompletado al Control de Receptor
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=24&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryComprobantePagos();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/AnalisisDoc.png" width="35px" />
        <h1>Reporte Comprobante Pagos</h1>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Buscar comprobantes por</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label class="Label" for="txtCliente">Cliente:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label class="Label" for="txtSerie">Serie:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label class="Label" for="txtFolio">Folio:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label class="Label" for="txtFicha">No. Ficha:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtFicha" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFicha" runat="server" CssClass="textbox_100px" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label class="Label" for="txtUUID">UUID:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="ddlFormaPago">Forma de Pago:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlFormaPago" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="dropdown2x" TabIndex="6"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label_negrita" for="txtFicha">Fecha Expedición</label>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaInicio">Fecha Inicio:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="chkIncluir" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <%--<div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncluir_CheckedChanged" Text="¿Incluir?" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>--%>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaFin">Fecha Fin:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="chkIncluir" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="9" CssClass="boton" />
                        </ContentTemplate>
                        <Triggers>
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>                
            </div>
        </div>
    </div>
    <div class="header_seccion">
        <img src="../Image/TablaResultado.png" />
        <h2>Detalle de Complementos</h2>
    </div>
    <div class="renglon3x">       
        <div class="control2x"></div>
        <div class="etiqueta_50px">
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
    <div class="grid_seccion_completa_altura_variable" id="contenedor_gv_facturas">
        <asp:UpdatePanel ID="upgvComprobantes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvComprobantes" runat="server" AllowPaging="False" AllowSorting="false" TabIndex="11" OnPageIndexChanging="gvComprobantes_PageIndexChanging" PageSize="200" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False" OnRowDataBound="gvComprobantes_RowDataBound">
                    <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                    <FooterStyle CssClass="gridviewfooter" />
                    <HeaderStyle CssClass="gridviewheader" />
                    <SelectedRowStyle CssClass="gridviewrowselected" />
                    <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                    <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    <Columns>                        
                        <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio Pagos" SortExpression="SerieFolio">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" Visible="true">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus"></asp:BoundField>   
                        <asp:BoundField DataField="SustituyeA" HeaderText="Sustituye A" SortExpression="SustituyeA"></asp:BoundField>
                        <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField>                      
                        <asp:BoundField DataField="FI" HeaderText="Ficha Ingreso" SortExpression="FI"></asp:BoundField>
                        <asp:BoundField DataField="Doc" HeaderText="Doc. Relacionado" SortExpression="Doc"></asp:BoundField>
                        <asp:BoundField DataField="UUIDDoc" HeaderText="Docs Rel." SortExpression="UUIDDoc">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha de Pago" SortExpression="FechaIngreso" DataFormatString="{0:dd/MM/yyyy HH:mm}"></asp:BoundField> 
                        <asp:BoundField DataField="FormaPago" HeaderText="Forma Pago" SortExpression="FormaPago"></asp:BoundField> 
                        <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda"></asp:BoundField> 
                        <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Aplicado" DataFormatString="{0:c}" HeaderText="Monto Aplicado" SortExpression="Aplicado">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Saldo" DataFormatString="{0:c}" HeaderText="Saldo" SortExpression="Saldo">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
