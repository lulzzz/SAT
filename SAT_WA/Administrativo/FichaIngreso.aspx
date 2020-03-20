<%@ Page Title="Ficha de Ingreso" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FichaIngreso.aspx.cs" Inherits="SAT.Administrativo.FichaIngreso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraFichaIngreso();
}
}
//Declarando Función de Validación
function ConfiguraFichaIngreso()
{   //Iniciando Función al Ejecutarse la Página
$(document).ready(function () {

//Declarando Función de Validación
var validaFichaIngreso = function () {

//Obteniendo Tipo de Entidad
var tipoEntidad = $("#<%=ddlTipoEntidad.ClientID%>").val();

//Removiendo Clases
$("#<%=txtNombreDep.ClientID%>").removeClass();
                
//Validando Tipo de Entidad
switch (tipoEntidad) {
case "25":
//Asignando Clase y Validadores [Campo Requerido, Catalogo Autocompleta]
$("#<%=txtNombreDep.ClientID%>").addClass('textbox2x validate[required, custom[IdCatalogo]]');
break;
default:
//Asignando Clase y Validadores [Campo Requerido]
$("#<%=txtNombreDep.ClientID%>").addClass('textbox2x validate[required]');
break;
}

//Validando Controles
var isValid1 = !$("#<%=txtNombreDep.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFechaEI.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtMonto.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtMontoPesos.ClientID%>").validationEngine('validate');

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

//Declarando Función de Cambio
$("#<%=txtMonto.ClientID%>").change(function () {

    //Asignando Valor
    $("#<%=txtMontoPesos.ClientID%>").val(this.value);

    //Asignando Enfoque al Control
    $("#<%=ddlMoneda.ClientID%>").focus();
});

//Cargando Control de Fechas
$("#<%=txtFechaEI.ClientID%>").datetimepicker({
lang: 'es',
    format: 'd/m/Y',
    maxDate: '0',
closeOnDateSelect: true,
onSelectDate: function (selected, evnt) {
//Asignando Valor Seleccionado
    $("#<%=txtFechaEI.ClientID%>").val(selected.format('dd/MM/yyyy'));

    //Causando Actualización del Control
    __doPostBack('<%= txtFechaEI.UniqueID %>', '');
},
timepicker: false
});


});
}

//Invocando Función de Configuración
ConfiguraFichaIngreso();
</script>
<div id="encabezado_forma">
<img src="../Image/Modulos.png" />
<h1>Ficha de Ingreso</h1>
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
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" /></li>
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
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoFicha">No. Ficha</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblNoFicha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblNoFicha" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEntidad">Tipo Depositante</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEntidad" runat="server" CssClass="dropdown2x" TabIndex="1" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreDep">Depositante</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNombreDep" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreDep" runat="server" CssClass="textbox2x" TabIndex="2" AutoPostBack="true"
OnTextChanged="txtNombreDep_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoEntidad" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown2x" TabIndex="3"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlMetodoPago">Forma de Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown2x" TabIndex="4"
OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreDep">Núm. Operación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNumCheque" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNumCheque" runat="server" CssClass="textbox2x" TabIndex="5" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCuentaOrigen">Cuenta Ordenante</label>
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
<asp:AsyncPostBackTrigger ControlID="ddlTipoEntidad" />
<asp:AsyncPostBackTrigger ControlID="txtNombreDep" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCuentaDestino">Cuenta Beneficiario</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCuentaDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCuentaDestino" runat="server" CssClass="dropdown2x" TabIndex="7"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
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
MaxLength="10" OnTextChanged="txtFechaEI_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:TextBox ID="txtMonto" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="15"
TabIndex="9" AutoPostBack="true" OnTextChanged="txtMonto_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:DropDownList ID="ddlMoneda" runat="server" CssClass="dropdown2x" TabIndex="10" AutoPostBack="true" 
OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Aplicadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFF">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFF" runat="server" CssClass="dropdown" TabIndex="14" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFF_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFF">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFF" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="15" OnClick="lnkExportarFF_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
    <div class="etiqueta">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="LinkButton1" runat="server" Text="CFDI" TabIndex="6" OnClick="LinkButton1_Click" Visible="false"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura">
<asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%" TabIndex="16"
OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting" PageSize="250"
CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true">
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
<asp:BoundField DataField="FacturaFicha" HeaderText="Serie-Folio" SortExpression="FacturaFicha" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="AplicadoPor" HeaderText="Aplicado Por" SortExpression="AplicadoPor" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarAplicacion" runat="server" Text="Eliminar" OnClick="lnkEliminarAplicacion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="uplblErrorFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFF" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
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
</asp:Content>
