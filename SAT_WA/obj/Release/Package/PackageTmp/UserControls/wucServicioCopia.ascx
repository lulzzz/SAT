<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucServicioCopia.ascx.cs" Inherits="SAT.UserControls.wucServicioCopia" %>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUCServicioCopia();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryUCServicioCopia() {
// *** Visualización de ventana de servicios maestros  *** //
$(document).ready(function () {
    $("#<%=txtUbicacionOrigenMaestro.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
    $("#<%=txtUbicacionDestinoMaestro.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
    $("#<%=txtClienteMaestro.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
    $("#<%=txtProductoCopia.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=5&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' + '&param2=1',
        appendTo: "<%=this.Contenedor%>"
    });

//Función de Validación de Búsqueda
$("#<%=btnBuscarMaestro.ClientID%>").click(function () {
//Validando Controles
var isValid1 = !$("#<%=txtUbicacionOrigenMaestro.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtUbicacionDestinoMaestro.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtClienteMaestro.ClientID%>").validationEngine('validate');

//Devolviendo Función de Validación
return isValid1 && isValid2 && isValid3;
});

//Calendarios de selección de fechas para copia
$("#<%=txtCitaCargaCopia.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtCitaDescargaCopia.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Función de Validación de Copia de servicio
$('.lkbCopiarServicio').click(function () {
//Validando Controles
var isValid1 = !$("#<%=txtCitaCargaCopia.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCitaDescargaCopia.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtProductoCopia.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtCantidadProductoCopia.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtPesoProductoCopia.ClientID%>").validationEngine('validate');

//Devolviendo Función de Validación
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
});

});
}

//Invocando Función de COnfiguración
ConfiguraJQueryUCServicioCopia();
</script>
<div class="columna2x">
<div class="header_seccion" style="float:left;">
<img alt="CopiaServicioMaestro" src="../Image/Buscar.png" />
<h2>Búsqueda de Servicio Maestro</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtClienteMaestro">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtClienteMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtClienteMaestro" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionOrigenMaestro">Origen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionOrigenMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionOrigenMaestro" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionDestinoMaestro">Destino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionDestinoMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionDestinoMaestro" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnCancelarBusquedaMaestro" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarBusquedaMaestro_Click" />
</div>
<div class="controlBoton">
<asp:Button ID="btnBuscarMaestro" runat="server" Text="Buscar" CssClass="boton" CommandName="Buscar" OnClick="btnBuscarMaestro_Click" />
</div>
</div>
</div>
<div class="columna" style ="width:310px">
<div class="header_seccion" style="float:left;">
<img alt="CopiaServicioMaestro" src="../Image/CrearDoc.png" />
<h2>Datos para Copia de Servicio</h2>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtCitaCargaCopia">Cita de Carga</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtCitaCargaCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCitaCargaCopia" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtCitaDescargaCopia">Cita de Descarga</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtCitaDescargaCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCitaDescargaCopia" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtNoViajeCopia">No. de Viaje</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoViajeCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoViajeCopia" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtNoConfirmacionViajeCopia">No. Confirmación</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoConfirmacionViajeCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoConfirmacionViajeCopia" runat="server" CssClass="textbox" MaxLength="50"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna" style="width:310px">
<div class="header_seccion" style="float:left;">
<img alt="CopiaServicioMaestro" src="../Image/carga.png" />
<h2>Información de Producto</h2>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtProductoCopia">Producto</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtProductoCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProductoCopia" runat="server" CssClass="textbox validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtCantidadProductoCopia">Cantidad</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtCantidadProductoCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidadProductoCopia" runat="server" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="13"></asp:TextBox>
<asp:DropDownList ID="ddlUnidadCantidadProductoCopia" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta"><label for="txtPesoProductoCopia">Peso</label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtPesoProductoCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPesoProductoCopia" runat="server" CssClass="textbox_50px validate[custom[positiveNumber]]" MaxLength="13"></asp:TextBox>
<asp:DropDownList ID="ddlUnidadPesoProductoCopia" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="header_seccion">
<h2>Servicios Encontrados</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoServiciosMaestros">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoServiciosMaestros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoServiciosMaestros" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoServiciosMaestros_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoServiciosMaestros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoServiciosMaestros" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosMaestros" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServiciosMaestros" runat="server" TabIndex="5"
OnClick="lkbExportarServiciosMaestros_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServiciosMaestros" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvServiciosMaestros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosMaestros" runat="server" CssClass="gridview" Width="100%"
OnPageIndexChanging="gvServiciosMaestros_PageIndexChanging" AllowPaging="True" AllowSorting="True"
ShowFooter="True" AutoGenerateColumns="False" PageSize="25" OnSorting="gvServiciosMaestros_Sorting">
<Columns>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbCopiar" CssClass="lkbCopiarServicio" runat="server" OnClick="lkbCopiar_Click">Copiar</asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarMaestro" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarBusquedaMaestro" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServiciosMaestros" />
</Triggers>
</asp:UpdatePanel>
</div>