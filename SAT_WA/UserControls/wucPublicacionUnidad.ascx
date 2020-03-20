<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="wucPublicacionUnidad.ascx.cs" Inherits="SAT.UserControls.wucPublicacionUnidad" %>
<script type='text/javascript'>
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUCPublicacionUnidad();
}
}

//Creando función para configuración de jquery en formulario
function ConfiguraJQueryUCPublicacionUnidad() {
$(document).ready(function () {

//Validación de controles de Inserción de Ciudades
var validacionCiudadDeseada = function () {
var isValidP1 = !$('.scriptCiudadDeseada').validationEngine('validate');
var isValidP2 = !$('.scriptTarifaDeseada').validationEngine('validate');
var isValidP3 = !$('.scriptAnticipoDeseado').validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos
$('.scriptGuardarCiudadDeseada').click(validacionCiudadDeseada);

//Cargando Catalogo AutoCompleta Operadores Disponibles
$("#<%=txtOperador.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=25&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
appendTo: "<%=this.Contenedor%>"
});
//Cargando Catalogo AutoCompleta Ciudades Disponibles
$('.scriptCiudadDeseada').autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=8',
appendTo: "<%=this.Contenedor%>"
});
$("#<%=txtFechaDisponible.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%= txtLimiteDisponibilidad.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
}); 
});
}

//Invocando función de Configuración
ConfiguraJQueryUCPublicacionUnidad();
</script>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/OperacionPatio.png" />
<h2>Publicación de Unidad</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoUnidad">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoUnidad" AutoPostBack="true"  OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">

<div class="etiqueta">
<label for="txtUnidad">Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidad"  runat="server"  AutoPostBack="true"  OnTextChanged="txtUnidad_TextChanged"  CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtInformacionUnidad">Información Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtInformacionUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox  ID="txtInformacionUnidad" runat="server"  CssClass="textbox2x" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtUnidad" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOperador">Operador</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
    <asp:AsyncPostBackTrigger ControlID="txtUnidad"  />
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtInformacionOperador">Información Op:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtInformacionOperador" runat="server"  UpdateMode="Conditional">
<ContentTemplate>                       
<asp:TextBox ID="txtInformacionOperador" runat="server" CssClass="textbox2x"  Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
    <asp:AsyncPostBackTrigger ControlID="txtUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="chkLicenciaVigente">Licencia Vigente</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkLicenciaVigente" runat="server"  UpdateMode="Conditional">
<ContentTemplate>                       
<asp:CheckBox ID="chkLicenciaVigente" runat="server"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
 <asp:AsyncPostBackTrigger ControlID="txtUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstadoDisponible">Estado Disponible</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstadoDisponible" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstadoDisponible" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadoDisponible_SelectedIndexChanged" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCiudadDisponible">Ciudad Disponible</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCiudadDisponible" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadDisponible" runat="server" MaxLength="150" CssClass ="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlEstadoDisponible" />
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecha">Inicio Disp:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtFechaDisponible" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaDisponible" MaxLength="18" CssClass="textbox2x validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPublicar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtLimiteDisponibilidad">Limite Disp:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtLimiteDisponibilidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtLimiteDisponibilidad" MaxLength="18" CssClass="textbox2x validate[required]"></asp:TextBox>
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
<asp:TextBox runat="server" MaxLength="250" ID="txtObservacion" CssClass="textbox2x validate[required]"></asp:TextBox>
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
<asp:Button  ID="btnPublicar" runat="server"  OnClick="OnClick_btnPublicar" CssClass="boton" Text="Publicar"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>  
</div>
<div class="columna3x">
<div class="header_seccion">
<h2>Ciudades Destinos Deseadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoCiudadesDeseadas">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoCiudadesDeseadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCiudadesDeseadas" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoCiudadesDeseadas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoCiudadesDeseadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoCiudadesDeseadas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCiudadesDeseadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarCiudadesDeseadas" runat="server" TabIndex="5" 
OnClick="lkbExportarCiudadesDeseadas_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarCiudadesDeseadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvCiudadesDeseadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCiudadesDeseadas" runat="server"  AutoGenerateColumns="False" OnSorting="gvCiudadesDeseadas_Sorting" OnPageIndexChanging="gvCiudadesDeseadas_PageIndexChanging"
ShowFooter="True" CssClass="gridview"  Width="100%"  AllowPaging="True" AllowSorting="True"  PageSize="25" >
<Columns>
<asp:TemplateField HeaderText="Ciudad" SortExpression="Ciudad">
<ItemTemplate>
<asp:Label  ID="lCiudadDeseada" runat="server" Text='<%#Eval("Ciudad") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtCiudadDeseada" MaxLength="150" runat="server" CssClass="textbox scriptCiudadDeseada validate[required, custom[IdCatalogo]]" Text='<%# Eval("Ciudad") %>' >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Tarifa" SortExpression="Tarifa">
<ItemTemplate>
      <ItemStyle HorizontalAlign="Right" />
<asp:Label  ID="lblTarifadeseado" runat="server" Text='<%#Eval("Tarifa","{0:C2}") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtTarifaDeseada" MaxLength="12" runat="server" CssClass="textbox scriptTarifaDeseada validate[required, custom[positiveNumber]]" Text='<%# Eval("Tarifa") %>' >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Anticipo Req." SortExpression="Anticipo">
<ItemTemplate>
    <ItemStyle HorizontalAlign="Right" />
<asp:Label  ID="lblAnticipodeseado" runat="server" Text='<%#Eval("Anticipo","{0:C2}") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>  
<asp:TextBox ID="txtAnticipoDeseado" MaxLength="12" runat="server" CssClass="textbox scriptAnticipoDeseado validate[required,custom[positiveNumber]]" Text='<%# Eval("Anticipo") %>' >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>     
<asp:TemplateField HeaderText="" SortExpression="">
<ItemTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbEliminar_Click" ></asp:LinkButton>
</ItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lkbInsertar" runat="server" CssClass="textbox scriptGuardarCiudadDeseada" Text="Insertar"  OnClick="lkbInsertar_Click"></asp:LinkButton>
</FooterTemplate>
</asp:TemplateField>                         
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCiudadesDeseadas" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
    
</div>

