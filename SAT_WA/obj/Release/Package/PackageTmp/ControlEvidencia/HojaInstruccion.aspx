<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HojaInstruccion.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.ControlEvidencia.HojaInstruccion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos Autocomplete, Zoom y Validadores JQuery -->
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<!--<link href="../CSS/jquery.autocomplete.css" rel="stylesheet" type="text/css" />-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada  y Zoom-->
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryHI();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryHI() {

//Validación campos Hoja de Instrucción
$(document).ready(function () {
//Función de validación 
var validacionHojaInstruccion = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtDescipcion.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtRemitente.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtConsignatario.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3 && isValid4;
};

//Menú Guardar
$("#<%= lkbGuardar.ClientID %>").click(validacionHojaInstruccion);
//Botón Guardar
$("#<%= btnAceptar.ClientID %>").click(validacionHojaInstruccion);

});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= lkbEliminar.ClientID%>").unbind("click");
$("#<%= lkbEliminar.ClientID%>").click(function () {
//Mostrando ventana modal 
$("#contenidoConfirmacionEliminarHI").animate({ width: "toggle" });
$("#confirmacionEliminarHI").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarEliminarHI.ClientID%>").unbind("click");
$("#<%= btnCancelarEliminarHI.ClientID%>").click(function (evt) {
evt.preventDefault()
//Ocultando ventana modal 
$("#contenidoConfirmacionEliminarHI").animate({ width: "toggle" });
$("#confirmacionEliminarHI").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarEliminarHIDocumento.ClientID%>").unbind("click");
$("#<%= btnCancelarEliminarHIDocumento.ClientID%>").click(function (evt) {
evt.preventDefault()
//Ocultando ventana modal 
$("#contenidoConfirmacionEliminarHIDocumento").animate({ width: "toggle" });
$("#confirmacionEliminarHIDocumento").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarEliminarHIAccesorio.ClientID%>").unbind("click");
$("#<%= btnCancelarEliminarHIAccesorio.ClientID%>").click(function (evt) {
evt.preventDefault()
//Ocultando ventana modal 
$("#contenidoConfirmacionEliminarHIAccesorio").animate({ width: "toggle" });
$("#confirmacionEliminarHIAccesorio").animate({ width: "toggle" });
});
// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtRemitente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
$("#<%=txtConsignatario.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=6&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryHI();
</script>
<div id="encabezado_forma">
<img src="../Image/BusquedaEvidencia.png" />
<h1>Hoja de Instrucciones</h1>        
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbCopiaHI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbCopia" CommandName="Copiar" OnClick="lkbElementoMenu_Click">Copiar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
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
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
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
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbImprimir" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
</Triggers>
</asp:UpdatePanel>
</li>

</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-truck"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbMapaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbMapaCarga" runat="server" Text="Mapa Carga" OnClick="lkbElementoMenu_Click" CommandName="MapaCarga" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbMapaCarga" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbMapaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbMapaDescarga" runat="server" Text="Mapa Descarga" OnClick="lkbElementoMenu_Click" CommandName="MapaDescarga" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbMapaDescarga" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
</ul>
</nav>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/AgregarRegistro.png" />
<h2>Agregar hoja de instruccion</h2>
</div>
<div class="columna2x">
<div class="renglon3x">
<div class="etiqueta">
<label class="Label" for="txtDescipcionn">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescipcion" runat="server">
<ContentTemplate>
<asp:TextBox ID="txtDescipcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="1" MaxLength="200"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblEstatusCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblEstatusCopia" runat="server" CssClass="label_negrita">[ Modo Copia ]</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCliente" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtRemitente">Remitente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtRemitente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRemitente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
           
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtConsignatario">Destinatario</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtConsignatario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtConsignatario" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlTerminalCobro">
Lugar de Cobro</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTerminalCobro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTerminalCobro" runat="server" CssClass="dropdown2x" TabIndex="6">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblID" Visible="false">ID</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCompania" runat="server">
<ContentTemplate>
<asp:TextBox ID="txtCompania" CssClass="textbox2x" runat="server" TabIndex="2" Enabled="false" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass=" boton_cancelar" Text="Cancelar" TabIndex="8"
OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar"
ValidationGroup="Encabezado" OnClick="btnAceptar_Click" TabIndex="7" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x" style="width: auto">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>     
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Evidencias requeridas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewDocumentoHoja">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewDocumentoHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewDocumentoHoja" runat="server" OnSelectedIndexChanged="gvDocumentoHoja_OnSelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarServicios">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewDocumentoHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewDocumentoHoja" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDocumentoHoja" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelDocumentoHoja" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelDocumentoHoja" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelDocumentoHoja_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelDocumentoHoja" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvDocumentoHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDocumentoHoja" CssClass="gridview" OnPageIndexChanging="gvDocumentoHoja_OnpageIndexChanging" OnSorting="gvDocumentoHoja_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True"
PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id"
ReadOnly="True" />
<asp:TemplateField HeaderText="Documento" SortExpression="Documento">
<ItemTemplate>
<asp:Label ID="lblDocumentoL" runat="server" Text='<%# Eval("Documento") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlDocumentoE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlDocumentoN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Evento" SortExpression="Evento">
<ItemTemplate>
<asp:Label ID="lblEventoL" runat="server" Text='<%# Eval("Evento") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlEventoE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlEventoN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Acción" SortExpression="Accion">
<ItemTemplate>
<asp:Label ID="lblAccionL" runat="server" Text='<%# Eval("Accion") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlAccionE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlAccionN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Evidencia" SortExpression="Evidencia">
<ItemTemplate>
<asp:Label ID="lblEvidenciaL" runat="server" Text='<%# Eval("Evidencia") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:CheckBox ID="chkEvidenciaE" runat="server"
    Checked='<%# Eval("*Evidencia") %>' />
</EditItemTemplate>
<FooterTemplate>
<asp:CheckBox ID="chkEvidenciaN" runat="server" Enabled='<%# asignaEstatusControlGridView() %>' />
</FooterTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Formato" SortExpression="Formato">
<ItemTemplate>
<asp:Label ID="lblFormatoL" runat="server" Text='<%# Eval("Formato") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlFormatoE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlFormatoN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sello"
SortExpression="Sello">
<ItemTemplate>
<asp:Label ID="lblSelloL" runat="server" Text='<%# Eval("Sello") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:CheckBox ID="chkSelloE" runat="server" Checked='<%# Eval("*Sello") %>' />
</EditItemTemplate>
<FooterTemplate>
<asp:CheckBox ID="chkSelloN" runat="server" Enabled='<%# asignaEstatusControlGridView() %>' />
</FooterTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Observación" SortExpression="Observacion">
<ItemTemplate>
<asp:Label ID="lblObservacionL" runat="server"
    Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Observacion").ToString(), 30, "...") %>' ToolTip='<%# Eval("Observacion") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtObservacionE" runat="server" CssClass="textbox"
    Text='<%# Eval("Observacion") %>' TextMode="MultiLine"></asp:TextBox>
</EditItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtObservacionN" runat="server" CssClass="textbox" Enabled='<%# asignaEstatusControlGridView() %>'
    TextMode="MultiLine"></asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEdicionDoc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
    CommandName="Editar" OnClick="lkbEdicionDoc_Click">Editar</asp:LinkButton><br />
<asp:LinkButton ID="lkbEliminarDoc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
    CommandName="Eliminar" OnClick="lkbEdicionDoc_Click">Eliminar</asp:LinkButton>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lkbGuardarDocE" runat="server" CommandName="Guardar"
    ValidationGroup="DocumentoE" OnClick="lkbGuardarDocE_Click">Guardar</asp:LinkButton>
</EditItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lkbGuardarDocN" runat="server"
    ValidationGroup="DocumentoN" Enabled='<%# asignaEstatusControlGridView() %>'
    OnClick="lkbGuardarDocN_Click">Guardar</asp:LinkButton>
</FooterTemplate>
<FooterStyle
HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacoraDoc" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:LinkButton ID="lkbBitacoraDoc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
            CommandName="Bitacora" OnClick="lkbEdicionDoc_Click">Bitácora</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbBitacoraDoc" />
    </Triggers>
</asp:UpdatePanel>
<br />
<asp:UpdatePanel ID="uplkbReferenciasDoc" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:LinkButton ID="lkbReferenciasDoc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
            CommandName="Referencias" OnClick="lkbEdicionDoc_Click">Referencias</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbReferenciasDoc" />
    </Triggers>
</asp:UpdatePanel>
<br />
<asp:UpdatePanel ID="uplkbImagenDoc" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:LinkButton ID="lkbImagenDoc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
            CommandName="Imagen" OnClick="lkbEdicionDoc_Click">Imagen</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbImagenDoc" />
    </Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lkbCancelarDocE" runat="server" CommandName="Cancelar"
    OnClick="lkbGuardarDocE_Click">Cancelar</asp:LinkButton>
</EditItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewDocumentoHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIDocumento" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblError2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError2" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvDocumentoHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIDocumento" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Imagenes_docs.png" />
<h2>Evidencias Muestra</h2>
</div>
<asp:Button ID="btnActualizarImagenes" runat="server" CssClass="boton" Text="Actualizar" OnClick="btnActualizarImagenes_Click" />
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
</asp:HyperLink>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvDocumentoHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIDocumento" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>                                
<asp:DataList ID="dtlImagenDocumentos" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id") + " " + Eval("Documento") + " (" + Eval("Evento") + ")" %>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
        </asp:LinkButton>
    </ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:DataList>                                
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvDocumentoHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIDocumento" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarImagenes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_media_seccion">
<div class="header_seccion">
<img src="../Image/equipo_seguridad.png" />
<h2>Accesorios requeridos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewAccesorioHoja">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewAccesorioHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewAccesorioHoja" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewAccesorioHoja_SelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarServicios">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioAccesorioHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioAccesorioHoja" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAccesorioHoja" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelAccesorioHoja" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelAccesorioHoja" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelAccesorioHoja_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelAccesorioHoja" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_media_seccion">
<asp:UpdatePanel ID="upgvAccesorioHoja" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAccesorioHoja" CssClass="gridview" OnPageIndexChanging="gvAccesorioHoja_PageIndexChanging" OnSorting="gvAccesorioHoja_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True"
PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id"
ReadOnly="True" />
<asp:TemplateField HeaderText="Accesorio" SortExpression="Accesorio">
<ItemTemplate>
<asp:Label ID="lblAccesorioL" runat="server" Text='<%# Eval("Accesorio") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlAccesorioE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlAccesorioN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Evento" SortExpression="Evento">
<ItemTemplate>
<asp:Label ID="lblEventoL" runat="server" Text='<%# Eval("Evento") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:DropDownList ID="ddlEventoE" runat="server" CssClass="dropdown">
</asp:DropDownList>
</EditItemTemplate>
<FooterTemplate>
<asp:DropDownList ID="ddlEventoN" runat="server" CssClass="dropdown" Enabled='<%# asignaEstatusControlGridView() %>'>
</asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Observación" SortExpression="Observacion">
<ItemTemplate>
<asp:Label ID="lblObservacionL" runat="server"
    Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Observacion").ToString(), 30, "...") %>' ToolTip='<%# Eval("Observacion") %>'></asp:Label>
</ItemTemplate>
<EditItemTemplate>
<asp:TextBox ID="txtObservacionE" runat="server" CssClass="textbox"
    Text='<%# Eval("Observacion") %>' TextMode="MultiLine"></asp:TextBox>
</EditItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtObservacionN" runat="server" CssClass="textbox" Enabled='<%# asignaEstatusControlGridView() %>'
    TextMode="MultiLine"></asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEdicionAcc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
    CommandName="Editar" OnClick="lkbEdicionAcc_Click">Editar</asp:LinkButton>
<asp:LinkButton ID="lkbEliminarAcc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
    CommandName="Eliminar" OnClick="lkbEdicionAcc_Click">Eliminar</asp:LinkButton>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lkbGuardarAccE" runat="server" CommandName="Guardar"
    ValidationGroup="DocumentoE" OnClick="lkbGuardarAccE_Click">Guardar</asp:LinkButton>
</EditItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lkbGuardarAccN" runat="server"
    ValidationGroup="DocumentoN" Enabled='<%# asignaEstatusControlGridView() %>'
    OnClick="lkbGuardarAccN_Click">Guardar</asp:LinkButton>
</FooterTemplate>
<FooterStyle
HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacoraACC" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:LinkButton ID="lkbBitacoraAcc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
            CommandName="Bitacora" OnClick="lkbEdicionAcc_Click">Bitácora</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbBitacoraAcc" />
    </Triggers>
</asp:UpdatePanel>
<br />
<asp:UpdatePanel ID="uplkbReferenciasAcc" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <asp:LinkButton ID="lkbReferenciasAcc" runat="server" CausesValidation="False" Enabled='<%# asignaEstatusControlGridView() %>'
            CommandName="Referencias" OnClick="lkbEdicionAcc_Click">Referencias</asp:LinkButton>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lkbReferenciasAcc" />
    </Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<EditItemTemplate>
<asp:LinkButton ID="lkbCancelarAccE" runat="server" CommandName="Cancelar"
    OnClick="lkbGuardarAccE_Click">Cancelar</asp:LinkButton>
</EditItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewAccesorioHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIAccesorio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblError3" runat="server" UpdateMode="Conditional" RenderMode="Block">
<ContentTemplate>
<asp:Label ID="lblError3" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHI" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCopia" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvAccesorioHoja" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarHIAccesorio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    
<div id="contenidoConfirmacionEliminarHI" class="modal">
<div id="confirmacionEliminarHI" class="contenedor_ventana_confirmacion">            
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Eliminar Hoja Instruccion</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea Eliminar la HI y todos sus componentes?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminarHI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminarHI" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminarHI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminarHI" runat="server" OnClick="btnAceptarEliminarHI_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>               
</div>
</div>
<div id="contenidoConfirmacionEliminarHIDocumento" class="modal">
<div id="confirmacionEliminarHIDocumento" class="contenedor_ventana_confirmacion">            
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Eliminar Evidencia</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">
¿Realmente desea eliminar el documento de la HI?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnCancelarEliminarHIDocumento" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminarHIDocumento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminarHIDocumento" runat="server" OnClick="btnAceptarEliminarDocumentoHI_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<div id="contenidoConfirmacionEliminarHIAccesorio" class="modal">
<div id="confirmacionEliminarHIAccesorio" class="contenedor_ventana_confirmacion">            
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Eliminar Accesorio</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea eliminar el accesorio de esta HI?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnCancelarEliminarHIAccesorio" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminarHIAccesorio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminarHIAccesorio" runat="server" OnClick="btnAceptarEliminarHIAccesorio_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
</asp:Content>
