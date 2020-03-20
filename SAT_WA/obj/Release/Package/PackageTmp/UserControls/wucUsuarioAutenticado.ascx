<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucUsuarioAutenticado.ascx.cs" Inherits="SAT.UserControls.wucUsuarioAutenticado" %>
<link href="../CSS/PaginaMaestra.css" rel="stylesheet" />
<!-- Notificaciones emergentes -->
<script type="text/javascript" src="../Scripts/jquery.noty.packaged.js"></script>
<script type="text/javascript" src="../Scripts/jquery.noty.packaged.min.js"></script>
<nav id="menu_usuario_contenedor">
<ul id="menu_usuario">
<li><asp:Image runat="server" ID="imgUser" ImageUrl="~/Image/User.png" CssClass="estilo_imagen_encabezado"/>
<asp:LinkButton ID="lkbUsuario" runat="server">Nombre</asp:LinkButton>
<ul>
<li><asp:LinkButton ID="lkbInicioSesion" runat="server">Inicio sesión</asp:LinkButton></li>
<li><asp:LinkButton ID="lkbFinSesion" runat="server">Fin sesión</asp:LinkButton></li>
<li><asp:LinkButton  ID="lkbPerfil" runat="server">Perfil</asp:LinkButton></li>
<li ><asp:LinkButton ID="lkbCompania" runat ="server" >Compañia</asp:LinkButton> </li>
</ul>
</li>
<li><a href="#">Control Acceso<asp:Image runat="server" ID="imgSeguridad" ImageUrl="~/Image/Seguridad.png" CssClass="estilo_imagen_encabezado"/></a>
<ul>
<li><asp:LinkButton ID="lkbCerrarSesion" runat="server" OnClick="OnClick_MenuOpciones" CommandName="CerrarSesion">Cerrar Sesión</asp:LinkButton></li>                        
<li><asp:LinkButton ID="lnkCambiaPerfil" runat="server" OnClick="OnClick_MenuOpciones" CommandName="CambiaPerfil">Cambia Perfil</asp:LinkButton></li>
<li><asp:LinkButton ID="lkbCambiaContrasena" runat="server" OnClick="OnClick_MenuOpciones" CommandName="CambiaContrasena">Cambiar Contraseña</asp:LinkButton></li>
</ul>
</li>   
<li>
<div class="controlr">
<asp:UpdatePanel ID="uplkbNotificacionesUsuario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Timer ID="tmrNotificaciones" runat="server" OnTick="tmrNotificaciones_Tick" Interval="900000" Enabled="false"></asp:Timer>
<asp:LinkButton ID="lkbNotificacionesUsuario" runat="server" OnClick="lkbNotificacionesUsuario_Click">    
<asp:Image ID="imgNotificacion" runat="server" AlternateText="Notificaciones" CssClass="estilo_imagen_encabezado" Height="20px" Width="20px" ImageUrl="~/Image/Exclamacion.png" />
<asp:Label ID="lblNotificacionesUsuario" runat="server" Text="Sin Notificaciones" CssClass="label_negrita"></asp:Label>
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</li>            
</ul>
</nav>