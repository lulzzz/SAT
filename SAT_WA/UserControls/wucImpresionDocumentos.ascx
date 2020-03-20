<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucImpresionDocumentos.ascx.cs" Inherits="SAT.UserControls.wucImpresionDocumentos" %>
<%@ Register Src="~/UserControls/wucImpresionPorte.ascx" TagPrefix="tectos" TagName="wucImpresionPorte" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryImpresionDocumentos();
        }
    }

    function ConfiguraJQueryImpresionDocumentos()
    {   // *** Fecha de carga, descarga (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
        $(document).ready(function () {
            //Función de Validación del Control
            $("#<%=btnBuscar.ClientID%>").click(function () {
                //Validando Control
                var isValid1;
                var isValid2;

                //Validando el Control
                if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
                    //Validando Controles
                    isValid1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                    isValid2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                }
                else {
                    //Asignando Valor Positivo
                    isValid1 = true;
                    isValid2 = true;
                }

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2;
            });

            $("#<%=lbxOperacionServicio.ClientID%>").multiselect({
                selectedList: 2,
                selectall: 1
            });
            $("#<%=lbxAlcance.ClientID%>").multiselect({
                selectedList: 2,
                selectall: 1
            });
            //Cargando Catalogo de Unidades
            $("#<%=txtUnidad.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
            });
            //Cargando Catalogo de Operadores
            $("#<%=txtOperador.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
            });
            //Cargando Controles de Fecha
            $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
        });
    }
    //Declarando Función de Validación de Fechas
    function CompareDates() {

        //Obteniendo Valores
        var txtDate1 = $("#<%=txtFechaInicio.ClientID%>").val();
        var txtDate2 = $("#<%=txtFechaFin.ClientID%>").val();

        //Fecha en Formato MM/DD/YYYY
        var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
        var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

        //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
        if (date1 > date2)
            //Mostrando Mensaje de Operación
            return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
    }
    //Invocando Funcion de Configuracion
    ConfiguraJQueryImpresionDocumentos();
</script>
<div class="contenida_control_documentos">
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/print2.png" width="32" height="32" />
            <h2>Impresión De Documentos</h2>     
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control">
                    <asp:CheckBox ID="chkDocumentados" runat="server" Checked="true" Text="Ver Documentados" />
                </div>
                <div class="control">
                    <asp:CheckBox ID="chkIniciados" runat="server" Checked="true" Text="Ver Iniciados" />
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtViaje">No. Viaje</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtViaje" runat="server" CssClass="textbox" TabIndex="3"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uprbCarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rbCarga" runat="server" Text="Carga" GroupName="General" Checked="true" TabIndex="4" />
                        </ContentTemplate>
                        <Triggers>
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_80px">
                    <asp:UpdatePanel ID="uprbDescarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rbDescarga" runat="server" Text="Descarga" GroupName="General" TabIndex="5" />
                        </ContentTemplate>
                        <Triggers>
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_80px">
                    <asp:UpdatePanel ID="uprbInicioViaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rbInicioViaje" runat="server" Text="Inicio Viaje" GroupName="General" TabIndex="6" />
                        </ContentTemplate>
                        <Triggers>

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_80px">
                    <asp:UpdatePanel ID="uprbFinViaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rbFinViaje" runat="server" Text="Fin Viaje" GroupName="General" TabIndex="7" />
                        </ContentTemplate>
                        <Triggers>
 
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="9" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="10" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>            
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtServicio">No. Servicio</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtServicio" runat="server" CssClass="textbox" TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOperador">Operador</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x" TabIndex="11"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUnidad">Unidad</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox" TabIndex="12"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lbxOperacionServicio">Operación</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uplbxOperacionServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListBox runat="server" ID="lbxOperacionServicio" SelectionMode="multiple" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lbxAlcance">Alcance</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uplbxAlcance" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ListBox runat="server" ID="lbxAlcance" SelectionMode="multiple" />
                        </ContentTemplate>
                        <Triggers>
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="8" />
                </div>
            </div>
        </div>        
    </div>
    <div class="seccion_controles" style="width:1330px;">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server"
                            TabIndex="5" CssClass="dropdown_100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
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
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click" ></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_encabezado_fijo">
            <asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvServicios" runat="server" AllowPaging="true" AllowSorting="true"
                        OnPageIndexChanging="gvServicios_PageIndexChanging" OnSorting="gvServicios_Sorting" OnRowDataBound="gvServicios_RowDataBound"
                        PageSize="25" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="false" EmptyDataText="No hay registros" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="idServicio" HeaderText="idServicio" SortExpression="idServicio" Visible="false" />
                            <asp:BoundField DataField="Documentacion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Documentación" SortExpression="Documentacion">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                            <asp:BoundField DataField="OperacionAlcance" HeaderText="Operacion/Alcance" SortExpression="OperacionAlcance" ItemStyle-CssClass="label_correcto" />
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CartaPorte" HeaderText="Carta Porte" SortExpression="CartaPorte">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                            <asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga"></asp:BoundField>
                            <asp:BoundField DataField="FechaLlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Carga" SortExpression="FechaLlegadaCarga"></asp:BoundField>
                            <asp:BoundField DataField="EstatusLlegadaCarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaCarga" />
                            <asp:BoundField DataField="FechaSalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Salida Carga" SortExpression="FechaSalidaCarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaLlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Descarga" SortExpression="FechaLlegadaDescarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EstatusLlegadaDescarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaDescarga" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub-Total" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Traslados" HeaderText="Traslados" SortExpression="Traslados" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Retenciones" HeaderText="Retenciones" SortExpression="Retenciones" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField SortExpression="Total" HeaderText="Total">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbCalculaTarifa" runat="server" Text='<%# Eval("Total", "{0:C2}") %>'
                                        ToolTip="Calcule su Tarifa" OnClick="lkbCalculaTarifa_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor" />
                            <asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" />
                            <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
                            <asp:TemplateField HeaderText="Carta Porte">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbPorte" runat="server" ImageUrl="~/Image/imprimir.png" Width="28" Height="28" OnClick="imbImprimir_Click" CommandName="Porte" ToolTip="Imprimir Carta Porte" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Carta Porte Viajera">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbViajera" runat="server" ImageUrl="~/Image/imprimir.png" Width="28" Height="28" OnClick="imbImprimir_Click" CommandName="Viajera" ToolTip="Imprimir Carta Porte Viajera" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hoja Instrucción">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbInstruccion" runat="server" ImageUrl="~/Image/imprimir.png" Width="28" Height="28" OnClick="imbImprimir_Click" CommandName="Instruccion" ToolTip="Imprimir Hoja de Instrucción" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gastos">
                                <ItemStyle HorizontalAlign="Center" Width="50" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbGastos" runat="server" ImageUrl="~/Image/imprimir.png" Width="28" Height="28" OnClick="imbImprimir_Click" CommandName="Gastos" ToolTip="Imprimir Gastos" />
                                </ItemTemplate>
                            </asp:TemplateField>                      
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
</div>

<!-- Ventana de Impresión -->
    <div id="contenedorVentanaImpresionPorte" class="modal">
        <div id="ventanaImpresionPorte" class="contenedor_ventana_confirmacion" style="width: auto;">
            <div class="columna2x">
                <asp:UpdatePanel ID="upwucImpresionPorte" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:wucImpresionPorte ID="wucImpresionPorte" runat="server" OnClickImprimirCartaPorte="wucImpresionPorte_ClickImprimirCartaPorte" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>