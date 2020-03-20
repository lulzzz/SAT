<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFacturado.ascx.cs" Inherits="SAT.UserControls.wucFacturado" %>
<%@ Register  Src="~/UserControls/wucAddendaComprobante.ascx" TagName="wucAddendaComprobante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucEmailCFDI.ascx" TagPrefix="tectos" TagName="wucEmailCFDI" %>

<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryControlFacturacionElectronica();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryControlFacturacionElectronica() {
    $(document).ready(function () {

        //Función de Validanción de Controles
        var validacionFacturado = function () {
            //Validando Controles
            var isValid1 = !$("#<%=txtFechaTC.ClientID%>").validationEngine('validate');
            var isValid2 = !$("#<%=txtFechaFactura.ClientID%>").validationEngine('validate');

            //Devolviendo Resultado
            return isValid1 && isValid2;
        }

        //Añadiendo Validación al Evento Click
        $("#<%=btnAceptarFactura.ClientID%>").click(validacionFacturado);

//Función de validación de campos
var validacionTimbrado = function (evt) {
var isValidP1 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
return isValidP1;
};

    //Función de validación de campos
    var validacionCancelacion = function (evt) {
        var isValidP1 = !$("#<%=txtMotivo.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
//Boton Guardar
    $("#<%=btnAceptarTimbrarFacturacionElectronica.ClientID%>").click(validacionTimbrado);
//Boton Cancelar
    $("#<%= btnAceptarCancelacionCFDI.ClientID%>").click(validacionCancelacion);
//Cargando Control DateTimePicker "Fecha Inicio"
$("#<%=txtFechaFactura.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Cargando 
$("#<%=txtFechaTC.ClientID%>").datetimepicker({
    lang: 'es',
    format: 'd/m/Y',
    timepicker: false,
    closeOnDateSelect: true,
    onSelectDate: function (selected, evnt) {
        //Asignando Valor antes del PostBack
        $("#<%=txtFechaTC.ClientID%>").val(selected.format('dd/MM/yyyy'));

        //Causando Actualización del Control
        __doPostBack('<%= txtFechaTC.UniqueID %>', '');
    }
});

});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryControlFacturacionElectronica();
</script>
<div class="contenido_pestañas_documentacion">

<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNofactura">No. Factura</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNofactura" runat="server" CssClass="textbox2x"
MaxLength="50" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatusfactura">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtEstatusFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTarifaCobroFactura">ID Tarifa de Cobro</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTarifaCobroFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTarifaCobroFactura" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"
Enabled="false" ClientIDMode="Static"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTarifaCobro">Tarifa</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblTarifaCobro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTarifaCobro" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaTC">Fecha T. Cambio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaTC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaTC" runat="server" CssClass="textbox2x validate[required, custom[date]]"
MaxLength="10" Enabled="false" OnTextChanged="txtFechaTC_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaFactura">Fecha Factura</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFactura" runat="server" CssClass="textbox2x validate[required, custom[dateTime24]]"
MaxLength="16" ClientIDMode="Static" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCondicionPagoFactura">Condición Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCondicionPagoFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCondicionPagoFactura" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<label for="txtSubTotal">Sub Total</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtSubTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSubTotal" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtImpRet">Importe Ret.</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtImpRet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImpRet" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtImpTra">Importe Tras.</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtImpTra" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImpTra" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtTotalFactura">Total</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTotalFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotalFactura" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlMonedaFactura">Moneda</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlMonedaFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMonedaFactura" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlMonedaFactura_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtTotalPesosFactura">Total Pesos</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTotalPesosFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotalPesosFactura" runat="server" CssClass="textbox validate[required, custom[min[1]]]"
Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
<asp:AsyncPostBackTrigger ControlID="ddlMonedaFactura" />
<asp:AsyncPostBackTrigger ControlID="txtFechaTC" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarFactura" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarFactura_Click"
Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarFactura" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAceptarFactura_Click"
Enabled="false" CausesValidation="true" ClientIDMode="Static" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="uplkbAplicarTarifa" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAplicarTarifa" runat="server" Text="Calcular Tarifa" OnClick="lkbAplicarTarifa_Click" ToolTip="Calcule Tarifa">
<img src="../Image/Calculadora.png" />                            
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorFactura" runat="server" CssClass="label_error" Width="450px"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkEditarFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbAplicarTarifa" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarCFDI" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacionCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:UpdatePanel runat="server" ID="uplkbRegistrarFacturaElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbRegistrarFacturaElectronica" runat="server" Text="Registrar" OnClick="lnkRegistrarFacturacionElectronica_Click" ToolTip="Registre CDFI" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbTimbrarFacturaElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbTimbrarFacturaElectronica" runat="server" Text="Timbrar" OnClick="lnkTimbrarFacturacionElectronica_Click" ToolTip="Timbrar CFDI" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbComentario" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbComentario" runat="server" Text="Comentario" OnClick="lkbComentario_Click" ToolTip="Comentario" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbVerComprobante" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerComprobante" runat="server" Text="Ver Comprobante" OnClick="lkbVerComprobante_Click" ToolTip="Ver Comprobante" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lnkEditarFactura" runat="server" Text="Editar Factura" OnClick="lnkEditarFactura_Click" ToolTip="Editar CFDI" />
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbEliminarCFDI" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEliminarCFDI" runat="server" Text="Eliminar CFDI" OnClick="lkbEliminarCFDI_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbCancelarCFDI" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCancelarCFDI" runat="server" Text="Cancelar CFDI" OnClick="lkbCancelarCFDI_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:UpdatePanel runat="server" ID="uplkbReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias"  OnClick="lkbReferencias_Click" ToolTip="Referencias" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel runat="server" ID="uplkbAddendaFacturaElectronica" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAddendaFacturaElectronica" runat="server" Text="Addendas" OnClick="lkbAddendaFacturaElectronica_Click" ToolTip="Addendas" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbPDF" runat="server" Text="PDF" OnClick="lkbPDF_Click" /></li>
<li>
<asp:UpdatePanel ID="ulkbXml" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbXml" runat="server"  Text="XML" OnClick="lkbXml_Click" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbXml" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbEmail" runat="server" Text="E-mail" OnClick="lkbEmail_Click"></asp:LinkButton></li>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="OnClick_lkbBitacora" ToolTip="Bitacora" />
</li>
</ul>
</li>
<li class="gray">
<a href="#" class="fa fa-book "></a>
<ul>
<li>
<asp:LinkButton ID="lkbVerRefacturacion" runat="server" Text="Ver Refacturación"  OnClick="lkbVerRefacturacion_Click" CommandName="VerRefacturacion" />
</li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
</asp:UpdatePanel>

<!-- Ventana Confirmación Registra Factura Electronica -->
<div id="contenidoConfirmacionRegistarFacturacionElectronica" class="modalControlUsuario">
<div id="confirmacionRegistarFacturacionElectronica" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
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
<label for="ddlTipoComrobante">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoComrobante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoComrobante" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTipoComrobante_SelectedIndexChanged" CssClass="dropdown2x" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
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
<asp:DropDownList ID="ddlFormaPago" runat="server"
CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
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
<asp:DropDownList ID="ddlSucursal" runat="server"
CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
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
<asp:DropDownList ID="ddlMetodoPago" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMetodoPago_SelectedIndexChanged"
CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlCuenta">Cuenta Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlCuenta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlCuenta" runat="server"
CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="lkbRegistrarFacturaElectronica" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoComrobante" />
<asp:AsyncPostBackTrigger ControlID="ddlMetodoPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorTerminoServicio" runat="server" UpdateMode="Conditional">
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
<asp:Button ID="btnRegistrarFacturaElectronica" runat="server" CssClass="boton" Text="Registrar" OnClick="btnRegistrarFacturaElectronica_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- Ventana Confirmación Timbrar Factura -->
<div id="contenidoConfirmacionTimbrarFacturacionElectronica" class="modalControlUsuario">
<div id="confirmaciontimbrarFacturacionElectronica" class="contenedor_ventana_confirmacion"> 
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
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" Text="" runat="server" CssClass="textbox validate[custom[onlyLetterSp]]"  MaxLength="10">
</asp:TextBox>
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
<asp:CheckBox ID="chkOmitirAddenda" runat="server" Text="Facturar sin 'Addenda'." />
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

<!-- Ventana Confirmación Addenda -->
<div id="contenidoConfirmacionAddenda" class="modalControlUsuario">
<div id="confirmacionAddenda" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAddenda" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAddenda" runat="server" Text="Cerrar" OnClick="lkbCerrarAddendaComprobante_Click" >
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
<asp:DropDownList ID="ddlAddenda" runat="server"   CssClass="dropdown2x" ></asp:DropDownList>
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

<!-- Ventana Confirmación Comprobante Addenda -->
<div id="contenidoConfirmacionWucComprobanteAddenda" class="modalControlUsuario"  >
<div id="confirmacionWucComprobanteAddenda"  class="contenedor_modal_seccion_completa_arriba"  style="top:15px;width:1050px" >
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarwucAddendaComprobante" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarwucAddendaComprobante" runat="server" Text="Cerrar" OnClick="lkbCerrarwucAddendaComprobante_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna3x">
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

<!-- Ventana Confirmación Email -->
<div id="contenidoConfirmacionEmail" class="modalControlUsuario">
<div id="confirmacionEmail" class="contenedor_ventana_confirmacion">
<asp:UpdatePanel ID="upwucEmailCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucEmailCFDI runat="server" ID="wucEmailCFDI" OnLkbCerrarEmail_Click="wucEmailCFDI_LkbCerrarEmail_Click" OnBtnEnviarEmail_Click="wucEmailCFDI_BtnEnviarEmail_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEmail" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>

<!-- Ventana Confirmación Eliminación CFDI -->
<div id="contenidoConfirmacionEliminarCFDI" class="modalControlUsuario">
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

<!-- Ventana Confirmación Cancelación CFDI -->
<div id="contenidoConfirmacionCancelacionCFDI" class="modalControlUsuario">
<div id="confirmacionCancelacionCFDI" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h3>Cancelar Factura Electrónica</h3>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upmtvCancelacionCFDI" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvCancelacionCFDI" runat="server">
<asp:View ID="vwFacturaSinAplicaciones" runat="server">
<div class="renglon2x">
<label class="mensaje_modal">
¿Realmente desea Cancelar la Factura Electrónica?
</label>
</div>
</asp:View>
<asp:View ID="vwFacturaConAplicaciones" runat="server">
<div class="renglon2x">
<label class="mensaje_modal">
Las siguientes Aplicaciones seran Eliminadas. ¿Desea Continuar?
</label>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvAplicaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAplicaciones" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false"
CssClass="gridview" ShowFooter="true" Width="100%">
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
<asp:BoundField DataField="FacturaFicha" HeaderText="Factura/Ficha" SortExpression="FacturaFicha" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCancelarCFDI" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCancelarCFDI" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivo">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine"  Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel></div></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
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

<!-- Ventana Referencias -->
<div id="contenedorVentanaReferencias" class="modalControlUsuario">
<div id="ventanaReferencias" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarDev" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" CommandName="referenciasRegistro" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="12">
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
<asp:DropDownList ID="ddlTamanoReferencias" runat="server" TabIndex="13" OnSelectedIndexChanged="ddlTamanoReferencias_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
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
<div  class ="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvReferencias" runat="server" AllowPaging="true" AllowSorting="true" Width="100%"
CssClass="gridview" ShowFooter="true" TabIndex="5" OnSorting="gvReferencias_Sorting" PageSize="25"
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
<br />
Seleccionados
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkTipoTodos" runat="server" AutoPostBack="True" Checked="true" CssClass="LabelResalta"
OnCheckedChanged="chkTipoTodos_CheckedChanged" Text="Todos" />
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
<asp:Button ID="btnRegistrarFE" runat="server"  OnClick="btnRegistrarFE_Click" CssClass="boton" Text="Registrar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana Comentario -->
<div id="contenidoComentario" class="modalControlUsuario"  >
<div id="confirmacionComentario"   class="contenedor_ventana_confirmacion" >
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrar" runat="server" CommandName="refacturacion" Text="Cerrar"  OnClick="lnkCerrar_Click">
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
<asp:TextBox ID="txtComentario" runat="server"  Text=" " CssClass="textbox2x" MaxLength="500" TabIndex="1"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel></div></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarComentario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarComentario" runat="server"  OnClick="btnAceptarComentario_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!--Ver Refacturaciones -->
<div id="contenidoVentanaRefacturaciones" class="modalControlUsuario">
<div id="ventanaRefacturaciones" class="contenedor_ventana_confirmacion">
<div style="text-align: right">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFF" runat="server" CommandName="refacturacion" OnClick="lnkCerrar_Click"  Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h3>Refacturaciones</h3>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFF">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoRefacturacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRefacturacion" runat="server" CssClass="dropdown" TabIndex="5"
OnSelectedIndexChanged="ddlTamanoRefacturacion_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoRefacturacion">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoRefacturacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoRefacturacion" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRefacturacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarRefacturacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarRefacturacion" OnClick="lnkExportarRefacturacion_Click" runat="server" Text="Exportar" TabIndex="6" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarRefacturacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvRefacturacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRefacturacion" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvRefacturacion_PageIndexChanging" OnSorting="gvRefacturacion_Sorting"
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
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" Visible="false" />
<asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha Expedición" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRefacturacion" />
<asp:AsyncPostBackTrigger ControlID="lkbVerRefacturacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>


