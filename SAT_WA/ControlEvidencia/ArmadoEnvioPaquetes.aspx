<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArmadoEnvioPaquetes.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.ControlEvidencia.ArmadoEnvioPaquetes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos -->
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryArmadoEnvioPaquete();
            }
        }
        //Creando función para configuración de jquery en formulario
        function ConfiguraJQueryArmadoEnvioPaquete() {

            //Validación campos
            $(document).ready(function () {
                //Función de validación Armado y Envió de Paquete
                var validacionArmadoEnvioPaquete = function (evt) {
                    //Validando sólo contenido de controles de interés (por separado para visualizar todos los mensajes de error a la vez)
                    var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
        return isValid1
    };
    //Menú Guardar
    $("#<%= lkbGuardar.ClientID %>").click(validacionArmadoEnvioPaquete);
    //Botón Guardar
    $("#<%= btnGuardar.ClientID %>").click(validacionArmadoEnvioPaquete);

    //Quitando cualquier manejador de evento click añadido previamente
    $("#<%= lkbEnviar.ClientID%>").unbind("click");
    $("#<%= lkbEnviar.ClientID%>").click(function () {
        //Mostrando ventana modal 
        $("#contenidoConfirmacionEnviar").animate({ width: "toggle" });
        $("#confirmacionEnviar").animate({ width: "toggle" });
    });
    //Quitando cualquier manejador de evento click añadido previamente
    $("#<%= lkbEliminar.ClientID%>").unbind("click");
    $("#<%= lkbEliminar.ClientID%>").click(function () {
        //Mostrando ventana modal 
        $("#contenidoConfirmacionEliminar").animate({ width: "toggle" });
        $("#confirmacionEliminar").animate({ width: "toggle" });
    });
    //Quitando cualquier manejador de evento click añadido previamente
    $("#<%= btnCancelarEnviar.ClientID%>").unbind("click");
    $("#<%= btnCancelarEnviar.ClientID%>").click(function (evt) {
        evt.preventDefault()
        //Ocultando ventana modal 
        $("#contenidoConfirmacionEnviar").animate({ width: "toggle" });
        $("#confirmacionEnviar").animate({ width: "toggle" });
    });
    //Quitando cualquier manejador de evento click añadido previamente
    $("#<%= btnCancelarEliminar.ClientID%>").unbind("click");
    $("#<%= btnCancelarEliminar.ClientID%>").click(function (evt) {
        evt.preventDefault()
        //Ocultando ventana modal 
        $("#contenidoConfirmacionEliminar").animate({ width: "toggle" });
        $("#confirmacionEliminar").animate({ width: "toggle" });
    });
    // *** Fecha de Inicio, Fin (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
    $(document).ready(function () {
        $("#<%=txtFechaInicioB.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        timepicker: false,
    });
    $("#<%=txtFechaFinB.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        timepicker: false,
    });
});

    <%--//Añadiendo Encabezado Fijo
    $("#<%=gvDocumentosA.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorDocumentosA").offsetWidth - 15,
        height: 400
    });

    //Añadiendo Encabezado Fijo
    $("#<%=gvDocumentosB.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorDocumentosB").offsetWidth - 15,
        height: 400
    });

    //Añadiendo Encabezado Fijo
    $("#<%=gvBusqueda.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorPaquetesEncontrados").offsetWidth - 15,
        height: 400
    });--%>

});


        }

        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryArmadoEnvioPaquete();
    </script>

    <div id="encabezado_forma">
        <img src="../Image/EnvioEvidencia.png" />
        <h1>Envio de evidencias</h1>
    </div>
    <nav id="menuForma">
        <ul>
            <li class="green">
                <a href="#" class="fa fa-floppy-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbBuscar" runat="server" Text="Buscar" OnClick="lkbElementoMenu_Click" CommandName="Buscar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbNuevo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbGuardar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbImprimir" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="red">
                <a href="#" class="fa fa-pencil-square-o"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbEditar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbEnviar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbEnviar" runat="server" Text="Enviar" CommandName="Enviar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="blue">
                <a href="#" class="fa fa-cog"></a>
                <ul>
                    <li>
                        <asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                    <li>
                        <asp:UpdatePanel ID="uplkbReferencias" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </li>
                </ul>
            </li>
            <li class="yellow">
                <a href="#" class="fa fa-question-circle"></a>
                <ul>
                    <li>
                        <asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
                    <li>
                        <asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
                </ul>
            </li>
        </ul>
    </nav>

    <div class="contenedor_botones_pestaña">
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnArmadoEdicion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnArmadoEdicion" Text="Armado y Edición" OnClick="btnVista_OnClick" CommandName="ArmadoEdicion" runat="server" CssClass="boton_pestana_activo" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
                    <asp:AsyncPostBackTrigger ControlID="gvBusqueda" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnBusqueda" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnBusqueda" Text="Búsqueda de Paquetes" OnClick="btnVista_OnClick" runat="server" CommandName="Busqueda" CssClass="boton_pestana" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnArmadoEdicion" />
                    <asp:AsyncPostBackTrigger ControlID="gvBusqueda" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_tabs_armado">
        <asp:UpdatePanel ID="upmtvPaquetes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mtvPaquetes" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwArmadoEdicion" runat="server">
                        <div class="vista_armado_paquetes">
                            <div class="header_vista_armado">
                                <img src="../Image/ArmadoPaquete.png" />
                                <h2>Armar paquete de evidencias</h2>
                            </div>
                            <div class="datos_paquete">
                                <div class="columna2x">
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <asp:UpdatePanel ID="uplblOrigen" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblOrigen" runat="server" CssClass="Label">Origen</asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlOrigen" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlOrigen" runat="server" TabIndex="3" CssClass="dropdown2x" OnSelectedIndexChanged="ddlOrigen_OnSelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlDestino">
                                                Destino</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlDestino" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDestino" runat="server" TabIndex="4" CssClass="dropdown2x" OnSelectedIndexChanged="ddlDestino_OnSelectedIndexChanged"
                                                        AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlMedioEnvio">Medio Envio</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlMedioEnvio" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlMedioEnvio" runat="server" TabIndex="6" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="txtReferencia">Referencia</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="upddlReferencia" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]" TabIndex="7" MaxLength="150"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlEstatus">
                                                Estatus</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlEstatus" runat="server" TabIndex="5" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="columna2x">
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="lblFecSal">
                                                Fecha Salida 
                                            </label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uplblFecSal" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblFecSal" runat="server" CssClass="LabelResalta"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="lblFecLle">
                                                Fecha Llegada 
                                            </label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uplblFecLle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblFecLle" runat="server" CssClass="LabelResalta"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon">
                                        <div class="controlBoton">
                                            <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnCancelar" runat="server" TabIndex="9" CssClass="boton" CausesValidation="false"
                                                        OnClick="btnGuardar_OnClick" Text="Cancelar" CommandName="Cancelar" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="controlBoton">
                                            <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGuardar" runat="server" TabIndex="8" CssClass="boton" CommandName="Guardar"
                                                        OnClick="btnGuardar_OnClick" Text="Guardar" ValidationGroup="Paquete" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="control">
                                            <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblId" runat="server" CssClass="LabelResalta" Visible="false">ID</asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtCompania" runat="server" TabIndex="1" Enabled="false" CssClass="textbox2x" Visible="false">
                                                    </asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="control2x">
                                        <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                </div>
                            </div>
                            <div class="armado_paquete">
                                <div class="documentos_disponibles">
                                    <div class="header_vista_armado">
                                        <img src="../Image/documento.png" />
                                        <h2>Documentos por enviar</h2>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlTamanoDocumentosA">Mostrar</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="upddlTamanoDocumentosA" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>

                                                    <asp:DropDownList ID="ddlTamanoDocumentosA" runat="server" AutoPostBack="True" CssClass="dropdown_100px"
                                                        TabIndex="10" OnSelectedIndexChanged="ddlTamanoDocumentosA_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div class="etiqueta">
                                            <asp:UpdatePanel ID="uplblOrdenarDocumentosA" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Ordenar:
                                                    <asp:Label ID="lblOrdenarDocumentosA" runat="server" CssClass="LabelResalta"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvDocumentosA" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="etiqueta">
                                            <asp:UpdatePanel ID="uplkbExportarDocumentosA" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lkbExportarDocumentosA" runat="server" TabIndex="11" OnClick="lkbExportarDocumentosA_OnClick"
                                                        Text="Exportar Excel"></asp:LinkButton>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="lkbExportarDocumentosA" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="grids_armado_envio" id="contenedorDocumentosA">
                                        <asp:UpdatePanel ID="upgvDocumentosA" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvDocumentosA" runat="server" AllowPaging="True" TabIndex="12" PageSize="25"
                                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvDocumentosA_PageIndexChanging"
                                                    OnSorting="gvDocumentosA_Sorting" ShowFooter="True" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkTodosDocumentos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosDocumentos_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblSeleccionadosA" runat="server" CssClass="LabelResalta" Text="0"></asp:Label><br />
                                                                Sel.
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkVariosDocumentos" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosDocumentos_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje" />
                                                        <asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClienteDocumento" runat="server" Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Cliente").ToString(), 15, "...") %>'
                                                                    ToolTip='<%# Eval("Cliente") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
                                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                                        <asp:BoundField DataField="Dias" HeaderText="Dias" SortExpression="Dias">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel ID="uplkbReferenciaDoc" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:LinkButton ID="lkbReferenciaDoc" runat="server" Enabled='<%#(TSDK.ASP.Pagina.Estatus)Session["estatus"] == TSDK.ASP.Pagina.Estatus.Lectura ? false:true%>'
                                                                            OnClick="lkbReferenciaDoc_Click">Referencias</asp:LinkButton>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="lkbReferenciaDoc" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
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
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentosA" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="columna_botones">
                                    <div class="renglon_botones"></div>
                                    <div class="renglon_botones"></div>
                                    <div class="renglon_botones"></div>
                                    <div class="renglon_botones"></div>
                                    <div class="renglon_botones">
                                        <asp:UpdatePanel ID="upbtnAgregarDoc" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAgregarDoc" runat="server" CssClass="boton" TabIndex="13" OnClick="btnActualizacion_OnClick"
                                                    Text="Agregar >" CommandName="Agregar" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="renglon_botones">
                                        <asp:UpdatePanel ID="upbtnAgregarTodos" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAgregarTodos" runat="server" CssClass="boton" OnClick="btnActualizacion_OnClick"
                                                    Text="Todos >>" TabIndex="14" CommandName="AgregarTodos" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div class="renglon_botones">
                                        <asp:UpdatePanel ID="upbtnQuitarTodos" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnQuitarTodos" runat="server" CssClass="boton_cancelar" OnClick="btnActualizacion_OnClick"
                                                    Text="<< Todos" TabIndex="19" CommandName="QuitarTodos" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="renglon_botones">
                                        <asp:UpdatePanel ID="upbtnQuitarDoc" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnQuitarDoc" runat="server" CssClass="boton_cancelar" TabIndex="18" OnClick="btnActualizacion_OnClick"
                                                    Text="< Quitar" CommandName="Quitar" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                </div>
                                <div class="documentos_agregados">
                                    <div class="header_vista_armado">
                                        <img src="../Image/PaqueteAgregar.png" />
                                        <h2>Documentos en paquete</h2>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlTamanoDocumentosB">Mostrar</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="upddlTamanoDocumentosB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlTamanoDocumentosB" runat="server" AutoPostBack="True" CssClass="dropdown_100px"
                                                        TabIndex="15" OnSelectedIndexChanged="ddlTamanoDocumentosB_OnSelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="etiqueta">
                                            <asp:UpdatePanel ID="uplblOrdenarDocumentosB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    Ordenar:
                                                    <asp:Label ID="lblOrdenarDocumentosB" runat="server" CssClass="LabelResalta"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvDocumentosB" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="etiqueta">
                                            <asp:UpdatePanel ID="uplkbExportarDocumentoB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:LinkButton ID="lkbExportarDocumentoB" runat="server" TabIndex="16" OnClick="lkbExportarDocumentoB_OnClick"
                                                        Text="Exportar Excel" CausesValidation="False"></asp:LinkButton>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="lkbExportarDocumentoB" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="grids_armado_envio" id="contenedorDocumentosB">
                                        <asp:UpdatePanel ID="upgvDocumentosB" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvDocumentosB" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                                                    AutoGenerateColumns="False" CssClass="gridview" OnPageIndexChanging="gvDocumentosB_PageIndexChanging"
                                                    OnSorting="gvDocumentosB_Sorting" ShowFooter="True" TabIndex="17" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkTodosPaquetes" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosPaquetes_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblSeleccionadosB" runat="server" CssClass="LabelResalta" Text="0"></asp:Label><br />
                                                                Sel.
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkVariosPaquetes" runat="server" AutoPostBack="true" OnCheckedChanged="chkTodosPaquetes_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Viaje" HeaderText="Viaje" SortExpression="Viaje" />
                                                        <asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClienteDocumentoPaquete" runat="server" Text='<%#  TSDK.Base.Cadena.TruncaCadena(Eval("Cliente").ToString(), 15, "...") %>'
                                                                    ToolTip='<%# Eval("Cliente") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
                                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                                        <asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha Rec." SortExpression="FechaRecepcion"
                                                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />
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
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentosB" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </div>
                                    <div class="control2x" style="width: auto; height: auto">
                                        <asp:UpdatePanel ID="uplblError2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblError2" runat="server" CssClass="label_error"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlOrigen" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlDestino" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAgregarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarDoc" />
                                                <asp:AsyncPostBackTrigger ControlID="btnQuitarTodos" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </asp:View>
                    <asp:View ID="vwBusqueda" runat="server">
                        <div class="vista_armado_paquetes">
                            <div class="header_vista_armado">
                                <img src="../Image/Buscar.png" />
                                <h2>Buscar paquete de evidencias</h2>
                            </div>
                            <div class="datos_paquete">
                                <div class="columna2x">
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlOrigenB">
                                                Origen</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlOrigenB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlOrigenB" runat="server" TabIndex="3" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlDestinoB">
                                                Destino</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlDestinoB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlDestinoB" runat="server" TabIndex="4" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlMedioEnvioB">
                                                Medio Envio</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlMedioEnvioB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlMedioEnvioB" runat="server" TabIndex="6" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="txtReferenciaB">
                                                Referencia</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="uptxtReferenciaB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtReferenciaB" runat="server" CssClass="textbox2x" TabIndex="7" MaxLength="150"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta">
                                            <label for="ddlEstatusB">
                                                Estatus</label>
                                        </div>
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="upddlEstatusB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlEstatusB" runat="server" TabIndex="5" CssClass="dropdown2x">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="columna2x">
                                    <div class="renglon2x">
                                        <div class="control">
                                            <asp:UpdatePanel ID="upchkFechasBusqueda" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkFechasBusqueda" runat="server" AutoPostBack="true" Text="Filtrar por"
                                                        CssChecked="false" TabIndex="8" OnCheckedChanged="chkFechasBusqueda_CheckedChanged" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uprdbFechaSalida" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:RadioButton ID="rdbFechaSalida" runat="server" GroupName="FechaBusqueda" Text="Fecha Salida"
                                                        CssClass="LabelResalta" Checked="true" TabIndex="9" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkFechasBusqueda" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uprdbFechaLlegada" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:RadioButton ID="rdbFechaLlegada" runat="server" GroupName="FechaBusqueda" Text="Fecha Llegada"
                                                        CssClass="LabelResalta" TabIndex="10" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkFechasBusqueda" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="etiqueta" style="width: auto">
                                            <label for="txtFechaInicioB" class="Label">
                                                Desde</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uptxtFechaInicioB" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtFechaInicioB" Enabled="false" runat="server" CssClass="textbox validate[required, custom[date]]" TabIndex="11"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkFechasBusqueda" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="etiqueta" style="width: auto">
                                            <label for="txtFechaFinB" class="Label">
                                                Hasta</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uptxtFechaFinB" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtFechaFinB" runat="server" CssClass="textbox validate[required, custom[date]]" Enabled="false" TabIndex="12"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkFechasBusqueda" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="control2x">
                                            <asp:UpdatePanel ID="CompaniaB" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtCompaniaB" runat="server" TabIndex="1" CssClass="dropdown2x" Enabled="false" Visible="false"
                                                        AutoPostBack="true">
                                                    </asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="renglon2x">
                                        <div class="controlBoton">
                                            <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnBuscar" runat="server" TabIndex="13" CssClass="boton" OnClick="btnBuscar_OnClick"
                                                        Text="Buscar" ValidationGroup="Busqueda" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="chkFechasBusqueda" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="paquete_encontrados">
                                <div class="header_vista_armado">
                                    <img src="../Image/PaqueteAgregar.png" />
                                    <h2>Paquetes encontrados</h2>
                                </div>
                                <div class="renglon3x">
                                    <div class="etiqueta">
                                        <label for="ddlTamañoGridViewBusqueda">Mostrar</label>
                                    </div>
                                    <div class="control">
                                        <asp:DropDownList ID="ddlTamañoGridViewBusqueda" runat="server" CssClass="dropdown"
                                            TabIndex="14" AutoPostBack="True" OnSelectedIndexChanged="ddlTamañoGridViewBusqueda_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="etiqueta">
                                        <label for="lblOrdenarServicios">Ordenado Por:</label>
                                    </div>
                                    <div class="etiqueta">
                                        <asp:UpdatePanel ID="uplblCriterioGridViewBusqueda" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblCriterioGridViewBusqueda" runat="server"></asp:Label>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvBusqueda" EventName="Sorting" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="etiqueta">
                                        <asp:UpdatePanel ID="uplkbExportarExcelBusqueda" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="lkbExportarExcelBusqueda" runat="server" Text="Exportar Excel"
                                                    OnClick="lkbExportarBusqueda_Onclick" TabIndex="15"></asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lkbExportarExcelBusqueda" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="grid_paquetes_encontrados" id="contenedorPaquetesEncontrados ">
                                    <asp:UpdatePanel runat="server" ID="upgvBusqueda" RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:GridView runat="server" ID="gvBusqueda" OnSorting="gvBusqueda_Onsorting" AllowSorting="True"
                                                CssClass="gridview" AllowPaging="True" OnPageIndexChanging="gvBusqueda_OnpageIndexChanging" PageSize="25"
                                                ShowFooter="True" AutoGenerateColumns="False" TabIndex="16" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Id" SortExpression="Id">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="uplkbPaquete" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:LinkButton ID="lkbPaquete" runat="server" CausesValidation="False" OnClick="lkbPaquete_Click"
                                                                        Text='<%# Eval("Id") %>' CommandName="Abrir"></asp:LinkButton>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="lkbPaquete" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" ItemStyle-HorizontalAlign="Left">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                                    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                                    <asp:BoundField DataField="FechaSalida" HeaderText="Fecha Salida" SortExpression="FechaSalida"
                                                        DataFormatString="{0:yyyy/MM/dd HH:mm}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Llegada" SortExpression="FechaLlegada"
                                                        DataFormatString="{0:yyyy/MM/dd HH:mm}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Medio" HeaderText="Medio" SortExpression="Medio" />
                                                    <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                                                    <asp:BoundField DataField="Documentos" HeaderText="Documentos" SortExpression="Documentos">
                                                        <ItemStyle HorizontalAlign="Center" />
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
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewBusqueda" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnArmadoEdicion" />
                <asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarEliminar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarEnviar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbBuscar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div id="contenidoConfirmacionEnviar" class="modal">
        <div id="confirmacionEnviar" class="contenedor_ventana_confirmacion">
            <div class="header_resumen_documentos_segmento">
                <img src="../Image/Exclamacion.png" />
                <h3>Enviar Documentos</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">Los documentos serán envíados al destino indicado ¿Desea continuar?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:Button ID="btnCancelarEnviar" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarEnviar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarEnviar" runat="server" OnClick="btnAceptarEnviar_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="contenidoConfirmacionEliminar" class="modal">
        <div id="confirmacionEliminar" class="contenedor_ventana_confirmacion">
            <div class="header_resumen_documentos_segmento">
                <img src="../Image/Exclamacion.png" />
                <h3>Eliminar paquete</h3>
            </div>
            <div class="columna2x">
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <label class="mensaje_modal">Los documentos quedarán disponibles para enviar en otro paquete ¿Desea continuar?</label>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:Button ID="btnCancelarEliminar" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarEliminar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarEliminar" runat="server" OnClick="btnAceptarEliminar_Click" CssClass="boton" Text="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
