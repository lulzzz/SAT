<%@ Page Title="Principal Documentacion" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalDocumentacion.aspx.cs" Inherits="SAT.Documentacion.Principal" MaintainScrollPositionOnPostback="true" %>

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
                <h2>Servicio a Clientes</h2>  
                <p>Nuestros clientes estan en el corazón de nuestra organización</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Documentar Ahora</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho">       
        <section class="modulo">
            <asp:LinkButton ID="lnkDocumentacion" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/ModuloDocumentacion.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Documentación</h2>
                <h3 class="descripcion_modulo">Documente de forma agil y precisa sus servicios.</h3>
            </asp:LinkButton>
        </section> 
         <section class="modulo_azul">
            <asp:LinkButton ID="lnkVisorDocumentacion" runat="server" CommandName="~/Documentacion/VisorDocumentacion.aspx" OnClick="lnkDireccionaModulo_Click" >       
                <img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Reporte Servicios</h2>
                <h3 class="descripcion_modulo">Visualice sus servicios en tiempo real</h3>
            </asp:LinkButton>
        </section>
        
       
    </div>

</div>
</asp:Content>
