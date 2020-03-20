<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="DepositoTesoreria.aspx.cs" Inherits="SAT.Soporte.DepositoTesoreria" %>
<%@ Register  Src="~/UserControls/wucSoporteTecnico.ascx" TagName="wucSoporteTecnico" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<style>
    #box {
        width: 250px;
        height: 50px;
        text-align: center;
        vertical-align: -webkit-baseline-middle;
        border: 2px solid #04B404;
        background-color: #00FF00;
        padding: 15px;
        font-family: Arial;
        font-size: 16px;
        margin-top: 35px;
    }
</style>
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Biblioteca para uso de carga de Archivos XML -->
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryDepositosTesorerias();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryDepositosTesorerias() {
    $(document).ready(function () {

        //Añadiendo Encabezado Fijo
        $("#<%=gvDepositoTesoreria.ClientID%>").gridviewScroll({
            width: document.getElementById("contenedorDepositoTesoreria").offsetWidth - 15,
            height: 400
        });


    $("#<%=txtUnidad.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    });

    $("#<%=txtOperador.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
    });

    $("#<%=txtTercero.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=17&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
    });


//Validación 
var validacionDepositoTesoreria = function () {

var isValidP1 = !$("#<%=txtNoDeposito.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
var isValidP4 = !$("#<%=txtTercero.ClientID%>").validationEngine('validate');
var isValidP5 = !$("#<%=txtIdentificador.ClientID%>").validationEngine('validate');
var isValidP6 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
var isValidP7 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
var isValidP8 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
var isValidP9 = !$("#<%=txtCartaPorte.ClientID%>").validationEngine('validate'); 
var isValidP10 = !$("#<%=txtNoViaje.ClientID%>").validationEngine('validate');
    return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8 && isValidP9 && isValidP10;
};
//Validación de campos requeridos
$("#<%=this.btnBuscar.ClientID%>").click(validacionDepositoTesoreria);

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFecIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
// *** Fecha de inicio, fin de Deposito (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFecFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryDepositosTesorerias();
</script>
<div id="encabezado_forma">
<img src="../Image/SignoPesos.png" />
<h1>Depósito Tesorería</h1>
</div>
<div class="contenedor_seccion_completa" style="float:left">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Buscar depósitos por</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoDeposito">No Depósito</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoDeposito" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="1" MaxLength="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label class="label">Concepto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlConceptoDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConceptoDeposito" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox2x" TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoViaje">No. Viaje</label>
</div>
<div class="control">
<asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox2x" TabIndex="4" MaxLength="500"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control">
<asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox2x" TabIndex="5" MaxLength="500"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnidad">Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="6"></asp:TextBox>
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
<label for="txtOperador">Operador</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtUnidad" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTercero">Tercero</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTercero" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="8"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdentificador">Identificador</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtIdentificador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtIdentificador" runat="server" CssClass="textbox2x " MaxLength="150" TabIndex="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkDeposito" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbDeposito" runat="server" Text="Deposito" GroupName="General" Checked="true" TabIndex="10" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbDocumentacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbDocumentacion" runat="server" Text="Documentacion" GroupName="General" TabIndex="11" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rbDeposito" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecDep">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="12" MaxLength="16" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="chkIncluir" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="13" OnCheckedChanged="chkIncluir_CheckedChanged" AutoPostBack="true" Checked="false" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecDoc">Fecha Fin.</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="14" MaxLength="16" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="chkIncluir" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError"  runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" TabIndex="15" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Depósitos</h2>
</div>
<div class="renglon4x">
<div class="etiqueta">
    <label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDepositos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoDepositos"></label>
<asp:DropDownList ID="ddlTamanoDepositos" runat="server" TabIndex="16" OnSelectedIndexChanged="ddlTamanoDepositos_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarDepositos">Ordenado Por:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenarDepositos" runat="server"  UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarDepositos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvDepositoTesoreria" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarDepositos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarDepositos" runat="server" Text="Exportar" CommandName="Depositos" TabIndex="17" OnClick="lkbExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
    <asp:PostBackTrigger ControlID="lkbExportarDepositos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
    <asp:UpdatePanel ID="uplnkEliminar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="lnkEliminar" runat="server" TabIndex="18" OnClick="lnkEliminar_Click">
                <asp:Image ID="Eliminar" runat="server" ImageUrl="~/Image/CobrosRecurrentes.png" Width="20" Height="20" />
            </asp:LinkButton>            
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_encabezado_fijo" id="contenedorDepositoTesoreria">
<asp:UpdatePanel ID="upgvDepositoTesoreria" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDepositoTesoreria" runat="server" AllowPaging="True" OnPageIndexChanging="gvDepositoTesoreria_PageIndexChanging"
OnSorting="gvDepositoTesoreria_Sorting" AllowSorting="True" AutoGenerateColumns="False" TabIndex="19" ShowFooter="True" PageSize="25">
<Columns>
<asp:TemplateField SortExpression="Folio">
<HeaderTemplate>
<asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="True"
OnCheckedChanged="chkTodos_CheckedChanged" Text="Folio" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVarios" runat="server" AutoPostBack="True" OnCheckedChanged="chkTodos_CheckedChanged" />
<asp:Label ID="lblSeleccionDeposito" runat="server" Text='<%# Eval("Folio") %>'></asp:Label>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
<ItemTemplate>
<asp:Label ID="lblConcepto" runat="server" ToolTip='<%# Eval("Concepto") %>'
Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Concepto").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus"
ItemStyle-HorizontalAlign="Left" >
<ItemStyle HorizontalAlign="Left" />
</asp:BoundField>
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="MontoComprobacion" HeaderText="Monto Comp." SortExpression="MontoComprobacion" DataFormatString="{0:c2}" >
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Comprobante" HeaderText="Comprobante" SortExpression="Comprobante" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Tercero" HeaderText="Tercero" SortExpression="Tercero" />
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CartaPorte" HeaderText="Carta Porte" SortExpression="CartaPorte"/>
<asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
<asp:TemplateField HeaderText="Movimiento" SortExpression="Movimiento"  >
<ItemTemplate>
<asp:Label ID="lblMovimiento" runat="server" ToolTip='<%# Eval("Movimiento") %>'
Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Movimiento").ToString(), 50, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Efectivo" HeaderText="Efectivo" SortExpression="Efectivo" >
<ItemStyle HorizontalAlign="Center" />
</asp:BoundField>
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud"
DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Solicitadopor" HeaderText="Solicitado por" SortExpression="Solicitadopor" />
<asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización" SortExpression="FechaAutorizacion"
DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Autorizadopor" HeaderText="Autorizado por" SortExpression="Autorizadopor" />
<asp:BoundField DataField="TiempoEsperaAutorizacion" HeaderText="Tiempo Autorización" SortExpression="*TiempoEsperaAutorizacion" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaDeposito" HeaderText="Fecha depósito" SortExpression="FechaDeposito"
DataFormatString="{0:dd/MM/yyyy HH:mm}" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Depositadopor" HeaderText="Depositado por" SortExpression="Depositadopor" />
<asp:BoundField DataField="RefBancaria" HeaderText="Ref. Bancaria" SortExpression="RefBancaria" />
<asp:BoundField DataField="TiempoEsperaDeposito" HeaderText="Tiempo Depositó" SortExpression="*TiempoEsperaDeposito" />
<asp:BoundField DataField="NoLiquidacion" HeaderText="No Liq." SortExpression="NoLiquidacion" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liq." SortExpression="FechaLiquidacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="Monto Facturas Proveedor" SortExpression="MontoFacturasProveedor">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkFacturasProveedor" runat="server" Text='<%# string.Format("{0:C2}", Eval("MontoFacturasProveedor")) %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="FacturasProveedor" HeaderText="Facturas Proveedor" SortExpression="FacturasProveedor" />
<asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
<ItemTemplate>
<asp:Label ID="lblReferencia" runat="server" ToolTip='<%# Eval("Referencia") %>'
Text='<%#TSDK.Base.Cadena.TruncaCadena(Eval("Referencia").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="HERRAMIENTAS">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" Text="Bitácora" runat="server" OnClick="OnClik_Bitacora"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
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
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
    <asp:AsyncPostBackTrigger ControlID="ddlTamanoDepositos" />
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
    <asp:AsyncPostBackTrigger ControlID="gvDepositoTesoreria" />
    <asp:AsyncPostBackTrigger ControlID="lnkEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>

