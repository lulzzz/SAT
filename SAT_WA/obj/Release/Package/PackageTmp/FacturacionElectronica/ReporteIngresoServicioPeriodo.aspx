<%@ Page Title="Reporte Ingreso Servicios Periodo" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteIngresoServicioPeriodo.aspx.cs" Inherits="SAT.FacturacionElectronica.ReporteIngresoServicioPeriodo" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Referencia a Hoja de Estilos requeridas -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos JQuery -->
<link  href="../CSS/jquery.datetimepicker.css" rel ="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript"  src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReporteIngresos();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryReporteIngresos() {
    $(document).ready(function () {
        //Añadiendo Encabezado Fijo
        $("#<%=gvIngreso.ClientID%>").gridviewScroll({
            width: document.getElementById("contenedorReporteIngresoServicioPeriodo").offsetWidth - 15,
            height: 400

        });



//Validación 
var validacionReporteIngresos = function () {
var isValidP1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');

return isValidP1 && isValidP2;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionReporteIngresos);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryReporteIngresos();
</script>
<div id="encabezado_forma">
<h1>Reporte Ingreso Servicio (Periodo)</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar Servicios</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Desde</label>
</div>
<div class="control">
<asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required,custom[dateTime24]]" TabIndex="1"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Hasta</label>
</div>
<div class="control">
<asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="2" ></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlAgrupador">Agrupar Por</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlAgrupador" runat="server" CssClass="dropdown" TabIndex="3" ></asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="4" />
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Agrupación del Ingreso</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewIngreso">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamañoGridViewIngreso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamañoGridViewIngreso" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewIngreso_SelectedIndexChanged" TabIndex="5" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewIngreso">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewIngreso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewIngreso" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvIngreso" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lkbExportarIngreso" runat="server" Text="Exportar" TabIndex="6" OnClick="lkbExportarIngreso_Click"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_altura_variable"id="contenedorReporteIngresoServicioPeriodo">
<asp:UpdatePanel ID="upgvIngreso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvIngreso" CssClass="gridview" OnPageIndexChanging="gvIngreso_PageIndexChanging" OnSorting="gvIngreso_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" TabIndex="7"
PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Agrupador" HeaderText="Agrupador" SortExpression="Agrupador" >
</asp:BoundField>
<asp:BoundField DataField="Servicios" HeaderText="Servicios" SortExpression="Servicios" />
<asp:BoundField DataField="IngresoTotal" HeaderText="Ingreso Total" SortExpression="IngresoTotal" DataFormatString="{0:c}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="KmCargado" HeaderText="Km Cargado" SortExpression="KmCargado" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="KmVacio" HeaderText="Km Vacío" SortExpression="KmVacio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="KmTronco" HeaderText="Km Tronco" SortExpression="KmTronco" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="%KmVacio" HeaderText="(%) Km Vacío" SortExpression="%KmVacio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="KmTotales" HeaderText="Km Totales" SortExpression="KmTotales" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IngresoKm" DataFormatString="{0:c}" HeaderText="Ingreso Km" SortExpression="IngresoKm">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TractoresTotal" HeaderText="Tractores Total" SortExpression="TractoresTotal" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="KmTractor" HeaderText="Km Tractor" SortExpression="KmTractor" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IngresoTractor" DataFormatString="{0:c}" HeaderText="Ingreso Tractor" SortExpression="IngresoTractor">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewIngreso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
