<%@ Page Title="Cambio de Operador" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="CambioOperador.aspx.cs" Inherits="SAT.Soporte.CambioOperador" %>
<%@ Register  Src="~/UserControls/wucSoporteTecnico.ascx" TagName="wucSoporteTecnico" TagPrefix="tectos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
<link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
<link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
<!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
<link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
<link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
<link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
<!-- Bibliotecas para Validación de formulario --> 
<script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
<script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
<!-- Biblioteca para uso de datetime picker -->
<script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!--Biblioteca para fijar encabeados GridView-->
<script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
//Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
function EndRequestHandler(sender, args) {
if (args.get_error() == undefined) {
ConfiguraJQueryCambioOperador();
}
}

//Declarando Función de Configuración
        function ConfiguraJQueryCambioOperador() {
            $(document).ready(function () {

                //Añadiendo Encabezado Fijo
                $("#<%=gvMovimientos.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorMovimientos").offsetWidth - 15,
                    height: 400
                });

                //Añadiendo Encabezado Fijo
                $("#<%=gvDepositos.ClientID%>").gridviewScroll({
                    width: document.getElementById("contenedorDepositos").offsetWidth - 15,
                    height: 400
                });

                //Añadiendo Encabezado Fijo
                //$("#<%//=gvRecursos.ClientID%>").gridviewScroll({
                  //  width: document.getElementById("contenedorRecursos").offsetWidth - 15,
                    //height: 400
                //});

                //Cargando Catalogo AutoCompleta Operadores Disponibles
                $("#<%=txtNuevoOpe.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=27&param=<%= ((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
                    appendTo: "#cambioOperador"
                });
                
                //Boton Confirmación Cambio Operador
                $("#<%=btnAcepta.ClientID%>").click(function () {
                    var isValid1 = !$("#<%=txtNuevoOpe.ClientID%>").validationEngine('validate');
                    return isValid1;
                });
            });
        }

//Invocando Función de Configuración
ConfiguraJQueryCambioOperador();
</script> 
    <div id="encabezado_forma">
        <img src="../Image/Servicio.png" />
        <h1>Servicio</h1>
    </div>
    <div class="seccion_controles" style="width:650px;">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Búsqueda</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>No. Servicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox" TabIndex="1" MaxLength="15"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>No. Viaje</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoViaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoViaje" runat="server" CssClass="textbox" TabIndex="2" MaxLength="15"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                      <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                      <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="3" CssClass="boton" OnClick="btnBuscar_Click"/>
                </ContentTemplate>
                      </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="seccion_controles" style="width:640px;">
        <div class="header_seccion">
            <h2>Datos del Servicio</h2>
        </div>
        <div class="columna">
            <div class="renglon">
                <div class="etiqueta">
                    <label>No. Servicio</label>
                </div>
                <div class="etiqueta">
                   <asp:UpdatePanel ID="uplblNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblNoServicio" runat="server" CssClass="label_negrita" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                    </asp:UpdatePanel>
                </div>
                </div>
                <div class="renglon">
                <div class="etiqueta">
                    <label>Estatus</label>
                </div>
                <div class="etiqueta">
                   <asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblEstatus" runat="server" CssClass="label_correcto" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                    </asp:UpdatePanel>
                </div>
                </div>
                <div class="renglon2x">
                <div class="etiqueta">
                    <label>Inicio Viaje</label>
                </div>
                <div class="etiqueta_200px">
                   <asp:UpdatePanel ID="uplblIniViaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblIniViaje" runat="server" CssClass="label_negrita" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                    </asp:UpdatePanel>
                </div>
                </div>
                <div class="renglon2x">
                <div class="etiqueta">
                   <label>Fin Viaje</label>
                </div>
                <div class="etiqueta_200px">
                   <asp:UpdatePanel ID="uplblFinViaje" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblFinViaje" runat="server" CssClass="label_negrita" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                   </asp:UpdatePanel>
                </div>
                </div>
            
                <div class="renglon2x">
                <div class="etiqueta">
                    <label>Cliente</label>
                </div>
                <div class="etiqueta_320px">
                   <asp:UpdatePanel ID="uplblCliente" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblCliente" runat="server" CssClass="label_negrita" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                   </asp:UpdatePanel>
                </div>
                </div>
                <div class="renglon2x">
                <div class="etiqueta">
                   <label>Fecha Doc.</label>
                </div>
                <div class="etiqueta_200px">
                   <asp:UpdatePanel ID="uplblFecDoc" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblFecDoc" runat="server" CssClass="label_negrita" Text="Por Asignar"/>
                        </ContentTemplate>
                       <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                       </Triggers>
                   </asp:UpdatePanel>
                </div>
                </div>
            </div>
    </div>
<div class="contenedor_seccion_completa">
<div class="header_seccion">
    <img src="../Image/paradas.png" />
    <h2>Movimientos</h2>
</div>
<div class="renglon4x">
<div class="etiqueta">
    <label for="ddlTamano">Mostrar</label>
</div>
<div class="control">
    <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
    </Triggers>
    </asp:UpdatePanel>
</div>
<div class="etiqueta">
<label for="lblOrdenado">Ordenado</label>
</div>
<div class="control">
<asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <b><asp:Label ID="lblOrdenado" runat="server" TabIndex="6"></asp:Label></b>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
    </Triggers>
    </asp:UpdatePanel>
    </div>
<div class="etiqueta">
    <asp:UpdatePanel ID="uplnkCambiar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="lnkCambiar" runat="server" TabIndex="7" OnClick="lnkCambiar_Click" ToolTip="Cambiar Operador" >
                <asp:Image ID="Cambiar" runat="server" ImageUrl="~/Image/ActualizarOperador.png" Width="20" Height="20" />
            </asp:LinkButton>            
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
        </Triggers>
    </asp:UpdatePanel>
</div>
    </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorMovimientos">
            <asp:UpdatePanel ID="upgvFichasIngreso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvMovimientos" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                        CssClass="gvMovimientos" ShowFooter="True" TabIndex="13" OnSorting="gvMovimientos_Sorting"
                        OnPageIndexChanging="gvMovimientos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                            <asp:CheckBox ID="chkTodos" runat="server"
                             OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="8" HeaderStyle-Width="50px"  ItemStyle-Width="50px"/>
                            </HeaderTemplate>
                            <ItemTemplate>
                            <asp:CheckBox ID="chkVarios" runat="server"
                             OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" TabIndex="9" HeaderStyle-Width="50px" ItemStyle-Width="50px"/>
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" HeaderStyle-Width="95px" ItemStyle-Width="95px"/>
                            <asp:BoundField DataField="NoMovimiento" HeaderText="No. Movimiento" SortExpression="NoMovimiento" HeaderStyle-Width="76px" ItemStyle-Width="76px"/>
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" Visible="false" />
                            <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" HeaderStyle-Width="61px" ItemStyle-Width="61px"/>
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" HeaderStyle-Width="282px" ItemStyle-Width="282px"/>
                            <asp:BoundField DataField="Destino" HeaderText="Destino" SortExpression="Destino" HeaderStyle-Width="239px" ItemStyle-Width="239px"/>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" HeaderStyle-Width="146px" ItemStyle-Width="146px"/>
                            <asp:BoundField DataField="Proveedor" HeaderText="Tercero" SortExpression="Proveedor" HeaderStyle-Width="37px" ItemStyle-Width="37px"/>
                            <asp:BoundField DataField="Tractor" HeaderText="Tractor" SortExpression="Tractor" HeaderStyle-Width="36px" ItemStyle-Width="36px"/>
                            <asp:BoundField DataField="Remolque1" HeaderText="Remolque 1" SortExpression="Remolque1" HeaderStyle-Width="55px" ItemStyle-Width="55px"/>
                            <asp:BoundField DataField="Remolque2" HeaderText="Remolque 2" SortExpression="Remolque2" HeaderStyle-Width="56px" ItemStyle-Width="56px"/>
                            <asp:BoundField DataField="Dolly" HeaderText="Dolly" SortExpression="Dolly" HeaderStyle-Width="26px" ItemStyle-Width="26px"/>
                            <asp:BoundField DataField="Rabon" HeaderText="Rabon" SortExpression="Rabon" HeaderStyle-Width="33px" ItemStyle-Width="33px"/>
                            <asp:BoundField DataField="Torton" HeaderText="Torton" SortExpression="Torton" HeaderStyle-Width="32px" ItemStyle-Width="32px"/>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnAcepta" />
                    <asp:AsyncPostBackTrigger ControlID="ucSoporte" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    <div class="header_seccion">
            <img src="../Image/DepositosVale.png" style="width:32px" />
            <h2>Depositos y Vales</h2>
    </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorDepositos">
            <asp:UpdatePanel ID="upgvDepositos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvDepositos" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                        CssClass="gvDepositos" ShowFooter="True" TabIndex="9" OnSorting="gvDepositos_Sorting"
                        OnPageIndexChanging="gvDepositos_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false"/>
                            <asp:BoundField DataField="Num" HeaderText="Numero" SortExpression="Num" HeaderStyle-Width="70px" ItemStyle-Width="70px"/>
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" HeaderStyle-Width="57px" ItemStyle-Width="57px"/>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" HeaderStyle-Width="57px" ItemStyle-Width="57px"/>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" HeaderStyle-Width="45px" ItemStyle-Width="45px"/>
                            <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" HeaderStyle-Width="40px" ItemStyle-Width="40px"/>
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" HeaderStyle-Width="40px" ItemStyle-Width="40px"/>
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="FechaSolicitud" SortExpression="FechaSolicitud" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="72px" ItemStyle-Width="72px"/>
                            <asp:BoundField DataField="FechaCargaoDeposito" HeaderText="FechaCargaoDeposito" SortExpression="FechaCargaoDeposito" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="108px" ItemStyle-Width="108px"/>
                            <asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorizacion" SortExpression="FechaAutorizacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderStyle-Width="63px" ItemStyle-Width="63px"/>
                            <asp:BoundField DataField="NoServicio" HeaderText="No. Servicio" SortExpression="NoServicio" HeaderStyle-Width="40px" ItemStyle-Width="40px"/>
                            <asp:BoundField DataField="Movimiento" HeaderText="Movimiento" SortExpression="Movimiento" HeaderStyle-Width="320px" ItemStyle-Width="320px"/>
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" HeaderStyle-Width="53px" ItemStyle-Width="53px"/>
                            <asp:BoundField DataField="Informacion" HeaderText="Informacion" SortExpression="Informacion" HeaderStyle-Width="142px" ItemStyle-Width="142px"/>
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" HeaderStyle-Width="51px" ItemStyle-Width="51px"/>
                            <asp:BoundField DataField="FacturaProveedor" HeaderText="Factura Proveedor" SortExpression="FacturaProveedor" HeaderStyle-Width="51px" ItemStyle-Width="51px"/>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnAcepta" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!--<div class="grid_seccion_completa_altura_variable" id="contenedorRecursos">-->
       <asp:UpdatePanel ID="upgvRecursos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                        <asp:GridView ID="gvRecursos" runat="server" AllowPaging="True" AllowSorting="True" PageSize="25"
                        CssClass="gvRecursos" ShowFooter="True" TabIndex="10" OnSorting="gvRecursos_Sorting"
                        OnPageIndexChanging="gvRecursos_PageIndexChanging" AutoGenerateColumns="false" Width="100%" Visible="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                            <asp:BoundField DataField="IdTipoAsignacion" HeaderText="Tipo Asignacion" SortExpression="IdTipoAsignacion" />
                            <asp:BoundField DataField="IdRecurso" HeaderText="Recurso Asignado" SortExpression="IdRecurso" />
                            <asp:BoundField DataField="IdEstatus" HeaderText="Estatus" SortExpression="IdEstatus" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                    <asp:AsyncPostBackTrigger ControlID="btnAcepta" />
                    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                    <asp:AsyncPostBackTrigger ControlID="ucSoporte" />
                </Triggers>
            </asp:UpdatePanel>
        <!--</div>-->


<!-- VENTANA MODAL DE SOPORTE TECNICO -->
<div id="soporteTecnicoModal" class="modal">
<div id="soporteTecnico" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;" >
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarEliminacionVale" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarEliminacionVale" runat="server" OnClick="lkbCerrarSoporte_Click" CommandName="Soporte" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upucSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucSoporteTecnico ID="ucSoporte" runat="server" TabIndex="11" OnClickGuardarSoporte="wucSoporteTecnico_ClickAceptarSoporte" OnClickCancelarSoporte="wucSoporteTecnico_ClickCancelarSoporte" />
</ContentTemplate>
<Triggers>
    <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
    <asp:AsyncPostBackTrigger ControlID="btnAcepta" />
</Triggers>
</asp:UpdatePanel>
</div>
</div>


<!-- VENTANA MODAL DE CAMBIO OPERADOR -->
<div id="cambioOperadorModal" class="modal">
<div id="cambioOperador" class="contenedor_ventana_confirmacion_arriba" style="width:500px;padding-bottom:8px;" >
<asp:Panel ID="btnEnter" runat="server" DefaultButton="btnAcepta">
<div class="boton_cerrar_modal">
<asp:UpdatePanel runat="server" ID="uplkbCerrarCambioOperador" UpdateMode="Conditional">
<ContentTemplate>
<asp:LinkButton ID="lkbCerrarCambioOperador" runat="server" OnClick="lkbCerrarCambioOperador_Click" CommandName="CambioOpe" >
<img src="../Image/Cerrar16.png" />
</asp:LinkButton>
</ContentTemplate>
</asp:UpdatePanel>
</div>
    <div class="header_seccion">
        <img src="../Image/ActualizarOperador.png" style="width:32px" />
        <h2>Cambio de Operador</h2>
    </div>
    <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Operador Actual</label>
                </div>
                <div class="etiqueta_320px">
                    <asp:UpdatePanel ID="uplblAntOpe" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblAntOpe" runat="server" CssClass="label_negrita" TabIndex="12"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkCambiar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Nuevo Operador</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNueOpe" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Textbox ID="txtNuevoOpe" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" TabIndex="13" ></asp:Textbox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lnkCambiar" />
                            <asp:AsyncPostBackTrigger ControlID="gvMovimientos" />
                            <asp:AsyncPostBackTrigger ControlID="lkbCerrarCambioOperador" />
                            <asp:AsyncPostBackTrigger ControlID="btnAcepta" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                      <asp:UpdatePanel ID="upbtnAcepta" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                      <asp:Button ID="btnAcepta" runat="server" Text="Aceptar" TabIndex="14" CssClass="boton" OnClick="btnAceptar_Click"/>
                </ContentTemplate>
                      </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
</div>
</asp:Content>

