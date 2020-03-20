<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucBitacoraMonitoreo.ascx.cs" Inherits="SAT.UserControls.wucBitacoraMonitoreo" %>
<!-- hoja de estilo que da formato al control de usuario-->
<link href="../CSS/Forma.css" rel="stylesheet" />
<link href="../CSS/Controles.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!--Invoca al estilo encargado de dar formato a las cajas de texto que almacenen datos datatime -->
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<!--Invoca a los script que que validan los datos de Fecha-->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
<!--Invoca a los scripts que crean el zoom de la imagen -->
<script type="text/javascript" src="../Scripts/jquery.elevatezoom.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.elevateZoom-3.0.8.min.js" charset="utf-8"></script>
<script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryBitacoraMonitoreo();
}
}
//Creando función para configuración de jquery en control de usuario
function ConfiguraJQueryBitacoraMonitoreo() {
$(document).ready(function () {
//Validación Ubicación
var validacionBitacoraMonitoreo = function () {
var isValidP1 = !$("#<%=txtFechaBitacora.ClientID%>").validationEngine('validate');
var isValidP2 = !$("#<%=txtComentario.ClientID%>").validationEngine('validate');
var isValidP3 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
//Devolviendo resultado
return isValidP1 && isValidP2 && isValidP3;
};
$("#<%=txtFechaBitacora.ClientID%>").datetimepicker({
lang: 'es',
format: 'd/m/Y H:i'
});
//Validación de campos requeridos
    $("#<%=this.btnRegistrar.ClientID%>").click(validacionBitacoraMonitoreo);

    //Script de Elevación de Imagen
    $(".myImageElevate").elevateZoom({
        tint: true,
        tintColour: '#7CBC7E',
        tintOpacity: 0.5
    });
});
}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryBitacoraMonitoreo();
</script>
<div style="width:1100px">
<div id="capturaIncidencia" style="width:500px; float:left; margin-left:5px;">
<div class="header_seccion">
<img src="../Image/Calendario.png" />
<h2>Registro Bitácora Monitoreo</h2>
</div>
<div class="renglon" style="float:left;">
<div class="etiqueta">
<label for="ddlTipo">Tipo Bitácora</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTipo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTipo" CssClass="dropdown" runat="server"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<asp:UpdatePanel ID="uplblDescripcionValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblDescripcionValor">Valor</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>  
</div>
<div class="control">
<asp:UpdatePanel ID="uplblValor" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblValor" runat="server" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtFechaBitacora">Fecha Bitácora</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtFechaBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtFechaBitacora" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" ></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_155px">
<asp:UpdatePanel ID="uplkbArchivos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbArchivos" runat="server" Text="Agregar Evidencias" OnClick="lkbArchivos_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
<asp:PostBackTrigger ControlID="lkbArchivos" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtComentario">Comentario</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtComentario" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtComentario" runat="server" CssClass="textbox2x validate[required]" MaxLength="500" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="txtUbicacion">Ubicación</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[required]" MaxLength="200" ></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="lblServicio">Servicio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblServicio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblServicio" Text="Sin Asignar" CssClass="label" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="lblParada">Parada</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblParada" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblParada" Text="Sin Asignar" CssClass="label" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="lblEvento">Evento</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblEvento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblEvento" Text="Sin Asignar" CssClass="label" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="float:left;">
<div class="etiqueta">
<label for="lblMovimiento">Movimiento</label>
</div>
<div class="control2x">
<asp:UpdatePanel ID="uplblMovimiento" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblMovimiento" runat="server" CssClass="label"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<br />
<div class="renglon2x" style="float:left;">
<div class="control2x">
<asp:UpdatePanel ID="uptxtGeoUbiicacion" runat="server" UpdateMode="Conditional" RenderMode="Inline">
<ContentTemplate>
<asp:TextBox ID="txtGeoUbicacion" runat="server" Visible="false" CssClass="textbox validate[required]" TabIndex="7" Enabled="False"></asp:TextBox>
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<div class="renglon2x" style="float:left">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnEliminar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnEliminar" runat="server" Text="Eliminar" OnClick="OnClick_btnEliminar"  CssClass="boton" />
</ContentTemplate>
<Triggers>                        
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnRegistrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRegistrar" runat="server" Text="Registrar" OnClick="OnClick_btnRegistrar"  CssClass="boton" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x" style="width: 350px; float:left">
<asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div id="incidencias" style="width:500px; float:left; margin-left:5px;">
<div class="header_seccion">
<img src="../Image/Imagenes_docs.png" />
<h2>Imagenes de Incidencias</h2>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>                    
<asp:DataList ID="dtlImagenDocumentos" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<img class="myImageElevate" title='<%# "ID: " + Eval("Id") + " " + Eval("Tipo")%>' width="95" height="73" 
    src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>'
    data-zoom-image="<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=sin_escala&url={0}", Eval("URLZoomImage")) %>" />
</ItemTemplate>
<SelectedItemStyle BackColor="#FFFF99" />
</asp:DataList>                   
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>

<!--Ventana modal que confirma la eliminación de una bitacora de monitoreo-->
<div id="contenidoBitacoraMonitoreo" class="modal">
<div id="bitacoraMonitoreo" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h3>Confirmación Eliminar Registro</h3>
</div>
<div class="columna2x">
<div class="renglon2x">
<asp:UpdatePanel runat="server" ID="uplblMensaje" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblMensaje" CssClass="mensaje_modal">
¿Esta seguro de querer eliminar el registro?
</asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="OnClick_btnAceptar"  CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
<asp:AsyncPostBackTrigger ControlID="btnCancelar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="OnClick_btnCancelar"  CssClass="boton" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptar" />                         
<asp:AsyncPostBackTrigger ControlID="btnRegistrar" />
<asp:AsyncPostBackTrigger ControlID="btnEliminar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
</div>
