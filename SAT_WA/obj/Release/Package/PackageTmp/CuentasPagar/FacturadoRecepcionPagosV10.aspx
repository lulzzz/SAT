<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="FacturadoRecepcionPagosV10.aspx.cs" Inherits="SAT.CuentasPagar.FacturadoRecepcionPagosV10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <!-- Estilos documentación de servicio -->
  <link href="../CSS/DocumentacionServicio.css" rel="stylesheet" />
  <link href="../CSS/Controles.css" type="text/css" rel="stylesheet" />
  <link href="../CSS/ControlesUsuario.css" type="text/css" rel="stylesheet" />
  <link href="../CSS/Forma.css" type="text/css" rel="stylesheet" />
  <link href="../CSS/Facturado.css" type="text/css" rel="stylesheet" />
  <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
  <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
  <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <style>
    #contenedorFacturaCxP {
      margin-left: 10px;
      height: 150px;
      width: 95%;
      border: dashed 3px;
      border-color: #a5d16f;
      background-color: #f8f8f8;
      display:flex;
      align-items:center;
      justify-content:center;
    }

    #instruccionContenedor {
      /*margin-left: 10%;*/
      width: 80%;
      /*margin-top: 12%;*/
      font-size: large;
      font-weight: bold;
      text-align: center;
      height: auto;
      color: #808080
    }
  </style>
  <%--<style>
    .renglon {
      border: 1px dotted red;
    }

    .control, .controlBoton {
      border: 1px dotted green;
    }

    .etiqueta {
      border: 1px dotted blue;
    }

    .columna {
      border: 1px dotted yellow;
    }
  </style>--%>
  <!-- Bibliotecas para Validación de formulario -->
  <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
  <script src='<% = ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
  <script src='<% = ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
  <script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
      if (args.get_error() == undefined) {
        //Invocando Función de Configuración
        ConfiguraJQueryFacturasPago();
      }
    }
    //Declarando Función de Configuración
    function ConfiguraJQueryFacturasPago() {
      //Agregar calendarios a las cajas de texto
      $(document).ready(function () {
        //Validar filtro
        var validaBusqueda = function (evt) {
          var isValid1 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
          var isValid2 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
          var isValid3 = !$("#<%=txtNoEgreso.ClientID%>").validationEngine('validate');
          var isValid4 = !$("#<%=txtNoAnticipo.ClientID%>").validationEngine('validate');
          var isValid5 = !$("#<%=txtNoLiquidacion.ClientID%>").validationEngine('validate');
          BorraNombreArchivoCargado();
          return isValid1 && isValid2 && isValid3 && isValid4 && isValid5;
        }

        //Agregar calendarios a las cajas de texto
        $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
          lang: 'es',
          format: 'd/m/Y',
        });
        $("#<%=txtFechaFin.ClientID%>").datetimepicker({
          lang: 'es',
          format: 'd/m/Y',
        });
      });
      //Agregar funcionalidad Drag-n-drop
      $(document).ready(function () {
        //Validando que el Navegador Soporte
        if ($.support.fileDrop) {
          $("#contenedorFacturaCxP").fileDrop({
            onFileRead: function (files) {
              $.each(files, function () {
                //Invocando Método de Guardado
                GuardaFacturaProveedor(files[0].data, files[0].name, files[0].type);
              });
            }
          });
        }
        else {
          //Mostrando Excepción
          alert('Su navegador actual no soporta la carga de archivos por arrastre :(');
        }
      });
    }

    //Invocando Función de Configuración
    ConfiguraJQueryFacturasPago();

    //Función de Guardado
    function GuardaFacturaProveedor(data, name, type) {
      //Construyendo Parametros del Valor de Datos
      var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "'}";
      //Definiendo Consumo
      $.ajax({
        type: "POST",
        url: "FacturadoRecepcionPagosV10.aspx/LecturaArchivo",
        data: dataValue,
        contentType: 'application/json',
        success: function (response) {
          //Cambiando nombre de archivo cargado
          CambiaNombreArchivoCargado(response.d);
          //Indicando carga correcta
          //alert(response.d);
        },
        error: function (err) {
          //Reiniciar control
          BorraNombreArchivoCargado();
        }
      });
    }
    //Función para actualizar el nombre del archivo cargado
    function CambiaNombreArchivoCargado(nombre) {
      $('#instruccionContenedor').text(nombre);
    };
    function BorraNombreArchivoCargado() {
      $('#instruccionContenedor').text('Arrastre y suelte sus archivos XML a este cuadro.');
    };
  </script>
  <div id="encabezado_forma">
    <img src="../Image/archivo_xml1.png" />
    <h1>Facturas de Proveedor - (Recepción de Pagos)</h1>
  </div>
  <div class="seccion_controles" style="padding:0;">
    <div class="header_seccion" style="width:50%; margin:0 0 10px 0; min-height: 50px;">
      <img src="../Image/AnalisisDoc.png" />
      <h2>Búsqueda de Egresos</h2>
    </div>
    <div class="header_seccion" style="width:50%; margin:0 0 10px 0; min-height: 50px;">
        <img src="../Image/archivo_xml2.png" />
      <h2>Complemento de pago</h2>
    </div>
    <div class="columna" style="width:50%">
      <div class="renglon">
        <div class="control">
          <asp:CheckBox ID="chkIncluirFechas" Text="Filtrar por fechas" runat="server" OnCheckedChanged="chkIncluirFechas_CheckedChanged" AutoPostBack="true" Checked="true" />
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtFechaInicio">Desde </label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtFechaInicio" runat ="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="textbox" Enabled="false" autocomplete="off"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="chkIncluirFechas" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtFechaFin">Hasta </label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtFechaFin" runat ="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtFechaFin" runat="server" CssClass="textbox" Enabled="false" autocomplete="off"></asp:TextBox>              
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="chkIncluirFechas" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtNoEgreso">No. de Egreso</label>
        </div>
        <div class="control">
          <asp:TextBox ID="txtNoEgreso" runat="server" CssClass=" textbox validate[custom[positiveNumber]]"></asp:TextBox>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtNoAnticipo">No. de Anticipo</label>
        </div>
        <div class="control">
          <asp:TextBox ID="txtNoAnticipo" runat="server" CssClass ="textbox validate[custom[positiveNumber]]"></asp:TextBox>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtNoLiquidacion">No. de Liquid.</label>
        </div>
        <div class="control">
          <asp:TextBox ID="txtNoLiquidacion" runat="server" CssClass ="textbox validate[custom[positiveNumber]]"></asp:TextBox>
        </div>
      </div>
      <div class="renglon">
        <div class="controlBoton">
          <asp:Button ID="btnBuscarEgreso" runat="server" Text="Buscar" CssClass="boton" OnClick="btnBuscarEgreso_Click"  />
        </div>
      </div>
    </div>
    <div class="columna2x" style="width:50%">
      <div id="contenedorFacturaCxP">
        <div id="instruccionContenedor">Arrastre sus comprobantes de Pago a este recuadro.</div>
      </div>
      <div class="renglon2x" style="padding-top: 10px;">
        <div class="controlBoton">
          <asp:UpdatePanel ID="upbtnImportar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Button ID="btnImportar" runat="server" Text="Importar" TabIndex="1" CssClass="boton" OnClick="btnImportar_Click" />
            </ContentTemplate>
            <Triggers>
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
  <div class="contenedor_controles">
    <div class="renglon4x">
      <div class="control" style="width: auto">
        <asp:Label runat="server" CssClass="etiqueta" Text="Mostrar" ID="lblMostrar" Width="70px"></asp:Label>
      </div>
      <div class="control">
        <asp:DropDownList ID="ddlMostrar" runat="server" CssClass="dropdown" OnSelectedIndexChanged="ddlMostrar_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
      </div>
      <div class="etiqueta"></div>
      <div class="etiqueta">
        <label for="lblOrdenado">Ordenado por: </label>
      </div>
      <div class="etiqueta">
        <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Label ID="lblOrdenado" runat="server" CssClass="label_negrita" Text=""></asp:Label>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnBuscarEgreso" />
            <asp:AsyncPostBackTrigger ControlID="gvEgresos" EventName="Sorting" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="grid_seccion_completa_150px_altura" style="width: calc(100% - 10px); height:auto; box-sizing: border-box; overflow:hidden; ">
      <asp:UpdatePanel ID="upgvEgresos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:GridView ID="gvEgresos" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" ShowFooter="true" PageSize="5" CssClass="gridview" Width="100%" OnSorting="gvEgresos_Sorting" OnPageIndexChanging="gvEgresos_PageIndexChanging">
            <AlternatingRowStyle CssClass="gridviewrowalternate" />
            <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
            <FooterStyle CssClass="gridviewfooter" />
            <HeaderStyle CssClass="gridviewheader" />
            <RowStyle CssClass="gridviewrow" />
            <SelectedRowStyle CssClass="gridviewrowselected" />
            <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
            <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
            <Columns>
              <asp:BoundField DataField="IdTEI" HeaderText="IdTEI" SortExpression="IdTEI" Visible="false" />
              <asp:BoundField DataField="NoEgreso" HeaderText="No. de Egreso" SortExpression="NoEgreso" />
              <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
              <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
              <asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio" />
              <asp:BoundField DataField="IdComprobante" HeaderText="IdComprobante" SortExpression="IdComprobante" Visible="false" />
              <asp:BoundField DataField="ComprobantePago" HeaderText="Comprobante de Pago" SortExpression="ComprobantePago" Visible="false" />
              <asp:BoundField DataField="FormaPago" HeaderText="Forma de Pago" SortExpression="FormaPago" />
              <asp:BoundField DataField="MetodoPago" HeaderText="Método de Pago" SortExpression="MetodoPago" Visible="false"/>
              <asp:BoundField DataField="FechaPago" HeaderText="Fecha de Pago" SortExpression="FechaPago" DataFormatString="{0:dd/MM/yyyy}" />
              <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
              <asp:BoundField DataField="Moneda" HeaderText="Moneda" SortExpression="Moneda" />
              <asp:BoundField DataField="MontoPesos" HeaderText="Monto Pesos" SortExpression="MontoPesos" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="Referencia" HeaderText="Referencia" SortExpression="Referencia" />
              <asp:BoundField DataField="Beneficiario" HeaderText="Beneficiario" SortExpression="Beneficiario" />
              <asp:TemplateField>
                <ItemTemplate>
                  <asp:LinkButton ID="lnkSeleccionarEgreso" runat="server" Text="Seleccionar Egreso" OnClick="lnkSeleccionarEgreso_Click" CommandName="LigaEgresos"></asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </ContentTemplate>
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnBuscarEgreso" />
        </Triggers>
      </asp:UpdatePanel>
    </div>
  </div>

  <!-- Si se reconoce el Proveedor. Abre esta ventana -->
  <div id="contenidoResultadoConsultaSATModal" class="modal">
    <div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba" style="border-radius:10px; left: calc(50% - 305px);
    top: calc(50% - 104px); ">
      <div class="columna2x" style="width:100%; display:grid; justify-content:center;">
        <div class="header_seccion" style="align-items:center; justify-content:center; display:flex; width:100%;">
          <asp:UpdatePanel ID="upheaderValidacionSAT" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
              <h2 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h2>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Emisoror</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Receptor</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblRFCReceptor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblRFCReceptor" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">UUID</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblUUID" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblUUID" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Fecha Expedición</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblFechaExpedicion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblFechaExpedicion" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Total Factura</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblTotalFactura" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblTotalFactura" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon_boton" style="width">
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCancelarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnCancelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Si la primer ventana NO reconoce el Proveedor. Abre esta ventana -->
  <div id="contenedorVentanaConfirmacion" class="modal">
    <div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion" style="width: 480px; border-radius: 10px; top:250px;">
      <div class="columna2x" style="width: 100%; display: grid; justify-content: center;">
        <div class="header_seccion">
          <img src="../Image/Exclamacion.png" />
          <h2>¿Desea dar de alta al proveedor?</h2>
          <asp:UpdatePanel ID="uplblProveedorFactura" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <h2><b>
                <asp:Label ID="lblProveedorFactura" runat="server"></asp:Label></b></h2>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnAceptarAltaProveedor" />
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="renglon">
          <div class="etiqueta">
            <label for="ddlTipoServicio">Tipo de Servicio</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="upddlTipoServicio" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="dropdown"></asp:DropDownList>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptarAltaProveedor" />
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon2x">
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAceptarAltaProveedor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnAceptarAltaProveedor" runat="server" Text="Aceptar" CssClass="boton" OnClick="btnAceptarAltaProveedor_Click" />
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptarAltaProveedor" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCancelarAltaProveedor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnCancelarAltaProveedor" runat="server" Text="Cancelar" CssClass="boton_cancelar" OnClick="btnCancelarAltaProveedor_Click" />
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptarAltaProveedor" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="modal" id="contenedorVentanaLigaEgresos">
    <div class="contenedor_ventana_confirmacion" id="ventanaLigaEgresos"  style="border-radius:10px; left:100px; top: 150px; width: calc(100% - 200px); height: calc(100% - 250px); min-height: 280px; ">
      <div class="boton_cerrar_modal">
          <asp:UpdatePanel ID="uplnkCerrarVentanaLigaEgresos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:LinkButton ID="lnkCerrarVentanaLigaEgresos" runat="server" CommandName="LigaEgresos" OnClick="lnkCerrarVentanaModal_Click">
                <img src = "../Image/Cerrar16.png" />
              </asp:LinkButton>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
      <div class="columna" style="width:28%; display:grid; justify-content:center;">
        <div class="header_seccion" style="align-items:center; justify-content:center; display:flex; width:100%;">
          <img src="../Image/Pagos.png" />
          <h2>Detalles del egreso</h2>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">No. Egreso</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblNoEgreso" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblNoEgreso" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Estatus</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblEstatus" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblEstatus" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Origen</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblOrigen" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblOrigen" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Forma Pago</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblFormaPago" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblFormaPago" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Fecha Pago</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblFechaPago" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblFechaPago" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Monto</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblMonto" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblMonto" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon" style="width:100%">
          <div class="etiqueta">
            <label class="label_negrita">Beneficiario</label>
          </div>
          <div class="control">
            <asp:UpdatePanel ID="uplblBeneficiario" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblBeneficiario" runat="server" Text="" CssClass="label"></asp:Label>
              </ContentTemplate>
              <Triggers><asp:AsyncPostBackTrigger ControlID="gvEgresos"/></Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
      <div class="columna" style="width: 72%; display: block; justify-content: normal; height:94%;">
        <div class="header_seccion" style="align-items: center; justify-content: center; display: flex; width: 99%; ">
          <img src="../Image/Pagos.png" />
          <h2>Pagos disponibles</h2>
        </div>
        <div class="columna" style="display:flex; flex-wrap:wrap; width:100%; justify-content:flex-start; align-items:center; height:88%; " id="contenedorGplnk">
          <div class="grid_seccion_completa" style="margin:0; padding:0; height:95%; width:99%;">
            <asp:UpdatePanel runat="server" ID="upgvFacturas" UpdateMode="Conditional">
              <ContentTemplate>
                <h1>PAGOS</h1>
                <asp:GridView ID="gvPagosEgreso" OnRowDataBound="gvPagosEgreso_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="gridview" AllowPaging="false" AllowSorting="true" PageSize="10" ShowFooter="false" Width="100%" Visible="true">
                  <AlternatingRowStyle CssClass="gridviewrowalternate" />
                  <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                  <FooterStyle CssClass="gridviewfooter" />
                  <HeaderStyle CssClass="gridviewheader" />
                  <RowStyle CssClass="gridviewrow" />
                  <SelectedRowStyle CssClass="gridviewrowselected" />
                  <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                  <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                  <Columns>
                    <asp:BoundField DataField="IdTPF" HeaderText="IdTPF" Visible="false" SortExpression="IdTPF"/>
                    <asp:BoundField DataField="IdTEI" HeaderText="IdTEI" Visible="false" SortExpression="IdTEI"/>
                    <asp:BoundField DataField="IdFPTPF" HeaderText="IdFPTPF" Visible="false" SortExpression="IdFPTPF"/>
                    <asp:BoundField DataField="FechaPagoTPF" HeaderText="Fecha" SortExpression="FechaPagoTPF" Visible="true" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="FormaPagoTPF" HeaderText="Forma de Pago" SortExpression="TipoCambioTPF" Visible="true"/>
                    <asp:BoundField DataField="MontoTPF" HeaderText="Monto" SortExpression="MontoTPF" Visible="true"  DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="MonedaTPF" HeaderText="Moneda" SortExpression="MonedaTPF" Visible="true"/>
                    <asp:BoundField DataField="TipoCambioTPF" HeaderText="T. de Cambio" SortExpression="FechaPagoTPF" Visible="true" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="FechaTimpoCambioTPF" HeaderText="Fecha T. de Cambio" SortExpression="FechaTimpoCambioTPF" Visible="true" DataFormatString="{0:dd/MM/yyyy}"/>
                    <asp:BoundField DataField="NoOperacionTPF" HeaderText="No. Operación" SortExpression="NoOperacionTPF" Visible="true"/>
                    <asp:BoundField DataField="IdBancoTPF" HeaderText="Banco Ext." SortExpression="IdBancoTPF" Visible="false"/>
                    <asp:BoundField DataField="RFCCtaOrdTPF" HeaderText="RFC Ordenante" SortExpression="RFCCtaOrdTPF" Visible="false"/>
                    <asp:BoundField DataField="CtaOrdTPF" HeaderText="Cuenta Ordenante" SortExpression="CtaOrdTPF" Visible="true"/>
                    <asp:BoundField DataField="RFCCtaBenTPF" HeaderText="RFC Beneficiario" SortExpression="RFCCtaBenTPF" Visible="false"/>
                    <asp:BoundField DataField="CtaBenTPF" HeaderText="Cuenta Beneficiario" SortExpression="CtaBenTPF" Visible="true"/>
                    <asp:BoundField DataField="IdCadenaPagoTPF" HeaderText="Cadena de Pago" SortExpression="IdCadenaPagoTPF" Visible="true"/>
                    <asp:BoundField DataField="EstatusPago" HeaderText="Estatus" SortExpression="EstatusPago" Visible="true"/>
                    <asp:TemplateField HeaderText="">
                      <ItemStyle HorizontalAlign="Left"/>
                      <ItemTemplate>
                        <asp:LinkButton ID="lnkAbrirPago" runat="server" Text="Abrir" OnClick="lnkSeleccionarPago_Click" CommandName="Abrir">
                        </asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lnkLigarPago" runat="server" Text="Ligar" OnClick="lnkSeleccionarPago_Click" CommandName="Ligar">
                        </asp:LinkButton>
                      </ItemTemplate>
                    </asp:TemplateField>
                  </Columns>
                </asp:GridView>
                <br />
                <h1>DOCUMENTOS RELACIONADOS</h1>
                <asp:GridView ID="gvDocumentosPago" OnRowDataBound="gvDocumentosPago_RowDataBound" runat="server" AutoGenerateColumns="false" CssClass="gridview" AllowPaging="false" AllowSorting="false" PageSize="5" ShowFooter="false" Width="100%" Visible="true">
                  <AlternatingRowStyle CssClass="gridviewrowalternate" />
                  <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                  <FooterStyle CssClass="gridviewfooter" />
                  <HeaderStyle CssClass="gridviewheader" />
                  <RowStyle CssClass="gridviewrow" />
                  <SelectedRowStyle CssClass="gridviewrowselected" />
                  <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                  <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                  <Columns>
                    <asp:BoundField DataField="IdTDP" HeaderText="IdTDP" Visible="false" SortExpression="IdTDP"/>
                    <asp:BoundField DataField="IdPF" HeaderText="Id" Visible="false" SortExpression="Id"/>
                    <asp:BoundField DataField="UUIDTDP" HeaderText="UUID" SortExpression="UUIDTDP" Visible="true"/>
                    <asp:BoundField DataField="SerieTDP" HeaderText="Serie" SortExpression="SerieTDP" Visible="true"/>
                    <asp:BoundField DataField="FolioTDP" HeaderText="Folio" SortExpression="FolioTDP" Visible="true"/>
                    <asp:BoundField DataField="NoParcialidadTDP" HeaderText="No. Parcialidad" SortExpression="NoParcialidadTDP" Visible="true"/>
                    <asp:BoundField DataField="ImporteSalAntTDP" HeaderText="Saldo Ant." SortExpression="ImporteSalAntTDP" Visible="true"  DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="ImportePagTDP" HeaderText="Saldo Pagado" SortExpression="ImportePagTDP" Visible="true"  DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="ImporteSalInsTDP" HeaderText="Saldo Ins." SortExpression="ImporteSalInsTDP" Visible="true"  DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="IdMonedaTDP" HeaderText="Moneda" SortExpression="IdMonedaTDP" Visible="true"/>
                    <asp:BoundField DataField="TipoCambioTDP" HeaderText="T. de Cambio" SortExpression="TipoCambioTDP" Visible="true"  DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"/>
                    <asp:BoundField DataField="MPagoTDP" HeaderText="M. de Pago" SortExpression="MPagoTDP" Visible="true"/>
                    <asp:BoundField DataField="Existente" HeaderText="" SortExpression="Existente" Visible="true" ItemStyle-ForeColor="Red" ControlStyle-ForeColor="Red"/>
                    
                    
                  </Columns>
                </asp:GridView>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvEgresos" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
