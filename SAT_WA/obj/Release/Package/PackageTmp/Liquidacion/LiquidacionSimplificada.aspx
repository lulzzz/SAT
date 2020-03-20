<%@ Page Title="Liquidación Simplificada" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="LiquidacionSimplificada.aspx.cs" Inherits="SAT.Liquidacion.LiquidacionSimplificada" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucDevolucionFaltante.ascx" TagName="wucDevolucionFaltante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagName="wucVencimientosHistorial" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucVencimiento.ascx" TagName="wucVencimiento" TagPrefix="tectos" %>
<%@ Register Src="~/Externa/wucCalificacion.ascx" TagPrefix="tectos" TagName="wucCalificacion" %>
<%@ Register Src="~/Externa/wucHistorialCalificacion.ascx" TagPrefix="tectos" TagName="wucHistorialCalificacion" %>
<%@ Register Src="~/UserControls/wucLectura.ascx" TagPrefix="tectos" TagName="wucLectura" %>
<%@ Register Src="~/UserControls/wucLecturaHistorial.ascx" TagPrefix="tectos" TagName="wucLecturaHistorial" %>
<%@ Register Src="~/UserControls/wucControlDiesel.ascx" TagName="wucControlDiesel" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Liquidacion.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<style>
#box {
width: 250px;
height: 50px;
text-align: center;
vertical-align: -webkit-baseline-middle;
border: 2px solid #04B404;
background-color: #00FF00;
padding: 15px;
font-family: Arial;
font-size: 16px;
margin-top: 35px;
}
#contenedorFacturaLiquidacion{
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
        .promLiq {
            margin: 0px 0px 0px 0px;
            padding:0px 0px 0px 550px;
            width: 165px;
            height: 29px;
            float: left;
        }

        .comentariosLiq {
            margin: 0px 0px 0px 0px;
            padding: 8px 0px 0px 0px;
            width: 238px;
            height: 18px;
            float: left;
        }
</style>
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraLiquidacion();
}
}
//Función de Configuración
function ConfiguraLiquidacion() {
    $(document).ready(function () {

/*Cargando Catalogos*/
//Unidad
$("#<%=txtRecursoUn.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=28&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
//Operador
$("#<%=txtRecursoOp.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=27&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
//Proveedor
$("#<%=txtRecursoPr.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=45&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

//Añadiendo Encabezado Fijo
$("#<%=gvServiciosLiquidacion.ClientID%>").gridviewScroll({
width: document.getElementById("contenedorServiciosLiquidacion").offsetWidth - 15,
height: 400,
freezesize: 2
});

//Función de Validación
var validaBusqueda = function () {
//Declarando Variable de Retorno
var isValid1;

//Obteniendo Tipo de Entidad
var tipoEntidad = $("#<%=ddlTipoRecurso.ClientID%>").val();

//Validando Tipo de Entidad
switch(tipoEntidad)
{
case "1":
//Validando Unidad
    isValid1 = !$("#<%=txtRecursoUn.ClientID%>").validationEngine('validate');
break;
case "2":
//Validando Operador
    isValid1 = !$("#<%=txtRecursoOp.ClientID%>").validationEngine('validate');
break;
case "3":
//Validando Unidad
    isValid1 = !$("#<%=txtRecursoPr.ClientID%>").validationEngine('validate');
break;
}

//Devolviendo Resultado de Validación
return isValid1;
}

//Función de Validación de cuenta de pago
var validaNuevaCuentaPago = function () {
    //Declarando Variable de Retorno
    var isValid1 = !$("#<%=txtNuevaCuentaPago.ClientID%>").validationEngine('validate');

    //Devolviendo Resultado de Validación
    return isValid1;
}

//Añadiendo Funcion de Validacion al Evento Click
$("#<%=btnBuscarLiquidaciones.ClientID%>").click(validaBusqueda);
//Añadiendo Funcion de Validacion al Evento Click
$("#<%=btnCrear.ClientID%>").click(validaBusqueda);

$("#<%=btnAgregarFactura.ClientID%>").click(
//Invocando Método de Limpieza
LimpiaContenedorXML()
);
        //Declarando Objeto de Función
        var limpiaContenedor = function () {
            //Limpiando Contenedor
            LimpiaContenedorFacturaProveedor();
        }

//Añadiendo Funcion de Validacion al Evento Click
$("#<%=btnAceptarCuentaPago.ClientID%>").click(validaNuevaCuentaPago);

//Cargando Control DateTimePicker
$("#<%=txtFechaLiq.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Función de Validación de los Controles de Pago
var validaPago = function () {
var isValid1 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtValorU.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtTotal.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1 && isValid2 && isValid3 && isValid4;
}

//Añadiendo Función de Validación
$("#<%=btnGuardarPago.ClientID%>").click(validaPago);

//Función de Validación de los Controles de Comprobación
var validaComprobacion = function () {
var isValid1 = !$("#<%=txtValorUnitario.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1;
}

//Añadiendo Función de Validación
    $("#<%=btnGuardarComprobacion.ClientID%>").click(validaComprobacion);

    //Evento de Calculo de Total
    $("#<%=txtCantidad.ClientID%>").change(function(){
        
        //Obteniendo Valores
        var cantidad = $("#<%=txtCantidad.ClientID%>").val();
        var valor_u = $("#<%=txtValorU.ClientID%>").val();
        var total = 0.00;

        //Validando que exista la Cantidad
        if (cantidad == "")
            //Asignando '0's a la Variable
            cantidad = "0";

        //Validando que exista el Valor Unitario
        if (valor_u == "")
            //Asignando '0's a la Variable
            valor_u = "0";

        //Calculando Total
        total = parseFloat(cantidad) * parseFloat(valor_u);

        //Asignando Valores
        $("#<%=txtCantidad.ClientID%>").val(cantidad);
        $("#<%=txtValorU.ClientID%>").val(valor_u);
        $("#<%=txtTotal.ClientID%>").val(total);
    
    });

    //Evento de Calculo de Total
    $("#<%=txtValorU.ClientID%>").change(function () {

        //Obteniendo Valores
        var cantidad = $("#<%=txtCantidad.ClientID%>").val();
        var valor_u = $("#<%=txtValorU.ClientID%>").val();
        var total = 0.00;

        //Validando que exista la Cantidad
        if (cantidad == "")
            //Asignando '0's a la Variable
            cantidad = "0";

        //Validando que exista el Valor Unitario
        if (valor_u == "")
            //Asignando '0's a la Variable
            valor_u = "0";

        //Calculando Total
        total = parseFloat(cantidad) * parseFloat(valor_u);

        //Asignando Valores
        $("#<%=txtCantidad.ClientID%>").val(cantidad);
        $("#<%=txtValorU.ClientID%>").val(valor_u);
        $("#<%=txtTotal.ClientID%>").val(total);

    });
});
}
//Invocando Función de Configuración
ConfiguraLiquidacion();

/**Script Contenedor de Archivos**/
//Declarando variable contenedora de Archivos
var selectedFiles;
//Función que limpia el Contenedor
function LimpiaContenedorXML() {   //Limpiando DIV
    selectedFiles = null;
    $("#box").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
}
function LimpiaContenedorFacturaProveedor() {   //Limpiando DIV
    selectedFiles = null;
    $("#contenedorFacturaLiquidacion").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
}

//Inicializando Función
$(document).ready(function () {
    //validando el Tipo de Archivo
    if (!Modernizr.draganddrop) {
        alert("This browser doesn't support File API and Drag & Drop features of HTML5!");
        return;
    }
    //Declarando Objeto contenedor del DIV
    var box;
    box = document.getElementById("box");
    //Añadiendo Eventos
    box.addEventListener("dragenter", OnDragEnter, false);
    box.addEventListener("dragover", OnDragOver, false);
    box.addEventListener("drop", OnDrop, false);

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
            PageMethods.ArchivoSesion(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
        };
        //Evento al Producirse un Error
        lector.onerror = function (evt) {
            alert('Error Carga ' + evt.target.error);

        };
        //Leyendo Texto
        lector.readAsText(selectedFiles[0]);
        //Mostrando mensaje
        alert('El Archivo se ha Cargado');
        //Indicando Archivo
        $("#box").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
    }

});

/**Script Contenedor de Archivos**/
//Declarando variable contenedora de Archivos
var selectedFilesFac;
//Función que limpia el Contenedor


</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Liquidaciones</h1>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBusqueda" Text="Búsqueda" OnClick="btnVista_OnClick" runat="server" CommandName="Busqueda" CssClass="boton_pestana_activo"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLiquidacion" Text="Liquidación" OnClick="btnVista_OnClick"  CommandName="Liquidacion" runat="server" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnResumen" Text="Resumen" OnClick="btnVista_OnClick"  CommandName="Resumen" runat="server" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs_300px">
<asp:UpdatePanel ID="upmtvEncabezado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvEncabezado" runat="server">
<asp:View ID="vwBusqueda" runat="server">
<div class="seccion_controles">
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Recursos</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoRecurso">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoRecurso" runat="server" CssClass="dropdown2x" AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="ddlTipoRecurso_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRecurso">Recurso</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRecursoUn" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2" Visible="true"></asp:TextBox>
<asp:TextBox ID="txtRecursoOp" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2" Visible="false"></asp:TextBox>
<asp:TextBox ID="txtRecursoPr" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarLiquidaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarLiquidaciones" runat="server" CssClass="boton" Text="Buscar" TabIndex="3" OnClick="btnBuscarLiquidaciones_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCrear" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="boton_cancelar"
OnClick="btnCrear_Click" TabIndex="4" />
</ContentTemplate>
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
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="contenedor_730px_derecha">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" TabIndex="4" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lkbExportar_Click"
TabIndex="5" CommandName="Liquidaciones"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class ="grid_seccion_completa_400px_altura">            
<asp:UpdatePanel ID="upgvLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLiquidacion" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="6" OnSorting="gvLiquidacion_Sorting"
OnPageIndexChanging="gvLiquidacion_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
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
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="CompaniaEmisora" HeaderText="Compania" SortExpression="CompaniaEmisora" />
<asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha" SortExpression="FechaLiquidacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="TotalSalario" HeaderText="Total Salario" SortExpression="TotalSalario" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalDeducciones" HeaderText="Total Deducciones" SortExpression="TotalDeducciones" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalDescuentos" HeaderText="Total Descuentos" SortExpression="TotalDescuentos" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalSueldo" HeaderText="Total Sueldo" SortExpression="TotalSueldo" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalAnticipos" HeaderText="Total Anticipos" SortExpression="TotalAnticipos" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalComprobaciones" HeaderText="Total Comprobaciones" SortExpression="TotalComprobaciones" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TotalAlcance" HeaderText="Total Alcance" SortExpression="TotalAlcance" ItemStyle-HorizontalAlign="Right" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionarLiquidacion" runat="server" Text="Seleccionar" OnClick="lnkSeleccionarLiquidacion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
<asp:View ID="vwLiquidacion" runat="server">
<div class="header_seccion">
<img src="../Image/DatosPrincipales.png" />
<h2>Datos de la Liquidación</h2>

                    <div class="promLiq">
                        <asp:UpdatePanel runat="server" ID="upimgbCalificacion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ImageButton runat="server" ID="imgbCalificacion" ImageUrl=""  OnClick="imgbCalificacion_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="comentariosLiq">
                        <asp:UpdatePanel runat="server" ID="uplkbComentarios" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton runat="server" ID="lkbComentarios" CssClass="leyenda_indicador" Font-Size="Large" OnClick="lkbComentarios_Click" ></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>


</div>
<div style="width:auto">
<div class="contenedor_media_seccion_izquierda">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoLiquidacion">No. Liquidación</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblNoLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblNoLiquidacion" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155pxr">
<asp:LinkButton ID="lkbAgregarFacturaLiq" runat="server" Text="Agregar Factura(s)" OnClick="lkbAgregarFacturaLiq_Click"></asp:LinkButton>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblEstatus">Estatus</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblEstatus" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTipoEntidad">Tipo Entidad</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblTipoEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblTipoEntidad" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblEntidad">Entidad</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblEntidad" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaLiq">Fecha</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaLiq" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100pxr">
<asp:UpdatePanel ID="upbtnCambiarCuentaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCambiarCuentaPago" runat="server" Text="Otra Cta. Pago" CssClass="boton" Enabled="true"
OnClick="btnCambiarCuentaPago_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100pxr">
<asp:UpdatePanel ID="upbtnEditar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEditar" runat="server" Text="Editar Fecha" TabIndex="9" CssClass="boton_cancelar"
OnClick="btnEditar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>    
</div>
</div>
<div class="contenedor_media_seccion_derecha">
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblPercepciones">T. Percepciones</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblPercepciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblPercepciones" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<label for="lblAnticipos">T. Anticipos</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblAnticipos" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblDeducciones">T. Deducciones</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblDeducciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDeducciones" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<label for="lblComprobaciones">T. Comprobaciones</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblComprobaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblComprobaciones" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblSueldo">T. Sueldo</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblSueldo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSueldo" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<label for="lblAlcance">T. Alcance</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblAlcance" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblAlcance" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="lblDescuentos">T. Descuentos</label>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblDescuentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDescuentos" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarLiq" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarLiq_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarLiq" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarLiq_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCerrarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCerrarLiquidacion" runat="server" Text="Cerrar Liq." CssClass="boton_cancelar"
OnClick="btnCerrarLiquidacion_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upbtnAbrirLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="pnlAbrirLiquidacion" runat="server">
<div class="controlBoton">
<asp:Button ID="btnAbrirLiq" runat="server" Text="Abrir Liq." CssClass="boton_cancelar"
OnClick="btnAbrirLiq_Click" />
</div>
</asp:Panel>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminarLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminarLiq" runat="server" CssClass="boton" Text="Eliminar" OnClick="btnEliminarLiq_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div style="width:auto">
<div class="contenedor_media_seccion_izquierda">
<div class="header_seccion">
<img src="../Image/Pagos.png" />
<h2>Pagos Asignados</h2>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCrearOtrosPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCrearOtrosPagos" runat="server" Text="Crear Pago" CommandName="OtrosPagos" OnClick="btnCrearOtrosPagos_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnDeducciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDeducciones" runat="server" Text="Deducción" CommandName="PagoNegativo" OnClick="btnCrearOtrosPagos_Click" CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoPago">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoPago" runat="server" TabIndex="19" OnSelectedIndexChanged="ddlTamanoPago_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoPago">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPago" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarPago" runat="server" Text="Exportar" CommandName="Pagos" OnClick="lkbExportar_Click"
TabIndex="20"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPagos" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="21" OnSorting="gvPagos_Sorting" OnRowDataBound="gvPagos_RowDataBound"
OnPageIndexChanging="gvPagos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate"/>
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdPago" HeaderText="No. Pago" SortExpression="IdPago" Visible="true" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="ValorU" HeaderText="Valor U." SortExpression="ValorU" />
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblSumaTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarPago" runat="server" OnClick="lnkEditarPago_Click" Text="Editar"></asp:LinkButton><br />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarPago" runat="server" OnClick="lnkEliminarPago_Click" Text="Eliminar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkBitacoraPago" runat="server" OnClick="lnkBitacoraPago_Click" Text="Bitacora"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenedor_media_seccion_derecha">
<div class="header_seccion">
<img src="../Image/CobrosRecurrentes.png" />
<h2>Cobros Recurrentes</h2>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnVerCobrosPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnVerCobrosPendientes" runat="server" Text="Ver Pendientes" OnClick="btnVerCobrosPendientes_Click"
TabIndex="35" CssClass="boton" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoCRL">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCRL" runat="server" TabIndex="32" OnSelectedIndexChanged="ddlTamanoCRL_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRL">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoCRL" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobroRecurrenteLiquidacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCRL" runat="server" Text="Exportar" CommandName="CobroRecurrenteLiquidacion" OnClick="lkbExportar_Click"
TabIndex="33"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCRL" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvCobroRecurrenteLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobroRecurrenteLiquidacion" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="34" OnSorting="gvCobroRecurrenteLiquidacion_Sorting"
OnPageIndexChanging="gvCobroRecurrenteLiquidacion_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate"/>
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
<ItemTemplate>
<asp:LinkButton ID="lnkVerHistorial" runat="server" OnClick="lnkVerHistorial_Click"
Text='<%# Eval("Descripcion") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
<asp:View ID="vwResumen" runat="server">
<div class="contenedor_seccion_95per">
<div class="header_seccion">
<h2>Visualizar Liquidación</h2>
</div>
<div class="grid_seccion_completa_400px_altura">
<asp:UpdatePanel ID="upgvResumenLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumenLiquidacion" runat="server" AllowPaging="false" AllowSorting="false"
CssClass="gridview" ShowFooter="true" AutoGenerateColumns="false"
OnRowDataBound="gvResumenLiquidacion_RowDataBound" Width="100%">
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
<asp:BoundField DataField="IdTipoRegistro" HeaderText="Tipo Registro" SortExpression="IdTipoRegistro" Visible="false" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="No" HeaderText="No" SortExpression="No" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:C2}" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon100Per">
<div class="controlr">
<asp:UpdatePanel ID="uplkbTimbrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbTimbrar" runat="server" Text="Timbrar" CommandName="Timbrar" OnClick="lkbReciboNomina_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbCancelarTimbrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCancelarTimbrado" runat="server" CommandName="Cancelar" Text="Cancelar Recibo de Nómina" OnClick="lkbReciboNomina_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplnkExportarResumenLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarResumenLiq" runat="server" Text="Exportar Resumen"
OnClick="lnkExportarResumenLiq_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarResumenLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplnkImprimirLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkImprimirLiquidacion" runat="server" Text="Imprimir Liquidación"
OnClick="lnkImprimirLiquidacion_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkImprimirLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EnTransito.png" />
<h2>Listado de Servicios</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px" style="width: auto">
<label for="ddlTamanoRecursos">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRecursos" runat="server" OnSelectedIndexChanged="ddlTamanoRecursos_SelectedIndexChanged" TabIndex="5" AutoPostBack="true" CssClass="dropdown_100px">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenarRecursos">Ordenado:</label>
</div>
<div class="etiqueta" style="width: auto">
<asp:UpdatePanel ID="uplblOrdenarRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarRecursos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplkbExportarRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRecursos" runat="server" Text="Exportar" CommandName="Recurso" OnClick="lkbExportar_Click" TabIndex="6"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRecursos" />
</Triggers>
</asp:UpdatePanel>
</div>
    
<div class="etiqueta_155px" style="width:auto;">
<asp:LinkButton ID="lnkHistorialViajes" runat="server" Text="Ver Historial Viajes" OnClick="lnkHistorialViajes_Click"></asp:LinkButton>
</div>
<div class="etiqueta_155px" style="width:auto;">
<asp:Label runat="server" ID="l" Text="  "></asp:Label>
</div>
<div class="etiqueta_155px" style="width:auto;">
<asp:LinkButton ID="lkbVerHistorialUnidad" runat="server" Text="Ver Historial Unidad"   OnClick="lkbVerHistorialUnidad_Click"></asp:LinkButton>
</div>

<div class="etiqueta_155px" style="width:auto;">
<asp:LinkButton ID="lnkControlDiesel" runat="server" Text="Control Diesel" OnClick="lnkControlDiesel_Click"></asp:LinkButton>
</div>


<div class="controlBoton">
<asp:UpdatePanel ID="upbtnPagarMovVacio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPagarMovVacio" runat="server" Text="Pagar Mov. Vacio" CssClass="boton" OnClick="btnPagarMovVacio_Click" Visible="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="contenedorServiciosLiquidacion" class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvServiciosLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosLiquidacion" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
CssClass="gridview" OnSorting="gvServiciosLiquidacion_Sorting" OnPageIndexChanging="gvServiciosLiquidacion_PageIndexChanging"
OnRowDataBound="gvServiciosLiquidacion_RowDataBound" PageSize="100" ShowFooter="true">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<Columns>
<asp:TemplateField>
<HeaderStyle HorizontalAlign="Center" />
<HeaderTemplate>
<asp:CheckBox ID="chkTodosMovimientos" runat="server" OnCheckedChanged="chkTodosMovimientos_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVariosMovimientos" runat="server" OnCheckedChanged="chkTodosMovimientos_CheckedChanged" AutoPostBack="true" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="IdServicio" HeaderText="IdServicio" SortExpression="IdServicio" Visible="false" />
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
<ItemTemplate>
<asp:LinkButton ID="lnkServicio" runat="server" Text='<%# Eval("NoServicio") %>' OnClick="lnkServicio_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:TemplateField HeaderText="Porte" SortExpression="Porte">
<ItemTemplate>
<asp:LinkButton ID="lkbPorte" runat="server" Text='<%# Eval("Porte") %>' CommandName="Origen"  OnClick="lkbOrigen_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Referencias1" HeaderText="Referencias 1" SortExpression="Referencias1" />
<asp:BoundField DataField="Referencias2" HeaderText="Referencias 2" SortExpression="Referencias2" />
<asp:BoundField DataField="Referencias3" HeaderText="Referencias 3" SortExpression="Referencias3" />
<asp:BoundField DataField="IdMovimiento" HeaderText="No. Movimiento" SortExpression="IdMovimiento" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="Kms" HeaderText="Km's" SortExpression="Kms" />
<asp:BoundField DataField="DuracionViaje" HeaderText="Duracion del Viaje" SortExpression="*DuracionViaje" />
<asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
<asp:BoundField DataField="EstatusDocumentos" HeaderText="Estatus Documentos" SortExpression="EstatusDocumentos" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:TemplateField HeaderText="Unidad" SortExpression="Unidad">
<ItemTemplate>
<asp:LinkButton ID="lkbUnidad" runat="server" Text='<%# Eval("Unidad") %>' CommandName="Unidad" OnClick="lkbEntidadLiquidacion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" />
<asp:TemplateField HeaderText="T. Pagos" SortExpression="Pagos">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkEditarPagos" runat="server" CommandName="Pagos" Text='<%#  string.Format("{0:C2}", Eval("Pagos")) %>' OnClick="lnkDetalleLiq_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="T. Anticipos" SortExpression="Anticipos">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkEditarAnticipos" runat="server" CommandName="Anticipos" Text='<%# string.Format("{0:C2}", Eval("Anticipos")) %>' OnClick="lnkDetalleLiq_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="T. Comprobaciones" SortExpression="Comprobaciones">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkEditarComprobaciones" runat="server" CommandName="Comprobaciones" Text='<%# string.Format("{0:C2}", Eval("Comprobaciones")) %>' OnClick="lnkDetalleLiq_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="T. Diesel" SortExpression="Diesel">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkDiesel" runat="server" CommandName="Diesel" Text='<%#  string.Format("{0:C2}", Eval("Diesel")) %>' OnClick="lnkDetalleLiq_Click"></asp:LinkButton>
</ItemTemplate>
<FooterStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="FacturasAnticipos" HeaderText="Facturas Anticipos" SortExpression="FacturasAnticipos" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="TFacturasAnticipos" HeaderText="T. Fact. Anticipos" SortExpression="TFacturasAnticipos" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:TemplateField HeaderText="Devoluciones" SortExpression="indDevoluciones">
<ItemTemplate>
<asp:LinkButton ID="lnkDevoluciones" runat="server" Text="Ver Devoluciones" OnClick="lnkDevoluciones_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Lectura" SortExpression="">
<ItemTemplate>
<asp:LinkButton ID="lnkLectura" runat="server" Text="Lectura" OnClick="lnkLectura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRecursos" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
</Triggers>
</asp:UpdatePanel>
</div>

<!-- Ventana Pagos -->
<div id="contenedorVentanaPagos" class="modal">
<div id="ventanaPagos" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarPagos" runat="server" OnClick="lnkCerrar_Click" CommandName="Pagos" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Pagos</h2>
</div>
<div class="columna2x">
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipoPago">Tipo de Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoPago" runat="server" CssClass="dropdown" TabIndex="37"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtCantidad">Cantidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidad" runat="server" CssClass="textbox validate[required, custom[positiveNumber4]]"
TabIndex="38" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtValorU">Valor Unitario</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValorU" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorU" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]"
TabIndex="39" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtCantidad" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtTotal">Total</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotal" runat="server" CssClass="textbox validate[required, custom[positiveNumber4]]" Enabled="false" 
TabIndex="40" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtCantidad" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
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
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="41" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" TabIndex="42" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarPago" runat="server" CssClass="boton" Text="Guardar Pago" OnClick="btnGuardarPago_Click"
TabIndex="43" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" /> 
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
<asp:AsyncPostBackTrigger ControlID="gvPagosMV" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarPago" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarPago_Click" 
TabIndex="44" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarMovVacio" />
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
<asp:AsyncPostBackTrigger ControlID="btnDeducciones" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicioManual" />
<asp:AsyncPostBackTrigger ControlID="btnCrearPagoMov" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- Ventana Confirmación Eliminación Pagos-->
<div id="contenedorVentanaConfirmacion" class="modal">
<div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>El Pago se eliminará. ¿Desea quitar los anticipos y comprobaciones?</h2>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarOperacion" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnAceptarOperacion_Click" TabIndex="55" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="gvPagosLigados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarOperacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarOperacion_Click" TabIndex="56" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- Ventana Anticipos -->
<div id="contenedorVentanaAnticipos" class="modal">
<div id="ventanaAnticipos" class="contenedor_ventana_confirmacion_arriba" style="width: 400px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarAnticipos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarAnticipos" runat="server" CommandName="Anticipos" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Depositos.png" />
<h2>Anticipos Asignados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoAnticipos">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoAnticipos" runat="server" TabIndex="22" OnSelectedIndexChanged="ddlTamanoAnticipos_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoAnticipo">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoAnticipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoAnticipo" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarAnticipos" runat="server" Text="Exportar" CommandName="Anticipos" OnClick="lkbExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarAnticipos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvAnticipos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAnticipos" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="24" OnSorting="gvAnticipos_Sorting"
OnRowDataBound="gvAnticipos_RowDataBound" AutoGenerateColumns="false" Width="100%"
OnSelectedIndexChanging="gvAnticipos_SelectedIndexChanging">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="No. Deposito" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" Visible="false" />
<asp:BoundField DataField="NoDeposito" HeaderText="No. Deposito" SortExpression="NoDeposito" />
<asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Metodo" HeaderText="Método" SortExpression="Metodo" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
<asp:BoundField DataField="Comprobacion" HeaderText="Comprobacion" SortExpression="Comprobacion" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkSeleccionarDeposito" runat="server" Text="Comprobar"
OnClick="lnkSeleccionarDeposito_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Alta Comprobaciones -->
<div id="contenedorVentanaAltaComprobaciones" class="modal">
<div id="ventanaAltaComprobaciones" style="z-index: 1002; display: none; position: fixed; background-color: #FFF; border: 1px solid #808080; left: 300px; top: 200px; width: 1000px; height: auto; padding: 10px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarAltaComprobacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarAltaComprobacion" runat="server" CommandName="AltaComprobaciones" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Comprobaciones</h2>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblIdComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblIdComprobacion" runat="server" Text="Por Asignar"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlConcepto">Concepto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown" TabIndex="46"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtObservacion">Observación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacion" runat="server" TabIndex="47" CssClass="textbox validate[required]" MaxLength="255"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtValorUnitario">Monto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValorUnitario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorUnitario" runat="server" TabIndex="48" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="15"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarComprobacion" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnGuardarComprobacion_Click" TabIndex="49" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarComprobacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarComprobacion_Click" TabIndex="50" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna">
<div id="box">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFactura" runat="server" Text="Agregar Factura" CssClass="boton"
OnClick="btnAgregarFactura_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlTamanoFacComp">Mostrar:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="upddlTamanoFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacComp" runat="server" CssClass="textbox_50px" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFacComp_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="">Ordenado:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uplblOrdenadoFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoFacComp" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacComp" runat="server" Text="Exportar" CommandName="FacturasComp"
OnClick="lkbExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="upgvFacturasComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasComprobacion" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="54" OnSorting="gvFacturasComprobacion_Sorting"
OnPageIndexChanging="gvFacturasComprobacion_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdFactura" HeaderText="Id" SortExpression="IdFactura" Visible="false" />
<asp:BoundField DataField="IdCompFact" HeaderText="IdComp" SortExpression="IdCompFact" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" />
<asp:BoundField DataField="Subtotal" HeaderText="Sub Total" SortExpression="Subtotal" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
<asp:BoundField DataField="Trasladado" HeaderText="IVA Trasladado" SortExpression="Trasladado" />
<asp:BoundField DataField="Retenido" HeaderText="IVA Retenido" SortExpression="Retenido" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eilminar"
OnClick="lnkEliminarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana Comprobaciones -->
<div id="contenedorVentanaComprobaciones" class="modal">
<div id="ventanaComprobaciones" class="contenedor_ventana_confirmacion_arriba" style="width: 400px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarComprobacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarComprobacion" runat="server" CommandName="Comprobaciones" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/DepositosVale.png" />
<h2>Comprobaciones Realizadas</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoComp">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoComp" runat="server" OnSelectedIndexChanged="ddlTamanoComp_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoComp">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoComp" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarComp" runat="server" Text="Exportar" CommandName="Comprobaciones" OnClick="lkbExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvComprobaciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvComprobaciones" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="27" OnSorting="gvComprobaciones_Sorting"
OnPageIndexChanging="gvComprobaciones_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
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
<asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
<asp:BoundField DataField="NoFacturas" HeaderText="No. Facturas" SortExpression="NoFacturas" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditaComp" runat="server" Text="Editar"
OnClick="lnkEditaComp_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminaComp" runat="server" Text="Eliminar"
OnClick="lnkEliminaComp_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Vales De Diesel -->
<div id="contenedorVentanaDiesel" class="modal">
<div id="ventanaDiesel" class="contenedor_ventana_confirmacion_arriba" style="width: 400px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarValesDiesel" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarValesDiesel" runat="server" CommandName="Diesel" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EstacionCombustible.png" />
<h2>Vales de Diesel Asignados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoDiesel">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDiesel" runat="server" TabIndex="29" OnSelectedIndexChanged="ddlTamanoDiesel_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoAnticipo">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoDiesel" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDiesel" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDiesel" runat="server" Text="Exportar" CommandName="Diesel" OnClick="lkbExportar_Click"
TabIndex="30"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDiesel" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="31" OnSorting="gvDiesel_Sorting"
OnPageIndexChanging="gvDiesel_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate"/>
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="No. Deposito" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoVale" HeaderText="No. Vale" SortExpression="NoVale" />
<asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
<asp:BoundField DataField="Costo" HeaderText="Costo Combustible" SortExpression="Costo" />
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Cobros Recurrentes Pendientes-->
<div id="contenedorVentanaCobrosPendientes" class="modal">
<div id="ventanaCobrosPendientes" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarVentanaCRP" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaCRP" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar" CommandName="CobrosRecurrentesPend" TabIndex="57">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Cobros Recurrentes</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoCR">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCR" runat="server" TabIndex="58" OnSelectedIndexChanged="ddlTamanoCR_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCR">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCR" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarCR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCR" runat="server" Text="Exportar" CommandName="CobrosRecurrentes" OnClick="lkbExportar_Click"
TabIndex="59"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCR" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvCobrosRecurrentes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobrosRecurrentes" runat="server" AllowPaging="true" AllowSorting="true" Width="100%"
CssClass="gridview" ShowFooter="true" TabIndex="60" OnSorting="gvCobrosRecurrentes_Sorting"
OnPageIndexChanging="gvCobrosRecurrentes_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
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
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="FechaUltimoCobro" HeaderText="Fecha Ultimo Cobro" SortExpression="FechaUltimoCobro" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="DiasTranscurridos" HeaderText="Dias Transcurridos" SortExpression="DiasTranscurridos" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="DiasCobro" HeaderText="Dias Cobro" SortExpression="DiasCobro" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:TemplateField SortExpression="EstatusInd">
<ItemTemplate>
<asp:LinkButton ID="lnkEstatus" runat="server" Text='<%# Eval("EstatusInd") %>' CommandName='<%# Eval("EstatusInd") %>'
OnClick="lnkEstatus_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCR" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminaLiq" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Tarifas Pago-->
<div id="contenedorVentanaTarifasPago" class="modal">
<div id="ventanaTarifasPago" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaTarifasPago" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVentanaTarifasPago" OnClick="lnkCerrar_Click" CommandName="TarifasAplicables" runat="server" Text="Cerrar" TabIndex="61">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Tarifas de Pago Aplicables</h2>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnPagarServicioManual" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPagarServicioManual" runat="server" Text="Pago Manual" OnClick="btnPagarServicioManual_Click" CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoGridViewTarifasAplicables">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoGridViewTarifasAplicables" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGridViewTarifasAplicables" runat="server" TabIndex="62"
CssClass="dropdown_100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoGridViewTarifasAplicables_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoTarifasAplicables">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoTarifasAplicables" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoTarifasAplicables" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplkbExportarTarifasAplicales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarTarifasAplicales" runat="server" Text="Exportar" CommandName="TarifasAplicables" OnClick="lkbExportar_Click"
TabIndex="63"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarTarifasAplicales" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvTarifasPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvTarifasPago" runat="server" AllowPaging="true" AllowSorting="true" Width="100%"
CssClass="gridview" ShowFooter="true" TabIndex="64" OnSorting="gvTarifasPago_Sorting"
OnPageIndexChanging="gvTarifasPago_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="100%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdTarifa" HeaderText="Id" SortExpression="IdTarifa" />
<asp:BoundField DataField="Aproximacion" HeaderText="Aproximación" SortExpression="Aproximacion" ItemStyle-HorizontalAlign="Center" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Base" HeaderText="Base" SortExpression="Base" />
<asp:BoundField DataField="NivelPago" HeaderText="NivelPago" SortExpression="NivelPago" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbSeleccionarTarifaPago" runat="server" Text="Aplicar" OnClick="lkbSeleccionarTarifaPago_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGridViewTarifasAplicables" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnPagarServicio" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Referencias de Viaje -->
<div id="contenedorVentanaReferenciasViaje" class="modal">
<div id="ventanaReferenciasViaje" class="contenedor_ventana_confirmacion" style="width:300px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" CommandName="ReferenciasViaje" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
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
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana Confirmación Timbrar Liquidación -->
<div id="contenidoConfirmacionTimbrarLiquidacion" class="modal">
<div id="confirmacionTimbrarLiquidacion"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarLiquidacion" UpdateMode="Conditional"  >
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTimbrarLiquidacion" runat="server" Text="Cerrar" CommandName="TimbrarLiquidacion" OnClick="lnkCerrar_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>Timbrar Liquidacion</h3>
<div class="columna"> 
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTimbrarLiquidacion">Sucursal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server"   CssClass="dropdown2x" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="upddlPeriocidadPago">Periocidad Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlPeriocidadPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPeriocidadPago" runat="server"   CssClass="dropdown" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="upddlMetodoPago">Método Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server"   CssClass="dropdown" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorTimbrarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorTimbrarLiquidacion" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbrarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbrarLiquidacion" runat="server"  OnClick="btnAceptarTimbrarLiquidacion_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>
</div>

<!-- Ventana Confirmación Cancelar Timbrado -->
<div id="contenidoConfirmacionCancelarTimbrado" class="modal">
<div id="confirmacionCancelarTimbrado"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCancelarTimbrado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCancelarTimbrado" runat="server" Text="Cerrar" CommandName="CancelarTimbrado" OnClick="lnkCerrar_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>CancelarTimbrado</h3>
<div class="columna"> 
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea cancelar el Recibo de Nómina?</label>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorCancelarTimbrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorCancelarTimbrado" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCancelarTimbrado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarTimbrado" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCancelarTimbrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelarTimbrado" runat="server"   OnClick="btnAceptarCancelarTimbrado_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>

<!-- Ventana Movimientos por Servicio -->
<div id="contenedorVentanaMovimientos" class="modal">
<div id="ventanaMovmientos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarMovimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarMovimientos" runat="server" CommandName="Movimientos" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/EntradasSalidas.png" />
<h2>Movimientos Pendientes</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoMov">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoMov" runat="server" TabIndex="15" OnSelectedIndexChanged="ddlTamanoMov_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoMov">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoMov" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarMov" runat="server" CommandName="Movimientos" Text="Exportar" OnClick="lkbExportar_Click"
TabIndex="16"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarMov" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvMovimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvMovimientos" runat="server" AllowPaging="True" AllowSorting="True" Width="100%"
CssClass="gridview" ShowFooter="True" TabIndex="17" OnSorting="gvMovimientos_Sorting" PageSize="25"
OnPageIndexChanging="gvMovimientos_PageIndexChanging" AutoGenerateColumns="False"
OnRowDataBound="gvMovimientos_RowDataBound">
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
<asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodos_CheckedChanged" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" AutoPostBack="true"
OnCheckedChanged="chkTodos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="No. Mov." SortExpression="Id" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Estatus Documentos" SortExpression="EstatusDoc">
<ItemTemplate>
<asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="lnkEvidencias_Click" Text='<%# Eval("EstatusDoc") %>'>
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Kms" HeaderText="Km's" SortExpression="Kms" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="NoPago" HeaderText="No. Pago" SortExpression="NoPago" Visible="false" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkPagarMov" runat="server" Text="" OnClick="lnkPagarMov_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnPagarServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPagarServicio" runat="server" Text="Pagar Servicio" CssClass="boton" OnClick="btnPagarServicio_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCrearPagoMov" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCrearPagoMov" runat="server" Text="Crear Pago" CssClass="boton" OnClick="btnCrearPagoMov_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x" style="float:right">
<div class="header_seccion">
<img src="../Image/Pagos.png" />
<h2>Pagos Ligados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoPagosLigados">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoPagosLigados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoPagosLigados" runat="server" TabIndex="15" OnSelectedIndexChanged="ddlTamanoPagosLigados_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoMov">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoPagoLigado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPagoLigado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarPagosLigados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarPagosLigados" runat="server" CommandName="PagosLigados" Text="Exportar" OnClick="lkbExportar_Click"
TabIndex="16"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarPagosLigados" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvPagosLigados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPagosLigados" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="21" OnSorting="gvPagosLigados_Sorting"
OnPageIndexChanging="gvPagosLigados_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate"/>
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdPago" HeaderText="No. Pago" SortExpression="IdPago" Visible="true" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="ValorU" HeaderText="Valor U." SortExpression="ValorU" />
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblSumaTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarPagoLigado" runat="server" OnClick="lnkEditarPagoLigado_Click" Text="Editar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarPagoLigado" runat="server" OnClick="lnkEliminarPagoLigado_Click" Text="Eliminar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkBitacoraPagoLigado" runat="server" OnClick="lnkBitacoraPagoLigado_Click" Text="Bitacora"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPagosLigados" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana Evidencias de Viaje -->
<div id="contenidoVentanaEvidencias" class="modal">
<div id="ventanaEvidencias" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" CommandName="Evidencias" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion"> 
<h2>Evidencias del Viaje</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoEvidencia">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoEvidencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoEvidencia" runat="server" TabIndex="65" OnSelectedIndexChanged="ddlTamanoEvidencia_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRH">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoEvidencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoEvidencias" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEvidencias" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarEvidencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarEvidencia" runat="server" Text="Exportar" CommandName="Evidencias" OnClick="lkbExportar_Click"
TabIndex="66"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarEvidencia" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvEvidencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEvidencias" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="67" OnSorting="gvEvidencias_Sorting"
OnPageIndexChanging="gvEvidencias_PageIndexChanging" AutoGenerateColumns="false"
OnRowDataBound="gvEvidencias_RowDataBound">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="ibEstatus" runat="server" ImageUrl="~/Image/EstatusRecibido.png" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
<asp:BoundField DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Remitente" HeaderText="Remitente" SortExpression="Remitente" />
<asp:BoundField DataField="Destinatario" HeaderText="Destinatario" SortExpression="Destinatario" />
<asp:BoundField DataField="LugarCobro" HeaderText="Lugar de Cobro" SortExpression="LugarCobro" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoEvidencia" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Historial Cobros Recurrentes -->
<div id="contenedorVentanaHistorialCobrosRecurrentes" class="modal">
<div id="ventanaHistorialCobrosRecurrentes" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarVentanaCRH" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaCRH" runat="server" CommandName="CobroRecurrenteHistorial" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="61">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Historial Cobros Recurrentes</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoCRH">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCRH" runat="server" TabIndex="62" OnSelectedIndexChanged="ddlTamanoCRH_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRH">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCRH" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentesHistorial" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCRH" runat="server" Text="Exportar" OnClick="lkbExportar_Click" CommandName="CobroRecurrenteHistorial"
TabIndex="63"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCRH" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvCobrosRecurrentesHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobrosRecurrentesHistorial" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="64" OnSorting="gvCobrosRecurrentesHistorial_Sorting" Width="100%"
OnPageIndexChanging="gvCobrosRecurrentesHistorial_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarLiq" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarLiquidaciones" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" /><asp:AsyncPostBackTrigger ControlID="btnAbrirLiq" />
<asp:AsyncPostBackTrigger ControlID="gvCobroRecurrenteLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Confirmación Eliminación Liquidación (Pagos y Comprobaciones)-->
<div id="contenedorVentanaConfirmacionEliminaLiquidacion" class="modal">
<div id="ventanaConfirmacionEliminaLiquidacion" class="contenedor_ventana_confirmacion_arriba">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Se eliminaran todos los Pagos y Comprobaciones. ¿Desea continuar?</h2>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminaLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminaLiq" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnAceptarEliminaLiq_Click" TabIndex="55" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminaLiq" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminaLiq" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarEliminaLiq_Click" TabIndex="56" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- Ventana Resumen Devoluciones -->
<div id="contenedorVentanaResumenDevoluciones" class="modal">
<div id="ventanaResumenDevoluciones" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarDev" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarDev" runat="server" CommandName="Devolucion" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="61">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/EnvioRecepcion.png" />
<h2>Devoluciones por Viaje</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoDev">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoDev" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDev" runat="server" TabIndex="62" OnSelectedIndexChanged="ddlTamanoDev_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoDev">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoDev" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoDev" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarDevolucion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDevolucion" runat="server" Text="Exportar" OnClick="lkbExportar_Click" CommandName="Devolucion"
TabIndex="63"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarDevolucion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvDevoluciones" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDevoluciones" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="64" OnSorting="gvDevoluciones_Sorting" Width="100%"
OnPageIndexChanging="gvDevoluciones_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField HeaderText="No. Devolución" SortExpression="NoDevolucion">
<ItemTemplate>
<asp:LinkButton ID="lkbNoDevolucion" runat="server" OnClick="lkbNoDevolucion_Click" Text='<%# Eval("NoDevolucion") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaDevolucion" HeaderText="Fecha Devolucion" SortExpression="FechaDevolucion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" />
<asp:BoundField DataField="UbicacionDevolucion" HeaderText="Ubicacion Devolución" SortExpression="UbicacionDevolucion" />
<asp:BoundField DataField="CodProd" HeaderText="Código Producto" SortExpression="CodProd" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="RazonDetalle" HeaderText="Razon Detalle" SortExpression="RazonDetalle" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDev" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana de Devolución Faltante -->
<div id="modalDevolucionFaltante" class="modal">
<div id="devolucionFaltante" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDevolucion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDevolucion" runat="server" OnClick="lnkCerrar_Click" CommandName="AltaDevolucion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucDevolucionFaltante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucDevolucionFaltante ID="wucDevolucionFaltante" runat="server" OnClickGuardarDevolucion="wucDevolucionFaltante_ClickGuardarDevolucion" 
OnClickGuardarDevolucionDetalle="wucDevolucionFaltante_ClickGuardarDevolucionDetalle" TabIndex="65"
OnClickEliminarDevolucionDetalle="wucDevolucionFaltante_ClickEliminarDevolucionDetalle" 
OnClickAgregarReferenciasDevolucion="wucDevolucionFaltante_ClickAgregarReferenciasDevolucion"
OnClickAgregarReferenciasDetalle="wucDevolucionFaltante_ClickAgregarReferenciasDetalle" Contenedor="#devolucionFaltante" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana Pago Movimiento Vacio -->
<div id="contenedorVentanaPagoMovVacio" class="modal">
<div id="ventanaPagoMovVacio" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarPagosMV" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarPagosMV" runat="server" OnClick="lnkCerrar_Click" CommandName="PagosMV" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Pagos.png" />
<h2>Pagos de Movimientos en Vacio</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoPagoMV">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoPagoMV" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoPagoMV" runat="server" TabIndex="66" OnSelectedIndexChanged="ddlTamanoPagoMV_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoPagoMV">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoPagoMV" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoPagoMV" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPagos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplkbExportarPagoMV" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarPagoMV" runat="server" Text="Exportar" CommandName="PagosMV" OnClick="lkbExportar_Click"
TabIndex="67"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarPagoMV" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvPagosMV" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPagosMV" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="68" OnSorting="gvPagosMV_Sorting"
OnPageIndexChanging="gvPagosMV_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate"/>
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdPago" HeaderText="No. Pago" SortExpression="IdPago" Visible="true" />
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="ValorU" HeaderText="Valor U." SortExpression="ValorU" />
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblSumaTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarPagoMV" runat="server" OnClick="lnkEditarPagoMV_Click" Text="Editar"></asp:LinkButton><br />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarPagoMV" runat="server" OnClick="lnkEliminarPagoMV_Click" Text="Eliminar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPagoMV" />
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- Ventana Historial Vencimiento -->
<div id="contenedorVentanaHistorialVencimientos" class="modal">
<div id="ventanaHistorialVencimientos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarHistorialVencimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarHistorialVencimiento" runat="server" OnClick="lnkCerrar_Click" CommandName="HistorialVencimiento" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimientosHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucVencimientosHistorial ID="wucVencimientosHistorial" runat="server" OnbtnNuevoVencimiento="wucVencimientosHistorial_btnNuevoVencimiento"
OnlkbConsultar="wucVencimientosHistorial_lkbConsultar" OnlkbTerminar="wucVencimientosHistorial_lkbTerminar" Contenedor="#ventanaHistorialVencimientos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- Ventana Cambio de Cuenta de Pago -->
<div id="contenedorVentanaCambioCuentaPagoModal" class="modal">
<div id="contenedorVentanaCambioCuentaPago" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCambioCuenta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCambioCuenta" runat="server" OnClick="lnkCerrar_Click" CommandName="CerrarCambioCuentaPago" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNuevaCuentaPago">Otra Cta. de Pago: </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNuevaCuentaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNuevaCuentaPago" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCambiarCuentaPago" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCuentaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCuentaPago" runat="server" OnClick="btnAceptarCuentaPago_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana Facturación Web Service -->
<div id="contenedorVentanaFacturacionWebService" class="modal">
<div id="ventanaFacturacionWebService" class="contenedor_ventana_confirmacion_arriba">
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
<asp:UpdatePanel ID="upbtnCancelarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!--Ventana Calificacion-->
<div id="contenedorVentanaCalificacion" class="modal">
<div id="ventanaCalificacion" class="contenedor_modal_seccion_completa" style="left: 327px; width: 687px; top: 35px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCalificacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCalificacion" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraCalificacion">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucCalificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCalificacion ID="wucCalificacion" runat="server" OnClickGuardarCalificacionGeneral="wucCalificacion_ClickGuardarCalificacionGeneral"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="imgbCalificacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Fin Ventana Modal Calificacion-->
<!--Ventana Historial Calificacion-->
<div id="contenedorVentanaHistorialCalificacion" class="modal">
<div id="ventanaHistorialCalificacion" class="contenedor_modal_seccion_completa" style="left:250px; width:805px; top:100px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorial" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorial" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraHistorialCalificacion">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucHistorialCalificacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucHistorialCalificacion ID="wucHistorialCalificacion" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbComentarios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Ventana Historial Calificacion-->
<div id="contenedorVentanaResumenParadas" class="modal">
<div id="ventanaResumenParadas" class="contenedor_modal_seccion_completa" style="height:320px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbnCerrarResumenParadas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarResumenParadas" runat="server" Text="Cerrar"  OnClick="lkbnCerrarResumenParadas_Click" CommandName="CierraHistorialCalificacion">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div  class="seccion_controles">
<div class="header_seccion">
<img alt="" src="../Image/ResumenReporte.png" />
<h2>Resumen</h2></div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadas"   ShowFooter="true"  runat="server" AutoGenerateColumns="false" PageSize="15" AllowPaging="true" AllowSorting="false" TabIndex="10"
ShowHeaderWhenEmpty="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="Secuencia" HeaderText="No." SortExpression="Secuencia" DataFormatString="{0:0}" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Origen" HeaderText="Parada" SortExpression="Origen" />
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita"   DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Llegada" SortExpression="FechaLlegada"  DataFormatString="{0:dd/MM/yyyy HH:mm}"  />
<asp:BoundField DataField="FechaSalida" HeaderText="Fecha Salida" SortExpression="FechaSalida"  DataFormatString="{0:dd/MM/yyyy HH:mm}"  />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Rem1" HeaderText="Rem 1" SortExpression="Rem1" />
<asp:BoundField DataField="Rem2" HeaderText="Rem 2" SortExpression="Rem2" />
<asp:BoundField DataField="Dolly" HeaderText="Dolly" SortExpression="Dolly" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Trans" HeaderText="Transportista" SortExpression="Trans" />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms" />
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
   </div>
    </div>   


<!--Ventana Lectura-->
<div id="contenedorVentanaLectura" class="modal">
<div id="ventanaLectura" class="contenedor_modal_seccion_completa" style="left: 327px; width: 720px; top: 35px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarLectura" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarLectura" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraLectura">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucLectura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucLectura ID="wucLectura" runat="server" OnClickEliminarLectura="wucLectura_ClickEliminarLectura" OnClickGuardarLectura="wucLectura_ClickGuardarLectura" /> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="wucLecturaHistorial" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Fin Ventana Modal Lectura-->

<!--Ventana LecturaHistorial-->
<div id="contenedorVentanaLecturaHistorial" class="modal">
<div id="ventanaLecturaHistorial" class="contenedor_modal_seccion_completa" style="left: 327px; width: 720px; top: 35px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarLecturaHistorial" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarLecturaHistorial" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraLecturaHistorial">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucLecturaHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucLecturaHistorial ID="wucLecturaHistorial" runat="server"  OnbtnNuevaLectura="wucLecturaHistorial_btnNuevaLectura" OnlkbConsultar="wucLecturaHistorial_lkbConsultar" /> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServiciosLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Fin Ventana Modal Lectura-->

<!--Ventana ControlDiesel-->
<div id="contenedorVentanaControlDiesel" class="modal">
<div id="ventanaControlDiesel" class="contenedor_modal_seccion_completa" style="left: 327px; width: 760px; top: 35px;padding:0px 0px 20px 20px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCierraControlDiesel" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCierraControlDiesel" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraControlDiesel">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucControlDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucControlDiesel ID="wucControlDiesel" runat="server"/> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkControlDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Fin Ventana Modal ControlDiesel-->


</asp:Content>
