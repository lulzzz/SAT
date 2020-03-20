<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFacturadoConceptoV33.ascx.cs" Inherits="SAT.UserControls.wucFacturadoConceptoV33" %>
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryControlFacturaConceptoV33();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryControlFacturaConceptoV33() {
$(document).ready(function () {
//Función de validación de campos
var validacionConcepto = function () {
var isValidP1 = !$("#<%=txtCantidadFacturaConcepto.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtValorUniFacturaConcepto.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtTasaImpTraFacturaConcepto.ClientID%>").validationEngine('validate');
var isValidP4 = !$("#<%=txtTasaImpRetFacturaConcepto.ClientID%>").validationEngine('validate');

return isValidP1 && isValidP2 && isValidP3 && isValidP4;
};
//Validación de campos requeridos
    $("#<%=this.btnAceptarFacturaConcepto.ClientID%>").click(validacionConcepto);
    $("#<%=this.ddlConceptoCobroFacturaConcepto.ClientID%>").change(function () {
        //Asignando Enfoque al Control que dispara el Evento
        $("#<%=ddlConceptoCobroFacturaConcepto.ClientID%>").focus();
    });

//Calcular campo de Importe al cambiar el Valor de la Cantidad
    $("#<%=txtCantidadFacturaConcepto.ClientID%>").change(function () {

        //Invocando Función
        calculaTotalImporte();
    });
    //Calcular campo de Importe al cambiar el Valor Unitario
    $("#<%=txtValorUniFacturaConcepto.ClientID%>").change(function () {

        //Invocando Función
        calculaTotalImporte();
    });

    //Declarando Función de Calculo
    function calculaTotalImporte() {

        //Obteniendo Valores
        var cantidad = $("#<%=txtCantidadFacturaConcepto.ClientID%>").val();
        var valor_u = $("#<%=txtValorUniFacturaConcepto.ClientID%>").val();
        var importe = 0;

        //Validando que existan la Cantidad
        if (cantidad == "")
            //Asignando '0's a la Variable
            cantidad = "0";

        //Validando que exista el Valor Unitario
        if (valor_u == "")
            //Asignando '0's a la Variable
            valor_u = "0";

        //Calculando Importe
        importe = parseFloat(cantidad) * parseFloat(valor_u);

        //Asignando Valores
        $("#<%=txtImporteFacturaConcepto.ClientID%>").val(importe);
        $("#<%=txtCantidadFacturaConcepto.ClientID%>").val(cantidad);
        $("#<%=txtValorUniFacturaConcepto.ClientID%>").val(valor_u);
    }
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryControlFacturaConceptoV33();
</script>
<div class="contenido_pestañas_documentacion">
    <asp:Panel ID="pnlControlFacturaConcepto" runat="server" DefaultButton="btnAceptarFacturaConcepto">
<div class="renglon_pestaña_documentacion">
<div class="control_60px">
<label for="txtCantidadFacturaConcepto">Cantidad</label>
<asp:UpdatePanel ID="uptxtCantidadFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidadFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]" 
MaxLength="9" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<label for="ddlConceptoCobroFacturaConcepto">Concepto Cobro</label>
<asp:UpdatePanel ID="upddlConceptoCobroFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConceptoCobroFacturaConcepto" runat="server" CssClass="dropdown" AutoPostBack="true" 
OnSelectedIndexChanged="ddlConceptoCobroFacturaConcepto_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="ddlUnidadFacturaConcepto">Unidad</label>
<asp:UpdatePanel ID="upddlUnidadFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnidadFacturaConcepto" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlConceptoCobroFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<label for="txtIdentificadorFacturaConcepto">Identificador</label>
<asp:UpdatePanel ID="uptxtIdentificadorFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtIdentificadorFacturaConcepto" runat="server" CssClass="textbox" 
MaxLength="2000"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="txtValorUniFacturaConcepto">Valor Unitario</label>
<asp:UpdatePanel ID="uptxtValorUniFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorUniFacturaConcepto" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" 
MaxLength="9" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<label for="txtImporteFacturaConcepto">Importe</label>
<asp:UpdatePanel ID="uptxtImporteFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImporteFacturaConcepto" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtValorUniFacturaConcepto" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="ddlImpuestoTrasladado">Tipo</label>
<asp:UpdatePanel ID="upddlImpuestoTrasladado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlImpuestoTrasladado" runat="server" style="width:60px"  CssClass="dropdown" AutoPostBack="false" 
></asp:DropDownList>
                
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="txtTasaImpTraFacturaConcepto">Tasa IVA</label>
<asp:UpdatePanel ID="uptxtTasaImpTraFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTasaImpTraFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]" 
MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlConceptoCobroFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="ddlImpuestoRetendido">Tipo</label>
<asp:UpdatePanel ID="upddlImpuestoRetenido" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlImpuestoRetenido" runat="server" CssClass="dropdown" style="width:60px"
></asp:DropDownList>                
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<label for="txtTasaImpRetFacturaConcepto">Tasa Ret</label>
<asp:UpdatePanel ID="uptxtTasaImpRetFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTasaImpRetFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]" 
MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlConceptoCobroFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upbtnAceptarFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarFacturaConcepto" runat="server" OnClick="btnAceptarFacturaConcepto_Click"
CssClass="boton" Text="Actualizar" CausesValidation="true" ClientIDMode="Static" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" OnClick="btnCancelar_Click"
CssClass="boton_cancelar" Text="Cancelar" CausesValidation="true" ClientIDMode="Static" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
<div class="renglon100Per"></div>
<div class="renglon_pestaña_documentacion">
<div class="control2x">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamano">Mostrar:</label>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenado" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2xr">
<asp:UpdatePanel ID="uplnkExportarExcel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarExcel" runat="server" Text="Exportar" 
OnClick="lnkExportarExcel_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarExcel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvConceptosFacturaConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvConceptosFacturaConcepto" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
OnSorting="gvConceptosFacturaConcepto_Sorting" OnPageIndexChanging="gvConceptosFacturaConcepto_PageIndexChanging" ShowFooter="True"
PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="ConceptoCobro" HeaderText="Concepto de Cobro" SortExpression="ConceptoCobro" />
<asp:BoundField DataField="ValorUnitario" HeaderText="Valor Unitario" SortExpression="ValorUnitario" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ImportePesos" HeaderText="Importe MXN" SortExpression="ImportePesos" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TasaImpuestoRetenido" HeaderText="Tasa Ret." SortExpression="TasaImpuestoRetenido" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ImporteRetenido" HeaderText="Retenido" SortExpression="ImporteRetenido" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TasaImpuestoTrasladado" HeaderText="Tasa IVA" SortExpression="TasaImpuestoTrasladado" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ImporteTrasladado" HeaderText="Trasladado" SortExpression="ImporteTrasladado" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarFacturaConcepto" runat="server" Text="Editar" OnClick="lnkEditarFacturaConcepto_Click"></asp:LinkButton><br />
<asp:LinkButton ID="lnkEliminarFacturaConcepto" runat="server" Text="Eliminar" OnClick="lnkEliminarFacturaConcepto_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora"  OnClick="OnClick_lkbBitacora"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uplnkReferenciasConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkReferenciasConcepto" runat="server" Text="Referencias" OnClick="lnkReferenciasConcepto_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkReferenciasConcepto" />
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon_pestaña_documentacion"></div>
</div>