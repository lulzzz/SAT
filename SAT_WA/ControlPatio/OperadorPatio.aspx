<%@ Page Title="OperadorPatio" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="OperadorPatio.aspx.cs" Inherits="SAT.ControlPatio.OperadorPatio" %>

<%@ Register Src="~/UserControls/wucPerfilUsuarioAlta.ascx" TagName="wucPerfilUsuarioAlta" TagPrefix="tectos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Estilos Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
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
                ConfiguraJQueryOperador();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryOperador() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionOperador= function (evt) {

                    //Validando Controles
                    var isValidP1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtNombreCorto.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtIdNombre.ClientID%>").validationEngine('validate');
                    <%--var isValidP4 = !$("#<%=txtIdPatio.ClientID%>").validationEngine('validate');--%>
                    

                    return isValidP1 && isValidP2 && isValidP3;
                };
                //Botón Guardar
                $("#<%=btnGuardar.ClientID%>").click(validacionOperador);
                //Link Guardar
                $("#<%=lkbGuardar.ClientID%>").click(validacionOperador);

                $("#<%=txtIdNombre.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=33&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryOperador();
    </script>

    <div id="encabezado_forma">
        <img src="../Image/Operador.png" />
        <h1>Operador</h1>
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
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" ToolTip="Coloca el contacto en estatus 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
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
            <h2>Datos del Operador</h2>
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
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNombreCorto">Nombre Corto</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNombreCorto" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNombreCorto" runat="server" TabIndex="2" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtIdNombre">Usuario</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtIdNombre" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdNombre" runat="server" TabIndex="2" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            <%--<div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtIdPatio">Patio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtIdPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdPatio" runat="server" TabIndex="3" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>--%>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlPatio">Patio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlPatio" runat="server" TabIndex="1" CssClass="dropdown2x"
                                   AutoPostBack="true"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkactivo" runat="server" Text="Activo" TabIndex="17" AutoPostBack="true"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>



            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="4" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="5" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
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
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
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
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
    </div>

</asp:Content>
