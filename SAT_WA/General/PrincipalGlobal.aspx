<%@ Page Title="Principal Configuracion" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalGlobal.aspx.cs" Inherits="SAT.General.PrincipalGlobal" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">    
    <div class="contenedor_imagen_principal_modulo">        
        <div class="vista">  
            <img src="../Image/PrincipalConfiguracion.jpg" />
            <div class="mascara">  
                <h2>Configuracion</h2>  
                <p>Antes que toda otra cosa la configuracion es la clave para el éxito de nuestro negocio.</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/General/CompaniaEmisorReceptor.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Clientes y Proveedores</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho"> 
        <section class="modulo_rojo" runat="server" id="slnkUnidades">
            <asp:LinkButton ID="lnkUnidades" runat="server" CommandName="~/General/Unidad.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Unidades.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Unidades</h2>
                <h3 class="descripcion_modulo">Administre las unidades de su empresa.</h3>
            </asp:LinkButton>
        </section>   
        <section class="modulo_naranja" runat="server" id="slnkOperadores">
            <asp:LinkButton ID="lnkOperadores" runat="server" CommandName="~/General/Operador.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Operadores.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Operadores</h2>
                <h3 class="descripcion_modulo">Administre a los operadores de su empresa.</h3>
            </asp:LinkButton>
        </section>  
        <section class="modulo_azul" runat="server" id="slnkReporteVencimientos">
            <asp:LinkButton ID="lnkReporteVencimientos" runat="server" CommandName="~/General/ReporteVencimientos.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Vencimientos.png" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Reporte Vencimientos</h2>
                <h3 class="descripcion_modulo">Visualice los vencimientos de sus recursos</h3>
            </asp:LinkButton>
        </section>    
        <section class="modulo" runat="server" id="slnkClientesProveedores">
            <asp:LinkButton ID="lnkClientesProveedores" runat="server" CommandName="~/General/CompaniaEmisorReceptor.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Compania.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Clientes y Proveedores</h2>
                <h3 class="descripcion_modulo">De de alta a sus clientes y proveedores.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_amarillo" runat="server" id="slnkUbicaciones">
            <asp:LinkButton ID="lnkUbicaciones" runat="server" CommandName="~/General/Ubicacion.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Ubicacion.jpeg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Ubicaciones</h2>
                <h3 class="descripcion_modulo">Administre todas las ubicaciones de su operación.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_naranja" runat="server" id="slnkKilometraje">
            <asp:LinkButton ID="lnkKilometraje" runat="server" CommandName="~/General/Kilometraje.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Kilometraje.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Kilometraje</h2>
                <h3 class="descripcion_modulo">Administre el kilometraje de su compañia.</h3>
            </asp:LinkButton>
        </section>
       <section class="modulo_azul" runat="server" id="slnkProductos">
            <asp:LinkButton ID="lnkProductos" runat="server" CommandName="~/General/Producto.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Productos.png" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Productos</h2>
                <h3 class="descripcion_modulo">Administre los productos que transporta.</h3>
            </asp:LinkButton>
        </section>
        
        <section class="modulo_rojo" runat="server" id="slnkTipoCobro">
            <asp:LinkButton ID="lnkTipoCobro" runat="server" CommandName="~/Tarifas/TipoCargo.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/TiposCobro.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Conceptos Cobro</h2>
                <h3 class="descripcion_modulo">Configure sus concepto de cobro a cliente.</h3>
            </asp:LinkButton>
        </section> 

        <section class="modulo" runat="server" id="slnkCostoDiesel">
            <asp:LinkButton ID="lnkCostoCombustible" runat="server" CommandName="~/EgresoServicio/PrecioCombustible.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Diesel.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Costo Diesel</h2>
                <h3 class="descripcion_modulo">Administre el costo de combustible de sus unidades.</h3>
            </asp:LinkButton>
        </section> 
        
        <section class="modulo_rojo" runat="server" id="slnkConceptoPago">
            <asp:LinkButton ID="lnkTipoPago" runat="server" CommandName="~/Liquidacion/TipoPago.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/TiposCobro.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Conceptos Pago</h2>
                <h3 class="descripcion_modulo">Configure sus concepto de pago a recursos.</h3>
            </asp:LinkButton>
        </section> 

        <section class="modulo" runat="server" id="slnkTipoCobroRecurrente">
            <asp:LinkButton ID="lnkTipoCobroRecurrente" runat="server" CommandName="~/Liquidacion/TipoCobroRecurrente.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Calendario.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Tipo Cobro Recurrente</h2>
                <h3 class="descripcion_modulo">Configure sus cobros recurrentes.</h3>
            </asp:LinkButton>
        </section> 

       <section class="modulo_azul" runat="server" id="slnkCaseta">
            <asp:LinkButton ID="lnkCaseta" runat="server" CommandName="~/Ruta/Caseta.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Caseta.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Caseta</h2>
                <h3 class="descripcion_modulo">Configure el uso exclusivo de alguna caseta.</h3>
            </asp:LinkButton>
        </section>
          <section class="modulo_azul" runat="server" id="slnkNotificacion">
            <asp:LinkButton ID="lnkNotificacion" runat="server" CommandName="~/General/Notificacion.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Notificacion.png" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Notificación</h2>
                <h3 class="descripcion_modulo">Envió de Correo Electrónico de acuerdo al evento establecido.</h3>
            </asp:LinkButton>
        </section>
    </div>

</div>
</asp:Content>
