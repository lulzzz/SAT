<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteHistorialAndenes.aspx.cs" Inherits="SAT.ControlPatio.ReporteHistorialAndenes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
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
ConfiguraHistorialCajones();
}
}
//Función de Configuración
function ConfiguraHistorialCajones(){
$(document).ready(function () {
                
var validaBusqueda = function () {

var isValid1 = !$("#<%=txtFechaIni.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');

return isValid1 && isValid2;
}
//Añadiendo Función al Evento Click
$("#<%=btnBuscar.ClientID%>").click(validaBusqueda);

//Cargando Control DateTimePicker
$("#<%=txtFechaIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});    
//Cargando Control DateTimePicker
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

});
}
        
//Invocando Función de Configuración
ConfiguraHistorialCajones();
</script>    
<div id="encabezado_forma">
<img src="../Image/ControlAcceso.png" />
<h1>Historial de Andenes</h1>
</div>
<section class="fila_indicador">        
<a href="#" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="upplblCajonesDisp" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCajonesDisp"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorUnidadesPatio.png" />
</div>
<div class="leyenda_indicador">
Andenes Disponibles
</div>           
</a>
<a href="#" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblCajonesOcup" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCajonesOcup"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorUnidadesPatio.png" />
</div>
<div class="leyenda_indicador">
Andenes Ocupados
</div>           
</a>
<a href="#" class="indicador_texto">
<div class="texto_indicador">
<asp:UpdatePanel runat="server" ID="uplblUtilizacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUtilizacion"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/EntradasSalidas.png" />
</div>
<div class="leyenda_indicador">
% Utilizacion
</div>           
</a>
<a href="#" class="indicador_texto" >
<div class="texto_indicador">
<asp:UpdatePanel runat="server" ID="uplblTiempoPromedio" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempoPromedio"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorTiempo.png" />
</div>
<div class="leyenda_indicador">
Tiempo Promedio
</div>           
</a>
<div class="indicador_actualiza">
<asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_Click">
<img src="../Image/Reload.png" />
</asp:LinkButton> 
</div>                 
</section>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlPatio">Patio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPatio" runat="server" TabIndex="1" CssClass="dropdown" 
OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Fecha de Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaIni" runat="server" TabIndex="2" CssClass="textbox validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Fecha de Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" TabIndex="4" CssClass="textbox validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="2" CssClass="boton"
OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_controles">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAndenes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" 
OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvAndenes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAndenes" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
OnSorting="gvAndenes_Sorting" OnPageIndexChanging="gvAndenes_PageIndexChanging"
OnRowDataBound="gvAndenes_RowDataBound"
PageSize="25" Width="100%" CssClass="gridview">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField HeaderText="Estatus">
<ItemTemplate>
<asp:Image ID="imgEstatus" runat="server" Height="20px" ImageAlign="AbsMiddle" Width="20px"
ImageUrl="~/Image/EntidadTiempoOK.png" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField HeaderText="Anden" SortExpression="Anden">
<ItemTemplate>
<asp:LinkButton ID="lnkBitacora" runat="server" Text='<%# Eval("Anden") %>' OnClick="lnkBitacora_Click">
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus en Patio" SortExpression="Estatus" />
<asp:BoundField DataField="TiempoEstatus" HeaderText="Tiempo de Operación" SortExpression="TiempoEstatus" />
<asp:BoundField DataField="Indicador" HeaderText="Indicador" SortExpression="Indicador" Visible="false" />
<asp:BoundField DataField="UtilizacionAnden" HeaderText="Utilización de Anden" SortExpression="UtilizacionAnden" />
<asp:BoundField DataField="TiempoPromedio" HeaderText="Tiempo Promedio" SortExpression="TiempoPromedio" />
<asp:BoundField DataField="NoEvt" HeaderText="No. Eventos" SortExpression="NoEvt" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_controles">
<div class="columna">
<asp:UpdatePanel ID="upchtEntidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtEntidades" runat="server" BackColor="Transparent">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="upgvEstatusEntidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEstatusEntidades" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="NoUnidades" HeaderText="No. Unidades" SortExpression="NoUnidades" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenidoBitacoraUnidades" class="modal">
<div id="bitacoraUnidades" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Bitacora<br />
Eventos de la Unidad
</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoBit">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoBit" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoBit_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoBit">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoBit" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacora" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarBit" runat="server" Text="Exportar" 
OnClick="lnkExportarBit_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarBit" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_unidad_agrupada">                
<asp:UpdatePanel ID="upgvBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvBitacora" runat="server" CssClass="gridview" 
OnSorting="gvBitacora_Sorting" OnPageIndexChanging="gvBitacora_PageIndexChanging"
AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
ShowFooter="true" PageSize="25" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="FecIni" HeaderText="Fecha de Inicio" SortExpression="FecIni" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FecFin" HeaderText="Fecha de Termino" SortExpression="FecFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoBit" />
<asp:AsyncPostBackTrigger ControlID="gvAndenes" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon_boton_salir">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCerrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CssClass="boton_cancelar"
OnClick="btnCerrar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAndenes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</asp:Content>
