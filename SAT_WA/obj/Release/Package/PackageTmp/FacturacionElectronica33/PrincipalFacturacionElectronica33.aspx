<%@ Page Title="Principal Facturación Electrónica 3.3" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalFacturacionElectronica33.aspx.cs" Inherits="SAT.FacturacionElectronica33.PrincipalFacturacionElectronica33" MaintainScrollPositionOnPostback="true"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Estilos Documentación de Servicio-->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet"/>    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div class="contenido_forma_principal">
        <div class="contenedor_imagen_principal_modulo">
            <div class="vista">
                <img src="../Image/FE33.png" />
                <div class="mascara">
                    <h2>Facturación Electrónica 3.3</h2>
                    <p>El camino hacia la riqueza depende fundamentalmente de dos palabras: trabajo y ahorro.</p><!--Párrafo inspiratorio random-->
                    <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/FacturacionElectronica33/FacturacionOtrosV33.aspx" OnClick ="lnkDireccionaModulo_Click" CssClass ="informacion">Cree su Facturacion Electronica, ahora.</asp:LinkButton> <!--Comando pendiente     Ej.: CommandName ="~/Documentacion/Servicio.aspx"-->
                </div>
            </div>
        </div>
        <div class="contenido_derecho">
            <!--Factura Global V33-->
            <section class="modulo_azul" runat ="server" id="slnkFacturacionGlobalV33">
                <asp:LinkButton ID="lnkFacturacionGlobalV33" runat="server" CommandName="~/FacturacionElectronica33/FacturaGlobalV33.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/Compania.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Facturación Global</h2>
                    <h3 class="descripcion_modulo">Gestione las una sola facturación para sus multiples necesidades</h3>
                </asp:LinkButton>
            </section>
            <!--Facturación Otros V33-->
            <section class="modulo_amarillo" runat ="server" id="slnkFacturacionOtrosV33">
                <asp:LinkButton ID="lnkFacturacionOtrosV33" runat="server" CommandName="~/FacturacionElectronica33/FacturacionOtrosV33.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/FacturacionOtros.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Facturación de Otros</h2>
                    <h3 class="descripcion_modulo">Gestione las versiones y series.</h3>
                </asp:LinkButton>
            </section>
            <!--Facturacion X Servicio-->
            <section class="modulo_rojo" runat ="server" id="slnkFacturacionSimplificadaV33">
                <asp:LinkButton ID="lnkFacturacionSimplificadaV33" runat="server" CommandName="~/FacturacionElectronica33/FacturacionSimplificadaV33.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/administrativo.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Facturación por Servicio</h2>
                    <h3 class="descripcion_modulo">Facture sus viajes de manera ágil</h3>
                </asp:LinkButton>
            </section>
            <!--Forma de Pago-->
            <section class="modulo_amarillo" runat ="server" id="slnkFormaPago">
                <asp:LinkButton ID="lnkFormaPago" runat="server" CommandName="~/FacturacionElectronica33/FormaPago.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/AplicacionPagos.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Forma de Pago</h2>
                    <h3 class="descripcion_modulo">Gestione las distintas formas de pago.</h3>
                </asp:LinkButton>
            </section>
            <!--Reporte Comprobante-->
            <section class="modulo_rojo" runat ="server" id="slnkReporteComprobanteV33">
                <asp:LinkButton ID="lnkReporteComprobanteV33" runat="server" CommandName="~/FacturacionElectronica33/ReporteComprobanteV33.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/RecepcionPaquete.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Reporte de Comprobantes</h2>
                    <h3 class="descripcion_modulo">Gestione las versiones y series.</h3>
                </asp:LinkButton>
            </section>
            <!--Serie Folio CFDI-->
            <section class="modulo_azul" runat ="server" id="slnkSerieFolioCFDI">
                <asp:LinkButton ID="lnkSerieFolioCFDI" runat="server" CommandName="~/FacturacionElectronica33/SerieFolioCFDI.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/FacturaProveedor.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Serie y Folio CFDI</h2>
                    <h3 class="descripcion_modulo">Gestione las versiones y series.</h3>
                </asp:LinkButton>
            </section>
            <!--CFDI Recepción Pagos-->
            <section class="modulo_amarillo" runat ="server" id="slnkCFDIRecepcionPagoV33">
               <asp:LinkButton ID="lnkCFDIRecepcionPagosV33" runat="server" CommandName="~/FacturacionElectronica33/ComprobanteRecepcionPagosV10.aspx" OnClick="lnkDireccionaModulo_Click">
                   <img src="../Image/ModuloCFDIRecepcionPago.jpg" class ="imagen_modulo"/>
                   <h2 class ="titulo_modulo">CFDI Recepción de Pagos</h2>
                   <h3 class="descripcion_modulo">-</h3>
               </asp:LinkButton>
            </section>
        </div>
    </div>
</asp:Content>
