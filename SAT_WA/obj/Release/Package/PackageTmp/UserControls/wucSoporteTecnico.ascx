<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucSoporteTecnico.ascx.cs" Inherits="SAT.UserControls.wucSoporteTecnico" %>
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario --> 
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar encabeados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
<script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQuerySoporteTecnico();
            }
        }
        //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQuerySoporteTecnico() {
        $(document).ready(function () {
            //Función de validación de campos
            var validacionSoporte = function (evt) {

                //Validando Controles
                var isValidP1 = !$("#<%=ddlTipoSoporte.ClientID%>").validationEngine('validate');
                var isValidP2 = !$("#<%=lblMensaje.ClientID%>").validationEngine('validate');
                var isValidP3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                var isValidP4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                var isValidP5 = !$("#<%=txtSolicita.ClientID%>").validationEngine('validate');
                var isValidP6;
                var isValidP7;
                var isValidP8;
                var isValidP9;

                return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8;
            };

            //Fecha de Captura
            $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Fecha de Reporte
            $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Autocompleta Usuario solicitante
            $("#<%=txtSolicita.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=61&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                
            });

            //Boton Aceptar
            $("#<%=btnGuardarSoporte.ClientID%>").click(validacionSoporte);

        })
    }
//Invocación Inicial de método de configuración JQuery
ConfiguraJQuerySoporteTecnico();
</script>
<asp:Panel ID="btnEnter" runat="server" DefaultButton="btnGuardarSoporte">
<div class="columna2x">
    <!--Controles-->
    <div class="header_seccion">
    <img src="../Image/Documentados.png" width="32" height="32"/>
    <h2>Soporte Técnico</h2>
    </div>
                <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label>Tipo:</label>
                </div>
                <div class="control2x">
                   <asp:UpdatePanel ID="upddlTipoSoporte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoSoporte" runat="server" CssClass="dropdown2x" OnSelectedIndexChanged="ddlTipoSoporte_SelectedIndexChanged" Enabled="false"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                </div>
        
                <div class="renglon2x" style="float: left;">
                    <asp:Panel ID="Panel1" runat="server">
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblSoporte" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblSoporte" runat="server" Text="Mensaje"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                            </asp:Panel>
                    <div class="etiqueta_320px" style="width:370px"> 
                        <asp:UpdatePanel ID="uplblMensaje" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMensaje" runat="server" CssClass="label_negrita" Text="Por Asignar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            <asp:Panel ID="Panel" runat="server" Visible="false">
                <div class="renglon2x" style="float: left;">
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblNvoOpe" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblNvoOpe" runat="server" Text="Nuevo Operador:"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="uplblNomOpe" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblNomOpe" runat="server" CssClass="label_negrita" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label>Fecha de Inicio:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                         </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label>Fecha de Fin:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label>Solicitado Por:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtSolicita" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSolicita" runat="server" CssClass="textbox2x validate[required]" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                         </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label>Observacion:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtObservacion" runat="server" CssClass="textbox2x" TabIndex="2" MaxLength="500"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                         </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
                <div class="renglon2x" style="float:left;">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelarSoporte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelarSoporte" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarSoporte_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardarSoporte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardarSoporte" runat="server" CssClass="boton" Text="Guardar" OnClick="btnGuardarSoporte_Click"/>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x" style="float:left;">
                    <asp:UpdatePanel ID="uplblErrorSoporte" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorSoporte" runat="server" CssClass="label_error" Width="450px"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarSoporte" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarSoporte" />
                            </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </asp:Panel>