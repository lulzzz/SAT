<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPerfilUsuarioAlta.ascx.cs" Inherits="SAT.UserControls.wucPerfilUsuarioAlta" %>
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryPerfilSeguridadUsuario();
        }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryPerfilSeguridadUsuario() {
        $(document).ready(function () {

            //Cargando Lista de Usuarios
            $("#<%=txtUsuario.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=33&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
                appendTo: "<%=this.Contenedor%>"
            });

                //Declarando Función de Validación
                var validaPerfilSeguridadUsuario = function () {
                    var isValid1 = !$("#<%=txtUsuario.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultado Obtenido
                    return isValid1;
                }

                //Añadiendo Validación a Evento del Control
                $("#<%=btnGuardar.ClientID%>").click(validaPerfilSeguridadUsuario);
            });
        }

        //Invocando Función de Configuración
        ConfiguraJQueryPerfilSeguridadUsuario();
</script>
<div class="seccion_controles">
    <div class="columna2x">
        <div class="header_seccion">
            <img src="../Image/Documentados.png" />
            <h2>Perfil de Usuario</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlPerfil">Perfil</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlPerfil" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUsuario">Usuario</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtUsuario" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="gvPerfilesUsuario" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="header_seccion">
            <img src="../Image/ResumenReporte.png" />
            <h2>Perfiles Disponibles</h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" Enabled="false"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvPerfilesUsuario" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click" Enabled="false"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_100px_altura">
            <asp:UpdatePanel ID="upgvPerfilesUsuario" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvPerfilesUsuario" runat="server" AllowPaging="true" AllowSorting="true" Enabled="false"
                        OnPageIndexChanging="gvPerfilesUsuario_PageIndexChanging" OnSorting="gvPerfilesUsuario_Sorting"
                        PageSize="5" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="IdPerfilUsuario" HeaderText="PerfilUsuario" SortExpression="IdPerfilUsuario" Visible="false" />
                            <asp:BoundField DataField="Perfil" HeaderText="Perfil" SortExpression="Perfil" />
                            <asp:BoundField DataField="Activo" HeaderText="¿Activo?" SortExpression="Activo" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>