<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministracionLlantas.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.LLantas.AdministracionLlantas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUnidad();
}
}
//Función encargada de restablecer la Configuración de los Scripts de Validación
function ConfiguraJQueryUnidad() {
$(document).ready(function () {
//Validando que se cumplan las condiciones para Insercció de Ruta
var validaUnidad = function () {
var isValid1 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
//Devolviendo Resultado
    return isValid1;
}
//Añadiendo Funcion al Evento Click del Boton "Buscar"
$("#<%=btnBuscar.ClientID%>").click(validaUnidad);
//Añadiendo Función de Autocompletado al Control de  Unidad
$("#<%=txtUnidad.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=28&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
});
}

//Invocando Funcion de Configuración
ConfiguraJQueryUnidad();
</script>
<div id="encabezado_forma">
<img src="../Image/LLanta.png"    width="40"/>
<h1>Administración de LLantas</h1>
</div>
<div class="seccion_controles">   
<div class="columna2x">
<div class="renglon3x">
<div class="etiqueta">
<label for="txtUnidad">Unidad</label></div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidad" runat="server" TabIndex="1" CssClass="textbox2x validate[required]" MaxLength="50"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
 <div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button  ID="btnBuscar" TabIndex="24" runat="server"  OnClick="btnBuscar_Click"   CssClass="boton" Text="Buscar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>          
</div>
<div class="columna600px">
<div class="header_seccion">
<h2>Posiciones</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoPosicion">Mostrar:</label>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoPosicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoPosicion" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPosicion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarPosicion" runat="server" TabIndex="29" 
OnClick="lkbExportarPosicion_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarPosicion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvPosicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPosicion" runat="server"  TabIndex="30" AutoGenerateColumns="False" OnSorting="gvPosicion_Sorting" OnPageIndexChanging="gvPosicion_PageIndexChanging" 
ShowFooter="True" CssClass="gridview" Width="100%"  AllowPaging="True" AllowSorting="True"  PageSize="20" >
<Columns> 
<asp:BoundField DataField="Eje" HeaderText="Eje" SortExpression="Eje" />
<asp:BoundField DataField="Posicion" HeaderText="Posicion" SortExpression="Posicion" />
<asp:BoundField DataField="FechaMontaje" HeaderText="Fecha Montaje" SortExpression="FechaMontaje" />
<asp:TemplateField HeaderText="" SortExpression="Accion" >
<ItemTemplate>
<asp:LinkButton ID="lnkAccion" runat="server"  OnClick="lkbAccionPosición_Click" Text='<%# Eval("Accion") %>' CommandName='<%# Eval("Accion") %>' ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>  
<!-- VENTANA MODAL PARA MONTAR UNA LLANTA -->
<div id="seleccionContenedorMontarLLanta" class="modal">
<div id="seleccionMontarLLanta" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarMontarLlanta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarMontarLlanta" OnClick="lkbCerrarVentanaModal_Click" runat="server"  CommandName="MontarLlanta" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Tractor.png" alt="SeleccionTarea" />
<h3>Montar Llanta</h3>
</div>
<div class="columna400px">
<div class="renglon">
<label class="mensaje_modal">Seleccione el tipo de movimiento para la Posición.</label>
</div>
<div class="renglon"></div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel  ID="upbtnDesmontada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnDesmontada" runat="server" CssClass="boton" OnClick="btnNuevaMontar_Click" CommandName="Desmontadas" Text="Desmontadas" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:Button ID="btnInventario" runat="server" CssClass="boton"  OnClick="btnNuevaMontar_Click" CommandName="Inventario" Text="Inventario" />
</div>
<div class="controlBoton">    
<asp:Button ID="btnNuevaLlanta" runat="server" CssClass="boton"  OnClick="btnNuevaMontar_Click" CommandName="NuevaLlanta" Text="Nueva Llanta" />
</div>
<div class="renglon"></div>
</div>
</div>
</div>
</div>  
<!-- VENTANA MODAL PARA DESMONTAR UNA LLANTA -->
<div id="seleccionContenedorDesmontarLLanta" class="modal">
<div id="seleccionDesmontarLLanta" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDesmontarLlanta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDesmontarLlanta" OnClick="lkbCerrarVentanaModal_Click" runat="server"  CommandName="DesmontarLlanta" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/desmontar.png" alt="SeleccionTarea" />
<h3>Desmontar Llanta</h3>
</div>
<div class="columna400px">
<div class="renglon">
<label class="mensaje_modal">Seleccione el tipo de movimiento para la Posición.</label>
</div>
<div class="renglon"></div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnDesmontar" runat="server" CssClass="boton" OnClick="btnNuevaDesmontar_Click" CommandName="Desmonantar" Text="Desmontar" />
</div>
<div class="controlBoton">
<asp:Button ID="btnRotacion" runat="server" CssClass="boton"  OnClick="btnNuevaDesmontar_Click" CommandName="Rotacion" Text="Rotacion" />
</div>
<div class="renglon"></div>
</div>
</div>
</div>
</div>  
<!-- VENTANA MODAL PARA MOSTRAR LAS LLANTAS DESMONTADAS -->
<div id="contenedorLLantasDesmontadas" class="modal">
<div id="LlantasDesmontadas" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarLlantasDesmontadas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarLlantasDesmontadas" runat="server" CommandName="LlantasDesmontadas"    OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Llanta.png"  width="40" />
<h2>Llantas Desmontadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoLlantasDesmontadas">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoLlantasDesmontadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList  ID="ddlTamanoLLantasDesmontadas" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoLlantasDesmontadas_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoLlantasDesmontadas">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoLlantasDesmontadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoLlantasDesmontadas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvLlantasDesmontadas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarLlantasDesmontadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarLlantasDesmontadas" runat="server" Text="Exportar" CommandName="Devolucion" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarLlantasDesmontadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvLlantasDesmontadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLlantasDesmontadas" runat="server" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvLlantasDesmontadas_PageIndexChanging" OnSorting="gvLlantasDesmontadas_Sorting"
CssClass="gridview" ShowFooter="true" TabIndex="18"  Width="100%" PageSize="25"
AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoLlantasDesmontadas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Content>