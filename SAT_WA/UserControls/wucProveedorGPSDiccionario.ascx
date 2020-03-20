<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProveedorGPSDiccionario.ascx.cs" Inherits="SAT.UserControls.wucProveedorGPSDiccionario" %>
<!-- Estilos documentación de servicio -->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario --> 
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!--Biblioteca para fijar encabeados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
<!-- Validación de datos de este formulario -->
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryProveedorGPSDiccionario();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryProveedorGPSDiccionario() {
// *** Visualización de ventana de servicios maestros  *** //
$(document).ready(function () {

//Añadiendo Validación al Evento Click del Boton
    $("#<%=btnGuardar.ClientID%>").click(function () {
var isValid1= !$("#<%=txtAlias.ClientID%>").validationEngine('validate');
var isValid2 = !$("#<%=txtValor.ClientID%>").validationEngine('validate');
var isValid3; 

//Validando el Control
if ($("#<%=txtSerie.ClientID%>").is(':enabled') == true) {
//Validando Controles

isValid3 = !$("#<%=txtSerie.ClientID%>").validationEngine('validate');
}
else {
//Asignando Valor Positivo

isValid3 = true;
}                
//Devolviendo Resultados Obtenidos
return isValid1 && isValid2 && isValid3 ;
    });

     
 //Funcion que carga los autocompletas y asigna placeholder y habilita los controles
                var seleccionTipoEmpleado = function () {
                    var personal = $("#<%=ddlTipoDato.ClientID%>").val();
                    if (personal === '1') {
                          

                    }
                    else if (personal === '2') {
    
                    }
                    else if (personal === '3') {
                        
                    }

    };

     
  
    });


    $(document).keyup(function (e) {
    var Tipo = $("#<%=ddlTipoDato.ClientID%>").val();
        if (Tipo == 1) { // escape key maps to keycode `27`
            //Ocultando Menu
            Tinyint();
        }
        else if (Tipo == 2) {
            Int();
        }
         else if (Tipo == 3) {
            Varchar();
        }
         else if (Tipo == 4) {
            
        }
    });


}

//Invocando Función de COnfiguración
    ConfiguraJQueryProveedorGPSDiccionario();
            //Declarando Función de Validación de Fechas
        function Tinyint() {
            //Obteniendo Valores
           $("#<%=txtValor.ClientID%>").on('input', function () { if (this.value.length > 3) this.value = this.value.slice(0, 3); }).on('keydown', function (e) {

            // Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
            if ($.inArray(e.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
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

            // Ensure that it is a number and stop the keypress
            if ((e.value.length > 1)) {
                this.value = this.value.slice(0, 1);
            }
        })                   
    }

              //Declarando Función de Validación de Fechas
        function Int() {
            //Obteniendo Valores
           $("#<%=txtValor.ClientID%>").on('input', function () { if (this.value.length > 16) this.value = this.value.slice(0, 16); }).on('keydown', function (e) {

            // Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
            if ($.inArray(e.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
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

            // Ensure that it is a number and stop the keypress
            if ((e.value.length > 1)) {
                this.value = this.value.slice(0, 1);
            }
        })                   
    }

        function Varchar() {
            //Obteniendo Valores
           $("#<%=txtValor.ClientID%>").on('input', function () { if (this.value.length > 500) this.value = this.value.slice(0, 500); }).on('keydown', function (e) {

            // Permite: backspace(46), delete(8), tab(9), escape(27), enter(13) and(110) .(190)
            if ($.inArray(e.keyCode, [8, 9, 27, 13, 190]) !== -1 ||
                // Permite Funciones: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Permite Teclas de Movimiento: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }

            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 90)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }

            // Ensure that it is a number and stop the keypress
            if ((e.value.length > 1)) {
                this.value = this.value.slice(0, 1);
            }
        })                   
        }
</script>
<%--<div class="seccion_controles" style="width:800px; height:450px">--%>
<div class="header_seccion">
<img src="../Image/dictionary.png" />
<h2>Proveedor GPS Diccionario</h2>
</div>    
<div class="columna4x">

<div class="renglon2x">

<div class="etiqueta_155px">
<asp:Label ID="lblIdEntidad" runat="server"></asp:Label>
</div>
<div class="etiqueta_200px">
<asp:Label ID="lblNombre" runat="server" CssClass="label_negrita"></asp:Label>
</div>


<div class="etiqueta_50px">
<asp:Label ID="lblIdRegistro" runat="server" CssClass="label_negrita" Visible="false"></asp:Label>
</div>

</div>

<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtProveedor">Proveedor GPS</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlProveedor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlProveedor" runat="server" CssClass="dropdown_200px" AutoPostBack="true" TabIndex="1">
<%--OnSelectedIndexChanged="ddlProveedor_SelectedIndexChanged"--%>
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>



<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtTipoDato">Tipo de dato</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoDato" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoDato" runat="server" CssClass="dropdown_200px" OnSelectedIndexChanged="ddlTipoDato_SelectedIndexChanged" AutoPostBack="true" TabIndex="2" >
<%--OnSelectedIndexChanged="ddlTipoDato_SelectedIndexChanged"--%>
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
    <asp:AsyncPostBackTrigger ControlID="ddlTipoDato" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

        
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtValor">Valor Sistema</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtValor" runat="server" TabIndex="3" CssClass="textbox_200px validate[required]" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoDato" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtAlias">Alias</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtAlias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtAlias" runat="server" CssClass="textbox_200px validate[required]" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtSerie">Serie</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox_200px validate[required]" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>




<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton"
OnClick="btnGuardar_Click"  TabIndex="6"/>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelar_Click" TabIndex="7" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<%--<div class="renglon2x">
    </div>--%>
<div class="header_seccion">
                    <h2>Unidad</h2>
                </div>
<div class="renglon3x">
<div class="etiqueta_50px">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" AutoPostBack="true" TabIndex="8"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenado" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportar" runat="server" TabIndex="9" 
OnClick="lkbExportar_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>



<div class="grid_seccion_completa_200px_altura" style="height:120px">
<asp:UpdatePanel ID="upgvProveedorGPS" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvProveedorGPS" runat="server"  AutoGenerateColumns="False"
ShowFooter="True" CssClass="gridview"  Width="100%" OnPageIndexChanging="gvProveedorGPS_PageIndexChanging" OnSorting="gvProveedorGPS_Sorting" AllowPaging="True" AllowSorting="True" TabIndex="10" >
<Columns>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:BoundField DataField="IdProveedor" HeaderText="Proveedor" SortExpression="IdProveedor" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
<asp:BoundField DataField="Alias" HeaderText="Alias" SortExpression="Alias" />
<asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />

<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" ItemStyle-Width="50px">
<ItemTemplate>
<asp:ImageButton runat="server" ID="imbEditar" ImageUrl="~/Image/EditarDoc.png" CommandName="Editar" Width="32" Height="32" ToolTip="Editar" OnClick="imbProveedorWSDiccionario_Click" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnGuardar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvProveedorGPS" />
</Triggers>
</asp:UpdatePanel> 
</div>
</div>
<%--</div>--%>