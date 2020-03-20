<%@ Page Title="Reporte Devoluciones y Detalles" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteDevoluciones.aspx.cs" Inherits="SAT.Operacion.ReporteDevoluciones" %>
<%@ Register Src="~/UserControls/wucDevolucionFaltante.ascx" TagName="wucDevolucionFaltante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
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
ConfiguraJQueryReporteDevoluciones();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryReporteDevoluciones() {
$(document).ready(function () {
    //Añadiendo Encabezado Fijo
    $("#<%=gvDevoluciones.ClientID%>").gridviewScroll({
        width: 1190,
        height: 400

    });

    $("#<%=gvDetalles.ClientID%>").gridviewScroll({
        width: 1190,
        height: 400

    });

 
    //Cargando Controles de Fecha
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Cargando Catalogo de Autocompletado
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

//Añadiendo Validación al Evento Click del Boton
$("#<%=btnBuscar.ClientID%>").click(function () {
var isValid1 = !$("#<%=txtNoDevolucion.ClientID%>").validationEngine('validate');
var isValid2;
var isValid3;

//Validando el Control
if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
//Validando Controles
isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
isValid3 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo
isValid2 = true;
isValid3 = true;
}

//Devolviendo Resultados Obtenidos
return isValid1 && isValid2 && isValid3;
});
 



});
}

//Declarando Función de Validación de Fechas
function CompareDates() {
//Obteniendo Valores
var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

//Fecha en Formato MM/DD/YYYY
var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

//Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
if (date1 > date2)
//Mostrando Mensaje de Operación
return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}

//Invocando Función de Configuración
ConfiguraJQueryReporteDevoluciones();

</script>
<div id="encabezado_forma">
<img src="../Image/EnvioRecepcion.png" />
<h1>Devoluciones y Rechazos</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda Por</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoDevolucion">No. Devolución</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoDevolucion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoDevolucion" runat="server" CssClass="textbox validate[custom[positiveNumber]]" TabIndex="1" MaxLength="9"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEnt">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server" TabIndex="2" AutoPostBack="true" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" TabIndex="3" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" TabIndex="4"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoViaje">No. Viaje</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox2x" TabIndex="5"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbCaptura" runat="server" Text="Captura" GroupName="General" Checked="true" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbDevolucion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbDevolucion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbDevolucion" runat="server" Text="Devolución" GroupName="General" TabIndex="8" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbCaptura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="9" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="10" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="11"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="12" CssClass="boton" OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Resultados Obtenidos</h2>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnServicio" Text="Servicio" OnClick="btnVista_OnClick" runat="server" CommandName="Servicio" TabIndex="13" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDetalle" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnDetalle" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDetalle" Text="Detalle" OnClick="btnVista_OnClick" CommandName="Detalle" runat="server" TabIndex="14" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnServicio" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs_300px" style="width:95%">
<asp:UpdatePanel ID="upmtv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvDevoluciones" runat="server" ActiveViewIndex="0">
<asp:View ID="vwDevoluciones" runat="server">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDev">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDev" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDev" runat="server" CssClass="dropdown" TabIndex="15" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoDev_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoDev">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoDev" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoDev" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarDev" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDev" runat="server" Text="Exportar" CommandName="Devoluciones" TabIndex="16" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarDev" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorReporteDevoluciones">
<asp:UpdatePanel ID="upgvDevoluciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDevoluciones" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="17"
OnPageIndexChanging="gvDevoluciones_PageIndexChanging" OnSorting="gvDevoluciones_Sorting"
PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
<asp:TemplateField HeaderText="No. Devolucion" SortExpression="NoDevolucion">
<ItemTemplate>
<asp:LinkButton ID="lkbEditaDevolucion" runat="server" Text='<%# Eval("NoDevolucion") %>' OnClick="lkbEditaDevolucion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width="70px" HeaderStyle-Width="70px"/>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" ItemStyle-Width="130px" HeaderStyle-Width="130px"/>
<asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" Visible="false" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="No. Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="UbicacionDevolucion" HeaderText="Ubicación Devolución" SortExpression="UbicacionDevolucion" />
<asp:BoundField DataField="FechaDevolucion" HeaderText="Fecha Devolución" SortExpression="FechaDevolucion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="NoFactura" HeaderText="No.Factura" SortExpression="NoFactura" />
<asp:BoundField DataField="NoReferencia" HeaderText="No.Referencia" SortExpression="NoReferencia" />
<asp:BoundField DataField="DBT" HeaderText="DBT" SortExpression="DBT" />
<asp:BoundField DataField="LNA" HeaderText="LNA" SortExpression="LNA" />
<asp:BoundField DataField="Cabeceros" HeaderText="Cabeceros" SortExpression="Cabeceros" />
<asp:BoundField DataField="CCR" HeaderText="CCR" SortExpression="CCR" />
<asp:BoundField DataField="RH" HeaderText="RH" SortExpression="RH" />
<asp:BoundField DataField="OrdenRechazo" HeaderText="Orden de Rechazo" SortExpression="OrdenRechazo" />
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbImprimir_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDev" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwDetalles" runat="server">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDet">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDet" runat="server" CssClass="dropdown" TabIndex="18" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoDet_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoDet">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoDet" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDetalles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDet" runat="server" Text="Exportar" CommandName="Detalles" TabIndex="19" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarDet" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorReporteDetalles">
<asp:UpdatePanel ID="upgvDetalles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDetalles" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="20"
OnPageIndexChanging="gvDetalles_PageIndexChanging" OnSorting="gvDetalles_Sorting"
PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
<asp:TemplateField HeaderText="No. Devolucion" SortExpression="NoDevolucion">
<ItemTemplate>
<asp:LinkButton ID="lkbEditaDetalle" runat="server" Text='<%# Eval("NoDevolucion") %>' OnClick="lkbEditaDetalle_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="FechaDevolucion" HeaderText="Fecha Devolución" SortExpression="FechaDevolucion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" Visible="false" />
<asp:BoundField DataField="UbicacionDevolucion" HeaderText="Ubicación Devolución" SortExpression="UbicacionDevolucion" />
<asp:BoundField DataField="CodProd" HeaderText="Código Producto" SortExpression="CodProd" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad"/>
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="RazonDetalle" HeaderText="Razon Detalle" SortExpression="RazonDetalle" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="NoFactura" HeaderText="No.Factura" SortExpression="NoFactura" />
<asp:BoundField DataField="NoReferencia" HeaderText="No.Referencia" SortExpression="NoReferencia" />
<asp:BoundField DataField="DBT" HeaderText="DBT" SortExpression="DBT" />
<asp:BoundField DataField="LNA" HeaderText="LNA" SortExpression="LNA" />
<asp:BoundField DataField="Cabeceros" HeaderText="Cabeceros" SortExpression="Cabeceros" />
<asp:BoundField DataField="CCR" HeaderText="CCR" SortExpression="CCR" />
<asp:BoundField DataField="RH" HeaderText="RH" SortExpression="RH" />
<asp:BoundField DataField="OrdenRechazo" HeaderText="Orden de Rechazo" SortExpression="OrdenRechazo" />
<asp:BoundField DataField="Causa" HeaderText="Causa" SortExpression="Causa" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDet" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnDetalle" />
<asp:AsyncPostBackTrigger ControlID="btnServicio" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana encargada de Gestionar las Devoluciones -->
<div id="modalDevolucionFaltante" class="modal">
<div id="devolucionFaltante" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDevolucion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDevolucion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Devolucion" Text="Cerrar" TabIndex="21">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucDevolucionFaltante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucDevolucionFaltante ID="wucDevolucionFaltante" runat="server" OnClickGuardarDevolucion="wucDevolucionFaltante_ClickGuardarDevolucion" 
OnClickGuardarDevolucionDetalle="wucDevolucionFaltante_ClickGuardarDevolucionDetalle" TabIndex="22"
OnClickEliminarDevolucionDetalle="wucDevolucionFaltante_ClickEliminarDevolucionDetalle" 
OnClickAgregarReferenciasDevolucion="wucDevolucionFaltante_ClickAgregarReferenciasDevolucion"
OnClickAgregarReferenciasDetalle="wucDevolucionFaltante_ClickAgregarReferenciasDetalle" Contenedor="#devolucionFaltante" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana de Referencias -->
<div id="contenedorVentanaReferencias" class="modal">
<div id="ventanaReferencias" class="contenedor_ventana_confirmacion_arriba">
<div class="columna3x">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Referencias" Text="Cerrar" TabIndex="23">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Modulos.png" />
<h2>Referencias</h2>
</div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciaViaje" runat="server" TabIndex="24" OnClickGuardarReferenciaViaje="ucReferenciaViaje_ClickGuardarReferenciaViaje"
    OnClickEliminarReferenciaViaje="ucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Content>
