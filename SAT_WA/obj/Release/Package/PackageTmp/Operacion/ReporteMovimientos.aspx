<%@ Page Title="Reporte de Movimientos" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteMovimientos.aspx.cs" Inherits="SAT.Operacion.ReporteMovimientos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
                <%--<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />--%>
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca encabezados GridView-->
                <%--<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteMovimientos();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryReporteMovimientos() {
            $(document).ready(function () {

                //Origen y Destino
                $("#<%=txtOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                $("#<%=txtDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

                //Validación de Controles
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Validando Controles
                    var isValid1 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtOrigen.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtDestino.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtEntidad.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtFechaIni.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');

                    //var total = isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6;
                    //alert("1.-" + isValid1 + "\n2.-" + isValid2 + "\n3.-" + isValid3 + "\n4.-" + isValid4 + "\n5.-" + isValid5 + "\n6.-" + isValid6 + "\nR=" + total);

                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6;
                });

                //Cargando Control DateTimePicker
                $("#<%=txtFechaIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Añadiendo Encabezado Fijo
                $("#<%=gvMovimientos.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorReporteMovimientos").offsetWidth - 15,
                    height: 400
                });

            });
        }

        //Invocando Función de Configuración
        ConfiguraJQueryReporteMovimientos();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/paradas.png" />
        <h1>Visor de Movimientos</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmi">Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompaniaEmi" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompaniaEmi" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoEntidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">No. Servicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkMovVacio" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkMovVacio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkMovVacio" runat="server" Text="¿Movimiento Vacio?" TabIndex="2"
                                OnCheckedChanged="chkMovVacio_CheckedChanged" AutoPostBack="true" />
                        </ContentTemplate>
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="3"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipo">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOrigen">Origen</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOrigen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDestino">Destino</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDestino" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoEntidad">Tipo Asignación</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoEntidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoEntidad" runat="server" CssClass="dropdown" TabIndex="7" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTipoEntidad_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEntidad">Entidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtEntidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEntidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoEntidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaIni">Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="9"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaFin">Termino</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="10"></asp:TextBox>
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
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="10" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <h2>Resultados Obtenidos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="11"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="12" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorReporteMovimientos">
            <asp:UpdatePanel ID="upgvFichasIngreso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvMovimientos" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                        CssClass="gridview" ShowFooter="True" TabIndex="13" OnSorting="gvMovimientos_Sorting"
                        OnPageIndexChanging="gvMovimientos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" />
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="EstatusDocumentos" HeaderText="EstatusDocumentos" SortExpression="EstatusDocumentos" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" />
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Proveedor" HeaderText="Tercero" SortExpression="Proveedor" />
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                            <asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" SortExpression="FechaIni" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Kms" HeaderText="Kilometros" SortExpression="Kms">
                                <ItemStyle HorizontalAlign="Right"/>
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TiempoTransitoEstimado" HeaderText="Tiempo Transito (Estimado)" SortExpression="TiempoTransitoEstimado">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TiempoTransitoReal" HeaderText="Tiempo Transito (Real)" SortExpression="TiempoTransitoReal">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EstatusLiq" HeaderText="Estatus Liq." SortExpression="EstatusLiq" />
                            <asp:BoundField DataField="TPagos" HeaderText="T. Pagos" SortExpression="TPagos" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TAnticipos" HeaderText="T. Anticipos" SortExpression="TAnticipos" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TComprobaciones" HeaderText="T. Comprobaciones" SortExpression="TComprobaciones" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TDiesel" HeaderText="T. Diesel" SortExpression="TDiesel" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
