<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="AsignacionRecurso.aspx.cs" Inherits="SAT.Operacion.AsignacionRecurso" %>

<%@ Register Src="~/UserControls/wucMovimientoVacioSinOrden.ascx" TagPrefix="operacion" TagName="wucMovimientoVacioSinOrden" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Operacion.css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <!-- Validación de datos de este formulario --><script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryAsignacionUnidad();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryAsignacionUnidad() {
//Función de validación Búsqueda Servicio
var validacionBuscarServicio = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtCiudadOrigen.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtCiudadDestino.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
return isValid1 && isValid2 && isValid3 && isValid4
};
//Función de validación Búsqeda Unidad
var validacionBuscarAsignacionUnidad = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
    var isValid1 = !$("#<%=txtPropietarioUnidad.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtUbicacionUnidad.ClientID%>").validationEngine('validate');
    return isValid1 && isValid2;
};
//Función de validación Búsqueda Operador
var validacionBuscarAsignacionOperador = function (evt) {
//Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
var isValid1 = !$("#<%=txtNombreOperador.ClientID%>").validationEngine('validate');
    return isValid1;
};
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarEliminacionRecurso.ClientID%>").unbind("click");
$("#<%= btnCancelarEliminacionRecurso.ClientID%>").click(function () {
//Mostrando ventana modal 
$("#contenidoConfirmacionQuitarRecursos").animate({ width: "toggle" });
$("#confirmacionQuitarRecursos").animate({ width: "toggle" });
});
//Botón Buscar Servicios
$("#<%=  btnBuscarServicios.ClientID %>").click(validacionBuscarServicio);
//Botón Buscar Unidades
$("#<%=  btnBuscarUnidades.ClientID %>").click(validacionBuscarAsignacionUnidad);
//Botón Buscar Operadores
$("#<%=  btnBuscarOperadores.ClientID %>").click(validacionBuscarAsignacionOperador);

// *** Catálogos Autocomplete *** //
$(document).ready(function () {

$("#<%=txtPropietarioUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=16&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
$("#<%=txtNombreOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>'});
$("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>'});
$("#<%=txtCiudadOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8'});
$("#<%=txtCiudadDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8'});
$("#<%=txtUbicacionUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>' });
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y',
timepicker: false,
});
});


}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryAsignacionUnidad();

</script>
<div id="encabezado_forma">
<img src="../Image/OperacionPatio.png" />
<h1>Asignación de Recursos</h1>
</div>
<section class="fila_indicador">        
<a href="#" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblDocumentados" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblDocumentados"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarServicios"/>
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
</Triggers>                     
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/Documentados.png" />
</div>
<div class="leyenda_indicador">
Servicios x asignar
</div>
</a>
<a href="#" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblCitaTarde" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCitaTarde"></asp:Label>
</ContentTemplate> 
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarServicios"/>
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
</Triggers>                   
</asp:UpdatePanel>
                
</div>
<div class="imagen_indicador">
<img src="../Image/CitaTarde.png" />
</div>
<div class="leyenda_indicador">
Citas Perdidas
</div>
</a>
<a href="#" class="indicadorL">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblCajas" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCajas"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarServicios"/>
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
</Triggers>                        
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorCajaEstaciona.png" />
</div>
<div class="leyenda_indicador">
Cajas Disponibles
</div>
</a>
<a href="#" class="indicadorL">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="upplblTractores" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTractores"></asp:Label>
</ContentTemplate> 
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarServicios"/>
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
</Triggers>                       
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/Transportista.png" />
</div>
<div class="leyenda_indicador">
Tractores Disponibles
</div>
</a>        
<a href="#" class="indicador_texto">
<div class="texto_indicador">
<asp:UpdatePanel runat="server" ID="upplblDisponibilidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblDisponibilidad"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID ="btnBuscarServicios"/>
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
</Triggers>                     
</asp:UpdatePanel>

</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorTiempo.png" />
</div>
<div class="leyenda_indicador">
Disponibilidad Promedio
</div>
</a>        
</section>
<div class="header_seccion_asignacion">        
<h2>Busque su servicio</h2>
</div>
<div class="renglon100Per">
<div class="etiqueta_50px">
<label for="txtCliente">
Cliente
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" AutoPostBack="true" TabIndex="14"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtCiudadOrigen">Ciudad Origen</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCiudadOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadOrigen" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtCiudadDestino">Ciudad Destino</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCiudadDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudadDestino" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="txtFecha" class="Label">
Cita</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtFecha" Enabled="true" runat="server" CssClass="textbox validate[custom[date]]" TabIndex="11"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarServicios" runat="server" CssClass="boton" Text="Buscar" TabIndex="2" OnClick="btnBuscarServicios_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<section class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2>Servicios pendientes</h2>
</div>
<div class="renglon_pestaña_documentacion">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoSevicios">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoSevicios" runat="server" AutoPostBack="true" CssClass="dropdown" TabIndex="5" OnSelectedIndexChanged="ddlTamanoSevicios_OnSelectedIndexChanged">
</asp:DropDownList>
</div>
<div class="etiqueta">
<label for="lblOrdenarServicios">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenarSevicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarSevicios" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSevicios" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar" TabIndex="7" OnClick="lkbExportarServicios_OnClick"></asp:LinkButton>
</div>
<div class="control2x">
    <asp:UpdatePanel ID="uplblErrorKm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblErrorKm" runat="server" CssClass="label_error"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvSevicios" />
            <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvSevicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSevicios" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" TabIndex="8"
OnPageIndexChanging="gvServicios_PageIndexChanging" OnSorting="gvServicios_Sorting" OnRowDataBound="gvServicios_RowDataBound" ShowFooter="True" CssClass="gridview" Width="100%" PageSize="10">
<Columns>
<asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
<ItemTemplate>
<asp:LinkButton ID="lkbServicio" runat="server" Text='<%#Eval("Servicio") %>' OnClick="lkbServicios_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
<ItemTemplate>
<asp:Label ID="lblCliente" runat="server" ToolTip='<%# Eval("Cliente") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Cliente").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CitaOrigen" HeaderText="Cita Carga" SortExpression="CitaOrigen" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Faltan" HeaderText="Faltan" SortExpression="Faltan" />                            
<asp:TemplateField HeaderText="Ciudad Carga" SortExpression="CiudadOrigen">
<ItemTemplate>
<asp:LinkButton ID="lnkOrigen" runat="server" Text='<%#Eval("CiudadOrigen") %>' OnClick="lnkOrigen_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CitaDestino" HeaderText="Cita Descarga" SortExpression="CitaDestino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />                          
<asp:TemplateField HeaderText="Ciudad Descarga" SortExpression="CiudadDestino">
<ItemTemplate>
<asp:LinkButton ID="lnkDestino" runat="server" Text='<%#Eval("CiudadDestino") %>' OnClick="lnkDestino_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Kilometraje" SortExpression="Kilometraje">
<ItemTemplate>
<asp:LinkButton ID="lnkKilometraje" runat="server" Text='<%#Eval("Kilometraje") %>' OnClick="lnkKilometraje_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="TiempoEstimado" HeaderText="Tiempo" SortExpression="TiempoEstimado" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lnkRecursos" runat="server" OnClick="lnkRecursos_Click" >
<img src="../Image/UnidadAsignada.png" width="20" height="20" />
</asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoSevicios" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</section>   
<div class="contenedor_botones_pestaña_filtros_asignacion">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnUnidad" Text="Unidad" Width="85px" OnClick="btnVista_OnClick" CommandName="Unidad" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnOperador" Text="Operador" Width="85px" runat="server" OnClick="btnVista_OnClick" CommandName="Operador" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnTercero" Text="Tercero" Width="85px" runat="server" OnClick="btnVista_OnClick" CommandName="Tercero" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<section class="contenido_tabs_filtros_asignacion">
<asp:UpdatePanel ID="upmtvRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvRecursos" runat="server" ActiveViewIndex="0">
<asp:View ID="vwUnidad" runat="server">
<div class="columna_filtros_asignacion">
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNoUnidad">No. Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoUnidad" runat="server" CssClass="textbox validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlTipoUnidad">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoUnidad" runat="server" TabIndex="2" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlEstatusUnidad">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusUnidad" runat="server" TabIndex="2" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtPropietarioUnidad">Propietario</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtPropietarioUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPropietarioUnidad" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtUbicacionUnidad">Ubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUbicacionUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionUnidad" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarUnidades" runat="server" TabIndex="4" CommandName="Unidades" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                                                                       
</asp:View>
<asp:View ID="vwOperador" runat="server">
<div class="columna_filtros_asignacion">
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlEstatusOperador">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusOperador" runat="server" TabIndex="2" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNombre">Nombre</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombreOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreOperador" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>                            
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarOperadores" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarOperadores" runat="server" TabIndex="4" CommandName="Operadores" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                        
</asp:View>
<asp:View ID="vwTercero" runat="server">
<div class="columna_filtros_asignacion">                        
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNombreTercero">Nombre</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombreTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreTercero" runat="server" CssClass="textbox_240px"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarTerceros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarTerceros" runat="server" TabIndex="4" CommandName="Tercero" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                        
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel> 
<div class="renglon">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>      
</section>    
<section class="contenedor_seccion_unidades_disponibles">
<div class="header_seccion">
<img  src="../Image/Buscar.png"/>
<h2>Recursos Disponibles</h2>
</div>       
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoRecursosDisponibles">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoRecursosDisponibles" runat="server" AutoPostBack="true" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoRecursosDisponibles_OnSelectedIndexChanged" TabIndex="15">
</asp:DropDownList>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarRecursosDisponibles">Ordenar:</label>
<asp:Label ID="lblOrdenarRecursosDisponibles" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" EventName="Sorting" />
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width: auto">
<asp:LinkButton ID="lkbExportarRecursosDisponibles" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarRecursosDisponibles_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosDisponibles" runat="server" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvRecursosDisponibles_PageIndexChanging" OnSorting="gvRecursosDisponibles_Sorting"
AllowSorting="True" TabIndex="18" Width="100%" OnRowDataBound="gvRecursosDisponibles_RowDataBound" AutoGenerateColumns="false"
ShowFooter="True" CssClass="gridview">
<Columns>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="Propio" HeaderText="Propio" SortExpression="Propio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" />
<asp:BoundField DataField="NoServicio" HeaderText="NoServicio" SortExpression="NoServicio" /> 
<asp:TemplateField HeaderText="UltimaUbicacion" SortExpression="UltimaUbicacion">
<ItemTemplate>
<asp:HyperLink ID="hlnkUltimaUbicacion" runat="server" Target="_blank" ToolTip='<%# Eval("UltimaUbicacion") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("UltimaUbicacion").ToString(), 30, "...") %>' >
</asp:HyperLink>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Pendientes" SortExpression="Asignaciones">
<ItemTemplate>
<asp:LinkButton ID="lnkAsignaciones" runat="server" ToolTip ='<%# Eval("Asignaciones") %>' OnClick="lkbServiciosRecursoDisponible_Click" >
<img src="../Image/Calendario20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Liquidar" SortExpression="PorLiquidar">
<ItemTemplate>
<asp:LinkButton ID="lnkLiquidar" runat="server" ToolTip ='<%# Eval("PorLiquidar") %>' OnClick="lnkLiquidar_Click" >
<img src="../Image/Terminado20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>                                                          
<asp:TemplateField  HeaderText="Depositos" SortExpression="PendientesLiq">
<ItemTemplate>
<asp:LinkButton ID="lnkDeposito" runat="server" ToolTip ='<%# Eval("PendientesLiq") %>' OnClick="lkbPendientesLiq_Click" >
<img src="../Image/Signo20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Vacios" SortExpression="Ubicacion">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbCambioUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCambioUbicacion" Text="Reubicar" runat="server" OnClick="lkbUbicacion_OnClick"></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Agregar" SortExpression="Agregar">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAgregar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAgregar" Text="Agregar" runat="server" OnClick="lkbAgregar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="" >
<ItemTemplate>
<asp:UpdatePanel ID="uplkbLiberar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbLiberar"  Visible="true" Text="Liberar" OnClick="lkbLiberar_Click" runat="server" ></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
</Triggers>
</asp:UpdatePanel>
</div>        
</section>    
<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE UNIDADES ASIGNADAS A SERVICIO -->
<div id="contenidoUnidadesAsignadas" class="modal">
<div id="unidadesAsignadas" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarUnidadesAsignadas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarUnidadesAsignadas" runat="server" Text="Cerrar" OnClick="lnkCerrarUnidadesAsignadas_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Tractor-Remolque.png"/>
<h2>Recursos Asignados</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlResursosAsignados">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlRecursosAsignados" runat="server" AutoPostBack="true" CssClass="dropdown" TabIndex="5" OnSelectedIndexChanged="ddlTamanoRecursosAsignados_OnSelectedIndexChanged">
</asp:DropDownList>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlRecursosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarRecursosAsignados">Ordenado Por:</label>
<asp:Label ID="lblOrdenarRecursosAsignados" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width: auto">
<asp:LinkButton ID="lkbExportarExcelRecursosAsignados" runat="server" Text="Exportar" TabIndex="7" OnClick="lkbExportarRecursosAsignados_OnClick"></asp:LinkButton>
</div>
</div>   
<div class="grid_seccion_completa_150px_altura">            
<asp:UpdatePanel ID="upgvRecursosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosAsignados" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" TabIndex="8"
CssClass="gridview" OnPageIndexChanging="gvRecursosAsignados_PageIndexChanging" OnSorting="gvRecursosAsignados_Sorting" Width="100%" PageSize="5">
<Columns>
<asp:BoundField DataField="Asignacion" HeaderText="Tipo" SortExpression="Asignacion" />                                
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />                                
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="UbicacionActual" HeaderText="Ubicación Actual" SortExpression="UbicacionActual" />
<asp:BoundField DataField="CiudadActual" HeaderText="Ciudad Actual" SortExpression="CiudadActual" />
<asp:BoundField DataField="Distancia" HeaderText="Distancia" SortExpression="Distancia" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" />
<asp:BoundField DataField="TiempoParaCita" HeaderText="Tiempo para Cita" SortExpression="TiempoParaCita" />
<asp:TemplateField>
<ItemTemplate>                                        
<asp:LinkButton ID="lkbQuitar" runat="server" Text="Quitar" OnClick="lkbQuitar_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlRecursosAsignados" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="gvSevicios" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoSevicios" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />                        
</Triggers>
</asp:UpdatePanel>
</div>      
</div>  
</div>
<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE SERVICIOS ASIGNADOS/TERMINADOS A UNA UNIDAD -->
<div id="contenidoServiciosAsignados" class="modal">
<div id="modalServiciosAsignados" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarServiciosAsignados" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarServiciosAsignados" runat="server" Text="Cerrar" OnClick="lnkCerrarServiciosAsignados_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Documento.png"/>
<h2>
<asp:updatepanel runat="server" ID="uplblServicios" UpdateMode="Conditional">
<ContentTemplate>
<asp:label runat="server" ID="lblServicios" Text="Servicios Asignados"></asp:label>
</ContentTemplate>                   
</asp:updatepanel>
</h2>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvServiciosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosAsignados" runat="server" CssClass="gridview" Width="100%" AllowPaging="True" OnPageIndexChanging="gvServiciosAsignados_PageIndexChanging"
ShowFooter="True" AutoGenerateColumns="False" PageSize="5" OnRowDataBound="gvServiciosAsignados_RowDataBound">
<Columns>
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignación" SortExpression="FechaAsignacion" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
<asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
<ItemTemplate>
<asp:Label ID="lblCliente" runat="server" ToolTip='<%# Eval("Cliente") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Cliente").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sitio Carga" SortExpression="Origen">
<ItemTemplate>
<asp:Label ID="lblOrigen" runat="server" ToolTip='<%# Eval("Origen") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Origen").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CiudadOrigen" HeaderText="Ciudad" SortExpression="CiudadOrigen" />
<asp:BoundField DataField="CitaOrigen" HeaderText="Cita" SortExpression="CitaOrigen" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="Sitio Descarga" SortExpression="Destino">
<ItemTemplate>
<asp:Label ID="lblDestino" runat="server" ToolTip='<%# Eval("Destino") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Destino").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CiudadDestino" HeaderText="Ciudad" SortExpression="CiudadDestino" />
<asp:BoundField DataField="CitaDestino" HeaderText="Cita " SortExpression="CitaDestino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:HyperLink ID="lkbMapa" Target="_blank" runat="server" Text="Ver Mapa"></asp:HyperLink>
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
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>            
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE ANTICIPOS REALIZADOS A LA ENTIDAD -->
<div id="contenidoAnticiposPendientes" class="modal">
<div id="modalAnticiposPendientes" class="contenedor_modal_seccion_completa"">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAnticipos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAnticiposP" runat="server" Text="Cerrar" OnClick="lkbCerrarAnticiposP_Click"> 
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Depositos.png"/>
<h2>Depositos y Vales Asignados</h2>
</div>            
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvAnticiposPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAnticiposPendientes" runat="server" CssClass="gridview" Width="100%" AllowPaging="True"
ShowFooter="True" AutoGenerateColumns="False" PageSize=" 10">
<Columns>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Num" HeaderText="Folio" SortExpression="Num" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Precio" HeaderText="Costo" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Monto" HeaderText="Total" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización " SortExpression="FechaAutorizacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaCargaoDeposito" HeaderText="Fecha Carga o Depósito" SortExpression="FechaCargaoDeposito" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:TemplateField HeaderText="Movimiento" SortExpression="Movimiento">
<ItemTemplate>
<asp:Label ID="lblMovimiento" runat="server" ToolTip='<%# Eval("Movimiento") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Movimiento").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Información" SortExpression="Informacion">
<ItemTemplate>
<asp:Label ID="lblInformacion" runat="server" ToolTip='<%# Eval("Informacion") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Informacion").ToString(), 25, "...") %>'></asp:Label>
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
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA EL CONTROL DE USUARIO QUE PERMITE REALIZAR LA REUBICACION DE UNIDADES -->
<div id="contenidoConfirmacionUbicacion" class="modal">
<div id="confirmacionUbicacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img  src="../Image/IndicadorUnidadesPatio.png"/>
<h2>Reubicacion de Unidades</h2>
</div>           
<asp:UpdatePanel ID="upwucReubicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<operacion:wucMovimientoVacioSinOrden ID="wucReubicacion" runat="server" OnClickRegistrar="wucReubicacion_OnClickRegistrar" OnClickCancelar="wucReubicacion_OnClickCancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>          
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA LOS DATOS DE LA UBICACION ORIGEN O DESTINO SELECCIONADA -->
<div id="contenidoDatosUbicacion" class="modal">
<div id="datosUbicacion" class="contenedor_ventana_confirmacion"> 
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Direccion.png" />
<h2>
<asp:UpdatePanel runat="server" ID="uplblNombre" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNombre"></asp:Label>
</ContentTemplate>                        
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSevicios" />
</Triggers>
</asp:UpdatePanel>
</h2>
</div>           
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="lblDireccionUbicacion">DIRECCIÓN:</label>
</div>
<asp:UpdatePanel runat="server" ID="uplblDireccionUbicacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblDireccionUbicacion"></asp:Label>
</ContentTemplate>                        
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSevicios" />
</Triggers>
</asp:UpdatePanel>                    
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblGeoubicacion">GEOUBICACIÓN:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uplblGeoubicacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblGeoubicacion" ></asp:Label>
</ContentTemplate>                        
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSevicios" />
</Triggers>
</asp:UpdatePanel>   
</div>                                    
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lblTelefono">TELEFONO:</label>
</div>
<asp:UpdatePanel runat="server" ID="uplblTelefono" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTelefono"></asp:Label>
</ContentTemplate>                        
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvSevicios" />
</Triggers>
</asp:UpdatePanel>                    
</div>
</div>
</div>
</div> 
<!-- VENTANA MODAL QUE MUESTRA EL MENSAJE DE CONFIRMACION EN LOS CASOS EN LOS QUE SE AGREGA UNA UNIDAD LIGADA A UN OPERADOR -->
<div id="contenidoConfirmacionAsignacionRecurso" class="modal">
<div id="confirmacionAsignacionRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Confirmación de asignación</h2>
</div>              
<div class="columna2x">               
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeAsignacion" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarAsignacion" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarAsignacion" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarAsignacion_OnClick" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAsignacion" runat="server" CssClass="boton" OnClick="btnAceptarAsignacion_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA EL MENSAJE DE CONFIRMACION EN LOS CASOS EN LOS QUE SE DESASIGNA UNA UNIDAD LIGADA A UN OPERADOR -->
<div id="contenidoConfirmacionQuitarRecursos" class="modal">
<div id="confirmacionQuitarRecursos" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Confirmación de eliminación de asignación</h2>
</div>
<div class="columna">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensaje" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarEliminacionRecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarEliminacionRecurso" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminacionRecurso" runat="server" CssClass="boton" OnClick="btnAceptarEliminacionRecurso_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE INDICA LA EXISTENCIA DE VENCIMIENTOS ACTIVOS  -->
<div id="modalIndicadorVencimientos" class="modal">
<div id="contenidoModalIndicadorVencimientos" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<asp:UpdatePanel ID="upimgAlertaVencimiento" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:Image ID="imgAlertaVencimiento" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<h3>¡Existen Vencimientos Activos!</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeHistorialVencimientos" runat="server" CssClass="mensaje_modal"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplkbVerHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerHistorialVencimientos" runat="server" Text="Mostrar Vencimientos Activos"
OnClick="lkbVerHistorialVencimientos_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarIndicadorVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarIndicadorVencimientos" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarIndicadorVencimientos_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA LOS VENCIMIENTOS ACTIVOS -->
<div id="modalHistorialVencimientos" class="modal">
<div id="vencimientosRecurso" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialVencimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialVencimientos" runat="server" OnClick="lkbCerrarHistorialVencimientos_Click" CommandName="Historial" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Calendar2.png" />
<h2>Vencimientos Activos</h2>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvVencimientos" runat="server"  AutoGenerateColumns="False" PageSize="10"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvVencimientos_PageIndexChanging" AllowPaging="True" OnRowDataBound="gvVencimientos_RowDataBound" >
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" />
<asp:BoundField DataField="TipoRecurso" HeaderText="TipoRecurso" />
<asp:BoundField DataField="Recurso" HeaderText="Recurso" />
<asp:BoundField DataField="TipoVencimiento" HeaderText="Tipo" SortExpression="TipoVencimiento" />
<asp:BoundField DataField="Prioridad" HeaderText="Prioridad" SortExpression="Prioridad" />
<asp:BoundField DataField="FechaInicio" HeaderText="Inicio" SortExpression="FechaInicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />                                  
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
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
</asp:UpdatePanel> 
</div>
</div>
</div>  
<!-- VENTANA MODAL QUE ADVIERTE SOBRE LA LIBERACIÓN DE TRACTORES Y OPERADORES VICULADOS -->
<div id="contenidoConfirmacionLiberacionRecurso" class="modal">
<div id="confirmacionLiberacionRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Liberación de Recursos Vinculados</h2>
</div>            
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeLiberacionRecurso" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarLiberacionRecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarLiberacionRecurso" runat="server"  OnClick="btnCancelarLiberacionRecurso_Click" CssClass="boton_cancelar"  Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarLiberacionRecurso"  OnClick="btnAceptarLiberacionRecurso_Click" runat="server" CssClass="boton"  Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</asp:Content>

