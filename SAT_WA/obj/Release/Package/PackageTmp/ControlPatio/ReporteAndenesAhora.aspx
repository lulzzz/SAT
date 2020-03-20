<%@ Page Title="Reporte Actual de Andenes" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteAndenesAhora.aspx.cs" Inherits="SAT.ControlPatio.ReporteAndenes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../CSS/ControlPatio.css" rel="stylesheet" />
<link href="../CSS/ControlEvidencias.css" rel="stylesheet" />
<script src="../Scripts/jquery.jqzoom-core.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
<div id="encabezado_forma">
<img src="../Image/Indicadores.png" />
<h1>Andenes Actual</h1>
</div>
<div class="contenedor_controles">
<div class="columna2x">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlPatio">Patio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlPatio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlPatio" runat="server" TabIndex="1" CssClass="dropdown" 
OnSelectedIndexChanged="ddlPatio_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="control">
<asp:UpdatePanel ID="uplnkBuscar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton runat="server" ID="lnkBuscar" OnClick="lnkBuscar_Click">
<img src="../Image/Reload.png" />
</asp:LinkButton>                            
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
<section class="fila_indicador">
<asp:LinkButton runat="server" ID="lnkDisponible" CssClass="indicadorL"  CommandName="AndenDisponible" OnClick="lnkIndicadores_Click">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblAnden" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblAnden"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />                         
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                                                                    
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/AndenLogistico.png" />
</div>
<div class="leyenda_indicador">
Andenes Disponibles
</div>           
</asp:LinkButton>
<asp:LinkButton runat="server" ID="lnkCargando" CssClass="indicador"  CommandName="AndenCargando" OnClick="lnkIndicadores_Click">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblCargando" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblCargando"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />                        
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                        
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorCajaCarga.png" />
</div>
<div class="leyenda_indicador">
Unidades Cargando
</div>           
</asp:LinkButton> 
<asp:LinkButton runat="server" ID="lnkDescargando" CssClass="indicadorL"  CommandName="AndenDescargando" OnClick="lnkIndicadores_Click">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="upplblDescargando" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblDescargando"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger EventName="SelectedIndexChanged" ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />                        
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                                                                
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorCajaDescarga.png" />
</div>
<div class="leyenda_indicador">
Unidades Descargando
</div>           
</asp:LinkButton>
       
<asp:LinkButton runat="server" ID="lnkTiempoPromedio" CssClass="indicador_texto"  CommandName="TiempoPromedio" OnClick="lnkIndicadores_Click">
<div class="texto_indicador">
<asp:UpdatePanel runat="server" ID="uplblTiempoPromedio" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempoPromedio"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                         
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/IndicadorTiempo.png" />
</div>
<div class="leyenda_indicador">
Tiempo Promedio por Operación
</div>           
</asp:LinkButton>
<asp:LinkButton runat="server" ID="lnkUtilizacion" CssClass="indicadorL"  CommandName="Utilizacion" OnClick="lnkIndicadores_Click">
<div class="numero_indicador">
<asp:UpdatePanel runat="server" ID="uplblUtilizacion" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUtilizacion"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />                                       
</Triggers>
</asp:UpdatePanel>
</div>
<div class="imagen_indicador">
<img src="../Image/Porcentaje.png" />
</div>
<div class="leyenda_indicador">
% Utilizacion
</div>           
</asp:LinkButton>
        
                         
</section>
<section class="contenedor_graficas">
<div class="grafica_pay">
<div class="header_grafica">
<h3>
<asp:UpdatePanel runat="server" ID="uplblNombrePay" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNombrePay" Text="Unidades por Estatus"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />    
</Triggers>
</asp:UpdatePanel>
</h3>                
</div>
<asp:UpdatePanel ID="upchtEntidades" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtEntidades" runat="server" BackColor="Transparent">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />     
</Triggers>
</asp:UpdatePanel>
<div class="grafica_pay_tabla">
<asp:UpdatePanel ID="upgvPay" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvPay" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview" ShowHeader="false">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:BoundField DataField="Rango" HeaderText="Descripcion" SortExpression="Rango" />
<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" /> 
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grafica_poligonal">
<div class="header_grafica">
<h3>
<asp:UpdatePanel runat="server" ID="uplblNombreLinea" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNombreLinea" Text="Eventos Andenes por Hora"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                       
</Triggers>
</asp:UpdatePanel>
</h3>
</div>
<asp:UpdatePanel ID="upchtLineaTiempo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Chart ID="chtLineaTiempo" runat="server" BackColor="Transparent" Width="950px">                        
<Legends>
<asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom"></asp:Legend>
</Legends>
</asp:Chart>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                        
</Triggers>
</asp:UpdatePanel>
<div class="grafica_poligonal_tabla">
<asp:UpdatePanel ID="upgvLineaTiempo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvLineaTiempo" runat="server" AutoGenerateColumns="true" AllowPaging="false" AllowSorting="false"
PageSize="25" Width="100%" CssClass="gridview">
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
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="lnkDisponible" />
<asp:AsyncPostBackTrigger ControlID="lnkDescargando" />
<asp:AsyncPostBackTrigger ControlID="lnkCargando" />
<asp:AsyncPostBackTrigger ControlID="lnkTiempoPromedio" />
<asp:AsyncPostBackTrigger ControlID="lnkUtilizacion" />                        
</Triggers>
</asp:UpdatePanel>
</div>             
                        
</div>  
</section>
<section class="contenedor_seccion_completa">
<div class="header_seccion">
<img src="../Image/TablaResultado.png" />
<h2>Estatus Andenes</h2>
<div class="controlr">
<asp:UpdatePanel ID="uplnkMapa" runat="server" UpdateMode="Conditional">
<ContentTemplate>                                           
<asp:LinkButton runat="server" ID="lnkMapa" Text="Mostrar Mapa" OnClick="lnkMapa_Click">
<img src="../Image/ImagenPatio.png" />
Mostrar Patio
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>     
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamano">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAndenes" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" 
OnClick="lnkExportar_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportar" />
</Triggers>
</asp:UpdatePanel>
</div>           
                         
</div>
<div class="grid_seccion_completa">
<asp:UpdatePanel ID="upgvAndenes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvAndenes" runat="server" AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
OnSorting="gvAndenes_Sorting" OnPageIndexChanging="gvAndenes_PageIndexChanging"
OnRowDataBound="gvAndenes_RowDataBound"
PageSize="25" Width="100%" CssClass="gridview">
<AlternatingRowStyle CssClass="gridviewrowalternate" />
<EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
<FooterStyle CssClass="gridviewfooter" />
<HeaderStyle CssClass="gridviewheader" />
<RowStyle CssClass="gridviewrow" />
<SelectedRowStyle CssClass="gridviewrowselected" />
<SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
<SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
<Columns>
<asp:TemplateField HeaderText="Estatus">
<ItemTemplate>
<asp:Image ID="imgEstatus" runat="server" Height="20px" ImageAlign="AbsMiddle" Width="20px"
ImageUrl="~/Image/EntidadTiempoOK.png" />
</ItemTemplate>
<ItemStyle HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
<asp:TemplateField HeaderText="Anden" SortExpression="Entidad">
<ItemTemplate>
<asp:LinkButton ID="lnkBitacora" runat="server" Text='<%# Eval("Entidad") %>' OnClick="lnkBitacora_Click">
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Estatus" HeaderText="Estatus en Patio" SortExpression="Estatus" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Duracion" HeaderText="Duración" SortExpression="Duracion" />
<asp:BoundField DataField="Indicador" HeaderText="Indicador" SortExpression="Indicador" Visible="false" />
<asp:BoundField DataField="Utilizacion" HeaderText="% Utilización" SortExpression="Utilizacion" />
<asp:BoundField DataField="TiempoPromedio" HeaderText="Tiempo Promedio" SortExpression="TiempoPromedio" />
<asp:BoundField DataField="CantidadEventos" HeaderText="Eventos" SortExpression="CantidadEventos" />
<asp:TemplateField>
<ItemStyle HorizontalAlign="Center" />
<ItemTemplate>
<asp:LinkButton ID="lnkEvidencias" runat="server" OnClick="ibEvidencias_Click">
<img src="../Image/ImagenEvidencia.png" width="20" height="20" />
</asp:LinkButton>
</ItemTemplate>                                
</asp:TemplateField>
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
<asp:AsyncPostBackTrigger ControlID="ddlTamano" />
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
<asp:AsyncPostBackTrigger ControlID="lnkCerrarMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</section>
<div id="contenidoBitacoraUnidades" class="modal">
<div id="bitacoraUnidades" class="contenedor_ventana_confirmacion">
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />
<h2>Bitacora<br />
Eventos de la Unidad
</h2>
</div>
<div class="renglon2x">
<div class="etiqueta_50px">
<label for="ddlTamanoBit">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoBit" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlTamanoBit_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<label for="lblOrdenadoBit">Ordenado:</label>
</div>
<div class="control_100px">
<asp:UpdatePanel ID="uplblOrdenadoBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<b><asp:Label ID="lblOrdenadoBit" runat="server"></asp:Label></b>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvBitacora" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta_50px">
<asp:UpdatePanel ID="uplnkExportarBit" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkExportarBit" runat="server" Text="Exportar" 
OnClick="lnkExportarBit_Click"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lnkExportarBit" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_unidad_agrupada">                
<asp:UpdatePanel ID="upgvBitacora" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvBitacora" runat="server" CssClass="gridview" 
OnSorting="gvBitacora_Sorting" OnPageIndexChanging="gvBitacora_PageIndexChanging"
AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
ShowFooter="true" PageSize="25" Width="100%">
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
<asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="FecIni" HeaderText="Fecha de Inicio" SortExpression="FecIni" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
<asp:BoundField DataField="FecFin" HeaderText="Fecha de Termino" SortExpression="FecFin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
</Columns>
</asp:GridView>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlTamanoBit" />
<asp:AsyncPostBackTrigger ControlID="gvAndenes" />
<asp:AsyncPostBackTrigger ControlID="btnCerrar" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon_boton_salir">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnCerrar" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CssClass="boton_cancelar"
OnClick="btnCerrar_Click" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvAndenes" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
</div>
</div>
</div>
<div id="mapa_patio" class="modal_mapa_patio">
<div id="visualizacion_mapa_patio" class="contenedor_mapa_patio">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrar" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrar" runat="server" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="informacion_mapa">                
<div class="header_seccion">
<img src="../Image/ImagenPatio.png" />
<h2>Mapa Patio</h2>
</div>
<div class="etiqueta">
<label for="">Zona de Patio</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlZonaPatio" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlZonaPatio" runat="server" CssClass="dropdown" AutoPostBack="true"
OnSelectedIndexChanged="ddlZonaPatio_SelectedIndexChanged"></asp:DropDownList>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlPatio" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="header_info_mapa">                    
<h3>Indicadores Patio</h3>
</div>                
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa2" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa2"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Cargando</div>
</div>
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa3" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa3"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Descargando</div>
</div>  
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa4" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa4"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Cargadas x Descarga</div>
</div> 
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa5" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa5"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Cargadas x Salir</div>
</div> 
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa6" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa6"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Vacias x Cargar</div>
</div>  
<div class="renglon_info_mapa_total">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa7" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa7"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Vacias x Salir</div>
</div>
<div class="renglon_info_mapa">
<div class="indicador_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblInfoMapa1" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblInfoMapa1"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="indicador_leyenda_info_mapa">Unidades en Patio</div>
</div>
<div class="header_info_mapa">                    
<h3>Datos Entidad</h3>
</div>  
<div class="renglon_info_mapa">                    
<div class="entidad_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblEntidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblEntidad"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div> 
<div class="renglon_info_mapa">
<div class="entidad_mas_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblTiempo" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempo"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_info_mapa">
<div class="entidad_mas_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblUnidad" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUnidad"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_info_mapa">
<div class="entidad_mas_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblNoOperaciones" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblNoOperaciones"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_info_mapa">
<div class="entidad_mas_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblTiempoPromedioAnden" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblTiempoPromedioAnden"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon_info_mapa">
<div class="entidad_mas_info_mapa">
<asp:UpdatePanel runat="server" ID="uplblUtilizacionAnden" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label runat="server" ID="lblUtilizacionAnden"></asp:Label>
</ContentTemplate>
<Triggers>                                
<asp:AsyncPostBackTrigger ControlID="imgLayout" />
<asp:AsyncPostBackTrigger ControlID="lnkMapa" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
                           
</div>
<div class="contenedor_mapa">                
<asp:UpdatePanel ID="upimgLayout" runat="server" UpdateMode="Always">
<ContentTemplate>
<asp:ImageButton ID="imgLayout" runat="server" ImageAlign="Middle" OnClick="imgLayout_Click"/>                       
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="ddlZonaPatio" EventName="SelectedIndexChanged" />
<asp:AsyncPostBackTrigger ControlID="ddlPatio" EventName="SelectedIndexChanged" />                        
<asp:AsyncPostBackTrigger ControlID="lnkBuscar" />
                        
</Triggers>
</asp:UpdatePanel>                
</div>
                        
</div>
</div>

<div id="contenidoVentanaEvidencias" class="modal">
<div id="ventanaEvidencias" class="contenedor_modal_imagenes">
<div class="cerrar_mapa">
<asp:UpdatePanel runat="server" ID="uplnkCerrarImagen" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lnkCerrarMapa" runat="server" OnClick="lnkCerrarImagen_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="contenedor_imagenes_patio">
<div class="header_seccion">
<img src="../Image/Imagenes.png" />
<h2>Evidencias en Patio</h2>
</div>
<div class="visor_imagen">
<asp:UpdatePanel ID="uphplImagenZoom" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:HyperLink ID="hplImagenZoom" runat="server" CssClass="MYCLASS" NavigateUrl="~/Image/noDisponible.jpg" Height="150" Width="200">
<asp:Image ID="imgImagenZoom" runat="server" Height="150px" Width="200px" ImageUrl="~/Image/noDisponible.jpg" BorderWidth="1" BorderStyle="Dotted" BorderColor="Gray" />
</asp:HyperLink>
</ContentTemplate>                        
</asp:UpdatePanel>
</div>
<div class="imagenes">
<asp:UpdatePanel ID="updtlImagenImagenes" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DataList ID="dtlImagenImagenes" runat="server" RepeatDirection="Horizontal">
<ItemTemplate>
<asp:UpdatePanel ID="uplkbThumbnailDoc" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbThumbnailDoc" runat="server" CommandName='<%# Eval("URL") %>' OnClick="lkbThumbnailDoc_Click">
<img alt='<%# "ID: " + Eval("Id")  %>' src='<%# String.Format("../Accesorios/VisorImagenID.aspx?t_carga=archivo&t_escala=pixcel&alto=73&ancho=95&url={0}", Eval("URL")) %>' width="95" height="73" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</ItemTemplate>
</asp:DataList>
</ContentTemplate>
                        
</asp:UpdatePanel>
</div>
               
</div>
</div>
</div>
</asp:Content>
