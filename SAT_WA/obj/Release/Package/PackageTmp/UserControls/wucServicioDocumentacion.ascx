<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucServicioDocumentacion.ascx.cs" Inherits="SAT.UserControls.wucServicioDocumentacion" %>
<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagPrefix="uc1" TagName="wucClasificacion" %>
<%@ Register Src="~/UserControls/wucTemperaturaServicio.ascx" TagPrefix="uc1" TagName="wucTemperaturaServicio" %>
<%@ Register Src="~/UserControls/wucParadaEvento.ascx" TagPrefix="uc1" TagName="wucParadaEvento" %>


<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />

<!-- REGISTRANDO SCRIPTS DE VALIDACIÓN DE CONTENIDO DE CONTROLES -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQuerywucServicioDocumentacion();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQuerywucServicioDocumentacion() {
$(document).ready(function () {

//Validación de controles de búsqueda
var validacionParada = function () {
var isValidP1 = !$("#<%=txtSecuenciaParada.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtUbicacionParada.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtCitaParada.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos en parada
$("#<%=imbAgregarParada.ClientID%>").click(validacionParada);

var validacionProducto = function () {

var isValidP2 = !$("#<%=txtCantidadProducto.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtPesoProducto.ClientID%>").validationEngine('validate');
return isValidP2 && isValidP3;
};
//Validación de campos requeridos en producto
$("#<%=imbAgregarProducto.ClientID%>").click(validacionProducto);

//Selector de Fecha Cita
$("#<%=txtCitaParada.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Autocomplete de Clientes Compania
$("#<%=txtClienteServicio.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
appendTo: "<%=this.Contenedor%>"
});
//Autocomplete de Ubicación
$("#<%= txtUbicacionParada.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
appendTo: "<%=this.Contenedor%>"
});
//Autocomplete de productos (evento carga)
$("#<%= txtProductoEventoParada.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' + '&param2=1',
appendTo: "<%=this.Contenedor%>"
});
});
}

//Diseño Grid View
$("[src*=plus]").live("click", function () {
$(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
$(this).attr("src", "../Image/minus.png");
});
$("[src*=minus]").live("click", function () {
$(this).attr("src", "../Image/plus.png");
$(this).closest("tr").next().remove();
});

//Invocación Inicial de método de configuración JQuery
ConfiguraJQuerywucServicioDocumentacion();
</script>

<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<asp:UpdatePanel ID="uph2EncabezadoServicioDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2 id="h2EncabezadoServicioDocumentacion" runat="server">Documentación de Servicio</h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaEcabezadoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaEcabezadoServicio" Text="Encabezado" OnClick="btnVistaServicioDocumentacion_OnClick" CommandName="Encabezado" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaParadasServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaClasificacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaParadasServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaParadasServicio" Text="Información de Carga" OnClick="btnVistaServicioDocumentacion_OnClick" runat="server" CommandName="Paradas" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaEcabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaClasificacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaClasificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaClasificacion" Text="Clasificación Servicio" OnClick="btnVistaServicioDocumentacion_OnClick" runat="server" CommandName="Clasificacion" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaEcabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaParadasServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaResumenServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaResumenServicio" Text="Detalles del Servicio" OnClick="btnVistaServicioDocumentacion_OnClick" runat="server" CommandName="Resumen" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaEcabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaParadasServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaClasificacion" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs_sin_margen">
<asp:UpdatePanel ID="upmtvDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvDocumentacion" runat="server" ActiveViewIndex="0" >
<asp:View ID="vwEncabezado" runat="server">
<div class="columna2x">
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Datos del Servicio</h2>
</div>
<br />
<div class="renglon2x">
<div class="etiqueta">
<label for="txtClienteServicio">Cliente</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtClienteServicio" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Ref. Cliente</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" MaxLength="150"></asp:TextBox>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEncabezadoDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEncabezadoDocumentacion" runat="server" CssClass="boton" Text="Guardar" OnClick="btnEncabezadoDocumentacion_Click" CommandName="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
        
</div>  
</div>
                  
</div>
<div class="columna2x">
<uc1:wucTemperaturaServicio runat="server" ID="wucTemperaturaServicio" VisualizaBotonGuardar="False" OnClickGuardarTemperaturas="wucTemperaturaServicio_ClickGuardarTemperaturas" />
</div>
</asp:View>
<asp:View ID="vwParadas" runat="server">
<div style="height:auto; width:43%; float:left; margin-left:5px; margin-top:5px;">
<asp:Panel ID="pnlParadaDocumentacion" runat="server" DefaultButton="imbAgregarParada"  Height="40px" >
<div class="control_60px">
<label for="txtSecuenciaParada">Secuencia</label>
<asp:UpdatePanel ID="uptxtSecuenciaParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSecuenciaParada" runat="server" CssClass="textbox_50px validate[required, custom[onlyNumberSp]]" MaxLength="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="ddlTipoParadaDocumentacion">Tipo</label>
<asp:UpdatePanel ID="upddlTipoParadaDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoParadaDocumentacion" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<label for="txtUbicacionParada">Ubicación</label>
<asp:UpdatePanel ID="uptxtUbicacionParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionParada" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="txtCitaParada">Cita</label>
<asp:UpdatePanel ID="uptxtCitaParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCitaParada" runat="server" CssClass="textbox_100px validate[custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div style="width:auto; padding:5px">
<asp:UpdatePanel ID="upimbAgregarParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="imbAgregarParada" runat="server"  OnClick="imbAgregar_Click" CommandName="Parada">
<img alt="" src="../Image/Agregar.png" width="25" height="25" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:Panel>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvParadasDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadasDocumentacion" runat="server" AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvParadasDocumentacion_PageIndexChanging" AllowPaging="True" >
<Columns>
<asp:TemplateField SortExpression="Secuencia" HeaderText="Sec.">
<ItemTemplate>
<asp:LinkButton ID="lkbSeleccionar" runat="server" CommandName="Seleccionar" Text='<%#Eval("Secuencia", "{0:###}") %>' ToolTip="Ver Eventos y Productos" OnClick="lkbParada_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="TipoParada" HeaderText="Tipo Parada" SortExpression="TipoParada">
</asp:BoundField>
<asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" >
</asp:BoundField>
<asp:TemplateField HeaderText="Cita" SortExpression="Cita">
<ItemTemplate>
<asp:LinkButton ID="lkbCitasEventos" runat="server" ToolTip="Ver Citas de Eventos en esta Parada" CommandName="CitasEventos" OnClick="lkbParada_Click" Text='<%#Eval("Cita", "{0:dd/MM/yyyy HH:mm}") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="imbEditarParada" runat="server" ImageUrl="~/Image/EditarDoc.png" CommandName="Editar" OnClick="imbParada_Click" Width="20px" ToolTip="Editar"></asp:ImageButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="lkbEliminarParada" runat="server" CommandName="Eliminar" ImageUrl="~/Image/borrar.png" OnClick="imbParada_Click" Width="20px" ToolTip="Eliminar"></asp:ImageButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="lkbReferenciasParada" runat="server" CommandName="Referencias" ImageUrl="~/Image/Referencias.png" OnClick="imbParada_Click" Width="20px" ToolTip="Referencias"></asp:ImageButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div style="height:auto; width:55%; float:left; margin-left:5px; margin-top:5px;">
<asp:Panel ID="pnlEventoProductoDocumentacion" runat="server" DefaultButton="imbAgregarProducto" Height="40px" >
<div class="control_80px">
<label for="ddlTipoEventoParada">Evento</label>
<asp:UpdatePanel ID="upddlTipoEventoParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEventoParada" runat="server" CssClass="dropdown_80px" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoEventoParada_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<label for="txtProductoEventoParada">Producto</label>
<asp:UpdatePanel ID="uptxtProductoEventoParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProductoEventoParada" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]"></asp:TextBox>
<asp:DropDownList ID="ddlProductoEventoParada" runat="server" CssClass="dropdown" Visible="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoEventoParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="txtCantidadProducto">Cant.</label>
<asp:UpdatePanel ID="uptxtCantidadProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidadProducto" runat="server" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="ddlUnidadCantidad">Unidad</label>
<asp:UpdatePanel ID="upddlUnidadCantidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnidadCantidad" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="txtPesoProducto">Peso</label>
<asp:UpdatePanel ID="uptxtPesoProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPesoProducto" runat="server" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="ddlUnidadPeso">Unidad</label>
<asp:UpdatePanel ID="upddlUnidadPeso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnidadPeso" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvProductoEvento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div style="width:auto; padding:5px">
<asp:LinkButton ID="imbAgregarProducto" runat="server"  OnClick="imbAgregar_Click" CommandName="Producto">
<img alt="" src="../Image/Agregar.png" width="25" height="25" />
</asp:LinkButton>
</div>
</asp:Panel>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvProductoEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvProductoEvento" runat="server" AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvProductoEvento_PageIndexChanging" AllowPaging="True">
<Columns>
<asp:BoundField DataField="TipoEvento" HeaderText="Evento" SortExpression="TipoEvento">
</asp:BoundField>
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" >
</asp:BoundField>
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="UnidadCantidad" HeaderText="Unidad" SortExpression="UnidadCantidad" />
<asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="UnidadPeso" HeaderText="Unidad" SortExpression="UnidadPeso" />
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="imbEditarProducto" runat="server" CommandName="Editar" ImageUrl="~/Image/EditarDoc.png" OnClick="imbProducto_Click" ToolTip="Editar" Width="20px"></asp:ImageButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="imbEliminarProducto" runat="server" CommandName="Eliminar" ImageUrl="~/Image/borrar.png" OnClick="imbProducto_Click" ToolTip="Eliminar" Width="20px"></asp:ImageButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="imbAgregarProducto" />
<asp:AsyncPostBackTrigger ControlID="imbAgregarParada" />
<asp:AsyncPostBackTrigger ControlID="gvParadasDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwClasificacion" runat="server">
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Clasificación del Servicio</h2>
</div>
<asp:UpdatePanel ID="upwucClasificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucClasificacion runat="server" ID="wucClasificacion" OnClickGuardar="wucClasificacion_ClickGuardar" OnClickCancelar="wucClasificacion_ClickCancelar"/>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwResumen" runat="server">
<div  class="seccion_controles">
<div class="header_seccion">
<img alt="" src="../Image/ResumenReporte.png" />
<h2>Resumen del Servicio</h2></div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblCliente">Cliente:</label></div>
<div class="control2xr">
<asp:UpdatePanel ID="uplblCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label  ID="lblCliente" CssClass="label_negrita" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblCartaPorte">Carta Porte:</label></div>
<div class="control2xr">
<asp:UpdatePanel ID="uplblCartaPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCartaPorte" CssClass="label_negrita" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblRefCliente">Ref Cliente:</label></div>
<div class="control2xr">
<asp:UpdatePanel ID="uplblRefCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRefCliente" CssClass="label_negrita" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div> 
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblObservacion">Observación:</label></div>
<div class="control2xr">
<asp:UpdatePanel ID="uplblObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblObservacion" CssClass="label_negrita" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>      
<div  class="seccion_controles">
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoParadas">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoParadas"></label>
<asp:DropDownList ID="ddlTamanoParadas" runat="server" OnSelectedIndexChanged="ddlTamanoParadas_OnSelectedIndexChanged" TabIndex="8" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarParadas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarParadas" runat="server" Text="Exportar Excel" OnClick="lkbExportarParadas_OnClick" TabIndex="9"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarParadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadas" OnRowDataBound="gvParadas_RowDataBound" OnPageIndexChanging="gvParadas_PageIndexChanging" ShowFooter="true"  runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="false" TabIndex="10"
ShowHeaderWhenEmpty="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField>
<ItemTemplate>
<img alt="" style="cursor: pointer" src="../Image/plus.png" />
<asp:Panel ID="pnlParadas" runat="server" Style="display: none">
<asp:UpdatePanel ID="upgvProductos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="false" CssClass="gridview" GridLines="Both">
<Columns>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="UniCant" HeaderText="Unidad Cantidad" SortExpression="UniCant" />
<asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso" />
<asp:BoundField DataField="UniPeso" HeaderText="Unidad Peso" SortExpression="UniPeso" />
</Columns>
<FooterStyle CssClass="gridview2footer" />
<HeaderStyle CssClass="gridview2header" />
<RowStyle CssClass="gridview2row" />
<SelectedRowStyle CssClass="gridview2rowselected" />
<AlternatingRowStyle CssClass="gridview2rowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
</asp:GridView>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</asp:Panel>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Secuencia" HeaderText="No." SortExpression="Secuencia" DataFormatString="{0:0}" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Origen" HeaderText="Parada" SortExpression="Origen" />
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita"   DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Llegada" SortExpression="FechaLlegada"  DataFormatString="{0:dd/MM/yyyy HH:mm}"  />
<asp:BoundField DataField="FechaSalida" HeaderText="Fecha Salida" SortExpression="FechaSalida"  DataFormatString="{0:dd/MM/yyyy HH:mm}"  />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Rem1" HeaderText="Rem 1" SortExpression="Rem1" />
<asp:BoundField DataField="Rem2" HeaderText="Rem 2" SortExpression="Rem2" />
<asp:BoundField DataField="Dolly" HeaderText="Dolly" SortExpression="Dolly" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Trans" HeaderText="Transportista" SortExpression="Trans" />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms" />
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaEcabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaParadasServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaResumenServicio" />
<asp:AsyncPostBackTrigger ControlID="btnVistaClasificacion" />
</Triggers>
</asp:UpdatePanel>
</div>