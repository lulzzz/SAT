<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucPreAsignacionRecurso.ascx.cs" Inherits="SAT.UserControls.wucPreAsignacionRecurso" %>
<!-- Estilos -->
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Operacion.css" rel="stylesheet" />
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryPreAsignacionRecurso();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryPreAsignacionRecurso() {
    $(document).ready(function () {

        //Cargando Catalogos Autocompleta
        $("#<%=txtPropietarioUnidad.ClientID%>").autocomplete({
            source: '../WebHandlers/AutoCompleta.ashx?id=16&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
            appendTo: "<%=this.Contenedor%>"
        });
        $("#<%=txtNombreOperador.ClientID%>").autocomplete({
            source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
            appendTo: "<%=this.Contenedor%>"
        });
        $("#<%=txtUbicacionUnidad.ClientID%>").autocomplete({
            source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>',
            appendTo: "<%=this.Contenedor%>"
        });

        //Función de validación Búsqeda Unidad
        var validacionBuscarAsignacionUnidad = function (evt) {
            //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
            var isValid1 = !$("#<%=txtPropietarioUnidad.ClientID%>").validationEngine('validate');
            var isValid2 = !$("#<%=txtUbicacionUnidad.ClientID%>").validationEngine('validate');
            return isValid1 && isValid2;
        };
        //Función de validación Búsqueda Operador
        var validacionBuscarAsignacionOperador = function (evt) {
            //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
            var isValid1 = !$("#<%=txtNombreOperador.ClientID%>").validationEngine('validate');
            return isValid1;
        };

        //Botón Buscar Unidades
        $("#<%=btnBuscarUnidades.ClientID %>").click(validacionBuscarAsignacionUnidad);
        //Botón Buscar Operadores
        $("#<%=btnBuscarOperadores.ClientID %>").click(validacionBuscarAsignacionOperador);

    });
}

//Invocando Función
ConfiguraJQueryPreAsignacionRecurso();
</script>
<div class="columna">
<div class="contenedor_botones_pestaña_filtros_asignacion">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnUnidad" Text="Unidad" Width="85px" OnClick="btnVista_OnClick" CommandName="Unidad" runat="server" CssClass="boton_pestana_activo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnOperador" Text="Operador" Width="85px" runat="server" OnClick="btnVista_OnClick" CommandName="Operador" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnTercero" Text="Tercero" Width="85px" runat="server" OnClick="btnVista_OnClick" CommandName="Tercero" CssClass="boton_pestana" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upmtvRecursos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvRecursos" runat="server" ActiveViewIndex="0">
<asp:View ID="vwUnidad" runat="server">
<div class="columna_filtros_asignacion">
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNoUnidad">No. Unidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNoUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNoUnidad" runat="server" CssClass="textbox"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlEstatusUnidad">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusUnidad" runat="server" TabIndex="2" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtPropietarioUnidad">Propietario</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtPropietarioUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtPropietarioUnidad" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtUbicacionUnidad">Ubicación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtUbicacionUnidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacionUnidad" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarUnidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarUnidades" runat="server" TabIndex="4" CommandName="Unidades" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                                                                       
</asp:View>
<asp:View ID="vwOperador" runat="server">
<div class="columna_filtros_asignacion">
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlEstatusOperador">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatusOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatusOperador" runat="server" TabIndex="2" CssClass="dropdown">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNombre">Nombre</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombreOperador" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreOperador" runat="server" CssClass="textbox_240px validate[ custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>                            
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarOperadores" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarOperadores" runat="server" TabIndex="4" CommandName="Operadores" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                        
</asp:View>
<asp:View ID="vwTercero" runat="server">
<div class="columna_filtros_asignacion">                        
<div class="renglon"></div>
<div class="renglon">
<div class="etiqueta_50px">
<label for="txtNombreTercero">Nombre</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtNombreTercero" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtNombreTercero" runat="server" CssClass="textbox_240px"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscarTerceros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscarTerceros" runat="server" TabIndex="4" CommandName="Tercero" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon"></div>
</div>                        
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel> 
<div class="renglon">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna3x">
<div class="header_seccion">
<img  src="../Image/Buscar.png"/>
<h2>Recursos Disponibles</h2>
</div>       
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoRecursosDisponibles">Mostrar</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamanoRecursosDisponibles" runat="server" AutoPostBack="true" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoRecursosDisponibles_OnSelectedIndexChanged" TabIndex="15">
</asp:DropDownList>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarRecursosDisponibles">Ordenar:</label>
<asp:Label ID="lblOrdenarRecursosDisponibles" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" EventName="Sorting" />
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width: auto">
<asp:LinkButton ID="lkbExportarRecursosDisponibles" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarRecursosDisponibles_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grid_seccion_completa_media_altura">
<asp:UpdatePanel ID="upgvRecursosDisponibles" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRecursosDisponibles" runat="server" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvRecursosDisponibles_PageIndexChanging" OnSorting="gvRecursosDisponibles_Sorting"
AllowSorting="True" TabIndex="18" Width="100%" AutoGenerateColumns="false" OnRowDataBound="gvRecursosDisponibles_RowDataBound"
ShowFooter="True" CssClass="gridview">
<Columns>
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
<asp:BoundField DataField="Propio" HeaderText="Propio" SortExpression="Propio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" />
<asp:BoundField DataField="NoServicio" HeaderText="NoServicio" SortExpression="NoServicio" /> 
<asp:TemplateField HeaderText="UltimaUbicacion" SortExpression="UltimaUbicacion">
<ItemTemplate>
<asp:HyperLink ID="hlnkUltimaUbicacion" runat="server" Target="_blank" ToolTip='<%# Eval("UltimaUbicacion") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("UltimaUbicacion").ToString(), 30, "...") %>' >
</asp:HyperLink>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Pendientes" SortExpression="Asignaciones">
<ItemTemplate>
<asp:LinkButton ID="lnkAsignaciones" runat="server" ToolTip ='<%# Eval("Asignaciones") %>' OnClick="lkbServiciosRecursoDisponible_Click" >
<img src="../Image/Calendario20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField  HeaderText="Liquidar" SortExpression="PorLiquidar">
<ItemTemplate>
<asp:LinkButton ID="lnkLiquidar" runat="server" ToolTip ='<%# Eval("PorLiquidar") %>' OnClick="lnkLiquidar_Click" >
<img src="../Image/Terminado20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>                                                          
<asp:TemplateField  HeaderText="Depositos" SortExpression="PendientesLiq">
<ItemTemplate>
<asp:LinkButton ID="lnkDeposito" runat="server" ToolTip ='<%# Eval("PendientesLiq") %>' OnClick="lkbPendientesLiq_Click" >
<img src="../Image/Signo20px.png" width="20" height="20" />
</asp:LinkButton>                                        
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Agregar" SortExpression="Agregar">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbAgregar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAgregar" Text="Agregar" runat="server" OnClick="lkbAgregar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="" >
<ItemTemplate>
<asp:UpdatePanel ID="uplkbLiberar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbLiberar"  Visible="true" Text="Liberar" OnClick="lkbLiberar_Click" runat="server" ></asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="btnBuscarUnidades" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarOperadores" />
<asp:AsyncPostBackTrigger ControlID="btnBuscarTerceros" />
<asp:AsyncPostBackTrigger ControlID="btnUnidad" />
<asp:AsyncPostBackTrigger ControlID="btnTercero" />
<asp:AsyncPostBackTrigger ControlID="btnOperador" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarLiberacionRecurso" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE SERVICIOS ASIGNADOS/TERMINADOS A UNA UNIDAD -->
<div id="contenidoServiciosAsignados" class="modal">
<div id="modalServiciosAsignados" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarServiciosAsignados" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarServiciosAsignados" runat="server" Text="Cerrar" OnClick="lnkCerrarServiciosAsignados_Click">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Documento.png"/>
<h2>
<asp:updatepanel runat="server" ID="uplblServicios" UpdateMode="Conditional">
<ContentTemplate>
<asp:label runat="server" ID="lblServicios" Text="Servicios Asignados"></asp:label>
</ContentTemplate>                   
</asp:updatepanel>
</h2>
</div>
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvServiciosAsignados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvServiciosAsignados" runat="server" CssClass="gridview" Width="100%" AllowPaging="True" OnPageIndexChanging="gvServiciosAsignados_PageIndexChanging"
ShowFooter="True" AutoGenerateColumns="False" PageSize="5" OnRowDataBound="gvServiciosAsignados_RowDataBound">
<Columns>
<asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignación" SortExpression="FechaAsignacion" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
<asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
<ItemTemplate>
<asp:Label ID="lblCliente" runat="server" ToolTip='<%# Eval("Cliente") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Cliente").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sitio Carga" SortExpression="Origen">
<ItemTemplate>
<asp:Label ID="lblOrigen" runat="server" ToolTip='<%# Eval("Origen") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Origen").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CiudadOrigen" HeaderText="Ciudad" SortExpression="CiudadOrigen" />
<asp:BoundField DataField="CitaOrigen" HeaderText="Cita" SortExpression="CitaOrigen" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="Sitio Descarga" SortExpression="Destino">
<ItemTemplate>
<asp:Label ID="lblDestino" runat="server" ToolTip='<%# Eval("Destino") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Destino").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="CiudadDestino" HeaderText="Ciudad" SortExpression="CiudadDestino" />
<asp:BoundField DataField="CitaDestino" HeaderText="Cita " SortExpression="CitaDestino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:TemplateField HeaderText="">
<ItemTemplate>
<asp:HyperLink ID="lkbMapa" Target="_blank" runat="server" Text="Ver Mapa"></asp:HyperLink>
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
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>            
</div>
</div>

<!-- VENTANA MODAL QUE MUESTRA EL MENSAJE DE CONFIRMACION EN LOS CASOS EN LOS QUE SE AGREGA UNA UNIDAD LIGADA A UN OPERADOR -->
<div id="contenidoConfirmacionAsignacionRecurso" class="modal">
<div id="confirmacionAsignacionRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Confirmación de asignación</h2>
</div>              
<div class="columna2x">               
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeAsignacion" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarAsignacion" />
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarAsignacion" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarAsignacion" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarAsignacion_OnClick" Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAsignacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAsignacion" runat="server" CssClass="boton" OnClick="btnAceptarAsignacion_OnClick" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- VENTANA MODAL QUE INDICA LA EXISTENCIA DE VENCIMIENTOS ACTIVOS  -->
<div id="modalIndicadorVencimientos" class="modal">
<div id="contenidoModalIndicadorVencimientos" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<asp:UpdatePanel ID="upimgAlertaVencimiento" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:Image ID="imgAlertaVencimiento" runat="server" />
</ContentTemplate>
</asp:UpdatePanel>
<h3>¡Existen Vencimientos Activos!</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeHistorialVencimientos" runat="server" CssClass="mensaje_modal"></asp:Label>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplkbVerHistorialVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbVerHistorialVencimientos" runat="server" Text="Mostrar Vencimientos Activos"
OnClick="lkbVerHistorialVencimientos_Click"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarIndicadorVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarIndicadorVencimientos" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarIndicadorVencimientos_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

<!-- VENTANA MODAL QUE MUESTRA LOS VENCIMIENTOS ACTIVOS -->
<div id="modalHistorialVencimientos" class="modal">
<div id="vencimientosRecurso" class="contenedor_modal_seccion_completa">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialVencimientos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarHistorialVencimientos" runat="server" OnClick="lkbCerrarHistorialVencimientos_Click" CommandName="Historial" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Calendar2.png" />
<h2>Vencimientos Activos</h2>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvVencimientos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvVencimientos" runat="server"  AutoGenerateColumns="False" PageSize="10"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvVencimientos_PageIndexChanging" AllowPaging="True" OnRowDataBound="gvVencimientos_RowDataBound" >
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" />
<asp:BoundField DataField="TipoRecurso" HeaderText="TipoRecurso" />
<asp:BoundField DataField="Recurso" HeaderText="Recurso" />
<asp:BoundField DataField="TipoVencimiento" HeaderText="Tipo" SortExpression="TipoVencimiento" />
<asp:BoundField DataField="Prioridad" HeaderText="Prioridad" SortExpression="Prioridad" />
<asp:BoundField DataField="FechaInicio" HeaderText="Inicio" SortExpression="FechaInicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />                                  
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
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
</asp:UpdatePanel> 
</div>
</div>
</div>

<!-- VENTANA MODAL QUE MUESTRA CONSULTA DE ANTICIPOS REALIZADOS A LA ENTIDAD -->
<div id="contenidoAnticiposPendientes" class="modal">
<div id="modalAnticiposPendientes" class="contenedor_modal_seccion_completa"">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAnticipos" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAnticiposP" runat="server" Text="Cerrar" OnClick="lkbCerrarAnticiposP_Click"> 
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img  src="../Image/Depositos.png"/>
<h2>Depositos y Vales Asignados</h2>
</div>            
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvAnticiposPendientes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAnticiposPendientes" runat="server" CssClass="gridview" Width="100%" AllowPaging="True"
ShowFooter="True" AutoGenerateColumns="False" PageSize=" 10">
<Columns>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:BoundField DataField="Num" HeaderText="Folio" SortExpression="Num" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
<asp:BoundField DataField="Precio" HeaderText="Costo" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="Monto" HeaderText="Total" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
<asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización " SortExpression="FechaAutorizacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FechaCargaoDeposito" HeaderText="Fecha Carga o Depósito" SortExpression="FechaCargaoDeposito" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" />
<asp:TemplateField HeaderText="Movimiento" SortExpression="Movimiento">
<ItemTemplate>
<asp:Label ID="lblMovimiento" runat="server" ToolTip='<%# Eval("Movimiento") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Movimiento").ToString(), 25, "...") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Información" SortExpression="Informacion">
<ItemTemplate>
<asp:Label ID="lblInformacion" runat="server" ToolTip='<%# Eval("Informacion") %>' Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Informacion").ToString(), 25, "...") %>'></asp:Label>
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
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>

<!-- VENTANA MODAL QUE ADVIERTE SOBRE LA LIBERACIÓN DE TRACTORES Y OPERADORES VICULADOS -->
<div id="contenidoConfirmacionLiberacionRecurso" class="modal">
<div id="confirmacionLiberacionRecurso" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Liberación de Recursos Vinculados</h2>
</div>            
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblMensajeLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMensajeLiberacionRecurso" CssClass="mensaje_modal" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRecursosDisponibles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarLiberacionRecurso" UpdateMode="Conditional" runat="server">
<ContentTemplate>
<asp:Button ID="btnCancelarLiberacionRecurso" runat="server"  OnClick="btnCancelarLiberacionRecurso_Click" CssClass="boton_cancelar"  Text="Cancelar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarLiberacionRecurso" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarLiberacionRecurso"  OnClick="btnAceptarLiberacionRecurso_Click" runat="server" CssClass="boton"  Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>