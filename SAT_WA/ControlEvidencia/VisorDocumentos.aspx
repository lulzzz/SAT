<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="VisorDocumentos.aspx.cs" Inherits="SAT.ControlEvidencia.VisorDocumentos" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<!-- Estilos Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
<script type="text/javascript" src="../Scripts/jquery.jqzoom-core.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryVisorDocumentos();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryVisorDocumentos() {

//Validación campos Visor Hoja de Instrucción
$(document).ready(function () {
//Función de validación 
var validacionVisorDocumentos = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%= txtNViaje.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%= txtFechaInicial.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%= txtFechaFinal.ClientID%>").validationEngine('validate');

return isValid1 && isValid2 && isValid3 && isValid4;
};

// *** Fecha de inicio, fin de Inicio de servicioa (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaInicial.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFechaFinal.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Botón Buscar
    $("#<%= btnBuscar.ClientID %>").click(validacionVisorDocumentos);

    //Añadiendo Encabezado Fijo
    $("#<%=gvDetalles.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorReporteEvidencias").offsetWidth - 15,
        height: 400,
        //freezesize: 4
    });

});
// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtCliente.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
});
});

    

}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryVisorDocumentos();
</script>
<div id="encabezado_forma">
<img src="../Image/Indicadores.png" />
<h1>Estado Global de Evidencias</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar evidencias por</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCliente" runat="server" TabIndex="1" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="ddlEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:DropDownList ID="ddlEstatus" runat="server" TabIndex="2" CssClass="dropdown">
</asp:DropDownList>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtNViaje">No. Servicio</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtNViaje" runat="server" TabIndex="3" CssClass="textbox  validate[custom[integer]]" MaxLength="50"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCartaPorte" runat="server" TabIndex="4" CssClass="textbox" MaxLength="50"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtReferencia">No. Viaje</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtReferencia" runat="server" TabIndex="5" CssClass="textbox" MaxLength="500"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Fecha Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicial" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicial" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkFechasInicio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:CheckBox ID="chkFechasInicio" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="chkFechasInicio_CheckedChanged" Text="Fecha Termino Servicio" TabIndex="6" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Fecha Final</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFechaFinal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFinal" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkFechasInicio" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">                    
</div>
<div class="control2x">
<asp:TextBox ID="txtCompania" CssClass="textbox2x" runat="server" TabIndex="8" Enabled="false" Visible="false"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="9" CssClass="boton"
OnClick="btnBuscar_OnClick" ValidationGroup="General" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>       
</div>
<div class="contenido_resumen_visor">
<div class="header_seccion">
<img src="../Image/ResumenReporte.png" />
<h2>Resumen por estatus</h2>
</div>
<div class="grafica_resumen_visor">
<asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="ChtDocumentos" runat="server" BackColor="Transparent">                        
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
OnSorting="gvResumen_Sorting" PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Documentos" HeaderText="Estatus" SortExpression="Documentos" />
<asp:TemplateField HeaderText="Servicios" SortExpression="TotalServicios">
<ItemTemplate>
<asp:Label ID="lbldetalles" Text='<%# Eval("TotalServicios") %>' runat="server"></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblSuma" runat="server" Text=""></asp:Label>
</FooterTemplate>
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
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
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>                    
<asp:Label ID="lblOrdenarResumen" runat="server" Visible="false"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvResumen" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>   
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Evidencias encontradas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDetalles">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDetalles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDetalles" runat="server" CssClass="dropdown" TabIndex="11"
OnSelectedIndexChanged="ddlTamanoDetalles_OnSelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarDetalles">Ordenar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarDetalles" runat="server" UpdateMode="Conditional">
<ContentTemplate>                        
<asp:Label ID="lblOrdenarDetalles" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDetalles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lnkExportarDetalles" runat="server" TabIndex="12" Text="Exportar"
OnClick="lnkExportarDetalles_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorReporteEvidencias">
<asp:UpdatePanel ID="upgvDetalles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDetalles" runat="server" AllowPaging="True" AllowSorting="True"
AutoGenerateColumns="False" ShowFooter="True" CssClass="gridview" Width="100%"
OnPageIndexChanging="gvDetalles_PageIndexChanging" PageSize="25"
OnSorting="gvDetalles_Sorting" TabIndex="13">
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No Viaje" SortExpression="NoViaje" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CartaPorte" HeaderText="Porte" SortExpression="CartaPorte" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="OrigenServicio" HeaderText="Origen Servicio" SortExpression="OrigenServicio" />
<asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="DestinoServicio" HeaderText="Destino Servicio" SortExpression="DestinoServicio" />
<asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" >
</asp:BoundField>
<asp:BoundField DataField="Caja" HeaderText="Caja" SortExpression="Caja" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Documentos" SortExpression="Documentos">
<ItemTemplate>
<asp:LinkButton ID="lkbDocumentos" runat="server" CommandName='<%#Eval("Documentos") %>' OnClick="lkbDocumentos_Click" Text='<%#Eval("Documentos") %>' ToolTip="Rercibir Documentos con Digitalización"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha Recepción" SortExpression="FechaRecepcion" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:UpdatePanel ID="uplnkBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkBitacora" runat="server" Text="Bitacora Operativa"
OnClick="lnkBitacora_OnClick" CommandName="BitacoraEvento"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkBitacora" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:UpdatePanel runat="server" ID="uplnkBitacoraEvidencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkBitacoraEvidencias" runat="server" Text="Bitacora Evidencia" OnClick="lnkBitacoraEvidencias_Click" CommandName="BitacoraEvicencias"></asp:LinkButton> 
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>   
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnRecibirDocumentosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaRecibirDocumentosDigitalizados" runat="server" Text="Recibir Documentos" CssClass="boton_pestana_activo" CommandName="RecibirDocumentosDigitalizados" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaDocuemntosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaDocumentosDigitalizados" runat="server" Text="Digitalizados" CssClass="boton_pestana" CommandName="DocumentosDigitalizados" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaRecibirDocumentosDigitalizados" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upDocumentosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvDocumentosDigitalizados" runat="server" ActiveViewIndex="0">
<asp:View ID="vwRecibirDocumentosDigitalizados" runat="server">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDocumentosDigitalizados">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDocumentosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDocumentosDigitalizados" runat="server" OnSelectedIndexChanged="ddlTamañoDocumentosDigitalizados_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarServicios">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblDocumentosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDocumentosDigitalizados" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDocumentosDigitalizados" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uplkbExportarDocumentosDigitalizados" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarDocumentosDigitalizados" runat="server" Text="Exportar Excel" OnClick="lkbExportarDocumentosDigitalizados_OnClick" TabIndex="12"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarDocumentosDigitalizados" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvDocumentosDigitalizados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDocumentosDigitalizados" CssClass="gridview" runat="server" AllowPaging="True" OnPageIndexChanging="gvDocumentosDigitalizados_PageIndexChanging" OnSorting="gvDocumentosDigitalizados_Sorting"
AllowSorting="True" AutoGenerateColumns="false" TabIndex="13"
ShowFooter="True" PageSize="5" Width="100%">
<Columns>
<asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Digitalizado" HeaderText="Digitalizado"
SortExpression="Digitalizado">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Formato" HeaderText="Formato" SortExpression="Formato" />
<asp:BoundField DataField="Sello" HeaderText="Sello" SortExpression="Sello">
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:TemplateField HeaderText="Remitente" SortExpression="Remitente">
<ItemTemplate>
<asp:Label ID="lblRemitente" runat="server" ToolTip='<%# Eval("Remitente") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Remitente").ToString(), 35, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Destinatario" SortExpression="Destinatario">
<ItemTemplate>
<asp:Label ID="lblDestinatario" runat="server" ToolTip='<%# Eval("Destinatario") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Destinatario").ToString(), 35, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="LugarCobro" SortExpression="LugarCobro">
<ItemTemplate>
<asp:Label ID="lblLugarCobro" runat="server" ToolTip='<%# Eval("LugarCobro") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("LugarCobro").ToString(), 35, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAdjuntar" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:LinkButton ID="lkbImagenes" runat="server" Text="Imagen" CommandName="Imagen" OnClick="lkbDocumentoDigitalizado_Click"
TabIndex="15"></asp:LinkButton><br />
<asp:LinkButton ID="lkbBitacora" CommandName="Bitacora" runat="server" OnClick="lkbDocumentoDigitalizado_Click">Bitácora</asp:LinkButton><br />
<asp:LinkButton ID="lkbReferencias" CommandName="Referencias" Text="Referencias" runat="server" OnClick="lkbDocumentoDigitalizado_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbImagenes" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentosDigitalizados" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View> 
<asp:View ID="vwDocumentosDigitalizados" runat="server">
        <div class="contenedor_seccion_completa">
<div class="header_imagenes_documentos">
<img src="../Image/Imagenes_docs.png" />
<h3>Documentos digitalizados</h3>
</div>
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" />
</asp:HyperLink>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbVerReales" />
<asp:AsyncPostBackTrigger ControlID="rdbVerEjemplos" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagenes"> 
<asp:UpdatePanel ID="updtlImagenDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>                    
<asp:DataList ID="dtlImagenDocumentos" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img  title='<%# "ID: " + Eval("Id") + " " + Eval("Documento")%>'  src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
<SelectedItemStyle BackColor="#FFFF99" />
</asp:DataList>                   
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbVerReales" />
<asp:AsyncPostBackTrigger ControlID="rdbVerEjemplos" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="filtro_imagen">
<div class="renglon">
<div class="control2x">
<asp:UpdatePanel ID="uprdbVerReales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbVerReales" runat="server" Text="Ver evidencias digitalizadas"
GroupName="Imagenes" AutoPostBack="true" Checked="True" OnCheckedChanged="rdbVerReales_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbVerEjemplos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="control">
<asp:UpdatePanel ID="uprdbVerEjemplos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbVerEjemplos" runat="server" Text="Ver ejemplos de evidencias"
GroupName="Imagenes" AutoPostBack="true"
OnCheckedChanged="rdbVerEjemplos_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbVerReales" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
        </asp:View>
</asp:MultiView>
   
</ContentTemplate>
    <Triggers> 
        <asp:AsyncPostBackTrigger ControlID="gvDetalles" />
            <asp:AsyncPostBackTrigger ControlID="btnPestanaRecibirDocumentosDigitalizados" />
            <asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
    </Triggers>       
</asp:UpdatePanel>
</div>  
</asp:Content>