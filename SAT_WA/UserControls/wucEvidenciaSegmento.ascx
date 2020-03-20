<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucEvidenciaSegmento.ascx.cs" Inherits="SAT.UserControls.wucEvidenciaSegmento" %>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Zoom y Validadores JQuery -->
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada  y Zoom-->
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/Transportista.png" />
        <h2>Segmento</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamanoSegmentos">Mostrar:</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoReqDisp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoSegmentos" runat="server" CssClass="dropdown_100px" TabIndex="1" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamanoSegmentos_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenarSeg">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenarSeg" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenarSeg" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSegmentos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportarSegmento" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarSegmento" runat="server" OnClick="lnkExportarSegmento_Click" Text="Exportar" TabIndex="2"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarSegmento" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_150px_altura">
            <asp:UpdatePanel ID="upgvSegmentos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvSegmentos" runat="server" TabIndex="11" PageSize="5" EmptyDataText="No hay registros"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvSegmentos_Sorting" Width="475px"
                        OnPageIndexChanging="gvSegmentos_PageIndexChanging" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True"
                        OnRowDataBound="gvSegmentos_RowDataBound">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="Segmento" HeaderText="Segmento" SortExpression="Segmento" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="ParadaIni" HeaderText="Parada Inicio" SortExpression="ParadaIni" />
                            <asp:BoundField DataField="ParadaFin" HeaderText="Parada Fin" SortExpression="ParadaFin" />
                            <asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVerDocumentos" runat="server" OnClick="lnkVerDocumentos_Click" Text="Ver Documentos"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplnkImprimirHI" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkImprimirHI" runat="server" OnClick="lnkImprimirHI_Click" Text="Imprimir"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="lnkImprimirHI" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoSegmentos" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoEvidencia">Mostrar:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoEvidencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoEvidencia" runat="server" CssClass="dropdown_100px" TabIndex="3" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTamanoEvidencia_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenarEvi">Ordenado Por:</label>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblOrdenarEvi" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenarEvi" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvEvidencias" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50pxr">
                    <asp:UpdatePanel ID="uplnkExportarEvidencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarEvidencia" runat="server" OnClick="lnkExportarEvidencia_Click" Text="Exportar" TabIndex="4"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarEvidencia" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="grid_seccion_completa_150px_altura">
            <asp:UpdatePanel ID="upgvEvidencias" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvEvidencias" runat="server" TabIndex="5" PageSize="5" EmptyDataText="No hay registros"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvEvidencias_Sorting"
                        OnPageIndexChanging="gvEvidencias_PageIndexChanging" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
                            <asp:BoundField DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" Visible="false" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Remitente" HeaderText="Remitente" SortExpression="Remitente" />
                            <asp:BoundField DataField="Destinatario" HeaderText="Destinatario" SortExpression="Destinatario" />
                            <asp:BoundField DataField="Evidencia" HeaderText="Evidencia" SortExpression="Evidencia" />
                            <asp:BoundField DataField="LugarCobro" HeaderText="Lugar de Cobro" SortExpression="LugarCobro" />
                            <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="lnkEvidencias_Click">
                                    <img src="../Image/ImagenEvidencia.png" width="20" height="20" />
                                </asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoSegmentos" />
                    <asp:AsyncPostBackTrigger ControlID="gvSegmentos" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div id="contenidoVentanaEvidencias" class="modalControlUsuario">
<div id="ventanaEvidencias" class="contenedor_modal_imagenes">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="contenedor_imagenes_patio">
<div class="header_seccion">
<img src="../Image/Imagenes.png" />
<h2>Evidencias de Viaje</h2>
</div>
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
</asp:HyperLink>
</ContentTemplate>                        
</asp:UpdatePanel>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenImagenes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DataList ID="dtlImagenImagenes" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id")  %>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:DataList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
