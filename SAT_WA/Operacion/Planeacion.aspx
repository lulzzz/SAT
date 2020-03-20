<%@ Page Title="Planeación de Servicios" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Planeacion.aspx.cs" Inherits="SAT.Operacion.Planeacion" MaintainScrollPositionOnPostback="true" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreoHistorial.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreoHistorial" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreo.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreo" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucServicioCopia.ascx" TagPrefix="uc1" TagName="wucServicioCopia" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="uc1" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucHistorialMovimiento.ascx" TagPrefix="uc1" TagName="wucHistorialMovimiento" %>
<%@ Register Src="~/UserControls/wucParadaEvento.ascx" TagPrefix="uc1" TagName="wucParadaEvento" %>
<%@ Register Src="~/UserControls/wucServicioDocumentacion.ascx" TagPrefix="uc1" TagName="wucServicioDocumentacion" %>
<%@ Register Src="~/UserControls/wucPreAsignacionRecurso.ascx" TagPrefix="uc1" TagName="wucPreAsignacionRecurso" %>
<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagPrefix="tectos" TagName="wucClasificacion" %>
<%--<%@ Register  Src="~/UserControls/wucPublicacionServicio.ascx" TagPrefix="uc1" TagName="wuPublicacion" %>
<%@ Register  Src="~/UserControls/wucPublicacionUnidad.ascx" TagPrefix="uc1" TagName="wuPublicacionUnidad" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryPlaneacion();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryPlaneacion() {
$(document).ready(function () {

//Validación de controles de búsqueda de servicios
var validacionBusquedaServicios = function () {
    var isValidP1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtNoUnidadA.ClientID%>").validationEngine('validate');
    var isValidP5 = !$("#<%=txtNoOperador.ClientID%>").validationEngine('validate');
return isValidP1,isValidP4,isValidP5;
};
//Validación de campos requeridos para búsqueda de servicios
$("#<%=this.btnBuscarServicios.ClientID%>").click(validacionBusquedaServicios);

//Validación de controles de búsqueda de unidades
    var validacionBusquedaUnidades = function () {
        var isValidP1 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
        var isValidP2;
        var isValidP3;

        //Validando el Control
        if ($("#<%=chkRangoFechas.ClientID%>").is(':checked') == true) {
            //Validando Controles
            isValidP2 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
            isValidP3 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
        }
        else {
            //Asignando Valor Positivo
            isValidP2 = true;
            isValidP3 = true;
        }

        //Devolviendo Resultados Obtenidos
        return isValidP1 && isValidP2 && isValidP3;
    };
    //Función de validación de campos
    var validacionCancelacionServicio = function (evt) {
        var isValidP1 = !$("#<%=txtMotivoCancelacion.ClientID%>").validationEngine('validate');
        return isValidP1;
    }; 
    //Validación de campos requeridos para Cancelación del Servicio
    $("#<%=this.btnAceptarCancelacion.ClientID%>").click(validacionCancelacionServicio);
    //Función de validación de campos Tercero
    var validacionAgregarTercero= function (evt) {
        var isValidP1 = !$("#<%=txtTercero.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
    //Validación de campos requeridos para Agregra al tercero
    $("#<%=this.btnAgregarTercero.ClientID%>").click(validacionAgregarTercero);
//Validación de campos requeridos para búsqueda de unidades
$("#<%=this.btnBuscarUnidades.ClientID%>").click(validacionBusquedaUnidades);

/* Autocompleta ubicación actual y cliente */
    $("#<%=txtUbicacion.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'});
    $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
    $("#<%=txtNoUnidadA.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
    $("#<%=txtNoOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=38&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

    /** Carga de Estatus de Unidad **/
    $("#<%=lbxEstatus.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });
    /** Carga de Flota de Unidad **/
    $("#<%=lbxFlota.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 0
    });

    //Función de validación de campos de la creación de Elementos en el Diccionario
    var validacionElemento = function (evt) {
        var isValidP1 = !$("#<%= txtElemento.ClientID%>").validationEngine('validate');
        return isValidP1;;
    };
    //Validación de campos requeridos al agregar un elemntos
    $("#<%=btnAgregarElemento.ClientID%>").click(validacionElemento);

    //Función de validación de campos de la creación de Respuesta de un Servicio
    var validacionAceptarRespuesta = function (evt) {
        var isValidP1 = !$("#<%=txtTarifaAceptadaPU.ClientID%>").validationEngine('validate');
        return isValidP1;;
    };
    //Validación de campos requeridos para Aceptar la Respuesta
    $("#<%= btnAceptarRespuesta.ClientID%>").click(validacionAceptarRespuesta);

    /** Carga Clasificaciones del Servicio **/
    $("#<%=lbxTerminal.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });
    $("#<%=lbxOperacionServicio.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });
    $("#<%=lbxAlcance.ClientID%>").multiselect({
        selectedList: 2,
        selectall: 1
    });



});
}

//Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryPlaneacion();

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
<div id="encabezado_forma">
<img src="../Image/OperacionPatio.png" />
<h1>Planeación de Servicios</h1>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaViajes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaViajes" runat="server" Text="Servicios Activos" CssClass="boton_pestana_activo" CommandName="Servicios" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnPestanaUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnPestanaUnidades" runat="server" Text="Estatus de Unidades" CssClass="boton_pestana" CommandName="Unidades" OnClick="btnPestana_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs">
<asp:UpdatePanel ID="upmtvPlaneacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvPlaneacion" runat="server" ActiveViewIndex="0">
<asp:View ID="vwServicios" runat="server" >
<div class="columna3x">
<div class="header_seccion" style="float:left;">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Servicios</h2>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">
Cliente
</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="control">
<asp:CheckBox ID="chkDocumentados" runat="server" Checked="true" Text="Ver Documentados" />
</div>
<div class="control">
<asp:CheckBox ID="chkIniciados" runat="server" Checked="true" Text="Ver Iniciados" />
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtNoServicio">
No. Servicio
</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" MaxLength="30" TabIndex="2"></asp:TextBox>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar X Fecha"
Checked="false" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" TabIndex="3" />
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaCarga" runat="server" CssClass="label" Text="Cita Carga" GroupName="FiltroFecha" TabIndex="4" Checked="true" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbCitaDescarga" runat="server" CssClass="label" Text="Cita Descarga" GroupName="FiltroFecha" TabIndex="5" Enabled="false" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>   
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label class="Label" for="txtFechaInicio">Desde</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label class="Label" for="txtFechaFin">Hasta</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="7" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxTerminal">Terminal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxTerminal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxTerminal" SelectionMode="multiple" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxOperacionServicio">Operación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxOperacionServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxOperacionServicio" SelectionMode="multiple" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="lbxAlcance">Alcance</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplbxAlcance" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:ListBox runat="server" ID="lbxAlcance" SelectionMode="multiple" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    <div class="renglon2x">
        <div class="etiqueta">
            <label for="txtNoUnidadA">
                No. Unidad
            </label>
        </div>
        <div class="control2x">
            <asp:TextBox ID="txtNoUnidadA" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="13"></asp:TextBox>
        </div>
    </div>

    <div class="renglon2x">
        <div class="etiqueta">
            <label for="txtNoOperador">
                Operador
            </label>
        </div>
        <div class="control2x">
            <asp:TextBox ID="txtNoOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="14"></asp:TextBox>
        </div>
    </div>
<div class="renglon2x"></div>            
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnBuscarServicios" runat="server" CssClass="boton" Text="Buscar" TabIndex="8" OnClick="btnBuscarServicios_Click" />
</div>
</div>
<div class="renglon2x"></div>
</div>
<div class="columna2x">
<div class="header_seccion" style="float:left;">
<img src="../Image/carga.png" />
<h2>Resumen de Cargas y Descargas</h2>
</div>
<div class="renglon2x">    
<div class="control">
<asp:UpdatePanel ID="uprdbResumenTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbResumenTotal" runat="server" Checked="true" GroupName="Resumen" AutoPostBack="true" Text="Ver Todos" OnCheckedChanged="rdbResumenTotal_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbResumenPendientes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbResumenPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbResumenPendientes" runat="server" GroupName="Resumen" AutoPostBack="true" Text="Sólo Pendientes" OnCheckedChanged="rdbResumenPendientes_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbResumenTotal" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplkbVerResumenCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerResumenCliente" runat="server" Text="Vista por Cliente" OnClick="lkbVerResumenCliente_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_100px_altura">
<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumenSemanal" runat="server" AutoGenerateColumns="False" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="Evento" HeaderText="Evento" SortExpression="Evento" />
<asp:BoundField DataField="Lunes" HeaderText="Lunes" SortExpression="Lunes" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Martes" HeaderText="Martes" SortExpression="Martes">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Miercoles" HeaderText="Miércoles" SortExpression="Miercoles" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Jueves" HeaderText="Jueves" SortExpression="Jueves">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Viernes" HeaderText="Viernes" SortExpression="Viernes">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Sabado" HeaderText="Sábado" SortExpression="Sabado">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Domingo" HeaderText="Domingo" SortExpression="Domingo" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
</Columns>
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
<asp:AsyncPostBackTrigger ControlID="wucParadaEvento" />
<asp:AsyncPostBackTrigger ControlID="wucServicioDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
<asp:AsyncPostBackTrigger ControlID="rdbResumenPendientes" />
<asp:AsyncPostBackTrigger ControlID="rdbResumenTotal" />
<asp:AsyncPostBackTrigger ControlID="btnSemanaMenos" />
<asp:AsyncPostBackTrigger ControlID="btnSemanaMas" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x" style="margin-left:120px">
    <div class="control_60px" style="text-align:center">
        <asp:UpdatePanel ID="upbtnSemanaMenos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton ID="btnSemanaMenos" runat="server" ToolTip="Resta una Semana al Conteo" 
                    OnClick="btnConfiguraSemana_Click" CommandArgument="Menos" ImageUrl="~/Image/ButtonMinus.png" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="etiqueta_50px">
        <label>Semana: </label>
    </div>
    <div class="etiqueta_50px">
        <asp:UpdatePanel ID="uplblSemana" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblSemana" runat="server" CssClass="label_negrita"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSemanaMenos" />
                <asp:AsyncPostBackTrigger ControlID="btnSemanaMas" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="control_60px" style="text-align:center">
        <asp:UpdatePanel ID="upbtnSemanaMas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton ID="btnSemanaMas" runat="server" ToolTip="Resta una Semana al Conteo" 
                    OnClick="btnConfiguraSemana_Click" CommandArgument="Mas" ImageUrl="~/Image/ButtonPlus.png" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
</div>
</div>
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Servicios Activos</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoServicios">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoServicios" runat="server" OnSelectedIndexChanged="ddlTamanoServicios_SelectedIndexChanged" TabIndex="5" AutoPostBack="true" CssClass="dropdown" />
</div>
<div class="etiqueta">
<label for="lblOrdenadoServicios">Ordenado Por:</label>
</div>
<div class="etiqueta"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenadoServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoServicios" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbNuevoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNuevoServicio" runat="server" Text="Nuevo Servicio" OnClick="lkbNuevoServicio_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto;">
<asp:UpdatePanel ID="uplkbExportarServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar Excel" OnClick="lkbExportarServicios_Click" TabIndex="6"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_altura_variable">
<asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServicios" OnPageIndexChanging="gvServicios_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvServicios_RowDataBound" OnSorting="gvServicios_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="7"
ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True" 
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio" >
<ItemTemplate>
<asp:LinkButton ID="lkbNoServicio" runat="server" CommandName="Documentacion" OnClick="lkbAccionServicio_Click" Text='<%#Eval("NoServicio") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="idServicio" HeaderText="idServicio" SortExpression="idServicio"  Visible="false"/>
<asp:BoundField DataField="Documentacion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Documentación" SortExpression="Documentacion">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<%--<asp:BoundField DataField="OperacionAlcance" HeaderText="Operacion/Alcance/Servicio" SortExpression="OperacionAlcance" ItemStyle-CssClass="label_correcto" />--%>
<asp:TemplateField HeaderText="Operacion/Alcance/Servicio" SortExpression="OperacionAlcance">
<ItemTemplate>
<asp:LinkButton ID="lkbOperacionAlcance" runat="server" CommandName="OperacionAlcance" OnClick="lkbAccionServicio_Click" Text='<%#Eval("OperacionAlcance") %>' ToolTip="Clasificacion Servicio"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
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
<asp:TemplateField SortExpression="SemaforoCarga">
<ItemTemplate>
<asp:Image ID="imgSemaforoCarga" runat="server" ImageUrl="~/Image/semaforo_verde.png" Height="20" Width="20" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Carga" SortExpression="CitaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField SortExpression="SemaforoDescarga">
<ItemTemplate>
<asp:Image ID="imgSemaforoDescarga" runat="server" ImageUrl="~/Image/semaforo_verde.png" Height="20" Width="20" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Carga" SortExpression="FechaLlegadaCarga">
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaCarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaCarga" />
<asp:BoundField DataField="FechaSalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Salida Carga" SortExpression="FechaSalidaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Descarga" SortExpression="FechaLlegadaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaDescarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaDescarga" />
<asp:TemplateField HeaderText="Despachar" SortExpression="Despachar">
<ItemTemplate>
<asp:LinkButton runat="server" ID="lkbDespachar" CommandName="Despachar" OnClick="lkbAccionServicio_Click" Text="Despachar"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Operador" SortExpression="Operador">
<ItemTemplate>
<asp:LinkButton ID="lkbOperador" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Operador") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Tractor" SortExpression="Tractor">
<ItemTemplate>
<asp:LinkButton ID="lkbTractor" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Tractor") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Remolque" SortExpression="Remolque">
<ItemTemplate>
<asp:LinkButton ID="lkbRemolque" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Remolque") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Transportista" SortExpression="Transportista">
<ItemTemplate>
<asp:LinkButton ID="lkbTransportista" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Transportista") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Referencias" SortExpression="Referencias">
<ItemTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" CommandName="Referencia" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Referencias") %>'></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbCancelarServicio" runat="server" Text="Cancelar" OnClick="lkbAccionServicio_Click" CommandName="Cancelar"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="Publicacion">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusPublicacion" runat="server"  OnClick="lkbAccionServicio_Click" Visible="false"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="Respuestas">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusRespuestas" runat="server"  OnClick="lkbAccionServicio_Click" Visible="false"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="AceptarRespuestas">
<ItemTemplate>
<asp:LinkButton ID="lkbAceptarRespuesta" runat="server" OnClick="lkbAccionServicio_Click" Visible="false"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServicios" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarReferencias" />
<asp:AsyncPostBackTrigger ControlID="wucParadaEvento" />
<asp:AsyncPostBackTrigger ControlID="wucPreAsignacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="wucServicioDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarResultadoRespuestaPS" />
<%--<asp:AsyncPostBackTrigger ControlID="wucPublicacionServicio" />--%>
<asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
    <asp:AsyncPostBackTrigger ControlID="btnAgregarTercero" />
<asp:AsyncPostBackTrigger ControlID="wucClasificacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
<asp:View ID="vwUnidades" runat="server" >
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Búsqueda de Unidades</h2>
</div>
<div class="columna2x">
<div class="renglon"></div>
<div class="renglon2x">
<div class="etiqueta">
<label class="label_negrita">Tipo de Unidades</label>
</div>
</div>
<div class="renglon2x">
<div class="control">
<asp:CheckBox ID="chkUnidadesPropias" runat="server" Checked="true" Text="Unidades Propias" />
</div>
<div class="control">
<asp:CheckBox ID="chkUnidadesNoPropias" runat="server"  Text="Unidades No Propias"/>
</div>
</div>   
<div class="renglon">
<div class="control">
<asp:UpdatePanel ID="uprdbUnidadMotriz" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbUnidadMotriz" runat="server" GroupName="TipoUnidad" Text="Unidades Motrices" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbUnidad_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbUnidadArrastre" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uprdbUnidadArrastre" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rdbUnidadArrastre" runat="server" GroupName="TipoUnidad" Text="Unidades de Arrastre" AutoPostBack="true" OnCheckedChanged="rdbUnidad_CheckedChanged" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="rdbUnidadMotriz" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtNoUnidad">
No. Unidad
</label>
</div>
<div class="control">
<asp:TextBox ID="txtNoUnidad" runat="server" CssClass="textbox" MaxLength="30" TabIndex="1"></asp:TextBox>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlEstatus">
Estatus
</label>
</div>
<div class="control">
<asp:ListBox runat="server" ID="lbxEstatus" SelectionMode="multiple" TabIndex="2" />
<%--<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>--%>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUbicacion">Ubicación</label>
</div>
<div class="control">
<asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtClienteUnidad">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtClienteUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtClienteUnidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlFlota">
Flota
</label>
</div>
<div class="control">
<asp:ListBox runat="server" ID="lbxFlota" SelectionMode="multiple" TabIndex="5" />
<%--<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>--%>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnBuscarUnidades" runat="server" CssClass="boton" Text="Buscar" TabIndex="6" OnClick="btnBuscarUnidades_Click" />
</div>
</div>
<div class="renglon2x"></div>
</div>
<div class="header_seccion">
<img src="../Image/EnTransito.png" />
<h2>Listado de Unidades</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoUnidades">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoUnidades"></label>
<asp:DropDownList ID="ddlTamanoUnidades" runat="server" OnSelectedIndexChanged="ddlTamanoUnidades_SelectedIndexChanged" TabIndex="7" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarUnidades">Ordenado Por:</label>
</div>
<div class="etiqueta"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarUnidades" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbExportarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarUnidades" runat="server" Text="Exportar Excel" OnClick="lkbExportarUnidades_Click" TabIndex="8"></asp:LinkButton>
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
<asp:GridView ID="gvUnidades" OnPageIndexChanging="gvUnidades_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvUnidades_RowDataBound" OnSorting="gvUnidades_Sorting" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="7"
ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
CssClass="gridview" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:BoundField DataField="*IdUnidad" SortExpression="*IdUnidad" Visible="false" />
<asp:TemplateField HeaderText="No. Unidad" SortExpression="NoUnidad" >
<ItemTemplate>
<asp:LinkButton ID="lkbNoUnidad" runat="server" Text='<%#Eval("NoUnidad") %>' OnClick="lkbAccionUnidad_Click" CommandName="Historial" ToolTip="Ver Historial de la Unidad"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="TipoUnidad" HeaderText="Tipo Unidad" SortExpression="TipoUnidad" />
<asp:BoundField DataField="EstatusUnidad" HeaderText="Estatus" SortExpression="EstatusUnidad" />
<asp:BoundField HeaderText="Tiempo" DataField="Tiempo" SortExpression="*Tiempo" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField HeaderText="Ubicación Actual" DataField="UbicacionActual" SortExpression="UbicacionActual" />
<asp:TemplateField HeaderText="Último Monitoreo" SortExpression="UltimoMonitoreo">
<ItemTemplate>
<asp:LinkButton ID="lkbUltimoMonitoreo" runat="server" Text='<%#Eval("UltimoMonitoreo", "{0:dd/MM/yyyy HH:mm}") %>' CommandName="UltimoMonitoreo"
ToolTip="Ver Bitácora de Monitoreo" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="NoServicioMov" HeaderText="No. Serv. / Movto." SortExpression="NoServicioMov">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Kms" HeaderText="Kms" SortExpression="Kms">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="ProximaCita" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Próxima Cita" SortExpression="ProximaCita">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField HeaderText="Estatus Serv. / Mvto." DataField="EstatusServMov" SortExpression="EstatusServMov" />
<asp:BoundField DataField="UnidadAsignada" HeaderText="Unidad Asignada" SortExpression="UnidadAsignada">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusUAsignada" HeaderText="Estatus U. Asignada" SortExpression="EstatusUAsignada" />
<asp:BoundField HeaderText="Tiempo Estatus." DataField="TiempoUAsignada" SortExpression="*TiempoUAsignada" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Porte" HeaderText="Carta Porte" SortExpression="Porte" >
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="" SortExpression="Publicacion">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusPublicacion" runat="server"  OnClick="lkbAccionUnidad_Click"
></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="Respuestas">
<ItemTemplate>
<asp:LinkButton ID="lkbEstatusRespuestas" runat="server"  OnClick="lkbAccionUnidad_Click"
></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="AceptarRespuestas">
<ItemTemplate>
<asp:LinkButton ID="lkbAceptarRespuesta" runat="server" OnClick="lkbAccionUnidad_Click"
></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="" SortExpression="Copiar">
<ItemTemplate>
<asp:LinkButton ID="lkbCopia" runat="server" OnClick="lkbAccionUnidad_Click"
></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="rdbUnidadArrastre" />
<asp:AsyncPostBackTrigger ControlID="rdbUnidadMotriz" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoUnidades" />
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreo" />
<asp:AsyncPostBackTrigger ControlID="wucPreAsignacionRecurso" />
<%--<asp:AsyncPostBackTrigger ControlID="wucPublicacionUnidad" />--%>
<asp:AsyncPostBackTrigger ControlID="lnkCerrrarResultadoRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarElemento" />
</Triggers>
</asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnPestanaViajes" />
<asp:AsyncPostBackTrigger ControlID="btnPestanaUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>

<!-- VENTANA MODAL DE HISTORIAL DE BITÁCORA DE MONITOREO -->
<div id="historialBitacoraModal" class="modal">
<div id="historialBitacora" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialBitacora" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialBitacora" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="HistorialBitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna3x">
<asp:UpdatePanel ID="upwucBitacoraMonitoreoHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucBitacoraMonitoreoHistorial runat="server" ID="wucBitacoraMonitoreoHistorial" OnbtnNuevoBitacora="wucBitacoraMonitoreoHistorial_btnNuevoBitacora" OnlkbConsultar="wucBitacoraMonitoreoHistorial_lkbConsultar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreo" />
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>
<!-- VENTANA MODAL DE EDICIÓN Y CAPTURA DE BITÁCORA DE MONITOREO -->
<div id="bitacoraMonitoreoModal" class="modal">
<div id="bitacoraMonitoreo" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarBitacoraMonitoreo" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarBitacoraMonitoreo" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Bitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="columna">
<asp:UpdatePanel ID="upwucBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucBitacoraMonitoreo runat="server" ID="wucBitacoraMonitoreo" OnClickRegistrar="wucBitacoraMonitoreo_ClickRegistrar" OnClickEliminar="wucBitacoraMonitoreo_ClickEliminar"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreoHistorial" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
</div>
<!-- VENTANA MODAL DE SELECCIÓN DE NUEVO SERVICIO O REPOSICIONAMIENTO -->
<div id="seleccionServicioMovimientoModal" class="modal">
<div id="seleccionServicioMovimiento" class="contenedor_ventana_confirmacion">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarSeleccionServicioMovimiento" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarSeleccionServicioMovimiento" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="SeleccionServMov" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Entrada.png" alt="SeleccionTarea" />
<h3>Nuevo Movimiento</h3>
</div>
<div class="columna400px">
<div class="renglon">
<label class="mensaje_modal">Seleccione el Método de Creación del Nuevo Servicio</label>
</div>
<div class="renglon"></div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:Button ID="btnNuevoServicio" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="NuevoServicio" Text="Crear Servicio" />
</div>
<div class="controlBoton">    
<asp:Button ID="btnNuevaCopiaServicio" runat="server" CssClass="boton" OnClick="btnNuevoServicioMovimiento_Click" CommandName="CopiaServicio" Text="Copiar Servicio" />
</div>
<div class="renglon"></div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL DE COPIA DE SERVICIOS MAESTROS -->
<div id="copiaServicioMaestroModal" class="modal">
<div id="copiaServicioMaestro" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCopiaServicioMaestro" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCopiaServicioMaestro" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="CopiaServicio">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucServicioCopia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucServicioCopia runat="server" id="wucServicioCopia" OnClickGuardarServicioCopia="wucServicioCopia_ClickGuardarServicioCopia" OnClickCancelarServicioCopia="wucServicioCopia_ClickCancelarServicioCopia" Contenedor="#copiaServicioMaestro" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevaCopiaServicio" />
</Triggers>
</asp:UpdatePanel>    
</div>
</div>
<!-- VENTANA MODAL DE ASIGNACIÓN DE RECURSOS -->
<div id="asignacionRecursosModal" class="modal">
<div id="asignacionRecursos" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAsignacionRecursos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAsignacionRecursos" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="AsignacionRecursos">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucAsignacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucPreAsignacionRecurso runat="server" ID="wucPreAsignacionRecurso" OnClickAsignarRecurso="wucAsignacionRecurso_ClickAgregarRecurso" OnClickLiberarRecurso="wucAsignacionRecurso_ClickLiberarRecurso" Contenedor="#asignacionRecursos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE EVENTOS DE PARADA ACTUAL -->
<div id="eventosParadaModal" class="modal">
<div id="eventosParada" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEventosParada" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEventosParada" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="EventosParada">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucParadaEvento" runat="server">
<ContentTemplate>
<uc1:wucParadaEvento runat="server" id="wucParadaEvento" OnBtnActualizar_Click="wucParadaEvento_OnBtnActualizarClick" 
OnBtnCancelar_Click="wucParadaEvento_OnBtnCancelarClick" OnBtnNuevo_Click="wucParadaEvento_OnBtnNuevoClick" 
OnLkbEliminar_Click="wucParadaEvento_OnlkbEliminarClick" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE REFERENCIAS DE SERVICIO -->
<div id="referenciasServicioModal" class="modal">
<div id="referenciasServicio" class="contenedor_ventana_confirmacion_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarReferencias" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ReferenciasServicio" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Clasificacion.png" />
<h2>Referencias Servicio</h2>
</div> 
<div class="columna2x">
<asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true"
OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- VENTANA MODAL DOCUMENTACIÓN DE SERVICIO -->
<div id="documentacionServicioModal" class="modal">
<div id="documentacionServicio" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarDocumentacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarDocumentacion" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="Documentacion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucServicioDocumentacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucServicioDocumentacion runat="server" ID="wucServicioDocumentacion" OnImbAgregarParada_Click="wucServicioDocumentacion_ImbAgregarParada_Click" 
OnLkbEliminarParada_Click="wucServicioDocumentacion_LkbEliminarParada_Click" OnImbAgregarProducto_Click="wucServicioDocumentacion_ImbAgregarProducto_Click" 
OnLkbEliminarProducto_Click="wucServicioDocumentacion_LkbEliminarProducto_Click" OnBtnAceptarEncabezado_Click="wucServicioDocumentacion_BtnAceptarEncabezado_Click" 
OnLkbCitasEventos_Click="wucServicioDocumentacion_LkbCitasEventos_Click" Contenedor="#documentacionServicio"/>    
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnNuevoServicio" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="wucServicioCopia" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL CANCELACIÓN DE SERVICIO -->
<div id="confirmacionCancelacionModal" class="modal">
<div id="confirmacionCancelacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" alt="SeleccionTarea" />
<h3>Cancelación de Servicio</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<label class="mensaje_modal">Esta acción no es reversible. ¿Desea Continuar?</label>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtMotivoNoFacturable">Motivo:</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtMotivoCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMotivoCancelacion" runat="server" TextMode="MultiLine"  CssClass="textbox2x validate[required]" MaxLength="500" TabIndex="1"></asp:TextBox></div></div>
</ContentTemplate>
<Triggers>    
   <asp:AsyncPostBackTrigger ControlID="gvServicios" /> 
</Triggers>
</asp:UpdatePanel>
</div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div  class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarCancelacion" runat="server" CssClass="boton" OnClick="btnCancelacion_Click" CommandName="Cancelar" Text="Cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton"> 
<asp:UpdatePanel ID="upbtnAceptarCancelacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelacion" runat="server" CssClass="boton" OnClick="btnCancelacion_Click" CommandName="Aceptar" Text="Aceptar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelarCancelacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE UNIDADES ASIGNADAS A SERVICIO -->
<div id="unidadesAsignadasModal" class="modal">
<div id="unidadesAsignadas" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarUnidadesAsignadas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarUnidadesAsignadas" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="UnidadesAsignadas">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Tractor-Remolque.png"/>
<h2>Recursos Asignados</h2>
</div>  
<div class="grid_seccion_completa_150px_altura">            
<asp:UpdatePanel ID="upgvRecursosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosAsignados" runat="server" AutoGenerateColumns="False" CssClass="gridview" Width="100%" PageSize="10">
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
<asp:LinkButton ID="lkbQuitarRecursoAsignado" runat="server" Text="Quitar" OnClick="lkbQuitarRecursoAsignado_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" /> 
<asp:AsyncPostBackTrigger ControlID="btnAceptarEliminacionRecurso" />                     
</Triggers>
</asp:UpdatePanel>
</div>      
</div>  
</div>
<!-- VENTANA MODAL QUE MUESTRA EL MENSAJE DE CONFIRMACION EN LOS CASOS EN LOS QUE SE DESASIGNA UNA UNIDAD LIGADA A UN OPERADOR -->
<div id="confirmacionQuitarRecursosModal" class="modal">
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
<asp:Button ID="btnCancelarEliminacionRecurso" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarEliminacionRecurso_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarEliminacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarEliminacionRecurso" runat="server" CssClass="boton" OnClick="btnAceptarEliminacionRecurso_Click" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- VENTANA MODAL QUE MUESTRA EL RESUMEN DE CARGAS Y DESCARGAS POR CLIENTE -->
<div id="resumenPorClienteModal" class="modal">
<div id="resumenPorCliente" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarResumenCliente" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarResumenCliente" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click" CommandName="ResumenCliente">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/carga.png"/>
<h2>Resumen de Cargas y Descargas por Cliente</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoResumenCliente">
Mostrar:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoResumenCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoResumenCliente" runat="server" OnSelectedIndexChanged="ddlTamanoResumenCliente_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenadoPorResumenCliente">Ordenado Por:</label>
</div>
<div class="etiqueta"  style="width: auto">
<asp:UpdatePanel ID="uplblOrdenadoPorResumenCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoPorResumenCliente" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvResumenCliente" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlr">
<asp:UpdatePanel ID="uplkbExportarResumenCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarResumenCliente" runat="server" Text="Exportar Excel" OnClick="lkbExportarResumenCliente_Click" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarResumenCliente" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="grid_seccion_completa_200px_altura">            
<asp:UpdatePanel ID="upgvResumenCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvResumenCliente" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true" CssClass="gridview" Width="100%" ShowFooter="true" PageSize="25"
    OnSorting="gvResumenCliente_Sorting" OnPageIndexChanging="gvResumenCliente_PageIndexChanging">
<Columns>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente"/>                               
<asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" />                                
<asp:BoundField DataField="Evento" HeaderText="Evento" SortExpression="Evento" />
<asp:BoundField DataField="Lunes" HeaderText="Lunes" SortExpression="Lunes" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Martes" HeaderText="Martes" SortExpression="Martes">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Miercoles" HeaderText="Miércoles" SortExpression="Miercoles" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Jueves" HeaderText="Jueves" SortExpression="Jueves">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Viernes" HeaderText="Viernes" SortExpression="Viernes">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Sabado" HeaderText="Sábado" SortExpression="Sabado">
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Domingo" HeaderText="Domingo" SortExpression="Domingo" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" >
<FooterStyle HorizontalAlign="Right" />
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
<asp:AsyncPostBackTrigger ControlID="rdbResumenTotal" />                    
<asp:AsyncPostBackTrigger ControlID="rdbResumenPendientes" /> 
<asp:AsyncPostBackTrigger ControlID="ddlTamanoResumenCliente" />
<asp:AsyncPostBackTrigger ControlID="btnSemanaMenos" />
<asp:AsyncPostBackTrigger ControlID="btnSemanaMas" />
</Triggers>
</asp:UpdatePanel>
</div>      
</div>  
</div>
<!-- VENTANA MODAL PUBLICACION DE UNIDAD-->
<div id="modalPublicar" class="modal">
<div id="confirmacionPublicar" class="contenedor_modal_seccion_completa_arriba"  style="width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarPublicacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarPublicacion" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="CerrarPublicacion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucPublicacionServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<%--<uc1:wuPublicacion ID="wucPublicacionServicio" runat="server" OnClickPublicar="wucPublicacionServicio_ClickRegistrar"  />--%>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL PARA PUBLICACION DE UNIDAD-->
<div id="modalPublicarUnidad" class="modal">
<div id="confirmacionPublicarUnidad" class="contenedor_modal_seccion_completa_arriba"  style="width:1200px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarPublicacionUnidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarPublicacionUnidad" runat="server"   OnClick="lkbCerrarVentanaModal_Click" CommandName="CerrarPublicacionUnidad" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucPublicacionUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<%--<uc1:wuPublicacionUnidad ID="wucPublicacionUnidad" runat="server" OnClickPublicar="wucPublicacionUnidad_ClickRegistrar"  />--%>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
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
<asp:LinkButton ID="lkbVerDetalleResultado" runat="server" Text="Ver Detalles"  OnClick="lkbAccionResultado_Click" CommandName="VerDetalles" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbVerRechazarRespuesta" runat="server" Text="Rechazar"  OnClick="lkbAccionResultado_Click" CommandName="RechazarRespuestas" ></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarSeleccionRespuestas" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarRechazarRespuesta" />
<asp:AsyncPostBackTrigger ControlID="btnRechazarRespuesta" />
</Triggers>
</asp:UpdatePanel>
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
                <asp:BoundField DataField="Peso" HeaderText="Peso" SortExpression="Peso"  />
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
        <asp:AsyncPostBackTrigger  ControlID="btnAceptarRespuesta" />
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
<!-- Ventana de Elemento Faltante (Ubicación, Producto, Cliente, Concepto -->
<div id="contenidoElemento" class="modal">
<div id="confirmacionElemento" class="contenedor_ventana_confirmacion">   
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
<asp:AsyncPostBackTrigger ControlID ="gvUnidades" />
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
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
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
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
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
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
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
<asp:AsyncPostBackTrigger ControlID="gvUnidades" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>                
</div>
</div>
<!-- Ventana de Tercero-->
<div id="contenidoTercero" class="modal">
<div id="confirmacionTercero" class="contenedor_ventana_confirmacion">   
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarTercero" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarTercero" runat="server" CommandName="CerrarTercero"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>         
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>    
<asp:UpdatePanel ID="uplblDescripcionTercero" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblDescripcionTercero" runat="server" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
</Triggers>
</asp:UpdatePanel></h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Seleccione el Tercero coincidente para su Alta?</label>
</div>
<div class="renglon2x"><div class="etiqueta">
<asp:UpdatePanel ID="uplblIdTercero" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblIdTercero" runat="server" Visible="false" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarTercero" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">   
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblTercero" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblTercero" runat="server"  CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarTercero" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtTercero" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtTercero" Contenedor="#ventanaTercero" MaxLength="150" CssClass="textbox2x validate[required,,custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarTercero" runat="server"  CssClass="boton"  OnClick="btnAgregarTercero_Click" Text="Agregar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x"><div class="etiqueta">
<asp:UpdatePanel ID="uplblValorTercero" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Label ID="lblValorTercero" runat="server" Text="No" Visible="false" ></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>                
</div>
</div>
<div id="contenidoConfirmacionRechazarRespuesta" class="modal">
<div id="confirmacionRechazarRespuesta" class="contenedor_ventana_confirmacion">            
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbRechazarRespuesta" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarRechazarRespuesta" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="RechazarRespuesta" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Rechazar Respuesta</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">
¿Realmente desea rechazar la Respuesta</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel UpdateMode="Conditional" ID="upbtnRechazarRespuesta" runat="server">
<ContentTemplate>
<asp:Button  ID ="btnRechazarRespuesta" runat="server"  CssClass="boton" OnClick="btnRechazarRespuesta_Click" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel> 
</div>
</div>
</div>                
</div>
</div>
<!-- Resultado Respuesta Publicación Servicio-->
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
<asp:AsyncPostBackTrigger ControlID="gvResultadoRespuestaPS" EventName="Sorting" />
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
<asp:BoundField DataField="Tarifa" HeaderText="Tarifa" SortExpression="Tarifa"  DataFormatString="{0:C2}"/>
<asp:BoundField DataField="TarifaAceptada" HeaderText="Tarifa Aceptada" SortExpression="TarifaAceptada"    ItemStyle-ForeColor="Red"  DataFormatString="{0:C2}"/>
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
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
<asp:AsyncPostBackTrigger ControlID="ddTamanoResultadoRespuestaPS" />
    <asp:AsyncPostBackTrigger ControlID="btnAceptarRespuesta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<!-- Resultado Respuesta Publicación Servicio-->
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
                <asp:BoundField DataField="TarifaAceptada" HeaderText="Tarifa Aceptada" SortExpression="TarifaAceptada"  ItemStyle-ForeColor="Red"  DataFormatString="{0:C2}"/>
                    <asp:BoundField DataField="Observación" HeaderText="Observación" SortExpression="Observación" />
                <asp:BoundField DataField="Contacto" HeaderText="Contacto" SortExpression="Contacto" />
                <asp:TemplateField HeaderText="" SortExpression="Indicador" >
<HeaderStyle Width="65px" />
<ItemStyle Width="65px" />
<ItemTemplate>
<asp:LinkButton ID="lkbAccionRespuestaPS" runat="server"  CommandName="Aceptar" Text="Aceptar" OnClick="lkbAccionRespuestaPS_Click" ></asp:LinkButton>
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
<!--VENTANA MODAL DE CLASIFICACION DE LOS SERVICIOS-->
<div id="contenedorClasificacion" class="modal">
        <div id="ventanaClasificacion" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Clasificacion" Text="Cerrar">
                                      <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Clasificacion.png" style="width: 32px;" /> 
                 <h2 id="H1" runat="server">Clasificación</h2>
            </div>
            <asp:UpdatePanel ID="upwucClasificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucClasificacion runat="server" ID="wucClasificacion" OnClickGuardar="wucClasificacion_ClickGuardar" OnClickCancelar="wucClasificacion_ClickCancelar" TabIndex="24" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
