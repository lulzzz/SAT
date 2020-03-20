<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActividadOrdenTrabajo.ascx.cs" Inherits="SAT.UserControls.ActividadOrdenTrabajo" %>
<div class="header_seccion">
    <img src="../Image/Evidencia.png" />
    <h2>Asignación de Actividades</h2>
</div>
<div class="seccion_controles" style="width: 1100px">
    <div class="encabezado_busqueda">
        <div class="columna" style="width: 250px">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="ddlArea">Familia</label>
                </div>
                <div class="control">
                    <asp:DropDownList ID="ddlArea" AutoPostBack="true" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" TabIndex="2"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="columna" style="width: 250px">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="ddlSubArea">Sub-familia</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlSubArea" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSubArea" AutoPostBack="true" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlArea" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna" style="width: 250px">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="ddlFallasActividad">Falla</label>
                </div>
                <div class="control">
                    <asp:DropDownList ID="ddlFallasActividad" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="columna" style="width: 250px">
            <div class="renglon">
                <div class="etiqueta">
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="6" CssClass="boton" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </div>
</div>
<div class="contenedor_controles" style="width: 1100px">
    <div class="contenedor_media_seccion_izquierda" style="width: 500px">
        <div class="header_seccion">
            <h2>Actividades </h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamañoGridViewActividades">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamañoGridViewActividades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewActividades" runat="server" CssClass="dropdown_100px"
                            AutoPostBack="true" TabIndex="13" OnSelectedIndexChanged="ddlTamañoGridViewActividades_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label for="lblCriterioGridViewActividades">Ordenado</label>
            </div>
            <div class="control_60px">
                <asp:UpdatePanel ID="uplblCriterioGridViewActividades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblCriterioGridViewActividades" runat="server" Text="" CssClass="label"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplkbExportarExcelgvActividades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelgvActividades" runat="server" Text="Exportar" OnClick="lkbExportarExcelgvActividades_Click"
                            TabIndex="14"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelgvActividades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvActividades" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvActividades" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="15" PageSize="10" Width="100%"
                        AutoGenerateColumns="False" ShowFooter="True" CssClass="gridview" OnPageIndexChanging="gvActividades_OnPageIndexChanging"
                        OnSorting="gvActividades_OnSorting">
                        <Columns>
                            <asp:BoundField DataField="Familia" HeaderText="Familia" SortExpression="Familia" />
                            <asp:BoundField DataField="Subfamilia" HeaderText="Sub Familia" SortExpression="Subfamilia" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Actividad" SortExpression="Descripcion" />
                            <asp:TemplateField HeaderText="" SortExpression="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAgregar" runat="server" Text="Agregar" OnClick="OnClick_btnRegistrar"></asp:LinkButton>
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
                    <asp:AsyncPostBackTrigger ControlID="gvActividadesAsignadas" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewActividades" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_media_seccion_derecha" style="width: 500px">
        <div class="header_seccion">
            <h2>Actividades de la Orden de Trabajo</h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamanoFacLigadas">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanogvActividadesAsignadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanogvActividadesAsignadas" OnSelectedIndexChanged="ddlTamanogvActividadesAsignadas_SelectedIndexChanged" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                            TabIndex="7">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label for="lblOrdenadogvActividadesAsignadas">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenadogvActividadesAsignadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadogvActividadesAsignadas" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvActividadesAsignadas" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportargvActividadesAsignadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportargvActividadesAsignadas" OnClick="lnkExportargvActividadesAsignadas_Click" runat="server" Text="Exportar" TabIndex="8"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportargvActividadesAsignadas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvActividadesAsignadas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvActividadesAsignadas" runat="server" OnSorting="gvActividadesAsignadas_Sorting" OnPageIndexChanging="gvActividadesAsignadas_PageIndexChanging" AllowPaging="true" AllowSorting="true" Width="100%"
                        PageSize="10" CssClass="gridview" ShowFooter="true" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Familia" HeaderText="Familia" SortExpression="Familia" />
                            <asp:BoundField DataField="Subfamilia" HeaderText="Sub familia" SortExpression="Subfamilia" />
                            <asp:BoundField DataField="Falla" HeaderText="Falla" SortExpression="Falla" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Actividad" SortExpression="Descripcion" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanogvActividadesAsignadas" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
