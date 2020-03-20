<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteEventos.aspx.cs" Inherits="SAT.Operacion.ReporteEventos" %>

<%@ Register Src="~/UserControls/wucParadaEvento.ascx" TagPrefix="tectos" TagName="wucParadaEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReporteEventos();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryReporteEventos() {
$(document).ready(function () {

//Validación 
var validacionReporteEventos = function () {
var isValidP1 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
return isValidP1 && isValidP2 && isValidP3
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteEventos);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Autocomplete
$("#<%=txtUbicacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});

    //Añadiendo Encabezado Fijo
    $("#<%=gvEventos.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorReporteEventos").offsetWidth - 15,
        height: 400,

    });

}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryReporteEventos();
</script>
<div id="encabezado_forma">
<h1>Reporte Eventos</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar Eventos </h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
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
<asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEvento">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFecha">Fecha </label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecha" Enabled="true" runat="server" CssClass="textbox validate[ custom[dateTime24]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" Enabled="true" runat="server" CssClass="textbox2x validate[ custom[dateTime24]]" TabIndex="8" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="9" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div></div>
<div class="contenido_resumen_visor">
<div class="header_seccion">
<img src="../Image/ResumenReporte.png" />
<h2>Resumen por evento</h2>
</div>
<div class="grafica_resumen_visor">
<asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="ChtEventos" runat="server" TabIndex="10" BackColor="Transparent">
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
</asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>

<div class="grid_resumen_visor">
<asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumen" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
TabIndex="10" ShowFooter="True" CssClass="gridview"
PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Descripcion" HeaderText="Evento" SortExpression="Descripcion" />
<asp:BoundField DataField="Estancia" HeaderText="Estancia" SortExpression="*Estancia" />
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
</Triggers>
</asp:UpdatePanel>
</div>
</div>
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
<asp:DropDownList ID="ddlTamañoGridViewEventos" runat="server" OnSelectedIndexChanged="gvEventos_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
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
<asp:AsyncPostBackTrigger ControlID="gvEventos" EventName="Sorting" />
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
<div class="grid_seccion_completa_altura_variable" id="contenedorReporteEventos">
<asp:UpdatePanel ID="upgvEventos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEventos" CssClass="gridview" OnPageIndexChanging="gvEventos_OnpageIndexChanging" OnSorting="gvEventos_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
ShowFooter="True" TabIndex="14"
PageSize="25" Width="100%">
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Ubicacion" HeaderText="Ubicacion" SortExpression="Ubicacion" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
<asp:BoundField DataField="TipoEvento" HeaderText="Tipo Evento" SortExpression="TipoEvento" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
<asp:BoundField DataField="Operacion" HeaderText="Operación" SortExpression="Operacion" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
<asp:BoundField DataField="InicioEvento" HeaderText="Inicio Evento" SortExpression="InicioEvento" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
<asp:BoundField DataField="ActualizacionInicio" HeaderText="Actualizacion Inicio" SortExpression="ActualizacionInicio" HeaderStyle-Width="75px" ItemStyle-Width="75px"/>
<asp:BoundField DataField="FinEvento" HeaderText="Fin Evento" SortExpression="FinEvento" />
<asp:BoundField DataField="ActualiacionFin" HeaderText="Actualiación Fin" SortExpression="ActualiacionFin" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
<asp:BoundField DataField="MotivoRetraso" HeaderText="Motivo Retraso" SortExpression="MotivoRetraso" HeaderStyle-Width="60px" ItemStyle-Width="60px" />
<asp:BoundField DataField="Estancia" HeaderText="Estancia" SortExpression="Estancia" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
<asp:BoundField DataField="Arrastre1" HeaderText="Arrastre 1" SortExpression="Arrastre1" />
<asp:BoundField DataField="Arrastre2" HeaderText="Arrastre 2" SortExpression="Arrastre2" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbBitacora_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEventos" runat="server" Text="Eventos" OnClick="lkbEventos_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewEventos" />
<asp:AsyncPostBackTrigger ControlID="wucParadaEvento" />
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucParadaEvento" runat="server">
<ContentTemplate>
<tectos:wucParadaEvento runat="server" id="wucParadaEvento" OnBtnActualizar_Click="wucParadaEvento_OnBtnActualizarClick" 
OnBtnCancelar_Click="wucParadaEvento_OnBtnCancelarClick" OnBtnNuevo_Click="wucParadaEvento_OnBtnNuevoClick" 
OnLkbEliminar_Click="wucParadaEvento_OnlkbEliminarClick" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
