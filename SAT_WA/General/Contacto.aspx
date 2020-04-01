<%@ Page Title="Contacto" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Contacto.aspx.cs" Inherits="SAT.General.Contacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <style>
        .promEmp {
            margin: -3px 0px 0px 0px;
            padding: 0px 0px 0px 550px;
            width: 165px;
            height: 29px;
            float: left;
        }

        .comentariosEmp {
            margin: 0px 0px 0px 0px;
            padding: 2px 0px 0px 0px;
            width: 238px;
            height: 18px;
            float: left;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryContacto();
                ConfiguraJQueryFechaVigenciaToken();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryContacto() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionContacto = function (evt) {

                    //Validando Controles
                    var isValidP1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtEmail.ClientID%>").validationEngine('validate');
                    var isValidP4 = !$("#<%=txtClienteProveedor.ClientID%>").validationEngine('validate');
                    return isValidP1 && isValidP2 && isValidP3 && isValidP4;
                };
                //Botón Guardar
                $("#<%=btnGuardar.ClientID%>").click(validacionContacto);
                //Link Guardar
                $("#<%=lkbGuardar.ClientID%>").click(validacionContacto);
                
                //Catálogo de Clientes y Proveedores
                $("#<%=txtClienteProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=67&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>&param2=' + $("#<%=ddlClienteProveedor.ClientID%>").val()});
            });
        }
        
        function ConfiguraJQueryFechaVigenciaToken() {
            $(document).ready(function () {
                //Validación de fecha de fin de token de vigencia personalizado
                var validacionFechaVigenciaToken = function (evt) {
                    var isValidP5 = !$("#<%=txtFechaVigenciaToken.ClientID%>").validationEngine('validate');

                    return isValidP5;
                };
                
                $("#<%=txtFechaVigenciaToken.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y',
                    timepicker: false
                });

                
                //Validación de campos requeridos
                $("#<%=btnGeneraTokenVigencia.ClientID%>").click(validacionFechaVigenciaToken);

            });
        }
            //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryContacto();
        ConfiguraJQueryFechaVigenciaToken();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Operador.png" />
        <h1>Contacto</h1>
    </div>

    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green">
                        <a href="#" class="fa fa-floppy-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbBajaEliminar" runat="server" Text="Eliminar" ToolTip="Coloca el contacto en estatus 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
                            <!--
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
                            <li>
                                <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>-->
                        </ul>
                    </li>
                    <li class="yellow">
                        <a href="#" class="fa fa-flag-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbTokens" runat="server" Text="Gestionar Tokens" OnClick="lkbElementoMenu_Click" CommandName="Tokens" /></li>
                            <!--<li>
                                <asp:LinkButton ID="lkbContratoIndeterminado" runat="server" Text="C.Definido" OnClick="lkbElementoMenu_Click" CommandName="ContratoIndeterminado" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbImprimirRenuncia" runat="server" Text="Renuncia" OnClick="lkbElementoMenu_Click" CommandName="Renuncia" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbContratoTiempoDefinido" runat="server" Text="C.Indet." OnClick="lkbElementoMenu_Click" CommandName="ContratoTiempoDefinido" />
                            </li>-->
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Requerimiento.png" />
            <h2>Datos del Contacto</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNombre">Nombre</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNombre" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTelefono">Teléfono</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTelefono" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTelefono" runat="server" TabIndex="2" CssClass="textbox validate[required] custom[phone]" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEmail">Correo electrónico</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtEmail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEmail" runat="server" TabIndex="3" CssClass="textbox validate[required] custom[email]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlClienteProveedor">Tipo de Contacto</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlClienteProveedor" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlClienteProveedor" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlClienteProveedor_SelectedIndexChanged"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtClienteProveedor">Companía</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtClienteProveedor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtClienteProveedor" runat="server" TabIndex="3" CssClass="textbox2x validate[required] custom[IdCatalogo]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlClienteProveedor" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlPerfil">Perfil</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlPerfil" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="5" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="6" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="control2x">
                <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="control2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbBajaEliminar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!--VENTANA MODAL DE GESTIÓN DE TOKENS DEL USUARIO-->
    <div id="contenedorGestionTokens" class="modal">
        <div id="ventanaGestionTokens" class="contenedor_ventana_confirmacion_arriba" style="min-width: 800px; width: 800px; height: 510px; padding-bottom: 5px;">
            <div class="columna4x" style="min-width: 800px; width: 800px; height: 510px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModal" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModal" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/Autorizacion.png" style="width: 32px;" />
                    <asp:UpdatePanel ID="uph2EncabezadoGestionTokens" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h2 id="h2EncabezadoGestionTokens" runat="server">Gestión de Tokens</h2>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvGestionTokens" />
                            <asp:AsyncPostBackTrigger ControlID="lkbTokens" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="renglon4x" style="width: 800px; height: 90px;">
                    <div class="etiqueta">
                        <label for="ddlTamañoGridViewGestionTokens">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamañoGridViewGestionTokens" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamañoGridViewGestionTokens" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewGestionTokens_SelectedIndexChanged" TabIndex="8" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblCriterioGridViewGestionTokens">Ordenado por:</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCriterioGridViewGestionTokens" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCriterioGridViewGestionTokens" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvGestionTokens" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upbtnGenerarToken" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGenerarToken" runat="server" CssClass="boton" Text="Generar Token" OnClick="btnGenerarToken_Click" TabIndex="6" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upbtnGenerarTokenVigenciaPersonalizada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGenerarTokenVigenciaPersonalizada" runat="server" CssClass="boton2x" Text="Token con Vigencia" OnClick="btnGenerarTokenVigenciaPersonalizada_Click" TabIndex="6" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div id="GestionTokens" class="grid_seccion_completa_400px_altura" style="width: 777px">
                    <asp:UpdatePanel ID="upgvGestionTokens" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvGestionTokens" CssClass="gridview" OnPageIndexChanging="gvGestionTokens_PageIndexChanging" OnSorting="gvGestionTokens_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                ShowFooter="True" TabIndex="22" OnRowDataBound="gvGestionTokens_RowDataBound"
                                PageSize="5" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="IdContacto" HeaderText="IdContacto" SortExpression="IdContacto" ItemStyle-Width="10px" HeaderStyle-Width="10px" Visible="false" />
                                    <asp:BoundField DataField="IdUsuarioToken" HeaderText="IdUsuarioToken" SortExpression="IdUsuarioToken" ItemStyle-Width="10px" HeaderStyle-Width="10px" Visible="false" />
                                    <asp:BoundField DataField="IdUsuarioSistema" HeaderText="IdUsuarioSistema" SortExpression="IdUsuarioSistema" ItemStyle-Width="10px" HeaderStyle-Width="10px" Visible="false" />
                                    <asp:BoundField DataField="IdUsuarioCompania" HeaderText="IdUsuarioCompania" SortExpression="IdUsuarioCompania" ItemStyle-Width="10px" HeaderStyle-Width="10px" Visible="false" />
                                    <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Token" HeaderText="Token" SortExpression="Token">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaInicioVigencia" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Inicio de vigencia" SortExpression="FechaInicioVigencia" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaFinVigencia" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fin de vigencia" SortExpression="FechaFinVigencia" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RegistroHabilitado" HeaderText="RegistroHabilitado" SortExpression="RegistroHabilitado" ItemStyle-Width="10px" HeaderStyle-Width="10px" Visible="false" />
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbAccionToken1" runat="server" Text="AccionToken1" OnClick="lkbTokens_OnClick" CommandName="AccionToken1"></asp:LinkButton>
                                            <asp:LinkButton ID="lkbFinalizar" runat="server" Text="Finalizar Token" OnClick="lkbTokens_OnClick" CommandName="FinalizarToken"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewGestionTokens" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModal" />
                            <asp:AsyncPostBackTrigger ControlID="lkbTokens" />
                            <asp:AsyncPostBackTrigger ControlID="btnGenerarToken" />
                            <asp:AsyncPostBackTrigger ControlID="gvGestionTokens" />
                            <asp:AsyncPostBackTrigger ControlID="btnGeneraTokenVigencia" />

                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>

    <!--VENTANA MODAL PARA ELEJIR FECHA DE EXPIRACIÖN DEL TOKEN-->
    <div id="contenedorFechaVigenciaToken" class="modal">
        <div id="ventanaFechaVigenciaToken" class="contenedor_ventana_confirmacion_arriba" style="min-width: 600px; width: 600px; padding-bottom: 5px;">
            <div class="columna" style="min-width: 600px; width: 600px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalFechaVigenciaToken" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModalFechaVigenciaToken" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalFechaVigenciaToken_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/Calendario.png" style="width: 32px;" />
                    <h2>Establecer vigencia del Token</h2>
                </div>
                <div class="renglon3x">
                    <div class="etiqueta" style="width: 150px;">
                        <label for="txtFechaVigenciaToken">Fecha de vigencia del Token</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaVigenciaToken" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaVigenciaToken" runat="server" CssClass="textbox validate[required] custom[date]" TabIndex="16"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGeneraTokenVigencia" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton" style="width: 200px">
                        <asp:UpdatePanel ID="upbtnGeneraTokenVigencia" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGeneraTokenVigencia" runat="server" Text="Generar Token" CssClass="boton" Style="width: 150px;"
                                    OnClick="btnGeneraTokenVigencia_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
