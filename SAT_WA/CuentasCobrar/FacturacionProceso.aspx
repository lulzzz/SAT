<%@ Page Title="Proceso Facturación" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FacturacionProceso.aspx.cs" Inherits="SAT.CuentasCobrar.FacturacionProceso" %>

<%@ Register Src="~/UserControls/wucEncabezadoServicio.ascx" TagPrefix="tectos" TagName="wucEncabezadoServicio" %>
<%@ Register Src="~/UserControls/wucAddendaComprobante.ascx" TagName="wucAddendaComprobante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagName="wucFacturadoConcepto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
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
ConfiguraJQueryFacturaProceso();
}
}

//Declarando Función de Configuración de Controles de la Página
function ConfiguraJQueryFacturaProceso() {
$(document).ready(function () {

//Añadiendo Función de Autocompletado al Control (Cliente)
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
//Añadiendo Función de Autocompletado al Control (Responsable)
$("#<%=txtUsuarioResp.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=33&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

//Declarando Función de Validación de Controles
var validaFacturaProceso = function () {
var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtUsuarioResp.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
}

//Cargando Control DateTimePicker "Fecha Inicio"
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Cargando Control DateTimePicker "Fecha Fin"
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Añadiendo Función al Evento Click del Control
$("#<%=btnGuardar.ClientID%>").click(validaFacturaProceso);
});
}

//Invocando Función de Configuración
ConfiguraJQueryFacturaProceso();
</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Proceso de Revisión y Cobro</h1>
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
<li>
<asp:LinkButton ID="lkbReversa" runat="server" Text="Reversa" OnClick="lkbElementoMenu_Click" CommandName="Reversa" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEntregar" runat="server" Text="Entregar" OnClick="lkbElementoMenu_Click" CommandName="Entregar" /></li>
<li>
<asp:LinkButton ID="lkbAceptar" runat="server" Text="Aceptar" OnClick="lkbElementoMenu_Click" CommandName="Aceptar" /></li>
<li>
<asp:LinkButton ID="lkbRechazar" runat="server" Text="Rechazar" OnClick="lkbElementoMenu_Click" CommandName="Rechazar" /></li>
<li>
<asp:LinkButton ID="lkbTerminar" runat="server" Text="Terminar" OnClick="lkbElementoMenu_Click" CommandName="Terminar" /></li>
</ul>
</li>
<li class="gray">
<a href="#" class="fa fa-book "></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcuseABC" runat="server" Text="Acuse ABC" OnClick="lkbElementoMenu_Click" CommandName="AcuseABC" /></li>
<li>
<asp:LinkButton ID="lkbAcuseColgate" runat="server" Text="Acuse Colgate" OnClick="lkbElementoMenu_Click" CommandName="AcuseColgate" /></li>
<li>
<asp:LinkButton ID="lkbAcuseSchindler" runat="server" Text="Acuse Schindler" OnClick="lkbElementoMenu_Click" CommandName="AcuseSchindler" /></li>

</ul>
</li>
<li class="purple">
<a href="#" class="fa fa-dashboard"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcuceLily" runat="server" Text="Acuse Lili" OnClick="lkbElementoMenu_Click" CommandName="AcuseLili" /></li>
<li>
<asp:LinkButton ID="lkbAcuseProcter" runat="server" Text="Acuse Procter" OnClick="lkbElementoMenu_Click" CommandName="AcuseProcter" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbAbrir" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
<asp:PostBackTrigger ControlID="lkbAcuseProcter" />
<asp:PostBackTrigger ControlID="lkbAcuceLily" />
<asp:PostBackTrigger ControlID="lkbAcuseSchindler" />
<asp:PostBackTrigger ControlID="lkbAcuseColgate" />
</Triggers>
</asp:UpdatePanel>

<div class="contenedor_controles">
<asp:Panel ID="pnlProcesoRevCob" runat="server" DefaultButton="btnGuardar">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoFactura">No. Paquete</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblNoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblNoPaquete" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Proceso</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoProceso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoProceso" runat="server" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompania">Compania</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicio">Fecha de Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaFin">Fecha de Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16" TabIndex="4" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUsuarioResp">Responsable</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUsuarioResp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUsuarioResp" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
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
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="5" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" TabIndex="5" CssClass="boton"
OnClick="btnGuardar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="6" CssClass="boton_cancelar"
OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblSubtotal">Sub Total</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblSubtotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblSubtotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTrasladado">Total Trasladado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTrasladado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblTrasladado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblRetenido">Total Retenido</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblRetenido" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblRetenido" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTotal">Total</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblTotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Factura Global</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFacturaGlobal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFacturaGlobal" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFG" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFG" runat="server" Text="Agregar" CssClass="boton_cancelar" OnClick="btnAgregarFG_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Paquete Previo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtPaqPrevio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPaqPrevio" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarPQ" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarPQ" runat="server" Text="Agregar" CssClass="boton_cancelar" OnClick="btnAgregarPQ_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaFacturasDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaFacturasDisponibles" runat="server" Text="Facturas Disponibles" CssClass="boton_pestana_activo" CommandName="FacturasDisponibles" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaFacturasLigadas" runat="server" Text="Facturas Ligadas" CssClass="boton_pestana" CommandName="FacturasLigadas" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvFacturas" runat="server" ActiveViewIndex="0">
<asp:View ID="vwFacturasDisponibles" runat="server">
<asp:Panel ID="pnlFacturasDisponibles" runat="server" DefaultButton="btnBuscarFacturas">
<div class="renglon3x" style="width: auto">
<div class="etiqueta_50px">
<label for="txtReferenciaServicio">Referencia</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferenciaServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferenciaServicio" runat="server" CssClass="textbox"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtCartaPorteBusqueda">Carta Porte</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCartaPorteBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCartaPorteBusqueda" runat="server" CssClass="textbox"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upbtnBuscarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFacturas" runat="server" Text="Buscar" OnClick="btnBuscarFacturas_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:LinkButton ID="lkbImportarArchivo" runat="server" Text="Importar Archivo .xls(x)" OnClick="lkbImportarArchivo_Click"></asp:LinkButton>
</div>
</asp:Panel>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoFacDisp">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFacDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacDisp" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="5" OnSelectedIndexChanged="ddlTamanoFacDisp_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoFacDisp">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoFacDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoFacDisp" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarFacDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacDisp" runat="server" Text="Exportar" TabIndex="6"
OnClick="lnkExportarFacDisp_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacDisp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvFacturasDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasDisponibles" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvFacturasDisponibles_PageIndexChanging" OnSorting="gvFacturasDisponibles_Sorting"
PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false"
OnRowDataBound="gvFacturasDisponibles_RowDataBound">
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
<asp:BoundField DataField="IdFactura" HeaderText="Factura" SortExpression="IdFactura" Visible="false" />
<asp:BoundField DataField="IdServicio" HeaderText="Servicio" SortExpression="IdServicio" Visible="false" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:TemplateField HeaderText="Porte" SortExpression="Porte">
<ItemTemplate>
<asp:LinkButton ID="lkbCartaPorte" runat="server" CommandName="CartaPorte" OnClick="lkbPorte_Click" Text='<%#Eval("Porte") %>' ToolTip="Ver y Editar Encabezado del Servicio"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Referencia 1" SortExpression="Referencia1">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia1" runat="server" Text='<%# Eval("Referencia1") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia 2" SortExpression="Referencia2">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia2" runat="server" Text='<%# Eval("Referencia2") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia 3" SortExpression="Referencia3">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia3" runat="server" Text='<%# Eval("Referencia3") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
<asp:BoundField DataField="FecFac" HeaderText="Fecha Servicio" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Flete" HeaderText="Flete" SortExpression="Flete" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Retencion" HeaderText="Retencion" SortExpression="Retencion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:LinkButton ID="lnkEditarConceptos" runat="server" Text='<%# Eval("Total", "{0:C2}") %>' OnClick="lnkEditarConceptos_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionarFactura" runat="server" Text="Ver Conceptos" OnClick="lnkSeleccionarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Indicador" HeaderText="Indicador" SortExpression="Indicador" Visible="false" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarFactura" runat="server" Text="Agregar" OnClick="lnkAgregarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
<asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwFacturasLigadas" runat="server">
<div class="renglon3x"></div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoFacLigadas">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFacLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacLigadas" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="7" OnSelectedIndexChanged="ddlTamanoFacLigadas_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoFacLigadas">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFacLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoFacLigadas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarFacLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacLigadas" runat="server" Text="Exportar" TabIndex="8"
OnClick="lnkExportarFacLigadas_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging" OnSorting="gvFacturasLigadas_Sorting"
OnRowDataBound="gvFacturasLigadas_RowDataBound" PageSize="25" CssClass="gridview" ShowFooter="true"
Width="100%" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdFactura" HeaderText="Factura" SortExpression="IdFactura" Visible="false" />
<asp:BoundField DataField="IdServicio" HeaderText="Servicio" SortExpression="IdServicio" Visible="false" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:TemplateField HeaderText="Porte" SortExpression="Porte">
<ItemTemplate>
<asp:LinkButton ID="lkbPorte" runat="server" CommandName="Porte" OnClick="lkbPorte_Click" Text='<%#Eval("Porte") %>' ToolTip="Ver y Editar Encabezado del Servicio"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Referencia 1" SortExpression="Referencia1">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia1" runat="server" Text='<%# Eval("Referencia1") %>' OnClick="lnkAgregarReferenciaFacturasLigadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia 2" SortExpression="Referencia2">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia2" runat="server" Text='<%# Eval("Referencia2") %>' OnClick="lnkAgregarReferenciaFacturasLigadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia 3" SortExpression="Referencia3">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia3" runat="server" Text='<%# Eval("Referencia3") %>' OnClick="lnkAgregarReferenciaFacturasLigadas_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
<asp:BoundField DataField="FecFac" HeaderText="Fecha Servicio" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Flete" HeaderText="Flete" SortExpression="Flete" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Retencion" HeaderText="Retencion" SortExpression="Retencion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />

<asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:LinkButton ID="lnkEditarConceptos" runat="server" Text='<%# Eval("Total", "{0:C2}") %>' OnClick="lnkEditarConceptosFacturasLigadas_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkVerConceptos" runat="server" Text="Ver Conceptos" OnClick="lnkVerConceptos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eliminar" OnClick="lnkEliminarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="No Entregado" SortExpression="NoEntregado">
<HeaderStyle HorizontalAlign="Center" Width="50px" />
<ItemStyle HorizontalAlign="Center" Width="50px" />
<ItemTemplate>
<asp:CheckBox ID="chkNoEntregado" runat="server" AutoPostBack="true" OnCheckedChanged="chkActualizaDetalle_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Rechazado" SortExpression="Rechazado">
<HeaderStyle HorizontalAlign="Center" Width="50px" />
<ItemStyle HorizontalAlign="Center" Width="50px" />
<ItemTemplate>
<asp:CheckBox ID="chkRechazado" runat="server" AutoPostBack="true" OnCheckedChanged="chkActualizaDetalle_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacLigadas" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
<asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarEdicionConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbEntregar" />
<asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFG" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="upbtnGuardarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarFactura" runat="server" Text="Guardar Factura(s)" CssClass="boton"
OnClick="btnGuardarFactura_Click" Visible="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- Ventana de Conceptos -->
<div id="contenedorVentanaConceptos" class="modal">
<div id="ventanaConceptos" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Conceptos Disponibles</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFacConceptos">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacConceptos" runat="server" CssClass="dropdown" AutoPostBack="true"
TabIndex="9" OnSelectedIndexChanged="ddlTamanoFacConceptos_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFacConceptos">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFacConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoFacConceptos" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturaConceptos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacConceptos" runat="server" Text="Exportar" TabIndex="10"
OnClick="lnkExportarFacConceptos_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvFacturaConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturaConceptos" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvFacturaConceptos_PageIndexChanging" OnSorting="gvFacturaConceptos_Sorting"
PageSize="5" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false"
OnRowDataBound="gvFacturaConceptos_RowDataBound">
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
<asp:CheckBox ID="chkTodosConceptos" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodosConceptos_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVariosConceptos" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodosConceptos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="IdFacturaConcepto" HeaderText="IdFacturaConcepto" SortExpression="IdFacturaConcepto" Visible="false" />
<asp:BoundField DataField="IdFacturaGlobal" HeaderText="Factura Global" SortExpression="IdFacturaGlobal" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="ImporteTrasladado" HeaderText="Imp. Trasladado" SortExpression="ImporteTrasladado" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="ImporteRetenido" HeaderText="Imp. Retenido" SortExpression="ImporteRetenido" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Total" HeaderText="Importe Total" SortExpression="Total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Indicador" HeaderText="Indicador" SortExpression="Indicador" Visible="false" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnReversa" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacConceptos" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarConceptos" runat="server" Text="Guardar Conceptos" TabIndex="11"
OnClick="btnGuardarConceptos_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturaConceptos" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana de Edición de Conceptos (Detalles de Factura) -->
<div id="contenedorVentanaEdicionDetalles" class="modal">
<div id="ventanaEdicionDetalles" class="contenedor_modal_seccion_completa_arriba" style="width: 1200px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarEdicionConceptos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarEdicionConceptos" runat="server" OnClick="lnkCerrarEdicionConceptos_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Detalles de la Factura</h2>
</div>
<asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucFacturadoConcepto ID="ucFacturadoConcepto" runat="server" OnClickGuardarFacturaConcepto="ucFacturadoConcepto_ClickGuardarFacturaConcepto"
OnClickEliminarFacturaConcepto="ucFacturadoConcepto_ClickEliminarFacturaConcepto" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- Ventana de Referencias de Viaje -->
<div id="contenedorVentanaReferenciasViaje" class="modal">
<div id="ventanaReferenciasViaje" class="contenedor_ventana_confirmacion" style="width: 300px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" OnClick="lnkCerrarReferencias_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Referencias del Viaje</h2>
</div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciasViaje" runat="server" OnClickGuardarReferenciaViaje="ucReferenciasViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="ucReferenciasViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE ENCABEZADO DE SERVICIO -->
<div id="encabezadoServicioModal" class="modal">
<div id="encabezadoServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEncabezadoServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEncabezadoServicio" runat="server" OnClick="lkbCerrarEncabezadoServicio_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucEncabezadoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucEncabezadoServicio ID="wucEncabezadoServicio" runat="server"
OnClickGuardarReferencia="wucEncabezadoServicio_ClickGuardarReferencia" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE SERVICIO NO FACTURABLE -->
<div id="confirmacionVentanaReversa" class="modal">
<div id="ventanaReversa" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReversa" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReversa" runat="server" OnClick="lnkCerrarReversa_Click" CommandName="Reversa">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Desea reversar el estatus (a Registrado) del Paquete?</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivoReversa">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoReversa" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivoReversa" runat="server" TextMode="MultiLine" Text=" " CssClass="textbox2x validate[required]" 
    MaxLength="500" placeholder="Ingrese su motivo de reversa"></asp:TextBox></div></div>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbReversa" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnReversa" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnReversa" Text="Si" CssClass="boton" OnClick="btnReversa_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>
</asp:Content>
