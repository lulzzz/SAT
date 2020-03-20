<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Comprobante.aspx.cs" Inherits="SAT.FacturacionElectronica.Comprobante" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <div id="encabezado_forma">
<img src="../Image/EnvioEvidencia.png" />
<h1>Comprobante Fiscal Digital</h1>
</div>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbEliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar">Eliminar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel></li>
<li>
<asp:UpdatePanel ID="uplkbCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbCancelar" OnClick="lkbElementoMenu_Click" CommandName="Cancelar">Cancelar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplbkBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbBitacora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora">Bitacora</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbReferenciasCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbReferenciasCFDI" OnClick="lkbElementoMenu_Click" CommandName="Referencias">Referencias</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbReferenciasCFDI" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbTimbrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbTimbrar" OnClick="lkbElementoMenu_Click"
CommandName="Timbrar">Timbrar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbVerTimbre"  />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbXML" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbXML" OnClick="lkbElementoMenu_Click"
CommandName="XML">XML</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbXML"  />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbPDF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbPDF" Visible="true" OnClick="lkbElementoMenu_Click"
CommandName="PDF">PDF</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbPDF"  />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="upVerTimbre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbVerTimbre" OnClick="lkbElementoMenu_Click"
CommandName="VerTimbre">Ver Timbre</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbVerTimbre"  />
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
</ul>
</li>
</ul>
</nav>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnEmisorReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEmisorReceptor" Text="Emisor - Receptor"  OnClick="btnVista_OnClick" CommandName="EmisorReceptor" runat="server" CssClass="boton_pestana_activo"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnDatosExpedicion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnDatosExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDatosExpedicion" Text="Datos Expedición" OnClick="btnVista_OnClick" runat="server" CommandName="DatosExpedicion" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEmisorReceptor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<br /> 
<div  class="contenido_tabs">
<asp:UpdatePanel ID="upmtvComprobante" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
<ContentTemplate>
<asp:MultiView ID="mtvComprobante" runat="server" ActiveViewIndex="0">
<asp:View ID="vwEmisorReceptor" runat="server">
<div>
<fieldset>
<legend>Datos Fiscales Principales</legend>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="lblID">
Id
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblID"  CssClass="label">ID</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtSerie">
Serie / Folio
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="1" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox" TabIndex="2" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEmisor">
Emisor
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox  ID="txtEmisor" Enabled="false" runat="server"  CssClass="textbox2x" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtDomicilioEmisor">
Domicilio
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDomicilioEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDomicilioEmisor" runat="server" CssClass="textbox2x" TabIndex="4" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlSucursal">
Sucursal
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server" CssClass="dropdown2x" TabIndex="5" 
AutoPostBack="true" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtDomicilioSucursal">
Domicilio
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDomicilioSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDomicilioSucursal" runat="server" CssClass="textbox2x" TabIndex="6" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />

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
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvConceptos" />
</Triggers>
</asp:UpdatePanel></div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtProveedor">
Receptor
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x" AutoPostBack="true"
TabIndex="7" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtDomicilioReceptor">
Domicilio
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDomicilioReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDomicilioReceptor" runat="server" CssClass="textbox2x" TabIndex="8" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlMetodoPago">
Método Pago
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown" TabIndex="9" 
AutoPostBack="true" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlNoCuentaPago">
No. Cta. Pago
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlNoCuentaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlNoCuentaPago" runat="server" TabIndex="10" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlTipoComprobante">
Tipo
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoComprobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoComprobante" runat="server" CssClass="dropdown" TabIndex="11"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEstatus">
Estatus
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="12" 
Enabled="False"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div></div>
</div>
</fieldset>
</asp:View>
<asp:View ID="vwDatosExpedicion" runat="server">
<div>
<fieldset>
<legend>Datos de Expedición y Parcialidades</legend>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlCondicionesPago">
Condiciones
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCondicionesPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCondicionesPago" runat="server" CssClass="dropdown2x" 
TabIndex="14">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlFormaPago">
Forma Pago
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlFormaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlFormaPago" runat="server" AutoPostBack="true" 
CssClass="dropdown2x" 
TabIndex="15">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtNoParcialidad">
Parcialidad
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoParcialidad" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtNoParcialidad" runat="server" CssClass="textbox_100px"
Enabled="False" MaxLength="3" TabIndex="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
<label for="txtTotalParcialidades" class="Label"> de </label>
<asp:UpdatePanel ID="uptxtTotalParcialidades" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtTotalParcialidades" runat="server"  CssClass="textbox_100px"
TabIndex="17" Enabled="False"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtSerie">
Serie / Folio Original
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerieOriginal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtSerieOriginal" runat="server" CssClass="textbox_100px"
Enabled="False" MaxLength="10" TabIndex="18" AutoPostBack="True" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />


</Triggers>
</asp:UpdatePanel>
<label class="Label" for="txtFolioOriginal"> / </label>
<asp:UpdatePanel ID="uptxtFolioOriginal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFolioOriginal" runat="server"  CssClass="textbox_100px" MaxLength="9" 
TabIndex="19" Enabled="False" AutoPostBack="True" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />


</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upchkCFD2_2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:CheckBox ID="chkCFD2_2" runat="server" CssClass="label" 
Text="Es CFD" AutoPostBack="true" Enabled="False" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFolioOriginal">
Fecha Original
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaFolioOriginal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaFolioOriginal" runat="server" CssClass="texbox" 
Enabled="false" TabIndex="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtMontoFolioOriginal">
Monto Original
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMontoFolioOriginal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMontoFolioOriginal" runat="server" CssClass="texbox" 
Enabled="false" MaxLength="16" TabIndex="21"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />

</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtTipoCambio">
Moneda / T.C.
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMoneda" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMoneda" runat="server" AutoPostBack="true" 
CssClass="dropdown"
TabIndex="22">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uptxtTipoCambio" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtTipoCambio" runat="server" CssClass="textbox" 
Enabled="false" TabIndex="22"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaCaptura">
Fecha Captura
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaCaptura" runat="server" CssClass="textbox_240px"
Enabled="false" TabIndex="23"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaExpedicion">
Fecha Expedición
</label>
</div>
<div class="celda_control">
<asp:UpdatePanel ID="uptxtFechaExpedicion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaExpedicion" runat="server" CssClass="textbox_240px" 
Enabled="false" TabIndex="24"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  />
    <asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
 <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upchkGenerado" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkGenerado" runat="server" CssClass="Label" 
Text="Timbrado" TabIndex="25" Enabled="False" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtLugarExpedicion">
Lugar Expedición
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtLugarExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLugarExpedicion" runat="server" CssClass="textbox2x" 
Enabled="false" TabIndex="26"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtSelloDigital">
Sello Digital
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSelloDigital" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSelloDigital" runat="server"  CssClass="textbox2x"
Enabled="false" TabIndex="27"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaCancelacion">
Fecha Cancelación
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaCancelacion" runat="server"  CssClass="textbox2x"
Enabled="false" TabIndex="28"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</fieldset>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnDatosExpedicion" />
<asp:AsyncPostBackTrigger ControlID="btnEmisorReceptor" />
</Triggers>
</asp:UpdatePanel>
</div>

<div class="columna3x">
<div class="header_seccion">
<h2>Detalles del Comprobante</h2>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnActualizarConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnActualizarConceptos" runat="server" Text="Actualizar" CssClass="boton" OnClick="btnActualizarConceptos_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamañoGridViewConceptos">Mostrar:</label>
</div>
<div class="control_100px">
<asp:DropDownList ID="ddlTamañoGridViewConceptos" runat="server" CssClass="dropdown_100px"
AutoPostBack="true" OnSelectedIndexChanged="ddlTamañoGridViewConceptos_SelectedIndexChanged"
TabIndex="29">
</asp:DropDownList>
</div>
<div class="etiqueta_50px">
<label for="lblCriterioGridViewConceptos">Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewConceptos" runat="server" Text="" CssClass="Label"></asp:Label></ContentTemplate><Triggers>
<asp:AsyncPostBackTrigger ControlID="gvConceptos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel runat="server" ID="uplkbExcelConceptos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExcelConceptos" runat="server" OnClick="lkbExcel_Click" EnableViewState="False"
CommandName="Conceptos" TabIndex="30">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExcelConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvConceptos" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" ShowFooter="True"  CssClass="gridview"
OnPageIndexChanging="gvConceptos_PageIndexChanging" 
        OnSorting="gvConceptos_Sorting" TabIndex="31">
<Columns>
<asp:BoundField DataField="Cantidad" HeaderText="Cant." 
SortExpression="Cantidad" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" 
        SortExpression="Concepto">
</asp:BoundField>
<asp:BoundField DataField="Identificador" HeaderText="Identificador" 
        SortExpression="Identificador">
</asp:BoundField>
<asp:BoundField DataField="Referencia1" HeaderText="Referencia 1" 
        SortExpression="Referencia1">
</asp:BoundField>
<asp:BoundField DataField="Referencia2" HeaderText="Referencia 2" 
        SortExpression="Referencia2">
</asp:BoundField>
<asp:BoundField DataField="Referencia3" HeaderText="Referencia 3" 
        SortExpression="Referencia3">
</asp:BoundField>
<asp:BoundField DataField="PrecioUnitario" HeaderText="Precio U." SortExpression="PrecioUnitario"
DataFormatString="{0:c4}" >
</asp:BoundField>
<asp:BoundField DataField="Importe" DataFormatString="{0:c4}" 
HeaderText="Importe" SortExpression="Importe">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lkbReferenciaConcepto" runat="server" Text="Referencia" OnClick="lkbReferenciaConcepto_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewConceptos" />
<asp:AsyncPostBackTrigger ControlID="btnActualizarConceptos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">
<div class="header_seccion">
<h2>Totales del Comprobante</h2>
</div>
<div>
<fieldset>
<table>
<tr>
<th>Concepto</th>
<th>Moneda Captura</th>
<th>Moneda Nacional</th>
</tr>
<tr>
<td>
<label for="lblSubtotalCaptura" class="Label">
Subtotales</label>
</td>
<td >
<asp:UpdatePanel ID="uplblSubTotalCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSubtotalCaptura" Text="$0.00"  CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
<td>
<asp:UpdatePanel ID="uplblSubtotalNacional" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSubtotalNacional" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
</tr>
<tr>
<td>
<label for="lblDescuentosCaptura" class="Label">
Descuentos</label>
</td>
<td>
<asp:UpdatePanel ID="uplblDescuentosCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDescuentosCaptura" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
<td>
<asp:UpdatePanel ID="uplblDescuentosNacional" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDescuentosNacional" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
</tr>
<tr>
<td>
<label for="lblImpuestosCaptura" class="Label">
Impuestos</label>
</td>
<td>
<asp:UpdatePanel ID="uplblImpuestosCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblImpuestosCaptura" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
<td> 
<asp:UpdatePanel ID="uplblImpuestosNacional" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblImpuestosNacional" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
</tr>
<tr>
<td>
<label for="lblTotalCaptura" class="Label">
Total</label>
</td>
<td>
<asp:UpdatePanel ID="uplblTotalCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalCaptura" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
<td>
<asp:UpdatePanel ID="uplblTotalNacional" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalNacional" Text="$0.00" CssClass="label" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar"  /><asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID ="gvConceptos" />
</Triggers>
</asp:UpdatePanel>
</td>
</tr>
</table>
</fieldset>
</div>
</div>
<div id="contenidoConfirmacionTimbrarFacturacionElectronica" class="modal">
<div id="confirmaciontimbrarFacturacionElectronica"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarFacturacionElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTimbrarFacturacionElectronica" runat="server" Text="Cerrar"  OnClick="lkbCerrarTimbrarFacturacionElectronica_Click"  >
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
<asp:UpdatePanel ID="uptxtSerieTimbrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerieTimbrar" Text="" runat="server" CssClass="textbox validate[custom[onlyLetterSp]]"  MaxLength="10">
</asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="upchkOmitirAddenda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkOmitirAddenda" runat="server" Text="Facturar sin 'Addenda'." />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarFacturacionElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbTimbrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbrarFacturacionElectronica" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbrarFacturacionElectronica" runat="server"   OnClick="btnAceptarTimbrarFacturacionElectronica_Click" CssClass ="boton"  Text="Timbrar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>
</asp:Content>
