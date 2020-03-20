<%@ Page Title="Importación de Lecturas de Diesel" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ImportacionLecturasDiesel.aspx.cs" Inherits="SAT.Mantenimiento.ImportacionLecturasDiesel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Arrastre de elementos -->
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
                    ConfiguraJQueryImportadorLecturas();
                }
            }
            //Declarando Función de Configuración
            function ConfiguraJQueryImportadorLecturas() {
                $(document).ready(function () {

                    //Validando si el Navegador Soporta 
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
                        alert('Su navegador actual no soporta la carga de archivos por arrastre :(');
                    }

                });
            }
            //Ejecutando Función
            ConfiguraJQueryImportadorLecturas();

            //Función de guardado de archivo en memoria
            function GuardaArchivo(data, name, type) {
                //Definiendo parametros de metodo web de guarado
                var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";
                //Definiendo consumo de método con json
                $.ajax({
                    type: "POST",
                    url: "ImportacionLecturasDiesel.aspx/LecturaArchivo",
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
                $('#nombreArchivo').text('Arrastre y suelte sus archivos *.xml');
            };
        </script>
    <div class="contenedor_seccion_completa">
            <div class="header_seccion">
                <img src="../Image/Calendario.png" width="36" height="36" />
                <h2>Importación de Lecturas de Diesel</h2>
            </div>
            <div class="columna2x">
                <p class="label_negrita">Instrucciones: </p>
                <br />
                <p class="label" style="margin-left: 10px;"><b>1.</b> Obtenga el archivo de importación en formato XML.</p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>2.</b> Arrastre y suelte el archivo desde su ubicación de almacenamiento (en su PC) hacia el área 'Gris' señalada.</p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>3.</b> Presione el botón <b>'Vista Previa'</b> y valide que el contenido por importar sea el correcto.</p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>4.</b> Presione el botón <b>'Importar'</b> para confirmar los cambios visualizados. </p>
                <p class="label" style="margin-left: 10px;"><b>5.</b> Presione el botón <b>'Finalizar'</b> para cerrar la ventana actual y visualizar los cambios realizados en la ventana principal.</p>
                <br />
            </div>
            <asp:Panel ID="pnlImportacionTarifaCobro" runat="server" DefaultButton="btnImportar">
                <div class="columna2x">
                    <div id="divFile" style="margin-left: 15px; height: 150px; width: 95%; border: dashed 3px; border-color: #a5d16f; background-color: #DDDDDD">
                        <div id="nombreArchivo" style="margin-left: 10%; width: 80%; margin-top: 12%; font-size: large; font-weight: bold; text-align: center; height: auto; color: #808080">Arrastre y suelte sus archivos "xml"</div>
                    </div>
                    <br />
                    <div class="renglon2x">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCerrarActualizar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCerrarActualizar" runat="server" Text="Limpiar" CssClass="boton_cancelar" OnClick="btnCerrarActualizar_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnCerrarActualizar" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:Button ID="btnImportar" runat="server" Text="Importar" CssClass="boton" OnClick="btnImportar_Click" />
                        </div>
                        <div class="controlBoton">
                            <asp:Button ID="btnVistaPrevia" runat="server" Text="Vista Previa" CssClass="boton_cancelar" OnClick="btnVistaPrevia_Click" />
                        </div>
                    </div>
                    <br />
                </div>
            </asp:Panel>
            <div class="header_seccion">
                <img src="../Image/TablaResultado.png" />
                <h2>Elementos de Vista Previa</h2>
            </div>

            <div class="renglon4x">
                <div class="etiqueta">
                    <label for="ddlTamanoUnidades">
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
                            <asp:AsyncPostBackTrigger ControlID="gvLecturasImportacion" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_altura_variable">
                <asp:UpdatePanel ID="upgvLecturasImportacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvLecturasImportacion" OnPageIndexChanging="gvLecturasImportacion_PageIndexChanging" ShowFooter="True" 
                            OnRowDataBound="gvLecturasImportacion_RowDataBound" OnSorting="gvLecturasImportacion_Sorting" runat="server" 
                            AutoGenerateColumns="False" AllowPaging="True" ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
                            CssClass="gridview" Width="100%">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id">
                                    <ItemStyle HorizontalAlign="Right" Width="10px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad">
                                    <ItemStyle Width="60px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador">
                                    <ItemStyle Width="120px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaLectura" HeaderText="Fecha de la Lectura" SortExpression="FechaLectura" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                    <ItemStyle Width="80px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="KmsLectura" HeaderText="Kms Lectura" SortExpression="KmsLectura">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="HrsLectura" HeaderText="Horas Lectura" SortExpression="HrsLectura">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LitrosLectura" HeaderText="Litros Lectura" SortExpression="LitrosLectura">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OdometroActual" HeaderText="Odómetro Actual (Kms)" SortExpression="OdometroActual">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EconomiaCombustible" HeaderText="Economía de Combustible (km/L)" SortExpression="EconomiaCombustible">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VelocidadPromedio" HeaderText="Velocidad del Vehículo Promedio (km/h)" SortExpression="VelocidadPromedio">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ConsumoCombustible" HeaderText="Consumo de Combustible (L/h)" SortExpression="ConsumoCombustible">
                                    <ItemStyle Width="80px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" SortExpression="Observaciones">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
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
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoVistaPrevia" />
                        <asp:AsyncPostBackTrigger ControlID="btnVistaPrevia" />
                        <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
