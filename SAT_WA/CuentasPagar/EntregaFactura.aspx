<%@ Page Title="Entrega Factura" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="EntregaFactura.aspx.cs" Inherits="SAT.CuentasPagar.EntregaFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
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
                ConfiguraEntregaFactura();
            }
        }
        //Función de Configuración de Controles
        function ConfiguraEntregaFactura()
        {   $(document).ready(function () {
                
            var validacionEntregaFactura = function () {
                //Declarando Objetos de Validación
                var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtFechaRecepcion.ClientID%>").validationEngine('validate');
                //Devolviendo resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4;
            }
            //Añadiendo validación al evento del Control
            $("#<%=btnBuscar.ClientID%>").click(validacionEntregaFactura);
            //Cargando Control de Autocompletado
            $("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
            //Cargando Control de Fecha
            $("#<%=txtFechaRecepcion.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y'
            });

            });
        }
        //Invocando Función de Configuración
        ConfiguraEntregaFactura();

    </script>
    <div id="encabezado_forma">
        <h1>Entrega de Facturas</h1>
    </div>
    <div class="contenedor_controles">
        <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
            <div class="columna2x">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtCompania">Compañia</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompania" runat="server" CssClass="textbox validate[custom[IdCatalogo]]"
                             TabIndex="1" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtProveedor">Proveedor</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox validate[custom[IdCatalogo]]"
                             TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtSerie">Serie</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtFolio">Folio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox validate[custom[positiveNumber]]" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtUUID">UUID</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtFechaRecepcion">Fecha Recepción</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaRecepcion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaRecepcion" runat="server" CssClass="textbox validate[custom[date]]" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon">
                <div class="control">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            <asp:AsyncPostBackTrigger ControlID="btnRecibir" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" TabIndex="7"
                                OnClick="btnBuscar_Click" />
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
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="8"
AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
</asp:DropDownList>
    </ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterio">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblCriterio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterio" runat="server" Text=""></asp:Label></ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvReporteFacturas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExcel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:LinkButton ID="lkbExcel" runat="server" OnClick="lkbExcel_Click" TabIndex="9">Exportar Excel</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbExcel" />
    </Triggers>
</asp:UpdatePanel>
</div>
</div>
        <div>
            <asp:UpdatePanel ID="upgvReporteFacturas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvReporteFacturas" runat="server" AllowPaging="true" AllowSorting="true"
                        OnSorting="gvReporteFacturas_Sorting" OnPageIndexChanging="gvReporteFacturas_PageIndexChanging"
                        PageSize="25" AutoGenerateColumns="false" CssClass="gridview" TabIndex="10">
                        <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField>
                            <HeaderTemplate>
                            <asp:CheckBox ID="chkTodos" runat="server" 
                                    OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <asp:CheckBox ID="chkVarios" runat="server" 
                                    OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true"/>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IdFactura" HeaderText="Factura" SortExpression="IdFactura" Visible="false" />
                            <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
                            <asp:BoundField DataField="Compania" HeaderText="Compania" SortExpression="Compania" />
                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                            <asp:BoundField DataField="FecFac" HeaderText="Fecha Fac." SortExpression="FecFac" />
                            <asp:BoundField DataField="TipoFac" HeaderText="Tipo" SortExpression="TipoFac" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Subtotal" SortExpression="SubTotal" />
                            <asp:BoundField DataField="ImpTras" HeaderText="Importe Trasladado" SortExpression="ImpTras" />
                            <asp:BoundField DataField="ImpRet" HeaderText="Importe Retenido" SortExpression="ImpRet" />
                            <asp:TemplateField>
                            <ItemTemplate>
                            <asp:LinkButton ID="lkbBitacoraFactura" runat="server" OnClick="lkbBitacoraFactura_Click" Text="Bitacora"
                                TabIndex="11"></asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="btnRecibir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="columna">
            <fieldset>
                <legend>Confirmación de Recepción</legend>
                <div class="renglon">
                    <asp:UpdatePanel ID="upbtnRecibir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnRecibir" runat="server" Text="Recibir" TabIndex="11" CssClass="boton"
                                OnClick="btnRecibir_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
