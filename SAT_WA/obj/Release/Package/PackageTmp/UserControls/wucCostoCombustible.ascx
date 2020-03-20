<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCostoCombustible.ascx.cs" Inherits="SAT.UserControls.wucCostoCombustible" %>


<!-- hoja de estilo que da formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<!-- Estilo de validación de los controles-->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Librerias para la validacion de los controles-->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js"></script>
<!--Codigo que valida los controles del formulario-->
<script type="text/javascript">
    //Gestio las actualizaciones cliente servidor para la validación de los controles
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Creación de la función que permite finalizar o validar los controles a partir de un error.
    function EndRequestHandler(sender, args) {
        //Valida si el argumento de error no esta definido
        if (args.get_error() == undefined) {
            ConfiguraJQueryCostoCombustible();
        }
    }
    //Declara la funcion Configuracion que valida los campos
    function ConfiguraJQueryCostoCombustible() {
        $(document).ready(function () {
            //Crea variables para la validción de los controles
            var validaCostoCombustible = function () {
                //Creación de las variables que permiten validar cada campo.
                var isValid1 = !$("#<%=txtReferencia.ClientID%>").validationEngine('validate');
                var isValid2 = !$("#<%=txtPrecioCombustible.ClientID%>").validationEngine('validate');
                var isValid3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                var isValid4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                //Devuelve un valor a la función
                return isValid1 && isValid2 && isValid3 && isValid4;
            };
            //Permite que los eventos de guardar activen la funcion de validación de controles.
            $("#<%=btnGuardar.ClientID%>").click(validaCostoCombustible);

        });
        // *** Fecha de inicio y fin (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
        $(document).ready(function () {
            $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
        });
    }
    //Invoca a la función ConfiguraJQueryPrecioCombustible()
    ConfiguraJQueryCostoCombustible();
</script>
<div class="seccion_controles">
    <div class="columna5x" style="width: 900px">
        <div class="header_seccion">
            <img src="../Image/SignoPesos.png" />
            <h2>Costo Combustible</h2>
        </div>

        <div class="contenedor_seccion_completa" style="width: 500px">
            <div class="header_seccion">
                <h2>Costo Combustible</h2>
            </div>

            <div class="renglon2x" runat="server">
                <div class="etiqueta_155px">
                    <label for="ddlTipoCombustible">Estacion Combustible:</label>
                </div>
                <div class="control2x" runat="server">
                    <asp:UpdatePanel ID="upddlUbicacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlUbicacion" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlUbicacion_SelectedIndexChanged"></asp:DropDownList>
                            <asp:Label ID="lblId" runat="server" Text="Por Asignar" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x" runat="server">
                <div class="etiqueta">
                    <label for="txtFechaInicio">Fecha Inicio:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtFechaInicio">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtFechaInicio" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFechaFin">Fecha Fin:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtFechaFin">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtFechaFin" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="3"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtPrecioCombustible">Precio:</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtPrecioCombustible" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtPrecioCombustible" CssClass="textbox_50px validate[required], custom[positiveNumber]" MaxLength="18" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x" runat="server">
                <div class="etiqueta">
                    <label for="ddlTipoCombustible">Tipo Combustible:</label>
                </div>
                <div class="control2x" runat="server">
                    <asp:UpdatePanel ID="upddlTipoCombustible" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoCombustible" runat="server" CssClass="dropdown2x"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtReferencia">Referencia: </label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel runat="server" ID="uptxtReferencia" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox runat="server" ID="txtReferencia" CssClass="textbox2x validate[required]" MaxLength="50" TabIndex="6"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>                            
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="13" CssClass="boton" OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" TabIndex="13" CssClass="boton" OnClick="btnGuardar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>





        <div class="contenedor_seccion_completa" style="width: 800px">
            <div class="header_seccion">
                <h2>Costo Registrados</h2>
            </div>

            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta_50px">
                        <label for="ddlTamano">
                            Mostrar:
                        </label>
                    </div>
                    <div class="control">
                        <asp:DropDownList ID="ddlTamano" runat="server" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true" CssClass="dropdown" TabIndex="5">
                        </asp:DropDownList>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uplblOrdenarCostoCombustible" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <label for="lblOrdenarCostoCombustible">Ordenado Por:</label>
                                <asp:Label ID="lblOrdenarCostoCombustible" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCostosRegistrados" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="grid_seccion_completa_100px_altura">
                <asp:UpdatePanel ID="upgvCostosRegistrados" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvCostosRegistrados" runat="server" CssClass="gridview" Width="100%" AllowPaging="True" AllowSorting="true"
                            ShowFooter="True" AutoGenerateColumns="False" PageSize=" 5" OnSorting="gvCostosRegistrados_Sorting" OnPageIndexChanging="gvCostosRegistrados_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="Gasolineria" HeaderText="Gasolineria" SortExpression="Gasolineria" />
                                <asp:BoundField DataField="TipoCombustible" HeaderText="Tipo Combustible" SortExpression="TipoCombustible" />
                                <asp:BoundField DataField="CostoCombustible" HeaderText="Precio" SortExpression="CostoCombustible" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Fecha Fin" SortExpression="FechaFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbEditar_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" SortExpression="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbEliminar_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />--%>
                        <asp:AsyncPostBackTrigger ControlID="ddlUbicacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <%--<asp:AsyncPostBackTrigger ControlID="gvRecursosAsignados" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoCostosRegistrados" />--%>
                    </Triggers>
                </asp:UpdatePanel>

            </div>


        </div>

    </div>
</div>
