<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sucursal.aspx.cs" Inherits="SAT.General.Sucursal" %>
<%@ Register Src="~/UserControls/wucDireccion.ascx" TagName="WucDireccion" TagPrefix="tectos" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>  
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script> 

</head>
<body>
<form id="form1"  runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQuerySucursal();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQuerySucursal() {
$(document).ready(function () {
//Función de validación de campos
var validacionSucursal = function (evt) {
var isValidP1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtDireccion.ClientID%>").validationEngine('validate');
    return isValidP1 && isValidP2;
};
//Boton Guardar
$("#<%=btnGuardar.ClientID%>").click(validacionSucursal);
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQuerySucursal();
</script>
<div id="encabezado_forma">
<h1>Sucursal</h1>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnSucursales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSucursales" Text="Sucursales"  OnClick="btnVista_OnClick" CommandName="Sucursales" runat="server" CssClass="boton_pestana_activo"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSucursal" />
<asp:AsyncPostBackTrigger  ControlID="gvSucursales" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSucursal" Text="Alta" OnClick="btnVista_OnClick" runat="server" CommandName="Sucursal" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />
<asp:AsyncPostBackTrigger ControlID="gvSucursales" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="contenido_tabs">
<asp:UpdatePanel ID="upmtvComprobante" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
<ContentTemplate>
<asp:MultiView ID="mtvSucursales" runat="server" ActiveViewIndex="0">
<asp:View  ID="vwSucursales" runat="server">
<div class="columna3x">
<div class="header_seccion">
<h2>Sucursales</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewSucursales">Mostrar:</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamañoGridViewSucursales" runat="server" CssClass="dropdown_100px"
AutoPostBack="true" OnSelectedIndexChanged="ddlTamañoGridViewSucursales_SelectedIndexChanged"
TabIndex="29">
</asp:DropDownList>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewSucursales">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewSucursales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewSucursales" runat="server" Text="" CssClass="Label"></asp:Label></ContentTemplate><Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSucursales" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uplkbExcelSucursales" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelSucursales" runat="server" OnClick="lkbExcel_Click" EnableViewState="False"
CommandName="Sucursales" TabIndex="30">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelSucursales" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvSucursales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSucursales" runat="server" PageSize="5" Width="100%" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True"   CssClass="gridview2"
OnPageIndexChanging="gvSucursales_PageIndexChanging" 
OnSorting="gvSucursales_Sorting" TabIndex="31">
<Columns>
<asp:BoundField DataField="Sucursal" HeaderText="Sucursal" >
</asp:BoundField>
<asp:BoundField DataField="Direccion" HeaderText="Dirección" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEditar" runat="server"   OnClick="lkbEditar_Click" CommandName ="Editar" >Editar</asp:LinkButton>
<br />
<asp:LinkButton ID="lkbEliminar" runat="server"   OnClick="lkbEliminar_Click" CommandName="Eliminar" >Eliminar</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewSucursales" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorSucursales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorSucursales" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />                      
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwSucursal" runat="server">
<div class="seccion_controles">
<div class="header_seccion">
<h2>Datos de la Sucursal</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombre">Nombre</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNombre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombre" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />   
<asp:AsyncPostBackTrigger ControlID="gvSucursales" />                       
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDireccion">Dirección</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDireccion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDireccion" runat="server" CssClass="textbox2x validate[required]" Enabled="false" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />                                                     
<asp:AsyncPostBackTrigger ControlID="ucDireccion" />
<asp:AsyncPostBackTrigger ControlID="gvSucursales" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplnkVentana" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkVentana" runat="server" Text="Dirección" OnClick="lnkVentana_Click" TabIndex="3"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />                       
</Triggers>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />       
 <asp:AsyncPostBackTrigger ControlID="btnSucursal" /> 
<asp:AsyncPostBackTrigger ControlID ="gvSucursales" />               
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" TabIndex="11" OnClick="btnGuardar_Click" Text="Guardar" CssClass="boton" />
</ContentTemplate>
<Triggers>                          
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" TabIndex="12" OnClick="btnCancelar_Click" Text="Cancelar" CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>                          
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<asp:UpdatePanel ID="upucDireccion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:WucDireccion ID="ucDireccion" runat="server" TabIndex="20" Enable="false" OnClickGuardarDireccion="ucDireccion_ClickGuardarDireccion"
OnClickEliminarDireccion="ucDireccion_ClickEliminarDireccion" OnClickSeleccionarDireccion="ucDireccion_ClickSeleccionarDireccion" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkVentana" />
</Triggers>
</asp:UpdatePanel>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSucursal" />
<asp:AsyncPostBackTrigger ControlID="btnSucursales" />
<asp:AsyncPostBackTrigger ControlID="gvSucursales" />
</Triggers>
</asp:UpdatePanel>
</div>
</form>
</body>
</html>


