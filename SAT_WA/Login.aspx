<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SAT.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
  <link href="CSS/Controles.css" rel="stylesheet" />
  <link href="CSS/Login.css" type="text/css" rel="stylesheet" />
  <link href="CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
  <!-- Bibliotecas para Validación de formulario -->
  <script type="text/javascript" src="Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
  <script type="text/javascript" src="Scripts/jquery.validationEngine.js" charset="utf-8"></script>
  <title>Login: Sistema Administracion Transporte</title>
  <link rel="shortcut icon" href="Image/favicon.ico" />
</head>
<body>
  <form id="form1" runat="server" style="width:100%; height:100%;">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
          //Validación Ubicación
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
    <header style="height:5%;">
    </header>
    <div class="contenido" style="height:85%; display: flex; justify-content: space-around; align-items: center; flex-wrap:wrap;">
      <div class="contenedor_imagen" style="margin:0;">
        <div class="imagen_login"></div>
        <div class="mensaje_login">
          <div class="mensaje_login_header">
            <h1>Innovando en la Administración de Transporte</h1>
          </div>
          <div class="mensaje_login_secundario">La administración de tu flota y tus recursos en tus manos.</div>
          <div class="mensaje_login_secundario2">Hacemos de tu negocio un negocio exitoso.</div>
        </div>
      </div>
      <div class="login"  style="margin:0;">
        <div class="login_header">
          <h1>Inicio de sesión</h1>
        </div>
        <div class="login_contenido">
          <asp:UpdatePanel ID="upmtvInicioSesion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:MultiView ID="mtvInicioSesion" runat="server" ActiveViewIndex="0">
                <asp:View ID="vwAtenticaUsuario" runat="server">
                  <asp:Panel ID="pnlAtenticaUsuario" DefaultButton="btnIniciaSesion" runat="server">
                    <div class="columna2x">
                      <div class="renglon2x"></div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto"></div>
                        <div class="etiqueta">
                          <label for="txtUsuario">Usuario</label>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto">
                          <img src="Image/User.png" />
                        </div>
                        <div class="control2x">
                          <asp:TextBox ID="txtUsuario" CssClass="textbox2x validate[required, custom[email]]" runat="server" MaxLength="100" TabIndex="1"></asp:TextBox>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto"></div>
                        <div class="etiqueta">
                          <label for="txtContrasena">Contraseña</label>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto">
                          <img src="Image/Seguridad.png" />
                        </div>
                        <div class="control2x">
                          <asp:TextBox ID="txtContrasena" CssClass="textbox2x validate[required]" TextMode="Password" runat="server" MaxLength="30" TabIndex="2"></asp:TextBox>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto"></div>
                        <div class="control2x" style="width: auto">
                          <asp:UpdatePanel ID="uplblbError" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                              <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="btnIniciaSesion" />
                            </Triggers>
                          </asp:UpdatePanel>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="controlBoton"></div>
                        <div class="controlBoton">
                          <asp:Button ID="btnIniciaSesion" runat="server" Text="Iniciar Sesión" CssClass="boton" TabIndex="3" OnClick="btnIniciaSesion_Click" />
                        </div>
                      </div>
                    </div>
                  </asp:Panel>
                </asp:View>
                <asp:View ID="vwCompania" runat="server">
                  <asp:Panel runat="server" ID="pnlCompania" DefaultButton="btnAceptar">
                    <div class="columna2x">
                      <div class="renglon2x"></div>
                      <div class="renglon2x"></div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto"></div>
                        <div class="control2x">
                          <label for="ddlCompania">Seleccione su compañia</label>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto">
                          <img src="Image/companiaLog.png" />
                        </div>
                        <div class="control2x">
                          <asp:DropDownList ID="ddlCompania" runat="server" CssClass="dropdown2x" TabIndex="4"></asp:DropDownList>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="imagen_caja_texto"></div>
                        <div class="control2x" style="width: auto">
                          <asp:UpdatePanel ID="uplblErrorCompania" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                              <asp:Label ID="lblErrorCompania" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                            </Triggers>
                          </asp:UpdatePanel>
                        </div>
                      </div>
                      <div class="renglon2x">
                        <div class="controlBoton"></div>
                        <div class="controlBoton">
                          <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="boton" TabIndex="5" OnClick="btnAceptar_Click" />
                        </div>
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
        </div>
        <div class="login_footer">
          <div class="columna2x">
            <div class="renglon_footer">
              ¿Olvidaste tu contraseña, para este correo?
              <asp:LinkButton ID="LinkButton1" runat="server"> Enviar correo</asp:LinkButton>
              o <a href="http://www.tectos.com.mx">Registrate</a>
            </div>
          </div>
        </div>
      </div>
    </div>
    <footer style="height:10%;">
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
        <img src="Image/TectosIcono.png" />
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
