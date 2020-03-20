<%@ Page Title="Caseta" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Caseta.aspx.cs" Inherits="SAT.Ruta.Caseta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Estilos para la forma-->
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />    
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Estilo del query-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type ="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryCaseta
ConfiguraJQueryCaseta();
}
}

//Declara la función que valida los controles de la pagina
function ConfiguraJQueryCaseta() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable validaActividad
var validaActividad = function () {
//Creación de las variables y asignacion de los controles de la pagina Actividad
var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtAlias.ClientID%>").validationEngine('validate');            
//Devuelve un valor a la funcion
return isValid1 && isValid2;
}
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaActividad);
$("#<%=lkbGuardar.ClientID%>").click(validaActividad);
//Creación  y asignación de la funcion a la variable validaActividad
var validaCostoCaseta = function () {
//Creación de las variables y asignacion de los controles de la pagina Actividad
var isValid1 = !$("#<%=txtNoEjes.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCosto.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2 && isValid3;
}
$("#<%=btnAgregarCosto.ClientID%>").click(validaCostoCaseta);
//Cargando Controles de Fecha
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
    
<%--//Añadiendo Encabezado Fijo
$("#<%=gvCostoCaseta.ClientID%>").gridviewScroll({
width: document.getElementById("contenedorProdctoActividad").offsetWidth - 15,
height: 400,
//freezesize: 4
});--%>

}
ConfiguraJQueryCaseta();
</script>
<div id="encabezado_forma">
<h1>Caseta</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
</ul>
</li>

</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbAbrir" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="contenedor_controles">
<div class="columna3x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoCaseta">No. Caseta</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uplblNoCaseta" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNoCaseta" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtDescripcion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" CssClass="textbox2x validate[required]" MaxLength="300" TabIndex="1" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtAlias">Nombre Corto</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtAlias" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtAlias" CssClass="textbox2x validate[custom[onlyLetterSp]]" MaxLength="150" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoCaseta">Tipo Caseta</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlTipoCaseta" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoCaseta" CssClass="dropdown_100px" TabIndex="3"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlRedCarretera">Red Carretera</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlRedCarretera" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlRedCarretera" CssClass="dropdown_100px" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upchkIAVE" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox runat="server" ID="chkIAVE" Text="Uso de tarjeta IAVE" TabIndex="5"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="6" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Costo Casetas</h2>
</div>
<div class="columna2x">
<div class="renglon_pestaña_documentacion">
<div class="etiqueta_50px">
<label for="ddlUnidadAutomotriz">Unidad</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="upddlUnidadAutomotriz" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlUnidadAutomotriz" CssClass="dropdown_100px" TabIndex="8"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtNoEjes">No. Ejes</label>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="uptxtNoEjes" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtNoEjes" CssClass="textbox_50px validate[required,custom[integer]]" MaxLength="9" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarCosto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtCosto">Costo</label>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="uptxtCosto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCosto" CssClass="textbox_50px validate[required,custom[number]]" MaxLength="9" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarCosto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna4x">
<div class="etiqueta">
<label for="ddlActualizacion">Actualización</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="upddlActualizacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlActualizacion" CssClass="dropdown_100px" TabIndex="10"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarCosto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtFecha">Fecha</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFecha" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFecha" CssClass="textbox validate[[required,custom[dateTime24]]]" TabIndex="11"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarCosto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="upbtnAgregarCosto" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnAgregarCosto" CssClass="boton" Text="Agregar" OnClick="btnAgregarCosto_Click" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_80px">
<asp:UpdatePanel runat="server" ID="upbtCancelarCosto" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelarCosto" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarCosto_Click" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewCostoCaseta">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewCostoCaseta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewCostoCaseta" runat="server" OnSelectedIndexChanged="gvCostoCaseta_OnSelectedIndexChanged" TabIndex="14" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewCostoCaseta">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewCostoCaseta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewCostoCaseta" TabIndex="15" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCostoCaseta" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelCostoCaseta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelCostoCaseta" runat="server" Text="Exportar" TabIndex="16" OnClick="lkbExportarExcelCostoCaseta_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelCostoCaseta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorProdctoActividad">
<asp:UpdatePanel ID="upgvCostoCaseta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCostoCaseta" CssClass="gridview" OnPageIndexChanging="gvCostoCaseta_OnpageIndexChanging" OnSorting="gvCostoCaseta_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="17"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="No Servicio" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoCaseta" HeaderText="No.Caseta" SortExpression="NoCaseta" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Caseta" HeaderText="Caseta" SortExpression="Caseta" ItemStyle-HorizontalAlign="Left"/>
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" ItemStyle-HorizontalAlign="Left"/>
<asp:BoundField DataField="NoEjes" HeaderText="No. Ejes" SortExpression="NoEjes" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Actualizacion" HeaderText="Actualizacion" SortExpression="Actualizacion" ItemStyle-HorizontalAlign="Left" />
<asp:BoundField DataField="FechaActualizacion" HeaderText="Fecha Actualizacion" SortExpression="FechaActualizacion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
<asp:BoundField DataField="Costo" HeaderText="Costo" SortExpression="Costo" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton runat="server" ID="lnkEditar" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton runat="server" ID="lnkEliminar" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnAgregarCosto" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewCostoCaseta" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>



