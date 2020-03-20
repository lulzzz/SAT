<%@ Page Title="Reporte de Servicios" Language="C#" AutoEventWireup="true" CodeBehind="VisorDocumentacion.aspx.cs"  MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Documentacion.VisorDocumentacion" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="tectos" TagName="wucReferenciaViaje" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Referencia a Hoja de Estilos requeridas -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos JQuery -->
<link  href="../CSS/jquery.datetimepicker.css" rel ="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript"  src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReporteServicios();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryReporteServicios() {
$(document).ready(function () {

$('#<%=gvServicios.ClientID %>').gridviewScroll({
width: document.getElementById("reporteServicios").offsetWidth - 15,
height: 450,
freezesize: 4

});


//Validación 
var validacionReporteServicio = function () {

var isValidP1 = !$("#<%=txtOrigen.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtDestino.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValidP4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
var isValidP5 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');

return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteServicio);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

/* Autocompleta origen, destino y cliente */
$("#<%=txtOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryReporteServicios();
</script>
<div id="encabezado_forma">
<img src="../Image/Grafica32px.png" />
<h1>Reporte de Servicios</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros de Busqueda</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">
No Servicio
</label>
</div>
<div class="control">
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox ]" TabIndex="1" MaxLength="30"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOrigen">Lugar Carga</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtOrigen" runat="server" CssClass="textbox2x validation[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDestino">Lugar Descarga</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtDestino" runat="server" CssClass="textbox2x validation[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validation[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtPorte">Porte</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtPorte" runat="server" CssClass="textbox2x" TabIndex="6"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="7"></asp:TextBox>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlRegion">Región</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlRegion" runat="server" CssClass="dropdown2x" TabIndex="8" ></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlTipoServicio">Tipo Servicio</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown2x" TabIndex="9" ></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlAlcance">Alcance Servicio</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlAlcance" runat="server" CssClass="dropdown2x" TabIndex="10" ></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar x Fecha"
Checked="false" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" TabIndex="11" />
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaCarga" runat="server" CssClass="label" Text="Cita Carga" GroupName="FiltroFecha" TabIndex="12" Checked="true" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaDescarga" runat="server" CssClass="label" Text="Cita Descarga" GroupName="FiltroFecha" TabIndex="13" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>   
</div>
<div class="renglon2x">
<div class="etiqueta"></div>
<div class="control">
<asp:UpdatePanel ID="uprdbInicioServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbInicioServicio" runat="server" CssClass="label" Text="Inicio Servicio" GroupName="FiltroFecha" TabIndex="14" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>    
<div class="control">
<asp:UpdatePanel ID="uprdbFinServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbFinServicio" runat="server" CssClass="label" Text="Fin Servicio" GroupName="FiltroFecha" TabIndex="15" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div> 
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Desde</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Hasta</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="17" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="18" />
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Servicios Encontrados</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewServicios">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewServicios" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewServicios_SelectedIndexChanged" TabIndex="19" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewServicios">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewServicios" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar" TabIndex="20" OnClick="lkbExportarServicios_Click"></asp:LinkButton>
</div>
</div>
<div id="reporteServicios" class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServicios" CssClass="gridview" OnPageIndexChanging="gvServicios_PageIndexChanging" OnSorting="gvServicios_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" TabIndex="21"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" ItemStyle-Width="42px" HeaderStyle-Width="42px" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width= "70px" HeaderStyle-Width="70px"/>
<asp:BoundField DataField="CopiaDe" HeaderText="Copia De" SortExpression="CopiaDe"  HeaderStyle-Width="71px" ItemStyle-Width="71px"/>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" HeaderStyle-Width="125px" ItemStyle-Width="125px"/>
<asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" ItemStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"/>
<asp:TemplateField HeaderText="Referencia" SortExpression="ReferenciaCliente">
<ItemTemplate>
<asp:LinkButton ID="lkbNoViaje" runat="server" Text='<%# Eval("ReferenciaCliente") %>' OnClick="lkbReferencias_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Pedido" SortExpression="Pedido">
<ItemTemplate>
<asp:LinkButton ID="lkbPedido" runat="server" Text='<%# Eval("Pedido") %>' OnClick="lkbReferencias_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FechaDocumentacion" HeaderText="Fecha Documentación" SortExpression="FechaDocumentacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="DocumentadoPor" HeaderText="Documentado Por" SortExpression="DocumentadoPor" ItemStyle-HorizontalAlign="Left" />
<asp:BoundField DataField="LugarCarga" HeaderText="Lugar Carga" SortExpression="LugarCarga" HeaderStyle-Width="90px" ItemStyle-Width="90px" />
<asp:BoundField DataField="CitaCarga" HeaderText="Cita Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="LlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Carga" SortExpression="LlegadaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CitaCargaEnTiempo" HeaderText="Cita En Tiempo" SortExpression="CitaCargaEnTiempo" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="RazonTardeCarga" HeaderText="Razón Tarde" SortExpression="RazonTardeCarga" />
<asp:BoundField DataField="SalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Salida Carga" SortExpression="SalidaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstanciaCarga" HeaderText="Estancia Carga" SortExpression="*EstanciaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="LugarDescarga" HeaderText="Lugar Descarga" SortExpression="LugarDescarga" />
<asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="LlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Descarga" SortExpression="LlegadaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CitaDescargaEnTiempo" HeaderText="Cita En Tiempo" SortExpression="CitaDescargaEnTiempo" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="FinViaje" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fin Viaje" SortExpression="FinViaje">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstanciaDescarga" HeaderText="Estancia Descarga" SortExpression="*EstanciaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="RazonTardeDescarga" HeaderText="Razón Tarde " SortExpression="RazonTardeDescarga" />
<asp:BoundField DataField="Region" HeaderText="Región" SortExpression="Region" />
<asp:BoundField DataField="TipoServicio" HeaderText="Tipo Servicio" SortExpression="TipoServicio" />
<asp:BoundField DataField="Alcance" HeaderText="Alcance" SortExpression="Alcance" />
<asp:BoundField DataField="EstatusFactura" HeaderText="Estatus Factura" SortExpression="EstatusFactura" />
<asp:BoundField DataField="MotivoNoFacturable" HeaderText="Motivo No Facturable" SortExpression="MotivoNoFacturable" />
<asp:BoundField DataField="SubTotal" DataFormatString="{0:c}" HeaderText="SubTotal" SortExpression="SubTotal">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Retenido" DataFormatString="{0:c}" HeaderText="Retenido" SortExpression="Retenido">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Trasladado" DataFormatString="{0:c}" HeaderText="Trasladado" SortExpression="Trasladado">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" DataFormatString="{0:c}" HeaderText="Total" SortExpression="Total">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Tarifa" HeaderText="Tarifa" SortExpression="Tarifa" />
<asp:BoundField DataField="FacturaElectronica" HeaderText="Factura Electrónica" SortExpression="FacturaElectronica" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="FacturaGlobal" HeaderText="Factura Global" SortExpression="FacturaGlobal" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="EstatusEvidencia" HeaderText="Estatus Evidencia" SortExpression="EstatusEvidencia" />
<asp:BoundField DataField="MontoAplicado" DataFormatString="{0:c}" HeaderText="Monto Aplicado" SortExpression="MontoAplicado">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Saldo" DataFormatString="{0:c}" HeaderText="Saldo" SortExpression="Saldo">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Liquidado" DataFormatString="{0:c}" HeaderText="Liquidado" SortExpression="Liquidado">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Liquidaciones" HeaderText="Liquidaciones" SortExpression="Liquidaciones">
<ItemStyle HorizontalAlign="Left" />
</asp:BoundField>
<asp:BoundField DataField="MontoDepositos" DataFormatString="{0:c}" HeaderText="Monto Depositos" SortExpression="MontoDepositos">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="LitrosDiesel" HeaderText="Litros Diesel" SortExpression="LitrosDiesel">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoDiesel" DataFormatString="{0:c}" HeaderText="Monto Diesel" SortExpression="MontoDiesel">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbBitacora_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewServicios" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModal" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- -->
<div id="contenedorVentanaReferenciaViaje" class="modal">
<div id="ventanaReferenciaViaje" class="contenedor_ventana_confirmacion_arriba" style="min-width:621px; width:621px; padding-bottom:5px;">
<div class="columna3x">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModal" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVentanaModal" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Referencias Servicio</h2>
</div>
<asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true" TabIndex="28"
OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

</asp:Content>
