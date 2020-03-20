<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Publicaciones.aspx.cs" Inherits="SAT.Operacion.Publicaciones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para uso de autocomplete en controles de búsqueda filtrada -->
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Biblioteca para ventana modal  -->
<script type="text/javascript" src="../Scripts/jquery.plainmodal.min.js" charset="utf-8"></script>
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryPublicacion();
        }
    }
    //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQueryPublicacion() {
        $(document).ready(function () {
            //Función de validación de campos de la creación de respuesta de una Unidad
            var validacionCraerRespuesta = function (evt) {
                var isValidP1 = !$("#<%=txtProducto.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtPeso.ClientID%>").validationEngine('validate');
    var isValidP3 = !$("#<%=txtTarifa.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtContacto.ClientID%>").validationEngine('validate');
    var isValidP5 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
    var isValidP6 = !$("#<%=txtObservacion.ClientID%>").validationEngine('validate');

    return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6;
};
    //Función de validación de campos de la creación de Respuesta de un Servicio
    var validacionCraerRespuestaPS = function (evt) {
        var isValidP1 = !$("#<%=txtTarifaPS.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtContactoPS.ClientID%>").validationEngine('validate');
    var isValidP3 = !$("#<%=txtTelefonoPS.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtObservacionPS.ClientID%>").validationEngine('validate');
    return isValidP1 && isValidP2 && isValidP3 && isValidP4;
};
    //Validación de controles de Inserción de Ciudades
    var validacionEventosDeseados = function () {
        var isValidP1 = !$('.scriptSecuenciaDeseada').validationEngine('validate');
        var isValidP2 = !$('.scriptCiudadDeseada').validationEngine('validate');
        var isValidP3 = !$('.scriptCitaDeseada').validationEngine('validate');
        return isValidP1 && isValidP2 && isValidP3;
    };
    //Función de validación de campos de la creación de Respuesta de un Servicio
    var validacionAceptarRespuesta = function (evt) {
        var isValidP1 = !$("#<%=txtTarifaAceptadaPU.ClientID%>").validationEngine('validate');
    return isValidP1;;
};
    //Validación de campos requeridos
$('.scriptGuardarEventosDeseados').click(validacionEventosDeseados);

    //Validación de controles de búsqueda
var validacionBusquedaUnidades = function () {
    var isValidP1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
    return isValidP1 && isValidP2;
};
    //Función de validación de campos de la creación de Elementos en el Diccionario
    var validacionElemento = function (evt) {
        var isValidP1 = !$("#<%= txtElemento.ClientID%>").validationEngine('validate');
        return isValidP1;;
    };
    //Validación de campos requeridos al agregar un elemntos
    $("#<%=btnAgregarElemento.ClientID%>").click(validacionElemento);
    //Validación de campos requeridos
    $("#<%=this.btnBuscarUnidades.ClientID%>").click(validacionBusquedaUnidades);
    //Validación de campos requeridos para Aceptar la Respuesta
    $("#<%= btnAceptarRespuesta.ClientID%>").click(validacionAceptarRespuesta);
    //Boton Guardar
    $("#<%=btnCrearRespuesta.ClientID%>").click(validacionCraerRespuesta);
    //Boton Guardar
    $("#<%=btnCrearRespuestaPS.ClientID%>").click(validacionCraerRespuestaPS);
    //Cargando Catalogo AutoCompleta
    $("#<%=txtProducto.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=1&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
    //Cargando Catalogo AutoCompleta Ciudades Disponibles
    $('.scriptCiudadDeseada').autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=8'
    });
    $('.scriptCitaDeseada').datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i',
    });
    /*Selectrores de fecha: Actualización de Llegadas y Salidas, Inicio y Fin de Eventos*/
    $("#<%=txtFecIni.ClientID%>").datetimepicker({
    lang: 'es',
    format: 'd/m/Y H:i'
});
    /*Selectrores de fecha: Actualización de Llegadas y Salidas, Inicio y Fin de Eventos*/
$("#<%=txtFecFin.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });

});
}
//Diseño Grid View
$("[src*=plus]").live("click", function () {
    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
    $(this).attr("src", "../Image/minus.png");
});
$("[src*=minus]").live("click", function () {
    $(this).attr("src", "../Image/plus.png");
    $(this).closest("tr").next().remove();
});
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryPublicacion();
</script>
<div id="encabezado_forma">
<img src="../Image/OperacionPatio.png" />
<h1>Publicaciones</h1>
</div>

<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaUnidades" runat="server" Text="Unidades" CssClass="boton_pestana_activo" CommandName="Unidades" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaViajes" runat="server" Text="Viajes" CssClass="boton_pestana" CommandName="Viajes" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvPublicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvPublicacion" runat="server" ActiveViewIndex="0">
<asp:View ID="vwUnidades" runat="server" >
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros de Busqueda</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCiudad">Ciudad</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCiudad" runat="server" MaxLength="100" CssClass="textbox2x" TabIndex="6"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha Inicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="2" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkIncluir" runat="server" Text="Filtrar por Fechas" TabIndex="2" />
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
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="3" MaxLength="16"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscarUnidades" runat="server" CssClass="boton"  OnClick="btnBuscarUnidades_Click" Text="Buscar" TabIndex="18" />
</div>
</div>
</div>
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Publicacion de Unidades</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoUnidades">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoUnidades" OnSelectedIndexChanged="ddlTamanoUnidades_SelectedIndexChanged" runat="server"  TabIndex="5" AutoPostBack="true" CssClass="dropdown" />
</div>
<div class="etiqueta">
<label for="lblOrdenadoUnidades">Ordenado Por:</label>
</div>
<div class="etiqueta"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenadoUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoUnidades" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto;">
<asp:UpdatePanel ID="uplkbExportarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarUnidades" runat="server" Text="Exportar Excel"  TabIndex="6"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvUnidades" OnPageIndexChanging="gvUnidades_PageIndexChanging" ShowFooter="True"  OnSorting="gvUnidades_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="7"
ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Ano" HeaderText="Año" SortExpression="Ano" />
<asp:BoundField DataField="SubTipo" HeaderText="SubTipo" SortExpression="SubTipo" />
<asp:BoundField DataField="Dimensiones" HeaderText="Dimensiones" SortExpression="Dimensiones" />
<asp:BoundField DataField="Licencia" HeaderText="Licencia" SortExpression="Licencia" />
<asp:BoundField DataField="Edad" HeaderText="Edad" SortExpression="Edad" />
<asp:BoundField DataField="RControl" HeaderText="R-Control" SortExpression="RControl" />

<asp:BoundField DataField="CiudadDisponibilidad" HeaderText="Ciudad Disponible" SortExpression="CiudadDisponibilidad" />
<asp:BoundField DataField="InicioDisponibilidad" HeaderText="Inicio Disponibilidad" SortExpression="InicioDisponibilidad" />
<asp:BoundField DataField="PorTerminar" HeaderText="Por Terminar" SortExpression="PorTerminar" />
<asp:TemplateField HeaderText="Destinos" SortExpression="CiudadDeseada" >
<ItemTemplate>
<asp:LinkButton ID="lkbCiudades" runat="server"  Text='<%# Eval("CiudadDeseada") %>' OnClick="lkbAccionUnidad_Click" CommandName="Ciudades"  ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion" />
<asp:TemplateField HeaderText="Ofertar" SortExpression="Ofertar" >
<ItemTemplate>
<asp:LinkButton ID="lkbOfertar" runat="server"  Text='<%# Eval("Ofertar") %>'  OnClick="lkbAccionUnidad_Click" CommandName='<%# Eval("Ofertar") %>'  ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Visualizacion" HeaderText="Visualización" SortExpression="Visualizacion" />
<asp:TemplateField HeaderText="Confirmación" SortExpression="Confirmacion" >
<ItemTemplate>
<asp:LinkButton ID="lkbConfirmacion" runat="server"  Text='<%# Eval("Confirmacion") %>'  OnClick="lkbAccionUnidad_Click" CommandName='<%# Eval("Confirmacion") %>'   ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrrarResultadoRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwViajes" runat="server" >
<div class="columna3x">
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Filtros de Busqueda</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCompaniaPS">Compañia</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCompaniaPS" runat="server" MaxLength="100" CssClass="textbox2x" TabIndex="6"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnBuscarPS" runat="server" CssClass="boton"  OnClick="btnBuscarPS_Click" Text="Buscar" TabIndex="18" />
</div>
</div>
</div>
<div class="header_seccion">
<img src="../Image/EnTransito.png" />
<h2>Viajes</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoViajes">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoViajes"></label>
<asp:DropDownList ID="ddlTamanoViajes" runat="server" OnSelectedIndexChanged="ddlTamanoViajes_SelectedIndexChanged" TabIndex="5" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarViajes">Ordenado Por:</label>
</div>
<div class="etiqueta"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenarViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarViajes" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbExportarViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarViajes" runat="server" Text="Exportar Excel" OnClick="lkbExportarViajes_Click" TabIndex="6"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvViajes" OnPageIndexChanging="gvViajes_PageIndexChanging" ShowFooter="True"  OnSorting="gvViajes_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="7"
ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField HeaderText="Origen" SortExpression="Origen" >
<ItemTemplate>
<asp:LinkButton ID="lkbOrigenPS" runat="server"   Text='<%# Eval("Origen") %>' OnClick="lkbAccionServicioPS_Click" CommandName="Paradas"  ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="Destino" SortExpression="Destino" >
<ItemTemplate>
<asp:LinkButton ID="lkbDestinoPS" runat="server"   Text='<%# Eval("Destino") %>' OnClick="lkbAccionServicioPS_Click" CommandName="Paradas"  ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
    <asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso"  />
     <asp:BoundField DataField="Dimensiones" HeaderText="Dimensiones" SortExpression="Dimensiones" />
<asp:BoundField DataField="Full" HeaderText="Full" SortExpression="Full" />
<asp:BoundField DataField="Maniobras" HeaderText="Maniobras" SortExpression="Maniobras" />
<asp:BoundField DataField="RC" HeaderText="RC" SortExpression="RC" />
<asp:BoundField DataField="Tarifa" HeaderText="Tarifa" SortExpression="Tarifa" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
<asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono" />
<asp:BoundField DataField="Contacto" HeaderText="Contacto" SortExpression="Contacto" />
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observación" />
<asp:TemplateField HeaderText="Ofertar" SortExpression="Ofertar" >
<ItemTemplate>
<asp:LinkButton ID="lkbOfertar" runat="server"  Text='<%# Eval("Ofertar") %>'  OnClick="lkbAccionServicioPS_Click" CommandName='<%# Eval("Ofertar") %>'  ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Visualizacion" HeaderText="Visualización" SortExpression="Visualizacion" />
<asp:TemplateField HeaderText="Confirmación" SortExpression="Confirmacion" >
<ItemTemplate>
<asp:LinkButton ID="lkbConfirmacion" runat="server"  Text='<%# Eval("Confirmacion") %>'  OnClick="lkbAccionServicioPS_Click" CommandName='<%# Eval("Confirmacion") %>'   ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoViajes" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarResultadoRespuestaPS" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnCrearRespuestaPS" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaViajes" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarPS" />
</Triggers>
</asp:UpdatePanel>
</div>
<!-- VENTANA MODAL PARA CRFEAR LA RESPUESTA DE LA PUBLICACIÓN DE VIAJE -->
<div id="ViajesCrearRespuesta" class="modal">
<div id="ViajesContenedorCrearRespuesta" class="contenedor_modal_seccion_completa_arriba" style="width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCrearRespuesta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCrearRespuesta" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="CerrarCrearRespuesta">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div> 
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/OperacionPatio.png" />
<h2>Crear Respuesta</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtEstatus">Estatus:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtEstatus" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtEstatus" Enabled="false" runat="server" CssClass="textbox"></asp:TextBox> 
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
            <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
        </Triggers>
    </asp:UpdatePanel>
</div>
   
    </div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtProducto">Producto</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtProducto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtProducto" MaxLength="50" CssClass="textbox2x validate[required,custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" /> 
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>        
     
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtPeso">Peso:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtPeso" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtPeso" runat="server" TabIndex="6" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
            <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
        </Triggers>
    </asp:UpdatePanel>
</div>
     <div class="label_negrita">
        <label>Ton.</label>
</div>
    </div>
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtTarifa">Tarifa Ofertada:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtTarifa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtTarifa"  runat="server" TabIndex="6" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox> 
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
            <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
        </Triggers>
    </asp:UpdatePanel>
</div></div>
   <div class="renglon2x">
<div class="etiqueta">
    <label for="lblTarifaOfertada">Tarifa Aceptada:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblTarifaOfertada" runat="server" ForeColor="Red" CssClass="label_negrita"></asp:Label>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
            <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
        </Triggers>
    </asp:UpdatePanel>
</div>
   
    </div>
   
    <div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtContacto" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtContacto" MaxLength="50" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
    <asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
            <div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefono">Telefono:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTelefono" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTelefono" MaxLength="20" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtObservacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtObservacion" MaxLength="150" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="controlBoton">
    <asp:UpdatePanel ID="upbtnCrearRespuesta" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnCrearRespuesta" runat="server"  OnClick="btnCrearRespuesta_Click" CssClass="boton" Text="Aceptar"  />
        </ContentTemplate>
          <Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
    </asp:UpdatePanel>
</div>
<div class="controlBoton">
    <asp:UpdatePanel ID="upbtnConfirmar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnConfirmar" runat="server"  OnClick="btnConfirmar_Click"  CssClass="boton" Text="Confirmar"  />
        </ContentTemplate>
        <Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
    </asp:UpdatePanel>
</div>
</div>  
   
</div>
<div class="columna3x">
<div class="header_seccion">
<h2>Paradas Deseadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamanoEventosDeseados">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoEventosDeseados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoEventosDeseados" runat="server"  OnSelectedIndexChanged="ddlTamanoEventosDeseados_SelectedIndexChanged" CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoEventosDeseados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoEventosDeseados" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEventosDeseados" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarEventosDeseados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarEventosDeseados" runat="server" TabIndex="5" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarEventosDeseados" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvEventosDeseados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEventosDeseados" runat="server" PageSize="25"  AutoGenerateColumns="False" OnPageIndexChanging="gvEventosDeseados_PageIndexChanging" OnSorting="gvEventosDeseados_Sorting" OnRowDataBound="gvEventosDeseados_RowDataBound"
ShowFooter="True" CssClass="gridview"  Width="100%"  AllowPaging="True" AllowSorting="True"  >
<Columns>
<asp:TemplateField HeaderText="Secuencia" SortExpression="Secuencia" >
<ItemTemplate>
<asp:Label  ID="lblSecuenciaDeseada" runat="server" Text='<%#Eval("Secuencia") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtSecuenciaDeseada" MaxLength="12" runat="server" CssClass="textbox_100px scriptSecuenciaDeseada validate[required, custom[positiveNumber]]" Text='<%# Eval("Secuencia") %>' >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Ciudad" SortExpression="Ciudad">
<ItemTemplate>
<asp:Label  ID="lblCiudadDeseada" runat="server" Text='<%#Eval("Ciudad") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtCiudadDeseada" MaxLength="100" runat="server" CssClass="textbox scriptCiudadDeseada validate[required]" Text='<%# Eval("Ciudad") %>' >
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Actividad" SortExpression="Actividad">
<ItemTemplate>
<asp:Label  ID="lblActividadDeseada" runat="server" Text='<%#Eval("Actividad") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
 <asp:DropDownList ID="ddlActividadDeseada" runat="server" CssClass="dropdown_100px"></asp:DropDownList>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Cita" SortExpression="Cita">
<ItemTemplate>
<asp:Label  ID="lblCitaDeseada" runat="server" Text='<%#Eval("Cita", "{0:dd/MM/yyyy HH:mm}") %>' ></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:TextBox ID="txtCitaDeseada" MaxLength="100" runat="server" CssClass="textbox scriptCitaDeseada validate[required]">
</asp:TextBox>
</FooterTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="">
<ItemTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar"  OnClick="lkbEliminar_Click" ></asp:LinkButton>
</ItemTemplate>
<FooterTemplate>
<asp:LinkButton ID="lkbInsertar" runat="server" CssClass="textbox scriptGuardarEventosDeseados" OnClick="lkbInsertar_Click" Text="Insertar" ></asp:LinkButton>
</FooterTemplate>
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
<asp:AsyncPostBackTrigger ControlID="gvEventosDeseados" />
<asp:AsyncPostBackTrigger ControlID="lkbCrearRespuesta" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
    
</div>
      
</div>  
</div>
<!-- Ventana Detalle de Respuestas de las Publicación de una Unidad -->
<div id="seleccionRespuestas" class="modal">
<div id="seleccionrespuestas" class="contenedor_modal_seccion_completa_arriba" style="width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSeleccionRespuestas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSeleccionRespuestas" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="SeleccionRespuestas" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Respuestas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoRespuestas">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoRespuestas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRespuestas" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoRespuestas_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoServicio">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoRespuestas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoRespuestas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRespuestas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto;">
<asp:UpdatePanel ID="uplkbExportarRespuestas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRespuestas" runat="server" Text="Exportar"   
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRespuestas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvRespuestas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="gvRespuestas" runat="server" AutoGenerateColumns="false" PageSize="25" AllowPaging="true" AllowSorting="true" TabIndex="10"  OnPageIndexChanging="gvRespuestas_PageIndexChanging"
            OnRowDataBound="gvRespuestas_OnRowDataBound" ShowHeaderWhenEmpty="True"  Width="100%"      OnSorting="gvRespuestas_Sorting"                   
            CssClass="gridview">
            <AlternatingRowStyle CssClass="gridviewrowalternate" />
            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" style="cursor: pointer" src="../Image/plus.png" />
                        <asp:Panel ID="pnlCiudadRespuesta" runat="server" Style="display: none">
                            <asp:UpdatePanel ID="upgvCiudadrespuesta" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvCiudadRespuesta" runat="server" AutoGenerateColumns="false" CssClass="gridview" GridLines="Both">
                                        <Columns>
                <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" />
                    <asp:BoundField DataField="Actividad" HeaderText="Actividad" SortExpression="Actividad" />
<asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" /> 
                                            <asp:BoundField DataField="Cita" HeaderText="Cita"  DataFormatString="{0:dd/MM/yyyy HH:mm}"  SortExpression="Cita" /> 
                                        </Columns>
                                        <FooterStyle CssClass="gridview2footer" />
                                        <HeaderStyle CssClass="gridview2header" />
                                        <RowStyle CssClass="gridview2row" />
                                        <SelectedRowStyle CssClass="gridview2rowselected" />
                                        <AlternatingRowStyle CssClass="gridview2rowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                <asp:BoundField DataField="Compania" HeaderText="Compañia" SortExpression="Compania" />
                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                <asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso" DataFormatString="{0:c}" />
                <asp:BoundField DataField="TarifaOfertada" HeaderText="Tarifa Ofertada" SortExpression="TarifaOfertada" DataFormatString="{0:c}" />
                    <asp:BoundField DataField="Observación" HeaderText="Observación" SortExpression="Observación" />
                <asp:BoundField DataField="Contacto" HeaderText="Contacto" SortExpression="Contador" />
                <asp:TemplateField HeaderText="" SortExpression="Indicador" >
<HeaderStyle Width="65px" />
<ItemStyle Width="65px" />
<ItemTemplate>
<asp:LinkButton ID="lkbAccionRespuesta" runat="server"  CommandName='<%# Convert.ToInt32(Eval("Indicador")) ==1? "Confirmar" : "Aceptar" %>' Text='<%# Convert.ToInt32(Eval("Indicador")) ==1? "Confirmar" : "Aceptar" %>' OnClick="lkbAccionRespuestaPU_Click" ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="gridviewfooter" />
            <HeaderStyle CssClass="gridviewheader" />
            <RowStyle CssClass="gridviewrow" />
            <SelectedRowStyle CssClass="gridviewrowselected" />
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvResultadoRespuesta" />
       <asp:AsyncPostBackTrigger ControlID="ddlTamanoRespuestas" />
        <asp:AsyncPostBackTrigger ControlID="lnkCerrarServicio" />
        <asp:AsyncPostBackTrigger  ControlID="btnAceptarRespuesta" />
    </Triggers>
</asp:UpdatePanel>
</div>     
</div>
</div>
<!-- Ventana Resumen de Ciudades -->
<div id="contenedorVentanaInformacionCiudades" class="modal">
<div id="ventanaInformacionCiudades"  class="contenedor_modal_seccion_completa_arriba" style="width:800px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarCiudades" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarCiudades" runat="server" CommandName="InformacionCiudades"    OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Información de las Ciudades Destinos Deseadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanociudades">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanociudades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCiudades" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoCiudades_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoCiudades">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoCiudades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCiudades" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCiudades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarCiudades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarCiudades" runat="server" Text="Exportar" CommandName="Devolucion" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarCiudades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvCiudades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCiudades" runat="server" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvCiudades_PageIndexChanging" OnSorting="gvCiudades_Sorting"
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
<asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" />
<asp:TemplateField HeaderText="Tarifa" SortExpression="Tarifa">
<ItemTemplate>
<asp:Label  ID="lblTarifaDeseada" runat="server" Text='<%#Eval("Tarifa","{0:C2}") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Anticipo" SortExpression="Anticipo">
<ItemTemplate>
<asp:Label  ID="lblAnticipoRequerido" runat="server" Text='<%#Eval("Anticipo","{0:C2}") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCiudades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- Ventana Resumen Devoluciones -->
<div id="contenedorVentanaInformacionViaje" class="modal">
<div id="ventanaInformacionViaje" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarServicio" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarServicio" runat="server" CommandName="Servicio"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Información del Viaje</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoInfoViaje">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoInfoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoInfoViaje" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoInfoViaje_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoInfoViaje">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoInfoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoInfoViaje" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvInfoViaje" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarInfoViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarInfoViaje" runat="server" Text="Exportar" CommandName="Devolucion" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarInfoViaje" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvInfoViaje" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="gvInfoViaje" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="true" TabIndex="10"
            OnRowDataBound="gvInfoViaje_RowDataBound" ShowHeaderWhenEmpty="True"  Width="100%" OnPageIndexChanging="gvInfoViaje_PageIndexChanging" OnSorting="gvInfoViaje_Sorting"
                       
            CssClass="gridview">
            <AlternatingRowStyle CssClass="gridviewrowalternate" />
            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img alt="" style="cursor: pointer" src="../Image/plus.png" />
                        <asp:Panel ID="pnlParadas" runat="server" Style="display: none">
                            <asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvParadas" runat="server" AutoGenerateColumns="false" CssClass="gridview" GridLines="Both">
                                        <Columns>
                                            <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" DataFormatString="{0:0}" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" DataFormatString="{0:dd/MM/yyyy HH:mm}" />     
                                        </Columns>
                                        <FooterStyle CssClass="gridview2footer" />
                                        <HeaderStyle CssClass="gridview2header" />
                                        <RowStyle CssClass="gridview2row" />
                                        <SelectedRowStyle CssClass="gridview2rowselected" />
                                        <AlternatingRowStyle CssClass="gridview2rowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
<asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Documentacion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Documentación" SortExpression="Documentacion">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Confirmacion" HeaderText="Confirmación" SortExpression="Confirmacion">
<ItemStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CartaPorte" HeaderText="Carta Porte" SortExpression="CartaPorte">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Observacion" HeaderText="Observación" SortExpression="Observacion" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Carga" SortExpression="CitaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbAceptarPublicacion" runat="server" Text="Asigna Viaje" OnClick="lkbAceptarPublicacion_Click" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="gridviewfooter" />
            <HeaderStyle CssClass="gridviewheader" />
            <RowStyle CssClass="gridviewrow" />
            <SelectedRowStyle CssClass="gridviewrowselected" />
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvRespuestas" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoInfoViaje" />
    <asp:AsyncPostBackTrigger ControlID="btnConfirmar" />
    </Triggers>
</asp:UpdatePanel>
</div>        
        
        
</div>
</div>
<!-- Ventana  de Confirmación de la Respuesta -->
<div id="contenidoAceptarRespuesta" class="modal">
<div id="confirmacionAceptarRespuesta" class="contenedor_ventana_confirmacion">   
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAceptarRespuesta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAceptarRespuesta" runat="server" CommandName="AceptarRespuesta"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>         
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Aceptar Respuesta</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea Aceptar la Respuesta?</label>
</div>
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtTarifaAceptadaPU">Tatifa Aceptada</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtTarifaAceptadaPU" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtTarifaAceptadaPU"  runat="server"  MaxLength="12"  CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
        </ContentTemplate>
        <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvRespuestas" />     
             <asp:AsyncPostBackTrigger ControlID="gvRespuestasPS" />   
        </Triggers>
    </asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarRespuesta" runat="server"  CssClass="boton" OnClick="btnAceptarRespuesta_Click" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<!-- Ventana Resumen  de Respuestas de una publicación de Unidad -->
<div id="contenedorResultadoRespuesta" class="modal">
<div id="ventanaResultadoRespuesta" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkResultadoRespuesta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrrarResultadoRespuesta" runat="server" CommandName="CerrarResultadoRespuesta" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Respuestas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlResultadoRespuesta">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoResultadoRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoResultadoRespuesta" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlResultadoRespuesta_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoResultadoRespuesta">Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoResultadoRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoResultadoRespuesta" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvResultadoRespuesta" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarResultadoRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarResultadoRespuesta" runat="server" Text="Exportar" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarResultadoRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbCrearRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCrearRespuesta" runat="server" Text="Crear Respuesta"    CommandName="OpcionCrearRespuesta"  OnClick="lkbCrearRespuesta_Click" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger  ControlID="gvUnidades" />
</Triggers>
  
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvResultadoRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResultadoRespuesta" runat="server" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvResultadoRespuesta_PageIndexChanging"
CssClass="gridview" ShowFooter="true" TabIndex="18"  Width="100%" OnSorting="gvResultadoRespuesta_Sorting" PageSize="25"
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
<asp:BoundField DataField="Compania" HeaderText="Compañia" SortExpression="Compania" />
 <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
 <asp:BoundField DataField="Tarifa" HeaderText="Tarifa" SortExpression="Tarifa" DataFormatString="{0:c2}" />
<asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
<asp:BoundField DataField="fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha" SortExpression="Fecha">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbVerDetalleResultado" runat="server" Text="Ver Detalles"  OnClick="lkbVerDetalleResultado_Click" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnCrearRespuesta" />
    <asp:AsyncPostBackTrigger ControlID="lkbCerrarSeleccionRespuestas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- Confirmar Respuesta de la Publicación de Unidad -->
<div id="contenidoConfirmarRespuestaPU" class="modal">
<div id="confirmacionConfirmarRespuestaPU" class="contenedor_ventana_confirmacion">   
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarConfirmarRespuestaPU" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarConfirmarRespuestaPU" runat="server" CommandName="ConfirmarRespuestaPU"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>         
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Confirmar Publicación </h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea Confirmar la Publicación </label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnConfirmarRespuesta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnConfirmarRespuesta" runat="server"  OnClick="btnConfirmarRespuesta_Click"  CssClass="boton"  Text="Confirmar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<div id="crearRespuestaPS" class="modal">
<div id="contenedorCrearRespuestaPS" class="contenedor_modal_seccion_completa_arriba" style="width:700px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCrearRespuestaPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCrearRespuestaPS" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="CrearRespuestaPS">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div> 
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/OperacionPatio.png" />
<h2>Crear Respuesta Publicación Servicio</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtEstatusPS">Estatus:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtEstatusPS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtEstatusPS" Enabled="false" runat="server" CssClass="textbox"></asp:TextBox> 
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvViajes" />
        </Triggers>
    </asp:UpdatePanel>
</div>
   
    </div>
<div class="renglon2x">
<div class="etiqueta">
    <label for="txtTarifaPS">Tarifa:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uptxtTarifaPS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TextBox ID="txtTarifaPS" runat="server" TabIndex="6" CssClass="textbox validate[required, custom[positiveNumber]]"></asp:TextBox>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvViajes" />
        </Triggers>
    </asp:UpdatePanel>
</div>
    </div>
      <div class="renglon2x">
<div class="etiqueta">
    <label for="lblTarifaOfertadaPS">Tarifa Aceptada:</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="uplblTarifaOfertadaPS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblTarifaOfertadaPS" runat="server" ForeColor="Red" CssClass="label_negrita"></asp:Label>
        </ContentTemplate>
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="gvViajes" />
        </Triggers>
    </asp:UpdatePanel>
</div>
   
    </div>
    <div class="renglon2x">
<div class="etiqueta">
<label for="txtContacto">Contacto:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtContactoPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtContactoPS" MaxLength="50" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
            <div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefonoPS">Telefono:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTelefonoPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTelefonoPS" MaxLength="20" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="etiqueta">
<label for="txtObservacion">Observación :</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtObservacionPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtObservacionPS" MaxLength="150" CssClass="textbox2x validate[required]"></asp:TextBox>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon2x">
<div class="controlBoton">
    <asp:UpdatePanel ID="upbtnCrearRespuestaPS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnCrearRespuestaPS" runat="server" OnClick="btnCrearRespuestaPS_Click"   CssClass="boton" Text="Aceptar"  />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvViajes" />
            </Triggers>
    </asp:UpdatePanel>
</div> 
 <div class="controlBoton">
    <asp:UpdatePanel ID="upbtnConfirmarPS" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button ID="btnConfirmarPS" runat="server"  OnClick="btnConfirmarPS_Click"  CssClass="boton" Text="Confirmar"  />
        </ContentTemplate>
        <Triggers>
 <asp:AsyncPostBackTrigger ControlID="gvViajes" />
</Triggers>
    </asp:UpdatePanel>
</div>  
</div>      
</div>     
</div>  
</div></div>
<div id="contenedorResultadoRespuestaPS" class="modal">
<div id="ventanaResultadoRespuestaPS" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkResultadoRespuestaPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarResultadoRespuestaPS" runat="server" CommandName="CerrarResultadoRespuestaPS" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Respuestas de la Publicación de Servicios</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddTamanoResultadoRespuestaPS">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoResultadoRespuestaPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddTamanoResultadoRespuestaPS" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddTamanolResultadoRespuestaPS_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoResultadoRespuestaPS">Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoResultadoRespuestaPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoResultadoRespuestaPS" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvInfoViaje" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportarResultadoRespuestaPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarResultadoRespuestaPS" runat="server" Text="Exportar" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarResultadoRespuestaPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvResultadoRespuestaPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResultadoRespuestaPS" runat="server" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvResultadoRespuestaPS_PageIndexChanging" OnSorting="gvResultadoRespuestaPS_Sorting"
CssClass="gridview" ShowFooter="true" TabIndex="18"  Width="100%"
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
<asp:BoundField DataField="Compania" HeaderText="Compañia" SortExpression="Compania" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Tarifa" HeaderText="Tarifa" SortExpression="Tarifa" />
<asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
<asp:BoundField DataField="fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha" SortExpression="Fecha">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbVerDetalleResultadoPS" runat="server" Text="Ver Detalles"   OnClick="lkbVerDetalleResultadoPS_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
    <asp:AsyncPostBackTrigger ControlID="btnCrearRespuestaPS" />
<asp:AsyncPostBackTrigger ControlID="ddTamanoResultadoRespuestaPS" />
    <asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
    <asp:AsyncPostBackTrigger ControlID="lkbCerrarseleccionRespuestasPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div id="seleccionRespuestasPS" class="modal">
<div id="contenedorSeleccionRespuestasPS" class="contenedor_modal_seccion_completa_arriba" style="width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarseleccionRespuestasPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarseleccionRespuestasPS" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="SeleccionRespuestasPS" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Respuestas de la Publicación de Servicio</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoRespuestasPS">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoRespuestasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRespuestasPS" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoRespuestasPS_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoServicioPS">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoRespuestasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoRespuestasPS" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRespuestasPS" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto;">
<asp:UpdatePanel ID="uplkbExportarRespuestasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRespuestasPS" runat="server" Text="Exportar"   
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRespuestasPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvRespuestasPS" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:GridView ID="gvRespuestasPS" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="true" TabIndex="10"
           ShowHeaderWhenEmpty="True"  Width="100%"     OnPageIndexChanging="gvRespuestasPS_PageIndexChanging" OnSorting="gvRespuestasPS_Sorting"                    
            CssClass="gridview">
            <AlternatingRowStyle CssClass="gridviewrowalternate" />
            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                <asp:BoundField DataField="Compania" HeaderText="Compañia" SortExpression="Compania" />
                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                <asp:BoundField DataField="TarifaOfertada" HeaderText="Tarifa Ofertada" SortExpression="TarifaOfertada" DataFormatString="{0:c}" />
                    <asp:BoundField DataField="Observación" HeaderText="Observación" SortExpression="Observación" />
                <asp:BoundField DataField="Contacto" HeaderText="Contacto" SortExpression="Contacto" />
                <asp:TemplateField HeaderText="" SortExpression="Indicador" >
<HeaderStyle Width="65px" />
<ItemStyle Width="65px" />
<ItemTemplate>
<asp:LinkButton ID="lkbAccionRespuestaPS" runat="server"  CommandName='<%# Convert.ToInt32(Eval("Indicador")) ==1? "Confirmar" : "Aceptar" %>' Text='<%# Convert.ToInt32(Eval("Indicador")) ==1? "Confirmar" : "Aceptar" %>' OnClick="lkbAccionRespuestaPS_Click" ></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="gridviewfooter" />
            <HeaderStyle CssClass="gridviewheader" />
            <RowStyle CssClass="gridviewrow" />
            <SelectedRowStyle CssClass="gridviewrowselected" />
        </asp:GridView>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvResultadoRespuestaPS" />
    <asp:AsyncPostBackTrigger ControlID="ddlTamanoRespuestasPS" />
        <asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
                        </Triggers>
</asp:UpdatePanel>
</div>     
</div>
</div>
<!-- Ventana de Elemento Faltante (Ubicación, Producto, Cliente, Concepto -->
<div id="contenidoElemento" class="modal">
<div id="confirmacionElemento"  class="contenedor_ventana_confirmacion">   
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarElemento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarElemento" runat="server" CommandName="CerrarElemento"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>         
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>    
<asp:UpdatePanel ID="uplblDescipcion" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblDescipcion" runat="server" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
<asp:AsyncPostBackTrigger ControlID ="btnConfirmarRespuesta" />
</Triggers>
</asp:UpdatePanel></h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Seleccione el elemento coincidente para su Alta?</label>
</div>
<div class="renglon2x"><div class="etiqueta">
<asp:UpdatePanel ID="uplblIdElemento" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblIdElemento" runat="server" Visible="false" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">   
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblElemnto" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblElemento" runat="server"  CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtElemento" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtElemento" Contenedor="#ventanaElemento" MaxLength="150" CssClass="textbox2x validate[required,,custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarElemento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarElemento" runat="server"  CssClass="boton"  OnClick="btnAgregarElemento_Click" Text="Agregar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"><div class="etiqueta">
<asp:UpdatePanel ID="uplblValor" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblValor" runat="server" Text="No" Visible="false" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>                
</div>
</div>
<div id="contenedorVentanaInformacionParadasPS" class="modal">
<div id="ventanaInformacionParadasPS" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarParadasPS" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarParadasPS" runat="server" CommandName="InformacionParadasPS"    OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Servicio.png" />
<h2>Información de las Paradas Deseadas</h2>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoParadasPS">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoParadasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoParadasPS" runat="server" TabIndex="17"  OnSelectedIndexChanged="ddlTamanoParadasPS_SelectedIndexChanged"
CssClass="dropdown" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoParadasPS">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoParadasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoParadasPS" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvParadasPS" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplkbExportarParadasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarParadasPS" runat="server" Text="Exportar" CommandName="Devolucion" 
TabIndex="18"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarParadasPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvParadasPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvParadasPS" runat="server" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gvParadasPS_PageIndexChanging" OnSorting="gvParadasPS_Sorting"
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
  <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" />
    <asp:BoundField DataField="Descripcion" HeaderText="Ciudad" SortExpression="Descripcion" />
        <asp:BoundField DataField="TipoEvento" HeaderText="Tipo Evento" SortExpression="TipoEvento" />
       <asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" />
     
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoParadasPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</asp:Content>
