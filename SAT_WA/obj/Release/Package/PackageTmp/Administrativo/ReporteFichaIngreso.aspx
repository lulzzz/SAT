<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteFichaIngreso.aspx.cs" Inherits="SAT.Administrativo.ReporteFichaIngreso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos documentación de servicio -->
    <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
    <link href="../CSS/ControlesUsuario.css" rel="stylesheet" />
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteFichasIngreso();
            }
        }

        //Declarando Función de Configuración
        function ConfiguraJQueryReporteFichasIngreso() {
            $(document).ready(function () {
                //Cargando Controles de Fecha
                $("#<%=txtFecIni.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });
                $("#<%=txtFecFin.ClientID%>").datetimepicker({
                    lang: 'es',
                    format: 'd/m/Y H:i'
                });

                //Añadiendo Validación al Evento Click del Boton
                $("#<%=btnBuscar.ClientID%>").click(function () {
                    var isValid1 = !$("#<%=txtNoFicha.ClientID%>").validationEngine('validate');
                    var isValid2;
                    var isValid3;

                    //Validando el Control
                    if ($("#<%=chkIncluir.ClientID%>").is(':checked') == true) {
                        //Validando Controles
                        isValid2 = !$("#<%=txtFecIni.ClientID%>").validationEngine('validate');
                        isValid3 = !$("#<%=txtFecFin.ClientID%>").validationEngine('validate');
                    }
                    else {
                        //Asignando Valor Positivo
                        isValid2 = true;
                        isValid3 = true;
                    }

                    var isValid4 = !$("#<%=txtNombreDep.ClientID%>").validationEngine('validate');

                    //Devolviendo Resultados Obtenidos
                    return isValid1 && isValid2 && isValid3 && isValid4;
                });
                
            });
        }
        //Declarando Función de Validación de Fechas
        function CompareDates() { 
            //Obteniendo Valores
            var txtDate1 = $("#<%=txtFecIni.ClientID%>").val();
            var txtDate2 = $("#<%=txtFecFin.ClientID%>").val();

            //Fecha en Formato MM/DD/YYYY
            var date1 = Date.parse(txtDate1.substring(5, 3) + "/" + txtDate1.substring(2, 0) + "/" + txtDate1.substring(10, 6) + " " + txtDate1.substring(13, 11) + ":" + txtDate1.substring(16, 14));
            var date2 = Date.parse(txtDate2.substring(5, 3) + "/" + txtDate2.substring(2, 0) + "/" + txtDate2.substring(10, 6) + " " + txtDate2.substring(13, 11) + ":" + txtDate2.substring(16, 14));

            //Validando que la Fecha de Inicio no sea Mayor q la Fecha de Fin
            if (date1 > date2)
                //Mostrando Mensaje de Operación
                return "* La Fecha de Inicio debe ser inferior a la Fecha de Fin";
        }

        //Invocando Función de Configuración
        ConfiguraJQueryReporteFichasIngreso();
    </script>
    <div id="encabezado_forma">
        <img src="../Image/Evidencia.png" />
        <h1>Visor de Fichas de Ingreso</h1>
    </div>
    <div class="seccion_controles">
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCompaniaEmi">Compania</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtCompaniaEmi" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtCompaniaEmi" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">No. Ficha</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoFicha" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlEstatus">Estatus</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlTipoDep">Tipo Depositante</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoDep" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoDep" runat="server" CssClass="dropdown2x" TabIndex="3" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlTipoDep_SelectedIndexChanged"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNombreDep">Depositante</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="uptxtNombreDep" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNombreDep" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="4" AutoPostBack="true"
                                OnTextChanged="txtNombreDep_TextChanged"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTipoDep" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMetodoPago">Método de Pago</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlMetodoPago" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMetodoPago" runat="server" CssClass="dropdown" TabIndex="5"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlCtaOrigen">Cta. Origen</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlCtaOrigen" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCtaOrigen" runat="server" CssClass="dropdown" TabIndex="6"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtNombreDep" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlCtaDestino">Cta. Destino</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlCtaDestino" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCtaDestino" runat="server" CssClass="dropdown" TabIndex="7"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecIni">Fecha Inicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecIni" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecIni" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="8" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta">
                    <asp:UpdatePanel ID="upchkIncluir" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="¿Incluir?" TabIndex="9" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtFecFin">Fecha Fin</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFecFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFecFin" runat="server" CssClass="textbox validate[required, custom[dateTime24], funcCall[CompareDates[]]" TabIndex="10" MaxLength="16"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlConcepto">Concepto</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown2x" TabIndex="11"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="ddlMoneda">Moneda</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlMoneda" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMoneda" runat="server" CssClass="dropdown" TabIndex="12"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="13" CssClass="boton" OnClick="btnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <h2>Resultados Obtenidos</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamano">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" TabIndex="14" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged"></asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFI">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b><asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvFichasIngreso" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="15" OnClick="lnkExportar_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_media_altura">
            <asp:UpdatePanel ID="upgvFichasIngreso" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvFichasIngreso" runat="server" AllowPaging="true" AllowSorting="true" TabIndex="16"
                        OnPageIndexChanging="gvFichasIngreso_PageIndexChanging" OnSorting="gvFichasIngreso_Sorting"
                        PageSize="250" CssClass="gridview" ShowFooter="true" Width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>
                            <asp:BoundField DataField="IdFichaIngreso" HeaderText="IdFichaIngreso" SortExpression="IdFichaIngreso" Visible="false" />
                            <asp:BoundField DataField="NoFicha" HeaderText="No. Ficha" SortExpression="NoFicha" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                            <asp:BoundField DataField="Depositante" HeaderText="Depositante" SortExpression="Depositante" />
                            <asp:BoundField DataField="MetodoPago" HeaderText="Metodo de Pago" SortExpression="MetodoPago" />
                            <asp:BoundField DataField="CuentaOrigen" HeaderText="Cuenta de Origen" SortExpression="CuentaOrigen" />
                            <asp:BoundField DataField="CuentaDestino" HeaderText="Cuenta de Destino" SortExpression="CuentaDestino" />
                            <asp:BoundField DataField="NumCheque" HeaderText="No. de Cheque" SortExpression="NumCheque" />
                            <asp:BoundField DataField="Moneda" HeaderText="Fecha" SortExpression="Moneda" />
                            <asp:BoundField DataField="FechaCaptura" HeaderText="Fecha de Captura" SortExpression="FechaCaptura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:C2}" />
                            <asp:BoundField DataField="MontoPesos" HeaderText="Monto Pesos" SortExpression="MontoPesos" DataFormatString="{0:C2}" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
