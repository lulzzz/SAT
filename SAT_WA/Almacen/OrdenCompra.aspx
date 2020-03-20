<%@ Page Title="Orden de Compra" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrdenCompra.aspx.cs" Inherits="SAT.Almacen.OrdenCompra" %>
<%@ Register Src="~/UserControls/wucEnvioEmail.ascx" TagPrefix="tectos" TagName="WucEnvioEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--Hoja de estilos que da formato a la página-->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!--Hoja de estilo para la validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<style>
#contenedorFacturaXML {
margin-top: 10px;
margin-left: 10px;
margin-bottom:20px;
width: 400px;
height: 120px;
text-align: center;
vertical-align: middle;
border: 2px solid #939393;
background-color: #f8f8f8;
padding: 15px;
font-family: Arial;
font-size: 16px;
}
</style>
<!--Script que valida el contenido de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!--Script que valida la insercion de datos en los controles-->
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryOrdenCompra
ConfiguraJQueryOrdenCompra();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryOrdenCompra() {
$(document).ready(function () {

//Creación  y asignación de la funcion a la variable ValidaTipoPago
var validaOrdenCompra = function () {
//Creación de las variables y asignacion de los controles de la pagina TipoPago
var isValid1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtNoOrdenCompra.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFechaSolicitud.ClientID%>").validationEngine('validate')
var isValid4 = !$("#<%=txtFechaCompromiso.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');

//Devuelve un valor a la funcion
    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
};

//Función de Validación - Abastecer
var validaAbastecerOrden = function (e) {
//Validando Controles
var isValid1 = !$("#<%=txtCantidadInv.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtPrecioInv.ClientID%>").validationEngine('validate');
var isValid3;

//Validando el Control
if ($("#<%=rbLote.ClientID%>").is(':checked') == true) {
//Validando Controles
isValid3 = !$("#<%=txtFecCad.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo
isValid3 = true;
}

//Devolviendo Resultado
return isValid1 && isValid2 && isValid3;
}

//Creación  y asignación de la funcion a la variable ValidaTipoPago
var validaOrdenCompraDetalle = function () {
//Creación de las variables y asignacion de los controles de la pagina TipoPago
var isValid1 = !$("#<%=txtProducto.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtPrecioUnitario.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2 && isValid3;
};

//Función de Validación - Edición de Inventario
$('.scriptGuardaInventario').click(function () {
//Validando Control
var isValid1 = !$('.scriptInventario').validationEngine('validate');
//Devolviendo Resultado
return isValid1;
});
//Función de Validación - Alta Inventario Temporal
$('.scriptGuardaInvTemp').click(function () {
//Validando Control
var isValid1 = !$('.scriptInvTemp').validationEngine('validate');
//Devolviendo Resultado
return isValid1;
});            

//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaOrdenCompra);
$("#<%=lkbGuardar.ClientID%>").click(validaOrdenCompra);
$("#<%=btnSolicitarOrden.ClientID%>").click(validaOrdenCompra);
$("#<%=btnSurtir.ClientID%>").click(validaAbastecerOrden);
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardarDOC.ClientID%>").click(validaOrdenCompraDetalle);


// *** Fecha de inicio y fin (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaSolicitud.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaEntrega.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaCompromiso.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecCad.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
closeOnDateSelect: true
});
//Cargando Script de Fecha
$('.scriptInventario').datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
closeOnDateSelect: true
});
//Cargando Script de Fecha
$('.scriptInvTemp').datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
closeOnDateSelect: true
});


//Cargando Catalogos Autocompleta
$("#<%=txtProducto.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=31&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
appendTo: '#ordenCompraDetalle',
select: function (event, ui) {
//Asignando Selección al Valor del Control
$("#<%=txtProducto.ClientID%>").val(ui.item.value);
//Causando Actualización del Control
__doPostBack('<%= txtProducto.UniqueID %>', '');
}
});
$("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
$("#<%=txtAlmacen.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});

//Funciones de Validación
$("#<%=txtTasaImpTrasladado.ClientID%>").keydown(function (e) {
// Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
if ($.inArray(e.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
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
$("#<%=txtTasaImpRetenido.ClientID%>").keydown(function (e) {
// Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
if ($.inArray(e.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
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

/** Función de Cambio - Impuesto Trasladado **/
$("#<%=txtTasaImpTrasladado.ClientID%>").change(function (e) {

//Obteniendo Valores
var valorTotal = 0.000000;
var valorImpTra = 0.000000;
var valorImpRet = parseFloat($("#<%=txtImpRetenido.ClientID%>").val());
var valorSubtotal = parseFloat($("#<%=txtSubTotal.ClientID%>").val());
var valorTasa = parseFloat(this.value == "" ? "0" : this.value);

//Validando que el Valor no sea '0'
if (valorTasa > 0) {

//Validando que la tasa no sea superior a '100'
if (valorTasa > 100) {
//Mostrando Validación
!$("#<%=txtTasaImpTrasladado.ClientID%>").validationEngine('validate');

//Asignando Valores
valorTasa = 100.00;
$("#<%=txtImpTrasladado.ClientID%>").val(valorSubtotal);
$("#<%=txtTasaImpTrasladado.ClientID%>").val(valorTasa.toFixed(2));
}
else {
//Calculando Impuesto
valorImpTra = valorSubtotal * (valorTasa / 100);

//Asignado valor Calculado
$("#<%=txtImpTrasladado.ClientID%>").val(valorImpTra.toFixed(6));
}
}
else {
//Asignando Valores en '0'
valorImpTra = 0.000000;
valorTasa = 0.00;
$("#<%=txtImpTrasladado.ClientID%>").val(valorImpTra.toFixed(6));
$("#<%=txtTasaImpTrasladado.ClientID%>").val(0.00);
}

//Sumando Totales
valorTotal = valorSubtotal + valorImpTra - valorImpRet;

//Mostrando Valores Finales
$("#<%=txtTasaImpTrasladado.ClientID%>").val(valorTasa.toFixed(2));
$("#<%=txtTotal.ClientID%>").val(valorTotal.toFixed(6));
});

/** Función de Cambio - Impuesto Retenido **/
$("#<%=txtTasaImpRetenido.ClientID%>").change(function (e) {

//Obteniendo Valores
var valorTotal = 0.000000;
var valorImpTra = parseFloat($("#<%=txtImpTrasladado.ClientID%>").val());
var valorImpRet = 0.000000;
var valorSubtotal = parseFloat($("#<%=txtSubTotal.ClientID%>").val());
var valorTasa = parseFloat(this.value == "" ? "0" : this.value);

//Validando que el Valor no sea '0'
if (valorTasa > 0) {

//Validando que la tasa no sea superior a '100'
if (valorTasa > 100) {
//Mostrando Validación
!$("#<%=txtTasaImpRetenido.ClientID%>").validationEngine('validate');

//Asignando Valores en '0'
valorTasa = 100.00;
$("#<%=txtImpRetenido.ClientID%>").val(valorSubtotal);
$("#<%=txtTasaImpRetenido.ClientID%>").val(valorTasa);
}
else {
//Calculando Impuesto
valorImpRet = valorSubtotal * (valorTasa / 100);

//Asignado valor Calculado
$("#<%=txtImpRetenido.ClientID%>").val(valorImpRet.toFixed(6));
}
}
else {
//Mostrando Validación
!$("#<%=txtTasaImpRetenido.ClientID%>").validationEngine('validate');

//Asignando Valores en '0'
valorImpTra = 0.000000;
valorTasa = 0.00;
$("#<%=txtImpRetenido.ClientID%>").val(valorImpTra.toFixed(6));
$("#<%=txtTasaImpRetenido.ClientID%>").val(0.00);
}

//Sumando Totales
valorTotal = valorSubtotal + valorImpTra - valorImpRet;

//Mostrando Valores Finales
$("#<%=txtTasaImpRetenido.ClientID%>").val(valorTasa.toFixed(2));
$("#<%=txtTotal.ClientID%>").val(valorTotal.toFixed(6));
});
});
$(document).keyup(function (e) {
if (e.keyCode == 27) { // escape key maps to keycode `27`
//Ocultando Menu
OcultarMenu();
}
});
$(document).click(function (e) {
            
//Ocultando Menu
OcultarMenu();
});
}

//Invocando Función de Configuración
ConfiguraJQueryOrdenCompra();

//Función encargada de Mostrar el Ménu
function MostrarMenu(control, e) {
//Ocultando en caso de estar Abierto
OcultarMenu();

//Obteniendo Coordenadas de las Forma
var posx = e.pageX + 'px';
var posy = e.pageY + 'px';

//Si el Evento es de Tipo Click
if (e.type == 'click')

//Detener Propagación del Evento
e.stopPropagation();

//Asignando Posiciones al Documento
document.getElementById(control).style.position = 'absolute';
document.getElementById(control).style.left = posx;
document.getElementById(control).style.top = posy;


//Ejecutando 
$(document).ready(function (evt) {

//Mostrando DIV
$('#' + control).slideDown(100);
});
}
//Función encargada de Ocultar el Ménu
function OcultarMenu() {
$(document).ready(function () {
//Ocultando DIV
$('.menuContainer').slideUp(100);
});
}

/**Script Contenedor de Archivos**/
//Declarando variable contenedora de Archivos
var selectedFiles;
//Función que limpia el Contenedor
function LimpiaContenedorXML() {   //Limpiando DIV
selectedFiles = null;
$('#contenedorFacturaXML').text('Arrastre y Suelte sus archivos desde su maquina en este cuadro.');
}
//Inicializando Función
$(document).ready(function () {
//validando el Tipo de Archivo
if (!Modernizr.draganddrop) {
alert('This browser doesnt support File API and Drag & Drop features of HTML5!');
return;
}


//Declarando Objeto contenedor del DIV
var box;
box = document.getElementById('contenedorFacturaXML');
//Añadiendo Eventos
box.addEventListener('dragenter', OnDragEnter, false);
box.addEventListener('dragover', OnDragOver, false);
box.addEventListener('drop', OnDrop, false);

});
//Función cuando se Arrastra el Objeto dentro del limite
function OnDragEnter(e) {
e.stopPropagation();
e.preventDefault();
}
//Función cuando se Arrastra el Objeto fuera del limite
function OnDragOver(e) {
e.stopPropagation();
e.preventDefault();
}
//Función cuando se Suelta el Objeto dentro del limite
function OnDrop(e) {
e.stopPropagation();
e.preventDefault();

selectedFiles = null;
selectedFiles = e.dataTransfer.files;
//Declarando Objeto de Lectura
var lector = new FileReader();

//Evento al Cargar el Archivo
lector.onload = function (evt) {
//Obteniendo Archivo
var bytes = evt.target.result;
//Invocando Método Web para Obtención de Archivos
PageMethods.ArchivoSesion(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
};
//Evento al Producirse un Error
lector.onerror = function (evt) {
alert('Error Carga ' + evt.target.error);

};
//Leyendo Texto
lector.readAsText(selectedFiles[0]);
//Mostrando mensaje
alert('El Archivo se ha Cargado')
//Indicando Archivo
$('#contenedorFacturaXML').text('El Archivo ' + selectedFiles[0].name + ' ha sido Cargado con exito');
}
</script>
<div id="encabezado_forma">
<h1>Orden Compra</h1>
</div>
<!--Menu Principal-->
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
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" /></li>
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
<asp:LinkButton ID="lkbEtiqueta" runat="server" Text="Etiqueta" OnClick="lkbElementoMenu_Click" CommandName="Etiqueta" /></li>
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
<li class="gray">
<a href="#" class="fa fa-file-archive-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbFacturaProveedor" runat="server" Text="Facturas" OnClick="lkbElementoMenu_Click" CommandName="Facturas" /></li>
<li>
<asp:LinkButton ID="lkbEnvioEmail" runat="server" Text="E-mail" OnClick="lkbElementoMenu_Click" CommandName="Email" /></li>
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
    
<div class="contenedor_controles">
<div class="header_seccion">
<img src="../Image/OrdenCompra.png" />
<h2>Descripcion Orden Compra:</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoOrdenCompra">No. Orden</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel runat="server" ID="uplblNoOrdenCompra" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNoOrdenCompra" CssClass="label_negrita" TabIndex="1"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompaniaEmisor">Compañia</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtCompaniaEmisor" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCompaniaEmisor" CssClass="textbox2x" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProveedor" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProveedor" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoOrdenCompra">No. Orden C.</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtNoOrdenCompra" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtNoOrdenCompra" CssClass="textbox validate[required]" MaxLength="150" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlEstatus" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlEstatus" CssClass="dropdown " TabIndex="3" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoOrden">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlTipoOrden" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipoOrden" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtAlmacen">Almacen</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtAlmacen" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtAlmacen" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
            
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txFechaSolicitud">Fecha de Solicitud</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaSolicitud" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaSolicitud" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="6" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtFechaCompromiso">Fecha de Compromiso</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaCompromiso" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaCompromiso" CssClass="textbox validate[required,custom[dateTime24]]" TabIndex="8" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtFechaEntrega">Fecha de Entrega</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaEntrega" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaEntrega" CssClass="textbox validate[custom[dateTime24]]" TabIndex="7" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
            
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlFormaEntrega">Forma de Entrega</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlFormaEntrega" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlFormaEntrega" CssClass="dropdown" TabIndex="9"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCondicionesPago">Condiciones Pago</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlCondicionesPago" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlCondicionesPago" CssClass="dropdown" TabIndex="11"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCondicionesPago">Moneda</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlMoneda" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlMoneda" CssClass="dropdown" TabIndex="10"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblSubTotal">Sub Total</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uplblSubTotal" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSubTotal" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<%--<div class="control_100px">
<asp:UpdatePanel ID="uplkbActualizarTotales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbActualizarTotales" runat="server" Text="Actualizar Totales"
OnClick="lkbActualizarTotales_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>--%>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtImpTrasladado">Imp. Trasladado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtImpTrasladado" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImpTrasladado" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtTasaImpTrasladado" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTasaImpTrasladado" runat="server" TabIndex="11" CssClass="textbox_50px validate[required, custom[positiveNumber], max[100]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<b><label>%</label></b>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtImpRetenido">Imp. Retenido</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtImpRetenido" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImpRetenido" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<asp:UpdatePanel runat="server" ID="uptxtTasaImpRetenido" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTasaImpRetenido" runat="server" TabIndex="12" CssClass="textbox_50px validate[required, custom[positiveNumber], max[100]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<b><label>%</label></b>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblTotal">Total</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uplblTotal" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotal" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnAbrirModalDOC" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnAbrirModalDOC" Text="Agregar Detalle" CssClass="boton" OnClick="btnAbrirModalDOC_Click" TabIndex="15" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="14" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardar" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnSolicitarOrden" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnSolicitarOrden" Text="Solicitar Orden" CssClass="boton" OnClick="btnSolicitarOrden_Click" TabIndex="16" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Detalles de la Orden</h2>
</div>
<div class="grid_seccion_completa_media_altura" oncontextmenu="return false">
<asp:UpdatePanel runat="server" ID="upgvOrdenCompraDetalle" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView runat="server" ID="gvOrdenCompraDetalle" CssClass="gridview" AllowPaging="true"
ShowFooter="true" AutoGenerateColumns="false" OnRowDataBound="gvOrdenCompraDetalle_RowDataBound">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Codigo" HeaderText="Código" SortExpression="Codigo" />
<asp:TemplateField HeaderText="Producto" SortExpression="Producto">
<ItemTemplate>
<asp:LinkButton ID="lnkInventario" runat="server" Text='<%# Eval("Producto") %>' OnClick="lnkInventario_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Categoria" HeaderText="Categoria" SortExpression="Categoria" />
<asp:BoundField DataField="CantidadInicial" HeaderText="Cantidad Solicitada" SortExpression="CantidadInicial" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="CantidadAbastecida" HeaderText="Cantidad Abastecida" SortExpression="CantidadAbastecida" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad Restante" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="UnidadMedida" HeaderText="Unidad Medida" SortExpression="UnidadMedida" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unitario" SortExpression="PrecioUnitario" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:TemplateField>
<ItemTemplate>
<div id="menuContext" runat="server">
<img src="../Image/menu_context2.png" />
</div>
<div id="menuOptions" runat="server" class="MenuContext menuContainer" style="display:none;">
<div class="ContextItem">
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lnkAbastecer" runat="server" Text="Abastecer" CommandName="Automatico" OnClick="lnkSurtir_Click"></asp:LinkButton>
</div>
</div>
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
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSurtir" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCantidadProducto" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarInventario" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana modal que permite agregar el desglose de productos de una orden de compra-->
<div id="contenidoOrdenCompraDetalle" class="modal">
<div id="ordenCompraDetalle" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarDetalleOrdenCompra" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarDetalleOrdenCompra" runat="server" Text="Cerrar" CommandName="DetalleOrdenCompra"
OnClick="lkbCerrarVentanaModal_Click" TabIndex="22">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class=" header_seccion">
<img src="../Image/OrdenCompra.png" />
<h2>Detalle Orden Compra</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatusDetalle">Estatus:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlEstatusDetalle" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlEstatusDetalle" CssClass="dropdown" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProducto">Producto:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProducto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProducto" CssClass="textbox2x  validate[required, custom[IdCatalogo]]" TabIndex="17"
AutoPostBack="true" OnTextChanged="txtProducto_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTExistencia">T. Existencia</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtTExistencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTExistencia" runat="server" CssClass="textbox_50px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtProducto" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<label for="txtTRequerido">T. Requerido</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtTRequerido" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTRequerido" runat="server" CssClass="textbox_50px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtProducto" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<label for="txtTPorEntregar">T. Por Entregar</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uptxtTPorEntregar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTPorEntregar" runat="server" CssClass="textbox_50px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtProducto" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtPrecioUnitario">Precio:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtPrecioUnitario" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPrecioUnitario" CssClass="textbox_100px  validate[required, custom[positiveNumber6]]" TabIndex="18" MaxLength="15"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
<asp:AsyncPostBackTrigger ControlID="txtProducto" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCantidad">Cantidad:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtCantidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidad" CssClass="textbox_100px validate[required,custom[positiveNumber6]]" TabIndex="19" MaxLength="15"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnGuardarDOC" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnGuardarDOC" Text="Guardar" CssClass="boton" OnClick="btnGuardarDOC_Click" TabIndex="20" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAbrirModalDOC" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarDetalleOrdenCompra" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarOrden" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana para Surtir Productos Automatico -->
<div id="contenedorVentanaCantidadProducto" class="modal">
<div id="ventanaCantidadProducto" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCantidadProducto" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCantidadProducto" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="CantidadProducto" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna3x">
<div class="header_seccion">
<h2>Abastecer Producto</h2>
</div>
<div class="renglon3x" style="float:left">
<div class="etiqueta">
<label for="lblProductoInv">Producto</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblProductoInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblProductoInv" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="txtCantidadInv">Cantidad</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtCantidadInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidadInv" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber6]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
<asp:AsyncPostBackTrigger ControlID="rbLote" />
<asp:AsyncPostBackTrigger ControlID="rbSerie" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtPrecioInv">Precio</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtPrecioInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPrecioInv" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber6]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x" style="float:left;">
<div class="etiqueta">
<label for="txtLote">Lote</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtLote" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLote" runat="server" MaxLength="50" CssClass="textbox_100px"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
<asp:AsyncPostBackTrigger ControlID="rbLote" />
<asp:AsyncPostBackTrigger ControlID="rbSerie" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtFecCad">Caducidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecCad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecCad" runat="server" MaxLength="10" CssClass="textbox validate[custom[date]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
<asp:AsyncPostBackTrigger ControlID="rbLote" />
<asp:AsyncPostBackTrigger ControlID="rbSerie" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="control_100px">
<asp:UpdatePanel ID="uprbLote" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbLote" runat="server" Text="Por Lote" Checked="true" GroupName="General"
OnCheckedChanged="rbLote_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbSerie" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uprbSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbSerie" runat="server" Text="Por Serie" Checked="false" GroupName="General"
OnCheckedChanged="rbLote_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbLote" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnSurtir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSurtir" runat="server" Text="Surtir" CssClass="boton" OnClick="btnSurtir_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div id="contenedorInventarioTemporal" runat="server" class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvInventarioTemp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView runat="server" ID="gvInventarioTemp" CssClass="gridview" AllowPaging="false"
ShowFooter="true" AutoGenerateColumns="false" PageSize="150" Width="98%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField HeaderText="Serie" SortExpression="Serie">
<ItemTemplate>
<asp:Label ID="lblSerie" runat="server" Text='<%# Eval("Serie")%>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtSerieE" runat="server" CssClass="textbox_100px" Text='<%# Eval("Serie")%>'></asp:TextBox>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Fecha Caducidad" SortExpression="FechaCaducidad">
<ItemTemplate>
<asp:Label ID="lblFechaCaducidad" runat="server" Text='<%# Eval("FechaCaducidad", "{0:dd/MM/yyyy}")%>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtFechaCaducidadE" runat="server" CssClass="textbox_100px scriptInvTemp validate[custom[date]]" Text='<%# Eval("FechaCaducidad", "{0:dd/MM/yyyy}")%>'></asp:TextBox>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarInvTemp" runat="server" Text="Editar" CommandName="Editar"
OnClick="lnkActulizarInvTemp_Click"></asp:LinkButton>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lnkGuardarInvTemp" runat="server" Text="Guardar" CommandName="Guardar" CssClass="scriptGuardaInvTemp"
OnClick="lnkActulizarInvTemp_Click"></asp:LinkButton>
<asp:LinkButton ID="lnkCancelarInvTemp" runat="server" Text="Cancelar" CommandName="Cancelar"
OnClick="lnkActulizarInvTemp_Click"></asp:LinkButton>
</EditItemTemplate>
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
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
<asp:AsyncPostBackTrigger ControlID="rbLote" />
<asp:AsyncPostBackTrigger ControlID="rbSerie" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana de Insercción y Vizualización de Facturas Ligadas -->
<div id="contenedorVentanaFacturasLigadas" class="modal">
<div id="ventanaFacturasLigadas" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarVentanaFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaFL" runat="server" CommandName="FacturasLigadas" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Ligadas</h2>
</div>
<div class="columna2x">
<div id="contenedorFacturaXML">Arrastre y suelte sus archivos XML desde su carpeta a este cuadro.</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFactura" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAgregarFactura_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoFL">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFL" CssClass="dropdown_100px" runat="server" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Ordenado:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
TabIndex="37" AutoGenerateColumns="false" Width="90%" PageSize="25">
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
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="ImpTrasladado" HeaderText="Importe Trasladado" SortExpression="ImpTrasladado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="ImpRetenido" HeaderText="Importe Retenido" SortExpression="ImpRetenido" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
<asp:AsyncPostBackTrigger ControlID="lkbFacturaProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFac" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacXML" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana de Confirmación de la Factura -->
<div id="contenedorVentanaConfirmacionFactura" class="modal">
<div id="ventanaConfirmacionFactura" class="contenedor_ventana_confirmacion_arriba">
<div class="columna3x">
<asp:UpdatePanel ID="upmtvConfirmacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvConfirmacion" runat="server" ActiveViewIndex="0">
<asp:View ID="vwAcepta" runat="server">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Existe una Factura ligada a la Orden. ¿Desea sobreescribirla?</h2>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarFac" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnAceptarFac_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarFac" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarFac_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwRechaza" runat="server">
<div class="header_seccion">
<img src="../Image/ExclamacionRoja.png" />
<asp:UpdatePanel ID="uplblMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2><asp:Label ID="lblMensaje" runat="server"></asp:Label></h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbFacturaProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCerrarFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCerrarFac" runat="server" Text="Cerrar" CssClass="boton_cancelar"
OnClick="btnCancelarFac_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbFacturaProveedor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Edición de Inventario -->
<div id="contenedorVentanaEdicionInventario" class="modal">
<div id="ventanaEdicionInventario" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarInventario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarInventario" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="ProductoInventario">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna3x">
<div class="header_seccion">
<h2>Productos de Inventario</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoProductoInv">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoProductoInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoProductoInv" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoProductoInv_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoProductoInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoProductoInv" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvProductosInventario" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarProdInv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarProdInv" runat="server" Text="Exportar" OnClick="lnkExportar_Click" CommandName="ProductoInv"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvProductosInventario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvProductosInventario" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" OnSorting="gvProductosInventario_Sorting" OnPageIndexChanging="gvProductosInventario_PageIndexChanging"
TabIndex="37" AutoGenerateColumns="false" Width="90%" PageSize="25">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:Label ID="lblEstatus" runat="server" Text='<%# Eval("Estatus") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:Label ID="lblEstatusE" runat="server" Text='<%# Eval("Estatus") %>'></asp:Label>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Producto" SortExpression="Producto">
<ItemTemplate>
<asp:Label ID="lblProducto" runat="server" Text='<%# Eval("Producto") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:Label ID="lblProductoE" runat="server" Text='<%# Eval("Producto") %>'></asp:Label>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
<ItemTemplate>
<asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("Cantidad") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:Label ID="lblCantidadE" runat="server" Text='<%# Eval("Cantidad") %>'></asp:Label>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Lote" SortExpression="Lote">
<ItemTemplate>
<asp:Label ID="lblLote" runat="server" Text='<%# Eval("Lote") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtLoteE" runat="server" CssClass="textbox_100px" Text='<%# Eval("Lote") %>'></asp:TextBox>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Serie" SortExpression="Serie">
<ItemTemplate>
<asp:Label ID="lblSerie" runat="server" Text='<%# Eval("Serie") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtSerieE" runat="server" CssClass="textbox_100px" Text='<%# Eval("Serie") %>'></asp:TextBox>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Fecha Caducidad" SortExpression="FechaCaducidad">
<ItemTemplate>
<asp:Label ID="lblFecCad" runat="server" Text='<%# Eval("FechaCaducidad", "{0:dd/MM/yyyy}") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtFecCad" runat="server" MaxLength="10" CssClass="textbox_100px scriptInventario validate[custom[date]]" Text='<%# string.Format("{0:dd/MM/yyyy}", Eval("FechaCaducidad")) %>'></asp:TextBox>
</EditItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkCambiar" runat="server" Text="Cambiar" OnClick="lnkActualizar_Click" CommandName="Cambiar"></asp:LinkButton>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lnkGuardar" runat="server" CssClass="scriptGuardaInventario" Text="Guardar" OnClick="lnkActualizar_Click" CommandName="Guardar"></asp:LinkButton>
<asp:LinkButton ID="lnkCancelar" runat="server" Text="Cancelar" OnClick="lnkActualizar_Click" CommandName="Cancelar"></asp:LinkButton>
</EditItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoProductoInv" />
<asp:AsyncPostBackTrigger ControlID="gvOrdenCompraDetalle" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana Facturación Diferencia -->
<div id="contenedorVentanaFacturacionDiferencia" class="modal">
<div id="ventanaFacturacionDiferencia" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplkbCerrarFacturaDiferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarFacturaDiferencia" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="FacturaDiferencia">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div>
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Existe una Diferencia en los montos de la Factura. ¿Desea continuar?</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblSubTotalFac">Sub Total</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblSubTotalFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSubTotalFac" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblSubTotalXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSubTotalXML" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTrasladadoFac">Trasladado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTrasladadoFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTrasladadoFac" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTrasladadoXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTrasladadoXML" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblRetenidoFac">Retenido</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblRetenidoFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRetenidoFac" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblRetenidoXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRetenidoXML" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTotalFac">Total</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTotalFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalFac" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTotalXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalXML" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarFacXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarFacXML" runat="server" Text="Aceptar" OnClick="btnAceptarFacXML_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>

<!-- Contenedor de Control de Envio por E-mail -->
<div id="contenedorVentanaEnvioOrdenCompra" class="modal">
<div id="ventanaEnvioOrdenCompra" class="contenedor_ventana_confirmacion_arriba" style="min-width:485px; width:485px">
    <asp:UpdatePanel ID="upwucEnvioOrdenCompra" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <tectos:WucEnvioEmail ID="wucEnvioOrdenCompra" runat="server"
                LkbCerrarEmail_Click="wucEnvioOrdenCompra_CerrarEmail_Click" 
                BtnEnviarEmail_Click="wucEnvioOrdenCompra_EnviarEmail_Click"></tectos:WucEnvioEmail>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbEnvioEmail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>
</asp:Content>
