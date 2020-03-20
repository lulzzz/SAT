<%@ Page Title="Unidad" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Unidad.aspx.cs" Inherits="SAT.General.Unidad" %>

<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucVencimiento.ascx" TagPrefix="uc1" TagName="wucVencimiento" %>
<%@ Register Src="~/UserControls/wucCambioOperador.ascx" TagPrefix="uc1" TagName="wucCambioOperador" %>
<%@ Register Src="~/UserControls/wucLecturaHistorial.ascx" TagPrefix="uc1" TagName="wucLecturaHistorial" %>
<%@ Register Src="~/UserControls/wucLectura.ascx" TagPrefix="uc1" TagName="wucLectura" %>
<%@ Register Src="~/UserControls/wucProveedorGPSDiccionario.ascx" TagPrefix="ucl" TagName="wucProveedorGPSDiccionario" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">        
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryUnidad();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryUnidad() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionUnidad = function (evt) {
                    var isValidP1 = !$("#<%=txtNumUnidad.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtNoEjes.ClientID%>").validationEngine('validate');
    var isValidP3 = !$("#<%=txtPropietario.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtFechaAdquisicion.ClientID%>").validationEngine('validate');
    var isValidP5 = !$("#<%=txtPlacas.ClientID%>").validationEngine('validate');
    var isValidP6 = !$("#<%=txtKmAsigando.ClientID%>").validationEngine('validate');
    var isValidP7 = !$("#<%=txtCapacidadCombustible.ClientID%>").validationEngine('validate');
    var isValidP8 = !$("#<%=txtPeso.ClientID%>").validationEngine('validate');
    var isValidP9 = !$("#<%=txtAno.ClientID%>").validationEngine('validate');

        return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8 && isValidP9;
    };
    //Función de validación de baja de operador
    var validacionBajaUnidad = function (evt) {
        var isValidP1 = !$("#<%=txtCFechaBaja.ClientID%>").validationEngine('validate');
        return isValidP1;
    };
    //Función de validación de ubicación Inicial
    var validacionUbicacionInicial = function (evt) {
        var isValidP1 = !$("#<%=txtUbicacionInicial.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtFechaEstanciaInicial.ClientID%>").validationEngine('validate');
        return isValidP1 && isValidP2;
    };

    //Función de validación de Servicio GPS
    var validacionGPS = function (evt) {
        var isValidP1 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
    var isValidP2 = !$("#<%=txtIdentificador.ClientID%>").validationEngine('validate');
    var isValidP3 = !$("#<%=txtTEncendido.ClientID%>").validationEngine('validate');
    var isValidP4 = !$("#<%=txtTApagado.ClientID%>").validationEngine('validate');
        return isValidP1 && isValidP2 && isValidP3 && isValidP4;
    };

    //Ubicación de Carga
    $("#<%=txtProveedor.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=58&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>',
    appendTo: "#ventanaProveedorGPS",
    select: function (event, ui) {
        //Asignando Selección al Valor del Control
        $("#<%=txtProveedor.ClientID%>").val(ui.item.value);
    //Causando Actualización del Control
    __doPostBack('<%= txtProveedor.UniqueID %>', '');
        }
    });
    //Boton Guardar
    $("#<%=btnGuardarGPS.ClientID%>").click(validacionGPS);
    //Boton Guardar
    $("#<%=btnGuardar.ClientID%>").click(validacionUnidad);
    //Boton Guardar
    $("#<%=lkbGuardar.ClientID%>").click(validacionUnidad);
    //Boton Confirmación de Baja
    $("#<%=btnAceptarBajaUnidad.ClientID%>").click(validacionBajaUnidad);
    //Boton Confirmación Ubicación Inicial
    $("#<%=btnAceptarUbicacionInicial.ClientID%>").click(validacionUbicacionInicial);
    //Cargando Catalogo AutoCompleta Compania
    $("#<%=txtCompania.ClientID%>").autocomplete({ source: '../System.IO.Stream.ashx?id=4' });
    //Cargando Catalogo AutoCompleta Proveedor
    $("#<%=txtPropietario.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=26&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
    //Cargando Catalogo AutoCompleta Ubicación Inicial
    $("#<%=txtUbicacionInicial.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>',
        appendTo: '#confirmacionEstanciaInicial'
    });
    //Fecha de Compra
    $("#<%=txtFechaAdquisicion.ClientID%>").datetimepicker({
        lang: 'es',
        timepicker: false,
        format: 'd/m/Y'
    });
    //Fecha de Baja
    $("#<%=txtCFechaBaja.ClientID%>").datetimepicker({
        lang: 'es',
        timepicker: false,
        format: 'd/m/Y'
    });
    //Fecha de Inicio de estancia
    $("#<%=txtFechaEstanciaInicial.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryUnidad();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/OperacionPatio.png" />
        <h1>Administracion de Unidades</h1>
    </div>
    <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <nav id="menuForma">
                <ul>
                    <li class="green">
                        <a href="#" class="fa fa-floppy-o"></a>
                        <ul>

                            <li>

                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
                            <li>
                                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
                            <li>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
                        </ul>
                    </li>
                    <li class="red">
                        <a href="#" class="fa fa-pencil-square-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbBajaUnidad" runat="server" Text="Estatus Baja" ToolTip="Coloca la Unidad en estatus 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Baja" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReactivar" runat="server" Text="Estatus Disp." ToolTip="Coloca la Unidad en estatus 'Disponible', despúes de estar en 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Reactivar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbRefrescaEcosistema" runat="server" Text="Ecosistema" ToolTip="Refresca la información requerida al Hecostistema para la Publicación de Unidades " OnClick="lkbElementoMenu_Click" CommandName="RefrescaEcosistema" /></li>
                        </ul>
                    </li>
                    <li class="blue">
                        <a href="#" class="fa fa-cog"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbHistorial" runat="server" Text="Historial" OnClick="lkbElementoMenu_Click" CommandName="Historial" /></li>
                            <li>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
                            <li>
                                <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
                        </ul>
                    </li>
                    <li class="yellow">
                        <a href="#" class="fa fa-flag-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbCambioOperador" runat="server" Text="Cambio Operador" OnClick="lkbElementoMenu_Click" CommandName="CambioOperador" /></li>
                            <li>
                                <asp:LinkButton ID="lkbVencimientos" runat="server" Text="Vencimientos" OnClick="lkbElementoMenu_Click" CommandName="Vencimientos" /></li>
                            <li>
                                <asp:LinkButton ID="lkbLecturaHistorial" runat="server" Text="Lecturas" OnClick="lkbElementoMenu_Click" CommandName="Lecturas" /></li>
                            
                        </ul>
                    </li>
                    <li class="gray">
                        <a href="#" class="fa fa-flag-checkered"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbKmsAsignado" runat="server" Text="Kms Asignado" OnClick="lkbElementoMenu_Click" CommandName="KmsAsignado" /></li>
                            <li>
                                <asp:LinkButton ID="lkbProveedorGPS" runat="server" Text="Proveedor GPS" OnClick="lkbElementoMenu_Click" CommandName="ProveedorGPS" /></li>
                             <li>
                                <asp:LinkButton ID="lkbProveedorWS" runat="server" Text="Proveedor WS" OnClick="lkbElementoMenu_Click" CommandName="ProveedorWS" /></li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
            <asp:PostBackTrigger ControlID="lkbHistorial" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Requerimiento.png" />
            <h2>Datos de la Unidad</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompania">Compañía</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompania" runat="server" TabIndex="1" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNumUnidad">Núm. Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNumUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNumUnidad" runat="server" TabIndex="2" CssClass="textbox validate[required]" MaxLength="30"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoUnidad">Tipo Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoUnidad" runat="server" CssClass="dropdown" TabIndex="3" OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" AutoPostBack="true" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlSubTipoUnidad">Subtipo Unidad</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlSubTipoUnidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSubTipoUnidad" runat="server" CssClass="dropdown" TabIndex="4" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlDimensiones">Dimensiones</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlDimensiones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlDimensiones" runat="server" CssClass="dropdown" TabIndex="5" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoEjes">No. Ejes</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNoEjes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoEjes" runat="server" TextMode="Number" min="1" max="99" CssClass="textbox validate[required, custom[onlyNumberSp]]" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPropietario">Propietario</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtPropietario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPropietario" runat="server" TabIndex="8" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="chkPropietario" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control_60px">
                    <asp:UpdatePanel ID="upchkPropietario" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkPropietario" runat="server" Text="¿Otro?" ToolTip="Indica que la unidad pertenece a un permisionario y debe ser especificado." CssClass="label" TabIndex="7" AutoPostBack="True" OnCheckedChanged="chkPropietario_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaAdquisicion">Fecha Adquisición</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaAdquisicion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaAdquisicion" runat="server" TabIndex="9" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaBaja">Fecha Baja</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaBaja" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaBaja" runat="server" TabIndex="10" CssClass="textbox validate[custom[date]]" MaxLength="10" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstadoPlacas">Edo. Exp. Placas</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlEstadoPlacas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlEstadoPlacas" runat="server" CssClass="dropdown2x" TabIndex="11" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPlacas">Placas</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtPlacas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPlacas" runat="server" TabIndex="12" CssClass="textbox validate[required]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOperador">Operador</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOperador" runat="server" TabIndex="13" CssClass="textbox2x validate[custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="15" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUbicacionActual">Ubicación Actual</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtUbicacionActual" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUbicacionActual" runat="server" TabIndex="16" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaActualizacion">Actualización</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaActualizacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaActualizacion" runat="server" TabIndex="17" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMarca">Marca</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlMarca" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMarca" runat="server" CssClass="dropdown" TabIndex="18" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtModelo">Modelo</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtModelo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtModelo" runat="server" TabIndex="19" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSerie">Serie</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerie" runat="server" TabIndex="20" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtAno">Año</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtAno" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAno" runat="server" TextMode="Number" min="1950" max="2099" TabIndex="21" CssClass="textbox validate[custom[onlyNumberSp]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMarcaMotor">Marca Motor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlMarcaMotor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMarcaMotor" runat="server" CssClass="dropdown" TabIndex="22" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtModeloMotor">Modelo Motor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtModeloMotor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtModeloMotor" runat="server" TabIndex="23" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtSerieMotor">Serie Motor</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtSerieMotor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSerieMotor" runat="server" TabIndex="24" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPeso">Peso</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtPeso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtPeso" runat="server" TabIndex="25" CssClass="textbox validate[custom[positiveNumber]]" MaxLength="19"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlUnidadPeso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlUnidadPeso" runat="server" CssClass="dropdown_100px" TabIndex="26"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCapacidadCombustible">Cap. Combustible</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCapacidadCombustible" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCapacidadCombustible" runat="server" TabIndex="27" CssClass="textbox validate[custom[positiveNumber]] " MaxLength="19"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="validador">
                    <label for="txtCapacidadCombustible">Lts.</label>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCombustibleAsignado">Comb. Asignado</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtCombustibleAsignado" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCombustibleAsignado" runat="server" TabIndex="28" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="validador">
                    <label for="txtCombustibleAsignado">Lts.</label>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtKmAsigando">Km. Asignado</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtKmAsigando" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtKmAsigando" runat="server" TabIndex="29" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="19"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtAntenaGPS">Antena GPS</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtAntenaGPS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAntenaGPS" runat="server" TabIndex="30" CssClass="textbox2x" MaxLength="30"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%--<div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlClasificacion">Clasificación</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlClasificacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlClasificacion" runat="server" CssClass="dropdown2x" TabIndex="31"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" /><asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>--%>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="32" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="33" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                            <asp:AsyncPostBackTrigger ControlID="wucCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarUbicacionInicial" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalBajaUnidad" class="modal">
        <div id="confirmacionBajaUnidad" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <h3>Actualización de Baja de Unidad</h3>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <label class="mensaje_modal">Confirme la Fecha de Baja para la Unidad.</label>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCFechaBaja">Fecha Baja</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCFechaBaja" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCFechaBaja" runat="server" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbBajaUnidad" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarBajaUnidad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarBajaUnidad" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarBajaUnidad_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarBajaUnidad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarBajaUnidad" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarBajaUnidad_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalEstanciaInicial" class="modal">
        <div id="confirmacionEstanciaInicial" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <h3>Ubicación Inical de la Unidad</h3>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <label class="mensaje_modal">Seleccione la Ubicación deseada.</label>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUbicacionInicial">Ubicación</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtUbicacionInicial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUbicacionInicial" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaEstanciaInicial">Inicio Estancia</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFechaEstanciaInicial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaEstanciaInicial" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarUbicacionInicial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarUbicacionInicial" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarUbicacionInicial_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarUbicacionInicial" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarUbicacionInicial" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarUbicacionInicial_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- VENTANA MODAL CAMBIO OPERADOR-->
    <div id="modalCambioOperador" class="modal">
        <div id="confirmacionCambioOperador" class="contenedor_ventana_confirmacion">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCambioOperador" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCambioOperador" runat="server" OnClick="lkbCerrarCambioOperador_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucCambioOperador" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucCambioOperador ID="wucCambioOperador" runat="server" OnClickRegistrar="wucCambioOperador_ClickRegistrar" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbCambioOperador" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL HISTORIAL VENCIMIENTOS -->
    <div id="modalHistorialVencimientos" class="modal">
        <div id="vencimientosRecurso" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialVencimientos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarHistorialVencimientos" runat="server" OnClick="lkbCerrarVencimientos_Click" CommandName="Historial" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucVencimientosHistorial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucVencimientosHistorial runat="server" ID="wucVencimientosHistorial" OnlkbConsultar="OnlkbConsultar_Click" OnlkbTerminar="OnlkbTerminar_Click" OnbtnNuevoVencimiento="OnbtnNuevoVencimiento_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbVencimientos" />
                    <asp:AsyncPostBackTrigger ControlID="wucVencimiento" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL VENCIMIENTOS -->
    <div id="modalVencimiento" class="modal">
        <div id="vencimientoSeleccionado" class="contenedor_modal_seccion_completa">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarVencimientoSeleccionado" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarVencimientoSeleccionado" runat="server" OnClick="lkbCerrarVencimientos_Click" CommandName="Vencimiento" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucVencimiento" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucVencimiento runat="server" ID="wucVencimiento" OnClickGuardarVencimiento="OnClickGuardarVencimiento_Click" OnClickTerminarVencimiento="OnClickTerminarVencimiento_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="wucVencimientosHistorial" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL HISTORIAL LECTURAS -->
    <div id="modalLecturaHistorial" class="modal">
        <div id="lecturaHistorial" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialLectura" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarLecturaHistorial" runat="server" OnClick="lkbCerrarLecturaHistorial_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucLecturaHistorial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucLecturaHistorial ID="wucLecturaHistorial" OnlkbConsultar="wucLecturaHistorial_lkbConsultar" OnbtnNuevaLectura="wucLecturaHistorial_btnNuevaLectura" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbLecturaHistorial" />
                    <asp:AsyncPostBackTrigger ControlID="wucLectura" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL LECTURAS -->
    <div id="modalLectura" class="modal">
        <div id="Lectura" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarLectura" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarLectura" runat="server" OnClick="lkbCerrarLectura_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucLectura" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:wucLectura ID="wucLectura" runat="server" OnClickEliminarLectura="wucLectura_ClickEliminarLectura" OnClickGuardarLectura="wucLectura_ClickGuardarLectura" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="wucLecturaHistorial" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana Modal de Servicios GPS -->
    <div id="contenedorVentanaProveedorGPS" class="modal">
        <div id="ventanaProveedorGPS" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarProveedorGPS" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarProveedorGPS" runat="server" OnClick="lkbCerrarProveedorGPS_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna3x">
                <div class="header_seccion">
                    <img src="../Image/paradas.png" />
                    <h2>Proveedor GPS</h2>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtProveedor">Proveedor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"
                                    AutoPostBack="true" OnTextChanged="txtProveedor_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtIdentificador">Identificador</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtIdentificador" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtIdentificador" runat="server" CssClass="textbox2x validate[required]" MaxLength="50"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlServicioGPS">Servicio GPS</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="upddlServicioGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlServicioGPS" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtProveedor" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon3x" style="float: left;">
                    <div class="etiqueta">
                        <label>T. Encencido</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtTEncendido" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTEncendido" runat="server" CssClass="textbox_100px validate[required, custom[integer]]" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label>T. Apagado</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtTApagado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTApagado" runat="server" CssClass="textbox_100px validate[required, custom[integer]]" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="upchkDefault" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkDefault" runat="server" Text="¿Antena Predeterminada?" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                                <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label>(min)</label>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarGPS" runat="server" Text="Cancelar" OnClick="btnCancelarGPS_Click" CssClass="boton_cancelar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardarGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardarGPS" runat="server" Text="Guardar" OnClick="btnGuardarGPS_Click" CssClass="boton" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="header_seccion">
                    <h2>Asignaciones</h2>
                </div>
                <div class="renglon3x" style="float: left;">
                    <div class="etiqueta">
                        <label for="ddlMostrarGPS">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoGPS" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoGPS_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblOrdenadoGPS">Ordenado</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblOrdenadoGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblOrdenadoGPS" runat="server" CssClass="label_negrita"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAsignaciones" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50pxr">
                        <asp:UpdatePanel ID="uplnkExportarGPS" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarGPS" runat="server" Text="Exportar" OnClick="lnkExportarGPS_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarGPS" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_200px_altura">
                    <asp:UpdatePanel ID="upgvAsignaciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAsignaciones" runat="server" AutoGenerateColumns="False" AllowPaging="True" ShowFooter="True"
                                OnPageIndexChanging="gvAsignaciones_PageIndexChanging" OnSorting="gvAsignaciones_Sorting"
                                ShowHeaderWhenEmpty="True" PageSize="25" AllowSorting="True" OnRowDataBound="gvAsignaciones_RowDataBound"
                                CssClass="gridview" Width="100%">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <Columns>
                                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
                                    <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
                                    <asp:BoundField DataField="ServicioGPS" HeaderText="Servicio GPS" SortExpression="ServicioGPS" />
                                    <asp:BoundField DataField="AntenaPredeterminada" HeaderText="Antena Predeterminada" SortExpression="AntenaPredeterminada" />
                                    <asp:BoundField DataField="TiempoEncendido" HeaderText="Tiempo Encendido" SortExpression="TiempoEncendido" />
                                    <asp:BoundField DataField="TiempoApagado" HeaderText="Tiempo Apagado" SortExpression="TiempoApagado" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditarGPS" runat="server" Text="Editar" OnClick="lnkEditarGPS_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarGPS" runat="server" Text="Eliminar" OnClick="lnkEliminarGPS_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkMarcaDefault" runat="server" Text="Marcar" OnClick="lnkMarcaDefault_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarGPS" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarGPS" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoGPS" />
                            <asp:AsyncPostBackTrigger ControlID="lkbProveedorGPS" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>

       <!-- Ventana Modal de Proveedor WS -->
    <div id="contenedorVentanaProveedorWS" class="modal">
        <div id="ventanaProveedorWS" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarProveedorWS" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarProveedorWS" runat="server" OnClick="lkbCerrarProveedorWS_Click" Text="Cerrar">
                     <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucProveedorGPSDiccionario" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <div class="columna4x" style="width:800px; height:470px;" runat="server">
                    <ucl:wucproveedorgpsdiccionario runat="server" id="wucProveedorGPSDiccionario" />
                          </div>
                </ContentTemplate>
                <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="lkbProveedorWS" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

   

    <!-- Ventana encargada de Gestionar el Kms Asignado de la Unidad -->
    <div id="contenedorVentanaKmsAsignado" class="modal">
        <div id="ventanaKmsAsignado" class="contenedor_ventana_confirmacion" style="min-width:485px; padding-bottom:15px;">
            <div class="columna2x">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaKmsAsignado" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarVentanaKmsAsignado" runat="server" OnClick="lkbCerrarVentanaKmsAsignado_Click" Text="Cerrar">
                                <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="header_seccion">
                    <img src="../Image/EnTransito.png" />
                    <h2>Actualice su Kms Asignado</h2>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="txtKmsAsignadoNvo">Kms Nvo.</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtKmsAsignadoNvo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtKmsAsignadoNvo" runat="server" CssClass="textbox2x" placeHolder="Ingrese su Kilometraje Actual"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnActualizarKms" />
                                <asp:AsyncPostBackTrigger ControlID="lkbKmsAsignado" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnActualizarKms" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnActualizarKms" runat="server" CssClass="boton"
                                    Text="Actualizar" OnClick="btnActualizarKms_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
