<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteLiquidacion12.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Liquidacion.ReporteLiquidacion12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada -->
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar los encabezados del gridview-->
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteLiquidacion();
            }
        }

        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryReporteLiquidacion() {
            $(document).ready(function () {

                //Añadiendo Encabezado Fijo
                $("#<%=gvLiquidacion.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorReporteLiquidacion").offsetWidth - 15,
                    height: 400,
                    freezesize: 6
                });


                //Validación 
                var validacionReporteLiquidacion = function () {

                var isValidP1 = !$("#<%=txtNoLiquidacion.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtValor.ClientID%>").validationEngine('validate');
                var isValidP3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                var isValidP4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                return isValidP1 && isValidP2;
            };
                //Validación de campos requeridos
                $("#<%=this.btnBuscar.ClientID%>").click(validacionReporteLiquidacion);

                // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
                $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

            });
        }

        //Invocación Inicial de método de configuración JQuery
      ConfiguraJQueryReporteLiquidacion();
    </script>
    <div id="encabezado_forma">
        <h1>Reporte Liquidaciones</h1>
    </div>
    <div class="seccion_controles">
       <div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar liquidaciones </h2>
</div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoLiquidacion">
                        No Liquidación
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoLiquidacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoLiquidacion" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="1" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoAsignacion">Tipo Asignación</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoAsignacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoAsignacion" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoAsignacion_OnSelectedIndexChanged" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblValor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblValor" runat="server" TabIndex="3">Unidad</asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtValor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoOperador">Tipo Operador</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoOperador" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoOperador" runat="server" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
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
                            <asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar por fechas de Liquidación."
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
                            <asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="7"></asp:TextBox>
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
                            <asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8"></asp:TextBox>
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
            <h2>Resumen por estatus</h2>
        </div>
        <div class="grafica_resumen_visor">
            <asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Chart ID="ChtLiquidaciones" runat="server" TabIndex="10" BackColor="Transparent">
                        <Legends>
                            <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                            </asp:Legend>
                        </Legends>
                    </asp:Chart>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                     <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="grid_resumen_visor">
            <asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvResumen" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        TabIndex="10" ShowFooter="True" CssClass="gridview"
                        PageSize="5" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:TemplateField HeaderText="Liquidaciones" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lbldetalles" Text='<%# Eval("Total") %>' runat="server"></asp:Label>
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
                     <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Documento.png" />
            <h2>Liquidaciones</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamañoGridViewLiquidacion">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewLiquidacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewLiquidacion" runat="server" OnSelectedIndexChanged="gvLiquidacion_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblCriterioGridViewLiquidacion">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterioGridViewLiquidacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewLiquidacion" TabIndex="12" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarExcelLiquidacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelLiquidacion" runat="server" Text="Exportar" TabIndex="13" OnClick="lkbExportarExcelLiquidacion_Onclick"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelLiquidacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorReporteLiquidacion">
            <asp:UpdatePanel ID="upgvLiquidacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvLiquidacion" CssClass="gridview" OnPageIndexChanging="gvLiquidacion_OnpageIndexChanging" OnSorting="gvLiquidacion_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True" TabIndex="14"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="NoLiquidacion" HeaderText="No Liq." SortExpression="NoLiquidacion" ItemStyle-Width="51px" HeaderStyle-Width="51px"/>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width="72px" HeaderStyle-Width="72px"/>
                            <asp:BoundField DataField="TipoAsignacion" HeaderText="Tipo Asignación" SortExpression="TipoAsignacion" ItemStyle-Width="92px" HeaderStyle-Width="92px" />
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" ItemStyle-Width="120px" HeaderStyle-Width="120px" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" ItemStyle-Width="103px" HeaderStyle-Width="103px"/>
                            <asp:BoundField DataField="Tercero" HeaderText="Tercero" SortExpression="Tercero" ItemStyle-Width="104px" HeaderStyle-Width="104px" />
                            <asp:BoundField DataField="Facturas" HeaderText="Facturas" SortExpression="Facturas" ItemStyle-Width="110px" HeaderStyle-Width="110px" />
                            <asp:TemplateField HeaderText="Servicios" SortExpression="Servicios">
                                <ItemStyle Width="110px" />
                                <HeaderStyle Width="110px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblServicios" runat="server" ToolTip='<%# Eval("Servicios") %>' CssClass="label_negrita"
                                        Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Servicios").ToString(), 10, "...") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liquidación" SortExpression="FechaLiquidacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Width="100px" HeaderStyle-Width="100px"/>
                            <asp:BoundField DataField="Diesel" HeaderText="Diesel" SortExpression="Diesel" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="SueldoFijo" HeaderText="Sueldo Fijo" SortExpression="SueldoFijo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="PagosViajes" HeaderText="Pago Viajes" SortExpression="PagosViajes" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="OtrosConceptos" HeaderText="Otros Conceptos" SortExpression="OtrosConceptos" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Estadias" HeaderText="Estadías" SortExpression="Estadias" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="BonoSemanal" HeaderText="Bono Semanal" SortExpression="BonoSemanal" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="Comprobaciones" HeaderText="Comprobaciones" SortExpression="Comprobaciones" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="Depositos" HeaderText="Depositos" SortExpression="Depositos" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>                                                                                   
                            <asp:BoundField DataField="IMSS" HeaderText="IMSS" SortExpression="IMSS" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="ISR" HeaderText="ISR" SortExpression="ISR" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="INFONAVIT" HeaderText="INFONAVIT" SortExpression="INFONAVIT" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Prestamo" HeaderText="Prestamo" SortExpression="Prestamo" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" FooterStyle-HorizontalAlign="Right" />
                             <asp:BoundField DataField="Deducciones" HeaderText="Deducciones" SortExpression="Deducciones"  DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:c}" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" ItemStyle-Width="90px"/>
                            <asp:BoundField DataField="NominaBasica" HeaderText="Nomina Basica" SortExpression="NominaBasica"  DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
                            <asp:BoundField DataField="NominaComplementaria" HeaderText="Nomina Complementaria"  DataFormatString="{0:c}" SortExpression="NominaComplementaria" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"/>
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
                    <asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewLiquidacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
