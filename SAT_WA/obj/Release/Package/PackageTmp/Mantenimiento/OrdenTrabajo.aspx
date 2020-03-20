<%@ Page Title="Orden de Trabajo" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrdenTrabajo.aspx.cs" Inherits="SAT.Mantenimiento.OrdenTrabajo" %>
<%@ Register  Src="~/UserControls/ActividadOrdenTrabajo.ascx"  TagPrefix="tectos" TagName="wucActividadOrdenTrabajo" %>
<%@ Register  Src="~/UserControls/wucLecturaHistorial.ascx"  TagPrefix="tectos" TagName="wucLecturaHistorial" %>
<%@ Register  Src="~/UserControls/wucLectura.ascx"  TagPrefix="tectos" TagName="wucLectura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Estilos Propios de la Página -->
<style>
.NivelCombustible .ui-slider-range { 
background: #8ae234;
}

#contenedorXML {
margin-top: 10px;
margin-left: 10px;
margin-bottom:20px;
width: 400px;
height: 120px;
text-align: center;
vertical-align: middle;
border: 2px solid #939393;
background-color: #f8f8f8;
padding: 15px;
font-family: Arial;
font-size: 16px;
}
</style>
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!-- Libreria para Carga de Archivos -->
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryOrdenTrabajo();
}
}

//Declarando Función de Configuración
function ConfiguraJQueryOrdenTrabajo() {
    $(document).ready(function () {

        //Evento de Cambio
        $("#<%=txtNivelCombustible.ClientID%>").change(function () {

            //Obteniendo Valor
            var nivelC = this.value;

            //Validando que el Valor no Exceda de 100
            if (parseInt(nivelC) > 100) {

                //Devolviendo valor
                !$("#<%=txtNivelCombustible.ClientID%>").validationEngine('validate');

                //Asignando Valor a 100
                $("#<%=txtNivelCombustible.ClientID%>").val(100);
            }

            //Asignando Valor
            $("#<%=ctdNivelCombustible.ClientID%>").slider('value', nivelC);
        });

        //Cargando Controles de Fecha
        $("#<%=txtFecFin.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });

        //Función de Validación
        $("#<%=txtNivelCombustible.ClientID%>").keydown(function (e) {
            // Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
            if ($.inArray(e.keyCode, [8, 9, 27, 13]) !== -1 ||
                // Permite Funciones: Ctrl+A, Command+A
            (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Permite Teclas de Movimiento: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });

        //Función de Validación
        var validaOrdenTrabajo = function () {
            var isValid1 = !$("#<%=txtFechaRecepcion.ClientID%>").validationEngine('validate');
            var isValid2 = !$("#<%=txtFechaCompromiso.ClientID%>").validationEngine('validate');
            var isValid3 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
            var isValid4 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
            var isValid5;
            var isValid6;
            var isValid7;
            var isValid8;
            var isValid9;

            if ($("#<%=chkBitUnidadExt.ClientID%>").is(':checked') == true) {
                //Asignando Validadores
                isValid5 = true;
                isValid6 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                isValid7 = !$("#<%=txtDescUnidad.ClientID%>").validationEngine('validate');
            }
            else {
                //Asignando Validadores
                isValid5 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
                isValid6 = true;
                isValid7 = true;
            }

            if ($("#<%=ddlTipoTaller.ClientID%>").val() == "1") {
                //Asignando Validador
                isValid8 = true;
                isValid9 = true;
            }
            else if ($("#<%=ddlTipoTaller.ClientID%>").val() == "2") {
                //Asignando Validador
                isValid8 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                isValid9 = !$("#<%=txtNoSiniestro.ClientID%>").validationEngine('validate');
            }
            else {
                //Asignando Validador
                isValid8 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                isValid9 = true;
            }

            var isValid10 = !$("#<%=txtTaller.ClientID%>").validationEngine('validate');
            var isValid11 = !$("#<%=txtOdometro.ClientID%>").validationEngine('validate');
            var isValid12 = !$("#<%=txtNivelCombustible.ClientID%>").validationEngine('validate');
            var isValid13 = !$("#<%=txtEntregadoPor.ClientID%>").validationEngine('validate');
            var isValid14 = !$("#<%=txtRecibidoPor.ClientID%>").validationEngine('validate');

            //Devolviendo Resultados Obtenidos
            return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 &&
            isValid6 && isValid7 && isValid8 && isValid9 && isValid10 &&
            isValid11 && isValid12 && isValid13 && isValid14;
        }


        //Añadiendo Validación a Controles
        $("#<%=btnGuardar.ClientID%>").click(validaOrdenTrabajo);
        $("#<%=lkbGuardar.ClientID%>").click(validaOrdenTrabajo);

        //Cargando Controles DateTime
        $("#<%=txtFechaRecepcion.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
        $("#<%=txtFechaCompromiso.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
        $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
        $("#<%=txtFechaFin.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });

        //Cargando Catalogos AutoCompleta
        $("#<%=txtCliente.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
        $("#<%=txtProveedor.ClientID%>").autocomplete({ source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>' });
        $("#<%=txtUnidad.ClientID%>").autocomplete({
            source: '../WebHandlers/AutoCompleta.ashx?id=28&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
            select: function (event, ui) {
                //Asignando Selección al Valor del Control
                $("#<%=txtUnidad.ClientID%>").val(ui.item.value);
                //Causando Actualización del Control
                __doPostBack('<%= txtUnidad.UniqueID %>', '');
            }
        });
        //Cargando Proveedor

        //Función de Validación
        var validaOrdenTrabajoFalla = function () {
            var isValid1 = !$("#<%=txtFechaFalla.ClientID%>").validationEngine('validate');
            var isValid2 = !$("#<%=txtDescripcionFallo.ClientID%>").validationEngine('validate');
            //Devolviendo Resultados Obtenidos
            return isValid1 && isValid2;
        }
        //Añadiendo Validación a Controles
        $("#<%=btnGuardarFalla.ClientID%>").click(validaOrdenTrabajoFalla);
        $("#<%=txtFechaFalla.ClientID%>").datetimepicker({
            lang: 'es',
            format: 'd/m/Y H:i'
        });
    });
}

//Declarando Función de Validación de Fechas
function CompareDates() {
    //Obteniendo Valores
    var nivelCombustible = $("#<%=txtNivelCombustible.ClientID%>").val();

    //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
    if (nivelCombustible > 100)
        //Mostrando Mensaje de Operación
        return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}

//Invocando Función de Configuración
ConfiguraJQueryOrdenTrabajo();

/**Script Contenedor de Archivos**/
//Declarando variable contenedora de Archivos
var selectedFiles;
//Función que limpia el Contenedor
function LimpiaContenedorXML() {   //Limpiando DIV
    selectedFiles = null;
    $("#contenedorXML").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
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
    box = document.getElementById("contenedorXML");
    //Añadiendo Eventos
    box.addEventListener("dragenter", OnDragEnter, false);
    box.addEventListener("dragover", OnDragOver, false);
    box.addEventListener("drop", OnDrop, false);

});
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
        //Leyendo Texto
        lector.readAsText(selectedFiles[0]);
        //Mostrando mensaje
        alert('El Archivo se ha Cargado')
        //Indicando Archivo
        $("#contenedorXML").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
    };
    //Evento al Producirse un Error
    lector.onerror = function (evt) {
        //Mostrando Error
        alert('Error Carga ' + evt.target.error);
        //Inicializando Mensaje
        $("#contenedorXML").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
    };
}
</script>
    <div id="encabezado_forma">
        <img src="../Image/Modulos.png" />
        <h1>Orden de Trabajo</h1>
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
                                <asp:LinkButton ID="lkbAgregarActividad" runat="server" Text="Agregar Actividad" OnClick="lkbElementoMenu_Click" CommandName="Actividad" /></li>
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
                                <asp:LinkButton ID="lkbTerminar" runat="server" Text="Terminar" OnClick="lkbElementoMenu_Click" CommandName="Terminar" /></li>
                        </ul>
                    </li>
                    <li class="yellow">
                        <a href="#" class="fa fa-file-archive-o"></a>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lkbFacturar" runat="server" Text="Facturar" OnClick="lkbElementoMenu_Click" CommandName="Facturar" /></li>
                            <li>
                                <asp:LinkButton ID="lkbFacturaProveedor" runat="server" Text="F. Proveedor" OnClick="lkbElementoMenu_Click" CommandName="FacturaProveedor" /></li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:PostBackTrigger ControlID="lkbBitacora" />
            <asp:PostBackTrigger ControlID="lkbReferencias" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="contenedor_controles">
        <div class="seccion_controles">
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="lblId">No Orden</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblId" runat="server">Por Asignar</asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="lblFacturada">Facturada</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblFacturada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblFacturada" runat="server">--</asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlEstatus">Estatus</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="1"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlTipo">Tipo</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaInicio">Fecha Inicio</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16"
                                    TabIndex="3" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaFin">Fecha Fin</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16"
                                    TabIndex="4" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaRecepcion">F. Recepción</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFechaRecepcion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaRecepcion" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16" TabIndex="5"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtFechaCompromiso">F. Compromiso</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtFechaCompromiso" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFechaCompromiso" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="16" TabIndex="6"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtOdometro">Odometro</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtOdometro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtOdometro" runat="server" CssClass="textbox validate[required,custom[positiveNumber]]" TabIndex="7" MaxLength="9"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtNivelCombustible">Nivel Combustible</label>
                    </div>
                    <div class="etiqueta" style="width: 290px; float: left;">
                        <div id="ctdNivelCombustible" runat="server" class="NivelCombustible"></div>
                    </div>
                    <div class="control_60px" style="float: right">
                        <asp:UpdatePanel ID="uplblNivelCombustible" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNivelCombustible" runat="server" CssClass="textbox_50px validate[required,custom[integer],max[100]]"
                                    Style="text-align: right" MaxLength="3" TabIndex="8"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtEntregadoPor">Entregado Por</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtEntregadoPor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEntregadoPor" runat="server" CssClass="textbox2x validate[required]" TabIndex="9" MaxLength="100"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtRecibidoPor">Recibido Por</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtRecibidoPor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtRecibidoPor" runat="server" CssClass="textbox2x validate[required]" TabIndex="10" MaxLength="100"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta_320px">
                        <asp:UpdatePanel ID="upchkBitUnidadExt" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkBitUnidadExt" runat="server" Text="¿La Unidad es Externa?" TabIndex="11" AutoPostBack="true"
                                    OnCheckedChanged="chkBitUnidadExt_CheckedChanged" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uplkbLectura" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbLectura" runat="server" Text="Lectura" OnClick="lkbLectura_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUnidad">No. Unidad</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="12"
                                    AutoPostBack="true" OnTextChanged="txtUnidad_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtDescUnidad">Descripción</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtDescUnidad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDescUnidad" runat="server" CssClass="textbox2x validate[required]" TabIndex="13" MaxLength="50"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="txtUnidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtEstatus">Estatus Unidad</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel runat="server" ID="uptxtEstatus" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEstatus" runat="server" CssClass="textbox" TabIndex="14"
                                    AutoPostBack="true" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtUnidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUbicacion">Ubicación Unidad</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel runat="server" ID="uptxtUbicacion" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x" TabIndex="15"
                                    AutoPostBack="true" Enabled="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtUnidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlTipoUnidad">Tipo Unidad</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTipoUnidad" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoUnidad" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTipoUnidad_SelectedIndexChanged" AutoPostBack="true" TabIndex="16"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="txtUnidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlSubTipo">Sub Tipo</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlSubTipo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSubTipo" runat="server" CssClass="dropdown" TabIndex="17"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="txtUnidad" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTipoUnidad" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCliente">Propietario Unidad</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCliente" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="18"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="chkBitUnidadExt" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlTipoTaller">Tipo Taller</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTipoTaller" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoTaller" runat="server" CssClass="dropdown" TabIndex="19" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTipoTaller_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtProveedor">Propietario Taller</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="20"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTipoTaller" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtTaller">Taller</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtTaller" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtTaller" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="21"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTipoTaller" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtNoSiniestro">No. Siniestro</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtNoSiniestro" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtNoSiniestro" runat="server" CssClass="textbox validate[required]" TabIndex="22" MaxLength="150"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTipoTaller" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardar" runat="server" CssClass="boton" OnClick="btnGuardar_Click"
                                    TabIndex="24" Text="Guardar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" OnClick="btnCancelar_Click"
                                    TabIndex="23" Text="Cancelar" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_botones_pestaña">
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaFallas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaFallas" runat="server" Text="Fallas" CssClass="boton_pestana_activo" CommandName="Fallas" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaManoObra" />
                    <asp:AsyncPostBackTrigger ControlID="btnRefaccionesConsumidas" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnActividades" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestnaActividades" runat="server" Text="Actividades" CssClass="boton_pestana" CommandName="Actividades" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaFallas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaManoObra" />
                    <asp:AsyncPostBackTrigger ControlID="btnRefaccionesConsumidas" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnPestanaManoObra" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnPestanaManoObra" runat="server" Text="Mano de Obra" CssClass="boton_pestana" CommandName="ManoObra" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaFallas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                    <asp:AsyncPostBackTrigger ControlID="btnRefaccionesConsumidas" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="control_boton_pestana">
            <asp:UpdatePanel ID="upbtnRefaccionesConsumidas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnRefaccionesConsumidas" runat="server" Text="Refacciones" CssClass="boton_pestana" CommandName="RefaccionesConsumidas" OnClick="btnPestana_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaFallas" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                    <asp:AsyncPostBackTrigger ControlID="btnPestanaManoObra" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenido_tabs">
        <asp:UpdatePanel ID="upmtvSecciones" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:MultiView ID="mtvSecciones" runat="server" ActiveViewIndex="0">
                    <asp:View ID="vwFallas" runat="server">

                        <div class="header_seccion">
                            <h2>Fallas Reportadas</h2>
                        </div>
                        <div class="seccion_controles" style="width: 1100px; min-height: 4px">
                            <div class="encabezado_busqueda">
                                <div class="columna" style="width: 560px">
                                    <div class="renglon">
                                        <div class="etiqueta">
                                            <label for="txtDescripcionFallo">Descripción</label>
                                        </div>
                                        <div class="control">
                                            <asp:UpdatePanel ID="uptxtDescripcionFallo" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtDescripcionFallo" runat="server" CssClass="textbox3x validate[required]" MaxLength="150" TabIndex="25"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarFalla" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarFalla" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvFallas" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="columna" style="width: 250px">
                                    <div class="renglon">
                                        <div class="etiqueta_50px">
                                            <label for="txtFechaFalla">Fecha</label>
                                        </div>
                                        <div class="control_100px">
                                            <asp:UpdatePanel ID="uptxtFechaFalla" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtFechaFalla" runat="server" CssClass="textbox validate[required,custom[dateTime24]]" MaxLength="16" TabIndex="26"></asp:TextBox>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardarFalla" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelarFalla" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvFallas" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="columna" style="width: 250px">
                                    <div class="renglon">
                                        <div class="control_100px">
                                            <asp:UpdatePanel ID="upbtnGuardarFalla" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGuardarFalla" runat="server" CssClass="boton" OnClick="btnGuardarFalla_Click"
                                                        TabIndex="27" Text="Guardar" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="control_100px">
                                            <asp:UpdatePanel ID="upbtnCancelarFalla" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnCancelarFalla" runat="server" CssClass="boton_cancelar" OnClick="btnCancelarFalla_Click"
                                                        TabIndex="28" Text="Cancelar" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="contenedor_controles" style="width: 1100px">
                            <div class="renglon3x">
                                <div class="etiqueta_50px">
                                    <label for="ddlTamano">Mostrar</label>
                                </div>
                                <div class="control">
                                    <label for="ddlTamano"></label>
                                    <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="29" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="etiqueta">
                                    <label>Ordenado Por:</label>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplblOrdenar" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblOrdenar" runat="server"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvFallas" EventName="Sorting" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="etiqueta">
                                    <asp:UpdatePanel ID="uplnkExportarFallas" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkExportarFallas" runat="server" OnClick="lnkExportar_Click" CommandName="Fallas" Text="Exportar" TabIndex="30"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lnkExportarFallas" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="grid_seccion_completa_media_altura">
                                <asp:UpdatePanel ID="upgvFallas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvFallas" runat="server" TabIndex="31" AllowPaging="true" AllowSorting="true"
                                            OnPageIndexChanging="gvFallas_PageIndexChanging" OnSorting="gvFallas_Sorting"
                                            CssClass="gridview" AutoGenerateColumns="false" Width="97%" PageSize="10">
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
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEditar" runat="server" Text="Editar" OnClick="lnkEditar_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEliminar" runat="server" Text="Eliminar" OnClick="lnkEliminar_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAgregarActividad" runat="server" Text="Agregar Actividad" OnClick="lnkAgregarActividad_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                        <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardarFalla" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancelarFalla" />
                                        <asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
                                        <asp:AsyncPostBackTrigger ControlID="btnEliminarActividadFalla" />
                                        <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </asp:View>
                    <asp:View ID="vwActividades" runat="server">
                        <!--Grid View Actividades Ligadas a orden de trabajo-->
                        <div class="header_seccion">
                            <h2>Actividades</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamanoActividad">Mostrar</label>
                            </div>
                            <div class="control">
                                <label for="ddlTamanoActividad"></label>
                                <asp:DropDownList ID="ddlTamanoActividad" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoActividad_SelectedIndexChanged" TabIndex="32">
                                </asp:DropDownList>
                            </div>
                            <div class="etiqueta">
                                <label>Ordenado Por:</label>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenadoActividad" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvActividadesOrdenTrabajo" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplnkExportarActividad" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkExportarActividad" runat="server" OnClick="lnkExportarActividad_Click" Text="Exportar Excel" TabIndex="33"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lnkExportarActividad" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="renglon3x"></div>
                        <div class="grid_seccion_completa_media_altura">
                            <asp:UpdatePanel ID="upgvActividadesOrdenTrabajo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvActividadesOrdenTrabajo" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="True"
                                        OnPageIndexChanging="gvActividadesOrdenTrabajo_PageIndexChanging" OnSorting="gvActividadesOrdenTrabajo_Sorting"
                                        CssClass="gridview" AutoGenerateColumns="false" Width="97%" PageSize="10" TabIndex="34">
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                        <Columns>
                                            <asp:BoundField DataField="IdActividad" HeaderText="IdActividad" SortExpression="IdActividad" Visible="false" />
                                            <asp:BoundField DataField="Falla" HeaderText="Falla" SortExpression="Falla" />
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                            <asp:BoundField DataField="Actividad" HeaderText="Actividad" SortExpression="Actividad" />
                                            <asp:BoundField DataField="FechaFalla" HeaderText="Fecha Falla" SortExpression="FechaFalla" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <asp:BoundField DataField="FechaInicio" HeaderText="Inicio Actividad" SortExpression="FechaInicio" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <asp:BoundField DataField="FechaFin" HeaderText="Fin Actividad" SortExpression="FechaFin" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                            <asp:BoundField DataField="DuracionReal" HeaderText="Duración Real" SortExpression="DuracionReal" />
                                            <asp:BoundField DataField="Duracion" HeaderText="Duración" SortExpression="Duracion" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkEliminarActividad" Text="Eliminar" OnClick="lnkEliminarActividad_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoActividad" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarImagen" />
                                    <asp:AsyncPostBackTrigger ControlID="btnEliminarActividadFalla" />
                                    <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                    <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <!--Fin Grid View Actividades Ligadas-->
                    </asp:View>
                    <asp:View ID="vwManoObra" runat="server">
                        <div class="header_seccion">
                            <h2>Mano de Obra</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamanoManoObra">Mostrar</label>
                            </div>
                            <div class="control">
                                <label for="ddlTamanoManoObra"></label>
                                <asp:DropDownList ID="ddlTamanoManoObra" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoManoObra_SelectedIndexChanged" TabIndex="32">
                                </asp:DropDownList>
                            </div>
                            <div class="etiqueta">
                                <label>Ordenado Por:</label>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplblOrdenarManoObra" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenarManoObra" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvManoObra" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplnkExportarManoObra" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkExportarManoObra" runat="server" OnClick="lnkExportarManoObra_Click" Text="Exportar Excel" TabIndex="33"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lnkExportarManoObra" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="renglon3x"></div>
                        <div class="grid_seccion_completa_media_altura">
                            <asp:UpdatePanel ID="upgvManoObra" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvManoObra" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="True"
                                        OnPageIndexChanging="gvManoObra_PageIndexChanging" OnSorting="gvManoObra_Sorting" FooterStyle-HorizontalAlign="Right"
                                        CssClass="gridview" AutoGenerateColumns="false" Width="97%" PageSize="10" TabIndex="34">
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                        <Columns>
                                            <asp:BoundField DataField="Actividad" HeaderText="Actividad" SortExpression="Actividad" />
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                            <asp:BoundField DataField="Responsable" HeaderText="Responsable" SortExpression="Responsable" />
                                            <asp:TemplateField HeaderText="Duración" SortExpression="Duracion">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDuracion" runat="server" Text='<%# TSDK.Base.Cadena.ConvierteMinutosACadena(Convert.ToInt32(TSDK.Base.Cadena.VerificaCadenaVacia(Eval("Duracion").ToString(), "0"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CostoXHora" HeaderText="Costo x Hora" SortExpression="CostoXHora" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoManoObra" />

                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                    <asp:View ID="vwRefaccionesConsumidas" runat="server">
                        <div class="header_seccion">
                            <h2>Refacciones Consumidas</h2>
                        </div>
                        <div class="renglon3x">
                            <div class="etiqueta_50px">
                                <label for="ddlTamanoRefaccionesConsumidas">Mostrar</label>
                            </div>
                            <div class="control">
                                <label for="ddlTamanoRefaccionesConsumidas"></label>
                                <asp:DropDownList ID="ddlTamanoRefaccionesConsumidas" runat="server" CssClass="dropdown" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoRefaccionesConsumidas_SelectedIndexChanged" TabIndex="32">
                                </asp:DropDownList>
                            </div>
                            <div class="etiqueta">
                                <label>Ordenado Por:</label>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplblOrdenarRefaccionesConsumidas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblOrdenarRefaccionesConsumidas" runat="server"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvRefaccionesConsumidas" EventName="Sorting" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="etiqueta">
                                <asp:UpdatePanel ID="uplnkExportarRefaccionesConsumidas" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkExportarRefaccionesConsumidas" runat="server" OnClick="lnkExportarRefaccionesConsumidas_Click" Text="Exportar Excel" TabIndex="33"></asp:LinkButton>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lnkExportarRefaccionesConsumidas" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="renglon3x"></div>
                        <div class="grid_seccion_completa_media_altura">
                            <asp:UpdatePanel ID="upgvRefaccionesConsumidas" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvRefaccionesConsumidas" runat="server" AllowPaging="true" AllowSorting="true" ShowFooter="True"
                                        OnPageIndexChanging="gvRefaccionesConsumidas_PageIndexChanging" OnSorting="gvRefaccionesConsumidas_Sorting"
                                        CssClass="gridview" AutoGenerateColumns="false" Width="97%" PageSize="10" TabIndex="34">
                                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                        <FooterStyle CssClass="gridviewfooter" />
                                        <HeaderStyle CssClass="gridviewheader" />
                                        <RowStyle CssClass="gridviewrow" />
                                        <SelectedRowStyle CssClass="gridviewrowselected" />
                                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                        <Columns>
                                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                            <asp:BoundField DataField="SKU" HeaderText="SKU" SortExpression="SKU" />
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" SortExpression="Producto" />
                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                                            <asp:BoundField DataField="CantidadAbastecida" HeaderText="Cantidad Abastecida" SortExpression="CantidadAbastecida" />
                                            <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad Medida" SortExpression="UnidadMedida" />
                                            <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" DataFormatString="{0:C2}" />
                                            <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" />
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoRefaccionesConsumidas" />

                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </asp:View>
                </asp:MultiView>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPestanaManoObra" />
                <asp:AsyncPostBackTrigger ControlID="btnPestnaActividades" />
                <asp:AsyncPostBackTrigger ControlID="btnRefaccionesConsumidas" />
                <asp:AsyncPostBackTrigger ControlID="btnPestanaFallas" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <!--Inicio de ventana modal-->
    <div id="contenidoActividadesFallasOT" class="modal">
        <div id="actividadesFallasOT" class="contenedor_ventana_confirmacion">
            <div class="header_seccion">
                <img src="../Image/Exclamacion.png" />
                <h2>La Falla tiene ligadas actividades ¿Desea eliminar ambos? </h2>
            </div>

            <div class="renglon2x">
                <div class="etiqueta"></div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnEliminarActividadFalla" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnEliminarActividadFalla" Text="Aceptar" CssClass="boton" OnClick="btnEliminarActividadFalla_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel runat="server" ID="upbtnCancelarActividadFalla" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnCancelarActividadFalla" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarActividadFalla_Click" />
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!--Ventana modal Actividad-->
    <div id="contenedorVentanaActividades" class="modal">
        <div id="ventanaActividades" class="contenedor_modal_seccion_completa_arriba" style="width: 1130px">
            <div style="float: right">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarImagen" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upucActividadOrdenTrabajo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucActividadOrdenTrabajo ID="ucActividadOrdenTrabajo" runat="server" OnClickRegistrar="ucActividadOrdenTrabajo_ClickRegistrar"
                        TabIndex="30" Contenedor="#ventanaActividades"></tectos:wucActividadOrdenTrabajo>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                    <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                    <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardarFalla" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelarFalla" />
                    <asp:AsyncPostBackTrigger ControlID="gvFallas" />
                    <asp:AsyncPostBackTrigger ControlID="lkbAgregarActividad" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Ventana modal Fecha Termino -->
    <div id="contenedorVentanaFechaTermino" class="modal">
        <div id="ventanaFechaTermino" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" CommandName="TerminoOrden">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Ingrese la Fecha de Termino</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label>Fecha</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnTerminar" />
                                <asp:AsyncPostBackTrigger ControlID="lkbTerminar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnTerminar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnTerminar" runat="server" Text="Terminar" CssClass="boton" OnClick="btnTerminar_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana modal de Facturas de Proveedor -->
    <div id="contenedorVentanaFacturaProveedor" class="modal">
        <div id="ventanaFacturaProveedor" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarFacturaProveedor" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarFacturaProveedor" runat="server" OnClick="lnkCerrar_Click" CommandName="FacturaProveedor">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Ingrese la Factura de Proveedor</h2>
            </div>
            <div class="columna2x">
                <div id="contenedorXML">Arrastre y suelte sus archivos XML desde su carpeta a este cuadro.</div>
                <div class="renglon2x">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAgregarFactura" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAgregarFactura_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="ddlTamanoFL">Mostrar:</label>
                    </div>
                    <div class="control_100px">
                        <asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoFL" CssClass="dropdown_100px" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label>Ordenado:</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50pxr">
                        <asp:UpdatePanel ID="uplnkExportarFacturasP" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarFacturasP" runat="server" CommandName="FacturasProveedor" OnClick="lnkExportar_Click">Exportar</asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarFacturasP" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvFacturasLigadas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true"
                                CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
                                TabIndex="37" AutoGenerateColumns="false" Width="90%" PageSize="25">
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
                                    <asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                                    <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ImpTrasladado" HeaderText="Importe Trasladado" SortExpression="ImpTrasladado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ImpRetenido" HeaderText="Importe Retenido" SortExpression="ImpRetenido" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
                            <asp:AsyncPostBackTrigger ControlID="lkbFacturaProveedor" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL HISTORIAL LECTURAS -->
    <div id="modalLecturaHistorial" class="modal">
        <div id="lecturaHistorial" class="contenedor_modal_seccion_completa_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarHistorialLectura" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarLecHistorial" runat="server" CommandName="LecturaHistorial" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucLecturaHistorial" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucLecturaHistorial ID="wucLecturaHistorial" OnlkbConsultar="wucLecturaHistorial_lkbConsultar" OnbtnNuevaLectura="wucLecturaHistorial_btnNuevaLectura" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lkbLectura" />
                    <asp:AsyncPostBackTrigger ControlID="wucLectura" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <!-- VENTANA MODAL LECTURAS -->
    <div id="modalLectura" class="modal">
        <div id="Lectura" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplkbCerrarLectura" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarLectura" runat="server" CommandName="Lectura" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel ID="upwucLectura" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <tectos:wucLectura ID="wucLectura" runat="server" OnClickEliminarLectura="wucLectura_ClickEliminarLectura" OnClickGuardarLectura="wucLectura_ClickGuardarLectura" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="wucLecturaHistorial" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
