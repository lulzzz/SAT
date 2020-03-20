<%@ Page Title="Principal Cobro" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalCobro.aspx.cs" Inherits="SAT.CuentasCobrar.PrincipalCobro" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
    <div class="contenedor_imagen_principal_modulo">        
        <div class="vista">  
            <img src="../Image/FacturacionE33.jpg" />
            <div class="mascara">  
                <h2>Gestion de cobro</h2>  
                <p>El camino hacia la riqueza depende fundamentalmente de dos palabras: trabajo y ahorro.</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Documentacion/Servicio.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Aplicar pagos ahora</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho">       
        <section class="modulo_rojo" runat="server" id="slnkRevision">
            <asp:LinkButton ID="lnkProcesoRevision" runat="server" CommandName="~/CuentasCobrar/FacturacionProceso.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/formapago.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Procesos Revisión</h2>
                <h3 class="descripcion_modulo">Gestione sus viajes en los distintos procesos de cobro.</h3>
            </asp:LinkButton>
        </section>  
        <section class="modulo_amarillo" runat="server" id="slnkFacturaGlobal">
            <asp:LinkButton ID="lnkFacturaGlobal" runat="server" CommandName="~/CuentasCobrar/FacturaGlobal.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/FacturaGlobal.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Facturación Global</h2>
                <h3 class="descripcion_modulo">Realice las facturas globales que requiera.</h3>
            </asp:LinkButton>
        </section>  
        <section class="modulo_azul" runat="server" id="slnkFacturacionOtros">
            <asp:LinkButton ID="lnkFacturacionOtros" runat="server" CommandName="~/FacturacionElectronica/FacturacionOtros.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/FacturacionOtros.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Facturación Otros</h2>
                <h3 class="descripcion_modulo">Facture por conceptos distintos a transporte.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_naranja" runat="server" id="slnkAplicacionPagos">
            <asp:LinkButton ID="lnkAplicacionPagos" runat="server" CommandName="~/CuentasCobrar/AplicacionPago.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/AplicacionPagos.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Aplicación Pagos</h2>
                <h3 class="descripcion_modulo">Aplique los pagos realizados por su cliente.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo" runat="server" id="slnkTarifa">
            <asp:LinkButton ID="lnkTarifa" runat="server" CommandName="~/Tarifas/Tarifas.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/TarifaCobro.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Tarifas Cobro</h2>
                <h3 class="descripcion_modulo">Ingrese sus tarifas de cobro a clientes.</h3>
            </asp:LinkButton>
        </section>   
        <section class="modulo_rojo" runat="server" id="slnkSaldoDetalle">
            <asp:LinkButton ID="lnkSaldoDetalle" runat="server" CommandName="~/CuentasCobrar/ReporteSaldosDetalle.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Saldos Detalle</h2>
                <h3 class="descripcion_modulo">Revise los saldos a detalle de su empresa.</h3>
            </asp:LinkButton>
        </section>   
        <section class="modulo" runat="server" id="slnkSaldoGlobal">
            <asp:LinkButton ID="lnkSaldoGlobal" runat="server" CommandName="~/CuentasCobrar/ReporteSaldosGlobales.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Saldos Globales</h2>
                <h3 class="descripcion_modulo">Revise sus saldos a nivel cliente.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_azul" runat="server" id="slnkSaldoPeriodo">
            <asp:LinkButton ID="lnkSaldoPeriodo" runat="server" CommandName="~/CuentasCobrar/ReporteSaldosPeriodo.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/VisorModulo3.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Saldos Periodo</h2>
                <h3 class="descripcion_modulo">Revise sus saldos por fecha de vencimiento.</h3>
            </asp:LinkButton>
        </section>          
    </div>
</div>
</asp:Content>
