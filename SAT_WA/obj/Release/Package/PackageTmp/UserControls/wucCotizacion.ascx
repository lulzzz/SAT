<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCotizacion.ascx.cs" Inherits="SAT.UserControls.wucCotizacion" %>
<!--hoja de estilos que dan formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!--Script que valida los controles de la pagina-->
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!--Script que valida la insercion de datos en los controles-->
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryCotizacion
ConfiguraJQueryCotizacion();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryCotizacion() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable ValidaCotizacion
var validaCotizacion = function (evt) {
//Creación de las variables y asignacion de los controles de la pagina Ciudad
var isValid1 = !$("#<%=txtNoRequisicion.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtProducto.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtPrecio.ClientID%>").validationEngine('validate');
var isValid6 = !$("#<%=txtFechaCotizacion.ClientID%>").validationEngine('validate');
var isValid7 = !$("#<%=txtVigencia.ClientID%>").validationEngine('validate');
var isValid8 = !$("#<%=txtEntrega.ClientID%>").validationEngine('validate');
var isValid9 = !$("#<%=txtComentario.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8 && isValid9;
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaCotizacion);
//Realiza el autoComplete del controlProveedor
    $("#<%=txtCompaniaEmisor.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=4',
        appendTo: "<%=this.Contenedor%>"
    });
//Realiza el autoComplete del controlProveedor
    $("#<%=txtProveedor.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=14',
        appendTo: "<%=this.Contenedor%>"
    });
//Realiza el autoComplete del control Producto
    $("#<%=txtProducto.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=1',
        appendTo: "<%=this.Contenedor%>"
    });
});
$(document).ready(function () {
$("#<%=txtFechaCotizacion.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});           
});
}
ConfiguraJQueryCotizacion();
</script>
<div class="contenedor_media_seccion">
<div class="header_seccion">
<h2>Cotización</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompaniaEmisor">Compañia: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtCompaniaEmisor" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCompaniaEmisor" CssClass="textbox2x" TabIndex="1"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoRequisicion">No. Requicisión: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtNoRequisicion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtNoRequisicion" CssClass="textbox2x validate[required]" TabIndex="2"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProveedor">Proveedor: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProveedor" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProveedor" CssClass="textbox2x validate[required ,custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label  for="txtProducto">Producto: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProducto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProducto" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCantidad">Cantidad: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtCantidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtCantidad" CssClass="textbox2x valida[required]" TabIndex="5"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtPrecio">Precio: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtPrecio" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtPrecio" CssClass="textbox2x validate[required, custom[number]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlIdMoneda">Tipo Moneda: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="upddlMoneda" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlMoneda" CssClass="dropdown2x" TabIndex="7"></asp:DropDownList>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaCotizacion">Fecha Cotización: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtFechaCotizacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaCotizacion" CssClass="textbox2x validate[required,custom[dateTime24]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDiasVigencia">Vigencia Cotización: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtVigencia" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtVigencia" CssClass="textbox2x validate[required , custom[integer]]" TabIndex="9"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtEntrega">Dias de Entrega: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtEntrega" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtEntrega" CssClass="textbox2x validate[required,custom[integer]]" TabIndex="10"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtComentario">Comentarios: </label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtComentario" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtComentario" CssClass="textbox2x validate[required]" TabIndex="11" MaxLength="100"></asp:TextBox>
</ContentTemplate>   
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>                 
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar"   OnClick="btnGuardar_Click"  CssClass="boton" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="boton_cancelar" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click"  CssClass="boton" TabIndex="14" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>