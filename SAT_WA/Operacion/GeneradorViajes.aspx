<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="GeneradorViajes.aspx.cs" Inherits="SAT.Operacion.GeneradorViajes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
    <link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryGeneradorViajes();
            }
        }
        //Función de Configuración
        function ConfiguraJQueryGeneradorViajes() {
            $(document).ready(function () {

            /** CATALOGOS DE AUTOCOMPLETADO **/
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
                //Ubicación de Carga
                $("#<%=txtOrigen.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtOrigen.ClientID%>").val(ui.item.value);
                    //Causando Actualización del Control
                    __doPostBack('<%= txtOrigen.UniqueID %>', '');
                    }
                });
                //Ubicación de Descarga
                $("#<%=txtDestino.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtDestino.ClientID%>").val(ui.item.value);
                    //Causando Actualización del Control
                    __doPostBack('<%= txtDestino.UniqueID %>', '');
                    }
                });
                //Fechas
                $("#<%=txtCitaCarga.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtCitaDescarga.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Transportista
                $("#<%=txtTransportista.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=16&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    select: function (event, ui) {
                        //Asignando Selección al Valor del Control
                        $("#<%=txtTransportista.ClientID%>").val(ui.item.value);
                        //Causando Actualización del Control
                        __doPostBack('<%= txtTransportista.UniqueID %>', '');
                    }
                });

                //Validando Datos
                $("#<%=btnGenerar.ClientID%>").click(function () {
                    //Validando controles
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtOrigen.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtDestino.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtCitaCarga.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtCitaDescarga.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtTransportista.ClientID%>").validationEngine('validate');
                    var isValid7 = !$("#<%=ddlViajeMaestro.ClientID%>").validationEngine('validate');
                    var isValid8 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8;
                });

                //Validando Datos
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    //Validando controles
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    
                    //Devolviendo Resultado
                    return isValid1;
                });
                //Fechas
                $("#<%=txtCitaCImp.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtCitaDImp.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                //Validando Datos
                $("#<%=btnConfirmarImportacion.ClientID%>").click(function () {
                    //Validando controles
                    var isValid1 = !$("#<%=txtCitaCImp.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtCitaDImp.ClientID%>").validationEngine('validate');
                    //Devolviendo Resultado
                    return isValid1 && isValid2;
                });

                /** CARGA FUNCIONES Y VALIDACIONES DENTRO DEL GRID **/
                $('.viConfirmar').click(function () {
                    //Validando Fechas
                    var isValid1 = !$('.viNoViaje').validationEngine('validate');
                    var isValid2 = !$('.viCitaC').validationEngine('validate');
                    var isValid3 = !$('.viCitaD').validationEngine('validate');
                    var isValid4 = !$('.viUnidad').validationEngine('validate');
                    //Devolviendo Datos
                    return isValid1 && isValid2 && isValid3 && isValid4;
                });

                //Fechas
                $('.viCitaC').datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $('.viCitaD').datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Catalogos de Autocompletado (Referencias)
                //Operadores
                $('.viOperador').autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=62&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
                });
                //Unidades
                $('.viUnidad').autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=63&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
                });
            });
        }
        //Ejecutando Función Inicial
        ConfiguraJQueryGeneradorViajes();

        //Declarando Función de Validación
        function ValidaValorInicial() {
            var ddlVM = document.getElementById("<%=ddlViajeMaestro.ClientID%>");
            var selectedVal = ddlVM.options[ddlVM.selectedIndex].value;
            if (selectedVal == 0)
                return "Debe seleccionar un Viaje Maestro";
        }

        //Declarando Función de Validación de Fechas
        function CompareDates() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtCitaCarga.ClientID%>").val();
            var txtDate2 = $("#<%=txtCitaDescarga.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
                //Mostrando Mensaje de Operación
                return "* La Cita de Carga debe ser inferior a la Cita de Descarga";
        }

        function CompareDatesViajes() {
            //Obteniendo Valores
            var txtDate1 = $('.viCitaC').val();
            var txtDate2 = $('.viCitaD').val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
                //Mostrando Mensaje de Operación
                return "* La Cita de Carga debe ser inferior a la Cita de Descarga";
        }

        function CompareDatesImp() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtCitaCImp.ClientID%>").val();
            var txtDate2 = $("#<%=txtCitaDImp.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
                //Mostrando Mensaje de Operación
                return "* La Cita de Carga debe ser inferior a la Cita de Descarga";
        }
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Documentacion.png" />
        <h1>Importador de Viajes</h1>
    </div>
    <div class="contenedor_seccion_completa" style="margin-right:25px;">
        <div class="columna2x">
            <div class="header_seccion">
                <img src="../Image/PaqueteAgregar.png" width="32" height="32" />
                <h2>Genere sus Viajes Manualmente</h2>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="1"
                                OnTextChanged="txtCliente_TextChanged"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="ddlViajeMaestro">Viaje Maestro</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlViajeMaestro" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlViajeMaestro" runat="server" CssClass="dropdown2x validate[funcCall[ValidaValorInicial[]]" TabIndex="2" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlViajeMaestro_SelectedIndexChanged"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtOrigen">Origen</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOrigen" runat="server" TabIndex="3" Enabled="false"
                                CssClass="textbox2x validate[required, custom[IdCatalogo]]" ></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                            <asp:AsyncPostBackTrigger ControlID="ddlViajeMaestro" />
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtDestino">Destino</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDestino" runat="server" TabIndex="4" Enabled="false"
                                CssClass="textbox2x validate[required, custom[IdCatalogo]]" ></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtCliente" />
                            <asp:AsyncPostBackTrigger ControlID="ddlViajeMaestro" />
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta_155px">
                    <label for="txtCitaCarga">Cita de Carga</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCitaCarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCitaCarga" runat="server" MaxLength="20" TabIndex="5" 
                                CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta_155px">
                    <label for="txtCitaDescarga">Cita de Descarga</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCitaDescarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCitaDescarga" runat="server" MaxLength="20" TabIndex="6" 
                                CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtTransportista">Transportista</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTransportista" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTransportista" runat="server" TabIndex="7"
                                CssClass="textbox2x validate[required, custom[IdCatalogo]]"
                                OnTextChanged="txtTransportista_TextChanged" ></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtCantidad">Cantidad</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCantidad" runat="server" MaxLength="20" TabIndex="8" 
                                CssClass="textbox_100px validate[required, min[1], max[40], custom[onlyNumberSp]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGenerar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGenerar" runat="server" CssClass="boton" CommandName="Generar"
                                 TabIndex="9" Text="Generar" OnClick="btnAccion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnLimpiar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnLimpiar" runat="server" CssClass="boton_cancelar" CommandName="Limpiar"
                                 TabIndex="10" Text="Limpiar" OnClick="btnAccion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton_cancelar" CommandName="Buscar"
                                 TabIndex="10" Text="Buscar" OnClick="btnAccion_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna3x">
            <div class="header_seccion">
                <img src="../Image/ArmadoPaquete.png" width="32" height="32" />
                <h2>Importación Anterior de Viajes</h2>
            </div>
            <p class="label_negrita">Instrucciones: </p>
            <br />
            <p class="label" style="margin-left: 10px;"><b>1.</b> Seleccione el cliente para ver sus importaciones previas.</p>
            <p class="label" style="margin-left: 10px;"><b>2.</b> Busque y seleccione la importación de viajes y gestionala a tu necesidad.</p>
            <div class="renglon3x" style="float:left;">
                <div class="etiqueta">
                    <label for="ddlTamanoIA">Mostrar</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTamanoIA" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoIA" runat="server" CssClass="dropdown" TabIndex="11"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoIA_SelectedIndexChanged" ></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoIA">Ordenado</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblOrdenadoIA" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenadoIA" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50pxr">
                    <asp:UpdatePanel ID="uplkbExportarIA" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbExportarIA" runat="server" Text="Exportar" TabIndex="12"
                                OnClick="lkbExportar_Click" CommandName="ImportacionAnterior" ></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbExportarIA" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvImportacionesAnteriores" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvImportacionesAnteriores" runat="server" TabIndex="13" OnSorting="gvImportacionesAnteriores_Sorting" 
                                CssClass="gridview" OnPageIndexChanging="gvImportacionesAnteriores_PageIndexChanging" AllowPaging="true" AllowSorting="true" 
                                AutoGenerateColumns="false" ShowFooter="true" Width="100%" PageSize="250"
                                OnRowDataBound="gvImportacionesAnteriores_RowDataBound" >
                                <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                    <asp:TemplateField HeaderText="--">
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbSeleccionar" runat="server" ImageUrl="~/Image/Select.png" Width="28" Height="28"
                                                OnClick="imbSeleccionar_Click" ToolTip="Seleccione la Importación que desee copiar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Generacion" SortExpression="FechaGeneracion">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFechaG" runat="server" Text='<%# Eval("FechaGeneracion", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="label_negrita"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CantidadViajes" HeaderText="Cantidad Viajes" SortExpression="CantidadViajes" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgEstatus" runat="server" ImageUrl="~/Image/Circle-IniciadoPendientes.png" Width="28" Height="28" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maestro" SortExpression="Maestro">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaestro" runat="server" Text='<%# Eval("Maestro") %>' CssClass="label_correcto"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cita Carga" SortExpression="CitaCarga">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCitaCarga" runat="server" Text='<%# Eval("CitaCarga", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="label_negrita"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cita Descarga" SortExpression="CitaDescarga">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCitaDescarga" runat="server" Text='<%# Eval("CitaDescarga", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="label_negrita"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoIA" />
                            <asp:AsyncPostBackTrigger ControlID="gvViajesImportados" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarCancelacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarGeneracion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarGeneracion" />
                            <asp:AsyncPostBackTrigger ControlID="btnContinuarImportacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarImportacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCompletarViajes" />
                            <asp:AsyncPostBackTrigger ControlID="btnConfirmarCompletarViajes" />
                        </Triggers>
                    </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="contenedor_seccion_completa" style="margin-right:25px;">
        <div class="header_seccion">
            <img src="../Image/OperacionPatio.png" />
            <h2>Gestión de Viajes</h2>
        </div>
        <div class="renglon4x">
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplkbExportarVI" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarVI" runat="server" Text="Exportar"
                            OnClick="lkbExportar_Click" CommandName="ViajesImportados" ></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarVI" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvViajesImportados" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvViajesImportados" runat="server" TabIndex="14" CssClass="gridview"
                        AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" ShowFooter="true" 
                        Width="100%" PageSize="250" OnRowDataBound="gvViajesImportados_RowDataBound">
                        <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="IdServicio" HeaderText="IdServicio" SortExpression="IdServicio" Visible="false" />
                            <asp:TemplateField HeaderText="Secuencia" SortExpression="Secuencia">
                                <HeaderStyle Width="50" />
                                <ItemStyle Width="50" />
                                <ItemTemplate>
                                    <asp:Label ID="lblSecuencia" runat="server" Text='<%# Eval("Secuencia") %>' CssClass="label"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSecuenciaE" runat="server" Text='<%# Eval("Secuencia") %>' CssClass="label"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
                                <HeaderStyle Width="100" />
                                <ItemStyle Width="100" />
                                <ItemTemplate>
                                    <asp:Label ID="lblNoServicio" runat="server" Text='<%# Eval("NoServicio") %>' CssClass="label_negrita"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblNoServicioE" runat="server" Text='<%# Eval("NoServicio") %>' CssClass="label_negrita"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. Viaje" SortExpression="NoViaje">
                                <HeaderStyle Width="100" />
                                <ItemStyle Width="100" />
                                <ItemTemplate>
                                    <asp:Label ID="lblNoViaje" runat="server" Text='<%# Eval("NoViaje") %>' CssClass="label_error"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNoViajeE" runat="server" CssClass="textbox viNoViaje validate[required, max[500]]" Text='<%# Eval("NoViaje") %>' TabIndex="15"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEstatusImportacion" CommandName='<%# Eval("Estatus") %>' runat="server" ImageUrl="~/Image/TripPending.png" Width="28" Height="28" ToolTip="Pendiente por Registrar" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="imbEstatusImportacionE" runat="server" ImageUrl="~/Image/TripWorking.png" Width="28" Height="28" ToolTip="Registrando" Enabled="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Maestro" SortExpression="Maestro">
                                <ItemTemplate>
                                    <asp:Label ID="lblMaestro" runat="server" Text='<%# Eval("Maestro") %>' CssClass="label_correcto"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblMaestroE" runat="server" Text='<%# Eval("Maestro") %>' CssClass="label_correcto"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CitaCarga" SortExpression="CitaCarga">
                                <HeaderStyle Width="160" />
                                <ItemStyle HorizontalAlign="Center" Width="160" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCitaCarga" runat="server" Text='<%# Eval("CitaCarga", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="label_negrita"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCitaCargaE" runat="server" CssClass="textbox viCitaC validate[required, custom[dateTime24], funcCall[CompareDatesViajes[]]" Text='<%# Eval("CitaCarga", "{0:dd/MM/yyyy HH:mm}") %>' TabIndex="16"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CitaDescarga" SortExpression="CitaDescarga">
                                <HeaderStyle Width="160" />
                                <ItemStyle HorizontalAlign="Center" Width="160" />
                                <ItemTemplate>
                                    <asp:Label ID="lblCitaDescarga" runat="server" Text='<%# Eval("CitaDescarga", "{0:dd/MM/yyyy HH:mm}") %>' CssClass="label_negrita"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCitaDescargaE" runat="server" CssClass="textbox viCitaD validate[required, custom[dateTime24]]" Text='<%# Eval("CitaDescarga", "{0:dd/MM/yyyy HH:mm}") %>' TabIndex="17"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador" SortExpression="Operador">
                                <ItemTemplate>
                                    <asp:Label ID="lblOperador" runat="server" Text='<%# Eval("Operador") %>' CssClass="label"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtOperadorE" runat="server" CssClass="textbox2x viOperador" Text='<%# Eval("Operador") %>' placeHolder="Ingrese su Operador" TabIndex="18"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad" SortExpression="Unidad">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnidad" runat="server" Text='<%# Eval("Unidad") %>' CssClass="label"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUnidadE" runat="server" CssClass="textbox viUnidad validate[required]" Text='<%# Eval("Unidad") %>' TabIndex="19"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="¿Auto-Termino?" SortExpression="AutoTermino">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkTerminoAuto" runat="server" Checked='<%# Eval("AutoTermino").ToString().Equals("SI") ? true : false %>' Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkTerminoAutoE" runat="server" Checked='<%# Eval("AutoTermino").ToString().Equals("SI") ? true : false %>' TabIndex="20" />
                                </EditItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Operación">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEditarServicio" runat="server" Width="28" Height="28" ToolTip="Actualice los Datos de su Servicio"
                                        OnClick="imbActualizarServicio_Click" TabIndex="21" CommandName="Editar" ImageUrl="~/Image/ManageEdit.png" />
                                    <asp:ImageButton ID="imbEliminarFila" runat="server" Width="28" Height="28" ToolTip="Elimine el Servicio"
                                        OnClick="imbActualizarServicio_Click" TabIndex="22" CommandName="Eliminar" ImageUrl="~/Image/ManageCancel.png" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="imbConfirmarServicio" runat="server" Width="28" Height="28" ToolTip="Guarde los Datos de su Servicio"
                                        OnClick="imbActualizarServicio_Click" TabIndex="23" CommandName="Confirmar" ImageUrl="~/Image/ManageSave.png" CssClass="viConfirmar" />
                                    <asp:ImageButton ID="imbCancelarFila" runat="server" Width="28" Height="28" ToolTip="Cancele la Actualización"
                                        OnClick="imbActualizarServicio_Click" TabIndex="24" CommandName="Cancelar" ImageUrl="~/Image/ManageBack.png" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bitacoras">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbBitacoraOperativa" runat="server" Width="28" Height="28" ToolTip="Revise los cambios de sus Servicios"
                                        OnClick="imbBitacoraOperativa_Click" CommandName="Operativa" ImageUrl="~/Image/ManageBinnacle.png" />
                                    <asp:ImageButton ID="imbBitacoraImportacion" runat="server" Width="28" Height="28" ToolTip="Revise los cambios de su Importación"
                                        OnClick="imbBitacoraOperativa_Click" CommandName="Importacion" ImageUrl="~/Image/ManageOperativeBinnacle.png" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Image ID="imgBitacoraOp" runat="server" Width="28" Height="28" ImageUrl="~/Image/ManageCancel.png" ToolTip="No puede consultar los datos en edición" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGenerar" />
                    <asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarLimpieza" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarCancelacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarGeneracion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarGeneracion" />
                    <asp:AsyncPostBackTrigger ControlID="btnContinuarImportacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnConsultarImportacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarImportacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                    <asp:AsyncPostBackTrigger ControlID="btnCompletarViajes" />
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmarCompletarViajes" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <p class="label_negrita">Confirmación General: </p>
                <br />
                <p class="label" style="margin-left: 10px;"><b>Nota 1:</b> Esta acción confirmará todos los viajes restantes.</p>
                <p class="label" style="margin-left: 10px;"><b>Nota 2:</b> Esta acción no se puede deshacer.</p>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCompletarViajes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCompletarViajes" runat="server" CssClass="boton2x" CommandName="CompletarViajes"
                                TabIndex="31" OnClick="btnCompletarViajes_Click" Text="Confirmar Todos" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana encargada de Confirmación de Limpieza -->
    <div id="confirmacionVentanaConfirmacionLimpieza" class="modal">
        <div id="ventanaConfirmacionLimpieza" class="contenedor_ventana_confirmacion">
            <div class="columna2x">
                <div class="header_seccion">
                    <img src="../Image/Exclamacion.png" />
                    <h2>Los Datos previos se limpiarán, ¿Desea Continuar?</h2>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarLimpieza" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarLimpieza" runat="server" CssClass="boton" TabIndex="22"
                                    OnClick="btnAccion_Click" CommandName="ConfirmarLimpieza" Text="Confirmar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarLimpieza" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarLimpieza" runat="server" CssClass="boton_cancelar" TabIndex="21"
                                    OnClick="btnAccion_Click" CommandName="CerrarLimpieza" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana encargada de Confirmación de Cancelación -->
    <div id="confirmacionVentanaConfirmacionCancelacion" class="modal">
        <div id="ventanaConfirmacionCancelacion" class="contenedor_ventana_confirmacion">
            <div class="columna2x">
                <div class="header_seccion">
                    <img src="../Image/ExclamacionRoja.png" />
                    <h2>Esta apunto de Eliminar un Viaje, ¿Desea Continuar?</h2>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarCancelacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarCancelacion" runat="server" CssClass="boton" TabIndex="23"
                                    OnClick="btnAccion_Click" CommandName="ConfirmarCancelacion" Text="Confirmar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarCancelacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarCancelacion" runat="server" CssClass="boton_cancelar" TabIndex="24"
                                    OnClick="btnAccion_Click" CommandName="CerrarCancelacion" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana encargada de Confirmación de Generación -->
    <div id="confirmacionVentanaConfirmacionGeneracion" class="modal">
        <div id="ventanaConfirmacionGeneracion" class="contenedor_ventana_confirmacion">
            <div class="columna3x">
                <div class="header_seccion">
                    <img src="../Image/ExclamacionRoja.png" />
                    <h2>Tiene Viajes pre-cargados pendientes, ¿Desea Continuar?</h2>
                </div>
                <div class="renglon3x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarGeneracion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarGeneracion" runat="server" CssClass="boton" TabIndex="25"
                                    OnClick="btnAccion_Click" CommandName="ConfirmarGeneracion" Text="Confirmar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarGeneracion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarGeneracion" runat="server" CssClass="boton_cancelar" TabIndex="26"
                                    OnClick="btnAccion_Click" CommandName="CerrarGeneracion" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana encargada de Confirmación de Importación -->
    <div id="confirmacionVentanaGestionImportacion" class="modal">
        <div id="ventanaGestionImportacion" class="contenedor_ventana_confirmacion" style="padding-right:20px; padding-bottom:20px;">
            <div class="columna4x">
                <div class="header_seccion">
                    <asp:UpdatePanel ID="uplblMensajeImportacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Image ID="imgImportacion" runat="server" ImageUrl="~/Image/Exclamacion.png" />
                            <h2><asp:Label ID="lblMensajeImportacion" runat="server" Text="Gestión Importación"></asp:Label></h2>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon4x" style="float:left;">
                    <div class="etiqueta">
                        <label for="txtCitaCImp">Cita de Carga</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCitaCImp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCitaCImp" runat="server" MaxLength="20" TabIndex="27" 
                                    CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDatesImp[]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_155px">
                        <label for="txtCitaDImp">Cita de Descarga</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCitaDImp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCitaDImp" runat="server" MaxLength="20" TabIndex="28" 
                                    CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon4x">                    
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarImportacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarImportacion" runat="server" CssClass="boton_cancelar" TabIndex="30"
                                    OnClick="btnImportacion_Click" CommandName="ConfirmarImportacion" Text="Nuevo" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <asp:UpdatePanel ID="uppnlContinuarImp" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlContinuarImp" runat="server">
                                <div class="controlBoton">
                                    <asp:Button ID="btnContinuarImportacion" runat="server" CssClass="boton" TabIndex="31"
                                        OnClick="btnImportacion_Click" CommandName="ContinuarImportacion" Text="Continuar" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarImportacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarImportacion" runat="server" CssClass="boton_cancelar" TabIndex="32"
                                    OnClick="btnImportacion_Click" CommandName="CerrarImportacion" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConsulta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConsultarImportacion" runat="server" CssClass="boton" TabIndex="29"
                                    OnClick="btnImportacion_Click" CommandName="ConsultarImportacion" Text="Consultar" />
                            </ContentTemplate>
                            <Triggers>    
                                <asp:AsyncPostBackTrigger ControlID="gvImportacionesAnteriores" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana encargada de Confirmación de Generación Total -->
    <div id="confirmacionVentanaCompletarViajes" class="modal">
        <div id="ventanaCompletarViajes" class="contenedor_ventana_confirmacion" style="padding-right:20px; padding-bottom:20px;">
            <div class="columna2x">
                <div class="header_seccion">
                    <asp:UpdatePanel ID="uplblConfirmacionViajes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <img src="../Image/Exclamacion.png" />
                            <h2><asp:Label ID="lblConfirmacionViajes" runat="server" Text="Gestión Importación"></asp:Label></h2>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCompletarViajes" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarCompletarViajes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarCompletarViajes" runat="server" CssClass="boton" TabIndex="33"
                                    OnClick="btnCompletarViajes_Click" CommandName="ConfirmarCompletarViajes" Text="Confirmar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCompletarViajes" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCerrarCompletarViajes" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCerrarCompletarViajes" runat="server" CssClass="boton_cancelar" TabIndex="34"
                                    OnClick="btnCompletarViajes_Click" CommandName="CerrarCompletarViajes" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCompletarViajes" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
