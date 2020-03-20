<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CuentaBancos.aspx.cs" Inherits="SAT.Administrativo.CuentaBancos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Hoja de estilos que da formato a la página-->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!--Hoja de estilo para la validación de los controles-->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
    <!--Script que valida el contenido de los controles-->
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
                //Invoca a la Funcion ConfiguraJQueryCuentaBancos
                ConfiguraJQueryCuentaBancos();
            }
        }
        //Declara la función que valida los controles de la pagina
        function ConfiguraJQueryCuentaBancos(){
            $(document).ready(function () {
                //Creación  y asignación de la funcion a la variable validaCuentaBancos
                var validaCuentaBancos = function (){
                    //Creación de las variables y asignacion de los controles de la pagina CuentaBancos
                    var isValid1 = !$("#<%=txtNumCuenta.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtRegistro.ClientID%>").validationEngine('validate');
                    //Devuelve un valor a la función
                    return isValid1 && isValid2;               
                }
            //Permite que los eventos de guardar activen la funcion de validación de controles.
            $("#<%=btnGuardar.ClientID%>").click(validaCuentaBancos);
            $("#<%=lkbGuardar.ClientID%>").click(validaCuentaBancos);
            //Crea la variable entidad Deposito y obtiene los valores del ddlTabla
            var tipoDepositante = $("#<%=ddlTabla.ClientID%>").val();
            //Evento de auto completa
            $("#<%=ddlTabla.ClientID%>").change(function(){
                //Limpia el valor del control al cambio de valor del dropdownlist
                $("#<%=txtRegistro.ClientID%>").val('');
                //Obtiene el valor del dropdownlist
                tipoDepositante = this.value;
                //Carga los valores del autocompleta 
                CargaAutoCompleta();
            });
            //Declara la función autoCompleta().
            function CargaAutoCompleta(){
                //Valida cada tipo depositante
                switch(tipoDepositante){
                    //En caso de que sea la primera opcion(compañias)
                    case "25":
                        {
                            //Carga el catalogo de companias
                            $("#<%=txtRegistro.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=24&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                            break;
                        }
                    //en caso de que sea la segunda opcion(operadores)
                    case "76":
                        {
                            //Carga el catalogo de operadores
                            $("#<%=txtRegistro.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=38&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
                            break;
                        }
                }
            }
            //Invoca a la función autoCompleta
            CargaAutoCompleta();
        });
        }
        //Invoca a la funcion CuentaBancos
        ConfiguraJQueryCuentaBancos();

    </script>
    <!--Fin del Script-->
    <div id="encabezado_forma">
        <img src="../Image/bank.png" />
        <h1>Cuenta Banco</h1>
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
        <div class="header_seccion">
            <img src="../Image/Depositos.png" />
            <h2> Descripción de Cuenta:</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlBancos">Banco:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlBancos" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlBancos" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
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
                    <label for="txtNumCuenta">Numero de Cuenta:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtNumCuenta" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtNumCuenta" CssClass="textbox2x validate[required]" TabIndex="2" MaxLength="50"></asp:TextBox>
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
                    <label for="ddlTipoCuenta">Tipo Cuenta: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTipoCuenta" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTipoCuenta" CssClass="dropdown2x" TabIndex="3"></asp:DropDownList>
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
                    <label for="ddlTabla">Tipo Depositante:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="upddlTabla" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList runat="server" ID="ddlTabla" CssClass="dropdown2x" TabIndex="4" AutoPostBack="true"></asp:DropDownList>
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
                    <label for="txtRegistro">Depositante</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtRegistro" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtRegistro" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
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
                           <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="7"/>
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
                           <asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="6" />
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
