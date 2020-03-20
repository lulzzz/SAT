<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucHistorialViajes.ascx.cs" Inherits="SAT.UserControls.wucHistorialViajes" %>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryHistorialViajes();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryHistorialViajes() {
    $(document).ready(function () {

        //Añadiendo Función de Autocompletado al Control
        $("#<%=txtCliente.ClientID%>").autocomplete({
            source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
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

        //Añadiendo Validación al Evento Click del Boton
        $("#<%=btnBuscar.ClientID%>").click(function () {
            var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
            var isValid2;
            var isValid3;

            //Validando el Control
            if ($("#<%=chkCitaCarga.ClientID%>").is(':checked') == true || $("#<%=chkCitaDescarga.ClientID%>").is(':checked') == true || $("#<%=chkDocumentacion.ClientID%>").is(':checked') == true) {
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

        <%--$('#<%=gvViajes.ClientID %>').gridviewScroll({
            width: document.getElementById("contenedorUCHistorialViajes").offsetWidth - 15,
            height: 1200
        });--%>

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

//Invocando Función
ConfiguraJQueryHistorialViajes();
</script>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/EnTransito.png" />
<h2>Viajes</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtNoServicio">No. Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCartaPorte">Carta Porte</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCartaPorte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox" MaxLength="30"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
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
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="upchkCitaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkCitaCarga" runat="server" Text="Cita Carga" Checked="true" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkCitaDescarga" runat="server" Text="Cita Descarga" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkDocumentacion" runat="server" Text="Documentación" />
</ContentTemplate>
<Triggers>
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
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
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
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="header_seccion">
<h2>Resultado Obtenidos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="14" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoFI">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
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
<div id="contenedorUCHistorialViajes" class="grid_seccion_completa_encabezado_fijo">
<asp:UpdatePanel ID="upgvViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvViajes" runat="server" AllowPaging="True" AllowSorting="True" TabIndex="16"
OnPageIndexChanging="gvViajes_PageIndexChanging" OnSorting="gvViajes_Sorting"
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
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Liquidado" HeaderText="Liquidado" SortExpression="Liquidado" />
<asp:BoundField DataField="Documentacion" HeaderText="Documentacion" SortExpression="Documentacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="OperacionAlcance" HeaderText="Operacion/Alcance/Servicio" SortExpression="OperacionAlcance" ItemStyle-CssClass="label_correcto" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="CitaCarga" HeaderText="Cita Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegadaCarga" HeaderText="Fecha Llegada" SortExpression="FechaLlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="EstatusLlegadaCarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaCarga" />
<asp:BoundField DataField="RazonLlegadaTardeCarga" HeaderText="Razon Llegada Carga" SortExpression="RazonLlegadaTardeCarga" />
<asp:BoundField DataField="FechaSalidaCarga" HeaderText="Fecha Salida" SortExpression="FechaSalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="RazonSalidaTardeCarga" HeaderText="Razon Salida Carga" SortExpression="RazonSalidaTardeCarga" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="CitaDescarga" HeaderText="Cita Descarga" SortExpression="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaLlegadaDescarga" HeaderText="Fecha Llegada" SortExpression="FechaLlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="EstatusLlegadaDescarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaDescarga" />
<asp:BoundField DataField="RazonLlegadaTardeDescarga" HeaderText="Razon Llegada Descarga" SortExpression="RazonLlegadaTardeDescarga" />
<asp:BoundField DataField="RazonSalidaTardeDescarga" HeaderText="Razon Salida Descarga" SortExpression="RazonSalidaTardeDescarga" />
<asp:BoundField DataField="FechaInicioDescarga" HeaderText="Inicio Descarga" SortExpression="FechaInicioDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaTerminoDescarga" HeaderText="Termino Descarga" SortExpression="FechaTerminoDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje" />
<asp:BoundField DataField="FechaTermino" HeaderText="Fecha de Termino" SortExpression="FechaTermino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Confirmacion" HeaderText="Confirmacion" SortExpression="Confirmacion" />
<asp:BoundField DataField="MotivoCancelacion" HeaderText="Motivo Cancelacion" SortExpression="MotivoCancelacion" />
<asp:BoundField DataField="CartaPorte" HeaderText="CartaPorte" SortExpression="CartaPorte" />
<asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
<asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor" />
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms" />
<asp:BoundField DataField="Rendimiento" HeaderText="Rendimiento (Kms/Lts)" SortExpression="Rendimiento" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
<asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas" />
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>