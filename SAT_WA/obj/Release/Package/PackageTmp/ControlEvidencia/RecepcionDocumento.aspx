<%@ Page Title="Recepción de Documentos" Language="C#" AutoEventWireup="true" CodeBehind="RecepcionDocumento.aspx.cs"  MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.ControlEvidencia.RecepcionDocumento" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagPrefix="uc1" TagName="wucKilometraje" %>
<%@ Register Src="~/UserControls/wucEncabezadoServicio.ascx" TagPrefix="uc1" TagName="wucEncabezadoServicio" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="uc1" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagPrefix="uc1" TagName="wucFacturadoConcepto" %>
<%@ Register Src="~/UserControls/wucDevolucionFaltante.ascx" TagName="wucDevolucionFaltante" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagPrefix="tectos" TagName="wucClasificacion" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <!-- Estilos -->
<link href="../CSS/Controles.css" rel="stylesheet"  type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet"  type="text/css" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
<!--Bibliotecas para Autocomplete, Imagen y Validación  -->
<script type="text/javascript" src="../Scripts/jquery.jqzoom-core.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script> 
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" ></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
<script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">  
    <!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryRecepcionDocumentos();
}
}

//Creando función para configuración de jquery en formulario
function ConfiguraJQueryRecepcionDocumentos() {
      
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCerrar.ClientID%>").unbind("click");
$("#<%= btnCerrar.ClientID%>").click(function (evt) {
evt.preventDefault()
//Ocultando Ventana Modal 
$("#contenidoResumenSegmentos").animate({ width: "toggle" });
$("#modalResumenSegmentos").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelar.ClientID%>").unbind("click");
$("#<%= btnCancelar.ClientID%>").click(function (evt) {
evt.preventDefault()
//Ocultando ventana modal 
$("#contenidoConfirmacionRecibeDocumentos").animate({ width: "toggle" });
$("#confirmacionRecibeDocumentos").animate({ width: "toggle" });
});
//Función de validación 
var validacionBuscarEvidencias = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3 && isValid4;
};

//Función de validación Hacer Servicio
var validacionHacerServicio = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtClienteHacerServicio.ClientID%>").validationEngine('validate');

return isValid1
};

    //Función de validación de campos
    var validacionNoFacturable = function (evt) {
        var isValidP1 = !$("#<%=txtMotivoNoFacturable.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
//Botón Buscar
$("#<%=  btnBuscar.ClientID %>").click(validacionBuscarEvidencias);   
//Botón No Facturable 
$("#<%=btnNoFacturable.ClientID %>").click(validacionNoFacturable);
//Botón Aceptar Hacer Servicio
$("#<%= btnAceptarHacerServicio.ClientID %>").click(validacionHacerServicio);
// *** Catálogos Autocomplete *** //
$(document).ready(function () {
$("#<%=txtOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>'});
$("#<%=txtUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
$("#<%=txtClienteHacerServicio.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
appendTo: "#confirmacionHacerServicio"
});
//Cliente
$("#<%=txtCliente.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
});

    //Añadiendo Encabezado Fijo
    $("#<%=gvServicios.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorRecepcionEvidenciaServicios").offsetWidth - 15,
        height: 400,
        freezesize: 4
    });

    // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
    $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    $("#<%=txtFechaFin.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });

    /** Carga Clasificaciones del Servicio **/
    $("#<%=lbxTerminal.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });
    $("#<%=lbxOperacionServicio.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });
    $("#<%=lbxAlcance.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });

});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryRecepcionDocumentos();

</script>
<div id="encabezado_forma">
<img src="../Image/Evidencia.png" />
<h1>Recepción de Documentos</h1>
</div>
<nav id="menuForma">
<ul>
<li class="yellow">
<a href="#" class="fa fa-flag-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbImpresionDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImpresionDocumentos" runat="server" Text="Impresión Documentos" OnClick="lkbMenuOperacion_Click" CommandName="ImpresionDocumentos" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbImpresionDocumentos" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
</ul>
</nav>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/BusquedaEvidencia.png" />
<h2>Buscar evidencias</h2> 
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="1" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:CheckBox ID="chkSoloServicios" runat="server" Text="Sólo Servicios" CssClass="label_negrita" TabIndex="2" />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="3" MaxLength="500"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" TabIndex="4" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar x Fecha"
Checked="false" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" TabIndex="5" />
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaCarga" runat="server" CssClass="label" Text="Cita Carga" GroupName="FiltroFecha" TabIndex="6" Checked="true" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaDescarga" runat="server" CssClass="label" Text="Cita Descarga" GroupName="FiltroFecha" TabIndex="7" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>   
</div>
<div class="renglon2x">
<div class="etiqueta"></div>
<div class="control">
<asp:UpdatePanel ID="uprdbInicioServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbInicioServicio" runat="server" CssClass="label" Text="Inicio Servicio" GroupName="FiltroFecha" TabIndex="8" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>    
<div class="control">
<asp:UpdatePanel ID="uprdbFinServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbFinServicio" runat="server" CssClass="label" Text="Fin Servicio" GroupName="FiltroFecha" TabIndex="9" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div> 
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Desde</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Hasta</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="11" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTerminal">Terminal C.</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UPddlTerminal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTerminal" Enabled="false" runat="server" CssClass="dropdown2x" TabIndex="15" Visible="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtPorte">Porte</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPorte" runat="server" TabIndex="12" CssClass="textbox" MaxLength="50"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOperador">Operador</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="13" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnidad">Unidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="14" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxTerminal">Terminal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxTerminal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxTerminal" SelectionMode="multiple" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxOperacionServicio">Operación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxOperacionServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxOperacionServicio" SelectionMode="multiple" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxAlcance">Alcance</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxAlcance" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxAlcance" SelectionMode="multiple" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<div class="control_100px">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" TabIndex="16" OnClick="btnBuscar_OnClick" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">              
<div class="header_grid">
<img src="../Image/Servicio.png" />
<h3>Viajes encontrados</h3>
</div>
<div class="renglon4x">
<div class="etiqueta_50px">
<label for="ddlTamanoServicios">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoServicios" runat="server" OnSelectedIndexChanged="ddlTamañoServicios_OnSelectedIndexChanged" TabIndex="17" AutoPostBack="true" CssClass="dropdown_100px">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarServicios">Ordenado Por:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenarServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarServicios" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel runat="server" ID="uplkbExportarServicios" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar" TabIndex="18" OnClick="lkbExportarServicios_OnClick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRecibirSeleccion" runat="server">
<ContentTemplate>
<asp:Button ID="btnRecibirSeleccion" runat="server" Text ="Recibir Seleccionados" CssClass="boton2x" TabIndex="19" OnClick="btnRecibirSeleccion_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorRecepcionEvidenciaServicios">
<asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServicios" CssClass="gridview" OnPageIndexChanging="gvServicios_PageIndexChanging" 
OnSorting="gvServicios_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
ShowFooter="True" PageSize="25" Width="100%" OnRowDataBound="gvServicios_RowDataBound" TabIndex="20">
<Columns>
<asp:TemplateField HeaderText="Sel." ItemStyle-Width="50px">
<FooterTemplate>
<asp:Label ID="lblContadorSeleccionados" runat="server" Text="0"></asp:Label> Selecc.
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkSeleccionarTodos" runat="server" Text="Todos" AutoPostBack="true" OnCheckedChanged="chkSeleccionarTodos_CheckedChanged" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSeleccionarServicio" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleccionarServicio_CheckedChanged" />
</ItemTemplate>
<HeaderStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField  HeaderText="Viaje"  SortExpression="Viaje" ItemStyle-Width="100px">
<ItemTemplate>
<asp:LinkButton ID="lkbSegmentos" CommandName="Segmentos" runat="server" Text='<%# Eval("Viaje") %>' OnClick="lkbSegmentos_Click" CssClass="si" ToolTip="Ver Resumen por Segmentos del Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField  HeaderText="Movimiento"  SortExpression="Movimiento" ItemStyle-Width="100px">
<ItemTemplate>
<asp:LinkButton ID="lkbMovimiento" runat="server" Text='<%# Eval("Movimiento") %>' CssClass="si"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<%--<asp:BoundField DataField="OperacionAlcance" HeaderText="Operacion/Alcance" SortExpression="OperacionAlcance" ItemStyle-CssClass="label_error" />--%>
<asp:TemplateField HeaderText="OperacionAlcance" SortExpression="OperacionAlcance">
<ItemTemplate>
<asp:LinkButton ID="lkbOperacionAlcance" runat="server" CommandName="OperacionAlcance" OnClick="lkbAccionServicio_Click" Text='<%#Eval("OperacionAlcance") %>' ToolTip="Clasificacion Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Carta Porte" SortExpression="CartaPorte">
<ItemTemplate>
<asp:LinkButton ID="lkbCartaPorteServ" runat="server" CommandName="CartaPorte" OnClick="lkbAccionServicio_Click" Text='<%#Eval("CartaPorte") %>' ToolTip="Ver y Editar Encabezado del Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
<ItemTemplate>
<asp:LinkButton ID="lkbReferenciaServ" runat="server" CommandName="ReferenciaCliente" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Referencia") %>' ToolTip="Ver y Añadir Referencias de Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:TemplateField HeaderText="Paradas" SortExpression="Paradas">
<ItemTemplate>
<asp:LinkButton ID="lkbParadas" runat="server" CommandName="Paradas" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Paradas") %>' ToolTip="Total de Paradas Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:TemplateField HeaderText="Kms" SortExpression="Kms">
<ItemTemplate>
<asp:LinkButton ID="lkbKmsServ" runat="server" CommandName="Kms" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Kms") %>' ToolTip="Calcular o Registrar Kilometraje del Movimiento"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" ItemStyle-CssClass="label_negrita" ItemStyle-Width="140" HeaderStyle-Width="135" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" ItemStyle-CssClass="label_error" />
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" ItemStyle-CssClass="label_correcto" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="SubTotal" DataFormatString="{0:c}" HeaderText="SubTotal" SortExpression="SubTotal">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Trasladado" DataFormatString="{0:c}" HeaderText="Trasladado" SortExpression="Trasladado">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Retenido" DataFormatString="{0:c}" HeaderText="Retenido" SortExpression="Retenido">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Total" SortExpression="Total">
<ItemTemplate>
<asp:LinkButton ID="lkbTotalServ" runat="server" CommandName="Total" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Total","{0:c}") %>' ToolTip="Ver y Editar Cargos del Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Tarifa">
<ItemTemplate>
<asp:LinkButton ID="lkbTarifaServ" runat="server" Text="Calcular" OnClick="lkbAccionServicio_Click" CommandName="Tarifa" ToolTip="Busca o Aplica la Tarifa(s) de Cobro ajustadas al Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Estatus" SortExpression="Estatus">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatus" runat="server" CommandName="Estatus" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Estatus") %>' ToolTip="Rercibir Documentos con Digitalización"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="InicioViaje" HeaderText="Inicio Viaje"
SortExpression="InicioViaje" DataFormatString="{0:yyyy/MM/dd HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FinViaje" HeaderText="Fin Viaje"
SortExpression="FinViaje" DataFormatString="{0:yyyy/MM/dd HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="InicioControlEvidencia"
HeaderText="Inicio C. Evidencia" SortExpression="InicioControlEvidencia"
DataFormatString="{0:yyyy/MM/dd HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Dias" HeaderText="Dias" SortExpression="Dias">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField   HeaderText ="Documentos" SortExpression="Documentos">
<ItemTemplate>
<asp:LinkButton ID="lkbDocumentos" runat="server" Text="Ver" OnClick="lkbIniciarRecepcion_Click" ToolTip="Ver Documentos Recibidos y Pendientes"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center"/>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbNoFacturable" runat="server" Text="No Facturable" OnClick="lkbNoFacturable_Click" ToolTip="Vuelve el Servicio No Facturable"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center"/>
</asp:TemplateField>
<asp:TemplateField SortExpression="Devoluciones" >
<ItemTemplate>
<asp:LinkButton ID="lkbDevolucion" runat="server" Text='<%# Eval("Devoluciones") %>' OnClick="lkbDevolucion_Click" ToolTip="Muestra la Devolución del Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center"/>
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServicios" />
<asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="wucKilometraje" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="wucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
<asp:AsyncPostBackTrigger ControlID="btnNoFacturable" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRecepcionMultiple" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarRecepcionMultiple" />
<asp:AsyncPostBackTrigger ControlID="gvDocumentosDigitalizados" />
<asp:AsyncPostBackTrigger ControlID="btnRecibirDD" />
<asp:AsyncPostBackTrigger ControlID="wucClasificacion" />
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
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
<asp:TemplateField HeaderStyle-Width="10%">
<FooterTemplate>
<asp:Label ID="lblSeleccionadosDoc" runat="server" Text="0"></asp:Label>
<br />
Sel.
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true" CssClass="LabelResalta" OnCheckedChanged="chkTodosDocumentosDigitalizados_CheckedChanged"
Text="TODOS" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" AutoPostBack="true"  OnCheckedChanged="chkTodosDocumentosDigitalizados_CheckedChanged"/>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<HeaderStyle Width="10%" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnRecibirDD" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentosDigitalizados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRecibirDD" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRecibirDD" runat="server" TabIndex="14"  OnClick="btnRecibirDD_Click"
CssClass="boton" Text="Recibir" Enabled="False" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
 <asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnRecibirDD" />
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnRecibirDD" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
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
            <asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnRecibirDD" />
             <asp:AsyncPostBackTrigger ControlID="btnPestanaRecibirDocumentosDigitalizados" />
             <asp:AsyncPostBackTrigger ControlID="btnPestanaDocumentosDigitalizados" />
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
        </Triggers>       
    </asp:UpdatePanel>
    </div>    
<div class="columna2x">            
<div class="renglon2x">                    
<div class="control">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" CssClass="textbox2x" runat="server" Enabled="false" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- VENTANA MODAL DE ESTATUS DE DOCUMENTOS ENCONTRADOS -->
<div id="contenedorVentanaDocumentosEncontrados" class="modal">
<div id="ventanaDocumentosEncontrados" class="contenedor_modal_seccion_completa">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" CommandName="DocumentosEncontrados">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h3>Documentos encontrados</h3>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoDocumentos">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDocumentos" runat="server" OnSelectedIndexChanged="ddlTamañoDocumentos_OnSelectedIndexChanged" TabIndex="11" AutoPostBack="true" CssClass="dropdown">
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
<asp:UpdatePanel ID="uplblDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDocumentos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDocumentos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uplkbExportarDocumentos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarDocumentos" runat="server" Text="Exportar Excel" OnClick="lkbExportarDocumentos_OnClick" TabIndex="12"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarDocumentos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDocumentos" CssClass="gridview" runat="server" AllowPaging="True" OnPageIndexChanging="gvDocumentos_PageIndexChanging" OnSorting="gvDocumentos_Sorting"
AllowSorting="True" AutoGenerateColumns="false" TabIndex="13"
ShowFooter="True" PageSize="5" Width="100%">
<Columns>
<asp:TemplateField HeaderStyle-Width="10%">
<FooterTemplate>
<asp:Label ID="lblSeleccionadosDoc" runat="server" Text="0"></asp:Label>
<br />
Sel.
</FooterTemplate>
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true" CssClass="LabelResalta" OnCheckedChanged="chkTodos_CheckedChanged"
Text="TODOS" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" AutoPostBack="true"  OnCheckedChanged="chkTodos_CheckedChanged"/>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<HeaderStyle Width="10%" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentos" />
<asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
<asp:AsyncPostBackTrigger ControlID="wucKilometraje" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="wucFacturadoConcepto" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRecibir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRecibir" runat="server" TabIndex="14"  OnClick="btnRecibir_Click"
CssClass="boton" Text="Recibir" Enabled="False" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DE ESTATUS DE RECPCIÓN POR SEGMENTO -->
<div id="contenidoResumenSegmentos" class="modal">
<div id="modalResumenSegmentos" class="resumen_documentos_segmento"> 
<div class="header_resumen_documentos_segmento">
<img src="../Image/Totales.png" />                 
<h3>Resumen por segmento</h3>
</div>
<div class="grid_documentos_segmento">
<asp:UpdatePanel ID="upgvSegmentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSegmentos" runat="server" CssClass="gridview" Width="100%" AllowPaging="True" OnPageIndexChanging="gvSegmentos_PageIndexChanging"
ShowFooter="True" AutoGenerateColumns="False" PageSize="5">
<Columns>
<asp:BoundField DataField="HojaInstruccion" HeaderText="Hoja Instruccion" SortExpression="HojaInstruccion" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:TemplateField HeaderText="Remitente" SortExpression="Remitente">
<ItemTemplate>
<asp:Label ID="lblRemitente" runat="server" ToolTip='<%# Eval("Remitente") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Remitente").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Destinatario" SortExpression="Destinatario">
<ItemTemplate>
<asp:Label ID="lblDestinatario" runat="server" ToolTip='<%# Eval("Destinatario") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Destinatario").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="EstatusSegmento" HeaderText="Estatus del Segmento" SortExpression="EstatusSegmento" />
<asp:TemplateField HeaderText="LugarCobro" SortExpression="LugarCobro">
<ItemTemplate>
<asp:Label ID="lblLugarCobro" runat="server" ToolTip='<%# Eval("LugarCobro") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("LugarCobro").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Documentos" HeaderText="Documentos" SortExpression="Documentos" />
</Columns>
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">        
<div class="renglon3x">                    
<div class="controlBoton">
<asp:Button ID="btnCerrar" runat="server" CssClass="boton" Text="Cerrar" />
</div>
</div>
<div class="renglon2x"> </div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE RECEPCIÓN DE DOCUMENTOS -->
<div id="contenidoConfirmacionRecibeDocumentos" class="modal">
<div id="confirmacionRecibeDocumentos" class="contenedor_ventana_confirmacion">
<div class="header_resumen_documentos_segmento">
<img src="../Image/Exclamacion.png" />                 
<h3>Recibir documentos</h3>
</div>
<div class="columna">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeConfirmacionRecepcion" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblMensajeConfirmacionRecepcion" runat="server"   CssClass="mensaje_modal"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRecibir" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" CssClass="boton" OnClick="btnAceptar_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>             
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE ENCABEZADO DE SERVICIO -->
<div id="encabezadoServicioModal" class="modal">
<div id="encabezadoServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEncabezadoServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEncabezadoServicio" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="EncabezadoServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucReferenciaServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucEncabezadoServicio ID="wucEncabezadoServicio" runat="server" 
OnClickGuardarReferencia="wucEncabezadoServicio_ClickGuardarReferencia" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE KILOMETRAJE -->
<div id="kilometrajeMovimientoModal" class="modal">
<div id="kilometrajeMovimiento" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarKilometrajeMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarKilometrajeMovimiento" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="KilometrajeMovimiento" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucKilometraje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucKilometraje runat="server" ID="wucKilometraje" OnClickGuardar="wucKilometraje_ClickGuardar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE REFERENCIAS DE SERVICIO -->
<div id="referenciasServicioModal" class="modal">
<div id="referenciasServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ReferenciasServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Referencias Servicio</h2>
</div> 
<div class="columna2x">
<asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true"
OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DE CARGOS DEL SERVICIO -->
<div id="cargosServicioModal" class="modal">
<div id="cargosServicio" class="contenedor_modal_seccion_completa_arriba" style="width:1200px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCargos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCargos" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="CargosServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/FacturacionCargos.png" />
<asp:UpdatePanel ID="uph2CargosServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2 id="h2CargosServicio" runat="server">Cargos del Servicio</h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div> 
<asp:UpdatePanel ID="upwucFacturadoConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucFacturadoConcepto runat="server" ID="wucFacturadoConcepto" OnClickGuardarFacturaConcepto="wucFacturadoConcepto_ClickGuardarFacturaConcepto" OnClickEliminarFacturaConcepto="wucFacturadoConcepto_ClickEliminarFacturaConcepto" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA CONFIRMACIÓN HACER SERVICIO A PARTIR DE MOV. VACÍO -->
<div id="contenidoConfirmacionHacerServicio" class="modal">
<div id="confirmacionHacerServicio" class="contenedor_ventana_confirmacion">   
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHacerServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHacerServicio"    OnClick="lkbCerrarHacerServicio_Click" runat="server" Text="Cerrar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>                        
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h2>Convertir Movimiento Vacio a Servicio</h2>
</div>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtClienteHacerServicio">Cliente</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uptxtClienteHacerServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtClienteHacerServicio" runat="server"  CssClass="textbox2x validate[required, custom[IdCatalogo]]"  ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarHacerServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarHacerServicio" runat="server"   OnClick="btnAceptarHacerServicio_Click" CssClass="boton" Text="Aceptar" />
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
<asp:LinkButton ID="lnkCerrarDev" runat="server" CommandName="DevolucionesServicio" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="61">
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
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
<asp:LinkButton ID="lkbCerrarDevolucion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Devolucion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucDevolucionFaltante" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucDevolucionFaltante ID="wucDevolucionFaltante" runat="server" OnClickGuardarDevolucion="wucDevolucionFaltante_ClickGuardarDevolucion" 
OnClickGuardarDevolucionDetalle="wucDevolucionFaltante_ClickGuardarDevolucionDetalle"
OnClickEliminarDevolucionDetalle="wucDevolucionFaltante_ClickEliminarDevolucionDetalle" 
OnClickAgregarReferenciasDevolucion="wucDevolucionFaltante_ClickAgregarReferenciasDevolucion"
OnClickAgregarReferenciasDetalle="wucDevolucionFaltante_ClickAgregarReferenciasDetalle" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="gvDevoluciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE SERVICIO NO FACTURABLE -->
<div id="confirmacionNoFacturable" class="modal">
<div id="NoFacturable" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarNoFacturable" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarNoFacturable" runat="server" OnClick="lkbCerrarNoFacturable_Click" CommandName="NoFacturable" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Desea cambiar el estatus del servicio a No Facturable? </h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivoNoFacturable">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoNoFacturable" runat="server" UpdateMode="Conditional">
<ContentTemplate>
 <asp:TextBox ID="txtMotivoNoFacturable" runat="server" TextMode="MultiLine"  Text=" " CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox></div></div>
</ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
    </Triggers>
</asp:UpdatePanel>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnNoFacturable" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnNoFacturable" Text="Si" CssClass="boton" OnClick="btnNoFacturable_Click" />
</ContentTemplate>
<Triggers>

</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div></div></div>
<!-- VENTANA DE CONFIRMACIÓN DE RECEPCIÓN MÚLTIPLE -->
<div id="confirmacionRecepcionMultipleModal" class="modal">
<div id="confirmacionRecepcionMultiple" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Recepción Múltiple de Evidencias</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="uplblConfirmacionRecepcionMultiple" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblConfirmacionRecepcionMultiple" runat="server" CssClass="mensaje_modal"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRecibirSeleccion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnCancelarRecepcionMultiple" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelarRecepcionMultiple" Text="No" CssClass="boton_cancelar" OnClick="btnConfirmarRecepcionMultiple_Click" CommandName="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnConfirmarRecepcionMultiple" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnConfirmarRecepcionMultiple" Text="Si" CssClass="boton" OnClick="btnConfirmarRecepcionMultiple_Click" CommandName="Confirmar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!--VENTANA MODAL DE PARADAS DE LOS SERVICIOS-->
<div id="contenedorVerAnticipos" class="modal">
        <div id="ventanaVerAnticipos" class="contenedor_ventana_confirmacion_arriba" style="min-width: 950px; width: 950px;padding-bottom: 5px;">
            <div class="columna3x" style="min-width: 950px; width: 950px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModal" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModal" runat="server" Text="Cerrar"  OnClick="lkbCerrarVentanaModal_Click" CommandName="Paradas">
    <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/EnTransito.png" style="width: 32px;" />
                    <asp:UpdatePanel ID="upEncabezadoAnticiposProgramados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h2 id="EncabezadoAnticiposProgramados" runat="server">Paradas del servicio</h2>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamañoGridViewAnticipos">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamañoGridViewAnticipos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamañoGridViewAnticipos" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewAnticipos_SelectedIndexChanged" TabIndex="8" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblCriterioGridViewAnticipos">Ordenado por:</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCriterioGridViewAnticipos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCriterioGridViewAnticipos" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAnticipos" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div id="Anticipos" class="grid_seccion_completa_200px_altura" >
                    <asp:UpdatePanel ID="upgvAnticipos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAnticipos" CssClass="gridview" OnPageIndexChanging="gvAnticipos_PageIndexChanging" OnSorting="gvAnticipos_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                ShowFooter="True" TabIndex="22" OnRowDataBound="gvAnticipos_RowDataBound"
                                PageSize="5" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                    <asp:BoundField DataField="Evento" HeaderText="Evento" SortExpression="Evento"/>
                                    <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="Cuidad" HeaderText="Cuidad" SortExpression="Cuidad"/>
                                    <asp:BoundField DataField="Ubicacion" HeaderText="Ubicacion" SortExpression="Ubicacion" />
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
                            <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
<!--VENTANA MODAL DE CLASIFICACION DE LOS SERVICIOS-->
<div id="contenedorClasificacion" class="modal">
        <div id="ventanaClasificacion" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarDocumentacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarDocumentacion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Clasificacion" Text="Cerrar">
                                      <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Clasificacion.png" style="width: 32px;" /> 
                 <h2 id="H1" runat="server">Clasificación</h2>
            </div>
            <asp:UpdatePanel ID="upwucClasificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucClasificacion runat="server" ID="wucClasificacion" OnClickGuardar="wucClasificacion_ClickGuardar" OnClickCancelar="wucClasificacion_ClickCancelar" TabIndex="24" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>