<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucTerminoMovimientoVacio.ascx.cs" Inherits="SAT.UserControls.wucTerminoMovimientoVacio" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de Terminar de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryMovimiento();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryMovimiento() {
$(document).ready(function () {
//Función de validación Búsqueda Movimientos
var validacionBuscarMovimientos = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtCiudadOrigenT.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCiudadDestino.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtValor.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
var isValid6 = !$("#<%=txtFechaInicioSalida.ClientID%>").validationEngine('validate');
var isValid7 = !$("#<%=txtFechaFinSalida.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7
};
//Función de validación Terminar Movimientos
var validacionTerminarMovimiento = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtFechaTerminar.ClientID%>").validationEngine('validate');

return isValid1
};
//Botón Buscar Movimientos
$("#<%=btnBuscarMovimientos.ClientID %>").click(validacionBuscarMovimientos);
// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
// *** Fecha de inicio, fin de Deposito (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicioSalida.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFinSalida.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Botón Buscar Movimientos
//Botón Buscar Movimientos
$("#<%= btnAceptarTerminarMovimiento.ClientID %>").click(validacionTerminarMovimiento);
// *** Catálogos Autocomplete *** //
$("#<%=txtCiudadOrigenT.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8', appendTo: "<%=this.Contenedor%>" });
$("#<%=txtCiudadDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8', appendTo: "<%=this.Contenedor%>" });
$("#<%= txtFechaTerminar.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryMovimiento();
</script>
<div class="contenido_pestañas_documentacion">
    
<div class="contenedor_controles" >
<div class="header_seccion" >
<img src="../Image/Buscar.png" />
<h2>Buscar Movimientos en Vacío</h2>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus"  runat="server" CssClass="dropdown" TabIndex="4" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipo">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoAsignacion"  OnSelectedIndexChanged="ddlTipoAsignacion_OnSelectedIndexChanged" AutoPostBack="true"   runat="server" CssClass="dropdown" TabIndex="4" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblValor" runat="server">Unidad</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValor" runat="server"   CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="4" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtCiudadOrigenT">Ciudad Origen</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCiudadOrigenT" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadOrigenT" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtCiudadDestino">Ciudad Destino</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCiudadDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadDestino" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
    <div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="upchkRangoFechasSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkRangoFechasSalida" runat="server" Text="Fechas Salida Origen"
Checked="false" TabIndex="6" AutoPostBack="true" OnCheckedChanged="chkRangoFechasSalida_CheckedChanged" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicioSalida">Fecha Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicioSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicioSalida" Enabled="false" runat="server" CssClass="textbox validate[required,  custom[dateTime24]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechasSalida" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFinSalida">Fecha Final</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFinSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFinSalida" runat="server" Enabled="false" CssClass="textbox validate[required,  custom[dateTime24]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechasSalida" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="upchkRangoFechas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Fechas Llegada Destino"
Checked="false" TabIndex="6" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Fecha Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox validate[required,  custom[dateTime24]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Fecha Final</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox validate[required,  custom[dateTime24]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarMovimientos" runat="server" Text="Buscar" TabIndex="6" OnClick="btnBuscarMovimientos_Click"  CssClass="boton" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div></div>
</div></div>
<div class="renglon3x">
<div class="etiqueta" style="width:auto"> 
<label for="ddlTamanoMovimientos">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoMovimientos"></label>
<asp:DropDownList ID="ddlTamanoMovimientos" runat="server"   OnSelectedIndexChanged="ddlTamanoMovimientos_SelectedIndexChanged"  TabIndex="8" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarMovimientos">Ordenado Por:</label>
<asp:Label ID="lblOrdenarMovimientos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarMovimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarMovimientos" runat="server" Text="Exportar Excel"  OnClick="lkbExportarMovimientos_Click"   TabIndex="9"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura"  style ="height:320px" >
<asp:UpdatePanel ID="upgvMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvMovimientos"  OnRowDataBound="gvMovimientos_OnRowDataBound" ShowFooter="true"  OnPageIndexChanging="gvMovimientos_PageIndexChanging"  OnSorting="gvMovimientos_Sorting" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="true" TabIndex="10"
ShowHeaderWhenEmpty="True" 
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="IdMovimiento" HeaderText="No. Movimiento" SortExpression="IdMovimiento"  />
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus" >
<ItemTemplate>
<asp:Label ID="lblEstatus" Text='<%# Eval("Estatus") %>'  runat="server"  TabIndex="11"></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Unidad" HeaderText="Tractor" SortExpression="Unidad" />
<asp:BoundField DataField="Rem1" HeaderText="Rem1" SortExpression="Rem1" />
<asp:BoundField DataField="Rem2" HeaderText="Rem2" SortExpression="Rem2"  />
<asp:BoundField DataField="Dolly" HeaderText="Dolly" SortExpression="Dolly" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador"  />
<asp:BoundField DataField="Trans" HeaderText="Trans" SortExpression="Trans"  />
<asp:BoundField DataField="FechaSalida" HeaderText="Fecha Inicio" SortExpression="FechaSalida"  DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Fin" SortExpression="FechaLlegada" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen"  />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino"  />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms"  />
<asp:TemplateField HeaderText="" SortExpression="" >
<ItemTemplate>
<asp:LinkButton ID="lkbTerminar"  runat="server"  OnClick="lkbMovimientos_Click"  Text="Terminar"   CommandName="Terminar" TabIndex="11"></asp:LinkButton>
</ItemTemplate> 
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="" >
<ItemTemplate>
<asp:LinkButton ID="lkbReversaMov"  runat="server"  OnClick="lkbMovimientos_Click" Text="Reversa"  CommandName="Reversa"  TabIndex="11"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="" >
<ItemTemplate>
<asp:LinkButton ID="lkbEliminar"  runat="server"  OnClick="lkbMovimientos_Click" Text="Eliminar"  CommandName="Eliminar"  TabIndex="11"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarFechaTerminarMovimiento" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenidoFechaTerminarMovimiento" class="modal">
<div id="confirmacionFechaTerminarMovimiento" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarFechaTerminarMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarFechaTerminarMovimiento" OnClick="lkbCerrarFechaTerminarMovimiento_Click" runat="server" Text="Cerrar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>      
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Terminar Movimiento</h3>
</div>         
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblOrigen">Origen</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrigen" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblDestino">Destino</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblDestino" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblFechaInicio">Fecha de Inicio</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblFechaInicio" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaTerminar">Fecha de Término </label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaTerminar" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaTerminar" runat="server" Enabled="true" CssClass="textbox  validate[required], custom[dateTime24]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="chkFechaActual" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkFechaActual" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkFechaActual" runat="server" Text="¿Fecha Actual?" AutoPostBack="true" 
OnCheckedChanged="chkFechaActual_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarTerminarMovimiento" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarTerminarMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarTerminarMovimiento" runat="server"    OnClick="btnCancelarTerminarMovimiento_Click" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTerminarMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTerminarMovimiento" runat="server" CssClass="boton" OnClick="btnAceptarTerminarMovimiento_Click"   Text="Aceptar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>
<div id="contenidoConfirmacionEliminarMovimiento" class="modal">
<div id="confirmacionEliminarMovimiento" class="contenedor_ventana_confirmacion">   
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEliminarMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEliminarMovimiento"   OnClick="lkbCerrarEliminarMovimiento_Click" runat="server" Text="Cerrar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>                        
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Eliminar Movimiento</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea eliminar movimiento?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminarMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminarMovimiento" runat="server" OnClick="btnCancelarEliminarMovimiento_Click" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminarMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminarMovimiento" runat="server"  OnClick="btnAceptarEliminarMovimiento_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>