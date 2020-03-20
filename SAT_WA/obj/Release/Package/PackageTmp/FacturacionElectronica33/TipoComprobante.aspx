<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoComprobante.aspx.cs" Inherits="SAT.FacturacionElectronica33.TipoComprobante" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos de controles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Idioma de validación -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <!-- ValidationEngine -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
     <!--Valida las inserciones de datos en los controles-->
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Crear la funcion que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no está definido
            if (args.get_error() == undefined) {
                //Invoca a la funcion ConfiguraTipoComprobante
                ConfiguraTipoComprobante();
            } 
        }
        //Declarando Función de Configuración para validar los controles de la página
        function ConfiguraTipoComprobante() {
            $(document).ready(function () {
                //Crear y asignar la funcion a la variable validaTipoComprobante
                var validaTipoComprobante = function () {
                    //Crear las variables y asignar los controles de la página
                    var isValid1 = !$("#<%=txtClave.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtValorMaximo.ClientID%>").validationEngine('validate');
                    var isValid4 = !$("#<%=txtValorMaximoSueldos.ClientID%>").validationEngine('validate');
                    var isValid5 = !$("#<%=txtValorMaximoOtros.ClientID%>").validationEngine('validate');
                    //Devuelve el valor de la funcion
                    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
                };
                //Permite que los eventos los metodos "Aceptar" activen la funcion de validaciond e controles
                $("#<%=btnGuardar.ClientID%>").click(validaTipoComprobante);
                $("#<%=lkbGuardar.ClientID%>").click(validaTipoComprobante);
            });
        }
        //Invocando Función de Configuración
        ConfiguraTipoComprobante();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Tipo de Comprobante</h1>
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
                                <asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
                            <li>
                                <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
                        </ul>
                    </li>
                    <li class="yellow">
                        <a href="#" class="fa fa-question-circle"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" Height="16px" Width="76px" /></li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbAbrir" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="seccion_controles">
        <div class="header_seccion">
            <%--<img src="../Image/" />--%>
            <h2>Datos de Tipo Comprobante</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtClave">Clave</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtClave" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtClave" runat="server" TabIndex="1" CssClass="textbox_50px validate[required, custom[onlyLetterSp]]" MaxLength="5"></asp:TextBox>
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
                    <label for="txtDescripcion">Descripción</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server" TabIndex="2" CssClass="textbox2x validate[required, custom[onlyLetterSp]]" MaxLength="50"></asp:TextBox>
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
                    <label for="txtValorMaximo">Valor Max</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtValorMaximo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorMaximo" runat="server" TabIndex="3" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
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
            <div class="renglon2x_35h">
                <div class="etiqueta">
                    <label for="txtValorMaximoSueldos">Valor Max sueldos</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtValorMaximoSueldos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorMaximoSueldos" runat="server" TabIndex="4" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
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
                    <label for="txtValorMaximoOtros">Valor Max Otros</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtValorMaximoOtros" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtValorMaximoOtros" runat="server" TabIndex="5" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
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
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" TabIndex="6" OnClick="btnGuardar_Click" Text="Guardar" CssClass="boton" />
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
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" TabIndex="7" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="boton_cancelar" />
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
            </div>
        </div>
    </div>
</asp:Content>
