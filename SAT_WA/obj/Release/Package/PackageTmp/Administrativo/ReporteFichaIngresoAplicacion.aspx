<%@ Page Title="Ficha de Ingreso Aplicación" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteFichaIngresoAplicacion.aspx.cs" Inherits="SAT.Administrativo.ReporteFichaIngresoAplicacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteFichasAplicacion();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryReporteFichasAplicacion() {
            $(document).ready(function () {

                //Cargando Controles de Fecha
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Declarando Función de Validación
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Validando Controles
                    var isValid1 = !$("#<%=txtCompaniaEmi.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2 && isValid3;
                });

            });
        }

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

        //Invocando Función de Configuración
        ConfiguraJQueryReporteFichasAplicacion();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Evidencia.png" />
        <h1>Visor de Aplicación de Fichas de Ingreso</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmi">Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompaniaEmi" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompaniaEmi" runat="server"  CssClass="textbox2x validate[custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
             <div class="renglon2x">
                <div class="etiqueta">
                    <label for="uptxtNoFicha">No. Ficha</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoFicha" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoFicha" runat="server" CssClass="textbox" MaxLength="13" TabIndex="1"></asp:TextBox>
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
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="2"></asp:TextBox>
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
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" TabIndex="4" />
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
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="5" AutoPostBack="true"
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
                        <asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="6" OnClick="lnkExportar_Click"></asp:LinkButton>
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
                    <asp:GridView ID="gvFichasIngreso" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="7"
                        OnPageIndexChanging="gvFichasIngreso_PageIndexChanging" OnSorting="gvFichasIngreso_Sorting"
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
                            <asp:BoundField DataField="IdFichaIngreso" HeaderText="IdFichaIngreso" SortExpression="IdFichaIngreso" Visible="false" />
                            <asp:BoundField DataField="NoFichaIngreso" HeaderText="No. Ficha" SortExpression="NoFichaIngreso" />
                             <asp:BoundField DataField="FechaFI" HeaderText="Fecha Ingreso" SortExpression="FechaFI" DataFormatString="{0:dd/MM/yyyy}" />
                             <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                            <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                            <asp:BoundField DataField="FechaFactura" HeaderText="Fecha Facturacion" SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="EstatusFac" HeaderText="Estatus Factura" SortExpression="EstatusFac" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                            <asp:BoundField DataField="Flete" HeaderText="Flete" SortExpression="Flete" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Retencion" HeaderText="Retención" SortExpression="Retencion" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="TotalFI" HeaderText="Total FI" SortExpression="TotalFI" DataFormatString="{0:C2}" />
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
