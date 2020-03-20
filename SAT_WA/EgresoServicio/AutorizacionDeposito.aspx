<%@ Page Title="Autorización Deposito" Language="C#" AutoEventWireup="true" CodeBehind="AutorizacionDeposito.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.EgresoServicio.AutorizacionDeposito" %>
<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagName="wucFacturadoConcepto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
<!-- Estilos -->
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
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
<script src="../Scripts/modernizr-2.5.3.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
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
        

        //Declarando Objeto contenedor del DIV
        var box;
        box = document.getElementById('contenedorXML');
        //Añadiendo Eventos
        box.addEventListener('dragenter', OnDragEnter, false);
        box.addEventListener('dragover', OnDragOver, false);
        box.addEventListener('drop', OnDrop, false);

    });
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
</script>
<div id="encabezado_forma"> 
<img src="../Image/Llave.png" />       
<h1>Autorización de depositos</h1>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Depositos.png" />
<h2>Depositos pendientes</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoAutorizaciones">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoAutorizaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoAutorizaciones" runat="server" CssClass="dropdown" TabIndex="7" AutoPostBack="true" OnSelectedIndexChanged="ddlTamañoGridViewAutorizaciones_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarAutorizaciones">Ordenar:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarAutorizaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarAutorizaciones" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAutorizaciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="8"  CommandName="Autorizaciones" OnClick="lkbExcel_Click"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvAutorizaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAutorizaciones" runat="server" AllowPaging="True" OnPageIndexChanging="gvAutorizaciones_PageIndexChanging" OnSorting="gvAutorizaciones_Sorting"
AllowSorting="True" AutoGenerateColumns="False" CssClass="gridview" Width="100%" OnRowDataBound="gvAutorizaciones_RowDataBound"
PageSize="25" ShowFooter="True" TabIndex="9">
<Columns>
<asp:TemplateField SortExpression="Folio">
<ItemTemplate>
<asp:CheckBox ID="chkAutorizacion" runat="server" AutoPostBack="True"
OnCheckedChanged="chkAutorizacion_CheckedChanged"
Text='<%# Eval("Folio") %>' />
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblSeleccionAutorizacion" runat="server" Text="0"></asp:Label>
                                    Seleccionado(s)
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkTodosAutorizacion" runat="server" AutoPostBack="True" CssClass="LabelResalta"
Text="FOLIO" OnCheckedChanged="chkTodosAutorizacion_CheckedChanged" />
</HeaderTemplate>
<FooterStyle HorizontalAlign="Center" />
<HeaderStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="NoOrden" HeaderText="No. Servicio"
SortExpression="NoOrden" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="No. Viaje" SortExpression="NoViaje">
<ItemTemplate>
<asp:LinkButton ID="lkbReferenciasViaje" runat="server" Text='<%# Eval("NoViaje") %>' OnClick="lkbReferenciasViaje_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="OrigenDestino" HeaderText="Origen - Destino"
SortExpression="OrigenDestino" />
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor"
SortExpression="Proveedor" />
<asp:BoundField DataField="Operador" HeaderText="Operador"
SortExpression="Operador" />
<asp:BoundField DataField="Tractor" HeaderText="Unidad"
SortExpression="Tractor" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud"
DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaCarga" HeaderText="Fecha Carga" SortExpression="FechaCarga"
DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="IdServicio" HeaderText="No. Servicio"
SortExpression="IdServicio" Visible="false" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto"></asp:BoundField>
<asp:BoundField DataField="Monto" DataFormatString="{0:c2}" HeaderText="Monto"
SortExpression="Monto">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TiempoEspera" HeaderText="Tiempo Espera" SortExpression="*TiempoEspera">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Solicitante" HeaderText="Solicitante"
SortExpression="Solicitante"></asp:BoundField>
<asp:TemplateField HeaderText="Total Cobro" SortExpression="TotalCobro">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbTotalCobro" runat="server" CommandName="TotalCobro" Text='<%# string.Format("{0:C2}", Eval("TotalCobro"))%>' OnClick="lkbTotalCobro_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Total Facturas Ligadas" SortExpression="TotalFacturasLigadas">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbTotalFL" runat="server" CommandName="TotalFacturasLigadas" Text='<%# string.Format("{0:C2}", Eval("TotalFacturasLigadas"))%>' OnClick="lkbTotalCobro_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacoraAutorizacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacoraAutorizacion" runat="server" CommandName="Bitacora" OnClick="lkbAutorizacion_Click">Bitácora</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacoraAutorizacion" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uplkbReferenciasAutorizacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbReferenciasAutorizacion" runat="server" CommandName="Referencias" OnClick="lkbAutorizacion_Click">Referencias</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbReferenciasAutorizacion" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoAutorizaciones" />
<asp:AsyncPostBackTrigger ControlID="btnAutorizar" />
<asp:AsyncPostBackTrigger ControlID="btnRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnActualizar" />
<asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
</Triggers>
</asp:UpdatePanel>
</div>        
</div>
<div class="renglon100Per">
<div class="controlBoton">
<asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click"
CssClass="boton" />
</div>
<div class="controlBoton">
<asp:Button ID="btnAutorizar" runat="server" Text="Autorizar" CommandName="Autorizar" OnClick="btnAutorizarRechazar_Click"
CssClass="boton" />
</div>
<div class="controlBoton">
<asp:Button ID="btnRechazar" runat="server" Text="Rechazar" CommandName="Rechazar" OnClick="btnAutorizarRechazar_Click"
CssClass="boton" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_320px" style="height: auto">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAutorizar" />
<asp:AsyncPostBackTrigger ControlID="btnRechazar" />
<asp:AsyncPostBackTrigger ControlID="btnActualizar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana de Edición de Conceptos (Detalles de Factura) -->
<div id="contenedorVentanaEdicionConceptos" class="modal">
<div id="ventanaEdicionConceptos" class="contenedor_modal_asignacion_recursos">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarVentanaEC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaEC" runat="server" CommandName="FacturadoConcepto" Text="Cerrar" OnClick="lnkCerrarVentana_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Detalles de la Factura</h2>
</div>
<asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Always">
<ContentTemplate>
<tectos:wucFacturadoConcepto ID="ucFacturadoConcepto" runat="server" OnClickGuardarFacturaConcepto="ucFacturadoConcepto_ClickGuardarFacturaConcepto"
OnClickEliminarFacturaConcepto="ucFacturadoConcepto_ClickEliminarFacturaConcepto" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAutorizaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana de Insercción y Vizualización de Facturas Ligadas -->
<div id="contenedorVentanaFacturasLigadas" class="modal">
<div id="ventanaFacturasLigadas" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel ID="uplnkCerrarVentanaFL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaFL" runat="server" CommandName="FacturasLigadas" Text="Cerrar" OnClick="lnkCerrarVentana_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<h2>Facturas Ligadas</h2>
</div>
<div class="columna2x">
<div id="contenedorXML">Arrastre y suelte sus archivos XML desde su carpeta a este cuadro.</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFactura" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAgregarFactura_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
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
<asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkExportar_Click">Exportar</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="true"
CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
TabIndex="37" AutoGenerateColumns="false" Width="100%" PageSize="5">
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
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha Fac." SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy}" />
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="SaldoActual" HeaderText="Saldo Actual" SortExpression="SaldoActual" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
<asp:AsyncPostBackTrigger ControlID="gvAutorizaciones" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana Referencias de Viaje -->
<div id="contenedorVentanaReferenciasViaje" class="modal">
<div id="ventanaReferenciasViaje" class="contenedor_ventana_confirmacion" style="width:300px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" CommandName="ReferenciasViaje" OnClick="lnkCerrarVentana_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAutorizaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Referencias del Viaje</h2>
</div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciasViaje" runat="server" OnClickGuardarReferenciaViaje="ucReferenciasViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="ucReferenciasViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAutorizaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    
</asp:Content>
