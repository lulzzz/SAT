<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteSoporteTecnico.aspx.cs" Inherits="SAT.Soporte.ReporteSoporteTecnico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery-->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Biblioteca para uso de carga de Archivos XML -->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQuerySoporte();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQuerySoporte() {
    $(document).ready(function () {

        //Añadiendo Encabezado Fijo
        $("#<%=gvSoportes.ClientID%>").gridviewScroll({
            width: document.getElementById("contenedorSoportes").offsetWidth - 15,
            height: 400
        });

//Añadiendo Validación al Evento Click del Boton
$("#<%=btnBuscar.ClientID%>").click(function () {
var isValid1 = !$("#<%=txtSolicitante.ClientID%>").validationEngine('validate');
var isValid2;
var isValid3; 
                 
//Devolviendo Resultados Obtenidos
return isValid1 && isValid2 && isValid3;
});

// *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaIni.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
// *** Fecha de inicio, fin de Deposito (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
$("#<%=txtFechaFin.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}

//Invocación Inicial de método de configuración JQuery
ConfiguraJQuerySoporte();
</script>
    <div id="encabezado_forma">
        <img src="../Image/Soporte1.png" />
        <h1>Reporte de Soporte Técnico</h1>
    </div>
    <div class="contenedor_seccion_completa style=float:left">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Buscar reportes por:</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Solicitante</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtSolicitante" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSolicitante" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    <Triggers>
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label">Tipo</label>
                </div>
                <div class="control">
                    <asp:updatepanel id="upddlSoportes" runat="server" updatemode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlSoportes" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:updatepanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel id="uptxtFechaIni" runat="server" updatemode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    <Triggers>
                    </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel id="uptxtFechaFin" runat="server" updatemode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Observación</label>
                </div>
                <div class="control">
                    <asp:updatepanel id="uptxtObser" runat="server" updatemode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtObser" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:updatepanel>
                </div>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel id="upbtnBuscar" runat="server" updatemode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" TabIndex="15" OnClick="btnBuscar_Click" />
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
<div class="header_seccion">
<h2>Soporte Técnico</h2>
</div>
<div class="renglon4x">
<div class="etiqueta">
    <label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoSoportes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="ddlTamanoSoportes"></label>
<asp:DropDownList ID="ddlTamanoSoportes" runat="server" TabIndex="16" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoSoportes_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenarSoportes">Ordenado Por:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenarSoportes" runat="server"  UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenarSoportes" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvSoportes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel runat="server" ID="uplkbExportarSoportes" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarSoportes" runat="server" Text="Exportar" CommandName="Soportes" OnClick="lkbExportar_Click" TabIndex="17"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarSoportes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_encabezado_fijo" id="contenedorSoportes">
<asp:UpdatePanel ID="upgvSoportes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvSoportes" runat="server" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvSoportes_PageIndexChanging"
OnSorting="gvSoportes_Sorting" AutoGenerateColumns="False" TabIndex="19" ShowFooter="True" PageSize="25" Width="100%">
<Columns>
    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
    <asp:BoundField DataField="NoSoporte" HeaderText="No. Soporte" SortExpression="NoSoporte" HeaderStyle-Width="70px" ItemStyle-Width="70px" />
    <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="5%" ItemStyle-Width="5%" />
    <asp:BoundField DataField="Tipo" HeaderText="Tipo Soporte" SortExpression="Tipo" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
    <asp:BoundField DataField="Solicitante" HeaderText="Solicitante" SortExpression="Solicitante" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
    <asp:BoundField DataField="FechaInicio" HeaderText="Fecha Inicio" SortExpression="FechaInicio" HeaderStyle-Width="10%" ItemStyle-Width="10%" />
    <asp:BoundField DataField="FechaFin" HeaderText="Fecha Final" SortExpression="FechaFin" HeaderStyle-Width= "10%" ItemStyle-Width="10%" />
    <asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" HeaderStyle-Width="40%" ItemStyle-Width="40%" />
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
    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
    <asp:AsyncPostBackTrigger ControlID="ddlTamanoSoportes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
