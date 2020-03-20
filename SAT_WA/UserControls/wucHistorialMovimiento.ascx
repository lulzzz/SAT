<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucHistorialMovimiento.ascx.cs" Inherits="SAT.UserControls.wucHistorialMovimiento" %>
<%@ Register   Src="~/UserControls/wucRuta.ascx" TagPrefix="tectos" TagName="wucCalcularRuta" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryHistorialMovimiento();
        }
    }

    //Función de Configuración
    function ConfiguraJQueryHistorialMovimiento() {
        $(document).ready(function () {

            //Función de Validación del Control
            $("#<%=btnBuscar.ClientID%>").click(function () {
        //Validando Control
        var isValid1 = !$("#<%=txtRecursoAsignado.ClientID%>").validationEngine('validate');
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

    //Devolviendo Resultado Obtenido
    return isValid1 && isValid2 && isValid3;
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

    //Obteniendo Tipo de Entidad
    var tipoEntidad = $("#<%=ddlTipoAsignacion.ClientID%>").val();
    //Función de validación Hacer Servicio
    var validacionHacerServicio = function (evt) {
        //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
        var isValid1 = !$("#<%=txtClienteHacerServicio.ClientID%>").validationEngine('validate');

    return isValid1
};
    //Botón Aceptar Hacer Servicio
$("#<%= btnAceptarHacerServicio.ClientID %>").click(validacionHacerServicio);
    // *** Catálogos Autocomplete *** //
    //Cliente  Hacer Servicio
    $("#<%=txtClienteHacerServicio.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
    appendTo: "#confirmacionHacerServicio"
});
    //Evento 
    $("#<%=ddlTipoAsignacion.ClientID%>").change(function () {

        //Limpiando Control
        $("#<%=txtRecursoAsignado.ClientID%>").val('');
    //Obteniendo Valor del Tipo de Entidad
    tipoEntidad = this.value;
    //Cargando Catalogo Autocompleta
    CargaAutocompleta();
});


    //Declarando Función de Autocompleta
    function CargaAutocompleta() {
        //Validando Tipo de Entidad
        switch (tipoEntidad) {
            case "1":
                {
                    //Cargando Catalogo de Unidades
                    $("#<%=txtRecursoAsignado.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
});
    break;
}
    case "2":
        {
            //Cargando Catalogo de Operadores
            $("#<%=txtRecursoAsignado.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
});
    break;
}
    default:
        {
            //Cargando Catalogo de Unidades
            $("#<%=txtRecursoAsignado.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
    appendTo: "#confirmacionHacerServicio"
});
    break;
}
}
}

    //Cargando Catalogo Autocompleta
    CargaAutocompleta();

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
ConfiguraJQueryHistorialMovimiento();
</script>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Transportista.png" />
<h2>Historial</h2>
</div>
<div class="seccion_controles">
<div class="columna3x">
<div class="renglon2x">
<div class="etiqueta">
<label>Tipo Asignación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoAsignacion" runat="server" CssClass="dropdown2x" AutoPostBack="true"
OnSelectedIndexChanged="ddlTipoAsignacion_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Recurso Asignado</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtRecursoAsignado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRecursoAsignado" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Desde:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir Fecha de Fin?" TabIndex="7" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Hasta</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="9" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar"
OnClick="btnBuscar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="renglon3x"></div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado Por:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvHistorialAsignacion" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="uplnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_encabezado_fijo" id="contenedorUCHistorialAsignacion">
<asp:UpdatePanel ID="upgvHistorialAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvHistorialAsignacion" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" OnSorting="gvHistorialAsignacion_Sorting" PageSize="25"  OnRowDataBound="gvHistorialAsignacion_RowDataBound"
OnPageIndexChanging="gvHistorialAsignacion_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio">
<ItemTemplate>
<asp:LinkButton ID="lkbNoServicio" Enabled="false" OnClick="lkbHacerServicio_Click" runat="server" Text='<%# Eval("NoServicio")%>' ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
<asp:BoundField DataField="EstatusMov" HeaderText="Estatus" SortExpression="EstatusMov" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
<asp:BoundField DataField="EstatusDoc" HeaderText="Estatus Documentos" SortExpression="EstatusDoc" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
<asp:BoundField DataField="Liquidado" HeaderText="Liquidado" SortExpression="Liquidado" />
<asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor" />
<asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque" />
<asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
<asp:BoundField DataField="Tercero" HeaderText="Tercero" SortExpression="Tercero" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente"  />
<asp:BoundField DataField="ParadaOrigen" HeaderText="Parada Origen" SortExpression="ParadaOrigen" />
<asp:BoundField DataField="ParadaDestino" HeaderText="Parada Destino" SortExpression="ParadaDestino" />
<asp:BoundField DataField="FechaIni" HeaderText="Fecha Inicio" SortExpression="FechaIni" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="KMS" SortExpression="Kms">
<ItemTemplate>
<asp:LinkButton ID="lkbKms" runat="server" Text='<%# Eval("Kms")%>' OnClick="lkbKms_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>    
<asp:BoundField DataField="TiempoTransitoEstimado" HeaderText="Tiempo Transito (Estimado)" SortExpression="TiempoTransitoEstimado" />
<asp:BoundField DataField="TiempoTransitoReal" HeaderText="Tiempo Transito (Real)" SortExpression="TiempoTransitoReal" />
<asp:TemplateField HeaderText="Porte" SortExpression="CartaPorte">
<ItemTemplate>
<asp:LinkButton ID="lkbCartaPorte"  OnClick="lkbCartaPorte_Click" runat="server" Text='<%# Eval("CartaPorte")%>' ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
<ItemTemplate>
<asp:LinkButton ID="lkbReferenciaServicio" runat="server" Text='<%# Eval("Referencia")%>' OnClick="lkbReferenciaServicio_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Depósitos" SortExpression="Depositos">
<ItemTemplate>
<asp:LinkButton ID="lkbDeposito"  CommandName="Deposito"  runat="server" Text='<%# Eval("Depositos")%>' OnClick="lkbAnticipos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Diesel Motriz(lts)" SortExpression="DieselMotriz">
<ItemTemplate>
<asp:LinkButton ID="lkbDieselMotriz" Text='<%# Eval("DieselMotriz")%>' runat="server" CommandName="DieselMotriz" OnClick="lkbAnticipos_Click"></asp:LinkButton>
<br />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Diesel Arrastre(lts)" SortExpression="DieselArrastre">
<ItemTemplate>
<asp:LinkButton ID="lkbDieselArrastre" Text='<%# Eval("DieselArrastre")%>' runat="server" CommandName="DieselArrastre" OnClick="lkbAnticipos_Click"></asp:LinkButton>
<br />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="KmsAcomulados" HeaderText="Kms Acumulado (Diesel)" SortExpression="KmsAcomulados" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
<asp:BoundField DataField="LitrosLectura" HeaderText="Diesel (Lectura lts)" SortExpression="LitrosLectura" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
<asp:BoundField DataField="DiferenciaDiesel" HeaderText="Diferencia Diesel (Vale VS Lectura)" SortExpression="DiferenciaDiesel" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
<asp:TemplateField HeaderText="LtsDespuesCargar" SortExpression="LtsDespuesCargar">
<ItemStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lkbLtsDespuesCargar" runat="server" Text='<%# Eval("LtsDespuesCargar")%>' OnClick="lkbLtsDespuesCargar_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Rendimiento" HeaderText="Rendimiento (Kms/Lts)" SortExpression="Rendimiento" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:0.00}" />
<asp:TemplateField HeaderText="" SortExpression="Ruta">
<ItemTemplate>
<asp:LinkButton ID="lkbRuta" Text="Ruta" runat="server" CommandName="Ruta" OnClick="lkbAnticipos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Devolución" SortExpression="NoDevolucion">
<ItemTemplate>
<asp:LinkButton ID="lkbDevolucion" runat="server" Text='<%# Eval("NoDevolucion")%>' OnClick="lkbDevolucion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Movimiento" SortExpression="AccionMovimiento">
<ItemTemplate>
<asp:LinkButton ID="lkbAccionMovimiento" runat="server" Text='<%# Eval("AccionMovimiento")%>'  CommandName="Eliminar"  OnClick="lkbAccionMovimiento_Click"  ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField >
<ItemTemplate>
<asp:LinkButton runat="server" ID="lkbBitacora" Text="Bitacora" OnClick="lkbBitacora_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Remolque3ero1" HeaderText="Remolque de 3ero 1" SortExpression="Remolque3ero1" />
<asp:BoundField DataField="Remolque3ero2" HeaderText="Remolque de 3ero 2" SortExpression="Remolque3ero2" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna3x">
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplblKms" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblKms" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<label for="lblKms">Total KMS</label>
</div>
</div>
</div>
<div id="contenidoConfirmacionHacerServicio" class="modal">
<div id="confirmacionHacerServicio" class="contenedor_ventana_confirmacion">   
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHacerServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHacerServicio"    OnClick="lkbCerrarHacerServicio_Click" runat="server" Text="Cerrar" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>                        
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h2>Convertir Movimiento Vacio a Servicio</h2>
</div>
<div class="columna">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtClienteHacerServicio">Cliente</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uptxtClienteHacerServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtClienteHacerServicio" runat="server"  CssClass="textbox2x validate[required, custom[IdCatalogo]]"  ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
<asp:AsyncPostBackTrigger ControlID="gvHistorialAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorHacerServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorHacerServicio" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarHacerServicio" />
<asp:AsyncPostBackTrigger ControlID="gvHistorialAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarHacerServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarHacerServicio" runat="server"   OnClick="btnAceptarHacerServicio_Click" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<!-- VENTANA MODAL QUE PERMITE REALIZAR  EL CALCULO DE RUTA-->
<div id="contenidoCalcularRuta" class="modal">
<div id="confirmacionCalcularRuta" class="contenedor_modal_seccion_completa_arriba"   style="width:1230px" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCalcularRuta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCalcularRuta" runat="server" CommandName="calcularRuta"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>  
<div class="header_seccion">
<img src="../Image/EntradasSalidas.png" />
<h2>Calcular Ruta</h2>
</div> 
<div class="columna2x">
<asp:UpdatePanel ID="upwucCalcularRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucCalcularRuta runat="server" ID="wucCalcularRuta" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvHistorialAsignacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>