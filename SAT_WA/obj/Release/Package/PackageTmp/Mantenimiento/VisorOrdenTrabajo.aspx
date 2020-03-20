<%@ Page Title="VisorOrdenTrabajo" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="VisorOrdenTrabajo.aspx.cs" Inherits="SAT.Mantenimiento.VisorOrdenTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraVisorOrdenTrabajo();
            }
        }
        //Función de Configuración
        function ConfiguraVisorOrdenTrabajo() {
            $(document).ready(function () {
                //Carga el autocomplet de proveedor
                $("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                $("#<%=txtEmpleado.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=44&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                $("#<%=txtUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=28&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                //Cargando Controles de Fecha
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Añadiendo Validación al Evento Click del Boton
                $("#<%=btnBuscarOrdenTrabajo.ClientID%>").click(function () {
                    var isValid1;
                    var isValid2;
                    var isValid3 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtEmpleado.ClientID%>").validationEngine('validate');
                    var isValid6 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
                    var isValid7 = !$("#<%=txtOrdenTrabajo.ClientID%>").validationEngine('validate');
                    var isValid8 = !$("#<%=txtTaller.ClientID%>").validationEngine('validate');
                    var isValid9 = !$("#<%=txtDescUnidad.ClientID%>").validationEngine('validate');







                    //Validando el Control
                    if ($("#<%=chkEntrega.ClientID%>").is(':checked') == true || $("#<%=chkSolicitud.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                        isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid1 = true;
                        isValid2 = true;
                    }

                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8 && isValid9;
                });

                //Añadiendo Encabezado Fijo
                $("#<%=gvOrdenTrabajo.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorVisorOrdenTrabajo").offsetWidth - 15,
                    height: 400,
                    //freezesize: 2
                });

            });
        }

        //Invocando Función de Configuración
        ConfiguraVisorOrdenTrabajo();

        //Declarando Función de Validación de Fechas
        function CompareDates() {
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
            var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

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
        <h1>Visor Orden Trabajo</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOrdenTrabajo">No. Orden Trabajo</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtOrdenTrabajo" runat="server" CssClass="textbox validate[custom[integer]" TabIndex="1" MaxLength="16"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatusOrden">Estatus Orden</label>
                </div>
                <div class="control">
                    <asp:DropDownList ID="ddlEstatusOrden" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEmpleado">Mecánico</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uptxtEmpleado">
                        <ContentTemplate>
                             <asp:TextBox ID="txtEmpleado" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label_negrita">Fecha</label>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkSolicitud" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkSolicitud" runat="server" TabIndex="4" Text="Recepción" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="upchkEntrega" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkEntrega" runat="server" TabIndex="5" Text="Entrega" />
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
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="6" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="7" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta_320px">
                    <asp:UpdatePanel ID="upchkBitUnidadExt" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkBitUnidadExt" runat="server" Text="¿La Unidad es Externa?" TabIndex="8" AutoPostBack="true"
                                OnCheckedChanged="chkBitUnidadExt_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUnidad">No. Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="9"
                                AutoPostBack="true"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDescUnidad">Descripción Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDescUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDescUnidad" runat="server" CssClass="textbox2x validate[custom[onlyLetterSp]]" TabIndex="10" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Propietario Unidad</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="11"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoTaller">Tipo Taller</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoTaller" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoTaller" runat="server" CssClass="dropdown" TabIndex="12" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlTipoTaller_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtProveedor">Propietario Taller</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="13"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoTaller" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTaller">Taller</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTaller" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTaller" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="14"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoTaller" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control"></div>
                <div class="control">
                    <asp:UpdatePanel ID="upbtnBuscarOrdenTrabajo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscarOrdenTrabajo" Text="Buscar" runat="server" OnClick="btnBuscarOrdenTrabajo_Click" TabIndex="15" class="boton" />
                        </ContentTemplate>
                        <Triggers></Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!--GridView-->
    <div class="contenedor_seccion_completa">
        <div class="renglon3x">
            <div class="etiqueta_50px">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="16">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenar">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenar" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvOrdenTrabajo" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click" Text="Exportar" TabIndex="17"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorVisorOrdenTrabajo">
            <asp:UpdatePanel ID="upgvOrdenTrabajo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvOrdenTrabajo" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                        OnPageIndexChanging="gvOrdenTrabajo_PageIndexChanging" OnSorting="gvOrdenTrabajo_Sorting"
                        PageSize="10" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoOrden" HeaderText="No. Orden" SortExpression="NoOrden" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha Recepción" SortExpression="FechaRecepcion " DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="FechaCompromiso" HeaderText="FechaEntrega" SortExpression="FechaCompromiso" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="Entegado" HeaderText="Entegado a" SortExpression="Entegado" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
                            <asp:BoundField DataField="Recibe" HeaderText="Recido por" SortExpression="Recibe" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="PropietarioUnidad" HeaderText="Cliente" SortExpression="PropietarioUnidad" HeaderStyle-Width="150px" ItemStyle-Width="150px"/>
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="Odometro" HeaderText="Odometro" SortExpression="Odometro" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="Combustible" HeaderText="Nivel Combustible" SortExpression="Combustible" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="NombreTaller" HeaderText="Taller" SortExpression="NombreTaller" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                            <asp:BoundField DataField="PropietarioTaller" HeaderText="Proveedor" SortExpression="PropietarioTaller" HeaderStyle-Width="150px" ItemStyle-Width="150px"/>
                            <asp:BoundField DataField="NoSiniestro" HeaderText="No. Siniestro" SortExpression="NoSiniestro" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Actividades y Fallas" SortExpression="ActividadFalla" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbActividadFalla" runat="server" Text='<%# Eval("ActividadFalla") %>' OnClick="lnkModales_Click" CommandName="ActividadFalla">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asignación de Actividades" SortExpression="Asignado" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAsignado" runat="server" Text='<%# Eval("Asignado") %>' OnClick="lnkModales_Click" CommandName="Asignado">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Producto" SortExpression="Producto" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate >
                                    <asp:LinkButton ID="lnkProducto" runat="server" Text='<%# Eval("Producto") %>' OnClick="lnkModales_Click" CommandName="Producto"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscarOrdenTrabajo" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!--GridView-->
    </div>
            <!--Ventana modal productos-->
        <div id="contenedorProducto" class="modal">
            <div id="Producto" class="contenedor_modal_seccion_completa">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplnkCerrar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" CommandName="TerminoOrden">
                            <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <asp:UpdatePanel runat="server" ID="uplblEncabezado" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblEncabezado" CssClass ="label" Font-Size="Large"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvOrdenTrabajo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="grid_seccion_completa_100px_altura">
                    <asp:UpdatePanel ID="upgvGenerico" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvGenerico" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
                                
                                PageSize="10" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>                                   
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrar" />
                            <asp:AsyncPostBackTrigger ControlID="gvOrdenTrabajo" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
        <!--Fin Ventana Modal Producto-->
</asp:Content>
