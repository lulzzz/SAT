<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Nomina12.aspx.cs"   MasterPageFile="~/MasterPage/MasterPage.Master" Inherits="SAT.Nomina.Nomina12" %>
<%@ Register Src="~/UserControls/wucEmailCFDI.ascx" TagPrefix="uc1" TagName="wucEmailCFDI" %>
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
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<script type="text/javascript"  src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryNomina();
        }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryNomina() {
        $(document).ready(function () {

            //Cargando Catalogos Autocompleta
            $("#<%=txtSucursal.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=37&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});
    $("#<%=txtEmpleado.ClientID%>").autocomplete({
        source: '../WebHandlers/AutoCompleta.ashx?id=38&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
});

    //Cargando Controles DateTimePicker
    $("#<%=txtFechaPago.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        closeOnDateSelect: true,
        timepicker: false
    });
    $("#<%=txtFecIniPago.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        closeOnDateSelect: true,
        onSelectDate: function (ct, $i) {
            //Causando Actualización del Control
            __doPostBack('<%= txtFecIniPago.UniqueID %>', '');
},
timepicker: false
});
    $("#<%=txtFecFinPago.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        closeOnDateSelect: true,
        onSelectDate: function (ct, $i) {
            //Causando Actualización del Control
            __doPostBack('<%= txtFecFinPago.UniqueID %>', '');
},
timepicker: false
});
    $("#<%=txtFecNomina.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y',
        closeOnDateSelect: true,
        timepicker: false
    });

    //Declarando Función de Validación de Encabezado
    var validaNominaEncabezado = function () {

        //Obteniendo Validación de Controles
        var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtFecIniPago.ClientID%>").validationEngine('validate');
    var isValid3 = !$("#<%=txtFecFinPago.ClientID%>").validationEngine('validate');
    var isValid4 = !$("#<%=txtDiasPago.ClientID%>").validationEngine('validate');
    var isValid5 = !$("#<%=txtFechaPago.ClientID%>").validationEngine('validate');
    var isValid6 = !$("#<%=txtFecNomina.ClientID%>").validationEngine('validate');
    var isValid7 = !$("#<%=txtSucursal.ClientID%>").validationEngine('validate');

    //Devolviendo Resultados Obtenidos
    return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7;
}

    //Añadiendo Validación a los Eventos de Guardado
    $("#<%=lkbGuardar.ClientID%>").click(validaNominaEncabezado);
    $("#<%=btnAceptar.ClientID%>").click(validaNominaEncabezado);
    $("#<%=lkbTimbrarTodo.ClientID%>").click(validaNominaEncabezado);


    //Declarando Función de Validación de Detalle
    var validaNominaEmpleado = function () {

        //Obteniendo Validación de Controles
        var isValid1 = !$("#<%=txtEmpleado.ClientID%>").validationEngine('validate');

    //Devolviendo Resultados Obtenidos
    return isValid1;
}

    //Añadiendo Validación a los Eventos de Guardado
    $("#<%=btnAgregarEmp.ClientID%>").click(validaNominaEmpleado);


    //Declarando Función de Validación de Encabezado
    var validaDetalleNomina = function () {

        //Obteniendo Validación de Controles
        var isValid1 = !$("#<%=txtImporteGravado.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtImporteExento.ClientID%>").validationEngine('validate');

    //Devolviendo Resultados Obtenidos
    return isValid1 && isValid2;
}

    //Añadiendo Validación a los Eventos de Guardado
   // $("#<%=btnGuardarDet.ClientID%>").click(validaDetalleNomina);

    //Declarando Función de Validación de Encabezado
    var validaNominaOtros = function () {

        //Obteniendo Validación de Controles
        var isValid1 = !$("#<%=txtDias.ClientID%>").validationEngine('validate');
    var isValid2 = !$("#<%=txtImportePagado.ClientID%>").validationEngine('validate');
    var isValid3 = !$("#<%=txtCantidad.ClientID%>").validationEngine('validate');

    //Devolviendo Resultados Obtenidos
    return isValid1 && isValid2 && isValid3;
}

    //Añadiendo Validación a los Eventos de Guardado
    $("#<%=btnGuardarNO.ClientID%>").click(validaNominaOtros);

});
}

//Declarando Función de Validación de Fechas
function CompareDates() {
    //Obteniendo Valores
    var txtDate1 = $("#<%=txtFecIniPago.ClientID%>").val();
    var txtDate2 = $("#<%=txtFecFinPago.ClientID%>").val();

    //Fecha en Formato MM/DD/YYYY
    var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6));
    var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6));

    //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
    if (date1 > date2)
        //Mostrando Mensaje de Operación
        return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
}

//Invocando Método de Configuración
ConfiguraJQueryNomina();
</script>
<div id="encabezado_forma">
<img src="../Image/Tabla.png" />
<h1>Nómina 1.2</h1>
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
<asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
</ul>
</li>
<li class="red">
<a href="#" class="fa fa-pencil-square-o"></a>
<ul>
<li>
<asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
<li>
<asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
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
<li>
<asp:LinkButton ID="lkbTimbrarTodo" runat="server" Text="Timbrar Nómina" OnClick="lkbElementoMenu_Click" CommandName="TimbrarTodo" /></li>
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
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:PostBackTrigger ControlID="lkbBitacora" />
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
<div class="seccion_controles">
<div class="columna2x">
<!--No. Consecutivo-->
<div class="renglon2x">
<div class="etiqueta">
<label for="lblNoConsecutivo">No. Consecutivo</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblNoConsecutivo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblNoConsecutivo" runat="server" Text="Por Asignar"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--Version-->
<div class="renglon2x">
<div class="etiqueta">
<label for="lblVersion">Versión</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtVersion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:TextBox ID="txtVersion" CssClass="textbox" Enabled="false" runat="server" Text="1.2"></asp:TextBox></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--1. Tipo Nómina-->
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTipoNomina">Tipo Nómina</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipoNomina" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipoNomina" runat="server" AutoPostBack="true" CssClass="dropdown" OnSelectedIndexChanged="ddlTipoNomina_SelectedIndexChanged" TabIndex="1"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--. Compania-->
<div class="renglon2x">
<div class="etiqueta">
<label>Compania</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--2. Inicio Pago-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecIniPago">Inicio Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecIniPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecIniPago" runat="server" CssClass="textbox validate[required, custom[date]]" MaxLength="20"
TabIndex="2" OnTextChanged="txtFecIniPago_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--3. Fin pago-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecFinPago">Fin Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecFinPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecFinPago" runat="server" CssClass="textbox validate[required, custom[date], funcCall[CompareDates[]]]" MaxLength="20"
TabIndex="3" OnTextChanged="txtFecFinPago_TextChanged"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--4. DiasPagados-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtDiasPago">Dias Pagados</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtDiasPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDiasPago" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber3], min[0.001], max[5490.000]]" MaxLength="9" TabIndex="4"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="txtFecIniPago" />
<asp:AsyncPostBackTrigger ControlID="txtFecFinPago" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--5. Fecha Pago-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFechaPago">Fecha Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaPago" runat="server" CssClass="textbox validate[required, custom[date]]" MaxLength="20" TabIndex="5"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<!--6. Fecha Nómina-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtFecNomina">Fecha Nomina</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFecNomina" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFecNomina" runat="server" CssClass="textbox validate[required, custom[date]]" MaxLength="20" TabIndex="6"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--7. Sucursal-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSucursal">Sucursal</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtSucursal" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSucursal" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--8. Periodicidad-->
<div class="renglon2x">
<div class="etiqueta">
<label>Periodicidad</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPeriodicidadPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPeriodicidadPago" runat="server" CssClass="dropdown" TabIndex="8"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
<asp:AsyncPostBackTrigger ControlID="ddlTipoNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--9. Método de Pago-->
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlMetodoPago">Método de Pago</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown" TabIndex="9"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--. lblError-->
<!--10. Boton Aceptar-->
<!--11. Boton Cancelar-->
<div class="renglon2x">
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptar_Click" TabIndex="10" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" OnClick="btnCancelar_Click" TabIndex="11" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<div class="seccion_controles">
<div class="header_seccion">
<img src="../Image/Totales.png" />
<h2>Detalles</h2>
</div>
<!--12. Empleado-->
<!--13. Boton Agregar-->
<div class="renglon3x">
<div class="etiqueta">
<label>Empleado</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtEmpleado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtEmpleado" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="12"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarEmp" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAgregarEmp" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAgregarEmp" runat="server" CssClass="boton" Text="Agregar" OnClick="btnAgregarEmp_Click" TabIndex="12" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--14. Tamaño-->
<!--15. Exportar-->
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar:</label>
</div>
<!--14. Tamaño-->
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="14" AutoPostBack="true"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado:</label>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--15. Exportar-->
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" CommandName="NominaEmpleados" OnClick="lnkExportar_Click" TabIndex="14"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_400px_altura">
<!--16. gvNominaEmpleado-->
<asp:UpdatePanel ID="upgvNominaEmpleado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvNominaEmpleado" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
CssClass="gridview" ShowFooter="True" TabIndex="16" OnSorting="gvNominaEmpleado_Sorting"
OnPageIndexChanging="gvNominaEmpleado_PageIndexChanging" AutoGenerateColumns="False" Width="100%">
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
<asp:BoundField DataField="Empleado" HeaderText="Empleado" SortExpression="Empleado" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="SalarioDI" HeaderText="Salario Diario Integrado" SortExpression="SalarioDI" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="SalarioCA" HeaderText="Salario Cotización Aprovación" SortExpression="SalarioCA" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField HeaderText="Aguinaldo" SortExpression="Aguinaldo">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkAguinaldo" runat="server" CommandName="Aguinaldo" Text='<%# string.Format("{0:C2}", Eval("Aguinaldo")) %>' OnClick="lnkPercepcion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Sueldo" SortExpression="Sueldo">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkSueldo" runat="server" CommandName="Sueldo" Text='<%# string.Format("{0:C2}", Eval("Sueldo")) %>' OnClick="lnkPercepcion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Otras Percepciones" SortExpression="OtrasPercepciones">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkOtrosPercepciones" runat="server" CommandName="Otros" Text='<%# string.Format("{0:C2}", Eval("OtrasPercepciones")) %>' OnClick="lnkPercepcion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="IMSS" SortExpression="IMSS">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkIMSS" runat="server" CommandName="IMSS" Text='<%# string.Format("{0:C2}", Eval("IMSS")) %>' OnClick="lnkDeduccion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="ISPT" SortExpression="ISPT">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkISPT" runat="server" CommandName="ISPT" Text='<%# string.Format("{0:C2}", Eval("ISPT")) %>' OnClick="lnkDeduccion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Infonavit" SortExpression="Infonavit">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkInfonavit" runat="server" CommandName="Infonavit" Text='<%# string.Format("{0:C2}", Eval("Infonavit")) %>' OnClick="lnkDeduccion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Otras Deducciones" SortExpression="OtrasDeducciones">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkOtrosDeducciones" runat="server" CommandName="Otros" Text='<%# string.Format("{0:C2}", Eval("OtrasDeducciones")) %>' OnClick="lnkDeduccion_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="HrsExtra" SortExpression="HrsExtra">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkHrsExtra" runat="server" Text='<%# string.Format("{0:C2}", Eval("HrsExtra")) %>' OnClick="lnkHrsExtra_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Incapacidad" SortExpression="Incapacidad">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkIncapacidad" runat="server" Text='<%# string.Format("{0:C2}", Eval("Incapacidad")) %>' OnClick="lnkIncapacidad_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Otras Pagos" SortExpression="OtrosPagos">
<ItemStyle HorizontalAlign="Right" />
<FooterStyle HorizontalAlign="Right" />
<ItemTemplate>
<asp:LinkButton ID="lnkOtrosPagos" runat="server" CommandName="Otros" Text='<%# string.Format("{0:C2}", Eval("OtrosPagos")) %>' OnClick="lnkOtrosPagos_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="TPercepciones" HeaderText="Total Percepciones" SortExpression="TPercepciones" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" Font-Bold="true" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TDeducciones" HeaderText="Total Deducciones" SortExpression="TDeducciones" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" Font-Bold="true" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:BoundField DataField="TPagado" HeaderText="Total Pagado" SortExpression="TPagado" DataFormatString="{0:C2}">
<ItemStyle HorizontalAlign="Right" Font-Bold="true" />
<FooterStyle HorizontalAlign="Right" />
</asp:BoundField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminaNE" runat="server" Text="Eliminar" CommandName="Eliminar" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkTimbraNE" runat="server" Text="Timbrar" CommandName="Timbrar" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
<asp:LinkButton ID="lnkCancelarNE" runat="server" Text="Cancelar" CommandName="Cancelar" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:UpdatePanel ID="uplnkImprimir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkPDF" runat="server" Text="PDF" CommandName="PDF" OnClick="lnkImprimirNomina_Click"></asp:LinkButton>
<asp:LinkButton ID="lnkXML" runat="server" Text="XML" CommandName="XML" OnClick="lnkImprimirNomina_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkPDF" />
<asp:PostBackTrigger ControlID="lnkXML" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkFiniquito" runat="server" Text="Imprimir Finiquito" CommandName="Finiquito" OnClick="lnkFiniquito_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEmailEmpleado" runat="server" OnClick="lnkEmailEmpleado_Click">E-mail</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="btnAgregarEmp" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbradoEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbradoNomina" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!-- Ventana de Percepciones y/ó Deducciones -->
<div id="contenedorVentanaDetallesNomina" class="modal">
<div id="ventanaDetallesNomina" class="contenedor_modal_seccion_completa_arriba">
<!--17. lnkCerrarVentanaTarifasPago-->
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarVentanaTarifasPago" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" CommandName="DetalleNomina" Text="Cerrar" TabIndex="17">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Detalle de Nomina</h2>
</div>
<div class="columna">
<!--18. ddlConcepto-->
<div class="renglon">
<div class="etiqueta">
<label for="ddlConcepto">Concepto</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown" TabIndex="18"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbradoEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--19. ImporteGrabado-->
<!--20. Importe-->
<div class="renglon">
<!--lbl Importe Gravado-->
<div class="etiqueta">
<asp:UpdatePanel ID="uplblImporteGravado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblImporteGravado" runat="server" Text="Importe Gravado"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--19. ImporteGrabado-->
<!--20. Importe-->
<div class="control_100px">
<asp:UpdatePanel ID="uptxtImporteGravado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<!--19. ImporteGrabado-->
<asp:TextBox ID="txtImporteGravado" runat="server" Text="0.00" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
MaxLength="9" TabIndex="19"></asp:TextBox>
<!--20. Importe-->
<asp:TextBox ID="txtImporte" runat="server" Text="0.00" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
MaxLength="9" TabIndex="20"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarTimbradoEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarCancelarNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--21. Importe Excento-->
<div class="renglon">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblImporteExento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblImporteExento" runat="server" Text="Importe Exento"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtImporteExento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImporteExento" runat="server" Text="0.00" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
MaxLength="9" TabIndex="21"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--22. chkSubsidioCausado-->
<!--23. txtSubsidioCausado-->
<div class="renglon">
<!--. lblSubsidioCausado-->
<div class="etiqueta">
<asp:UpdatePanel ID="uplblSubsidioCausado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblSubsidioCausado" runat="server" Text="Subsidio Causado"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--22. chkSubsidioCausado-->
<div class="etiqueta_50px">
<asp:UpdatePanel ID="upchkSubsidioCausado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkSubsidioCausado" Checked="false" AutoPostBack="true" OnCheckedChanged="chkSubsidioCausado_CheckedChanged" runat="server" TabIndex="22"/>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--23. txtSubsidioCausado-->
<div class="control_100px">
<asp:UpdatePanel ID="uptxtSubsidioCausado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSubsidioCausado" Enabled="false" runat="server" Text="0.00" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
MaxLength="9" TabIndex="23"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" />
<asp:AsyncPostBackTrigger ControlID="chkSubsidioCausado" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--24. btnGuardarDet-->
<!--25. btnCancelarDet-->
<div class="renglon">
<!--24. btnGuardarDet-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarDet" runat="server" CssClass="boton" Text="Guardar"
OnClick="btnGuardarDet_Click" TabIndex="24" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<!--25. btnCancelarDet-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarDet" runat="server" CssClass="boton_cancelar" Text="Cancelar"
OnClick="btnCancelarDet_Click" TabIndex="25" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<!--26. ddlTamañoDet-->
<!--27. lnkExportarDet-->
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoDet">Mostrar</label>
</div>
<!--26. ddlTamañoDet-->
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDet" runat="server" CssClass="dropdown_100px" TabIndex="26" OnSelectedIndexChanged="ddlTamanoDet_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoDet" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDetalleNomina" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--27. lnkExportarDet-->
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarDet" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarDet" runat="server" CommandName="DetalleNomina" Text="Exportar"
OnClick="lnkExportar_Click" TabIndex="27"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<!--28. gvDetalleNomina-->
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvDetalleNomina" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDetalleNomina" runat="server" AllowPaging="true" AllowSorting="true" OnRowDataBound="gvDetalleNomina_RowDataBound"
CssClass="gridview" ShowFooter="true" TabIndex="28" OnSorting="gvDetalleNomina_Sorting" PageSize="25"
OnPageIndexChanging="gvDetalleNomina_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField SortExpression="Id">
<ItemTemplate>
<asp:Label ID="lblId" runat="server" Text='<%#  Eval("Id") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
<asp:TemplateField SortExpression="ImporteGravado">
<ItemTemplate>
<asp:Label ID="lblImporteGravado" runat="server" ToolTip="Importe Gravado" Text='<%# string.Format("{0:C2}", Eval("ImporteGravado")) %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="ImporteExento">
<ItemTemplate>
<asp:Label ID="lblImporteExento" runat="server" ToolTip="Importe Exento" Text='<%# string.Format("{0:C2}", Eval("ImporteExento")) %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="Importe">
<ItemTemplate>
<asp:Label ID="lblImporte" runat="server" ToolTip="Importe" Text='<%# string.Format("{0:C2}", Eval("Importe")) %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField SortExpression="SubsidioCausado" HeaderText="Subsidio C.">
<ItemTemplate>
<asp:Label ID="lblSubsidioCausado" runat="server" ToolTip="Importe" Text='<%# string.Format("{0:C2}", Eval("SubsidioCausado")) %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
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
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDet" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarDet" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana de Nomina Otros -->
<div id="contenedorVentanaNominaOtros" class="modal">
<div id="ventanaNominaOtros" class="contenedor_modal_seccion_completa_arriba">
<!--29. lnkCerrarNominaOtros-->
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarNominaOtros" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarNominaOtros" runat="server" OnClick="lnkCerrar_Click" CommandName="NominaOtros" Text="Cerrar" TabIndex="29">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Nomina Otros</h2>
</div>
<div class="columna">
<!--30. ddlTipo-->
<div class="renglon">
<div class="etiqueta">
<label for="ddlTipo">Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" runat="server" CssClass="dropdown" TabIndex="30" Enabled="false" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--31. ddlTipo-->
<div class="renglon">
<div class="etiqueta">
<label for="txtDias">Dias</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtDias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtDias" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" MaxLength="9" TabIndex="31"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--32. ddlSubTipo-->
<div class="renglon">
<div class="etiqueta">
<label for="ddlSubTipo">Sub Tipo</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlSubTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlSubTipo" runat="server" CssClass="dropdown" TabIndex="32"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--33. txtImportePagado-->
<div class="renglon">
<div class="etiqueta">
<label for="txtImportePagado">Importe Pagado.</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtImportePagado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtImportePagado" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" MaxLength="10" TabIndex="33"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--34. txtCantidad-->
<div class="renglon">
<div class="etiqueta">
<label for="txtCantidad">Cantidad</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uptxtCantidad" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCantidad" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]" MaxLength="10" TabIndex="34"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" />
<asp:AsyncPostBackTrigger ControlID="ddlTipo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--35. btnGuardarNO-->
<!--36. btnCancelarNO-->
<div class="renglon">
<!--35. btnGuardarNO-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnGuardarNO" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnGuardarNO" runat="server" CssClass="boton" Text="Guardar" TabIndex="35" OnClick="btnGuardarNO_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
<!--36. btnCancelarNO-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelarNO" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelarNO" runat="server" CssClass="boton_cancelar" Text="Cancelar" TabIndex="36" OnClick="btnCancelarNO_Click" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="columna2x">
<!--37. ddlTamañoNO-->
<!--38. lnkExportarNO-->
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoNO">Mostrar</label>
</div>
<!--37. ddlTamañoNO-->
<div class="control_100px">
<asp:UpdatePanel ID="upddlTamanoNO" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoNO" runat="server" CssClass="dropdown_100px" TabIndex="37" OnSelectedIndexChanged="ddlTamanoNO_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label>Ordenado</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoNO" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b>
<asp:Label ID="lblOrdenadoNO" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaOtros" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--38. lnkExportarNO-->
<div class="etiqueta_50pxr">
<asp:UpdatePanel ID="uplnkExportarNO" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarNO" runat="server" CommandName="NominaOtros" Text="Exportar" OnClick="lnkExportar_Click" TabIndex="38"></asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<!--39. gvNominaOtros-->
<div class="grid_seccion_completa_150px_altura">
<asp:UpdatePanel ID="upgvNominaOtros" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvNominaOtros" runat="server" AllowPaging="true" AllowSorting="true"
CssClass="gridview" ShowFooter="true" TabIndex="39" OnSorting="gvNominaOtros_Sorting" PageSize="25"
OnPageIndexChanging="gvNominaOtros_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
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
<asp:BoundField DataField="Dias" HeaderText="Dias" SortExpression="Dias" />
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="HorasExtras" HeaderText="Cantidad" SortExpression="HorasExtras" />
<asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" />
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEditarNO" runat="server" Text="Editar" OnClick="lnkEditarNO_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton ID="lnkEliminarNO" runat="server" Text="Eliminar" OnClick="lnkEliminarNO_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
<asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
<asp:AsyncPostBackTrigger ControlID="lkbEditar" />
<asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoNO" />
<asp:AsyncPostBackTrigger ControlID="btnGuardarNO" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<!-- Ventana Timbrado Nomina -->
<div id="contenedorVentanaTimbradoNomina" class="modal">
<div id="ventanaTimbradoNomina" class="contenedor_ventana_confirmacion_arriba">
<!--40. lnkCerrarTimbrado-->
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarTimbrado" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarTimbrado" runat="server" OnClick="lnkCerrar_Click" CommandName="TimbradoNomina" Text="Cerrar" TabIndex="40">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<h2>Timbrado de Nomina</h2>
</div>
<div class="columna2x">
<!--41. txtSerie-->
<div class="renglon2x">
<div class="etiqueta">
<label for="txtSerie">Serie</label>
</div>
<!--41. txtSerie-->
<div class="control2x">
<asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="41"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<!--42. btnAceptarTimbradoEmpleado-->
<!--43. btnAceptarTimbradoEmpleado-->
<div class="renglon2x">
<!--42. btnAceptarTimbradoEmpleado-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbradoEmpleado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbradoEmpleado" CommandName="TimbrarEmpleado" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarTimbrado_Click" TabIndex="42" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
</Triggers>
</asp:UpdatePanel>
</div>
<!--43. btnAceptarTimbradoEmpleado-->
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarTimbradoNomina" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarTimbradoNomina" CommandName="TimbrarNomina" runat="server" CssClass="boton" Text="Aceptar" OnClick="btnAceptarTimbrado_Click" TabIndex="43" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
<asp:AsyncPostBackTrigger ControlID="lkbTimbrarTodo" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- Ventana Cancelado Nomina -->
<div id="contenedorVentanaCanceladoNomina" class="modal">
<div id="ventanaCanceladoNomina" class="contenedor_ventana_confirmacion_arriba">
<!--44. lnkCerrarCancelarNomina-->
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplnkCerrarCancelarNomina" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarCancelarNomina" runat="server" OnClick="lnkCerrar_Click" CommandName="CanceladoNomina" Text="Cerrar" TabIndex="44">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>¿Esta seguro que desea Cancelar la Nómina?</h2>
</div>
<div class="columna2x">
<!--45. btnAceptarCancelarNomina-->
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarCancelarNomina" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarCancelarNomina" runat="server" CssClass="boton" Text="Aceptar"
OnClick="btnAceptarCancelarNomina_Click" TabIndex="45" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<!-- Ventana Confirmación Email -->
<div id="contenidoConfirmacionEmail" class="modal">
<div id="confirmacionEmail" class="contenedor_ventana_confirmacion_arriba">
<asp:UpdatePanel ID="upwucEnvioEmail" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<uc1:wucEmailCFDI runat="server" ID="wucEmailCFDI" OnBtnEnviarEmail_Click="BtnEnviarEmail_Click" OnLkbCerrarEmail_Click="LkbCerrarEmail_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvNominaEmpleado" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>

