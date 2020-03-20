<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportadorTarifaCobro.aspx.cs" Inherits="SAT.CuentasPagar.ImportadorTarifaCobro" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../CSS/Forma.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.min.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.structure.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.structure.min.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.theme.css" rel="stylesheet" />
    <link href="../CSS/jquery-ui.theme.min.css" rel="stylesheet" />
    <!-- Animaciones de entrada y salida de elementos -->
    <link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
    <title>Importación de Tarifa de Cobro</title>
    <!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery-ui.min.js") %>' type="text/javascript"></script>
    <!-- Ventana de espera -->
    <script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
    <!-- Arrastre de elementos -->
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
    <!-- Notificaciones emergentes -->
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(Loading);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Loaded);

            function Loading() {
                $.blockUI({ message: '<h2><img src="../Image/loading2.gif" /> Espere por favor...</h2>', fadeIn: 200 });
            }
            function Loaded() {
                $.unblockUI({ fadeOut: 200 });
            }

        </script>
        <script type="text/javascript">

            //Creando función para configuración de jquery en formulario
            function ConfiguraJQueryAutorizacionDeposito() {
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
                    url: "ImportadorTarifaCobro.aspx/LecturaArchivo",
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
            ConfiguraJQueryAutorizacionDeposito();
        </script>
        <div class="contenedor_seccion_completa">
            <div class="header_seccion">
                <img src="../Image/CobrosRecurrentes.png" />
                <h2>Importación de Tarifa de Cobro</h2>
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
                    <b>2.</b> Llene el archivo de importación, manteniendo el formato de cada columna (texto/decimal). <b>Si no se especifica un Monto para 'Flete, Varios, Maniobras, Casetas, Renta' o el apartado 'Retención', será considerado como '0.00' y 'NO', respectivamente.</b>
                    <b>Si se especifica en Monto -1 para 'Flete, Varios, Maniobras, Casetas, Renta' no será considerado la actualización de respectivos conceptos.</b>
                </p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>3.</b> Guarde los cambios, arrastre y suelte el archivo desde su ubicación de almacenamiento (en su PC) hacia el área 'Gris' señalada.</p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>4.</b> Presione el botón <b>'Vista Previa'</b> y valide que el contenido por importar sea el correcto.</p>
                <p class="label" style="margin-bottom: 2px; margin-left: 10px;"><b>5.</b> Presione el botón <b>'Importar'</b> para confirmar los cambios visualizados. <b>Todos los datos 'Identificador' de los conceptos existentes serán borrados.</b></p>
                <p class="label" style="margin-left: 10px;"><b>6.</b> Presione el botón <b>'Finalizar'</b> para cerrar la ventana actual y visualizar los cambios realizados en la ventana principal.</p>
                <br />
            </div>
            <asp:Panel ID="pnlImportacionTarifaCobro" runat="server" DefaultButton="btnImportar">
                <div class="columna2x">
                    <div id="divFile" style="margin-left: 15px; height: 150px; width: 95%; border: dashed 3px; border-color: #a5d16f; background-color: #DDDDDD">
                        <div id="nombreArchivo" style="margin-left: 10%; width: 80%; margin-top: 12%; font-size: large; font-weight: bold; text-align: center; height: auto; color: #808080">Arrastre y suelte sus archivos xls o xlsx</div>
                    </div>
                    <br />
                    <div class="renglon2x">
                        <div class="controlBoton">
                            <asp:Button ID="btnCerrarActualizar" runat="server" Text="Finalizar" CssClass="boton_cancelar" OnClick="btnCerrarActualizar_Click" />
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
                <div class="etiqueta" style="width: auto">
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
                            <asp:AsyncPostBackTrigger ControlID="gvVistaPrevia" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_altura_variable">
                <asp:UpdatePanel ID="upgvVistaPrevia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvVistaPrevia" OnPageIndexChanging="gvVistaPrevia_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvVistaPrevia_RowDataBound" OnSorting="gvVistaPrevia_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
                            CssClass="gridview" Width="100%">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <Columns>
                                <asp:BoundField HeaderText="Id" DataField="Id" SortExpression="Id">
                                    <ItemStyle HorizontalAlign="Right" Width="15px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Referencia" DataField="Referencia" SortExpression="Referencia">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio">
                                    <ItemStyle Width="40px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fin Servicio" SortExpression="FechaFin">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FleteActual" DataFormatString="{0:c}" HeaderText="Flete Actual" SortExpression="FleteActual">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Flete" DataField="Flete" SortExpression="Flete" DataFormatString="{0:c}">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Varios" DataField="Varios" SortExpression="Varios" DataFormatString="{0:c}">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ManiobrasActual" DataFormatString="{0:c}" HeaderText="Maniobras Actual" SortExpression="ManiobrasActual">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Maniobras" DataFormatString="{0:c}" HeaderText="Maniobras" SortExpression="Maniobras">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="MRetencion" HeaderText="Ret." SortExpression="MRetencion">
                                    <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:BoundField DataField="CasetasActual" DataFormatString="{0:c}" HeaderText="Casetas Actual" SortExpression="CasetasActual">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Casetas" DataField="Casetas" SortExpression="Casetas" DataFormatString="{0:c}">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="CRetencion" HeaderText="Ret." SortExpression="CRetencion">
                                    <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:BoundField DataField="RentaActual" DataFormatString="{0:c}" HeaderText="Renta Actual" SortExpression="RentaActual">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Renta" DataField="Renta" SortExpression="Renta" DataFormatString="{0:c}">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="RRetencion" HeaderText="Ret." SortExpression="RRetencion">
                                    <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:BoundField HeaderText="Estadias" DataField="Estadias" SortExpression="Estadias" DataFormatString="{0:c}">
                                    <ItemStyle Width="60px" Wrap="true" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:CheckBoxField DataField="ERetencion" HeaderText="Ret." SortExpression="ERetencion">
                                    <ItemStyle Width="20px" Wrap="true" HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:BoundField DataField="Nota" HeaderText="Nota" SortExpression="Nota" />
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
    </form>
</body>
</html>
