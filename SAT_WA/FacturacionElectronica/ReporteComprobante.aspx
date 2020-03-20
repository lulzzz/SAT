<%@ Page Title="Reporte Comprobante" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteComprobante.aspx.cs" Inherits="SAT.FacturacionElectronica.ReporteComprobante" %>
<%@ Register Src="~/UserControls/wucEmailCFDI.ascx" TagPrefix="uc1" TagName="wucEmailCFDI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryComprobante();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryComprobante() {

//Validación campos Hoja de Instrucción
$(document).ready(function () {

$('#<%=gvComprobantes.ClientID %>').gridviewScroll({
width: 1260,
height: 450
});

//Función de validación 
var validacionComprobante = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtReceptor.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3
};

    //Función de validación de campos
    var validacionCancelacion = function (evt) {
        var isValidP1 = !$("#<%=txtMotivo.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
//Botón buscra
    $("#<%= btnBuscar.ClientID %>").click(validacionComprobante);
    //Botón Aceptar Cancelación 
    $("#<%= btnAceptarCancelacionCFDI.ClientID %>").click(validacionCancelacion); 
//Añadiendo funcion de Calendario
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Añadiendo Función de Autocompletado al Control de Receptor
$("#<%=txtReceptor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtUsuarioTimbra.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=33&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryComprobante();
</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Reporte Comprobante</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar comprobantes por</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtReceptor">Receptor:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" AutoPostBack="True"
TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtSerie">Serie:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server"  CssClass="textbox_50px validate[custom[onlyLetterSp]]" MaxLength="10" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label class="Label" for="txtFolio">Folio:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_50px validate[custom[onlyNumberSp]]"  MaxLength="9" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEstatus">Estatus:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" TabIndex="4" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="celda_etiqueta">
<asp:CheckBox ID="chkGenerado" runat="server" Text="Generado" TabIndex="5" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlTipo">Tipo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x" TabIndex="5">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbExpedicion" runat="server" TabIndex="6"
Text="Expedición" GroupName="SeleccionarTipo" AutoPostBack="true" Checked="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCaptura" runat="server" TabIndex="7"
Text="Captura" GroupName="SeleccionarTipo" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCancelacion" runat="server" TabIndex="8"
Text="Cancelación" GroupName="SeleccionarTipo" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label">Fecha Inicial:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="updpFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label">Fecha Final:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="updpFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtUsuarioTimbra">Timbrado por:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUsuarioTimbra" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUsuarioTimbra" runat="server" MaxLength="150" CssClass="textbox2x validate[custom[IdCatalogo]]" AutoPostBack="True"
TabIndex="11"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_OnClick" Text="Buscar"
TabIndex="12" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnLimpiar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLimpiar" runat="server" OnClick="btnLimpiar_OnClick" Text="Limpiar"
TabIndex="13" CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Facturas</h2>
</div>
<div class="renglon3x" style="width: auto">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañogvComprobantes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañogvComprobantes" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamañogvComprobantes_SelectedIndexChanged" TabIndex="14">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCriteriogvComprobantes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblCriteriogvComprobantes" runat="server" Text="" CssClass="Label"> </asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportargvComprobantes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="div_orden_gridviewS">
<asp:LinkButton ID="lkbExportargvComprobantes" runat="server"
OnClick="lkbExportarExcel_OnClick" TabIndex="15" CommandName="Facturas">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportargvComprobantes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" >
<asp:UpdatePanel ID="upgvComprobantes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvComprobantes" runat="server" TabIndex="16"
AllowPaging="True" AllowSorting="True"
OnPageIndexChanging="gvComprobantes_OnPageIndexChanging"
OnSorting="gvComprobantes_OnSorting"
ShowFooter="True" PageSize="25" AutoGenerateColumns="False" OnRowDataBound="gvComprobantes_RowDataBound">
<Columns>
<asp:TemplateField>
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="15" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="16" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField SortExpression="UUID" HeaderText="Folio Fiscal">
<ItemTemplate>
<asp:Label ID="lblUUIDComp" runat="server" Text='<%#TSDK.Base.Cadena.InvierteCadena(TSDK.Base.Cadena.TruncaCadena(TSDK.Base.Cadena.InvierteCadena(Eval("UUID").ToString()), 12, "...")) %>'
ToolTip='<%#Eval("UUID")%>'></asp:Label>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="OrigenCFDI" HeaderText="Origen CFDI" SortExpression="OrigenCFDI" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Receptor" HeaderText="Receptor" SortExpression="Receptor" />
<asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC" />
<asp:TemplateField SortExpression="FleteIdentificacion" HeaderText="Flete No. Identificación">
<ItemTemplate>
<asp:Label ID="lblFleteIdentificacionComp" runat="server" Text='<%#TSDK.Base.Cadena.TruncaCadena(Eval("FleteIdentificacion").ToString(), 57, "...") %>'
ToolTip='<%#Eval("FleteIdentificacion")%>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaExpedicion" HeaderText="Fecha de Expedicion" SortExpression="FechaExpedicion" DataFormatString="{0:yyy-MM-dd HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaCancelacion" HeaderText="Fecha de Cancelacion" SortExpression="FechaCancelacion" DataFormatString="{0:yyy-MM-dd HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CanceladoPor" HeaderText="Cancelado por" SortExpression="CanceladoPor"> <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MotivoCancelacion" HeaderText="Motivo" SortExpression="MotivoCancelacion"> <ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Flete" HeaderText="Flete" SortExpression="Flete" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Renta" HeaderText="Renta" SortExpression="Renta" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Estadias" HeaderText="Estadias" SortExpression="Estadias" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Casetas" HeaderText="Casetas" SortExpression="Casetas" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Descuento" HeaderText="Descuento" SortExpression="Descuento" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVATrasladado" HeaderText="IVA Trasladado" SortExpression="IVATrasladado" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVARetenido" HeaderText="IVA Retenido" SortExpression="IVARetenido" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Impuestos" HeaderText="Impuestos" SortExpression="Impuestos" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TipoCambio" HeaderText="Tipo de Cambio" SortExpression="TipoCambio" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" DataFormatString="{0:c}" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TimbradoPor" HeaderText="Timbrado por" SortExpression="TimbradoPor"/>
<asp:TemplateField HeaderText="Timbrar FE"  SortExpression="TimbrarFE">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbCancelarCFDI" runat="server"    OnClick="lkbCancelarCFDI_Click"  Text="Cancelar CFDI" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="upAccesorios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacoraDetalle" runat="server" TabIndex="17" CommandName="Bitacora" OnClick="lkbDetalles_Click">Bitácora</asp:LinkButton>
<br />
<asp:LinkButton ID="lkbReferenciasDetalle" runat="server" TabIndex="18" OnClick="lkbDetalles_Click"
CommandName="Referencias">Referencias</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacoraDetalle" />
<asp:PostBackTrigger ControlID="lkbReferenciasDetalle" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañogvComprobantes" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEmail" runat="server" TabIndex="19" OnClick="lkbDetalles_Click"
CommandName="Email">E-mail</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbXML" runat="server" TabIndex="19"  OnClick="lkbDetalles_Click"
CommandName="XML">XML</asp:LinkButton>
<asp:LinkButton ID="lkbPDF" runat="server" TabIndex="20"   OnClick="lkbDetalles_Click"
CommandName="PDF">PDF</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbXML" />
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
</div>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañogvComprobantes" />
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacionCFDI" />
</Triggers>
</asp:UpdatePanel>    
</div>
</div>
<div class="seccion_controles">
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
Exportar Archivos
<div class="renglon2x">
<div class="celda_etiqueta">
<label class="Label" for="rdbPDF">
</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uprdbPDF">
<ContentTemplate>
<asp:CheckBox ID="chkPDF" Text="PDF" runat="server" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uprdbXML">
<ContentTemplate>
<asp:CheckBox ID="chkXML" Text="XML" runat="server" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnExportarPDF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnExportar" runat="server" TabIndex="21" CssClass="boton" Text="Exportar" OnClick="btnExportar_OnClick" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="btnExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana Confirmación Email -->
<div id="contenidoConfirmacionEmail" class="modal">
<div id="confirmacionEmail" class="contenedor_ventana_confirmacion_arriba">
<asp:UpdatePanel ID="upwucEmailCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucEmailCFDI runat="server" ID="wucEmailCFDI" OnBtnEnviarEmail_Click="wucEmailCFDI_BtnEnviarEmail_Click" OnLkbCerrarEmail_Click="wucEmailCFDI_LkbCerrarEmail_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" />
</Triggers>
</asp:UpdatePanel>    
</div>
</div>
<!-- Ventana Confirmación Cancelación CFDI -->
<div id="contenidoConfirmacionCancelacionCFDI" class="modal">
<div id="confirmacionCancelacionCFDI" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h3>Cancelar Factura Electrónica</h3>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upmtvCancelacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvCancelacionCFDI" runat="server">
<asp:View ID="vwFacturaSinAplicaciones" runat="server">
<div class="renglon2x">
<label class="mensaje_modal">
¿Realmente desea Cancelar la Factura Electrónica?
</label>
</div>
</asp:View>
<asp:View ID="vwFacturaConAplicaciones" runat="server">
<div class="renglon2x">
<label class="mensaje_modal">
Las siguientes Aplicaciones seran Eliminadas. ¿Desea Continuar?
</label>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvAplicaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAplicaciones" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false"
CssClass="gridview" ShowFooter="true" Width="100%">
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
<asp:BoundField DataField="FacturaFicha" HeaderText="Factura/Ficha" SortExpression="FacturaFicha" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobantes" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivo">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine"  Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel></div></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarCancelacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarCancelacionCFDI" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarCancelacionCFDI_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCancelacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelacionCFDI" runat="server" OnClick="btnAceptarCancelacionCFDI_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

</asp:Content>
