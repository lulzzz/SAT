<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteNomina12.aspx.cs" Inherits="SAT.Nomina.ReporteNomina12" %>

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
  <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
  <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteNomina();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryReporteNomina() {
            $(document).ready(function () {

                //Cargando Controles DateTimePicker
                $("#<%=txtFecIniPago.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFecFinPago.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Declarando Función de Validación de Encabezado
                var validaNominaEncabezado = function () {

                    //Obteniendo Validación de Controles
                    var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFecIniPago.ClientID%>").validationEngine('validate');
                    var isValid3 = !$("#<%=txtFecFinPago.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3;
                }

                //Añadiendo Validación a los Eventos de Guardado
                $("#<%=btnBuscar%>").click(validaNominaEncabezado);

            });
        }

        //Invocando Método de Configuración
        ConfiguraJQueryReporteNomina();
  </script>
    <div id="encabezado_forma">
        <img src="../Image/FacturacionCargos.png" />
        <h1>Reporte de Nómina</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoNomina">No. de Nomina</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uptxtNoNomina" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoNomina" runat="server" CssClass="textbox_100px" MaxLength="9" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkCitaCarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkFechaPago" runat="server" Text="Fecha Pago" Checked="true" TabIndex="2" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkCitaDescarga" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkFechaNomina" runat="server" Text="Fecha Nomina" TabIndex="3" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIniPago">Inicio Pago</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIniPago" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIniPago" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="20" TabIndex="4"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFinPago">Fin Pago</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFinPago" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFinPago" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" MaxLength="20" TabIndex="5"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="6" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="seccion_controles">
        <div class="header_seccion">
            <img src="../Image/Totales.png" />
            <h2>Detalles de Nómina</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar:</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="13"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenado">Ordenado:</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvNominaEncabezado" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta_50pxr">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" CommandName="NominaEncabezado" OnClick="lnkExportar_Click" TabIndex="14"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_400px_altura">
            <asp:UpdatePanel ID="upgvNominaEncabezado" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvNominaEncabezado" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
                        CssClass="gridview" ShowFooter="true" TabIndex="15" OnSorting="gvNominaEncabezado_Sorting"
                        OnPageIndexChanging="gvNominaEncabezado_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
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
                            <asp:BoundField DataField="NoNomina" HeaderText="No. Nomina" SortExpression="NoNomina" />
                            <asp:BoundField DataField="FechaPago" HeaderText="Fecha de Pago" SortExpression="FechaPago" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaNomina" HeaderText="Fecha de Nomina" SortExpression="FechaNomina" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Aguinaldo" HeaderText="Aguinaldo" SortExpression="Aguinaldo" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Sueldo" HeaderText="Sueldo" SortExpression="Sueldo" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OtrasPercepciones" HeaderText="Otras Percepciones" SortExpression="OtrasPercepciones" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                           <asp:BoundField DataField="SeparacionIndemnizacion" HeaderText="Separación Indemnizacion" SortExpression="SeparacionIndemnizacion" DataFormatString="{0:C2}">
                              <ItemStyle HorizontalAlign="Right" />
                              <FooterStyle HorizontalAlign="Right" /> 
                            </asp:BoundField>
                            <asp:BoundField DataField="HrsExtra" HeaderText="Hrs. Extra" SortExpression="HrsExtra" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TPercepciones" HeaderText="Total Percepciones" SortExpression="TPercepciones" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="IMSS" HeaderText="IMSS" SortExpression="IMSS" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ISPT" HeaderText="ISPT" SortExpression="ISPT" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Infonavit" HeaderText="Infonavit" SortExpression="Infonavit" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Incapacidad" HeaderText="Incapacidad" SortExpression="Incapacidad" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OtrasDeducciones" HeaderText="Otras Deducciones" SortExpression="OtrasDeducciones" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TDeducciones" HeaderText="Total Deducciones" SortExpression="TDeducciones" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TOtrosPagos" HeaderText="Total Otros Pagos" SortExpression="TOtrosPagos" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TPagado" HeaderText="Total Pagado" SortExpression="TPagado" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVerNomina" runat="server" Text="Ver Nómina" CommandName="VerNomina" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCopiarNomina" runat="server" Text="Copiar" CommandName="CopiarNomina" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTimbraNE" runat="server" Text="Timbrar" CommandName="Timbrar" OnClick="lnkActualizaNomina_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
      <!-- Ventana Timbrado Nomina -->
    <div id="contenedorVentanaTimbradoNomina" class="modal">
        <div id="ventanaTimbradoNomina" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarTimbrado" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarTimbrado" runat="server" OnClick="lnkCerrar_Click" CommandName="TimbradoNomina" 
                            Text="Cerrar" TabIndex="37">
                            <img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Timbrado de Nomina</h2>
            </div>
            <div class="columna2x">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtSerie">Serie</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" TabIndex="38"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvNominaEncabezado" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                     <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAceptarTimbradoEmpleado" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAceptarTimbradoNomina" CommandName="TimbrarNomina" runat="server" CssClass="boton" Text="Aceptar" 
                                    OnClick="btnAceptarTimbradoNomina_Click" TabIndex="39" />
                            </ContentTemplate>
                             <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvNominaEncabezado" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
