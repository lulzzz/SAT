<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRequisicion.ascx.cs" Inherits="SAT.UserControls.wucRequisicion" %>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryWucRequisicion();
        }
    }
    //Declarando Función de Configuración
    function ConfiguraJQueryWucRequisicion() {
        $(document).ready(function () {

            //Cargando Controles de Fechas
            $("#<%=txtFechaSolicitud.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaEntReq.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaEntrega.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });

            //Declarando Función de Validación
            var validaRequisicion = function () {
                var isValid1 = !$("#<%=txtCompaniaEmisora.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtUsuarioSolicitante.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtFechaSolicitud.ClientID%>").validationEngine('validate');
                var isValid5 = !$("#<%=txtFechaEntReq.ClientID%>").validationEngine('validate');
                var isValid6 = !$("#<%=txtFechaEntrega.ClientID%>").validationEngine('validate');
                
                //Devolviendo Resultado
                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6;
            }

            //Añadiendo Validación al Evento Click del Boton
            $("#<%=btnGuardar.ClientID%>").click(validaRequisicion);

            //Declarando Función de Validación
            var validaDetalleRequisicion = function () {
                var isValid1 = !$("#<%=txtProductoDet.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtCantidadDet.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado
                return isValid1 && isValid2;
            }

            //Añadiendo Validación al Evento Click del Boton
            $("#<%=btnGuardarDet.ClientID%>").click(validaDetalleRequisicion);

            //Cargando Catalogo Autocompleta
            $("#<%=txtAlmacen.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
            });
            $("#<%=txtProductoDet.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=31&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
            });
        });
    }

    //Invocando Función de Configuración
    ConfiguraJQueryWucRequisicion();
</script>
<div id="divEncabezado" runat="server" class="contenedor_media_seccion" visible="true">
    <div class="header_seccion">
        <h2>Requisición</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="lblNoAsignacion">No. Requisición</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblNoAsignacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblNoRequisicion" runat="server" Text="Por Asignar"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtCompaniaEmisora">Compania</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtCompaniaEmisora" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCompaniaEmisora" runat="server" CssClass="textbox2x validate[required]" 
                            Enabled="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtUsuarioSolicitante">Solicitante</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtUsuarioSolicitante" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUsuarioSolicitante" runat="server" CssClass="textbox2x validate[required]" Enabled="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtReferencia">Referencia</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
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
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
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
                        <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtAlmacen">Almacen</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtFechaSolicitud">Fecha Solicitud</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFechaSolicitud" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFechaSolicitud" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" Enabled="false"
                            MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtFechaEntrega">Fecha Entrega</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFechaEntrega" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFechaEntrega" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16" Enabled="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtFechaEntReq">F.E.Requerida</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFechaEntReq" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFechaEntReq" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnSolicitar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnSolicitar" runat="server" Text="Solicitar" OnClick="btnSolicitar_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnImprimir" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" CssClass="boton_cancelar" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    
                </Triggers>
            </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnReferencias" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnReferencias" runat="server" Text="Referencias" OnClick="btnReferencias_Click" CssClass="boton_cancelar" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    
                </Triggers>
            </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
<div class="contenedor_media_seccion">
    <div class="header_seccion">
        <h2>Detalles</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="lblNoDetalle">No. Detalle</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblNoDetalle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblNoDetalle" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlEstatusDetalle">Estatus</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlEstatusDetalle" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlEstatusDetalle" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="txtProductoDet">Producto</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtProductoDet" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtProductoDet" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="txtCantidadDet">Cantidad</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtCantidadDet" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCantidadDet" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardarDet" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardarDet" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardarDet_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelarDet" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelarDet" runat="server" Text="Cancelar" OnClick="btnCancelarDet_Click" CssClass="boton_cancelar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenar">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblOrdenar" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalles" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click" Text="Exportar"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                        <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_150px_altura" oncontextmenu="return false">
            <asp:UpdatePanel ID="upgvRequisicionDetalles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvRequisicionDetalles" runat="server" PageSize="25" EmptyDataText="No hay registros" Width="100px"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvRequisicionDetalles_Sorting"
                        OnPageIndexChanging="gvRequisicionDetalles_PageIndexChanging" CssClass="gridview" ShowFooter="True" 
                        OnRowDataBound="gvRequisicionDetalles_RowDataBound" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:BoundField DataField="NoDetalle" HeaderText="No. Detalle" SortExpression="NoDetalle" ItemStyle-HorizontalAlign="Right" Visible="false" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="CodProducto" HeaderText="Código de Producto" SortExpression="CodProducto" ItemStyle-HorizontalAlign="Left"/>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad de Medida" SortExpression="UnidadMedida" ItemStyle-HorizontalAlign="Left"/>                            
                            <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" FooterStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" FooterStyle-HorizontalAlign="Right" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkEditar_Click" Text="Editar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" OnClick="lnkEliminar_Click" Text="Eliminar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarDet" />
                    <asp:AsyncPostBackTrigger ControlID="btnSolicitar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>