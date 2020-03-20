<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReferenciaViaje.ascx.cs" Inherits="SAT.UserControls.wucReferenciaViaje" %>
<!-- Estilos de los Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
<link href="../CSS/MenuUsuario.css" rel="stylesheet" />
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)      
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryReferenciaViaje();
        }
    }
    //Creando función para configuración de jquery en formulario
    function ConfiguraJQueryReferenciaViaje() {
        $(document).ready(function () {
            //Funcion de Validación de Controles
            var validaReferenciaViaje = function (evt) {
                var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado
                return isValid1;
            };
            //Añadiendo Funcion a Evento Click del Boton
            $("#<%=btnAgregar.ClientID%>").click(validaReferenciaViaje);
        });
    }
    //Ejecutando Función
    ConfiguraJQueryReferenciaViaje();
</script>
<div class="contenido_pestañas_documentacion">
    <div class="contenedor_controles" style="width:550px; font-family:Calibri; font-size:12px;">
        <asp:Panel ID="pnlReferencias" runat="server" DefaultButton="btnAgregar">
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTipoReferencia">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoReferencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoReferencia" runat="server" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvReferencias" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="txtReferencia">Referencia</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox validate[required]" MaxLength="500"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvReferencias" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvReferencias" />
                        <asp:AsyncPostBackTrigger ControlID="btnAgregar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnAgregar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" CssClass="boton" Text="Agregar" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" CssClass="boton_cancelar" Text="Cancelar" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </asp:Panel>
        
    </div>
    <div class="contenedor_controles" style="width:550px; font-family:Calibri; font-size:12px;">
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label>Ordenado:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblCriterio" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvReferencias" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="columna2x">
        <asp:UpdatePanel ID="uplgvReferencias" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <asp:Panel ID="pnlgvReferencias" runat="server" ScrollBars="Auto">
            <asp:GridView ID="gvReferencias" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true"
            Width="97%" PageSize="5" CssClass="gridview" OnPageIndexChanging="gvReferencias_PageIndexChanging"
            OnSorting="gvReferencias_Sorting">
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
                <asp:BoundField DataField="NombreTipo" HeaderText="Tipo Referencia" SortExpression="NombreTipo" />
                <asp:BoundField DataField="ValorReferencia" HeaderText="Referencia" SortExpression="ValorReferencia" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server" OnClick="lnkEditar_Click">Editar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEliminar" runat="server" OnClick="lnkEliminar_Click">Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                                              <asp:TemplateField>
                        <ItemTemplate>
                            <asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                            <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora"  OnClick="OnClick_lkbBitacora"></asp:LinkButton>
                                    </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lkbBitacora" />
                                </Triggers>
                                </asp:UpdatePanel>
                        </ItemTemplate>
                    </asp:TemplateField>
            </Columns>
            </asp:GridView>                    
            </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAgregar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
            </Triggers>
        </asp:UpdatePanel>
        </div>
    </div>
</div>