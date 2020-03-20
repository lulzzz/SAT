<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteTrazabilidadInventario.aspx.cs" Inherits="SAT.Almacen.ReporteTrazabilidadInventario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Liquidacion.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryTrazabilidad();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryTrazabilidad() {
$(document).ready(function () {
//Cargando Catalogo Autocompleta
$("#<%=txtAlmacen.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});
$("#<%=txtProducto.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=31&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});

//Creando Validación de Busqueda
var validaBusquedaReporte = function () {
//Validando Controles
var isValid1 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtProducto.ClientID%>").validationEngine('validate');

//Devolviendo Resultado
return isValid1 && isValid2;
}

//Añadiendo a Evento Click
$("#<%=btnBuscar.ClientID%>").click(validaBusquedaReporte);

//Añadiendo Encabezado Fijo
$("#<%=gvInventario.ClientID%>").gridviewScroll({
width: document.getElementById("contenedorInventario").offsetWidth - 15,
height: 500
});
});
}

//Invocando Función de Configuración
ConfiguraJQueryTrazabilidad();
</script>
<div id="encabezado_forma">
<img src="../Image/Clasificacion.png" />
<h1>Trazabilidad del Inventario</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtAlmacen">Almacen</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtLote">Lote</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtLote" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLote" runat="server" CssClass="textbox" TabIndex="2" MaxLength="50"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProducto">Producto</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtProducto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProducto" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="4" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Tabla.png" />
<h2>Reporte de Inventario</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplkbExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportar" runat="server" Text="Exportar Excel" OnClick="lkbExportar_Click" TabIndex="5"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenedorInventario" class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvInventario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvInventario" ShowFooter="True" runat="server" AutoGenerateColumns="False" AllowPaging="False" 
TabIndex="6" ShowHeaderWhenEmpty="True" AllowSorting="False"
CssClass="gridview" Width="100%" OnRowDataBound="gvInventario_RowDataBound" >
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<Columns>
<asp:BoundField DataField="IdTipoRegistro" HeaderText="IdTipoRegistro" SortExpression="IdTipoRegistro" Visible="false" />
<asp:BoundField DataField="Lote" HeaderText="Lote" SortExpression="Lote" />
<asp:BoundField DataField="FechaCaducidad" HeaderText="Fecha de Caducidad" SortExpression="FechaCaducidad" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="SKU" HeaderText="SKU" SortExpression="SKU" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
<asp:BoundField DataField="FechaOperacion" HeaderText="Fecha de Operacion" SortExpression="FechaOperacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TipoOperacion" HeaderText="Operacion" SortExpression="TipoOperacion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Registro" HeaderText="Registro" SortExpression="Registro" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="FechaEntrega" HeaderText="Fecha de Entrega" SortExpression="FechaEntrega" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:Content>
