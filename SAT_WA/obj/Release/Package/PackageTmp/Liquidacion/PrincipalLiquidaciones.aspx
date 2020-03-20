<%@ Page Title="Módulo Liquidaciones" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalLiquidaciones.aspx.cs" Inherits="SAT.Liquidacion.PrincipalLiquidaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
<link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div class="contenido_forma_principal">
<div class="contenedor_imagen_principal_modulo">        
<div class="vista">  
<img src="../Image/PrincipalLiquidacion.jpg" />
<div class="mascara">  
<h2>Liquidacion</h2>  
<p>Genere la liquidación de sus operadores de forma agil y precisa.</p>
<asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Liquidacion/Liquidacion.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Realizar Liquidación</asp:LinkButton>
                
</div>  
</div>      
</div>
<div class="contenido_derecho"> 
<section class="modulo_amarillo" runat="server" id="slnkTarifaPago">            
<asp:LinkButton ID="lnkTarifaPago" runat="server" CommandName="~/Tarifas/TarifasPago.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/TarifaCobro.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Tarifas Pago</h2>
<h3 class="descripcion_modulo">Configure la forma en que paga a su recurso.</h3>
</asp:LinkButton>
</section>      
<section class="modulo" runat="server" id="slnkLiquidacion">            
<asp:LinkButton ID="lnkLiquidacion" runat="server" CommandName="~/Liquidacion/Liquidacion.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/PagoOperador.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Liquidaciones</h2>
<h3 class="descripcion_modulo">Liquide a sus operadores facil y rapido.</h3>
</asp:LinkButton>
</section>
<section class="modulo_azul" runat="server" id="slnkCobroRecurrente">            
<asp:LinkButton ID="lnkCobroRecurrente" runat="server" CommandName="~/Liquidacion/CobroRecurrente.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/CobrosRecurrentes.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Cobro Recurrente</h2>
<h3 class="descripcion_modulo">Controle sus cobros recurrentes.</h3>
</asp:LinkButton>
</section>
<section class="modulo_rojo" runat="server" id="slnkReporteLiquidacion">            
<asp:LinkButton ID="lnkReporteLiquidacion" runat="server" CommandName="~/Liquidacion/ReporteLiquidacion.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Reporte Liquidaciones</h2>
<h3 class="descripcion_modulo">Visualice las liquidaciones generadas.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo" runat="server" id="slnkReportePagoMovimiento">            
<asp:LinkButton ID="lnkReportePagoMovimientos" runat="server" CommandName="~/Liquidacion/ReportePagoMovimientos.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Reporte Pago Operador</h2>
<h3 class="descripcion_modulo">Visualice los movimientos y servicios de operadores, así como sus pagos.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkDepositos">            
<asp:LinkButton ID="lnkDepositos" runat="server" CommandName="~/EgresoServicio/ReporteDepositos.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Depositos</h2>
<h3 class="descripcion_modulo">Revise los depositos realizados a sus recursos.</h3>
</asp:LinkButton>
</section>      
<section class="modulo_azul" runat="server" id="slnkDiesel">            
<asp:LinkButton ID="lnkDiesel" runat="server" CommandName="~/EgresoServicio/ReporteValesDeDiesel.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/VisorModulo3.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Vales Diesel</h2>
<h3 class="descripcion_modulo">Visualice los vales de diesel emitidos por su empresa.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkLiquidacionSimplificada">            
<asp:LinkButton ID="lnkLiquidacionSimplificada" runat="server" CommandName="~/Liquidacion/LiquidacionSimplificada.aspx" OnClick="lnkDireccionaModulo_Click" >
<img src="../Image/PagoOperador.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Liquidacion Simplificada</h2>
<h3 class="descripcion_modulo">Liquide a sus operadores facil y rapido.</h3>
</asp:LinkButton>
</section>
</div>
</div>
</asp:Content>
