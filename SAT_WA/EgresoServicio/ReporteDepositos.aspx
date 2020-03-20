<%@ Page Title="Reporte de Depósitos" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage/MasterPage.Master" CodeBehind="ReporteDepositos.aspx.cs" Inherits="SAT.EgresoServicio.ReporteDepositos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Estilos Autocomplete, Mascara y Validadores JQuery -->
    <link href="../CSS/jquery.validationEngine.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <style>
        #box {
            width: 250px;
            height: 50px;
            text-align: center;
            vertical-align: -webkit-baseline-middle;
            border: 2px solid #04B404;
            background-color: #00FF00;
            padding: 15px;
            font-family: Arial;
            font-size: 16px;
            margin-top: 35px;
        }
    </style>
    <!-- Bibliotecas para Validación de formulario -->
    <script type="text/javascript" src="../Scripts/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.validationEngine.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de datetime picker -->
    <script type="text/javascript" src="../Scripts/jquery.datetimepicker.js" charset="utf-8"></script>
    <!-- Biblioteca para uso de carga de Archivos XML -->
    <script type="text/javascript" src="../Scripts/modernizr-2.5.3.js"></script>
    <script type="text/javascript" src="../Scripts/gridviewScroll.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content1" runat="server">
    <script type="text/javascript">
        //Obteniendo instancia actual de la página y añadiendo manejador de evento
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        //Manejador de evento de termino de petición web (Permite reasignación de scripts después de actualizaciones parciales)
        function EndRequestHandler(sender, args) {
            if (args.get_error() == undefined) {
                ConfiguraJQueryReporteDepositos();
            }
        }
        //Creando función para configuración de jquery en control de usuario
        function ConfiguraJQueryReporteDepositos() {
            $(document).ready(function () {

                //Añadiendo Encabezado Fijo
                $("#<%=gvDepositos.ClientID%>").gridviewScroll({
              width: document.getElementById("contenedorReporteDepositos").offsetWidth - 15,
              height: 400
          });


          $("#<%=txtUnidad.ClientID%>").autocomplete({
              source: '../WebHandlers/AutoCompleta.ashx?id=12&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>',
          select: function (event, ui) {

              //Asignando Selección al Valor del Control
              $("#<%=txtUnidad.ClientID%>").val(ui.item.value);

          //Causando Actualización del Control
          __doPostBack('<%= txtUnidad.UniqueID %>', '');
          }
      });

          $("#<%=txtOperador.ClientID%>").autocomplete({
              source: '../WebHandlers/AutoCompleta.ashx?id=11&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
      });

          $("#<%=txtTercero.ClientID%>").autocomplete({
              source: '../WebHandlers/AutoCompleta.ashx?id=17&param=<%=((SAT_CL.Seguridad.UsuarioSesion)Session["usuario_sesion"]).id_compania_emisor_receptor.ToString()%>'
      });


          //Validación 
          var validacionReporteDepositos = function () {

              var isValidP1 = !$("#<%=txtNoDeposito.ClientID%>").validationEngine('validate');
          var isValidP2 = !$("#<%=txtUnidad.ClientID%>").validationEngine('validate');
          var isValidP3 = !$("#<%=txtOperador.ClientID%>").validationEngine('validate');
          var isValidP4 = !$("#<%=txtTercero.ClientID%>").validationEngine('validate');
          var isValidP5 = !$("#<%=txtIdentificador.ClientID%>").validationEngine('validate');
          var isValidP6 = !$("#<%=txtNoServicio.ClientID%>").validationEngine('validate');
          var isValidP7 = !$("#<%=txtFechaInicio.ClientID%>").validationEngine('validate');
          var isValidP8 = !$("#<%=txtFechaFin.ClientID%>").validationEngine('validate');
          var isValidP9 = !$("#<%=txtFechaInicioD.ClientID%>").validationEngine('validate');
          var isValidP10 = !$("#<%=txtFechaFinD.ClientID%>").validationEngine('validate');
          var isValidP11 = !$("#<%=txtCartaPorte.ClientID%>").validationEngine('validate');
          var isValidP12 = !$("#<%=txtReferenciaViaje.ClientID%>").validationEngine('validate');
              return isValidP1 && isValidP2 && isValidP3 && isValidP4 && isValidP5 && isValidP6 && isValidP7 && isValidP8 && isValidP9 && isValidP10 && isValidP11 && isValidP12;
          };
          //Validación de campos requeridos
          $("#<%=this.btnBuscar.ClientID%>").click(validacionReporteDepositos);

          // *** Fecha de inicio, fin de Registro (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
          $("#<%=txtFechaInicio.ClientID%>").datetimepicker({
              lang: 'es',
              format: 'd/m/Y H:i'
          });
          $("#<%=txtFechaFin.ClientID%>").datetimepicker({
              lang: 'es',
              format: 'd/m/Y H:i'
          });
          // *** Fecha de inicio, fin de Deposito (Idioma: Español, Formato: 'dd:MM:aaaa HH:mm') *** //
          $("#<%=txtFechaInicioD.ClientID%>").datetimepicker({
              lang: 'es',
              format: 'd/m/Y H:i'
          });
          $("#<%=txtFechaFinD.ClientID%>").datetimepicker({
              lang: 'es',
              format: 'd/m/Y H:i'
          });

      });
        }

        /**Script Contenedor de Archivos**/
        //Declarando variable contenedora de Archivos
        var selectedFiles;
        //Función que limpia el Contenedor
        function LimpiaContenedorXML() {
            //Limpiando DIV
            selectedFiles = null;
            $("#box").text("Arrastre y Suelte sus archivos desde su maquina en este cuadro.");
        }

        //Inicializando Función
        $(document).ready(function () {
            //validando el Tipo de Archivo
            if (!Modernizr.draganddrop) {
                alert("This browser doesn't support File API and Drag & Drop features of HTML5!");
                return;
            }
            //Declarando Objeto contenedor del DIV
            var box;
            box = document.getElementById("box");
            //Añadiendo Eventos
            box.addEventListener("dragenter", OnDragEnter, false);
            box.addEventListener("dragover", OnDragOver, false);
            box.addEventListener("drop", OnDrop, false);

            //Función cuando se Arrastra el Objeto dentro del limite
            function OnDragEnter(e) {
                e.stopPropagation();
                e.preventDefault();
            }
            //Función cuando se Arrastra el Objeto fuera del limite
            function OnDragOver(e) {
                e.stopPropagation();
                e.preventDefault();
            }
            //Función cuando se Suelta el Objeto dentro del limite
            function OnDrop(e) {
                e.stopPropagation();
                e.preventDefault();

                selectedFiles = null;
                selectedFiles = e.dataTransfer.files;
                //Declarando Objeto de Lectura
                var lector = new FileReader();
                //Evento al Cargar el Archivo
                lector.onload = function (evt) {
                    //Obteniendo Archivo
                    var bytes = evt.target.result;
                    //Invocando Método Web para Obtención de Archivos
                    PageMethods.ArchivoSesion(evt.target.result, selectedFiles[0].name, function (r) { }, function (e) { alert('Error Invocacion MW ' + e); }, this);
                };
                //Evento al Producirse un Error
                lector.onerror = function (evt) {
                    alert('Error Carga ' + evt.target.error);

                };
                //Leyendo Texto
                lector.readAsText(selectedFiles[0]);
                //Mostrando mensaje
                alert('El Archivo se ha Cargado');
                //Indicando Archivo
                $("#box").text("El Archivo " + selectedFiles[0].name + " ha sido Cargado con exito");
            }

        });

        //Invocación Inicial de método de configuración JQuery
        ConfiguraJQueryReporteDepositos();
    </script>
    <div id="encabezado_forma">
        <h1>Reporte Depósitos</h1>
    </div>
    <div class="seccion_controles" style="float: left; width: calc(50% - 25px);">
        <div class="header_seccion">
            <img src="../Image/Buscar.png" />
            <h2>Buscar depósitos por</h2>
        </div>
        <div class="columna2x">
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoDeposito">No Depósito</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoDeposito" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoDeposito" runat="server" CssClass="textbox validate[custom[onlyNumberSp]]" TabIndex="1" MaxLength="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
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
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="dropdown" TabIndex="2"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtNoServicio">No Servicio</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtNoServicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNoServicio" runat="server" CssClass="textbox2x" TabIndex="3" MaxLength="30"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtReferenciaViaje">No. Viaje</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtReferenciaViaje" runat="server" CssClass="textbox2x" TabIndex="4" MaxLength="500"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtCartaPorte">Carta Porte</label>
                </div>
                <div class="control">
                    <asp:TextBox ID="txtCartaPorte" runat="server" CssClass="textbox2x" TabIndex="5" MaxLength="500"></asp:TextBox>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtUnidad">Unidad</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtUnidad" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:TextBox ID="txtUnidad" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="6"
                                OnTextChanged="txtUnidad_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtOperador">Operador</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtOperador" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOperador" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="7"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtUnidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtTercero">Tercero</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtTercero" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtTercero" runat="server" CssClass="textbox2x validate[custom[IdCatalogo]]" TabIndex="8"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label for="txtIdentificador">Identificador</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtIdentificador" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdentificador" runat="server" CssClass="textbox2x " MaxLength="150" TabIndex="9"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Efectivo</label>
                </div>
                <div class="control2x">
                    <asp:RadioButton runat="server" ID="rdbTodos" AutoPostBack="false" Text="Todos" GroupName="Efectivo"
                        TabIndex="10" />

                    <asp:RadioButton runat="server" ID="rdbEfectivo" AutoPostBack="false" Text="Efectivo"
                        CssClass="Label" GroupName="Efectivo" TabIndex="11" />


                    <asp:RadioButton runat="server" ID="rdbTranseferencia" AutoPostBack="false" Text="Transferencia"
                        CssClass="Label" GroupName="Efectivo" TabIndex="12" />
                </div>

            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label>Comprobacion</label>
                </div>
                <div class="control2x">
                    <asp:RadioButton runat="server" ID="rdbTodosComprobacion" AutoPostBack="false" Text="Todos" GroupName="Comprobacion"
                        TabIndex="13" />

                    <asp:RadioButton runat="server" ID="rdbSi" AutoPostBack="false" Text="Si" GroupName="Comprobacion"
                        TabIndex="14" />

                    <asp:RadioButton runat="server" ID="rdbNo" AutoPostBack="false" Text="No"
                        CssClass="Label" GroupName="Comprobacion" TabIndex="15" />

                </div>
            </div>

            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="upchkRangoFechas" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkRangoFechas" runat="server" Text="Filtrar por fechas de Solicitud."
                                Checked="false" TabIndex="16" AutoPostBack="true" OnCheckedChanged="chkRangoFechas_CheckedChanged" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaInicio">Fecha Inicial</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicio" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicio" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24], past[ctl00_content1_txtFechaFin]]" TabIndex="17"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaFin">Fecha Final</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFin" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFin" runat="server" Enabled="false" CssClass="textbox validate[required, custom[dateTime24], future[ctl00_content1_txtFechaFin]]" TabIndex="18"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechas" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="control2x">
                    <asp:UpdatePanel ID="upchkRangoFechasD" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkRangoFechasD" runat="server" Text="Filtrar por fechas de Depósito."
                                Checked="false" TabIndex="19" AutoPostBack="true" OnCheckedChanged="chkRangoFechasD_CheckedChanged" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaInicioD">Fecha Inicial</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaInicioD" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaInicioD" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="20"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechasD" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="Label" for="txtFechaFin">Fecha Final</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="uptxtFechaFinD" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtFechaFinD" Enabled="false" runat="server" CssClass="textbox validate[required, custom[dateTime24]]" TabIndex="21"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="chkRangoFechasD" EventName="CheckedChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <div class="etiqueta">
                    <label class="label">Concepto</label>
                </div>
                <div class="control">
                    <asp:UpdatePanel ID="upddlConceptoDeposito" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlConceptoDeposito" runat="server" CssClass="dropdown" TabIndex="22"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="renglon2x">
                <asp:UpdatePanel ID="uplblError" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="controlBoton">
                <asp:UpdatePanel ID="upbtnBuscar" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="boton" Text="Buscar" TabIndex="23" OnClick="btnBuscar_Click" />
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="contenido_resumen_visor">
        <div class="header_seccion">
            <img src="../Image/ResumenReporte.png" />
            <h2>Resumen por estatus</h2>
        </div>
        <div class="grafica_resumen_visor">
            <asp:UpdatePanel ID="upchart" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Chart ID="ChtDepositos" runat="server" TabIndex="24" BackColor="Transparent">
                        <Legends>
                            <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                            </asp:Legend>
                        </Legends>
                    </asp:Chart>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div class="grid_resumen_visor">
            <asp:UpdatePanel ID="upgvResumen" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvResumen" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        TabIndex="25" ShowFooter="True" CssClass="gridview"
                        PageSize="5" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                            <asp:TemplateField HeaderText="Liquidaciones" SortExpression="Total">
                                <ItemTemplate>
                                    <asp:Label ID="lbldetalles" Text='<%# Eval("Total") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblSuma" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
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
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="contenedor_seccion_completa">
        <div class="header_seccion">
            <h2>Depósitos</h2>
        </div>
        <div class="renglon3x">
            <div class="control">
                <asp:UpdatePanel ID="upddlTamanoDepositos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label for="ddlTamanoDepositos"></label>
                        <asp:DropDownList ID="ddlTamanoDepositos" runat="server" OnSelectedIndexChanged="ddlTamanoDepositos_SelectedIndexChanged" TabIndex="26" AutoPostBack="true" CssClass="dropdown">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <label for="lblOrdenarDepositos">Ordenado Por:</label>
            </div>
            <div class="etiqueta_155px">
                <asp:UpdatePanel ID="uplblOrdenarDepositos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblOrdenarDepositos" runat="server"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvDepositos" EventName="Sorting" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="etiqueta">
                <asp:UpdatePanel runat="server" ID="uplkbExportarDepositos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbExportarDepositos" runat="server" Text="Exportar Excel" CommandName="Depositos" OnClick="lkbExportar_Click" TabIndex="27"></asp:LinkButton>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lkbExportarDepositos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="grid_seccion_completa_altura_variable" id="contenedorReporteDepositos">
            <asp:UpdatePanel ID="upgvDepositos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvDepositos" runat="server" AllowPaging="True" OnPageIndexChanging="gvDepositos_PageIndexChanging" AllowSorting="True"
                        OnSorting="gvDepositos_Sorting" AutoGenerateColumns="False" TabIndex="28"
                        ShowFooter="True"
                        PageSize="25">
                        <Columns>
                            <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
                                <ItemTemplate>
                                    <asp:Label ID="lblConcepto" runat="server" ToolTip='<%# Eval("Concepto") %>'
                                        Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Concepto").ToString(), 25, "...") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus"
                                ItemStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" DataFormatString="{0:c2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MontoComprobacion" HeaderText="Monto Comp." SortExpression="MontoComprobacion" DataFormatString="{0:c2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Comprobante" HeaderText="Comprobante" SortExpression="Comprobante">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Operador" HeaderText="Operador" SortExpression="Operador" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                            <asp:BoundField DataField="Tercero" HeaderText="Tercero" SortExpression="Tercero" />
                            <asp:BoundField DataField="Servicio" HeaderText="Servicio" SortExpression="Servicio">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CartaPorte" HeaderText="Carta Porte" SortExpression="CartaPorte" />
                            <asp:BoundField DataField="NoViaje" HeaderText="No. Viaje" SortExpression="NoViaje" />
                            <asp:TemplateField HeaderText="Movimiento" SortExpression="Movimiento">
                                <ItemTemplate>
                                    <asp:Label ID="lblMovimiento" runat="server" ToolTip='<%# Eval("Movimiento") %>'
                                        Text='<%# TSDK.Base.Cadena.TruncaCadena(Eval("Movimiento").ToString(), 50, "...") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" SortExpression="Cliente" />
                            <asp:BoundField DataField="Efectivo" HeaderText="Efectivo" SortExpression="Efectivo">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaSolicitud" HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Solicitadopor" HeaderText="Solicitado por" SortExpression="Solicitadopor" />
                            <asp:BoundField DataField="Programadopor" HeaderText="Programado por" SortExpression="Programadopor" />
                            <asp:BoundField DataField="FechaProgramado" HeaderText="Fecha Programado" SortExpression="FechaProgramado"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaAutorizacion" HeaderText="Fecha Autorización" SortExpression="FechaAutorizacion"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Autorizadopor" HeaderText="Autorizado por" SortExpression="Autorizadopor" />
                            <asp:BoundField DataField="TiempoEsperaAutorizacion" HeaderText="Tiempo Autorización" SortExpression="*TiempoEsperaAutorizacion">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaDeposito" HeaderText="Fecha depósito" SortExpression="FechaDeposito"
                                DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Depositadopor" HeaderText="Depositado por" SortExpression="Depositadopor" />
                            <asp:BoundField DataField="RefBancaria" HeaderText="Ref. Bancaria" SortExpression="RefBancaria" />
                            <asp:BoundField DataField="TiempoEsperaDeposito" HeaderText="Tiempo Depositó" SortExpression="*TiempoEsperaDeposito" />
                            <asp:BoundField DataField="NoLiquidacion" HeaderText="No Liq." SortExpression="NoLiquidacion">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaLiquidacion" HeaderText="Fecha Liq." SortExpression="FechaLiquidacion" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:TemplateField HeaderText="Monto Facturas Proveedor" SortExpression="MontoFacturasProveedor">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkFacturasProveedor" runat="server" Text='<%# string.Format("{0:C2}", Eval("MontoFacturasProveedor")) %>' OnClick="lnkFacturasProveedor_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FacturasProveedor" HeaderText="Facturas Proveedor" SortExpression="FacturasProveedor" />
                            <asp:TemplateField HeaderText="Referencia" SortExpression="Referencia">
                                <ItemTemplate>
                                    <asp:Label ID="lblReferencia" runat="server" ToolTip='<%# Eval("Referencia") %>'
                                        Text='<%#TSDK.Base.Cadena.TruncaCadena(Eval("Referencia").ToString(), 25, "...") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HERRAMIENTAS">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="uplkbBitacora" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lkbBitacora" Text="Bitácora" runat="server" OnClick="OnClik_Bitacora"></asp:LinkButton>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="lkbBitacora" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbEliminarDeposito" runat="server" Text="Eliminar" OnClick="lkbDeposito_Click" CommandName="Eliminar"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbVerComprobacion" runat="server" Text="Ver Comprobaciones" OnClick="lkbDeposito_Click" CommandName="VerComprobacion"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lkbAltaComprobacion" runat="server" Text="Alta Comprobación" OnClick="lkbDeposito_Click" CommandName="AltaComprobacion"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
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
                    <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                    <asp:AsyncPostBackTrigger ControlID="ddlTamanoDepositos" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarAltaComprobacion" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCerrarComprobacion" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <!-- Ventana Comprobaciones -->
    <div id="contenedorVentanaComprobaciones" class="modal">
        <div id="ventanaComprobaciones" class="contenedor_ventana_confirmacion_arriba" style="width: 400px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarComprobacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarComprobacion" runat="server" CommandName="Comprobaciones" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/DepositosVale.png" />
                <h2>Comprobaciones Realizadas</h2>
            </div>
            <div class="renglon2x">
                <div class="etiqueta_50px">
                    <label for="ddlTamanoComp">Mostrar</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="upddlTamanoComp" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTamanoComp" runat="server" OnSelectedIndexChanged="ddlTamanoComp_SelectedIndexChanged"
                                CssClass="dropdown_100px" AutoPostBack="true">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <label for="lblOrdenadoComp">Ordenado</label>
                </div>
                <div class="control_100px">
                    <asp:UpdatePanel ID="uplblOrdenadoComp" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <b>
                                <asp:Label ID="lblOrdenadoComp" runat="server"></asp:Label></b>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" EventName="Sorting" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="etiqueta_50px">
                    <asp:UpdatePanel ID="uplnkExportarComp" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:LinkButton ID="lnkExportarComp" runat="server" Text="Exportar" CommandName="Comprobaciones" OnClick="lkbExportar_Click"></asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkExportarComp" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="grid_seccion_completa_200px_altura">
                <asp:UpdatePanel ID="upgvComprobaciones" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvComprobaciones" runat="server" AllowPaging="true" AllowSorting="true"
                            CssClass="gridview" ShowFooter="true" TabIndex="27" OnSorting="gvComprobaciones_Sorting"
                            OnPageIndexChanging="gvComprobaciones_PageIndexChanging" AutoGenerateColumns="false" Width="100%">
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
                                <asp:BoundField DataField="NoMov" HeaderText="No. Movimiento" SortExpression="NoMov" />
                                <asp:BoundField DataField="Concepto" HeaderText="Concepto" SortExpression="Concepto" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" SortExpression="Observacion" />
                                <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" />
                                <asp:BoundField DataField="NoFacturas" HeaderText="No. Facturas" SortExpression="NoFacturas" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEditaComp" runat="server" Text="Editar"
                                            OnClick="lnkEditaComp_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEliminaComp" runat="server" Text="Eliminar"
                                            OnClick="lnkEliminaComp_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Ventana Alta Comprobaciones -->
    <div id="contenedorVentanaAltaComprobaciones" class="modal">
        <div id="ventanaAltaComprobaciones" style="z-index: 1002; display: none; position: fixed; background-color: #FFF; border: 1px solid #808080; left: 300px; top: 200px; width: 1000px; height: auto; padding: 10px;">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel runat="server" ID="uplnkCerrarAltaComprobacion" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarAltaComprobacion" runat="server" CommandName="AltaComprobaciones" OnClick="lnkCerrar_Click" Text="Cerrar">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <h2>Comprobaciones</h2>
            </div>
            <div class="columna">
                <div class="renglon">
                    <div class="etiqueta">
                        <asp:UpdatePanel ID="uplblIdComprobacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblIdComprobacion" runat="server" Text="Por Asignar"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="ddlConcepto">Concepto</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlConcepto" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlConcepto" runat="server" CssClass="dropdown" TabIndex="46"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtObservacion">Observación</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtObservacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtObservacion" runat="server" TabIndex="47" CssClass="textbox validate[required]" MaxLength="255"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="etiqueta">
                        <label for="txtValorUnitario">Monto</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="uptxtValorUnitario" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtValorUnitario" runat="server" TabIndex="48" CssClass="textbox validate[required, custom[positiveNumber]]" MaxLength="15"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnGuardarComprobacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnGuardarComprobacion" runat="server" Text="Aceptar" CssClass="boton"
                                    OnClick="btnGuardarComprobacion_Click" TabIndex="49" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnCancelarComprobacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnCancelarComprobacion" runat="server" Text="Cancelar" CssClass="boton_cancelar"
                                    OnClick="btnCancelarComprobacion_Click" TabIndex="50" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                                <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="renglon">
                    <asp:UpdatePanel ID="uplblErrorComprobacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblErrorComprobacion" runat="server" CssClass="label_error"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                            <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                            <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="columna">
                <div id="box">Arrastre y Suelte sus archivos desde su maquina en este cuadro.</div>
                <div class="renglon">
                    <div class="controlBoton">
                        <asp:UpdatePanel ID="upbtnAgregarFactura" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnAgregarFactura" runat="server" Text="Agregar Factura" CssClass="boton"
                                    OnClick="btnAgregarFactura_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                                <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                                <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="columna">
                <div class="renglon">
                    <div class="etiqueta_50px">
                        <label for="ddlTamanoFacComp">Mostrar:</label>
                    </div>
                    <div class="control_60px">
                        <asp:UpdatePanel ID="upddlTamanoFacComp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoFacComp" runat="server" CssClass="textbox_50px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoFacComp_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <label for="">Ordenado:</label>
                    </div>
                    <div class="control_60px">
                        <asp:UpdatePanel ID="uplblOrdenadoFacComp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblOrdenadoFacComp" runat="server"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasComprobacion" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50px">
                        <asp:UpdatePanel ID="uplnkExportarFacComp" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarFacComp" runat="server" Text="Exportar" CommandName="FacturasComp"
                                    OnClick="lkbExportar_Click"></asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarFacComp" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_100px_altura">
                    <asp:UpdatePanel ID="upgvFacturasComprobacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvFacturasComprobacion" runat="server" AllowPaging="true" AllowSorting="true"
                                CssClass="gridview" ShowFooter="true" TabIndex="54" OnSorting="gvFacturasComprobacion_Sorting"
                                OnPageIndexChanging="gvFacturasComprobacion_PageIndexChanging" AutoGenerateColumns="false">
                                <AlternatingRowStyle CssClass="gridviewrowalternate" Width="70%" />
                                <EmptyDataRowStyle BackColor="#ffffff" ForeColor="#ff0000" />
                                <FooterStyle CssClass="gridviewfooter" />
                                <HeaderStyle CssClass="gridviewheader" />
                                <RowStyle CssClass="gridviewrow" />
                                <SelectedRowStyle CssClass="gridviewrowselected" />
                                <SortedAscendingCellStyle CssClass="gridviewcellsortASC" />
                                <SortedDescendingCellStyle CssClass="gridviewcellsortDESC" />
                                <Columns>
                                    <asp:BoundField DataField="IdFactura" HeaderText="Id" SortExpression="IdFactura" Visible="false" />
                                    <asp:BoundField DataField="IdCompFact" HeaderText="IdComp" SortExpression="IdCompFact" Visible="false" />
                                    <asp:BoundField DataField="SerieFolio" HeaderText="Serie-Folio" SortExpression="SerieFolio" />
                                    <asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="Sub Total" SortExpression="Subtotal" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                                    <asp:BoundField DataField="Trasladado" HeaderText="IVA Trasladado" SortExpression="Trasladado" />
                                    <asp:BoundField DataField="Retenido" HeaderText="IVA Retenido" SortExpression="Retenido" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEliminarFactura" runat="server" Text="Eilminar"
                                                OnClick="lnkEliminarFactura_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancelarComprobacion" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                            <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                            <asp:AsyncPostBackTrigger ControlID="gvComprobaciones" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <!-- Ventana de Insercción y Vizualización de Facturas Ligadas -->
    <div id="contenedorVentanaFacturasLigadas" class="modal">
        <div id="ventanaFacturasLigadas" class="contenedor_ventana_confirmacion_arriba">
            <div class="boton_cerrar_modal">
                <asp:UpdatePanel ID="uplnkCerrarVentanaFL" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton ID="lnkCerrarVentanaFL" runat="server" CommandName="FacturasLigadas" Text="Cerrar" OnClick="lnkCerrarVentana_Click">
<img src="../Image/Cerrar16.png" />
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="header_seccion">
                <img src="../Image/FacturacionCargos.png" />
                <h2>Facturas Ligadas</h2>
            </div>
            <div class="columna3x">
                <div class="renglon3x">
                    <div class="etiqueta">
                        <label for="ddlTamanoFL">Mostrar:</label>
                    </div>
                    <div class="control">
                        <asp:UpdatePanel ID="upddlTamanoFL" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTamanoFL" CssClass="dropdown" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlTamanoFL_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta">
                        <label>Ordenado:</label>
                    </div>
                    <div class="etiqueta_155px">
                        <asp:UpdatePanel ID="uplblOrdenadoGrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <b>
                                    <asp:Label ID="lblOrdenadoGrid" runat="server"></asp:Label></b>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvFacturasLigadas" EventName="Sorting" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="etiqueta_50pxr">
                        <asp:UpdatePanel ID="uplnkExportarFL" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkExportarFL" runat="server" OnClick="lnkExportarFL_Click">Exportar</asp:LinkButton>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkExportarFL" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="grid_seccion_completa_150px_altura">
                    <asp:UpdatePanel ID="upgvConceptos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="gvFacturasLigadas" runat="server" AllowPaging="true" AllowSorting="true"
                                CssClass="gridview" OnSorting="gvFacturasLigadas_Sorting" OnPageIndexChanging="gvFacturasLigadas_PageIndexChanging"
                                TabIndex="37" AutoGenerateColumns="false" Width="90%" PageSize="25" ShowFooter="true">
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
                                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" SortExpression="Proveedor" />
                                    <asp:BoundField DataField="FechaFactura" HeaderText="Fecha" SortExpression="FechaFactura" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                    <asp:BoundField DataField="Serie" HeaderText="Serie" SortExpression="Serie" />
                                    <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" />
                                    <asp:BoundField DataField="UUID" HeaderText="UUID" SortExpression="UUID" />
                                    <asp:BoundField DataField="SubTotal" HeaderText="Sub Total" SortExpression="SubTotal" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ImpTrasladado" HeaderText="Importe Trasladado" SortExpression="ImpTrasladado" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ImpRetenido" HeaderText="Importe Retenido" SortExpression="ImpRetenido" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTamanoFL" />
                            <asp:AsyncPostBackTrigger ControlID="gvDepositos" />
                            <asp:AsyncPostBackTrigger ControlID="btnAgregarFactura" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

