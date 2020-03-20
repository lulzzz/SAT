<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalEcosistema.aspx.cs" Inherits="SAT.Operacion.PrincipalEcosistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
<link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
<div class="contenedor_imagen_principal_modulo">        
<div class="vista">  
<img src="../Image/mundo_transporte2.jpg" />
<div class="mascara">  
<h2>Ecosistema</h2>  
<p>Atentos siempre en brindarle el mejor servicio</p>  
<asp:LinkButton ID="lnkPublicaciones" runat="server" CommandName="~/Operacion/Publicaciones.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Publicar Ahora</asp:LinkButton>
</div>  
</div>      
</div>
<div class="contenido_derecho">
<section class="modulo_rojo"  runat="server" id="slnkAsignacion">
<asp:LinkButton ID="lnkAsignacion" runat="server" CommandName="~/Operacion/Publicaciones.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/ModuloOperacionAsignacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Publicaciones</h2>
<h3 class="descripcion_modulo">Administre y planifique las publicaciones de sus Unidades y/ó Servicios.</h3>
</asp:LinkButton>
</section>
</div>
</div>
</asp:Content>
