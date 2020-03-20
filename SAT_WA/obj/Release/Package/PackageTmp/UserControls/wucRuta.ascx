<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRuta.ascx.cs" Inherits="SAT.UserControls.wucRuta" %>
<%--<div class="contenido_pestañas_documentacion" >--%>
<div class="header_seccion">
<h2>Rutas Concidentes</h2>
</div>

<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoRuta">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoRuta" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoRuta_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoRuta" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRuta" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
    </div>
<div class="renglon2x">
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportarRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarRuta" runat="server" TabIndex="5"
OnClick="lkbExportarRuta_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarRuta" />
</Triggers>
</asp:UpdatePanel>
</div>
    </div>

<div class="grid_seccion_completa_200px_altura"  >
<asp:UpdatePanel ID="upgvRuta" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvRuta" runat="server" AutoGenerateColumns="False" PageSize="25"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvRuta_PageIndexChanging" OnSorting="gvRuta_Sorting" AllowPaging="True" AllowSorting="True" >
<Columns>
<asp:BoundField DataField="Segmento" HeaderText="Segmento" SortExpression="Segmento" />
<asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
<asp:BoundField DataField="Rem1" HeaderText="Rem1" SortExpression="Rem1"  />
<asp:BoundField DataField="Rem2" HeaderText="Rem2" SortExpression="Rem2"  />
<asp:BoundField DataField="Dolly" HeaderText="Dolly" SortExpression="Dolly"  />
<asp:BoundField DataField="Ruta" HeaderText="Ruta" SortExpression="Ruta"  />
<asp:TemplateField HeaderText="" SortExpression="">
<ItemTemplate>
<asp:LinkButton ID="lkbDetalles" runat="server"  Text="Detalles"  CommandName="Detalles" OnClick="lkbAccionRuta_Click"></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Calcular" SortExpression="Calcular">
<ItemTemplate>
<asp:LinkButton ID="lkbCalcular" runat="server"  CommandName='<%# Eval("Calcular") %>'  Text='<%# Eval("Calcular") %>' OnClick="lkbAccionRuta_Click"></asp:LinkButton>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoRuta" />
<asp:AsyncPostBackTrigger ControlID="btnConfirmarCalculo" />
</Triggers>
</asp:UpdatePanel>
</div>

<div id="detalles" class="modal">
<div id="contenedorDetalles" class="contenedor_modal_seccion_completa_arriba" style="width:1000px;top:1px">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCasetas" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCasetas" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="detalles" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="header_seccion">
<img src="../Image/Documento.png" />
<h2 runat="server">Casetas
</h2>
</div>

<div class="contenedor_seccion_completa" style="width: 93%">
<div class="renglon3x">
    <div class="etiqueta3x" >
   <asp:UpdatePanel ID="uplblDimensionesUnidadCasetas" runat="server" UpdateMode="Conditional">
       <ContentTemplate>
    <asp:Label ID="lblDimensionesUnidadCasetas" runat="server" Text="IAVE" CssClass="label_negrita"></asp:Label>
           </ContentTemplate>
       <Triggers>
           <asp:AsyncPostBackTrigger ControlID="gvRuta" />
       </Triggers>
       </asp:UpdatePanel>
        </div>
</div>
<div class="renglon3x">
<div class="etiqueta">
<label for="ddlTamanoCasetas">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoCasetas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoCasetas" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoCasetas_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoCasetas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoCasetas" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvCasetas" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplkbExportarCasetas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbExportarCasetas" runat="server" TabIndex="5"
OnClick="lkbExportarCasetas_Click" Text="Exportar"></asp:LinkButton>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="lkbExportarCasetas" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvCasetas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvCasetas" runat="server" AutoGenerateColumns="False" PageSize="25"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvCasetas_PageIndexChanging" OnSorting="gvCasetas_Sorting" AllowPaging="True" AllowSorting="True">
<Columns>
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
<asp:BoundField DataField="TipoCaseta" HeaderText="Tipo Caseta" SortExpression="TipoCaseta"  />
<asp:BoundField DataField="RedCarretera" HeaderText="Red Carretera" SortExpression="RedCarretera" />
<asp:BoundField DataField="IAVE" HeaderText="IAVE" SortExpression="IAVE"  />
<asp:BoundField DataField="MontoIAVE" HeaderText="Monto IAVE" SortExpression="MontoIAVE" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}"
    />
<asp:BoundField DataField="Monto" HeaderText="Monto Efectivo" SortExpression="Monto" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}"
    />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoCasetas" />
<asp:AsyncPostBackTrigger ControlID="gvRuta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="columna2x">
<div class="header_seccion">
    <img src="../Image/Depositos.png" />
<h2 runat="server">Conceptos</h2>
</div>
<div class="contenedor_seccion_completa" style="width: 93%">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoConceptos">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoConceptos" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoConceptos_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoConceptos" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvConceptos" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvConceptos" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvConceptos" runat="server" AutoGenerateColumns="False" PageSize="25"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvConceptos_PageIndexChanging" OnSorting="gvConceptos_Sorting" AllowPaging="True" AllowSorting="True">
<Columns>
<asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion"  />
<asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto"  ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}"/>
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoConceptos" />
<asp:AsyncPostBackTrigger ControlID="gvRuta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div></div>
<div class="columna2x">
<div class="header_seccion">
<img src="../Image/EstacionCombustible.png" />
<h2 runat="server">Diesel</h2>
</div>
<div class="contenedor_seccion_completa" style="width: 93%">
<div class="renglon2x">
<div class="etiqueta">
<label for="ddlTamanoDiesel">Mostrar:</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upddlTamanoDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:DropDownList ID="ddlTamanoDiesel" runat="server" CssClass="dropdown"
OnSelectedIndexChanged="ddlTamanoDiesel_SelectedIndexChanged" AutoPostBack="true">
</asp:DropDownList>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<div class="etiqueta">
<label>Ordenado Por:</label>
</div>
<div class="etiqueta">
<asp:UpdatePanel ID="uplblOrdenadoDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Label ID="lblOrdenadoDiesel" runat="server"></asp:Label>
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvDiesel" EventName="Sorting" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="grid_seccion_completa_200px_altura">
<asp:UpdatePanel ID="upgvDiesel" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:GridView ID="gvDiesel" runat="server" AutoGenerateColumns="False" PageSize="25"
ShowFooter="True" CssClass="gridview" Width="100%" OnPageIndexChanging="gvDiesel_PageIndexChanging" OnSorting="gvDiesel_Sorting" AllowPaging="True" AllowSorting="True">
<Columns>
<asp:BoundField DataField="Litros" HeaderText="Litros" SortExpression="Litros" />
<asp:BoundField DataField="TipoOperacion" HeaderText="Tipo Operación" SortExpression="TipoOperacion"  />
<asp:BoundField DataField="UbicacionEstacion" HeaderText="Ubicación Estación" SortExpression="UbicacionEstacion" />
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
<asp:AsyncPostBackTrigger ControlID="ddlTamanoDiesel" />
<asp:AsyncPostBackTrigger ControlID="gvRuta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
    </div>
</div>
</div>
<div id="contenidoConfirmarCalculo" class="modal">
<div id="confirmacionConfirmarCalculo" class="contenedor_ventana_confirmacion">   
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCalculo" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCalculo" runat="server" CommandName="calculo"  OnClick="lkbCerrarVentanaModal_Click" Text="Cerrar" TabIndex="16">
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>         
<div class="header_seccion">
<img src="../Image/Exclamacion.png" />                 
<h3>Confirmar </h3>
</div>
<div class="columna2x">
<div class="renglon2x"></div>
<div class="renglon2x">
<label class="mensaje_modal">¿Realmente desea realizar el Cálculo de la Ruta </label>
</div>
<div class="renglon2x"></div>
<div class="renglon2x">
<div class="etiqueta">
<label for="chkEfectivoCasetas">Efectivo Casetas</label>
</div>
<div class="control">
<asp:UpdatePanel ID="upchkEfectivoCasetas" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:CheckBox ID="chkEfectivoCasetas" TabIndex="9" runat="server" />
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvRuta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>
<div class="renglon2x">
<div class="controlBoton">
<asp:UpdatePanel ID="upbtnConfirmarCalculo" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<asp:Button ID="btnConfirmarCalculo" runat="server"   OnClick="btnConfirmarCalculo_Click"  CssClass="boton"  Text="Calcular" />
</ContentTemplate>
</asp:UpdatePanel>
</div>
</div>
</div>                
</div>
</div>
<%--</div>--%>
