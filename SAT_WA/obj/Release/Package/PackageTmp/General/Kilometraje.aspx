<%@ Page Title="Kilometraje" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Kilometraje.aspx.cs" Inherits="SAT.General.Kilometraje" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagName="wucKilometraje" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
//Invocando Función de Configuración
ConfiguraJQueryKilometraje();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryKilometraje() {
$(document).ready(function () {

//Catalogos de Ubicación
$("#<%=txtUbicacionOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
$("#<%=txtUbicacionDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
            
//Catalogos de Ubicación
$("#<%=txtCiudadOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
$("#<%=txtCiudadDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=9&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'});
            
//Cargando Catalogo AutoCompleta
$("#<%=txtCompania.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=4' });

//Validando Controles
$("#<%=btnBuscar.ClientID%>").click(function () {
//Validando Control
var isValid1 = !$("#<%=txtUbicacionOrigen.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtUbicacionDestino.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtCiudadOrigen.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtCiudadDestino.ClientID%>").validationEngine('validate');

//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
})
});
}

//Invocando Función de Configuración
ConfiguraJQueryKilometraje();

</script>
<div id="encabezado_forma">
<img src="../Image/Paradas.png" />
<h1>Kilometraje</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompania">Compañía</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionOrigen">Lugar Origen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionOrigen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCiudadOrigen">Ciudad Origen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCiudadOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadOrigen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionDestino">Lugar Destino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionDestino" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCiudadDestino">Ciudad Destino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCiudadDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadDestino" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="boton_cancelar" OnClick="btnNuevo_Click"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click"  />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>    
</div>

<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaKilometraje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaKilometraje" Text="Kilometraje Registrado" OnClick="btnVista_Click" CommandName="Kilometraje" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaPendiente" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnVistaPendiente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVistaPendiente" Text="Kilometraje Pendiente" OnClick="btnVista_Click" runat="server" CommandName="Pendientes" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaKilometraje" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvKilometrajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvKilometrajes" runat="server" ActiveViewIndex="0">
<asp:View ID="vwKilometrajeExistente" runat="server">
<br />
<div class="renglon100per">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" ></asp:DropDownList>
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado Por</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvKilometrajes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click" CommandName="Kilometraje"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura">
<asp:UpdatePanel ID="upgvKilometrajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvKilometrajes" runat="server" AllowPaging="True" AllowSorting="True"
CssClass="gridview" ShowFooter="True" OnSorting="gvKilometrajes_Sorting" 
OnPageIndexChanging="gvKilometrajes_PageIndexChanging" AutoGenerateColumns="False" Width="100%" PageSize="25">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Km's Reales" SortExpression="KmsReales">
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionar" runat="server" Text='<%#Eval("KmsReales") %>' OnClick="lnkSeleccionar_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="KmsMaps" HeaderText="Km's (Maps)" SortExpression="KmsMaps" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoMaps" HeaderText="Tiempo (Maps)" SortExpression="TiempoMaps" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
<asp:AsyncPostBackTrigger ControlID="btnVistaKilometraje" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwKilometrajePendiente" runat="server">
<br />
<div class="renglon100per">
<div class="etiqueta_50px">
<label for="ddlTamanoPendiente">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoPendiente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoPendiente" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoPendiente_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoPendiente">Ordenado Por</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoPendiente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoPendiente" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvKilometrajesPendientes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarPendiente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarPendiente" runat="server" Text="Exportar" CommandName="Pendiente" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarPendiente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura">
<asp:UpdatePanel ID="upgvKilometrajesPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvKilometrajesPendientes" runat="server" AllowPaging="True" AllowSorting="True"
CssClass="gridview" ShowFooter="True" OnSorting="gvKilometrajesPendientes_Sorting" 
OnPageIndexChanging="gvKilometrajesPendientes_PageIndexChanging" AutoGenerateColumns="False" Width="100%" PageSize="25">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Km's Reales" SortExpression="KmsReales">
<ItemTemplate>
<asp:LinkButton ID="lkbEditarPendiente" runat="server" Text='<%#Eval("KmsReales") %>' OnClick="lkbPendientes_Click" CommandName="Editar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="KmsMaps" HeaderText="Km's (Maps)" SortExpression="KmsMaps">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoMaps" HeaderText="Tiempo (Maps)" SortExpression="TiempoMaps">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Pendientes">
<ItemTemplate>
<asp:LinkButton ID="lkbVerMovimientos" runat="server" OnClick="lkbPendientes_Click" Text="Ver Movimientos" CommandName="VerPendientes"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPendiente" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnVistaPendiente" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarMovimientos" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnVistaKilometraje" />
<asp:AsyncPostBackTrigger ControlID="btnVistaPendiente" />
</Triggers>
</asp:UpdatePanel>
</div>

<!-- VENTANA MODAL DE ACTUALIZACIÓN (INSERCIÓN/EDICIÓN) KILOMETRAJE -->
<div id="contenedorVentanaKilometraje" class="modal">
<div id="ventanaKilometraje" class="contenedor_ventana_confirmacion_arriba">        
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrar" OnClick="lkbCerrar_Click" runat="server" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucKilometraje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucKilometraje ID="ucKilometraje" runat="server" OnClickGuardar="ucKilometraje_ClickGuardar" Contenedor="#ventanaKilometraje"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevo" />
<asp:AsyncPostBackTrigger ControlID="gvKilometrajes" />
<asp:AsyncPostBackTrigger ControlID="gvKilometrajesPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE ACTUALIZACIÓN DE MOVIMIENTOS SIN KILOMETRAJE -->
<div id="ventanaMovimientosPendientesModal" class="modal">
<div id="ventanaMovimientosPendientes" class="contenedor_modal_seccion_completa_arriba">        
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarMovimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarMovimientos" OnClick="lkbCerrarMovimientos_Click" runat="server">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Calculadora.png" />
<h2>Movimientos sin Kilometraje Actualizado</h2>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvMovimientosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvMovimientosPendientes" runat="server" AllowPaging="True"
CssClass="gridview" ShowFooter="True" PageSize="25"
OnPageIndexChanging="gvMovimientosPendientes_PageIndexChanging" AutoGenerateColumns="False" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="FechaSalida" HeaderText="FechaSalida" SortExpression="FechaSalida" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="FechaLlegada" HeaderText="FechaLlegada" SortExpression="FechaLlegada" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Kms">
<ItemTemplate>
<asp:LinkButton ID="lkbActualizarKM" runat="server" Text="Actualizar" OnClick="lkbMovimientoPendiente_Click" CommandName="Actualizar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvKilometrajesPendientes" />
<asp:AsyncPostBackTrigger ControlID="ucKilometraje" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon3x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnActualizarMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnActualizarMovimientos" runat="server" CssClass="boton" Text="Actualizar Todos" OnClick="btnActualizarMovimientos_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</asp:Content>