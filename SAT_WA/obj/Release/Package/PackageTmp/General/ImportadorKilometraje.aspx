<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ImportadorKilometraje.aspx.cs" Inherits="SAT.General.ImportadorKilometraje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Facturado.css" type="text/css" rel="stylesheet" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryImportadorKms();
            }
        }
        //Creando función para configuración de jquery en formulario
        function ConfiguraJQueryImportadorKms() {
            $(document).ready(function () {

                if ($.support.fileDrop) {

                    $('#divFile').fileDrop({
                        onFileRead: function (fileCollection) {
                            $.each(fileCollection, function () {
                                //Almacenando archivo en sesión
                                GuardaArchivo(fileCollection[0].data, fileCollection[0].name, fileCollection[0].type);
                            });
                        }
                    });
                }
                else {
                    alert('Su navegador actual no soporta la carga de archivos por arrastre :(');
                }
            });
        };

        //Función de guardado de archivo en ememoria
        function GuardaArchivo(data, name, type) {
            //Definiendo parametros de metodo web de guarado
            var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";
            //Definiendo consumo de método con json
            $.ajax({
                type: "POST",
                url: "ImportadorKilometraje.aspx/LecturaArchivo",
                data: dataValue,
                contentType: 'application/json',
                success: function (response) {
                    //Cambiando nombre de archivo cargado
                    CambiaNombreArchivoCargado(response.d);
                    //Indicando carga correcta
                    alert(response.d);
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
            $('#nombreArchivo').text('Arrastre y suelte sus archivos xls o xlsx');
        };

        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryImportadorKms();
    </script>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/EnTransito.png" />
            <h2>Importación de Kilometrajes</h2>
        </div>
        <div class="columna2x">
            <div class="renglon">
                <div class="control2x">
                    <asp:LinkButton ID="lkbDescargaEsquema" runat="server" OnClick="lkbDescargaEsquema_Click">Descargar Formato de Importación</asp:LinkButton>
                </div>
            </div>
            <p class="label_negrita">Instrucciones: </p>
            <br />
            <p class="label" style="margin-left: 10px;"><b>1.</b> Descargue el archivo con el formato de importación.</p>
            <p class="label" style="margin-bottom: 2px; margin-left: 10px;">
                <b>2.</b> Llene el archivo de importación, manteniendo el formato de cada columna (texto/decimal). <b>Si no se especifica unos de los siguientes campos 'Tiempo, KMS(Pago), KMS(Cobro), será considerado como '0.00', respectivamente.</b>
                <b>Si se especifica un origen y destino que no tenga coincidencia o no exista, no será considerado la actualización de respectivos conceptos.</b>
            </p>
            <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>3.</b> Sin el archivo se encuentra con error cuando genere la vista previa no podras importar los kilometrajes.</p>
            <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>4.</b> Guarde los cambios, arrastre y suelte el archivo desde su ubicación de almacenamiento (en su PC) hacia el área 'Gris' señalada.</p>
            <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>5.</b> Presione el botón <b>'Vista Previa'</b> y valide que el contenido por importar sea el correcto.</p>
            <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>6.</b> Presione el botón <b>'Importar'</b> para confirmar los cambios visualizados. <b>Todos los datos 'Identificador' de los conceptos existentes serán borrados.</b></p>
            <br />
        </div>
        <asp:Panel ID="pnlImportacionKilometraje" runat="server" DefaultButton="btnImportar">
            <div class="columna2x">
                <div id="divFile" style="margin-left: 15px; height: 150px; width: 95%; border: dashed 3px; border-color: #a5d16f; background-color: #DDDDDD">
                    <div id="nombreArchivo" style="margin-left: 10%; width: 80%; margin-top: 12%; font-size: large; font-weight: bold; text-align: center; height: auto; color: #808080">Arrastre y suelte sus archivos xls o xlsx</div>
                </div>
                <br />
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:Button ID="btnImportar" runat="server" Text="Importar" CssClass="boton" OnClick="btnImportar_Click" TabIndex="2" />
                    </div>
                    <div class="controlBoton">
                        <asp:Button ID="btnVistaPrevia" runat="server" Text="Vista Previa" CssClass="boton_cancelar" OnClick="btnVistaPrevia_Click" TabIndex="1"/>
                    </div>
                </div>
                <br />
            </div>
        </asp:Panel>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="columna3x">
            <div class="header_seccion">
                <img src="../Image/Pin_Verde.png" width="32" height="32" />
                <h2>Kilometrajes Nuevos</h2>
            </div>
            <div class="renglon3x" style="width: auto">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoVistaPreviaKmsNuevos">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoVistaPreviaKmsNuevos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label for="ddlTamanoVistaPrevia"></label>
                            <asp:DropDownList ID="ddlTamanoVistaPreviaKmsNuevos" runat="server" OnSelectedIndexChanged="ddlTamanoVistaPreviaKmsNuevos_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown_100px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_80px">
                    <label for="lblOrdenarVistaPreviaKmsNuevos">Ordenado</label>
                </div>
                <div class="etiqueta" style="width: auto">
                    <asp:UpdatePanel ID="uplblOrdenarVistaPrevia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenarVistaPreviaKmsNuevos" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvKmsNuevos" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
               <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportarNuevos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarNuevos" runat="server" Text="Exportar" TabIndex="3" CommandName="ExportarNuevos" OnClick="lkbExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarNuevos" />
                    </Triggers>
                </asp:UpdatePanel>
             </div>
            </div>
            <div class="grid_seccion_completa_400px_altura">
                <asp:UpdatePanel ID="upgvKmsNuevos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvKmsNuevos" OnPageIndexChanging="gvKmsNuevos_PageIndexChanging" ShowFooter="True" 
                            OnRowDataBound="gvKmsNuevos_RowDataBound" OnSorting="gvKmsNuevos_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
                            CssClass="gridview" Width="100%" TabIndex="4">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <Columns>
                                <asp:BoundField HeaderText="No" DataField="Id" SortExpression="Id" Visible="false">
                                    <ItemStyle HorizontalAlign="Right" Width="15px" Wrap="true" />
                                </asp:BoundField>                           
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField> 
                                  <asp:BoundField DataField="KMS" DataFormatString="{0:f}" HeaderText="KMS" SortExpression="KMS">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Tiempo"  DataFormatString="{0:f}" HeaderText="Tiempo" SortExpression="Tiempo">
                                    <ItemStyle Width="150px" Wrap="true"  HorizontalAlign="Right"  />
                                </asp:BoundField>
                                  <asp:BoundField DataField="KMSPago"  DataFormatString="{0:f}" HeaderText="KMS Pago" SortExpression="KMSPago">
                                    <ItemStyle Width="150px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KMSCobro" DataFormatString="{0:f}" HeaderText="KMS Cobro" SortExpression="KMSCobro">
                                    <ItemStyle Width="150px" Wrap="true" HorizontalAlign="Right"/>
                                </asp:BoundField>
                                 <asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                               
                            </Columns>
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoVistaPreviaKmsNuevos" />
                        <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="columna3x">
            <div class="header_seccion">
                <img src="../Image/Pin_Azul.png" width="32" height="32" />
                <h2>Kilometrajes Existentes</h2>
            </div>
            <div class="renglon3x" style="width: auto">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoVistaPreviaKmsExistentes">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoVistaPreviaKmsExistentes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label for="ddlTamanoVistaPreviaKmsExistentes"></label>
                            <asp:DropDownList ID="ddlTamanoVistaPreviaKmsExistentes" runat="server" OnSelectedIndexChanged="ddlTamanoVistaPreviaKmsExistentes_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown_100px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_80px">
                    <label for="lblOrdenarVistaPreviaKmsExistentes">Ordenado</label>
                </div>
                <div class="etiqueta" style="width: auto">
                    <asp:UpdatePanel ID="uplblOrdenarVistaPreviaKmsExistentes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenarVistaPreviaKmsExistentes" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvKmsExistentes" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
               <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportarExistentes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarExistentes" runat="server" Text="Exportar" TabIndex="5" CommandName="ExportarExistentes" OnClick="lkbExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarExistentes" />
                    </Triggers>
                </asp:UpdatePanel>
              </div>
            </div>
            <div class="grid_seccion_completa_400px_altura">
                <asp:UpdatePanel ID="upgvKmsExistentes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvKmsExistentes" OnPageIndexChanging="gvKmsExistentes_PageIndexChanging" ShowFooter="True" 
                            OnRowDataBound="gvKmsExistentes_RowDataBound" OnSorting="gvKmsExistentes_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
                            CssClass="gridview" Width="100%" TabIndex="6">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <Columns>
                                <asp:BoundField HeaderText="No" DataField="Id" SortExpression="Id" Visible="false">
                                    <ItemStyle HorizontalAlign="Right" Width="15px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="KMSActual"  DataFormatString="{0:f}" HeaderText="KMS Actual" SortExpression="KMSActual">
                                    <ItemStyle Width="150px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KMS" DataFormatString="{0:f}" HeaderText="KMS Nuevo" SortExpression="KMS">
                                    <ItemStyle Width="150px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                            </Columns>
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoVistaPreviaKmsExistentes" />
                        <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
