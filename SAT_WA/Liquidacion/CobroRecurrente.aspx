<%@ Page Title="Cobro Recurrente" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CobroRecurrente.aspx.cs" Inherits="SAT.Liquidacion.CobroRecurrente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraCobroRecurrente();
}
}
//Función de Configuración
function ConfiguraCobroRecurrente() {
$(document).ready(function () {
//Funcion de validación de Controles
var validaCobroRecurrente = function () {
var isValid1 = !$("#<%=txtTotalDeuda.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtTotalCobrado.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtMontoCobro.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtEntidad.ClientID%>").validationEngine('validate');
var isValid5 = !$("#<%=txtFechaIni.ClientID%>").validationEngine('validate');
//Devolviendo Resultado de Validación
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
}
    //Funcion de validación de Controles
    var validaTermino = function () {
        var isValid1 = !$("#<%=txtMotivo.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado de Validación
        return isValid1;
    }
//Limpiando Control al Cambiar Selección
$("#<%=ddlTipoEntApl.ClientID%>").change(function () {
//Limpiando Control de Entidad
$("#<%=txtEntidad.ClientID%>").val('');
});
//Añadiendo Validación al Evento
    $("#<%=btnTerminar.ClientID%>").click(validaTermino);            
//Añadiendo Validación al Evento
$("#<%=btnAceptar.ClientID%>").click(validaCobroRecurrente);
/** Función de fechas **/
$("#<%=txtFechaIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false
});
});
}

ConfiguraCobroRecurrente();

</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Cobros Recurrentes</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
<li>
<asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbTerminar" runat="server" Text="Terminar" OnClick="lkbElementoMenu_Click" CommandName="Terminar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblId">Id</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Text="<b>Por Asignar</b>"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatusTermino">Estatus Termino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatusTermino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusTermino" runat="server" CssClass="dropdown2x" TabIndex="12"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoEntApl">Tipo Entidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoEntApl" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEntApl" runat="server" CssClass="dropdown2x" TabIndex="7" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtEntidad">Entidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtEntidad" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoEntApl" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoCobroRecurrente">Tipo de Cobro</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoCobroRecurrente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoCobroRecurrente" runat="server" CssClass="dropdown2x" TabIndex="1"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTotalDeuda">Total Deuda</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTotalDeuda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotalDeuda" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMontoCobro">Descuento</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMontoCobro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMontoCobro" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<%--<div class="renglon2x">
<div class="etiqueta">
<label for="txtDiasCobro">Dias de Cobro</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDiasCobro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDiasCobro" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>--%>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaIni">Fec. Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaIni" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="10" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTotalCobrado">Total Cobrado</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTotalCobrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotalCobrado" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSaldo">Saldo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSaldo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSaldo" runat="server" CssClass="textbox2x" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaUltCob">Fec. Ultimo Cobro</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaUltCob" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaUltCob" runat="server" CssClass="textbox validate[custom[date]]" TabIndex="11" MaxLength="10" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAceptar_Click" TabIndex="13" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="14" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_controles">
<div class="columna2x">
<div class="header_seccion">
<h2>Cobros Recurrentes Por Liquidación</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoCRL">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCRL" runat="server" TabIndex="15" OnSelectedIndexChanged="ddlTamanoCRL_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRL">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCRL" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobroRecurrenteLiquidacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarCRL" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCRL" runat="server" Text="Exportar" OnClick="lnkExportarCRL_Click"
TabIndex="16"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCRL" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvCobroRecurrenteLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobroRecurrenteLiquidacion" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
CssClass="gridview" ShowFooter="true" TabIndex="17" OnSorting="gvCobroRecurrenteLiquidacion_Sorting"
OnSelectedIndexChanging="gvCobroRecurrenteLiquidacion_SelectedIndexChanging" OnPageIndexChanging ="gvCobroRecurrenteLiquidacion_PageIndexChanging" AutoGenerateColumns="false">
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
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion">
    <ItemStyle HorizontalAlign="Right"/>
</asp:BoundField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
    <ItemStyle HorizontalAlign="Right"/>
</asp:BoundField>
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" DataFormatString ="{0:C2}">
    <ItemStyle HorizontalAlign="Right"/>
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString ="{0:C2}">
    <ItemStyle HorizontalAlign="Right" Font-Bold ="true" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCRL" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA DE CONFIRMACIÓN DE TERMINO COBRO RECURRENTE -->
<div id="confirmacionTerminoCobroMultipleModal" class="modal">
<div id="ventanaconfirmacionTerminoCobroMultiple" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarMotivo" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarMotivo" runat="server" OnClick="lkbCerrarMotivo_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div> 
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Terminó de Cobro Recurrente</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<label class="mensaje_modal">
¿Realmente desea Terminar el Cobro Recurrente?
</label>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uptxtMotivo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivo" runat="server" CssClass="textbox3x validate[required]" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel runat="server" ID="upbtnTerminar" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnTerminar" Text="Aceptar" CssClass="boton" OnClick="btnTerminar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</asp:Content>
