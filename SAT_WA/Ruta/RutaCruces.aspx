<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="RutaCruces.aspx.cs" Inherits="SAT.Ruta.RutaCruces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraRutaCruces();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraRutaCruces() {   //Inicializando Contenido
            $(document).ready(function () {
                //Declarando Función de Validación
                var validaBusquedaFactura = function () {
                    var isValid1 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1 && isValid2;
    }
    //Declarando Función de Validación
    var validaBusquedaCaseta = function () {
        var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
        var isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado
        return isValid1 && isValid2;
    }
    //Añadiendo Validación al Método Click Buscar Vale
    $("#<%=btnBuscarCaseta.ClientID%>").click(validaBusquedaCaseta);
    //Añadiendo Validación al Método Click Buscar Vale
    $("#<%=btnBuscarFactura.ClientID%>").click(validaBusquedaFactura);
    //Fecha Citas
    //Cargando Control DateTimePicker "Fecha Inicio"
    $("#<%=txtFecIni.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    //Cargando Control DateTimePicker "Fecha Fin"
    $("#<%=txtFecFin.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    // *** Fecha de Carga (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
    $('.scriptFechaE').datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    //Validación de controles de Edición de Vales
    var validacionEdicionCaseta = function () {
        var isValidP1 = !$('.scriptLitrosE').validationEngine('validate');
        var isValidP2 = !$('.scriptFechaE').validationEngine('validate');
        return isValidP1 && isValidP2;

    };
    //Validación de campos requeridos
    $('.scriptGuardarValeE').click(validacionEdicionCaseta);

});
        }
        //Ejecutando Función
        ConfiguraRutaCruces();
    </script>
    <div id="encabezado_forma">
        <h1>Asignación de Rutas Cruces</h1>
    </div>
    <div class="columna3x">
        <!--Controles-->
        <div class="seccion_controles" style="width: 600px;">
            <div class="header_seccion">
                <img src="../Image/FacturacionCargos.png" />
                <h2>Facturas</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtProveedor">Proveedor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlProveedor" runat="server" CssClass="dropdown2x" TabIndex="1" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSerie">Serie</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFolio">Folio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox validate[custom[positiveNumber]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="lblmontoactual">Monto Actual Factura:</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel runat="server" ID="Uplblmontoactual" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblmontoactual" runat="server" CssClass="label_negrita" Text="Por Asignar"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvfacturas" />
                            <asp:AsyncPostBackTrigger ControlID="gvIaves" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <label for="lblmontocasetas">Monto a Relacionar:</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel runat="server" ID="Uplblmontocasetas" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblmontocasetas" runat="server" CssClass="label_negrita" Text="Por Asignar"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvfacturas" />
                            <asp:AsyncPostBackTrigger ControlID="gvIaves" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" > <%--style="padding-bottom: 40px"--%>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscarFactura" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscarFactura" runat="server" Text="Buscar Factura" TabIndex="7"
                                CssClass="boton" OnClick="btnBuscarFactura_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!--Controles Grid-->
        <div class="seccion_controles" style="width: 600px;">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoFac">Mostrar:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoFac" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoFac" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                                TabIndex="9" OnSelectedIndexChanged="ddlTamanoFac_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoFactura">Ordenado:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenadoFactura" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenadoFactura" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvFacturas" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplnkExportarFactura" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarFactura" runat="server" Text="Exportar" TabIndex="10"
                                OnClick="lnkExportarFactura_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarFactura" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_media_altura">
                <asp:UpdatePanel ID="upgvFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvFacturas" runat="server" AllowSorting="true" AllowPaging="true"
                            CssClass="gridview" OnSorting="gvFacturas_Sorting" OnPageIndexChanging="gvFacturas_PageIndexChanging"
                            AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="100%" OnRowDataBound="gvFacturas_RowDataBound">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                                <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                                <asp:TemplateField SortExpression="UUID" HeaderText="UUID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUUIDComp" runat="server" Text='<%#TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.TruncaCadena(TSDK.Base.Cadena.InvierteCadena(Eval("UUID").ToString()), 12, "...")) %>'
                                            ToolTip='<%#Eval("UUID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="FecFac" HeaderText="Fecha Factura" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy }" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c}" />
                                <asp:BoundField DataField="Asignado" HeaderText="Asignado" SortExpression="Asignado" DataFormatString="{0:c}" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbSeleccionar" runat="server" OnClick="lkbSeleccionar_Click"
                                            Text="Seleccionar"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
                        <asp:AsyncPostBackTrigger ControlID="btnAsignarIave" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoFac" />
                        <asp:AsyncPostBackTrigger ControlID="gvIavesAsignados" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div class="columna3x">
        <!--Controles-->
        <div class="seccion_controles" style="width: 750px;">
            <div class="header_seccion">
                <img src="../Image/Evidencia.png" />
                <h2>Casetas Iave</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTag">Tag:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTag" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTag" runat="server" CssClass="textbox" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">No. Caseta</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNoCaseta" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoCaseta" runat="server" CssClass="textbox" TabIndex="6"></asp:TextBox>
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
                                <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="3" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Fechas Citas" Checked="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtFecFin">Fecha Fin</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="4" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>    
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnAsignarIave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAsignarIave" runat="server" Text="Asignar Casetas" CssClass="boton" TabIndex="13"
                                OnClick="btnAsignarIave_Click" />
                        </ContentTemplate>
                        <Triggers></Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscarCaseta" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscarCaseta" runat="server" Text="Buscar Casetas" TabIndex="8"
                                CssClass="boton" OnClick="btnBuscarCaseta_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <!--Controles Grid-->
        <div class="seccion_controles" style="width: 750px;">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoIave">Mostrar:</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoIave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoIave" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                                TabIndex="11" OnSelectedIndexChanged="ddlTamanoIave_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoIave">Ordenado:</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblOrdenadoIave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenadoIave" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvIaves" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplnkExportarVale" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarVale" runat="server" Text="Exportar" TabIndex="12"
                                OnClick="lnkExportarVale_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarVale" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_media_altura">
                <asp:UpdatePanel ID="upgvIaves" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvIaves" runat="server" AllowSorting="true" AllowPaging="true"
                            CssClass="gridview" OnSorting="gvIaves_Sorting" OnPageIndexChanging="gvIaves_PageIndexChanging"
                            AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="100%">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Caseta" SortExpression="Caseta">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCaseta" runat="server" Text='<%#Eval("Caseta") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NoCaseta" SortExpression="NoCaseta">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoCaseta" runat="server" Text='<%#Eval("NoCaseta") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tag" SortExpression="Tag">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTag" runat="server" Text='<%#Eval("Tag") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No.Servicio" SortExpression="NoServicio">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoServicio" runat="server" Text='<%#Eval("NoServicio") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asignación" SortExpression="Asignacion">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsignacion" runat="server" Text='<%#Eval("Asignacion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unidad" SortExpression="Unidad">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnidad" runat="server" Text='<%#Eval("Unidad") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Carga" SortExpression="Fecha">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtFechaE" MaxLength="18" runat="server" CssClass="textbox_100px scriptFechaE validate[required,custom[dateTime24]]">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("Fecha","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Descarga" SortExpression="FechaDes">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="FechaDes" MaxLength="18" runat="server" CssClass="textbox_100px scriptFechaDes validate[required,custom[dateTime24]]">
                                        </asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFechaDes" runat="server" Text='<%# Eval("FechaDes","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Monto" SortExpression="Monto">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonto" runat="server" Text='<%#Eval("Monto", "{0:c}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarCaseta" />
                        <asp:AsyncPostBackTrigger ControlID="btnAsignarIave" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoIave" />
                        <asp:AsyncPostBackTrigger ControlID="gvfacturas" />
                        <asp:AsyncPostBackTrigger ControlID="gvIavesAsignados" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon2x">
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Documento.png" />
            <h2>Casetas Asignados</h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamanoIavesAsig">Mostrar:</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoIavesAsig" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoIavesAsig" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                            TabIndex="13" OnSelectedIndexChanged="ddlTamanoIavesAsig_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label for="lblOrdenadoIavesAsig">Ordenado:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenadoIavesAsig" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenadoIavesAsig" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvIavesAsignados" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarValesAsig" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarValesAsig" runat="server" Text="Exportar" TabIndex="14"
                            OnClick="lnkExportarValesAsig_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarValesAsig" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa">
            <asp:UpdatePanel ID="upgvIavesAsignados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvIavesAsignados" runat="server" AllowSorting="true" AllowPaging="true"
                        CssClass="gridview" OnSorting="gvIavesAsignados_Sorting" OnPageIndexChanging="gvIavesAsignados_PageIndexChanging"
                        AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                            <asp:BoundField DataField="NoCaseta" HeaderText="No. Caseta" SortExpression="NoCaseta" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Asignacion" HeaderText="Asignacion" SortExpression="Asignacion" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="FechaCarga" HeaderText="FechaCarga" SortExpression="FechaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaDescarga" HeaderText="FechaDescarga" SortExpression="FechaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                            <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:c}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkQuitar" runat="server" OnClick="lnkQuitar_Click" Text="Quitar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvFacturas" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscarFactura" />
                    <asp:AsyncPostBackTrigger ControlID="btnAsignarIave" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoIavesAsig" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
