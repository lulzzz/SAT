<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucVencimientosHistorial.ascx.cs" Inherits="SAT.UserControls.wucVencimientosHistorial" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryVencimientoHistorial();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryVencimientoHistorial() {
$(document).ready(function () {

//Fecha de Inicio (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm')
$("#<%= txtFechaInicio.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Fecha de Fin (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm')
$("#<%= txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

/*Cargando Catalogos*/
//Unidad
$("#<%=txtUnidad.ClientID%>").autocomplete({
    source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    appendTo: "<%=this.Contenedor%>"
});
//Operador
$("#<%=txtOperador.ClientID%>").autocomplete({
    source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    appendTo: "<%=this.Contenedor%>"
});
//Proveedor
$("#<%=txtProveedor.ClientID%>").autocomplete({
    source: '../WebHandlers/AutoCompleta.ashx?id=16&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    appendTo: "<%=this.Contenedor%>"
});

//Validación Ubicación
    var validacionVencimientoHistorial = function () {

        var isValidP1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
        var isValidP2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
        var isValidP3;

        //Obteniendo Tipo de Entidad
        var tipoEntidad = $("#<%=ddlTipoEntidad.ClientID%>").val();

        //Validando Tipo de Entidad
        switch (tipoEntidad) {
            case 1:
                //Validando Unidad
                isValidP3 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
                break;
            case 2:
                //Validando Operador
                isValidP3 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
                break;
            case 3:
                //Validando Transportista
                isValidP3 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                break;
        }

        return isValidP1 && isValidP2 && isValidP3;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionVencimientoHistorial);
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryVencimientoHistorial();
</script>
<div class="header_seccion">
<img src="../Image/Calendar2.png" />
<h2 id="h2EntidadConsultada" runat="server">Vencimientos Registrados</h2>
</div>
<div class="seccion_controles">

<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoUnidad">Tipo Entidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoEntidad" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoEntidad_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="uptxtEntidad">Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtEntidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]" Visible="true"></asp:TextBox>
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]" Visible="false"></asp:TextBox>
<asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]" Visible="false"></asp:TextBox>
<asp:TextBox ID="txtServicio" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]" Visible="false" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoEntidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipo">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>           
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlPrioridad">Prioridad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPrioridad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPrioridad" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="control">
<asp:UpdatePanel ID="upchkRangoFechas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar por fecha." OnCheckedChanged="chkRangoFechas_CheckedChanged"
Checked="false" AutoPostBack="true" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbInicioVencimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbInicioVencimiento" runat="server" CssClass="label" Text="Inicio Vencimiento" GroupName="FiltroFecha" Checked="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbFinVenciamiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbFinVenciamiento" runat="server" CssClass="label" Text="Fin Vencimiento" GroupName="FiltroFecha" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Fecha Inicial</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Fecha Final</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" OnClick="btnBuscar_Click" runat="server" CssClass="boton" Text="Buscar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoVencimientoHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoVencimientoHistorial" runat="server" CssClass="boton" Text="Nuevo" OnClick="btnNuevoVencimientoHistorial_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa" style="width: 93%">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoVencimientoHistorial">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoVencimientoHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoVencimientoHistorial" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoVencimientoHistorial_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoVencimientoHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoVencimientoHistorial" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvVencimientos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarVencimientoHistorial" runat="server" TabIndex="5"
OnClick="lkbExportarVencimientoHistorial_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarVencimientoHistorial" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvVencimientos" runat="server" AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvVencimientos_PageIndexChanging" OnSorting="gvVencimientos_Sorting" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvVencimientos_RowDataBound">
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
<asp:BoundField DataField="TipoVencimiento" HeaderText="Tipo" SortExpression="TipoVencimiento" />
<asp:BoundField DataField="Prioridad" HeaderText="Prioridad" SortExpression="Prioridad" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="FechaInicio" HeaderText="Inicio" SortExpression="FechaInicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaFin" HeaderText="Fin" SortExpression="FechaFin" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbConsultarVencimiento" runat="server" Text="Consultar" OnClick="lkbVencimientoOperacion_Click" CommandName="Consultar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbTerminarVencimiento" runat="server" Text="Terminar" OnClick="lkbVencimientoOperacion_Click" CommandName="Terminar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoVencimientoHistorial" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
