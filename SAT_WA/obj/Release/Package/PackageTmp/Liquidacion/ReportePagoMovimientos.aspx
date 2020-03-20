<%@ Page Title="Reporte de Pago de Movimientos" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportePagoMovimientos.aspx.cs" Inherits="SAT.Liquidacion.ReportePagoMovimientos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Selector de Fecha JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReportePagoMovtos();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryReportePagoMovtos() {
$(document).ready(function () {

    //Añadiendo Encabezado Fijo
    $("#<%=gvLiquidacion.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorReportePagoMovimientos").offsetWidth - 15,
        height: 400,
        freezesize: 4
    });


//Validación 
var validacionReporteLiquidacion = function () {

var isValidP1 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteLiquidacion);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Autocomplete de operadores por compañía
$("#<%=txtOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>' });

});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryReportePagoMovtos();
</script>
<div id="encabezado_forma">
<h1>Reporte de Pago a Operador</h1>
</div>
<div class="contenedor_seccion_completa">

<div class="columna3x">
<div class="header_seccion" style="width:90%;">
<img src="../Image/Buscar.png" />
<h2>Filtros de Búsqueda</h2>
</div>
<div style="float:left;"><br />
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOperador">Operador</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
<div class="validador"></div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Fecha Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Utilizar Filtrado de Fechas."
Checked="false" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Fecha Final</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="9" />
</div>
</div>
</div>
</div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/ResumenReporte.png" />
<h2>Resumen por Operador</h2>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumen" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="50"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvResumen_PageIndexChanging">
<Columns>
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" ItemStyle-Width="60%" >
<ItemStyle Width="60%" />
</asp:BoundField>
<asp:BoundField DataField="NoServicios" HeaderText="NoServicios" SortExpression="NoServicios" ItemStyle-Width="20%" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" Width="20%" />
</asp:BoundField>
<asp:BoundField DataField="NoMovimientos" HeaderText="NoMovimientos" SortExpression="NoMovimientos" ItemStyle-Width="20%" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" Width="20%" />
</asp:BoundField>
<asp:BoundField DataField="Litros" DataFormatString="{0:f2}" HeaderText="Litros" SortExpression="Litros">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Diesel" DataFormatString="{0:c}" HeaderText="Diesel" SortExpression="Diesel">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Anticipos" DataFormatString="{0:c}" HeaderText="Anticipos" SortExpression="Anticipos">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Comprobaciones" DataFormatString="{0:c}" HeaderText="Comprobaciones" SortExpression="Comprobaciones">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Pagos" DataFormatString="{0:c}" HeaderText="Pagos" SortExpression="Pagos">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
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
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
    
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Servicios y Movimientos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewLiquidacion">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewLiquidacion" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewLiquidacion_OnSelectedIndexChanged" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewLiquidacion">Ordenado Por:</label>
</div>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewLiquidacion" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelLiquidacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelLiquidacion" runat="server" Text="Exportar" OnClick="lkbExportarExcelLiquidacion_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorReportePagoMovimientos">
<asp:UpdatePanel ID="upgvLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLiquidacion" CssClass="gridview" OnPageIndexChanging="gvLiquidacion_OnpageIndexChanging" OnSorting="gvLiquidacion_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" TabIndex="14"
PageSize="25" Width="100%">
<Columns>
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" Text='<%# Eval("NoServicio") %>' OnClick="lkbReferencias_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IdMovimiento" HeaderText="Movimiento" SortExpression="IdMovimiento" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" HeaderStyle-Width="120px" ItemStyle-Width="120px"/>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen"  HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaFin" HeaderText="FechaFin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusDocumentos" HeaderText="Estatus Doctos." SortExpression="EstatusDocumentos" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Litros" DataFormatString="{0:f2}" HeaderText="Litros" SortExpression="Litros">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Diesel" DataFormatString="{0:c}" HeaderText="Diesel" SortExpression="Diesel">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Anticipos" DataFormatString="{0:c}" HeaderText="Anticipos" SortExpression="Anticipos" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Comprobaciones" HeaderText="Comprobaciones" SortExpression="Comprobaciones" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Pagos" DataFormatString="{0:c}" HeaderText="Pagos" SortExpression="Pagos">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Pagado" HeaderText="Pagado" SortExpression="Pagado">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Liquidado" HeaderText="Liquidado" SortExpression="Liquidado">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
</Triggers>
</asp:UpdatePanel>
</div>

    
<!-- Ventana Referencias de Viaje -->
<div id="contenedorVentanaReferenciasViaje" class="modal">
<div id="ventanaReferenciasViaje" class="contenedor_ventana_confirmacion" style="width:300px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" CommandName="ReferenciasViaje" OnClick="lnkCerrarReferencias_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Referencias del Viaje</h2> </div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciasViaje" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
