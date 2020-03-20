<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucBitacoraMonitoreoHistorial.ascx.cs" Inherits="SAT.UserControls.wucBitacoraMonitoreoHistorial" %>
<!--hoja de estilos que dan formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
//Obtiene la instancia actual de la pagina y añade un manejador de eventos
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Creación de la función que permite finalizar o validar los controles a partir de un error.
function EndRequestHandler(sender, args) {
//Valida si el argumento de error no esta definido
if (args.get_error() == undefined) {
//Invoca a la Funcion ConfiguraJQueryBitacoraMonitoreoHistorial
ConfiguraJQueryBitacoraMonitoreoHistorial();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryBitacoraMonitoreoHistorial() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable validaBitacoraMonitoreoHistorial
var validaBitacoraMonitoreoHistorial = function (evt) {
//Creación de las variables y asignacion de los controles de la pagina BitacoraMonitoreoHistorial
var isValid1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
//Devuelve un valor a la funcion
return isValid1 && isValid2;
};
//Permite que los eventos de guardar activen la funcion de validación de controles.
    $("#<%=btnBusca.ClientID%>").click(validaBitacoraMonitoreoHistorial);
    $("#<%=lkbMapsTipo.ClientID%>").click(validaBitacoraMonitoreoHistorial);
});
    $(document).ready(function () {
        $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
    });

    $(document).ready(function () {
        $("#<%=txtFechaFin.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
    });
}
    ConfiguraJQueryBitacoraMonitoreoHistorial();

    //Declarando Función de Validación de Fechas
    function CompareDates() {
        //Obteniendo Valores
        var txtDate1 = $("#<%=txtFechaInicio.ClientID%>").val();
        var txtDate2 = $("#<%=txtFechaFin.ClientID%>").val();

        //Fecha en Formato MM/DD/YYYY
        var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
        var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

        //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
        if (date1 > date2)
            //Mostrando Mensaje de Operación
            return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
    }
</script>
<div>
<div class="header_seccion">
<img src="../Image/Calendar2.png" />
<h2>Bitácora Monitoreo</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaInicio">Fecha Inicio:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox validate[required,custom[dateTime24], funcCall[CompareDates[]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_80px">
<label for="txtNoServicio">No. Servicio</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox_100px"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaFin">Fecha Fin:</label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uptxtFechaFin" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipo">Tipo: </label>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="upddlTipo" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList runat="server" ID="ddlTipo" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="uphylMapsTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbMapsTipo" runat="server" OnClick="lkbMapsTipo_Click">
    <img src="../Image/ImagenPatio.png" width="20" height="20" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbMapsTipo" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusca" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevoBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevoBitacoraMonitoreo" runat="server" CssClass="boton" Text="Nuevo" OnClick="btnNuevoBitacoraMonitoreo_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBusca" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBusca" runat="server" CssClass="boton_cancelar" Text="Buscar" OnClick="btnBusca_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnProveedorGPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnProveedorGPS" runat="server" CssClass="boton" Text="Ubicación GPS" OnClick="btnProveedorGPS_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnSolicitarUbicacionMovil" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnSolicitarUbicacionMovil" runat="server" CssClass="boton" Text="Ubicación APP" OnClick="btnSolicitarUbicacionMovil_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoBitacoraMonitoreo">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoBitacoraMonitoreo" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoBitacoraMonitoreo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoBitacoraMonitoreo" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacoraMonitoreo" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarBitacoraMonitoreo" runat="server" TabIndex="5" 
OnClick="lkbExportarBitacoraMonitoreo_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarBitacoraMonitoreo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvBitacoraMonitoreo" runat="server"  AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvBitacoraMonitoreo_PageIndexChanging" OnSorting="gvBitacoraMonitoreo_Sorting" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvBitacoraMonitoreo_RowDataBound" >
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="FechaBitacora" HeaderText="Fecha Bitácora" SortExpression="FechaBitacora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="Diferencia Fecha" SortExpression="DiferenciaFecha">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:Label ID="lblDifFechas" runat="server" CssClass="label_error" Text='<%# Eval("DiferenciaFecha") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="UbicacionSistema" HeaderText="Ubicación Sistema" SortExpression="UbicacionSistema" />
<asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" />
<asp:BoundField DataField="Comentario" HeaderText="Comentario" SortExpression="Comentario" />
<asp:BoundField DataField="Evidencias" HeaderText="Evidencias" SortExpression="Evidencias" />
<asp:TemplateField HeaderText="MAPS" SortExpression="Maps">
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:HyperLink ID="hylMaps" runat="server" Target="_blank" ToolTip="Visualize la Ubicación">
<img src="../Image/ImagenPatio.png" width="28" height="28" />
</asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbConsultarVencimiento" OnClick="lkbBitacoraMonitoreoClick" runat="server" Text="Consultar" Enabled="true"  CommandName="Consultar"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoBitacoraMonitoreo" />
<asp:AsyncPostBackTrigger ControlID="btnBusca" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnSolicitarUbicacionMovil" />
<asp:AsyncPostBackTrigger ControlID="btnSeleccionar" />
<asp:AsyncPostBackTrigger ControlID="btnProveedorGPS" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>

<!-- Ventana de Selección de Proveedor GPS -->
<div id="contenedorVentanaProveedorGPS" class="modal">
<div id="ventanaProveedorGPS" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarProveedorGPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarProveedorGPS" runat="server" CommandName="ProveedorGPS" OnClick="lkbCerrarProveedorGPS_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Seleccione su Proveedor de GPS</h2>
</div>
<div class="renglon2x">
    <div class="control2x">
        <asp:UpdatePanel ID="upddlServicioGPS" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:DropDownList ID="ddlServicioGPS" runat="server" CssClass="dropdown2x"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProveedorGPS" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="controlBoton">
        <asp:UpdatePanel ID="upbtnSeleccionar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" OnClick="btnSeleccionar_Click" CssClass="boton" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnProveedorGPS" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
</div>

</div>
</div>