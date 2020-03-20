<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucKilometraje.ascx.cs" Inherits="SAT.UserControls.wucKilometraje" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            //Invocando Función de Configuración
            ConfiguraJQueryUCKilometraje();
        }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryUCKilometraje() {
        $(document).ready(function () {

            //Función de Validación en el Evento Click
            $("#<%=btnAceptar.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtUbicacionOrigen.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtUbicacionDestino.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtKms.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtTiempo.ClientID%>").validationEngine('validate');
                var isValid5;
                var isValid6;

                //Obteniendo Valor del Indicador
                var validaIndicador = $("#<%=chkIndicador.ClientID%>").is(':checked');

                //Validando Indicador
                if (validaIndicador == true)
                {
                    //Asignando Validación Positiva
                    isValid5 = true;
                    isValid6 = true;
                    //alert('No hay seleccion');
                }
                else
                {
                    //Validando Controles
                    isValid5 = !$("#<%=txtKmsInv.ClientID%>").validationEngine('validate');
                    isValid6 = !$("#<%=txtTiempoInv.ClientID%>").validationEngine('validate');
                   // alert('Si hay seleccion');
                }

                //alert('1' + isValid1 + '<br />2' + isValid2 + '<br />3' + isValid3 + '<br />4' + isValid4 + '<br />5' + isValid5 + '<br />6' + isValid6);
                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6;
            });

            //Catalogos de Ubicación
            $("#<%=txtUbicacionOrigen.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
            });
            $("#<%=txtUbicacionDestino.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                appendTo: "<%=this.Contenedor%>"
            });

            //Validación de Controles de Ventana Modal
            $("#<%=lnkOtrosKmsReal.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtKms.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1;
            });

            //Configurando Validación de Kms de pago y cobro
            $("#<%=btnConfirmarKms.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtKmsPago.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtKmsCobro.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2;
            });

            //Configurando Validación de Kms de pago y cobro
            $("#<%=btnConfirmarKmsInv.ClientID%>").click(function () {
                //Validando Controles
                var isValid1 = !$("#<%=txtKmsInvPago.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtKmsInvCobro.ClientID%>").validationEngine('validate');

                //Devolviendo Resultado Obtenido
                return isValid1 && isValid2;
            });

        });
    }

    //Invocando Función de Configuración
    ConfiguraJQueryUCKilometraje();
</script>
<div class="seccion_controles_modal">
    <div class="header_seccion">
        <img src="../Image/EnTransito.png" />
        <h2>Kilometraje</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtUbicacionOrigen">Origen</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtUbicacionOrigen" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUbicacionOrigen" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtUbicacionDestino">Destino</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtUbicacionDestino" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUbicacionDestino" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlRuta">Ruta</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlRuta" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlRuta" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="txtKms">Kms</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtKms" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtKms" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="txtKmsMaps">Kms (Maps)</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtKmsMaps" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtKmsMaps" runat="server" CssClass="textbox_100px" Enabled="false" ></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCalcular" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkCalcular" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCalcular" runat="server" Text="Calcular" OnClick="lnkCalcular_Click"></asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="txtTiempo">Tiempo</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTiempo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTiempo" runat="server" CssClass="textbox_100px validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCalcular" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="txtTiempoMaps">Tiempo (Maps)</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTiempoMaps" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTiempoMaps" runat="server" CssClass="textbox_100px" Enabled="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label>min(s)</label>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkOtrosKmsReal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkOtrosKmsReal" runat="server" Text="Otros KMS" OnClick="lnkOtrosKms_Click"></asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_320px">
                <asp:UpdatePanel ID="upchkIndicador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkIndicador" runat="server" Text="¿Mismo Kilometraje de Regreso?" AutoPostBack="true"
                            OnCheckedChanged="chkIndicador_CheckedChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_50px">
                <label for="txtKmsInv">Kms</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtKmsInv" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtKmsInv" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="chkIndicador" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="txtTiempoInv">Tiempo</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtTiempoInv" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtTiempoInv" runat="server" CssClass="textbox_100px validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="chkIndicador" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <label>min(s)</label>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplnkOtrosKmsInv" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkOtrosKmsInv" runat="server" Text="Otros KMS (Inverso)" OnClick="lnkOtrosKms_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="chkIndicador" />
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
                    <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" />
                    </ContentTemplate>
                    <Triggers>
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
                        <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>
<div id="contenidoVentanaKmsPagoCobro" class="modal" >
    <div id="ventanaKmsPagoCobro" class="contenedor_ventana_confirmacion">
        <div class="seccion_controles">
            <div class="header_seccion">
                <h2>Kilometraje de Pago y Cobro</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="upchkKmsPC" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkKmsPC" runat="server" Text="Respetar el Kilometraje Real" AutoPostBack="true"
                                OnCheckedChanged="chkKmsPC_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsReal" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="txtKmsPago">Kms Pago</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtKmsPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtKmsPago" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsReal" />
                                <asp:AsyncPostBackTrigger ControlID="chkKmsPC" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="txtKmsCobro">Kms Cobro</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtKmsCobro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtKmsCobro" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsReal" />
                                <asp:AsyncPostBackTrigger ControlID="chkKmsPC" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarKms" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarKms" runat="server" Text="Confirmar" CssClass="boton" OnClick="btnConfirmarKms_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsReal" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="contenidoVentanaKmsInvPagoCobro" class="modal" >
    <div id="ventanaKmsInvPagoCobro" class="contenedor_ventana_confirmacion">
        <div class="seccion_controles">
            <div class="header_seccion">
                <h2>Kilometraje de Pago y Cobro (Inverso)</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <asp:UpdatePanel ID="upchkKmsInvPC" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkKmsInvPC" runat="server" Text="Respetar el Kilometraje Real" AutoPostBack="true"
                                OnCheckedChanged="chkKmsPC_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsInv" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="txtKmsInvPago">Kms Pago</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtKmsInvPago" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtKmsInvPago" runat="server" CssClass="textbox_100px validate[required]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="chkKmsInvPC" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsInv" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="txtKmsInvCobro">Kms Cobro</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="uptxtKmsInvCobro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtKmsInvCobro" runat="server" CssClass="textbox_100px"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="chkKmsInvPC" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsInv" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnConfirmarKmsInv" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnConfirmarKmsInv" runat="server" Text="Confirmar" CssClass="boton" OnClick="btnConfirmarKms_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lnkOtrosKmsInv" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
