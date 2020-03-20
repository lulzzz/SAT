<%@ Page Title="Operador" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Operador.aspx.cs" Inherits="SAT.General.Operador" %>

<%@ Register Src="~/UserControls/wucCuentaBanco.ascx" TagPrefix="tectos" TagName="wucCuentaBanco" %>
<%@ Register Src="~/UserControls/wucDireccion.ascx" TagName="WucDireccion" TagPrefix="tectos" %>
<%@ Register Src="~/UserControls/wucVencimientosHistorial.ascx" TagPrefix="uc1" TagName="wucVencimientosHistorial" %>
<%@ Register Src="~/UserControls/wucVencimiento.ascx" TagPrefix="uc1" TagName="wucVencimiento" %>
<%@ Register Src="~/Externa/wucCalificacion.ascx" TagPrefix="tectos" TagName="wucCalificacion" %>
<%@ Register Src="~/Externa/wucHistorialCalificacion.ascx" TagPrefix="tectos" TagName="wucHistorialCalificacion" %>
<%@ Register Src="~/UserControls/wucProveedorGPSDiccionario.ascx" TagPrefix="ucl" TagName="wucProveedorGPSDiccionario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Estilos Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <style>
        .promEmp {
            margin: -3px 0px 0px 0px;
            padding: 0px 0px 0px 550px;
            width: 165px;
            height: 29px;
            float: left;
        }

        .comentariosEmp {
            margin: 0px 0px 0px 0px;
            padding: 2px 0px 0px 0px;
            width: 238px;
            height: 18px;
            float: left;
        }

        .foto {
            padding: 0px 0px 0px 0px;
            border: 2px solid black;
            width: 150px;
            float: right;
        }

        .columnafoto {
            margin: 0px;
            padding: 0px;
            width: 200px;
            float: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryOperador();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryOperador() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionOperador = function (evt) {

                    //Validando Controles
                    var isValidP1 = !$("#<%=txtNombre.ClientID%>").validationEngine('validate');
                    var isValidP2 = !$("#<%=txtFechaNacimiento.ClientID%>").validationEngine('validate');
                    var isValidP3 = !$("#<%=txtRFC.ClientID%>").validationEngine('validate');
                    var isValidP4 = !$("#<%=txtCURP.ClientID%>").validationEngine('validate');
                    var isValidP5 = !$("#<%=txtNSS.ClientID%>").validationEngine('validate');
                    var isValidP6;
                    var isValidP7 = !$("#<%=txtFechaIngreso.ClientID%>").validationEngine('validate');
                    var isValidP8 = !$("#<%=txtDireccion.ClientID%>").validationEngine('validate');
                    var isValidP9 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
                    var isValidP10 = !$("#<%=txtPeriodoInicial.ClientID%>").validationEngine('validate');
                    var isValidP11 = !$("#<%=txtPeriodoFinal.ClientID%>").validationEngine('validate');

                    //Obteniendo Valor del Control
                    var tipoOperador = $("#<%=ddlTipoOperador.ClientID%>").val();

                    //Validando Selección
                    switch (tipoOperador) {
                        case "1":
                            {
                                //Controles de Operador
                                var isValidP6 = !$("#<%=txtNoLicencia.ClientID%>").validationEngine('validate');
                                break;
                            }
                        case "2":
                            {
                                //Resultado Positivo
                                var isValidP6 = true;
                                break;
                            }
                    }

                    return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8 && isValidP9 && isValidP10 && isValidP11;
                };
                //Función de validación de baja de operador
                var validacionBajaOperador = function (evt) {
                    var isValidP1 = !$("#<%=txtCFechaBaja.ClientID%>").validationEngine('validate');
                    return isValidP1;
                };
                //Boton Guardar
                $("#<%=btnGuardar.ClientID%>").click(validacionOperador);
                //Boton Guardar
                $("#<%=lkbGuardar.ClientID%>").click(validacionOperador);
                //Boton Confirmación de Baja
                $("#<%=btnAceptarBajaOperador.ClientID%>").click(validacionBajaOperador);
                //Boton imprimir
                $("#<%=btnImprimir.ClientID%>").click(validacionOperador)
                //Cargando Catalogo AutoCompleta
                $("#<%=txtCompania.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=4' });
                //Fecha de Nacimiento
                $("#<%=txtFechaNacimiento.ClientID%>").datetimepicker({
                    lang: 'es',
                    timepicker: false,
                    format: 'd/m/Y'
                });
                //Fecha de Nacimiento
                $("#<%=txtFechaIngreso.ClientID%>").datetimepicker({
                    lang: 'es',
                    timepicker: false,
                    format: 'd/m/Y'
                });
                //Fecha de Nacimiento
                $("#<%=txtCFechaBaja.ClientID%>").datetimepicker({
                    lang: 'es',
                    timepicker: false,
                    format: 'd/m/Y'
                });
                //Fecha PeriodoInicialModal
                $("#<%=txtPeriodoInicial.ClientID%>").datetimepicker({
                    lang: 'es',
                    timepicker: false,
                    format: 'd/m/Y'
                });
                //Fecha PeriodoFinalModal
                $("#<%=txtPeriodoFinal.ClientID%>").datetimepicker({
                    lang: 'es',
                    timepicker: false,
                    format: 'd/m/Y'
                });
            });
        }
        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryOperador();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Operador.png" />
        <h1>Empleado</h1>
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
                                <asp:LinkButton ID="lkbCuentasBanco" runat="server" Text="Cuentas Banco" OnClick="lkbElementoMenu_Click" CommandName="CuentasBanco" /></li>
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
                                <asp:LinkButton ID="lkbBajaOperador" runat="server" Text="Estatus Baja" ToolTip="Coloca la Unidad en estatus 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Baja" /></li>
                            <li>
                                <asp:LinkButton ID="lkbReactivar" runat="server" Text="Estatus Disp." ToolTip="Coloca la Unidad en estatus 'Disponible', despúes de estar en 'Baja'" OnClick="lkbElementoMenu_Click" CommandName="Reactivar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbRefrescaEcosistema" runat="server" Text="Ecosistema" ToolTip="Refresca la información requerida al Hecostistema para la Publicación de Operadores " OnClick="lkbElementoMenu_Click" CommandName="RefrescaEcosistema" /></li>
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
                                <asp:LinkButton ID="lkbVencimientos" runat="server" Text="Vencimientos" OnClick="lkbElementoMenu_Click" CommandName="Vencimientos" /></li>
                            <li>
                                <asp:LinkButton ID="lkbContratoIndeterminado" runat="server" Text="C.Definido" OnClick="lkbElementoMenu_Click" CommandName="ContratoIndeterminado" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbImprimirRenuncia" runat="server" Text="Renuncia" OnClick="lkbElementoMenu_Click" CommandName="Renuncia" />
                            </li>
                            <li>
                                <asp:LinkButton ID="lkbContratoTiempoDefinido" runat="server" Text="C.Indet." OnClick="lkbElementoMenu_Click" CommandName="ContratoTiempoDefinido" />
                            </li>
                        </ul>
                    </li>
                    <li class="gray">
                        <a href="#" class="fa fa-flag-checkered"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbProveedorWS" runat="server" Text="Proveedor WS" OnClick="lkbElementoMenu_Click" CommandName="ProveedorWS" /></li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
            <asp:PostBackTrigger ControlID="lkbHistorial" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Requerimiento.png" />
            <h2>Datos del Empleado</h2>
            <div class="promEmp">
                <asp:UpdatePanel runat="server" ID="upimgbCalificacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ImageButton runat="server" ID="imgbCalificacion" ImageUrl="" OnClick="imgbCalificacion_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="comentariosEmp">
                <asp:UpdatePanel runat="server" ID="uplkbComentarios" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="lkbComentarios" CssClass="leyenda_indicador" Font-Size="Large" OnClick="lkbComentarios_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
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
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNombre">Nombre</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNombre" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNombre" runat="server" TabIndex="2" CssClass="textbox2x validate[required]" MaxLength="100"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoOperador">Tipo</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlTipoOperador" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoOperador" runat="server" CssClass="dropdown" TabIndex="3"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblCodAuth">Código Auth</label>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblCodAuth" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblCodAuth" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaNacimiento">Fecha Nacimiento</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaNacimiento" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" TabIndex="4" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtRFC">RFC</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtRFC" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtRFC" runat="server" TabIndex="5" CssClass="textbox validate[required, custom[RFC]]" MaxLength="13"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCURP">CURP</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCURP" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCURP" runat="server" TabIndex="6" CssClass="textbox validate[required, custom[CURP]]" MaxLength="18"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNSS">NSS</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNSS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNSS" runat="server" TabIndex="7" CssClass="textbox validate[required, custom[onlyNumberSp]]" MaxLength="11"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtRControl">R Control</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtRControl" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtRControl" runat="server" TabIndex="8" CssClass="textbox" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoLicencia">Tipo Licencia</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoLicencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoLicencia" runat="server" TabIndex="9" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoLicencia">No. Licencia</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNoLicencia" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoLicencia" runat="server" TabIndex="10" CssClass="textbox validate[required]" MaxLength="50"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtDireccion">Dirección</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtDireccion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="textbox2x" Enabled="false" TabIndex="11"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                            <asp:AsyncPostBackTrigger ControlID="ucDireccion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="validador">
                    <asp:UpdatePanel ID="uplnkVentana" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkVentana" runat="server" Text="Dirección" OnClick="lnkVentana_Click" TabIndex="12" CommandName="Direccion"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTelefono">Teléfono Móvil</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTelefono" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTelefono" runat="server" TabIndex="13" CssClass="textbox" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTelefonoCasa">Teléfono Casa</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtTelefonoCasa" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTelefonoCasa" runat="server" TabIndex="14" CssClass="textbox" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaIngreso">Fecha Ingreso</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaIngreso" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaIngreso" runat="server" TabIndex="15" CssClass="textbox validate[required, custom[date]]" MaxLength="10"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
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
                            <asp:TextBox ID="txtFechaBaja" runat="server" TabIndex="16" CssClass="textbox validate[custom[date]]" MaxLength="10" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="17" Enabled="false"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
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
                            <asp:TextBox ID="txtUbicacionActual" runat="server" TabIndex="18" CssClass="textbox2x" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
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
                            <asp:TextBox ID="txtFechaActualizacion" runat="server" TabIndex="19" CssClass="textbox" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="21" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" TabIndex="20" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>


        </div>

        <div class="columnafoto">
            <div class="foto">
                <asp:UpdatePanel ID="upimgOperador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Image ID="imgOperador" runat="server" ImageUrl="" Width="150px" Height="150px" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                        <asp:AsyncPostBackTrigger ControlID="lkbReactivar" />
                    </Triggers>
                </asp:UpdatePanel>


            </div>
        </div>

    </div>
    <div id="modalBajaOperador" class="modal">
        <div id="confirmacionBajaOperador" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <h3>Actualización de Baja de Operador</h3>
            </div>
            <div class="columna">
                <div class="renglon2x">
                    <label class="mensaje_modal">Confirme la Fecha de Baja para el Operador.</label>
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
                                <asp:AsyncPostBackTrigger ControlID="lkbBajaOperador" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="UPbtnCancelarBajaOperador" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarBajaOperador" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarBajaOperador_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarBajaOperador" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarBajaOperador" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarBajaOperador_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
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

    <!--Ventana modal que captura el periodo de contrato de un empleado-->
    <div id="contenedorModalContratoDefinido" class="modal">
        <div id="ModalContratoDefinido" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarContratoDefinido" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarContratoDefinido" runat="server" Text="Cerrar" OnClick="lkbCerrarVentana_Click" CommandName="CierraContratoDefinido">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/Calendario.png" />
                <h2>Fecha de Contrato Indeterminado</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPeriodoInicial">Inicio Periodo:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtPeriodoInicial" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtPeriodoInicial" CssClass="textbox  validate[required]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPeriodoFinal">Fin Periodo:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtPeriodoFinal" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtPeriodoFinal" CssClass="textbox  validate[required]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnImprimir" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnImprimir" Text="Imprimir" CssClass="boton" OnClick="btnImprimir_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <!--Fin ventana modal-->
    <!--Ventana modal direccion-->
    <div id="contenedorDireccionModal" class="modal">
        <div id="direccionModal" class="contenedor_modal_seccion_completa_arriba" style="width: 1197px; top: 15px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarImagenDireccion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="lkbCerrarImagenDireccion" OnClick="lkbCerrarVentana_Click" Text="Cerrar" CommandName="CierraDireccion">
             <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucDireccion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:WucDireccion ID="ucDireccion" runat="server" TabIndex="22" Enable="false" OnClickGuardarDireccion="ucDireccion_ClickGuardarDireccion"
                        OnClickEliminarDireccion="ucDireccion_ClickEliminarDireccion" OnClickSeleccionarDireccion="ucDireccion_ClickSeleccionarDireccion" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkVentana" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>

    <!--Ventana Calificacion-->
    <div id="contenedorVentanaCalificacion" class="modal">
        <div id="ventanaCalificacion" class="contenedor_modal_seccion_completa" style="left: 327px; width: 687px; top: 35px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCalificacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCalificacion" runat="server" Text="Cerrar" OnClick="lkbCerrarVentana_Click" CommandName="CierraCalificacion">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucCalificacion ID="wucCalificacion" runat="server" OnClickGuardarCalificacionGeneral="wucCalificacion_ClickGuardarCalificacionGeneral" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="imgbCalificacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Ventana Historial Calificacion-->
    <div id="contenedorVentanaHistorialCalificacion" class="modal">
        <div id="ventanaHistorialCalificacion" class="contenedor_modal_seccion_completa" style="left: 250px; width: 805px; top: 100px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorial" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarHistorial" runat="server" Text="Cerrar" OnClick="lkbCerrarVentana_Click" CommandName="CierraHistorialCalificacion">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucHistorialCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucHistorialCalificacion ID="wucHistorialCalificacion" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbComentarios" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--Ventana para Alta de Cuentas -->
    <div id="contenedorVentanaAltaCuentas" class="modal">
        <div id="ventanaAltaCuentas" class="contenedor_modal_seccion_completa_arriba" style="width: 800px; height: 390px">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarCuentas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarCuentas" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Cerrra" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="columna2x">
                <asp:UpdatePanel ID="upwucCuentaBancos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <tectos:wucCuentaBanco ID="wucCuentaBancos" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbCuentasBanco" />
                    </Triggers>
                </asp:UpdatePanel>
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
                    <div class="columna4x" style="width: 800px; height: 470px;" runat="server">
                        <ucl:wucProveedorGPSDiccionario runat="server" ID="wucProveedorGPSDiccionario" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbProveedorWS" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
