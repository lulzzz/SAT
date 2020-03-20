<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAsignacionDieselProgramado.ascx.cs" Inherits="SAT.UserControls.wucAsignacionDieselProgramado" %>

<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlAsignacion();
        }
    }
    //Declarando Función de Configuración
    function ConfiguraJQueryControlAsignacion() {
        $(document).ready(function () {

        });
    }
    //Invocando Función
    ConfiguraJQueryControlAsignacion();
</script>
<div class="seccion_controles">
    <div class="header_seccion">
        <img src="../Image/Documento.png" />
        <h2>Vales de Diesel</h2>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblId" runat="server" Text="No. de Vale: Por Asignar"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upmtvBeneficiario" runat="server">
                <ContentTemplate>
                    <asp:MultiView ID="mtvBeneficiario" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vwlblBeneficiario" runat="server">
                            <div class="etiqueta_155px" style="width: 255px">
                                <asp:UpdatePanel ID="uplblBeneficiario" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblBeneficiario" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                        <asp:View ID="vwddlBeneficiario" runat="server">
                            <div class="control2x">
                                <asp:UpdatePanel ID="upddlBeneficiario" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBeneficiario" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlBeneficiario_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblEstatus" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_155px" style="width: 255px">
                <asp:UpdatePanel ID="uplblOperador" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOperador" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="ddlBeneficiario" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <%--        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtOperadorProveedor">Operador/Proveedor</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtOperadorProveedor" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtOperadorProveedor" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="ddlBeneficiario" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>--%>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtUbicacion">Estación Combustible</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlUbicacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="dropdown2x" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlUbicacion_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtLitros">Litros</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtLitros" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtLitros" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber4]]"
                            AutoPostBack="true" OnTextChanged="txtLitros_TextChanged"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="rdbMagna" />
                        <asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
                        <asp:AsyncPostBackTrigger ControlID="gvKilometros" />
                        <asp:AsyncPostBackTrigger ControlID="gvKilometros" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplnkCalculado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCalculado" runat="server" Text="Calculado" OnClick="lnkCalculado_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUnidadDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="gvKilometros" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtFecCarga">Fecha de Carga</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtFecCarga" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFecCarga" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblTotal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblTotal" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="txtLitros" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                        <asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="rdbMagna" />
                        <asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_155px">
                <label for="txtUbicacionActual">Ubicación Actual</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtUbicacionActual" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtUbicacionActual" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16" AutoPostBack="true" OnTextChanged="txtUbicacionActual_TextChanged"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="columna2x">
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="ddlUnidadDiesel">Unidad Diesel</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlUnidadDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlUnidadDiesel" CssClass="dropdown2x" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidadDiesel_SelectedIndexChanged" runat="server"></asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblCantidadDisp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblCantidadDisp" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_155px" style="width: 255px">
                <asp:UpdatePanel ID="uplblImpresion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblImpresion" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upddlTipoVale" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlTipoVale" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="uptxtFecSol" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <asp:TextBox ID="txtFecSol" runat="server" CssClass="textbox" Enabled="false"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <asp:UpdatePanel ID="uprdbDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:RadioButton ID="rdbDiesel" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged"
                            GroupName="Combustible" AutoPostBack="true" Text="Diesel" Checked="true" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uprdbMagna" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:RadioButton ID="rdbMagna" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged"
                            GroupName="Combustible" AutoPostBack="true" Text="G. Magna" Checked="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uprdbPremiun" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:RadioButton ID="rdbPremiun" runat="server" OnCheckedChanged="rdbTipoCombustible_CheckedChanged"
                            GroupName="Combustible" AutoPostBack="true" Text="G. Premiun" Checked="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtCostoDiesel">Costo de Diesel</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uptxtCostoDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtCostoCombustible" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber], min[1]]"
                            AutoPostBack="true" OnTextChanged="txtCostoCombustible_TextChanged"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="rdbDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="rdbMagna" />
                        <asp:AsyncPostBackTrigger ControlID="rdbPremiun" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkCostoDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCostoDiesel" runat="server" Text="Costo Diesel" OnClick="lnkCostoDiesel_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="ddlUnidadDiesel" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarDiesel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta_155px">
                <label for="txtReferencia">Referencia</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox2x validate[required]"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon" style="width: 450px">
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnImprimir" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="boton"
                            OnClick="btnImprimir_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton"
                            OnClick="btnGuardar_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar"
                            OnClick="btnCancelar_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnReferencias" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnReferencias" runat="server" Text="Referencias" CssClass="boton" OnClick="btnReferencias_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnImprimir" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="upGridKilometrajes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="grid_seccion_completa_media_altura" style="height: 200px">
                <asp:UpdatePanel ID="upgvKilometros" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvKilometros" runat="server" AllowSorting="true" AllowPaging="true"
                            CssClass="gridview" OnPageIndexChanging="gvKilometros_PageIndexChanging"
                            AutoGenerateColumns="false" PageSize="25" ShowFooter="true" Width="100%" OnRowDataBound="gvKilometros_RowDataBound">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:TemplateField>
                                    <%--                            <HeaderTemplate>
                                <asp:CheckBox ID="chkTodos" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="16" />
                            </HeaderTemplate>--%>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkVarios" runat="server" OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="17" />
                                        <asp:Image ID="imgEstatusChk" runat="server" ImageUrl="~/Image/diesel.png" ToolTip="Estatus" Width="27" Height="27" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:BoundField DataField="IdServicio" HeaderText="IdServicio" SortExpression="IdServicio" Visible="false" />
                                <asp:BoundField DataField="IdMovimiento" HeaderText="IdMovimiento" SortExpression="IdMovimiento" Visible="false" />
                                <asp:BoundField DataField="IdMovimientoVale" HeaderText="IdMovimientoVale" SortExpression="IdMovimientoVale" Visible="false" />
                                <asp:BoundField DataField="KmsActuales" HeaderText="KmsActuales" SortExpression="KmsActuales" Visible="false" />
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemStyle Width="50px" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:Image ID="imgEstatus" runat="server" ImageUrl="~/Image/Iniciar.png" ToolTip="Estatus" Width="27" Height="27" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="NoServicio" HeaderText="Servicio/Movimiento" SortExpression="NoServicio" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                <asp:TemplateField HeaderText="kms" SortExpression="kms">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbKmsActualizar" runat="server" Text='<%# Eval("Kms") %>' OnClick="lkbKms_Click" CommandName="Detalles"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtUbicacionActual" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</div>
<!--VENTANA MODAL PARA CALCULAR LA RUTA-->
<div id="contenedorVentanaConfirmacionDiesel" class="modal">
    <div id="ventanaConfirmacionDiesel" class="contenedor_ventana_confirmacion">
        <div class="header_seccion">
            <img src="../Image/Exclamacion.png" />
            <h2>Al cambiar la Fecha de Carga, se modificará el Precio del Diesel.</h2>
        </div>
        <div class="renglon">
            <asp:UpdatePanel ID="upbtnAceptarDiesel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnAceptarDiesel" runat="server" Text="Aceptar" OnClick="btnAceptarDiesel_Click" CssClass="controlBoton" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
