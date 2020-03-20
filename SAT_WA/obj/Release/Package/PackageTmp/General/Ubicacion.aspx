<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="Ubicacion.aspx.cs" Inherits="SAT.General.Ubicacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!-- Estilos documentación de servicio -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?v=3.exp&key=AIzaSyBH8LH8WQEO7Y9BqT62mCWWV0WPYKQnaCY&libraries=drawing"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryUbicacion();
InicializaMapa();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryUbicacion() {
$(document).ready(function () {

//Validación Ubicación
var validacionUbicacion = function () {
var isValidP1 = !$("#<%=txtDescripcion.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtDireccion.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtCiudad.ClientID%>").validationEngine('validate');
var isValidP4 = !$("#<%=txtCodigoPostal.ClientID%>").validationEngine('validate');
var isValidP5 = !$("#<%=txtTelefono.ClientID%>").validationEngine('validate');
var result = isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5;

//Si no hay errores de validación, se procede a validar y almacenar en sesion los puntos de la figura trazada
if (result)
guardaPuntos();

//Devolviendo resultado
return result;
};
//Validación de campos requeridos
$("#<%=this.btnAceptar.ClientID%>").click(validacionUbicacion);
//Menú Guardar
$("#<%= lkbGuardar.ClientID %>").click(validacionUbicacion);
//Cargando Catalogo AutoCompleta 
    $("#<%=txtCiudad.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=19'
    });


//Validando Guardado de Patio
$("#<%=btnGuardarUbicacionPatio.ClientID%>").click(function () {

//Validando Tiempo
var isValid1 = !$("#<%=txtTiempoPatio.ClientID%>").validationEngine('validate');
return isValid1;
});


});

//Cargando Controles de Fecha
$("#<%=txtFechaCarga.ClientID%>").datetimepicker({
    lang: 'es',
    format: 'd/m/Y H:i'
});

//Función de Cambio
    $("#<%=txtLitros.ClientID%>").change(function () {

        //Obteniendo Valores
        var litros_carga = $("#<%=txtLitros.ClientID%>").val();
        var sobrante_anterior = $("#<%=txtSobranteAnterior.ClientID%>").val();
        var sobrante_actual = 0.00;

        //Validando que existan los litros
        if (litros_carga == "")
            //Asignando '0's a la Variable
            litros_carga = "0";

        //Validando que exista el Sobrante Anterior
        if (sobrante_anterior == "")
            //Asignando '0's a la Variable
            sobrante_anterior = "0";

        //Calculando Total
        sobrante_actual = parseFloat(litros_carga) + parseFloat(sobrante_anterior);

        //Asignando Valores
        $("#<%=txtLitros.ClientID%>").val(litros_carga);
        $("#<%=txtSobranteAnterior.ClientID%>").val(sobrante_anterior);
        $("#<%=txtSobranteActual.ClientID%>").val(sobrante_actual);

        //Obteniendo la Ubicación Actual
        obtieneUbicacion();
    });

    //Función de Obtención de Ubicación
    $("#<%=lkbLocalizarMapa.ClientID%>").click(function () {
        obtieneUbicacion();
    });
    
}

//Invocación Inicial de método de configuración JQuery
    ConfiguraJQueryUbicacion();

    /** MÉTODOS DE POSICIONAMIENTO **/
    //Función que obtiene la Posición Actual
    function obtieneUbicacion() {
        if (navigator.geolocation)
            navigator.geolocation.getCurrentPosition(guardaUbicacionServidor, muestraError)
        else
            alert("La Geolocalización no esta soportada por este Navegador");
    };
    function guardaUbicacionServidor(position) {
        //Definiendo Datos de Envio
        var point = "{ 'latitud' : '" + position.coords.latitude + "', 'longitud' : '" + position.coords.longitude + "' }";

        //Consumiendo Datos desde el Lado del Servidor
        $ajax({
            type: "POST",
            url: "Ubicacion.aspx/ObtienePosicionActual",
            data: point,
            contentType: 'application/json',
            success: function (response) {
                
            },
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });


        alert("Mi posicion es: " + lat + ", " + long);
    };
    //Función que muestra los Errores
    function muestraError(error) {
        if (error.code == 1) {
            alert("User denied the request for Geolocation.");
        }
        else if (err.code == 2) {
            alert("Location information is unavailable.");
        }
        else if (err.code == 3) {
            alert("The request to get user location timed out.");
        }
        else {
            alert("An unknown error occurred.");
        }
    };
</script>
<div id="encabezado_forma">
<img src="../Image/Paradas.png" />
<h1>Ubicación</h1>
</div>
<nav id="menuForma">
    <ul>
        <li class="green"><a href="#" class="fa fa-floppy-o"></a>
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
                <li>
<asp:UpdatePanel ID="uplkbImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbImprimir" runat="server" Text="Imprimir" OnClick="lkbElementoMenu_Click" CommandName="Imprimir" />
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
<li>
<asp:UpdatePanel ID="uplnkAgregarCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkAgregarCarga" runat="server" Text="Agregar Carga Diesel" OnClick="lkbElementoMenu_Click" CommandName="AgregarCarga" />
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
    <asp:PostBackTrigger ControlID="lkbBitacora" />
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
    <asp:PostBackTrigger ControlID="lkbReferencias" />
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
    <asp:PostBackTrigger ControlID="lkbArchivos" />
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
<li>
<asp:UpdatePanel ID="uplkbControlPatios" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbControlPatios" runat="server" Text="Control Patios" OnClick="lkbElementoMenu_Click" CommandName="ControlPatios" />
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
</ul>
</li>
</ul>
</nav>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Direccion.png" />
<h2>Datos de la Ubicación</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDescripcion">Descripción</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDescripcion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox2x validate[required]" TabIndex="1" MaxLength="100"></asp:TextBox>
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
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoUbicacion">Tipo Ubicación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="upddlTipoUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoUbicacion" runat="server" CssClass="dropdown2x" TabIndex="2"></asp:DropDownList>
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
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDireccion">Dirección</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtDireccion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtDireccion" runat="server" CssClass="textbox2x validate[required]" TabIndex="3" MaxLength="150"></asp:TextBox>
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
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCiudad">Ciudad</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCiudad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCiudad" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="4" ></asp:TextBox>
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
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtCodigoPostal">Código Postal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCodigoPostal" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textbox2x validate[required]" TabIndex="5" MaxLength="10"></asp:TextBox>
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
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtTelefono">Teléfono</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtTelefono" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtTelefono" runat="server" CssClass="textbox2x validate[required]" TabIndex="6" MaxLength="15"></asp:TextBox>
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
</div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="txtGeoUbicacion">Geoubicación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtGeoUbiicacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtGeoUbicacion" runat="server" CssClass="textbox2x" TabIndex="7" Enabled="False"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbLimpiarMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplkbLimpiarMapa" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbLimpiarMapa" runat="server" ToolTip="Limpiar Mapa" OnClick="lkbLimpiarMapa_Click" TabIndex="9">
<img src="../Image/borrar.png" alt="Limpiar Mapa" height="25" width="25" /> Limpiar
</asp:LinkButton>
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
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplkbLocalizarMapa" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbLocalizarMapa" runat="server" ToolTip="Localizar en Mapa" OnClick="lkbLocalizarMapa_Click" TabIndex="10">
<img src="../Image/paradas.png" alt="Localizar en Mapa" height="25" width="25" /> Buscar
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>               
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelar_Click" TabIndex="11" />
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
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="boton" TabIndex="12"
CausesValidation="true" OnClick="btnAceptar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon">
<div class="etiqueta">                    
</div>
<div class="control">
<asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
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
</div>
</div>
<div class="renglon3x">
<div class="etiqueta"></div>                
<asp:UpdatePanel ID="uplblbError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="lkbLocalizarMapa" />
</Triggers>
</asp:UpdatePanel>                
</div>
           
</div>
</div> 
<div class="contenedor_seccion_completa">
<div class="mapa" id="mapa"></div>
</div>

<!-- Ventana de Carga Autotanque -->
<div id="contenedorVentanaCargaAutotanque" class="modal">
<div id="ventanaCargaAutotanque" class="contenedor_modal_seccion_completa_arriba">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" CommandName="Pagos" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Carga Autotanque</h2>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlEstatus">Estatus</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" Enabled="false"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaCarga">Fecha de Carga</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaCarga" runat="server" CssClass="textbox validate[custom[dateTime24]]" MaxLength="16"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtLitros">Litros de Carga</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtLitros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtLitros" runat="server" CssClass="textbox_100px validate[custom[positiveNumber]]"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtSobranteAnterior">Sobrante Anterior</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtSobranteAnterior" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSobranteAnterior" runat="server" CssClass="textbox_100px validate[custom[positiveNumber]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta_155px">
<label for="txtSobranteActual">Sobrante Actual</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtSobranteActual" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSobranteActual" runat="server" CssClass="textbox_100px validate[custom[positiveNumber]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="txtLitros" />
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_boton">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarCarga" runat="server" Text="Guardar" CssClass="boton"
OnClick="btnGuardarCarga_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarCarga" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarCarga" runat="server" Text="Cancelar" CssClass="boton_cancelar"
OnClick="btnCancelarCarga_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoCargas">Mostrar</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoCargas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCargas" runat="server" TabIndex="22" OnSelectedIndexChanged="ddlTamanoCargas_SelectedIndexChanged"
CssClass="dropdown_100px" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoCargas">Ordenado</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenadoCargas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoCargas" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCargasAnteriores" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarCargas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarCargas" runat="server" Text="Exportar" CommandName="Cargas" OnClick="lnkExportarCargas_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarCargas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvCargasAnteriores" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCargasAnteriores" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="24" OnSorting="gvCargasAnteriores_Sorting"
OnSelectedIndexChanging="gvCargasAnteriores_SelectedIndexChanging" AutoGenerateColumns="false" Width="100%">
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
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaCarga" HeaderText="Fecha Carga" SortExpression="FechaCarga" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
<asp:BoundField DataField="SobranteAnterior" HeaderText="Sobrante Anterior" SortExpression="SobranteAnterior" />
<asp:BoundField DataField="SobranteActual" HeaderText="Sobrante Actual" SortExpression="SobranteActual" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarCarga" runat="server" Text="Eliminar"
OnClick="lnkEliminarCarga_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCargas" />
<asp:AsyncPostBackTrigger ControlID="lnkAgregarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnCancelarCarga" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarCarga" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>

<!-- Ventana de Control de Patios -->
<div id="contenedorVentanaPatios" class="modal">
    <div id="ventanaControlPatios" class="contenedor_ventana_confirmacion_arriba" style="min-width:484px; padding-bottom:25px;">
        <div class="columna2x">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplkbCerrarModal" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbCerrarModal" runat="server" OnClick="lkbCerrarModal_Click">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/ControlAcceso.png" />
                <h2>Control de Patios</h2>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="lblUbicacion">Ubicación</label>
                </div>
                <div class="etiqueta_200px">
                    <asp:UpdatePanel ID="uplblUbicacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblUbicacion" runat="server" CssClass="label_negrita"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUbicacionPatio" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtNombrePatio">Patio</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNombrePatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNombrePatio" runat="server" CssClass="textbox2x" TabIndex="25"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUbicacionPatio" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="etiqueta">
                    <label for="txtNombrePatio">Tipo</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTipoPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoPatio" runat="server" CssClass="dropdown_100px" TabIndex="26"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUbicacionPatio" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <label for="txtTiempoPatio">Tiempo</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtTiempoPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTiempoPatio" runat="server" CssClass="textbox_100px validate[required, custom[integer]]" TabIndex="27"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUbicacionPatio" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x" style="float:left;">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnGuardarUbicacionPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnGuardarUbicacionPatio" runat="server" CssClass="boton"
                                OnClick="btnGuardarUbicacionPatio_Click" TabIndex="28" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnCancelarUbicacionPatio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnCancelarUbicacionPatio" runat="server" CssClass="boton_cancelar"
                                OnClick="btnCancelarUbicacionPatio_Click" TabIndex="29" Text="Cancelar" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lkbControlPatios" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarUbicacionPatio" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</div>

</asp:Content>
