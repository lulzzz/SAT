<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucHistorialCalificacion.ascx.cs" Inherits="SAT.Externa.wucHistorialCalificacion" %>
<!-- hoja de estilo que da formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<div class="seccion_controles">
    <div class="columna4x">
        <div class="header_seccion">
            <asp:UpdatePanel runat="server" ID="uplblEntidad" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label runat="server" ID="lblEntidad" CssClass="label_negrita" Font-Size="Large"></asp:Label>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="grid_seccion_completa_altura_variable">
            <asp:UpdatePanel ID="upgvHistorialCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvHistorialCalificacion" CssClass="gridview" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True" TabIndex="17" OnRowDataBound="gvHistorialCalificacion_RowDataBound"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Comentario" HeaderText="Comentario" SortExpression="Comentario" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Promedio" HeaderText="Promedio" SortExpression="Promedio" ItemStyle-HorizontalAlign="Right" />

                            <asp:TemplateField HeaderText="Asistencia" SortExpression="Asistencia" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgAsistencia" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Disciplina" SortExpression="Disciplina" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgDisciplina" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Presentación" SortExpression="Presentación" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgPresentacion" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Puntualidad" SortExpression="Puntualidad" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgPuntualidad" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rendimiento" SortExpression="Rendimiento" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgRendimiento" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Seguridad" SortExpression="Seguridad" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgSeguridad" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Citas a Tiempo" SortExpression="CitasTiempo" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgCitasTiempo" ImageUrl="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuidado de Producto" SortExpression="CuidadoProducto" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="imgCuidadoProducto" ImageUrl="" />
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
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
