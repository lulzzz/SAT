<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucClasificacion.ascx.cs" Inherits="SAT.UserControls.Clasificacion" %>

<!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlFlota">Flota</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlFlota" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlFlota" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlRegion">Región</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlRegion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlRegion" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlUbicacionTerminal">Terminal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUbicacionTerminal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUbicacionTerminal" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Tipo Servicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlAlcanceServicio">Alcance Servicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlAlcanceServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlAlcanceServicio" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDetalleNegocio">Operacion Servicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlDetalleNegocio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlDetalleNegocio" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlClasificacion1">Clasificación 1</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlClasificacion1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlClasificacion1" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlClasificacion2">Clasificación 2</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlClasificacion2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlClasificacion2" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Visible="false">Por Asignar</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlr">
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora"  OnClick="OnClick_lkbBitacora" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
OnClick="btnCancelar_Click" CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>        
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar"
OnClick="btnGuardar_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>