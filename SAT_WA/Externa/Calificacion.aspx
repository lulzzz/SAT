<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calificacion.aspx.cs" Inherits="SAT.Externa.Calificacion" %>
<%@ Register Src="~/Externa/wucCalificacion.ascx" TagName="wucCalificacion" TagPrefix="tectos" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="../CSS/PaginaMaestra.css" rel="stylesheet" />
<link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
<link href="../CSS/MenuUsuario.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<!-- Estilos de Scripts -->
<link href="../CSS/jquery-ui.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.min.css" rel="stylesheet" />
<!-- Animaciones de entrada y salida de elementos -->
<link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
<link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
<!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
<script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>' type="text/javascript"></script>
<!--<script src="../Scripts/jquery-1.7.1.js" type="text/javascript"></script>-->
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.min.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
<!-- Notificaciones emergentes -->
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
<!-- Estilos JQuery -->
<link  href="../CSS/jquery.datetimepicker.css" rel ="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<title></title>
</head>
<body><form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
<header>
<h1>TECTOS - <b>entregando.com.mx</b></h1>
</header>
<div id="ContenidoForma">
<div class="seccion_controles">
 
<div class="header_seccion">
    
<div class="renglon3x">
   <div class="control2x"><b>
    <asp:Label ID="lblServicio" Font-Size="X-Large" runat="server" ></asp:Label></b>
      </div>
    <div class="control2x">

<asp:UpdatePanel ID="upimbCalificarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>  
<asp:LinkButton ID="imbCalificarServicio"  OnClick="imbCalificarServicio_Click"  TabIndex="15" runat="server" >
<asp:Image runat="server" ID="imgCalificarServicio" ImageUrl="" Width="110px" ImageAlign="Right" />    
    </asp:LinkButton> 
</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
        <asp:AsyncPostBackTrigger ControlID="lkbCerrarContacto" />
    </Triggers>
</asp:UpdatePanel> </div>
   
</div></div>
<div class="columna3x">
<div class="renglon2x">
<div class="etiqueta_80px">
<label  style="font-size:medium" for="lblEstatus">Estatus:</label>
</div>
<div  class="control2x">
<asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblEstatus"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_80px">
<label style="font-size:medium" for="lblNoViaje">No Viaje:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblNoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblNoViaje"   runat="server" Font-Size="Medium"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_80px">
<label  style="font-size:medium" for="lblOrigen">Origen:</label>
</div>
<div  class="control2x">
<asp:UpdatePanel ID="uplblOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrigen"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_80px">
<label  style="font-size:medium" for="lblDestino">Destino:</label>
</div>
<div  class="control2x">
<asp:UpdatePanel ID="uplblDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDestino"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_80px">
<label  style="font-size:medium" for="lblCliente">Cliente:</label>
</div>
<div  class="control2x">
<asp:UpdatePanel ID="uplblCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCliente" runat="server" Font-Size="Medium"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_80px">
<label style="font-size:medium" for="lblPorte">Porte:</label>
</div>
<div  class="control2x">
<asp:UpdatePanel ID="uplblPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblPorte"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_80px">
<label style="font-size:medium" for="lblOperador">Operador:</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOperador" Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<asp:UpdatePanel ID="upimbCalificarOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="imbCalificarOperador"  OnClick="imbCalificarOperador_Click" TabIndex="15" runat="server" >
  <asp:Image runat="server" ID="imgCalificarOperador" ImageUrl="" Width="110px" ImageAlign="Right" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
        <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
        <asp:AsyncPostBackTrigger ControlID="lkbCerrarContacto" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div  class="control2x">
<asp:UpdatePanel ID="uplblTablaServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTablaServicio"  Visible="false"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="imbCalificarServicio" />
    <asp:AsyncPostBackTrigger ControlID="imbCalificarOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div  class="control2x">
<asp:UpdatePanel ID="uplblTablaOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTablaOperador" Visible="false"  Font-Size="Medium" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="imbCalificarServicio" />
    <asp:AsyncPostBackTrigger ControlID="imbCalificarOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/paradas.png" />
<h2>Paradas del Servicio</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewParadas">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewParadas" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewParadas_SelectedIndexChanged"  TabIndex="19" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewParadas">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewParadas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvParadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lkbExportarParadas" runat="server"  OnClick="lkbExportarParadas_Click" Text="Exportar" TabIndex="20" ></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadas" CssClass="gridview"  runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" TabIndex="21"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" DataFormatString="{0:0}" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Llegada" SortExpression="FechaLlegada" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="RazonLlegadaTarde" HeaderText="Razon Llegada Tarde" SortExpression="RazonLlegadaTarde" />
<asp:BoundField DataField="FechaSalida" HeaderText="Fecha Salida" SortExpression="FechaSalida" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewParadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div id="crearCalificar" class="modal">
<div id="contenedorCalificar" class="contenedor_modal_seccion_completa_arriba" style="width:700px;top:0px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCalificacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarContacto" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="Calificacion">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div> 
<asp:UpdatePanel ID="upwucCalificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCalificacion ID="wucCalificacion" OnClickGuardarCalificacionGeneral="wucCalificacion_ClickGuardarCalificacionGeneral" runat="server"    TabIndex="23" /> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbCalificarOperador" />
<asp:AsyncPostBackTrigger ControlID="imbCalificarServicio" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<footer>
<section class="footer_section_izquierda">
<p>This site is managed for ARI TECTOS S.A. de C.V. | © 2015 TECTOS. All rights reserved</p>
<nav>
<ul class="footer_menu">
<li><a href="http://www.tectos.com.mx/AvisoPrivacidad.htm">Politica de Privacidad |</a></li>
<li><a href="#">Condiciones de Uso |</a></li>
<li><a href="#">Empieza a Usarnos |</a></li>
<li><a href="#">Soporte Tecnico |</a></li>
</ul>
</nav>
</section>
<section class="footer_section_derecha">
<asp:Image runat="server" ID="imgIcono" ImageUrl ="~/Image/TectosIcono.png" />                    
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
