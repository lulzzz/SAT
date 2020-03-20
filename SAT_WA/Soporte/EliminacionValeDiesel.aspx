<%@ Page Title="Eliminacion de Vales de Diesel" Language="C#" AutoEventWireup="true" CodeBehind="EliminacionValeDiesel.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Soporte.EliminacionValeDiesel" %>
<%@ Register  Src="~/UserControls/wucSoporteTecnico.ascx" TagName="wucSoporteTecnico" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario --> 
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar encabeados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryEliminacionValeDiesel();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryEliminacionValeDiesel() {
$(document).ready(function () {

    //Añadiendo Encabezado Fijo
    $("#<%=gvEliminacionValeDiesel.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorEliminacionValeDiesel").offsetWidth - 15,
        height: 400
    });



//Cargando Controles de Fecha
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});

//Cargando Catalogo de Autocompletado
$("#<%=txtUnidadDiesel.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

//Añadiendo Validación al Evento Click del Boton
$("#<%=btnBuscar.ClientID%>").click(function () {
var isValid1 = !$("#<%=txtUnidadDiesel.ClientID%>").validationEngine('validate');
var isValid2;
var isValid3; 

//Validando el Control
if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
//Validando Controles
isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
isValid3 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo
isValid2 = true;
isValid3 = true;
}
                 
//Devolviendo Resultados Obtenidos
return isValid1 && isValid2 && isValid3;
});
});
}
//Declarando Función de Validación de Fechas
function CompareDates() {
//Obteniendo Valores
var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

//Fecha en Formato MM/DD/YYYY
var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

//Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
if (date1 > date2)
//Mostrando Mensaje de Operación
return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
        }



//Invocando Función de Configuración
ConfiguraJQueryEliminacionValeDiesel();
</script>
<div id="encabezado_forma">
<img src="../Image/Evidencia.png" />
<h1>Eliminación de Vales de Diesel</h1>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoVale">No. Vale</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoVale" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoVale" runat="server" CssClass="textbox" TabIndex="1" MaxLength="9"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkSolicitud" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbSolicitud" runat="server" Text="Solicitud" GroupName="General" Checked="true" TabIndex="2" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkCaptura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbCarga" runat="server" Text="Carga" GroupName="General" TabIndex="3" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbSolicitud" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="5" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha Fin</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="6" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtUnidadDiesel">Unidad Diesel</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUnidadDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidadDiesel" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="7" MaxLength="120"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtNoServicio">No. Servicio</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="8" MaxLength="120"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtNoViaje">No. Viaje</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtNoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox" TabIndex="9" MaxLength="120"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="10" CssClass="boton" OnClick="btnBuscar_Click"/>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
    <h2>Vales De Diesel</h2>
</div>
<div class="renglon4x">
<div class="etiqueta">
    <label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="12" AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
    </ContentTemplate>
    <Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEliminacionValeDiesel" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
    <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="13" OnClick="lnkExportar_Click"></asp:LinkButton>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportar" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="etiqueta">
    <asp:UpdatePanel ID="uplnkEliminar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="lnkEliminar" runat="server" TabIndex="14" OnClick="lnkEliminar_Click">
                <asp:Image ID="Eliminar" runat="server" ImageUrl="~/Image/RealizadoEMail.png" Width="20" Height="20" />
            </asp:LinkButton>            
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable" id="contenedorEliminacionValeDiesel">
<asp:UpdatePanel ID="upgvEliminacionValeDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEliminacionValeDiesel" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="15"
OnPageIndexChanging="gvEliminacionValeDiesel_PageIndexChanging" OnSorting="gvEliminacionValeDiesel_Sorting"
PageSize="25" CssClass="gridview" ShowFooter="True" Width="100%" AutoGenerateColumns="False">
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
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="16" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server"
OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="17" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="NoVale" HeaderText="No. Vale" SortExpression="NoVale" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="80px" ItemStyle-Width="80px"/>
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidacion" SortExpression="NoLiquidacion" HeaderStyle-Width="70px" ItemStyle-Width="70px">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLiq" HeaderText="Fecha Liquidación" SortExpression="FechaLiq" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="70px" ItemStyle-Width="70px" Visible="false">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TipoVale" HeaderText="Tipo Vale" SortExpression="TipoVale" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
<asp:BoundField DataField="NoFactura" HeaderText="No. Factura" SortExpression="NoFactura" HeaderStyle-Width="55px" ItemStyle-Width="55px">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstacionCombustible" HeaderText="Estacion Combustible" SortExpression="EstacionCombustible" HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
<asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" HeaderStyle-Width="100px" ItemStyle-Width="100px"/>
<asp:BoundField DataField="Asignacion" HeaderText="Asignación" SortExpression="Asignacion" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="65px" ItemStyle-Width="65px">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaCarga" HeaderText="Fecha Carga" SortExpression="FechaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="65px" ItemStyle-Width="65px">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="Ref1" HeaderText="Ref. 1" SortExpression="Ref1" Visible="false" />
<asp:BoundField DataField="Ref2" HeaderText="Ref. 2" SortExpression="Ref2" Visible="false" />
<asp:BoundField DataField="PrecioCombustible" HeaderText="Precio Combustible" SortExpression="PrecioCombustible" DataFormatString="{0:C2}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" HeaderStyle-Width="70px" ItemStyle-Width="70px"  FooterStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right"/>
</asp:BoundField>
<asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" DataFormatString="{0:C2}"  FooterStyle-HorizontalAlign="Right" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="ucSoporte" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL DE SOPORTE TECNICO -->
<div id="soporteTecnicoModal" class="modal">
<div id="soporteTecnico" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEliminacionVale" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEliminacionVale" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Soporte" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucSoporteTecnico ID="ucSoporte" runat="server" TabIndex="3" OnClickGuardarSoporte="wucSoporteTecnico_ClickAceptarSoporte" OnClickCancelarSoporte="wucSoporteTecnico_ClickCancelarSoporte" />
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvEliminacionValeDiesel" />
    <asp:AsyncPostBackTrigger ControlID="lnkEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>

