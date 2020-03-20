<%@ Page Title="Principal Evidencias" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalEvidencias.aspx.cs" Inherits="SAT.ControlEvidencia.Principal" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
    <div class="contenedor_imagen_principal_modulo">        
        <div class="vista">  
            <img src="../Image/PrincipalEvidencia.jpg" />
            <div class="mascara">  
                <h2>Control de Evidencias</h2>  
                <p>El seguimiento de nuestros evidencias son la base de un buen cobro</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/ControlEvidencia/RecepcionDocumento.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Recibir documentos ahora</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho">       
        <section class="modulo" runat="server" id="slnkRecepcion">
            <asp:LinkButton ID="lnkRecepcion" runat="server" CommandName="~/ControlEvidencia/RecepcionDocumento.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/ModuloCobranza.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Recepcion Evidencia</h2>
                <h3 class="descripcion_modulo">Reciba las evidencias de servicio.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_azul" runat="server" id="slnkArmadoEnvio">
            <asp:LinkButton ID="lnkArmadoEnvio" runat="server" CommandName="~/ControlEvidencia/ArmadoEnvioPaquetes.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/EnvioEvidencias.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Envio Evidencias</h2>
                <h3 class="descripcion_modulo">Direccione sus evidencias a su centro de cobro.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_amarillo" runat="server" id="slnkRecepcionEnvio">
            <asp:LinkButton ID="lnkRecepcionEnvio" runat="server" CommandName="~/ControlEvidencia/RecepcionPaquetes.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/RecepcionPaquete.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Recepcion Envios</h2>
                <h3 class="descripcion_modulo">Reciba los paquetes de evidencias que le han enviado.</h3>
            </asp:LinkButton>
        </section>
         <section class="modulo_rojo"  runat="server" id="slnkVisorDocumentos">
            <asp:LinkButton ID="lnkVisorDocumentos" runat="server" CommandName="~/ControlEvidencia/VisorDocumentos.aspx" OnClick="lnkDireccionaModulo_Click" >       
                <img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Visor Documentos</h2>
                <h3 class="descripcion_modulo">Visualice los estatus global de sus evidencias.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_naranja" runat="server" id="slnkHojaInstruccion">
            <asp:LinkButton ID="lnkHojaInstruccion" runat="server" CommandName="~/ControlEvidencia/HojaInstruccion.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/HojaInstruccion.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Hoja de Instruccion</h2>
                <h3 class="descripcion_modulo">Realice las hojas de instruccion de sus viajes</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo" runat="server" id="slnkVisorHI">
            <asp:LinkButton ID="lnkVisorHI" runat="server" CommandName="~/ControlEvidencia/VisorHojaInstruccion.aspx" OnClick="lnkDireccionaModulo_Click" >       
                <img src="../Image/VisorDocumentos.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Visor Hoja Instruccion</h2>
                <h3 class="descripcion_modulo">Visualice sus hojas de instruccion dadas de alta.</h3>
            </asp:LinkButton>
        </section>
        
        
       
    </div>

</div>
</asp:Content>
