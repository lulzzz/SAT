<%@ Page Title="Servicio" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Servicio.aspx.cs" Inherits="SAT.Documentacion.Servicio" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="../UserControls/wucProducto.ascx" TagName="wucProducto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagPrefix="tectos" TagName="wucClasificacion" %>
<%@ Register Src="~/UserControls/wucParada.ascx" TagPrefix="tectos" TagName="wucParada" %>
<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagPrefix="tectos" TagName="wucFacturadoConcepto" %>
<%@ Register Src="~/UserControls/wucFacturado.ascx" TagPrefix="tectos" TagName="wucFacturado" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="tectos" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucEvidenciaSegmento.ascx" TagPrefix="tectos" TagName="wucEvidenciaSegmento" %>
<%@ Register Src="~/UserControls/wucServicioCopia.ascx" TagPrefix="tectos" TagName="wucServicioCopia" %>
<%@ Register Src="~/UserControls/wucTemperaturaServicio.ascx" TagPrefix="tectos" TagName="wucTemperaturaServicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada -->
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Biblioteca para ventana modal  -->
<script type="text/javascript" src="../Scripts/jquery.plainmodal.min.js" charset="utf-8"></script>
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQuery();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQuery() {

// *** Añadiendo lógica de validación de Campos *** //
//Disparadores de validación de Encabezado de Servicio
$(document).ready(function () {
//Función de validación de encabezado de servicio
var validacionServicio = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtUbicacionCarga.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtUbicacionDescarga.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtCitaCarga.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtCitaDescarga.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
};

    //Función de validación de campos
    var validacionNoFacturable = function (evt) {
        var isValidP1 = !$("#<%=txtMotivoNoFacturable.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
    //Función de validación de campos
    var validacionCancelacionServicio = function (evt) {
        var isValidP1 = !$("#<%=txtMotivoCancelacion.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
//Botón No Facturable 
$("#<%=btnNoFacturable.ClientID %>").click(validacionNoFacturable);
//Botón No Facturable 
    $("#<%=btnAceptarCancelacion.ClientID %>").click(validacionCancelacionServicio);
//Menú Guardar
$("#<%= lkbGuardar.ClientID %>").click(validacionServicio);
//Botón Guardar
$("#<%= btnGuardar.ClientID %>").click(validacionServicio);

});

// *** Catálogos Autocomplete *** //
$(document).ready(function () {

//Ubicación de Carga
$("#<%=txtUbicacionCarga.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>',
select: function (event, ui) {
//Asignando Selección al Valor del Control
$("#<%=txtUbicacionCarga.ClientID%>").val(ui.item.value);
//Causando Actualización del Control
__doPostBack('<%= txtUbicacionCarga.UniqueID %>', '');
}
});

//Ubicación de Descarga
$("#<%=txtUbicacionDescarga.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>',
select: function (event, ui) {
//Asignando Selección al Valor del Control
$("#<%=txtUbicacionDescarga.ClientID%>").val(ui.item.value);
//Causando Actualización del Control
__doPostBack('<%= txtUbicacionDescarga.UniqueID %>', '');
}
});

//Cliente
$("#<%=txtCliente.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>',
select: function (event, ui) {
//Asignando Selección al Valor del Control
$("#<%=txtCliente.ClientID%>").val(ui.item.value);
//Causando Actualización del Control
__doPostBack('<%= txtCliente.UniqueID %>', '');
}
});
});

// *** Fecha de carga, descarga (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$(document).ready(function () {
$("#<%=txtCitaCarga.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtCitaDescarga.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});

// *** Visualización de ventana de guardado de servicios maestros  *** //
$(document).ready(function () {
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= lkbHacerMaestro.ClientID%>").unbind("click");
$("#<%= lkbHacerMaestro.ClientID%>").click(function (evt) {
//Mostrando Ventana Modal 
//$("#hacer_maestro").plainModal('open', { duration: 500 });                    
$("#hacer_maestro").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarMaestro.ClientID%>").unbind("click");
$("#<%= btnCancelarMaestro.ClientID%>").click(function () {
//Ocultando ventana modal del copia
//$("#hacer_maestro").plainModal('close', { duration: 500 });
$("#hacer_maestro").animate({ width: "toggle" });
});

//Función de validación de identificador de servicio maestro
var validacionMaestro = function (evt) {
var isValid = !$("#<%=txtDescripcionMaestro.ClientID%>").validationEngine('validate');
//Si no hay errores,
//Ocultando ventana modal del copia
if (isValid)
//$("#hacer_maestro").plainModal('close', { duration: 500 });
$("#hacer_maestro").animate({ width: "toggle" });
//Devolviendo resultado
return isValid;
};

 //Botón Guardar Servicio Maestro
    $("#<%= btnGuardarMaestro.ClientID %>").unbind("click");
$("#<%= btnGuardarMaestro.ClientID %>").click(validacionMaestro);
});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQuery();

</script>
<div id="encabezado_forma">
<img src="../Image/Documentacion.png" />
<h1>Documentación Servicio</h1>
</div>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbNuevo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:UpdatePanel ID="uplkbGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />

<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbEditar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li><li>
<asp:UpdatePanel ID="uplkbCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCancelar" runat="server" Text="Cancelar" OnClick="lkbElementoMenu_Click" CommandName="Cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="gray">
<a href="#" class="fa fa-book "></a>
<ul>
<li>
<asp:LinkButton ID="lkbAbrirMaestro" runat="server" Text="Abrir Maestro" OnClick="lkbElementoMenu_Click" CommandName="AbrirMaestro" />
</li>
<li>
<asp:UpdatePanel ID="uplkbHacerMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbHacerMaestro" runat="server" Text="Hacer Maestro" OnClick="lkbElementoMenu_Click" CommandName="HacerMaestro" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopiarMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbCopiarMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCopiarMaestro" runat="server" Text="Copiar Maestro" OnClick="lkbElementoMenu_Click" CommandName="CopiarMaestro" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnNoFacturable" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbArchivos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
<li>
<asp:LinkButton ID="lkbReferenciaTemperaturas" runat="server" Text="Temperaturas" OnClick="lkbElementoMenu_Click" CommandName="Temperaturas" /></li>
<li>
<asp:UpdatePanel ID="uplkbNoFacturable" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNoFacturable" runat="server" Text="No Facturable" OnClick="lkbElementoMenu_Click" CommandName="NoFacturable" />
</ContentTemplate>
<Triggers>
           
</Triggers></asp:UpdatePanel></li>
</ul>
</li>
</ul>
</nav>

<div class="contenedor_controles">
<div class="contenedor_identificadores">
<article>
<label for="txtCompania">Compañia</label>
<img src="../Image/Compania.png" />
<div class="control">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x" Enabled="false" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</article>

<article>
<label for="txtIdServicio">No. Servicio</label>
<img src="../Image/Servicio.png" />
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox2x" Enabled="False" TabIndex="2"></asp:TextBox>
<asp:Label ID="lblCopiaDe" runat="server" CssClass="label_error" Text="Copia de: 27893 - MAESTRO"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</article>
<article>
<label for="txtEstatus">Estatus Servicio</label>
<img src="../Image/Estatus1.png" />
<div class="control">
<asp:UpdatePanel ID="uptxtEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtEstatus" runat="server" CssClass="textbox2x" Enabled="False" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID ="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</article>
</div>

<div class="contenedor_origen_destino">
<article>
<img src="../Image/carga.png" />
<h4>Datos de Carga</h4>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionCarga">Sitio Carga</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionCarga" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" OnTextChanged="txtUbicacionCarga_TextChanged" AutoPostBack="true" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDireccionCarga">Dirección</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtDireccionCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDireccionCarga" runat="server" CssClass="textbox2x" Enabled="False" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCoordenadasCarga">Geoubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCoordenadasCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCoordenadasCarga" runat="server" CssClass="textbox" Enabled="False" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplnkMapaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="lkbMapaCarga" runat="server" Target="_blank" Text="Ver Mapa" TabIndex="7">
<img src="../Image/ImagenPatio.png" />Ver Mapa</asp:HyperLink>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCitaCarga">Cita Carga</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCitaCarga" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtCitaCarga" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</article>
<article>
<img src="../Image/descarga.png" />
<h4>Datos de Descarga</h4>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacionDescarga">Sitio Descarga</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacionDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionDescarga" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" AutoPostBack="True" OnTextChanged="txtUbicacionDescarga_TextChanged" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDireccionDescarga">Dirección</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtDireccionDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDireccionDescarga" runat="server" CssClass="textbox2x" Enabled="False" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionDescarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCoordenadasDescarga">Geoubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCoordenadasDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCoordenadasDescarga" runat="server" CssClass="textbox" Enabled="False" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionDescarga" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplnkMapaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="lkbMapaDescarga" runat="server" Target="_blank" TabIndex="9">
<img src="../Image/ImagenPatio.png" />Ver Mapa</asp:HyperLink>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtUbicacionDescarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCitaDescarga">Cita Descarga</label><label for="txtCitaDescarga"></label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCitaDescarga" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</article>
</div>

<div class="contenedor_cliente">
<article>
<img src="../Image/Cliente.png" />
<h4>Datos Cliente</h4>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">
Cliente
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" AutoPostBack="true" OnTextChanged="txtCliente_TextChanged" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDireccionCliente">Direccion</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtDireccionCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDireccionCliente" runat="server" CssClass="textbox2x" Enabled="False" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">RFC<label for="txtRFC"></label></div>
<div class="control">
<asp:UpdatePanel ID="uptxtRFC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRFC" runat="server" CssClass="textbox" Enabled="False" TabIndex="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label>Limite</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblLimite" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblLimite" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Adeudo</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblTotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Saldo</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblSaldo" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorServicio" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

</article>
<article>
<img src="../Image/Comentarios.png" />
<h4>Referencias Cliente</h4>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCartaPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox2x" TabIndex="17"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtCliente" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferenciaCliente">Referencia Cliente</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferenciaCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferenciaCliente" runat="server" CssClass="textbox2x" TabIndex="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" TabIndex="19"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" CommandName="Cancelar" OnClick="btnGuardarCancelar_Click" TabIndex="21" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" CommandName="Guardar" OnClick="btnGuardarCancelar_Click" TabIndex="20" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</article>
</div>

<section class="acordion">
<div>
<input type="radio" name="secciones" id="rdbParadas" />
<label for="rdbParadas">
<img src="../Image/paradas.png" />Paradas</label>
<article>
<asp:UpdatePanel ID="upwucParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucParada runat="server" ID="wucParada"  OnClickInsertarEvento="wucParada_ClickInsertarEvento" OnClickAgregarArriba="wucParada_ClickAgregarArriba" OnClickAgregarAbajo="wucParada_ClickAgregarAbajo" OnClickEditar="wucParada_ClickEditar" OnClickEliminar="wucParada_ClickEliminar" TabIndex="22" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbProducto" />
<label for="rdbProducto">
<img src="../Image/Producto.png" />Producto</label>
<article>
<asp:UpdatePanel ID="upwucProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucProducto ID="wucProducto" runat="server"    OnClickGuardarProducto="wucProducto_ClickGuardarProducto" OnClickEliminarProducto="wucProducto_ClickEliminarProducto" TabIndex="23" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="wucParada" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbClasificacion" />
<label for="rdbClasificacion">
<img src="../Image/Clasificacion.png" />Clasificación</label>
<article>
    <div class="contenido_pestañas_documentacion">
<asp:UpdatePanel ID="upwucClasificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucClasificacion runat="server" ID="wucClasificacion" OnClickGuardar="wucClasificacion_ClickGuardar" OnClickCancelar="wucClasificacion_ClickCancelar" TabIndex="24" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
        </div>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbFacturacion" />
<label for="rdbFacturacion">
<img src="../Image/Facturacion.png" />Facturación</label>
<article>
<asp:UpdatePanel ID="upwucFacturado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucFacturado runat="server" ID="wucFacturado" OnClickGuardarFactura="wucFacturado_ClickGuardarFactura" OnClickAplicarTarifa="wucFacturado_ClickAplicarTarifa" TabIndex="26" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="wucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNoFacturable" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbFacturacionCargos" />
<label for="rdbFacturacionCargos">
<img src="../Image/FacturacionCargos.png" />Cargos del Servicio</label>
<article>
<asp:UpdatePanel ID="upwucFacturadoConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucFacturadoConcepto runat="server" ID="wucFacturadoConcepto" OnClickGuardarFacturaConcepto="wucFacturadoConcepto_ClickGuardarFacturaConcepto" OnClickEliminarFacturaConcepto="wucFacturadoConcepto_ClickEliminarFacturaConcepto" TabIndex="27" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="wucFacturado" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbReferencia" />
<label for="rdbReferencia">
<img src="../Image/Modulos.png" />Referencias del Servicio</label>
<article>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciaViaje" runat="server" Enable="true" TabIndex="28"
OnClickGuardarReferenciaViaje="ucReferenciaViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="ucReferenciaViaje_ClickEliminarReferenciaViaje"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="wucFacturado" />
<asp:AsyncPostBackTrigger ControlID="btnNoFacturable" />
    <asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
<div>
<input type="radio" name="secciones" id="rdbSegmentos" />
<label for="rdbSegmentos">
<img src="../Image/Documento.png" />Evidencias</label>
<article>
<asp:UpdatePanel ID="upucEvidenciaSegmento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucEvidenciaSegmento ID="ucEvidenciaSegmento" runat="server" TabIndex="29" Enable="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="wucFacturado" />
<asp:AsyncPostBackTrigger ControlID="wucParada" />
</Triggers>
</asp:UpdatePanel>
</article>
</div>
</section>
</div>
<div id="contenedorVentanaCopiaServicio" class="modal">
    <div id="ventanaCopiaServicio" class="contenedor_modal_seccion_completa_arriba">
        <asp:UpdatePanel ID="upucServicioCopia" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <tectos:wucServicioCopia ID="ucServicioCopia" runat="server" OnClickGuardarServicioCopia="ucServicioCopia_ClickGuardarServicioCopia"
                    OnClickCancelarServicioCopia="ucServicioCopia_ClickCancelarServicioCopia" Contenedor="#ventanaCopiaServicio" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lkbCopiarMaestro" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div id="hacer_maestro" class="contenedor_hacer_maestro" style="width:765px;">
<article>
<asp:Panel runat="server">
<h4>Guardar como Servicio Maestro</h4>
<asp:Panel ID="pnlHacerMaestro" runat="server" DefaultButton="btnGuardarMaestro">
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcionMaestro">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcionMaestro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcionMaestro" runat="server" CssClass="textbox2x validate[required, maxSize[100]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarMaestro" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnCancelarMaestro" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:Button ID="btnGuardarMaestro" runat="server" CssClass="boton" CommandName="GuardarMaestro" OnClick="btnGuardarCancelar_Click" Text="Aceptar" />
</div>
</div>
</div>
</asp:Panel>
</asp:Panel>
</article>
</div>
<div id="contenedorVentanaTemperatura" class="modal">
<div id="ventanaTemperatura" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTemperaturas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTemperaturas" runat="server" OnClick="lkbCerrarTemperaturas_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucTemperaturaUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucTemperaturaServicio ID="wucTemperaturaUnidad" runat="server" Enabled="true" TabIndex="30"
OnClickGuardarTemperaturas="wucTemperaturaUnidad_ClickGuardarTemperaturas" vizualizaBotonGuardar="true"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbHacerMaestro" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbReferenciaTemperaturas" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE SERVICIO NO FACTURABLE -->
<div id="confirmacionNoFacturable" class="modal">
<div id="NoFacturable" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarNoFacturable" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarNoFacturable" runat="server" OnClick="lkbCerrarNoFacturable_Click" CommandName="NoFacturable" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Desea cambiar el estatus del servicio a No Facturable? </h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivoNoFacturable">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoNoFacturable" runat="server" UpdateMode="Conditional">
<ContentTemplate>
 <asp:TextBox ID="txtMotivoNoFacturable" runat="server" TextMode="MultiLine"  Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox></div></div>
</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lkbNoFacturable" />
    </Triggers>
</asp:UpdatePanel>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnNoFacturable" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnNoFacturable" Text="Si" CssClass="boton" OnClick="btnNoFacturable_Click" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div></div></div>
<!-- VENTANA MODAL CANCELACIÓN DE SERVICIO -->
<div id="confirmacionCancelacionModal" class="modal">
<div id="confirmacionCancelacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" alt="SeleccionTarea" />
<h3>Cancelación de Servicio</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<label class="mensaje_modal">Esta acción no es reversible. ¿Desea Continuar?</label>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivoNoFacturable">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivoCancelacion" runat="server" TextMode="MultiLine"  CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox></div></div>
</ContentTemplate>
<Triggers>    
   <asp:AsyncPostBackTrigger ControlID="lkbCancelar" /> 
</Triggers>
</asp:UpdatePanel>
</div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarCancelacion" runat="server" CssClass="boton" OnClick="btnCancelacion_Click" CommandName="Cancelar" Text="Cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton"> 
<asp:UpdatePanel ID="upbtnAceptarCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelacion" runat="server" CssClass="boton" OnClick="btnCancelacion_Click" CommandName="Aceptar" Text="Aceptar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>
</asp:Content>

