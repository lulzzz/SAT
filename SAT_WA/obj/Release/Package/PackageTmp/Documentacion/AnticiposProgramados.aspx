<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="AnticiposProgramados.aspx.cs" Inherits="SAT.Documentacion.AnticiposProgramados" %>

<%@ Register Src="~/UserControls/wucAnticipoProgramado.ascx" TagName="wucAnticipoProgramado" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucAsignacionDieselProgramado.ascx" TagName="wucAsignacionDieselProgramado" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucRutaIave.ascx" TagPrefix="tectos" TagName="wucCalcularRuta" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagName="wucKilometraje" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucCostoCombustible.ascx" TagName="wucCostoCombustible" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Referencia a Hoja de Estilos requeridas -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos JQuery -->
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script src="../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteServicios();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryReporteServicios() {
            $(document).ready(function () {

<%--                $('#<%=gvServiciosContextual.ClientID %>').gridviewScroll({
                    width: document.getElementById("reporteServicios").offsetWidth - 15,
                    height: 450
                });--%>

                <%--$('#<%=gvAnticipos.ClientID %>').gridviewScroll({
                    width: document.getElementById("Anticipos").offsetWidth - 15,
                    height: 600
                });--%>

                //Validación 
                var validacionReporteServicio = function () {

                    var isValidP1 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtOrigen.ClientID%>").validationEngine('validate');
                    var isValidP4 = !$("#<%=txtDestino.ClientID%>").validationEngine('validate');
                    var isValidP5 = !$("#<%=txtNoViaje.ClientID%>").validationEngine('validate');

                    return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5;
                };
                //Validación de campos requeridos
                $("#<%=this.btnBuscar.ClientID%>").click(validacionReporteServicio);

                // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
                $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });


                /* Autocompleta origen, destino y cliente */
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
                $("#<%=txtOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
                $("#<%=txtDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
                $("#<%=txtEconomico.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });
            });


            $(document).keyup(function (e) {
                if (e.keyCode == 27) { // escape key maps to keycode `27`
                    //Ocultando Menu
                    OcultarMenu();
                }
            });
            $(document).click(function (e) {

                //Ocultando Menu
                OcultarMenu();
            });

        }

        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryReporteServicios();
        //Función encargada de Mostrar el Ménu
        function MostrarMenu(control, e) {
            //Ocultando en caso de estar Abierto
            OcultarMenu();

            //Obteniendo Coordenadas de las Forma
            var posx = e.pageX + 'px';
            var posy = e.pageY + 'px';

            //Si el Evento es de Tipo Click
            if (e.type == 'click')

                //Detener Propagación del Evento
                e.stopPropagation();

            //Asignando Posiciones al Documento
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;


            //Ejecutando 
            $(document).ready(function (evt) {

                //Mostrando DIV
                $('#' + control).slideDown(100);
            });
        }
        //Función encargada de Ocultar el Ménu
        function OcultarMenu() {
            $(document).ready(function () {
                //Ocultando DIV
                $('.menuContainer').slideUp(100);
            });
        }
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Circle-Programado.png" width="38" />
        <h1>Anticipos programados</h1>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Filtros de búsqueda</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">No. de servicio</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="1" MaxLength="30"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">Cliente</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOrigen">Origen</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtOrigen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="3"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDestino">Destino</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtDestino" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtEconomico">No. Economico</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtEconomico" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoViaje">No. de viaje</label>
                </div>
                <div class="control2x">
                    <asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox" TabIndex="5"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar x Fecha"
                        Checked="false" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" TabIndex="11" />
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uprdbCitaCarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rdbCitaCarga" runat="server" CssClass="label" Text="Cita Carga" GroupName="FiltroFecha" TabIndex="12" Checked="true" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uprdbCitaDescarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rdbCitaDescarga" runat="server" CssClass="label" Text="Cita Descarga" GroupName="FiltroFecha" TabIndex="13" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta"></div>
                <div class="control">
                    <asp:UpdatePanel ID="uprdbInicioServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rdbInicioServicio" runat="server" CssClass="label" Text="Inicio Servicio" GroupName="FiltroFecha" TabIndex="14" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uprdbFinServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButton ID="rdbFinServicio" runat="server" CssClass="label" Text="Fin Servicio" GroupName="FiltroFecha" TabIndex="15" Enabled="false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaInicio">Desde</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="16"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaFin">Hasta</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox2x validate[custom[dateTime24]]" TabIndex="17"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="boton" OnClick="btnBuscar_Click" Text="Buscar" TabIndex="6" />
                </div>
            </div>
        </div>
    </div>

    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Totales.png" />
            <h2>Servicios encontrados</h2>
        </div>

        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamañoGridViewServicios">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewServicios" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewServicios" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewServicios_SelectedIndexChanged" TabIndex="7" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblCriterioGridViewServicios">Ordenado por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterioGridViewServicios" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewServicios" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>


        <%--<div id="reporteServicios" class="grid_seccion_completa_altura_variable" oncontextmenu="return false">
            <asp:UpdatePanel ID="upgvServicios" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvServicios" CssClass="gridview" OnPageIndexChanging="gvServicios_PageIndexChanging" OnSorting="gvServicios_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        ShowFooter="True" TabIndex="21"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="IdServicio" HeaderText="Id Servicio" SortExpression="IdServicio" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" ItemStyle-Width="42px" HeaderStyle-Width="42px">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CopiaDe" HeaderText="Copia De" SortExpression="CopiaDe" HeaderStyle-Width="71px" ItemStyle-Width="71px" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" HeaderStyle-Width="125px" ItemStyle-Width="125px" />
                            <asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="ReferenciaCliente" HeaderText="Referencia Cliente" SortExpression="ReferenciaCliente" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="DocumentadoPor" HeaderText="Documentado Por" SortExpression="DocumentadoPor" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FechaDocumentacion" HeaderText="Fecha Documentación" SortExpression="FechaDocumentacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="InicioViaje" HeaderText="Inicio Viaje" SortExpression="InicioViaje" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="LugarCarga" HeaderText="Lugar Carga" SortExpression="LugarCarga" HeaderStyle-Width="90px" ItemStyle-Width="90px" />
                            <asp:BoundField DataField="CitaCarga" HeaderText="Cita Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Carga" SortExpression="LlegadaCarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Salida Carga" SortExpression="SalidaCarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LugarDescarga" HeaderText="Lugar Descarga" SortExpression="LugarDescarga" />
                            <asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Descarga" SortExpression="LlegadaDescarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaTerminoViaje" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Término Viaje" SortExpression="FechaTerminoViaje">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
                            <asp:BoundField DataField="IdMovimiento" HeaderText="Id Movimiento" SortExpression="IdMovimiento" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="IdMovimientoAsignacion" HeaderText="Id Movimiento Asignación" SortExpression="IdMovimientoAsignacion" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="IdRecurso" HeaderText="IdRecurso" SortExpression="IdRecurso" Visible="false" />
                            <asp:TemplateField HeaderText="Anticipos/Vales">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerAnticipos" runat="server" OnClick="lkbVerAnticipos_Click"><img src="../Image/Depositos.png" width="22" height="22" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AnticiposProgramados" HeaderText="Anticipos Programados" SortExpression="AnticiposProgramados">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Programación">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbProgramacion" runat="server" OnClick="lkbProgramacion_Click"><img src="../Image/Calendar.jpg" width="22" height="22" /></asp:LinkButton>
                                </ItemTemplate>                            
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div id="menuContext" runat="server">
                                        <img src="../Image/menu_context2.png" />
                                    </div>
                                    <div id="menuOptions" runat="server" class="MenuContext menuContainer" style="display: none;">
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
                                        </div>
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
                                        </div>
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkTerminarSoporte" runat="server" Text="Terminar Soporte" OnClick="lkbAccionUnidad_Click"></asp:LinkButton>
                                        </div>
                                    </div>
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
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewServicios" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalProgramacion" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModal" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalProgramarAnticipo" />
                    <asp:AsyncPostBackTrigger ControlID="ucDepositos" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalAsignacionDieselProgramado" />
                    <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalCalcularRuta" />
                </Triggers>
            </asp:UpdatePanel>
        </div>--%>

        <div class="grid_seccion_completa_400px_altura" oncontextmenu="return false">
            <asp:UpdatePanel ID="upgvServiciosContextual" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvServiciosContextual" runat="server" PageSize="25" Width="100%" TabIndex="13"
                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true" OnSorting="gvServiciosContextual_Sorting"
                        OnRowDataBound="gvServiciosContextual_RowDataBound" OnPageIndexChanging="gvServiciosContextual_PageIndexChanging"
                        CssClass="gridview" ShowFooter="True">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:BoundField DataField="IdServicio" HeaderText="Id Servicio" SortExpression="IdServicio" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="NoServicio" HeaderText="No Servicio" SortExpression="NoServicio" ItemStyle-Width="42px" HeaderStyle-Width="42px">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CopiaDe" HeaderText="Copia De" SortExpression="CopiaDe" HeaderStyle-Width="71px" ItemStyle-Width="71px" />
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" HeaderStyle-Width="125px" ItemStyle-Width="125px" />
                            <asp:BoundField DataField="Porte" HeaderText="Porte" SortExpression="Porte" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                            <asp:BoundField DataField="ReferenciaCliente" HeaderText="Referencia Cliente" SortExpression="ReferenciaCliente" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                            <asp:BoundField DataField="DocumentadoPor" HeaderText="Documentado Por" SortExpression="DocumentadoPor" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FechaDocumentacion" HeaderText="Fecha Documentación" SortExpression="FechaDocumentacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="InicioViaje" HeaderText="Inicio Viaje" SortExpression="InicioViaje" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="LugarCarga" HeaderText="Lugar Carga" SortExpression="LugarCarga" HeaderStyle-Width="90px" ItemStyle-Width="90px" />
                            <asp:BoundField DataField="CitaCarga" HeaderText="Cita Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LlegadaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Carga" SortExpression="LlegadaCarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SalidaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Salida Carga" SortExpression="SalidaCarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LugarDescarga" HeaderText="Lugar Descarga" SortExpression="LugarDescarga" />
                            <asp:BoundField DataField="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Cita Descarga" SortExpression="CitaDescarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LlegadaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Llegada Descarga" SortExpression="LlegadaDescarga">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaTerminoViaje" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Término Viaje" SortExpression="FechaTerminoViaje">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Remolque" HeaderText="Remolque" SortExpression="Remolque">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Transportista" HeaderText="Transportista" SortExpression="Transportista" />
                            <asp:BoundField DataField="IdMovimiento" HeaderText="Id Movimiento" SortExpression="IdMovimiento" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="IdMovimientoAsignacion" HeaderText="Id Movimiento Asignación" SortExpression="IdMovimientoAsignacion" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                            <asp:BoundField DataField="IdRecurso" HeaderText="IdRecurso" SortExpression="IdRecurso" Visible="false" />
                            <asp:TemplateField HeaderText="Anticipos/Vales">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerAnticipos" runat="server" OnClick="lkbVerAnticipos_Click"><img src="../Image/Depositos.png" width="22" height="22" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AnticiposProgramados" HeaderText="Anticipos Programados" SortExpression="AnticiposProgramados">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Programación">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbProgramacion" runat="server" OnClick="lkbProgramacion_Click"><img src="../Image/Calendar.jpg" width="22" height="22" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div id="menuContext" runat="server">
                                        <img src="../Image/menu_context2.png" />
                                    </div>
                                    <div id="menuOptions" runat="server" class="MenuContext menuContainer" style="display: none;">
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkProgramacion" runat="server" Text="Programación" CommandName="Programacion" OnClick="lkbAcciones_Click"></asp:LinkButton>
                                        </div>
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkCartaPorte" runat="server" Text="Carta Porte" CommandName="CartaPorte" OnClick="lkbAcciones_Click"></asp:LinkButton>
                                        </div>
                                        <div class="ContextItem">
                                            <asp:LinkButton ID="lnkHojaInstrucciones" runat="server" Text="Hoja Instrucciones" CommandName="HojaInstruccion" OnClick="lkbAcciones_Click"></asp:LinkButton>
                                        </div>
                                        <div class="ContextItem">
                                            <asp:UpdatePanel ID="uplnkGastosGenerales" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lnkGastosGenerales" runat="server" Text="Imprimir Gastos" CommandName="GastosGenerales" OnClick="lkbAcciones_Click"></asp:LinkButton>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="lnkGastosGenerales" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
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
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewServicios" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalProgramacion" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModal" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalProgramarAnticipo" />
                    <asp:AsyncPostBackTrigger ControlID="ucDepositos" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalAsignacionDieselProgramado" />
                    <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalCalcularRuta" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

    </div>

    <!--VENTANA MODAL PARA ELEJIR LA OPCIÓN A PROGRAMAR-->
    <div id="contenedorProgramacion" class="modal">
        <div id="ventanaProgramacion" class="contenedor_ventana_confirmacion_arriba" style="min-width: 750px; width: 750px; padding-bottom: 5px;">
            <div class="columna" style="min-width: 750px; width: 750px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalProgramacion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModalProgramacion" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalProgramacion_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/Calendario.png" style="width: 32px;" />
                    <h2>Elija una acción a realizar</h2>
                </div>
                <div class="renglon" style="width: 700px">
                    <div class="controlBoton" style="width: 200px">
                        <asp:UpdatePanel ID="upbtnCalcularRuta" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCalcularRuta" runat="server" Text="Calcular ruta" CssClass="boton" Style="width: 150px"
                                    OnClick="btnCalcularRuta_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                                <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton" style="width: 200px">
                        <asp:UpdatePanel ID="upbtnValeDiesel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnValeDiesel" runat="server" Text="Vale de diésel" CssClass="boton" Style="width: 150px"
                                    OnClick="btnValeDiesel_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                                <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton" style="width: 200px">
                        <asp:UpdatePanel ID="upbtnProgramarAnticipo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnProgramarAnticipo" runat="server" Text="Programar anticipo" CssClass="boton" Style="width: 150px"
                                    OnClick="btnProgramarAnticipo_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                                <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!--VENTANA MODAL DE ANTICIPOS DE LOS SERVICIOS-->
    <div id="contenedorVerAnticipos" class="modal">
        <div id="ventanaVerAnticipos" class="contenedor_ventana_confirmacion_arriba" style="min-width: 950px; width: 950px; height: 450px; padding-bottom: 5px;">
            <div class="columna3x" style="min-width: 950px; width: 950px; height: 450px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModal" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModal" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModal_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/Depositos.png" style="width: 32px;" />
                    <asp:UpdatePanel ID="uph2EncabezadoAnticiposProgramados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h2 id="h2EncabezadoAnticiposProgramados" runat="server">Anticipos del servicio</h2>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamañoGridViewAnticipos">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamañoGridViewAnticipos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamañoGridViewAnticipos" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewAnticipos_SelectedIndexChanged" TabIndex="8" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblCriterioGridViewAnticipos">Ordenado por:</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblCriterioGridViewAnticipos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCriterioGridViewAnticipos" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAnticipos" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

                <div id="Anticipos" class="grid_seccion_completa_200px_altura"  style="height: 320px;">
                    <asp:UpdatePanel ID="upgvAnticipos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAnticipos" CssClass="gridview" OnPageIndexChanging="gvAnticipos_PageIndexChanging" OnSorting="gvAnticipos_Sorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                                ShowFooter="True" TabIndex="22" OnRowDataBound="gvAnticipos_RowDataBound"
                                PageSize="5" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Width="42px" HeaderStyle-Width="42px" Visible="false" />
                                    <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Num" HeaderText="Folio" SortExpression="Num" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="71px" ItemStyle-Width="71px" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" ItemStyle-Width="70px" HeaderStyle-Width="70px" DataFormatString="{0:C2}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" HeaderStyle-Width="125px" ItemStyle-Width="125px" DataFormatString="{0:C2}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaAuth" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Autorización" SortExpression="FechaAuth">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaDep" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Fecha Depósito" SortExpression="FechaDep">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FechaEjecucion" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Fecha Ejecución" SortExpression="FechaEjecucion">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReferenciapOpUn" HeaderText="Referencia Opr./U." SortExpression="ReferenciapOpUn" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="FacProveedor" HeaderText="Factura Prov." SortExpression="FacProveedor" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" SortExpression="Movimiento" ItemStyle-Width="200px" HeaderStyle-Width="200px" />
                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                                    <asp:BoundField DataField="Programado" HeaderText="Programado" SortExpression="Programado" ItemStyle-Width="70px" HeaderStyle-Width="70px" Visible="false" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEditarAnticipoProgramado" runat="server" Text="Editar" OnClick="lkbAnticiposProgramados_OnClick" CommandName="Editar"></asp:LinkButton>
                                            <asp:LinkButton ID="lkbEditarAsignacionDieselProgramado" runat="server" Text="Editar vale diésel" OnClick="lkbAnticiposProgramados_OnClick" CommandName="EditarValeDiesel"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEliminarAnticipoProgramado" runat="server" Text="Eliminar" OnClick="lkbAnticiposProgramados_OnClick" CommandName="Eliminar"></asp:LinkButton>
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
                            <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewAnticipos" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModal" />
                            <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                            <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                            <asp:AsyncPostBackTrigger ControlID="ucDepositos" />
                            <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalAsignacionDieselProgramado" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!--VENTANA MODAL PARA PROGRAMAR ANTICIPOS-->
    <div id="contenedorProgramarAnticipo" class="modal">
        <div id="ventanaProgramarAnticipo" class="contenedor_ventana_confirmacion_arriba" style="min-width: 672px; width: 672px; padding-bottom: 5px;">
            <div class="columna3x" style="min-width: 672px; width: 672px; padding-bottom: 5px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalProgramarAnticipo" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaModalProgramarAnticipo" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalProgramarAnticipo_Click">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="columna">
                    <asp:UpdatePanel ID="upucDepositos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <tectos:wucAnticipoProgramado ID="ucDepositos" runat="server" OnClickRegistrar="ucDepositos_ClickRegistrar" OnClickEliminar="ucDepositos_ClickEliminar" OnClickCancelar="ucDepositos_ClickCancelar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                            <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalProgramarAnticipo" />
                            <asp:AsyncPostBackTrigger ControlID="ucDepositos" />
                            <asp:AsyncPostBackTrigger ControlID="btnProgramarAnticipo" />
                            <asp:AsyncPostBackTrigger ControlID="btnCalcularRuta" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!--VENTANA MODAL PARA PROGRAMAR ASIGNACIÓN DE DIÉSEL-->
    <div id="contenedorAsignacionDieselProgramado" class="modal">
        <div id="ventanaAsignacionDieselProgramado" class="contenedor_ventana_confirmacion_arriba" style="min-width: 1000px; width: 1000px; padding-right: 10px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalAsignacionDieselProgramado" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarVentanaModalAsignacionDieselProgramado" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalAsignacionDieselProgramado_Click">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucDiesel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucAsignacionDieselProgramado ID="ucAsignacionDiesel" runat="server" OnClickGuardarAsignacion="ucAsignacionDiesel_ClickGuardarAsignacion" OnClickCancelarAsignacion="ucAsignacionDiesel_ClickCancelarAsignacion" OnClickCalculadoDiesel="ucAsignacionDiesel_ClickCalculado1" OnClickCostoDiesel="ucAsignacionDiesel_ClickCostoDiesel" OnClickCalculadoSegmento="ucAsignacionDiesel_ClickCalculadoSegmentos" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServiciosContextual" />
                    <asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalAsignacionDieselProgramado" />
                    <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="btnValeDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="btnValeDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarSegmentos" />
                    <asp:AsyncPostBackTrigger ControlID="wucCostoCombustible" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarVentanaModalCostoCombustible" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!--VENTANA MODAL QUE DESPLIEGA LA INFORMACIÓN CALCULADA DIÉSEL VS KILÓMETROS-->
    <div id="contenedorVentanaConfirmacionInformacionCalculado" class="modal">
        <div id="ventanaConfirmacionInformacionCalculado" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarVentanaInformacionCalculado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarVentanaInformacionCalculado" runat="server" CommandName="InformacionCalculado" Text="Cerrar" OnClick="lnkCerrarVentanaInformacionCalculado_Click">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Información Diesel vs Kms.</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblCapacidadTanque">Capacidad Tanque:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblCapacidadTanque" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCapacidadTanque" runat="server" Text="0"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblFechaUnltimaCarga">Fecha Última Carga:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblFechaUltimaCarga" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblFechaUltimaCarga" runat="server" Text="Por Asignar"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblKmsUltimaCarga">Kms. Última Carga:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblKmsUltimaCarga" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblKmsUltimaCarga" runat="server" Text="0 kms"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblRendimiento">Rendimiento:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblRendieminto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblRendimiento" runat="server" Text="0 kms/lts"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblCalculado">Calculado:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblCalculado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCalculado" runat="server" CssClass="label_error" Text="0lts"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblSobrante">Sobra tanque:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblSobrante" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblSobrante" runat="server" Text="0lts"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_155px">
                        <label for="lblSobrante">Alcance Kms:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblAlcanceKms" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblAlcanceKms" runat="server" Text="0kms"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <!--VENTANA MODAL PARA CALCULAR LA RUTA-->
    <div id="contenedorCalcularRuta" class="modal">
        <div id="ventanaCalcularRuta" class="contenedor_ventana_confirmacion_arriba" style="min-width: 1000px; width: 1000px; padding-bottom: 5px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalCalcularRuta" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarVentanaModalCalcularRuta" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalCalcularRuta_Click">
                                <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucCalcularRuta" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucCalcularRuta ID="wucCalcularRuta" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnCalcularRuta" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!--VENTANA MODAL SEGMENTOS KILOMETROS-->
    <div id="Segmentos" class="modal">
        <div id="contenedorSegmentos" class="contenedor_ventana_confirmacion_arriba" style="min-width: 672px; width: 672px; padding-bottom: 5px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarSegmentos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarSegmentos" runat="server" OnClick="lkbCerrarVentanaModal1_Click" CommandName="Segmentos" Text="Cerrar">
                             <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" style="width: 32px;" />
                <h2>Información Kms</h2>
            </div>
            <div class="grid_seccion_completa_media_altura" style="height: 150px">
                <asp:UpdatePanel ID="upgvActualizacionkms" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvActualizacionkms" runat="server" AllowSorting="true" AllowPaging="true"
                            CssClass="gridview" OnPageIndexChanging="gvActualizacionkms_PageIndexChanging"
                            AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="100%" OnRowDataBound="gvActualizacionkms_RowDataBound">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="IdKilometraje" HeaderText="IdKilometraje" SortExpression="IdKilometraje" Visible="false" />
                                <asp:BoundField DataField="IdServicio" HeaderText="IdServicio" SortExpression="IdServicio" Visible="false" />
                                <asp:BoundField DataField="IdUbicacionOrigenActuales" HeaderText="IdUbicacionOrigen" SortExpression="IdUbicacionOrigen" Visible="false" />
                                <asp:BoundField DataField="IdUbicacionDestino" HeaderText="IdUbicacionDestino" SortExpression="IdUbicacionDestino" Visible="false" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                <asp:TemplateField HeaderText="kms" SortExpression="kms">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbKmsActualizar" runat="server" Text='<%# Eval("Kilometrajes") %>' OnClick="lkbKmsActualizar_Click" CommandName="Editar"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="ucKilometraje" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL DE ACTUALIZACIÓN (INSERCIÓN/EDICIÓN) KILOMETRAJE -->
    <div id="contenedorVentanaKilometraje" class="modal">
        <div id="ventanaKilometraje" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrar" runat="server" OnClick="lkbCerrarVentanaModal1_Click" CommandName="Kilometros" Text="Cerrar">
                         <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucKilometraje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucKilometraje ID="ucKilometraje" runat="server" OnClickGuardar="ucKilometraje_ClickGuardar" Contenedor="#ventanaKilometraje" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvActualizacionkms" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--VENTANA MODAL PARA MODIFICAR Y/O GUARDAR UN COSTO DE DIESEL -->
    <div id="contenedorCostoCombustible" class="modal">
        <div id="ventanaCostoCombustible" class="contenedor_ventana_confirmacion_arriba" style="min-width: 950px; width: 950px; padding-bottom: 2px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaModalCostoCombustible" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarVentanaModalCostoCombustible" runat="server" Text="Cerrar" OnClick="lkbCerrarVentanaModalCostoCombustible_Click" CommandName="CostoDiesel">
                                <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucCostoCombustible" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucCostoCombustible ID="wucCostoCombustible" runat="server" OnClickGuardar="wucCostoCombustible_ClickGuardar"/>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ucAsignacionDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="wucCostoCombustible" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
