<%@ Page Title="Servicios Pendientes" Language="C#" AutoEventWireup="true" CodeBehind="ServiciosPendientes.aspx.cs" Inherits="SAT.Despacho.ServiciosPendientes" %>
<!DOCTYPE html>
<%@ Register Src="~/UserControls/wucPreAsignacionRecurso.ascx" TagPrefix="uc1" TagName="wucPreAsignacionRecurso" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="uc1" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucServicioDocumentacion.ascx" TagPrefix="uc1" TagName="wucServicioDocumentacion" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucVencimiento.ascx" TagPrefix="uc1" TagName="wucVencimiento" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title></title>
<!-- Estilos de los Controles -->
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />    
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
<!-- Estilos de Scripts -->
<link href="../CSS/jquery-ui.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.structure.min.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.css" rel="stylesheet" />
<link href="../CSS/jquery-ui.theme.min.css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Habilitación para uso de jquery en formas ligadas a esta master page -->
<script src='<%=ResolveUrl("~/Scripts/jquery-1.7.1.min.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jquery-ui.min.js") %>' type="text/javascript"></script>
<!-- Notificaciones emergentes -->
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.js") %>'></script>
<script type="text/javascript" src='<%=ResolveUrl("~/Scripts/jquery.noty.packaged.min.js") %>'></script>
<!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
<script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/jquery.datetimepicker.js" type="text/javascript" charset="utf-8"></script>
<script src="../Scripts/gridviewScroll.min.js" type="text/javascript" charset="utf-8"></script>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
<script>
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(Loading);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Loaded);

function Loading() {
$.blockUI({ message: '<h2><img src="../Image/loading2.gif" /> Espere por favor...</h2>', fadeIn: 200 });
}
function Loaded() {
$.unblockUI({ fadeOut: 200 });
}

</script>

<!-- Script de Configuración de Controles -->
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryServicioPendientes();
        }
    }

    //Creando función para configuración de jquery en formulario
    function ConfiguraJQueryServicioPendientes() {
        $(document).ready(function () {
            
            //Añadiendo Función de Autocompletado al Control
            $("#<%=txtCliente.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
            });
            $("#<%=txtNoUnidadA.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
            $("#<%=txtNoOperador.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=38&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
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
                var isValid1;
                var isValid2;
                var isValid3 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');

                //Validando el Control
                if ($("#<%=chkCitaCarga.ClientID%>").is(':checked') == true || $("#<%=chkCitaDescarga.ClientID%>").is(':checked') == true) {
                    //Validando Controles
                    isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                    isValid2 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                }
                else {
                    //Asignando Valor Positivo
                    isValid1 = true;
                    isValid2 = true;
                }

                //Devolviendo Resultados Obtenidos
                return isValid1 && isValid2 && isValid3;
            });

            <%--//Añadiendo Encabezado Fijo
            $("#<%=gvServicios.ClientID%>").gridviewScroll({
                width: document.getElementById("contenedorServicios").offsetWidth - 15,
                height: 1200,
                freezesize: 4
            });--%>

        });
    }

    //Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryServicioPendientes();

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
</script>
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Servicios Activos</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCliente">Cliente</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control2x">
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x" TabIndex="1"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="upchkCitaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkCitaCarga" runat="server" Text="Cita Carga" Checked="true" TabIndex="2" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="upchkCitaDescarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkCitaDescarga" runat="server" Text="Cita Descarga" TabIndex="3" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIni">Fecha de Inicio</label>
</div>
<div class="control">
<asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="4"></asp:TextBox>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFin">Fecha de Fin</label>
</div>
<div class="control">
<asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="5"></asp:TextBox>
</div>
</div>

<div class="renglon2x">
    <div class="etiqueta">
        <label for="txtNoUnidadA">
            No. Unidad
        </label>
    </div>
    <div class="control2x">
        <asp:TextBox ID="txtNoUnidadA" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="13"></asp:TextBox>
    </div>
</div>

<div class="renglon2x">
    <div class="etiqueta">
        <label for="txtNoOperador">
            Operador
        </label>
    </div>
    <div class="control2x">
        <asp:TextBox ID="txtNoOperador" runat="server" CssClass="textbox validate[custom[IdCatalogo]]" TabIndex="14"></asp:TextBox>
    </div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:CheckBox ID="chkDocumentado" runat="server" Text="Ver Documentados" TabIndex="6" />
</div>
<div class="etiqueta_155px">
<asp:CheckBox ID="chkIniciado" runat="server" Text="Ver Iniciados" TabIndex="7" />
</div>
<div class="controlBoton">
<asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscar_Click" TabIndex="8" />
</div>
</div>
</div>
<div class="header_seccion">
<img src="../Image/Buscar.png" />
<h2>Resultados</h2>
</div>
<div class="renglon3x">
<div class="etiqueta" style="width: auto">
<label for="ddlTamanoServicios">
Mostrar:
</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoServicios" runat="server" OnSelectedIndexChanged="ddlTamanoServicios_SelectedIndexChanged" TabIndex="9" AutoPostBack="true" CssClass="dropdown" />
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
<div class="etiqueta" style="width:auto;">
<asp:UpdatePanel ID="uplkbExportarServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar Excel" OnClick="lkbExportarServicios_Click" TabIndex="10"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_encabezado_fijo" id="contenedorServicios">
<asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServicios" OnPageIndexChanging="gvServicios_PageIndexChanging" ShowFooter="True" OnSorting="gvServicios_Sorting" 
runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="11"
ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True"
CssClass="gridview" Width="100%" OnRowDataBound="gvServicios_RowDataBound">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<Columns>
<asp:TemplateField HeaderText="No. Servicio" SortExpression="NoServicio" >
<ItemTemplate>
<asp:LinkButton ID="lkbNoServicio" runat="server" CommandName="Documentacion" OnClick="lkbAccionServicio_Click" Text='<%#Eval("NoServicio") %>' ToolTip="Muestra la Información de Documentación del Servicio."></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Documentacion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Documentación" SortExpression="Documentacion">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:TemplateField HeaderText="Viaje" SortExpression="Viaje">
<ItemTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" CommandName="Referencia" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Viaje") %>' ToolTip="Muestra las Referencias del Servicio."></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Confirmacion" HeaderText="Confirmación" SortExpression="Confirmacion">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Carta Porte" SortExpression="CartaPorte">
<ItemTemplate>
<asp:LinkButton ID="lkbCartaPorte" runat="server" CommandName="CartaPorte" OnClick="lkbAccionServicio_Click" Text='<%#Eval("CartaPorte") %>' ToolTip="Descarga la Carta Porte en su Formato para Impresión."></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
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
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Carga" SortExpression="FechaLlegadaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaCarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaCarga" />
<asp:BoundField DataField="FechaSalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Salida Carga" SortExpression="FechaSalidaCarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="FechaLlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Llegada Descarga" SortExpression="FechaLlegadaDescarga">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="EstatusLlegadaDescarga" HeaderText="Estatus Llegada" SortExpression="EstatusLlegadaDescarga" />
<asp:TemplateField HeaderText="Operador" SortExpression="Operador">
<ItemTemplate>
<asp:LinkButton ID="lkbOperador" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Operador") %>' ToolTip="Permite Asignar o Quitar un Recurso al Servicio."></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Tractor" SortExpression="Tractor">
<ItemTemplate>
<asp:LinkButton ID="lkbTractor" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Tractor") %>' ToolTip="Permite Asignar o Quitar un Recurso al Servicio."></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:BoundField DataField="Placas" HeaderText="Placas" SortExpression="Placas">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Remolque" SortExpression="Remolque">
<ItemTemplate>
<asp:LinkButton ID="lkbRemolque" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Remolque") %>' ToolTip="Permite Asignar o Quitar un Recurso al Servicio."></asp:LinkButton>
</ItemTemplate>
<ItemStyle HorizontalAlign="Right" />
</asp:TemplateField>
<asp:TemplateField HeaderText="Transportista" SortExpression="Transportista">
<ItemTemplate>
<asp:LinkButton ID="lkbTransportista" runat="server" CommandName="Asignaciones" OnClick="lkbAccionServicio_Click" Text='<%#Eval("Transportista") %>' ToolTip="Permite Asignar o Quitar un Recurso al Servicio."></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad">
<ItemStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Vencimiento" SortExpression="Vencimiento">
<ItemTemplate>
<asp:LinkButton ID="lkbVencimiento" runat="server" CommandName="Vencimiento" OnClick="lkbAccionServicio_Click" Text='<%# Eval("Vencimiento") %>' ToolTip="Agregue o Termine Vencimientos del Servicio."></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoServicios" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="wucPreAsignacionRecurso" />
<asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
<asp:AsyncPostBackTrigger ControlID="wucServicioDocumentacion" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarVencimientoSeleccionado" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarHistorialVencimientos" />
</Triggers>
</asp:UpdatePanel>
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
<uc1:wucPreAsignacionRecurso runat="server" ID="wucPreAsignacionRecurso" OnClickAsignarRecurso="wucAsignacionRecurso_ClickAgregarRecurso" 
OnClickLiberarRecurso="wucAsignacionRecurso_ClickLiberarRecurso" Contenedor="#asignacionRecursos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
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
OnLkbEliminarParada_Click="wucServicioDocumentacion_LkbEliminarParada_Click" Contenedor="#documentacionServicio"
OnImbAgregarProducto_Click="wucServicioDocumentacion_ImbAgregarProducto_Click" OnLkbEliminarProducto_Click="wucServicioDocumentacion_LkbEliminarProducto_Click" 
OnBtnAceptarEncabezado_Click="wucServicioDocumentacion_BtnAceptarEncabezado_Click" />    
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE VISUALIZACIÓN DE HISTORIAL DE VENCIMIENTOS -->
<div id="historialVencimientosModal" class="modal">
<div id="historialVencimientos" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialVencimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialVencimientos" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="HistorialVencimientos" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimientosHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucVencimientosHistorial runat="server" id="wucVencimientosHistorial" OnlkbConsultar="wucVencimientosHistorial_lkbConsultar" 
    OnlkbTerminar="wucVencimientosHistorial_lkbTerminar" Contenedor="#historialVencimientos"
    OnbtnNuevoVencimiento="wucVencimientosHistorial_btnNuevoVencimiento" />            
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucVencimiento" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- VENTANA MODAL DE ACTUALIZACIÓN DE VENCIMIENTOS -->
<div id="actualizacionVencimientoModal" class="modal">
<div id="actualizacionVencimiento" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVencimientoSeleccionado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVencimientoSeleccionado" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="ActualizacionVencimiento" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upwucVencimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucVencimiento runat="server" id="wucVencimiento" OnClickGuardarVencimiento="wucVencimiento_ClickGuardarVencimiento" 
    OnClickTerminarVencimiento="wucVencimiento_ClickTerminarVencimiento" Contenedor="#actualizacionVencimiento" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="wucVencimientosHistorial" />
<asp:AsyncPostBackTrigger ControlID="gvServicios" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</form>
</body>
</html>
