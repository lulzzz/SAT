<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SAT.Externa.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <%--<script type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit"
        async defer></script>--%>
    <!-- Animaciones de entrada y salida de elementos -->
    <link href="../CSS/animate.css" rel="stylesheet" type="text/css" />   
    <!-- Notificaciones emergentes -->
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' ></script>
    <%--librerias de boostrap--%>
    <link href="../Bootstrap/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="../Bootstrap/CSS/style.css" rel="stylesheet" />
    <%--librerias de boostrap--%>
    <script src="../Bootstrap/jss/jquery.js"></script>
    <script src="../Bootstrap/jss/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.1/css/all.css" integrity="sha384-50oBUHEmvpQ+1lW4y57PTFmhCaXp0ML5d60M1M7uH2+nqUivzIebhndOJK28anvf" crossorigin="anonymous"> 
    <title>Login: SatExterna</title>
    <link rel="shortcut icon" href="Image/User.png" />
</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript">
            var onloadCallback = function () {
            }
        </script>
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script>
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(Loading);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Loaded);

            function Loading() {
                $.blockUI({ message: '<h1><img height="25px" width="25px" src="Image/loading.gif" /> Espere por favor...</h1>', fadeIn: 200 });
            }
            function Loaded() {
                $.unblockUI({ fadeOut: 200 });
            }

        </script>
        <script type="text/javascript">
            //Obteniendo instancia actual de la página y añadiendo manejador de evento
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
            function EndRequestHandler(sender, args) {
                if (args.get_error() == undefined) {
                    ConfiguraJQueryLogin();
                }
            }

            //Creando función para configuración de jquery en control de usuario
            function ConfiguraJQueryLogin() {
                $(document).ready(function () {
                    //Validación de Inicio de sesión Usuario Estándar
                    var validacionLogin = function () {
                        var isValidP1 = !$("#<%=txtUsuario.ClientID%>").validationEngine('validate');
                        var isValidP2 = !$("#<%=txtContrasena.ClientID%>").validationEngine('validate');;
                        return isValidP1 && isValidP2;
                    };
                    //Validación de campos requeridos
                    $("#<%=this.btnIniciaSesion.ClientID%>").click(validacionLogin);
                });
            }
            //Invocación Inicial de método de configuración JQuery
            ConfiguraJQueryLogin();
        </script>
                <header>
            <div class="img_logo">
            </div>
            <div class="color_header"></div>
        </header>
          <div class="container">            
            <div class="row">
                <div class="imagen">
                    <div class="col-12 col-md-7 col-xl-2 "> 
                        <img src="../Image/imagen_login.jpg" class="img-responsive .img-fluid. max-width: 100%;y height: auto;" alt="" />
                        <div class="card-body mensaje_login_bootstrap visible-xs">          
                            <h3 class="center">innovando en la Administración de Transporte</h3>
                            <h4 class="mensaje_login_secundario">La administración de tu flota y tus recursos en tus manos.</h4>
                            <h5 class="mensaje_login_secundario2">Hacemos de tu negocio un negocio exitoso.</h5>
                        </div>
                        <div class="card-body mensaje_login_bootstrap hidden-xs" style="height:100%; width:76.7%;">          
                            <h3 class="center">innovando en la Administración de Transporte</h3>
                            <h4 class="mensaje_login_secundario">La administración de tu flota y tus recursos en tus manos.</h4>
                            <h5 class="mensaje_login_secundario2">Hacemos de tu negocio un negocio exitoso.</h5>
                        </div>
                    </div>
                </div>              
                <div class="col-12 col-md-1 col-xl-2  text-center"> </div>
                <div class="col-12 col-md-4 col-xl-2  text-center">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="upmtvInicioSesion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:MultiView ID="mtvInicioSesion" runat="server" ActiveViewIndex="1">
                                    <asp:View ID="vwAutenticaEmpleado" runat="server">
                                        
                                    </asp:View>
                                    <asp:View ID="vwAtenticaUsuario" runat="server">
                                        <asp:Panel ID="pnlAtenticaUsuario" DefaultButton="btnIniciaSesion" runat="server">
                                            <div class="col-md-12 col-xl-2 user-img">
                                                <img src="../Image/usuario.png" />
                                            </div>
                                            <div class="g">
                                                <h3>Inicio de sesión</h3>
                                                <div class="input-group h">
                                                    <span class="input-group-addon"><i class="fa fa-user" aria-hidden="true"></i></span>
                                                    <asp:TextBox ID="txtUsuario" CssClass="textbox2x validate[required] form-control" placeholder="Usuario ó Correo" runat="server" MaxLength="100"></asp:TextBox>                                                  
                                                </div>
                                                <div class="input-group h">
                                                    <span class="input-group-addon"><i class="fa fa-key" aria-hidden="true"></i></span>
                                                    <asp:TextBox ID="txtContrasena" Type="textbox2x validate[required]" CssClass="textbox2x validate[required] form-control" TextMode="Password" runat="server" MaxLength="30" placeholder="Contraseña"></asp:TextBox>
                                                </div>
                                                <asp:Button ID="btnIniciaSesion" runat="server" Text="Iniciar Sesión" CssClass="btn btn-info" CommandName="Usuario" OnClick="btnIniciaSesion_Click" />
                                            </div>
                                        </asp:Panel>
                                    </asp:View>
                                    <asp:View ID="vwCompania" runat="server">
                                        <asp:Panel runat="server" ID="pnlCompania" DefaultButton="btnAceptar">
                                            <div class="col-12 col-md-7 col-xl-2  user-img">
                                                <img src="../Image/factory.png" />
                                            </div>
                                            <div class="g">
                                                <h3 class="head">Seleccione su Compañia</h3>
                                                <div class="input-group h">
                                                    <span class="input-group-addon"><i class="fas fa-city" aria-hidden="true"></i></span>
                                                    <asp:DropDownList ID="ddlCompania" runat="server" CssClass="dropdown2x form-control"></asp:DropDownList>
                                                </div>
                                                <div class="controlBoton"></div>
                                                <div class="controlBoton">
                                                    <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="btn btn-info" OnClick="btnAceptar_Click" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </asp:View>
                                </asp:MultiView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnIniciaSesion" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <%--  footer para recuperar contraseña--%>
                        <div class="login-footer">
                            <div class="renglon-footer">
                                ¿Olvidaste tu contraseña, para este correo?
                                <asp:LinkButton ID="LinkButton1" runat="server">GenerarToken</asp:LinkButton>
                                o <a href="http://www.tectos.com.mx">GenerarToken</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12 col-md-3 col-xl-2">
                    <h3 class=""></h3>
                </div>
            </div>
        </div>

        <footer>
         <section class="footer_section_izquierda">
        <p>This application is managed by ARI TECTOS S.A. de C.V. | 2014 - 2019 TECTOS ©. All rights reserved</p>
        <nav>
          <ul class="footer_menu">
            <li><a href="#">Politica de Privacidad |</a></li>
            <li><a href="#">Condiciones de Uso |</a></li>
            <li><a href="#">Empieza a Usarnos |</a></li>
            <li><a href="#">Soporte Tecnico |</a></li>
          </ul>
        </nav>
      </section>
      <section class="footer_section_derecha">
        <img src="../Image/TectosIcono.png" />
        <nav>
          <ul class="footer_menu">
            <li><a href="#">www.tectos.com.mx |</a></li>
            <li><a href="#">File Bugs |</a></li>
            <li><a href="#">Join us |</a></li>
          </ul>
        </nav>
      </section>
      </footer>
    </form>
</body>
</html>

