<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteVencimientos.aspx.cs" Inherits="SAT.General.ReporteVencimientos" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div id="encabezado_forma">
        <h1>Reporte Vencimientos</h1>
    </div>
    <div class="seccion_controles">
       <div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar Vencimiento</h2>
</div>
        <div class="columna2x">
                     <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipo">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtBeneficiario">
                        Identificador
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtIdentificador" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdentificador" runat="server" CssClass="textbox" TabIndex="3" MaxLength="200"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="4" />
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
            <h2>Resumen por días próximos a vencer</h2>
        </div>
        <div class="grafica_resumen_visor">
            <asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Chart ID="ChtVencimiento" runat="server" TabIndex="5" BackColor="Transparent">
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
                        TabIndex="6" ShowFooter="True" CssClass="gridview"
                        PageSize="10" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="DiaProximo" HeaderText="Días Próximo" SortExpression="DiaProximo" />
                            <asp:TemplateField HeaderText="Total" SortExpression="Total">
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
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Documento.png" />
            <h2>Vencimientos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamañoGridViewVencimiento">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewVencimiento" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewVencimiento" runat="server" OnSelectedIndexChanged="gvVencimiento_OnSelectedIndexChanged" TabIndex="7" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblCriterioGridViewVencimiento">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterioGridViewVencimiento" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewVencimiento" TabIndex="11" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvVencimiento" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarExcelVencimiento" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelVencimiento" runat="server" Text="Exportar" TabIndex="8" OnClick="lkbExportarExcelVencimiento_Onclick"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelVencimiento" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa">
            <asp:UpdatePanel ID="upgvVencimiento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvVencimiento" CssClass="gridview" OnPageIndexChanging="gvVencimiento_OnpageIndexChanging" OnSorting="gvVencimiento_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True" TabIndex="9"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="TipoVencimiento" HeaderText="Tipo Vencimiento" SortExpression="TipoVencimiento" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
                            <asp:BoundField DataField="Prioridad" HeaderText="Prioridad" SortExpression="Prioridad" />
                            <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:g}"   />
                            <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:g}"   />
                            <asp:BoundField DataField="ValorKilometraje" HeaderText="Kilometraje" SortExpression="ValorKilometraje" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
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
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewVencimiento" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

