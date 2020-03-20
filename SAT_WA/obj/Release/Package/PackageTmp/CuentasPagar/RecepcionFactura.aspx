<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecepcionFactura.aspx.cs" MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.CuentasPagar.RecepcionFactura" Title="Recepción de Facturas" %>

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
<script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
<script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraRecepcionFactura();
}
}
//Funcion de Definición
function ConfiguraRecepcionFactura() {
$(document).ready(function () {
//Función de Validación
var validacionRecepcionTarifa = function (evt) {
var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');

var isValid3 = !$("#<%=txtEntregadoPor.ClientID%>").validationEngine('validate');
var isValid4 = !$("#<%=txtFechaRecepcion.ClientID%>").validationEngine('validate');

//Devolviendo resultados
return isValid1 && isValid3 && isValid4;
}

    $("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
    $("#<%=txtEntregadoPor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=64&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>' });
//Añadiendo Función de validación al evento del Control
$("#<%=lkbGuardar.ClientID%>").click(validacionRecepcionTarifa);
//Añadiendo Función de validación al evento del Control
    $("#<%=btnAceptar.ClientID%>").click(validacionRecepcionTarifa);
    $("#<%=btnAceptarValidacion.ClientID%>").click(function () {
        //LimpiaContenedorXML();
    });

$("#<%=lkbNuevo.ClientID%>").click(function () {
//LimpiaContenedorXML();
});

$("#<%=txtFechaRecepcion.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
/**Script Contenedor de Archivos**/
if ($.support.fileDrop) {
                    $('#divFile').fileDrop({
                        onFileRead: function (fileCollection) {
                            var index = 0;
                            $.each(fileCollection, function () {
                                //Almacenando archivo en sesión
                                GuardaArchivo(fileCollection[index].data, fileCollection[index].name, fileCollection[index].type);
                                index++;
                            });
                            fileCollection = null;
                        }
                    });
                }
                else {
                    alert('Su navegador actual no soporta la carga de archivos por arrastre :(');
                }
});
    }
//Función de guardado de archivo en memoria
        function GuardaArchivo(data, name, type) {
            //Definiendo parametros de metodo web de guarado
            var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";
            //Definiendo consumo de método con json
            $.ajax({
                type: "POST",
                url: "RecepcionFactura.aspx/LecturaArchivo",
                data: dataValue,
                contentType: 'application/json',
                success: function (response) {
                    //Cambiando nombre de archivo cargado
                    CambiaNombreArchivoCargado(response.d);
                },
                failure: function (response) {
                    //Colocando instrucciones de carga
                    BorraNombreArchivoCargado();
                    alert(response.d);
                },
                error: function (response) {
                    //COlocando instrucciones de carga
                    BorraNombreArchivoCargado();
                    alert(response.d);
                }
            });
        };
        //Función para actualizar el nombre del archivo cargado
        function CambiaNombreArchivoCargado(nombre) {
            $('#nombreArchivo').text(nombre);
        };
        function BorraNombreArchivoCargado() {
            $('#nombreArchivo').text('Arrastre y suelte sus archivos *.xml (CFDI)');
        };
//Invocando Función de Configuración
ConfiguraRecepcionFactura();
</script>
<div id="encabezado_forma">
<h1>Recepción de Facturas</h1>
</div>
<asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
<li>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a> 
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
<li>
<asp:LinkButton ID="lkbConfirmar" runat="server" Text="Confirmar" OnClick="lkbElementoMenu_Click" CommandName="Confirmar" /></li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
<li>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
<li>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
</ul>
</li>
<li class="yellow">
<a href="#" class="fa fa-question-circle"></a>
<ul>
<li>
<asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
<li>
<asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
</ul>
</li>
</ul>
</nav>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon">
<div class="etiqueta">
<label class="Label" for="lblID">Folio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblID" CssClass="LabelResalta">ID</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label class="Label" for="txtCompania">
Compañia
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox" TabIndex="1" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtProveedor" class="Label">
Proveedor</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label>Recepción</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtMedioRecepcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtMedioRecepcion" runat="server" CssClass="textbox" TabIndex="3" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label class="Label" for="txtFechaRecepcion">
Fecha Recepcion
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaRecepcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFechaRecepcion" CssClass="textbox validate[custom[dateTime24]]"
TabIndex="3"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label class="Label" for="txtEntregadoPor">
Entregado por:
</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtEntregadoPor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtEntregadoPor" CssClass="textbox validate[required]"
MaxLength="100" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
    <div class="columna">
        <asp:UpdatePanel ID="updivFile" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divFile" style="margin-left: 15px; height: 150px; width: 95%; border: dashed 3px; border-color: #a5d16f; background-color: #DDDDDD">
                    <div id="nombreArchivo" style="margin-left: 10%; width: 80%; margin-top: 12%; font-size: large; font-weight: bold; text-align: center; height: auto; color: #808080">Arrastre y suelte sus archivos *.xml (CFDI)</div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <div class="renglon">
                <br />
            <div class="controlBoton">
                <asp:updatepanel id="upbtnAceptar" runat="server" updatemode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                    </Triggers>
                </asp:updatepanel>
            </div>
                <div class="controlBoton">
                     <asp:Button ID="btnVistaPrevia" runat="server" Text="Vista Previa" CssClass="boton_cancelar" OnClick="btnVistaPrevia_Click" />
                </div>
                <div class="controlBoton">
                    <input id="fuVisorCFDI" runat="server" class="boton_cancelar" type="file" visible="false" />
                </div>
            </div>
    </div>
<%--</div>--%>
<%--<div class="contenedor_controles">--%>
<%--<div class="encabezado_control">
<h4>Detalles de Recepción</h4>
</div>--%>
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Detalles de Recepción</h2>
</div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamañoGridViewDetalles">Mostrar</label>
</div>
<div class="etiqueta_155px">
<asp:DropDownList ID="ddlTamañoGridViewDetalles" runat="server" CssClass="dropdown"
AutoPostBack="true" OnSelectedIndexChanged="ddlTamañoGridViewDetalles_SelectedIndexChanged">
</asp:DropDownList>
</div>
<div class="etiqueta_50px">
<label for="lblCriterioGridViewDetalles">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblCriterioGridView" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewDetalles" runat="server" Text=""></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDetalles" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<%--<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplkbImprimir" runat="server" RenderMode="Inline" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" TabIndex="7" OnClick="lkbImprimir_Click">Imprimir</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbImprimir" />
</Triggers>
</asp:UpdatePanel>
</div>--%>
<div class="etiqueta">
<asp:LinkButton ID="lkbExcelDetalles" runat="server" EnableViewState="False" OnClick="lkbExcelDetalles_Click">Exportar Excel</asp:LinkButton>
</div>
</div>
    <div class="grid_seccion_completa_altura_variable">
        <asp:UpdatePanel ID="upgvDetalles" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvDetalles" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                    AutoGenerateColumns="False" ShowFooter="True" CssClass="gridview" OnPageIndexChanging="gvDetalles_PageIndexChanging"
                    OnSorting="gvDetalles_Sorting" Width="100%">
                    <AlternatingRowStyle CssClass="gridviewrowalternate" />
                    <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                    <FooterStyle CssClass="gridviewfooter" />
                    <HeaderStyle CssClass="gridviewheader" />
                    <RowStyle CssClass="gridviewrow" />
                    <SelectedRowStyle CssClass="gridviewrowselected" />
                    <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                    <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                    <Columns>
                        <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                        <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                        <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                        <asp:BoundField DataField="FechaFactura" HeaderText="Fecha Facturación" SortExpression="FechaFactura" />
                        <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" SortExpression="SubTotal" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Descuento" HeaderText="Descuento" SortExpression="Descuento" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Trasladado" HeaderText="Trasladado" SortExpression="Trasladado" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Retenido" HeaderText="Retenido" SortExpression="Retenido" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:c}" />                        
                        <asp:TemplateField HeaderText="Validación SAT">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbValidacion" runat="server" OnClick="imbValidacion_Click" ImageUrl="~/Image/cfdi_consulta.png" Width="25" Height="25" />
                                </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewDetalles" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <%--<div class="contenedor_controles">--%>
<div class="columna2x">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblError" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" EventName="RowCommand" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<%--<div class="columna">
<div class="renglon_boton">
<div class="controlBoton">


</div>
</div>
</div>--%>
<%--</div>--%>
</div>
<%--<div class="contenedor_controles">
<div id="box">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div>
</div>--%>
<div id="contenedorVentanaConfirmacion" class="modal">
<div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Desea dar de alta al Proveedor </h2>
<asp:UpdatePanel ID="uplblProveedorFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2><b>
<asp:Label ID="lblProveedorFactura" runat="server"></asp:Label></b></h2>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
</Triggers>
</asp:UpdatePanel>
<h2>?</h2>
</div>
<div class="contenedor_media_seccion">
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipoServicio">Tipo de Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarOperacion" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnAceptarOperacion_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarOperacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarOperacion_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>


<!-- Ventana Confirmación Resultado Consulta SAT -->
<div id="contenidoResultadoConsultaSATModal" class="modal">
<div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba">
<div class="columna2x">
<div class="header_seccion">
<asp:UpdatePanel ID="upheaderValidacionSAT" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
<h3 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h3>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Emisor</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Receptor</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblRFCReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblRFCReceptor" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">UUID</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblUUID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblUUID" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Fecha Expedición</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="">Total Factura</label>
</div>
<div class="etiqueta_320px">
<asp:UpdatePanel ID="uplblTotalFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalFactura" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
<asp:AsyncPostBackTrigger ControlID="gvDetalles" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCanelarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCanelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
    <!-- Ventana Confirmación Resultado Consulta SAT -->
<div id="contenidoConfirmarAceptacionModal" class="modal">
<div id="contenidoConfirmarAceptacion" class="contenedor_ventana_confirmacion_arriba">
<div class="columna2x">
<div class="header_seccion">
<img id="img1" runat="server" src="../Image/Exclamacion.png" />
<h3 id="h1" runat="server">
<div class="etiqueta_400px">
<asp:UpdatePanel ID="upTotalFacturas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblTotalFacturas" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>  
</h3>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">   
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasR" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<asp:UpdatePanel ID="upPAceptadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="PAceptadas" runat="server" Visible="false">
<div class="renglon2x">
<div class="etiqueta_50px">    
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasA" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasA" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Panel>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPPagos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="PPagos" runat="server" Visible="false">
<div class="renglon2x">
<div class="etiqueta_50px">
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasP" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    </asp:Panel>
    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPRefacturadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="PRefacturadas" runat="server" Visible="false">
<div class="renglon2x">
<div class="etiqueta_50px">
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasRef" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasRef" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    </asp:Panel>
    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPCanceladas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="PCanceladas" runat="server" Visible="false">
<div class="renglon2x">
<div class="etiqueta_50px">
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasC" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    </asp:Panel>
    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upPRechazadas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Panel ID="PRechazadas" runat="server" Visible="false">
<div class="renglon2x">
<div class="etiqueta_50px">
</div>
<div class="etiqueta_400px">
<asp:UpdatePanel ID="uplblFacturasRec" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblFacturasRec" runat="server" CssClass="label_negrita"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    </asp:Panel>
    </ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbConfirmar" />
</Triggers>
</asp:UpdatePanel>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnConfirmar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" CssClass="boton" CommandName="Confirmar" OnClick="btnConfirmar_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnConfirmar_Click" CommandName="Cancelar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>

</asp:Content>
