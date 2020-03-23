<%@ Page Title="ImportadorNotasCreditoCxP" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="NotasCreditoCxP.aspx.cs" Inherits="SAT.CuentasPagar.NotasCreditoCxP" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href='<%=ResolveUrl("~/CSS/ControlesUsuario.css") %>' type="text/css" rel="stylesheet" />
    <!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
    <link href='<%=ResolveUrl("~/CSS/jquery.datetimepicker.css") %>' rel="stylesheet" type="text/css" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href='<%=ResolveUrl("~/CSS/jquery.validationEngine.css")%>' rel="stylesheet" type="text/css" />
    <!-- Arrastre de elementos -->
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
    <!--Invoca a los script que que validan los datos de Fecha-->
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.datetimepicker.js")%>'></script>
    <!--Script para validación de controles-->
    <script src='<%=ResolveUrl("~/Scripts/jquery.validationEngine-es.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery.validationEngine.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryAddenda();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryAddenda() {
            $(document).ready(function () {
                if ($.support.fileDrop) {
                    $('#divFile').fileDrop({
                        onFileRead: function (fileCollection) {
                            var index = 0;
                            $.each(fileCollection, function () {
                                //Almacenando archivo en sesión
                                GuardaArchivo(fileCollection[index].data, fileCollection[index].name, fileCollection[index].type);
                                index++;
                            });
                            fileCollection = null;
                        }
                    });
                }
                else {
                    alert('Su navegador actual no soporta la carga de archivos por arrastre.');
                }
            });
        };
        //Función de guardado de archivo en memoria
        function GuardaArchivo(data, name, type) {
            //Definiendo parametros de metodo web de guarado
            var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";
            //Definiendo consumo de método con json
            $.ajax({
                type: "POST",
                url: "NotasCreditoCxP.aspx/LecturaArchivo",
                data: dataValue,
                contentType: 'application/json',
                success: function (response) {
                    //Cambiando nombre de archivo cargado
                    CambiaNombreArchivoCargado(response.d);
                },
                failure: function (response) {
                    //Colocando instrucciones de carga
                    BorraNombreArchivoCargado();
                    alert(response.d);
                },
                error: function (response) {
                    //COlocando instrucciones de carga
                    BorraNombreArchivoCargado();
                    alert(response.d);
                }
            });
        };
        //Función para actualizar el nombre del archivo cargado
        function CambiaNombreArchivoCargado(nombre) {
            $('#nombreArchivo').text(nombre);
        };
        function BorraNombreArchivoCargado() {
            $('#nombreArchivo').text('Arrastre y suelte su archivo *.xml (CFDI)');
        };
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryAddenda();
    </script>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/ArmadoPaquete.png" />
            <h2>Importación de Notas de Crédito CxP</h2>
        </div>
        <div class="contenedor_media_seccion_izquierda">
            <div class="header_seccion">
                <img src="../Image/cfdi_xml.png" />
                <h2>1. Arrastra el archivo XML de la Nota de Crédito.</h2>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="updivFile" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divFile" style="margin-left: 15px; height: 120px; width: 80%; border: dashed 3px; border-color: #a5d16f; background-color: #DDDDDD">
                            <div id="nombreArchivo" style="margin-left: 10%; width: 80%; margin-top: 12%; font-size: large; font-weight: bold; text-align: center; height: auto; color: #808080">Arrastre y suelte su archivo *.xml (CFDI)</div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                        <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:Button ID="btnBorrar" runat="server" Text="Limpiar" CssClass="boton_cancelar" OnClick="btnBorrar_Click" />
            </div>

            <div class="controlBoton">
                <asp:Button ID="btnVistaPrevia" runat="server" Text="Vista Previa >>" CssClass="boton" OnClick="btnVistaPrevia_Click" />
            </div>
        </div>
        <div class="contenedor_media_seccion_derecha">
            <div class="header_seccion">
                <img src="../Image/find.png"/>
                <h2>2. Visualiza los datos contenidos del XML.</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtEmisor">Emisor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtEmisor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEmisor" runat="server" CssClass="textbox2x validate[required]" TabIndex="5" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtReceptor">Receptor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]"
                                    TabIndex="6" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
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
                                <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox2x validate[required]" TabIndex="7" MaxLength="36"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
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
                                <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox2x validate[required]" TabIndex="14"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUUID">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x validate[required, custom[number]]"
                                    TabIndex="12" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="columna">
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtFechaFactura">Fecha de factura</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaFactura" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaFactura" runat="server" CssClass="textbox validate[required, custom[number]]"
                                    TabIndex="13" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtSubtotal">Subtotal</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtSubtotal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSubtotal" runat="server" CssClass="textbox validate[required, custom[number]]"
                                    TabIndex="15" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtDescuento">Descuento</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtDescuento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDescuento" runat="server" CssClass="textbox validate[required, custom[number]]"
                                    TabIndex="16" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtTrasladado">Trasladado</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtTrasladado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTrasladado" runat="server" CssClass="textbox validate[required, custom[number]]"
                                    TabIndex="11" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtRetenido">Retenido</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtRetenido" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtRetenido" runat="server" CssClass="textbox validate[required, custom[number]]"
                                    TabIndex="17" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtTotal">Total</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtTotal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="textbox validate[required]" TabIndex="4"
                                    MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtEstatusSistema">Estatus sistema</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtEstatusSistema" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEstatusSistema" runat="server" CssClass="textbox validate[required]" TabIndex="9" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtEstatusSAT">Estatus SAT</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtEstatusSAT" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEstatusSAT" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="10"
                                    MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtObservacion">Observación</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox validate[required]" TabIndex="18"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                                <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--Sección del GV que muestra las facturas relacionadas del XML con las registradas-->
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/intercambio.png" style="width: 48px; height: 48px" />
            <h2>3. Verifica las facturas relacionadas e importalas al sistema.</h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta" style="width: auto">
                <label for="ddlTamanoVistaPrevia">
                    Mostrar:
                </label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoUnidades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label for="ddlTamanoVistaPrevia"></label>
                        <asp:DropDownList ID="ddlTamanoVistaPrevia" runat="server" OnSelectedIndexChanged="ddlTamanoVistaPrevia_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenarVistaPrevia">Ordenado Por:</label>
            </div>
            <div class="etiqueta" style="width: auto">
                <asp:UpdatePanel ID="uplblOrdenarVistaPrevia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenarVistaPrevia" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplimbValidacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton ID="imbValidacion" runat="server" OnClick="imbValidacion_Click" ImageUrl="~/Image/cfdi_consulta.png" Width="20" Height="20" />
                        <%--<asp:LinkButton ID="lnkValidar" runat="server" OnClick="lnkValidar_Click" ToolTip="Validación SAT">
                                <asp:Image ID="Validar" runat="server" ImageUrl="~/Image/cfdi_consulta.png" Width="20" Height="20" />
                            </asp:LinkButton>--%>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upbtnImportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnImportar" runat="server" Text="Importar" CssClass="boton" OnClick="btnImportar_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvVistaPrevia" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvVistaPrevia" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnSelectedIndexChanged="ddlTamanoVistaPrevia_SelectedIndexChanged"
                        OnPageIndexChanging="gvVistaPrevia_PageIndexChanging" OnRowDataBound="gvVistaPrevia_RowDataBound" OnSorting="gvVistaPrevia_Sorting"
                        ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
                        CssClass="gridview" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkTodos" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" runat="server" HeaderStyle-Width="10px" ItemStyle-Width="10px" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVarios" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" runat="server" HeaderStyle-Width="10px" ItemStyle-Width="10px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Cont" DataField="Cont" SortExpression="Cont" Visible="false">
                                <ItemStyle HorizontalAlign="Right" Width="15px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Id" DataField="Id" SortExpression="Id" Visible="false">
                                <ItemStyle HorizontalAlign="Right" Width="15px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="xml" DataField="xml" SortExpression="xml" Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="nombre" DataField="nombre" SortExpression="nombre" Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="rfcE" DataField="rfcE" SortExpression="rfcE" Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="rfcR" DataField="rfcR" SortExpression="rfcR" Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="TipoComprobante" DataField="TipoComprobante" SortExpression="TipoComprobante" Visible="false">
                                <ItemStyle Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Emisor" HeaderText="Emisor" SortExpression="Emisor">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Receptor" HeaderText="Receptor" SortExpression="Receptor">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie">
                                <ItemStyle Width="15px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio">
                                <ItemStyle Width="15px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="UUID" DataField="UUID" SortExpression="UUID">
                                <ItemStyle Width="150px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaFactura" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SubTotal" DataFormatString="{0:c}" HeaderText="SubTotal" SortExpression="SubTotal">
                                <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Descuento" DataField="Descuento" SortExpression="Descuento" DataFormatString="{0:c}">
                                <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Trasladado" DataField="Trasladado" SortExpression="Trasladado" DataFormatString="{0:c}">
                                <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Retenido" DataFormatString="{0:c}" HeaderText="Retenido" SortExpression="Retenido">
                                <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Total" SortExpression="Total">
                                <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField SortExpression="EstatusSistema" HeaderText="EstatusSistema" ItemStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstatusSistema" Text='<%# Eval("EstatusSistema") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="EstatusSAT" HeaderText="EstatusSAT" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstatusSAT" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Observacion" HeaderText="Observación" ItemStyle-Width="170px">
                                <ItemTemplate>
                                    <asp:Label ID="lblObservacion" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="¿Aceptada?" SortExpression="Aceptada">
                                <HeaderStyle Width="60" />
                                <ItemStyle HorizontalAlign="Center" Width="60" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVariosA" runat="server" AutoPostBack="true" OnCheckedChanged="chkAceptadaE_CheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbEliminar" runat="server" OnClick="imbEliminar_Click" ImageUrl="~/Image/borrar.png" Width="25" Height="25" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvVistaPrevia" />
                    <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                    <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                    <asp:AsyncPostBackTrigger ControlID="btnBorrar" />
                    <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana Confirmación Resultado Consulta SAT -->
    <%--<div id="contenidoResultadoConsultaSATModal" class="modal">
    <div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba">
      <div class="columna2x">
        <div class="header_seccion">
          <asp:UpdatePanel ID="upheaderValidacionSAT" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
              <h3 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h3>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
              <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Emisor</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Receptor</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblRFCReceptor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblRFCReceptor" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">UUID</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblUUID" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblUUID" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Fecha Expedición</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Total Factura</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblTotalFactura" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblTotalFactura" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                <asp:AsyncPostBackTrigger ControlID="imbValidacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon_boton">
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCanelarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnCanelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </div>
  </div>--%>
    <!-- -->
    <div id="contenedorVentanaConfirmacion" class="modal">
        <div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>¿Desea dar de alta al Proveedor </h2>
                <asp:UpdatePanel ID="uplblProveedorFactura" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <h2><b>
                            <asp:Label ID="lblProveedorFactura" runat="server"></asp:Label></b></h2>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />--%>
                    </Triggers>
                </asp:UpdatePanel>
                <h2>?</h2>
            </div>
            <div class="contenedor_media_seccion">
                <div class="columna">
                    <div class="renglon">
                        <div class="etiqueta">
                            <label for="ddlTipoServicio">Tipo de Servicio</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnAceptarOperacion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnAceptarOperacion" runat="server" Text="Aceptar" CssClass="boton"
                                        OnClick="btnAceptarOperacion_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelarOperacion" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelarOperacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
                                        OnClick="btnCancelarOperacion_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                                    <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
