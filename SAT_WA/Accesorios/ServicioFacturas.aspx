<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServicioFacturas.aspx.cs" Inherits="SAT.Accesorios.ServicioFacturas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<!-- Estilos de Scripts -->
<link href="../CSS/jquery-ui.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.min.css" rel="stylesheet" />
<!-- Animaciones de entrada y salida de elementos -->
<link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
<link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
<style>
#contenedorXML {
margin-top: 10px;
margin-left: 10px;
margin-bottom:20px;
width: 400px;
height: 120px;
text-align: center;
vertical-align: middle;
border: 2px solid #939393;
background-color: #f8f8f8;
padding: 15px;
font-family: Arial;
font-size: 16px;
}
</style>
<!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
<script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>' type="text/javascript"></script>
<!--<script src="../Scripts/jquery-1.7.1.js" type="text/javascript"></script>-->
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.min.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
<!-- Notificaciones emergentes -->
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
<!-- Libreria para Carga de Archivos -->
<script src="../Scripts/modernizr-2.5.3.js" type="text/javascript"></script>
<!-- Biblioteca para encabezado de GridView -->
<%--<script type="text/javascript" src="../Scripts/gridviewScroll.min.js" charset="utf-8"></script>--%>
<title></title>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
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

<!-- Script de Configuración de Controles -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryAutorizacionDeposito();
}
}

//Creando función para configuración de jquery en formulario
function ConfiguraJQueryAutorizacionDeposito() {
    $(document).ready(function () {

        <%--//Añadiendo Encabezado Fijo
        $("#<%=gvFacturasLigadas.ClientID%>").gridviewScroll({
            width: document.getElementById("contenedorFacturasLigadas").offsetWidth - 15,
            height: 250
        });--%>

        //Declarando Objeto contenedor del DIV
        var box;
        box = document.getElementById('contenedorXML');

        //Validando que exista el Contenedor
        if (box != null) {

            <%--//Añadiendo Encabezado Fijo
            $("#<%=gvFacturasDisponibles.ClientID%>").gridviewScroll({
                width: document.getElementById("contenedorFacturasDisponibles").offsetWidth - 15,
                height: 200
            });--%>

            //Añadiendo Eventos
            box.addEventListener('dragenter', OnDragEnter, false);
            box.addEventListener('dragover', OnDragOver, false);
            box.addEventListener('drop', OnDrop, false);

            //Función cuando se Arrastra el Objeto dentro del limite
            function OnDragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
            }
            //Función cuando se Arrastra el Objeto fuera del limite
            function OnDragOver(e) {
                e.stopPropagation();
                e.preventDefault();
            }
            //Función cuando se Suelta el Objeto dentro del limite
            function OnDrop(e) {
                e.stopPropagation();
                e.preventDefault();

                selectedFiles = null;
                selectedFiles = e.dataTransfer.files;
                //Declarando Objeto de Lectura
                var lector = new FileReader();

                //Evento al Cargar el Archivo
                lector.onload = function (evt) {
                    //Obteniendo Archivo
                    var bytes = evt.target.result;
                    //Invocando Método Web para Obtención de Archivos
                    PageMethods.ArchivoSesionDepositoFactura(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
                };
                //Evento al Producirse un Error
                lector.onerror = function (evt) {
                    alert('Error Carga ' + evt.target.error);

                };
                //Leyendo Texto
                lector.readAsText(selectedFiles[0]);
                //Mostrando mensaje
                alert('El Archivo se ha Cargado')
                //Indicando Archivo
                $('#contenedorXML').text('El Archivo ' + selectedFiles[0].name + ' ha sido Cargado con exito');
            }
        }
    });
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryAutorizacionDeposito();

/**Script Contenedor de Archivos**/
//Declarando variable contenedora de Archivos
var selectedFiles;
//Función que limpia el Contenedor
function LimpiaContenedorXML() {   //Limpiando DIV
selectedFiles = null;
$('#contenedorXML').text('Arrastre y Suelte sus archivos desde su maquina en este cuadro.');
}
//Inicializando Función
$(document).ready(function () {
//validando el Tipo de Archivo
if (!Modernizr.draganddrop) {
alert('This browser doesnt support File API and Drag & Drop features of HTML5!');
return;
}


});

</script>

<div>
<div class="contenedor_media_seccion_izquierda" style="float:left; width:510px;">
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Ligadas</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoFL">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFL" CssClass="dropdown_100px" runat="server" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Ordenado:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" CommandName="FacturasLigadas" OnClick="lnkExportar_Click">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura" id="contenedorFacturasLigadas">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="true"
CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
OnRowDataBound="gvFacturasLigadas_RowDataBound" TabIndex="37" AutoGenerateColumns="false" Width="100%" PageSize="5">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:LinkButton ID="lnkAceptarFacturaLigada" runat="server" Text='<%# Eval("Estatus") %>' OnClick ="lnkAceptarFacturaLigada_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha Fac." SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbRelacionAplicacion" runat="server" Text='<%# Eval("SaldoActual", "{0:C2}") %>' OnClick="lkbRelacionAplicacionFacLig_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eliminar" OnClick="lnkEliminarFacturaLiq_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="btnLigarFactura" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_media_seccion_derecha" style="float:left; width:510px;">
<asp:UpdatePanel ID="upmtvFacturasLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvFacturasLiquidacion" runat="server" ActiveViewIndex="0">
<asp:View ID="vwFacturasExistentes" runat="server">
<div class="header_seccion">
<img src="../Image/Facturacion.png" />
<h2>Seleccionar una Factura Existente</h2>
<div class="etiqueta_80pxr">
<asp:UpdatePanel ID="uplkbNuevaFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNuevaFactura" runat="server" Text="Nueva Factura" CommandName="Nueva" OnClick="lkbVerFacturasLiq_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta_50px">
<label for="txtSerie">Serie</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_100px"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLigarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtFolio">Folio</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFolio" runat="server" CssClass="textbox_100px validate[custom[number]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLigarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="etiqueta_50px">
<label for="txtSerie">UUID</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x" MaxLength="36"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLigarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarFac" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarFac" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscarFac_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoFacLiq">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoFacLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacLiq" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFacLiq_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoFacLiq">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoFacLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoFacLiq" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarFacLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacLiq" runat="server" Text="Exportar" CommandName="FacturasLiq" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura" id="contenedorFacturasDisponibles">
<asp:UpdatePanel ID="upgvFacturasDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasDisponibles" runat="server" AllowPaging="true" AllowSorting="true"
OnRowDataBound="gvFacturasDisponibles_RowDataBound"
OnPageIndexChanging="gvFacturasDisponibles_PageIndexChanging" OnSorting="gvFacturasDisponibles_Sorting"
PageSize="25" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkTodosFac" runat="server"
OnCheckedChanged="chkTodosFac_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVariosFac" runat="server"
OnCheckedChanged="chkTodosFac_CheckedChanged" AutoPostBack="true" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
<asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:LinkButton ID="lnkAceptarFacturaDisponible" runat="server" Text='<%# Eval("Estatus") %>' OnClick="lnkAceptarFacturaDisponible_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" >
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbRelacionAplicacion" runat="server" Text='<%# Eval("SaldoActual", "{0:C2}") %>' OnClick="lkbRelacionAplicacionFacDisp_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarFac" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacLiq" />
<asp:AsyncPostBackTrigger ControlID="btnLigarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnLigarFacturaP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLigarFactura" runat="server" CssClass="boton" Text="Ligar Fac." OnClick="btnLigarFactura_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:View>
<asp:View ID="vwFacturasNuevas" runat="server">
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Agregar una Nueva Factura</h2>
<div class="etiqueta_80pxr">
<asp:UpdatePanel ID="uplkbVerFacturasLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerFacturasLiq" runat="server" CommandName="Ver" Text="Seleccionar Factura(s)" OnClick="lkbVerFacturasLiq_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">
<div id="contenedorXML">Arrastre y suelte sus archivos XML desde su carpeta a este cuadro.</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFactura" runat="server" Text="Importar Factura" CssClass="boton" OnClick="btnAgregarFactura_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="header_seccion">
<label class="label_negrita">
NOTA : ES RESPONSABILIDAD DEL USUARIO VERIFICAR LOS DATOS DE LA FACTURA (CONCEPTOS, IMPORTES E IMPUESTOS) ANTES DE CAMBIAR EL ESTATUS DE LA FACTURA A 'ACEPTADA'.
</label>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevaFactura" />
<asp:AsyncPostBackTrigger ControlID="lkbVerFacturasLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana Confirmación Resultado Consulta SAT -->
<div id="contenidoResultadoConsultaSATModal" class="modal">
<div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba">
<div class="columna2x">
<div class="header_seccion">
<asp:UpdatePanel ID="upheaderValidacionSAT" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
<h3 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h3>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Emisor</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Receptor</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblRFCReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRFCReceptor" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">UUID</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblUUID" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Fecha Expedición</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Total Factura</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblTotalFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalFactura" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCanelarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCanelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- Ventana de Aplicaciones y Relaciones -->
<div id="contenidoVentanaFichasFacturas" class="modal">
<div id="ventanaFichasFacturas" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarFF" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarFF" runat="server" CommandName="FichasFacturas" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<asp:UpdatePanel ID="uplblVentanaFacturasFichas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2>Aplicaciones y Relaciones de la Factura</h2>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoFF">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFF" runat="server" CssClass="dropdown" TabIndex="10"
OnSelectedIndexChanged="ddlTamanoFF_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFF">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFF" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarFF" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFF" runat="server" Text="Exportar" TabIndex="11" CommandName="FichasFacturas" OnClick="lnkExportarFF_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvFichasFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFichasFacturas" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvFichasFacturas_PageIndexChanging" OnSorting="gvFichasFacturas_Sorting"
OnRowDataBound="gvFichasFacturas_RowDataBound" CssClass="gridview" AllowSorting="true" 
AllowPaging="true" ShowFooter="true">
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
<asp:BoundField DataField="Egreso" HeaderText="No. Egreso" SortExpression="Egreso" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="IdEntidad" HeaderText="IdEntidad" SortExpression="IdEntidad" Visible="false" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="FechaAplicacion" HeaderText="Fecha Aplicación" SortExpression="FechaAplicacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Viajes" SortExpression="Viajes">
<ItemTemplate>
<asp:LinkButton ID="lnkServiciosEntidad" runat="server" OnClick="lnkServiciosEntidad_Click" Text='<%# Eval("Viajes") %>'></asp:LinkButton>
<asp:Label ID="lblServiciosEntidad" runat="server" Text='<%# Eval("Viajes") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFF" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Fichas Aplicadas -->
<div id="contenidoVentanaServiciosEntidad" class="modal">
<div id="ventanaServiciosEntidad" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarSE" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarSE" runat="server" CommandName="ServiciosEntidad" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Servicios de la Liquidación</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoSE">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoSE" runat="server" CssClass="dropdown" TabIndex="10"
OnSelectedIndexChanged="ddlTamanoSE_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoSE">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenadoSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoSE" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosEntidad" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarSE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarSE" runat="server" Text="Exportar" TabIndex="11" OnClick="lnkExportarSE_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarSE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvServiciosEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosEntidad" runat="server" AutoGenerateColumns="false" Width="100%" PageSize="25"
OnPageIndexChanging="gvServiciosEntidad_PageIndexChanging" OnSorting="gvServiciosEntidad_Sorting"
CssClass="gridview" AllowSorting="true" AllowPaging="true" ShowFooter="true">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFichasFacturas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoSE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</form>
</body>
</html>
