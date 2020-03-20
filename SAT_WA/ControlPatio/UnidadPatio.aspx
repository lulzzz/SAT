<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="UnidadPatio.aspx.cs" Inherits="SAT.ControlPatio.UnidadPatio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Hoja de estilo que da formato a la página-->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!--Hoja de estlo para la validación de los controles-->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
    <!--Script que valida el contenido de los controles-->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
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
                ConfiguraJQueryUnidadPatio();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryUnidadPatio() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionContacto = function (evt) {

                    //Validando Controles
                    var isValidP1 = !$("#<%=txtEconomico.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtPlacas.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtColor.ClientID%>").validationEngine('validate');
                    var isValidP4 = !$("#<%=txtTransportista.ClientID%>").validationEngine('validate');

                    return isValidP1 && isValidP2 && isValidP3 && isValidP4;
                };
                //Botón Guardar
                $("#<%=btnGuardar.ClientID%>").click(validacionContacto);
                //Link Guardar
                $("#<%=lkbGuardar.ClientID%>").click(validacionContacto);

                $("#<%=txtTransportista.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=64' });
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryUnidadPatio();
    </script>

    <div id="encabezado_forma">
        <img src="../Image/Tractor.png" />
        <h1>Unidad Patio</h1>
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
                            <li>
                                <asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir"/></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbBajaEliminar" runat="server" Text="Eliminar" ToolTip="Coloca la Unidad Patio en estatus 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
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
            <h2>Datos de la Unidad y su Operador</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEconomico">Número económico</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtEconomico" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEconomico" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="250"></asp:TextBox>
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
                    <label for="txtPlacas">Placas</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtPlacas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPlacas" runat="server" TabIndex="2" CssClass="textbox validate[required]" MaxLength="100"></asp:TextBox>
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
                    <label for="txtColor">Color (Hex)</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtColor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtColor" runat="server" TabIndex="3" CssClass="textbox validate[required]" MaxLength="10"></asp:TextBox>
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
                    <label for="txtTransportista" class="active">Transportista</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTransportista" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTransportista" runat="server" TabIndex="4" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
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

</asp:Content>
