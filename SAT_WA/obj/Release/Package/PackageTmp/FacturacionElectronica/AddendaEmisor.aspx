<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master"  CodeBehind="AddendaEmisor.aspx.cs" Inherits="SAT.FacturacionElectronica.AddendaEmisor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
<link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
<style>
#box {
width:300px;
height:14px;
text-align:center;
vertical-align:middle;
border:2px solid #04B404;
background-color:#00FF00;
padding:12px;
font-family:Arial;
font-size:11px;
}
#box2 {
width:300px;
height:14px;
text-align:center;
vertical-align:middle;
border:2px solid #04B404;
background-color:#00FF00;
padding:12px;
font-family:Arial;
font-size:11px;
}
</style>
<script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryAddenda();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryAddenda() {
            $(document).ready(function () {
                //Función de validación de campos
                var validacionAddenda = function (evt) {
                    var isValidP1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
                    return isValidP1
                };
                    //Función de validación de campos
                    var validacionAddendaEmisor = function (evt) {
                        var isValidP1 = !$("#<%=txtReceptor.ClientID%>").validationEngine('validate');
                        return isValidP1 
            };
                 //Boton Guardar
                $("#<%=btnAceptar.ClientID%>").click(validacionAddenda);  
                //Boton Nuevo
                $("#<%=lkbNuevo.ClientID%>").click(validacionAddenda); 
                    //Boton Guardar
                $("#<%=btnAceptarAddendaEmisor.ClientID%>").click(validacionAddendaEmisor);  
                //Añadiendo Función de Autocompletado al Control
                $("#<%=txtReceptor.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=15&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    appendTo: "#confirmacionAddendaEmisor"
                });
                //Limpiando Contenedor XSD
                $("#<%=lkbNuevo.ClientID%>").click(function () {
                    LimpiaContenedorXSD();
                });
                //Limpiando Contenedor XSD
                $("#<%=btnCancelar.ClientID%>").click(function () {
                    LimpiaContenedorXSD();
                });
                //Limpiando Contenedor XML
                $("#<%=btnNuevaAddendaE.ClientID%>").click(function () {
                    LimpiaContenedorXML();
                });
                //Limpiando Contenedor XML
                $("#<%=lkbCerrarAddendaEmisor.ClientID%>").click(function () {
                    LimpiaContenedorXML();
                });
                //Limpiando Contenedor XML
                $("#<%=gvAddendaEmisor.ClientID%>").click(function () {
                    LimpiaContenedorXML();
                });

        });
    }
    //Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryAddenda();
    /**Script Contenedor de Archivos**/
    //Declarando variable contenedora de Archivos
    var selectedFiles;
    //Función que limpia el Contenedor
    function LimpiaContenedorXSD() {   //Limpiando DIV
        selectedFiles = null;
        $("#box").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
    }
    //Función que limpia el Contenedor
    function LimpiaContenedorXML() {   //Limpiando DIV
        selectedFiles = null;
        $("#box2").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
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
        //Declarando Objeto contenedor del DIV
        var box2;
        box2 = document.getElementById("box2");
        //Añadiendo Eventos
        box2.addEventListener("dragenter", OnDragEnter2, false);
        box2.addEventListener("dragover", OnDragOver2, false);
        box2.addEventListener("drop", OnDrop2, false);

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
    //Función cuando se Arrastra el Objeto dentro del limite
    function OnDragEnter2(e) {
        e.stopPropagation();
        e.preventDefault();
    }
    //Función cuando se Arrastra el Objeto fuera del limite
    function OnDragOver2(e) {
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
        alert('El Archivo se ha Cargado')
        //Indicando Archivo
        $("#box").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
    }
        function OnDrop2(e) {
            e.stopPropagation();
            e.preventDefault();

            selectedFiles = null;
            selectedFiles = e.dataTransfer.files;
        var lector2 = new FileReader();

        //Evento al Cargar el Archivo
        lector2.onload = function (evt) {
            //Obteniendo Archivo
            var bytes = evt.target.result;
            //Invocando Método Web para Obtención de Archivos
            PageMethods.ArchivoSesionAddendaEmisor(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
        };
        //Evento al Producirse un Error
        lector2.onerror = function (evt) {
            alert('Error Carga ' + evt.target.error);

        };
        //Leyendo Texto
        lector2.readAsText(selectedFiles[0]);
        //Mostrando mensaje
        alert('El Archivo se ha Cargado')
        //Indicando Archivo
        $("#box2").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
    }

    </script>
<div id="encabezado_forma">
<h1>Addenda Emisor</h1>
</div>
<nav id="menuForma">
<ul>
<li class="green">
<a href="#" class="fa fa-floppy-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbNuevo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
<li>
<asp:UpdatePanel ID="uplkbGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbEditar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li></li>
<li>
<asp:UpdatePanel ID="uplkbEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
</ul>
</li>
<li class="blue">
<a href="#" class="fa fa-cog"></a>
<ul>
<li>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
<li>
<asp:UpdatePanel ID="uplkbArchivos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</li>
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
<div class="header_seccion"> 
<h2>Información Addenda</h2>
</div>
<div class="seccion_controles">
<div class="columna3x">
<div class="renglon3x">
<div class="etiqueta">
<label class="Label" for="lblID">
ID</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblID" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblID"  CssClass="label">ID</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label class="Label" for="txtDescripcion">
Elemento
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlElemento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlElemento"   CssClass="dropdown" runat="server"  TabIndex="1"    
></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div  class="renglon3x">
<div class="etiqueta">
<label class="Label" for="txtDescripcion">
Descripción
</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtDescripcion" TabIndex="3"    CssClass="textbox2x validate[required]"
MaxLength="50"  ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div></div>
<div class="renglon3x">
<div class="etiqueta_320px">
<div id="box">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div></div>
</div>
<div class="renglon3x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnAceptar"  CssClass="boton" Text="Aceptar" TabIndex="5"
ValidationGroup="General" onclick="btnAceptar_Click"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button runat="server" ID="btnCancelar"   CssClass="boton_cancelar" Text="Cancelar" TabIndex="6" onclick="btnCancelar_OnClick"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorC" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblErrorAddenda"  CssClass="label_error"></asp:Label></ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">
<div  class="grid_media_seccion" style="width:450px">
<asp:UpdatePanel ID="upgvXsd" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvXsd"  runat="server"    CssClass="gridview"
AutoGenerateColumns="False" ShowFooter="True"   
>
<Columns>
<asp:BoundField HeaderText="XSD" ReadOnly="true" DataField="XSD" SortExpression="XSD"  >
    
    </asp:BoundField>
    <asp:BoundField />
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
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
<h4>
Addenda Emisor</h4>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamañoGridViewgvAddendaEmisor">Mostrar:</label>
</div>
<div class="control">
<asp:DropDownList ID="ddlTamañoGridViewgvAddendaEmisor" runat="server" CssClass="dropdown"
AutoPostBack="true" 
TabIndex="29">
</asp:DropDownList>
</div>
<div class="etiqueta">
<label for="lblCriterioGridViewSucursales">Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblCriterioGridViewAddendaEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblCriterioGridViewAddendaEmisor" runat="server" Text="" CssClass="Label"></asp:Label></ContentTemplate>
 <Triggers>
  <asp:AsyncPostBackTrigger ControlID="gvAddendaEmisor" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel runat="server" ID="uplkbExportarExcel" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarExcel" runat="server" EnableViewState="False"
CommandName="Sucursales" TabIndex="30">Exportar Excel</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvAddendaEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAddendaEmisor"  Width="100%" runat="server" AllowPaging="True" AllowSorting="True"
AutoGenerateColumns="False" ShowFooter="True"  CssClass="gridview2" OnRowDataBound="gvAddendaEmisor_RowDataBound"
OnSorting="gvAddendaEmisor_Onsorting" OnPageIndexChanging="gvAddendaEmisor_OnpageIndexChanging"   OnSelectedIndexChanged="gvAddendaEmisor_OnSelectedIndexChanged"
>
<Columns>
<asp:BoundField HeaderText="Id" ReadOnly="true" DataField="Id" SortExpression="Id" />
<asp:TemplateField HeaderText="Receptor" SortExpression="Receptor">
<ItemTemplate>
<asp:Label ID="lblReceptor" runat="server" Text='<%#Eval("Receptor") %>'></asp:Label></ItemTemplate><EditItemTemplate>
</EditItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="XML" ReadOnly="true" DataField="XMLPredeterminado" SortExpression="XMLPredeterminado" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbDeshabilitarE" runat="server"  TabIndex="13" OnClick="clickMenuFormaAddendaEmisor_Click" 
CommandName="DeshabilitarE">Deshabilitar</asp:LinkButton></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lkbEditarE" runat="server"  TabIndex="13" OnClick="clickMenuFormaAddendaEmisor_Click" 
CommandName="EditarE">Editar</asp:LinkButton></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="upAccesoriosDetalle" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacoraI" runat="server" OnClick="clickMenuFormaAddendaEmisor_Click"  TabIndex="14"
CommandName="BitacoraI">Bitácora</asp:LinkButton></ContentTemplate><Triggers>
<asp:PostBackTrigger ControlID="lkbBitacoraI" />
</Triggers>
</asp:UpdatePanel>
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
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaAddendaE" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="ddlTamañoGridViewgvAddendaEmisor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div  class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnNuevaAddendaE" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnNuevaAddendaE" runat="server"    OnClick="btnNuevaAddendaE_Click" CssClass ="boton"  Text="Añadir Addenda Emisor"  />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<asp:UpdatePanel ID="uplblErrorAdedendaEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblErrorAdedendaEmisor"  CssClass="label_error"></asp:Label></ContentTemplate><Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvAddendaEmisor" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div id="contenidoConfirmacionAddendaEmisor" class="modal">
<div id="confirmacionAddendaEmisor"" class="contenedor_ventana_confirmacion"> 
<div  style="text-align:right">
<asp:UpdatePanel runat="server" ID="uplkbCerrarAddendaEmisor"  UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarAddendaEmisor" OnClick="lkbCerrarAddendaEmisor_Click" runat="server" Text="Cerrar"   >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
<h3>AddendaEmisor</h3>
<div class="columna3x"> 
<div class="renglon3x">
<div class="etiqueta">
<label for="txtReceptor">Receptor</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtReceptor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtReceptor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="2"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaAddendaE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon3x">
<div class="etiqueta_320px">
<div id="box2">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div></div>
</div>
<br /><br />
<div class="renglon3x">
<div class="control" >
<asp:UpdatePanel ID="uplblErrorAddendaEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="Label1" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbCerrarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="gvAddendaEmisor" />
<asp:AsyncPostBackTrigger ControlID="btnNuevaAddendaE" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAddendaEmisor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAddendaEmisor" runat="server"   OnClick="btnAceptarAddendaEmisor_Click"  CssClass ="boton"  Text="Aceptar"  />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>  
</div>            
</div>
</div>
</asp:Content>
