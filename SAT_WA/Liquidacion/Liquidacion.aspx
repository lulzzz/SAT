<%@ Page Title="Liquidación" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Liquidacion.aspx.cs" Inherits="SAT.Liquidacion.Liquidacion" %>
<%@ Register Src="~/UserControls/wucReferenciaViaje.ascx" TagName="wucReferenciaViaje" TagPrefix="tectos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Liquidacion.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<style>
    #box {
        width: 250px;
        height: 50px;
        text-align: center;
        vertical-align: -webkit-baseline-middle;
        border: 2px solid #04B404;
        background-color: #00FF00;
        padding: 15px;
        font-family: Arial;
        font-size: 16px;
        margin-top: 35px;
    }
</style>
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
<script type="text/javascript" src="../Scripts/jquery.jqzoom-core.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraLiquidacion();
        }
    }
    //Función de Configuración
    function ConfiguraLiquidacion() {
        $(document).ready(function () {
            var validaBusqueda = function () {
                var isValid1 = !$("#<%=txtEntidad.ClientID%>").validationEngine('validate');
                //Devolviendo Resultado de Validación
                return isValid1;
            }
            //Añadiendo Funcion de Validacion al Evento Click
            $("#<%=btnBuscar.ClientID%>").click(validaBusqueda);
            //Añadiendo Funcion de Validacion al Evento Click
            $("#<%=btnCrear.ClientID%>").click(validaBusqueda);

            $("#<%=btnAgregarFactura.ClientID%>").click(
            //Invocando Método de Limpieza
            LimpiaContenedorXML()
            );

            //Cargando Control DateTimePicker
            $("#<%=txtFecha.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
            });
            //Función de Validación de los Controles de Pago
            var validaPago = function () {
                var isValid1 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');
        var isValid2 = !$("#<%=txtValorU.ClientID%>").validationEngine('validate');
        var isValid3 = !$("#<%=txtTotal.ClientID%>").validationEngine('validate');
        var isValid4 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado de Validación
        return isValid1 && isValid2 && isValid3 && isValid4;
    }

            //Añadiendo Función de Validación
            $("#<%=btnGuardarPago.ClientID%>").click(validaPago);

            //Función de Validación de los Controles de Comprobación
            var validaComprobacion = function () {
        var isValid1 = !$("#<%=txtValorUnitario.ClientID%>").validationEngine('validate');
        //Devolviendo Resultado de Validación
        return isValid1;
    }

            //Añadiendo Función de Validación
            $("#<%=btnGuardarComprobacion.ClientID%>").click(validaComprobacion);
        });
    }
    //Invocando Función de Configuración
    ConfiguraLiquidacion();

    /**Script Contenedor de Archivos**/
    //Declarando variable contenedora de Archivos
    var selectedFiles;
    //Función que limpia el Contenedor
    function LimpiaContenedorXML() {   //Limpiando DIV
        selectedFiles = null;
        $("#box").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
    }

    //Inicializando Función
    $(document).ready(function () {
        //validando el Tipo de Archivo
        if (!Modernizr.draganddrop) {
            alert("This browser doesn't support File API and Drag & Drop features of HTML5!");
            return;
        }
        //Declarando Objeto contenedor del DIV
        var box;
        box = document.getElementById("box");
        //Añadiendo Eventos
        box.addEventListener("dragenter", OnDragEnter, false);
        box.addEventListener("dragover", OnDragOver, false);
        box.addEventListener("drop", OnDrop, false);

        //Función cuando se Arrastra el Objeto dentro del limite
        function OnDragEnter(e) {
            e.stopPropagation();
            e.preventDefault();
        }
        //Función cuando se Arrastra el Objeto fuera del limite
        function OnDragOver(e) {
            e.stopPropagation();
            e.preventDefault();
        }
        //Función cuando se Suelta el Objeto dentro del limite
        function OnDrop(e) {
            e.stopPropagation();
            e.preventDefault();

            selectedFiles = null;
            selectedFiles = e.dataTransfer.files;
            //Declarando Objeto de Lectura
            var lector = new FileReader();
            //Evento al Cargar el Archivo
            lector.onload = function (evt) {
                //Obteniendo Archivo
                var bytes = evt.target.result;
                //Invocando Método Web para Obtención de Archivos
                PageMethods.ArchivoSesion(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
            };
            //Evento al Producirse un Error
            lector.onerror = function (evt) {
                alert('Error Carga ' + evt.target.error);

            };
            //Leyendo Texto
            lector.readAsText(selectedFiles[0]);
            //Mostrando mensaje
            alert('El Archivo se ha Cargado');
            //Indicando Archivo
            $("#box").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
        }

    });

</script>
<div id="encabezado_forma">
<img src="../Image/FacturacionCargos.png" />
<h1>Liquidación</h1>
</div>
<div class="contenedor_botones_pestaña">
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnBusqueda" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBusqueda" Text="Búsqueda" OnClick="btnVista_OnClick" runat="server" CommandName="Busqueda" CssClass="boton_pestana_activo"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLiquidacion" Text="Liquidación" OnClick="btnVista_OnClick"  CommandName="Liquidacion" runat="server" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_boton_pestana">
<asp:UpdatePanel ID="upbtnResumen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnResumen" Text="Resumen" OnClick="btnVista_OnClick"  CommandName="Resumen" runat="server" CssClass="boton_pestana"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="contenido_tabs_300px">
<asp:UpdatePanel ID="upmtvEncabezado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:MultiView ID="mtvEncabezado" runat="server" ActiveViewIndex="0">
<asp:View ID="vwBusqueda" runat="server">
<div class="seccion_controles">
    <div class="columna2x">
        <div class="header_seccion">
            <h2>Busqueda Liquidaciones</h2>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="ddlTipoEnt">Tipo Entidad</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="upddlTipoEnt" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTipoEnt" runat="server" TabIndex="1" AutoPostBack="true"
                            CssClass="dropdown2x">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="etiqueta">
                <label for="txtEntidad">Entidad</label>
            </div>
            <div class="control2x">
                <asp:UpdatePanel ID="uptxtEntidad" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:TextBox ID="txtEntidad" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="renglon2x">
            <div class="control">
                <asp:UpdatePanel ID="uplblErrorBusqueda" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorBusqueda" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                        <asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton"
                            OnClick="btnBuscar_Click" TabIndex="3" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCrear" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="boton_cancelar"
                            OnClick="btnCrear_Click" TabIndex="4" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenedor_730px_derecha">
        <div class="renglon100Per">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" TabIndex="5" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"
                            CssClass="dropdown_100px">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenado">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" OnClick="lnkExportar_Click"
                            TabIndex="6"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class ="grid_seccion_completa_400px_altura">            
            <asp:UpdatePanel ID="upgvLiquidacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvLiquidacion" runat="server" AllowPaging="true" AllowSorting="true"
                        CssClass="gridview" ShowFooter="true" TabIndex="7" OnSorting="gvLiquidacion_Sorting"
                        OnPageIndexChanging="gvLiquidacion_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="CompaniaEmisora" HeaderText="Compania" SortExpression="CompaniaEmisora" />
                            <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha" SortExpression="FechaLiquidacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="TotalSalario" HeaderText="Total Salario" SortExpression="TotalSalario" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalDeducciones" HeaderText="Total Deducciones" SortExpression="TotalDeducciones" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalDescuentos" HeaderText="Total Descuentos" SortExpression="TotalDescuentos" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalSueldo" HeaderText="Total Sueldo" SortExpression="TotalSueldo" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalAnticipos" HeaderText="Total Anticipos" SortExpression="TotalAnticipos" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalComprobaciones" HeaderText="Total Comprobaciones" SortExpression="TotalComprobaciones" ItemStyle-HorizontalAlign="Right" />
                            <asp:BoundField DataField="TotalAlcance" HeaderText="Total Alcance" SortExpression="TotalAlcance" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" Text="Seleccionar" OnClick="lnkSeleccionar_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
</asp:View>
<asp:View ID="vwLiquidacion" runat="server">
    <div class="seccion_controles">
        <div class="columna2x">             
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label_negrita">Datos Liquidación</label>
                </div>
            </div>          
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lblId">No. Liquidación</label>
                </div>
                <div class="etiqueta_155px">
                    <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblNoLiq" runat="server" Text="Por Asignar" CssClass="label_negrita"></asp:Label>
                            <asp:Label ID="lblId" runat="server" Text="Por Asignar" CssClass="label_negrita" Visible="false"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lblEstatus">Estatus</label>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblEstatus" runat="server" Text="Ninguno" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="lblTipoEntidad">Tipo Entidad</label>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplblTipoEntidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblTipoEntidad" runat="server" Text="Ninguno" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Entidad</label>
                </div>
                <div class="etiqueta_320px">
                    <asp:UpdatePanel ID="uplblEntidad" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblEntidad" runat="server" Text="Ninguno" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Fecha</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecha" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="lblPercepciones">T. Percepciones</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblPercepciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblPercepciones" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                            <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                            <asp:AsyncPostBackTrigger ControlID="gvPagos" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <label for="lblAnticipos">T. Anticipos</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblAnticipos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblAnticipos" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="lblDeducciones">T. Deducciones</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblDeducciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblDeducciones" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <label for="lblComprobaciones">T. Comprobaciones</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblComprobaciones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblComprobaciones" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="lblSueldo">T. Sueldo</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblSueldo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblSueldo" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_155px">
                    <label for="lblAlcance">T. Alcance</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblAlcance" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblAlcance" runat="server" Text="0.00" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_155px">
                    <label for="lblDescuentos">T. Descuentos</label>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplblDescuentos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblDescuentos" runat="server" Text="0.00" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon_boton">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnEditar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" TabIndex="9" CssClass="boton_cancelar"
                                OnClick="btnEditar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" TabIndex="10" CssClass="boton"
                                OnClick="btnGuardar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" TabIndex="11" CssClass="boton_cancelar"
                                OnClick="btnCancelar_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCerrarLiquidacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCerrarLiquidacion" runat="server" Text="Cerrar Liq." CssClass="boton_cancelar"
                                OnClick="btnCerrarLiquidacion_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                            <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                            <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                            <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblErrorLiquidacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblErrorLiquidacion" runat="server" CssClass="label_error"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnEditar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                        <asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
                        <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="contenedor_730px_derecha">
            <div class="header_seccion">
                <h2>Viajes Pendientes</h2>
            </div>
            <div class="renglon100per">
                <div class="etiqueta">
                    <label for="ddlTamanoViajes">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoViajes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoViajes" runat="server" TabIndex="12" OnSelectedIndexChanged="ddlTamanoViajes_SelectedIndexChanged"
                                CssClass="dropdown_100px" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoViajes">Ordenado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uplblOrdenadoViajes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoViajes" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplnkExportarViajes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarViajes" runat="server" Text="Exportar" OnClick="lnkExportarViajes_Click"
                                TabIndex="13"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarViajes" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvViajes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvViajes" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                            CssClass="gridview" ShowFooter="True" TabIndex="14" OnSorting="gvViajes_Sorting"
                            OnPageIndexChanging="gvViajes_PageIndexChanging" AutoGenerateColumns="False" Width="100%">
                            <AlternatingRowStyle CssClass="gridviewrowalternate" />
                            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                            <FooterStyle CssClass="gridviewfooter" />
                            <HeaderStyle CssClass="gridviewheader" />
                            <RowStyle CssClass="gridviewrow" />
                            <SelectedRowStyle CssClass="gridviewrowselected" />
                            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                                <asp:TemplateField HeaderText="No. Viaje" SortExpression="NoViaje">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkReferenciasServicio" runat="server" Text='<%# Eval("NoViaje") %>' OnClick="lnkReferenciasServicio_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EstatusViaje" HeaderText="Estatus" SortExpression="EstatusViaje" />
                                <asp:BoundField DataField="ClienteReceptor" HeaderText="Cliente Receptor" SortExpression="ClienteReceptor" />
                                <asp:BoundField DataField="FechaTermino" HeaderText="Fecha Termino" SortExpression="FechaTermino" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                <asp:TemplateField HeaderText="Movimientos" SortExpression="Movimientos">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkMovimientos" runat="server" Text='<%# Eval("Movimientos") %>' OnClick="lnkMovimientos_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbPagarServicio" runat="server" OnClick="lkbPagarServicio_Click">Pagar</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoViajes" />
                        <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="ucReferenciasViaje" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarReferencias" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Movimientos Pendientes</h2>
            </div>
            <div class="renglon100per">
                <div class="etiqueta">
                    <label for="ddlTamanoMov">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoMov" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoMov" runat="server" TabIndex="15" OnSelectedIndexChanged="ddlTamanoMov_SelectedIndexChanged"
                                CssClass="dropdown_100px" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="lblOrdenadoMov">Ordenado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uplblOrdenadoMov" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoMov" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvMovimientos" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="uplnkExportarMov" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarMov" runat="server" Text="Exportar" OnClick="lnkExportarMov_Click"
                                TabIndex="16"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarMov" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_150px_altura">
                <asp:UpdatePanel ID="upgvMovimientos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvMovimientos" runat="server" AllowPaging="True" AllowSorting="True" Width="100%"
                            CssClass="gridview" ShowFooter="True" TabIndex="17" OnSorting="gvMovimientos_Sorting" PageSize="25"
                            OnPageIndexChanging="gvMovimientos_PageIndexChanging" AutoGenerateColumns="False"
                            OnRowDataBound="gvMovimientos_RowDataBound">
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
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkTodos" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkTodos_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkVarios" runat="server" AutoPostBack="true"
                                            OnCheckedChanged="chkTodos_CheckedChanged" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No. Mov." SortExpression="Id">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSeleccionarMovimiento" runat="server" Text='<%# Eval("Id") %>' OnClick="lnkSeleccionarMovimiento_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
                                <asp:TemplateField HeaderText="Estatus Documentos" SortExpression="EstatusDoc">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="lnkEvidencias_Click" Text='<%# Eval("EstatusDoc") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Kms" HeaderText="Km's" SortExpression="Kms" />
                                <asp:BoundField DataField="NoPago" HeaderText="No. Pago" SortExpression="NoPago" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditarPago" runat="server" Text="" OnClick="lnkEditarPago_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
                        <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                        <asp:AsyncPostBackTrigger ControlID="gvPagos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrar" />
                        <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCrearPago" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCrearPago" runat="server" Text="Crear Pago" CssClass="boton"
                                Visible="false" TabIndex="18" OnClick="btnCrearPago_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:View>
<asp:View ID="vwResumen" runat="server">
    <div class="contenedor_seccion_95per">
        <div class="header_seccion">
            <h2>Visualizar Liquidación</h2>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvResumenLiquidacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvResumenLiquidacion" runat="server" AllowPaging="false" AllowSorting="false"
                        CssClass="gridview" ShowFooter="true" AutoGenerateColumns="false"
                        OnRowDataBound="gvResumenLiquidacion_RowDataBound" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="IdTipoRegistro" HeaderText="Tipo Registro" SortExpression="IdTipoRegistro" Visible="false" />
                            <asp:BoundField DataField="Entidad" HeaderText="Entidad" SortExpression="Entidad" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                            <asp:BoundField DataField="No" HeaderText="No" SortExpression="No" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                            <asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:C2}" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnResumen" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="renglon100Per">
            <div class="controlr">
                <asp:UpdatePanel ID="uplkbTimbrar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbTimbrar" runat="server" Text="Timbrar" CommandName="Timbrar" OnClick="lkbReciboNomina_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlr">
                <asp:UpdatePanel ID="uplkbCancelarTimbrado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCancelarTimbrado" runat="server" CommandName="Cancelar" Text="Cancelar Recibo de Nómina" OnClick="lkbReciboNomina_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlr">
                <asp:UpdatePanel ID="uplnkExportarResumenLiq" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarResumenLiq" runat="server" Text="Exportar Resumen"
                            OnClick="lnkExportarResumenLiq_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarResumenLiq" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlr">
                <asp:UpdatePanel ID="uplnkImprimirLiquidacion" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkImprimirLiquidacion" runat="server" Text="Imprimir Liquidación"
                            OnClick="lnkImprimirLiquidacion_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkImprimirLiquidacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:View>
</asp:MultiView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnBusqueda" />
<asp:AsyncPostBackTrigger ControlID="btnLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnResumen" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
    
    <div class="contenedor_media_seccion_izquierda">
        <div class="header_seccion">
            <h2>Pagos Asignados</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoPago">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoPago" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoPago" runat="server" TabIndex="19" OnSelectedIndexChanged="ddlTamanoPago_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoPago">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenadoPago" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoPago" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvPagos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarPago" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarPago" runat="server" Text="Exportar" OnClick="lnkExportarPago_Click"
                            TabIndex="20"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarPago" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCrearOtrosPagos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCrearOtrosPagos" runat="server" Text="Crear Pago" OnClick="btnCrearOtrosPagos_Click" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>

                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvPagos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvPagos" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
                        CssClass="gridview" ShowFooter="true" TabIndex="21" OnSorting="gvPagos_Sorting"
                        OnPageIndexChanging="gvPagos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="IdPago" HeaderText="No. Pago" SortExpression="IdPago" Visible="true" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                            <asp:BoundField DataField="ValorU" HeaderText="Valor U." SortExpression="ValorU" />
                            <asp:TemplateField HeaderText="Total" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblSumaTotal" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditarPago" runat="server" OnClick="lnkEditarPagoGnrl_Click" Text="Editar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminar" runat="server" OnClick="lnkEliminar_Click" Text="Eliminar"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarOperacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvTarifasPago" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_media_seccion_derecha">
        <div class="header_seccion">
            <h2>Cobros Recurrentes</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoCRL">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoCRL" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoCRL" runat="server" TabIndex="32" OnSelectedIndexChanged="ddlTamanoCRL_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoCRL">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenadoCRL" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoCRL" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvCobroRecurrenteLiquidacion" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarCRL" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarCRL" runat="server" Text="Exportar" OnClick="lnkExportarCRL_Click"
                            TabIndex="33"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarCRL" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnVerCobrosPendientes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnVerCobrosPendientes" runat="server" Text="Ver Pendientes" OnClick="btnVerCobrosPendientes_Click"
                            TabIndex="35" CssClass="boton" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                        <asp:AsyncPostBackTrigger ControlID="ddlTamanoCRL" />
                        <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                        <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                        <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvCobroRecurrenteLiquidacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvCobroRecurrenteLiquidacion" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
                        CssClass="gridview" ShowFooter="true" TabIndex="34" OnSorting="gvCobroRecurrenteLiquidacion_Sorting"
                        OnPageIndexChanging="gvCobroRecurrenteLiquidacion_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                            <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVerHistorial" runat="server" OnClick="lnkVerHistorial_Click"
                                        Text='<%# Eval("Descripcion") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                            <asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoCRL" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarVentanaCRP" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_media_seccion_izquierda">
        <div class="header_seccion">
            <h2>Anticipos Asignados</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoAnticipos">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoAnticipos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoAnticipos" runat="server" TabIndex="22" OnSelectedIndexChanged="ddlTamanoAnticipos_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoAnticipo">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenadoAnticipo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoAnticipo" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvAnticipos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarAnticipos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarAnticipos" runat="server" Text="Exportar" OnClick="lnkExportarAnticipos_Click"
                            TabIndex="23"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarAnticipos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvAnticipos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvAnticipos" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
                        CssClass="gridview" ShowFooter="true" TabIndex="24" OnSorting="gvAnticipos_Sorting"
                        OnPageIndexChanging="gvAnticipos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="No. Deposito" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoDeposito" HeaderText="No. Deposito" SortExpression="NoDeposito" />
                            <asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
                            <asp:BoundField DataField="Metodo" HeaderText="Método" SortExpression="Metodo" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionarDeposito" runat="server" Text="Comprobar"
                                        OnClick="lnkSeleccionarDeposito_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_media_seccion_derecha">
        <div class="header_seccion">
            <h2>Comprobaciones Realizadas</h2>            
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoComp">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoComp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoComp" runat="server" TabIndex="25" OnSelectedIndexChanged="ddlTamanoComp_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoComp">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenadoComp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoComp" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarComp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarComp" runat="server" Text="Exportar" OnClick="lnkExportarComp_Click"
                            TabIndex="26"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarComp" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnCrearComp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnCrearComp" runat="server" Text="Crear" TabIndex="28" CssClass="boton"
                            OnClick="btnCrearComp_Click" />
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvComprobaciones" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvComprobaciones" runat="server" AllowPaging="true" AllowSorting="true"
                        CssClass="gridview" ShowFooter="true" TabIndex="27" OnSorting="gvComprobaciones_Sorting"
                        OnPageIndexChanging="gvComprobaciones_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                            <asp:BoundField DataField="NoFacturas" HeaderText="No. Facturas" SortExpression="NoFacturas" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEditaComp" runat="server" Text="Editar"
                                        OnClick="lnkEditaComp_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEliminaComp" runat="server" Text="Eliminar"
                                        OnClick="lnkEliminaComp_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_media_seccion_izquierda">
        <div class="header_seccion">
            <h2>Vales de Diesel Asignados</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoDiesel">Mostrar</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="upddlTamanoDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoDiesel" runat="server" TabIndex="29" OnSelectedIndexChanged="ddlTamanoDiesel_SelectedIndexChanged"
                            CssClass="dropdown_100px" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoAnticipo">Ordenado</label>
            </div>
            <div class="control_100px">
                <asp:UpdatePanel ID="uplblOrdenadoDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoDiesel" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvDiesel" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50px">
                <asp:UpdatePanel ID="uplnkExportarDiesel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarDiesel" runat="server" Text="Exportar" OnClick="lnkExportarDiesel_Click"
                            TabIndex="30"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarDiesel" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_200px_altura">
            <asp:UpdatePanel ID="upgvDiesel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvDiesel" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
                        CssClass="gridview" ShowFooter="true" TabIndex="31" OnSorting="gvDiesel_Sorting"
                        OnPageIndexChanging="gvDiesel_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate"/>
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="No. Deposito" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="NoVale" HeaderText="No. Vale" SortExpression="NoVale" />
                            <asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
                            <asp:BoundField DataField="Costo" HeaderText="Costo Combustible" SortExpression="Costo" />
                            <asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoDiesel" />
                    <asp:AsyncPostBackTrigger ControlID="btnCrear" />
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
                    <asp:AsyncPostBackTrigger ControlID="gvViajes" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                    <asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

<div id="contenedorVentanaPagos" class="modal">
<div id="ventanaPagos" class="contenedor_ventana_confirmacion">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar" TabIndex="36">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Pagos</h2>
</div>
<div class="columna2x">
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipoPago">Tipo de Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoPago" runat="server" CssClass="dropdown" Enabled="false" TabIndex="37"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtCantidad">Cantidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidad" runat="server" CssClass="textbox validate[required, custom[positiveNumber4]]" Enabled="false" 
TabIndex="38" MaxLength="9" OnTextChanged="txtCantidad_TextChanged" AutoPostBack="true"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtValorU">Valor Unitario</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValorU" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorU" runat="server" CssClass="textbox validate[required, custom[positiveNumber]]" Enabled="false" 
TabIndex="39" MaxLength="9" OnTextChanged="txtValorU_TextChanged" AutoPostBack="true"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtCantidad" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtTotal">Total</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtTotal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtTotal" runat="server" CssClass="textbox validate[required, custom[positiveNumber4]]" Enabled="false" 
TabIndex="40" MaxLength="9"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtCantidad" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="41" Enabled="false"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtReferencia">Referencia</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtReferencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReferencia" runat="server" CssClass="textbox" TabIndex="42"
MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="txtValorU" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarPago" runat="server" CssClass="boton" Text="Guardar Pago" OnClick="btnGuardarPago_Click"
TabIndex="43" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarPago" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelarPago_Click" 
TabIndex="44" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<asp:UpdatePanel ID="uplblErrorPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorPago" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoMov" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvPagos" />
<asp:AsyncPostBackTrigger ControlID="btnCrearOtrosPagos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenedorVentanaComprobaciones" class="modal">
<div id="ventanaComprobaciones" style="z-index:1002;display:none;position:fixed;background-color:#FFF;border: 1px solid #808080; 
left: 300px;top: 200px;width:1000px;height:auto;padding:10px;">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarComprobacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarComprobacion" runat="server" OnClick="lnkCerrarComprobacion_Click" Text="Cerrar" TabIndex="45">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Comprobaciones</h2>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblIdComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblIdComprobacion" runat="server" Text="Por Asignar"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="ddlConcepto">Concepto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown" TabIndex="46"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtObservacion">Observación</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtObservacion" runat="server" TabIndex="47" CssClass="textbox validate[required]" MaxLength="255"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">
<label for="txtValorUnitario">Monto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValorUnitario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValorUnitario" runat="server" TabIndex="48" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="15"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarComprobacion" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnGuardarComprobacion_Click" TabIndex="49" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarComprobacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarComprobacion_Click" TabIndex="50" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<asp:UpdatePanel ID="uplblErrorComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorComprobacion" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna">
<div id="box">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div>
<div class="renglon">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarFactura" runat="server" Text="Agregar Factura" CssClass="boton" TabIndex="51"
OnClick="btnAgregarFactura_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna">
<div class="renglon">
<div class="etiqueta_50px">
<label for="ddlTamanoFacComp">Mostrar:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="upddlTamanoFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoFacComp" runat="server" CssClass="textbox_50px" TabIndex="52" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoFacComp_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="">Ordenado:</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uplblOrdenadoFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoFacComp" runat="server"></asp:Label></b>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarFacComp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarFacComp" runat="server" Text="Exportar" TabIndex="53"
OnClick="lnkExportarFacComp_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarFacComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvFacturasComprobacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvFacturasComprobacion" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="54" OnSorting="gvFacturasComprobacion_Sorting"
OnPageIndexChanging="gvFacturasComprobacion_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdFactura" HeaderText="Id" SortExpression="IdFactura" Visible="false" />
<asp:BoundField DataField="IdCompFact" HeaderText="IdComp" SortExpression="IdCompFact" Visible="false" />
<asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
<asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" />
<asp:BoundField DataField="Subtotal" HeaderText="Sub Total" SortExpression="Subtotal" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
<asp:BoundField DataField="Trasladado" HeaderText="IVA Trasladado" SortExpression="Trasladado" />
<asp:BoundField DataField="Retenido" HeaderText="IVA Retenido" SortExpression="Retenido" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eilminar" 
OnClick="lnkEliminarFactura_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoFacComp" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="gvAnticipos" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
<asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
<asp:AsyncPostBackTrigger ControlID="btnCrearComp" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenedorVentanaConfirmacion" class="modal">
<div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>El Pago se eliminará. ¿Desea quitar los anticipos y comprobaciones?</h2>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarOperacion" runat="server" Text="Aceptar" CssClass="boton"
OnClick="btnAceptarOperacion_Click" TabIndex="55" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarOperacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarOperacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarOperacion_Click" TabIndex="56" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="contenedorVentanaCobrosPendientes" class="modal">
<div id="ventanaCobrosPendientes" class="contenedor_ventana_confirmacion">
<div class="columna2x">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarVentanaCRP" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaCRP" runat="server" OnClick="lnkCerrarVentanaCRP_Click" Text="Cerrar" TabIndex="57">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Cobros Recurrentes</h2>
</div>
<div class="renglon_pestaña_documentacion">
<div class="etiqueta_50px">
<label for="ddlTamanoCR">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCR" runat="server" TabIndex="58" OnSelectedIndexChanged="ddlTamanoCR_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCR">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCR" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCR" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCR" runat="server" Text="Exportar" OnClick="lnkExportarCR_Click"
TabIndex="59"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCR" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvCobrosRecurrentes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobrosRecurrentes" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="60" OnSorting="gvCobrosRecurrentes_Sorting"
OnPageIndexChanging="gvCobrosRecurrentes_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="DiasTranscurridos" HeaderText="Dias Transcurridos" SortExpression="DiasTranscurridos" />
<asp:BoundField DataField="DiasCobro" HeaderText="Dias Cobro" SortExpression="DiasCobro" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCR" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
    
</div>
<div id="contenedorVentanaHistorialCobrosRecurrentes" class="modal">
<div id="ventanaHistorialCobrosRecurrentes" class="contenedor_ventana_confirmacion">
<div class="columna2x">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarVentanaCRH" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarVentanaCRH" runat="server" OnClick="lnkCerrarVentanaCRH_Click" Text="Cerrar" TabIndex="61">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Historial Cobros Recurrentes</h2>
</div>
<div class="renglon_pestaña_documentacion">
<div class="etiqueta_50px">
<label for="ddlTamanoCRH">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCRH" runat="server" TabIndex="62" OnSelectedIndexChanged="ddlTamanoCRH_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRH">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoCRH" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCobrosRecurrentesHistorial" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarCRH" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCRH" runat="server" Text="Exportar" OnClick="lnkExportarCRH_Click"
TabIndex="63"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCRH" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvCobrosRecurrentesHistorial" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCobrosRecurrentesHistorial" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="64" OnSorting="gvCobrosRecurrentesHistorial_Sorting"
OnPageIndexChanging="gvCobrosRecurrentesHistorial_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="NoLiquidacion" HeaderText="No. Liquidación" SortExpression="NoLiquidacion" Visible="false" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
<asp:BoundField DataField="MontoCobro" HeaderText="Monto Cobro" SortExpression="MontoCobro" />
<asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarPago" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarPago" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCR" />
<asp:AsyncPostBackTrigger ControlID="btnCrear" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="gvLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
<asp:AsyncPostBackTrigger ControlID="btnCerrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="gvCobroRecurrenteLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenidoVentanaEvidencias" class="modal">
<div id="ventanaEvidencias" class="contenedor_ventana_confirmacion">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion"> 
<h2>Evidencias del Viaje</h2>
</div>
<div class="renglon_pestaña_documentacion">
<div class="etiqueta_50px">
<label for="ddlTamanoEvidencia">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoEvidencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoEvidencia" runat="server" TabIndex="65" OnSelectedIndexChanged="ddlTamanoEvidencia_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCRH">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoEvidencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoEvidencias" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvEvidencias" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarEvidencia" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarEvidencia" runat="server" Text="Exportar" OnClick="lnkExportarEvidencia_Click"
TabIndex="66"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarEvidencia" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvEvidencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvEvidencias" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="67" OnSorting="gvEvidencias_Sorting"
OnPageIndexChanging="gvEvidencias_PageIndexChanging" AutoGenerateColumns="false"
OnRowDataBound="gvEvidencias_RowDataBound">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField>
<ItemTemplate>
<asp:ImageButton ID="ibEstatus" runat="server" ImageUrl="~/Image/EstatusRecibido.png" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
<asp:BoundField DataField="IdEstatus" HeaderText="IdEstatus" SortExpression="IdEstatus" Visible="false" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Remitente" HeaderText="Remitente" SortExpression="Remitente" />
<asp:BoundField DataField="Destinatario" HeaderText="Destinatario" SortExpression="Destinatario" />
<asp:BoundField DataField="LugarCobro" HeaderText="Lugar de Cobro" SortExpression="LugarCobro" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoEvidencia" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div id="contenedorVentanaTarifasPago" class="modal">
<div id="ventanaTarifasPago" class="contenedor_ventana_confirmacion">
<div class="columna2x">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaTarifasPago" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarVentanaTarifasPago" OnClick="lkbCerrarVentanaTarifasPago_Click" runat="server" Text="Cerrar" TabIndex="61">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Tarifas de Pago Aplicables</h2>
</div>
<div class="renglon_pestaña_documentacion">
<div class="etiqueta_50px">
<label for="ddlTamanoGridViewTarifasAplicables">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoGridViewTarifasAplicables" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoGridViewTarifasAplicables" runat="server" TabIndex="62"
CssClass="dropdown_100px" AutoPostBack="true" OnSelectedIndexChanged="ddlTamanoGridViewTarifasAplicables_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoTarifasAplicables">Ordenado</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoTarifasAplicables" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoTarifasAplicables" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvTarifasPago" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplkbExportarTarifasAplicales" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarTarifasAplicales" runat="server" Text="Exportar" OnClick="lkbExportarTarifasAplicales_Click"
TabIndex="63"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarTarifasAplicales" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<asp:UpdatePanel ID="upgvTarifasPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvTarifasPago" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="64" OnSorting="gvTarifasPago_Sorting"
OnPageIndexChanging="gvTarifasPago_PageIndexChanging" AutoGenerateColumns="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" Width="100%" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="IdTarifa" HeaderText="Id" SortExpression="IdTarifa" />
<asp:BoundField DataField="Aproximacion" HeaderText="Aproximación" SortExpression="Aproximacion" ItemStyle-HorizontalAlign="Center" />
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="Base" HeaderText="Base" SortExpression="Base" />
<asp:BoundField DataField="NivelPago" HeaderText="NivelPago" SortExpression="NivelPago" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbSeleccionarTarifaPago" runat="server" Text="Aplicar" OnClick="lkbSeleccionarTarifaPago_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoGridViewTarifasAplicables" />
<asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div id="contenidoConfirmacionTimbrarLiquidacion" class="modal">
<div id="confirmacionTimbrarLiquidacion"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarTimbrarLiquidacion" UpdateMode="Conditional"  >
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarTimbrarLiquidacion" runat="server" Text="Cerrar"  OnClick="lkbCerrarTimbrarLiquidacion_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>Timbrar Liquidacion</h3>
<div class="columna"> 
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTimbrarLiquidacion">Sucursal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSucursal" runat="server"   CssClass="dropdown2x" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="upddlPeriocidadPago">Periocidad Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlPeriocidadPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPeriocidadPago" runat="server"   CssClass="dropdown" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="upddlMetodoPago">Mètodo Pago</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server"   CssClass="dropdown" ></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorTimbrarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorTimbrarLiquidacion" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarTimbrarLiquidacion" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbrarLiquidacion" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbrarLiquidacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbrarLiquidacion" runat="server"  OnClick="btnAceptarTimbrarLiquidacion_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div></div>
<div id="contenidoConfirmacionCancelarTimbrado" class="modal">
<div id="confirmacionCancelarTimbrado"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCancelarTimbrado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCancelarTimbrado" runat="server" Text="Cerrar"  OnClick="lkbCerrarCancelarTimbrado_Click" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>CancelarTimbrado</h3>
<div class="columna"> 
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea cancelar el Recibo de Nómina?</label>
</div>
<div class="renglon2x">
<div class="control" style="width: auto">
<asp:UpdatePanel ID="uplblErrorCancelarTimbrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblErrorCancelarTimbrado" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarCancelarTimbrado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarTimbrado" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCancelarTimbrado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelarTimbrado" runat="server"   OnClick="btnAceptarCancelarTimbrado_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>
<!-- Ventana de Referencias de Viaje -->
<div id="contenedorVentanaReferenciasViaje" class="modal">
<div id="ventanaReferenciasViaje" class="contenedor_ventana_confirmacion" style="width:300px;">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarReferencias" runat="server" OnClick="lnkCerrarReferencias_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Referencias del Viaje</h2>
</div>
<asp:UpdatePanel ID="upucReferenciasViaje" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucReferenciaViaje ID="ucReferenciasViaje" runat="server" OnClickGuardarReferenciaViaje="ucReferenciasViaje_ClickGuardarReferenciaViaje"
OnClickEliminarReferenciaViaje="ucReferenciasViaje_ClickEliminarReferenciaViaje" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvViajes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
