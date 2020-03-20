<%@ Page Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteBalanceUnidades.aspx.cs" Inherits="SAT.Operacion.ReporteBalanceUnidades" %>

<%@ Register Src="~/UserControls/wucBitacoraMonitoreo.ascx" TagPrefix="monitoreo" TagName="wucBitacoraMonitoreo" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreoHistorial.ascx" TagPrefix="monitoreo" TagName="wucBitacoraMonitoreoHistorial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/Controles.css" rel="stylesheet" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryBalanceUnidades();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryBalanceUnidades() {
            $(document).ready(function () {

                //Añadiendo Encabezado Fijo
                $("#<%=gvUnidades.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorReporteBalanceUnidades").offsetWidth - 15,
                    height: 400
                    
                });



                //Validación 
                var validacionReporteEventos = function () {
                    var isValidP1 = !$("#<%=txtNoUnidad.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
                    return isValidP1 && isValidP2
                };
                //Validación de campos requeridos
                $("#<%=this.btnBuscar.ClientID%>").click(validacionReporteEventos);
                //Autocomplete
                $("#<%=txtNoUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>' });
                $("#<%=txtUbicacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
            });
        }

        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryBalanceUnidades();
    </script>
    <div id="encabezado_forma">
        <h1>Balance de Unidades</h1>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Buscar Unidades </h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatusUnidad">Estatus</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlEstatusUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlEstatusUnidad" runat="server" TabIndex="1" CssClass="dropdown">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoUnidad">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoUnidad" runat="server" TabIndex="2" AutoPostBack="true" CssClass="dropdown">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtNoUnidad">Unidad</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoUnidad" Enabled="true" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtNoServicio">Servicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoServicio" Enabled="true" runat="server" CssClass="textbox" MaxLength=" 30" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtUbicacion">Ubicación Actual</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
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
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="6" />
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
                    <asp:Chart ID="ChtEstatus" runat="server" TabIndex="10" BackColor="Transparent">
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
                    <asp:GridView ID="gvResumen" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        TabIndex="10" ShowFooter="True" CssClass="gridview"
                        PageSize="5" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
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
            <h2>Unidades</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamañoGridViewUnidades">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewUnidades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewUnidades" runat="server" OnSelectedIndexChanged="gvUnidades_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblCriterioGridViewUnidades">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterioGridViewUnidades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewUnidades" TabIndex="12" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvUnidades" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarExcelUnidades" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelUnidades" runat="server" Text="Exportar" TabIndex="13" OnClick="lkbExportarExcelUnidades_Onclick"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelUnidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorReporteBalanceUnidades">
            <asp:UpdatePanel ID="upgvUnidades" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvUnidades" CssClass="gridview" OnPageIndexChanging="gvUnidades_OnpageIndexChanging" OnSorting="gvUnidades_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True" TabIndex="14"
                        PageSize="25" Width="100%">
                        <Columns>

                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                            <asp:TemplateField HeaderText="No. Unidad" SortExpression="NumUnidad">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbNumUnidad" runat="server" Text='<%#Eval("NumUnidad") %>' OnClick="lkbNumUnidad_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador" SortExpression="Operador">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbOperador" runat="server" Text='<%#Eval("Operador") %>' OnClick="lkbOperador_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="TiempoCadena" HeaderText="Tiempo" SortExpression="Tiempo" />
                            <asp:BoundField DataField="UltimaUbicacion" HeaderText="Ultima Ubicación" SortExpression="UltimaUbicacion" />
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                            <asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" />
                            <asp:BoundField DataField="Vencimiento" HeaderText="Vencimiento" SortExpression="Vencimiento" />
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
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewUnidades" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA EL CONTROL DE USUARI EL HISTORIAL DE BITACORA MONITOREO -->
    <div id="modalBitacoraMonitoreoH" class="modal">
        <div id="BitacoraMonitoreoH" class="contenedor_modal_seccion_completa">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarBitacoraMonitoreoH" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarBitacoraMonitoreoH" OnClick="lkbCerrarBitacoraMonitoreoH_Click" runat="server" CommandName="Historial" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucBitacoraMonitoreoH" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <monitoreo:wucBitacoraMonitoreoHistorial ID="wucBitacoraMonitoreoH" OnlkbConsultar="wucBitacoraMonitoreoH_lkbConsultar" OnbtnNuevoBitacora="wucBitacoraMonitoreoH_btnNuevoBitacora" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
                    <asp:AsyncPostBackTrigger ControlID="wucBitMonitoreo" />

                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA EL CONTROL DE USUARIO DE BITACORA MONITOREO -->
    <div id="modalBitacoraM" class="modal">
        <div id="BitacoraM" class="contenedor_modal_seccion_completa">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarwucBitMonitoreo" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarBitMonitoreo" runat="server" OnClick="lkbCerrarBitMonitoreo_Click" Text="Cerrar"> 
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucBitMonitoreo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <monitoreo:wucBitacoraMonitoreo ID="wucBitMonitoreo" OnClickRegistrar="wucBitMonitoreo_ClickRegistrar" OnClickEliminar="wucBitMonitoreo_ClickEliminar" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreoH" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA EL HISTORIAL DE LOS OPERADORES-->
    <div id="modalHistorialOp" class="modal">
        <div id="HistorialOp" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialOperadores" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarHistorialOperadores" runat="server" OnClick="lkbCerrarHistorialOperadores_Click" Text="Cerrar"> 
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Historial</h2>
            </div>
            <div class="grid_seccion_completa_200px_altura">
                <asp:UpdatePanel ID="upgvHisOperador" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvHisOperador" OnPageIndexChanging="gvHisOperador_PageIndexChanging" CssClass="gridview" runat="server" AllowPaging="True" AutoGenerateColumns="false"
                            ShowFooter="True" TabIndex="14"
                            PageSize="5" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio" SortExpression="FechaInicio" />
                                <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" SortExpression="FechaFin" />
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
                        <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

