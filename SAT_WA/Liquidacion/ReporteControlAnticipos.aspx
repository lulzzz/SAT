<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteControlAnticipos.aspx.cs" Inherits="SAT.Liquidacion.ReporteControlAnticipos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryControlAnticipos();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraJQueryControlAnticipos() {
            //Inicializando Función
            $(document).ready(function () {
                //Añadiendo Encabezado Fijo
                $("#<%=gvControlAnticipos.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorControlAnticipos").offsetWidth - 15,
                    height: 400,
                    //freezesize: 2
                });
                //Cargando Catalogo de Autocompletado
                //Cliente
                $("#<%=txtCliente.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtCliente.ClientID%>").val(ui.item.value);
                        //Causando Actualización del Control
                        __doPostBack('<%= txtCliente.UniqueID %>', '');
                    }
                });
                                //Cargando Catalogo de Autocompletado
                $("#<%=txtClienteP.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                });
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
                //Configurando Validación de Controles
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Añadiendo Controles a la Validación
                    var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2 && isValid3 && isValid4;
                });
            });
        }
        //Invocando Función de Configuración
        ConfiguraJQueryControlAnticipos();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Reporte Control Anticipos</h1>
    </div>
    <div class="contenedor_controles">
        <asp:Panel runat="server" ID="pnlReporteSaldosDetalle" DefaultButton="btnBuscar">
            <div class="columna2x">
                <div class="renglon2x">
                    <label class="label_negrita">Filtros por cliente</label>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCliente">Cliente</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" 
                                    TabIndex="1" OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="ddlViajeMaestro">Viajes Maestros</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlViajeMaestro" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlViajeMaestro" runat="server" CssClass="dropdown2x" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                        </Triggers>
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
                  <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtNoServicio">No.Servicio</label>
                    </div>
                    <div class="control_60px">
                        <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNoServicio" runat="server" TabIndex="5" CssClass="textbox_50px" MaxLength="5"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>              
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtFolio">Folio</label>
                    </div>
                    <div class="control_60px">
                        <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolio" runat="server" TabIndex="6" CssClass="textbox_50px validate[custom[onlyNumberSp]]" MaxLength="8"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80px">
                        <label for="txtSerie">Serie</label>
                    </div>
                    <div class="control">
                        <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" MaxLength="10" TabIndex="7"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="columna2x">              
                <div class="renglon2x">
                    <label class="label_negrita">Filtros por Proveedor</label>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtClienteP">Proveedor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtClienteP" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtClienteP" runat="server" TabIndex="8" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>

                 <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFolioP">Folio</label>
                    </div>
                    <div class="control_60px">
                        <asp:UpdatePanel ID="uptxtFolioP" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolioP" runat="server" TabIndex="9" CssClass="textbox_50px validate[custom[onlyNumberSp]]" MaxLength="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
 
                    <div class="etiqueta_80px">
                  <label for="txtSerieP">Serie</label>
                </div>
                <div class="control">
                <asp:TextBox ID="txtSerieP" runat="server" CssClass="textbox" MaxLength="10" TabIndex="10"></asp:TextBox>
                </div>
                      </div>

                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUUID">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUID" runat="server" TabIndex="11" CssClass="textbox2x" MaxLength="36"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="12" CssClass="boton" OnClick="btnBuscar_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="contenedor_controles">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="13"
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
                        <asp:AsyncPostBackTrigger ControlID="gvControlAnticipos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="14" CommandName="ControlAnticipos" OnClick="lkbExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="contenedorControlAnticipos" class="grid_seccion_completa_altura_variable">
            <asp:UpdatePanel ID="upgvControlAnticipos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvControlAnticipos" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="15"
                        OnRowDataBound="gvControlAnticipos_RowDataBound" OnPageIndexChanging="gvControlAnticipos_PageIndexChanging"
                        OnSorting="gvControlAnticipos_Sorting" PageSize="250" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
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
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente"   /> <%--ItemStyle-Width="51px" HeaderStyle-Width="51px"--%>
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio"  />     <%-- ItemStyle-Width="60px" HeaderStyle-Width="60px"--%>                                                 
                            <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje"/>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas" />
                            <asp:BoundField DataField="CitaCarga" HeaderText="Fec. CitaCarga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="CitaDescarga" HeaderText="Fec. CitaDescarga" SortExpression="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Imp. SubTotal" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Trasladado" HeaderText="Imp. Trasladado" SortExpression="Trasladado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Retenido" HeaderText="Imp. Retenido" SortExpression="Retenido" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FacturaGlobal" HeaderText="Factura Global" SortExpression="FacturaGlobal" />
                            <asp:BoundField DataField="VersionCFDI" HeaderText="Version CFDI" SortExpression="VersionCFDI" />
                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                            <asp:BoundField DataField="TotalCFDI" HeaderText="Total CFDI" SortExpression="TotalCFDI" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" ItemStyle-CssClass="label_negrita"  />
                            <asp:BoundField DataField="TotalAnticiposCC" HeaderText="Total Anticipos (Con Chofer)" SortExpression="TotalAnticiposCC" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#3AB53A" HeaderStyle-ForeColor="White" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalAnticiposSC" HeaderText="Total Anticipos (Sin Chofer)" SortExpression="TotalAnticiposSC" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#E24848" HeaderStyle-ForeColor="White" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalAnticiposRes" HeaderText="Total Anticipos (Restantes)" SortExpression="TotalAnticiposRes" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#4285F4" HeaderStyle-ForeColor="White" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                            <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                            <asp:BoundField DataField="TotalFacturaCXP" HeaderText="Total Factura CXP" SortExpression="TotalFacturaCXP" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalFiniquitos" HeaderText="Total Finiquitos" SortExpression="TotalFiniquitos" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#000000" HeaderStyle-ForeColor="White" DataFormatString="{0:C2}">
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
