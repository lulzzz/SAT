<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalCuentasPagar.aspx.cs" Inherits="SAT.CuentasPagar.PrincipalCuentasCobrar" %>

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
<h2>Cuentas por Pagar</h2>
<p>El seguimiento de nuestras cuentas por pagar en una sola pantalla.</p>
<asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/CuentasPagar/ReporteSaldosGlobales.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Visualice sus saldos</asp:LinkButton>
</div>
</div>
</div>

<div class="contenido_derecho">
<section class="modulo_rojo" runat="server" id="slnkRecepcionFactura">
<asp:LinkButton ID="lnkRecepcionFactura" runat="server" CommandName="~/CuentasPagar/RecepcionFactura.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/RecepcionFactura.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Recepción Facturas</h2>
<h3 class="descripcion_modulo">Reciba de forma sencilla las facturas de su proveedor.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkFacturasProveedor">
<asp:LinkButton ID="lnkFacturasProveedor" runat="server" CommandName="~/CuentasPagar/Facturado.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/FacturaProveedor.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Factura Proveedor</h2>
<h3 class="descripcion_modulo">Visualice las facturas de sus proveedores.</h3>
</asp:LinkButton>
</section>
<section class="modulo_naranja" runat="server" id="slnkAplicacionPagos">
<asp:LinkButton ID="lnkAplicacionPagos" runat="server" CommandName="~/CuentasPagar/AplicacionFacturasProveedor.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/CuentasCobrar.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Pagos Proveedor</h2>
<h3 class="descripcion_modulo">Realice los pagos a sus  proveedores.</h3>
</asp:LinkButton>
</section>
<section class="modulo_amarillo" runat="server" id="slnkFacturasDiesel">
<asp:LinkButton ID="lnkFacturasDiesel" runat="server" CommandName="~/EgresoServicio/DieselFactura.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/Diesel.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Facturas Diesel</h2>
<h3 class="descripcion_modulo">Compruebe sus facturas de diesel.</h3>
</asp:LinkButton>
</section>

<section class="modulo_rojo" runat="server" id="slnkReporteSaldoDetalle">
<asp:LinkButton ID="lnkReporteSaldoDetalle" runat="server" CommandName="~/CuentasPagar/ReporteSaldosDetalles.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/VisorModulo.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Saldos Detalle</h2>
<h3 class="descripcion_modulo">Revise los saldos a detalle de sus proveedores.</h3>
</asp:LinkButton>
</section>
<section class="modulo_azul" runat="server" id="slnkReporteSaldoGlobal">
<asp:LinkButton ID="lnkReporteSaldoGlobal" runat="server" CommandName="~/CuentasPagar/ReporteSaldosGlobales.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/VisorModulo2.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Saldos Globales</h2>
<h3 class="descripcion_modulo">Revise sus saldos a nivel proveedor.</h3>
</asp:LinkButton>
</section>
<section class="modulo" runat="server" id="slnkReporteSaldoPeriodo">
<asp:LinkButton ID="lnkReporteSaldoPeriodo" runat="server" CommandName="~/CuentasPagar/ReporteSaldosPeriodo.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/VisorModulo3.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Saldos Periodo</h2>
<h3 class="descripcion_modulo">Revise sus saldos por fecha de vencimiento.</h3>
</asp:LinkButton>
</section>
<section class="modulo_naranja" runat="server" id="slnkReporteIntegracionCXP">
<asp:LinkButton ID="lnkReporteIntegracionCXP" runat="server" CommandName="~/CuentasPagar/ReporteIntegracionCuentasPagar.aspx" OnClick="lnkDireccionaModulo_Click">
<img src="../Image/ModuloFacturacion.jpg" class="imagen_modulo"/>
<h2 class="titulo_modulo">Integración CxP</h2>
<h3 class="descripcion_modulo">Consulte los pagos realizados a facturas de Proveedor.</h3>
</asp:LinkButton>
</section>
</div>

</div>
</asp:Content>
