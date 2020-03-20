<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucBitacora.ascx.cs" Inherits="SAT.UserControls.wucBitacora" %>
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryControlBitacora();
        }
    }

    function ConfiguraJQueryControlBitacora()
    {   // *** Fecha de carga, descarga (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
        $(document).ready(function () {
            $("#<%=txtFecIni.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFecFin.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Declarando Función de Validación
            var validaBitacora = function (evt) {
                var isValid1 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado de Validación
                return isValid1 && isValid2
            }
            //Añadiendo Función a Evento Click
            $("#<%=btnBuscar.ClientID%>").click(validaBitacora);
        });

    }
    //Invocando Funcion de Configuracion
    ConfiguraJQueryControlBitacora();
</script>
<div class="contenida_control_bitacora">
     <div class="encabezado_control">
        <h2>Bitacora</h2>     
    </div>
    <div class="contenido_resultado_bitacora">
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"
                            TabIndex="5" CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label>Ordenado:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblCriterio" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblCriterio" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvResultado" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_bitacora">
            <asp:UpdatePanel ID="upgvResultado" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvResultado" runat="server" AllowPaging="true" AllowSorting="true"
                        OnPageIndexChanging="gvResultado_PageIndexChanging" OnSorting="gvResultado_Sorting"
                        PageSize="5" CssClass="gridview" ShowFooter="True" ShowHeaderWhenEmpty="True"
                        AutoGenerateColumns="false" EmptyDataText="No hay registros" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                            <asp:BoundField DataField="Anterior" HeaderText="Anterior" SortExpression="Anterior" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_filtro_bitacora">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipo">Tipo</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipo" runat="server" TabIndex="1" CssClass="dropdown2x"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Inicio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" TabIndex="2" CssClass="textbox2x validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fin</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" TabIndex="3" CssClass="textbox2x validate[required, custom[dateTime24]]"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton"></div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" TabIndex="4" Text="Buscar" 
                                OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="control_60px">
                    <asp:UpdatePanel ID="upupBuscarBitacora" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="upBuscarBitacora" runat="server" AssociatedUpdatePanelID="upbtnBuscar">
                                <ProgressTemplate>
                                    <asp:Image ID="imgBuscarBitacora" runat="server" ImageUrl="~/Image/cargando.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            
        </div>
    </div>
    <div class="imagen_control">
        <img src="../Image/Bitacora.png" />
    </div>
</div>
