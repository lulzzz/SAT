<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="RecepcionPaquetes.aspx.cs" Inherits="SAT.ControlEvidencia.RecepcionPaquetes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/Controles.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" type="text/css" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<link href="../CSS/jquery.jqzoom.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
//Obteniendo instancia actual de la página y añadiendo manejador de evento
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryRecepcionPaquete();
}
}
//Creando función para configuración de jquery en formulario
function ConfiguraJQueryRecepcionPaquete() {
$(document).ready(function () {
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnAclarar.ClientID%>").unbind("click");
$("#<%= btnAclarar.ClientID%>").click(function () {
//Mostrando ventana modal 
$("#contenidoConfirmacionAclarar").animate({ width: "toggle" });
$("#confirmacionAclarar").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnRecibir.ClientID%>").unbind("click");
$("#<%= btnRecibir.ClientID%>").click(function () {
//Mostrando ventana modal 
$("#contenidoConfirmacionRecibir").animate({ width: "toggle" });
$("#confirmacionRecibir").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarRecibir.ClientID%>").unbind("click");
$("#<%= btnCancelarRecibir.ClientID%>").click(function (evt) {
evt.preventDefault();
//Ocultando ventana modal 
$("#contenidoConfirmacionRecibir").animate({ width: "toggle" });
$("#confirmacionRecibir").animate({ width: "toggle" });
});
//Quitando cualquier manejador de evento click añadido previamente
$("#<%= btnCancelarAclarar.ClientID%>").unbind("click");
$("#<%= btnCancelarAclarar.ClientID%>").click(function (evt) {
evt.preventDefault();
//Ocultando ventana modal 
$("#contenidoConfirmacionAclarar").animate({ width: "toggle" });
$("#confirmacionAclarar").animate({ width: "toggle" });
});
});

    <%--//Añadiendo Encabezado Fijo
    $("#<%=gvPaquetes.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorPaquete").offsetWidth - 15,
        height: 400
        
    });

    //Añadiendo Encabezado Fijo
    $("#<%=gvDocumentos.ClientID%>").gridviewScroll({
        width: document.getElementById("contenedorDocumentos").offsetWidth - 15,
        height: 400
        
    });--%>

}
//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryRecepcionPaquete();
</script>
    
<div id="encabezado_forma">
<img src="../Image/EnvioEvidencia.png" />
<h1>Recepción de envios</h1>
</div>
<div class="datos_paquete_recepcion">
<div class="header_vista_armado">
<img src="../Image/Buscar.png" />
<h2>Buscar paquete de evidencias</h2>
</div>
<div class="columna2x">            
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlOrigen">Terminal Origen</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlOrigen" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlOrigen" runat="server" TabIndex="2" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlDestino">Destino</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlDestino" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlDestino" runat="server" TabIndex="2" CssClass="dropdown2x">
</asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">                        
</div>
<div class="control">
<asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x" Enabled="false" Visible="false"></asp:TextBox>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnBuscar" runat="server" TabIndex="4" CssClass="boton"
OnClick="btnBuscar_OnClick" Text="Buscar" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnLimpiar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnLimpiar" runat="server" TabIndex="3" CssClass="boton"
Text="Limpiar" OnClick="btnLimpiar_Click" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>       
</div>
<div class="armado_paquete">
<div class="documentos_recibir"> 
<div class="header_vista_armado">
<img src="../Image/PaqueteAgregar.png" />
<h2>Paquetes por recibir</h2>
</div>
<div class="renglon2x">
<div class="etiqueta"> 
<label for="ddlTamanoPaquetes">
Mostrar:
</label>
</div>
<div class="control">                    
<asp:DropDownList ID="ddlTamanoPaquetes" runat="server" AutoPostBack="true" CssClass="dropdown" TabIndex="5"
OnSelectedIndexChanged="ddlTamanoPaquetes_OnSelectedIndexChanged">
</asp:DropDownList>
</div>               
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarPaquetes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarPaquetes">Ordenado Por:</label>
<asp:Label ID="lblOrdenarPaquetes" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPaquetes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto">
<asp:LinkButton ID="lkbExportarPaquete" runat="server" Text="Exportar" TabIndex="7"
OnClick="lkbExportarPaquete_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grids_armado_envio" id="contenedorPaquete">
<asp:UpdatePanel ID="upgvPaquetes" runat="server" UpdateMode="Conditional">
<ContentTemplate>                        
<asp:GridView ID="gvPaquetes" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
OnPageIndexChanging="gvPaquetes_PageIndexChanging" TabIndex="8" ShowFooter="True" CssClass="gridview" OnSorting="gvPaquetes_Sorting" PageSize="25" Width="100%">
<HeaderStyle CssClass="EncabezadoGridViewCSS" />
<RowStyle CssClass="FilaGridViewCSS" />
<Columns>
<asp:TemplateField SortExpression="Id">
<ItemTemplate>
<asp:LinkButton ID="lkbId" runat="server" Text='<%#Eval("Id") %>' OnClick="OnClick_lkbId"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
<asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
<asp:BoundField DataField="MedioEnvio" HeaderText="Medio Envio" SortExpression="MedioEnvio" />
<asp:TemplateField HeaderText="Documentos"  SortExpression="NoDocumentos">
<ItemTemplate>
<asp:LinkButton ID="lkbDocumentos" runat="server" OnClick="lkbVer_OnClick" TabIndex="9"
Text='<%#Eval("NoDocumentos") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Tiempo" HeaderText="Tiempo" SortExpression="Tiempo" />
<asp:TemplateField HeaderText="HERRAMIENTAS">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitácoraP" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitácoraP" runat="server" OnClick="lkbBitacoraP_OnClick" TabIndex="10"
Text="Bitácora"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitácoraP" />
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
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
<asp:AsyncPostBackTrigger ControlID="btnAclarar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarRecibir" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPaquetes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="documentos_recibir">
<div class="header_vista_armado">
<img src="../Image/Documento.png" />
<h2>Documentos por recibir</h2>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoDocumentos">Mostrar</label>
</div>
<div class="control">                   
<asp:DropDownList ID="ddlTamanoDocumentos" runat="server" AutoPostBack="true" CssClass="dropdown" TabIndex="15"
OnSelectedIndexChanged="ddlTamanoDocumentos_OnSelectedIndexChanged">
</asp:DropDownList>
</div>               
<div class="control">
<asp:UpdatePanel ID="uplblOrdenarDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<label for="lblOrdenarDocumentos">Ordenar:</label>
<asp:Label ID="lblOrdenarDocumentos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDocumentos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta" style="width:auto">                    
<asp:LinkButton ID="lkbExportarDocumentos" runat="server" Text="Exportar" TabIndex="17"
OnClick="lkbExportarDocumentos_OnClick"></asp:LinkButton>
</div>
</div>
<div class="grids_armado_envio" id="contenedorDocumentos">
<asp:UpdatePanel ID="upgvDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>                        
<asp:GridView ID="gvDocumentos" runat="server" AllowPaging="True" PageSize="25"
AllowSorting="True" AutoGenerateColumns="False"
OnPageIndexChanging="gvDocumentos_PageIndexChanging" TabIndex="18"
ShowFooter="True" CssClass="gridview"
OnSorting="gvDocumentos_Sorting" Width="100%">
<Columns>
<asp:TemplateField SortExpression="Id">
<HeaderTemplate>
<asp:CheckBox ID="chkTodosDocumentos" runat="server" AutoPostBack="true" Text="TODOS"
OnCheckedChanged="chkTodosDocumentos_CheckedChanged" />
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkVariosDocumentos" runat="server" Text='<%# Eval("Id") %>' AutoPostBack="True"
OnCheckedChanged="chkDetallesDocumentos_CheckedChanged" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Viaje" SortExpression="Viaje">
<ItemTemplate>
<asp:Label ID="Viaje" runat="server"
Text='<%# Eval("Viaje")%>'></asp:Label>
</ItemTemplate>
<FooterTemplate>
<asp:Label ID="lblContadorDetalles" runat="server" Text="0 ">
</asp:Label>
Seleccionado(s)
</FooterTemplate>
<FooterStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
<asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
<asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
<asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha" SortExpression="FechaRecepcion" DataFormatString="{0:dd/MM/yyyy hh:mm:ss}" />
<asp:TemplateField HeaderText="HERRAMIENTAS">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbBitacora_OnClick" TabIndex="19"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbBitacora" />
</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="uplkbReferencias" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbReferencias_OnClick" TabIndex="19"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbReferencias" />
</Triggers>
</asp:UpdatePanel>
</ItemTemplate>
<FooterStyle HorizontalAlign="Center" />
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
<asp:AsyncPostBackTrigger ControlID="gvPaquetes" />
<asp:AsyncPostBackTrigger ControlID="btnBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarRecibir" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAclarar" />
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDocumentos" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPaquetes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x" style="height:auto">
<asp:UpdatePanel ID="upPanelResultados" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblError" runat="server" CssClass="label_error" Text="" TabIndex="20"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="btnAceptarRecibir" />
<asp:AsyncPostBackTrigger ControlID="btnLimpiar" />
<asp:AsyncPostBackTrigger ControlID="btnAceptarAclarar" />
<asp:AsyncPostBackTrigger ControlID="gvDocumentos" />
<asp:AsyncPostBackTrigger ControlID="gvPaquetes" />
<asp:AsyncPostBackTrigger ControlID="ddlTamanoPaquetes" />
</Triggers>
</asp:UpdatePanel>
</div>            
</div>
</div>
<div class="columna_botones">
<div class="renglon_botones"></div>
<div class="renglon_botones"></div>
<div class="renglon_botones"></div>
<div class="renglon_botones"></div>
<div class="renglon_botones">
<asp:UpdatePanel ID="upbtnRecibir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnRecibir" runat="server" CssClass="boton" Text="Recibir" TabIndex="22" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
<div class="renglon_botones">
<asp:UpdatePanel ID="upbtnAclarar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAclarar" runat="server" CssClass="boton" Text="Aclaración" TabIndex="21" />
</ContentTemplate>
<Triggers>
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
<div class="imagenes_documentos">
<div class="header_imagenes_documentos">
<img src="../Image/Imagenes_docs.png" />
<h3>Documentos digitalizados</h3>
</div>
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
</asp:HyperLink>
</center>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPaquetes" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenDocumentos" runat="server" UpdateMode="Conditional">
<ContentTemplate>                        
<asp:DataList ID="dtlImagenDocumentos" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id") + " " + Eval("Documento")%>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:DataList>                        
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvPaquetes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
       
<div id="contenidoConfirmacionRecibir" class="modal">
<div id="confirmacionRecibir" class="contenedor_ventana_confirmacion">
<div class="header_resumen_documentos_segmento">
<img src="../Image/Exclamacion.png" />                 
<h3>Recibir documentos</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Está seguro de recibir los documentos?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnCancelarRecibir" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarRecibir" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarRecibir" runat="server" OnClick="btnRecibir_OnClick" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<div id="contenidoConfirmacionAclarar" class="modal">
<div id="confirmacionAclarar" class="contenedor_ventana_confirmacion">            
<div class="header_resumen_documentos_segmento">
<img src="../Image/Exclamacion.png" />                 
<h3>Aclarar documentos</h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Está seguro de realizar la aclaración del documento?</label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="controlBoton">
<asp:Button ID="btnCancelarAclarar" runat="server" CssClass="boton_cancelar" Text="Cancelar" />
</div>
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnAceptarAclarar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnAceptarAclarar" runat="server" OnClick="btnAclarar_OnClick" CssClass="boton" Text="Aceptar" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>               
</div>
</div>
</asp:Content>
