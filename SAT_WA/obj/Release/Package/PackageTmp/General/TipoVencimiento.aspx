<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="TipoVencimiento.aspx.cs" Inherits="SAT.General.TipoVencimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la pagina-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!--Codigo que permite validar cada uno de los controles del fomulario-->
    <script type="text/javascript">
        //Obtiene la instancia actual de la pagina y añade un manejador de eventos
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Creación de la función que permite finalizar o validar los controles a partir de un error.
        function EndRequestHandler(sender, args) {
            //Valida si el argumento de error no esta definido
            if (args.get_error() == undefined) {
                //Invoca a la Funcion ConfiguraJqueryTipoPago
                ConfiguraJQueryTipoVencimiento();
            }
        }
        //Declara la función que valida los controles de la pagina
        function ConfiguraJQueryTipoVencimiento()
        {
            $(document).ready(function () {
                //Creación  y asignación de la funcion a la variable validaTipoVencimiento
                var validaTipoVencimiento = function () {
                    //Creación de las variables y asignacion de los controles de la pagina TipoVencimiento
                    var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    //Devuelve un valor a la función
                    return isValid1;
                };
                //Permite que los eventos de guardar activen la funcion de validación de controles.
                $("#<%=btnGuardar.ClientID%>").click(validaTipoVencimiento);
                $("#<%=lkbGuardar.ClientID%>").click(validaTipoVencimiento);
            });
        }
        ConfiguraJQueryTipoVencimiento();

    </script>
    <!--Fin del Script-->

    <div id="encabezado_forma">
        <img src="../Image/tiempo.jpg" />
        <h1>Tipo Vencimiento</h1>
    </div>
    <!--Menu Principal-->
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
    <!--Fin menú principal-->

    <div class="seccion_controles">
        <div class=" header_seccion">
            <img src="../Image/calendar.jpg" />
            <h1>Descripción</h1>
        </div>

        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoAplicacion">Aplicación: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTipoAplicacion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTipoAplicacion" CssClass="dropdown" TabIndex="1" ></asp:DropDownList>
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
                <div class="etiqueta">
                    <label for="ddlPrioridad">Prioridad: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlPrioridad" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlPrioridad" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
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
                <div class="etiqueta">
                    <label for="txtDescripcion">Descripcion: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional" >
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" MaxLength="100" TabIndex="3"></asp:TextBox>
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
               <div class="controlBoton">
                   <asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
                       <ContentTemplate>
                           <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="5"/>
                       </ContentTemplate>
                       <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                       </Triggers>
                   </asp:UpdatePanel>
               </div>

               <div class="controlBoton">
                   <asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
                       <ContentTemplate>
                           <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="4" />
                       </ContentTemplate>
                       <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
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
                            <asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
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
