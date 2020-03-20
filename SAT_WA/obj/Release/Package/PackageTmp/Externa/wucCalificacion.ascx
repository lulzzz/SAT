<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCalificacion.ascx.cs" Inherits="SAT.SinLogin.Calificacion" %>
<%@ Register Src="~/Externa/wucHistorialCalificacion.ascx" TagName="wucHistorial" TagPrefix="us" %>
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
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!--Script que valida la insercion de datos en los controles-->
<script type="text/javascript">
    //Obtiene la instancia actual de la pagina y añade un manejador de eventos
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Creación de la función que permite finalizar o validar los controles a partir de un error.
    function EndRequestHandler(sender, args) {
        //Valida si el argumento de error no esta definido
        if (args.get_error() == undefined) {
            //Invoca a la Funcion ConfiguraJQueryLectura
            ConfiguraJQueryCalificacion();
        }
    }
    function ConfiguraJQueryCalificacion() {
        $(document).ready(function () {

            var validaCalificacion = function (evt) {
                var isValid1 = $('input:radio[name=estrellas]').attr('checked', false);
                return isValid1;
            };
            //Añadiendo Función de Validación
            $("#<%=btnGuardar.ClientID%>").click(validaCalificacion);
        $("#<%=ddlConcepto.ClientID%>").click(validaCalificacion);
        $("#<%=gvConceptosCalificacion.ClientID%>").click(validaCalificacion);
    });
}
ConfiguraJQueryCalificacion();
</script>
<!--Fin Script-->
<!--CSS-->
<style type="text/css">
    input[type="radio"] {
        display: none;
    }

    label {
        color: black;
    }

    .estrella {
        font-size: 32px;
    }

    .eti {
        margin: 0px 0px 44px 316px;
        padding: 0px 44px 0px 32px;
        width: 155px;
        height: 18px;
    }

    .cali {
        margin: 15px 0px 0px 0px;
        padding: 5px 0px 0px 5px;
        width: 155px;
        height: 18px;
        float: left;
    }

    .prom {
        margin: 0px 0px 0px 0px;
        width: 165px;
        height: 23px;
        float: left;
    }

    .comentarios {
        margin: 0px;
        padding: 8px 0px 0px 0px;
        width: 238px;
        height: 18px;
        float: left;
    }

    .clasificacion {
        direction: rtl;
        unicode-bidi: bidi-override;
        float: inherit;
        margin: 0px 0px 44px 283px;
        padding: 0px 46px 0px 30px;
        width: 155px;
        height: 18px;
    }

    label:hover,
    label:hover ~ label {
        color: orange;
    }

    input[type="radio"]:checked ~ label {
        color: orange;
    }
    .calificacionComentario{
    width: 411px;
    height: 55px;
    float: left;
        padding: 0px 0px 8px 0px;
    }
</style>
<!--Fin CSS-->
<div class="seccion_controles">
    <div class="columna3x">
        <div class="header_seccion">
            <div class="etiqueta_200px">
                <asp:UpdatePanel runat="server" ID="uplblEntidad" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblEntidad" CssClass="label_negrita" Font-Size="Large"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="prom">
                <asp:UpdatePanel runat="server" ID="upimgHistorial" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Image runat="server" ID="imgHistorial" ImageUrl="" ImageAlign="Right" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="imgbtnAgregarConcepto" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvConceptosCalificacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="comentarios">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="uplkbCantidadComentarios">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="lkbCantidadComentarios" CssClass="leyenda_indicador" OnClick="lkbCantidadComentarios_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="imgbtnAgregarConcepto" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="gvConceptosCalificacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon3x">
            </div>
            <div class="renglon3x">
            </div>
        </div>
    </div>

    <div class="renglon3x">
        <div class="etiqueta_155px">
            <label for="ddlConcepto">Evalua:</label>
        </div>
        <div class="control2x">
            <asp:UpdatePanel runat="server" ID="upddlConcepto" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList runat="server" ID="ddlConcepto" CssClass="dropdown2x"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="imgbtnAgregarConcepto" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta"></div>
    </div>
    <div class="renglon3x">
        <div class="cali">
            <label>Calificación Asignada: </label>
        </div>
        <div class="etiqueta_50px"></div>
        <div class="clasificacion">
            <input id="radio1" name="estrellas" value="5" type="radio">
            <label class="estrella" for="radio1">&#9733;</label>

            <input id="radio2" name="estrellas" value="4" type="radio">
            <label class="estrella" for="radio2">&#9733;</label>
            <input id="radio3" name="estrellas" value="3" type="radio">
            <label class="estrella" for="radio3">&#9733;</label>
            <input id="radio4" name="estrellas" value="2" type="radio">
            <label class="estrella" for="radio4">&#9733;</label>
            <input id="radio5" name="estrellas" value="1" type="radio">
            <label class="estrella" for="radio5">&#9733;</label>
        </div>
        <div class="eti">
            <asp:UpdatePanel runat="server" ID="upimgbtnAgregarConcepto" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton runat="server" ID="imgbtnAgregarConcepto" ImageUrl="~/Image/Agregar.png" OnClick="imgbtnAgregarConcepto_Click" ImageAlign="Right" Width="32px" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


</div>

<div class="seccion_controles">
    <div class="columna3x">
        <div class="header_seccion">
            <h2>Calificación Asignada</h2>
        </div>
    </div>
    <div class="renglon3x">
        <div class="etiqueta">
            <label for="ddlTamañoGridViewConceptosCaseta">Mostrar</label>
        </div>
        <div class="control">
            <asp:UpdatePanel ID="upddlTamañoGridViewConceptosCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:DropDownList ID="ddlTamañoGridViewConceptosCalificacion" runat="server" OnSelectedIndexChanged="gvConceptosCalificacion_OnSelectedIndexChanged" TabIndex="14" AutoPostBack="true" CssClass="dropdown">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta">
            <label for="lblCriterioGridViewConceptosCalificacion">Ordenado Por:</label>
        </div>
        <div class="etiqueta">
            <asp:UpdatePanel ID="uplblCriterioGridViewConceptosCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblCriterioGridViewConceptosCalificacion" TabIndex="15" runat="server"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvConceptosCalificacion" EventName="Sorting" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="etiqueta">
            <asp:UpdatePanel runat="server" ID="uplkbExportarExcelConceptosCalificacion" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lkbExportarExcelConceptosCalificacion" runat="server" Text="Exportar" TabIndex="16" OnClick="lkbExportarExcelConceptosCalificacion_Onclick"></asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lkbExportarExcelConceptosCalificacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="grid_seccion_completa_altura_variable">
        <asp:UpdatePanel ID="upgvConceptosCalificacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvConceptosCalificacion" CssClass="gridview" OnPageIndexChanging="gvConceptosCalificacion_OnpageIndexChanging" OnSorting="gvConceptosCalificacion_Onsorting" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="false"
                    ShowFooter="True" TabIndex="17" OnRowDataBound="gvConceptosCalificacion_RowDataBound"
                    PageSize="25" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                        <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Calificación" SortExpression="Calificacion" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image runat="server" ID="imgCalificacion" ImageUrl="~/Image/EstrellaH1.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Promedio" HeaderText="Promedio" SortExpression="Promedio" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEliminar" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
                            </ItemTemplate>
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
                <asp:AsyncPostBackTrigger ControlID="imgbtnAgregarConcepto" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewConceptosCalificacion" />
                <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="renglon3x">
        <div class="etiqueta_50px">
            <label for="txtComentario">Comentario</label>
        </div>
        <div class="calificacionComentario">
            <asp:UpdatePanel runat="server" ID="uptxtComentario" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox runat="server" ID="txtComenatrio" TextMode="MultiLine" CssClass="textbox2x" Width="390px" Height="55px" MaxLength="300"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control">
            <asp:UpdatePanel runat="server" ID="upbtnGuardar" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnGuardar" CssClass="boton" Text="Aceptar" OnClick="btnGuardar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>

<div id="contenedorHistorialCalificacion" class="modal">
    <div id="HistorialCalificacion" class="contenedor_modal_seccion_completa" style="left:150px; width:805px; top:100px;">
        <div class="boton_cerrar_modal">
            <asp:UpdatePanel ID="uplnkCerrarVentanaHistorialCalificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lnkCerrarVentanaHistorialCalificacion" runat="server" Text="Cerrar" OnClick="lnkCerrarVentanaHistorialCalificacion_Click">
                            <img src="../Image/Cerrar16.png" />
                    </asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="header_seccion">
            <h2>HISTORIAL DE CALIFICACIÓN</h2>
        </div>
        <div class="columna3x">
            <div class="renglon2x">
                <asp:UpdatePanel ID="upwucHistorial" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <us:wucHistorial ID="wucHistorial" runat="server"/>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbCantidadComentarios" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</div>


