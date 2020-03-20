<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="SustitucionCxC.aspx.cs" Inherits="SAT.CuentasCobrar.SustitucionCxC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Facturado.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar encabeados GridView-->
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                //Invocando Función de Configuración
                ConfiguraJQuerySustitucionCxC();
            }
        }

        //Creando Función de Configuración
        function ConfiguraJQuerySustitucionCxC() {
            $(document).ready(function(){

                /** Cargando Catalogo de Autocompletado **/
                //Cliente
                $("#<%=txtCliente.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtCliente.ClientID%>").val(ui.item.value);
                        //Causando Actualización del Control
                        __doPostBack('<%= txtCliente.UniqueID %>', '');
                    }
                });
                //Ubicación de Carga
                $("#<%=txtLugarCarga.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
                });
                //Ubicación de Descarga
                $("#<%=txtLugarDescarga.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
                });

                /** Cargando Controles de Fecha **/
                //Facturación Otros/CFDI
                $("#<%=txtFInicio.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFTermino.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Servicios
                $("#<%=txtInicioServ.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtTerminoServ.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Validación de Controles
                $("#<%=btnBuscarFO.ClientID%>").click(function() {
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2;
                    var isValid3; 

                    //Validando el Control
                    if ($("#<%=chkIncluirFO.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid2 = !$("#<%=txtFInicio.ClientID%>").validationEngine('validate');
                        isValid3 = !$("#<%=txtFTermino.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid2 = true;
                        isValid3 = true;
                    }

                    var isValid4 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    
                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3 && isValid4;
                });

                //Validación de Controles
                $("#<%=btnBuscarServ.ClientID%>").click(function() {
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2;
                    var isValid3; 

                    //Validando el Control
                    if ($("#<%=chkIncluirServ.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid2 = !$("#<%=txtInicioServ.ClientID%>").validationEngine('validate');
                        isValid3 = !$("#<%=txtTerminoServ.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid2 = true;
                        isValid3 = true;
                    }

                    var isValid4 = !$("#<%=txtLugarCarga.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtLugarDescarga.ClientID%>").validationEngine('validate');
                    
                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                });

                //Añadiendo Encabezado Fijo
                $("#<%=gvFacturacionOtros.ClientID%>").gridviewScroll({
                    width: document.getElementById("facturasPorSustituir").offsetWidth - 15,
                    height: 250
                });
                $("#<%=gvServicios.ClientID%>").gridviewScroll({
                    width: document.getElementById("serviciosSinFacturar").offsetWidth - 15,
                    height: 250
                });

            });
        }

        //Invocando Función de Configuración
        ConfiguraJQuerySustitucionCxC();

        //Declarando Función de Validación de Fechas
        function CompareDatesFO() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtFInicio.ClientID%>").val();
            var txtDate2 = $("#<%=txtFTermino.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
            //Mostrando Mensaje de Operación
            return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
        }

        //Declarando Función de Validación de Fechas
        function CompareDatesServ() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtInicioServ.ClientID%>").val();
            var txtDate2 = $("#<%=txtTerminoServ.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
            //Mostrando Mensaje de Operación
            return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
        }

    </script>

    <div id="encabezado_forma">
        <img src="../Image/archivo_xml1.png" />
        <h1>Sustitución de Facturas (Viajes)</h1>
    </div>
    <div class="contenedor_controles">
        <div class="header_seccion">
            <img src="../Image/Compania.png" width="32" height="32" />
            <h2>Especifique el Cliente que desee</h2>
        </div>
        <div class="columna3x">
            <div class="renglon2x">
                <div class="etiqueta_80px">
                    <label>Cliente</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" 
                                TabIndex="1" AutoPostBack="true" OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="columna3x">
            <asp:Panel ID="pnlBusquedaFacturacion" runat="server" DefaultButton="btnBuscarFO">
                <div class="header_seccion">
                    <img src="../Image/Buscar.png" width="32" height="32" />
                    <h2>Busqueda de Facturas Generadas (Otros)</h2>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_80px">
                        <label for="txtSerie">Serie</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px" TabIndex="2" MaxLength="30"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80px">
                        <label for="txtFolio">Folio</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px validate[custom[onlyNumberSp]]" TabIndex="3" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_80px">
                        <label for="txtUUID">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x" MaxLength="100" TabIndex="4"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_200px">
                        <label class="label_negrita">Fechas de Expedición del CFDI</label>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_80px">
                        <label for="txtFInicio">Inicio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFInicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFInicio" runat="server" CssClass="textbox validate[custom[dateTime24]]" 
                                    TabIndex="5" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80pxr">
                        <asp:UpdatePanel ID="upchkIncluirFO" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkIncluirFO" runat="server" Text="¿Incluir?" Checked="true" TabIndex="6" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_80px">
                        <label for="txtFTermino">Termino</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFTermino" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFTermino" runat="server" CssClass="textbox validate[custom[dateTime24], funcCall[CompareDatesFO[]]]" 
                                    TabIndex="7" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_200px">
                        <label class="label_negrita">Datos adicionales de Facturación</label>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_80px">
                        <label for="txtReferenciaFO">Referencia</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtReferenciaFO" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReferenciaFO" runat="server" CssClass="textbox2x" MaxLength="100" TabIndex="8"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscarFO" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscarFO" runat="server" Text="Buscar" CssClass="boton"
                                    OnClick="btnBuscarFO_Click" TabIndex="9" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="columna3x">
            <asp:Panel ID="pnlBusquedaServicios" runat="server" DefaultButton="btnBuscarServ">
                <div class="header_seccion">
                    <img src="../Image/Buscar.png" width="32" height="32" />
                    <h2>Busqueda de Servicios</h2>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtNoServicio">No. Servicio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="10" MaxLength="30"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtReferencia">No. Viaje</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" TabIndex="11" MaxLength="100"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_200px">
                        <label class="label_negrita">Origen y Destino</label>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtLugarCarga">Lugar Carga</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtLugarCarga" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtLugarCarga" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtLugarDescarga">Lugar Descarga</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtLugarDescarga" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtLugarDescarga" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="13"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_200px">
                        <label class="label_negrita">Fechas de Carga/Descarga</label>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_80px">
                        <label for="txtFInicio">Inicio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtInicioServ" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtInicioServ" runat="server" CssClass="textbox validate[custom[dateTime24]]" 
                                    TabIndex="14" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_80pxr">
                        <asp:UpdatePanel ID="upchkIncluirServ" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkIncluirServ" runat="server" Text="¿Incluir?" Checked="true" TabIndex="15" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x" style="float:left;">
                    <div class="etiqueta_80px">
                        <label for="txtFTermino">Termino</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtTerminoServ" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTerminoServ" runat="server" CssClass="textbox validate[custom[dateTime24], funcCall[CompareDatesServ[]]]" 
                                    TabIndex="16" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscarServ" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscarServ" runat="server" CssClass="boton" Text="Buscar"
                                    TabIndex="17" OnClick="btnBuscarServ_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="contenedor_controles">
        <div class="columna3x">
            <div class="header_seccion">
                <img src="../Image/FacturacionCargos.png" width="32" height="32" />
                <h2>Facturas de Otros (CFDI)</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoFO">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoFO" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoFO" runat="server" CssClass="dropdown_100px" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlTamanoFO_SelectedIndexChanged" TabIndex="18"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoPago">Ordenado</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblOrdenadoFO" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenadoFO" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvFacturacionOtros" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_30pxr">
                    <asp:UpdatePanel ID="uplkbExportarFO" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbExportarFO" runat="server" Text="Exportar" TabIndex="19"
                                OnClick="lkbExportar_Click" CommandName="FacOtros"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbExportarFO" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="facturasPorSustituir" class="grid_seccion_completa_altura_variable">
                <asp:UpdatePanel ID="upgvPagosEgresos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvFacturacionOtros" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                            TabIndex="20" OnSorting="gvFacturacionOtros_Sorting" OnPageIndexChanging="gvFacturacionOtros_PageIndexChanging"
                            OnRowDataBound="gvFacturacionOtros_RowDataBound" Width="100%" AllowPaging="true" AllowSorting="true" ShowFooter="true">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:TemplateField HeaderText="Seleccionar">
                                    <HeaderStyle Width="80px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbSeleccionarFO" runat="server" ImageUrl="~/Image/Entrada.png" Width="22px" Height="22px"
                                            OnClick="imbSeleccionarFO_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdFO" SortExpression="IdFO" HeaderText="IdFO" Visible="false" />
                                <asp:BoundField DataField="IdFacturado" SortExpression="IdFacturado" HeaderText="IdFacturado" Visible="false" />
                                <asp:BoundField DataField="IdCFDI" SortExpression="IdCFDI" HeaderText="IdCFDI" Visible="false" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" ItemStyle-Width="70px" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" ItemStyle-Width="60px" HeaderStyle-Width="60px" />
                                <asp:TemplateField HeaderText="Folio" SortExpression="Folio">
                                    <HeaderStyle Width="40px" />
                                    <ItemStyle Width="40px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFolio" runat="server" Text='<%# Eval("Folio") %>' CssClass="label_error" ToolTip='<%# Eval("Folio") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UUID" SortExpression="UUID">
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUUID" runat="server" Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("UUID").ToString(), 7, "...") %>'
                                            CssClass="label_negrita" ToolTip='<%# Eval("UUID") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha de Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" ItemStyle-Width="60px" HeaderStyle-Width="60px" />
                                <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Descuentos" HeaderText="Descuentos" SortExpression="Descuentos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoFO" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarFO" />
                        <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarSustitucion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarSustitucion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="columna3x">
            <div class="header_seccion">
                <img src="../Image/Documentacion.png" width="32" height="32" />
                <h2>Servicios Resultantes</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoServ">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoServ" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoServ" runat="server" CssClass="dropdown_100px" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlTamanoServ_SelectedIndexChanged" TabIndex="21"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoPago">Ordenado</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblOrdenadoServ" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenadoServ" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_30pxr">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbExportarServ" runat="server" Text="Exportar" TabIndex="22"
                                OnClick="lkbExportar_Click" CommandName="Servicios"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbExportarFO" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="serviciosSinFacturar" class="grid_seccion_completa_altura_variable">
                <asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvServicios" runat="server" CssClass="gridview" AutoGenerateColumns="false"
                            TabIndex="23" OnSorting="gvServicios_Sorting" OnPageIndexChanging="gvServicios_PageIndexChanging"
                            OnRowDataBound="gvServicios_RowDataBound" Width="100%" AllowPaging="true" AllowSorting="true" ShowFooter="true">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:TemplateField HeaderText="Reemplazar">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbReemplazarServ" runat="server" ImageUrl="~/Image/EntradasSalidas.png" Width="22px" Height="22px"
                                            OnClick="imbReemplazarServ_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdServicio" SortExpression="IdServicio" HeaderText="IdServicio" Visible="false" />
                                <asp:BoundField DataField="IdFacturado" SortExpression="IdFacturado" HeaderText="IdFacturado" Visible="false" />
                                <asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Width="70px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoServ" runat="server" Text='<%# Eval("NoServicio") %>' CssClass="label_negrita" ToolTip='<%# Eval("NoServicio") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                <asp:BoundField DataField="FechaDocumentacion" HeaderText="Fecha de Documentación" SortExpression="FechaDocumentacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Traslados" HeaderText="Traslados" SortExpression="Traslados" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Retenciones" HeaderText="Retenciones" SortExpression="Retenciones" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoServ" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarServ" />
                        <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarSustitucion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarSustitucion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Ventana de Confirmación de Sustitución -->
    <div id="contenedorVentanaConfirmacionSustitucion" class="modal">
        <div id="ventanaConfirmacionSustitucion" class="contenedor_ventana_confirmacion">
            <div class="columna2x" style="padding-bottom:10px;">
                <div class="header_seccion">
                    <img src="../Image/Exclamacion.png" />
                    <asp:UpdatePanel ID="uplblEncabezadoConfirmacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h2><asp:Label ID="lblEncabezadoConfirmacion" runat="server" Text="Se añadira la Factura '{0}' al Servicio '{1}'"></asp:Label></h2>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnAceptarSustitucion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnAceptarSustitucion" runat="server" CssClass="boton"
                                        OnClick="btnAccionSustitucion_Click" CommandName="Aceptar" Text="Aceptar" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelarSustitucion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelarSustitucion" runat="server" CssClass="boton_cancelar"
                                        OnClick="btnAccionSustitucion_Click" CommandName="Cancelar" Text="Cancelar" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
