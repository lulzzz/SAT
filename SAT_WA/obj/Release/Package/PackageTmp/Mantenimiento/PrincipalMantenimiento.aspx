<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalMantenimiento.aspx.cs" Inherits="SAT.Mantenimiento.PrincipalMantenimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div class="contenido_forma_principal">
        <div class="contenedor_imagen_principal_modulo">        
            <div class="vista">  
                <img src="../Image/PrincipalMantenimiento.jpg" />
                <div class="mascara">  
                    <h2>Mantenimiento</h2>  
                    <p>Conserve en estado optimo sus unidades y extienda su ciclo de vida.</p>
                    <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/Mantenimiento/OrdenTrabajo.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Orden Trabajo</asp:LinkButton>
                </div>  
            </div>      
        </div>
        <div class="contenido_derecho"> 
            <section class="modulo_amarillo" runat="server" id="slnkOrdenTrabajo">            
                <asp:LinkButton ID="lnkOrdenTrabajo" runat="server" CommandName="~/Mantenimiento/OrdenTrabajo.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/OrdenTrabajo.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Orden Trabajo</h2>
                    <h3 class="descripcion_modulo">De de alta sus ordenes de trabajo.</h3>
                </asp:LinkButton>
            </section>      
            <section class="modulo_naranja" runat="server" id="slnkActividades">            
                <asp:LinkButton ID="lnkActividades" runat="server" CommandName="~/Mantenimiento/ActividadesTaller.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/ActividadesTaller.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Actividades Taller</h2>
                    <h3 class="descripcion_modulo">Asigne personal e insumos a sus actividades.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo_azul" runat="server" id="slnkActividad">            
                <asp:LinkButton ID="lnkActividad" runat="server" CommandName="~/Mantenimiento/Actividad.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/actividad.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Actividad</h2>
                    <h3 class="descripcion_modulo">De de alta las actividades de mantenimiento automotriz.</h3>
                </asp:LinkButton>
            </section>
            <section class="modulo_rojo" runat="server" id="slnkImportadorLectura">            
                <asp:LinkButton ID="lnkImportadorLectura" runat="server" CommandName="~/Mantenimiento/ImportacionLecturasDiesel.aspx" OnClick="lnkDireccionaModulo_Click" >
                    <img src="../Image/Diesel.jpg" class="imagen_modulo"/>
                    <h2 class="titulo_modulo">Lecturas Diesel</h2>
                    <h3 class="descripcion_modulo">Ingrese las Lecturas de sus Unidades desde un archivo.</h3>
                </asp:LinkButton>
            </section>
        </div>
    </div>
</asp:Content>
