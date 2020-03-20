<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPublicacionServicio.ascx.cs" Inherits="SAT.UserControls.wucPublicacionServicio" %>
<script type='text/javascript'>
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryUCPublicacionServicio();
        }
    }

    //Creando función para configuración de jquery en formulario
    function ConfiguraJQueryUCPublicacionServicio() {
        $(document).ready(function () {

            //Validación de controles de Inserción de Ciudades
            var validacionPublicacionServicio = function () {
                var isValidP1 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtProducto.ClientID%>").validationEngine('validate');
                var isValidP3 = !$("#<%=txtPeso.ClientID%>").validationEngine('validate');
                var isValidP4 = !$("#<%=txtTarifa.ClientID%>").validationEngine('validate');
                var isValidP5 = !$("#<%=txtContacto.ClientID%>").validationEngine('validate');
                var isValidP6 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
                var isValidP7 = !$("#<%=txtObservacion.ClientID%>").validationEngine('validate');
                return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7;
            };
            //Validación de campos requeridos
            $("#<%=btnPublicar.ClientID%>").click(validacionPublicacionServicio);
            //Cargando Catalogo AutoCompleta
            $("#<%=txtProducto.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=1&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
    });

    });
    }

    //Invocando función de Configuración
    ConfiguraJQueryUCPublicacionServicio();
</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/OperacionPatio.png" />
        <h2>Publicación de Servicio</h2>
    </div>
    <div class="columna2x">
           <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtNoServicio">No Servicio</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtNoServicio"  runat="server"  MaxLength="30"  CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
              <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtConfirmacion">Confirmación</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtConfirmacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtConfirmacion"  MaxLength="50" runat="server"   CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
<div class="etiqueta">
<label for="txtProducto">Producto</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProducto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProducto" MaxLength="50" CssClass="textbox2x validate[required,custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
      <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>             
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtPeso">Peso:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtPeso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtPeso" runat="server" TabIndex="6" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
             </div>
         <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlDimensiones">Dimensiones</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlDimensiones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlDimensiones" AutoPostBack="true"  runat="server" CssClass="dropdown"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
            <div class="renglon3x">
            
            <div class="etiqueta">
                <asp:UpdatePanel ID="upchkFull" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>                       
                      <asp:CheckBox ID="chkFull" runat="server"  Text="Full" />
                    </ContentTemplate>
                    <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>            
            <div class="etiqueta">
                <asp:UpdatePanel ID="upchkManiobras" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>                       
                      <asp:CheckBox ID="chkManiobras" runat="server"  Text="Maniobras" />
                    </ContentTemplate>
                    <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>                
            <div class="etiqueta">
                <asp:UpdatePanel ID="upchkRC" runat="server"  UpdateMode="Conditional">
                    <ContentTemplate>                       
                      <asp:CheckBox ID="chkRC" runat="server"  Text="R. Control" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>              
        </div>   
          <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtTarifa">Tarifa:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtTarifa" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTarifa" runat="server" TabIndex="6" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
             </div>
           <div class="renglon2x">
<div class="etiqueta">
    <label for="lblTarifaAceptada">Tarifa Aceptada:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uplblTarifaAceptada" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblTarifaAceptada" runat="server" ForeColor="Red" CssClass="label_negrita"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
        </Triggers>
    </asp:UpdatePanel>
</div>
   
    </div>
                <div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtContacto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtContacto" MaxLength="50" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
                        <div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefono">Telefono:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTelefono" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTelefono" MaxLength="20" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
  <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
               
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtObservacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtObservacion" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
      <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnPublicar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button  ID="btnPublicar" runat="server"   OnClick="OnClick_btnPublicar" CssClass="boton" Text="Publicar"  />
                    </ContentTemplate>
                    <Triggers>
                         
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>  
    </div>
    <div class="columna3x">
         <div class="header_seccion">
<h2>Paradas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoParadas">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoParadas" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoParadas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoParadas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvParadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarParadas" runat="server" TabIndex="5" 
OnClick="lkbExportarParadas_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarParadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadas" runat="server"  AutoGenerateColumns="False" OnSorting="gvParadas_Sorting" OnPageIndexChanging="gvParadas_PageIndexChanging"
 ShowFooter="True" CssClass="gridview"  Width="100%"  AllowPaging="True" AllowSorting="True"  PageSize="25" >
<Columns>
<asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" DataFormatString="{0:0}" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" DataFormatString="{0:dd/MM/yyyy HH:mm}" />                                             
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
  <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
    <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
</Triggers>
</asp:UpdatePanel> 
</div>
    </div>
    
    
</div>
 

