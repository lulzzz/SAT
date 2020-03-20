<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Bancos.aspx.cs" Inherits="SAT.Administrativo.Bancos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/Controles.css" rel="stylesheet" />
    <link href="../CSS/Forma.css" rel="stylesheet" />
    <!-- Estilos JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
    <!-- Librerias JQuery -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Funcion de Fin de Petición
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                //Invocando Función de Configuración
                ConfiguraJQueryBancos();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryBancos() {
            $(document).ready(function () {

                //Creando Función de Validación de Controles
                var validaBancos = function () {
                    //Configurando Validación de Controles 
                    var isValid1 = !$("#<%=txtClave.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtRazonSocial.ClientID%>").validationEngine('validate')
                    //Devolviendo Valor Obtenido
                    return isValid1 && isValid2 && isValid3;
                };

                //Añadiendo Función de Validación
                $("#<%=btnGuardar.ClientID%>").click(validaBancos);
                //Añadiendo Función de Validación
                $("#<%=lkbGuardar.ClientID%>").click(validaBancos);

            });
        }

        //Invocando Función de Configuración
        ConfiguraJQueryBancos();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Compania.png" />
        <h1>Bancos</h1>
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
                                <asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
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
            <img src="../Image/Depositos.png" />
            <h2>Datos Bancos</h2>
        </div>

        <div class="columna2x">
            <div class="renglon2x">
                <div class ="etiqueta">
                    <label for="txtClave">Clave: </label>
                 </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtClave" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtClave" CssClass="textbox_50px  validate[required, custom[onlyNumberSp]]"  MaxLength="3" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNombre">Nombre: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtNombre" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtNombre" CssClass="textbox2x validate[required]" MaxLength="50" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtRazonSocial">Razón Social</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtRazonSocial" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtRazonSocial" CssClass="textbox2x validate[required]" MaxLength="150" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>
                             <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID ="lkbEditar" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtRFC">RFC</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtRFC" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtRFC" CssClass="textbox2x validate[required]" MaxLength="150" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID ="btnGuardar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID ="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID ="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID ="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID ="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                    <div class="etiqueta"></div>
                     <div class ="control2x">
                         <asp:UpdatePanel runat="server" ID="upchkNacional" UpdateMode="Conditional">
                             <ContentTemplate>
                                <asp:CheckBox runat="server" Text="Nacional" ID="chkNacional" TabIndex="5"/>
                             </ContentTemplate>
                             <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID ="btnCancelar" EventName="Click" />
                                 <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                 <asp:AsyncPostBackTrigger ControlID ="lkbEditar" EventName ="Click" />
                                 <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                                 <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                             </Triggers>
                         </asp:UpdatePanel>
                      </div>  
            </div>
 
             <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnCancelar" cssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="7"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="6" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>
              
            <div class="renglon2x">
                <div class="etiqueta_320px">
                    <asp:UpdatePanel runat="server" ID="uplblError" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblError" CssClass="label_error"> </asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click"/>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
     </div>
 </div>
</asp:Content>
