<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FacturacionOtrosV33.aspx.cs" Inherits="SAT.FacturacionElectronica33.FacturacionOtrosV33" %>
<%@ Register Src="~/UserControls/wucFacturadoV3_3.ascx" TagPrefix="tectos" TagName="wucFacturado" %>
<%@ Register Src="~/UserControls/wucFacturadoConceptoV33.ascx" TagPrefix="tectos" TagName="wucFacturadoConcepto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
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
                ConfiguraJQueryFacturacionOtrosV33();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraJQueryFacturacionOtrosV33() {
            $(document).ready(function () {
                //Cargando Catalogo de Autocompletado
                $("#<%=txtClienteReceptor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });

                //Función de Validación de Controles
                var validaFacturacionOtros = function (evt) {
                    var isValid1 = !$("#<%=txtCompaniaEmisor.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtClienteReceptor.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2;
                }

                //Asignando Validación al Evento Click del Boton
                $("#<%=btnGuardar.ClientID%>").click(validaFacturacionOtros);

                //Asignando Validación al Evento Click del Link
                $("#<%=lkbGuardar.ClientID%>").click(validaFacturacionOtros);
            });
        }
        //Invocando Función de Configuración
        ConfiguraJQueryFacturacionOtrosV33();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Facturacion.png" />
        <h1>Facturación Otros</h1>
    </div>
    <nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbNuevo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:UpdatePanel ID="uplkbGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li><li>
<asp:UpdatePanel ID="uplkbCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCancelar" runat="server" Text="Cancelar" OnClick="lkbElementoMenu_Click" CommandName="Cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
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
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbArchivos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbAyuda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" />
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbAcercaDe" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
</ul>
</li>
</ul>
</nav>
    <div class="contenedor_controles">
        <div class="header_seccion">
            <h2>Encabezado</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmisor">Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompaniaEmisor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompaniaEmisor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmisor">Cliente</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtClienteReceptor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtClienteReceptor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="upucFacturado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <tectos:wucFacturado ID="ucFacturado" runat="server" TabIndex="3" OnClickGuardarFactura="ucFacturado_ClickGuardarFactura" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ucFacturadoConcepto" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="contenedor_controles">
        <div class="header_seccion">
            <h2>Detalles</h2>
        </div>
        <asp:UpdatePanel ID="upucFacturadoConcepto" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <tectos:wucFacturadoConcepto ID="ucFacturadoConcepto" runat="server" TabIndex="4" OnClickEliminarFacturaConcepto="ucFacturadoConcepto_ClickEliminarFacturaConcepto"
                    OnClickGuardarFacturaConcepto="ucFacturadoConcepto_ClickGuardarFacturaConcepto" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ucFacturado" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                <asp:AsyncPostBackTrigger ControlID="lkbCancelar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
