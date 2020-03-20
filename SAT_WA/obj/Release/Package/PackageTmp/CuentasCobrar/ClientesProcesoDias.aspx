<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ClientesProcesoDias.aspx.cs" Inherits="SAT.CuentasCobrar.ClientesProcesoDias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos de la Forma -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraClienteProcesosDias()
}
}


function ConfiguraClienteProcesosDias() {
$(document).ready(function () {

//Agregamos Al TextBox HoraInicio el control de horas
$("#<%=txtHoraInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'H:i',
datepicker: false
});

//Agregamos Al TextBox HoraTermino el control de horas
$("#<%=txtHoraTermino.ClientID%>").datetimepicker({
lang: 'es',
format: 'H:i',
datepicker: false
});


//Añadiendo Función de Autocompletado al Control
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=24&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

// Valida los controles de Cliente Procesos
var validaClientesProceso = function () {
//Configurando Validación de Controles 
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtSecuencia.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtContacto.ClientID%>").validationEngine('validate');
//Devolviendo Valor Obtenido
return isValid1 && isValid2 && isValid3 && isValid4;
};

//Añadiendo Función de Validación
$("#<%=btnGuardar.ClientID%>").click(validaClientesProceso);
//Añadiendo Función de Validación
$("#<%=lkbGuardar.ClientID%>").click(validaClientesProceso);

// Valida los controles de Dias De Revision
var validaDiasRevision = function () {
var isValid1 = !$("#<%=txtHoraInicio.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtHoraTermino.ClientID%>").validationEngine('validate');
if ($("#<%=rdbDiasSemana.ClientID%>").is(':checked')) {
} else {
var isValid3 = !$("#<%=txtDiaMes.ClientID%>").validationEngine('validate');
}
return isValid1 && isValid2 && isValid3;
}

//Añadiendo Función de Validación
$("#<%=btnGuardarDiasRevision.ClientID%>").click(validaDiasRevision);
});
}


//Funcion para validar que la hora de inicio no sea igual o mayor a la de termino
function ComparaHoras() {

//Obteniendo Valores
var horaInicio = $("#<%=txtHoraInicio.ClientID%>").val();
var horaTermino = $("#<%=txtHoraTermino.ClientID%>").val();

//Validando que la hora de inicio no sea maayor o igual a la hora de termino
if (horaInicio >= horaTermino)
//Mostrando Mensaje de Operación
return "* La Hora de Inicio debe ser inferior a la Hora de Termino";
}

//Funcion para validar que los dias ingresados se encuentren en un rango de 1 - 31
function VerificaDiasMes() {

//Obteniendo Valores
var dia = $("#<%=txtDiaMes.ClientID%>").val();

//Validando que el dia no sea menor a uno o mayor a 31
if (dia < 1 || dia > 31)
//Mostrando Mensaje de Operación
return "* Dias Validos del 1 al 31";
}

ConfiguraClienteProcesosDias()

</script>

<div id="encabezado_forma">
<img src="../Image/Cliente.jpg" />
<h1>Cliente Procesos Dias</h1>
</div>

<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" CommandName="Nuevo" OnClick="lkbEventosMenu_Click" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" CommandName="Abrir" OnClick="lkbEventosMenu_Click" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" CommandName="Guardar" OnClick="lkbEventosMenu_Click" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" CommandName="Editar" OnClick="lkbEventosMenu_Click" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" OnClick="lkbEventosMenu_Click" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" CommandName="Bitacora" OnClick="lkbEventosMenu_Click" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" CommandName="Referencias" OnClick="lkbEventosMenu_Click" /></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" CommandName="Archivos" OnClick="lkbEventosMenu_Click" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Cliente.png" />
<h2>Cliente Procesos</h2>
</div>
<asp:Panel ID="pnlClienteProcesos" runat="server" DefaultButton="btnGuardar">
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente : </label>
</div>

<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoProceso">Tipo de Proceso : </label>
</div>

<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoProceso" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSecuencia">Secuencia : </label>
</div>

<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSecuencia" runat="server" CssClass="textbox2x validate[required, custom[integer]]" MaxLength="3"></asp:TextBox>
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripcion : </label>
</div>

<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto : </label>
</div>

<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtContacto" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="UpdatePanel7" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>

<div class="controlBoton">
<asp:UpdatePanel ID="UpdatePanel6" runat="server">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Panel>
</div>
<div class="seccion_controles">
<asp:Panel ID="pnlDiasRevision" runat="server" DefaultButton="btnGuardarDiasRevision">
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/calendar.jpg" />
<h2>Dias De Revision</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="rdbEtiqueta">Forma De Revison : </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbDiasSemana" runat="server" Text="Dias De La Semana" GroupName="DiasRevision" OnCheckedChanged="rdbDias_CheckedChanged" AutoPostBack="true" />
<asp:RadioButton ID="rdbDiasMes" runat="server" Text="Dias Del Mes" GroupName="DiasRevision" OnCheckedChanged="rdbDias_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDiasSemana">Dia De La Semana : </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlDiasSemana" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="rdbDiasSemana" />
<asp:AsyncPostBackTrigger ControlID="rdbDiasMes" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDiaMes">Dia Del Mes : </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDiaMes" runat="server" CssClass="textbox validate[required, custom[integer], funcCall[VerificaDiasMes[]]" MaxLength="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="rdbDiasSemana" EventName="CheckedChanged" />
<asp:AsyncPostBackTrigger ControlID="rdbDiasMes" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtHoraInicio">Hora De Inicio : </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtHoraInicio" runat="server" CssClass="textbox validate[required, custom[time24]]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtHoraTermino">Hora De Termino : </label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtHoraTermino" runat="server" CssClass="textbox validate[required, custom[time24], funcCall[ComparaHoras[]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>

</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="UpdatePanel13" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarDiasRevision" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarDiasRevision_Click" />
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>

<div class="controlBoton">
<asp:UpdatePanel ID="UpdatePanel14" runat="server">
<ContentTemplate>
<asp:Button ID="btnGuardarDiasRevision" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardarDiasRevision_Click" />
</ContentTemplate>

<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Panel>
<div class="contenedor_730px_derecha">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="14" AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenado" runat="server" Text="Ordenado"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDiasRevision" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="15" OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDiasRevision" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="7"
AutoGenerateColumns="false" Width="100%" PageSize="5" OnPageIndexChanging="gvDiasRevision_PageIndexChanging" OnSorting="gvDiasRevision_Sorting">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="DiaSemana" HeaderText="Dias Semana" SortExpression="DiaSemana" />
<asp:BoundField DataField="DiaMes" HeaderText="Dias Mes" SortExpression="DiaMes" />
<asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" SortExpression="HoraInicio" />
<asp:BoundField DataField="HoraTermino" HeaderText="Hora Termino" SortExpression="HoraTermino" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbAbrir" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarDiasRevision" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />

</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

</asp:Content>
