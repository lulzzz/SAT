<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="AutorizacionValesCombustible.aspx.cs" Inherits="SAT.EgresoServicio.AutorizacionValesCombustible" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Estilos-->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Script-->
    <script></script>
    <!--Encabezado-->
    <div id="encabezado_forma">
        <img src="../Image/GotaCombustible.png" style="height:100%"/>
        <h1>Autorización de vales para combustible</h1>
    </div>
    <!--Contenedor principal (GridView)-->
    <div class="seccion_controles" style="width:100%">
         <!--Renglon encabezado-->
        <div class="renglon" style="width:100%; height:auto">
            <!--LBL Mostrar-->
            <div class="etiqueta_50px">
                <label for="ddlTamanoGrid">Mostrar:</label>
            </div>
            <!--DDL Mostrar-->
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList AutoPostBack="true" ID="ddlTamanoGrid" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoGrid_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!--LBL "Ordenado"-->
            <div class="etiqueta" style="text-align:right">
                <label>Ordenado:</label>
            </div>
            <!--LBL Ordenado-->
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvAutorizacionValesCombustible" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <!--LNK Exportar-->
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" CommandName="ValesCombustible" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--GridView-->
        <div class="grid_seccion_completa_400px_altura" style="width:98%; height:auto">
            <asp:UpdatePanel ID="upgvAutorizacionValesCombustible" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvAutorizacionValesCombustible" runat="server" AllowPaging="true" AllowSorting="true" Enabled="true" PageSize="25" CssClass="gridview" ShowFooter="true" OnSorting="gvAutorizacionValesCombustible_OnSorting" OnPageIndexChanging="gvAutorizacionValesCombustible_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader"/>
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkTodosValesCombustible" runat="server" OnCheckedChanged="chkTodosValesCombustible_CheckedChanged" AutoPostBack="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVariosValesCombustible" runat="server" OnCheckedChanged="chkTodosValesCombustible_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoVale" HeaderText="Número de Vale" SortExpression="NoVale" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="FechaCarga" HeaderText="Fecha Carga" SortExpression="FechaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="TipoCombustible" HeaderText="Tipo de Combustible" SortExpression="TipoCombustible" />
                            <asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros">
                                <ItemStyle HorizontalAlign="Right"/>
                                <FooterStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="PrecioPorLitro" HeaderText="Precio por litro" SortExpression="PrecioPorLitro" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right"/>
                                <FooterStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right"/>
                                <FooterStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="LitrosPermitidos" HeaderText="Litros Permitidos" SortExpression="LitrosPermitidos">
                                <ItemStyle HorizontalAlign="Right"/>
                                <FooterStyle HorizontalAlign="Right"/>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoGrid" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!--Botónes-->
        <div class="renglon4x">
            <!--Btn Actualizar-->
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnActualizarVales" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button Text="Actualizar Vales" ID="btnActualizarVales" runat="server" CssClass="boton" OnClick="btnActualizarEstatusVales_Click" CommandName="ActualizarVales" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnActualizarVales" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <!--Btn Rechazar-->
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnRechazarVales" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button Text="Rechazar Vales" ID="btnRechazarVales" runat="server" CssClass="boton_cancelar" OnClick="btnActualizarEstatusVales_Click" CommandName="RechazarVales"/>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnRechazarVales" />
                    </Triggers>
                </asp:UpdatePanel>                
            </div>
        </div>
    </div>
</asp:Content>