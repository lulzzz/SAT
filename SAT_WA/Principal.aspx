<%@ Page Title="Principal" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="SAT.Principal" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="CSS/ControlesUsuario.css" rel="stylesheet" />  
<link href="CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">

    <div class="contenido_forma_principal">
<div class="contenedor_imagen_principal_modulo">        
<div class="vista">  
<img src="Image/TMS2.jpg" />
<div class="mascara">  
<h2>Sistema de Administracion de Flotas</h2>  
<p>Integramos todos sus requerimientos en una sola solución.</p>  
<asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Empiece a utilizarlo ahora</asp:LinkButton>
</div>  
</div>      
</div>        
<div class="contenido_derecho">        
<section class="modulo_azul" runat="server" id="slnkOperacion">
<asp:LinkButton ID="lnkOperacion" runat="server" CommandName="~/Operacion/PrincipalOperacion.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="Image/ModuloOperacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Operación</h2>
<h3 class="descripcion_modulo">Documente, despache y de seguimiento a sus servicios</h3>
</asp:LinkButton>
</section>   
          
<section class="modulo_amarillo" runat="server" id="slnkEvidencias">
<asp:LinkButton ID="lnkEvidencias" runat="server" CommandName="~/ControlEvidencia/PrincipalEvidencias.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/ModuloCobranza.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Evidencias</h2>
<h3 class="descripcion_modulo">Brinde un seguimiento oportuno de sus evidencias</h3>
</asp:LinkButton>
</section>
<section class="modulo_rojo" runat="server" id="slnkLiquidacion">
<asp:LinkButton ID="lnkLiquidacion" runat="server" CommandName="~/Liquidacion/PrincipalLiquidaciones.aspx" OnClick="lnkDireccionaModulo_Click" >            
<img src="Image/ModuloLiquidacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Liquidación</h2>
<h3 class="descripcion_modulo">Liquide los servicios de sus operadores</h3>
</asp:LinkButton>
</section>
<section class="modulo_naranja" runat="server" id="slnkCobro">
<asp:LinkButton ID="lnkCobro" runat="server" CommandName="~/CuentasCobrar/PrincipalCobro.aspx" OnClick="lnkDireccionaModulo_Click" >             
<img src="Image/CuentasCobrar.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Gestión Cobro</h2>
<h3 class="descripcion_modulo">Controle de forma integral su proceso de cobro</h3>
</asp:LinkButton>
</section>
<section class="modulo_azul" runat="server" id="slnkCuentasPagar">
<asp:LinkButton ID="lnkCuentasPagar" runat="server" CommandName="~/CuentasPagar/PrincipalCuentasPagar.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/PrincipalCuentasPagar.png" class="imagen_modulo"/>
<h2 class="titulo_modulo">Cuentas por Pagar</h2>
<h3 class="descripcion_modulo">Gestione el pago a sus proveedores.</h3>
</asp:LinkButton>            
</section>
<section class="modulo"  runat="server" id="slnkAdministrativo">
<asp:LinkButton ID="lnkAdministrativo" runat="server" CommandName="~/Administrativo/PrincipalAdministrativo.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/Administrativo.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Administrativo</h2>
<h3 class="descripcion_modulo">Realice sus procesos administrativos de forma agil.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo" runat="server" id="slnkControlPatio">
<asp:LinkButton ID="lnkControlPatio" runat="server" CommandName="~/ControlPatio/PrincipalControlPatio.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/ModuloControlPatio.gif" class="imagen_modulo"/>
<h2 class="titulo_modulo">Control Patio</h2>
<h3 class="descripcion_modulo">Gestione su patio lógistico de forma rapida y sencilla.</h3>
</asp:LinkButton>            
</section>
<section class="modulo_rojo" runat="server" id="slnkMantenimiento">
<asp:LinkButton ID="lnkMantenimiento" runat="server" CommandName="~/Mantenimiento/PrincipalMantenimiento.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/Mantenimiento.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Mantenimiento</h2>
<h3 class="descripcion_modulo">Brinde mantenimiento a sus unidades</h3>
</asp:LinkButton>            
</section>

<section class="modulo_azul" runat="server" id="snlkAlmacen">
<asp:LinkButton ID="lnkAlmacen" runat="server" CommandName="~/Almacen/PrincipalAlmacen.aspx" OnClick="lnkDireccionaModulo_Click" >           
<img src="Image/almacen.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Almacén</h2>
<h3 class="descripcion_modulo">Gestione las entradas y salidas de producto del almacén</h3>
</asp:LinkButton>            
</section>
       
</div>   
</div>
</asp:Content>
