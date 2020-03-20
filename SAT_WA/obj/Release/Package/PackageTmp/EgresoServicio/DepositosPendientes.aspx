<%@ Page Title="Depositos Pendientes" Language="C#" AutoEventWireup="true" CodeBehind="DepositosPendientes.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.EgresoServicio.DepositosPendientes" %>

<%@ Register Src="~/UserControls/wucCobroRecurrente.ascx" TagPrefix="tectos" TagName="wucCobroRecurrente" %>
<%@ Register Src="~/UserControls/wucCuentaBanco.ascx" TagPrefix="tectos" TagName="wucCuentaBanco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryDepositosPendientes();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryDepositosPendientes() {

//Validación campos Hoja de Instrucción
$(document).ready(function () {
//Función de validación 
var validacionUltimosDepositos = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3
};
//Función de validación  de Depósitos
var validacionDepositar = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtFechaDeposito.ClientID%>").validationEngine('validate');
return isValid1;
};
//Función de validación  de Depósitos
var validacionDepositarAnticipoFactura = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtFechaDepFac.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtReferenciaDepFac.ClientID%>").validationEngine('validate');
return isValid1 && isValid2;
};
//Función de validación Rechazo de Depósitos
var validacionRechazarDeposito = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
return isValid1
};

//Función de validación de Liquidación
var validacionDepositarLiquidacion = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtFechaLiquidacion.ClientID%>").validationEngine('validate');
return isValid1
};
//Función de validación de Rechazo Liquidación
var validacionRechazarLiquidacion = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtReferenciaLiquidacion.ClientID%>").validationEngine('validate');
return isValid1
};

//Función de validación de Facturas
var validacionDepositarFacturas = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtFechaFactura.ClientID%>").validationEngine('validate');
return isValid1
}
//Función de validación Rechazo de Facturas
var validacionRechazarFacturas = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtReferenciaFactura.ClientID%>").validationEngine('validate');
return isValid1
}

//Botón Buscar
$("#<%= btnBuscar.ClientID %>").click(validacionUltimosDepositos);
//Botón Depositar Depósitos
$("#<%= btnDepositar.ClientID %>").click(validacionDepositar);
//Botón Rechazar Depósitos
$("#<%= btnRechazar.ClientID %>").click(validacionRechazarDeposito);
//Botón Depositar Liquidación
$("#<%= btnDepositarLiquidacion.ClientID %>").click(validacionDepositarLiquidacion);
//Botón Rechazar Liquidación
$("#<%= btnRechazarLiquidacion.ClientID %>").click(validacionRechazarLiquidacion);
//Botón Depositar Facturas
$("#<%= btnDepositarFactura.ClientID %>").click(validacionDepositarFacturas);
//Botón Rechazar Facturas
$("#<%= btnRechazarFactura.ClientID %>").click(validacionRechazarFacturas);
//Botón Depositar Anticipos/Liquidaciones - Facturas
$("#<%= btnDepositarDepFac.ClientID %>").click(validacionDepositarAnticipoFactura);
//Botón Depositar Anticipos/Liquidaciones - Facturas
$("#<%=btnRechazarDepFac.ClientID%>").click(function () {
var isValid1 = !$("#<%=txtReferenciaDepFac.ClientID%>").validationEngine('validate');
return isValid1;
});

//*** Fecha de Depósito (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaDeposito.ClientID%>").datetimepicker({
lang: 'es',
maxDate: '0',
format: 'd/m/Y',
timepicker: false,
});
$("#<%=txtFechaDepFac.ClientID%>").datetimepicker({
lang: 'es',
maxDate: '0',
format: 'd/m/Y',
timepicker: false,
});
//*** Fecha de Depósito de Liquidación (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaLiquidacion.ClientID%>").datetimepicker({
lang: 'es',
maxDate: '0',
format: 'd/m/Y',
timepicker: false,
});
//*** Fecha de Depósito de Factura (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaFactura.ClientID%>").datetimepicker({
lang: 'es',
maxDate: '0',
format: 'd/m/Y',
timepicker: false,
});
// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=10&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});

// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtProveedorLiquidacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=10&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtOperadorLiquidacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
$("#<%=txtUnidadLiquidacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});

});

// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtProveedorFacturas.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=10&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
//Cargando Catalogo de Autocompletado
$("#<%=txtProveedorEgreso.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
});

//Iniciando Función al Ejecutarse la Página
$(document).ready(function () {
//Configurando Control
$("#<%=txtFechaEI.ClientID%>").live('click', function () {
//Cargando Control DateTimePicker
$(this).datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
closeOnDateSelect: true
}).focus();
});

//Declarando Función de Validación
var validaFichaIngreso = function () {
var isValid1;
var isValid2 = !$("#<%=txtFechaEI.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtMonto.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtMontoPesos.ClientID%>").validationEngine('validate');

//Obteniendo Concepto
var idConcepto = $("#<%=ddlConcepto.ClientID%>").val();

//Validando el Concepto (17 Y 50 ANTICIPO/NC PROVEEDOR)
switch (idConcepto) {
case '50':
case '17': {
isValid1 = !$("#<%=txtProveedorEgreso.ClientID%>").validationEngine('validate');
break;
}
default: {
isValid1 = !$("#<%=txtNombreDep.ClientID%>").validationEngine('validate');
break;
}
}

//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3 && isValid4;
}
//Añadiendo Función al Evento Click
$("#<%=btnGuardar.ClientID%>").click(validaFichaIngreso);
$("#<%=lkbGuardar.ClientID%>").click(validaFichaIngreso);

//Función de Validación
$("#<%=txtMonto.ClientID%>").keydown(function (e) {
// Permite: backspace, delete, tab, escape, enter and .
if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
// Permite Funciones: Ctrl+A, Command+A
(e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
// Permite Teclas de Movimiento: home, end, left, right, down, up
(e.keyCode >= 35 && e.keyCode <= 40)) {
// let it happen, don't do anything
return;
}
// Ensure that it is a number and stop the keypress
if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
e.preventDefault();
}
});

});

}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryDepositosPendientes();
</script>
<div id="encabezado_forma">
<img src="../Image/SignoPesos.png" />
<h1>Depósitos Tesorería</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/BancoCuenta.png" />
<h2>Seleccione la cuenta de banco</h2>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlBanco">Banco</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlBanco" runat="server" AutoPostBack="true" CssClass="dropdown2x" OnSelectedIndexChanged="ddlBanco_SelectedIndexChanged"></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCuenta">Cuenta</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCuenta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCuenta" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlBanco" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnAnticipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAnticipo" Text="Anticipos" OnClick="btnVista_OnClick" CommandName="Anticipo" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLiquidacion" Text="Liquidacion" OnClick="btnVista_OnClick" runat="server" CommandName="Liquidacion" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnFacturasProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnFacturasProveedor" Text="Facturas" OnClick="btnVista_OnClick" runat="server" CommandName="Facturacion" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnEgresosVarios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEgresosVarios" Text="Egresos Varios" OnClick="btnVista_OnClick" runat="server" CommandName="EgresosVarios" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvDepositos" runat="server" ActiveViewIndex="0">
<asp:View ID="vwAnticipos" runat="server">
<div class="tab">
<div class="header_seccion">
<img src="../Image/Depositos.png" />
<h2>Anticipos pendientes</h2>
</div>
<div class="renglon100Per" style="float: left;">
<b>Nota: Para poder depositar los Anticipos que contienen Facturas, debera dar click en el "No. de Folio" que desee.</b>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoGrid" runat="server" OnSelectedIndexChanged="ddlTamanoGrid_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</div>
<div class="etiqueta">
<label for="lblCriterioGridDepositosPendientes">Ordenado por:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCriterioGridDepositosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridDepositosPendientes" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="ulkbExcelExportarDepositosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelExportarDepositosPendientes" CommandName="DepositosPendientes"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelExportarDepositosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvDepositosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDepositosPendientes" runat="server" AutoGenerateColumns="False"
AllowPaging="True" CssClass="gridview" AllowSorting="True" OnPageIndexChanging="gvDepositosPendientes_PageIndexChanging"
OnRowDataBound="gvDepositosPendientes_RowDataBound"
OnSorting="gvDepositosPendientes_Sorting" PageSize="25" ShowFooter="True" Width="100%">
<Columns>
<asp:TemplateField SortExpression="NoDeposito">
<FooterTemplate>
<asp:Label ID="lblContadorDepositos" runat="server" Text="0"></asp:Label>
<br />
Seleccionados
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkDepositosTodos" runat="server" AutoPostBack="True" CssClass="LabelResalta"
OnCheckedChanged="chkDepositosTodos_CheckedChanged" Text="Folio" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSeleccionDeposito" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleccionDeposito_CheckedChanged" />
<asp:Label ID="lblSeleccionDeposito" runat="server" Text='<%# Eval("NoDeposito") %>'></asp:Label>
<asp:LinkButton ID="lkbDepositarAnticipo" runat="server" Text='<%# Eval("NoDeposito") %>' CommandName="AnticipoFactura" OnClick="lkbDepositos_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="No. Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Factura" HeaderText="Factura" SortExpression="Factura" />
<asp:BoundField DataField="Banco" HeaderText="Banco" SortExpression="Banco" />
<asp:BoundField DataField="Cuenta" HeaderText="Cuenta" SortExpression="Cuenta">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoEspera" HeaderText="Tiempo" SortExpression="TiempoEspera">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAltaCuentaDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAltaCuentaDepositos" runat="server"
    CommandName="Depositos" OnClick="lkbAltaCuentaDepositos_Click">Alta Cuenta</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacoraDepositosPendientes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacoraDepositosPendientes" runat="server"
    CommandName="Bitacora" OnClick="lkbDepositos_Click">Bitácora</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacoraDepositosPendientes" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbReferenciasDepositosPendientes" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:LinkButton ID="lkbReferenciasDepositosPendientes" runat="server"
    CommandName="Referencias" OnClick="lkbDepositos_Click">Referencias</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbReferenciasDepositosPendientes" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGrid" />
<asp:AsyncPostBackTrigger ControlID="btnActualizar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnDepositar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnRechazar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCuentas" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacEg" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon100per">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnActualizar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnActualizar" CssClass="boton" Text="Actualizar"
CommandName="Actualizar" OnClick="Boton_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRechazar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRechazar" runat="server" CssClass="boton" Text="Rechazar" OnClick="Boton_Click"
CommandName="Rechazar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnDepositar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDepositar" runat="server" CssClass="boton" Text="Depositar" CommandName="Depositar"
OnClick="Boton_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDepositar" />
<asp:AsyncPostBackTrigger ControlID="btnRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnActualizar" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155pxr">
<label for="txtReferencia">Referencia (Trans./Rechazo)</label>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uptxtFechaDeposito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaDeposito" runat="server" CssClass="textbox validate[required, custom[date]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDepositar" />
<asp:AsyncPostBackTrigger ControlID="btnRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnActualizar" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<label for="txtFechaDeposito">Fecha Dep.</label>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="upddlFormaPagoDeposito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPagoDeposito" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80pxr">
<label for="ddlFormaPagoDeposito">Forma Pago</label>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda depositos realizados</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtOperador">
Operador</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtUnidad">
Unidad</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtProveedor">
Proveedor</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscar" runat="server" CssClass="boton"
Text="Buscar" OnClick="btnBuscar_Click" />
</div>
</div>
</div>
<div class="grid_media_seccion">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoGridViewUltimosDepositos" runat="server" OnSelectedIndexChanged="ddlTamanoGridViewUltimosDepositos_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewUltimosDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
Ordenado por:
<asp:Label ID="lblCriterioGridViewUltimosDepositos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUltimosDepositos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div>
<asp:UpdatePanel ID="uplkbExcelExportarUltimosDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelExportarUltimosDepositos" CommandName="UltimosDepositos"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelExportarUltimosDepositos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvUltimosDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUltimosDepositos" runat="server" AllowPaging="True" AllowSorting="True"
AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvUltimosDepositos_PageIndexChanging"
OnSorting="gvUltimosDepositos_Sorting" PageSize="25" ShowFooter="True" Width="100%">
<Columns>
<asp:BoundField DataField="NoDeposito" HeaderText="NoDeposito" SortExpression="Id" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Monto" DataFormatString="{0:C}" HeaderText="Monto" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="FacturaProveedor" HeaderText="Factura Proveedor" SortExpression="FacturaProveedor" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGridViewUltimosDepositos" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>


</div>
</asp:View>
<asp:View ID="vwLiquidacion" runat="server">
<div class="tab">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Liquidaciones Pendientes</h2>
</div>
<div class="renglon100Per" style="float: left;">
<b>Nota: Para poder depositar las Liquidaciones que contienen Factura, debera dar click en el "No. de Liquidación" que desee.</b>
</div>
<div class="renglon3x" style="float: left;">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoLiquidacion" runat="server" OnSelectedIndexChanged="ddlTamanoLiquidacion_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridDepositosPendientes">Ordenado por:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoLiquidacion" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarLiquidacion" CommandName="LiquidacionesPendientes"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvLiquidacionesPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLiquidacionesPendientes" runat="server" AutoGenerateColumns="False"
AllowPaging="True" CssClass="gridview" AllowSorting="True" OnPageIndexChanging="gvLiquidacionesPendientes_PageIndexChanging"
OnSorting="gvLiquidacionesPendientes_Sorting" PageSize="25" ShowFooter="True" Width="100%"
OnRowDataBound="gvLiquidacionesPendientes_RowDataBound">
<Columns>
<asp:TemplateField SortExpression="NoLiquidacion">
<FooterTemplate>
<asp:Label ID="lblContadorLiquidaciones" runat="server" Text="0"></asp:Label>
<br />
Seleccionados
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkLiquidacionesTodos" runat="server" AutoPostBack="True" CssClass="LabelResalta"
OnCheckedChanged="chkLiquidacionesTodos_CheckedChanged" Text="No. Liquidacion" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSeleccionLiquidacion" runat="server" AutoPostBack="True" OnCheckedChanged="chkSeleccionLiquidacion_CheckedChanged" />
<asp:Label ID="lblLiquidacion" runat="server" Text='<%# Eval("NoLiquidacion") %>'></asp:Label>
<asp:LinkButton ID="lkbDepositarLiquidacion" runat="server" Text='<%# Eval("NoLiquidacion") %>' CommandName="AnticipoFactura" OnClick="lkbDepositarLiquidacion_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="IdEgresoConcepto" HeaderText="IdEgresoConcepto" SortExpression="IdEgresoConcepto" Visible="false" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="No. Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Factura" HeaderText="Factura" SortExpression="Factura" />
<asp:TemplateField HeaderText="Servicio(s)" SortExpression="Servicios">
<ItemTemplate>
<asp:Label ID="lblServicio" runat="server" ToolTip='<%# Eval("Servicios") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Servicios").ToString(), 7, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Banco" HeaderText="Banco" SortExpression="Banco" />
<asp:BoundField DataField="Cuenta" HeaderText="Cuenta" SortExpression="Cuenta">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoEspera" HeaderText="Tiempo" SortExpression="TiempoEspera">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbCobroRecurrente" runat="server" Text="Asignar Cobro" OnClick="lkbCobroRecurrente_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="PagoEnContra" HeaderText="Pago En Contra" SortExpression="PagoEnContra" Visible="false" />
<asp:TemplateField ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAltaCuentaLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAltaCuentaLiquidacion" runat="server"
    CommandName="Liquidacion" OnClick="lkbAltaCuentaLiquidacion_Click">Alta Cuenta</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarLiquidacion" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarLiquidacion" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarLiquidacion" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="wucCobroRecurrente" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCuentas" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnAplicarFacEg" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon100per">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnActualizarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnActualizarLiquidacion" CssClass="boton" Text="Actualizar" CommandName="Actualizar"
OnClick="BotonLiquidacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRechazarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRechazarLiquidacion" runat="server" CssClass="boton" Text="Rechazar" CommandName="Rechazar"
OnClick="BotonLiquidacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnDepositarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDepositarLiquidacion" runat="server" CssClass="boton" Text="Depositar" CommandName="Depositar"
OnClick="BotonLiquidacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="uptxtReferenciaLiquidacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtReferenciaLiquidacion" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDepositarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155pxr">
<label for="txtReferenciaLiquidacion">Referencia (Trans./Rechazo)</label>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uptxtFechaLiquidacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaLiquidacion" runat="server" CssClass="textbox validate[required, custom[date]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDepositarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<label for="txtFechaLiquidacion">Fecha Dep.</label>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="upddlFormaPagoLiquidacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPagoLiquidacion" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80pxr">
<label for="ddlFormaPagoLiquidacion">Forma Pago</label>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda Liquidaciones Realizadas</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtOperadorLiquidacion">
Operador</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtOperadorLiquidacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtUnidad">
Unidad</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtUnidadLiquidacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtProveedorLiquidacion">
Proveedor</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtProveedorLiquidacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscarLiquidaciones" runat="server" CssClass="boton"
Text="Buscar" OnClick="btnBuscarLiquidaciones_Click" />
</div>
</div>
</div>
<div class="grid_media_seccion">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoLiquidaciones" runat="server" OnSelectedIndexChanged="ddlTamanoLiquidaciones_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
<ContentTemplate>
Ordenado por:
<asp:Label ID="lblOrdenadoUltimasLiq" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUltimasLiquidaciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div>
<asp:UpdatePanel ID="uplnkExportarUltLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarUltLiq" CommandName="UltimasLiqudiaciones"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarUltLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvUltimasLiquidaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUltimasLiquidaciones" runat="server" AllowPaging="True" AllowSorting="True"
AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvUltimasLiquidaciones_PageIndexChanging"
OnSorting="gvUltimasLiquidaciones_Sorting" PageSize="25" ShowFooter="true" Width="100%">
<Columns>
<asp:BoundField DataField="NoLiquidacion" HeaderText="No Liquidacion" SortExpression="NoLiquidacion" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Monto" DataFormatString="{0:C}" HeaderText="Monto" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FacturasProveedor" HeaderText="Factura Proveedor" SortExpression="FacturasProveedor" ItemStyle-HorizontalAlign="Right" />
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoLiquidaciones" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwFacturacion" runat="server">
<div class="tab">
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Pendientes</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacturas" runat="server" OnSelectedIndexChanged="ddlTamanoFactura_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFacturas">Ordenado por:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoFacturas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasPendientes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarFacturas" CommandName="FacturasPendientes"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvFacturasPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasPendientes" runat="server" AutoGenerateColumns="False"
AllowPaging="True" CssClass="gridview" AllowSorting="True" OnPageIndexChanging="gvFacturasPendientes_PageIndexChanging"
OnSorting="gvFacturasPendientes_Sorting" PageSize="25" ShowFooter="True" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField SortExpression="NoFactura">
<FooterTemplate>
<asp:Label ID="lblContadorFacturas" runat="server" Text="0"></asp:Label>
<br />
Seleccionados
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkFacturasTodos" runat="server" AutoPostBack="True" CssClass="LabelResalta"
OnCheckedChanged="chkFacturasTodos_CheckedChanged" Text="FOLIO" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSeleccionFactura" runat="server" AutoPostBack="True" Text='<%# Eval("NoFactura") %>' OnCheckedChanged="chkSeleccionFactura_CheckedChanged" />
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Factura" HeaderText="Factura" SortExpression="Factura" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="TotalFactura" HeaderText="Total Factura" SortExpression="TotalFactura" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Banco" HeaderText="Banco" SortExpression="Banco" />
<asp:BoundField DataField="IdCuenta" HeaderText="IdCuenta" SortExpression="IdCuenta" Visible="false" />
<asp:BoundField DataField="Cuenta" HeaderText="Cuenta" SortExpression="Cuenta">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoEspera" HeaderText="Tiempo" SortExpression="TiempoEspera">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField ItemStyle-HorizontalAlign="Right">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAltaCuentaDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAltaCuentaFacturas" runat="server"
    CommandName="Facturas" OnClick="lkbAltaCuentaFacturas_Click">Alta Cuenta</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturas" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCuentas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon100per">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnActualizarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnActualizarFactura" CssClass="boton" Text="Actualizar"
CommandName="Actualizar" OnClick="BotonFacturacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRechazarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRechazarFactura" runat="server" CssClass="boton" Text="Rechazar" CommandName="Rechazar"
OnClick="BotonFacturacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnDepositarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnDepositarFactura" CssClass="boton" Text="Depositar" CommandName="Depositar"
OnClick="BotonFacturacion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="uptxtReferenciaFactura" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtReferenciaFactura" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155pxr">
<label for="txtReferenciaFactura">Referencia (Trans./Rechazo)</label>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uptxtFechaFactura" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaFactura" runat="server" CssClass="textbox validate[required, custom[date]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<label for="txtFechaFactura">Fecha Dep.</label>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="upddlFormaPagoFactura" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPagoFactura" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnActualizarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80pxr">
<label for="ddlFormmaPagoFactura">Forma Pago</label>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Busqueda Facturas por Pagar</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtProveedorFacturas">
Proveedor</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtProveedorFacturas" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscarFacturasPorPagar" runat="server" CssClass="boton"
Text="Buscar" OnClick="btnBuscarFacturasPorPagar_Click" />
</div>
</div>
</div>
<div class="grid_media_seccion">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoUltimasFacturas" runat="server" OnSelectedIndexChanged="ddlTamanoFacturas_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="True">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
Ordenado por:
<asp:Label ID="lblOrdenadoUltimasFacturas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUltimasFacturas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div>
<asp:UpdatePanel ID="uplkbExcelFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelFactura" CommandName="UltimasFacturas"
runat="server" OnClick="lkbExcel_Click">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvUltimasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUltimasFacturas" runat="server" AllowPaging="True" AllowSorting="True"
AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvUltimasFacturas_PageIndexChanging"
OnSorting="gvUltimasFacturas_Sorting" PageSize="25" ShowFooter="true" Width="100%">
<Columns>
<asp:BoundField DataField="NoFactura" HeaderText="No. Factura" SortExpression="NoFactura" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Monto" DataFormatString="{0:C}" HeaderText="Monto" SortExpression="Monto">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoLiquidaciones" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwEgresosVarios" runat="server">
<div class="seccion_controles">
<div class="header_seccion">
<h2>Egresos</h2>
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
<li>
<asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
<li>
<asp:LinkButton ID="lkbDepositar" runat="server" Text="Depositar" OnClick="lkbElementoMenu_Click" CommandName="Depositar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
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
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoEgreso">No. Egreso</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblNoEgreso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblNoEgreso" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlConcepto">Concepto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown2x" TabIndex="1" AutoPostBack="true"
OnSelectedIndexChanged="ddlConcepto_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreDep">A Nombre de</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNombreDep" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreDep" runat="server" CssClass="textbox2x validate[required]" TabIndex="2" MaxLength="100" Visible="true"></asp:TextBox>
<asp:TextBox ID="txtProveedorEgreso" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlConcepto" />
</Triggers>
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
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbDepositar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlMetodoPago">Método de Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown2x" TabIndex="3"
OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreDep">No. Cheque</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNumCheque" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNumCheque" runat="server" CssClass="textbox2x" TabIndex="4" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoTransferenciaBancaria">No. Transferencia Bancaria</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoTransferenciaBancaria" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoTransferenciaBancaria" runat="server" CssClass="textbox2x" TabIndex="4" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDepartamento">Departamento</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlDepartamento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCuentaOrigen">Cuenta Origen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCuentaOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCuentaOrigen" runat="server" CssClass="dropdown2x" TabIndex="6"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="txtNombreDep" EventName="TextChanged" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCuentaDestino">Cuenta Destino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCuentaDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCuentaDestino" runat="server" CssClass="textbox2x" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" EventName="SelectedIndexChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaEI">Fecha</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaEI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaEI" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="8"
MaxLength="10" AutoPostBack="true" OnTextChanged="txtFechaEI_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMonto">Monto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMonto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMonto" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]" TabIndex="9"
AutoPostBack="true" OnTextChanged="txtMonto_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlMoneda">Moneda</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMoneda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMoneda" runat="server" CssClass="dropdown" TabIndex="10" AutoPostBack="true"
OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMontoPesos">Monto Pesos</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMontoPesos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMontoPesos" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]" TabIndex="11" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMoneda" />
<asp:AsyncPostBackTrigger ControlID="txtMonto" EventName="TextChanged" />
<asp:AsyncPostBackTrigger ControlID="txtFechaEI" EventName="TextChanged" />
</Triggers>
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
<asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" TabIndex="11" MaxLength="500"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMoneda" />
<asp:AsyncPostBackTrigger ControlID="txtMonto" EventName="TextChanged" />
<asp:AsyncPostBackTrigger ControlID="txtFechaEI" EventName="TextChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorEgresos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorEgresos" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAnticipo" />
<asp:AsyncPostBackTrigger ControlID="btnFacturasProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnEgresosVarios" />
</Triggers>
</asp:UpdatePanel>
</div>

<!-- Ventana para Aplicar las Facturas -->
<div id="contenedorVentanaAplicacionFactura" class="modal">
<div id="ventanaAplicacionFactura" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarFacturas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarFacturas" runat="server" CommandName="FacturasLigadas" OnClick="lkbCerrarDepositoFac_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Ligadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label>Saldo:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblSaldoFI" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblPorAplicar">Por Aplicar:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblPorAplicar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblPorAplicar" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblSaldoFinal">Saldo Final:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSaldoFinal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblSaldoFinal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoFL">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFL" CssClass="dropdown" runat="server" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" CommandName="FacturasLigadas" OnClick="lkbExcel_Click">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura" id="contenedorFacturasLigadas">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="true"
CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
OnRowDataBound="gvFacturasLigadas_RowDataBound" TabIndex="37" AutoGenerateColumns="false" Width="100%" PageSize="5">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkTodosFactura" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodosFactura_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVariosFactura" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodosFactura_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:LinkButton ID="lnkAceptarFacturaLigada" runat="server" Text='<%# Eval("Estatus") %>' OnClick="lnkAceptarFacturaLigada_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha Fac." SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:TemplateField HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:TextBox ID="txtMXA" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" Text='<%# Eval("MontoPorAplicar","{0:0.00}") %>'
Enabled="false" TabIndex="9" Style="text-align: right">
</asp:TextBox>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="MPA2" HeaderText="MPA2" SortExpression="MPA2" Visible="false" />
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lnkCambiar" runat="server" Text="Cambiar" OnClick="lnkCambiar_Click" CommandName="Cambiar" Enabled="false"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon3x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAplicarFacEg" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAplicarFacEg" runat="server" CssClass="boton" Text="Aplicar" OnClick="btnAplicarFacturas_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana para Alta de Cobro Recurrente -->
<div id="contenedorVentanaCobroRecurrente" class="modal">
<div id="ventanaCobroRecurrente" class="contenedor_ventana_confirmacion_arriba">
<div class="columna2x">
<div class="header_seccion">
<h2>Liquidación en Contra</h2>
</div>
<asp:UpdatePanel ID="upwucCobroRecurrente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCobroRecurrente ID="wucCobroRecurrente" runat="server"
OnClickGuardarCobroRecurrente="wucCobroRecurrente_ClickGuardarCobroRecurrente"
OnClickCancelarCobroRecurrente="wucCobroRecurrente_ClickCancelarCobroRecurrente" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!--Ventana para Alta de Cuentas -->
<div id="contenedorVentanaAltaCuentas" class="modal">
<div id="ventanaAltaCuentas" class="contenedor_modal_seccion_completa_arriba" style="width: 800px; height: 390px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCuentas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCuentas" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Cerrra" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upwucCuentaBancos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCuentaBanco ID="wucCuentaBancos" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Contenedor Ventana Deposito de Anticipo de Proveedor -->
<div id="contenedorVentanaDepositoFactura" class="modal">
<div id="ventanaDepositoFactura" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDepositoFac" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDepositoFac" runat="server" CommandName="AnticipoFactura" OnClick="lkbCerrarDepositoFac_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Datos del Deposito</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtFechaDepFac">Fecha de Deposito</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaDepFac" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaDepFac" runat="server" CssClass="textbox validate[required, custom[date]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtReferenciaDepFac">Referencia (Trans./Rechazo)</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferenciaDepFac" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtReferenciaDepFac" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="ddlFormaPagoDepFac">Forma de Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlFormaPagoDepFac" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPagoDepFac" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
<asp:AsyncPostBackTrigger ControlID="btnDepositarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarDepFac" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRechazarDepFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRechazarDepFac" runat="server" CssClass="boton_cancelar" Text="Rechazar" CommandName="Rechazar"
OnClick="BotonDepFac_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnDepositarDepFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDepositarDepFac" runat="server" CssClass="boton" Text="Depositar" CommandName="Anticipo"
OnClick="BotonDepFac_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacionesPendientes" />
<asp:AsyncPostBackTrigger ControlID="gvDepositosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</asp:Content>
