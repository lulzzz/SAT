<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalAlmacen.aspx.cs" Inherits="SAT.Almacen.PrincipalAlmacen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div class="contenido_forma_principal">
        <div class="contenedor_imagen_principal_modulo">
            <div class="vista">
                <img src="../Image/PrincipalAlmacen.jpg" />
                <div class="mascara">
                    <h2>Almacén</h2>
                    <p>Lleve un control de entradas y salidas de producto.</p>
                    <asp:LinkButton ID="lnkImage" runat="server" CommandName="~/Almacen/OrdenCompra.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Inventario</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="contenido_derecho">
            <section class="modulo_amarillo" runat="server" id="slnkProducto">
                <asp:LinkButton ID="lnkProducto" runat="server" CommandName="~/Almacen/Producto.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/ProductoAlmacen.png" class="imagen_modulo" />
                    <h2 class="titulo_modulo">Producto</h2>
                    <h3 class="descripcion_modulo">Da de alta las caracteristicas principales de un producto.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo_azul" runat="server" id="slnkOrdenCompra">            
                <asp:LinkButton ID="lnkOrdenCompra" runat="server" CommandName="~/Almacen/OrdenCompra.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/OrdenCompra.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Orden Compra</h2>
                    <h3 class="descripcion_modulo">Genere las ordenes de compra de sus refacciones.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo_naranja" runat="server" id="slnkOrdenCompraPendiente">
                <asp:LinkButton ID="lnkOrdenCompraPendiente" runat="server" CommandName="~/Almacen/OrdenesCompraPendientes.aspx" OnClick="lnkDireccionaModulo_Click">
                    <img src="../Image/HojaInstruccion.jpg" class="imagen_modulo" />
                    <h2 class="titulo_modulo">Reporte O.C Pendientes</h2>
                    <h3 class="descripcion_modulo">Visualizar las O.C. solicitadas o pendientes por abastecer.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo_rojo" runat="server" id="slnkRequisicionPendiente">            
                <asp:LinkButton ID="lnkRequisicionP" runat="server" CommandName="~/Almacen/RequisicionesPendientes.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/EntregaFactura.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Requisiciones Pendientes.</h2>
                    <h3 class="descripcion_modulo">Visualiza las Requisiciones pendientes por abastecer.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo" runat="server" id="slnkExistenciasAlmacen">            
                <asp:LinkButton ID="lnkExistenciasALmacen" runat="server" CommandName="~/Almacen/ExistenciasAlmacen.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/existencias-almacen.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Existencias Almacén.</h2>
                    <h3 class="descripcion_modulo">Visualiza el número de productos disponibles en almacén.</h3>
                </asp:LinkButton>
            </section>
        </div>


        
    </div>
</asp:Content>
