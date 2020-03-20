<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLectura.ascx.cs" Inherits="SAT.UserControls.Lectura" %>
<!-- hoja de estilo que da formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
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
ConfiguraJQueryLectura();
}
}
//Declara la función que valida los controles de la pagina
function ConfiguraJQueryLectura() {
$(document).ready(function () {
//Creación  y asignación de la funcion a la variable ValidaLectura
var validaLectura = function (evt) {
//Creación de las variables y asignacion de los controles de la pagina Lectura
    var isValid1 = !$("#<%=txtFecha.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
    var isValid3 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
    var isValid4 = !$("#<%=txtLitrosLectura.ClientID%>").validationEngine('validate');
    
    //Validando el Control
    if ($("#<%=rbKmsLec.ClientID%>").is(':checked') == true) {
        
        //Remiviendo Clases
        $("#<%=txtKmsLectura.ClientID%>").removeClass();
        $("#<%=txtHrsLectura.ClientID%>").removeClass();

        //Asignando Clase y Validadores [Campo Requerido, Número Positivo]
        $("#<%=txtKmsLectura.ClientID%>").addClass('textbox validate[required, custom[positiveNumber]]');
        //Asignando Clase y Validadores [Número Entero]
        $("#<%=txtHrsLectura.ClientID%>").addClass('textbox validate[custom[integer]]');
    }
    else if ($("#<%=rbHrsLec.ClientID%>").is(':checked') == true) {
        
        //Remiviendo Clases
        $("#<%=txtKmsLectura.ClientID%>").removeClass();
        $("#<%=txtHrsLectura.ClientID%>").removeClass();

        //Asignando Clase y Validadores [Número Positivo]
        $("#<%=txtKmsLectura.ClientID%>").addClass('textbox validate[custom[positiveNumber]]');
        //Asignando Clase y Validadores [Campo Requerido, Número Entero]
        $("#<%=txtHrsLectura.ClientID%>").addClass('textbox validate[required, custom[integer]]');
    }

    var isValid5 = !$("#<%=txtKmsLectura.ClientID%>").validationEngine('validate');
    var isValid6 = !$("#<%=txtHrsLectura.ClientID%>").validationEngine('validate');
    var isValid7 = !$("#<%=txtKmsSistema.ClientID%>").validationEngine('validate');


//Devuelve un valor a la funcion
return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7;
};

//Permite que los eventos de guardar activen la funcion de validación de controles.
$("#<%=btnGuardar.ClientID%>").click(validaLectura);

//Realiza el autoComplete del operador
$("#<%=txtOperador.ClientID%>").autocomplete({
source: '../WebHandlers/AutoCompleta.ashx?id=25&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
appendTo: "<%=this.Contenedor%>"
});

    //Eventos de Carga
    $("#<%=txtLitrosLectura.ClientID%>").change(function () {
        //Invocando Función de Calculo
        calculaRendimiento();
    });
    $("#<%=txtKmsLectura.ClientID%>").change(function () {
        //Invocando Función de Calculo
        calculaRendimiento();
    });
    $("#<%=txtHrsLectura.ClientID%>").change(function () {
        //Invocando Función de Calculo
        calculaRendimiento();
    });

    //Declarando Función de Calculo de Rendimiento
    function calculaRendimiento() {

        //Obteniendo Valores
        var litros = $("#<%=txtLitrosLectura.ClientID%>").val();
        var kms = $("#<%=txtKmsLectura.ClientID%>").val();
        var hrs = $("#<%=txtHrsLectura.ClientID%>").val();
        var rendimiento = 0;

        //Validando que existan los Litros
        if (litros == "")    
            //Asignando '0's a la Variable
            litros = "0";
        
        //Validando que exista el Kilometraje
        if (kms == "") 
            //Asignando '0's a la Variable
            kms = "0";
        //Validando que existan las Horas
        if (hrs == "")
            //Asignando '0's a la Variable
            hrs = "0";
        

        //Si no hay Litros
        if (litros == "0") 
            //Asignando Rendimiento
            rendimiento = 0.00;
        //Si es Rendimiento de Kilometros
        else if ($("#<%=rbKmsLec.ClientID%>").is(':checked') == true) {
            //Calculando Rendimiento
            rendimiento = parseFloat(kms) / parseFloat(litros);
        }
        else if ($("#<%=rbHrsLec.ClientID%>").is(':checked') == true) {
            //Calculando Rendimiento
            rendimiento = parseFloat(hrs) / parseFloat(litros);
        }

        //Asignando Valores
        $("#<%=txtRendimiento.ClientID%>").val(rendimiento);
        $("#<%=txtLitrosLectura.ClientID%>").val(litros);
        $("#<%=txtKmsLectura.ClientID%>").val(kms);
        $("#<%=txtHrsLectura.ClientID%>").val(hrs);        
    }



});
$(document).ready(function () {
$("#<%=txtFecha.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
});
}
ConfiguraJQueryLectura();
</script>
<!--Fin Script-->
<div class ="seccion_controles">
<div class="header_seccion">
<img src="../Image/Calendario.png" />
<h2>Registro Lectura</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecha">Fecha Lectura:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtFecha" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtFecha" CssClass="textbox2x validate[required,custom[dateTime24]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtUnidad">Unidad:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtUnidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtUnidad" CssClass="textbox2x validate[required ,custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtOperador">Operador:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtOperador" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtOperador" CssClass="textbox2x validate[required ,custom[IdCatalogo]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtLitrosLectura">Litros Lectura:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtLitrosLectura" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtLitrosLectura" CssClass="textbox validate[required,custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbHrsLec" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbHrsLec" runat="server" GroupName="General" Text="Hrs. Lectura" 
Checked="true" OnCheckedChanged="rbGestionaLectura_CheckedChanged" AutoPostBack="true" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbKmsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uprbKmsLec" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:RadioButton ID="rbKmsLec" runat="server" GroupName="General" Text="Kms. Lectura"
OnCheckedChanged="rbGestionaLectura_CheckedChanged" AutoPostBack="true"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbHrsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtKmsLectura">Kms. Lectura</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtKmsLectura" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtKmsLectura" CssClass="textbox_100px validate[required,custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbHrsLec" />
<asp:AsyncPostBackTrigger ControlID="rbKmsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Rendimiento</label>
</div>
<div class="control_60px">
<asp:UpdatePanel ID="uplblRendimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtRendimiento" runat="server" CssClass="textbox_50px" Enabled="false" Text="0.00" style="text-align:right"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbHrsLec" />
<asp:AsyncPostBackTrigger ControlID="rbKmsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplblUnidadRendimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblUnidadRendimiento" runat="server" CssClass="label_negrita" Text="Km/lt"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbHrsLec" />
<asp:AsyncPostBackTrigger ControlID="rbKmsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtHrsLectura">Horas Lectura:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtHrsLectura" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtHrsLectura" CssClass="textbox_100px validate[custom[integer]]" MaxLength="12"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="rbHrsLec" />
<asp:AsyncPostBackTrigger ControlID="rbKmsLec" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtKmsSistema">Kms. Sistema:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel runat="server" ID="uptxtKmsSistema" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtKmsSistema" CssClass="textbox_100px validate[custom[positiveNumber]]" MaxLength="18"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtIdentificador">Identificador:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtIdentificador" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtIdentificador" CssClass="textbox2x validate[required]" MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtReferencia">Referencia:</label>
</div>
<div class="control2x">
<asp:UpdatePanel runat="server" ID="uptxtReferencias" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox runat="server" ID="txtReferencia" CssClass="textbox2x "  MaxLength="150"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardar" runat="server" Text="Guardar"   OnClick="btnGuardar_Click"  CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar"   OnClick="btnCancelar_Click"  CssClass="boton_cancelar" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="btnEliminar_Click"  CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
