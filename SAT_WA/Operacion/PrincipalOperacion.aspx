<%@ Page Title="Principal Operacion" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalOperacion.aspx.cs" Inherits="SAT.Operacion.PrincipalOperacion" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
<link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
<div class="contenedor_imagen_principal_modulo">        
<div class="vista">  
<img src="../Image/PrincipalDocumentacion5.jpg" />
<div class="mascara">  
<h2>Operación</h2>  
<p>Atentos siempre en brindarle el mejor servicio</p>  
<asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Documentar Ahora</asp:LinkButton>
</div>  
</div>      
</div>
<div class="contenido_derecho">       
<section class="modulo" runat="server" id="slnkDocumentacion">
<asp:LinkButton ID="lnkDocumentacion" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/ModuloDocumentacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Documentación</h2>
<h3 class="descripcion_modulo">Documente de forma agil y precisa sus servicios.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo"  runat="server" id="slnkAsignacion">
<asp:LinkButton ID="lnkAsignacion" runat="server" CommandName="~/Operacion/AsignacionRecurso.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/ModuloOperacionAsignacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Asignación</h2>
<h3 class="descripcion_modulo">Administre y planifique sus recursos.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo"  runat="server" id="snlkPlaneacion">
<asp:LinkButton ID="lnkPlaneacion" runat="server" CommandName="~/Operacion/Planeacion.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/Planeacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Planeación</h2>
<h3 class="descripcion_modulo">Planifique Servicios y asigne Recursos.</h3>
</asp:LinkButton>
</section>
<section class="modulo_rojo" runat="server" id="slnkDespacho">
<asp:LinkButton ID="lnkDespacho" runat="server" CommandName="~/Operacion/Despacho.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/DespachoServicio.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Despacho por Servicio</h2>
<h3 class="descripcion_modulo">Brinde seguimiento detallado a sus servicios iniciados.</h3>
</asp:LinkButton>
</section>
<section class="modulo_rojo" runat="server" id="slnkDespachoSimple">
<asp:LinkButton ID="lnkDespachoSimple" runat="server" CommandName="~/Operacion/DespachoSimplificado.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/DespachoUnidad.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Despacho por Unidad</h2>
<h3 class="descripcion_modulo">Optimice el uso de sus unidades.</h3>
</asp:LinkButton>
</section>
<section class="modulo_azul" runat="server" id="slnkAutorizaDeposito">
<asp:LinkButton ID="lnkAutorizaDeposito" runat="server" CommandName="~/EgresoServicio/AutorizacionDeposito.aspx" OnClick="lnkDireccionaModulo_Click" >       
<img src="../Image/AutorizacionDeposito.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Autorice Depositos</h2>
<h3 class="descripcion_modulo">Autorice los depositos asignados a sus servicios.</h3>
</asp:LinkButton>
</section> 
<section class="modulo_naranja" runat="server" id="slnkVisorDocumentacion">
<asp:LinkButton ID="lnkVisorDocumentacion" runat="server" CommandName="~/Documentacion/VisorDocumentacion.aspx" OnClick="lnkDireccionaModulo_Click" >       
<img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Reporte Servicios</h2>
<h3 class="descripcion_modulo">Visualice sus servicios en tiempo real.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkMovimientos">
<asp:LinkButton ID="lnkMovimientos" runat="server" CommandName="~/Operacion/ReporteMovimientos.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Movimientos</h2>
<h3 class="descripcion_modulo">Permite la visualizacion de movimientos generados.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo" runat="server" id="slnkEventos">
<asp:LinkButton ID="lnkEventos" runat="server" CommandName="~/Operacion/ReporteEventos.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo3.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Eventos</h2>
<h3 class="descripcion_modulo">Permite la visualizacion de eventos generados.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkBalanceUnidades">
<asp:LinkButton ID="lnkBalanceUnidades" runat="server" CommandName="~/Operacion/ReporteBalanceUnidades.aspx" OnClick="lnkDireccionaModulo_Click" >       
<img src="../Image/BalanceUnidades.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Balance Unidades</h2>
<h3 class="descripcion_modulo">Visualice el estatus de sus unidades en un solo click.</h3>
</asp:LinkButton>
</section> 
<section class="modulo_rojo" runat="server" id="slnkIngresoUnidad">
<asp:LinkButton ID="lnkIngresoUnidad" runat="server" CommandName="~/Operacion/ReporteIngresoUnidad.aspx" OnClick="lnkDireccionaModulo_Click" >       
<img src="../Image/AplicacionPagos.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Ingreso Unidad</h2>
<h3 class="descripcion_modulo">Visualice el ingreso de sus unidades.</h3>
</asp:LinkButton>
</section>          
<section class="modulo_amarillo" runat="server" id="slnkIndicadoresPeriodo">
<asp:LinkButton ID="lnkIndicadoresPeriodo" runat="server" CommandName="~/FacturacionElectronica/ReporteIngresoServicioPeriodo.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo4.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Indicadores</h2>
<h3 class="descripcion_modulo">Permite la visualizacion de indicadores principales.</h3>
</asp:LinkButton>
</section>
<section class="modulo_azul" runat="server" id="slnkBitacoraEventoServicio">
<asp:LinkButton ID="lnkReporteBitacoraEvento" runat="server" CommandName="~/Operacion/ReporteBitacoraEventos.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Reporte Eventos</h2>
<h3 class="descripcion_modulo">Visualice los eventos de un servicio.</h3>
</asp:LinkButton>
</section>
</div>
</div>
</asp:Content>
