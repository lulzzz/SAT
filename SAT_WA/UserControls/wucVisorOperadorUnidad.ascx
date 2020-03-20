<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucVisorOperadorUnidad.ascx.cs" Inherits="SAT.UserControls.wucVisorOperadorUnidad" %>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryBusquedaRecurso();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryBusquedaRecurso() {

// *** Añadiendo lógica de validación de Campos *** //
//Disparadores de validación de Encabezado de Servicio
$(document).ready(function () {

// *** Catálogos Autocomplete *** //
$(document).ready(function () {
    $("#<%=txtUnUbicacion.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
    $("#<%=txtOpUbicacion.ClientID%>").autocomplete({ 
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
        
    $("#<%=txtUnProveedor.ClientID%>").autocomplete({ 
        source: '../WebHandlers/AutoCompleta.ashx?id=26&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
        appendTo: "<%=this.Contenedor%>"
    });
});
});
}

//Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryBusquedaRecurso();

</script>
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Recursos
</h2>
</div>
<div class="seccion_controles">
<div class="renglon2x">
<div class="control">
<asp:UpdatePanel ID="uprdbUnUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbUnUnidad" runat="server" CssClass="Label" GroupName="BusquedaRecursos" Text="Unidades" AutoPostBack="true" OnCheckedChanged="rdbUnUnidad_CheckedChanged" Checked="True" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbOpOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbOpOperador" runat="server" CssClass="Label" GroupName="BusquedaRecursos" Text="Operadores" AutoPostBack="true" OnCheckedChanged="rdbUnUnidad_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upmtvBusquedaOU" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvBusquedaOU" runat="server" ActiveViewIndex="0">
<asp:View ID="vwBusquedaUnidad" runat="server">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnNumeroUnidad">Número Unidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnNumeroUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnNumeroUnidad" runat="server" CssClass="textbox" MaxLength="30"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlUnTipoUnidad">Tipo Unidad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUnTipoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnTipoUnidad" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlUnEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlUnEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlUnEstatus" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnProveedor">Propietario</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnProveedor" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" MaxLength="150" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkUnNoPropio" />
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="upchkUnNoPropio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkUnNoPropio" runat="server" AutoPostBack="true" CssClass="Label" OnCheckedChanged="chkUnNoPropio_CheckedChanged" Text="¿Otro?" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnUbicacion">Ubicación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
<asp:View ID="vwBusquedaOperador" runat="server">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOpNombre">Nombre</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtOpNombre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtOpNombre" runat="server" CssClass="textbox2x" MaxLength="100"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlOpEstatus">Estatus</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlOpEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlOpEstatus" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOpUbicacion">Ubicación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtOpUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtOpUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnOUBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnOUBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnOUBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanogvOUResultadosBusqueda">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanogvOUResultadosBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanogvOUResultadosBusqueda" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanogvOUResultadosBusqueda_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoddlTamanogvOUResultadosBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoddlTamanogvOUResultadosBusqueda" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvOUResultadosBusqueda" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:LinkButton ID="lkbExportargvOUResultadosBusqueda" runat="server" 
OnClick="lkbExportargvOUResultadosBusqueda_Click" Text="Exportar"></asp:LinkButton>
</div>
</div>
    </div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvOUResultadosBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvOUResultadosBusqueda" runat="server"  AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvOUResultadosBusqueda_PageIndexChanging" OnSorting="gvOUResultadosBusqueda_Sorting" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvOUResultadosBusqueda_RowDataBound" >
<Columns>
<asp:BoundField DataField="Recurso" HeaderText="Recurso" SortExpression="Recurso" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="SubTipo" HeaderText="Sub Tipo" SortExpression="SubTipo" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />                                    
<asp:BoundField DataField="UbicacionActual" HeaderText="Ubicación Actual" SortExpression="UbicacionActual" />
<asp:BoundField DataField="FechaUbicacion" HeaderText="Fecha Ubicación" SortExpression="FechaUbicacion" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" SortExpression="FechaIngreso" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaBaja" HeaderText="Fecha Baja" SortExpression="FechaBaja" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanogvOUResultadosBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnOUBuscar" />
<asp:AsyncPostBackTrigger ControlID="rdbOpOperador" />
<asp:AsyncPostBackTrigger ControlID="rdbUnUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>