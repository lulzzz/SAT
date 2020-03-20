<%@ Page Title="Facturado" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true" CodeBehind="Facturado.aspx.cs" Inherits="SAT.CuentasPagar.Facturado" %>

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
    #contenedorFacturaXML {
      margin-left: 15px;
      height: 150px;
      width: 95%;
      border: dashed 3px;
      border-color: #a5d16f;
      background-color: #f8f8f8;
    }

    #nombreContenedor {
      margin-left: 10%;
      width: 80%;
      margin-top: 12%;
      font-size: large;
      font-weight: bold;
      text-align: center;
      height: auto;
      color: #808080
    }
  </style>
  <!-- Bibliotecas para Validación de formulario -->
  <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
  <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
  <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.js") %>' type="text/javascript"></script>
  <script src='<%=ResolveUrl("~/Scripts/jQuery.FileDrop.min.js") %>' type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
  <script type="text/javascript">
    //Obteniendo instancia actual de la página y añadiendo manejador de evento
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
    function EndRequestHandler(sender, args) {
      if (args.get_error() == undefined) {
        ConfiguraFacturado();
      }
    }
    //Declarando Método de Configuración
    function ConfiguraFacturado() {
      $(document).ready(function () {
        //Creando Método de validación de Controles
        var validaFacturado = function (evt) {
          //Obteniendo resultado de Validación de Controles
          var isValid1 = !$("#<%=txtCompania.ClientID%>").validationEngine('validate');
                  var isValid2 = !$("#<%=txtProveedor.ClientID%>").validationEngine('validate');
                  var isValid3 = !$("#<%=txtUUID.ClientID%>").validationEngine('validate');
                  var isValid4 = !$("#<%=txtFechaFactura.ClientID%>").validationEngine('validate');
                  var isValid5 = !$("#<%=txtMontoTC.ClientID%>").validationEngine('validate');
                  var isValid6 = !$("#<%=txtFechaTC.ClientID%>").validationEngine('validate');
                  var isValid7 = !$("#<%=txtTotal.ClientID%>").validationEngine('validate');
                  var isValid8 = !$("#<%=txtSubTotal.ClientID%>").validationEngine('validate');
                  var isValid9 = !$("#<%=txtDescuento.ClientID%>").validationEngine('validate');
                  var isValid10 = !$("#<%=txtTrasladado.ClientID%>").validationEngine('validate');
                  var isValid11 = !$("#<%=txtRetenido.ClientID%>").validationEngine('validate');
                  var isValid12 = !$("#<%=txtSaldo.ClientID%>").validationEngine('validate');
                  var isValid13 = !$("#<%=txtDiasCredito.ClientID%>").validationEngine('validate');
                //Limpiando Contenedor del XML
                BorraNombreArchivoCargado();
                //Devolviendo resultado Obtenido
                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7 && isValid8 && isValid9 && isValid10 && isValid11 && isValid12 && isValid13;
              }

              var validaConceptoFactura = function (evt) {
                //Obteniendo resultado de la Validación
                var isValid1 = !$("#<%=txtCantidadFacturaConcepto.ClientID%>").validationEngine('validate');
              var isValid2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
              var isValid3 = !$("#<%=txtIdentificadorFacturaConcepto.ClientID%>").validationEngine('validate');
              var isValid4 = !$("#<%=txtConceptoCobro.ClientID%>").validationEngine('validate');
              var isValid5 = !$("#<%=txtValorUniFacturaConcepto.ClientID%>").validationEngine('validate');
              var isValid6 = !$("#<%=txtTasaImpRetFacturaConcepto.ClientID%>").validationEngine('validate');
              var isValid7 = !$("#<%=txtTasaImpTraFacturaConcepto.ClientID%>").validationEngine('validate');

                return isValid1 && isValid2 && isValid3 && isValid4 && isValid5 && isValid6 && isValid7;
              }
              //Cargando Proveedor
              $("#<%=txtProveedor.ClientID%>").autocomplete({
                source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=TSDK.Base.Cadena.RegresaCadenaSeparada(txtCompania.Text, "ID:", 1)%>'
              });
              //Añadiendo Función al Evento del Control
              $("#<%=btnAceptar.ClientID%>").click(validaFacturado);
              //Añadiendo Función al Evento del Control
              $("#<%=lkbGuardar.ClientID%>").click(validaFacturado);

              /** Función de fechas **/
              $("#<%=txtFechaFactura.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
              });
              $("#<%=txtFechaTC.ClientID%>").datetimepicker({
                lang: 'es',
                format: 'd/m/Y H:i'
              });

              //Añadiendo Función al Evento del Control
              $("#<%=btnAceptarFacturaConcepto.ClientID%>").click(validaConceptoFactura);
              //Calcular campo de Importe al cambiar el Valor de la Cantidad
              $("#<%=txtCantidadFacturaConcepto.ClientID%>").change(function () {
                $("#<%=txtImporteFacturaConcepto.ClientID%>").val(parseFloat($("#<%=txtValorUniFacturaConcepto.ClientID%>").val()) * parseFloat($("#<%=txtCantidadFacturaConcepto.ClientID%>").val()));
              });
              //Calcular campo de Importe al cambiar el Valor Unitario
              $("#<%=txtValorUniFacturaConcepto.ClientID%>").change(function () {
                $("#<%=txtImporteFacturaConcepto.ClientID%>").val(parseFloat($("#<%=txtValorUniFacturaConcepto.ClientID%>").val()) * parseFloat($("#<%=txtCantidadFacturaConcepto.ClientID%>").val()));
              });
              //Limpiando Contenedor XML
              $("#<%=lkbNuevo.ClientID%>").click(function () {
                BorraNombreArchivoCargado();
              });
              //Limpiando Contenedor XML
              $("#<%=btnAceptarValidacion.ClientID%>").click(function () {
                BorraNombreArchivoCargado();
              });

              //Quitando cualquier manejador de evento click añadido previamente
              $("#<%= btnCerrar.ClientID%>").unbind("click");
              $("#<%= btnCerrar.ClientID%>").click(function (evt) {
          evt.preventDefault()
          //Ocultando ventana modal 
          $("#contenido_concepto_modal").animate({ width: "toggle" });
          $("#concepto_modal").animate({ width: "toggle" });
        });

        //Validando que el Navegador Soporte
        if ($.support.fileDrop) {
          $("#contenedorFacturaXML").fileDrop({
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
    //Invocando Método de Configuración
    ConfiguraFacturado();

    //Función de Guardado
    function GuardaFacturaProveedor(data, name, type) {

      //Construyendo Parametros del Valor de Datos
      var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";

      //Definiendo Consumo
      $.ajax({
        type: "POST",
        url: "Facturado.aspx/LecturaArchivo",
        data: dataValue,
        contentType: 'application/json',
        success: function (response) {
          //Cambiando nombre de archivo cargado
          CambiaNombreArchivoCargado(response.d);
          //Indicando carga correcta
          //alert(response.d);
        }
      });
    }
    //Función para actualizar el nombre del archivo cargado
    function CambiaNombreArchivoCargado(nombre) {
      $('#nombreContenedor').text(nombre);
    };
    function BorraNombreArchivoCargado() {
      $('#nombreContenedor').text('Arrastre y suelte sus archivos XML a este cuadro.');
    };

  </script>
  <div id="encabezado_forma">
    <img src="../Image/FacturacionCargos.png" />
    <h1>Facturas Proveedor</h1>
  </div>
  <asp:UpdatePanel ID="upMenuPrincipal" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
      <nav id="menuForma">
        <ul>
          <li class="green">
            <a href="#" class="fa fa-floppy-o"></a>
            <ul>
              <li>
                <asp:LinkButton ID="lkbNuevo" runat="server" Text="Nuevo" OnClick="lkbElementoMenu_Click" CommandName="Nuevo" /></li>
              <li>
                <asp:LinkButton ID="lkbAbrir" runat="server" Text="Abrir" OnClick="lkbElementoMenu_Click" CommandName="Abrir" /></li>
              <li>
                <asp:LinkButton ID="lkbGuardar" runat="server" Text="Guardar" OnClick="lkbElementoMenu_Click" CommandName="Guardar" /></li>
              <li>
                <asp:LinkButton ID="lkbSalir" runat="server" Text="Salir" OnClick="lkbElementoMenu_Click" CommandName="Salir" /></li>
            </ul>
          </li>
          <li class="red">
            <a href="#" class="fa fa-pencil-square-o"></a>
            <ul>
              <li>
                <asp:LinkButton ID="lkbEditar" runat="server" Text="Editar" OnClick="lkbElementoMenu_Click" CommandName="Editar" /></li>
              <li>
                <asp:LinkButton ID="lkbEliminar" runat="server" Text="Eliminar" OnClick="lkbElementoMenu_Click" CommandName="Eliminar" /></li>
              <li>
                <asp:LinkButton ID="lkbActualizar" runat="server" Text="Actualizar" OnClick="lkbElementoMenu_Click" CommandName="Actualizar" /></li>
              <li>
                <asp:LinkButton ID="lkbAceptar" runat="server" Text="Aceptar Factura" OnClick="lkbElementoMenu_Click" CommandName="Aceptar" /></li>
            </ul>
          </li>
          <li class="blue">
            <a href="#" class="fa fa-cog"></a>
            <ul>
              <li>
                <asp:LinkButton ID="lkbBitacora" runat="server" Text="Bitácora" OnClick="lkbElementoMenu_Click" CommandName="Bitacora" /></li>
              <li>
                <asp:LinkButton ID="lkbReferencias" runat="server" Text="Referencias" OnClick="lkbElementoMenu_Click" CommandName="Referencias" /></li>
              <li>
                <asp:LinkButton ID="lkbArchivos" runat="server" Text="Archivos" OnClick="lkbElementoMenu_Click" CommandName="Archivos" /></li>
              <li>
                <asp:LinkButton ID="lkbRefacturacion" runat="server" Text="Refacturación" OnClick="lkbElementoMenu_Click" CommandName="Refacturacion" /></li>
            </ul>
          </li>
          <li class="yellow">
            <a href="#" class="fa fa-question-circle"></a>
            <ul>
              <li>
                <asp:LinkButton ID="lkbAcercaDe" runat="server" Text="Acerca de" OnClick="lkbElementoMenu_Click" CommandName="Acerca" /></li>
              <li>
                <asp:LinkButton ID="lkbAyuda" runat="server" Text="Ayuda" OnClick="lkbElementoMenu_Click" CommandName="Ayuda" /></li>
            </ul>
          </li>
        </ul>
      </nav>
    </ContentTemplate>
    <Triggers>
      <asp:AsyncPostBackTrigger ControlID="btnImportar" />
      <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
      <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
      <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
      <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
      <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
      <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
      <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
      <asp:PostBackTrigger ControlID="lkbBitacora" />
      <asp:PostBackTrigger ControlID="lkbReferencias" />
      <asp:PostBackTrigger ControlID="lkbArchivos" />
    </Triggers>
  </asp:UpdatePanel>
  <div class="seccion_controles">
    <div class="header_seccion">
      <img src="../Image/AnalisisDoc.png" />
      <h2>Datos Factura</h2>
    </div>
    <div class="columna2x">
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtCompania">ID</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Label ID="lblId" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
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
              <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown2x" Enabled="false"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtCompania">Compañia</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtCompania" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtCompania" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"
                TabIndex="1" Enabled="false"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="ddlTipoFactura">Tipo de Factura</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="upddlTipoFactura" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:DropDownList ID="ddlTipoFactura" runat="server" CssClass="dropdown2x" TabIndex="3"></asp:DropDownList>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtProveedor">Proveedor</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtProveedor" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtProveedor" runat="server" CssClass="textbox2x validate[required, custom[IdCatalogo]]"
                TabIndex="2"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtFechaFactura">Fecha de Factura</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uptxtFechaFactura" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtFechaFactura" runat="server" CssClass="textbox2x validate[required, custom[dateTime24]]"
                TabIndex="8" MaxLength="16"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtSerie">Serie</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uptxtSerie" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox2x validate[required]" TabIndex="5" MaxLength="10"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtFolio">Folio</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtFolio" runat="server" CssClass="textbox2x validate[required, custom[positiveNumber]]"
                TabIndex="6" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtUUID">UUID</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtUUID" runat="server" CssClass="textbox2x validate[required]" TabIndex="7" MaxLength="36"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon2x">
        <div class="etiqueta">
          <label for="txtCondPago">Condición de Pago</label>
        </div>
        <div class="control2x">
          <asp:UpdatePanel ID="uptxtCondPago" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtCondPago" runat="server" CssClass="textbox2x validate[required]" TabIndex="14"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
    <div class="columna">
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtSubTotal">Subtotal</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtSubTotal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtSubTotal" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="12" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtDescuento">Descuento</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtDescuento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtDescuento" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="13" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtTrasladado">Trasladado</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtTrasladado" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtTrasladado" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="15" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtRetenido">Retenido</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtRetenido" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtRetenido" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="16" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtTotal">Total</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtTotal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtTotal" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="11" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtSaldo">Saldo</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtSaldo" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtSaldo" runat="server" CssClass="textbox validate[required, custom[number]]"
                TabIndex="17" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtMoneda">Moneda</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtMoneda" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtMoneda" runat="server" CssClass="textbox validate[required]" TabIndex="4"
                MaxLength="10"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtMontoTC">Tipo de Cambio</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtMontoTC" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtMontoTC" runat="server" CssClass="textbox validate[required]" TabIndex="9" MaxLength="9"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtFechaTC">Fecha T.C.</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtFechaTC" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtFechaTC" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="10"
                MaxLength="16"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="etiqueta">
          <label for="txtDiasCredito">Dias de Credito</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uptxtDiasCredito" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:TextBox ID="txtDiasCredito" runat="server" CssClass="textbox validate[required]" TabIndex="18"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <div class="controlBoton">
          <asp:UpdatePanel ID="upbtnAceptar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Button ID="btnAceptar" runat="server" CssClass="boton" Text="Aceptar" TabIndex="19"
                OnClick="btnAceptar_Click" />
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
            <Triggers>
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="controlBoton">
          <asp:UpdatePanel ID="upbtnCancelar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Button ID="btnCancelar" runat="server" CssClass="boton_cancelar" Text="Cancelar" TabIndex="20"
                OnClick="btnCancelar_Click" />
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="renglon">
        <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Label ID="lblError" runat="server" CssClass="label_error"></asp:Label>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="columna" style="width: 400px">
      <div id="contenedorFacturaXML">
        <div id="nombreContenedor">Arrastre y suelte sus archivos XML a este cuadro.</div>
      </div>
      <div class="renglon">
        <div class="controlBoton">
          <asp:UpdatePanel ID="upbtnImportar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Button ID="btnImportar" runat="server" Text="Importar Factura" TabIndex="34" CssClass="boton"
                OnClick="btnImportar_Click" />
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
    </div>
  </div>
  <div class="contenedor_controles">
    <div class="renglon_pestaña_documentacion">
      <h2>Conceptos Factura</h2>
    </div>
    <div class="renglon_pestaña_documentacion">
      <div class="etiqueta_50px">
        <label for="txtCantidadFacturaConcepto">Cantidad</label>
      </div>
      <div class="etiqueta">
        <label for="ddlUnidadFacturaConcepto">Unidad</label>
      </div>
      <div class="etiqueta_155px">
        <label for="txtIdentificadorFacturaConcepto">Identificador</label>
      </div>
      <div class="etiqueta_155px">
        <label for="ddlConceptoCobroFacturaConcepto">Concepto Cobro</label>
      </div>
      <div class="etiqueta">
        <label for="txtValorUniFacturaConcepto">Valor Unitario</label>
      </div>
      <div class="etiqueta">
        <label for="txtImporteFacturaConcepto">Importe</label>
      </div>
      <div class="etiqueta_50px">
        <label for="txtTasaImpTraFacturaConcepto">Tasa IVA</label>
      </div>
      <div class="etiqueta_50px">
        <label for="txtTasaImpRetFacturaConcepto">Tasa Ret</label>
      </div>

    </div>
    <div class="renglon_pestaña_documentacion">
      <div class="control_60px">
        <asp:UpdatePanel ID="uptxtCantidadFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtCantidadFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]"
              MaxLength="9" TabIndex="20"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control_100px">
        <asp:UpdatePanel ID="upddlUnidadFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox_50px validate[required]"
              MaxLength="50" TabIndex="21"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control">
        <asp:UpdatePanel ID="uptxtIdentificadorFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtIdentificadorFacturaConcepto" runat="server" CssClass="textbox validate[required]"
              MaxLength="50" TabIndex="22"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control">
        <asp:UpdatePanel ID="upddlConceptoCobroFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtConceptoCobro" runat="server" CssClass="textbox validate[required]" TabIndex="23" MaxLength="150"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control_100px">
        <asp:UpdatePanel ID="uptxtValorUniFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtValorUniFacturaConcepto" runat="server" CssClass="textbox_100px validate[required, custom[positiveNumber]]"
              MaxLength="9" TabIndex="24"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control_100px">
        <asp:UpdatePanel ID="uptxtImporteFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtImporteFacturaConcepto" runat="server" CssClass="textbox_100px" Enabled="false" TabIndex="25"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="txtValorUniFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control_60px">
        <asp:UpdatePanel ID="uptxtTasaImpTraFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtTasaImpTraFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]"
              MaxLength="9" TabIndex="26"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control_60px">
        <asp:UpdatePanel ID="uptxtTasaImpRetFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:TextBox ID="txtTasaImpRetFacturaConcepto" runat="server" CssClass="textbox_50px validate[required, custom[positiveNumber]]"
              MaxLength="9" TabIndex="27"></asp:TextBox>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="etiqueta">
        <asp:UpdatePanel ID="upbtnAceptarFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Button ID="btnAceptarFacturaConcepto" runat="server" OnClick="btnAceptarFacturaConcepto_Click"
              CssClass="boton" Text="Actualizar" TabIndex="28" />
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="etiqueta">
        <asp:UpdatePanel ID="upbtnCancelarFacturaConcepto" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Button ID="btnCancelarFacturaConcepto" runat="server" OnClick="btnCancelarFacturaConcepto_Click"
              CssClass="boton_cancelar" Text="Cancelar" TabIndex="29" />
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="renglon_pestaña_documentacion">
      <asp:UpdatePanel ID="uplblErrorConcepto" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:Label ID="lblErrorConcepto" runat="server" CssClass="label_error" Width="300px"></asp:Label>
        </ContentTemplate>
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnImportar" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
          <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
          <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
          <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
          <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
          <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
          <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
        </Triggers>
      </asp:UpdatePanel>
    </div>
    <div class="renglon_pestaña_documentacion">
      <div class="control2x">
        <asp:UpdatePanel ID="upddlTamano" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <label for="ddlTamano">Mostrar:</label>
            <asp:DropDownList ID="ddlTamano" runat="server" CssClass="dropdown" AutoPostBack="true"
              OnSelectedIndexChanged="ddlTamano_SelectedIndexChanged" TabIndex="30">
            </asp:DropDownList>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="etiqueta">
        <label for="lblOrdenado">Ordenado por:</label>
      </div>
      <div class="etiqueta">
        <asp:UpdatePanel ID="uplblOrdenado" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:Label ID="lblOrdenado" runat="server"></asp:Label>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="gvConceptosFacturaConcepto" EventName="Sorting" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="control2xr">
        <asp:UpdatePanel ID="uplnkExportarExcel" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:LinkButton ID="lnkExportarExcel" runat="server" Text="Exportar" TabIndex="32"
              OnClick="lnkExportarExcel_Click"></asp:LinkButton>
          </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportarExcel" />
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
    </div>
    <div class="grid_seccion_completa_150px_altura">
      <asp:UpdatePanel ID="upgvConceptosFacturaConcepto" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:GridView ID="gvConceptosFacturaConcepto" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            OnSorting="gvConceptosFacturaConcepto_Sorting" OnPageIndexChanging="gvConceptosFacturaConcepto_PageIndexChanging" ShowFooter="True"
            PageSize="5" Width="100%" TabIndex="33">
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
              <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
              <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
              <asp:BoundField DataField="ConceptoCobro" HeaderText="Concepto de Cobro" SortExpression="ConceptoCobro" />
              <asp:BoundField DataField="ValorUnitario" HeaderText="Valor Unitario" SortExpression="ValorUnitario" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="ImportePesos" HeaderText="Importe MXN" SortExpression="ImportePesos" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="TasaImpuestoRetenido" HeaderText="Tasa Ret." SortExpression="TasaImpuestoRetenido" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="ImporteRetenido" HeaderText="Retenido" SortExpression="ImporteRetenido" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="TasaImpuestoTrasladado" HeaderText="Tasa IVA" SortExpression="TasaImpuestoTrasladado" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="ImporteTrasladado" HeaderText="IVA" SortExpression="ImporteTrasladado" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
              <asp:TemplateField>
                <ItemTemplate>
                  <asp:LinkButton ID="lnkEditarFacturaConcepto" runat="server" Text="Editar" OnClick="lnkEditarFacturaConcepto_Click"></asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField>
                <ItemTemplate>
                  <asp:LinkButton ID="lnkEliminarFacturaConcepto" runat="server" Text="Eliminar" OnClick="lnkEliminarFacturaConcepto_Click"></asp:LinkButton>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
          </asp:GridView>
        </ContentTemplate>
        <Triggers>
          <asp:AsyncPostBackTrigger ControlID="btnImportar" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
          <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
          <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
          <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
          <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
          <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
          <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
          <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
          <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
        </Triggers>
      </asp:UpdatePanel>
    </div>

  </div>

  <div id="contenido_concepto_modal" class="modal">
    <div id="concepto_modal" class="resumen_facturado_concepto">
      <div class="renglon3x">
        <div class="etiqueta_50px">
          <label>Mostrar:</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="upddlTamanoGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:DropDownList ID="ddlTamanoGrid" CssClass="dropdown" runat="server" TabIndex="35" AutoPostBack="true"
                OnSelectedIndexChanged="ddlTamanoGrid_SelectedIndexChanged">
              </asp:DropDownList>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <div class="etiqueta_50px">
          <label>Ordenado:</label>
        </div>
        <div class="control">
          <asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="gvConceptos" EventName="Sorting" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="controlr">
          <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <asp:LinkButton ID="lnkExportar" runat="server" TabIndex="36" OnClick="lnkExportar_Click">Exportar</asp:LinkButton>
            </ContentTemplate>
            <Triggers>
              <asp:PostBackTrigger ControlID="lnkExportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
              <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
              <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
              <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
              <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
              <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
              <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
              <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
      </div>
      <div class="grid_concepto">
        <asp:UpdatePanel ID="upgvConceptos" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <asp:GridView ID="gvConceptos" runat="server" AllowPaging="true" AllowSorting="true"
              CssClass="gridview" OnSorting="gvConceptos_Sorting" OnPageIndexChanging="gvConceptos_PageIndexChanging"
              TabIndex="37" AutoGenerateColumns="false" Width="90%" PageSize="5">
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
                  <HeaderStyle HorizontalAlign="Center" />
                  <ItemStyle HorizontalAlign="Center" />
                  <HeaderTemplate>
                    <asp:CheckBox ID="chkTodos" runat="server"
                      OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                  </HeaderTemplate>
                  <ItemTemplate>
                    <asp:CheckBox ID="chkVarios" runat="server"
                      OnCheckedChanged="chkTodos_CheckedChanged" AutoPostBack="true" />
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ValorUnitario" HeaderText="Valor Unitario" SortExpression="ValorUnitario" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                <asp:BoundField DataField="Identificador" HeaderText="Identificador" SortExpression="Identificador" />
                <asp:BoundField DataField="ConceptoCobro" HeaderText="Concepto de Cobro" SortExpression="ConceptoCobro" />
                <asp:BoundField DataField="Importe" HeaderText="Importe" SortExpression="Importe" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ImportePesos" HeaderText="Importe MXN" SortExpression="ImportePesos" DataFormatString="{0:#,###,###,###.00}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Clasificacion" HeaderText="Clasificacion" SortExpression="Clasificacion" />
              </Columns>
            </asp:GridView>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
            <asp:AsyncPostBackTrigger ControlID="ddlTamanoGrid" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
            <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
            <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
            <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
            <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
            <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
            <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
            <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
          </Triggers>
        </asp:UpdatePanel>
      </div>
      <div class="contenedor_controles_facturado">
        <div class="columna2x">
          <div class="renglon">
            <div class="etiqueta">
              <label for="ddlTipoServ">Tipo de Servicio</label>
            </div>
            <div class="control">
              <asp:UpdatePanel ID="upddlTipoServ" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:DropDownList ID="ddlTipoServ" runat="server" CssClass="dropdown" TabIndex="38"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
          <div class="renglon">
            <div class="etiqueta">
              <label for="ddlSegNeg">Seg. de Negocio</label>
            </div>
            <div class="control">
              <asp:UpdatePanel ID="upddlSegNeg" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:DropDownList ID="ddlSegNeg" runat="server" CssClass="dropdown" TabIndex="39"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
          <div class="renglon">
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnAceptarFactura" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnAceptarFactura" runat="server" Text="Aceptar" TabIndex="40"
                    OnClick="btnAceptarFactura_Click" CssClass="boton" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnCancelarFactura" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnCancelarFactura" runat="server" Text="Cancelar" TabIndex="41"
                    OnClick="btnCancelarFactura_Click" CssClass="boton_cancelar" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnRechazarFactura" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnRechazarFactura" runat="server" Text="Rechazar" TabIndex="42"
                    OnClick="btnRechazarFactura_Click" CssClass="boton" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
          <div class="renglon">
            <asp:UpdatePanel ID="uplblErrorFactura" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblErrorFactura" runat="server" CssClass="label_error"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarFactura" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarFactura" />
                <asp:AsyncPostBackTrigger ControlID="btnRechazarFactura" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="columna">
          <div class="renglon">
            <div class="etiqueta">
              <label for="ddlClasifCont">Clasif. Contable</label>
            </div>
            <div class="control">
              <asp:UpdatePanel ID="upddlClasifCont" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:DropDownList ID="ddlClasifCont" runat="server" CssClass="dropdown" TabIndex="43"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
          <div class="renglon">
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnAceptarConcepto" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnAceptarConcepto" runat="server" Text="Aceptar" CssClass="boton" TabIndex="44"
                    OnClick="btnAceptarConcepto_Click" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelarFacturaConcepto" />
                  <asp:AsyncPostBackTrigger ControlID="ddlTamano" />
                  <asp:AsyncPostBackTrigger ControlID="btnAceptar" />
                  <asp:AsyncPostBackTrigger ControlID="btnCancelar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbGuardar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbNuevo" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEditar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbEliminar" />
                  <asp:AsyncPostBackTrigger ControlID="lkbRefacturacion" />
                  <asp:AsyncPostBackTrigger ControlID="lkbAceptar" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnCerrar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" CssClass="boton_cancelar" TabIndex="45" />
                </ContentTemplate>
                <Triggers></Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- -->
  <div id="contenedorVentanaConfirmacion" class="modal">
    <div id="ventanaConfirmacion" class="contenedor_ventana_confirmacion">
      <div class="header_seccion">
        <img src="../Image/Exclamacion.png" />
        <h2>¿Desea dar de alta al Proveedor </h2>
        <asp:UpdatePanel ID="uplblProveedorFactura" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <h2><b>
              <asp:Label ID="lblProveedorFactura" runat="server"></asp:Label></b></h2>
          </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
            <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
          </Triggers>
        </asp:UpdatePanel>
        <h2>?</h2>
      </div>
      <div class="contenedor_media_seccion">
        <div class="columna">
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
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
          <div class="renglon">
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnAceptarOperacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnAceptarOperacion" runat="server" Text="Aceptar" CssClass="boton"
                    OnClick="btnAceptarOperacion_Click" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
              <asp:UpdatePanel ID="upbtnCancelarOperacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                  <asp:Button ID="btnCancelarOperacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
                    OnClick="btnCancelarOperacion_Click" />
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
                </Triggers>
              </asp:UpdatePanel>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Ventana Confirmación Resultado Consulta SAT -->
  <div id="contenidoResultadoConsultaSATModal" class="modal">
    <div id="contenidoResultadoConsultaSAT" class="contenedor_ventana_confirmacion_arriba">
      <div class="columna2x">
        <div class="header_seccion">
          <asp:UpdatePanel ID="upheaderValidacionSAT" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
              <img id="imgValidacionSAT" runat="server" src="../Image/Exclamacion.png" />
              <h3 id="headerValidacionSAT" runat="server">Resultado de Validación Servidores SAT</h3>
            </ContentTemplate>
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btnImportar" />
              <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
            </Triggers>
          </asp:UpdatePanel>
        </div>
        <div class="renglon2x">
          <div class="etiqueta">
            <label for="">Emisor</label>
          </div>
          <div class="etiqueta_320px">
            <asp:UpdatePanel ID="uplblRFCEmisor" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Label ID="lblRFCEmisor" runat="server" CssClass="label_negrita"></asp:Label>
              </ContentTemplate>
              <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnImportar" />
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
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
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
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
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
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
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
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
                <asp:AsyncPostBackTrigger ControlID="btnAceptarOperacion" />
              </Triggers>
            </asp:UpdatePanel>
          </div>
        </div>
        <div class="renglon_boton">
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnCanelarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnCanelarValidacion" runat="server" Text="Descartar" CssClass="boton_cancelar" CommandName="Descartar" OnClick="btnValidacionSAT_Click" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
          <div class="controlBoton">
            <asp:UpdatePanel ID="upbtnAceptarValidacion" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                <asp:Button ID="btnAceptarValidacion" runat="server" Text="Continuar" CssClass="boton" OnClick="btnValidacionSAT_Click" CommandName="Continuar" />
              </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
