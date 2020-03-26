<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ControlAccesoGeneral.aspx.cs" Inherits="SAT.ControlPatio.ControlAccesoGeneral" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
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
ConfiguraControlAcceso();
}
}
//Función de Configuración
function ConfiguraControlAcceso(){
$(document).ready(function () {
//Validando la Entrada
var validaEntrada = function () {
var isValid1 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtTransportista.ClientID%>").validationEngine('validate');
var isValid3 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');

return isValid1 && isValid2 && isValid3;
}
//Añadiendo Validación al Evento Click del Control
$("#<%=btnAgregar.ClientID%>").click(validaEntrada);



//Cargando Control DateTimePicker
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});    


});
}
        
//Invocando Función de Configuración
ConfiguraControlAcceso();
</script>
<div id="encabezado_forma">
<img src="../Image/ControlAcceso.png" />
<h1>Control Acceso</h1>
</div>
    
<div class="encabezado_acceso">
<div class="renglon_encabezado_acceso">        
<div class="etiqueta">
<label for="ddlPatio">Patio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPatio" runat="server" TabIndex="1" CssClass="dropdown2x" AutoPostBack="true"
OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="ddlAcceso">Acceso</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlAcceso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlAcceso" runat="server" TabIndex="2" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="txtFecha">Fecha</label>
</div>            
</div>
<div class="control_fecha_acceso">
<asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecha" runat="server" CssClass="txtFecha_acceso validate[required, custom[dateTime24]]" TabIndex="3"
MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
</Triggers>
</asp:UpdatePanel>
</div>           
</div>
<section class="fila_indicador">        
<a href="UnidadesDentro.aspx" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="upplblUnidades" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUnidades"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />                        
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorUnidadesPatio.png" />
</div>
<div class="leyenda_indicador">
Unidades en patio
</div>           
</a>
<a href="UnidadesDentro.aspx" class="indicador_texto">
<div class="texto_indicador">
<asp:UpdatePanel runat="server" ID="upplblTiempo" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempo"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorTiempo.png" />
</div>
<div class="leyenda_indicador">
Estancia promedio de unidades
</div>           
</a>
<a href="ReporteHistorialUnidades.aspx" class="indicador">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblEntradaSalida" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblEntradaSalida"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/EntradasSalidas.png" />
</div>
<div class="leyenda_indicador">
Entradas y salidas
</div>           
</a>
<a href="UnidadesDentro.aspx" class="indicador" >
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblCargadasxSalir" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCargadasxSalir"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" /> 
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="lnkActualizar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorUnidadesCargadas.png" />
</div>
<div class="leyenda_indicador">
Cargadas por salir
</div>           
</a>
<div class="indicador_actualiza">
<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_Click">
<img src="../Image/Reload.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel> 
</div>                 
</section>    
<div class="contenedor_botones_pestana_acceso">        
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnEntrada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEntrada" Text="Entrada" OnClick="btnVista_OnClick" CommandName="Entrada" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSalida" />
<asp:AsyncPostBackTrigger ControlID="btnEntrada" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSalida" Text="Salida" OnClick="btnVista_OnClick" runat="server" CommandName="Salida" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSalida" />
<asp:AsyncPostBackTrigger ControlID="btnEntrada" />                   
</Triggers>
</asp:UpdatePanel>
</div>
</div>    
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvControlAcceso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvControlAcceso" runat="server" ActiveViewIndex="0">
<asp:View ID="vwEntrada" runat="server">
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Entrada.png" />
<h2>Entrada</h2>
</div>
<div class="columna2x">
<div class="renglon3x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTipo" runat="server" Text="Tipo Unidad" Visible="True"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown2x" AutoPostBack="true"
TabIndex="6" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" Visible="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>     
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkMasUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkMasUnidades" runat="server" Text="Mas Unidades" TabIndex="7"
OnClick="lnkMasUnidades_Click" CommandName="Algunas"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_tipo_unidad">
<div class="imagen_tipo_unidad">
<img src="../Image/Caja2.png"/>
</div>
<div class="imagen_tipo_unidad">
<img src="../Image/Rabon2.png"  />
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_tipo_unidad">
<asp:UpdatePanel ID="uprdbRemolque" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbRemolque" runat="server" Text="Remolque" GroupName="General"
Checked="true" TabIndex="8"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_unidad_rabon">
<asp:UpdatePanel ID="uprdbRabon" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbRabon" runat="server" Text="Rabon" GroupName="General"
Checked="false" TabIndex="9" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>                                
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTransportista">Transportista</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTransportista" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTransportista" runat="server" CssClass="textbox2x validate[required]" 
TabIndex="10"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblDescripcion" runat="server" Text="No. Economico"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="12"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregar" runat="server" TabIndex="15" CssClass="boton_agregar_unidad" OnClick="btnAgregar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblIdentificador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblIdentificador" runat="server" Text="Identificador" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtIdentificador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtIdentificador" runat="server" CssClass="textbox2x validate[required]" TabIndex="13"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblReferencia" runat="server" Text="Referencia" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]" TabIndex="13"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstado">Estado</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlEstado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstado" runat="server" TabIndex="14" CssClass="dropdown2x" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<%--<div class="renglon2x">
<div class="etiqueta_tipo_unidad">
<asp:UpdatePanel ID="uprdbEntrada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbEntrada" runat="server" Text="Entrada" GroupName="Acceso"
Checked="true" TabIndex="10" vi />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_unidad_rabon">
<asp:UpdatePanel ID="uprdbSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbSalida" runat="server" Text="Salida" GroupName="Acceso"
Checked="false" TabIndex="11" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>--%>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" TabIndex="12" CssClass="boton_cancelar" 
Text="Cancelar" OnClick="btnCancelar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEntrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEntrar" runat="server" TabIndex="13" CssClass="boton" 
Text="Entrar" OnClick="btnEntrar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"></div>                            
</div>
</div>
<div class="grid_unidad_agregada">
<div class="header_seccion">                               
<h2>Entrada Agrupada</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="14" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenado">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEntidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" 
OnClick="lnkExportar_Click" TabIndex="14"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvEntidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEntidades" runat="server" CssClass="gridview" 
AllowPaging="true" AllowSorting="true" TabIndex="15" AutoGenerateColumns="false"
OnSorting="gvEntidades_Sorting" OnPageIndexChanging="gvEntidades_PageIndexChanging"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:BoundField DataField="IdTipo" HeaderText="IdTipo" SortExpression="IdTipo" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="Estado" HeaderText="Estatus" SortExpression="Estado" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="control2x">
<asp:UpdatePanel ID="uplblErrorEntrada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorEntrada" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
</Triggers>
</asp:UpdatePanel>
</div>                                    
</div>
</div>
</asp:View>
<asp:View ID="vwSalida" runat="server">
<div class="seccion_controles_busqueda_salida">
<div class="header_seccion">
<img src="../Image/Salida.png" />
<h2>Salida</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSTag" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSTag" runat="server" Text="Tag"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSTag" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSTag" runat="server" CssClass="textbox2x validate[required]"  AutoPostBack="true" TabIndex="7" 
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="gvEntidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">                            
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtBDescripcion">No.Unidad / Nombre</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtBDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtBDescripcion" runat="server" CssClass="textbox" TabIndex="16"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="txtSTag" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" TabIndex="17"
OnClick="btnBuscar_Click" Text="Buscar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">                                    
<div class="etiqueta_155px">
<label for="txtBPlacas">Placas / Identificacion</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtBPlacas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtBPlacas" runat="server" CssClass="textbox" TabIndex="18"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="txtSTag" />
</Triggers>
</asp:UpdatePanel>
</div>                                    
</div>
</div>
<div class="grid_busqueda_unidades_salir">                                
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoUnidad">Mostrar:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoUnidad" runat="server" CssClass="dropdown_100px" AutoPostBack="true"
TabIndex="19" OnSelectedIndexChanged="ddlTamanoUnidad_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoUnidad">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoUnidad" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarUnidades" runat="server" Text="Exportar" 
OnClick="lnkExportarUnidades_Click" TabIndex="20"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUnidades" runat="server" CssClass="gridview" 
AllowPaging="true" AllowSorting="true" TabIndex="21" AutoGenerateColumns="false"
OnSorting="gvUnidades_Sorting" OnPageIndexChanging="gvUnidades_PageIndexChanging"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:BoundField DataField="IdAccesoEntrada" HeaderText="IdAccesoEntrada" SortExpression="IdAccesoEntrada" Visible="false" />
<asp:TemplateField HeaderText="No. de Unidad" SortExpression="NoUnidad">
<ItemTemplate>
<asp:LinkButton ID="lnkNoUnidad" runat="server" Text='<%# Eval("NoUnidad") %>'
OnClick="lnkNoUnidad_Click" CommandName="Unidad"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="TiempoEst" HeaderText="Tiempo Estancia" SortExpression="TiempoEst" />
<asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkDarSalida" runat="server" Text="Dar Salida" OnClick="lnkDarSalida_Click" CommandName="Salida"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidadesTemp" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>                                            
</div>             
<div class="seccion_controles_busqueda_salida">
<div class="header_seccion">                               
<h2>Salida Agrupada</h2>
</div>
<div class="grid_unidad_salir">                            
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoTemp">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoTemp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoTemp" runat="server" CssClass="dropdown" AutoPostBack="true"
TabIndex="18" OnSelectedIndexChanged="ddlTamanoTemp_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoTemp">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoTemp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoTemp" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidadesTemp" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarTemp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarTemp" runat="server" Text="Exportar" 
OnClick="lnkExportarTemp_Click" TabIndex="19"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarTemp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upgvUnidadesTemp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUnidadesTemp" runat="server" CssClass="gridview" 
AllowPaging="true" AllowSorting="true" TabIndex="20" AutoGenerateColumns="false"
OnSorting="gvUnidadesTemp_Sorting" OnPageIndexChanging="gvUnidadesTemp_PageIndexChanging"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:BoundField DataField="NoUnidad" HeaderText="No. de Unidad" SortExpression="NoUnidad" />
<asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="TiempoEst" HeaderText="Tiempo Estancia" SortExpression="TiempoEst" />
<asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click1"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoTemp" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon2x"></div>
<div class="renglon_boton_salir">
<div class="control">
<asp:UpdatePanel ID="uplblErrorAsignaSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorAsignaSalida" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAsignarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarSalida" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSalida" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAsignarSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAsignarSalida" runat="server" Text="Salida" CssClass="boton"
TabIndex="21" OnClick="btnAsignarSalida_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelarSalida" />
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
<asp:AsyncPostBackTrigger ControlID="btnSalida" />
<asp:AsyncPostBackTrigger ControlID="btnEntrada" />                     
</Triggers>
</asp:UpdatePanel>
</div>
<div id="contenidoAgregarUnidades" class="modal">
<div id="agregarUnidades" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>La unidad ingresó agrupada<br />
¿Desea agregar alguna de estas unidades a la salida?
</h2>
</div>
<div class="grid_unidad_agrupada">                
<asp:UpdatePanel ID="upgvSalidas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSalidas" runat="server" CssClass="gridview" 
AllowPaging="false" AllowSorting="false" TabIndex="14" AutoGenerateColumns="false"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
</HeaderTemplate>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="IdAccesoEnt" HeaderText="IdAccesoEnt" SortExpression="IdAccesoEnt" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="TiempoEst" HeaderText="Estancia" SortExpression="TiempoEst" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="Estado" HeaderText="Estado" SortExpression="Estado" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="lnkMasUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregar" />
<asp:AsyncPostBackTrigger ControlID="btnEntrar" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarSalida" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon_boton_salir">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarSalida" runat="server" Text="Aceptar" TabIndex="20" CssClass="boton"
OnClick="btnAceptarSalida_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarSalida" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarSalida" runat="server" Text="Cancelar" TabIndex="21" CssClass="boton_cancelar"
OnClick="btnCancelarSalida_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</asp:Content>
