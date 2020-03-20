<%@ Page Title="Bitácora Eventos de Servicio" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteBitacoraEventos.aspx.cs" Inherits="SAT.Operacion.ReporteBitacoraEventos" %>
<%@ Register Src="~/UserControls/wucParadaEvento.ascx" TagPrefix="paradaevento" TagName="wucParadaEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!--Biblioteca encabezados Estaticos GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReporteBitacoraEventos();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryReporteBitacoraEventos() {
    $(document).ready(function () {
        //Añadiendo Encabezado Fijo
        $("#<%=gvBitacoraEventos.ClientID%>").gridviewScroll({
            width: 1260,
            height: 450,
            freezesize: 5
        });

//Validación 
var validacionReporteEventos = function () {
var isValidP1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteEventos);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Autocomplete
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=24&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});

}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryReporteBitacoraEventos();
</script>
<div id="encabezado_forma">
<h1>Bitácora Eventos de Servicio</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros de Búsqueda</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control">
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No Servicio</label>
</div>
<div class="control">
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="2"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">Referencia</label>
</div>
<div class="control">
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" TabIndex="3"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEvento">Evento</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="dropdown" TabIndex="4"></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Entre las</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="true" runat="server" CssClass="textbox validate[ custom[dateTime24]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkCita" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkCita" runat="server" UpdateMode="Conditional" >
<ContentTemplate>
<asp:CheckBox  runat="server" ID="chkCita" text="Cita de Llegada Destino" TabIndex="5" AutoPostBack="true" OnCheckedChanged="chkCita_CheckedChanged"/>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Y las</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" Enabled="true" runat="server" CssClass="textbox validate[ custom[dateTime24]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkCita" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta"><label>Estatus Servicio</label></div>
<div class="control">
<asp:CheckBox  runat="server" ID="chkEstatus"  text="Iniciados" TabIndex="8"/>
</div>
<div class="control">
<asp:CheckBox  runat="server" ID="chkEstatusTerminado" text="Terminados" TabIndex="9"/>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">

<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="10" />

</div>
</div>
</div></div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Eventos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewEventos">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewEventos" runat="server" OnSelectedIndexChanged="gvBitacoraEventos_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewEventos">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewEventos" TabIndex="12" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacoraEventos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcelEventos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcelEventos" runat="server" Text="Exportar" TabIndex="13" OnClick="lkbExportarExcelEventos_Onclick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarExcelEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" >
<asp:UpdatePanel ID="upgvBitacoraEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvBitacoraEventos" CssClass="gridview" OnPageIndexChanging="gvBitacoraEventos_OnpageIndexChanging" OnSorting="gvBitacoraEventos_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="14"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="IdServicio" HeaderText="No Servicio" SortExpression="IdServicio" Visible="false"/>
<asp:BoundField DataField="IdMovimiento" HeaderText="IdMovimiento" SortExpression="IdMovimiento" Visible="false"/>
<asp:BoundField DataField="Patio" HeaderText="Patio" SortExpression="Patio" ItemStyle-Width="60px" HeaderStyle-Width="60px"/>
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50px" />
<asp:BoundField DataField="SecuenciaMov" HeaderText="Secuencia Mov." SortExpression="SecuenciaMov" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="152px"/>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" ItemStyle-Width="100px" HeaderStyle-Width="100px"/>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino"  />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="SalidaOrigen" HeaderText="Salida Origen" SortExpression="SalidaOrigen" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="SalidaEnTiempo" HeaderText="Salida Tiempo" SortExpression="SalidaEnTiempo" />
<asp:BoundField DataField="CitaDestino" HeaderText="Cita Destino" SortExpression="CitaDestino" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="LlegadaDestino" HeaderText="Llegada Destino" SortExpression="LlegadaDestino" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="LlegadaEnTiempo" HeaderText="Llegada Tiempo" SortExpression="LlegadaEnTiempo" />
<asp:TemplateField>
   <ItemTemplate>
       <asp:LinkButton runat="server" ID="lnkVerEvento" Text="Editar Eventos" OnClick="lnkVerEvento_Click"></asp:LinkButton>
   </ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="E1_Evento" HeaderText="Evento 1" SortExpression="E1_Evento" />
<asp:BoundField DataField="E1_inicio" HeaderText="Inicio" SortExpression="E1_inicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="E1_Fin" HeaderText="Fin" SortExpression="E1_Fin" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="E1_Incidencia" HeaderText="Incidencia" SortExpression="E1_Incidencia" />
<asp:BoundField DataField="E2_Evento" HeaderText="Evento 2" SortExpression="E2_Evento" />
<asp:BoundField DataField="E2_inicio" HeaderText="Inicio" SortExpression="E2_inicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="E2_Fin" HeaderText="Fin" SortExpression="E2_Fin" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="E2_Incidencia" HeaderText="Incidencia" SortExpression="E2_Incidencia" />
<asp:BoundField DataField="E3_Evento" HeaderText="Evento 3" SortExpression="E3_Evento" />
<asp:BoundField DataField="E3_inicio" HeaderText="Inicio" SortExpression="E3_inicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
<asp:BoundField DataField="E3_Fin" HeaderText="Fin" SortExpression="E3_Fin" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="E3_Incidencia" HeaderText="Incidencia" SortExpression="E3_Incidencia" />
<asp:BoundField DataField="Devolucion" HeaderText="Devolucion" SortExpression="Devolucion" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Rechazo" HeaderText="Rechazo" SortExpression="Rechazo" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Faltante" HeaderText="Faltante" SortExpression="Faltante" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Sobrante" HeaderText="Sobrante" SortExpression="Sobrante" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="ComentariosDev" HeaderText="Comentarios" SortExpression="ComentariosDev" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="ValorUnitario" HeaderText="Valor Unitario" SortExpression="ValorUnitario" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalManiobras" HeaderText="Total Maniobras" SortExpression="TotalManiobras" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="SolicitudManiobras" HeaderText="Solicitud Maniobras" SortExpression="SolicitudManiobras" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy/MM/dd HH:mm}"/>
<asp:BoundField DataField="HrsEstadia" HeaderText="Hrs Estadia" SortExpression="HrsEstadia" ItemStyle-HorizontalAlign="Right"/>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewEventos" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarEventoModal" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!--Ventana modal ParadaEvento-->
<div  id="contenedorEventoModal" class="modal">
<div id="EventoModal" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEventoModal" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lkbCerrarEventoModal" OnClick="lkbCerrarEventoModal_Click" CommandName="EventoModal">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel runat="server" ID="upwucParadaEvento" UpdateMode="Conditional">
<ContentTemplate>
<paradaevento:wucParadaEvento runat="server" ID="wucParadaEvento" OnBtnNuevo_Click="wucParadaEvento_BtnNuevo_Click" OnBtnActualizar_Click="wucParadaEvento_BtnActualizar_Click" OnBtnCancelar_Click="wucParadaEvento_BtnCancelar_Click" OnLkbEliminar_Click="wucParadaEvento_LkbEliminar_Click"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacoraEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Fin Ventana modal ParadaEvento-->
</asp:Content>
