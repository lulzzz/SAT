<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAsignacionDiesel.ascx.cs" Inherits="SAT.UserControls.wucAsignacionDiesel" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryControlAsignacion();
}
}
//Declarando Función de Configuración
function ConfiguraJQueryControlAsignacion() {
$(document).ready(function () {

});
}
//Invocando Función
ConfiguraJQueryControlAsignacion();
</script>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Vales de Diesel</h2>
</div>    
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblId">No. de Vale</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Text="Por Asignar"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblBeneficiario">Beneficiario</label>
</div>
<asp:UpdatePanel ID="upmtvBeneficiario" runat="server">
<ContentTemplate>
<asp:MultiView ID="mtvBeneficiario" runat="server" ActiveViewIndex="0">
<asp:View ID="vwlblBeneficiario" runat="server">
<div class="control2x">
<asp:UpdatePanel ID="uplblBeneficiario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblBeneficiario" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwddlBeneficiario" runat="server">
<div class="control2x">
<asp:UpdatePanel ID="upddlBeneficiario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlBeneficiario" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlBeneficiario_SelectedIndexChanged" runat="server"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtOperadorProveedor">Operador/Proveedor</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtOperadorProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtOperadorProveedor" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
<asp:AsyncPostBackTrigger ControlID="ddlBeneficiario" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtUbicacion">Estación Combustible</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="dropdown2x" AutoPostBack="true"
OnSelectedIndexChanged="ddlUbicacion_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label   for="ddlUnidadDiesel">Unidad Diesel</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUnidadDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnidadDiesel" CssClass="dropdown2x" AutoPostBack="true"  OnSelectedIndexChanged="ddlUnidadDiesel_SelectedIndexChanged"  runat="server"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtLitros">Litros</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtLitros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLitros" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber4]]" 
AutoPostBack="true" OnTextChanged="txtLitros_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
<asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
<asp:AsyncPostBackTrigger ControlID="rdbMagna" />
<asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
</Triggers>
</asp:UpdatePanel></div>
    <div class="etiqueta_155px">
<asp:UpdatePanel ID="uplnkCalculado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCalculado" runat="server" Text="Calculado"  OnClick="lnkCalculado_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="ddlUnidadDiesel" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtFecCarga">Fecha de Carga</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecCarga" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtLitros">Total</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uplblTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblTotal" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
<asp:AsyncPostBackTrigger ControlID="txtLitros" />
<asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
<asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
<asp:AsyncPostBackTrigger ControlID="rdbMagna" />
<asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblImpresion">Cantidad Disponible</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCantidadDisp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblCantidadDisp" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
<asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblImpresion">Impresiones</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblImpresion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblImpresion" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="ddlTipoVale">Tipo de Vale</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoVale" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtFecSol">Fecha de Solicitud</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecSol" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecSol" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbDiesel" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged" 
    GroupName="Combustible" AutoPostBack="true" Text="Diesel" Checked="true" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbMagna" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbMagna" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged" 
    GroupName="Combustible" AutoPostBack="true" Text="G. Magna" Checked="false" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uprdbPremiun" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbPremiun" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged" 
    GroupName="Combustible" AutoPostBack="true" Text="G. Premiun" Checked="false" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtCostoDiesel">Costo de Diesel</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtCostoDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCostoCombustible" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber], min[1]]" 
    AutoPostBack="true" OnTextChanged="txtCostoCombustible_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarDiesel" />
<asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
<asp:AsyncPostBackTrigger ControlID="rdbMagna" />
<asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
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
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="boton"
OnClick="btnImprimir_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton"
OnClick="btnGuardar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnReferencias" runat="server" Text="Referencias" CssClass="boton" OnClick="btnReferencias_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenedorVentanaConfirmacionDiesel" class="modal">
<div id="ventanaConfirmacionDiesel" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Al cambiar la Fecha de Carga, se modificará el Precio del Diesel.</h2>
</div>
<div class="renglon">
<asp:UpdatePanel ID="upbtnAceptarDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarDiesel" runat="server" Text="Aceptar" OnClick="btnAceptarDiesel_Click" CssClass="controlBoton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
