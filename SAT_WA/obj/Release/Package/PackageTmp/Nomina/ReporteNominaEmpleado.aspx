<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReporteNominaEmpleado.aspx.cs" Inherits="SAT.Nomina.ReporteNominaEmpleado" %>

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
    //Obtener la instancia actual de la página y añadir manejador de evento 
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
    //Manejador de evento de Terminacion de peticion web. (Permite reasignar scripts después de actualizaciones parciales)
    function EndRequest(sender, args) {
      if (args.get_error() == undefined) {
        ConfiguraJQueryReporteNomina();
      }
    }

    //Declarando Función de Configuración
    function ConfiguraJQueryReporteNomina() {
      $(document).ready(function () {
        //Selector de fechas
        $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y',
              });
              $("#<%=txtFechaFin.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y',
              });
              //Autocompleta Empleado
              $("#<%=txtEmpleado.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=38&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>'
              });
              //Declarando Función de Validación de Encabezado
              var validaNominaEncabezado = function () {
                //Obteniendo Validación de Controles
                var isValid1 = !$("#<%=txtEmpleado.ClientID%>").validationEngine('validate');
                  var isValid2 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
                  var isValid3 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
                //Devolviendo Resultados Obtenidos
                return isValid1 && isValid2 && isValid3;
              }
              //Añadiendo Validación a los Eventos de Guardado
              $("#<%=btnBuscar.ClientID%>").click(validaNominaEncabezado);
      });
    }
    //Invocando método de configuracion
    ConfiguraJQueryReporteNomina();
  </script>
  <div id="encabezado_forma">
    <img src="../Image/FacturacionCargos.png" />
    <h1>Reporte - Nómina de Empleados</h1>
  </div>
  <!--Filtros-->
  <div class="seccion_controles">
    <asp:Panel runat="server" DefaultButton="btnBuscar">
      <!--Columna filtros-->
      <div class="columna2x">
        <!--Renglón Búsqueda-->
        <div class="renglon2x">
          <div class="etiqueta_400px">
            <label><b>BUSCAR POR EMPLEADO Y/O NÓMINA</b></label>
          </div>
        </div>
        <!--Empleado-->
        <div class="renglon2x">
          <div class="etiqueta">
            <label>Empleado:</label>
          </div>
          <div class="control2x">
            <asp:UpdatePanel ID="uptxtEmpleado" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:TextBox ID="txtEmpleado" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="1"></asp:TextBox>
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
        <!--No. Nomina-->
        <div class="renglon2x">
          <div class="etiqueta">
            <label>No. de nómina:</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uptxtNoNomina" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:TextBox ID="txtNoNomina" runat="server" CssClass="textbox_100px" TabIndex="2"></asp:TextBox>
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label>Estatus: </label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="upddlEstatus" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="3"></asp:DropDownList>
              </ContentTemplate>
              <Triggers>
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <!--Tipo (Ordinaria/Extraordinaria)-->
        <%--<div class="renglon2x">
                <div class="etiqueta_155px">
                    <label>Tipo de nómina</label>
                </div>
                <div class="control2x">
                    <asp:UpdatePanel ID="upddlTipoNomina" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTipoNomina" runat="server" CssClass="dropdown"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>--%>
      </div>
      <div class="columna2x">
        <!--Renglón Filtro-->
        <div class="renglon2x">
          <div class="etiqueta_200px">
            <label><b>AÑADIR FILTROS DE FECHA:</b></label>
          </div>
          <!--Check Fechas Nómina-->
          <div class="etiqueta">
            <asp:UpdatePanel ID="upchkFiltroNomina" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:CheckBox AutoPostBack="true" ID="chkFiltroNomina" runat="server" Text="Nómina" OnCheckedChanged="chkFiltroNomina_CheckedChanged" TabIndex="4" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
          <!--Check Fechas Pago-->
          <div class="etiqueta">
            <asp:UpdatePanel ID="upchkFiltroPago" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:CheckBox AutoPostBack="true" ID="chkFiltroPago" runat="server" Text="Pago" OnCheckedChanged="chkFiltroPago_CheckedChanged" TabIndex="5" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
        <!--Fecha inicio-->
        <div class="renglon2x">
          <div class="etiqueta">
            <label>Desde:</label>
          </div>
          <div class="control2x">
            <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox validate[custom[date]]" Enabled="false" TabIndex="6"></asp:TextBox>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkFiltroNomina" />
                <asp:AsyncPostBackTrigger ControlID="chkFiltroPago" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <!--Fecha fin-->
        <div class="renglon2x">
          <div class="etiqueta">
            <label>Hasta:</label>
          </div>
          <div class="control2x">
            <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox validate[custom[date]]" Enabled="false" TabIndex="7"></asp:TextBox>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkFiltroNomina" />
                <asp:AsyncPostBackTrigger ControlID="chkFiltroPago" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <!--Boton-->
        <div class="renglon2x">
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="8" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </asp:Panel>
  </div>
  <!--GridView-->
  <div class="seccion_controles">
    <div class="header_seccion">
      <img src="../Image/Totales.png" />
      <h2>Detalles de nómina</h2>
    </div>
    <!--Renglon encabezado-->
    <div class="renglon3x">
      <!--LBL Mostrar-->
      <div class="etiqueta">
        <label for="ddlTamanoGrid">Mostrar:</label>
      </div>
      <!--DDL Mostrar-->
      <div class="control">
        <asp:UpdatePanel ID="upddlTamanoGrid" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:DropDownList AutoPostBack="true" ID="ddlTamanoGrid" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlTamanoGrid_SelectedIndexChanged"></asp:DropDownList>
          </ContentTemplate>
        </asp:UpdatePanel>
      </div>
      <!--LBL "Ordenado"-->
      <div class="etiqueta">
        <label>Ordenado:</label>
      </div>
      <!--LBL Ordenado-->
      <div class="etiqueta_155px">
        <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <b>
              <asp:Label ID="lblOrdenado" runat="server"></asp:Label></b>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvNominaEmpleados" EventName="Sorting" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <!--LNK Exportar-->
      <div class="etiqueta_50pxr">
        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" CommandName="NominaEmpleado" OnClick="lnkExportar_Click"></asp:LinkButton>
          </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportar" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <!--GridView-->
    <div class="grid_seccion_completa_400px_altura">
      <asp:UpdatePanel ID="upgvNominaEmpleado" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:GridView ID="gvNominaEmpleados" runat="server" AllowPaging="true" AllowSorting="true" PageSize="25"
            CssClass="gridview" ShowFooter="true" OnSorting="gvNominaEmpleado_OnSorting" OnPageIndexChanging="gvNominaEmpleado_OnPageIndexChanging" AutoGenerateColumns="false" Width="100%">
            <AlternatingRowStyle CssClass="gridviewrowalternate" />
            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
            <FooterStyle CssClass="gridviewfooter" />
            <HeaderStyle CssClass="gridviewheader" />
            <RowStyle CssClass="gridviewrow" />
            <SelectedRowStyle CssClass="gridviewrowselected" />
            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
            <Columns>
              <asp:TemplateField>
                <HeaderTemplate>
                  <asp:CheckBox ID="chkTodosNominaEmpleado" runat="server" OnCheckedChanged="chkTodosNominaEmpleado_CheckedChanged" AutoPostBack="true" />
                </HeaderTemplate>
                <ItemTemplate>
                  <asp:CheckBox ID="chkVariosNominaEmpleado" runat="server" OnCheckedChanged="chkTodosNominaEmpleado_CheckedChanged" AutoPostBack="true" />
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
              <asp:BoundField DataField="NoNomina" HeaderText="Nómina" SortExpression="NoNomina" />
              <asp:BoundField DataField="Empleado" HeaderText="Empleado" SortExpression="Empleado" />
              <asp:BoundField DataField="FechaPago" HeaderText="Fecha de Pago" SortExpression="FechaPago" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
              <asp:BoundField DataField="FechaNomina" HeaderText="Fecha de Nomina" SortExpression="FechaNomina" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
              <asp:BoundField DataField="SerieFolio" HeaderText="Serie Folio" SortExpression="SerieFolio" />
              <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
              <asp:BoundField DataField="SalarioDiarioIntegrado" HeaderText="Salario Diario Integrado" SortExpression="SalarioDiarioIntegrado" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
              <asp:BoundField DataField="SalarioCotizacionAprobacion" HeaderText="Salario Cotización Aprovación" SortExpression="SalarioCotizacionAprobacion" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
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

              <asp:BoundField DataField="HorasExtra" HeaderText="Horas Extra" SortExpression="HorasExtra" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
              <asp:BoundField DataField="TotalPercepciones" HeaderText="Total Percepciones" SortExpression="TotalPercepciones" DataFormatString="{0:C2}">
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
              <asp:BoundField DataField="TotalDeducciones" HeaderText="Total Deducciones" SortExpression="TotalDeducciones" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
              <asp:BoundField DataField="TotalOtrosPagos" HeaderText="Total Otros Pagos" SortExpression="TotalOtrosPagos" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
              <asp:BoundField DataField="TotalPagado" HeaderText="Total Pagado" SortExpression="TotalPagado" DataFormatString="{0:C2}">
                <ItemStyle HorizontalAlign="Right" Font-Bold="true" />
                <FooterStyle HorizontalAlign="Right" />
              </asp:BoundField>
              <asp:TemplateField>
                <HeaderTemplate>
                  <label>↓</label>
                </HeaderTemplate>
                <ItemTemplate>
                  <asp:UpdatePanel ID="uplnkImprimir" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                      <asp:LinkButton ID="lnkPDF" runat="server" Text="PDF" CommandName="PDF" OnClick="lnkDescargarNomina_Click"></asp:LinkButton>
                      <asp:LinkButton ID="lnkXML" runat="server" Text="XML" CommandName="XML" OnClick="lnkDescargarNomina_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>

                      <asp:PostBackTrigger ControlID="lnkXML" />
                    </Triggers>
                  </asp:UpdatePanel>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </ContentTemplate>
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
          <asp:AsyncPostBackTrigger ControlID="ddlTamanoGrid" />
        </Triggers>
      </asp:UpdatePanel>
    </div>
  </div>
  <!--Exportar-->
  <div class="seccion_controles">
    <div class="renglon2x">
      <!--LBL "Exportar archivos"-->
      <div class="celda_etiqueta">
        <label class="label" for="chkPDF"><b>Exportar archivos</b></label>
      </div>
      <!--CHK PDF-->
      <div class="etiqueta_50px">
        <asp:UpdatePanel runat="server" ID="upchkPDF">
          <ContentTemplate>
            <asp:CheckBox ID="chkPDF" Text="PDF" runat="server" />
          </ContentTemplate>
          <Triggers>
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <!--CHK XML-->
      <div class="etiqueta_50px">
        <asp:UpdatePanel runat="server" ID="upchkXML">
          <ContentTemplate>
            <asp:CheckBox ID="chkXML" Text="XML" runat="server" />
          </ContentTemplate>
          <Triggers>
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="renglon_boton">
      <div class="controlBoton">
        <asp:UpdatePanel ID="upbtnExportar" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Button ID="btnExportar" runat="server" CssClass="boton" Text="Exportar" OnClick="btnExportar_Click" />
          </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="btnExportar" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
  </div>
</asp:Content>
