<%@ Page Title="Principal Administrativo" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalAdministrativo.aspx.cs" Inherits="SAT.Administrativo.PrincipalAdministrativo" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
    <div class="contenedor_imagen_principal_modulo">        
        <div class="vista">  
            <img src="../Image/PrincipalAdministrativo.jpg" />
            <div class="mascara">  
                <h2>Procesos Administrativos</h2>  
                <p>Mejorar es siempre encontrar nuevas oportunidades en los procesos que ya realizamos.</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/EgresoServicio/DepositosPendientes.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Depositos Pendientes</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho">       
        <section class="modulo" runat="server" id="slknDepositoTesoreria">
            <asp:LinkButton ID="lknDepositoTesoreria" runat="server" CommandName="~/EgresoServicio/DepositosPendientes.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/DepositoTesoreria.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Depositos y Pagos</h2>
                <h3 class="descripcion_modulo">Realice sus depositos y pagos pendientes.</h3>
            </asp:LinkButton>
        </section>        
        <section class="modulo_azul" runat="server" id="slnkFichaIngreso">
            <asp:LinkButton ID="lnkFichaIngreso" runat="server" CommandName="~/Administrativo/FichaIngreso.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/FichaIngreso.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Fichas Ingreso</h2>
                <h3 class="descripcion_modulo">Registre sus fichas de ingreso.</h3>
            </asp:LinkButton>
        </section>
       <section class="modulo" runat="server" id="slnkCuentasBancos">
            <asp:LinkButton ID="lnkCuentasBancos" runat="server" CommandName="~/Administrativo/CuentaBancos.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/CuentasBancos.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Cuentas Bancos</h2>
                <h3 class="descripcion_modulo">Administre cuentas de banco de sus entidades.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_amarillo" runat="server" id="slnkBancos">
            <asp:LinkButton ID="lnkBancos" runat="server" CommandName="~/Administrativo/Bancos.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/Bancos.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Bancos</h2>
                <h3 class="descripcion_modulo">Administre los bancos que maneja su empresa</h3>
            </asp:LinkButton>
        </section>
         <section class="modulo" runat="server" id="slnkReporteEgreso">
            <asp:LinkButton ID="lnkReporteEgreso" runat="server" CommandName="~/EgresoServicio/ReporteEgreso.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Reporte Egresos</h2>
                <h3 class="descripcion_modulo">Visualice el egreso de sus cuentas.</h3>
            </asp:LinkButton>
        </section>              
        <section class="modulo_rojo" runat="server" id="slnkReporteFichaIngreso">
            <asp:LinkButton ID="lbkFichaIngreso" runat="server" CommandName="~/Administrativo/ReporteFichaIngreso.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/VisorModulo4.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Reporte FI</h2>
                <h3 class="descripcion_modulo">Visualice el ingreso a sus cuentas.</h3>
            </asp:LinkButton>
        </section>        
    </div>

</div>
</asp:Content>
