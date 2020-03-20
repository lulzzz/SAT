<%@ Page Title="Despacho Simplificado" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="DespachoSimplificado.aspx.cs" Inherits="SAT.Operacion.DespachoSimplificado" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/wucBitacoraMonitoreoHistorial.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreoHistorial" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreo.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreo" %>
<%@ Register Src="~/UserControls/wucServicioCopia.ascx" TagPrefix="uc1" TagName="wucServicioCopia" %>
<%@ Register Src="~/UserControls/wucMovimientoVacioSinOrden.ascx" TagPrefix="uc1" TagName="wucMovimientoVacioSinOrden" %>
<%@ Register Src="~/UserControls/wucTerminoMovimientoVacio.ascx" TagPrefix="uc1" TagName="wucTerminoMovimientoVacio" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="uc1" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucVencimiento.ascx" TagPrefix="uc1" TagName="wucVencimiento" %>
<%@ Register Src="~/UserControls/wucVencimientoSimplificado.ascx" TagPrefix="uc1" TagName="wucVencimientoSimplificado" %>
<%@ Register Src="~/UserControls/wucAsignacionRecurso.ascx" TagPrefix="uc1" TagName="wucAsignacionRecurso" %>
<%@ Register Src="~/UserControls/wucHistorialMovimiento.ascx" TagPrefix="uc1" TagName="wucHistorialMovimiento" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagPrefix="uc1" TagName="wucKilometraje" %>
<%@ Register Src="~/UserControls/wucParadaEvento.ascx" TagPrefix="uc1" TagName="wucParadaEvento" %>
<%@ Register Src="~/UserControls/wucServicioDocumentacion.ascx" TagPrefix="uc1" TagName="wucServicioDocumentacion" %>
<%@ Register Src="~/UserControls/wucCambioOperador.ascx" TagPrefix="uc1" TagName="wucCambioOperador" %>
<%@ Register Src="~/UserControls/wucImpresionPorte.ascx" TagPrefix="tectos" TagName="wucImpresionPorte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
<script type="text/javascript" src="../Scripts/jquery.elevatezoom.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryDespachoSimp();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryDespachoSimp() {
$(document).ready(function () {
        
//Validación de controles de búsqueda
var validacionBusquedaUnidades = function () {
var isValidP1 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
return isValidP1;
};
//Validación de campos requeridos
$("#<%=this.btnBuscarServicios.ClientID%>").click(validacionBusquedaUnidades);

//Validación de fecha de actualziación (inicio, llegada, salida, término)
var validacionIngresoSalida = function () {
var isValidP1 = !$("#<%=txtFechaActualizacion.ClientID%>").validationEngine('validate');
return isValidP1;
};
//Validación de campos requeridos
$("#<%=this.btnAceptarIngresoSalida.ClientID%>").click(validacionIngresoSalida);

/* Autocompleta origen, destino y cliente */
$("#<%=txtUbicacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
//Cargando Catalogo de Autocompletado
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
/*Selectrores de fecha: Actualización de Llegadas y Salidas, Inicio y Fin de Eventos*/
$("#<%=txtFechaActualizacion.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Controles de Fecha
$("#<%=txtFecIni.ClientID%>").datetimepicker({
    lang: 'es',
    format: 'd/m/Y H:i'
});
$("#<%=txtFecFin.ClientID%>").datetimepicker({
    lang: 'es',
    format: 'd/m/Y H:i'
});

/** Carga de Estatus de Unidad **/
$("#<%=lbxEstatus.ClientID%>").multiselect({
selectedList: 2,
selectall: 1
 });
/** Carga de Flota de Unidad **/
$("#<%=lbxFlota.ClientID%>").multiselect({
selectedList: 2,
selectall: 0
});

});

$(document).keyup(function (e) {
    if (e.keyCode == 27) { // escape key maps to keycode `27`
        //Ocultando Menu
        OcultarMenuDespachoSimplificado();
    }
});
$(document).click(function (e) {
    //Ocultando Menu
    OcultarMenuDespachoSimplificado();
});

}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryDespachoSimp();

//Función encargada de Mostrar el Ménu
function MostrarMenuDespachoSimplificado(control, e) {
    //Ocultando en caso de estar Abierto
    OcultarMenuDespachoSimplificado();

    //Obteniendo Coordenadas de las Forma
    var posx = e.pageX + 'px';
    var posy = e.pageY + 'px';

    //Si el Evento es de Tipo Click
    if (e.type == 'click')
        //Detener Propagación del Evento
        e.stopPropagation();

    //Asignando Posiciones al Documento
    document.getElementById(control).style.position = 'absolute';
    document.getElementById(control).style.left = posx;
    document.getElementById(control).style.top = posy;

    //Ejecutando 
    $(document).ready(function (evt) {
        //Mostrando DIV
        $('#' + control).slideDown(100);
    });
}
//Función encargada de Ocultar el Ménu
function OcultarMenuDespachoSimplificado() {
    $(document).ready(function () {

        //Ocultando DIV
        $('.menuDespachoSimp').slideUp(100);
    });
}

</script>
<div id="encabezado_forma">
<img src="../Image/OperacionPatio.png" />
<h1>Despacho de Unidades</h1>
</div>
<nav id="menuForma">
<ul>
<li class="yellow">
<a href="#" class="fa fa-flag-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbMenuMovimientosVacio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbMenuMovimientosVacio" runat="server" Text="Movimientos Vacío" OnClick="lkbElementoMenu_Click" CommandName="MovimientosVacio" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbMenuVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbMenuVencimientos" runat="server" Text="Vencimientos" OnClick="lkbElementoMenu_Click" CommandName="Vencimientos" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbCambioOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCambioOperador" runat="server" Text="Cambio Operador" OnClick="lkbElementoMenu_Click" CommandName="CambioOperador" />
</ContentTemplate>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="gray">
<a href="#" class="fa fa-book "></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbVerServiciosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerServiciosPendientes" runat="server" Text="Servicios Pendientes" OnClick="lkbElementoMenu_Click" CommandName="ServiciosPendientes" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbVerServiciosPendientes" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbHistorialServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbHistorialServicios" runat="server" Text="Historial Viajes" OnClick="lkbElementoMenu_Click" CommandName="HistorialViajes" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbHistorialServicios" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbImpresionDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImpresionDocumentos" runat="server" Text="Impresión Documentos" OnClick="lkbElementoMenu_Click" CommandName="ImpresionDocumentos" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbImpresionDocumentos" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
</ul>
</nav>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Unidades</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="label_negrita">Tipo de Unidades</label>
</div>
</div>
<div class="renglon">
<div class="control">
<asp:CheckBox ID="chkUnidadesPropias" runat="server" Checked="true" Text="Unidades Propias" TabIndex="1"/>
</div>
<div class="control">
<asp:CheckBox ID="chkUnidadesNoPropias" runat="server"  Text="Unidades No Propias" TabIndex="2"/>
</div>
</div>
<div class="renglon">
<div class="control">
<asp:UpdatePanel ID="uprdbUnidadMotriz" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbUnidadMotriz" runat="server" GroupName="TipoUnidad" Text="Unidades Motrices" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbUnidad_CheckedChanged" TabIndex="3"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbUnidadArrastre" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbUnidadArrastre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbUnidadArrastre" runat="server" GroupName="TipoUnidad" Text="Unidades de Arrastre" AutoPostBack="true" OnCheckedChanged="rdbUnidad_CheckedChanged" TabIndex="4"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbUnidadMotriz" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtNoUnidad">
No. Unidad
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoUnidad" runat="server" CssClass="textbox" MaxLength="30" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxEstatus" SelectionMode="multiple" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacion">Ubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxFlota">Flota</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxFlota" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxFlota" SelectionMode="multiple" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarServicios" runat="server" CssClass="boton" Text="Buscar" TabIndex="9" OnClick="btnBuscarServicios_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorServicio" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
<asp:AsyncPostBackTrigger ControlID="rdbUnidadArrastre" />
<asp:AsyncPostBackTrigger ControlID="rdbUnidadMotriz" />
<asp:AsyncPostBackTrigger ControlID="wucKilometraje" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<div class="header_seccion">
<img src="../Image/EnTransito.png" />
<h2>Listado de Unidades</h2>
</div>
<div class="renglon4x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoUnidades">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoUnidades"></label>
<asp:DropDownList ID="ddlTamanoUnidades" runat="server" OnSelectedIndexChanged="ddlTamanoUnidades_SelectedIndexChanged" TabIndex="9" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarUnidades">Ordenado Por:</label>
</div>
<div class="etiqueta_155px"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarUnidades" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnValidarGPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnValidarGPS" runat="server" Text="Validar GPS" OnClick="btnValidarGPS_Click" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80pxr">
<asp:LinkButton ID="lkbExportarUnidades" runat="server" Text="Exportar Excel" OnClick="lkbExportarUnidades_Click" TabIndex="10"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorUnidades" oncontextmenu="return false">
<asp:UpdatePanel ID="upgvUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUnidades" OnPageIndexChanging="gvUnidades_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvUnidades_RowDataBound" OnSorting="gvUnidades_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="11"
ShowHeaderWhenEmpty="True" PageSize="250" AllowSorting="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField HeaderText="" SortExpression="Operador">
    <ItemTemplate>
        <asp:ImageButton runat="server" ID="imbOperador" ImageUrl="" OnClick="imbOperador_Click" />
    </ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Evaluación Monitoreo">
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:ImageButton ID="imbEvaluacion" runat="server" ImageUrl="~/Image/semaforo_verde.png" 
    Width="16px" Height="16px" OnClick="imbEvaluacion_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="No. Unidad" SortExpression="NoUnidad" >
<HeaderStyle Width="65px" />
<ItemStyle Width="65px" />
<ItemTemplate>
<asp:LinkButton ID="lkbNoUnidad" runat="server" Text='<%#Eval("NoUnidad") %>' OnClick="lkbAccionUnidad_Click" CommandName="NoUnidad" ToolTip="Ver Historial de la Unidad"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="TipoUnidad" HeaderText="Tipo Unidad" SortExpression="TipoUnidad" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
<asp:TemplateField HeaderText="Estatus" SortExpression="EstatusUnidad">
<HeaderStyle Width="80px" />
<ItemStyle Width="80px" />
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusVencimiento" runat="server" Text='<%#Eval("EstatusUnidad") %>' OnClick="lkbAccionUnidad_Click" CommandName="EstatusVencimiento" ToolTip="Consultar Vencimientos Activos"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Tiempo" DataField="Tiempo" SortExpression="*Tiempo" >
<ItemStyle HorizontalAlign="Right" />
<HeaderStyle Width="55px" />
<ItemStyle Width="55px" />
</asp:BoundField>
<asp:BoundField HeaderText="Ubicación Actual" DataField="UbicacionActual" SortExpression="UbicacionActual" ItemStyle-Width="170px" HeaderStyle-Width="170px" />
<asp:TemplateField HeaderText="Último Monitoreo" SortExpression="UltimoMonitoreo">
<HeaderStyle Width="70px" />
<ItemStyle Width="70px" />
<ItemTemplate>
<asp:LinkButton ID="lkbUltimoMonitoreo" runat="server" Text='<%#Eval("UltimoMonitoreo", "{0:dd/MM/yyyy HH:mm}") %>' CommandName="UltimoMonitoreo"
ToolTip="Ver Bitácora de Monitoreo" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="No. Serv. / Movto." SortExpression="NoServicioMov">
<HeaderStyle Width="80px" />
<ItemStyle Width="80px" />
<ItemTemplate>
<asp:LinkButton ID="lkbNuevoServMov" runat="server" Text='<%#Eval("NoServicioMov") %>' OnClick="lkbAccionUnidad_Click" CommandName ="Nuevo" />
<asp:Label ID="lblNoServMov" runat="server" Text='<%#Eval("NoServicioMov") %>' />
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Kms" SortExpression="Kms">
<HeaderStyle Width="40px" />
<ItemStyle Width="40px" />
<ItemTemplate>
<asp:LinkButton ID="lkbKms" runat="server" OnClick="lkbAccionUnidad_Click" CommandName="Kms" Text='<%#Eval("Kms") %>' ToolTip="Calcular o Registrar Kilometraje del Movimiento"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="HoraEstimada" HeaderText="Hora Estimada" SortExpression="HoraEstimada" Visible="false" />
<asp:TemplateField HeaderText="Hora Estimada" SortExpression="*HoraEstimada">
<ItemStyle HorizontalAlign="Right" />
<HeaderStyle Width="100px" />
<ItemStyle Width="100px" />
<ItemTemplate>
<asp:Image ID="imbHoraLlegada" runat="server" ImageUrl="" 
    Width="16px" Height="16px" />
<asp:LinkButton ID="lkbHoraEstimada" runat="server" Text='<%# Eval("HoraEstimada") %>' CommandName="Evaluacion" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="ProximaCita" HeaderText="Próxima Cita" SortExpression="ProximaCita" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField HeaderText="Estatus Serv. / Mvto." DataField="EstatusServMov" SortExpression="EstatusServMov">
<HeaderStyle Width="100px" />
<ItemStyle Width="100px" />
</asp:BoundField>
<asp:TemplateField HeaderText="Unidad Asignada" SortExpression="UnidadAsignada">
<HeaderStyle Width="65px" />
<ItemStyle Width="65px" />
<ItemTemplate>
<asp:LinkButton ID="lkbRem1" runat="server" Text='<%#Eval("UnidadAsignada") %>' CommandName="Remolque" ToolTip="Ver Recursos Asignados" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Estatus U. Asiganda" SortExpression="EstatusUAsignada">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusRem1" runat="server" Text='<%#Eval("EstatusUAsignada") %>' CommandName="Eventos" ToolTip="Ver Eventos de la Parada" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Tiempo Estatus." DataField="TiempoUAsignada" SortExpression="*TiempoUAsignada" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Carta Porte" SortExpression="Porte">
<ItemTemplate>
<asp:LinkButton ID="lkbPorteCliente" runat="server" Text='<%#Eval("Porte") %>' ToolTip="Ver Todos los Eventos de la Parada" CommandName="Porte" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencias" SortExpression="Referencia">
<ItemTemplate>
<asp:LinkButton ID="lkbReferenciaCliente" runat="server" Text='<%#Eval("Referencia") %>' ToolTip="Ver y Añadir Referencias de Servicio" CommandName="Referencia" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Act. Parada" SortExpression="ActParada">
<ItemTemplate>
<asp:LinkButton ID="lkbSalidaLLegada" runat="server" Text="Salida/Llegada" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Act. Servicio" SortExpression="ActServicio">
<ItemTemplate>
<asp:LinkButton ID="lkbIniciarTerminar" runat="server" Text="Iniciar/Terminar" OnClick="lkbAccionUnidad_Click" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<div id="menuDespachoSimp" runat="server">
<img src="../Image/menu_context2.png" />
</div>
<div id="menuDespachoSimpOpciones" runat="server" class="MenuContext menuDespachoSimp" style="display:none;">
<div class="ContextItem">
<asp:LinkButton ID="lkbValidar" runat="server" CommandName="Validar" Text="Validar" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
</div>
<div class="ContextItem">
<asp:LinkButton ID="lkbImprimirPorte" runat="server" CommandName="ImprimirPorte" Text="Imprimir Porte" OnClick="lkbAccionUnidad_Click" />
</div>
<div class="ContextItem">
<asp:LinkButton ID="lkbBitocoraViaje" runat="server" CommandName="BitacoraViaje" Text="Bitacora Viaje" OnClick="lkbAccionUnidad_Click" />
</div>
<div class="ContextItem">
<asp:LinkButton ID="lkbImprimirPorteViajera" runat="server" CommandName="ImprimirPorteViajera" Text="Imprimir Porte Viajera" OnClick="lkbAccionUnidad_Click" />
</div>
</div>
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
<asp:AsyncPostBackTrigger ControlID="rdbUnidadArrastre" />
<asp:AsyncPostBackTrigger ControlID="rdbUnidadMotriz" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoUnidades" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreo" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarMovimientosVacio" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarReferencias" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarHistorialVencimientos" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarVencimientoSeleccionado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
<asp:AsyncPostBackTrigger ControlID="wucParadaEvento" />
<asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
<asp:AsyncPostBackTrigger ControlID="wucAsignacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="wucServicioDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosPendientes" />
<asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
<asp:AsyncPostBackTrigger ControlID="wucVencimientoSimp" />
<asp:AsyncPostBackTrigger ControlID="btnValidarGPS" />
</Triggers>
</asp:UpdatePanel>
</div>


<!-- VENTANA MODAL DE HISTORIAL DE BITÁCORA DE MONITOREO -->
<div id="historialBitacoraModal" class="modal">
<div id="historialBitacora" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialBitacora" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialBitacora" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="HistorialBitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div>
<asp:UpdatePanel ID="upwucBitacoraMonitoreoHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucBitacoraMonitoreoHistorial runat="server" ID="wucBitacoraMonitoreoHistorial" OnbtnNuevoBitacora="wucBitacoraMonitoreoHistorial_btnNuevoBitacora" OnlkbConsultar="wucBitacoraMonitoreoHistorial_lkbConsultar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreo" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>
<!-- VENTANA MODAL DE EDICIÓN Y CAPTURA DE BITÁCORA DE MONITOREO -->
<div id="bitacoraMonitoreoModal" class="modal">
<div id="bitacoraMonitoreo" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarBitacoraMonitoreo" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarBitacoraMonitoreo" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Bitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="upwucBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucBitacoraMonitoreo runat="server" ID="wucBitacoraMonitoreo" OnClickRegistrar="wucBitacoraMonitoreo_ClickRegistrar" 
    OnClickEliminar="wucBitacoraMonitoreo_ClickEliminar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreoHistorial" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>
<!-- VENTANA MODAL DE SELECCIÓN DE NUEVO SERVICIO O REPOSICIONAMIENTO -->
<div id="seleccionServicioMovimientoModal" class="modal">
<div id="seleccionServicioMovimiento" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSeleccionServicioMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSeleccionServicioMovimiento" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="SeleccionServMov" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Entrada.png" alt="SeleccionTarea" />
<h3>Nuevo Movimiento</h3>
</div>
<div class="columna400px">
<div class="renglon">
<label class="mensaje_modal">Seleccione el tipo de movimiento para esta unidad</label>
</div>
<div class="renglon"></div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnNuevoServicio" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="NuevoServicio" Text="Crear Servicio" />
</div>
<div class="controlBoton">
<asp:Button ID="btnServicioPendiente" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="ServicioPendiente" Text="Serv. Pendiente" />
</div>
<div class="controlBoton">    
<asp:Button ID="btnNuevaCopiaServicio" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="CopiaServicio" Text="Copiar Servicio" />
</div>
<div class="controlBoton">
<asp:Button ID="btnNuevoMovimiento" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="Movimiento" Text ="Reposicionar" />
</div>
<div class="renglon"></div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE COPIA DE SERVICIOS MAESTROS -->
<div id="copiaServicioMaestroModal" class="modal">
<div id="copiaServicioMaestro" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCopiaServicioMaestro" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCopiaServicioMaestro" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="CopiaServicio">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucServicioCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucServicioCopia runat="server" id="wucServicioCopia" Contenedor="#copiaServicioMaestro"
    OnClickGuardarServicioCopia="wucServicioCopia_ClickGuardarServicioCopia" 
    OnClickCancelarServicioCopia="wucServicioCopia_ClickCancelarServicioCopia" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevaCopiaServicio" />
</Triggers>
</asp:UpdatePanel>    
</div>
</div>
<!-- VENTANA MODAL DE NUEVO MOVIMIENTO EN VACÍO -->
<div id="reubicacionUnidadModal" class="modal">
<div id="reubicacionUnidad" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReubicacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReubicacion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ReubicacionUnidad" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EntradasSalidas.png" />
<h2>Reubicar Unidad</h2>
</div> 
<div class="columna2x">
<asp:UpdatePanel ID="upwucReubicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucMovimientoVacioSinOrden ID="wucReubicacion" runat="server" OnClickRegistrar="wucReubicacion_ClickRegistrar" 
    OnClickCancelar="wucReubicacion_ClickCancelar" Contenedor="#reubicacionUnidad" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevoMovimiento" />
<asp:AsyncPostBackTrigger ControlID="wucAsignacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DE MOVS. EN VACÍO -->
<div id="movimientosVacioModal" class="modal">
<div id="movimientosVacio" class="contenedor_modal_seccion_completa_arriba"  style="height:635px;top:10px;width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTerminoMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarMovimientosVacio" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="MovimientosVacio">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upwucTerminoMovimientoVacio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucTerminoMovimientoVacio ID="wucTerminoMovimientoVacio" runat="server" Contenedor="#movimientosVacio" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbMenuMovimientosVacio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DE ASIGNACIÓN DE RECURSOS -->
<div id="asignacionRecursosModal" class="modal">
<div id="asignacionRecursos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAsignacionRecursos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAsignacionRecursos" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="AsignacionRecursos">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucAsignacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucAsignacionRecurso runat="server" ID="wucAsignacionRecurso" OnClickAgregarRecurso="wucAsignacionRecurso_ClickAgregarRecurso" OnClickQuitarRecurso="wucAsignacionRecurso_ClickQuitarRecurso" 
OnClickReubicarRecurso="wucAsignacionRecurso_ClickReubicarRecurso" OnClickLiberarRecurso="wucAsignacionRecurso_ClickLiberarRecurso" Contenedor="#asignacionRecursos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE SELECCION DE FECHA PARA INICIO Y FIN DE MOVIMIENTO, ASÍ COMO SALIDA Y LLEGADA A PARADA-->
<div id="inicioFinMovimientoServicioModal" class="modal">
<div id="inicioFinMovimientoServicio" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Aceptar.png" />
<asp:UpdatePanel ID="uplblEncabezadoActualizacionServMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2 id="lblEncabezadoActualizacionServMov" runat="server"></h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<asp:UpdatePanel ID="upRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<div id="divOperador" runat="server" class="renglon2x">
<div class="etiqueta_50px">
<label for="lblOp">Operador</label>
</div>
<div class="etiqueta_320px">
<asp:Label ID="lblOp" runat="server" Text="----" CssClass="label_correcto"></asp:Label>
</div>
</div>
<div id="divTercero" runat="server" class="renglon2x">
<div class="etiqueta_50px">
<label for="lblPr">Proveedor</label>
</div>
<div class="etiqueta_320px">
<asp:Label ID="lblPr" runat="server" Text="----" CssClass="label_negrita"></asp:Label>
</div>
</div>
<div id="divUnidades" runat="server" style="height: 55px; width:475px;">
<div class="renglon2x">
<div class="etiqueta" style="width:120px;">
<label for="lblU1">Unidad 1</label>
</div>
<div class="etiqueta" style="width:120px;">
<label for="lblU2">Unidad 2</label>
</div>
<div class="etiqueta" style="width:120px;">
<label for="lblU3">Unidad 3</label>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta" style="width:120px;">
<asp:Label ID="lblU1" runat="server" Text="----" CssClass="label_error"></asp:Label>
</div>
<div class="etiqueta" style="width:120px;">
<asp:Label ID="lblU2" runat="server" Text="----" CssClass="label_error"></asp:Label>
</div>
<div class="etiqueta" style="width:120px;">
<asp:Label ID="lblU3" runat="server" Text="----" CssClass="label_error"></asp:Label>
</div>
</div>
<br />
</div>
<div style="width:471px; border-bottom: 1px solid #DDD;"></div>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblValorFechaSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblValorFechaSalida" runat="server" Text="Fecha salida" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblFechaSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaSalida" runat="server" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblTipoFechaCita" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTipoFechaCita" runat="server" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblFechaCita" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaCita" runat="server" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblFechaActualizacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaActualizacion" runat="server" Text="Fecha" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaActualizacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaActualizacion" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblRazonRetraso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRazonLlegadaTarde" runat="server" CssClass="label">Razón Tarde</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlRazonLlegadaTarde" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlRazonLlegadaTarde" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSalidaETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarIngresoSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarIngresoSalida" runat="server" OnClick="btnIngresoSalida_Click" CommandName="Cancelar" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarIngresoSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarIngresoSalida" runat="server" OnClick="btnIngresoSalida_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE NOTIFICACIÓN DE VENCIMIENTOS ACTIVOS -->
<div id="indicadorVencimientosModal" class="modal">
<div id="indicadorVencimientos" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarNotificacionVencimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarNotificacionVencimientos" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="IndicadorVencimientos" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<asp:UpdatePanel ID="upimgAlertaVencimiento" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:Image ID="imgAlertaVencimiento" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<h3>¡Existen Vencimientos Activos!</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeHistorialVencimientos" runat="server" CssClass="mensaje_modal"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplkbVerHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerHistorialVencimientos" runat="server" Text="Mostrar Vencimientos Activos"
OnClick="lkbVerHistorialVencimientos_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="wucAsignacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarIndicadorVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarIndicadorVencimientos" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarIndicadorVencimientos_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE VISUALIZACIÓN DE HISTORIAL DE VENCIMIENTOS -->
<div id="historialVencimientosModal" class="modal">
<div id="historialVencimientos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialVencimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialVencimientos" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="HistorialVencimientos" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialVencimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimientosHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucVencimientosHistorial runat="server" id="wucVencimientosHistorial" OnlkbConsultar="wucVencimientosHistorial_lkbConsultar" 
    OnlkbTerminar="wucVencimientosHistorial_lkbTerminar" Contenedor="#historialVencimientos"
    OnbtnNuevoVencimiento="wucVencimientosHistorial_btnNuevoVencimiento" />            
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialVencimientos" />
<asp:AsyncPostBackTrigger ControlID="wucVencimiento" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="lkbMenuVencimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE VENCIMIENTOS -->
<div id="actualizacionVencimientoModal" class="modal">
<div id="actualizacionVencimiento" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVencimientoSeleccionado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVencimientoSeleccionado" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ActualizacionVencimiento" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucVencimiento runat="server" id="wucVencimiento" OnClickGuardarVencimiento="wucVencimiento_ClickGuardarVencimiento" 
    OnClickTerminarVencimiento="wucVencimiento_ClickTerminarVencimiento" Contenedor="#actualizacionVencimiento" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucVencimientosHistorial" />
<asp:AsyncPostBackTrigger ControlID="lkbMenuVencimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE ACTUALIZACIÓN DE VENCIMIENTOS -->
<div id="actualizacionVencimientoSimplificadoModal" class="modal">
<div id="actualizacionVencimientoSimplificado" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVencimientoSimp" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVencimientoSimp" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ActualizacionVencimientoSimp" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimientoSimp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucVencimientoSimplificado ID="wucVencimientoSimp" runat="server" 
OnClickGuardarVencimiento="wucVencimientoSimp_ClickGuardarVencimiento"
OnClickTerminarVencimiento="wucVencimientoSimp_ClickTerminarVencimiento" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE NOTIFICACIÓN DE EVENTOS PENDIENTES EN PARADA -->
<div id="confirmacionEventosPendientesModal" class="modal">
<div id="confirmacionEventosPendientes" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbconfirmacionEventosPendientes" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbconfirmacionEventosPendientes" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ConfirmacionEventosPendientes" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Aceptar.png" />
<h2>Actualización Eventos Pendientes</h2>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal" >Existen eventos sin actualizar. ¿Desea actualizar dichos eventos?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNoActualizacionEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNoActualizacionEventos" runat="server" OnClick="btnEvento_Click" CommandName="NoActualizar" CssClass="boton_cancelar" Text="No" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoSalida" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnSiActualizacionEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSiActualizacionEventos" runat="server" OnClick="btnEvento_Click" CommandName="Actualizar" CssClass="boton" Text="Si" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE EVENTOS DE PARADA ACTUAL -->
<div id="eventosParadaModal" class="modal">
<div id="eventosParada" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEventosParada" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEventosParada" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="EventosParada">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnSiActualizacionEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucParadaEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucParadaEvento runat="server" id="wucParadaEvento" OnBtnActualizar_Click="wucParadaEvento_OnBtnActualizarClick" 
OnBtnCancelar_Click="wucParadaEvento_OnBtnCancelarClick" OnBtnNuevo_Click="wucParadaEvento_OnBtnNuevoClick" 
OnLkbEliminar_Click="wucParadaEvento_OnlkbEliminarClick" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE REFERENCIAS DE SERVICIO -->
<div id="referenciasServicioModal" class="modal">
<div id="referenciasServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ReferenciasServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Referencias Servicio</h2>
</div> 
<div class="columna2x">
<asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true"
OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DE KILOMETRAJE -->
<div id="kilometrajeMovimientoModal" class="modal">
<div id="kilometrajeMovimiento" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarKilometrajeMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarKilometrajeMovimiento" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="KilometrajeMovimiento" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucKilometraje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucKilometraje runat="server" ID="wucKilometraje" OnClickGuardar="wucKilometraje_ClickGuardar" Contenedor="#kilometrajeMovimiento" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DOCUMENTACIÓN DE SERVICIO -->
<div id="documentacionServicioModal" class="modal">
<div id="documentacionServicio" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDocumentacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDocumentacion" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="Documentacion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucServicioDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucServicioDocumentacion runat="server" ID="wucServicioDocumentacion" OnImbAgregarParada_Click="wucServicioDocumentacion_ImbAgregarParada_Click" OnLkbEliminarParada_Click="wucServicioDocumentacion_LkbEliminarParada_Click" 
OnImbAgregarProducto_Click="wucServicioDocumentacion_ImbAgregarProducto_Click" OnLkbEliminarProducto_Click="wucServicioDocumentacion_LkbEliminarProducto_Click" 
OnBtnAceptarEncabezado_Click="wucServicioDocumentacion_BtnAceptarEncabezado_Click" OnLkbCitasEventos_Click="wucServicioDocumentacion_LkbCitasEventos_Click" Contenedor="#documentacionServicio" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevoServicio" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL ASIGNACIÓN DE SERVICIO DOCUMENTADO -->
<div id="asignacionServicioDocumentadoModal" class="modal">
<div id="asignacionServicioDocumentado" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAsignacionServicioDocumentado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAsignacionServicioDocumentado" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="ServicioPendiente" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Servicios Pendientes</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_155px">
<label for="lblOrdenadoPorServiciosPendientes">Ordenado Por:</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblOrdenadoPorServiciosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoPorServiciosPendientes" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbExportarServiciosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServiciosPendientes" runat="server" Text="Exportar Excel" OnClick="lkbExportarServiciosPendientes_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServiciosPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvServiciosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosPendientes" OnPageIndexChanging="gvServiciosPendientes_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvServiciosPendientes_RowDataBound" 
runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True"
ShowHeaderWhenEmpty="True" PageSize="250" CssClass="gridview" Width="100%" OnSorting="gvServiciosPendientes_Sorting">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Documentacion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Documentación" SortExpression="Documentacion">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Confirmacion" HeaderText="Confirmación" SortExpression="Confirmacion">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CartaPorte" HeaderText="Carta Porte" SortExpression="CartaPorte">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField SortExpression="SemaforoCarga">
<ItemTemplate>
<asp:Image ID="imgSemaforoCarga" runat="server" ImageUrl="~/Image/semaforo_verde.png" Height="20" Width="20" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Carga" SortExpression="CitaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField SortExpression="SemaforoDescarga">
<ItemTemplate>
<asp:Image ID="imgSemaforoDescarga" runat="server" ImageUrl="~/Image/semaforo_verde.png" Height="20" Width="20" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Carga" SortExpression="FechaLlegadaCarga">
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaCarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaCarga" />
<asp:BoundField DataField="FechaSalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Salida Carga" SortExpression="FechaSalidaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Descarga" SortExpression="FechaLlegadaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaDescarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaDescarga" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista">
</asp:BoundField>
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Referencias" SortExpression="Referencias">
<ItemTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" CommandName="Referencia" OnClick="lkbAccionServicioPendiente_Click" Text='<%#Eval("Referencias") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField >
<ItemTemplate>
<asp:LinkButton ID="lkbAsignarUnidad" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicioPendiente_Click" Text="Asignar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnServicioPendiente" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL CAMBIO OPERADOR-->
<div id="modalCambioOperador" class="modal">
<div id="confirmacionCambioOperador" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCambioOperador" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCambioOperador" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="CambioOperador" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucCambioOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucCambioOperador ID="wucCambioOperador" runat="server" OnClickRegistrar="wucCambioOperador_ClickRegistrar" Contenedor="#confirmacionCambioOperador" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCambioOperador" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana de Selección de Proveedor GPS -->
<div id="contenedorVentanaProveedorGPS" class="modal">
<div id="ventanaProveedorGPS" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarProveedorGPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarProveedorGPS" runat="server" CommandName="ProveedorGPS" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Seleccione la Antena de su Proveedor de GPS</h2>
</div>
<div class="renglon2x">
    <div class="control2x">
        <asp:UpdatePanel ID="upddlServicioGPS" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlServicioGPS" runat="server" CssClass="dropdown2x"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="controlBoton">
        <asp:UpdatePanel ID="upbtnSeleccionar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionar_Click" CssClass="boton" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
</div>
</div>
</div>

<!-- Ventana Modal de Información de la Evaluación (ETA) -->
<div id="contenedorVentanaETA" class="modal">
<div id="ventanaETA" class="contenedor_ventana_confirmacion">
<div class="columna3x">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplknCerrarETA" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lknCerrarETA" runat="server" CommandName="VentanaETA" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Tiempo Estimado de Llegada</h2>
</div>
<div class="renglon3x" style="float:left;">
    <div class="etiqueta_155px">
        <label>Destino</label>
    </div>
    <div class="etiqueta_320px">
        <asp:UpdatePanel ID="uplblDestino" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblDestino" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="renglon3x" style="float:left;">
    <div class="etiqueta_155px">
        <label>Distancia Restante</label>
    </div>
    <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblDistancia" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblDistancia" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="renglon3x" style="float:left;">
    <div class="etiqueta_155px">
        <label>Tiempo Faltante</label>
    </div>
    <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblTiempo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblTiempo" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="renglon3x" style="float:left;">
    <div class="etiqueta_155px">
        <label>Hora de Llegada</label>
    </div>
    <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblHoraLlegada" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblHoraLlegada" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="renglon2x" style="float:left;">
    <div class="etiqueta_155px">
        <label>Cita Destino</label>
    </div>
    <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblCita" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblCita" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
</div>
</div>
</div>

<!-- Ventana de Resultado de la Evaluación (ETA) -->
<div id="contenedorVentanaResultadoValidacion" class="modal">
<div id="ventanaResultadoValidacion" class="contenedor_ventana_confirmacion_arriba" style="width:640px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarResultadoETA" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarResultadoETA" runat="server" CommandName="ResultadoETA" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upmtEvaluacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvEvaluacion" runat="server" ActiveViewIndex="0">
<asp:View ID="vwResultado" runat="server">
<div class="header_seccion">
<asp:UpdatePanel ID="upimgExcepcionResult" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<img id="imgResultadoETA" runat="server" src="../Image/Exclamacion.png" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
<h3>
<asp:UpdatePanel ID="uplblResultadoETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblResultadoETA1" runat="server"></asp:Label>
<br />
<asp:Label ID="lblResultadoETA2" runat="server"></asp:Label>
<br />
<asp:Label ID="lblResultadoETA3" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
    <h3></h3>
    <h3></h3>
    <h3></h3>
    <h3></h3>
</h3>
</div>
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta_50px">
<label>Velocidad:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblVelocidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblVelocidad" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnSalidaETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSalidaETA" runat="server" Text="Aceptar" OnClick="btnSalidaETA_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label>Dirección:</label>
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblDireccion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDireccion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80pxr">
<asp:UpdatePanel ID="uplkbVerHistorialETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerHistorialETA" runat="server" Text="Ver Historial" CommandName="Historial" OnClick="lkbResultadoETA_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upimgMapaResultado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<img id="imgMapaResultado" runat="server" src="~/Image/noDisponible.jpg" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwHistorial" runat="server">
<div class="header_seccion">
<h2>Historial de Evaluaciones</h2>
</div>
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta">
<asp:UpdatePanel ID="uprbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbBitacora" runat="server" Text="Bitacora" ValidationGroup="ETA" Checked="true" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uprbEvaluacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbEvaluacion" runat="server" Text="Evaluación" ValidationGroup="ETA" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_100pxr">
<asp:UpdatePanel ID="uplkbResultadoETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbResultadoETA" runat="server" Text="Ultima Respuesta" CommandName="Resultado" OnClick="lkbResultadoETA_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label>Desde</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox_100px validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Hasta</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox_100px validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialETA" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upchkIncluirETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluirETA" runat="server" Text="¿Incluir?" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarETA" runat="server" CssClass="boton" OnClick="btnBuscarETA_Click" Text="Buscar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="header_seccion">
<h2>Resultados Obtenidos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label>Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoETA" runat="server" OnSelectedIndexChanged="ddlTamanoETA_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoETA" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarETA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarETA" runat="server" Text="Exportar" OnClick="lkbExportarETA_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvEvaluaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEvaluaciones" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
OnPageIndexChanging="gvEvaluaciones_PageIndexChanging" OnSorting="gvEvaluaciones_Sorting"
PageSize="250" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
<asp:BoundField DataField="Resultado" HeaderText="Resultado" SortExpression="Resultado" />
<asp:BoundField DataField="FechaEvaluacion" HeaderText="Fecha Evaluación" SortExpression="FechaEvaluacion" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoGPSExcedido" HeaderText="Tiempo GPS Excedido" SortExpression="TiempoGPSExcedido" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Distancia" HeaderText="Distancia" SortExpression="Distancia" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="HoraLlegada" HeaderText="Hora de Llegada" SortExpression="HoraLlegada" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CitaDestino" HeaderText="Cita Destino" SortExpression="CitaDestino" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
    <asp:LinkButton ID="lkbConsultar" runat="server" Text="Consultar" OnClick="lkbConsultar_Click" CommandName="Resultado"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoETA" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialETA" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEvaluaciones" />
<asp:AsyncPostBackTrigger ControlID="lkbVerHistorialETA" />
<asp:AsyncPostBackTrigger ControlID="lkbResultadoETA" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana de Impresión -->
<div id="contenedorVentanaImpresionPorte" class="modal">
<div id="ventanaImpresionPorte" class="contenedor_ventana_confirmacion" style="width:auto;">
<div class="columna2x">
<asp:UpdatePanel ID="upwucImpresionPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucImpresionPorte ID="wucImpresionPorte" runat="server" OnClickImprimirCartaPorte="wucImpresionPorte_ClickImprimirCartaPorte" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

</asp:Content>
