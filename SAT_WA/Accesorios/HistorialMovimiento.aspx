<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistorialMovimiento.aspx.cs" Inherits="SAT.Accesorios.HistorialMovimiento" %>
<%@ Register Src="~/UserControls/wucHistorialMovimiento.ascx" TagName="wucHistorialMovimiento" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucDeposito.ascx" TagName="wucAsignacionDeposito" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucAsignacionDiesel.ascx" TagName="wucAsignacionDiesel" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucDevolucionFaltante.ascx" TagName="wucDevolucionFaltante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucEncabezadoServicio.ascx" TagPrefix="uc1" TagName="wucEncabezadoServicio" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<!-- Estilos de los Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
<link href="../CSS/MenuUsuario.css" rel="stylesheet" />
<!-- Estilos de Validación, DateTimePicker, MasketTextBox -->
<link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.datetimepicker.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.noty.packaged.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.noty.packaged.min.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript" charset="utf-8"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
<script>
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(Loading);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Loaded);

function Loading() {
$.blockUI({ message: '<h2><img src="../Image/loading2.gif" /> Espere por favor...</h2>', fadeIn: 200 });
}
function Loaded() {
$.unblockUI({ fadeOut: 200 });
}

</script>
<div>
<asp:UpdatePanel ID="upucHistorialMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucHistorialMovimiento ID="ucHistorialMovimiento" runat="server" Enabled="true" TabIndex="1" 
OnClickVerReferencia="ucHistorialMovimiento_ClickVerReferencia" OnClickCalcularKMS="ucHistorialMovimiento_ClickCalcularKMS"
OnClickDiesel="ucHistorialMovimiento_ClickDiesel" OnClickDepositos="ucHistorialMovimiento_ClickDepositos"  OnClickEncabezadoServicio="ucHistorialMovimiento_ClickEncabezadoServicio"
OnClickDevolucion="ucHistorialMovimiento_ClickDevolucion" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarDevolucion" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAnticiposR" />
<asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
</Triggers>
</asp:UpdatePanel>
</div>

<!-- Ventana de Referencias -->
<div id="contenedorVentanaReferencias" class="modal">
<div id="ventanaReferencias" class="contenedor_ventana_confirmacion_arriba">
<div class="columna3x">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Referencias" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Modulos.png" />
<h2>Referencias</h2>
</div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciaViaje" runat="server" TabIndex="2" OnClickGuardarReferenciaViaje="ucReferenciaViaje_ClickGuardarReferenciaViaje"
    OnClickEliminarReferenciaViaje="ucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Devolución Faltante -->
<div id="modalDevolucionFaltante" class="modal">
<div id="devolucionFaltante" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDevolucion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDevolucion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Devolucion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucDevolucionFaltante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucDevolucionFaltante ID="wucDevolucionFaltante" runat="server" OnClickGuardarDevolucion="wucDevolucionFaltante_ClickGuardarDevolucion" 
OnClickGuardarDevolucionDetalle="wucDevolucionFaltante_ClickGuardarDevolucionDetalle"
OnClickEliminarDevolucionDetalle="wucDevolucionFaltante_ClickEliminarDevolucionDetalle" 
OnClickAgregarReferenciasDevolucion="wucDevolucionFaltante_ClickAgregarReferenciasDevolucion"
OnClickAgregarReferenciasDetalle="wucDevolucionFaltante_ClickAgregarReferenciasDetalle" Contenedor="#devolucionFaltante" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL QUE PERMITE REALIZAR DEPOSITOS y GENERAR VALES DE DIESEL  -->
<!-- Ventana de Devolución Faltante -->
<div id="modalAnticipos" class="modal">
<div id="anticipos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAnticipos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAnticiposR" runat="server" Text="Anticipos" CommandName="Anticipos" OnClick="lkbCerrarVentanaModal_Click">
<img src="../Image/Cerrar16.png" />
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
<tectos:wucAsignacionDeposito ID="ucDepositos" runat="server" OnClickRegistrar="wucAsigancionDeposito_ClickRegistrar" OnClickEliminar="wucAsigancionDeposito_ClickEliminar"
OnClickCancelar="wucAsigancionDeposito_ClickCancelar" OnClickSolicitar="wucAsigancionDeposito_ClickSolicitar"  />
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
<tectos:wucAsignacionDiesel  ID="ucAsignacionDiesel" runat="server" OnClickGuardarAsignacion="wucAsignacionDiesel1_ClickGuardarAsignacion"
OnClickCancelarAsignacion="wucAsignacionDiesel1_ClickCancelarAsignacion"  OnClickReferenciaAsignacion="wucAsignacionDiesel1_ClickReferenciaAsignacion" 
 OnClickCalculadoDiesel="ucAsignacionDiesel_ClickCalculadoDiesel"    />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoVale" />
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
</Triggers>
</asp:UpdatePanel>                            
</asp:View>
<asp:View ID="VwReporteAnticipos" runat="server">
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
ShowFooter="True" AutoGenerateColumns="False" PageSize=" 5" OnSorting="gvAnticipos_Sorting" OnPageIndexChanging="gvAnticipos_PageIndexChanging">
<Columns>
<asp:TemplateField HeaderText="Folio" SortExpression="Num">
<ItemTemplate>
<asp:LinkButton ID="lkbEditar" runat="server" ToolTip="Edita el Anticipo" Text='<%# Eval("Num") %>'  OnClick="lkbAnticipos_OnClick" CommandName="Editar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Precio" HeaderText="Costo" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Monto" HeaderText="Total" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Asignacion" HeaderText="Asignacion" SortExpression="Asignacion" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización " SortExpression="FechaAutorizacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaCargaoDeposito" HeaderText="Fecha Carga o Depósito" SortExpression="FechaCargaoDeposito" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia Operador/Unidad" SortExpression="Referencia" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="FacturaProveedor" HeaderText="Fac. Proveedor" SortExpression="FacturaProveedor" />
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
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoAnticipos" />
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
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
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
<asp:AsyncPostBackTrigger ControlID="ucDepositos" />   
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoDeposito" Text="Depósitos" OnClick="btnNuevoDeposito_Click" runat="server" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoVale" Text="Vales Diesel" runat="server" OnClick="btnNuevoVale_Click"  CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>

</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoVale" />
<asp:AsyncPostBackTrigger ControlID="btnNuevoDeposito" />
<asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
<asp:AsyncPostBackTrigger ControlID="ucDepositos" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenedorVentanaConfirmacionInformacionCalculado" class="modal">
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
    <!-- VENTANA MODAL DE ACTUALIZACIÓN DE ENCABEZADO DE SERVICIO -->
<div id="encabezadoServicioModal" class="modal">
<div id="encabezadoServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEncabezadoServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEncabezadoServicio" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="EncabezadoServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucReferenciaServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucEncabezadoServicio ID="wucEncabezadoServicio" runat="server" 
OnClickGuardarReferencia="wucEncabezadoServicio_ClickGuardarReferencia" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucHistorialMovimiento" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</form>
</body>
</html>
