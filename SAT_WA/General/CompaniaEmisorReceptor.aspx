<%@ Page Title="Catálogo de Clientes y Proveedores" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CompaniaEmisorReceptor.aspx.cs" Inherits="SAT.General.CompaniaEmisorReceptor" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/wucCuentaBanco.ascx" TagPrefix="tectos" TagName="wucCuentaBanco" %>
<%@ Register Src="~/UserControls/wucDireccion.ascx" TagName="WucDireccion" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucProveedorGPSDiccionario.ascx" TagPrefix="ucl" TagName="wucProveedorGPSDiccionario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlEvidencias.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryCompania();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryCompania() {
$(document).ready(function () {
//Función de validación de campos
var validacionCompania = function (evt) {
var isValidP1 = !$("#<%=txtRFC.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtRFC.ClientID%>").validationEngine('validate');
var isValidP4 = !$("#<%=txtDireccion.ClientID%>").validationEngine('validate');
var isValidP5 = !$("#<%=txtCorreo.ClientID%>").validationEngine('validate');
var isValidP6 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
var isValidP7 = !$("#<%=txtLimiteCredito.ClientID%>").validationEngine('validate');
var isValidP8 = !$("#<%=txtDiasCredito.ClientID%>").validationEngine('validate');

return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8;
};
//Boton Guardar
$("#<%=btnGuardar.ClientID%>").click(validacionCompania);
//Boton Guardar
$("#<%=lkbGuardar.ClientID%>").click(validacionCompania);
//Cargando Catalogo AutoCompleta
$("#<%=txtCompaniaAgrup.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

//Añadiendo Encabezado Fijo
$("#<%=gvFacturasCliente.ClientID%>").gridviewScroll({
width: document.getElementById("contenedorFacturasCliente").offsetWidth - 15,
height: 400,
});
});            
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryCompania();
</script>
<div id="encabezado_forma">
<img src="../Image/Compania.png" />
<h1>Compañia</h1>
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
<asp:LinkButton ID="lkbCuentasBanco" runat="server" Text="Cuentas Banco" OnClick="lkbElementoMenu_Click" CommandName="CuentasBanco" /></li>
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
<asp:LinkButton ID="lkbHI" runat="server" Text="Hoja de Instrucción" OnClick="lkbElementoMenu_Click" CommandName="HI" /></li>
</ul>
</li>
<li class="gray">
<a href="#" class="fa fa-flag-checkered"></a>
<ul>
<li>
<asp:LinkButton ID="lkbProveedorWS" runat="server" Text="Proveedor WS" OnClick="lkbElementoMenu_Click" CommandName="ProveedorWS" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/DatosPrincipales.png" />
<h2>Datos de la compañia</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombre">Razon Social</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNombre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombre" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRFC">RFC</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtRFC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRFC" runat="server" TabIndex="2" CssClass="textbox2x validate[required, custom[RFC]]" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDireccion">Dirección</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDireccion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDireccion" runat="server" CssClass="textbox2x validate[required]" Enabled="false" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ucDireccion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplnkVentana" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkVentana" runat="server" Text="Dirección" OnClick="lnkVentana_Click" TabIndex="3"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNombreCorto">Nombre Corto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNombreCorto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreCorto" runat="server" TabIndex="4" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>ID Alterno</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtIdAlterno" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtIdAlterno" runat="server" TabIndex="5" CssClass="textbox2x" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoServicio">Tipo de Servicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown2x" TabIndex="6"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarServicio" />
<asp:AsyncPostBackTrigger ControlID="chkBitProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplnkAgregarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkAgregarServicio" runat="server" Text="Agregar" OnClick="lnkAgregarServicio_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarServicio" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosAgregados" />
<asp:AsyncPostBackTrigger ControlID="chkBitProveedor" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtContacto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtContacto" runat="server" TabIndex="7" CssClass="textbox2x" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefono">Telefono</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTelefono" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTelefono" runat="server" TabIndex="8" CssClass="textbox2x " MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCorreo">Correo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCorreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCorreo" runat="server" TabIndex="9" CssClass="textbox2x validate[custom[email]]" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtLimiteCredito">Limite de Credito</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtLimiteCredito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLimiteCredito" runat="server" TabIndex="10" CssClass="textbox2x validate[custom[positiveNumber]]" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDiasCredito">Dias de Credito</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDiasCredito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDiasCredito" runat="server" TabIndex="11" CssClass="textbox2x validate[custom[positiveNumber]]" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtInfoAd1">Info Extra</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtInfoAd1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtInfoAd1" runat="server" TabIndex="12" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtInfoAd2">Mas Información</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtInfoAd2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtInfoAd2" runat="server" TabIndex="13" CssClass="textbox2x" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompaniaAgrup">Corporativo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompaniaAgrup" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompaniaAgrup" runat="server" TabIndex="14" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplkbSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbSucursal" runat="server" Text="Sucursal" OnClick="lkbSucursal_Click" TabIndex="3"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Tipo de Compañia</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkBitReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkBitReceptor" runat="server" TabIndex="15" Text="Cliente" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkBitProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkBitProveedor" runat="server" TabIndex="16" Text="Proveedor" AutoPostBack="true"
OnCheckedChanged="chkBitProveedor_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkBitEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkBitEmisor" runat="server" TabIndex="17" Text="Emisor" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompaniaUso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompaniaUso" runat="server" TabIndex="18" CssClass="textbox2x" Enabled="false" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlRegimenFiscal">Regimen F.</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlRegimenFiscal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlRegimenFiscal" runat="server" CssClass="dropdown2x" TabIndex="19"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px">
<b>(*) Regímenes Disponibles de Personas Físicas</b>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Uso CFDI</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUsoCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUsoCliente" runat="server" CssClass="dropdown2x" TabIndex="20"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="validador">
<asp:UpdatePanel ID="uplnkOpcionesUso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkOpcionesUso" runat="server" Text="Opciones.." OnClick="lnkOpcionesUso_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="19" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="20" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x">
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
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!--GRIDVIEW FACTURAS CLIENTES-->
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Facturas en el Último Mes</h2>
</div>
<div class="renglon3x">
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFacturasCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoFacturasCliente"></label>
<asp:DropDownList ID="ddlTamanoFacturasCliente" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFacturasCliente_SelectedIndexChanged" TabIndex="32">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoFacturasCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoFacturasCliente" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasCliente" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFacturasCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacturasCliente" runat="server" OnClick="lnkExportarFacturasCliente_Click" Text="Exportar Excel" TabIndex="33"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacturasCliente" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorFacturasCliente">
<asp:UpdatePanel ID="upgvFacturasCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasCliente" runat="server" AllowPaging="true" AllowSorting="true"
OnPageIndexChanging="gvFacturasCliente_PageIndexChanging" OnSorting="gvFacturasCliente_Sorting"
CssClass="gridview" AutoGenerateColumns="false" Width="100%" PageSize="25" TabIndex="34">
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
<asp:BoundField DataField="Seriefolio" HeaderText="Folio" SortExpression="Seriefolio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Fechaexpedicion" HeaderText="Fecha Expedición" SortExpression="Fechaexpedicion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Fechacancelacion" HeaderText="Fecha Cancelación" SortExpression="Fechacancelacion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Flete" HeaderText="Flete" SortExpression="Flete" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Maniobras" HeaderText="Maniobras" SortExpression="Maniobras" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Estadias" HeaderText="Estadias" SortExpression="Estadias" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Otros" HeaderText="Otros" SortExpression="Otros" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Trasladado" HeaderText="Trasladado" SortExpression="Trasladado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Retenido" HeaderText="Retenido" SortExpression="Retenido" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Subtotal" HeaderText="SubTotal" SortExpression="Subtotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Impuesto" HeaderText="Impuesto" SortExpression="Impuesto" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbPDF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbPDF" runat="server" TabIndex="20" OnClick="lkbDetalles_Click"
CommandName="PDF">PDF</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbXML" runat="server" TabIndex="19" OnClick="lkbDetalles_Click"
CommandName="XML">XML</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbXML" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturasCliente" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--GRIDVIEW FACTURAS CLIENTES-->
<!--Ventana modal Direccion-->
<div id="contenedorDireccionModal" class="modal">
<div id="DireccionModal" class="contenedor_modal_seccion_completa_arriba" style="width: 1197px; top: 15px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagenDireccion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagenDireccion" runat="server" OnClick="lnkCerrarImagenDireccion_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers></Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucDireccion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:WucDireccion ID="ucDireccion" runat="server" TabIndex="20" Enable="false" OnClickGuardarDireccion="ucDireccion_ClickGuardarDireccion"
OnClickEliminarDireccion="ucDireccion_ClickEliminarDireccion" OnClickSeleccionarDireccion="ucDireccion_ClickSeleccionarDireccion" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkVentana" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<!--Fin ventanamodal-->
<div id="contenidoVentanaConfirmacion" class="modal">
<div id="ventanaConfirmacion" class="resumen_documentos_segmento">
<div class="header_seccion">
<h2>Asignación de Tipos de Servicio</h2>
</div>
<div class="contenedor_media_seccion">
<div class="renglon2x">
<div class="etiqueta">
<label for="">Tipo de Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoServicioAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoServicioAsignacion" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarServicio" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosAgregados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarServicio" runat="server" Text="Cancelar" OnClick="btnCancelarServicio_Click"
CssClass="boton_cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarServicio" runat="server" Text="Guardar" OnClick="btnGuardarServicio_Click"
CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorServicio" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarServicio" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosAgregados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Servicios Agregados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado Por:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvServiciosAgregados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosAgregados" runat="server" AutoGenerateColumns="false"
OnSorting="gvServiciosAgregados_Sorting" OnPageIndexChanging="gvServiciosAgregados_PageIndexChanging"
CssClass="gridview" AllowPaging="true" AllowSorting="true" PageSize="5" Width="100%"
ShowFooter="true">
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
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarServicio" />
<asp:AsyncPostBackTrigger ControlID="lnkAgregarServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!--Ventana para Alta de Cuentas -->
<div id="contenedorVentanaAltaCuentas" class="modal">
<div id="ventanaAltaCuentas"     class="contenedor_modal_seccion_completa_arriba" style="width:800px;height:390px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCuentas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCuentas" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="Cuentas" Text="Cerrar">
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
<asp:AsyncPostBackTrigger ControlID="lkbCuentasBanco" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Alta de Usos del CFDI dado un Cliente -->
<div id="ventanaContenedorUsoCFDI" class="modal">
<div id="contenedorUsoCFDI" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarUsoCFDI" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarUsoCFDI" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="UsosClienteCFDI" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Actualice los Usos de sus CFDI v3.3</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Uso CFDI</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUsoCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUsoCFDI" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlRegimenFiscal" />
<asp:AsyncPostBackTrigger ControlID="lnkOpcionesUso" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCFDI" />
<asp:AsyncPostBackTrigger ControlID="gvUsosCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_200px">
<b>(*) Uso único de Personas Físicas</b>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarUsoCDFI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarUsoCDFI" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardarUsoCDFI_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlRegimenFiscal" />
<asp:AsyncPostBackTrigger ControlID="lnkOpcionesUso" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCFDI" />
<asp:AsyncPostBackTrigger ControlID="gvUsosCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarCFDI" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarCFDI_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="ddlRegimenFiscal" />
<asp:AsyncPostBackTrigger ControlID="lnkOpcionesUso" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
<asp:AsyncPostBackTrigger ControlID="gvUsosCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="header_seccion">
<h2>Usos del Cliente</h2>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvUsosCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUsosCliente" runat="server" AllowPaging="false" AllowSorting="false" 
AutoGenerateColumns="false" Width="100%" ShowFooter="true">
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
<asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" ItemStyle-Width="50px" />
<asp:BoundField DataField="ClaveSAT" HeaderText="Clave SAT" SortExpression="ClaveSAT" ItemStyle-Width="50px" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
<asp:TemplateField>
<ItemStyle Width="50px" />
<ItemTemplate>
<asp:LinkButton ID="lnkEditarUsoCFDI" runat="server" Text="Editar" OnClick="lnkEditarUsoCFDI_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemStyle Width="70px" />
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarUsoCFDI" runat="server" Text="Eliminar" OnClick="lnkEliminarUsoCFDI_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
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
<asp:AsyncPostBackTrigger ControlID="ddlRegimenFiscal" />
<asp:AsyncPostBackTrigger ControlID="lnkOpcionesUso" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarUsoCDFI" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>

</div>
</div>
</div>
<!-- Ventana Modal de Proveedor WS -->
<div id="contenedorVentanaProveedorWS" class="modal">
<div id="ventanaProveedorWS" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarProveedorWS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarProveedorWS" runat="server" OnClick="lkbCerrarProveedorWS_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucProveedorGPSDiccionario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div class="columna4x" style="width:800px; height:470px;" runat="server">
<ucl:wucproveedorgpsdiccionario runat="server" id="wucProveedorGPSDiccionario" />
</div>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbProveedorWS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
