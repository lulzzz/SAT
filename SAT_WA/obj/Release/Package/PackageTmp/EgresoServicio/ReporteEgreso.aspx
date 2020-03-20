<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteEgreso.aspx.cs" Inherits="SAT.EgresoServicio.ReporteEgreso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
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
                ConfiguraJQueryReporteEgreso();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryReporteEgreso() {
            $(document).ready(function () {

                //Validación 
                var validacionReporteEgreso = function () {

                var isValidP1 = !$("#<%=txtNoEgreso.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                var isValidP3 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                    return isValidP1 && isValidP2 && isValidP3;
            };
                //Validación de campos requeridos
                $("#<%=this.btnBuscar.ClientID%>").click(validacionReporteEgreso);

                // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
                $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y',
                    timepicker:false
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y',
                    timepicker:false
                });

            });
        }

        //Invocación Inicial de método de configuración JQuery
      ConfiguraJQueryReporteEgreso();
    </script>
    <div id="encabezado_forma">
        <h1>Reporte Egresos</h1>
    </div>
    <div class="seccion_controles">
       <div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar Egreso </h2>
</div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoEgreso">
                        No Egreso
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoEgreso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoEgreso" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="1" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
             <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlConcepto">Concepto</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
                        <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtBeneficiario">
                        Beneficiario
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtBeneficiario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtBeneficiario" runat="server" CssClass="textbox" TabIndex="3" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
               <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlCuentaOrigen">Cuenta Origen </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlCuentaOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCuentaOrigen" AutoPostBack="true"  runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatus">Estatus</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="upchkRangoFechas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar por fechas de Egreso."
                                Checked="false" TabIndex="6" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaInicio">Fecha Inicial</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaFin">Fecha Final</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox validate[required, custom[date]]" TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="9" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenido_resumen_visor">
        <div class="header_seccion">
            <img src="../Image/ResumenReporte.png" />
            <h2>Resumen por concepto</h2>
        </div>
        <div class="grafica_resumen_visor">
            <asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Chart ID="ChtEgreso" runat="server" TabIndex="10" BackColor="Transparent">
                        <Legends>
                            <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                            </asp:Legend>
                        </Legends>
                    </asp:Chart>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="grid_resumen_visor">
            <asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvResumen" runat="server" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="False"
                        TabIndex="10" ShowFooter="True" CssClass="gridview"
                        PageSize="10" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:TemplateField HeaderText="Total" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lbldetalles" Text='<%# Eval("Total", "{0:c}") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblSuma" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
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
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Documento.png" />
            <h2>Egresos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamañoGridViewEgreso">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewEgreso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewEgreso" runat="server" OnSelectedIndexChanged="gvEgreso_OnSelectedIndexChanged" TabIndex="10" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblCriterioGridViewEgreso">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterioGridViewEgreso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewEgreso" TabIndex="11" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvEgreso" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarExcelEgreso" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelEgreso" runat="server" Text="Exportar" TabIndex="12" OnClick="lkbExportarExcelEgreso_Onclick"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelEgreso" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa">
            <asp:UpdatePanel ID="upgvEgreso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvEgreso" CssClass="gridview" OnPageIndexChanging="gvEgreso_OnpageIndexChanging" OnSorting="gvEgreso_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True" TabIndex="13"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="NoEgreso" HeaderText="No Egreso" SortExpression="NoEgreso" />
                            <asp:BoundField DataField="Beneficiario" HeaderText="Beneficiario" SortExpression="Beneficiario" />
                            <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="CuentaOrigen" HeaderText="Cuenta Origen" SortExpression="CuentaOrigen" />
                            <asp:BoundField DataField="CuentaDestino" HeaderText="Cuenta Destino" SortExpression="CuentaDestino" />
                            <asp:BoundField DataField="NoCheque" HeaderText="No Cheque" SortExpression="NoCheque" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c2}" />
                            <asp:BoundField DataField="MontoNacional" HeaderText="Monto Nacional" SortExpression="MontoNacional" DataFormatString="{0:c2}" />
                            <asp:BoundField DataField="FechaEgreso" HeaderText="Fecha Egreso" SortExpression="FechaEgreso" DataFormatString="{0:dd-MM-yyyy}"  />
                            <asp:BoundField DataField="MetodoPago" HeaderText="Método de Pago" SortExpression="MetodoPago" />
                            <asp:BoundField DataField="Transferencia" HeaderText="Transferencia" SortExpression="Transferencia" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbBitacora_Click"></asp:LinkButton>
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
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewEgreso" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
