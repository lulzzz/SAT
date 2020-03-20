<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FacturaGlobalV33.aspx.cs" Inherits="SAT.FacturacionElectronica33.FacturaGlobalV33" %>
<%@ Register  Src="~/UserControls/wucAddendaComprobante.ascx" TagName="wucAddendaComprobante" TagPrefix="tectos" %>
<%@ Register  Src="~/UserControls/wucFacturadoConcepto.ascx" TagName="wucFacturadoConcepto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryFacturaGlobal();
}
}
//Declarando Función de Configuración de Controles de la Página
function ConfiguraJQueryFacturaGlobal() {
$(document).ready(function () {
//Añadiendo Función de Autocompletado al Control
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
//Declarando Función de Validación de Controles
var validaFacturaGlobal = function () {
var isValid1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1 && isValid2;
}
//Función de validación de campos
var validacionCancelacion = function (evt) {
var isValidP1 = !$("#<%=txtMotivo.ClientID%>").validationEngine('validate');
return isValidP1;
};
//Declarando Función de Validación de Controles
var validaRegistrafactura = function () {
var isValid1 = !$("#<%=txtTotalFactura.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1
}
//Añadiendo Función al Evento Click del Control
$("#<%=btnAceptarCancelacionCFDI.ClientID%>").click(validacionCancelacion);
//Añadiendo Función al Evento Click del Control
$("#<%=btnGuardar.ClientID%>").click(validaFacturaGlobal);
//Añadiendo Función al Evento Click del Control
$("#<%=btnRegistrarFacturaElectronica.ClientID%>").click(validaRegistrafactura);
});
}
//Invocando Función de COnfiguración
ConfiguraJQueryFacturaGlobal();
</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Factura Global</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green"><a href="#" class="fa fa-floppy-o"></a>
<ul>
<li><asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li><asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li><asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
<li><asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
</ul>
</li>
<li class="red"><a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li><asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li><asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
<li><asp:LinkButton ID="lkbVerComprobante" runat="server" Text="Ver CFDI" OnClick="lkbVerComprobante_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbComentario" runat="server" Text="Comentario" OnClick="lkbComentario_Click"></asp:LinkButton></li>
</ul>
</li>
<li class="blue"><a href="#" class="fa fa-cog"></a>
<ul>
<li><asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li><asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
<li><asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
<li><asp:LinkButton ID="lkbAddendaFacturaElectronica" runat="server" Text="Addenda" OnClick="lkbAddendaFacturaElectronica_Click"></asp:LinkButton></li>
</ul>
</li>
<li class="gray"><a href="#" class="fa fa-file-archive-o"></a>
<ul>
<li><asp:LinkButton ID="lkbRegistrarFacturaElectronica" runat="server" Text="Registrar" OnClick="lnkRegistrarFacturacionElectronica_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbTimbrarFacturaElectronica" runat="server" Text="Timbrar" OnClick="lnkTimbrarFacturacionElectronica_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbEliminarCFDI" runat="server" Text="EliminarCFDI" OnClick="lkbEliminarCFDI_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbCancelarCFDI" runat="server" Text="CancelarCFDI" OnClick="lkbCancelarCFDI_Click"></asp:LinkButton></li>
</ul>
</li>
<li class="yellow"><a href="#" class="fa fa-download"></a>
<ul>
<li><asp:LinkButton ID="lkbPDF" runat="server" Text="PDF" OnClick="lkbPDF_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbXML" runat="server" Text="XML" OnClick="lkbXML_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbEmail" runat="server" Text="E-mail" OnClick="lkbEmail_Click"></asp:LinkButton></li>
<li><asp:LinkButton ID="lkbFEReferencias" runat="server" Text="Referencias" OnClick="lkbFEReferencias_Click"></asp:LinkButton></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbAbrir" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
<asp:PostBackTrigger ControlID="lkbXML" />
</Triggers>
</asp:UpdatePanel>
<div class="contenedor_controles">
<div class="renglon100Per">
<div class="controlr" style="width: 150px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 150px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 140px"></div>
<div class="controlr" style="width: 140px"></div>
</div>
<asp:Panel ID="pnlFacturaGlobal" runat="server" DefaultButton="btnGuardar">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoFactura">No. Factura</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblNoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblNoFactura" runat="server"></asp:Label></b>
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacionCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="1"></asp:TextBox>
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
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
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
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" TabIndex="3" CssClass="boton"
OnClick="btnGuardar_Click" />
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
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="4" CssClass="boton_cancelar" OnClick="btnCancelar_Click" />
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
<b><asp:Label ID="lblSubtotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaEC" />
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
<b><asp:Label ID="lblTrasladado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaEC" />
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
<b><asp:Label ID="lblRetenido" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaEC" />
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
<b><asp:Label ID="lblTotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaEC" />
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
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
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
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
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
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacionCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarCFDI" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
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
<div class="renglon3x">
<div class="etiqueta_50px">
<label>Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:LinkButton ID="lkbImportarArchivo" runat="server" Text="Importar Archivo .xls(x)" OnClick="lkbImportarArchivo_Click"></asp:LinkButton>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFacturas" runat="server" Text="Buscar" OnClick="btnBuscarFacturas_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoFacDisp">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFacDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacDisp" runat="server" CssClass="dropdown_100px" AutoPostBack="true" TabIndex="5" OnSelectedIndexChanged="ddlTamanoFacDisp_SelectedIndexChanged">
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
<asp:BoundField DataField="NoServicio" HeaderText="Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="No. Viaje" SortExpression="NoViaje">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia1" runat="server" Text='<%# Eval("NoViaje") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Pedido" SortExpression="Pedido">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia2" runat="server" Text='<%# Eval("Pedido") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
<ItemTemplate>
<asp:LinkButton ID="lnkAgregarReferencia3" runat="server" Text='<%# Eval("Referencia") %>' OnClick="lnkAgregarReferencia_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
<asp:BoundField DataField="FecFac" HeaderText="Fecha Factura" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
<asp:TemplateField HeaderText="TotalFlete" SortExpression="TotalFlete">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkEditarConceptos" runat="server" Text='<%# string.Format("{0:C2}",Eval("TotalFlete")) %>' OnClick="lnkEditarConceptos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="TotalOtros" HeaderText="Total Otros" SortExpression="TotalOtros" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Retencion" HeaderText="Retencion" SortExpression="Retencion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacDisp" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwFacturasLigadas" runat="server">
<div class="renglon2x">
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
PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
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
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Pedido" HeaderText="Pedido" SortExpression="Pedido" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" />
<asp:BoundField DataField="FecFac" HeaderText="Fecha Factura" SortExpression="FecFac" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalFlete" HeaderText="Total Flete" SortExpression="TotalFlete" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="TotalOtros" HeaderText="Total Otros" SortExpression="TotalOtros" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IVA" HeaderText="IVA" SortExpression="IVA" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Retencion" HeaderText="Retencion" SortExpression="Retencion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />

<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkVerConceptos" runat="server" Text="Ver Conceptos" OnClick="lnkVerConceptos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarConceptosLigados" runat="server" Text="Editar Conceptos" OnClick="lnkEditarConceptosLigados_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eliminar" OnClick="lnkEliminarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarFacturas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaEC" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="upbtnGuardarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarFactura" runat="server" Text="Guardar Factura(s)" CssClass="boton" OnClick="btnGuardarFactura_Click" Visible="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
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
<asp:DropDownList ID="ddlTamanoFacConceptos" runat="server" CssClass="dropdown" AutoPostBack="true" TabIndex="9" OnSelectedIndexChanged="ddlTamanoFacConceptos_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFacConceptos">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFacConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFacConceptos" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturaConceptos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacConceptos" runat="server" Text="Exportar" TabIndex="10" OnClick="lnkExportarFacConceptos_Click"></asp:LinkButton>
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
<asp:CheckBox ID="chkTodosConceptos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosConceptos_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVariosConceptos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosConceptos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="IdFacturaConcepto" HeaderText="IdFacturaConcepto" SortExpression="IdFacturaConcepto" Visible="false" />
<asp:BoundField DataField="IdFacturaGlobal" HeaderText="Factura Global" SortExpression="IdFacturaGlobal" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" />
<asp:BoundField DataField="ImporteTrasladado" HeaderText="Imp. Trasladado" SortExpression="ImporteTrasladado" />
<asp:BoundField DataField="ImporteRetenido" HeaderText="Imp. Retenido" SortExpression="ImporteRetenido" />
<asp:BoundField DataField="Total" HeaderText="Importe Total" SortExpression="Total" />
<asp:BoundField DataField="Indicador" HeaderText="Indicador" SortExpression="Indicador" Visible="false" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
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
<asp:Button ID="btnGuardarConceptos" runat="server" Text="Guardar Conceptos" TabIndex="11" OnClick="btnGuardarConceptos_Click" CssClass="boton" />
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
<div id="contenidoConfirmacionRegistarFacturacionElectronica" class="modal" >
<div id="confirmacionRegistarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarRegistarFacturacionElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarRegistarFacturacionElectronica" runat="server" Text="Cerrar"  OnClick="lkbCerrarRegistarFacturacionElectronica_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Registrar Factura</h3>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoFacturacionElectronica">Tipo Facturación E.</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoFacturacionElectronica" runat="server" OnSelectedIndexChanged="ddlTipoFacturacionElectronica_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoIdentificacion">No. Identificación.</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoIdentificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoIdentificacion" Enabled="false" runat="server" Text=" " CssClass="textbox2x" MaxLength="2000" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlFormaPago">Forma de Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlFormaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
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
<asp:DropDownList ID="ddlMetodoPago" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlSucursal">Sucursal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlUsoCFDI">Uso CFDI</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUsoCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUsoCFDI" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTotalFactura">Total Factura.</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTotalFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotalFactura" Enabled="false" runat="server" Text=" " CssClass="textbox_100px validate[required, custom[positiveNumber]]" MaxLength="20" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnNo" />
<asp:AsyncPostBackTrigger ControlID="btnSi" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarPQ" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFacturacionElectronica" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRegistrarFacturaElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRegistrarFacturaElectronica" runat="server" CssClass="boton" OnClick="btnRegistrarFacturaElectronica_Click" Text="Registrar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionTimbrarFacturacionElectronica" class="modal">
<div id="confirmaciontimbrarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarFacturacionElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTimbrarFacturacionElectronica" runat="server" Text="Cerrar" OnClick="lkbCerrarTimbrarFacturacionElectronica_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Timbrar Factura</h3>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" Text="" runat="server" CssClass="textbox validate[custom[onlyLetterSp]]" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="upchkOmitirAddenda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkOmitirAddenda" Checked="false" runat="server" Text="Facturar sin 'Addenda'." />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="lbllblTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTimbrarFacturacionElectronica" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbrarFacturacionElectronica" runat="server"  OnClick="btnAceptarTimbrarFacturacionElectronica_Click"  CssClass ="boton"  Text="Timbrar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionAddenda" class="modal">
<div id="confirmacionAddenda"" class="contenedor_ventana_confirmacion"> 
<div style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAddenda" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAddenda" runat="server" Text="Cerrar" OnClick="lkbCerrarAddendaComprobante_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Addenda</h3>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlAddenda">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlAddenda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlAddenda" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAddenda" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddenda" />
<asp:AsyncPostBackTrigger ControlID="lkbAddendaFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorAddenda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorAddenda" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAddenda" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddenda" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAddenda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAddenda" runat="server" OnClick="btnAceptarAddenda_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionWucComprobanteAddenda" class="modal">
<div id="confirmacionWucComprobanteAddenda" class="contenedor_modal_seccion_completa_arriba" style="top: 15px; width: 950px">
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarwucAddendaComprobante" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarwucAddendaComprobante" runat="server" Text="Cerrar" OnClick="lkbCerrarwucAddendaComprobante_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="upwucAddendaComprobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucAddendaComprobante ID="wucAddendaComprobante" OnClickEliminar="wucAddendaComprobante_ClickEliminar" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddenda" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div id="contenidoConfirmacionEmail" class="modal">
<div id="confirmacionEmail" class="contenedor_ventana_confirmacion" style="height: auto">
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEmail" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEmail" TabIndex="22" runat="server" Text="Cerrar" OnClick="lkbCerrarEmail_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<h3>Addenda</h3>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtAsunto">Asunto:</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtAsunto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAsunto" runat="server" TabIndex="23" CssClass="textbox2x"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEmail" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtMensaje">Mensaje:</label>
</div>
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uptxtMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMensaje" TabIndex="24" runat="server" CssClass="textbox2x"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEmail" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control">
<asp:UpdatePanel ID="uplblErrorEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorEmail" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEmail" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEmail" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEmail" Text="Enviar" runat="server" TabIndex="25" OnClick="btnAceptarEmail_Click" CssClass="boton"></asp:Button>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionEliminarCFDI" class="modal">
<div id="confirmacionEliminarCFDI" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h3>Eliminar Factura Electrónica</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea eliminar la Factura Electrónica ?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminarCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminarCFDI" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarEliminarCFDI_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminarCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminarCFDI" runat="server" OnClick="btnAceptarEliminarCFDI_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionCancelacionCFDI" class="modal">
<div id="confirmacionCancelacionCFDI" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h3>Cancelar Factura Electrónica</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea Cancelar la Factura Electrónica ?</label>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivo">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine" Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
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
<!-- Ventana de Edición de Conceptos (Detalles de Factura) -->
<div id="contenedorVentanaEdicionConceptos" class="modal">
<div id="ventanaEdicionConceptos" class="contenedor_modal_asignacion_recursos">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarVentanaEC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaEC" runat="server" Text="Cerrar" OnClick="lnkCerrarVentanaEC_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Detalles de la Factura</h2>
</div>
<asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Always">
<ContentTemplate>
<tectos:wucFacturadoConcepto ID="ucFacturadoConcepto" runat="server" OnClickGuardarFacturaConcepto="ucFacturadoConcepto_ClickGuardarFacturaConcepto"
OnClickEliminarFacturaConcepto="ucFacturadoConcepto_ClickEliminarFacturaConcepto" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
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
<tectos:wucReferenciaViaje ID="ucReferenciasViaje" runat="server" OnClickGuardarReferenciaViaje="ucReferenciasViaje_ClickGuardarReferenciaViaje" OnClickEliminarReferenciaViaje="ucReferenciasViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- Ventana Referencias -->
<div id="contenedorVentanaReferencias" class="modal">
<div id="ventanaReferencias" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarDev" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="LinkButton1" runat="server" CommandName="referenciasRegistro" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="12">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EnvioRecepcion.png" />
<h2>Referencias Viaje</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoReferencias">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoReferencias" runat="server" TabIndex="13" Enabled="false" OnSelectedIndexChanged="ddlTamanoReferencias_SelectedIndexChanged" CssClass="dropdown" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoReferencias">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoReferencias" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvReferencias" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvReferencias" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" PageSize="300"
CssClass="gridview" ShowFooter="true" TabIndex="25" OnSorting="gvReferencias_Sorting"
OnPageIndexChanging="gvReferencias_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField SortExpression="Tipo">
<FooterTemplate>
<asp:Label ID="lblContadorTipo" runat="server" Text="0"></asp:Label>
<br />Seleccionados
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkTipoTodos" runat="server" AutoPostBack="True" Checked="true" CssClass="LabelResalta" OnCheckedChanged="chkTipoTodos_CheckedChanged" Text="Todos" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSeleccionTipo" runat="server" Checked="true" AutoPostBack="True" OnCheckedChanged="chkSeleccionTipo_CheckedChanged" />
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoReferencias" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRegistrarFE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRegistrarFE" runat="server" OnClick="btnRegistrarFE_Click" CssClass="boton" Text="Registrar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana Comentario -->
<div id="contenidoComentario" class="modalControlUsuario">
<div id="confirmacionComentario" class="contenedor_ventana_confirmacion">
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrar" runat="server" CommandName="comentario" Text="Cerrar" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtComentario">Comentario</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtComentario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtComentario" runat="server" Text=" " CssClass="textbox2x" MaxLength="500" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarComentario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarComentario" runat="server" OnClick="btnAceptarComentario_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="confirmacionAgregarFacturaGlobal" class="modal">
<div id="agregarFacturaGlobal" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Desea agregar el Servicio a la Factura Global? </h2>
</div>
<div class="renglon2x">
<div class="etiqueta"></div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnSi" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnSi" Text="Si" CssClass="boton" OnClick="btnSi_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnNo" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnNo" Text="No" CssClass="boton" OnClick="btnNo_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</asp:Content>
