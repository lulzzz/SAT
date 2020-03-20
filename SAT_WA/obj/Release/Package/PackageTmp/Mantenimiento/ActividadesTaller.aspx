<%@ Page Title="Actividades de Taller" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ActividadesTaller.aspx.cs" Inherits="SAT.Mantenimiento.ActividadesTaller" %>
<%@ Register  Src="~/UserControls/ActividadOrdenTrabajo.ascx"  TagPrefix="tectos" TagName="wucActividadOrdenTrabajo" %>
<%@ Register Src="~/UserControls/wucAsignacionActividad.ascx" TagName="wucAsignacion" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucRequisicion.ascx" TagName="wucRequisicion" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
    <link href="../CSS/Forma.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
    <!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlActividades();
        }
    }
    //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQueryControlActividades() {
        $(document).ready(function () {
            //Función de validación de campos
            var validacionActividades = function (evt) {
                var isValidP1 = !$("#<%=txtNoOrden.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
                return isValidP1 && isValidP2
            };
            //Función de validación de campos
            var validacionFechaInicio = function (evt) {
                var isValidP1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                return isValidP1
            };
            //Función de validación de campos
            var validacionFechaFin = function (evt) {
                var isValidP1 = !$("#<%=txtFechaTerminar.ClientID%>").validationEngine('validate');
                return isValidP1
            };
            //Función de validación de campos
            var validacionFechaInicioDirecto = function (evt) {
                var isValidP1 = !$("#<%=txtFechaInicioDirecto.ClientID%>").validationEngine('validate');
                return isValidP1
            };
            //Función de validación de campos
            var validacionFechaFinDirecto = function (evt) {
                var isValidP1 = !$("#<%=txtFechaTerminarDirecto.ClientID%>").validationEngine('validate');
                return isValidP1
            };
            //Función de validación de campos
            var validacionFechaTerminoActividad = function (evt) {
                var isValidP1 = !$("#<%=txtFechaTerminarActividad.ClientID%>").validationEngine('validate');
                return isValidP1
            };
            //Función de validación de campos (Almacen)
            var validacionTallerIngresado = function (evt) {
                var isValidP1 = !$("#<%=txtAlmacen.ClientID%>").validationEngine('validate');
                return isValidP1;
            };
            //Función de validación de campos (Sin Copia de Req)
            var validacionAlmacenReq = function (evt) {
                var isValidP1 = !$("#<%=txtAlmacenReq.ClientID%>").validationEngine('validate');
                return isValidP1;
            };
            //Función de validación de Fechas de Orden de Trabajo
            var validacionFechasOrdenTrabajo = function (evt) {
                var isValidP1 = !$("#<%=txtFechaInicioOrdenTrabajo.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=txtFechaFinOrdenTrabajo.ClientID%>").validationEngine('validate');
                return isValidP1 && isValidP2;
            };

            //Boton Guardar
            $("#<%=btnBuscar.ClientID%>").click(validacionActividades);
            //Boton Iniciar Asignacion
            $("#<%=btnAceptarInicio.ClientID%>").click(validacionFechaInicio);
            //Boton Terminar Asignacion
            $("#<%=btnAceptarTerminar.ClientID%>").click(validacionFechaFin);
            //Boton Iniciar Asignacion
            $("#<%=btnAceptarInicioDirecto.ClientID%>").click(validacionFechaInicioDirecto);
            //Boton Terminar Asignacion
            $("#<%=btnAceptarTerminarDirecto.ClientID%>").click(validacionFechaFinDirecto);
            //Boton Guardar
            $("#<%=btnTerminarActividad.ClientID%>").click(validacionFechaTerminoActividad);
            //Boton Guardar Almacen
            $("#<%=btnGuardarReq.ClientID%>").click(validacionTallerIngresado);
            //Boton Guardar Almacen
            $("#<%=btnGuardarAlmacenReq.ClientID%>").click(validacionAlmacenReq);
            //Boton Buscar Actividades
            $("#<%=btnBuscar.ClientID%>").click(validacionFechasOrdenTrabajo);

            //Controles de Fecha
            $("#<%=txtFechaTerminar.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaTerminarDirecto.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaInicioDirecto.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaTerminarActividad.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaInicioOrdenTrabajo.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaFinOrdenTrabajo.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Añadiendo Encabezado Fijo
            $("#<%=gvActividades.ClientID%>").gridviewScroll({
                width: document.getElementById("contenedorActividades").offsetWidth - 15,
                height: 400,
                freezesize: 2
            });
            $("#<%=txtAlmacen.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                appendTo: "#ventanaIngresoAlmacen"
            });
            $("#<%=txtAlmacenReq.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=32&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                appendTo: "#ventanaIngresoAlmacenReq"
            });
            
        });
    }
    //Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryControlActividades();
</script>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/AgregarRegistro.png" />
            <h2>Asignación de Actividades</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoOrden">No Orden</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoOrden" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoOrden" runat="server" CssClass="textbox  validate[custom[onlyNumberSp]] " TabIndex="1" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUnidad">Unidad</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox" TabIndex="2" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlArea">Área </label>
                    &nbsp;
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlArea" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" CssClass="dropdown" TabIndex="3"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlSubArea">Sub Área</label>
                </div>
                <asp:UpdatePanel ID="upddlSubArea" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlSubArea" runat="server" CssClass="dropdown" TabIndex="4">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlArea" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <label class="label_negrita">Filtros de Estatus de la Orden </label>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <asp:CheckBox ID="chkActiva" runat="server" Text="Activa" TabIndex="5" />
                </div>
                <div class="etiqueta">
                    <asp:CheckBox ID="chkPausada" runat="server" Text="Pausada" TabIndex="6" />
                </div>
                <div class="etiqueta_155px">
                    <asp:CheckBox ID="chkTerminada" runat="server" AutoPostBack="true" OnCheckedChanged="chkTerminada_CheckedChanged" Text="Terminada" TabIndex="7" />
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaInicioOrdenTrabajo">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicioOrdenTrabajo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicioOrdenTrabajo" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="2" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkTerminada" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaFinOrdenTrabajo">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFinOrdenTrabajo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFinOrdenTrabajo" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="9" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkTerminada" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" TabIndex="14" OnClick="btnBuscar_Click" />
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/Documento.png" />
            <h2>Actividades</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta_50px">
                <label for="ddlTamañoGridViewActividades">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamañoGridViewActividades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamañoGridViewActividades" runat="server" OnSelectedIndexChanged="gvActividades_OnSelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenarServicios">Ordenado Por:</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblCriterioGridViewActividades" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterioGridViewActividades" runat="server" CssClass="label_negrita"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvActividades" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel runat="server" ID="uplkbExportarExcelActividades" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarExcelActividades" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelActividades_Onclick"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarExcelActividades" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div id="contenedorActividades" class="grid_seccion_completa_altura_variable">
            <asp:UpdatePanel ID="upgvActividades" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvActividades" CssClass="gridview" OnPageIndexChanging="gvActividades_OnpageIndexChanging" OnSorting="gvActividades_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                        ShowFooter="True"
                        PageSize="25" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Orden" SortExpression="Orden">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrden" runat="server" Text='<%# Eval("Orden") %>' OnClick="lnkOrden_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EstatusOrden" HeaderText="Estatus" SortExpression="EstatusOrden" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="TipoUnidad" HeaderText="Tipo Unidad" SortExpression="TipoUnidad" />
                            <asp:BoundField DataField="SubTipoUnidad" HeaderText="Subtipo Unidad" SortExpression="SubTipoUnidad" />
                            <asp:BoundField DataField="Area" HeaderText="Área" SortExpression="Area" />
                            <asp:BoundField DataField="SubArea" HeaderText="Sub Área" SortExpression="SubArea" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" SortExpression="FechaInicio" />
                            <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" DataFormatString="{0:dd/MM/yyyy HH:mm}" SortExpression="FechaFin" />
                            <asp:TemplateField HeaderText="Duración" SortExpression="Duracion">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuracion" runat="server" Text='<%# TSDK.Base.Cadena.ConvierteMinutosACadena(Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(Eval("Duracion").ToString(), "0"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Duración Aprox." SortExpression="DuracionActividad">
                                <ItemTemplate>
                                    <asp:Label ID="lblDuracionActividad" runat="server" Text='<%# TSDK.Base.Cadena.ConvierteMinutosACadena(Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(Eval("DuracionActividad").ToString(), "0"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Asignación" SortExpression="Asignación">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAsignacion" runat="server" Text="Asignar" OnClick="lkbActividades_Click" CommandName="Asignacion"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requisición" SortExpression="Requisición">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAgregar" runat="server" Text="Requerir" OnClick="lkbRequisiciones_Click" CommandName="Agregar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actividad" SortExpression="Actividad">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAgregarAc" runat="server" Text="Agregar" CommandName="AgregarActividad" OnClick="lkbActividades_Click"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbCancelar" runat="server" Text="Cancelar" CommandName="Cancelar" OnClick="lkbActividades_Click"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbTerminar" runat="server" Text="Terminar" CommandName="Terminar" OnClick="lkbActividades_Click"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbAbrirActividad" runat="server" Text="Abrir Actividad" CommandName="AbrirActividad" OnClick="lkbActividades_Click"></asp:LinkButton><br />
                                    <asp:LinkButton ID="lkbDeshabilitar" runat="server" Text="Deshabilitar" CommandName="Eliminar" OnClick="lkbActividades_Click"></asp:LinkButton>
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
                    <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewActividades" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInicio" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminar" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarAsignacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarActividad" />
                    <asp:AsyncPostBackTrigger ControlID="btnTerminarActividad" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarActividad" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrarImagen" />
                    <asp:AsyncPostBackTrigger ControlID="ucActividadOrdenTrabajo" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarAbrirActividad" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarInicioDirecto" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="renglon2x"></div>
    <div class="columna3x">
        <div class="contenedor_seccion_completa">
            <div class="header_seccion">
                <h2>Asignaciones</h2>
            </div>
            <div class="renglon3x">
                <div class="etiqueta_50px">
                    <label for="ddlTamañoGridViewAsignaciones">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamañoGridViewAsignaciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamañoGridViewAsignaciones" runat="server" OnSelectedIndexChanged="gvAsignaciones_OnSelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown_100px">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenarServicios">Ordenado Por:</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblCriterioGridViewAsignaciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblCriterioGridViewAsignaciones" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel runat="server" ID="uplkbExportarExcelAsignaciones" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbExportarExcelAsignaciones" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelAsignaciones_Onclick"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lkbExportarExcelAsignaciones" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa">
                <asp:UpdatePanel ID="upgvAsignaciones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvAsignaciones" CssClass="gridview" OnPageIndexChanging="gvAsignaciones_OnpageIndexChanging" OnSorting="gvAsignaciones_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                            ShowFooter="True"
                            PageSize="5" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" />
                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                <asp:TemplateField HeaderText="Responsable" SortExpression="Responsable">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbResponsable" runat="server" Text='<%# Eval("Responsable") %>' OnClick="lkbResponsable_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Inicio" HeaderText="Inicio" SortExpression="Inicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="Fin" HeaderText="Fin" SortExpression="Fin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Duración" SortExpression="Duracion">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDuracion" runat="server" Text='<%# TSDK.Base.Cadena.ConvierteMinutosACadena(Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(Eval("Duracion").ToString(), "0"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbOperaciones" runat="server" CommandName='<%#Eval("Estatus").ToString() =="Asignada" ? "Iniciar" : Eval("Estatus").ToString() == "Terminada" ? " ": Eval("Estatus").ToString() == "Registrada" ? " ": "Terminar"%>' Text='<%#Eval("Estatus").ToString() =="Asignada" ? "Iniciar" : Eval("Estatus").ToString() == "Terminada" ? " ": Eval("Estatus").ToString() == "Registrada" ? " ": "Terminar"%>' OnClick="lkbOperaciones_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbCancelarAsignacion" runat="server" Text='<%#Eval("Estatus").ToString() =="Asignada" ? "Cancelar" : " "%>' OnClick="lkbCancelarAsignacion_Click"></asp:LinkButton>
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
                        <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewAsignaciones" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarInicio" />
                        <asp:AsyncPostBackTrigger ControlID="wucAsignacionActividad" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarAsignacion" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarAsigancion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="upmtvRequisicion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="mtvRequisicion" runat="server" ActiveViewIndex="0">
                <asp:View ID="vwEncabezado" runat="server">
                    <div class="columna3x">
                        <div class="contenedor_seccion_completa">
                            <div class="header_seccion">
                                <h2>Requisición</h2>
                            </div>
                            <div class="renglon3x">
                                <div class="etiqueta_50px">
                                    <label for="ddlTamañoGridViewRequisicion">Mostrar</label>
                                </div>
                                <div class="control_100px">
                                    <asp:UpdatePanel ID="upddlTamañoGridViewRequisicion" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTamañoGridViewRequisicion" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewRequisicion_SelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown_100px">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <label for="lblOrdenarServicios">Ordenado Por:</label>
                                </div>
                                <div class="etiqueta_155px">
                                    <asp:UpdatePanel ID="uplblCriterioGridViewRequisicion" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblCriterioGridViewRequisicion" runat="server" CssClass="label_negrita"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvRequisicion" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta_50px">
                                    <asp:UpdatePanel runat="server" ID="uplkbExportarExcelRequisicion" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbExportarExcelRequisicion" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelRequisicion_Onclick"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbExportarExcelRequisicion" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta_50px">
                                    <asp:UpdatePanel runat="server" ID="uplkbNuevaRequisicion" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkNuevaRequisicion" Text="Requisición" OnClick="lkbNuevaRequisicion_Click" runat="server">
                                            </asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="grid_seccion_completa">
                                <asp:UpdatePanel ID="upgvRequisicion" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvRequisicion" CssClass="gridview" OnPageIndexChanging="gvRequisicion_OnpageIndexChanging"
                                            OnSorting="gvRequisicion_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            ShowFooter="True" PageSize="5" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                                <asp:BoundField DataField="NoRequisicion" HeaderText="No. Req." SortExpression="NoRequisicion" Visible="true" />
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Responsable" HeaderText="Responsable" SortExpression="Responsable" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Almacen" HeaderText="Almacen" SortExpression="Almacen" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CantidadProducto" HeaderText="Cant. Prod" SortExpression="CantidadProducto" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="FechaRequerida" HeaderText="Fecha Requerida" SortExpression="FechaRequerida" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lkbDetalle" runat="server" Text="Detalle" OnClick="lkbDetalle_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lkbRequisicionOrden" runat="server" Text="Editar" OnClick="lkbRequisicionOrden_Click"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="lkbCancelar" runat="server" Text="Cancelar" OnClick="lkbCancelar_Click"></asp:LinkButton><br />
                                                        <asp:LinkButton ID="lkbEliminarReq" runat="server" Text="Eliminar" OnClick="lkbEliminarReq_Click"></asp:LinkButton><br />
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
                                        <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewRequisicion" />
                                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                                        <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                                        <asp:AsyncPostBackTrigger ControlID="ucRequisicion" />
                                        <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalle" />
                                        <asp:AsyncPostBackTrigger ControlID="lnkNuevaRequisicion" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarImagen" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminarReq" />
                                        <asp:AsyncPostBackTrigger ControlID="btnAceptarCanRequisicion" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwDetalles" runat="server">


                    <div class="columna3x">
                        <div class="contenedor_seccion_completa">
                            <div class="header_seccion">
                                <h2>Detalles</h2>
                            </div>
                            <div class="renglon3x">
                                <div class="etiqueta_50px">
                                    <label for="ddlTamañoGridViewRequisicionDetalle">Mostrar</label>
                                </div>
                                <div class="control_100px">
                                    <asp:UpdatePanel ID="upddlTamañoGridViewRequisicionDetalle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTamañoGridViewRequisicionDetalle" runat="server" OnSelectedIndexChanged="ddlTamañoGridViewRequisicionDetalle_SelectedIndexChanged" TabIndex="16" AutoPostBack="true" CssClass="dropdown_100px">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <label for="lblOrdenarServicios">Ordenado Por:</label>
                                </div>
                                <div class="etiqueta_155px">
                                    <asp:UpdatePanel ID="uplblCriterioGridViewRequisicionDetalle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblCriterioGridViewRequisicionDetalle" runat="server" CssClass="label_negrita"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalle" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta_50px">
                                    <asp:UpdatePanel runat="server" ID="uplkbExportarExcelRequisicionDetalle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbExportarExcelRequisicionDetalle" runat="server" Text="Exportar" TabIndex="17" OnClick="lkbExportarExcelRequisicionDetalle_Onclick"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbExportarExcelRequisicionDetalle" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="grid_seccion_completa">
                                <asp:UpdatePanel ID="upgvRequisicionDetalle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvRequisicionDetalle" CssClass="gridview" OnPageIndexChanging="gvRequisicionDetalle_OnpageIndexChanging"
                                            OnSorting="gvRequisicionDetalle_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                                            ShowFooter="True" FooterStyle-HorizontalAlign="Right" PageSize="5" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="No. Req." SortExpression="NoRequisicion">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lkbNoRequisicion" runat="server" Text='<%# Eval("NoRequisicion")%>' OnClick="lkbRequicionEncabezado_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NoDetalle" HeaderText="No. Detalle" SortExpression="NoDetalle" ItemStyle-HorizontalAlign="Right" Visible="false" />
                                                <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CodProducto" HeaderText="Código" SortExpression="CodProducto" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad de Medida" SortExpression="UnidadMedida" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" />
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
                                        <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvRequisicion" />
            <asp:AsyncPostBackTrigger ControlID="gvRequisicionDetalle" />
            <asp:AsyncPostBackTrigger ControlID="gvActividades" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- VENTANA MODAL QUE MUESTRA EL CONTROL DE USUARIO PARA DAR DE ALTA LAS ASIGNACIONES -->
    <div id="contenidoAsignacion" class="modal">
        <div id="confirmacionAsignacion" class="contenedor_ventana_confirmacion">
            <div style="text-align: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarAsigancion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarAsigancion" runat="server" Text="Cerrar" OnClick="lkbCerrarAsignacion_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="upwucAsignacionActividad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <tectos:wucAsignacion ID="wucAsignacionActividad" runat="server" OnClickIniciarAsignacion="wucAsignacionActividad_ClickIniciarAsignacion" OnClickEliminarAsignacion="wucAsignacionActividad_ClickEliminarAsignacion"
                                OnClickGuardarAsignacion="wucAsignacionActividad_ClickGuardarAsignacion" OnClickTerminarAsignacion="wucAsignacionActividad_ClickTerminarAsignacion" TabIndex="23" Contenedor="#confirmacionAsignacion"></tectos:wucAsignacion>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                            <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarInicio" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE EL INICIO DE LA ASIGNACIÓN-->
    <div id="contenidoFechaInicio" class="modal">
        <div id="confirmacionFechaInicio" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaInicio" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaInicio" runat="server" Text="Cerrar" OnClick="lkbCerrarFechaInicio_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Inicia/Pausa Asignación</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaInicio">Fecha </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaInicio" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarInicio" />
                                <asp:AsyncPostBackTrigger ControlID="wucAsignacionActividad" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarInicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarInicio" runat="server" CssClass="boton" OnClick="btnAceptarInicio_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE EL INICIO DE LA ASIGNACIÓN-->
    <div id="contenidoFechaInicioDirecto" class="modal">
        <div id="confirmacionFechaInicioDirecto" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaInicioDirecto" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaInicioDirecto" runat="server" Text="Cerrar" CommandName="inicioAsignacionDirecto" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Inicia/Pausa Asignación</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaInicioDirecto">Fecha </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaInicioDirecto" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaInicioDirecto" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarInicioDirecto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarInicioDirecto" runat="server" CssClass="boton" OnClick="btnAceptarInicioDirecto_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE EL TERMINO DE LA ASIGNACIÓN-->
    <div id="contenidoFechaTerminarAsignacion" class="modal">
        <div id="confirmacionFechaTerminarAsignacion" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaTerminar" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaTerminar" runat="server" Text="Cerrar" OnClick="lkbCerrarFechaTerminar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Terminar Asignación</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCita">Fecha </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaTerminar" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaTerminar" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarTerminar" />
                                <asp:AsyncPostBackTrigger ControlID="wucAsignacionActividad" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarTerminar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarTerminar" runat="server" CssClass="boton" OnClick="btnAceptarTerminar_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE EL TERMINO DE LA ACTIVIDAD-->
    <div id="contenidoFechaTerminarActividad" class="modal">
        <div id="confirmacionFechaTerminarActividad" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaTerminarActividad" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaTerminarActividad" runat="server" Text="Cerrar" OnClick="lkbCerrarFechaTerminarActividad_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Terminar Actividad</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaTerminarActividad">Fecha </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaTerminarActividad" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaTerminarActividad" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnTerminarActividad" />
                                <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnTerminarActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnTerminarActividad" runat="server" CssClass="boton" OnClick="btnAceptarTerminarActividad_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL QUE ADVIERTE EL TERMINO DE LA ASIGNACIÓN-->
    <div id="contenidoFechaTerminarAsignacionDirecto" class="modal">
        <div id="confirmacionFechaTerminarAsignacionDirecto" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarFechaTerminarDirecto" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarFechaTerminarDirecto" runat="server" Text="Cerrar" CommandName="terminoAsignacionDirecto" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Terminar Asignación</h2>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaTerminarDirecto">Fecha </label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaTerminarDirecto" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaTerminarDirecto" runat="server" Enabled="true" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarTerminarDirecto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarTerminarDirecto" runat="server" CssClass="boton" OnClick="btnAceptarTerminarDirecto_Click" Text="Aceptar" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="contenidoConfirmacionEliminarActividad" class="modal">
        <div id="confirmacionEliminarActividad" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Eliminar Actividad</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea eliminar la Actividad?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarEliminarActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarEliminarActividad" runat="server" OnClick="btnCancelarEliminarActividad_Click" CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers></Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarEliminarActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarEliminarActividad" runat="server" OnClick="btnAceptarEliminarActividad_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Modal Cancelación-->
    <div id="contenidoConfirmacionCancelarAsignacion" class="modal">
        <div id="confirmacionCancelarAsignacion" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Cancelar Asignación</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea cancelar la Asignación?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarCancelarAsignacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarCancelarAsignacion" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarCancelarAsignacion_Click" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers></Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarCancelarAsignacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarCancelarAsignacion" runat="server" OnClick="btnAceptarCancelarAsignacion_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="contenidoConfirmacionCancelarActividad" class="modal">
        <div id="confirmacionCancelarActividad" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Cancelar Actividad</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea cancelar la Actividad?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarCancelarActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarCancelarActividad" runat="server" OnClick="btnCancelarCancelarActividad_Click" CssClass="boton_cancelar" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers></Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarCancelarActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarCancelarActividad" runat="server" OnClick="btnAceptarCancelarActividad_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Alta Requisición -->
    <div id="contenidoVentanaRequisicion" class="modal">
        <div id="ventanaRequisicion" class="contenedor_modal_seccion_completa_arriba">
            <div style="float: right">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarImagen" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarImagen" runat="server" CommandName="Requisicion" OnClick="lkbCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucRequisicion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucRequisicion ID="ucRequisicion" runat="server" OnClickGuardarRequisicion="ucRequisicion_ClickGuardarRequisicion"
                        OnClickSolicitarRequisicion="ucRequisicion_ClickSolicitarRequisicion" Contenedor="#ventanaRequisicion" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                    <asp:AsyncPostBackTrigger ControlID="gvRequisicion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarReq" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarReq" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarAlmacenReq" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana Alta Requisición -->
    <div id="contenedorVentanaIngresoAlmacen" class="modal">
        <div id="ventanaIngresoAlmacen" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarAlmacen" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarAlmacen" runat="server" CommandName="Almacen" OnClick="lkbCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Ingrese el Almacen</h2>
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta_50px">
                        <label for="txtAlmacen">Almacen</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtAlmacen" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAlmacen" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardarReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardarReq" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardarReq_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarReq" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarReq_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Ventana Alta Requisición (Sin Copia) -->
    <div id="contenedorVentanaIngresoAlmacenReq" class="modal">
        <div id="ventanaIngresoAlmacenReq" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarAlmacenReq" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarAlmacenReq" runat="server" CommandName="almacenReq" OnClick="lnkCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>Ingrese el Almacen </h2>
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta_50px">
                        <label for="txtAlmacenReq">Almacen</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtAlmacenReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAlmacenReq" runat="server" Contenedor="#ventanaIngresoAlmacenReq" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkNuevaRequisicion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardarAlmacenReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardarAlmacenReq" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardarAlmacenReq_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Ventana modal Actividad-->
    <div id="contenedorVentanaActividades" class="modal">
        <div id="ventanaActividades" class="contenedor_modal_seccion_completa_arriba" style="width: 1130px">
            <div style="float: right">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarImagen" runat="server" Text="Cerrar" CommandName="activiades" OnClick="lnkCerrar_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucActividadOrdenTrabajo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucActividadOrdenTrabajo ID="ucActividadOrdenTrabajo" runat="server" OnClickRegistrar="wucActividadOrdenTrabajo_Registrar"
                        TabIndex="30"></tectos:wucActividadOrdenTrabajo>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvActividades" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Fin modal Cancelar Requisicion-->
    <div id="contenidoConfirmacionCanRequisicion" class="modal">
        <div id="confirmacionCanRequisicion" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCanRequisicion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCanRequisicion" runat="server" CommandName="canRequisicion" OnClick="lnkCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Cancelar Requisición</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea cancelar la Requisición?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarCanRequisicion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarCanRequisicion" runat="server" OnClick="btnAceptarCanRequisicion_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Fin modal Eliminar Requisicion-->
    <div id="contenidoConfirmacionEliminarRequisicion" class="modal">
        <div id="confirmacionEliminarRequisicion" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarEliminarReq" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarEliminarReq" runat="server" CommandName="eliminarReq" OnClick="lnkCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Eliminar Requisición</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea Eliminar la Requisición?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarEliminarReq" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarEliminarReq" runat="server" OnClick="btnAceptarEliminarReq_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--Fin modal AbrirActividad Requisicion-->
    <div id="contenidoConfirmacionAbrirActividad" class="modal">
        <div id="confirmacionAbrirActividad" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbAbrirActividad" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbAbrirActividad" runat="server" CommandName="abrirActividad" OnClick="lnkCerrar_Click" Text="Cerrar">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h3>Abrir Actividad </h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">¿Realmente desea Abrir la Actividad?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarAbrirActividad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarAbrirActividad" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarAbrirActividad_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

