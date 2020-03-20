<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterPage.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="SustitucionFacturaCxP.aspx.cs"
    Inherits="SAT.Soporte.SustitucionFacturaCxP" %>
<%@ Register Src="~/UserControls/wucSoporteTecnico.ascx" TagPrefix="tectos" TagName="wucSoporteTecnico" %>
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

<link href="../CSS/jquery.multiselect.css" rel="stylesheet" type="text/css" />
        <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
       
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.multiselect.js"></script>

    <!-- Biblioteca para uso de datetime picker -->

    
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
            text-align:center; 
            height: auto; 
            color: #808080
        }
    </style>



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
                ConfiguraSustitucionFacturaCXP();
            }
        }
        //Declarando Función de Configuración
        function ConfiguraSustitucionFacturaCXP() {
               
            $(document).ready(function () {

                Encabezado();

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

       
                //Cargando Catalogo de Autocompletado
                $("#<%=txtCliente.ClientID%>").autocomplete({
                    source: '../WebHandlers/AutoCompleta.ashx?id=14&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor%>',
                });





                //Declarando Función de Validación de Busqueda
                var validaBusquedaFacturas = function () {
                    //Añadiendo Validador
                    var isValid1 = !$("#<%=txtCliente.ClientID%>").validationEngine('validate');
                    var isValid2 = !$("#<%=txtFolio.ClientID%>").validationEngine('validate');
                    
                  
                    //Devolviendo Resultado Obtenido
                    return isValid1 && isValid2;
                }


         



                  //Añadiendo Función de Validación al Evento Click del Boton
                $("#<%=btnBuscar.ClientID%>").click(validaBusquedaFacturas);
                 //Listado de clasificacion
                $("#<%=lbxEstatusClasificacion.ClientID%>").multiselect({
                    selectedList: 1,
                    selectall: 1
                });

      
            });
         
            
            $(document).keyup(function (e) {
                if (e.keyCode == 27) { // escape key maps to keycode `27`
                    //Ocultando Menu
                    OcultarMenu();
                }
            });
            $(document).click(function (e) {

                //Ocultando Menu
                OcultarMenu();
            });
        }
    
         //Invocando Función de Configuración
        ConfiguraSustitucionFacturaCXP();
        
    //Función encargada de Mostrar el Ménu
        function MostrarMenu(control, e) {
            //Ocultando en caso de estar Abierto
            OcultarMenu();

            //Obteniendo Coordenadas de las Forma
            var posx = e.pageX + 'px';
            var posy = e.pageY + 'px';

            //Si el Evento es de Tipo Click
            if (e.type == 'click')

                //Detener Propagación del Evento
                e.stopPropagation();

            //Asignando Posiciones al Documento
            document.getElementById(control).style.position = 'absolute';
            document.getElementById(control).style.left = posx;
            document.getElementById(control).style.top = posy;


            //Ejecutando 
            $(document).ready(function (evt) {

                //Mostrando DIV
                $('#' + control).slideDown(100);
            });
        }
        //Función encargada de Ocultar el Ménu
        function OcultarMenu() {
            $(document).ready(function () {
                //Ocultando DIV
                $('.menuContainer').slideUp(100);
            });
        }



       //Función de Guardado
    function GuardaFacturaProveedor(data, name, type) {

        //Construyendo Parametros del Valor de Datos
        var dataValue = "{ 'archivoBase64' : '" + data + "', 'nombreArchivo' : '" + name + "', 'mimeType' : '" + type + "' }";

        //Definiendo Consumo
        $.ajax({
            type: "POST",
            url: "SustitucionFacturaCxP.aspx/LecturaArchivo",
            data: dataValue,
            contentType: 'application/json',
            success: function (response) {
                //Cambiando nombre de archivo cargado
                CambiaNombreArchivoCargado(response.d);
                //Indicando carga correcta
                alert(response.d);
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

        function Encabezado(){

            $("#<%=gvFacturas.ClientID%>").gridviewScroll({
                width: document.getElementById("contenedorFacturaCXP").offsetWidth - 15,
                height: 400,
                with: 400
            });
        };

       
    </script>




    <div id="encabezado_forma">
         <img src="../Image/CXPI.png" />
        <h1>Sustitucion De Facturas Por Pagar</h1>
    </div>

  
    <div class="contenedor_controles">
    <div class="columna2x">
            <asp:Panel ID="pnlBusqueda" runat="server" DefaultButton="btnBuscar">
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtCliente">Proveedor</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtCliente" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCliente" runat="server" TabIndex="1" CssClass="textbox2x validate[required, custom[IdCatalogo]]"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon4x">
                    <div class="etiqueta">
                        <label for="txtFolio">Folio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFolio" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolio" runat="server" TabIndex="2" CssClass="textbox_50px validate[custom[onlyNumberSp]]" MaxLength="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
 
                    <div class="etiqueta_80px">
                  <label for="txtSerie">Serie</label>
                </div>
                <div class="control">
                <asp:TextBox ID="txtSerie" runat="server" CssClass="textbox" MaxLength="10" TabIndex="3"></asp:TextBox>
                </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUUID">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUID" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUID" runat="server" TabIndex="4" CssClass="textbox2x" MaxLength="36"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
                <div class="etiqueta">
                <label for="ddlClasificacion">Estatus</label>
                </div>
                <div class="control2x">
                <asp:UpdatePanel ID="uplbxClasificacion" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:ListBox runat="server" ID="lbxEstatusClasificacion" SelectionMode="Single" TabIndex="3" />                 
                </ContentTemplate>
                </asp:UpdatePanel>
                </div>

                </div>
                <div class="renglon2x">
     
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" TabIndex="5" CssClass="boton"
                                    OnClick="btnBuscar_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <img src="../Image/cfdiXML.png" />
            <h2>Facturas</h2>
        </div>
        <div class="renglon3x">
            <div class="etiqueta">
                <label for="ddlTamanoFacturas">Mostrar</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlTamanoFacturas" runat="server" CssClass="dropdown" TabIndex="5" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlTamanoFacturas_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>     
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenadoFacturas">Ordenado</label>
            </div>
            <div class="control">
                <asp:UpdatePanel ID="uplblOrdenadoFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>
                            <asp:Label ID="lblOrdenadoFacturas" runat="server"></asp:Label></b>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvFacturas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel ID="uplnkExportarFacturas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkExportarFacturas" runat="server" Text="Exportar" TabIndex="6" OnClick="lnkExportarFacturas_Click"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lnkExportarFacturas" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="grid_seccion_completa_encabezado_fijo"  id="contenedorFacturaCXP" oncontextmenu="return false">
            <asp:UpdatePanel ID="upgvFacturas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvFacturas" runat="server" AllowPaging="true" AllowSorting="true"
                        OnPageIndexChanging="gvFacturas_PageIndexChanging" OnSorting="gvFacturas_Sorting" OnRowDataBound="gvFacturas_RowDataBound" 
                        PageSize="25" CssClass="gridview" ShowFooter="true" width="100%" AutoGenerateColumns="false">
                        <AlternatingRowStyle CssClass="gridviewrowalternate" />
                        <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                        <FooterStyle CssClass="gridviewfooter" />
                        <HeaderStyle CssClass="gridviewheader" />
                        <RowStyle CssClass="gridviewrow" />
                        <SelectedRowStyle CssClass="gridviewrowselected" />
                        <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                        <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                        <Columns>                   
                            <asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" />
                            <asp:BoundField DataField="TipoServicio" HeaderText="Tipo de Servicio" SortExpression="TipoServicio" />   
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />  
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />                          
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" Visible="false" />
                            <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>                      
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPendiente" HeaderText="Monto Pendiente" SortExpression="MontoPendiente" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPorAplicar" HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>    
                            <asp:TemplateField HeaderText="Existentes" SortExpression="Facturas Existentes" ItemStyle-Width="60px" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbExitentes" ImageUrl="~/Image/exchange.png" Width="30" Height="30" runat="server" CommandName="Existente"  OnClick="imbsustitucionExistente_Click"></asp:ImageButton>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="Importar Factura" SortExpression="Facturas Nuevas" ItemStyle-Width="60px" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imbNuevas" ImageUrl="~/Image/intercambio.png" Width="30" Height="30" runat="server" CommandName="Nueva"  OnClick="imbsustitucionExistente_Click"></asp:ImageButton>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                            </Columns>
                    </asp:GridView>
                     <asp:HiddenField ID="hfHgvFacturas" runat="server" Value="0" />
                     <asp:HiddenField ID="hfVgvFacturas" runat="server" Value="0" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoFacturas" /> 
                    <asp:AsyncPostBackTrigger ControlID="ucSoporte" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        </div>
    <!-- Ventana Sustitucion Existente -->  
    <div id="contenedorVentanaSustitucionExistente" class="modal">
        <div id="VentanaSustitucionExistente" class="contenedor_ventana_confirmacion_arriba" style="width:700px; height:400px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="UplkbCerrarSustitucionExistente" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:LinkButton ID="lkbCerrarSustitucionExistente" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Existente" >
                <img src="../Image/Cerrar16.png" />
                </asp:LinkButton>
                </ContentTemplate>
                </asp:UpdatePanel>
              </div>
            <div class="header_seccion">
                <img src="../Image/CXPB.png" />
                <h2>Sustitucion Factura Existente</h2>
            </div>
            <div class="contenedor_controles" style=" margin-left: 10px; padding: 0px; width: 95%;">
             <div class="columna2x">
            <asp:Panel ID="pnlBusquedaModal" runat="server" DefaultButton="btnBuscarModal">
                <div class="renglon4x">
                    <div class="etiqueta">
                        <label for="txtFolioModal">Folio</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtFolioModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtFolioModal" runat="server" TabIndex="2" CssClass="textbox_50px validate[custom[onlyNumberSp]]" MaxLength="12"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
 
                    <div class="etiqueta_80px">
                  <label for="txtSerieModal">Serie</label>
                </div>
                <div class="control">
                <asp:TextBox ID="txtSerieModal" runat="server" CssClass="textbox" MaxLength="10" TabIndex="3"></asp:TextBox>
                </div>
                </div>
                <div class="renglon2x">
                    <div class="etiqueta">
                        <label for="txtUUIDModal">UUID</label>
                    </div>
                    <div class="control2x">
                        <asp:UpdatePanel ID="uptxtUUIDModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUUIDModal" runat="server" TabIndex="4" CssClass="textbox2x" MaxLength="36"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon2x">
     
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnBuscarModal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnBuscarModal" runat="server" Text="Buscar" TabIndex="5" CssClass="boton"
                                    OnClick="btnBuscarModal_Click" />
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </asp:Panel>
        </div>
                </div>
            <div class="contenedor_controles" style=" margin-left: 10px; padding: 0px; width: 95%; height:60%;">
            <div class="columna3x">
         
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamanoFI">Mostrar</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoFI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoFI" runat="server" CssClass="dropdown" TabIndex="3" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoFI_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label for="lblOrdenadoFI">Ordenado</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uplblOrdenadoFI" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblOrdenadoFI" runat="server"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplnkExportar" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportar" runat="server" Text="Exportar" TabIndex="4" OnClick="lnkExportar_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_200px_altura" style=" margin-left: 10px; padding: 0px; width:105%;">
                    <asp:UpdatePanel ID="uplblConfirmacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvAnticiposProveedor" runat="server" AllowPaging="true" AllowSorting="true"
                                OnPageIndexChanging="gvAnticiposProveedor_PageIndexChanging" OnSorting="gvAnticiposProveedor_Sorting"
                                PageSize="25" CssClass="gridview" ShowFooter="true" Width="110%" AutoGenerateColumns="false">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                 <asp:BoundField DataField="Id" HeaderText="No. Factura" SortExpression="Id" />
                            <asp:BoundField DataField="TipoServicio" HeaderText="Tipo de Servicio" SortExpression="TipoServicio" />                          
                            <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />                          
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" Visible="false" />
                            <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>                      
                            <asp:BoundField DataField="MontoTotal" HeaderText="Monto Total" SortExpression="MontoTotal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoAplicado" HeaderText="Monto Aplicado" SortExpression="MontoAplicado" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPendiente" HeaderText="Monto Pendiente" SortExpression="MontoPendiente" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoPorAplicar" HeaderText="Monto Por Aplicar" SortExpression="MontoPorAplicar" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}">
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField> 
                                 <asp:TemplateField HeaderText="" SortExpression="Facturas Existentes" ItemStyle-Width="60px" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                <asp:ImageButton ID="imbPruebas" ImageUrl="~/Image/exchange.png" Width="30" Height="30" runat="server" CommandName="Cambio"  OnClick="imbsustitucion_Click" ToolTip="Sustituir"></asp:ImageButton>
                                </ItemTemplate>
                                <HeaderStyle Width="50px" />
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoFI" />
                              <asp:AsyncPostBackTrigger ControlID="btnBuscarModal" />
                               <asp:AsyncPostBackTrigger ControlID="ucSoporte" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                
            </div>

                 </div>
        </div>
    </div>
    <!-- ventana sustitucion nueva -->
    <div id="contenedorVentanaSustitucionNueva" class="modal">
        <div id="VentanaSustitucionNueva" class="contenedor_ventana_confirmacion_arriba" style=" min-width: 330px;">

            <div class="columna2x">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="UplkbCerrarSustitucionNueva" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:LinkButton ID="lkbCerrarSustitucionNueva" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Nueva" >
                <img src="../Image/Cerrar16.png" />
                </asp:LinkButton>
                </ContentTemplate>
                </asp:UpdatePanel>
              </div>
            <div class="header_seccion">
                <img src="../Image/CXPI.png" />
                <h2>Sustitucion</h2>
            </div>


       
         <asp:Panel ID="PanelDetalleFactura" runat="server" Visible="false" >
        <div class="columna">
            <div class="renglon">
                <div class="etiqueta">
                    <label for="txtCompania">ID</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uplblId" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblId" runat="server" ></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
  
                      <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
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
                                TabIndex="11" MaxLength="9" Enabled="false"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnImportar" />
               <asp:AsyncPostBackTrigger ControlID="btnAceptarValidacion" />
                            
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
     </asp:Panel>

         <div class="columna" style="width:95%">
            <div id="contenedorFacturaXML" >
                <div id="nombreContenedor">Arrastre y suelte sus archivos XML a este cuadro.</div>
            </div>
  
        </div>

         <div class="renglon">
             <div class="controlBoton">
                    <asp:UpdatePanel ID="upbtnImportar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnImportar" runat="server" Text="Importar Factura" TabIndex="1" CssClass="boton"
                                OnClick="btnImportar_Click" />
                        </ContentTemplate>
                        <Triggers>
                     
                        </Triggers>
                    </asp:UpdatePanel>
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
        <!-- Ventana Confirmación Soporte-->
       <div id="contenidoSoporteTecnicoModal" class="modal">
        <div id="contenidoSoporteTecnico" class="contenedor_ventana_confirmacion_arriba" style="min-width:500px;padding-bottom:20px;">
    
<div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="UplkbCerrarSoporteTecnico" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:LinkButton ID="lkbCerrarSoporteTecnico" runat="server" OnClick="lkbCerrarVentanaModal_Click" CommandName="Soporte" >
                <img src="../Image/Cerrar16.png" />
                </asp:LinkButton>
                </ContentTemplate>
                </asp:UpdatePanel>
              </div>
<asp:UpdatePanel ID="upucSoporte" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<tectos:wucSoporteTecnico ID="ucSoporte" runat="server" TabIndex="3" OnClickCancelarSoporte="wucSoporteTecnico_ClickCancelarSoporte" OnClickGuardarSoporte="wucSoporteTecnico_ClickAceptarSoporte" />
</ContentTemplate>
<Triggers>
 <asp:AsyncPostBackTrigger ControlID="gvAnticiposProveedor"/> 
    <asp:AsyncPostBackTrigger ControlID="gvFacturas"/> 

</Triggers>
</asp:UpdatePanel>
</div>
</div>
</asp:Content>
