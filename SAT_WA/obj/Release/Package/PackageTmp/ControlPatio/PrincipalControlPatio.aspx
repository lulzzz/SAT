<%@ Page Title="Principal Control Patio" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="PrincipalControlPatio.aspx.cs" Inherits="SAT.ControlPatio.PrincipalControlPatio" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />    
    <link href="../CSS/PaginaPrincipal.css" rel="stylesheet" />
    <link href="../CSS/ControlPatio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div class="contenido_forma_principal">
    <div class="contenedor_imagen_principal_modulo">        
        <div class="vista">  
            <img src="../Image/PrincipalPatioLogistico.jpg" />
            <div class="mascara">  
                <h2>Control de Patios</h2>  
                <p>Incremente la productividad de su patio logistico</p>  
                <asp:LinkButton ID="lnkImagen" runat="server" CommandName="~/ControlPatio/ControlAcceso.aspx" OnClick="lnkDireccionaModulo_Click" CssClass="informacion">Dar acceso a una unidad..</asp:LinkButton>
            </div>  
        </div>      
    </div>
    <div class="contenido_derecho">       
        <section class="modulo" runat="server" id="slnkControlAcceso">
            <asp:LinkButton ID="lnkControlAcceso" runat="server" CommandName="~/ControlPatio/ControlAcceso.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/ControlAcceso.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Control Acceso</h2>
                <h3 class="descripcion_modulo">Capture la entrada y salida de unidades.</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_azul" runat="server" id="slnkOperacionPatio">
            <asp:LinkButton ID="lnkOperacionPatio" runat="server" CommandName="~/ControlPatio/OperacionPatio.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/OperacionPatio.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Operación Patio</h2>
                <h3 class="descripcion_modulo">Asigne andenes y cajones a las unidades en patio</h3>
            </asp:LinkButton>
        </section>
        <section class="modulo_amarillo" runat="server" id="slnkPatio">
            <asp:LinkButton ID="lnkPatio" runat="server" CommandName="~/ControlPatio/UnidadesDentro.aspx" OnClick="lnkDireccionaModulo_Click" >
                <img src="../Image/EstatusPatio.jpg" class="imagen_modulo"/>
                <h2 class="titulo_modulo">Estatus Patio</h2>
                <h3 class="descripcion_modulo">Visualice el estado general del patio</h3>
            </asp:LinkButton>
        </section>
        <section class="resumen_modulo">
            <div class="renglon2x">
                <div class="etiqueta"><label for="ddlPatio">Patio</label></div>
                <div class="control2x">                    
                    <asp:DropDownList ID="ddlPatio" runat="server" CssClass="dropdown2x" OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                     
                </div>               
                <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_Click">
                    <img src="../Image/Reload.png" />
                </asp:LinkButton>                
            </div>           
            <section class="fila_indicador">        
        <a href="#" class="indicador">
            <div class="numero_indicador">
                 <asp:UpdatePanel runat="server" ID="upplblUnidades" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblUnidades"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                 <img src="../Image/IndicadorUnidadesPatio.png" />
            </div>
            <div class="leyenda_indicador">
                Unidades en patio
            </div>           
        </a>
        <a href="#" class="indicador_texto">
            <div class="texto_indicador">
                <asp:UpdatePanel runat="server" ID="upplblTiempo" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblTiempo"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="lnkActualizar" />                                              
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                 <img src="../Image/IndicadorTiempo.png" />
            </div>
            <div class="leyenda_indicador">
                Estancia promedio de unidades
            </div>           
        </a>
        <a href="#" class="indicador">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblEntradaSalida" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblEntradaSalida"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
                        <asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                 <img src="../Image/EntradasSalidas.png" />
            </div>
            <div class="leyenda_indicador">
                Entradas y salidas
            </div>           
        </a>                       
                        
    </section>    
        </section>
            
         
        
       
    </div>

</div>
</asp:Content>
