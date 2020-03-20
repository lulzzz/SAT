<%@ Page Title="Historial de Unidades" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteHistorialUnidades.aspx.cs" Inherits="SAT.ControlPatio.ReporteHistorialUnidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.jqzoom-core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraReporteHistorialUnidades();
}
}
//Función de Configuración
function ConfiguraReporteHistorialUnidades(){
$(document).ready(function () {
                
//Cargando Control DateTimePicker
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Cargando Control DateTimePicker
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
                
});
}
        
//Invocando Función de Configuración
ConfiguraReporteHistorialUnidades();
</script>
<div id="encabezado_forma">
<img src="../Image/Indicadores.png" />
<h1>Historial de Unidades</h1>
</div>    
<section class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlPatio">Patio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPatio" runat="server" TabIndex="1" CssClass="dropdown2x" 
OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTransportista">Transportista</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTransportista" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTransportista" runat="server" TabIndex="2" CssClass="textbox2x validate[IdCatalogo]">
</asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEntidad">Tipo Entidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEntidad" runat="server" CssClass="dropdown2x" TabIndex="3"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" TabIndex="4" CssClass="textbox2x">
</asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdentificacion">Identificación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtIdentificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtIdentificacion" runat="server" TabIndex="5" CssClass="textbox2x">
</asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatusAcceso">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatusAcceso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusAcceso" runat="server" CssClass="dropdown2x" TabIndex="6"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>            
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha de Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" TabIndex="7" CssClass="textbox validate[required, custom[dateTime24]]">
</asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkFechas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="upchkFechas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkFechas" runat="server" Text="¿Incluir Fechas?" Checked="true" AutoPostBack="true"
OnCheckedChanged="chkFechas_CheckedChanged"/>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha de Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" TabIndex="8" CssClass="textbox validate[required, custom[dateTime24]]">
</asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkFechas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="9" CssClass="boton"
OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkFechas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</section>    
<section class="contenedor_graficas">
<div class="grafica_pay">
<div class="header_grafica_indicador">
<div class="header_grafica_indicador_valor">
<asp:UpdatePanel runat="server" ID="uplblUnidades" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUnidades" Text="123"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />                       
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_grafica_indicador_imagen">
<img src="../Image/IndicadorUnidadesPatio.png" />
</div>
<div class="header_grafica_indicador_descripcion">
Unidades Encontradas
</div>           
</div>
<asp:UpdatePanel ID="upchtUnidadesTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtUnidadesTipo" runat="server" BackColor="Transparent">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />                    
</Triggers>
</asp:UpdatePanel>
<div class="grafica_pay_tabla">
<asp:UpdatePanel ID="upgvUnidadesTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUnidadesTipo" runat="server" AutoGenerateColumns="true" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview" ShowFooter="false" ShowHeader="false">
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
<div class="grafica_pay">
<div class="header_grafica_indicador">
<div class="header_grafica_indicador_valor">
<asp:UpdatePanel runat="server" ID="uplblTiempoProm" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempoProm" Text="10d 15h 16m"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />                       
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_grafica_indicador_imagen">
<img src="../Image/IndicadorTiempo.png" />
</div>
<div class="header_grafica_indicador_descripcion">
Estancia promedio de unidades
</div>           
</div>
<asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtTiempoUnidades" runat="server" BackColor="Transparent">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
</Triggers>
</asp:UpdatePanel>
<div class="grafica_pay_tabla">
<asp:UpdatePanel ID="upgvTiempoUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvTiempoUnidades" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview" ShowFooter="false" ShowHeader="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="EstatusTiempo" HeaderText="Tiempo Transcurrido" SortExpression="EstatusTiempo" />
<asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:Label ID="lblCantidad" runat="server" Text='<%# Eval("Cantidad") %>'></asp:Label>
</ItemTemplate>
<FooterStyle HorizontalAlign="Right" />
<FooterTemplate>
<asp:Label ID="lblSumaCantidad" runat="server"></asp:Label>
</FooterTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>         
<div class="grafica_pay">
<div class="header_grafica_indicador">
<div class="header_grafica_indicador_valor">
<asp:UpdatePanel runat="server" ID="uplblTransportista" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTransportista" Text="12"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />                       
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_grafica_indicador_imagen">
<img src="../Image/Transportista.png" />
</div>
<div class="header_grafica_indicador_descripcion">
Transportistas Ligados
</div>           
</div>
<asp:UpdatePanel ID="upchtTransportista" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtTransportista" runat="server" BackColor="Transparent">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />                    
</Triggers>
</asp:UpdatePanel>
<div class="grafica_pay_tabla">
<asp:UpdatePanel ID="upgvTransportista" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvTransportista" runat="server" AutoGenerateColumns="True" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview" ShowFooter="false" ShowHeader="false">
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
</section>
<section class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Resultado Busqueda</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" Tabindex="10"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEntidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" 
OnClick="lnkExportar_Click" Tabindex="11"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvEntidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEntidades" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
OnSorting="gvEntidades_Sorting" OnPageIndexChanging="gvEntidades_PageIndexChanging"
OnRowDataBound="gvEntidades_RowDataBound"
PageSize="25" Width="100%" CssClass="gridview" ShowFooter="true" Tabindex="12">
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
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
<ItemTemplate>
<asp:LinkButton ID="lnkBitacora" runat="server" Text='<%# Eval("Descripcion") %>' OnClick="lnkBitacora_Click">
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Identificacion" HeaderText="Identificación" SortExpression="Identificacion" />
<asp:BoundField DataField="Transportista" HeaderText="Linea de Transporte" SortExpression="Transportista" />
<asp:BoundField DataField="EstatusPatio" HeaderText="Estatus en Patio" SortExpression="EstatusPatio" />
<asp:BoundField DataField="FechaEntrada" HeaderText="Fecha de Entrada" SortExpression="FechaEntrada" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="ActualCargado" HeaderText="Estado" SortExpression="ActualCargado" />
<asp:BoundField DataField="FechaSalida" HeaderText="Fecha de Salida" SortExpression="FechaSalida" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="TiempoEstancia" HeaderText="Tiempo de Estancia" SortExpression="TiempoEstancia" />
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="lnkEvidencias_Click">
<img src="../Image/ImagenEvidencia.png" width="20" height="20" />
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
</Triggers>
</asp:UpdatePanel>
</div>
</section>
<div id="contenidoBitacoraUnidades" class="modal">
<div id="bitacoraUnidades" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Bitacora<br />
Eventos de la Unidad
</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoBit">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoBit" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoBit_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoBit">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoBit" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacora" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarBit" runat="server" Text="Exportar" 
OnClick="lnkExportarBit_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarBit" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_unidad_agrupada">                
<asp:UpdatePanel ID="upgvBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvBitacora" runat="server" CssClass="gridview" 
OnSorting="gvBitacora_Sorting" OnPageIndexChanging="gvBitacora_PageIndexChanging"
AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="FecIni" HeaderText="Fecha de Inicio" SortExpression="FecIni" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FecFin" HeaderText="Fecha de Termino" SortExpression="FecFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoBit" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon_boton_salir">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCerrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CssClass="boton_cancelar"
OnClick="btnCerrar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenidoVentanaEvidencias" class="modal">
<div id="ventanaEvidencias" class="contenedor_modal_imagenes">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="contenedor_imagenes_patio">
<div class="header_seccion">
<img src="../Image/Imagenes.png" />
<h2>Evidencias en Patio</h2>
</div>
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
</asp:HyperLink>
</ContentTemplate>                        
</asp:UpdatePanel>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenImagenes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DataList ID="dtlImagenImagenes" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id")  %>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:DataList>
</ContentTemplate>
                        
</asp:UpdatePanel>
</div>
               
</div>
</div>
</div>
</asp:Content>
