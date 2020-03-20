<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatosOperador.aspx.cs" Inherits="SAT.Accesorios.DatosOperador" %>

<%@ Register Src="~/Externa/wucCalificacion.ascx" TagPrefix="uc" TagName="wucCalificacion" %>
<%@ Register Src="~/Externa/wucHistorialCalificacion.ascx" TagPrefix="uc1" TagName="wucHistorialCalificacion" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!-- Estilos de los Controles -->
    <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/MenuPrincipal.css" rel="stylesheet" />
    <link href="../CSS/MenuUsuario.css" rel="stylesheet" />
    <!-- Estilos de Validación, DateTimePicker, MasketTextBox -->
    <link href="../CSS/jquery.validationEngine.css" type="text/css" rel="stylesheet" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/animate.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery-ui.css" rel="stylesheet" type="text/css" />
    <!-- Libreiras de Validación, DateTimePicker, MasketTextBox -->
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery-ui.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery.validationEngine.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery.validationEngine-es.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery.noty.packaged.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/jquery.noty.packaged.min.js" type="text/javascript" charset="utf-8"></script>
    <script src="../Scripts/gridviewScroll.min.js" type="text/javascript" charset="utf-8"></script>
    <script src='<%=ResolveUrl("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
    <style type="text/css">
        .columnaDO {
            margin: 0px;
            padding: 0px;
            width: 912px;
            float: left;
        }

        .Etiquetas {
            font-size: medium;
            font-weight: bold;
            width: 92px;
            height: 23px;
            float: left;
        }

        .EtiquetasPeq {
            font-size: medium;
            font-weight: bold;
            width: 54px;
            height: 23px;
            float: left;
        }

        .Etiqueta {
            font-size: medium;
            font-weight: bold;
            margin: 0px;
            padding: 5px 0px 0px 0px;
            width: 300px;
            height: 18px;
            float: left;
        }

        Etique {
            font-size: medium;
            font-weight: bold;
            width: 33px;
            height: 23px;
            float: left;
        }

        .foto {
            padding: 0px 0px 0px 7px;
            width: 270px;
            float: left;
        }

        .Direccion {
            width: 635px;
            height: 48px;
        }

        .RFC {
            width: 122px;
            height: 18px;
            float: left;
        }

        .ventanaCalificacion {
            padding: 0px;
            z-index: 1002;
            position: fixed;
            background-color: #ffffff;
            border: 1px solid #808080;
            left: 127px;
            top: 13px;
            width: 687px;
            height: auto;
        }

        .prom {
            margin: 0px 0px 0px 0px;
            width: 165px;
            height: 29px;
            float: left;
        }

        .comentarios {
            margin: 0px;
            padding: 8px 0px 0px 0px;
            width: 238px;
            height: 18px;
            float: left;
        }

        .nom {
            margin: 0px 0px 0px 0px;
            width: 496px;
            height: 35px;
            float: left;
        }
    </style>
    <!--FinEstilo-->
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
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
        <div class="seccion_controles">
            <div class="columnaDO">
                <div class="header_seccion">
                    <div class="nom">
                        <asp:Label runat="server" ID="lblNombre" CssClass="label_negrita" Font-Size="X-Large"></asp:Label>
                    </div>
                    <div class="prom">
                        <asp:UpdatePanel runat="server" ID="upimgbCalificacion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:ImageButton runat="server" ID="imgbCalificacion" ImageUrl="" OnClick="imgbCalificacion_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="comentarios">
                        <asp:UpdatePanel runat="server" ID="uplkbComentarios" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton runat="server" ID="lkbComentarios" CssClass="leyenda_indicador" Font-Size="Large" OnClick="lkbComentarios_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrarCalificacion" />
                                <asp:AsyncPostBackTrigger ControlID="wucCalificacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="foto">
                <asp:Image ID="imgOperador" runat="server" ImageUrl="" Width="256px" Height="256px" />
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="Etiquetas">
                        <label for="lblTipoLicencia">Licencia Tipo</label>
                    </div>
                    <div class="control_80px">
                        <asp:Label runat="server" ID="lblTipoLicencia" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="Etiquetas">
                        <label for="lblNoLicencia">No.Licencia</label>
                    </div>
                    <div class="control_100px">
                        <asp:Label runat="server" ID="lblNoLicencia" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="control_100px">
                        <asp:Label runat="server" ID="lblVencimiento" CssClass="label_negrita" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="control">
                        <asp:Label runat="server" ID="lblFechaVencimiento" Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                <div class="renglon3x">
                    <div class="Etiqueta">
                        <label for="lblDireccion">Dirección:</label>
                    </div>
                    <div class="etiqueta_155px">
                    </div>
                </div>
                <div class="Direccion">
                    <asp:Label runat="server" ID="lblDireccion" Font-Size="Small"></asp:Label>
                </div>
                <div class="renglon3x">
                    <div class="EtiquetasPeq">
                        <label for="lblCelular">Celular</label>
                    </div>
                    <div class="control_100px">
                        <asp:Label runat="server" ID="lblCelular" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="EtiquetasPeq">
                        <label for="lblTelefonoCasa">Casa</label>
                    </div>
                    <div class="control_100px">
                        <asp:Label runat="server" ID="lblTelefonoCasa" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="Etiquetas">
                        <label for="RControl">R-Control</label>
                    </div>
                    <div class="control_100px">
                        <asp:Label runat="server" ID="lblRControl" Font-Size="Medium"></asp:Label>
                    </div>

                </div>
                <div class="renglon3x">
                    <div class="EtiquetasPeq">
                        <label for="lblNSS">NSS</label>
                    </div>
                    <div class="RFC">
                        <asp:Label runat="server" ID="lblNSS" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="EtiquetasPeq">
                        <label for="lblRFC">RFC</label>
                    </div>
                    <div class="RFC">
                        <asp:Label runat="server" ID="lblRFC" Font-Size="Medium"></asp:Label>
                    </div>
                    <div class="EtiquetasPeq">
                        <label for="lblCURP">CURP</label>
                    </div>
                    <div class="RFC">
                        <asp:Label runat="server" ID="lblCURP" Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                <div class="renglon3x">
                    <div class="Etiqueta">
                        <label for="lblCodAutorizacion">Código Móvil Autorización</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:Label runat="server" ID="lblCodAutorizacion" Font-Size="Medium"></asp:Label>
                    </div>
                </div>
                <div class="renglon3x">
                    <div class="Etiqueta">
                        <label for="lblUbicacionActual">Ubicación Actual:</label>
                    </div>
                    <div class="etiqueta_155px">
                    </div>
                </div>

                <div class="Direccion">
                    <asp:Label runat="server" ID="lblUbicacionActual" Font-Size="Small"></asp:Label>
                </div>
            </div>
        </div>
        <!--Ventana Calificacion-->
        <div id="contenedorVentanaCalificacion" class="modal">
            <div id="ventanaCalificacion" class="contenedor_modal_seccion_completa" style="left: 127px; width: 687px; top: 13px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarCalificacion" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarCalificacion" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraCalificacion">
                            <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="upwucCalificacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc:wucCalificacion ID="wucCalificacion" runat="server" OnClickGuardarCalificacionGeneral="wucCalificacion_ClickGuardarCalificacionGeneral"/>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="imgbCalificacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <!--Fin Ventana Modal Calificacion-->
        <!--Ventana Historial Calificacion-->
        <div id="contenedorVentanaHistorialCalificacion" class="modal">
            <div id="ventanaHistorialCalificacion" class="contenedor_modal_seccion_completa" style="left:68px; width:805px; top:95px;">
                <div class="boton_cerrar_modal">
                    <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorial" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lkbCerrarHistorial" runat="server" Text="Cerrar" OnClick="lkbCerrarCalificacion_Click" CommandName="CierraHistorialCalificacion">
                            <img src="../Image/Cerrar16.png" />
                            </asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="upwucHistorialCalificacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:wucHistorialCalificacion ID="wucHistorialCalificacion" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbComentarios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
