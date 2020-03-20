<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ExistenciasAlmacen.aspx.cs" Inherits="SAT.Almacen.ExistenciasAlmacen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!--Biblioteca encabezados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryReporteExistencias();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryReporteExistencias() {
$(document).ready(function () {

//Almacen
$("#<%=txtAlmacen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
//Validación de Controles
$("#<%=btnBuscar.ClientID%>").click(function () {
//Validando Controles
var isValid1 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
//Añadiendo Controles a la Validación
var isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
//Devolviendo Resultado Obtenido
return isValid1 && isValid2 && isValid3;
});

});

          
//Cargando Control DateTimePicker "Fecha Inicio"
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false
});
//Cargando Control DateTimePicker "Fecha Fin"
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker:false
});
//Añadiendo Encabezado Fijo
$("#<%=gvExistencias.ClientID%>").gridviewScroll({
width: document.getElementById("contenedor").offsetWidth - 15,
height: 400
});
}

//Invocando Función de Configuración
ConfiguraJQueryReporteExistencias();
</script>
<div id="encabezado_forma">
      
<h1>Existencias en Almacén</h1>
</div>
<div class="contenedor_seccion_completa">
<div class="columna2x">
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtAlmacen">Almacén</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtProducto">Producto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProducto" runat="server" CssClass="textbox2x"  TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
            
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtLote">Lote</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtLote" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLote" runat="server" MaxLength="50" CssClass="textbox" TabIndex="3" ></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" MaxLength="50" CssClass="textbox" TabIndex="4" ></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtFecIni">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="5" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Caducidad" TabIndex="6" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtFecFin">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="7" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="8"  OnClick="btnBuscar_Click" CssClass="boton" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>          
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Existencias</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" CssClass="dropdown" TabIndex="9"
AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvExistencias" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click" TabIndex="10" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class ="grid_seccion_completa" id="contenedor">
<asp:UpdatePanel ID="upgvExistencias"  runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvExistencias" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="11" OnPageIndexChanging="gvExistencias_PageIndexChanging" OnSorting="gvExistencias_Sorting"
PageSize="25" CssClass="gridview" FooterStyle-HorizontalAlign="Right" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
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
<asp:BoundField DataField="Almacen" HeaderText="Almacén" SortExpression="Almacen" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Lote" HeaderText="Lote" SortExpression="Lote" />
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
<asp:BoundField DataField="FechaCaducidad" HeaderText="Fecha Caducidad" SortExpression="FechaCaducidad" DataFormatString="{0:dd/MM/yyyy}"/>
<asp:BoundField DataField="PrecioEntrada" HeaderText="Precio Entrada" SortExpression="PrecioEntrada" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="PrecioSalidaActual" HeaderText="Precio Salida Actual" SortExpression="PrecioSalidaActual" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="CantEntrada" HeaderText="Cantidad Entrada" SortExpression="CantEntrada" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="CantSalida" HeaderText="Cantidad Salida" SortExpression="CantSalida" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="CantExistencia" HeaderText="Cantidad Existencia" SortExpression="CantExistencia" ItemStyle-HorizontalAlign="Right"/>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
