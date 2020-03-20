<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucParada.ascx.cs" Inherits="SAT.UserControls.wucParada" %>
<%@ Register Src="~/UserControls/wucKilometraje.ascx" TagPrefix="tectos" TagName="wucKilometraje" %>

<script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
        if (args.get_error() == undefined) {
            ConfiguraJQueryParada();
        }
    }
    //Creando función para configuración de jquery en control de usuario
    function ConfiguraJQueryParada() {
        $(document).ready(function () {

            //Sugerencias de Ubicación
            $("#<%= txtUbicacion.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=2&param=<%=this.id_compania.ToString()%>',
                select: function (event, ui) {
                    //Asignando Selección al Valor del Control
                    $("#<%=txtUbicacion.ClientID%>").val(ui.item.value);
                    //Causando Actualización del Control
                    __doPostBack('<%= txtUbicacion.UniqueID %>', '');
                }
            });

    //Fecha de cita (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm')
    $("#<%= txtCita.ClientID%>").datetimepicker({
        lang: 'es',
        format: 'd/m/Y H:i'
    });
    //Validación Ubicación
    var validacionParada = function () {
        var isValidP1 = !$("#<%=txtUbicacion.ClientID%>").validationEngine('validate');
        var isValidP2;

        //Validando el Control
        if ($("#<%=chkTipoParada.ClientID%>").is(':checked') == false) {
            //Validando Controles
            isValidP2 = !$("#<%=txtCita.ClientID%>").validationEngine('validate');
        }
        else {
            //Asignando Valor Positivo
            isValidP2 = true;
        }

    return isValidP1 && isValidP2;
};
    //Validación de campos requeridos
    $("#<%=this.btnGuardar.ClientID%>").click(validacionParada);
    //Validación de campos requeridos
    $("#<%=this.btnAgregarAbajo.ClientID%>").click(validacionParada);
    //Validación de campos requeridos
    $("#<%=this.btnAgregarParadaArriba.ClientID%>").click(validacionParada);
});
}
//Diseño Grid View
$("[src*=plus]").live("click", function () {
    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
    $(this).attr("src", "../Image/minus.png");
});
$("[src*=minus]").live("click", function () {
    $(this).attr("src", "../Image/plus.png");
    $(this).closest("tr").next().remove();
});

//Invocación Inicial de método de configuración JQuery
ConfiguraJQueryParada();
</script>
    <div class="contenedor_seccion_95per">
        <div class="header_seccion">
            <img src="../Image/Direccion.png" />
            <h2>Paradas del Servicio</h2>
        </div>
        <div class="renglon100Per">
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoParadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label for="ddlTamanoParadas"></label>
                        <asp:DropDownList ID="ddlTamanoParadas" runat="server" OnSelectedIndexChanged="SelectedIndexChanged_ddlTamanoParadas"  AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenarParadas">Ordenado Por:</label>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplblOrdenarParadas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenarParadas" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvParadas" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarParadas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarParadas" runat="server" Text="Exportar Excel" OnClick="lnkExportar_Click" ></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarParadas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvParadas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvParadas" runat="server" AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AllowSorting="true" 
                        OnRowDataBound="OnRowDataBound" ShowHeaderWhenEmpty="True" OnSorting="Sorting_gvParadas" Width="100%"
                        OnPageIndexChanging="PageIndexChanging_gvParadas"
                        CssClass="gridview">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <img alt="" style="cursor: pointer" src="../Image/plus.png" />
                                    <asp:Panel ID="pnlParadas" runat="server" Style="display: none">
                                        <asp:UpdatePanel ID="upgvEventos" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvEventos" runat="server" AutoGenerateColumns="false" CssClass="gridview" GridLines="Both">
                                                    <Columns>
                                                        <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" DataFormatString="{0:0}" />
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                                        <asp:BoundField DataField="InicioEvento" HeaderText="Inicio Evento" SortExpression="InicioEvento" />
                                                        <asp:BoundField DataField="TipoActualizacionInicio" HeaderText="Tipo Actualizacion Inicio" SortExpression="TipoActualizacionInicio" />
                                                        <asp:BoundField DataField="FinEvento" HeaderText="Fin Evento" SortExpression="FinEvento" />
                                                        <asp:BoundField DataField="TipoActualizacionFin" HeaderText="Tipo Actualizacion Fin" SortExpression="TipoActualizacionFin" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lkbEditar" runat="server" Text="Cambiar Tipo" OnClick="OnClick_lkbEditarEvento"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar"  OnClick="OnClick_lkbEliminarEvento"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                            <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora"  OnClick="OnClick_lkbBitacoraEvento"></asp:LinkButton>
                                                                        </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="lkbBitacora" />
                                                                    </Triggers>
                                                                    </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle CssClass="gridview2footer" />
                                                    <HeaderStyle CssClass="gridview2header" />
                                                    <RowStyle CssClass="gridview2row" />
                                                    <SelectedRowStyle CssClass="gridview2rowselected" />
                                                    <AlternatingRowStyle CssClass="gridview2rowalternate" />
                                                    <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Secuencia" HeaderText="Secuencia" SortExpression="Secuencia" DataFormatString="{0:0}" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Cita" HeaderText="Cita" SortExpression="Cita" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaLlegada" HeaderText="Fecha Llegada" SortExpression="FechaLlegada" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="ActualizacionLlegada" HeaderText="Actualización Llegada" SortExpression="ActualizacionLlegada" />
                            <asp:BoundField DataField="RazonLlegadaTarde" HeaderText="Razon Llegada Tarde" SortExpression="RazonLlegadaTarde" />
                            <asp:BoundField DataField="FechaSalida" HeaderText="Fecha Salida" SortExpression="FechaSalida" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="ActualizacionSalida" HeaderText="Actualización Salida" SortExpression="ActualizacionSalida" />
                            <asp:TemplateField HeaderText="Kilometraje" SortExpression="Kilometraje">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkKilometraje" runat="server" Text='<%#Eval("Kilometraje") %>' OnClick="lnkKilometraje_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Tiempo" HeaderText="Tiempo Transito" SortExpression="Tiempo" />                            
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbSeleccionarParada" runat="server" ToolTip="Seleccionar" OnClick="OnClick_lkbSeleccionarParada">
                                        <asp:Image ID="Seleccionar" runat="server" ImageUrl="~/Image/Select.png" Width="20" Height="20" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbEliminarParada" runat="server" ToolTip="Eliminar" OnClick="OnClick_lkbEliminar">
                                        <asp:Image ID="Eliminar" runat="server" ImageUrl="~/Image/borrar.png" Width="20" Height="20" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbInsertarEvento" runat="server" ToolTip="Insertar Evento"  OnClick="OnClick_lkbInsertarEvento"  >
                                        <asp:Image ID="Insertar" runat="server" ImageUrl="~/Image/Agregar.png" Width="20" Height="20" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbReferenciasParada" runat="server" ImageUrl="~/Image/Referencias.png" OnClick="imbReferenciasParada_Click" Width="20px" ToolTip="Referencias" TabIndex="34"></asp:ImageButton>
                                    </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:UpdatePanel ID="uplkbBitacoraParada" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                    <asp:LinkButton ID="lkbBitacoraParada" runat="server" ToolTip="Bitácora"  OnClick="OnClick_lkbBitacoraParada">
                                        <asp:Image ID="Bitacora" runat="server" ImageUrl="~/Image/Bitacora.png" Width="20" Height="20" />
                                    </asp:LinkButton>
                                        </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="lkbBitacoraParada" />
                                    </Triggers>
                                    </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                    <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                    <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                    <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                    <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>        
        
        
        <div class="seccion_controles">
            <div class="columna400px">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="ddlTipoEvento">Evento</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTipoEvento" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTipoEvento" runat="server" CssClass="dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="chkTipoParada" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="upchkTipoParada" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkTipoParada" runat="server" Text="¿De Servicio?" 
                                    OnCheckedChanged="chkTipoParada_CheckedChanged" AutoPostBack="true" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUbicacion">Lugar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtUbicacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUbicacion" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" AutoPostBack="True"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="chkTipoParada" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCita">Cita</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtCita" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCita" runat="server" CssClass="textbox2x validate[required, custom[dateTime24]]"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="chkTipoParada" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div> 
                <div class="renglon2x">
                    <div class="controlr">
                         <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                                        CssClass="boton_cancelar" OnClick="btnCancelar_Click"  />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                    <asp:AsyncPostBackTrigger ControlID="chkTipoParada" />
                                </Triggers>
                            </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x"></div>
                <div class="renglon2x">
                    <div class="control" style="width: auto">
                        <asp:UpdatePanel ID="uplblbError" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="chkTipoParada" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>      
            </div>
            <div class="botones_parada">
                <h3>Opciones</h3>
            <div class="renglon_parada">
                <div class="boton_parada">
                    <asp:UpdatePanel ID="upbtnAgregarArriba" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnAgregarParadaArriba" runat="server" Text="Insertar Arriba" OnClick="OnClick_btnAgregarArriba" 
                                CssClass="boton" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="icono_parada"></div>
                <div class="icono_flecha"></div>            
            </div>
            <div class="renglon_parada">
                <div class="boton_parada">
                    <asp:UpdatePanel ID="upbtnGuardar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardar" runat="server" Text="Modificar Actual" OnClick="OnClick_btnEditar" 
                                    CssClass="boton" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                            </Triggers>
                        </asp:UpdatePanel>
                </div>
                <div class="icono_parada"></div>            
            </div>
            <div class="renglon_parada">
                <div class="boton_parada">
                     <asp:UpdatePanel ID="upbtnAgregarAbajo" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAgregarAbajo" runat="server" Text="Insertar Abajo" OnClick="OnClick_btnAgregarAbajo" 
                                    CssClass="boton" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                            </Triggers>
                        </asp:UpdatePanel>
                </div>
                <div class="icono_parada"></div>
                <div class="icono_flecha"></div>
            </div>       
        </div>
            <div class="columna2x">
                <div class="header_seccion">
                    <img src="../Image/Direccion.png" />
                    <h2>Totales Paradas</h2>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="lblTotalParadas">Paradas</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblTotalParadas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTotalParadas"></asp:Label>
                            </ContentTemplate>
                             <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblTotalKilometros">Kilometros</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblTotalKilometros" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTotalKilometros"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblTiempo">Transito</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel runat="server" ID="uplblTiempo" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTiempo"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>                    
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="lblCargas">Cargas</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblCargas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblCargas"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblOTCargas">Perdidas</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblOTCargas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblOTCargas"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblTiempoCarga">Tiempo</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel runat="server" ID="uplblTiempoCarga" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTiempoCarga"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div> 
                    
                </div>
                <div class="renglon2x">
                    <div class="etiqueta_50px">
                        <label for="lblDescargas">Descargas</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblDescargas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblDescargas"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblOTDescargas">Perdidas</label>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel runat="server" ID="uplblOTDescargas" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblOTDescargas"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                    <div class="etiqueta_50px">
                        <label for="lblTiempoDescarga">Tiempo</label>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel runat="server" ID="uplblTiempoDescarga" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblTiempoDescarga"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarParadaArriba" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarAbajo" />
                                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
                                <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTamanoParadas" />
                                <asp:AsyncPostBackTrigger ControlID="lkbCerrar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>                    
                </div>
            </div>
    </div>
</div>
<!-- VENTANA MODAL QUE MUESTRA EL CONTROL PARA ACTUALIZAR KILOMETRAJE -->
<div id="contenedorVentanaKilometraje" class="modal">
    <div id="ventanaKilometraje" class="contenedor_ventana_confirmacion">        
        <div class="boton_cerrar_modal">
            <asp:UpdatePanel runat="server" ID="uplkbCerrar" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="lkbCerrar" OnClick="lkbCerrar_Click" runat="server" >
                        <img src="../Image/Cerrar16.png" />
                    </asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:UpdatePanel ID="upucKilometraje" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <tectos:wucKilometraje ID="ucKilometraje" runat="server" OnClickGuardar="ucKilometraje_ClickGuardar"  />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvParadas" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
