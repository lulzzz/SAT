<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucVencimiento.ascx.cs" Inherits="SAT.UserControls.wucVencimiento" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryVencimiento();
}
}

//Declarand Función de Configuración
function ConfiguraJQueryVencimiento()
{   //Inicializando Función
$(document).ready(function () {

//Declarando Función de Validación
var validaVencimiento = function () {

//Declarando Validacion 1
var isValid1;

//Obteniendo Valor
var ent = $("#<%=ddlEntidad.ClientID%>").val();

//Obteniendo Control a Validar
if (ent == 76)
//Operadores
isValid1 = !$("#<%=txtRegistroEnt1.ClientID%>").validationEngine('validate');
else if (ent == 19)
//Unidades
isValid1 = !$("#<%=txtRegistroEnt2.ClientID%>").validationEngine('validate');
else if (ent == 1)
//Unidades
isValid1 = !$("#<%=txtRegistroEnt3.ClientID%>").validationEngine('validate');

//Validando Controles
var isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtValorKm.ClientID%>").validationEngine('validate');

//Devolviendo Resultado de la Validación
return isValid1 && isValid2 && isValid3 && isValid4;
}

//Añadiendo Función de validación al Evento
$("#<%=btnAceptar.ClientID%>").click(validaVencimiento);

//Cargando Controles de Fechas
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Cargando Catalogo de Operadores
    $("#<%=txtRegistroEnt1.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=27&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });

//Cargando Catalogo de Unidades
    $("#<%=txtRegistroEnt2.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=28&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
});
}

//Invocando Función de Configuración
ConfiguraJQueryVencimiento();


</script>
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Vencimiento</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblId">No. Vencimiento</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblId" runat="server" Text="Por Asignar"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEntidad">Entidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEntidad" runat="server" CssClass="dropdown2x" AutoPostBack="true" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtRegistro">Registro</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtRegistro" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRegistroEnt1" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="true"></asp:TextBox>
<asp:TextBox ID="txtRegistroEnt2" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="false"></asp:TextBox>
<asp:TextBox ID="txtRegistroEnt3" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Visible="false" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoVencimiento">Tipo</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoVencimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoVencimiento" runat="server" CssClass="dropdown2x" AutoPostBack="true"
OnSelectedIndexChanged="ddlTipoVencimiento_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlPrioridad">Prioridad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlPrioridad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoVencimiento" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha Inicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox2x validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha Termino</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox2x validate[custom[dateTime24]]" MaxLength="16" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtValorKm">Valor Kilometraje</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtValorKm" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorKm" runat="server" CssClass="textbox validate[custom[positiveNumber]]" MaxLength="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnTerminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnTerminar" runat="server" CssClass="boton" Text="Terminar" OnClick="btnTerminar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnTerminar" />
</Triggers>
</asp:UpdatePanel>
</div><br />
</div>
