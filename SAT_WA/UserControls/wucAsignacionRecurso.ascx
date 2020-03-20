<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAsignacionRecurso.ascx.cs" Inherits="SAT.UserControls.wucAsignacionRecurso" %>
<%@ Register Src="~/UserControls/wucDeposito.ascx" TagName="wucDeposito" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucAsignacionDiesel.ascx" TagName="ucAsignacionDiesel" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucMovimientoVacioSinOrden.ascx" TagName="wucMovimientoVacioSinOrden" TagPrefix="tectos" %>
<%@ Register   Src="~/UserControls/wucRutaIave.ascx" TagPrefix="tectos" TagName="wucCalcularRuta" %>
<!-- Estilos -->
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUCAsignacionRecurso();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryUCAsignacionRecurso() {
$(document).ready(function () {
//Autocomplete de Clientes Compania
$("#<%=txtOtroProveedor.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=57&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
appendTo: "<%=this.Contenedor%>",
select: function (event, ui) {
//Asignando Selección al Valor del Control
$("#<%=txtOtroProveedor.ClientID%>").val(ui.item.value);
//Causando Actualización del Control
__doPostBack('<%= txtOtroProveedor.UniqueID %>', '');
}
});
});
}

//Invocando Método de Configuración
ConfiguraJQueryUCAsignacionRecurso();
</script>
<asp:UpdatePanel ID="upmvwPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mvwPrincipal" runat="server" ActiveViewIndex="0">
<asp:View ID="vwAsignacion" runat="server">
<!-- VENTANA MODAL QUE PERMITE ASIGNAR RECURSOS AL SERVICIO -->
<div class="header_seccion">
<img src="../Image/Transportista.png" />
<h2>Asignación de Recursos</h2>
</div>
<div class="seccion_controles">                
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipo">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoAsignacion" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoAsignacion_OnSelectedIndexChanged" runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
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
<asp:TextBox ID="txtValor" runat="server" CssClass="textbox" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacion">Ubicación Actual</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkUbicacionActual" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkUbicacionActual" runat="server" AutoPostBack="true" TabIndex="8" Checked="true" OnCheckedChanged="chkUbicacionActual_CheckedChanged" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarRecursos" runat="server" TabIndex="4" CssClass="boton" Text="Buscar" OnClick="btnBuscarRecursos_OnClick" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorRecursosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorRecursosAsignados" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarRecursos" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignadoAlrecurso" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>
</div>
</div>
<div class="contenedor_recursos_asignados">
<div class="header_seccion">
<img src="../Image/IndicadorUnidadesPatio.png" />
<h2>Recursos Asignados</h2>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvRecursosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosAsignados" runat="server"  AutoGenerateColumns="False" TabIndex="8"
ShowFooter="True" CssClass="gridview"  Width="100%" >
<Columns>
<asp:BoundField DataField="Asignacion" HeaderText="Tipo" SortExpression="Asignacion" />
<asp:BoundField DataField="EstatusAsignacion" HeaderText="Estatus Asignación" SortExpression="EstatusAsignacion" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbQuitar" runat="server" Text="Quitar" OnClick="lkbRecursosAsignados_Click" CommandName="Quitar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbAnticipos" runat="server" Text="Anticipos" OnClick="lkbRecursosAsignados_Click" CommandName="Anticipos" TabIndex="11"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarRecursos" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignadoAlrecurso" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAnticiposR" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
<div class="contenedor_seccion_95per">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlRecursosDisponibles">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoRecursosDisponibles" runat="server" OnSelectedIndexChanged="ddlTamanoRecursosDisponibles_OnSelectedIndexChanged" AutoPostBack="true" CssClass="dropdown" TabIndex="5">
</asp:DropDownList>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarRecursosDisponibles">Ordenado Por:</label>
<asp:Label ID="lblOrdenarRecursosDisponibles" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width: auto">
<asp:UpdatePanel ID="lnk" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRecursosDisponibles" runat="server" OnClick="lkbExportarRecursosDisponibles_OnClick" Text="Exportar" TabIndex="7"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>            
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosDisponibles" runat="server" AutoGenerateColumns="true" CssClass="gridview" OnSorting="gvRecursosDisponibles_Sorting" OnPageIndexChanging="gvRecursosDisponibles_PageIndexChanging"
OnRowDataBound="gvRecursosDisponibles_OnRowDataBound" AllowPaging="true"
AllowSorting="true" ShowFooter="true" PageSize="5" Width="100%">
<Columns>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarRecursos" />
<asp:AsyncPostBackTrigger ControlID="chkUbicacionActual" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignadoAlrecurso" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>                
</div>
</asp:View>
<asp:View ID="vwAnticipos" runat="server">
<!-- VENTANA MODAL QUE PERMITE REALIZAR DEPOSITOS y GENERAR VALES DE DIESEL  -->
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAnticipos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAnticiposR" runat="server" Text="Cerrar" OnClick="lkbCerrarAnticiposR_Click">
<img src="../Image/Flecha.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/DepositosVale.png" />
<h2>Asignación de Depositos y Diesel</h2>
</div>
<asp:UpdatePanel ID="upmtvAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvAnticipos" runat="server" ActiveViewIndex="1">
<asp:View ID="VwDepositos" runat="server">                           
<div class="columna">
<asp:UpdatePanel ID="upucDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucDeposito ID="ucDepositos" runat="server" OnClickRegistrar="ucDepositos_ClickRegistrar" OnClickEliminar="ucDepositos_ClickEliminar"
OnClickCancelar="ucDepositos_ClickCancelar" OnClickSolicitar="ucDepositos_ClickSolicitar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoDeposito" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="VwDiesel" runat="server">                           
<asp:UpdatePanel ID="upwucDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:ucAsignacionDiesel ID="ucAsignacionDiesel" runat="server" OnClickGuardarAsignacion="ucAsignacionDiesel_ClickGuardarAsignacion"
OnClickCancelarAsignacion="ucAsignacionDiesel_ClickCancelarAsignacion"    OnClickCalculadoDiesel="ucAsignacionDiesel_ClickCalculado1"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoVale" />
</Triggers>
</asp:UpdatePanel>                            
</asp:View>
<asp:View ID="VwReporteAnticipos" runat="server">
<div class="columna3x" style="margin-left:10px;">
<div class="renglon3x">
<div class="control2x">
<asp:UpdatePanel ID="upmtvAsignacionActivaDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvAsignacionActivaDeposito" runat="server">
<asp:View ID="vwAsignacionMovimiento" runat="server">  
<asp:Label ID="lblAsignacionMovimiento" runat="server" CssClass="label_negrita"></asp:Label>         
</asp:View>
<asp:View ID="vwOtroProveedor" runat="server">
<asp:TextBox ID="txtOtroProveedor" runat="server" CssClass="textbox2x" AutoPostBack="true" OnTextChanged="txtOtroProveedor_TextChanged"></asp:TextBox>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="chkOtroProveedor" />        
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkOtroProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkOtroProveedor" runat="server" Text="Otro Proveedor (Depósito)" AutoPostBack="true" Checked="false" OnCheckedChanged="chkOtroProveedor_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">

</div>
</div>
</div>
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoAnticipos">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoAnticipos" runat="server" OnSelectedIndexChanged="ddlTamanoAnticipos_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown" TabIndex="5">
</asp:DropDownList>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblOrdenarAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarAnticipos">Ordenado Por:</label>
<asp:Label ID="lblOrdenarAnticipos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width: auto">
<asp:LinkButton ID="lkbExportarAnticipos" runat="server" Text="Exportar" OnClick="lkbExportarAnticipos_Click" TabIndex="7"></asp:LinkButton>
</div>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAnticipos" runat="server" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="true"
ShowFooter="True" AutoGenerateColumns="False" PageSize=" 5" OnSorting="gvAnticipos_Sorting" OnPageIndexChanging="gvAnticipos_PageIndexChanging" OnRowDataBound="gvAnticipos_RowDataBound">
<Columns>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Num" HeaderText="Folio" SortExpression="Num" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Precio" HeaderText="Costo" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Monto" HeaderText="Total" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización " SortExpression="FechaAutorizacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaCargaoDeposito" HeaderText="Fecha Carga o Depósito" SortExpression="FechaCargaoDeposito" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia Operador/Unidad" SortExpression="Referencia" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Programado" HeaderText="Programado" SortExpression="Programado" Visible="false" />
<asp:TemplateField HeaderText="Fac. Proveedor" SortExpression="FacturaProveedor">
<ItemTemplate>
<asp:LinkButton ID="lkbFacturaProveedor" runat="server" Text='<%# Eval("FacturaProveedor") %>' OnClick="lkbAnticipos_OnClick" CommandName="Factura"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Movimiento" SortExpression="Movimiento">
<ItemTemplate>
<asp:Label ID="lblMovimiento" runat="server" ToolTip='<%# Eval("Movimiento") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Movimiento").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Información" SortExpression="Informacion">
<ItemTemplate>
<asp:Label ID="lblInformacion" runat="server" ToolTip='<%# Eval("Informacion") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Informacion").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbAnticipos_OnClick" CommandName="Editar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoAnticipos" />
<asp:AsyncPostBackTrigger ControlID="txtOtroProveedor" />
<asp:AsyncPostBackTrigger ControlID="chkOtroProveedor" />   
 <asp:AsyncPostBackTrigger ControlID="ucDepositos" />     
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorAnticipos" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoVale" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoDeposito" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoDepositoProgramados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoDepositoProgramado" Text="D.Programados" OnClick="btnNuevoDepositoProgramado_Click" CommandName="DepositoProgamado" runat="server" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoDeposito" Text="Depósitos" OnClick="btnNuevoDeposito_Click" CommandName="Deposito" runat="server" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoVale" Text="Vales Diesel" runat="server" OnClick="btnNuevoVale_Click" CommandName="Operador" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCalcularRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCalcularRuta" Text="Calcular Ruta" runat="server"  OnClick="btnCalcularRuta_Click" CommandName="CalcularRuta" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
<asp:View ID="VwCalcularRuta" runat="server"> 
<%--<div class="columna">--%>
<asp:UpdatePanel ID="upwucCalcularRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCalcularRuta runat="server" ID="wucCalcularRuta" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCalcularRuta" />
</Triggers>
</asp:UpdatePanel>
<%--</div>--%>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoVale" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoDeposito" />
<asp:AsyncPostBackTrigger ControlID="btnCalcularRuta" />
<asp:AsyncPostBackTrigger ControlID="ucDepositos" />
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
</Triggers>
</asp:UpdatePanel>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAnticiposR" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
</Triggers>
</asp:UpdatePanel>

<!-- VENTANA MODAL QUE ADVIERTE SOBRE LA DESASIGNACION DE TRACTORES Y OPERADORES LIGADOS -->
<div id="contenidoConfirmacionQuitarRecursos" class="modal"> <!-- class="modal" -->
<div id="confirmacionQuitarRecursos" class="contenedor_ventana_confirmacion"> <!-- class="contenedor_ventana_confirmacion" -->
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Eliminación de Recursos</h2>
</div>            
<div class="columna2x">                
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensaje" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminacionRecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminacionRecurso" runat="server" OnClick="btnCancelarEliminacionRecurso_OnClick" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminacionRecurso" runat="server" CssClass="boton" OnClick="btnAceptarEliminacionRecurso_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE ADVIERTE SOBRE LA ASIGNACION DE TRACTORES Y OPERADORES VICULADOS -->
<div id="contenidoConfirmacionAsignadoAlRecurso" class="modal">
<div id="confirmacionAsignadoAlRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Asignación de Recursos Vinculados</h2>
</div>            
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeAsignadoAlRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeAsignadoAlRecurso" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarAsignadoAlrecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarAsignadoAlrecurso" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarAsignadoAlrecurso_OnClick" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAsignadoAlrecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAsignadoAlrecurso" runat="server" CssClass="boton" OnClick="btnAceptarAsignadoAlrecurso_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE ADVIERTE SOBRE LA LIBERACIÓN DE TRACTORES Y OPERADORES VICULADOS -->
<div id="contenidoConfirmacionLiberacionRecurso" class="modal">
<div id="confirmacionLiberacionRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Liberación de Recursos Vinculados</h2>
</div>            
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeLiberacionRecurso" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarLiberacionRecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarLiberacionRecurso" runat="server" OnClick="btnCancelarLiberacionRecurso_Click" CssClass="boton_cancelar"  Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarLiberacionRecurso" OnClick="btnAceptarLiberacionRecurso_Click" runat="server" CssClass="boton"  Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div><div id="contenedorVentanaConfirmacionInformacionCalculado" class="modal">
<div id="ventanaConfirmacionInformacionCalculado" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarVentanaInformacionCalculado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaInformacionCalculado" runat="server" CommandName="InformacionCalculado" Text="Cerrar"   OnClick="lnkCerrarVentanaInformacionCalculado_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Información Diesel vs Kms.</h2>
</div>
<div class="columna2x">
    <div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblCapacidadTanque">Capacidad Tanque:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCapacidadTanque" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCapacidadTanque" runat="server" Text="0"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    <div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblFechaUnltimaCarga">Fecha Última Carga:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblFechaUltimaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaUltimaCarga" runat="server" Text="Por Asignar"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    <div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblKmsUltimaCarga">Kms. Última Carga:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblKmsUltimaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblKmsUltimaCarga" runat="server" Text="0 kms"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblRendimiento">Rendimiento:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblRendieminto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRendimiento" runat="server" Text="0 kms/lts"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblCalculado">Calculado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblCalculado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCalculado" runat="server" CssClass="label_error"  Text="0lts"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblSobrante">Sobra tanque:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblSobrante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSobrante" runat="server"  Text="0lts"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblSobrante">Alcance Kms:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblAlcanceKms" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblAlcanceKms" runat="server"   Text="0kms"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
    
</div>
</div>

