<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Despacho.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Operacion.Despacho" %>

<%@ Register Src="~/UserControls/wucProducto.ascx" TagName="wucProducto" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucClasificacion.ascx" TagPrefix="tectos" TagName="wucClasificacion" %>
<%@ Register Src="~/UserControls/wucFacturadoConcepto.ascx" TagPrefix="tectos" TagName="wucFacturadoConcepto" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagPrefix="tectos" TagName="wucReferenciaViaje" %>
<%@ Register Src="~/UserControls/wucTerminoMovimientoVacio.ascx" TagPrefix="operacion" TagName="wucTerminoMovimientoVacio" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagPrefix="tectos" TagName="wucKilometraje" %>
<%@ Register Src="~/UserControls/wucAsignacionRecurso.ascx" TagPrefix="tectos" TagName="wucAsignacionRecurso" %>
<%@ Register Src="~/UserControls/wucMovimientoVacioSinOrden.ascx" TagPrefix="tectos" TagName="wucMovimientoVacioSinOrden" %>
<%@ Register Src="~/UserControls/wucRuta.ascx" TagPrefix="tectos" TagName="wucCalcularRuta" %>
<%@ Register Src="~/UserControls/wucImpresionDocumentos.ascx" TagPrefix="tectos" TagName="wucImpresionDocumentos" %>
<%@ Register Src="~/UserControls/wucEncabezadoServicio.ascx" TagPrefix="uc1" TagName="wucEncabezadoServicio" %>
<%@ Register Src="~/UserControls/wucDevolucionFaltante.ascx" TagName="wucDevolucionFaltante" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreoHistorial.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreoHistorial" %>
<%@ Register Src="~/UserControls/wucBitacoraMonitoreo.ascx" TagPrefix="uc1" TagName="wucBitacoraMonitoreo" %>
<%@ Register Src="~/UserControls/wucImpresionPorte.ascx" TagPrefix="tectos" TagName="wucImpresionPorte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
    <link href="../CSS/ControlPatio.css" rel="stylesheet" />
    <link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Operacion.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">

    <!-- Validación de datos de este formulario -->
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryDespacho();
            }
        }
        //Creando función para configuración de jquery en formulario
        function ConfiguraJQueryDespacho() {
            //Función de validación Búsqueda Servicio
            var validacionBuscarServicio = function (evt) {
                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtCiudadOrigen.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtCiudadDestino.ClientID%>").validationEngine('validate');
                return isValid1 && isValid2 && isValid3
            };
            //Función de validación Fecha Llegada
            var validacionFechaLlegada = function (evt) {
                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                var isValid1 = !$("#<%=txtFechaLlegada.ClientID%>").validationEngine('validate');
                return isValid1
            };
            //Función de validación Fecha Llegada
            var validacionFechaSalida = function (evt) {
                //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                var isValid1 = !$("#<%=txtFechaSalida.ClientID%>").validationEngine('validate');
                return isValid1
            };
            //Botón Buscar Servicios
            $("#<%=btnBuscarServicios.ClientID %>").click(validacionBuscarServicio);
            //Aceptar Fecha Llegada
            $("#<%= btnAceptarIngresoLlegada.ClientID %>").click(validacionFechaLlegada);
            //Aceptar Fecha Salida
            $("#<%= btnAceptarIngresoSalida.ClientID %>").click(validacionFechaSalida);
            // *** Catálogos Autocomplete *** //

            $(document).ready(function () {
                $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString() %>' });
                $("#<%=txtCiudadOrigen.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8' });
                $("#<%=txtCiudadDestino.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=8' });
                //Sugerencias de Ubicación
                $("#<%= txtUbicacionP.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>' });

            });
            $("#<%=txtFechaLlegada.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaSalida.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaInicioEvento.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaFinEvento.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtCita.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaTermino.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryDespacho();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/OperacionPatio.png" />
        <h1>Despacho de Unidades</h1>
    </div>
    <nav id="menuForma">
        <ul>
            <li class="gray">
                <a href="#" class="fa fa-book "></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbInsertaParada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbInsertaParada" runat="server" Text="Inserta Parada" OnClick="lkbElementoMenu_Click" CommandName="InsertarParada" />
                            </ContentTemplate>
                            <Triggers></Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbDeshabilitaParada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbDeshabilitaParada" runat="server" Text="Quitar Parada" OnClick="lkbElementoMenu_Click" CommandName="DeshabilitaParada" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbCargos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbCargos" runat="server" Text="Cargos" OnClick="lkbElementoMenu_Click" CommandName="Cargos" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="blue">
                <a href="#" class="fa fa-cog"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbClasificacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbClasificacion" runat="server" Text="Clasificación" OnClick="lkbElementoMenu_Click" CommandName="Clasificacion" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbProducto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbProducto" runat="server" Text="Producto" OnClick="lkbElementoMenu_Click" CommandName="Producto" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbReferenciaViaje" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbReferenciaViaje" runat="server" Text="Referencias de Viaje" OnClick="lkbElementoMenu_Click" CommandName="ReferenciasViaje" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbDespachoMTC" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbDespachoMTC" runat="server" Text="Despacho MTC" OnClick="lkbElementoMenu_Click" CommandName="MTCDespacho" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="yellow">
                <a href="#" class="fa fa-flag-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbReversaLlegada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbReversaLlegada" runat="server" Text="Reversa en Llegada" OnClick="lkbElementoMenu_Click" CommandName="ReversaLlegada" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbReversaSalida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbReversaSalida" runat="server" Text="Reversa en Salida" OnClick="lkbElementoMenu_Click" CommandName="ReversaSalida" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbTerminoMovimientoVacio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbTerminoMovimientoVacio" runat="server" Text="Movimientos en Vacío" OnClick="lkbElementoMenu_Click" CommandName="TerminoMovimientoVacio" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="red">
                <a href="#" class="fa fa-pencil-square-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel runat="server" ID="uplkbCalcular" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbCalcular" runat="server" Text="Calcular Ruta" OnClick="lkbElementoMenu_Click" CommandName="CalcularRuta" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel runat="server" ID="uplnkImpresion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkImpresion" runat="server" Text="Imprimir Documentos" OnClick="lkbElementoMenu_Click" CommandName="ImprimirDocumentos" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
        </ul>
    </nav>
    <section class="fila_indicador">
        <a href="#" class="indicador">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblIniciar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblIniciar"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/Iniciar.png" />
            </div>
            <div class="leyenda_indicador">
                Servicios x iniciar
            </div>
        </a>
        <a href="#" class="indicadorL">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblTransito" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblTransito"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/EnTransito.png" />
            </div>
            <div class="leyenda_indicador">
                Servicios en Transito
            </div>
        </a>
        <a href="#" class="indicadorL">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="uplblCajas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblCajas"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorCajaEstaciona.png" />
            </div>
            <div class="leyenda_indicador">
                Cajas en Cliente
            </div>
        </a>
        <a href="#" class="indicadorL">
            <div class="numero_indicador">
                <asp:UpdatePanel runat="server" ID="upplblTerminar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblTerminar"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/Terminar.png" />
            </div>
            <div class="leyenda_indicador">
                Servicios x Terminar
            </div>
        </a>
        <a href="#" class="indicador_texto">
            <div class="texto_indicador">
                <asp:UpdatePanel runat="server" ID="upplblEstancia" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblEstancia"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="imagen_indicador">
                <img src="../Image/IndicadorTiempo.png" />
            </div>
            <div class="leyenda_indicador">
                Estadia Cliente Promedio
            </div>
        </a>
    </section>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Busqueda de Servicios</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x"></div>
            <!--CHECAR-->
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">
                        No Servicio
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" MaxLength="30" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upchkTerminado" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkTerminado" runat="server" Text="Incluir Terminados" Checked="false" TabIndex="2" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoReferencia">
                        Referencia
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoReferencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoReferencia" runat="server" CssClass="textbox" MaxLength="30" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <!--CHECAR-->
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCartaPorte">
                        Porte
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCartaPorte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox" MaxLength="30" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <!--CHECAR-->
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCliente">
                        Cliente
                    </label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCiudadOrigen">Ciudad Origen</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCiudadOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCiudadOrigen" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCiudadDestino">Ciudad Destino</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCiudadDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCiudadDestino" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscarServicios" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscarServicios" runat="server" CssClass="boton" Text="Buscar" TabIndex="8" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control" style="width: auto">
                    <asp:UpdatePanel ID="uplblErrorServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorServicio" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                            <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="contenedor_730px_derecha">
            <div class="renglon2x">
                <div class="etiqueta" style="width: auto">
                    <label for="ddlTamanoSevicios">
                        Mostrar:
                    </label>
                </div>
                <div class="control">
                    <asp:DropDownList ID="ddlTamanoSevicios" runat="server" AutoPostBack="true" CssClass="dropdown" TabIndex="9" OnSelectedIndexChanged="ddlTamanoSevicios_OnSelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenarServicios">Ordenado Por:</label>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblOrdenarSevicios" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblOrdenarSevicios" runat="server"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServicios" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control" style="width: auto">
                    <asp:LinkButton ID="lkbExportarServicios" runat="server" Text="Exportar" TabIndex="10" OnClick="lkbExportarServicios_OnClick"></asp:LinkButton>
                </div>
            </div>
            <div class="grid_seccion_completa_200px_altura">
                <asp:UpdatePanel ID="upgvSevicios" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvServicios" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gvServicios_Sorting" OnPageIndexChanging="gvServicios_PageIndexChanging" AutoGenerateColumns="False" TabIndex="11" ShowFooter="True" CssClass="gridview" OnRowDataBound="gvServicios_RowDataBound" Width="100%" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Servicio" SortExpression="Servicio">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbServicio" runat="server" Text='<%#Eval("Servicio") %>' OnClick="lkbServicio_OnClick" CommandName="Seleccionar"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                                <asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbReferencia" runat="server" Text='<%# Eval("Referencia")%>' OnClick="lkbServicio_OnClick" CommandName="ReferenciasViaje" TabIndex="12"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="C.Porte" SortExpression="CartaPorte">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbCartaPorte" runat="server" Text='<%# Eval("CartaPorte")%>' OnClick="lkbServicio_OnClick" CommandName="EncabezadoServicio" TabIndex="13"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SitioCarga" HeaderText="Sitio Carga" SortExpression="SitioCarga" />
                                <asp:BoundField DataField="CitaCarga" HeaderText="Cita Carga" SortExpression="CitaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="SitioDescarga" HeaderText="Sitio Descarga" SortExpression="SitioDescarga" />
                                <asp:BoundField DataField="CitaDescarga" HeaderText="Cita Descarga" SortExpression="CitaDescarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Kms." SortExpression="Kilometraje">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbCalcularKilometraje" runat="server" Text='<%# Eval("Kilometraje")%>' OnClick="lkbServicio_OnClick" CommandName="Calcular" TabIndex="14"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbTerminarServicio" runat="server" Text="Terminar" OnClick="lkbServicio_OnClick" CommandName="Terminar" TabIndex="15"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbReabrirServicio" runat="server" Text="Reabrir" OnClick="lkbServicio_OnClick" CommandName="ReabrirServicio" TabIndex="16"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbCartaPorteViajera" runat="server" Text="Carta Porte Viajera" OnClick="lkbServicio_OnClick" CommandName="CartaPorteViajera" TabIndex="17"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbHojaInstrucciones" runat="server" Text="Hoja Instrucciones" OnClick="lkbServicio_OnClick" CommandName="HojaInstrucciones" TabIndex="18"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="ValidacionTerminar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbValidacionTerminar" runat="server" Visible="false" Text='<%#Eval("ValidacionTerminar") %>' TabIndex="18"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbImprimirCartaPorte" runat="server" Text="Imprimir Porte." OnClick="lkbServicio_OnClick" CommandName="CartaPorte" TabIndex="18"></asp:LinkButton>
                                        <asp:LinkButton ID="lkbBitacoraViaje" runat="server" Text="Bitacora Viaje" OnClick="lkbServicio_OnClick" CommandName="BitacoraViaje" TabIndex="19"></asp:LinkButton>
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
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoSevicios" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                        <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                        <asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarDocumentacion" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
                        <asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/EnTransito.png" />
            <h2>Seguimiento de Servicios</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta" style="width: auto">
                <label for="ddlTamanoParadas">
                    Mostrar:
                </label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoParadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label for="ddlTamanoParadas"></label>
                        <asp:DropDownList ID="ddlTamanoParadas" runat="server" OnSelectedIndexChanged="ddlTamanoParadas_OnSelectedIndexChanged" TabIndex="20" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarParadas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarParadas" runat="server" Text="Exportar Excel" OnClick="lkbExportarParadas_OnClick" TabIndex="21"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarParadas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_150px_altura">
            <asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvParadas" OnPageIndexChanging="gvParadas_PageIndexChanging" ShowFooter="True" OnRowDataBound="gvParadas_RowDataBound" runat="server" AutoGenerateColumns="False" AllowPaging="True" TabIndex="22"
                        ShowHeaderWhenEmpty="True"
                        CssClass="gridview" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:TemplateField HeaderText="" SortExpression="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbSeleccion" runat="server" Text="Selec." OnClick="lkbSeleccionParada_OnClick" TabIndex="23"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Secuencia" HeaderText="No." SortExpression="Secuencia" DataFormatString="{0:0}" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" ItemStyle-Width="250px">
                                <ItemStyle Width="250px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" />
                            <asp:TemplateField HeaderText="Fecha Llegada" SortExpression="FechaLlegada">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbFechaLlegada" runat="server" Text='<%#Eval("FechaLlegada") %>' OnClick="lkbFechaLlegada_OnClick" TabIndex="24"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Salida" SortExpression="FechaSalida">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbFechaSalida" runat="server" Text='<%#Eval("FechaSalida") %>' OnClick="lkbFechaSalida_OnClick" TabIndex="25"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" SortExpression="Eventos">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbEventos" runat="server" Text='<%#Eval("Eventos") %>' OnClick="lkbEventos_Click" TabIndex="26"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tractor" SortExpression="Unidad">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbTractor" runat="server" Text='<%#Eval("Unidad") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="27"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rem 1" SortExpression="Rem1">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbRem1" runat="server" Text='<%#Eval("Rem1") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="28"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rem 2" SortExpression="Rem2">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbRem2" runat="server" Text='<%#Eval("Rem2") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="29"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dolly" SortExpression="Dolly">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbDolly" runat="server" Text='<%#Eval("Dolly") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="30"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Operador" SortExpression="Operador">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbOperador" runat="server" Text='<%#Eval("Operador") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="31"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transportista" SortExpression="Trans">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbTrans" runat="server" Text='<%#Eval("Trans") %>' OnClick="lkbAsignacionRecursos_OnClick" TabIndex="32"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="--">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbBitacoraMonitoreo" runat="server" OnClick="imbBitacoraMonitoreo_Click"
                                        ImageUrl="~/Image/bitacora_monitoreo1.png" Width="22px" Height="22px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Kms" SortExpression="Kms">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbKilometrajeMov" runat="server" Text='<%#Eval("Kms") %>' OnClick="lkbKilometrajeMov_Click" TabIndex="33"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Devolución" SortExpression="Devolucion" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDevolucionMov" Text='<%#Eval("Devolucion") %>' runat="server" OnClick="lnkDevolucionMov_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbReferenciasParada" runat="server" ImageUrl="~/Image/Referencias.png" OnClick="imbReferenciasParada_Click" Width="20px" ToolTip="Referencias" TabIndex="34"></asp:ImageButton>
                                    </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" SortExpression="">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplkbBitacoraParada" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbBitacoraParada" runat="server" ToolTip="Bitácora" OnClick="lkbBitacoraParada_Click" TabIndex="35">
                                                <asp:Image ID="Bitacora" runat="server" ImageUrl="~/Image/Bitacora.png" Width="20" Height="20" /> 
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbBitacoraParada" />
                                        </Triggers>
                                    </asp:UpdatePanel>
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
                    <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoSevicios" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParada" />
                    <asp:AsyncPostBackTrigger ControlID="lkbReversaSalida" />
                    <asp:AsyncPostBackTrigger ControlID="lkbReversaLlegada" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParada" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParadaDocumento" />
                    <asp:AsyncPostBackTrigger ControlID="lkbDeshabilitaParada" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarDeshabilitaParadaDocumento" />
                    <asp:AsyncPostBackTrigger ControlID="ucKilometraje" EventName="ClickGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="ucAsignacionRecurso" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
                    <asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
                    <asp:AsyncPostBackTrigger ControlID="wucEncabezadoServicio" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarDocumentacion" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="renglon3x" style="width: auto">
                <asp:UpdatePanel ID="uplblErrorParada1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorParada" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbReversaSalida" />
                        <asp:AsyncPostBackTrigger ControlID="lkbReversaLlegada" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarDeshabilitaParadaDocumento" />
                        <asp:AsyncPostBackTrigger ControlID="lkbDeshabilitaParada" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscarServicios" />
                        <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL QUE PERMITE ASIGNAR RECURSOS AL SERVICIO -->
    <div id="contenidoRecursosAsignadosDespacho" class="modal">
        <div id="actualizacionRecursosAsignadosDespacho" class="contenedor_modal_asignacion_recursos">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucAsignacionRecurso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucAsignacionRecurso ID="ucAsignacionRecurso" runat="server" OnClickAgregarRecurso="ucAsignacionRecurso_ClickAgregarRecurso" OnClickLiberarRecurso="ucAsignacionRecurso_ClickLiberarRecurso"
                        OnClickQuitarRecurso="ucAsignacionRecurso_ClickQuitarRecurso" OnClickReubicarRecurso="ucAsignacionRecurso_ClickReubicarRecurso" Contenedor="#actualizacionRecursosAsignadosDespacho" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                    <asp:AsyncPostBackTrigger ControlID="wucReubicacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL DISPARADA AL DAR CLICK EN LA FECHA DE LLEGADA DE LA PARADA -->
    <div id="contenidoConfirmacionActualizacionLlegada" class="modal">
        <div id="confirmacionActualizacionLlegada" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Calendario.png" />
                <h2>
                    <asp:UpdatePanel ID="uplblTituloActualizacionLlegada" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            Actualización Fecha
                            <asp:Label ID="lblTituloActualizacionLlegada" runat="server">
                            </asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoLlegada" />
                        </Triggers>
                    </asp:UpdatePanel>
                </h2>
            </div>
            <asp:UpdatePanel ID="uppnlActualizacionFechaLlegada" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlActualizacionFechaLlegada" runat="server">
                        <div class="columna2x">
                            <div id="divOperador" runat="server" class="renglon2x">
                                <div class="etiqueta_50px">
                                    <label for="lblOp">Operador</label>
                                </div>
                                <div class="etiqueta_320px">
                                    <asp:Label ID="lblOp" runat="server" Text="----" CssClass="label_correcto"></asp:Label>
                                </div>
                            </div>
                            <div id="divTercero" runat="server" class="renglon2x">
                                <div class="etiqueta_50px">
                                    <label for="lblPr">Proveedor</label>
                                </div>
                                <div class="etiqueta_320px">
                                    <asp:Label ID="lblPr" runat="server" Text="----" CssClass="label_negrita"></asp:Label>
                                </div>
                                <div style="width: 471px; border-bottom: 1px solid #DDD;"></div>
                            </div>
                            <div id="divUnidades" runat="server" style="height: 55px; width: 475px;">
                                <div class="renglon2x">
                                    <div class="etiqueta" style="width: 120px;">
                                        <label for="lblU1">Unidad 1</label>
                                    </div>
                                    <div class="etiqueta" style="width: 120px;">
                                        <label for="lblU2">Unidad 2</label>
                                    </div>
                                    <div class="etiqueta" style="width: 120px;">
                                        <label for="lblU3">Unidad 3</label>
                                    </div>
                                </div>
                                <div class="renglon2x">
                                    <div class="etiqueta" style="width: 120px;">
                                        <asp:Label ID="lblU1" runat="server" Text="----" CssClass="label_error"></asp:Label>
                                    </div>
                                    <div class="etiqueta" style="width: 120px;">
                                        <asp:Label ID="lblU2" runat="server" Text="----" CssClass="label_error"></asp:Label>
                                    </div>
                                    <div class="etiqueta" style="width: 120px;">
                                        <asp:Label ID="lblU3" runat="server" Text="----" CssClass="label_error"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div style="width: 471px; border-bottom: 1px solid #DDD;"></div>
                            </div>
                            
                            <div class="renglon2x">
                                <asp:UpdatePanel ID="uplblMensajeLlegada" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblMensajeLlegada" CssClass="mensaje_modal" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoLlegada" />
                                        <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="renglon2x">
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplblValorFechaSalida" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblValorFechaSalida" runat="server" Text="Fecha Salida"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="uplblFechaSalida" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblFechaSalida" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="renglon2x">
                                <div class="etiqueta">
                                    <label for="lblCitaLlegada">Cita </label>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="uplblCitaLlegada" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblCitaLlegada" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="renglon2x">
                                <div class="etiqueta">
                                    <label for="txtFechaLlegada">Fecha LLegada</label>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="uptxtFechaLlegada" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtFechaLlegada" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                            <div class="renglon2x">
                                <div class="etiqueta">
                                    <asp:Label ID="lblRazonLlegadaTarde" runat="server">Razón Llegada Tarde</asp:Label>
                                </div>
                                <div class="control">
                                    <asp:UpdatePanel ID="upddlRazonLlegadaTarde" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlRazonLlegadaTarde" runat="server" TabIndex="35" CssClass="dropdown">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="renglon2x">
                                <div class="controlBoton">
                                    <asp:UpdatePanel ID="upbtnCancelarIngresoLlegada" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCancelarIngresoLlegada" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarIngresoLlegada_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="controlBoton">
                                    <asp:UpdatePanel ID="upbtnAceptarIngresoLlegada" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Button ID="btnAceptarIngresoLlegada" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarIngresoLlegada_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="renglon2x">
                                <div class="control2x">
                                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblErrorLlegada" runat="server" CssClass="label_error"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoLlegada" />
                                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoLlegada" />
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL DISPARADA AL DAR CLICK EN LA FECHA DE SALIDA DE LA PARADA -->
    <div id="contenidoConfirmacionActualizacionSalida" class="modal">
        <div id="confirmacionActualizacionSalida" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Calendario.png" />
                <h2>Actualización Fecha de Salida</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="uplblMensajeSalida" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMensajeSalida" CssClass="mensaje_modal" runat="server">Esta acción pondra en Tránsito al Servicio.</asp:Label>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="lblFechaLlegada">Fecha Llegada</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblFechaLlegada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblFechaLlegada" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="uptxtFechaSalida">Fecha Salida</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaSalida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaSalida" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <asp:Label ID="lblRazonSalidaTarde" runat="server">Razón Salida Tarde</asp:Label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlRazonSalidaTarde" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlRazonSalidaTarde" runat="server" CssClass="dropdown">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarIngresoSalida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarIngresoSalida" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarIngresoSalida_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarIngresoSalida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarIngresoSalida" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarIngresoSalida_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="control2x">
                        <asp:UpdatePanel ID="uplblErrorSalida" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblErrorSalida" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarIngresoSalida" />
                                <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL QUE INDICA LA EXISTENCIA DE EVENTOS PENDIENTES EN PARADA -->
    <div id="contenidoConfirmacionActualizacionEventos" class="modal">
        <div id="confirmacionConfirmacionActualizacionEventos" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Aceptar.png" />
                <h2>Actualización Eventos Pendientes</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <asp:UpdatePanel ID="uplblMensajeEventos" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMensajeEventos" runat="server" CssClass="mensaje_modal"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarIndicadorVencimientos" />
                            <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarActualizacionEventos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarActualizacionEventos" runat="server" OnClick="btnCancelarActualizacionEventos_Click" CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnNoActualizacionEventos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnNoActualizacionEventos" runat="server" OnClick="btnNoActualizacionEventos_Click" CommandName="Parada" CssClass="boton_cancelar" Text="No" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnSiActualizacionEventos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnSiActualizacionEventos" runat="server" OnClick="btnSiActualizacionEventos_Click" CssClass="boton" Text="Si" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarIngresoSalida" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL QUE PERMITE LA ACTUALIZACION DE EVENTOS PENDIENTES -->
    <div id="contenidoActualizacionEventos" class="modal">
        <div id="actualizacionEventos" class="contenedor_modal_seccion_completa">
            <div class="header_seccion">
                <img src="../Image/Descarga.png" />
                <h2>Eventos Pendientes</h2>
            </div>
            <div class="contenedor_seccion_95per">
                <div class="renglon100Per">
                    <div class="etiqueta" style="width: auto">
                        <label for="ddlTamanoEventos">
                            Mostrar:
                        </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoEventos" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <label for="ddlTamanoEventos"></label>
                                <asp:DropDownList ID="ddlTamanoEventos" runat="server" OnSelectedIndexChanged="ddlTamanoEventos_OnSelectedIndexChanged" TabIndex="36" AutoPostBack="true" CssClass="dropdown">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel runat="server" ID="uplkbExportarEventos" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbExportarEventos" runat="server" Text="Exportar Excel" OnClick="lkbExportarEventos_OnClick" TabIndex="37"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lkbExportarEventos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_100px_altura">
                    <asp:UpdatePanel ID="upgvEventos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvEventos" runat="server" AutoGenerateColumns="false" CssClass="gridview" OnPageIndexChanging="gvEventos_PageIndexChanging" AllowSorting="false" AllowPaging="true"
                                PageSize="5" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Secuencia" HeaderText="No." SortExpression="Secuencia" DataFormatString="{0:0}" />
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                    <asp:BoundField DataField="CitaEvento" HeaderText="Cita Evento" SortExpression="CitaEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="InicioEvento" HeaderText="Inicio Evento" SortExpression="InicioEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="TipoActualizacionInicio" HeaderText="Tipo Actualizacion Inicio" SortExpression="TipoActualizacionInicio" />
                                    <asp:BoundField DataField="FinEvento" HeaderText="Fin Evento" SortExpression="FinEvento" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="TipoActualizacionFin" HeaderText="Tipo Actualizacion Fin" SortExpression="TipoActualizacionFin" />
                                    <asp:BoundField DataField="MotivoRetraso" HeaderText="Motivo Retraso" SortExpression="MotivoRetraso" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbSeleccionar" runat="server" Text="Seleccionar" OnClick="lkbEvento_Click" CommandName="Seleccionar" TabIndex="38"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbEvento_Click" CommandName="Eliminar" TabIndex="39"></asp:LinkButton>
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
                            <asp:AsyncPostBackTrigger ControlID="btnSiActualizacionEventos" />
                            <asp:AsyncPostBackTrigger ControlID="btnModificarEvento" />
                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoEventos" />
                            <asp:AsyncPostBackTrigger ControlID="btnSiActualizacionEventos" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                            <asp:AsyncPostBackTrigger ControlID="btnInsertarEvento" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="seccion_controles">
                <div class="columna3x">
                    <div class="renglon3x">
                        <div class="etiqueta">
                            <label for="txtFechaInicioEvento">Fecha Inicio</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtFechaInicioEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFechaInicioEvento" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="40"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnModificarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="etiqueta">
                            <label for="txtFechaFinEvento">Fecha Fin</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtFechaFinEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFechaFinEvento" runat="server" CssClass="textbox validate[custom[dateTime24]]" TabIndex="41"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnModificarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon3x">
                        <div class="etiqueta">
                            <label for="ddlTipoEvento">Tipo Evento</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="upddlTipoEventos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTipoEventos" runat="server" CssClass="dropdown" TabIndex="42"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSiActualizacionEventos" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="etiqueta">
                            <label for="ddlMotivoRetraso">Motivo Retraso</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="upddlMotivoRetraso" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlMotivoRetraso" runat="server" CssClass="dropdown" TabIndex="43"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnModificarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x" style="width: auto">
                        <div class="control" style="width: auto">
                            <asp:UpdatePanel ID="uplblErrorEventos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorEventos" runat="server" CssClass="label_error"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnModificarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="gvEventos" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCerrarEvento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnInsertarEvento" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelarEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelarEvento" runat="server" Text="Cancelar"
                                        CssClass="boton_cancelar" TabIndex="44" OnClick="btnCancelarEvento_Click" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCerrarEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCerrarEvento" runat="server" OnClick="btnCerrarEventos_Click" Text="Cerrar"
                                        CssClass="boton" TabIndex="45" CommandName="Parada" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSiActualizacionEventos" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnModificarEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnModificarEvento" runat="server" OnClick="btnModificarEvento_Click" Text="Modificar Actual"
                                        CssClass="boton" TabIndex="46" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnInsertarEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnInsertarEvento" runat="server" OnClick="btnInsertarEvento_Click" Text="Insertar Evento"
                                        CssClass="boton" TabIndex="47" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE PERMITE DAR DE ALTA NUEVAS PARADAS AL SERVICIO -->
    <div id="contenidoInsertaParada" class="modal">
        <div id="confirmacionInsertaParada" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/ImagenPatio.png" />
                <h2>Insertar Nueva Parada</h2>
            </div>
            <div class="seccion_controles">
                <div class="columna2x">
                    <div class="renglon2x"></div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="ddlTipoParada">Tipo </label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="upddlTipoParada" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTipoParada" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoParada_OnSelectedIndexChanged" TabIndex="48"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbInsertaParada" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="ddlTipoEvento">Tipo Evento</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="upddlTipoEvento" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="dropdown" TabIndex="49"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoParada" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbInsertaParada" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="txtUbicacionP">Lugar</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtUbicacionP" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtUbicacionP" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="50"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoParada" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbInsertaParada" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="etiqueta">
                            <label for="txtCita">Cita</label>
                        </div>
                        <div class="control">
                            <asp:UpdatePanel ID="uptxtCita" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCita" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="51"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoParada" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbInsertaParada" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="control" style="width: auto">
                            <asp:UpdatePanel ID="uplblErrorParada" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblErrorInsertaParada" runat="server" CssClass="label_error"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbInsertaParada" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParadaDocumento" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParadaDocumento" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="renglon2x">
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnCancelarInsertaParada" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelarInsertaParada" runat="server" Text="Cancelar" OnClick="btnCancelarInsertaParada_Click"
                                        CssClass="boton_cancelar" TabIndex="52" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="controlBoton">
                            <asp:UpdatePanel ID="upbtnAceptarInsertaParada" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnAceptarInsertaParada" runat="server" CssClass="boton" OnClick="btnAceptarInsertaParada_Click" Text="Aceptar" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTipoParada" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE DE LA TERMINACION DEL SERVICIO -->
    <div id="contenidoFechaTerminoServicio" class="modal">
        <div id="confirmacionFechaTerminoServicio" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaTerminoServicio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaTerminoServicio" runat="server" Text="Cerrar" OnClick="lkbCerrarTerminoServicio_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Terminar.png" />
                <h2>Terminación de Servicio</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCita">Fecha de Término</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaTermino" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaTermino" runat="server" Enabled="true" CssClass="textbox validate[custom[dateTime24]]" TabIndex="53"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarFechaTerminoServicio" />
                                <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="control" style="width: auto">
                        <asp:UpdatePanel ID="uplblErrorTerminoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblErrorTerminoServicio" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnNoActualizacionEventos" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminoServicio" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarFechaTerminoServicio" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarTerminoServicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarTerminoServicio" runat="server" CssClass="boton" OnClick="btnAceptarTerminoServicio_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA DATOS RELEVANTES DE DOCUMENTACION  -->
    <div id="modalDocumentacion" class="modal">
        <div id="contenidoDocumentacion" class="contenedor_modal_asignacion_recursos">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarDocumentacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarDocumentacion" runat="server" Text="Cerrar" OnClick="lkbCerrarDocumentacion_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upmtvDocumentacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:MultiView ID="mtvDocumentacion" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vwClasificacion" runat="server">
                            <div class="header_seccion">
                                <img src="../Image/Clasificacion.png" />
                                <h2>Clasificación</h2>
                            </div>
                            <div class="seccion_controles">
                                <asp:UpdatePanel ID="upwucClasificacion" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <tectos:wucClasificacion runat="server" ID="wucClasificacion" OnClickGuardar="wucClasificacion_ClickGuardar" OnClickCancelar="wucClasificacion_ClickCancelar" TabIndex="54" />
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                        <asp:View ID="vwReferencias" runat="server">
                            <div class="header_seccion">
                                <img src="../Image/Clasificacion.png" />
                                <h2>Referencias Servicio</h2>
                            </div>
                            <asp:UpdatePanel ID="upwucReferenciaViaje" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <tectos:wucReferenciaViaje ID="wucReferenciaViaje" runat="server" Enable="true" TabIndex="55"
                                        OnClickGuardarReferenciaViaje="wucReferenciaViaje_ClickGuardarReferenciaViaje"
                                        OnClickEliminarReferenciaViaje="wucReferenciaViaje_ClickEliminarReferenciaViaje" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                            <br />
                        </asp:View>
                        <asp:View ID="vwProducto" runat="server">
                            <h3>Producto 
                            </h3>
                            <asp:UpdatePanel ID="upwucProducto" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <tectos:wucProducto ID="wucProducto" runat="server" OnClickGuardarProducto="wucProducto_ClickGuardarProducto"
                                        OnClickEliminarProducto="wucProducto_ClickEliminarProducto" TabIndex="56" Contenedor="#contenidoDocumentacion" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:View>
                        <asp:View ID="vwCargos" runat="server">
                            <h3>Cargos del Servicio</h3>
                            <asp:UpdatePanel ID="upwucFacturadoConcepto" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <tectos:wucFacturadoConcepto runat="server" ID="wucFacturadoConcepto" OnClickGuardarFacturaConcepto="wucFacturadoConcepto_ClickGuardarFacturaConcepto" OnClickEliminarFacturaConcepto="wucFacturadoConcepto_ClickEliminarFacturaConcepto" TabIndex="57" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbReferenciaViaje" />
                    <asp:AsyncPostBackTrigger ControlID="lkbClasificacion" />
                    <asp:AsyncPostBackTrigger ControlID="lkbProducto" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCargos" />
                    <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    <asp:AsyncPostBackTrigger ControlID="wucDevolucionFaltante" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA DIALOGO DE TÉRMINO DE MOVS. VACIOS  -->
    <div id="contenidoConfirmacionTerminoMovimientoVacio" class="modal">
        <div id="confirmacionTerminoMovimientoVacio" class="contenedor_modal_seccion_completa_arriba" style="height: 635px; top: 10px; width: 1200px">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarTerminoMovimiento" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarTerminoMovimiento" runat="server" Text="Cerrar" OnClick="lkbCerrarTerminoMovimiento_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="upWucTerminoMovimientoVacio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <operacion:wucTerminoMovimientoVacio ID="WucTerminoMovimientoVacio" runat="server" TabIndex="58" Contenedor="#confirmacionTerminoMovimientoVacio" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbTerminoMovimientoVacio" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE MUESTRA LOS VENCIMIENTOS ACTIVOS -->
    <div id="contenedorVentanaKilometraje" class="modal">
        <div id="ventanaKilometraje" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrar" OnClick="lkbCerrar_Click" runat="server" TabIndex="59">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucKilometraje" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucKilometraje ID="ucKilometraje" runat="server" OnClickGuardar="ucKilometraje_ClickGuardar"
                        TabIndex="11" Contenedor="#ventanaKilometraje" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                </Triggers>
            </asp:UpdatePanel>
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
                        <asp:GridView ID="gvVencimientos" runat="server" AutoGenerateColumns="False" PageSize="10"
                            ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvVencimientos_PageIndexChanging" AllowPaging="True" OnRowDataBound="gvVencimientos_RowDataBound">
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
    <!-- VENTANA MODAL QUE INDICA LA EXISTENCIA DE CONTROL DE EVIEDNCIAS ACTIVOS INSERCCIÓN -->
    <div id="modalIndicadorInsertaParadaDocumento" class="modal">
        <div id="contenidoModalIndicadorInsertaParadaDocumentos" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <asp:UpdatePanel ID="upModalIndicadorInsertaParadaDocumentos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <img src="../Image/Exclamacion.png" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3>¡Control Evidencia!</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="uplblMensajeInsertaParadaDocumento" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMensajeInsertaParadaDocumento" runat="server" CssClass="mensaje_modal"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarInsertaParada" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarInsertaParadaDocumento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarInsertaParadaDocumento" runat="server" OnClick="btnAceptarInsertaParadaDocumento_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarInsertaParadaDocumento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarInsertaParadaDocumento" runat="server" OnClick="btnCancelarInsertaParadaDocumento_Click" CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE INDICA LA EXISTENCIA DE CONTROL DE EVIEDNCIAS ACTIVOS DESHABILITACIÓN  -->
    <div id="modalIndicadorDeshabilitaParadaDocumento" class="modal">
        <div id="contenidoModalIndicadorDeshabilitaParadaDocumentos" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <asp:UpdatePanel ID="upcontenidoModalIndicadorDeshabilitaParadaDocumento" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <img src="../Image/Exclamacion.png" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3>¡Control Evidencia!</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="uplblMensajeDeshabilitaParadaDocumento" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMensajeDeshabilitaParadaDocumento" runat="server" CssClass="mensaje_modal"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbDeshabilitaParada" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarDeshabilitaParadaDocumento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarDeshabilitaParadaDocumento" OnClick="btnAceptarDeshabilitaParadaDocumento_Click" runat="server" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarDeshabilitaParadaDocumento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarDeshabilitaParadaDocumento" runat="server" OnClick="btnCancelarDeshabilitaParadaDocumento_Click" CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL QUE PERMITE REALIZAR REASIGNACION DE UBICACION A UNIDADES -->
    <div id="contenidoConfirmacionUbicacion" class="modal">
        <div id="confirmacionUbicacion" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/EntradasSalidas.png" />
                <h2>Reubicación de Unidades</h2>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="upwucReubicacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:wucMovimientoVacioSinOrden ID="wucReubicacion" runat="server" OnClickRegistrar="wucReubicacion_OnClickRegistrar"
                            OnClickCancelar="wucReubicacion_OnClickCancelar" Contenedor="#contenidoConfirmacionUbicacion" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ucAsignacionRecurso" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL QUE PERMITE REALIZAR  EL CALCULO DE RUTA-->
    <div id="contenidoCalcularRuta" class="modal">
        <div id="confirmacionCalcularRuta" class="contenedor_modal_seccion_completa_arriba" style="width: 1230px">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCalcularRuta" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCalcularRuta" runat="server" CommandName="calcularRuta" OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="60">
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
                        <asp:AsyncPostBackTrigger ControlID="lkbCalcular" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL DE ACTUALIZACIÓN DE ENCABEZADO DE SERVICIO -->
    <div id="encabezadoServicioModal" class="modal">
        <div id="encabezadoServicio" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarEncabezadoServicio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarEncabezadoServicio" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="EncabezadoServicio">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucReferenciaServicio" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucEncabezadoServicio ID="wucEncabezadoServicio" runat="server"
                        OnClickGuardarReferencia="wucEncabezadoServicio_ClickGuardarReferencia" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Devolución Faltante -->
    <div id="modalDevolucionFaltante" class="modal">
        <div id="devolucionFaltante" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarDevolucion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarDevolucion" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Devolucion" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucDevolucionFaltante" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc:wucDevolucionFaltante ID="wucDevolucionFaltante" runat="server" OnClickGuardarDevolucion="wucDevolucionFaltante_ClickGuardarDevolucion"
                        OnClickGuardarDevolucionDetalle="wucDevolucionFaltante_ClickGuardarDevolucionDetalle"
                        OnClickEliminarDevolucionDetalle="wucDevolucionFaltante_ClickEliminarDevolucionDetalle"
                        OnClickAgregarReferenciasDevolucion="wucDevolucionFaltante_ClickAgregarReferenciasDevolucion"
                        OnClickAgregarReferenciasDetalle="wucDevolucionFaltante_ClickAgregarReferenciasDetalle" Contenedor="#devolucionFaltante" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                    <asp:AsyncPostBackTrigger ControlID="wucReferenciaViaje" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarEncabezadoServicio" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana de Recursos para Bitacora -->
    <div id="contenedorVentanaRecursosMovimiento" class="modal">
        <div id="ventanaRecursosMovimiento" class="contenedor_ventana_confirmacion" style="width: 484px; min-width: 484px">
            <div class="columna2x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel ID="uplnkCerrarRecursosMov" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkCerrarRecursosMov" runat="server" OnClick="lnkCerrarVentanaModal_Click"
                                Text="Cerrar" CommandName="RecursosBitacora">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/bitacora_monitoreo1.png" />
                    <h2>Seleccione la Entidad a la que quiere dar seguimiento</h2>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvRecursosAsignados" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvRecursosAsignados" runat="server" AutoGenerateColumns="False"
                                ShowFooter="True" CssClass="gridview" AllowPaging="false" AllowSorting="false" Width="100%"
                                OnRowDataBound="gvRecursosAsignados_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo" SortExpression="Asignacion">
                                        <HeaderStyle Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgAsignacion" runat="server" ImageUrl="~/Image/operador2.png"
                                                Width="22px" Height="22px" ToolTip='<%# Eval("Asignacion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EstatusAsignacion" HeaderText="Estatus" SortExpression="EstatusAsignacion" ItemStyle-Width="70px" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                                    <asp:TemplateField>
                                        <HeaderStyle Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbNvaBitacora" runat="server" ImageUrl="~/Image/bitacora_monitoreo2.png"
                                                Width="22px" Height="22px" CommandName="Bitacora" ToolTip="Agregar Bitacora de Viaje"
                                                OnClick="imbSeleccionar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="40px" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbHistorialBit" runat="server" ImageUrl="~/Image/transportista3.png"
                                                Width="22px" Height="22px" CommandName="HistorialBitacora" ToolTip="Ver Historial de Bitacoras"
                                                OnClick="imbSeleccionar_Click" />
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
                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL DE HISTORIAL DE BITÁCORA DE MONITOREO -->
    <div id="historialBitacoraModal" class="modal">
        <div id="historialBitacora" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialBitacora" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarHistorialBitacora" runat="server" OnClick="lnkCerrarVentanaModal_Click" CommandName="HistorialBitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div>
                <asp:UpdatePanel ID="upwucBitacoraMonitoreoHistorial" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:wucBitacoraMonitoreoHistorial runat="server" ID="wucBitacoraMonitoreoHistorial" OnbtnNuevoBitacora="wucBitacoraMonitoreoHistorial_btnNuevoBitacora" OnlkbConsultar="wucBitacoraMonitoreoHistorial_lkbConsultar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreo" />
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL DE EDICIÓN Y CAPTURA DE BITÁCORA DE MONITOREO -->
    <div id="bitacoraMonitoreoModal" class="modal">
        <div id="bitacoraMonitoreo" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarBitacoraMonitoreo" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarBitacoraMonitoreo" runat="server" OnClick="lnkCerrarVentanaModal_Click" CommandName="Bitacora" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna">
                <asp:UpdatePanel ID="upwucBitacoraMonitoreo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:wucBitacoraMonitoreo runat="server" ID="wucBitacoraMonitoreo" OnClickRegistrar="wucBitacoraMonitoreo_ClickRegistrar"
                            OnClickEliminar="wucBitacoraMonitoreo_ClickEliminar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="wucBitacoraMonitoreoHistorial" />
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                        <asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Ventana de Impresión -->
    <div id="contenedorVentanaImpresionPorte" class="modal">
        <div id="ventanaImpresionPorte" class="contenedor_ventana_confirmacion" style="width: auto;">
            <div class="columna2x">
                <asp:UpdatePanel ID="upwucImpresionPorte" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:wucImpresionPorte ID="wucImpresionPorte" runat="server" OnClickImprimirCartaPorte="wucImpresionPorte_ClickImprimirCartaPorte" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvServicios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL EDITAR SOPORTE TECNICO AGREGAR DETALLE -->
    <div id="despachoModalMTC" class="modal">
        <div id="despachoMTC" class="contenedor_ventana_confirmacion_arriba" style="min-width: 500px; padding-bottom: 20px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarDespachoMTC" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarDespachoMTC" runat="server" OnClick="lnkCerrarVentanaModal_Click" CommandName="DespachoMTC" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna">
                <asp:UpdatePanel ID="upwucdespachoMTC" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!--Formulario de ventana modal-->

                        <div class="header_seccion">
                            <img src="../Image/OperacionPatio.png" width="32" height="32" />
                            <h2>Busca Despacho MTC</h2>
                        </div>
                        <div class="columna">
                            <div class="renglon2x">
                                <div class="etiqueta_80px">
                                    <label class="txtFolioDespacho">Folio Despacho:</label>
                                </div>
                                <asp:UpdatePanel ID="uplblfolio" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblfolio" runat="server" CssClass="label"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lkbDespachoMTC" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="columna">
                            <div class="renglon2x">
                                <div class="etiqueta_80px">
                                    <label class="txtfechaInicio">Fecha Inicio:</label>
                                </div>
                                <asp:UpdatePanel ID="uplblfecha" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblfecha" runat="server" CssClass="label"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lkbDespachoMTC" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="columna">
                            <div class="renglon2x">
                                <div class="etiqueta_80px">
                                    <label class="txtTEA">TEA:</label>
                                </div>
                                <asp:UpdatePanel ID="uplblTEA" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTEA" runat="server" CssClass="label"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lkbDespachoMTC" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <div class="renglon2x">
                                <div class="etiqueta_80px">
                                    <label class="txtTEAFinal">TEA Final:</label>
                                </div>
                                <asp:UpdatePanel ID="uplblTEAFinal" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblTEAFinal" runat="server" CssClass="label"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lkbDespachoMTC" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <!--fin de ventana modal -->
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


</asp:Content>
