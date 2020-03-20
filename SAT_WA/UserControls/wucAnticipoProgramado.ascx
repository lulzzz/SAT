<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAnticipoProgramado.ascx.cs" Inherits="SAT.UserControls.wucAnticipoProgramado" %>

<script type='text/javascript'>
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryUCDeposito();
        }
    }

    //Creando función para configuración de jquery en formulario
    function ConfiguraJQueryUCDeposito() {
        $(document).ready(function () {
            //Validación 
            var validacionFechaEjecucion = function () {

                var isValidP1 = !$("#<%=txtFechaProgramada.ClientID%>").validationEngine('validate');

                return isValidP1;
            };
            //Validación de campos requeridos
            $("#<%=this.btnRegistrar.ClientID%>").click(validacionFechaEjecucion);
        });

        $("#<%=txtFechaProgramada.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y',
            timepicker: false
        });

    }
    
    //Invocando función de Configuración
    ConfiguraJQueryUCDeposito();
</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/Calendar.jpg" style="width: 32px;"/>
        <h2>Programar Asignación de Depósitos</h2>
    </div>
    <div class="columna3x">
        <div class="renglon">
            <div class="etiqueta">
                <label for="lblNoDeposito">No. Depósito</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblNoDeposito" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblNoDeposito" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtEstatus">Estatus</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtEstatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtEstatus" runat="server" CssClass="textbox "></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="lblBeneficiario">Beneficiario</label>
            </div>
            <asp:UpdatePanel ID="upmtvBeneficiario" runat="server">
                <ContentTemplate>
                    <asp:MultiView ID="mtvBeneficiario" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vwlblbeneficiario" runat="server">
                            <div class="control2x">
                                <asp:UpdatePanel ID="uplblBeneficiario" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblBeneficiario" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                        <asp:View ID="vwddlbeneficiario" runat="server">
                            <div class="control2x">
                                <asp:UpdatePanel ID="upddlBeneficiario" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBeneficiario" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlBeneficiario_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtOperadorUnidad">Operador/Unidad</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtOperadorUnidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtOperadorUnidad" runat="server" CssClass="textbox2x validate[required]" MaxLength="150"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlBeneficiario" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlConcepto">Concepto</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown2x" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlConcepto_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x" runat="server" id="divUbicaciones" visible="false">
            <div class="etiqueta">
                <label for="ddlUbicaciones">E. Combustible</label>
            </div>
            <div class="control2x" runat="server">
                <asp:UpdatePanel ID="upddlUbicaciones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlUbicaciones" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlConcepto" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtMonto">Monto</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtMonto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="textbox validate[required], custom[positiveNumber]" MaxLength="19"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="control">
                <asp:CheckBox ID="chkDepositoExtraEfectivo" runat="server" Text="Entregado en Efectivo" />
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlTipoCargo">Tipo Cargo</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTipoCargo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTipoCargo" runat="server" CssClass="dropdown"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplkbAgregarFactura" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbAgregarFactura" runat="server" Text="Agregar Factura" OnClick="lkbAgregarFactura_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbAgregarFactura" />
                        <asp:AsyncPostBackTrigger ControlID="ddlConcepto" />
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtReferencia">Referencia</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]" MaxLength="150"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtFechaProgramada">Fecha de ejecución</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFechaProgramada" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFechaProgramada" runat="server" CssClass="textbox2x validate[required] custom[date]" TabIndex="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon3x" style="width: 600px">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnRegistrar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="boton" OnClick="OnClick_btnRegistrar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="boton" OnClick="OnClick_btnEliminar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="OnClick_btnCancelar" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon" style="width: 350px">
            <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnEliminar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlBeneficiario" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>