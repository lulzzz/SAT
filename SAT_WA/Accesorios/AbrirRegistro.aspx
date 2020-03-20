<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbrirRegistro.aspx.cs" Inherits="SAT.Accesorios.AbrirRegistro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!-- Estilos de los Controles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>        
        <div class="contenido_control_abrir">
            <div class="encabezado_control">
                <h2>Abrir Registro</h2>     
            </div>
            <div class="contenido_resultado_abrir">
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamano">Mostrar:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamano" runat="server" TabIndex="4" CssClass="dropdown_100px"
                                    OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label>Ordenado:</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblOrdenado" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAbrir" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportar" runat="server" TabIndex="5" 
                                    OnClick="lnkExportar_Click" Text="Exportar"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_abrir">
                    <asp:UpdatePanel ID="upgvAbrir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAbrir" runat="server" TabIndex="5" OnSorting="gvAbrir_Sorting" CssClass="gridview"
                                OnPageIndexChanging="gvAbrir_PageIndexChanging" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvAbrir_RowDataBound" ShowFooter="true" Width="100%" PageSize="5">
                                <AlternatingRowStyle CssClass="gridviewrowalternate"/>
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
                            <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="contenido_filtro_abrir">
                <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlFiltro">Filtro</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlFiltro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFiltro" runat="server" CssClass="dropdown2x" TabIndex="1"
                                    OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label>Valor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upBusqueda" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtBusqueda" runat="server" TabIndex="2" CssClass="textbox2x"
                                    Visible="true"></asp:TextBox>
                                <asp:DropDownList ID="ddlBusqueda" runat="server" CssClass="dropdown" TabIndex="2"
                                    Visible="false"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlFiltro" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>                    
                </div>
                <div class="renglon2x">
                    <div class="controlBoton"></div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="3" 
                                    OnClick="btnBuscar_Click" CssClass="boton" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>    
            </div>  
            <div class="imagen_control">
                <img src="../Image/Abrir.png" />
            </div>     
        </div>  
        
    </form>
</body>
</html>
